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
        bool IsNotHoldingEnter { get; set; }
        bool IsBusted { get; }
        bool CanSurrender { get; }
        bool HandsAreUp { get; set; }
        bool CanDropWeapon { get; }
        VehicleExt CurrentVehicle { get; }
        bool IsInVehicle { get; }
        bool IsMoveControlPressed { get; set; }
        bool CanConverseWithLookedAtPed { get; }
        bool CanHoldUpTargettedPed { get; }
        bool IsConversing { get; }
        List<ButtonPrompt> ButtonPromptList { get; }
        bool IsInteracting { get; }
        bool IsAiming { get; }
        bool IsMoving { get; }
        bool IsPerformingActivity { get; set; }
        Ped Character { get; }
        Cop AliasedCop { get; }
        bool IsDisplayingCustomMenus { get; }
        bool IsCustomizingPed { get; }
        bool IsPressingFireWeapon { get; set; }
        bool ReleasedFireWeapon { get; set; }
        Sprinting Sprinting { get; }
        bool IsMale { get; }
        bool IsWavingHands { get; set; }
        bool IsNotWanted { get; }
        bool IsWanted { get; }
        bool CanWaveHands { get; }
        bool CanPerformActivities { get; }
        bool CurrentVehicleIsRolledOver { get; }
        bool CurrentVehicleIsInAir { get; }

        void RaiseHands();
        void LowerHands();
        void DropWeapon();
        void SetUnarmed();
        void StartConversation();
        void StartScenario();
        void StartTransaction();
        

        void CloseDriverDoor();

        void ToggleSelector();
        void StartLocationInteraction();
        void Gesture();
        void WaveHands();
        void PlaySpeech(string player, bool v);
        void PauseDynamicActivity();
        void LootPed();
        void GrabPed();
        void Crouch();
        void StartSimpleCellphoneActivity();
    }
}
