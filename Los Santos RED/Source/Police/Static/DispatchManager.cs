using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class DispatchManager
{
    private enum DispatchType
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
    private static PoliceSpawn CurrentSpawn = new PoliceSpawn();
    private static uint GameTimeCheckedSpawn;
    private static uint GameTimeCheckedDeleted;
    private static int SpawnedCopLimit
    {
        get
        {
            if (InvestigationManager.InInvestigationMode)
                return 10;
            else if(PlayerStateManager.WantedLevel == 0)
            {
                return 5;//10//5
            }
            else if (PlayerStateManager.WantedLevel == 1)
            {
                return 7;//10//8
            }
            else if (PlayerStateManager.WantedLevel == 2)
            {
                return 10;//12
            }
            else if (PlayerStateManager.WantedLevel == 3)
            {
                return 18;//20
            }
            else if (PlayerStateManager.WantedLevel == 4)
            {
                return 25;
            }
            else if (PlayerStateManager.WantedLevel == 5)
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
    private static bool CanSpawn
    {
        get
        {
            if (GameTimeCheckedSpawn == 0)
                return true;
            else if (InvestigationManager.InInvestigationMode && !TaskManager.HasCopsInvestigating)
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
            if (PersonOfInterestManager.LethalForceAuthorized)
            {
                return 2000;
            }
            else  if (!PolicePedManager.AnyRecentlySeenPlayer)
            {
                return 3000;
            }
            else
            {
                int InverseWanted = 5 - PlayerStateManager.WantedLevel;
                return (InverseWanted * 2000) + 2000;
            }
        }
    }
    private static float ClosestSpawnToPlayerAllowed
    {
        get
        {
            if (PlayerStateManager.IsWanted)
                return 150f;
            else
                return 250f;
        }
    }
    private static float ClosestSpawnToOtherPoliceAllowed
    {
        get
        {
            if (PlayerStateManager.IsWanted)
                return 200f;
            else
                return 500f;
        }
    }
    private static float MinDistanceToSpawn
    {
        get
        {
            if (PlayerStateManager.IsWanted)
            {
                if (!PolicePedManager.AnyRecentlySeenPlayer)
                    return 250f - (PlayerStateManager.WantedLevel * -40);
                else
                    return 400f - (PlayerStateManager.WantedLevel * -40);
            }
            else if (InvestigationManager.InInvestigationMode)
                return InvestigationManager.InvestigationDistance / 2;
            else
                return 350f;//450f;//750f
        }
    }
    private static float MaxDistanceToSpawn
    {
        get
        {
            if (PlayerStateManager.IsWanted)
            {
                if (!PolicePedManager.AnyRecentlySeenPlayer)
                    return 350f;
                else
                    return 550f;
            }
            else if (InvestigationManager.InInvestigationMode)
                return InvestigationManager.InvestigationDistance;
            else
                return 900f;//1250f//1500f
        }
    }
    private static float DistanceToDelete
    {
        get
        {
            if (PlayerStateManager.IsWanted)
                return 600f;
            else
                return 1000f;
        }
    }
    private static float DistanceToDeleteOnFoot
    {
        get
        {
            if (PlayerStateManager.IsWanted)
                return 125f;
            else
                return 1000f;
        }
    }
    private static bool NeedToSpawn
    {
        get
        {
            if (PedManager.TotalSpawnedCops < SpawnedCopLimit)
                return true;
            else
                return false;
        }
    }
    private static uint MinimumExistingTime { get; set; } = 20000;//30000
    private static float MinimumDeleteDistance{ get; set; } = 200f;//350f
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
            if (NeedToSpawn && CurrentSpawn.HasSpawns)
            {
                CurrentSpawn.GetAgencyToSpawn();
                if (!CurrentSpawn.HasAgency)
                {
                    Debugging.WriteToLog("Dispatch", string.Format("Could not find Agencies To Spawn {0}", 1));
                    return;
                }

                VehicleInformation AgencyVehicle = CurrentSpawn.AgencyToSpawn.GetRandomVehicle();
                if (AgencyVehicle == null)
                {
                    Debugging.WriteToLog("Dispatch", string.Format("Could not find Auto for {0}", CurrentSpawn.AgencyToSpawn.Initials));
                    return;
                }

                if (AgencyVehicle.IsHelicopter)
                {
                    CurrentSpawn.FinalSpawnPosition = CurrentSpawn.Position + new Vector3(0f, 0f, 250f);
                    Debugging.WriteToLog("Dispatch", string.Format("Helicopter: {0}", AgencyVehicle.ModelName));
                }
                else if (AgencyVehicle.IsBoat)
                {
                    CurrentSpawn.FinalSpawnPosition = CurrentSpawn.Position;
                    Debugging.WriteToLog("Dispatch", string.Format("Boat: {0} isWater {1} WaterHieght {2}", AgencyVehicle.ModelName, CurrentSpawn.IsWater, CurrentSpawn.WaterHeight));
                }
                else
                {
                    CurrentSpawn.FinalSpawnPosition = CurrentSpawn.StreetPosition;
                }
                PoliceSpawningManager.SpawnGTACop(CurrentSpawn.AgencyToSpawn, CurrentSpawn.FinalSpawnPosition, CurrentSpawn.Heading, AgencyVehicle, false);
            }
            GameTimeCheckedSpawn = Game.GameTime;
        }
        if (IsRunning)
        {
            SetVanillaDispaceServices(false);
        }
    }
    public static void DeleteChecking()
    {
        if (IsRunning && CanDelete)
        {
            foreach(Cop OutOfRangeCop in PedManager.Cops.Where(x => x.RecentlyUpdated && x.DistanceToPlayer >= MinimumDeleteDistance && x.HasBeenSpawnedFor >= MinimumExistingTime)) 
            {
                if (!OutOfRangeCop.AssignedAgency.CanSpawn)
                {
                    PoliceSpawningManager.DeleteCop(OutOfRangeCop);
                }
                if (OutOfRangeCop.IsInVehicle && OutOfRangeCop.DistanceToPlayer > DistanceToDelete) //Beyond Caring
                {
                    PoliceSpawningManager.DeleteCop(OutOfRangeCop);
                }
                else if (!OutOfRangeCop.IsInVehicle && OutOfRangeCop.DistanceToPlayer > DistanceToDeleteOnFoot) //Beyond Caring
                {
                    PoliceSpawningManager.DeleteCop(OutOfRangeCop);
                }
                else if (OutOfRangeCop.ClosestDistanceToPlayer <= 15f) //Got Close and Then got away
                {
                    PoliceSpawningManager.DeleteCop(OutOfRangeCop);
                }
                else if (OutOfRangeCop.CountNearbyCops >= 3 && OutOfRangeCop.TimeBehindPlayer >= 15000) //Got Close and Then got away
                {
                    PoliceSpawningManager.DeleteCop(OutOfRangeCop);
                }
            }
            GameTimeCheckedDeleted = Game.GameTime;
        }
    }
    public static void Dispose()
    {
        IsRunning = false;
        SetVanillaDispaceServices(true);
    }
    private static void SetVanillaDispaceServices(bool ValueToSet)
    {
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.PoliceAutomobile, ValueToSet);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.PoliceHelicopter, ValueToSet);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.PoliceVehicleRequest, ValueToSet);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.SwatAutomobile, ValueToSet);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.SwatHelicopter, ValueToSet);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.PoliceRiders, ValueToSet);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.PoliceRoadBlock, ValueToSet);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.PoliceAutomobileWaitCruising, ValueToSet);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.PoliceAutomobileWaitPulledOver, ValueToSet);
    }
    private class PoliceSpawn
    {
        public Vector3 Position = Vector3.Zero;
        public float Heading;
        public Vector3 StreetPosition = Vector3.Zero;
        public Vector3 SidewalkPosition = Vector3.Zero;
        public Vector3 FinalSpawnPosition = Vector3.Zero;
        public Agency AgencyToSpawn = null;
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
        public bool HasSidewalkPosition
        {
            get
            {
                if (SidewalkPosition != Vector3.Zero)
                    return true;
                else
                    return false;

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
        public void GetAgencyToSpawn()
        {
            AgencyToSpawn = null;
            List<Agency> PossibleAgencies = AgencyManager.GetAllSpawnableAgencies(StreetPosition);
            if (General.RandomPercent(50))//Favor Helicopter Spawns
            {
                AgencyToSpawn = PossibleAgencies.Where(x => x.HasSpawnableHelicopters).PickRandom();
            }
            if (AgencyToSpawn == null)
            {
                AgencyToSpawn = PossibleAgencies.PickRandom();
            }
            if (AgencyToSpawn == null)
            {
                AgencyToSpawn = AgencyManager.GetAllSpawnableAgencies(Position).PickRandom();
            }
        }
        private void GetInitialPosition()
        {
            if (PlayerStateManager.IsWanted && Game.LocalPlayer.Character.IsInAnyVehicle(false))
                Position = Game.LocalPlayer.Character.GetOffsetPositionFront(350f);
            else if (InvestigationManager.InInvestigationMode)
                Position = InvestigationManager.InvestigationPosition;
            else
                Position = Game.LocalPlayer.Character.Position;

            Position = Position.Around2D(MinDistanceToSpawn, MaxDistanceToSpawn);

            if (!InvestigationManager.InInvestigationMode && PedManager.AnyCopsNearPosition(Position, ClosestSpawnToOtherPoliceAllowed))
            {
                Position = Vector3.Zero;
            }
        }
        private void GetStreetPosition()
        {
            General.GetStreetPositionandHeading(Position, out StreetPosition, out Heading, true);

            if (StreetPosition.DistanceTo2D(Game.LocalPlayer.Character) < ClosestSpawnToPlayerAllowed)
                StreetPosition = Vector3.Zero;

            if(PedManager.AnyCopsNearPosition(StreetPosition, ClosestSpawnToOtherPoliceAllowed))
                StreetPosition = Vector3.Zero;

            if(StreetPosition != Vector3.Zero)
            {
                General.GetSidewalkPositionAndHeading(StreetPosition, out SidewalkPosition);
            }
        }

    }

}
