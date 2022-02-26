using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

public class GangDispatcher
{
    private readonly IGangs Gangs;
    private readonly IDispatchable Player;
    private readonly int LikelyHoodOfAnySpawn;
    private readonly float MinimumDeleteDistance = 150f;//200f
    private readonly uint MinimumExistingTime = 20000;
    private readonly ISettingsProvideable Settings;
    private readonly IStreets Streets;
    private readonly IEntityProvideable World;
    private readonly IZones Zones;
    private uint GameTimeAttemptedDispatch;
    private uint GameTimeAttemptedRecall;
    private bool HasDispatchedThisTick;
    private IWeapons Weapons;
    private INameProvideable Names;
    private IGangTerritories GangTerritories;
    private IPedGroups PedGroups;
    private ICrimes Crimes;
    private IShopMenus ShopMenus;
    public GangDispatcher(IEntityProvideable world, IDispatchable player, IGangs gangs, ISettingsProvideable settings, IStreets streets, IZones zones, IGangTerritories gangTerritories, IWeapons weapons, INameProvideable names, IPedGroups pedGroups, ICrimes crimes, IShopMenus shopMenus)
    {
        Player = player;
        World = world;
        Gangs = gangs;
        Settings = settings;
        Streets = streets;
        Zones = zones;
        GangTerritories = gangTerritories;
        Weapons = weapons;
        Names = names;
        PedGroups = pedGroups;
        Crimes = crimes;
        ShopMenus = shopMenus;
        LikelyHoodOfAnySpawn = Settings.SettingsManager.GangSettings.PercentSpawnOutsideTerritory;
    }
    private float ClosestOfficerSpawnToPlayerAllowed => 45f;
    private List<GangMember> DeleteableGangMembers => World.Pedestrians.GangMemberList.Where(x => (x.RecentlyUpdated && x.DistanceToPlayer >= MinimumDeleteDistance && x.HasBeenSpawnedFor >= MinimumExistingTime) || x.CanRemove).ToList();
    private float DistanceToDelete => 300f;
    private float DistanceToDeleteOnFoot => 250f;
    private bool HasNeedToDispatch => World.Pedestrians.TotalSpawnedGangMembers <= Settings.SettingsManager.GangSettings.TotalSpawnedMembersLimit;
    private bool IsTimeToDispatch => Game.GameTime - GameTimeAttemptedDispatch >= 25000;//15000;
    private bool IsTimeToRecall => Game.GameTime - GameTimeAttemptedRecall >= TimeBetweenSpawn;
    private float MaxDistanceToSpawn => Settings.SettingsManager.GangSettings.MaxDistanceToSpawn;//150f;
    private float MinDistanceToSpawn => Settings.SettingsManager.GangSettings.MinDistanceToSpawn;//50f;
    private int TimeBetweenSpawn => Settings.SettingsManager.GangSettings.TimeBetweenSpawn;//15000;
    public bool Dispatch()
    {
        HasDispatchedThisTick = false;
        if (Settings.SettingsManager.GangSettings.ManageDispatching && IsTimeToDispatch && HasNeedToDispatch)
        {
            HasDispatchedThisTick = true;//up here for now, might be better down low
            EntryPoint.WriteToConsole($"DISPATCHER: Attempting Gang Spawn", 3);
            int timesTried = 0;
            bool isValidSpawn = false;
            SpawnLocation spawnLocation = new SpawnLocation();
            do
            {
                spawnLocation.InitialPosition = GetPositionAroundPlayer();
                spawnLocation.GetClosestStreet();
                spawnLocation.GetClosestSidewalk();
                GameFiber.Yield();
                isValidSpawn = IsValidSpawn(spawnLocation);
                timesTried++;
            }
            while (!spawnLocation.HasSpawns && !isValidSpawn && timesTried < 2);//10
            if (spawnLocation.HasSpawns && isValidSpawn)
            {
                Gang gang = GetRandomGang(spawnLocation);
                if (gang != null)
                {
                    int TotalGangMembers = World.Pedestrians.GangMemberList.Count(x => x.Gang?.ID == gang.ID);
                    if(TotalGangMembers >= gang.SpawnLimit)
                    {
                        return true;
                    }
                    EntryPoint.WriteToConsole($"DISPATCHER: Attempting Gang Spawn for {gang.ID} spawnLocation.HasSidewalk {spawnLocation.HasSidewalk}", 3);
                    DispatchableVehicle VehicleType = null;
                    //VehicleType = gang.GetRandomVehicle(Player.WantedLevel, false, false, true);
                    if (!spawnLocation.HasSidewalk || RandomItems.RandomPercent(gang.VehicleSpawnPercentage))
                    {
                         VehicleType = gang.GetRandomVehicle(Player.WantedLevel, false, false, true);
                    }
                    if (VehicleType != null || spawnLocation.HasSidewalk)
                    {
                        EntryPoint.WriteToConsole($"DISPATCHER: Attempting Gang Spawn Vehicle? {VehicleType?.ModelName}", 3);
                        string RequiredGroup = "";
                        if (VehicleType != null)
                        {
                            RequiredGroup = VehicleType.RequiredPedGroup;
                        }
                        DispatchablePerson PersonType = gang.GetRandomPed(Player.WantedLevel, RequiredGroup);
                        if (PersonType != null)
                        {
                            EntryPoint.WriteToConsole($"DISPATCHER: Attempting Gang Spawn Person {PersonType.ModelName}", 3);
                            try
                            {
                                SpawnTask spawnTask = new SpawnTask(gang, spawnLocation, VehicleType, PersonType, Settings.SettingsManager.GangSettings.ShowSpawnedBlip, Settings, Weapons, Names, true, Crimes, PedGroups, ShopMenus, World);// Settings.SettingsManager.Police.SpawnedAmbientPoliceHaveBlip);
                                spawnTask.AttemptSpawn();
                                foreach(PedExt created in spawnTask.CreatedPeople)
                                {
                                    World.Pedestrians.AddEntity(created);

                                }
                                //spawnTask.CreatedPeople.ForEach(x => World.AddEntity(x));
                                spawnTask.CreatedVehicles.ForEach(x => World.Vehicles.AddEntity(x, ResponseType.None));
                                HasDispatchedThisTick = true;
                            }
                            catch (Exception ex)
                            {
                                EntryPoint.WriteToConsole($"DISPATCHER: Spawn Gang ERROR {ex.Message} : {ex.StackTrace}", 0);
                            }
                        }
                    }
                }
            }
            else
            {
                EntryPoint.WriteToConsole($"DISPATCHER: Attempting to Spawn Gang Failed, Has Spawns {spawnLocation.HasSpawns} Is Valid {isValidSpawn}", 5);
            }
            GameTimeAttemptedDispatch = Game.GameTime;
        }
        return HasDispatchedThisTick;
    }
    public void Dispose()
    {

    }
    public void Recall()
    {
        if (IsTimeToRecall)
        {
            foreach (GangMember emt in DeleteableGangMembers)
            {
                if (ShouldBeRecalled(emt))
                {
                    Delete(emt);
                    GameFiber.Yield();
                }
            }
            GameTimeAttemptedRecall = Game.GameTime;
        }
    }
    private bool ShouldBeRecalled(GangMember emt)
    {
        if (emt.IsInVehicle)
        {
            return emt.DistanceToPlayer >= DistanceToDelete;
        }
        else
        {
            return emt.DistanceToPlayer >= DistanceToDeleteOnFoot;
        }
    }
    private void Delete(PedExt emt)
    {
        if (emt != null && emt.Pedestrian.Exists())
        {
            //EntryPoint.WriteToConsole($"Attempting to Delete {Cop.Pedestrian.Handle}");
            if (emt.Pedestrian.IsInAnyVehicle(false))
            {
                if (emt.Pedestrian.CurrentVehicle.HasPassengers)
                {
                    foreach (Ped Passenger in emt.Pedestrian.CurrentVehicle.Passengers)
                    {
                        if (Passenger.Handle != Game.LocalPlayer.Character.Handle)
                        {
                            RemoveBlip(Passenger);
                            Passenger.Delete();
                            EntryPoint.PersistentPedsDeleted++;
                        }
                    }
                }
                if (emt.Pedestrian.Exists() && emt.Pedestrian.CurrentVehicle.Exists() && emt.Pedestrian.CurrentVehicle != null)
                {
                    emt.Pedestrian.CurrentVehicle.Delete();
                    EntryPoint.PersistentVehiclesDeleted++;
                }
            }
            RemoveBlip(emt.Pedestrian);
            if (emt.Pedestrian.Exists())
            {
                //EntryPoint.WriteToConsole(string.Format("Delete Cop Handle: {0}, {1}, {2}", Cop.Pedestrian.Handle, Cop.DistanceToPlayer, Cop.AssignedAgency.Initials));
                emt.Pedestrian.Delete();
                EntryPoint.PersistentPedsDeleted++;
            }
        }
    }
    private void RemoveBlip(Ped emt)
    {
        if (!emt.Exists())
        {
            return;
        }
        Blip MyBlip = emt.GetAttachedBlip();
        if (MyBlip.Exists())
        {
            MyBlip.Delete();
        }
    }
    private List<Gang> GetGangs(Vector3 Position, int WantedLevel)
    {
        List<Gang> ToReturn = new List<Gang>();
        Zone CurrentZone = Zones.GetZone(Position);
        Gang ZoneAgency = GangTerritories.GetRandomGang(CurrentZone.InternalGameName, WantedLevel);
        if (ZoneAgency != null)
        {
            ToReturn.Add(ZoneAgency); //Zone Jurisdiciton Random
        }
        if (!ToReturn.Any() || RandomItems.RandomPercent(LikelyHoodOfAnySpawn))//fall back to anybody
        {
            ToReturn.Clear();
            ToReturn.AddRange(Gangs.GetSpawnableGangs(WantedLevel));
            EntryPoint.WriteToConsole("Gang Dispatcher, set to random gang spawn");
        }
        foreach (Gang ag in ToReturn)
        {
            //EntryPoint.WriteToConsole(string.Format("Debugging: Agencies At Pos: {0}", ag.Initials));
        }
        return ToReturn;
    }
    private Vector3 GetPositionAroundPlayer()
    {
        Vector3 Position;
        if (Player.IsInVehicle)
        {
            Position = Player.Character.GetOffsetPositionFront(250f);//350f
        }
        else
        {
            Position = Player.Position;
        }
        Position = Position.Around2D(MinDistanceToSpawn, MaxDistanceToSpawn);
        return Position;
    }
    private Gang GetRandomGang(SpawnLocation spawnLocation)
    {
        Gang Gang;
        List<Gang> PossibleGangs = GetGangs(spawnLocation.StreetPosition, Player.WantedLevel);
        Gang = PossibleGangs.PickRandom();
        if (Gang == null)
        {
            Gang = GetGangs(spawnLocation.InitialPosition, Player.WantedLevel).PickRandom();
        }
        if (Gang == null)
        {
            //EntryPoint.WriteToConsole("Dispatcher could not find Agency To Spawn");
        }
        return Gang;
    }
    private Gang GetRandomGang(Vector3 spawnLocation)
    {
        Gang agency;
        List<Gang> PossibleAgencies = GetGangs(spawnLocation, Player.WantedLevel);
        agency = PossibleAgencies.PickRandom();
        if (agency == null)
        {
            agency = GetGangs(spawnLocation, Player.WantedLevel).PickRandom();
        }
        if (agency == null)
        {
            //EntryPoint.WriteToConsole("Dispatcher could not find Agency To Spawn");
        }
        return agency;
    }
    private bool IsValidSpawn(SpawnLocation spawnLocation)
    {
        if (spawnLocation.StreetPosition.DistanceTo2D(Player.Position) < ClosestOfficerSpawnToPlayerAllowed)
        {
            return false;
        }
        else if (spawnLocation.InitialPosition.DistanceTo2D(Player.Position) < ClosestOfficerSpawnToPlayerAllowed)
        {
            return false;
        }
        return true;
    }

