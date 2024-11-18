using LosSantosRED.lsr.Player;
using LSR.Vehicles;
using Rage;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface ILocationInteractable : IInteractionable //: IActivityPerformable,IActionable
    {
        //ButtonPrompts ButtonPrompts { get; }
        //BankAccounts BankAccounts { get; }
        //Inventory Inventory { get; }
        Properties Properties { get; }
        //ActivityManager ActivityManager { get; }
        //Ped Character { get; }
        //bool CharacterModelIsFreeMode { get; }
        //string FreeModeVoice { get; }
        Vector3 Position { get; }
        //bool IsTransacting { get; set; }
        //bool IsMoveControlPressed { get; }
        bool IsDoingSuspiciousActivity { get; set; }
        //WeaponEquipment WeaponEquipment { get; }
        //VehicleOwnership VehicleOwnership { get; }
        //RelationshipManager RelationshipManager { get; }
        Licenses Licenses { get; }
        string PlayerName { get; }
        //bool IsAliveAndFree { get; }
        //bool IsInVehicle { get; }
        float VehicleSpeedMPH { get; }
        bool RecentlyCrashedVehicle { get; }
        Vehicle LastFriendlyVehicle { get; set; }
        VehicleExt CurrentVehicle { get; }
        bool IsResting { get; set; }
        bool IsSleeping { get; set; }
        //bool IsDealingDrugs { get; set; }
        //bool IsDealingIllegalGuns { get; set; }
        bool IsNotWanted { get; }
        bool IsWanted { get; }
        PlayerTasks PlayerTasks { get; }
        string Gender { get; }
        HealthManager HealthManager { get; }
        Rage.Object CurrentLookedAtObject { get; }
        float ClosestPoliceDistanceToPlayer { get; }
        bool AnyPoliceRecentlySeenPlayer { get; }
        PedVariation CurrentModelVariation { get; set; }
        OutfitManager OutfitManager { get; }
        bool IsCop { get; }
        Respawning Respawning { get; }
        Violations Violations { get; }
        bool IsFireFighter { get; }
        Investigation Investigation { get; }
        PedSwap PedSwap { get; }
        bool CharacterModelIsPrimaryCharacter { get; }
        bool IsMale { get; }
        string ModelName { get; }

        void ChangeName(string newName);

        void SetWantedLevel(int v1, string v2, bool v3);
        void OnInteractionMenuCreated(GameLocation gameLocation, MenuPool menuPool, UIMenu interactionMenu);

        ICasinoGamePlayable CasinoGamePlayer { get; }
    }
}
