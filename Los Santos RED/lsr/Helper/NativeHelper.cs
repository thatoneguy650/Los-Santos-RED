using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DispatchScannerFiles;

namespace LosSantosRED.lsr.Helper
{
    public static class NativeHelper
    {
        public static void AddLongString(string str)//https://github.com/alexguirre/RAGENativeUI/blob/fa32a06b84b3ff33f4988b7ba6fb4d3bb158b134/Source/Elements/ResText.cs
        {
            const int strLen = 99;
            for (int i = 0; i < str.Length; i += strLen)
            {
                string substr = str.Substring(i, Math.Min(strLen, str.Length - i));
                NativeFunction.CallByHash<uint>(0x6c188be134e074aa, substr);      // _ADD_TEXT_COMPONENT_STRING
            }

        }
        internal static uint CashHash(object p)
        {
            throw new NotImplementedException();
        }
        public static void StartScript(string scriptName, int buffer)
        {
            NativeFunction.Natives.REQUEST_SCRIPT(scriptName);

            while (!NativeFunction.Natives.HAS_SCRIPT_LOADED<bool>(scriptName))
            {
                NativeFunction.Natives.REQUEST_SCRIPT(scriptName);
                GameFiber.Yield();
            }

            NativeFunction.Natives.START_NEW_SCRIPT(scriptName, buffer);
            NativeFunction.Natives.SET_SCRIPT_AS_NO_LONGER_NEEDED(scriptName);
        }



        public static bool IsStringHash(string value, out uint hash)
        {
            var hex = value.ToLower();
            if (hex.StartsWith("0x", StringComparison.CurrentCultureIgnoreCase) || hex.StartsWith("&H", StringComparison.CurrentCultureIgnoreCase))
            {
                hex = hex.Substring(2);
            }
            return uint.TryParse(hex, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out hash);
        }

