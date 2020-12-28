using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

public class Dispatcher
{

    private readonly int LikelyHoodOfAnySpawn = 5;
    private IAgencies Agencies;
    private ICountyJurisdictions CountyJurisdictions;
    private IPlayer CurrentPlayer;
    private uint GameTimeCheckedDeleted;
    //private SpawnLocation CurrentSpawnLocation;
    private uint GameTimeCheckedSpawn;
    private float MinimumDeleteDistance = 200f;    //350f//will be a setting
    private uint MinimumExistingTime = 20000;//30000//will be a setting
    private IPoliceSight Police;
    private ISettings Settings;
    private ISpawner Spawner;
    private IStreets Streets;
    private IWeapons Weapons;
    private IWorld World;
    private IZoneJurisdictions ZoneJurisdictions;
    private IZones Zones;
    public Dispatcher(IWorld world, IPlayer currentPlayer, IPoliceSight police, ISpawner spawner, IAgencies agencies, IWeapons weapons, ISettings settings, IStreets streets, IZones zones, ICountyJurisdictions countyJurisdictions, IZoneJurisdictions zoneJurisdictions)
    {
        CurrentPlayer = currentPlayer;
        World = world;
        Police = police;
        Spawner = spawner;
        Agencies = agencies;
        Weapons = weapons;
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
    private bool CanRecall
    {
        get
        {
            if (GameTimeCheckedDeleted == 0)
            {
                return true;
            }
            else if (Game.GameTime - GameTimeCheckedDeleted >= TimeBetweenSpawn)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    private float ClosestSpawnToOtherPoliceAllowed
    {
        get
        {
            if (CurrentPlayer.IsWanted)
            {
                return 200f;
            }
            else
            {
                return 500f;
            }
        }
    }
    private float ClosestSpawnToSuspectAllowed
    {
        get
        {
            if (CurrentPlayer.IsWanted)
            {
                return 150f;
            }
            else
            {
                return 250f;
            }
        }
    }
    private float DistanceToDelete
    {
        get
        {
            if (CurrentPlayer.IsWanted)
            {
                return 600f;
            }
            else
            {
                return 1000f;
            }
        }
    }
    private float DistanceToDeleteOnFoot
    {
        get
        {
            if (CurrentPlayer.IsWanted)
            {
                return 125f;
            }
            else
            {
                return 1000f;
            }
        }
    }
    private float MaxDistanceToSpawn
    {
        get
        {
            if (CurrentPlayer.IsWanted)
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
            else if (CurrentPlayer.Investigations.IsActive)
            {
                return CurrentPlayer.Investigations.Distance;
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
            if (CurrentPlayer.IsWanted)
            {
                if (!Police.AnyRecentlySeenPlayer)
                {
                    return 250f - (CurrentPlayer.WantedLevel * -40);
                }
                else
                {
                    return 400f - (CurrentPlayer.WantedLevel * -40);
                }
            }
            else if (CurrentPlayer.Investigations.IsActive)
            {
                return CurrentPlayer.Investigations.Distance / 2;
            }
            else
            {
                return 350f;//450f;//750f
            }
        }
    }
    private bool NeedToDispatch
    {
        get
        {
            if (World.TotalSpawnedCops < SpawnedCopLimit)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    private bool ShouldCheckDispatch
    {
        get
        {
            if (GameTimeCheckedSpawn == 0)
            {
                return true;
            }
            else if (Game.GameTime - GameTimeCheckedSpawn >= TimeBetweenSpawn)
            {
                return true;
            }
            else
            {
                return false;
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
            if (CurrentPlayer.WantedLevel == 0)
            {
                return 5;//10//5
            }
            else if (CurrentPlayer.WantedLevel == 1)
            {
                return 7;//10//8
            }
            else if (CurrentPlayer.WantedLevel == 2)
            {
                return 10;//12
            }
            else if (CurrentPlayer.WantedLevel == 3)
            {
                return 18;//20
            }
            else if (CurrentPlayer.WantedLevel == 4)
            {
                return 25;
            }
            else if (CurrentPlayer.WantedLevel == 5)
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
                return ((5 - CurrentPlayer.WantedLevel) * 2000) + 2000;
            }
        }
    }
    public void Dispatch()
    {
        if (ShouldCheckDispatch && NeedToDispatch)
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
                PoliceSpawn policeSpawn = new PoliceSpawn(spawnLocation, agency, CurrentPlayer.WantedLevel, World.PoliceHelicoptersCount < Settings.SettingsManager.Police.HelicopterLimit, World.PoliceBoatsCount < Settings.SettingsManager.Police.BoatLimit, Settings.SettingsManager.Police.SpawnedAmbientPoliceHaveBlip);
                Spawner.Spawn(policeSpawn);
            }
            GameTimeCheckedSpawn = Game.GameTime;
        }
        SetVanilla(false);
    }
    public void Dispose()
    {
        SetVanilla(true);
    }
    public void Recall()
    {
        if (CanRecall)
        {
            foreach (Cop OutOfRangeCop in World.PoliceList.Where(x => x.RecentlyUpdated && x.DistanceToPlayer >= MinimumDeleteDistance && x.HasBeenSpawnedFor >= MinimumExistingTime))
            {
                bool ShouldDelete = false;
                if (!OutOfRangeCop.AssignedAgency.CanSpawn(CurrentPlayer.WantedLevel))
                {
                    ShouldDelete = true;
                }
                if (OutOfRangeCop.IsInVehicle && OutOfRangeCop.DistanceToPlayer > DistanceToDelete) //Beyond Caring
                {
                    ShouldDelete = true;
                }
                else if (!OutOfRangeCop.IsInVehicle && OutOfRangeCop.DistanceToPlayer > DistanceToDeleteOnFoot) //Beyond Caring
                {
                    ShouldDelete = true;
                }
                else if (OutOfRangeCop.ClosestDistanceToPlayer <= 15f) //Got Close and Then got away
                {
                    ShouldDelete = true;
                }
                else if (World.CountNearbyCops(OutOfRangeCop.Pedestrian) >= 3 && OutOfRangeCop.TimeBehindPlayer >= 15000) //Got Close and Then got away
                {
                    ShouldDelete = true;
                }
                else if (!OutOfRangeCop.AssignedAgency.CanSpawn(CurrentPlayer.WantedLevel))
                {
                    ShouldDelete = true;
                }
                if (ShouldDelete)
                {
                    Spawner.Delete(OutOfRangeCop);
                }
            }
            GameTimeCheckedDeleted = Game.GameTime;
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
        if (CurrentPlayer.WantedLevel > 0 && Game.LocalPlayer.Character.IsInAnyVehicle(false))
        {
            Position = Game.LocalPlayer.Character.GetOffsetPositionFront(250f);//350f
        }
        else if (CurrentPlayer.Investigations.IsActive)
        {
            Position = CurrentPlayer.Investigations.Position;
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
        List<Agency> PossibleAgencies = GetAgencies(spawnLocation.StreetPosition, CurrentPlayer.WantedLevel);
        agency = PossibleAgencies.PickRandom();
        if (agency == null)
        {
            agency = GetAgencies(spawnLocation.InitialPosition, CurrentPlayer.WantedLevel).PickRandom();
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
}
