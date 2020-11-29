using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class SettingsManager
{
    private static readonly string ConfigFileName = "Plugins\\LosSantosRED\\Settings.xml";
    public static LSRSettings MySettings { get; private set; }
    public static void Initialize()
    {
        ReadConfig();
    }
    public static void Dispose()
    {

    }
    public static void SerializeAllSettings()
    {
        if (MySettings == null)
            MySettings = new LSRSettings();
        List<LSRSettings> ToSerialize = new List<LSRSettings>
        {
            MySettings
        };
        General.SerializeParams(ToSerialize, ConfigFileName);
    }
    private static void ReadConfig()
    {
        if (File.Exists(ConfigFileName))
        {
            MySettings = General.DeserializeParams<LSRSettings>(ConfigFileName).FirstOrDefault();
        }
        else
        {
            MySettings = new LSRSettings();
            List<LSRSettings> ToSerialize = new List<LSRSettings>
            {
                MySettings
            };
            General.SerializeParams(ToSerialize, ConfigFileName);
        }
    }

}
