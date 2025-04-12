using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class AIVehicleRacer : VehicleRacer
{
    uint GameTimeLastClearedFront;
    public AIVehicleRacer(PedExt pedExt, VehicleExt vehicleExt) : base(vehicleExt)
    {
        PedExt = pedExt;
    }
    public PedExt PedExt { get; set; }
    public bool WasSpawnedForRace { get; set; }
    public override string RacerName => PedExt == null ? base.RacerName : PedExt.Name;
    public override void Update(VehicleRace vehicleRace)
    {
        if(vehicleRace == null)
        {
            return;
        }
        if (PedExt == null)
        {
            return;
        }
        if (HasFinishedRace)
        {
            return;
        }
        base.Update(vehicleRace);
        PedExt.CurrentTask?.Update();
        if (VehicleExt != null && VehicleExt.Vehicle.Exists())
        {
            NativeFunction.Natives.SET_VEHICLE_CHEAT_POWER_INCREASE(VehicleExt.Vehicle, 2.0f);
        }
        if (Game.GameTime - GameTimeLastClearedFront > 200 && vehicleRace.HasRaceStarted)
        {
            ClearFront();
            GameTimeLastClearedFront = Game.GameTime;
        }
    }
    public void AssignTask(VehicleRace vehicleRace, ITargetable Targetable, IEntityProvideable World, ISettingsProvideable Settings)
    {
        if(PedExt == null || !PedExt.Pedestrian.Exists())
        {
            return;
        }
        PedExt.CurrentTask = new GeneralRace(PedExt, PedExt, Targetable, World, new List<VehicleExt>() { PedExt.AssignedVehicle }, null, Settings, vehicleRace, this);
        PedExt.CurrentTask.Start();


        

    }
    public override void Dispose()
    {
        if (PedExt != null)
        {
            PedExt.SetNonPersistent();
            if (WasSpawnedForRace)
            {
                PedExt.DeleteBlip();
                if(PedExt.Pedestrian.Exists())
                {
                    PedExt.Pedestrian.IsPersistent = false;
                }
                PedExt.CanBeIdleTasked = true;
            }

            PedExt.CanBeAmbientTasked = true;
            PedExt.CanBeTasked = true;       
            PedExt.CurrentTask?.Stop();
            PedExt.CurrentTask = null;
        }
        if (VehicleExt != null)
        {
            if (WasSpawnedForRace && !VehicleExt.IsOwnedByPlayer)
            {
                VehicleExt.RemoveBlip();
                if (VehicleExt.Vehicle.Exists())
                {
                    VehicleExt.Vehicle.IsPersistent = false;
                }
            }
        }
        base.Dispose();
    }
    public override void OnFinishedRace(int finalPosition, VehicleRace vehicleRace)
    {
        if(finalPosition == 1 && vehicleRace != null && vehicleRace.BetAmount > 0)
        {
            int totalWinAmount = vehicleRace.BetAmount * vehicleRace.VehicleRacers.Count();
            PedExt.Money += totalWinAmount;
            EntryPoint.WriteToConsole($"OnFinishedRace AI {PedExt?.Handle} finalPosition:{finalPosition} totalWinAmount:{totalWinAmount}");
        }  
        base.OnFinishedRace(finalPosition, vehicleRace);
    }
    public void ClearFront()
    {
        if (PedExt.IsInHelicopter || !PedExt.Pedestrian.Exists())
        {
            return;
        }
        Vehicle raceCar = PedExt.Pedestrian.CurrentVehicle;
        if (!raceCar.Exists())
        {
            return;
        }

        //EntryPoint.WriteToConsole("RACER CLEARED FRONT FOR ASSISTS");
        if (PedExt.DistanceToPlayer <= 20f || raceCar.IsOnScreen)//50f
        {
            CarefulFrontDelete(raceCar);
        }
        else
        {
            LargeFrontDelete(raceCar);
        }


    }
    private void LargeFrontDelete(Vehicle raceCar)
    {
        if (!raceCar.Exists())
        {
            return;
        }
        float length = raceCar.Model.Dimensions.Y;
        float speed = raceCar.Speed;
        float distanceInFront = 4.25f;
        float range = 7.25f;// 4f;
        if (speed >= 27f || PedExt.DistanceToPlayer >= 120f)//if(speed >= 27f || Cop.DistanceToPlayer >= 150f)//~60mph//if (speed >= 27f || Cop.DistanceToPlayer >= 120f)//if(speed >= 27f || Cop.DistanceToPlayer >= 150f)//~60mph
        {
            distanceInFront = 9.0f;// 6.25f;
            range = 12f;// 10f;
        }
        Vector3 Position = raceCar.GetOffsetPositionFront(length / 2f + distanceInFront);
        NativeFunction.Natives.CLEAR_AREA(Position.X, Position.Y, Position.Z, range, true, false, false, false);
    }
    private void CarefulFrontDelete(Vehicle copCar)
    {
        if (!copCar.Exists())
        {
            return;
        }
        float length = copCar.Model.Dimensions.Y;
        float speed = copCar.Speed;
        float distanceInFront = 4.25f;
        float range = 7.25f;// 4f;
        if (speed >= 27f || PedExt.DistanceToPlayer >= 120f)//if(speed >= 27f || Cop.DistanceToPlayer >= 150f)//~60mph//if (speed >= 27f || Cop.DistanceToPlayer >= 120f)//if(speed >= 27f || Cop.DistanceToPlayer >= 150f)//~60mph
        {
            distanceInFront = 9.0f;// 6.25f;
            range = 12f;// 10f;
        }
        Entity ClosestCarEntity = Rage.World.GetClosestEntity(copCar.GetOffsetPositionFront(length / 2f + distanceInFront), range, GetEntitiesFlags.ConsiderGroundVehicles | GetEntitiesFlags.ExcludePoliceCars | GetEntitiesFlags.ExcludePlayerVehicle);
        GameFiber.Yield();
        if (PedExt.Pedestrian.Exists() && ClosestCarEntity.Exists() && PedExt.Pedestrian.CurrentVehicle.Exists())
        {
            if (ClosestCarEntity != null && ClosestCarEntity.Handle != PedExt.Pedestrian.CurrentVehicle.Handle && !ClosestCarEntity.IsOnScreen && !ClosestCarEntity.IsPersistent)
            {
                Vehicle ClosestCar = (Vehicle)ClosestCarEntity;
                foreach (Ped carOccupant in ClosestCar.Occupants.ToList())
                {
                    if (carOccupant.Exists())
                    {
                        if (carOccupant.IsPersistent)
                        {
                            return;
                        }
                        carOccupant.Delete();
                    }
                }
                if (ClosestCar.Exists())
                {
                    ClosestCar.Delete();
                }
                GameFiber.Yield();
                EntryPoint.WriteToConsole($"DELETED CAR IN FRONT USING ASSIST MANAGER {PedExt.Handle}");
            }
        }
    }
}

