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
    public interface IInventoryable
    {
        Inventory Inventory { get; }
        bool IsPerformingActivity { get; }
        int Money { get; }
        Ped Character { get; }

        bool RemoveFromInventory(ModItem toAdd, int v);
        void GiveMoney(int salesPrice);
        void AddToInventory(ModItem toAdd, int amountPerPackage);
      //  void StartServiceActivity(ModItem toAdd, GameLocation store, int itemsBought);
        void TakeOwnershipOfVehicle(VehicleExt myNewCar);
        void DisplayPlayerNotification();
        void SetReputation(Gang myGang, int item2);
        void ClearVehicleOwnership();
    }
}
