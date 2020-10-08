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
    public static int SpawnedCopLimit
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
            else if (Game.GameTime - GameTimeCheckedSpawn >= 5000)
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
            else if (PlayerState.IsWanted && Game.GameTime - GameTimeCheckedDeleted >= 2000)
                return true;
            else if (Game.GameTime - GameTimeCheckedDeleted >= 5000)
                return true;
            else
                return false;
        }
    }
    public static bool IsRunning { get; set; }
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
    public static float MinDistanceToSpawn
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
    public static float MaxDistanceToSpawn
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
    public static float DistanceToDelete
    {
        get
        {
            if (PlayerState.IsWanted)
                return 600f;
            else
                return 1000f;
        }
    }

    public static bool NeedToSpawn
    {
        get
        {
            if (PedList.TotalSpawnedCops < SpawnedCopLimit)
                return true;
            else
                return false;
        }
    }
    public static bool NeedToDelete
    {
        get
        {
            if (PedList.TotalSpawnedCops > SpawnedCopLimit)
                return true;
            else
                return false;
        }
    }

    public static void Initialize()
    {
        IsRunning = true;
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void Tick()
    {
        if (IsRunning)
        {
            SpawnChecking();
            DeleteChecking();
        }
    }
    public static void SpawnChecking()
    {
        if (IsRunning && CanSpawn)
        {
            CurrentSpawn.UpdateSpawnPosition();
            if (NeedToSpawn)
            {
                PoliceSpawning.SpawnGTACop(CurrentSpawn.StreetAgency, CurrentSpawn.StreetPosition, CurrentSpawn.Heading);
            }
            GameTimeCheckedSpawn = Game.GameTime;
        }
    }
    public static void DeleteChecking()
    {
        if (IsRunning && CanDelete)
        {
           // if (NeedToDelete)
           // {
                PoliceSpawning.DeleteCop(PedList.CopPeds.Where(x=>x.DistanceToPlayer >= DistanceToDelete).OrderByDescending(x =>x.DistanceToPlayer).FirstOrDefault());
           // }
            GameTimeCheckedDeleted = Game.GameTime;
        }
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
        public Agency StreetAgency
        {
            get
            {
                if (CurrentSpawn.StreetPosition != Vector3.Zero)
                {
                    return Jurisdiction.AgencyAtZone(Zones.GetZoneAtLocation(CurrentSpawn.StreetPosition).InternalGameName);
                }
                else
                {
                    return null;
                }
            }
        }
        public Agency PositionAgency
        {
            get
            {
                if (CurrentSpawn.Position != Vector3.Zero)
                {
                    return Jurisdiction.AgencyAtZone(Zones.GetZoneAtLocation(CurrentSpawn.Position).InternalGameName);
                }
                else
                {
                    return null;
                }
            }
        }

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
                Position = Vector3.Zero;
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
