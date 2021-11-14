using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

public class LEDispatcher
{
    private readonly IAgencies Agencies;
    private readonly IDispatchable Player;
    private readonly int LikelyHoodOfAnySpawn = 5;
    private readonly int LikelyHoodOfCountySpawn = 10;
    private readonly float MinimumDeleteDistance = 150f;//200f
    private readonly uint MinimumExistingTime = 20000;
    private readonly ISettingsProvideable Settings;
    private readonly IStreets Streets;
    private readonly IEntityProvideable World;
    private readonly IJurisdictions Jurisdictions;
    private readonly IZones Zones;
    private bool HasDispatchedThisTick;
    private uint GameTimeAttemptedDispatch;
    private uint GameTimeAttemptedDispatchRoadblock;
    private uint GameTimeAttemptedRecall;
    private uint GameTimeLastSpawnedRoadblock;
    private Roadblock Roadblock;
    private Agency LastAgencySpawned;
    private IWeapons Weapons;
    private int TotalWantedLevel;
    private bool TotalIsWanted;
    public LEDispatcher(IEntityProvideable world, IDispatchable player, IAgencies agencies, ISettingsProvideable settings, IStreets streets, IZones zones, IJurisdictions jurisdictions, IWeapons weapons)
    {
        Player = player;
        World = world;
        Agencies = agencies;
        Settings = settings;
        Streets = streets;
        Zones = zones;
        Jurisdictions = jurisdictions;
        Weapons = weapons;
    }

    private float ClosestPoliceSpawnToOtherPoliceAllowed => TotalIsWanted ? 200f : 500f;
    private float ClosestPoliceSpawnToSuspectAllowed => TotalIsWanted ? 150f : 250f;
    private List<Cop> DeletableCops => World.PoliceList.Where(x => (x.RecentlyUpdated && x.DistanceToPlayer >= MinimumDeleteDistance && x.HasBeenSpawnedFor >= MinimumExistingTime) || x.CanRemove).ToList();
    private float DistanceToDelete => TotalIsWanted ? 600f : 1000f;
    private float DistanceToDeleteOnFoot => TotalIsWanted ? 125f : 1000f;
    private bool HasNeedToDispatch => World.TotalSpawnedPolice < SpawnedCopLimit;
    private bool HasNeedToDispatchRoadblock => Player.WantedLevel >= Settings.SettingsManager.PoliceSettings.RoadblockMinWantedLevel && Player.WantedLevel <= Settings.SettingsManager.PoliceSettings.RoadblockMaxWantedLevel && Roadblock == null;//roadblocks are only for player
    public void SpawnCop(Vector3 position)
    {
        Vector3 spawnLocation = position;
        Agency agency = GetRandomAgency(spawnLocation, ResponseType.LawEnforcement);
        GameFiber.Yield();
        if (agency != null)
        {
            DispatchableVehicle VehicleType = agency.GetRandomVehicle(TotalWantedLevel, World.PoliceHelicoptersCount <= 2, World.PoliceBoatsCount <= 1, true);//turned off for now as i work on the AI//World.PoliceHelicoptersCount < Settings.SettingsManager.Police.HelicopterLimit, World.PoliceBoatsCount < Settings.SettingsManager.Police.BoatLimit);
            GameFiber.Yield();
            if (VehicleType != null)
            {
                DispatchablePerson OfficerType = agency.GetRandomPed(TotalWantedLevel, VehicleType.RequiredPassengerModels);
                GameFiber.Yield();
                if (OfficerType != null)
                {
                    try
                    {
                        SpawnTask spawnTask = new SpawnTask(agency, spawnLocation, spawnLocation, 0f, VehicleType, OfficerType, Settings.SettingsManager.PoliceSettings.ShowSpawnedBlips, Settings, Weapons);
                        spawnTask.AttemptSpawn();
                        GameFiber.Yield();
                        spawnTask.CreatedPeople.ForEach(x => World.AddEntity(x));
                        spawnTask.CreatedVehicles.ForEach(x => World.AddEntity(x, ResponseType.LawEnforcement));
                    }
                    catch (Exception ex)
                    {
                        EntryPoint.WriteToConsole($"DISPATCHER: SpawnCop ERROR {ex.Message} : {ex.StackTrace}", 0);
                    }
                }
            }
        }
    }

