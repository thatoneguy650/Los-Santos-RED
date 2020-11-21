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
            if (Investigation.InInvestigationMode)
                return 10;
            else if(PlayerState.WantedLevel == 0)
            {
                return 5;//10//5
            }
            else if (PlayerState.WantedLevel == 1)
            {
                return 7;//10//8
            }
            else if (PlayerState.WantedLevel == 2)
            {
                return 10;//12
            }
            else if (PlayerState.WantedLevel == 3)
            {
                return 18;//20
            }
            else if (PlayerState.WantedLevel == 4)
            {
                return 25;
            }
            else if (PlayerState.WantedLevel == 5)
            {
                return 35;
            }
            else
            {
                return 15;
            }
            //return 10 + 2 * PlayerState.WantedLevel;
        }
    }
    //private static int SpawnedVehicleLimit
    //{
    //    get
    //    {
    //        return 10 + 2 * PlayerState.WantedLevel;
    //    }
    //}
    private static bool CanSpawn
    {
        get
        {
            if (GameTimeCheckedSpawn == 0)
                return true;
            else if (Investigation.InInvestigationMode && !NewTasking.HasCopsInvestigating)
                return true;
            else if (Game.GameTime - GameTimeCheckedSpawn >= TimeBetweenSpawn)
                return true;
            else
                return false;
        }
    }
    private static bool CanSpawnPedestrianOfficers
    {
        get
        {
            if (PlayerState.IsWanted)
                return false;
            else
                return true;
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
            else  if (!Police.AnyRecentlySeenPlayer)
            {
                return 3000;
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
            {
                if (!Police.AnyRecentlySeenPlayer)
                    return 250f - (PlayerState.WantedLevel * -40);
                else
                    return 400f - (PlayerState.WantedLevel * -40);
            }
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
            {
                if (!Police.AnyRecentlySeenPlayer)
                    return 350f;
                else
                    return 550f;
            }
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
    private static float DistanceToDeleteOnFoot
    {
        get
        {
            if (PlayerState.IsWanted)
                return 125f;
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
    private static float MinimumDeleteDistance{ get; set; } = 125f;//350f

    private static int PercentagePedestrainOfficers { get; set; } = 5;
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

            Zone CurrentSpawnZone = Zones.GetZoneAtLocation(CurrentSpawn.Position);
            Zone CurrentSpawnZoneStreet = Zones.GetZoneAtLocation(CurrentSpawn.StreetPosition);

            if (NeedToSpawn && CurrentSpawn.HasSpawns)
            {
                //First Try Street
                List<Agency> PossibleAgencies = Agencies.AgenciesAtPosition(CurrentSpawn.StreetPosition);
                Agency HeliAgency = PossibleAgencies.Where(x => x.HasSpawnableHelicopters).PickRandom();

                Agency AgencyToSpawn = PossibleAgencies.PickRandom();

                if (HeliAgency != null)
                    AgencyToSpawn = HeliAgency;

                AgencyToSpawn = PossibleAgencies.PickRandom();
                bool SpawnOnFoot = false;

                if(AgencyToSpawn != null && CurrentSpawnZoneStreet != null)
                {
                    if (CanSpawnPedestrianOfficers && Jurisdiction.CanSpawnPedestrainOfficersAtZone(CurrentSpawnZoneStreet.InternalGameName, AgencyToSpawn.Initials) && General.RandomPercent(PercentagePedestrainOfficers))
                    {
                        SpawnOnFoot = true;
                    }
                }
                if(AgencyToSpawn == null)
                {
                    AgencyToSpawn = Agencies.AgenciesAtPosition(CurrentSpawn.Position).PickRandom();
                }
                if (AgencyToSpawn == null)
                {
                    Debugging.WriteToLog("Dispatch", string.Format("Could not find Agencies To Spawn {0}", 1));
                    return;
                }

                Vector3 PositionToSpawn = CurrentSpawn.StreetPosition;
                if (SpawnOnFoot && CurrentSpawn.SidewalkPosition != Vector3.Zero)
                {
                    Debugging.WriteToLog("Dispatch", string.Format("Spawned On Foot {0}", 1));
                    PoliceSpawning.SpawnGTACop(AgencyToSpawn, CurrentSpawn.SidewalkPosition, CurrentSpawn.Heading, null,true);
                }
                else
                {
                    Agency.VehicleInformation MyCarInfo = AgencyToSpawn.GetRandomVehicle();
                    if (MyCarInfo == null)
                    {
                        Debugging.WriteToLog("Dispatch", string.Format("Could not find Auto Info for {0}", AgencyToSpawn.Initials));
                        return;
                    }
                    
                    if (MyCarInfo.IsHelicopter)
                    {
                        Debugging.WriteToLog("Dispatch", string.Format("Helicopter: {0}", MyCarInfo.ModelName));
                        PositionToSpawn = CurrentSpawn.Position + new Vector3(0f, 0f, 250f);
                    }
                    else if (MyCarInfo.IsBoat)
                    {

                        Debugging.WriteToLog("Dispatch", string.Format("Boat: {0} isWater {1} WaterHieght {2}", MyCarInfo.ModelName, CurrentSpawn.IsWater, CurrentSpawn.WaterHeight));
                        PositionToSpawn = CurrentSpawn.Position;
                    }
                    PoliceSpawning.SpawnGTACop(AgencyToSpawn, PositionToSpawn, CurrentSpawn.Heading, MyCarInfo,false);
                }

                
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
                if (!OutOfRangeCop.AssignedAgency.CanSpawn)
                {
                    PoliceSpawning.DeleteCop(OutOfRangeCop);
                }
                if (OutOfRangeCop.IsInVehicle && OutOfRangeCop.DistanceToPlayer > DistanceToDelete) //Beyond Caring
                {
                    PoliceSpawning.DeleteCop(OutOfRangeCop);
                }
                else if (!OutOfRangeCop.IsInVehicle && OutOfRangeCop.DistanceToPlayer > DistanceToDeleteOnFoot) //Beyond Caring
                {
                    PoliceSpawning.DeleteCop(OutOfRangeCop);
                }
                else if (OutOfRangeCop.ClosestDistanceToPlayer <= 15f) //Got Close and Then got away
                {
                    PoliceSpawning.DeleteCop(OutOfRangeCop);
                }
                else if (OutOfRangeCop.CountNearbyCops >= 3 && OutOfRangeCop.TimeBehindPlayer >= 15000) //Got Close and Then got away
                {
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
        public Vector3 SidewalkPosition = Vector3.Zero;
        public bool IsWater
        {
            get
            {
                if (NativeFunction.Natives.GET_WATER_HEIGHT<bool>(Position.X, Position.Y, Position.Z, out float height))
                {
                    if (height >= 0.5f)//2f// has some water depth
                        return true;
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
                    return true;
                else
                    return false;

            }
        }
        public PoliceSpawn()
        {

        }
        public void UpdateSpawnPosition()
        {
            Position = Vector3.Zero;
            StreetPosition = Vector3.Zero;
            SidewalkPosition = Vector3.Zero;
            int TimesTried = 0;
            while(!HasSpawns && TimesTried < 10)
            {
                GetInitialPosition();
                GetStreetPosition();
                TimesTried++;
            }
        }
        private void GetInitialPosition()
        {
            if (PlayerState.IsWanted && Game.LocalPlayer.Character.IsInAnyVehicle(false))
                Position = Game.LocalPlayer.Character.GetOffsetPositionFront(350f);
            else if (Investigation.InInvestigationMode)
                Position = Investigation.InvestigationPosition;
            else
                Position = Game.LocalPlayer.Character.Position;

            Position = Position.Around2D(MinDistanceToSpawn, MaxDistanceToSpawn);

            if (!Investigation.InInvestigationMode && PedList.AnyCopsNearPosition(Position, ClosestSpawnToOtherPoliceAllowed))
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

            if(StreetPosition != Vector3.Zero)
            {
                General.GetSidewalkPositionAndHeading(StreetPosition, out SidewalkPosition);
            }
        }
    }

}
