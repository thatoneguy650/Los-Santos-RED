using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using System.IO;

public class Settings : ISettingsProvideable
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Settings.xml";
    public SettingsManager SettingsManager { get; private set; }
    public void ReadConfig()
    {
        if (File.Exists(ConfigFileName))
        {
            SettingsManager = Serialization.DeserializeParam<SettingsManager>(ConfigFileName);
        }
        else
        {
            SettingsManager = new SettingsManager();
            Serialization.SerializeParam(SettingsManager, ConfigFileName);
        }
    }
    public void SerializeAllSettings()
    {
        Serialization.SerializeParam(SettingsManager == null ? new SettingsManager() : SettingsManager, ConfigFileName);
    }
}