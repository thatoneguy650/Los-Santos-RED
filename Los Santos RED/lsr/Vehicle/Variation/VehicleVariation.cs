using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
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


    public LicensePlate LicensePlate { get; set; }



    public int WheelType { get; set; }

    public int WindowTint { get; set; }

    public bool HasCustomWheels { get; set; }



    public List<VehicleExtra> VehicleExtras { get; set; } = new List<VehicleExtra>();
    public List<VehicleToggle> VehicleToggles { get; set; } = new List<VehicleToggle>();
    public List<VehicleMod> VehicleMods { get; set; } = new List<VehicleMod>();


    public int DirtLevel { get; set; } = 0;

    public void Apply(VehicleExt vehicleExt)
    {

        //doesnt do wheels or tires right, might be missing some others, does some turbo and transmission stuff properly, mod colors IDK what the fuck those do
        if(vehicleExt != null && vehicleExt.Vehicle.Exists())
        {
            if(LicensePlate != null)
            {
                vehicleExt.Vehicle.LicensePlate = LicensePlate.PlateNumber;
                NativeFunction.Natives.SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX(vehicleExt.Vehicle, LicensePlate.PlateType);
            }


            NativeFunction.Natives.SET_VEHICLE_MOD_KIT(vehicleExt.Vehicle, 0);
            NativeFunction.Natives.SET_VEHICLE_WINDOW_TINT(vehicleExt.Vehicle, WindowTint);
            NativeFunction.Natives.SET_VEHICLE_WHEEL_TYPE(vehicleExt.Vehicle, WheelType);



            foreach(VehicleExtra vehicleExtra in VehicleExtras)
            {
                NativeFunction.Natives.SET_VEHICLE_EXTRA(vehicleExt.Vehicle, vehicleExtra.ID, vehicleExtra.IsTurnedOn);
            }





            foreach(VehicleToggle vehicleToggle in VehicleToggles)
            {
                NativeFunction.Natives.TOGGLE_VEHICLE_MOD(vehicleExt.Vehicle, vehicleToggle.ID, vehicleToggle.IsTurnedOn);
            }

            foreach(VehicleMod vehicleMod in VehicleMods)
            {
                NativeFunction.Natives.SET_VEHICLE_MOD(vehicleExt.Vehicle, vehicleMod.ID, vehicleMod.Output);
            }







            //int customWheelID = 23;
            //if (vehicleExt.Vehicle.IsBike)
            //{
            //    customWheelID = 24;
            //}
            //if (HasCustomWheels)
            //{
            //    NativeFunction.Natives.SET_VEHICLE_MOD(vehicleExt.Vehicle, customWheelID, true);
            //}
            //else
            //{
            //    NativeFunction.Natives.SET_VEHICLE_MOD(vehicleExt.Vehicle, customWheelID, false);
            //}







            if (Livery != -1)
            {
                NativeFunction.Natives.SET_VEHICLE_LIVERY(vehicleExt.Vehicle, Livery);
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

            if(DirtLevel < 0)
            {
                DirtLevel = 0;
            }
            if (DirtLevel > 15)
            {
                DirtLevel = 15;
            }
            NativeFunction.Natives.SET_VEHICLE_DIRT_LEVEL(vehicleExt.Vehicle, DirtLevel);



        }
    }

}

