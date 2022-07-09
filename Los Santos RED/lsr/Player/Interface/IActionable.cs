using LosSantosRED.lsr.Locations;
using LosSantosRED.lsr.Player;
using LSR.Vehicles;
using Rage;
using Rage.Native;
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
        bool IsPerformingActivity { get; set; }
        bool CanPerformActivities { get; }
        DanceData LastDance { get; set; }
        List<LicensePlate> SpareLicensePlates { get; }
        bool IsCop { get; }
        LocationData CurrentLocation { get; }
        Ped Character { get; }
        bool IsMale { get; }
        bool IsSitting { get; set; }

        bool IsLayingDown { get; set; }

        Rage.Object AttachedProp { get; }
        bool IsMoveControlPressed { get; }
        string ModelName { get; }
        bool HasCurrentActivity { get; }
        bool CharacterModelIsFreeMode { get; }
        GestureData LastGesture { get; set; }
        bool IsMakingInsultingGesture { get; set; }
        void StartSittingDown(bool FindSittingProp, bool EnterForward);
        void CommitSuicide();
        void DisplayPlayerNotification();
        void GiveMoney(int v);
        void RemovePlate();
        void ChangePlate(int Index);
        void StopDynamicActivity();
        void ChangePlate(LicensePlate selectedItem);
        void TakeOwnershipOfNearestCar();
        void CallPolice();
        void SetUnarmed();
        void StartConsumingActivity(ModItem selectedStuff, bool performActivity);
        void PauseCurrentActivity();
        void ContinueCurrentActivity();
        void Gesture(GestureData gestureData);
        void EnterVehicleAsPassenger();
        void ShuffleToNextSeat();
        void ForceErraticDriver();
        void ToggleBodyArmor(int Type);
        void SetBodyArmor(int value);
        void StartLayingDown(bool v);
        void StartHotwiring();
        GangRelationships GangRelationships { get; }
        Inventory Inventory { get; }
        Intoxication Intoxication { get; }
        bool IsDancing { get; set; }
        VehicleExt CurrentVehicle { get; }
        ButtonPrompts ButtonPrompts { get; }
        CellPhone CellPhone { get; }
        Investigation Investigation { get; }
        bool IsNotWanted { get; }
        PoliceResponse PoliceResponse { get; }
        WeaponInformation CurrentWeapon { get; }
        bool IsStill { get; }
        bool IsResting { get; set; }
        bool IsSleeping { get; set; }
        HumanState HumanState { get; }


        //Action DropWeapon { get; }

        void RemoveOwnershipOfNearestCar();
        void SetMoney(int bribeAmount);
        void Gesture();
        void ToggleActionMode();
        void ToggleStealthMode();
        void ToggleSelector();
        void Crouch();
        void Dance(DanceData selectedItem);
        void Dance();
        void ToggleSurrender();
        void DropWeapon();
        void CloseDriverDoor();
        void ToggleRightIndicator();
        void ToggleLeftIndicator();
        void ToggleHazards();
        void ToggleVehicleEngine();
        void ToggleDriverWindow();
        void SetWantedLevel(int selectedItem, string v1, bool v2);
    }
}
