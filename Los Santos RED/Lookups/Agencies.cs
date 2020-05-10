using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

public static class Agencies
{
    private static string ConfigFileName = "Plugins\\LosSantosRED\\Agencies.xml";
    public static List<Agency> AgenciesList { get; set; }

    public static void Initialize()
    {
        ReadConfig();
    }
    public static void Dispose()
    {

    }
    public static void ReadConfig()
    {
        if (File.Exists(ConfigFileName))
        {
            AgenciesList = LosSantosRED.DeserializeParams<Agency>(ConfigFileName);
        }
        else
        {
            DefaultConfig();
            LosSantosRED.SerializeParams(AgenciesList, ConfigFileName);
        }
    }
    private static void DefaultConfig()
    {

        //Peds
        List<Agency.ModelInformation> StandardCops = new List<Agency.ModelInformation>() {
            new Agency.ModelInformation("s_m_y_cop_01", true,85,85),
            new Agency.ModelInformation("s_f_y_cop_01", false,15,15) };
        List<Agency.ModelInformation> ExtendedStandardCops = new List<Agency.ModelInformation>() {
            new Agency.ModelInformation("s_m_y_cop_01", true,85,85),
            new Agency.ModelInformation("s_f_y_cop_01", false,10,10),
            new Agency.ModelInformation("ig_trafficwarden", true,5,5) };
        List<Agency.ModelInformation> ParkRangers = new List<Agency.ModelInformation>() {
            new Agency.ModelInformation("s_m_y_ranger_01", true,75,75),
            new Agency.ModelInformation("s_f_y_ranger_01", false,25,25) };
        List<Agency.ModelInformation> SheriffPeds = new List<Agency.ModelInformation>() {
            new Agency.ModelInformation("s_m_y_sheriff_01", true,75,75),
            new Agency.ModelInformation("s_f_y_sheriff_01", false,25,25) };
        List<Agency.ModelInformation> SWAT = new List<Agency.ModelInformation>() {
            new Agency.ModelInformation("s_m_y_swat_01", true, 100,100) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(10, 0, 0,0) },new List<PropComponent>() { new PropComponent(0, 0, 0) }) } };
        List<Agency.ModelInformation> PoliceAndSwat = new List<Agency.ModelInformation>() {
            new Agency.ModelInformation("s_m_y_cop_01", true,70,0),
            new Agency.ModelInformation("s_f_y_cop_01", false,30,0),
            new Agency.ModelInformation("s_m_y_swat_01", true, 0,100) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(10, 0, 0,0) },new List<PropComponent>() { new PropComponent(0, 0, 0) }) } };
        List<Agency.ModelInformation> SheriffAndSwat = new List<Agency.ModelInformation>() {
            new Agency.ModelInformation("s_m_y_sheriff_01", true, 75, 0),
            new Agency.ModelInformation("s_f_y_sheriff_01", false, 25, 0),
            new Agency.ModelInformation("s_m_y_swat_01", true, 0, 100) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(10, 0, 0,0) },new List<PropComponent>() { new PropComponent(0, 0, 0) }) } };
        List<Agency.ModelInformation> DOAPeds = new List<Agency.ModelInformation>() {
            new Agency.ModelInformation("u_m_m_doa_01", true,100,100) };
        List<Agency.ModelInformation> IAAPeds = new List<Agency.ModelInformation>() {
            new Agency.ModelInformation("s_m_m_fibsec_01", true,100,100) };
        List<Agency.ModelInformation> SAHPPeds = new List<Agency.ModelInformation>() {
            new Agency.ModelInformation("s_m_y_hwaycop_01", true,100,100) };
        List<Agency.ModelInformation> MilitaryPeds = new List<Agency.ModelInformation>() {
            new Agency.ModelInformation("s_m_y_armymech_01", true,25,0),
            new Agency.ModelInformation("s_m_m_marine_01", true,50,0),
            new Agency.ModelInformation("s_m_m_marine_02", true,0,0),
            new Agency.ModelInformation("s_m_y_marine_01", true,25,0),
            new Agency.ModelInformation("s_m_y_marine_02", true,0,0),
            new Agency.ModelInformation("s_m_y_marine_03", true,100,100) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(2, 1, 0, 0),new PedComponent(8, 0, 0, 0) },new List<PropComponent>() { new PropComponent(3, 1, 0) }) },
            new Agency.ModelInformation("s_m_m_pilot_02", true,0,0),
            new Agency.ModelInformation("s_m_y_pilot_01", true,0,0) };
        List<Agency.ModelInformation> FIBPeds = new List<Agency.ModelInformation>() {
            new Agency.ModelInformation("s_m_m_fibsec_01", true,55,70),
            new Agency.ModelInformation("s_m_m_fiboffice_01", true,15,0),
            new Agency.ModelInformation("s_m_m_fiboffice_02", true,15,0),
            new Agency.ModelInformation("u_m_m_fibarchitect", true,10,0),
            new Agency.ModelInformation("s_m_y_swat_01", true, 5,30) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(10, 0, 1,0) },new List<PropComponent>() { new PropComponent(0, 0, 0) }) } };
        List<Agency.ModelInformation> PrisonPeds = new List<Agency.ModelInformation>() {
            new Agency.ModelInformation("s_m_m_prisguard_01", true,100,100) };
        List<Agency.ModelInformation> SecurityPeds = new List<Agency.ModelInformation>() {
            new Agency.ModelInformation("s_m_m_security_01", true,100,100) };
        List<Agency.ModelInformation> CoastGuardPeds = new List<Agency.ModelInformation>() {
            new Agency.ModelInformation("s_m_y_uscg_01", true,100,100) };
        List<Agency.ModelInformation> NOOSEPeds = new List<Agency.ModelInformation>() {
            new Agency.ModelInformation("s_m_y_swat_01", true, 100,100) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(10, 0, 0,0) },new List<PropComponent>() { new PropComponent(0, 0, 0) }) } };

        //Vehicles
        List<Agency.VehicleInformation> UnmarkedVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("police4", 100, 100) };
        List<Agency.VehicleInformation> SecurityVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("dilettante2", 100, 100) {MaxOccupants = 1 } };

        List<Agency.VehicleInformation> ParkRangerVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("pranger", 100, 100) };
        List<Agency.VehicleInformation> FIBVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("fbi", 70, 70),
            new Agency.VehicleInformation("fbi2", 30, 30) };
        List<Agency.VehicleInformation> NOOSEVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("fbi", 70, 70) {MaxWantedLevelSpawn = 3 },
            new Agency.VehicleInformation("fbi2", 30, 30) {MaxWantedLevelSpawn = 3 },
            new Agency.VehicleInformation("riot", 0, 100) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 5, AllowedPedModels = new List<string>() { "s_m_y_swat_01" },MinOccupants = 2, MaxOccupants = 3 },
           // new Agency.VehicleInformation("riot2", 0, 45) { MinWantedLevelSpawn = 5, AllowedPedModels = new List<string>() { "s_m_y_swat_01" } },
            new Agency.VehicleInformation("annihilator", 0, 100) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 5, AllowedPedModels = new List<string>() { "s_m_y_swat_01" },IsHelicopter = true,MinOccupants = 3,MaxOccupants = 4 }};
        List<Agency.VehicleInformation> HighwayPatrolVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("policeb", 95, 95) { IsMotorcycle = true, MaxOccupants = 1 },
            new Agency.VehicleInformation("police4", 5, 5) };
        List<Agency.VehicleInformation> PrisonVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("policet", 70, 70),
            new Agency.VehicleInformation("police4", 30, 30) };
        List<Agency.VehicleInformation> LSPDVehiclesVanilla = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("police", 48,35) { Liveries = new List<int>() { 0,1,2,3,4,5 } },
            new Agency.VehicleInformation("police2", 25, 20) { Liveries = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 } },
            new Agency.VehicleInformation("police3", 25, 20) { Liveries = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 } },
            new Agency.VehicleInformation("police4", 1,1),
            new Agency.VehicleInformation("fbi2", 1,1),
            new Agency.VehicleInformation("policet", 0, 25) { MinWantedLevelSpawn = 3} };
        List<Agency.VehicleInformation> LSSDVehiclesVanilla = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("sheriff", 50, 50){ Liveries = new List<int> { 0, 1, 2, 3 } },
            new Agency.VehicleInformation("sheriff2", 50, 50) { Liveries = new List<int> { 0, 1, 2, 3 } } };
        List<Agency.VehicleInformation> LSPDVehicles = LSPDVehiclesVanilla;
        List<Agency.VehicleInformation> SAHPVehicles = HighwayPatrolVehicles;
        List<Agency.VehicleInformation> LSSDVehicles = LSSDVehiclesVanilla;
        List<Agency.VehicleInformation> BCSOVehicles = LSSDVehiclesVanilla;
        List<Agency.VehicleInformation> VWHillsLSSDVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("sheriff2", 100, 100) { Liveries = new List<int> { 0, 1, 2, 3 } } };
        List<Agency.VehicleInformation> ChumashLSSDVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("sheriff2", 100, 100) { Liveries = new List<int> { 0, 1, 2, 3 } } };
        List<Agency.VehicleInformation> LSSDDavisVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("sheriff", 100, 100){ Liveries = new List<int> { 0, 1, 2, 3 } } };
        List<Agency.VehicleInformation> RHPDVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("police2", 100, 75) { Liveries = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 } },
            new Agency.VehicleInformation("policet", 0, 25) { MinWantedLevelSpawn = 3} };
        List<Agency.VehicleInformation> DPPDVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("police3", 100, 75) { Liveries = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 } },
            new Agency.VehicleInformation("policet", 0, 25) { MinWantedLevelSpawn = 3} };
        List<Agency.VehicleInformation> ChumashLSPDVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("police3", 100, 75) { Liveries = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 } },
            new Agency.VehicleInformation("policet", 0, 25) { MinWantedLevelSpawn = 3} };
        List<Agency.VehicleInformation> EastLSPDVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("police", 100,75) { Liveries = new List<int>() { 0,1,2,3,4,5 } },
            new Agency.VehicleInformation("policet", 0, 25) { MinWantedLevelSpawn = 3} };
        List<Agency.VehicleInformation> VWPDVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("police", 100,75) { Liveries = new List<int>() { 0,1,2,3,4,5 } },
            new Agency.VehicleInformation("policet", 0, 25) { MinWantedLevelSpawn = 3} };
        List<Agency.VehicleInformation> PoliceHeliVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("polmav", 0,100) { Liveries = new List<int>() { 0 }, MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4,IsHelicopter = true,MinOccupants = 3,MaxOccupants = 3 } };
        List<Agency.VehicleInformation> SheriffHeliVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("buzzard2", 0,100) { Liveries = new List<int>() { 0 }, MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4,IsHelicopter = true,MinOccupants = 3,MaxOccupants = 3 } };
        List<Agency.VehicleInformation> ArmyVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("crusader", 75,50) { Liveries = new List<int>() { 0 }, IsHelicopter = false,MinOccupants = 1,MaxOccupants = 2,MaxWantedLevelSpawn = 4 },
            new Agency.VehicleInformation("barracks", 25,50) { Liveries = new List<int>() { 0 }, IsHelicopter = false,MinOccupants = 3,MaxOccupants = 5,MinWantedLevelSpawn = 4 },
            new Agency.VehicleInformation("rhino", 0,10) { Liveries = new List<int>() { 0 }, IsHelicopter = false,MinOccupants = 1,MaxOccupants = 2,MinWantedLevelSpawn = 5 },
            new Agency.VehicleInformation("valkyrie", 0,50) { Liveries = new List<int>() { 0 }, IsHelicopter = true,MinOccupants = 3,MaxOccupants = 3,MinWantedLevelSpawn = 4 },
            new Agency.VehicleInformation("valkyrie2", 0,50) { Liveries = new List<int>() { 0 }, IsHelicopter = true,MinOccupants = 3,MaxOccupants = 3,MinWantedLevelSpawn = 4 },
        };


        //Weapons
        List<Agency.IssuedWeapon> AllWeapons = new List<Agency.IssuedWeapon>()
        {
            // Pistols
            new Agency.IssuedWeapon("weapon_pistol", true, new GTAWeapon.WeaponVariation()),
            new Agency.IssuedWeapon("weapon_pistol", true, new GTAWeapon.WeaponVariation(0, new List<string> { "Flashlight" })),
            new Agency.IssuedWeapon("weapon_pistol", true, new GTAWeapon.WeaponVariation(0,new List<string> { "Extended Clip" })),
            new Agency.IssuedWeapon("weapon_pistol", true, new GTAWeapon.WeaponVariation(0,new List<string> { "Flashlight","Extended Clip" })),

            new Agency.IssuedWeapon("weapon_pistol_mk2", true ,new GTAWeapon.WeaponVariation()),
            new Agency.IssuedWeapon("weapon_pistol_mk2", true, new GTAWeapon.WeaponVariation(0,new List<string> { "Flashlight" })),
            new Agency.IssuedWeapon("weapon_pistol_mk2", true, new GTAWeapon.WeaponVariation(0,new List<string> { "Extended Clip" })),
            new Agency.IssuedWeapon("weapon_pistol_mk2", true, new GTAWeapon.WeaponVariation(0, new List<string> { "Flashlight","Extended Clip" })),

            new Agency.IssuedWeapon("weapon_combatpistol", true, new GTAWeapon.WeaponVariation()),
            new Agency.IssuedWeapon("weapon_combatpistol", true, new GTAWeapon.WeaponVariation(0, new List<string> { "Flashlight" })),
            new Agency.IssuedWeapon("weapon_combatpistol", true, new GTAWeapon.WeaponVariation(0,new List<string> { "Extended Clip" })),
            new Agency.IssuedWeapon("weapon_combatpistol", true, new GTAWeapon.WeaponVariation(0, new List<string> { "Flashlight","Extended Clip" })),

            new Agency.IssuedWeapon("weapon_heavypistol", true, new GTAWeapon.WeaponVariation()),
            new Agency.IssuedWeapon("weapon_heavypistol", true, new GTAWeapon.WeaponVariation(0,new List<string> { "Etched Wood Grip Finish" })),
            new Agency.IssuedWeapon("weapon_heavypistol", true, new GTAWeapon.WeaponVariation(0,new List<string> { "Flashlight","Extended Clip" })),
            new Agency.IssuedWeapon("weapon_heavypistol", true, new GTAWeapon.WeaponVariation(0,new List<string> { "Extended Clip" })),

            // Shotguns
            new Agency.IssuedWeapon("weapon_pumpshotgun", false, new GTAWeapon.WeaponVariation()),
            new Agency.IssuedWeapon("weapon_pumpshotgun", false, new GTAWeapon.WeaponVariation(0,new List<string> { "Flashlight" })),

            new Agency.IssuedWeapon("weapon_pumpshotgun_mk2", false, new GTAWeapon.WeaponVariation()),
            new Agency.IssuedWeapon("weapon_pumpshotgun_mk2", false, new GTAWeapon.WeaponVariation(0, new List<string> { "Flashlight" })),
            new Agency.IssuedWeapon("weapon_pumpshotgun_mk2", false, new GTAWeapon.WeaponVariation(0, new List<string> { "Holographic Sight" })),
            new Agency.IssuedWeapon("weapon_pumpshotgun_mk2", false, new GTAWeapon.WeaponVariation(0, new List<string> { "Flashlight","Holographic Sight" })),

            // ARs
            new Agency.IssuedWeapon("weapon_carbinerifle", false, new GTAWeapon.WeaponVariation()),
            new Agency.IssuedWeapon("weapon_carbinerifle", false, new GTAWeapon.WeaponVariation(0,new List<string> { "Grip","Flashlight" })),
            new Agency.IssuedWeapon("weapon_carbinerifle", false, new GTAWeapon.WeaponVariation(0, new List<string> { "Scope", "Grip","Flashlight" })),
            new Agency.IssuedWeapon("weapon_carbinerifle", false, new GTAWeapon.WeaponVariation(0,new List<string> { "Scope", "Grip","Flashlight","Extended Clip" })),

            new Agency.IssuedWeapon("weapon_carbinerifle_mk2", false, new GTAWeapon.WeaponVariation()),
            new Agency.IssuedWeapon("weapon_carbinerifle_mk2", false, new GTAWeapon.WeaponVariation(0, new List<string> { "Holographic Sight","Grip","Flashlight" })),
            new Agency.IssuedWeapon("weapon_carbinerifle_mk2", false, new GTAWeapon.WeaponVariation(0, new List<string> { "Holographic Sight", "Grip","Extended Clip" })),
            new Agency.IssuedWeapon("weapon_carbinerifle_mk2", false, new GTAWeapon.WeaponVariation(0, new List<string> { "Large Scope", "Grip","Flashlight","Extended Clip" })),
        };

        List<Agency.IssuedWeapon> BestWeapons = new List<Agency.IssuedWeapon>()
        {
            new Agency.IssuedWeapon("weapon_pistol_mk2", true, new GTAWeapon.WeaponVariation(0,new List<string> { "Flashlight" })),
            new Agency.IssuedWeapon("weapon_pistol_mk2", true, new GTAWeapon.WeaponVariation(0,new List<string> { "Extended Clip" })),
            new Agency.IssuedWeapon("weapon_pistol_mk2", true, new GTAWeapon.WeaponVariation(0, new List<string> { "Flashlight","Extended Clip" })),
            new Agency.IssuedWeapon("weapon_carbinerifle_mk2", false, new GTAWeapon.WeaponVariation(0, new List<string> { "Holographic Sight","Grip","Flashlight" })),
            new Agency.IssuedWeapon("weapon_carbinerifle_mk2", false, new GTAWeapon.WeaponVariation(0, new List<string> { "Holographic Sight", "Grip","Extended Clip" })),
            new Agency.IssuedWeapon("weapon_carbinerifle_mk2", false, new GTAWeapon.WeaponVariation(0, new List<string> { "Large Scope", "Grip","Flashlight","Extended Clip" })),
        };

        List<Agency.IssuedWeapon> LimitedWeapons = new List<Agency.IssuedWeapon>()
        {
            new Agency.IssuedWeapon("weapon_heavypistol", true, new GTAWeapon.WeaponVariation()),
            new Agency.IssuedWeapon("weapon_revolver", true, new GTAWeapon.WeaponVariation()),
            new Agency.IssuedWeapon("weapon_heavypistol", true, new GTAWeapon.WeaponVariation(0,new List<string> { "Flashlight" })),
            new Agency.IssuedWeapon("weapon_pumpshotgun", false, new GTAWeapon.WeaponVariation()),
            new Agency.IssuedWeapon("weapon_pumpshotgun", false, new GTAWeapon.WeaponVariation(0,new List<string> { "Flashlight" })),

        };

        AgenciesList = new List<Agency>
        {
            new Agency("~b~", "LSPD", "Los Santos Police Department", "Blue", Agency.Classification.Police, StandardCops, LSPDVehicles, "LS ",AllWeapons),
            new Agency("~b~", "LSPD-VW", "Los Santos Police - Vinewood Division", "Blue", Agency.Classification.Police, ExtendedStandardCops, VWPDVehicles, "LSV ",LimitedWeapons),
            new Agency("~b~", "LSPD-ELS", "Los Santos Police - East Los Santos Division", "Blue", Agency.Classification.Police, ExtendedStandardCops, EastLSPDVehicles, "LSE ",LimitedWeapons),
            new Agency("~b~", "LSPD-CH", "Los Santos Police - Chumash Division", "Blue", Agency.Classification.Police, StandardCops, ChumashLSPDVehicles, "LSC ",AllWeapons),
            new Agency("~b~", "LSPD-DP", "Los Santos Police - Del Pierro Division", "DarkBlue", Agency.Classification.Police, StandardCops, DPPDVehicles, "VP ",AllWeapons),
            new Agency("~b~", "LSPD-RH", "Los Santos Police - Rockford Hills Division", "LightBlue", Agency.Classification.Police, StandardCops, RHPDVehicles, "RH ",AllWeapons),        

            new Agency("~b~", "LSPD-ASD", "Los Santos Police Department - Air Support Division", "White", Agency.Classification.Police, PoliceAndSwat, PoliceHeliVehicles, "ASD ",BestWeapons) { MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4 },
         
            new Agency("~r~", "LSSD", "Los Santos County Sheriff", "Red", Agency.Classification.Sheriff, SheriffPeds, LSSDVehicles, "LSCS ",LimitedWeapons),
            new Agency("~r~", "LSSD-VW", "Los Santos Sheriff - Vinewood Division", "Red", Agency.Classification.Sheriff, SheriffPeds, VWHillsLSSDVehicles, "LSCS ",LimitedWeapons),
            new Agency("~r~", "LSSD-CH", "Los Santos Sheriff - Chumash Division", "Red", Agency.Classification.Sheriff, SheriffPeds, ChumashLSSDVehicles, "LSCS ",LimitedWeapons),
            new Agency("~r~", "LSSD-BC", "Los Santos Sheriff - Blaine County Division", "Red", Agency.Classification.Sheriff, SheriffPeds, BCSOVehicles, "BCS ",LimitedWeapons),
            new Agency("~r~", "LSSD-DV", "Los Santos Sheriff - Davis Division", "Red", Agency.Classification.Sheriff, SheriffPeds, LSSDDavisVehicles, "LSCS ",LimitedWeapons),


            new Agency("~r~", "LSSD-ASD", "Los Santos Sheriffs Department - Air Support Division", "White", Agency.Classification.Sheriff, SheriffAndSwat, SheriffHeliVehicles, "ASD ",BestWeapons) { MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4 },

            new Agency("~r~", "NOOSE", "National Office of Security Enforcement", "DarkRed", Agency.Classification.Federal, NOOSEPeds, NOOSEVehicles, "",BestWeapons) { MinWantedLevelSpawn = 3 },
            new Agency("~p~", "FIB", "Federal Investigation Bureau", "Purple", Agency.Classification.Federal, FIBPeds, FIBVehicles, "FIB ",BestWeapons),
            new Agency("~p~", "DOA", "Drug Observation Agency", "Purple", Agency.Classification.Federal, DOAPeds, UnmarkedVehicles, "DOA ",AllWeapons) {SpawnLimit = 4 },
            new Agency("~o~", "SASPA", "San Andreas State Prison Authority", "Orange", Agency.Classification.Other, PrisonPeds, PrisonVehicles, "SASPA ",AllWeapons),
            new Agency("~y~", "SAHP", "San Andreas Highway Patrol", "Yellow", Agency.Classification.Police, SAHPPeds, SAHPVehicles, "HP ",LimitedWeapons) {SpawnsOnHighway = true },
            new Agency("~g~", "SAPR", "San Andreas Park Ranger", "Green", Agency.Classification.Federal, ParkRangers, ParkRangerVehicles, "",AllWeapons) {MaxWantedLevelSpawn = 3,SpawnLimit = 3 },      
            new Agency("~p~", "IAA", "International Affairs Agency", "Purple", Agency.Classification.Federal, IAAPeds, UnmarkedVehicles, "IAA ",AllWeapons),

            new Agency("~p~", "LSIAPD", "Los Santos International Airport Police Department", "LightBlue", Agency.Classification.Police, StandardCops, LSPDVehicles, "LSA ",AllWeapons) { SpawnLimit = 3 },
            new Agency("~o~", "PRISEC", "Private Security", "White", Agency.Classification.Security, SecurityPeds, SecurityVehicles, "",LimitedWeapons) {MaxWantedLevelSpawn = 3,SpawnLimit = 3 },
            new Agency("~p~", "LSPA", "Port Authority of Los Santos", "LightGray", Agency.Classification.Security, SecurityPeds, UnmarkedVehicles, "LSPA ",LimitedWeapons) {MaxWantedLevelSpawn = 3,SpawnLimit = 3 },
            new Agency("~o~", "SACG", "San Andreas Coast Guard", "DarkOrange", Agency.Classification.Other, CoastGuardPeds, UnmarkedVehicles, "SACG ",LimitedWeapons){MaxWantedLevelSpawn = 3,SpawnLimit = 3 },

            new Agency("~u~", "ARMY", "Army", "Black", Agency.Classification.Federal, MilitaryPeds, ArmyVehicles, "",BestWeapons) {IsArmy = true },

            new Agency("~s~", "UNK", "Unknown Agency", "White", Agency.Classification.Other, null, null, "",null),
        };
      
    }
    public static Agency GetAgencyFromPed(Ped Cop)
    {
        if (!Cop.IsPoliceArmy())
            return null;
        if (Cop.IsArmy())
            return AgenciesList.Where(x => x.IsArmy).FirstOrDefault();
        else if (Cop.IsPolice())
            return GetPedAgencyFromZone(Cop);
        else
            return null;
    }
    public static Agency GetRandomHighwayAgency()
    {
        return AgenciesList.Where(x => x.CanSpawn && x.SpawnsOnHighway).PickRandom();
    }
    public static Agency GetRandomArmyAgency()
    {
        return AgenciesList.Where(x => x.CanSpawn && x.IsArmy).PickRandom();
    }
    private static Agency GetPedAgencyFromZone(Ped Cop)
    {
        Zone ZoneFound = Zones.GetZoneAtLocation(Cop.Position);
        Agency ZoneAgency = null;
        if (ZoneFound != null)
        {
            foreach (ZoneAgency MyAgency in ZoneFound.ZoneAgencies)
            {
                if (MyAgency.AssociatedAgency != null && MyAgency.AssociatedAgency.CopModels != null && MyAgency.AssociatedAgency.CopModels.Any())
                {
                    if (MyAgency.AssociatedAgency.CopModels.Any(x => x.ModelName.ToLower() == Cop.Model.Name.ToLower()))
                    {
                        ZoneAgency = MyAgency.AssociatedAgency;
                        break;
                    }
                }
            }
        }

        if (ZoneAgency == null)
        {
            ZoneAgency = AgenciesList.Where(x => x.CopModels != null && x.CopModels.Any(y => y.ModelName.ToLower() == Cop.Model.Name.ToLower()) && x.SpawnsOnHighway).PickRandom();
            if (ZoneAgency == null)
            {
                Debugging.WriteToLog("GetPedAgencyFromZone", string.Format("Couldnt get agency from zone {0} ped {1}", ZoneFound.TextName, Cop.Model.Name));
                Cop.Delete();
            }
        }
        return ZoneAgency;
    }
    public static void ChangeLivery(Vehicle CopCar, Agency AssignedAgency)
    {
        Agency.VehicleInformation MyVehicle = null;
        if (AssignedAgency != null && AssignedAgency.Vehicles != null)
        {
            MyVehicle = AssignedAgency.Vehicles.Where(x => x.ModelName.ToLower() == CopCar.Model.Name.ToLower()).FirstOrDefault();
        }
        if (MyVehicle == null)
        {
            Debugging.WriteToLog("ChangeLivery", string.Format("No Match for Vehicle {0} for {1}", CopCar.Model.Name, AssignedAgency.Initials));
            CopCar.Delete();
            return;
        }
        if (MyVehicle.Liveries != null && MyVehicle.Liveries.Any())
        {
            //Debugging.WriteToLog("ChangeLivery", string.Format("Agency {0}, {1}, {2}", AssignedAgency.Initials, CopCar.Model.Name,string.Join(",", MyVehicle.Liveries.Select(x => x.ToString()))));
            int NewLiveryNumber = MyVehicle.Liveries.PickRandom();
            NativeFunction.CallByName<bool>("SET_VEHICLE_LIVERY", CopCar, NewLiveryNumber);
        }
        CopCar.LicensePlate = AssignedAgency.LicensePlatePrefix + LosSantosRED.RandomString(8 - AssignedAgency.LicensePlatePrefix.Length);
    }
    public static void ChangeLiveryAtZone(Vehicle CopCar,Zone ZoneFound)
    {
        Agency.VehicleInformation MyVehicle = null;
        Agency ZoneAgency = null;
        if (ZoneFound != null)
        {
            foreach (ZoneAgency MyAgency in ZoneFound.ZoneAgencies)
            {
                if (MyAgency.AssociatedAgency != null && MyAgency.AssociatedAgency.Vehicles != null && MyAgency.AssociatedAgency.Vehicles.Any())
                {
                    if (MyAgency.AssociatedAgency.Vehicles.Any(x => x.ModelName == CopCar.Model.Name.ToLower()))
                    {
                        ZoneAgency = MyAgency.AssociatedAgency;
                        break;
                    }
                }
            }
            if (ZoneAgency != null && ZoneAgency.Vehicles != null)
            {
                MyVehicle = ZoneAgency.Vehicles.Where(x => x.ModelName.ToLower() == CopCar.Model.Name.ToLower()).FirstOrDefault();
            }
        }
        if (MyVehicle == null || ZoneAgency == null)
        {
            Debugging.WriteToLog("CheckandChangeLivery", string.Format("No Match for Vehicle {0} at {1}", CopCar.Model.Name, ZoneFound.TextName));
            CopCar.Delete();
            return;
        }
        if (MyVehicle.Liveries != null && MyVehicle.Liveries.Any())
        {
            int NewLiveryNumber = MyVehicle.Liveries.PickRandom();
            NativeFunction.CallByName<bool>("SET_VEHICLE_LIVERY", CopCar, NewLiveryNumber);
        }
        if(ZoneAgency != null)
        {
            CopCar.LicensePlate = ZoneAgency.LicensePlatePrefix + LosSantosRED.RandomString(8 - ZoneAgency.LicensePlatePrefix.Length);
        }     
    }
 }
