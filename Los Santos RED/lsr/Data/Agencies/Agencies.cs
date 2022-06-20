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
    private bool UseVanillaConfig = true;
    private List<Agency> AgenciesList;
    private Agency DefaultAgency;

    public Agencies()
    {

    }
    public void ReadConfig()
    {
        #if DEBUG
            UseVanillaConfig =  true;
#else
            UseVanillaConfig = true;
#endif

        DirectoryInfo taskDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo taskFile = taskDirectory.GetFiles("Agencies*.xml").OrderByDescending(x=> x.Name).FirstOrDefault();
        if(taskFile != null)
        {
            EntryPoint.WriteToConsole($"Deserializing 1 {taskFile.FullName}");
            AgenciesList = Serialization.DeserializeParams<Agency>(taskFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Deserializing 2 {ConfigFileName}");
            AgenciesList = Serialization.DeserializeParams<Agency>(ConfigFileName);
        }
        else
        {
            DefaultConfig_Gresk();
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
    private void DefaultConfig()
    {

        DefaultAgency = new Agency("~b~", "LSPD", "Los Santos Police Department", "Blue", Classification.Police, "StandardCops", "LSPDVehicles", "LS ", "AllSidearms", "AllLongGuns", "LSPD Officer");
        AgenciesList = new List<Agency>
        {
            new Agency("~b~", "LSPD", "Los Santos Police Department", "Blue", Classification.Police, "StandardCops", "LSPDVehicles", "LS ","AllSidearms","AllLongGuns", "LSPD Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 1 },
            new Agency("~b~", "LSPD-VW", "Los Santos Police - Vinewood Division", "Blue", Classification.Police, "StandardCops", "VWPDVehicles", "LSV ","LimitedSidearms","LimitedLongGuns", "LSPD Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 2  },
            new Agency("~b~", "LSPD-ELS", "Los Santos Police - East Los Santos Division", "Blue", Classification.Police, "StandardCops", "EastLSPDVehicles", "LSE ","LimitedSidearms","LimitedLongGuns", "LSPD Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 3  },
            new Agency("~b~", "LSPD-DP", "Los Santos Police - Del Perro Division", "Blue", Classification.Police, "StandardCops", "DPPDVehicles", "VP ","AllSidearms","AllLongGuns", "LSPD Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 4  },
            new Agency("~b~", "LSPD-RH", "Los Santos Police - Rockford Hills Division", "Blue", Classification.Police, "StandardCops", "RHPDVehicles", "RH ","AllSidearms","AllLongGuns", "LSPD Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 5  },



            new Agency("~b~", "LSPD-ASD", "Los Santos Police Department - Air Support Division", "Blue", Classification.Police, "StandardCops", "PoliceHeliVehicles", "ASD ","HeliSidearms","HeliLongGuns", "LSPD Officer") { MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 6  },

            new Agency("~r~", "LSSD", "Los Santos County Sheriff", "Red", Classification.Sheriff, "SheriffPeds", "LSSDVehicles", "LSCS ","LimitedSidearms","LimitedLongGuns", "LSSD Deputy") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 7  },
            new Agency("~r~", "LSSD-VW", "Los Santos Sheriff - Vinewood Division", "Red", Classification.Sheriff, "SheriffPeds", "VWHillsLSSDVehicles", "LSCS ","LimitedSidearms","LimitedLongGuns", "LSSD Deputy") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 8  },
            new Agency("~r~", "LSSD-CH", "Los Santos Sheriff - Chumash Division", "Red", Classification.Sheriff, "SheriffPeds", "ChumashLSSDVehicles", "LSCS ","LimitedSidearms","LimitedLongGuns", "LSSD Deputy") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 9  },
            new Agency("~r~", "LSSD-BC", "Los Santos Sheriff - Blaine County Division", "Red", Classification.Sheriff, "SheriffPeds", "BCSOVehicles", "BCS ","LimitedSidearms","LimitedLongGuns", "LSSD Deputy") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 10 },
            new Agency("~r~", "LSSD-MJ", "Los Santos Sheriff - Majestic County Division", "Red", Classification.Sheriff, "SheriffPeds", "BCSOVehicles", "MCS ","LimitedSidearms","LimitedLongGuns", "LSSD Deputy") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 11  },
           // new Agency("~r~", "LSSD-VN", "Los Santos Sheriff - Ventura County Division", "Red", Classification.Sheriff, "SheriffPeds", "BCSOVehicles", "VCS ","LimitedSidearms","LimitedLongGuns", "LSSD Deputy") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 12 },
            
            new Agency("~r~", "LSSD-ASD", "Los Santos Sheriffs Department - Air Support Division", "Red", Classification.Sheriff, "SheriffPeds", "SheriffHeliVehicles", "ASD ","HeliSidearms","HeliLongGuns", "LSSD Deputy") { MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 13  },


            new Agency("~b~", "GSPD", "Grapeseed Police Department", "Blue", Classification.Police, "StandardCops", "UnmarkedVehicles", "GS ","LimitedSidearms","LimitedLongGuns", "GSPD Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 14  },
            
            new Agency("~p~", "LSPA", "Port Authority of Los Santos", "LightGray", Classification.Police, "SecurityPeds", "UnmarkedVehicles", "LSPA ","LimitedSidearms","LimitedLongGuns", "Port Authority Officer") {MaxWantedLevelSpawn = 3, SpawnLimit = 3, HeadDataGroupID = "AllHeads",Division = 15  },
            new Agency("~p~", "LSIAPD", "Los Santos International Airport Police Department", "LightBlue", Classification.Police, "StandardCops", "LSPDVehicles", "LSA ","AllSidearms","AllLongGuns", "LSIAPD Officer") { MaxWantedLevelSpawn = 3, SpawnLimit = 3, HeadDataGroupID = "AllHeads", Division = 16  },

            new Agency("~r~", "NOOSE", "National Office of Security Enforcement", "DarkSlateGray", Classification.Federal, "NOOSEPeds", "NOOSEVehicles", "","BestSidearms","BestLongGuns", "NOOSE Officer") { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5,CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads" },
            new Agency("~p~", "FIB", "Federal Investigation Bureau", "Purple", Classification.Federal, "FIBPeds", "FIBVehicles", "FIB ","BestSidearms","BestLongGuns", "FIB Agent") { MaxWantedLevelSpawn = 5, SpawnLimit = 6,CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads"  },
            new Agency("~p~", "DOA", "Drug Observation Agency", "Purple", Classification.Federal, "DOAPeds", "UnmarkedVehicles", "DOA ","AllSidearms","AllLongGuns", "DOA Agent")  {MaxWantedLevelSpawn = 3, SpawnLimit = 4,CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads"  },
            
            new Agency("~y~", "SAHP", "San Andreas Highway Patrol", "Yellow", Classification.State, "SAHPPeds", "SAHPVehicles", "HP ","LimitedSidearms","LimitedLongGuns", "SAHP Officer") { MaxWantedLevelSpawn = 3, SpawnsOnHighway = true, HeadDataGroupID = "AllHeads"  },
            new Agency("~o~", "SASPA", "San Andreas State Prison Authority", "Orange", Classification.State, "PrisonPeds", "PrisonVehicles", "SASPA ","AllSidearms","AllLongGuns", "SASPA Officer") { MaxWantedLevelSpawn = 3, SpawnLimit = 2, HeadDataGroupID = "AllHeads"  },
            new Agency("~g~", "SAPR", "San Andreas Park Ranger", "Green", Classification.State, "ParkRangers", "ParkRangerVehicles", "","AllSidearms","AllLongGuns", "SA Park Ranger") { MaxWantedLevelSpawn = 3, SpawnLimit = 3, HeadDataGroupID = "AllHeads" },
            new Agency("~o~", "SACG", "San Andreas Coast Guard", "DarkOrange", Classification.State, "CoastGuardPeds", "CoastGuardVehicles", "SACG ","LimitedSidearms","LimitedLongGuns", "Coast Guard Officer"){ MaxWantedLevelSpawn = 3,SpawnLimit = 3, HeadDataGroupID = "AllHeads"  },
     
            new Agency("~u~", "ARMY", "Army", "Black", Classification.Military, "MilitaryPeds", "ArmyVehicles", "","MilitarySidearms","MilitaryLongGuns", "Soldier") { MinWantedLevelSpawn = 6,CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads"  },
              
            new Agency("~r~", "LSFD", "Los Santos Fire Department", "Red", Classification.Fire, "Firefighters", "Firetrucks", "LSFD ",null, null, "LSFD Firefighter") { MaxWantedLevelSpawn = 0, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads"  },
            new Agency("~w~", "LSMC", "Los Santos Medical Center", "White", Classification.EMS, "EMTs", "Amublance1", "LSMC ",null, null, "LSMC EMT") { MaxWantedLevelSpawn = 0, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads"  },
            new Agency("~w~", "MRH", "Mission Row Hospital", "White", Classification.EMS, "EMTs", "Amublance2", "MRH ",null, null, "MRH Officer") { MaxWantedLevelSpawn = 0, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads"  },
            new Agency("~w~", "LSFD", "Los Santos Fire Department", "White", Classification.EMS, "EMTs", "Amublance3", "LSFD ",null, null, "LSFD EMT") { MaxWantedLevelSpawn = 0, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads"  },
            new Agency("~s~", "UNK", "Unknown Agency", "White", Classification.Other, null, null, "",null,null,"Officer") { MaxWantedLevelSpawn = 0 },
        };

        Serialization.SerializeParams(AgenciesList, ConfigFileName);
    }
    private void DefaultConfig_Gresk()
    {
        DefaultAgency = new Agency("~b~", "LSPD", "Los Santos Police Department", "Blue", Classification.Police, "StandardCops", "LSPDVehicles", "LS ", "AllSidearms", "AllLongGuns", "LSPD Officer");
        List<Agency> SimpleAgenicesList = new List<Agency>
        {
            new Agency("~b~", "LSPD", "Los Santos Police Department", "Blue", Classification.Police, "StandardCops", "LSPDVehicles", "LS ","AllSidearms","AllLongGuns", "LSPD Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 1 },
           
            new Agency("~b~", "LSPD-VW", "Los Santos Police - Vinewood Division", "Blue", Classification.Police, "StandardCops", "VWPDVehicles", "LSV ","LimitedSidearms","LimitedLongGuns", "LSPD Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 2  },
            new Agency("~b~", "LSPD-ELS", "Los Santos Police - East Los Santos Division", "Blue", Classification.Police, "StandardCops", "EastLSPDVehicles", "LSE ","LimitedSidearms","LimitedLongGuns", "LSPD Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 3  },
            
            new Agency("~b~", "DPPD", "Del Perro Police Department", "Blue", Classification.Police, "DPPDCops", "DPPDVehicles", "DP ","AllSidearms","AllLongGuns", "LSPD Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 4  },
            new Agency("~b~", "RHPD", "Rockford Hills Police Department", "Blue", Classification.Police, "RHPDCops", "RHPDVehicles", "RH ","AllSidearms","AllLongGuns", "LSPD Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 5  },



            new Agency("~b~", "LSPD-ASD", "Los Santos Police Department - Air Support Division", "Blue", Classification.Police, "StandardCops", "PoliceHeliVehicles", "ASD ","HeliSidearms","HeliLongGuns", "LSPD Officer") { MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 6  },

            new Agency("~r~", "LSSD", "Los Santos County Sheriff", "Red", Classification.Sheriff, "SheriffPeds", "LSSDVehicles", "LSCS ","LimitedSidearms","LimitedLongGuns", "LSSD Deputy") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 7  },
            new Agency("~r~", "LSSD-VW", "Los Santos Sheriff - Vinewood Division", "Red", Classification.Sheriff, "SheriffPeds", "VWHillsLSSDVehicles", "LSCS ","LimitedSidearms","LimitedLongGuns", "LSSD Deputy") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 8  },
            new Agency("~r~", "LSSD-CH", "Los Santos Sheriff - Chumash Division", "Red", Classification.Sheriff, "SheriffPeds", "ChumashLSSDVehicles", "LSCS ","LimitedSidearms","LimitedLongGuns", "LSSD Deputy") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 9  },
            
            new Agency("~r~", "LSSD-MJ", "Los Santos Sheriff - Majestic County Division", "Red", Classification.Sheriff, "SheriffPeds", "MajesticLSSDVehicles", "MCS ","LimitedSidearms","LimitedLongGuns", "LSSD Deputy") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 11  },


            new Agency("~r~", "BCSD", "Blaine County Sheriff", "Red", Classification.Sheriff, "BCSheriffPeds", "BCSOVehicles", "BCS ","LimitedSidearms","LimitedLongGuns", "LSSD Deputy") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 10 },


            new Agency("~r~", "LSSD-ASD", "Los Santos Sheriffs Department - Air Support Division", "Red", Classification.Sheriff, "SheriffPeds", "SheriffHeliVehicles", "ASD ","HeliSidearms","HeliLongGuns", "LSSD Deputy") { MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 13  },


            new Agency("~b~", "GSPD", "Grapeseed Police Department", "Blue", Classification.Police, "StandardCops", "UnmarkedVehicles", "GS ","LimitedSidearms","LimitedLongGuns", "GSPD Officer") { MaxWantedLevelSpawn = 3, HeadDataGroupID = "AllHeads", Division = 14  },

            new Agency("~p~", "LSPA", "Port Authority of Los Santos", "LightGray", Classification.Police, "SecurityPeds", "UnmarkedVehicles", "LSPA ","LimitedSidearms","LimitedLongGuns", "Port Authority Officer") {MaxWantedLevelSpawn = 3, SpawnLimit = 3, HeadDataGroupID = "AllHeads",Division = 15  },
            new Agency("~p~", "LSIAPD", "Los Santos International Airport Police Department", "LightBlue", Classification.Police, "LSIAPDPeds", "LSIAPDVehicles", "LSA ","AllSidearms","AllLongGuns", "LSIAPD Officer") { MaxWantedLevelSpawn = 3, SpawnLimit = 3, HeadDataGroupID = "AllHeads", Division = 16  },

            new Agency("~r~", "NOOSE", "National Office of Security Enforcement", "DarkSlateGray", Classification.Federal, "NOOSEPeds", "NOOSEVehicles", "","BestSidearms","BestLongGuns", "NOOSE Officer") { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 5,CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads" },
            new Agency("~p~", "FIB", "Federal Investigation Bureau", "Purple", Classification.Federal, "FIBPeds", "FIBVehicles", "FIB ","BestSidearms","BestLongGuns", "FIB Agent") { MaxWantedLevelSpawn = 5, SpawnLimit = 6,CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads"  },
            new Agency("~p~", "DOA", "Drug Observation Agency", "Purple", Classification.Federal, "DOAPeds", "UnmarkedVehicles", "DOA ","AllSidearms","AllLongGuns", "DOA Agent")  {MaxWantedLevelSpawn = 3, SpawnLimit = 4,CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads"  },

            new Agency("~y~", "SAHP", "San Andreas Highway Patrol", "Yellow", Classification.State, "SAHPPeds", "SAHPVehicles", "HP ","LimitedSidearms","LimitedLongGuns", "SAHP Officer") { MaxWantedLevelSpawn = 3, SpawnsOnHighway = true, HeadDataGroupID = "AllHeads"  },
            new Agency("~o~", "SASPA", "San Andreas State Prison Authority", "Orange", Classification.State, "PrisonPeds", "PrisonVehicles", "SASPA ","AllSidearms","AllLongGuns", "SASPA Officer") { MaxWantedLevelSpawn = 3, SpawnLimit = 2, HeadDataGroupID = "AllHeads"  },
            new Agency("~g~", "SAPR", "San Andreas Park Ranger", "Green", Classification.State, "ParkRangers", "ParkRangerVehicles", "","AllSidearms","AllLongGuns", "SA Park Ranger") { MaxWantedLevelSpawn = 3, SpawnLimit = 3, HeadDataGroupID = "AllHeads" },
            new Agency("~o~", "SACG", "San Andreas Coast Guard", "DarkOrange", Classification.State, "CoastGuardPeds", "CoastGuardVehicles", "SACG ","LimitedSidearms","LimitedLongGuns", "Coast Guard Officer"){ MaxWantedLevelSpawn = 3,SpawnLimit = 3, HeadDataGroupID = "AllHeads"  },

            new Agency("~u~", "ARMY", "Army", "Black", Classification.Military, "MilitaryPeds", "ArmyVehicles", "","MilitarySidearms","MilitaryLongGuns", "Soldier") { MinWantedLevelSpawn = 6,CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads"  },

            new Agency("~r~", "LSFD", "Los Santos Fire Department", "Red", Classification.Fire, "Firefighters", "Firetrucks", "LSFD ",null, null, "LSFD Firefighter") { MaxWantedLevelSpawn = 0, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads"  },
            new Agency("~w~", "LSMC", "Los Santos Medical Center", "White", Classification.EMS, "EMTs", "Amublance1", "LSMC ",null, null, "LSMC EMT") { MaxWantedLevelSpawn = 0, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads"  },
            new Agency("~w~", "MRH", "Mission Row Hospital", "White", Classification.EMS, "EMTs", "Amublance2", "MRH ",null, null, "MRH Officer") { MaxWantedLevelSpawn = 0, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads"  },
            new Agency("~w~", "LSFD", "Los Santos Fire Department", "White", Classification.EMS, "EMTs", "Amublance3", "LSFD ",null, null, "LSFD EMT") { MaxWantedLevelSpawn = 0, CanSpawnAnywhere = true, HeadDataGroupID = "AllHeads"  },
            new Agency("~s~", "UNK", "Unknown Agency", "White", Classification.Other, null, null, "",null,null,"Officer") { MaxWantedLevelSpawn = 0 },
        };
        Directory.CreateDirectory("Plugins\\LosSantosRED\\AlternateConfigs");
        Serialization.SerializeParams(SimpleAgenicesList, "Plugins\\LosSantosRED\\AlternateConfigs\\Agencies_Gresk.xml");
    }

    public void Setup(IHeads heads, IDispatchableVehicles dispatchableVehicles, IDispatchablePeople dispatchablePeople, IIssuableWeapons issuableWeapons)
    {
        foreach(Agency agency in AgenciesList)
        {
            //EntryPoint.WriteToConsole($"AGENCY NAME {agency.FullName} LongGunsID {agency.LongGunsID} SideArmsID {agency.SideArmsID} PersonnelID {agency.PersonnelID} VehiclesID {agency.VehiclesID} HeadDataGroupID {agency.HeadDataGroupID}");
            agency.LongGuns = issuableWeapons.GetWeaponData(agency.LongGunsID);
            agency.SideArms = issuableWeapons.GetWeaponData(agency.SideArmsID);
            agency.Personnel = dispatchablePeople.GetPersonData(agency.PersonnelID);
            agency.Vehicles = dispatchableVehicles.GetVehicleData(agency.VehiclesID);
            agency.PossibleHeads = heads.GetHeadData(agency.HeadDataGroupID);
        }
    }

}