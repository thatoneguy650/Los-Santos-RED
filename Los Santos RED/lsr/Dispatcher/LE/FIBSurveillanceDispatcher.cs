using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

public class FIBSurveillanceDispatcher
{
    private readonly IEntityProvideable World;
    private readonly IDispatchable Player;
    private readonly IAgencies Agencies;
    private readonly ISettingsProvideable Settings;
    private readonly IWeapons Weapons;
    private readonly INameProvideable Names;
    private readonly IModItems ModItems;
    private readonly IShopMenus ShopMenus;
    private readonly ICrimes Crimes;
    private readonly Dictionary<string, RuntimeTeam> RuntimeTeams = new Dictionary<string, RuntimeTeam>();
    private uint LastUpdateGameTime;
    private bool HasInitializedRuntime;
    private LawEnforcementIntelligence CurrentRuntimeIntelligence;

    private class RuntimeTeam
    {
        public string AssignmentID;
        public PedExt Driver;
        public VehicleExt Vehicle;
        public uint LastFollowCommandAt;
        public uint LastDetectionCheckAt;
        public bool DisengageCommandIssued;
    }

    public FIBSurveillanceDispatcher(IEntityProvideable world, IDispatchable player, IAgencies agencies,
        ISettingsProvideable settings, IWeapons weapons, INameProvideable names, IModItems modItems,
        IShopMenus shopMenus, ICrimes crimes)
    {
        World = world;
        Player = player;
        Agencies = agencies;
        Settings = settings;
        Weapons = weapons;
        Names = names;
        ModItems = modItems;
        ShopMenus = shopMenus;
        Crimes = crimes;
    }

    public void Update()
    {
        Mod.Player modPlayer = Player as Mod.Player;
        if (modPlayer == null || modPlayer.LawEnforcementIntelligence == null ||
            Settings?.SettingsManager?.LawEnforcementIntelligenceSettings == null)
            return;
        LawEnforcementIntelligenceSettings settings = Settings.SettingsManager.LawEnforcementIntelligenceSettings;
        settings.Normalize();

        if (Game.GameTime - LastUpdateGameTime < 1000)
            return;
        LastUpdateGameTime = Game.GameTime;

        LawEnforcementIntelligence intelligence = modPlayer.LawEnforcementIntelligence;
        intelligence.Normalize();
        if (!ReferenceEquals(CurrentRuntimeIntelligence, intelligence))
        {
            foreach (RuntimeTeam oldTeam in RuntimeTeams.Values.ToList())
                CleanupTeam(oldTeam);
            RuntimeTeams.Clear();
            CurrentRuntimeIntelligence = intelligence;
            HasInitializedRuntime = false;
        }
        if (!HasInitializedRuntime)
        {
            HasInitializedRuntime = true;
            foreach (SurveillanceAssignment savedAssignment in intelligence.SurveillanceAssignments.Where(x =>
                x != null && x.State != SurveillanceAssignmentState.Available &&
                x.State != SurveillanceAssignmentState.Completed))
            {
                // Runtime entities are deliberately not restored from saves. Reacquire with a new team.
                savedAssignment.State = SurveillanceAssignmentState.LostContact;
                savedAssignment.LastStateChangedAt = modPlayer.CurrentDateTime;
                savedAssignment.ReplacementRequired = true;
            }
        }
        if (!settings.EnableFIBSurveillance || settings.MaxActiveFIBTeams <= 0)
        {
            DisengageAll(intelligence, modPlayer.CurrentDateTime);
            return;
        }

        UpdateRuntimeTeams(intelligence, modPlayer.CurrentDateTime, settings);
        AssignEligibleInvestigations(intelligence, modPlayer.CurrentDateTime, settings);
    }

    public void Dispose()
    {
        Mod.Player modPlayer = Player as Mod.Player;
        if (modPlayer != null && modPlayer.LawEnforcementIntelligence != null)
            DisengageAll(modPlayer.LawEnforcementIntelligence, modPlayer.CurrentDateTime);
        else
        {
            foreach (RuntimeTeam team in RuntimeTeams.Values.ToList())
                CleanupTeam(team);
            RuntimeTeams.Clear();
        }
        CurrentRuntimeIntelligence = null;
        HasInitializedRuntime = false;
    }

