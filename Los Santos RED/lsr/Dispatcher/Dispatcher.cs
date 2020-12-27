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

    private PoliceSpawn CurrentSpawn;
    private uint GameTimeCheckedSpawn;
    private uint GameTimeCheckedDeleted;
    private uint MinimumExistingTime = 20000;//30000//will be a setting
    private float MinimumDeleteDistance = 200f;//350f//will be a setting
    private IPlayer CurrentPlayer;
    private IWorld World;
    private IPoliceSight Police;
    private ISpawner Spawner;
    private IAgencies Agencies;
    private IWeapons Weapons;
    private ISettings Settings;
    private IStreets Streets;
    private ICountyJurisdictions CountyJurisdictions;
    private IZoneJurisdictions ZoneJurisdictions;
    private IZones Zones;
    private readonly int LikelyHoodOfAnySpawn = 5;
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
    private bool CanDispatch
    {
        get
        {
            if (GameTimeCheckedSpawn == 0)
            {
                return true;
            }
            //else if (CurrentPlayer.Investigations.IsActive && !World.Tasking.HasCopsInvestigating)//maybe add this back, maybe not? breaks lots of over reach....
            //{
            //    return true;
            //}
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
    private bool NeedToSpawn
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
    public void Dispose()
    {
        SetVanilla(true);
    }
    public void Dispatch()
    {
        if (CanDispatch && NeedToSpawn)
        {
            CurrentSpawn = new PoliceSpawn(GetInitialPosition(), MinDistanceToSpawn, MaxDistanceToSpawn, ClosestSpawnToSuspectAllowed);
            int TimesTried = 0;
            while (!CurrentSpawn.HasSpawns && TimesTried < 10)
            {
                CurrentSpawn.GetStreetPosition();
                if (CurrentSpawn.StreetPosition.DistanceTo2D(Game.LocalPlayer.Character) < ClosestSpawnToSuspectAllowed || World.AnyCopsNearPosition(CurrentSpawn.StreetPosition, ClosestSpawnToOtherPoliceAllowed))
                {
                    CurrentSpawn.StreetPosition = Vector3.Zero;
                }
                TimesTried++;
            }
            if (CurrentSpawn.HasSpawns)
            {
                AttemptSpawn();
            }
            GameTimeCheckedSpawn = Game.GameTime;
        }
        SetVanilla(false);
    }
    private void AttemptSpawn()
    {
        DetermineAgency();
        if (!CurrentSpawn.HasAgency)
        {
            Game.Console.Print("Dispatcher could not find Agency To Spawn");
            return;
        }
        bool IncludeHelicopters = false;
        if(World.PoliceHelicoptersCount < Settings.SettingsManager.Police.HelicopterLimit)
        {
            IncludeHelicopters = true;
        }
        bool IncludeBoats = false;
        if (World.PoliceBoatsCount < Settings.SettingsManager.Police.BoatLimit)
        {
            IncludeBoats = true;
        }
        CurrentSpawn.VehicleToSpawn = CurrentSpawn.AgencyToSpawn.GetRandomVehicle(CurrentPlayer.WantedLevel, IncludeHelicopters, IncludeBoats);
        if (CurrentSpawn.VehicleToSpawn == null)
        {
            Game.Console.Print(string.Format("Dispatcher could not find vehicle to spawn for {0}", CurrentSpawn.AgencyToSpawn.Initials));
            return;
        }
        GetFinalPosition();
        AgencyAssignedWeapon AssignedPistolType = CurrentSpawn.AgencyToSpawn.IssuedWeapons.Where(x => x.IsPistol).PickRandom();
        AgencyAssignedWeapon AssignedHeavyType = CurrentSpawn.AgencyToSpawn.IssuedWeapons.Where(x => !x.IsPistol).PickRandom();
        Spawner.SpawnCop(CurrentSpawn.AgencyToSpawn, CurrentSpawn.FinalSpawnPosition, CurrentSpawn.Heading, CurrentSpawn.VehicleToSpawn, CurrentPlayer.WantedLevel, Settings.SettingsManager.Police.SpawnedAmbientPoliceHaveBlip, Weapons.GetWeapon(AssignedPistolType.ModelName), AssignedPistolType.Variation, Weapons.GetWeapon(AssignedHeavyType.ModelName), AssignedHeavyType.Variation);
    }
    private void DetermineAgency()
    {
        List<Agency> PossibleAgencies = GetAgencies(CurrentSpawn.StreetPosition, CurrentPlayer.WantedLevel);
        CurrentSpawn.AgencyToSpawn = PossibleAgencies.PickRandom();
        if (CurrentSpawn.AgencyToSpawn == null)
        {
            CurrentSpawn.AgencyToSpawn = GetAgencies(CurrentSpawn.Position, CurrentPlayer.WantedLevel).PickRandom();
        }
    }
    private Vector3 GetInitialPosition()
    {
        Vector3 Position;
        if (CurrentPlayer.WantedLevel > 0 && Game.LocalPlayer.Character.IsInAnyVehicle(false))
        {
            Position = Game.LocalPlayer.Character.GetOffsetPositionFront(350f);
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
        if (!CurrentPlayer.Investigations.IsActive && World.AnyCopsNearPosition(Position, ClosestSpawnToOtherPoliceAllowed))
        {
            Position = Vector3.Zero;
        }
        return Position;
    }
    private void GetFinalPosition()
    {
        if (CurrentSpawn.VehicleToSpawn.IsHelicopter)
        {
            CurrentSpawn.FinalSpawnPosition = CurrentSpawn.Position + new Vector3(0f, 0f, 250f);
            Game.Console.Print(string.Format("Dispatching Helicopter: {0}", CurrentSpawn.VehicleToSpawn.ModelName));
        }
        else if (CurrentSpawn.VehicleToSpawn.IsBoat)
        {
            CurrentSpawn.FinalSpawnPosition = CurrentSpawn.Position;
            Game.Console.Print(string.Format("Dispatching Boat: {0} isWater {1} WaterHieght {2}", CurrentSpawn.VehicleToSpawn.ModelName, CurrentSpawn.IsWater, CurrentSpawn.WaterHeight));
        }
        else
        {
            CurrentSpawn.FinalSpawnPosition = CurrentSpawn.StreetPosition;
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
                    Spawner.DeleteCop(OutOfRangeCop);
                }
            }
            GameTimeCheckedDeleted = Game.GameTime;
        }
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
    private class PoliceSpawn
    {
        private float MinDistanceToSpawn;
        private float MaxDistanceToSpawn;
        private float ClosestSpawnToSuspectAllowed;
        public Vector3 Position { get; set; } = Vector3.Zero;
        public Vector3 StreetPosition { get; set; } = Vector3.Zero;
        public Vector3 FinalSpawnPosition { get; set; } = Vector3.Zero;
        public float Heading { get; set; }
        public Agency AgencyToSpawn { get; set; }
        public VehicleInformation VehicleToSpawn { get; set; }
        public bool IsWater
        {
            get
            {
                if (NativeFunction.Natives.GET_WATER_HEIGHT<bool>(Position.X, Position.Y, Position.Z, out float height))
                {
                    if (height >= 0.5f)//2f// has some water depth
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        public float WaterHeight
        {
            get
            {
                if (NativeFunction.Natives.GET_WATER_HEIGHT<bool>(Position.X, Position.Y, Position.Z, out float height))
                {
                    return height;
                }
                return height;
            }
        }
        public bool HasSpawns
        {
            get
            {
                if (Position != Vector3.Zero && StreetPosition != Vector3.Zero)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
        public bool HasAgency
        {
            get
            {
                if (AgencyToSpawn == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public PoliceSpawn(Vector3 initialPosition, float minDistanceToSpawn, float maxDistanceToSpawn, float closestSpawnToSuspectAllowed)
        {
            Position = initialPosition;
            StreetPosition = Vector3.Zero;
            MinDistanceToSpawn = minDistanceToSpawn;
            MaxDistanceToSpawn = maxDistanceToSpawn;
            ClosestSpawnToSuspectAllowed = closestSpawnToSuspectAllowed;
        }
        public void GetStreetPosition()
        {
            Vector3 streetPos;
            float heading;
            GetStreetPositionandHeading(Position, out streetPos, out heading, true);
            StreetPosition = streetPos;
            Heading = heading;
        }
        private void GetStreetPositionandHeading(Vector3 PositionNear, out Vector3 SpawnPosition, out float Heading, bool MainRoadsOnly)
        {
            Vector3 pos = PositionNear;
            SpawnPosition = Vector3.Zero;
            Heading = 0f;

            Vector3 outPos;
            float heading;
            float val;

            if (MainRoadsOnly)
            {
                unsafe
                {
                    NativeFunction.CallByName<bool>("GET_CLOSEST_VEHICLE_NODE_WITH_HEADING", pos.X, pos.Y, pos.Z, &outPos, &heading, 0, 3, 0);
                }

                SpawnPosition = outPos;
                Heading = heading;
            }
            else
            {
                for (int i = 1; i < 40; i++)
                {
                    unsafe
                    {
                        NativeFunction.CallByName<bool>("GET_NTH_CLOSEST_VEHICLE_NODE_WITH_HEADING", pos.X, pos.Y, pos.Z, i, &outPos, &heading, &val, 1, 0x40400000, 0);
                    }
                    if (!NativeFunction.CallByName<bool>("IS_POINT_OBSCURED_BY_A_MISSION_ENTITY", outPos.X, outPos.Y, outPos.Z, 5.0f, 5.0f, 5.0f, 0))
                    {
                        SpawnPosition = outPos;
                        Heading = heading;
                        break;
                    }
                }
            }
        }
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
}
