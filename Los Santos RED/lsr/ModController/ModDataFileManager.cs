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
    public PhysicalItems PhysicalItems;
    public Seats Seats;
    public LocationTypes LocationTypes;
    public WeatherForecasts WeatherForecasts;
    public ClothesNames ClothesNames;
    public LanguageStrings LanguageStrings;
    public WantedLevels WantedLevels;
    public TattooNames TattooNames;
    public SavedOutfits SavedOutfits;
    public VehicleSeatAndDoorLookup VehicleSeatDoorData;
    public Organizations Organizations;
    public Cellphones Cellphones;
    public Contacts Contacts;
    public TestAnimations TestAnimations;
    public SpawnBlocks SpawnBlocks;

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
        Intoxicants = new Intoxicants();
        Intoxicants.ReadConfig();
        GameFiber.Yield();

        Cellphones = new Cellphones();
        Cellphones.ReadConfig();
        GameFiber.Yield();

        ModItems = new ModItems();
        ModItems.ReadConfig();
        ModItems.Setup(PhysicalItems, Weapons, Intoxicants, Cellphones);
        GameFiber.Yield();
        ShopMenus = new ShopMenus();
        ShopMenus.ReadConfig();
        ShopMenus.Setup(ModItems);
        GameFiber.Yield();
        LocationTypes = new LocationTypes();
        LocationTypes.ReadConfig();
        GameFiber.Yield();
        Zones = new Zones();
        Zones.ReadConfig();
        Zones.Setup(LocationTypes);
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
        DispatchableVehicles.Setup();
        GameFiber.Yield();
        IssueableWeapons = new IssueableWeapons();
        IssueableWeapons.ReadConfig();
        GameFiber.Yield();
        DispatchablePeople = new DispatchablePeople();
        DispatchablePeople.ReadConfig();
        DispatchablePeople.Setup(IssueableWeapons);
        GameFiber.Yield();

        Contacts = new Contacts();
        Contacts.ReadConfig();
        GameFiber.Yield();


        Agencies = new Agencies();
        Agencies.ReadConfig();
        Agencies.Setup(Heads, DispatchableVehicles, DispatchablePeople, IssueableWeapons);
        GameFiber.Yield();
        Gangs = new Gangs();
        Gangs.ReadConfig();
        Gangs.Setup(Heads, DispatchableVehicles, DispatchablePeople, IssueableWeapons, Contacts);
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
        Gangs.CheckTerritory(GangTerritories);
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

        TattooNames = new TattooNames();
        TattooNames.DefaultConfig();
        GameFiber.Yield();


#if DEBUG
        WantedLevels = new WantedLevels();
        WantedLevels.ReadConfig();
        WantedLevels.Setup(Heads, DispatchableVehicles, DispatchablePeople, IssueableWeapons);
        GameFiber.Yield();