    private void AssignEligibleInvestigations(LawEnforcementIntelligence intelligence, DateTime now,
        LawEnforcementIntelligenceSettings settings)
    {
        int activeCount = intelligence.SurveillanceAssignments.Count(x => x != null &&
            x.State != SurveillanceAssignmentState.Available &&
            x.State != SurveillanceAssignmentState.Completed);
        if (activeCount >= settings.MaxActiveFIBTeams)
            return;

        List<InvestigationRecord> candidates = intelligence.Investigations
            .Where(x => x != null && x.Stage >= InvestigationStage.Focused &&
                x.Stage != InvestigationStage.Suspended && x.Stage != InvestigationStage.Closed)
            .OrderByDescending(x => x.Stage)
            .ThenByDescending(x => x.LastActivityAt)
            .ToList();

        foreach (InvestigationRecord investigation in candidates)
        {
            if (activeCount >= settings.MaxActiveFIBTeams)
                break;
            bool alreadyAssigned = intelligence.SurveillanceAssignments.Any(x => x != null &&
                x.InvestigationID == investigation.ID &&
                x.State != SurveillanceAssignmentState.Available &&
                x.State != SurveillanceAssignmentState.Completed);
            if (alreadyAssigned)
                continue;

            string teamID = Guid.NewGuid().ToString("N");
            RuntimeTeam team = SpawnTeam(teamID, settings);
            if (team == null)
                continue;

            if (!((Player as Mod.Player)?.LawEnforcementIntelligenceManager?.TryAssignSurveillance(
                intelligence, investigation.ID, teamID, now, settings) ?? false))
            {
                CleanupTeam(team);
                continue;
            }

            SurveillanceAssignment assignment = intelligence.SurveillanceAssignments
                .LastOrDefault(x => x != null && x.TeamID == teamID);
            if (assignment == null)
            {
                CleanupTeam(team);
                continue;
            }
            team.AssignmentID = assignment.ID;
            RuntimeTeams[assignment.ID] = team;
            (Player as Mod.Player)?.LawEnforcementIntelligenceManager?.SurveillanceAssignmentManager?
                .Transition(assignment, SurveillanceAssignmentState.Approaching, now);
            activeCount++;
        }
    }

    private RuntimeTeam SpawnTeam(string teamID, LawEnforcementIntelligenceSettings settings)
    {
        Agency fib = Agencies?.GetAgency("FIB");
        if (fib == null || fib.Vehicles == null || fib.Personnel == null)
            return null;

        // Do not silently turn marked patrol vehicles into covert surveillance assets.
        // Only use FIB fleet entries whose configuration explicitly identifies them as FIB,
        // unmarked, undercover or surveillance vehicles.
        List<DispatchableVehicle> candidates = fib.Vehicles.Where(x => x != null && x.IsCar &&
            !x.IsMotorcycle && IsConfiguredUnmarkedVehicle(x)).ToList();
        DispatchablePerson agent = fib.Personnel.FirstOrDefault(x => x != null && !x.IsAnimal);
        if (candidates.Count == 0 || agent == null)
        {
            EntryPoint.WriteToConsole("FIB surveillance unavailable: no configured undercover vehicle or agent.", 1);
            return null;
        }

        DispatchableVehicle vehicleType = candidates[RandomItems.MyRand.Next(0, candidates.Count)];
        for (int attempt = 0; attempt < 8; attempt++)
        {
            float angle = RandomItems.MyRand.Next(0, 360) * (float)Math.PI / 180f;
            float distance = settings.SurveillanceStartDistance +
                RandomItems.MyRand.Next(50, Math.Max(51, (int)(settings.SurveillanceStartDistance * 0.5f)));
            Vector3 playerPosition = Player.Position;
            Vector3 candidatePosition = new Vector3(
                playerPosition.X + (float)Math.Cos(angle) * distance,
                playerPosition.Y + (float)Math.Sin(angle) * distance,
                playerPosition.Z);

            SpawnLocation spawnLocation = new SpawnLocation(candidatePosition);
            spawnLocation.GetClosestStreet(false);
            if (!spawnLocation.HasSpawns)
                continue;

            float spawnDistance = Distance2D(spawnLocation.StreetPosition, playerPosition);
            if (spawnDistance < settings.SurveillanceStartDistance * 0.8f)
                continue;

            LESpawnTask spawnTask = new LESpawnTask(fib, spawnLocation, vehicleType, agent, false,
                Settings, Weapons, Names, false, World, ModItems, false, ShopMenus, Crimes);
            spawnTask.AllowBuddySpawn = false;
            spawnTask.ClearVehicleArea = false;
            spawnTask.SpawnWithAllWeapons = false;
            spawnTask.AttemptSpawn();

            VehicleExt spawnedVehicle = spawnTask.CreatedVehicles.FirstOrDefault(x =>
                x != null && x.Vehicle.Exists());
            PedExt spawnedDriver = spawnTask.CreatedPeople.FirstOrDefault(x =>
                x != null && x.Pedestrian.Exists());
            if (spawnedVehicle == null || spawnedDriver == null)
            {
                foreach (PedExt ped in spawnTask.CreatedPeople.Where(x => x != null))
                    if (ped.Pedestrian.Exists()) ped.Pedestrian.Delete();
                foreach (VehicleExt vehicle in spawnTask.CreatedVehicles.Where(x => x != null))
                    if (vehicle.Vehicle.Exists()) vehicle.FullyDelete();
                continue;
            }

            if (NativeFunction.Natives.HAS_ENTITY_CLEAR_LOS_TO_ENTITY<bool>(
                Player.Character, spawnedVehicle.Vehicle, 17))
            {
                spawnedDriver.Pedestrian.Delete();
                spawnedVehicle.FullyDelete();
                continue;
            }

            spawnTask.CreatedPeople.ForEach(x =>
            {
                if (x != null && x.Pedestrian.Exists())
                    World.Pedestrians.AddEntity(x);
            });
            spawnTask.CreatedVehicles.ForEach(x =>
            {
                if (x != null && x.Vehicle.Exists())
                    x.AddVehicleToList(World);
            });

            RuntimeTeam team = new RuntimeTeam
            {
                AssignmentID = teamID,
                Driver = spawnedDriver,
                Vehicle = spawnedVehicle
            };
            IssueFollowCommand(team, settings);
            return team;
        }
        return null;
    }

