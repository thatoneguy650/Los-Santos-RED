using ExtensionsMethods;
using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using System.Collections.Generic;
using System.Linq;

public class Dispatcher
{
    private PoliceSpawn CurrentSpawn;
    private uint GameTimeCheckedSpawn;
    private uint GameTimeCheckedDeleted;
    private uint MinimumExistingTime = 20000;//30000//will be a setting
    private float MinimumDeleteDistance = 200f;//350f//will be a setting
    private int SpawnedCopLimit
    {
        get
        {
            if (Mod.Player.Investigations.IsActive)
            {
                return 6;//10
            }
            else if (Mod.Player.WantedLevel == 0)
            {
                return 5;//10//5
            }
            else if (Mod.Player.WantedLevel == 1)
            {
                return 7;//10//8
            }
            else if (Mod.Player.WantedLevel == 2)
            {
                return 10;//12
            }
            else if (Mod.Player.WantedLevel == 3)
            {
                return 18;//20
            }
            else if (Mod.Player.WantedLevel == 4)
            {
                return 25;
            }
            else if (Mod.Player.WantedLevel == 5)
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
            //else if (Mod.Player.Investigations.IsActive && !Mod.World.Tasking.HasCopsInvestigating)//maybe add this back, maybe not? breaks lots of over reach....
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
            if (Mod.Player.LethalForceAuthorized)
            {
                return 2000;
            }
            else  if (!Mod.World.AnyPoliceRecentlySeenPlayer)
            {
                return 3000;
            }
            else
            {
                return ((5 - Mod.Player.WantedLevel) * 2000) + 2000;
            }
        }
    }
    private float ClosestSpawnToSuspectAllowed
    {
        get
        {
            if (Mod.Player.IsWanted)
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
            if (Mod.Player.IsWanted)
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
            if (Mod.Player.IsWanted)
            {
                if (!Mod.World.AnyPoliceRecentlySeenPlayer)
                {
                    return 250f - (Mod.Player.WantedLevel * -40);
                }
                else
                {
                    return 400f - (Mod.Player.WantedLevel * -40);
                }
            }
            else if (Mod.Player.Investigations.IsActive)
            {
                return Mod.Player.Investigations.Distance / 2;
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
            if (Mod.Player.IsWanted)
            {
                if (!Mod.World.AnyPoliceRecentlySeenPlayer)
                {
                    return 350f;
                }
                else
                {
                    return 550f;
                }
            }
            else if (Mod.Player.Investigations.IsActive)
            {
                return Mod.Player.Investigations.Distance;
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
            if (Mod.Player.IsWanted)
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
            if (Mod.Player.IsWanted)
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
            if (Mod.World.Pedestrians.TotalSpawnedCops < SpawnedCopLimit)
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
        if (CanDispatch)
        {
            CurrentSpawn = new PoliceSpawn(MinDistanceToSpawn, MaxDistanceToSpawn, ClosestSpawnToOtherPoliceAllowed, ClosestSpawnToSuspectAllowed);
            CurrentSpawn.GetPosition();
            //CurrentSpawn.Update();
            if (NeedToSpawn && CurrentSpawn.HasSpawns)
            {
                CurrentSpawn.GetAgency();
                if (!CurrentSpawn.HasAgency)
                {
                    Mod.Debug.WriteToLog("Dispatch", string.Format("Could not find Agencies To Spawn {0}", 1));
                    return;
                }

                VehicleInformation AgencyVehicle = CurrentSpawn.AgencyToSpawn.GetRandomVehicle();
                if (AgencyVehicle == null)
                {
                    Mod.Debug.WriteToLog("Dispatch", string.Format("Could not find Auto for {0}", CurrentSpawn.AgencyToSpawn.Initials));
                    return;
                }

                if (AgencyVehicle.IsHelicopter)
                {
                    CurrentSpawn.FinalSpawnPosition = CurrentSpawn.Position + new Vector3(0f, 0f, 250f);
                    Mod.Debug.WriteToLog("Dispatch", string.Format("Helicopter: {0}", AgencyVehicle.ModelName));
                }
                else if (AgencyVehicle.IsBoat)
                {
                    CurrentSpawn.FinalSpawnPosition = CurrentSpawn.Position;
                    Mod.Debug.WriteToLog("Dispatch", string.Format("Boat: {0} isWater {1} WaterHieght {2}", AgencyVehicle.ModelName, CurrentSpawn.IsWater, CurrentSpawn.WaterHeight));
                }
                else
                {
                    CurrentSpawn.FinalSpawnPosition = CurrentSpawn.StreetPosition;
                }
                Mod.World.SpawnCop(CurrentSpawn.AgencyToSpawn, CurrentSpawn.FinalSpawnPosition, CurrentSpawn.Heading, AgencyVehicle);
            }
            GameTimeCheckedSpawn = Game.GameTime;
        }
        SetVanilla(false);      
    }
    public void Recall()
    {
        if (CanRecall)
        {
            foreach(Cop OutOfRangeCop in Mod.World.Pedestrians.Police.Where(x => x.RecentlyUpdated && x.DistanceToPlayer >= MinimumDeleteDistance && x.HasBeenSpawnedFor >= MinimumExistingTime)) 
            {
                bool ShouldDelete = false;
                if (!OutOfRangeCop.AssignedAgency.CanSpawn)
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
                else if (Mod.World.Pedestrians.CountNearbyCops(OutOfRangeCop.Pedestrian) >= 3 && OutOfRangeCop.TimeBehindPlayer >= 15000) //Got Close and Then got away
                {
                    ShouldDelete = true;
                }
                else if(!OutOfRangeCop.AssignedAgency.CanSpawn)
                {
                    ShouldDelete = true;
                }
                if(ShouldDelete)
                {
                    Mod.World.Delete(OutOfRangeCop);
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
        private float ClosestSpawnToOtherPoliceAllowed;
        private float ClosestSpawnToSuspectAllowed;
        public Vector3 Position { get; set; } = Vector3.Zero;
        public Vector3 StreetPosition { get; set; } = Vector3.Zero;
        public Vector3 SidewalkPosition { get; set; } = Vector3.Zero;
        public Vector3 FinalSpawnPosition { get; set; } = Vector3.Zero;
        public float Heading { get; set; }
        public Agency AgencyToSpawn { get; set; }
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
        public bool HasSidewalkPosition
        {
            get
            {
                if (SidewalkPosition != Vector3.Zero)
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
        public PoliceSpawn(float minDistanceToSpawn,float maxDistanceToSpawn, float closestSpawnToOtherPoliceAllowed, float closestSpawnToSuspectAllowed)
        {
            MinDistanceToSpawn = minDistanceToSpawn;
            MaxDistanceToSpawn = maxDistanceToSpawn;
            ClosestSpawnToOtherPoliceAllowed = closestSpawnToOtherPoliceAllowed;
            ClosestSpawnToSuspectAllowed = closestSpawnToSuspectAllowed;
        }
        public void GetPosition()
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
        public void GetAgency()
        {
            AgencyToSpawn = null;
            List<Agency> PossibleAgencies = Mod.DataMart.Agencies.GetAgencies(StreetPosition);
            if (RandomItems.RandomPercent(50))//Favor Helicopter Spawns
            {
                AgencyToSpawn = PossibleAgencies.Where(x => x.HasSpawnableHelicopters).PickRandom();
            }
            if (AgencyToSpawn == null)
            {
                AgencyToSpawn = PossibleAgencies.PickRandom();
            }
            if (AgencyToSpawn == null)
            {
                AgencyToSpawn = Mod.DataMart.Agencies.GetAgencies(Position).PickRandom();
            }
        }
        private void GetInitialPosition()
        {
            if (Mod.Player.IsWanted && Game.LocalPlayer.Character.IsInAnyVehicle(false))
            {
                Position = Game.LocalPlayer.Character.GetOffsetPositionFront(350f);
            }
            else if (Mod.Player.Investigations.IsActive)
            {
                Position = Mod.Player.Investigations.Position;
            }
            else
            {
                Position = Game.LocalPlayer.Character.Position;
            }

            Position = Position.Around2D(MinDistanceToSpawn, MaxDistanceToSpawn);

            if (!Mod.Player.Investigations.IsActive && Mod.World.Pedestrians.AnyCopsNearPosition(Position, ClosestSpawnToOtherPoliceAllowed))
            {
                Position = Vector3.Zero;
            }
        }
        private void GetStreetPosition()
        {
            Vector3 streetPos;
            float heading;
            Mod.DataMart.Streets.GetStreetPositionandHeading(Position, out streetPos, out heading, true);
            StreetPosition = streetPos;
            Heading = heading;

            if (StreetPosition.DistanceTo2D(Game.LocalPlayer.Character) < ClosestSpawnToSuspectAllowed)
            {
                StreetPosition = Vector3.Zero;
            }

            if (Mod.World.Pedestrians.AnyCopsNearPosition(StreetPosition, ClosestSpawnToOtherPoliceAllowed))
            {
                StreetPosition = Vector3.Zero;
            }

            if(StreetPosition != Vector3.Zero)
            {
                Vector3 sidewalkPosition;
                Mod.DataMart.Streets.GetSidewalkPositionAndHeading(StreetPosition, out sidewalkPosition);
                SidewalkPosition = sidewalkPosition;
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
