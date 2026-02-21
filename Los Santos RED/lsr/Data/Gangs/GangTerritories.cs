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
    private readonly string GangTurfConfigFileName = "Plugins\\LosSantosRED\\GangTerritories.xml";
    private List<GangTerritory> GangTerritoriesList = new List<GangTerritory>();
    public GangTerritories(IGangs gangProvider)
    {
        GangProvider = gangProvider;
    }
    public void ReadConfig(string configName)
    {
        string fileName = string.IsNullOrEmpty(configName) ? "GangTerritories_*.xml" : $"GangTerritories_{configName}.xml";

        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles(fileName).Where(x => !x.Name.Contains("+")).OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null && !configName.Equals("Default"))
        {
            EntryPoint.WriteToConsole($"Loaded Gang Territories config  {ConfigFile.FullName}", 0);
            GangTerritoriesList = Serialization.DeserializeParams<GangTerritory>(ConfigFile.FullName);
        }
        else if (File.Exists(GangTurfConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Gang Territories config  {GangTurfConfigFileName}", 0);
            GangTerritoriesList = Serialization.DeserializeParams<GangTerritory>(GangTurfConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Gang Territories config found, creating default", 0);

            DefaultConfig();
            DefaultConfig_LosSantos2008();
            DefaultConfig_Simple();
            DefaultConfig_LibertyCity();
            DefaultConfig_LibertyCityLPP();
        }
        //Load Additive
        foreach (FileInfo fileInfo in LSRDirectory.GetFiles("GangTerritories+_*.xml").OrderByDescending(x => x.Name))
        {
            EntryPoint.WriteToConsole($"Loaded ADDITIVE Gang Territories config  {fileInfo.FullName}", 0);
            List<GangTerritory> additivePossibleItems = Serialization.DeserializeParams<GangTerritory>(fileInfo.FullName);
            GangTerritoriesList.RemoveAll(x => additivePossibleItems.Any(y => y.ZoneInternalGameName == x.ZoneInternalGameName));
            GangTerritoriesList.AddRange(additivePossibleItems);
        }
    }
    public void Setup()
    {
        foreach (Gang gang in GangProvider.AllGangs)
        {
            List<GangTerritory> totalTerritory = GangTerritoriesList.Where(x => x.GangID.Equals(gang.ID)).ToList();
            if (totalTerritory?.Any() == true) foreach (GangTerritory gt in totalTerritory) gt.Setup(gang);
        }
    }
    public Gang GetMainGang(string ZoneName)
    {
        if (GangTerritoriesList.Any())
        {
            foreach (GangTerritory gt in GangTerritoriesList.Where(x => x.ZoneInternalGameName.ToLower() == ZoneName.ToLower()).OrderBy(x => x.Priority))
            {
                Gang Gang = GangProvider.GetGang(gt.GangID);
                if (Gang != null)
                {
                    return Gang;
                }
            }
            return null;
        }
        return null;
    }
    public Gang GetNthGang(string ZoneName, int itemNumber)
    {
        if (GangTerritoriesList.Any())
        {
            foreach (GangTerritory gt in GangTerritoriesList.Where(x => x.ZoneInternalGameName.ToLower() == ZoneName.ToLower()).Skip(itemNumber - 1).OrderBy(x => x.Priority))
            {
                Gang Gang = GangProvider.GetGang(gt.GangID);
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
        if (GangTerritoriesList.Any())
        {
            List<GangTerritory> ToPickFrom = new List<GangTerritory>();
            foreach (GangTerritory gt in GangTerritoriesList.Where(x => x.ZoneInternalGameName.ToLower() == ZoneName.ToLower()))
            {
                Gang Gang = GangProvider.GetGang(gt.GangID);
                if (Gang != null && Gang.CanSpawn(WantedLevel))
                {
                    ToPickFrom.Add(gt);
                }
            }
            int Total = ToPickFrom.Sum(x => x.CurrentSpawnChance());
            int RandomPick = RandomItems.MyRand.Next(0, Total);
            foreach (GangTerritory gt in ToPickFrom)
            {
                int SpawnChance = gt.CurrentSpawnChance();
                if (RandomPick < SpawnChance)
                {
                    return GangProvider.GetGang(gt.GangID);
                }
                RandomPick -= SpawnChance;
            }
        }
        return null;
    }
    public List<Gang> GetGangs(string zoneName, int wantedLevel)
    {
        if (GangTerritoriesList.Any())
        {
            List<Gang> ToReturn = new List<Gang>();
            foreach (GangTerritory gt in GangTerritoriesList.Where(x => x.ZoneInternalGameName.ToLower() == zoneName.ToLower()).OrderBy(k => k.CurrentSpawnChance()))
            {
                Gang Gang = GangProvider.GetGang(gt.GangID);
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
    public List<GangTerritory> GetGangTerritory(string gangID)
    {
        if (GangTerritoriesList.Any())
        {
            List<GangTerritory> ToReturn = GangTerritoriesList.Where(x => x.GangID.ToLower() == gangID.ToLower()).ToList();
            return ToReturn;
        }
        else
        {
            return null;
        }
    }
    private void DefaultConfig()
    {
        GangTerritoriesList = new List<GangTerritory>()
        {
            //new GangTerritory("MRH","AIRP", 0, 100),
            new GangTerritory("AMBIENT_GANG_HILLBILLY","ALAMO", 0, 100),//Alamo Sea
            //new GangTerritory("LSMC","ALTA", 0, 100),
            //new GangTerritory("ARMY","ARMYB", 0, 100),
            //new GangTerritory("LSMC","BANHAMC", 0, 100),
            // new GangTerritory("AMBIENT_GANG_BALLAS","BANNING", 0, 50),//Banning
            //new GangTerritory("AMBIENT_GANG_MEXICAN","BANNING", 0, 100),//Banning
            //new GangTerritory("LSMC","BAYTRE", 0, 100),
            new GangTerritory("AMBIENT_GANG_YARDIES","BEACH", 0, 100),//Vespucci Beach
            //new GangTerritory("LSMC","BHAMCA", 0, 100),
            //new GangTerritory("LSMC","BRADP", 0, 100),
            //new GangTerritory("LSMC","BRADT", 0, 100),
            //new GangTerritory("AMBIENT_GANG_LUPISELLA","BURTON", 0, 100),//burton
            //new GangTerritory("LSMC","CALAFB", 0, 100),
            //new GangTerritory("LSMC","CANNY", 0, 100),
            //new GangTerritory("LSMC","CCREAK", 0, 100),
            new GangTerritory("AMBIENT_GANG_FAMILY","CHAMH", 0, 100),//chamberlain hills
            new GangTerritory("AMBIENT_GANG_MADRAZO","CHIL", 0, 100),//Vinewood Hills
            new GangTerritory("AMBIENT_GANG_ANCELOTTI","CHU", 0, 100),//Chumash
            //new GangTerritory("LSMC","CMSW", 0, 100),
            new GangTerritory("AMBIENT_GANG_MEXICAN","CYPRE", 0, 100),//Cypress Flats
            new GangTerritory("AMBIENT_GANG_MEXICAN","CYPRE2", 0, 100),//Cypress Flats
            new GangTerritory("AMBIENT_GANG_BALLAS","DAVIS", 0, 100),
            // new GangTerritory("AMBIENT_GANG_FAMILY","DAVIS", 1, 50),
            //new GangTerritory("LSMC","DELBE", 0, 100),
            //new GangTerritory("LSMC","DELPE", 0, 100),
            new GangTerritory("AMBIENT_GANG_YARDIES","DELSOL", 0, 100),//Puerto Del Sol
            new GangTerritory("AMBIENT_GANG_ANGELS","DESRT", 0, 100),//grande senora desert//new GangTerritory("AMBIENT_GANG_LOST","DESRT", 0, 100),//grande senora desert
            //new GangTerritory("MRH","DOWNT", 0, 100),
            new GangTerritory("AMBIENT_GANG_GAMBETTI","DTVINE", 0, 100),//Downtown Vinewood
            new GangTerritory("AMBIENT_GANG_LOST","EAST_V", 0, 100),//East Vinewood
            new GangTerritory("AMBIENT_GANG_MARABUNTE","EBURO", 0, 100),//el burro heights
            //new GangTerritory("LSMC","ELGORL", 0, 100),
            new GangTerritory("AMBIENT_GANG_DIABLOS","ELYSIAN", 0, 100),//elysian island
            //new GangTerritory("LSMC","GALFISH", 0, 100),
            //new GangTerritory("LSMC","GOLF", 0, 100),
            new GangTerritory("AMBIENT_GANG_PAVANO","GRAPES", 0, 100),//Grapeseed
            //new GangTerritory("LSMC","GREATC", 0, 100),
            new GangTerritory("AMBIENT_GANG_ANGELS","HARMO", 0, 100),
            //new GangTerritory("AMBIENT_GANG_LOST","HAWICK", 0, 100), // Hawick
            //new GangTerritory("LSMC","HORS", 0, 100),
            //new GangTerritory("LSMC","HUMLAB", 0, 100),
            //new GangTerritory("SASPA","JAIL", 0, 100),
            new GangTerritory("AMBIENT_GANG_KKANGPAE","KOREAT", 0, 100),//Little Seoul
            // new GangTerritory("AMBIENT_GANG_WEICHENG","KOREAT", 1, 20),//Little Seoul
            //new GangTerritory("LSMC","LACT", 0, 100),
            //new GangTerritory("LSMC","LAGO", 0, 100),
            //new GangTerritory("LSMC","LDAM", 0, 100),
            new GangTerritory("AMBIENT_GANG_WEICHENG","LEGSQU", 0, 100),//Legion Square
            new GangTerritory("AMBIENT_GANG_MADRAZO","LMESA", 0, 100),//La Mesa
            new GangTerritory("AMBIENT_GANG_ARMENIAN","LOSPUER", 0, 100),//La Puerta
            new GangTerritory("AMBIENT_GANG_LOST","MIRR", 0, 100),//mirror park
            //new GangTerritory("AMBIENT_GANG_ANCELOTTI","MORN", 0, 100),//Morningwood
            //new GangTerritory("LSMC","MOVIE", 0, 100),
            //new GangTerritory("LSMC","MTCHIL", 0, 100),
            //new GangTerritory("LSMC","MTGORDO", 0, 100),
            //new GangTerritory("LSMC","MTJOSE", 0, 100),
            new GangTerritory("AMBIENT_GANG_MARABUNTE","MURRI", 0, 100),//Murrieta Heights
            //new GangTerritory("AMBIENT_GANG_MESSINA","NCHU", 0, 100),//north chumash
            //new GangTerritory("LSMC","NOOSE", 0, 100),
            //new GangTerritory("LSMC","OCEANA", 0, 100),
            //new GangTerritory("LSMC","PALCOV", 0, 100),
            new GangTerritory("AMBIENT_GANG_LUPISELLA","PALETO", 0, 100),//Paleto Bay
            new GangTerritory("AMBIENT_GANG_LOST","PALFOR", 0, 100),//Paleto Forest
            //new GangTerritory("LSMC","PALHIGH", 0, 100),
            //new GangTerritory("LSMC","PALMPOW", 0, 100),
            //new GangTerritory("LSMC","PBLUFF", 0, 100),
            new GangTerritory("AMBIENT_GANG_WEICHENG","PBOX", 0, 100),//Pillbox Hill
            //new GangTerritory("LSMC","PROCOB", 0, 100),


            new GangTerritory("AMBIENT_GANG_MEXICAN","RANCHO", 1, 50),//south rancho?
            new GangTerritory("AMBIENT_GANG_SALVA","RANCHO", 0, 50),//north rancho?




            //new GangTerritory("AMBIENT_GANG_BALLAS","RANCHO", 1, 50),
            //new GangTerritory("LSMC","RGLEN", 0, 100),
            new GangTerritory("AMBIENT_GANG_MESSINA","RICHM", 0, 100),//Richman
            //new GangTerritory("AMBIENT_GANG_MESSINA","ROCKF", 0, 100),//Rockford Hills
            //new GangTerritory("LSMC","RTRAK", 0, 100),
            //new GangTerritory("AMBIENT_GANG_MEXICAN","SANAND", 0, 100),//broken zone in east LS
            //new GangTerritory("LSMC","SANCHIA", 0, 100),
            new GangTerritory("AMBIENT_GANG_HILLBILLY","SANDY", 0, 100),//Sandy Shores
            new GangTerritory("AMBIENT_GANG_BALLAS","SKID", 0, 100),//Mission Row
            new GangTerritory("AMBIENT_GANG_LOST","SLAB", 0, 100),//stab city aka slab city//new GangTerritory("AMBIENT_GANG_LOST","SLAB", 0, 100),//stab city aka slab city
            new GangTerritory("AMBIENT_GANG_SALVA","STAD", 0, 100),//Maze Bank
            new GangTerritory("AMBIENT_GANG_FAMILY","STRAW", 0, 100),//Strawberry
            //new GangTerritory("LSSD","TATAMO", 0, 100),
            //new GangTerritory("LSMC","TERMINA", 0, 100),
            new GangTerritory("AMBIENT_GANG_BALLAS","TEXTI", 0, 100),//Textile City
            //new GangTerritory("LSMC","TONGVAH", 0, 100),
            //new GangTerritory("LSMC","TONGVAV", 0, 100),
            new GangTerritory("AMBIENT_GANG_KKANGPAE","VCANA", 0, 100),//Vespucci Canals
            //new GangTerritory("LSMC","VESP", 0, 100),
            //new GangTerritory("LSMC","VINE", 0, 100),
            //new GangTerritory("LSMC","WINDF", 0, 100),
            new GangTerritory("AMBIENT_GANG_GAMBETTI","WVINE", 0, 100),//West Vinewood
            //new GangTerritory("SAPR","ZANCUDO", 0, 100),
            //new GangTerritory("LSMC","ZP_ORT", 0, 100),
            //new GangTerritory("LSMC","ZQ_UAR", 0, 100),
           //new GangTerritory("LSMC","GALLI", 0, 100),
        };
        Serialization.SerializeParams(GangTerritoriesList, GangTurfConfigFileName);
    }
    private void DefaultConfig_LibertyCity()
    {
        List<GangTerritory> LibertyGangTerritoriesList = new List<GangTerritory>()
        {
            //Lost (ONLY ALderney)
            new GangTerritory("AMBIENT_GANG_LOST","ACTRR", 0, 100),//acter
            new GangTerritory("AMBIENT_GANG_LOST","ACTIP", 0, 100),//acter indus
            new GangTerritory("AMBIENT_GANG_LOST","ALSCF", 0, 100),//acter prison
            new GangTerritory("AMBIENT_GANG_LOST","TUDOR", 0, 100),//tudor
          
            //Yardies (Only Alderney)
            new GangTerritory("AMBIENT_GANG_YARDIES","SCHOL", 0, 100),//schlotter
            new GangTerritory("AMBIENT_GANG_YARDIES","BEECW", 0, 100),//beachwood city
            new GangTerritory("AMBIENT_GANG_YARDIES","WILLI", 0, 100),//Willis

            //Korean Mob (Only Alderney)
            new GangTerritory("AMBIENT_GANG_KOREAN","ALDCI", 0, 100),//Alderny City
            new GangTerritory("AMBIENT_GANG_KOREAN","LEFWO", 0, 100),//Leftwood
            new GangTerritory("AMBIENT_GANG_KOREAN","WESDY", 0, 100),//Westdyke
            new GangTerritory("AMBIENT_GANG_KOREAN","BERCH", 0, 100),//bercham

            //Triads (South East Algonquin)
            new GangTerritory("AMBIENT_GANG_WEICHENG","CHITO", 0, 100),//CHinatown   
            new GangTerritory("AMBIENT_GANG_WEICHENG","THXCH", 0, 100),//the exchange
            new GangTerritory("AMBIENT_GANG_WEICHENG","FISSO", 0, 100),//fishmarket south

            //North Holland Hustlers (As Said)
            new GangTerritory("AMBIENT_GANG_HOLHUST","NOHOL", 0, 100),//North Holland

            //AOD North West Algonquin
            new GangTerritory("AMBIENT_GANG_ANGELS","VASIH", 0, 100),//Varsity Heights
            new GangTerritory("AMBIENT_GANG_ANGELS","MIDPW", 0, 100),//Middle Park West
            new GangTerritory("AMBIENT_GANG_ANGELS","NOHOL", 1, 100),//North Holland

            //Uptown Riders (only Northwood, maybe westdyke?)
            new GangTerritory("AMBIENT_GANG_UPTOWN","NORWO", 0, 100),//Northwood

            //Spanish Lords (in West Bohan)
            new GangTerritory("AMBIENT_GANG_SPANISH","STHBO", 0, 100),//South BOhan
            new GangTerritory("AMBIENT_GANG_SPANISH","FORSI", 0, 100),//Fortside
            new GangTerritory("AMBIENT_GANG_SPANISH","CHAPO", 0, 100),//Chase Point
            new GangTerritory("AMBIENT_GANG_SPANISH","BOULE", 0, 100),//Chase Point 

            //Mafia
            //Gambetti (Southern Algonquin (With Triads))
            new GangTerritory("AMBIENT_GANG_GAMBETTI","LITAL", 0, 100),//little italy
            new GangTerritory("AMBIENT_GANG_GAMBETTI","CITH", 0, 100),//city hall
            new GangTerritory("AMBIENT_GANG_GAMBETTI","SUFFO", 0, 100),//suffolk
            new GangTerritory("AMBIENT_GANG_GAMBETTI","THTRI", 0, 100),//the triangle
            new GangTerritory("AMBIENT_GANG_GAMBETTI","THPRES", 0, 100),//presidents city

            //Lupisella (Bohan (With Spanish Lords))
            new GangTerritory("AMBIENT_GANG_LUPISELLA","INSTI", 0, 100),//Industrial Bohan
            new GangTerritory("AMBIENT_GANG_LUPISELLA","LTBAY", 0, 100),//Little Bay Boahn
            new GangTerritory("AMBIENT_GANG_LUPISELLA","NRTGA", 0, 100),//Northern Gardens

            //Petrovic (Broker)
            new GangTerritory("AMBIENT_GANG_PETROVIC","HOBEH", 0, 100),//Hove Beach
            new GangTerritory("AMBIENT_GANG_PETROVIC","FIISL", 0, 100),//Firefly Island
            new GangTerritory("AMBIENT_GANG_PETROVIC","BEGGA", 0, 100),//Beachgate

            //Pavano
            new GangTerritory("AMBIENT_GANG_PAVANO","EAHOL", 0, 100),//East Holland
            new GangTerritory("AMBIENT_GANG_PAVANO","LANCA", 0, 100),//Laancaster
            new GangTerritory("AMBIENT_GANG_PAVANO","MIDPE", 0, 100),//Middle Park East

            //Messian 
            new GangTerritory("AMBIENT_GANG_MESSINA","STARJ", 0, 100),//Star Junction
            new GangTerritory("AMBIENT_GANG_MESSINA","PUGAT", 0, 100),//Purgatory
            new GangTerritory("AMBIENT_GANG_MESSINA","HATGA", 0, 100),//Hafton Gardens
            new GangTerritory("AMBIENT_GANG_MESSINA","LANCE", 0, 100),//Lancet
            new GangTerritory("AMBIENT_GANG_MESSINA","WESMI", 0, 100),//Westminster

            //Ancelotti
            new GangTerritory("AMBIENT_GANG_ANCELOTTI","NORMY", 0, 100),//Normandy
            new GangTerritory("AMBIENT_GANG_ANCELOTTI","PORTU", 0, 100),//port tudor
        };
        Serialization.SerializeParams(LibertyGangTerritoriesList, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LibertyConfigFolder}\\GangTerritories_{StaticStrings.LibertyConfigSuffix}.xml");
    }

    private void DefaultConfig_LibertyCityLPP()
    {
        List<GangTerritory> LibertyLPPGangTerritoriesList = new List<GangTerritory>()
        {
            //Lost (ONLY ALderney)
            new GangTerritory("AMBIENT_GANG_LOST","ACTRR", 0, 100),//acter
            new GangTerritory("AMBIENT_GANG_LOST","ACTIP", 0, 100),//acter indus
            new GangTerritory("AMBIENT_GANG_LOST","ALSCF", 0, 100),//acter prison
            new GangTerritory("AMBIENT_GANG_LOST","TUDOR", 0, 100),//tudor
            new GangTerritory("AMBIENT_GANG_LOST","BEECW", 0, 100),//beachwood city LOST have taken over in LPP
          
            //Yardies (Only Alderney)
            new GangTerritory("AMBIENT_GANG_YARDIES","SCHOL", 0, 100),//schlotter
            new GangTerritory("AMBIENT_GANG_YARDIES","BEECW", 0, 100),//beachwood city
            new GangTerritory("AMBIENT_GANG_YARDIES","WILLI", 0, 100),//Willis
            new GangTerritory("AMBIENT_GANG_YARDIES","MEADH", 0, 100),//Meadow Hills

            //Korean Mob (Only Alderney)
            new GangTerritory("AMBIENT_GANG_KOREAN","ALDCI", 0, 100),//Alderny City
            new GangTerritory("AMBIENT_GANG_KOREAN","LEFWO", 0, 100),//Leftwood
            new GangTerritory("AMBIENT_GANG_KOREAN","WESDY", 0, 100),//Westdyke
            new GangTerritory("AMBIENT_GANG_KOREAN","BERCH", 0, 100),//bercham

            //Triads (South East Algonquin)
            new GangTerritory("AMBIENT_GANG_WEICHENG","CHITO", 0, 100),//CHinatown   
            new GangTerritory("AMBIENT_GANG_WEICHENG","THXCH", 0, 100),//the exchange
            new GangTerritory("AMBIENT_GANG_WEICHENG","FISSO", 0, 100),//fishmarket south

            //North Holland Hustlers (As Said)
            new GangTerritory("AMBIENT_GANG_HOLHUST","NOHOL", 0, 100),//North Holland

            //AOD North West Algonquin
            new GangTerritory("AMBIENT_GANG_ANGELS","VASIH", 0, 100),//Varsity Heights
            new GangTerritory("AMBIENT_GANG_ANGELS","MIDPW", 0, 100),//Middle Park West
            new GangTerritory("AMBIENT_GANG_ANGELS","NOHOL", 1, 100),//North Holland

            //Uptown Riders (only Northwood, maybe westdyke?)
            new GangTerritory("AMBIENT_GANG_UPTOWN","NORWO", 0, 100),//Northwood

            //Spanish Lords (in West Bohan)
            new GangTerritory("AMBIENT_GANG_SPANISH","STHBO", 0, 100),//South BOhan
            new GangTerritory("AMBIENT_GANG_SPANISH","FORSI", 0, 100),//Fortside
            new GangTerritory("AMBIENT_GANG_SPANISH","CHAPO", 0, 100),//Chase Point
            new GangTerritory("AMBIENT_GANG_SPANISH","BOULE", 0, 100),//Chase Point 

            //Mafia
            //Gambetti (Southern Algonquin (With Triads))
            new GangTerritory("AMBIENT_GANG_GAMBETTI","LITAL", 0, 100),//little italy
            new GangTerritory("AMBIENT_GANG_GAMBETTI","CITH", 0, 100),//city hall
            new GangTerritory("AMBIENT_GANG_GAMBETTI","SUFFO", 0, 100),//suffolk
            new GangTerritory("AMBIENT_GANG_GAMBETTI","THTRI", 0, 100),//the triangle
            new GangTerritory("AMBIENT_GANG_GAMBETTI","THPRES", 0, 100),//presidents city

            //Lupisella (Bohan (With Spanish Lords))
            new GangTerritory("AMBIENT_GANG_LUPISELLA","INSTI", 0, 100),//Industrial Bohan
            new GangTerritory("AMBIENT_GANG_LUPISELLA","LTBAY", 0, 100),//Little Bay Boahn
            new GangTerritory("AMBIENT_GANG_LUPISELLA","NRTGA", 0, 100),//Northern Gardens

            //Petrovic (Broker)
            new GangTerritory("AMBIENT_GANG_PETROVIC","HOBEH", 0, 100),//Hove Beach
            new GangTerritory("AMBIENT_GANG_PETROVIC","FIISL", 0, 100),//Firefly Island
            new GangTerritory("AMBIENT_GANG_PETROVIC","BEGGA", 0, 100),//Beachgate

            //Pavano
            new GangTerritory("AMBIENT_GANG_PAVANO","EAHOL", 0, 100),//East Holland
            new GangTerritory("AMBIENT_GANG_PAVANO","LANCA", 0, 100),//Laancaster
            new GangTerritory("AMBIENT_GANG_PAVANO","MIDPE", 0, 100),//Middle Park East

            //Messian 
            new GangTerritory("AMBIENT_GANG_MESSINA","STARJ", 0, 100),//Star Junction
            new GangTerritory("AMBIENT_GANG_MESSINA","PUGAT", 0, 100),//Purgatory
            new GangTerritory("AMBIENT_GANG_MESSINA","HATGA", 0, 100),//Hafton Gardens
            new GangTerritory("AMBIENT_GANG_MESSINA","LANCE", 0, 100),//Lancet
            new GangTerritory("AMBIENT_GANG_MESSINA","WESMI", 0, 100),//Westminster

            //Ancelotti
            new GangTerritory("AMBIENT_GANG_ANCELOTTI","NORMY", 0, 100),//Normandy
            new GangTerritory("AMBIENT_GANG_ANCELOTTI","PORTU", 0, 100),//port tudor
        };
        Serialization.SerializeParams(LibertyLPPGangTerritoriesList, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LPPConfigFolder}\\GangTerritories_{StaticStrings.LPPConfigSuffix}.xml");

        //List<GangTerritory> LPPZoneJurisdiction = new List<GangTerritory>();
        //LPPZoneJurisdiction.AddRange(LibertyGangTerritoriesList);
        //LPPZoneJurisdiction.AddRange(GangTerritoriesList);
        //Serialization.SerializeParams(LPPZoneJurisdiction, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LPPConfigFolder}\\GangTerritories_{StaticStrings.LPPConfigSuffix}.xml");

    }

    private void DefaultConfig_LosSantos2008()
    {
        List<GangTerritory> OldGangTerritoriesList = new List<GangTerritory>()
        {
            new GangTerritory("AMBIENT_GANG_FAMILY", "CHAMH", 0, 100),//chamberlain hills
            new GangTerritory("AMBIENT_GANG_FAMILY", "STRAW", 0, 100),//Strawberry
            new GangTerritory("AMBIENT_GANG_FAMILY", "DAVIS", 0, 100),

            new GangTerritory("AMBIENT_GANG_HILLBILLY", "ALAMO", 0, 100),//Alamo Sea
            new GangTerritory("AMBIENT_GANG_HILLBILLY", "SANDY", 0, 100),//Sandy Shores

            new GangTerritory("AMBIENT_GANG_KKANGPAE", "VCANA", 0, 100),//Vespucci Canals
            new GangTerritory("AMBIENT_GANG_KKANGPAE", "KOREAT", 0, 100),//Little Seoul

            new GangTerritory("AMBIENT_GANG_MEXICAN", "CYPRE", 0, 100),//Cypress Flats
            new GangTerritory("AMBIENT_GANG_MEXICAN", "CYPRE2", 0, 100),//Cypress Flats
            new GangTerritory("AMBIENT_GANG_MEXICAN", "RANCHO", 1, 50),//south rancho?
            //new GangTerritory("AMBIENT_GANG_MEXICAN", "SANAND", 0, 100),//broken zone in east LS

            new GangTerritory("AMBIENT_GANG_ARMENIAN", "LOSPUER", 0, 100),//La Puerta

            new GangTerritory("AMBIENT_GANG_YARDIES", "BEACH", 0, 100),//Vespucci Beach
            new GangTerritory("AMBIENT_GANG_YARDIES", "DELSOL", 0, 100),//Puerto Del Sol

            new GangTerritory("AMBIENT_GANG_MADRAZO", "LMESA", 0, 100),//La Mesa
            new GangTerritory("AMBIENT_GANG_MADRAZO", "CHIL", 0, 100),//Vinewood Hills
                      
            new GangTerritory("AMBIENT_GANG_MARABUNTE", "EBURO", 0, 100),//el burro heights

            new GangTerritory("AMBIENT_GANG_DIABLOS", "ELYSIAN", 0, 100),//elysian island
            
            new GangTerritory("AMBIENT_GANG_WEICHENG", "LEGSQU", 0, 100),//Legion Square
            
            new GangTerritory("AMBIENT_GANG_LOST", "PALFOR", 0, 100),//Paleto Forest
            new GangTerritory("AMBIENT_GANG_LOST", "EAST_V", 0, 100),//East Vinewood
            new GangTerritory("AMBIENT_GANG_LOST", "DESRT", 0, 100),//grande senora desert
            new GangTerritory("AMBIENT_GANG_LOST", "MIRR", 0, 100),//mirror park
            new GangTerritory("AMBIENT_GANG_LOST", "SLAB", 0, 100),//stab city aka slab city

            new GangTerritory("AMBIENT_GANG_MARABUNTE", "MURRI", 0, 100),//Murrieta Heights
                
            new GangTerritory("AMBIENT_GANG_WEICHENG", "PBOX", 0, 100),//Pillbox Hill
            
            new GangTerritory("AMBIENT_GANG_SALVA", "RANCHO", 0, 50),//north rancho? 
            new GangTerritory("AMBIENT_GANG_SALVA", "STAD", 0, 100),//Maze Bank

            new GangTerritory("AMBIENT_GANG_GAMBETTI", "WVINE", 0, 100),//West Vinewood
            new GangTerritory("AMBIENT_GANG_GAMBETTI", "DTVINE", 0, 100),//Downtown Vinewood
            //new GangTerritory("AMBIENT_GANG_PAVANO", "GRAPES", 0, 100),//Grapeseed
            //new GangTerritory("AMBIENT_GANG_LUPISELLA", "PALETO", 0, 100),//Paleto Bay
            //new GangTerritory("AMBIENT_GANG_MESSINA", "RICHM", 0, 100),//Richman
            //new GangTerritory("AMBIENT_GANG_ANCELOTTI", "CHU", 0, 100),//Chumash

            new GangTerritory("AMBIENT_GANG_BALLAS", "SKID", 0, 100),//Mission Row
            new GangTerritory("AMBIENT_GANG_BALLAS", "TEXTI", 0, 100),//Textile City

        };
        Serialization.SerializeParams(OldGangTerritoriesList, "Plugins\\LosSantosRED\\AlternateConfigs\\LosSantos2008\\GangTerritories_LosSantos2008.xml");
    }
    private void DefaultConfig_Simple()
    {
        List<GangTerritory> SimpleGangTerritoriesList = new List<GangTerritory>()
        {
            new GangTerritory("AMBIENT_GANG_HILLBILLY","ALAMO", 0, 100),//Alamo Sea
            new GangTerritory("AMBIENT_GANG_FAMILY","CHAMH", 0, 100),//chamberlain hills
            new GangTerritory("AMBIENT_GANG_MADRAZO","CHIL", 0, 100),//Vinewood Hills
            new GangTerritory("AMBIENT_GANG_MEXICAN","CYPRE", 0, 100),//Cypress Flats
            new GangTerritory("AMBIENT_GANG_MEXICAN","CYPRE2", 0, 100),//Cypress Flats
            new GangTerritory("AMBIENT_GANG_BALLAS","DAVIS", 0, 100),
            new GangTerritory("AMBIENT_GANG_LOST","DESRT", 0, 100),//grande senora desert
            new GangTerritory("AMBIENT_GANG_LOST","EAST_V", 0, 100),//East Vinewood
            new GangTerritory("AMBIENT_GANG_MARABUNTE","EBURO", 0, 100),//el burro heights
            new GangTerritory("AMBIENT_GANG_KKANGPAE","KOREAT", 0, 100),//Little Seoul
            new GangTerritory("AMBIENT_GANG_WEICHENG","LEGSQU", 0, 100),//Legion Square
            new GangTerritory("AMBIENT_GANG_MADRAZO","LMESA", 0, 100),//La Mesa
            new GangTerritory("AMBIENT_GANG_ARMENIAN","LOSPUER", 0, 100),//La Puerta
            new GangTerritory("AMBIENT_GANG_LOST","MIRR", 0, 100),//mirror park
            new GangTerritory("AMBIENT_GANG_MARABUNTE","MURRI", 0, 100),//Murrieta Heights
            new GangTerritory("AMBIENT_GANG_LOST","PALFOR", 0, 100),//Paleto Forest
            new GangTerritory("AMBIENT_GANG_WEICHENG","PBOX", 0, 100),//Pillbox Hill

            //new GangTerritory("AMBIENT_GANG_SALVA","RANCHO", 0, 100),
            new GangTerritory("AMBIENT_GANG_MEXICAN","RANCHO", 1, 50),//south rancho?
            new GangTerritory("AMBIENT_GANG_SALVA","RANCHO", 0, 50),//north rancho?

            new GangTerritory("AMBIENT_GANG_MESSINA","RICHM", 0, 100),//Richman
            //new GangTerritory("AMBIENT_GANG_MEXICAN","SANAND", 0, 100),//broken zone in east LS
            new GangTerritory("AMBIENT_GANG_HILLBILLY","SANDY", 0, 100),//Sandy Shores
            new GangTerritory("AMBIENT_GANG_BALLAS","SKID", 0, 100),//Mission Row
            new GangTerritory("AMBIENT_GANG_LOST","SLAB", 0, 100),//stab city aka slab city
            new GangTerritory("AMBIENT_GANG_SALVA","STAD", 0, 100),//Maze Bank
            new GangTerritory("AMBIENT_GANG_FAMILY","STRAW", 0, 100),//Strawberry
            new GangTerritory("AMBIENT_GANG_BALLAS","TEXTI", 0, 100),//Textile City
            new GangTerritory("AMBIENT_GANG_KKANGPAE","VCANA", 0, 100),//Vespucci Canals
        };
        Serialization.SerializeParams(SimpleGangTerritoriesList, "Plugins\\LosSantosRED\\AlternateConfigs\\Simple\\GangTerritories_Simple.xml");
    }
}



