using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

public static partial class Agencies
{
    private static string ConfigFileName = "Plugins\\LosSantosRED\\Agencies.xml";
    public static List<Agency> AgenciesList { get; set; }
    private static int LikelyHoodOfAnySpawn { get; set; } = 5;
    public static void Initialize()
    {
        ReadConfig();
    }
    public static void Dispose()
    {

    }
    public static List<Agency> AgenciesAtPosition(Vector3 Position)
    {
        List<Agency> ToReturn = new List<Agency>();
        Street StreetAtPosition = Streets.GetCurrentStreet(Position);
        if(StreetAtPosition != null && Streets.GetCurrentStreet(Position).IsHighway) //Highway Patrol Jurisdiction
        {
            ToReturn.AddRange(AgenciesList.Where(x => x.CanSpawn && x.SpawnsOnHighway));
        }
        Zone CurrentZone = Zones.GetZoneAtLocation(Position);
        Agency ZoneAgency1 = Jurisdiction.AgencyAtZone(CurrentZone.InternalGameName);
        if (ZoneAgency1 != null)
        {
            ToReturn.Add(ZoneAgency1); //Zone Jurisdiciton Random
        }
        Agency CountyAgency1 = Jurisdiction.AgencyAtCounty(CurrentZone.InternalGameName);
        if (CountyAgency1 != null)
        {
            ToReturn.Add(CountyAgency1); //Zone Jurisdiciton Random
        }
        if (!ToReturn.Any() || General.RandomPercent(LikelyHoodOfAnySpawn))
        {
            ToReturn.AddRange(AgenciesList.Where(x => x.CanSpawn && x.CanSpawnAnywhere));
        }
        foreach (Agency ag in ToReturn)
        {
            Debugging.WriteToLog("Debugging", string.Format("Agencies At Pos: {0}", ag.Initials));
        }
        return ToReturn;
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
           // DispatchWorksDefaultConfig();
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
            new Agency.ModelInformation("s_m_m_fibsec_01",55,70){MaxWantedLevelSpawn = 3 },
            new Agency.ModelInformation("s_m_m_fiboffice_01",15,0){MaxWantedLevelSpawn = 3 },
            new Agency.ModelInformation("s_m_m_fiboffice_02",15,0){MaxWantedLevelSpawn = 3 },
            new Agency.ModelInformation("u_m_m_fibarchitect",10,0) {MaxWantedLevelSpawn = 3 },
            new Agency.ModelInformation("s_m_y_swat_01", 5,30) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 4, RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(10, 0, 1,0) },new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) }) } };
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
            new Agency.VehicleInformation("fbi", 70, 70){ MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new Agency.VehicleInformation("fbi2", 30, 30) { MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new Agency.VehicleInformation("fbi2", 0, 30) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 4, AllowedPedModels = new List<string>() { "s_m_y_swat_01" },MinOccupants = 4, MaxOccupants = 6 },
        };
        List<Agency.VehicleInformation> NOOSEVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("fbi", 70, 70){ MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new Agency.VehicleInformation("fbi2", 30, 30) { MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new Agency.VehicleInformation("fbi2", 0, 30) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 4, AllowedPedModels = new List<string>() { "s_m_y_swat_01" },MinOccupants = 4, MaxOccupants = 6 },
            new Agency.VehicleInformation("riot", 0, 70) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 4, AllowedPedModels = new List<string>() { "s_m_y_swat_01" },MinOccupants = 2, MaxOccupants = 3 },
            new Agency.VehicleInformation("annihilator", 0, 100) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 4, AllowedPedModels = new List<string>() { "s_m_y_swat_01" },MinOccupants = 3,MaxOccupants = 4 }};
        List<Agency.VehicleInformation> HighwayPatrolVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("policeb", 70, 70) { MaxOccupants = 1 },
            new Agency.VehicleInformation("police4", 30, 30) };
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

        AgenciesList = new List<Agency>
        {
            new Agency("~b~", "LSPD", "Los Santos Police Department", "Blue", Agency.Classification.Police, StandardCops, LSPDVehicles, "LS ",AllWeapons) { MaxWantedLevelSpawn = 3 },
            new Agency("~b~", "LSPD-VW", "Los Santos Police - Vinewood Division", "Blue", Agency.Classification.Police, ExtendedStandardCops, VWPDVehicles, "LSV ",LimitedWeapons) { MaxWantedLevelSpawn = 3 },
            new Agency("~b~", "LSPD-ELS", "Los Santos Police - East Los Santos Division", "Blue", Agency.Classification.Police, ExtendedStandardCops, EastLSPDVehicles, "LSE ",LimitedWeapons) { MaxWantedLevelSpawn = 3 },
            new Agency("~b~", "LSPD-DP", "Los Santos Police - Del Pierro Division", "Blue", Agency.Classification.Police, StandardCops, DPPDVehicles, "VP ",AllWeapons) { MaxWantedLevelSpawn = 3 },
            new Agency("~b~", "LSPD-RH", "Los Santos Police - Rockford Hills Division", "Blue", Agency.Classification.Police, StandardCops, RHPDVehicles, "RH ",AllWeapons) { MaxWantedLevelSpawn = 3 },

            new Agency("~r~", "LSSD", "Los Santos County Sheriff", "Red", Agency.Classification.Sheriff, SheriffPeds, LSSDVehicles, "LSCS ",LimitedWeapons) { MaxWantedLevelSpawn = 3 },
            new Agency("~r~", "LSSD-VW", "Los Santos Sheriff - Vinewood Division", "Red", Agency.Classification.Sheriff, SheriffPeds, VWHillsLSSDVehicles, "LSCS ",LimitedWeapons) { MaxWantedLevelSpawn = 3 },
            new Agency("~r~", "LSSD-CH", "Los Santos Sheriff - Chumash Division", "Red", Agency.Classification.Sheriff, SheriffPeds, ChumashLSSDVehicles, "LSCS ",LimitedWeapons) { MaxWantedLevelSpawn = 3 },
            new Agency("~r~", "LSSD-BC", "Los Santos Sheriff - Blaine County Division", "Red", Agency.Classification.Sheriff, SheriffPeds, BCSOVehicles, "BCS ",LimitedWeapons) { MaxWantedLevelSpawn = 3 },

            new Agency("~b~", "LSPD-ASD", "Los Santos Police Department - Air Support Division", "Blue", Agency.Classification.Police, PoliceAndSwat, PoliceHeliVehicles, "ASD ",HeliWeapons) { MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4, SpawnLimit = 3 },
            new Agency("~r~", "LSSD-ASD", "Los Santos Sheriffs Department - Air Support Division", "Red", Agency.Classification.Sheriff, SheriffAndSwat, SheriffHeliVehicles, "ASD ",HeliWeapons) { MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4, SpawnLimit = 3 },

            new Agency("~r~", "NOOSE", "National Office of Security Enforcement", "DarkSlateGray", Agency.Classification.Federal, NOOSEPeds, NOOSEVehicles, "",BestWeapons) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 4,CanSpawnAnywhere = true},
            new Agency("~p~", "FIB", "Federal Investigation Bureau", "Purple", Agency.Classification.Federal, FIBPeds, FIBVehicles, "FIB ",BestWeapons) {MaxWantedLevelSpawn = 4, SpawnLimit = 6,CanSpawnAnywhere = true },
            new Agency("~p~", "DOA", "Drug Observation Agency", "Purple", Agency.Classification.Federal, DOAPeds, UnmarkedVehicles, "DOA ",AllWeapons)  {MaxWantedLevelSpawn = 3, SpawnLimit = 4,CanSpawnAnywhere = true },

            new Agency("~y~", "SAHP", "San Andreas Highway Patrol", "Yellow", Agency.Classification.State, SAHPPeds, SAHPVehicles, "HP ",LimitedWeapons) { MaxWantedLevelSpawn = 3, SpawnsOnHighway = true },
            new Agency("~o~", "SASPA", "San Andreas State Prison Authority", "Orange", Agency.Classification.State, PrisonPeds, PrisonVehicles, "SASPA ",AllWeapons) { MaxWantedLevelSpawn = 3, SpawnLimit = 2 },
            new Agency("~g~", "SAPR", "San Andreas Park Ranger", "Green", Agency.Classification.State, ParkRangers, ParkRangerVehicles, "",AllWeapons) { MaxWantedLevelSpawn = 3, SpawnLimit = 3 },            
            new Agency("~o~", "SACG", "San Andreas Coast Guard", "DarkOrange", Agency.Classification.State, CoastGuardPeds, CoastGuardVehicles, "SACG ",LimitedWeapons){ MaxWantedLevelSpawn = 3,SpawnLimit = 3 },

            new Agency("~p~", "LSPA", "Port Authority of Los Santos", "LightGray", Agency.Classification.Police, SecurityPeds, UnmarkedVehicles, "LSPA ",LimitedWeapons) {MaxWantedLevelSpawn = 3, SpawnLimit = 3 },
            new Agency("~p~", "LSIAPD", "Los Santos International Airport Police Department", "LightBlue", Agency.Classification.Police, StandardCops, LSPDVehicles, "LSA ",AllWeapons) { MaxWantedLevelSpawn = 3, SpawnLimit = 3 },

            new Agency("~o~", "PRISEC", "Private Security", "White", Agency.Classification.Security, SecurityPeds, SecurityVehicles, "",LimitedWeapons) {MaxWantedLevelSpawn = 1, SpawnLimit = 1 },

            new Agency("~u~", "ARMY", "Army", "Black", Agency.Classification.Military, MilitaryPeds, ArmyVehicles, "",BestWeapons) { MinWantedLevelSpawn = 5,CanSpawnAnywhere = true },


            new Agency("~s~", "UNK", "Unknown Agency", "White", Agency.Classification.Other, null, null, "",null) { MaxWantedLevelSpawn = 0 },

        };
      
    }
    private static void DispatchWorksDefaultConfig()
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
            new Agency.VehicleInformation("dw_cgheli", 75, 50),
            new Agency.VehicleInformation("dw_cgpredato", 75, 50),
            new Agency.VehicleInformation("dw_cgdinghy", 0, 25),
            new Agency.VehicleInformation("dw_cgexecutioner", 50, 50),
            new Agency.VehicleInformation("dw_cgsandking", 50, 50),
            new Agency.VehicleInformation("seashark2", 25, 25) { MaxOccupants = 1 },};


        List<Agency.VehicleInformation> SecurityVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("dilettante2", 100, 100) {MaxOccupants = 1 } };


        List<Agency.VehicleInformation> ParkRangerVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("pranger", 100, 100) };


        List<Agency.VehicleInformation> FIBVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("fbi", 70, 70),
            new Agency.VehicleInformation("fbi2", 30, 30) };


        List<Agency.VehicleInformation> NOOSEVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("dw_noosepia3", 70, 70) {MaxWantedLevelSpawn = 3 },
            new Agency.VehicleInformation("dw_noosepia2", 30, 30) {MaxWantedLevelSpawn = 3 },
            new Agency.VehicleInformation("riot", 0, 100) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 5, AllowedPedModels = new List<string>() { "s_m_y_swat_01" },MinOccupants = 2, MaxOccupants = 3 },
            new Agency.VehicleInformation("annihilator", 0, 100) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 5, AllowedPedModels = new List<string>() { "s_m_y_swat_01" },MinOccupants = 3,MaxOccupants = 4 }};


        List<Agency.VehicleInformation> HighwayPatrolVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("policeb", 33, 33) { MaxOccupants = 1 },
            new Agency.VehicleInformation("dw_sahpb2", 33, 33) { MaxOccupants = 1 },
            new Agency.VehicleInformation("dw_sahpb", 33, 33) { MaxOccupants = 1 },

            new Agency.VehicleInformation("dw_sahp1a", 10, 10),
            new Agency.VehicleInformation("dw_sahp1b", 10, 10),
            new Agency.VehicleInformation("dw_sahp1c", 10, 10),
            new Agency.VehicleInformation("dw_sahp22", 10, 10),
            new Agency.VehicleInformation("dw_sahp22a", 10, 10),
            new Agency.VehicleInformation("dw_sahp2", 10, 10),
            new Agency.VehicleInformation("dw_sahp2a", 10, 10),
            new Agency.VehicleInformation("dw_sahp5", 10, 10),
            new Agency.VehicleInformation("dw_sahp5a", 10, 10),
            new Agency.VehicleInformation("dw_sahp3", 10, 10),
            new Agency.VehicleInformation("dw_sahp3a", 10, 10),



        };


        List<Agency.VehicleInformation> PrisonVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("policet", 70, 70),
            new Agency.VehicleInformation("dw_saspacar2", 30, 30) };


        List<Agency.VehicleInformation> LSPDVehiclesVanilla = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("police", 48,35) { Liveries = new List<int>() { 0,1,2,3,4,5 } },
            new Agency.VehicleInformation("police2", 25, 20) { Liveries = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 } },
            new Agency.VehicleInformation("police3", 25, 20) { Liveries = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 } },
            new Agency.VehicleInformation("dw_lspd2", 25, 20),
            new Agency.VehicleInformation("dw_pscout", 1,1),
            new Agency.VehicleInformation("dw_lspd1", 1,1),
            new Agency.VehicleInformation("policet", 0, 25) { MinWantedLevelSpawn = 3} };


        List<Agency.VehicleInformation> LSSDVehiclesVanilla = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("sheriff", 50, 50){ Liveries = new List<int> { 0, 1, 2, 3 } },
            new Agency.VehicleInformation("sheriff2", 50, 50),

            new Agency.VehicleInformation("dw_sheriffgauntlet", 50, 50),
            new Agency.VehicleInformation("dw_sherifffug", 50, 50),
            new Agency.VehicleInformation("dw_lssd2", 50, 50),
            new Agency.VehicleInformation("dw_sheriffinterceptor", 50, 50),
            new Agency.VehicleInformation("dw_sheriffscout", 50, 50),
            new Agency.VehicleInformation("dw_lssd1", 50, 50),


        };

        List<Agency.VehicleInformation> LSPDVehicles = LSPDVehiclesVanilla;
        List<Agency.VehicleInformation> SAHPVehicles = HighwayPatrolVehicles;
        List<Agency.VehicleInformation> LSSDVehicles = LSSDVehiclesVanilla;

        List<Agency.VehicleInformation> BCSOVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("dw_bcsostanier", 40, 40),
            new Agency.VehicleInformation("dw_bcsoscout", 10, 10),
            new Agency.VehicleInformation("dw_bcsogranger", 20, 20),
            new Agency.VehicleInformation("dw_bcsogranger2", 10, 10),
            new Agency.VehicleInformation("dw_bcsogranger4", 10, 10),
            new Agency.VehicleInformation("dw_bcsofugitive", 5, 5),
            new Agency.VehicleInformation("dw_bcsobison", 5, 5),

            
        };


    List<Agency.VehicleInformation> VWHillsLSSDVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("dw_sheriffscout", 100, 100) { Liveries = new List<int> { 0, 1, 2, 3 } } };


        List<Agency.VehicleInformation> ChumashLSSDVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("dw_sheriffscout", 100, 100) { Liveries = new List<int> { 0, 1, 2, 3 } } };


        List<Agency.VehicleInformation> LSSDDavisVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("dw_lssd1", 100, 100){ Liveries = new List<int> { 0, 1, 2, 3 } } };



        List<Agency.VehicleInformation> RHPDVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("dw_rhpolice3", 20, 20),
            new Agency.VehicleInformation("dw_rhpolice2", 40, 40),
            new Agency.VehicleInformation("dw_rhpolice", 40, 40),      };

        List<Agency.VehicleInformation> DPPDVehicles = new List<Agency.VehicleInformation>() {
            new Agency.VehicleInformation("dw_dppolice4", 10, 10),
            new Agency.VehicleInformation("dw_dppolice2", 40, 40),
            new Agency.VehicleInformation("dw_dppolice3", 40, 40),
            new Agency.VehicleInformation("dw_dppolice", 10, 10),
        };


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
            new Agency.VehicleInformation("dw_sheriffbuz", 0,75) { Liveries = new List<int>() { 2 }, MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 3 } };


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



        AgenciesList = new List<Agency>
        {
            new Agency("~b~", "LSPD", "Los Santos Police Department", "Blue", Agency.Classification.Police, StandardCops, LSPDVehicles, "LS ",AllWeapons) { MaxWantedLevelSpawn = 4 },
            new Agency("~b~", "LSPD-VW", "Los Santos Police - Vinewood Division", "Blue", Agency.Classification.Police, ExtendedStandardCops, VWPDVehicles, "LSV ",LimitedWeapons) { MaxWantedLevelSpawn = 4 },
            new Agency("~b~", "LSPD-ELS", "Los Santos Police - East Los Santos Division", "Blue", Agency.Classification.Police, ExtendedStandardCops, EastLSPDVehicles, "LSE ",LimitedWeapons) { MaxWantedLevelSpawn = 4 },
            new Agency("~b~", "LSPD-DP", "Los Santos Police - Del Pierro Division", "Blue", Agency.Classification.Police, StandardCops, DPPDVehicles, "VP ",AllWeapons) { MaxWantedLevelSpawn = 4 },
            new Agency("~b~", "LSPD-RH", "Los Santos Police - Rockford Hills Division", "Blue", Agency.Classification.Police, StandardCops, RHPDVehicles, "RH ",AllWeapons) { MaxWantedLevelSpawn = 4 },

            new Agency("~r~", "LSSD", "Los Santos County Sheriff", "Red", Agency.Classification.Sheriff, SheriffPeds, LSSDVehicles, "LSCS ",LimitedWeapons) { MaxWantedLevelSpawn = 4 },
            new Agency("~r~", "LSSD-VW", "Los Santos Sheriff - Vinewood Division", "Red", Agency.Classification.Sheriff, SheriffPeds, VWHillsLSSDVehicles, "LSCS ",LimitedWeapons) { MaxWantedLevelSpawn = 4 },
            new Agency("~r~", "LSSD-CH", "Los Santos Sheriff - Chumash Division", "Red", Agency.Classification.Sheriff, SheriffPeds, ChumashLSSDVehicles, "LSCS ",LimitedWeapons) { MaxWantedLevelSpawn = 4 },
            new Agency("~r~", "LSSD-BC", "Los Santos Sheriff - Blaine County Division", "Red", Agency.Classification.Sheriff, SheriffPeds, BCSOVehicles, "BCS ",LimitedWeapons) { MaxWantedLevelSpawn = 4 },

            new Agency("~b~", "LSPD-ASD", "Los Santos Police Department - Air Support Division", "Blue", Agency.Classification.Police, PoliceAndSwat, PoliceHeliVehicles, "ASD ",HeliWeapons) { MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4, SpawnLimit = 3 },
            new Agency("~r~", "LSSD-ASD", "Los Santos Sheriffs Department - Air Support Division", "Red", Agency.Classification.Sheriff, SheriffAndSwat, SheriffHeliVehicles, "ASD ",HeliWeapons) { MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4, SpawnLimit = 3 },

            new Agency("~r~", "NOOSE", "National Office of Security Enforcement", "DarkSlateGray", Agency.Classification.Federal, NOOSEPeds, NOOSEVehicles, "",BestWeapons) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 4,CanSpawnAnywhere = true },
            new Agency("~p~", "FIB", "Federal Investigation Bureau", "Purple", Agency.Classification.Federal, FIBPeds, FIBVehicles, "FIB ",BestWeapons) {MaxWantedLevelSpawn = 4, SpawnLimit = 3,CanSpawnAnywhere = true },
            new Agency("~p~", "DOA", "Drug Observation Agency", "Purple", Agency.Classification.Federal, DOAPeds, UnmarkedVehicles, "DOA ",AllWeapons)  {MaxWantedLevelSpawn = 4, SpawnLimit = 3,CanSpawnAnywhere = true },

            new Agency("~y~", "SAHP", "San Andreas Highway Patrol", "Yellow", Agency.Classification.State, SAHPPeds, SAHPVehicles, "HP ",LimitedWeapons) { MaxWantedLevelSpawn = 4, SpawnsOnHighway = true },
            new Agency("~o~", "SASPA", "San Andreas State Prison Authority", "Orange", Agency.Classification.State, PrisonPeds, PrisonVehicles, "SASPA ",AllWeapons) { MaxWantedLevelSpawn = 3, SpawnLimit = 2 },
            new Agency("~g~", "SAPR", "San Andreas Park Ranger", "Green", Agency.Classification.State, ParkRangers, ParkRangerVehicles, "",AllWeapons) { MaxWantedLevelSpawn = 3, SpawnLimit = 3 },
            new Agency("~o~", "SACG", "San Andreas Coast Guard", "DarkOrange", Agency.Classification.State, CoastGuardPeds, CoastGuardVehicles, "SACG ",LimitedWeapons){ MaxWantedLevelSpawn = 3,SpawnLimit = 3 },

            new Agency("~p~", "LSPA", "Port Authority of Los Santos", "LightGray", Agency.Classification.Police, SecurityPeds, UnmarkedVehicles, "LSPA ",LimitedWeapons) {MaxWantedLevelSpawn = 3, SpawnLimit = 3 },
            new Agency("~p~", "LSIAPD", "Los Santos International Airport Police Department", "LightBlue", Agency.Classification.Police, StandardCops, LSPDVehicles, "LSA ",AllWeapons) { MaxWantedLevelSpawn = 3, SpawnLimit = 3 },

            new Agency("~o~", "PRISEC", "Private Security", "White", Agency.Classification.Security, SecurityPeds, SecurityVehicles, "",LimitedWeapons) {MaxWantedLevelSpawn = 1, SpawnLimit = 1 },

            new Agency("~u~", "ARMY", "Army", "Black", Agency.Classification.Military, MilitaryPeds, ArmyVehicles, "",BestWeapons) { MinWantedLevelSpawn = 5,CanSpawnAnywhere = true },


            new Agency("~s~", "UNK", "Unknown Agency", "White", Agency.Classification.Other, null, null, "",null) { MaxWantedLevelSpawn = 0 },

        };

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
        Agency ToReturn;
        List<Agency> ModelMatchAgencies = AgenciesList.Where(x => x.CopModels != null && x.CopModels.Any(b => b.ModelName.ToLower() == Cop.Model.Name.ToLower())).ToList();
        if (ModelMatchAgencies.Count > 1)
        {
            Zone ZoneFound = Zones.GetZoneAtLocation(Cop.Position);
            if (ZoneFound != null)
            {
                foreach (Agency ZoneAgency in Jurisdiction.GetAgenciesAtZone(ZoneFound.InternalGameName))
                {
                    if (ModelMatchAgencies.Any(x => x.Initials == ZoneAgency.Initials))
                        return ZoneAgency;
                }
            }
        }
        ToReturn = ModelMatchAgencies.FirstOrDefault();
        if (ToReturn == null)
        {
            Debugging.WriteToLog("GetAgencyFromPed", string.Format("Couldnt get agency from {0} ped deleting", Cop.Model.Name));
            Cop.Delete();
        }
        return ToReturn;
    }
    public static Agency GetAgencyFromVehicle(Vehicle CopCar)
    {
        Agency ToReturn;
        List<Agency> ModelMatchAgencies = AgenciesList.Where(x => x.Vehicles != null && x.Vehicles.Any(b => b.ModelName.ToLower() == CopCar.Model.Name.ToLower())).ToList();
        if (ModelMatchAgencies.Count > 1)
        {
            Zone ZoneFound = Zones.GetZoneAtLocation(CopCar.Position);
            if (ZoneFound != null)
            {
                foreach (Agency ZoneAgency in Jurisdiction.GetAgenciesAtZone(ZoneFound.InternalGameName))
                {
                    if (ModelMatchAgencies.Any(x => x.Initials == ZoneAgency.Initials))
                        return ZoneAgency;
                }
            }
        }
        ToReturn = ModelMatchAgencies.FirstOrDefault();
        if (ToReturn == null)
        {
            Debugging.WriteToLog("GetAgencyFromPed", string.Format("Couldnt get agency from {0} car deleting", CopCar.Model.Name));
            CopCar.Delete();
        }
        return ToReturn;
    }
    public static void ChangeLivery(Vehicle CopCar, Agency AssignedAgency)
    {
        Agency.VehicleInformation MyVehicle = null;
        if (AssignedAgency != null && AssignedAgency.Vehicles != null && CopCar.Exists())
        {
            MyVehicle = AssignedAgency.Vehicles.Where(x => x.ModelName.ToLower() == CopCar.Model.Name.ToLower()).FirstOrDefault();
        }
        if (MyVehicle == null)
        {
            if (CopCar.Exists())
            {
                Debugging.WriteToLog("ChangeLivery", string.Format("No Match for Vehicle {0} for {1}", CopCar.Model.Name, AssignedAgency.Initials));
                CopCar.Delete();
            }
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
    public static Agency GetAgencyFromInitials(string AgencyInitials)
    {
        return AgenciesList.Where(x => x.Initials.ToLower() == AgencyInitials.ToLower()).FirstOrDefault();
    }
}

[Serializable()]
public class Agency
{
    public string ColorPrefix { get; set; } = "~s~";
    public string Initials { get; set; } = "UNK";
    public string FullName { get; set; } = "Unknown";
    public List<ModelInformation> CopModels { get; set; } = new List<ModelInformation>();
    public List<VehicleInformation> Vehicles { get; set; } = new List<VehicleInformation>();
    public string AgencyColorString { get; set; } = "White";
    public Classification AgencyClassification { get; set; } = Classification.Other;
    public string LicensePlatePrefix { get; set; } = "";
    public bool SpawnsOnHighway { get; set; } = false;
    public bool CanSpawnAnywhere { get; set; } = false;
    public uint MinWantedLevelSpawn { get; set; } = 0;
    public uint MaxWantedLevelSpawn { get; set; } = 5;
    public int SpawnLimit { get; set; } = 99;
    public List<IssuedWeapon> IssuedWeapons { get; set; } = new List<IssuedWeapon>();
    public bool CanSpawn
    {
        get
        {
            if (PlayerState.WantedLevel >= MinWantedLevelSpawn && PlayerState.WantedLevel <= MaxWantedLevelSpawn)
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
            if (AgencyClassification == Classification.Police || AgencyClassification == Classification.Federal || AgencyClassification == Classification.Sheriff || AgencyClassification == Classification.State)
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
        State = 3,
        Security = 4,
        Military = 5,
        Other = 6,
    }
    public VehicleInformation GetVehicleInfo(Vehicle CopCar)
    {
        return Vehicles.Where(x => x.ModelName.ToLower() == CopCar.Model.Name.ToLower()).FirstOrDefault();
    }
    public VehicleInformation GetRandomVehicle()
    {
        if (Vehicles == null || !Vehicles.Any())
            return null;

        List<VehicleInformation> ToPickFrom = Vehicles.Where(x => x.CanCurrentlySpawn).ToList();
        int Total = ToPickFrom.Sum(x => x.CurrentSpawnChance);
        // Debugging.WriteToLog("GetRandomVehicle", string.Format("Total Chance {0}, Items {1}", Total, string.Join(",",ToPickFrom.Select( x => x.ModelName + " " + x.CanCurrentlySpawn + "  " + x.CurrentSpawnChance))));
        int RandomPick = General.MyRand.Next(0, Total);
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

        List<ModelInformation> ToPickFrom = CopModels.Where(x => PlayerState.WantedLevel >= x.MinWantedLevelSpawn && PlayerState.WantedLevel <= x.MaxWantedLevelSpawn).ToList();
        if (RequiredModels != null && RequiredModels.Any())
        {
            ToPickFrom = ToPickFrom.Where(x => RequiredModels.Contains(x.ModelName.ToLower())).ToList();
        }
        int Total = ToPickFrom.Sum(x => x.CurrentSpawnChance);
        //Debugging.WriteToLog("GetRandomPed", string.Format("Total Chance {0}, Total Items {1}", Total, ToPickFrom.Count()));
        int RandomPick = General.MyRand.Next(0, Total);
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
    public Agency(string _ColorPrefix, string _Initials, string _FullName, string _AgencyColorString, Classification _AgencyClassification, List<ModelInformation> _CopModels, List<VehicleInformation> _Vehicles, string _LicensePlatePrefix, List<IssuedWeapon> _IssuedWeapons)
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
        public int MinWantedLevelSpawn = 0;
        public int MaxWantedLevelSpawn = 5;
        public PedVariation RequiredVariation;
        public bool CanCurrentlySpawn
        {
            get
            {
                if (PlayerState.IsWanted)
                {
                    if (PlayerState.WantedLevel >= MinWantedLevelSpawn && PlayerState.WantedLevel <= MaxWantedLevelSpawn)
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
                if (PlayerState.IsWanted)
                {
                    if (PlayerState.WantedLevel >= MinWantedLevelSpawn && PlayerState.WantedLevel <= MaxWantedLevelSpawn)
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
        public ModelInformation(string _ModelName, int ambientSpawnChance, int wantedSpawnChance)
        {
            ModelName = _ModelName;
            AmbientSpawnChance = ambientSpawnChance;
            WantedSpawnChance = wantedSpawnChance;
        }
    }
    public class VehicleInformation
    {
        public string ModelName;
        public int AmbientSpawnChance = 0;
        public int WantedSpawnChance = 0;
        public bool IsCar
        {
            get
            {
                return NativeFunction.CallByName<bool>("IS_THIS_MODEL_A_CAR", Game.GetHashKey(ModelName));
            }
        }
        public bool IsMotorcycle
        {
            get
            {
                return NativeFunction.CallByName<bool>("IS_THIS_MODEL_A_BIKE", Game.GetHashKey(ModelName));
            }
        }
        public bool IsHelicopter
        {
            get
            {
                return NativeFunction.CallByName<bool>("IS_THIS_MODEL_A_HELI", Game.GetHashKey(ModelName));
            }
        }
        public bool IsBoat
        {
            get
            {
                return NativeFunction.CallByName<bool>("IS_THIS_MODEL_A_BOAT", Game.GetHashKey(ModelName));
            }
        }
        public int MinOccupants = 1;
        public int MaxOccupants = 2;
        public int MinWantedLevelSpawn = 0;
        public int MaxWantedLevelSpawn = 5;
        public List<string> AllowedPedModels = new List<string>();//only ped models can spawn in this, if emptyt any ambient spawn can
        public List<int> Liveries = new List<int>();
        public bool CanSpawnWanted
        {
            get
            {
                if (WantedSpawnChance > 0)
                    return true;
                else
                    return false;
            }
        }
        public bool CanSpawnAmbient
        {
            get
            {
                if (AmbientSpawnChance > 0)
                    return true;
                else
                    return false;
            }
        }
        public bool CanCurrentlySpawn
        {
            get
            {
                if (IsHelicopter && PedList.PoliceVehicles.Count(x => x.IsHelicopter) >= General.MySettings.Police.HelicopterLimit)
                {
                    return false;
                }
                else if (IsBoat && PedList.PoliceVehicles.Count(x => x.IsBoat) >= General.MySettings.Police.BoatLimit)
                {
                    return false;
                }

                if (PlayerState.IsWanted)
                {
                    if (PlayerState.WantedLevel >= MinWantedLevelSpawn && PlayerState.WantedLevel <= MaxWantedLevelSpawn)
                        return CanSpawnWanted;
                    else
                        return false;
                }
                else
                    return CanSpawnAmbient;
            }
        }
        public int CurrentSpawnChance
        {
            get
            {
                if (!CanCurrentlySpawn)
                    return 0;
                if (PlayerState.IsWanted)
                {
                    if (PlayerState.WantedLevel >= MinWantedLevelSpawn && PlayerState.WantedLevel <= MaxWantedLevelSpawn)
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






