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
    public interface IPedSwappable
    {
        WeaponEquipment WeaponEquipment { get; }
        Inventory Inventory { get; set; }
        VehicleOwnership VehicleOwnership { get; }
        ButtonPrompts ButtonPrompts { get; }
        CriminalHistory CriminalHistory { get; }
        BankAccounts BankAccounts { get; }
        Ped Character { get; }
        bool CharacterModelIsFreeMode { get; }
        PedVariation CurrentModelVariation { get; set; }
        VehicleExt CurrentVehicle { get; }
        int GroupID { get; set; }
        bool IsBusted { get; }
        bool IsCop { get; set; }
        bool IsCustomizingPed { get; set; }
        bool IsMoveControlPressed { get; }
        bool IsWanted { get; }
        string ModelName { get; set; }
        string PlayerName { get; set; }
        Vector3 Position { get; }
        int WantedLevel { get; }
        RelationshipManager RelationshipManager { get; }
        Licenses Licenses { get; }
        HumanState HumanState { get; }
        Agency AssignedAgency { get; }
        string FreeModeVoice { get; }
        bool CharacterModelIsPrimaryCharacter { get; }
        CellPhone CellPhone { get; }
        Gang CurrentGang { get; }
        LocationData CurrentLocation { get; }
        InteriorManager InteriorManager { get; }

        void DisplayPlayerNotification();
        void Reset(bool v1, bool v2, bool v3, bool v4, bool v5, bool v6, bool v7, bool v8, bool v9, bool v10, bool v11, bool v12, bool resetNeeds, bool resetGroup, bool resetLicenses, bool resetACtivites, bool resetGracePeriod, bool resetBankAccounts, 
            bool resetSavedGame, bool resetPendingMessages, bool resetInteriors, bool resetGambling, bool resetPersistVehicles);
        void SetDemographics(string modelName, bool isMale, string v1, int v2, int speechSkill, string voiceName);
        void SetWantedLevel(int v1, string v2, bool v3);
        void UpdateVehicleData();
        void RemoveAgencyStatus();
        void SetAgencyStatus(Agency assignedAgency);
        void SetVoice(string voiceName);
    }
}
