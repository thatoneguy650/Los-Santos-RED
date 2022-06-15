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
        Vector3 Position { get; }
        VehicleExt CurrentVehicle { get; }
        int WantedLevel { get; }
        Ped Character { get; }
        Inventory Inventory { get; set; }
        int GroupID { get; set; }
        bool IsCop { get; set; }
        bool IsBusted { get; }
        Cop AliasedCop { get; set; }
        string PlayerName { get; set; }
        bool IsCustomizingPed { get; set; }
        string ModelName { get; set; }
        bool CharacterModelIsFreeMode { get; }
        List<ButtonPrompt> ButtonPromptList { get; }
        void SetUnarmed();
        void DisplayPlayerNotification();
        void SetMoney(int currentPedMoney);
        void AddCrimeToHistory(Crime crime);
        void SetWantedLevel(int v1, string v2, bool v3);
        void TakeOwnershipOfVehicle(VehicleExt currentVehicle, bool showNotification);
        void ClearVehicleOwnership();
        void UpdateVehicleData();
        void Reset(bool v1, bool v2, bool v3, bool v4, bool v5, bool v6, bool v7, bool v8, bool v9, bool v10, bool v11, bool v12);
        void SetDemographics(string modelName, bool isMale, string v1, int v2);
    }
}
