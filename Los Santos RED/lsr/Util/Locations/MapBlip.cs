using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Util.Locations
{
    public class MapBlip
    {
        public MapBlip(Vector3 LocationPosition, string Name, LocationType Type)
        {
            this.LocationPosition = LocationPosition;
            this.Name = Name;
            this.Type = Type;
        }
        public Vector3 LocationPosition { get; }
        public string Name { get; }
        public LocationType Type { get; }
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
                else if (Type == LocationType.Grave)
                {
                    return BlipSprite.Dead;
                }
                else if (Type == LocationType.FoodStand)
                {
                    return BlipSprite.Bar;
                }
                else
                {
                    return BlipSprite.PointOfInterest;
                }
            }
        }
        public Blip AddToMap()
        {
            Blip MyLocationBlip = new Blip(LocationPosition)
            {
                Name = Name
            };
            MyLocationBlip.Sprite = Icon;
            MyLocationBlip.Color = Color.White;
            NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)MyLocationBlip.Handle, true);
            return MyLocationBlip;
        }
    }
}
