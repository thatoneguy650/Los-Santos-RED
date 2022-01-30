using LosSantosRED.lsr.Data;
using Rage;
using System.Collections.Generic;

namespace LosSantosRED.lsr.Interface
{
    public interface IGameSaves
    {
        List<GameSave> GameSaveList { get; }


        void Save(ISaveable player, IWeapons weapons, ITimeReportable time);
        void DeleteSave(string playerName, string modelName);
        void Load(GameSave selectedItem, IWeapons weapons, IPedSwap pedSwap, IInventoryable playerInvetory, ISettingsProvideable settings, IEntityProvideable world, IGangs gangs, ITimeControllable time);
    }
}