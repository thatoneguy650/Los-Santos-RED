using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ExtensionsMethods;
using Rage.Native;

public static class Vehicles
{
    public static void Initialize()
    {

    }
    public static void Dispose()
    {

    }
    public static Color VehicleColor(VehicleExt VehicleToDescribe)
    {
        if (VehicleToDescribe.VehicleEnt.Exists())
        {
            Color BaseColor = Extensions.GetBaseColor(VehicleToDescribe.DescriptionColor);
            return BaseColor;
        }
        else
        {
            return Color.White;
        }
    }
    public static string MakeName(VehicleExt VehicleToDescribe)
    {
        if (VehicleToDescribe.VehicleEnt.Exists())
        {
            string MakeName;
            unsafe
            {
                IntPtr ptr = NativeFunction.CallByHash<IntPtr>(0xF7AF4F159FF99F97, VehicleToDescribe.VehicleEnt.Model.Hash);
                MakeName = Marshal.PtrToStringAnsi(ptr);
            }
            unsafe
            {
                IntPtr ptr2 = NativeFunction.CallByHash<IntPtr>(0x7B5280EBA9840C72, MakeName);
                MakeName = Marshal.PtrToStringAnsi(ptr2);
            }
            if (MakeName == "CARNOTFOUND" || MakeName == "NULL")
                return "";
            else
                return MakeName;
        }
        else
        {
            return "";
        }

    }
    public static string ModelName(VehicleExt VehicleToDescribe)
    {
        if (VehicleToDescribe.VehicleEnt.Exists())
        {
            string ModelName;
            unsafe
            {
                IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_DISPLAY_NAME_FROM_VEHICLE_MODEL", VehicleToDescribe.VehicleEnt.Model.Hash);
                ModelName = Marshal.PtrToStringAnsi(ptr);
            }
            unsafe
            {
                IntPtr ptr2 = NativeFunction.CallByHash<IntPtr>(0x7B5280EBA9840C72, ModelName);
                ModelName = Marshal.PtrToStringAnsi(ptr2);
            }
            if (ModelName == "CARNOTFOUND" || ModelName == "NULL")
                return "";
            else
                return ModelName;
        }
        else
        {
            return "";
        }
    }
    public static int ClassInt(VehicleExt VehicleToDescribe)
    {
        int ClassInt = NativeFunction.CallByName<int>("GET_VEHICLE_CLASS", VehicleToDescribe.VehicleEnt);
        return ClassInt;
    }
}