        public static string FormatControls(Keys modifier, Keys key)
        {
            string KeyString = $"~o~{KeyHandyName(key)}~s~";
            string ModifierString = $"~o~{KeyHandyName(modifier)}~s~";
            if (modifier != Keys.None && key != Keys.None)
            {
                return $"{KeyString} + {ModifierString}";
            }
            else if (modifier != Keys.None && key == Keys.None)
            {
                return $"{ModifierString}";
            }
            else if (modifier == Keys.None && key != Keys.None)
            {
                return $"{KeyString}";
            }
            return "";
        }
        private static string KeyHandyName(Keys key)
        {
            if(key == Keys.XButton1)
            {
                return "Mouse-4";
            }
            else if (key == Keys.XButton2)
            {
                return "Mouse-5";
            }
            return key.ToString();
        }
        public static uint CashHash(string PlayerName)
        {
            switch (PlayerName.ToLower())
            {
                case "michael":
                    return Game.GetHashKey("SP0_TOTAL_CASH");
                case "player_zero":
                    return Game.GetHashKey("SP0_TOTAL_CASH");
                case "franklin":
                    return Game.GetHashKey("SP1_TOTAL_CASH");
                case "player_one":
                    return Game.GetHashKey("SP1_TOTAL_CASH");
                case "trevor":
                    return Game.GetHashKey("SP2_TOTAL_CASH");
                case "player_two":
                    return Game.GetHashKey("SP2_TOTAL_CASH");
                default:
                    return Game.GetHashKey("SP0_TOTAL_CASH");
            }
        }
        public static string CellToStreetNumber(int CellX, int CellY)
        {
            string StreetNumber;
            if (CellY < 0)
            {
                StreetNumber = Math.Abs(CellY * 10).ToString() + "S";
            }
            else
            {
                if(CellY == 0)
                {
                    StreetNumber = "1N";
                }
                else
                {
                    StreetNumber = Math.Abs(CellY * 10).ToString() + "N";
                }
                
            }
            if (CellX < 0)
            {
                StreetNumber += Math.Abs(CellX * 10).ToString() + "W";
            }
            else
            {
                if (CellX == 0)
                {
                    StreetNumber = "1E";
                }
                else
                {
                    StreetNumber += Math.Abs(CellX * 10).ToString() + "E";
                }
            }
            return StreetNumber;
        }
        public static Vector3 GetGameplayCameraDirection()
        {
            //Scripthook dot net adaptation stuff i dont understand. I forgot most of my math.....
            Vector3 CameraRotation = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
            double rotX = CameraRotation.X / 57.295779513082320876798154814105;
            double rotZ = CameraRotation.Z / 57.295779513082320876798154814105;
            double multXY = Math.Abs(Math.Cos(rotX));
            return new Vector3((float)(-Math.Sin(rotZ) * multXY), (float)(Math.Cos(rotZ) * multXY), (float)Math.Sin(rotX));
        }
        public static Vector3 GetCameraDirection(Camera camera)
        {
            //Scripthook dot net adaptation stuff i dont understand. I forgot most of my math.....
            Vector3 CameraRotation = new Vector3(camera.Rotation.Pitch,camera.Rotation.Roll, camera.Rotation.Yaw);
            double rotX = CameraRotation.X / 57.295779513082320876798154814105;
            double rotZ = CameraRotation.Z / 57.295779513082320876798154814105;
            double multXY = Math.Abs(Math.Cos(rotX));
            return new Vector3((float)(-Math.Sin(rotZ) * multXY), (float)(Math.Cos(rotZ) * multXY), (float)Math.Sin(rotX));
        }
        public static Vector3 GetCameraDirection(Camera camera, float scale)
        {
            //Scripthook dot net adaptation stuff i dont understand. I forgot most of my math.....
            Vector3 CameraRotation = new Vector3(camera.Rotation.Pitch, camera.Rotation.Roll, camera.Rotation.Yaw);
            double rotX = CameraRotation.X / 57.295779513082320876798154814105;
            double rotZ = CameraRotation.Z / 57.295779513082320876798154814105;
            double multXY = Math.Abs(Math.Cos(rotX));
            return new Vector3((float)(-Math.Sin(rotZ) * multXY) * scale, (float)(Math.Cos(rotZ) * multXY) * scale, (float)Math.Sin(rotX) * scale);
        }
        public static Vector3 GetCameraDirectionOffset(Camera camera, float offset)
        {
            //Scripthook dot net adaptation stuff i dont understand. I forgot most of my math.....
            Vector3 CameraRotation = new Vector3(camera.Rotation.Pitch, camera.Rotation.Roll, camera.Rotation.Yaw);
            double rotX = CameraRotation.X / 57.295779513082320876798154814105;
            double rotZ = CameraRotation.Z / 57.295779513082320876798154814105;
            double multXY = Math.Abs(Math.Cos(rotX));
            return new Vector3((float)(-Math.Sin(rotZ) * multXY), (float)(Math.Cos(rotZ) * multXY), (float)Math.Sin(rotX));
        }
        public static uint GetTargettingHandle()
        {
            uint TargetEntity;
            bool Found;
            Found = NativeFunction.Natives.GET_PLAYER_TARGET_ENTITY<bool>(Game.LocalPlayer, out TargetEntity);
            if (!Found)
            {
                return 0;
            }
            uint Handle = TargetEntity;
            return Handle;
        }
        public static string FirstCharToUpper(string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }
        public static void DisplayNotificationCustom(string textureDictionaryName, string textureName, string title, string subtitle, string text, NotificationIconTypes iconID, bool flash)
        {
           // NativeFunction.Natives.BEGIN_TEXT_COMMAND_THEFEED_POST("STRING");
            NativeFunction.Natives.BEGIN_TEXT_COMMAND_THEFEED_POST("jamyfafi");


            //NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(text);
            //NativeFunction.Natives.x25fbb336df1804cb("jamyfafi"); //NativeFunction.Natives.x25fbb336df1804cb("STRING");
            AddLongString(text);
            NativeFunction.Natives.END_TEXT_COMMAND_THEFEED_POST_MESSAGETEXT(textureDictionaryName, textureName, flash, (int)iconID, title, subtitle);
        }
        public static void DisplayNotificationCustom(string textureDictionaryName, string textureName, string title, string subtitle, NotificationIconTypes iconID, bool flash)
        {
            NativeFunction.Natives.BEGIN_TEXT_COMMAND_THEFEED_POST("STRING");
            NativeFunction.Natives.END_TEXT_COMMAND_THEFEED_POST_MESSAGETEXT(textureDictionaryName, textureName, flash, (int)iconID, title, subtitle);
        }
        public static string GetSimpleCompassHeading(float Heading)
        {
            //float Heading = Game.LocalPlayer.Character.Heading;
            string Abbreviation;

            //yeah could be simpler, whatever idk computers are fast
            if (Heading >= 354.375f || Heading <= 5.625f) { Abbreviation = "N"; }
            else if (Heading >= 5.625f && Heading <= 16.875f) { Abbreviation = "N"; }
            else if (Heading >= 16.875f && Heading <= 28.125f) { Abbreviation = "N"; }
            else if (Heading >= 28.125f && Heading <= 39.375f) { Abbreviation = "N"; }
            else if (Heading >= 39.375f && Heading <= 50.625f) { Abbreviation = "N"; }
            else if (Heading >= 50.625f && Heading <= 61.875f) { Abbreviation = "N"; }
            else if (Heading >= 61.875f && Heading <= 73.125f) { Abbreviation = "E"; }
            else if (Heading >= 73.125f && Heading <= 84.375f) { Abbreviation = "E"; }
            else if (Heading >= 84.375f && Heading <= 95.625f) { Abbreviation = "E"; }
            else if (Heading >= 95.625f && Heading <= 106.875f) { Abbreviation = "E"; }
            else if (Heading >= 106.875f && Heading <= 118.125f) { Abbreviation = "E"; }
            else if (Heading >= 118.125f && Heading <= 129.375f) { Abbreviation = "S"; }
            else if (Heading >= 129.375f && Heading <= 140.625f) { Abbreviation = "S"; }
            else if (Heading >= 140.625f && Heading <= 151.875f) { Abbreviation = "S"; }
            else if (Heading >= 151.875f && Heading <= 163.125f) { Abbreviation = "S"; }
            else if (Heading >= 163.125f && Heading <= 174.375f) { Abbreviation = "S"; }
            else if (Heading >= 174.375f && Heading <= 185.625f) { Abbreviation = "S"; }
            else if (Heading >= 185.625f && Heading <= 196.875f) { Abbreviation = "S"; }
            else if (Heading >= 196.875f && Heading <= 208.125f) { Abbreviation = "S"; }
            else if (Heading >= 208.125f && Heading <= 219.375f) { Abbreviation = "S"; }
            else if (Heading >= 219.375f && Heading <= 230.625f) { Abbreviation = "S"; }
            else if (Heading >= 230.625f && Heading <= 241.875f) { Abbreviation = "S"; }
            else if (Heading >= 241.875f && Heading <= 253.125f) { Abbreviation = "W"; }
            else if (Heading >= 253.125f && Heading <= 264.375f) { Abbreviation = "W"; }
            else if (Heading >= 264.375f && Heading <= 275.625f) { Abbreviation = "W"; }
            else if (Heading >= 275.625f && Heading <= 286.875f) { Abbreviation = "W"; }
            else if (Heading >= 286.875f && Heading <= 298.125f) { Abbreviation = "W"; }
            else if (Heading >= 298.125f && Heading <= 309.375f) { Abbreviation = "N"; }
            else if (Heading >= 309.375f && Heading <= 320.625f) { Abbreviation = "N"; }
            else if (Heading >= 320.625f && Heading <= 331.875f) { Abbreviation = "N"; }
            else if (Heading >= 331.875f && Heading <= 343.125f) { Abbreviation = "N"; }
            else if (Heading >= 343.125f && Heading <= 354.375f) { Abbreviation = "N"; }
            else if (Heading >= 354.375f || Heading <= 5.625f) { Abbreviation = "N"; }
            else { Abbreviation = ""; }

            return Abbreviation;
        }
        public static void DisablePlayerControl()
        {
            Game.DisableControlAction(0, GameControl.LookLeftRight, true);
            Game.DisableControlAction(0, GameControl.LookUpDown, true);
            Game.DisableControlAction(0, GameControl.MoveUpDown, true);
            Game.DisableControlAction(0, GameControl.MoveLeftRight, true);
            Game.DisableControlAction(0, GameControl.VehicleAccelerate, true);
            Game.DisableControlAction(0, GameControl.VehicleBrake, true);
            Game.DisableControlAction(0, GameControl.Jump, true);
        }
        public static bool IsUsingKeyboard => NativeFunction.Natives.IS_USING_KEYBOARD_AND_MOUSE<bool>(2);
        public static bool IsUsingController => !NativeFunction.Natives.IS_USING_KEYBOARD_AND_MOUSE<bool>(2);

