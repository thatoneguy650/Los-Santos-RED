using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DispatchScannerFiles;

namespace LosSantosRED.lsr.Data
{
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
        private void DefaultConfig()
        {

            ZoneList = new List<ZoneLookup>
            {
            //One Off
            new ZoneLookup("OCEANA", areas.TheOcean.FileName),

            //North Blaine
            new ZoneLookup("PROCOB", areas.ProcopioBeach.FileName),
            new ZoneLookup("MTCHIL", areas.MountChiliad.FileName),
            new ZoneLookup("MTGORDO", areas.MountGordo.FileName),
            new ZoneLookup("PALETO", areas.PaletoBay.FileName),
            new ZoneLookup("PALCOV", areas.PaletoBay.FileName),
            new ZoneLookup("PALFOR", areas.PaletoForest.FileName),
            new ZoneLookup("CMSW", areas.ChilliadMountainStWilderness.FileName),
            new ZoneLookup("CALAFB", ""),
            new ZoneLookup("GALFISH", ""),
            new ZoneLookup("ELGORL", areas.MountGordo.FileName),
            new ZoneLookup("GRAPES", areas.Grapeseed.FileName),
            new ZoneLookup("BRADP", areas.BraddockPass.FileName),
            new ZoneLookup("BRADT", areas.TheBraddockTunnel.FileName),
            new ZoneLookup("CCREAK", ""),

            //Blaine
            new ZoneLookup("ALAMO", areas.TheAlamaSea.FileName),
            new ZoneLookup("ARMYB", areas.FtZancudo.FileName),
            new ZoneLookup("CANNY", areas.RatonCanyon.FileName),
            new ZoneLookup("DESRT", areas.GrandeSonoranDesert.FileName),
            new ZoneLookup("HUMLAB", ""),
            new ZoneLookup("JAIL", areas.BoilingBrookPenitentiary.FileName),
            new ZoneLookup("LAGO", areas.LagoZancudo.FileName),
            new ZoneLookup("MTJOSE", areas.MtJosiah.FileName),
            new ZoneLookup("NCHU", areas.NorthChumash.FileName),
            new ZoneLookup("SANCHIA", ""),
            new ZoneLookup("SANDY", areas.SandyShores.FileName),
            new ZoneLookup("SLAB", areas.SlabCity.FileName),
            new ZoneLookup("ZANCUDO", areas.ZancudoRiver.FileName),
            new ZoneLookup("ZQ_UAR", areas.DavisCourts.FileName),

            //Vespucci
            new ZoneLookup("BEACH", areas.VespucciBeach.FileName),
            new ZoneLookup("DELBE", areas.DelPierroBeach.FileName),
            new ZoneLookup("DELPE", areas.DelPierro.FileName),
            new ZoneLookup("VCANA", areas.VespucciCanal.FileName),
            new ZoneLookup("VESP", areas.Vespucci.FileName),
            new ZoneLookup("LOSPUER", areas.LaPuertes.FileName),
            new ZoneLookup("PBLUFF", areas.PacificBluffs.FileName),
            new ZoneLookup("DELSOL", areas.PuertoDelSoul.FileName),

            //Central
            new ZoneLookup("BANNING", areas.Banning.FileName),
            new ZoneLookup("CHAMH", areas.ChamberlainHills.FileName),
            new ZoneLookup("DAVIS", areas.Davis.FileName),
            new ZoneLookup("DOWNT", areas.Downtown.FileName),
            new ZoneLookup("PBOX", areas.PillboxHill.FileName),
            new ZoneLookup("RANCHO", areas.Rancho.FileName),
            new ZoneLookup("SKID", areas.MissionRow.FileName),
            new ZoneLookup("STAD", areas.MazeBankArena.FileName),
            new ZoneLookup("STRAW", areas.Strawberry.FileName),
            new ZoneLookup("TEXTI", areas.TextileCity.FileName),
            new ZoneLookup("LEGSQU", ""),

            //East LS
            new ZoneLookup("CYPRE", areas.CypressFlats.FileName),
            new ZoneLookup("LMESA", areas.LaMesa.FileName),
            new ZoneLookup("MIRR", areas.MirrorPark.FileName),
            new ZoneLookup("MURRI", areas.MuriettaHeights.FileName),
            new ZoneLookup("EBURO", areas.ElBerroHights.FileName),

            //Vinewood
            new ZoneLookup("ALTA", areas.Alta.FileName),
            new ZoneLookup("DTVINE", areas.DowntownVinewood.FileName),
            new ZoneLookup("EAST_V", areas.EastVinewood.FileName),
            new ZoneLookup("HAWICK", ""),
            new ZoneLookup("HORS", areas.TheRaceCourse.FileName),
            new ZoneLookup("VINE", areas.Vinewood.FileName),
            new ZoneLookup("WVINE", areas.WestVinewood.FileName),

            //PortOfLosSantos
            new ZoneLookup("ELYSIAN", areas.ElysianIsland.FileName),
            new ZoneLookup("ZP_ORT", areas.PortOfSouthLosSantos.FileName),
            new ZoneLookup("TERMINA", areas.Terminal.FileName),
            new ZoneLookup("ZP_ORT", areas.PortOfSouthLosSantos.FileName),
            new ZoneLookup("AIRP", areas.LosSantosInternationalAirport.FileName),

            //Rockford Hills
            new ZoneLookup("BURTON", areas.Burton.FileName),
            new ZoneLookup("GOLF", areas.TheGWCGolfingSociety.FileName),
            new ZoneLookup("KOREAT", areas.LittleSeoul.FileName),
            new ZoneLookup("MORN", areas.MorningWood.FileName),
            new ZoneLookup("MOVIE", areas.RichardsMajesticStudio.FileName),
            new ZoneLookup("RICHM", areas.Richman.FileName),
            new ZoneLookup("ROCKF", areas.RockfordHills.FileName),     

            //Vinewood Hills
            new ZoneLookup("CHIL", areas.VinewoodHills.FileName),
            new ZoneLookup("GREATC", areas.GreatChapparalle.FileName),
            new ZoneLookup("BAYTRE", areas.BayTreeCanyon.FileName),
            new ZoneLookup("RGLEN", areas.RichmanGlenn.FileName),
            new ZoneLookup("TONGVAV", areas.TongvaValley.FileName),
            new ZoneLookup("HARMO", areas.Harmony.FileName),
            new ZoneLookup("RTRAK", areas.TheRedwoodLightsTrack.FileName),
           
            //Chumash
            new ZoneLookup("BANHAMC", ""),
            new ZoneLookup("BHAMCA", ""),
            new ZoneLookup("CHU", areas.Chumash.FileName),
            new ZoneLookup("TONGVAH", areas.TongaHills.FileName),
           
            //Tataviam 
            new ZoneLookup("LACT", ""),
            new ZoneLookup("LDAM", ""),
            new ZoneLookup("NOOSE", ""),
            new ZoneLookup("PALHIGH", areas.PalominoHighlands.FileName),
            new ZoneLookup("PALMPOW", areas.PalmerTaylorPowerStation.FileName),
            new ZoneLookup("SANAND", areas.SanAndreas.FileName),
            new ZoneLookup("TATAMO", areas.TatathiaMountains.FileName),
            new ZoneLookup("WINDF", areas.RonAlternatesWindFarm.FileName),
    };

        }
        private class ZoneLookup
        {
            public string InternalGameName { get; set; }
            public string ScannerValue { get; set; }
            public ZoneLookup()
            {

            }
            public ZoneLookup(string _GameName, string _ScannerValue)
            {
                InternalGameName = _GameName;
                ScannerValue = _ScannerValue;
            }
        }

    }
}
