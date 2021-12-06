using LosSantosRED.lsr.Player;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IPedSwappable
    {
        int Money { get; }
        bool IsWanted { get; }
        bool IsMoveControlPressed { get; }
        PedVariation CurrentModelVariation { get; set; }
        string CurrentModelName { get; set; }
        Vector3 Position { get; }
        VehicleExt CurrentVehicle { get; }
        //PoolHandle OwnedVehicleHandle { get; set; }
        VehicleExt OwnedVehicle { get;  }
        int WantedLevel { get; }
        Ped Character { get; }
        Inventory Inventory { get; set; }
        int GroupID { get; set; }
        bool IsCop { get; set; }
        bool IsBusted { get; }
        Cop AliasedCop { get; set; }
        string PlayerName { get; }
        HeadBlendData CurrentHeadBlendData { get; set; }
        int CurrentPrimaryHairColor { get; set; }
        int CurrentSecondaryColor { get; set; }
        bool IsCustomizingPed { get; set; }

        void SetUnarmed();
        void DisplayPlayerNotification();
        void SetMoney(int currentPedMoney);
        void AddCrimeToHistory(Crime crime);
        void UpdateCurrentVehicle();
        void SetWantedLevel(int v1, string v2, bool v3);
        void TakeOwnershipOfVehicle(VehicleExt currentVehicle);
        void ClearVehicleOwnership();
    }
}