#endif

        TestAnimations = new TestAnimations();
        TestAnimations.ReadConfig();

        //LanguageStrings = new LanguageStrings();
        //LanguageStrings.DefaultConfig();
        //GameFiber.Yield();

        SavedOutfits = new SavedOutfits();
        SavedOutfits.ReadConfig();
        GameFiber.Yield();


        VehicleSeatDoorData = new VehicleSeatAndDoorLookup();
        VehicleSeatDoorData.ReadConfig();
        GameFiber.Yield();

        Organizations = new Organizations();
        Organizations.ReadConfig();
        Organizations.Setup(Heads, DispatchableVehicles, DispatchablePeople, IssueableWeapons, Contacts);
        GameFiber.Yield();

        Contacts.Setup(Organizations);

        SpawnBlocks = new SpawnBlocks();
        SpawnBlocks.ReadConfig();
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
        SetupFullModernTraffic();
        SetupFullExpandedExperience();
    }

    private void SetupFullExpandedExperience()
    {
        //Directory.CreateDirectory("Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedExperience");
        string Description =
            "Full Expanded Experience"
            + Environment.NewLine +
            "";
        //File.WriteAllText("Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedExperience\\readme.txt", Description);
    }

    private void SetupFullModernTraffic()
    {
        Directory.CreateDirectory("Plugins\\LosSantosRED\\AlternateConfigs\\FullModernTraffic");
        string Description = "Adds DLC vehicles to the traffic by modifying the popgroups.ymt file. Adds most normal vehicles to the corresponding traffic. Works with MP or SP map (traffic groups are identical). Install the greskfullmoderntraffic.oiv file. Incompatible with Los Santos 2008 config.";
        File.WriteAllText("Plugins\\LosSantosRED\\AlternateConfigs\\FullModernTraffic\\readme.txt", Description);
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
        string Description = "PreMade config for 'Emergency uniforms pack - Law & Order 8.3'and 'Emergency uniforms pack - Serve and Rescue' by Alex_Ashford. Need some vehicles to match? The FullExpandedJurisdiction config includes the EUP uniforms along with vehicles for most lore friendly departments. If you have your own or just want to use vanilla, this is the config for you. " + Environment.NewLine + Environment.NewLine + Environment.NewLine +
            "EUP Installation: "
            + Environment.NewLine +
            "LSR FEJ Requires BOTH EUP Base and EUP Serve and Rescue"
            + Environment.NewLine +
            "1. Install EUP Base. Follow the instructions at https://www.lcpdfr.com/downloads/gta5mods/character/8151-emergency-uniforms-pack-law-order/"
            + Environment.NewLine +
            "2. Install EUP Serve and Rescue. Follow the instructions at https://www.lcpdfr.com/downloads/gta5mods/character/16256-emergency-uniforms-pack-serve-rescue/"
            + Environment.NewLine +
            "To use, copy all of the .xml files from the AlternateConfigs\\EUP folder into the top level LosSantosRED folder and restart the mod. You can leave the vanilla configs, alternate configs will be loaded first (if they exist)" + Environment.NewLine + Environment.NewLine +
            "";
        File.WriteAllText("Plugins\\LosSantosRED\\AlternateConfigs\\EUP\\readme.txt", Description);
    }
    private void SetupFullExpandedJurisdiction()
    {
        Directory.CreateDirectory("Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedJurisdiction");
        Directory.CreateDirectory("Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedJurisdiction\\Variations");
        string Description =
            "PreMade config for 'Emergency uniforms pack - Law & Order 8.3' and 'Emergency uniforms pack - Serve and Rescue' by Alex_Ashford and expanded department liveries (RHPD, BCSO, LSIA, etc.) made by me for Yard1, Lt.Caine, AllenKennedy, and my own mapped default police/service vehicles. Looking for modern only? Try the Modern vehicle config in the Variations subfolder "
            + Environment.NewLine +
            "These vehicles are all replace, open the dlc.rpf file to see modelnames"
            + Environment.NewLine + Environment.NewLine +
            "EUP Installation: "
            + Environment.NewLine +
            "LSR FEJ Requires BOTH EUP Base and EUP Serve and Rescue"
            + Environment.NewLine +
            "1. Install EUP Base. Follow the instructions at https://www.lcpdfr.com/downloads/gta5mods/character/8151-emergency-uniforms-pack-law-order/"
            + Environment.NewLine +
            "2. Install EUP Serve and Rescue. Follow the instructions at https://www.lcpdfr.com/downloads/gta5mods/character/16256-emergency-uniforms-pack-serve-rescue/"
            + Environment.NewLine + Environment.NewLine +
            "Expanded Department Liveries Installation: "
            + Environment.NewLine +
            "1. Install the greskfejinstaller.oiv with OpenIV"
            + Environment.NewLine +
            "2. Copy all of the .xml files from the AlternateConfigs\\FullExpandedJurisdiction folder into the top level LosSantosRED folder and restart the mod. You can leave the vanilla configs, alternate configs will be loaded first (if they exist)"
            + Environment.NewLine + Environment.NewLine + Environment.NewLine +
            ""
            + Environment.NewLine + Environment.NewLine + Environment.NewLine +
            "Model Credits:"
            + Environment.NewLine +
            "Stanier - Model by Rockstar Games, UV-Map by LtMattJeter, template by Lt.Caine, mirror lights by Netman, lightbars, assembly and skins by Yard1"
            + Environment.NewLine +
            "Buffalo - Model by Rockstar Games, UV-Map, template, lightbar, assembly and skins by Yard1."
            + Environment.NewLine +
            "Granger - Base GTAV vehicle model made by Rockstar Games and modified by Lt.Caine. Vehicle body remapped by Lt.Caine. Yard1 for fixing vehicle glass collision issues"
            + Environment.NewLine +
            "Gresley - Model by Rockstar Games, UV-Map, template, lightbar, assembly and skins by Yard1."
            + Environment.NewLine +
            "Police Bike Retro - Model: Rockstar Games. Mapping: AllenKennedy"
            + Environment.NewLine +
            "Merit - Model by Rockstar Games, converted to GTA V by _CP_, HQ interior by _CP_, template by Lt.Caine, UV-Map, thin LED lightbar, assembly and skins by Yard1."
            + Environment.NewLine +
            "Bison - Model by Rockstar Games; UV-Map, template, and assembly by Yard1."
            + Environment.NewLine + Environment.NewLine +
            "Creator Links"
            + Environment.NewLine +
            "Yard1 - https://www.gta5-mods.com/users/Yard1/files"
            + Environment.NewLine +
            "Lt.Caine - https://www.lcpdfr.com/profile/143082-ltcaine/content/?type=downloads_file"
            + Environment.NewLine +
            "AllenKennedy - https://www.gta5-mods.com/users/AllenKennedy/files"
            + Environment.NewLine +
            "";
        File.WriteAllText("Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedJurisdiction\\readme.txt", Description);
    }
    private void SetupLosSantos2008()
    {
        string Description =
            "See what San Andreas was like in 2008 when we were in Liberty City with Niko. Includes customized traffic, phones, police, and gangs."
            + Environment.NewLine + Environment.NewLine +
            "1. Navigate to the Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedJurisdiction\\Files folder and install the greskfejinstaller.oiv with OpenIV (Vehicles from the Full Expanded Jurisdiction config are used in this config)"
            + Environment.NewLine +
            "2. Navigate to the Plugins\\LosSantosRED\\AlternateConfigs\\LosSantos2008 folder and install the gresk2008installer.oiv with OpenIV"
            + Environment.NewLine +
            "3. Copy all of the .xml files from the AlternateConfigs\\LosSantos2008 folder into the top level LosSantosRED folder and restart the mod. You can leave the vanilla configs, alternate configs will be loaded first (if they exist)"
            + Environment.NewLine + Environment.NewLine + Environment.NewLine +
            ""
            + Environment.NewLine + Environment.NewLine + Environment.NewLine +
            "Model Credits:"
            + Environment.NewLine +
            "Stanier - Model by Rockstar Games, UV-Map by LtMattJeter, template by Lt.Caine, mirror lights by Netman, lightbars, assembly and skins by Yard1"
            + Environment.NewLine +
            "Buffalo - Model by Rockstar Games, UV-Map, template, lightbar, assembly and skins by Yard1."
            + Environment.NewLine +
            "Granger - Base GTAV vehicle model made by Rockstar Games and modified by Lt.Caine. Vehicle body remapped by Lt.Caine. Yard1 for fixing vehicle glass collision issues"
            + Environment.NewLine +
            "Police Bike Retro - Model: Rockstar Games. Mapping: AllenKennedy"
            + Environment.NewLine +
            "Merit - Model by Rockstar Games, converted to GTA V by _CP_, HQ interior by _CP_, template by Lt.Caine, UV-Map, thin LED lightbar, assembly and skins by Yard1."
            + Environment.NewLine + Environment.NewLine +
            "Creator Links"
            + Environment.NewLine +
            "Yard1 - https://www.gta5-mods.com/users/Yard1/files"
            + Environment.NewLine +
            "Lt.Caine - https://www.lcpdfr.com/profile/143082-ltcaine/content/?type=downloads_file"
            + Environment.NewLine +
            "AllenKennedy - https://www.gta5-mods.com/users/AllenKennedy/files"
            + Environment.NewLine +
            "";
        File.WriteAllText("Plugins\\LosSantosRED\\AlternateConfigs\\LosSantos2008\\readme.txt", Description);
    }
    private void SetupLibertyCity()
    {
        Directory.CreateDirectory("Plugins\\LosSantosRED\\AlternateConfigs\\LibertyCity");
        string Description = "For use with Liberty City (centered). Not complete. Credit to box for the locations. The Full Expanded Jurisdiction config has some LCPD liveries if you need some vehicles.";
        File.WriteAllText("Plugins\\LosSantosRED\\AlternateConfigs\\LibertyCity\\readme.txt", Description);
    }
    private void SetupSimple()
    {
        Directory.CreateDirectory("Plugins\\LosSantosRED\\AlternateConfigs\\Simple");
        string Description = "Simple and basic jurisdiction config, vanilla units and gangs only. Basic ped/vehicle models only. If you want close to vanilla, this is for you." + Environment.NewLine + Environment.NewLine +
        "To use, copy all of the .xml files from the AlternateConfigs\\Simple folder into the top level LosSantosRED folder and restart the mod. You can leave the vanilla configs, alternate configs will be loaded first (if they exist)" + Environment.NewLine + Environment.NewLine +
        "";
        File.WriteAllText("Plugins\\LosSantosRED\\AlternateConfigs\\Simple\\readme.txt", Description);
    }
}

