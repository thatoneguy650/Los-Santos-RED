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
            DefaultZoneConfig();
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
            DefaultCountyConfig();
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
                Agency Agency = AgencyProvider.GetAgency(zoneJurisdiction.AgencyInitials);
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
                Agency Agency = AgencyProvider.GetAgency(zoneJurisdiction.AgencyInitials);
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
                Agency Agency = AgencyProvider.GetAgency(zoneJurisdiction.AgencyInitials);
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
                    return AgencyProvider.GetAgency(MyJurisdiction.AgencyInitials);
                }
                RandomPick -= SpawnChance;
            }
        }
        return null;
    }
    public Agency GetRandomAgency(County county, int WantedLevel, ResponseType responseType)//was zone, instead take county
    {
        List<CountyJurisdiction> ToPickFrom = new List<CountyJurisdiction>();
        foreach (CountyJurisdiction countyJurisdiction in CountyJurisdictionList.Where(x => x.County == county))
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
    public List<Agency> GetAgencies(string zoneName, int wantedLevel, ResponseType responseType)
    {
        if (ZoneJurisdictionsList.Any())
        {
            List<Agency> ToReturn = new List<Agency>();
            foreach (ZoneJurisdiction zoneJurisdiction in ZoneJurisdictionsList.Where(x => x.ZoneInternalGameName.ToLower() == zoneName.ToLower()).OrderBy(k => k.CurrentSpawnChance(wantedLevel)))
            {
                Agency Agency = AgencyProvider.GetAgency(zoneJurisdiction.AgencyInitials);
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
    public bool CanSpawnPedestrianAtZone(string zoneName, string agencyInitials)
    {
        return ZoneJurisdictionsList.Where(x => x.ZoneInternalGameName.ToLower() == zoneName.ToLower() && x.AgencyInitials == agencyInitials).FirstOrDefault()?.CanSpawnPedestrianOfficers == true;
    }
    private void DefaultZoneConfig()
    {
        ZoneJurisdictionsList = new List<ZoneJurisdiction>()
        {
            new ZoneJurisdiction("LSIAPD","AIRP", 0, 95, 95) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSPD","AIRP", 1, 5, 5) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSFD","AIRP", 0, 100, 100),
            new ZoneJurisdiction("MRH","AIRP", 0, 100, 100),

            new ZoneJurisdiction("LSSD-MJ","ALAMO", 0, 85, 75),
            new ZoneJurisdiction("LSSD","ALAMO", 1, 15, 25),
            new ZoneJurisdiction("LSFD","ALAMO", 0, 100, 100),
            new ZoneJurisdiction("LSMC","ALAMO", 0, 100, 100),

            new ZoneJurisdiction("LSPD-VW","ALTA", 0, 100, 100) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSFD","ALTA", 0, 100, 100),
            new ZoneJurisdiction("LSMC","ALTA", 0, 100, 100),

            new ZoneJurisdiction("ARMY","ARMYB", 0, 100, 100),

            new ZoneJurisdiction("LSSD","BANHAMC", 0, 100, 100),
            new ZoneJurisdiction("LSFD","BANHAMC", 0, 100, 100),
            new ZoneJurisdiction("LSMC","BANHAMC", 0, 100, 100),

            new ZoneJurisdiction("LSPD","BANNING", 0, 100, 100),
            new ZoneJurisdiction("LSFD","BANNING", 0, 100, 100),
            new ZoneJurisdiction("LSMC","BANNING", 0, 100, 100),

            new ZoneJurisdiction("LSSD-VW","BAYTRE", 0, 100, 100),
            new ZoneJurisdiction("LSFD","BAYTRE", 0, 100, 100),
            new ZoneJurisdiction("LSMC","BAYTRE", 0, 100, 100),

            new ZoneJurisdiction("LSPD-DP","BEACH", 0, 95, 70),
            new ZoneJurisdiction("LSPD","BEACH", 1, 5, 30),
            new ZoneJurisdiction("LSFD","BEACH", 0, 100, 100),
            new ZoneJurisdiction("LSMC","BEACH", 0, 100, 100),

            new ZoneJurisdiction("LSSD","BHAMCA", 0, 100, 100),
            new ZoneJurisdiction("LSFD","BHAMCA", 0, 100, 100),
            new ZoneJurisdiction("LSMC","BHAMCA", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","BRADP", 0, 85, 75),
            new ZoneJurisdiction("LSSD","BRADP", 1, 15, 25),
            new ZoneJurisdiction("LSFD","BRADP", 0, 100, 100),
            new ZoneJurisdiction("LSMC","BRADP", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","BRADT", 0, 85, 75),
            new ZoneJurisdiction("LSSD","BRADT", 1, 15, 25),
            new ZoneJurisdiction("LSFD","BRADT", 0, 100, 100),
            new ZoneJurisdiction("LSMC","BRADT", 0, 100, 100),

            new ZoneJurisdiction("LSPD-RH","BURTON", 0, 85, 70) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSPD","BURTON", 1, 15, 30) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSFD","BURTON", 0, 100, 100),
            new ZoneJurisdiction("LSMC","BURTON", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","CALAFB", 0, 85, 75),
            new ZoneJurisdiction("LSSD","CALAFB", 1, 15, 25),
            new ZoneJurisdiction("LSFD","CALAFB", 0, 100, 100),
            new ZoneJurisdiction("LSMC","CALAFB", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","CANNY", 0, 85, 75),
            new ZoneJurisdiction("LSSD","CANNY", 1, 15, 25),
            new ZoneJurisdiction("LSFD","CANNY", 0, 100, 100),
            new ZoneJurisdiction("LSMC","CANNY", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","CCREAK", 0, 85, 75),
            new ZoneJurisdiction("LSSD","CCREAK", 1, 15, 25),
            new ZoneJurisdiction("LSFD","CCREAK", 0, 100, 100),
            new ZoneJurisdiction("LSMC","CCREAK", 0, 100, 100),

            new ZoneJurisdiction("LSPD","CHAMH", 0, 100, 100),
            new ZoneJurisdiction("LSFD","CHAMH", 0, 100, 100),
            new ZoneJurisdiction("LSMC","CHAMH", 0, 100, 100),

            new ZoneJurisdiction("LSSD-VW","CHIL", 0, 100, 100),
            new ZoneJurisdiction("LSFD","CHIL", 0, 100, 100),
            new ZoneJurisdiction("LSMC","CHIL", 0, 100, 100),

            new ZoneJurisdiction("LSSD","CHU", 0, 100, 100),
            new ZoneJurisdiction("LSFD","CHU", 0, 100, 100),
            new ZoneJurisdiction("LSMC","CHU", 0, 100, 100),

            new ZoneJurisdiction("SAPR","CMSW", 0, 51, 35),
            new ZoneJurisdiction("LSSD-BC","CMSW", 1, 49, 65),
            new ZoneJurisdiction("LSFD","CMSW", 0, 100, 100),
            new ZoneJurisdiction("LSMC","CMSW", 0, 100, 100),

            new ZoneJurisdiction("LSPD-ELS","CYPRE", 0, 85, 70),
            new ZoneJurisdiction("LSPD","CYPRE", 1, 15, 30),
            new ZoneJurisdiction("LSFD","CYPRE", 0, 100, 100),
            new ZoneJurisdiction("LSMC","CYPRE", 0, 100, 100),

            new ZoneJurisdiction("LSSD","DAVIS", 0, 100, 100),
            new ZoneJurisdiction("LSFD","DAVIS", 0, 100, 100),
            new ZoneJurisdiction("MRH","DAVIS", 0, 100, 100),

            new ZoneJurisdiction("LSPD-DP","DELBE", 0, 85, 70),
            new ZoneJurisdiction("LSPD","DELBE", 1, 15, 30),
            new ZoneJurisdiction("LSFD","DELBE", 0, 100, 100),
            new ZoneJurisdiction("LSMC","DELBE", 0, 100, 100),

            new ZoneJurisdiction("LSPD-DP","DELPE", 0, 85, 70) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSPD","DELPE", 1, 15, 30) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSFD","DELPE", 0, 100, 100),
            new ZoneJurisdiction("LSMC","DELPE", 0, 100, 100),

            new ZoneJurisdiction("LSPD-DP","DELSOL", 0, 85, 70),
            new ZoneJurisdiction("LSPD","DELSOL", 1, 15, 30),
            new ZoneJurisdiction("LSFD","DELSOL", 0, 100, 100),
            new ZoneJurisdiction("LSMC","DELSOL", 0, 100, 100),

            new ZoneJurisdiction("LSSD-MJ","DESRT", 0, 85, 75),
            new ZoneJurisdiction("LSSD","DESRT", 1, 15, 25),
            new ZoneJurisdiction("LSFD","DESRT", 0, 100, 100),
            new ZoneJurisdiction("LSMC","DESRT", 0, 100, 100),

            new ZoneJurisdiction("LSPD","DOWNT", 0, 100, 100),
            new ZoneJurisdiction("LSFD","DOWNT", 0, 100, 100),
            new ZoneJurisdiction("MRH","DOWNT", 0, 100, 100),

            new ZoneJurisdiction("LSPD-VW","DTVINE", 0, 85, 70),
            new ZoneJurisdiction("LSSD-VW","DTVINE", 1, 15, 30),
            new ZoneJurisdiction("LSFD","DTVINE", 0, 100, 100),
            new ZoneJurisdiction("LSMC","DTVINE", 0, 100, 100),

            new ZoneJurisdiction("LSPD-VW","EAST_V", 0, 85, 70),
            new ZoneJurisdiction("LSSD-VW","EAST_V", 1, 15, 30),
            new ZoneJurisdiction("LSFD","EAST_V", 0, 100, 100),
            new ZoneJurisdiction("LSMC","EAST_V", 0, 100, 100),

            new ZoneJurisdiction("LSPD-ELS","EBURO", 0, 85, 70),
            new ZoneJurisdiction("LSPD","EBURO", 1, 15, 30),
            new ZoneJurisdiction("LSFD","EBURO", 0, 100, 100),
            new ZoneJurisdiction("LSMC","EBURO", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","ELGORL", 0, 85, 75),
            new ZoneJurisdiction("LSSD","ELGORL", 1, 15, 25),
            new ZoneJurisdiction("LSFD","ELGORL", 0, 100, 100),
            new ZoneJurisdiction("LSMC","ELGORL", 0, 100, 100),

            new ZoneJurisdiction("LSPP","ELYSIAN", 0, 95, 80),
            new ZoneJurisdiction("LSPD","ELYSIAN", 1, 5, 20),
            new ZoneJurisdiction("LSFD","ELYSIAN", 0, 100, 100),
            new ZoneJurisdiction("LSMC","ELYSIAN", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","GALFISH", 0, 85, 75),
            new ZoneJurisdiction("LSSD","GALFISH", 1, 15, 25),
            new ZoneJurisdiction("LSFD","GALFISH", 0, 100, 100),
            new ZoneJurisdiction("LSMC","GALFISH", 0, 100, 100),

            new ZoneJurisdiction("LSPD-RH","GOLF", 0, 85, 70),
            new ZoneJurisdiction("LSPD","GOLF", 1, 15, 30),
            new ZoneJurisdiction("LSFD","GOLF", 0, 100, 100),
            new ZoneJurisdiction("LSMC","GOLF", 0, 100, 100),

            new ZoneJurisdiction("GSPD","GRAPES", 0, 85, 75),
            new ZoneJurisdiction("LSSD-MJ","GRAPES", 1, 15, 25),
            new ZoneJurisdiction("LSFD","GRAPES", 0, 100, 100),
            new ZoneJurisdiction("LSMC","GRAPES", 0, 100, 100),

            new ZoneJurisdiction("LSSD-VW","GREATC", 0, 100, 100),
            new ZoneJurisdiction("LSFD","GREATC", 0, 100, 100),
            new ZoneJurisdiction("LSMC","GREATC", 0, 100, 100),

            new ZoneJurisdiction("LSSD-MJ","HARMO", 0, 100, 100),
            new ZoneJurisdiction("LSFD","HARMO", 0, 100, 100),
            new ZoneJurisdiction("LSMC","HARMO", 0, 100, 100),

            new ZoneJurisdiction("LSPD-VW","HAWICK", 0, 85, 70) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSSD-VW","HAWICK", 1, 15, 30) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSFD","HAWICK", 0, 100, 100),
            new ZoneJurisdiction("LSMC","HAWICK", 0, 100, 100),

            new ZoneJurisdiction("LSPD-VW","HORS", 0, 85, 70),
            new ZoneJurisdiction("LSSD-VW","HORS", 1, 15, 30),
            new ZoneJurisdiction("LSFD","HORS", 0, 100, 100),
            new ZoneJurisdiction("LSMC","HORS", 0, 100, 100),

            new ZoneJurisdiction("LSSD-MJ","HUMLAB", 0, 85, 75),
            new ZoneJurisdiction("LSSD","HUMLAB", 1, 15, 25),
            new ZoneJurisdiction("LSFD","HUMLAB", 0, 100, 100),
            new ZoneJurisdiction("LSMC","HUMLAB", 0, 100, 100),

            new ZoneJurisdiction("SASPA","JAIL", 0, 95, 70),
            new ZoneJurisdiction("LSSD-MJ","JAIL", 1, 5, 30),

            new ZoneJurisdiction("LSPD-RH","KOREAT", 0, 85, 75),
            new ZoneJurisdiction("LSPD","KOREAT", 1, 15, 25),
            new ZoneJurisdiction("LSFD","KOREAT", 0, 100, 100),
            new ZoneJurisdiction("LSMC","KOREAT", 0, 100, 100),

            new ZoneJurisdiction("LSSD","LACT", 0, 100, 100),
            new ZoneJurisdiction("LSFD","LACT", 0, 100, 100),
            new ZoneJurisdiction("LSMC","LACT", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","LAGO", 0, 100, 100),
            new ZoneJurisdiction("LSFD","LAGO", 0, 100, 100),
            new ZoneJurisdiction("LSMC","LAGO", 0, 100, 100),

            new ZoneJurisdiction("LSSD","LDAM", 0, 100, 100),
            new ZoneJurisdiction("LSFD","LDAM", 0, 100, 100),
            new ZoneJurisdiction("LSMC","LDAM", 0, 100, 100),

            new ZoneJurisdiction("LSPD","LEGSQU", 0, 100, 100),
            new ZoneJurisdiction("LSFD","LEGSQU", 0, 100, 100),
            new ZoneJurisdiction("MRH","LEGSQU", 0, 100, 100),

            new ZoneJurisdiction("LSPD-ELS","LMESA", 0, 85, 70) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSPD","LMESA", 1, 15, 10) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSFD","LMESA", 0, 100, 100),
            new ZoneJurisdiction("LSMC","LMESA", 0, 100, 100),

            new ZoneJurisdiction("LSPD-DP","LOSPUER", 0, 85, 70),
            new ZoneJurisdiction("LSPD","LOSPUER", 1, 15, 30),
            new ZoneJurisdiction("LSFD","LOSPUER", 0, 100, 100),
            new ZoneJurisdiction("LSMC","LOSPUER", 0, 100, 100),

            new ZoneJurisdiction("LSPD-ELS","MIRR", 0, 85, 70),
            new ZoneJurisdiction("LSPD","MIRR", 1, 15, 10),
            new ZoneJurisdiction("LSFD","MIRR", 0, 100, 100),
            new ZoneJurisdiction("LSMC","MIRR", 0, 100, 100),

            new ZoneJurisdiction("LSPD-RH","MORN", 0, 85, 70),
            new ZoneJurisdiction("LSPD","MORN", 1, 15, 5),
            new ZoneJurisdiction("LSFD","MORN", 0, 100, 100),
            new ZoneJurisdiction("LSMC","MORN", 0, 100, 100),

            new ZoneJurisdiction("LSPD-RH","MOVIE", 0, 85, 70),
            new ZoneJurisdiction("LSPD","MOVIE", 1, 15, 30),
            new ZoneJurisdiction("LSFD","MOVIE", 0, 100, 100),
            new ZoneJurisdiction("LSMC","MOVIE", 0, 100, 100),

            new ZoneJurisdiction("SAPR","MTCHIL", 0, 51, 35),
            new ZoneJurisdiction("LSSD-BC","MTCHIL", 1, 49, 65),
            new ZoneJurisdiction("LSFD","MTCHIL", 0, 100, 100),
            new ZoneJurisdiction("LSMC","MTCHIL", 0, 100, 100),

            new ZoneJurisdiction("SAPR","MTGORDO", 0, 51, 35),
            new ZoneJurisdiction("LSSD-BC","MTGORDO", 1, 49, 65),
            new ZoneJurisdiction("LSFD","MTGORDO", 0, 100, 100),
            new ZoneJurisdiction("LSMC","MTGORDO", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","MTJOSE", 0, 95, 75),
            new ZoneJurisdiction("LSSD","MTJOSE", 1, 5, 25),
            new ZoneJurisdiction("LSFD","MTJOSE", 0, 100, 100),
            new ZoneJurisdiction("LSMC","MTJOSE", 0, 100, 100),

            new ZoneJurisdiction("LSPD-ELS","MURRI", 0, 85, 70),
            new ZoneJurisdiction("LSPD","MURRI", 1, 15, 30),
            new ZoneJurisdiction("LSFD","MURRI", 0, 100, 100),
            new ZoneJurisdiction("LSMC","MURRI", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","NCHU", 0, 85, 75),
            new ZoneJurisdiction("LSSD","NCHU", 1, 15, 25),
            new ZoneJurisdiction("LSFD","NCHU", 0, 100, 100),
            new ZoneJurisdiction("LSMC","NCHU", 0, 100, 100),

            new ZoneJurisdiction("LSSD","NOOSE", 0, 100, 100),
            new ZoneJurisdiction("LSFD","NOOSE", 0, 100, 100),
            new ZoneJurisdiction("LSMC","NOOSE", 0, 100, 100),

            new ZoneJurisdiction("SACG","OCEANA", 0, 95, 80),
            new ZoneJurisdiction("LSPD","OCEANA", 1, 5, 20),
            new ZoneJurisdiction("LSFD","OCEANA", 0, 100, 100),
            new ZoneJurisdiction("LSMC","OCEANA", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","PALCOV", 0, 85, 75),
            new ZoneJurisdiction("LSSD","PALCOV", 1, 15, 25),
            new ZoneJurisdiction("LSFD","PALCOV", 0, 100, 100),
            new ZoneJurisdiction("LSMC","PALCOV", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","PALETO", 0, 85, 75) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSSD","PALETO", 1, 15, 25) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSFD","PALETO", 0, 100, 100),
            new ZoneJurisdiction("LSMC","PALETO", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","PALFOR", 0, 85, 75),
            new ZoneJurisdiction("LSSD","PALFOR", 1, 15, 25),
            new ZoneJurisdiction("LSFD","PALFOR", 0, 100, 100),
            new ZoneJurisdiction("LSMC","PALFOR", 0, 100, 100),

            new ZoneJurisdiction("LSSD","PALHIGH", 0, 100, 100),
            new ZoneJurisdiction("LSFD","PALHIGH", 0, 100, 100),
            new ZoneJurisdiction("LSMC","PALHIGH", 0, 100, 100),

            new ZoneJurisdiction("LSSD","PALMPOW", 0, 100, 100),
            new ZoneJurisdiction("LSFD","PALMPOW", 0, 100, 100),
            new ZoneJurisdiction("LSMC","PALMPOW", 0, 100, 100),

            new ZoneJurisdiction("LSPD-DP","PBLUFF", 0, 85, 70),
            new ZoneJurisdiction("LSPD","PBLUFF", 1, 15, 30),
            new ZoneJurisdiction("LSFD","PBLUFF", 0, 100, 100),
            new ZoneJurisdiction("LSMC","PBLUFF", 0, 100, 100),

            new ZoneJurisdiction("LSPD","PBOX", 0, 100, 100) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSFD","PBOX", 0, 100, 100),
            new ZoneJurisdiction("LSMC","PBOX", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","PROCOB", 0, 85, 75),
            new ZoneJurisdiction("LSSD","PROCOB", 1, 15, 25),
            new ZoneJurisdiction("LSFD","PROCOB", 0, 100, 100),
            new ZoneJurisdiction("LSMC","PROCOB", 0, 100, 100),

            new ZoneJurisdiction("LSPD","RANCHO", 0, 100, 100),
            new ZoneJurisdiction("LSFD","RANCHO", 0, 100, 100),
            new ZoneJurisdiction("LSMC","RANCHO", 0, 100, 100),

            new ZoneJurisdiction("LSSD-VW","RGLEN", 0, 100, 100),
            new ZoneJurisdiction("LSFD","RGLEN", 0, 100, 100),
            new ZoneJurisdiction("LSMC","RGLEN", 0, 100, 100),

            new ZoneJurisdiction("LSPD-RH","RICHM", 0, 85, 70),
            new ZoneJurisdiction("LSPD","RICHM", 1, 15, 30),
            new ZoneJurisdiction("LSFD","RICHM", 0, 100, 100),
            new ZoneJurisdiction("LSMC","RICHM", 0, 100, 100),

            new ZoneJurisdiction("LSPD-RH","ROCKF", 0, 85, 70) { CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSPD","ROCKF", 1, 15, 30) { CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSFD","ROCKF", 0, 100, 100),
            new ZoneJurisdiction("LSMC","ROCKF", 0, 100, 100),

            new ZoneJurisdiction("LSSD-MJ","RTRAK", 0, 100, 100),
            new ZoneJurisdiction("LSFD","RTRAK", 0, 100, 100),
            new ZoneJurisdiction("LSMC","RTRAK", 0, 100, 100),

            new ZoneJurisdiction("LSSD","SANAND", 0, 100, 100),
            new ZoneJurisdiction("LSFD","SANAND", 0, 100, 100),
            new ZoneJurisdiction("LSMC","SANAND", 0, 100, 100),

            new ZoneJurisdiction("SAPR","SANCHIA", 0, 51, 35),
            new ZoneJurisdiction("LSSD-MJ","SANCHIA", 1, 49, 65),
            new ZoneJurisdiction("LSFD","SANCHIA", 0, 100, 100),
            new ZoneJurisdiction("LSMC","SANCHIA", 0, 100, 100),

            new ZoneJurisdiction("LSSD-MJ","SANDY", 0, 85, 75),
            new ZoneJurisdiction("LSSD","SANDY", 1, 15, 25),
            new ZoneJurisdiction("LSFD","SANDY", 0, 100, 100),
            new ZoneJurisdiction("LSMC","SANDY", 0, 100, 100),

            new ZoneJurisdiction("LSPD","SKID", 0, 100, 100),
            new ZoneJurisdiction("LSFD","SKID", 0, 100, 100),
            new ZoneJurisdiction("MRH","SKID", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","SLAB", 0, 85, 75),
            new ZoneJurisdiction("LSSD","SLAB", 1, 15, 25),
            new ZoneJurisdiction("LSFD","SLAB", 0, 100, 100),
            new ZoneJurisdiction("MRH","SLAB", 0, 100, 100),

            //new ZoneJurisdiction("PRISEC","STAD", 0, 95, 0),
            //new ZoneJurisdiction("LSPD","STAD", 1, 5, 100),
            new ZoneJurisdiction("LSPD","STAD", 0, 100, 100),
            new ZoneJurisdiction("LSFD","STAD", 0, 100, 100),
            new ZoneJurisdiction("LSMC","STAD", 0, 100, 100),

            new ZoneJurisdiction("LSPD","STRAW", 0, 100, 100),
            new ZoneJurisdiction("LSFD","STRAW", 0, 100, 100),
            new ZoneJurisdiction("LSMC","STRAW", 0, 100, 100),

            new ZoneJurisdiction("LSSD","TATAMO", 0, 100, 100),
            new ZoneJurisdiction("LSFD","TATAMO", 0, 100, 100),
            new ZoneJurisdiction("LSMC","TATAMO", 0, 100, 100),

            new ZoneJurisdiction("LSPP","TERMINA", 0, 95, 80),
            new ZoneJurisdiction("LSPD","TERMINA", 1, 5, 20),
            new ZoneJurisdiction("LSFD","TERMINA", 0, 100, 100),
            new ZoneJurisdiction("LSMC","TERMINA", 0, 100, 100),

            new ZoneJurisdiction("LSPD","TEXTI", 0, 100, 100),
            new ZoneJurisdiction("LSFD","TEXTI", 0, 100, 100),
            new ZoneJurisdiction("MRH","TEXTI", 0, 100, 100),

            new ZoneJurisdiction("LSSD","TONGVAH", 0, 100, 100),
            new ZoneJurisdiction("LSFD","TONGVAH", 0, 100, 100),
            new ZoneJurisdiction("LSMC","TONGVAH", 0, 100, 100),

            new ZoneJurisdiction("LSSD-VW","TONGVAV", 0, 100, 100),
            new ZoneJurisdiction("LSFD","TONGVAV", 0, 100, 100),
            new ZoneJurisdiction("LSMC","TONGVAV", 0, 100, 100),

            new ZoneJurisdiction("LSPD-DP","VCANA", 0, 85, 70) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSPD","VCANA", 1, 15, 30) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSFD","VCANA", 0, 100, 100),
            new ZoneJurisdiction("LSMC","VCANA", 0, 100, 100),

            new ZoneJurisdiction("LSPD-DP","VESP", 0, 85, 70) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSPD","VESP", 1, 15, 30) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSFD","VESP", 0, 100, 100),
            new ZoneJurisdiction("LSMC","VESP", 0, 100, 100),

            new ZoneJurisdiction("LSPD-VW","VINE", 0, 85, 70),
            new ZoneJurisdiction("LSSD-VW","VINE", 1, 15, 30),
            new ZoneJurisdiction("LSFD","VINE", 0, 100, 100),
            new ZoneJurisdiction("LSMC","VINE", 0, 100, 100),

            new ZoneJurisdiction("LSSD","WINDF", 0, 100, 100),
            new ZoneJurisdiction("LSFD","WINDF", 0, 100, 100),
            new ZoneJurisdiction("LSMC","WINDF", 0, 100, 100),

            new ZoneJurisdiction("LSPD-VW","WVINE", 0, 85, 70),
            new ZoneJurisdiction("LSSD-VW","WVINE", 1, 15, 30),
            new ZoneJurisdiction("LSFD","WVINE", 0, 100, 100),
            new ZoneJurisdiction("LSMC","WVINE", 0, 100, 100),

            new ZoneJurisdiction("SAPR","ZANCUDO", 0, 51, 35),
            new ZoneJurisdiction("LSSD-BC","ZANCUDO", 1, 49, 65),

            new ZoneJurisdiction("LSPP","ZP_ORT", 0, 95, 80),
            new ZoneJurisdiction("LSPD","ZP_ORT", 1, 5, 20),
            new ZoneJurisdiction("LSFD","ZP_ORT", 0, 100, 100),
            new ZoneJurisdiction("LSMC","ZP_ORT", 0, 100, 100),

            new ZoneJurisdiction("LSSD-MJ","ZQ_UAR", 0, 85, 75),
            new ZoneJurisdiction("LSSD","ZQ_UAR", 1, 15, 25),
            new ZoneJurisdiction("LSFD","ZQ_UAR", 0, 100, 100),
            new ZoneJurisdiction("LSMC","ZQ_UAR", 0, 100, 100),

            new ZoneJurisdiction("LSSD-BC","GALLI", 0, 100, 100),
            new ZoneJurisdiction("LSFD","GALLI", 0, 100, 100),
            new ZoneJurisdiction("LSMC","GALLI", 0, 100, 100),
            ////Custom
            //new ZoneJurisdiction("APD", "CHI1", 0, 100, 100),
            //new ZoneJurisdiction("NYSP", "LUDEN", 0, 100, 100),
            //new ZoneJurisdiction("VCPD", "VICE", 0, 100, 100),
        };
        Serialization.SerializeParams(ZoneJurisdictionsList, ZoneConfigFileName);
    }
    private void DefaultCountyConfig()
    {
        CountyJurisdictionList = new List<CountyJurisdiction>()
        {
            new CountyJurisdiction("LSPD-ASD",County.CityOfLosSantos, 0, 100, 100),
            new CountyJurisdiction("LSSD-ASD",County.BlaineCounty, 0, 100, 100),
            new CountyJurisdiction("LSSD-ASD",County.LosSantosCounty, 0, 100, 100),
            new CountyJurisdiction("NYSP", County.NorthYankton, 0, 100, 100),
        };
        Serialization.SerializeParams(CountyJurisdictionList, CountyConfigFileName);
    }


    private void DefaultZoneConfig_FullExpandedJurisdiction()
    {
        List<ZoneJurisdiction> SimpleZoneJurisdictionsList = new List<ZoneJurisdiction>()
        {
            new ZoneJurisdiction("LSIAPD","AIRP", 0, 100, 100) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSFD","AIRP", 0, 100, 100),
            new ZoneJurisdiction("MRH","AIRP", 0, 100, 100),

            new ZoneJurisdiction("LSSD-MJ","ALAMO", 0, 100, 100),
            new ZoneJurisdiction("LSFD","ALAMO", 0, 100, 100),
            new ZoneJurisdiction("LSMC","ALAMO", 0, 100, 100),

            new ZoneJurisdiction("LSPD-VW","ALTA", 0, 100, 100) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSFD","ALTA", 0, 100, 100),
            new ZoneJurisdiction("LSMC","ALTA", 0, 100, 100),

            new ZoneJurisdiction("ARMY","ARMYB", 0, 100, 100),

            new ZoneJurisdiction("LSSD","BANHAMC", 0, 100, 100),
            new ZoneJurisdiction("LSFD","BANHAMC", 0, 100, 100),
            new ZoneJurisdiction("LSMC","BANHAMC", 0, 100, 100),

            new ZoneJurisdiction("LSPD","BANNING", 0, 100, 100),
            new ZoneJurisdiction("LSFD","BANNING", 0, 100, 100),
            new ZoneJurisdiction("LSMC","BANNING", 0, 100, 100),

            new ZoneJurisdiction("LSSD-VW","BAYTRE", 0, 100, 100),
            new ZoneJurisdiction("LSFD","BAYTRE", 0, 100, 100),
            new ZoneJurisdiction("LSMC","BAYTRE", 0, 100, 100),

            new ZoneJurisdiction("DPPD","BEACH", 0, 100, 100),
            new ZoneJurisdiction("LSFD","BEACH", 0, 100, 100),
            new ZoneJurisdiction("LSMC","BEACH", 0, 100, 100),

            new ZoneJurisdiction("LSSD","BHAMCA", 0, 100, 100),
            new ZoneJurisdiction("LSFD","BHAMCA", 0, 100, 100),
            new ZoneJurisdiction("LSMC","BHAMCA", 0, 100, 100),

            new ZoneJurisdiction("BCSO","BRADP", 0, 100, 100),
            new ZoneJurisdiction("LSFD","BRADP", 0, 100, 100),
            new ZoneJurisdiction("LSMC","BRADP", 0, 100, 100),

            new ZoneJurisdiction("BCSO","BRADT", 0, 100, 100),
            new ZoneJurisdiction("LSFD","BRADT", 0, 100, 100),
            new ZoneJurisdiction("LSMC","BRADT", 0, 100, 100),

            new ZoneJurisdiction("RHPD","BURTON", 0, 100, 100) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSFD","BURTON", 0, 100, 100),
            new ZoneJurisdiction("LSMC","BURTON", 0, 100, 100),

            new ZoneJurisdiction("BCSO","CALAFB", 0, 100, 100),
            new ZoneJurisdiction("LSFD","CALAFB", 0, 100, 100),
            new ZoneJurisdiction("LSMC","CALAFB", 0, 100, 100),

            new ZoneJurisdiction("BCSO","CANNY", 0, 100, 100),
            new ZoneJurisdiction("LSFD","CANNY", 0, 100, 100),
            new ZoneJurisdiction("LSMC","CANNY", 0, 100, 100),

            new ZoneJurisdiction("BCSO","CCREAK", 0, 100, 100),
            new ZoneJurisdiction("LSFD","CCREAK", 0, 100, 100),
            new ZoneJurisdiction("LSMC","CCREAK", 0, 100, 100),

            new ZoneJurisdiction("LSPD","CHAMH", 0, 100, 100),
            new ZoneJurisdiction("LSFD","CHAMH", 0, 100, 100),
            new ZoneJurisdiction("LSMC","CHAMH", 0, 100, 100),

            new ZoneJurisdiction("LSSD-VW","CHIL", 0, 100, 100),
            new ZoneJurisdiction("LSFD","CHIL", 0, 100, 100),
            new ZoneJurisdiction("LSMC","CHIL", 0, 100, 100),

            new ZoneJurisdiction("LSSD","CHU", 0, 100, 100),
            new ZoneJurisdiction("LSFD","CHU", 0, 100, 100),
            new ZoneJurisdiction("LSMC","CHU", 0, 100, 100),

            new ZoneJurisdiction("SAPR","CMSW", 0, 51, 35),
            new ZoneJurisdiction("BCSO","CMSW", 1, 49, 65),
            new ZoneJurisdiction("LSFD","CMSW", 0, 100, 100),
            new ZoneJurisdiction("LSMC","CMSW", 0, 100, 100),

            new ZoneJurisdiction("LSPD-ELS","CYPRE", 0, 100, 100),
            new ZoneJurisdiction("LSFD","CYPRE", 0, 100, 100),
            new ZoneJurisdiction("LSMC","CYPRE", 0, 100, 100),

            new ZoneJurisdiction("LSSD-DV","DAVIS", 0, 100, 100),
            new ZoneJurisdiction("LSFD","DAVIS", 0, 100, 100),
            new ZoneJurisdiction("MRH","DAVIS", 0, 100, 100),

            new ZoneJurisdiction("DPPD","DELBE", 0, 85, 70),
            new ZoneJurisdiction("LSFD","DELBE", 0, 100, 100),
            new ZoneJurisdiction("LSMC","DELBE", 0, 100, 100),

            new ZoneJurisdiction("DPPD","DELPE", 0, 100, 100) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSFD","DELPE", 0, 100, 100),
            new ZoneJurisdiction("LSMC","DELPE", 0, 100, 100),

            new ZoneJurisdiction("DPPD","DELSOL", 0, 100, 100),
            new ZoneJurisdiction("LSFD","DELSOL", 0, 100, 100),
            new ZoneJurisdiction("LSMC","DELSOL", 0, 100, 100),

            new ZoneJurisdiction("LSSD-MJ","DESRT", 0, 100, 100),
            new ZoneJurisdiction("LSFD","DESRT", 0, 100, 100),
            new ZoneJurisdiction("LSMC","DESRT", 0, 100, 100),

            new ZoneJurisdiction("LSPD","DOWNT", 0, 100, 100),
            new ZoneJurisdiction("LSFD","DOWNT", 0, 100, 100),
            new ZoneJurisdiction("MRH","DOWNT", 0, 100, 100),

            new ZoneJurisdiction("LSPD-VW","DTVINE", 0, 100, 100),
            new ZoneJurisdiction("LSFD","DTVINE", 0, 100, 100),
            new ZoneJurisdiction("LSMC","DTVINE", 0, 100, 100),

            new ZoneJurisdiction("LSPD-VW","EAST_V", 0, 100, 100),
            new ZoneJurisdiction("LSFD","EAST_V", 0, 100, 100),
            new ZoneJurisdiction("LSMC","EAST_V", 0, 100, 100),

            new ZoneJurisdiction("LSPD-ELS","EBURO", 0, 100, 100),
            new ZoneJurisdiction("LSFD","EBURO", 0, 100, 100),
            new ZoneJurisdiction("LSMC","EBURO", 0, 100, 100),

            new ZoneJurisdiction("BCSO","ELGORL", 0, 100, 100),
            new ZoneJurisdiction("LSFD","ELGORL", 0, 100, 100),
            new ZoneJurisdiction("LSMC","ELGORL", 0, 100, 100),

            new ZoneJurisdiction("LSPP","ELYSIAN", 0, 100, 100),
            new ZoneJurisdiction("LSFD","ELYSIAN", 0, 100, 100),
            new ZoneJurisdiction("LSMC","ELYSIAN", 0, 100, 100),

            new ZoneJurisdiction("BCSO","GALFISH", 0, 100, 100),
            new ZoneJurisdiction("LSFD","GALFISH", 0, 100, 100),
            new ZoneJurisdiction("LSMC","GALFISH", 0, 100, 100),

            new ZoneJurisdiction("RHPD","GOLF", 0, 100, 100),
            new ZoneJurisdiction("LSFD","GOLF", 0, 100, 100),
            new ZoneJurisdiction("LSMC","GOLF", 0, 100, 100),

            new ZoneJurisdiction("GSPD","GRAPES", 0, 100, 100),
            new ZoneJurisdiction("LSFD","GRAPES", 0, 100, 100),
            new ZoneJurisdiction("LSMC","GRAPES", 0, 100, 100),

            new ZoneJurisdiction("LSSD-VW","GREATC", 0, 100, 100),
            new ZoneJurisdiction("LSFD","GREATC", 0, 100, 100),
            new ZoneJurisdiction("LSMC","GREATC", 0, 100, 100),

            new ZoneJurisdiction("LSSD-MJ","HARMO", 0, 100, 100),
            new ZoneJurisdiction("LSFD","HARMO", 0, 100, 100),
            new ZoneJurisdiction("LSMC","HARMO", 0, 100, 100),

            new ZoneJurisdiction("LSPD-VW","HAWICK", 0, 100, 100) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSFD","HAWICK", 0, 100, 100),
            new ZoneJurisdiction("LSMC","HAWICK", 0, 100, 100),

            new ZoneJurisdiction("LSPD-VW","HORS", 0, 100, 100),
            new ZoneJurisdiction("LSFD","HORS", 0, 100, 100),
            new ZoneJurisdiction("LSMC","HORS", 0, 100, 100),

            new ZoneJurisdiction("LSSD-MJ","HUMLAB", 0, 100, 100),
            new ZoneJurisdiction("LSFD","HUMLAB", 0, 100, 100),
            new ZoneJurisdiction("LSMC","HUMLAB", 0, 100, 100),

            new ZoneJurisdiction("SASPA","JAIL", 0, 100, 100),

            new ZoneJurisdiction("RHPD","KOREAT", 0, 100, 100),
            new ZoneJurisdiction("LSFD","KOREAT", 0, 100, 100),
            new ZoneJurisdiction("LSMC","KOREAT", 0, 100, 100),

            new ZoneJurisdiction("LSSD","LACT", 0, 100, 100),
            new ZoneJurisdiction("LSFD","LACT", 0, 100, 100),
            new ZoneJurisdiction("LSMC","LACT", 0, 100, 100),

            new ZoneJurisdiction("BCSO","LAGO", 0, 100, 100),
            new ZoneJurisdiction("LSFD","LAGO", 0, 100, 100),
            new ZoneJurisdiction("LSMC","LAGO", 0, 100, 100),

            new ZoneJurisdiction("LSSD","LDAM", 0, 100, 100),
            new ZoneJurisdiction("LSFD","LDAM", 0, 100, 100),
            new ZoneJurisdiction("LSMC","LDAM", 0, 100, 100),

            new ZoneJurisdiction("LSPD","LEGSQU", 0, 100, 100),
            new ZoneJurisdiction("LSFD","LEGSQU", 0, 100, 100),
            new ZoneJurisdiction("MRH","LEGSQU", 0, 100, 100),

            new ZoneJurisdiction("LSPD-ELS","LMESA", 0, 100, 100) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSFD","LMESA", 0, 100, 100),
            new ZoneJurisdiction("LSMC","LMESA", 0, 100, 100),

            new ZoneJurisdiction("DPPD","LOSPUER", 0, 100, 100),
            new ZoneJurisdiction("LSFD","LOSPUER", 0, 100, 100),
            new ZoneJurisdiction("LSMC","LOSPUER", 0, 100, 100),

            new ZoneJurisdiction("LSPD-ELS","MIRR", 0, 100, 100),
            new ZoneJurisdiction("LSFD","MIRR", 0, 100, 100),
            new ZoneJurisdiction("LSMC","MIRR", 0, 100, 100),

            new ZoneJurisdiction("RHPD","MORN", 0, 100, 100),
            new ZoneJurisdiction("LSFD","MORN", 0, 100, 100),
            new ZoneJurisdiction("LSMC","MORN", 0, 100, 100),

            new ZoneJurisdiction("RHPD","MOVIE", 0, 100, 100),
            new ZoneJurisdiction("LSFD","MOVIE", 0, 100, 100),
            new ZoneJurisdiction("LSMC","MOVIE", 0, 100, 100),

            new ZoneJurisdiction("SAPR","MTCHIL", 0, 100, 100),
            new ZoneJurisdiction("LSFD","MTCHIL", 0, 100, 100),
            new ZoneJurisdiction("LSMC","MTCHIL", 0, 100, 100),

            new ZoneJurisdiction("SAPR","MTGORDO", 0, 100, 100),
            new ZoneJurisdiction("LSFD","MTGORDO", 0, 100, 100),
            new ZoneJurisdiction("LSMC","MTGORDO", 0, 100, 100),

            new ZoneJurisdiction("BCSO","MTJOSE", 0, 100, 100),
            new ZoneJurisdiction("LSFD","MTJOSE", 0, 100, 100),
            new ZoneJurisdiction("LSMC","MTJOSE", 0, 100, 100),

            new ZoneJurisdiction("LSPD-ELS","MURRI", 0, 100, 100),
            new ZoneJurisdiction("LSFD","MURRI", 0, 100, 100),
            new ZoneJurisdiction("LSMC","MURRI", 0, 100, 100),

            new ZoneJurisdiction("BCSO","NCHU", 0, 100, 100),
            new ZoneJurisdiction("LSFD","NCHU", 0, 100, 100),
            new ZoneJurisdiction("LSMC","NCHU", 0, 100, 100),

            new ZoneJurisdiction("LSSD","NOOSE", 0, 100, 100),
            new ZoneJurisdiction("LSFD","NOOSE", 0, 100, 100),
            new ZoneJurisdiction("LSMC","NOOSE", 0, 100, 100),

            new ZoneJurisdiction("SACG","OCEANA", 0, 100, 100),
            new ZoneJurisdiction("LSFD","OCEANA", 0, 100, 100),
            new ZoneJurisdiction("LSMC","OCEANA", 0, 100, 100),

            new ZoneJurisdiction("BCSO","PALCOV", 0, 100, 100),
            new ZoneJurisdiction("LSFD","PALCOV", 0, 100, 100),
            new ZoneJurisdiction("LSMC","PALCOV", 0, 100, 100),

            new ZoneJurisdiction("BCSO","PALETO", 0, 100, 100) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSFD","PALETO", 0, 100, 100),
            new ZoneJurisdiction("LSMC","PALETO", 0, 100, 100),

            new ZoneJurisdiction("BCSO","PALFOR", 0, 100, 100),
            new ZoneJurisdiction("LSFD","PALFOR", 0, 100, 100),
            new ZoneJurisdiction("LSMC","PALFOR", 0, 100, 100),

            new ZoneJurisdiction("LSSD","PALHIGH", 0, 100, 100),
            new ZoneJurisdiction("LSFD","PALHIGH", 0, 100, 100),
            new ZoneJurisdiction("LSMC","PALHIGH", 0, 100, 100),

            new ZoneJurisdiction("LSSD","PALMPOW", 0, 100, 100),
            new ZoneJurisdiction("LSFD","PALMPOW", 0, 100, 100),
            new ZoneJurisdiction("LSMC","PALMPOW", 0, 100, 100),

            new ZoneJurisdiction("DPPD","PBLUFF", 0, 100, 100),
            new ZoneJurisdiction("LSFD","PBLUFF", 0, 100, 100),
            new ZoneJurisdiction("LSMC","PBLUFF", 0, 100, 100),

            new ZoneJurisdiction("LSPD","PBOX", 0, 100, 100) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSFD","PBOX", 0, 100, 100),
            new ZoneJurisdiction("LSMC","PBOX", 0, 100, 100),

            new ZoneJurisdiction("BCSO","PROCOB", 0, 100, 100),
            new ZoneJurisdiction("LSFD","PROCOB", 0, 100, 100),
            new ZoneJurisdiction("LSMC","PROCOB", 0, 100, 100),

            new ZoneJurisdiction("LSPD","RANCHO", 0, 100, 100),
            new ZoneJurisdiction("LSFD","RANCHO", 0, 100, 100),
            new ZoneJurisdiction("LSMC","RANCHO", 0, 100, 100),

            new ZoneJurisdiction("LSSD-VW","RGLEN", 0, 100, 100),
            new ZoneJurisdiction("LSFD","RGLEN", 0, 100, 100),
            new ZoneJurisdiction("LSMC","RGLEN", 0, 100, 100),

            new ZoneJurisdiction("RHPD","RICHM", 0, 100, 100),
            new ZoneJurisdiction("LSFD","RICHM", 0, 100, 100),
            new ZoneJurisdiction("LSMC","RICHM", 0, 100, 100),

            new ZoneJurisdiction("RHPD","ROCKF", 0, 100, 100) { CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSFD","ROCKF", 0, 100, 100),
            new ZoneJurisdiction("LSMC","ROCKF", 0, 100, 100),

            new ZoneJurisdiction("LSSD-MJ","RTRAK", 0, 100, 100),
            new ZoneJurisdiction("LSFD","RTRAK", 0, 100, 100),
            new ZoneJurisdiction("LSMC","RTRAK", 0, 100, 100),

            new ZoneJurisdiction("LSSD","SANAND", 0, 100, 100),
            new ZoneJurisdiction("LSFD","SANAND", 0, 100, 100),
            new ZoneJurisdiction("LSMC","SANAND", 0, 100, 100),

            new ZoneJurisdiction("SAPR","SANCHIA", 0, 100, 100),
            new ZoneJurisdiction("LSFD","SANCHIA", 0, 100, 100),
            new ZoneJurisdiction("LSMC","SANCHIA", 0, 100, 100),

            new ZoneJurisdiction("LSSD-MJ","SANDY", 0, 100, 100),
            new ZoneJurisdiction("LSFD","SANDY", 0, 100, 100),
            new ZoneJurisdiction("LSMC","SANDY", 0, 100, 100),

            new ZoneJurisdiction("LSPD","SKID", 0, 100, 100),
            new ZoneJurisdiction("LSFD","SKID", 0, 100, 100),
            new ZoneJurisdiction("MRH","SKID", 0, 100, 100),

            new ZoneJurisdiction("BCSO","SLAB", 0, 100, 100),
            new ZoneJurisdiction("LSFD","SLAB", 0, 100, 100),
            new ZoneJurisdiction("MRH","SLAB", 0, 100, 100),

            //new ZoneJurisdiction("PRISEC","STAD", 0, 95, 0),
            //new ZoneJurisdiction("LSPD","STAD", 1, 5, 100),
            new ZoneJurisdiction("LSPD","STAD", 0, 100, 100),
            new ZoneJurisdiction("LSFD","STAD", 0, 100, 100),
            new ZoneJurisdiction("LSMC","STAD", 0, 100, 100),

            new ZoneJurisdiction("LSPD","STRAW", 0, 100, 100),
            new ZoneJurisdiction("LSFD","STRAW", 0, 100, 100),
            new ZoneJurisdiction("LSMC","STRAW", 0, 100, 100),

            new ZoneJurisdiction("LSSD","TATAMO", 0, 100, 100),
            new ZoneJurisdiction("LSFD","TATAMO", 0, 100, 100),
            new ZoneJurisdiction("LSMC","TATAMO", 0, 100, 100),

            new ZoneJurisdiction("LSPP","TERMINA", 0, 100, 100),
            new ZoneJurisdiction("LSFD","TERMINA", 0, 100, 100),
            new ZoneJurisdiction("LSMC","TERMINA", 0, 100, 100),

            new ZoneJurisdiction("LSPD","TEXTI", 0, 100, 100),
            new ZoneJurisdiction("LSFD","TEXTI", 0, 100, 100),
            new ZoneJurisdiction("MRH","TEXTI", 0, 100, 100),

            new ZoneJurisdiction("LSSD","TONGVAH", 0, 100, 100),
            new ZoneJurisdiction("LSFD","TONGVAH", 0, 100, 100),
            new ZoneJurisdiction("LSMC","TONGVAH", 0, 100, 100),

            new ZoneJurisdiction("LSSD-VW","TONGVAV", 0, 100, 100),
            new ZoneJurisdiction("LSFD","TONGVAV", 0, 100, 100),
            new ZoneJurisdiction("LSMC","TONGVAV", 0, 100, 100),

            new ZoneJurisdiction("DPPD","VCANA", 0, 100, 100) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSFD","VCANA", 0, 100, 100),
            new ZoneJurisdiction("LSMC","VCANA", 0, 100, 100),

            new ZoneJurisdiction("DPPD","VESP", 0, 100, 100) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSFD","VESP", 0, 100, 100),
            new ZoneJurisdiction("LSMC","VESP", 0, 100, 100),

            new ZoneJurisdiction("LSPD-VW","VINE", 0, 100, 100),
            new ZoneJurisdiction("LSFD","VINE", 0, 100, 100),
            new ZoneJurisdiction("LSMC","VINE", 0, 100, 100),

            new ZoneJurisdiction("LSSD","WINDF", 0, 100, 100),
            new ZoneJurisdiction("LSFD","WINDF", 0, 100, 100),
            new ZoneJurisdiction("LSMC","WINDF", 0, 100, 100),

            new ZoneJurisdiction("LSPD-VW","WVINE", 0, 100, 100),
            new ZoneJurisdiction("LSFD","WVINE", 0, 100, 100),
            new ZoneJurisdiction("LSMC","WVINE", 0, 100, 100),

            new ZoneJurisdiction("SAPR","ZANCUDO", 0, 51, 35),
            new ZoneJurisdiction("BCSO","ZANCUDO", 1, 49, 65),

            new ZoneJurisdiction("LSPP","ZP_ORT", 0, 100, 100),
            new ZoneJurisdiction("LSFD","ZP_ORT", 0, 100, 100),
            new ZoneJurisdiction("LSMC","ZP_ORT", 0, 100, 100),

            new ZoneJurisdiction("LSSD-MJ","ZQ_UAR", 0, 100, 100),
            new ZoneJurisdiction("LSFD","ZQ_UAR", 0, 100, 100),
            new ZoneJurisdiction("LSMC","ZQ_UAR", 0, 100, 100),

            new ZoneJurisdiction("BCSO","GALLI", 0, 100, 100),
            new ZoneJurisdiction("LSFD","GALLI", 0, 100, 100),
            new ZoneJurisdiction("LSMC","GALLI", 0, 100, 100),
            ////Custom
            //new ZoneJurisdiction("APD", "CHI1", 0, 100, 100),
            //new ZoneJurisdiction("NYSP", "LUDEN", 0, 100, 100),
            //new ZoneJurisdiction("VCPD", "VICE", 0, 100, 100),      
        };
        Serialization.SerializeParams(SimpleZoneJurisdictionsList, "Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedJurisdiction\\ZoneJurisdictions_FullExpandedJurisdiction.xml");
        Serialization.SerializeParams(SimpleZoneJurisdictionsList, "Plugins\\LosSantosRED\\AlternateConfigs\\EUP\\ZoneJurisdictions_EUP.xml");
    }
    private void DefaultCountyConfig_FullExpandedJurisdiction()
    {
        List<CountyJurisdiction> SimpleCountyJurisdictionList = new List<CountyJurisdiction>()
        {
            new CountyJurisdiction("LSPD-ASD",County.CityOfLosSantos, 0, 100, 100),
            new CountyJurisdiction("LSSD-ASD",County.BlaineCounty, 0, 100, 100),
            new CountyJurisdiction("LSSD-ASD",County.LosSantosCounty, 0, 100, 100),
            new CountyJurisdiction("NYSP", County.NorthYankton, 0, 100, 100),
        };
        Serialization.SerializeParams(SimpleCountyJurisdictionList, "Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedJurisdiction\\CountyJurisdictions_FullExpandedJurisdiction.xml");
        Serialization.SerializeParams(SimpleCountyJurisdictionList, "Plugins\\LosSantosRED\\AlternateConfigs\\EUP\\CountyJurisdictions_EUP.xml");

    }
}



