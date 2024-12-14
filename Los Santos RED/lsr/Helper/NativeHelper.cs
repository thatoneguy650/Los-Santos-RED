using ExtensionsMethods;
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

        public static bool IsPointInPolygon(Vector2 point, Vector2[] polygon)
        {
            int polygonLength = polygon.Length, i = 0;
            bool inside = false;
            // x, y for tested point.
            float pointX = point.X, pointY = point.Y;
            // start / end point for the current polygon segment.
            float startX, startY, endX, endY;
            Vector2 endPoint = polygon[polygonLength - 1];
            endX = endPoint.X;
            endY = endPoint.Y;
            while (i < polygonLength)
            {
                startX = endX; startY = endY;
                endPoint = polygon[i++];
                endX = endPoint.X; endY = endPoint.Y;
                //
                inside ^= (endY > pointY ^ startY > pointY) /* ? pointY inside [startY;endY] segment ? */
                          && /* if so, test if it is under the segment */
                          ((pointX - endX) < (pointY - endY) * (startX - endX) / (startY - endY));
            }
            return inside;
        }
        public static void PlayErrorSound()
        {
            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "ERROR", "HUD_LIQUOR_STORE_SOUNDSET", 0);
        }
        public static void PlaySuccessSound()
        {
            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "PURCHASE", "HUD_LIQUOR_STORE_SOUNDSET", 0);
            //NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "WEAPON_PURCHASE", "HUD_AMMO_SHOP_SOUNDSET", 0);
        }
        public static void PlayAcceptSound()
        {
            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "Menu_Accept", "Phone_SoundSet_Michael", 0);
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



        public static void DisplayTextOnScreen(string TextToShow, float X, float Y, float Scale, Color TextColor, GTAFont Font, GTATextJustification Justification, bool outline)
        {
            DisplayTextOnScreen(TextToShow, X, Y, Scale, TextColor, Font, Justification, outline, 255);
        }
        public static void DisplayTextOnScreen(string TextToShow, float X, float Y, float Scale, Color TextColor, GTAFont Font, GTATextJustification Justification, bool outline, int alpha)
        {
            try
            {
                if (TextToShow == "" || alpha == 0 || TextToShow is null)
                {
                    return;
                }
                NativeFunction.Natives.SET_TEXT_FONT((int)Font);
                NativeFunction.Natives.SET_TEXT_SCALE(Scale, Scale);
                NativeFunction.Natives.SET_TEXT_COLOUR((int)TextColor.R, (int)TextColor.G, (int)TextColor.B, alpha);

                NativeFunction.Natives.SetTextJustification((int)Justification);

                NativeFunction.Natives.SET_TEXT_DROP_SHADOW();

                if (outline)
                {
                    NativeFunction.Natives.SET_TEXT_OUTLINE();


                    //NativeFunction.Natives.SET_TEXT_EDGE(1, 0, 0, 0, 255);
                }
                NativeFunction.Natives.SET_TEXT_DROP_SHADOW();
                //NativeFunction.Natives.SetTextDropshadow(20, 255, 255, 255, 255);//NativeFunction.Natives.SetTextDropshadow(2, 2, 0, 0, 0);
                //NativeFunction.Natives.SetTextJustification((int)GTATextJustification.Center);
                if (Justification == GTATextJustification.Right)
                {
                    NativeFunction.Natives.SET_TEXT_WRAP(0f, Y);
                }
                else
                {
                    NativeFunction.Natives.SET_TEXT_WRAP(0f, 1f);
                }
                NativeFunction.Natives.x25fbb336df1804cb("STRING"); //NativeFunction.Natives.x25fbb336df1804cb("STRING");
                                                                    //NativeFunction.Natives.x25FBB336DF1804CB(TextToShow);
                NativeFunction.Natives.x6C188BE134E074AA(TextToShow);
                NativeFunction.Natives.xCD015E5BB0D96A57(Y, X);
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole($"UI ERROR {ex.Message} {ex.StackTrace}", 0);
            }
            //return;
        }

        //private void DisplayTextOnScreen(string TextToShow, float X, float Y, float Scale, Color TextColor, GTAFont Font, GTATextJustification Justification, bool outline)
        //{
        //    DisplayTextOnScreen(TextToShow, X, Y, Scale, TextColor, Font, Justification, outline, 255);
        //}
        //private void DisplayTextOnScreen(string TextToShow, float X, float Y, float Scale, Color TextColor, GTAFont Font, GTATextJustification Justification, bool outline, int alpha)
        //{
        //    try
        //    {
        //        if (TextToShow == "" || alpha == 0 || TextToShow is null)
        //        {
        //            return;
        //        }
        //        NativeFunction.Natives.SET_TEXT_FONT((int)Font);
        //        NativeFunction.Natives.SET_TEXT_SCALE(Scale, Scale);
        //        NativeFunction.Natives.SET_TEXT_COLOUR((int)TextColor.R, (int)TextColor.G, (int)TextColor.B, alpha);

        //        NativeFunction.Natives.SetTextJustification((int)Justification);

        //        NativeFunction.Natives.SET_TEXT_DROP_SHADOW();

        //        if (outline)
        //        {
        //            NativeFunction.Natives.SET_TEXT_OUTLINE(true);


        //            NativeFunction.Natives.SET_TEXT_EDGE(1, 0, 0, 0, 255);
        //        }
        //        NativeFunction.Natives.SET_TEXT_DROP_SHADOW();
        //        //NativeFunction.Natives.SetTextDropshadow(20, 255, 255, 255, 255);//NativeFunction.Natives.SetTextDropshadow(2, 2, 0, 0, 0);
        //        //NativeFunction.Natives.SetTextJustification((int)GTATextJustification.Center);
        //        if (Justification == GTATextJustification.Right)
        //        {
        //            NativeFunction.Natives.SET_TEXT_WRAP(0f, Y);
        //        }
        //        else
        //        {
        //            NativeFunction.Natives.SET_TEXT_WRAP(0f, 1f);
        //        }
        //        NativeFunction.Natives.x25fbb336df1804cb("STRING"); //NativeFunction.Natives.x25fbb336df1804cb("STRING");
        //                                                            //NativeFunction.Natives.x25FBB336DF1804CB(TextToShow);
        //        NativeFunction.Natives.x6C188BE134E074AA(TextToShow);
        //        NativeFunction.Natives.xCD015E5BB0D96A57(Y, X);
        //    }
        //    catch (Exception ex)
        //    {
        //        EntryPoint.WriteToConsole($"UI ERROR {ex.Message} {ex.StackTrace}", 0);
        //    }
        //    //return;
        //}

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
                    StreetNumber += "1E";
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

            //NativeFunction.Natives.DISABLE_ALL_CONTROL_ACTIONS(0);



            Game.DisableControlAction(0, GameControl.LookLeftRight, true);
            Game.DisableControlAction(0, GameControl.LookUpDown, true);
            Game.DisableControlAction(0, GameControl.MoveUpDown, true);
            Game.DisableControlAction(0, GameControl.MoveLeftRight, true);
            Game.DisableControlAction(0, GameControl.VehicleAccelerate, true);
            Game.DisableControlAction(0, GameControl.VehicleBrake, true);
            Game.DisableControlAction(0, GameControl.Jump, true);


            NativeFunction.Natives.SET_PLAYER_CONTROL(Game.LocalPlayer, (int)(1 << 8), false);
            Game.DisableControlAction(0, GameControl.LookLeftRight, true);
            Game.DisableControlAction(0, GameControl.LookUpDown, true);

            Game.DisableControlAction(0, GameControl.VehicleExit, true);
            Game.DisableControlAction(0, GameControl.Enter, true);


            Game.DisableControlAction(0, GameControl.Attack, true);
            Game.DisableControlAction(0, GameControl.Attack2, true);

            Game.DisableControlAction(0, GameControl.MeleeAttack1, true);
            Game.DisableControlAction(0, GameControl.MeleeAttack2, true);

        }
        public static void DisablePlayerMovementControl()
        {

            //NativeFunction.Natives.DISABLE_ALL_CONTROL_ACTIONS(0);




            Game.DisableControlAction(0, GameControl.MoveUpDown, true);
            Game.DisableControlAction(0, GameControl.MoveLeftRight, true);
            Game.DisableControlAction(0, GameControl.VehicleAccelerate, true);
            Game.DisableControlAction(0, GameControl.VehicleBrake, true);
            Game.DisableControlAction(0, GameControl.Jump, true);




            Game.DisableControlAction(0, GameControl.VehicleExit, true);
            Game.DisableControlAction(0, GameControl.Enter, true);


            Game.DisableControlAction(0, GameControl.Attack, true);
            Game.DisableControlAction(0, GameControl.Attack2, true);

            Game.DisableControlAction(0, GameControl.MeleeAttack1, true);
            Game.DisableControlAction(0, GameControl.MeleeAttack2, true);

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
        public static Vector3 GetStreetPosition(Vector3 PositionNear, bool withYield)
        {
            Vector3 pos = PositionNear;
            Vector3 SpawnPosition = Vector3.Zero;
            Vector3 outPos;
            float heading;
            float val;
            int timesRan = 0;
            for (int i = 1; i < 40; i++)
            {
                NativeFunction.Natives.GET_NTH_CLOSEST_VEHICLE_NODE_WITH_HEADING<bool>(pos.X, pos.Y, pos.Z, i, out outPos, out heading, out val, 1, 0x40400000, 0);
                if (!NativeFunction.Natives.IS_POINT_OBSCURED_BY_A_MISSION_ENTITY<bool>(outPos.X, outPos.Y, outPos.Z, 5.0f, 5.0f, 5.0f, 0))
                {
                    SpawnPosition = outPos;
                    break;
                }
                timesRan++;
                if(timesRan > 10 && withYield)
                {
                    timesRan = 0;
                    GameFiber.Yield();
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
                vehicleVariation.Livery2 = NativeFunction.Natives.GET_VEHICLE_LIVERY2<int>(vehicle);
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
                vehicleVariation.DirtLevel = NativeFunction.Natives.GET_VEHICLE_DIRT_LEVEL<float>(vehicle);
                vehicleVariation.FuelLevel = vehicle.FuelLevel;
                vehicleVariation.HasInvicibleTires = !NativeFunction.Natives.GET_VEHICLE_TYRES_CAN_BURST<bool>(vehicle);
                int tireSmokeColorRed;
                int tireSmokeColorGreen;
                int tireSmokeColorBlue;
                unsafe
                {
                    NativeFunction.CallByName<int>("GET_VEHICLE_TYRE_SMOKE_COLOR", vehicle, &tireSmokeColorRed, &tireSmokeColorGreen, &tireSmokeColorBlue);
                }
                if(tireSmokeColorRed != 0 || tireSmokeColorGreen != 0 || tireSmokeColorBlue != 0)
                {
                    vehicleVariation.IsTireSmokeColorCustom = true;
                    vehicleVariation.TireSmokeColorR = tireSmokeColorRed;// Color.FromArgb(tireSmokeColorRed, tireSmokeColorGreen, tireSmokeColorBlue);
                    vehicleVariation.TireSmokeColorG = tireSmokeColorGreen;
                    vehicleVariation.TireSmokeColorB = tireSmokeColorBlue;
                }
                vehicleVariation.VehicleNeons = new List<VehicleNeon>();
                for (int neonID = 0; neonID <= 3; neonID++)
                {
                    vehicleVariation.VehicleNeons.Add(new VehicleNeon(neonID, NativeFunction.Natives.GET_VEHICLE_NEON_ENABLED<bool>(vehicle, neonID)));
                }
                int neonColorRed;
                int neonColorGreen;
                int neonColorBlue;
                unsafe
                {
                    NativeFunction.CallByName<int>("GET_VEHICLE_NEON_COLOUR", vehicle, &neonColorRed, &neonColorGreen, &neonColorBlue);
                }
                vehicleVariation.NeonColorR = neonColorRed;// Color.FromArgb(neonColorRed, neonColorGreen, neonColorBlue);
                vehicleVariation.NeonColorG = neonColorGreen;
                vehicleVariation.NeonColorB = neonColorBlue;
                int interiorColor = 0;
                unsafe
                {
                    NativeFunction.CallByName<int>("GET_VEHICLE_EXTRA_COLOUR_5", vehicle, &interiorColor);
                }
                vehicleVariation.InteriorColor = interiorColor;
                int dashboardColor = 0;
                unsafe
                {
                    NativeFunction.CallByName<int>("GET_VEHICLE_EXTRA_COLOUR_6", vehicle, &dashboardColor);
                }
                vehicleVariation.DashboardColor = dashboardColor;
                vehicleVariation.XenonLightColor = NativeFunction.Natives.GET_VEHICLE_XENON_LIGHT_COLOR_INDEX<int>(vehicle);
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
            //0x5d0c5325
            EntryPoint.WriteToConsole($"ChangeModel START:{ModelRequested}");
            Model characterModel;
            if (uint.TryParse(ModelRequested, out uint result))
            {
                EntryPoint.WriteToConsole($"ChangeModel SET AS UINT INPUT:{ModelRequested} result:{result}");
                characterModel = new Model(result);
                
            }
            else if(ModelRequested.Left(2) == "0x")
            {
                EntryPoint.WriteToConsole($"ChangeModel LEFT IS 0x");
                uint newModel = Convert.ToUInt32(ModelRequested,16);
                EntryPoint.WriteToConsole($"ChangeModel LEFT IS 0x newModel{newModel}");
                characterModel = new Model(newModel);
            }
            else
            {
                characterModel = new Model(ModelRequested);
            }
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
                    return "Unknown";
            }
        }



        public static MaterialHash GroundMaterialAtPosition(Vector3 position, Entity ignoredEntity)
        {
            Vector3 source = position;//Game.LocalPlayer.Character.Position;
            Vector3 target = new Vector3(source.X, source.Y, source.Z + 1.0f);
            if (!NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD<bool>(source.X, source.Y, source.Z, out float GroundZ, true, false))
            {
                return 0;
            }
            target = new Vector3(source.X, source.Y, GroundZ - 1.0f);
            int ShapeTestResultID = NativeFunction.Natives.START_SHAPE_TEST_CAPSULE<int>(source.X, source.Y, source.Z, target.X, target.Y, target.Z, 1.0f, 1, ignoredEntity, 7);
            if (ShapeTestResultID == 0)
            {
                return 0;
            }
            Vector3 hitPositionArg;
            bool hitSomethingArg;
            uint materialHashArg;
            int entityHandleArg;
            Vector3 surfaceNormalArg;
            int Result = 0;
            unsafe
            {
                Result = NativeFunction.CallByName<int>("GET_SHAPE_TEST_RESULT_INCLUDING_MATERIAL", ShapeTestResultID, &hitSomethingArg, &hitPositionArg, &surfaceNormalArg, &materialHashArg, &entityHandleArg);
            }
            if (Result == 0)
            {
                return 0;
            }
            bool DidHit = hitSomethingArg;
            Vector3 HitPosition = hitPositionArg;
            Vector3 SurfaceNormal = surfaceNormalArg;
            uint MaterialHash = materialHashArg;
            return (MaterialHash)MaterialHash;
        }


        public static string GenerateNewLicensePlateNumber(string SerialFormat)
        {
            if (SerialFormat != "")
            {
                string NewPlateNumber = "";
                foreach (char c in SerialFormat)
                {
                    char NewChar = c;
                    if (c == Convert.ToChar(" "))
                    {
                        NewChar = Convert.ToChar(" ");
                    }
                    else if (char.IsDigit(c))
                    {
                        NewChar = RandomItems.RandomNumber();
                    }
                    else if (char.IsLetter(c))
                    {
                        NewChar = RandomItems.RandomLetter();
                    }
                    NewPlateNumber += NewChar;

                }
                return NewPlateNumber.ToUpper();
            }
            else
            {
                return "";
            }

        }

        internal static Vector3 GetOffsetPosition(Vector3 posI, float v, object barberXOffset)
        {
            throw new NotImplementedException();
        }

        internal static Vector3 GetOffsetPosition(Vector3 posI, object value, float barberXOffset)
        {
            throw new NotImplementedException();
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

    [StructLayout(LayoutKind.Sequential)]
    public struct ANIM_DATA
    {
        public int type;
        public string dictionary0;          // The dictionary name containing the first anim (used in single clip and three way blend)
        public string anim0;			// The first anim name
        public float phase0;            // The starting phase
        public float rate0;         // The rate (speed) to play back the anim at. 1.0 is standard rate
        public float weight0;           // How blended in the anim will be. When using multiple anims this can be used to affect// how much of the anim is visible relative to other anims.
        public string dictionary1;          // The dictionary name containing the second anim (only used in three way blend)
        public string anim1;			// The second anim name
        public float phase1;            // The starting phase
        public float rate1;         // The rate (speed) to play back the anim at. 1.0 is standard rate
        public float weight1;           // How blended in the anim will be. When using multiple anims this can be used to affect                           // how much of the anim is visible relative to other anims.
        public string dictionary2;          // The dictionary name containing the third anim (only used in 3 way blend)
        public string anim2;            // The third anim name
        public float phase2;		// The starting phase
        public float rate2; // The rate (speed) to play back the anim at. 1.0 is standard rate
        public float weight2;           // How blended in the anim will be. When using multiple anims this can be used to affect                                // how much of the anim is visible relative to other anims.
        public int filter;                 // The hash of the name of the filter to apply at this priority level
        public float blendInDelta; // How fast should this priority level be blended in
        public float blendOutDelta;	// How fast should this priority level be blended out at the end
        public int timeToPlay;				// Time (in milliseconds) to play this priority level for
        public int flags;          // Animation flags for this level
        public int ikFlags;                // Ik control flags for this level

    }
    public enum MaterialHash : uint//from shvdn
    {
        None = 0x0,
        Default = 0x962C3F7B,
        Concrete = 0x46CA81E8,
        ConcretePothole = 0x1567BF52,
        ConcreteDusty = 0xBF59B491,
        Tarmac = 0x10DD5498,
        TarmacPainted = 0xB26EEFB0,
        TarmacPothole = 0x70726A55,
        RumbleStrip = 0xF116BC2D,
        BreezeBlock = 0xC72165D6,
        Rock = 0xCDEB5023,
        RockMossy = 0xF8902AC8,
        Stone = 0x2D9C1E0D,
        Cobblestone = 0x2257A573,
        Brick = 0x61B1F936,
        Marble = 0x73EF7697,
        PavingSlab = 0x71AB3FEE,
        SandstoneSolid = 0x23500534,
        SandstoneBrittle = 0x7209440E,
        SandLoose = 0xA0EBF7E4,
        SandCompact = 0x1E6D775E,
        SandWet = 0x363CBCD5,
        SandTrack = 0x8E4D8AFF,
        SandUnderwater = 0xBC4922A4,
        SandDryDeep = 0x1E5E7A48,
        SandWetDeep = 0x4CCC2AFF,
        Ice = 0xD125AA55,
        IceTarmac = 0x8CE6E7D9,
        SnowLoose = 0x8C8308CA,
        SnowCompact = 0xCBA23987,
        SnowDeep = 0x608ABC80,
        SnowTarmac = 0x5C67C62A,
        GravelSmall = 0x38BBD00C,
        GravelLarge = 0x7EDC5571,
        GravelDeep = 0xEABD174E,
        GravelTrainTrack = 0x72C668B6,
        DirtTrack = 0x8F9CD58F,
        MudHard = 0x8C31B7EA,
        MudPothole = 0x129ECA2A,
        MudSoft = 0x61826E7A,
        MudUnderwater = 0xEFB2DF09,
        MudDeep = 0x42251DC0,
        Marsh = 0xD4C07E2,
        MarshDeep = 0x5E73A22E,
        Soil = 0xD63CCDDB,
        ClayHard = 0x4434DFE7,
        ClaySoft = 0x216FF3F0,
        GrassLong = 0xE47A3E41,
        Grass = 0x4F747B87,
        GrassShort = 0xB34E900D,
        Hay = 0x92B69883,
        Bushes = 0x22AD7B72,
        Twigs = 0xC98F5B61,
        Leaves = 0x8653C6CD,
        Woodchips = 0xED932E53,
        TreeBark = 0x8DD4EBB9,
        MetalSolidSmall = 0xA9BC4217,
        MetalSolidMedium = 0xEA34E8F8,
        MetalSolidLarge = 0x2CD49BD1,
        MetalHollowSmall = 0xF3B93B,
        MetalHollowMedium = 0x6E3DBFB8,
        MetalHollowLarge = 0xDD3CDCF9,
        MetalChainLinkSmall = 0x2D6E26CD,
        MetalChainLinkLarge = 0x781FA34,
        MetalCorrugatedIron = 0x31B80AD6,
        MetalGrille = 0xE699F485,
        MetalRailing = 0x7D368D93,
        MetalDuct = 0x68FEB9FD,
        MetalGarageDoor = 0xF2373DE9,
        MetalManhole = 0xD2FFA63D,
        WoodSolidSmall = 0xE82A6F1C,
        WoodSolidMedium = 0x2114B37D,
        WoodSolidLarge = 0x309F8BB7,
        WoodSolidPolished = 0x789C7AB,
        WoodFloorDusty = 0xD35443DE,
        WoodHollowSmall = 0x76D9AC2F,
        WoodHollowMedium = 0xEA3746BD,
        WoodHollowLarge = 0xC8D738E7,
        WoodChipboard = 0x461D0E9B,
        WoodOldCreaky = 0x2B13503D,
        WoodHighDensity = 0x981E5200,
        WoodLattice = 0x77E08A22,
        Ceramic = 0xB94A2EB5,
        RoofTile = 0x689E0E75,
        RoofFelt = 0xAB87C845,
        Fiberglass = 0x50B728DB,
        Tarpaulin = 0xD9B1CDE0,
        Plastic = 0x846BC4FF,
        PlasticHollow = 0x25612338,
        PlasticHighDensity = 0x9F154729,
        PlasticClear = 0x9126E8CB,
        PlasticHollowClear = 0x2E0ECF63,
        PlasticHighDensityClear = 0xB038852E,
        FiberglassHollow = 0xD256ED46,
        Rubber = 0xF7503F13,
        RubberHollow = 0xD1461B30,
        Linoleum = 0x11436942,
        Laminate = 0x6E02C9AA,
        CarpetSolid = 0x27E49616,
        CarpetSolidDusty = 0x973AE44,
        CarpetFloorboard = 0xACC354B1,
        Cloth = 0x7519E5D,
        PlasterSolid = 0xDDC7963F,
        PlasterBrittle = 0xF0FC7AFE,
        CardboardSheet = 0xE18DFF5,
        CardboardBox = 0xAC038918,
        Paper = 0x1C42F3BC,
        Foam = 0x30341454,
        FeatherPillow = 0x4FFB413F,
        Polystyrene = 0x97476A9D,
        Leather = 0xDDFF4E0C,
        TvScreen = 0x553BE97C,
        SlattedBlinds = 0x2827CBD9,
        GlassShootThrough = 0x37E12A0B,
        GlassBulletproof = 0xE931A0E,
        GlassOpaque = 0x596C55D1,
        Perspex = 0x9F73E76C,
        CarMetal = 0xFA73FCA1,
        CarPlastic = 0x7F630AE2,
        CarSoftTop = 0xC59BC28A,
        CarSoftTopClear = 0x7EFDF110,
        CarGlassWeak = 0x4A57FFCA,
        CarGlassMedium = 0x23EF48BC,
        CarGlassStrong = 0x3FD6150A,
        CarGlassBulletproof = 0x995DA5E6,
        CarGlassOpaque = 0x1E94B2B7,
        Water = 0x19F81600,
        Blood = 0x4FE54A,
        Oil = 0xDA2E9567,
        Petrol = 0x9E98536C,
        FreshMeat = 0x33C7D38F,
        DriedMeat = 0xA9DC9A13,
        EmissiveGlass = 0x5978A2ED,
        EmissivePlastic = 0x3F28ABAC,
        VfxMetalElectrified = 0xED92FC47,
        VfxMetalWaterTower = 0x2473B1BF,
        VfxMetalSteam = 0xD6CBF212,
        VfxMetalFlame = 0x13D5CB0D,
        PhysNoFriction = 0x63545F03,
        PhysGolfBall = 0x9B0A74CA,
        PhysTennisBall = 0xF0B2FF05,
        PhysCaster = 0xF1F990E5,
        PhysCasterRusty = 0x7830C8F1,
        PhysCarVoid = 0x50384F9D,
        PhysPedCapsule = 0xEE9E1045,
        PhysElectricFence = 0xBA428CAB,
        PhysElectricMetal = 0x87F87187,
        PhysBarbedWire = 0xA402C0C0,
        PhysPoolTableSurface = 0x241B6C19,
        PhysPoolTableCushion = 0x39FDE2BB,
        PhysPoolTableBall = 0xD36536C6,
        Buttocks = 0x1CD01A28,
        ThighLeft = 0xE48CC7C1,
        ShinLeft = 0x26E885F4,
        FootLeft = 0x72D0C8E7,
        ThighRight = 0xF1DFF3F9,
        ShinRight = 0xE56A0745,
        FootRight = 0xAE64A1D4,
        Spine0 = 0x8D6C3ADC,
        Spine1 = 0xBC0B421B,
        Spine2 = 0x56E0CA1D,
        Spine3 = 0x1F3C404,
        ClavicleLeft = 0xA8676EAF,
        UpperArmLeft = 0xE194CB2A,
        LowerArmLeft = 0x3E4A6464,
        HandLeft = 0x6BDCCA1,
        ClavicleRight = 0xA32DA7DA,
        UpperArmRight = 0x5979C903,
        LowerArmRight = 0x69F8EE36,
        HandRight = 0x774441B4,
        Neck = 0x666B1694,
        Head = 0xD42ACC0F,
        AnimalDefault = 0x110F7216,
        CarEngine = 0x8DBDD298,
        Puddle = 0x3B982E13,
        ConcretePavement = 0x78239B1A,
        BrickPavement = 0xBB9CA6D8,
        PhysDynamicCoverBound = 0x85F61AC9,
        VfxWoodBeerBarrel = 0x3B7F59CE,
        WoodHighFriction = 0x8070DCF9,
        RockNoinst = 0x79E4953,
        BushesNoinst = 0x55E5AAEE,
        MetalSolidRoadSurface = 0xD48AA0F2,
        StuntRampSurface = 0x8388FA6C,
        Temp01 = 0x2C848051,
        Temp02 = 0x8A1A9241,
        Temp03 = 0x71E96559,
        Temp04 = 0x72ADD5E0,
        Temp05 = 0xACEE6610,
        Temp06 = 0x3F4163F1,
        Temp07 = 0x96C43F1E,
        Temp08 = 0x5016ECD6,
        Temp09 = 0x3D285B19,
        Temp10 = 0x3C5F90A,
        Temp11 = 0x2D45692,
        Temp12 = 0x29E0C642,
        Temp13 = 0x9E65F2A7,
        Temp14 = 0xD97F800A,
        Temp15 = 0xA1961C15,
        Temp16 = 0xA5D57DD7,
        Temp17 = 0x3C514932,
        Temp18 = 0x50C38DF2,
        Temp19 = 0xD0356F62,
        Temp20 = 0x85A387EB,
        Temp21 = 0xC2251964,
        Temp22 = 0xDB059FFF,
        Temp23 = 0x1BB7608F,
        Temp24 = 0x750D8481,
        Temp25 = 0x745D8E31,
        Temp26 = 0xBD775456,
        Temp27 = 0x3500F64A,
        Temp28 = 0xB9AF9A0E,
        Temp29 = 0x40475AB5,
        Temp30 = 0xCFEBB4,
    }
}
