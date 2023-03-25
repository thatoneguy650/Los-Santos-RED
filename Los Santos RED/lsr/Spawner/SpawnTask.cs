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


    //public void UpgradePerformance(VehicleExt vehicleExt)//should be an inherited class? VehicleExt and CopCar? For now itll stay in here 
    //{
    //    if (vehicleExt.Vehicle.Exists() || vehicleExt.Vehicle.IsHelicopter)
    //    {
    //        return;
    //    }
    //    NativeFunction.CallByName<bool>("SET_VEHICLE_MOD_KIT", vehicleExt.Vehicle, 0);//Required to work
    //    NativeFunction.CallByName<bool>("SET_VEHICLE_MOD", vehicleExt.Vehicle, 11, NativeFunction.CallByName<int>("GET_NUM_VEHICLE_MODS", vehicleExt.Vehicle, 11) - 1, true);//Engine
    //    NativeFunction.CallByName<bool>("SET_VEHICLE_MOD", vehicleExt.Vehicle, 12, NativeFunction.CallByName<int>("GET_NUM_VEHICLE_MODS", vehicleExt.Vehicle, 12) - 1, true);//Brakes
    //    NativeFunction.CallByName<bool>("SET_VEHICLE_MOD", vehicleExt.Vehicle, 13, NativeFunction.CallByName<int>("GET_NUM_VEHICLE_MODS", vehicleExt.Vehicle, 13) - 1, true);//Tranny
    //    NativeFunction.CallByName<bool>("SET_VEHICLE_MOD", vehicleExt.Vehicle, 15, NativeFunction.CallByName<int>("GET_NUM_VEHICLE_MODS", vehicleExt.Vehicle, 15) - 1, true);//Suspension
    //}
    //public void UpdatePlate(VehicleExt vehicleExt, Agency AssignedAgency)
    //{
    //    if (AssignedAgency == null)
    //    {
    //        return;
    //    }
    //    vehicleExt.Vehicle.LicensePlate = AssignedAgency.LicensePlatePrefix + RandomItems.RandomString(8 - AssignedAgency.LicensePlatePrefix.Length);
    //    //if (RequiredLiveries == null || !RequiredLiveries.Any())
    //    //{
    //    //    return;
    //    //}
    //    //NativeFunction.CallByName<bool>("SET_VEHICLE_LIVERY", vehicleExt.Vehicle, RequiredLiveries.PickRandom());
    //}


}
