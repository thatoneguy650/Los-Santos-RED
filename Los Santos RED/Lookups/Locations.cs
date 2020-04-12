using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class Locations
{
    private static string ConfigFileName = "Plugins\\LosSantosRED\\Locations.xml";
    public static List<Location> LocationsList;

    public static void Initialize()
    {
        ReadConfig();
        CreateBlips();
    }
    public static void Dispose()
    {

    }
    public static void ReadConfig()
    {
        if (File.Exists(ConfigFileName))
        {
            LocationsList = LosSantosRED.DeserializeParams<Location>(ConfigFileName);
        }
        else
        {
            DefaultConfig();
            LosSantosRED.SerializeParams(LocationsList, ConfigFileName);
        }
    }
    private static void DefaultConfig()
    {
        LocationsList = new List<Location>
        {
            //Hospital
            new Location(new Vector3(364.7124f, -583.1641f, 28.69318f), 280.637f, Location.LocationType.Hospital, "Pill Box Hill Hospital"),
            new Location(new Vector3(338.208f, -1396.154f, 32.50927f), 77.07102f, Location.LocationType.Hospital, "Central Los Santos Hospital"),
            new Location(new Vector3(1842.057f, 3668.679f, 33.67996f), 228.3818f, Location.LocationType.Hospital, "Sandy Shores Hospital"),
            new Location(new Vector3(-244.3214f, 6328.575f, 32.42618f), 219.7734f, Location.LocationType.Hospital, "Paleto Bay Hospital"),
            //Police
            new Location(new Vector3(358.9726f, -1582.881f, 29.29195f), 323.5287f, Location.LocationType.Police, "Davis Police Station"),
            new Location(new Vector3(1858.19f, 3679.873f, 33.75724f), 218.3256f, Location.LocationType.Police, "Sandy Shores Police Station"),
            new Location(new Vector3(-437.973f, 6021.403f, 31.49011f), 316.3756f, Location.LocationType.Police, "Paleto Bay Police Station"),
            new Location(new Vector3(440.0835f, -982.3911f, 30.68966f), 47.88088f, Location.LocationType.Police, "Mission Row Police Station"),
            new Location(new Vector3(815.8774f, -1290.531f, 26.28391f), 74.91704f, Location.LocationType.Police, "La Mesa Police Station"),
            new Location(new Vector3(642.1356f, -3.134667f, 82.78872f), 215.299f, Location.LocationType.Police, "Vinewood Police Station"),
            new Location(new Vector3(-557.0687f, -134.7315f, 38.20231f), 214.5968f, Location.LocationType.Police, "Rockford Hills Police Station"),
            new Location(new Vector3(-1093.817f, -807.1993f, 19.28864f), 22.23846f, Location.LocationType.Police, "Vespucci Police Station"),
            new Location(new Vector3(-1633.314f, -1010.025f, 13.08503f), 351.7007f, Location.LocationType.Police, "Del Perro Police Station"),
            new Location(new Vector3(-1311.877f, -1528.808f, 4.410581f), 233.9121f, Location.LocationType.Police, "Vespucci Beach Police Station"),
            //Stores
            new Location(new Vector3(-709.68f, -923.198f, 19.0193f), 22.23846f, Location.LocationType.ConvenienceStore, "LTD Gas - Little Seoul"),
            new Location(new Vector3(-1226.09f, -896.166f, 12.4057f), 22.23846f, Location.LocationType.ConvenienceStore, "Rob's Liquors"),
            new Location(new Vector3(1725f, 6410f, 35f), 22.23846f, Location.LocationType.ConvenienceStore, "24/7 Store 1"),
            new Location(new Vector3(2560f, 385f, 107f), 22.23846f, Location.LocationType.ConvenienceStore, "24/7 Store 2"),
            new Location(new Vector3(547f, 2678f, 41f), 22.23846f, Location.LocationType.ConvenienceStore, "24/7 Store 3")
    };
    }
    private static void CreateBlips()
    {
        foreach(Location MyLocation in LocationsList)
        {
            MyLocation.CreateLocationBlip();
        }
    }
    public static Location GetClosestLocationByType(Vector3 Position,Location.LocationType Type)
    {
        return LocationsList.Where(x => x.Type == Type).OrderBy(s => Position.DistanceTo2D(s.LocationPosition)).FirstOrDefault();
    }
    public static List<Location> GetAllLocationsOfType(Location.LocationType Type)
    {
        return LocationsList.Where(x => x.Type == Type).ToList();
    }
}
[Serializable()]
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
    public string Name;
    public Location()
    {

    }
    public Location(Vector3 _LocationPosition, float _Heading, LocationType _Type, String _Name)
    {
        LocationPosition = _LocationPosition;
        Heading = _Heading;
        Type = _Type;
        Name = _Name;     
    }
    public void CreateLocationBlip()
    {
        Blip MyLocationBlip = new Blip(LocationPosition)
        {
        Name = Name
        };

        if (Type == LocationType.Hospital)
        {
            MyLocationBlip.Sprite = BlipSprite.Hospital;
            MyLocationBlip.Color = Color.White;
        }
        else if (Type == LocationType.Police)
        {
            MyLocationBlip.Sprite = BlipSprite.PoliceStation;
            MyLocationBlip.Color = Color.White;
        }
        else if (Type == LocationType.ConvenienceStore)
        {
            MyLocationBlip.Sprite = BlipSprite.CriminalHoldups;
            MyLocationBlip.Color = Color.White;
        }

        NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)MyLocationBlip.Handle, true);
        Police.CreatedBlips.Add(MyLocationBlip);
    }
    public override string ToString()
    {
        return Name.ToString();
    }
}

