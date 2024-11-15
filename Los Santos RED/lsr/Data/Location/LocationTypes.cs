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
using static System.Windows.Forms.AxHost;

public class LocationTypes : ILocationTypes
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\LocationTypes.xml";
    public LocationTypeManager LocationTypeNames { get; private set; }
    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("LocationTypes*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded LocationTypes config: {ConfigFile.FullName}", 0);
            LocationTypeNames = Serialization.DeserializeParam<LocationTypeManager>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded LocationTypes config  {ConfigFileName}", 0);
            LocationTypeNames = Serialization.DeserializeParam<LocationTypeManager>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No LocationTypes config found, creating default", 0);
            DefaultConfig();
        }
    }
    public GameCounty GetCounty(string countyID)
    {
        if (string.IsNullOrEmpty(countyID))
        {
            return null;
        }
        return LocationTypeNames.CountyList.Where(x => x.CountyID.ToLower() == countyID.ToLower()).FirstOrDefault();
    }
    public GameState GetState(string stateID)
    {
        if(string.IsNullOrEmpty(stateID))
        {
            return null;
        }
        return LocationTypeNames.StateList.Where(x => x.StateID.ToLower() == stateID.ToLower()).FirstOrDefault();
    }
    private void DefaultConfig()
    {
        LocationTypeNames = new LocationTypeManager();
        LocationTypeNames.CountyList = new List<GameCounty>
        {
            new GameCounty(StaticStrings.CityOfLosSantosCountyID, "City of Los Santos","LS"),
            new GameCounty(StaticStrings.LosSantosCountyID, "Los Santos County"),
            new GameCounty(StaticStrings.BlaineCountyID, "Blaine County","BC"),
            new GameCounty(StaticStrings.PacificOceanCountyID, "Pacific Ocean"),       
            new GameCounty(StaticStrings.CrookCountyID, "Crook County") { ColorPrefix = "~b~" },
            new GameCounty(StaticStrings.NorthYanktonCountyID, "North Yankton"),      
            new GameCounty(StaticStrings.VenturaCountyID, "Ventura County"),
            new GameCounty(StaticStrings.MajesticCountyID, "Majestic County"),
            new GameCounty(StaticStrings.LibertyCityCountyID, "Liberty City","LC"),
            new GameCounty(StaticStrings.AlderneyCountyID, "Alderney"),
            new GameCounty(StaticStrings.CityOfViceCountyID, "City of Vice","VC"),
            new GameCounty(StaticStrings.ViceCountyID, "Vice County"),

            new GameCounty(StaticStrings.ColombiaCountyID, "Columbia"),

            new GameCounty("Unknown", "Unknown"),
        };

        LocationTypeNames.StateList = new List<GameState>
        {
            new GameState(StaticStrings.SanAndreasStateID, "San Andreas"),
            new GameState(StaticStrings.NorthYanktonStateID, "North Yankton"),
            new GameState(StaticStrings.LibertyStateID, "Liberty") { SisterStateIDs = new List<string>() { StaticStrings.AlderneyStateID } },
            new GameState(StaticStrings.AlderneyStateID, "Alderney") { SisterStateIDs = new List<string>() { StaticStrings.LibertyStateID } },
            new GameState(StaticStrings.LeonidaStateID, "Leonida"),
            new GameState(StaticStrings.ColombiaStateID, "Colombia") { ColorPrefix = "~p~" },
            new GameState("Unknown", "Unknown"),
        };

        Serialization.SerializeParam(LocationTypeNames, ConfigFileName);
    }
}
