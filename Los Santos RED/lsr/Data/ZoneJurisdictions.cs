using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ZoneJurisdictions : IZoneJurisdictions
{
    private IAgencies AgencyProvider;
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\ZoneJurisdiction.xml";
    private List<ZoneJurisdiction> ZoneJurisdictionsList = new List<ZoneJurisdiction>();
    private bool UseVanillaConfig = true;

    public ZoneJurisdictions(IAgencies agencyProvider)
    {
        AgencyProvider = agencyProvider;
    }
    public void ReadConfig()
    {
        if (File.Exists(ConfigFileName))
        {
            ZoneJurisdictionsList = Serialization.DeserializeParams<ZoneJurisdiction>(ConfigFileName);
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
            Serialization.SerializeParams(ZoneJurisdictionsList, ConfigFileName);
        }
    }
    public Agency GetMainAgency(string ZoneName)
    {
        if (ZoneJurisdictionsList.Any())
        {
            ZoneJurisdiction cool = ZoneJurisdictionsList.Where(x => x.ZoneInternalGameName.ToLower() == ZoneName.ToLower()).OrderBy(y => y.Priority).FirstOrDefault();
            if (cool != null)
            {
                return AgencyProvider.GetAgency(cool.AgencyInitials);
            }
        }
        return null;
    }
    public Agency GetRandomAgency(string ZoneName, int WantedLevel)
    {
        if (ZoneJurisdictionsList.Any())
        {
            List<ZoneJurisdiction> ToPickFrom = new List<ZoneJurisdiction>();
            foreach (ZoneJurisdiction zoneJurisdiction in ZoneJurisdictionsList.Where(x => x.ZoneInternalGameName.ToLower() == ZoneName.ToLower()))
            {
                Agency Agency = AgencyProvider.GetAgency(zoneJurisdiction.AgencyInitials);
                if (Agency != null && Agency.CanSpawn(WantedLevel))
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
    public List<Agency> GetAgencies(string ZoneName, int WantedLevel)
    {
        if (ZoneJurisdictionsList.Any())
        {
            List<Agency> ToReturn = new List<Agency>();
            foreach (ZoneJurisdiction zoneJurisdiction in ZoneJurisdictionsList.Where(x => x.ZoneInternalGameName.ToLower() == ZoneName.ToLower()).OrderBy(k => k.CurrentSpawnChance(WantedLevel)))
            {
                Agency Agency = AgencyProvider.GetAgency(zoneJurisdiction.AgencyInitials);
                if(Agency != null && Agency.CanSpawn(WantedLevel))
                {
                    ToReturn.Add(Agency);
                }
            }
            if(!ToReturn.Any())
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
    private void DefaultConfig()
    {
        ZoneJurisdictionsList = new List<ZoneJurisdiction>()
        {
            new ZoneJurisdiction("LSIAPD","AIRP", 0, 95, 95) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSPD","AIRP", 1, 5, 5) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSSD-BC","ALAMO", 0, 85, 75),
            new ZoneJurisdiction("LSSD","ALAMO", 1, 15, 5),

            new ZoneJurisdiction("LSPD-VW","ALTA", 0, 100, 70) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("ARMY","ARMYB", 0, 100, 100),

            new ZoneJurisdiction("LSSD-CH","BANHAMC", 0, 100, 70),

            new ZoneJurisdiction("LSPD","BANNING", 0, 100, 70),
            new ZoneJurisdiction("LSSD-VW","BAYTRE", 0, 100, 70),

            new ZoneJurisdiction("LSPD-DP","BEACH", 0, 95, 70),
            new ZoneJurisdiction("LSPD","BEACH", 1, 5, 10),

            new ZoneJurisdiction("LSSD-CH","BHAMCA", 0, 100, 70),

            new ZoneJurisdiction("LSSD-BC","BRADP", 0, 85, 75),
            new ZoneJurisdiction("LSSD","BRADP", 1, 15, 5),

            new ZoneJurisdiction("LSSD-BC","BRADT", 0, 85, 75),
            new ZoneJurisdiction("LSSD","BRADT", 1, 15, 5),

            new ZoneJurisdiction("LSPD-RH","BURTON", 0, 85, 70) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSPD","BURTON", 1, 15, 5) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSSD-BC","CALAFB", 0, 85, 75),
            new ZoneJurisdiction("LSSD","CALAFB", 1, 15, 5),

            new ZoneJurisdiction("LSSD-BC","CANNY", 0, 85, 75),
            new ZoneJurisdiction("LSSD","CANNY", 1, 15, 5),

            new ZoneJurisdiction("LSSD-BC","CCREAK", 0, 85, 75),
            new ZoneJurisdiction("LSSD","CCREAK", 1, 15, 5),

            new ZoneJurisdiction("LSPD","CHAMH", 0, 100, 70),

            new ZoneJurisdiction("LSSD-VW","CHIL", 0, 100, 70),

            new ZoneJurisdiction("LSSD-CH","CHU", 0, 100, 70),

            new ZoneJurisdiction("SAPR","CMSW", 0, 51, 20),
            new ZoneJurisdiction("LSSD-BC","CMSW", 1, 49, 65),

            new ZoneJurisdiction("LSPD-ELS","CYPRE", 0, 85, 70),
            new ZoneJurisdiction("LSPD","CYPRE", 1, 15, 10),

            new ZoneJurisdiction("LSPD","DAVIS", 0, 100, 70),

            new ZoneJurisdiction("LSPD-DP","DELBE", 0, 85, 70),
            new ZoneJurisdiction("LSPD","DELBE", 1, 15, 10),

            new ZoneJurisdiction("LSPD-DP","DELPE", 0, 85, 70) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSPD","DELPE", 1, 15, 10) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSPD-DP","DELSOL", 0, 85, 70),
            new ZoneJurisdiction("LSPD","DELSOL", 1, 15, 10),

            new ZoneJurisdiction("LSSD-BC","DESRT", 0, 85, 75),
            new ZoneJurisdiction("LSSD","DESRT", 1, 15, 5),

            new ZoneJurisdiction("LSPD","DOWNT", 0, 100, 80),

            new ZoneJurisdiction("LSPD-VW","DTVINE", 0, 85, 70),
            new ZoneJurisdiction("LSSD-VW","DTVINE", 1, 15, 10),

            new ZoneJurisdiction("LSPD-VW","EAST_V", 0, 85, 70),
            new ZoneJurisdiction("LSSD-VW","EAST_V", 1, 15, 10),

            new ZoneJurisdiction("LSPD-ELS","EBURO", 0, 85, 70),
            new ZoneJurisdiction("LSPD","EBURO", 1, 15, 10),

            new ZoneJurisdiction("LSSD-BC","ELGORL", 0, 85, 75),
            new ZoneJurisdiction("LSSD","ELGORL", 1, 15, 5),

            new ZoneJurisdiction("LSPA","ELYSIAN", 0, 95, 80),
            new ZoneJurisdiction("LSPD","ELYSIAN", 1, 5, 5),

            new ZoneJurisdiction("LSSD-BC","GALFISH", 0, 85, 75),
            new ZoneJurisdiction("LSSD","GALFISH", 1, 15, 5),

            new ZoneJurisdiction("LSPD-RH","GOLF", 0, 85, 70),
            new ZoneJurisdiction("LSPD","GOLF", 1, 15, 5),

            new ZoneJurisdiction("LSSD-BC","GRAPES", 0, 85, 75),
            new ZoneJurisdiction("LSSD","GRAPES", 1, 15, 5),

            new ZoneJurisdiction("LSSD-VW","GREATC", 0, 100, 70),

            new ZoneJurisdiction("LSSD-VW","HARMO", 0, 100, 70),

            new ZoneJurisdiction("LSPD-VW","HAWICK", 0, 85, 70) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSSD-VW","HAWICK", 1, 15, 10) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSPD-VW","HORS", 0, 85, 70),
            new ZoneJurisdiction("LSSD-VW","HORS", 1, 15, 10),

            new ZoneJurisdiction("LSSD-BC","HUMLAB", 0, 85, 75),
            new ZoneJurisdiction("LSSD","HUMLAB", 1, 15, 5),

            new ZoneJurisdiction("SASPA","JAIL", 0, 95, 70),
            new ZoneJurisdiction("LSSD-BC","JAIL", 1, 5, 65),

            new ZoneJurisdiction("LSPD-RH","KOREAT", 0, 85, 70),
            new ZoneJurisdiction("LSPD","KOREAT", 1, 15, 5),

            new ZoneJurisdiction("LSSD","LACT", 0, 100, 70),
            new ZoneJurisdiction("LSSD-BC","LAGO", 0, 100, 75),
            new ZoneJurisdiction("LSSD","LDAM", 0, 100, 70),
            new ZoneJurisdiction("LSPD","LEGSQU", 0, 100, 80),

            new ZoneJurisdiction("LSPD-ELS","LMESA", 0, 85, 70),
            new ZoneJurisdiction("LSPD","LMESA", 1, 15, 10),

            new ZoneJurisdiction("LSPD-DP","LOSPUER", 0, 85, 70),
            new ZoneJurisdiction("LSPD","LOSPUER", 1, 15, 10),

            new ZoneJurisdiction("LSPD-ELS","MIRR", 0, 85, 70),
            new ZoneJurisdiction("LSPD","MIRR", 1, 15, 10),

            new ZoneJurisdiction("LSPD-RH","MORN", 0, 85, 70),
            new ZoneJurisdiction("LSPD","MORN", 1, 15, 5),

            new ZoneJurisdiction("LSPD-RH","MOVIE", 0, 85, 70),
            new ZoneJurisdiction("LSPD","MOVIE", 1, 15, 5),

            new ZoneJurisdiction("SAPR","MTCHIL", 0, 51, 20),
            new ZoneJurisdiction("LSSD-BC","MTCHIL", 1, 49, 65),

            new ZoneJurisdiction("SAPR","MTGORDO", 0, 51, 20),
            new ZoneJurisdiction("LSSD-BC","MTGORDO", 1, 49, 65),

            new ZoneJurisdiction("LSSD-BC","MTJOSE", 0, 95, 75),
            new ZoneJurisdiction("LSSD","MTJOSE", 1, 5, 5),

            new ZoneJurisdiction("LSPD-ELS","MURRI", 0, 85, 70),
            new ZoneJurisdiction("LSPD","MURRI", 1, 15, 10),

            new ZoneJurisdiction("LSSD-BC","NCHU", 0, 85, 75),
            new ZoneJurisdiction("LSSD","NCHU", 1, 15, 5),

            new ZoneJurisdiction("LSSD","NOOSE", 0, 100, 70),

            new ZoneJurisdiction("SACG","OCEANA", 0, 95, 80),
            new ZoneJurisdiction("LSPD","OCEANA", 1, 5, 5),

            new ZoneJurisdiction("LSSD-BC","PALCOV", 0, 85, 75),
            new ZoneJurisdiction("LSSD","PALCOV", 1, 15, 5),

            new ZoneJurisdiction("LSSD-BC","PALETO", 0, 85, 75) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSSD","PALETO", 1, 15, 5) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSSD-BC","PALFOR", 0, 85, 75),
            new ZoneJurisdiction("LSSD","PALFOR", 1, 15, 5),

            new ZoneJurisdiction("LSSD","PALHIGH", 0, 100, 70),
            new ZoneJurisdiction("LSSD","PALMPOW", 0, 100, 70),

            new ZoneJurisdiction("LSPD-DP","PBLUFF", 0, 85, 70),
            new ZoneJurisdiction("LSPD","PBLUFF", 1, 15, 10),

            new ZoneJurisdiction("LSPD","PBOX", 0, 100, 80),

            new ZoneJurisdiction("LSSD-BC","PROCOB", 0, 85, 75),
            new ZoneJurisdiction("LSSD","PROCOB", 1, 15, 5),

            new ZoneJurisdiction("LSPD","RANCHO", 0, 100, 70),
            new ZoneJurisdiction("LSSD-VW","RGLEN", 0, 100, 70),

            new ZoneJurisdiction("LSPD-RH","RICHM", 0, 85, 70),
            new ZoneJurisdiction("LSPD","RICHM", 1, 15, 5),

            new ZoneJurisdiction("LSPD-RH","ROCKF", 0, 85, 70) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSPD","ROCKF", 1, 15, 5) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSSD-VW","RTRAK", 0, 100, 70),
            new ZoneJurisdiction("LSSD","SANAND", 0, 100, 70),

            new ZoneJurisdiction("SAPR","SANCHIA", 0, 51, 20),
            new ZoneJurisdiction("LSSD-BC","SANCHIA", 1, 49, 65),

            new ZoneJurisdiction("LSSD-BC","SANDY", 0, 85, 75),
            new ZoneJurisdiction("LSSD","SANDY", 1, 15, 5),

            new ZoneJurisdiction("LSPD","SKID", 0, 100, 80),

            new ZoneJurisdiction("LSSD-BC","SLAB", 0, 85, 75),
            new ZoneJurisdiction("LSSD","SLAB", 1, 15, 5),

            //new ZoneJurisdiction("PRISEC","STAD", 0, 95, 0),
            //new ZoneJurisdiction("LSPD","STAD", 1, 5, 100),
            new ZoneJurisdiction("LSPD","STAD", 0, 100, 100),

            new ZoneJurisdiction("LSPD","STRAW", 0, 100, 70),

            new ZoneJurisdiction("LSSD","TATAMO", 0, 100, 70),

            new ZoneJurisdiction("LSPA","TERMINA", 0, 95, 80),
            new ZoneJurisdiction("LSPD","TERMINA", 1, 5, 5),

            new ZoneJurisdiction("LSPD","TEXTI", 0, 100, 80),

            new ZoneJurisdiction("LSSD-CH","TONGVAH", 0, 100, 70),

            new ZoneJurisdiction("LSSD-VW","TONGVAV", 0, 100, 70),

            new ZoneJurisdiction("LSPD-DP","VCANA", 0, 85, 70) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSPD","VCANA", 1, 10, 10) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSPD-DP","VESP", 0, 85, 70) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSPD","VESP", 1, 10, 10) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSPD-VW","VINE", 0, 85, 70),
            new ZoneJurisdiction("LSSD-VW","VINE", 1, 10, 10),

            new ZoneJurisdiction("LSSD","WINDF", 0, 100, 70),

            new ZoneJurisdiction("LSPD-VW","WVINE", 0, 85, 70),
            new ZoneJurisdiction("LSSD-VW","WVINE", 1, 15, 10),

            new ZoneJurisdiction("SAPR","ZANCUDO", 0, 51, 20),
            new ZoneJurisdiction("LSSD-BC","ZANCUDO", 1, 49, 65),

            new ZoneJurisdiction("LSPA","ZP_ORT", 0, 95, 80),
            new ZoneJurisdiction("LSPD","ZP_ORT", 1, 5, 5),

            new ZoneJurisdiction("LSSD-BC","ZQ_UAR", 0, 85, 75),
            new ZoneJurisdiction("LSSD","ZQ_UAR", 1, 15, 5),


            new ZoneJurisdiction("LSSD-BC","GALLI", 0, 100, 100),


            //Custom
            new ZoneJurisdiction("APD", "CHI1", 0, 100, 100),
            new ZoneJurisdiction("NYSP", "LUDEN", 0, 100, 100),
            new ZoneJurisdiction("VCPD", "VICE", 0, 100, 100),

            
        };


    }
    private void CustomConfig()
    {
        ZoneJurisdictionsList = new List<ZoneJurisdiction>()
        {
            new ZoneJurisdiction("LSIAPD","AIRP", 0, 95, 95) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSPD","AIRP", 1, 5, 5) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("BCSO","ALAMO", 0, 85, 75),
            new ZoneJurisdiction("LSSD","ALAMO", 1, 15, 5),

            new ZoneJurisdiction("LSPD-VW","ALTA", 0, 100, 70) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("ARMY","ARMYB", 0, 100, 100),

            new ZoneJurisdiction("LSSD-CH","BANHAMC", 0, 100, 70),

            new ZoneJurisdiction("LSPD","BANNING", 0, 100, 70),
            new ZoneJurisdiction("LSSD-VW","BAYTRE", 0, 100, 70),

            new ZoneJurisdiction("DPPD","BEACH", 0, 95, 70),
            new ZoneJurisdiction("LSPD","BEACH", 1, 5, 10),

            new ZoneJurisdiction("LSSD-CH","BHAMCA", 0, 100, 70),

            new ZoneJurisdiction("BCSO","BRADP", 0, 85, 75),
            new ZoneJurisdiction("LSSD","BRADP", 1, 15, 5),

            new ZoneJurisdiction("BCSO","BRADT", 0, 85, 75),
            new ZoneJurisdiction("LSSD","BRADT", 1, 15, 5),

            new ZoneJurisdiction("RHPD","BURTON", 0, 85, 70) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSPD","BURTON", 1, 15, 5) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("BCSO","CALAFB", 0, 85, 75),
            new ZoneJurisdiction("LSSD","CALAFB", 1, 15, 5),

            new ZoneJurisdiction("BCSO","CANNY", 0, 85, 75),
            new ZoneJurisdiction("LSSD","CANNY", 1, 15, 5),

            new ZoneJurisdiction("BCSO","CCREAK", 0, 85, 75),
            new ZoneJurisdiction("LSSD","CCREAK", 1, 15, 5),

            new ZoneJurisdiction("LSPD","CHAMH", 0, 100, 70),

            new ZoneJurisdiction("LSSD-VW","CHIL", 0, 100, 70),

            new ZoneJurisdiction("LSSD-CH","CHU", 0, 100, 70),

            new ZoneJurisdiction("SAPR","CMSW", 0, 51, 20),
            new ZoneJurisdiction("BCSO","CMSW", 1, 49, 65),

            new ZoneJurisdiction("LSPD-ELS","CYPRE", 0, 85, 70),
            new ZoneJurisdiction("LSPD","CYPRE", 1, 15, 10),

            new ZoneJurisdiction("LSPD","DAVIS", 0, 100, 70),

            new ZoneJurisdiction("DPPD","DELBE", 0, 85, 70),
            new ZoneJurisdiction("LSPD","DELBE", 1, 15, 10),

            new ZoneJurisdiction("DPPD","DELPE", 0, 85, 70) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSPD","DELPE", 1, 15, 10) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("DPPD","DELSOL", 0, 85, 70),
            new ZoneJurisdiction("LSPD","DELSOL", 1, 15, 10),

            new ZoneJurisdiction("BCSO","DESRT", 0, 85, 75),
            new ZoneJurisdiction("LSSD","DESRT", 1, 15, 5),

            new ZoneJurisdiction("LSPD","DOWNT", 0, 100, 80),

            new ZoneJurisdiction("LSPD-VW","DTVINE", 0, 85, 70),
            new ZoneJurisdiction("LSSD-VW","DTVINE", 1, 15, 10),

            new ZoneJurisdiction("LSPD-VW","EAST_V", 0, 85, 70),
            new ZoneJurisdiction("LSSD-VW","EAST_V", 1, 15, 10),

            new ZoneJurisdiction("LSPD-ELS","EBURO", 0, 85, 70),
            new ZoneJurisdiction("LSPD","EBURO", 1, 15, 10),

            new ZoneJurisdiction("BCSO","ELGORL", 0, 85, 75),
            new ZoneJurisdiction("LSSD","ELGORL", 1, 15, 5),

            new ZoneJurisdiction("LSPA","ELYSIAN", 0, 95, 80),
            new ZoneJurisdiction("LSPD","ELYSIAN", 1, 5, 5),

            new ZoneJurisdiction("BCSO","GALFISH", 0, 85, 75),
            new ZoneJurisdiction("LSSD","GALFISH", 1, 15, 5),

            new ZoneJurisdiction("RHPD","GOLF", 0, 85, 70),
            new ZoneJurisdiction("LSPD","GOLF", 1, 15, 5),

            new ZoneJurisdiction("BCSO","GRAPES", 0, 85, 75),
            new ZoneJurisdiction("LSSD","GRAPES", 1, 15, 5),

            new ZoneJurisdiction("LSSD-VW","GREATC", 0, 100, 70),

            new ZoneJurisdiction("LSSD-VW","HARMO", 0, 100, 70),

            new ZoneJurisdiction("LSPD-VW","HAWICK", 0, 85, 70) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSSD-VW","HAWICK", 1, 15, 10) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSPD-VW","HORS", 0, 85, 70),
            new ZoneJurisdiction("LSSD-VW","HORS", 1, 15, 10),

            new ZoneJurisdiction("BCSO","HUMLAB", 0, 85, 75),
            new ZoneJurisdiction("LSSD","HUMLAB", 1, 15, 5),

            new ZoneJurisdiction("SASPA","JAIL", 0, 95, 70),
            new ZoneJurisdiction("BCSO","JAIL", 1, 5, 65),

            new ZoneJurisdiction("RHPD","KOREAT", 0, 85, 70),
            new ZoneJurisdiction("LSPD","KOREAT", 1, 15, 5),

            new ZoneJurisdiction("LSSD","LACT", 0, 100, 70),
            new ZoneJurisdiction("BCSO","LAGO", 0, 100, 75),
            new ZoneJurisdiction("LSSD","LDAM", 0, 100, 70),
            new ZoneJurisdiction("LSPD","LEGSQU", 0, 100, 80),

            new ZoneJurisdiction("LSPD-ELS","LMESA", 0, 85, 70),
            new ZoneJurisdiction("LSPD","LMESA", 1, 15, 10),

            new ZoneJurisdiction("DPPD","LOSPUER", 0, 85, 70),
            new ZoneJurisdiction("LSPD","LOSPUER", 1, 15, 10),

            new ZoneJurisdiction("LSPD-ELS","MIRR", 0, 85, 70),
            new ZoneJurisdiction("LSPD","MIRR", 1, 15, 10),

            new ZoneJurisdiction("RHPD","MORN", 0, 85, 70),
            new ZoneJurisdiction("LSPD","MORN", 1, 15, 5),

            new ZoneJurisdiction("RHPD","MOVIE", 0, 85, 70),
            new ZoneJurisdiction("LSPD","MOVIE", 1, 15, 5),

            new ZoneJurisdiction("SAPR","MTCHIL", 0, 51, 20),
            new ZoneJurisdiction("BCSO","MTCHIL", 1, 49, 65),

            new ZoneJurisdiction("SAPR","MTGORDO", 0, 51, 20),
            new ZoneJurisdiction("BCSO","MTGORDO", 1, 49, 65),

            new ZoneJurisdiction("BCSO","MTJOSE", 0, 95, 75),
            new ZoneJurisdiction("LSSD","MTJOSE", 1, 5, 5),

            new ZoneJurisdiction("LSPD-ELS","MURRI", 0, 85, 70),
            new ZoneJurisdiction("LSPD","MURRI", 1, 15, 10),

            new ZoneJurisdiction("BCSO","NCHU", 0, 85, 75),
            new ZoneJurisdiction("LSSD","NCHU", 1, 15, 5),

            new ZoneJurisdiction("LSSD","NOOSE", 0, 100, 70),

            new ZoneJurisdiction("SACG","OCEANA", 0, 95, 80),
            new ZoneJurisdiction("LSPD","OCEANA", 1, 5, 5),

            new ZoneJurisdiction("BCSO","PALCOV", 0, 85, 75),
            new ZoneJurisdiction("LSSD","PALCOV", 1, 15, 5),

            new ZoneJurisdiction("BCSO","PALETO", 0, 85, 75) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSSD","PALETO", 1, 15, 5) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("BCSO","PALFOR", 0, 85, 75),
            new ZoneJurisdiction("LSSD","PALFOR", 1, 15, 5),

            new ZoneJurisdiction("LSSD","PALHIGH", 0, 100, 70),
            new ZoneJurisdiction("LSSD","PALMPOW", 0, 100, 70),

            new ZoneJurisdiction("DPPD","PBLUFF", 0, 85, 70),
            new ZoneJurisdiction("LSPD","PBLUFF", 1, 15, 10),

            new ZoneJurisdiction("LSPD","PBOX", 0, 100, 80),

            new ZoneJurisdiction("BCSO","PROCOB", 0, 85, 75),
            new ZoneJurisdiction("LSSD","PROCOB", 1, 15, 5),

            new ZoneJurisdiction("LSPD","RANCHO", 0, 100, 70),
            new ZoneJurisdiction("LSSD-VW","RGLEN", 0, 100, 70),

            new ZoneJurisdiction("RHPD","RICHM", 0, 85, 70),
            new ZoneJurisdiction("LSPD","RICHM", 1, 15, 5),

            new ZoneJurisdiction("RHPD","ROCKF", 0, 85, 70) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSPD","ROCKF", 1, 15, 5) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSSD-VW","RTRAK", 0, 100, 70),
            new ZoneJurisdiction("LSSD","SANAND", 0, 100, 70),

            new ZoneJurisdiction("SAPR","SANCHIA", 0, 51, 20),
            new ZoneJurisdiction("BCSO","SANCHIA", 1, 49, 65),

            new ZoneJurisdiction("BCSO","SANDY", 0, 85, 75),
            new ZoneJurisdiction("LSSD","SANDY", 1, 15, 5),

            new ZoneJurisdiction("LSPD","SKID", 0, 100, 80),

            new ZoneJurisdiction("BCSO","SLAB", 0, 85, 75),
            new ZoneJurisdiction("LSSD","SLAB", 1, 15, 5),

            //new ZoneJurisdiction("PRISEC","STAD", 0, 95, 0),
            //new ZoneJurisdiction("LSPD","STAD", 1, 5, 100),
            new ZoneJurisdiction("LSPD","STAD", 0, 100, 100),

            new ZoneJurisdiction("LSPD","STRAW", 0, 100, 70),

            new ZoneJurisdiction("LSSD","TATAMO", 0, 100, 70),

            new ZoneJurisdiction("LSPA","TERMINA", 0, 95, 80),
            new ZoneJurisdiction("LSPD","TERMINA", 1, 5, 5),

            new ZoneJurisdiction("LSPD","TEXTI", 0, 100, 80),

            new ZoneJurisdiction("LSSD-CH","TONGVAH", 0, 100, 70),

            new ZoneJurisdiction("LSSD-VW","TONGVAV", 0, 100, 70),

            new ZoneJurisdiction("DPPD","VCANA", 0, 85, 70) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSPD","VCANA", 1, 10, 10) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("DPPD","VESP", 0, 85, 70) {CanSpawnPedestrianOfficers = true },
            new ZoneJurisdiction("LSPD","VESP", 1, 10, 10) {CanSpawnPedestrianOfficers = true },

            new ZoneJurisdiction("LSPD-VW","VINE", 0, 85, 70),
            new ZoneJurisdiction("LSSD-VW","VINE", 1, 10, 10),

            new ZoneJurisdiction("LSSD","WINDF", 0, 100, 70),

            new ZoneJurisdiction("LSPD-VW","WVINE", 0, 85, 70),
            new ZoneJurisdiction("LSSD-VW","WVINE", 1, 15, 10),

            new ZoneJurisdiction("SAPR","ZANCUDO", 0, 51, 20),
            new ZoneJurisdiction("BCSO","ZANCUDO", 1, 49, 65),

            new ZoneJurisdiction("LSPA","ZP_ORT", 0, 95, 80),
            new ZoneJurisdiction("LSPD","ZP_ORT", 1, 5, 5),

            new ZoneJurisdiction("BCSO","ZQ_UAR", 0, 85, 75),
            new ZoneJurisdiction("LSSD","ZQ_UAR", 1, 15, 5),

            new ZoneJurisdiction("LSSD-BC","GALLI", 0, 100, 100),

        };
    }
}



