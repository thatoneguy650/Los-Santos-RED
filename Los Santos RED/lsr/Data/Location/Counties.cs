using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

public class Counties : ICounties
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Counties.xml";
    public List<GameCounty> CountyList { get; private set; } = new List<GameCounty>();
    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("Counties*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Counties config: {ConfigFile.FullName}", 0);
            CountyList = Serialization.DeserializeParams<GameCounty>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Counties config  {ConfigFileName}", 0);
            CountyList = Serialization.DeserializeParams<GameCounty>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Counties config found, creating default", 0);
            DefaultConfig();
        }
    }
    public GameCounty GetCounty(string countyID)
    {
        return CountyList.Where(x => x.CountyID.ToLower() == countyID.ToLower()).FirstOrDefault();
    }
    private void DefaultConfig()
    {
        CountyList = new List<GameCounty>
        {
            new GameCounty("CityOfLosSantos", "City of Los Santos"),
            new GameCounty("LosSantosCounty", "Los Santos County"),
            new GameCounty("BlaineCounty", "Blaine County"),
            new GameCounty("PacificOcean", "Pacific Ocean"),
            new GameCounty("Unknown", "Unknown"),
            new GameCounty("Crook", "Crook County"),
            new GameCounty("NorthYankton", "North Yankton"),
            new GameCounty("Vice", "City of Vice"),
            new GameCounty("VenturaCounty", "Ventura County"),
            new GameCounty("MajesticCounty", "Majestic County"),
            new GameCounty("LibertyCity", "Liberty City"),
            new GameCounty("Alderney", "Alderney"),

            };
        Serialization.SerializeParams(CountyList, ConfigFileName);
    }
}
