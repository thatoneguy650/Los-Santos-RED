using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CountyJurisdictions
{
    private IZoneAgencyProvider DataMart;
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\CountyJurisdiction.xml";
    private List<CountyJurisdiction> CountyJurisdictionList = new List<CountyJurisdiction>();
    private bool UseVanillaConfig = true;

    public CountyJurisdictions(IZoneAgencyProvider dataMart)
    {
        DataMart = dataMart;
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
    public Agency GetRandomAgency(string ZoneName, int WantedLevel)
    {
        Zone MyZone = DataMart.Zones.GetZone(ZoneName);
        if (MyZone != null)
        {
            List<CountyJurisdiction> ToPickFrom = new List<CountyJurisdiction>();
            foreach (CountyJurisdiction countyJurisdiction in CountyJurisdictionList.Where(x => x.County == MyZone.ZoneCounty))
            {
                Agency Agency = DataMart.Agencies.GetAgency(countyJurisdiction.AgencyInitials);
                if (Agency != null && Agency.CanSpawn(WantedLevel))
                {
                    ToPickFrom.Add(countyJurisdiction);
                }
            }
            //List<CountyJurisdiction> ToPickFrom = CountyJurisdictionList.Where(x => x.County == MyZone.ZoneCounty && x.GameAgency.CanSpawn(WantedLevel)).ToList();
            int Total = ToPickFrom.Sum(x => x.CurrentSpawnChance(WantedLevel));
            int RandomPick = RandomItems.MyRand.Next(0, Total);
            foreach (CountyJurisdiction MyJurisdiction in ToPickFrom)
            {
                int SpawnChance = MyJurisdiction.CurrentSpawnChance(WantedLevel);
                if (RandomPick < SpawnChance)
                {
                    return DataMart.Agencies.GetAgency(MyJurisdiction.AgencyInitials);
                    //return MyJurisdiction.GameAgency;
                }
                RandomPick -= SpawnChance;
            }
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
        };

        CountyJurisdictionList.Add(new CountyJurisdiction("APD-ASD", County.Crook, 0, 100, 100));

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



