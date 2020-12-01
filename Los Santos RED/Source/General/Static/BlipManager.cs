using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class BlipManager
{
    private static List<Blip> CreatedBlips;
    public static void Initialize()
    {
        CreatedBlips = new List<Blip>();
        foreach (GameLocation MyLocation in LocationManager.GetAllLocations())
        {
            CreateBlip(MyLocation.LocationPosition, MyLocation.Name, MyLocation.Type);
        }   
    }
    public static void Dispose()
    {
        foreach (Blip MyBlip in CreatedBlips)
        {
            if (MyBlip.Exists())
                MyBlip.Delete();
        }
    }
    public static void AddBlip(Blip myBlip)//temp, move everything that creates blips outside of this in here.
    {
        CreatedBlips.Add(myBlip);
    }
    public static void CreateBlip(Vector3 LocationPosition, string Name, LocationType Type)
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
        else if (Type == LocationType.GasStation)
        {
            MyLocationBlip.Sprite = BlipSprite.JerryCan;
            MyLocationBlip.Color = Color.White;
        }
        NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)MyLocationBlip.Handle, true);
        CreatedBlips.Add(MyLocationBlip);
    }
}
