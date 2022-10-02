using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class AssistManager
{
    private bool IsSetNoCollision = false;//this is new
    private Cop Cop;
    private bool IsCheatFiberRunning = false;

    public AssistManager(Cop cop)
    {
        Cop = cop;
    }
    public void UpdateCollision(bool IsWanted)
    {
        if (Cop.Pedestrian.Exists() && Cop.IsDriver && Cop.Pedestrian.CurrentVehicle.Exists())
        {
            if (IsWanted && Cop.DistanceToPlayer > 15f)
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
        if(Cop.IsDriver && Cop.DistanceToPlayer <= 500f && isWanted && !Cop.IsInHelicopter && Cop.Pedestrian.Exists())
        {
            Vehicle copCar = Cop.Pedestrian.CurrentVehicle;
            if(copCar.Exists())
            {
                float length = copCar.Model.Dimensions.Y;
                float speed = copCar.Speed;
                float distanceInFront = 3f + 1.25f;
                if (speed >= 5f || Cop.DistanceToPlayer >= 150f) //if (speed >= 18f || Cop.DistanceToPlayer >= 150f)//~40mph
                {
                    float range = 6f + 1.25f;// 4f;
                    if(speed >= 27f || Cop.DistanceToPlayer >= 150f)//~60mph
                    {
                        distanceInFront = 5f + 1.25f;
                        range = 10f;
                    }
                    Entity ClosestCarEntity = Rage.World.GetClosestEntity(copCar.GetOffsetPositionFront(length/2f + distanceInFront), range, GetEntitiesFlags.ConsiderGroundVehicles | GetEntitiesFlags.ExcludePoliceCars | GetEntitiesFlags.ExcludePlayerVehicle);
                    if (ClosestCarEntity != null && ClosestCarEntity.Handle != Cop.Pedestrian.CurrentVehicle.Handle && !ClosestCarEntity.IsOnScreen && !ClosestCarEntity.IsPersistent)
                    {
                        Vehicle ClosestCar = (Vehicle)ClosestCarEntity;
                        foreach (Ped carOccupant in ClosestCar.Occupants.ToList())
                        {
                            if(carOccupant.Exists())
                            {
                                if(carOccupant.IsPersistent)
                                {
                                    return;
                                }
                                carOccupant.Delete();
                            }
                        }
                        if(ClosestCar.Exists())
                        {
                            ClosestCar.Delete();
                        }
                        EntryPoint.WriteToConsole($"DELETED CAR IN FRONT USING ASSIST MANAGER {Cop.Handle}");
                    }
                }
            }
        }
    }
    public void PowerAssist(bool isWanted)
    {
        if (Cop.IsDriver && Cop.DistanceToPlayer <= 200f && Cop.DistanceToPlayer >= 30f && isWanted && !Cop.IsInHelicopter && Cop.Pedestrian.Exists())
        {
            Vehicle copCar = Cop.Pedestrian.CurrentVehicle;
            if (copCar.Exists())
            { 
                if(!IsCheatFiberRunning)
                {
                    EntryPoint.WriteToConsole($"POWER ASSIST STARTED {Cop.Handle}");
                    IsCheatFiberRunning = true;
                    GameFiber.StartNew(delegate
                    {
                        while (copCar.Exists() && IsCheatFiberRunning)
                        {
                            NativeFunction.Natives.SET_VEHICLE_CHEAT_POWER_INCREASE(copCar, 1.8f);
                            GameFiber.Yield();
                        }
                    }, "cheatfiber");
                }
            }
            else
            {
                IsCheatFiberRunning = false;
            }
        }
        else
        {
            IsCheatFiberRunning = false;
        }
    }
}

