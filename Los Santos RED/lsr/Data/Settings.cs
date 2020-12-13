using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
public class Settings
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Settings.xml";
    public LSRSettings MySettings { get; private set; }
    public void ReadConfig()
    {
        if (File.Exists(ConfigFileName))
        {
            MySettings = Serialization.DeserializeParams<LSRSettings>(ConfigFileName).FirstOrDefault();
        }
        else
        {
            MySettings = new LSRSettings();
            List<LSRSettings> ToSerialize = new List<LSRSettings>
            {
                MySettings
            };
            Serialization.SerializeParams(ToSerialize, ConfigFileName);
        }
    }
    public void SerializeAllSettings()
    {
        if (MySettings == null)
            MySettings = new LSRSettings();
        List<LSRSettings> ToSerialize = new List<LSRSettings>
        {
            MySettings
        };
        Serialization.SerializeParams(ToSerialize, ConfigFileName);
    }
}
