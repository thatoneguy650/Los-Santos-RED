using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class AssistManager
{
    private Cop Cop;

    public AssistManager(Cop cop)
    {
        Cop = cop;
    }

    public void UpdateCollision(bool IsWanted)
    {
        if (Cop.IsDriver && Cop.Pedestrian.CurrentVehicle.Exists())
        {
            if (IsWanted && Cop.DistanceToPlayer > 15f)
            {
                Cop.Pedestrian.CurrentVehicle.IsCollisionProof = true;
                //SetClosestVehicleNoCollision();
            }
            else
            {
                Cop.Pedestrian.CurrentVehicle.IsCollisionProof = false;
            }

        }
    }
    public void UpdateDrivingFlags()
    {
        NativeFunction.Natives.SET_DRIVER_ABILITY(Cop.Pedestrian, 100f);
        NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE(Cop.Pedestrian, 8f);
        NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Cop.Pedestrian, 32, true);
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
}

