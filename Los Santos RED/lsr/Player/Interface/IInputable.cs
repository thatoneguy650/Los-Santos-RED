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
    public interface IInputable
    {
        ButtonPrompts ButtonPrompts { get; }
        CellPhone CellPhone { get; }
        WeaponEquipment WeaponEquipment { get; }
        Sprinting Sprinting { get; }
        Stance Stance { get; }  
        SurrenderActivity Surrendering { get; }
       // bool CanCancelCurrentActivity { get; }
      //  bool CanConverseWithLookedAtPed { get; }
      //  bool CanHoldUpTargettedPed { get; }
      //  bool CanPauseCurrentActivity { get; }
      //  bool CanPerformActivities { get; }

        Ped Character { get; }
        VehicleExt CurrentVehicle { get; }
        bool CurrentVehicleIsInAir { get; }
        bool CurrentVehicleIsRolledOver { get; }

        bool IsAiming { get; }
        bool IsAliveAndFree { get; }
        bool IsBusted { get; }
      //  bool IsConversing { get; }
        bool IsCustomizingPed { get; }
        bool IsDisplayingCustomMenus { get; }
       // bool IsInteracting { get; }
        bool IsInVehicle { get; }
        bool IsMale { get; }
        bool IsMoveControlPressed { get; set; }
        bool IsMoving { get; }
        bool IsNotHoldingEnter { get; set; }
        bool IsNotWanted { get; }
      //  bool IsPerformingActivity { get; set; }
        bool IsPressingFireWeapon { get; set; }
        bool IsWanted { get; }
        bool ReleasedFireWeapon { get; set; }
        int WantedLevel { get; }
        GroupManager GroupManager { get; }
        ActivityManager ActivityManager { get; }
        bool IsUsingController { get; set; }
        VehicleExt CurrentLookedAtVehicle { get; }
        bool IsAlive { get; }
        bool IsSetDisabledControls { get; set; }
        CuffManager CuffManager { get; }
        IntimidationManager IntimidationManager { get; }

        //   void CancelCurrentActivity();
        //  void CloseDriverDoor();
        //void ContinueCurrentActivity();
        // void DragPed();
        //  void Gesture();
        // void GrabPed();
        //  void LootPed();
        // void PauseCurrentActivity();
        void PlaySpeech(string player, bool v);
        void ShowVehicleInteractMenu(bool showDefault);
        //   void StartConversation();
        //   void StartLocationInteraction();
        //   void StartScenario();
        //   void StartSimpleCellphoneActivity();
        //  void StartTransaction();
        //    void StartSittingDown(bool v1, bool v2);
    }
}
