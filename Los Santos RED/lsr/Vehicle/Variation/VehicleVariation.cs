using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class VehicleVariation
{
    public VehicleVariation()
    {

    }
    public int PrimaryColor { get; set; }
    public int SecondaryColor { get; set; }
    public bool IsPrimaryColorCustom { get; set; }
    public System.Drawing.Color CustomPrimaryColor { get; set; }
    public bool IsSecondaryColorCustom { get; set; }
    public System.Drawing.Color CustomSecondaryColor { get; set; }
    public int PearlescentColor { get; set; }
    public int WheelColor { get; set; }
    public int Mod1PaintType { get; set; }
    public int Mod1Color { get; set; }
    public int Mod1PearlescentColor { get; set; }
    public int Mod2PaintType { get; set; }
    public int Mod2Color { get; set; }
    public int Livery { get; set; } = -1;
    public int Livery2 { get; set; } = -1;
    public LicensePlate LicensePlate { get; set; }
    public int WheelType { get; set; }
    public int WindowTint { get; set; }
    public bool HasCustomWheels { get; set; }
    public List<VehicleExtra> VehicleExtras { get; set; } = new List<VehicleExtra>();
    public List<VehicleToggle> VehicleToggles { get; set; } = new List<VehicleToggle>();
    public List<VehicleMod> VehicleMods { get; set; } = new List<VehicleMod>();
    public List<VehicleNeon> VehicleNeons { get; set; } = new List<VehicleNeon>();
    public float FuelLevel { get; set; } = 65.0f;
    public float DirtLevel { get; set; } = 0.0f;
    public bool HasInvicibleTires { get; set; } = false;
    public bool IsTireSmokeColorCustom { get; set; } = false;
    public int TireSmokeColorR { get; set; }
    public int TireSmokeColorG { get; set; }
    public int TireSmokeColorB { get; set; }
    public int NeonColorR { get; set; }
    public int NeonColorG { get; set; }
    public int NeonColorB { get; set; }
    public int InteriorColor { get; set; } = -1;
    public int DashboardColor { get; set; } = -1;
    public int XenonLightColor { get; set; } = -1;
    public bool CanTiresBurst { get; set; }

    public void Apply(VehicleExt vehicleExt)
    {
        if (vehicleExt == null || !vehicleExt.Vehicle.Exists())
        {
            return;
            
        }
        if(LicensePlate != null)
        {
            string plateNumber = LicensePlate.PlateNumber;
            if (string.IsNullOrEmpty(LicensePlate.PlateNumber))
            {
                plateNumber = NativeHelper.GenerateNewLicensePlateNumber("12ABC345");
            }
            vehicleExt.Vehicle.LicensePlate = plateNumber;
            NativeFunction.Natives.SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX(vehicleExt.Vehicle, LicensePlate.PlateType);
            vehicleExt.HasUpdatedPlateType = true;
        }
        NativeFunction.Natives.SET_VEHICLE_MOD_KIT(vehicleExt.Vehicle, 0);
        //EntryPoint.WriteToConsole("SETTING VEHICLE MODS 22222");
        NativeFunction.Natives.SET_VEHICLE_WINDOW_TINT(vehicleExt.Vehicle, WindowTint);
        NativeFunction.Natives.SET_VEHICLE_WHEEL_TYPE(vehicleExt.Vehicle, WheelType);
        foreach (VehicleExtra vehicleExtra in VehicleExtras.OrderBy(x => x.ID))
        {
            NativeFunction.Natives.SET_VEHICLE_EXTRA(vehicleExt.Vehicle, vehicleExtra.ID, !vehicleExtra.IsTurnedOn);
        }
        foreach(VehicleToggle vehicleToggle in VehicleToggles.OrderBy(x=> x.ID))
        {
            NativeFunction.Natives.TOGGLE_VEHICLE_MOD(vehicleExt.Vehicle, vehicleToggle.ID, vehicleToggle.IsTurnedOn);
        }
        foreach(VehicleMod vehicleMod in VehicleMods)
        {
            bool isCustomTires = false;
            if(vehicleExt.Vehicle.IsBike)
            {
                if (vehicleMod.ID == 24)
                {
                    isCustomTires = HasCustomWheels;
                }
            }
            else
            {
                if (vehicleMod.ID == 23)
                {
                    isCustomTires = HasCustomWheels;
                }
            }
            NativeFunction.Natives.SET_VEHICLE_MOD(vehicleExt.Vehicle, vehicleMod.ID, vehicleMod.Output, isCustomTires);
        }
        GameFiber.Yield();
        if (vehicleExt == null || !vehicleExt.Vehicle.Exists())
        {
            return;
        }
        if (Livery != -1)
        {
            NativeFunction.Natives.SET_VEHICLE_LIVERY(vehicleExt.Vehicle, Livery);
        }
        if(Livery2 != -1)
        {
            NativeFunction.Natives.SET_VEHICLE_LIVERY2(vehicleExt.Vehicle, Livery2);
        }
        if (PrimaryColor != -1)
        {
            NativeFunction.Natives.SET_VEHICLE_COLOURS(vehicleExt.Vehicle, PrimaryColor, SecondaryColor);
        }
        if (IsPrimaryColorCustom)
        {
            NativeFunction.Natives.SET_VEHICLE_CUSTOM_PRIMARY_COLOUR(vehicleExt.Vehicle, CustomPrimaryColor.R, CustomPrimaryColor.G, CustomPrimaryColor.B);
        }
        if (IsSecondaryColorCustom)
        {
            NativeFunction.Natives.SET_VEHICLE_CUSTOM_SECONDARY_COLOUR(vehicleExt.Vehicle, CustomSecondaryColor.R, CustomSecondaryColor.G, CustomSecondaryColor.B);
        }
        NativeFunction.Natives.SET_VEHICLE_EXTRA_COLOURS(vehicleExt.Vehicle, PearlescentColor, WheelColor);
        if (Mod1PaintType != 0 || Mod1Color != 0 || Mod1PearlescentColor != 0)
        {
            NativeFunction.Natives.SET_VEHICLE_MOD_COLOR_1(vehicleExt.Vehicle, Mod1PaintType, Mod1Color, Mod1PearlescentColor);
        }
        if (Mod2PaintType != 0 || Mod2Color != 0)
        {
            NativeFunction.Natives.SET_VEHICLE_MOD_COLOR_2(vehicleExt.Vehicle, Mod2PaintType, Mod2Color);
        }
        NativeFunction.Natives.SET_VEHICLE_DIRT_LEVEL(vehicleExt.Vehicle, DirtLevel.Clamp(0.0f,15.0f));
        vehicleExt.Vehicle.FuelLevel = FuelLevel;        
        NativeFunction.Natives.SET_VEHICLE_TYRES_CAN_BURST(vehicleExt.Vehicle, !HasInvicibleTires);

        if(IsTireSmokeColorCustom)
        {
            NativeFunction.Natives.SET_VEHICLE_TYRE_SMOKE_COLOR(vehicleExt.Vehicle,TireSmokeColorR,TireSmokeColorG,TireSmokeColorB);
        }
        foreach(VehicleNeon vehicleNeon in VehicleNeons)
        {
            NativeFunction.Natives.SET_VEHICLE_NEON_ENABLED(vehicleExt.Vehicle, vehicleNeon.ID, vehicleNeon.IsEnabled);
        }
        NativeFunction.Natives.SET_VEHICLE_NEON_COLOUR(vehicleExt.Vehicle,NeonColorR,NeonColorG,NeonColorB);
        if (InteriorColor != -1)
        {
            NativeFunction.Natives.SET_VEHICLE_EXTRA_COLOUR_5(vehicleExt.Vehicle, InteriorColor);
        }
        if(DashboardColor != -1)
        {
            NativeFunction.Natives.SET_VEHICLE_EXTRA_COLOUR_6(vehicleExt.Vehicle, DashboardColor);
        }
        if(XenonLightColor != -1)
        {
            NativeFunction.Natives.SET_VEHICLE_XENON_LIGHT_COLOR_INDEX(vehicleExt.Vehicle, XenonLightColor);
        }

        
    }

}

