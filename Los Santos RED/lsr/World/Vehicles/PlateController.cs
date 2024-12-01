using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PlateController
{
    private Vehicles Vehicles;
    private IZones Zones;
    private IPlateTypes PlateTypes;
    private ISettingsProvideable Settings;

    public PlateController(Vehicles vehicles, IZones zones, IPlateTypes plateTypes, ISettingsProvideable settings)
    {
        Vehicles = vehicles;
        Zones = zones;
        PlateTypes = plateTypes;
        Settings = settings;
    }
    public void UpdatePlates()
    {
        if (Settings.SettingsManager.WorldSettings.UpdateVehiclePlates)
        {
            try
            {
                int VehiclesUpdated = 0;
                foreach (VehicleExt MyCar in Vehicles.NonServiceVehicles.Where(x => x.Vehicle.Exists() && !x.HasUpdatedPlateType && !x.CanNeverUpdatePlate).ToList().Take(20))
                {
                    if (MyCar.Vehicle.Exists())
                    {
                        MyCar.UpdatePlateType(false, Zones, PlateTypes, true, false);
                    }
                    VehiclesUpdated++;
                    GameFiber.Yield();
                }
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole($"Update Plates Error: {ex.Message} {ex.StackTrace}", 0);
            }
        }
    }
    //private void UpdatePlateType(VehicleExt vehicleExt, bool force)//this might need to come out of here.... along with the two bools
    //{
    //    if(!vehicleExt.Vehicle.Exists())
    //    {
    //        return;
    //    }
    //    vehicleExt.HasUpdatedPlateType = true;
    //    PlateType CurrentType = PlateTypes.GetPlateType(NativeFunction.CallByName<int>("GET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", vehicleExt.Vehicle));
    //    Zone CurrentZone = Zones.GetZone(vehicleExt.Vehicle.Position);
    //    PlateType NewType = null;
    //    if (force)
    //    {
    //        NewType = PlateTypes.GetRandomPlateType();
    //    }
    //    else if (CurrentZone != null && CurrentZone.State != "San Andreas")//change the plates based on state
    //    {
    //        NewType = PlateTypes.GetPlateType(CurrentZone.State);
    //    }
    //    else
    //    {
    //        if (RandomItems.RandomPercent(Settings.SettingsManager.WorldSettings.RandomVehiclePlatesPercent) && CurrentType != null && CurrentType.CanOverwrite && vehicleExt.CanUpdatePlate)
    //        {
    //            NewType = PlateTypes.GetRandomPlateType();
    //        }
    //    }
    //    if (NewType != null)
    //    {
    //        string NewPlateNumber;
    //        if (Settings.SettingsManager.WorldSettings.AllowRandomVanityPlates && RandomItems.RandomPercent(Settings.SettingsManager.WorldSettings.RandomVehicleVanityPlatesPercent))
    //        {
    //            NewPlateNumber = PlateTypes.GetRandomVanityPlateText();
    //        }
    //        else
    //        {
    //            NewPlateNumber = NewType.GenerateNewLicensePlateNumber();
    //        }
    //        if (NewPlateNumber != "")
    //        {
    //            vehicleExt.Vehicle.LicensePlate = NewPlateNumber;
    //            vehicleExt.OriginalLicensePlate.PlateNumber = NewPlateNumber;
    //            vehicleExt.CarPlate.PlateNumber = NewPlateNumber;
    //        }
    //        else
    //        {
    //            NewPlateNumber = RandomItems.RandomString(8);
    //            vehicleExt.Vehicle.LicensePlate = NewPlateNumber;
    //            vehicleExt.OriginalLicensePlate.PlateNumber = NewPlateNumber;
    //            vehicleExt.CarPlate.PlateNumber = NewPlateNumber;
    //        }
    //        if (NewType.Index <= NativeFunction.CallByName<int>("GET_NUMBER_OF_VEHICLE_NUMBER_PLATES"))
    //        {
    //            NativeFunction.CallByName<int>("SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", vehicleExt.Vehicle, NewType.Index);
    //            vehicleExt.OriginalLicensePlate.PlateType = NewType.Index;
    //            vehicleExt.CarPlate.PlateType = NewType.Index;         
    //        }
    //    }
    //    else
    //    {
    //        string NewPlateNumber;
    //        if (Settings.SettingsManager.WorldSettings.AllowRandomVanityPlates && RandomItems.RandomPercent(Settings.SettingsManager.WorldSettings.RandomVehicleVanityPlatesPercent))
    //        {
    //            NewPlateNumber = PlateTypes.GetRandomVanityPlateText();
    //            if (NewPlateNumber != "")
    //            {
    //                vehicleExt.Vehicle.LicensePlate = NewPlateNumber;
    //                vehicleExt.OriginalLicensePlate.PlateNumber = NewPlateNumber;
    //                vehicleExt.CarPlate.PlateNumber = NewPlateNumber;
    //            }
    //        }
    //    }  
    //}
}

