using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class CountyJurisdictionManager
{
    private static readonly string ConfigFileName = "Plugins\\LosSantosRED\\CountyJurisdiction.xml";
    private static List<CountyJurisdiction> CountyJurisdictions = new List<CountyJurisdiction>();
    private static bool UseVanillaConfig = true;
    public static void Initialize()
    {
        if (File.Exists(ConfigFileName))
        {
            CountyJurisdictions = SettingsManager.DeserializeParams<CountyJurisdiction>(ConfigFileName);
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
            SettingsManager.SerializeParams(CountyJurisdictions, ConfigFileName);
        }
    }
    public static Agency GetRandomAgency(string ZoneName)
    {
        Zone MyZone = ZoneManager.GetZone(ZoneName);
        if (MyZone != null)
        {
            List<CountyJurisdiction> ToPickFrom = CountyJurisdictions.Where(x => x.County == MyZone.ZoneCounty && x.GameAgency.CanSpawn).ToList();
            int Total = ToPickFrom.Sum(x => x.CurrentSpawnChance);
            int RandomPick = RandomItems.MyRand.Next(0, Total);
            foreach (CountyJurisdiction MyJurisdiction in ToPickFrom)
            {
                int SpawnChance = MyJurisdiction.CurrentSpawnChance;
                if (RandomPick < SpawnChance)
                {
                    return MyJurisdiction.GameAgency;
                }
                RandomPick -= SpawnChance;
            }
        }
        return null;
    }
    private static void DefaultConfig()
    {
        CountyJurisdictions = new List<CountyJurisdiction>()
        {
            new CountyJurisdiction("LSPD-ASD",County.CityOfLosSantos, 0, 100, 100),
            new CountyJurisdiction("LSSD-ASD",County.BlaineCounty, 0, 100, 100),
            new CountyJurisdiction("LSSD-ASD",County.LosSantosCounty, 0, 100, 100),
        };
    }
    private static void CustomConfig()
    {
        CountyJurisdictions = new List<CountyJurisdiction>()
        {
            new CountyJurisdiction("LSPD-ASD",County.CityOfLosSantos, 0, 100, 100),
            new CountyJurisdiction("LSSD-ASD",County.BlaineCounty, 0, 100, 100),
            new CountyJurisdiction("LSSD-ASD",County.LosSantosCounty, 0, 100, 100),
        };
    }
}



