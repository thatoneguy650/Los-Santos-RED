using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

public static class SettingsManager
{
    private static readonly string ConfigFileName = "Plugins\\LosSantosRED\\Settings.xml";
    public static LSRSettings MySettings { get; private set; }
    public static void Initialize()
    {
        if (File.Exists(ConfigFileName))
        {
            MySettings = DeserializeParams<LSRSettings>(ConfigFileName).FirstOrDefault();
        }
        else
        {
            MySettings = new LSRSettings();
            List<LSRSettings> ToSerialize = new List<LSRSettings>
            {
                MySettings
            };
            SerializeParams(ToSerialize, ConfigFileName);
        }
    }
    public static void SerializeAllSettings()
    {
        if (MySettings == null)
            MySettings = new LSRSettings();
        List<LSRSettings> ToSerialize = new List<LSRSettings>
        {
            MySettings
        };
        SerializeParams(ToSerialize, ConfigFileName);
    }
    public static void SerializeParams<T>(List<T> paramList, string FileName)
    {
        XDocument doc = new XDocument();
        XmlSerializer serializer = new XmlSerializer(paramList.GetType());
        XmlWriter writer = doc.CreateWriter();
        serializer.Serialize(writer, paramList);
        writer.Close();
        File.WriteAllText(FileName, doc.ToString());
        Debugging.WriteToLog("Settings ReadConfig", string.Format("Using Default Data {0}", FileName));
    }
    public static List<T> DeserializeParams<T>(string FileName)
    {
        XDocument doc = new XDocument(XDocument.Load(FileName));
        XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
        XmlReader reader = doc.CreateReader();
        List<T> result = (List<T>)serializer.Deserialize(reader);
        reader.Close();
        Debugging.WriteToLog("Settings ReadConfig", string.Format("Using Saved Data {0}", FileName));
        return result;
    }

}
