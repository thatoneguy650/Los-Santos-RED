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
        bool IsChangingLicensePlates { get; set; }
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
        bool IsCop { get; }
        Ped Character { get; }
        bool IsMale { get; }
        List<Rage.Object> AttachedProp { get; set; }
        bool IsMoveControlPressed { get; }
        string ModelName { get; }
        bool CharacterModelIsFreeMode { get; }
        bool IsMakingInsultingGesture { get; set; }
        VehicleExt CurrentVehicle { get; }
        bool IsNotWanted { get; }
        bool IsStill { get; }
        bool IsResting { get; set; }
        bool IsSleeping { get; set; }
        VehicleExt CurrentLookedAtVehicle { get; }
        HealthManager HealthManager { get; }
        ArmorManager ArmorManager { get; }
        GroupManager GroupManager { get; }
        Scanner Scanner { get; }
        Licenses Licenses { get; }
        Rage.Object CurrentLookedAtObject { get; }
        bool IsGangMember { get; }
        uint Handle { get; }
        bool IsVisiblyArmed { get; }
        bool IsOnFoot { get; }
        string Gender { get; }
        bool IsDriver { get; }
        bool IsAliveAndFree { get; }
        bool IsIncapacitated { get; }
        bool IsSleepingOutside { get; set; }
        bool IsShowingActionWheel { get; }
        ClipsetManager ClipsetManager { get; }
        bool IsWanted { get; }
        PedVariation CurrentModelVariation { get; set; }
        OutfitManager OutfitManager { get; }
        Agency AssignedAgency { get; }
        Gang CurrentGang { get; }
        bool IsServicePed { get; }
        PedExt CurrentLookedAtPed { get; }
        VehicleExt InterestedVehicle { get; }
        GPSManager GPSManager { get; }
        TaxiManager TaxiManager { get; }
        InteriorManager InteriorManager { get; }
        CuffManager CuffManager { get; }
        Dispatcher Dispatcher { get; }
        RadarDetector RadarDetector { get; }
        PlayerVoice PlayerVoice { get; }

        void DisplayPlayerNotification();
        void SetWantedLevel(int selectedItem, string v1, bool v2);
        void Arrest();
        void Reset(bool v1, bool v2, bool v3, bool v4, bool v5, bool v6, bool v7, bool v8, bool v9, bool v10, bool v11, bool v12, bool v13, bool v14, bool v15, bool resetACtivites, bool resetGracePeriod, bool resetBankAccounts, bool resetSavedGame, 
            bool resetPendingMessages, bool resetInteriors, bool resetGambling, bool resetPersistVehicles);
        void ToggleCopTaskable();
        void ShowVehicleInteractMenu(bool showDefault);
        void PlaySpeech(string v1, bool v2);
        void ToggleAutoBackup();
    }
}
