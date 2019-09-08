
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
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
            else if (Game.LocalPlayer.Character.Inventory.EquippedWeapon != (WeaponHash)2725352035 || Game.LocalPlayer.Character.Inventory.EquippedWeapon != (WeaponHash)966099553)
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
        public static bool IsInRangeOf(this Ped myPed,Vector3 position, float range)
        {
            return Vector3.Subtract(myPed.Position, position).LengthSquared() < range * range;
        }
    }
}