        //public static string VehicleMakeName(uint ModelHash)
        //{

        //    string MakeName;
        //    unsafe
        //    {
        //        IntPtr ptr = NativeFunction.CallByHash<IntPtr>(0xF7AF4F159FF99F97, ModelHash);
        //        MakeName = Marshal.PtrToStringAnsi(ptr);
        //    }
        //    unsafe
        //    {
        //        IntPtr ptr2 = NativeFunction.CallByHash<IntPtr>(0x7B5280EBA9840C72, MakeName);
        //        MakeName = Marshal.PtrToStringAnsi(ptr2);
        //    }
        //    if (MakeName == "CARNOTFOUND" || MakeName == "NULL")
        //        return "";
        //    else
        //        return MakeName;


        //}
        //public static string VehicleModelName(uint ModelHash)
        //{

        //        string ModelName;
        //        unsafe
        //        {
        //            IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_DISPLAY_NAME_FROM_VEHICLE_MODEL", ModelHash);
        //            ModelName = Marshal.PtrToStringAnsi(ptr);
        //        }
        //        unsafe
        //        {
        //            IntPtr ptr2 = NativeFunction.CallByHash<IntPtr>(0x7B5280EBA9840C72, ModelName);
        //            ModelName = Marshal.PtrToStringAnsi(ptr2);
        //        }
        //        if (ModelName == "CARNOTFOUND" || ModelName == "NULL")
        //            return "";
        //        else
        //            return ModelName;
        //}


