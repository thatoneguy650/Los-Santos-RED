using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static partial class Agencies
{
    private static string ConfigFileName = "Plugins\\LosSantosRED\\Agencies.xml";
    public static List<Agency> AgenciesList { get; set; }
    public static Agency RandomHighwayAgency
    {
        get
        {
            return AgenciesList.Where(x => x.CanSpawn && x.SpawnsOnHighway).PickRandom();
        }
    }
    public static Agency RandomArmyAgency
    {
        get
        {
            return AgenciesList.Where(x => x.CanSpawn && x.AgencyClassification == Agency.Classification.Military).PickRandom();
        }
    }
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
            AgenciesList = General.DeserializeParams<Agency>(ConfigFileName);
        }
        else
        {
            DefaultConfig();
            General.SerializeParams(AgenciesList, ConfigFileName);
        }
    }
    private static void DefaultConfig()
    {
        //Peds
        List<Agency.ModelInformation> StandardCops = new List<Agency.ModelInformation>() {
            new Agency.ModelInformation("s_m_y_cop_01",85,85),
            new Agency.ModelInformation("s_f_y_cop_01",15,15) };
        List<Agency.ModelInformation> ExtendedStandardCops = new List<Agency.ModelInformation>() {
            new Agency.ModelInformation("s_m_y_cop_01",85,85),
            new Agency.ModelInformation("s_f_y_cop_01",10,10),
            new Agency.ModelInformation("ig_trafficwarden",5,5) };
        List<Agency.ModelInformation> ParkRangers = new List<Agency.ModelInformation>() {
            new Agency.ModelInformation("s_m_y_ranger_01",75,75),
            new Agency.ModelInformation("s_f_y_ranger_01",25,25) };
        List<Agency.ModelInformation> SheriffPeds = new List<Agency.ModelInformation>() {
            new Agency.ModelInformation("s_m_y_sheriff_01",75,75),
            new Agency.ModelInformation("s_f_y_sheriff_01",25,25) };
        List<Agency.ModelInformation> SWAT = new List<Agency.ModelInformation>() {
            new Agency.ModelInformation("s_m_y_swat_01", 100,100) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(10, 0, 0,0) },new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) }) } };
        List<Agency.ModelInformation> PoliceAndSwat = new List<Agency.ModelInformation>() {
            new Agency.ModelInformation("s_m_y_cop_01",70,0),
            new Agency.ModelInformation("s_f_y_cop_01",30,0),
            new Agency.ModelInformation("s_m_y_swat_01", 0,100) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(10, 0, 0,0) },new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) }) } };
        List<Agency.ModelInformation> SheriffAndSwat = new List<Agency.ModelInformation>() {
            new Agency.ModelInformation("s_m_y_sheriff_01", 75, 0),
            new Agency.ModelInformation("s_f_y_sheriff_01", 25, 0),
            new Agency.ModelInformation("s_m_y_swat_01", 0, 100) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(10, 0, 0,0) },new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) }) } };
        List<Agency.ModelInformation> DOAPeds = new List<Agency.ModelInformation>() {
            new Agency.ModelInformation("u_m_m_doa_01",100,100) };
        List<Agency.ModelInformation> IAAPeds = new List<Agency.ModelInformation>() {
            new Agency.ModelInformation("s_m_m_fibsec_01",100,100) };
        List<Agency.ModelInformation> SAHPPeds = new List<Agency.ModelInformation>() {
            new Agency.ModelInformation("s_m_y_hwaycop_01",100,100) };
        List<Agency.ModelInformation> MilitaryPeds = new List<Agency.ModelInformation>() {
            new Agency.ModelInformation("s_m_y_armymech_01",25,0),
            new Agency.ModelInformation("s_m_m_marine_01",50,0),
            new Agency.ModelInformation("s_m_m_marine_02",0,0),
            new Agency.ModelInformation("s_m_y_marine_01",25,0),
            new Agency.ModelInformation("s_m_y_marine_02",0,0),
            new Agency.ModelInformation("s_m_y_marine_03",100,100) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(2, 1, 0, 0),new PedComponent(8, 0, 0, 0) },new List<PedPropComponent>() { new PedPropComponent(3, 1, 0) }) },
            new Agency.ModelInformation("s_m_m_pilot_02",0,0),
            new Agency.ModelInformation("s_m_y_pilot_01",0,0) };
        List<Agency.ModelInformation> FIBPeds = new List<Agency.ModelInformation>() {
            new Agency.ModelInformation("s_m_m_fibsec_01",55,70),
            new Agency.ModelInformation("s_m_m_fiboffice_01",15,0),
            new Agency.ModelInformation("s_m_m_fiboffice_02",15,0),
            new Agency.ModelInformation("u_m_m_fibarchitect",10,0),
            new Agency.ModelInformation("s_m_y_swat_01", 5,30) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(10, 0, 1,0) },new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) }) } };
        List<Agency.ModelInformation> PrisonPeds = new List<Agency.ModelInformation>() {
            new Agency.ModelInformation("s_m_m_prisguard_01",100,100) };
        List<Agency.ModelInformation> SecurityPeds = new List<Agency.ModelInformation>() {
            new Agency.ModelInformation("s_m_m_security_01",100,100) };
        List<Agency.ModelInformation> CoastGuardPeds = new List<Agency.ModelInformation>() {
            new Agency.ModelInformation("s_m_y_uscg_01",100,100) };
        List<Agency.ModelInformation> NOOSEPeds = new List<Agency.ModelInformation>() {
            new Agency.ModelInformation("s_m_y_swat_01", 100,100) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(10, 0, 0,0) },new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) }) } };

        //Vehicles
        List<Agency.VehicleInformation> UnmarkedVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("police4", 100, 100) };
        List<Agency.VehicleInformation> CoastGuardVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("predator", 75, 50),
            new Agency.VehicleInformation("dinghy", 0, 25),
            new Agency.VehicleInformation("seashark2", 25, 25) { MaxOccupants = 1 },};      
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
            new Agency.VehicleInformation("annihilator", 0, 100) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 5, AllowedPedModels = new List<string>() { "s_m_y_swat_01" },MinOccupants = 3,MaxOccupants = 4 }};
        List<Agency.VehicleInformation> HighwayPatrolVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("policeb", 95, 95) { MaxOccupants = 1 },
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
            new Agency.VehicleInformation("polmav", 0,100) { Liveries = new List<int>() { 0 }, MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 3 } };
        List<Agency.VehicleInformation> SheriffHeliVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("buzzard2", 0,25) { Liveries = new List<int>() { 0 }, MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 3 },
            new Agency.VehicleInformation("polmav", 0,75) { Liveries = new List<int>() { 2 }, MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 3 } };
        List<Agency.VehicleInformation> ArmyVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("crusader", 75,50) { Liveries = new List<int>() { 0 },MinOccupants = 1,MaxOccupants = 2,MaxWantedLevelSpawn = 4 },
            new Agency.VehicleInformation("barracks", 25,50) { Liveries = new List<int>() { 0 },MinOccupants = 3,MaxOccupants = 5,MinWantedLevelSpawn = 4 },
            new Agency.VehicleInformation("rhino", 0,10) { Liveries = new List<int>() { 0 },MinOccupants = 1,MaxOccupants = 2,MinWantedLevelSpawn = 5 },
            new Agency.VehicleInformation("valkyrie", 0,50) { Liveries = new List<int>() { 0 },MinOccupants = 3,MaxOccupants = 3,MinWantedLevelSpawn = 4 },
            new Agency.VehicleInformation("valkyrie2", 0,50) { Liveries = new List<int>() { 0 },MinOccupants = 3,MaxOccupants = 3,MinWantedLevelSpawn = 4 },
        };

        //Weapon
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

        List<Agency.IssuedWeapon> HeliWeapons = new List<Agency.IssuedWeapon>()
        {
            new Agency.IssuedWeapon("weapon_pistol_mk2", true, new GTAWeapon.WeaponVariation(0,new List<string> { "Flashlight" })),
            new Agency.IssuedWeapon("weapon_pistol_mk2", true, new GTAWeapon.WeaponVariation(0,new List<string> { "Extended Clip" })),
            new Agency.IssuedWeapon("weapon_pistol_mk2", true, new GTAWeapon.WeaponVariation(0, new List<string> { "Flashlight","Extended Clip" })),
            new Agency.IssuedWeapon("weapon_marksmanrifle_mk2", false, new GTAWeapon.WeaponVariation(0, new List<string> { "Large Scope", "Suppressor", "Tracer Rounds" })),
            new Agency.IssuedWeapon("weapon_marksmanrifle_mk2", false, new GTAWeapon.WeaponVariation(0, new List<string> { "Large Scope","Tracer Rounds" })),
        };

        List<Agency.IssuedWeapon> LimitedWeapons = new List<Agency.IssuedWeapon>()
        {
            new Agency.IssuedWeapon("weapon_heavypistol", true, new GTAWeapon.WeaponVariation()),
            new Agency.IssuedWeapon("weapon_revolver", true, new GTAWeapon.WeaponVariation()),
            new Agency.IssuedWeapon("weapon_heavypistol", true, new GTAWeapon.WeaponVariation(0,new List<string> { "Flashlight" })),
            new Agency.IssuedWeapon("weapon_pumpshotgun", false, new GTAWeapon.WeaponVariation()),
            new Agency.IssuedWeapon("weapon_pumpshotgun", false, new GTAWeapon.WeaponVariation(0,new List<string> { "Flashlight" })),

        };

        List<Agency.ZoneJurisdiction> LSPDJurisdication = new List<Agency.ZoneJurisdiction>()
        {
            //Central
            new Agency.ZoneJurisdiction("DOWNT", 0, 94, 80),
            new Agency.ZoneJurisdiction("PBOX", 0, 94, 80),
            new Agency.ZoneJurisdiction("SKID", 0, 94, 80),
            new Agency.ZoneJurisdiction("TEXTI", 0, 94, 80),
            new Agency.ZoneJurisdiction("LEGSQU", 0, 94, 80),
            new Agency.ZoneJurisdiction("BANNING", 1, 50, 40),
            new Agency.ZoneJurisdiction("CHAMH", 1, 50, 40),
            new Agency.ZoneJurisdiction("DAVIS", 1, 50, 40),
            new Agency.ZoneJurisdiction("RANCHO", 1, 50, 40),
            new Agency.ZoneJurisdiction("STRAW", 1, 50, 40),

            //Vespucci
            new Agency.ZoneJurisdiction("BEACH", 1, 10, 10),
            new Agency.ZoneJurisdiction("DELBE", 1, 10, 10),
            new Agency.ZoneJurisdiction("DELPE", 1, 10, 10),
            new Agency.ZoneJurisdiction("VCANA", 1, 10, 10),
            new Agency.ZoneJurisdiction("VESP", 1, 10, 10),
            new Agency.ZoneJurisdiction("LOSPUER", 1, 10, 10),
            new Agency.ZoneJurisdiction("PBLUFF", 1, 10, 10),
            new Agency.ZoneJurisdiction("DELSOL", 1, 10, 10),

            //EastLS
            new Agency.ZoneJurisdiction("CYPRE", 1, 10, 10),
            new Agency.ZoneJurisdiction("LMESA", 1, 10, 10),
            new Agency.ZoneJurisdiction("MIRR", 1, 10, 10),
            new Agency.ZoneJurisdiction("MURRI", 1, 10, 10),
            new Agency.ZoneJurisdiction("EBURO", 1, 10, 10),

            //Port
            new Agency.ZoneJurisdiction("ELYSIAN", 1, 5, 5),
            new Agency.ZoneJurisdiction("ZP_ORT", 1, 5, 5),
            new Agency.ZoneJurisdiction("TERMINA", 1, 5, 5),
            new Agency.ZoneJurisdiction("ZP_ORT", 1, 5, 5),
            new Agency.ZoneJurisdiction("AIRP", 1, 5, 5),

            //Ocean
            new Agency.ZoneJurisdiction("OCEANA", 1, 5, 5),

            //Rockford Hills
            new Agency.ZoneJurisdiction("BURTON", 2, 5, 5),
            new Agency.ZoneJurisdiction("GOLF", 2, 5, 5),
            new Agency.ZoneJurisdiction("KOREAT", 2, 5, 5),
            new Agency.ZoneJurisdiction("MORN", 2, 5, 5),
            new Agency.ZoneJurisdiction("MOVIE", 2, 5, 5),
            new Agency.ZoneJurisdiction("RICHM", 2, 5, 5),
            new Agency.ZoneJurisdiction("ROCKF", 2, 5, 5),

        };

        List<Agency.ZoneJurisdiction> LSSDJurisdication = new List<Agency.ZoneJurisdiction>()
        {
            //Tatiavian
            new Agency.ZoneJurisdiction("LACT", 0, 85, 70),
            new Agency.ZoneJurisdiction("LDAM", 0, 85, 70),
            new Agency.ZoneJurisdiction("NOOSE", 0, 85, 70),
            new Agency.ZoneJurisdiction("PALHIGH", 0, 85, 70),
            new Agency.ZoneJurisdiction("PALMPOW", 0, 85, 70),
            new Agency.ZoneJurisdiction("SANAND", 0, 85, 70),
            new Agency.ZoneJurisdiction("TATAMO", 0, 85, 70),
            new Agency.ZoneJurisdiction("WINDF", 0, 85, 70),

            //North Blaine
            new Agency.ZoneJurisdiction("PROCOB", 2, 0, 5),
            new Agency.ZoneJurisdiction("PALETO", 2, 0, 5),
            new Agency.ZoneJurisdiction("PALCOV", 2, 0, 5),
            new Agency.ZoneJurisdiction("PALFOR", 2, 0, 5),
            new Agency.ZoneJurisdiction("CALAFB", 2, 0, 5),
            new Agency.ZoneJurisdiction("GALFISH", 2, 0, 5),
            new Agency.ZoneJurisdiction("ELGORL", 2, 0, 5),
            new Agency.ZoneJurisdiction("GRAPES", 2, 0, 5),
            new Agency.ZoneJurisdiction("BRADP", 2, 0, 5),
            new Agency.ZoneJurisdiction("BRADT", 2, 0, 5),
            new Agency.ZoneJurisdiction("CCREAK", 2, 0, 5),

            //Blaine
            new Agency.ZoneJurisdiction("ALAMO", 2, 0, 5),
            new Agency.ZoneJurisdiction("CANNY", 2, 0, 5),
            new Agency.ZoneJurisdiction("DESRT", 2, 0, 5),
            new Agency.ZoneJurisdiction("HUMLAB", 2, 0, 5),
            new Agency.ZoneJurisdiction("MTJOSE", 2, 0, 5),
            new Agency.ZoneJurisdiction("NCHU", 2, 0, 5),
            new Agency.ZoneJurisdiction("SANDY", 2, 0, 5),
            new Agency.ZoneJurisdiction("SLAB", 2, 0, 5),
            new Agency.ZoneJurisdiction("ZQ_UAR", 2, 0, 5),

        };

        List<Agency.ZoneJurisdiction> LSPDVWJurisdiction = new List<Agency.ZoneJurisdiction>()
        {
            //Vinewood
            new Agency.ZoneJurisdiction("ALTA", 0, 85, 70),
            new Agency.ZoneJurisdiction("DTVINE", 0, 85, 70),
            new Agency.ZoneJurisdiction("EAST_V", 0, 85, 70),
            new Agency.ZoneJurisdiction("HAWICK", 0, 85, 70),
            new Agency.ZoneJurisdiction("HORS", 0, 85, 70),
            new Agency.ZoneJurisdiction("VINE", 0, 85, 70),
            new Agency.ZoneJurisdiction("WVINE", 0, 85, 70),
            new Agency.ZoneJurisdiction("LSSD-VW", 0, 85, 70),
            new Agency.ZoneJurisdiction("LSSD-VW", 0, 85, 70),

        };

        List<Agency.ZoneJurisdiction> LSPDELSJurisdiction = new List<Agency.ZoneJurisdiction>()
        {
            //East LS
            new Agency.ZoneJurisdiction("CYPRE", 0, 85, 70),
            new Agency.ZoneJurisdiction("LMESA", 0, 85, 70),
            new Agency.ZoneJurisdiction("MIRR", 0, 85, 70),
            new Agency.ZoneJurisdiction("MURRI", 0, 85, 70),
            new Agency.ZoneJurisdiction("EBURO", 0, 85, 70),

        };

        List<Agency.ZoneJurisdiction> LSPDDPJurisdiction = new List<Agency.ZoneJurisdiction>()
        {
            //Vespucci
            new Agency.ZoneJurisdiction("BEACH", 0, 84, 70),
            new Agency.ZoneJurisdiction("DELBE", 0, 84, 70),
            new Agency.ZoneJurisdiction("DELPE", 0, 84, 70),
            new Agency.ZoneJurisdiction("VCANA", 0, 84, 70),
            new Agency.ZoneJurisdiction("VESP", 0, 84, 70),
            new Agency.ZoneJurisdiction("LOSPUER", 0, 84, 70),
            new Agency.ZoneJurisdiction("PBLUFF", 0, 84, 70),
            new Agency.ZoneJurisdiction("DELSOL", 0, 84, 70),
        };

        List<Agency.ZoneJurisdiction> LSPDRHJurisdiction = new List<Agency.ZoneJurisdiction>()
        {
            //Rockford Hills
            new Agency.ZoneJurisdiction("BURTON", 0, 85, 70),
            new Agency.ZoneJurisdiction("GOLF", 0, 85, 70),
            new Agency.ZoneJurisdiction("KOREAT", 0, 85, 70),
            new Agency.ZoneJurisdiction("MORN", 0, 85, 70),
            new Agency.ZoneJurisdiction("MOVIE", 0, 85, 70),
            new Agency.ZoneJurisdiction("RICHM", 0, 85, 70),
            new Agency.ZoneJurisdiction("ROCKF", 0, 85, 70),
        };


        List<Agency.ZoneJurisdiction> LSSDVWJurisdiction = new List<Agency.ZoneJurisdiction>()
        {
            //Vinewood Hills
            new Agency.ZoneJurisdiction("CHIL", 0, 85, 70),
            new Agency.ZoneJurisdiction("GREATC", 0, 85, 70),
            new Agency.ZoneJurisdiction("BAYTRE", 0, 85, 70),
            new Agency.ZoneJurisdiction("RGLEN", 0, 85, 70),
            new Agency.ZoneJurisdiction("TONGVAV", 0, 85, 70),
            new Agency.ZoneJurisdiction("HARMO", 0, 85, 70),
            new Agency.ZoneJurisdiction("RTRAK", 0, 85, 70),

            //Vinewood
            new Agency.ZoneJurisdiction("ALTA", 1, 10, 10),
            new Agency.ZoneJurisdiction("DTVINE", 1, 10, 10),
            new Agency.ZoneJurisdiction("EAST_V", 1, 10, 10),
            new Agency.ZoneJurisdiction("HAWICK", 1, 10, 10),
            new Agency.ZoneJurisdiction("HORS", 1, 10, 10),
            new Agency.ZoneJurisdiction("VINE", 1, 10, 10),
            new Agency.ZoneJurisdiction("WVINE", 1, 10, 10),
        };

        List<Agency.ZoneJurisdiction> LSSDCHJurisdiction = new List<Agency.ZoneJurisdiction>()
        {
            //Chumash
            new Agency.ZoneJurisdiction("BANHAMC", 0, 85, 70),
            new Agency.ZoneJurisdiction("BHAMCA", 0, 85, 70),
            new Agency.ZoneJurisdiction("CHU", 0, 85, 70),
            new Agency.ZoneJurisdiction("TONGVAH", 0, 85, 70),
        };

        List<Agency.ZoneJurisdiction> LSSDBCJurisdiction = new List<Agency.ZoneJurisdiction>()
        {
            //North Blaine
            new Agency.ZoneJurisdiction("PROCOB", 0, 85, 75),
            new Agency.ZoneJurisdiction("PALETO", 0, 85, 75),
            new Agency.ZoneJurisdiction("PALCOV", 0, 85, 75),
            new Agency.ZoneJurisdiction("PALFOR", 0, 85, 75),
            new Agency.ZoneJurisdiction("CALAFB", 0, 85, 75),
            new Agency.ZoneJurisdiction("GALFISH", 0, 85, 75),
            new Agency.ZoneJurisdiction("ELGORL", 0, 85, 75),
            new Agency.ZoneJurisdiction("GRAPES", 0, 85, 75),
            new Agency.ZoneJurisdiction("BRADP", 0, 85, 75),
            new Agency.ZoneJurisdiction("BRADT", 0, 85, 75),
            new Agency.ZoneJurisdiction("CCREAK", 0, 85, 75),

            new Agency.ZoneJurisdiction("MTCHIL", 1, 49, 65),
            new Agency.ZoneJurisdiction("MTGORDO", 1, 49, 65),
            new Agency.ZoneJurisdiction("CMSW", 1, 49, 65),

            //Blaine
            new Agency.ZoneJurisdiction("ALAMO", 0, 85, 75),
            new Agency.ZoneJurisdiction("CANNY", 0, 85, 75),
            new Agency.ZoneJurisdiction("DESRT", 0, 85, 75),
            new Agency.ZoneJurisdiction("HUMLAB", 0, 85, 75),
            new Agency.ZoneJurisdiction("LAGO", 0, 85, 75),
            new Agency.ZoneJurisdiction("MTJOSE", 0, 85, 75),
            new Agency.ZoneJurisdiction("NCHU", 0, 85, 75),
            new Agency.ZoneJurisdiction("SANDY", 0, 85, 75),
            new Agency.ZoneJurisdiction("SLAB", 0, 85, 75),
            new Agency.ZoneJurisdiction("ZQ_UAR", 0, 85, 75),

            new Agency.ZoneJurisdiction("JAIL", 1, 49, 65),
            new Agency.ZoneJurisdiction("SANCHIA", 1, 49, 65),
            new Agency.ZoneJurisdiction("ZANCUDO", 1, 49, 65),
        };

        List<Agency.ZoneJurisdiction> SASPAJurisdiction = new List<Agency.ZoneJurisdiction>()
        {
            new Agency.ZoneJurisdiction("JAIL", 0, 100, 70),
        };

        List<Agency.ZoneJurisdiction> SAPRJurisdiction = new List<Agency.ZoneJurisdiction>()
        {
            new Agency.ZoneJurisdiction("MTCHIL", 0, 51, 20),
            new Agency.ZoneJurisdiction("MTGORDO", 0, 51, 20),
            new Agency.ZoneJurisdiction("CMSW", 0, 51, 20),
            new Agency.ZoneJurisdiction("SANCHIA", 0, 51, 20),
            new Agency.ZoneJurisdiction("ZANCUDO", 0, 51, 20),
        };

        List<Agency.ZoneJurisdiction> SACGJurisdiction = new List<Agency.ZoneJurisdiction>()
        {
            new Agency.ZoneJurisdiction("OCEANA", 0, 95, 80),
        };

        List<Agency.ZoneJurisdiction> LSPAJurisdiction = new List<Agency.ZoneJurisdiction>()
        {
            //Port
            new Agency.ZoneJurisdiction("ELYSIAN", 0, 95, 80),
            new Agency.ZoneJurisdiction("ZP_ORT", 0, 95, 80),
            new Agency.ZoneJurisdiction("TERMINA", 0, 95, 80),
            new Agency.ZoneJurisdiction("ZP_ORT", 0, 95, 80),
        };

        List<Agency.ZoneJurisdiction> LSIAPDJurisdiction = new List<Agency.ZoneJurisdiction>()
        {
            new Agency.ZoneJurisdiction("AIRP", 0, 100, 100),
        };

        List<Agency.ZoneJurisdiction> PRISECJurisdiction = new List<Agency.ZoneJurisdiction>()
        {
            new Agency.ZoneJurisdiction("STAD", 0, 100, 50),
        };

        List<Agency.ZoneJurisdiction> ARMYJurisdiction = new List<Agency.ZoneJurisdiction>()
        {
            new Agency.ZoneJurisdiction("ARMYB", 0, 100, 100),
        };

        AgenciesList = new List<Agency>
        {
            new Agency("~b~", "LSPD", "Los Santos Police Department", "Blue", Agency.Classification.Police, LSPDJurisdication, StandardCops, LSPDVehicles, "LS ",AllWeapons),
            new Agency("~b~", "LSPD-VW", "Los Santos Police - Vinewood Division", "Blue", Agency.Classification.Police, LSPDVWJurisdiction, ExtendedStandardCops, VWPDVehicles, "LSV ",LimitedWeapons),
            new Agency("~b~", "LSPD-ELS", "Los Santos Police - East Los Santos Division", "Blue", Agency.Classification.Police, LSPDELSJurisdiction, ExtendedStandardCops, EastLSPDVehicles, "LSE ",LimitedWeapons),
            new Agency("~b~", "LSPD-DP", "Los Santos Police - Del Pierro Division", "Blue", Agency.Classification.Police, LSPDDPJurisdiction, StandardCops, DPPDVehicles, "VP ",AllWeapons),
            new Agency("~b~", "LSPD-RH", "Los Santos Police - Rockford Hills Division", "Blue", Agency.Classification.Police, LSPDRHJurisdiction, StandardCops, RHPDVehicles, "RH ",AllWeapons),        
      
            new Agency("~r~", "LSSD", "Los Santos County Sheriff", "Red", Agency.Classification.Sheriff, LSSDJurisdication, SheriffPeds, LSSDVehicles, "LSCS ",LimitedWeapons),
            new Agency("~r~", "LSSD-VW", "Los Santos Sheriff - Vinewood Division", "Red", Agency.Classification.Sheriff, LSSDVWJurisdiction, SheriffPeds, VWHillsLSSDVehicles, "LSCS ",LimitedWeapons),
            new Agency("~r~", "LSSD-CH", "Los Santos Sheriff - Chumash Division", "Red", Agency.Classification.Sheriff, LSSDCHJurisdiction, SheriffPeds, ChumashLSSDVehicles, "LSCS ",LimitedWeapons),
            new Agency("~r~", "LSSD-BC", "Los Santos Sheriff - Blaine County Division", "Red", Agency.Classification.Sheriff, LSSDBCJurisdiction, SheriffPeds, BCSOVehicles, "BCS ",LimitedWeapons),

            new Agency("~b~", "LSPD-ASD", "Los Santos Police Department - Air Support Division", "Blue", Agency.Classification.Police, null, PoliceAndSwat, PoliceHeliVehicles, "ASD ",HeliWeapons) { MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4 },
            new Agency("~r~", "LSSD-ASD", "Los Santos Sheriffs Department - Air Support Division", "Red", Agency.Classification.Sheriff, null, SheriffAndSwat, SheriffHeliVehicles, "ASD ",HeliWeapons) { MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4 },

            new Agency("~r~", "NOOSE", "National Office of Security Enforcement", "DarkSlateGray", Agency.Classification.Federal, null, NOOSEPeds, NOOSEVehicles, "",BestWeapons) { MinWantedLevelSpawn = 3 },
            new Agency("~p~", "FIB", "Federal Investigation Bureau", "Purple", Agency.Classification.Federal, null, FIBPeds, FIBVehicles, "FIB ",BestWeapons),
            new Agency("~p~", "DOA", "Drug Observation Agency", "Purple", Agency.Classification.Federal, null, DOAPeds, UnmarkedVehicles, "DOA ",AllWeapons) {SpawnLimit = 4 },

            new Agency("~y~", "SAHP", "San Andreas Highway Patrol", "Yellow", Agency.Classification.State, null, SAHPPeds, SAHPVehicles, "HP ",LimitedWeapons) {SpawnsOnHighway = true },
            new Agency("~o~", "SASPA", "San Andreas State Prison Authority", "Orange", Agency.Classification.State, SASPAJurisdiction, PrisonPeds, PrisonVehicles, "SASPA ",AllWeapons), 
            new Agency("~g~", "SAPR", "San Andreas Park Ranger", "Green", Agency.Classification.State, SAPRJurisdiction, ParkRangers, ParkRangerVehicles, "",AllWeapons) {MaxWantedLevelSpawn = 3,SpawnLimit = 3 },            
            new Agency("~o~", "SACG", "San Andreas Coast Guard", "DarkOrange", Agency.Classification.State, SACGJurisdiction, CoastGuardPeds, CoastGuardVehicles, "SACG ",LimitedWeapons){MaxWantedLevelSpawn = 3,SpawnLimit = 3 },

            new Agency("~p~", "LSPA", "Port Authority of Los Santos", "LightGray", Agency.Classification.Police, LSPAJurisdiction, SecurityPeds, UnmarkedVehicles, "LSPA ",LimitedWeapons) {MaxWantedLevelSpawn = 3,SpawnLimit = 3 },
            new Agency("~p~", "LSIAPD", "Los Santos International Airport Police Department", "LightBlue", Agency.Classification.Police, LSIAPDJurisdiction, StandardCops, LSPDVehicles, "LSA ",AllWeapons) { SpawnLimit = 3 },

            new Agency("~o~", "PRISEC", "Private Security", "White", Agency.Classification.Security, PRISECJurisdiction, SecurityPeds, SecurityVehicles, "",LimitedWeapons) {MaxWantedLevelSpawn = 1,SpawnLimit = 2 },

            new Agency("~u~", "ARMY", "Army", "Black", Agency.Classification.Military, ARMYJurisdiction, MilitaryPeds, ArmyVehicles, "",BestWeapons),


            new Agency("~s~", "UNK", "Unknown Agency", "White", Agency.Classification.Other, null, null, null, "",null),

        };
      
    }
    public static Agency MainAgencyAtZone(string ZoneName)
    {
        return AgenciesList.Where(z => z.Jurisdiction.Any(y => y.ZoneInternalGameName.ToLower() == ZoneName.ToLower())).OrderBy(z => z.Jurisdiction.Where(y => y.ZoneInternalGameName.ToLower() == ZoneName.ToLower()).Min(h => h.Priority)).FirstOrDefault();

        //return AgenciesList.Where(x => x.Jurisdiction != null).OrderBy(z => z.Jurisdiction.Where(y => y.ZoneInternalGameName.ToLower() == ZoneName.ToLower()).Min(h => h.Priority)).FirstOrDefault();
    }
    public static Agency RandomAgencyAtZone(string ZoneName)
    {
        List<Tuple<Agency, Agency.ZoneJurisdiction>> ToPickFrom = new List<Tuple<Agency, Agency.ZoneJurisdiction>>();
        foreach (Agency agency in AgenciesList)
        {
            foreach(Agency.ZoneJurisdiction zoneJurisdiction in agency.Jurisdiction)
            {
                if(zoneJurisdiction.ZoneInternalGameName.ToLower() == ZoneName.ToLower())
                {
                    ToPickFrom.Add(new Tuple<Agency, Agency.ZoneJurisdiction>(agency,zoneJurisdiction));
                }
            }
        }
        List<Agency> ZoneAgencies = AgenciesList.Where(z => z.Jurisdiction.Any(y => y.ZoneInternalGameName.ToLower() == ZoneName.ToLower())).ToList();
        if (ZoneAgencies == null || !ZoneAgencies.Any())
            return null;

        int Total = ToPickFrom.Sum(x => x.Item2.CurrentSpawnChance);
        int RandomPick = General.MyRand.Next(0, Total);
        foreach (Tuple<Agency, Agency.ZoneJurisdiction> ZA in ToPickFrom)
        {
            int SpawnChance = ZA.Item2.CurrentSpawnChance;
            if (RandomPick < SpawnChance)
            {
                return ZA.Item1;
            }
            RandomPick -= SpawnChance;
        }
        return null;     
    }
    public static Agency DetermineAgency(Ped Cop)
    {
        if (!Cop.IsPoliceArmy())
            return null;
        if (Cop.IsArmy())
            return AgenciesList.Where(x => x.AgencyClassification == Agency.Classification.Military).FirstOrDefault();
        else if (Cop.IsPolice())
            return GetAgencyFromPed(Cop);
        else
            return null;
    }
    public static Agency GetAgencyFromPed(Ped Cop)
    {
        Zone ZoneFound = Zones.GetZoneAtLocation(Cop.Position);
        Agency AgencyToReturn = null;
        if (ZoneFound != null)
        {
            AgencyToReturn = AgenciesList.Where(x => x.CopModels.Any(b => b.ModelName.ToLower() == Cop.Model.Name.ToLower()) && x.Jurisdiction.Any(y => y.ZoneInternalGameName == ZoneFound.InternalGameName)).OrderByDescending(z => z.Jurisdiction.Sum(f => f.CurrentSpawnChance)).FirstOrDefault();
        }

        if (AgencyToReturn == null)
        {
            AgencyToReturn = AgenciesList.Where(x => x.CopModels.Any(y => y.ModelName.ToLower() == Cop.Model.Name.ToLower())).PickRandom();
            if (AgencyToReturn == null)
            {
                Debugging.WriteToLog("GetAgencyFromPed", string.Format("Couldnt get agency from zone {0} ped {1}, deleting", ZoneFound.DisplayName, Cop.Model.Name));
                Cop.Delete();
            }
        }
        return AgencyToReturn;
    }
    public static Agency GetAgencyFromVehicle(Vehicle CopCar)
    {
        Zone ZoneFound = Zones.GetZoneAtLocation(CopCar.Position);
        Agency AgencyToReturn = null;
        if (ZoneFound != null)
        {
            AgencyToReturn = AgenciesList.Where(x => x.Vehicles.Any(b => b.ModelName.ToLower() == CopCar.Model.Name.ToLower()) && x.Jurisdiction.Any(y => y.ZoneInternalGameName == ZoneFound.InternalGameName)).OrderByDescending(z => z.Jurisdiction.Sum(f => f.CurrentSpawnChance)).FirstOrDefault();
        }

        if (AgencyToReturn == null)
        {
            AgencyToReturn = AgenciesList.Where(x => x.Vehicles.Any(y => y.ModelName.ToLower() == CopCar.Model.Name.ToLower())).PickRandom();
            if (AgencyToReturn == null)
            {
                Debugging.WriteToLog("GetAgencyFromVehicle", string.Format("Couldnt get agency from zone {0} car {1}, deleting", ZoneFound.DisplayName, CopCar.Model.Name));
                CopCar.Delete();
            }
        }
        return AgencyToReturn;
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
        CopCar.LicensePlate = AssignedAgency.LicensePlatePrefix + General.RandomString(8 - AssignedAgency.LicensePlatePrefix.Length);
    }
}


