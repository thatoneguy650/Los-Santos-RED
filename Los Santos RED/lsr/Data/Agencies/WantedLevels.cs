using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class WantedLevels : IWantedLevels
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\WantedLevels.xml";
    private List<WantedLevel> WantedLevelList;
    private WantedLevel wantedLevelOne;
    private WantedLevel wantedLevelTwo;
    private WantedLevel wantedLevelThree;
    private WantedLevel wantedLevelFour;
    private WantedLevel wantedLevelFive;
    private WantedLevel wantedLevelSix;

    public WantedLevels()
    {

    }
    public void ReadConfig()
    {
        DirectoryInfo taskDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo taskFile = taskDirectory.GetFiles("WantedLevels*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (taskFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded WantedLevels Config: {taskFile.FullName}", 0);
            WantedLevelList = Serialization.DeserializeParams<WantedLevel>(taskFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded WantedLevels Config  {ConfigFileName}", 0);
            WantedLevelList = Serialization.DeserializeParams<WantedLevel>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No WantedLevel config found, creating default", 0);
            SetupDefault();
            DefaultConfig();
        }
    }
    private void SetupDefault()
    {
        wantedLevelOne = new WantedLevel(1);
        wantedLevelTwo = new WantedLevel(2);
        wantedLevelThree = new WantedLevel(3);
        wantedLevelFour = new WantedLevel(4);
        wantedLevelFive = new WantedLevel(5);
        wantedLevelSix = new WantedLevel(6);
    }
    private void DefaultConfig()
    {
        WantedLevelList = new List<WantedLevel>
        {
            wantedLevelOne, wantedLevelTwo, wantedLevelThree, wantedLevelFour, wantedLevelFive, wantedLevelSix
        };
        Serialization.SerializeParams(WantedLevelList, ConfigFileName);
    }   
    public void Setup(IHeads heads, IDispatchableVehicles dispatchableVehicles, IDispatchablePeople dispatchablePeople, IIssuableWeapons issuableWeapons)
    {

    }

}