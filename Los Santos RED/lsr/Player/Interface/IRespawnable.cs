using LosSantosRED.lsr.Locations;
using LosSantosRED.lsr.Player;
using Rage;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IRespawnable : ICameraControllable
    {
        BankAccounts BankAccounts { get; }
        bool IsBusted { get; }
        PoliceResponse PoliceResponse { get; }
        Inventory Inventory { get; }
        CellPhone CellPhone { get; }
        Licenses Licenses { get; }
        PlayerTasks PlayerTasks { get; }
        Respawning Respawning { get; }
        SurrenderActivity Surrendering { get; }
        int WantedLevel { get; }
        bool DiedInVehicle { get; }
        string PlayerName { get; }
        string ModelName { get; }
        Scanner Scanner { get; }
        Vehicle LastFriendlyVehicle { get; set; }
        int SpeechSkill { get; }
        ButtonPrompts ButtonPrompts { get; }
        Ped Character { get; }
        HumanState HumanState { get; }
        bool IsMoveControlPressed { get; }
        bool IsAliveAndFree { get; }
        bool IsIncapacitated { get; }
        bool IsInVehicle { get; }
        bool IsAlive { get; }
        bool IsBeingBooked { get; set; }
        bool IsArrested { get; set; }
        LocationData CurrentLocation { get; }
        WeaponEquipment WeaponEquipment { get; }
        Violations Violations { get; }
        bool IsWanted { get; }
        VehicleOwnership VehicleOwnership { get; }
        PedExt PedLastKilledPlayer { get; }
        string Gender { get; }
        Dispatcher Dispatcher { get; }
        CuffManager CuffManager { get; }

        void Reset(bool resetWanted, bool resetTimesDied, bool clearWeapons, bool clearCriminalHistory, bool clearInventory, bool clearIntoxication, bool resetGangRelationships, bool clearVehicleOwnership, 
            bool resetCellphone, bool clearActiveTasks, bool clearProperties, bool resetHealth, bool resetNeeds, bool resetGroup, bool resetLicenses, bool resetACtivites, bool resetGracePeriod, bool resetBankAccounts, bool resetSavedGame, 
            bool resetPendingMessages, bool resetInteriors, bool resetGambling, bool resetPersistVehicles);
        void SetWantedLevel(int maxWantedLastLife, string v1, bool v2);
        int FineAmount();
        void SetNotBusted();
        void PlaySpeech(string v1, bool v2);
    }
}
