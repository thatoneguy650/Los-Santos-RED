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
        public Spawner Spawner { get; private set; } = new Spawner();
        public Vehicles Vehicles { get; private set; } = new Vehicles();
        public Pedestrians Pedestrians { get; private set; } = new Pedestrians();
        public Tasking Tasking { get; private set; } = new Tasking();
        public Wounds Wounds { get; private set; } = new Wounds();
        public Scanner Scanner { get; private set; } = new Scanner();
        public Dispatcher Dispatcher { get; private set; } = new Dispatcher();
        public Time Time { get; private set; } = new Time();
        public Police Police { get; private set; } = new Police();//maybe go under pedestrians?
        public Civilians Civilians { get; private set; } = new Civilians();
        public PedSwap PedSwap { get; private set; } = new PedSwap();//dunno about this, go in player? go static?
        public bool IsNightTime { get; private set; }
        public World()
        {
            
        }
        public void Update()
        {
            IsNightTime = false;
            var HourOfDay = NativeFunction.CallByName<int>("GET_CLOCK_HOURS");
            if (HourOfDay >= 19 || HourOfDay <= 6)//7pm to 6 am lights need to be on
            {
                IsNightTime = true;
            }
        }
        public void PuneVehicleList()
        {

        }
        public void Dispose()
        {
            RemoveBlips();
            Time.Dispose();
            Pedestrians.Dispose();
            Dispatcher.Dispose();
            Spawner.Dispose();
            Vehicles.ClearPolice();
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
        public void AddBlip(Blip myBlip)
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
