using LosSantosRED.lsr.Data;
using Rage;
using System.Collections.Generic;

namespace LosSantosRED.lsr.Interface
{
    public interface IGameSaves
    {
        List<GameSave> GameSaveList { get; }


        void SaveSamePlayer(ISaveable player, IWeapons weapons, ITimeReportable time, IPlacesOfInterest placesOfInterest, IModItems modItems);
        void DeleteSave(string playerName, string modelName);
        void Load(GameSave selectedItem, IWeapons weapons, IPedSwap pedSwap, IInventoryable playerInvetory, ISettingsProvideable settings, IEntityProvideable world, IGangs gangs, ITimeControllable time, IPlacesOfInterest placesOfInterest, IModItems modItems);
        GameSave GetSave(ISaveable player);
        void UpdateSave(GameSave mySave);
        void DeleteSave(GameSave gs);
        void Save(ISaveable saveable, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest, IModItems modItems);
    }
}