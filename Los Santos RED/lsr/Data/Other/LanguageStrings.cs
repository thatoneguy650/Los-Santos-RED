using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class LanguageStrings 
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\LanguageStrings.xml";
    public HashSet<LanguageString> LanguageStringList { get; set; } = new HashSet<LanguageString>();
    public LanguageStrings()
    {
    }


    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("LanguageStrings*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded LanguageStrings config: {ConfigFile.FullName}", 0);
            LanguageStringList = Serialization.DeserializeHashSetParams<LanguageString>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded LanguageStrings config  {ConfigFileName}", 0);
            LanguageStringList = Serialization.DeserializeHashSetParams<LanguageString>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No LanguageStrings config found, creating default", 0);
            DefaultConfig();
        }
    }
   public void DefaultConfig()
    {
        LanguageStringList = new HashSet<LanguageString>();
        LanguageStringList.Add(new LanguageString("bribefailed", "Thats it? ~r~${0}~s~?"));
        Serialization.SerializeHashSetParams(LanguageStringList, ConfigFileName);
    }
    public string GetString(string ID)
    {
        return LanguageStringList.FirstOrDefault(x => x.ID == ID)?.Text;
    }
}

