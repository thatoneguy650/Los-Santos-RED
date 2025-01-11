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
    private Agency USCG;
    private Agency ARMY;
    private Agency USMC;
    private Agency USAF;
    private Agency LSFDFire;
    private Agency LSFD;
    private Agency NYSP;
    private Agency USMS;
    private Agency LSLFG;
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
            DefaultConfig_LosSantos2008();
            DefaultConfig_LibertyCity();
            DefaultConfig_SunshineDream();
            DefaultConfig();
        }
    }
    public Agency GetAgency(string AgencyInitials)
    {
        if(string.IsNullOrEmpty(AgencyInitials))
        {
            return null;
        }
        return AgenciesList.Where(x => x.ID.ToLower() == AgencyInitials.ToLower()).FirstOrDefault();
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
        LSPD = new Agency("~b~", "LSPD", "LSPD", "Los Santos Police Department", "Blue", Classification.Police, "StandardCops", "LSPDVehicles", "LS ", "Tasers", "AllSidearms", "AllLongGuns", "LSPD Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 1 ,OffDutyDispatchPercent = 1, OffDutyPersonnelID = "OffDutyCops", OffDutyVehiclesID = "OffDutyCopVehicles" };
        LSPDVW = new Agency("~b~", "LSPD-VW", "LSPD-VW", "Los Santos Police - Vinewood Division", "Blue", Classification.Police, "StandardCops", "VWPDVehicles", "LSV ", "Tasers", "LimitedSidearms", "LimitedLongGuns", "LSPD Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 2, OffDutyDispatchPercent = 1, OffDutyPersonnelID = "OffDutyCops", OffDutyVehiclesID = "OffDutyCopVehicles" };
        LSPDELS = new Agency("~b~", "LSPD-ELS", "LSPD-ELS", "Los Santos Police - East Los Santos Division", "Blue", Classification.Police, "StandardCops", "EastLSPDVehicles", "LSE ", "Tasers", "LimitedSidearms", "LimitedLongGuns", "LSPD Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 3, OffDutyDispatchPercent = 1, OffDutyPersonnelID = "OffDutyCops", OffDutyVehiclesID = "OffDutyCopVehicles" };
        LSPDDP = new Agency("~b~", "LSPD-DP", "LSPD-DP", "Los Santos Police - Del Perro Division", "Blue", Classification.Police, "StandardCops", "DPPDVehicles", "VP ", "Tasers", "AllSidearms", "AllLongGuns", "LSPD Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 4, OffDutyDispatchPercent = 1, OffDutyPersonnelID = "OffDutyCops", OffDutyVehiclesID = "OffDutyCopVehicles" };
        LSPDRH = new Agency("~b~", "LSPD-RH", "LSPD-RH", "Los Santos Police - Rockford Hills Division", "Blue", Classification.Police, "StandardCops", "RHPDVehicles", "RH ", "Tasers", "AllSidearms", "AllLongGuns", "LSPD Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 5, OffDutyDispatchPercent = 1, OffDutyPersonnelID = "OffDutyCops", OffDutyVehiclesID = "OffDutyCopVehicles" };
        LSPDASD = new Agency("~b~", "LSPD-ASD", "LSPD-ASD", "Los Santos Police Department - Air Support Division", "Blue", Classification.Police, "LSPDASDPeds", "PoliceHeliVehicles", "ASD ", "Tasers", "HeliSidearms", "HeliLongGuns", "LSPD Officer") { MinWantedLevelSpawn = 3, MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 6 };     
        LSSD = new Agency("~r~", "LSSD", "LSSD", "Los Santos County Sheriff", "Red", Classification.Sheriff, "SheriffPeds", "LSSDVehicles", "LSCS ", "Tasers", "LimitedSidearms", "LimitedLongGuns", "LSSD Deputy") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 7 };
        LSSDVW = new Agency("~r~", "LSSD-VW", "LSSD-VW", "Los Santos Sheriff - Vinewood Division", "Red", Classification.Sheriff, "SheriffPeds", "VWHillsLSSDVehicles", "LSCS ", "Tasers", "LimitedSidearms", "LimitedLongGuns", "LSSD Deputy") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 8, OffDutyDispatchPercent = 1, OffDutyPersonnelID = "OffDutyCops", OffDutyVehiclesID = "OffDutyCopVehicles" };
        LSSDDV = new Agency("~r~", "LSSD-DV", "LSSD-DV", "Los Santos Sheriff - Davis Division", "Red", Classification.Sheriff, "SheriffPeds", "DavisLSSDVehicles", "LSCS ", "Tasers", "LimitedSidearms", "LimitedLongGuns", "LSSD Deputy") { MaxWantedLevelSpawn = 4, HeadDataGroupID = "AllHeads", Division = 9, OffDutyDispatchPercent = 1, OffDutyPersonnelID = "OffDutyCops", OffDutyVehiclesID = "OffDutyCopVehicles" };
        LSSDBC = new Agency("~r~", "LSSD-BC", "LSSD-BC", "Los Santos Sheriff - Blaine County Division", "Red", Classification.Sheriff, "SheriffPeds", "BCSOVehicles", "BCS ", "Nightsticks", "LimitedSidearms", "LimitedLongGuns", "LSSD Deputy") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 10, OffDutyDispatchPercent = 1, OffDutyPersonnelID = "OffDutyCops", OffDutyVehiclesID = "OffDutyCopVehicles" };
        LSSDMJ = new Agency("~r~", "LSSD-MJ", "LSSD-MJ", "Los Santos Sheriff - Majestic County Division", "Red", Classification.Sheriff, "SheriffPeds", "MajesticLSSDVehicles", "MCS ", "Tasers", "LimitedSidearms", "LimitedLongGuns", "LSSD Deputy") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 11, OffDutyDispatchPercent = 1, OffDutyPersonnelID = "OffDutyCops", OffDutyVehiclesID = "OffDutyCopVehicles" };
        LSSDASD = new Agency("~r~", "LSSD-ASD", "LSSD-ASD", "Los Santos Sheriffs Department - Air Support Division", "Red", Classification.Sheriff, "LSSDASDPeds", "SheriffHeliVehicles", "ASD ", "Tasers", "HeliSidearms", "HeliLongGuns", "LSSD Deputy") { MinWantedLevelSpawn = 3, MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 13 };     
        LSPP = new Agency("~p~", "LSPP", "LSPP", "Los Santos Port Police", "LightGray", Classification.Police, "StandardCops", "LSPPVehicles", "LSPP ", "Tasers", "LimitedSidearms", "LimitedLongGuns", "Port Authority Officer") { MaxWantedLevelSpawn = 3, SpawnLimit = 3, HeadDataGroupID = "AllHeads", Division = 15, OffDutyDispatchPercent = 1, OffDutyPersonnelID = "OffDutyCops", OffDutyVehiclesID = "OffDutyCopVehicles" };
        LSIAPD = new Agency("~p~", "LSIAPD", "LSIAPD", "Los Santos International Airport Police Department", "LightBlue", Classification.Police, "StandardCops", "LSIAPDVehicles", "LSA ", "Tasers", "AllSidearms", "AllLongGuns", "LSIAPD Officer") { MaxWantedLevelSpawn = 3, SpawnLimit = 3, HeadDataGroupID = "AllHeads", Division = 16, OffDutyDispatchPercent = 1, OffDutyPersonnelID = "OffDutyCops", OffDutyVehiclesID = "OffDutyCopVehicles" };

        SAHP = new Agency("~y~", "SAHP", "SAHP", "San Andreas Highway Patrol", "Yellow", Classification.State, "SAHPPeds", "SAHPVehicles", "HP ", "Tasers", "LimitedSidearms", "LimitedLongGuns", "SAHP Officer") { MaxWantedLevelSpawn = 3, SpawnsOnHighway = true, HeadDataGroupID = "AllHeads", OffDutyDispatchPercent = 1, OffDutyPersonnelID = "OffDutyCops", OffDutyVehiclesID = "OffDutyCopVehicles" };
        SASPA = new Agency("~o~", "SASPA", "SASPA", "San Andreas State Prison Authority", "Orange", Classification.State, "PrisonPeds", "PrisonVehicles", "SASPA ", "Tasers", "AllSidearms", "AllLongGuns", "SASPA Officer") { MaxWantedLevelSpawn = 3, SpawnLimit = 4, HeadDataGroupID = "AllHeads" };
        SAPR = new Agency("~g~", "SAPR", "SAPR", "San Andreas State Parks Ranger", "Green", Classification.State, "ParkRangers", "ParkRangerVehicles", "", "Tasers", "AllSidearms", "AllLongGuns", "SA Parks Ranger") { MaxWantedLevelSpawn = 3, SpawnLimit = 4, HeadDataGroupID = "AllHeads" };
        USCG = new Agency("~w~", "USCG", "USCG", "U.S. Coast Guard", "White", Classification.Federal, "CoastGuardPeds", "CoastGuardVehicles", "USCG ", "Tasers", "AllSidearms", "BestLongGuns", "Coast Guard Officer") { MaxWantedLevelSpawn = 4, SpawnLimit = 4, HeadDataGroupID = "AllHeads" };

        LCPD = new Agency("~b~", "LCPD", "LCPD", "Liberty City Police Department", "Blue", Classification.Police, "LCPDPeds", "LCPDVehicles", "LC ", "Tasers", "AllSidearms", "AllLongGuns", "LCPD Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 1, OffDutyDispatchPercent = 1, OffDutyPersonnelID = "OffDutyCops", OffDutyVehiclesID = "OffDutyCopVehicles" };
        NYSP = new Agency("~b~", "NYSP", "NYSP", "North Yankton State Patrol", "Blue", Classification.Police, "NYSPPeds", "NYSPVehicles", "NYSP ", "Nightsticks", "LimitedSidearms", "LimitedLongGuns", "NYSP Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", OffDutyDispatchPercent = 1, OffDutyPersonnelID = "OffDutyCops", OffDutyVehiclesID = "OffDutyCopVehicles" };

        NOOSE = new Agency("~r~", "NOOSE", "NOOSE", "National Office of Security Enforcement", "DarkSlateGray", Classification.Federal, "NOOSEPeds", "NOOSEVehicles", "", "Tasers", "BestSidearms", "BestLongGuns", "NOOSE Officer") { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads" };
        FIB = new Agency("~p~", "FIB", "FIB", "Federal Investigation Bureau", "Purple", Classification.Federal, "FIBPeds", "FIBVehicles", "FIB ", "Tasers", "BestSidearms", "BestLongGuns", "FIB Agent") { MaxWantedLevelSpawn = 5, SpawnLimit = 6, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads" };
        DOA = new Agency("~p~", "DOA", "DOA", "Drug Observation Agency", "Purple", Classification.Federal, "DOAPeds", "DOAVehicles", "DOA ", "Tasers", "AllSidearms", "AllLongGuns", "DOA Agent") { MaxWantedLevelSpawn = 3, SpawnLimit = 4, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads" };    
           
        
        ARMY = new Agency("~u~", "ARMY", "ARMY", "U.S. Army", "Black", Classification.Military, "ArmyPeds", "ArmyVehicles", "", null, "MilitarySidearms", "MilitaryLongGuns", "Soldier") { MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads" };
        USAF = new Agency("~u~", "USAF", "USAF", "U.S. Air Force", "Black", Classification.Military, "USAFPeds", "USAFVehicles", "", null, "MilitarySidearms", "MilitaryLongGuns", "Airman") { MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads" };
        USMC = new Agency("~u~", "USMC", "USMC", "U.S. Marine Corps", "Black", Classification.Military, "USMCPeds", "USMCVehicles", "", null, "MilitarySidearms", "MilitaryLongGuns", "Marine") { MinWantedLevelSpawn = 6, MaxWantedLevelSpawn = 10, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads" };





        USMS = new Agency("~r~", "USMS", "Marshals Service", "US Marshals Service", "DarkSlateGray", Classification.Marshal, "MarshalsServicePeds", "MarshalsServiceVehicles", "", "Tasers", "BestSidearms", "BestLongGuns", "Marshals Service Officer") { MaxWantedLevelSpawn = 3, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads" };


        LSLFG = new Agency("~r~", "LSLFG", "LS Lifeguards", "Los Santos Lifeguards", "Red", Classification.EMS, "LSLifeguardPeds", "LSLifeguardVehicles", "LFG ", null, null, null, "Lifeguard") { MaxWantedLevelSpawn = 2,SpawnLimit = 3, HeadDataGroupID = "AllHeads" };

        LSFDFire = new Agency("~r~", "LSFD", "LSFD", "Los Santos Fire Department", "Red", Classification.Fire, "Firefighters", "Firetrucks", "LSFD ", "FireExtinguisher", null, null, "LSFD Firefighter") { MaxWantedLevelSpawn = 0,  HeadDataGroupID = "AllHeads" };
        
        LSMC = new Agency("~w~", "LSMC", "LSMC", "Los Santos Medical Center", "White", Classification.EMS, "GreenEMTs", "Amublance1", "LSMC ", null, null, null, "LSMC EMT") { MaxWantedLevelSpawn = 0, HeadDataGroupID = "AllHeads" };
        MRH = new Agency("~w~", "MRH", "MRH", "Mission Row Hospital", "White", Classification.EMS, "BlueEMTs", "Amublance2", "MRH ", null, null, null, "MRH Officer") { MaxWantedLevelSpawn = 0, HeadDataGroupID = "AllHeads" };
        LSFD = new Agency("~w~", "LSFD-EMS", "LSFD-EMS", "Los Santos Fire Department EMS", "White", Classification.EMS, "GreenEMTs", "Amublance3", "LSFD ", null, null, null, "LSFD EMT") { MaxWantedLevelSpawn = 0, HeadDataGroupID = "AllHeads" };
        
        GRPSECHS = new Agency("~g~", "GRP6", "G6", "Gruppe Sechs", "Green",Classification.Security, "GruppeSechsPeds", "GroupSechsVehicles", "GS ","Tasers", "LimitedSidearms", null, "Gruppe Sechs Officer") { MaxWantedLevelSpawn = 2, HeadDataGroupID = "AllHeads" };  
        SECURO = new Agency("~b~", "SECURO", "SecuroServ", "SecuroServ", "Black", Classification.Security, "SecuroservPeds", "SecuroservVehicles", "SS ", "Tasers", "LimitedSidearms", null, "SecuroServ Officer") { MaxWantedLevelSpawn = 2, HeadDataGroupID = "AllHeads" };
        MERRY = new Agency("~w~", "MERRY", "Merryweather", "Merryweather Security", "White", Classification.Security, "MerryweatherSecurityPeds", "MerryweatherPatrolVehicles", "MW ", "Tasers", "LimitedSidearms", null, "Merryweather Officer") { MaxWantedLevelSpawn = 2, HeadDataGroupID = "AllHeads" };
        BOBCAT = new Agency("~w~", "BOBCAT", "Bobcat", "Bobcat Security", "White", Classification.Security, "BobcatPeds", "BobcatSecurityVehicles", "BC ", "Tasers", "LimitedSidearms", null, "Bobcat Officer") { MaxWantedLevelSpawn = 2, HeadDataGroupID = "AllHeads" };
        
        UNK = new Agency("~s~", "UNK", "UNK", "Unknown Agency", "White", Classification.Other, null, null, "", null, null, null, "Officer") { MaxWantedLevelSpawn = 0 };
    }
    private void DefaultConfig()
    {
        DefaultAgency = new Agency("~b~", "LSPD", "LSPD", "Los Santos Police Department", "Blue", Classification.Police, "StandardCops", "LSPDVehicles", "LS ", "Tasers", "AllSidearms", "AllLongGuns", "LSPD Officer");
        AgenciesList = new List<Agency>
        {
            LSPD,LSPDVW,LSPDELS,LSPDDP,LSPDRH,LSPDASD,
            LSSD,LSSDVW,LSSDDV,LSSDBC,LSSDMJ,LSSDASD,LSPP,LSIAPD,
            SAHP,SASPA,SAPR,USCG,
            NYSP,//LCPD,
            LSLFG,
            NOOSE,FIB,DOA,ARMY,USMC,USAF,USMS,
            LSFDFire,LSMC,MRH,LSFD, 
            GRPSECHS,SECURO,MERRY,BOBCAT,
            UNK,
        };

        Serialization.SerializeParams(AgenciesList, ConfigFileName);
    }
    private void DefaultConfig_FullExpanded()
    {
        
        Agency DPPD = new Agency("~b~", "DPPD", "DPPD", "Del Perro Police Department", "Blue", Classification.Police, "DPPDCops", "DPPDVehicles", "DP ", "Tasers", "AllSidearms", "AllLongGuns", "DPPD Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 4, OffDutyDispatchPercent = 1, OffDutyPersonnelID = "OffDutyCops", OffDutyVehiclesID = "OffDutyCopVehicles" };
        Agency RHPD = new Agency("~b~", "RHPD", "RHPD", "Rockford Hills Police Department", "Blue", Classification.Police, "RHPDCops", "RHPDVehicles", "RH ", "Tasers", "AllSidearms", "AllLongGuns", "RHPD Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 5, OffDutyDispatchPercent = 1, OffDutyPersonnelID = "OffDutyCops", OffDutyVehiclesID = "OffDutyCopVehicles" };
        Agency BCSO = new Agency("~r~", "BCSO", "BCSO", "Blaine County Sheriff", "Red", Classification.Sheriff, "BCSheriffPeds", "BCSOVehicles", "BCS ", "Tasers", "LimitedSidearms", "LimitedLongGuns", "BCSO Deputy") { MaxWantedLevelSpawn = 4, HeadDataGroupID = "AllHeads", Division = 10, OffDutyDispatchPercent = 1, OffDutyPersonnelID = "OffDutyCops", OffDutyVehiclesID = "OffDutyCopVehicles" };

        Agency LSPPFEJ = Extensions.DeepCopy(LSPP);

        LSPPFEJ.PersonnelID = "LSPPPeds";
        LSPPFEJ.VehiclesID = "LSPPVehicles";

        Agency LSIAPDFEJ = Extensions.DeepCopy(LSIAPD);
        LSIAPDFEJ.PersonnelID = "LSIAPDPeds";
        LSIAPDFEJ.VehiclesID = "LSIAPDVehicles";

        Agency SAHPFEJ = Extensions.DeepCopy(SAHP);
        Agency LSPDFEJ = Extensions.DeepCopy(LSPD);
        Agency LSPDVWFEJ = Extensions.DeepCopy(LSPDVW);
        Agency LSPDELSFEJ = Extensions.DeepCopy(LSPDELS);
        SAHPFEJ.MaxWantedLevelSpawn = 4;
        LSPDFEJ.MaxWantedLevelSpawn = 4;
        LSPDVWFEJ.MaxWantedLevelSpawn = 4;
        LSPDELSFEJ.MaxWantedLevelSpawn = 4;

        Agency LSSDFEJ = Extensions.DeepCopy(LSSD);
        Agency LSSDVWFEJ = Extensions.DeepCopy(LSSDVW);
        Agency LSSDDVFEJ = Extensions.DeepCopy(LSSDDV);
        Agency LSSDMJFEJ = Extensions.DeepCopy(LSSDMJ);
        LSSDFEJ.MaxWantedLevelSpawn = 4;
        LSSDVWFEJ.MaxWantedLevelSpawn = 4;
        LSSDDVFEJ.MaxWantedLevelSpawn = 4;
        LSSDMJFEJ.MaxWantedLevelSpawn = 4;

        Agency LSFDFireFEJ = Extensions.DeepCopy(LSFDFire);

        Agency LSFDFEJ = Extensions.DeepCopy(LSFD);

        Agency BorderPatrol = new Agency("~r~", "NOOSE-BP", "Border Patrol", "NOoSE Border Patrol", "DarkSlateGray", Classification.Federal, "BorderPatrolPeds", "BorderPatrolVehicles", "", "Tasers", "BestSidearms", "BestLongGuns", "Border Patrol Officer") { MaxWantedLevelSpawn = 3, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads" };
        Agency NOOSEPIA = new Agency("~r~", "NOOSE-PIA", "Patriotism and Immigration Authority", "NOoSE Patriotism and Immigration Authority", "DarkSlateGray", Classification.Federal, "NOOSEPIAPeds", "NOOSEPIAVehicles", "", "Tasers", "BestSidearms", "BestLongGuns", "NOOSE-PIA Officer") { MaxWantedLevelSpawn = 4, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads" };
        Agency NOOSESEP = new Agency("~r~", "NOOSE", "Security Enforcement Police", "NOoSE Security Enforcement Police", "DarkSlateGray", Classification.Federal, "NOOSESEPPeds", "NOOSESEPVehicles", "", "Tasers", "BestSidearms", "BestLongGuns", "NOOSE-SEP Officer") { MaxWantedLevelSpawn = 4, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads" };


        Agency USNPS = new Agency("~g~","USNPS","USNPS","US National Park Service","Green",Classification.Federal, "USNPSParkRangers", "USNPSParkRangersVehicles", "NPS ", "Tasers", "AllSidearms", "AllLongGuns", "US Park Ranger") { MaxWantedLevelSpawn = 3, SpawnLimit = 3, HeadDataGroupID = "AllHeads" };
        Agency LSDPR = new Agency("~g~", "LSDPR", "LSDPR", "Los Santos Dept. Parks & Rec", "Green", Classification.Police, "LSDPRParkRangers", "LSDPRParkRangersVehicles", "DPR ", "Tasers", "AllSidearms", "AllLongGuns", "LSDPR Park Ranger") { MaxWantedLevelSpawn = 3, SpawnLimit = 3, HeadDataGroupID = "AllHeads" };
        Agency SADFW = new Agency("~g~", "SADFW", "SADFW", "San Andreas Dept. Fish and Wildlife", "Green", Classification.State, "SADFWParkRangers", "SADFWParkRangersVehicles", "DFW ", "Tasers", "AllSidearms", "AllLongGuns", "Game Warden") { MaxWantedLevelSpawn = 3, SpawnLimit = 3, HeadDataGroupID = "AllHeads" };

        Agency CHUFFSEC = new Agency("~w~", "CHUFF", "Chuff", "Chuff Security", "White", Classification.Security, "ChuffPeds", "ChuffVehicles", "MW ", "Tasers", "LimitedSidearms", null, "Chuff Officer") { MaxWantedLevelSpawn = 2, HeadDataGroupID = "AllHeads" };
        Agency LNLSEC = new Agency("~w~", "LNL", "Lock & Load", "Lock & Load Security", "White", Classification.Security, "LNLPeds", "LNLVehicles", "BC ", "Tasers", "LimitedSidearms", null, "L&L Officer") { MaxWantedLevelSpawn = 2, HeadDataGroupID = "AllHeads" };

        LSFDFireFEJ.PersonnelID = "LSFDPeds";
        LSFDFireFEJ.VehiclesID = "LSFDVehicles";

        Agency LSCoFDFire = new Agency("~r~", "LSCoFD", "LSCoFD", "Los Santos County Fire Department", "Red", Classification.Fire, "LSCOFDPeds", "LSCOFDVehicles", "LSCO ", "LSCO ", "FireExtinguisher", null, "LSCoFD Firefighter") { MaxWantedLevelSpawn = 0, HeadDataGroupID = "AllHeads" };
        Agency BCFDFire = new Agency("~r~", "BCFD", "BCFD", "Blaine County County Fire Department", "Red", Classification.Fire, "BCFDPeds", "BCFDVehicles", "BCFD ", "BCFD ", "FireExtinguisher", null, "BCFD Firefighter") { MaxWantedLevelSpawn = 0,  HeadDataGroupID = "AllHeads" }; 
        Agency SanFire = new Agency("~r~", "SANFIRE", "San Fire", "San Andreas Dept of Forestry and Fire Protection", "Red", Classification.Fire, "SanFirePeds", "SanFireVehicles", "SANF ", "FireExtinguisher", null, null, "SANFIRE Firefighter") { MaxWantedLevelSpawn = 0, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads" };
        //sanfire has no EMTS, no full gear and some LEOs, not sure what to do with it

        LSFDFEJ.PersonnelID = "LSFDEMTPeds";
        LSFDFEJ.VehiclesID = "LSFDEMTVehicles";

        Agency LSCoFDEMT = new Agency("~w~", "LSCoFD-EMS", "LSCoFD-EMS", "Los Santos County Fire Department EMS", "White", Classification.EMS, "LSCOFDEMTPeds", "LSCOFDEMSVehicles", "LSCO ", null, null, null, "LSCoFD EMT") { MaxWantedLevelSpawn = 0, HeadDataGroupID = "AllHeads" };
        Agency BCFDEMT = new Agency("~w~", "BCFD-EMS", "BCFD-EMS", "Blaine County Fire Department EMS", "White", Classification.EMS, "BCFDEMTPeds", "BCFDEMSVehicles", "BCFD ", null, null, null, "BCFD EMT") { MaxWantedLevelSpawn = 0, HeadDataGroupID = "AllHeads" };
        Agency SAMSEMT = new Agency("~w~", "SAMS", "SAMS", "San Andreas Medical Services", "White", Classification.EMS, "SAMSEMTPeds", "SAMSVehicles", "SAMS ", null, null, null, "SAMS EMT") { MaxWantedLevelSpawn = 0, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads" };

        Agency LSMCFEJ = Extensions.DeepCopy(LSMC);
        Agency MRHFEJ = Extensions.DeepCopy(MRH);

        LSMCFEJ.VehiclesID = "LSMCVehicles";
        MRHFEJ.VehiclesID = "MRHVehicles";

        List <Agency> FullAgenciesList = new List<Agency>
        {
            LSPDFEJ,LSPDVWFEJ,LSPDELSFEJ,
            DPPD,RHPD,
            LSSDFEJ,LSSDVWFEJ,LSSDDVFEJ,LSSDMJFEJ,
            BCSO,
            LSPDASD,LSSDASD,
            LSPPFEJ,LSIAPDFEJ,
            SAHPFEJ,SASPA,SAPR,USCG,


            USNPS,LSDPR,SADFW,

            NYSP,//LCPD,
            LSLFG,
            FIB,BorderPatrol,NOOSEPIA,NOOSESEP,DOA,ARMY,USMC,USAF,USMS,
            LSFDFireFEJ,LSCoFDFire,BCFDFire,SanFire,
            LSMCFEJ,MRHFEJ,LSFDFEJ,LSCoFDEMT,BCFDEMT,SAMSEMT,
            UNK,
            GRPSECHS,SECURO,MERRY,BOBCAT,CHUFFSEC,LNLSEC,
            UNK,
        };
        Serialization.SerializeParams(FullAgenciesList, "Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedJurisdiction\\Variations\\Full\\Agencies_FullExpandedJurisdiction.xml");
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
        Agency SACG2008 = Extensions.DeepCopy(USCG);
        Agency ARMY2008 = Extensions.DeepCopy(ARMY);
        Agency USMC2008 = Extensions.DeepCopy(USMC);
        Agency USAF2008 = Extensions.DeepCopy(USAF);
        Agency GRPSECHS2008 = Extensions.DeepCopy(GRPSECHS);
        Agency SECURO2008 = Extensions.DeepCopy(SECURO);
        Agency MERRY2008 = Extensions.DeepCopy(MERRY);
        Agency BOBCAT2008 = Extensions.DeepCopy(BOBCAT);
        Agency USMS2008 = Extensions.DeepCopy(USMS);
        Agency LSMC2008 = Extensions.DeepCopy(LSMC);
        LSMC2008.VehiclesID = "LSMCVehicles";
        Agency MRH2008 = Extensions.DeepCopy(MRH);
        MRH2008.VehiclesID = "MRHVehicles";
        Agency LSFDFire2008 = Extensions.DeepCopy(LSFDFire);
        LSFDFire2008.VehiclesID = "LSFDVehicles";
        Agency LSFD2008 = Extensions.DeepCopy(LSFD);
        LSFD2008.VehiclesID = "LSFDEMTVehicles";
        List<Agency> AgenciesList2008 = new List<Agency>
        {
            LSPD2008,LSPDASD2008,
            LSSD2008,LSSDASD2008,        
            SAHP2008,SASPA2008,SAPR2008,SACG2008,
            NYSP,
            LSLFG,
            NOOSE2008,FIB2008,DOA2008,ARMY2008,USMC2008,USAF2008,USMS2008,
            LSFDFire2008,LSMC2008,MRH2008,LSFD2008,
            GRPSECHS2008,SECURO2008,MERRY2008,BOBCAT2008,
            UNK,
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

        Agency BorderPatrol = new Agency("~r~", "NOOSE-BP", "Border Patrol", "NOoSE Border Patrol", "DarkSlateGray", Classification.Federal, "NOOSEPeds", "BorderPatrolVehicles", "", "Tasers", "BestSidearms", "BestLongGuns", "Border Patrol Officer") { MaxWantedLevelSpawn = 3, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads" };
        Agency NOOSEPIA = new Agency("~r~", "NOOSE-PIA", "Patriotism and Immigration Authority", "NOoSE Patriotism and Immigration Authority", "DarkSlateGray", Classification.Federal, "NOOSEPeds", "NOOSEPIAVehicles", "", "Tasers", "BestSidearms", "BestLongGuns", "NOOSE-PIA Officer") { MaxWantedLevelSpawn = 4, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads" };
        Agency NOOSESEP = new Agency("~r~", "NOOSE", "Security Enforcement Police", "NOoSE Security Enforcement Police", "DarkSlateGray", Classification.Federal, "NOOSEPeds", "NOOSESEPVehicles", "", "Tasers", "BestSidearms", "BestLongGuns", "NOOSE-SEP Officer") { MaxWantedLevelSpawn = 4, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads" };
        Agency USNPS = new Agency("~g~", "USNPS", "USNPS", "US National Park Service", "Green", Classification.Federal, "ParkRangers", "USNPSParkRangersVehicles", "NPS ", "Tasers", "AllSidearms", "AllLongGuns", "US Park Ranger") { MaxWantedLevelSpawn = 3, SpawnLimit = 3, HeadDataGroupID = "AllHeads" };
        Agency HMSEMT = new Agency("~w~", "HMS", "HMS", "Homeland Medical Services", "White", Classification.EMS, "BlueEMTs", "HMSVehicles", "SAMS ", null, null, null, "HMS EMT") { MaxWantedLevelSpawn = 0, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads" };

        List<Agency> LCAgenicesList = new List<Agency>
        {





            LCPD,
            new Agency("~b~", "LCPD-ASD","LCPD-ASD", "Liberty City Police Department - Air Support Division", "Blue", Classification.Police, "LCPDPeds", "LCPDHeliVehicles", "ASD ","Tasers","HeliSidearms","HeliLongGuns", "LSPD Officer") { MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 6  },

            new Agency("~b~", "ASP-ASD","ASP-ASD", "Alderney State Police - Air Support Division", "Blue", Classification.Police, "ASPPeds", "ASPHeliVehicles", "ASP ","Tasers","HeliSidearms","HeliLongGuns", "ASP Officer") { MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 4  },

            new Agency("~b~", "ASP","ASP", "Alderney State Police", "Blue", Classification.Police, "ASPPeds", "ASPVehicles", "ASP ","Tasers","AllSidearms","AllLongGuns", "ASP Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 1,OffDutyDispatchPercent = 1, OffDutyPersonnelID = "OffDutyCops", OffDutyVehiclesID = "OffDutyCopVehicles" },
            NYSP,
            BorderPatrol,NOOSEPIA,NOOSESEP,
            USNPS,
            FIB,DOA,ARMY,USMC,USAF,USMS,USCG,
            new Agency("~r~", "FDLC","FDLC", "Liberty City Fire Department", "Red", Classification.Fire, "FDLCFirePeds", "FDNYFireVehicles", "FD ","FireExtinguisher",null, null, "FDLC Firefighter") { MaxWantedLevelSpawn = 0, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads"  },
            new Agency("~w~", "FDLC-EMS","FDLC-EMS", "Liberty City Fire Department EMS", "White", Classification.EMS, "GreenEMTs", "FDNYEMTVehicles", "LC ",null,null, null, "FDLC EMT") { MaxWantedLevelSpawn = 0, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads"  },
            HMSEMT,
            GRPSECHS,SECURO,MERRY,BOBCAT,
            UNK,
        };
        Serialization.SerializeParams(LCAgenicesList, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LibertyConfigFolder}\\Agencies_{StaticStrings.LibertyConfigSuffix}.xml");


        List<Agency> LCPPAgenicesList = new List<Agency>
        {


            LSPD,LSPDVW,LSPDELS,LSPDDP,LSPDRH,LSPDASD,
            LSSD,LSSDVW,LSSDDV,LSSDBC,LSSDMJ,LSSDASD,LSPP,LSIAPD,
            SAHP,SASPA,SAPR,USCG,
            NYSP,//LCPD,
            LSLFG,
            NOOSE,FIB,DOA,ARMY,USMC,USAF,USMS,
            LSFDFire,LSMC,MRH,LSFD,
            GRPSECHS,SECURO,MERRY,BOBCAT,


            LCPD,
            new Agency("~b~", "LCPD-ASD","LCPD-ASD", "Liberty City Police Department - Air Support Division", "Blue", Classification.Police, "LCPDPeds", "LCPDHeliVehicles", "ASD ","Tasers","HeliSidearms","HeliLongGuns", "LSPD Officer") { MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 6  },

            new Agency("~b~", "ASP-ASD","ASP-ASD", "Alderney State Police - Air Support Division", "Blue", Classification.Police, "ASPPeds", "ASPHeliVehicles", "ASP ","Tasers","HeliSidearms","HeliLongGuns", "ASP Officer") { MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 4  },

            new Agency("~b~", "ASP","ASP", "Alderney State Police", "Blue", Classification.Police, "ASPPeds", "ASPVehicles", "ASP ","Tasers","AllSidearms","AllLongGuns", "ASP Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 1,OffDutyDispatchPercent = 1, OffDutyPersonnelID = "OffDutyCops", OffDutyVehiclesID = "OffDutyCopVehicles" },
            
            BorderPatrol,NOOSEPIA,NOOSESEP,


            new Agency("~r~", "FDLC","FDLC", "Liberty City Fire Department", "Red", Classification.Fire, "FDLCFirePeds", "FDNYFireVehicles", "FD ","FireExtinguisher",null, null, "FDLC Firefighter") { MaxWantedLevelSpawn = 0, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads"  },
            new Agency("~w~", "FDLC-EMS","FDLC-EMS", "Liberty City Fire Department EMS", "White", Classification.EMS, "GreenEMTs", "FDNYEMTVehicles", "LC ",null,null, null, "FDLC EMT") { MaxWantedLevelSpawn = 0, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads"  },
            HMSEMT,

            UNK,
        };
        Serialization.SerializeParams(LCPPAgenicesList, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LPPConfigFolder}\\Agencies_{StaticStrings.LPPConfigSuffix}.xml");
    }
    private void DefaultConfig_SunshineDream()
    {
        List<Agency> SimpleAgenicesList = new List<Agency>
        {
            new Agency("~b~", "VCPD","VCPD", "Vice City Police Department", "Blue", Classification.Police, "VCPDPeds", "VCPDVehicles", "VC ","Tasers","AllSidearms","AllLongGuns", "VCPD Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 1,OffDutyDispatchPercent = 1, OffDutyPersonnelID = "OffDutyCops", OffDutyVehiclesID = "OffDutyCopVehicles" },
            new Agency("~b~", "VDPD","VDPD", "Vice-Dale Police Department", "Blue", Classification.Police, "VDPDPeds", "VDPDVehicles", "VD ","Tasers","AllSidearms","AllLongGuns", "VDPD Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 1,OffDutyDispatchPercent = 1, OffDutyPersonnelID = "OffDutyCops", OffDutyVehiclesID = "OffDutyCopVehicles" },


            new Agency("~b~", "VCPD-ASD","VCPD-ASD", "Vice City Police Department - Air Support Division", "Blue", Classification.Police, "VCPDHeliPeds", "VCPDHeliVehicles", "ASD ","Tasers","HeliSidearms","HeliLongGuns", "VCPD Officer") { MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 6  },
            NYSP,
            NOOSE,FIB,DOA,ARMY,USMC,USAF,USMS,
            new Agency("~r~", "FDVC","FDVC", "Vice City Fire Department", "Red", Classification.Fire, "Firefighters", "Firetrucks", "FD ","FireExtinguisher",null, null, "FDLC Firefighter") { MaxWantedLevelSpawn = 0, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads"  },
            new Agency("~w~", "VCMC","VCMC", "Vice City Medical Center", "White", Classification.EMS, "BlueEMTs", "Amublance1", "MC ",null,null, null, "LCMC EMT") { MaxWantedLevelSpawn = 0, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads"  },
            GRPSECHS,SECURO,MERRY,BOBCAT,
            UNK,
        };
        Serialization.SerializeParams(SimpleAgenicesList, "Plugins\\LosSantosRED\\AlternateConfigs\\SunshineDream\\Agencies_SunshineDream.xml");
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
            LSPD,LSPDASD,
            LSSD,LSSDASD,
            SAHP,SASPA,SAPR,USCG,
            NYSP,
            LSLFG,
            NOOSE,FIB,DOA,ARMY,USMC,USAF,USMS,
            LSFDFire,LSMC_Simple,MRH_Simple,LSFD_Simple,    
            GRPSECHS_Simple,SECURO_Simple,MERRY_Simple,BOBCAT_Simple,
            UNK,
        };
        Serialization.SerializeParams(SimpleAgenicesList, "Plugins\\LosSantosRED\\AlternateConfigs\\Simple\\Agencies_Simple.xml");
    }
    public void Setup(IHeads heads, IDispatchableVehicles dispatchableVehicles, IDispatchablePeople dispatchablePeople, IIssuableWeapons issuableWeapons)
    {
        foreach(Agency agency in AgenciesList)
        {
            agency.Setup(heads, dispatchableVehicles, dispatchablePeople, issuableWeapons);
            //EntryPoint.WriteToConsole($"AGENCY NAME {agency.FullName} LongGunsID {agency.LongGunsID} SideArmsID {agency.SideArmsID} PersonnelID {agency.PersonnelID} VehiclesID {agency.VehiclesID} HeadDataGroupID {agency.HeadDataGroupID}");
        }
    }

}