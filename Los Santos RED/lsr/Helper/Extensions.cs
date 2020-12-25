
using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
        public static bool IsPoliceArmy(this Ped myPed)
        {
            string ModelName = myPed.Model.Name.ToLower();
            int PedType = NativeFunction.CallByName<int>("GET_PED_TYPE", myPed);//Function.Call<int>(Hash.GET_PED_TYPE, myPed);
            if (PedType == (int)ePedType.PED_TYPE_COP || PedType == (int)ePedType.PED_TYPE_ARMY || PedType == (int)ePedType.PED_TYPE_SWAT || ModelName == "s_m_m_prisguard_01")//if ((PedType == (int)ePedType.PED_TYPE_COP || PedType == (int)ePedType.PED_TYPE_ARMY || PedType == (int)ePedType.PED_TYPE_SWAT || ModelName == "s_m_m_prisguard_01" || ModelName == "s_m_m_security_01") && ModelName != "Shepherd")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsPolice(this Ped myPed)
        {
            string ModelName = myPed.Model.Name.ToLower();
            int PedType = NativeFunction.CallByName<int>("GET_PED_TYPE", myPed);//Function.Call<int>(Hash.GET_PED_TYPE, myPed);
            if (PedType == (int)ePedType.PED_TYPE_COP || PedType == (int)ePedType.PED_TYPE_SWAT || ModelName == "s_m_m_prisguard_01")// || ModelName == "s_m_m_security_01") && ModelName != "Shepherd")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsArmy(this Ped myPed)
        {
            if (NativeFunction.CallByName<int>("GET_PED_TYPE", myPed) == (int)ePedType.PED_TYPE_ARMY)
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
            if (!string.IsNullOrEmpty(Nameo) && !string.IsNullOrWhiteSpace(Nameo) && Nameo.ToLower().Contains("securitygurad"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsNormalPerson(this Ped myPed)
        {
            if (myPed.Model.Hash == 225514697 || myPed.Model.Hash == 2602752943 || myPed.Model.Hash == 2608926626)
            {
                return false;
            }
            int PedType = NativeFunction.CallByName<int>("GET_PED_TYPE", myPed);//Function.Call<int>(Hash.GET_PED_TYPE, myPed);
            if (PedType == (int)ePedType.PED_TYPE_CIVMALE || PedType == (int)ePedType.PED_TYPE_CIVFEMALE || PedType == (int)ePedType.PED_TYPE_CIVFEMALE || PedType == (int)ePedType.PED_TYPE_MISSION)// || ModelName == "s_m_m_security_01") && ModelName != "Shepherd")
            {
                string ModelName = myPed.Model.Name.ToLower();
                if (ShopPeds.Contains(ModelName))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        public static bool IsConsideredMainCharacter(this Ped myPed)
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
        public static bool SetLock(this Vehicle ToLock, VehicleLockStatus DesiredLockStatus)
        {
            if (ToLock.LockStatus == DesiredLockStatus)
            {
                return true;
            }
            foreach (VehicleDoor myDoor in ToLock.GetDoors())
            {
                if (!myDoor.IsValid() || myDoor.IsOpen)
                {
                    return false;//invalid doors make the car not locked
                }
            }
            if (!NativeFunction.CallByName<bool>("ARE_ALL_VEHICLE_WINDOWS_INTACT", ToLock))
            {
                return false;//broken windows == not locked
            }
            if (ToLock.IsConvertible && ToLock.ConvertibleRoofState == VehicleConvertibleRoofState.Lowered)
            {
                return false;
            }
            if (ToLock.IsBike || ToLock.IsPlane || ToLock.IsHelicopter)
            {
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

    }
}
