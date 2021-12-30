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
                if (Type == LocationType.FireStation)
                {
                    return (BlipSprite)436;
                }
                else if (Type == LocationType.Police)
                {
                    return BlipSprite.PoliceStation;
                }
                else if (Type == LocationType.Stadium)
                {
                    return (BlipSprite)459;
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
                else if (Type == LocationType.Morgue)
                {
                    return BlipSprite.Dead;
                }
                else if (Type == LocationType.FoodStand)
                {
                    return (BlipSprite)480;//radar_vip
                }
                else if (Type == LocationType.Dispensary)
                {
                    return BlipSprite.Stash;
                }
                else if (Type == LocationType.Restaurant)
                {
                    return (BlipSprite)621;// 475;//.Bar;(304)(621)
                }
                else if (Type == LocationType.DriveThru)
                {
                    return (BlipSprite)523;//.Bar;
                }
                else if (Type == LocationType.LiquorStore)
                {
                    return BlipSprite.Bar;
                }
                else if (Type == LocationType.Bank)
                {
                    return BlipSprite.Devin;
                }
                else if (Type == LocationType.Bar)
                {
                    return BlipSprite.Bar;
                }
                else if (Type == LocationType.Pharmacy)
                {
                    return BlipSprite.CriminalDrugs;
                }
                else if (Type == LocationType.Hotel)
                {
                    return (BlipSprite)475;
                }
                else if (Type == LocationType.Apartment)
                {
                    return BlipSprite.GarageForSale;
                }
                else if (Type == LocationType.HardwareStore)
                {
                    return (BlipSprite)566; //BlipSprite.Repair;
                }
                else if (Type == LocationType.Headshop)
                {
                    return (BlipSprite)96;// BlipSprite.Stash;
                }
                else if (Type == LocationType.CarDealer)
                {
                    return BlipSprite.GangVehicle;
                }
                else if (Type == LocationType.PawnShop)
                {
                    return BlipSprite.PointOfInterest;
                }
                else if (Type == LocationType.BusStop)
                {
                    return BlipSprite.VinewoodTours;
                }
                else if (Type == LocationType.Brothel)
                {
                    return BlipSprite.DropOffHooker;
                }
                else if (Type == LocationType.Yoga)
                {
                    return BlipSprite.Yoga;
                }
                else if (Type == LocationType.MassageParlor)
                {
                    return (BlipSprite)466;
                }
                else if (Type == LocationType.DrugDealer)
                {
                    return BlipSprite.UGCMission;
                }
                else if (Type == LocationType.VendingMachine)
                {
                    return BlipSprite.PointOfInterest;
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


            NativeFunction.Natives.BEGIN_TEXT_COMMAND_SET_BLIP_NAME("STRING");
            NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(Name);
            NativeFunction.Natives.END_TEXT_COMMAND_SET_BLIP_NAME(MyLocationBlip);

            return MyLocationBlip;
        }


        //private void SetBlipName(string BlipString, Blip BlipName)
        //{
        //    InputArgument arguments  = new InputArgument() { "STRING"};
        //    Native.Function.Call(Hash._0xF9113A30DE5C6670, arguments); 
        //    Native.Function.Call(Hash._ADD_TEXT_COMPONENT_STRING, BlipString);        
        //    Native.Function.Call(Hash._0xBC38B49BCB83BC9B, BlipName);
        //}
    }
}
