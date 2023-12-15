using LosSantosRED.lsr.Data;
using Rage;
using System.Collections.Generic;

namespace LosSantosRED.lsr.Interface
{
    public interface IGameSaves
    {
        List<GameSave> GameSaveList { get; }
        int NextSaveGameNumber { get; }

        //void SaveSamePlayer_Obsolete(ISaveable player, IWeapons weapons, ITimeReportable time, IPlacesOfInterest placesOfInterest, IModItems modItems);
        //void DeleteSave_Obsolete(string playerName, string modelName);
        void Load(GameSave selectedItem, IWeapons weapons, IPedSwap pedSwap, IInventoryable playerInvetory, ISettingsProvideable settings, IEntityProvideable world, IGangs gangs, ITimeControllable time, IPlacesOfInterest placesOfInterest,
            IModItems modItems, IAgencies agencies, IContacts contacts, IInteractionable interactionable);
        //GameSave GetSave_Obsolete(ISaveable player);

        void DeleteSave(GameSave gs);
        void Save(ISaveable saveable, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest, IModItems modItems, int saveNumber);
        void DeleteSave();
        bool IsPlaying(GameSave gs);
        void OnChangedPlayer();
    }
}