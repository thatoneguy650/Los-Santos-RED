using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PlateTypes : IPlateTypes
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\PlateTypes.xml";
    private List<PlateType> PlateTypeList = new List<PlateType>();
    public void ReadConfig()
    {

        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("PlateTypes*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Plate Types config: {ConfigFile.FullName}",0);
            PlateTypeList = Serialization.DeserializeParams<PlateType>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Plate Types config  {ConfigFileName}",0);
            PlateTypeList = Serialization.DeserializeParams<PlateType>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Plate Types config found, creating default", 0);
            DefaultConfig_Full();
            DefaultConfig();
        }
    }
    public PlateType GetPlateType(int CurrentIndex)
    {
        return PlateTypeList.FirstOrDefault(x => x.Index == CurrentIndex);
    }
    public PlateType GetPlateType(string State)
    {
        return PlateTypeList.Where(x => x.State == State).PickRandom();
    }
    public PlateType GetRandomPlateType()
    {
        if (!PlateTypeList.Any())
            return null;

        List<PlateType> ToPickFrom = PlateTypeList.Where(x => x.CanSpawn).ToList();
        int Total = ToPickFrom.Sum(x => x.SpawnChance);
        int RandomPick = RandomItems.MyRand.Next(0, Total);
        foreach (PlateType Type in ToPickFrom)
        {
            int SpawnChance = Type.SpawnChance;
            if (RandomPick < SpawnChance)
            {
                return Type;
            }
            RandomPick -= SpawnChance;
        }
        return null;
    }
    private void DefaultConfig()
    {
        PlateTypeList.Add(new PlateType(0, "Red on White California", "San Andreas", 0, "1ABC234"));
        PlateTypeList.Add(new PlateType(1, "Yellow on Black California", "San Andreas", 0, "1ABC234"));
        PlateTypeList.Add(new PlateType(2, "Yellow on Blue California", "San Andreas", 0, "1ABC234"));
        PlateTypeList.Add(new PlateType(3, "Classic California", "San Andreas", 0, "1ABC234"));
        PlateTypeList.Add(new PlateType(4, "Exempt California", "San Andreas", 0, "1ABC234") { CanOverwrite = false });
        Serialization.SerializeParams(PlateTypeList, ConfigFileName);
    }
    private void DefaultConfig_Full()
    {
        List<PlateType> FullPlateTypeList = new List<PlateType>();
        FullPlateTypeList.Add(new PlateType(0, "Red on White California", "San Andreas", 0, "1ABC234"));
        FullPlateTypeList.Add(new PlateType(1, "Yellow on Black California", "San Andreas", 0, "1ABC234"));
        FullPlateTypeList.Add(new PlateType(2, "Yellow on Blue California", "San Andreas", 0, "1ABC234"));
        FullPlateTypeList.Add(new PlateType(3, "Classic California", "San Andreas", 0, "1ABC234"));
        FullPlateTypeList.Add(new PlateType(4, "Exempt California", "San Andreas", 0, "1ABC234") { CanOverwrite = false });
        FullPlateTypeList.Add(new PlateType(5, "North Dakota Old", "North Yankton", 3, "123 ABC"));
        FullPlateTypeList.Add(new PlateType(6, "New York 1", "Liberty", 0, "ABC-1234"));
        FullPlateTypeList.Add(new PlateType(7, "Florida 1", "Vice City", 0, "123 4AB"));
        FullPlateTypeList.Add(new PlateType(8, "New York 2", "Liberty", 0, "ABC-1234"));
        FullPlateTypeList.Add(new PlateType(9, "New York 3", "Liberty", 0, "ABC-1234"));
        FullPlateTypeList.Add(new PlateType(10, "Sprunk Logo", "None", 0, "1ABC234"));
        FullPlateTypeList.Add(new PlateType(11, "Patriots?", "None", 0, "1ABC234"));
        FullPlateTypeList.Add(new PlateType(12, "Los Santos Shrimps", "San Andreas", 1, "1ABC234"));
        FullPlateTypeList.Add(new PlateType(13, "Nevada San Andreas Mashup", "San Andreas", 0, "1ABC234"));
        FullPlateTypeList.Add(new PlateType(14, "North Carolina", "North Volucrina", 3, "ABC-1234"));
        FullPlateTypeList.Add(new PlateType(15, "New Jersey", "Alderney", 3, "D12-ABC"));

        FullPlateTypeList.Add(new PlateType(16, "Nevada", "Robada", 10, "123-A45"));




        FullPlateTypeList.Add(new PlateType(17, "Illinois", "Lincoln", 3, "AB 12345"));
        FullPlateTypeList.Add(new PlateType(18, "Epsilon", "None", 3, "1ABC234"));
        FullPlateTypeList.Add(new PlateType(19, "Our Florida", "Miami", 3, "123 4AB"));
        FullPlateTypeList.Add(new PlateType(20, "Florida 1", "Miami", 0, "123 4AB"));
        FullPlateTypeList.Add(new PlateType(21, "Florida 2", "Miami", 0, "123 4AB"));

        FullPlateTypeList.Add(new PlateType(22, "Arizona", "Hareona", 12));


        FullPlateTypeList.Add(new PlateType(23, "North Dakota New", "North Yankton", 3, "123 ABC"));
        FullPlateTypeList.Add(new PlateType(24, "South Dakota New", "South Yankton", 3, "0A1 234"));
        FullPlateTypeList.Add(new PlateType(25, "South Carolina", "South Volucrina", 3, "ABC 123"));
        FullPlateTypeList.Add(new PlateType(26, "Firefighter California", "San Andras", 0, "1ABC234"));


        FullPlateTypeList.Add(new PlateType(27, "Texas 1", "Alamo", 5, "ABC-1234"));
        FullPlateTypeList.Add(new PlateType(28, "Texas 2", "Alamo", 1, "ABC-1234"));
        FullPlateTypeList.Add(new PlateType(29, "Texas 3", "Alamo", 1, "ABC-1234"));
        FullPlateTypeList.Add(new PlateType(30, "Idaho", "Cataldo", 5, "A 123456"));


        FullPlateTypeList.Add(new PlateType(31, "Louisiana", "Maraisiana", 3, "123 ABC"));

        FullPlateTypeList.Add(new PlateType(32, "Oregon", "Cascadia", 10, "123 ABC"));



        FullPlateTypeList.Add(new PlateType(33, "Corvette Plate", "San Andreas", 0, "1ABC234"));
        FullPlateTypeList.Add(new PlateType(34, "Nothing", "San Andreas", 0));
        FullPlateTypeList.Add(new PlateType(35, "Nothing", "San Andreas", 0));
        FullPlateTypeList.Add(new PlateType(36, "Nothing", "San Andreas", 0));

        FullPlateTypeList.Add(new PlateType(37, "Montana", "Colina", 8, "0-12345A"));
        FullPlateTypeList.Add(new PlateType(38, "Colorado", "Coloverdo", 8, "ABC-D12"));
        FullPlateTypeList.Add(new PlateType(39, "Washington", "Jefferson", 8, "ABC1234"));



        FullPlateTypeList.Add(new PlateType(40, "Washington DC", "Jefferson CD", 3, "AB-1234"));
        FullPlateTypeList.Add(new PlateType(41, "Wisconsin", "Meskousin", 3, "ABC-1234"));
        FullPlateTypeList.Add(new PlateType(42, "Black on Yellow California", "San Andreas", 0, "1ABC234"));
        FullPlateTypeList.Add(new PlateType(43, "Nothing", "San Andreas", 0, "1ABC234"));
        FullPlateTypeList.Add(new PlateType(44, "Minnesota", "Minnewa", 3, "123-ABC"));
        FullPlateTypeList.Add(new PlateType(45, "North Dakota Old", "North Yankton", 3, "123 ABC"));
        FullPlateTypeList.Add(new PlateType(46, "Michigan", "Misquakewan", 3, "ABC 1234"));
        FullPlateTypeList.Add(new PlateType(47, "Nothing", "Carcer City", 0));
        FullPlateTypeList.Add(new PlateType(48, "Alaska", "Tanadux", 3, "ABC 123"));
        FullPlateTypeList.Add(new PlateType(49, "Hawaii", "Haiateaa", 3, "ABC 123"));
        FullPlateTypeList.Add(new PlateType(50, "Nothing", "San Andreas", 0, "1ABC234"));
        FullPlateTypeList.Add(new PlateType(51, "Willsylvania", "Willsylvania", 3, "ABC 123"));
        FullPlateTypeList.Add(new PlateType(52, "Nothing", "None", 0));
        FullPlateTypeList.Add(new PlateType(53, "Nothing", "None", 0));
        FullPlateTypeList.Add(new PlateType(54, "Nothing", "None", 0));
        FullPlateTypeList.Add(new PlateType(55, "Nothing", "None", 0));
        FullPlateTypeList.Add(new PlateType(56, "Nothing", "None", 0));

        FullPlateTypeList.Add(new PlateType(57, "Nevada 2", "Robada", 10, "123-A45"));

        FullPlateTypeList.Add(new PlateType(58, "San Andreas Veteran", "San Andreas", 1, "1ABC234"));
        FullPlateTypeList.Add(new PlateType(59, "Nothing", "None", 0));
        FullPlateTypeList.Add(new PlateType(60, "Nothing", "None", 0));
        FullPlateTypeList.Add(new PlateType(61, "Nothing", "None", 0));
        FullPlateTypeList.Add(new PlateType(62, "Nothing", "None", 0));
        FullPlateTypeList.Add(new PlateType(63, "Nothing", "None", 0));
        FullPlateTypeList.Add(new PlateType(64, "New Austin 1", "New Austin", 3, "ABC 123"));
        FullPlateTypeList.Add(new PlateType(65, "New Austin 2", "New Austin", 3, "ABC 123"));

        Serialization.SerializeParams(FullPlateTypeList, "Plugins\\LosSantosRED\\AlternateConfigs\\AddOnPlates_Wildbrick142\\PlateTypes_AddOnPlates_Wildbrick142.xml");

    }
}

