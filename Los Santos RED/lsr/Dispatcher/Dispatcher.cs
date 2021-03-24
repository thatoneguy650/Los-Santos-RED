using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

public class Dispatcher
{
    private readonly IAgencies Agencies;
    private readonly IDispatchable Player;
    private readonly int LikelyHoodOfAnySpawn = 5;
    private readonly float MinimumDeleteDistance = 150f;//200f
    private readonly uint MinimumExistingTime = 20000;
    private readonly ISettingsProvideable Settings;
    private readonly IStreets Streets;
    private readonly IEntityProvideable World;
    private readonly IJurisdictions Jurisdictions;
    private readonly IZones Zones;
    private bool HasDispatchedThisTick;

    private uint GameTimeAttemptedDispatchLE;
    private uint GameTimeAttemptedDispatchEMS;
    private uint GameTimeAttemptedDispatchFire;
    private uint GameTimeAttemptedDispatchRoadblock;

    private uint GameTimeAttemptedRecallLE;
    private uint GameTimeAttemptedRecallEMS;
    private uint GameTimeAttemptedRecallFire;
    private uint GameTimeLastSpawnedRoadblock;

    private Roadblock Roadblock;

    private Agency LastAgencySpawned;
    public Dispatcher(IEntityProvideable world, IDispatchable player, IAgencies agencies, ISettingsProvideable settings, IStreets streets, IZones zones, IJurisdictions jurisdictions)
    {
        Player = player;
        World = world;
        Agencies = agencies;
        Settings = settings;
        Streets = streets;
        Zones = zones;
        Jurisdictions = jurisdictions;
    }
    private DispatchParameters LawEnforcement = new DispatchParameters();
    private float ClosestPoliceSpawnToOtherPoliceAllowed => Player.IsWanted ? 200f : 500f;
    private float ClosestPoliceSpawnToSuspectAllowed => Player.IsWanted ? 150f : 250f;
    private List<Cop> DeletableCops => World.PoliceList.Where(x => x.RecentlyUpdated && x.DistanceToPlayer >= MinimumDeleteDistance && x.HasBeenSpawnedFor >= MinimumExistingTime).ToList();
    private List<EMT> DeletableEMTs => World.EMTList.Where(x => x.RecentlyUpdated && x.DistanceToPlayer >= MinimumDeleteDistance && x.HasBeenSpawnedFor >= MinimumExistingTime).ToList();
    private List<Firefighter> DeletableFIrefighters => World.FirefighterList.Where(x => x.RecentlyUpdated && x.DistanceToPlayer >= MinimumDeleteDistance && x.HasBeenSpawnedFor >= MinimumExistingTime).ToList();
    private float DistanceToDelete => Player.IsWanted ? 600f : 1000f;
    private float DistanceToDeleteOnFoot => Player.IsWanted ? 125f : 1000f;
    private bool HasNeedToDispatchLE => World.TotalSpawnedPolice < SpawnedCopLimit;
    private bool HasNeedToDispatchEMS => World.TotalSpawnedEMTs < 2;
    private bool HasNeedToDispatchFire => World.TotalSpawnedFirefighters < 2;
    private bool HasNeedToDispatchRoadblock => Player.WantedLevel >= 3 && Roadblock == null;
    private bool IsTimeToDispatchLE => Game.GameTime - GameTimeAttemptedDispatchLE >= TimeBetweenLESpawn;
    private bool IsTimeToDispatchEMS => Game.GameTime - GameTimeAttemptedDispatchEMS >= 60000;
    private bool IsTimeToDispatchFire => Game.GameTime - GameTimeAttemptedDispatchFire >= 60000;
    private bool IsTimeToDispatchRoadblock => Game.GameTime - GameTimeLastSpawnedRoadblock >= TimeBetweenLESpawn;
    private bool IsTimeToRecallLE => Game.GameTime - GameTimeAttemptedRecallLE >= TimeBetweenLESpawn;
    private bool IsTimeToRecallEMS => Game.GameTime - GameTimeAttemptedRecallEMS >= TimeBetweenEMSSpawn;
    private bool IsTimeToRecallFire => Game.GameTime - GameTimeAttemptedRecallFire >= TimeBetweenFireSpawn;//thes need to be classes!!!
    private float MaxDistanceToSpawn
    {
        get
        {
            if (Player.IsWanted)
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
            if (Player.IsWanted)
            {
                if (!Player.AnyPoliceRecentlySeenPlayer)
                {
                    return 250f - (Player.WantedLevel * -40);
                }
                else
                {
                    return 400f - (Player.WantedLevel * -40);
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
            if (Player.Investigation.IsActive)
            {
                return 9;//6
            }
            if (Player.WantedLevel == 0)
            {
                return 8;//5
            }
            else if (Player.WantedLevel == 1)
            {
                return 10;//7
            }
            else if (Player.WantedLevel == 2)
            {
                return 12;//10
            }
            else if (Player.WantedLevel == 3)
            {
                return 18;//18
            }
            else if (Player.WantedLevel == 4)
            {
                return 25;//25
            }
            else if (Player.WantedLevel == 5)
            {
                return 35;//35
            }
            else
            {
                return 5;//15
            }
        }
    }
    private int TimeBetweenLESpawn
    {
        get
        {
            if (!Player.AnyPoliceRecentlySeenPlayer)
            {
                return 3000;
            }
            else
            {
                return ((5 - Player.WantedLevel) * 2000) + 2000;
            }
        }
    }
    private int TimeBetweenEMSSpawn => 60000;
    private int TimeBetweenFireSpawn => 60000;
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
                return ((5 - Player.WantedLevel) * 10000) + 70000;//90 seconds at level 3?, 70 at level 5? sounds okay?
            }
        }
    }
    public void Dispatch()
    {
        HasDispatchedThisTick = false;
        DispatchLawEnforcement();
        if(!HasDispatchedThisTick)//for now
        {
            DispatchEMS();
        }
        
        if(!HasDispatchedThisTick)
        {
            DispatchFire();
        }
        
    }
    public void Dispose()
    {

    }
    public void Recall()
    {
        if (IsTimeToRecallLE)
        {
            //EntryPoint.WriteToConsole($"DISPATCHER: Attempting Recall");
            foreach (Cop DeleteableCop in DeletableCops)
            {
                if (ShouldCopBeRecalled(DeleteableCop))
                {
                    Delete(DeleteableCop);
                }
            }
            if (Roadblock != null && Player.Position.DistanceTo2D(Roadblock.CenterPosition) >= 550f)
            {
                Roadblock.Dispose();
                Roadblock = null;
                EntryPoint.WriteToConsole($"DISPATCHER: Deleted Roadblock", 3);
            }




            GameTimeAttemptedRecallLE = Game.GameTime;
        }
        if(IsTimeToRecallEMS)
        {
            foreach (EMT emt in DeletableEMTs)
            {
                if (emt.DistanceToPlayer >= DistanceToDelete)
                {
                    Delete(emt);
                }
            }
            GameTimeAttemptedRecallEMS = Game.GameTime;
        }
        if (IsTimeToRecallFire)
        {
            foreach (Firefighter firefighter in DeletableFIrefighters)
            {
                if (firefighter.DistanceToPlayer >= DistanceToDelete)
                {
                    Delete(firefighter);
                }
            }
            GameTimeAttemptedRecallFire = Game.GameTime;
        }
    }
    private void DispatchLawEnforcement()
    {
        if (IsTimeToDispatchLE && HasNeedToDispatchLE)
        {
            EntryPoint.WriteToConsole($"DISPATCHER: Attempting LE Spawn", 5);
            int timesTried = 0;
            bool isValidSpawn = false;
            SpawnLocation spawnLocation = new SpawnLocation();
            do
            {
                spawnLocation.InitialPosition = GetPositionAroundPlayer();
                spawnLocation.GetClosestStreet();
                isValidSpawn = IsValidSpawn(spawnLocation);
                timesTried++;
            }
            while (!spawnLocation.HasSpawns && !isValidSpawn && timesTried < 2);//10
            if (spawnLocation.HasSpawns && isValidSpawn)
            {
                Agency agency = GetRandomAgency(spawnLocation, ResponseType.LawEnforcement);
                LastAgencySpawned = agency;
                DispatchableVehicle VehicleType = agency.GetRandomVehicle(Player.WantedLevel, World.PoliceHelicoptersCount <= 2, World.PoliceBoatsCount <= 1, true);//turned off for now as i work on the AI//World.PoliceHelicoptersCount < Settings.SettingsManager.Police.HelicopterLimit, World.PoliceBoatsCount < Settings.SettingsManager.Police.BoatLimit);
                if (VehicleType != null)
                {
                    DispatchablePerson OfficerType = agency.GetRandomPed(Player.WantedLevel, VehicleType.RequiredPassengerModels);
                    if (OfficerType != null)
                    {
                        try
                        {
                            SpawnTask spawnTask = new SpawnTask(agency, spawnLocation.InitialPosition, spawnLocation.StreetPosition, spawnLocation.Heading, VehicleType, OfficerType, Settings.SettingsManager.Police.SpawnedAmbientPoliceHaveBlip);
                            spawnTask.AttemptSpawn();
                            spawnTask.CreatedPeople.ForEach(x => World.AddEntity(x));
                            spawnTask.CreatedVehicles.ForEach(x => World.AddEntity(x));
                            HasDispatchedThisTick = true;
                        }
                        catch (Exception ex)
                        {
                            EntryPoint.WriteToConsole($"DISPATCHER: SpawnCop ERROR {ex.Message} : {ex.StackTrace}", 0);
                        }
                    }
                }
            }
            else
            {
                EntryPoint.WriteToConsole($"DISPATCHER: Attempting to Spawn LE Failed, Has Spawns {spawnLocation.HasSpawns} Is Valid {isValidSpawn}", 5);
            }
            GameTimeAttemptedDispatchLE = Game.GameTime;
        }
        //if (IsTimeToRoadblock && HasNeedToRoadblock)
        //{
        //    //to be readd :(
        //    SpawnRegularRoadblock();
        //}
        Player.DebugLine11 = $"Roadblock: {Roadblock != null}; LastAgencySpawned: {LastAgencySpawned.ID}";
    }
    private void DispatchEMS()
    {
        if (IsTimeToDispatchEMS && HasNeedToDispatchEMS)
        {
            EntryPoint.WriteToConsole($"DISPATCHER: Attempting EMS Spawn", 3);
            int timesTried = 0;
            bool isValidSpawn = false;
            SpawnLocation spawnLocation = new SpawnLocation();
            do
            {
                spawnLocation.InitialPosition = GetPositionAroundPlayer();
                spawnLocation.GetClosestStreet();
                isValidSpawn = IsValidSpawn(spawnLocation);
                timesTried++;
            }
            while (!spawnLocation.HasSpawns && !isValidSpawn && timesTried < 2);//10
            if (spawnLocation.HasSpawns && isValidSpawn)
            {
                Agency agency = GetRandomAgency(spawnLocation, ResponseType.EMS);
                if (agency != null)
                {
                    LastAgencySpawned = agency;
                    EntryPoint.WriteToConsole($"DISPATCHER: Attempting EMS Spawn for {agency.ID}", 3);
                    DispatchableVehicle VehicleType = agency.GetRandomVehicle(Player.WantedLevel, false, false, false);
                    if (VehicleType != null)
                    {
                        EntryPoint.WriteToConsole($"DISPATCHER: Attempting EMS Spawn Vehicle {VehicleType.ModelName}", 3);
                        DispatchablePerson PersonType = agency.GetRandomPed(Player.WantedLevel, VehicleType.RequiredPassengerModels);
                        if (PersonType != null)
                        {
                            EntryPoint.WriteToConsole($"DISPATCHER: Attempting EMS Spawn Vehicle {PersonType.ModelName}", 3);
                            try
                            {
                                SpawnTask spawnTask = new SpawnTask(agency, spawnLocation.InitialPosition, spawnLocation.StreetPosition, spawnLocation.Heading, VehicleType, PersonType, Settings.SettingsManager.Police.SpawnedAmbientPoliceHaveBlip);
                                spawnTask.AttemptSpawn();
                                spawnTask.CreatedPeople.ForEach(x => World.AddEntity(x));
                                spawnTask.CreatedVehicles.ForEach(x => World.AddEntity(x));
                                HasDispatchedThisTick = true;
                            }
                            catch (Exception ex)
                            {
                                EntryPoint.WriteToConsole($"DISPATCHER: Spawn EMS ERROR {ex.Message} : {ex.StackTrace}", 0);
                            }
                        }
                    }
                }
            }
            else
            {
                EntryPoint.WriteToConsole($"DISPATCHER: Attempting to Spawn EMS Failed, Has Spawns {spawnLocation.HasSpawns} Is Valid {isValidSpawn}", 5);
            }
            GameTimeAttemptedDispatchEMS = Game.GameTime;
        }
    }
    private void DispatchFire()
    {
        if (IsTimeToDispatchFire && HasNeedToDispatchFire)
        {
            EntryPoint.WriteToConsole($"DISPATCHER: Attempting Fire Spawn", 3);
            int timesTried = 0;
            bool isValidSpawn = false;
            SpawnLocation spawnLocation = new SpawnLocation();
            do
            {
                spawnLocation.InitialPosition = GetPositionAroundPlayer();
                spawnLocation.GetClosestStreet();
                isValidSpawn = IsValidSpawn(spawnLocation);
                timesTried++;
            }
            while (!spawnLocation.HasSpawns && !isValidSpawn && timesTried < 2);//10
            if (spawnLocation.HasSpawns && isValidSpawn)
            {
                Agency agency = GetRandomAgency(spawnLocation, ResponseType.Fire);
                LastAgencySpawned = agency;
                DispatchableVehicle VehicleType = agency.GetRandomVehicle(Player.WantedLevel, false, false, false);
                if (VehicleType != null)
                {
                    DispatchablePerson PersonType = agency.GetRandomPed(Player.WantedLevel, VehicleType.RequiredPassengerModels);
                    if (PersonType != null)
                    {
                        try
                        {
                            SpawnTask spawnTask = new SpawnTask(agency, spawnLocation.InitialPosition, spawnLocation.StreetPosition, spawnLocation.Heading, VehicleType, PersonType, Settings.SettingsManager.Police.SpawnedAmbientPoliceHaveBlip);
                            spawnTask.AttemptSpawn();
                            spawnTask.CreatedPeople.ForEach(x => World.AddEntity(x));
                            spawnTask.CreatedVehicles.ForEach(x => World.AddEntity(x));
                            HasDispatchedThisTick = true;
                        }
                        catch (Exception ex)
                        {
                            EntryPoint.WriteToConsole($"DISPATCHER: Spawn Fire ERROR {ex.Message} : {ex.StackTrace}", 0);
                        }
                    }
                }
            }
            else
            {
                EntryPoint.WriteToConsole($"DISPATCHER: Attempting to Spawn Fire Failed, Has Spawns {spawnLocation.HasSpawns} Is Valid {isValidSpawn}", 5);
            }
            GameTimeAttemptedDispatchFire = Game.GameTime;
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
        //if (ZoneAgency == null || RandomItems.RandomPercent(10))
        //{
        //    Agency CountyAgency = CountyJurisdictions.GetRandomAgency(CurrentZone.ZoneCounty, WantedLevel);
        //    if (CountyAgency != null)//randomly spawn the county agency
        //    {
        //        ToReturn.Add(CountyAgency); //Zone Jurisdiciton Random
        //    }
        //}
        foreach (Agency ag in ToReturn)
        {
            //EntryPoint.WriteToConsole(string.Format("Debugging: Agencies At Pos: {0}", ag.Initials));
        }
        return ToReturn;
    }
    private Vector3 GetPositionAroundPlayer()
    {
        Vector3 Position;
        if (Player.WantedLevel > 0 && Player.IsInVehicle)
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
        List<Agency> PossibleAgencies = GetAgencies(spawnLocation.StreetPosition, Player.WantedLevel, responseType);
        agency = PossibleAgencies.PickRandom();
        if (agency == null)
        {
            agency = GetAgencies(spawnLocation.InitialPosition, Player.WantedLevel,responseType).PickRandom();
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
        List<Agency> PossibleAgencies = GetAgencies(spawnLocation, Player.WantedLevel,responseType);
        agency = PossibleAgencies.PickRandom();
        if (agency == null)
        {
            agency = GetAgencies(spawnLocation, Player.WantedLevel,responseType).PickRandom();
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
        if (!cop.AssignedAgency.CanSpawn(Player.WantedLevel))
        {
            //EntryPoint.WriteToConsole($"DISPATCHER: Recalling Cop {cop.Pedestrian.Handle} Reason: Agency Can Not Spawn");
            return true;
        }
        else if (cop.IsInVehicle && cop.DistanceToPlayer > DistanceToDelete) //Beyond Caring
        {
            //EntryPoint.WriteToConsole($"DISPATCHER: Recalling Cop {cop.Pedestrian.Handle} Reason: Beyond Distance (Vehicle)");
            return true;
        }
        else if (!cop.IsInVehicle && cop.DistanceToPlayer > DistanceToDeleteOnFoot) //Beyond Caring
        {
            //EntryPoint.WriteToConsole($"DISPATCHER: Recalling Cop {cop.Pedestrian.Handle} Reason: Beyond Distance (Foot)");
            return true;
        }
        else if (cop.ClosestDistanceToPlayer <= 15f) //Got Close and Then got away
        {
            //EntryPoint.WriteToConsole($"DISPATCHER: Recalling Cop {cop.Pedestrian.Handle} Reason: Was Close");
            return true;
        }
        else if (World.CountNearbyPolice(cop.Pedestrian) >= 3 && cop.TimeBehindPlayer >= 15000) //Got Close and Then got away
        {
            //EntryPoint.WriteToConsole($"DISPATCHER: Recalling Cop {cop.Pedestrian.Handle} Reason: Behind Player Around Others");
            return true;
        }
        return false;
    }
    public void SpawnRegularRoadblock()//temp public
    {
        Vector3 Position = Player.Character.GetOffsetPositionFront(400f);
        Street ForwardStreet = Streets.GetStreet(Position);
        if (ForwardStreet?.Name == Player.CurrentLocation.CurrentStreet?.Name)
        {
            if (NativeFunction.Natives.GET_CLOSEST_VEHICLE_NODE_WITH_HEADING<bool>(Position.X, Position.Y, Position.Z, out Vector3 CenterPosition, out float Heading, 0, 3.0f, 0))
            {
                Agency ToSpawn = GetRandomAgency(CenterPosition,ResponseType.LawEnforcement);
                DispatchableVehicle VehicleToUse = ToSpawn.GetRandomVehicle(Player.WantedLevel, false, false, false);

                if(Roadblock != null)
                {
                    Roadblock.Dispose();
                }

                Roadblock = new Roadblock(Player, World, ToSpawn, VehicleToUse, CenterPosition);
                Roadblock.SpawnRoadblock();
                GameTimeLastSpawnedRoadblock = Game.GameTime;
                EntryPoint.WriteToConsole($"DISPATCHER: Spawned Roadblock {VehicleToUse.ModelName}", 3);
            }
        }
    }
}