using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Dispatch
{
    private static PoliceSpawn CurrentSpawn = new PoliceSpawn();
    private static uint GameTimeCheckedSpawn;
    private static uint GameTimeCheckedDeleted;
    private static int SpawnedCopLimit
    {
        get
        {
            return 10 + 2 * PlayerState.WantedLevel;
        }
    }
    public static int HelicopterLimit
    {
        get
        {
            int WantedLevelLimit = 4;
            if (PlayerState.WantedLevel <= WantedLevelLimit)
                return 0;
            else
                return PlayerState.WantedLevel - WantedLevelLimit + 1;
        }
    }
    private static bool CanSpawn
    {
        get
        {
            if (GameTimeCheckedSpawn == 0)
                return true;
            else if (Game.GameTime - GameTimeCheckedSpawn >= TimeBetweenSpawn)
                return true;
            else
                return false;
        }
    }
    private static bool CanDelete
    {
        get
        {
            if (GameTimeCheckedDeleted == 0)
                return true;
            else if (Game.GameTime - GameTimeCheckedDeleted >= TimeBetweenSpawn)
                return true;
            else
                return false;
        }
    }
    private static int TimeBetweenSpawn
    {
        get
        {
            if (PersonOfInterest.LethalForceAuthorized)
            {
                return 2000;
            }
            else
            {
                int InverseWanted = 5 - PlayerState.WantedLevel;
                return (InverseWanted * 2000) + 2000;
            }
        }
    }
    private static float ClosestSpawnToPlayerAllowed
    {
        get
        {
            if (PlayerState.IsWanted)
                return 150f;
            else
                return 250f;
        }
    }
    private static float ClosestSpawnToOtherPoliceAllowed
    {
        get
        {
            if (PlayerState.IsWanted)
                return 200f;
            else
                return 500f;
        }
    }
    private static float MinDistanceToSpawn
    {
        get
        {
            if (PlayerState.IsWanted)
                return 400f - (PlayerState.WantedLevel * -40);
            else if (Investigation.InInvestigationMode)
                return Investigation.InvestigationDistance / 2;
            else
                return 350f;//450f;//750f
        }
    }
    private static float MaxDistanceToSpawn
    {
        get
        {
            if (PlayerState.IsWanted)
                return 550f;
            else if (Investigation.InInvestigationMode)
                return Investigation.InvestigationDistance;
            else
                return 900f;//1250f//1500f
        }
    }
    private static float DistanceToDelete
    {
        get
        {
            if (PlayerState.IsWanted)
                return 600f;
            else
                return 1000f;
        }
    }
    private static bool NeedToSpawn
    {
        get
        {
            if (PedList.TotalSpawnedCops < SpawnedCopLimit)
                return true;
            else
                return false;
        }
    }
    private static uint MinimumExistingTime { get; set; } = 30000;
    private static float MinimumDeleteDistance { get; set; } = 350f;
    public static bool IsRunning { get; set; }
    public static void Initialize()
    {
        IsRunning = true;
    }
    public static void SpawnChecking()
    {
        if (IsRunning && CanSpawn)
        {
            CurrentSpawn.UpdateSpawnPosition();
            Debugging.WriteToLog("Dispatch", string.Format("Spawn Checking: Position {0},{1}", CurrentSpawn.Position, CurrentSpawn.StreetPosition));
            Zone DebugCurrentSpawnZone = Zones.GetZoneAtLocation(CurrentSpawn.Position);
            if(DebugCurrentSpawnZone != null)
                Debugging.WriteToLog("Dispatch", string.Format("Spawn Checking: Zone {0}", DebugCurrentSpawnZone.DisplayName));
            Zone DebugCurrentSpawnZoneStreet = Zones.GetZoneAtLocation(CurrentSpawn.StreetPosition);
            if (DebugCurrentSpawnZoneStreet != null)
                Debugging.WriteToLog("Dispatch", string.Format("Spawn Checking: Zone (Street) {0}", DebugCurrentSpawnZoneStreet.DisplayName));

            if (NeedToSpawn && CurrentSpawn.HasSpawns)
            {
                Agency AgencyToSpawn = Agencies.AgenciesAtPosition(CurrentSpawn.StreetPosition).PickRandom();

                if(AgencyToSpawn == null)
                {
                    AgencyToSpawn = Agencies.AgenciesAtPosition(CurrentSpawn.Position).PickRandom();
                }
                if (AgencyToSpawn == null)
                {
                    Debugging.WriteToLog("Dispatch", string.Format("Could not find Agencies To Spawn {0}", 1));
                    return;
                }
                Debugging.WriteToLog("Dispatch", string.Format("Spawn Checking: Agency {0}", AgencyToSpawn.Initials));

                Agency.VehicleInformation MyCarInfo = AgencyToSpawn.GetRandomVehicle();
                if (MyCarInfo == null)
                {
                    Debugging.WriteToLog("Dispatch", string.Format("Could not find Auto Info for {0}", AgencyToSpawn.Initials));
                    return;
                }
                Vector3 PositionToSpawn = CurrentSpawn.StreetPosition;
                Debugging.WriteToLog("Dispatch", string.Format("Spawn Checking: Vehcile {0}", MyCarInfo.ModelName));

                if (MyCarInfo.IsHelicopter)
                {
                    PositionToSpawn = CurrentSpawn.Position + new Vector3(0f, 0f, 250f);
                }
                else if (MyCarInfo.IsBoat && CurrentSpawn.IsWater)
                {
                    PositionToSpawn = CurrentSpawn.Position;
                }
                PoliceSpawning.SpawnGTACop(AgencyToSpawn, PositionToSpawn, CurrentSpawn.Heading, MyCarInfo);
            }
            GameTimeCheckedSpawn = Game.GameTime;
        }
    }
    public static void DeleteChecking()
    {
        if (IsRunning && CanDelete)
        {
            foreach(Cop OutOfRangeCop in PedList.CopPeds.Where(x => x.RecentlyUpdated && x.DistanceToPlayer >= MinimumDeleteDistance && x.HasBeenSpawnedFor >= MinimumExistingTime)) 
            {

                //Need to log which one of these i did
                //seems to be deletying too many people
                if(OutOfRangeCop.DistanceToPlayer > DistanceToDelete) //Beyond Caring
                {
                    Debugging.WriteToLog("Dispatch", string.Format("  Distance Delete {0}", OutOfRangeCop.Pedestrian.Handle));
                    PoliceSpawning.DeleteCop(OutOfRangeCop);
                }
                else if (OutOfRangeCop.ClosestDistanceToPlayer <= 15f) //Got Close and Then got away
                {
                    Debugging.WriteToLog("Dispatch", string.Format("  Close Then Far Delete {0}", OutOfRangeCop.Pedestrian.Handle));
                    PoliceSpawning.DeleteCop(OutOfRangeCop);
                }
                else if (OutOfRangeCop.CountNearbyCops >= 3 && OutOfRangeCop.TimeBehindPlayer >= 15000) //Got Close and Then got away
                {
                    Debugging.WriteToLog("Dispatch", string.Format("  NearbyCops Delete {0}", OutOfRangeCop.Pedestrian.Handle));
                    PoliceSpawning.DeleteCop(OutOfRangeCop);
                }
            }
            GameTimeCheckedDeleted = Game.GameTime;
        }
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    private class PoliceSpawn
    {
        public Vector3 Position = Vector3.Zero;
        public float Heading;
        public Vector3 StreetPosition = Vector3.Zero;
               
        public bool IsWater
        {
            get
            {
                if (NativeFunction.Natives.GET_WATER_HEIGHT<bool>(Position.X, Position.Y, Position.Z, out float height))
                {
                    if (height >= 2f)// has some water depth
                        return true;
                }
                return false;
            }
        }
        public bool HasSpawns
        {
            get
            {
                if (Position != Vector3.Zero && StreetPosition != Vector3.Zero)
                    return true;
                else
                    return false;

            }
        }
        //public Agency StreetAgency
        //{
        //    get
        //    {
        //        if (CurrentSpawn.StreetPosition != Vector3.Zero)
        //        {
        //            return Jurisdiction.AgencyAtZone(Zones.GetZoneAtLocation(CurrentSpawn.StreetPosition).InternalGameName);
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //}
        //public Agency PositionAgency
        //{
        //    get
        //    {
        //        if (CurrentSpawn.Position != Vector3.Zero)
        //        {
        //            return Jurisdiction.AgencyAtZone(Zones.GetZoneAtLocation(CurrentSpawn.Position).InternalGameName);
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //}
        //public Agency CountyAgency
        //{
        //    get
        //    {
        //        if (CurrentSpawn.Position != Vector3.Zero)
        //        {
        //            return Jurisdiction.AgencyAtCounty(Zones.GetZoneAtLocation(CurrentSpawn.Position).InternalGameName);
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //}

        public PoliceSpawn()
        {

        }
        public void UpdateSpawnPosition()
        {
            Position = Vector3.Zero;
            StreetPosition = Vector3.Zero;

            GetInitialPosition();
            GetStreetPosition();
        }
        private void GetInitialPosition()
        {
            if (PlayerState.IsWanted && Game.LocalPlayer.Character.IsInAnyVehicle(false))
                Position = Game.LocalPlayer.Character.GetOffsetPositionFront(350f);
            else
                Position = Game.LocalPlayer.Character.Position;

            Position = Position.Around2D(MinDistanceToSpawn, MaxDistanceToSpawn);


            if (PedList.AnyCopsNearPosition(Position, ClosestSpawnToOtherPoliceAllowed))
            {
                Position = Vector3.Zero;
            }
        }
        private void GetStreetPosition()
        {
            General.GetStreetPositionandHeading(Position, out StreetPosition, out Heading, true);

            if (StreetPosition.DistanceTo2D(Game.LocalPlayer.Character) < ClosestSpawnToPlayerAllowed)
                StreetPosition = Vector3.Zero;

            if(PedList.AnyCopsNearPosition(StreetPosition, ClosestSpawnToOtherPoliceAllowed))
                StreetPosition = Vector3.Zero;
        }
    }

}