    private static bool IsConfiguredUnmarkedVehicle(DispatchableVehicle vehicle)
    {
        string descriptor = ((vehicle.DebugName ?? "") + " " + (vehicle.ModelName ?? "") +
            " " + (vehicle.GroupName ?? "")).ToLowerInvariant();
        return descriptor.Contains("fib") || descriptor.Contains("unmarked") ||
            descriptor.Contains("undercover") || descriptor.Contains("surveillance") ||
            descriptor.Contains("plainclothes") || descriptor.Contains("uc ");
    }

    private void UpdateRuntimeTeams(LawEnforcementIntelligence intelligence, DateTime now,
        LawEnforcementIntelligenceSettings settings)
    {
        int excessTeams = Math.Max(0, RuntimeTeams.Count - settings.MaxActiveFIBTeams);
        if (excessTeams > 0)
        {
            foreach (KeyValuePair<string, RuntimeTeam> excess in RuntimeTeams
                .Select(pair => new
                {
                    Pair = pair,
                    Assignment = intelligence.SurveillanceAssignments.FirstOrDefault(x =>
                        x != null && x.ID == pair.Key),
                    Investigation = intelligence.SurveillanceAssignments
                        .Where(x => x != null && x.ID == pair.Key)
                        .Select(x => intelligence.Investigations.FirstOrDefault(i =>
                            i != null && i.ID == x.InvestigationID)).FirstOrDefault()
                })
                .OrderBy(x => x.Investigation == null ? InvestigationStage.Preliminary : x.Investigation.Stage)
                .ThenBy(x => x.Assignment == null ? DateTime.MinValue : x.Assignment.LastSuccessfulObservationAt)
                .Take(excessTeams).Select(x => x.Pair).ToList())
            {
                SurveillanceAssignment assignment = intelligence.SurveillanceAssignments
                    .FirstOrDefault(x => x != null && x.ID == excess.Key);
                if (assignment != null && assignment.State != SurveillanceAssignmentState.Disengaging &&
                    assignment.State != SurveillanceAssignmentState.Compromised)
                    (Player as Mod.Player)?.LawEnforcementIntelligenceManager?.SurveillanceAssignmentManager?
                        .Transition(assignment, SurveillanceAssignmentState.Disengaging, now);
            }
        }

        foreach (KeyValuePair<string, RuntimeTeam> pair in RuntimeTeams.ToList())
        {
            RuntimeTeam team = pair.Value;
            SurveillanceAssignment assignment = intelligence.SurveillanceAssignments
                .FirstOrDefault(x => x != null && x.ID == pair.Key);
            if (assignment == null)
            {
                CleanupTeam(team);
                RuntimeTeams.Remove(pair.Key);
                continue;
            }

            InvestigationRecord investigation = intelligence.Investigations.FirstOrDefault(x =>
                x != null && x.ID == assignment.InvestigationID);
            if (investigation == null || investigation.Stage == InvestigationStage.Closed ||
                investigation.Stage == InvestigationStage.Suspended)
            {
                if (assignment.State != SurveillanceAssignmentState.Disengaging &&
                    assignment.State != SurveillanceAssignmentState.Compromised)
                    (Player as Mod.Player)?.LawEnforcementIntelligenceManager?.SurveillanceAssignmentManager?
                        .Transition(assignment, SurveillanceAssignmentState.Disengaging, now);
            }

            bool entitiesExist = team.Driver != null && team.Driver.Pedestrian.Exists() &&
                team.Vehicle != null && team.Vehicle.Vehicle.Exists() &&
                team.Driver.Pedestrian.IsAlive;
            if (!entitiesExist)
            {
                (Player as Mod.Player)?.LawEnforcementIntelligenceManager?.SurveillanceAssignmentManager?
                    .Transition(assignment, SurveillanceAssignmentState.LostContact, now);
                assignment.ReplacementRequired = true;
                CleanupTeam(team);
                RuntimeTeams.Remove(pair.Key);
                continue;
            }

            float distance = Distance2D(team.Vehicle.Vehicle.Position, Player.Position);
            bool canObserve = NativeFunction.Natives.HAS_ENTITY_CLEAR_LOS_TO_ENTITY<bool>(
                team.Driver.Pedestrian, Player.Character, 17);
            bool playerCanSeeTeam = NativeFunction.Natives.HAS_ENTITY_CLEAR_LOS_TO_ENTITY<bool>(
                Player.Character, team.Vehicle.Vehicle, 17);

            if (assignment.State == SurveillanceAssignmentState.Compromised ||
                assignment.State == SurveillanceAssignmentState.Disengaging)
            {
                if (!team.DisengageCommandIssued)
                {
                    NativeFunction.Natives.TASK_VEHICLE_DRIVE_WANDER(team.Driver.Pedestrian,
                        team.Vehicle.Vehicle, 20f, 3);
                    team.DisengageCommandIssued = true;
                }
                if (distance >= settings.SurveillanceMaxDistance + 100f)
                {
                    (Player as Mod.Player)?.LawEnforcementIntelligenceManager?.SurveillanceAssignmentManager?
                        .Transition(assignment, SurveillanceAssignmentState.Completed, now);
                    CleanupTeam(team);
                    RuntimeTeams.Remove(pair.Key);
                }
                continue;
            }

            if (distance > settings.SurveillanceMaxDistance || !canObserve)
            {
                if (assignment.State == SurveillanceAssignmentState.Following ||
                    assignment.State == SurveillanceAssignmentState.Approaching)
                    (Player as Mod.Player)?.LawEnforcementIntelligenceManager?.SurveillanceAssignmentManager?
                        .Transition(assignment, SurveillanceAssignmentState.LostContact, now);

                DateTime lastContact = assignment.LastSuccessfulObservationAt == DateTime.MinValue
                    ? assignment.AssignedAt : assignment.LastSuccessfulObservationAt;
                if (now - lastContact >= TimeSpan.FromMinutes(2))
                {
                    assignment.ReplacementRequired = true;
                    (Player as Mod.Player)?.LawEnforcementIntelligenceManager?.SurveillanceAssignmentManager?
                        .Transition(assignment, SurveillanceAssignmentState.Disengaging, now);
                }
                continue;
            }

            if (playerCanSeeTeam && Game.GameTime - team.LastDetectionCheckAt >= 15000)
            {
                team.LastDetectionCheckAt = Game.GameTime;
                if (RandomItems.RandomPercent(settings.SurveillanceDetectionChance))
                {
                    (Player as Mod.Player)?.LawEnforcementIntelligenceManager?.SurveillanceAssignmentManager?
                        .Transition(assignment, SurveillanceAssignmentState.Compromised, now);
                    assignment.ReplacementRequired = true;
                    NativeFunction.Natives.TASK_VEHICLE_DRIVE_WANDER(team.Driver.Pedestrian,
                        team.Vehicle.Vehicle, 22f, 3);
                    team.DisengageCommandIssued = true;
                    continue;
                }
            }

            if (assignment.State == SurveillanceAssignmentState.LostContact)
            {
                (Player as Mod.Player)?.LawEnforcementIntelligenceManager?.SurveillanceAssignmentManager?
                    .Transition(assignment, SurveillanceAssignmentState.Approaching, now);
            }
            if (assignment.State == SurveillanceAssignmentState.Approaching)
            {
                (Player as Mod.Player)?.LawEnforcementIntelligenceManager?.SurveillanceAssignmentManager?
                    .Transition(assignment, SurveillanceAssignmentState.Following, now);
            }

            if (Game.GameTime - team.LastFollowCommandAt >= 5000)
                IssueFollowCommand(team, settings);

            RecordActualObservation(intelligence, assignment, now, team);
        }

        // A team whose entities disappeared has no runtime object left to drive its timeout.
        // Complete its assignment after the reacquisition window so another team can replace it.
        foreach (SurveillanceAssignment assignment in intelligence.SurveillanceAssignments.Where(x =>
            x != null && x.State == SurveillanceAssignmentState.LostContact &&
            !RuntimeTeams.ContainsKey(x.ID)).ToList())
        {
            DateTime lastContact = assignment.LastSuccessfulObservationAt == DateTime.MinValue
                ? assignment.AssignedAt : assignment.LastSuccessfulObservationAt;
            if (now - lastContact >= TimeSpan.FromMinutes(2))
            {
                assignment.ReplacementRequired = true;
                (Player as Mod.Player)?.LawEnforcementIntelligenceManager?.SurveillanceAssignmentManager?
                    .Transition(assignment, SurveillanceAssignmentState.Disengaging, now);
                (Player as Mod.Player)?.LawEnforcementIntelligenceManager?.SurveillanceAssignmentManager?
                    .Transition(assignment, SurveillanceAssignmentState.Completed, now);
            }
        }
    }

