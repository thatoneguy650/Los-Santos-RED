using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CountyJurisdictions : ICountyJurisdictions
{
    private IAgencies AgencyProvider;
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\CountyJurisdiction.xml";
    private List<CountyJurisdiction> CountyJurisdictionList = new List<CountyJurisdiction>();
    private bool UseVanillaConfig = true;

    public CountyJurisdictions(IAgencies agencyProvider)
    {
        AgencyProvider = agencyProvider;
    }
    public void ReadConfig()
    {
        if (File.Exists(ConfigFileName))
        {
            CountyJurisdictionList = Serialization.DeserializeParams<CountyJurisdiction>(ConfigFileName);
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
            Serialization.SerializeParams(CountyJurisdictionList, ConfigFileName);
        }
    }
    public Agency GetRandomAgency(County county, int WantedLevel)//was zone, instead take county
    {
        List<CountyJurisdiction> ToPickFrom = new List<CountyJurisdiction>();
        foreach (CountyJurisdiction countyJurisdiction in CountyJurisdictionList.Where(x => x.County == county))
        {
            Agency Agency = AgencyProvider.GetAgency(countyJurisdiction.AgencyInitials);
            if (Agency != null && Agency.CanSpawn(WantedLevel))
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
    private void DefaultConfig()
    {
        CountyJurisdictionList = new List<CountyJurisdiction>()
        {
            new CountyJurisdiction("LSPD-ASD",County.CityOfLosSantos, 0, 100, 100),
            new CountyJurisdiction("LSSD-ASD",County.BlaineCounty, 0, 100, 100),
            new CountyJurisdiction("LSSD-ASD",County.LosSantosCounty, 0, 100, 100),
            new CountyJurisdiction("APD-ASD", County.Crook, 0, 100, 100),
            new CountyJurisdiction("NYSP", County.NorthYankton, 0, 100, 100)
        };
    }
    private void CustomConfig()
    {
        CountyJurisdictionList = new List<CountyJurisdiction>()
        {
            new CountyJurisdiction("LSPD-ASD",County.CityOfLosSantos, 0, 100, 100),
            new CountyJurisdiction("LSSD-ASD",County.BlaineCounty, 0, 100, 100),
            new CountyJurisdiction("LSSD-ASD",County.LosSantosCounty, 0, 100, 100),
        };
    }
}



