using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CopAssistManager
{
    private bool IsSetNoCollision = false;//this is new
    private Cop Cop;
    private bool IsCheatFiberRunning = false;

    public CopAssistManager(Cop cop)
    {
        Cop = cop;
    }
    public void UpdateCollision(bool IsWanted)
    {
        if (Cop.Pedestrian.Exists() && Cop.IsDriver && Cop.Pedestrian.CurrentVehicle.Exists())
        {
            if (IsWanted)// && Cop.DistanceToPlayer > 15f)
            {
                if (!IsSetNoCollision)
                {
                    Cop.Pedestrian.CurrentVehicle.IsCollisionProof = true;
                    IsSetNoCollision = true;
                }

                //SetClosestVehicleNoCollision();
            }
            else
            {
                if (IsSetNoCollision)
                {
                    Cop.Pedestrian.CurrentVehicle.IsCollisionProof = false;
                    IsSetNoCollision = false;
                }
            }

        }
    }
    private void SetClosestVehicleNoCollision()
    {
        Entity Closest = Rage.World.GetClosestEntity(Cop.Pedestrian.CurrentVehicle.GetOffsetPositionFront(5f), 10f, GetEntitiesFlags.ConsiderGroundVehicles | GetEntitiesFlags.ExcludePoliceCars | GetEntitiesFlags.ExcludePlayerVehicle);
        if (Closest != null && Closest.Handle != Cop.Pedestrian.CurrentVehicle.Handle)
        {
            Cop.Pedestrian.CurrentVehicle.CollisionIgnoredEntity = Closest;
        }
        else
        {
            Cop.Pedestrian.CurrentVehicle.CollisionIgnoredEntity = null;
        }
    }

    public void ClearFront(bool isWanted)
    {
        if(!Cop.IsDriver || Cop.DistanceToPlayer > 500f || !isWanted || Cop.IsInHelicopter || !Cop.Pedestrian.Exists())
        {
            return;
        }
        Vehicle copCar = Cop.Pedestrian.CurrentVehicle;
        if(!copCar.Exists())
        {
            return;
        }


        if(Cop.DistanceToPlayer <= 20f || copCar.IsOnScreen)//50f
        {
            CarefulFrontDelete(copCar);
        }
        else
        {
            LargeFrontDelete(copCar);
        }


    }

    private void LargeFrontDelete(Vehicle copCar)
    {
        if (!copCar.Exists())
        {
            return;
        }
        float length = copCar.Model.Dimensions.Y;
        float speed = copCar.Speed;
        float distanceInFront = 4.25f;
        float range = 7.25f;// 4f;
        if (speed >= 27f || Cop.DistanceToPlayer >= 120f)//if(speed >= 27f || Cop.DistanceToPlayer >= 150f)//~60mph//if (speed >= 27f || Cop.DistanceToPlayer >= 120f)//if(speed >= 27f || Cop.DistanceToPlayer >= 150f)//~60mph
        {
            distanceInFront = 9.0f;// 6.25f;
            range = 12f;// 10f;
        }
        Vector3 Position = copCar.GetOffsetPositionFront(length / 2f + distanceInFront);
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
        if (speed >= 27f || Cop.DistanceToPlayer >= 120f)//if(speed >= 27f || Cop.DistanceToPlayer >= 150f)//~60mph//if (speed >= 27f || Cop.DistanceToPlayer >= 120f)//if(speed >= 27f || Cop.DistanceToPlayer >= 150f)//~60mph
        {
            distanceInFront = 9.0f;// 6.25f;
            range = 12f;// 10f;
        }
        Entity ClosestCarEntity = Rage.World.GetClosestEntity(copCar.GetOffsetPositionFront(length / 2f + distanceInFront), range, GetEntitiesFlags.ConsiderGroundVehicles | GetEntitiesFlags.ExcludePoliceCars | GetEntitiesFlags.ExcludePlayerVehicle);
        GameFiber.Yield();
        if (Cop.Pedestrian.Exists() && ClosestCarEntity.Exists() && Cop.Pedestrian.CurrentVehicle.Exists())
        {
            if (ClosestCarEntity != null && ClosestCarEntity.Handle != Cop.Pedestrian.CurrentVehicle.Handle && !ClosestCarEntity.IsOnScreen && !ClosestCarEntity.IsPersistent)
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
                EntryPoint.WriteToConsole($"DELETED CAR IN FRONT USING ASSIST MANAGER {Cop.Handle}");
            }
        }
    }
    public void PowerAssist(bool isWanted)
    {
        if (Cop.IsDriver && Cop.DistanceToPlayer <= 200f && isWanted && !Cop.IsInHelicopter && Cop.Pedestrian.Exists())//if (Cop.IsDriver && Cop.DistanceToPlayer <= 200f && Cop.DistanceToPlayer >= 30f && isWanted && !Cop.IsInHelicopter && Cop.Pedestrian.Exists())
        {
            Vehicle copCar = Cop.Pedestrian.CurrentVehicle;
            if (copCar.Exists())
            {



                NativeFunction.Natives.SET_VEHICLE_CHEAT_POWER_INCREASE(copCar, 2.0f);


                //if (!IsCheatFiberRunning)
                //{
                //    //EntryPoint.WriteToConsoleTestLong($"POWER ASSIST STARTED {Cop.Handle}");
                //    IsCheatFiberRunning = true;
                //    GameFiber.StartNew(delegate
                //    {
                //        try
                //        {
                //            while (copCar.Exists() && IsCheatFiberRunning)
                //            {
                //                NativeFunction.Natives.SET_VEHICLE_CHEAT_POWER_INCREASE(copCar, 1.8f);
                //                GameFiber.Sleep(100);
                //                //GameFiber.Yield();
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                //            //EntryPoint.ModController.CrashUnload();
                //        }
                //    }, "cheatfiber");
                //}
            }
            //else
            //{
            //    IsCheatFiberRunning = false;
            //}
        }
        //else
        //{
        //    IsCheatFiberRunning = false;
        //}
    }
}

