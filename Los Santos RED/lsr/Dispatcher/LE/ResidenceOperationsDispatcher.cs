using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

public class ResidenceOperationsDispatcher
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
    private readonly IPlacesOfInterest PlacesOfInterest;
    private readonly Dictionary<string, RuntimeOperation> RuntimeOperations =
        new Dictionary<string, RuntimeOperation>();
    private uint LastUpdateGameTime;

    private class RuntimeOperation
    {
        public Residence Residence;
        public List<Cop> Officers = new List<Cop>();
        public uint StageStartedAt;
        public bool AnnouncementShown;
        public bool TeamSpawned;
    }

    public ResidenceOperationsDispatcher(IEntityProvideable world, IDispatchable player,
        IAgencies agencies, ISettingsProvideable settings, IWeapons weapons,
        INameProvideable names, IModItems modItems, IShopMenus shopMenus,
        ICrimes crimes, IPlacesOfInterest placesOfInterest)
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
        PlacesOfInterest = placesOfInterest;
    }

    public void Update()
    {
        Mod.Player modPlayer = Player as Mod.Player;
        LawEnforcementIntelligenceSettings settings =
            Settings?.SettingsManager?.LawEnforcementIntelligenceSettings;
        if (modPlayer == null || modPlayer.LawEnforcementIntelligence == null || settings == null)
            return;
        if (Game.GameTime - LastUpdateGameTime < 1000)
            return;
        LastUpdateGameTime = Game.GameTime;

        LawEnforcementIntelligence intelligence = modPlayer.LawEnforcementIntelligence;
        intelligence.Normalize();
        settings.Normalize();

        if (!settings.EnableWarrants)
        {
            AbortAll(intelligence, modPlayer.CurrentDateTime,
                "Residence operations stopped because warrant operations are disabled.");
            return;
        }

        UpdateActiveOperations(intelligence, modPlayer.CurrentDateTime);
        TryPlanOperation(intelligence, modPlayer.CurrentDateTime, settings);
    }

    public void Dispose()
    {
        Mod.Player modPlayer = Player as Mod.Player;
        if (modPlayer != null && modPlayer.LawEnforcementIntelligence != null)
            AbortAll(modPlayer.LawEnforcementIntelligence, modPlayer.CurrentDateTime,
                "Residence operation ended because the mod session is shutting down.");
        else
        {
            foreach (RuntimeOperation runtime in RuntimeOperations.Values.ToList())
                CleanupRuntime(runtime);
            RuntimeOperations.Clear();
        }
    }

    private void TryPlanOperation(LawEnforcementIntelligence intelligence, DateTime now,
        LawEnforcementIntelligenceSettings settings)
    {
        if (intelligence.PoliceOperations.Any(x => x != null &&
            x.Stage != PoliceOperationStage.Completed && x.Stage != PoliceOperationStage.Aborted))
            return;

        Residence currentResidence = Player.IsDead ? null : GetCurrentResidence();
        if (currentResidence != null && currentResidence.IsOwnedOrRented)
        {
            string currentResidenceID = currentResidence.ResidenceID.ToString();
            WarrantRecord arrestWarrant = intelligence.Warrants.FirstOrDefault(x => x != null &&
                x.Status == WarrantStatus.Active && IsArrestWarrant(x.Type) &&
                string.Equals(x.SuspectedResidenceID, currentResidenceID, StringComparison.OrdinalIgnoreCase) &&
                x.ResidenceConfidence * 100f >= settings.RaidMinimumConfidence &&
                x.Priority >= WarrantPriority.High);
            if (arrestWarrant != null)
            {
                string investigationID = FindInvestigationID(intelligence, arrestWarrant.ID);
                PoliceOperationRecord raid = modPlayerOperations(intelligence).PlanResidenceOperation(
                    intelligence, arrestWarrant.ID, investigationID, currentResidenceID,
                    true, 2, now, settings);
                if (raid != null)
                {
                    RuntimeOperations[raid.ID] = new RuntimeOperation
                    {
                        Residence = currentResidence,
                        StageStartedAt = Game.GameTime
                    };
                    return;
                }
            }
        }

        WarrantRecord searchWarrant = intelligence.Warrants
            .Where(x => x != null && x.Status == WarrantStatus.Active &&
                x.Type == WarrantType.Search &&
                !string.IsNullOrWhiteSpace(x.SuspectedResidenceID) &&
                x.ResidenceConfidence * 100f >= settings.RaidMinimumConfidence &&
                x.Priority >= WarrantPriority.Moderate)
            .OrderByDescending(x => x.Priority).ThenBy(x => x.IssuedAt).FirstOrDefault();
        if (searchWarrant == null)
            return;

        Residence searchResidence = FindResidence(searchWarrant.SuspectedResidenceID);
        if (searchResidence == null)
            return;
        string searchResidenceID = searchResidence.ResidenceID.ToString();
        string searchInvestigationID = FindInvestigationID(intelligence, searchWarrant.ID);
        bool playerBelievedHome = IsPlayerAtResidence(searchResidence);
        PoliceOperationRecord search = modPlayerOperations(intelligence).PlanResidenceOperation(
            intelligence, searchWarrant.ID, searchInvestigationID, searchResidenceID,
            playerBelievedHome, 1, now, settings);
        if (search != null)
        {
            RuntimeOperations[search.ID] = new RuntimeOperation
            {
                Residence = searchResidence,
                StageStartedAt = Game.GameTime
            };
        }
    }

    private PoliceOperationsManager modPlayerOperations(LawEnforcementIntelligence intelligence)
    {
        return (Player as Mod.Player)?.LawEnforcementIntelligenceManager?.PoliceOperationsManager;
    }

    private void UpdateActiveOperations(LawEnforcementIntelligence intelligence, DateTime now)
    {
        foreach (PoliceOperationRecord operation in intelligence.PoliceOperations.Where(x => x != null &&
            x.Stage != PoliceOperationStage.Completed && x.Stage != PoliceOperationStage.Aborted).ToList())
        {
            if (!RuntimeOperations.TryGetValue(operation.ID, out RuntimeOperation runtime))
            {
                Residence residence = FindResidence(operation.ResidenceID);
                if (residence == null)
                {
                    modPlayerOperations(intelligence)?.Abort(intelligence, operation.ID, now,
                        "The warrant residence could not be resolved in the current world data.");
                    continue;
                }
                runtime = new RuntimeOperation { Residence = residence, StageStartedAt = Game.GameTime };
                RuntimeOperations[operation.ID] = runtime;
            }

            if (operation.Type == PoliceOperationType.ArrestWarrantRaid)
                UpdateArrestRaid(intelligence, operation, runtime, now);
            else if (operation.Type == PoliceOperationType.SearchWarrantExecution)
                UpdateSearchOperation(intelligence, operation, runtime, now);
        }

        foreach (string operationID in RuntimeOperations.Keys.ToList())
        {
            bool stillActive = intelligence.PoliceOperations.Any(x => x != null &&
                x.ID == operationID && x.Stage != PoliceOperationStage.Completed &&
                x.Stage != PoliceOperationStage.Aborted);
            if (!stillActive)
            {
                CleanupRuntime(RuntimeOperations[operationID]);
                RuntimeOperations.Remove(operationID);
            }
        }
    }

    private void UpdateArrestRaid(LawEnforcementIntelligence intelligence,
        PoliceOperationRecord operation, RuntimeOperation runtime, DateTime now)
    {
        PoliceOperationsManager manager = modPlayerOperations(intelligence);
        if (manager == null || runtime.Residence == null)
            return;

        if (operation.Stage == PoliceOperationStage.Planned)
            manager.Advance(intelligence, operation.ID, PoliceOperationStage.Deploying, now);
        if (operation.Stage == PoliceOperationStage.Deploying)
            manager.Advance(intelligence, operation.ID, PoliceOperationStage.Perimeter, now);
        if (operation.Stage == PoliceOperationStage.Perimeter)
        {
            manager.Advance(intelligence, operation.ID, PoliceOperationStage.Announcement, now);
            runtime.StageStartedAt = Game.GameTime;
            runtime.AnnouncementShown = true;
            Game.DisplaySubtitle("Police: We have an arrest warrant. Come out with your hands visible.", 5000);
        }

        if (operation.Stage == PoliceOperationStage.Announcement)
        {
            if (Player.IsDead || !IsPlayerAtResidence(runtime.Residence))
            {
                manager.RecordAnnouncementResponse(intelligence, operation.ID,
                    PoliceOperationPlayerResponse.Absent, now);
                return;
            }

            if (Player.IsAttemptingToSurrender)
                manager.RecordAnnouncementResponse(intelligence, operation.ID,
                    PoliceOperationPlayerResponse.Surrendered, now);
            else if (Player.IsDangerouslyArmed || Player.IsWanted)
                manager.RecordAnnouncementResponse(intelligence, operation.ID,
                    PoliceOperationPlayerResponse.Resisted, now);
            else if (Game.GameTime - runtime.StageStartedAt >= 6000)
                manager.RecordAnnouncementResponse(intelligence, operation.ID,
                    PoliceOperationPlayerResponse.DidNotComply, now);

            if (operation.Stage == PoliceOperationStage.Entry)
                runtime.StageStartedAt = Game.GameTime;
        }

        if (operation.Stage == PoliceOperationStage.Entry)
        {
            if (!IsPlayerAtResidence(runtime.Residence))
            {
                manager.AbortIfPlayerAbsent(intelligence, operation.ID, now);
                return;
            }
            if (!runtime.TeamSpawned)
            {
                Agency agency = GetAgencyForWarrant(intelligence, operation.WarrantID);
                Vector3 firstPosition = Player.Character.GetOffsetPosition(new Vector3(4f, 0f, 0f));
                Vector3 secondPosition = Player.Character.GetOffsetPosition(new Vector3(-4f, 0f, 0f));
                runtime.Officers.Add(SpawnOfficer(agency, firstPosition, false));
                runtime.Officers.Add(SpawnOfficer(agency, secondPosition, false));
                runtime.Officers.RemoveAll(x => x == null);
                runtime.TeamSpawned = true;
                if (runtime.Officers.Count < 2)
                {
                    manager.Abort(intelligence, operation.ID, now,
                        "Raid aborted because the required entry team could not be deployed.");
                    return;
                }
                manager.Advance(intelligence, operation.ID, PoliceOperationStage.ArrestAttempt, now,
                    "Entry team deployed; officers are attempting to apprehend the subject.");
                runtime.StageStartedAt = Game.GameTime;
            }
        }

        if (operation.Stage == PoliceOperationStage.ArrestAttempt)
        {
            if (Player.IsBusted)
            {
                manager.Advance(intelligence, operation.ID, PoliceOperationStage.Completed, now,
                    "Subject apprehended during the residence raid.");
                WarrantRecord warrant = intelligence.Warrants.FirstOrDefault(x =>
                    x != null && x.ID == operation.WarrantID);
                if (warrant != null && warrant.Status == WarrantStatus.Active)
                    intelligence.ResolveWarrant(warrant.ID, WarrantStatus.Served, now,
                        "Arrest warrant served during a residence raid.");
                RecordOperationObservation(intelligence, operation, runtime.Residence, now,
                    "Arrest warrant raid completed and the subject was apprehended.");
                return;
            }

            if (!IsPlayerAtResidence(runtime.Residence) ||
                Game.GameTime - runtime.StageStartedAt >= 60000)
            {
                manager.Advance(intelligence, operation.ID, PoliceOperationStage.Completed, now,
                    "Raid ended without a confirmed apprehension; the warrant remains active.");
                return;
            }
        }
    }

    private void UpdateSearchOperation(LawEnforcementIntelligence intelligence,
        PoliceOperationRecord operation, RuntimeOperation runtime, DateTime now)
    {
        PoliceOperationsManager manager = modPlayerOperations(intelligence);
        if (manager == null || runtime.Residence == null)
            return;

        if (operation.Stage == PoliceOperationStage.Planned)
            manager.Advance(intelligence, operation.ID, PoliceOperationStage.Deploying, now);
        if (operation.Stage == PoliceOperationStage.Deploying)
            manager.Advance(intelligence, operation.ID, PoliceOperationStage.Perimeter, now);
        if (operation.Stage == PoliceOperationStage.Perimeter)
        {
            if (IsPlayerAtResidence(runtime.Residence))
            {
                manager.Advance(intelligence, operation.ID, PoliceOperationStage.Announcement, now);
                runtime.StageStartedAt = Game.GameTime;
                runtime.AnnouncementShown = true;
                Game.DisplaySubtitle("Police: We have a search warrant for these premises.", 5000);
            }
            else
            {
                manager.Advance(intelligence, operation.ID, PoliceOperationStage.Search, now);
                runtime.StageStartedAt = Game.GameTime;
            }
        }

        if (operation.Stage == PoliceOperationStage.Announcement)
        {
            if (!IsPlayerAtResidence(runtime.Residence))
                manager.RecordAnnouncementResponse(intelligence, operation.ID,
                    PoliceOperationPlayerResponse.Absent, now);
            else if (Player.IsAttemptingToSurrender)
                manager.RecordAnnouncementResponse(intelligence, operation.ID,
                    PoliceOperationPlayerResponse.CompliedWithSearch, now);
            else if (Game.GameTime - runtime.StageStartedAt >= 5000)
                manager.RecordAnnouncementResponse(intelligence, operation.ID,
                    PoliceOperationPlayerResponse.RefusedEntry, now);

            if (operation.Stage == PoliceOperationStage.Entry)
                manager.Advance(intelligence, operation.ID, PoliceOperationStage.Search, now);
            if (operation.Stage == PoliceOperationStage.Search)
                runtime.StageStartedAt = Game.GameTime;
        }

        if (operation.Stage == PoliceOperationStage.Search)
        {
            if (!runtime.TeamSpawned)
            {
                Agency agency = GetAgencyForWarrant(intelligence, operation.WarrantID);
                List<Vector3> positions = GetSearchPositions(runtime.Residence);
                foreach (Vector3 position in positions.Take(2))
                {
                    Cop officer = SpawnOfficer(agency, position, true);
                    if (officer != null)
                    {
                        runtime.Officers.Add(officer);
                        NativeFunction.Natives.TASK_STAND_STILL(officer.Pedestrian, 9000);
                    }
                }
                runtime.TeamSpawned = true;
                if (runtime.Officers.Count == 0)
                {
                    manager.Abort(intelligence, operation.ID, now,
                        "Search warrant execution aborted because no search officers could be deployed.");
                    return;
                }
                runtime.StageStartedAt = Game.GameTime;
            }

            if (Game.GameTime - runtime.StageStartedAt >= 10000)
            {
                manager.Advance(intelligence, operation.ID, PoliceOperationStage.Completed, now,
                    "Search warrant executed at the identified premises. This operation record does not imply that contraband was found.");
                WarrantRecord warrant = intelligence.Warrants.FirstOrDefault(x =>
                    x != null && x.ID == operation.WarrantID);
                if (warrant != null && warrant.Status == WarrantStatus.Active)
                    intelligence.ResolveWarrant(warrant.ID, WarrantStatus.Served, now,
                        "Search warrant executed at the identified residence.");
                RecordOperationObservation(intelligence, operation, runtime.Residence, now,
                    "Official record of search warrant execution; no evidence finding is inferred.");
            }
        }
    }

    private void RecordOperationObservation(LawEnforcementIntelligence intelligence,
        PoliceOperationRecord operation, Residence residence, DateTime now, string notes)
    {
        if (intelligence == null || operation == null || residence == null)
            return;
        Vector3 position = residence.EntrancePosition;
        intelligence.RecordObservation(new IntelligenceObservation
        {
            ObservedAt = now,
            Source = IntelligenceSource.PoliceOperations,
            EvidenceType = IntelligenceEvidenceType.OfficialRecord,
            TargetType = IntelligenceTargetType.Location,
            TargetID = "RESIDENCE:" + residence.ResidenceID,
            TargetDescription = residence.Name,
            SourceID = "POLICE_OPERATION:" + operation.ID,
            IsKnownToPolice = true,
            IsVerified = true,
            Confidence = 1f,
            PositionX = position.X,
            PositionY = position.Y,
            PositionZ = position.Z,
            LocationID = residence.ResidenceID.ToString(),
            LocationName = residence.Name,
            Notes = notes
        });
    }

    private Cop SpawnOfficer(Agency agency, Vector3 position, bool offDuty)
    {
        DispatchablePerson person = agency?.Personnel?.FirstOrDefault(x => x != null && !x.IsAnimal);
        if (person == null)
            return null;

        SpawnLocation spawnLocation = new SpawnLocation(position);
        spawnLocation.StreetPosition = position;
        LESpawnTask spawnTask = new LESpawnTask(agency, spawnLocation, null, person, false,
            Settings, Weapons, Names, false, World, ModItems, false, ShopMenus, Crimes);
        spawnTask.AllowAnySpawn = true;
        spawnTask.AllowBuddySpawn = false;
        spawnTask.ClearVehicleArea = false;
        spawnTask.PlacePedOnGround = false;
        spawnTask.SpawnWithAllWeapons = false;
        spawnTask.IsOffDutySpawn = offDuty;
        spawnTask.AttemptSpawn();

        foreach (PedExt ped in spawnTask.CreatedPeople.Where(x => x != null && x.Pedestrian.Exists()))
            World.Pedestrians.AddEntity(ped);

        Cop cop = spawnTask.CreatedPeople.OfType<Cop>().FirstOrDefault(x =>
            x != null && x.Pedestrian.Exists());
        if (cop == null)
        {
            foreach (PedExt ped in spawnTask.CreatedPeople.Where(x => x != null))
                if (ped.Pedestrian.Exists()) ped.Pedestrian.Delete();
        }
        return cop;
    }

    private Agency GetAgencyForWarrant(LawEnforcementIntelligence intelligence, string warrantID)
    {
        WarrantRecord warrant = intelligence.Warrants.FirstOrDefault(x => x != null && x.ID == warrantID);
        Agency agency = string.IsNullOrWhiteSpace(warrant?.IssuingAgencyID)
            ? null : Agencies?.GetAgency(warrant.IssuingAgencyID);
        if (agency?.Personnel != null && agency.Personnel.Any(x => x != null && !x.IsAnimal))
            return agency;
        Agency fib = Agencies?.GetAgency("FIB");
        return fib?.Personnel != null && fib.Personnel.Any(x => x != null && !x.IsAnimal) ? fib : agency;
    }

    private List<Vector3> GetSearchPositions(Residence residence)
    {
        List<Vector3> positions = residence?.ResidenceInterior?.AllInteractPoints?
            .Where(x => x != null && x.Position != Vector3.Zero)
            .Select(x => x.Position).Distinct().Take(2).ToList() ?? new List<Vector3>();
        if (positions.Count == 0 && residence != null && residence.EntrancePosition != Vector3.Zero)
            positions.Add(residence.EntrancePosition);
        return positions;
    }

    private Residence GetCurrentResidence()
    {
        if (Player?.CurrentLocation == null || !Player.CurrentLocation.IsInside)
            return null;
        return Player.CurrentLocation.CurrentInteriorGameLocation as Residence;
    }

    private bool IsPlayerAtResidence(Residence residence)
    {
        Residence current = GetCurrentResidence();
        return current != null && residence != null && current.ResidenceID == residence.ResidenceID;
    }

    private Residence FindResidence(string residenceID)
    {
        if (string.IsNullOrWhiteSpace(residenceID) || PlacesOfInterest?.PossibleLocations?.Residences == null)
            return null;
        if (!int.TryParse(residenceID, out int id))
            return null;
        return PlacesOfInterest.PossibleLocations.Residences.FirstOrDefault(x =>
            x != null && x.ResidenceID == id);
    }

    private static string FindInvestigationID(LawEnforcementIntelligence intelligence, string warrantID)
    {
        return intelligence.Investigations.FirstOrDefault(x => x != null &&
            x.Stage != InvestigationStage.Closed && x.Stage != InvestigationStage.Suspended &&
            x.WarrantIDs != null && x.WarrantIDs.Contains(warrantID))?.ID;
    }

    private static bool IsArrestWarrant(WarrantType type)
    {
        return type == WarrantType.Arrest || type == WarrantType.Bench ||
            type == WarrantType.ProbationParole || type == WarrantType.Extradition;
    }

    private void AbortAll(LawEnforcementIntelligence intelligence, DateTime now, string reason)
    {
        foreach (PoliceOperationRecord operation in intelligence.PoliceOperations.Where(x => x != null &&
            x.Stage != PoliceOperationStage.Completed && x.Stage != PoliceOperationStage.Aborted).ToList())
            modPlayerOperations(intelligence)?.Abort(intelligence, operation.ID, now, reason);
        foreach (RuntimeOperation runtime in RuntimeOperations.Values.ToList())
            CleanupRuntime(runtime);
        RuntimeOperations.Clear();
    }

    private static void CleanupRuntime(RuntimeOperation runtime)
    {
        if (runtime == null) return;
        foreach (Cop officer in runtime.Officers.Where(x => x != null).ToList())
            if (officer.Pedestrian.Exists()) officer.Pedestrian.Delete();
        runtime.Officers.Clear();
    }
}
