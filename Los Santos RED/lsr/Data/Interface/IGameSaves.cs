using LosSantosRED.lsr.Data;
using Rage;
using System.Collections.Generic;

namespace LosSantosRED.lsr.Interface
{
    public interface IGameSaves
    {
        List<GameSave> GameSaveList { get; }
        void Load(GameSave gameSave, IWeapons weapons);

        void Save(ISaveable player, IWeapons weapons);
    }
}