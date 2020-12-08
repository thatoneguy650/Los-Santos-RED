using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr
{
    public class Map
    {
        private List<Blip> CreatedBlips;
        public Map()
        {
            
        }
        public void CreateLocationBlips()
        {
            CreatedBlips = new List<Blip>();
            foreach (GameLocation MyLocation in LocationManager.GetAllLocations())
            {
                MapBlip myBlip = new MapBlip(MyLocation.LocationPosition, MyLocation.Name, MyLocation.Type);
                myBlip.Create();
            }
        }
        public void RemoveBlips()
        {
            foreach (Blip MyBlip in CreatedBlips)
            {
                if (MyBlip.Exists())
                    MyBlip.Delete();
            }
        }
        public void AddBlip(Blip myBlip)//temp, move everything that creates blips outside of this in here.
        {
            CreatedBlips.Add(myBlip);
        }
        private class MapBlip
        {
            private BlipSprite Icon
            {
                get
                {
                    if (Type == LocationType.Hospital)
                    {
                        return BlipSprite.Hospital;
                    }
                    else if (Type == LocationType.Police)
                    {
                        return BlipSprite.PoliceStation;
                    }
                    else if (Type == LocationType.ConvenienceStore)
                    {
                        return BlipSprite.CriminalHoldups;
                    }
                    else if (Type == LocationType.GasStation)
                    {
                        return BlipSprite.JerryCan;
                    }
                    else
                    {
                        return BlipSprite.Objective;
                    }
                }
            }
            public Vector3 LocationPosition { get; }
            public string Name { get; }
            public LocationType Type { get; }
            public MapBlip(Vector3 LocationPosition, string Name, LocationType Type)
            {
                this.LocationPosition = LocationPosition;
                this.Name = Name;
                this.Type = Type;
            }
            public void Create()
            {
                Blip MyLocationBlip = new Blip(LocationPosition)
                {
                    Name = Name
                };
                MyLocationBlip.Sprite = Icon;
                MyLocationBlip.Color = Color.White;
                NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)MyLocationBlip.Handle, true);
            }
        }


    }
}
