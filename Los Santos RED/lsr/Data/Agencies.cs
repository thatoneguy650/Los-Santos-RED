using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

public class Agencies
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Agencies.xml";
    private bool UseVanillaConfig = true;
    private int LikelyHoodOfAnySpawn = 5;
    private List<Agency> AgenciesList;

    public Agencies()
    {

    }

    public void ReadConfig()
    {
        if (File.Exists(ConfigFileName))
        {
            AgenciesList = Serialization.DeserializeParams<Agency>(ConfigFileName);
        }
        else
        {
            if (UseVanillaConfig)
            {
                DefaultConfig();
            }
            else
            {
                CustomConfig();
            }
            Serialization.SerializeParams(AgenciesList, ConfigFileName);
        }
    }
    public List<Agency> GetAgencies(Vector3 Position)
    {
        List<Agency> ToReturn = new List<Agency>();
        Street StreetAtPosition = Mod.DataMart.Streets.GetStreet(Position);
        if(StreetAtPosition != null && Mod.DataMart.Streets.GetStreet(Position).IsHighway) //Highway Patrol Jurisdiction
        {
            ToReturn.AddRange(AgenciesList.Where(x => x.CanSpawn && x.SpawnsOnHighway));
        }
        Zone CurrentZone = Mod.DataMart.Zones.GetZone(Position);

        Agency ZoneAgency1 = Mod.DataMart.ZoneJurisdiction.GetRandomAgency(CurrentZone.InternalGameName);
        if (ZoneAgency1 != null)
        {
            ToReturn.Add(ZoneAgency1); //Zone Jurisdiciton Random
        }

        Agency CountyAgency1 = Mod.DataMart.CountyJurisdictions.GetRandomAgency(CurrentZone.InternalGameName);
        if (CountyAgency1 != null)
        {
            ToReturn.Add(CountyAgency1); //Zone Jurisdiciton Random
        }

        if (!ToReturn.Any() || RandomItems.RandomPercent(LikelyHoodOfAnySpawn))
        {
            ToReturn.AddRange(AgenciesList.Where(x => x.CanSpawn && x.CanSpawnAnywhere));
        }
        foreach (Agency ag in ToReturn)
        {
            Mod.Debug.WriteToLog("Debugging", string.Format("Agencies At Pos: {0}", ag.Initials));
        }
        return ToReturn;
    }
    public Agency GetAgency(Ped Cop)
    {
        if (!Cop.IsPoliceArmy())
        {
            return null;
        }
        if (Cop.IsArmy())
        {
            return AgenciesList.Where(x => x.AgencyClassification == Classification.Military).FirstOrDefault();
        }
        else if (Cop.IsPolice())
        {
            Agency ToReturn;
            List<Agency> ModelMatchAgencies = AgenciesList.Where(x => x.CopModels != null && x.CopModels.Any(b => b.ModelName.ToLower() == Cop.Model.Name.ToLower())).ToList();
            if (ModelMatchAgencies.Count > 1)
            {
                Zone ZoneFound = Mod.DataMart.Zones.GetZone(Cop.Position);
                if (ZoneFound != null)
                {
                    foreach (Agency ZoneAgency in Mod.DataMart.ZoneJurisdiction.GetAgencies(ZoneFound.InternalGameName))
                    {
                        if (ModelMatchAgencies.Any(x => x.Initials == ZoneAgency.Initials))
                            return ZoneAgency;
                    }
                }
            }
            ToReturn = ModelMatchAgencies.FirstOrDefault();
            if (ToReturn == null)
            {
                Mod.Debug.WriteToLog("GetAgencyFromPed", string.Format("Couldnt get agency from {0} ped deleting", Cop.Model.Name));
                Cop.Delete();
            }
            return ToReturn;
        }
        else
        {
            return null;
        }
    }
    public Agency GetAgency(Vehicle CopCar)
    {
        Agency ToReturn;
        List<Agency> ModelMatchAgencies = AgenciesList.Where(x => x.Vehicles != null && x.Vehicles.Any(b => b.ModelName.ToLower() == CopCar.Model.Name.ToLower())).ToList();
        if (ModelMatchAgencies.Count > 1)
        {
            Zone ZoneFound = Mod.DataMart.Zones.GetZone(CopCar.Position);
            if (ZoneFound != null)
            {
                foreach (Agency ZoneAgency in Mod.DataMart.ZoneJurisdiction.GetAgencies(ZoneFound.InternalGameName))
                {
                    if (ModelMatchAgencies.Any(x => x.Initials == ZoneAgency.Initials))
                        return ZoneAgency;
                }
            }
        }
        ToReturn = ModelMatchAgencies.FirstOrDefault();
        if (ToReturn == null)
        {
            Mod.Debug.WriteToLog("GetAgencyFromPed", string.Format("Couldnt get agency from {0} car deleting", CopCar.Model.Name));
            CopCar.Delete();
        }
        return ToReturn;
    }
    public Agency GetAgency(string AgencyInitials)
    {
        return AgenciesList.Where(x => x.Initials.ToLower() == AgencyInitials.ToLower()).FirstOrDefault();
    }
    private void DefaultConfig()
    {
        //Peds
        List<PedestrianInformation> StandardCops = new List<PedestrianInformation>() {
            new PedestrianInformation("s_m_y_cop_01",85,85),
            new PedestrianInformation("s_f_y_cop_01",15,15) };
        List<PedestrianInformation> ExtendedStandardCops = new List<PedestrianInformation>() {
            new PedestrianInformation("s_m_y_cop_01",85,85),
            new PedestrianInformation("s_f_y_cop_01",10,10),
            new PedestrianInformation("ig_trafficwarden",5,5) };
        List<PedestrianInformation> ParkRangers = new List<PedestrianInformation>() {
            new PedestrianInformation("s_m_y_ranger_01",75,75),
            new PedestrianInformation("s_f_y_ranger_01",25,25) };
        List<PedestrianInformation> SheriffPeds = new List<PedestrianInformation>() {
            new PedestrianInformation("s_m_y_sheriff_01",75,75),
            new PedestrianInformation("s_f_y_sheriff_01",25,25) };
        List<PedestrianInformation> SWAT = new List<PedestrianInformation>() {
            new PedestrianInformation("s_m_y_swat_01", 100,100) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(10, 0, 0,0) },new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) }) } };
        List<PedestrianInformation> PoliceAndSwat = new List<PedestrianInformation>() {
            new PedestrianInformation("s_m_y_cop_01",70,0),
            new PedestrianInformation("s_f_y_cop_01",30,0),
            new PedestrianInformation("s_m_y_swat_01", 0,100) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(10, 0, 0,0) },new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) }) } };
        List<PedestrianInformation> SheriffAndSwat = new List<PedestrianInformation>() {
            new PedestrianInformation("s_m_y_sheriff_01", 75, 0),
            new PedestrianInformation("s_f_y_sheriff_01", 25, 0),
            new PedestrianInformation("s_m_y_swat_01", 0, 100) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(10, 0, 0,0) },new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) }) } };
        List<PedestrianInformation> DOAPeds = new List<PedestrianInformation>() {
            new PedestrianInformation("u_m_m_doa_01",100,100) };
        List<PedestrianInformation> IAAPeds = new List<PedestrianInformation>() {
            new PedestrianInformation("s_m_m_fibsec_01",100,100) };
        List<PedestrianInformation> SAHPPeds = new List<PedestrianInformation>() {
            new PedestrianInformation("s_m_y_hwaycop_01",100,100) };
        List<PedestrianInformation> MilitaryPeds = new List<PedestrianInformation>() {
            new PedestrianInformation("s_m_y_armymech_01",25,0),
            new PedestrianInformation("s_m_m_marine_01",50,0),
            new PedestrianInformation("s_m_m_marine_02",0,0),
            new PedestrianInformation("s_m_y_marine_01",25,0),
            new PedestrianInformation("s_m_y_marine_02",0,0),
            new PedestrianInformation("s_m_y_marine_03",100,100) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(2, 1, 0, 0),new PedComponent(8, 0, 0, 0) },new List<PedPropComponent>() { new PedPropComponent(3, 1, 0) }) },
            new PedestrianInformation("s_m_m_pilot_02",0,0),
            new PedestrianInformation("s_m_y_pilot_01",0,0) };
        List<PedestrianInformation> FIBPeds = new List<PedestrianInformation>() {
            new PedestrianInformation("s_m_m_fibsec_01",55,70){MaxWantedLevelSpawn = 3 },
            new PedestrianInformation("s_m_m_fiboffice_01",15,0){MaxWantedLevelSpawn = 3 },
            new PedestrianInformation("s_m_m_fiboffice_02",15,0){MaxWantedLevelSpawn = 3 },
            new PedestrianInformation("u_m_m_fibarchitect",10,0) {MaxWantedLevelSpawn = 3 },
            new PedestrianInformation("s_m_y_swat_01", 5,30) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 4, RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(10, 0, 1,0) },new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) }) } };
        List<PedestrianInformation> PrisonPeds = new List<PedestrianInformation>() {
            new PedestrianInformation("s_m_m_prisguard_01",100,100) };
        List<PedestrianInformation> SecurityPeds = new List<PedestrianInformation>() {
            new PedestrianInformation("s_m_m_security_01",100,100) };
        List<PedestrianInformation> CoastGuardPeds = new List<PedestrianInformation>() {
            new PedestrianInformation("s_m_y_uscg_01",100,100) };
        List<PedestrianInformation> NOOSEPeds = new List<PedestrianInformation>() {
            new PedestrianInformation("s_m_y_swat_01", 100,100) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(10, 0, 0,0) },new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) }) } };

        //Vehicles
        List<VehicleInformation> UnmarkedVehicles = new List<VehicleInformation>() {
            new VehicleInformation("police4", 100, 100) };
        List<VehicleInformation> CoastGuardVehicles = new List<VehicleInformation>() {
            new VehicleInformation("predator", 75, 50),
            new VehicleInformation("dinghy", 0, 25),
            new VehicleInformation("seashark2", 25, 25) { MaxOccupants = 1 },};      
        List<VehicleInformation> SecurityVehicles = new List<VehicleInformation>() {
            new VehicleInformation("dilettante2", 100, 100) {MaxOccupants = 1 } };
        List<VehicleInformation> ParkRangerVehicles = new List<VehicleInformation>() {
            new VehicleInformation("pranger", 100, 100) };
        List<VehicleInformation> FIBVehicles = new List<VehicleInformation>() {
            new VehicleInformation("fbi", 70, 70){ MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new VehicleInformation("fbi2", 30, 30) { MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new VehicleInformation("fbi2", 0, 30) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 4, AllowedPedModels = new List<string>() { "s_m_y_swat_01" },MinOccupants = 4, MaxOccupants = 6 },
        };
        List<VehicleInformation> NOOSEVehicles = new List<VehicleInformation>() {
            new VehicleInformation("fbi", 70, 70){ MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new VehicleInformation("fbi2", 30, 30) { MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new VehicleInformation("fbi2", 0, 30) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 4, AllowedPedModels = new List<string>() { "s_m_y_swat_01" },MinOccupants = 4, MaxOccupants = 6 },
            new VehicleInformation("riot", 0, 70) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 4, AllowedPedModels = new List<string>() { "s_m_y_swat_01" },MinOccupants = 2, MaxOccupants = 3 },
            new VehicleInformation("annihilator", 0, 100) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 4, AllowedPedModels = new List<string>() { "s_m_y_swat_01" },MinOccupants = 3,MaxOccupants = 4 }};
        List<VehicleInformation> HighwayPatrolVehicles = new List<VehicleInformation>() {
            new VehicleInformation("policeb", 70, 70) { MaxOccupants = 1 },
            new VehicleInformation("police4", 30, 30) };
        List<VehicleInformation> PrisonVehicles = new List<VehicleInformation>() {
            new VehicleInformation("policet", 70, 70),
            new VehicleInformation("police4", 30, 30) };
        List<VehicleInformation> LSPDVehiclesVanilla = new List<VehicleInformation>() {
            new VehicleInformation("police", 48,35) { Liveries = new List<int>() { 0,1,2,3,4,5 } },
            new VehicleInformation("police2", 25, 20) { Liveries = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 } },
            new VehicleInformation("police3", 25, 20) { Liveries = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 } },
            new VehicleInformation("police4", 1,1),
            new VehicleInformation("fbi2", 1,1),
            new VehicleInformation("policet", 0, 25) { MinWantedLevelSpawn = 3} };
        List<VehicleInformation> LSSDVehiclesVanilla = new List<VehicleInformation>() {
            new VehicleInformation("sheriff", 50, 50){ Liveries = new List<int> { 0, 1, 2, 3 } },
            new VehicleInformation("sheriff2", 50, 50) { Liveries = new List<int> { 0, 1, 2, 3 } } };
        List<VehicleInformation> LSPDVehicles = LSPDVehiclesVanilla;
        List<VehicleInformation> SAHPVehicles = HighwayPatrolVehicles;
        List<VehicleInformation> LSSDVehicles = LSSDVehiclesVanilla;
        List<VehicleInformation> BCSOVehicles = LSSDVehiclesVanilla;
        List<VehicleInformation> VWHillsLSSDVehicles = new List<VehicleInformation>() {
            new VehicleInformation("sheriff2", 100, 100) { Liveries = new List<int> { 0, 1, 2, 3 } } };
        List<VehicleInformation> ChumashLSSDVehicles = new List<VehicleInformation>() {
            new VehicleInformation("sheriff2", 100, 100) { Liveries = new List<int> { 0, 1, 2, 3 } } };
        List<VehicleInformation> LSSDDavisVehicles = new List<VehicleInformation>() {
            new VehicleInformation("sheriff", 100, 100){ Liveries = new List<int> { 0, 1, 2, 3 } } };
        List<VehicleInformation> RHPDVehicles = new List<VehicleInformation>() {
            new VehicleInformation("police2", 100, 75) { Liveries = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 } },
            new VehicleInformation("policet", 0, 25) { MinWantedLevelSpawn = 3} };
        List<VehicleInformation> DPPDVehicles = new List<VehicleInformation>() {
            new VehicleInformation("police3", 100, 75) { Liveries = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 } },
            new VehicleInformation("policet", 0, 25) { MinWantedLevelSpawn = 3} };
        List<VehicleInformation> ChumashLSPDVehicles = new List<VehicleInformation>() {
            new VehicleInformation("police3", 100, 75) { Liveries = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 } },
            new VehicleInformation("policet", 0, 25) { MinWantedLevelSpawn = 3} };
        List<VehicleInformation> EastLSPDVehicles = new List<VehicleInformation>() {
            new VehicleInformation("police", 100,75) { Liveries = new List<int>() { 0,1,2,3,4,5 } },
            new VehicleInformation("policet", 0, 25) { MinWantedLevelSpawn = 3} };
        List<VehicleInformation> VWPDVehicles = new List<VehicleInformation>() {
            new VehicleInformation("police", 100,75) { Liveries = new List<int>() { 0,1,2,3,4,5 } },
            new VehicleInformation("policet", 0, 25) { MinWantedLevelSpawn = 3} };
        List<VehicleInformation> PoliceHeliVehicles = new List<VehicleInformation>() {
            new VehicleInformation("polmav", 0,100) { Liveries = new List<int>() { 0 }, MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 3 } };
        List<VehicleInformation> SheriffHeliVehicles = new List<VehicleInformation>() {
            new VehicleInformation("buzzard2", 0,25) { Liveries = new List<int>() { 0 }, MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 3 },
            new VehicleInformation("polmav", 0,75) { Liveries = new List<int>() { 0 }, MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 3 } };
        List<VehicleInformation> ArmyVehicles = new List<VehicleInformation>() {
            new VehicleInformation("crusader", 75,50) { Liveries = new List<int>() { 0 },MinOccupants = 1,MaxOccupants = 2,MaxWantedLevelSpawn = 4 },
            new VehicleInformation("barracks", 25,50) { Liveries = new List<int>() { 0 },MinOccupants = 3,MaxOccupants = 5,MinWantedLevelSpawn = 4 },
            new VehicleInformation("rhino", 0,10) { Liveries = new List<int>() { 0 },MinOccupants = 1,MaxOccupants = 2,MinWantedLevelSpawn = 5 },
            new VehicleInformation("valkyrie", 0,50) { Liveries = new List<int>() { 0 },MinOccupants = 3,MaxOccupants = 3,MinWantedLevelSpawn = 4 },
            new VehicleInformation("valkyrie2", 0,50) { Liveries = new List<int>() { 0 },MinOccupants = 3,MaxOccupants = 3,MinWantedLevelSpawn = 4 },
        };

        //Weapon
        List<IssuedWeapon> AllWeapons = new List<IssuedWeapon>()
        {
            // Pistols
            new IssuedWeapon("weapon_pistol", true, new WeaponVariation()),
            new IssuedWeapon("weapon_pistol", true, new WeaponVariation(0, new List<string> { "Flashlight" })),
            new IssuedWeapon("weapon_pistol", true, new WeaponVariation(0,new List<string> { "Extended Clip" })),
            new IssuedWeapon("weapon_pistol", true, new WeaponVariation(0,new List<string> { "Flashlight","Extended Clip" })),

            new IssuedWeapon("weapon_pistol_mk2", true ,new WeaponVariation()),
            new IssuedWeapon("weapon_pistol_mk2", true, new WeaponVariation(0,new List<string> { "Flashlight" })),
            new IssuedWeapon("weapon_pistol_mk2", true, new WeaponVariation(0,new List<string> { "Extended Clip" })),
            new IssuedWeapon("weapon_pistol_mk2", true, new WeaponVariation(0, new List<string> { "Flashlight","Extended Clip" })),

            new IssuedWeapon("weapon_combatpistol", true, new WeaponVariation()),
            new IssuedWeapon("weapon_combatpistol", true, new WeaponVariation(0, new List<string> { "Flashlight" })),
            new IssuedWeapon("weapon_combatpistol", true, new WeaponVariation(0,new List<string> { "Extended Clip" })),
            new IssuedWeapon("weapon_combatpistol", true, new WeaponVariation(0, new List<string> { "Flashlight","Extended Clip" })),

            new IssuedWeapon("weapon_heavypistol", true, new WeaponVariation()),
            new IssuedWeapon("weapon_heavypistol", true, new WeaponVariation(0,new List<string> { "Etched Wood Grip Finish" })),
            new IssuedWeapon("weapon_heavypistol", true, new WeaponVariation(0,new List<string> { "Flashlight","Extended Clip" })),
            new IssuedWeapon("weapon_heavypistol", true, new WeaponVariation(0,new List<string> { "Extended Clip" })),

            // Shotguns
            new IssuedWeapon("weapon_pumpshotgun", false, new WeaponVariation()),
            new IssuedWeapon("weapon_pumpshotgun", false, new WeaponVariation(0,new List<string> { "Flashlight" })),

            new IssuedWeapon("weapon_pumpshotgun_mk2", false, new WeaponVariation()),
            new IssuedWeapon("weapon_pumpshotgun_mk2", false, new WeaponVariation(0, new List<string> { "Flashlight" })),
            new IssuedWeapon("weapon_pumpshotgun_mk2", false, new WeaponVariation(0, new List<string> { "Holographic Sight" })),
            new IssuedWeapon("weapon_pumpshotgun_mk2", false, new WeaponVariation(0, new List<string> { "Flashlight","Holographic Sight" })),

            // ARs
            new IssuedWeapon("weapon_carbinerifle", false, new WeaponVariation()),
            new IssuedWeapon("weapon_carbinerifle", false, new WeaponVariation(0,new List<string> { "Grip","Flashlight" })),
            new IssuedWeapon("weapon_carbinerifle", false, new WeaponVariation(0, new List<string> { "Scope", "Grip","Flashlight" })),
            new IssuedWeapon("weapon_carbinerifle", false, new WeaponVariation(0,new List<string> { "Scope", "Grip","Flashlight","Extended Clip" })),

            new IssuedWeapon("weapon_carbinerifle_mk2", false, new WeaponVariation()),
            new IssuedWeapon("weapon_carbinerifle_mk2", false, new WeaponVariation(0, new List<string> { "Holographic Sight","Grip","Flashlight" })),
            new IssuedWeapon("weapon_carbinerifle_mk2", false, new WeaponVariation(0, new List<string> { "Holographic Sight", "Grip","Extended Clip" })),
            new IssuedWeapon("weapon_carbinerifle_mk2", false, new WeaponVariation(0, new List<string> { "Large Scope", "Grip","Flashlight","Extended Clip" })),
        };
        List<IssuedWeapon> BestWeapons = new List<IssuedWeapon>()
        {
            new IssuedWeapon("weapon_pistol_mk2", true, new WeaponVariation(0,new List<string> { "Flashlight" })),
            new IssuedWeapon("weapon_pistol_mk2", true, new WeaponVariation(0,new List<string> { "Extended Clip" })),
            new IssuedWeapon("weapon_pistol_mk2", true, new WeaponVariation(0, new List<string> { "Flashlight","Extended Clip" })),
            new IssuedWeapon("weapon_carbinerifle_mk2", false, new WeaponVariation(0, new List<string> { "Holographic Sight","Grip","Flashlight" })),
            new IssuedWeapon("weapon_carbinerifle_mk2", false, new WeaponVariation(0, new List<string> { "Holographic Sight", "Grip","Extended Clip" })),
            new IssuedWeapon("weapon_carbinerifle_mk2", false, new WeaponVariation(0, new List<string> { "Large Scope", "Grip","Flashlight","Extended Clip" })),
        };
        List<IssuedWeapon> HeliWeapons = new List<IssuedWeapon>()
        {
            new IssuedWeapon("weapon_pistol_mk2", true, new WeaponVariation(0,new List<string> { "Flashlight" })),
            new IssuedWeapon("weapon_pistol_mk2", true, new WeaponVariation(0,new List<string> { "Extended Clip" })),
            new IssuedWeapon("weapon_pistol_mk2", true, new WeaponVariation(0, new List<string> { "Flashlight","Extended Clip" })),
            new IssuedWeapon("weapon_marksmanrifle_mk2", false, new WeaponVariation(0, new List<string> { "Large Scope", "Suppressor", "Tracer Rounds" })),
            new IssuedWeapon("weapon_marksmanrifle_mk2", false, new WeaponVariation(0, new List<string> { "Large Scope","Tracer Rounds" })),
        };
        List<IssuedWeapon> LimitedWeapons = new List<IssuedWeapon>()
        {
            new IssuedWeapon("weapon_heavypistol", true, new WeaponVariation()),
            new IssuedWeapon("weapon_revolver", true, new WeaponVariation()),
            new IssuedWeapon("weapon_heavypistol", true, new WeaponVariation(0,new List<string> { "Flashlight" })),
            new IssuedWeapon("weapon_pumpshotgun", false, new WeaponVariation()),
            new IssuedWeapon("weapon_pumpshotgun", false, new WeaponVariation(0,new List<string> { "Flashlight" })),

        };

        AgenciesList = new List<Agency>
        {
            new Agency("~b~", "LSPD", "Los Santos Police Department", "Blue", Classification.Police, StandardCops, LSPDVehicles, "LS ",AllWeapons) { MaxWantedLevelSpawn = 3 },
            new Agency("~b~", "LSPD-VW", "Los Santos Police - Vinewood Division", "Blue", Classification.Police, ExtendedStandardCops, VWPDVehicles, "LSV ",LimitedWeapons) { MaxWantedLevelSpawn = 3 },
            new Agency("~b~", "LSPD-ELS", "Los Santos Police - East Los Santos Division", "Blue", Classification.Police, ExtendedStandardCops, EastLSPDVehicles, "LSE ",LimitedWeapons) { MaxWantedLevelSpawn = 3 },
            new Agency("~b~", "LSPD-DP", "Los Santos Police - Del Pierro Division", "Blue", Classification.Police, StandardCops, DPPDVehicles, "VP ",AllWeapons) { MaxWantedLevelSpawn = 3 },
            new Agency("~b~", "LSPD-RH", "Los Santos Police - Rockford Hills Division", "Blue", Classification.Police, StandardCops, RHPDVehicles, "RH ",AllWeapons) { MaxWantedLevelSpawn = 3 },

            new Agency("~r~", "LSSD", "Los Santos County Sheriff", "Red", Classification.Sheriff, SheriffPeds, LSSDVehicles, "LSCS ",LimitedWeapons) { MaxWantedLevelSpawn = 3 },
            new Agency("~r~", "LSSD-VW", "Los Santos Sheriff - Vinewood Division", "Red", Classification.Sheriff, SheriffPeds, VWHillsLSSDVehicles, "LSCS ",LimitedWeapons) { MaxWantedLevelSpawn = 3 },
            new Agency("~r~", "LSSD-CH", "Los Santos Sheriff - Chumash Division", "Red", Classification.Sheriff, SheriffPeds, ChumashLSSDVehicles, "LSCS ",LimitedWeapons) { MaxWantedLevelSpawn = 3 },
            new Agency("~r~", "LSSD-BC", "Los Santos Sheriff - Blaine County Division", "Red", Classification.Sheriff, SheriffPeds, BCSOVehicles, "BCS ",LimitedWeapons) { MaxWantedLevelSpawn = 3 },

            new Agency("~b~", "LSPD-ASD", "Los Santos Police Department - Air Support Division", "Blue", Classification.Police, PoliceAndSwat, PoliceHeliVehicles, "ASD ",HeliWeapons) { MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4, SpawnLimit = 3 },
            new Agency("~r~", "LSSD-ASD", "Los Santos Sheriffs Department - Air Support Division", "Red", Classification.Sheriff, SheriffAndSwat, SheriffHeliVehicles, "ASD ",HeliWeapons) { MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4, SpawnLimit = 3 },

            new Agency("~r~", "NOOSE", "National Office of Security Enforcement", "DarkSlateGray", Classification.Federal, NOOSEPeds, NOOSEVehicles, "",BestWeapons) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 4,CanSpawnAnywhere = true},
            new Agency("~p~", "FIB", "Federal Investigation Bureau", "Purple", Classification.Federal, FIBPeds, FIBVehicles, "FIB ",BestWeapons) {MaxWantedLevelSpawn = 4, SpawnLimit = 6,CanSpawnAnywhere = true },
            new Agency("~p~", "DOA", "Drug Observation Agency", "Purple", Classification.Federal, DOAPeds, UnmarkedVehicles, "DOA ",AllWeapons)  {MaxWantedLevelSpawn = 3, SpawnLimit = 4,CanSpawnAnywhere = true },

            new Agency("~y~", "SAHP", "San Andreas Highway Patrol", "Yellow", Classification.State, SAHPPeds, SAHPVehicles, "HP ",LimitedWeapons) { MaxWantedLevelSpawn = 3, SpawnsOnHighway = true },
            new Agency("~o~", "SASPA", "San Andreas State Prison Authority", "Orange", Classification.State, PrisonPeds, PrisonVehicles, "SASPA ",AllWeapons) { MaxWantedLevelSpawn = 3, SpawnLimit = 2 },
            new Agency("~g~", "SAPR", "San Andreas Park Ranger", "Green", Classification.State, ParkRangers, ParkRangerVehicles, "",AllWeapons) { MaxWantedLevelSpawn = 3, SpawnLimit = 3 },            
            new Agency("~o~", "SACG", "San Andreas Coast Guard", "DarkOrange", Classification.State, CoastGuardPeds, CoastGuardVehicles, "SACG ",LimitedWeapons){ MaxWantedLevelSpawn = 3,SpawnLimit = 3 },

            new Agency("~p~", "LSPA", "Port Authority of Los Santos", "LightGray", Classification.Police, SecurityPeds, UnmarkedVehicles, "LSPA ",LimitedWeapons) {MaxWantedLevelSpawn = 3, SpawnLimit = 3 },
            new Agency("~p~", "LSIAPD", "Los Santos International Airport Police Department", "LightBlue", Classification.Police, StandardCops, LSPDVehicles, "LSA ",AllWeapons) { MaxWantedLevelSpawn = 3, SpawnLimit = 3 },

            new Agency("~o~", "PRISEC", "Private Security", "White", Classification.Security, SecurityPeds, SecurityVehicles, "",LimitedWeapons) {MaxWantedLevelSpawn = 1, SpawnLimit = 1 },

            new Agency("~u~", "ARMY", "Army", "Black", Classification.Military, MilitaryPeds, ArmyVehicles, "",BestWeapons) { MinWantedLevelSpawn = 5,CanSpawnAnywhere = true },


            new Agency("~s~", "UNK", "Unknown Agency", "White", Classification.Other, null, null, "",null) { MaxWantedLevelSpawn = 0 },

        };
      
    }
    private void CustomConfig()
    {
        //Custom Vehicles

        //Bravado Buffao S
        //https://www.gta5-mods.com/vehicles/lspd-14-bravado-buffalo-s-add-on
        //police2b - No Liveries

        //Del Pierro Pack
        //https://www.gta5-mods.com/vehicles/del-perro-police-dept-vehicle-pack-add-on
        //• Del Perro Declasse Yosemite (dppolice),
        //• Del Perro Police Cruiser(dppolice2),
        // My Liveries 0,1 DP -- 2,3 LSPD VW -- 4,5 LSPDCH -- 6,7 LSPDELS 8,9 BCSO 10 LSSDVW 11 LSSD CH - 12 SAHP 13
        //• Del Perro Police Cruiser Utility(dppolice3),
        //• Del Perro Declasse Alamo(dppolice4).

        //RockfordHills Pack
        //https://www.gta5-mods.com/vehicles/rockford-hills-police-department-vehicle-pack-add-on
        //• Vapid Stanier (rhpolice),
        //• Declasse Alamo(rhpolice2),
        //• Cheval Fugitive(rhpolice3).

        //Vapid Scout
        //https://www.gta5-mods.com/vehicles/lspd-vapid-police-cruiser-utility-scout-add-on-custom-soundbank
        //pscout - No Liveries
        //0,1 LSPD -- LSPD-VW 2,3 -- LSPD-CH 4,5 -- LSPD-ELS 6,7 -- RHPH 8,9 BCSO 10,11 --SAHP 12,13

        //Peds
        List<PedestrianInformation> StandardCops = new List<PedestrianInformation>() {
            new PedestrianInformation("s_m_y_cop_01",85,85),
            new PedestrianInformation("s_f_y_cop_01",15,15) };
        List<PedestrianInformation> ExtendedStandardCops = new List<PedestrianInformation>() {
            new PedestrianInformation("s_m_y_cop_01",85,85),
            new PedestrianInformation("s_f_y_cop_01",10,10),
            new PedestrianInformation("ig_trafficwarden",5,5) };
        List<PedestrianInformation> ParkRangers = new List<PedestrianInformation>() {
            new PedestrianInformation("s_m_y_ranger_01",75,75),
            new PedestrianInformation("s_f_y_ranger_01",25,25) };
        List<PedestrianInformation> SheriffPeds = new List<PedestrianInformation>() {
            new PedestrianInformation("s_m_y_sheriff_01",75,75),
            new PedestrianInformation("s_f_y_sheriff_01",25,25) };
        List<PedestrianInformation> SWAT = new List<PedestrianInformation>() {
            new PedestrianInformation("s_m_y_swat_01", 100,100) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(10, 0, 0,0) },new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) }) } };
        List<PedestrianInformation> PoliceAndSwat = new List<PedestrianInformation>() {
            new PedestrianInformation("s_m_y_cop_01",70,0),
            new PedestrianInformation("s_f_y_cop_01",30,0),
            new PedestrianInformation("s_m_y_swat_01", 0,100) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(10, 0, 0,0) },new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) }) } };
        List<PedestrianInformation> SheriffAndSwat = new List<PedestrianInformation>() {
            new PedestrianInformation("s_m_y_sheriff_01", 75, 0),
            new PedestrianInformation("s_f_y_sheriff_01", 25, 0),
            new PedestrianInformation("s_m_y_swat_01", 0, 100) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(10, 0, 0,0) },new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) }) } };
        List<PedestrianInformation> DOAPeds = new List<PedestrianInformation>() {
            new PedestrianInformation("u_m_m_doa_01",100,100) };
        List<PedestrianInformation> IAAPeds = new List<PedestrianInformation>() {
            new PedestrianInformation("s_m_m_fibsec_01",100,100) };
        List<PedestrianInformation> SAHPPeds = new List<PedestrianInformation>() {
            new PedestrianInformation("s_m_y_hwaycop_01",100,100) };
        List<PedestrianInformation> MilitaryPeds = new List<PedestrianInformation>() {
            new PedestrianInformation("s_m_y_armymech_01",25,0),
            new PedestrianInformation("s_m_m_marine_01",50,0),
            new PedestrianInformation("s_m_m_marine_02",0,0),
            new PedestrianInformation("s_m_y_marine_01",25,0),
            new PedestrianInformation("s_m_y_marine_02",0,0),
            new PedestrianInformation("s_m_y_marine_03",100,100) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(2, 1, 0, 0),new PedComponent(8, 0, 0, 0) },new List<PedPropComponent>() { new PedPropComponent(3, 1, 0) }) },
            new PedestrianInformation("s_m_m_pilot_02",0,0),
            new PedestrianInformation("s_m_y_pilot_01",0,0) };
        List<PedestrianInformation> FIBPeds = new List<PedestrianInformation>() {
            new PedestrianInformation("s_m_m_fibsec_01",55,70){MaxWantedLevelSpawn = 3 },
            new PedestrianInformation("s_m_m_fiboffice_01",15,0){MaxWantedLevelSpawn = 3 },
            new PedestrianInformation("s_m_m_fiboffice_02",15,0){MaxWantedLevelSpawn = 3 },
            new PedestrianInformation("u_m_m_fibarchitect",10,0) {MaxWantedLevelSpawn = 3 },
            new PedestrianInformation("s_m_y_swat_01", 5,30) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 4, RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(10, 0, 1,0) },new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) }) } };
        List<PedestrianInformation> PrisonPeds = new List<PedestrianInformation>() {
            new PedestrianInformation("s_m_m_prisguard_01",100,100) };
        List<PedestrianInformation> SecurityPeds = new List<PedestrianInformation>() {
            new PedestrianInformation("s_m_m_security_01",100,100) };
        List<PedestrianInformation> CoastGuardPeds = new List<PedestrianInformation>() {
            new PedestrianInformation("s_m_y_uscg_01",100,100) };
        List<PedestrianInformation> NOOSEPeds = new List<PedestrianInformation>() {
            new PedestrianInformation("s_m_y_swat_01", 100,100) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(10, 0, 0,0) },new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) }) } };

        //Vehicles
        List<VehicleInformation> UnmarkedVehicles = new List<VehicleInformation>() {
            new VehicleInformation("police4", 100, 100) };
        List<VehicleInformation> CoastGuardVehicles = new List<VehicleInformation>() {
            new VehicleInformation("predator", 75, 50),
            new VehicleInformation("dinghy", 0, 25),
            new VehicleInformation("seashark2", 25, 25) { MaxOccupants = 1 },};
        List<VehicleInformation> SecurityVehicles = new List<VehicleInformation>() {
            new VehicleInformation("dilettante2", 100, 100) {MaxOccupants = 1 } };
        List<VehicleInformation> ParkRangerVehicles = new List<VehicleInformation>() {
            new VehicleInformation("pranger", 100, 100) };
        List<VehicleInformation> FIBVehicles = new List<VehicleInformation>() {
            new VehicleInformation("fbi", 70, 70){ MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new VehicleInformation("fbi2", 30, 30) { MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new VehicleInformation("fbi2", 0, 30) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 4, AllowedPedModels = new List<string>() { "s_m_y_swat_01" },MinOccupants = 4, MaxOccupants = 6 },
        };
        List<VehicleInformation> NOOSEVehicles = new List<VehicleInformation>() {
            new VehicleInformation("fbi", 70, 70){ MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new VehicleInformation("fbi2", 30, 30) { MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new VehicleInformation("fbi2", 0, 30) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 4, AllowedPedModels = new List<string>() { "s_m_y_swat_01" },MinOccupants = 4, MaxOccupants = 6 },
            new VehicleInformation("riot", 0, 70) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 4, AllowedPedModels = new List<string>() { "s_m_y_swat_01" },MinOccupants = 2, MaxOccupants = 3 },
            new VehicleInformation("annihilator", 0, 100) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 4, AllowedPedModels = new List<string>() { "s_m_y_swat_01" },MinOccupants = 3,MaxOccupants = 4 }};
        List<VehicleInformation> HighwayPatrolVehicles = new List<VehicleInformation>() {
            new VehicleInformation("policeb", 30, 30) { MaxOccupants = 1 },
            new VehicleInformation("pscout", 50,50) { Liveries = new List<int>() { 12, 13 } },
            new VehicleInformation("dppolice2", 50, 50){ Liveries = new List<int>() { 13 } } };
        List<VehicleInformation> PrisonVehicles = new List<VehicleInformation>() {
            new VehicleInformation("policet", 70, 70),
            new VehicleInformation("police4", 30, 30) };
        List<VehicleInformation> LSPDVehiclesVanilla = new List<VehicleInformation>() {
            new VehicleInformation("police", 25,25) { Liveries = new List<int>() { 0,1,2,3,4,5 } },
            new VehicleInformation("police3", 25, 20) { Liveries = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 } },
            new VehicleInformation("police4", 1,1),
            new VehicleInformation("police2b", 35,35),
            new VehicleInformation("pscout", 50,50) { Liveries = new List<int>() { 0, 1 } },
            new VehicleInformation("fbi2", 1,1),
            new VehicleInformation("policet", 0, 25) { MinWantedLevelSpawn = 3} };
        List<VehicleInformation> LSSDVehiclesVanilla = new List<VehicleInformation>() {
            new VehicleInformation("sheriff", 50, 50){ Liveries = new List<int> { 0, 1, 2, 3 } },
            new VehicleInformation("sheriff2", 50, 50) { Liveries = new List<int> { 0, 1, 2, 3 } },
            new VehicleInformation("dppolice2", 100,100) { Liveries = new List<int>() { 10 } },};
        List<VehicleInformation> LSPDVehicles = LSPDVehiclesVanilla;
        List<VehicleInformation> SAHPVehicles = HighwayPatrolVehicles;
        List<VehicleInformation> LSSDVehicles = LSSDVehiclesVanilla;
        List<VehicleInformation> BCSOVehicles = new List<VehicleInformation>() {
            new VehicleInformation("dppolice2", 50,50) { Liveries = new List<int>() { 8,9 } },
            new VehicleInformation("pscout", 100, 100){ Liveries = new List<int> { 10,11 } },
        };
        List<VehicleInformation> VWHillsLSSDVehicles = new List<VehicleInformation>() {
            new VehicleInformation("dppolice2", 100,100) { Liveries = new List<int>() { 12 } }, };
        List<VehicleInformation> ChumashLSSDVehicles = new List<VehicleInformation>() {
            new VehicleInformation("dppolice2", 100,100) { Liveries = new List<int>() { 11 } }, };
        List<VehicleInformation> LSSDDavisVehicles = new List<VehicleInformation>() {
            new VehicleInformation("sheriff", 100, 100){ Liveries = new List<int> { 0, 1, 2, 3 } } };
        List<VehicleInformation> RHPDVehicles = new List<VehicleInformation>() {
            new VehicleInformation("rhpolice", 33, 33),
            new VehicleInformation("rhpolice2", 33, 33),
            new VehicleInformation("rhpolice3", 33, 33) };
        List<VehicleInformation> DPPDVehicles = new List<VehicleInformation>() {

           // My Liveries 0,1 DP -- 2,3 LSPD VW -- (4,5 LSPDCH) -- (6,7 LSPDELS) (8,9 BCSO) (10  LSSD)(11 LSSDVW) (12 LSSD CH)
            new VehicleInformation("dppolice", 25, 25),
            new VehicleInformation("dppolice2", 25, 25) { Liveries = new List<int>() { 0,1 } },
            new VehicleInformation("dppolice3", 25, 25),
            new VehicleInformation("dppolice4", 25, 25) };
        List<VehicleInformation> ChumashLSPDVehicles = new List<VehicleInformation>() {
            new VehicleInformation("dppolice2", 50,50) { Liveries = new List<int>() { 4, 5 } },
          
            new VehicleInformation("pscout", 50,50) { Liveries = new List<int>() { 4, 5 } },
            new VehicleInformation("policet", 0, 25) { MinWantedLevelSpawn = 3} };
        List<VehicleInformation> EastLSPDVehicles = new List<VehicleInformation>() {
            new VehicleInformation("dppolice2", 50,50) { Liveries = new List<int>() { 6,7 } },
            new VehicleInformation("pscout", 50,50) { Liveries = new List<int>() { 6, 7 } } };
        List<VehicleInformation> VWPDVehicles = new List<VehicleInformation>() {
            new VehicleInformation("dppolice2", 100,75){ Liveries = new List<int>() { 2,3 } },
            new VehicleInformation("pscout", 50,50) { Liveries = new List<int>() { 2, 3 } } };
        List<VehicleInformation> PoliceHeliVehicles = new List<VehicleInformation>() {
            new VehicleInformation("polmav", 0,100) { Liveries = new List<int>() { 0 }, MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 3 } };
        List<VehicleInformation> SheriffHeliVehicles = new List<VehicleInformation>() {
            new VehicleInformation("buzzard2", 0,25) { Liveries = new List<int>() { 0 }, MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 3 },
            new VehicleInformation("polmav", 0,75) { Liveries = new List<int>() { 0 }, MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 3 } };
        List<VehicleInformation> ArmyVehicles = new List<VehicleInformation>() {
            new VehicleInformation("crusader", 75,50) { Liveries = new List<int>() { 0 },MinOccupants = 1,MaxOccupants = 2},
            new VehicleInformation("barracks", 25,50) { Liveries = new List<int>() { 0 },MinOccupants = 3,MaxOccupants = 5,MinWantedLevelSpawn = 4 },
            new VehicleInformation("rhino", 0,10) { Liveries = new List<int>() { 0 },MinOccupants = 1,MaxOccupants = 2,MinWantedLevelSpawn = 5 },
            new VehicleInformation("valkyrie", 0,50) { Liveries = new List<int>() { 0 },MinOccupants = 3,MaxOccupants = 3,MinWantedLevelSpawn = 4 },
            new VehicleInformation("valkyrie2", 0,50) { Liveries = new List<int>() { 0 },MinOccupants = 3,MaxOccupants = 3,MinWantedLevelSpawn = 4 },
        };

        //Weapon
        List<IssuedWeapon> AllWeapons = new List<IssuedWeapon>()
        {
            // Pistols
            new IssuedWeapon("weapon_pistol", true, new WeaponVariation()),
            new IssuedWeapon("weapon_pistol", true, new WeaponVariation(0, new List<string> { "Flashlight" })),
            new IssuedWeapon("weapon_pistol", true, new WeaponVariation(0,new List<string> { "Extended Clip" })),
            new IssuedWeapon("weapon_pistol", true, new WeaponVariation(0,new List<string> { "Flashlight","Extended Clip" })),

            new IssuedWeapon("weapon_pistol_mk2", true ,new WeaponVariation()),
            new IssuedWeapon("weapon_pistol_mk2", true, new WeaponVariation(0,new List<string> { "Flashlight" })),
            new IssuedWeapon("weapon_pistol_mk2", true, new WeaponVariation(0,new List<string> { "Extended Clip" })),
            new IssuedWeapon("weapon_pistol_mk2", true, new WeaponVariation(0, new List<string> { "Flashlight","Extended Clip" })),

            new IssuedWeapon("weapon_combatpistol", true, new WeaponVariation()),
            new IssuedWeapon("weapon_combatpistol", true, new WeaponVariation(0, new List<string> { "Flashlight" })),
            new IssuedWeapon("weapon_combatpistol", true, new WeaponVariation(0,new List<string> { "Extended Clip" })),
            new IssuedWeapon("weapon_combatpistol", true, new WeaponVariation(0, new List<string> { "Flashlight","Extended Clip" })),

            new IssuedWeapon("weapon_heavypistol", true, new WeaponVariation()),
            new IssuedWeapon("weapon_heavypistol", true, new WeaponVariation(0,new List<string> { "Etched Wood Grip Finish" })),
            new IssuedWeapon("weapon_heavypistol", true, new WeaponVariation(0,new List<string> { "Flashlight","Extended Clip" })),
            new IssuedWeapon("weapon_heavypistol", true, new WeaponVariation(0,new List<string> { "Extended Clip" })),

            // Shotguns
            new IssuedWeapon("weapon_pumpshotgun", false, new WeaponVariation()),
            new IssuedWeapon("weapon_pumpshotgun", false, new WeaponVariation(0,new List<string> { "Flashlight" })),

            new IssuedWeapon("weapon_pumpshotgun_mk2", false, new WeaponVariation()),
            new IssuedWeapon("weapon_pumpshotgun_mk2", false, new WeaponVariation(0, new List<string> { "Flashlight" })),
            new IssuedWeapon("weapon_pumpshotgun_mk2", false, new WeaponVariation(0, new List<string> { "Holographic Sight" })),
            new IssuedWeapon("weapon_pumpshotgun_mk2", false, new WeaponVariation(0, new List<string> { "Flashlight","Holographic Sight" })),

            // ARs
            new IssuedWeapon("weapon_carbinerifle", false, new WeaponVariation()),
            new IssuedWeapon("weapon_carbinerifle", false, new WeaponVariation(0,new List<string> { "Grip","Flashlight" })),
            new IssuedWeapon("weapon_carbinerifle", false, new WeaponVariation(0, new List<string> { "Scope", "Grip","Flashlight" })),
            new IssuedWeapon("weapon_carbinerifle", false, new WeaponVariation(0,new List<string> { "Scope", "Grip","Flashlight","Extended Clip" })),

            new IssuedWeapon("weapon_carbinerifle_mk2", false, new WeaponVariation()),
            new IssuedWeapon("weapon_carbinerifle_mk2", false, new WeaponVariation(0, new List<string> { "Holographic Sight","Grip","Flashlight" })),
            new IssuedWeapon("weapon_carbinerifle_mk2", false, new WeaponVariation(0, new List<string> { "Holographic Sight", "Grip","Extended Clip" })),
            new IssuedWeapon("weapon_carbinerifle_mk2", false, new WeaponVariation(0, new List<string> { "Large Scope", "Grip","Flashlight","Extended Clip" })),
        };
        List<IssuedWeapon> BestWeapons = new List<IssuedWeapon>()
        {
            new IssuedWeapon("weapon_pistol_mk2", true, new WeaponVariation(0,new List<string> { "Flashlight" })),
            new IssuedWeapon("weapon_pistol_mk2", true, new WeaponVariation(0,new List<string> { "Extended Clip" })),
            new IssuedWeapon("weapon_pistol_mk2", true, new WeaponVariation(0, new List<string> { "Flashlight","Extended Clip" })),
            new IssuedWeapon("weapon_carbinerifle_mk2", false, new WeaponVariation(0, new List<string> { "Holographic Sight","Grip","Flashlight" })),
            new IssuedWeapon("weapon_carbinerifle_mk2", false, new WeaponVariation(0, new List<string> { "Holographic Sight", "Grip","Extended Clip" })),
            new IssuedWeapon("weapon_carbinerifle_mk2", false, new WeaponVariation(0, new List<string> { "Large Scope", "Grip","Flashlight","Extended Clip" })),
        };
        List<IssuedWeapon> HeliWeapons = new List<IssuedWeapon>()
        {
            new IssuedWeapon("weapon_pistol_mk2", true, new WeaponVariation(0,new List<string> { "Flashlight" })),
            new IssuedWeapon("weapon_pistol_mk2", true, new WeaponVariation(0,new List<string> { "Extended Clip" })),
            new IssuedWeapon("weapon_pistol_mk2", true, new WeaponVariation(0, new List<string> { "Flashlight","Extended Clip" })),
            new IssuedWeapon("weapon_marksmanrifle_mk2", false, new WeaponVariation(0, new List<string> { "Large Scope", "Suppressor", "Tracer Rounds" })),
            new IssuedWeapon("weapon_marksmanrifle_mk2", false, new WeaponVariation(0, new List<string> { "Large Scope","Tracer Rounds" })),
        };
        List<IssuedWeapon> LimitedWeapons = new List<IssuedWeapon>()
        {
            new IssuedWeapon("weapon_heavypistol", true, new WeaponVariation()),
            new IssuedWeapon("weapon_revolver", true, new WeaponVariation()),
            new IssuedWeapon("weapon_heavypistol", true, new WeaponVariation(0,new List<string> { "Flashlight" })),
            new IssuedWeapon("weapon_pumpshotgun", false, new WeaponVariation()),
            new IssuedWeapon("weapon_pumpshotgun", false, new WeaponVariation(0,new List<string> { "Flashlight" })),

        };

        AgenciesList = new List<Agency>
        {
            new Agency("~b~", "LSPD", "Los Santos Police Department", "Blue", Classification.Police, StandardCops, LSPDVehicles, "LS ",AllWeapons) { MaxWantedLevelSpawn = 3 },
            new Agency("~b~", "LSPD-VW", "Los Santos Police - Vinewood Division", "Blue", Classification.Police, ExtendedStandardCops, VWPDVehicles, "LSV ",LimitedWeapons) { MaxWantedLevelSpawn = 3 },
            new Agency("~b~", "LSPD-ELS", "Los Santos Police - East Los Santos Division", "Blue", Classification.Police, ExtendedStandardCops, EastLSPDVehicles, "LSE ",LimitedWeapons) { MaxWantedLevelSpawn = 3 },
            new Agency("~b~", "DPPD", "Del Pierro Police Department", "Blue", Classification.Police, StandardCops, DPPDVehicles, "VP ",AllWeapons) { MaxWantedLevelSpawn = 3 },
            new Agency("~b~", "RHPD", "Rockford Hills Police Department", "Blue", Classification.Police, StandardCops, RHPDVehicles, "RH ",AllWeapons) { MaxWantedLevelSpawn = 3 },

            new Agency("~r~", "LSSD", "Los Santos County Sheriff", "Red", Classification.Sheriff, SheriffPeds, LSSDVehicles, "LSCS ",LimitedWeapons) { MaxWantedLevelSpawn = 3 },
            new Agency("~r~", "LSSD-VW", "Los Santos Sheriff - Vinewood Division", "Red", Classification.Sheriff, SheriffPeds, VWHillsLSSDVehicles, "LSCS ",LimitedWeapons) { MaxWantedLevelSpawn = 3 },
            new Agency("~r~", "LSSD-CH", "Los Santos Sheriff - Chumash Division", "Red", Classification.Sheriff, SheriffPeds, ChumashLSSDVehicles, "LSCS ",LimitedWeapons) { MaxWantedLevelSpawn = 3 },
            new Agency("~r~", "BCSO", "Blaine County Sheriff", "Red", Classification.Sheriff, SheriffPeds, BCSOVehicles, "BCS ",LimitedWeapons) { MaxWantedLevelSpawn = 3 },

            new Agency("~b~", "LSPD-ASD", "Los Santos Police Department - Air Support Division", "Blue", Classification.Police, PoliceAndSwat, PoliceHeliVehicles, "ASD ",HeliWeapons) { MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4, SpawnLimit = 3 },
            new Agency("~r~", "LSSD-ASD", "Los Santos Sheriffs Department - Air Support Division", "Red", Classification.Sheriff, SheriffAndSwat, SheriffHeliVehicles, "ASD ",HeliWeapons) { MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4, SpawnLimit = 3 },

            new Agency("~r~", "NOOSE", "National Office of Security Enforcement", "DarkSlateGray", Classification.Federal, NOOSEPeds, NOOSEVehicles, "",BestWeapons) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 4,CanSpawnAnywhere = true},
            new Agency("~p~", "FIB", "Federal Investigation Bureau", "Purple", Classification.Federal, FIBPeds, FIBVehicles, "FIB ",BestWeapons) {MaxWantedLevelSpawn = 4, SpawnLimit = 6,CanSpawnAnywhere = true },
            new Agency("~p~", "DOA", "Drug Observation Agency", "Purple", Classification.Federal, DOAPeds, UnmarkedVehicles, "DOA ",AllWeapons)  {MaxWantedLevelSpawn = 3, SpawnLimit = 4,CanSpawnAnywhere = true },

            new Agency("~y~", "SAHP", "San Andreas Highway Patrol", "Yellow", Classification.State, SAHPPeds, SAHPVehicles, "HP ",LimitedWeapons) { MaxWantedLevelSpawn = 3, SpawnsOnHighway = true },
            new Agency("~o~", "SASPA", "San Andreas State Prison Authority", "Orange", Classification.State, PrisonPeds, PrisonVehicles, "SASPA ",AllWeapons) { MaxWantedLevelSpawn = 3, SpawnLimit = 2 },
            new Agency("~g~", "SAPR", "San Andreas Park Ranger", "Green", Classification.State, ParkRangers, ParkRangerVehicles, "",AllWeapons) { MaxWantedLevelSpawn = 3, SpawnLimit = 3 },
            new Agency("~o~", "SACG", "San Andreas Coast Guard", "DarkOrange", Classification.State, CoastGuardPeds, CoastGuardVehicles, "SACG ",LimitedWeapons){ MaxWantedLevelSpawn = 3,SpawnLimit = 3 },

            new Agency("~p~", "LSPA", "Port Authority of Los Santos", "LightGray", Classification.Police, SecurityPeds, UnmarkedVehicles, "LSPA ",LimitedWeapons) {MaxWantedLevelSpawn = 3, SpawnLimit = 3 },
            new Agency("~p~", "LSIAPD", "Los Santos International Airport Police Department", "LightBlue", Classification.Police, StandardCops, LSPDVehicles, "LSA ",AllWeapons) { MaxWantedLevelSpawn = 3, SpawnLimit = 3 },

            new Agency("~o~", "PRISEC", "Private Security", "White", Classification.Security, SecurityPeds, SecurityVehicles, "",LimitedWeapons) {MaxWantedLevelSpawn = 1, SpawnLimit = 1 },

            new Agency("~u~", "ARMY", "Army", "Black", Classification.Military, MilitaryPeds, ArmyVehicles, "",BestWeapons) { MinWantedLevelSpawn = 5,CanSpawnAnywhere = true },


            new Agency("~s~", "UNK", "Unknown Agency", "White", Classification.Other, null, null, "",null) { MaxWantedLevelSpawn = 0 },

        };

    }

}








