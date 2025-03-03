using LosSantosRED.lsr.Data;
using Rage;
using System.Collections.Generic;

namespace LosSantosRED.lsr.Interface
{
    public interface IGameConfigs
    {
        List<GameConfig> GameConfigList { get; }
        void Load(GameConfig config);
    }
}