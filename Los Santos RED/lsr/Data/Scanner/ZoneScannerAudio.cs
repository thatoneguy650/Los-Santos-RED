using ExtensionsMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DispatchScannerFiles;


public class ZoneScannerAudio
{
    private List<ZoneLookup> ZoneList = new List<ZoneLookup>();
    public void ReadConfig()
    {
        DefaultConfig();
    }
    public string GetAudio(string ZoneName)
    {
        ZoneLookup Returned = ZoneList.Where(x => x.InternalGameName == ZoneName).FirstOrDefault();
        if (Returned == null)
            return "";
        return Returned.ScannerValue;
    }
    public string GetUnitAudio(string ZoneName)
    {
        ZoneLookup Returned = ZoneList.Where(x => x.InternalGameName == ZoneName).FirstOrDefault();
        if (Returned == null)
            return "";
        return Returned.ScannerUnitValues.PickRandom();
    }
    public ZoneLookup GetLookup(string ZoneName)
    {
        return ZoneList.Where(x => x.InternalGameName == ZoneName).FirstOrDefault();
    }
    private void DefaultConfig()
    {

        ZoneList = new List<ZoneLookup>
            {
            //One Off
            new ZoneLookup("OCEANA", areas.TheOcean.FileName),
       
            ////Ventura County?
            //new Zone("PROCOB", "Procopio Beach", County.VenturaCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            //new Zone("MTCHIL", "Mount Chiliad", County.VenturaCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            //new Zone("PALETO", "Paleto Bay", County.VenturaCounty, "San Andreas", false, eLocationEconomy.Middle, eLocationType.Rural),
            //new Zone("PALCOV", "Paleto Cove", County.VenturaCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            //new Zone("PALFOR", "Paleto Forest", County.VenturaCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            //new Zone("CMSW", "Chiliad Mountain State Wilderness", County.VenturaCounty, "San Andreas", false, eLocationEconomy.Middle, eLocationType.Wilderness),
            //new Zone("CCREAK", "Cassidy Creek", County.VenturaCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            //new Zone("CALAFB", "Calafia Bridge", County.VenturaCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            //new Zone("GALFISH", "Galilee", County.VenturaCounty, "San Andreas", false, eLocationEconomy.Middle, eLocationType.Wilderness),
            
            ////Majestic County
            //new Zone("GRAPES", "Grapeseed", County.MajesticCounty, "San Andreas", false, eLocationEconomy.Middle, eLocationType.Rural),
            //new Zone("BRADP", "Braddock Pass", County.MajesticCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            //new Zone("BRADT", "Braddock Tunnel", County.MajesticCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            //new Zone("ELGORL", "El Gordo Lighthouse", County.MajesticCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            //new Zone("MTGORDO", "Mount Gordo", County.MajesticCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            //new Zone("SANCHIA", "San Chianski Mountain Range", County.MajesticCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),

            //Ventura
            new ZoneLookup("PROCOB", areas.ProcopioBeach.FileName, new List<string>(){ attention_all_area_units.VenturaCountyUnits1.FileName,attention_all_area_units.VenturaCountyUnits2.FileName,attention_all_area_units.VenturaCountyUnits3.FileName,attention_all_area_units.VenturaCountyUnits4.FileName, }),
            new ZoneLookup("MTCHIL", areas.MountChiliad.FileName, new List<string>(){ attention_all_area_units.VenturaCountyUnits1.FileName,attention_all_area_units.VenturaCountyUnits2.FileName,attention_all_area_units.VenturaCountyUnits3.FileName,attention_all_area_units.VenturaCountyUnits4.FileName, }),
            new ZoneLookup("PALETO", areas.PaletoBay.FileName, new List<string>(){ attention_all_area_units.VenturaCountyUnits1.FileName,attention_all_area_units.VenturaCountyUnits2.FileName,attention_all_area_units.VenturaCountyUnits3.FileName,attention_all_area_units.VenturaCountyUnits4.FileName, }),
            new ZoneLookup("PALCOV", areas.PaletoBay.FileName, new List<string>(){ attention_all_area_units.VenturaCountyUnits1.FileName,attention_all_area_units.VenturaCountyUnits2.FileName,attention_all_area_units.VenturaCountyUnits3.FileName,attention_all_area_units.VenturaCountyUnits4.FileName, }),
            new ZoneLookup("PALFOR", areas.PaletoForest.FileName, new List<string>(){ attention_all_area_units.VenturaCountyUnits1.FileName,attention_all_area_units.VenturaCountyUnits2.FileName,attention_all_area_units.VenturaCountyUnits3.FileName,attention_all_area_units.VenturaCountyUnits4.FileName, }),
            new ZoneLookup("CMSW", areas.ChilliadMountainStWilderness.FileName, new List<string>(){ attention_all_area_units.VenturaCountyUnits1.FileName,attention_all_area_units.VenturaCountyUnits2.FileName,attention_all_area_units.VenturaCountyUnits3.FileName,attention_all_area_units.VenturaCountyUnits4.FileName, }),
            new ZoneLookup("CALAFB", "", new List<string>(){ attention_all_area_units.VenturaCountyUnits1.FileName,attention_all_area_units.VenturaCountyUnits2.FileName,attention_all_area_units.VenturaCountyUnits3.FileName,attention_all_area_units.VenturaCountyUnits4.FileName, }),
            new ZoneLookup("GALFISH", "", new List<string>(){ attention_all_area_units.VenturaCountyUnits1.FileName,attention_all_area_units.VenturaCountyUnits2.FileName,attention_all_area_units.VenturaCountyUnits3.FileName,attention_all_area_units.VenturaCountyUnits4.FileName, }),
            new ZoneLookup("CCREAK", "", new List<string>(){ attention_all_area_units.VenturaCountyUnits1.FileName,attention_all_area_units.VenturaCountyUnits2.FileName,attention_all_area_units.VenturaCountyUnits3.FileName,attention_all_area_units.VenturaCountyUnits4.FileName, }),

            //Majestic
            new ZoneLookup("MTGORDO", areas.MountGordo.FileName, new List<string>(){ attention_all_area_units.MajesticCountyUnits1.FileName,attention_all_area_units.MajesticCountyUnits2.FileName,attention_all_area_units.MajesticCountyUnits3.FileName,attention_all_area_units.MajesticCountyUnits4.FileName, }),
            new ZoneLookup("ELGORL", areas.MountGordo.FileName, new List<string>(){ attention_all_area_units.MajesticCountyUnits1.FileName,attention_all_area_units.MajesticCountyUnits2.FileName,attention_all_area_units.MajesticCountyUnits3.FileName,attention_all_area_units.MajesticCountyUnits4.FileName, }),
            new ZoneLookup("GRAPES", areas.Grapeseed.FileName, new List<string>(){ attention_all_area_units.MajesticCountyUnits1.FileName,attention_all_area_units.MajesticCountyUnits2.FileName,attention_all_area_units.MajesticCountyUnits3.FileName,attention_all_area_units.MajesticCountyUnits4.FileName, }),
            new ZoneLookup("BRADP", areas.BraddockPass.FileName, new List<string>(){ attention_all_area_units.MajesticCountyUnits1.FileName,attention_all_area_units.MajesticCountyUnits2.FileName,attention_all_area_units.MajesticCountyUnits3.FileName,attention_all_area_units.MajesticCountyUnits4.FileName, }),
            new ZoneLookup("BRADT", areas.TheBraddockTunnel.FileName, new List<string>(){ attention_all_area_units.MajesticCountyUnits1.FileName,attention_all_area_units.MajesticCountyUnits2.FileName,attention_all_area_units.MajesticCountyUnits3.FileName,attention_all_area_units.MajesticCountyUnits4.FileName, }),
            new ZoneLookup("SANCHIA", "", new List<string>(){ attention_all_area_units.MajesticCountyUnits1.FileName,attention_all_area_units.MajesticCountyUnits2.FileName,attention_all_area_units.MajesticCountyUnits3.FileName,attention_all_area_units.MajesticCountyUnits4.FileName, }),
            new ZoneLookup("HARMO", areas.Harmony.FileName, new List<string>(){ attention_all_area_units.MajesticCountyUnits1.FileName,attention_all_area_units.MajesticCountyUnits2.FileName,attention_all_area_units.MajesticCountyUnits3.FileName,attention_all_area_units.MajesticCountyUnits4.FileName, }),


            //Blaine
            new ZoneLookup("ALAMO", areas.TheAlamaSea.FileName),
            new ZoneLookup("ARMYB", areas.FtZancudo.FileName),
            new ZoneLookup("CANNY", areas.RatonCanyon.FileName),
            new ZoneLookup("DESRT", areas.GrandeSonoranDesert.FileName),
            new ZoneLookup("HUMLAB", ""),
            new ZoneLookup("JAIL", areas.BoilingBrookPenitentiary.FileName, attention_all_area_units.BollingBrokeUnits.FileName),
            new ZoneLookup("LAGO", areas.LagoZancudo.FileName),
            new ZoneLookup("MTJOSE", areas.MtJosiah.FileName),
            new ZoneLookup("NCHU", areas.NorthChumash.FileName, new List<string>(){ attention_all_area_units.ChumashUnits1.FileName,attention_all_area_units.ChumashUnits2.FileName,attention_all_area_units.ChumashUnits3.FileName }),
            new ZoneLookup("SANDY", areas.SandyShores.FileName, new List<string>(){ attention_all_area_units.SandyShoresUnits1.FileName,attention_all_area_units.SandyShoresUnits2.FileName,attention_all_area_units.SandyShoresUnits3.FileName }),
            new ZoneLookup("SLAB", areas.SlabCity.FileName),
            new ZoneLookup("ZANCUDO", areas.ZancudoRiver.FileName, attention_all_area_units.ZancudoRiverUnits.FileName),
            new ZoneLookup("ZQ_UAR", areas.DavisCourts.FileName),

            //Vespucci
            new ZoneLookup("BEACH", areas.VespucciBeach.FileName, new List<string>(){ attention_all_area_units.VespucciAreaUnits1.FileName,attention_all_area_units.VespucciAreaUnits2.FileName }),
            new ZoneLookup("DELBE", areas.DelPierroBeach.FileName, new List<string>(){ attention_all_area_units.VespucciAreaUnits1.FileName,attention_all_area_units.VespucciAreaUnits2.FileName }),
            new ZoneLookup("DELPE", areas.DelPierro.FileName, new List<string>(){ attention_all_area_units.VespucciAreaUnits1.FileName,attention_all_area_units.VespucciAreaUnits2.FileName }),
            new ZoneLookup("VCANA", areas.VespucciCanal.FileName, new List<string>(){ attention_all_area_units.VespucciAreaUnits1.FileName,attention_all_area_units.VespucciAreaUnits2.FileName }),
            new ZoneLookup("VESP", areas.Vespucci.FileName, new List<string>(){ attention_all_area_units.VespucciAreaUnits1.FileName,attention_all_area_units.VespucciAreaUnits2.FileName }),
            new ZoneLookup("LOSPUER", areas.LaPuertes.FileName, new List<string>(){ attention_all_area_units.VespucciAreaUnits1.FileName,attention_all_area_units.VespucciAreaUnits2.FileName }),
            new ZoneLookup("PBLUFF", areas.PacificBluffs.FileName, new List<string>(){ attention_all_area_units.VespucciAreaUnits1.FileName,attention_all_area_units.VespucciAreaUnits2.FileName }),
            new ZoneLookup("DELSOL", areas.PuertoDelSoul.FileName, new List<string>(){ attention_all_area_units.VespucciAreaUnits1.FileName,attention_all_area_units.VespucciAreaUnits2.FileName }),

            //Central
            new ZoneLookup("BANNING", areas.Banning.FileName, new List<string>(){ attention_all_area_units.CentralUnits1.FileName,attention_all_area_units.CentralUnits2.FileName,attention_all_area_units.CentralUnits3.FileName }),
            new ZoneLookup("CHAMH", areas.ChamberlainHills.FileName, new List<string>(){ attention_all_area_units.CentralUnits1.FileName,attention_all_area_units.CentralUnits2.FileName,attention_all_area_units.CentralUnits3.FileName }),
            new ZoneLookup("DAVIS", areas.Davis.FileName, new List<string>(){ attention_all_area_units.CentralUnits1.FileName,attention_all_area_units.CentralUnits2.FileName,attention_all_area_units.CentralUnits3.FileName }),
            new ZoneLookup("DOWNT", areas.Downtown.FileName, new List<string>(){ attention_all_area_units.CentralUnits1.FileName,attention_all_area_units.CentralUnits2.FileName,attention_all_area_units.CentralUnits3.FileName }),
            new ZoneLookup("PBOX", areas.PillboxHill.FileName, new List<string>(){ attention_all_area_units.CentralUnits1.FileName,attention_all_area_units.CentralUnits2.FileName,attention_all_area_units.CentralUnits3.FileName }),
            new ZoneLookup("RANCHO", areas.Rancho.FileName, new List<string>(){ attention_all_area_units.CentralUnits1.FileName,attention_all_area_units.CentralUnits2.FileName,attention_all_area_units.CentralUnits3.FileName }),
            new ZoneLookup("SKID", areas.MissionRow.FileName, new List<string>(){ attention_all_area_units.CentralUnits1.FileName,attention_all_area_units.CentralUnits2.FileName,attention_all_area_units.CentralUnits3.FileName }),
            new ZoneLookup("STAD", areas.MazeBankArena.FileName, new List<string>(){ attention_all_area_units.CentralUnits1.FileName,attention_all_area_units.CentralUnits2.FileName,attention_all_area_units.CentralUnits3.FileName }),
            new ZoneLookup("STRAW", areas.Strawberry.FileName, new List<string>(){ attention_all_area_units.CentralUnits1.FileName,attention_all_area_units.CentralUnits2.FileName,attention_all_area_units.CentralUnits3.FileName }),
            new ZoneLookup("TEXTI", areas.TextileCity.FileName, new List<string>(){ attention_all_area_units.CentralUnits1.FileName,attention_all_area_units.CentralUnits2.FileName,attention_all_area_units.CentralUnits3.FileName }),
            new ZoneLookup("LEGSQU", "", new List<string>(){ attention_all_area_units.CentralUnits1.FileName,attention_all_area_units.CentralUnits2.FileName,attention_all_area_units.CentralUnits3.FileName }),

            //East LS
            new ZoneLookup("CYPRE", areas.CypressFlats.FileName, new List<string>(){ attention_all_area_units.EastLosSantosUnits1.FileName,attention_all_area_units.EastLosSantosUnits2.FileName,attention_all_area_units.EastLosSantosUnits3.FileName }),
            new ZoneLookup("LMESA", areas.LaMesa.FileName, new List<string>(){ attention_all_area_units.EastLosSantosUnits1.FileName,attention_all_area_units.EastLosSantosUnits2.FileName,attention_all_area_units.EastLosSantosUnits3.FileName }),
            new ZoneLookup("MIRR", areas.MirrorPark.FileName, new List<string>(){ attention_all_area_units.EastLosSantosUnits1.FileName,attention_all_area_units.EastLosSantosUnits2.FileName,attention_all_area_units.EastLosSantosUnits3.FileName }),
            new ZoneLookup("MURRI", areas.MuriettaHeights.FileName, new List<string>(){ attention_all_area_units.EastLosSantosUnits1.FileName,attention_all_area_units.EastLosSantosUnits2.FileName,attention_all_area_units.EastLosSantosUnits3.FileName }),
            new ZoneLookup("EBURO", areas.ElBerroHights.FileName, new List<string>(){ attention_all_area_units.EastLosSantosUnits1.FileName,attention_all_area_units.EastLosSantosUnits2.FileName,attention_all_area_units.EastLosSantosUnits3.FileName }),

            //Vinewood
            new ZoneLookup("ALTA", areas.Alta.FileName, new List<string>(){ attention_all_area_units.VinewoodUnits1.FileName,attention_all_area_units.VinewoodUnits2.FileName,attention_all_area_units.VinewoodUnits3.FileName }),
            new ZoneLookup("DTVINE", areas.DowntownVinewood.FileName, new List<string>(){ attention_all_area_units.VinewoodUnits1.FileName,attention_all_area_units.VinewoodUnits2.FileName,attention_all_area_units.VinewoodUnits3.FileName }),
            new ZoneLookup("EAST_V", areas.EastVinewood.FileName, new List<string>(){ attention_all_area_units.VinewoodUnits1.FileName,attention_all_area_units.VinewoodUnits2.FileName,attention_all_area_units.VinewoodUnits3.FileName }),
            new ZoneLookup("HAWICK", "", new List<string>(){ attention_all_area_units.VinewoodUnits1.FileName,attention_all_area_units.VinewoodUnits2.FileName,attention_all_area_units.VinewoodUnits3.FileName }),
            new ZoneLookup("HORS", areas.TheRaceCourse.FileName, new List<string>(){ attention_all_area_units.VinewoodUnits1.FileName,attention_all_area_units.VinewoodUnits2.FileName,attention_all_area_units.VinewoodUnits3.FileName }),
            new ZoneLookup("VINE", areas.Vinewood.FileName, new List<string>(){ attention_all_area_units.VinewoodUnits1.FileName,attention_all_area_units.VinewoodUnits2.FileName,attention_all_area_units.VinewoodUnits3.FileName }),
            new ZoneLookup("WVINE", areas.WestVinewood.FileName, new List<string>(){ attention_all_area_units.VinewoodUnits1.FileName,attention_all_area_units.VinewoodUnits2.FileName,attention_all_area_units.VinewoodUnits3.FileName }),

            //PortOfLosSantos
            new ZoneLookup("ELYSIAN", areas.ElysianIsland.FileName, new List<string>(){ attention_all_area_units.PortOfLosSantosUnits1.FileName,attention_all_area_units.PortOfLosSantosUnits2.FileName,attention_all_area_units.PortOfLosSantosUnits3.FileName,attention_all_area_units.PortOfLosSantosUnits4.FileName, }),
            new ZoneLookup("ZP_ORT", areas.PortOfSouthLosSantos.FileName, new List<string>(){ attention_all_area_units.PortOfLosSantosUnits1.FileName,attention_all_area_units.PortOfLosSantosUnits2.FileName,attention_all_area_units.PortOfLosSantosUnits3.FileName,attention_all_area_units.PortOfLosSantosUnits4.FileName, }),
            new ZoneLookup("TERMINA", areas.Terminal.FileName, new List<string>(){ attention_all_area_units.PortOfLosSantosUnits1.FileName,attention_all_area_units.PortOfLosSantosUnits2.FileName,attention_all_area_units.PortOfLosSantosUnits3.FileName,attention_all_area_units.PortOfLosSantosUnits4.FileName, }),
            new ZoneLookup("AIRP", areas.LosSantosInternationalAirport.FileName, new List<string>(){ attention_all_area_units.PortOfLosSantosUnits1.FileName,attention_all_area_units.PortOfLosSantosUnits2.FileName,attention_all_area_units.PortOfLosSantosUnits3.FileName,attention_all_area_units.PortOfLosSantosUnits4.FileName, }),

            //Rockford Hills
            new ZoneLookup("BURTON", areas.Burton.FileName, new List<string>(){ attention_all_area_units.CentralUnits1.FileName,attention_all_area_units.CentralUnits2.FileName,attention_all_area_units.CentralUnits3.FileName }),
            new ZoneLookup("GOLF", areas.TheGWCGolfingSociety.FileName, new List<string>(){ attention_all_area_units.CentralUnits1.FileName,attention_all_area_units.CentralUnits2.FileName,attention_all_area_units.CentralUnits3.FileName }),
            new ZoneLookup("KOREAT", areas.LittleSeoul.FileName, new List<string>(){ attention_all_area_units.CentralUnits1.FileName,attention_all_area_units.CentralUnits2.FileName,attention_all_area_units.CentralUnits3.FileName }),
            new ZoneLookup("MORN", areas.MorningWood.FileName, new List<string>(){ attention_all_area_units.CentralUnits1.FileName,attention_all_area_units.CentralUnits2.FileName,attention_all_area_units.CentralUnits3.FileName }),
            new ZoneLookup("MOVIE", areas.RichardsMajesticStudio.FileName, new List<string>(){ attention_all_area_units.CentralUnits1.FileName,attention_all_area_units.CentralUnits2.FileName,attention_all_area_units.CentralUnits3.FileName }),
            new ZoneLookup("RICHM", areas.Richman.FileName, new List<string>(){ attention_all_area_units.CentralUnits1.FileName,attention_all_area_units.CentralUnits2.FileName,attention_all_area_units.CentralUnits3.FileName }),
            new ZoneLookup("ROCKF", areas.RockfordHills.FileName, new List<string>(){ attention_all_area_units.CentralUnits1.FileName,attention_all_area_units.CentralUnits2.FileName,attention_all_area_units.CentralUnits3.FileName }),  

            //Vinewood Hills
            new ZoneLookup("CHIL", areas.VinewoodHills.FileName, new List<string>(){ attention_all_area_units.VinewoodHillsUnits1.FileName,attention_all_area_units.VinewoodHillsUnits2.FileName,attention_all_area_units.VinewoodHillsUnits3.FileName }),
            new ZoneLookup("GREATC", areas.GreatChapparalle.FileName, new List<string>(){ attention_all_area_units.VinewoodHillsUnits1.FileName,attention_all_area_units.VinewoodHillsUnits2.FileName,attention_all_area_units.VinewoodHillsUnits3.FileName }),
            new ZoneLookup("BAYTRE", areas.BayTreeCanyon.FileName, new List<string>(){ attention_all_area_units.VinewoodHillsUnits1.FileName,attention_all_area_units.VinewoodHillsUnits2.FileName,attention_all_area_units.VinewoodHillsUnits3.FileName }),
            new ZoneLookup("RGLEN", areas.RichmanGlenn.FileName, new List<string>(){ attention_all_area_units.VinewoodHillsUnits1.FileName,attention_all_area_units.VinewoodHillsUnits2.FileName,attention_all_area_units.VinewoodHillsUnits3.FileName }),
            new ZoneLookup("TONGVAV", areas.TongvaValley.FileName, new List<string>(){ attention_all_area_units.VinewoodHillsUnits1.FileName,attention_all_area_units.VinewoodHillsUnits2.FileName,attention_all_area_units.VinewoodHillsUnits3.FileName }),

            new ZoneLookup("RTRAK", areas.TheRedwoodLightsTrack.FileName, attention_all_area_units.RedwoodLightsUnits1.FileName),
           
            //Chumash
            new ZoneLookup("BANHAMC", "", new List<string>(){ attention_all_area_units.ChumashUnits1.FileName,attention_all_area_units.ChumashUnits2.FileName,attention_all_area_units.ChumashUnits3.FileName }),
            new ZoneLookup("BHAMCA", "", new List<string>(){ attention_all_area_units.ChumashUnits1.FileName,attention_all_area_units.ChumashUnits2.FileName,attention_all_area_units.ChumashUnits3.FileName }),
            new ZoneLookup("CHU", areas.Chumash.FileName, new List<string>(){ attention_all_area_units.ChumashUnits1.FileName,attention_all_area_units.ChumashUnits2.FileName,attention_all_area_units.ChumashUnits3.FileName }),
            new ZoneLookup("TONGVAH", areas.TongaHills.FileName, new List<string>(){ attention_all_area_units.ChumashUnits1.FileName,attention_all_area_units.ChumashUnits2.FileName,attention_all_area_units.ChumashUnits3.FileName }),
           
            //Tataviam 
            new ZoneLookup("LACT", "", attention_all_area_units.LandActReservoirUnits.FileName),
            new ZoneLookup("LDAM", "", attention_all_area_units.LandActDamnUnits.FileName),
            new ZoneLookup("NOOSE", ""),
            new ZoneLookup("PALHIGH", areas.PalominoHighlands.FileName),
            new ZoneLookup("PALMPOW", areas.PalmerTaylorPowerStation.FileName),
            new ZoneLookup("SANAND", areas.SanAndreas.FileName),
            new ZoneLookup("TATAMO", areas.TatathiaMountains.FileName),
            new ZoneLookup("WINDF", areas.RonAlternatesWindFarm.FileName),
    };

    }


}

