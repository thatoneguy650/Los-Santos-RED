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
    public class World
    {
        private List<Blip> CreatedBlips;

        public PoliceSpawning PoliceSpawning { get; private set; } = new PoliceSpawning();
        public Vehicles Vehicles { get; private set; } = new Vehicles();
        public Tasking Tasking { get; private set; } = new Tasking();
        public PedDamage PedDamage { get; private set; } = new PedDamage();
        public Scanner Scanner { get; private set; } = new Scanner();
        public Pedestrians Pedestrians { get; private set; } = new Pedestrians();
        public Time Clock { get; private set; } = new Time();
        public PoliceForce PoliceForce { get; private set; } = new PoliceForce();
        public Civilians Civilians { get; private set; } = new Civilians();
        public Dispatch Dispatch { get; private set; } = new Dispatch();
        public PedSwap PedSwap { get; private set; } = new PedSwap();
        public bool IsNightTime { get; private set; }
        public World()
        {
            
        }
        public void CacheWorldData()
        {
            IsNightTime = false;
            var HourOfDay = NativeFunction.CallByName<int>("GET_CLOCK_HOURS");
            //var MinuteOfDay = NativeFunction.CallByName<int>("GET_CLOCK_MINUTES");
            if (HourOfDay >= 19 || HourOfDay <= 6)//7pm to 6 am lights need to be on
                IsNightTime = true;
        }
        public void PuneVehicleList()
        {

        }
        public void Dispose()
        {
            RemoveBlips();
            Clock.Dispose();
            Pedestrians.Dispose();
            Dispatch.Dispose();
            PoliceSpawning.Dispose();
            Vehicles.ClearPolice();
            Dispatch.Dispose();
        }
        public void CreateLocationBlips()
        {
            CreatedBlips = new List<Blip>();
            foreach (GameLocation MyLocation in Mod.DataMart.Places.GetAllPlaces())
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
