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
    private readonly ICountyJurisdictions CountyJurisdictions;
    private readonly IPoliceRespondable Player;
    private readonly int LikelyHoodOfAnySpawn = 5;
    private readonly float MinimumDeleteDistance = 150f;//200f
    private readonly uint MinimumExistingTime = 20000;
    private readonly ISettingsProvideable Settings;
    private readonly IStreets Streets;
    private readonly IEntityProvideable World;
    private readonly IZoneJurisdictions ZoneJurisdictions;
    private readonly IZones Zones;
    private uint GameTimeAttemptedDispatch;
    private uint GameTimeAttemptedRoadblock;
    private uint GameTimeAttemptedRecall;
    private uint GameTimeLastSpawnedRoadblock;
    private Blip Blip1;
    private Blip Blip2;
    private Blip Blip3;
    private Roadblock Roadblock;
    public Dispatcher(IEntityProvideable world, IPoliceRespondable player, IAgencies agencies, ISettingsProvideable settings, IStreets streets, IZones zones, ICountyJurisdictions countyJurisdictions, IZoneJurisdictions zoneJurisdictions)
    {
        Player = player;
        World = world;
        Agencies = agencies;
        Settings = settings;
        Streets = streets;
        Zones = zones;
        CountyJurisdictions = countyJurisdictions;
        ZoneJurisdictions = zoneJurisdictions;


        Blip1 = new Blip(Vector3.Zero, 1f);
        Blip1.Color = Color.Red;
        Blip2 = new Blip(Vector3.Zero, 1f);
        Blip1.Color = Color.Green;



    }
    private enum VanillaDispatchType //Only for disabling
    {
        PoliceAutomobile = 1,
        PoliceHelicopter = 2,
        FireDepartment = 3,
        SwatAutomobile = 4,
        AmbulanceDepartment = 5,
        PoliceRiders = 6,
        PoliceVehicleRequest = 7,
        PoliceRoadBlock = 8,
        PoliceAutomobileWaitPulledOver = 9,
        PoliceAutomobileWaitCruising = 10,
        Gangs = 11,
        SwatHelicopter = 12,
        PoliceBoat = 13,
        ArmyVehicle = 14,
        BikerBackup = 15
    };
    private float ClosestSpawnToOtherPoliceAllowed => Player.IsWanted ? 200f : 500f;
    private float ClosestSpawnToSuspectAllowed => Player.IsWanted ? 150f : 250f;
    private List<Cop> DeletableCops => World.PoliceList.Where(x => x.RecentlyUpdated && x.DistanceToPlayer >= MinimumDeleteDistance && x.HasBeenSpawnedFor >= MinimumExistingTime).ToList();
    private float DistanceToDelete => Player.IsWanted ? 600f : 1000f;
    private float DistanceToDeleteOnFoot => Player.IsWanted ? 125f : 1000f;
    private bool HasNeedToDispatch => World.TotalSpawnedCops < SpawnedCopLimit;
    private bool HasNeedToRoadblock => Player.WantedLevel >= 3 && Roadblock == null;
    private bool IsTimeToDispatch => Game.GameTime - GameTimeAttemptedDispatch >= TimeBetweenSpawn;
    private bool IsTimeToRoadblock => Game.GameTime - GameTimeLastSpawnedRoadblock >= TimeBetweenSpawn;
    private bool IsTimeToRecall => Game.GameTime - GameTimeAttemptedRecall >= TimeBetweenSpawn;
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
                return ((5 - Player.WantedLevel) * 2000) + 2000;
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
                return ((5 - Player.WantedLevel) * 10000) + 70000;//90 seconds at level 3?, 70 at level 5? sounds okay?
            }
        }
    }
    public void Dispatch()
    {
        if (IsTimeToDispatch && HasNeedToDispatch)
        {
            EntryPoint.WriteToConsole($"DISPATCHER: Attempting Spawn", 5);
            int timesTried = 0;
            SpawnLocation spawnLocation = new SpawnLocation();
            do
            {
                spawnLocation.InitialPosition = GetPositionAroundPlayer();
                spawnLocation.GetClosestStreet();
                Blip1.Position = spawnLocation.StreetPosition;
                Blip2.Position = spawnLocation.InitialPosition;
                timesTried++;
            }
            while (!spawnLocation.HasSpawns && !IsValidSpawn(spawnLocation) && timesTried < 10);
            if (spawnLocation.HasSpawns && IsValidSpawn(spawnLocation))
            {
                Agency agency = GetRandomAgency(spawnLocation);
                DispatchableVehicle VehicleType = agency.GetRandomVehicle(Player.WantedLevel, World.PoliceHelicoptersCount <= 2, World.PoliceBoatsCount <= 1,true);//turned off for now as i work on the AI//World.PoliceHelicoptersCount < Settings.SettingsManager.Police.HelicopterLimit, World.PoliceBoatsCount < Settings.SettingsManager.Police.BoatLimit);
                if (VehicleType != null)
                {
                    DispatchableOfficer OfficerType = agency.GetRandomPed(Player.WantedLevel, VehicleType.RequiredPassengerModels);
                    if (OfficerType != null)
                    {
                        try
                        {
                            SpawnTask spawnTask = new SpawnTask(agency, spawnLocation.InitialPosition, spawnLocation.StreetPosition, spawnLocation.Heading, VehicleType, OfficerType, Settings.SettingsManager.Police.SpawnedAmbientPoliceHaveBlip);
                            spawnTask.AttemptSpawn();
                            spawnTask.CreatedCops.ForEach(x => World.AddEntity(x));
                            spawnTask.CreatedVehicles.ForEach(x => World.AddEntity(x));
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
                EntryPoint.WriteToConsole($"DISPATCHER: Attempting to Spawn Failed, Has Spawns {spawnLocation.HasSpawns} Is Valid {IsValidSpawn(spawnLocation)}", 5);
            }
            GameTimeAttemptedDispatch = Game.GameTime;
        }
        if (IsTimeToRoadblock && HasNeedToRoadblock)
        {
            SpawnRegularRoadblock();
        }
        SetVanilla(false);//need to turn off vanilla gta 5 dispatch services nearly every tick?
    }
    public void Dispose()
    {

        if (Blip1.Exists())
        {
            Blip1.Delete();
        }
        if (Blip2.Exists())
        {
            Blip2.Delete();
        }

        SetVanilla(true);
    }
    public void Recall()
    {
        if (IsTimeToRecall)
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
            GameTimeAttemptedRecall = Game.GameTime;
        }
    }
    private void Delete(Cop Cop)
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
    private List<Agency> GetAgencies(Vector3 Position, int WantedLevel)
    {
        List<Agency> ToReturn = new List<Agency>();
        Street StreetAtPosition = Streets.GetStreet(Position);
        if (StreetAtPosition != null && StreetAtPosition.IsHighway) //Highway Patrol Jurisdiction
        {
            ToReturn.AddRange(Agencies.GetSpawnableHighwayAgencies(WantedLevel));
        }
        Zone CurrentZone = Zones.GetZone(Position);
        Agency ZoneAgency = ZoneJurisdictions.GetRandomAgency(CurrentZone.InternalGameName, WantedLevel);
        if (ZoneAgency != null)
        {
            ToReturn.Add(ZoneAgency); //Zone Jurisdiciton Random
        }
        if (!ToReturn.Any() || RandomItems.RandomPercent(LikelyHoodOfAnySpawn))//fall back to anybody
        {
            ToReturn.AddRange(Agencies.GetSpawnableAgencies(WantedLevel));
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
        if (Player.WantedLevel > 0 && Game.LocalPlayer.Character.IsInAnyVehicle(false))
        {
            Position = Game.LocalPlayer.Character.GetOffsetPositionFront(250f);//350f
        }
        else if (Player.Investigation.IsActive)
        {
            Position = Player.Investigation.Position;
        }
        else
        {
            Position = Game.LocalPlayer.Character.Position;
        }
        Position = Position.Around2D(MinDistanceToSpawn, MaxDistanceToSpawn);
        return Position;
    }
    private Agency GetRandomAgency(SpawnLocation spawnLocation)
    {
        Agency agency;
        List<Agency> PossibleAgencies = GetAgencies(spawnLocation.StreetPosition, Player.WantedLevel);
        agency = PossibleAgencies.PickRandom();
        if (agency == null)
        {
            agency = GetAgencies(spawnLocation.InitialPosition, Player.WantedLevel).PickRandom();
        }
        if (agency == null)
        {
            //EntryPoint.WriteToConsole("Dispatcher could not find Agency To Spawn");
        }
        return agency;
    }
    private Agency GetRandomAgency(Vector3 spawnLocation)
    {
        Agency agency;
        List<Agency> PossibleAgencies = GetAgencies(spawnLocation, Player.WantedLevel);
        agency = PossibleAgencies.PickRandom();
        if (agency == null)
        {
            agency = GetAgencies(spawnLocation, Player.WantedLevel).PickRandom();
        }
        if (agency == null)
        {
            //EntryPoint.WriteToConsole("Dispatcher could not find Agency To Spawn");
        }
        return agency;
    }
    private bool IsValidSpawn(SpawnLocation spawnLocation)
    {
        if (spawnLocation.StreetPosition.DistanceTo2D(Game.LocalPlayer.Character) < ClosestSpawnToSuspectAllowed || World.AnyCopsNearPosition(spawnLocation.StreetPosition, ClosestSpawnToOtherPoliceAllowed))
        {
            return false;
        }
        else if (spawnLocation.InitialPosition.DistanceTo2D(Game.LocalPlayer.Character) < ClosestSpawnToSuspectAllowed || World.AnyCopsNearPosition(spawnLocation.InitialPosition, ClosestSpawnToOtherPoliceAllowed))
        {
            return false;
        }
        return true;
    }
    private void SetVanilla(bool Enabled)
    {
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.PoliceAutomobile, Enabled);
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.PoliceHelicopter, Enabled);
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.PoliceVehicleRequest, Enabled);
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.SwatAutomobile, Enabled);
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.SwatHelicopter, Enabled);
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.PoliceRiders, Enabled);
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.PoliceRoadBlock, Enabled);
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.PoliceAutomobileWaitCruising, Enabled);
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.PoliceAutomobileWaitPulledOver, Enabled);
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
        else if (World.CountNearbyCops(cop.Pedestrian) >= 3 && cop.TimeBehindPlayer >= 15000) //Got Close and Then got away
        {
            //EntryPoint.WriteToConsole($"DISPATCHER: Recalling Cop {cop.Pedestrian.Handle} Reason: Behind Player Around Others");
            return true;
        }
        return false;
    }
    private void SpawnRegularRoadblock()
    {
        Vector3 Position = Player.Character.GetOffsetPositionFront(400f);
        Street ForwardStreet = Streets.GetStreet(Position);
        if (ForwardStreet?.Name == Player.CurrentLocation.CurrentStreet?.Name)
        {
            if (NativeFunction.Natives.GET_CLOSEST_VEHICLE_NODE_WITH_HEADING<bool>(Position.X, Position.Y, Position.Z, out Vector3 CenterPosition, out float Heading, 0, 3.0f, 0))
            {
                Agency ToSpawn = GetRandomAgency(CenterPosition);
                DispatchableVehicle VehicleToUse = ToSpawn.GetRandomVehicle(Player.WantedLevel, false, false, false);
                Roadblock = new Roadblock(Player, World, ToSpawn, VehicleToUse, CenterPosition);
                Roadblock.SpawnRoadblock();
                GameTimeLastSpawnedRoadblock = Game.GameTime;
                EntryPoint.WriteToConsole($"DISPATCHER: Spawned Roadblock {VehicleToUse.ModelName}", 3);
            }
        }
    }
}