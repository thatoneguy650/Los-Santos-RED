using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Agencies : IAgencies
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Agencies.xml";
    private List<Agency> AgenciesList;
    private Agency DefaultAgency;
    private Agency LSPD;
    private Agency LSPDVW;
    private Agency LSPDELS;
    private Agency LSPDDP;
    private Agency LSPDRH;
    private Agency LSPDASD;
    private Agency LSSD;
    private Agency LSSDVW;
    private Agency LSSDDV;
    private Agency LSSDBC;
    private Agency LSSDMJ;
    private Agency LSSDASD;
    private Agency LSPP;
    private Agency LSIAPD;
    private Agency NOOSE;
    private Agency FIB;
    private Agency DOA;
    private Agency SAHP;
    private Agency SASPA;
    private Agency SAPR;
    private Agency SACG;
    private Agency ARMY;
    private Agency LSFDFire;
    private Agency LSFD;
    private Agency NYSP;
    private Agency GRPSECHS;
    private Agency SECURO;
    private Agency MERRY;
    private Agency BOBCAT;
    private Agency UNK;
    private Agency LSMC;
    private Agency MRH;
    private Agency LCPD;
    public Agencies()
    {

    }
    public void ReadConfig()
    {
        DirectoryInfo taskDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo taskFile = taskDirectory.GetFiles("Agencies*.xml").OrderByDescending(x=> x.Name).FirstOrDefault();
        if(taskFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Agencies Config: {taskFile.FullName}",0);
            AgenciesList = Serialization.DeserializeParams<Agency>(taskFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Agencies Config  {ConfigFileName}", 0);
            AgenciesList = Serialization.DeserializeParams<Agency>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Agencies config found, creating default", 0);
            SetupDefault();
            DefaultConfig_Simple();
            DefaultConfig_FullExpanded();
            //DefaultConfig_LosSantos2008();
            DefaultConfig_LibertyCity();
            DefaultConfig();
        }
    }
    public Agency GetAgency(string AgencyInitials)
    {
        return AgenciesList.Where(x => x.ID.ToLower() == AgencyInitials.ToLower()).FirstOrDefault();
    }
    public Agency GetRandomMilitaryAgency()
    {
        return AgenciesList.Where(x => x.Classification == Classification.Military).PickRandom();
    }
    public Agency GetRandomAgency(ResponseType responseType)
    {
        return AgenciesList.Where(x => x.ResponseType == responseType).PickRandom();
    }
    public List<Agency> GetAgencies(Ped ped)
    {
        return AgenciesList.Where(x => x.Personnel != null && x.Personnel.Any(b => b.ModelName.ToLower() == ped.Model.Name.ToLower())).ToList();
    }
    public List<Agency> GetAgencies(Vehicle vehicle)
    {
        return AgenciesList.Where(x => x.Vehicles != null && x.Vehicles.Any(b => b.ModelName.ToLower() == vehicle.Model.Name.ToLower())).ToList();
    }
    public List<Agency> GetSpawnableAgencies(int WantedLevel)
    {
        return AgenciesList.Where(x => x.CanSpawnAnywhere && x.CanSpawn(WantedLevel)).ToList();
    }
    public List<Agency> GetSpawnableHighwayAgencies(int WantedLevel)
    {
        return AgenciesList.Where(x => x.SpawnsOnHighway && x.CanSpawn(WantedLevel)).ToList();
    }
    public List<Agency> GetSpawnableAgencies(int WantedLevel, ResponseType responseType)
    {
        return AgenciesList.Where(x => x.CanSpawnAnywhere && x.CanSpawn(WantedLevel) && x.ResponseType == responseType).ToList();
    }
    public List<Agency> GetSpawnableHighwayAgencies(int WantedLevel, ResponseType responseType)
    {
        return AgenciesList.Where(x => x.SpawnsOnHighway && x.CanSpawn(WantedLevel) && x.ResponseType == responseType).ToList();
    }
    public List<Agency> GetAgencies()
    {
        return AgenciesList;
    }
    public List<Agency> GetAgenciesByResponse(ResponseType responseType)
    {
        return AgenciesList.Where(x => x.ResponseType == responseType).ToList();
    }

    private void SetupDefault()
    {
        LSPD = new Agency("~b~", "LSPD", "LSPD", "Los Santos Police Department", "Blue", Classification.Police, "StandardCops", "LSPDVehicles", "LS ", "Tasers", "AllSidearms", "AllLongGuns", "LSPD Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 1 };
        LSPDVW = new Agency("~b~", "LSPD-VW", "LSPD-VW", "Los Santos Police - Vinewood Division", "Blue", Classification.Police, "StandardCops", "VWPDVehicles", "LSV ", "Tasers", "LimitedSidearms", "LimitedLongGuns", "LSPD Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 2 };
        LSPDELS = new Agency("~b~", "LSPD-ELS", "LSPD-ELS", "Los Santos Police - East Los Santos Division", "Blue", Classification.Police, "StandardCops", "EastLSPDVehicles", "LSE ", "Tasers", "LimitedSidearms", "LimitedLongGuns", "LSPD Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 3 };
        LSPDDP = new Agency("~b~", "LSPD-DP", "LSPD-DP", "Los Santos Police - Del Perro Division", "Blue", Classification.Police, "StandardCops", "DPPDVehicles", "VP ", "Tasers", "AllSidearms", "AllLongGuns", "LSPD Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 4 };
        LSPDRH = new Agency("~b~", "LSPD-RH", "LSPD-RH", "Los Santos Police - Rockford Hills Division", "Blue", Classification.Police, "StandardCops", "RHPDVehicles", "RH ", "Tasers", "AllSidearms", "AllLongGuns", "LSPD Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 5 };
        LSPDASD = new Agency("~b~", "LSPD-ASD", "LSPD-ASD", "Los Santos Police Department - Air Support Division", "Blue", Classification.Police, "StandardCops", "PoliceHeliVehicles", "ASD ", "Tasers", "HeliSidearms", "HeliLongGuns", "LSPD Officer") { MinWantedLevelSpawn = 3, MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 6 };
        LSSD = new Agency("~r~", "LSSD", "LSSD", "Los Santos County Sheriff", "Red", Classification.Sheriff, "SheriffPeds", "LSSDVehicles", "LSCS ", "Tasers", "LimitedSidearms", "LimitedLongGuns", "LSSD Deputy") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 7 };
        LSSDVW = new Agency("~r~", "LSSD-VW", "LSSD-VW", "Los Santos Sheriff - Vinewood Division", "Red", Classification.Sheriff, "SheriffPeds", "VWHillsLSSDVehicles", "LSCS ", "Tasers", "LimitedSidearms", "LimitedLongGuns", "LSSD Deputy") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 8 };
        LSSDDV = new Agency("~r~", "LSSD-DV", "LSSD-DV", "Los Santos Sheriff - Davis Division", "Red", Classification.Sheriff, "SheriffPeds", "DavisLSSDVehicles", "LSCS ", "Tasers", "LimitedSidearms", "LimitedLongGuns", "LSSD Deputy") { MaxWantedLevelSpawn = 4, HeadDataGroupID = "AllHeads", Division = 9 };
        LSSDBC = new Agency("~r~", "LSSD-BC", "LSSD-BC", "Los Santos Sheriff - Blaine County Division", "Red", Classification.Sheriff, "SheriffPeds", "BCSOVehicles", "BCS ", "Nightsticks", "LimitedSidearms", "LimitedLongGuns", "LSSD Deputy") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 10 };
        LSSDMJ = new Agency("~r~", "LSSD-MJ", "LSSD-MJ", "Los Santos Sheriff - Majestic County Division", "Red", Classification.Sheriff, "SheriffPeds", "MajesticLSSDVehicles", "MCS ", "Tasers", "LimitedSidearms", "LimitedLongGuns", "LSSD Deputy") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 11 };
        LSSDASD = new Agency("~r~", "LSSD-ASD", "LSSD-ASD", "Los Santos Sheriffs Department - Air Support Division", "Red", Classification.Sheriff, "SheriffPeds", "SheriffHeliVehicles", "ASD ", "Tasers", "HeliSidearms", "HeliLongGuns", "LSSD Deputy") { MinWantedLevelSpawn = 3, MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 13 };     
        LSPP = new Agency("~p~", "LSPP", "LSPP", "Los Santos Port Police", "LightGray", Classification.Police, "SecurityPeds", "UnmarkedVehicles", "LSPP ", "Tasers", "LimitedSidearms", "LimitedLongGuns", "Port Authority Officer") { MaxWantedLevelSpawn = 3, SpawnLimit = 3, HeadDataGroupID = "AllHeads", Division = 15 };
        LSIAPD = new Agency("~p~", "LSIAPD", "LSIAPD", "Los Santos International Airport Police Department", "LightBlue", Classification.Police, "StandardCops", "LSPDVehicles", "LSA ", "Tasers", "AllSidearms", "AllLongGuns", "LSIAPD Officer") { MaxWantedLevelSpawn = 3, SpawnLimit = 3, HeadDataGroupID = "AllHeads", Division = 16 };
        NOOSE = new Agency("~r~", "NOOSE", "NOOSE", "National Office of Security Enforcement", "DarkSlateGray", Classification.Federal, "NOOSEPeds", "NOOSEVehicles", "", "Tasers", "BestSidearms", "BestLongGuns", "NOOSE Officer") { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads" };
        FIB = new Agency("~p~", "FIB", "FIB", "Federal Investigation Bureau", "Purple", Classification.Federal, "FIBPeds", "FIBVehicles", "FIB ", "Tasers", "BestSidearms", "BestLongGuns", "FIB Agent") { MaxWantedLevelSpawn = 5, SpawnLimit = 6, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads" };
        DOA = new Agency("~p~", "DOA", "DOA", "Drug Observation Agency", "Purple", Classification.Federal, "DOAPeds", "UnmarkedVehicles", "DOA ", "Tasers", "AllSidearms", "AllLongGuns", "DOA Agent") { MaxWantedLevelSpawn = 3, SpawnLimit = 4, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads" };    
        SAHP = new Agency("~y~", "SAHP", "SAHP", "San Andreas Highway Patrol", "Yellow", Classification.State, "SAHPPeds", "SAHPVehicles", "HP ", "Tasers", "LimitedSidearms", "LimitedLongGuns", "SAHP Officer") { MaxWantedLevelSpawn = 3, SpawnsOnHighway = true, HeadDataGroupID = "AllHeads" };
        SASPA = new Agency("~o~", "SASPA", "SASPA", "San Andreas State Prison Authority", "Orange", Classification.State, "PrisonPeds", "PrisonVehicles", "SASPA ", "Tasers", "AllSidearms", "AllLongGuns", "SASPA Officer") { MaxWantedLevelSpawn = 3, SpawnLimit = 2, HeadDataGroupID = "AllHeads" };
        SAPR = new Agency("~g~", "SAPR", "SAPR", "San Andreas Park Ranger", "Green", Classification.State, "ParkRangers", "ParkRangerVehicles", "", "Tasers", "AllSidearms", "AllLongGuns", "SA Park Ranger") { MaxWantedLevelSpawn = 3, SpawnLimit = 3, HeadDataGroupID = "AllHeads" };
        SACG = new Agency("~o~", "SACG", "SACG", "San Andreas Coast Guard", "DarkOrange", Classification.State, "CoastGuardPeds", "CoastGuardVehicles", "SACG ", "Tasers", "LimitedSidearms", "LimitedLongGuns", "Coast Guard Officer") { MaxWantedLevelSpawn = 3, SpawnLimit = 3, HeadDataGroupID = "AllHeads" };
        ARMY = new Agency("~u~", "ARMY", "ARMY", "Army", "Black", Classification.Military, "MilitaryPeds", "ArmyVehicles", "", "Tasers", "MilitarySidearms", "MilitaryLongGuns", "Soldier") { MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads" };
        LSFDFire = new Agency("~r~", "LSFD", "LSFD", "Los Santos Fire Department", "Red", Classification.Fire, "Firefighters", "Firetrucks", "LSFD ", null, null, null, "LSFD Firefighter") { MaxWantedLevelSpawn = 0, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads" };
        LSMC = new Agency("~w~", "LSMC", "LSMC", "Los Santos Medical Center", "White", Classification.EMS, "GreenEMTs", "Amublance1", "LSMC ", null, null, null, "LSMC EMT") { MaxWantedLevelSpawn = 0, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads" };
        MRH = new Agency("~w~", "MRH", "MRH", "Mission Row Hospital", "White", Classification.EMS, "BlueEMTs", "Amublance2", "MRH ", null, null, null, "MRH Officer") { MaxWantedLevelSpawn = 0, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads" };
        LSFD = new Agency("~w~", "LSFD", "LSFD", "Los Santos Fire Department", "White", Classification.EMS, "GreenEMTs", "Amublance3", "LSFD ", null, null, null, "LSFD EMT") { MaxWantedLevelSpawn = 0, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads" };
        NYSP = new Agency("~b~", "NYSP", "NYSP", "North Yankton State Patrol", "Blue", Classification.Police, "NYSPPeds", "NYSPVehicles", "NYSP ", "Nightsticks", "LimitedSidearms", "LimitedLongGuns", "NYSP Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads" };

        GRPSECHS = new Agency("~g~", "GRP6", "G6", "Gruppe Sechs", "Green",Classification.Security, "GruppeSechsPeds", "GroupSechsVehicles", "GS ","Tasers", "LimitedSidearms", null, "Gruppe Sechs Officer") { MaxWantedLevelSpawn = 2, HeadDataGroupID = "AllHeads" };  
        SECURO = new Agency("~b~", "SECURO", "SecuroServ", "SecuroServ", "Black", Classification.Security, "SecuroservPeds", "SecuroservVehicles", "SS ", "Tasers", "LimitedSidearms", null, "SecuroServ Officer") { MaxWantedLevelSpawn = 2, HeadDataGroupID = "AllHeads" };
        MERRY = new Agency("~w~", "MERRY", "Merryweather", "Merryweather Security", "White", Classification.Security, "MerryweatherSecurityPeds", "MerryweatherPatrolVehicles", "MW ", "Tasers", "LimitedSidearms", null, "Merryweather Officer") { MaxWantedLevelSpawn = 2, HeadDataGroupID = "AllHeads" };
        BOBCAT = new Agency("~w~", "BOBCAT", "Bobcat", "Bobcat Security", "White", Classification.Security, "BobcatPeds", "BobcatSecurityVehicles", "BC ", "Tasers", "LimitedSidearms", null, "Bobcat Officer") { MaxWantedLevelSpawn = 2, HeadDataGroupID = "AllHeads" };


        LCPD = new Agency("~b~", "LCPD", "LCPD", "Liberty City Police Department", "Blue", Classification.Police, "StandardCops", "LCPDVehicles", "LC ", "Tasers", "AllSidearms", "AllLongGuns", "LCPD Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 1 };



        UNK = new Agency("~s~", "UNK", "UNK", "Unknown Agency", "White", Classification.Other, null, null, "", null, null, null, "Officer") { MaxWantedLevelSpawn = 0 };

//#if DEBUG
//        SASPA.PersonnelID = "OtherPeds";
//#endif


    }

    private void DefaultConfig()
    {

        DefaultAgency = new Agency("~b~", "LSPD", "LSPD", "Los Santos Police Department", "Blue", Classification.Police, "StandardCops", "LSPDVehicles", "LS ", "Tasers", "AllSidearms", "AllLongGuns", "LSPD Officer");
        AgenciesList = new List<Agency>
        {
            LSPD,
            LSPDVW,
            LSPDELS,
            LSPDDP,
            LSPDRH,
            LSPDASD,
            LSSD,
            LSSDVW,
            LSSDDV,
            LSSDBC,
            LSSDMJ,
            LSSDASD,
            LSPP,
            LSIAPD,
            NOOSE,
            FIB,
            DOA,
            SAHP,
            SASPA,
            SAPR,
            SACG,
            ARMY,
            LSFDFire,
            LSMC,
            MRH,
            LSFD,
            UNK,
            NYSP,
            LCPD,
            GRPSECHS,
            SECURO,
            MERRY,
            BOBCAT,
        };

        Serialization.SerializeParams(AgenciesList, ConfigFileName);
    }
    private void DefaultConfig_FullExpanded()
    {
        
        Agency DPPD = new Agency("~b~", "DPPD", "DPPD", "Del Perro Police Department", "Blue", Classification.Police, "DPPDCops", "DPPDVehicles", "DP ", "Tasers", "AllSidearms", "AllLongGuns", "DPPD Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 4 };
        Agency RHPD = new Agency("~b~", "RHPD", "RHPD", "Rockford Hills Police Department", "Blue", Classification.Police, "RHPDCops", "RHPDVehicles", "RH ", "Tasers", "AllSidearms", "AllLongGuns", "RHPD Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 5 };
        Agency BCSO = new Agency("~r~", "BCSO", "BCSO", "Blaine County Sheriff", "Red", Classification.Sheriff, "BCSheriffPeds", "BCSOVehicles", "BCS ", "Tasers", "LimitedSidearms", "LimitedLongGuns", "BCSO Deputy") { MaxWantedLevelSpawn = 4, HeadDataGroupID = "AllHeads", Division = 10 };

        Agency LSPPFEJ = Extensions.DeepCopy(LSPP);

        LSPPFEJ.PersonnelID = "LSPPPeds";
        LSPPFEJ.VehiclesID = "LSPPVehicles";

        Agency LSIAPDFEJ = Extensions.DeepCopy(LSIAPD);
        LSIAPDFEJ.PersonnelID = "LSIAPDPeds";
        LSIAPDFEJ.VehiclesID = "LSIAPDVehicles";


        List< Agency> FullAgenciesList = new List<Agency>
        {
            LSPD,LSPDVW,LSPDELS,
            DPPD,RHPD,
            LSSD,LSSDVW,LSSDDV,LSSDMJ,
            BCSO,
            LSPDASD,LSSDASD,
            LSPPFEJ,LSIAPDFEJ,
            NOOSE,FIB,DOA,SAHP,SASPA,SAPR,SACG,
            ARMY,
            LSFDFire,LSMC,MRH,LSFD,UNK,
            NYSP,LCPD,

            GRPSECHS,SECURO,MERRY,BOBCAT,
        };
        Serialization.SerializeParams(FullAgenciesList, "Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedJurisdiction\\Agencies_FullExpandedJurisdiction.xml");
        Serialization.SerializeParams(FullAgenciesList, "Plugins\\LosSantosRED\\AlternateConfigs\\EUP\\Agencies_EUP.xml");
    }
    private void DefaultConfig_LosSantos2008()
    {
        Agency LSPD2008 = Extensions.DeepCopy(LSPD);
        Agency LSPDASD2008 = Extensions.DeepCopy(LSPDASD);
        Agency LSSD2008 = Extensions.DeepCopy(LSSD);
        Agency LSSDASD2008 = Extensions.DeepCopy(LSSDASD);
        Agency NOOSE2008 = Extensions.DeepCopy(NOOSE);
        Agency FIB2008 = Extensions.DeepCopy(FIB);
        Agency DOA2008 = Extensions.DeepCopy(DOA);
        Agency SAHP2008 = Extensions.DeepCopy(SAHP);
        Agency SASPA2008 = Extensions.DeepCopy(SASPA);
        Agency SAPR2008 = Extensions.DeepCopy(SAPR);
        Agency SACG2008 = Extensions.DeepCopy(SACG);
        Agency ARMY2008 = Extensions.DeepCopy(ARMY);
        Agency GRPSECHS2008 = Extensions.DeepCopy(GRPSECHS);
        Agency SECURO2008 = Extensions.DeepCopy(SECURO);
        Agency MERRY2008 = Extensions.DeepCopy(MERRY);
        Agency BOBCAT2008 = Extensions.DeepCopy(BOBCAT);

        List<Agency> AgenciesList2008 = new List<Agency>
        {
            LSPD2008,LSPDASD2008,LSSD2008,LSSDASD2008,NOOSE2008,FIB2008,DOA2008,SAHP2008,SASPA2008,SAPR2008,SACG2008,ARMY2008,LSFDFire,LSMC,MRH,LSFD,UNK,
            NYSP,GRPSECHS2008,SECURO2008,MERRY2008,BOBCAT2008,
        };
        foreach(Agency ag in AgenciesList2008)
        {
            if(!string.IsNullOrEmpty(ag.LessLethalWeaponsID) && ag.LessLethalWeaponsID == "Tasers")
            {
                ag.LessLethalWeaponsID = "Nightsticks";
            }
        }
        Serialization.SerializeParams(AgenciesList2008, "Plugins\\LosSantosRED\\AlternateConfigs\\LosSantos2008\\Agencies_LosSantos2008.xml");
    }
    private void DefaultConfig_LibertyCity()
    {
        List<Agency> SimpleAgenicesList = new List<Agency>
        {
            LCPD,
            new Agency("~b~", "ASP","ASP", "Alderney State Police", "Blue", Classification.Police, "SheriffPeds", "LCPDVehicles", "ASP ","Tasers","AllSidearms","AllLongGuns", "ASP Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 1 },
            new Agency("~b~", "LCPD-ASD","LCPD-ASD", "Liberty City Police Department - Air Support Division", "Blue", Classification.Police, "StandardCops", "PoliceHeliVehicles", "ASD ","Tasers","HeliSidearms","HeliLongGuns", "LSPD Officer") { MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 6  },
           NOOSE,FIB,DOA,ARMY,
            NYSP,GRPSECHS,
            SECURO,
            MERRY,
            BOBCAT,
            new Agency("~r~", "FDLC","FDLC", "Liberty City Fire Department", "Red", Classification.Fire, "Firefighters", "Firetrucks", "FD ",null,null, null, "FDLC Firefighter") { MaxWantedLevelSpawn = 0, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads"  },
            new Agency("~w~", "LCMC","LCMC", "Liberty City Medical Center", "White", Classification.EMS, "BlueEMTs", "Amublance1", "MC ",null,null, null, "LCMC EMT") { MaxWantedLevelSpawn = 0, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads"  },
            new Agency("~s~", "UNK","UNK", "Unknown Agency", "White", Classification.Other, null, null, "",null,null,null,"Officer") { MaxWantedLevelSpawn = 0 },
        };
        Serialization.SerializeParams(SimpleAgenicesList, "Plugins\\LosSantosRED\\AlternateConfigs\\LibertyCity\\Agencies_LibertyCity.xml");
    }
    private void DefaultConfig_Simple()
    {
        Agency LSMC_Simple = LSMC.Copy();
        Agency MRH_Simple = MRH.Copy();
        Agency LSFD_Simple = LSFD.Copy();
        LSMC_Simple.PersonnelID = "EMTs";
        MRH_Simple.PersonnelID = "EMTs";
        LSFD_Simple.PersonnelID = "EMTs";

        Agency GRPSECHS_Simple = GRPSECHS.Copy();
        Agency SECURO_Simple = SECURO.Copy();
        Agency MERRY_Simple = MERRY.Copy();
        Agency BOBCAT_Simple = BOBCAT.Copy();


        GRPSECHS_Simple.PersonnelID = "SecurityPeds";
        SECURO_Simple.PersonnelID = "SecurityPeds";
        MERRY_Simple.PersonnelID = "SecurityPeds";
        BOBCAT_Simple.PersonnelID = "SecurityPeds";

        GRPSECHS_Simple.VehiclesID = "UnmarkedVehicles";
        SECURO_Simple.VehiclesID = "UnmarkedVehicles";
        MERRY_Simple.VehiclesID = "UnmarkedVehicles";
        BOBCAT_Simple.VehiclesID = "UnmarkedVehicles";

        List<Agency> SimpleAgenicesList = new List<Agency>
        {
            LSPD,LSPDASD,LSSD,LSSDASD,NOOSE,FIB,DOA,SAHP,SASPA,SAPR,SACG,ARMY,LSFDFire,LSMC_Simple,MRH_Simple,LSFD_Simple,UNK,
            NYSP,GRPSECHS_Simple,SECURO_Simple,MERRY_Simple,BOBCAT_Simple,
        };
        Serialization.SerializeParams(SimpleAgenicesList, "Plugins\\LosSantosRED\\AlternateConfigs\\Simple\\Agencies_Simple.xml");
    }

    public void Setup(IHeads heads, IDispatchableVehicles dispatchableVehicles, IDispatchablePeople dispatchablePeople, IIssuableWeapons issuableWeapons)
    {
        foreach(Agency agency in AgenciesList)
        {
            //EntryPoint.WriteToConsole($"AGENCY NAME {agency.FullName} LongGunsID {agency.LongGunsID} SideArmsID {agency.SideArmsID} PersonnelID {agency.PersonnelID} VehiclesID {agency.VehiclesID} HeadDataGroupID {agency.HeadDataGroupID}");
            agency.LessLethalWeapons = issuableWeapons.GetWeaponData(agency.LessLethalWeaponsID);
            agency.LongGuns = issuableWeapons.GetWeaponData(agency.LongGunsID);
            agency.SideArms = issuableWeapons.GetWeaponData(agency.SideArmsID);
            agency.Personnel = dispatchablePeople.GetPersonData(agency.PersonnelID);
            agency.Vehicles = dispatchableVehicles.GetVehicleData(agency.VehiclesID);
            agency.PossibleHeads = heads.GetHeadData(agency.HeadDataGroupID);
        }
    }

}