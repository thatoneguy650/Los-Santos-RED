using LosSantosRED.lsr.Locations;
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
    public interface IDisplayable
    {
       // Street CurrentCrossStreet { get; }
        VehicleExt CurrentSeenVehicle { get; }
       // Street CurrentStreet { get; }
        VehicleExt CurrentVehicle { get; }
        //Zone CurrentZone { get; }



        LocationData CurrentLocation { get; set; }
        string DebugLine2 { get; }
        string DebugLine7 { get; }
        string DebugLine4 { get; set; }
        string DebugLine5 { get; }
        string DebugLine6 { get; }
        string DebugLine3 { get; }
        string DebugLine1 { get; }
        bool IsAliveAndFree { get; }
        bool IsDead { get; }
        bool IsBusted { get; }
        bool IsSpeeding { get; }
      //  bool IsViolatingAnyTrafficLaws { get; }
        bool IsConversing { get; }
        bool IsPerformingActivity { get; }
        bool CanConverseWithLookedAtPed { get; }
        List<ButtonPrompt> ButtonPromptList { get; }
        string DebugLine8 { get; }
        string DebugLine9 { get; }
        float VehicleSpeedMPH { get; }
        float VehicleSpeedKMH { get; }
        Investigation Investigation { get; }
        bool HasCriminalHistory { get; }
        //string LawsViolating { get; }
        int CriminalHistoryMaxWantedLevel { get; }
        bool HasDeadlyCriminalHistory { get; }
        bool IsWanted { get; }
        bool IsInSearchMode { get; }
        PoliceResponse PoliceResponse { get; }
        bool IsInVehicle { get; }
        bool IsNotWanted { get; }
        //bool IsSprinting { get; }
       // float StaminaPercent { get; }
        float IntoxicatedIntensityPercent { get; }
        //GameLocation CurrentShop { get; }
        WeaponInformation CurrentWeapon { get; }
        SelectorOptions CurrentSelectorSetting { get; }
        short CurrentWeaponMagazineSize { get; }
        Violations Violations { get; }
        int CellX { get; }
        int CellY { get; }
        Ped Character { get; }
        Sprinting Sprinting { get; }
        bool IsTransacting { get; }
        bool RecentlyChangedMoney { get; }
        CellPhone CellPhone { get; }
        bool CharacterModelIsPrimaryCharacter { get; }
        int Money { get; }
        HumanState HumanState { get; set; }
        int LastChangeMoneyAmount { get; }
    }
}