    public bool ForceDispatch(string gangID, bool onFoot)
    {
        HasDispatchedThisTick = false;
        if (1==1)//Settings.SettingsManager.GangSettings.ManageDispatching && IsTimeToDispatch && HasNeedToDispatch)
        {
            HasDispatchedThisTick = true;//up here for now, might be better down low
            EntryPoint.WriteToConsole($"DISPATCHER: Attempting Gang Spawn", 3);
            int timesTried = 0;
            bool isValidSpawn = false;
            SpawnLocation spawnLocation = new SpawnLocation();
            do
            {
                spawnLocation.InitialPosition = Game.LocalPlayer.Character.GetOffsetPositionFront(10f);//GetPositionAroundPlayer();
                spawnLocation.StreetPosition = spawnLocation.InitialPosition;
               //spawnLocation.GetClosestStreet();
               // spawnLocation.GetClosestSidewalk();
                isValidSpawn = true;// IsValidSpawn(spawnLocation);
                timesTried++;
            }
            while (!spawnLocation.HasSpawns && !isValidSpawn && timesTried < 2);//10
            if (spawnLocation.HasSpawns && isValidSpawn)
            {
                Gang gang = null;
                if (gangID == "")
                {
                    gang = GetRandomGang(spawnLocation);
                }
                else
                {
                    gang = Gangs.GetGang(gangID);
                }
                if (gang != null)
                {
                    EntryPoint.WriteToConsole($"DISPATCHER: Attempting Gang Spawn for {gang.ID} spawnLocation.HasSidewalk {spawnLocation.HasSidewalk}", 3);
                    DispatchableVehicle VehicleType = null;
                    //VehicleType = gang.GetRandomVehicle(Player.WantedLevel, false, false, true);

                    if (!onFoot)
                    {

                        if (!spawnLocation.HasSidewalk || RandomItems.RandomPercent(10))
                        {
                            VehicleType = gang.GetRandomVehicle(Player.WantedLevel, false, false, true);
                        }
                    }

                    if (VehicleType != null || spawnLocation.HasSidewalk || onFoot)
                    {
                        EntryPoint.WriteToConsole($"DISPATCHER: Attempting Gang Spawn Vehicle? {VehicleType?.ModelName}", 3);
                        string RequiredGroup = "";
                        if (VehicleType != null)
                        {
                            RequiredGroup = VehicleType.RequiredPedGroup;
                        }
                        DispatchablePerson PersonType = gang.GetRandomPed(Player.WantedLevel, RequiredGroup);
                        if (PersonType != null)
                        {
                            EntryPoint.WriteToConsole($"DISPATCHER: Attempting Gang Spawn Person {PersonType.ModelName}", 3);
                            try
                            {
                                SpawnTask spawnTask = new SpawnTask(gang, spawnLocation, VehicleType, PersonType, Settings.SettingsManager.GangSettings.ShowSpawnedBlip, Settings, Weapons, Names, true, Crimes, PedGroups, ShopMenus, World) { AllowAnySpawn = true };// Settings.SettingsManager.Police.SpawnedAmbientPoliceHaveBlip);
                                spawnTask.AttemptSpawn();
                                foreach (PedExt created in spawnTask.CreatedPeople)
                                {
                                    World.Pedestrians.AddEntity(created);

                                }
                                //spawnTask.CreatedPeople.ForEach(x => World.AddEntity(x));
                                spawnTask.CreatedVehicles.ForEach(x => World.Vehicles.AddEntity(x, ResponseType.None));
                                HasDispatchedThisTick = true;
                            }
                            catch (Exception ex)
                            {
                                EntryPoint.WriteToConsole($"DISPATCHER: Spawn Gang ERROR {ex.Message} : {ex.StackTrace}", 0);
                            }
                        }
                    }
                }
            }
            else
            {
                EntryPoint.WriteToConsole($"DISPATCHER: Attempting to Spawn Gang Failed, Has Spawns {spawnLocation.HasSpawns} Is Valid {isValidSpawn}", 5);
            }
            GameTimeAttemptedDispatch = Game.GameTime;
        }
        return HasDispatchedThisTick;
    }
    
}