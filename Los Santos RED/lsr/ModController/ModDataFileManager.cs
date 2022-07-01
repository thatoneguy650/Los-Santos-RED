﻿using Rage;
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

    public ModDataFileManager()
    {
    }

    public void Setup()
    {
        SetupAlternateConfigs();

        Settings = new Settings();
        Settings.ReadConfig();
        GameFiber.Yield();
        ModItems = new ModItems();
        ModItems.ReadConfig();
        GameFiber.Yield();
        ShopMenus = new ShopMenus();
        ShopMenus.ReadConfig();
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
        Weapons = new Weapons();
        Weapons.ReadConfig();
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
        DispatchablePeople = new DispatchablePeople();
        DispatchablePeople.ReadConfig();
        GameFiber.Yield();
        IssueableWeapons = new IssueableWeapons();
        IssueableWeapons.ReadConfig();
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
    }
    private void SetupAlternateConfigs()
    {
        Directory.CreateDirectory("Plugins\\LosSantosRED\\AlternateConfigs");
        SetupAddonPlatesConfig();
        SetupEUPAndLiveries();
    }
    private void SetupAddonPlatesConfig()
    {
        Directory.CreateDirectory("Plugins\\LosSantosRED\\AlternateConfigs\\AddOnPlates_Wildbrick142");
        string Description = "PreMade config for 'Addon Plates' by Wildbrick 142's. Installation: https://www.gta5-mods.com/paintjobs/new-license-plates-add-on." + Environment.NewLine +
            "To use, copy the all of the .xml files from the AlternateConfigs\\AddOnPlates_Wildbrick142 folder into the top level LosSantosRED folder and restart the mod";
        File.WriteAllText("Plugins\\LosSantosRED\\AlternateConfigs\\AddOnPlates_Wildbrick142\\readme.txt", Description);
    }
    private void SetupEUPAndLiveries()
    {
        Directory.CreateDirectory("Plugins\\LosSantosRED\\AlternateConfigs\\EUPBasicPedsAndExpandedJurisdictionLiveries");
        string Description = "PreMade config for 'Emergency uniforms pack - Law & Order 8.3' by Alex_Ashford and expanded department liveries (RHPD, BCSO, LSIA, etc.) made by me for Yard1 & Lt.Caine's mapped default police vehicles. No gameconfig changes are needed, as these are mostly replace." + Environment.NewLine + Environment.NewLine + Environment.NewLine +
            "EUP Installation: https://www.lcpdfr.com/downloads/gta5mods/character/8151-emergency-uniforms-pack-law-order/" + Environment.NewLine + Environment.NewLine +
           "Expanded Department Liveries Installation: " + Environment.NewLine +
            "Stanier (police) - Copy police_hi.yft, police+hi.ytd, police.ytd, and police.yft to '\\mods\\x64e.rpf\\levels\\gta5\\vehicles.rpf'" + Environment.NewLine +
            "Buffalo (police2) - Copy police2_hi.yft, police2.ytd, police2.yft, and police2+hi.ytd to '\\mods\\update\\x64\\dlcpacks\\patchday3ng\\dlc.rpf\\x64\\levels\\gta5\\vehicles.rpf\\'" + Environment.NewLine +
            "Interceptor (police3) - Copy police3_hi.yft, police3.ytd, police3.yft, and police3+hi.ytd to '\\mods\\update\\x64\\dlcpacks\\patchday4ng\\dlc.rpf\\x64\\levels\\gta5\\vehicles.rpf\\'" + Environment.NewLine +
            "Granger (sheriff2) - Copy sheriff2_hi.yft, sheriff2.ytd, sheriff2.yft, and sheriff2+hi.ytd to '\\mods\\update\\x64\\dlcpacks\\patchday3ng\\dlc.rpf\\x64\\levels\\gta5\\vehicles.rpf\\'" + Environment.NewLine + Environment.NewLine +
            "To use, copy all of the .xml files from the AlternateConfigs\\EUPBasicPedsAndExpandedJurisdictionLiveries folder into the top level LosSantosRED folder and restart the mod" + Environment.NewLine + Environment.NewLine +
            "Model Credits:" + Environment.NewLine +
            "Stanier - Model by Rockstar Games, UV-Map by LtMattJeter, template by Lt.Caine, mirror lights by Netman, lightbars, assembly and skins by Yard1" + Environment.NewLine +
            "Buffalo - Model by Rockstar Games, UV-Map, template, lightbar, assembly and skins by Yard1." + Environment.NewLine +
            "Interceptor - Base GTAV vehicle model made by Rockstar Games and modified by Lt.Caine. Vehicle body remapped by Lt.Caine. Yard1 for fixing vehicle glass collision issues." + Environment.NewLine +
            "Granger - Base GTAV vehicle model made by Rockstar Games and modified by Lt.Caine. Vehicle body remapped by Lt.Caine. Yard1 for fixing vehicle glass collision issues" + Environment.NewLine +
            "";
        File.WriteAllText("Plugins\\LosSantosRED\\AlternateConfigs\\EUPBasicPedsAndExpandedJurisdictionLiveries\\readme.txt", Description);
    }
}
