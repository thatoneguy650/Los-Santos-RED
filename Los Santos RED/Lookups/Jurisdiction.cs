using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static partial class Jurisdiction
{
    private static string ConfigFileName = "Plugins\\LosSantosRED\\Jurisdiction.xml";
    private static List<ZoneJurisdiction> ZoneJurisdictions = new List<ZoneJurisdiction>();
    private static List<CountyJurisdiction> CountyJurisdictions = new List<CountyJurisdiction>();

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
            ZoneJurisdictions = General.DeserializeParams<ZoneJurisdiction>(ConfigFileName);
        }
        else
        {
            DefaultConfig();
            General.SerializeParams(ZoneJurisdictions, ConfigFileName);
        }
    }
    private static void DefaultConfig()
    {
        CountyJurisdictions = new List<CountyJurisdiction>()
        {
            new CountyJurisdiction("LSPD-ASD",Zone.County.CityOfLosSantos, 0, 100, 100),
            new CountyJurisdiction("LSSD-ASD",Zone.County.BlaineCounty, 0, 100, 100),
            new CountyJurisdiction("LSSD-ASD",Zone.County.LosSantosCounty, 0, 100, 100),
        };

        ZoneJurisdictions = new List<ZoneJurisdiction>()
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

            new ZoneJurisdiction("PRISEC","STAD", 0, 95, 0),
            new ZoneJurisdiction("LSPD","STAD", 1, 5, 100),

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
            new ZoneJurisdiction("LSSD","ZQ_UAR", 1, 15, 5)

        };
    }
    public static Agency MainAgencyAtZone(string ZoneName)
    {
        if (ZoneJurisdictions.Any())
        {
            ZoneJurisdiction cool = ZoneJurisdictions.Where(x => x.ZoneInternalGameName.ToLower() == ZoneName.ToLower()).OrderBy(y => y.Priority).FirstOrDefault();

            if (cool != null)
                return cool.GameAgency;
            else
                return null;
        }
        else
            return null;
    }
    public static Agency AgencyAtZone(string ZoneName)
    {
        if (ZoneJurisdictions.Any())
        {
            List<ZoneJurisdiction> ToPickFrom = ZoneJurisdictions.Where(x => x.ZoneInternalGameName.ToLower() == ZoneName.ToLower() && x.GameAgency.CanSpawn).ToList();
            int Total = ToPickFrom.Sum(x => x.CurrentSpawnChance);
            int RandomPick = General.MyRand.Next(0, Total);
            foreach (ZoneJurisdiction MyJurisdiction in ToPickFrom)
            {
                int SpawnChance = MyJurisdiction.CurrentSpawnChance;
                if (RandomPick < SpawnChance)
                {
                    return MyJurisdiction.GameAgency;
                }
                RandomPick -= SpawnChance;
            }
            return null;
        }
        else
        {
            return null;
        }
    }
    public static bool CanSpawnPedestrainOfficersAtZone(string zoneInternalName,string agencyInitials)
    {
        ZoneJurisdiction ToFind = ZoneJurisdictions.FirstOrDefault(x => x.GameAgency.Initials == agencyInitials && x.GameZone.InternalGameName == zoneInternalName);
        if (ToFind == null)
            return false;
        else
            return ToFind.CanSpawnPedestrianOfficers;
    }
    public static Agency AgencyAtCounty(string ZoneName)
    {
        Zone MyZone = Zones.GetZoneByName(ZoneName);
        if (MyZone != null)
        {
            List<CountyJurisdiction> ToPickFrom = CountyJurisdictions.Where(x => x.County == MyZone.ZoneCounty && x.GameAgency.CanSpawn).ToList();
            int Total = ToPickFrom.Sum(x => x.CurrentSpawnChance);
            int RandomPick = General.MyRand.Next(0, Total);
            foreach (CountyJurisdiction MyJurisdiction in ToPickFrom)
            {
                int SpawnChance = MyJurisdiction.CurrentSpawnChance;
                if (RandomPick < SpawnChance)
                {
                    return MyJurisdiction.GameAgency;
                }
                RandomPick -= SpawnChance;
            }
            return null;
            
        }
        else
        {
            return null;
        }
    }
    public static List<Agency> GetAgenciesAtZone(string ZoneName)
    {
        if (ZoneJurisdictions.Any())
            return ZoneJurisdictions.Where(x => x.ZoneInternalGameName.ToLower() == ZoneName.ToLower() && x.GameAgency.CanSpawn).OrderBy(k => k.CurrentSpawnChance).Select(y => y.GameAgency).ToList();
        else
            return null;
    }
    public class ZoneJurisdiction
    {
        public string AgencyInitials { get; set; } = "";
        public string ZoneInternalGameName { get; set; } = "";
        public int Priority { get; set; } = 99;
        public int AmbientSpawnChance { get; set; } = 0;
        public int WantedSpawnChance { get; set; } = 0;
        public bool CanSpawnPedestrianOfficers { get; set; } = false;
        public Zone GameZone
        {
            get
            {
                return Zones.GetZoneByName(ZoneInternalGameName);
            }
        }
        public Agency GameAgency
        {
            get
            {
                return Agencies.GetAgencyFromInitials(AgencyInitials);
            }
        }
        public ZoneJurisdiction()
        {

        }
        public ZoneJurisdiction(string agencyInitials, string zoneInternalName, int priority, int ambientSpawnChance, int wantedSpawnChance)
        {
            AgencyInitials = agencyInitials;
            ZoneInternalGameName = zoneInternalName;
            Priority = priority;
            AmbientSpawnChance = ambientSpawnChance;
            WantedSpawnChance = wantedSpawnChance;
        }
        public bool CanCurrentlySpawn
        {
            get
            {
                if (PlayerState.IsWanted)
                    return WantedSpawnChance > 0;
                else
                    return AmbientSpawnChance > 0;
            }
        }
        public int CurrentSpawnChance
        {
            get
            {
                if (PlayerState.IsWanted)
                    return WantedSpawnChance;
                else
                    return AmbientSpawnChance;
            }
        }
    }
    public class CountyJurisdiction
    {
        public string AgencyInitials { get; set; } = "";
        public Zone.County County { get; set; }
        public int Priority { get; set; } = 99;
        public int AmbientSpawnChance { get; set; } = 0;
        public int WantedSpawnChance { get; set; } = 0;
        public bool CanSpawnPedestrianOfficers { get; set; } = false;
        public Agency GameAgency
        {
            get
            {
                return Agencies.GetAgencyFromInitials(AgencyInitials);
            }
        }
        public CountyJurisdiction()
        {

        }
        public CountyJurisdiction(string agencyInitials, Zone.County county, int priority, int ambientSpawnChance, int wantedSpawnChance)
        {
            AgencyInitials = agencyInitials;
            County = county;
            Priority = priority;
            AmbientSpawnChance = ambientSpawnChance;
            WantedSpawnChance = wantedSpawnChance;
        }
        public bool CanCurrentlySpawn
        {
            get
            {
                if (PlayerState.IsWanted)
                    return WantedSpawnChance > 0;
                else
                    return AmbientSpawnChance > 0;
            }
        }
        public int CurrentSpawnChance
        {
            get
            {
                if (PlayerState.IsWanted)
                    return WantedSpawnChance;
                else
                    return AmbientSpawnChance;
            }
        }
    }
}

    

