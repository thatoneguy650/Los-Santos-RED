using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GangTerritories : IGangTerritories
{
    private IGangs GangProvider;
    private readonly string ZoneConfigFileName = "Plugins\\LosSantosRED\\GangTerritories.xml";
    private List<ZoneJurisdiction> ZoneJurisdictionsList = new List<ZoneJurisdiction>();
    public GangTerritories(IGangs gangProvider)
    {
        GangProvider = gangProvider;
    }
    public void ReadConfig()
    {
        if (File.Exists(ZoneConfigFileName))
        {
            ZoneJurisdictionsList = Serialization.DeserializeParams<ZoneJurisdiction>(ZoneConfigFileName);
        }
        else
        {
            DefaultZoneConfig();
            Serialization.SerializeParams(ZoneJurisdictionsList, ZoneConfigFileName);
        }
    }
    public Gang GetMainGang(string ZoneName)
    {
        if (ZoneJurisdictionsList.Any())
        {
            foreach (ZoneJurisdiction zoneJurisdiction in ZoneJurisdictionsList.Where(x => x.ZoneInternalGameName.ToLower() == ZoneName.ToLower()).OrderBy(x => x.Priority))
            {
                Gang Agency = GangProvider.GetGang(zoneJurisdiction.AgencyInitials);
                if (Agency != null)
                {
                    return Agency;
                }
            }
            return null;
        }
        return null;
    }
    public Gang GetNthGang(string ZoneName, int itemNumber)
    {
        if (ZoneJurisdictionsList.Any())
        {
            foreach (ZoneJurisdiction zoneJurisdiction in ZoneJurisdictionsList.Where(x => x.ZoneInternalGameName.ToLower() == ZoneName.ToLower()).Skip(itemNumber - 1).OrderBy(x => x.Priority))
            {
                Gang Gang = GangProvider.GetGang(zoneJurisdiction.AgencyInitials);
                if (Gang != null)
                {
                    return Gang;
                }
            }
            return null;
        }
        return null;
    }
    public Gang GetRandomGang(string ZoneName, int WantedLevel)
    {
        if (ZoneJurisdictionsList.Any())
        {
            List<ZoneJurisdiction> ToPickFrom = new List<ZoneJurisdiction>();
            foreach (ZoneJurisdiction zoneJurisdiction in ZoneJurisdictionsList.Where(x => x.ZoneInternalGameName.ToLower() == ZoneName.ToLower()))
            {
                Gang Gang = GangProvider.GetGang(zoneJurisdiction.AgencyInitials);
                if (Gang != null && Gang.CanSpawn(WantedLevel))
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
                    return GangProvider.GetGang(MyJurisdiction.AgencyInitials);
                }
                RandomPick -= SpawnChance;
            }
        }
        return null;
    }
    public List<Gang> GetGangs(string zoneName, int wantedLevel)
    {
        if (ZoneJurisdictionsList.Any())
        {
            List<Gang> ToReturn = new List<Gang>();
            foreach (ZoneJurisdiction zoneJurisdiction in ZoneJurisdictionsList.Where(x => x.ZoneInternalGameName.ToLower() == zoneName.ToLower()).OrderBy(k => k.CurrentSpawnChance(wantedLevel)))
            {
                Gang Gang = GangProvider.GetGang(zoneJurisdiction.AgencyInitials);
                if (Gang != null && Gang.CanSpawn(wantedLevel))
                {
                    ToReturn.Add(Gang);
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
    public List<ZoneJurisdiction> GetGangTerritory(string gangID)
    {
        if (ZoneJurisdictionsList.Any())
        {
            List<ZoneJurisdiction> ToReturn = ZoneJurisdictionsList.Where(x => x.AgencyInitials.ToLower() == gangID.ToLower()).ToList();
            //foreach (ZoneJurisdiction zoneJurisdiction in ZoneJurisdictionsList.Where(x => x.AgencyInitials.ToLower() == gangID.ToLower()))
            //{
            //    Gang Gang = GangProvider.GetGang(zoneJurisdiction.AgencyInitials);
            //    if (Gang != null && Gang.CanSpawn(wantedLevel))
            //    {
            //        ToReturn.Add(Gang);
            //    }
            //}
            //if (!ToReturn.Any())
            //{
            //    return null;
            //}
            return ToReturn;
        }
        else
        {
            return null;
        }
    }
    private void DefaultZoneConfig()
    {
        ZoneJurisdictionsList = new List<ZoneJurisdiction>()
        {
            //new ZoneJurisdiction("MRH","AIRP", 0, 100, 100),

            //new ZoneJurisdiction("LSMC","ALAMO", 0, 100, 100),

            //new ZoneJurisdiction("LSMC","ALTA", 0, 100, 100),

            //new ZoneJurisdiction("ARMY","ARMYB", 0, 100, 100),

            //new ZoneJurisdiction("LSMC","BANHAMC", 0, 100, 100),

           // new ZoneJurisdiction("AMBIENT_GANG_BALLAS","BANNING", 0, 50, 50),//Banning
            //new ZoneJurisdiction("AMBIENT_GANG_SALVA","BANNING", 1, 50, 50),//Banning

            //new ZoneJurisdiction("LSMC","BAYTRE", 0, 100, 100),

            new ZoneJurisdiction("AMBIENT_GANG_YARDIES","BEACH", 0, 100, 100),//Vespucci Beach

            //new ZoneJurisdiction("LSMC","BHAMCA", 0, 100, 100),

            //new ZoneJurisdiction("LSMC","BRADP", 0, 100, 100),

            //new ZoneJurisdiction("LSMC","BRADT", 0, 100, 100),

            //new ZoneJurisdiction("AMBIENT_GANG_LUPISELLA","BURTON", 0, 100, 100),//burton

            //new ZoneJurisdiction("LSMC","CALAFB", 0, 100, 100),

            //new ZoneJurisdiction("LSMC","CANNY", 0, 100, 100),

            //new ZoneJurisdiction("LSMC","CCREAK", 0, 100, 100),

            new ZoneJurisdiction("AMBIENT_GANG_FAMILY","CHAMH", 0, 100, 100),//chamberlain hills

            //new ZoneJurisdiction("LSMC","CHIL", 0, 100, 100),

            new ZoneJurisdiction("AMBIENT_GANG_ANCELOTTI","CHU", 0, 100, 100),//Chumash

            //new ZoneJurisdiction("LSMC","CMSW", 0, 100, 100),

            new ZoneJurisdiction("AMBIENT_GANG_MEXICAN","CYPRE", 0, 100, 100),//Cypress Flats

            new ZoneJurisdiction("AMBIENT_GANG_BALLAS","DAVIS", 0, 100, 100),
           // new ZoneJurisdiction("AMBIENT_GANG_FAMILY","DAVIS", 1, 50, 50),
 
            //new ZoneJurisdiction("LSMC","DELBE", 0, 100, 100),

            //new ZoneJurisdiction("LSMC","DELPE", 0, 100, 100),

            //new ZoneJurisdiction("LSMC","DELSOL", 0, 100, 100),

           new ZoneJurisdiction("AMBIENT_GANG_LOST","DESRT", 0, 100, 100),//grande senora desert

            //new ZoneJurisdiction("MRH","DOWNT", 0, 100, 100),

            new ZoneJurisdiction("AMBIENT_GANG_GAMBETTI","DTVINE", 0, 100, 100),//Downtown Vinewood

            new ZoneJurisdiction("AMBIENT_GANG_LOST","EAST_V", 0, 100, 100),//East Vinewood

            new ZoneJurisdiction("AMBIENT_GANG_MARABUNTE","EBURO", 0, 100, 100),//el burro heights

            //new ZoneJurisdiction("LSMC","ELGORL", 0, 100, 100),

            new ZoneJurisdiction("AMBIENT_GANG_DIABLOS","ELYSIAN", 0, 100, 100),//elysian island

            //new ZoneJurisdiction("LSMC","GALFISH", 0, 100, 100),

            //new ZoneJurisdiction("LSMC","GOLF", 0, 100, 100),

            new ZoneJurisdiction("AMBIENT_GANG_PAVANO","GRAPES", 0, 100, 100),//Grapeseed

            //new ZoneJurisdiction("LSMC","GREATC", 0, 100, 100),

            //new ZoneJurisdiction("LSMC","HARMO", 0, 100, 100),

            //new ZoneJurisdiction("AMBIENT_GANG_LOST","HAWICK", 0, 100, 100) {CanSpawnPedestrianOfficers = true },//Hawikc

            //new ZoneJurisdiction("LSMC","HORS", 0, 100, 100),

            //new ZoneJurisdiction("LSMC","HUMLAB", 0, 100, 100),

            //new ZoneJurisdiction("SASPA","JAIL", 0, 100, 100),

            new ZoneJurisdiction("AMBIENT_GANG_KKANGPAE","KOREAT", 0, 100, 100),//Little Seoul
           // new ZoneJurisdiction("AMBIENT_GANG_WEICHENG","KOREAT", 1, 20, 20),//Little Seoul


            

            //new ZoneJurisdiction("LSMC","LACT", 0, 100, 100),

            //new ZoneJurisdiction("LSMC","LAGO", 0, 100, 100),

            //new ZoneJurisdiction("LSMC","LDAM", 0, 100, 100),

            new ZoneJurisdiction("AMBIENT_GANG_WEICHENG","LEGSQU", 0, 100, 100),//Legion Square

            new ZoneJurisdiction("AMBIENT_GANG_MADRAZO","LMESA", 0, 100, 100),//La Mesa

            new ZoneJurisdiction("AMBIENT_GANG_ARMENIAN","LOSPUER", 0, 100, 100),//La Puerta

            new ZoneJurisdiction("AMBIENT_GANG_LOST","MIRR", 0, 100, 100),//mirror park

            //new ZoneJurisdiction("AMBIENT_GANG_ANCELOTTI","MORN", 0, 100, 100),//Morningwood

            //new ZoneJurisdiction("LSMC","MOVIE", 0, 100, 100),

            //new ZoneJurisdiction("LSMC","MTCHIL", 0, 100, 100),

            //new ZoneJurisdiction("LSMC","MTGORDO", 0, 100, 100),

            //new ZoneJurisdiction("LSMC","MTJOSE", 0, 100, 100),

            new ZoneJurisdiction("AMBIENT_GANG_MARABUNTE","MURRI", 0, 100, 100),//Murrieta Heights

            //new ZoneJurisdiction("AMBIENT_GANG_MESSINA","NCHU", 0, 100, 100),//north chumash

            //new ZoneJurisdiction("LSMC","NOOSE", 0, 100, 100),

            //new ZoneJurisdiction("LSMC","OCEANA", 0, 100, 100),

            //new ZoneJurisdiction("LSMC","PALCOV", 0, 100, 100),

            new ZoneJurisdiction("AMBIENT_GANG_LUPISELLA","PALETO", 0, 100, 100),//Paleto Bay

            new ZoneJurisdiction("AMBIENT_GANG_LOST","PALFOR", 0, 100, 100),//Paleto Forest

            //new ZoneJurisdiction("LSMC","PALHIGH", 0, 100, 100),

            //new ZoneJurisdiction("LSMC","PALMPOW", 0, 100, 100),

            //new ZoneJurisdiction("LSMC","PBLUFF", 0, 100, 100),

            new ZoneJurisdiction("AMBIENT_GANG_WEICHENG","PBOX", 0, 100, 100),//Pillbox Hill

            //new ZoneJurisdiction("LSMC","PROCOB", 0, 100, 100),

            new ZoneJurisdiction("AMBIENT_GANG_SALVA","RANCHO", 0, 100, 100),
            //new ZoneJurisdiction("AMBIENT_GANG_BALLAS","RANCHO", 1, 50, 50),

            //new ZoneJurisdiction("LSMC","RGLEN", 0, 100, 100),

            new ZoneJurisdiction("AMBIENT_GANG_MESSINA","RICHM", 0, 100, 100),//Richman

            //new ZoneJurisdiction("AMBIENT_GANG_MESSINA","ROCKF", 0, 100, 100),//Rockford Hills

            //new ZoneJurisdiction("LSMC","RTRAK", 0, 100, 100),

            new ZoneJurisdiction("AMBIENT_GANG_MEXICAN","SANAND", 0, 100, 100),//broken zone in east LS

            //new ZoneJurisdiction("LSMC","SANCHIA", 0, 100, 100),

            new ZoneJurisdiction("AMBIENT_GANG_SALVA","SANDY", 0, 100, 100),//Sandy Shores

            new ZoneJurisdiction("AMBIENT_GANG_BALLAS","SKID", 0, 100, 100),//Mission Row

            new ZoneJurisdiction("AMBIENT_GANG_LOST","SLAB", 0, 100, 100),//stab city aka slab city

            new ZoneJurisdiction("AMBIENT_GANG_SALVA","STAD", 0, 100, 100),//Maze Bank

            new ZoneJurisdiction("AMBIENT_GANG_FAMILY","STRAW", 0, 100, 100),//Strawberry

            //new ZoneJurisdiction("LSSD","TATAMO", 0, 100, 100),

            //new ZoneJurisdiction("LSMC","TERMINA", 0, 100, 100),

            new ZoneJurisdiction("AMBIENT_GANG_BALLAS","TEXTI", 0, 100, 100),//Textile City

            //new ZoneJurisdiction("LSMC","TONGVAH", 0, 100, 100),

            //new ZoneJurisdiction("LSMC","TONGVAV", 0, 100, 100),

            new ZoneJurisdiction("AMBIENT_GANG_KKANGPAE","VCANA", 0, 100, 100),//Vespucci Canals

            //new ZoneJurisdiction("LSMC","VESP", 0, 100, 100),

            //new ZoneJurisdiction("LSMC","VINE", 0, 100, 100),

            //new ZoneJurisdiction("LSMC","WINDF", 0, 100, 100),

            new ZoneJurisdiction("AMBIENT_GANG_GAMBETTI","WVINE", 0, 100, 100),//West Vinewood

            //new ZoneJurisdiction("SAPR","ZANCUDO", 0, 100, 100),

            //new ZoneJurisdiction("LSMC","ZP_ORT", 0, 100, 100),

            //new ZoneJurisdiction("LSMC","ZQ_UAR", 0, 100, 100),

            //new ZoneJurisdiction("LSMC","GALLI", 0, 100, 100),

        };
    }
}



