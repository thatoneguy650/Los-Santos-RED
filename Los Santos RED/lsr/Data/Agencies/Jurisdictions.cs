using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Jurisdictions : IJurisdictions
{
    private IAgencies AgencyProvider;
    private readonly string ZoneConfigFileName = "Plugins\\LosSantosRED\\ZoneJurisdictions.xml";
    private readonly string CountyConfigFileName = "Plugins\\LosSantosRED\\CountyJurisdictions.xml";
    private List<ZoneJurisdiction> ZoneJurisdictionsList = new List<ZoneJurisdiction>();
    private List<CountyJurisdiction> CountyJurisdictionList = new List<CountyJurisdiction>();


    public List<ZoneJurisdiction> ZoneJurisdictions => ZoneJurisdictionsList;
    public Jurisdictions(IAgencies agencyProvider)
    {
        AgencyProvider = agencyProvider;
    }
    public void ReadConfig()
    {

        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ZoneFile = LSRDirectory.GetFiles("ZoneJurisdictions*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ZoneFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Zone Jurisdictions config: {ZoneFile.FullName}",0);
            ZoneJurisdictionsList = Serialization.DeserializeParams<ZoneJurisdiction>(ZoneFile.FullName);
        }
        else if (File.Exists(ZoneConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Zone Jurisdictions config  {ZoneConfigFileName}",0);
            ZoneJurisdictionsList = Serialization.DeserializeParams<ZoneJurisdiction>(ZoneConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Zone Jurisdiction config found, creating default", 0);
            DefaultZoneConfig_FullExpandedJurisdiction();
            DefaultZoneConfig_LosSantos2008();
            DefaultZoneConfig_LibertyCity();
            DefaultZoneConfig_SunshineDream();
            DefaultZoneConfig_Simple();
            DefaultZoneConfig();
            DefaultZoneConfig_LibertyPP();
        }


        FileInfo CountyFile = LSRDirectory.GetFiles("CountyJurisdictions*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (CountyFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded County Jurisdictions config: {CountyFile.FullName}",0);
            CountyJurisdictionList = Serialization.DeserializeParams<CountyJurisdiction>(CountyFile.FullName);
        }
        else if (File.Exists(CountyConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded County Jurisdictions config  {CountyConfigFileName}",0);
            CountyJurisdictionList = Serialization.DeserializeParams<CountyJurisdiction>(CountyConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No County Jurisdiction config found, creating default", 0);
            DefaultCountyConfig_FullExpandedJurisdiction();
            DefaultCountyConfig_LosSantos2008();
            DefaultCountyConfig_LibertyCity();
            DefaultCountyConfig_SunshineDream();
            DefaultCountyConfig_Simple();
            DefaultCountyConfig();
            DefaultCountyConfig_LibertyPP();
        }
    }
    public Agency GetMainAgency(string ZoneName, ResponseType responseType)
    {
        if (ZoneJurisdictionsList.Any())
        {
            //ZoneJurisdiction cool = ZoneJurisdictionsList.Where(x => x.ZoneInternalGameName.ToLower() == ZoneName.ToLower()).OrderBy(y => y.Priority).FirstOrDefault();
            //if (cool != null)
            //{
            //    return AgencyProvider.GetAgency(cool.AgencyInitials);
            //}
            foreach (ZoneJurisdiction zoneJurisdiction in ZoneJurisdictionsList.Where(x => x.ZoneInternalGameName.ToLower() == ZoneName.ToLower()).OrderBy(x=>x.Priority))
            {
                Agency Agency = AgencyProvider.GetAgency(zoneJurisdiction.AgencyID);
                if (Agency != null && Agency.ResponseType == responseType)
                {
                    return Agency;
                }
            }
            return null;
        }
        return null;
    }
    public Agency GetNthAgency(string ZoneName, ResponseType responseType, int itemNumber)
    {
        if (ZoneJurisdictionsList.Any())
        {
            foreach (ZoneJurisdiction zoneJurisdiction in ZoneJurisdictionsList.Where(x => x.ZoneInternalGameName.ToLower() == ZoneName.ToLower()).Skip(itemNumber - 1).OrderBy(x => x.Priority))
            {
                Agency Agency = AgencyProvider.GetAgency(zoneJurisdiction.AgencyID);
                if (Agency != null && Agency.ResponseType == responseType)
                {
                    return Agency;
                }
            }
            return null;
        }
        return null;
    }
    public Agency GetRandomAgency(string ZoneName, int WantedLevel, ResponseType responseType)
    {
        if (ZoneJurisdictionsList.Any())
        {
            List<ZoneJurisdiction> ToPickFrom = new List<ZoneJurisdiction>();
            foreach (ZoneJurisdiction zoneJurisdiction in ZoneJurisdictionsList.Where(x => x.ZoneInternalGameName.ToLower() == ZoneName.ToLower()))
            {
                Agency Agency = AgencyProvider.GetAgency(zoneJurisdiction.AgencyID);
                if (Agency != null && Agency.CanSpawn(WantedLevel) && Agency.ResponseType == responseType)
                {
                    ToPickFrom.Add(zoneJurisdiction);
                }
            }
            int Total = ToPickFrom.Sum(x => x.CurrentSpawnChance(WantedLevel));
            int RandomPick = RandomItems.MyRand.Next(0, Total);
            foreach (ZoneJurisdiction MyJurisdiction in ToPickFrom)
            {
                int SpawnChance = MyJurisdiction.CurrentSpawnChance(WantedLevel);
                if (RandomPick < SpawnChance)
                {
                    return AgencyProvider.GetAgency(MyJurisdiction.AgencyID);
                }
                RandomPick -= SpawnChance;
            }
        }
        return null;
    }
    public Agency GetRandomCountyAgency(string countyID, int WantedLevel, ResponseType responseType)//was zone, instead take county
    {
        List<CountyJurisdiction> ToPickFrom = new List<CountyJurisdiction>();
        foreach (CountyJurisdiction countyJurisdiction in CountyJurisdictionList.Where(x => x.CountyID == countyID))
        {
            Agency Agency = AgencyProvider.GetAgency(countyJurisdiction.AgencyInitials);
            if (Agency != null && Agency.CanSpawn(WantedLevel) && Agency.ResponseType == responseType)
            {
                ToPickFrom.Add(countyJurisdiction);
            }
        }
        int Total = ToPickFrom.Sum(x => x.CurrentSpawnChance(WantedLevel));
        int RandomPick = RandomItems.MyRand.Next(0, Total);
        foreach (CountyJurisdiction MyJurisdiction in ToPickFrom)
        {
            int SpawnChance = MyJurisdiction.CurrentSpawnChance(WantedLevel);
            if (RandomPick < SpawnChance)
            {
                return AgencyProvider.GetAgency(MyJurisdiction.AgencyInitials);
            }
            RandomPick -= SpawnChance;
        }
        return null;
    }
    public Agency GetRespondingAgency(string zoneInternalGameName, string countyID, ResponseType responseType)
    {
        Agency toReturn = null;
        if(!string.IsNullOrEmpty(zoneInternalGameName))
        {
            toReturn = GetMainAgency(zoneInternalGameName, responseType);

        }
        if (toReturn == null)
        {
            toReturn = GetRandomCountyAgency(countyID, 0, responseType);
        }
        return toReturn;
    }
    public ZoneJurisdiction GetJurisdiction(string zoneInternalGameName, Agency agency)
    {
        if (agency == null || string.IsNullOrEmpty(zoneInternalGameName))
        {
            return null;
        }
        return ZoneJurisdictionsList.Where(x => x.ZoneInternalGameName.ToLower() == zoneInternalGameName.ToLower() && x.AgencyID == agency.ID).FirstOrDefault();
    }
    public List<Agency> GetAgencies(string zoneName, int wantedLevel, ResponseType responseType)
    {
        if (ZoneJurisdictionsList.Any())
        {
            List<Agency> ToReturn = new List<Agency>();
            foreach (ZoneJurisdiction zoneJurisdiction in ZoneJurisdictionsList.Where(x => x.ZoneInternalGameName.ToLower() == zoneName.ToLower()).OrderBy(k => k.CurrentSpawnChance(wantedLevel)))
            {
                Agency Agency = AgencyProvider.GetAgency(zoneJurisdiction.AgencyID);
                if (Agency != null && Agency.CanSpawn(wantedLevel) && Agency.ResponseType == responseType)
                {
                    ToReturn.Add(Agency);
                }
            }
            if (!ToReturn.Any())
            {
                return null;
            }
            return ToReturn;
        }
        else
        {
            return null;
        }
    }
    public bool CanSpawnAmbientPedestrians(string zoneInternalGameName, Agency agency)
    {
        if(agency == null || string.IsNullOrEmpty(zoneInternalGameName))
        {
            return false;
        }
        return ZoneJurisdictionsList.Where(x => x.ZoneInternalGameName.ToLower() == zoneInternalGameName.ToLower() && x.AgencyID == agency.ID && x.CanSpawnPedestrianOfficers).Any();
    }
    public string TestString()
    {
        return "OH YEAH!";
    }
    private void DefaultZoneConfig()
    {
        ZoneJurisdictionsList = new List<ZoneJurisdiction>()
        {
            new ZoneJurisdiction("LSIAPD","AIRP", 0, 100, 100) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSPD","AIRP", 1, 5, 5) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSMC","AIRP", 0, 100, 100),

            new ZoneJurisdiction("LSSD-MJ","ALAMO", 0, 100, 100),

            new ZoneJurisdiction("LSPD-VW","ALTA", 0, 100, 100) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("ARMY","ARMYB", 0, 100, 100),

            new ZoneJurisdiction("LSSD","BANHAMC", 0, 100, 100),

            new ZoneJurisdiction("LSPD","BANNING", 0, 100, 100),
            new ZoneJurisdiction("LSMC","BANNING", 0, 100, 100),

            new ZoneJurisdiction("LSSD-VW","BAYTRE", 0, 100, 100),

            new ZoneJurisdiction("LSPD-DP","BEACH", 0, 80, 100),
            new ZoneJurisdiction("LSLFG","BEACH",1, 20, 0) { CanSpawnPedestrianOfficers = true, CanSpawnBicycleOfficers = true, },

            new ZoneJurisdiction("LSSD","BHAMCA", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","BRADP", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","BRADT", 0, 100, 100),

            new ZoneJurisdiction("LSPD-RH","BURTON", 0, 100, 100) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSSD-BC","CALAFB", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","CANNY", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","CCREAK", 0, 100, 100),

            new ZoneJurisdiction("LSPD","CHAMH", 0, 100, 100),
            new ZoneJurisdiction("LSMC","CHAMH", 0, 100, 100),

            new ZoneJurisdiction("LSSD-VW","CHIL", 0, 100, 100),

            new ZoneJurisdiction("LSSD","CHU", 0, 100, 100),

            new ZoneJurisdiction("SAPR","CMSW", 0, 51, 35),
            new ZoneJurisdiction("LSSD-BC","CMSW", 1, 49, 65),

            new ZoneJurisdiction("LSPD-ELS","CYPRE", 0, 100, 100),
            new ZoneJurisdiction("LSMC","CYPRE", 0, 100, 100),

            new ZoneJurisdiction("LSSD","DAVIS", 0, 100, 100),

            new ZoneJurisdiction("LSPD-DP","DELBE", 0, 80, 100),
            new ZoneJurisdiction("LSLFG","DELBE",1, 20, 0) { CanSpawnPedestrianOfficers = true, CanSpawnBicycleOfficers = true, },

            new ZoneJurisdiction("LSPD-DP","DELPE", 0, 100, 100) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSPD-DP","DELSOL", 0, 100, 100),

            new ZoneJurisdiction("LSSD-MJ","DESRT", 0, 100, 100),

            new ZoneJurisdiction("LSPD","DOWNT", 0, 100, 100),
            new ZoneJurisdiction("MRH","DOWNT", 0, 100, 100),

            new ZoneJurisdiction("LSPD-VW","DTVINE", 0, 85, 70),
            new ZoneJurisdiction("LSSD-VW","DTVINE", 1, 15, 30),

            new ZoneJurisdiction("LSPD-VW","EAST_V", 0, 85, 70),
            new ZoneJurisdiction("LSSD-VW","EAST_V", 1, 15, 30),

            new ZoneJurisdiction("LSPD-ELS","EBURO", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","ELGORL", 0, 100, 100),

            new ZoneJurisdiction("LSPP","ELYSIAN", 0, 99, 99),
            new ZoneJurisdiction("LSPD","ELYSIAN", 1, 1, 1),
            new ZoneJurisdiction("LSMC","ELYSIAN", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","GALFISH", 0, 100, 100),

            new ZoneJurisdiction("LSPD-RH","GOLF", 0, 100, 100),

            new ZoneJurisdiction("LSSD-MJ","GRAPES", 0, 100, 100),

            new ZoneJurisdiction("LSSD-VW","GREATC", 0, 100, 100),

            new ZoneJurisdiction("LSSD-MJ","HARMO", 0, 100, 100),

            new ZoneJurisdiction("LSPD-VW","HAWICK", 0, 85, 70) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSSD-VW","HAWICK", 1, 15, 30) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSPD-VW","HORS", 0, 85, 70),
            new ZoneJurisdiction("LSSD-VW","HORS", 1, 15, 30),

            new ZoneJurisdiction("LSSD-MJ","HUMLAB", 0, 100, 100),

            new ZoneJurisdiction("SASPA","JAIL", 0, 100, 100),

            new ZoneJurisdiction("LSPD-RH","KOREAT", 0, 100, 100),

            new ZoneJurisdiction("LSSD","LACT", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","LAGO", 0, 100, 100),

            new ZoneJurisdiction("LSSD","LDAM", 0, 100, 100),

            new ZoneJurisdiction("LSPD","LEGSQU", 0, 100, 100),
            new ZoneJurisdiction("MRH","LEGSQU", 0, 100, 100),

            new ZoneJurisdiction("LSPD-ELS","LMESA", 0, 100, 100) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("MRH","LMESA", 0, 100, 100),

            new ZoneJurisdiction("LSPD-DP","LOSPUER", 0, 100, 100),
            new ZoneJurisdiction("LSMC","LOSPUER", 0, 100, 100),

            new ZoneJurisdiction("LSPD-ELS","MIRR", 0, 100, 100),
            new ZoneJurisdiction("MRH","MIRR", 0, 100, 100),

            new ZoneJurisdiction("LSPD-RH","MORN", 0, 100, 100),

            new ZoneJurisdiction("LSPD-RH","MOVIE", 0, 100, 100),

            new ZoneJurisdiction("SAPR","MTCHIL", 0, 51, 35),
            new ZoneJurisdiction("LSSD-BC","MTCHIL", 1, 49, 65),

            new ZoneJurisdiction("SAPR","MTGORDO", 0, 51, 35),
            new ZoneJurisdiction("LSSD-BC","MTGORDO", 1, 49, 65),

            new ZoneJurisdiction("LSSD-BC","MTJOSE", 0, 100, 100),

            new ZoneJurisdiction("LSPD-ELS","MURRI", 0, 100, 100),
            new ZoneJurisdiction("MRH","MURRI", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","NCHU", 0, 100, 100),

            new ZoneJurisdiction("LSSD","NOOSE", 0, 100, 100),

            new ZoneJurisdiction("USCG","OCEANA", 0, 95, 80),
            new ZoneJurisdiction("LSPD","OCEANA", 1, 5, 20),

            new ZoneJurisdiction("LSSD-BC","PALCOV", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","PALETO", 0, 100, 100) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSSD-BC","PALFOR", 0, 100, 100),

            new ZoneJurisdiction("LSSD","PALHIGH", 0, 100, 100),

            new ZoneJurisdiction("LSSD","PALMPOW", 0, 100, 100),

            new ZoneJurisdiction("LSPD-DP","PBLUFF", 0, 100, 100),

            new ZoneJurisdiction("LSPD","PBOX", 0, 100, 100) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSMC","PBOX", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","PROCOB", 0, 100, 100),

            new ZoneJurisdiction("LSPD","RANCHO", 0, 100, 100),
            new ZoneJurisdiction("LSMC","RANCHO", 0, 100, 100),

            new ZoneJurisdiction("LSSD-VW","RGLEN", 0, 100, 100),

            new ZoneJurisdiction("LSPD-RH","RICHM", 0, 100, 100),

            new ZoneJurisdiction("LSPD-RH","ROCKF", 0, 100, 100) { CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSSD-MJ","RTRAK", 0, 100, 100),

            new ZoneJurisdiction("LSSD","SANAND", 0, 100, 100),

            new ZoneJurisdiction("SAPR","SANCHIA", 0, 51, 35),
            new ZoneJurisdiction("LSSD-MJ","SANCHIA", 1, 49, 65),

            new ZoneJurisdiction("LSSD-MJ","SANDY", 0, 100, 100),

            new ZoneJurisdiction("LSPD","SKID", 0, 100, 100),
            new ZoneJurisdiction("MRH","SKID", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","SLAB", 0, 100, 100),

            new ZoneJurisdiction("LSPD","STAD", 0, 100, 100),
            new ZoneJurisdiction("LSMC","STAD", 0, 100, 100),

            new ZoneJurisdiction("LSPD","STRAW", 0, 100, 100),
            new ZoneJurisdiction("LSMC","STRAW", 0, 100, 100),

            new ZoneJurisdiction("LSSD","TATAMO", 0, 100, 100),

            new ZoneJurisdiction("LSPP","TERMINA", 0, 95, 80),
            new ZoneJurisdiction("LSPD","TERMINA", 1, 5, 20),

            new ZoneJurisdiction("LSPD","TEXTI", 0, 100, 100),
            new ZoneJurisdiction("MRH","TEXTI", 0, 100, 100),

            new ZoneJurisdiction("LSSD","TONGVAH", 0, 100, 100),

            new ZoneJurisdiction("LSSD-VW","TONGVAV", 0, 100, 100),

            new ZoneJurisdiction("LSPD-DP","VCANA", 0, 100, 100) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSPD-DP","VESP", 0, 85, 70) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSLFG","VESP",1, 20, 0) { CanSpawnPedestrianOfficers = true, CanSpawnBicycleOfficers = true, },

            new ZoneJurisdiction("LSPD-VW","VINE", 0, 85, 70),
            new ZoneJurisdiction("LSSD-VW","VINE", 1, 15, 30),

            new ZoneJurisdiction("LSSD","WINDF", 0, 100, 100),

            new ZoneJurisdiction("LSPD-VW","WVINE", 0, 85, 70),
            new ZoneJurisdiction("LSSD-VW","WVINE", 1, 15, 30),

            new ZoneJurisdiction("SAPR","ZANCUDO", 0, 51, 35),
            new ZoneJurisdiction("LSSD-BC","ZANCUDO", 1, 49, 65),

            new ZoneJurisdiction("LSPP","ZP_ORT", 0, 95, 80),
            new ZoneJurisdiction("LSPD","ZP_ORT", 1, 5, 20),

            new ZoneJurisdiction("LSSD-MJ","ZQ_UAR", 0, 85, 75),
            new ZoneJurisdiction("LSSD","ZQ_UAR", 1, 15, 25),

            new ZoneJurisdiction("LSSD-BC","GALLI", 0, 100, 100),
            ////Custom
            //new ZoneJurisdiction("APD", "CHI1", 0, 100, 100),
            new ZoneJurisdiction("NYSP", "PROL", 0, 100, 100),
            //new ZoneJurisdiction("VCPD", "VICE", 0, 100, 100),
        };
        Serialization.SerializeParams(ZoneJurisdictionsList, ZoneConfigFileName);
    }
    private void DefaultCountyConfig()
    {
        CountyJurisdictionList = new List<CountyJurisdiction>()
        {
            //Police
            new CountyJurisdiction("LSPD-ASD",StaticStrings.CityOfLosSantosCountyID, 0, 100, 100),
            new CountyJurisdiction("LSSD-ASD",StaticStrings.BlaineCountyID, 0, 100, 100),
            new CountyJurisdiction("LSSD-ASD",StaticStrings.MajesticCountyID, 0, 100, 100),
            new CountyJurisdiction("LSSD-ASD",StaticStrings.LosSantosCountyID, 0, 100, 100),
            new CountyJurisdiction("NYSP", StaticStrings.NorthYanktonCountyID, 0, 100, 100),

            //EMS
            new CountyJurisdiction("LSFD-EMS",StaticStrings.CityOfLosSantosCountyID, 0, 100, 100),
            new CountyJurisdiction("LSFD-EMS",StaticStrings.BlaineCountyID, 0, 100, 100),
            new CountyJurisdiction("LSFD-EMS",StaticStrings.MajesticCountyID, 0, 100, 100),
            new CountyJurisdiction("LSFD-EMS",StaticStrings.LosSantosCountyID, 0, 100, 100),

            //Fire
            new CountyJurisdiction("LSFD",StaticStrings.CityOfLosSantosCountyID, 0, 100, 100),
            new CountyJurisdiction("LSFD",StaticStrings.BlaineCountyID, 0, 100, 100),
            new CountyJurisdiction("LSFD",StaticStrings.MajesticCountyID, 0, 100, 100),
            new CountyJurisdiction("LSFD",StaticStrings.LosSantosCountyID, 0, 100, 100),
        };
        Serialization.SerializeParams(CountyJurisdictionList, CountyConfigFileName);
    }
    private void DefaultZoneConfig_FullExpandedJurisdiction()
    {
        List<ZoneJurisdiction> SimpleZoneJurisdictionsList = new List<ZoneJurisdiction>()
        {
            new ZoneJurisdiction("LSIAPD","AIRP", 0, 100, 100) { CanSpawnPedestrianOfficers = true, CanSpawnBicycleOfficers = true, },
            new ZoneJurisdiction("LSMC","AIRP", 0, 100, 100),

            new ZoneJurisdiction("LSSD-MJ","ALAMO", 0, 100, 100),

            new ZoneJurisdiction("LSPD-VW","ALTA", 0, 100, 100) {CanSpawnPedestrianOfficers = true, CanSpawnBicycleOfficers = true, },

            new ZoneJurisdiction("ARMY","ARMYB", 0, 100, 100),

            new ZoneJurisdiction("LSSD","BANHAMC", 0, 100, 100),

            new ZoneJurisdiction("LSPD","BANNING", 0, 100, 100),
            new ZoneJurisdiction("LSMC","BANNING", 0, 100, 100),

            new ZoneJurisdiction("LSSD-VW","BAYTRE", 0, 100, 100),

            new ZoneJurisdiction("DPPD","BEACH", 0, 80, 100) { CanSpawnPedestrianOfficers = true, CanSpawnBicycleOfficers = true, },
            new ZoneJurisdiction("LSLFG","BEACH",1, 20, 0) { CanSpawnPedestrianOfficers = true, CanSpawnBicycleOfficers = true, },

            new ZoneJurisdiction("LSSD","BHAMCA", 0, 100, 100),

            new ZoneJurisdiction("BCSO","BRADP", 0, 100, 100),

            new ZoneJurisdiction("BCSO","BRADT", 0, 100, 100),

            new ZoneJurisdiction("RHPD","BURTON", 0, 100, 100) { CanSpawnPedestrianOfficers = true, CanSpawnBicycleOfficers = true, },

            new ZoneJurisdiction("BCSO","CALAFB", 0, 100, 100),
    
            new ZoneJurisdiction("SADFW","CANNY", 0, 51, 51),
            new ZoneJurisdiction("BCSO","CANNY", 1, 49, 49),

            new ZoneJurisdiction("SADFW","CCREAK", 0, 51, 51),
            new ZoneJurisdiction("BCSO","CCREAK", 1, 49, 49),

            new ZoneJurisdiction("LSPD","CHAMH", 0, 100, 100),//CHamberlain
            new ZoneJurisdiction("LSMC","CHAMH", 0, 100, 100),

            new ZoneJurisdiction("LSSD-VW","CHIL", 0, 100, 100),

            new ZoneJurisdiction("LSSD","CHU", 0, 100, 100) { CanSpawnBicycleOfficers = true, },

            new ZoneJurisdiction("SADFW","CMSW", 0, 51, 35),
            new ZoneJurisdiction("BCSO","CMSW", 1, 49, 65),

            new ZoneJurisdiction("LSPD-ELS","CYPRE", 0, 100, 100),
            new ZoneJurisdiction("LSMC","CYPRE", 0, 100, 100),

            new ZoneJurisdiction("LSSD-DV","DAVIS", 0, 100, 100),
            new ZoneJurisdiction("LSMC","DAVIS", 0, 100, 100),

            new ZoneJurisdiction("DPPD","DELBE", 0, 80, 100),
            new ZoneJurisdiction("LSLFG","DELBE",1, 20, 0) { CanSpawnPedestrianOfficers = true, CanSpawnBicycleOfficers = true, },

            new ZoneJurisdiction("DPPD","DELPE", 0, 100, 100) { CanSpawnPedestrianOfficers = true, CanSpawnBicycleOfficers = true, },

            new ZoneJurisdiction("DPPD","DELSOL", 0, 100, 100),

            new ZoneJurisdiction("LSSD-MJ","DESRT", 0, 75, 75),
            new ZoneJurisdiction("NOOSE-BP","DESRT", 1, 25, 25),

            new ZoneJurisdiction("LSPD","DOWNT", 0, 100, 100),
            new ZoneJurisdiction("MRH","DOWNT", 0, 100, 100),

            new ZoneJurisdiction("LSPD-VW","DTVINE", 0, 100, 100),

            new ZoneJurisdiction("LSPD-VW","EAST_V", 0, 100, 100),

            new ZoneJurisdiction("LSPD-ELS","EBURO", 0, 100, 100),

            new ZoneJurisdiction("BCSO","ELGORL", 0, 100, 100),

            new ZoneJurisdiction("LSPP","ELYSIAN", 0, 100, 100),
            new ZoneJurisdiction("LSMC","ELYSIAN", 0, 100, 100),

            new ZoneJurisdiction("BCSO","GALFISH", 0, 100, 100),

            new ZoneJurisdiction("RHPD","GOLF", 0, 100, 100),

            new ZoneJurisdiction("LSSD-MJ","GRAPES", 0, 100, 100),

            new ZoneJurisdiction("LSSD-VW","GREATC", 0, 100, 100),

            new ZoneJurisdiction("LSSD-MJ","HARMO", 0, 100, 100),

            new ZoneJurisdiction("LSPD-VW","HAWICK", 0, 100, 100) { CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSPD-VW","HORS", 0, 100, 100),

            new ZoneJurisdiction("LSSD-MJ","HUMLAB", 0, 100, 100),

            new ZoneJurisdiction("SASPA","JAIL", 0, 100, 100),

            new ZoneJurisdiction("RHPD","KOREAT", 0, 100, 100),

            new ZoneJurisdiction("LSSD","LACT", 0, 100, 100),

            new ZoneJurisdiction("BCSO","LAGO", 0, 100, 100),

            new ZoneJurisdiction("LSSD","LDAM", 0, 100, 100),

            new ZoneJurisdiction("LSPD","LEGSQU", 0, 100, 100),
            new ZoneJurisdiction("MRH","LEGSQU", 0, 100, 100),

            new ZoneJurisdiction("LSPD-ELS","LMESA", 0, 100, 100) { CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("MRH","LMESA", 0, 100, 100),

            new ZoneJurisdiction("DPPD","LOSPUER", 0, 100, 100),
            new ZoneJurisdiction("LSMC","LOSPUER", 0, 100, 100),

            new ZoneJurisdiction("LSPD-ELS","MIRR", 0, 100, 100),
            new ZoneJurisdiction("MRH","MIRR", 0, 100, 100),

            new ZoneJurisdiction("RHPD","MORN", 0, 100, 100),

            new ZoneJurisdiction("RHPD","MOVIE", 0, 100, 100) { CanSpawnBicycleOfficers = true, },

            new ZoneJurisdiction("USNPS","MTCHIL", 0, 100, 100),

            new ZoneJurisdiction("SAPR","MTGORDO", 0, 100, 100),

            new ZoneJurisdiction("USNPS","MTJOSE", 0, 51, 51),//added
            new ZoneJurisdiction("BCSO","MTJOSE", 1, 49, 49),

            new ZoneJurisdiction("LSPD-ELS","MURRI", 0, 100, 100),
            new ZoneJurisdiction("MRH","MURRI", 0, 100, 100),

            new ZoneJurisdiction("BCSO","NCHU", 0, 100, 100),

            new ZoneJurisdiction("LSSD","NOOSE", 0, 100, 100),

            new ZoneJurisdiction("USCG","OCEANA", 0, 100, 100),

            new ZoneJurisdiction("BCSO","PALCOV", 0, 100, 100),

            new ZoneJurisdiction("BCSO","PALETO", 0, 100, 100) { CanSpawnPedestrianOfficers = true, CanSpawnBicycleOfficers = true, },

            new ZoneJurisdiction("BCSO","PALFOR", 0, 100, 100),

            new ZoneJurisdiction("LSDPR","PALHIGH", 0, 51, 51),
            new ZoneJurisdiction("LSSD","PALHIGH", 1, 49, 49),//new ZoneJurisdiction("LSSD","PALHIGH", 0, 100, 100),

            new ZoneJurisdiction("LSSD","PALMPOW", 0, 100, 100),

            new ZoneJurisdiction("DPPD","PBLUFF", 0, 100, 100) { CanSpawnBicycleOfficers = true, },

            new ZoneJurisdiction("LSPD","PBOX", 0, 100, 100) { CanSpawnPedestrianOfficers = true, CanSpawnBicycleOfficers = true, },
            new ZoneJurisdiction("LSMC","PBOX", 0, 100, 100),

            new ZoneJurisdiction("BCSO","PROCOB", 0, 100, 100),

            new ZoneJurisdiction("LSPD","RANCHO", 0, 100, 100),
            new ZoneJurisdiction("LSMC","RANCHO", 0, 100, 100),

            new ZoneJurisdiction("LSSD-VW","RGLEN", 0, 100, 100),

            new ZoneJurisdiction("RHPD","RICHM", 0, 100, 100),

            new ZoneJurisdiction("RHPD","ROCKF", 0, 100, 100) { CanSpawnPedestrianOfficers = true, CanSpawnBicycleOfficers = true, },

            new ZoneJurisdiction("LSSD-MJ","RTRAK", 0, 100, 100),

            new ZoneJurisdiction("LSSD","SANAND", 0, 100, 100),

            new ZoneJurisdiction("SAPR","SANCHIA", 0, 100, 100),

            new ZoneJurisdiction("LSSD-MJ","SANDY", 0, 100, 100) { CanSpawnBicycleOfficers = true, },

            new ZoneJurisdiction("LSPD","SKID", 0, 100, 100),//Mission Row
            new ZoneJurisdiction("MRH","SKID", 0, 100, 100),

            new ZoneJurisdiction("BCSO","SLAB", 0, 100, 100),

            new ZoneJurisdiction("LSPD","STAD", 0, 100, 100),
            new ZoneJurisdiction("LSMC","STAD", 0, 100, 100),

            new ZoneJurisdiction("LSPD","STRAW", 0, 100, 100),
            new ZoneJurisdiction("LSMC","STRAW", 0, 100, 100),

            new ZoneJurisdiction("LSDPR","TATAMO", 0, 100, 100),

            new ZoneJurisdiction("LSPP","TERMINA", 0, 100, 100),

            new ZoneJurisdiction("LSPD","TEXTI", 0, 100, 100),//Textile City
            new ZoneJurisdiction("MRH","TEXTI", 0, 100, 100),

            new ZoneJurisdiction("LSSD","TONGVAH", 0, 100, 100),

            new ZoneJurisdiction("LSSD-VW","TONGVAV", 0, 100, 100),

            new ZoneJurisdiction("DPPD","VCANA", 0, 100, 100) { CanSpawnPedestrianOfficers = true, CanSpawnBicycleOfficers = true, },

            new ZoneJurisdiction("DPPD","VESP", 0, 80, 100) { CanSpawnPedestrianOfficers = true, CanSpawnBicycleOfficers = true, },
            new ZoneJurisdiction("LSLFG","VESP", 1, 20, 0) { CanSpawnPedestrianOfficers = true, CanSpawnBicycleOfficers = true, },

            new ZoneJurisdiction("LSPD-VW","VINE", 0, 100, 100),

            new ZoneJurisdiction("LSSD","WINDF", 0, 100, 100),

            new ZoneJurisdiction("LSPD-VW","WVINE", 0, 100, 100),

            new ZoneJurisdiction("USNPS","ZANCUDO", 0, 51, 51),

            new ZoneJurisdiction("LSPP","ZP_ORT", 0, 100, 100),

            new ZoneJurisdiction("LSSD-MJ","ZQ_UAR", 0, 100, 100),

            new ZoneJurisdiction("BCSO","GALLI", 0, 100, 100),

            ////Custom
            //new ZoneJurisdiction("APD", "CHI1", 0, 100, 100),
            new ZoneJurisdiction("NYSP", "PROL", 0, 100, 100),
            //new ZoneJurisdiction("VCPD", "VICE", 0, 100, 100),      
        };
        Serialization.SerializeParams(SimpleZoneJurisdictionsList, "Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedJurisdiction\\Variations\\Full\\ZoneJurisdictions_FullExpandedJurisdiction.xml");
        Serialization.SerializeParams(SimpleZoneJurisdictionsList, "Plugins\\LosSantosRED\\AlternateConfigs\\EUP\\ZoneJurisdictions_EUP.xml");
    }
    private void DefaultCountyConfig_FullExpandedJurisdiction()
    {
        List<CountyJurisdiction> SimpleCountyJurisdictionList = new List<CountyJurisdiction>()
        {
            new CountyJurisdiction("LSPD-ASD",StaticStrings.CityOfLosSantosCountyID, 0, 100, 100),
            new CountyJurisdiction("BCSO",StaticStrings.BlaineCountyID, 0, 100, 100),//NEW WAS LSSD ASD
            new CountyJurisdiction("LSSD-ASD",StaticStrings.MajesticCountyID, 0, 100, 100),
            new CountyJurisdiction("LSSD-ASD",StaticStrings.LosSantosCountyID, 0, 100, 100),
            new CountyJurisdiction("NYSP", StaticStrings.NorthYanktonCountyID, 0, 100, 100),

            //EMS
            new CountyJurisdiction("LSCoFD-EMS",StaticStrings.LosSantosCountyID,0,100,100),
            new CountyJurisdiction("LSCoFD-EMS",StaticStrings.MajesticCountyID, 0, 100, 100),
            new CountyJurisdiction("BCFD-EMS",StaticStrings.BlaineCountyID,0,100,100),
            new CountyJurisdiction("LSFD-EMS",StaticStrings.CityOfLosSantosCountyID, 0, 100, 100),

            //Fire
            new CountyJurisdiction("LSCoFD",StaticStrings.LosSantosCountyID,0,100,100),
            new CountyJurisdiction("LSCoFD",StaticStrings.MajesticCountyID,0,100,100),
            new CountyJurisdiction("BCFD",StaticStrings.BlaineCountyID,0,100,100),
            new CountyJurisdiction("LSFD",StaticStrings.CityOfLosSantosCountyID, 0, 100, 100),
        };
        Serialization.SerializeParams(SimpleCountyJurisdictionList, "Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedJurisdiction\\Variations\\Full\\CountyJurisdictions_FullExpandedJurisdiction.xml");
        Serialization.SerializeParams(SimpleCountyJurisdictionList, "Plugins\\LosSantosRED\\AlternateConfigs\\EUP\\CountyJurisdictions_EUP.xml");

    }
    private void DefaultZoneConfig_LosSantos2008()
    {
        List<ZoneJurisdiction> ZoneJurisdictionsList2008 = new List<ZoneJurisdiction>()
        {
            new ZoneJurisdiction("LSPD","AIRP", 0, 100, 100) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSMC","AIRP", 0, 100, 100),

            new ZoneJurisdiction("LSSD","ALAMO", 0, 100, 100),

            new ZoneJurisdiction("LSPD","ALTA", 0, 100, 100) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("ARMY","ARMYB", 0, 100, 100),

            new ZoneJurisdiction("LSSD","BANHAMC", 0, 100, 100),

            new ZoneJurisdiction("LSPD","BANNING", 0, 100, 100),
            new ZoneJurisdiction("LSMC","BANNING", 0, 100, 100),

            new ZoneJurisdiction("LSSD","BAYTRE", 0, 100, 100),

            new ZoneJurisdiction("LSPD","BEACH", 0, 80, 100),
            new ZoneJurisdiction("LSLFG","BEACH",1, 20, 0) { CanSpawnPedestrianOfficers = true, CanSpawnBicycleOfficers = true, },

            new ZoneJurisdiction("LSSD","BHAMCA", 0, 100, 100),

            new ZoneJurisdiction("LSSD","BRADP", 0, 100, 100),

            new ZoneJurisdiction("LSSD","BRADT", 0, 100, 100),

            new ZoneJurisdiction("LSPD","BURTON", 0, 100, 100) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSSD","CALAFB", 0, 100, 100),

            new ZoneJurisdiction("LSSD","CANNY", 0, 100, 100),

            new ZoneJurisdiction("LSSD","CCREAK", 0, 100, 100),

            new ZoneJurisdiction("LSPD","CHAMH", 0, 100, 100),
            new ZoneJurisdiction("LSMC","CHAMH", 0, 100, 100),

            new ZoneJurisdiction("LSSD","CHIL", 0, 100, 100),

            new ZoneJurisdiction("LSSD","CHU", 0, 100, 100),

            new ZoneJurisdiction("SAPR","CMSW", 0, 51, 35),
            new ZoneJurisdiction("LSSD","CMSW", 1, 49, 65),

            new ZoneJurisdiction("LSPD","CYPRE", 0, 100, 100),
            new ZoneJurisdiction("LSMC","CYPRE", 0, 100, 100),

            new ZoneJurisdiction("LSSD","DAVIS", 0, 100, 100),

            new ZoneJurisdiction("LSPD","DELBE", 0, 80, 100),
            new ZoneJurisdiction("LSLFG","DELBE",1, 20, 0) { CanSpawnPedestrianOfficers = true, CanSpawnBicycleOfficers = true, },

            new ZoneJurisdiction("LSPD","DELPE", 0, 100, 100) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSPD","DELSOL", 0, 100, 100),

            new ZoneJurisdiction("LSSD","DESRT", 0, 100, 100),

            new ZoneJurisdiction("LSPD","DOWNT", 0, 100, 100),
            new ZoneJurisdiction("MRH","DOWNT", 0, 100, 100),

            new ZoneJurisdiction("LSPD","DTVINE", 0, 100, 100),

            new ZoneJurisdiction("LSPD","EAST_V", 0, 100, 100),

            new ZoneJurisdiction("LSPD","EBURO", 0, 100, 100),

            new ZoneJurisdiction("LSSD","ELGORL", 0, 100, 100),

            new ZoneJurisdiction("LSPD","ELYSIAN", 0, 95, 80),
            new ZoneJurisdiction("LSMC","ELYSIAN", 0, 100, 100),

            new ZoneJurisdiction("LSSD","GALFISH", 0, 100, 100),

            new ZoneJurisdiction("LSPD","GOLF", 0, 100, 100),

            new ZoneJurisdiction("LSSD","GRAPES", 0, 100, 100),

            new ZoneJurisdiction("LSSD","GREATC", 0, 100, 100),

            new ZoneJurisdiction("LSSD","HARMO", 0, 100, 100),

            new ZoneJurisdiction("LSPD","HAWICK", 0, 100, 100) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSPD","HORS", 0, 100, 100),

            new ZoneJurisdiction("LSSD","HUMLAB", 0, 100, 100),

            new ZoneJurisdiction("SASPA","JAIL", 0, 95, 70),
            new ZoneJurisdiction("LSSD","JAIL", 1, 5, 30),

            new ZoneJurisdiction("LSPD","KOREAT", 0, 100, 100),

            new ZoneJurisdiction("LSSD","LACT", 0, 100, 100),

            new ZoneJurisdiction("LSSD","LAGO", 0, 100, 100),

            new ZoneJurisdiction("LSSD","LDAM", 0, 100, 100),

            new ZoneJurisdiction("LSPD","LEGSQU", 0, 100, 100),
            new ZoneJurisdiction("MRH","LEGSQU", 0, 100, 100),

            new ZoneJurisdiction("LSPD","LMESA", 0, 100, 100) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("MRH","LMESA", 0, 100, 100),

            new ZoneJurisdiction("LSPD","LOSPUER", 0, 100, 100),
            new ZoneJurisdiction("LSMC","LOSPUER", 0, 100, 100),

            new ZoneJurisdiction("LSPD","MIRR", 0, 100, 100),
            new ZoneJurisdiction("MRH","MIRR", 0, 100, 100),

            new ZoneJurisdiction("LSPD","MORN", 0, 100, 100),

            new ZoneJurisdiction("LSPD-RH","MOVIE", 0, 100, 100),

            new ZoneJurisdiction("SAPR","MTCHIL", 0, 51, 35),
            new ZoneJurisdiction("LSSD","MTCHIL", 1, 49, 65),

            new ZoneJurisdiction("SAPR","MTGORDO", 0, 51, 35),
            new ZoneJurisdiction("LSSD","MTGORDO", 1, 49, 65),

            new ZoneJurisdiction("LSSD","MTJOSE", 0, 100, 100),

            new ZoneJurisdiction("LSPD","MURRI", 0, 100, 100),
            new ZoneJurisdiction("MRH","MURRI", 0, 100, 100),

            new ZoneJurisdiction("LSSD","NCHU", 0, 100, 100),

            new ZoneJurisdiction("LSSD","NOOSE", 0, 100, 100),

            new ZoneJurisdiction("USCG","OCEANA", 0, 95, 80),
            new ZoneJurisdiction("LSPD","OCEANA", 1, 5, 20),

            new ZoneJurisdiction("LSSD","PALCOV", 0, 100, 100),

            new ZoneJurisdiction("LSSD","PALETO", 0, 100, 100) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSSD","PALFOR", 0, 100, 100),

            new ZoneJurisdiction("LSSD","PALHIGH", 0, 100, 100),

            new ZoneJurisdiction("LSSD","PALMPOW", 0, 100, 100),

            new ZoneJurisdiction("LSPD","PBLUFF", 0, 100, 100),

            new ZoneJurisdiction("LSPD","PBOX", 0, 100, 100) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSMC","PBOX", 0, 100, 100),

            new ZoneJurisdiction("LSSD","PROCOB", 0, 100, 100),

            new ZoneJurisdiction("LSPD","RANCHO", 0, 100, 100),
            new ZoneJurisdiction("LSMC","RANCHO", 0, 100, 100),

            new ZoneJurisdiction("LSSD","RGLEN", 0, 100, 100),

            new ZoneJurisdiction("LSPD","RICHM", 0, 100, 100),

            new ZoneJurisdiction("LSPD","ROCKF", 0, 100, 100) { CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSSD","RTRAK", 0, 100, 100),

            new ZoneJurisdiction("LSSD","SANAND", 0, 100, 100),

            new ZoneJurisdiction("SAPR","SANCHIA", 0, 51, 35),
            new ZoneJurisdiction("LSSD","SANCHIA", 1, 49, 65),

            new ZoneJurisdiction("LSSD","SANDY", 0, 100, 100),

            new ZoneJurisdiction("LSPD","SKID", 0, 100, 100),
            new ZoneJurisdiction("MRH","SKID", 0, 100, 100),

            new ZoneJurisdiction("LSSD","SLAB", 0, 100, 100),

            new ZoneJurisdiction("LSPD","STAD", 0, 100, 100),
            new ZoneJurisdiction("LSMC","STAD", 0, 100, 100),

            new ZoneJurisdiction("LSPD","STRAW", 0, 100, 100),
            new ZoneJurisdiction("LSMC","STRAW", 0, 100, 100),

            new ZoneJurisdiction("LSSD","TATAMO", 0, 100, 100),

            new ZoneJurisdiction("LSPD","TERMINA", 0, 100, 100),

            new ZoneJurisdiction("LSPD","TEXTI", 0, 100, 100),
            new ZoneJurisdiction("MRH","TEXTI", 0, 100, 100),

            new ZoneJurisdiction("LSSD","TONGVAH", 0, 100, 100),

            new ZoneJurisdiction("LSSD","TONGVAV", 0, 100, 100),

            new ZoneJurisdiction("LSPD","VCANA", 0, 100, 100) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSPD","VESP", 0, 80, 100) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSLFG","VESP", 1, 20, 0) { CanSpawnPedestrianOfficers = true, CanSpawnBicycleOfficers = true, },

            new ZoneJurisdiction("LSPD-VW","VINE", 0, 100, 100),

            new ZoneJurisdiction("LSSD","WINDF", 0, 100, 100),

            new ZoneJurisdiction("LSPD","WVINE", 0, 100, 100),

            new ZoneJurisdiction("SAPR","ZANCUDO", 0, 51, 35),
            new ZoneJurisdiction("LSSD","ZANCUDO", 1, 49, 65),

            new ZoneJurisdiction("LSPD","ZP_ORT", 0, 100, 100),

            new ZoneJurisdiction("LSSD","ZQ_UAR", 0, 100, 100),

            new ZoneJurisdiction("LSSD","GALLI", 0, 100, 100),

            new ZoneJurisdiction("NYSP", "PROL", 0, 100, 100),
        };
        Serialization.SerializeParams(ZoneJurisdictionsList2008, "Plugins\\LosSantosRED\\AlternateConfigs\\LosSantos2008\\ZoneJurisdictions_LosSantos2008.xml");
    }
    private void DefaultCountyConfig_LosSantos2008()
    {
        List<CountyJurisdiction> CountyJurisdictionList2008 = new List<CountyJurisdiction>()
        {
            //Police
            new CountyJurisdiction("LSPD-ASD",StaticStrings.CityOfLosSantosCountyID, 0, 100, 100),
            new CountyJurisdiction("LSSD-ASD",StaticStrings.BlaineCountyID, 0, 100, 100),
            new CountyJurisdiction("LSSD-ASD",StaticStrings.MajesticCountyID, 0, 100, 100),
            new CountyJurisdiction("LSSD-ASD",StaticStrings.LosSantosCountyID, 0, 100, 100),
            new CountyJurisdiction("NYSP", StaticStrings.NorthYanktonCountyID, 0, 100, 100),

            //EMS
            new CountyJurisdiction("LSFD-EMS",StaticStrings.CityOfLosSantosCountyID, 0, 100, 100),
            new CountyJurisdiction("LSFD-EMS",StaticStrings.BlaineCountyID, 0, 100, 100),
            new CountyJurisdiction("LSFD-EMS",StaticStrings.MajesticCountyID, 0, 100, 100),
            new CountyJurisdiction("LSFD-EMS",StaticStrings.LosSantosCountyID, 0, 100, 100),

            //Fire
            new CountyJurisdiction("LSFD",StaticStrings.CityOfLosSantosCountyID, 0, 100, 100),
            new CountyJurisdiction("LSFD",StaticStrings.BlaineCountyID, 0, 100, 100),
            new CountyJurisdiction("LSFD",StaticStrings.MajesticCountyID, 0, 100, 100),
            new CountyJurisdiction("LSFD",StaticStrings.LosSantosCountyID, 0, 100, 100),
        };
        Serialization.SerializeParams(CountyJurisdictionList2008, "Plugins\\LosSantosRED\\AlternateConfigs\\LosSantos2008\\CountyJurisdictions_LosSantos2008.xml");
    }
    private void DefaultZoneConfig_LibertyCity()
    {
        List<ZoneJurisdiction> ZoneJurisdictionsListLibertyCity = new List<ZoneJurisdiction>()
        {
            
            
            new ZoneJurisdiction("LCPD","BOAB", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            new ZoneJurisdiction("LCPD","BOULE", 0, 100, 100),
            new ZoneJurisdiction("LCPD","BRALG", 0, 100, 100),
            new ZoneJurisdiction("LCPD","BRDBB", 0, 100, 100),
            new ZoneJurisdiction("LCPD","BREBB", 0, 100, 100),
            new ZoneJurisdiction("LCPD","BRBRO", 0, 100, 100),
            new ZoneJurisdiction("LCPD","BEECW", 0, 100, 100),
            new ZoneJurisdiction("LCPD","BOTU", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            new ZoneJurisdiction("LCPD","CHITO", 0, 100, 100),
            new ZoneJurisdiction("LCPD","CITH", 0, 100, 100),
            new ZoneJurisdiction("LCPD","COISL", 0, 100, 100),
            new ZoneJurisdiction("LCPD","CHISL", 0, 100, 100),
            new ZoneJurisdiction("LCPD","CASGR", 0, 100, 100),
            new ZoneJurisdiction("LCPD","CHAPO", 0, 100, 100),
            new ZoneJurisdiction("LCPD","CASGC", 0, 100, 100),
            new ZoneJurisdiction("LCPD","CERHE", 0, 100, 100),
            new ZoneJurisdiction("LCPD","DOWTW", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            new ZoneJurisdiction("LCPD","EAHOL", 0, 100, 100),
            new ZoneJurisdiction("LCPD","EISLC", 0, 100, 100),
            new ZoneJurisdiction("LCPD","FISSO", 0, 100, 100),
            new ZoneJurisdiction("LCPD","FRANI", 0, 100, 100),
            new ZoneJurisdiction("LCPD","FISSN", 0, 100, 100),
            new ZoneJurisdiction("LCPD","FIREP", 0, 100, 100),
            new ZoneJurisdiction("LCPD","FORSI", 0, 100, 100),
            new ZoneJurisdiction("LCPD","HATGA", 0, 100, 100),
            new ZoneJurisdiction("LCPD","HOBEH", 0, 100, 100),
            new ZoneJurisdiction("LCPD","INSTI", 0, 100, 100),
            new ZoneJurisdiction("LCPD","LANCE", 0, 100, 100),
            
            new ZoneJurisdiction("LCPD","LTBAY", 0, 100, 100),
            new ZoneJurisdiction("LCPD","LANCA", 0, 100, 100),
            new ZoneJurisdiction("LCPD","LOWEA", 0, 100, 100),
            new ZoneJurisdiction("LCPD","LITAL", 0, 100, 100),
            new ZoneJurisdiction("LCPD","MIDPE", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            new ZoneJurisdiction("LCPD","MIDPA", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            new ZoneJurisdiction("LCPD","MIDPW", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            new ZoneJurisdiction("LCPD","MEADP", 0, 100, 100),
            new ZoneJurisdiction("LCPD","MEADH", 0, 100, 100),
            new ZoneJurisdiction("LCPD","NOHOL", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            new ZoneJurisdiction("LCPD","NORWO", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            new ZoneJurisdiction("LCPD","NRTGA", 0, 100, 100),
            new ZoneJurisdiction("LCPD","NOWOB", 0, 100, 100),
            //new ZoneJurisdiction("LCPD","OCEANA", 0, 100, 100),

            new ZoneJurisdiction("USCG","OCEANA", 0, 95, 80),
            new ZoneJurisdiction("LCPD","OCEANA", 1, 5, 20),


            new ZoneJurisdiction("LCPD","OUTL", 0, 100, 100),
            new ZoneJurisdiction("LCPD","PUGAT", 0, 100, 100),

            new ZoneJurisdiction("LCPD","SANAND", 0, 100, 100),
            new ZoneJurisdiction("LCPD","SUTHS", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            new ZoneJurisdiction("LCPD","SCHOL", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            new ZoneJurisdiction("LCPD","STARJ", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            new ZoneJurisdiction("LCPD","STEIN", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            new ZoneJurisdiction("LCPD","STHBO", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            new ZoneJurisdiction("LCPD","SUFFO", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            
            new ZoneJurisdiction("LCPD","THPRES", 0, 100, 100),
            new ZoneJurisdiction("LCPD","THXCH", 0, 100, 100),
            new ZoneJurisdiction("LCPD","THTRI", 0, 100, 100),
            new ZoneJurisdiction("LCPD","TMEQU", 0, 100, 100),
            new ZoneJurisdiction("LCPD","VASIH", 0, 100, 100),
            new ZoneJurisdiction("LCPD","WESMI", 0, 100, 100),
            new ZoneJurisdiction("LCPD","WESDI", 0, 100, 100),

            new ZoneJurisdiction("ASP","ALSCF", 0, 100, 100),
            new ZoneJurisdiction("ASP","ACTRR", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            new ZoneJurisdiction("ASP","ALDCI", 0, 100, 100),
            new ZoneJurisdiction("ASP","ACTIP", 0, 100, 100),
            new ZoneJurisdiction("ASP","BERCH", 0, 100, 100),
            new ZoneJurisdiction("ASP","LEFWO", 0, 100, 100),
            new ZoneJurisdiction("ASP","NORMY", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            new ZoneJurisdiction("ASP","PORTU", 0, 100, 100),
            new ZoneJurisdiction("ASP","TUDOR", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            new ZoneJurisdiction("ASP","WESDY", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },

        };
        Serialization.SerializeParams(ZoneJurisdictionsListLibertyCity, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LibertyConfigFolder}\\ZoneJurisdictions_{StaticStrings.LibertyConfigSuffix}.xml");
    }
    private void DefaultCountyConfig_LibertyCity()
    {
        List<CountyJurisdiction> CountyJurisdictionList2008 = new List<CountyJurisdiction>()
        {
            new CountyJurisdiction("LCPD-ASD",StaticStrings.LibertyCityCountyID, 0, 100, 100),
            new CountyJurisdiction("ASP-ASD",StaticStrings.AlderneyCountyID, 0, 100, 100),


            new CountyJurisdiction("FDLC-EMS",StaticStrings.LibertyCityCountyID, 0, 100, 100),
            new CountyJurisdiction("FDLC-EMS",StaticStrings.AlderneyCountyID, 0, 100, 100),

            new CountyJurisdiction("FDLC",StaticStrings.LibertyCityCountyID, 0, 100, 100),
            new CountyJurisdiction("FDLC",StaticStrings.AlderneyCountyID, 0, 100, 100),


            new CountyJurisdiction("NYSP", StaticStrings.NorthYanktonCountyID, 0, 100, 100),
        };
        Serialization.SerializeParams(CountyJurisdictionList2008, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LibertyConfigFolder}\\CountyJurisdictions_{StaticStrings.LibertyConfigSuffix}.xml");
    }


    private void DefaultZoneConfig_LibertyPP()
    {
        List<ZoneJurisdiction> ZoneJurisdictionsListLibertyCity = new List<ZoneJurisdiction>()
        {
            new ZoneJurisdiction("LSIAPD","AIRP", 0, 100, 100) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSPD","AIRP", 1, 5, 5) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSMC","AIRP", 0, 100, 100),

            new ZoneJurisdiction("LSSD-MJ","ALAMO", 0, 100, 100),

            new ZoneJurisdiction("LSPD-VW","ALTA", 0, 100, 100) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("ARMY","ARMYB", 0, 100, 100),

            new ZoneJurisdiction("LSSD","BANHAMC", 0, 100, 100),

            new ZoneJurisdiction("LSPD","BANNING", 0, 100, 100),
            new ZoneJurisdiction("LSMC","BANNING", 0, 100, 100),

            new ZoneJurisdiction("LSSD-VW","BAYTRE", 0, 100, 100),

            new ZoneJurisdiction("LSPD-DP","BEACH", 0, 80, 100),
            new ZoneJurisdiction("LSLFG","BEACH",1, 20, 0) { CanSpawnPedestrianOfficers = true, CanSpawnBicycleOfficers = true, },

            new ZoneJurisdiction("LSSD","BHAMCA", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","BRADP", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","BRADT", 0, 100, 100),

            new ZoneJurisdiction("LSPD-RH","BURTON", 0, 100, 100) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSSD-BC","CALAFB", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","CANNY", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","CCREAK", 0, 100, 100),

            new ZoneJurisdiction("LSPD","CHAMH", 0, 100, 100),
            new ZoneJurisdiction("LSMC","CHAMH", 0, 100, 100),

            new ZoneJurisdiction("LSSD-VW","CHIL", 0, 100, 100),

            new ZoneJurisdiction("LSSD","CHU", 0, 100, 100),

            new ZoneJurisdiction("SAPR","CMSW", 0, 51, 35),
            new ZoneJurisdiction("LSSD-BC","CMSW", 1, 49, 65),

            new ZoneJurisdiction("LSPD-ELS","CYPRE", 0, 100, 100),
            new ZoneJurisdiction("LSMC","CYPRE", 0, 100, 100),

            new ZoneJurisdiction("LSSD","DAVIS", 0, 100, 100),

            new ZoneJurisdiction("LSPD-DP","DELBE", 0, 80, 100),
            new ZoneJurisdiction("LSLFG","DELBE",1, 20, 0) { CanSpawnPedestrianOfficers = true, CanSpawnBicycleOfficers = true, },

            new ZoneJurisdiction("LSPD-DP","DELPE", 0, 100, 100) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSPD-DP","DELSOL", 0, 100, 100),

            new ZoneJurisdiction("LSSD-MJ","DESRT", 0, 100, 100),

            new ZoneJurisdiction("LSPD","DOWNT", 0, 100, 100),
            new ZoneJurisdiction("MRH","DOWNT", 0, 100, 100),

            new ZoneJurisdiction("LSPD-VW","DTVINE", 0, 85, 70),
            new ZoneJurisdiction("LSSD-VW","DTVINE", 1, 15, 30),

            new ZoneJurisdiction("LSPD-VW","EAST_V", 0, 85, 70),
            new ZoneJurisdiction("LSSD-VW","EAST_V", 1, 15, 30),

            new ZoneJurisdiction("LSPD-ELS","EBURO", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","ELGORL", 0, 100, 100),

            new ZoneJurisdiction("LSPP","ELYSIAN", 0, 99, 99),
            new ZoneJurisdiction("LSPD","ELYSIAN", 1, 1, 1),
            new ZoneJurisdiction("LSMC","ELYSIAN", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","GALFISH", 0, 100, 100),

            new ZoneJurisdiction("LSPD-RH","GOLF", 0, 100, 100),

            new ZoneJurisdiction("LSSD-MJ","GRAPES", 0, 100, 100),

            new ZoneJurisdiction("LSSD-VW","GREATC", 0, 100, 100),

            new ZoneJurisdiction("LSSD-MJ","HARMO", 0, 100, 100),

            new ZoneJurisdiction("LSPD-VW","HAWICK", 0, 85, 70) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSSD-VW","HAWICK", 1, 15, 30) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSPD-VW","HORS", 0, 85, 70),
            new ZoneJurisdiction("LSSD-VW","HORS", 1, 15, 30),

            new ZoneJurisdiction("LSSD-MJ","HUMLAB", 0, 100, 100),

            new ZoneJurisdiction("SASPA","JAIL", 0, 100, 100),

            new ZoneJurisdiction("LSPD-RH","KOREAT", 0, 100, 100),

            new ZoneJurisdiction("LSSD","LACT", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","LAGO", 0, 100, 100),

            new ZoneJurisdiction("LSSD","LDAM", 0, 100, 100),

            new ZoneJurisdiction("LSPD","LEGSQU", 0, 100, 100),
            new ZoneJurisdiction("MRH","LEGSQU", 0, 100, 100),

            new ZoneJurisdiction("LSPD-ELS","LMESA", 0, 100, 100) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("MRH","LMESA", 0, 100, 100),

            new ZoneJurisdiction("LSPD-DP","LOSPUER", 0, 100, 100),
            new ZoneJurisdiction("LSMC","LOSPUER", 0, 100, 100),

            new ZoneJurisdiction("LSPD-ELS","MIRR", 0, 100, 100),
            new ZoneJurisdiction("MRH","MIRR", 0, 100, 100),

            new ZoneJurisdiction("LSPD-RH","MORN", 0, 100, 100),

            new ZoneJurisdiction("LSPD-RH","MOVIE", 0, 100, 100),

            new ZoneJurisdiction("SAPR","MTCHIL", 0, 51, 35),
            new ZoneJurisdiction("LSSD-BC","MTCHIL", 1, 49, 65),

            new ZoneJurisdiction("SAPR","MTGORDO", 0, 51, 35),
            new ZoneJurisdiction("LSSD-BC","MTGORDO", 1, 49, 65),

            new ZoneJurisdiction("LSSD-BC","MTJOSE", 0, 100, 100),

            new ZoneJurisdiction("LSPD-ELS","MURRI", 0, 100, 100),
            new ZoneJurisdiction("MRH","MURRI", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","NCHU", 0, 100, 100),

            new ZoneJurisdiction("LSSD","NOOSE", 0, 100, 100),

            new ZoneJurisdiction("USCG","OCEANA", 0, 95, 80),
            new ZoneJurisdiction("LSPD","OCEANA", 1, 5, 20),

            new ZoneJurisdiction("LSSD-BC","PALCOV", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","PALETO", 0, 100, 100) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSSD-BC","PALFOR", 0, 100, 100),

            new ZoneJurisdiction("LSSD","PALHIGH", 0, 100, 100),

            new ZoneJurisdiction("LSSD","PALMPOW", 0, 100, 100),

            new ZoneJurisdiction("LSPD-DP","PBLUFF", 0, 100, 100),

            new ZoneJurisdiction("LSPD","PBOX", 0, 100, 100) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSMC","PBOX", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","PROCOB", 0, 100, 100),

            new ZoneJurisdiction("LSPD","RANCHO", 0, 100, 100),
            new ZoneJurisdiction("LSMC","RANCHO", 0, 100, 100),

            new ZoneJurisdiction("LSSD-VW","RGLEN", 0, 100, 100),

            new ZoneJurisdiction("LSPD-RH","RICHM", 0, 100, 100),

            new ZoneJurisdiction("LSPD-RH","ROCKF", 0, 100, 100) { CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSSD-MJ","RTRAK", 0, 100, 100),

            new ZoneJurisdiction("LSSD","SANAND", 0, 100, 100),

            new ZoneJurisdiction("SAPR","SANCHIA", 0, 51, 35),
            new ZoneJurisdiction("LSSD-MJ","SANCHIA", 1, 49, 65),

            new ZoneJurisdiction("LSSD-MJ","SANDY", 0, 100, 100),

            new ZoneJurisdiction("LSPD","SKID", 0, 100, 100),
            new ZoneJurisdiction("MRH","SKID", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","SLAB", 0, 100, 100),

            new ZoneJurisdiction("LSPD","STAD", 0, 100, 100),
            new ZoneJurisdiction("LSMC","STAD", 0, 100, 100),

            new ZoneJurisdiction("LSPD","STRAW", 0, 100, 100),
            new ZoneJurisdiction("LSMC","STRAW", 0, 100, 100),

            new ZoneJurisdiction("LSSD","TATAMO", 0, 100, 100),

            new ZoneJurisdiction("LSPP","TERMINA", 0, 95, 80),
            new ZoneJurisdiction("LSPD","TERMINA", 1, 5, 20),

            new ZoneJurisdiction("LSPD","TEXTI", 0, 100, 100),
            new ZoneJurisdiction("MRH","TEXTI", 0, 100, 100),

            new ZoneJurisdiction("LSSD","TONGVAH", 0, 100, 100),

            new ZoneJurisdiction("LSSD-VW","TONGVAV", 0, 100, 100),

            new ZoneJurisdiction("LSPD-DP","VCANA", 0, 100, 100) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSPD-DP","VESP", 0, 85, 70) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSLFG","VESP",1, 20, 0) { CanSpawnPedestrianOfficers = true, CanSpawnBicycleOfficers = true, },

            new ZoneJurisdiction("LSPD-VW","VINE", 0, 85, 70),
            new ZoneJurisdiction("LSSD-VW","VINE", 1, 15, 30),

            new ZoneJurisdiction("LSSD","WINDF", 0, 100, 100),

            new ZoneJurisdiction("LSPD-VW","WVINE", 0, 85, 70),
            new ZoneJurisdiction("LSSD-VW","WVINE", 1, 15, 30),

            new ZoneJurisdiction("SAPR","ZANCUDO", 0, 51, 35),
            new ZoneJurisdiction("LSSD-BC","ZANCUDO", 1, 49, 65),

            new ZoneJurisdiction("LSPP","ZP_ORT", 0, 95, 80),
            new ZoneJurisdiction("LSPD","ZP_ORT", 1, 5, 20),

            new ZoneJurisdiction("LSSD-MJ","ZQ_UAR", 0, 85, 75),
            new ZoneJurisdiction("LSSD","ZQ_UAR", 1, 15, 25),

            new ZoneJurisdiction("LSSD-BC","GALLI", 0, 100, 100),
            ////Custom
            //new ZoneJurisdiction("APD", "CHI1", 0, 100, 100),
            new ZoneJurisdiction("NYSP", "PROL", 0, 100, 100),

            new ZoneJurisdiction("LCPD","BOAB", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            new ZoneJurisdiction("LCPD","BOULE", 0, 100, 100),
            new ZoneJurisdiction("LCPD","BRALG", 0, 100, 100),
            new ZoneJurisdiction("LCPD","BRDBB", 0, 100, 100),
            new ZoneJurisdiction("LCPD","BREBB", 0, 100, 100),
            new ZoneJurisdiction("LCPD","BRBRO", 0, 100, 100),
            new ZoneJurisdiction("LCPD","BEECW", 0, 100, 100),
            new ZoneJurisdiction("LCPD","BOTU", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            new ZoneJurisdiction("LCPD","CHITO", 0, 100, 100),
            new ZoneJurisdiction("LCPD","CITH", 0, 100, 100),
            new ZoneJurisdiction("LCPD","COISL", 0, 100, 100),
            new ZoneJurisdiction("LCPD","CHISL", 0, 100, 100),
            new ZoneJurisdiction("LCPD","CASGR", 0, 100, 100),
            new ZoneJurisdiction("LCPD","CHAPO", 0, 100, 100),
            new ZoneJurisdiction("LCPD","CASGC", 0, 100, 100),
            new ZoneJurisdiction("LCPD","CERHE", 0, 100, 100),
            new ZoneJurisdiction("LCPD","DOWTW", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            new ZoneJurisdiction("LCPD","EAHOL", 0, 100, 100),
            new ZoneJurisdiction("LCPD","EISLC", 0, 100, 100),
            new ZoneJurisdiction("LCPD","FISSO", 0, 100, 100),
            new ZoneJurisdiction("LCPD","FRANI", 0, 100, 100),
            new ZoneJurisdiction("LCPD","FISSN", 0, 100, 100),
            new ZoneJurisdiction("LCPD","FIREP", 0, 100, 100),
            new ZoneJurisdiction("LCPD","FORSI", 0, 100, 100),
            new ZoneJurisdiction("LCPD","HATGA", 0, 100, 100),
            new ZoneJurisdiction("LCPD","HOBEH", 0, 100, 100),
            new ZoneJurisdiction("LCPD","INSTI", 0, 100, 100),
            new ZoneJurisdiction("LCPD","LANCE", 0, 100, 100),

            new ZoneJurisdiction("LCPD","LTBAY", 0, 100, 100),
            new ZoneJurisdiction("LCPD","LANCA", 0, 100, 100),
            new ZoneJurisdiction("LCPD","LOWEA", 0, 100, 100),
            new ZoneJurisdiction("LCPD","LITAL", 0, 100, 100),
            new ZoneJurisdiction("LCPD","MIDPE", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            new ZoneJurisdiction("LCPD","MIDPA", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            new ZoneJurisdiction("LCPD","MIDPW", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            new ZoneJurisdiction("LCPD","MEADP", 0, 100, 100),
            new ZoneJurisdiction("LCPD","MEADH", 0, 100, 100),
            new ZoneJurisdiction("LCPD","NOHOL", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            new ZoneJurisdiction("LCPD","NORWO", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            new ZoneJurisdiction("LCPD","NRTGA", 0, 100, 100),
            new ZoneJurisdiction("LCPD","NOWOB", 0, 100, 100),
            //new ZoneJurisdiction("LCPD","OCEANA", 0, 100, 100),

            new ZoneJurisdiction("USCG","OCEANA", 0, 95, 80),
            new ZoneJurisdiction("LCPD","OCEANA", 1, 5, 20),


            new ZoneJurisdiction("LCPD","OUTL", 0, 100, 100),
            new ZoneJurisdiction("LCPD","PUGAT", 0, 100, 100),

            new ZoneJurisdiction("LCPD","SANAND", 0, 100, 100),
            new ZoneJurisdiction("LCPD","SUTHS", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            new ZoneJurisdiction("LCPD","SCHOL", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            new ZoneJurisdiction("LCPD","STARJ", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            new ZoneJurisdiction("LCPD","STEIN", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            new ZoneJurisdiction("LCPD","STHBO", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            new ZoneJurisdiction("LCPD","SUFFO", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },

            new ZoneJurisdiction("LCPD","THPRES", 0, 100, 100),
            new ZoneJurisdiction("LCPD","THXCH", 0, 100, 100),
            new ZoneJurisdiction("LCPD","THTRI", 0, 100, 100),
            new ZoneJurisdiction("LCPD","TMEQU", 0, 100, 100),
            new ZoneJurisdiction("LCPD","VASIH", 0, 100, 100),
            new ZoneJurisdiction("LCPD","WESMI", 0, 100, 100),
            new ZoneJurisdiction("LCPD","WESDI", 0, 100, 100),

            new ZoneJurisdiction("ASP","ALSCF", 0, 100, 100),
            new ZoneJurisdiction("ASP","ACTRR", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            new ZoneJurisdiction("ASP","ALDCI", 0, 100, 100),
            new ZoneJurisdiction("ASP","ACTIP", 0, 100, 100),
            new ZoneJurisdiction("ASP","BERCH", 0, 100, 100),
            new ZoneJurisdiction("ASP","LEFWO", 0, 100, 100),
            new ZoneJurisdiction("ASP","NORMY", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            new ZoneJurisdiction("ASP","PORTU", 0, 100, 100),
            new ZoneJurisdiction("ASP","TUDOR", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },
            new ZoneJurisdiction("ASP","WESDY", 0, 100, 100) { CanSpawnPedestrianOfficers = true, },

        };
        Serialization.SerializeParams(ZoneJurisdictionsListLibertyCity, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LPPConfigFolder}\\ZoneJurisdictions_{StaticStrings.LPPConfigSuffix}.xml");
    }



    private void DefaultCountyConfig_LibertyPP()
    {
        List<CountyJurisdiction> CountyJurisdictionList2008 = new List<CountyJurisdiction>()
        {

            //Police
            new CountyJurisdiction("LSPD-ASD",StaticStrings.CityOfLosSantosCountyID, 0, 100, 100),
            new CountyJurisdiction("LSSD-ASD",StaticStrings.BlaineCountyID, 0, 100, 100),
            new CountyJurisdiction("LSSD-ASD",StaticStrings.MajesticCountyID, 0, 100, 100),
            new CountyJurisdiction("LSSD-ASD",StaticStrings.LosSantosCountyID, 0, 100, 100),
            new CountyJurisdiction("NYSP", StaticStrings.NorthYanktonCountyID, 0, 100, 100),

            //EMS
            new CountyJurisdiction("LSFD-EMS",StaticStrings.CityOfLosSantosCountyID, 0, 100, 100),
            new CountyJurisdiction("LSFD-EMS",StaticStrings.BlaineCountyID, 0, 100, 100),
            new CountyJurisdiction("LSFD-EMS",StaticStrings.MajesticCountyID, 0, 100, 100),
            new CountyJurisdiction("LSFD-EMS",StaticStrings.LosSantosCountyID, 0, 100, 100),

            //Fire
            new CountyJurisdiction("LSFD",StaticStrings.CityOfLosSantosCountyID, 0, 100, 100),
            new CountyJurisdiction("LSFD",StaticStrings.BlaineCountyID, 0, 100, 100),
            new CountyJurisdiction("LSFD",StaticStrings.MajesticCountyID, 0, 100, 100),
            new CountyJurisdiction("LSFD",StaticStrings.LosSantosCountyID, 0, 100, 100),

            new CountyJurisdiction("LCPD-ASD",StaticStrings.LibertyCityCountyID, 0, 100, 100),
            new CountyJurisdiction("ASP-ASD",StaticStrings.AlderneyCountyID, 0, 100, 100),


            new CountyJurisdiction("FDLC-EMS",StaticStrings.LibertyCityCountyID, 0, 100, 100),
            new CountyJurisdiction("FDLC-EMS",StaticStrings.AlderneyCountyID, 0, 100, 100),

            new CountyJurisdiction("FDLC",StaticStrings.LibertyCityCountyID, 0, 100, 100),
            new CountyJurisdiction("FDLC",StaticStrings.AlderneyCountyID, 0, 100, 100),


            new CountyJurisdiction("NYSP", StaticStrings.NorthYanktonCountyID, 0, 100, 100),
        };
        Serialization.SerializeParams(CountyJurisdictionList2008, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LPPConfigFolder}\\CountyJurisdictions_{StaticStrings.LPPConfigSuffix}.xml");
    }

    private void DefaultZoneConfig_SunshineDream()
    {
        List<ZoneJurisdiction> ZoneJurisdictionsListSunshineDream = new List<ZoneJurisdiction>()
        {
            new ZoneJurisdiction("VCPD","MFLATO", 0, 100, 100),
            new ZoneJurisdiction("VCPD","MFLBSB", 0, 100, 100),
            new ZoneJurisdiction("VCPD","MFLMIA", 0, 100, 100),
            new ZoneJurisdiction("VCPD","MFLMIB", 0, 100, 100),
            new ZoneJurisdiction("VCPD","MFLSPB", 0, 100, 100),
            new ZoneJurisdiction("VCPD","MFLADD", 0, 100, 100),
            new ZoneJurisdiction("VCPD","MFLBKL", 0, 100, 100),
            new ZoneJurisdiction("VCPD","MFLCCG", 0, 100, 100),
            new ZoneJurisdiction("VCPD","MFLCRW", 0, 100, 100),
            new ZoneJurisdiction("VCPD","MFLCRG", 0, 100, 100),
            new ZoneJurisdiction("VCPD","MFLMB", 0, 100, 100),
            new ZoneJurisdiction("VCPD","MFLCC", 0, 100, 100),
            new ZoneJurisdiction("VCPD","MFLFMG", 0, 100, 100),
            new ZoneJurisdiction("VCPD","MFLBSH", 0, 100, 100),
            new ZoneJurisdiction("VCPD","MFLSB", 0, 100, 100),
            new ZoneJurisdiction("VCPD","MFLBI", 0, 100, 100),
            new ZoneJurisdiction("VCPD","MFLSI", 0, 100, 100),
            new ZoneJurisdiction("VCPD","MFLWI", 0, 100, 100),
            new ZoneJurisdiction("VCPD","MFLDI", 0, 100, 100),
            new ZoneJurisdiction("VCPD","MFSMRI", 0, 100, 100),
            new ZoneJurisdiction("VCPD","MFLDLI", 0, 100, 100),
            new ZoneJurisdiction("VCPD","MFLSMI", 0, 100, 100),
            new ZoneJurisdiction("VCPD","MFLFSI", 0, 100, 100),
            new ZoneJurisdiction("VCPD","MFLFMP", 0, 100, 100),
            new ZoneJurisdiction("VCPD","MFLSPP", 0, 100, 100),
            new ZoneJurisdiction("VCPD","MFDMIA", 0, 100, 100),
            new ZoneJurisdiction("VCPD","MFOMNI", 0, 100, 100),
            new ZoneJurisdiction("VCPD","MFLPKW", 0, 100, 100),
            new ZoneJurisdiction("VCPD","MFOTWN", 1, 100, 100),
            new ZoneJurisdiction("VCPD","MFLUMP", 1, 100, 100),
            new ZoneJurisdiction("VCPD","MFLSPG", 1, 100, 100),
            new ZoneJurisdiction("VCPD","MFLHVA", 1, 100, 100),
            new ZoneJurisdiction("VCPD","MFLFMI", 1, 100, 100),


            new ZoneJurisdiction("VDPD","MFLATO",1, 100, 100),
            new ZoneJurisdiction("VDPD","MFLBSB",1, 100, 100),
            new ZoneJurisdiction("VDPD","MFLMIA",1, 100, 100),
            new ZoneJurisdiction("VDPD","MFLMIB",1, 100, 100),
            new ZoneJurisdiction("VDPD","MFLSPB",1, 100, 100),
            new ZoneJurisdiction("VDPD","MFLADD",1, 100, 100),
            new ZoneJurisdiction("VDPD","MFLBKL",1, 100, 100),
            new ZoneJurisdiction("VDPD","MFLCCG",1, 100, 100),
            new ZoneJurisdiction("VDPD","MFLCRW",1, 100, 100),
            new ZoneJurisdiction("VDPD","MFLCRG",1, 100, 100),
            new ZoneJurisdiction("VDPD","MFLMB",1, 100, 100),
            new ZoneJurisdiction("VDPD","MFLCC",1, 100, 100),
            new ZoneJurisdiction("VDPD","MFLFMG",1, 100, 100),
            new ZoneJurisdiction("VDPD","MFLBSH",1, 100, 100),
            new ZoneJurisdiction("VDPD","MFLSB",1, 100, 100),
            new ZoneJurisdiction("VDPD","MFLBI",1, 100, 100),
            new ZoneJurisdiction("VDPD","MFLSI",1, 100, 100),
            new ZoneJurisdiction("VDPD","MFLWI",1, 100, 100),
            new ZoneJurisdiction("VDPD","MFLDI",1, 100, 100),
            new ZoneJurisdiction("VDPD","MFSMRI",1, 100, 100),
            new ZoneJurisdiction("VDPD","MFLDLI",1, 100, 100),
            new ZoneJurisdiction("VDPD","MFLSMI",1, 100, 100),
            new ZoneJurisdiction("VDPD","MFLFSI",1, 100, 100),
            new ZoneJurisdiction("VDPD","MFLFMP",1, 100, 100),
            new ZoneJurisdiction("VDPD","MFLSPP",1, 100, 100),
            new ZoneJurisdiction("VDPD","MFDMIA",1, 100, 100),
            new ZoneJurisdiction("VDPD","MFOMNI",1, 100, 100),
            new ZoneJurisdiction("VDPD","MFLPKW",0, 100, 100),
            new ZoneJurisdiction("VDPD","MFOTWN",0, 100, 100),
            new ZoneJurisdiction("VDPD","MFLUMP",0, 100, 100),
            new ZoneJurisdiction("VDPD","MFLSPG",0, 100, 100),
            new ZoneJurisdiction("VDPD","MFLHVA",0, 100, 100),
            new ZoneJurisdiction("VDPD","MFLFMI",0, 100, 100),

            //Other
            new ZoneJurisdiction("VCPD","OCEANA", 0, 100, 100),
            new ZoneJurisdiction("VCPD","SANAND", 0, 100, 100),
        };
        Serialization.SerializeParams(ZoneJurisdictionsListSunshineDream, "Plugins\\LosSantosRED\\AlternateConfigs\\SunshineDream\\ZoneJurisdictions_SunshineDream.xml");
    }
    private void DefaultCountyConfig_SunshineDream()
    {
        List<CountyJurisdiction> CountyJurisdictionListSunshineDream = new List<CountyJurisdiction>()
        {
            new CountyJurisdiction("VCPD-ASD",StaticStrings.CityOfViceCountyID, 0, 100, 100),
            new CountyJurisdiction("VCPD-ASD",StaticStrings.ViceCountyID, 0, 100, 100),


            new CountyJurisdiction("FDVC",StaticStrings.CityOfViceCountyID, 0, 100, 100),
            new CountyJurisdiction("FDVC",StaticStrings.ViceCountyID, 0, 100, 100),

            new CountyJurisdiction("VCMC",StaticStrings.CityOfViceCountyID, 0, 100, 100),
            new CountyJurisdiction("VCMC",StaticStrings.ViceCountyID, 0, 100, 100),

        };
        Serialization.SerializeParams(CountyJurisdictionListSunshineDream, "Plugins\\LosSantosRED\\AlternateConfigs\\SunshineDream\\CountyJurisdictions_SunshineDream.xml");
    }

    private void DefaultZoneConfig_Simple()
    {
        List<ZoneJurisdiction> ZoneJurisdictionsListSimple = new List<ZoneJurisdiction>()
        {
            new ZoneJurisdiction("LSPD","AIRP", 0, 100, 100) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSMC","AIRP", 0, 100, 100),

            new ZoneJurisdiction("LSSD","ALAMO", 0, 100, 100),

            new ZoneJurisdiction("LSPD","ALTA", 0, 100, 100) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("ARMY","ARMYB", 0, 100, 100),

            new ZoneJurisdiction("LSSD","BANHAMC", 0, 100, 100),

            new ZoneJurisdiction("LSPD","BANNING", 0, 100, 100),
            new ZoneJurisdiction("LSMC","BANNING", 0, 100, 100),

            new ZoneJurisdiction("LSSD","BAYTRE", 0, 100, 100),

            new ZoneJurisdiction("LSPD","BEACH", 0, 80, 100),
            new ZoneJurisdiction("LSLFG","BEACH",1, 20, 0) { CanSpawnPedestrianOfficers = true, CanSpawnBicycleOfficers = true, },

            new ZoneJurisdiction("LSSD","BHAMCA", 0, 100, 100),

            new ZoneJurisdiction("LSSD","BRADP", 0, 100, 100),

            new ZoneJurisdiction("LSSD","BRADT", 0, 100, 100),

            new ZoneJurisdiction("LSPD","BURTON", 0, 100, 100) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSSD","CALAFB", 0, 100, 100),

            new ZoneJurisdiction("LSSD","CANNY", 0, 100, 100),

            new ZoneJurisdiction("LSSD","CCREAK", 0, 100, 100),

            new ZoneJurisdiction("LSPD","CHAMH", 0, 100, 100),
            new ZoneJurisdiction("LSMC","CHAMH", 0, 100, 100),

            new ZoneJurisdiction("LSSD","CHIL", 0, 100, 100),

            new ZoneJurisdiction("LSSD","CHU", 0, 100, 100),

            new ZoneJurisdiction("SAPR","CMSW", 0, 51, 35),
            new ZoneJurisdiction("LSSD","CMSW", 1, 49, 65),

            new ZoneJurisdiction("LSPD","CYPRE", 0, 100, 100),
            new ZoneJurisdiction("LSMC","CYPRE", 0, 100, 100),

            new ZoneJurisdiction("LSSD","DAVIS", 0, 100, 100),

            new ZoneJurisdiction("LSPD","DELBE", 0, 80, 100),
            new ZoneJurisdiction("LSLFG","DELBE",1, 20, 0) { CanSpawnPedestrianOfficers = true, CanSpawnBicycleOfficers = true, },

            new ZoneJurisdiction("LSPD","DELPE", 0, 100, 100) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSPD","DELSOL", 0, 100, 100),

            new ZoneJurisdiction("LSSD","DESRT", 0, 100, 100),

            new ZoneJurisdiction("LSPD","DOWNT", 0, 100, 100),
            new ZoneJurisdiction("MRH","DOWNT", 0, 100, 100),

            new ZoneJurisdiction("LSPD","DTVINE", 0, 100, 100),

            new ZoneJurisdiction("LSPD","EAST_V", 0, 100, 100),

            new ZoneJurisdiction("LSPD","EBURO", 0, 100, 100),

            new ZoneJurisdiction("LSSD","ELGORL", 0, 100, 100),

            new ZoneJurisdiction("LSPD","ELYSIAN", 0, 95, 80),
            new ZoneJurisdiction("LSMC","ELYSIAN", 0, 100, 100),

            new ZoneJurisdiction("LSSD","GALFISH", 0, 100, 100),

            new ZoneJurisdiction("LSPD","GOLF", 0, 100, 100),

            new ZoneJurisdiction("LSSD","GRAPES", 0, 100, 100),

            new ZoneJurisdiction("LSSD","GREATC", 0, 100, 100),

            new ZoneJurisdiction("LSSD","HARMO", 0, 100, 100),

            new ZoneJurisdiction("LSPD","HAWICK", 0, 100, 100) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSPD","HORS", 0, 100, 100),

            new ZoneJurisdiction("LSSD","HUMLAB", 0, 100, 100),

            new ZoneJurisdiction("SASPA","JAIL", 0, 95, 70),
            new ZoneJurisdiction("LSSD","JAIL", 1, 5, 30),

            new ZoneJurisdiction("LSPD","KOREAT", 0, 100, 100),

            new ZoneJurisdiction("LSSD","LACT", 0, 100, 100),

            new ZoneJurisdiction("LSSD","LAGO", 0, 100, 100),

            new ZoneJurisdiction("LSSD","LDAM", 0, 100, 100),

            new ZoneJurisdiction("LSPD","LEGSQU", 0, 100, 100),
            new ZoneJurisdiction("MRH","LEGSQU", 0, 100, 100),

            new ZoneJurisdiction("LSPD","LMESA", 0, 100, 100) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("MRH","LMESA", 0, 100, 100),

            new ZoneJurisdiction("LSPD","LOSPUER", 0, 100, 100),
            new ZoneJurisdiction("LSMC","LOSPUER", 0, 100, 100),

            new ZoneJurisdiction("LSPD","MIRR", 0, 100, 100),
            new ZoneJurisdiction("MRH","MIRR", 0, 100, 100),

            new ZoneJurisdiction("LSPD","MORN", 0, 100, 100),

            new ZoneJurisdiction("LSPD-RH","MOVIE", 0, 100, 100),

            new ZoneJurisdiction("SAPR","MTCHIL", 0, 51, 35),
            new ZoneJurisdiction("LSSD","MTCHIL", 1, 49, 65),

            new ZoneJurisdiction("SAPR","MTGORDO", 0, 51, 35),
            new ZoneJurisdiction("LSSD","MTGORDO", 1, 49, 65),

            new ZoneJurisdiction("LSSD","MTJOSE", 0, 100, 100),

            new ZoneJurisdiction("LSPD","MURRI", 0, 100, 100),
            new ZoneJurisdiction("MRH","MURRI", 0, 100, 100),

            new ZoneJurisdiction("LSSD","NCHU", 0, 100, 100),

            new ZoneJurisdiction("LSSD","NOOSE", 0, 100, 100),

            new ZoneJurisdiction("USCG","OCEANA", 0, 95, 80),
            new ZoneJurisdiction("LSPD","OCEANA", 1, 5, 20),

            new ZoneJurisdiction("LSSD","PALCOV", 0, 100, 100),

            new ZoneJurisdiction("LSSD","PALETO", 0, 100, 100) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSSD","PALFOR", 0, 100, 100),

            new ZoneJurisdiction("LSSD","PALHIGH", 0, 100, 100),

            new ZoneJurisdiction("LSSD","PALMPOW", 0, 100, 100),

            new ZoneJurisdiction("LSPD","PBLUFF", 0, 100, 100),

            new ZoneJurisdiction("LSPD","PBOX", 0, 100, 100) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSMC","PBOX", 0, 100, 100),

            new ZoneJurisdiction("LSSD","PROCOB", 0, 100, 100),

            new ZoneJurisdiction("LSPD","RANCHO", 0, 100, 100),
            new ZoneJurisdiction("LSMC","RANCHO", 0, 100, 100),

            new ZoneJurisdiction("LSSD","RGLEN", 0, 100, 100),

            new ZoneJurisdiction("LSPD","RICHM", 0, 100, 100),

            new ZoneJurisdiction("LSPD","ROCKF", 0, 100, 100) { CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSSD","RTRAK", 0, 100, 100),

            new ZoneJurisdiction("LSSD","SANAND", 0, 100, 100),

            new ZoneJurisdiction("SAPR","SANCHIA", 0, 51, 35),
            new ZoneJurisdiction("LSSD","SANCHIA", 1, 49, 65),

            new ZoneJurisdiction("LSSD","SANDY", 0, 100, 100),

            new ZoneJurisdiction("LSPD","SKID", 0, 100, 100),
            new ZoneJurisdiction("MRH","SKID", 0, 100, 100),

            new ZoneJurisdiction("LSSD","SLAB", 0, 100, 100),

            new ZoneJurisdiction("LSPD","STAD", 0, 100, 100),
            new ZoneJurisdiction("LSMC","STAD", 0, 100, 100),

            new ZoneJurisdiction("LSPD","STRAW", 0, 100, 100),
            new ZoneJurisdiction("LSMC","STRAW", 0, 100, 100),

            new ZoneJurisdiction("LSSD","TATAMO", 0, 100, 100),

            new ZoneJurisdiction("LSPD","TERMINA", 0, 100, 100),

            new ZoneJurisdiction("LSPD","TEXTI", 0, 100, 100),
            new ZoneJurisdiction("MRH","TEXTI", 0, 100, 100),

            new ZoneJurisdiction("LSSD","TONGVAH", 0, 100, 100),

            new ZoneJurisdiction("LSSD","TONGVAV", 0, 100, 100),

            new ZoneJurisdiction("LSPD","VCANA", 0, 100, 100) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSPD","VESP", 0, 80, 100) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSLFG","VESP", 1, 20, 0) { CanSpawnPedestrianOfficers = true, CanSpawnBicycleOfficers = true, },

            new ZoneJurisdiction("LSPD","VINE", 0, 100, 100),

            new ZoneJurisdiction("LSSD","WINDF", 0, 100, 100),

            new ZoneJurisdiction("LSPD","WVINE", 0, 100, 100),

            new ZoneJurisdiction("SAPR","ZANCUDO", 0, 51, 35),
            new ZoneJurisdiction("LSSD","ZANCUDO", 1, 49, 65),

            new ZoneJurisdiction("LSPD","ZP_ORT", 0, 100, 100),

            new ZoneJurisdiction("LSSD","ZQ_UAR", 0, 100, 100),

            new ZoneJurisdiction("LSSD","GALLI", 0, 100, 100),

            new ZoneJurisdiction("NYSP", "PROL", 0, 100, 100),
        };
        Serialization.SerializeParams(ZoneJurisdictionsListSimple, "Plugins\\LosSantosRED\\AlternateConfigs\\Simple\\ZoneJurisdictions_Simple.xml");
    }
    private void DefaultCountyConfig_Simple()
    {
        List<CountyJurisdiction> CountyJurisdictionListSimple = new List<CountyJurisdiction>()
        {
            //Police
            new CountyJurisdiction("LSPD-ASD",StaticStrings.CityOfLosSantosCountyID, 0, 100, 100),
            new CountyJurisdiction("LSSD-ASD",StaticStrings.BlaineCountyID, 0, 100, 100),
            new CountyJurisdiction("LSSD-ASD",StaticStrings.MajesticCountyID, 0, 100, 100),
            new CountyJurisdiction("LSSD-ASD",StaticStrings.LosSantosCountyID, 0, 100, 100),
            new CountyJurisdiction("NYSP", StaticStrings.NorthYanktonCountyID, 0, 100, 100),

            //EMS
            new CountyJurisdiction("LSFD-EMS",StaticStrings.CityOfLosSantosCountyID, 0, 100, 100),
            new CountyJurisdiction("LSFD-EMS",StaticStrings.BlaineCountyID, 0, 100, 100),
            new CountyJurisdiction("LSFD-EMS",StaticStrings.MajesticCountyID, 0, 100, 100),
            new CountyJurisdiction("LSFD-EMS",StaticStrings.LosSantosCountyID, 0, 100, 100),

            //Fire
            new CountyJurisdiction("LSFD",StaticStrings.CityOfLosSantosCountyID, 0, 100, 100),
            new CountyJurisdiction("LSFD",StaticStrings.BlaineCountyID, 0, 100, 100),
            new CountyJurisdiction("LSFD",StaticStrings.MajesticCountyID, 0, 100, 100),
            new CountyJurisdiction("LSFD",StaticStrings.LosSantosCountyID, 0, 100, 100),
        };
        Serialization.SerializeParams(CountyJurisdictionListSimple, "Plugins\\LosSantosRED\\AlternateConfigs\\Simple\\CountyJurisdictions_Simple.xml");
    }
}