        public static bool IsNearby(int cellX, int cellY, int targetCellX, int targetCellY, int distance) => cellX >= targetCellX - distance && cellX <= targetCellX + distance && cellY >= targetCellY - distance && cellY <= targetCellY + distance;
        public static int MaxCellsAway(int cellX, int cellY, int targetCellX, int targetCellY) => Math.Max(Math.Abs(cellX - targetCellX),Math.Abs(cellY - targetCellY));
        public static void GetStreetPositionandHeading(Vector3 PositionNear, out Vector3 SpawnPosition, out float Heading, bool MainRoadsOnly)
        {
            Vector3 pos = PositionNear;
            SpawnPosition = Vector3.Zero;
            Heading = 0f;
            Vector3 outPos;
            float heading;
            float val;
            if (MainRoadsOnly)
            {
                NativeFunction.Natives.GET_CLOSEST_VEHICLE_NODE_WITH_HEADING<bool>(pos.X, pos.Y, pos.Z, out outPos, out heading, 0, 3, 0);
                SpawnPosition = outPos;
                Heading = heading;
            }
            else
            {
                for (int i = 1; i < 40; i++)
                {
                    NativeFunction.Natives.GET_NTH_CLOSEST_VEHICLE_NODE_WITH_HEADING<bool>(pos.X, pos.Y, pos.Z, i, out outPos, out heading, out val, 1, 0x40400000, 0);
                    if (!NativeFunction.Natives.IS_POINT_OBSCURED_BY_A_MISSION_ENTITY<bool>(outPos.X, outPos.Y, outPos.Z, 5.0f, 5.0f, 5.0f, 0))
                    {
                        SpawnPosition = outPos;
                        Heading = heading;
                        break;
                    }
                }
            }
        }
        public static Vector3 GetStreetPosition(Vector3 PositionNear)
        {
            Vector3 pos = PositionNear;
            Vector3 SpawnPosition = Vector3.Zero;
            Vector3 outPos;
            float heading;
            float val;
            for (int i = 1; i < 40; i++)
            {
                NativeFunction.Natives.GET_NTH_CLOSEST_VEHICLE_NODE_WITH_HEADING<bool>(pos.X, pos.Y, pos.Z, i, out outPos, out heading, out val, 1, 0x40400000, 0);
                if (!NativeFunction.Natives.IS_POINT_OBSCURED_BY_A_MISSION_ENTITY<bool>(outPos.X, outPos.Y, outPos.Z, 5.0f, 5.0f, 5.0f, 0))
                {
                    SpawnPosition = outPos;
                    break;
                }
            }
            if (SpawnPosition == Vector3.Zero)
            {
                return PositionNear;
            }
            return SpawnPosition;
        }
        public static PedVariation GetPedVariation(Ped myPed)
        {
            try
            {
                PedVariation myPedVariation = new PedVariation
                {
                    Components = new List<PedComponent>(),
                    Props = new List<PedPropComponent>()
                };
                for (int ComponentNumber = 0; ComponentNumber < 12; ComponentNumber++)
                {
                    myPedVariation.Components.Add(new PedComponent(ComponentNumber, NativeFunction.Natives.GET_PED_DRAWABLE_VARIATION<int>(myPed, ComponentNumber), NativeFunction.Natives.GET_PED_TEXTURE_VARIATION<int>(myPed, ComponentNumber), NativeFunction.Natives.GET_PED_PALETTE_VARIATION<int>(myPed, ComponentNumber)));
                }
                for (int PropNumber = 0; PropNumber < 8; PropNumber++)
                {
                    myPedVariation.Props.Add(new PedPropComponent(PropNumber, NativeFunction.Natives.GET_PED_PROP_INDEX<int>(myPed, PropNumber), NativeFunction.Natives.GET_PED_PROP_TEXTURE_INDEX<int>(myPed, PropNumber)));
                }

                try
                {
                    NativeFunction.Natives.GET_PED_HEAD_BLEND_DATA(myPed, out HeadBlendDataStruct structout);
                    if (structout.shapeMix != 0.0f || structout.skinMix != 0.0f || structout.thirdMix != 0.0f || structout.shapeFirstID != 0 || structout.shapeSecondID != 0 || structout.shapeThirdID != 0)//has some mix?
                    {
                        myPedVariation.HeadBlendData = new HeadBlendData(structout.shapeFirstID, structout.shapeSecondID, structout.shapeThirdID, structout.skinFirstID, structout.skinSecondID, structout.skinThirdID, structout.shapeMix, structout.skinMix, structout.thirdMix);
                    }

                List<HeadOverlayData> HeadOverlays = new List<HeadOverlayData>() {
                    new HeadOverlayData(0, "Blemishes") { Index = 255, Opacity = 1.0f, PrimaryColor = 0, SecondaryColor = 0 },
                    new HeadOverlayData(1, "Facial Hair") { ColorType = 1, Index = 255, Opacity = 1.0f, PrimaryColor = 0, SecondaryColor = 0 },
                    new HeadOverlayData(2, "Eyebrows") { ColorType = 1, Index = 3, Opacity = 1.0f, PrimaryColor = 0, SecondaryColor = 0 },
                    new HeadOverlayData(3, "Ageing") { Index = 255, Opacity = 1.0f, PrimaryColor = 0, SecondaryColor = 0 },
                    new HeadOverlayData(4, "Makeup") { Index = 12, Opacity = 1.0f, PrimaryColor = 0, SecondaryColor = 0 },
                    new HeadOverlayData(5, "Blush") { ColorType = 2, Index = 3, Opacity = 0.4f, PrimaryColor = 0, SecondaryColor = 0 },
                    new HeadOverlayData(6, "Complexion") { Index = 255, Opacity = 1.0f, PrimaryColor = 0, SecondaryColor = 0 },
                    new HeadOverlayData(7, "Sun Damage") { Index = 255, Opacity = 1.0f, PrimaryColor = 0, SecondaryColor = 0 },
                    new HeadOverlayData(8, "Lipstick") { ColorType = 2, Index = 2, Opacity = 0.6f, PrimaryColor = 0, SecondaryColor = 0 },
                    new HeadOverlayData(9, "Moles/Freckles") { Index = 255, Opacity = 1.0f, PrimaryColor = 0, SecondaryColor = 0 },
                    new HeadOverlayData(10, "Chest Hair") { ColorType = 1, Index = 255, Opacity = 1.0f, PrimaryColor = 0, SecondaryColor = 0 },
                    new HeadOverlayData(11, "Body Blemishes") { Index = 255, Opacity = 1.0f, PrimaryColor = 0, SecondaryColor = 0 },
                    new HeadOverlayData(12, "Add Body Blemishes") { Index = 255, Opacity = 1.0f, PrimaryColor = 0, SecondaryColor = 0 },

                };
                    myPedVariation.HeadOverlays = new List<HeadOverlayData>();
                foreach(HeadOverlayData headOverlayData in HeadOverlays)
                {
                    int index = NativeFunction.Natives.xA60EF3B6461A4D43<int>(myPed, headOverlayData.OverlayID);

                        myPedVariation.HeadOverlays.Add(new HeadOverlayData(headOverlayData.OverlayID, headOverlayData.Part) { Index = index });
                }
                }
                catch (Exception e)//started throwing an error after a gta update
                {
                    //EntryPoint.WriteToConsole($"GetPedVariation GET_PED_HEAD_BLEND_DATA Error: {e.Message} {e.StackTrace}", 5);
                }
                
                return myPedVariation;
            }
            catch (Exception e)
            {
                EntryPoint.WriteToConsole("GetPedVariation General Error: " + e.Message, 0);
                return null;
            }
        }

