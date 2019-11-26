
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExtensionsMethods
{
    public static class Extensions
    {
        public static List<string> ShopPeds = new List<string>() { "s_m_y_ammucity_01","s_m_m_ammucountry","u_m_y_tattoo_01","s_f_y_shop_low","s_f_y_shop_mid","s_f_m_shop_high","s_m_m_autoshop_01","s_m_m_autoshop_02" };
        //Controls
            public static bool IsMoveControlPressed()
            {
                if (Game.IsControlPressed(2, GameControl.MoveUp) || Game.IsControlPressed(2, GameControl.MoveRight) || Game.IsControlPressed(2, GameControl.MoveDown) || Game.IsControlPressed(2, GameControl.MoveLeft))
                    return true;
                else
                    return false;
            }
        //Pedestrian
        public static bool isPoliceArmy(this Ped myPed)
        {
            string ModelName = myPed.Model.Name.ToLower();
            int PedType = NativeFunction.CallByName<int>("GET_PED_TYPE", myPed);//Function.Call<int>(Hash.GET_PED_TYPE, myPed);
            if ((PedType == 6 || PedType == 29 || PedType == 27 || ModelName == "s_m_m_prisguard_01" || ModelName == "s_m_m_security_01") && ModelName != "Shepherd")//PedHash.Shepherd)
            {
                    
                return true;
            }
            else
            {
                return false;
            }
        }



        public static bool isPolice(this Ped myPed)
        {
            string ModelName = myPed.Model.Name.ToLower();
            int PedType = NativeFunction.CallByName<int>("GET_PED_TYPE", myPed);//Function.Call<int>(Hash.GET_PED_TYPE, myPed);
            if ((PedType == 6 || PedType == 27 || ModelName == "s_m_m_prisguard_01" || ModelName == "s_m_m_security_01") && ModelName != "Shepherd")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool isArmy(this Ped myPed)
        {
            int PedType = NativeFunction.CallByName<int>("GET_PED_TYPE", myPed);//Function.Call<int>(Hash.GET_PED_TYPE, myPed);
            if (PedType == 29)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool isMainCharacter(this Ped myPed)
        {
            int PedType = NativeFunction.CallByName<int>("GET_PED_TYPE", myPed);//Function.Call<int>(Hash.GET_PED_TYPE, myPed);
            if (PedType == 0 || PedType == 1 || PedType == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool isConsideredArmed(this Ped myPed)
        {
            if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && !Game.LocalPlayer.IsFreeAiming)
                return false;
            else if (Game.LocalPlayer.Character.Inventory.EquippedWeapon == null)
                return false;
            else if (Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)2725352035 || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)966099553)
                return false;
            else
                return true;
        }
        public static bool IsNormalPed(this Ped myPed)
        {
            int PedType = NativeFunction.CallByName<int>("GET_PED_TYPE", myPed);
            if (PedType == 4 || PedType == 5 || PedType == 26)
            {
            string ModelName = myPed.Model.Name.ToLower();
                if (ShopPeds.Contains(ModelName))
                    return false;
                else
                    return true;
            }
            else
            {
                return false;
            }
        }
        public static bool isInLosSantosCity(this Ped myPed)
        {
            if (Game.LocalPlayer.Character.Position.Y <= 0f && Game.LocalPlayer.Character.Position.X <= 1500)
                return true;
            else
                return false;
        }

        
        public static bool inSameCar(this Ped myPed, Ped PedToCompare)
        {
            bool ImInVehicle = myPed.IsInAnyVehicle(false);
            bool YourInVehicle = PedToCompare.IsInAnyVehicle(false);
            if (ImInVehicle && YourInVehicle)
            {
                if (myPed.CurrentVehicle == PedToCompare.CurrentVehicle)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        public static bool isBelowWorld(this Ped myPed)
        {
            if (myPed.Position.Z <= -50)
                return true;
            else
                return false;
        }

        public static bool CanTakeoverPed(this Ped myPed)
        {
            if (myPed.Exists() && myPed != Game.LocalPlayer.Character && myPed.IsAlive && myPed.IsHuman && !myPed.isPoliceArmy() && !myPed.inSameCar(Game.LocalPlayer.Character) && myPed.IsNormalPed() && !myPed.isBelowWorld())
                return true;
            else
                return false;

        }
        public static void GiveCash(this Ped myPed, int Amount, String PlayerName)
        {
            int CurrentCash;
            uint PlayerCashHash = CashHash(PlayerName);
            unsafe
            {
                NativeFunction.CallByName<int>("STAT_GET_INT", PlayerCashHash, &CurrentCash, -1);
            }
            if(CurrentCash+ Amount < 0)
                NativeFunction.CallByName<int>("STAT_SET_INT", PlayerCashHash, 0, 1);
            else
                NativeFunction.CallByName<int>("STAT_SET_INT", PlayerCashHash, CurrentCash + Amount, 1);
        }
        public static int GetCash(this Ped myPed, String PlayerName)
        {
            int CurrentCash;
            unsafe
            {
                NativeFunction.CallByName<int>("STAT_GET_INT", CashHash(PlayerName), &CurrentCash, -1);
            }

            return CurrentCash;
        }
        public static void SetCash(this Ped myPed, int Amount, String PlayerName)
        {
            NativeFunction.CallByName<int>("STAT_SET_INT", CashHash(PlayerName), Amount, 1);
        }
        private static uint CashHash(String PlayerName)
        {
            switch (PlayerName)
            {
                case "Michael":
                    return Game.GetHashKey("SP0_TOTAL_CASH");
                case "Franklin":
                    return Game.GetHashKey("SP1_TOTAL_CASH");
                case "Trevor":
                    return Game.GetHashKey("SP2_TOTAL_CASH");
                default:
                    return Game.GetHashKey("SP0_TOTAL_CASH");
            }
        }
        public static bool IsInRangeOf(this Ped myPed, Vector3 position, float range)
        {
            return Vector3.Subtract(myPed.Position, position).LengthSquared() < range * range;
        }
        public static float RangeTo(this Ped myPed, Vector3 position)
        {
            return Math.Abs(Vector3.Subtract(myPed.Position, position).Length());
        }

        //Math
            public static double GetRandomNumber(double minimum, double maximum)
            {
                Random random = new Random();
                return random.NextDouble() * (maximum - minimum) + minimum;
            }
        //Car Stuff
            public static bool IsRoadWorthy(this Vehicle myCar)
            {
                bool LightsOn;
                bool HighbeamsOn;
                if (Police.IsNightTime)
                {
                    unsafe
                    {
                        NativeFunction.CallByName<bool>("GET_VEHICLE_LIGHTS_STATE", myCar, &LightsOn, &HighbeamsOn);
                    }
                    if (!LightsOn)
                        return false;
                    if (HighbeamsOn)
                        return false;



                    if (NativeFunction.CallByName<bool>("GET_IS_RIGHT_VEHICLE_HEADLIGHT_DAMAGED", myCar) || NativeFunction.CallByName<bool>("GET_IS_LEFT_VEHICLE_HEADLIGHT_DAMAGED", myCar))
                        return false;
                }

                if (myCar.LicensePlate == "        ")
                    return false;

                return true;
            }
            public static bool IsDamaged(this Vehicle myCar)
            {
                if (myCar.Health <= 700 || myCar.EngineHealth <= 700)
                    return true;

                if (!NativeFunction.CallByName<bool>("ARE_ALL_VEHICLE_WINDOWS_INTACT", myCar))
                    return true;

                VehicleDoor[] CarDoors = myCar.GetDoors();

                foreach (VehicleDoor myDoor in CarDoors)
                {
                    if (myDoor.IsDamaged)
                        return true;
                }

                if (Police.IsNightTime)
                {
                    if (NativeFunction.CallByName<bool>("GET_IS_RIGHT_VEHICLE_HEADLIGHT_DAMAGED", myCar) || NativeFunction.CallByName<bool>("GET_IS_LEFT_VEHICLE_HEADLIGHT_DAMAGED", myCar))
                        return true;
                }

                if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar, 0, false))
                    return true;

                if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar, 1, false))
                    return true;

                if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar, 2, false))
                    return true;

                if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar, 3, false))
                    return true;

                if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar, 4, false))
                    return true;

                if (NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar, 5, false))
                    return true;

                return false;
            }
        //Vector
            public static bool IsWithin(this float value, float minimum, float maximum)
            {
                return value >= minimum && value <= maximum;
            }
            public static float getDotVectorResult(Ped source, Ped target)
            {
                if (source.Exists() && target.Exists())
                {
                    Vector3 dir = (target.Position - source.Position).ToNormalized();
                    return Vector3.Dot(dir, source.ForwardVector);
                }
                else return -1.0f;
            }
            public static float getDotVectorResult(Entity source, Entity target)
            {
                if (source.Exists() && target.Exists())
                {
                    Vector3 dir = (target.Position - source.Position).ToNormalized();
                    return Vector3.Dot(dir, source.ForwardVector);
                }
                else return -1.0f;
            }
            public static float getDotVectorResult(Vehicle source, Vehicle target)
            {
                if (source.Exists() && target.Exists())
                {
                    Vector3 dir = (target.Position - source.Position).ToNormalized();
                    return Vector3.Dot(dir, source.ForwardVector);
                }
                else return -1.0f;
            }
            public static bool PlayerIsInFront(this Ped myPed)
            {
                float Result = getDotVectorResult(myPed, Game.LocalPlayer.Character);
                if (Result > 0)
                    return true;
                else
                    return false;

            }
            public static bool IsInFront(this Entity Target,Entity Source)
            {
                float Result = getDotVectorResult(Target, Source);
                if (Result > 0)
                    return true;
                else
                    return false;

            }
            public static bool PlayerVehicleIsBehind(this Vehicle myVehicle)
            {
                float Result = getDotVectorResult(Game.LocalPlayer.Character.CurrentVehicle, myVehicle);
                if (Result > 0)
                    return true;
                else
                    return false;

            }
            public static void FaceEntity(this Entity Source,Entity Target)
            {
                Vector3 Resultant = Vector3.Subtract(Target.Position, Source.Position);
                Source.Heading = NativeFunction.CallByName<float>("GET_HEADING_FROM_VECTOR_2D", Resultant.X, Resultant.Y);
            }
            public static float Dot(Vector3 left, Vector3 right)
            {
                return left.X * right.X + left.Y * right.Y + left.Z * right.Z;
            }
            public static float Angle(Vector3 from, Vector3 to)
            {
                from.Normalize();
                to.Normalize();
                double dot = Dot(from, to);
                return (float)(System.Math.Acos((dot)) * (180.0 / System.Math.PI));
            }
            public static bool FacingSameDirection(this Entity Entity1, Entity Entity2)
            {
                float MyAngle = Angle(Entity1.ForwardVector, Entity2.ForwardVector);
                if (MyAngle <= 40f)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            public static bool FacingOppositeDirection(this Entity Entity1, Entity Entity2)
            {
                float MyAngle = Angle(Entity1.ForwardVector, Entity2.ForwardVector);
                if (MyAngle >= 140f && MyAngle <= 220f)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        //Enumerable Functions
            public static T PickRandom<T>(this IEnumerable<T> source)
            {
                if (source.Count() == 0)
                    return default(T);
                else
                    return source.PickRandom(1).Single();
            }

            public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
            {
                return source.Shuffle().Take(count);
            }

            public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
            {
                return source.OrderBy(x => Guid.NewGuid());
        }
        //Color Picking
            public static int closestColor1(List<Color> colors, Color target)
            {
                var hue1 = target.GetHue();
                var diffs = colors.Select(n => getHueDistance(n.GetHue(), hue1));
                var diffMin = diffs.Min(n => n);
                return diffs.ToList().FindIndex(n => n == diffMin);
            }

            // closed match in RGB space
            public static int closestColor2(List<Color> colors, Color target)
            {
                var colorDiffs = colors.Select(n => ColorDiff(n, target)).Min(n => n);
                return colors.FindIndex(n => ColorDiff(n, target) == colorDiffs);
            }


            // weighed distance using hue, saturation and brightness
            public static int closestColor3(List<Color> colors, Color target)
            {
                float hue1 = target.GetHue();
                var num1 = ColorNum(target);
                var diffs = colors.Select(n => Math.Abs(ColorNum(n) - num1) +
                                               getHueDistance(n.GetHue(), hue1));
                var diffMin = diffs.Min(x => x);
                return diffs.ToList().FindIndex(n => n == diffMin);
            }

            // color brightness as perceived:
            public static float getBrightness(Color c)
            { return (c.R * 0.299f + c.G * 0.587f + c.B * 0.114f) / 256f; }

            // distance between two hues:
            public static float getHueDistance(float hue1, float hue2)
            {
                float d = Math.Abs(hue1 - hue2); return d > 180 ? 360 - d : d;
            }

            //  weighed only by saturation and brightness (from my trackbars)
            public static float ColorNum(Color c)
            {
                return c.GetSaturation() * 1 +
                            getBrightness(c) * 1;
            }

            // distance in RGB space
            public static int ColorDiff(Color c1, Color c2)
            {
                return (int)Math.Sqrt((c1.R - c2.R) * (c1.R - c2.R)
                                       + (c1.G - c2.G) * (c1.G - c2.G)
                                       + (c1.B - c2.B) * (c1.B - c2.B));
            }

        //Unused
        public static bool Like(this string toSearch, string toFind)
        {
            return new Regex(@"\A" + new Regex(@"\.|\$|\^|\{|\[|\(|\||\)|\*|\+|\?|\\").Replace(toFind, ch => @"\" + ch).Replace('_', '.').Replace("%", ".*") + @"\z", RegexOptions.Singleline).IsMatch(toSearch);
        }
        public static bool PlayerVehicleIsInFront(this Vehicle myvehicle)
        {
            float Result = getDotVectorResult(myvehicle, Game.LocalPlayer.Character.CurrentVehicle);
            if (Result > 0)
                return true;
            else
                return false;

        }
        public static bool InFrontOf(this Vehicle Source, Vehicle Target)
        {
            float Result = getDotVectorResult(Target, Source);
            if (Result > 0)
                return true;
            else
                return false;

        }


    }
}
