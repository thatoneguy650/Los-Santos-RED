using Rage;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ModDataFileManager
{
    public Agencies Agencies;
    public Crimes Crimes;
    public GameSaves GameSaves;
    public Gangs Gangs;
    public GangTerritories GangTerritories;
    public Interiors Interiors;
    public Intoxicants Intoxicants;
    public Jurisdictions Jurisdictions;
    public ModItems ModItems;
    public Names Names;
    public PlacesOfInterest PlacesOfInterest;
    public PlateTypes PlateTypes;
    public RadioStations RadioStations;
    public PedGroups RelationshipGroups;
    public Scenarios Scenarios;
    public Settings Settings;
    public ShopMenus ShopMenus;
    public Streets Streets;
    public Weapons Weapons;
    public Zones Zones;
    public Heads Heads;
    public DispatchableVehicles DispatchableVehicles;
    public DispatchablePeople DispatchablePeople;
    public IssueableWeapons IssueableWeapons;
    public Dances DanceList;
    public Gestures GestureList;
    public Speeches SpeechList;
    private PhysicalItems PhysicalItems;
    public Seats Seats;
    public Counties Counties;
    public WeatherForecasts WeatherForecasts;
    public ClothesNames ClothesNames;
    public LanguageStrings LanguageStrings;
    private WantedLevels WantedLevels;

    public ModDataFileManager()
    {

    }

    public void Setup()
    {
        SetupAlternateConfigs();
        Settings = new Settings();
        Settings.ReadConfig();
        GameFiber.Yield();


        Weapons = new Weapons();
        Weapons.ReadConfig();
        GameFiber.Yield();


        PhysicalItems = new PhysicalItems();
        PhysicalItems.ReadConfig();
        GameFiber.Yield();
        ModItems = new ModItems();
        ModItems.ReadConfig();
        ModItems.Setup(PhysicalItems, Weapons);
        GameFiber.Yield();
        ShopMenus = new ShopMenus();
        ShopMenus.ReadConfig();
        ShopMenus.Setup(ModItems);
        GameFiber.Yield();
        Counties = new Counties();
        Counties.ReadConfig();
        GameFiber.Yield();
        Zones = new Zones();
        Zones.ReadConfig();
        GameFiber.Yield();
        PlateTypes = new PlateTypes();
        PlateTypes.ReadConfig();
        GameFiber.Yield();
        Streets = new Streets();
        Streets.ReadConfig();
        

        GameFiber.Yield();
        Names = new Names();
        Names.ReadConfig();
        GameFiber.Yield();
        Heads = new Heads();
        Heads.ReadConfig();
        GameFiber.Yield();
        DispatchableVehicles = new DispatchableVehicles();
        DispatchableVehicles.ReadConfig();
        GameFiber.Yield();
        IssueableWeapons = new IssueableWeapons();
        IssueableWeapons.ReadConfig();
        GameFiber.Yield();
        DispatchablePeople = new DispatchablePeople();
        DispatchablePeople.ReadConfig();
        DispatchablePeople.Setup(IssueableWeapons);
        GameFiber.Yield();
        Agencies = new Agencies();
        Agencies.ReadConfig();
        Agencies.Setup(Heads, DispatchableVehicles, DispatchablePeople, IssueableWeapons);
        GameFiber.Yield();
        Gangs = new Gangs();
        Gangs.ReadConfig();
        Gangs.Setup(Heads, DispatchableVehicles, DispatchablePeople, IssueableWeapons);
        GameFiber.Yield();
        PlacesOfInterest = new PlacesOfInterest(ShopMenus, Gangs);
        PlacesOfInterest.ReadConfig();
        PlacesOfInterest.Setup();
        GameFiber.Yield();
        Jurisdictions = new Jurisdictions(Agencies);
        Jurisdictions.ReadConfig();
        GameFiber.Yield();
        GangTerritories = new GangTerritories(Gangs);
        GangTerritories.ReadConfig();
        GameFiber.Yield();
        RadioStations = new RadioStations();
        RadioStations.ReadConfig();
        GameFiber.Yield();
        RelationshipGroups = new PedGroups();
        RelationshipGroups.ReadConfig();
        GameFiber.Yield();
        Scenarios = new Scenarios();
        GameFiber.Yield();
        Crimes = new Crimes();
        Crimes.ReadConfig();
        GameFiber.Yield();
        GameSaves = new GameSaves();
        GameSaves.ReadConfig();
        GameFiber.Yield();
        Interiors = new Interiors();
        Interiors.ReadConfig();
        Intoxicants = new Intoxicants();
        Intoxicants.ReadConfig();
        GameFiber.Yield();
        DanceList = new Dances();
        DanceList.ReadConfig();
        GameFiber.Yield();
        GestureList = new Gestures();
        GestureList.ReadConfig();
        GameFiber.Yield();
        SpeechList = new Speeches();
        SpeechList.ReadConfig();
        GameFiber.Yield();
        Seats = new Seats();
        Seats.ReadConfig();
        GameFiber.Yield();
        WeatherForecasts = new WeatherForecasts();
        //WeatherForecasts.ReadConfig();

        GameFiber.Yield();
        ClothesNames = new ClothesNames();
        ClothesNames.DefaultConfig();
        GameFiber.Yield();

#if DEBUG
        WantedLevels = new WantedLevels();
        WantedLevels.ReadConfig();
        WantedLevels.Setup(Heads, DispatchableVehicles, DispatchablePeople, IssueableWeapons);
        GameFiber.Yield();
#endif 

        //LanguageStrings = new LanguageStrings();
        //LanguageStrings.DefaultConfig();
        //GameFiber.Yield();

    }
    private void SetupAlternateConfigs()
    {
        Directory.CreateDirectory("Plugins\\LosSantosRED\\AlternateConfigs");
        SetupAddonPlatesConfig();
        SetupEUP();
        SetupFullExpandedJurisdiction();
        SetupLosSantos2008();
        SetupLibertyCity();
        SetupSimple();
    }
    private void SetupAddonPlatesConfig()
    {
        Directory.CreateDirectory("Plugins\\LosSantosRED\\AlternateConfigs\\AddOnPlates_Wildbrick142");
        string Description = "PreMade config for 'Addon Plates' by Wildbrick 142's. Installation: https://www.gta5-mods.com/paintjobs/new-license-plates-add-on." + Environment.NewLine +
            "To use, copy the all of the .xml files from the AlternateConfigs\\AddOnPlates_Wildbrick142 folder into the top level LosSantosRED folder and restart the mod. You can leave the vanilla configs, alternate configs will be loaded first (if they exist)";
        File.WriteAllText("Plugins\\LosSantosRED\\AlternateConfigs\\AddOnPlates_Wildbrick142\\readme.txt", Description);
    }
    private void SetupEUP()
    {
        Directory.CreateDirectory("Plugins\\LosSantosRED\\AlternateConfigs\\EUP");
        string Description = "PreMade config for 'Emergency uniforms pack - Law & Order 8.3' by Alex_Ashford. Need some vehicles to match? The FullExpandedJurisdiction config includes the EUP uniforms along with vehicles for most lore friendly departments. If you have your own or just want to use vanilla, this is the config for you. " + Environment.NewLine + Environment.NewLine + Environment.NewLine +
            "EUP Installation: "
            + Environment.NewLine +
            "https://www.lcpdfr.com/downloads/gta5mods/character/8151-emergency-uniforms-pack-law-order/"
            + Environment.NewLine +
            "To use, copy all of the .xml files from the AlternateConfigs\\EUP folder into the top level LosSantosRED folder and restart the mod. You can leave the vanilla configs, alternate configs will be loaded first (if they exist)" + Environment.NewLine + Environment.NewLine +
            "";
        File.WriteAllText("Plugins\\LosSantosRED\\AlternateConfigs\\EUP\\readme.txt", Description);
    }
    private void SetupFullExpandedJurisdiction()
    {
        Directory.CreateDirectory("Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedJurisdiction");
        string Description = 
            "PreMade config for 'Emergency uniforms pack - Law & Order 8.3' by Alex_Ashford and expanded department liveries (RHPD, BCSO, LSIA, etc.) made by me for Yard1 & Lt.Caine's mapped default police vehicles." 
            + Environment.NewLine + 
            "These are all replace vehicles for police, police2, police3, sheriff, and sheriff2. No custom gameconfig should be required." 
            + Environment.NewLine + 
            "EUP Installation: "
            + Environment.NewLine + 
            "https://www.lcpdfr.com/downloads/gta5mods/character/8151-emergency-uniforms-pack-law-order/" 
            + Environment.NewLine +
            "Expanded Department Liveries Installation: " 
            + Environment.NewLine +
            "Copy greskfej folder to '\\mods\\update\\x64\\dlcpacks'" 
            + Environment.NewLine + 
            "Add" 
            + Environment.NewLine + 
            "<Item>dlcpacks:/greskfej/</Item>" 
            + Environment.NewLine + 
            "to the end of " 
            + Environment.NewLine + 
            "'\\mods\\update\\update.rpf\\common\\data\\dlclist.xml'" 
            + Environment.NewLine
            + Environment.NewLine +
            "NOTE: To set the correct handling of the Police Gresley"
            + Environment.NewLine +
            "Copy the sheriff entry from '\\mods\\update\\x64\\dlcpacks\\greskfej\\dlc.rpf\\data\\vehicles.meta'"
            + Environment.NewLine +
            "and overwrite the existing sheriff entry in '\\mods\\update\\update.rpf\\common\\data\\levels\\gta5\\vehicles.meta'"
            + Environment.NewLine
            + Environment.NewLine +

            "To use, copy all of the .xml files from the AlternateConfigs\\FullExpandedJurisdiction folder into the top level LosSantosRED folder and restart the mod. You can leave the vanilla configs, alternate configs will be loaded first (if they exist)" 
            + Environment.NewLine 
            + Environment.NewLine +
            "Model Credits:" 
            + Environment.NewLine +
            "Stanier - Model by Rockstar Games, UV-Map by LtMattJeter, template by Lt.Caine, mirror lights by Netman, lightbars, assembly and skins by Yard1" 
            + Environment.NewLine +
            "Buffalo - Model by Rockstar Games, UV-Map, template, lightbar, assembly and skins by Yard1." 
            + Environment.NewLine +
            "Interceptor - Base GTAV vehicle model made by Rockstar Games and modified by Lt.Caine. Vehicle body remapped by Lt.Caine. Yard1 for fixing vehicle glass collision issues." 
            + Environment.NewLine +
            "Granger - Base GTAV vehicle model made by Rockstar Games and modified by Lt.Caine. Vehicle body remapped by Lt.Caine. Yard1 for fixing vehicle glass collision issues" 
            + Environment.NewLine +
            "Gresley - Model by Rockstar Games, UV-Map, template, lightbar, assembly and skins by Yard1." 
            + Environment.NewLine +
            "";
        File.WriteAllText("Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedJurisdiction\\readme.txt", Description);




        //"Stanier (police)" + Environment.NewLine + 
        //   "Copy police_hi.yft, police+hi.ytd, police.ytd, and police.yft from AlternateConfigs\\FullExpandedJurisdiction\\Files to '\\mods\\update\\x64\\dlcpacks\\patchday3ng\\dlc.rpf\\x64\\levels\\gta5\\vehicles.rpf\\'" + Environment.NewLine +
        //"Buffalo (police2)" + Environment.NewLine + "" +
        //   "Copy police2_hi.yft, police2.ytd, police2.yft, and police2+hi.ytd from AlternateConfigs\\FullExpandedJurisdiction\\Files to '\\mods\\update\\x64\\dlcpacks\\patchday3ng\\dlc.rpf\\x64\\levels\\gta5\\vehicles.rpf\\'" + Environment.NewLine +
        //"Granger (sheriff2)" + Environment.NewLine + 
        //   "Copy sheriff2_hi.yft, sheriff2.ytd, sheriff2.yft, and sheriff2+hi.ytd from AlternateConfigs\\FullExpandedJurisdiction\\Files to '\\mods\\update\\x64\\dlcpacks\\patchday3ng\\dlc.rpf\\x64\\levels\\gta5\\vehicles.rpf\\'" + Environment.NewLine + Environment.NewLine +

        //"Interceptor (police3)" + Environment.NewLine + 
        //   "Copy police3_hi.yft, police3.ytd, police3.yft, and police3+hi.ytd from AlternateConfigs\\FullExpandedJurisdiction\\Files to '\\mods\\update\\x64\\dlcpacks\\patchday4ng\\dlc.rpf\\x64\\levels\\gta5\\vehicles.rpf\\'" + Environment.NewLine + Environment.NewLine +

        //"Gresley (police5)" + Environment.NewLine + 
        //   "Copy police5_hi.yft, police5.ytd, and police5.yft from AlternateConfigs\\FullExpandedJurisdiction\\Files to '\\mods\\update\\x64\\dlcpacks\\mpchristmas2\\dlc.rpf\\x64\\levels\\gta5\\vehicles.rpf\\'." + Environment.NewLine + 
        //   "Copy vehicles.meta from AlternateConfigs\\FullExpandedJurisdiction\\Files to '\\mods\\update\\x64\\dlcpacks\\mpchristmas2\\dlc.rpf\\common\\data\\levels\\gta5\\' and replace the existing entry." + Environment.NewLine + 
        //   "Copy carvariations.meta from AlternateConfigs\\FullExpandedJurisdiction\\Files to '\\mods\\update\\x64\\dlcpacks\\mpchristmas2\\dlc.rpf\\common\\data\\ and replace the existing entry'." + Environment.NewLine + Environment.NewLine + Environment.NewLine +

    }
    private void SetupLosSantos2008()
    {
        Directory.CreateDirectory("Plugins\\LosSantosRED\\AlternateConfigs\\LosSantos2008");
        string Description = "PreMade config for 2008 Los Santos. See what LS was like when we were in LS with Niko. Includes a custom vehiclemodelsets.meta and popgroups.ymt to give you a 2008 feel using existing GTA 5 vehicles. More to come!" + Environment.NewLine + Environment.NewLine +
        "Los Santos 2008 Installation: " + Environment.NewLine +
        "Copy popgroups.ymt from AlternateConfigs\\LosSantos2008\\Files to '\\mods\\update\\update.rpf\\x64\\levels\\gta5\\'" + Environment.NewLine +
        "Copy vehiclemodelsets.meta from AlternateConfigs\\LosSantos2008\\Files to '\\mods\\update\\update.rpf\\common\\data\\ai\\'" + Environment.NewLine +
        "To use, copy all of the .xml files from the AlternateConfigs\\LosSantos2008 folder into the top level LosSantosRED folder and restart the mod. You can leave the vanilla configs, alternate configs will be loaded first (if they exist)" + Environment.NewLine + Environment.NewLine +
        "";
        File.WriteAllText("Plugins\\LosSantosRED\\AlternateConfigs\\LosSantos2008\\readme.txt", Description);
    }
    private void SetupLibertyCity()
    {
        Directory.CreateDirectory("Plugins\\LosSantosRED\\AlternateConfigs\\LibertyCity");
        string Description = "For use with liberty city (centered)";
        File.WriteAllText("Plugins\\LosSantosRED\\AlternateConfigs\\LibertyCity\\readme.txt", Description);
    }
    private void SetupSimple()
    {
        Directory.CreateDirectory("Plugins\\LosSantosRED\\AlternateConfigs\\Simple");
        string Description = "Simple and basic jurisdiction config, vanilla units and gangs only. Basic ped/vehicle models only." + Environment.NewLine + Environment.NewLine +
        "To use, copy all of the .xml files from the AlternateConfigs\\Simple folder into the top level LosSantosRED folder and restart the mod. You can leave the vanilla configs, alternate configs will be loaded first (if they exist)" + Environment.NewLine + Environment.NewLine +
        "";
        File.WriteAllText("Plugins\\LosSantosRED\\AlternateConfigs\\Simple\\readme.txt", Description);
    }
}

