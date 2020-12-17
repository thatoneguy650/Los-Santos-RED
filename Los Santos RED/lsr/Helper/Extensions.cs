
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
        public static bool IsPoliceArmy(this Ped myPed)
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
        public static bool IsPolice(this Ped myPed)
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
        public static bool IsArmy(this Ped myPed)
        {
            if (NativeFunction.CallByName<int>("GET_PED_TYPE", myPed) == 29)
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
            if(ToLock.LockStatus == DesiredLockStatus)
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
