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
    public PlateTypeManager PlateTypeManager { get; private set; }

    public void ReadConfig()
    {

        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("PlateTypes*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
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

    public PlateType GetRandomInStatePlate(string State)
    {
        return PlateTypeManager.PlateTypeList.Where(x => x.StateID == State).RandomElementByWeight(x => x.InStateSpawnChance);
    }

    public PlateType GetPlateByDescription(string description)
    {
        return PlateTypeManager.PlateTypeList.Where(x => x.Description.ToLower() == description.ToLower()).FirstOrDefault();
    }
    public PlateType GetRandomPlateType()
    {
        if (!PlateTypeManager.PlateTypeList.Any())
            return null;

        List<PlateType> ToPickFrom = PlateTypeManager.PlateTypeList.Where(x => x.CanSpawn).ToList();
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
    public string GetRandomVanityPlateText()
    {
        return PlateTypeManager.VanityPlates.PickRandom();
    }
    private void DefaultConfig()
    {
        PlateTypeManager = new PlateTypeManager();
        PlateTypeManager.PlateTypeList.Add(new PlateType(0, "San Andreas", StaticStrings.SanAndreasStateID, 1, "12ABC345"));
        PlateTypeManager.PlateTypeList.Add(new PlateType(1, "San Andreas Yellow on Black", StaticStrings.SanAndreasStateID, 1, "12ABC345"));
        PlateTypeManager.PlateTypeList.Add(new PlateType(2, "San Andreas Yellow on Blue", StaticStrings.SanAndreasStateID, 1, "12ABC345"));
        PlateTypeManager.PlateTypeList.Add(new PlateType(3, "San Andreas Classic", StaticStrings.SanAndreasStateID, 1, "12ABC345"));
        PlateTypeManager.PlateTypeList.Add(new PlateType(4, "San Andreas Exempt", StaticStrings.SanAndreasStateID, 0, "12ABC345") { CanOverwrite = false });
        PlateTypeManager.PlateTypeList.Add(new PlateType(5, "North Yankton", StaticStrings.NorthYanktonStateID, 2, "123  ABC"));


        PlateTypeManager.PlateTypeList.Add(new PlateType(6, "eCola", StaticStrings.SanAndreasStateID, 1, "12ABC345"));
        PlateTypeManager.PlateTypeList.Add(new PlateType(7, "Las Venturas", StaticStrings.LasVenturasStateID, 2, "12A  345"));
        PlateTypeManager.PlateTypeList.Add(new PlateType(8, "Liberty", StaticStrings.LibertyStateID, 2, "ABC 1234"));
        PlateTypeManager.PlateTypeList.Add(new PlateType(8, "Liberty", StaticStrings.AlderneyStateID, 2, "ABC 1234") { Order = 2 });
        PlateTypeManager.PlateTypeList.Add(new PlateType(9, "LS Car Meet", StaticStrings.SanAndreasStateID, 1, "12ABC345"));
        PlateTypeManager.PlateTypeList.Add(new PlateType(10, "LS Panic", StaticStrings.SanAndreasStateID, 1, "12ABC345"));
        PlateTypeManager.PlateTypeList.Add(new PlateType(11, "LS Pounders", StaticStrings.SanAndreasStateID, 1, "12ABC345"));
        PlateTypeManager.PlateTypeList.Add(new PlateType(12, "Sprunkn", StaticStrings.SanAndreasStateID, 1, "12ABC345"));




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
    //private void DefaultConfig_Full()
    //{
    //    PlateTypeManager FullPlateTypeManager = new PlateTypeManager();
    //    List<PlateType> FullPlateTypeList = new List<PlateType>();
    //    FullPlateTypeList.Add(new PlateType(0, "San Andreas", StaticStrings.SanAndreasStateID, 1, "12ABC345"));
    //    FullPlateTypeList.Add(new PlateType(1, "San Andreas Yellow on Black", StaticStrings.SanAndreasStateID, 1, "12ABC345"));
    //    FullPlateTypeList.Add(new PlateType(2, "San Andreas Yellow on Blue", StaticStrings.SanAndreasStateID, 1, "12ABC345"));
    //    FullPlateTypeList.Add(new PlateType(3, "San Andreas Classic", StaticStrings.SanAndreasStateID, 1, "12ABC345"));
    //    FullPlateTypeList.Add(new PlateType(4, "San Andreas Exempt", StaticStrings.SanAndreasStateID, 0, "12ABC345") { CanOverwrite = false });
    //    FullPlateTypeList.Add(new PlateType(5, "North Yankton Classic", StaticStrings.NorthYanktonStateID, 3, "123  ABC"));
    //    FullPlateTypeList.Add(new PlateType(6, "Liberty Alt 1", StaticStrings.LibertyStateID, 0, "ABC-1234"));
    //    FullPlateTypeList.Add(new PlateType(7, "Vice City", "Vice City", 0, "123  4AB"));
    //    FullPlateTypeList.Add(new PlateType(8, "Liberty Alt 2", StaticStrings.LibertyStateID, 0, "ABC-1234"));
    //    FullPlateTypeList.Add(new PlateType(9, "Liberty Alt 3", StaticStrings.LibertyStateID, 0, "ABC-1234"));
    //    FullPlateTypeList.Add(new PlateType(10, "Sprunk Logo", "None", 0, "1ABC2345"));
    //    FullPlateTypeList.Add(new PlateType(11, "San Andreas Patriots", "None", 0, "1ABC2345"));
    //    FullPlateTypeList.Add(new PlateType(12, "Los Santos Shrimps", StaticStrings.SanAndreasStateID, 1, "12ABC345"));
    //    FullPlateTypeList.Add(new PlateType(13, "San Andreas Alt 1", StaticStrings.SanAndreasStateID, 0, "12ABC345"));
    //    FullPlateTypeList.Add(new PlateType(14, "North Volucrina", "North Volucrina", 3, "ABC-1234"));
    //    FullPlateTypeList.Add(new PlateType(15, "Alderny", StaticStrings.AlderneyStateID, 3, "D12-ABCD"));

    //    FullPlateTypeList.Add(new PlateType(16, "Robada", "Robada", 10, "123-A45B"));




    //    FullPlateTypeList.Add(new PlateType(17, "Lincoln", "Lincoln", 3, "AB 12345"));
    //    FullPlateTypeList.Add(new PlateType(18, "Epsilon", "None", 3, "12ABC345"));
    //    FullPlateTypeList.Add(new PlateType(19, "Our Pantera", "Miami", 3, "123  4AB"));
    //    FullPlateTypeList.Add(new PlateType(20, "Pantera Alt 1", "Miami", 0, "123  4AB"));
    //    FullPlateTypeList.Add(new PlateType(21, "Pantera Alt 2", "Miami", 0, "123  4AB"));

    //    FullPlateTypeList.Add(new PlateType(22, "Hareona", "Hareona", 12, "12ABC345"));


    //    FullPlateTypeList.Add(new PlateType(23, "North Yankton", StaticStrings.NorthYanktonStateID, 3, "123  ABC"));
    //    FullPlateTypeList.Add(new PlateType(24, "South Yankton", "South Yankton", 3, "0A1  234"));
    //    FullPlateTypeList.Add(new PlateType(25, "South Volucrina", "South Volucrina", 3, "ABC  123"));
    //    FullPlateTypeList.Add(new PlateType(26, "San Andreas Firefighter", StaticStrings.SanAndreasStateID, 0, "1ABC234"));


    //    FullPlateTypeList.Add(new PlateType(27, "Alamo", "Alamo", 5, "ABC-1234"));
    //    FullPlateTypeList.Add(new PlateType(28, "Alamo Classic 1", "Alamo", 1, "ABC-1234"));
    //    FullPlateTypeList.Add(new PlateType(29, "Alamo Classic 2", "Alamo", 1, "ABC-1234"));
    //    FullPlateTypeList.Add(new PlateType(30, "Cataldo", "Cataldo", 5, "A 123456"));


    //    FullPlateTypeList.Add(new PlateType(31, "Maraisiana", "Maraisiana", 3, "123  ABC"));

    //    FullPlateTypeList.Add(new PlateType(32, "Cascadia", "Cascadia", 10, "123  ABC"));



    //    FullPlateTypeList.Add(new PlateType(33, "San Andreas Corvette", StaticStrings.SanAndreasStateID, 0, "12ABC345"));
    //    FullPlateTypeList.Add(new PlateType(34, "Nothing", StaticStrings.SanAndreasStateID, 0));
    //    FullPlateTypeList.Add(new PlateType(35, "Nothing", StaticStrings.SanAndreasStateID, 0));
    //    FullPlateTypeList.Add(new PlateType(36, "Nothing", StaticStrings.SanAndreasStateID, 0));

    //    FullPlateTypeList.Add(new PlateType(37, "Colina", "Colina", 8, "0-12345A"));
    //    FullPlateTypeList.Add(new PlateType(38, "Ambarino", "Ambarino", 8, "ABC-D123"));
    //    FullPlateTypeList.Add(new PlateType(39, "Jefferson", "Jefferson", 8, "ABC12345"));



    //    FullPlateTypeList.Add(new PlateType(40, "Jefferson CD", "Jefferson CD", 3, "AB-1234"));
    //    FullPlateTypeList.Add(new PlateType(41, "Meskousin", "Meskousin", 3, "ABC-1234"));
    //    FullPlateTypeList.Add(new PlateType(42, "San Andreas Black on Yellow", StaticStrings.SanAndreasStateID, 0, "12ABC345"));
    //    FullPlateTypeList.Add(new PlateType(43, "Nothing", StaticStrings.SanAndreasStateID, 0, "12ABC345"));
    //    FullPlateTypeList.Add(new PlateType(44, "Minnewa", "Minnewa", 3, "123-ABCD"));
    //    FullPlateTypeList.Add(new PlateType(45, "North Yankton Old", StaticStrings.NorthYanktonStateID, 3, "123  ABC"));
    //    FullPlateTypeList.Add(new PlateType(46, "Misquakewan", "Misquakewan", 3, "ABC 1234"));
    //    FullPlateTypeList.Add(new PlateType(47, "Carcer City", "Carcer City", 0));
    //    FullPlateTypeList.Add(new PlateType(48, "Tanadux", "Tanadux", 3, "ABC  123"));
    //    FullPlateTypeList.Add(new PlateType(49, "Haiateaa", "Haiateaa", 3, "ABC  123"));
    //    FullPlateTypeList.Add(new PlateType(50, "Neon San Andreas", StaticStrings.SanAndreasStateID, 0, "12ABC345"));
    //    FullPlateTypeList.Add(new PlateType(51, "Willsylvania", "Willsylvania", 3, "ABC  123"));
    //    FullPlateTypeList.Add(new PlateType(52, "LS Plate 2 Years", StaticStrings.SanAndreasStateID, 0));
    //    FullPlateTypeList.Add(new PlateType(53, "Nothing", "None", 0));
    //    FullPlateTypeList.Add(new PlateType(54, "Nothing", "None", 0));
    //    FullPlateTypeList.Add(new PlateType(55, "Nothing", "None", 0));
    //    FullPlateTypeList.Add(new PlateType(56, "Nothing", "None", 0));

    //    FullPlateTypeList.Add(new PlateType(57, "Robada Classic", "Robada", 10, "123-A456"));

    //    FullPlateTypeList.Add(new PlateType(58, "San Andreas Veteran", StaticStrings.SanAndreasStateID, 1, "12ABC345"));
    //    FullPlateTypeList.Add(new PlateType(59, "Nothing", "None", 0));
    //    FullPlateTypeList.Add(new PlateType(60, "Nothing", "None", 0));
    //    FullPlateTypeList.Add(new PlateType(61, "Nothing", "None", 0));
    //    FullPlateTypeList.Add(new PlateType(62, "Nothing", "None", 0));
    //    FullPlateTypeList.Add(new PlateType(63, "Nothing", "None", 0));
    //    FullPlateTypeList.Add(new PlateType(64, "New Austin", "New Austin", 3, "ABC  123"));
    //    FullPlateTypeList.Add(new PlateType(65, "New Austin Centennial", "New Austin", 3, "ABC  123"));

    //    FullPlateTypeManager.PlateTypeList = FullPlateTypeList;
    //    FullPlateTypeManager.VanityPlates = PlateTypeManager.VanityPlates.Copy();
    //    Serialization.SerializeParam(FullPlateTypeManager, "Plugins\\LosSantosRED\\AlternateConfigs\\AddOnPlates_Wildbrick142\\PlateTypes_AddOnPlates_Wildbrick142.xml");

    //}
    private void DefaultConfig_Gresk()
    {
        PlateTypeManager FullPlateTypeManager = new PlateTypeManager();
        List<PlateType> FullPlateTypeList = new List<PlateType>();


        FullPlateTypeList.Add(new PlateType(0, "San Andreas", StaticStrings.SanAndreasStateID, 1, "12ABC345"));
        FullPlateTypeList.Add(new PlateType(1, "San Andreas Yellow on Black", StaticStrings.SanAndreasStateID, 1, "12ABC345"));
        FullPlateTypeList.Add(new PlateType(2, "San Andreas Yellow on Blue", StaticStrings.SanAndreasStateID, 1, "12ABC345"));
        FullPlateTypeList.Add(new PlateType(3, "San Andreas Classic", StaticStrings.SanAndreasStateID, 1, "12ABC345"));
        FullPlateTypeList.Add(new PlateType(4, "San Andreas Exempt", StaticStrings.SanAndreasStateID, 0, "12ABC345") { CanOverwrite = false });
        //FullPlateTypeList.Add(new PlateType(5, "North Yankton", StaticStrings.NorthYanktonStateID, 2, "123  ABC"));


        FullPlateTypeList.Add(new PlateType(6, "eCola", StaticStrings.SanAndreasStateID, 1, "12ABC345"));
        FullPlateTypeList.Add(new PlateType(7, "Las Venturas", StaticStrings.LasVenturasStateID, 2, "12A  345"));
        //FullPlateTypeList.Add(new PlateType(8, "Liberty", StaticStrings.LibertyStateID, 2, "ABC 1234"));
        //FullPlateTypeList.Add(new PlateType(8, "Liberty", StaticStrings.AlderneyStateID, 2, "ABC 1234") { Order = 2 });
        FullPlateTypeList.Add(new PlateType(9, "LS Car Meet", StaticStrings.SanAndreasStateID, 1, "12ABC345"));
        FullPlateTypeList.Add(new PlateType(10, "LS Panic", StaticStrings.SanAndreasStateID, 1, "12ABC345"));
        FullPlateTypeList.Add(new PlateType(11, "LS Pounders", StaticStrings.SanAndreasStateID, 1, "12ABC345"));
        FullPlateTypeList.Add(new PlateType(12, "Sprunkn", StaticStrings.SanAndreasStateID, 1, "12ABC345"));

        FullPlateTypeList.Add(new PlateType(13, "Pimahito", "Pimahito", 12, "ABC  1DE") { DisablePrefix = true, });
        FullPlateTypeList.Add(new PlateType(14, "Columbia", "Columbia", 8, "ABC12345"));

        FullPlateTypeList.Add(new PlateType(15, "US Gov", "US Gov", 0, "1234A") { DisablePrefix = true, CanOverwrite = false });


        FullPlateTypeList.Add(new PlateType(16, "Liberty", StaticStrings.LibertyStateID, 2, "ABC 1234") { DisablePrefix = true, InStateSpawnChance = 75 });
        FullPlateTypeList.Add(new PlateType(17, "Alderney", StaticStrings.AlderneyStateID, 2, "A12  BCD") { DisablePrefix = true });
        FullPlateTypeList.Add(new PlateType(18, "Leonida", StaticStrings.LeonidaStateID, 2, "A12  BCD") { DisablePrefix = true });


        FullPlateTypeList.Add(new PlateType(19, "Liberty Police", StaticStrings.LibertyStateID,0, " 123456 ") { DisablePrefix = true, });
        FullPlateTypeList.Add(new PlateType(20, "North Yankton", StaticStrings.NorthYanktonStateID, 2, "123  ABC") { DisablePrefix = true, });

        FullPlateTypeList.Add(new PlateType(21, "New Austin", "New Austin", 7, "ABC 1234"));
        FullPlateTypeList.Add(new PlateType(22, "Ambarino", "Ambarino", 6, "ABC  D12"));
        FullPlateTypeList.Add(new PlateType(23, "Robada", "Robada", 12, "12A  345"));

        FullPlateTypeList.Add(new PlateType(24, "Leonida County", StaticStrings.LeonidaStateID, 0, " 123456 ") { DisablePrefix = true, });
        FullPlateTypeList.Add(new PlateType(25, "Leonida City", StaticStrings.LeonidaStateID, 0, " 123456 ") { DisablePrefix = true, });
        FullPlateTypeList.Add(new PlateType(26, "South Yankton", "South Yankton", 2, "1AB  345"));
        FullPlateTypeList.Add(new PlateType(27, "Lemoyne", "Lemoyne", 2, "123  ABC") { DisablePrefix = true, });
        FullPlateTypeList.Add(new PlateType(28, "Willamette", "Willamette", 8, "123  ABC") { DisablePrefix = true, });
        FullPlateTypeList.Add(new PlateType(29, "Nacimiento", "Nacimiento", 6, "ABC  123") { DisablePrefix = true, });

        FullPlateTypeList.Add(new PlateType(30, "Jolliet", "Jolliet", 2, "AB 12345"));
        FullPlateTypeList.Add(new PlateType(31, "Nez Perce", "Nez Perce", 5, "1A B2345"));
        FullPlateTypeList.Add(new PlateType(32, "Noochee", "Noochee", 5, "1ABC2"));

        FullPlateTypeList.Add(new PlateType(33, "Shoshone", "Shoshone", 5, "1 23456A"));



        FullPlateTypeList.Add(new PlateType(34, "Liberty Exculpate", StaticStrings.LibertyStateID, 2, "A1B 23C4") { DisablePrefix = true, InStateSpawnChance = 25 });
        FullPlateTypeList.Add(new PlateType(35, "Alderney Municipal", StaticStrings.AlderneyStateID, 0, "123456AB") { CanOverwrite = false, });
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

        FullPlateTypeList.Add(new PlateType(54, "Altamaha", "Altamaha", 2, "ABC 1234"));
        FullPlateTypeList.Add(new PlateType(55, "Tippecanoe", "Tippecanoe", 2, " 123ABC "));
        FullPlateTypeList.Add(new PlateType(56, "Humboldt", "Humboldt", 2, "1234ABCD"));



        FullPlateTypeList.Add(new PlateType(57, "Franklin", "Franklin", 2, "123 ABCD"));
        FullPlateTypeList.Add(new PlateType(58, "Knobs", "Knobs", 2, "  123ABC"));
        FullPlateTypeList.Add(new PlateType(59, "Perche", "Perche", 2, "  123 AB"));

        FullPlateTypeList.Add(new PlateType(60, "Anneland", "Anneland", 2, "1AB23455"));
        FullPlateTypeList.Add(new PlateType(61, "Jackson", "Jackson", 2, "ABC  123"));


        FullPlateTypeList.Add(new PlateType(62, "Squaretop", "Squaretop", 4, "1A  2345"));
        FullPlateTypeList.Add(new PlateType(63, "Squaretop Old", "Squaretop Old", 4, "1  2345A"));



        FullPlateTypeList.Add(new PlateType(64, "Niobrara", "Niobrara", 2, "ABC  123"));
        FullPlateTypeList.Add(new PlateType(65, "Putnam", "Putnam", 2, "ABC 1234"));
        FullPlateTypeList.Add(new PlateType(66, "Pascagoula", "Pascagoula", 2, "ABC  123"));
        FullPlateTypeList.Add(new PlateType(67, "Coosa", "Coosa", 2, "1A2345B6"));
        FullPlateTypeList.Add(new PlateType(68, "New Cornwall", "New Cornwall", 2, "123 4567"));

        FullPlateTypeList.Add(new PlateType(69, "Columbia, DW", "Columbia, DW", 2, "AB  1234"));


        //Pascagoula
        FullPlateTypeManager.PlateTypeList = FullPlateTypeList;
        FullPlateTypeManager.VanityPlates = PlateTypeManager.VanityPlates.Copy();
        Serialization.SerializeParam(FullPlateTypeManager, "Plugins\\LosSantosRED\\AlternateConfigs\\FullModernLicensePlates\\PlateTypes_FullModernLicensePlates.xml");

    }
    //private void DefaultConfig_Full()
    //{
    //    PlateTypeManager FullPlateTypeManager = new PlateTypeManager();
    //    List<PlateType> FullPlateTypeList = new List<PlateType>();
    //    FullPlateTypeList.Add(new PlateType(0, "San Andreas", StaticStrings.SanAndreasStateID, 1, "12ABC345"));
    //    FullPlateTypeList.Add(new PlateType(1, "San Andreas Yellow on Black", StaticStrings.SanAndreasStateID, 1, "12ABC345"));
    //    FullPlateTypeList.Add(new PlateType(2, "San Andreas Yellow on Blue", StaticStrings.SanAndreasStateID, 1, "12ABC345"));
    //    FullPlateTypeList.Add(new PlateType(3, "San Andreas Classic", StaticStrings.SanAndreasStateID, 1, "12ABC345"));
    //    FullPlateTypeList.Add(new PlateType(4, "San Andreas Exempt", StaticStrings.SanAndreasStateID, 0, "12ABC345") { CanOverwrite = false });
    //    FullPlateTypeList.Add(new PlateType(5, "North Yankton Classic", StaticStrings.NorthYanktonStateID, 3, "123 ABC"));
    //    FullPlateTypeList.Add(new PlateType(6, "Liberty 1", StaticStrings.LibertyStateID, 0, "ABC-1234"));
    //    FullPlateTypeList.Add(new PlateType(7, "Florida 1", "Vice City", 0, "123  4AB"));
    //    FullPlateTypeList.Add(new PlateType(8, "Liberty 2", StaticStrings.LibertyStateID, 0, "ABC-1234"));
    //    FullPlateTypeList.Add(new PlateType(9, "Liberty 3", StaticStrings.LibertyStateID, 0, "ABC-1234"));
    //    FullPlateTypeList.Add(new PlateType(10, "Sprunk Logo", "None", 0, "1ABC234"));
    //    FullPlateTypeList.Add(new PlateType(11, "Patriots?", "None", 0, "1ABC234"));
    //    FullPlateTypeList.Add(new PlateType(12, "Los Santos Shrimps", StaticStrings.SanAndreasStateID, 1, "12ABC345"));
    //    FullPlateTypeList.Add(new PlateType(13, "Nevada San Andreas Mashup", StaticStrings.SanAndreasStateID, 0, "12ABC345"));
    //    FullPlateTypeList.Add(new PlateType(14, "North Carolina", "North Volucrina", 3, "ABC-1234"));
    //    FullPlateTypeList.Add(new PlateType(15, "New Jersey", StaticStrings.AlderneyStateID, 3, "D12-ABC"));

    //    FullPlateTypeList.Add(new PlateType(16, "Nevada", "Robada", 10, "123-A45"));




    //    FullPlateTypeList.Add(new PlateType(17, "Illinois", "Lincoln", 3, "AB 12345"));
    //    FullPlateTypeList.Add(new PlateType(18, "Epsilon", "None", 3, "12ABC345"));
    //    FullPlateTypeList.Add(new PlateType(19, "Our Florida", "Miami", 3, "123  4AB"));
    //    FullPlateTypeList.Add(new PlateType(20, "Florida 1", "Miami", 0, "123  4AB"));
    //    FullPlateTypeList.Add(new PlateType(21, "Florida 2", "Miami", 0, "123  4AB"));

    //    FullPlateTypeList.Add(new PlateType(22, "Arizona", "Hareona", 12, "12ABC345"));


    //    FullPlateTypeList.Add(new PlateType(23, "North Dakota New", StaticStrings.NorthYanktonStateID, 3, "123  ABC"));
    //    FullPlateTypeList.Add(new PlateType(24, "South Dakota New", "South Yankton", 3, "0A1  234"));
    //    FullPlateTypeList.Add(new PlateType(25, "South Carolina", "South Volucrina", 3, "ABC  123"));
    //    FullPlateTypeList.Add(new PlateType(26, "Firefighter California", StaticStrings.SanAndreasStateID, 0, "1ABC234"));


    //    FullPlateTypeList.Add(new PlateType(27, "Texas 1", "Alamo", 5, "ABC-1234"));
    //    FullPlateTypeList.Add(new PlateType(28, "Texas 2", "Alamo", 1, "ABC-1234"));
    //    FullPlateTypeList.Add(new PlateType(29, "Texas 3", "Alamo", 1, "ABC-1234"));
    //    FullPlateTypeList.Add(new PlateType(30, "Idaho", "Cataldo", 5, "A 123456"));


    //    FullPlateTypeList.Add(new PlateType(31, "Louisiana", "Maraisiana", 3, "123  ABC"));

    //    FullPlateTypeList.Add(new PlateType(32, "Oregon", "Cascadia", 10, "123  ABC"));



    //    FullPlateTypeList.Add(new PlateType(33, "Corvette Plate", StaticStrings.SanAndreasStateID, 0, "12ABC345"));
    //    FullPlateTypeList.Add(new PlateType(34, "Nothing", StaticStrings.SanAndreasStateID, 0));
    //    FullPlateTypeList.Add(new PlateType(35, "Nothing", StaticStrings.SanAndreasStateID, 0));
    //    FullPlateTypeList.Add(new PlateType(36, "Nothing", StaticStrings.SanAndreasStateID, 0));

    //    FullPlateTypeList.Add(new PlateType(37, "Montana", "Colina", 8, "0-12345A"));
    //    FullPlateTypeList.Add(new PlateType(38, "Colorado", "Coloverdo", 8, "ABC-D12"));
    //    FullPlateTypeList.Add(new PlateType(39, "Washington", "Jefferson", 8, "ABC1234"));



    //    FullPlateTypeList.Add(new PlateType(40, "Washington DC", "Jefferson CD", 3, "AB-1234"));
    //    FullPlateTypeList.Add(new PlateType(41, "Wisconsin", "Meskousin", 3, "ABC-1234"));
    //    FullPlateTypeList.Add(new PlateType(42, "Black on Yellow California", StaticStrings.SanAndreasStateID, 0, "12ABC345"));
    //    FullPlateTypeList.Add(new PlateType(43, "Nothing", StaticStrings.SanAndreasStateID, 0, "12ABC345"));
    //    FullPlateTypeList.Add(new PlateType(44, "Minnesota", "Minnewa", 3, "123-ABC"));
    //    FullPlateTypeList.Add(new PlateType(45, "North Dakota Old", StaticStrings.NorthYanktonStateID, 3, "123 ABC"));
    //    FullPlateTypeList.Add(new PlateType(46, "Michigan", "Misquakewan", 3, "ABC 1234"));
    //    FullPlateTypeList.Add(new PlateType(47, "Nothing", "Carcer City", 0));
    //    FullPlateTypeList.Add(new PlateType(48, "Alaska", "Tanadux", 3, "ABC 123"));
    //    FullPlateTypeList.Add(new PlateType(49, "Hawaii", "Haiateaa", 3, "ABC 123"));
    //    FullPlateTypeList.Add(new PlateType(50, "Nothing", StaticStrings.SanAndreasStateID, 0, "12ABC345"));
    //    FullPlateTypeList.Add(new PlateType(51, "Willsylvania", "Willsylvania", 3, "ABC  123"));
    //    FullPlateTypeList.Add(new PlateType(52, "Nothing", "None", 0));
    //    FullPlateTypeList.Add(new PlateType(53, "Nothing", "None", 0));
    //    FullPlateTypeList.Add(new PlateType(54, "Nothing", "None", 0));
    //    FullPlateTypeList.Add(new PlateType(55, "Nothing", "None", 0));
    //    FullPlateTypeList.Add(new PlateType(56, "Nothing", "None", 0));

    //    FullPlateTypeList.Add(new PlateType(57, "Nevada 2", "Robada", 10, "123-A45"));

    //    FullPlateTypeList.Add(new PlateType(58, "San Andreas Veteran", StaticStrings.SanAndreasStateID, 1, "12ABC345"));
    //    FullPlateTypeList.Add(new PlateType(59, "Nothing", "None", 0));
    //    FullPlateTypeList.Add(new PlateType(60, "Nothing", "None", 0));
    //    FullPlateTypeList.Add(new PlateType(61, "Nothing", "None", 0));
    //    FullPlateTypeList.Add(new PlateType(62, "Nothing", "None", 0));
    //    FullPlateTypeList.Add(new PlateType(63, "Nothing", "None", 0));
    //    FullPlateTypeList.Add(new PlateType(64, "New Austin 1", "New Austin", 3, "ABC  123"));
    //    FullPlateTypeList.Add(new PlateType(65, "New Austin 2", "New Austin", 3, "ABC  123"));

    //    FullPlateTypeManager.PlateTypeList = FullPlateTypeList;
    //    FullPlateTypeManager.VanityPlates = PlateTypeManager.VanityPlates.Copy();
    //    Serialization.SerializeParam(FullPlateTypeManager, "Plugins\\LosSantosRED\\AlternateConfigs\\AddOnPlates_Wildbrick142\\PlateTypes_AddOnPlates_Wildbrick142.xml");

    //}
}

