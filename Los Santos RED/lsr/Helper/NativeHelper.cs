using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Helper
{
    public static class NativeHelper
    {
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
            NativeFunction.Natives.BEGIN_TEXT_COMMAND_THEFEED_POST("STRING");
            NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(text);
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
                return myPedVariation;
            }
            catch (Exception e)
            {
                EntryPoint.WriteToConsole("CopyPedComponentVariation! CopyPedComponentVariation Error; " + e.Message, 0);
                return null;
            }
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

        internal static void GetSimpleCompassHeading(object heading)
        {
            throw new NotImplementedException();
        }
    }
}