        public static VehicleVariation GetVehicleVariation(Vehicle vehicle)
        {

            try
            {

                VehicleVariation vehicleVariation = new VehicleVariation();

                int primaryColor;
                int secondaryColor;
                unsafe
                {
                    NativeFunction.CallByName<int>("GET_VEHICLE_COLOURS", vehicle, &primaryColor, &secondaryColor);
                }
                vehicleVariation.PrimaryColor = primaryColor;
                vehicleVariation.SecondaryColor = secondaryColor;




                vehicleVariation.IsPrimaryColorCustom = NativeFunction.Natives.GET_IS_VEHICLE_PRIMARY_COLOUR_CUSTOM<bool>(vehicle);
                if(vehicleVariation.IsPrimaryColorCustom)
                {
                    int r1;
                    int g1;
                    int b1;
                    unsafe
                    {
                        NativeFunction.CallByName<int>("GET_VEHICLE_CUSTOM_PRIMARY_COLOUR", vehicle, &r1, &g1, &b1);
                    }
                    vehicleVariation.CustomPrimaryColor = Color.FromArgb(r1, g1, b1);
                }


                vehicleVariation.IsSecondaryColorCustom = NativeFunction.Natives.GET_IS_VEHICLE_SECONDARY_COLOUR_CUSTOM<bool>(vehicle);
                if (vehicleVariation.IsSecondaryColorCustom)
                {
                    int r2;
                    int g2;
                    int b2;
                    unsafe
                    {
                        NativeFunction.CallByName<int>("GET_VEHICLE_CUSTOM_SECONDARY_COLOUR", vehicle, &r2, &g2, &b2);
                    }
                    vehicleVariation.CustomSecondaryColor = Color.FromArgb(r2, g2, b2);
                }




                int pearlescentColor;
                int wheelColor;
                unsafe
                {
                    NativeFunction.CallByName<int>("GET_VEHICLE_EXTRA_COLOURS", vehicle, &pearlescentColor, &wheelColor);
                }
                vehicleVariation.PearlescentColor = pearlescentColor;
                vehicleVariation.WheelColor = wheelColor;

                int mod1paintType;
                int mod1color;
                int mod1PearlescentColor;
                unsafe
                {
                    NativeFunction.CallByName<int>("GET_VEHICLE_MOD_COLOR_1", vehicle, &mod1paintType, &mod1color, &mod1PearlescentColor);
                }
                vehicleVariation.Mod1PaintType = mod1paintType;
                vehicleVariation.Mod1Color = mod1color;
                vehicleVariation.Mod1PearlescentColor = mod1PearlescentColor;

                int mod2paintType;
                int mod2color;
                unsafe
                {
                    NativeFunction.CallByName<int>("GET_VEHICLE_MOD_COLOR_2", vehicle, &mod2paintType, &mod2color);
                }
                vehicleVariation.Mod2PaintType = mod2paintType;
                vehicleVariation.Mod2Color = mod2color;
                vehicleVariation.Livery = NativeFunction.Natives.GET_VEHICLE_LIVERY<int>(vehicle);
                vehicleVariation.LicensePlate = new LSR.Vehicles.LicensePlate();
                vehicleVariation.LicensePlate.PlateNumber = NativeFunction.Natives.GET_VEHICLE_NUMBER_PLATE_TEXT<string>(vehicle);
                vehicleVariation.LicensePlate.PlateType = NativeFunction.Natives.GET_VEHICLE_NUMBER_PLATE_TEXT_INDEX<int>(vehicle);
                vehicleVariation.WheelType = NativeFunction.Natives.GET_VEHICLE_WHEEL_TYPE<int>(vehicle);




                vehicleVariation.WindowTint = NativeFunction.Natives.GET_VEHICLE_WINDOW_TINT<int>(vehicle);


                int customWheelID = 23;
                if(vehicle.IsBike)
                {
                    customWheelID = 24;
                }
                vehicleVariation.HasCustomWheels = NativeFunction.Natives.GET_VEHICLE_MOD_VARIATION<bool>(vehicle, customWheelID);





                vehicleVariation.VehicleExtras = new List<VehicleExtra>();
                for (int i = 0; i <= 15; i++)
                {
                    if (!NativeFunction.Natives.DOES_EXTRA_EXIST<bool>(vehicle, i))
                    {
                        vehicleVariation.VehicleExtras.Add(new VehicleExtra(i, false));
                    }
                    else
                    {
                        vehicleVariation.VehicleExtras.Add(new VehicleExtra(i, NativeFunction.Natives.IS_VEHICLE_EXTRA_TURNED_ON<bool>(vehicle,i)));
                    }
                }
                vehicleVariation.VehicleToggles = new List<VehicleToggle>();
                vehicleVariation.VehicleMods = new List<VehicleMod>();
                for (int i = 0; i <= 50; i++)
                {
                    if (i >= 17 && i <= 22)
                    {
                        bool isToggledOn = NativeFunction.Natives.IS_TOGGLE_MOD_ON<bool>(vehicle, i);
                        vehicleVariation.VehicleToggles.Add(new VehicleToggle(i, isToggledOn));
                    }
                    else
                    {
                        int vehicleMod = NativeFunction.Natives.GET_VEHICLE_MOD<int>(vehicle, i);
                        //if(vehicleMod != -1)
                        //{
                            vehicleVariation.VehicleMods.Add(new VehicleMod(i, vehicleMod));
                        //}
                    }
                }


                vehicleVariation.DirtLevel = NativeFunction.Natives.GET_VEHICLE_DIRT_LEVEL<int>(vehicle);


                return vehicleVariation;
            }
            catch (Exception e)
            {
                EntryPoint.WriteToConsole("Copy Vehicle Variation Error: " + e.Message + " " + e.StackTrace, 0);
                return null;
            }
        }