[Serializable()]
public class Agency
{
    public string ColorPrefix { get; set; } = "~s~";
    public string Initials { get; set; }
    public string FullName { get; set; }
    public List<ModelInformation> CopModels { get; set; }
    public List<VehicleInformation> Vehicles { get; set; }
    public string AgencyColorString { get; set; } = "White";
    public Classification AgencyClassification { get; set; }
    public string LicensePlatePrefix { get; set; }
    public bool SpawnsOnHighway { get; set; } = false;
    public bool IsArmy { get; set; } = false;
    public uint MinWantedLevelSpawn { get; set; } = 0;
    public uint MaxWantedLevelSpawn { get; set; } = 5;
    public int SpawnLimit { get; set; } = 99;
    public List<IssuedWeapon> IssuedWeapons { get; set; } = new List<IssuedWeapon>();

    public bool CanSpawn
    {
        get
        {
            if (LosSantosRED.PlayerWantedLevel >= MinWantedLevelSpawn && LosSantosRED.PlayerWantedLevel <= MaxWantedLevelSpawn)
            {
                if (PedList.CopPeds.Count(x => x.AssignedAgency == this) < SpawnLimit)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
    public bool HasMotorcycles
    {
        get
        {
            return Vehicles.Any(x => x.IsMotorcycle);
        }
    }
    public bool HasSpawnableHelicopters
    {
        get
        {
            return Vehicles.Any(x => x.IsHelicopter && x.CanCurrentlySpawn);
        }
    }
    public Color AgencyColor
    {
        get
        {
            return Color.FromName(AgencyColorString);
        }
    }
    public string ColoredInitials
    {
        get
        {
            return ColorPrefix + Initials;
        }
    }
    public bool CanCheckTrafficViolations
    {
        get
        {
            if (AgencyClassification == Classification.Police || AgencyClassification == Classification.Federal || AgencyClassification == Classification.Sheriff)
                return true;
            else
                return false;
        }
    }
    public enum Classification
    {
        Police = 0,
        Sheriff = 1,
        Federal = 2,
        Security = 3,
        Other = 4,
    }
    public VehicleInformation GetVehicleInfo(Vehicle CopCar)
    {
        return Vehicles.Where(x => x.ModelName.ToLower() == CopCar.Model.Name.ToLower()).FirstOrDefault();
    }
    public VehicleInformation GetRandomVehicle(bool IsMotorcycle,bool IsHelicopter)
    {
        if (Vehicles == null || !Vehicles.Any())
            return null;

        List<VehicleInformation> ToPickFrom = Vehicles.Where(x => x.IsMotorcycle == IsMotorcycle && x.IsHelicopter == IsHelicopter && x.CanCurrentlySpawn).ToList();     
        int Total = ToPickFrom.Sum(x => x.CurrentSpawnChance);
       // Debugging.WriteToLog("GetRandomVehicle", string.Format("Total Chance {0}, Items {1}", Total, string.Join(",",ToPickFrom.Select( x => x.ModelName + " " + x.CanCurrentlySpawn + "  " + x.CurrentSpawnChance))));
        int RandomPick = LosSantosRED.MyRand.Next(0, Total);
        foreach (VehicleInformation Vehicle in ToPickFrom)
        {
            int SpawnChance = Vehicle.CurrentSpawnChance;
            if (RandomPick < SpawnChance)
            {
                return Vehicle;
            }
            RandomPick -= SpawnChance;
        }
        return null;
    }

    public ModelInformation GetRandomPed(List<string> RequiredModels)
    {
        if (CopModels == null || !CopModels.Any())
            return null;

        List<ModelInformation> ToPickFrom = CopModels.Where(x => LosSantosRED.PlayerWantedLevel >= x.MinWantedLevelSpawn && LosSantosRED.PlayerWantedLevel <= x.MaxWantedLevelSpawn).ToList();
        if(RequiredModels != null && RequiredModels.Any())
        {
            ToPickFrom = ToPickFrom.Where(x => RequiredModels.Contains(x.ModelName.ToLower())).ToList();
        }

        int Total = ToPickFrom.Sum(x => x.CurrentSpawnChance);
        //Debugging.WriteToLog("GetRandomPed", string.Format("Total Chance {0}, Total Items {1}", Total, ToPickFrom.Count()));
        int RandomPick = LosSantosRED.MyRand.Next(0, Total);
        foreach (ModelInformation Cop in ToPickFrom)
        {
            int SpawnChance = Cop.CurrentSpawnChance;
            if (RandomPick < SpawnChance)
            {
                return Cop;
            }
            RandomPick -= SpawnChance;
        }
        return null;
    }

    public Agency()
    {

    }
    public Agency(string _ColorPrefix, string _Initials, string _FullName, string _AgencyColorString, Classification _AgencyClassification, List<ModelInformation> _CopModels, List<VehicleInformation> _Vehicles,string _LicensePlatePrefix, List<IssuedWeapon> _IssuedWeapons)
    {
        ColorPrefix = _ColorPrefix;
        Initials = _Initials;
        FullName = _FullName;
        CopModels = _CopModels;
        AgencyColorString = _AgencyColorString;
        Vehicles = _Vehicles;
        AgencyClassification = _AgencyClassification;
        LicensePlatePrefix = _LicensePlatePrefix;
        IssuedWeapons = _IssuedWeapons;
    }
    public class ModelInformation
    {
        public string ModelName;
        public int AmbientSpawnChance = 0;
        public int WantedSpawnChance = 0;
        public bool IsMale = true;
        public int MinWantedLevelSpawn = 0;
        public int MaxWantedLevelSpawn = 5;
        public PedVariation RequiredVariation;
        public bool CanCurrentlySpawn
        {
            get
            {
                if (LosSantosRED.PlayerIsWanted)
                {
                    if (LosSantosRED.PlayerWantedLevel >= MinWantedLevelSpawn && LosSantosRED.PlayerWantedLevel <= MaxWantedLevelSpawn)
                        return WantedSpawnChance > 0;
                    else
                        return false;
                }
                else
                    return AmbientSpawnChance > 0;
            }
        }
        public int CurrentSpawnChance
        {
            get
            {
                if (LosSantosRED.PlayerIsWanted)
                {
                    if (LosSantosRED.PlayerWantedLevel >= MinWantedLevelSpawn && LosSantosRED.PlayerWantedLevel <= MaxWantedLevelSpawn)
                        return WantedSpawnChance;
                    else
                        return 0;
                }
                else
                    return AmbientSpawnChance;
            }
        }

        public ModelInformation()
        {

        }
        public ModelInformation(string _ModelName, bool _isMale, int ambientSpawnChance, int wantedSpawnChance)
        {
            ModelName = _ModelName;
            IsMale = _isMale;
            AmbientSpawnChance = ambientSpawnChance;
            WantedSpawnChance = wantedSpawnChance;
        }
    }
    public class VehicleInformation
    {
        public string ModelName;
        public int AmbientSpawnChance = 0;
        public int WantedSpawnChance = 0;
        public bool IsMotorcycle = false;
        public bool IsHelicopter = false;
        public int MinOccupants = 1;
        public int MaxOccupants = 2;
        public int MinWantedLevelSpawn = 0;
        public int MaxWantedLevelSpawn = 5;
        public List<string> AllowedPedModels = new List<string>();//only ped models can spawn in this, if emptyt any ambient spawn can
        public List<int> Liveries = new List<int>();
        public bool CanCurrentlySpawn
        {
            get
            {
                if (LosSantosRED.PlayerIsWanted)
                {
                    if (LosSantosRED.PlayerWantedLevel >= MinWantedLevelSpawn && LosSantosRED.PlayerWantedLevel <= MaxWantedLevelSpawn)
                        return WantedSpawnChance > 0;
                    else
                        return false;
                }
                else
                    return AmbientSpawnChance > 0;
            }
        }
        public int CurrentSpawnChance
        {
            get
            {
                if (LosSantosRED.PlayerIsWanted)
                {
                    if (LosSantosRED.PlayerWantedLevel >= MinWantedLevelSpawn && LosSantosRED.PlayerWantedLevel <= MaxWantedLevelSpawn)
                        return WantedSpawnChance;
                    else
                        return 0;
                }
                else
                    return AmbientSpawnChance;
            }
        }
        public VehicleInformation()
        {

        }
        public VehicleInformation(string modelName, int ambientSpawnChance, int wantedSpawnChance)
        {
            ModelName = modelName;
            AmbientSpawnChance = ambientSpawnChance;
            WantedSpawnChance = wantedSpawnChance;
        }
    }
    public class IssuedWeapon
    {
        public string ModelName;
        public bool IsPistol = false;
        public GTAWeapon.WeaponVariation MyVariation = new GTAWeapon.WeaponVariation();
        public IssuedWeapon()
        {

        }
        public IssuedWeapon(string _ModelName, bool _IsPistol, GTAWeapon.WeaponVariation _MyVariation)
        {
            ModelName = _ModelName;
            IsPistol = _IsPistol;
            MyVariation = _MyVariation;
        }

    }
}

