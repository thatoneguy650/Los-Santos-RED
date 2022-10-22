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
        SurrenderActivity Surrendering { get; }
        VehicleOwnership VehicleOwnership { get; }
        BankAccounts BankAccounts { get; }
        HumanState HumanState { get; }
        Stance Stance { get; }
        WeaponEquipment WeaponEquipment { get; }
        ButtonPrompts ButtonPrompts { get; }
        CellPhone CellPhone { get; }
        Investigation Investigation { get; }
        RelationshipManager RelationshipManager { get; }
        Inventory Inventory { get; }
        Intoxication Intoxication { get; }
        PoliceResponse PoliceResponse { get; }
        ActivityManager ActivityManager { get; }
        LocationData CurrentLocation { get; }
        bool IsDead { get; }
        bool IsBusted { get; }
        bool IsInVehicle { get; }
       // bool IsPerformingActivity { get; set; }
       // bool CanPerformActivities { get; }
       // DanceData LastDance { get; set; }
        List<LicensePlate> SpareLicensePlates { get; }
        bool IsCop { get; }
        Ped Character { get; }
        bool IsMale { get; }
      //  bool IsSitting { get; set; }
      //  bool IsLayingDown { get; set; }
        Rage.Object AttachedProp { get; set; }
        bool IsMoveControlPressed { get; }
        string ModelName { get; }
     //   bool HasCurrentActivity { get; }
        bool CharacterModelIsFreeMode { get; }
      //  GestureData LastGesture { get; set; }
        bool IsMakingInsultingGesture { get; set; }
      //  bool IsDancing { get; set; }
        VehicleExt CurrentVehicle { get; }
        bool IsNotWanted { get; }
        bool IsStill { get; }
        bool IsResting { get; set; }
        bool IsSleeping { get; set; }
        VehicleExt CurrentLookedAtVehicle { get; }
        HealthManager HealthManager { get; }
        GroupManager GroupManager { get; }
        Scanner Scanner { get; }
        Licenses Licenses { get; }
        Rage.Object CurrentLookedAtObject { get; }
        bool IsGangMember { get; }
        uint Handle { get; }
        bool IsVisiblyArmed { get; }

        //bool IsCommitingSuicide { get; set; }

        // void Gesture();
        //   void Dance(DanceData selectedItem);
        //  void Dance();
        //    void CloseDriverDoor();
        //  void StartSittingDown(bool FindSittingProp, bool EnterForward);
        //  void CommitSuicide();
        void DisplayPlayerNotification();
     //   void RemovePlate();
     //   void ChangePlate(int Index);
     //   void StopDynamicActivity();
     //   void ChangePlate(LicensePlate selectedItem);
    //    void StartConsumingActivity(ModItem selectedStuff, bool performActivity);
    //    void PauseCurrentActivity();
     //   void ContinueCurrentActivity();
    //    void Gesture(GestureData gestureData);
        //void EnterVehicleAsPassenger(bool withBlocking);
    //    void ShuffleToNextSeat();
     //   void ForceErraticDriver();
        void ToggleBodyArmor(int Type);
        void SetBodyArmor(int value);
     //   void StartSleeping(bool v);
       // void StartHotwiring();
       // void ToggleRightIndicator();
      //  void ToggleLeftIndicator();
     //   void ToggleHazards();
     //   void ToggleVehicleEngine();
     //   void ToggleDriverWindow();
        void SetWantedLevel(int selectedItem, string v1, bool v2);
        void Arrest();
        void Reset(bool v1, bool v2, bool v3, bool v4, bool v5, bool v6, bool v7, bool v8, bool v9, bool v10, bool v11, bool v12, bool v13, bool v14, bool v15);
        void ToggleCopTaskable();
    }
}
