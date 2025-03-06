using LosSantosRED.lsr.Data;
using Rage;
using System.Collections.Generic;

namespace LosSantosRED.lsr.Interface
{
    public interface IGameConfigs
    {
        List<GameConfig> SuffixConfigList { get; }
        List<GameConfig> CustomConfigList { get; }

        bool AreFilesAvailable(GameConfig config);
        void Load(GameConfig config);
        void SerializeAllSettings();
    }
}