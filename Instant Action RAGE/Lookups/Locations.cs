using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class Locations
{
    public static List<Location> LocationsList = new List<Location>();
    //Hospital
    public static Location PillBoxHillHospital = new Location(new Vector3(364.7124f, -583.1641f, 28.69318f), 280.637f, Location.LocationType.Hospital, "Pill Box Hill Hospital");
    public static Location CentralLosStantosHospital = new Location(new Vector3(338.208f, -1396.154f, 32.50927f), 77.07102f, Location.LocationType.Hospital, "Central Los Santos Hospital");
    public static Location SandyShoresHospital = new Location(new Vector3(1842.057f, 3668.679f, 33.67996f), 228.3818f, Location.LocationType.Hospital, "Sandy Shores Hospital");
    public static Location PaletoBayHospital = new Location(new Vector3(-244.3214f, 6328.575f, 32.42618f), 219.7734f, Location.LocationType.Hospital, "Paleto Bay Hospital");
    //Police
    public static Location DavisPolice = new Location(new Vector3(358.9726f, -1582.881f, 29.29195f), 323.5287f, Location.LocationType.Police, "Davis Police Station");
    public static Location SandyShoresPolice = new Location(new Vector3(1858.19f, 3679.873f, 33.75724f), 218.3256f, Location.LocationType.Police, "Sandy Shores Police Station");
    public static Location PaletoBayPolice = new Location(new Vector3(-437.973f, 6021.403f, 31.49011f), 316.3756f, Location.LocationType.Police, "Paleto Bay Police Station");
    public static Location MissionRowPolice = new Location(new Vector3(440.0835f, -982.3911f, 30.68966f), 47.88088f, Location.LocationType.Police, "Mission Row Police Station");
    public static Location LasMesaPolice = new Location(new Vector3(815.8774f, -1290.531f, 26.28391f), 74.91704f, Location.LocationType.Police, "La Mesa Police Station");
    public static Location VinewoodPolice = new Location(new Vector3(642.1356f, -3.134667f, 82.78872f), 215.299f, Location.LocationType.Police, "Vinewood Police Station");
    public static Location RockfordHillsPolice = new Location(new Vector3(-557.0687f, -134.7315f, 38.20231f), 214.5968f, Location.LocationType.Police, "Vinewood Police Station");
    public static Location VespucciPolice = new Location(new Vector3(-1093.817f, -807.1993f, 19.28864f), 22.23846f, Location.LocationType.Police, "Vinewood Police Station");
    //Stores
    public static Location LTDGasLIttleSeoul = new Location(new Vector3(-709.68f, -923.198f, 19.0193f), 22.23846f, Location.LocationType.ConvenienceStore, "LTD Gas - Little Seoul");
    public static Location RobsLiquors = new Location(new Vector3(-1226.09f, -896.166f, 12.4057f), 22.23846f, Location.LocationType.ConvenienceStore, "Rob's Liquors");
    public static Location Store1 = new Location(new Vector3(1725f, 6410f, 35f), 22.23846f, Location.LocationType.ConvenienceStore, "24/7 Store");
    public static Location Store2 = new Location(new Vector3(2560f, 385f, 107f), 22.23846f, Location.LocationType.ConvenienceStore, "24/7 Store");
    public static Location Store3 = new Location(new Vector3(547f, 2678f, 41f), 22.23846f, Location.LocationType.ConvenienceStore, "24/7 Store");
    public static void Initialize()
    {
        LocationsList.Add(PillBoxHillHospital);
        LocationsList.Add(CentralLosStantosHospital);
        LocationsList.Add(SandyShoresHospital);
        LocationsList.Add(PaletoBayHospital);
        LocationsList.Add(DavisPolice);
        LocationsList.Add(SandyShoresPolice);
        LocationsList.Add(PaletoBayPolice);
        LocationsList.Add(MissionRowPolice);
        LocationsList.Add(LasMesaPolice);
        LocationsList.Add(VinewoodPolice);
        LocationsList.Add(RockfordHillsPolice);
        LocationsList.Add(VespucciPolice);
        LocationsList.Add(LTDGasLIttleSeoul);
        LocationsList.Add(RobsLiquors);
        LocationsList.Add(Store1);
        LocationsList.Add(Store2);
        LocationsList.Add(Store3);
    }
    public static Location GetClosestLocationByType(Vector3 Position,Location.LocationType Type)
    {
        return LocationsList.Where(x => x.Type == Type).OrderBy(s => Position.DistanceTo2D(s.LocationPosition)).FirstOrDefault();
    }
    
}
public class Location
{
    public enum LocationType : int
    {
        Police = 0,
        Hospital = 1,
        ConvenienceStore = 2,
    }
    public Vector3 LocationPosition;
    public float Heading;
    public LocationType Type;
    public String Name;
    public Blip LocationBlip;
    public Location()
    {

    }
    public Location(Vector3 _LocationPosition, float _Heading, LocationType _Type, String _Name)
    {
        LocationPosition = _LocationPosition;
        Heading = _Heading;
        Type = _Type;
        Name = _Name;

        LocationBlip = new Blip(LocationPosition);
        LocationBlip.Name = _Name;

        if (Type == LocationType.Hospital)
        {
            LocationBlip.Sprite = BlipSprite.Hospital;
            LocationBlip.Color = Color.White;
        }
        else if (Type == LocationType.Police)
        {
            LocationBlip.Sprite = BlipSprite.PoliceStation;
            LocationBlip.Color = Color.White;
        }
        else if (Type == LocationType.ConvenienceStore)
        {
            LocationBlip.Sprite = BlipSprite.CriminalHoldups;
            LocationBlip.Color = Color.White;
        }

        NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)LocationBlip.Handle, true);
        Police.CreatedBlips.Add(LocationBlip);
    }
}

