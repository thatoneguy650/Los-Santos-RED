
using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ExtensionsMethods
{
    public static class Extensions
    {

        public static bool IsGenericList(this object o)
        {
            var oType = o.GetType();
            return (oType.IsGenericType && (oType.GetGenericTypeDefinition() == typeof(System.Collections.Generic.List<>)));
        }

        private static List<string> ShopPeds = new List<string>() { "s_m_y_ammucity_01", "s_m_m_ammucountry", "u_m_y_tattoo_01", "s_f_y_shop_low", "s_f_y_shop_mid", "s_f_m_shop_high", "s_m_m_autoshop_01", "s_m_m_autoshop_02" };
        private enum ePedType
        {
            PED_TYPE_PLAYER_0,
            PED_TYPE_PLAYER_1,
            PED_TYPE_NETWORK_PLAYER,
            PED_TYPE_PLAYER_2,
            PED_TYPE_CIVMALE,
            PED_TYPE_CIVFEMALE,
            PED_TYPE_COP,
            PED_TYPE_GANG_ALBANIAN,
            PED_TYPE_GANG_BIKER_1,
            PED_TYPE_GANG_BIKER_2,
            PED_TYPE_GANG_ITALIAN,
            PED_TYPE_GANG_RUSSIAN,
            PED_TYPE_GANG_RUSSIAN_2,
            PED_TYPE_GANG_IRISH,
            PED_TYPE_GANG_JAMAICAN,
            PED_TYPE_GANG_AFRICAN_AMERICAN,
            PED_TYPE_GANG_KOREAN,
            PED_TYPE_GANG_CHINESE_JAPANESE,
            PED_TYPE_GANG_PUERTO_RICAN,
            PED_TYPE_DEALER,
            PED_TYPE_MEDIC,
            PED_TYPE_FIREMAN,
            PED_TYPE_CRIMINAL,
            PED_TYPE_BUM,
            PED_TYPE_PROSTITUTE,
            PED_TYPE_SPECIAL,
            PED_TYPE_MISSION,
            PED_TYPE_SWAT,
            PED_TYPE_ANIMAL,
            PED_TYPE_ARMY
        };


        public static string Left(this string input, int count)
        {
            return input.Substring(0, Math.Min(input.Length, count));
        }
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }
        public static bool IsPoliceArmy(this Ped myPed, int PedType)
        {
            string ModelName = myPed.Model.Name.ToLower();
            //int PedType = NativeFunction.Natives.GET_PED_TYPE<int>(myPed);//Function.Call<int>(Hash.GET_PED_TYPE, myPed);
            if (PedType == (int)ePedType.PED_TYPE_COP || PedType == (int)ePedType.PED_TYPE_ARMY || PedType == (int)ePedType.PED_TYPE_SWAT || ModelName == "s_m_m_prisguard_01")//if ((PedType == (int)ePedType.PED_TYPE_COP || PedType == (int)ePedType.PED_TYPE_ARMY || PedType == (int)ePedType.PED_TYPE_SWAT || ModelName == "s_m_m_prisguard_01" || ModelName == "s_m_m_security_01") && ModelName != "Shepherd")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsPolice(this Ped myPed, int PedType)
        {
            string ModelName = myPed.Model.Name.ToLower();
            if (PedType == (int)ePedType.PED_TYPE_COP || PedType == (int)ePedType.PED_TYPE_SWAT || ModelName == "s_m_m_prisguard_01")// || ModelName == "s_m_m_security_01") && ModelName != "Shepherd")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsArmy(this Ped myPed, int PedType)
        {
            if (PedType == (int)ePedType.PED_TYPE_ARMY)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsGangMember(this Ped myPed)
        {
            string Nameo = myPed.RelationshipGroup.Name;
            if (!string.IsNullOrEmpty(Nameo) && !string.IsNullOrWhiteSpace(Nameo) && Nameo.ToLower().Contains("gang"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsSecurity(this Ped myPed)
        {
            string Nameo = myPed.RelationshipGroup.Name;
            if ((!string.IsNullOrEmpty(Nameo) && !string.IsNullOrWhiteSpace(Nameo) && Nameo.ToLower().Contains("securityguard")) || (myPed.Model.Name.ToLower() == "s_m_m_security_01"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Gets the display name for an enum.
        /// </summary>
        /// <param name="enumValue"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static string GetDisplayName(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var names = new List<string>();
            foreach (var e in Enum.GetValues(enumType))
            {
                var flag = (Enum)e;
                if (enumValue.HasFlag(flag))
                {
                    names.Add(GetSingleDisplayName(flag));
                }
            }
            if (names.Count <= 0) throw new ArgumentException();
            if (names.Count == 1) return names.First();
            return string.Join(", ", names);
        }

        /// <summary>
        /// Gets the display value for a single enum flag or 
        /// name of that flag if the display value is not set
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static string GetSingleDisplayName(this Enum flag)
        {
            try
            {
                return flag.GetType()
                        .GetMember(flag.ToString())
                        .First()
                        .GetCustomAttribute<DisplayAttribute>()
                        .Name;
            }
            catch
            {
                return flag.ToString();
            }
        }
        public static bool IsNormalPerson(this Ped myPed)
        {
            if (myPed.Model.Hash == 225514697 || myPed.Model.Hash == 2602752943 || myPed.Model.Hash == 2608926626)
            {
                return false;
            }
            return true;
            //int PedType = NativeFunction.Natives.GET_PED_TYPE<int>(myPed);//Function.Call<int>(Hash.GET_PED_TYPE, myPed);
            //if (PedType == (int)ePedType.PED_TYPE_CIVMALE || PedType == (int)ePedType.PED_TYPE_CIVFEMALE || PedType == (int)ePedType.PED_TYPE_CIVFEMALE || PedType == (int)ePedType.PED_TYPE_MISSION)// || ModelName == "s_m_m_security_01") && ModelName != "Shepherd")
            //{
            //    string ModelName = myPed.Model.Name.ToLower();
            //    if (ShopPeds.Contains(ModelName))
            //    {
            //        return false;
            //    }
            //    else
            //    {
            //        return true;
            //    }
            //}
            //else
            //{
            //    return false;
            //}
        }
        public static T Next<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(Arr, src) + 1;
            return (Arr.Length == j) ? Arr[0] : Arr[j];
        }
        public static List<Enum> GetFlags(this Enum e)
        {
            return Enum.GetValues(e.GetType()).Cast<Enum>().Where(e.HasFlag).ToList();
        }
        public static bool IsConsideredMainCharacter(this Ped myPed)
        {
            int PedType = NativeFunction.Natives.GET_PED_TYPE<int>(myPed);//Function.Call<int>(Hash.GET_PED_TYPE, myPed);
            if (PedType == 0 || PedType == 1 || PedType == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool SetLock(this Vehicle ToLock, VehicleLockStatus DesiredLockStatus)
        {
            if (ToLock.LockStatus == DesiredLockStatus)
            {
                EntryPoint.WriteToConsole($"SetLock ALREADY DESIRED STATUS {DesiredLockStatus}");
                return true;
            }
            foreach (VehicleDoor myDoor in ToLock.GetDoors())
            {
                if (!myDoor.IsValid() || myDoor.IsOpen)
                {
                    EntryPoint.WriteToConsole("SetLock DOOR OR WINDOW OPEN, NOT LOCKING");
                    return false;//invalid doors make the car not locked
                }
            }
            if (!NativeFunction.Natives.ARE_ALL_VEHICLE_WINDOWS_INTACT<bool>(ToLock))
            {
                EntryPoint.WriteToConsole("SetLock WINDOW BROKEN, NOT LOCKING");
                return false;//broken windows == not locked
            }
            if (ToLock.IsConvertible && ToLock.ConvertibleRoofState == VehicleConvertibleRoofState.Lowered)
            {
                EntryPoint.WriteToConsole($"SetLock IS CONVERTIBLE AND LOWERED IsConvertible:{ToLock.IsConvertible} ConvertibleRoofState:{ToLock.ConvertibleRoofState}");
                return false;
            }
            //if(!NativeFunction.Natives.IS_VEHICLE_A_CONVERTIBLE<bool>(ToLock, false) && NativeFunction.Natives.IS_VEHICLE_A_CONVERTIBLE<bool>(ToLock,true))
            //{
            //    EntryPoint.WriteToConsole($"SetLock IS CONVERTIBLE WITH EXTRAS");
            //    return false;
            //}
            if (ToLock.IsBike || ToLock.IsPlane || ToLock.IsHelicopter)
            {
                EntryPoint.WriteToConsole("SetLock IS BIKE PLANE OR HELICOPTER");
                return false;
            }
            ToLock.LockStatus = DesiredLockStatus;
            return true;
        }
        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            if (source.Count() == 0)
                return default;
            else
                return source.PickRandom(1).Single();
        }
        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }
        private static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }


        public static T DeepCopy<T>(T other)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, other);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }
        public static bool PointIsInFrontOfPed(this Ped myPed, Vector3 position) => GetDotVectorResult(myPed, position) > 0;
        public static bool PointIsDirectlyInFrontOfPed(this Ped myPed, Vector3 position) => GetDotVectorResult(myPed, position) > 0.7f;

        public static bool IsThisPedInFrontOf(this Ped myPed, Ped ToCheck)
        {
            float Result = GetDotVectorResult(ToCheck, myPed);
            if (Result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private static float GetDotVectorResult(Entity source, Entity target)
        {
            if (source.Exists() && target.Exists())
            {
                Vector3 dir = (target.Position - source.Position).ToNormalized();
                return Vector3.Dot(dir, source.ForwardVector);
            }
            else return -1.0f;
        }


        public static float GetDotVectorResult(Entity source, Vector3 position)
        {
            if (source.Exists())
            {
                Vector3 dir = (position - source.Position).ToNormalized();
                return Vector3.Dot(dir, source.ForwardVector);
            }
            else return -1.0f;
        }

        public static float GetHeadingDifference(float initial, float final)
        {
            if (initial > 360 || initial < 0 || final > 360 || final < 0)
            {
                //throw some error
            }

            var diff = final - initial;
            var absDiff = Math.Abs(diff);

            if (absDiff <= 180)
            {
                //Edit 1:27pm
                return absDiff == 180 ? absDiff : diff;
            }

            else if (final > initial)
            {
                return absDiff - 360;
            }

            else
            {
                return 360 - absDiff;
            }
        }
        public static int Round(this int i, int nearest)
        {
            if (nearest <= 0 || nearest % 10 != 0)
                throw new ArgumentOutOfRangeException("nearest", "Must round to a positive multiple of 10");

            return (i + 5 * nearest / 10) / nearest * nearest;
        }

    }
}
