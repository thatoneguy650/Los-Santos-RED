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
        Equipment Equipment { get; }
        Inventory Inventory { get; set; }
        Cop AliasedCop { get; set; }
        Ped Character { get; }
        bool CharacterModelIsFreeMode { get; }
        PedVariation CurrentModelVariation { get; set; }
        VehicleExt CurrentVehicle { get; }
        int GroupID { get; set; }
        bool IsBusted { get; }
        bool IsCop { get; set; }
        bool IsCustomizingPed { get; set; }
        bool IsMoveControlPressed { get; }
        bool IsWanted { get; }
        string ModelName { get; set; }
        int Money { get; }
        string PlayerName { get; set; }
        Vector3 Position { get; }
        int WantedLevel { get; }
        ButtonPrompts ButtonPrompts { get; }

        void AddCrimeToHistory(Crime crime);
        void ClearVehicleOwnership();
        void DisplayPlayerNotification();
        void Reset(bool v1, bool v2, bool v3, bool v4, bool v5, bool v6, bool v7, bool v8, bool v9, bool v10, bool v11, bool v12, bool v13);
        void SetDemographics(string modelName, bool isMale, string v1, int v2);
        void SetMoney(int currentPedMoney);
        void SetWantedLevel(int v1, string v2, bool v3);
        void TakeOwnershipOfVehicle(VehicleExt currentVehicle, bool showNotification);
        void UpdateVehicleData();
    }
}