    public void SpawnHelicopterCop(Vector3 position)
    {
        Vector3 spawnLocation = position;
        Agency agency = Agencies.GetAgency("LSPD-ASD");// GetRandomAgency(spawnLocation, ResponseType.LawEnforcement);
        GameFiber.Yield();
        if (agency != null)
        {
            DispatchableVehicle VehicleType = agency.GetRandomVehicle(TotalWantedLevel, true, false, false);//turned off for now as i work on the AI//World.PoliceHelicoptersCount < Settings.SettingsManager.Police.HelicopterLimit, World.PoliceBoatsCount < Settings.SettingsManager.Police.BoatLimit);
            GameFiber.Yield();
            if (VehicleType != null)
            {
                DispatchablePerson OfficerType = agency.GetRandomPed(TotalWantedLevel, VehicleType.RequiredPassengerModels);
                GameFiber.Yield();
                if (OfficerType != null)
                {
                    try
                    {
                        SpawnTask spawnTask = new SpawnTask(agency, spawnLocation, spawnLocation, 0f, VehicleType, OfficerType, Settings.SettingsManager.PoliceSettings.ShowSpawnedBlips, Settings, Weapons);
                        spawnTask.AttemptSpawn();
                        GameFiber.Yield();
                        spawnTask.CreatedPeople.ForEach(x => World.AddEntity(x));
                        spawnTask.CreatedVehicles.ForEach(x => World.AddEntity(x, ResponseType.LawEnforcement));
                    }
                    catch (Exception ex)
                    {
                        EntryPoint.WriteToConsole($"DISPATCHER: SpawnCop ERROR {ex.Message} : {ex.StackTrace}", 0);
                    }
                }
            }
        }
    }
    private bool IsTimeToDispatch => Game.GameTime - GameTimeAttemptedDispatch >= TimeBetweenSpawn;
    private bool IsTimeToDispatchRoadblock => Game.GameTime - GameTimeLastSpawnedRoadblock >= TimeBetweenRoadblocks;
    private bool IsTimeToRecall => Game.GameTime - GameTimeAttemptedRecall >= TimeBetweenSpawn;
    private float MaxDistanceToSpawn
    {
        get
        {
            if (TotalIsWanted)
            {
                if (!Player.AnyPoliceRecentlySeenPlayer)
                {
                    return 350f;
                }
                else
                {
                    return 550f;
                }
            }
            else if (Player.Investigation.IsActive)
            {
                return Player.Investigation.Distance;
            }
            else
            {
                return 900f;//1250f//1500f
            }
        }
    }
    private float MinDistanceToSpawn
    {
        get
        {
            if (TotalIsWanted)
            {
                if (!Player.AnyPoliceRecentlySeenPlayer)
                {
                    return 250f - (TotalWantedLevel * -40);
                }
                else
                {
                    return 400f - (TotalWantedLevel * -40);
                }
            }
            else if (Player.Investigation.IsActive)
            {
                return Player.Investigation.Distance / 2;
            }
            else
            {
                return 350f;//450f;//750f
            }
        }
    }
    private int SpawnedCopLimit
    {
        get
        {
            if (TotalWantedLevel == 5)
            {
                return Settings.SettingsManager.PoliceSettings.SpawnLimit_Wanted5;//35;//35
            }
            else if (TotalWantedLevel == 4)
            {
                return Settings.SettingsManager.PoliceSettings.SpawnLimit_Wanted4;//25;//25
            }
            else if (TotalWantedLevel == 3)
            {
                return Settings.SettingsManager.PoliceSettings.SpawnLimit_Wanted3;//18;//18
            }
            else if (TotalWantedLevel == 2)
            {
                return Settings.SettingsManager.PoliceSettings.SpawnLimit_Wanted2;//10;// 12;//10
            }
            else if (TotalWantedLevel == 1)
            {
                return Settings.SettingsManager.PoliceSettings.SpawnLimit_Wanted1;//7;// 10;//7
            }
            else if (Player.Investigation.IsActive)
            {
                return Settings.SettingsManager.PoliceSettings.SpawnLimit_Investigation;//6;// 9;//6
            }
            if (TotalWantedLevel == 0)
            {
                return Settings.SettingsManager.PoliceSettings.SpawnLimit_Default;//5;// 8;//5
            }
            else
            {
                return Settings.SettingsManager.PoliceSettings.SpawnLimit_Default;//5;//15
            }
        }
    }
    private int TimeBetweenSpawn
    {
        get
        {
            if (!Player.AnyPoliceRecentlySeenPlayer)
            {
                return 3000;
            }
            else
            {
                return ((5 - TotalWantedLevel) * 2000) + 2000;
            }
        }
    }
    private int TimeBetweenRoadblocks
    {
        get
        {
            if (!Player.AnyPoliceRecentlySeenPlayer)
            {
                return 999999;
            }
            else
            {
                return ((5 - TotalWantedLevel) * Settings.SettingsManager.PoliceSettings.RoadblockWantedLevelAdditionalTimeScaler) + Settings.SettingsManager.PoliceSettings.MinTimeBetweenRoadblocks;//90 seconds at level 3?, 70 at level 5? sounds okay?
            }
        }
    }
    public bool Dispatch()
    {
        HasDispatchedThisTick = false;

        PedExt worstPed = World.CivilianList.OrderByDescending(x=>x.WantedLevel).FirstOrDefault();
        if(worstPed != null && worstPed.WantedLevel > Player.WantedLevel)
        {
            TotalWantedLevel = worstPed.WantedLevel;
        }
        else
        {
            TotalWantedLevel = Player.WantedLevel;
        }
        TotalIsWanted = TotalWantedLevel > 0;

        if (Settings.SettingsManager.PoliceSettings.ManageDispatching && IsTimeToDispatch && HasNeedToDispatch)
        {
            HasDispatchedThisTick = true;//up here for now, might be better down low
            EntryPoint.WriteToConsole($"DISPATCHER: Attempting LE Spawn IsWanted: {TotalIsWanted} WantedLevel:{TotalWantedLevel}", 3);
            int timesTried = 0;
            bool isValidSpawn = false;
            SpawnLocation spawnLocation = new SpawnLocation();
            do
            {
                spawnLocation.InitialPosition = GetPositionAroundPlayer();
                spawnLocation.GetClosestStreet();
                isValidSpawn = IsValidSpawn(spawnLocation);
                timesTried++;
                GameFiber.Yield();
            }
            while (!spawnLocation.HasSpawns && !isValidSpawn && timesTried < 1);//2//10
            if (spawnLocation.HasSpawns && isValidSpawn)
            {
                Agency agency = GetRandomAgency(spawnLocation, ResponseType.LawEnforcement);
                GameFiber.Yield();
                if (agency != null)
                {
                    LastAgencySpawned = agency;
                    DispatchableVehicle VehicleType = agency.GetRandomVehicle(TotalWantedLevel, World.PoliceHelicoptersCount <= 2, World.PoliceBoatsCount <= 1, true);//turned off for now as i work on the AI//World.PoliceHelicoptersCount < Settings.SettingsManager.Police.HelicopterLimit, World.PoliceBoatsCount < Settings.SettingsManager.Police.BoatLimit);
                    GameFiber.Yield();
                    if (VehicleType != null)
                    {
                        DispatchablePerson OfficerType = agency.GetRandomPed(TotalWantedLevel, VehicleType.RequiredPassengerModels);
                        GameFiber.Yield();
                        if (OfficerType != null)
                        {
                            try
                            {
                                SpawnTask spawnTask = new SpawnTask(agency, spawnLocation.InitialPosition, spawnLocation.StreetPosition, spawnLocation.Heading, VehicleType, OfficerType, Settings.SettingsManager.PoliceSettings.ShowSpawnedBlips, Settings, Weapons);
                                spawnTask.AttemptSpawn();
                                GameFiber.Yield();
                                spawnTask.CreatedPeople.ForEach(x => World.AddEntity(x));
                                spawnTask.CreatedVehicles.ForEach(x => World.AddEntity(x, ResponseType.LawEnforcement));
                                HasDispatchedThisTick = true;
                                Player.OnLawEnforcementSpawn(agency, VehicleType, OfficerType);
                            }
                            catch (Exception ex)
                            {
                                EntryPoint.WriteToConsole($"DISPATCHER: SpawnCop ERROR {ex.Message} : {ex.StackTrace}", 0);
                            }
                        }
                    }
                }
            }
            else
            {
                EntryPoint.WriteToConsole($"DISPATCHER: Attempting to Spawn LE Failed, Has Spawns {spawnLocation.HasSpawns} Is Valid {isValidSpawn}", 3);
            }
            GameTimeAttemptedDispatch = Game.GameTime;
        }
        if (IsTimeToDispatchRoadblock && HasNeedToDispatchRoadblock)
        {
            //to be readd :(
            SpawnRoadblock();
        }
        if (LastAgencySpawned != null)
        {
            Player.DebugLine11 = $"Roadblock: {Roadblock != null}; LastAgencySpawned: {LastAgencySpawned.ID}";
        }
        else
        {
            Player.DebugLine11 = $"Roadblock: {Roadblock != null}";
        }
        return HasDispatchedThisTick;
    }
    public void Dispose()
    {
        RemoveRoadblock();
    }
    public void Recall()
    {
        if (Settings.SettingsManager.PoliceSettings.ManageDispatching && IsTimeToRecall)
        {
            foreach (Cop DeleteableCop in DeletableCops)
            {
                if (ShouldCopBeRecalled(DeleteableCop))
                {
                    Delete(DeleteableCop);
                    GameFiber.Yield();
                }
            }
            if (Roadblock != null && Player.Position.DistanceTo2D(Roadblock.CenterPosition) >= 550f)
            {
                Roadblock.Dispose();
                Roadblock = null;
                EntryPoint.WriteToConsole($"DISPATCHER: Deleted Roadblock", 3);
            }
            GameTimeAttemptedRecall = Game.GameTime;
        }
    }
    private void Delete(PedExt Cop)
    {
        if (Cop != null && Cop.Pedestrian.Exists())
        {
            //EntryPoint.WriteToConsole($"Attempting to Delete {Cop.Pedestrian.Handle}");
            if (Cop.Pedestrian.IsInAnyVehicle(false))
            {
                if (Cop.Pedestrian.CurrentVehicle.HasPassengers)
                {
                    foreach (Ped Passenger in Cop.Pedestrian.CurrentVehicle.Passengers)
                    {
                        RemoveBlip(Passenger);
                        Passenger.Delete();
                    }
                }
                if (Cop.Pedestrian.Exists() && Cop.Pedestrian.CurrentVehicle.Exists() && Cop.Pedestrian.CurrentVehicle != null)
                {
                    Cop.Pedestrian.CurrentVehicle.Delete();
                }
            }
            RemoveBlip(Cop.Pedestrian);
            if (Cop.Pedestrian.Exists())
            {
                //EntryPoint.WriteToConsole(string.Format("Delete Cop Handle: {0}, {1}, {2}", Cop.Pedestrian.Handle, Cop.DistanceToPlayer, Cop.AssignedAgency.Initials));
                Cop.Pedestrian.Delete();
            }
        }
    }
    private void RemoveBlip(Ped MyPed)
    {
        if (!MyPed.Exists())
        {
            return;
        }
        Blip MyBlip = MyPed.GetAttachedBlip();
        if (MyBlip.Exists())
        {
            MyBlip.Delete();
        }
    }
    private List<Agency> GetAgencies(Vector3 Position, int WantedLevel, ResponseType responseType)
    {
        List<Agency> ToReturn = new List<Agency>();
        Street StreetAtPosition = Streets.GetStreet(Position);
        if (StreetAtPosition != null && StreetAtPosition.IsHighway) //Highway Patrol Jurisdiction
        {
            ToReturn.AddRange(Agencies.GetSpawnableHighwayAgencies(WantedLevel, responseType));
        }
        Zone CurrentZone = Zones.GetZone(Position);
        Agency ZoneAgency = Jurisdictions.GetRandomAgency(CurrentZone.InternalGameName, WantedLevel, responseType);
        if (ZoneAgency != null)
        {
            ToReturn.Add(ZoneAgency); //Zone Jurisdiciton Random
        }
        if (!ToReturn.Any() || RandomItems.RandomPercent(LikelyHoodOfAnySpawn))//fall back to anybody
        {
            ToReturn.AddRange(Agencies.GetSpawnableAgencies(WantedLevel, responseType));
        }
        if (ZoneAgency == null || RandomItems.RandomPercent(LikelyHoodOfCountySpawn))
        {
            Agency CountyAgency = Jurisdictions.GetRandomAgency(CurrentZone.ZoneCounty, WantedLevel, ResponseType.LawEnforcement);
            if (CountyAgency != null)//randomly spawn the county agency
            {
                ToReturn.Add(CountyAgency); //Zone Jurisdiciton Random
            }
        }
        foreach (Agency ag in ToReturn)
        {
            //EntryPoint.WriteToConsole(string.Format("Debugging: Agencies At Pos: {0}", ag.Initials));
        }
        return ToReturn;
    }
    private Vector3 GetPositionAroundPlayer()
    {
        Vector3 Position;
        if (TotalWantedLevel > 0 && Player.IsInVehicle)
        {
            Position = Player.Character.GetOffsetPositionFront(250f);//350f
        }
        else if (Player.Investigation.IsActive)
        {
            Position = Player.Investigation.Position;
        }
        else
        {
            Position = Player.Position;
        }
        Position = Position.Around2D(MinDistanceToSpawn, MaxDistanceToSpawn);
        return Position;
    }
    private Agency GetRandomAgency(SpawnLocation spawnLocation, ResponseType responseType)
    {
        Agency agency;
        List<Agency> PossibleAgencies = GetAgencies(spawnLocation.StreetPosition, TotalWantedLevel, responseType);
        agency = PossibleAgencies.PickRandom();
        if (agency == null)
        {
            agency = GetAgencies(spawnLocation.InitialPosition, TotalWantedLevel,responseType).PickRandom();
        }
        if (agency == null)
        {
            //EntryPoint.WriteToConsole("Dispatcher could not find Agency To Spawn");
        }
        return agency;
    }
    private Agency GetRandomAgency(Vector3 spawnLocation, ResponseType responseType)
    {
        Agency agency;
        List<Agency> PossibleAgencies = GetAgencies(spawnLocation, TotalWantedLevel, responseType);
        agency = PossibleAgencies.PickRandom();
        if (agency == null)
        {
            agency = GetAgencies(spawnLocation, TotalWantedLevel,responseType).PickRandom();
        }
        if (agency == null)
        {
            //EntryPoint.WriteToConsole("Dispatcher could not find Agency To Spawn");
        }
        return agency;
    }
    private bool IsValidSpawn(SpawnLocation spawnLocation)
    {
        if (spawnLocation.StreetPosition.DistanceTo2D(Player.Position) < ClosestPoliceSpawnToSuspectAllowed || World.AnyCopsNearPosition(spawnLocation.StreetPosition, ClosestPoliceSpawnToOtherPoliceAllowed))
        {
            return false;
        }
        else if (spawnLocation.InitialPosition.DistanceTo2D(Player.Position) < ClosestPoliceSpawnToSuspectAllowed || World.AnyCopsNearPosition(spawnLocation.InitialPosition, ClosestPoliceSpawnToOtherPoliceAllowed))
        {
            return false;
        }
        return true;
    }
    private bool ShouldCopBeRecalled(Cop cop)
    {
        if (!cop.AssignedAgency.CanSpawn(TotalWantedLevel))
        {
            EntryPoint.WriteToConsole($"DISPATCHER: Recalling Cop {cop.Pedestrian.Handle} Reason: Agency Can Not Spawn",5);
            return true;
        }
        else if (cop.IsInVehicle && cop.DistanceToPlayer > DistanceToDelete) //Beyond Caring
        {
            EntryPoint.WriteToConsole($"DISPATCHER: Recalling Cop {cop.Pedestrian.Handle} Reason: Beyond Distance (Vehicle)",5);
            return true;
        }
        else if (!cop.IsInVehicle && cop.DistanceToPlayer > DistanceToDeleteOnFoot) //Beyond Caring
        {
            EntryPoint.WriteToConsole($"DISPATCHER: Recalling Cop {cop.Pedestrian.Handle} Reason: Beyond Distance (Foot)",5);
            return true;
        }
        else if (cop.ClosestDistanceToPlayer <= 15f && !cop.IsInHelicopter) //Got Close and Then got away
        {
            EntryPoint.WriteToConsole($"DISPATCHER: Recalling Cop {cop.Pedestrian.Handle} Reason: Was Close",5);
            return true;
        }
        //else if (World.CountNearbyPolice(cop.Pedestrian) >= 3 && cop.TimeBehindPlayer >= 15000) //Got Close and Then got away
        //{
        //    //EntryPoint.WriteToConsole($"DISPATCHER: Recalling Cop {cop.Pedestrian.Handle} Reason: Behind Player Around Others");
        //    return true;
        //}
        return false;
    }
    public void SpawnRoadblock()//temp public
    {
        EntryPoint.WriteToConsole($"DISPATCHER: Attempting Roadblock", 3);
        Vector3 Position = Player.Character.GetOffsetPositionFront(350f);//400f 400 is mostly far enough to not see it
        Street ForwardStreet = Streets.GetStreet(Position);
        if (ForwardStreet?.Name == Player.CurrentLocation.CurrentStreet?.Name)
        {
            if (NativeFunction.Natives.GET_CLOSEST_VEHICLE_NODE_WITH_HEADING<bool>(Position.X, Position.Y, Position.Z, out Vector3 CenterPosition, out float Heading, 0, 3.0f, 0))
            {
                Agency ToSpawn = GetRandomAgency(CenterPosition, ResponseType.LawEnforcement);
                if (ToSpawn != null)
                {
                    DispatchableVehicle VehicleToUse = ToSpawn.GetRandomVehicle(TotalWantedLevel, false, false, false);
                    if (VehicleToUse != null)
                    {
                        DispatchablePerson OfficerType = ToSpawn.GetRandomPed(TotalWantedLevel, VehicleToUse.RequiredPassengerModels);
                        if (OfficerType != null)
                        {
                            if (Roadblock != null)
                            {
                                Roadblock.Dispose();
                            }
                            Roadblock = new Roadblock(Player, World, ToSpawn, VehicleToUse, OfficerType, CenterPosition, Settings, Weapons);
                            Roadblock.SpawnRoadblock();
                            GameTimeLastSpawnedRoadblock = Game.GameTime;
                            EntryPoint.WriteToConsole($"DISPATCHER: Spawned Roadblock {VehicleToUse.ModelName}", 3);
                        }
                    }
                }
            }
        }
    }
    public void RemoveRoadblock()//temp public
    {
        if (Roadblock != null)
        {
            Roadblock.Dispose();
            Roadblock = null;
            EntryPoint.WriteToConsole($"DISPATCHER: Deleted Roadblock", 3);
        }
    }
}