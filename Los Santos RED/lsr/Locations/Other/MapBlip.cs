//using Rage;
//using Rage.Native;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace LosSantosRED.lsr.Util.Locations
//{
//    public class MapBlip
//    {
//        private bool HasSprite = true;
//        public MapBlip(Vector3 locationPosition, string name, BlipSprite blipSprite)
//        {
//            LocationPosition = locationPosition;
//            Name = name;
//            BlipSprite = blipSprite;
//        }
//        public MapBlip(Vector3 locationPosition, string name)
//        {
//            LocationPosition = locationPosition;
//            Name = name;
//            HasSprite = false;
//        }
//        public Vector3 LocationPosition { get; }
//        public string Name { get; }
//        public BlipSprite BlipSprite { get; set; }
//        public Blip AddToMap()
//        {
//            Blip MyLocationBlip = new Blip(LocationPosition)
//            {
//                Name = Name
//            };

//            if(HasSprite)
//            {
//                MyLocationBlip.Sprite = BlipSprite;
//            }
//            else
//            {
//                MyLocationBlip.Scale = 0.5f;
//            }


//            if(BlipSprite == BlipSprite.PointOfInterest)
//            {
//                MyLocationBlip.Scale = 0.5f;
//            }

//            //MyLocationBlip.Scale = 0.5f;
//            MyLocationBlip.Color = Color.White;
//            NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)MyLocationBlip.Handle, true);
//            NativeFunction.Natives.BEGIN_TEXT_COMMAND_SET_BLIP_NAME("STRING");
//            NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(Name);
//            NativeFunction.Natives.END_TEXT_COMMAND_SET_BLIP_NAME(MyLocationBlip);

//            return MyLocationBlip;
//        }
//    }
//}
