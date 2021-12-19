using LosSantosRED.lsr.Locations;
using LSR.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IActionable
    {
        bool IsDead { get; }
        bool IsBusted { get; }
        bool IsInVehicle { get; }
        bool IsPerformingActivity { get; }
        bool CanPerformActivities { get; }
        List<LicensePlate> SpareLicensePlates { get; }
        List<InventoryItem> ConsumableItems { get; }
        bool IsCop { get; }
        LocationData CurrentLocation { get; }
        List<InventoryItem> InventoryItems { get; }
        void StartSmokingPot();
        void StartSittingDown();
        void StartSmoking();
        void StartDrinkingActivity();
        void CommitSuicide();
        void DisplayPlayerNotification();
        void GiveMoney(int v);
        void RemovePlate();
        void ChangePlate(int Index);
        void StopDynamicActivity();
        void ChangePlate(LicensePlate selectedItem);
        void TakeOwnershipOfNearestCar();
        void CallPolice();
        //void StartEatingActivity(ConsumableSubstance selectedStuff);
        bool RemoveFromInventory(ModItem selectedStuff, int v);
        void StartConsumingActivity(ModItem selectedStuff);
        void PauseDynamicActivity();
        void ContinueDynamicActivity();
    }
}
