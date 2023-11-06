using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface ISettingsProvideable
    {
        SettingsManager SettingsManager { get; }
        //SettingsManager EasySettingsManager { get; }
        SettingsManager DefaultSettingsManager { get; }
        bool IsBackendChanged { get; set; }

        void SerializeAllSettings();
        void SetHard();
        void SetEasy();
        void SetDefault();
        void ReadConfig();
        void SetPreferred();
        void SetLC();
    }
}
