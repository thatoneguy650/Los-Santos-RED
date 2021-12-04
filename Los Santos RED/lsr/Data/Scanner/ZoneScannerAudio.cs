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
        return Returned.ScannerUnitValue;
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
            new ZoneLookup("PROCOB", areas.ProcopioBeach.FileName,attention_all_area_units.VenturaCountyUnits1.FileName),
            new ZoneLookup("MTCHIL", areas.MountChiliad.FileName,attention_all_area_units.VenturaCountyUnits1.FileName),
            new ZoneLookup("PALETO", areas.PaletoBay.FileName,attention_all_area_units.VenturaCountyUnits1.FileName),
            new ZoneLookup("PALCOV", areas.PaletoBay.FileName,attention_all_area_units.VenturaCountyUnits1.FileName),
            new ZoneLookup("PALFOR", areas.PaletoForest.FileName,attention_all_area_units.VenturaCountyUnits1.FileName),
            new ZoneLookup("CMSW", areas.ChilliadMountainStWilderness.FileName,attention_all_area_units.VenturaCountyUnits1.FileName),
            new ZoneLookup("CALAFB", "",attention_all_area_units.VenturaCountyUnits1.FileName),
            new ZoneLookup("GALFISH", "",attention_all_area_units.VenturaCountyUnits1.FileName),
            new ZoneLookup("CCREAK", "",attention_all_area_units.VenturaCountyUnits1.FileName),

            //Majestic
            new ZoneLookup("MTGORDO", areas.MountGordo.FileName,attention_all_area_units.MajesticCountyUnits1.FileName),
            new ZoneLookup("ELGORL", areas.MountGordo.FileName,attention_all_area_units.MajesticCountyUnits1.FileName),
            new ZoneLookup("GRAPES", areas.Grapeseed.FileName,attention_all_area_units.MajesticCountyUnits1.FileName),
            new ZoneLookup("BRADP", areas.BraddockPass.FileName,attention_all_area_units.MajesticCountyUnits1.FileName),
            new ZoneLookup("BRADT", areas.TheBraddockTunnel.FileName,attention_all_area_units.MajesticCountyUnits1.FileName),
            new ZoneLookup("SANCHIA", "",attention_all_area_units.MajesticCountyUnits1.FileName),

            //Blaine
            new ZoneLookup("ALAMO", areas.TheAlamaSea.FileName),
            new ZoneLookup("ARMYB", areas.FtZancudo.FileName),
            new ZoneLookup("CANNY", areas.RatonCanyon.FileName),
            new ZoneLookup("DESRT", areas.GrandeSonoranDesert.FileName),
            new ZoneLookup("HUMLAB", ""),
            new ZoneLookup("JAIL", areas.BoilingBrookPenitentiary.FileName, attention_all_area_units.BollingBrokeUnits.FileName),
            new ZoneLookup("LAGO", areas.LagoZancudo.FileName),
            new ZoneLookup("MTJOSE", areas.MtJosiah.FileName),
            new ZoneLookup("NCHU", areas.NorthChumash.FileName,attention_all_area_units.ChumashUnits1.FileName),
            new ZoneLookup("SANDY", areas.SandyShores.FileName,attention_all_area_units.SandyShoresUnits1.FileName),
            new ZoneLookup("SLAB", areas.SlabCity.FileName),
            new ZoneLookup("ZANCUDO", areas.ZancudoRiver.FileName, attention_all_area_units.ZancudoRiverUnits.FileName),
            new ZoneLookup("ZQ_UAR", areas.DavisCourts.FileName),

            //Vespucci
            new ZoneLookup("BEACH", areas.VespucciBeach.FileName, attention_all_area_units.VespucciAreaUnits1.FileName),
            new ZoneLookup("DELBE", areas.DelPierroBeach.FileName, attention_all_area_units.VespucciAreaUnits1.FileName),
            new ZoneLookup("DELPE", areas.DelPierro.FileName, attention_all_area_units.VespucciAreaUnits1.FileName),
            new ZoneLookup("VCANA", areas.VespucciCanal.FileName, attention_all_area_units.VespucciAreaUnits1.FileName),
            new ZoneLookup("VESP", areas.Vespucci.FileName, attention_all_area_units.VespucciAreaUnits1.FileName),
            new ZoneLookup("LOSPUER", areas.LaPuertes.FileName, attention_all_area_units.VespucciAreaUnits1.FileName),
            new ZoneLookup("PBLUFF", areas.PacificBluffs.FileName, attention_all_area_units.VespucciAreaUnits1.FileName),
            new ZoneLookup("DELSOL", areas.PuertoDelSoul.FileName, attention_all_area_units.VespucciAreaUnits1.FileName),

            //Central
            new ZoneLookup("BANNING", areas.Banning.FileName, attention_all_area_units.CentralUnits1.FileName),
            new ZoneLookup("CHAMH", areas.ChamberlainHills.FileName, attention_all_area_units.CentralUnits1.FileName),
            new ZoneLookup("DAVIS", areas.Davis.FileName, attention_all_area_units.CentralUnits1.FileName),
            new ZoneLookup("DOWNT", areas.Downtown.FileName, attention_all_area_units.DowntownUnits1.FileName),
            new ZoneLookup("PBOX", areas.PillboxHill.FileName, attention_all_area_units.DowntownUnits1.FileName),
            new ZoneLookup("RANCHO", areas.Rancho.FileName, attention_all_area_units.CentralUnits1.FileName),
            new ZoneLookup("SKID", areas.MissionRow.FileName, attention_all_area_units.CentralUnits1.FileName),
            new ZoneLookup("STAD", areas.MazeBankArena.FileName, attention_all_area_units.CentralUnits1.FileName),
            new ZoneLookup("STRAW", areas.Strawberry.FileName, attention_all_area_units.CentralUnits1.FileName),
            new ZoneLookup("TEXTI", areas.TextileCity.FileName, attention_all_area_units.CentralUnits1.FileName),
            new ZoneLookup("LEGSQU", "", attention_all_area_units.DowntownUnits1.FileName),

            //East LS
            new ZoneLookup("CYPRE", areas.CypressFlats.FileName, attention_all_area_units.EastLosSantosUnits1.FileName),
            new ZoneLookup("LMESA", areas.LaMesa.FileName, attention_all_area_units.EastLosSantosUnits1.FileName),
            new ZoneLookup("MIRR", areas.MirrorPark.FileName, attention_all_area_units.EastLosSantosUnits1.FileName),
            new ZoneLookup("MURRI", areas.MuriettaHeights.FileName, attention_all_area_units.EastLosSantosUnits1.FileName),
            new ZoneLookup("EBURO", areas.ElBerroHights.FileName, attention_all_area_units.EastLosSantosUnits1.FileName),

            //Vinewood
            new ZoneLookup("ALTA", areas.Alta.FileName, attention_all_area_units.VinewoodUnits1.FileName),
            new ZoneLookup("DTVINE", areas.DowntownVinewood.FileName, attention_all_area_units.VinewoodUnits1.FileName),
            new ZoneLookup("EAST_V", areas.EastVinewood.FileName, attention_all_area_units.VinewoodUnits1.FileName),
            new ZoneLookup("HAWICK", "", attention_all_area_units.VinewoodUnits1.FileName),
            new ZoneLookup("HORS", areas.TheRaceCourse.FileName, attention_all_area_units.VinewoodUnits1.FileName),
            new ZoneLookup("VINE", areas.Vinewood.FileName, attention_all_area_units.VinewoodUnits1.FileName),
            new ZoneLookup("WVINE", areas.WestVinewood.FileName, attention_all_area_units.VinewoodUnits1.FileName),

            //PortOfLosSantos
            new ZoneLookup("ELYSIAN", areas.ElysianIsland.FileName, attention_all_area_units.PortOfLosSantosUnits1.FileName),
            new ZoneLookup("ZP_ORT", areas.PortOfSouthLosSantos.FileName, attention_all_area_units.PortOfLosSantosUnits1.FileName),
            new ZoneLookup("TERMINA", areas.Terminal.FileName, attention_all_area_units.PortOfLosSantosUnits1.FileName),
            new ZoneLookup("AIRP", areas.LosSantosInternationalAirport.FileName, attention_all_area_units.PortOfLosSantosUnits1.FileName),

            //Rockford Hills
            new ZoneLookup("BURTON", areas.Burton.FileName, attention_all_area_units.CentralUnits1.FileName),
            new ZoneLookup("GOLF", areas.TheGWCGolfingSociety.FileName, attention_all_area_units.CentralUnits1.FileName),
            new ZoneLookup("KOREAT", areas.LittleSeoul.FileName, attention_all_area_units.CentralUnits1.FileName),
            new ZoneLookup("MORN", areas.MorningWood.FileName, attention_all_area_units.CentralUnits1.FileName),
            new ZoneLookup("MOVIE", areas.RichardsMajesticStudio.FileName, attention_all_area_units.CentralUnits1.FileName),
            new ZoneLookup("RICHM", areas.Richman.FileName, attention_all_area_units.CentralUnits1.FileName),
            new ZoneLookup("ROCKF", areas.RockfordHills.FileName, attention_all_area_units.CentralUnits1.FileName),     

            //Vinewood Hills
            new ZoneLookup("CHIL", areas.VinewoodHills.FileName, attention_all_area_units.VinewoodHillsUnits1.FileName),
            new ZoneLookup("GREATC", areas.GreatChapparalle.FileName, attention_all_area_units.VinewoodHillsUnits1.FileName),
            new ZoneLookup("BAYTRE", areas.BayTreeCanyon.FileName, attention_all_area_units.VinewoodHillsUnits1.FileName),
            new ZoneLookup("RGLEN", areas.RichmanGlenn.FileName, attention_all_area_units.VinewoodHillsUnits1.FileName),
            new ZoneLookup("TONGVAV", areas.TongvaValley.FileName, attention_all_area_units.VinewoodHillsUnits1.FileName),
            new ZoneLookup("HARMO", areas.Harmony.FileName, attention_all_area_units.VinewoodHillsUnits1.FileName),
            new ZoneLookup("RTRAK", areas.TheRedwoodLightsTrack.FileName, attention_all_area_units.RedwoodLightsUnits1.FileName),
           
            //Chumash
            new ZoneLookup("BANHAMC", "", attention_all_area_units.ChumashUnits1.FileName),
            new ZoneLookup("BHAMCA", "", attention_all_area_units.ChumashUnits1.FileName),
            new ZoneLookup("CHU", areas.Chumash.FileName, attention_all_area_units.ChumashUnits1.FileName),
            new ZoneLookup("TONGVAH", areas.TongaHills.FileName, attention_all_area_units.ChumashUnits1.FileName),
           
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

