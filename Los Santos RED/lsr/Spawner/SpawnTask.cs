using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosSantosRED.lsr.Interface;
using System.Drawing;
using ExtensionsMethods;

public abstract class SpawnTask
{
    public SpawnTask(SpawnLocation spawnLocation, DispatchableVehicle vehicleType, DispatchablePerson personType)
    {
        SpawnLocation = spawnLocation;
        PersonType = personType;
        VehicleType = vehicleType;
    }
    public List<PedExt> CreatedPeople { get; private set; } = new List<PedExt>();
    public List<VehicleExt> CreatedVehicles { get; private set; } = new List<VehicleExt>();
    public SpawnLocation SpawnLocation { get; set; }
    public DispatchableVehicle VehicleType { get; set; }
    public DispatchablePerson PersonType { get; set; }
    public bool AllowAnySpawn { get; set; } = false;
    public bool AllowBuddySpawn { get; set; } = true;
    public Vector3 Position
    {
        get
        {
            if (VehicleType == null)
            {
                if (SpawnLocation.HasSidewalk)
                {
                    return SpawnLocation.SidewalkPosition;
                }
                return SpawnLocation.InitialPosition;
            }
            else if (VehicleType.IsHelicopter)
            {
                return SpawnLocation.InitialPosition + new Vector3(0f, 0f, 250f);
            }
            else if (VehicleType.IsBoat)
            {
                return SpawnLocation.InitialPosition;
            }
            else
            {
                return SpawnLocation.StreetPosition;
            }
        }
    }
    public bool PlacePedOnGround { get; set; } = false;
    public abstract void AttemptSpawn();

}
