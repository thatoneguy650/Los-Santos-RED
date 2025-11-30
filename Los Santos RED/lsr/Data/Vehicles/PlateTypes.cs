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
using static System.Windows.Forms.AxHost;


public class PlateTypes : IPlateTypes
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\PlateTypes.xml";
    public PlateTypeManager PlateTypeManager { get; private set; }
    public void ReadConfig(string configName)
    {
        string fileName = string.IsNullOrEmpty(configName) ? "PlateTypes*.xml" : $"PlateTypes_{configName}.xml";

        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles(fileName).OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null && !configName.Equals("Default"))
        {
            EntryPoint.WriteToConsole($"Loaded PlateTypes config: {ConfigFile.FullName}", 0);
            PlateTypeManager = Serialization.DeserializeParam<PlateTypeManager>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded PlateTypes config  {ConfigFileName}", 0);
            PlateTypeManager = Serialization.DeserializeParam<PlateTypeManager>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No PlateTypes config found, creating default", 0);
            DefaultConfig();
            //DefaultConfig_Full();
            DefaultConfig_Gresk();
        }
    }
    public PlateType GetPlateType(int CurrentIndex)
    {
        return PlateTypeManager.PlateTypeList.OrderBy(x=> x.Order).FirstOrDefault(x => x.Index == CurrentIndex);
    }
    public PlateType GetPlateType(string State)
    {
        return PlateTypeManager.PlateTypeList.Where(x => x.StateID == State).PickRandom();
    }

    public PlateType GetRandomInStatePlate(string State, bool isMotorcycle)
    {
        if(isMotorcycle)
        {
            PlateType toReturn = PlateTypeManager.PlateTypeList.Where(x => x.StateID == State && x.IsMotorcyclePlate).RandomElementByWeight(x => x.InStateSpawnChance);
            if (toReturn != null)
            {
                return toReturn;
            }
        }
        return PlateTypeManager.PlateTypeList.Where(x => x.StateID == State && !x.IsMotorcyclePlate).RandomElementByWeight(x => x.InStateSpawnChance);
    }

    public PlateType GetPlateByDescription(string description)
    {
        return PlateTypeManager.PlateTypeList.Where(x => x.Description.ToLower() == description.ToLower()).FirstOrDefault();
    }
    public PlateType GetRandomPlateType(bool isMotorcycle)
    {
        if (!PlateTypeManager.PlateTypeList.Any())
            return null;

        if (isMotorcycle)
        {
            PlateType toReturn = PlateTypeManager.PlateTypeList.Where(x => x.CanSpawn && x.IsMotorcyclePlate).RandomElementByWeight(x => x.SpawnChance);
            if (toReturn != null)
            {
                return toReturn;
            }
        }
        return PlateTypeManager.PlateTypeList.Where(x=> x.CanSpawn && !x.IsMotorcyclePlate).RandomElementByWeight(x => x.SpawnChance);


        //List<PlateType> ToPickFrom = PlateTypeManager.PlateTypeList.Where(x => x.CanSpawn).ToList();
        //int Total = ToPickFrom.Sum(x => x.SpawnChance);
        //int RandomPick = RandomItems.MyRand.Next(0, Total);
        //foreach (PlateType Type in ToPickFrom)
        //{
        //    int SpawnChance = Type.SpawnChance;
        //    if (RandomPick < SpawnChance)
        //    {
        //        return Type;
        //    }
        //    RandomPick -= SpawnChance;
        //}
        //return null;
    }
    public string GetRandomVanityPlateText()
    {
        return PlateTypeManager.VanityPlates.PickRandom();
    }
    private void DefaultConfig()
    {
        PlateTypeManager = new PlateTypeManager();
        PlateTypeManager.PlateTypeList.Add(new PlateType(0, "San Andreas", StaticStrings.SanAndreasStateID, 1, "12ABC345") { AllowVanity = true, });
        PlateTypeManager.PlateTypeList.Add(new PlateType(1, "San Andreas Yellow on Black", StaticStrings.SanAndreasStateID, 1, "12ABC345") { AllowVanity = true, });
        PlateTypeManager.PlateTypeList.Add(new PlateType(2, "San Andreas Yellow on Blue", StaticStrings.SanAndreasStateID, 1, "12ABC345") { AllowVanity = true, });
        PlateTypeManager.PlateTypeList.Add(new PlateType(3, "San Andreas Classic", StaticStrings.SanAndreasStateID, 1, "12ABC345") { AllowVanity = true, });
        PlateTypeManager.PlateTypeList.Add(new PlateType(4, "San Andreas Exempt", StaticStrings.SanAndreasStateID, 0, "12ABC345") { CanOverwrite = false });
        PlateTypeManager.PlateTypeList.Add(new PlateType(5, "North Yankton", StaticStrings.NorthYanktonStateID, 2, "123  ABC"));


        PlateTypeManager.PlateTypeList.Add(new PlateType(6, "eCola", StaticStrings.SanAndreasStateID, 1, "12ABC345") { AllowVanity = true, });
        PlateTypeManager.PlateTypeList.Add(new PlateType(7, "Las Venturas", StaticStrings.LasVenturasStateID, 2, "12A  345") { AllowVanity = true, });
        PlateTypeManager.PlateTypeList.Add(new PlateType(8, "Liberty", StaticStrings.LibertyStateID, 2, "ABC 1234"));
        PlateTypeManager.PlateTypeList.Add(new PlateType(8, "Liberty", StaticStrings.AlderneyStateID, 2, "ABC 1234") { Order = 2 });
        PlateTypeManager.PlateTypeList.Add(new PlateType(9, "LS Car Meet", StaticStrings.SanAndreasStateID, 1, "12ABC345") { AllowVanity = true, });
        PlateTypeManager.PlateTypeList.Add(new PlateType(10, "LS Panic", StaticStrings.SanAndreasStateID, 1, "12ABC345") { AllowVanity = true, });
        PlateTypeManager.PlateTypeList.Add(new PlateType(11, "LS Pounders", StaticStrings.SanAndreasStateID, 1, "12ABC345") { AllowVanity = true, });
        PlateTypeManager.PlateTypeList.Add(new PlateType(12, "Sprunkn", StaticStrings.SanAndreasStateID, 1, "12ABC345") { AllowVanity = true, });




        PlateTypeManager.VanityPlates.Add("AZZ KIKR");
        PlateTypeManager.VanityPlates.Add("STARK 4");
        PlateTypeManager.VanityPlates.Add("OUTATIME");
        PlateTypeManager.VanityPlates.Add("MUF DVR");

        PlateTypeManager.VanityPlates.Add("NRVOUS");
        PlateTypeManager.VanityPlates.Add("PSY WGN");
        PlateTypeManager.VanityPlates.Add("SOUTHPAW");


        PlateTypeManager.VanityPlates.Add("UZI 4U");
        PlateTypeManager.VanityPlates.Add("DEV IL");
        PlateTypeManager.VanityPlates.Add("THX 138");
        PlateTypeManager.VanityPlates.Add("D-FENS");
        PlateTypeManager.VanityPlates.Add("ECTO-1");
        PlateTypeManager.VanityPlates.Add("THE CAPN");
        PlateTypeManager.VanityPlates.Add("18A 4RE");
        PlateTypeManager.VanityPlates.Add("LWYRUP");

        PlateTypeManager.VanityPlates.Add("ASSMAN");
        PlateTypeManager.VanityPlates.Add("WEIRDO");
        PlateTypeManager.VanityPlates.Add("PLNCRAZY");
        PlateTypeManager.VanityPlates.Add("WDRFULL");
        PlateTypeManager.VanityPlates.Add("YOU FOOL");
        PlateTypeManager.VanityPlates.Add("RUB1OUT");
        PlateTypeManager.VanityPlates.Add("ID8MOMS");

        PlateTypeManager.VanityPlates.Add("ANUSTART");
        PlateTypeManager.VanityPlates.Add("FL4TOUT");
        PlateTypeManager.VanityPlates.Add("NVRMYND");
        PlateTypeManager.VanityPlates.Add("LEDFOOT");


        PlateTypeManager.VanityPlates.Add("ECTO 1A");
        PlateTypeManager.VanityPlates.Add("KITT");
        PlateTypeManager.VanityPlates.Add("RNO 263");
        PlateTypeManager.VanityPlates.Add("LYN 274");
        PlateTypeManager.VanityPlates.Add("BAN ONE");
        PlateTypeManager.VanityPlates.Add("BDR529");
        PlateTypeManager.VanityPlates.Add("CQB241");
        PlateTypeManager.VanityPlates.Add("STYNCLSY");

        PlateTypeManager.VanityPlates.Add("FCK IT");
        PlateTypeManager.VanityPlates.Add("ORP 967");
        PlateTypeManager.VanityPlates.Add("NTGUILTY");
        PlateTypeManager.VanityPlates.Add("GRAYMTR");
        PlateTypeManager.VanityPlates.Add("PLAYUH");

        Serialization.SerializeParam(PlateTypeManager, ConfigFileName);
    }
    private void DefaultConfig_Gresk()
    {
        PlateTypeManager FullPlateTypeManager = new PlateTypeManager();
        List<PlateType> FullPlateTypeList = new List<PlateType>();


        FullPlateTypeList.Add(new PlateType(0, "San Andreas", StaticStrings.SanAndreasStateID, 1, "12ABC345") { AllowVanity = true, InStateSpawnChance = 100 });
        FullPlateTypeList.Add(new PlateType(1, "San Andreas Yellow on Black", StaticStrings.SanAndreasStateID, 0, "12ABC345") { AllowVanity = true });
        FullPlateTypeList.Add(new PlateType(2, "San Andreas Yellow on Blue", StaticStrings.SanAndreasStateID, 0, "12ABC345") { AllowVanity = true });;
        FullPlateTypeList.Add(new PlateType(3, "San Andreas Classic", StaticStrings.SanAndreasStateID, 0, "12ABC345") { AllowVanity = true });
        FullPlateTypeList.Add(new PlateType(4, "San Andreas Exempt", StaticStrings.SanAndreasStateID, 0, "12ABC345") { CanOverwrite = false });
        FullPlateTypeList.Add(new PlateType(5, "North Yankton Old", StaticStrings.NorthYanktonStateID, 0, "123  ABC") { InStateSpawnChance = 0 });


        FullPlateTypeList.Add(new PlateType(6, "eCola", StaticStrings.SanAndreasStateID, 0, "12ABC345") { AllowVanity = true });
        FullPlateTypeList.Add(new PlateType(7, "San Andreas Lake Washo", StaticStrings.SanAndreasStateID, 2, "12ABC345") { AllowVanity = true });
        FullPlateTypeList.Add(new PlateType(8, "Liberty", StaticStrings.LibertyStateID, 2, "ABC 1234") { DisablePrefix = true, InStateSpawnChance = 3 });
        FullPlateTypeList.Add(new PlateType(9, "LS Car Meet", StaticStrings.SanAndreasStateID, 0, "12ABC345") { AllowVanity = true });
        FullPlateTypeList.Add(new PlateType(10, "LS Panic", StaticStrings.SanAndreasStateID, 0, "12ABC345") { AllowVanity = true });
        FullPlateTypeList.Add(new PlateType(11, "LS Pounders", StaticStrings.SanAndreasStateID, 0, "12ABC345") { AllowVanity = true });
        FullPlateTypeList.Add(new PlateType(12, "Sprunkn", StaticStrings.SanAndreasStateID, 0, "12ABC345") { AllowVanity = true });

        FullPlateTypeList.Add(new PlateType(13, "Pimahito", "Pimahito", 8, "ABC  1DE"));
        FullPlateTypeList.Add(new PlateType(14, "Columbia", "Columbia", 4, "ABC12345"));

        FullPlateTypeList.Add(new PlateType(15, "US Gov", "US Gov", 0, "1234A") { DisablePrefix = true, CanOverwrite = false });


        FullPlateTypeList.Add(new PlateType(16, "Liberty", StaticStrings.LibertyStateID, 2, "ABC 1234") { DisablePrefix = true, InStateSpawnChance = 75 });
        FullPlateTypeList.Add(new PlateType(17, "Alderney", StaticStrings.AlderneyStateID, 2, "A12  BCD") { DisablePrefix = true, InStateSpawnChance = 100 });
        FullPlateTypeList.Add(new PlateType(18, "Leonida", StaticStrings.LeonidaStateID, 2, "A12  BCD") { DisablePrefix = true });


        FullPlateTypeList.Add(new PlateType(19, "Liberty Police", StaticStrings.LibertyStateID,0, " 123456 ") { DisablePrefix = true, });
        FullPlateTypeList.Add(new PlateType(20, "North Yankton", StaticStrings.NorthYanktonStateID, 2, "123  ABC") { DisablePrefix = true, InStateSpawnChance = 100 });

        FullPlateTypeList.Add(new PlateType(21, "New Austin", "New Austin", 4, "ABC 1234"));
        FullPlateTypeList.Add(new PlateType(22, "Ambarino", "Ambarino", 6, "ABC  D12"));
        FullPlateTypeList.Add(new PlateType(23, "Robada", "Robada", 8, "12A  345"));

        FullPlateTypeList.Add(new PlateType(24, "Leonida County", StaticStrings.LeonidaStateID, 0, " 123456 ") { DisablePrefix = true, });
        FullPlateTypeList.Add(new PlateType(25, "Leonida City", StaticStrings.LeonidaStateID, 0, " 123456 ") { DisablePrefix = true, });
        FullPlateTypeList.Add(new PlateType(26, "South Yankton", "South Yankton", 2, "1AB  345"));
        FullPlateTypeList.Add(new PlateType(27, "Lemoyne", "Lemoyne", 2, "123  ABC"));
        FullPlateTypeList.Add(new PlateType(28, "Willamette", "Willamette", 6, "123  ABC"));
        FullPlateTypeList.Add(new PlateType(29, "Nacimiento", "Nacimiento", 6, "ABC  123"));

        FullPlateTypeList.Add(new PlateType(30, "Jolliet", "Jolliet", 2, "AB 12345"));
        FullPlateTypeList.Add(new PlateType(31, "Nez Perce", "Nez Perce", 4, "1A B2345"));
        FullPlateTypeList.Add(new PlateType(32, "Noochee", "Noochee", 4, "A12  3BC"));

        FullPlateTypeList.Add(new PlateType(33, "Shoshone", "Shoshone", 2, "1 23456A"));



        FullPlateTypeList.Add(new PlateType(34, "Liberty Exculpate", StaticStrings.LibertyStateID, 2, "A1B 23C4") { DisablePrefix = true, InStateSpawnChance = 25 });
        FullPlateTypeList.Add(new PlateType(35, "Alderney Municipal", StaticStrings.AlderneyStateID, 2, "123456AB") { CanOverwrite = false, });
        FullPlateTypeList.Add(new PlateType(36, "Keweenawan", "Keweenawan", 2, "ABC-1234"));
        FullPlateTypeList.Add(new PlateType(37, "Meskousing", "Meskousing", 2, "1 23456A"));

        FullPlateTypeList.Add(new PlateType(38, "Radon", "Radon", 2, "ABC  123"));
        FullPlateTypeList.Add(new PlateType(39, "Saybrook", "Saybrook", 2, "AB 12345"));


        FullPlateTypeList.Add(new PlateType(40, "Lenape", "Lenape", 2, " 123456 "));
        FullPlateTypeList.Add(new PlateType(41, "Chickatawbut", "Chickatawbut", 2, "123  AB4"));


        FullPlateTypeList.Add(new PlateType(42, "Platte", "Platte", 2, "123  AB4"));
        FullPlateTypeList.Add(new PlateType(43, "South Cromwell", "South Cromwell", 2, "123  ABC"));
        FullPlateTypeList.Add(new PlateType(44, "North Cromwell", "North Cromwell", 2, "ABC 1234"));

        FullPlateTypeList.Add(new PlateType(45, "West Powhatan", "West Powhatan", 2, "123 345A"));
        FullPlateTypeList.Add(new PlateType(46, "Powhatan", "Powhatan", 2, "123 345A"));


        FullPlateTypeList.Add(new PlateType(47, "Champlain", "Champlain", 2, "ABC  123"));


        FullPlateTypeList.Add(new PlateType(48, "Patmo Island", "Patmo Island", 2, "1AB  234"));
        FullPlateTypeList.Add(new PlateType(49, "Gnadenhutten", "Gnadenhutten", 2, "ABC 1234"));

        FullPlateTypeList.Add(new PlateType(50, "Mackinac", "Mackinac", 2, "ABC 1234"));
        FullPlateTypeList.Add(new PlateType(51, "Mauna Loa", "Mauna Loa", 1, "ABC  123"));

        FullPlateTypeList.Add(new PlateType(52, "Aleutia", "Aleutia", 2, "ABC  123"));

        FullPlateTypeList.Add(new PlateType(53, "Quapaw", "Quapaw", 2, "ABC  12D"));

        FullPlateTypeList.Add(new PlateType(54, "Gloriana", "Gloriana", 2, "ABC 1234"));
        FullPlateTypeList.Add(new PlateType(55, "Tippecanoe", "Tippecanoe", 2, " 123ABC "));
        FullPlateTypeList.Add(new PlateType(56, "Humboldt", "Humboldt", 2, "1234ABCD"));



        FullPlateTypeList.Add(new PlateType(57, "Franklin", "Franklin", 2, "123 ABCD"));
        FullPlateTypeList.Add(new PlateType(58, "Knobs", "Knobs", 2, "  123ABC"));
        FullPlateTypeList.Add(new PlateType(59, "Perche", "Perche", 2, "  123 AB"));

        FullPlateTypeList.Add(new PlateType(60, "Anneland", "Anneland", 2, "1AB23455"));
        FullPlateTypeList.Add(new PlateType(61, "Jackson", "Jackson", 2, "ABC  123"));


        FullPlateTypeList.Add(new PlateType(62, "Squaretop", "Squaretop", 2, "1A  2345"));
        FullPlateTypeList.Add(new PlateType(63, "Squaretop Old", "Squaretop Old", 2, "1  2345A"));



        FullPlateTypeList.Add(new PlateType(64, "Niobrara", "Niobrara", 2, "ABC  123"));
        FullPlateTypeList.Add(new PlateType(65, "Putnam", "Putnam", 2, "ABC 1234"));
        FullPlateTypeList.Add(new PlateType(66, "Pascagoula", "Pascagoula", 2, "ABC  123"));
        FullPlateTypeList.Add(new PlateType(67, "Coosa", "Coosa", 2, "1A2345B6"));
        FullPlateTypeList.Add(new PlateType(68, "New Cornwall", "New Cornwall", 2, "123 4567"));

        FullPlateTypeList.Add(new PlateType(69, "Columbia, DW", "Columbia, DW", 2, "AB  1234"));


        FullPlateTypeList.Add(new PlateType(70, "San Andreas Disabled", StaticStrings.SanAndreasStateID, 1, "AB123") { InStateSpawnChance = 2 });
        FullPlateTypeList.Add(new PlateType(71, "San Andreas Motorcycle", StaticStrings.SanAndreasStateID, 1, "1A234567") { IsMotorcyclePlate = true, });
        FullPlateTypeList.Add(new PlateType(72, "San Andreas Motorcycle Alt", StaticStrings.SanAndreasStateID, 1, "1A234567"){ IsMotorcyclePlate = true, });


        FullPlateTypeList.Add(new PlateType(73, "Gloriana Legacy", "Gloriana", 1, "ABC   D1"));
        FullPlateTypeList.Add(new PlateType(74, "Gloriana Legacy Motorcycle", "Gloriana", 1, "ABC   D1") { IsMotorcyclePlate = true });




        FullPlateTypeList.Add(new PlateType(75, "Jackson Older", "Jackson", 2, "ABC  123"));
        FullPlateTypeList.Add(new PlateType(76, "New Austin Black", "New Austin", 4, "ABC 1234"));
        FullPlateTypeList.Add(new PlateType(77, "Noochee Alt", "Noochee", 4, "   1234A"));
        FullPlateTypeList.Add(new PlateType(78, "Putnam Older", "Putnam", 2, "ABC 1234"));
        FullPlateTypeList.Add(new PlateType(79, "Shoshone Older", "Shoshone", 2, "1 23456A"));



        FullPlateTypeList.Add(new PlateType(80, "Athabasca", "Athabasca", 2, "123  ABC"));
        FullPlateTypeList.Add(new PlateType(81, "British Washington", "British Washington", 2, "AB1  23C"));
        FullPlateTypeList.Add(new PlateType(82, "Victoria 01", "Victoria", 2, "ABC 1234"));
        FullPlateTypeList.Add(new PlateType(83, "Victoria 02", "Victoria", 2, "ABC 1234"));


        FullPlateTypeManager.PlateTypeList = FullPlateTypeList;
        FullPlateTypeManager.VanityPlates = PlateTypeManager.VanityPlates.Copy();
        Serialization.SerializeParam(FullPlateTypeManager, "Plugins\\LosSantosRED\\AlternateConfigs\\FullModernLicensePlates\\PlateTypes_FullModernLicensePlates.xml");

    }
}

