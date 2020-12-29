using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

public class Dispatcher
{
    private readonly IAgencies Agencies;
    private readonly ICountyJurisdictions CountyJurisdictions;
    private readonly IPlayer Player;
    private readonly int LikelyHoodOfAnySpawn = 5;
    private readonly float MinimumDeleteDistance = 200f;
    private readonly uint MinimumExistingTime = 20000;
    private readonly IPoliceSight Police;
    private readonly ISettings Settings;
    private readonly IStreets Streets;
    private readonly IWorld World;
    private readonly IZoneJurisdictions ZoneJurisdictions;
    private readonly IZones Zones;
    private uint GameTimeAttemptedDispatch;
    private uint GameTimeAttemptedRecall;
    public Dispatcher(IWorld world, IPlayer currentPlayer, IPoliceSight police, IAgencies agencies, ISettings settings, IStreets streets, IZones zones, ICountyJurisdictions countyJurisdictions, IZoneJurisdictions zoneJurisdictions)
    {
        Player = currentPlayer;
        World = world;
        Police = police;
        Agencies = agencies;
        Settings = settings;
        Streets = streets;
        Zones = zones;
        CountyJurisdictions = countyJurisdictions;
        ZoneJurisdictions = zoneJurisdictions;
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
    private bool IsTimeToDispatch => Game.GameTime - GameTimeAttemptedDispatch >= TimeBetweenSpawn;
    private bool IsTimeToRecall => Game.GameTime - GameTimeAttemptedRecall >= TimeBetweenSpawn;
    private float MaxDistanceToSpawn
    {
        get
        {
            if (Player.IsWanted)
            {
                if (!Police.AnyRecentlySeenPlayer)
                {
                    return 350f;
                }
                else
                {
                    return 550f;
                }
            }
            else if (Player.Investigations.IsActive)
            {
                return Player.Investigations.Distance;
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
                if (!Police.AnyRecentlySeenPlayer)
                {
                    return 250f - (Player.WantedLevel * -40);
                }
                else
                {
                    return 400f - (Player.WantedLevel * -40);
                }
            }
            else if (Player.Investigations.IsActive)
            {
                return Player.Investigations.Distance / 2;
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
            //if (CurrentPlayer.Investigations.IsActive)
            //{
            //    return 6;//10
            //}
            if (Player.WantedLevel == 0)
            {
                return 5;//10//5
            }
            else if (Player.WantedLevel == 1)
            {
                return 7;//10//8
            }
            else if (Player.WantedLevel == 2)
            {
                return 10;//12
            }
            else if (Player.WantedLevel == 3)
            {
                return 18;//20
            }
            else if (Player.WantedLevel == 4)
            {
                return 25;
            }
            else if (Player.WantedLevel == 5)
            {
                return 35;
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
            if (!Police.AnyRecentlySeenPlayer)
            {
                return 3000;
            }
            else
            {
                return ((5 - Player.WantedLevel) * 2000) + 2000;
            }
        }
    }
    public void Dispatch()
    {
        if (IsTimeToDispatch && HasNeedToDispatch)
        {
            int timesTried = 0;
            SpawnLocation spawnLocation = new SpawnLocation();
            do
            {
                spawnLocation.InitialPosition = GetPositionAroundPlayer();
                spawnLocation.GetClosestStreet();
                timesTried++;
            }
            while (!spawnLocation.HasSpawns && !IsValidSpawn(spawnLocation) && timesTried < 10);
            if (spawnLocation.HasSpawns)
            {
                Agency agency = GetRandomAgency(spawnLocation);
                DispatchableVehicle VehicleType = agency.GetRandomVehicle(Player.WantedLevel, World.PoliceHelicoptersCount < Settings.SettingsManager.Police.HelicopterLimit, World.PoliceBoatsCount < Settings.SettingsManager.Police.BoatLimit);
                if(VehicleType != null)
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
                            Game.Console.Print("SpawnCop " + ex.Message + " : " + ex.StackTrace);
                        }
                    }
                }

            }
            GameTimeAttemptedDispatch = Game.GameTime;
        }
        SetVanilla(false);//need to turn off vanilla gta 5 dispatch services nearly every tick?
    }
    public void Dispose()
    {
        SetVanilla(true);
    }
    public void Recall()
    {
        if (IsTimeToRecall)
        {
            foreach (Cop DeleteableCop in DeletableCops)
            {
                if (ShouldCopBeRecalled(DeleteableCop))
                {
                    Delete(DeleteableCop);
                }
            }
            GameTimeAttemptedRecall = Game.GameTime;
        }
    }
    private void Delete(Cop Cop)
    {
        if (Cop != null && Cop.Pedestrian.Exists())
        {
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
                Game.Console.Print(string.Format("Delete Cop Handle: {0}, {1}, {2}", Cop.Pedestrian.Handle, Cop.DistanceToPlayer, Cop.AssignedAgency.Initials));
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
        if (ZoneAgency == null || RandomItems.RandomPercent(10))
        {
            Agency CountyAgency = CountyJurisdictions.GetRandomAgency(CurrentZone.ZoneCounty, WantedLevel);
            if (CountyAgency != null)//randomly spawn the county agency
            {
                ToReturn.Add(CountyAgency); //Zone Jurisdiciton Random
            }
        }
        if (!ToReturn.Any() || RandomItems.RandomPercent(LikelyHoodOfAnySpawn))//fall back to anybody
        {
            ToReturn.AddRange(Agencies.GetSpawnableAgencies(WantedLevel));
        }
        foreach (Agency ag in ToReturn)
        {
            Game.Console.Print(string.Format("Debugging: Agencies At Pos: {0}", ag.Initials));
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
        else if (Player.Investigations.IsActive)
        {
            Position = Player.Investigations.Position;
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
            Game.Console.Print("Dispatcher could not find Agency To Spawn");
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
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)VanillaDispatchType.PoliceAutomobile, Enabled);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)VanillaDispatchType.PoliceHelicopter, Enabled);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)VanillaDispatchType.PoliceVehicleRequest, Enabled);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)VanillaDispatchType.SwatAutomobile, Enabled);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)VanillaDispatchType.SwatHelicopter, Enabled);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)VanillaDispatchType.PoliceRiders, Enabled);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)VanillaDispatchType.PoliceRoadBlock, Enabled);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)VanillaDispatchType.PoliceAutomobileWaitCruising, Enabled);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)VanillaDispatchType.PoliceAutomobileWaitPulledOver, Enabled);
    }
    private bool ShouldCopBeRecalled(Cop cop)
    {
        if (!cop.AssignedAgency.CanSpawn(Player.WantedLevel))
        {
            return true;
        }
        else if (cop.IsInVehicle && cop.DistanceToPlayer > DistanceToDelete) //Beyond Caring
        {
            return true;
        }
        else if (!cop.IsInVehicle && cop.DistanceToPlayer > DistanceToDeleteOnFoot) //Beyond Caring
        {
            return true;
        }
        else if (cop.ClosestDistanceToPlayer <= 15f) //Got Close and Then got away
        {
            return true;
        }
        else if (World.CountNearbyCops(cop.Pedestrian) >= 3 && cop.TimeBehindPlayer >= 15000) //Got Close and Then got away
        {
            return true;
        }
        return false;
    }
}