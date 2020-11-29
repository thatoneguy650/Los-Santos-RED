using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class AmbientPlateManager
{
    private static List<AmbientVehicle> AmbientVehicles = new List<AmbientVehicle>();
    public static bool IsRunning { get; set; }
    public static void Initialize()
    {
        IsRunning = true;
        AmbientVehicles = new List<AmbientVehicle>();
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void Tick()
    {
        if (IsRunning)
        {
            UpdateVehiclePlates();
        }
    }

    private static void UpdateVehiclePlates()
    {
        int VehiclesUpdated = 0;
        AmbientVehicles.RemoveAll(x => !x.CivVehicle.VehicleEnt.Exists());
        foreach (VehicleExt MyCar in PedManager.CivilianVehicles.Where(x => x.VehicleEnt.Exists()))
        {
            if (!AmbientVehicles.Any(x => x.CivVehicle.VehicleEnt.Handle == MyCar.VehicleEnt.Handle))
            {
                AmbientVehicle AmbCar = new AmbientVehicle(MyCar);
                AmbCar.UpdatePlate();
                AmbientVehicles.Add(AmbCar);
                VehiclesUpdated++;
            }
            if(VehiclesUpdated > 10)
            {
                break;
            }
        }
    }
    private class AmbientVehicle
    {
        public AmbientVehicle(VehicleExt vehicle)
        {
            CivVehicle = vehicle;
        }
        public VehicleExt CivVehicle { get; set; }    
        public void UpdatePlate()
        {
            if(NativeFunction.CallByName<int>("GET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", CivVehicle.VehicleEnt) != 4 && General.RandomPercent(30))//SA Exempt don't change, only change 30% of plates
            {
                int NewIndex = General.MyRand.Next(5, 48);
                NativeFunction.CallByName<int>("SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", CivVehicle.VehicleEnt, NewIndex);
                if (CivVehicle.CarPlate.PlateType == CivVehicle.OriginalLicensePlate.PlateType)
                {   
                    CivVehicle.OriginalLicensePlate.PlateType = NewIndex;
                }
                CivVehicle.CarPlate.PlateType = NewIndex;
            }
        }
    }
    private class LicensePlateChance
    {
        public int PlateType { get; set; }
        public LicensePlateChance(int plateType)
        {
            PlateType = plateType;
        }
    }
}