    private void RecordActualObservation(LawEnforcementIntelligence intelligence,
        SurveillanceAssignment assignment, DateTime now, RuntimeTeam team)
    {
        // Only called after the spawned FIB agent has direct line of sight to the player.
        IntelligenceObservation observation = new IntelligenceObservation
        {
            ObservedAt = now,
            Source = IntelligenceSource.Surveillance,
            EvidenceType = IntelligenceEvidenceType.Surveillance,
            TargetType = IntelligenceTargetType.Person,
            TargetID = intelligence.SubjectID,
            TargetDescription = "Player",
            SourceID = "FIB_SURVEILLANCE:" + assignment.TeamID,
            IsKnownToPolice = true,
            IsVerified = true,
            Confidence = 0.9f,
            PositionX = Player.Position.X,
            PositionY = Player.Position.Y,
            PositionZ = Player.Position.Z,
            Notes = "Direct visual observation by an assigned FIB surveillance team."
        };
        if (intelligence.RecordObservation(observation))
        {
            (Player as Mod.Player)?.LawEnforcementIntelligenceManager?.SurveillanceAssignmentManager?
                .RecordObservation(assignment, now, Player.Position.X, Player.Position.Y, Player.Position.Z);
        }
    }

    private void IssueFollowCommand(RuntimeTeam team, LawEnforcementIntelligenceSettings settings)
    {
        if (team == null || team.Driver == null || !team.Driver.Pedestrian.Exists() ||
            team.Vehicle == null || !team.Vehicle.Vehicle.Exists())
            return;
        Entity target = Player.IsInVehicle && Player.CurrentVehicle != null &&
            Player.CurrentVehicle.Vehicle.Exists()
            ? (Entity)Player.CurrentVehicle.Vehicle
            : Player.Character;
        int followDistance = Math.Max(50, (int)settings.SurveillanceMinDistance);
        NativeFunction.Natives.xFC545A9F0626E3B6(team.Driver.Pedestrian,
            team.Vehicle.Vehicle, target, 22f, 3, followDistance);
        team.LastFollowCommandAt = Game.GameTime;
    }