        public static bool IsSittableModel(string modelName)
        {
            if(modelName.Contains("chair") || modelName.Contains("sofa") || modelName.Contains("couch") || modelName.Contains("bench") || modelName.Contains("seat") || modelName.Contains("chr"))
            {
                return true;
            }
            return false;
        }
        public static void ChangeModel(string ModelRequested)
        {
            Model characterModel = new Model(ModelRequested);
            characterModel.LoadAndWait();
            characterModel.LoadCollisionAndWait();
            Game.LocalPlayer.Model = characterModel;
            Game.LocalPlayer.Character.IsCollisionEnabled = true;
        }
        public static void SetAsMainPlayer()
        {
            // from bigbruh in discord, supplied the below, seems to work just fine
            unsafe
            {
                var PedPtr = (ulong)Game.LocalPlayer.Character.MemoryAddress;
                ulong SkinPtr = *((ulong*)(PedPtr + 0x20));
                *((ulong*)(SkinPtr + 0x18)) = (ulong)225514697;//set as player_zero
            }      
        }
        public static string GetKeyboardInput(string DefaultText)
        {
            NativeFunction.Natives.DISPLAY_ONSCREEN_KEYBOARD<bool>(true, "FMMC_KEY_TIP8", "", DefaultText, "", "", "", 255 + 1);
            while (NativeFunction.Natives.UPDATE_ONSCREEN_KEYBOARD<int>() == 0)
            {
                GameFiber.Sleep(500);
            }
            string Value;
            IntPtr ptr = NativeFunction.Natives.GET_ONSCREEN_KEYBOARD_RESULT<IntPtr>();
            Value = Marshal.PtrToStringAnsi(ptr);
            return Value;
        }
        public static Vector3 GetOffsetPosition(Vector3 Position, float heading, float Offset)
        {
            return Position + (new Vector3((float)Math.Cos(heading * Math.PI / 180), (float)Math.Sin(heading * Math.PI / 180), 0) * Offset);//Positon + Direction UnitVector From Heading, Times the Length
        }
        public static void RequestIPL(string iplName)
        {
            if (!NativeFunction.Natives.IS_IPL_ACTIVE<bool>(iplName))
            {
                NativeFunction.Natives.REQUEST_IPL(iplName);
            }
        }
        public static void RemoveIPL(string iplName)
        {
            if (NativeFunction.Natives.IS_IPL_ACTIVE<bool>(iplName))
            {
                NativeFunction.Natives.REMOVE_IPL(iplName);
            }
        }       
        public static string VehicleMakeName(uint modelHash)
        {
            string MakeName;
            unsafe
            {
                IntPtr ptr = NativeFunction.CallByHash<IntPtr>(0xF7AF4F159FF99F97, modelHash);
                MakeName = Marshal.PtrToStringAnsi(ptr);
            }
            unsafe
            {
                IntPtr ptr2 = NativeFunction.CallByHash<IntPtr>(0x7B5280EBA9840C72, MakeName);
                MakeName = Marshal.PtrToStringAnsi(ptr2);
            }
            if (MakeName == "CARNOTFOUND" || MakeName == "NULL")
            {
                return "";
            }
            else
            {
                return MakeName;
            }
        }
        public static string VehicleModelName(uint modelHash)
        {
            string ModelName;
            unsafe
            {
                IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_DISPLAY_NAME_FROM_VEHICLE_MODEL", modelHash);
                ModelName = Marshal.PtrToStringAnsi(ptr);
            }
            unsafe
            {
                IntPtr ptr2 = NativeFunction.CallByHash<IntPtr>(0x7B5280EBA9840C72, ModelName);
                ModelName = Marshal.PtrToStringAnsi(ptr2);
            }
            if (ModelName == "CARNOTFOUND" || ModelName == "NULL")
            {
                return "";
            }
            else
            {
                return ModelName;
            }

        }
        public static string VehicleClassName(uint modelHash)
        {
            int ClassInt = NativeFunction.CallByName<int>("GET_VEHICLE_CLASS_FROM_NAME", modelHash);
            switch (ClassInt)
            {
                case 0:
                    return "Compact";
                case 1:
                    return "Sedan";
                case 2:
                    return "SUV";
                case 3:
                    return "Coupe";
                case 4:
                    return "Muscle";
                case 5:
                    return "Sports Classic";
                case 6:
                    return "Sports Car";
                case 7:
                    return "Super";
                case 8:
                    return "Motorcycle";
                case 9:
                    return "Off Road";
                case 10:
                    return "Industrial";
                case 11:
                    return "Utility";
                case 12:
                    return "Van";
                case 13:
                    return "Bicycle";
                case 14:
                    return "Boat";
                case 15:
                    return "Helicopter";
                case 16:
                    return "Plane";
                case 17:
                    return "Service";
                case 18:
                    return "Emergency";
                case 19:
                    return "Military";
                case 20:
                    return "Commercial";
                case 21:
                    return "Train";
                default:
                    return "";
            }
        }
    }
    [StructLayout(LayoutKind.Explicit, Size = 80)]
    public struct HeadBlendDataStruct
    {
        [FieldOffset(0)]
        public int shapeFirstID;
        [FieldOffset(8)]
        public int shapeSecondID;
        [FieldOffset(16)]
        public int shapeThirdID;
        [FieldOffset(24)]
        public int skinFirstID;
        [FieldOffset(32)]
        public int skinSecondID;
        [FieldOffset(40)]
        public int skinThirdID;
        [FieldOffset(48)]
        public float shapeMix;
        [FieldOffset(56)]
        public float skinMix;
        [FieldOffset(64)]
        public float thirdMix;
        [FieldOffset(75)]
        public bool isParent;
    }
}
