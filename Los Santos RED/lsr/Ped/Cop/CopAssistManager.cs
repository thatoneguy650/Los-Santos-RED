using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


public class CopAssistManager
{
    private bool IsSetNoCollision = false;//this is new
    private Cop Cop;
    private bool IsCheatFiberRunning = false;
    private bool IsRunningForceFiber;

    public bool IsRunningOtherFiber { get; private set; }

    public CopAssistManager(Cop cop)
    {
        Cop = cop;
    }
    public void UpdateCollision(bool IsWanted)
    {
        //if (Cop.Pedestrian.Exists() && Cop.IsDriver && Cop.Pedestrian.CurrentVehicle.Exists())
        //{
        //    if (IsWanted)// && Cop.DistanceToPlayer > 15f)
        //    {
        //        if (!IsSetNoCollision)
        //        {
        //            Cop.Pedestrian.CurrentVehicle.IsCollisionProof = true;
        //            IsSetNoCollision = true;
        //        }

        //        //SetClosestVehicleNoCollision();
        //    }
        //    else
        //    {
        //        if (IsSetNoCollision)
        //        {
        //            Cop.Pedestrian.CurrentVehicle.IsCollisionProof = false;
        //            IsSetNoCollision = false;
        //        }
        //    }

        //}
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
        if(!Cop.IsDriver || Cop.DistanceToPlayer > 500f || !isWanted || Cop.IsInHelicopter || Cop.IsInAirVehicle || !Cop.Pedestrian.Exists())
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
        if (speed >= 27f)//if(speed >= 27f || Cop.DistanceToPlayer >= 150f)//~60mph//if (speed >= 27f || Cop.DistanceToPlayer >= 120f)//if(speed >= 27f || Cop.DistanceToPlayer >= 150f)//~60mph
        {
            distanceInFront = 9.0f;// 6.25f;
            range = 12f;// 10f;
        }
        if(speed >= 35f || Cop.DistanceToPlayer >= 120f)
        {
            distanceInFront = 12.0f;// 6.25f;
            range = 15f;// 10f;
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
        if (speed >= 27f)//if(speed >= 27f || Cop.DistanceToPlayer >= 150f)//~60mph//if (speed >= 27f || Cop.DistanceToPlayer >= 120f)//if(speed >= 27f || Cop.DistanceToPlayer >= 150f)//~60mph
        {
            distanceInFront = 9.0f;// 6.25f;
            range = 12f;// 10f;
        }
        if(speed >= 35f || Cop.DistanceToPlayer >= 120f)
        {
            distanceInFront = 12.0f;// 6.25f;
            range = 15f;// 10f;
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
    public void PowerAssist(int wantedLevel, float playerVehicleSpeedMPH)
    {
        if (Cop.IsDriver && Cop.DistanceToPlayer <= 200f && wantedLevel >= 2 && playerVehicleSpeedMPH >= 20f && !Cop.IsInHelicopter && Cop.Pedestrian.Exists())//if (Cop.IsDriver && Cop.DistanceToPlayer <= 200f && Cop.DistanceToPlayer >= 30f && isWanted && !Cop.IsInHelicopter && Cop.Pedestrian.Exists())
        {
            Vehicle copCar = Cop.Pedestrian.CurrentVehicle;
            if (copCar.Exists())
            {
                NativeFunction.Natives.SET_VEHICLE_CHEAT_POWER_INCREASE(copCar, 2.0f);
              
            }
        }

    }
    public void StabilityForceApplier(int wantedLevel, float vehicleSpeedMPH)
    {
        if (!Cop.IsInVehicle || wantedLevel == 0)
        {
            IsRunningOtherFiber = false;
            return;
        }
        if (!IsRunningOtherFiber)
        {
            StartOtherApplier();
        }

    }
    private void StartOtherApplier()
    {
        IsRunningOtherFiber = true;
        GameFiber.StartNew(delegate
        {
            EntryPoint.WriteToConsole($"I AM COP {Cop?.Handle} AND I STARTED AN OTHER APPLIER");

            while (EntryPoint.ModController.IsRunning && IsRunningOtherFiber && Cop != null && Cop.Pedestrian.Exists() && Cop.Pedestrian.CurrentVehicle.Exists())
            {
                ApplyStabilityForce(Cop.Pedestrian.CurrentVehicle);
                GameFiber.Yield();
            }
            IsRunningOtherFiber = false;
            EntryPoint.WriteToConsole($"I AM COP {Cop?.Handle} AND I STOPPED AN OTHER APPLIER");
        }, "Run Debug Logic");
    }
    private void ApplyStabilityForce(Vehicle copCar)
    {

        if(!Cop.IsInVehicle || !Cop.IsDriver || Cop.IsInAirVehicle || Cop.IsInBoat || Cop.IsInPlane || Cop.IsOnBike)
        {
            return;
        }

        if (copCar.Model.IsBike || copCar.Model.IsQuadBike || copCar.Model.IsHelicopter || copCar.Model.IsPlane)
        {
            return;
        }
        float speedFactor = copCar.Speed * 0.0018f;
        NativeFunction.Natives.APPLY_FORCE_TO_ENTITY(copCar, 3, 0f, 0f, -speedFactor, 0f, 0f, 0f, 0, false, true, true, false, true);
        NativeFunction.Natives.SET_VEHICLE_CHEAT_POWER_INCREASE(copCar, 1.5f);
    }


    public void ForceApplier(bool isWanted, ISettingsProvideable settings)
    {
        if (!Cop.IsDriver || Cop.DistanceToPlayer > 300f || !isWanted || Cop.IsInAirVehicle || !Cop.Pedestrian.Exists())
        {
            IsRunningForceFiber = false;
            return;
        }
        Vehicle copCar = Cop.Pedestrian.CurrentVehicle;
        if (!copCar.Exists())
        {
            IsRunningForceFiber = false;
            return;
        }
        if(IsRunningForceFiber)
        {
            return;
        }
        ForceApplierCheck(settings);

    }


    private void ForceApplierCheck(ISettingsProvideable Settings)
    {
        IsRunningForceFiber = true;

        //[Description("Enable or disable applied force for police vehicles")]
        bool AllowForceAssist;
        float ForceAssistAmount;
        float ForceAssistSpeedChangeThreshold;
        float ForceAssistTurningRadiusLimit;
        float ForceAssistMinimumSpeedMetersPerSecond;

        AllowForceAssist = false;
        ForceAssistAmount = 10.0f;
        ForceAssistSpeedChangeThreshold = 0.2f;
        ForceAssistTurningRadiusLimit = 10.0f;
        ForceAssistMinimumSpeedMetersPerSecond = 15.0f;


        GameFiber.StartNew(delegate
        {
            EntryPoint.WriteToConsole($"I AM COP {Cop.Handle} AND I STARTED A FORCE APPLIER");
            float prevSpeed = 0.0f;
            string CurrentSubtitle = "";
            bool isAccelerating = false;
            bool isBraking = false;
            bool isTurning = false;
            Vehicle coolVeh = Cop.Pedestrian.CurrentVehicle;
            float CurrentSpeed = 0.0f;
            float speedThreshold = ForceAssistSpeedChangeThreshold;
            float turningRadius = ForceAssistTurningRadiusLimit;
            while (IsRunningForceFiber)
            {


                if (coolVeh.Exists())
                {

                    CurrentSpeed = coolVeh.Speed;
                    float speedDiff = CurrentSpeed - prevSpeed;
                    if (speedDiff > speedThreshold)
                    {
                        isAccelerating = true;
                    }
                    else
                    {
                        isAccelerating = false;
                    }

                    
                    if (speedDiff < -1.0f* speedThreshold)
                    {
                        isBraking = true;
                    }
                    else
                    {
                        isBraking = false;
                    }



                    prevSpeed = CurrentSpeed;
                    if (coolVeh.SteeringAngle > turningRadius)
                    {
                        isTurning = true;
                    }
                    else
                    {
                        isTurning = false;
                    }

                    bool isApplyingForce = false;

                    if (!isTurning && CurrentSpeed >= ForceAssistMinimumSpeedMetersPerSecond)
                    {
                        if (isAccelerating && 1==0)
                        {
                            isApplyingForce = true;
                            coolVeh.ApplyForce(new Vector3(0.0f, 1.0f, 0.0f) * ForceAssistAmount, Vector3.Zero, true, true);
                        }
                        else if (isBraking)
                        {
                            isApplyingForce = true;
                            coolVeh.ApplyForce(new Vector3(0.0f, -1.0f * ForceAssistAmount, 0.0f), Vector3.Zero, true, true);
                        }

                    }
                    //CurrentSubtitle = $" {Cop.Handle} isAccelerating{isAccelerating} isBraking{isBraking} CurrentSpeed:{Math.Round(CurrentSpeed, 2)} PrevSpeed:{Math.Round(prevSpeed, 2)} Diff:{Math.Round(speedDiff, 2)} isTurning {isTurning}  isApplyingForce{isApplyingForce}";

                }




                //EntryPoint.WriteToConsole(CurrentSubtitle);

               //Game.DisplaySubtitle(CurrentSubtitle);
                GameFiber.Yield();
            }
            IsRunningForceFiber = false;

        }, "Run Debug Logic");
    }

}

