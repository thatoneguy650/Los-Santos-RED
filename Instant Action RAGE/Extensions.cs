
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionsMethods
{
    public static class Extensions
    {

        public static bool isPoliceArmy(this Ped myPed)
        {
            int PedType = NativeFunction.CallByName<int>("GET_PED_TYPE", myPed);//Function.Call<int>(Hash.GET_PED_TYPE, myPed);
            if ((PedType == 6 || PedType == 29 || PedType == 27) && myPed.Model.Name != "Shepherd")//PedHash.Shepherd)
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
                return true;
                //if (Function.Call<bool>(Hash.IS_PED_MODEL, myPed, (uint)PedHash.Ammucity01SMY) || Function.Call<bool>(Hash.IS_PED_MODEL, myPed, (uint)PedHash.Ammucity01SMY))
                //    return false;
                //else
                //    return true;
            }
            else
            {
                return false;
            }
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
        //public static bool IS_ANY_PED_IN_LOS(List<Ped> peds, float minAngle, bool withOcclusion = true, bool includeDead = false)
        //{
        //    foreach (Ped ped in peds)
        //    {
        //        if (ped.Exists())
        //        {
        //            if (ped.IsDead && !includeDead)
        //            {
        //                continue;
        //            }
        //            if (withOcclusion) // with obstacle detection                
        //            {
        //                if (HAS_ENTITY_CLEAR_LOS_TO_ENTITY(ped, Game.LocalPlayer.Character)) // No Obstacles?   
        //                {
        //                    float dot = getDotVectorResult(ped, Game.LocalPlayer.Character);
        //                    if (dot > minAngle) // Is in acceptable range for dot product?     
        //                    {
        //                        return true;
        //                    }
        //                }


        //            }
        //            else // without obstacle detection     
        //            {
        //                float dot = getDotVectorResult(ped, Game.LocalPlayer.Character);
        //                if (dot > minAngle) // Is in acceptable range for dot product?   
        //                {
        //                    return true;
        //                }
        //            }
        //        }
        //    }
        //    return false;
        //}
        public static float getDotVectorResult(Ped source, Ped target)
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
        public static bool PlayerVehicleIsBehind(this Vehicle myVehicle)
        {
            float Result = getDotVectorResult(Game.LocalPlayer.Character.CurrentVehicle, myVehicle);
            if (Result > 0)
                return true;
            else
                return false;

        }
        public static bool PlayerVehicleIsInFront(this Vehicle myvehicle)
        {
            float Result = getDotVectorResult(myvehicle, Game.LocalPlayer.Character.CurrentVehicle);
            if (Result > 0)
                return true;
            else
                return false;

        }

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



        //public static bool IsInFront(this Ped myPed)
        //{
        //    /// <summary>        /// Determine the dot vector product between source and target ped       
        //    /// 
        //    /// 
        //    /// /// </summary>        /// <param name="source"></param>        /// <param name="target"></param>      
        //    /// /// <returns>float in range -1.0 to 1.0, negative value if source is behind target, positive value if source is in front of target, 0 if source is orthogonal to target, 1 if directly in front of target, -1 if directly behind target</returns>       
        //    public static float getDotVectorResult(Ped source, Ped target)
        //    {
        //        if (source.Exists() && target.Exists())
        //        {
        //            Vector3 dir = (target.Position - source.Position).Normalized;
        //            return Vector3.Dot(dir, source.ForwardVector);
        //        }            else                return -1.0f;
        //    }        
        //    /// <summary>  
        //    /// /// Determine if any of given peds is in line of sight to the player   
        //    /// /// </summary>   
        //    /// /// <param name="peds">List of peds to check for</param>    
        //    /// /// <param name="minAngle">the value of the dot product at which a ped is considered not in LoS</param>   
        //    /// /// <param name="withOcclusion">true if occlusion check should be included, false otherwise</param> 
        //    /// /// <param name="includeDead">true if dead peds should be included, false otherwise</param>
        //    /// /// <returns>true if at least one ped was in los, false otherwise</returns>  
        //    public static bool IS_ANY_PED_IN_LOS(List<Ped> peds, float minAngle, bool withOcclusion = true, bool includeDead = false)
        //    {            foreach (Ped ped in peds)
        //        {                if (ped.Exists())
        //            {
        //                if (ped.IsDead && !includeDead)
        //                {
        //                    continue;
        //                }
        //                if (withOcclusion) // with obstacle detection              
        //                {
        //                    if (HAS_ENTITY_CLEAR_LOS_TO_ENTITY(ped, Game.Player.Character)) // No Obstacles?          
        //                    {
        //                        float dot = getDotVectorResult(ped, Game.Player.Character);
        //                        if (dot > minAngle) // Is in acceptable range for dot product?       
        //                        {                                return true;
        //                        }                        }
        //                }                    else // without obstacle detection        
        //                {
        //                    float dot = getDotVectorResult(ped, Game.Player.Character);
        //                    if (dot > minAngle) // Is in acceptable range for dot product?      
        //                    {                            return true;
        //                    }
        //                }
        //            }
        //        }
        //        return false;
        //    }
        //}
    }
}
