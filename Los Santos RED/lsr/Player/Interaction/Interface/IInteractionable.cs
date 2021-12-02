using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IInteractionable
    {
        bool IsHoldingUp { get; set; }
        WeaponCategory CurrentWeaponCategory { get; }
        bool IsConversing { get; set; }
        Ped Character { get; }
        PedExt CurrentTargetedPed { get; }
        bool IsAliveAndFree { get; }
        List<ButtonPrompt> ButtonPrompts { get; }
        bool IsInVehicle { get; }
        bool CanConverse { get; }
        int Money { get; }
        bool IsPerformingActivity { get; }

        void SetAngeredCop();
        void GiveMoney(int v);
        void StartSmoking();
        void StartDrinkingActivity();
        void AddToInventory(ModItem offering, int v);
        void StartServiceActivity(ModItem toAdd, GameLocation store);
        void TakeOwnershipOfVehicle(VehicleExt myNewCar);
        bool RemoveFromInventory(ModItem toAdd, int amountPerPackage);
        bool HasItemInInventory(string modItemName);
    }
}