    private void DisengageAll(LawEnforcementIntelligence intelligence, DateTime now)
    {
        foreach (KeyValuePair<string, RuntimeTeam> pair in RuntimeTeams.ToList())
        {
            SurveillanceAssignment assignment = intelligence.SurveillanceAssignments
                .FirstOrDefault(x => x != null && x.ID == pair.Key);
            if (assignment != null && assignment.State != SurveillanceAssignmentState.Completed)
            {
                (Player as Mod.Player)?.LawEnforcementIntelligenceManager?.SurveillanceAssignmentManager?
                    .Transition(assignment, SurveillanceAssignmentState.Disengaging, now);
            }
            CleanupTeam(pair.Value);
            if (assignment != null)
                (Player as Mod.Player)?.LawEnforcementIntelligenceManager?.SurveillanceAssignmentManager?
                    .Transition(assignment, SurveillanceAssignmentState.Completed, now);
            RuntimeTeams.Remove(pair.Key);
        }
    }

    private static void CleanupTeam(RuntimeTeam team)
    {
        if (team == null) return;
        if (team.Driver != null && team.Driver.Pedestrian.Exists())
            team.Driver.Pedestrian.Delete();
        if (team.Vehicle != null && team.Vehicle.Vehicle.Exists())
            team.Vehicle.FullyDelete();
    }

    private static float Distance2D(Vector3 a, Vector3 b)
    {
        float dx = a.X - b.X;
        float dy = a.Y - b.Y;
        return (float)Math.Sqrt(dx * dx + dy * dy);
    }
}
