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
    public enum Manufacturer
    {
        Albany = 1,
        Annis = 2,
        Benefactor = 3,
        Burgerfahrzeug = 4,
        Bollokan = 5,
        Bravado = 6,
        Brute = 7,
        Buckingham = 8,
        Canis = 9,
        Chariot = 10,
        Cheval = 11,
        Classique = 12,
        Coil = 13,
        Declasse = 14,
        Dewbauchee = 15,
        Dinka = 16,
        DUDE = 17,
        Dundreary = 18,
        Emperor = 19,
        Enus = 20,
        Fathom = 21,
        Gallivanter = 22,
        Grotti = 23,
        HVY = 24,
        Hijak = 25,
        Imponte = 26,
        Invetero = 27,
        JackSheepe = 28,
        Jobuilt = 29,
        Karin = 30,
        KrakenSubmersibles = 31,
        Lampadati = 32,
        LibertyChopShop = 33,
        LibertyCityCycles = 34,
        MaibatsuCorporation = 35,
        Mammoth = 36,
        MTL = 37,
        Nagasaki = 38,
        Obey = 39,
        Ocelot = 40,
        Overflod = 41,
        Pegassi = 42,
        Pfister = 43,
        Principe = 44,
        Progen = 45,
        ProLaps = 46,
        RUNE = 47,
        Schyster = 48,
        Shitzu = 49,
        Speedophile = 50,
        Stanley = 51,
        SteelHorse = 52,
        Truffade = 53,
        Ubermacht = 54,
        Vapid = 55,
        Vulcar = 56,
        Vysser = 57,
        Weeny = 58,
        WesternCompany = 59,
        WesternMotorcycleCompany = 60,
        Willard = 61,
        Zirconium = 62,
        Unknown = 63,
    }
    public enum VehicleClass
    {
        Compacts = 0,
        Sedans = 1,
        SUVs = 2,
        Coupes = 3,
        Muscle = 4,
        SportsClassics = 5,
        Sports = 6,
        Super = 7,
        Motorcycles = 8,
        OffRoad = 9,
        Industrial = 10,
        Utility = 11,
        Vans = 12,
        Cycles = 13,
        Boats = 14,
        Helicopters = 15,
        Planes = 16,
        Service = 17,
        Emergency = 18,
        Military = 19,
        Commercial = 20,
        Trains = 21,
        Unknown = 24,//unofficial
        Trailer = 25,//unofficial
    }
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

