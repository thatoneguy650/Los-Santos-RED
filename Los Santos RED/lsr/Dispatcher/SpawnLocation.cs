using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SpawnLocation
{
    public SpawnLocation()
    {

    }
    public SpawnLocation(Vector3 initialPosition)
    {
        InitialPosition = initialPosition;
    }
    public bool HasSpawns
    {
        get
        {
            if (InitialPosition != Vector3.Zero && StreetPosition != Vector3.Zero)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
    public float Heading { get; set; }
    public Vector3 InitialPosition { get; set; } = Vector3.Zero;
    public bool IsWater
    {
        get
        {
            if (NativeFunction.Natives.GET_WATER_HEIGHT<bool>(InitialPosition.X, InitialPosition.Y, InitialPosition.Z, out float height))
            {
                if (height >= 0.5f)//2f// has some water depth
                {
                    return true;
                }
            }
            return false;
        }
    }
    public Vector3 StreetPosition { get; set; } = Vector3.Zero;
    public float WaterHeight
    {
        get
        {
            if (NativeFunction.Natives.GET_WATER_HEIGHT<bool>(InitialPosition.X, InitialPosition.Y, InitialPosition.Z, out float height))
            {
                return height;
            }
            return height;
        }
    }
    public void GetClosestStreet()
    {
        Vector3 streetPosition;
        float streetHeading;
        NativeFunction.Natives.GET_CLOSEST_VEHICLE_NODE_WITH_HEADING<bool>(InitialPosition.X, InitialPosition.Y, InitialPosition.Z, out streetPosition, out streetHeading, 0, 3, 0);
        StreetPosition = streetPosition;
        Heading = streetHeading;
    }
}

