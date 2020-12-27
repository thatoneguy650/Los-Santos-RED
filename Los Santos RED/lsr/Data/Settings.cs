using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

public class Settings : ISettings
{
    internal readonly object General;
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Settings.xml";
    public SettingsManager SettingsManager { get; private set; }
    public void ReadConfig()
    {
        if (File.Exists(ConfigFileName))
        {
            SettingsManager = Serialization.DeserializeParams<SettingsManager>(ConfigFileName).FirstOrDefault();
        }
        else
        {
            SettingsManager = new SettingsManager();
            List<SettingsManager> ToSerialize = new List<SettingsManager>
            {
                SettingsManager
            };
            Serialization.SerializeParams(ToSerialize, ConfigFileName);
        }
    }
    public void SerializeAllSettings()
    {
        if (SettingsManager == null)
            SettingsManager = new SettingsManager();
        List<SettingsManager> ToSerialize = new List<SettingsManager>
        {
            SettingsManager
        };
        Serialization.SerializeParams(ToSerialize, ConfigFileName);
    }
}
