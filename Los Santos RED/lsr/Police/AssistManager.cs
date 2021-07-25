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

