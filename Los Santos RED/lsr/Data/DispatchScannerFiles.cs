using ExtensionsMethods;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DispatchScannerFiles
{

    public DispatchScannerFiles()
    {

    }

    

    public class StreetScanner
    {
        private List<StreetLookup> StreetsList = new List<StreetLookup>();

        public void Intitialize()
        {
            DefaultConfig();
        }
        public string GetAudio(string StreetName)
        {
            StreetLookup Returned = StreetsList.Where(x => x.Name == StreetName).FirstOrDefault();
            if (Returned == null)
                return "";
            return Returned.DispatchFile;
        }
        private void DefaultConfig()
        {
            StreetsList = new List<StreetLookup>
        {
            new StreetLookup("Joshua Rd", streets.JoshuaRoad.FileName),
            new StreetLookup("East Joshua Road", streets.EastJoshuaRoad.FileName),
            new StreetLookup("Marina Dr", streets.MarinaDrive.FileName),
            new StreetLookup("Alhambra Dr", streets.ElHamberDrive.FileName),
            new StreetLookup("Niland Ave", streets.NeelanAve.FileName),
            new StreetLookup("Zancudo Ave", streets.ZancudoAve.FileName),
            new StreetLookup("Armadillo Ave", streets.ArmadilloAve.FileName),
            new StreetLookup("Algonquin Blvd", streets.AlgonquinBlvd.FileName),
            new StreetLookup("Mountain View Dr", streets.MountainViewDrive.FileName),
            new StreetLookup("Cholla Springs Ave", streets.ChollaSpringsAve.FileName),
            new StreetLookup("Panorama Dr", streets.PanoramaDrive.FileName),
            new StreetLookup("Lesbos Ln", streets.LesbosLane.FileName),
            new StreetLookup("Calafia Rd", streets.CalapiaRoad.FileName),
            new StreetLookup("North Calafia Way", streets.NorthKalafiaWay.FileName),
            new StreetLookup("Cassidy Trail", streets.CassidyTrail.FileName),
            new StreetLookup("Seaview Rd", streets.SeaviewRd.FileName),
            new StreetLookup("Grapeseed Main St", streets.GrapseedMainStreet.FileName),
            new StreetLookup("Grapeseed Ave", streets.GrapeseedAve.FileName),
            new StreetLookup("Joad Ln", streets.JilledLane.FileName),
            new StreetLookup("Union Rd", streets.UnionRoad.FileName),
            new StreetLookup("O'Neil Way", streets.OneilWay.FileName),
            new StreetLookup("Senora Fwy", streets.SonoraFreeway.FileName),
            new StreetLookup("Catfish View", streets.CatfishView.FileName),
            new StreetLookup("Great Ocean Hwy", streets.GreatOceanHighway.FileName),
            new StreetLookup("Paleto Blvd", streets.PaletoBlvd.FileName),
            new StreetLookup("Duluoz Ave", streets.DelouasAve.FileName),
            new StreetLookup("Procopio Dr", streets.ProcopioDrive.FileName),
            new StreetLookup("Cascabel Ave"),
            new StreetLookup("Peaceful St", streets.PeacefulStreet.FileName),
            new StreetLookup("Procopio Promenade", streets.ProcopioPromenade.FileName),
            new StreetLookup("Pyrite Ave", streets.PyriteAve.FileName),
            new StreetLookup("Fort Zancudo Approach Rd", streets.FortZancudoApproachRoad.FileName),
            new StreetLookup("Barbareno Rd", streets.BarbarinoRoad.FileName),
            new StreetLookup("Ineseno Road", streets.EnecinoRoad.FileName),
            new StreetLookup("West Eclipse Blvd", streets.WestEclipseBlvd.FileName),
            new StreetLookup("Playa Vista", streets.PlayaVista.FileName),
            new StreetLookup("Bay City Ave", streets.BaseCityAve.FileName),
            new StreetLookup("Del Perro Fwy", streets.DelPierroFreeway.FileName),
            new StreetLookup("Equality Way", streets.EqualityWay.FileName),
            new StreetLookup("Red Desert Ave", streets.RedDesertAve.FileName),
            new StreetLookup("Magellan Ave", streets.MagellanAve.FileName),
            new StreetLookup("Sandcastle Way", streets.SandcastleWay.FileName),
            new StreetLookup("Vespucci Blvd", streets.VespucciBlvd.FileName),
            new StreetLookup("Prosperity St", streets.ProsperityStreet.FileName),
            new StreetLookup("San Andreas Ave", streets.SanAndreasAve.FileName),
            new StreetLookup("North Rockford Dr", streets.NorthRockfordDrive.FileName),
            new StreetLookup("South Rockford Dr", streets.SouthRockfordDrive.FileName),
            new StreetLookup("Marathon Ave", streets.MarathonAve.FileName),
            new StreetLookup("Boulevard Del Perro", streets.BlvdDelPierro.FileName),
            new StreetLookup("Cougar Ave", streets.CougarAve.FileName),
            new StreetLookup("Liberty St", streets.LibertyStreet.FileName),
            new StreetLookup("Bay City Incline", streets.BaseCityIncline.FileName),
            new StreetLookup("Conquistador St", streets.ConquistadorStreet.FileName),
            new StreetLookup("Cortes St", streets.CortezStreet.FileName),
            new StreetLookup("Vitus St", streets.VitasStreet.FileName),
            new StreetLookup("Aguja St", streets.ElGouhaStreet.FileName),/////maytbe????!?!?!
            new StreetLookup("Goma St", streets.GomezStreet.FileName),
            new StreetLookup("Melanoma St", streets.MelanomaStreet.FileName),
            new StreetLookup("Palomino Ave", streets.PalaminoAve.FileName),
            new StreetLookup("Invention Ct", streets.InventionCourt.FileName),
            new StreetLookup("Imagination Ct", streets.ImaginationCourt.FileName),
            new StreetLookup("Rub St", streets.RubStreet.FileName),
            new StreetLookup("Tug St", streets.TugStreet.FileName),
            new StreetLookup("Ginger St", streets.GingerStreet.FileName),
            new StreetLookup("Lindsay Circus", streets.LindsayCircus.FileName),
            new StreetLookup("Calais Ave", streets.CaliasAve.FileName),
            new StreetLookup("Adam's Apple Blvd", streets.AdamsAppleBlvd.FileName),
            new StreetLookup("Alta St", streets.AlterStreet.FileName),
            new StreetLookup("Integrity Way", streets.IntergrityWy.FileName),
            new StreetLookup("Swiss St", streets.SwissStreet.FileName),
            new StreetLookup("Strawberry Ave", streets.StrawberryAve.FileName),
            new StreetLookup("Capital Blvd", streets.CapitalBlvd.FileName),
            new StreetLookup("Crusade Rd", streets.CrusadeRoad.FileName),
            new StreetLookup("Innocence Blvd", streets.InnocenceBlvd.FileName),
            new StreetLookup("Davis Ave", streets.DavisAve.FileName),
            new StreetLookup("Little Bighorn Ave", streets.LittleBighornAve.FileName),
            new StreetLookup("Roy Lowenstein Blvd", streets.RoyLowensteinBlvd.FileName),
            new StreetLookup("Jamestown St", streets.JamestownStreet.FileName),
            new StreetLookup("Carson Ave", streets.CarsonAve.FileName),
            new StreetLookup("Grove St", streets.GroveStreet.FileName),
            new StreetLookup("Brouge Ave"),
            new StreetLookup("Covenant Ave", streets.CovenantAve.FileName),
            new StreetLookup("Dutch London St", streets.DutchLondonStreet.FileName),
            new StreetLookup("Signal St", streets.SignalStreet.FileName),
            new StreetLookup("Elysian Fields Fwy", streets.ElysianFieldsFreeway.FileName),
            new StreetLookup("Plaice Pl"),
            new StreetLookup("Chum St", streets.ChumStreet.FileName),
            new StreetLookup("Chupacabra St"),
            new StreetLookup("Miriam Turner Overpass", streets.MiriamTurnerOverpass.FileName),
            new StreetLookup("Autopia Pkwy", streets.AltopiaParkway.FileName),
            new StreetLookup("Exceptionalists Way", streets.ExceptionalistWay.FileName),
            new StreetLookup("La Puerta Fwy", ""),
            new StreetLookup("New Empire Way", streets.NewEmpireWay.FileName),
            new StreetLookup("Runway1", streets.RunwayOne.FileName),
            new StreetLookup("Greenwich Pkwy", streets.GrenwichParkway.FileName),
            new StreetLookup("Kortz Dr", streets.KortzDrive.FileName),
            new StreetLookup("Banham Canyon Dr", streets.BanhamCanyonDrive.FileName),
            new StreetLookup("Buen Vino Rd"),
            new StreetLookup("Route 68", streets.Route68.FileName),
            new StreetLookup("Zancudo Grande Valley", streets.ZancudoGrandeValley.FileName),
            new StreetLookup("Zancudo Barranca", streets.ZancudoBaranca.FileName),
            new StreetLookup("Galileo Rd", streets.GallileoRoad.FileName),
            new StreetLookup("Mt Vinewood Dr", streets.MountVinewoodDrive.FileName),
            new StreetLookup("Marlowe Dr"),
            new StreetLookup("Milton Rd", streets.MiltonRoad.FileName),
            new StreetLookup("Kimble Hill Dr", streets.KimbalHillDrive.FileName),
            new StreetLookup("Normandy Dr", streets.NormandyDrive.FileName),
            new StreetLookup("Hillcrest Ave", streets.HillcrestAve.FileName),
            new StreetLookup("Hillcrest Ridge Access Rd", streets.HillcrestRidgeAccessRoad.FileName),
            new StreetLookup("North Sheldon Ave", streets.NorthSheldonAve.FileName),
            new StreetLookup("Lake Vinewood Dr", streets.LakeVineWoodDrive.FileName),
            new StreetLookup("Lake Vinewood Est", streets.LakeVinewoodEstate.FileName),
            new StreetLookup("Baytree Canyon Rd", streets.BaytreeCanyonRoad.FileName),
            new StreetLookup("North Conker Ave", streets.NorthConkerAve.FileName),
            new StreetLookup("Wild Oats Dr", streets.WildOatsDrive.FileName),
            new StreetLookup("Whispymound Dr", streets.WispyMoundDrive.FileName),
            new StreetLookup("Didion Dr", streets.DiedianDrive.FileName),
            new StreetLookup("Cox Way", streets.CoxWay.FileName),
            new StreetLookup("Picture Perfect Drive", streets.PicturePerfectDrive.FileName),
            new StreetLookup("South Mo Milton Dr", streets.SouthMoMiltonDrive.FileName),
            new StreetLookup("Cockingend Dr", streets.CockandGinDrive.FileName),
            new StreetLookup("Mad Wayne Thunder Dr", streets.MagwavevendorDrive.FileName),
            new StreetLookup("Hangman Ave", streets.HangmanAve.FileName),
            new StreetLookup("Dunstable Ln", streets.DunstableLane.FileName),
            new StreetLookup("Dunstable Dr", streets.DunstableDrive.FileName),
            new StreetLookup("Greenwich Way", streets.GrenwichWay.FileName),
            new StreetLookup("Greenwich Pl", streets.GrunnichPlace.FileName),
            new StreetLookup("Hardy Way"),
            new StreetLookup("Richman St", streets.RichmondStreet.FileName),
            new StreetLookup("Ace Jones Dr", streets.AceJonesDrive.FileName),
            new StreetLookup("Los Santos Freeway", ""),
            new StreetLookup("Senora Rd", streets.SonoraRoad.FileName),
            new StreetLookup("Nowhere Rd", streets.NowhereRoad.FileName),
            new StreetLookup("Smoke Tree Rd", streets.SmokeTreeRoad.FileName),
            new StreetLookup("Cholla Rd", streets.ChollaRoad.FileName),
            new StreetLookup("Cat-Claw Ave", streets.CatClawAve.FileName),
            new StreetLookup("Senora Way", streets.SonoraWay.FileName),
            new StreetLookup("Palomino Fwy", streets.PaliminoFreeway.FileName),
            new StreetLookup("Shank St", streets.ShankStreet.FileName),
            new StreetLookup("Macdonald St", streets.McDonaldStreet.FileName),
            new StreetLookup("Route 68 Approach", streets.Route68.FileName),
            new StreetLookup("Vinewood Park Dr", streets.VinewoodParkDrive.FileName),
            new StreetLookup("Vinewood Blvd", streets.VinewoodBlvd.FileName),
            new StreetLookup("Mirror Park Blvd", streets.MirrorParkBlvd.FileName),
            new StreetLookup("Glory Way", streets.GloryWay.FileName),
            new StreetLookup("Bridge St", streets.BridgeStreet.FileName),
            new StreetLookup("West Mirror Drive", streets.WestMirrorDrive.FileName),
            new StreetLookup("Nikola Ave", streets.NicolaAve.FileName),
            new StreetLookup("East Mirror Dr", streets.EastMirrorDrive.FileName),
            new StreetLookup("Nikola Pl", streets.NikolaPlace.FileName),
            new StreetLookup("Mirror Pl", streets.MirrorPlace.FileName),
            new StreetLookup("El Rancho Blvd", streets.ElRanchoBlvd.FileName),
            new StreetLookup("Olympic Fwy", streets.OlympicFreeway.FileName),
            new StreetLookup("Fudge Ln", streets.FudgeLane.FileName),
            new StreetLookup("Amarillo Vista", streets.AmarilloVista.FileName),
            new StreetLookup("Labor Pl", streets.ForceLaborPlace.FileName),
            new StreetLookup("El Burro Blvd", streets.ElBurroBlvd.FileName),
            new StreetLookup("Sustancia Rd", streets.SustanciaRoad.FileName),
            new StreetLookup("South Shambles St", streets.SouthShambleStreet.FileName),
            new StreetLookup("Hanger Way", streets.HangarWay.FileName),
            new StreetLookup("Orchardville Ave", streets.OrchidvilleAve.FileName),
            new StreetLookup("Popular St", streets.PopularStreet.FileName),
            new StreetLookup("Buccaneer Way", streets.BuccanierWay.FileName),
            new StreetLookup("Abattoir Ave", streets.AvatorAve.FileName),
            new StreetLookup("Voodoo Place"),
            new StreetLookup("Mutiny Rd", streets.MutineeRoad.FileName),
            new StreetLookup("South Arsenal St", streets.SouthArsenalStreet.FileName),
            new StreetLookup("Forum Dr", streets.ForumDrive.FileName),
            new StreetLookup("Morningwood Blvd", streets.MorningwoodBlvd.FileName),
            new StreetLookup("Dorset Dr", streets.DorsetDrive.FileName),
            new StreetLookup("Caesars Place", streets.CaesarPlace.FileName),
            new StreetLookup("Spanish Ave", streets.SpanishAve.FileName),
            new StreetLookup("Portola Dr", streets.PortolaDrive.FileName),
            new StreetLookup("Edwood Way", streets.EdwardWay.FileName),
            new StreetLookup("San Vitus Blvd", streets.SanVitusBlvd.FileName),
            new StreetLookup("Eclipse Blvd", streets.EclipseBlvd.FileName),
            new StreetLookup("Gentry Lane"),
            new StreetLookup("Las Lagunas Blvd", streets.LasLegunasBlvd.FileName),
            new StreetLookup("Power St", streets.PowerStreet.FileName),
            new StreetLookup("Mt Haan Rd", streets.MtHaanRoad.FileName),
            new StreetLookup("Elgin Ave", streets.ElginAve.FileName),
            new StreetLookup("Hawick Ave", streets.HawickAve.FileName),
            new StreetLookup("Meteor St", streets.MeteorStreet.FileName),
            new StreetLookup("Alta Pl", streets.AltaPlace.FileName),
            new StreetLookup("Occupation Ave", streets.OccupationAve.FileName),
            new StreetLookup("Carcer Way", streets.CarcerWay.FileName),
            new StreetLookup("Eastbourne Way", streets.EastbourneWay.FileName),
            new StreetLookup("Rockford Dr", streets.RockfordDrive.FileName),
            new StreetLookup("Abe Milton Pkwy", streets.EightMiltonParkway.FileName),
            new StreetLookup("Laguna Pl", streets.LagunaPlace.FileName),
            new StreetLookup("Sinners Passage", streets.SinnersPassage.FileName),
            new StreetLookup("Atlee St", streets.AtleyStreet.FileName),
            new StreetLookup("Sinner St", streets.SinnerStreet.FileName),
            new StreetLookup("Supply St", streets.SupplyStreet.FileName),
            new StreetLookup("Amarillo Way", streets.AmarilloWay.FileName),
            new StreetLookup("Tower Way", streets.TowerWay.FileName),
            new StreetLookup("Decker St", streets.DeckerStreet.FileName),
            new StreetLookup("Tackle St", streets.TackleStreet.FileName),
            new StreetLookup("Low Power St", streets.LowPowerStreet.FileName),
            new StreetLookup("Clinton Ave", streets.ClintonAve.FileName),
            new StreetLookup("Fenwell Pl", streets.FenwellPlace.FileName),
            new StreetLookup("Utopia Gardens", streets.UtopiaGardens.FileName),
            new StreetLookup("Cavalry Blvd"),
            new StreetLookup("South Boulevard Del Perro", streets.SouthBlvdDelPierro.FileName),
            new StreetLookup("Americano Way", streets.AmericanoWay.FileName),
            new StreetLookup("Sam Austin Dr", streets.SamAustinDrive.FileName),
            new StreetLookup("East Galileo Ave", streets.EastGalileoAve.FileName),
            new StreetLookup("Galileo Park"),
            new StreetLookup("West Galileo Ave", streets.WestGalileoAve.FileName),
            new StreetLookup("Tongva Dr", streets.TongvaDrive.FileName),
            new StreetLookup("Zancudo Rd", streets.ZancudoRoad.FileName),
            new StreetLookup("Movie Star Way", streets.MovieStarWay.FileName),
            new StreetLookup("Heritage Way", streets.HeritageWay.FileName),
            new StreetLookup("Perth St", streets.PerfStreet.FileName),
            new StreetLookup("Chianski Passage"),
            new StreetLookup("Lolita Ave", streets.LolitaAve.FileName),
            new StreetLookup("Meringue Ln", streets.MirangeLane.FileName),
            new StreetLookup("Strangeways Dr", streets.StrangeWaysDrive.FileName),

            new StreetLookup("Mt Haan Dr", streets.MtHaanDrive.FileName)
        };
        }
        private class StreetLookup
        {
            public string Name { get; set; } = "";
            public string DispatchFile { get; set; } = "";
            public StreetLookup()
            {

            }
            public StreetLookup(string _Name)
            {
                Name = _Name;
            }
            public StreetLookup(string _Name, string _DispatchFile)
            {
                Name = _Name;
                DispatchFile = _DispatchFile;
            }
        }
    }
    public class gtaiv_legacy_support
    {
        public static ScannerFile InuhhEast { get { return new ScannerFile("01_0_gtaiv_legacy_support\\0x007624B6.wav", "In...uhh... East.", "0_gtaiv_legacy_support"); } }
        public static ScannerFile InuhhWestern { get { return new ScannerFile("01_0_gtaiv_legacy_support\\0x00C21410.wav", "In...uhh... Western", "0_gtaiv_legacy_support"); } }
        public static ScannerFile InuhhWest { get { return new ScannerFile("01_0_gtaiv_legacy_support\\0x040B13FE.wav", "In...uhh... West", "0_gtaiv_legacy_support"); } }
        public static ScannerFile beepplaceholderbeep { get { return new ScannerFile("01_0_gtaiv_legacy_support\\0x06C2C42B.wav", "*beep*placeholder(?)*beep*", "0_gtaiv_legacy_support"); } }
        public static ScannerFile FIBteamdispatchingfrom { get { return new ScannerFile("01_0_gtaiv_legacy_support\\0x0708C2C6.wav", "FIB team dispatching from...", "0_gtaiv_legacy_support"); } }
        public static ScannerFile Dispatchairunitfrom { get { return new ScannerFile("01_0_gtaiv_legacy_support\\0x095C198D.wav", "Dispatch air unit from...", "0_gtaiv_legacy_support"); } }
        public static ScannerFile DispatchingSWATunitsfrom { get { return new ScannerFile("01_0_gtaiv_legacy_support\\0x0F106A48.wav", "Dispatching SWAT units from...", "0_gtaiv_legacy_support"); } }
        public static ScannerFile InuhhNorth { get { return new ScannerFile("01_0_gtaiv_legacy_support\\0x11889985.wav", "In...uhh... North", "0_gtaiv_legacy_support"); } }
        public static ScannerFile InuhhCentral { get { return new ScannerFile("01_0_gtaiv_legacy_support\\0x13A847B7.wav", "In...uhh... Central", "0_gtaiv_legacy_support"); } }
        public static ScannerFile InuhhNorthern { get { return new ScannerFile("01_0_gtaiv_legacy_support\\0x1B806676.wav", "In...uhh... Northern", "0_gtaiv_legacy_support"); } }
        public static ScannerFile InEastern { get { return new ScannerFile("01_0_gtaiv_legacy_support\\0x1CC03B69.wav", "In... Eastern", "0_gtaiv_legacy_support"); } }
        public static ScannerFile InuhhSouth { get { return new ScannerFile("01_0_gtaiv_legacy_support\\0x1D349635.wav", "In...uhh... South", "0_gtaiv_legacy_support"); } }
        public static ScannerFile InuhhSouthern { get { return new ScannerFile("01_0_gtaiv_legacy_support\\0x1D74BE76.wav", "In...uhh... Southern", "0_gtaiv_legacy_support"); } }
    }
    public class gtaiv_legacy_support_small
    {

        public static ScannerFile Suspect { get { return new ScannerFile("01_0_gtaiv_legacy_support_small\\0x003685A2.wav", "Suspect", "0_gtaiv_legacy_support_small"); } }
        public static ScannerFile for121 { get { return new ScannerFile("01_0_gtaiv_legacy_support_small\\0x01A766C8.wav", "for(?)", "0_gtaiv_legacy_support_small"); } }
        public static ScannerFile Uhh { get { return new ScannerFile("01_0_gtaiv_legacy_support_small\\0x01E5D9FA.wav", "Uhh...", "0_gtaiv_legacy_support_small"); } }
        public static ScannerFile Bright { get { return new ScannerFile("01_0_gtaiv_legacy_support_small\\0x0866A86E.wav", "Bright", "0_gtaiv_legacy_support_small"); } }
        public static ScannerFile Dark { get { return new ScannerFile("01_0_gtaiv_legacy_support_small\\0x0B35C659.wav", "Dark", "0_gtaiv_legacy_support_small"); } }
        public static ScannerFile Light { get { return new ScannerFile("01_0_gtaiv_legacy_support_small\\0x0E18707B.wav", "Light", "0_gtaiv_legacy_support_small"); } }
        public static ScannerFile Central { get { return new ScannerFile("01_0_gtaiv_legacy_support_small\\0x0E43511A.wav", "Central", "0_gtaiv_legacy_support_small"); } }
        public static ScannerFile Lastseen { get { return new ScannerFile("01_0_gtaiv_legacy_support_small\\0x15B4F5FC.wav", "Last seen", "0_gtaiv_legacy_support_small"); } }
        public static ScannerFile beep { get { return new ScannerFile("01_0_gtaiv_legacy_support_small\\0x189A930F.wav", "*beep*", "0_gtaiv_legacy_support_small"); } }
        public static ScannerFile Heading { get { return new ScannerFile("01_0_gtaiv_legacy_support_small\\0x19DDC879.wav", "Heading", "0_gtaiv_legacy_support_small"); } }

    }
    public class age
    {
        public static ScannerFile Early50s { get { return new ScannerFile("01_age\\0x00617E13.wav", "Early 50s", "age"); } }
        public static ScannerFile Early20s { get { return new ScannerFile("01_age\\0x028E8E07.wav", "Early 20s", "age"); } }
        public static ScannerFile Young { get { return new ScannerFile("01_age\\0x057E03E3.wav", "Young", "age"); } }
        public static ScannerFile Elderly { get { return new ScannerFile("01_age\\0x09B1D180.wav", "Elderly", "age"); } }
        public static ScannerFile Late20s { get { return new ScannerFile("01_age\\0x0F4F6D73.wav", "Late 20s", "age"); } }
        public static ScannerFile Mid20s { get { return new ScannerFile("01_age\\0x107FA3B0.wav", "Mid 20s", "age"); } }
        public static ScannerFile Late50s { get { return new ScannerFile("01_age\\0x11BC4434.wav", "Late 50s", "age"); } }
        public static ScannerFile Mid50s { get { return new ScannerFile("01_age\\0x164F5F0E.wav", "Mid 50s", "age"); } }
        public static ScannerFile Mid40s { get { return new ScannerFile("01_age\\0x16B57506.wav", "Mid 40s", "age"); } }
        public static ScannerFile Early30s { get { return new ScannerFile("01_age\\0x18ECC893.wav", "Early 30s", "age"); } }
        public static ScannerFile Mid30s { get { return new ScannerFile("01_age\\0x198138E7.wav", "Mid 30s", "age"); } }
        public static ScannerFile Late30s { get { return new ScannerFile("01_age\\0x1A9DE131.wav", "Late 30s", "age"); } }
        public static ScannerFile Late40s { get { return new ScannerFile("01_age\\0x1C74AC5A.wav", "Late 40s", "age"); } }
        public static ScannerFile Teenage { get { return new ScannerFile("01_age\\0x1D5268E6.wav", "Teenage", "age"); } }
        public static ScannerFile Early40s { get { return new ScannerFile("01_age\\0x1F9A8632.wav", "Early 40s", "age"); } }
    }
    public class air_support_shoot_tires
    {
        public static ScannerFile Airsupportceasefireonsuspectsvehicle { get { return new ScannerFile("01_air_support_shoot_tires\\0x00B655FE.wav", "Air support, cease fire on suspect's vehicle.", "air_support_shoot_tires"); } }
        public static ScannerFile Airsupportholdyourfire { get { return new ScannerFile("01_air_support_shoot_tires\\0x03C6DB9C.wav", "Air support, hold your fire.", "air_support_shoot_tires"); } }
        public static ScannerFile Airsupportmoveinatinmobileatsuspectsvehicle { get { return new ScannerFile("01_air_support_shoot_tires\\0x06694A56.wav", "Air support, move in at in mobile at suspect's vehicle.", "air_support_shoot_tires"); } }
        public static ScannerFile Airsupportyouareagotoimmobilizesuspectsvehicle { get { return new ScannerFile("01_air_support_shoot_tires\\0x09FE117F.wav", "Air support, you are a go to immobilize suspect's vehicle.", "air_support_shoot_tires"); } }
        public static ScannerFile Airsupportceasefire { get { return new ScannerFile("01_air_support_shoot_tires\\0x0A63E8D7.wav", "Air support, cease fire.", "air_support_shoot_tires"); } }
        public static ScannerFile Airsupportholdfire { get { return new ScannerFile("01_air_support_shoot_tires\\0x110F362E.wav", "Air support, hold fire.", "air_support_shoot_tires"); } }
        public static ScannerFile Airsupportholdyourfire1 { get { return new ScannerFile("01_air_support_shoot_tires\\0x1A46091E.wav", "Air support, hold your fire.", "air_support_shoot_tires"); } }
        public static ScannerFile Airsupportceasefiringonthevehicle { get { return new ScannerFile("01_air_support_shoot_tires\\0x1AED49EA.wav", "Air support, cease firing on the vehicle.", "air_support_shoot_tires"); } }
        public static ScannerFile Airsupportceasefire1 { get { return new ScannerFile("01_air_support_shoot_tires\\0x1C300C70.wav", "Air support, cease fire.", "air_support_shoot_tires"); } }
        public static ScannerFile Airsupportmoveinandimmobilizesuspectsvehicle { get { return new ScannerFile("01_air_support_shoot_tires\\0x1C36B5F0.wav", "Air support, move in and immobilize suspect's vehicle.", "air_support_shoot_tires"); } }
        public static ScannerFile Airsupportyouarecleartoimmobilizesuspectsvehicle { get { return new ScannerFile("01_air_support_shoot_tires\\0x1F7BFC7C.wav", "Air support, you are clear to immobilize suspect's vehicle.", "air_support_shoot_tires"); } }
        public static ScannerFile Airsupporttakeoutsuspectstires { get { return new ScannerFile("01_air_support_shoot_tires\\0x1FB47CEB.wav", "Air support, take out suspect's tires.", "air_support_shoot_tires"); } }
    }
    public class all_crooks_caught
    {
        public static ScannerFile Allsuspectsapprehended { get { return new ScannerFile("01_all_crooks_caught\\0x08CC5E87.wav", "All suspects apprehended.", "all_crooks_caught"); } }
        public static ScannerFile Allsuspects1015 { get { return new ScannerFile("01_all_crooks_caught\\0x0DAB6838.wav", "All suspects 10-15.", "all_crooks_caught"); } }
        public static ScannerFile Allcriminalsincustody { get { return new ScannerFile("01_all_crooks_caught\\0x1BD6048E.wav", "All criminals in custody.", "all_crooks_caught"); } }
    }
    public class areas
    {
        public static ScannerFile PalmerTaylorPowerStation { get { return new ScannerFile("01_areas\\0x001F9BDF.wav", "Palmer-Taylor Power Station", "areas"); } }
        public static ScannerFile WestVinewood { get { return new ScannerFile("01_areas\\0x006C113D.wav", "West Vinewood", "areas"); } }
        public static ScannerFile Strawberry { get { return new ScannerFile("01_areas\\0x012E7CF0.wav", "Strawberry", "areas"); } }
        public static ScannerFile PaletoBay { get { return new ScannerFile("01_areas\\0x014B6EC2.wav", "Paleto Bay", "areas"); } }
        public static ScannerFile TheMazeBankStadium { get { return new ScannerFile("01_areas\\0x017459B6.wav", "The Maze Bank Stadium", "areas"); } }
        public static ScannerFile VinewoodHills { get { return new ScannerFile("01_areas\\0x0184FB75.wav", "Vinewood Hills", "areas"); } }
        public static ScannerFile SouthLosSantos { get { return new ScannerFile("01_areas\\0x018BAE55.wav", "South Los Santos", "areas"); } }
        public static ScannerFile TextileCity { get { return new ScannerFile("01_areas\\0x01E8415E.wav", "Textile City", "areas"); } }
        public static ScannerFile BayTreeCanyon { get { return new ScannerFile("01_areas\\0x02011728.wav", "Bay Tree Canyon", "areas"); } }
        public static ScannerFile MirrorPark { get { return new ScannerFile("01_areas\\0x021443CE.wav", "Mirror Park", "areas"); } }
        public static ScannerFile TongvaValley { get { return new ScannerFile("01_areas\\0x023C5452.wav", "Tongva Valley", "areas"); } }
        public static ScannerFile RonAlternatesWindFarm { get { return new ScannerFile("01_areas\\0x028CCF7A.wav", "Ron Alternates Wind Farm", "areas"); } }
        public static ScannerFile SlabCity { get { return new ScannerFile("01_areas\\0x029DB946.wav", "Slab City", "areas"); } }
        public static ScannerFile Eclipse { get { return new ScannerFile("01_areas\\0x02CA0B60.wav", "Eclipse", "areas"); } }
        public static ScannerFile LasPuertasFreeway { get { return new ScannerFile("01_areas\\0x02E1BE9C.wav", "Las Puertas Freeway", "areas"); } }
        public static ScannerFile VespucciBeach { get { return new ScannerFile("01_areas\\0x0345886A.wav", "Vespucci Beach", "areas"); } }
        public static ScannerFile MarlowDrive { get { return new ScannerFile("01_areas\\0x0364A059.wav", "Marlow Drive", "areas"); } }
        public static ScannerFile EastLosSantos { get { return new ScannerFile("01_areas\\0x036541D5.wav", "East Los Santos", "areas"); } }
        public static ScannerFile LagoZancudo { get { return new ScannerFile("01_areas\\0x0387465C.wav", "Lago Zancudo", "areas"); } }
        public static ScannerFile Vespucci { get { return new ScannerFile("01_areas\\0x03A3CA25.wav", "Vespucci", "areas"); } }
        public static ScannerFile Terminal { get { return new ScannerFile("01_areas\\0x03C9418A.wav", "Terminal", "areas"); } }
        public static ScannerFile PacificBluffs { get { return new ScannerFile("01_areas\\0x03DBE985.wav", "Pacific Bluffs", "areas"); } }
        public static ScannerFile TheChiliadMountainStateWilderness { get { return new ScannerFile("01_areas\\0x03F7E5D5.wav", "The Chiliad Mountain State Wilderness", "areas"); } }
        public static ScannerFile MuriettaHeights { get { return new ScannerFile("01_areas\\0x04672520.wav", "Murietta Heights", "areas"); } }
        public static ScannerFile MountGordo { get { return new ScannerFile("01_areas\\0x05136C06.wav", "Mount Gordo", "areas"); } }
        public static ScannerFile LasPuertasFreeway1 { get { return new ScannerFile("01_areas\\0x05358344.wav", "Las Puertas Freeway", "areas"); } }
        public static ScannerFile MountChiliad { get { return new ScannerFile("01_areas\\0x05390970.wav", "Mount Chiliad", "areas"); } }
        public static ScannerFile LosSantosInternationalAirport { get { return new ScannerFile("01_areas\\0x05E7E888.wav", "Los Santos International Airport", "areas"); } }
        public static ScannerFile RatonCanyon { get { return new ScannerFile("01_areas\\0x060A34CC.wav", "Raton Canyon", "areas"); } }
        public static ScannerFile Route68approach { get { return new ScannerFile("01_areas\\0x06F346C2.wav", "Route 68 approach", "areas"); } }
        public static ScannerFile LosPorta { get { return new ScannerFile("01_areas\\0x07ADC1D6.wav", "Los Porta(?)", "areas"); } }
        public static ScannerFile Harmony { get { return new ScannerFile("01_areas\\0x07B1DA7C.wav", "Harmony", "areas"); } }
        public static ScannerFile GalileoPark { get { return new ScannerFile("01_areas\\0x07EFD2BE.wav", "Galileo Park", "areas"); } }
        public static ScannerFile LittleSeoul { get { return new ScannerFile("01_areas\\0x0803333B.wav", "Little Seoul", "areas"); } }
        public static ScannerFile PaletoForest { get { return new ScannerFile("01_areas\\0x08D2B05D.wav", "PaletoForest", "areas"); } }
        public static ScannerFile PillboxHill { get { return new ScannerFile("01_areas\\0x092393C4.wav", "PillboxHill", "areas"); } }
        public static ScannerFile VespucciCanal { get { return new ScannerFile("01_areas\\0x092C44E3.wav", "VespucciCanal", "areas"); } }
        public static ScannerFile MirrorPark_1 { get { return new ScannerFile("01_areas\\0x09F6938F.wav", "MirrorPark", "areas"); } }
        public static ScannerFile PartolaDrive { get { return new ScannerFile("01_areas\\0x0A0F49DE.wav", "PartolaDrive", "areas"); } }
        public static ScannerFile PortOfSouthLosSantos { get { return new ScannerFile("01_areas\\0x0A4783C2.wav", "PortOfSouthLosSantos", "areas"); } }
        public static ScannerFile LosPuertesStadium { get { return new ScannerFile("01_areas\\0x0A5D231E.wav", "LosPuertesStadium", "areas"); } }
        public static ScannerFile SandyShores { get { return new ScannerFile("01_areas\\0x0AFA8A12.wav", "SandyShores", "areas"); } }
        public static ScannerFile GrandeSonoranDesert { get { return new ScannerFile("01_areas\\0x0B00BF56.wav", "GrandeSonoranDesert", "areas"); } }
        public static ScannerFile ProcopioBeach { get { return new ScannerFile("01_areas\\0x0B8229F0.wav", "ProcopioBeach", "areas"); } }
        public static ScannerFile NorthChumash { get { return new ScannerFile("01_areas\\0x0B9376DB.wav", "NorthChumash", "areas"); } }
        public static ScannerFile LosSantosFreeway { get { return new ScannerFile("01_areas\\0x0BB07036.wav", "LosSantosFreeway", "areas"); } }
        public static ScannerFile MazeBankArena { get { return new ScannerFile("01_areas\\0x0BC694DB.wav", "MazeBankArena", "areas"); } }
        public static ScannerFile UtopiaGardens { get { return new ScannerFile("01_areas\\0x0BDCCEF7.wav", "UtopiaGardens", "areas"); } }
        public static ScannerFile LosSantosInternational { get { return new ScannerFile("01_areas\\0x0BFD1A02.wav", "LosSantosInternational", "areas"); } }
        public static ScannerFile MtJosiah { get { return new ScannerFile("01_areas\\0x0CBDAD0D.wav", "MtJosiah", "areas"); } }
        public static ScannerFile TheRedwoodLightsTrack { get { return new ScannerFile("01_areas\\0x0D2920BD.wav", "TheRedwoodLightsTrack", "areas"); } }
        public static ScannerFile LosSantosInternational_1 { get { return new ScannerFile("01_areas\\0x0D4D375A.wav", "LosSantosInternational", "areas"); } }
        public static ScannerFile Chumash { get { return new ScannerFile("01_areas\\0x0D68048E.wav", "Chumash", "areas"); } }
        public static ScannerFile Chumash_1 { get { return new ScannerFile("01_areas\\0x0D9B84EB.wav", "Chumash", "areas"); } }
        public static ScannerFile PorcopiaTruckStop { get { return new ScannerFile("01_areas\\0x0DAB6018.wav", "PorcopiaTruckStop", "areas"); } }
        public static ScannerFile RockfordHills { get { return new ScannerFile("01_areas\\0x0E1A3BD7.wav", "RockfordHills", "areas"); } }
        public static ScannerFile EastVinewood { get { return new ScannerFile("01_areas\\0x0E1C639C.wav", "EastVinewood", "areas"); } }
        public static ScannerFile SouthLosSantos_1 { get { return new ScannerFile("01_areas\\0x0E61C7FF.wav", "SouthLosSantos", "areas"); } }
        public static ScannerFile GreatOceanHighway { get { return new ScannerFile("01_areas\\0x0EA94CB1.wav", "GreatOceanHighway", "areas"); } }
        public static ScannerFile GreatChapparalle { get { return new ScannerFile("01_areas\\0x0EA9C578.wav", "GreatChapparalle", "areas"); } }
        public static ScannerFile ChamberlainHills { get { return new ScannerFile("01_areas\\0x0EF6ED8C.wav", "ChamberlainHills", "areas"); } }
        public static ScannerFile Vinewood { get { return new ScannerFile("01_areas\\0x0F2B8859.wav", "Vinewood", "areas"); } }
        public static ScannerFile DorsetDrive { get { return new ScannerFile("01_areas\\0x0F78F5C9.wav", "DorsetDrive", "areas"); } }
        public static ScannerFile BraddockPass { get { return new ScannerFile("01_areas\\0x0FA9EE2A.wav", "BraddockPass", "areas"); } }
        public static ScannerFile TongaHills { get { return new ScannerFile("01_areas\\0x0FF01B2B.wav", "TongaHills", "areas"); } }
        public static ScannerFile PacificBluffs_1 { get { return new ScannerFile("01_areas\\0x101A0201.wav", "PacificBluffs", "areas"); } }
        public static ScannerFile LaPuertes { get { return new ScannerFile("01_areas\\0x106A6182.wav", "LaPuertes", "areas"); } }
        public static ScannerFile Richman { get { return new ScannerFile("01_areas\\0x10C9CE0E.wav", "Richman", "areas"); } }
        public static ScannerFile SanAndreas { get { return new ScannerFile("01_areas\\0x114D6EDA.wav", "SanAndreas", "areas"); } }
        public static ScannerFile DavisCourts { get { return new ScannerFile("01_areas\\0x118C3C55.wav", "DavisCourts", "areas"); } }
        public static ScannerFile TheAlamaSea { get { return new ScannerFile("01_areas\\0x11EC2FB7.wav", "TheAlamaSea", "areas"); } }
        public static ScannerFile CountrySide { get { return new ScannerFile("01_areas\\0x12D2E37E.wav", "CountrySide", "areas"); } }
        public static ScannerFile Burton { get { return new ScannerFile("01_areas\\0x12FF796D.wav", "Burton", "areas"); } }
        public static ScannerFile TheBraddockTunnel { get { return new ScannerFile("01_areas\\0x13538DA2.wav", "TheBraddockTunnel", "areas"); } }
        public static ScannerFile Alta { get { return new ScannerFile("01_areas\\0x138D4EAB.wav", "Alta", "areas"); } }
        public static ScannerFile EastLosSantos_1 { get { return new ScannerFile("01_areas\\0x13A22250.wav", "EastLosSantos", "areas"); } }
        public static ScannerFile MorningWood { get { return new ScannerFile("01_areas\\0x13C5B629.wav", "MorningWood", "areas"); } }
        public static ScannerFile BoilingBrookPenitentiary { get { return new ScannerFile("01_areas\\0x13CCE7C9.wav", "BoilingBrookPenitentiary", "areas"); } }
        public static ScannerFile Banning { get { return new ScannerFile("01_areas\\0x13F1BE48.wav", "Banning", "areas"); } }
        public static ScannerFile TheRaceCourse { get { return new ScannerFile("01_areas\\0x13F539FA.wav", "TheRaceCourse", "areas"); } }
        public static ScannerFile ChilliadMountainStWilderness { get { return new ScannerFile("01_areas\\0x140A2311.wav", "0x140A2311", "areas"); } }
        public static ScannerFile DowntownVinewood { get { return new ScannerFile("01_areas\\0x149F97C8.wav", "DowntownVinewood", "areas"); } }
        public static ScannerFile Downtown { get { return new ScannerFile("01_areas\\0x14E31C13.wav", "Downtown", "areas"); } }
        public static ScannerFile PorcopioBeach { get { return new ScannerFile("01_areas\\0x14FF4DA7.wav", "PorcopioBeach", "areas"); } }
        public static ScannerFile DelPierro { get { return new ScannerFile("01_areas\\0x153EE641.wav", "DelPierro", "areas"); } }
        public static ScannerFile ZancudoRiver { get { return new ScannerFile("01_areas\\0x15BABEA1.wav", "ZancudoRiver", "areas"); } }
        public static ScannerFile Richman_1 { get { return new ScannerFile("01_areas\\0x16E5DA45.wav", "Richman_1", "areas"); } }
        public static ScannerFile FtZancudo { get { return new ScannerFile("01_areas\\0x16E98448.wav", "FtZancudo", "areas"); } }
        public static ScannerFile MissionRow { get { return new ScannerFile("01_areas\\0x17235ECC.wav", "MissionRow", "areas"); } }
        public static ScannerFile Howard { get { return new ScannerFile("01_areas\\0x17919CF8.wav", "Howard", "areas"); } }
        public static ScannerFile SanTiaskiMtRange { get { return new ScannerFile("01_areas\\0x17AD6380.wav", "SanTiaskiMtRange", "areas"); } }
        public static ScannerFile TheRedwoodLightsTrack_1 { get { return new ScannerFile("01_areas\\0x17BA758E.wav", "TheRedwoodLightsTrack", "areas"); } }
        public static ScannerFile ZancudoRiver_1 { get { return new ScannerFile("01_areas\\0x17D282D1.wav", "ZancudoRiver", "areas"); } }
        public static ScannerFile SonoraWindfarm { get { return new ScannerFile("01_areas\\0x17E495A5.wav", "SonoraWindfarm", "areas"); } }
        public static ScannerFile BacklotCity { get { return new ScannerFile("01_areas\\0x17F11DAD.wav", "BacklotCity", "areas"); } }
        public static ScannerFile ElysianIsland { get { return new ScannerFile("01_areas\\0x1891CD57.wav", "ElysianIsland", "areas"); } }
        public static ScannerFile PortOfSouthLosSantos_1 { get { return new ScannerFile("01_areas\\0x1891E057.wav", "PortOfSouthLosSantos_1", "areas"); } }
        public static ScannerFile TheMovieStudios { get { return new ScannerFile("01_areas\\0x189EC125.wav", "TheMovieStudios", "areas"); } }
        public static ScannerFile TheStadium { get { return new ScannerFile("01_areas\\0x18C2B8B5.wav", "TheStadium", "areas"); } }
        public static ScannerFile LaMesa { get { return new ScannerFile("01_areas\\0x18C85309.wav", "LaMesa", "areas"); } }
        public static ScannerFile Downtown_1 { get { return new ScannerFile("01_areas\\0x18D0A3EF.wav", "Downtown", "areas"); } }
        public static ScannerFile TheGWCGolfingSociety { get { return new ScannerFile("01_areas\\0x1910AD05.wav", "TheGWCGolfingSociety", "areas"); } }
        public static ScannerFile LittleSeoul_1 { get { return new ScannerFile("01_areas\\0x19319598.wav", "LittleSeoul", "areas"); } }
        public static ScannerFile NorthYankton { get { return new ScannerFile("01_areas\\0x193D6DBC.wav", "NorthYankton", "areas"); } }
        public static ScannerFile SandyShores_1 { get { return new ScannerFile("01_areas\\0x19BBE794.wav", "SandyShores", "areas"); } }
        public static ScannerFile Chumash_2 { get { return new ScannerFile("01_areas\\0x1ACA5F52.wav", "Chumash", "areas"); } }
        public static ScannerFile APMTerminal { get { return new ScannerFile("01_areas\\0x1AD23A10.wav", "APMTerminal", "areas"); } }
        public static ScannerFile PuertoDelSoul { get { return new ScannerFile("01_areas\\0x1B64BCB8.wav", "PuertoDelSoul", "areas"); } }
        public static ScannerFile Chumash_3 { get { return new ScannerFile("01_areas\\0x1B6CE08C.wav", "Chumash", "areas"); } }
        public static ScannerFile RichmanGlenn { get { return new ScannerFile("01_areas\\0x1BD63BBB.wav", "RichmanGlenn", "areas"); } }
        public static ScannerFile DelPierroBeach { get { return new ScannerFile("01_areas\\0x1C4C6513.wav", "DelPierroBeach", "areas"); } }
        public static ScannerFile PalominoHighlands { get { return new ScannerFile("01_areas\\0x1C528EF2.wav", "PalominoHighlands", "areas"); } }
        public static ScannerFile SonoraFreeway { get { return new ScannerFile("01_areas\\0x1C6991D6.wav", "SonoraFreeway", "areas"); } }
        public static ScannerFile RedwoodLightsStadium { get { return new ScannerFile("01_areas\\0x1C8CEC13.wav", "RedwoodLightsStadium", "areas"); } }
        public static ScannerFile CypressFlats { get { return new ScannerFile("01_areas\\0x1C9D42A2.wav", "CypressFlats", "areas"); } }
        public static ScannerFile Grapeseed { get { return new ScannerFile("01_areas\\0x1C9D6CA9.wav", "Grapeseed", "areas"); } }
        public static ScannerFile TatathiaMountains { get { return new ScannerFile("01_areas\\0x1CC50A90.wav", "TatathiaMountains", "areas"); } }
        public static ScannerFile LosPuerta { get { return new ScannerFile("01_areas\\0x1CD22C22.wav", "LosPuerta", "areas"); } }
        public static ScannerFile LostMCClubhouse { get { return new ScannerFile("01_areas\\0x1D127686.wav", "LostMCClubhouse", "areas"); } }
        public static ScannerFile LaPorta { get { return new ScannerFile("01_areas\\0x1D9900EF.wav", "LaPorta", "areas"); } }
        public static ScannerFile TheOcean { get { return new ScannerFile("01_areas\\0x1D9FFECB.wav", "TheOcean", "areas"); } }
        public static ScannerFile RichardsMajesticStudio { get { return new ScannerFile("01_areas\\0x1DF1BA88.wav", "RichardsMajesticStudio", "areas"); } }
        public static ScannerFile HeartAttacksBeach { get { return new ScannerFile("01_areas\\0x1E0EF1FA.wav", "HeartAttacksBeach", "areas"); } }
        public static ScannerFile Rancho { get { return new ScannerFile("01_areas\\0x1E8562C5.wav", "Rancho", "areas"); } }
        public static ScannerFile Davis { get { return new ScannerFile("01_areas\\0x1ED41C86.wav", "Davis", "areas"); } }
        public static ScannerFile LaPortaFreeway { get { return new ScannerFile("01_areas\\0x1EE53212.wav", "LaPortaFreeway", "areas"); } }
        public static ScannerFile ElBerroHights { get { return new ScannerFile("01_areas\\0x1EF77B93.wav", "ElBerroHights", "areas"); } }
        public static ScannerFile DelPierro_1 { get { return new ScannerFile("01_areas\\0x1F7B7ABB.wav", "DelPierro", "areas"); } }
        public static ScannerFile RockfordHills_1 { get { return new ScannerFile("01_areas\\0x1FD39F4A.wav", "RockfordHills", "areas"); } }
    }
    public class area_direction
    {
        public static ScannerFile Northern { get { return new ScannerFile("01_area_direction\\0x00C822E3.wav", "Northern", "area_direction"); } }
        public static ScannerFile Eastern { get { return new ScannerFile("01_area_direction\\0x00D20737.wav", "Eastern", "area_direction"); } }
        public static ScannerFile Western { get { return new ScannerFile("01_area_direction\\0x03F4BFA1.wav", "Western", "area_direction"); } }
        public static ScannerFile Central { get { return new ScannerFile("01_area_direction\\0x04886DF6.wav", "Central", "area_direction"); } }
        public static ScannerFile Central1 { get { return new ScannerFile("01_area_direction\\0x088F3603.wav", "Central", "area_direction"); } }
        public static ScannerFile Southern { get { return new ScannerFile("01_area_direction\\0x0A6BC575.wav", "Southern", "area_direction"); } }
        public static ScannerFile Central1_1 { get { return new ScannerFile("01_area_direction\\0x123E8963.wav", "Central", "area_direction"); } }
    }
    public class assistance_required
    {
        public static ScannerFile Backupneeded { get { return new ScannerFile("01_assistance_required\\0x04BBD783.wav", "Backup needed.", "assistance_required"); } }
        public static ScannerFile Officersneeded { get { return new ScannerFile("01_assistance_required\\0x09EF61E7.wav", "Officers needed.", "assistance_required"); } }
        public static ScannerFile Assistancerequired { get { return new ScannerFile("01_assistance_required\\0x0D3F2889.wav", "Assistance required.", "assistance_required"); } }
        public static ScannerFile Backuprequired { get { return new ScannerFile("01_assistance_required\\0x0F0BEC23.wav", "Backup required.", "assistance_required"); } }
        public static ScannerFile Officersrequired { get { return new ScannerFile("01_assistance_required\\0x167DBB07.wav", "Officers required.", "assistance_required"); } }
        public static ScannerFile Assistanceneeded { get { return new ScannerFile("01_assistance_required\\0x1D4D88A6.wav", "Assistance needed.", "assistance_required"); } }
    }
    public class attempt_to_find
    {
        public static ScannerFile AllunitsATonsuspects20 { get { return new ScannerFile("01_attempt_to_find\\0x04F0EE36.wav", "All units, AT on suspect's 20.", "attempt_to_find"); } }
        public static ScannerFile Allunitsattempttoreacquirevisual { get { return new ScannerFile("01_attempt_to_find\\0x0CD4FDFF.wav", "All units, attempt to reacquire visual.", "attempt_to_find"); } }
        public static ScannerFile RemainintheareaATL20onsuspect { get { return new ScannerFile("01_attempt_to_find\\0x0F32C2BA.wav", "Remain in the area; AT-L20 on suspect.", "attempt_to_find"); } }
        public static ScannerFile Allunitsattempttoreacquire { get { return new ScannerFile("01_attempt_to_find\\0x178D9375.wav", "All units, attempt to reacquire.", "attempt_to_find"); } }
        public static ScannerFile RemainintheareaATL20onsuspect1 { get { return new ScannerFile("01_attempt_to_find\\0x1A92197A.wav", "Remain in the area; AT-L20 on suspect.", "attempt_to_find"); } }
    }
    public class attention_all_area_units
    {
        public static ScannerFile CentralAreaUnits { get { return new ScannerFile("01_attention_all_area_units\\0x002E9CB4.wav", "Any unit in the central area,", "attention_all_area_units"); } }
        public static ScannerFile PortOfLosSantosUnits { get { return new ScannerFile("01_attention_all_area_units\\0x006919FF.wav", "Any unit in the Port of Los Santos area,", "attention_all_area_units"); } }
        public static ScannerFile MajesticCountyUnits { get { return new ScannerFile("01_attention_all_area_units\\0x0183A299.wav", "Attention all Majestic County units,", "attention_all_area_units"); } }
        public static ScannerFile AirTrafficUnits { get { return new ScannerFile("01_attention_all_area_units\\0x026A5966.wav", "Dispatch to all air traffic units,", "attention_all_area_units"); } }
        public static ScannerFile ChumashUnits { get { return new ScannerFile("01_attention_all_area_units\\0x0271A78D.wav", "Dispatch to any available Chumash unit,", "attention_all_area_units"); } }
        public static ScannerFile DowntownUnits { get { return new ScannerFile("01_attention_all_area_units\\0x02C5949A.wav", "Any unit in the Downtown area,", "attention_all_area_units"); } }
        public static ScannerFile CentralUnits { get { return new ScannerFile("01_attention_all_area_units\\0x02F56240.wav", "Dispatch to any available central unit,", "attention_all_area_units"); } }
        public static ScannerFile MajesticCountyUnits1 { get { return new ScannerFile("01_attention_all_area_units\\0x032265D7.wav", "Dispatch to any available Majestic County unit,", "attention_all_area_units"); } }
        public static ScannerFile PaletoBayUnits { get { return new ScannerFile("01_attention_all_area_units\\0x032E41B8.wav", "0x032E41B8", "attention_all_area_units"); } }
        public static ScannerFile CapeCatfishUnits { get { return new ScannerFile("01_attention_all_area_units\\0x0361C69C.wav", "0x0361C69C", "attention_all_area_units"); } }
        public static ScannerFile VenturaCounty { get { return new ScannerFile("01_attention_all_area_units\\0x0412FFEA.wav", "0x0412FFEA", "attention_all_area_units"); } }
        public static ScannerFile EastLosSantosUnits { get { return new ScannerFile("01_attention_all_area_units\\0x04473B6D.wav", "0x04473B6D", "attention_all_area_units"); } }
        public static ScannerFile CoastCuardUnits { get { return new ScannerFile("01_attention_all_area_units\\0x05922078.wav", "0x05922078", "attention_all_area_units"); } }
        public static ScannerFile ZancudoRiverUnits { get { return new ScannerFile("01_attention_all_area_units\\0x05A60B35.wav", "0x05A60B35", "attention_all_area_units"); } }
        public static ScannerFile GalilleUnits { get { return new ScannerFile("01_attention_all_area_units\\0x07872AFA.wav", "0x07872AFA", "attention_all_area_units"); } }
        public static ScannerFile VespucciAreaUnits { get { return new ScannerFile("01_attention_all_area_units\\0x07A78326.wav", "0x07A78326", "attention_all_area_units"); } }
        public static ScannerFile AirTrafficUnits1 { get { return new ScannerFile("01_attention_all_area_units\\0x0804A4A0.wav", "0x0804A4A0", "attention_all_area_units"); } }
        public static ScannerFile RedwoodLightsUnits { get { return new ScannerFile("01_attention_all_area_units\\0x0A089398.wav", "0x0A089398", "attention_all_area_units"); } }
        public static ScannerFile SandyShoreUnits { get { return new ScannerFile("01_attention_all_area_units\\0x0A36E84D.wav", "0x0A36E84D", "attention_all_area_units"); } }
        public static ScannerFile PortOfLosSantosUnits1 { get { return new ScannerFile("01_attention_all_area_units\\0x0AB3AEA2.wav", "0x0AB3AEA2", "attention_all_area_units"); } }
        public static ScannerFile KalafiaBridgeUnits { get { return new ScannerFile("01_attention_all_area_units\\0x0B8B47C9.wav", "0x0B8B47C9", "attention_all_area_units"); } }
        public static ScannerFile MajesticCountyUnits2 { get { return new ScannerFile("01_attention_all_area_units\\0x0CC93925.wav", "0x0CC93925", "attention_all_area_units"); } }
        public static ScannerFile ChumashUnits1 { get { return new ScannerFile("01_attention_all_area_units\\0x0D257CF4.wav", "0x0D257CF4", "attention_all_area_units"); } }
        public static ScannerFile CassidyCreekUnits { get { return new ScannerFile("01_attention_all_area_units\\0x0D43C2A8.wav", "0x0D43C2A8", "attention_all_area_units"); } }
        public static ScannerFile LandActDamnUnits { get { return new ScannerFile("01_attention_all_area_units\\0x0D8B042A.wav", "0x0D8B042A", "attention_all_area_units"); } }
        public static ScannerFile DowntownUnits1 { get { return new ScannerFile("01_attention_all_area_units\\0x0DBA6A78.wav", "0x0DBA6A78", "attention_all_area_units"); } }
        public static ScannerFile HumaneLabsUnits { get { return new ScannerFile("01_attention_all_area_units\\0x0E5F5E95.wav", "0x0E5F5E95", "attention_all_area_units"); } }
        public static ScannerFile PaletoBayDeltaUnits { get { return new ScannerFile("01_attention_all_area_units\\0x0F28DEDA.wav", "0x0F28DEDA", "attention_all_area_units"); } }
        public static ScannerFile VinewoodHillsUnits { get { return new ScannerFile("01_attention_all_area_units\\0x0F6BBEC8.wav", "0x0F6BBEC8", "attention_all_area_units"); } }
        public static ScannerFile ChumashUnits2 { get { return new ScannerFile("01_attention_all_area_units\\0x104FC34A.wav", "0x104FC34A", "attention_all_area_units"); } }
        public static ScannerFile BollingBrokeUnits { get { return new ScannerFile("01_attention_all_area_units\\0x10A654A4.wav", "0x10A654A4", "attention_all_area_units"); } }
        public static ScannerFile DowntownUnits2 { get { return new ScannerFile("01_attention_all_area_units\\0x10FC3107.wav", "0x10FC3107", "attention_all_area_units"); } }
        public static ScannerFile PortOfLosSantosUnits2 { get { return new ScannerFile("01_attention_all_area_units\\0x11137B4A.wav", "0x11137B4A", "attention_all_area_units"); } }
        public static ScannerFile VinewoodHillsUnits1 { get { return new ScannerFile("01_attention_all_area_units\\0x1145C27E.wav", "0x1145C27E", "attention_all_area_units"); } }
        public static ScannerFile CoastGuardUnits2 { get { return new ScannerFile("01_attention_all_area_units\\0x126F7A3C.wav", "0x126F7A3C", "attention_all_area_units"); } }
        public static ScannerFile EastLosSantosUnits1 { get { return new ScannerFile("01_attention_all_area_units\\0x127917D1.wav", "0x127917D1", "attention_all_area_units"); } }
        public static ScannerFile CentralUnits1 { get { return new ScannerFile("01_attention_all_area_units\\0x12D3C1FD.wav", "0x12D3C1FD", "attention_all_area_units"); } }
        public static ScannerFile SandyShoresUnits2 { get { return new ScannerFile("01_attention_all_area_units\\0x148BFCF6.wav", "0x148BFCF6", "attention_all_area_units"); } }
        public static ScannerFile DowntownUnits3 { get { return new ScannerFile("01_attention_all_area_units\\0x14EC78E8.wav", "0x14EC78E8", "attention_all_area_units"); } }
        public static ScannerFile VenturaCountyUnits { get { return new ScannerFile("01_attention_all_area_units\\0x1522220A.wav", "0x1522220A", "attention_all_area_units"); } }
        public static ScannerFile AlamoSeaUnits { get { return new ScannerFile("01_attention_all_area_units\\0x1580264A.wav", "0x1580264A", "attention_all_area_units"); } }
        public static ScannerFile VespucciAreaUnits2 { get { return new ScannerFile("01_attention_all_area_units\\0x15DFDF96.wav", "0x15DFDF96", "attention_all_area_units"); } }
        public static ScannerFile VenturaCountyArea2 { get { return new ScannerFile("01_attention_all_area_units\\0x16B56530.wav", "0x16B56530", "attention_all_area_units"); } }
        public static ScannerFile HASH16C28216 { get { return new ScannerFile("01_attention_all_area_units\\0x16C28216.wav", "0x16C28216", "attention_all_area_units"); } }
        public static ScannerFile VinewoodUnits2 { get { return new ScannerFile("01_attention_all_area_units\\0x16D8A845.wav", "0x16D8A845", "attention_all_area_units"); } }
        public static ScannerFile SanyShoreUnits3 { get { return new ScannerFile("01_attention_all_area_units\\0x17DBC397.wav", "0x17DBC397", "attention_all_area_units"); } }
        public static ScannerFile VinewoodRacetrackUnits { get { return new ScannerFile("01_attention_all_area_units\\0x1824EF93.wav", "0x1824EF93", "attention_all_area_units"); } }
        public static ScannerFile HASH183605CA { get { return new ScannerFile("01_attention_all_area_units\\0x183605CA.wav", "0x183605CA", "attention_all_area_units"); } }
        public static ScannerFile ElGordoLightHouseUnits { get { return new ScannerFile("01_attention_all_area_units\\0x1843E726.wav", "0x1843E726", "attention_all_area_units"); } }
        public static ScannerFile HASH184885E5 { get { return new ScannerFile("01_attention_all_area_units\\0x184885E5.wav", "0x184885E5", "attention_all_area_units"); } }
        public static ScannerFile HASH18709072 { get { return new ScannerFile("01_attention_all_area_units\\0x18709072.wav", "0x18709072", "attention_all_area_units"); } }
        public static ScannerFile PaletoaBayUnits { get { return new ScannerFile("01_attention_all_area_units\\0x18FFAD5C.wav", "0x18FFAD5C", "attention_all_area_units"); } }
        public static ScannerFile HASH1A434396 { get { return new ScannerFile("01_attention_all_area_units\\0x1A434396.wav", "0x1A434396", "attention_all_area_units"); } }
        public static ScannerFile HASH1BBD320D { get { return new ScannerFile("01_attention_all_area_units\\0x1BBD320D.wav", "0x1BBD320D", "attention_all_area_units"); } }
        public static ScannerFile HASH1BC9F71B { get { return new ScannerFile("01_attention_all_area_units\\0x1BC9F71B.wav", "0x1BC9F71B", "attention_all_area_units"); } }
        public static ScannerFile HASH1BE850F4 { get { return new ScannerFile("01_attention_all_area_units\\0x1BE850F4.wav", "0x1BE850F4", "attention_all_area_units"); } }
        public static ScannerFile HASH1C807125 { get { return new ScannerFile("01_attention_all_area_units\\0x1C807125.wav", "0x1C807125", "attention_all_area_units"); } }
        public static ScannerFile VinewoodHillsUnits3 { get { return new ScannerFile("01_attention_all_area_units\\0x1CA5193B.wav", "0x1CA5193B", "attention_all_area_units"); } }
        public static ScannerFile SouthLosSantosAreaUnits { get { return new ScannerFile("01_attention_all_area_units\\0x1CEB8EB7.wav", "0x1CEB8EB7", "attention_all_area_units"); } }
        public static ScannerFile VinewoodAreaUnits { get { return new ScannerFile("01_attention_all_area_units\\0x1DA275D7.wav", "0x1DA275D7", "attention_all_area_units"); } }
        public static ScannerFile PaletoBayUnits3 { get { return new ScannerFile("01_attention_all_area_units\\0x1DCDF6F8.wav", "0x1DCDF6F8", "attention_all_area_units"); } }
        public static ScannerFile SouthLosSantosUnits { get { return new ScannerFile("01_attention_all_area_units\\0x1E7BD1D7.wav", "0x1E7BD1D7", "attention_all_area_units"); } }
        public static ScannerFile HASH1E8634D1 { get { return new ScannerFile("01_attention_all_area_units\\0x1E8634D1.wav", "0x1E8634D1", "attention_all_area_units"); } }
        public static ScannerFile EastLosSantos2 { get { return new ScannerFile("01_attention_all_area_units\\0x1EC9F072.wav", "0x1EC9F072", "attention_all_area_units"); } }
        public static ScannerFile HASH1F400CC0 { get { return new ScannerFile("01_attention_all_area_units\\0x1F400CC0.wav", "0x1F400CC0", "attention_all_area_units"); } }
        public static ScannerFile HASH1FE288E6 { get { return new ScannerFile("01_attention_all_area_units\\0x1FE288E6.wav", "0x1FE288E6", "attention_all_area_units"); } }
        public static ScannerFile DowntownUnits4 { get { return new ScannerFile("01_attention_all_area_units\\0x1FF0CEE5.wav", "0x1FF0CEE5", "attention_all_area_units"); } }
    }
    public class attention_all_units_gen
    {
        public static ScannerFile Attentionallunits { get { return new ScannerFile("01_attention_all_units_gen\\0x0834F3C0.wav", "Attention all units,", "attention_all_units_gen"); } }
        public static ScannerFile Attentionallunits1 { get { return new ScannerFile("01_attention_all_units_gen\\0x0E42FFE4.wav", "Attention all units,", "attention_all_units_gen"); } }
        public static ScannerFile Allunits { get { return new ScannerFile("01_attention_all_units_gen\\0x15078D6D.wav", "All units,", "attention_all_units_gen"); } }
        public static ScannerFile Attentionallunits2 { get { return new ScannerFile("01_attention_all_units_gen\\0x1D7ADE4C.wav", "Attention all units,", "attention_all_units_gen"); } }
        public static ScannerFile Attentionallunits3 { get { return new ScannerFile("01_attention_all_units_gen\\0x1EC1A0E1.wav", "Attention all units,", "attention_all_units_gen"); } }
    }
    public class attention_generic
    {
        public static ScannerFile AttentionThisisdispatch { get { return new ScannerFile("01_attention_generic\\0x053CA2E6.wav", "Attention. This is dispatch.", "attention_generic"); } }
        public static ScannerFile Attentionthisisdispatch { get { return new ScannerFile("01_attention_generic\\0x12043C6C.wav", "Attention, this is dispatch.", "attention_generic"); } }
    }
    public class attention_specific
    {
        public static ScannerFile Dispatchtocarumm { get { return new ScannerFile("01_attention_specific\\0x066E5F2B.wav", "Dispatch to car umm...", "attention_specific"); } }
        public static ScannerFile Attentioncaruhh { get { return new ScannerFile("01_attention_specific\\0x073C20C6.wav", "Attention car uhh...", "attention_specific"); } }
        public static ScannerFile Dispatchtocarumm1 { get { return new ScannerFile("01_attention_specific\\0x14D23BEE.wav", "Dispatch to car umm...", "attention_specific"); } }
        public static ScannerFile Attentioncaruhh1 { get { return new ScannerFile("01_attention_specific\\0x1A1846A3.wav", "Attention car uhh...", "attention_specific"); } }
        public static ScannerFile Attentioncaruhhorof { get { return new ScannerFile("01_attention_specific\\0x1D930D74.wav", "Attention car uhh... (or 'of')", "attention_specific"); } }
    }
    public class attention_unit_specific
    {
        public static ScannerFile Dispatchcallingunitumm { get { return new ScannerFile("01_attention_unit_specific\\0x0AA6BFE8.wav", "Dispatch calling unit umm...", "attention_unit_specific"); } }
        public static ScannerFile Dispatchcallingunit { get { return new ScannerFile("01_attention_unit_specific\\0x1874DB8F.wav", "Dispatch calling unit...", "attention_unit_specific"); } }
        public static ScannerFile Attentionunit { get { return new ScannerFile("01_attention_unit_specific\\0x1E2DE701.wav", "Attention unit...", "attention_unit_specific"); } }
    }
    public class build
    {
        public static ScannerFile BUILDATHLETIC01 { get { return new ScannerFile("01_build\\BUILD_ATHLETIC_01.wav", "BUILD_ATHLETIC_01", "build"); } }
        public static ScannerFile BUILDAVERAGE01 { get { return new ScannerFile("01_build\\BUILD_AVERAGE_01.wav", "BUILD_AVERAGE_01", "build"); } }
        public static ScannerFile BUILDHEAVY01 { get { return new ScannerFile("01_build\\BUILD_HEAVY_01.wav", "BUILD_HEAVY_01", "build"); } }
        public static ScannerFile BUILDMUSCULAR01 { get { return new ScannerFile("01_build\\BUILD_MUSCULAR_01.wav", "BUILD_MUSCULAR_01", "build"); } }
        public static ScannerFile BUILDOBESE01 { get { return new ScannerFile("01_build\\BUILD_OBESE_01.wav", "BUILD_OBESE_01", "build"); } }
        public static ScannerFile BUILDSLENDER01 { get { return new ScannerFile("01_build\\BUILD_SLENDER_01.wav", "BUILD_SLENDER_01", "build"); } }
        public static ScannerFile BUILDTHIN01 { get { return new ScannerFile("01_build\\BUILD_THIN_01.wav", "BUILD_THIN_01", "build"); } }
    }
    public class car
    {
        public static ScannerFile CAR01 { get { return new ScannerFile("01_car\\CAR_01.wav", "CAR_01", "car"); } }
        public static ScannerFile CAR02 { get { return new ScannerFile("01_car\\CAR_02.wav", "CAR_02", "car"); } }
        public static ScannerFile CAR03 { get { return new ScannerFile("01_car\\CAR_03.wav", "CAR_03", "car"); } }
        public static ScannerFile CAR04 { get { return new ScannerFile("01_car\\CAR_04.wav", "CAR_04", "car"); } }
        public static ScannerFile CAR05 { get { return new ScannerFile("01_car\\CAR_05.wav", "CAR_05", "car"); } }
    }
    public class carrying_weapon
    {
        public static ScannerFile Carryingagrenadelauncher { get { return new ScannerFile("01_carrying_weapon\\0x00FE11D6.wav", "Carrying a grenade launcher.", "carrying_weapon"); } }
        public static ScannerFile ArmedwithanRPG { get { return new ScannerFile("01_carrying_weapon\\0x01007DCB.wav", "Armed with an RPG.", "carrying_weapon"); } }
        public static ScannerFile Armedwithexplosives { get { return new ScannerFile("01_carrying_weapon\\0x01948EA6.wav", "Armed with explosives.", "carrying_weapon"); } }
        public static ScannerFile Carryingaknife { get { return new ScannerFile("01_carrying_weapon\\0x01A0F654.wav", "Carrying a knife.", "carrying_weapon"); } }
        public static ScannerFile Armedwithagun { get { return new ScannerFile("01_carrying_weapon\\0x048A699A.wav", "Armed with a gun.", "carrying_weapon"); } }
        public static ScannerFile Carryingabat { get { return new ScannerFile("01_carrying_weapon\\0x04CFB5C1.wav", "Carrying a bat.", "carrying_weapon"); } }
        public static ScannerFile Armedwithaweapon { get { return new ScannerFile("01_carrying_weapon\\0x04FA024B.wav", "Armed with a weapon.", "carrying_weapon"); } }
        public static ScannerFile Armedwithasniperrifle { get { return new ScannerFile("01_carrying_weapon\\0x05F26EE0.wav", "Armed with a sniper rifle.", "carrying_weapon"); } }
        public static ScannerFile Armedwithagat { get { return new ScannerFile("01_carrying_weapon\\0x062DE1F3.wav", "Armed with a gat(?)", "carrying_weapon"); } }
        public static ScannerFile Armedwithasawedoffshotgun { get { return new ScannerFile("01_carrying_weapon\\0x07E4EAE1.wav", "Armed with a sawed-off shotgun.", "carrying_weapon"); } }
        public static ScannerFile Carryinganassaultshotgun { get { return new ScannerFile("01_carrying_weapon\\0x0872CE5F.wav", "Carrying an assault shotgun.", "carrying_weapon"); } }
        public static ScannerFile Carryinganassaultrifle { get { return new ScannerFile("01_carrying_weapon\\0x087FD323.wav", "Carrying an assault rifle.", "carrying_weapon"); } }
        public static ScannerFile Carryinganexplosive { get { return new ScannerFile("01_carrying_weapon\\0x0B56F594.wav", "Carrying an explosive.", "carrying_weapon"); } }
        public static ScannerFile Armedwithagrenadelauncher { get { return new ScannerFile("01_carrying_weapon\\0x0EDCED90.wav", "Armed with a grenade launcher.", "carrying_weapon"); } }
        public static ScannerFile Armedwithaknife { get { return new ScannerFile("01_carrying_weapon\\0x100F1331.wav", "Armed with a knife.", "carrying_weapon"); } }
        public static ScannerFile Armedwithafirearm { get { return new ScannerFile("01_carrying_weapon\\0x10B1C1E8.wav", "Armed with a firearm.", "carrying_weapon"); } }
        public static ScannerFile Carryingashotgun { get { return new ScannerFile("01_carrying_weapon\\0x10DCD959.wav", "Carrying a shotgun.", "carrying_weapon"); } }
        public static ScannerFile Armedwithanassaultshotgun { get { return new ScannerFile("01_carrying_weapon\\0x133223DD.wav", "Armed with an assault shotgun.", "carrying_weapon"); } }
        public static ScannerFile Carryingasawedoffshotgun { get { return new ScannerFile("01_carrying_weapon\\0x15968645.wav", "Carrying a sawed-off shotgun.", "carrying_weapon"); } }
        public static ScannerFile Armedwithabat { get { return new ScannerFile("01_carrying_weapon\\0x15EF5805.wav", "Armed with a bat.", "carrying_weapon"); } }
        public static ScannerFile Carryingafirearm { get { return new ScannerFile("01_carrying_weapon\\0x164D0D1F.wav", "Carrying a firearm.", "carrying_weapon"); } }
        public static ScannerFile Armedwithasubmachinegun { get { return new ScannerFile("01_carrying_weapon\\0x16BF89F9.wav", "Armed with a sub machine gun.", "carrying_weapon"); } }
        public static ScannerFile Armedwithamachinegun { get { return new ScannerFile("01_carrying_weapon\\0x16E501E6.wav", "Armed with a machine gun.", "carrying_weapon"); } }
        public static ScannerFile Carryingaweapon { get { return new ScannerFile("01_carrying_weapon\\0x174C66EE.wav", "Carrying a weapon.", "carrying_weapon"); } }
        public static ScannerFile Carryingagun { get { return new ScannerFile("01_carrying_weapon\\0x191092A7.wav", "Carrying a gun.", "carrying_weapon"); } }
        public static ScannerFile Armedwithanexplosive { get { return new ScannerFile("01_carrying_weapon\\0x1A8453EF.wav", "Armed with an explosive.", "carrying_weapon"); } }
        public static ScannerFile Armedwithaminigun { get { return new ScannerFile("01_carrying_weapon\\0x1B175AFC.wav", "Armed with a minigun.", "carrying_weapon"); } }
        public static ScannerFile Carryingagat { get { return new ScannerFile("01_carrying_weapon\\0x1B62CC5D.wav", "Carrying a gat(?)", "carrying_weapon"); } }
        public static ScannerFile Carryingasubmachinegun { get { return new ScannerFile("01_carrying_weapon\\0x1B685349.wav", "Carrying a sub machine gun.", "carrying_weapon"); } }
        public static ScannerFile Carryingamachinegun { get { return new ScannerFile("01_carrying_weapon\\0x1B950B8E.wav", "Carrying a machine gun.", "carrying_weapon"); } }
        public static ScannerFile Armedwithashotgun { get { return new ScannerFile("01_carrying_weapon\\0x1D8FB2BF.wav", "Armed with a shotgun.", "carrying_weapon"); } }
        public static ScannerFile CarryinganRPG { get { return new ScannerFile("01_carrying_weapon\\0x1F6F3AA8.wav", "Carrying an RPG.", "carrying_weapon"); } }
    }
    //public class car_code_beat { public static ScannerFile 20 { get {return new ScannerFile("01_car_code_beat\\0x04114DC3.wav","20","car_code_beat"); } }
    //public static ScannerFile 7 { get {return new ScannerFile("01_car_code_beat\\0x045F6810.wav","7","car_code_beat"); } }
    //public static ScannerFile 6 { get {return new ScannerFile("01_car_code_beat\\0x048DB95D.wav","6","car_code_beat"); } }
    //public static ScannerFile 13 { get {return new ScannerFile("01_car_code_beat\\0x056934B0.wav","13","car_code_beat"); } }
    //public static ScannerFile 4 { get {return new ScannerFile("01_car_code_beat\\0x05AFDDFC.wav","4","car_code_beat"); } }
    //public static ScannerFile 22 { get {return new ScannerFile("01_car_code_beat\\0x070401AA.wav","22","car_code_beat"); } }
    //public static ScannerFile 17 { get {return new ScannerFile("01_car_code_beat\\0x08632482.wav","17","car_code_beat"); } }
    //public static ScannerFile 5 { get {return new ScannerFile("01_car_code_beat\\0x0A17C498.wav","5","car_code_beat"); } }
    //public static ScannerFile 21 { get {return new ScannerFile("01_car_code_beat\\0x0AC0CD3B.wav","21","car_code_beat"); } }
    //public static ScannerFile 8 { get {return new ScannerFile("01_car_code_beat\\0x0D675D57.wav","8","car_code_beat"); } }
    //public static ScannerFile 11 { get {return new ScannerFile("01_car_code_beat\\0x0D88503E.wav","11","car_code_beat"); } }
    //public static ScannerFile 15 { get {return new ScannerFile("01_car_code_beat\\0x0DCE9405.wav","15","car_code_beat"); } }
    //public static ScannerFile 16 { get {return new ScannerFile("01_car_code_beat\\0x0E40B65E.wav","16","car_code_beat"); } }
    //public static ScannerFile 3 { get {return new ScannerFile("01_car_code_beat\\0x0E57C6EE.wav","3","car_code_beat"); } }
    //public static ScannerFile 2 { get {return new ScannerFile("01_car_code_beat\\0x1049F2F3.wav","2","car_code_beat"); } }
    //public static ScannerFile 19 { get {return new ScannerFile("01_car_code_beat\\0x12850991.wav","19","car_code_beat"); } }
    //public static ScannerFile 10 { get {return new ScannerFile("01_car_code_beat\\0x131E6122.wav","10","car_code_beat"); } }
    //public static ScannerFile 1 { get {return new ScannerFile("01_car_code_beat\\0x1361FCAE.wav","1","car_code_beat"); } }
    //public static ScannerFile 23 { get {return new ScannerFile("01_car_code_beat\\0x13F6EC22.wav","23","car_code_beat"); } }
    //public static ScannerFile 9 { get {return new ScannerFile("01_car_code_beat\\0x14488E9C.wav","9","car_code_beat"); } }
    //public static ScannerFile 14 { get {return new ScannerFile("01_car_code_beat\\0x15E37467.wav","14","car_code_beat"); } }
    //public static ScannerFile 18 { get {return new ScannerFile("01_car_code_beat\\0x1CC8A958.wav","18","car_code_beat"); } }
    //public static ScannerFile 12 { get {return new ScannerFile("01_car_code_beat\\0x1D7566AD.wav","12","car_code_beat"); } }
    //public static ScannerFile 24 { get {return new ScannerFile("01_car_code_beat\\0x1E112D7D.wav","24","car_code_beat"); } }}
    //public class car_code_composite { public static ScannerFile 7Edward7 { get {return new ScannerFile("01_car_code_composite\\0x04ECD8ED.wav","7-Edward-7","car_code_composite"); } }
    //public static ScannerFile 1Adam13 { get {return new ScannerFile("01_car_code_composite\\0x04F7F7DD.wav","1-Adam-13","car_code_composite"); } }
    //public static ScannerFile 4Mary5 { get {return new ScannerFile("01_car_code_composite\\0x0741C430.wav","4-Mary-5","car_code_composite"); } }
    //public static ScannerFile 1David4 { get {return new ScannerFile("01_car_code_composite\\0x0749580E.wav","1-David-4","car_code_composite"); } }
    //public static ScannerFile 2Lincoln8 { get {return new ScannerFile("01_car_code_composite\\0x07911FB2.wav","2-Lincoln-8","car_code_composite"); } }
    //public static ScannerFile 2Edward6 { get {return new ScannerFile("01_car_code_composite\\0x08094919.wav","2-Edward-6","car_code_composite"); } }
    //public static ScannerFile 3Lincoln12 { get {return new ScannerFile("01_car_code_composite\\0x0CFFC800.wav","3-Lincoln-12","car_code_composite"); } }
    //public static ScannerFile 9David1 { get {return new ScannerFile("01_car_code_composite\\0x0E2B53BA.wav","9-David-1","car_code_composite"); } }
    //public static ScannerFile 8Lincoln5 { get {return new ScannerFile("01_car_code_composite\\0x12354C79.wav","8-Lincoln-5","car_code_composite"); } }
    //public static ScannerFile 3Lincoln2 { get {return new ScannerFile("01_car_code_composite\\0x1240ABC0.wav","3-Lincoln-2","car_code_composite"); } }
    //public static ScannerFile 6David6 { get {return new ScannerFile("01_car_code_composite\\0x15CA5EF5.wav","6-David-6","car_code_composite"); } }
    //public static ScannerFile 1Adam5 { get {return new ScannerFile("01_car_code_composite\\0x17DC4DCB.wav","1-Adam-5","car_code_composite"); } }
    //public static ScannerFile 8Mary7 { get {return new ScannerFile("01_car_code_composite\\0x19BDE3DC.wav","8-Mary-7","car_code_composite"); } }
    //public static ScannerFile 2Edward5 { get {return new ScannerFile("01_car_code_composite\\0x1A9F3B2C.wav","2-Edward-5","car_code_composite"); } }
    //public static ScannerFile 7Edward14 { get {return new ScannerFile("01_car_code_composite\\0x1CD2F553.wav","7-Edward-14","car_code_composite"); } }
    //public static ScannerFile 9Lincoln15 { get {return new ScannerFile("01_car_code_composite\\0x1F00095C.wav","9-Lincoln-15","car_code_composite"); } }}
    //public class car_code_division { public static ScannerFile 1 { get {return new ScannerFile("01_car_code_division\\0x06DF1221.wav","1","car_code_division"); } }
    //public static ScannerFile 9 { get {return new ScannerFile("01_car_code_division\\0x071CEDC9.wav","9","car_code_division"); } }
    //public static ScannerFile 8 { get {return new ScannerFile("01_car_code_division\\0x0792F0F1.wav","8","car_code_division"); } }
    //public static ScannerFile 4 { get {return new ScannerFile("01_car_code_division\\0x0EA506D5.wav","4","car_code_division"); } }
    //public static ScannerFile 2 { get {return new ScannerFile("01_car_code_division\\0x10564158.wav","2","car_code_division"); } }
    //public static ScannerFile 5 { get {return new ScannerFile("01_car_code_division\\0x1202F364.wav","5","car_code_division"); } }
    //public static ScannerFile 7 { get {return new ScannerFile("01_car_code_division\\0x141CAEE1.wav","7","car_code_division"); } }
    //public static ScannerFile 6 { get {return new ScannerFile("01_car_code_division\\0x146737ED.wav","6","car_code_division"); } }
    //public static ScannerFile 10 { get {return new ScannerFile("01_car_code_division\\0x15777950.wav","10","car_code_division"); } }
    //public static ScannerFile 3 { get {return new ScannerFile("01_car_code_division\\0x1C908C63.wav","3","car_code_division"); } }}
    public class car_code_unit_type
    {
        public static ScannerFile Adam { get { return new ScannerFile("01_car_code_unit_type\\0x019DB368.wav", "Adam", "car_code_unit_type"); } }
        public static ScannerFile Charles { get { return new ScannerFile("01_car_code_unit_type\\0x01B6CB32.wav", "Charles", "car_code_unit_type"); } }
        public static ScannerFile Ida { get { return new ScannerFile("01_car_code_unit_type\\0x028EAC65.wav", "Ida", "car_code_unit_type"); } }
        public static ScannerFile Queen { get { return new ScannerFile("01_car_code_unit_type\\0x0312DE16.wav", "Queen", "car_code_unit_type"); } }
        public static ScannerFile Sam { get { return new ScannerFile("01_car_code_unit_type\\0x06A12117.wav", "Sam", "car_code_unit_type"); } }
        public static ScannerFile David { get { return new ScannerFile("01_car_code_unit_type\\0x06D04A5D.wav", "David", "car_code_unit_type"); } }
        public static ScannerFile Ocean { get { return new ScannerFile("01_car_code_unit_type\\0x07AD2CCF.wav", "Ocean", "car_code_unit_type"); } }
        public static ScannerFile Young { get { return new ScannerFile("01_car_code_unit_type\\0x095B6C31.wav", "Young", "car_code_unit_type"); } }
        public static ScannerFile Lincoln { get { return new ScannerFile("01_car_code_unit_type\\0x0978B52D.wav", "Lincoln", "car_code_unit_type"); } }
        public static ScannerFile Tom { get { return new ScannerFile("01_car_code_unit_type\\0x0D320902.wav", "Tom", "car_code_unit_type"); } }
        public static ScannerFile Zebra { get { return new ScannerFile("01_car_code_unit_type\\0x0DD6DFB0.wav", "Zebra", "car_code_unit_type"); } }
        public static ScannerFile Union { get { return new ScannerFile("01_car_code_unit_type\\0x0F9D1299.wav", "Union", "car_code_unit_type"); } }
        public static ScannerFile King { get { return new ScannerFile("01_car_code_unit_type\\0x0FA51727.wav", "King", "car_code_unit_type"); } }
        public static ScannerFile Mary { get { return new ScannerFile("01_car_code_unit_type\\0x106EEE1F.wav", "Mary", "car_code_unit_type"); } }
        public static ScannerFile Paul { get { return new ScannerFile("01_car_code_unit_type\\0x1148453E.wav", "Paul", "car_code_unit_type"); } }
        public static ScannerFile Edward { get { return new ScannerFile("01_car_code_unit_type\\0x1507C292.wav", "Edward", "car_code_unit_type"); } }
        public static ScannerFile George { get { return new ScannerFile("01_car_code_unit_type\\0x155E5D66.wav", "George", "car_code_unit_type"); } }
        public static ScannerFile Robert { get { return new ScannerFile("01_car_code_unit_type\\0x17FE0862.wav", "Robert", "car_code_unit_type"); } }
        public static ScannerFile Henry { get { return new ScannerFile("01_car_code_unit_type\\0x1859236F.wav", "Henry", "car_code_unit_type"); } }
        public static ScannerFile Boy { get { return new ScannerFile("01_car_code_unit_type\\0x192087BE.wav", "Boy", "car_code_unit_type"); } }
        public static ScannerFile John { get { return new ScannerFile("01_car_code_unit_type\\0x1C48EEC3.wav", "John", "car_code_unit_type"); } }
        public static ScannerFile XRay { get { return new ScannerFile("01_car_code_unit_type\\0x1C9CE941.wav", "X-Ray", "car_code_unit_type"); } }
        public static ScannerFile Nora { get { return new ScannerFile("01_car_code_unit_type\\0x1D0F2801.wav", "Nora", "car_code_unit_type"); } }
        public static ScannerFile William { get { return new ScannerFile("01_car_code_unit_type\\0x1ED2ABFD.wav", "William", "car_code_unit_type"); } }
        public static ScannerFile Frank { get { return new ScannerFile("01_car_code_unit_type\\0x1FA95EBC.wav", "Frank", "car_code_unit_type"); } }
        public static ScannerFile Victor { get { return new ScannerFile("01_car_code_unit_type\\0x1FE2D860.wav", "Victor", "car_code_unit_type"); } }
    }
    public class clothing_item_outfit
    {
        public static ScannerFile Afirefightersuniform { get { return new ScannerFile("01_clothing_item_outfit\\0x05FCA5E3.wav", "A firefighter's uniform", "clothing_item_outfit"); } }
        public static ScannerFile Awetsuit { get { return new ScannerFile("01_clothing_item_outfit\\0x07F171A5.wav", "A wet suit", "clothing_item_outfit"); } }
        public static ScannerFile Atracksuit { get { return new ScannerFile("01_clothing_item_outfit\\0x088DE65C.wav", "A track suit", "clothing_item_outfit"); } }
        public static ScannerFile Firefightingattire { get { return new ScannerFile("01_clothing_item_outfit\\0x13964116.wav", "Firefighting attire", "clothing_item_outfit"); } }
        public static ScannerFile Ajanitorsuniform { get { return new ScannerFile("01_clothing_item_outfit\\0x17D713A7.wav", "A janitor's uniform", "clothing_item_outfit"); } }
        public static ScannerFile Apoliceofficersuniform { get { return new ScannerFile("01_clothing_item_outfit\\0x1A275FE2.wav", "A police officer's uniform", "clothing_item_outfit"); } }
        public static ScannerFile Agasmask { get { return new ScannerFile("01_clothing_item_outfit\\0x1AE21C68.wav", "A gas mask", "clothing_item_outfit"); } }
        public static ScannerFile Adarkgraysuit { get { return new ScannerFile("01_clothing_item_outfit\\0x1C215456.wav", "A dark gray suit", "clothing_item_outfit"); } }
    }
    public class clothing_item_pants
    {
        public static ScannerFile Brookjeans { get { return new ScannerFile("01_clothing_item_pants\\0x0124C899.wav", "Brook(?) jeans", "clothing_item_pants"); } }
        public static ScannerFile Lightpants { get { return new ScannerFile("01_clothing_item_pants\\0x07DAA810.wav", "Light pants", "clothing_item_pants"); } }
        public static ScannerFile Darkpants { get { return new ScannerFile("01_clothing_item_pants\\0x09FF1A21.wav", "Dark pants", "clothing_item_pants"); } }
        public static ScannerFile Bluejeans { get { return new ScannerFile("01_clothing_item_pants\\0x0CEA5160.wav", "Blue jeans", "clothing_item_pants"); } }
        public static ScannerFile Lightshorts { get { return new ScannerFile("01_clothing_item_pants\\0x12A4D46F.wav", "Light shorts", "clothing_item_pants"); } }
        public static ScannerFile Blackjeans { get { return new ScannerFile("01_clothing_item_pants\\0x15D1CBC3.wav", "Black jeans", "clothing_item_pants"); } }
        public static ScannerFile Cargopants { get { return new ScannerFile("01_clothing_item_pants\\0x1761F58C.wav", "Cargo pants", "clothing_item_pants"); } }
        public static ScannerFile Darkshorts { get { return new ScannerFile("01_clothing_item_pants\\0x186A9052.wav", "Dark shorts", "clothing_item_pants"); } }
    }
    public class clothing_item_shoes
    {
        public static ScannerFile Lightsneakers { get { return new ScannerFile("01_clothing_item_shoes\\0x04DF1D1B.wav", "Light sneakers", "clothing_item_shoes"); } }
        public static ScannerFile Blackboots { get { return new ScannerFile("01_clothing_item_shoes\\0x0D8BF112.wav", "Black boots", "clothing_item_shoes"); } }
        public static ScannerFile Darksneakers { get { return new ScannerFile("01_clothing_item_shoes\\0x1222B07E.wav", "Dark sneakers", "clothing_item_shoes"); } }
        public static ScannerFile Brownboots { get { return new ScannerFile("01_clothing_item_shoes\\0x18770478.wav", "Brown boots", "clothing_item_shoes"); } }
        public static ScannerFile Brownshoes { get { return new ScannerFile("01_clothing_item_shoes\\0x1C00A135.wav", "Brown shoes", "clothing_item_shoes"); } }
        public static ScannerFile Blackshoes { get { return new ScannerFile("01_clothing_item_shoes\\0x1DF3345A.wav", "Black shoes", "clothing_item_shoes"); } }
    }
    public class clothing_item_torso
    {
        public static ScannerFile Shortsleeveshirt { get { return new ScannerFile("01_clothing_item_torso\\0x021EEEAF.wav", "Short sleeve shirt", "clothing_item_torso"); } }
        public static ScannerFile Suitjacket { get { return new ScannerFile("01_clothing_item_torso\\0x02BEDD9F.wav", "Suit jacket", "clothing_item_torso"); } }
        public static ScannerFile Leatherjacket { get { return new ScannerFile("01_clothing_item_torso\\0x03FB29CC.wav", "Leather jacket", "clothing_item_torso"); } }
        public static ScannerFile Darkjacket { get { return new ScannerFile("01_clothing_item_torso\\0x04E5C73E.wav", "Dark jacket", "clothing_item_torso"); } }
        public static ScannerFile Blueshirt { get { return new ScannerFile("01_clothing_item_torso\\0x06CA50C1.wav", "Blue shirt", "clothing_item_torso"); } }
        public static ScannerFile Darkshirt { get { return new ScannerFile("01_clothing_item_torso\\0x0724F7EF.wav", "Dark shirt", "clothing_item_torso"); } }
        public static ScannerFile Denimjacket { get { return new ScannerFile("01_clothing_item_torso\\0x0C7CEAEE.wav", "Denim jacket", "clothing_item_torso"); } }
        public static ScannerFile Noshirt { get { return new ScannerFile("01_clothing_item_torso\\0x0CE3051C.wav", "No shirt", "clothing_item_torso"); } }
        public static ScannerFile Greenshirt { get { return new ScannerFile("01_clothing_item_torso\\0x0D4A8107.wav", "Green shirt", "clothing_item_torso"); } }
        public static ScannerFile Longsleeveshirt { get { return new ScannerFile("01_clothing_item_torso\\0x0DF91657.wav", "Long sleeve shirt", "clothing_item_torso"); } }
        public static ScannerFile Redshirt { get { return new ScannerFile("01_clothing_item_torso\\0x107319A5.wav", "Red shirt", "clothing_item_torso"); } }
        public static ScannerFile Lightshirt { get { return new ScannerFile("01_clothing_item_torso\\0x1707806C.wav", "Light shirt", "clothing_item_torso"); } }
        public static ScannerFile Lightjacket { get { return new ScannerFile("01_clothing_item_torso\\0x1D00E5A1.wav", "Light jacket", "clothing_item_torso"); } }
    }
    public class colour
    {
        public static ScannerFile COLORAQUA01 { get { return new ScannerFile("01_colour\\COLOR_AQUA_01.wav", "COLOR_AQUA_01", "colour"); } }
        public static ScannerFile COLORBEIGE01 { get { return new ScannerFile("01_colour\\COLOR_BEIGE_01.wav", "COLOR_BEIGE_01", "colour"); } }
        public static ScannerFile COLORBLACK01 { get { return new ScannerFile("01_colour\\COLOR_BLACK_01.wav", "COLOR_BLACK_01", "colour"); } }
        public static ScannerFile COLORBLUE01 { get { return new ScannerFile("01_colour\\COLOR_BLUE_01.wav", "COLOR_BLUE_01", "colour"); } }
        public static ScannerFile COLORBRONZE01 { get { return new ScannerFile("01_colour\\COLOR_BRONZE_01.wav", "COLOR_BRONZE_01", "colour"); } }
        public static ScannerFile COLORBROWN01 { get { return new ScannerFile("01_colour\\COLOR_BROWN_01.wav", "COLOR_BROWN_01", "colour"); } }
        public static ScannerFile COLORDARKBLUE01 { get { return new ScannerFile("01_colour\\COLOR_DARK_BLUE_01.wav", "COLOR_DARK_BLUE_01", "colour"); } }
        public static ScannerFile COLORDARKBROWN01 { get { return new ScannerFile("01_colour\\COLOR_DARK_BROWN_01.wav", "COLOR_DARK_BROWN_01", "colour"); } }
        public static ScannerFile COLORDARKGREEN01 { get { return new ScannerFile("01_colour\\COLOR_DARK_GREEN_01.wav", "COLOR_DARK_GREEN_01", "colour"); } }
        public static ScannerFile COLORDARKGREY01 { get { return new ScannerFile("01_colour\\COLOR_DARK_GREY_01.wav", "COLOR_DARK_GREY_01", "colour"); } }
        public static ScannerFile COLORDARKMAROON01 { get { return new ScannerFile("01_colour\\COLOR_DARK_MAROON_01.wav", "COLOR_DARK_MAROON_01", "colour"); } }
        public static ScannerFile COLORDARKORANGE01 { get { return new ScannerFile("01_colour\\COLOR_DARK_ORANGE_01.wav", "COLOR_DARK_ORANGE_01", "colour"); } }
        public static ScannerFile COLORDARKPURPLE01 { get { return new ScannerFile("01_colour\\COLOR_DARK_PURPLE_01.wav", "COLOR_DARK_PURPLE_01", "colour"); } }
        public static ScannerFile COLORDARKRED01 { get { return new ScannerFile("01_colour\\COLOR_DARK_RED_01.wav", "COLOR_DARK_RED_01", "colour"); } }
        public static ScannerFile COLORDARKSILVER01 { get { return new ScannerFile("01_colour\\COLOR_DARK_SILVER_01.wav", "COLOR_DARK_SILVER_01", "colour"); } }
        public static ScannerFile COLORDARKYELLOW01 { get { return new ScannerFile("01_colour\\COLOR_DARK_YELLOW_01.wav", "COLOR_DARK_YELLOW_01", "colour"); } }
        public static ScannerFile COLORGOLD01 { get { return new ScannerFile("01_colour\\COLOR_GOLD_01.wav", "COLOR_GOLD_01", "colour"); } }
        public static ScannerFile COLORGRAPHITE01 { get { return new ScannerFile("01_colour\\COLOR_GRAPHITE_01.wav", "COLOR_GRAPHITE_01", "colour"); } }
        public static ScannerFile COLORGREEN01 { get { return new ScannerFile("01_colour\\COLOR_GREEN_01.wav", "COLOR_GREEN_01", "colour"); } }
        public static ScannerFile COLORGREY01 { get { return new ScannerFile("01_colour\\COLOR_GREY_01.wav", "COLOR_GREY_01", "colour"); } }
        public static ScannerFile COLORGREY02 { get { return new ScannerFile("01_colour\\COLOR_GREY_02.wav", "COLOR_GREY_02", "colour"); } }
        public static ScannerFile COLORLIGHTBLUE01 { get { return new ScannerFile("01_colour\\COLOR_LIGHT_BLUE_01.wav", "COLOR_LIGHT_BLUE_01", "colour"); } }
        public static ScannerFile COLORLIGHTBROWN01 { get { return new ScannerFile("01_colour\\COLOR_LIGHT_BROWN_01.wav", "COLOR_LIGHT_BROWN_01", "colour"); } }
        public static ScannerFile COLORLIGHTGREEN01 { get { return new ScannerFile("01_colour\\COLOR_LIGHT_GREEN_01.wav", "COLOR_LIGHT_GREEN_01", "colour"); } }
        public static ScannerFile COLORLIGHTGREY01 { get { return new ScannerFile("01_colour\\COLOR_LIGHT_GREY_01.wav", "COLOR_LIGHT_GREY_01", "colour"); } }
        public static ScannerFile COLORLIGHTMAROON01 { get { return new ScannerFile("01_colour\\COLOR_LIGHT_MAROON_01.wav", "COLOR_LIGHT_MAROON_01", "colour"); } }
        public static ScannerFile COLORLIGHTORANGE01 { get { return new ScannerFile("01_colour\\COLOR_LIGHT_ORANGE_01.wav", "COLOR_LIGHT_ORANGE_01", "colour"); } }
        public static ScannerFile COLORLIGHTPURPLE01 { get { return new ScannerFile("01_colour\\COLOR_LIGHT_PURPLE_01.wav", "COLOR_LIGHT_PURPLE_01", "colour"); } }
        public static ScannerFile COLORLIGHTRED01 { get { return new ScannerFile("01_colour\\COLOR_LIGHT_RED_01.wav", "COLOR_LIGHT_RED_01", "colour"); } }
        public static ScannerFile COLORLIGHTSILVER01 { get { return new ScannerFile("01_colour\\COLOR_LIGHT_SILVER_01.wav", "COLOR_LIGHT_SILVER_01", "colour"); } }
        public static ScannerFile COLORLIGHTYELLOW01 { get { return new ScannerFile("01_colour\\COLOR_LIGHT_YELLOW_01.wav", "COLOR_LIGHT_YELLOW_01", "colour"); } }
        public static ScannerFile COLORMAROON01 { get { return new ScannerFile("01_colour\\COLOR_MAROON_01.wav", "COLOR_MAROON_01", "colour"); } }
        public static ScannerFile COLORMETALLICBLACK01 { get { return new ScannerFile("01_colour\\COLOR_METALLIC_BLACK_01.wav", "COLOR_METALLIC_BLACK_01", "colour"); } }
        public static ScannerFile COLORMETALLICBLUE01 { get { return new ScannerFile("01_colour\\COLOR_METALLIC_BLUE_01.wav", "COLOR_METALLIC_BLUE_01", "colour"); } }
        public static ScannerFile COLORMETALLICBROWN01 { get { return new ScannerFile("01_colour\\COLOR_METALLIC_BROWN_01.wav", "COLOR_METALLIC_BROWN_01", "colour"); } }
        public static ScannerFile COLORMETALLICGREEN01 { get { return new ScannerFile("01_colour\\COLOR_METALLIC_GREEN_01.wav", "COLOR_METALLIC_GREEN_01", "colour"); } }
        public static ScannerFile COLORMETALLICGREY01 { get { return new ScannerFile("01_colour\\COLOR_METALLIC_GREY_01.wav", "COLOR_METALLIC_GREY_01", "colour"); } }
        public static ScannerFile COLORMETALLICPURPLE01 { get { return new ScannerFile("01_colour\\COLOR_METALLIC_PURPLE_01.wav", "COLOR_METALLIC_PURPLE_01", "colour"); } }
        public static ScannerFile COLORMETALLICRED01 { get { return new ScannerFile("01_colour\\COLOR_METALLIC_RED_01.wav", "COLOR_METALLIC_RED_01", "colour"); } }
        public static ScannerFile COLORMETALLICSILVER01 { get { return new ScannerFile("01_colour\\COLOR_METALLIC_SILVER_01.wav", "COLOR_METALLIC_SILVER_01", "colour"); } }
        public static ScannerFile COLORMETALLICYELLOW01 { get { return new ScannerFile("01_colour\\COLOR_METALLIC_YELLOW_01.wav", "COLOR_METALLIC_YELLOW_01", "colour"); } }
        public static ScannerFile COLORORANGE01 { get { return new ScannerFile("01_colour\\COLOR_ORANGE_01.wav", "COLOR_ORANGE_01", "colour"); } }
        public static ScannerFile COLORPINK01 { get { return new ScannerFile("01_colour\\COLOR_PINK_01.wav", "COLOR_PINK_01", "colour"); } }
        public static ScannerFile COLORPURPLE01 { get { return new ScannerFile("01_colour\\COLOR_PURPLE_01.wav", "COLOR_PURPLE_01", "colour"); } }
        public static ScannerFile COLORRED01 { get { return new ScannerFile("01_colour\\COLOR_RED_01.wav", "COLOR_RED_01", "colour"); } }
        public static ScannerFile COLORSILVER01 { get { return new ScannerFile("01_colour\\COLOR_SILVER_01.wav", "COLOR_SILVER_01", "colour"); } }
        public static ScannerFile COLORWHITE01 { get { return new ScannerFile("01_colour\\COLOR_WHITE_01.wav", "COLOR_WHITE_01", "colour"); } }
        public static ScannerFile COLORYELLOW01 { get { return new ScannerFile("01_colour\\COLOR_YELLOW_01.wav", "COLOR_YELLOW_01", "colour"); } }
    }
    public class conjunctives
    {
        public static ScannerFile Onuh { get { return new ScannerFile("01_conjunctives\\0x011A8195.wav", "On, uh...", "conjunctives"); } }
        public static ScannerFile In { get { return new ScannerFile("01_conjunctives\\0x03D81B43.wav", "In...", "conjunctives"); } }
        public static ScannerFile after { get { return new ScannerFile("01_conjunctives\\0x04E066E1.wav", "after(?)", "conjunctives"); } }
        public static ScannerFile In_1 { get { return new ScannerFile("01_conjunctives\\0x0504FCB6.wav", "In...", "conjunctives"); } }
        public static ScannerFile In_2 { get { return new ScannerFile("01_conjunctives\\0x05F99F84.wav", "In...", "conjunctives"); } }
        public static ScannerFile Over { get { return new ScannerFile("01_conjunctives\\0x0919B635.wav", "Over...", "conjunctives"); } }
        public static ScannerFile Closetoum { get { return new ScannerFile("01_conjunctives\\0x0AF7ACDC.wav", "Close to, um...", "conjunctives"); } }
        public static ScannerFile PossiblyAttheneedsconfirmation { get { return new ScannerFile("01_conjunctives\\0x0B53F3CF.wav", "(Possibly At the, needs confirmation)", "conjunctives"); } }
        public static ScannerFile Insideuhh { get { return new ScannerFile("01_conjunctives\\0x0EC92FF6.wav", "Inside, uhh...", "conjunctives"); } }
        public static ScannerFile Closetouhh { get { return new ScannerFile("01_conjunctives\\0x1037F75C.wav", "Close to, uhh...", "conjunctives"); } }
        public static ScannerFile Unknownrequiresfurtherobservation { get { return new ScannerFile("01_conjunctives\\0x11263F74.wav", "(Unknown, requires further observation)", "conjunctives"); } }
        public static ScannerFile Onauhh { get { return new ScannerFile("01_conjunctives\\0x12CF24FE.wav", "On a, uhh...", "conjunctives"); } }
        public static ScannerFile PossiblyAttheneedsconfirmation1 { get { return new ScannerFile("01_conjunctives\\0x12E8C2F2.wav", "(Possibly At the, needs confirmation)", "conjunctives"); } }
        public static ScannerFile Drivinga { get { return new ScannerFile("01_conjunctives\\0x14C128D7.wav", "Driving a...", "conjunctives"); } }
        public static ScannerFile PossiblyAttheneedsconfirmation2 { get { return new ScannerFile("01_conjunctives\\0x17220B65.wav", "(Possibly At the, needs confirmation)", "conjunctives"); } }
        public static ScannerFile After { get { return new ScannerFile("01_conjunctives\\0x17EF8D06.wav", "After(?)", "conjunctives"); } }
        public static ScannerFile Nearumm { get { return new ScannerFile("01_conjunctives\\0x18AA0841.wav", "Near, umm...", "conjunctives"); } }
        public static ScannerFile PossiblyAttheneedsconfirmation3 { get { return new ScannerFile("01_conjunctives\\0x19BE50A4.wav", "(Possibly At the, needs confirmation)", "conjunctives"); } }
        public static ScannerFile DrivingAUmmm { get { return new ScannerFile("01_conjunctives\\0x1AF7F545.wav", "Driving on... (combination of uhh and possibly a yawn)", "conjunctives"); } }
        public static ScannerFile Inside { get { return new ScannerFile("01_conjunctives\\0x1C248AAD.wav", "Inside...", "conjunctives"); } }
        public static ScannerFile PossiblyAttheneedsconfirmation4 { get { return new ScannerFile("01_conjunctives\\0x1D19175A.wav", "(Possibly At the, needs confirmation)", "conjunctives"); } }
        public static ScannerFile Over1 { get { return new ScannerFile("01_conjunctives\\0x1DC1DFC1.wav", "Over...", "conjunctives"); } }
        public static ScannerFile Insideuhh1 { get { return new ScannerFile("01_conjunctives\\0x1F065070.wav", "Inside, uhh...", "conjunctives"); } }
        public static ScannerFile AT01 { get { return new ScannerFile("01_conjunctives\\AT_01.wav", "AT_01", "conjunctives"); } }
        public static ScannerFile AT02 { get { return new ScannerFile("01_conjunctives\\AT_02.wav", "AT_02", "conjunctives"); } }
        public static ScannerFile A01 { get { return new ScannerFile("01_conjunctives\\A_01.wav", "A_01", "conjunctives"); } }
        public static ScannerFile A02 { get { return new ScannerFile("01_conjunctives\\A_02.wav", "A_02", "conjunctives"); } }
        public static ScannerFile FAN2BJAD01 { get { return new ScannerFile("01_conjunctives\\FAN2_BJAD_01.wav", "FAN2_BJAD_01", "conjunctives"); } }
        public static ScannerFile Inuhh { get { return new ScannerFile("01_conjunctives\\IN_01.wav", "In, uhh...", "conjunctives"); } }
        public static ScannerFile Inuhh2 { get { return new ScannerFile("01_conjunctives\\IN_02.wav", "In...", "conjunctives"); } }
        public static ScannerFile Inuhh3 { get { return new ScannerFile("01_conjunctives\\IN_03.wav", "IN_03", "conjunctives"); } }
        public static ScannerFile INA01 { get { return new ScannerFile("01_conjunctives\\IN_A_01.wav", "IN_A_01", "conjunctives"); } }
        public static ScannerFile INA02 { get { return new ScannerFile("01_conjunctives\\IN_A_02.wav", "IN_A_02", "conjunctives"); } }
        public static ScannerFile INA03 { get { return new ScannerFile("01_conjunctives\\IN_A_03.wav", "IN_A_03", "conjunctives"); } }
        public static ScannerFile Onumm { get { return new ScannerFile("01_conjunctives\\ON_01.wav", "On, umm...", "conjunctives"); } }
        public static ScannerFile Onuhh { get { return new ScannerFile("01_conjunctives\\ON_02.wav", "On, uhh...", "conjunctives"); } }
        public static ScannerFile On { get { return new ScannerFile("01_conjunctives\\ON_03.wav", "On...", "conjunctives"); } }
        public static ScannerFile On1 { get { return new ScannerFile("01_conjunctives\\ON_04.wav", "On...", "conjunctives"); } }
        public static ScannerFile On2 { get { return new ScannerFile("01_conjunctives\\ON_05.wav", "On...", "conjunctives"); } }
        public static ScannerFile On3 { get { return new ScannerFile("01_conjunctives\\ON_06.wav", "On", "conjunctives"); } }
        public static ScannerFile On4 { get { return new ScannerFile("01_conjunctives\\ON_07.wav", "On", "conjunctives"); } }
        public static ScannerFile OVER01 { get { return new ScannerFile("01_conjunctives\\OVER_01.wav", "OVER_01", "conjunctives"); } }
    }
    public class crime_10_24
    {
        public static ScannerFile Astationemergency { get { return new ScannerFile("01_crime_10_24\\0x026A3806.wav", "A station emergency.", "crime_10_24"); } }
        public static ScannerFile A1024 { get { return new ScannerFile("01_crime_10_24\\0x10A0D480.wav", "A 10-24.", "crime_10_24"); } }
        public static ScannerFile Astationemergency1 { get { return new ScannerFile("01_crime_10_24\\0x14A19C80.wav", "A station emergency.", "crime_10_24"); } }
        public static ScannerFile A10241 { get { return new ScannerFile("01_crime_10_24\\0x15BC9EAB.wav", "A 10-24.", "crime_10_24"); } }
    }
    public class crime_10_351
    {
        public static ScannerFile Possessionofdrugsforsale { get { return new ScannerFile("01_crime_10_351\\0x0E4C71EB.wav", "Possession of drugs for sale.", "crime_10_351"); } }
        public static ScannerFile Apossessionofdrugsforsaleincident { get { return new ScannerFile("01_crime_10_351\\0x17A244B2.wav", "A possession of drugs for sale incident.", "crime_10_351"); } }
        public static ScannerFile Possessionofdrugsforsale1 { get { return new ScannerFile("01_crime_10_351\\0x1E95927E.wav", "Possession of drugs for sale.", "crime_10_351"); } }
    }
    public class crime_10_851
    {
        public static ScannerFile A10851 { get { return new ScannerFile("01_crime_10_851\\0x013CC71E.wav", "A 10-851.", "crime_10_851"); } }
        public static ScannerFile A108511 { get { return new ScannerFile("01_crime_10_851\\0x05258EF1.wav", "A 10-851.", "crime_10_851"); } }
        public static ScannerFile Astolenvehicle { get { return new ScannerFile("01_crime_10_851\\0x0E03A0AD.wav", "A stolen vehicle.", "crime_10_851"); } }
        public static ScannerFile Astolenvehicle1 { get { return new ScannerFile("01_crime_10_851\\0x12D86A56.wav", "A stolen vehicle.", "crime_10_851"); } }
    }
    public class crime_10_99
    {
        public static ScannerFile Anofficerrequiresimmediateassistance { get { return new ScannerFile("01_crime_10_99\\0x029C50BE.wav", "An officer requires immediate assistance.", "crime_10_99"); } }
        public static ScannerFile Acode99 { get { return new ScannerFile("01_crime_10_99\\0x02A890D4.wav", "A code 99.", "crime_10_99"); } }
        public static ScannerFile A1099 { get { return new ScannerFile("01_crime_10_99\\0x108A6C97.wav", "A 10-99.", "crime_10_99"); } }
        public static ScannerFile Anofficerrequiresimmediatehelp { get { return new ScannerFile("01_crime_10_99\\0x149B34B9.wav", "An officer requires immediate help.", "crime_10_99"); } }
    }
    public class crime_11_351
    {
        public static ScannerFile An11351 { get { return new ScannerFile("01_crime_11_351\\0x0654770E.wav", "An 11-351.", "crime_11_351"); } }
        public static ScannerFile An113511 { get { return new ScannerFile("01_crime_11_351\\0x0B6E4141.wav", "An 11-351.", "crime_11_351"); } }
        public static ScannerFile Apossessionofdrugsforsaleincident { get { return new ScannerFile("01_crime_11_351\\0x10140A8E.wav", "A possession of drugs for sale incident.", "crime_11_351"); } }
    }
    public class crime_11_357
    {
        public static ScannerFile Adrugpossessionincident { get { return new ScannerFile("01_crime_11_357\\0x0403DE60.wav", "A drug possession incident.", "crime_11_357"); } }
        public static ScannerFile An11357 { get { return new ScannerFile("01_crime_11_357\\0x086B6730.wav", "An 11-357.", "crime_11_357"); } }
        public static ScannerFile An113571 { get { return new ScannerFile("01_crime_11_357\\0x0C0EAE77.wav", "An 11-357.", "crime_11_357"); } }
        public static ScannerFile An113571_1 { get { return new ScannerFile("01_crime_11_357\\0x1502C05F.wav", "An 11-357.", "crime_11_357"); } }
        public static ScannerFile Adrugpossessionincident1 { get { return new ScannerFile("01_crime_11_357\\0x1A250AA4.wav", "A drug possession incident.", "crime_11_357"); } }
    }
    public class crime_1_48_resist_arrest
    {
        public static ScannerFile Acriminalresistingarrest { get { return new ScannerFile("01_crime_1_48_resist_arrest\\0x0419C880.wav", "A criminal resisting arrest.", "crime_1_48_resist_arrest"); } }
        public static ScannerFile A148 { get { return new ScannerFile("01_crime_1_48_resist_arrest\\0x08149075.wav", "A 148.", "crime_1_48_resist_arrest"); } }
        public static ScannerFile Asuspectontherun { get { return new ScannerFile("01_crime_1_48_resist_arrest\\0x0EA15D8E.wav", "A suspect on the run.", "crime_1_48_resist_arrest"); } }
        public static ScannerFile Apossible148 { get { return new ScannerFile("01_crime_1_48_resist_arrest\\0x131E2687.wav", "A...possible 148.", "crime_1_48_resist_arrest"); } }
        public static ScannerFile Apossible1481 { get { return new ScannerFile("01_crime_1_48_resist_arrest\\0x16142C75.wav", "A possible 148.", "crime_1_48_resist_arrest"); } }
        public static ScannerFile Acriminalresistingarrest1 { get { return new ScannerFile("01_crime_1_48_resist_arrest\\0x180F306A.wav", "A criminal resisting arrest.", "crime_1_48_resist_arrest"); } }
        public static ScannerFile Asuspectfleeingacrimescene { get { return new ScannerFile("01_crime_1_48_resist_arrest\\0x1A29F4A0.wav", "A suspect fleeing a crime scene.", "crime_1_48_resist_arrest"); } }
        public static ScannerFile A1481 { get { return new ScannerFile("01_crime_1_48_resist_arrest\\0x1B41BE61.wav", "A...148", "crime_1_48_resist_arrest"); } }
    }
    public class crime_1_87
    {
        public static ScannerFile A187 { get { return new ScannerFile("01_crime_1_87\\0x0CB0FAB6.wav", "A 187.", "crime_1_87"); } }
        public static ScannerFile Ahomicide { get { return new ScannerFile("01_crime_1_87\\0x133307C2.wav", "A homicide.", "crime_1_87"); } }
    }
    public class crime_2_07
    {
        public static ScannerFile Akidnapping { get { return new ScannerFile("01_crime_2_07\\0x0AA06B74.wav", "A kidnapping.", "crime_2_07"); } }
        public static ScannerFile A207 { get { return new ScannerFile("01_crime_2_07\\0x14D87FE1.wav", "A 207.", "crime_2_07"); } }
        public static ScannerFile A2071 { get { return new ScannerFile("01_crime_2_07\\0x184786C0.wav", "A 207.", "crime_2_07"); } }
        public static ScannerFile Akidnapping1 { get { return new ScannerFile("01_crime_2_07\\0x1C284E82.wav", "A kidnapping.", "crime_2_07"); } }
    }
    public class crime_2_11
    {
        public static ScannerFile Anarmedrobbery { get { return new ScannerFile("01_crime_2_11\\0x07B6402C.wav", "An armed robbery.", "crime_2_11"); } }
        public static ScannerFile Apossible211 { get { return new ScannerFile("01_crime_2_11\\0x19F9E4B3.wav", "A possible 211.", "crime_2_11"); } }
        public static ScannerFile A211 { get { return new ScannerFile("01_crime_2_11\\0x1E842DC8.wav", "A 211.", "crime_2_11"); } }
    }
    public class crime_2_13
    {
        public static ScannerFile A213 { get { return new ScannerFile("01_crime_2_13\\0x1490EDEF.wav", "A 213.", "crime_2_13"); } }
        public static ScannerFile Useofexplosives { get { return new ScannerFile("01_crime_2_13\\0x190F36ED.wav", "Use of explosives.", "crime_2_13"); } }
    }
    public class crime_2_17
    {
        public static ScannerFile Anattemptedmurder { get { return new ScannerFile("01_crime_2_17\\0x0AFA345E.wav", "An attempted murder.", "crime_2_17"); } }
        public static ScannerFile A217 { get { return new ScannerFile("01_crime_2_17\\0x1A5C9323.wav", "A 217.", "crime_2_17"); } }
    }
    public class crime_2_40
    {
        public static ScannerFile A240 { get { return new ScannerFile("01_crime_2_40\\0x0458F5C8.wav", "A 240.", "crime_2_40"); } }
        public static ScannerFile Anassault { get { return new ScannerFile("01_crime_2_40\\0x15AA986C.wav", "An assault.", "crime_2_40"); } }
    }
    public class crime_2_42
    {
        public static ScannerFile A242 { get { return new ScannerFile("01_crime_2_42\\0x039009FB.wav", "A 242.", "crime_2_42"); } }
        public static ScannerFile Abatteryincident { get { return new ScannerFile("01_crime_2_42\\0x13C5AA66.wav", "A battery incident.", "crime_2_42"); } }
    }
    public class crime_2_45
    {
        public static ScannerFile Anassaultwithadeadlyweapon { get { return new ScannerFile("01_crime_2_45\\0x032B7620.wav", "An assault with a deadly weapon.", "crime_2_45"); } }
        public static ScannerFile Apossible245 { get { return new ScannerFile("01_crime_2_45\\0x0C3E8856.wav", "A possible 245.", "crime_2_45"); } }
        public static ScannerFile A245 { get { return new ScannerFile("01_crime_2_45\\0x1E1A2C0D.wav", "A 245.", "crime_2_45"); } }
    }
    public class crime_2_46
    {
        public static ScannerFile Apossible246 { get { return new ScannerFile("01_crime_2_46\\0x04FA0824.wav", "A possible 246.", "crime_2_46"); } }
        public static ScannerFile A246 { get { return new ScannerFile("01_crime_2_46\\0x1624AA79.wav", "A 246.", "crime_2_46"); } }
        public static ScannerFile Ashootinginadwelling { get { return new ScannerFile("01_crime_2_46\\0x17176C4F.wav", "A shooting in a dwelling.", "crime_2_46"); } }
    }
    public class crime_2_88
    {
        public static ScannerFile Apossible288 { get { return new ScannerFile("01_crime_2_88\\0x001ABF70.wav", "A possible 288.", "crime_2_88"); } }
        public static ScannerFile A288 { get { return new ScannerFile("01_crime_2_88\\0x0E4E5BD7.wav", "A 288.", "crime_2_88"); } }
    }
    public class crime_3_11
    {
        public static ScannerFile A311 { get { return new ScannerFile("01_crime_3_11\\0x0232E557.wav", "A 311.", "crime_3_11"); } }
        public static ScannerFile Apossible311 { get { return new ScannerFile("01_crime_3_11\\0x15AE8C53.wav", "A possible 311.", "crime_3_11"); } }
        public static ScannerFile Anindecentexposure { get { return new ScannerFile("01_crime_3_11\\0x196E53D3.wav", "An indecent exposure.", "crime_3_11"); } }
    }
    public class crime_3_90
    {
        public static ScannerFile Apossible390 { get { return new ScannerFile("01_crime_3_90\\0x08698BA8.wav", "A possible 390.", "crime_3_90"); } }
        public static ScannerFile Publicintoxication { get { return new ScannerFile("01_crime_3_90\\0x1632E73A.wav", "Public intoxication.", "crime_3_90"); } }
        public static ScannerFile A390 { get { return new ScannerFile("01_crime_3_90\\0x19882DE5.wav", "A 390.", "crime_3_90"); } }
    }
    public class crime_4_06
    {
        public static ScannerFile Apossible406 { get { return new ScannerFile("01_crime_4_06\\0x061DEFD6.wav", "A possible 406.", "crime_4_06"); } }
        public static ScannerFile Breakingandentering { get { return new ScannerFile("01_crime_4_06\\0x0FDFC35A.wav", "Breaking and entering.", "crime_4_06"); } }
        public static ScannerFile A406 { get { return new ScannerFile("01_crime_4_06\\0x13790A8D.wav", "A 406.", "crime_4_06"); } }
        public static ScannerFile ABnE { get { return new ScannerFile("01_crime_4_06\\0x1E81A09D.wav", "A BnE.", "crime_4_06"); } }
    }
    public class crime_4_15 { public static ScannerFile A415 { get { return new ScannerFile("01_crime_4_15\\0x06E7893A.wav", "A 415.", "crime_4_15"); } } }
    public class crime_4_15_george
    {
        public static ScannerFile Apossiblegangrelateddisturbance { get { return new ScannerFile("01_crime_4_15_george\\0x0A3A3012.wav", "A possible gang-related disturbance.", "crime_4_15_george"); } }
        public static ScannerFile Agangrelateddisturbance { get { return new ScannerFile("01_crime_4_15_george\\0x0B23B1DB.wav", "A gang-related disturbance.", "crime_4_15_george"); } }
        public static ScannerFile A415George { get { return new ScannerFile("01_crime_4_15_george\\0x1D721678.wav", "A 415 George.", "crime_4_15_george"); } }
    }
    public class crime_4_17
    {
        public static ScannerFile Apersonwithafirearm { get { return new ScannerFile("01_crime_4_17\\0x0A578619.wav", "A person with a firearm.", "crime_4_17"); } }
        public static ScannerFile A417 { get { return new ScannerFile("01_crime_4_17\\0x15D4DD10.wav", "A 417.", "crime_4_17"); } }
        public static ScannerFile Apersonwithagun { get { return new ScannerFile("01_crime_4_17\\0x1F9EB0A4.wav", "A person with a gun.", "crime_4_17"); } }
    }
    public class crime_4_17_king
    {
        public static ScannerFile Apersonwithaknife { get { return new ScannerFile("01_crime_4_17_king\\0x05BD27F9.wav", "A person with a knife.", "crime_4_17_king"); } }
        public static ScannerFile A417King { get { return new ScannerFile("01_crime_4_17_king\\0x11B4BFE4.wav", "A 417 King.", "crime_4_17_king"); } }
        public static ScannerFile Apossible417King { get { return new ScannerFile("01_crime_4_17_king\\0x12F6426C.wav", "A possible 417 King.", "crime_4_17_king"); } }
        public static ScannerFile A417King1 { get { return new ScannerFile("01_crime_4_17_king\\0x1AFE9278.wav", "A 417 King.", "crime_4_17_king"); } }
    }
    public class crime_4_19
    {
        public static ScannerFile Apossible419 { get { return new ScannerFile("01_crime_4_19\\0x0658CBC9.wav", "A possible 419.", "crime_4_19"); } }
        public static ScannerFile A419 { get { return new ScannerFile("01_crime_4_19\\0x1103211D.wav", "A 419.", "crime_4_19"); } }
        public static ScannerFile Adeadbody { get { return new ScannerFile("01_crime_4_19\\0x148F6836.wav", "A dead body.", "crime_4_19"); } }
        public static ScannerFile Adeceasedperson { get { return new ScannerFile("01_crime_4_19\\0x19D032B4.wav", "A deceased person.", "crime_4_19"); } }
    }
    public class crime_4_59
    {
        public static ScannerFile ABnE { get { return new ScannerFile("01_crime_4_59\\0x159DC5DC.wav", "A BnE.", "crime_4_59"); } }
        public static ScannerFile Apossible459 { get { return new ScannerFile("01_crime_4_59\\0x18214A95.wav", "A possible 459.", "crime_4_59"); } }
    }
    public class crime_4_80
    {
        public static ScannerFile A480 { get { return new ScannerFile("01_crime_4_80\\0x05D8072E.wav", "A 480.", "crime_4_80"); } }
        public static ScannerFile Afelonyhitandrun { get { return new ScannerFile("01_crime_4_80\\0x09A9CED1.wav", "A felony hit and run.", "crime_4_80"); } }
        public static ScannerFile Ahitandrun { get { return new ScannerFile("01_crime_4_80\\0x0C46540B.wav", "A hit and run.", "crime_4_80"); } }
        public static ScannerFile Apossible480 { get { return new ScannerFile("01_crime_4_80\\0x1719A9B2.wav", "A possible 480.", "crime_4_80"); } }
    }
    public class crime_4_81
    {
        public static ScannerFile Ahitandrun { get { return new ScannerFile("01_crime_4_81\\0x08B8E1B0.wav", "A hit and run.", "crime_4_81"); } }
        public static ScannerFile A481 { get { return new ScannerFile("01_crime_4_81\\0x1A9E8577.wav", "A 481.", "crime_4_81"); } }
    }
    public class crime_4_84
    {
        public static ScannerFile Apettytheft { get { return new ScannerFile("01_crime_4_84\\0x089B4C9F.wav", "A petty theft.", "crime_4_84"); } }
        public static ScannerFile Apossible484 { get { return new ScannerFile("01_crime_4_84\\0x125A601D.wav", "A possible 484.", "crime_4_84"); } }
        public static ScannerFile A484 { get { return new ScannerFile("01_crime_4_84\\0x1C0E7386.wav", "A 484.", "crime_4_84"); } }
    }
    public class crime_4_84_paul_sam
    {
        public static ScannerFile Apursetheft { get { return new ScannerFile("01_crime_4_84_paul_sam\\0x05649D12.wav", "A purse theft.", "crime_4_84_paul_sam"); } }
        public static ScannerFile A484PaulSam { get { return new ScannerFile("01_crime_4_84_paul_sam\\0x07E5A217.wav", "A 484 Paul-Sam.", "crime_4_84_paul_sam"); } }
        public static ScannerFile Apossible484PaulSam { get { return new ScannerFile("01_crime_4_84_paul_sam\\0x13CE79E5.wav", "A possible 484 Paul-Sam.", "crime_4_84_paul_sam"); } }
    }
    public class crime_4_87
    {
        public static ScannerFile Apossible487 { get { return new ScannerFile("01_crime_4_87\\0x047A29C5.wav", "A possible 487.", "crime_4_87"); } }
        public static ScannerFile Agrandtheft { get { return new ScannerFile("01_crime_4_87\\0x11C84461.wav", "A grand theft.", "crime_4_87"); } }
        public static ScannerFile A487 { get { return new ScannerFile("01_crime_4_87\\0x14FA8AC6.wav", "A 487,", "crime_4_87"); } }
    }
    public class crime_4_88
    {
        public static ScannerFile Apossible488 { get { return new ScannerFile("01_crime_4_88\\0x0A83BA37.wav", "A possible 488.", "crime_4_88"); } }
        public static ScannerFile A488 { get { return new ScannerFile("01_crime_4_88\\0x1D09DF42.wav", "A 488.", "crime_4_88"); } }
    }
    public class crime_5_02
    {
        public static ScannerFile ADUI { get { return new ScannerFile("01_crime_5_02\\0x02098AFD.wav", "A DUI.", "crime_5_02"); } }
        public static ScannerFile A502DUI { get { return new ScannerFile("01_crime_5_02\\0x021C8B2F.wav", "A 502 DUI.", "crime_5_02"); } }
        public static ScannerFile A502 { get { return new ScannerFile("01_crime_5_02\\0x08BE9874.wav", "A 502.", "crime_5_02"); } }
        public static ScannerFile Adriverundertheinfluence { get { return new ScannerFile("01_crime_5_02\\0x139E2E26.wav", "A driver under the influence.", "crime_5_02"); } }
        public static ScannerFile Apossible502 { get { return new ScannerFile("01_crime_5_02\\0x142D2F50.wav", "A possible 502.", "crime_5_02"); } }
        public static ScannerFile Adriverundertheinfluence1 { get { return new ScannerFile("01_crime_5_02\\0x1E54C392.wav", "A driver under the influence.", "crime_5_02"); } }
    }
    public class crime_5_03
    {
        public static ScannerFile A503 { get { return new ScannerFile("01_crime_5_03\\0x1ADB2E53.wav", "A 503.", "crime_5_03"); } }
        public static ScannerFile Apossible503 { get { return new ScannerFile("01_crime_5_03\\0x1E52F541.wav", "A possible 503.", "crime_5_03"); } }
    }
    public class crime_5_04
    {
        public static ScannerFile Tamperingwithavehicle { get { return new ScannerFile("01_crime_5_04\\0x08C01409.wav", "Tampering with a vehicle.", "crime_5_04"); } }
        public static ScannerFile A504 { get { return new ScannerFile("01_crime_5_04\\0x1868B35A.wav", "A 504.", "crime_5_04"); } }
    }
    public class crime_5_05
    {
        public static ScannerFile A505 { get { return new ScannerFile("01_crime_5_05\\0x037BFDBB.wav", "A 505.", "crime_5_05"); } }
        public static ScannerFile Adriveroutofcontrol { get { return new ScannerFile("01_crime_5_05\\0x05E10283.wav", "A driver out of control.", "crime_5_05"); } }
    }
    public class crime_5_07
    {
        public static ScannerFile A507 { get { return new ScannerFile("01_crime_5_07\\0x021ACC57.wav", "A 507.", "crime_5_07"); } }
        public static ScannerFile Apublicnuisance { get { return new ScannerFile("01_crime_5_07\\0x0CE561EB.wav", "A public nuisance.", "crime_5_07"); } }
    }
    public class crime_5_10
    {
        public static ScannerFile A510 { get { return new ScannerFile("01_crime_5_10\\0x00FCB2AB.wav", "A 510.", "crime_5_10"); } }
        public static ScannerFile Vehiclesracing { get { return new ScannerFile("01_crime_5_10\\0x12CE564E.wav", "Vehicles racing.", "crime_5_10"); } }
        public static ScannerFile Speedingvehicle { get { return new ScannerFile("01_crime_5_10\\0x17561F60.wav", "Speeding vehicles.", "crime_5_10"); } }
    }
    public class crime_5_86
    {
        public static ScannerFile A586 { get { return new ScannerFile("01_crime_5_86\\0x092E5D13.wav", "A 586.", "crime_5_86"); } }
        public static ScannerFile Anillegallyparkedvehicle { get { return new ScannerFile("01_crime_5_86\\0x0D4EA555.wav", "An illegally parked vehicle.", "crime_5_86"); } }
        public static ScannerFile Illegalparking { get { return new ScannerFile("01_crime_5_86\\0x18DA7C6C.wav", "Illegal parking.", "crime_5_86"); } }
        public static ScannerFile Aparkingviolation { get { return new ScannerFile("01_crime_5_86\\0x1E1106DA.wav", "A parking violation.", "crime_5_86"); } }
    }
    public class crime_5_94
    {
        public static ScannerFile Maliciousmischief { get { return new ScannerFile("01_crime_5_94\\0x08494618.wav", "Malicious mischief.", "crime_5_94"); } }
        public static ScannerFile Maliciousmischief1 { get { return new ScannerFile("01_crime_5_94\\0x0EA052C1.wav", "Malicious mischief.", "crime_5_94"); } }
        public static ScannerFile A594 { get { return new ScannerFile("01_crime_5_94\\0x16BD22FD.wav", "A 594.", "crime_5_94"); } }
    }
    public class crime_6_53_mary
    {
        public static ScannerFile Athreateningphonecall { get { return new ScannerFile("01_crime_6_53_mary\\0x04A45011.wav", "A threatening phone call.", "crime_6_53_mary"); } }
        public static ScannerFile A653Mary { get { return new ScannerFile("01_crime_6_53_mary\\0x14332F31.wav", "A 653-Mary.", "crime_6_53_mary"); } }
    }
    public class crime_9_14a_attempted_suicide
    {
        public static ScannerFile Apossibleattemptedsuicide { get { return new ScannerFile("01_crime_9_14a_attempted_suicide\\0x037FB5C7.wav", "A possible attempted suicide.", "crime_9_14a_attempted_suicide"); } }
        public static ScannerFile Anattemptedsuicide { get { return new ScannerFile("01_crime_9_14a_attempted_suicide\\0x0765BD91.wav", "An attempted suicide.", "crime_9_14a_attempted_suicide"); } }
        public static ScannerFile Apossible914A { get { return new ScannerFile("01_crime_9_14a_attempted_suicide\\0x08A0C007.wav", "A possible 914A.", "crime_9_14a_attempted_suicide"); } }
        public static ScannerFile A914A { get { return new ScannerFile("01_crime_9_14a_attempted_suicide\\0x1141114A.wav", "A 914A.", "crime_9_14a_attempted_suicide"); } }
        public static ScannerFile A914Adam { get { return new ScannerFile("01_crime_9_14a_attempted_suicide\\0x1AE16488.wav", "A 914-Adam.", "crime_9_14a_attempted_suicide"); } }
    }
    public class crime_9_25
    {
        public static ScannerFile Asuspiciousperson { get { return new ScannerFile("01_crime_9_25\\0x11BA9943.wav", "A suspicious person.", "crime_9_25"); } }
        public static ScannerFile A925 { get { return new ScannerFile("01_crime_9_25\\0x1F38B43F.wav", "A 925.", "crime_9_25"); } }
    }
    public class crime_9_96
    {
        public static ScannerFile A996 { get { return new ScannerFile("01_crime_9_96\\0x0BD091D7.wav", "A 996.", "crime_9_96"); } }
        public static ScannerFile Acombustiblematerialincident { get { return new ScannerFile("01_crime_9_96\\0x11861D3A.wav", "A combustible material incident.", "crime_9_96"); } }
    }
    public class crime_9_96_boy
    {
        public static ScannerFile A996Boy { get { return new ScannerFile("01_crime_9_96_boy\\0x05AC6C07.wav", "A 996-Boy.", "crime_9_96_boy"); } }
        public static ScannerFile Anexplosion { get { return new ScannerFile("01_crime_9_96_boy\\0x1BF8589E.wav", "An explosion.", "crime_9_96_boy"); } }
    }
    public class crime_accident { public static ScannerFile Anaccident { get { return new ScannerFile("01_crime_accident\\0x0AC0DB4B.wav", "An accident.", "crime_accident"); } } }
    public class crime_airplane_crash
    {
        public static ScannerFile Anaircraftcrash { get { return new ScannerFile("01_crime_airplane_crash\\0x04A51B94.wav", "An aircraft crash.", "crime_airplane_crash"); } }
        public static ScannerFile AnAC { get { return new ScannerFile("01_crime_airplane_crash\\0x183EC2CD.wav", "An AC.", "crime_airplane_crash"); } }
        public static ScannerFile Anairplanecrash { get { return new ScannerFile("01_crime_airplane_crash\\0x190D4464.wav", "An airplane crash.", "crime_airplane_crash"); } }
    }
    public class crime_air_squad_down { public static ScannerFile Anairsquaddown { get { return new ScannerFile("01_crime_air_squad_down\\0x08895194.wav", "An air squad down.", "crime_air_squad_down"); } } }
    public class crime_air_unit_down { public static ScannerFile Anairunitdown { get { return new ScannerFile("01_crime_air_unit_down\\0x098D02C9.wav", "An air unit down.", "crime_air_unit_down"); } } }
    public class crime_ambulance_requested
    {
        public static ScannerFile Anambulancecall { get { return new ScannerFile("01_crime_ambulance_requested\\0x01B705D3.wav", "An ambulance call.", "crime_ambulance_requested"); } }
        public static ScannerFile Aninjuryincident { get { return new ScannerFile("01_crime_ambulance_requested\\0x0B58D917.wav", "An injury incident.", "crime_ambulance_requested"); } }
        public static ScannerFile Anambulancerequestedinjuriesunknown { get { return new ScannerFile("01_crime_ambulance_requested\\0x1208E676.wav", "An ambulance requested; injuries unknown.", "crime_ambulance_requested"); } }
    }
    public class crime_animal_cruelty
    {
        public static ScannerFile Reportofanimalcruelty { get { return new ScannerFile("01_crime_animal_cruelty\\0x0383DAFA.wav", "Report of animal cruelty.", "crime_animal_cruelty"); } }
        public static ScannerFile Reportsofanimalcruelty { get { return new ScannerFile("01_crime_animal_cruelty\\0x12BA7967.wav", "Reports of animal cruelty.", "crime_animal_cruelty"); } }
        public static ScannerFile Reportsofanimalcruelty1 { get { return new ScannerFile("01_crime_animal_cruelty\\0x16DD41AE.wav", "Reports of animal cruelty.", "crime_animal_cruelty"); } }
    }
    public class crime_animal_killed
    {
        public static ScannerFile Animalkilledbyperson { get { return new ScannerFile("01_crime_animal_killed\\0x0BAACC73.wav", "Animal killed by person.", "crime_animal_killed"); } }
        public static ScannerFile Animalkilled { get { return new ScannerFile("01_crime_animal_killed\\0x0F3A9393.wav", "Animal killed.", "crime_animal_killed"); } }
    }
    public class crime_armored_car_robbery
    {
        public static ScannerFile Anarmoredcarrobbery { get { return new ScannerFile("01_crime_armored_car_robbery\\0x0E8B5D1D.wav", "An armored car robbery.", "crime_armored_car_robbery"); } }
        public static ScannerFile Apossiblearmoredcarrobbery { get { return new ScannerFile("01_crime_armored_car_robbery\\0x1D40BA88.wav", "A possible armored car robbery.", "crime_armored_car_robbery"); } }
    }
    public class crime_arson
    {
        public static ScannerFile Anarsonattack { get { return new ScannerFile("01_crime_arson\\0x040FE61D.wav", "An arson attack.", "crime_arson"); } }
        public static ScannerFile Arson { get { return new ScannerFile("01_crime_arson\\0x124D0298.wav", "Arson.", "crime_arson"); } }
    }
    public class crime_assault
    {
        public static ScannerFile Apossibleassault { get { return new ScannerFile("01_crime_assault\\0x1009963F.wav", "A possible assault.", "crime_assault"); } }
        public static ScannerFile Apossibleassault1 { get { return new ScannerFile("01_crime_assault\\0x1E4332B2.wav", "A possible assault.", "crime_assault"); } }
    }
    public class crime_assault_and_battery
    {
        public static ScannerFile AnAE { get { return new ScannerFile("01_crime_assault_and_battery\\0x0C4A5075.wav", "An A&E.", "crime_assault_and_battery"); } }
        public static ScannerFile Anassaultandbattery { get { return new ScannerFile("01_crime_assault_and_battery\\0x1AF12DC3.wav", "An assault and battery.", "crime_assault_and_battery"); } }
    }
    public class crime_assault_on_an_officer
    {
        public static ScannerFile Anassaultonanofficer { get { return new ScannerFile("01_crime_assault_on_an_officer\\0x162AEAAA.wav", "An assault on an officer.", "crime_assault_on_an_officer"); } }
        public static ScannerFile Anofficerassault { get { return new ScannerFile("01_crime_assault_on_an_officer\\0x1F80BD56.wav", "An officer assault.", "crime_assault_on_an_officer"); } }
    }
    public class crime_assault_on_a_civilian { public static ScannerFile Anassaultonacivilian { get { return new ScannerFile("01_crime_assault_on_a_civilian\\0x058AC21E.wav", "An assault on a civilian.", "crime_assault_on_a_civilian"); } } }
    public class crime_assault_with_a_deadly_weapon
    {
        public static ScannerFile AnADW { get { return new ScannerFile("01_crime_assault_with_a_deadly_weapon\\0x0B1C964E.wav", "An ADW.", "crime_assault_with_a_deadly_weapon"); } }
        public static ScannerFile Assaultwithadeadlyweapon { get { return new ScannerFile("01_crime_assault_with_a_deadly_weapon\\0x1909F229.wav", "Assault with a deadly weapon.", "crime_assault_with_a_deadly_weapon"); } }
    }
    public class crime_association
    {
        public static ScannerFile Associationwithanindividualengagedincriminalactivity { get { return new ScannerFile("01_crime_association\\0x047A56F7.wav", "Association with an individual engaged in criminal activity.", "crime_association"); } }
        public static ScannerFile Associationwithaknownfelon { get { return new ScannerFile("01_crime_association\\0x11807101.wav", "Association with a known felon.", "crime_association"); } }
        public static ScannerFile Associationwithaknowncriminal { get { return new ScannerFile("01_crime_association\\0x123BF27A.wav", "Association with a known criminal.", "crime_association"); } }
        public static ScannerFile Associationwithanindividualengagedincriminalactivity1 { get { return new ScannerFile("01_crime_association\\0x16813B05.wav", "Association with an individual engaged in criminal activity.", "crime_association"); } }
        public static ScannerFile Acivilianwantedforassociationwithaknownfelon { get { return new ScannerFile("01_crime_association\\0x1F598CB7.wav", "A civilian wanted for association with a known felon.", "crime_association"); } }
    }
    public class crime_attack_on_an_endangered_species { public static ScannerFile Anattackonanendangeredspecies { get { return new ScannerFile("01_crime_attack_on_an_endangered_species\\0x1B784403.wav", "An attack on an endangered species.", "crime_attack_on_an_endangered_species"); } } }
    public class crime_attack_on_an_officer { public static ScannerFile Anattackonanofficer { get { return new ScannerFile("01_crime_attack_on_an_officer\\0x05D1A54E.wav", "An attack on an officer.", "crime_attack_on_an_officer"); } } }
    public class crime_attack_on_a_motor_vehicle { public static ScannerFile Anattackonamotorvehicle { get { return new ScannerFile("01_crime_attack_on_a_motor_vehicle\\0x05B29DBA.wav", "An attack on a motor vehicle.", "crime_attack_on_a_motor_vehicle"); } } }
    public class crime_attack_on_a_protected_species
    {
        public static ScannerFile Anattackonaprotectedspecies { get { return new ScannerFile("01_crime_attack_on_a_protected_species\\0x07F5B9BF.wav", "An attack on a protected species.", "crime_attack_on_a_protected_species"); } }
        public static ScannerFile Attackonaprotectedspecies { get { return new ScannerFile("01_crime_attack_on_a_protected_species\\0x19A71D22.wav", "Attack on a protected species.", "crime_attack_on_a_protected_species"); } }
    }
    public class crime_attack_on_a_vehicle { public static ScannerFile Anattackonavehicle { get { return new ScannerFile("01_crime_attack_on_a_vehicle\\0x0BCD234E.wav", "An attack on a vehicle.", "crime_attack_on_a_vehicle"); } } }
    public class crime_attempted_homicide { public static ScannerFile Anattemptedhomicide { get { return new ScannerFile("01_crime_attempted_homicide\\0x137F395C.wav", "An attempted homicide.", "crime_attempted_homicide"); } } }
    public class crime_bank_robbery
    {
        public static ScannerFile Abankrobbery { get { return new ScannerFile("01_crime_bank_robbery\\0x08797576.wav", "A bank robbery.", "crime_bank_robbery"); } }
        public static ScannerFile Abankheist { get { return new ScannerFile("01_crime_bank_robbery\\0x0D703F9D.wav", "A bank heist.", "crime_bank_robbery"); } }
        public static ScannerFile Abankrobbery1 { get { return new ScannerFile("01_crime_bank_robbery\\0x12590970.wav", "A bank robbery.", "crime_bank_robbery"); } }
        public static ScannerFile Apossiblebankrobbery { get { return new ScannerFile("01_crime_bank_robbery\\0x18C2D609.wav", "A possible bank robbery.", "crime_bank_robbery"); } }
        public static ScannerFile Apossiblebankrobbery1 { get { return new ScannerFile("01_crime_bank_robbery\\0x1B249B06.wav", "A possible bank robbery.", "crime_bank_robbery"); } }
    }
    public class crime_burglary { public static ScannerFile Apossibleburglary { get { return new ScannerFile("01_crime_burglary\\0x065CDE1D.wav", "A possible burglary.", "crime_burglary"); } } }
    public class crime_car_jacking
    {
        public static ScannerFile Apossiblecarjacking { get { return new ScannerFile("01_crime_car_jacking\\0x0BFAF610.wav", "A possible carjacking.", "crime_car_jacking"); } }
        public static ScannerFile Acarjacking { get { return new ScannerFile("01_crime_car_jacking\\0x1A6892EB.wav", "A carjacking.", "crime_car_jacking"); } }
    }
    public class crime_car_on_fire
    {
        public static ScannerFile Acarfire { get { return new ScannerFile("01_crime_car_on_fire\\0x065069D8.wav", "A car fire.", "crime_car_on_fire"); } }
        public static ScannerFile Acaronfire { get { return new ScannerFile("01_crime_car_on_fire\\0x13964464.wav", "A car on fire.", "crime_car_on_fire"); } }
        public static ScannerFile Anautomobileonfire { get { return new ScannerFile("01_crime_car_on_fire\\0x18138D5F.wav", "An automobile on fire.", "crime_car_on_fire"); } }
    }
    public class crime_civilian_down { public static ScannerFile Aciviliandown { get { return new ScannerFile("01_crime_civilian_down\\0x0320C921.wav", "A civilian down.", "crime_civilian_down"); } } }
    public class crime_civilian_fatality { public static ScannerFile Acivilianfatality { get { return new ScannerFile("01_crime_civilian_fatality\\0x00445298.wav", "A civilian fatality.", "crime_civilian_fatality"); } } }
    public class crime_civilian_needing_assistance
    {
        public static ScannerFile Acivilianinneedofassistance { get { return new ScannerFile("01_crime_civilian_needing_assistance\\0x09EF3EE7.wav", "A civilian in need of assistance.", "crime_civilian_needing_assistance"); } }
        public static ScannerFile Acivilianrequiringassistance { get { return new ScannerFile("01_crime_civilian_needing_assistance\\0x0B380179.wav", "A civilian requiring assistance.", "crime_civilian_needing_assistance"); } }
    }
    public class crime_civillian_gsw
    {
        public static ScannerFile AcivilianGSW { get { return new ScannerFile("01_crime_civillian_gsw\\0x0382F686.wav", "A civilian GSW.", "crime_civillian_gsw"); } }
        public static ScannerFile Agunshotwound { get { return new ScannerFile("01_crime_civillian_gsw\\0x11579233.wav", "A gunshot wound.", "crime_civillian_gsw"); } }
        public static ScannerFile Acivilianshot { get { return new ScannerFile("01_crime_civillian_gsw\\0x15439A09.wav", "A civilian shot.", "crime_civillian_gsw"); } }
    }
    public class crime_civillian_on_fire { public static ScannerFile Acivilianonfire { get { return new ScannerFile("01_crime_civillian_on_fire\\0x0CCE003C.wav", "A civilian on fire.", "crime_civillian_on_fire"); } } }
    public class crime_civil_disturbance { public static ScannerFile Acivildisturbance { get { return new ScannerFile("01_crime_civil_disturbance\\0x16A9B4AA.wav", "A civil disturbance.", "crime_civil_disturbance"); } } }
    //public class crime_code { public static ScannerFile 217 { get {return new ScannerFile("01_crime_code\\0x00FB6351.wav","217","crime_code"); } }
    //public static ScannerFile 10851 { get {return new ScannerFile("01_crime_code\\0x0103395C.wav","10-851.","crime_code"); } }
    //public static ScannerFile 488 { get {return new ScannerFile("01_crime_code\\0x0188BBA3.wav","488","crime_code"); } }
    //public static ScannerFile 904Ita { get {return new ScannerFile("01_crime_code\\0x030753BA.wav","904-Ita.","crime_code"); } }
    //public static ScannerFile 901 { get {return new ScannerFile("01_crime_code\\0x030C3584.wav","901","crime_code"); } }
    //public static ScannerFile 487 { get {return new ScannerFile("01_crime_code\\0x0310EC28.wav","487","crime_code"); } }
    //public static ScannerFile 419 { get {return new ScannerFile("01_crime_code\\0x03243366.wav","419","crime_code"); } }
    //public static ScannerFile 207 { get {return new ScannerFile("01_crime_code\\0x038A9512.wav","207","crime_code"); } }
    //public static ScannerFile 417King { get {return new ScannerFile("01_crime_code\\0x03EB0FF6.wav","417-King.","crime_code"); } }
    //public static ScannerFile 245 { get {return new ScannerFile("01_crime_code\\0x0401D618.wav","245","crime_code"); } }
    //public static ScannerFile 903Lincoln { get {return new ScannerFile("01_crime_code\\0x04D4AD2D.wav","903-Lincoln.","crime_code"); } }
    //public static ScannerFile 914Adam { get {return new ScannerFile("01_crime_code\\0x04EF35D7.wav","914-Adam.","crime_code"); } }
    //public static ScannerFile 907 { get {return new ScannerFile("01_crime_code\\0x05065437.wav","907","crime_code"); } }
    //public static ScannerFile 480 { get {return new ScannerFile("01_crime_code\\0x06093E1B.wav","480","crime_code"); } }
    //public static ScannerFile 996 { get {return new ScannerFile("01_crime_code\\0x06368CB0.wav","996","crime_code"); } }
    //public static ScannerFile 902Mary { get {return new ScannerFile("01_crime_code\\0x06736F2D.wav","902-Mary.","crime_code"); } }
    //public static ScannerFile 904Sam { get {return new ScannerFile("01_crime_code\\0x069F3F90.wav","904-Sam.","crime_code"); } }
    //public static ScannerFile 929 { get {return new ScannerFile("01_crime_code\\0x06B2322F.wav","929","crime_code"); } }
    //public static ScannerFile 484 { get {return new ScannerFile("01_crime_code\\0x06DFA11C.wav","484","crime_code"); } }
    //public static ScannerFile 902 { get {return new ScannerFile("01_crime_code\\0x06EA9253.wav","902","crime_code"); } }
    //public static ScannerFile 502 { get {return new ScannerFile("01_crime_code\\0x0746316D.wav","502","crime_code"); } }
    //public static ScannerFile 904Charles { get {return new ScannerFile("01_crime_code\\0x07A4EF29.wav","904-Charles.","crime_code"); } }
    //public static ScannerFile 927David { get {return new ScannerFile("01_crime_code\\0x07D4794C.wav","927-David.","crime_code"); } }
    //public static ScannerFile 414 { get {return new ScannerFile("01_crime_code\\0x0808B6D4.wav","414","crime_code"); } }
    //public static ScannerFile 966 { get {return new ScannerFile("01_crime_code\\0x0863A7EE.wav","966","crime_code"); } }
    //public static ScannerFile 909Boy { get {return new ScannerFile("01_crime_code\\0x09135B68.wav","909-Boy.","crime_code"); } }
    //public static ScannerFile 311 { get {return new ScannerFile("01_crime_code\\0x094F689E.wav","311","crime_code"); } }
    //public static ScannerFile 11351 { get {return new ScannerFile("01_crime_code\\0x0996DA87.wav","11-351.","crime_code"); } }
    //public static ScannerFile 240 { get {return new ScannerFile("01_crime_code\\0x09D448E7.wav","240","crime_code"); } }
    //public static ScannerFile 594 { get {return new ScannerFile("01_crime_code\\0x09F3C77B.wav","594","crime_code"); } }
    //public static ScannerFile 510 { get {return new ScannerFile("01_crime_code\\0x0BC94D46.wav","510","crime_code"); } }
    //public static ScannerFile 148 { get {return new ScannerFile("01_crime_code\\0x0BF24A76.wav","148","crime_code"); } }
    //public static ScannerFile 904Adam { get {return new ScannerFile("01_crime_code\\0x0C36AD40.wav","904-Adam.","crime_code"); } }
    //public static ScannerFile 390 { get {return new ScannerFile("01_crime_code\\0x0C381C87.wav","390","crime_code"); } }
    //public static ScannerFile 507 { get {return new ScannerFile("01_crime_code\\0x0D2C6CAE.wav","507","crime_code"); } }
    //public static ScannerFile 187 { get {return new ScannerFile("01_crime_code\\0x0DA299B4.wav","187","crime_code"); } }
    //public static ScannerFile 484PaulSam { get {return new ScannerFile("01_crime_code\\0x0DC9BC8D.wav","484-Paul-Sam.","crime_code"); } }
    //public static ScannerFile 444 { get {return new ScannerFile("01_crime_code\\0x0DF44376.wav","444","crime_code"); } }
    //public static ScannerFile 211 { get {return new ScannerFile("01_crime_code\\0x0E7FA39E.wav","211","crime_code"); } }
    //public static ScannerFile 417 { get {return new ScannerFile("01_crime_code\\0x0FAE97B7.wav","417","crime_code"); } }
    //public static ScannerFile 288 { get {return new ScannerFile("01_crime_code\\0x10FAE4CB.wav","288","crime_code"); } }
    //public static ScannerFile 213 { get {return new ScannerFile("01_crime_code\\0x11F9EB28.wav","213","crime_code"); } }
    //public static ScannerFile 983 { get {return new ScannerFile("01_crime_code\\0x13DE7588.wav","983","crime_code"); } }
    //public static ScannerFile 481 { get {return new ScannerFile("01_crime_code\\0x142813A7.wav","481","crime_code"); } }
    //public static ScannerFile 905Victor { get {return new ScannerFile("01_crime_code\\0x143869C1.wav","905-Victor.","crime_code"); } }
    //public static ScannerFile 967 { get {return new ScannerFile("01_crime_code\\0x1438833D.wav","967","crime_code"); } }
    //public static ScannerFile 504 { get {return new ScannerFile("01_crime_code\\0x1605163B.wav","504","crime_code"); } }
    //public static ScannerFile 903 { get {return new ScannerFile("01_crime_code\\0x160BB4CD.wav","903","crime_code"); } }
    //public static ScannerFile 246 { get {return new ScannerFile("01_crime_code\\0x1630735E.wav","246","crime_code"); } }
    //public static ScannerFile 406 { get {return new ScannerFile("01_crime_code\\0x1692A6C2.wav","406","crime_code"); } }
    //public static ScannerFile 11357 { get {return new ScannerFile("01_crime_code\\0x18D16FE2.wav","11-357.","crime_code"); } }
    //public static ScannerFile 925 { get {return new ScannerFile("01_crime_code\\0x18ED7E1A.wav","925","crime_code"); } }
    //public static ScannerFile 1024 { get {return new ScannerFile("01_crime_code\\0x192D5BC6.wav","10-24.","crime_code"); } }
    //public static ScannerFile 921 { get {return new ScannerFile("01_crime_code\\0x1AAFC639.wav","921","crime_code"); } }
    //public static ScannerFile 653Mary { get {return new ScannerFile("01_crime_code\\0x1AF30F89.wav","653-Mary.","crime_code"); } }
    //public static ScannerFile 586 { get {return new ScannerFile("01_crime_code\\0x1B01B8D9.wav","586","crime_code"); } }
    //public static ScannerFile 999 { get {return new ScannerFile("01_crime_code\\0x1B880933.wav","999","crime_code"); } }
    //public static ScannerFile 503 { get {return new ScannerFile("01_crime_code\\0x1B94ADAB.wav","503","crime_code"); } }
    //public static ScannerFile 505 { get {return new ScannerFile("01_crime_code\\0x1D3FB67D.wav","505","crime_code"); } }
    //public static ScannerFile 242 { get {return new ScannerFile("01_crime_code\\0x1E6525C5.wav","242","crime_code"); } }
    //public static ScannerFile 415George { get {return new ScannerFile("01_crime_code\\0x1E7426F9.wav","415-George.","crime_code"); } }
    //public static ScannerFile 459 { get {return new ScannerFile("01_crime_code\\0x1E8F076F.wav","459","crime_code"); } }
    //public static ScannerFile 604 { get {return new ScannerFile("01_crime_code\\0x1F2EBBEC.wav","604","crime_code"); } }}
    public class crime_criminal_activity
    {
        public static ScannerFile Areportofillegalactivity { get { return new ScannerFile("01_crime_criminal_activity\\0x02EE6B4D.wav", "A report of illegal activity.", "crime_criminal_activity"); } }
        public static ScannerFile Illegalactivity { get { return new ScannerFile("01_crime_criminal_activity\\0x09E0F933.wav", "Illegal activity.", "crime_criminal_activity"); } }
        public static ScannerFile Prohibitedactivity { get { return new ScannerFile("01_crime_criminal_activity\\0x112987C5.wav", "Prohibited activity.", "crime_criminal_activity"); } }
        public static ScannerFile Areportofcriminalactivity { get { return new ScannerFile("01_crime_criminal_activity\\0x1160C832.wav", "A report of criminal activity.", "crime_criminal_activity"); } }
        public static ScannerFile Criminalactivity { get { return new ScannerFile("01_crime_criminal_activity\\0x1B755C5C.wav", "Criminal activity.", "crime_criminal_activity"); } }
        public static ScannerFile Areportofacriminalactivity { get { return new ScannerFile("01_crime_criminal_activity\\0x1E7A2266.wav", "A report of a criminal activity.", "crime_criminal_activity"); } }
    }
    public class crime_dangerous_driving
    {
        public static ScannerFile Dangerousdriving { get { return new ScannerFile("01_crime_dangerous_driving\\0x0129A4E4.wav", "Dangerous driving.", "crime_dangerous_driving"); } }
        public static ScannerFile Dangerousdriving1 { get { return new ScannerFile("01_crime_dangerous_driving\\0x1B6C996A.wav", "Dangerous driving.", "crime_dangerous_driving"); } }
    }
    public class crime_dead_body { public static ScannerFile Adeadbody { get { return new ScannerFile("01_crime_dead_body\\0x1567C297.wav", "A dead body.", "crime_dead_body"); } } }
    public class crime_disturbance
    {
        public static ScannerFile Apossibledisturbance { get { return new ScannerFile("01_crime_disturbance\\0x06A8CC92.wav", "A possible disturbance.", "crime_disturbance"); } }
        public static ScannerFile Adisturbance { get { return new ScannerFile("01_crime_disturbance\\0x146B2817.wav", "A disturbance.", "crime_disturbance"); } }
        public static ScannerFile Adisturbance1 { get { return new ScannerFile("01_crime_disturbance\\0x14AEA8AC.wav", "A disturbance.", "crime_disturbance"); } }
        public static ScannerFile A415 { get { return new ScannerFile("01_crime_disturbance\\0x19F7733E.wav", "A 415.", "crime_disturbance"); } }
    }
    public class crime_domestic_disturbance
    {
        public static ScannerFile Adomesticdisturbance { get { return new ScannerFile("01_crime_domestic_disturbance\\0x0BECC465.wav", "A domestic disturbance.", "crime_domestic_disturbance"); } }
        public static ScannerFile Apossibledomesticdisturbance { get { return new ScannerFile("01_crime_domestic_disturbance\\0x1A19A0BE.wav", "A possible domestic disturbance.", "crime_domestic_disturbance"); } }
    }
    public class crime_domestic_violence_incident { public static ScannerFile Domesticviolenceincident { get { return new ScannerFile("01_crime_domestic_violence_incident\\0x0533758F.wav", "Domestic violence incident.", "crime_domestic_violence_incident"); } } }
    public class crime_driveby_shooting { public static ScannerFile Adrivebyshooting { get { return new ScannerFile("01_crime_driveby_shooting\\0x1C66805E.wav", "A driveby shooting.", "crime_driveby_shooting"); } } }
    public class crime_drug_deal
    {
        public static ScannerFile Narcoticstrafficking { get { return new ScannerFile("01_crime_drug_deal\\0x043280CC.wav", "Narcotics trafficking.", "crime_drug_deal"); } }
        public static ScannerFile Adrugdeal { get { return new ScannerFile("01_crime_drug_deal\\0x0EB8D5D8.wav", "A drug deal.", "crime_drug_deal"); } }
        public static ScannerFile Adrugdealinprogress { get { return new ScannerFile("01_crime_drug_deal\\0x15D9E419.wav", "A drug deal in progress.", "crime_drug_deal"); } }
        public static ScannerFile Apossibledrugdeal { get { return new ScannerFile("01_crime_drug_deal\\0x18F3AA4E.wav", "A possible drug deal.", "crime_drug_deal"); } }
    }
    public class crime_drug_overdose
    {
        public static ScannerFile An11357PossibleOD { get { return new ScannerFile("01_crime_drug_overdose\\0x053AC7E1.wav", "An 11-357. Possible OD.", "crime_drug_overdose"); } }
        public static ScannerFile AnODvictim { get { return new ScannerFile("01_crime_drug_overdose\\0x0FF49D54.wav", "An OD victim.", "crime_drug_overdose"); } }
        public static ScannerFile AnOD { get { return new ScannerFile("01_crime_drug_overdose\\0x1217E179.wav", "An OD.", "crime_drug_overdose"); } }
        public static ScannerFile Adrugoverdose { get { return new ScannerFile("01_crime_drug_overdose\\0x195AF020.wav", "A drug overdose.", "crime_drug_overdose"); } }
        public static ScannerFile Anoverdosedcivilian { get { return new ScannerFile("01_crime_drug_overdose\\0x1D563817.wav", "An overdosed civilian.", "crime_drug_overdose"); } }
    }
    public class crime_firearms_incident
    {
        public static ScannerFile AfirearmsincidentShotsfired { get { return new ScannerFile("01_crime_firearms_incident\\0x054E27A4.wav", "A firearms incident. Shots fired.", "crime_firearms_incident"); } }
        public static ScannerFile Anincidentinvolvingshotsfired { get { return new ScannerFile("01_crime_firearms_incident\\0x0684AA0E.wav", "An incident involving shots fired.", "crime_firearms_incident"); } }
        public static ScannerFile AweaponsincidentShotsfired { get { return new ScannerFile("01_crime_firearms_incident\\0x193ACF7B.wav", "A weapons incident. Shots fired.", "crime_firearms_incident"); } }
    }
    public class crime_firearms_possession { public static ScannerFile Afirearmspossession { get { return new ScannerFile("01_crime_firearms_possession\\0x0C2145BC.wav", "A firearms possession.", "crime_firearms_possession"); } } }
    public class crime_firearm_discharged_in_a_public_place { public static ScannerFile Afirearmdischargedinapublicplace { get { return new ScannerFile("01_crime_firearm_discharged_in_a_public_place\\0x085D09EB.wav", "A firearm discharged in a public place.", "crime_firearm_discharged_in_a_public_place"); } } }
    public class crime_fire_alarm { public static ScannerFile Afirealarm { get { return new ScannerFile("01_crime_fire_alarm\\0x0B3DC516.wav", "A fire alarm.", "crime_fire_alarm"); } } }
    public class crime_gang_activity_incident { public static ScannerFile Agangactivityincident { get { return new ScannerFile("01_crime_gang_activity_incident\\0x18F48FFD.wav", "A gang activity incident.", "crime_gang_activity_incident"); } } }
    public class crime_gang_related_violence
    {
        public static ScannerFile Gangrelatedviolence { get { return new ScannerFile("01_crime_gang_related_violence\\0x0092E590.wav", "Gang-related violence.", "crime_gang_related_violence"); } }
        public static ScannerFile Gangrelatedviolence1 { get { return new ScannerFile("01_crime_gang_related_violence\\0x0EB801DB.wav", "Gang-related violence.", "crime_gang_related_violence"); } }
    }
    public class crime_grand_theft_auto
    {
        public static ScannerFile AGTAinprogress { get { return new ScannerFile("01_crime_grand_theft_auto\\0x008EDFEF.wav", "A GTA in progress.", "crime_grand_theft_auto"); } }
        public static ScannerFile Agrandtheftautoinprogress { get { return new ScannerFile("01_crime_grand_theft_auto\\0x0A4D736D.wav", "A grand theft auto in progress.", "crime_grand_theft_auto"); } }
        public static ScannerFile AGTAinprogress1 { get { return new ScannerFile("01_crime_grand_theft_auto\\0x0AA4B41B.wav", "A GTA in progress.", "crime_grand_theft_auto"); } }
        public static ScannerFile Agrandtheftauto { get { return new ScannerFile("01_crime_grand_theft_auto\\0x1C0316D8.wav", "A grand theft auto.", "crime_grand_theft_auto"); } }
    }
    public class crime_gsw_driveby_attack { public static ScannerFile Adrivebyattack { get { return new ScannerFile("01_crime_gsw_driveby_attack\\0x0AC6744D.wav", "A driveby attack.", "crime_gsw_driveby_attack"); } } }
    public class crime_helicopter_down
    {
        public static ScannerFile Ahelicopterdown { get { return new ScannerFile("01_crime_helicopter_down\\0x1DB63742.wav", "A helicopter down.", "crime_helicopter_down"); } }
        public static ScannerFile Achopperdown { get { return new ScannerFile("01_crime_helicopter_down\\0x1F6ABAA8.wav", "A chopper down.", "crime_helicopter_down"); } }
    }
    public class crime_high_ranking_gang_member_in_transit { public static ScannerFile Ahighrankinggangmemberintransit { get { return new ScannerFile("01_crime_high_ranking_gang_member_in_transit\\0x0920EE1F.wav", "A high-ranking gang member in transit.", "crime_high_ranking_gang_member_in_transit"); } } }
    public class crime_hijacked_aircraft { public static ScannerFile Ahijackedaircraft { get { return new ScannerFile("01_crime_hijacked_aircraft\\0x1CFA777C.wav", "A hijacked aircraft.", "crime_hijacked_aircraft"); } } }
    public class crime_hijacked_vehicle { public static ScannerFile Ahijackedvehicle { get { return new ScannerFile("01_crime_hijacked_vehicle\\0x0E518420.wav", "A hijacked vehicle.", "crime_hijacked_vehicle"); } } }
    public class crime_hold_up { public static ScannerFile Aholdup { get { return new ScannerFile("01_crime_hold_up\\0x162A5F2D.wav", "A hold-up.", "crime_hold_up"); } } }
    public class crime_hunting_an_endangered_species { public static ScannerFile Huntinganendangeredspecies { get { return new ScannerFile("01_crime_hunting_an_endangered_species\\0x1AFDD2BB.wav", "Hunting an endangered species.", "crime_hunting_an_endangered_species"); } } }
    public class crime_hunting_without_a_permit
    {
        public static ScannerFile Personhuntingwithoutapermit { get { return new ScannerFile("01_crime_hunting_without_a_permit\\0x08CC0434.wav", "Person hunting without a permit.", "crime_hunting_without_a_permit"); } }
        public static ScannerFile Huntingwithoutapermit { get { return new ScannerFile("01_crime_hunting_without_a_permit\\0x19FEA699.wav", "Hunting without a permit.", "crime_hunting_without_a_permit"); } }
    }
    public class crime_illegal_burning
    {
        public static ScannerFile Anillegalfire { get { return new ScannerFile("01_crime_illegal_burning\\0x1802EBFB.wav", "An illegal fire.", "crime_illegal_burning"); } }
        public static ScannerFile Illegalburning { get { return new ScannerFile("01_crime_illegal_burning\\0x1ED03998.wav", "Illegal burning.", "crime_illegal_burning"); } }
    }
    public class crime_injured_civilian { public static ScannerFile Aninjuredcivilian { get { return new ScannerFile("01_crime_injured_civilian\\0x1AF8CB2D.wav", "An injured civilian.", "crime_injured_civilian"); } } }
    public class crime_killing_animals { public static ScannerFile Killinganimals { get { return new ScannerFile("01_crime_killing_animals\\0x0651B03C.wav", "Killing animals.", "crime_killing_animals"); } } }
    public class crime_kinfe_assault_on_an_officer { public static ScannerFile Aknifeassaultonanofficer { get { return new ScannerFile("01_crime_kinfe_assault_on_an_officer\\0x18674968.wav", "A knife assault on an officer.", "crime_kinfe_assault_on_an_officer"); } } }
    public class crime_low_flying_aircraft
    {
        public static ScannerFile Alowflyingaircraft { get { return new ScannerFile("01_crime_low_flying_aircraft\\0x0ED60811.wav", "A low-flying aircraft.", "crime_low_flying_aircraft"); } }
        public static ScannerFile Lowflyingaircraft { get { return new ScannerFile("01_crime_low_flying_aircraft\\0x1D91E58B.wav", "Low-flying aircraft.", "crime_low_flying_aircraft"); } }
    }
    public class crime_malicious_damage_to_property { public static ScannerFile Maliciousdamagetoproperty { get { return new ScannerFile("01_crime_malicious_damage_to_property\\0x0E66D16B.wav", "Malicious damage to property.", "crime_malicious_damage_to_property"); } } }
    public class crime_malicious_vehicle_damage { public static ScannerFile Maliciousvehicledamage { get { return new ScannerFile("01_crime_malicious_vehicle_damage\\0x13B65F4F.wav", "Malicious vehicle damage.", "crime_malicious_vehicle_damage"); } } }
    public class crime_mdv { public static ScannerFile MDV { get { return new ScannerFile("01_crime_mdv\\0x0419EF0A.wav", "MDV.", "crime_mdv"); } } }
    public class crime_medical_aid_requested { public static ScannerFile Medicalaidrequested { get { return new ScannerFile("01_crime_medical_aid_requested\\0x0BB0BD5B.wav", "Medical aid requested.", "crime_medical_aid_requested"); } } }
    public class crime_motorcycle_rider_without_a_helmet
    {
        public static ScannerFile Amotorcycleridersseenwithoutahelmet { get { return new ScannerFile("01_crime_motorcycle_rider_without_a_helmet\\0x0563B805.wav", "A motorcycle rider(s) seen without a helmet.", "crime_motorcycle_rider_without_a_helmet"); } }
        public static ScannerFile Ridingamotorcyclewithoutahelmet { get { return new ScannerFile("01_crime_motorcycle_rider_without_a_helmet\\0x139E947B.wav", "Riding a motorcycle without a helmet.", "crime_motorcycle_rider_without_a_helmet"); } }
        public static ScannerFile Amotorcycleriderwithoutahelmet { get { return new ScannerFile("01_crime_motorcycle_rider_without_a_helmet\\0x13D5D4E1.wav", "A motorcycle rider without a helmet.", "crime_motorcycle_rider_without_a_helmet"); } }
    }
    public class crime_motor_vehicle_accident
    {
        public static ScannerFile Amotorvehicleaccident { get { return new ScannerFile("01_crime_motor_vehicle_accident\\0x0305104D.wav", "A motor vehicle accident.", "crime_motor_vehicle_accident"); } }
        public static ScannerFile AnAEincident { get { return new ScannerFile("01_crime_motor_vehicle_accident\\0x0829DA96.wav", "An A&E incident.", "crime_motor_vehicle_accident"); } }
        public static ScannerFile AseriousMVA { get { return new ScannerFile("01_crime_motor_vehicle_accident\\0x1E710726.wav", "A serious MVA.", "crime_motor_vehicle_accident"); } }
    }
    public class crime_moving_violation { public static ScannerFile Amovingviolation { get { return new ScannerFile("01_crime_moving_violation\\0x13C84BC4.wav", "A moving violation.", "crime_moving_violation"); } } }
    public class crime_mugging { public static ScannerFile Apossiblemugging { get { return new ScannerFile("01_crime_mugging\\0x195CFE4E.wav", "A possible mugging.", "crime_mugging"); } } }
    public class crime_multiple_injuries { public static ScannerFile Multipleinjuries { get { return new ScannerFile("01_crime_multiple_injuries\\0x03841223.wav", "Multiple injuries.", "crime_multiple_injuries"); } } }
    public class crime_narcotics_activity { public static ScannerFile Narcoticsactivity { get { return new ScannerFile("01_crime_narcotics_activity\\0x060CE903.wav", "Narcotics activity.", "crime_narcotics_activity"); } } }
    public class crime_narcotics_in_transit { public static ScannerFile Narcoticsintransit { get { return new ScannerFile("01_crime_narcotics_in_transit\\0x0E83D176.wav", "Narcotics in transit.", "crime_narcotics_in_transit"); } } }
    public class crime_officers_down
    {
        public static ScannerFile Severalofficersdown { get { return new ScannerFile("01_crime_officers_down\\0x10A785B3.wav", "Several officers down.", "crime_officers_down"); } }
        public static ScannerFile Multipleofficersdown { get { return new ScannerFile("01_crime_officers_down\\0x1FF1244F.wav", "Multiple officers down.", "crime_officers_down"); } }
    }
    public class crime_officer_assault { public static ScannerFile Anofficerassault { get { return new ScannerFile("01_crime_officer_assault\\0x11374E1D.wav", "An officer assault.", "crime_officer_assault"); } } }
    public class crime_officer_down
    {
        public static ScannerFile Anofficerdownconditionunknown { get { return new ScannerFile("01_crime_officer_down\\0x01FDB341.wav", "An officer down; condition unknown.", "crime_officer_down"); } }
        public static ScannerFile Anofficerdown { get { return new ScannerFile("01_crime_officer_down\\0x05AFFAAA.wav", "An officer down.", "crime_officer_down"); } }
        public static ScannerFile AnofferdownpossiblyKIA { get { return new ScannerFile("01_crime_officer_down\\0x143757B4.wav", "An offer down, possibly KIA.", "crime_officer_down"); } }
        public static ScannerFile AcriticalsituationOfficerdown { get { return new ScannerFile("01_crime_officer_down\\0x17C49ECF.wav", "A critical situation: Officer down.", "crime_officer_down"); } }
    }
    public class crime_officer_fatality
    {
        public static ScannerFile Anofficerhomicide { get { return new ScannerFile("01_crime_officer_fatality\\0x06FE2FC0.wav", "An officer homicide.", "crime_officer_fatality"); } }
        public static ScannerFile Anofficerfatality { get { return new ScannerFile("01_crime_officer_fatality\\0x1844124C.wav", "An officer fatality.", "crime_officer_fatality"); } }
    }
    public class crime_officer_homicide { public static ScannerFile Anofficerhomicide { get { return new ScannerFile("01_crime_officer_homicide\\0x146BDE45.wav", "An officer homicide.", "crime_officer_homicide"); } } }
    public class crime_officer_injured { public static ScannerFile Anofficerinjured { get { return new ScannerFile("01_crime_officer_injured\\0x095FEC28.wav", "An officer injured.", "crime_officer_injured"); } } }
    public class crime_officer_in_danger { public static ScannerFile Anofficerindanger { get { return new ScannerFile("01_crime_officer_in_danger\\0x0E2B57AB.wav", "An officer in danger.", "crime_officer_in_danger"); } } }
    public class crime_officer_in_need_of_assistance
    {
        public static ScannerFile Anofficerrequiringassistance { get { return new ScannerFile("01_crime_officer_in_need_of_assistance\\0x04B233C3.wav", "An officer requiring assistance.", "crime_officer_in_need_of_assistance"); } }
        public static ScannerFile Anofficerinneedofassistance { get { return new ScannerFile("01_crime_officer_in_need_of_assistance\\0x1AA91FB1.wav", "An officer in need of assistance.", "crime_officer_in_need_of_assistance"); } }
    }
    public class crime_officer_on_fire
    {
        public static ScannerFile Anofficeronfire { get { return new ScannerFile("01_crime_officer_on_fire\\0x0D64C020.wav", "An officer on fire.", "crime_officer_on_fire"); } }
        public static ScannerFile Anofficersetonfire { get { return new ScannerFile("01_crime_officer_on_fire\\0x1FADA4B3.wav", "An officer set on fire.", "crime_officer_on_fire"); } }
    }
    public class crime_officer_stabbed { public static ScannerFile Anofficerstabbed { get { return new ScannerFile("01_crime_officer_stabbed\\0x16DAE516.wav", "An officer stabbed.", "crime_officer_stabbed"); } } }
    public class crime_officer_struck_by_vehicle
    {
        public static ScannerFile Anofficerstruckbyavehicle { get { return new ScannerFile("01_crime_officer_struck_by_vehicle\\0x05861C4A.wav", "An officer struck by a vehicle.", "crime_officer_struck_by_vehicle"); } }
        public static ScannerFile Apedestrianstruckbyavehicle { get { return new ScannerFile("01_crime_officer_struck_by_vehicle\\0x0F91B046.wav", "A pedestrian struck by a vehicle.", "crime_officer_struck_by_vehicle"); } }
    }
    public class crime_officer_wounded { public static ScannerFile Anofficerwounded { get { return new ScannerFile("01_crime_officer_wounded\\0x15E81134.wav", "An officer wounded.", "crime_officer_wounded"); } } }
    public class crime_pedestrian_involved_accident { public static ScannerFile Apedestrianinvolvedaccident { get { return new ScannerFile("01_crime_pedestrian_involved_accident\\0x1933F786.wav", "A pedestrian-involved accident.", "crime_pedestrian_involved_accident"); } } }
    public class crime_ped_struck_by_veh
    {
        public static ScannerFile Apedestrianstruck { get { return new ScannerFile("01_crime_ped_struck_by_veh\\0x0366CCA6.wav", "A pedestrian struck.", "crime_ped_struck_by_veh"); } }
        public static ScannerFile Apedestrianstruckbyavehicle { get { return new ScannerFile("01_crime_ped_struck_by_veh\\0x071DD422.wav", "A pedestrian struck by a vehicle.", "crime_ped_struck_by_veh"); } }
        public static ScannerFile Apedestrianstruckbyavehicle1 { get { return new ScannerFile("01_crime_ped_struck_by_veh\\0x0BB09D49.wav", "A pedestrian struck by a vehicle.", "crime_ped_struck_by_veh"); } }
        public static ScannerFile Apedestrianstruck1 { get { return new ScannerFile("01_crime_ped_struck_by_veh\\0x1AD73B96.wav", "A pedestrian struck.", "crime_ped_struck_by_veh"); } }
    }
    public class crime_person_attempting_to_steal_a_car { public static ScannerFile Apersonattemptingtostealacar { get { return new ScannerFile("01_crime_person_attempting_to_steal_a_car\\0x0EA0B0C9.wav", "A person attempting to steal a car.", "crime_person_attempting_to_steal_a_car"); } } }
    public class crime_person_down { public static ScannerFile Apersondown { get { return new ScannerFile("01_crime_person_down\\0x084DB426.wav", "A person down.", "crime_person_down"); } } }
    public class crime_person_fleeing_a_crime_scene { public static ScannerFile Apersonfleeingacrimescene { get { return new ScannerFile("01_crime_person_fleeing_a_crime_scene\\0x13FA7AA2.wav", "A person fleeing a crime scene.", "crime_person_fleeing_a_crime_scene"); } } }
    public class crime_person_in_a_stolen_car { public static ScannerFile Apersoninastolencar { get { return new ScannerFile("01_crime_person_in_a_stolen_car\\0x06ECE5BE.wav", "A person in a stolen car.", "crime_person_in_a_stolen_car"); } } }
    public class crime_person_in_a_stolen_vehicle { public static ScannerFile Apersoninastolenvehicle { get { return new ScannerFile("01_crime_person_in_a_stolen_vehicle\\0x0C290C9F.wav", "A person in a stolen vehicle.", "crime_person_in_a_stolen_vehicle"); } } }
    public class crime_person_resisting_arrest { public static ScannerFile Apersonresistingarrest { get { return new ScannerFile("01_crime_person_resisting_arrest\\0x04A12B61.wav", "A person resisting arrest.", "crime_person_resisting_arrest"); } } }
    public class crime_person_running_a_red_light { public static ScannerFile Apersonrunningaredlight { get { return new ScannerFile("01_crime_person_running_a_red_light\\0x0BFCA53A.wav", "A person running a red light.", "crime_person_running_a_red_light"); } } }
    public class crime_person_stealing_a_car { public static ScannerFile Apersonstealingacar { get { return new ScannerFile("01_crime_person_stealing_a_car\\0x03CBA5DC.wav", "A person stealing a car.", "crime_person_stealing_a_car"); } } }
    public class crime_person_transporting_narcotics { public static ScannerFile Apersonorpersonstransportingnarcotics { get { return new ScannerFile("01_crime_person_transporting_narcotics\\0x02CFBABE.wav", "A person or persons transporting narcotics.", "crime_person_transporting_narcotics"); } } }
    public class crime_perverting_justice
    {
        public static ScannerFile Acivilianpervertingthecourseofjustice { get { return new ScannerFile("01_crime_perverting_justice\\0x08AFAEFF.wav", "A civilian perverting the course of justice.", "crime_perverting_justice"); } }
        public static ScannerFile Obstructionofanofficer { get { return new ScannerFile("01_crime_perverting_justice\\0x15400820.wav", "Obstruction of an officer.", "crime_perverting_justice"); } }
    }
    public class crime_pimping_and_solicitation { public static ScannerFile Pimpingandsolicitation { get { return new ScannerFile("01_crime_pimping_and_solicitation\\0x1436A1CD.wav", "Pimping and solicitation.", "crime_pimping_and_solicitation"); } } }
    public class crime_police_convoy_under_attack
    {
        public static ScannerFile Apolicetransportunderattack { get { return new ScannerFile("01_crime_police_convoy_under_attack\\0x0831006A.wav", "A police transport under attack.", "crime_police_convoy_under_attack"); } }
        public static ScannerFile Apoliceconvoyunderattack { get { return new ScannerFile("01_crime_police_convoy_under_attack\\0x1A6FA4E7.wav", "A police convoy under attack.", "crime_police_convoy_under_attack"); } }
    }
    public class crime_property_damage
    {
        public static ScannerFile Damagetoproperty { get { return new ScannerFile("01_crime_property_damage\\0x009DAE97.wav", "Damage to property.", "crime_property_damage"); } }
        public static ScannerFile Propertydamage { get { return new ScannerFile("01_crime_property_damage\\0x020A316D.wav", "Property damage.", "crime_property_damage"); } }
        public static ScannerFile Areportofpropertydamage { get { return new ScannerFile("01_crime_property_damage\\0x13A2949E.wav", "A report of property damage.", "crime_property_damage"); } }
    }
    public class crime_prowler { public static ScannerFile Aprowler { get { return new ScannerFile("01_crime_prowler\\0x05EF2A48.wav", "A prowler.", "crime_prowler"); } } }
    public class crime_reckless_driver { public static ScannerFile Arecklessdriver { get { return new ScannerFile("01_crime_reckless_driver\\0x06F0317A.wav", "A reckless driver.", "crime_reckless_driver"); } } }
    public class crime_road_blockade
    {
        public static ScannerFile Aroadwayblocked { get { return new ScannerFile("01_crime_road_blockade\\0x01CE4B75.wav", "A roadway blocked.", "crime_road_blockade"); } }
        public static ScannerFile Aroadblockade { get { return new ScannerFile("01_crime_road_blockade\\0x0B3C5E40.wav", "A road blockade.", "crime_road_blockade"); } }
        public static ScannerFile Aroadblock { get { return new ScannerFile("01_crime_road_blockade\\0x1D348242.wav", "A roadblock.", "crime_road_blockade"); } }
    }
    public class crime_robbery
    {
        public static ScannerFile Arobbery { get { return new ScannerFile("01_crime_robbery\\0x0A677E12.wav", "A robbery.", "crime_robbery"); } }
        public static ScannerFile Apossiblerobbery { get { return new ScannerFile("01_crime_robbery\\0x149A5278.wav", "A possible robbery.", "crime_robbery"); } }
    }
    public class crime_robbery_with_a_firearm { public static ScannerFile Arobberywithafirearm { get { return new ScannerFile("01_crime_robbery_with_a_firearm\\0x146F5EF1.wav", "A robbery with a firearm.", "crime_robbery_with_a_firearm"); } } }
    public class crime_shooting
    {
        public static ScannerFile Aweaponsincidentshotsfired { get { return new ScannerFile("01_crime_shooting\\0x01AC508D.wav", "A weapons incident; shots fired.", "crime_shooting"); } }
        public static ScannerFile Afirearmssituationseveralshotsfired { get { return new ScannerFile("01_crime_shooting\\0x1F5D8BEF.wav", "A firearms situation; several shots fired.", "crime_shooting"); } }
    }
    public class crime_shooting_at_animals { public static ScannerFile Personshootingatanimals { get { return new ScannerFile("01_crime_shooting_at_animals\\0x0EC4903B.wav", "Person shooting at animals.", "crime_shooting_at_animals"); } } }
    public class crime_shooting_a_protected_bird
    {
        public static ScannerFile Reportsofsuspectshootingaprotectedbird { get { return new ScannerFile("01_crime_shooting_a_protected_bird\\0x0CBF96A2.wav", "Reports of suspect shooting a protected bird.", "crime_shooting_a_protected_bird"); } }
        public static ScannerFile Suspectshootingaprotectedbird { get { return new ScannerFile("01_crime_shooting_a_protected_bird\\0x195A6FDB.wav", "Suspect shooting a protected bird.", "crime_shooting_a_protected_bird"); } }
        public static ScannerFile Reportsofsomeoneshootingaprotectedbird { get { return new ScannerFile("01_crime_shooting_a_protected_bird\\0x1A94324F.wav", "Reports of someone shooting a protected bird.", "crime_shooting_a_protected_bird"); } }
    }
    public class crime_shooting_wildlife { public static ScannerFile Suspectshootingwildlife { get { return new ScannerFile("01_crime_shooting_wildlife\\0x03E5D09E.wav", "Suspect shooting wildlife.", "crime_shooting_wildlife"); } } }
    public class crime_shoot_out { public static ScannerFile Ashootout { get { return new ScannerFile("01_crime_shoot_out\\0x09B682AC.wav", "A shoot-out.", "crime_shoot_out"); } } }
    public class crime_shots_fired
    {
        public static ScannerFile Shotsfired { get { return new ScannerFile("01_crime_shots_fired\\0x0EADE575.wav", "Shots fired.", "crime_shots_fired"); } }
        public static ScannerFile Gunfirereported { get { return new ScannerFile("01_crime_shots_fired\\0x1A7C3D10.wav", "Gunfire reported.", "crime_shots_fired"); } }
        public static ScannerFile Gunshotsreported { get { return new ScannerFile("01_crime_shots_fired\\0x1D3E4296.wav", "Gunshots reported.", "crime_shots_fired"); } }
    }
    public class crime_shots_fired_at_an_officer { public static ScannerFile Shotsfiredatanofficer { get { return new ScannerFile("01_crime_shots_fired_at_an_officer\\0x0B6ECE66.wav", "Shots fired at an officer.", "crime_shots_fired_at_an_officer"); } } }
    public class crime_shots_fired_at_officer
    {
        public static ScannerFile Anofficershot { get { return new ScannerFile("01_crime_shots_fired_at_officer\\0x0237C6BB.wav", "An officer shot.", "crime_shots_fired_at_officer"); } }
        public static ScannerFile Shotsfiredatanofficer { get { return new ScannerFile("01_crime_shots_fired_at_officer\\0x06D48FF4.wav", "Shots fired at an officer.", "crime_shots_fired_at_officer"); } }
        public static ScannerFile Anofficerunderfire { get { return new ScannerFile("01_crime_shots_fired_at_officer\\0x0802D252.wav", "An officer under fire.", "crime_shots_fired_at_officer"); } }
        public static ScannerFile Afirearmattackonanofficer { get { return new ScannerFile("01_crime_shots_fired_at_officer\\0x0C669B19.wav", "A firearm attack on an officer.", "crime_shots_fired_at_officer"); } }
    }
    public class crime_solicitation { public static ScannerFile Solicitation { get { return new ScannerFile("01_crime_solicitation\\0x0DFDDC04.wav", "Solicitation.", "crime_solicitation"); } } }
    public class crime_sos_call
    {
        public static ScannerFile AnSOScall { get { return new ScannerFile("01_crime_sos_call\\0x04F36BF2.wav", "An SOS call.", "crime_sos_call"); } }
        public static ScannerFile AnSOSdistresssignal { get { return new ScannerFile("01_crime_sos_call\\0x123D0686.wav", "An SOS distress signal.", "crime_sos_call"); } }
    }
    public class crime_speeding { public static ScannerFile Speeding { get { return new ScannerFile("01_crime_speeding\\0x1C912AD5.wav", "Speeding.", "crime_speeding"); } } }
    public class crime_speeding_felony { public static ScannerFile Aspeedingfelony { get { return new ScannerFile("01_crime_speeding_felony\\0x01C16921.wav", "A speeding felony.", "crime_speeding_felony"); } } }
    public class crime_speeding_incident { public static ScannerFile Aspeedingincident { get { return new ScannerFile("01_crime_speeding_incident\\0x00BA5D8E.wav", "A speeding incident.", "crime_speeding_incident"); } } }
    public class crime_stabbing
    {
        public static ScannerFile Apossiblestabbing { get { return new ScannerFile("01_crime_stabbing\\0x10112C89.wav", "A possible stabbing.", "crime_stabbing"); } }
        public static ScannerFile Astabbing { get { return new ScannerFile("01_crime_stabbing\\0x1280B178.wav", "A stabbing.", "crime_stabbing"); } }
        public static ScannerFile Astabbing1 { get { return new ScannerFile("01_crime_stabbing\\0x1DDB481E.wav", "A stabbing.", "crime_stabbing"); } }
    }
    public class crime_stolen_aircraft { public static ScannerFile Astolenaircraft { get { return new ScannerFile("01_crime_stolen_aircraft\\0x11D9B7A5.wav", "A stolen aircraft.", "crime_stolen_aircraft"); } } }
    public class crime_stolen_cop_car
    {
        public static ScannerFile Astolenpolicecar { get { return new ScannerFile("01_crime_stolen_cop_car\\0x00EA9A63.wav", "A stolen police car.", "crime_stolen_cop_car"); } }
        public static ScannerFile Astolenpolicevehicle { get { return new ScannerFile("01_crime_stolen_cop_car\\0x063C24EB.wav", "A stolen police vehicle.", "crime_stolen_cop_car"); } }
        public static ScannerFile Astolenpolicevehicle1 { get { return new ScannerFile("01_crime_stolen_cop_car\\0x073466F5.wav", "A stolen police vehicle.", "crime_stolen_cop_car"); } }
        public static ScannerFile Defectivepolicevehicle { get { return new ScannerFile("01_crime_stolen_cop_car\\0x0FEBF849.wav", "Defective police vehicle.", "crime_stolen_cop_car"); } }
    }
    public class crime_stolen_helicopter { public static ScannerFile Astolenhelicopter { get { return new ScannerFile("01_crime_stolen_helicopter\\0x1D5AF1B0.wav", "A stolen helicopter.", "crime_stolen_helicopter"); } } }
    public class crime_stolen_vehicle { public static ScannerFile Apossiblestolenvehicle { get { return new ScannerFile("01_crime_stolen_vehicle\\0x09CEE854.wav", "A possible stolen vehicle.", "crime_stolen_vehicle"); } } }
    public class crime_structure_on_fire
    {
        public static ScannerFile Astructureonfire { get { return new ScannerFile("01_crime_structure_on_fire\\0x067D94A5.wav", "A structure on fire.", "crime_structure_on_fire"); } }
        public static ScannerFile Astructureonfire1 { get { return new ScannerFile("01_crime_structure_on_fire\\0x1E35841E.wav", "A structure on fire.", "crime_structure_on_fire"); } }
    }
    public class crime_suspect_armed_and_dangerous { public static ScannerFile Asuspectarmedanddangerous { get { return new ScannerFile("01_crime_suspect_armed_and_dangerous\\0x08E2DF46.wav", "A suspect...armed and dangerous.", "crime_suspect_armed_and_dangerous"); } } }
    public class crime_suspect_resisting_arrest { public static ScannerFile Asuspectresistingarrest { get { return new ScannerFile("01_crime_suspect_resisting_arrest\\0x04DE3185.wav", "A suspect resisting arrest.", "crime_suspect_resisting_arrest"); } } }
    public class crime_suspect_threatening_an_officer_with_a_firearm { public static ScannerFile Asuspectthreateninganofficerwithafirearm { get { return new ScannerFile("01_crime_suspect_threatening_an_officer_with_a_firearm\\0x05A09EE6.wav", "A suspect threatening an officer with a firearm.", "crime_suspect_threatening_an_officer_with_a_firearm"); } } }
    public class crime_suspicious_activity { public static ScannerFile Suspiciousactivity { get { return new ScannerFile("01_crime_suspicious_activity\\0x092DAC4A.wav", "Suspicious activity.", "crime_suspicious_activity"); } } }
    public class crime_suspicious_offshore_activity { public static ScannerFile Suspiciousoffshoreactivity { get { return new ScannerFile("01_crime_suspicious_offshore_activity\\0x1891C188.wav", "Suspicious offshore activity.", "crime_suspicious_offshore_activity"); } } }
    public class crime_suspicious_persons_loitering { public static ScannerFile Agroupofsuspiciouspersonsloitering { get { return new ScannerFile("01_crime_suspicious_persons_loitering\\0x14C10DE0.wav", "A group of suspicious persons loitering.", "crime_suspicious_persons_loitering"); } } }
    public class crime_suspicious_vehicle { public static ScannerFile Asuspiciousvehicle { get { return new ScannerFile("01_crime_suspicious_vehicle\\0x08C22EB1.wav", "A suspicious vehicle.", "crime_suspicious_vehicle"); } } }
    public class crime_terrorist_activity
    {
        public static ScannerFile Possibleterroristactivity { get { return new ScannerFile("01_crime_terrorist_activity\\0x01AD7615.wav", "Possible terrorist activity.", "crime_terrorist_activity"); } }
        public static ScannerFile Possibleterroristactivity1 { get { return new ScannerFile("01_crime_terrorist_activity\\0x0B7D09B8.wav", "Possible terrorist activity.", "crime_terrorist_activity"); } }
        public static ScannerFile Terroristactivity { get { return new ScannerFile("01_crime_terrorist_activity\\0x117ED5B8.wav", "Terrorist activity.", "crime_terrorist_activity"); } }
        public static ScannerFile Possibleterroristactivity2 { get { return new ScannerFile("01_crime_terrorist_activity\\0x1C8DABD5.wav", "Possible terrorist activity.", "crime_terrorist_activity"); } }
    }
    public class crime_theft { public static ScannerFile Apossibletheft { get { return new ScannerFile("01_crime_theft\\0x0536E1CD.wav", "A possible theft.", "crime_theft"); } } }
    public class crime_theft_of_an_aircraft { public static ScannerFile Theftofanaircraft { get { return new ScannerFile("01_crime_theft_of_an_aircraft\\0x045DD97F.wav", "Theft of an aircraft.", "crime_theft_of_an_aircraft"); } } }
    public class crime_torturing_an_animal { public static ScannerFile Persontorturingananimal { get { return new ScannerFile("01_crime_torturing_an_animal\\0x0BB248B5.wav", "Person torturing an animal.", "crime_torturing_an_animal"); } } }
    public class crime_traffic_alert { public static ScannerFile Atrafficalert { get { return new ScannerFile("01_crime_traffic_alert\\0x1C20369B.wav", "A traffic alert.", "crime_traffic_alert"); } } }
    public class crime_traffic_felony { public static ScannerFile Atrafficfelony { get { return new ScannerFile("01_crime_traffic_felony\\0x19F600A8.wav", "A traffic felony.", "crime_traffic_felony"); } } }
    public class crime_traffic_violation { public static ScannerFile Atrafficviolation { get { return new ScannerFile("01_crime_traffic_violation\\0x10D7F5BC.wav", "A traffic violation.", "crime_traffic_violation"); } } }
    public class crime_transporting_narcotics { public static ScannerFile Apersonorpersonstransportingnarcotics { get { return new ScannerFile("01_crime_transporting_narcotics\\0x0CA3A65C.wav", "A person or persons transporting narcotics.", "crime_transporting_narcotics"); } } }
    public class crime_trespassing
    {
        public static ScannerFile Trespassing { get { return new ScannerFile("01_crime_trespassing\\0x05C3CC3B.wav", "Trespassing.", "crime_trespassing"); } }
        public static ScannerFile Possibletrespassing { get { return new ScannerFile("01_crime_trespassing\\0x1071E197.wav", "Possible trespassing.", "crime_trespassing"); } }
    }
    public class crime_trespassing_on_government_property { public static ScannerFile Trespassingongovernmentproperty { get { return new ScannerFile("01_crime_trespassing_on_government_property\\0x11228580.wav", "Trespassing on government property.", "crime_trespassing_on_government_property"); } } }
    public class crime_unauthorized_hunting { public static ScannerFile Unauthorizedhunting { get { return new ScannerFile("01_crime_unauthorized_hunting\\0x0D93CE7F.wav", "Unauthorized hunting.", "crime_unauthorized_hunting"); } } }
    public class crime_unconscious_civilian { public static ScannerFile Anunconsciouscivilian { get { return new ScannerFile("01_crime_unconscious_civilian\\0x0839AE65.wav", "An unconscious civilian.", "crime_unconscious_civilian"); } } }
    public class crime_unconscious_female { public static ScannerFile Anunconsciousfemale { get { return new ScannerFile("01_crime_unconscious_female\\0x19D87B01.wav", "An unconscious female.", "crime_unconscious_female"); } } }
    public class crime_unconscious_male { public static ScannerFile Anunconsciousmale { get { return new ScannerFile("01_crime_unconscious_male\\0x106D54AF.wav", "An unconscious male.", "crime_unconscious_male"); } } }
    public class crime_unit_under_fire { public static ScannerFile Aunitunderfire { get { return new ScannerFile("01_crime_unit_under_fire\\0x1EC3B6A6.wav", "A unit under fire.", "crime_unit_under_fire"); } } }
    public class crime_vehicle_explosion { public static ScannerFile Avehicleexplosion { get { return new ScannerFile("01_crime_vehicle_explosion\\0x00ACF28E.wav", "A vehicle explosion.", "crime_vehicle_explosion"); } } }
    public class crime_vehicle_on_fire { public static ScannerFile Avehicleonfire { get { return new ScannerFile("01_crime_vehicle_on_fire\\0x0CF82D90.wav", "A vehicle on fire.", "crime_vehicle_on_fire"); } } }
    public class crime_vehicle_theft { public static ScannerFile Avehicletheft { get { return new ScannerFile("01_crime_vehicle_theft\\0x126934E4.wav", "A vehicle theft.", "crime_vehicle_theft"); } } }
    public class crime_vehicular_homicide { public static ScannerFile Avehicularhomicide { get { return new ScannerFile("01_crime_vehicular_homicide\\0x08687686.wav", "A vehicular homicide.", "crime_vehicular_homicide"); } } }
    public class crime_vessel_in_distress { public static ScannerFile Avesselindistress { get { return new ScannerFile("01_crime_vessel_in_distress\\0x0FE8B7D8.wav", "A vessel in distress.", "crime_vessel_in_distress"); } } }
    public class crime_vicious_animal
    {
        public static ScannerFile Aviciousanimalontheloose { get { return new ScannerFile("01_crime_vicious_animal\\0x00029C1B.wav", "A vicious animal on the loose.", "crime_vicious_animal"); } }
        public static ScannerFile Aviciousanimal { get { return new ScannerFile("01_crime_vicious_animal\\0x03A4A365.wav", "A vicious animal.", "crime_vicious_animal"); } }
    }
    public class crime_violation_of_a_non_kill_order { public static ScannerFile Violationofanonkillorder { get { return new ScannerFile("01_crime_violation_of_a_non_kill_order\\0x0FCF6888.wav", "Violation of a non-kill order.", "crime_violation_of_a_non_kill_order"); } } }
    public class crime_violation_of_a_no_kill_order
    {
        public static ScannerFile Violationofanokillmandate { get { return new ScannerFile("01_crime_violation_of_a_no_kill_order\\0x09EE350E.wav", "Violation of a no-kill mandate.", "crime_violation_of_a_no_kill_order"); } }
        public static ScannerFile Violationofanokillorder { get { return new ScannerFile("01_crime_violation_of_a_no_kill_order\\0x183F91B1.wav", "Violation of a no-kill order.", "crime_violation_of_a_no_kill_order"); } }
    }
    public class crime_wanted_felon_on_the_loose { public static ScannerFile Awantedfelonontheloose { get { return new ScannerFile("01_crime_wanted_felon_on_the_loose\\0x17CA8289.wav", "A wanted felon on the loose.", "crime_wanted_felon_on_the_loose"); } } }
    public class crime_warrant_issued { public static ScannerFile Awarrantissued { get { return new ScannerFile("01_crime_warrant_issued\\0x09926678.wav", "A warrant issued.", "crime_warrant_issued"); } } }
    public class crooks_arrested
    {
        public static ScannerFile Multiplesuspectsarrested { get { return new ScannerFile("01_crooks_arrested\\0x03169622.wav", "Multiple suspects arrested.", "crooks_arrested"); } }
        public static ScannerFile Officershaveapprehendedallsuspects { get { return new ScannerFile("01_crooks_arrested\\0x07A25F39.wav", "Officers have apprehended all suspects.", "crooks_arrested"); } }
        public static ScannerFile Suspectsincustody { get { return new ScannerFile("01_crooks_arrested\\0x0C75E8E3.wav", "Suspects in custody.", "crooks_arrested"); } }
        public static ScannerFile Officershaveapprehendedmultiplesuspects { get { return new ScannerFile("01_crooks_arrested\\0x10166FB6.wav", "Officers have apprehended multiple suspects.", "crooks_arrested"); } }
        public static ScannerFile Unitshavesuspectsincustody { get { return new ScannerFile("01_crooks_arrested\\0x139936BB.wav", "Units have suspects in custody.", "crooks_arrested"); } }
        public static ScannerFile Suspectsincustody1 { get { return new ScannerFile("01_crooks_arrested\\0x151DBA30.wav", "Suspects in custody.", "crooks_arrested"); } }
        public static ScannerFile Multiplesuspectsincustody { get { return new ScannerFile("01_crooks_arrested\\0x19D883A9.wav", "Multiple suspects in custody.", "crooks_arrested"); } }
        public static ScannerFile Suspectsare1015 { get { return new ScannerFile("01_crooks_arrested\\0x1CFC0672.wav", "Suspects are 10-15.", "crooks_arrested"); } }
        public static ScannerFile Unitshaveapprehendedsuspects { get { return new ScannerFile("01_crooks_arrested\\0x1E69CC5D.wav", "Units have apprehended suspects.", "crooks_arrested"); } }
        public static ScannerFile Suspectsapprehended { get { return new ScannerFile("01_crooks_arrested\\0x1EC68D84.wav", "Suspects apprehended.", "crooks_arrested"); } }
    }
    public class crooks_escaped
    {
        public static ScannerFile Suspectshaveevadedpursuit { get { return new ScannerFile("01_crooks_escaped\\0x062BA4E6.wav", "Suspects have evaded pursuit.", "crooks_escaped"); } }
        public static ScannerFile Suspectsevadedofficers { get { return new ScannerFile("01_crooks_escaped\\0x0B21EED1.wav", "Suspects evaded officers.", "crooks_escaped"); } }
        public static ScannerFile Targetshaveevadedcapture { get { return new ScannerFile("01_crooks_escaped\\0x11987BBF.wav", "Targets have evaded capture.", "crooks_escaped"); } }
        public static ScannerFile Criminalshaveevadedpursuit { get { return new ScannerFile("01_crooks_escaped\\0x14430115.wav", "Criminals have evaded pursuit.", "crooks_escaped"); } }
        public static ScannerFile Targetsevadedpursuingofficers { get { return new ScannerFile("01_crooks_escaped\\0x18F84A7E.wav", "Targets evaded pursuing officers.", "crooks_escaped"); } }
        public static ScannerFile Suspectslost { get { return new ScannerFile("01_crooks_escaped\\0x1FAF17EC.wav", "Suspects lost.", "crooks_escaped"); } }
        public static ScannerFile Suspectshaveeludedofficers { get { return new ScannerFile("01_crooks_escaped\\0x1FE8D860.wav", "Suspects have eluded officers.", "crooks_escaped"); } }
    }
    public class crooks_killed
    {
        public static ScannerFile Unitshaveneutralizedsuspects { get { return new ScannerFile("01_crooks_killed\\0x065E8858.wav", "Units have neutralized suspects.", "crooks_killed"); } }
        public static ScannerFile Suspectsneutralized { get { return new ScannerFile("01_crooks_killed\\0x083B4D1C.wav", "Suspects neutralized.", "crooks_killed"); } }
        public static ScannerFile Suspectsdown { get { return new ScannerFile("01_crooks_killed\\0x0F5A9A4F.wav", "Suspects down.", "crooks_killed"); } }
        public static ScannerFile Officershavepacifiedsuspects { get { return new ScannerFile("01_crooks_killed\\0x11ADDEF5.wav", "Officers have pacified suspects.", "crooks_killed"); } }
        public static ScannerFile Suspectspacified { get { return new ScannerFile("01_crooks_killed\\0x11FE60A2.wav", "Suspects pacified.", "crooks_killed"); } }
        public static ScannerFile Unitshavepacifiedsuspects { get { return new ScannerFile("01_crooks_killed\\0x1BDB3351.wav", "Units have pacified suspects.", "crooks_killed"); } }
        public static ScannerFile Officershaveneutralizedsuspects { get { return new ScannerFile("01_crooks_killed\\0x1F707A7A.wav", "Officers have neutralized suspects.", "crooks_killed"); } }
    }
    public class crook_arrested
    {
        public static ScannerFile Asuspectincustody { get { return new ScannerFile("01_crook_arrested\\0x02B3621C.wav", "A suspect in custody.", "crook_arrested"); } }
        public static ScannerFile Suspectis1015 { get { return new ScannerFile("01_crook_arrested\\0x04D9A179.wav", "Suspect is 10-15.", "crook_arrested"); } }
        public static ScannerFile Officershaveapprehendedsuspect { get { return new ScannerFile("01_crook_arrested\\0x0829E81B.wav", "Officers have apprehended suspect.", "crook_arrested"); } }
        public static ScannerFile Suspectplaceunderarrest { get { return new ScannerFile("01_crook_arrested\\0x0A696B9D.wav", "Suspect place under arrest.", "crook_arrested"); } }
        public static ScannerFile Asuspectarrested { get { return new ScannerFile("01_crook_arrested\\0x0BC9F44A.wav", "A suspect arrested.", "crook_arrested"); } }
        public static ScannerFile Suspectapprehended { get { return new ScannerFile("01_crook_arrested\\0x0CC1715C.wav", "Suspect apprehended.", "crook_arrested"); } }
        public static ScannerFile Suspectincustody { get { return new ScannerFile("01_crook_arrested\\0x0DC8B356.wav", "Suspect in custody.", "crook_arrested"); } }
        public static ScannerFile suspectincustody1015 { get { return new ScannerFile("01_crook_arrested\\0x11D3FB6C.wav", "10-15; suspect in custody.", "crook_arrested"); } }
        public static ScannerFile Asuspectapprehended { get { return new ScannerFile("01_crook_arrested\\0x13C98449.wav", "A suspect apprehended.", "crook_arrested"); } }
        public static ScannerFile Officershaveapprehendedsuspect1 { get { return new ScannerFile("01_crook_arrested\\0x170FC5E7.wav", "Officers have apprehended suspect.", "crook_arrested"); } }
        public static ScannerFile Unitshavesuspectincustody { get { return new ScannerFile("01_crook_arrested\\0x171345EC.wav", "Units have suspect in custody.", "crook_arrested"); } }
        public static ScannerFile Asuspectincustody1 { get { return new ScannerFile("01_crook_arrested\\0x1729CB0B.wav", "A suspect in custody.", "crook_arrested"); } }
        public static ScannerFile suspectapprehended1015 { get { return new ScannerFile("01_crook_arrested\\0x17BB4641.wav", "10-15; suspect apprehended.", "crook_arrested"); } }
        public static ScannerFile Asuspect10151 { get { return new ScannerFile("01_crook_arrested\\0x18EE4E92.wav", "A suspect 10-15.", "crook_arrested"); } }
        public static ScannerFile Suspectarrested1 { get { return new ScannerFile("01_crook_arrested\\0x1A818CC8.wav", "Suspect arrested.", "crook_arrested"); } }
        public static ScannerFile Suspectapprehended1 { get { return new ScannerFile("01_crook_arrested\\0x1B044DE2.wav", "Suspect apprehended.", "crook_arrested"); } }
        public static ScannerFile Asuspectplacedunderarrest { get { return new ScannerFile("01_crook_arrested\\0x1D74179F.wav", "A suspect placed under arrest.", "crook_arrested"); } }
    }
    public class crook_killed
    {
        public static ScannerFile Suspectdown { get { return new ScannerFile("01_crook_killed\\0x0269B41C.wav", "Suspect down.", "crook_killed"); } }
        public static ScannerFile Asuspectdown { get { return new ScannerFile("01_crook_killed\\0x03FCE3D3.wav", "A suspect down.", "crook_killed"); } }
        public static ScannerFile Suspectdowncoronerenroute { get { return new ScannerFile("01_crook_killed\\0x064FBBE9.wav", "Suspect down; coroner en route.", "crook_killed"); } }
        public static ScannerFile Asuspectdown1 { get { return new ScannerFile("01_crook_killed\\0x0A22B024.wav", "A suspect down.", "crook_killed"); } }
        public static ScannerFile Criminaldown { get { return new ScannerFile("01_crook_killed\\0x0B868657.wav", "Criminal down.", "crook_killed"); } }
        public static ScannerFile Asuspectdown2 { get { return new ScannerFile("01_crook_killed\\0x0D96770A.wav", "A suspect down.", "crook_killed"); } }
        public static ScannerFile Suspectneutralized { get { return new ScannerFile("01_crook_killed\\0x10088F5D.wav", "Suspect neutralized.", "crook_killed"); } }
        public static ScannerFile Suspectdownmedicalexaminerenroute { get { return new ScannerFile("01_crook_killed\\0x14FC5942.wav", "Suspect down; medical examiner en route.", "crook_killed"); } }
        public static ScannerFile Acriminaldown { get { return new ScannerFile("01_crook_killed\\0x1C44D467.wav", "A criminal down.", "crook_killed"); } }
        public static ScannerFile Officershavepacifiedsuspect { get { return new ScannerFile("01_crook_killed\\0x1E57EBF8.wav", "Officers have pacified suspect.", "crook_killed"); } }
    }
    public class custom_player_flow
    {
        public static ScannerFile WantedinconnectionwiththeRichmanJewelryStoreHeist { get { return new ScannerFile("01_custom_player_flow\\0x005719B7.wav", "Wanted in connection with theRichman Jewelry Store Heist.", "custom_player_flow"); } }
        public static ScannerFile WantedinconnectionwiththeRobberyofthePaletoBayBank { get { return new ScannerFile("01_custom_player_flow\\0x018DF534.wav", "Wanted in connection with theRobbery of the Paleto Bay Bank.", "custom_player_flow"); } }
        public static ScannerFile OfficersreportsightingofTrevorPhilips { get { return new ScannerFile("01_custom_player_flow\\0x01A271E2.wav", "Officers report sighting of Trevor Philips.", "custom_player_flow"); } }
        public static ScannerFile SuspectisFranklinClinton { get { return new ScannerFile("01_custom_player_flow\\0x02D9EA4C.wav", "Suspect is Franklin Clinton.", "custom_player_flow"); } }
        public static ScannerFile WantedforquestioningabouttheHumaneLabsRobbery { get { return new ScannerFile("01_custom_player_flow\\0x076EFD5F.wav", "Wanted for questioning about theHumane Labs Robbery.", "custom_player_flow"); } }
        public static ScannerFile FornumerousviolationsinthemiddleLosSantosarea { get { return new ScannerFile("01_custom_player_flow\\0x079A4471.wav", "For numerous violations in the middle(?) Los Santos area.", "custom_player_flow"); } }
        public static ScannerFile Attentionallunitssuspectisamiddleagedwhitemale { get { return new ScannerFile("01_custom_player_flow\\0x0C8FA2D3.wav", "Attention all units, suspect is a middle-aged white male.", "custom_player_flow"); } }
        public static ScannerFile Suspectsarereportedtobethegroupwanted { get { return new ScannerFile("01_custom_player_flow\\0x10D916E2.wav", "Suspects are reported to be the group wanted.", "custom_player_flow"); } }
        public static ScannerFile Wantedinconnectionwithmultiplehomicides { get { return new ScannerFile("01_custom_player_flow\\0x119211C1.wav", "Wanted in connection with multiple homicides.", "custom_player_flow"); } }
        public static ScannerFile WantedforquestioningabouttheHumaneLabsRobbery1 { get { return new ScannerFile("01_custom_player_flow\\0x129809C9.wav", "Wanted for questioning about theHumane Labs Robbery.", "custom_player_flow"); } }
        public static ScannerFile WantedinconnectionwiththeRichmanJewelryStoreHeist1 { get { return new ScannerFile("01_custom_player_flow\\0x1343B038.wav", "Wanted in connection with theRichman Jewelry Store Heist.", "custom_player_flow"); } }
        public static ScannerFile Suspectisayoungafricanamericanmale { get { return new ScannerFile("01_custom_player_flow\\0x15BC99FA.wav", "Suspect is a young, african-american male.", "custom_player_flow"); } }
        public static ScannerFile Officersreportsightingofaheavysetwhitemale { get { return new ScannerFile("01_custom_player_flow\\0x17EE21F7.wav", "Officers report sighting of a heavyset white male.", "custom_player_flow"); } }
        public static ScannerFile AttentionallunitssuspectissupportedasMichaeldeSanto { get { return new ScannerFile("01_custom_player_flow\\0x1B360F7C.wav", "Attention all units, suspect is supported as Michael de Santo.", "custom_player_flow"); } }
    }
    public class custom_wanted_level_line
    {
        public static ScannerFile OfficersauthorizedtousetasersCodeTom { get { return new ScannerFile("01_custom_wanted_level_line\\0x00381DCD.wav", "Officers authorized to use tasers; Code Tom.", "custom_wanted_level_line"); } }
        public static ScannerFile Officerdownairsupportenroute { get { return new ScannerFile("01_custom_wanted_level_line\\0x02484600.wav", "Officer down; air support en route.", "custom_wanted_level_line"); } }
        public static ScannerFile Officerdownsituationiscode99 { get { return new ScannerFile("01_custom_wanted_level_line\\0x05180BA0.wav", "Officer down; situation is code 99.", "custom_wanted_level_line"); } }
        public static ScannerFile Code99officersrequireimmediateassistance { get { return new ScannerFile("01_custom_wanted_level_line\\0x0849C003.wav", "Code 99; officers require immediate assistance.", "custom_wanted_level_line"); } }
        public static ScannerFile HelicopterdownallDavidunitsonalert { get { return new ScannerFile("01_custom_wanted_level_line\\0x08B4DF2A.wav", "Helicopter down, all David units on alert.", "custom_wanted_level_line"); } }
        public static ScannerFile Officersauthorizedtousetasers { get { return new ScannerFile("01_custom_wanted_level_line\\0x0E02B962.wav", "Officers authorized to use tasers.", "custom_wanted_level_line"); } }
        public static ScannerFile Shotsfiredatanofficeruseoflethalforceisauthorized { get { return new ScannerFile("01_custom_wanted_level_line\\0x0EAD1ECA.wav", "Shots fired at an officer, use of lethal force is authorized.", "custom_wanted_level_line"); } }
        public static ScannerFile Officersdownairsupportenroute { get { return new ScannerFile("01_custom_wanted_level_line\\0x1107E37F.wav", "Officers down; air support en route.", "custom_wanted_level_line"); } }
        public static ScannerFile Wehavea1099allavailableunitsrespond { get { return new ScannerFile("01_custom_wanted_level_line\\0x12A7D458.wav", "We have a 10-99, all available units respond.", "custom_wanted_level_line"); } }
        public static ScannerFile Code99allavailableunitsconvergeonsuspect { get { return new ScannerFile("01_custom_wanted_level_line\\0x14577671.wav", "Code 99; all available units converge on suspect.", "custom_wanted_level_line"); } }
        public static ScannerFile Apprehendsuspectwithoutexcessiveforce { get { return new ScannerFile("01_custom_wanted_level_line\\0x1487C663.wav", "Apprehend suspect without excessive force.", "custom_wanted_level_line"); } }
        public static ScannerFile Apprehendsuspectandreturntothestationforquestioning { get { return new ScannerFile("01_custom_wanted_level_line\\0x14C346E4.wav", "Apprehend suspect and return to the station for questioning.", "custom_wanted_level_line"); } }
        public static ScannerFile HelicopterdownallavailableDavidunitsonalert { get { return new ScannerFile("01_custom_wanted_level_line\\0x196A0098.wav", "Helicopter down, all available David units on alert.", "custom_wanted_level_line"); } }
        public static ScannerFile Suspectisarmedanddangerousweaponsfree { get { return new ScannerFile("01_custom_wanted_level_line\\0x1B146598.wav", "Suspect is armed and dangerous; weapons free.", "custom_wanted_level_line"); } }
        public static ScannerFile Code13militaryunitsrequested { get { return new ScannerFile("01_custom_wanted_level_line\\0x1E4F9A87.wav", "Code 13; military units requested.", "custom_wanted_level_line"); } }
    }
    public class direction_bound
    {
        public static ScannerFile Southbound { get { return new ScannerFile("01_direction_bound\\0x02F43627.wav", "Southbound.", "direction_bound"); } }
        public static ScannerFile Westbound { get { return new ScannerFile("01_direction_bound\\0x089A1D9B.wav", "Westbound.", "direction_bound"); } }
        public static ScannerFile Northbound { get { return new ScannerFile("01_direction_bound\\0x0F00AAE2.wav", "Northbound.", "direction_bound"); } }
        public static ScannerFile Eastbound { get { return new ScannerFile("01_direction_bound\\0x15D27B32.wav", "Eastbound.", "direction_bound"); } }
    }
    public class direction_heading
    {
        public static ScannerFile South { get { return new ScannerFile("01_direction_heading\\0x0BB197C4.wav", "South.", "direction_heading"); } }
        public static ScannerFile East { get { return new ScannerFile("01_direction_heading\\0x0E70541A.wav", "East.", "direction_heading"); } }
        public static ScannerFile North { get { return new ScannerFile("01_direction_heading\\0x168FB5D8.wav", "North.", "direction_heading"); } }
        public static ScannerFile West { get { return new ScannerFile("01_direction_heading\\0x1E1242F0.wav", "West.", "direction_heading"); } }
    }
    public class dispatch_custom_field_report_response
    {

        public static ScannerFile copythat104 { get { return new ScannerFile("01_dispatch_custom_field_report_response\\0x01A472D4.wav", "10-4, copy that.", "dispatch_custom_field_report_response"); } }
        public static ScannerFile Copythat1041 { get { return new ScannerFile("01_dispatch_custom_field_report_response\\0x06283BDB.wav", "Copy that, 10-4.", "dispatch_custom_field_report_response"); } }
        public static ScannerFile Rogerthat { get { return new ScannerFile("01_dispatch_custom_field_report_response\\0x0DD40B1E.wav", "Roger that.", "dispatch_custom_field_report_response"); } }
        public static ScannerFile Acknowledged { get { return new ScannerFile("01_dispatch_custom_field_report_response\\0x1356D639.wav", "Acknowledged.", "dispatch_custom_field_report_response"); } }
        public static ScannerFile Number104 { get { return new ScannerFile("01_dispatch_custom_field_report_response\\0x1460D84C.wav", "10-4.", "dispatch_custom_field_report_response"); } }
        public static ScannerFile Number104copy { get { return new ScannerFile("01_dispatch_custom_field_report_response\\0x17D59F36.wav", "10-4, copy.", "dispatch_custom_field_report_response"); } }
        public static ScannerFile Roger { get { return new ScannerFile("01_dispatch_custom_field_report_response\\0x1E802C76.wav", "Roger.", "dispatch_custom_field_report_response"); } }

    }
    public class dispatch_respond_code
    {
        public static ScannerFile UnitsrespondCode2 { get { return new ScannerFile("01_dispatch_respond_code\\0x00B0B158.wav", "Units respond; Code 2.", "dispatch_respond_code"); } }
        public static ScannerFile AllunitsrespondCode99 { get { return new ScannerFile("01_dispatch_respond_code\\0x067E6E48.wav", "All units respond; Code 99.", "dispatch_respond_code"); } }
        public static ScannerFile Code99allunitsrespond { get { return new ScannerFile("01_dispatch_respond_code\\0x09BEB4C8.wav", "Code 99; all units respond.", "dispatch_respond_code"); } }
        public static ScannerFile RespondCode2 { get { return new ScannerFile("01_dispatch_respond_code\\0x0B6786C0.wav", "Respond; Code 2.", "dispatch_respond_code"); } }
        public static ScannerFile UnitsrespondCode3 { get { return new ScannerFile("01_dispatch_respond_code\\0x0DD15A9F.wav", "Units respond; Code 3.", "dispatch_respond_code"); } }
        public static ScannerFile RespondCode3 { get { return new ScannerFile("01_dispatch_respond_code\\0x0E9C9C35.wav", "Respond; Code 3.", "dispatch_respond_code"); } }
        public static ScannerFile AllunitsrespondCode99emergency { get { return new ScannerFile("01_dispatch_respond_code\\0x113C43C4.wav", "All units respond; Code 99 emergency.", "dispatch_respond_code"); } }
        public static ScannerFile UnitrespondCode3 { get { return new ScannerFile("01_dispatch_respond_code\\0x1917312B.wav", "Unit respond; Code 3.", "dispatch_respond_code"); } }
        public static ScannerFile EmergencyallunitsrespondCode99 { get { return new ScannerFile("01_dispatch_respond_code\\0x1B01574D.wav", "Emergency; all units respond Code 99.", "dispatch_respond_code"); } }
        public static ScannerFile UnitrespondCode2 { get { return new ScannerFile("01_dispatch_respond_code\\0x1CB5A95C.wav", "Unit respond; Code 2.", "dispatch_respond_code"); } }
    }
    public class dispatch_to
    {
        public static ScannerFile Dispatchtouhh { get { return new ScannerFile("01_dispatch_to\\0x04BCE5D5.wav", "Dispatch to, uhh...", "dispatch_to"); } }
        public static ScannerFile Dispatchtoumm { get { return new ScannerFile("01_dispatch_to\\0x14FE4658.wav", "Dispatch to, umm...", "dispatch_to"); } }
    }
    public class dispatch_units_from
    {
        public static ScannerFile Dispatchunitsfromuh { get { return new ScannerFile("01_dispatch_units_from\\0x08A4112E.wav", "Dispatch units from uh...", "dispatch_units_from"); } }
        public static ScannerFile Dispatchunitsfrom { get { return new ScannerFile("01_dispatch_units_from\\0x0B7756D4.wav", "Dispatch units from...", "dispatch_units_from"); } }
        public static ScannerFile Dispatchunitsfromumm { get { return new ScannerFile("01_dispatch_units_from\\0x1668ECB8.wav", "Dispatch units from...umm...", "dispatch_units_from"); } }
        public static ScannerFile Dispatchunitsfromumm1 { get { return new ScannerFile("01_dispatch_units_from\\0x175AEE96.wav", "Dispatch units from...umm...", "dispatch_units_from"); } }
    }
    public class dispatch_units_full
    {
        public static ScannerFile DispatchingSWATunitsfrompoliceheadquarters { get { return new ScannerFile("01_dispatch_units_full\\0x011432C4.wav", "Dispatching SWAT units from police headquarters.", "dispatch_units_full"); } }
        public static ScannerFile DispatchunitsfromMirrorParkPoliceStation { get { return new ScannerFile("01_dispatch_units_full\\0x01E4F3CE.wav", "Dispatch units fromMirror Park Police Station.", "dispatch_units_full"); } }
        public static ScannerFile ScramblingmilitaryaircraftfromKazanskyAirForceBase { get { return new ScannerFile("01_dispatch_units_full\\0x06AFD53C.wav", "Scrambling military aircraft from Kazansky Air-Force Base.", "dispatch_units_full"); } }
        public static ScannerFile DispatchunitsfromMissionRowPoliceDepartment { get { return new ScannerFile("01_dispatch_units_full\\0x087C8D8D.wav", "Dispatch units fromMission Row Police Department.", "dispatch_units_full"); } }
        public static ScannerFile DispatchingFIBinvestigators { get { return new ScannerFile("01_dispatch_units_full\\0x0AD31361.wav", "Dispatching FIB investigators.", "dispatch_units_full"); } }
        public static ScannerFile Dispatchairunitfromcentralstation { get { return new ScannerFile("01_dispatch_units_full\\0x0B24363A.wav", "Dispatch air unit from central station.", "dispatch_units_full"); } }
        public static ScannerFile Dispatchallavailableunitsfromdowntownarea { get { return new ScannerFile("01_dispatch_units_full\\0x0BA92B59.wav", "Dispatch all available units from downtown area.", "dispatch_units_full"); } }
        public static ScannerFile DispatchingSWATunitsfrompoliceheadquarters1 { get { return new ScannerFile("01_dispatch_units_full\\0x10569149.wav", "Dispatching SWAT units from police headquarters.", "dispatch_units_full"); } }
        public static ScannerFile DispatchunitsfromVespucciBeachPD { get { return new ScannerFile("01_dispatch_units_full\\0x14000A1A.wav", "Dispatch units from Vespucci Beach PD.", "dispatch_units_full"); } }
        public static ScannerFile Dispatchingairunitfromcentralstation { get { return new ScannerFile("01_dispatch_units_full\\0x14B7C962.wav", "Dispatching air unit from central station.", "dispatch_units_full"); } }
        public static ScannerFile Dispatchingairunitfromcentralstation1 { get { return new ScannerFile("01_dispatch_units_full\\0x16310C54.wav", "Dispatching air unit from central station.", "dispatch_units_full"); } }
        public static ScannerFile Dispatchallavailableunitsfromthedowntownarea { get { return new ScannerFile("01_dispatch_units_full\\0x1887454D.wav", "Dispatch all available units from the downtown area.", "dispatch_units_full"); } }
        public static ScannerFile Dispatchunitsfromdowntownprecinct { get { return new ScannerFile("01_dispatch_units_full\\0x196806D7.wav", "Dispatch units from downtown precinct.", "dispatch_units_full"); } }
        public static ScannerFile DispatchingunitsfromKazanskyAirForceBase { get { return new ScannerFile("01_dispatch_units_full\\0x19C1FB60.wav", "Dispatching units from Kazansky Air-Force Base.", "dispatch_units_full"); } }
        public static ScannerFile FIBteamdispatchingfromstation { get { return new ScannerFile("01_dispatch_units_full\\0x1D08B7CC.wav", "FIB team dispatching from station.", "dispatch_units_full"); } }
    }
    public class doing_speed
    {
        public static ScannerFile Doing100mph { get { return new ScannerFile("01_doing_speed\\0x082AF3B3.wav", "Doing 100 mph.", "doing_speed"); } }
        public static ScannerFile Doing70mph { get { return new ScannerFile("01_doing_speed\\0x0EB2AEE4.wav", "Doing 70 mph.", "doing_speed"); } }
        public static ScannerFile Doingover100mph { get { return new ScannerFile("01_doing_speed\\0x10BF0162.wav", "Doing over 100 mph.", "doing_speed"); } }
        public static ScannerFile Doing50mph { get { return new ScannerFile("01_doing_speed\\0x13559E06.wav", "Doing 50 mph.", "doing_speed"); } }
        public static ScannerFile Doing60mph { get { return new ScannerFile("01_doing_speed\\0x18545449.wav", "Doing 60 mph.", "doing_speed"); } }
        public static ScannerFile Doing80mph { get { return new ScannerFile("01_doing_speed\\0x1A1E8187.wav", "Doing 80 mph.", "doing_speed"); } }
        public static ScannerFile Doing40mph { get { return new ScannerFile("01_doing_speed\\0x1CCDF2A2.wav", "Doing 40 mph.", "doing_speed"); } }
        public static ScannerFile Doing90mph { get { return new ScannerFile("01_doing_speed\\0x1F166F61.wav", "Doing 90 mph.", "doing_speed"); } }
    }
    public class emergency
    {
        public static ScannerFile Apossiblefire { get { return new ScannerFile("01_emergency\\0x006667C2.wav", "A possible fire.", "emergency"); } }
        public static ScannerFile ApossibleAnthraxexposure { get { return new ScannerFile("01_emergency\\0x0173E77A.wav", "A possible Anthrax exposure.", "emergency"); } }
        public static ScannerFile ApossibleEbolaexposure { get { return new ScannerFile("01_emergency\\0x02FFD687.wav", "A possible Ebola exposure.", "emergency"); } }
        public static ScannerFile Anapartmentfire { get { return new ScannerFile("01_emergency\\0x03E3FED7.wav", "An apartment fire.", "emergency"); } }
        public static ScannerFile Athreealarmfire { get { return new ScannerFile("01_emergency\\0x0518696C.wav", "A three-alarm fire.", "emergency"); } }
        public static ScannerFile Apossibleapartmentfire { get { return new ScannerFile("01_emergency\\0x058BC227.wav", "A possible apartment fire.", "emergency"); } }
        public static ScannerFile Amedicalemergency { get { return new ScannerFile("01_emergency\\0x082543D6.wav", "A medical emergency.", "emergency"); } }
        public static ScannerFile Anapartmentfire1 { get { return new ScannerFile("01_emergency\\0x0A00CB0F.wav", "An apartment fire.", "emergency"); } }
        public static ScannerFile Apossiblecondofire { get { return new ScannerFile("01_emergency\\0x0BACA760.wav", "A possible condo fire.", "emergency"); } }
        public static ScannerFile ApossibleAnthraxrelease { get { return new ScannerFile("01_emergency\\0x0C65BD68.wav", "A possible Anthrax release.", "emergency"); } }
        public static ScannerFile Afire { get { return new ScannerFile("01_emergency\\0x0DDC42AE.wav", "A fire.", "emergency"); } }
        public static ScannerFile Apossibleburnvictim { get { return new ScannerFile("01_emergency\\0x0F1FA623.wav", "A possible burn victim.", "emergency"); } }
        public static ScannerFile AcardiacarrestatBurgerShot { get { return new ScannerFile("01_emergency\\0x0F9F9E46.wav", "A cardiac arrest at Burger Shot.", "emergency"); } }
        public static ScannerFile Aburnvictim { get { return new ScannerFile("01_emergency\\0x0FDF27A4.wav", "A burn victim.", "emergency"); } }
        public static ScannerFile Apossiblecaseoftuberculosis { get { return new ScannerFile("01_emergency\\0x0FE599E5.wav", "A possible case of tuberculosis.", "emergency"); } }
        public static ScannerFile Aburningbuilding { get { return new ScannerFile("01_emergency\\0x0FE6D76A.wav", "A burning building.", "emergency"); } }
        public static ScannerFile ApossibleEbolarelease { get { return new ScannerFile("01_emergency\\0x10F43270.wav", "A possible Ebola release.", "emergency"); } }
        public static ScannerFile Anapartmentcomplexfire { get { return new ScannerFile("01_emergency\\0x1131D970.wav", "An apartment complex fire.", "emergency"); } }
        public static ScannerFile ApersonwithchestpainsatBurgerShot { get { return new ScannerFile("01_emergency\\0x115C61C0.wav", "A person with chest pains at Burger Shot.", "emergency"); } }
        public static ScannerFile Abuildingonfire { get { return new ScannerFile("01_emergency\\0x121C1BD8.wav", "A building on fire.", "emergency"); } }
        public static ScannerFile Apossiblehousefire { get { return new ScannerFile("01_emergency\\0x15619DA2.wav", "A possible house fire.", "emergency"); } }
        public static ScannerFile PossiblecaseofEbola { get { return new ScannerFile("01_emergency\\0x157B7B7B.wav", "Possible case of Ebola.", "emergency"); } }
        public static ScannerFile Ahousefire { get { return new ScannerFile("01_emergency\\0x169EA01C.wav", "A house fire.", "emergency"); } }
        public static ScannerFile Apossiblemedicalemergency { get { return new ScannerFile("01_emergency\\0x188B24A1.wav", "A possible medical emergency.", "emergency"); } }
        public static ScannerFile Acondofire { get { return new ScannerFile("01_emergency\\0x19EB03DD.wav", "A condo fire.", "emergency"); } }
        public static ScannerFile AcontaminantreleasepossiblyEbola { get { return new ScannerFile("01_emergency\\0x1B05068F.wav", "A contaminant release, possibly Ebola.", "emergency"); } }
        public static ScannerFile Afivealarmfire { get { return new ScannerFile("01_emergency\\0x1C3C46C1.wav", "A five-alarm fire.", "emergency"); } }
        public static ScannerFile Apossiblebuildingonfire { get { return new ScannerFile("01_emergency\\0x1C4EF03C.wav", "A possible building on fire.", "emergency"); } }
        public static ScannerFile Anapartmentonfire { get { return new ScannerFile("01_emergency\\0x1E8EF42A.wav", "An apartment on fire.", "emergency"); } }
    }
    public class escort_boss
    {
        public static ScannerFile WehaveaCode99 { get { return new ScannerFile("01_escort_boss\\0x009F1452.wav", "We have a Code 99.", "escort_boss"); } }
        public static ScannerFile Allunitsprimarysuspectisincustody { get { return new ScannerFile("01_escort_boss\\0x01CCC705.wav", "All units, primary suspect is in custody.", "escort_boss"); } }
        public static ScannerFile Primarysuspecthasbeendetainedatprecinct { get { return new ScannerFile("01_escort_boss\\0x021FCAA6.wav", "Primary suspect has been detained at precinct.", "escort_boss"); } }
        public static ScannerFile Primarysuspecthasarrivedattheprecinct { get { return new ScannerFile("01_escort_boss\\0x026DB302.wav", "Primary suspect has arrived at the precinct.", "escort_boss"); } }
        public static ScannerFile Primarysuspecthasbeendetainedattheprecinct { get { return new ScannerFile("01_escort_boss\\0x04EEE6A8.wav", "Primary suspect has been detained at the precinct.", "escort_boss"); } }
        public static ScannerFile Officersrequestingimmediateescorttoprecinct { get { return new ScannerFile("01_escort_boss\\0x050DFFB4.wav", "Officers requesting immediate escort to precinct.", "escort_boss"); } }
        public static ScannerFile Primarysuspecthasarrivedattheprecinct1 { get { return new ScannerFile("01_escort_boss\\0x055E6B9E.wav", "Primary suspect has arrived at the precinct.", "escort_boss"); } }
        public static ScannerFile Immediateassistancerequired { get { return new ScannerFile("01_escort_boss\\0x07652595.wav", "Immediate assistance required.", "escort_boss"); } }
        public static ScannerFile Allunitsmainsuspectis1014 { get { return new ScannerFile("01_escort_boss\\0x07BD2B7B.wav", "All units, main suspect is 10-14.", "escort_boss"); } }
        public static ScannerFile AllunitsCode99 { get { return new ScannerFile("01_escort_boss\\0x0987A9E6.wav", "All units; Code 99.", "escort_boss"); } }
        public static ScannerFile SuspectisenroutetoCentralLosSantos { get { return new ScannerFile("01_escort_boss\\0x0C0DACFB.wav", "Suspect is en route to Central Los Santos.", "escort_boss"); } }
        public static ScannerFile WehavereportsofahighlevelVagosgangmemberintransit { get { return new ScannerFile("01_escort_boss\\0x0D17473D.wav", "We have reports of a high-level Vagos gang member in transit.", "escort_boss"); } }
        public static ScannerFile Allunitsprovidedimmediatebackup { get { return new ScannerFile("01_escort_boss\\0x0D276D5F.wav", "All units provided immediate backup.", "escort_boss"); } }
        public static ScannerFile SuspectisenroutetoCentralLosSantos1 { get { return new ScannerFile("01_escort_boss\\0x0D9A700E.wav", "Suspect is en route to Central Los Santos.", "escort_boss"); } }
        public static ScannerFile WehavereportsofahighlevelLostgangmemberintransit { get { return new ScannerFile("01_escort_boss\\0x0E9E5B46.wav", "We have reports of a high-level Lost gang member in transit.", "escort_boss"); } }
        public static ScannerFile Suspecttransportunderattack { get { return new ScannerFile("01_escort_boss\\0x0F74B1FD.wav", "Suspect transport under attack.", "escort_boss"); } }
        public static ScannerFile Allunitsmainsuspectisa1014 { get { return new ScannerFile("01_escort_boss\\0x12385A09.wav", "All units, main suspect is a 10-14.", "escort_boss"); } }
        public static ScannerFile Allunitsstanddown { get { return new ScannerFile("01_escort_boss\\0x12A2020F.wav", "All units stand down.", "escort_boss"); } }
        public static ScannerFile AllunitsCode4 { get { return new ScannerFile("01_escort_boss\\0x12CC867A.wav", "All units; Code 4.", "escort_boss"); } }
        public static ScannerFile Suspectsareheavilyarmed { get { return new ScannerFile("01_escort_boss\\0x158FFB90.wav", "Suspects are heavily armed.", "escort_boss"); } }
        public static ScannerFile Officersrequireescorttoprecinct { get { return new ScannerFile("01_escort_boss\\0x159E77AB.wav", "Officers require escort to precinct.", "escort_boss"); } }
        public static ScannerFile SuspectisenroutetoCentralLosSantos2 { get { return new ScannerFile("01_escort_boss\\0x16440167.wav", "Suspect is en route to Central Los Santos.", "escort_boss"); } }
        public static ScannerFile Officersrequestingimmediateescorttoprecinct1 { get { return new ScannerFile("01_escort_boss\\0x169F4940.wav", "Officers requesting immediate escort to precinct.", "escort_boss"); } }
        public static ScannerFile Officerstransportingprimarysuspectsareunderattack { get { return new ScannerFile("01_escort_boss\\0x16C08458.wav", "Officers transporting primary suspects are under attack.", "escort_boss"); } }
        public static ScannerFile AllunitsCode41 { get { return new ScannerFile("01_escort_boss\\0x17975D56.wav", "All units; Code 4.", "escort_boss"); } }
        public static ScannerFile SuspectisenroutetoCentralLosSantos2_1 { get { return new ScannerFile("01_escort_boss\\0x19C0C860.wav", "Suspect is en route to Central Los Santos.", "escort_boss"); } }
        public static ScannerFile WehavereportsofahighlevelLostgangmemberintransit1 { get { return new ScannerFile("01_escort_boss\\0x1A857318.wav", "We have reports of a high-level Lost gang member in transit.", "escort_boss"); } }
        public static ScannerFile Officersrequestingimmediatetransitassistance { get { return new ScannerFile("01_escort_boss\\0x1A97F89B.wav", "Officers requesting immediate transit assistance.", "escort_boss"); } }
        public static ScannerFile Allunitsstanddown1 { get { return new ScannerFile("01_escort_boss\\0x1BD9FE19.wav", "All units stand down.", "escort_boss"); } }
        public static ScannerFile Allunitsprimarytargetisunderarrest { get { return new ScannerFile("01_escort_boss\\0x1EC349F5.wav", "All units, primary target is under arrest.", "escort_boss"); } }
        public static ScannerFile WehavereportsofahighlevelVagosgangmemberintransit1 { get { return new ScannerFile("01_escort_boss\\0x1EC96AA1.wav", "We have reports of a high-level Vagos gang member in transit.", "escort_boss"); } }
    }
    public class extra_prefix
    {
        public static ScannerFile Beatup { get { return new ScannerFile("01_extra_prefix\\0x02CB8B4C.wav", "Beat up.", "extra_prefix"); } }
        public static ScannerFile Dented { get { return new ScannerFile("01_extra_prefix\\0x02E3C342.wav", "Dented.", "extra_prefix"); } }
        public static ScannerFile Modified { get { return new ScannerFile("01_extra_prefix\\0x06127D7F.wav", "Modified.", "extra_prefix"); } }
        public static ScannerFile Dirty { get { return new ScannerFile("01_extra_prefix\\0x067B9330.wav", "Dirty.", "extra_prefix"); } }
        public static ScannerFile Mint { get { return new ScannerFile("01_extra_prefix\\0x08510305.wav", "Mint.", "extra_prefix"); } }
        public static ScannerFile Custom { get { return new ScannerFile("01_extra_prefix\\0x0DE67420.wav", "Custom.", "extra_prefix"); } }
        public static ScannerFile Customized { get { return new ScannerFile("01_extra_prefix\\0x1303EEAD.wav", "Customized.", "extra_prefix"); } }
        public static ScannerFile Chopped { get { return new ScannerFile("01_extra_prefix\\0x1501AFC3.wav", "Chopped.", "extra_prefix"); } }
        public static ScannerFile Damaged { get { return new ScannerFile("01_extra_prefix\\0x151A41F6.wav", "Damaged.", "extra_prefix"); } }
        public static ScannerFile Rundown { get { return new ScannerFile("01_extra_prefix\\0x158689B3.wav", "Run-down.", "extra_prefix"); } }
        public static ScannerFile Distressed { get { return new ScannerFile("01_extra_prefix\\0x15F33674.wav", "Distressed.", "extra_prefix"); } }
        public static ScannerFile Rundown1 { get { return new ScannerFile("01_extra_prefix\\0x160B4ABB.wav", "Run-down.", "extra_prefix"); } }
        public static ScannerFile Battered { get { return new ScannerFile("01_extra_prefix\\0x17987A64.wav", "Battered.", "extra_prefix"); } }
        public static ScannerFile Rusty { get { return new ScannerFile("01_extra_prefix\\0x1CDCBBCC.wav", "Rusty.", "extra_prefix"); } }
    }
    public class gang_name
    {
        public static ScannerFile TheLost { get { return new ScannerFile("01_gang_name\\0x00A35403.wav", "The Lost.", "gang_name"); } }
        public static ScannerFile TheVagos { get { return new ScannerFile("01_gang_name\\0x0476C52A.wav", "The Vagos.", "gang_name"); } }
        public static ScannerFile Aprofessionalcrimeoutfit { get { return new ScannerFile("01_gang_name\\0x06123C0C.wav", "A professional crime outfit.", "gang_name"); } }
        public static ScannerFile AMexicangang { get { return new ScannerFile("01_gang_name\\0x06A0C782.wav", "A Mexican gang.", "gang_name"); } }
        public static ScannerFile Aprofessionalcriminalorganization { get { return new ScannerFile("01_gang_name\\0x0A713996.wav", "A professional criminal organization", "gang_name"); } }
        public static ScannerFile Aprofessionalcrimesyndicate { get { return new ScannerFile("01_gang_name\\0x0BE7D9C7.wav", "A professional crime syndicate.", "gang_name"); } }
        public static ScannerFile AnAfricanAmericangang { get { return new ScannerFile("01_gang_name\\0x0CB8D07E.wav", "An African-American gang.", "gang_name"); } }
        public static ScannerFile Abikergang { get { return new ScannerFile("01_gang_name\\0x0FCCB6E6.wav", "A biker gang.", "gang_name"); } }
        public static ScannerFile Aprofessionalcrimecrew { get { return new ScannerFile("01_gang_name\\0x1F1DE0EA.wav", "A professional crime crew.", "gang_name"); } }
    }
    public class generic_direction { public static ScannerFile Central { get { return new ScannerFile("01_generic_direction\\0x0BF563E8.wav", "Central.", "generic_direction"); } } }
    public class hair
    {
        public static ScannerFile Purplehair { get { return new ScannerFile("01_hair\\0x019331A8.wav", "Purple hair.", "hair"); } }
        public static ScannerFile Bluehair { get { return new ScannerFile("01_hair\\0x06449285.wav", "Blue hair.", "hair"); } }
        public static ScannerFile Orangehair { get { return new ScannerFile("01_hair\\0x0F349B52.wav", "Orange hair.", "hair"); } }
        public static ScannerFile Coloredhair { get { return new ScannerFile("01_hair\\0x133F208B.wav", "Colored hair.", "hair"); } }
        public static ScannerFile Aknittedcap { get { return new ScannerFile("01_hair\\0x1433A16F.wav", "A knitted cap.", "hair"); } }
        public static ScannerFile Brownhair { get { return new ScannerFile("01_hair\\0x184B98DE.wav", "Brown hair.", "hair"); } }
        public static ScannerFile Redhair { get { return new ScannerFile("01_hair\\0x18E34C46.wav", "Red hair.", "hair"); } }
        public static ScannerFile Greenhair { get { return new ScannerFile("01_hair\\0x18FC48DA.wav", "Green hair.", "hair"); } }
        public static ScannerFile Blackhair { get { return new ScannerFile("01_hair\\0x19960FF6.wav", "Black hair.", "hair"); } }
        public static ScannerFile Abaseballcap { get { return new ScannerFile("01_hair\\0x1A122B24.wav", "A baseball cap.", "hair"); } }
        public static ScannerFile Blondehair { get { return new ScannerFile("01_hair\\0x1D7A1600.wav", "Blonde hair.", "hair"); } }
        public static ScannerFile Punkhair { get { return new ScannerFile("01_hair\\0x1E913A94.wav", "Punk hair.", "hair"); } }
        public static ScannerFile Acowboyhat { get { return new ScannerFile("01_hair\\0x1F47D088.wav", "A cowboy hat.", "hair"); } }
        public static ScannerFile Nohair { get { return new ScannerFile("01_hair\\0x1F975F7C.wav", "No hair.", "hair"); } }
    }
    public class helicopter_refuelling
    {
        public static ScannerFile Airunitlowonfuelreturningtobase { get { return new ScannerFile("01_helicopter_refuelling\\0x021BAEBA.wav", "Air unit low on fuel, returning to base.", "helicopter_refuelling"); } }
        public static ScannerFile Eagleunithasbeenrecalledforarefueling { get { return new ScannerFile("01_helicopter_refuelling\\0x0783F887.wav", "Eagle unit has been recalled for a refueling.", "helicopter_refuelling"); } }
        public static ScannerFile Eagleunithasbeenrecalledforrefueling { get { return new ScannerFile("01_helicopter_refuelling\\0x0CCA0311.wav", "Eagle unit has been recalled for refueling.", "helicopter_refuelling"); } }
        public static ScannerFile Airsupportlowonfuelreturningtobase { get { return new ScannerFile("01_helicopter_refuelling\\0x1017CABD.wav", "Air support low on fuel; returning to base.", "helicopter_refuelling"); } }
        public static ScannerFile Airsupportisreturningtobaseforarefueling { get { return new ScannerFile("01_helicopter_refuelling\\0x12924E9F.wav", "Air support is returning to base for a refueling.", "helicopter_refuelling"); } }
        public static ScannerFile Airsupportunitneedstorefuel { get { return new ScannerFile("01_helicopter_refuelling\\0x1336D0F7.wav", "Air support unit needs to refuel.", "helicopter_refuelling"); } }
        public static ScannerFile Airunitneedstorefuel { get { return new ScannerFile("01_helicopter_refuelling\\0x15761452.wav", "Air unit needs to refuel.", "helicopter_refuelling"); } }
        public static ScannerFile Airunitislowonfuelreturningtobase { get { return new ScannerFile("01_helicopter_refuelling\\0x19F1DD49.wav", "Air unit is low on fuel; returning to base.", "helicopter_refuelling"); } }
        public static ScannerFile Airsupportisheadinghometorefuel { get { return new ScannerFile("01_helicopter_refuelling\\0x1C4F2219.wav", "Air support is heading home to refuel.", "helicopter_refuelling"); } }
        public static ScannerFile Airunitisreturningtobaseforarefueling { get { return new ScannerFile("01_helicopter_refuelling\\0x1FD3E922.wav", "Air unit is returning to base for a refueling.", "helicopter_refuelling"); } }
    }
    public class in_the_water
    {
        public static ScannerFile IntheLSriver { get { return new ScannerFile("01_in_the_water\\0x0243B2FD.wav", "In the LS river.", "in_the_water"); } }
        public static ScannerFile OfftheWesterncoast { get { return new ScannerFile("01_in_the_water\\0x03680F1E.wav", "Off the Western coast.", "in_the_water"); } }
        public static ScannerFile OfftheWestcoast { get { return new ScannerFile("01_in_the_water\\0x09AD1BA7.wav", "Off the West coast.", "in_the_water"); } }
        public static ScannerFile OfftheNortherncoast { get { return new ScannerFile("01_in_the_water\\0x0BC82B92.wav", "Off the Northern coast.", "in_the_water"); } }
        public static ScannerFile Inthewater { get { return new ScannerFile("01_in_the_water\\0x0BF2657F.wav", "In the water.", "in_the_water"); } }
        public static ScannerFile IntheAlamoSea { get { return new ScannerFile("01_in_the_water\\0x0D0526A6.wav", "In the Alamo Sea.", "in_the_water"); } }
        public static ScannerFile OfftheEastcoast { get { return new ScannerFile("01_in_the_water\\0x0F083043.wav", "Off the East coast.", "in_the_water"); } }
        public static ScannerFile OfftheSoutherncoast { get { return new ScannerFile("01_in_the_water\\0x10CDD7BA.wav", "Off the Southern coast.", "in_the_water"); } }
        public static ScannerFile OfftheSouthcoast { get { return new ScannerFile("01_in_the_water\\0x15E2E1E5.wav", "Off the South coast.", "in_the_water"); } }
        public static ScannerFile OfftheNorthcoast { get { return new ScannerFile("01_in_the_water\\0x1916062E.wav", "Off the North coast.", "in_the_water"); } }
        public static ScannerFile OfftheEasterncoast { get { return new ScannerFile("01_in_the_water\\0x1EC9CFC7.wav", "Off the Eastern coast.", "in_the_water"); } }
    }
    public class lethal_force
    {
        public static ScannerFile Useoflethalforceisnotauthorized { get { return new ScannerFile("01_lethal_force\\0x0270822F.wav", "Use of lethal force is not authorized.", "lethal_force"); } }
        public static ScannerFile Useofdeadlyforcepermitted { get { return new ScannerFile("01_lethal_force\\0x064EE711.wav", "Use of deadly force permitted.", "lethal_force"); } }
        public static ScannerFile Nocasualties { get { return new ScannerFile("01_lethal_force\\0x073ACBC4.wav", "No casualties.", "lethal_force"); } }
        public static ScannerFile Useofdeadlyforceisauthorized { get { return new ScannerFile("01_lethal_force\\0x07E6AA1A.wav", "Use of deadly force is authorized.", "lethal_force"); } }
        public static ScannerFile Nonlethalweaponstobeemployed { get { return new ScannerFile("01_lethal_force\\0x0B7B9446.wav", "Non-lethal weapons to be employed.", "lethal_force"); } }
        public static ScannerFile Useoflethalforceisauthorized { get { return new ScannerFile("01_lethal_force\\0x145F4332.wav", "Use of lethal force is authorized.", "lethal_force"); } }
        public static ScannerFile Useoflethalforceispermitted { get { return new ScannerFile("01_lethal_force\\0x16BA07E8.wav", "Use of lethal force is permitted.", "lethal_force"); } }
        public static ScannerFile Useofdeadlyforceauthorized { get { return new ScannerFile("01_lethal_force\\0x17940975.wav", "Use of deadly force authorized.", "lethal_force"); } }
        public static ScannerFile Nonlethalweaponsonly { get { return new ScannerFile("01_lethal_force\\0x18AAAEA3.wav", "Non-lethal weapons only.", "lethal_force"); } }
        public static ScannerFile Useofdeadlyforcepermitted1 { get { return new ScannerFile("01_lethal_force\\0x1B901193.wav", "Use of deadly force permitted.", "lethal_force"); } }
        public static ScannerFile Useofdeadlyforceisauthorized1 { get { return new ScannerFile("01_lethal_force\\0x1EEAD848.wav", "Use of deadly force is authorized.", "lethal_force"); } }
        public static ScannerFile Nonlethaltacticsonly { get { return new ScannerFile("01_lethal_force\\0x1F81FC45.wav", "Non-lethal tactics only.", "lethal_force"); } }
    }
    public class lp_letters_high
    {
        public static ScannerFile Nora { get { return new ScannerFile("01_lp_letters_high\\0x001E74CF.wav", "Nora.", "lp_letters_high"); } }
        public static ScannerFile George { get { return new ScannerFile("01_lp_letters_high\\0x002E9DE5.wav", "George.", "lp_letters_high"); } }
        public static ScannerFile Sam { get { return new ScannerFile("01_lp_letters_high\\0x0087733A.wav", "Sam.", "lp_letters_high"); } }
        public static ScannerFile Mary { get { return new ScannerFile("01_lp_letters_high\\0x00E210C8.wav", "Mary.", "lp_letters_high"); } }
        public static ScannerFile Frank { get { return new ScannerFile("01_lp_letters_high\\0x0158F4B2.wav", "Frank.", "lp_letters_high"); } }
        public static ScannerFile David { get { return new ScannerFile("01_lp_letters_high\\0x02F91F2A.wav", "David.", "lp_letters_high"); } }
        public static ScannerFile William { get { return new ScannerFile("01_lp_letters_high\\0x03A9D3DF.wav", "William.", "lp_letters_high"); } }
        public static ScannerFile George1 { get { return new ScannerFile("01_lp_letters_high\\0x03AF64F1.wav", "George.", "lp_letters_high"); } }
        public static ScannerFile John { get { return new ScannerFile("01_lp_letters_high\\0x03DAC98A.wav", "John.", "lp_letters_high"); } }
        public static ScannerFile Boy { get { return new ScannerFile("01_lp_letters_high\\0x043556DB.wav", "Boy.", "lp_letters_high"); } }
        public static ScannerFile King { get { return new ScannerFile("01_lp_letters_high\\0x0458C6FF.wav", "King.", "lp_letters_high"); } }
        public static ScannerFile Tom { get { return new ScannerFile("01_lp_letters_high\\0x047066CE.wav", "Tom.", "lp_letters_high"); } }
        public static ScannerFile Edward { get { return new ScannerFile("01_lp_letters_high\\0x04A5AE85.wav", "Edward.", "lp_letters_high"); } }
        public static ScannerFile Charles { get { return new ScannerFile("01_lp_letters_high\\0x068372C5.wav", "Charles.", "lp_letters_high"); } }
        public static ScannerFile King1 { get { return new ScannerFile("01_lp_letters_high\\0x080ECE69.wav", "King.", "lp_letters_high"); } }
        public static ScannerFile Union { get { return new ScannerFile("01_lp_letters_high\\0x082957EE.wav", "Union.", "lp_letters_high"); } }
        public static ScannerFile Paul { get { return new ScannerFile("01_lp_letters_high\\0x0A69B2A3.wav", "Paul.", "lp_letters_high"); } }
        public static ScannerFile Victor { get { return new ScannerFile("01_lp_letters_high\\0x0ADD2EDE.wav", "Victor.", "lp_letters_high"); } }
        public static ScannerFile Sam1 { get { return new ScannerFile("01_lp_letters_high\\0x0B5CC8E2.wav", "Sam.", "lp_letters_high"); } }
        public static ScannerFile Adam { get { return new ScannerFile("01_lp_letters_high\\0x0C5E4FE3.wav", "Adam.", "lp_letters_high"); } }
        public static ScannerFile Frank1 { get { return new ScannerFile("01_lp_letters_high\\0x0C804B02.wav", "Frank.", "lp_letters_high"); } }
        public static ScannerFile Tom1 { get { return new ScannerFile("01_lp_letters_high\\0x0C8476F7.wav", "Tom.", "lp_letters_high"); } }
        public static ScannerFile Adam1 { get { return new ScannerFile("01_lp_letters_high\\0x0D951251.wav", "Adam.", "lp_letters_high"); } }
        public static ScannerFile Queen { get { return new ScannerFile("01_lp_letters_high\\0x0DFBFA78.wav", "Queen.", "lp_letters_high"); } }
        public static ScannerFile Ocean { get { return new ScannerFile("01_lp_letters_high\\0x0F6D568F.wav", "Ocean.", "lp_letters_high"); } }
        public static ScannerFile Robert { get { return new ScannerFile("01_lp_letters_high\\0x0FC33F6E.wav", "Robert.", "lp_letters_high"); } }
        public static ScannerFile Mary1 { get { return new ScannerFile("01_lp_letters_high\\0x101A2F36.wav", "Mary.", "lp_letters_high"); } }
        public static ScannerFile Ita { get { return new ScannerFile("01_lp_letters_high\\0x106ED745.wav", "Ita.", "lp_letters_high"); } }
        public static ScannerFile Paul1 { get { return new ScannerFile("01_lp_letters_high\\0x109CFF03.wav", "Paul.", "lp_letters_high"); } }
        public static ScannerFile Lincoln { get { return new ScannerFile("01_lp_letters_high\\0x11B9CE69.wav", "Lincoln.", "lp_letters_high"); } }
        public static ScannerFile Boy1 { get { return new ScannerFile("01_lp_letters_high\\0x11E5B23C.wav", "Boy.", "lp_letters_high"); } }
        public static ScannerFile Ita1 { get { return new ScannerFile("01_lp_letters_high\\0x1233DACD.wav", "Ita.", "lp_letters_high"); } }
        public static ScannerFile Edward1 { get { return new ScannerFile("01_lp_letters_high\\0x12670A08.wav", "Edward.", "lp_letters_high"); } }
        public static ScannerFile Henry { get { return new ScannerFile("01_lp_letters_high\\0x12E88C8C.wav", "Henry.", "lp_letters_high"); } }
        public static ScannerFile Queen1 { get { return new ScannerFile("01_lp_letters_high\\0x13B305EE.wav", "Queen.", "lp_letters_high"); } }
        public static ScannerFile Zebra { get { return new ScannerFile("01_lp_letters_high\\0x1562FF84.wav", "Zebra.", "lp_letters_high"); } }
        public static ScannerFile Union1 { get { return new ScannerFile("01_lp_letters_high\\0x156BF273.wav", "Union.", "lp_letters_high"); } }
        public static ScannerFile Zebra1 { get { return new ScannerFile("01_lp_letters_high\\0x15BA0034.wav", "Zebra.", "lp_letters_high"); } }
        public static ScannerFile John1 { get { return new ScannerFile("01_lp_letters_high\\0x15D52D7F.wav", "John.", "lp_letters_high"); } }
        public static ScannerFile Young { get { return new ScannerFile("01_lp_letters_high\\0x1624C995.wav", "Young.", "lp_letters_high"); } }
        public static ScannerFile Henry1 { get { return new ScannerFile("01_lp_letters_high\\0x16451346.wav", "Henry.", "lp_letters_high"); } }
        public static ScannerFile XRay { get { return new ScannerFile("01_lp_letters_high\\0x16F5D733.wav", "X-Ray.", "lp_letters_high"); } }
        public static ScannerFile William1 { get { return new ScannerFile("01_lp_letters_high\\0x17073A99.wav", "William.", "lp_letters_high"); } }
        public static ScannerFile Nora1 { get { return new ScannerFile("01_lp_letters_high\\0x173F6314.wav", "Nora.", "lp_letters_high"); } }
        public static ScannerFile Charles1 { get { return new ScannerFile("01_lp_letters_high\\0x18239606.wav", "Charles.", "lp_letters_high"); } }
        public static ScannerFile Victor1 { get { return new ScannerFile("01_lp_letters_high\\0x188F8A43.wav", "Victor.", "lp_letters_high"); } }
        public static ScannerFile Ocean1 { get { return new ScannerFile("01_lp_letters_high\\0x19432A3B.wav", "Ocean.", "lp_letters_high"); } }
        public static ScannerFile Robert1 { get { return new ScannerFile("01_lp_letters_high\\0x197BD2D8.wav", "Robert.", "lp_letters_high"); } }
        public static ScannerFile XRay1 { get { return new ScannerFile("01_lp_letters_high\\0x1B1BDF79.wav", "X-Ray.", "lp_letters_high"); } }
        public static ScannerFile Young1 { get { return new ScannerFile("01_lp_letters_high\\0x1B2FD3AF.wav", "Young.", "lp_letters_high"); } }
        public static ScannerFile Lincoln1 { get { return new ScannerFile("01_lp_letters_high\\0x1E5D67B5.wav", "Lincoln.", "lp_letters_high"); } }
    }
    public class lp_letters_low
    {
        public static ScannerFile William { get { return new ScannerFile("01_lp_letters_low\\0x0031F6C5.wav", "William.", "lp_letters_low"); } }
        public static ScannerFile Robert { get { return new ScannerFile("01_lp_letters_low\\0x00D31CC4.wav", "Robert.", "lp_letters_low"); } }
        public static ScannerFile HASH01E6E7C1 { get { return new ScannerFile("01_lp_letters_low\\0x01E6E7C1.wav", "0x01E6E7C1", "lp_letters_low"); } }
        public static ScannerFile HASH03361ED9 { get { return new ScannerFile("01_lp_letters_low\\0x03361ED9.wav", "0x03361ED9", "lp_letters_low"); } }
        public static ScannerFile HASH03B8C5D6 { get { return new ScannerFile("01_lp_letters_low\\0x03B8C5D6.wav", "0x03B8C5D6", "lp_letters_low"); } }
        public static ScannerFile HASH048C0CEC { get { return new ScannerFile("01_lp_letters_low\\0x048C0CEC.wav", "0x048C0CEC", "lp_letters_low"); } }
        public static ScannerFile HASH04936189 { get { return new ScannerFile("01_lp_letters_low\\0x04936189.wav", "0x04936189", "lp_letters_low"); } }
        public static ScannerFile HASH0537B541 { get { return new ScannerFile("01_lp_letters_low\\0x0537B541.wav", "0x0537B541", "lp_letters_low"); } }
        public static ScannerFile HASH055D0BCA { get { return new ScannerFile("01_lp_letters_low\\0x055D0BCA.wav", "0x055D0BCA", "lp_letters_low"); } }
        public static ScannerFile HASH05EE78E6 { get { return new ScannerFile("01_lp_letters_low\\0x05EE78E6.wav", "0x05EE78E6", "lp_letters_low"); } }
        public static ScannerFile HASH05F0170F { get { return new ScannerFile("01_lp_letters_low\\0x05F0170F.wav", "0x05F0170F", "lp_letters_low"); } }
        public static ScannerFile HASH064182C9 { get { return new ScannerFile("01_lp_letters_low\\0x064182C9.wav", "0x064182C9", "lp_letters_low"); } }
        public static ScannerFile HASH074858B4 { get { return new ScannerFile("01_lp_letters_low\\0x074858B4.wav", "0x074858B4", "lp_letters_low"); } }
        public static ScannerFile HASH07511EA5 { get { return new ScannerFile("01_lp_letters_low\\0x07511EA5.wav", "0x07511EA5", "lp_letters_low"); } }
        public static ScannerFile HASH0843F47A { get { return new ScannerFile("01_lp_letters_low\\0x0843F47A.wav", "0x0843F47A", "lp_letters_low"); } }
        public static ScannerFile HASH0856C2D4 { get { return new ScannerFile("01_lp_letters_low\\0x0856C2D4.wav", "0x0856C2D4", "lp_letters_low"); } }
        public static ScannerFile HASH0A913F22 { get { return new ScannerFile("01_lp_letters_low\\0x0A913F22.wav", "0x0A913F22", "lp_letters_low"); } }
        public static ScannerFile HASH0ABED54D { get { return new ScannerFile("01_lp_letters_low\\0x0ABED54D.wav", "0x0ABED54D", "lp_letters_low"); } }
        public static ScannerFile HASH0B4627A7 { get { return new ScannerFile("01_lp_letters_low\\0x0B4627A7.wav", "0x0B4627A7", "lp_letters_low"); } }
        public static ScannerFile HASH0B905BFE { get { return new ScannerFile("01_lp_letters_low\\0x0B905BFE.wav", "0x0B905BFE", "lp_letters_low"); } }
        public static ScannerFile HASH0D681934 { get { return new ScannerFile("01_lp_letters_low\\0x0D681934.wav", "0x0D681934", "lp_letters_low"); } }
        public static ScannerFile HASH0DF35248 { get { return new ScannerFile("01_lp_letters_low\\0x0DF35248.wav", "0x0DF35248", "lp_letters_low"); } }
        public static ScannerFile HASH0E67EDE6 { get { return new ScannerFile("01_lp_letters_low\\0x0E67EDE6.wav", "0x0E67EDE6", "lp_letters_low"); } }
        public static ScannerFile HASH0F54B9C7 { get { return new ScannerFile("01_lp_letters_low\\0x0F54B9C7.wav", "0x0F54B9C7", "lp_letters_low"); } }
        public static ScannerFile HASH1040DC27 { get { return new ScannerFile("01_lp_letters_low\\0x1040DC27.wav", "0x1040DC27", "lp_letters_low"); } }
        public static ScannerFile HASH10A007F6 { get { return new ScannerFile("01_lp_letters_low\\0x10A007F6.wav", "0x10A007F6", "lp_letters_low"); } }
        public static ScannerFile HASH11043372 { get { return new ScannerFile("01_lp_letters_low\\0x11043372.wav", "0x11043372", "lp_letters_low"); } }
        public static ScannerFile HASH110EAB58 { get { return new ScannerFile("01_lp_letters_low\\0x110EAB58.wav", "0x110EAB58", "lp_letters_low"); } }
        public static ScannerFile HASH1274A090 { get { return new ScannerFile("01_lp_letters_low\\0x1274A090.wav", "0x1274A090", "lp_letters_low"); } }
        public static ScannerFile HASH13032F14 { get { return new ScannerFile("01_lp_letters_low\\0x13032F14.wav", "0x13032F14", "lp_letters_low"); } }
        public static ScannerFile HASH1313AF33 { get { return new ScannerFile("01_lp_letters_low\\0x1313AF33.wav", "0x1313AF33", "lp_letters_low"); } }
        public static ScannerFile HASH134D0FEB { get { return new ScannerFile("01_lp_letters_low\\0x134D0FEB.wav", "0x134D0FEB", "lp_letters_low"); } }
        public static ScannerFile HASH138511DC { get { return new ScannerFile("01_lp_letters_low\\0x138511DC.wav", "0x138511DC", "lp_letters_low"); } }
        public static ScannerFile HASH15A77B52 { get { return new ScannerFile("01_lp_letters_low\\0x15A77B52.wav", "0x15A77B52", "lp_letters_low"); } }
        public static ScannerFile HASH15BBE1BC { get { return new ScannerFile("01_lp_letters_low\\0x15BBE1BC.wav", "0x15BBE1BC", "lp_letters_low"); } }
        public static ScannerFile HASH16079E36 { get { return new ScannerFile("01_lp_letters_low\\0x16079E36.wav", "0x16079E36", "lp_letters_low"); } }
        public static ScannerFile HASH175B37F2 { get { return new ScannerFile("01_lp_letters_low\\0x175B37F2.wav", "0x175B37F2", "lp_letters_low"); } }
        public static ScannerFile HASH1774FFFB { get { return new ScannerFile("01_lp_letters_low\\0x1774FFFB.wav", "0x1774FFFB", "lp_letters_low"); } }
        public static ScannerFile HASH177C9C03 { get { return new ScannerFile("01_lp_letters_low\\0x177C9C03.wav", "0x177C9C03", "lp_letters_low"); } }
        public static ScannerFile HASH17BE3350 { get { return new ScannerFile("01_lp_letters_low\\0x17BE3350.wav", "0x17BE3350", "lp_letters_low"); } }
        public static ScannerFile HASH18F531BA { get { return new ScannerFile("01_lp_letters_low\\0x18F531BA.wav", "0x18F531BA", "lp_letters_low"); } }
        public static ScannerFile HASH18F83C11 { get { return new ScannerFile("01_lp_letters_low\\0x18F83C11.wav", "0x18F83C11", "lp_letters_low"); } }
        public static ScannerFile HASH192F773C { get { return new ScannerFile("01_lp_letters_low\\0x192F773C.wav", "0x192F773C", "lp_letters_low"); } }
        public static ScannerFile HASH1B56ADC8 { get { return new ScannerFile("01_lp_letters_low\\0x1B56ADC8.wav", "0x1B56ADC8", "lp_letters_low"); } }
        public static ScannerFile HASH1C1CE18D { get { return new ScannerFile("01_lp_letters_low\\0x1C1CE18D.wav", "0x1C1CE18D", "lp_letters_low"); } }
        public static ScannerFile HASH1C49E292 { get { return new ScannerFile("01_lp_letters_low\\0x1C49E292.wav", "0x1C49E292", "lp_letters_low"); } }
        public static ScannerFile HASH1CEB0504 { get { return new ScannerFile("01_lp_letters_low\\0x1CEB0504.wav", "0x1CEB0504", "lp_letters_low"); } }
        public static ScannerFile HASH1CEFAECF { get { return new ScannerFile("01_lp_letters_low\\0x1CEFAECF.wav", "0x1CEFAECF", "lp_letters_low"); } }
        public static ScannerFile HASH1CFE711A { get { return new ScannerFile("01_lp_letters_low\\0x1CFE711A.wav", "0x1CFE711A", "lp_letters_low"); } }
        public static ScannerFile HASH1E2EF14D { get { return new ScannerFile("01_lp_letters_low\\0x1E2EF14D.wav", "0x1E2EF14D", "lp_letters_low"); } }
        public static ScannerFile HASH1E6B3DD7 { get { return new ScannerFile("01_lp_letters_low\\0x1E6B3DD7.wav", "0x1E6B3DD7", "lp_letters_low"); } }
        public static ScannerFile HASH1ED86465 { get { return new ScannerFile("01_lp_letters_low\\0x1ED86465.wav", "0x1ED86465", "lp_letters_low"); } }
        public static ScannerFile HASH1F7AA846 { get { return new ScannerFile("01_lp_letters_low\\0x1F7AA846.wav", "0x1F7AA846", "lp_letters_low"); } }
    }
    public class lp_numbers
    {
        public static ScannerFile Zero { get { return new ScannerFile("01_lp_numbers\\0x04F3CC0F.wav", "0", "lp_numbers"); } }
        public static ScannerFile Zero1 { get { return new ScannerFile("01_lp_numbers\\0x02A288B0.wav", "0", "lp_numbers"); } }
        public static ScannerFile Zero2 { get { return new ScannerFile("01_lp_numbers\\0x03B8B23A.wav", "0", "lp_numbers"); } }
        public static ScannerFile Zero3 { get { return new ScannerFile("01_lp_numbers\\0x001F8998.wav", "0", "lp_numbers"); } }
        public static ScannerFile Zero4 { get { return new ScannerFile("01_lp_numbers\\0x0BA6ADB6.wav", "0", "lp_numbers"); } }

        public static ScannerFile One { get { return new ScannerFile("01_lp_numbers\\0x0100252F.wav", "1", "lp_numbers"); } }
        public static ScannerFile One1 { get { return new ScannerFile("01_lp_numbers\\0x0945F55C.wav", "1", "lp_numbers"); } }
        public static ScannerFile One2 { get { return new ScannerFile("01_lp_numbers\\0x0A44E2A4.wav", "1", "lp_numbers"); } }
        public static ScannerFile One3 { get { return new ScannerFile("01_lp_numbers\\0x0AB5A365.wav", "1", "lp_numbers"); } }
        public static ScannerFile One4 { get { return new ScannerFile("01_lp_numbers\\0x0B053108.wav", "1", "lp_numbers"); } }
        public static ScannerFile One5 { get { return new ScannerFile("01_lp_numbers\\0x0B0CEFA0.wav", "1", "lp_numbers"); } }

        public static ScannerFile Two { get { return new ScannerFile("01_lp_numbers\\0x02E5EEC0.wav", "2", "lp_numbers"); } }
        public static ScannerFile Two1 { get { return new ScannerFile("01_lp_numbers\\0x03BEB046.wav", "2", "lp_numbers"); } }
        public static ScannerFile Two2 { get { return new ScannerFile("01_lp_numbers\\0x03C6BD7E.wav", "2", "lp_numbers"); } }
        public static ScannerFile Two3 { get { return new ScannerFile("01_lp_numbers\\0x09B8F8B2.wav", "2", "lp_numbers"); } }
        public static ScannerFile Two4 { get { return new ScannerFile("01_lp_numbers\\0x09E3959A.wav", "2", "lp_numbers"); } }
        public static ScannerFile Two5 { get { return new ScannerFile("01_lp_numbers\\0x0A21BD3A.wav", "2", "lp_numbers"); } }

        public static ScannerFile Three { get { return new ScannerFile("01_lp_numbers\\0x060BDED7.wav", "3", "lp_numbers"); } }
        public static ScannerFile Three1 { get { return new ScannerFile("01_lp_numbers\\0x0866698B.wav", "3", "lp_numbers"); } }
        public static ScannerFile Three2 { get { return new ScannerFile("01_lp_numbers\\0x0B624017.wav", "3", "lp_numbers"); } }

        public static ScannerFile Four { get { return new ScannerFile("01_lp_numbers\\0x02886760.wav", "4", "lp_numbers"); } }
        public static ScannerFile Four1 { get { return new ScannerFile("01_lp_numbers\\0x03C3D7A1.wav", "4", "lp_numbers"); } }
        public static ScannerFile Four2 { get { return new ScannerFile("01_lp_numbers\\0x041D81A4.wav", "4", "lp_numbers"); } }
        public static ScannerFile Four3 { get { return new ScannerFile("01_lp_numbers\\0x04E9AB5B.wav", "4", "lp_numbers"); } }
        public static ScannerFile Four4 { get { return new ScannerFile("01_lp_numbers\\0x08A07389.wav", "4", "lp_numbers"); } }
        public static ScannerFile Four5 { get { return new ScannerFile("01_lp_numbers\\0x0AC34F2D.wav", "4", "lp_numbers"); } }
        public static ScannerFile Four6 { get { return new ScannerFile("01_lp_numbers\\0x0B193882.wav", "4", "lp_numbers"); } }

        public static ScannerFile Five { get { return new ScannerFile("01_lp_numbers\\0x034F92AA.wav", "5", "lp_numbers"); } }
        public static ScannerFile Five1 { get { return new ScannerFile("01_lp_numbers\\0x057A4C20.wav", "5", "lp_numbers"); } }
        public static ScannerFile Five2 { get { return new ScannerFile("01_lp_numbers\\0x07DC8B99.wav", "5", "lp_numbers"); } }
        public static ScannerFile Five3 { get { return new ScannerFile("01_lp_numbers\\0x084F6895.wav", "5", "lp_numbers"); } }

        public static ScannerFile Six { get { return new ScannerFile("01_lp_numbers\\0x00717979.wav", "6", "lp_numbers"); } }
        public static ScannerFile Six1 { get { return new ScannerFile("01_lp_numbers\\0x01C22022.wav", "6", "lp_numbers"); } }
        public static ScannerFile Six2 { get { return new ScannerFile("01_lp_numbers\\0x056635CE.wav", "6", "lp_numbers"); } }
        public static ScannerFile Six3 { get { return new ScannerFile("01_lp_numbers\\0x072B44F1.wav", "6", "lp_numbers"); } }
        public static ScannerFile Six4 { get { return new ScannerFile("01_lp_numbers\\0x0BC33189.wav", "6", "lp_numbers"); } }

        public static ScannerFile Seven { get { return new ScannerFile("01_lp_numbers\\0x009E8E85.wav", "7", "lp_numbers"); } }
        public static ScannerFile Seven1 { get { return new ScannerFile("01_lp_numbers\\0x0250392E.wav", "7", "lp_numbers"); } }
        public static ScannerFile Seven2 { get { return new ScannerFile("01_lp_numbers\\0x08193D1F.wav", "7", "lp_numbers"); } }
        public static ScannerFile Seven3 { get { return new ScannerFile("01_lp_numbers\\0x08DA0642.wav", "7", "lp_numbers"); } }
        public static ScannerFile Seven4 { get { return new ScannerFile("01_lp_numbers\\0x0A520191.wav", "7", "lp_numbers"); } }

        public static ScannerFile Eight { get { return new ScannerFile("01_lp_numbers\\0x00896952.wav", "8", "lp_numbers"); } }
        public static ScannerFile Eight1 { get { return new ScannerFile("01_lp_numbers\\0x020E8A21.wav", "8", "lp_numbers"); } }
        public static ScannerFile Eight2 { get { return new ScannerFile("01_lp_numbers\\0x02267CBE.wav", "8", "lp_numbers"); } }
        public static ScannerFile Eight3 { get { return new ScannerFile("01_lp_numbers\\0x0702944B.wav", "8", "lp_numbers"); } }
        public static ScannerFile Eight4 { get { return new ScannerFile("01_lp_numbers\\0x0738B2AC.wav", "8", "lp_numbers"); } }
        public static ScannerFile Eight5 { get { return new ScannerFile("01_lp_numbers\\0x0B06D7D8.wav", "8", "lp_numbers"); } }

        public static ScannerFile Nine { get { return new ScannerFile("01_lp_numbers\\0x0083267E.wav", "9", "lp_numbers"); } }
        public static ScannerFile Niner { get { return new ScannerFile("01_lp_numbers\\0x04391135.wav", "Niner", "lp_numbers"); } }
        public static ScannerFile Niner1 { get { return new ScannerFile("01_lp_numbers\\0x04E10A53.wav", "Niner", "lp_numbers"); } }
        public static ScannerFile Niner2 { get { return new ScannerFile("01_lp_numbers\\0x0AC5F426.wav", "Niner", "lp_numbers"); } }
        public static ScannerFile Niner3 { get { return new ScannerFile("01_lp_numbers\\0x0B111513.wav", "Niner", "lp_numbers"); } }











        public static ScannerFile HASH0BD3DC4A { get { return new ScannerFile("01_lp_numbers\\0x0BD3DC4A.wav", "0x0BD3DC4A", "lp_numbers"); } }
        public static ScannerFile HASH0BDF3B4E { get { return new ScannerFile("01_lp_numbers\\0x0BDF3B4E.wav", "0x0BDF3B4E", "lp_numbers"); } }
        public static ScannerFile HASH0C475100 { get { return new ScannerFile("01_lp_numbers\\0x0C475100.wav", "0x0C475100", "lp_numbers"); } }
        public static ScannerFile HASH0D0A9A07 { get { return new ScannerFile("01_lp_numbers\\0x0D0A9A07.wav", "0x0D0A9A07", "lp_numbers"); } }
        public static ScannerFile HASH0E059625 { get { return new ScannerFile("01_lp_numbers\\0x0E059625.wav", "0x0E059625", "lp_numbers"); } }
        public static ScannerFile HASH0E326A5E { get { return new ScannerFile("01_lp_numbers\\0x0E326A5E.wav", "0x0E326A5E", "lp_numbers"); } }
        public static ScannerFile HASH0E5738DB { get { return new ScannerFile("01_lp_numbers\\0x0E5738DB.wav", "0x0E5738DB", "lp_numbers"); } }
        public static ScannerFile HASH0E5D7871 { get { return new ScannerFile("01_lp_numbers\\0x0E5D7871.wav", "0x0E5D7871", "lp_numbers"); } }
        public static ScannerFile HASH0F57708C { get { return new ScannerFile("01_lp_numbers\\0x0F57708C.wav", "0x0F57708C", "lp_numbers"); } }
        public static ScannerFile HASH0F5C49C5 { get { return new ScannerFile("01_lp_numbers\\0x0F5C49C5.wav", "0x0F5C49C5", "lp_numbers"); } }
        public static ScannerFile HASH0F684FCA { get { return new ScannerFile("01_lp_numbers\\0x0F684FCA.wav", "0x0F684FCA", "lp_numbers"); } }
        public static ScannerFile HASH0F6A7978 { get { return new ScannerFile("01_lp_numbers\\0x0F6A7978.wav", "0x0F6A7978", "lp_numbers"); } }
        public static ScannerFile HASH0FA44161 { get { return new ScannerFile("01_lp_numbers\\0x0FA44161.wav", "0x0FA44161", "lp_numbers"); } }
        public static ScannerFile HASH0FF332A5 { get { return new ScannerFile("01_lp_numbers\\0x0FF332A5.wav", "0x0FF332A5", "lp_numbers"); } }
        public static ScannerFile HASH1005A74F { get { return new ScannerFile("01_lp_numbers\\0x1005A74F.wav", "0x1005A74F", "lp_numbers"); } }
        public static ScannerFile HASH100925CC { get { return new ScannerFile("01_lp_numbers\\0x100925CC.wav", "0x100925CC", "lp_numbers"); } }
        public static ScannerFile HASH1047F6FB { get { return new ScannerFile("01_lp_numbers\\0x1047F6FB.wav", "0x1047F6FB", "lp_numbers"); } }
        public static ScannerFile HASH105D245D { get { return new ScannerFile("01_lp_numbers\\0x105D245D.wav", "0x105D245D", "lp_numbers"); } }
        public static ScannerFile HASH109F8A35 { get { return new ScannerFile("01_lp_numbers\\0x109F8A35.wav", "0x109F8A35", "lp_numbers"); } }
        public static ScannerFile HASH118D3334 { get { return new ScannerFile("01_lp_numbers\\0x118D3334.wav", "0x118D3334", "lp_numbers"); } }
        public static ScannerFile HASH11A62DC2 { get { return new ScannerFile("01_lp_numbers\\0x11A62DC2.wav", "0x11A62DC2", "lp_numbers"); } }
        public static ScannerFile HASH11D9ED0F { get { return new ScannerFile("01_lp_numbers\\0x11D9ED0F.wav", "0x11D9ED0F", "lp_numbers"); } }
        public static ScannerFile HASH12154D1F { get { return new ScannerFile("01_lp_numbers\\0x12154D1F.wav", "0x12154D1F", "lp_numbers"); } }
        public static ScannerFile HASH12657A9F { get { return new ScannerFile("01_lp_numbers\\0x12657A9F.wav", "0x12657A9F", "lp_numbers"); } }
        public static ScannerFile HASH1326514D { get { return new ScannerFile("01_lp_numbers\\0x1326514D.wav", "0x1326514D", "lp_numbers"); } }
        public static ScannerFile HASH134D0ED7 { get { return new ScannerFile("01_lp_numbers\\0x134D0ED7.wav", "0x134D0ED7", "lp_numbers"); } }
        public static ScannerFile HASH140BA943 { get { return new ScannerFile("01_lp_numbers\\0x140BA943.wav", "0x140BA943", "lp_numbers"); } }
        public static ScannerFile HASH14215F58 { get { return new ScannerFile("01_lp_numbers\\0x14215F58.wav", "0x14215F58", "lp_numbers"); } }
        public static ScannerFile HASH146411BD { get { return new ScannerFile("01_lp_numbers\\0x146411BD.wav", "0x146411BD", "lp_numbers"); } }
        public static ScannerFile HASH14692C3D { get { return new ScannerFile("01_lp_numbers\\0x14692C3D.wav", "0x14692C3D", "lp_numbers"); } }
        public static ScannerFile HASH146FE1F7 { get { return new ScannerFile("01_lp_numbers\\0x146FE1F7.wav", "0x146FE1F7", "lp_numbers"); } }
        public static ScannerFile HASH14789B9E { get { return new ScannerFile("01_lp_numbers\\0x14789B9E.wav", "0x14789B9E", "lp_numbers"); } }
        public static ScannerFile HASH148662B3 { get { return new ScannerFile("01_lp_numbers\\0x148662B3.wav", "0x148662B3", "lp_numbers"); } }
        public static ScannerFile HASH14C60314 { get { return new ScannerFile("01_lp_numbers\\0x14C60314.wav", "0x14C60314", "lp_numbers"); } }
        public static ScannerFile HASH1504CC53 { get { return new ScannerFile("01_lp_numbers\\0x1504CC53.wav", "0x1504CC53", "lp_numbers"); } }
        public static ScannerFile HASH151F0732 { get { return new ScannerFile("01_lp_numbers\\0x151F0732.wav", "0x151F0732", "lp_numbers"); } }
        public static ScannerFile HASH158DD629 { get { return new ScannerFile("01_lp_numbers\\0x158DD629.wav", "0x158DD629", "lp_numbers"); } }
        public static ScannerFile HASH15BA4CFC { get { return new ScannerFile("01_lp_numbers\\0x15BA4CFC.wav", "0x15BA4CFC", "lp_numbers"); } }
        public static ScannerFile HASH15BB0EA5 { get { return new ScannerFile("01_lp_numbers\\0x15BB0EA5.wav", "0x15BB0EA5", "lp_numbers"); } }
        public static ScannerFile HASH15FED6C9 { get { return new ScannerFile("01_lp_numbers\\0x15FED6C9.wav", "0x15FED6C9", "lp_numbers"); } }
        public static ScannerFile HASH161DACCC { get { return new ScannerFile("01_lp_numbers\\0x161DACCC.wav", "0x161DACCC", "lp_numbers"); } }
        public static ScannerFile HASH1657B158 { get { return new ScannerFile("01_lp_numbers\\0x1657B158.wav", "0x1657B158", "lp_numbers"); } }
        public static ScannerFile HASH1669BA1A { get { return new ScannerFile("01_lp_numbers\\0x1669BA1A.wav", "0x1669BA1A", "lp_numbers"); } }
        public static ScannerFile HASH16A14539 { get { return new ScannerFile("01_lp_numbers\\0x16A14539.wav", "0x16A14539", "lp_numbers"); } }
        public static ScannerFile HASH16BEAFA4 { get { return new ScannerFile("01_lp_numbers\\0x16BEAFA4.wav", "0x16BEAFA4", "lp_numbers"); } }
        public static ScannerFile HASH16ECC442 { get { return new ScannerFile("01_lp_numbers\\0x16ECC442.wav", "0x16ECC442", "lp_numbers"); } }
        public static ScannerFile HASH18093005 { get { return new ScannerFile("01_lp_numbers\\0x18093005.wav", "0x18093005", "lp_numbers"); } }
        public static ScannerFile HASH182B594D { get { return new ScannerFile("01_lp_numbers\\0x182B594D.wav", "0x182B594D", "lp_numbers"); } }
        public static ScannerFile HASH188A2706 { get { return new ScannerFile("01_lp_numbers\\0x188A2706.wav", "0x188A2706", "lp_numbers"); } }
        public static ScannerFile HASH188F221E { get { return new ScannerFile("01_lp_numbers\\0x188F221E.wav", "0x188F221E", "lp_numbers"); } }
        public static ScannerFile HASH18A43F66 { get { return new ScannerFile("01_lp_numbers\\0x18A43F66.wav", "0x18A43F66", "lp_numbers"); } }
        public static ScannerFile HASH1900D63B { get { return new ScannerFile("01_lp_numbers\\0x1900D63B.wav", "0x1900D63B", "lp_numbers"); } }
        public static ScannerFile HASH192C90F3 { get { return new ScannerFile("01_lp_numbers\\0x192C90F3.wav", "0x192C90F3", "lp_numbers"); } }
        public static ScannerFile HASH1952F85E { get { return new ScannerFile("01_lp_numbers\\0x1952F85E.wav", "0x1952F85E", "lp_numbers"); } }
        public static ScannerFile HASH195A091D { get { return new ScannerFile("01_lp_numbers\\0x195A091D.wav", "0x195A091D", "lp_numbers"); } }
        public static ScannerFile HASH196A34A7 { get { return new ScannerFile("01_lp_numbers\\0x196A34A7.wav", "0x196A34A7", "lp_numbers"); } }
        public static ScannerFile HASH1970FBA7 { get { return new ScannerFile("01_lp_numbers\\0x1970FBA7.wav", "0x1970FBA7", "lp_numbers"); } }
        public static ScannerFile HASH197574B5 { get { return new ScannerFile("01_lp_numbers\\0x197574B5.wav", "0x197574B5", "lp_numbers"); } }
        public static ScannerFile HASH19984137 { get { return new ScannerFile("01_lp_numbers\\0x19984137.wav", "0x19984137", "lp_numbers"); } }
        public static ScannerFile HASH19E87FDC { get { return new ScannerFile("01_lp_numbers\\0x19E87FDC.wav", "0x19E87FDC", "lp_numbers"); } }
        public static ScannerFile HASH1A41A730 { get { return new ScannerFile("01_lp_numbers\\0x1A41A730.wav", "0x1A41A730", "lp_numbers"); } }
        public static ScannerFile HASH1A580D70 { get { return new ScannerFile("01_lp_numbers\\0x1A580D70.wav", "0x1A580D70", "lp_numbers"); } }
        public static ScannerFile HASH1A9A4FD7 { get { return new ScannerFile("01_lp_numbers\\0x1A9A4FD7.wav", "0x1A9A4FD7", "lp_numbers"); } }
        public static ScannerFile HASH1AF8440B { get { return new ScannerFile("01_lp_numbers\\0x1AF8440B.wav", "0x1AF8440B", "lp_numbers"); } }
        public static ScannerFile HASH1B3B522D { get { return new ScannerFile("01_lp_numbers\\0x1B3B522D.wav", "0x1B3B522D", "lp_numbers"); } }
        public static ScannerFile HASH1B5AC892 { get { return new ScannerFile("01_lp_numbers\\0x1B5AC892.wav", "0x1B5AC892", "lp_numbers"); } }
        public static ScannerFile HASH1B5E13AF { get { return new ScannerFile("01_lp_numbers\\0x1B5E13AF.wav", "0x1B5E13AF", "lp_numbers"); } }
        public static ScannerFile HASH1C30743F { get { return new ScannerFile("01_lp_numbers\\0x1C30743F.wav", "0x1C30743F", "lp_numbers"); } }
        public static ScannerFile HASH1CCC3889 { get { return new ScannerFile("01_lp_numbers\\0x1CCC3889.wav", "0x1CCC3889", "lp_numbers"); } }
        public static ScannerFile HASH1D20A337 { get { return new ScannerFile("01_lp_numbers\\0x1D20A337.wav", "0x1D20A337", "lp_numbers"); } }
        public static ScannerFile HASH1D3F9FF7 { get { return new ScannerFile("01_lp_numbers\\0x1D3F9FF7.wav", "0x1D3F9FF7", "lp_numbers"); } }
        public static ScannerFile HASH1D49BE35 { get { return new ScannerFile("01_lp_numbers\\0x1D49BE35.wav", "0x1D49BE35", "lp_numbers"); } }
        public static ScannerFile HASH1DA41775 { get { return new ScannerFile("01_lp_numbers\\0x1DA41775.wav", "0x1DA41775", "lp_numbers"); } }
        public static ScannerFile HASH1DC8568D { get { return new ScannerFile("01_lp_numbers\\0x1DC8568D.wav", "0x1DC8568D", "lp_numbers"); } }
        public static ScannerFile HASH1DEE5E25 { get { return new ScannerFile("01_lp_numbers\\0x1DEE5E25.wav", "0x1DEE5E25", "lp_numbers"); } }
        public static ScannerFile HASH1DF45254 { get { return new ScannerFile("01_lp_numbers\\0x1DF45254.wav", "0x1DF45254", "lp_numbers"); } }
        public static ScannerFile HASH1E1EA590 { get { return new ScannerFile("01_lp_numbers\\0x1E1EA590.wav", "0x1E1EA590", "lp_numbers"); } }
        public static ScannerFile HASH1E3F03C2 { get { return new ScannerFile("01_lp_numbers\\0x1E3F03C2.wav", "0x1E3F03C2", "lp_numbers"); } }
        public static ScannerFile HASH1E7FA242 { get { return new ScannerFile("01_lp_numbers\\0x1E7FA242.wav", "0x1E7FA242", "lp_numbers"); } }
        public static ScannerFile HASH1EF897F4 { get { return new ScannerFile("01_lp_numbers\\0x1EF897F4.wav", "0x1EF897F4", "lp_numbers"); } }
        public static ScannerFile HASH1F858981 { get { return new ScannerFile("01_lp_numbers\\0x1F858981.wav", "0x1F858981", "lp_numbers"); } }
        public static ScannerFile HASH1F869BAC { get { return new ScannerFile("01_lp_numbers\\0x1F869BAC.wav", "0x1F869BAC", "lp_numbers"); } }
    }
    public class manufacturer
    {
        public static ScannerFile Thrifter { get { return new ScannerFile("01_manufacturer\\0x0A69F4AC.wav", "Thrifter(?)", "manufacturer"); } }
        public static ScannerFile ALBANY01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_ALBANY_01.wav", "MANUFACTURER_ALBANY_01", "manufacturer"); } }
        public static ScannerFile ANNIS01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_ANNIS_01.wav", "MANUFACTURER_ANNIS_01", "manufacturer"); } }
        public static ScannerFile ARMY01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_ARMY_01.wav", "MANUFACTURER_ARMY_01", "manufacturer"); } }
        public static ScannerFile BENEFACTOR01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_BENEFACTOR_01.wav", "MANUFACTURER_BENEFACTOR_01", "manufacturer"); } }
        public static ScannerFile BF01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_BF_01.wav", "MANUFACTURER_BF_01", "manufacturer"); } }
        public static ScannerFile BOLLOKAN01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_BOLLOKAN_01.wav", "MANUFACTURER_BOLLOKAN_01", "manufacturer"); } }
        public static ScannerFile BRAVADO01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_BRAVADO_01.wav", "MANUFACTURER_BRAVADO_01", "manufacturer"); } }
        public static ScannerFile BRUTE01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_BRUTE_01.wav", "MANUFACTURER_BRUTE_01", "manufacturer"); } }
        public static ScannerFile CANIS01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_CANIS_01.wav", "MANUFACTURER_CANIS_01", "manufacturer"); } }
        public static ScannerFile CHARIOT01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_CHARIOT_01.wav", "MANUFACTURER_CHARIOT_01", "manufacturer"); } }
        public static ScannerFile CHEVAL01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_CHEVAL_01.wav", "MANUFACTURER_CHEVAL_01", "manufacturer"); } }
        public static ScannerFile CLASSIQUE01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_CLASSIQUE_01.wav", "MANUFACTURER_CLASSIQUE_01", "manufacturer"); } }
        public static ScannerFile COIL01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_COIL_01.wav", "MANUFACTURER_COIL_01", "manufacturer"); } }
        public static ScannerFile CUSTOM01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_CUSTOM_01.wav", "MANUFACTURER_CUSTOM_01", "manufacturer"); } }
        public static ScannerFile DECLASSE01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_DECLASSE_01.wav", "MANUFACTURER_DECLASSE_01", "manufacturer"); } }
        public static ScannerFile DEWBAUCHEE01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_DEWBAUCHEE_01.wav", "MANUFACTURER_DEWBAUCHEE_01", "manufacturer"); } }
        public static ScannerFile DINKA01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_DINKA_01.wav", "MANUFACTURER_DINKA_01", "manufacturer"); } }
        public static ScannerFile DUNDREARY01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_DUNDREARY_01.wav", "MANUFACTURER_DUNDREARY_01", "manufacturer"); } }
        public static ScannerFile EMERGENCY01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_EMERGENCY_01.wav", "MANUFACTURER_EMERGENCY_01", "manufacturer"); } }
        public static ScannerFile EMPEROR01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_EMPEROR_01.wav", "MANUFACTURER_EMPEROR_01", "manufacturer"); } }
        public static ScannerFile EMPORER01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_EMPORER_01.wav", "MANUFACTURER_EMPORER_01", "manufacturer"); } }
        public static ScannerFile ENUS01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_ENUS_01.wav", "MANUFACTURER_ENUS_01", "manufacturer"); } }
        public static ScannerFile FATHOM01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_FATHOM_01.wav", "MANUFACTURER_FATHOM_01", "manufacturer"); } }
        public static ScannerFile FIB01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_FIB_01.wav", "MANUFACTURER_FIB_01", "manufacturer"); } }
        public static ScannerFile GALLIVANTER01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_GALLIVANTER_01.wav", "MANUFACTURER_GALLIVANTER_01", "manufacturer"); } }
        public static ScannerFile GOPOSTAL01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_GO_POSTAL_01.wav", "MANUFACTURER_GO_POSTAL_01", "manufacturer"); } }
        public static ScannerFile GROTTI01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_GROTTI_01.wav", "MANUFACTURER_GROTTI_01", "manufacturer"); } }
        public static ScannerFile HIJAK01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_HIJAK_01.wav", "MANUFACTURER_HIJAK_01", "manufacturer"); } }
        public static ScannerFile HVY01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_HVY_01.wav", "MANUFACTURER_HVY_01", "manufacturer"); } }
        public static ScannerFile IMPONTE01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_IMPONTE_01.wav", "MANUFACTURER_IMPONTE_01", "manufacturer"); } }
        public static ScannerFile INVETERO01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_INVETERO_01.wav", "MANUFACTURER_INVETERO_01", "manufacturer"); } }
        public static ScannerFile JACKSHEEPE01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_JACK_SHEEPE_01.wav", "MANUFACTURER_JACK_SHEEPE_01", "manufacturer"); } }
        public static ScannerFile JOEBUILT01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_JOEBUILT_01.wav", "MANUFACTURER_JOEBUILT_01", "manufacturer"); } }
        public static ScannerFile KARIN01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_KARIN_01.wav", "MANUFACTURER_KARIN_01", "manufacturer"); } }
        public static ScannerFile LAMPADATI01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_LAMPADATI_01.wav", "MANUFACTURER_LAMPADATI_01", "manufacturer"); } }
        public static ScannerFile MAIBATSU01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_MAIBATSU_01.wav", "MANUFACTURER_MAIBATSU_01", "manufacturer"); } }
        public static ScannerFile MAMMOTH01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_MAMMOTH_01.wav", "MANUFACTURER_MAMMOTH_01", "manufacturer"); } }
        public static ScannerFile MILITARY01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_MILITARY_01.wav", "MANUFACTURER_MILITARY_01", "manufacturer"); } }
        public static ScannerFile MOTORCYCLEWCOMPANY01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_MOTORCYCLE_W_COMPANY_01.wav", "MANUFACTURER_MOTORCYCLE_W_COMPANY_01", "manufacturer"); } }
        public static ScannerFile MTL01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_MTL_01.wav", "MANUFACTURER_MTL_01", "manufacturer"); } }
        public static ScannerFile NAGASAKI01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_NAGASAKI_01.wav", "MANUFACTURER_NAGASAKI_01", "manufacturer"); } }
        public static ScannerFile OBEY01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_OBEY_01.wav", "MANUFACTURER_OBEY_01", "manufacturer"); } }
        public static ScannerFile OCELOT01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_OCELOT_01.wav", "MANUFACTURER_OCELOT_01", "manufacturer"); } }
        public static ScannerFile OVERFLOD01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_OVERFLOD_01.wav", "MANUFACTURER_OVERFLOD_01", "manufacturer"); } }
        public static ScannerFile PEGASI01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_PEGASI_01.wav", "MANUFACTURER_PEGASI_01", "manufacturer"); } }
        public static ScannerFile POLICE01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_POLICE_01.wav", "MANUFACTURER_POLICE_01", "manufacturer"); } }
        public static ScannerFile PRINCIPE01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_PRINCIPE_01.wav", "MANUFACTURER_PRINCIPE_01", "manufacturer"); } }
        public static ScannerFile PROGEN01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_PROGEN_01.wav", "MANUFACTURER_PROGEN_01", "manufacturer"); } }
        public static ScannerFile SCHYSTER01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_SCHYSTER_01.wav", "MANUFACTURER_SCHYSTER_01", "manufacturer"); } }
        public static ScannerFile SHITZU01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_SHITZU_01.wav", "MANUFACTURER_SHITZU_01", "manufacturer"); } }
        public static ScannerFile SKIVER01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_SKIVER_01.wav", "MANUFACTURER_SKIVER_01", "manufacturer"); } }
        public static ScannerFile SPEEDOPHILE01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_SPEEDOPHILE_01.wav", "MANUFACTURER_SPEEDOPHILE_01", "manufacturer"); } }
        public static ScannerFile STANLEY01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_STANLEY_01.wav", "MANUFACTURER_STANLEY_01", "manufacturer"); } }
        public static ScannerFile STEELHORSE01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_STEEL_HORSE_01.wav", "MANUFACTURER_STEEL_HORSE_01", "manufacturer"); } }
        public static ScannerFile TRUFFADE01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_TRUFFADE_01.wav", "MANUFACTURER_TRUFFADE_01", "manufacturer"); } }
        public static ScannerFile UBERMACHT01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_UBERMACHT_01.wav", "MANUFACTURER_UBERMACHT_01", "manufacturer"); } }
        public static ScannerFile VAPID01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_VAPID_01.wav", "MANUFACTURER_VAPID_01", "manufacturer"); } }
        public static ScannerFile VULCAR01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_VULCAR_01.wav", "MANUFACTURER_VULCAR_01", "manufacturer"); } }
        public static ScannerFile WEENY01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_WEENY_01.wav", "MANUFACTURER_WEENY_01", "manufacturer"); } }
        public static ScannerFile WESTERNCOMPANY01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_WESTERN_COMPANY_01.wav", "MANUFACTURER_WESTERN_COMPANY_01", "manufacturer"); } }
        public static ScannerFile WESTERNMOTORCYCLECOMPANY01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_WESTERN_MOTORCYCLE_COMPANY_01.wav", "MANUFACTURER_WESTERN_MOTORCYCLE_COMPANY_01", "manufacturer"); } }
        public static ScannerFile WILLARD01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_WILLARD_01.wav", "MANUFACTURER_WILLARD_01", "manufacturer"); } }
        public static ScannerFile WMC01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_WMC_01.wav", "MANUFACTURER_WMC_01", "manufacturer"); } }
        public static ScannerFile ZIRCONIUM01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_ZIRCONIUM_01.wav", "MANUFACTURER_ZIRCONIUM_01", "manufacturer"); } }
        public static ScannerFile ZIREONIUM01 { get { return new ScannerFile("01_manufacturer\\MANUFACTURER_ZIREONIUM_01.wav", "MANUFACTURER_ZIREONIUM_01", "manufacturer"); } }
    }
    public class model
    {
        public static ScannerFile HASH071B9975 { get { return new ScannerFile("01_model\\0x071B9975.wav", "0x071B9975", "model"); } }
        public static ScannerFile ADDER01 { get { return new ScannerFile("01_model\\MODEL_ADDER_01.wav", "MODEL_ADDER_01", "model"); } }
        public static ScannerFile AIRSHIP01 { get { return new ScannerFile("01_model\\MODEL_AIRSHIP_01.wav", "MODEL_AIRSHIP_01", "model"); } }
        public static ScannerFile AIRTUG01 { get { return new ScannerFile("01_model\\MODEL_AIRTUG_01.wav", "MODEL_AIRTUG_01", "model"); } }
        public static ScannerFile AKUMA01 { get { return new ScannerFile("01_model\\MODEL_AKUMA_01.wav", "MODEL_AKUMA_01", "model"); } }
        public static ScannerFile AMBULANCE01 { get { return new ScannerFile("01_model\\MODEL_AMBULANCE_01.wav", "MODEL_AMBULANCE_01", "model"); } }
        public static ScannerFile ANNIHILATOR01 { get { return new ScannerFile("01_model\\MODEL_ANNIHILATOR_01.wav", "MODEL_ANNIHILATOR_01", "model"); } }
        public static ScannerFile ASEA01 { get { return new ScannerFile("01_model\\MODEL_ASEA_01.wav", "MODEL_ASEA_01", "model"); } }
        public static ScannerFile ASTEROPE01 { get { return new ScannerFile("01_model\\MODEL_ASTEROPE_01.wav", "MODEL_ASTEROPE_01", "model"); } }
        public static ScannerFile BAGGER01 { get { return new ScannerFile("01_model\\MODEL_BAGGER_01.wav", "MODEL_BAGGER_01", "model"); } }
        public static ScannerFile BALLER01 { get { return new ScannerFile("01_model\\MODEL_BALLER_01.wav", "MODEL_BALLER_01", "model"); } }
        public static ScannerFile BANSHEE01 { get { return new ScannerFile("01_model\\MODEL_BANSHEE_01.wav", "MODEL_BANSHEE_01", "model"); } }
        public static ScannerFile BARRACKS01 { get { return new ScannerFile("01_model\\MODEL_BARRACKS_01.wav", "MODEL_BARRACKS_01", "model"); } }
        public static ScannerFile BATI01 { get { return new ScannerFile("01_model\\MODEL_BATI_01.wav", "MODEL_BATI_01", "model"); } }
        public static ScannerFile BENSON01 { get { return new ScannerFile("01_model\\MODEL_BENSON_01.wav", "MODEL_BENSON_01", "model"); } }
        public static ScannerFile BFINJECTION01 { get { return new ScannerFile("01_model\\MODEL_BF_INJECTION_01.wav", "MODEL_BF_INJECTION_01", "model"); } }
        public static ScannerFile BIFF01 { get { return new ScannerFile("01_model\\MODEL_BIFF_01.wav", "MODEL_BIFF_01", "model"); } }
        public static ScannerFile BISON01 { get { return new ScannerFile("01_model\\MODEL_BISON_01.wav", "MODEL_BISON_01", "model"); } }
        public static ScannerFile BJXL01 { get { return new ScannerFile("01_model\\MODEL_BJXL_01.wav", "MODEL_BJXL_01", "model"); } }
        public static ScannerFile BLAZER01 { get { return new ScannerFile("01_model\\MODEL_BLAZER_01.wav", "MODEL_BLAZER_01", "model"); } }
        public static ScannerFile BLIMP01 { get { return new ScannerFile("01_model\\MODEL_BLIMP_01.wav", "MODEL_BLIMP_01", "model"); } }
        public static ScannerFile BLISTA01 { get { return new ScannerFile("01_model\\MODEL_BLISTA_01.wav", "MODEL_BLISTA_01", "model"); } }
        public static ScannerFile BMX01 { get { return new ScannerFile("01_model\\MODEL_BMX_01.wav", "MODEL_BMX_01", "model"); } }
        public static ScannerFile BOBCAT01 { get { return new ScannerFile("01_model\\MODEL_BOBCAT_01.wav", "MODEL_BOBCAT_01", "model"); } }
        public static ScannerFile BOBCATXL01 { get { return new ScannerFile("01_model\\MODEL_BOBCAT_XL_01.wav", "MODEL_BOBCAT_XL_01", "model"); } }
        public static ScannerFile BODHI01 { get { return new ScannerFile("01_model\\MODEL_BODHI_01.wav", "MODEL_BODHI_01", "model"); } }
        public static ScannerFile BOXVILLE01 { get { return new ScannerFile("01_model\\MODEL_BOXVILLE_01.wav", "MODEL_BOXVILLE_01", "model"); } }
        public static ScannerFile BUCCANEER01 { get { return new ScannerFile("01_model\\MODEL_BUCCANEER_01.wav", "MODEL_BUCCANEER_01", "model"); } }
        public static ScannerFile BUFFALO01 { get { return new ScannerFile("01_model\\MODEL_BUFFALO_01.wav", "MODEL_BUFFALO_01", "model"); } }
        public static ScannerFile BULLDOZER01 { get { return new ScannerFile("01_model\\MODEL_BULLDOZER_01.wav", "MODEL_BULLDOZER_01", "model"); } }
        public static ScannerFile BULLET01 { get { return new ScannerFile("01_model\\MODEL_BULLET_01.wav", "MODEL_BULLET_01", "model"); } }
        public static ScannerFile BULLETGT01 { get { return new ScannerFile("01_model\\MODEL_BULLET_GT_01.wav", "MODEL_BULLET_GT_01", "model"); } }
        public static ScannerFile BURRITO01 { get { return new ScannerFile("01_model\\MODEL_BURRITO_01.wav", "MODEL_BURRITO_01", "model"); } }
        public static ScannerFile BUS01 { get { return new ScannerFile("01_model\\MODEL_BUS_01.wav", "MODEL_BUS_01", "model"); } }
        public static ScannerFile BUZZARD01 { get { return new ScannerFile("01_model\\MODEL_BUZZARD_01.wav", "MODEL_BUZZARD_01", "model"); } }
        public static ScannerFile CADDY01 { get { return new ScannerFile("01_model\\MODEL_CADDY_01.wav", "MODEL_CADDY_01", "model"); } }
        public static ScannerFile CAMPER01 { get { return new ScannerFile("01_model\\MODEL_CAMPER_01.wav", "MODEL_CAMPER_01", "model"); } }
        public static ScannerFile CARBONIZZARE01 { get { return new ScannerFile("01_model\\MODEL_CARBONIZZARE_01.wav", "MODEL_CARBONIZZARE_01", "model"); } }
        public static ScannerFile CARBONRS01 { get { return new ScannerFile("01_model\\MODEL_CARBON_RS_01.wav", "MODEL_CARBON_RS_01", "model"); } }
        public static ScannerFile CARGOBOB01 { get { return new ScannerFile("01_model\\MODEL_CARGOBOB_01.wav", "MODEL_CARGOBOB_01", "model"); } }
        public static ScannerFile CARGOPLANE01 { get { return new ScannerFile("01_model\\MODEL_CARGO_PLANE_01.wav", "MODEL_CARGO_PLANE_01", "model"); } }
        public static ScannerFile CARGOPLANE02 { get { return new ScannerFile("01_model\\MODEL_CARGO_PLANE_02.wav", "MODEL_CARGO_PLANE_02", "model"); } }
        public static ScannerFile CAVALCADE01 { get { return new ScannerFile("01_model\\MODEL_CAVALCADE_01.wav", "MODEL_CAVALCADE_01", "model"); } }
        public static ScannerFile CEMENTMIXER01 { get { return new ScannerFile("01_model\\MODEL_CEMENT_MIXER_01.wav", "MODEL_CEMENT_MIXER_01", "model"); } }
        public static ScannerFile CHEETAH01 { get { return new ScannerFile("01_model\\MODEL_CHEETAH_01.wav", "MODEL_CHEETAH_01", "model"); } }
        public static ScannerFile COACH01 { get { return new ScannerFile("01_model\\MODEL_COACH_01.wav", "MODEL_COACH_01", "model"); } }
        public static ScannerFile COGNOSCENTI01 { get { return new ScannerFile("01_model\\MODEL_COGNOSCENTI_01.wav", "MODEL_COGNOSCENTI_01", "model"); } }
        public static ScannerFile COG5501 { get { return new ScannerFile("01_model\\MODEL_COG_55_01.wav", "MODEL_COG_55_01", "model"); } }
        public static ScannerFile COGCABRIO01 { get { return new ScannerFile("01_model\\MODEL_COG_CABRIO_01.wav", "MODEL_COG_CABRIO_01", "model"); } }
        public static ScannerFile COMET01 { get { return new ScannerFile("01_model\\MODEL_COMET_01.wav", "MODEL_COMET_01", "model"); } }
        public static ScannerFile COQUETTE01 { get { return new ScannerFile("01_model\\MODEL_COQUETTE_01.wav", "MODEL_COQUETTE_01", "model"); } }
        public static ScannerFile CRUISER01 { get { return new ScannerFile("01_model\\MODEL_CRUISER_01.wav", "MODEL_CRUISER_01", "model"); } }
        public static ScannerFile CRUSADER01 { get { return new ScannerFile("01_model\\MODEL_CRUSADER_01.wav", "MODEL_CRUSADER_01", "model"); } }
        public static ScannerFile CUBAN80001 { get { return new ScannerFile("01_model\\MODEL_CUBAN_800_01.wav", "MODEL_CUBAN_800_01", "model"); } }
        public static ScannerFile CUTTER01 { get { return new ScannerFile("01_model\\MODEL_CUTTER_01.wav", "MODEL_CUTTER_01", "model"); } }
        public static ScannerFile DAEMON01 { get { return new ScannerFile("01_model\\MODEL_DAEMON_01.wav", "MODEL_DAEMON_01", "model"); } }
        public static ScannerFile DIGGER01 { get { return new ScannerFile("01_model\\MODEL_DIGGER_01.wav", "MODEL_DIGGER_01", "model"); } }
        public static ScannerFile DILETTANTE01 { get { return new ScannerFile("01_model\\MODEL_DILETTANTE_01.wav", "MODEL_DILETTANTE_01", "model"); } }
        public static ScannerFile DINGHY01 { get { return new ScannerFile("01_model\\MODEL_DINGHY_01.wav", "MODEL_DINGHY_01", "model"); } }
        public static ScannerFile DOCKTUG01 { get { return new ScannerFile("01_model\\MODEL_DOCK_TUG_01.wav", "MODEL_DOCK_TUG_01", "model"); } }
        public static ScannerFile DOMINATOR01 { get { return new ScannerFile("01_model\\MODEL_DOMINATOR_01.wav", "MODEL_DOMINATOR_01", "model"); } }
        public static ScannerFile DOUBLE01 { get { return new ScannerFile("01_model\\MODEL_DOUBLE_01.wav", "MODEL_DOUBLE_01", "model"); } }
        public static ScannerFile DOUBLET01 { get { return new ScannerFile("01_model\\MODEL_DOUBLE_T_01.wav", "MODEL_DOUBLE_T_01", "model"); } }
        public static ScannerFile DUBSTA01 { get { return new ScannerFile("01_model\\MODEL_DUBSTA_01.wav", "MODEL_DUBSTA_01", "model"); } }
        public static ScannerFile DUKES01 { get { return new ScannerFile("01_model\\MODEL_DUKES_01.wav", "MODEL_DUKES_01", "model"); } }
        public static ScannerFile DUMPER01 { get { return new ScannerFile("01_model\\MODEL_DUMPER_01.wav", "MODEL_DUMPER_01", "model"); } }
        public static ScannerFile DUMP01 { get { return new ScannerFile("01_model\\MODEL_DUMP_01.wav", "MODEL_DUMP_01", "model"); } }
        public static ScannerFile DUNELOADER01 { get { return new ScannerFile("01_model\\MODEL_DUNELOADER_01.wav", "MODEL_DUNELOADER_01", "model"); } }
        public static ScannerFile DUNE01 { get { return new ScannerFile("01_model\\MODEL_DUNE_01.wav", "MODEL_DUNE_01", "model"); } }
        public static ScannerFile DUNEBUGGY01 { get { return new ScannerFile("01_model\\MODEL_DUNE_BUGGY_01.wav", "MODEL_DUNE_BUGGY_01", "model"); } }
        public static ScannerFile DUSTER01 { get { return new ScannerFile("01_model\\MODEL_DUSTER_01.wav", "MODEL_DUSTER_01", "model"); } }
        public static ScannerFile ELEGY01 { get { return new ScannerFile("01_model\\MODEL_ELEGY_01.wav", "MODEL_ELEGY_01", "model"); } }
        public static ScannerFile EMPEROR01 { get { return new ScannerFile("01_model\\MODEL_EMPEROR_01.wav", "MODEL_EMPEROR_01", "model"); } }
        public static ScannerFile ENTITYXF01 { get { return new ScannerFile("01_model\\MODEL_ENTITY_XF_01.wav", "MODEL_ENTITY_XF_01", "model"); } }
        public static ScannerFile EOD01 { get { return new ScannerFile("01_model\\MODEL_EOD_01.wav", "MODEL_EOD_01", "model"); } }
        public static ScannerFile EXEMPLAR01 { get { return new ScannerFile("01_model\\MODEL_EXEMPLAR_01.wav", "MODEL_EXEMPLAR_01", "model"); } }
        public static ScannerFile F62001 { get { return new ScannerFile("01_model\\MODEL_F620_01.wav", "MODEL_F620_01", "model"); } }
        public static ScannerFile FACTION01 { get { return new ScannerFile("01_model\\MODEL_FACTION_01.wav", "MODEL_FACTION_01", "model"); } }
        public static ScannerFile FAGGIO01 { get { return new ScannerFile("01_model\\MODEL_FAGGIO_01.wav", "MODEL_FAGGIO_01", "model"); } }
        public static ScannerFile FELON01 { get { return new ScannerFile("01_model\\MODEL_FELON_01.wav", "MODEL_FELON_01", "model"); } }
        public static ScannerFile FELTZER01 { get { return new ScannerFile("01_model\\MODEL_FELTZER_01.wav", "MODEL_FELTZER_01", "model"); } }
        public static ScannerFile FEROCI01 { get { return new ScannerFile("01_model\\MODEL_FEROCI_01.wav", "MODEL_FEROCI_01", "model"); } }
        public static ScannerFile FIELDMASTER01 { get { return new ScannerFile("01_model\\MODEL_FIELDMASTER_01.wav", "MODEL_FIELDMASTER_01", "model"); } }
        public static ScannerFile FIRETRUCK01 { get { return new ScannerFile("01_model\\MODEL_FIRETRUCK_01.wav", "MODEL_FIRETRUCK_01", "model"); } }
        public static ScannerFile FLATBED01 { get { return new ScannerFile("01_model\\MODEL_FLATBED_01.wav", "MODEL_FLATBED_01", "model"); } }
        public static ScannerFile FORKLIFT01 { get { return new ScannerFile("01_model\\MODEL_FORKLIFT_01.wav", "MODEL_FORKLIFT_01", "model"); } }
        public static ScannerFile FQ201 { get { return new ScannerFile("01_model\\MODEL_FQ2_01.wav", "MODEL_FQ2_01", "model"); } }
        public static ScannerFile FREIGHT01 { get { return new ScannerFile("01_model\\MODEL_FREIGHT_01.wav", "MODEL_FREIGHT_01", "model"); } }
        public static ScannerFile FROGGER01 { get { return new ScannerFile("01_model\\MODEL_FROGGER_01.wav", "MODEL_FROGGER_01", "model"); } }
        public static ScannerFile FUGITIVE01 { get { return new ScannerFile("01_model\\MODEL_FUGITIVE_01.wav", "MODEL_FUGITIVE_01", "model"); } }
        public static ScannerFile FUSILADE01 { get { return new ScannerFile("01_model\\MODEL_FUSILADE_01.wav", "MODEL_FUSILADE_01", "model"); } }
        public static ScannerFile FUTO01 { get { return new ScannerFile("01_model\\MODEL_FUTO_01.wav", "MODEL_FUTO_01", "model"); } }
        public static ScannerFile GAUNTLET01 { get { return new ScannerFile("01_model\\MODEL_GAUNTLET_01.wav", "MODEL_GAUNTLET_01", "model"); } }
        public static ScannerFile GRANGER01 { get { return new ScannerFile("01_model\\MODEL_GRANGER_01.wav", "MODEL_GRANGER_01", "model"); } }
        public static ScannerFile GRESLEY01 { get { return new ScannerFile("01_model\\MODEL_GRESLEY_01.wav", "MODEL_GRESLEY_01", "model"); } }
        public static ScannerFile HABANERO01 { get { return new ScannerFile("01_model\\MODEL_HABANERO_01.wav", "MODEL_HABANERO_01", "model"); } }
        public static ScannerFile HAKUMAI01 { get { return new ScannerFile("01_model\\MODEL_HAKUMAI_01.wav", "MODEL_HAKUMAI_01", "model"); } }
        public static ScannerFile HANDLER01 { get { return new ScannerFile("01_model\\MODEL_HANDLER_01.wav", "MODEL_HANDLER_01", "model"); } }
        public static ScannerFile HAULER01 { get { return new ScannerFile("01_model\\MODEL_HAULER_01.wav", "MODEL_HAULER_01", "model"); } }
        public static ScannerFile HEARSE01 { get { return new ScannerFile("01_model\\MODEL_HEARSE_01.wav", "MODEL_HEARSE_01", "model"); } }
        public static ScannerFile HELLFURY01 { get { return new ScannerFile("01_model\\MODEL_HELLFURY_01.wav", "MODEL_HELLFURY_01", "model"); } }
        public static ScannerFile HEXER01 { get { return new ScannerFile("01_model\\MODEL_HEXER_01.wav", "MODEL_HEXER_01", "model"); } }
        public static ScannerFile HOTKNIFE01 { get { return new ScannerFile("01_model\\MODEL_HOT_KNIFE_01.wav", "MODEL_HOT_KNIFE_01", "model"); } }
        public static ScannerFile HUNTER01 { get { return new ScannerFile("01_model\\MODEL_HUNTER_01.wav", "MODEL_HUNTER_01", "model"); } }
        public static ScannerFile INFERNUS01 { get { return new ScannerFile("01_model\\MODEL_INFERNUS_01.wav", "MODEL_INFERNUS_01", "model"); } }
        public static ScannerFile INGOT01 { get { return new ScannerFile("01_model\\MODEL_INGOT_01.wav", "MODEL_INGOT_01", "model"); } }
        public static ScannerFile INTRUDER01 { get { return new ScannerFile("01_model\\MODEL_INTRUDER_01.wav", "MODEL_INTRUDER_01", "model"); } }
        public static ScannerFile ISSI01 { get { return new ScannerFile("01_model\\MODEL_ISSI_01.wav", "MODEL_ISSI_01", "model"); } }
        public static ScannerFile JACKAL01 { get { return new ScannerFile("01_model\\MODEL_JACKAL_01.wav", "MODEL_JACKAL_01", "model"); } }
        public static ScannerFile JB70001 { get { return new ScannerFile("01_model\\MODEL_JB700_01.wav", "MODEL_JB700_01", "model"); } }
        public static ScannerFile JETMAX01 { get { return new ScannerFile("01_model\\MODEL_JETMAX_01.wav", "MODEL_JETMAX_01", "model"); } }
        public static ScannerFile JET01 { get { return new ScannerFile("01_model\\MODEL_JET_01.wav", "MODEL_JET_01", "model"); } }
        public static ScannerFile JOURNEY01 { get { return new ScannerFile("01_model\\MODEL_JOURNEY_01.wav", "MODEL_JOURNEY_01", "model"); } }
        public static ScannerFile KHAMELION01 { get { return new ScannerFile("01_model\\MODEL_KHAMELION_01.wav", "MODEL_KHAMELION_01", "model"); } }
        public static ScannerFile LANDSTALKER01 { get { return new ScannerFile("01_model\\MODEL_LANDSTALKER_01.wav", "MODEL_LANDSTALKER_01", "model"); } }
        public static ScannerFile LAZER01 { get { return new ScannerFile("01_model\\MODEL_LAZER_01.wav", "MODEL_LAZER_01", "model"); } }
        public static ScannerFile LIFEGUARDGRANGER01 { get { return new ScannerFile("01_model\\MODEL_LIFEGUARD_GRANGER_01.wav", "MODEL_LIFEGUARD_GRANGER_01", "model"); } }
        public static ScannerFile MANANA01 { get { return new ScannerFile("01_model\\MODEL_MANANA_01.wav", "MODEL_MANANA_01", "model"); } }
        public static ScannerFile MAVERICK01 { get { return new ScannerFile("01_model\\MODEL_MAVERICK_01.wav", "MODEL_MAVERICK_01", "model"); } }
        public static ScannerFile MESA01 { get { return new ScannerFile("01_model\\MODEL_MESA_01.wav", "MODEL_MESA_01", "model"); } }
        public static ScannerFile METROTRAIN01 { get { return new ScannerFile("01_model\\MODEL_METROTRAIN_01.wav", "MODEL_METROTRAIN_01", "model"); } }
        public static ScannerFile MINIVAN01 { get { return new ScannerFile("01_model\\MODEL_MINIVAN_01.wav", "MODEL_MINIVAN_01", "model"); } }
        public static ScannerFile MIXER01 { get { return new ScannerFile("01_model\\MODEL_MIXER_01.wav", "MODEL_MIXER_01", "model"); } }
        public static ScannerFile MONROE01 { get { return new ScannerFile("01_model\\MODEL_MONROE_01.wav", "MODEL_MONROE_01", "model"); } }
        public static ScannerFile MOWER01 { get { return new ScannerFile("01_model\\MODEL_MOWER_01.wav", "MODEL_MOWER_01", "model"); } }
        public static ScannerFile MULE01 { get { return new ScannerFile("01_model\\MODEL_MULE_01.wav", "MODEL_MULE_01", "model"); } }
        public static ScannerFile NEMESIS01 { get { return new ScannerFile("01_model\\MODEL_NEMESIS_01.wav", "MODEL_NEMESIS_01", "model"); } }
        public static ScannerFile NINEF01 { get { return new ScannerFile("01_model\\MODEL_NINE_F_01.wav", "MODEL_NINE_F_01", "model"); } }
        public static ScannerFile ORACLE01 { get { return new ScannerFile("01_model\\MODEL_ORACLE_01.wav", "MODEL_ORACLE_01", "model"); } }
        public static ScannerFile PACKER01 { get { return new ScannerFile("01_model\\MODEL_PACKER_01.wav", "MODEL_PACKER_01", "model"); } }
        public static ScannerFile PATRIOT01 { get { return new ScannerFile("01_model\\MODEL_PATRIOT_01.wav", "MODEL_PATRIOT_01", "model"); } }
        public static ScannerFile PCJ60001 { get { return new ScannerFile("01_model\\MODEL_PCJ_600_01.wav", "MODEL_PCJ_600_01", "model"); } }
        public static ScannerFile PENUMBRA01 { get { return new ScannerFile("01_model\\MODEL_PENUMBRA_01.wav", "MODEL_PENUMBRA_01", "model"); } }
        public static ScannerFile PEYOTE01 { get { return new ScannerFile("01_model\\MODEL_PEYOTE_01.wav", "MODEL_PEYOTE_01", "model"); } }
        public static ScannerFile PHANTOM01 { get { return new ScannerFile("01_model\\MODEL_PHANTOM_01.wav", "MODEL_PHANTOM_01", "model"); } }
        public static ScannerFile PHOENIX01 { get { return new ScannerFile("01_model\\MODEL_PHOENIX_01.wav", "MODEL_PHOENIX_01", "model"); } }
        public static ScannerFile PICADOR01 { get { return new ScannerFile("01_model\\MODEL_PICADOR_01.wav", "MODEL_PICADOR_01", "model"); } }
        public static ScannerFile POLICECAR01 { get { return new ScannerFile("01_model\\MODEL_POLICE_CAR_01.wav", "MODEL_POLICE_CAR_01", "model"); } }
        public static ScannerFile POLICEFUGITIVE01 { get { return new ScannerFile("01_model\\MODEL_POLICE_FUGITIVE_01.wav", "MODEL_POLICE_FUGITIVE_01", "model"); } }
        public static ScannerFile POLICEMAVERICK01 { get { return new ScannerFile("01_model\\MODEL_POLICE_MAVERICK_01.wav", "MODEL_POLICE_MAVERICK_01", "model"); } }
        public static ScannerFile POLICETRANSPORT01 { get { return new ScannerFile("01_model\\MODEL_POLICE_TRANSPORT_01.wav", "MODEL_POLICE_TRANSPORT_01", "model"); } }
        public static ScannerFile PONY01 { get { return new ScannerFile("01_model\\MODEL_PONY_01.wav", "MODEL_PONY_01", "model"); } }
        public static ScannerFile POUNDER01 { get { return new ScannerFile("01_model\\MODEL_POUNDER_01.wav", "MODEL_POUNDER_01", "model"); } }
        public static ScannerFile PRAIRIE01 { get { return new ScannerFile("01_model\\MODEL_PRAIRIE_01.wav", "MODEL_PRAIRIE_01", "model"); } }
        public static ScannerFile PREDATOR01 { get { return new ScannerFile("01_model\\MODEL_PREDATOR_01.wav", "MODEL_PREDATOR_01", "model"); } }
        public static ScannerFile PREMIER01 { get { return new ScannerFile("01_model\\MODEL_PREMIER_01.wav", "MODEL_PREMIER_01", "model"); } }
        public static ScannerFile PRIMO01 { get { return new ScannerFile("01_model\\MODEL_PRIMO_01.wav", "MODEL_PRIMO_01", "model"); } }
        public static ScannerFile RADIUS01 { get { return new ScannerFile("01_model\\MODEL_RADIUS_01.wav", "MODEL_RADIUS_01", "model"); } }
        public static ScannerFile RADI01 { get { return new ScannerFile("01_model\\MODEL_RADI_01.wav", "MODEL_RADI_01", "model"); } }
        public static ScannerFile RANCHERXL01 { get { return new ScannerFile("01_model\\MODEL_RANCHER_XL_01.wav", "MODEL_RANCHER_XL_01", "model"); } }
        public static ScannerFile RAPIDGT01 { get { return new ScannerFile("01_model\\MODEL_RAPID_GT_01.wav", "MODEL_RAPID_GT_01", "model"); } }
        public static ScannerFile RATLOADER01 { get { return new ScannerFile("01_model\\MODEL_RATLOADER_01.wav", "MODEL_RATLOADER_01", "model"); } }
        public static ScannerFile RCBANDITO01 { get { return new ScannerFile("01_model\\MODEL_RC_BANDITO_01.wav", "MODEL_RC_BANDITO_01", "model"); } }
        public static ScannerFile REBEL01 { get { return new ScannerFile("01_model\\MODEL_REBEL_01.wav", "MODEL_REBEL_01", "model"); } }
        public static ScannerFile REGINA01 { get { return new ScannerFile("01_model\\MODEL_REGINA_01.wav", "MODEL_REGINA_01", "model"); } }
        public static ScannerFile RHINO01 { get { return new ScannerFile("01_model\\MODEL_RHINO_01.wav", "MODEL_RHINO_01", "model"); } }
        public static ScannerFile RIDEONMOWER01 { get { return new ScannerFile("01_model\\MODEL_RIDE_ON_MOWER_01.wav", "MODEL_RIDE_ON_MOWER_01", "model"); } }
        public static ScannerFile RIOT01 { get { return new ScannerFile("01_model\\MODEL_RIOT_01.wav", "MODEL_RIOT_01", "model"); } }
        public static ScannerFile RIPLEY01 { get { return new ScannerFile("01_model\\MODEL_RIPLEY_01.wav", "MODEL_RIPLEY_01", "model"); } }
        public static ScannerFile ROCOTO01 { get { return new ScannerFile("01_model\\MODEL_ROCOTO_01.wav", "MODEL_ROCOTO_01", "model"); } }
        public static ScannerFile RUBBLE01 { get { return new ScannerFile("01_model\\MODEL_RUBBLE_01.wav", "MODEL_RUBBLE_01", "model"); } }
        public static ScannerFile RUFFIAN01 { get { return new ScannerFile("01_model\\MODEL_RUFFIAN_01.wav", "MODEL_RUFFIAN_01", "model"); } }
        public static ScannerFile RUINER01 { get { return new ScannerFile("01_model\\MODEL_RUINER_01.wav", "MODEL_RUINER_01", "model"); } }
        public static ScannerFile RUMPO01 { get { return new ScannerFile("01_model\\MODEL_RUMPO_01.wav", "MODEL_RUMPO_01", "model"); } }
        public static ScannerFile SABERGT01 { get { return new ScannerFile("01_model\\MODEL_SABER_GT_01.wav", "MODEL_SABER_GT_01", "model"); } }
        public static ScannerFile SABREGT01 { get { return new ScannerFile("01_model\\MODEL_SABREGT_01.wav", "MODEL_SABREGT_01", "model"); } }
        public static ScannerFile SADLER01 { get { return new ScannerFile("01_model\\MODEL_SADLER_01.wav", "MODEL_SADLER_01", "model"); } }
        public static ScannerFile SANCHEZ01 { get { return new ScannerFile("01_model\\MODEL_SANCHEZ_01.wav", "MODEL_SANCHEZ_01", "model"); } }
        public static ScannerFile SANDKING01 { get { return new ScannerFile("01_model\\MODEL_SANDKING_01.wav", "MODEL_SANDKING_01", "model"); } }
        public static ScannerFile SCHAFTER01 { get { return new ScannerFile("01_model\\MODEL_SCHAFTER_01.wav", "MODEL_SCHAFTER_01", "model"); } }
        public static ScannerFile SCHWARZER01 { get { return new ScannerFile("01_model\\MODEL_SCHWARZER_01.wav", "MODEL_SCHWARZER_01", "model"); } }
        public static ScannerFile SCORCHER01 { get { return new ScannerFile("01_model\\MODEL_SCORCHER_01.wav", "MODEL_SCORCHER_01", "model"); } }
        public static ScannerFile SCRAP01 { get { return new ScannerFile("01_model\\MODEL_SCRAP_01.wav", "MODEL_SCRAP_01", "model"); } }
        public static ScannerFile SEAPLANE01 { get { return new ScannerFile("01_model\\MODEL_SEAPLANE_01.wav", "MODEL_SEAPLANE_01", "model"); } }
        public static ScannerFile SEASHARK01 { get { return new ScannerFile("01_model\\MODEL_SEASHARK_01.wav", "MODEL_SEASHARK_01", "model"); } }
        public static ScannerFile SEMINOLE01 { get { return new ScannerFile("01_model\\MODEL_SEMINOLE_01.wav", "MODEL_SEMINOLE_01", "model"); } }
        public static ScannerFile SENTINEL01 { get { return new ScannerFile("01_model\\MODEL_SENTINEL_01.wav", "MODEL_SENTINEL_01", "model"); } }
        public static ScannerFile SERRANO01 { get { return new ScannerFile("01_model\\MODEL_SERRANO_01.wav", "MODEL_SERRANO_01", "model"); } }
        public static ScannerFile SKIVVY01 { get { return new ScannerFile("01_model\\MODEL_SKIVVY_01.wav", "MODEL_SKIVVY_01", "model"); } }
        public static ScannerFile SKYLIFT01 { get { return new ScannerFile("01_model\\MODEL_SKYLIFT_01.wav", "MODEL_SKYLIFT_01", "model"); } }
        public static ScannerFile SLAMVAN01 { get { return new ScannerFile("01_model\\MODEL_SLAMVAN_01.wav", "MODEL_SLAMVAN_01", "model"); } }
        public static ScannerFile SPEEDER01 { get { return new ScannerFile("01_model\\MODEL_SPEEDER_01.wav", "MODEL_SPEEDER_01", "model"); } }
        public static ScannerFile SPEEDO01 { get { return new ScannerFile("01_model\\MODEL_SPEEDO_01.wav", "MODEL_SPEEDO_01", "model"); } }
        public static ScannerFile SQUADDIE01 { get { return new ScannerFile("01_model\\MODEL_SQUADDIE_01.wav", "MODEL_SQUADDIE_01", "model"); } }
        public static ScannerFile SQUALO01 { get { return new ScannerFile("01_model\\MODEL_SQUALO_01.wav", "MODEL_SQUALO_01", "model"); } }
        public static ScannerFile STANIER01 { get { return new ScannerFile("01_model\\MODEL_STANIER_01.wav", "MODEL_STANIER_01", "model"); } }
        public static ScannerFile STINGER01 { get { return new ScannerFile("01_model\\MODEL_STINGER_01.wav", "MODEL_STINGER_01", "model"); } }
        public static ScannerFile STINGERGT01 { get { return new ScannerFile("01_model\\MODEL_STINGER_GT_01.wav", "MODEL_STINGER_GT_01", "model"); } }
        public static ScannerFile STOCKADE01 { get { return new ScannerFile("01_model\\MODEL_STOCKADE_01.wav", "MODEL_STOCKADE_01", "model"); } }
        public static ScannerFile STRATUM01 { get { return new ScannerFile("01_model\\MODEL_STRATUM_01.wav", "MODEL_STRATUM_01", "model"); } }
        public static ScannerFile STRETCH01 { get { return new ScannerFile("01_model\\MODEL_STRETCH_01.wav", "MODEL_STRETCH_01", "model"); } }
        public static ScannerFile STUNT01 { get { return new ScannerFile("01_model\\MODEL_STUNT_01.wav", "MODEL_STUNT_01", "model"); } }
        public static ScannerFile SUBMARINE01 { get { return new ScannerFile("01_model\\MODEL_SUBMARINE_01.wav", "MODEL_SUBMARINE_01", "model"); } }
        public static ScannerFile SUBMERSIBLE01 { get { return new ScannerFile("01_model\\MODEL_SUBMERSIBLE_01.wav", "MODEL_SUBMERSIBLE_01", "model"); } }
        public static ScannerFile SULTAN01 { get { return new ScannerFile("01_model\\MODEL_SULTAN_01.wav", "MODEL_SULTAN_01", "model"); } }
        public static ScannerFile SUNTRAP01 { get { return new ScannerFile("01_model\\MODEL_SUNTRAP_01.wav", "MODEL_SUNTRAP_01", "model"); } }
        public static ScannerFile SUPERDIAMOND01 { get { return new ScannerFile("01_model\\MODEL_SUPER_DIAMOND_01.wav", "MODEL_SUPER_DIAMOND_01", "model"); } }
        public static ScannerFile SURANO01 { get { return new ScannerFile("01_model\\MODEL_SURANO_01.wav", "MODEL_SURANO_01", "model"); } }
        public static ScannerFile SURFER01 { get { return new ScannerFile("01_model\\MODEL_SURFER_01.wav", "MODEL_SURFER_01", "model"); } }
        public static ScannerFile SURGE01 { get { return new ScannerFile("01_model\\MODEL_SURGE_01.wav", "MODEL_SURGE_01", "model"); } }
        public static ScannerFile SXR01 { get { return new ScannerFile("01_model\\MODEL_SXR_01.wav", "MODEL_SXR_01", "model"); } }
        public static ScannerFile TACO01 { get { return new ScannerFile("01_model\\MODEL_TACO_01.wav", "MODEL_TACO_01", "model"); } }
        public static ScannerFile TACOVAN01 { get { return new ScannerFile("01_model\\MODEL_TACO_VAN_01.wav", "MODEL_TACO_VAN_01", "model"); } }
        public static ScannerFile TAILGATER01 { get { return new ScannerFile("01_model\\MODEL_TAILGATER_01.wav", "MODEL_TAILGATER_01", "model"); } }
        public static ScannerFile TAMPA01 { get { return new ScannerFile("01_model\\MODEL_TAMPA_01.wav", "MODEL_TAMPA_01", "model"); } }
        public static ScannerFile TAXI01 { get { return new ScannerFile("01_model\\MODEL_TAXI_01.wav", "MODEL_TAXI_01", "model"); } }
        public static ScannerFile TIPPER01 { get { return new ScannerFile("01_model\\MODEL_TIPPER_01.wav", "MODEL_TIPPER_01", "model"); } }
        public static ScannerFile TIPTRUCK01 { get { return new ScannerFile("01_model\\MODEL_TIPTRUCK_01.wav", "MODEL_TIPTRUCK_01", "model"); } }
        public static ScannerFile TITAN01 { get { return new ScannerFile("01_model\\MODEL_TITAN_01.wav", "MODEL_TITAN_01", "model"); } }
        public static ScannerFile TORNADO01 { get { return new ScannerFile("01_model\\MODEL_TORNADO_01.wav", "MODEL_TORNADO_01", "model"); } }
        public static ScannerFile TOURBUS01 { get { return new ScannerFile("01_model\\MODEL_TOUR_BUS_01.wav", "MODEL_TOUR_BUS_01", "model"); } }
        public static ScannerFile TOWTRUCK01 { get { return new ScannerFile("01_model\\MODEL_TOWTRUCK_01.wav", "MODEL_TOWTRUCK_01", "model"); } }
        public static ScannerFile TR301 { get { return new ScannerFile("01_model\\MODEL_TR3_01.wav", "MODEL_TR3_01", "model"); } }
        public static ScannerFile TRACTOR01 { get { return new ScannerFile("01_model\\MODEL_TRACTOR_01.wav", "MODEL_TRACTOR_01", "model"); } }
        public static ScannerFile TRASH01 { get { return new ScannerFile("01_model\\MODEL_TRASH_01.wav", "MODEL_TRASH_01", "model"); } }
        public static ScannerFile TRIALS01 { get { return new ScannerFile("01_model\\MODEL_TRIALS_01.wav", "MODEL_TRIALS_01", "model"); } }
        public static ScannerFile TRIBIKE01 { get { return new ScannerFile("01_model\\MODEL_TRIBIKE_01.wav", "MODEL_TRIBIKE_01", "model"); } }
        public static ScannerFile TROPIC01 { get { return new ScannerFile("01_model\\MODEL_TROPIC_01.wav", "MODEL_TROPIC_01", "model"); } }
        public static ScannerFile TUGBOAT01 { get { return new ScannerFile("01_model\\MODEL_TUG_BOAT_01.wav", "MODEL_TUG_BOAT_01", "model"); } }
        public static ScannerFile UTILITYTRUCK01 { get { return new ScannerFile("01_model\\MODEL_UTILITY_TRUCK_01.wav", "MODEL_UTILITY_TRUCK_01", "model"); } }
        public static ScannerFile VACCA01 { get { return new ScannerFile("01_model\\MODEL_VACCA_01.wav", "MODEL_VACCA_01", "model"); } }
        public static ScannerFile VADER01 { get { return new ScannerFile("01_model\\MODEL_VADER_01.wav", "MODEL_VADER_01", "model"); } }
        public static ScannerFile VIGERO01 { get { return new ScannerFile("01_model\\MODEL_VIGERO_01.wav", "MODEL_VIGERO_01", "model"); } }
        public static ScannerFile VOLTIC01 { get { return new ScannerFile("01_model\\MODEL_VOLTIC_01.wav", "MODEL_VOLTIC_01", "model"); } }
        public static ScannerFile VOODOO01 { get { return new ScannerFile("01_model\\MODEL_VOODOO_01.wav", "MODEL_VOODOO_01", "model"); } }
        public static ScannerFile VULKAN01 { get { return new ScannerFile("01_model\\MODEL_VULKAN_01.wav", "MODEL_VULKAN_01", "model"); } }
        public static ScannerFile WASHINGTON01 { get { return new ScannerFile("01_model\\MODEL_WASHINGTON_01.wav", "MODEL_WASHINGTON_01", "model"); } }
        public static ScannerFile WAYFARER01 { get { return new ScannerFile("01_model\\MODEL_WAYFARER_01.wav", "MODEL_WAYFARER_01", "model"); } }
        public static ScannerFile WILLARD01 { get { return new ScannerFile("01_model\\MODEL_WILLARD_01.wav", "MODEL_WILLARD_01", "model"); } }
        public static ScannerFile XL01 { get { return new ScannerFile("01_model\\MODEL_XL_01.wav", "MODEL_XL_01", "model"); } }
        public static ScannerFile YOUGA01 { get { return new ScannerFile("01_model\\MODEL_YOUGA_01.wav", "MODEL_YOUGA_01", "model"); } }
        public static ScannerFile ZION01 { get { return new ScannerFile("01_model\\MODEL_ZION_01.wav", "MODEL_ZION_01", "model"); } }
        public static ScannerFile ZTYPE01 { get { return new ScannerFile("01_model\\MODEL_ZTYPE_01.wav", "MODEL_ZTYPE_01", "model"); } }
    }
    public class near_dir
    {
        public static ScannerFile Westofumm { get { return new ScannerFile("01_near_dir\\0x07FAA085.wav", "West of, umm...", "near_dir"); } }
        public static ScannerFile Eastofuh { get { return new ScannerFile("01_near_dir\\0x0B24BCE7.wav", "East of, uh...", "near_dir"); } }
        public static ScannerFile Southofumm { get { return new ScannerFile("01_near_dir\\0x0DEF2D26.wav", "South of, umm...", "near_dir"); } }
        public static ScannerFile Eastofumm { get { return new ScannerFile("01_near_dir\\0x156E917B.wav", "East of, umm...", "near_dir"); } }
        public static ScannerFile Northofuhh { get { return new ScannerFile("01_near_dir\\0x15BD7E38.wav", "North of, uhh...", "near_dir"); } }
        public static ScannerFile Northofuh { get { return new ScannerFile("01_near_dir\\0x16C2C038.wav", "North of, (uh)", "near_dir"); } }
        public static ScannerFile Westof { get { return new ScannerFile("01_near_dir\\0x19D04430.wav", "West of...", "near_dir"); } }
        public static ScannerFile Southofuhh { get { return new ScannerFile("01_near_dir\\0x1D0DCB63.wav", "South of, uhh...", "near_dir"); } }
    }
    public class no_further_units
    {
        public static ScannerFile WereCode4Adam { get { return new ScannerFile("01_no_further_units\\0x0605420C.wav", "We're Code 4-Adam.", "no_further_units"); } }
        public static ScannerFile Code4Adamnoadditionalsupportneeded { get { return new ScannerFile("01_no_further_units\\0x0A5BCAB6.wav", "Code 4-Adam; no additional support needed.", "no_further_units"); } }
        public static ScannerFile Noadditionalofficersneeded { get { return new ScannerFile("01_no_further_units\\0x0B1C8C3B.wav", "No additional officers needed.", "no_further_units"); } }
        public static ScannerFile Code4Adam { get { return new ScannerFile("01_no_further_units\\0x185CA6BB.wav", "Code 4-Adam.", "no_further_units"); } }
        public static ScannerFile Noadditionalofficersneeded1 { get { return new ScannerFile("01_no_further_units\\0x189CE738.wav", "No additional officers needed.", "no_further_units"); } }
        public static ScannerFile Nofurtherunitsrequired { get { return new ScannerFile("01_no_further_units\\0x1CE7EFD0.wav", "No further units required.", "no_further_units"); } }
    }
    public class number_criminals
    {
        public static ScannerFile suspects8 { get { return new ScannerFile("01_number_criminals\\0x03F2B4F9.wav", "8 suspects.", "number_criminals"); } }
        public static ScannerFile suspects3 { get { return new ScannerFile("01_number_criminals\\0x06344DE2.wav", "3 suspects.", "number_criminals"); } }
        public static ScannerFile suspects9 { get { return new ScannerFile("01_number_criminals\\0x08A64BDC.wav", "9 suspects.", "number_criminals"); } }
        public static ScannerFile suspects6 { get { return new ScannerFile("01_number_criminals\\0x0E5BE06F.wav", "6 suspects.", "number_criminals"); } }
        public static ScannerFile suspects5 { get { return new ScannerFile("01_number_criminals\\0x113E5F08.wav", "5 suspects.", "number_criminals"); } }
        public static ScannerFile suspects7 { get { return new ScannerFile("01_number_criminals\\0x13572036.wav", "7 suspects.", "number_criminals"); } }
        public static ScannerFile suspect1 { get { return new ScannerFile("01_number_criminals\\0x136DA273.wav", "1 suspect.", "number_criminals"); } }
        public static ScannerFile suspects10 { get { return new ScannerFile("01_number_criminals\\0x16B651EB.wav", "10 suspects.", "number_criminals"); } }
        public static ScannerFile suspects2 { get { return new ScannerFile("01_number_criminals\\0x19FB0E23.wav", "2 suspects.", "number_criminals"); } }
        public static ScannerFile suspects4 { get { return new ScannerFile("01_number_criminals\\0x1DF7C60A.wav", "4 suspects.", "number_criminals"); } }
    }
    public class officer
    {
        public static ScannerFile Unituhh { get { return new ScannerFile("01_officer\\0x03087D87.wav", "Unit, uhh...", "officer"); } }
        public static ScannerFile Officeruhh { get { return new ScannerFile("01_officer\\0x060A837E.wav", "Officer, uhh...", "officer"); } }
        public static ScannerFile Unitumm { get { return new ScannerFile("01_officer\\0x0C5C9031.wav", "Unit, umm..", "officer"); } }
        public static ScannerFile Officerumm { get { return new ScannerFile("01_officer\\0x17D62715.wav", "Officer, umm...", "officer"); } }
        public static ScannerFile Officer { get { return new ScannerFile("01_officer\\0x19CC6B10.wav", "Officer...", "officer"); } }
        public static ScannerFile Officerrrrumm { get { return new ScannerFile("01_officer\\0x1BE7EF39.wav", "Officerrrr...umm...", "officer"); } }
        public static ScannerFile Officeruhh1 { get { return new ScannerFile("01_officer\\0x1E9A34AC.wav", "Officer...uhh...", "officer"); } }
    }
    public class officers_on_scene
    {
        public static ScannerFile Officersareatthescene { get { return new ScannerFile("01_officers_on_scene\\0x0307A167.wav", "Officers are at the scene.", "officers_on_scene"); } }
        public static ScannerFile Officershavearrived { get { return new ScannerFile("01_officers_on_scene\\0x06606818.wav", "Officers have arrived.", "officers_on_scene"); } }
        public static ScannerFile Officersonscene { get { return new ScannerFile("01_officers_on_scene\\0x0FFB7B47.wav", "Officers on scene.", "officers_on_scene"); } }
        public static ScannerFile Officersarrivedonscene { get { return new ScannerFile("01_officers_on_scene\\0x154DC5F2.wav", "Officers arrived on scene.", "officers_on_scene"); } }
        public static ScannerFile Officersonsite { get { return new ScannerFile("01_officers_on_scene\\0x15BC46C9.wav", "Officers on site.", "officers_on_scene"); } }
    }
    public class officer_begin_patrol
    {
        public static ScannerFile Proceedwithpatrol { get { return new ScannerFile("01_officer_begin_patrol\\0x00F6FF69.wav", "Proceed with patrol.", "officer_begin_patrol"); } }
        public static ScannerFile Proceedtopatrolarea { get { return new ScannerFile("01_officer_begin_patrol\\0x01D5812A.wav", "Proceed to patrol area.", "officer_begin_patrol"); } }
        public static ScannerFile Beginpatrol { get { return new ScannerFile("01_officer_begin_patrol\\0x062F49DC.wav", "Begin patrol.", "officer_begin_patrol"); } }
        public static ScannerFile Assigntopatrol { get { return new ScannerFile("01_officer_begin_patrol\\0x0D6A5853.wav", "Assign to patrol.", "officer_begin_patrol"); } }
        public static ScannerFile Beginbeat { get { return new ScannerFile("01_officer_begin_patrol\\0x152767CD.wav", "Begin beat.", "officer_begin_patrol"); } }
        public static ScannerFile Proceedtopatrolarea1 { get { return new ScannerFile("01_officer_begin_patrol\\0x17ABACD6.wav", "Proceed to patrol area.", "officer_begin_patrol"); } }
    }
    public class officer_down
    {
        public static ScannerFile Hasbeeninjured { get { return new ScannerFile("01_officer_down\\0x02CD8D91.wav", "Has been injured.", "officer_down"); } }
        public static ScannerFile Hasbeenwounded { get { return new ScannerFile("01_officer_down\\0x0721163B.wav", "Has been wounded.", "officer_down"); } }
        public static ScannerFile Iswounded { get { return new ScannerFile("01_officer_down\\0x0A2E5C55.wav", "Is wounded.", "officer_down"); } }
        public static ScannerFile Isnotrespondingpossiblywounded { get { return new ScannerFile("01_officer_down\\0x0A701DB8.wav", "Is not responding; possibly wounded.", "officer_down"); } }
        public static ScannerFile Killedinaction { get { return new ScannerFile("01_officer_down\\0x103D2951.wav", "Killed in action.", "officer_down"); } }
        public static ScannerFile Isseverelywounded { get { return new ScannerFile("01_officer_down\\0x1418F029.wav", "Is severely wounded.", "officer_down"); } }
        public static ScannerFile Hasbeenseverelyinjured { get { return new ScannerFile("01_officer_down\\0x1423B03D.wav", "Has been severely injured.", "officer_down"); } }
        public static ScannerFile Hasbeenkilled { get { return new ScannerFile("01_officer_down\\0x1E62C4BA.wav", "Has been killed.", "officer_down"); } }
        public static ScannerFile OFFICERDOWN01 { get { return new ScannerFile("01_officer_down\\OFFICER_DOWN_01.wav", "OFFICER_DOWN_01", "officer_down"); } }
        public static ScannerFile OFFICERDOWN02 { get { return new ScannerFile("01_officer_down\\OFFICER_DOWN_02.wav", "OFFICER_DOWN_02", "officer_down"); } }
        public static ScannerFile OFFICERDOWN03 { get { return new ScannerFile("01_officer_down\\OFFICER_DOWN_03.wav", "OFFICER_DOWN_03", "officer_down"); } }
    }
    public class officer_down_shot
    {
        public static ScannerFile Hasbeenfatallyshot { get { return new ScannerFile("01_officer_down_shot\\0x00A2944D.wav", "Has been fatally shot.", "officer_down_shot"); } }
        public static ScannerFile Hasbeenshot { get { return new ScannerFile("01_officer_down_shot\\0x0A66680B.wav", "Has been shot.", "officer_down_shot"); } }
        public static ScannerFile Isreportedlyshotandwounded { get { return new ScannerFile("01_officer_down_shot\\0x0DBDEE80.wav", "Is reportedly shot and wounded.", "officer_down_shot"); } }
        public static ScannerFile Hasbeenshotandcriticallywounded { get { return new ScannerFile("01_officer_down_shot\\0x0EDD30C2.wav", "Has been shot and critically wounded.", "officer_down_shot"); } }
        public static ScannerFile Isreportedlyshot { get { return new ScannerFile("01_officer_down_shot\\0x1C6B0BDB.wav", "Is reportedly shot.", "officer_down_shot"); } }
    }
    public class officer_requests_air_support
    {
        public static ScannerFile Officersrequireaerialsupport { get { return new ScannerFile("01_officer_requests_air_support\\0x03867D3A.wav", "Officers require aerial support.", "officer_requests_air_support"); } }
        public static ScannerFile Unitsrequestinghelicoptersupport { get { return new ScannerFile("01_officer_requests_air_support\\0x064A82C3.wav", "Units requesting helicopter support.", "officer_requests_air_support"); } }
        public static ScannerFile Unitsrequestaerialsupport { get { return new ScannerFile("01_officer_requests_air_support\\0x0CADCF88.wav", "Units request aerial support.", "officer_requests_air_support"); } }
        public static ScannerFile Unitsrequestingairsupport { get { return new ScannerFile("01_officer_requests_air_support\\0x1010164D.wav", "Units requesting air support.", "officer_requests_air_support"); } }
        public static ScannerFile Code99unitsrequestimmediateairsupport { get { return new ScannerFile("01_officer_requests_air_support\\0x1482DF27.wav", "Code 99; units request immediate air support.", "officer_requests_air_support"); } }
        public static ScannerFile Officersrequestinghelicoptersupport { get { return new ScannerFile("01_officer_requests_air_support\\0x1528207E.wav", "Officers requesting helicopter support.", "officer_requests_air_support"); } }
        public static ScannerFile Officersrequireaerialsupport1 { get { return new ScannerFile("01_officer_requests_air_support\\0x19946955.wav", "Officers require aerial support.", "officer_requests_air_support"); } }
        public static ScannerFile Officersrequireairsupport { get { return new ScannerFile("01_officer_requests_air_support\\0x1CE9B000.wav", "Officers require air support.", "officer_requests_air_support"); } }
    }
    public class officer_requests_backup
    {
        public static ScannerFile Code99officersrequireassistance { get { return new ScannerFile("01_officer_requests_backup\\0x001BB1F7.wav", "Code 99; officers require assistance.", "officer_requests_backup"); } }
        public static ScannerFile Code99unitneedsimmediatebackup { get { return new ScannerFile("01_officer_requests_backup\\0x033FF83C.wav", "Code 99; unit needs immediate backup.", "officer_requests_backup"); } }
        public static ScannerFile Officersrequestingbackup { get { return new ScannerFile("01_officer_requests_backup\\0x0ACEC75C.wav", "Officers requesting backup.", "officer_requests_backup"); } }
        public static ScannerFile Unitsrequirebackup { get { return new ScannerFile("01_officer_requests_backup\\0x0DE40D87.wav", "Units require backup.", "officer_requests_backup"); } }
        public static ScannerFile Unitsrequireimmediateassistance { get { return new ScannerFile("01_officer_requests_backup\\0x11D15562.wav", "Units require immediate assistance.", "officer_requests_backup"); } }
        public static ScannerFile Code99unitrequiresbackup { get { return new ScannerFile("01_officer_requests_backup\\0x14FC9BB9.wav", "Code 99; unit requires backup.", "officer_requests_backup"); } }
        public static ScannerFile Unitsrequestingbackup { get { return new ScannerFile("01_officer_requests_backup\\0x190D23D9.wav", "Units requesting backup.", "officer_requests_backup"); } }
        public static ScannerFile Officerneedsimmediateassistance { get { return new ScannerFile("01_officer_requests_backup\\0x1C296A12.wav", "Officer needs immediate assistance.", "officer_requests_backup"); } }
        public static ScannerFile Officerrequestingbackup { get { return new ScannerFile("01_officer_requests_backup\\0x1C79AAAE.wav", "Officer requesting backup.", "officer_requests_backup"); } }
    }
    public class on_foot
    {
        public static ScannerFile Onfoot { get { return new ScannerFile("01_on_foot\\0x00EF6F8D.wav", "On foot.", "on_foot"); } }
        public static ScannerFile Onfoot1 { get { return new ScannerFile("01_on_foot\\0x0EA9CB02.wav", "On foot.", "on_foot"); } }
    }
    public class ornate_bank_heist
    {
        public static ScannerFile Atthe211inEastLosSantos { get { return new ScannerFile("01_ornate_bank_heist\\0x0006F181.wav", "At the 2-11 in East Los Santos.", "ornate_bank_heist"); } }
        public static ScannerFile Allunits { get { return new ScannerFile("01_ornate_bank_heist\\0x011A4380.wav", "All units.", "ornate_bank_heist"); } }
        public static ScannerFile AtthebankrobberyattheEastVinewoodBankofLosSantos { get { return new ScannerFile("01_ornate_bank_heist\\0x04EDBB4E.wav", "At the bank robbery at the East Vinewood Bank of Los Santos.", "ornate_bank_heist"); } }
        public static ScannerFile AttheEastVinewoodBankofLosSantos { get { return new ScannerFile("01_ornate_bank_heist\\0x0614A3D8.wav", "At the East Vinewood Bank of Los Santos.", "ornate_bank_heist"); } }
        public static ScannerFile Officershavearrivedatthe211 { get { return new ScannerFile("01_ornate_bank_heist\\0x08C55498.wav", "Officers have arrived at the 2-11.", "ornate_bank_heist"); } }
        public static ScannerFile Unitsatthe211inEastLosSantosbeadvised { get { return new ScannerFile("01_ornate_bank_heist\\0x0A022F01.wav", "Units at the 2-11 in East Los Santos, be advised.", "ornate_bank_heist"); } }
        public static ScannerFile Attentionallunitsatthe211inEastLosSantos { get { return new ScannerFile("01_ornate_bank_heist\\0x0B9E89B4.wav", "Attention all units at the 2-11 in East Los Santos.", "ornate_bank_heist"); } }
        public static ScannerFile Attentionallunitsatthe211inEastLosSantos1 { get { return new ScannerFile("01_ornate_bank_heist\\0x0D40F8A4.wav", "Attention all units at the 2-11 in East Los Santos.", "ornate_bank_heist"); } }
        public static ScannerFile Suspectsarefleeingthecrime { get { return new ScannerFile("01_ornate_bank_heist\\0x0D6D38FC.wav", "Suspects are fleeing the crime.", "ornate_bank_heist"); } }
        public static ScannerFile Wehavereportsofapossible211inEastLosSantos { get { return new ScannerFile("01_ornate_bank_heist\\0x10BBC345.wav", "We have reports of a possible 2-11 in East Los Santos.", "ornate_bank_heist"); } }
        public static ScannerFile Atthe211inEastLosSantos1 { get { return new ScannerFile("01_ornate_bank_heist\\0x1264563C.wav", "At the 2-11 in East Los Santos.", "ornate_bank_heist"); } }
        public static ScannerFile Suspectshavebreachedthebanksvault { get { return new ScannerFile("01_ornate_bank_heist\\0x137722CE.wav", "Suspects have breached the bank's vault.", "ornate_bank_heist"); } }
        public static ScannerFile EastVinewoodBankofLosSantos { get { return new ScannerFile("01_ornate_bank_heist\\0x14D4EAF7.wav", "East Vinewood Bank of Los Santos.", "ornate_bank_heist"); } }
        public static ScannerFile Officershavearrivedatthe2111 { get { return new ScannerFile("01_ornate_bank_heist\\0x1606C3BC.wav", "Officers have arrived at the 2-11.", "ornate_bank_heist"); } }
        public static ScannerFile Atthe211attheEastVinewoodBankofLosSantos { get { return new ScannerFile("01_ornate_bank_heist\\0x17301FD3.wav", "At the 2-11 at the East Vinewood Bank of Los Santos.", "ornate_bank_heist"); } }
        public static ScannerFile Suspectshaveenteredthebanksvault { get { return new ScannerFile("01_ornate_bank_heist\\0x18C08C7E.wav", "Suspects have entered the bank's vault.", "ornate_bank_heist"); } }
        public static ScannerFile AttheEastVinewoodBankofLS { get { return new ScannerFile("01_ornate_bank_heist\\0x1A34F777.wav", "At the East Vinewood Bank of LS.", "ornate_bank_heist"); } }
        public static ScannerFile Wehaveconfirmationofabankrobberyatthe { get { return new ScannerFile("01_ornate_bank_heist\\0x1A853657.wav", "We have confirmation of a bank robbery at the...", "ornate_bank_heist"); } }
        public static ScannerFile Suspectsarefleeingthecrime1 { get { return new ScannerFile("01_ornate_bank_heist\\0x1CDC2C2F.wav", "Suspects are fleeing the crime.", "ornate_bank_heist"); } }
        public static ScannerFile Unitsatthe211inEastLosSantosbeadvised1 { get { return new ScannerFile("01_ornate_bank_heist\\0x1D6A36A5.wav", "Units at the 2-11 in East Los Santos, be advised.", "ornate_bank_heist"); } }
        public static ScannerFile Banksecurityreports4suspectsarmedanddangerous { get { return new ScannerFile("01_ornate_bank_heist\\0x1DFFFD4C.wav", "Bank security reports 4 suspects, armed and dangerous.", "ornate_bank_heist"); } }
        public static ScannerFile Atthe211attheEastVinewoodBankofLS { get { return new ScannerFile("01_ornate_bank_heist\\0x1F2DEFE7.wav", "At the 2-11 at the East Vinewood Bank of LS.", "ornate_bank_heist"); } }
        public static ScannerFile AllavailableunitsrespondCode3 { get { return new ScannerFile("01_ornate_bank_heist\\0x1FB8613E.wav", "All available units respond; Code 3.", "ornate_bank_heist"); } }
    }
    public class over_area
    {
        public static ScannerFile OVERAREAALAMOSEA01 { get { return new ScannerFile("01_over_area\\OVER_AREA_ALAMO_SEA_01.wav", "OVER_AREA_ALAMO_SEA_01", "over_area"); } }
        public static ScannerFile OVERAREABRADDOCKPASS01 { get { return new ScannerFile("01_over_area\\OVER_AREA_BRADDOCK_PASS_01.wav", "OVER_AREA_BRADDOCK_PASS_01", "over_area"); } }
        public static ScannerFile OVERAREACENTRALLOSSANTOS01 { get { return new ScannerFile("01_over_area\\OVER_AREA_CENTRAL_LOS_SANTOS_01.wav", "OVER_AREA_CENTRAL_LOS_SANTOS_01", "over_area"); } }
        public static ScannerFile OVERAREACHUMASH01 { get { return new ScannerFile("01_over_area\\OVER_AREA_CHUMASH_01.wav", "OVER_AREA_CHUMASH_01", "over_area"); } }
        public static ScannerFile OVERAREADOWNTOWN01 { get { return new ScannerFile("01_over_area\\OVER_AREA_DOWNTOWN_01.wav", "OVER_AREA_DOWNTOWN_01", "over_area"); } }
        public static ScannerFile OVERAREAEASTLOSSANTOS01 { get { return new ScannerFile("01_over_area\\OVER_AREA_EAST_LOS_SANTOS_01.wav", "OVER_AREA_EAST_LOS_SANTOS_01", "over_area"); } }
        public static ScannerFile OVERAREAGRANDESENORADESERT01 { get { return new ScannerFile("01_over_area\\OVER_AREA_GRANDE_SENORA_DESERT_01.wav", "OVER_AREA_GRANDE_SENORA_DESERT_01", "over_area"); } }
        public static ScannerFile OVERAREALAGOZANCUDO01 { get { return new ScannerFile("01_over_area\\OVER_AREA_LAGO_ZANCUDO_01.wav", "OVER_AREA_LAGO_ZANCUDO_01", "over_area"); } }
        public static ScannerFile OVERAREALOSSANTOSINTERNATIONAL01 { get { return new ScannerFile("01_over_area\\OVER_AREA_LOS_SANTOS_INTERNATIONAL_01.wav", "OVER_AREA_LOS_SANTOS_INTERNATIONAL_01", "over_area"); } }
        public static ScannerFile OVERAREAMOUNTJOSIAH01 { get { return new ScannerFile("01_over_area\\OVER_AREA_MOUNT_JOSIAH_01.wav", "OVER_AREA_MOUNT_JOSIAH_01", "over_area"); } }
        public static ScannerFile OVERAREANORTHLOSSANTOS01 { get { return new ScannerFile("01_over_area\\OVER_AREA_NORTH_LOS_SANTOS_01.wav", "OVER_AREA_NORTH_LOS_SANTOS_01", "over_area"); } }
        public static ScannerFile OVERAREAPALETOBAY01 { get { return new ScannerFile("01_over_area\\OVER_AREA_PALETO_BAY_01.wav", "OVER_AREA_PALETO_BAY_01", "over_area"); } }
        public static ScannerFile OVERAREAPORTOFLOSSANTOS01 { get { return new ScannerFile("01_over_area\\OVER_AREA_PORT_OF_LOS_SANTOS_01.wav", "OVER_AREA_PORT_OF_LOS_SANTOS_01", "over_area"); } }
        public static ScannerFile OVERAREARATONCANYON01 { get { return new ScannerFile("01_over_area\\OVER_AREA_RATON_CANYON_01.wav", "OVER_AREA_RATON_CANYON_01", "over_area"); } }
        public static ScannerFile OVERAREASANDYSHORES01 { get { return new ScannerFile("01_over_area\\OVER_AREA_SANDY_SHORES_01.wav", "OVER_AREA_SANDY_SHORES_01", "over_area"); } }
        public static ScannerFile OVERAREASOUTHLOSSANTOS01 { get { return new ScannerFile("01_over_area\\OVER_AREA_SOUTH_LOS_SANTOS_01.wav", "OVER_AREA_SOUTH_LOS_SANTOS_01", "over_area"); } }
        public static ScannerFile OVERAREAVINEWOODHILLS01 { get { return new ScannerFile("01_over_area\\OVER_AREA_VINEWOOD_HILLS_01.wav", "OVER_AREA_VINEWOOD_HILLS_01", "over_area"); } }
        public static ScannerFile OVERAREAWESTLOSSANTOS01 { get { return new ScannerFile("01_over_area\\OVER_AREA_WEST_LOS_SANTOS_01.wav", "OVER_AREA_WEST_LOS_SANTOS_01", "over_area"); } }
        public static ScannerFile OVERAREAWESTLOSSANTOS02 { get { return new ScannerFile("01_over_area\\OVER_AREA_WEST_LOS_SANTOS_02.wav", "OVER_AREA_WEST_LOS_SANTOS_02", "over_area"); } }
    }
    public class player_description
    {
        public static ScannerFile Blackmaleearly20sblackhair { get { return new ScannerFile("01_player_description\\0x002BC54F.wav", "Black male, early 20s, black hair.", "player_description"); } }
        public static ScannerFile Whitemalelate40sdarkhair { get { return new ScannerFile("01_player_description\\0x0372C4A6.wav", "White male, late 40s, dark hair.", "player_description"); } }
        public static ScannerFile Athleticwhitemalelate40sdarkhair { get { return new ScannerFile("01_player_description\\0x049E79F3.wav", "Athletic white male, late 40s, dark hair.", "player_description"); } }
        public static ScannerFile Athleticblackmaleearly20sblackhair { get { return new ScannerFile("01_player_description\\0x0B2492A0.wav", "Athletic black male, early 20s, black hair.", "player_description"); } }
        public static ScannerFile Heavysetblackmaleearly20sblackhair { get { return new ScannerFile("01_player_description\\0x1285A907.wav", "Heavyset black male, early 20s, black hair.", "player_description"); } }
        public static ScannerFile Heavysetwhitemalelate40sdarkhair { get { return new ScannerFile("01_player_description\\0x18396F91.wav", "Heavyset white male, late 40s, dark hair.", "player_description"); } }
        public static ScannerFile Whitemalemid30sbrownhair { get { return new ScannerFile("01_player_description\\0x19D42672.wav", "White male, mid-30s, brown hair.", "player_description"); } }
        public static ScannerFile Heavysetwhitemalemid30sbrownhair { get { return new ScannerFile("01_player_description\\0x1B2189EA.wav", "Heavyset white male, mid-30s, brown hair.", "player_description"); } }
        public static ScannerFile Athleticwhitemalemid30sbrownhair { get { return new ScannerFile("01_player_description\\0x1EF15F7E.wav", "Athletic white male, mid-30s, brown hair.", "player_description"); } }
    }
    public class police_station_bust_out
    {
        public static ScannerFile Emergencyallunitsrespond { get { return new ScannerFile("01_police_station_bust_out\\0x0121C945.wav", "Emergency; all units respond.", "police_station_bust_out"); } }
        public static ScannerFile Allunitswehavea1024 { get { return new ScannerFile("01_police_station_bust_out\\0x0362211C.wav", "All units, we have a 10-24.", "police_station_bust_out"); } }
        public static ScannerFile Allunitsrespondimmediatelycodepurple { get { return new ScannerFile("01_police_station_bust_out\\0x047D8AA7.wav", "All units respond immediately, code purple.", "police_station_bust_out"); } }
        public static ScannerFile Allunitsrespondimmediatelyemergency { get { return new ScannerFile("01_police_station_bust_out\\0x05959293.wav", "All units respond immediately; emergency.", "police_station_bust_out"); } }
        public static ScannerFile Allunitsrespondimmediatelycode99 { get { return new ScannerFile("01_police_station_bust_out\\0x080C5809.wav", "All units respond immediately; code 99.", "police_station_bust_out"); } }
        public static ScannerFile Allunitsrespondimmediatelyemergency1 { get { return new ScannerFile("01_police_station_bust_out\\0x08968ED7.wav", "All units respond immediately; emergency.", "police_station_bust_out"); } }
        public static ScannerFile Attentionallunitswehaveareportofanexplosionattheprecinct { get { return new ScannerFile("01_police_station_bust_out\\0x0A4618EC.wav", "Attention all units, we have a report of an explosion at the precinct.", "police_station_bust_out"); } }
        public static ScannerFile Allunitsrespond1024 { get { return new ScannerFile("01_police_station_bust_out\\0x0A786E55.wav", "All units respond; 10-24.", "police_station_bust_out"); } }
        public static ScannerFile Emergencyallunitsrespond1 { get { return new ScannerFile("01_police_station_bust_out\\0x0EC921B4.wav", "Emergency; all units respond.", "police_station_bust_out"); } }
        public static ScannerFile Allunitsrespondimmediatelycode991 { get { return new ScannerFile("01_police_station_bust_out\\0x0FB1DEC0.wav", "All units respond immediately; code 99.", "police_station_bust_out"); } }
        public static ScannerFile Attentionwehavea982atthecentralprecinct { get { return new ScannerFile("01_police_station_bust_out\\0x0FEDA742.wav", "Attention, we have a 9-82 at the central precinct.", "police_station_bust_out"); } }
        public static ScannerFile Allunitsanexplosionhasoccurredattheprecinct { get { return new ScannerFile("01_police_station_bust_out\\0x10A0609E.wav", "All units, an explosion has occurred at the precinct.", "police_station_bust_out"); } }
        public static ScannerFile Attentionwehavea983atthecentralprecinct { get { return new ScannerFile("01_police_station_bust_out\\0x12C9A33D.wav", "Attention, we have a 9-83 at the central precinct.", "police_station_bust_out"); } }
        public static ScannerFile Allunitsrespondimmediatelycodepurple1 { get { return new ScannerFile("01_police_station_bust_out\\0x13594109.wav", "All units respond immediately; code purple.", "police_station_bust_out"); } }
        public static ScannerFile Wehaveareportofanexplosionattheprecinct { get { return new ScannerFile("01_police_station_bust_out\\0x13672DD0.wav", "We have a report of an explosion at the precinct.", "police_station_bust_out"); } }
        public static ScannerFile Allunitsanexplosionhasoccurredattheprecinct1 { get { return new ScannerFile("01_police_station_bust_out\\0x15CD338B.wav", "All units, an explosion has occurred at the precinct.", "police_station_bust_out"); } }
        public static ScannerFile Allunitswehavea10241 { get { return new ScannerFile("01_police_station_bust_out\\0x16362E18.wav", "All units, we have a 10-24.", "police_station_bust_out"); } }
        public static ScannerFile Attentionallunitswevegota996attheprecinct { get { return new ScannerFile("01_police_station_bust_out\\0x19B8CCD6.wav", "Attention all units, we've got a 9-96 at the precinct.", "police_station_bust_out"); } }
    }
    public class proceed_with_caution
    {
        public static ScannerFile Proceedwithcaution { get { return new ScannerFile("01_proceed_with_caution\\0x0C544469.wav", "Proceed with caution.", "proceed_with_caution"); } }
        public static ScannerFile Approachwithcaution { get { return new ScannerFile("01_proceed_with_caution\\0x1A101FE4.wav", "Approach with caution.", "proceed_with_caution"); } }
        public static ScannerFile Officersproceedwithcaution { get { return new ScannerFile("01_proceed_with_caution\\0x1AEC219A.wav", "Officers, proceed with caution.", "proceed_with_caution"); } }
    }
    public class race_sex
    {
        public static ScannerFile Caucasianmale { get { return new ScannerFile("01_race_sex\\0x000394EA.wav", "Caucasian male.", "race_sex"); } }
        public static ScannerFile Asianmale { get { return new ScannerFile("01_race_sex\\0x035E8EE4.wav", "Asian male.", "race_sex"); } }
        public static ScannerFile AfricanAmericanfemale { get { return new ScannerFile("01_race_sex\\0x040777A8.wav", "African-American female.", "race_sex"); } }
        public static ScannerFile Caucasianfemale { get { return new ScannerFile("01_race_sex\\0x0858623E.wav", "Caucasian female.", "race_sex"); } }
        public static ScannerFile AMA { get { return new ScannerFile("01_race_sex\\0x0920E051.wav", "AMA.", "race_sex"); } }
        public static ScannerFile WFA { get { return new ScannerFile("01_race_sex\\0x0B427DCF.wav", "WFA.", "race_sex"); } }
        public static ScannerFile Blackmale { get { return new ScannerFile("01_race_sex\\0x0B8CBDE4.wav", "Black male.", "race_sex"); } }
        public static ScannerFile AfricanAmericanmale { get { return new ScannerFile("01_race_sex\\0x0CE2C090.wav", "African-American male.", "race_sex"); } }
        public static ScannerFile Hispanicmale { get { return new ScannerFile("01_race_sex\\0x0E6CBBB1.wav", "Hispanic male.", "race_sex"); } }
        public static ScannerFile Whitefemale { get { return new ScannerFile("01_race_sex\\0x0EDCEF46.wav", "White female.", "race_sex"); } }
        public static ScannerFile Whitemale { get { return new ScannerFile("01_race_sex\\0x122AF938.wav", "White male.", "race_sex"); } }
        public static ScannerFile BFA { get { return new ScannerFile("01_race_sex\\0x13490822.wav", "BFA.", "race_sex"); } }
        public static ScannerFile Asianfemale { get { return new ScannerFile("01_race_sex\\0x17579460.wav", "Asian female.", "race_sex"); } }
        public static ScannerFile BMA { get { return new ScannerFile("01_race_sex\\0x17608E77.wav", "BMA.", "race_sex"); } }
        public static ScannerFile Hispanicfemale { get { return new ScannerFile("01_race_sex\\0x177B46CA.wav", "Hispanic female.", "race_sex"); } }
        public static ScannerFile AFA { get { return new ScannerFile("01_race_sex\\0x18792A5B.wav", "AFA.", "race_sex"); } }
        public static ScannerFile WFA1 { get { return new ScannerFile("01_race_sex\\0x188BD862.wav", "WFA.", "race_sex"); } }
        public static ScannerFile HFA { get { return new ScannerFile("01_race_sex\\0x19BB2712.wav", "HFA.", "race_sex"); } }
        public static ScannerFile WMA { get { return new ScannerFile("01_race_sex\\0x1BF59998.wav", "WMA.", "race_sex"); } }
        public static ScannerFile Blackfemale { get { return new ScannerFile("01_race_sex\\0x1D26A9B5.wav", "Black female.", "race_sex"); } }
        public static ScannerFile HMA { get { return new ScannerFile("01_race_sex\\0x1EB40183.wav", "HMA.", "race_sex"); } }
    }
    public class requesting_backup
    {
        public static ScannerFile Requiresassistance { get { return new ScannerFile("01_requesting_backup\\0x00659C9E.wav", "Requires assistance.", "requesting_backup"); } }
        public static ScannerFile Needsassistance { get { return new ScannerFile("01_requesting_backup\\0x16328837.wav", "Needs assistance.", "requesting_backup"); } }
        public static ScannerFile REQUESTBACKUP01 { get { return new ScannerFile("01_requesting_backup\\REQUEST_BACKUP_01.wav", "REQUEST_BACKUP_01", "requesting_backup"); } }
        public static ScannerFile REQUESTBACKUP02 { get { return new ScannerFile("01_requesting_backup\\REQUEST_BACKUP_02.wav", "REQUEST_BACKUP_02", "requesting_backup"); } }
        public static ScannerFile REQUESTBACKUP03 { get { return new ScannerFile("01_requesting_backup\\REQUEST_BACKUP_03.wav", "REQUEST_BACKUP_03", "requesting_backup"); } }
        public static ScannerFile REQUESTBACKUP04 { get { return new ScannerFile("01_requesting_backup\\REQUEST_BACKUP_04.wav", "REQUEST_BACKUP_04", "requesting_backup"); } }
    }
    public class requesting_escort
    {
        public static ScannerFile Requestsanescort { get { return new ScannerFile("01_requesting_escort\\0x0C7682B4.wav", "Requests an escort.", "requesting_escort"); } }
        public static ScannerFile Requesting1014 { get { return new ScannerFile("01_requesting_escort\\0x0DFA05B6.wav", "Requesting 10-14.", "requesting_escort"); } }
        public static ScannerFile Requestsprisonertransportassistance { get { return new ScannerFile("01_requesting_escort\\0x17C81958.wav", "Requests prisoner transport assistance.", "requesting_escort"); } }
        public static ScannerFile Requesta1014 { get { return new ScannerFile("01_requesting_escort\\0x1A27DE16.wav", "Request a 10-14.", "requesting_escort"); } }
        public static ScannerFile Requestingescort { get { return new ScannerFile("01_requesting_escort\\0x1F80E8C2.wav", "Requesting escort.", "requesting_escort"); } }
    }
    public class scars
    {
        public static ScannerFile Alargefacialscar { get { return new ScannerFile("01_scars\\0x03FA659B.wav", "A large facial scar.", "scars"); } }
        public static ScannerFile Smallupperbodyscar { get { return new ScannerFile("01_scars\\0x054E3D88.wav", "Small upper-body scar.", "scars"); } }
        public static ScannerFile Largeupperbodyscar { get { return new ScannerFile("01_scars\\0x05D1764C.wav", "Large upper-body scar.", "scars"); } }
        public static ScannerFile Asmallscarontheirface { get { return new ScannerFile("01_scars\\0x0655846D.wav", "A small scar on their face.", "scars"); } }
        public static ScannerFile Asmallscarontheirupperbody { get { return new ScannerFile("01_scars\\0x079FC228.wav", "A small scar on their upper-body.", "scars"); } }
        public static ScannerFile Alargescarorscarsontheface { get { return new ScannerFile("01_scars\\0x09DD315F.wav", "A large scar or scars on the face.", "scars"); } }
        public static ScannerFile Lightupperbodyscarring { get { return new ScannerFile("01_scars\\0x0A246759.wav", "Light upper-body scarring.", "scars"); } }
        public static ScannerFile Asmallscarorscarsontheface { get { return new ScannerFile("01_scars\\0x0A5CCC7D.wav", "A small scar or scars on the face.", "scars"); } }
        public static ScannerFile Heavyfacialscarring { get { return new ScannerFile("01_scars\\0x0B7E0ED1.wav", "Heavy facial scarring.", "scars"); } }
        public static ScannerFile Alargescarontheirface { get { return new ScannerFile("01_scars\\0x11B7C116.wav", "A large scar on their face.", "scars"); } }
        public static ScannerFile Lightfacialscarring { get { return new ScannerFile("01_scars\\0x136455B7.wav", "Light facial scarring.", "scars"); } }
        public static ScannerFile Smallscarsontheirfaceandbody { get { return new ScannerFile("01_scars\\0x13D6CBC0.wav", "Small scars on their face and body.", "scars"); } }
        public static ScannerFile Largeupperbodyscarorscars { get { return new ScannerFile("01_scars\\0x141F92E9.wav", "Large upper-body scar or scars.", "scars"); } }
        public static ScannerFile Lifescarringtofaceandbody { get { return new ScannerFile("01_scars\\0x14A8577D.wav", "Life scarring to face and body.", "scars"); } }
        public static ScannerFile Heavyupperbodyscarring { get { return new ScannerFile("01_scars\\0x17E59DFE.wav", "Heavy upper-body scarring.", "scars"); } }
        public static ScannerFile Alargescarontheirupperbody { get { return new ScannerFile("01_scars\\0x188CDBC2.wav", "A large scar on their upper-body.", "scars"); } }
        public static ScannerFile Asmallfacialscar { get { return new ScannerFile("01_scars\\0x1910A9E3.wav", "A small facial scar.", "scars"); } }
        public static ScannerFile Largescarsontheirfaceandbody { get { return new ScannerFile("01_scars\\0x19604F3C.wav", "Large scars on their face and body.", "scars"); } }
        public static ScannerFile Smallupperbodyscarorscars { get { return new ScannerFile("01_scars\\0x19E7E6BA.wav", "Small upper-body scar or scars.", "scars"); } }
        public static ScannerFile Heavyscarringtofaceandbody { get { return new ScannerFile("01_scars\\0x1FD6FB59.wav", "Heavy scarring to face and body.", "scars"); } }
    }
    public class scripted_lines
    {
        public static ScannerFile Attentionallunits { get { return new ScannerFile("01_scripted_lines\\0x00075F1C.wav", "Attention all units.", "scripted_lines"); } }
        public static ScannerFile TheLSRiver { get { return new ScannerFile("01_scripted_lines\\0x002DE1C8.wav", "The LS River.", "scripted_lines"); } }
        public static ScannerFile Wehavea211silentonanarmoredtruck { get { return new ScannerFile("01_scripted_lines\\0x0046621B.wav", "We have a 2-11 silent on an armored truck.", "scripted_lines"); } }
        public static ScannerFile Wehavea211attheUnionDepository { get { return new ScannerFile("01_scripted_lines\\0x005F5082.wav", "We have a 2-11 at the Union Depository.", "scripted_lines"); } }
        public static ScannerFile Whitemaleadultseenfleeingthearea { get { return new ScannerFile("01_scripted_lines\\0x01034057.wav", "White male, adult, seen fleeing the area.", "scripted_lines"); } }
        public static ScannerFile Useofdeadlyforceisauthorized { get { return new ScannerFile("01_scripted_lines\\0x010F1FF6.wav", "Use of deadly force is authorized.", "scripted_lines"); } }
        public static ScannerFile Possiblesuspectseenheadingwestinabulldozer { get { return new ScannerFile("01_scripted_lines\\0x0116D5B7.wav", "Possible suspect seen heading west in a bulldozer.", "scripted_lines"); } }
        public static ScannerFile TheparkinggarageattheArcadiusBusinessCenter { get { return new ScannerFile("01_scripted_lines\\0x015CB5CE.wav", "The parking garage at the Arcadius Business Center.", "scripted_lines"); } }
        public static ScannerFile AttentionLaPuertaunits { get { return new ScannerFile("01_scripted_lines\\0x016048EA.wav", "Attention La Puerta units.", "scripted_lines"); } }
        public static ScannerFile Code4allunitsreturntopatrol { get { return new ScannerFile("01_scripted_lines\\0x0193CCC9.wav", "Code 4; all units return to patrol.", "scripted_lines"); } }
        public static ScannerFile RespondCode3 { get { return new ScannerFile("01_scripted_lines\\0x01F3ACCB.wav", "Respond; Code 3.", "scripted_lines"); } }
        public static ScannerFile Piggybankisbrokenrepeatbroken { get { return new ScannerFile("01_scripted_lines\\0x02001264.wav", "Piggy bank is broken; repeat, broken.", "scripted_lines"); } }
        public static ScannerFile Attentionallunits1 { get { return new ScannerFile("01_scripted_lines\\0x023C372E.wav", "Attention all units.", "scripted_lines"); } }
        public static ScannerFile Vehiclesarereportedstolen { get { return new ScannerFile("01_scripted_lines\\0x027592ED.wav", "Vehicles are reported stolen.", "scripted_lines"); } }
        public static ScannerFile Wevegotareportofanarmedcarhijacked { get { return new ScannerFile("01_scripted_lines\\0x02B6EF7F.wav", "We've got a report of an armed car hijacked.", "scripted_lines"); } }
        public static ScannerFile AttentionGruppe6securityalarmtriggeredon { get { return new ScannerFile("01_scripted_lines\\0x02E399B8.wav", "Attention, Gruppe 6 security alarm triggered on...", "scripted_lines"); } }
        public static ScannerFile Wehaveareported187 { get { return new ScannerFile("01_scripted_lines\\0x02EDC597.wav", "We have a reported 1-87.", "scripted_lines"); } }
        public static ScannerFile Beadvisedsuspectisarmed { get { return new ScannerFile("01_scripted_lines\\0x033A2DB4.wav", "Be advised, suspect is armed.", "scripted_lines"); } }
        public static ScannerFile HASH040A5429 { get { return new ScannerFile("01_scripted_lines\\0x040A5429.wav", "0x040A5429", "scripted_lines"); } }
        public static ScannerFile HASH04152C67 { get { return new ScannerFile("01_scripted_lines\\0x04152C67.wav", "0x04152C67", "scripted_lines"); } }
        public static ScannerFile HASH04907C35 { get { return new ScannerFile("01_scripted_lines\\0x04907C35.wav", "0x04907C35", "scripted_lines"); } }
        public static ScannerFile HASH04977E6C { get { return new ScannerFile("01_scripted_lines\\0x04977E6C.wav", "0x04977E6C", "scripted_lines"); } }
        public static ScannerFile HASH04A776F2 { get { return new ScannerFile("01_scripted_lines\\0x04A776F2.wav", "0x04A776F2", "scripted_lines"); } }
        public static ScannerFile HASH04C12DC7 { get { return new ScannerFile("01_scripted_lines\\0x04C12DC7.wav", "0x04C12DC7", "scripted_lines"); } }
        public static ScannerFile HASH04C47163 { get { return new ScannerFile("01_scripted_lines\\0x04C47163.wav", "0x04C47163", "scripted_lines"); } }
        public static ScannerFile HASH04D1464C { get { return new ScannerFile("01_scripted_lines\\0x04D1464C.wav", "0x04D1464C", "scripted_lines"); } }
        public static ScannerFile HASH04F54D95 { get { return new ScannerFile("01_scripted_lines\\0x04F54D95.wav", "0x04F54D95", "scripted_lines"); } }
        public static ScannerFile HASH0524C7F7 { get { return new ScannerFile("01_scripted_lines\\0x0524C7F7.wav", "0x0524C7F7", "scripted_lines"); } }
        public static ScannerFile HASH0550EC10 { get { return new ScannerFile("01_scripted_lines\\0x0550EC10.wav", "0x0550EC10", "scripted_lines"); } }
        public static ScannerFile HASH056D892C { get { return new ScannerFile("01_scripted_lines\\0x056D892C.wav", "0x056D892C", "scripted_lines"); } }
        public static ScannerFile HASH05D97E65 { get { return new ScannerFile("01_scripted_lines\\0x05D97E65.wav", "0x05D97E65", "scripted_lines"); } }
        public static ScannerFile HASH065281C6 { get { return new ScannerFile("01_scripted_lines\\0x065281C6.wav", "0x065281C6", "scripted_lines"); } }
        public static ScannerFile HASH067F9B20 { get { return new ScannerFile("01_scripted_lines\\0x067F9B20.wav", "0x067F9B20", "scripted_lines"); } }
        public static ScannerFile HASH06FEFB5C { get { return new ScannerFile("01_scripted_lines\\0x06FEFB5C.wav", "0x06FEFB5C", "scripted_lines"); } }
        public static ScannerFile HASH07337749 { get { return new ScannerFile("01_scripted_lines\\0x07337749.wav", "0x07337749", "scripted_lines"); } }
        public static ScannerFile HASH07D987FF { get { return new ScannerFile("01_scripted_lines\\0x07D987FF.wav", "0x07D987FF", "scripted_lines"); } }
        public static ScannerFile HASH07DCD88F { get { return new ScannerFile("01_scripted_lines\\0x07DCD88F.wav", "0x07DCD88F", "scripted_lines"); } }
        public static ScannerFile HASH08137472 { get { return new ScannerFile("01_scripted_lines\\0x08137472.wav", "0x08137472", "scripted_lines"); } }
        public static ScannerFile HASH08138AE8 { get { return new ScannerFile("01_scripted_lines\\0x08138AE8.wav", "0x08138AE8", "scripted_lines"); } }
        public static ScannerFile HASH083EB2E3 { get { return new ScannerFile("01_scripted_lines\\0x083EB2E3.wav", "0x083EB2E3", "scripted_lines"); } }
        public static ScannerFile HASH084969C1 { get { return new ScannerFile("01_scripted_lines\\0x084969C1.wav", "0x084969C1", "scripted_lines"); } }
        public static ScannerFile HASH08B98F20 { get { return new ScannerFile("01_scripted_lines\\0x08B98F20.wav", "0x08B98F20", "scripted_lines"); } }
        public static ScannerFile HASH08D6E5A5 { get { return new ScannerFile("01_scripted_lines\\0x08D6E5A5.wav", "0x08D6E5A5", "scripted_lines"); } }
        public static ScannerFile HASH08E29570 { get { return new ScannerFile("01_scripted_lines\\0x08E29570.wav", "0x08E29570", "scripted_lines"); } }
        public static ScannerFile HASH091C12E0 { get { return new ScannerFile("01_scripted_lines\\0x091C12E0.wav", "0x091C12E0", "scripted_lines"); } }
        public static ScannerFile HASH0927CCA7 { get { return new ScannerFile("01_scripted_lines\\0x0927CCA7.wav", "0x0927CCA7", "scripted_lines"); } }
        public static ScannerFile HASH093D0F23 { get { return new ScannerFile("01_scripted_lines\\0x093D0F23.wav", "0x093D0F23", "scripted_lines"); } }
        public static ScannerFile HASH097B0A47 { get { return new ScannerFile("01_scripted_lines\\0x097B0A47.wav", "0x097B0A47", "scripted_lines"); } }
        public static ScannerFile HASH09814DE2 { get { return new ScannerFile("01_scripted_lines\\0x09814DE2.wav", "0x09814DE2", "scripted_lines"); } }
        public static ScannerFile HASH09ADEC88 { get { return new ScannerFile("01_scripted_lines\\0x09ADEC88.wav", "0x09ADEC88", "scripted_lines"); } }
        public static ScannerFile HASH0A053B49 { get { return new ScannerFile("01_scripted_lines\\0x0A053B49.wav", "0x0A053B49", "scripted_lines"); } }
        public static ScannerFile HASH0A1B3E4C { get { return new ScannerFile("01_scripted_lines\\0x0A1B3E4C.wav", "0x0A1B3E4C", "scripted_lines"); } }
        public static ScannerFile HASH0A29EB23 { get { return new ScannerFile("01_scripted_lines\\0x0A29EB23.wav", "0x0A29EB23", "scripted_lines"); } }
        public static ScannerFile HASH0AA97460 { get { return new ScannerFile("01_scripted_lines\\0x0AA97460.wav", "0x0AA97460", "scripted_lines"); } }
        public static ScannerFile HASH0AD51F6A { get { return new ScannerFile("01_scripted_lines\\0x0AD51F6A.wav", "0x0AD51F6A", "scripted_lines"); } }
        public static ScannerFile HASH0AF13A84 { get { return new ScannerFile("01_scripted_lines\\0x0AF13A84.wav", "0x0AF13A84", "scripted_lines"); } }
        public static ScannerFile HASH0AFD1BC2 { get { return new ScannerFile("01_scripted_lines\\0x0AFD1BC2.wav", "0x0AFD1BC2", "scripted_lines"); } }
        public static ScannerFile HASH0B01A425 { get { return new ScannerFile("01_scripted_lines\\0x0B01A425.wav", "0x0B01A425", "scripted_lines"); } }
        public static ScannerFile HASH0B09BEF6 { get { return new ScannerFile("01_scripted_lines\\0x0B09BEF6.wav", "0x0B09BEF6", "scripted_lines"); } }
        public static ScannerFile HASH0B7DC711 { get { return new ScannerFile("01_scripted_lines\\0x0B7DC711.wav", "0x0B7DC711", "scripted_lines"); } }
        public static ScannerFile HASH0B885CD8 { get { return new ScannerFile("01_scripted_lines\\0x0B885CD8.wav", "0x0B885CD8", "scripted_lines"); } }
        public static ScannerFile HASH0B902335 { get { return new ScannerFile("01_scripted_lines\\0x0B902335.wav", "0x0B902335", "scripted_lines"); } }
        public static ScannerFile HASH0B9C2AC1 { get { return new ScannerFile("01_scripted_lines\\0x0B9C2AC1.wav", "0x0B9C2AC1", "scripted_lines"); } }
        public static ScannerFile HASH0BDD30E8 { get { return new ScannerFile("01_scripted_lines\\0x0BDD30E8.wav", "0x0BDD30E8", "scripted_lines"); } }
        public static ScannerFile HASH0BF0ABD5 { get { return new ScannerFile("01_scripted_lines\\0x0BF0ABD5.wav", "0x0BF0ABD5", "scripted_lines"); } }
        public static ScannerFile HASH0C1B7C74 { get { return new ScannerFile("01_scripted_lines\\0x0C1B7C74.wav", "0x0C1B7C74", "scripted_lines"); } }
        public static ScannerFile HASH0C3C8054 { get { return new ScannerFile("01_scripted_lines\\0x0C3C8054.wav", "0x0C3C8054", "scripted_lines"); } }
        public static ScannerFile HASH0C5713E5 { get { return new ScannerFile("01_scripted_lines\\0x0C5713E5.wav", "0x0C5713E5", "scripted_lines"); } }
        public static ScannerFile HASH0C5BC7B0 { get { return new ScannerFile("01_scripted_lines\\0x0C5BC7B0.wav", "0x0C5BC7B0", "scripted_lines"); } }
        public static ScannerFile HASH0C5F7B24 { get { return new ScannerFile("01_scripted_lines\\0x0C5F7B24.wav", "0x0C5F7B24", "scripted_lines"); } }
        public static ScannerFile HASH0CB286C4 { get { return new ScannerFile("01_scripted_lines\\0x0CB286C4.wav", "0x0CB286C4", "scripted_lines"); } }
        public static ScannerFile HASH0D321A78 { get { return new ScannerFile("01_scripted_lines\\0x0D321A78.wav", "0x0D321A78", "scripted_lines"); } }
        public static ScannerFile HASH0D7F5BA7 { get { return new ScannerFile("01_scripted_lines\\0x0D7F5BA7.wav", "0x0D7F5BA7", "scripted_lines"); } }
        public static ScannerFile HASH0DC66418 { get { return new ScannerFile("01_scripted_lines\\0x0DC66418.wav", "0x0DC66418", "scripted_lines"); } }
        public static ScannerFile HASH0DDE56F3 { get { return new ScannerFile("01_scripted_lines\\0x0DDE56F3.wav", "0x0DDE56F3", "scripted_lines"); } }
        public static ScannerFile HASH0DEBA202 { get { return new ScannerFile("01_scripted_lines\\0x0DEBA202.wav", "0x0DEBA202", "scripted_lines"); } }
        public static ScannerFile HASH0DFFE738 { get { return new ScannerFile("01_scripted_lines\\0x0DFFE738.wav", "0x0DFFE738", "scripted_lines"); } }
        public static ScannerFile HASH0E2B252B { get { return new ScannerFile("01_scripted_lines\\0x0E2B252B.wav", "0x0E2B252B", "scripted_lines"); } }
        public static ScannerFile HASH0E5DDB1B { get { return new ScannerFile("01_scripted_lines\\0x0E5DDB1B.wav", "0x0E5DDB1B", "scripted_lines"); } }
        public static ScannerFile HASH0E8C2586 { get { return new ScannerFile("01_scripted_lines\\0x0E8C2586.wav", "0x0E8C2586", "scripted_lines"); } }
        public static ScannerFile HASH0ED3BB7F { get { return new ScannerFile("01_scripted_lines\\0x0ED3BB7F.wav", "0x0ED3BB7F", "scripted_lines"); } }
        public static ScannerFile HASH0EF44F2C { get { return new ScannerFile("01_scripted_lines\\0x0EF44F2C.wav", "0x0EF44F2C", "scripted_lines"); } }
        public static ScannerFile HASH0F1D6717 { get { return new ScannerFile("01_scripted_lines\\0x0F1D6717.wav", "0x0F1D6717", "scripted_lines"); } }
        public static ScannerFile HASH0F2A4E66 { get { return new ScannerFile("01_scripted_lines\\0x0F2A4E66.wav", "0x0F2A4E66", "scripted_lines"); } }
        public static ScannerFile HASH0F57ACB1 { get { return new ScannerFile("01_scripted_lines\\0x0F57ACB1.wav", "0x0F57ACB1", "scripted_lines"); } }
        public static ScannerFile HASH0F704604 { get { return new ScannerFile("01_scripted_lines\\0x0F704604.wav", "0x0F704604", "scripted_lines"); } }
        public static ScannerFile HASH0F8255E1 { get { return new ScannerFile("01_scripted_lines\\0x0F8255E1.wav", "0x0F8255E1", "scripted_lines"); } }
        public static ScannerFile HASH0FD0D6F5 { get { return new ScannerFile("01_scripted_lines\\0x0FD0D6F5.wav", "0x0FD0D6F5", "scripted_lines"); } }
        public static ScannerFile HASH10458A9D { get { return new ScannerFile("01_scripted_lines\\0x10458A9D.wav", "0x10458A9D", "scripted_lines"); } }
        public static ScannerFile HASH1075F4DC { get { return new ScannerFile("01_scripted_lines\\0x1075F4DC.wav", "0x1075F4DC", "scripted_lines"); } }
        public static ScannerFile HASH1078E105 { get { return new ScannerFile("01_scripted_lines\\0x1078E105.wav", "0x1078E105", "scripted_lines"); } }
        public static ScannerFile HASH108405AA { get { return new ScannerFile("01_scripted_lines\\0x108405AA.wav", "0x108405AA", "scripted_lines"); } }
        public static ScannerFile HASH10B7612A { get { return new ScannerFile("01_scripted_lines\\0x10B7612A.wav", "0x10B7612A", "scripted_lines"); } }
        public static ScannerFile HASH10E29C6E { get { return new ScannerFile("01_scripted_lines\\0x10E29C6E.wav", "0x10E29C6E", "scripted_lines"); } }
        public static ScannerFile HASH10EB80E5 { get { return new ScannerFile("01_scripted_lines\\0x10EB80E5.wav", "0x10EB80E5", "scripted_lines"); } }
        public static ScannerFile HASH10EDE805 { get { return new ScannerFile("01_scripted_lines\\0x10EDE805.wav", "0x10EDE805", "scripted_lines"); } }
        public static ScannerFile HASH11174006 { get { return new ScannerFile("01_scripted_lines\\0x11174006.wav", "0x11174006", "scripted_lines"); } }
        public static ScannerFile HASH117239AB { get { return new ScannerFile("01_scripted_lines\\0x117239AB.wav", "0x117239AB", "scripted_lines"); } }
        public static ScannerFile HASH11946C00 { get { return new ScannerFile("01_scripted_lines\\0x11946C00.wav", "0x11946C00", "scripted_lines"); } }
        public static ScannerFile HASH119C4775 { get { return new ScannerFile("01_scripted_lines\\0x119C4775.wav", "0x119C4775", "scripted_lines"); } }
        public static ScannerFile HASH11A9D669 { get { return new ScannerFile("01_scripted_lines\\0x11A9D669.wav", "0x11A9D669", "scripted_lines"); } }
        public static ScannerFile HASH11CEB3EC { get { return new ScannerFile("01_scripted_lines\\0x11CEB3EC.wav", "0x11CEB3EC", "scripted_lines"); } }
        public static ScannerFile HASH11F7DC3A { get { return new ScannerFile("01_scripted_lines\\0x11F7DC3A.wav", "0x11F7DC3A", "scripted_lines"); } }
        public static ScannerFile HASH1259A260 { get { return new ScannerFile("01_scripted_lines\\0x1259A260.wav", "0x1259A260", "scripted_lines"); } }
        public static ScannerFile HASH125DAD47 { get { return new ScannerFile("01_scripted_lines\\0x125DAD47.wav", "0x125DAD47", "scripted_lines"); } }
        public static ScannerFile HASH1276F4B1 { get { return new ScannerFile("01_scripted_lines\\0x1276F4B1.wav", "0x1276F4B1", "scripted_lines"); } }
        public static ScannerFile HASH12884CEB { get { return new ScannerFile("01_scripted_lines\\0x12884CEB.wav", "0x12884CEB", "scripted_lines"); } }
        public static ScannerFile HASH1288DA4F { get { return new ScannerFile("01_scripted_lines\\0x1288DA4F.wav", "0x1288DA4F", "scripted_lines"); } }
        public static ScannerFile HASH12AEE908 { get { return new ScannerFile("01_scripted_lines\\0x12AEE908.wav", "0x12AEE908", "scripted_lines"); } }
        public static ScannerFile HASH12B349B2 { get { return new ScannerFile("01_scripted_lines\\0x12B349B2.wav", "0x12B349B2", "scripted_lines"); } }
        public static ScannerFile HASH130F07AC { get { return new ScannerFile("01_scripted_lines\\0x130F07AC.wav", "0x130F07AC", "scripted_lines"); } }
        public static ScannerFile HASH1334E4BA { get { return new ScannerFile("01_scripted_lines\\0x1334E4BA.wav", "0x1334E4BA", "scripted_lines"); } }
        public static ScannerFile HASH141E9AF0 { get { return new ScannerFile("01_scripted_lines\\0x141E9AF0.wav", "0x141E9AF0", "scripted_lines"); } }
        public static ScannerFile HASH14532861 { get { return new ScannerFile("01_scripted_lines\\0x14532861.wav", "0x14532861", "scripted_lines"); } }
        public static ScannerFile HASH14F98E38 { get { return new ScannerFile("01_scripted_lines\\0x14F98E38.wav", "0x14F98E38", "scripted_lines"); } }
        public static ScannerFile HASH152C3020 { get { return new ScannerFile("01_scripted_lines\\0x152C3020.wav", "0x152C3020", "scripted_lines"); } }
        public static ScannerFile HASH158B2362 { get { return new ScannerFile("01_scripted_lines\\0x158B2362.wav", "0x158B2362", "scripted_lines"); } }
        public static ScannerFile HASH15F6451B { get { return new ScannerFile("01_scripted_lines\\0x15F6451B.wav", "0x15F6451B", "scripted_lines"); } }
        public static ScannerFile HASH165078B5 { get { return new ScannerFile("01_scripted_lines\\0x165078B5.wav", "0x165078B5", "scripted_lines"); } }
        public static ScannerFile HASH167E6DA6 { get { return new ScannerFile("01_scripted_lines\\0x167E6DA6.wav", "0x167E6DA6", "scripted_lines"); } }
        public static ScannerFile HASH169BA7E0 { get { return new ScannerFile("01_scripted_lines\\0x169BA7E0.wav", "0x169BA7E0", "scripted_lines"); } }
        public static ScannerFile HASH16B27BD2 { get { return new ScannerFile("01_scripted_lines\\0x16B27BD2.wav", "0x16B27BD2", "scripted_lines"); } }
        public static ScannerFile HASH16D57635 { get { return new ScannerFile("01_scripted_lines\\0x16D57635.wav", "0x16D57635", "scripted_lines"); } }
        public static ScannerFile HASH16DF54FD { get { return new ScannerFile("01_scripted_lines\\0x16DF54FD.wav", "0x16DF54FD", "scripted_lines"); } }
        public static ScannerFile HASH16E76B7B { get { return new ScannerFile("01_scripted_lines\\0x16E76B7B.wav", "0x16E76B7B", "scripted_lines"); } }
        public static ScannerFile HASH16F228A5 { get { return new ScannerFile("01_scripted_lines\\0x16F228A5.wav", "0x16F228A5", "scripted_lines"); } }
        public static ScannerFile HASH16FD3DC6 { get { return new ScannerFile("01_scripted_lines\\0x16FD3DC6.wav", "0x16FD3DC6", "scripted_lines"); } }
        public static ScannerFile HASH171931DD { get { return new ScannerFile("01_scripted_lines\\0x171931DD.wav", "0x171931DD", "scripted_lines"); } }
        public static ScannerFile HASH178BEBC0 { get { return new ScannerFile("01_scripted_lines\\0x178BEBC0.wav", "0x178BEBC0", "scripted_lines"); } }
        public static ScannerFile HASH17AC2479 { get { return new ScannerFile("01_scripted_lines\\0x17AC2479.wav", "0x17AC2479", "scripted_lines"); } }
        public static ScannerFile HASH18115D81 { get { return new ScannerFile("01_scripted_lines\\0x18115D81.wav", "0x18115D81", "scripted_lines"); } }
        public static ScannerFile HASH1827FEB0 { get { return new ScannerFile("01_scripted_lines\\0x1827FEB0.wav", "0x1827FEB0", "scripted_lines"); } }
        public static ScannerFile HASH183E1F75 { get { return new ScannerFile("01_scripted_lines\\0x183E1F75.wav", "0x183E1F75", "scripted_lines"); } }
        public static ScannerFile HASH1846C75D { get { return new ScannerFile("01_scripted_lines\\0x1846C75D.wav", "0x1846C75D", "scripted_lines"); } }
        public static ScannerFile HASH18843C41 { get { return new ScannerFile("01_scripted_lines\\0x18843C41.wav", "0x18843C41", "scripted_lines"); } }
        public static ScannerFile HASH18A02156 { get { return new ScannerFile("01_scripted_lines\\0x18A02156.wav", "0x18A02156", "scripted_lines"); } }
        public static ScannerFile HASH18DEFFCD { get { return new ScannerFile("01_scripted_lines\\0x18DEFFCD.wav", "0x18DEFFCD", "scripted_lines"); } }
        public static ScannerFile HASH19296CAA { get { return new ScannerFile("01_scripted_lines\\0x19296CAA.wav", "0x19296CAA", "scripted_lines"); } }
        public static ScannerFile HASH194740B0 { get { return new ScannerFile("01_scripted_lines\\0x194740B0.wav", "0x194740B0", "scripted_lines"); } }
        public static ScannerFile HASH19505B83 { get { return new ScannerFile("01_scripted_lines\\0x19505B83.wav", "0x19505B83", "scripted_lines"); } }
        public static ScannerFile HASH196B32E0 { get { return new ScannerFile("01_scripted_lines\\0x196B32E0.wav", "0x196B32E0", "scripted_lines"); } }
        public static ScannerFile HASH19B73C44 { get { return new ScannerFile("01_scripted_lines\\0x19B73C44.wav", "0x19B73C44", "scripted_lines"); } }
        public static ScannerFile HASH1A1E2335 { get { return new ScannerFile("01_scripted_lines\\0x1A1E2335.wav", "0x1A1E2335", "scripted_lines"); } }
        public static ScannerFile HASH1A58D8EF { get { return new ScannerFile("01_scripted_lines\\0x1A58D8EF.wav", "0x1A58D8EF", "scripted_lines"); } }
        public static ScannerFile HASH1A9A3E12 { get { return new ScannerFile("01_scripted_lines\\0x1A9A3E12.wav", "0x1A9A3E12", "scripted_lines"); } }
        public static ScannerFile HASH1AA7D7B6 { get { return new ScannerFile("01_scripted_lines\\0x1AA7D7B6.wav", "0x1AA7D7B6", "scripted_lines"); } }
        public static ScannerFile HASH1ACBFE04 { get { return new ScannerFile("01_scripted_lines\\0x1ACBFE04.wav", "0x1ACBFE04", "scripted_lines"); } }
        public static ScannerFile HASH1B8DB7C3 { get { return new ScannerFile("01_scripted_lines\\0x1B8DB7C3.wav", "0x1B8DB7C3", "scripted_lines"); } }
        public static ScannerFile HASH1BE3AF18 { get { return new ScannerFile("01_scripted_lines\\0x1BE3AF18.wav", "0x1BE3AF18", "scripted_lines"); } }
        public static ScannerFile HASH1C34C0F5 { get { return new ScannerFile("01_scripted_lines\\0x1C34C0F5.wav", "0x1C34C0F5", "scripted_lines"); } }
        public static ScannerFile HASH1C4BE8AD { get { return new ScannerFile("01_scripted_lines\\0x1C4BE8AD.wav", "0x1C4BE8AD", "scripted_lines"); } }
        public static ScannerFile HASH1C5303DF { get { return new ScannerFile("01_scripted_lines\\0x1C5303DF.wav", "0x1C5303DF", "scripted_lines"); } }
        public static ScannerFile HASH1C5727B4 { get { return new ScannerFile("01_scripted_lines\\0x1C5727B4.wav", "0x1C5727B4", "scripted_lines"); } }
        public static ScannerFile HASH1C66569D { get { return new ScannerFile("01_scripted_lines\\0x1C66569D.wav", "0x1C66569D", "scripted_lines"); } }
        public static ScannerFile HASH1C76B3B4 { get { return new ScannerFile("01_scripted_lines\\0x1C76B3B4.wav", "0x1C76B3B4", "scripted_lines"); } }
        public static ScannerFile HASH1C7BC165 { get { return new ScannerFile("01_scripted_lines\\0x1C7BC165.wav", "0x1C7BC165", "scripted_lines"); } }
        public static ScannerFile HASH1C89DA80 { get { return new ScannerFile("01_scripted_lines\\0x1C89DA80.wav", "0x1C89DA80", "scripted_lines"); } }
        public static ScannerFile HASH1CA4F480 { get { return new ScannerFile("01_scripted_lines\\0x1CA4F480.wav", "0x1CA4F480", "scripted_lines"); } }
        public static ScannerFile HASH1CB9B1BE { get { return new ScannerFile("01_scripted_lines\\0x1CB9B1BE.wav", "0x1CB9B1BE", "scripted_lines"); } }
        public static ScannerFile HASH1CBFFF47 { get { return new ScannerFile("01_scripted_lines\\0x1CBFFF47.wav", "0x1CBFFF47", "scripted_lines"); } }
        public static ScannerFile HASH1D148E21 { get { return new ScannerFile("01_scripted_lines\\0x1D148E21.wav", "0x1D148E21", "scripted_lines"); } }
        public static ScannerFile HASH1D19EB77 { get { return new ScannerFile("01_scripted_lines\\0x1D19EB77.wav", "0x1D19EB77", "scripted_lines"); } }
        public static ScannerFile HASH1D4C8E22 { get { return new ScannerFile("01_scripted_lines\\0x1D4C8E22.wav", "0x1D4C8E22", "scripted_lines"); } }
        public static ScannerFile HASH1D4F5F3F { get { return new ScannerFile("01_scripted_lines\\0x1D4F5F3F.wav", "0x1D4F5F3F", "scripted_lines"); } }
        public static ScannerFile HASH1D8203E2 { get { return new ScannerFile("01_scripted_lines\\0x1D8203E2.wav", "0x1D8203E2", "scripted_lines"); } }
        public static ScannerFile HASH1D894914 { get { return new ScannerFile("01_scripted_lines\\0x1D894914.wav", "0x1D894914", "scripted_lines"); } }
        public static ScannerFile HASH1DA7CF40 { get { return new ScannerFile("01_scripted_lines\\0x1DA7CF40.wav", "0x1DA7CF40", "scripted_lines"); } }
        public static ScannerFile HASH1DB18B27 { get { return new ScannerFile("01_scripted_lines\\0x1DB18B27.wav", "0x1DB18B27", "scripted_lines"); } }
        public static ScannerFile HASH1DBD4790 { get { return new ScannerFile("01_scripted_lines\\0x1DBD4790.wav", "0x1DBD4790", "scripted_lines"); } }
        public static ScannerFile HASH1DC689AF { get { return new ScannerFile("01_scripted_lines\\0x1DC689AF.wav", "0x1DC689AF", "scripted_lines"); } }
        public static ScannerFile HASH1DE12E58 { get { return new ScannerFile("01_scripted_lines\\0x1DE12E58.wav", "0x1DE12E58", "scripted_lines"); } }
        public static ScannerFile HASH1DFF2F14 { get { return new ScannerFile("01_scripted_lines\\0x1DFF2F14.wav", "0x1DFF2F14", "scripted_lines"); } }
        public static ScannerFile HASH1E133BCE { get { return new ScannerFile("01_scripted_lines\\0x1E133BCE.wav", "0x1E133BCE", "scripted_lines"); } }
        public static ScannerFile HASH1E34857B { get { return new ScannerFile("01_scripted_lines\\0x1E34857B.wav", "0x1E34857B", "scripted_lines"); } }
        public static ScannerFile HASH1E8024DB { get { return new ScannerFile("01_scripted_lines\\0x1E8024DB.wav", "0x1E8024DB", "scripted_lines"); } }
        public static ScannerFile HASH1E89D108 { get { return new ScannerFile("01_scripted_lines\\0x1E89D108.wav", "0x1E89D108", "scripted_lines"); } }
        public static ScannerFile HASH1E9DE1DD { get { return new ScannerFile("01_scripted_lines\\0x1E9DE1DD.wav", "0x1E9DE1DD", "scripted_lines"); } }
        public static ScannerFile HASH1EA59FB1 { get { return new ScannerFile("01_scripted_lines\\0x1EA59FB1.wav", "0x1EA59FB1", "scripted_lines"); } }
        public static ScannerFile HASH1EB09F1D { get { return new ScannerFile("01_scripted_lines\\0x1EB09F1D.wav", "0x1EB09F1D", "scripted_lines"); } }
        public static ScannerFile HASH1ED51471 { get { return new ScannerFile("01_scripted_lines\\0x1ED51471.wav", "0x1ED51471", "scripted_lines"); } }
        public static ScannerFile HASH1F71BEF7 { get { return new ScannerFile("01_scripted_lines\\0x1F71BEF7.wav", "0x1F71BEF7", "scripted_lines"); } }
        public static ScannerFile HASH1F9FBA77 { get { return new ScannerFile("01_scripted_lines\\0x1F9FBA77.wav", "0x1F9FBA77", "scripted_lines"); } }
        public static ScannerFile HASH1FAB0581 { get { return new ScannerFile("01_scripted_lines\\0x1FAB0581.wav", "0x1FAB0581", "scripted_lines"); } }
        public static ScannerFile HASH1FC130C5 { get { return new ScannerFile("01_scripted_lines\\0x1FC130C5.wav", "0x1FC130C5", "scripted_lines"); } }
        public static ScannerFile HASH1FD90887 { get { return new ScannerFile("01_scripted_lines\\0x1FD90887.wav", "0x1FD90887", "scripted_lines"); } }
    }
    public class special_instructions_approach
    {
        public static ScannerFile Engagefromthe { get { return new ScannerFile("01_special_instructions_approach\\0x05B3288B.wav", "Engage from the...", "special_instructions_approach"); } }
        public static ScannerFile Approachfromthe { get { return new ScannerFile("01_special_instructions_approach\\0x0E2BF983.wav", "Approach from the...", "special_instructions_approach"); } }
        public static ScannerFile Engagefromthe1 { get { return new ScannerFile("01_special_instructions_approach\\0x13EF4503.wav", "Engage...from the...", "special_instructions_approach"); } }
        public static ScannerFile Convenefromthe { get { return new ScannerFile("01_special_instructions_approach\\0x18644DEE.wav", "Convene from the...", "special_instructions_approach"); } }
        public static ScannerFile Approachfromthe1 { get { return new ScannerFile("01_special_instructions_approach\\0x1FF51D16.wav", "Approach from the...", "special_instructions_approach"); } }
    }
    public class special_ins_app_dir
    {
        public static ScannerFile West { get { return new ScannerFile("01_special_ins_app_dir\\0x00D13053.wav", "West.", "special_ins_app_dir"); } }
        public static ScannerFile South { get { return new ScannerFile("01_special_ins_app_dir\\0x01CD1C1A.wav", "South.", "special_ins_app_dir"); } }
        public static ScannerFile Rear { get { return new ScannerFile("01_special_ins_app_dir\\0x08633D59.wav", "Rear.", "special_ins_app_dir"); } }
        public static ScannerFile Eastside { get { return new ScannerFile("01_special_ins_app_dir\\0x0B61A4C6.wav", "East side.", "special_ins_app_dir"); } }
        public static ScannerFile Northentrance { get { return new ScannerFile("01_special_ins_app_dir\\0x0C81C88E.wav", "North entrance.", "special_ins_app_dir"); } }
        public static ScannerFile Eastentrance { get { return new ScannerFile("01_special_ins_app_dir\\0x0D6B53E7.wav", "East entrance.", "special_ins_app_dir"); } }
        public static ScannerFile Frontentrance { get { return new ScannerFile("01_special_ins_app_dir\\0x0DAF97CD.wav", "Front entrance.", "special_ins_app_dir"); } }
        public static ScannerFile Westentrance { get { return new ScannerFile("01_special_ins_app_dir\\0x0F81625C.wav", "West entrance.", "special_ins_app_dir"); } }
        public static ScannerFile Northside { get { return new ScannerFile("01_special_ins_app_dir\\0x1159147E.wav", "North side.", "special_ins_app_dir"); } }
        public static ScannerFile East { get { return new ScannerFile("01_special_ins_app_dir\\0x12F149E3.wav", "East.", "special_ins_app_dir"); } }
        public static ScannerFile Southside { get { return new ScannerFile("01_special_ins_app_dir\\0x14223FB2.wav", "South side.", "special_ins_app_dir"); } }
        public static ScannerFile North { get { return new ScannerFile("01_special_ins_app_dir\\0x16D1886A.wav", "North.", "special_ins_app_dir"); } }
        public static ScannerFile Southentrance { get { return new ScannerFile("01_special_ins_app_dir\\0x1B075B88.wav", "South entrance.", "special_ins_app_dir"); } }
        public static ScannerFile Front { get { return new ScannerFile("01_special_ins_app_dir\\0x1D1883D9.wav", "Front.", "special_ins_app_dir"); } }
        public static ScannerFile Rearentrance { get { return new ScannerFile("01_special_ins_app_dir\\0x1DB770EB.wav", "Rear entrance.", "special_ins_app_dir"); } }
        public static ScannerFile Westside { get { return new ScannerFile("01_special_ins_app_dir\\0x1F58477F.wav", "West side.", "special_ins_app_dir"); } }
    }
    public class special_vehicle
    {
        public static ScannerFile Drivingadigger { get { return new ScannerFile("01_special_vehicle\\0x000D2597.wav", "Driving a digger.", "special_vehicle"); } }
        public static ScannerFile Flyingamilitaryaircraft { get { return new ScannerFile("01_special_vehicle\\0x00BA742F.wav", "Flying a military aircraft.", "special_vehicle"); } }
        public static ScannerFile Drivingatank { get { return new ScannerFile("01_special_vehicle\\0x01684C1F.wav", "Driving a tank.", "special_vehicle"); } }
        public static ScannerFile Drivingapolicecar { get { return new ScannerFile("01_special_vehicle\\0x023F3422.wav", "Driving a police car.", "special_vehicle"); } }
        public static ScannerFile Onadarkcoloredhorse { get { return new ScannerFile("01_special_vehicle\\0x02C1BD67.wav", "On a dark-colored horse.", "special_vehicle"); } }
        public static ScannerFile Drivingadumptruck { get { return new ScannerFile("01_special_vehicle\\0x04474592.wav", "Driving a dump truck.", "special_vehicle"); } }
        public static ScannerFile Ridingacustomizedmotorcycle { get { return new ScannerFile("01_special_vehicle\\0x059DD2D4.wav", "Riding a customized motorcycle.", "special_vehicle"); } }
        public static ScannerFile Ridingamountainbike { get { return new ScannerFile("01_special_vehicle\\0x05AA96B3.wav", "Riding a mountain bike.", "special_vehicle"); } }
        public static ScannerFile Onayacht { get { return new ScannerFile("01_special_vehicle\\0x05DA0986.wav", "On a yacht.", "special_vehicle"); } }
        public static ScannerFile OperatinganAnnihilatorhelicopter { get { return new ScannerFile("01_special_vehicle\\0x06B3CFF4.wav", "Operating an Annihilator helicopter.", "special_vehicle"); } }
        public static ScannerFile Onaschooner { get { return new ScannerFile("01_special_vehicle\\0x07158FF4.wav", "On a schooner.", "special_vehicle"); } }
        public static ScannerFile OperatinganAnnihilatorAttackHelicopter { get { return new ScannerFile("01_special_vehicle\\0x079051AE.wav", "Operating an Annihilator Attack Helicopter.", "special_vehicle"); } }
        public static ScannerFile Flyingacivilianairplane { get { return new ScannerFile("01_special_vehicle\\0x079FB220.wav", "Flying a civilian airplane.", "special_vehicle"); } }
        public static ScannerFile Operatingamilitaryhelicopter { get { return new ScannerFile("01_special_vehicle\\0x07BB9776.wav", "Operating a military helicopter.", "special_vehicle"); } }
        public static ScannerFile Onalightcoloredhorse { get { return new ScannerFile("01_special_vehicle\\0x0840074D.wav", "On a light-colored horse.", "special_vehicle"); } }
        public static ScannerFile Drivinganambulance { get { return new ScannerFile("01_special_vehicle\\0x0A95F1BD.wav", "Driving an ambulance.", "special_vehicle"); } }
        public static ScannerFile Ridingablackhorse { get { return new ScannerFile("01_special_vehicle\\0x0ACBBD8F.wav", "Riding a black horse.", "special_vehicle"); } }
        public static ScannerFile Drivingagarbagetruck { get { return new ScannerFile("01_special_vehicle\\0x0B1FA404.wav", "Driving a garbage truck.", "special_vehicle"); } }
        public static ScannerFile OperatingaMaverickhelicopter { get { return new ScannerFile("01_special_vehicle\\0x0B9D5088.wav", "Operating a Maverick helicopter.", "special_vehicle"); } }
        public static ScannerFile Drivingabulldozer { get { return new ScannerFile("01_special_vehicle\\0x0C505B49.wav", "Driving a bulldozer.", "special_vehicle"); } }
        public static ScannerFile Ridingawhitehorse { get { return new ScannerFile("01_special_vehicle\\0x0D808D20.wav", "Riding a white horse.", "special_vehicle"); } }
        public static ScannerFile Drivingapolicesedan { get { return new ScannerFile("01_special_vehicle\\0x0E0C0BBC.wav", "Driving a police sedan.", "special_vehicle"); } }
        public static ScannerFile Onabrownhorse { get { return new ScannerFile("01_special_vehicle\\0x0E38C84F.wav", "On a brown horse.", "special_vehicle"); } }
        public static ScannerFile Flyingamilitaryairplane { get { return new ScannerFile("01_special_vehicle\\0x0FEC9294.wav", "Flying a military airplane.", "special_vehicle"); } }
        public static ScannerFile Ridingaracingbike { get { return new ScannerFile("01_special_vehicle\\0x10F1559B.wav", "Riding a racing bike.", "special_vehicle"); } }
        public static ScannerFile Ridingadarkcoloredhorse { get { return new ScannerFile("01_special_vehicle\\0x110B19FA.wav", "Riding a dark-colored horse.", "special_vehicle"); } }
        public static ScannerFile Flyinganairplane { get { return new ScannerFile("01_special_vehicle\\0x11E586AD.wav", "Flying an airplane.", "special_vehicle"); } }
        public static ScannerFile RidingaBMXbike { get { return new ScannerFile("01_special_vehicle\\0x12C22CBB.wav", "Riding a BMX bike.", "special_vehicle"); } }
        public static ScannerFile Drivingatippertruck { get { return new ScannerFile("01_special_vehicle\\0x1330A850.wav", "Driving a tipper truck.", "special_vehicle"); } }
        public static ScannerFile Ridingamotorcycle { get { return new ScannerFile("01_special_vehicle\\0x13A1BDA3.wav", "Riding a motorcycle.", "special_vehicle"); } }
        public static ScannerFile PilotinganAnnihilatorhelicopter { get { return new ScannerFile("01_special_vehicle\\0x142D6AE7.wav", "Piloting an Annihilator helicopter.", "special_vehicle"); } }
        public static ScannerFile Onaboat { get { return new ScannerFile("01_special_vehicle\\0x144919B6.wav", "On a boat.", "special_vehicle"); } }
        public static ScannerFile Flyingamilitaryhelicopter { get { return new ScannerFile("01_special_vehicle\\0x15EAF3D3.wav", "Flying a military helicopter.", "special_vehicle"); } }
        public static ScannerFile FlyingaVTOLaircraft { get { return new ScannerFile("01_special_vehicle\\0x186F5839.wav", "Flying a VTOL aircraft.", "special_vehicle"); } }
        public static ScannerFile Ridingalightcoloredhorse { get { return new ScannerFile("01_special_vehicle\\0x191228F1.wav", "Riding a light-colored horse.", "special_vehicle"); } }
        public static ScannerFile Onawhitehorse { get { return new ScannerFile("01_special_vehicle\\0x1B3E289B.wav", "On a white horse.", "special_vehicle"); } }
        public static ScannerFile Onablackhorse { get { return new ScannerFile("01_special_vehicle\\0x1C5DE0B3.wav", "On a black horse.", "special_vehicle"); } }
        public static ScannerFile Ridingabrownhorse { get { return new ScannerFile("01_special_vehicle\\0x1CCB2569.wav", "Riding a brown horse.", "special_vehicle"); } }
        public static ScannerFile PilotingaMaverickhelicopter { get { return new ScannerFile("01_special_vehicle\\0x1D77B43D.wav", "Piloting a Maverick helicopter.", "special_vehicle"); } }
    }
    public class specific_location
    {
        public static ScannerFile HASH000E7300 { get { return new ScannerFile("01_specific_location\\0x000E7300.wav", "0x000E7300", "specific_location"); } }
        public static ScannerFile HASH0011827A { get { return new ScannerFile("01_specific_location\\0x0011827A.wav", "0x0011827A", "specific_location"); } }
        public static ScannerFile HASH0044397F { get { return new ScannerFile("01_specific_location\\0x0044397F.wav", "0x0044397F", "specific_location"); } }
        public static ScannerFile HASH005E5414 { get { return new ScannerFile("01_specific_location\\0x005E5414.wav", "0x005E5414", "specific_location"); } }
        public static ScannerFile HASH007AC3FC { get { return new ScannerFile("01_specific_location\\0x007AC3FC.wav", "0x007AC3FC", "specific_location"); } }
        public static ScannerFile HASH0092CBCB { get { return new ScannerFile("01_specific_location\\0x0092CBCB.wav", "0x0092CBCB", "specific_location"); } }
        public static ScannerFile HASH00CA973E { get { return new ScannerFile("01_specific_location\\0x00CA973E.wav", "0x00CA973E", "specific_location"); } }
        public static ScannerFile HASH00EF8558 { get { return new ScannerFile("01_specific_location\\0x00EF8558.wav", "0x00EF8558", "specific_location"); } }
        public static ScannerFile HASH00FEFA6E { get { return new ScannerFile("01_specific_location\\0x00FEFA6E.wav", "0x00FEFA6E", "specific_location"); } }
        public static ScannerFile HASH014AA746 { get { return new ScannerFile("01_specific_location\\0x014AA746.wav", "0x014AA746", "specific_location"); } }
        public static ScannerFile HASH017D2BE2 { get { return new ScannerFile("01_specific_location\\0x017D2BE2.wav", "0x017D2BE2", "specific_location"); } }
        public static ScannerFile HASH01800482 { get { return new ScannerFile("01_specific_location\\0x01800482.wav", "0x01800482", "specific_location"); } }
        public static ScannerFile HASH01A84F28 { get { return new ScannerFile("01_specific_location\\0x01A84F28.wav", "0x01A84F28", "specific_location"); } }
        public static ScannerFile HASH01CA4D77 { get { return new ScannerFile("01_specific_location\\0x01CA4D77.wav", "0x01CA4D77", "specific_location"); } }
        public static ScannerFile HASH021501AD { get { return new ScannerFile("01_specific_location\\0x021501AD.wav", "0x021501AD", "specific_location"); } }
        public static ScannerFile HASH02249C78 { get { return new ScannerFile("01_specific_location\\0x02249C78.wav", "0x02249C78", "specific_location"); } }
        public static ScannerFile HASH02389919 { get { return new ScannerFile("01_specific_location\\0x02389919.wav", "0x02389919", "specific_location"); } }
        public static ScannerFile HASH0289F802 { get { return new ScannerFile("01_specific_location\\0x0289F802.wav", "0x0289F802", "specific_location"); } }
        public static ScannerFile HASH02967EFD { get { return new ScannerFile("01_specific_location\\0x02967EFD.wav", "0x02967EFD", "specific_location"); } }
        public static ScannerFile HASH02A305DE { get { return new ScannerFile("01_specific_location\\0x02A305DE.wav", "0x02A305DE", "specific_location"); } }
        public static ScannerFile HASH02A53146 { get { return new ScannerFile("01_specific_location\\0x02A53146.wav", "0x02A53146", "specific_location"); } }
        public static ScannerFile HASH02C36B8B { get { return new ScannerFile("01_specific_location\\0x02C36B8B.wav", "0x02C36B8B", "specific_location"); } }
        public static ScannerFile HASH02DEC59F { get { return new ScannerFile("01_specific_location\\0x02DEC59F.wav", "0x02DEC59F", "specific_location"); } }
        public static ScannerFile HASH02EC9FB5 { get { return new ScannerFile("01_specific_location\\0x02EC9FB5.wav", "0x02EC9FB5", "specific_location"); } }
        public static ScannerFile HASH035776E6 { get { return new ScannerFile("01_specific_location\\0x035776E6.wav", "0x035776E6", "specific_location"); } }
        public static ScannerFile HASH03949A0F { get { return new ScannerFile("01_specific_location\\0x03949A0F.wav", "0x03949A0F", "specific_location"); } }
        public static ScannerFile HASH03A2A34A { get { return new ScannerFile("01_specific_location\\0x03A2A34A.wav", "0x03A2A34A", "specific_location"); } }
        public static ScannerFile HASH0431FE2B { get { return new ScannerFile("01_specific_location\\0x0431FE2B.wav", "0x0431FE2B", "specific_location"); } }
        public static ScannerFile HASH04510C42 { get { return new ScannerFile("01_specific_location\\0x04510C42.wav", "0x04510C42", "specific_location"); } }
        public static ScannerFile HASH045D23E3 { get { return new ScannerFile("01_specific_location\\0x045D23E3.wav", "0x045D23E3", "specific_location"); } }
        public static ScannerFile HASH04817594 { get { return new ScannerFile("01_specific_location\\0x04817594.wav", "0x04817594", "specific_location"); } }
        public static ScannerFile HASH04D70992 { get { return new ScannerFile("01_specific_location\\0x04D70992.wav", "0x04D70992", "specific_location"); } }
        public static ScannerFile HASH04F66C50 { get { return new ScannerFile("01_specific_location\\0x04F66C50.wav", "0x04F66C50", "specific_location"); } }
        public static ScannerFile HASH04F6FA01 { get { return new ScannerFile("01_specific_location\\0x04F6FA01.wav", "0x04F6FA01", "specific_location"); } }
        public static ScannerFile HASH051C5D0C { get { return new ScannerFile("01_specific_location\\0x051C5D0C.wav", "0x051C5D0C", "specific_location"); } }
        public static ScannerFile HASH05217FAC { get { return new ScannerFile("01_specific_location\\0x05217FAC.wav", "0x05217FAC", "specific_location"); } }
        public static ScannerFile HASH053A9900 { get { return new ScannerFile("01_specific_location\\0x053A9900.wav", "0x053A9900", "specific_location"); } }
        public static ScannerFile HASH05505BE8 { get { return new ScannerFile("01_specific_location\\0x05505BE8.wav", "0x05505BE8", "specific_location"); } }
        public static ScannerFile HASH056AF0EC { get { return new ScannerFile("01_specific_location\\0x056AF0EC.wav", "0x056AF0EC", "specific_location"); } }
        public static ScannerFile HASH059E6639 { get { return new ScannerFile("01_specific_location\\0x059E6639.wav", "0x059E6639", "specific_location"); } }
        public static ScannerFile HASH05AB836E { get { return new ScannerFile("01_specific_location\\0x05AB836E.wav", "0x05AB836E", "specific_location"); } }
        public static ScannerFile HASH068CED6A { get { return new ScannerFile("01_specific_location\\0x068CED6A.wav", "0x068CED6A", "specific_location"); } }
        public static ScannerFile HASH06AD518B { get { return new ScannerFile("01_specific_location\\0x06AD518B.wav", "0x06AD518B", "specific_location"); } }
        public static ScannerFile HASH06C75569 { get { return new ScannerFile("01_specific_location\\0x06C75569.wav", "0x06C75569", "specific_location"); } }
        public static ScannerFile HASH06EEB5C2 { get { return new ScannerFile("01_specific_location\\0x06EEB5C2.wav", "0x06EEB5C2", "specific_location"); } }
        public static ScannerFile HASH0723E151 { get { return new ScannerFile("01_specific_location\\0x0723E151.wav", "0x0723E151", "specific_location"); } }
        public static ScannerFile HASH0761393B { get { return new ScannerFile("01_specific_location\\0x0761393B.wav", "0x0761393B", "specific_location"); } }
        public static ScannerFile HASH076BF9A3 { get { return new ScannerFile("01_specific_location\\0x076BF9A3.wav", "0x076BF9A3", "specific_location"); } }
        public static ScannerFile HASH076E01B0 { get { return new ScannerFile("01_specific_location\\0x076E01B0.wav", "0x076E01B0", "specific_location"); } }
        public static ScannerFile HASH077E335F { get { return new ScannerFile("01_specific_location\\0x077E335F.wav", "0x077E335F", "specific_location"); } }
        public static ScannerFile HASH078D31E5 { get { return new ScannerFile("01_specific_location\\0x078D31E5.wav", "0x078D31E5", "specific_location"); } }
        public static ScannerFile HASH07910E35 { get { return new ScannerFile("01_specific_location\\0x07910E35.wav", "0x07910E35", "specific_location"); } }
        public static ScannerFile HASH07DD9E5C { get { return new ScannerFile("01_specific_location\\0x07DD9E5C.wav", "0x07DD9E5C", "specific_location"); } }
        public static ScannerFile HASH07FB062C { get { return new ScannerFile("01_specific_location\\0x07FB062C.wav", "0x07FB062C", "specific_location"); } }
        public static ScannerFile HASH081BD374 { get { return new ScannerFile("01_specific_location\\0x081BD374.wav", "0x081BD374", "specific_location"); } }
        public static ScannerFile HASH0868B699 { get { return new ScannerFile("01_specific_location\\0x0868B699.wav", "0x0868B699", "specific_location"); } }
        public static ScannerFile HASH08AA4C64 { get { return new ScannerFile("01_specific_location\\0x08AA4C64.wav", "0x08AA4C64", "specific_location"); } }
        public static ScannerFile HASH08D9D223 { get { return new ScannerFile("01_specific_location\\0x08D9D223.wav", "0x08D9D223", "specific_location"); } }
        public static ScannerFile HASH08E0DB6C { get { return new ScannerFile("01_specific_location\\0x08E0DB6C.wav", "0x08E0DB6C", "specific_location"); } }
        public static ScannerFile HASH096C35C1 { get { return new ScannerFile("01_specific_location\\0x096C35C1.wav", "0x096C35C1", "specific_location"); } }
        public static ScannerFile HASH096CA882 { get { return new ScannerFile("01_specific_location\\0x096CA882.wav", "0x096CA882", "specific_location"); } }
        public static ScannerFile HASH09A9666F { get { return new ScannerFile("01_specific_location\\0x09A9666F.wav", "0x09A9666F", "specific_location"); } }
        public static ScannerFile HASH09E1E461 { get { return new ScannerFile("01_specific_location\\0x09E1E461.wav", "0x09E1E461", "specific_location"); } }
        public static ScannerFile HASH09E4AAC8 { get { return new ScannerFile("01_specific_location\\0x09E4AAC8.wav", "0x09E4AAC8", "specific_location"); } }
        public static ScannerFile HASH09E521B9 { get { return new ScannerFile("01_specific_location\\0x09E521B9.wav", "0x09E521B9", "specific_location"); } }
        public static ScannerFile HASH09E57626 { get { return new ScannerFile("01_specific_location\\0x09E57626.wav", "0x09E57626", "specific_location"); } }
        public static ScannerFile HASH09F6C27C { get { return new ScannerFile("01_specific_location\\0x09F6C27C.wav", "0x09F6C27C", "specific_location"); } }
        public static ScannerFile HASH09FE57DE { get { return new ScannerFile("01_specific_location\\0x09FE57DE.wav", "0x09FE57DE", "specific_location"); } }
        public static ScannerFile HASH0A158A5B { get { return new ScannerFile("01_specific_location\\0x0A158A5B.wav", "0x0A158A5B", "specific_location"); } }
        public static ScannerFile HASH0A281CB4 { get { return new ScannerFile("01_specific_location\\0x0A281CB4.wav", "0x0A281CB4", "specific_location"); } }
        public static ScannerFile HASH0A33D1E1 { get { return new ScannerFile("01_specific_location\\0x0A33D1E1.wav", "0x0A33D1E1", "specific_location"); } }
        public static ScannerFile HASH0A4084AF { get { return new ScannerFile("01_specific_location\\0x0A4084AF.wav", "0x0A4084AF", "specific_location"); } }
        public static ScannerFile HASH0A45FA8A { get { return new ScannerFile("01_specific_location\\0x0A45FA8A.wav", "0x0A45FA8A", "specific_location"); } }
        public static ScannerFile HASH0A70FEFC { get { return new ScannerFile("01_specific_location\\0x0A70FEFC.wav", "0x0A70FEFC", "specific_location"); } }
        public static ScannerFile HASH0A98BB4F { get { return new ScannerFile("01_specific_location\\0x0A98BB4F.wav", "0x0A98BB4F", "specific_location"); } }
        public static ScannerFile HASH0AC416A0 { get { return new ScannerFile("01_specific_location\\0x0AC416A0.wav", "0x0AC416A0", "specific_location"); } }
        public static ScannerFile HASH0B421414 { get { return new ScannerFile("01_specific_location\\0x0B421414.wav", "0x0B421414", "specific_location"); } }
        public static ScannerFile HASH0B4EB13E { get { return new ScannerFile("01_specific_location\\0x0B4EB13E.wav", "0x0B4EB13E", "specific_location"); } }
        public static ScannerFile HASH0BC6790E { get { return new ScannerFile("01_specific_location\\0x0BC6790E.wav", "0x0BC6790E", "specific_location"); } }
        public static ScannerFile HASH0BCCFE7B { get { return new ScannerFile("01_specific_location\\0x0BCCFE7B.wav", "0x0BCCFE7B", "specific_location"); } }
        public static ScannerFile HASH0C57ACE0 { get { return new ScannerFile("01_specific_location\\0x0C57ACE0.wav", "0x0C57ACE0", "specific_location"); } }
        public static ScannerFile HASH0C5A4ECA { get { return new ScannerFile("01_specific_location\\0x0C5A4ECA.wav", "0x0C5A4ECA", "specific_location"); } }
        public static ScannerFile HASH0CC13082 { get { return new ScannerFile("01_specific_location\\0x0CC13082.wav", "0x0CC13082", "specific_location"); } }
        public static ScannerFile HASH0CC361AF { get { return new ScannerFile("01_specific_location\\0x0CC361AF.wav", "0x0CC361AF", "specific_location"); } }
        public static ScannerFile HASH0CCADD03 { get { return new ScannerFile("01_specific_location\\0x0CCADD03.wav", "0x0CCADD03", "specific_location"); } }
        public static ScannerFile HASH0D1B649D { get { return new ScannerFile("01_specific_location\\0x0D1B649D.wav", "0x0D1B649D", "specific_location"); } }
        public static ScannerFile HASH0D40D5A4 { get { return new ScannerFile("01_specific_location\\0x0D40D5A4.wav", "0x0D40D5A4", "specific_location"); } }
        public static ScannerFile HASH0D6F777B { get { return new ScannerFile("01_specific_location\\0x0D6F777B.wav", "0x0D6F777B", "specific_location"); } }
        public static ScannerFile HASH0D8D06A1 { get { return new ScannerFile("01_specific_location\\0x0D8D06A1.wav", "0x0D8D06A1", "specific_location"); } }
        public static ScannerFile HASH0D956322 { get { return new ScannerFile("01_specific_location\\0x0D956322.wav", "0x0D956322", "specific_location"); } }
        public static ScannerFile HASH0D99B50F { get { return new ScannerFile("01_specific_location\\0x0D99B50F.wav", "0x0D99B50F", "specific_location"); } }
        public static ScannerFile HASH0DA81BFE { get { return new ScannerFile("01_specific_location\\0x0DA81BFE.wav", "0x0DA81BFE", "specific_location"); } }
        public static ScannerFile HASH0DAF6020 { get { return new ScannerFile("01_specific_location\\0x0DAF6020.wav", "0x0DAF6020", "specific_location"); } }
        public static ScannerFile HASH0DB18443 { get { return new ScannerFile("01_specific_location\\0x0DB18443.wav", "0x0DB18443", "specific_location"); } }
        public static ScannerFile HASH0DE50E66 { get { return new ScannerFile("01_specific_location\\0x0DE50E66.wav", "0x0DE50E66", "specific_location"); } }
        public static ScannerFile HASH0E1F32BB { get { return new ScannerFile("01_specific_location\\0x0E1F32BB.wav", "0x0E1F32BB", "specific_location"); } }
        public static ScannerFile HASH0E94FE38 { get { return new ScannerFile("01_specific_location\\0x0E94FE38.wav", "0x0E94FE38", "specific_location"); } }
        public static ScannerFile HASH0EE35330 { get { return new ScannerFile("01_specific_location\\0x0EE35330.wav", "0x0EE35330", "specific_location"); } }
        public static ScannerFile HASH0F7D8050 { get { return new ScannerFile("01_specific_location\\0x0F7D8050.wav", "0x0F7D8050", "specific_location"); } }
        public static ScannerFile HASH105B95C3 { get { return new ScannerFile("01_specific_location\\0x105B95C3.wav", "0x105B95C3", "specific_location"); } }
        public static ScannerFile HASH10E16467 { get { return new ScannerFile("01_specific_location\\0x10E16467.wav", "0x10E16467", "specific_location"); } }
        public static ScannerFile HASH111BE082 { get { return new ScannerFile("01_specific_location\\0x111BE082.wav", "0x111BE082", "specific_location"); } }
        public static ScannerFile HASH111BF88B { get { return new ScannerFile("01_specific_location\\0x111BF88B.wav", "0x111BF88B", "specific_location"); } }
        public static ScannerFile HASH11358D45 { get { return new ScannerFile("01_specific_location\\0x11358D45.wav", "0x11358D45", "specific_location"); } }
        public static ScannerFile HASH1139507A { get { return new ScannerFile("01_specific_location\\0x1139507A.wav", "0x1139507A", "specific_location"); } }
        public static ScannerFile HASH1190C8A4 { get { return new ScannerFile("01_specific_location\\0x1190C8A4.wav", "0x1190C8A4", "specific_location"); } }
        public static ScannerFile HASH11CCE448 { get { return new ScannerFile("01_specific_location\\0x11CCE448.wav", "0x11CCE448", "specific_location"); } }
        public static ScannerFile HASH11F08287 { get { return new ScannerFile("01_specific_location\\0x11F08287.wav", "0x11F08287", "specific_location"); } }
        public static ScannerFile HASH120B0B74 { get { return new ScannerFile("01_specific_location\\0x120B0B74.wav", "0x120B0B74", "specific_location"); } }
        public static ScannerFile HASH1223E5D3 { get { return new ScannerFile("01_specific_location\\0x1223E5D3.wav", "0x1223E5D3", "specific_location"); } }
        public static ScannerFile HASH122B5EFF { get { return new ScannerFile("01_specific_location\\0x122B5EFF.wav", "0x122B5EFF", "specific_location"); } }
        public static ScannerFile HASH124A359B { get { return new ScannerFile("01_specific_location\\0x124A359B.wav", "0x124A359B", "specific_location"); } }
        public static ScannerFile HASH126496AC { get { return new ScannerFile("01_specific_location\\0x126496AC.wav", "0x126496AC", "specific_location"); } }
        public static ScannerFile HASH128B95B2 { get { return new ScannerFile("01_specific_location\\0x128B95B2.wav", "0x128B95B2", "specific_location"); } }
        public static ScannerFile HASH12C1F74A { get { return new ScannerFile("01_specific_location\\0x12C1F74A.wav", "0x12C1F74A", "specific_location"); } }
        public static ScannerFile HASH1337CFB1 { get { return new ScannerFile("01_specific_location\\0x1337CFB1.wav", "0x1337CFB1", "specific_location"); } }
        public static ScannerFile HASH13583D6F { get { return new ScannerFile("01_specific_location\\0x13583D6F.wav", "0x13583D6F", "specific_location"); } }
        public static ScannerFile HASH13A5966F { get { return new ScannerFile("01_specific_location\\0x13A5966F.wav", "0x13A5966F", "specific_location"); } }
        public static ScannerFile HASH13B9D9F7 { get { return new ScannerFile("01_specific_location\\0x13B9D9F7.wav", "0x13B9D9F7", "specific_location"); } }
        public static ScannerFile HASH13CBAB64 { get { return new ScannerFile("01_specific_location\\0x13CBAB64.wav", "0x13CBAB64", "specific_location"); } }
        public static ScannerFile HASH141154C0 { get { return new ScannerFile("01_specific_location\\0x141154C0.wav", "0x141154C0", "specific_location"); } }
        public static ScannerFile HASH143EDDDF { get { return new ScannerFile("01_specific_location\\0x143EDDDF.wav", "0x143EDDDF", "specific_location"); } }
        public static ScannerFile HASH1457D3BB { get { return new ScannerFile("01_specific_location\\0x1457D3BB.wav", "0x1457D3BB", "specific_location"); } }
        public static ScannerFile HASH147855FA { get { return new ScannerFile("01_specific_location\\0x147855FA.wav", "0x147855FA", "specific_location"); } }
        public static ScannerFile HASH14A1C730 { get { return new ScannerFile("01_specific_location\\0x14A1C730.wav", "0x14A1C730", "specific_location"); } }
        public static ScannerFile HASH14B8A4DB { get { return new ScannerFile("01_specific_location\\0x14B8A4DB.wav", "0x14B8A4DB", "specific_location"); } }
        public static ScannerFile HASH14C89994 { get { return new ScannerFile("01_specific_location\\0x14C89994.wav", "0x14C89994", "specific_location"); } }
        public static ScannerFile HASH14C9D45F { get { return new ScannerFile("01_specific_location\\0x14C9D45F.wav", "0x14C9D45F", "specific_location"); } }
        public static ScannerFile HASH14F358CA { get { return new ScannerFile("01_specific_location\\0x14F358CA.wav", "0x14F358CA", "specific_location"); } }
        public static ScannerFile HASH150D4A67 { get { return new ScannerFile("01_specific_location\\0x150D4A67.wav", "0x150D4A67", "specific_location"); } }
        public static ScannerFile HASH15151AB5 { get { return new ScannerFile("01_specific_location\\0x15151AB5.wav", "0x15151AB5", "specific_location"); } }
        public static ScannerFile HASH1549EDC8 { get { return new ScannerFile("01_specific_location\\0x1549EDC8.wav", "0x1549EDC8", "specific_location"); } }
        public static ScannerFile HASH159C20B3 { get { return new ScannerFile("01_specific_location\\0x159C20B3.wav", "0x159C20B3", "specific_location"); } }
        public static ScannerFile HASH159DC557 { get { return new ScannerFile("01_specific_location\\0x159DC557.wav", "0x159DC557", "specific_location"); } }
        public static ScannerFile HASH15AD6A1C { get { return new ScannerFile("01_specific_location\\0x15AD6A1C.wav", "0x15AD6A1C", "specific_location"); } }
        public static ScannerFile HASH15B15683 { get { return new ScannerFile("01_specific_location\\0x15B15683.wav", "0x15B15683", "specific_location"); } }
        public static ScannerFile HASH15D35E76 { get { return new ScannerFile("01_specific_location\\0x15D35E76.wav", "0x15D35E76", "specific_location"); } }
        public static ScannerFile HASH15D4147E { get { return new ScannerFile("01_specific_location\\0x15D4147E.wav", "0x15D4147E", "specific_location"); } }
        public static ScannerFile HASH1639890D { get { return new ScannerFile("01_specific_location\\0x1639890D.wav", "0x1639890D", "specific_location"); } }
        public static ScannerFile HASH165792C7 { get { return new ScannerFile("01_specific_location\\0x165792C7.wav", "0x165792C7", "specific_location"); } }
        public static ScannerFile HASH16677E71 { get { return new ScannerFile("01_specific_location\\0x16677E71.wav", "0x16677E71", "specific_location"); } }
        public static ScannerFile HASH1667D63F { get { return new ScannerFile("01_specific_location\\0x1667D63F.wav", "0x1667D63F", "specific_location"); } }
        public static ScannerFile HASH166818B8 { get { return new ScannerFile("01_specific_location\\0x166818B8.wav", "0x166818B8", "specific_location"); } }
        public static ScannerFile HASH1677E89E { get { return new ScannerFile("01_specific_location\\0x1677E89E.wav", "0x1677E89E", "specific_location"); } }
        public static ScannerFile HASH168085D1 { get { return new ScannerFile("01_specific_location\\0x168085D1.wav", "0x168085D1", "specific_location"); } }
        public static ScannerFile HASH16AAEFAC { get { return new ScannerFile("01_specific_location\\0x16AAEFAC.wav", "0x16AAEFAC", "specific_location"); } }
        public static ScannerFile HASH16C620C3 { get { return new ScannerFile("01_specific_location\\0x16C620C3.wav", "0x16C620C3", "specific_location"); } }
        public static ScannerFile HASH16F87D3A { get { return new ScannerFile("01_specific_location\\0x16F87D3A.wav", "0x16F87D3A", "specific_location"); } }
        public static ScannerFile HASH17AE5C6D { get { return new ScannerFile("01_specific_location\\0x17AE5C6D.wav", "0x17AE5C6D", "specific_location"); } }
        public static ScannerFile HASH17E65770 { get { return new ScannerFile("01_specific_location\\0x17E65770.wav", "0x17E65770", "specific_location"); } }
        public static ScannerFile HASH18508E93 { get { return new ScannerFile("01_specific_location\\0x18508E93.wav", "0x18508E93", "specific_location"); } }
        public static ScannerFile HASH1878923B { get { return new ScannerFile("01_specific_location\\0x1878923B.wav", "0x1878923B", "specific_location"); } }
        public static ScannerFile HASH189ED18A { get { return new ScannerFile("01_specific_location\\0x189ED18A.wav", "0x189ED18A", "specific_location"); } }
        public static ScannerFile HASH18C4124F { get { return new ScannerFile("01_specific_location\\0x18C4124F.wav", "0x18C4124F", "specific_location"); } }
        public static ScannerFile HASH18C6F152 { get { return new ScannerFile("01_specific_location\\0x18C6F152.wav", "0x18C6F152", "specific_location"); } }
        public static ScannerFile HASH18EA44DE { get { return new ScannerFile("01_specific_location\\0x18EA44DE.wav", "0x18EA44DE", "specific_location"); } }
        public static ScannerFile HASH1907E32D { get { return new ScannerFile("01_specific_location\\0x1907E32D.wav", "0x1907E32D", "specific_location"); } }
        public static ScannerFile HASH19532EA2 { get { return new ScannerFile("01_specific_location\\0x19532EA2.wav", "0x19532EA2", "specific_location"); } }
        public static ScannerFile HASH19797B25 { get { return new ScannerFile("01_specific_location\\0x19797B25.wav", "0x19797B25", "specific_location"); } }
        public static ScannerFile HASH197FDA82 { get { return new ScannerFile("01_specific_location\\0x197FDA82.wav", "0x197FDA82", "specific_location"); } }
        public static ScannerFile HASH1980DD57 { get { return new ScannerFile("01_specific_location\\0x1980DD57.wav", "0x1980DD57", "specific_location"); } }
        public static ScannerFile HASH1983B0D4 { get { return new ScannerFile("01_specific_location\\0x1983B0D4.wav", "0x1983B0D4", "specific_location"); } }
        public static ScannerFile HASH19924D41 { get { return new ScannerFile("01_specific_location\\0x19924D41.wav", "0x19924D41", "specific_location"); } }
        public static ScannerFile HASH19E069DE { get { return new ScannerFile("01_specific_location\\0x19E069DE.wav", "0x19E069DE", "specific_location"); } }
        public static ScannerFile HASH19E4D5BD { get { return new ScannerFile("01_specific_location\\0x19E4D5BD.wav", "0x19E4D5BD", "specific_location"); } }
        public static ScannerFile HASH1A23351D { get { return new ScannerFile("01_specific_location\\0x1A23351D.wav", "0x1A23351D", "specific_location"); } }
        public static ScannerFile HASH1A2E04DF { get { return new ScannerFile("01_specific_location\\0x1A2E04DF.wav", "0x1A2E04DF", "specific_location"); } }
        public static ScannerFile HASH1A3F7860 { get { return new ScannerFile("01_specific_location\\0x1A3F7860.wav", "0x1A3F7860", "specific_location"); } }
        public static ScannerFile HASH1A67316D { get { return new ScannerFile("01_specific_location\\0x1A67316D.wav", "0x1A67316D", "specific_location"); } }
        public static ScannerFile HASH1A91C6DE { get { return new ScannerFile("01_specific_location\\0x1A91C6DE.wav", "0x1A91C6DE", "specific_location"); } }
        public static ScannerFile HASH1A94B384 { get { return new ScannerFile("01_specific_location\\0x1A94B384.wav", "0x1A94B384", "specific_location"); } }
        public static ScannerFile HASH1AB17532 { get { return new ScannerFile("01_specific_location\\0x1AB17532.wav", "0x1AB17532", "specific_location"); } }
        public static ScannerFile HASH1AB60F0D { get { return new ScannerFile("01_specific_location\\0x1AB60F0D.wav", "0x1AB60F0D", "specific_location"); } }
        public static ScannerFile HASH1ABB2DE0 { get { return new ScannerFile("01_specific_location\\0x1ABB2DE0.wav", "0x1ABB2DE0", "specific_location"); } }
        public static ScannerFile HASH1AF37109 { get { return new ScannerFile("01_specific_location\\0x1AF37109.wav", "0x1AF37109", "specific_location"); } }
        public static ScannerFile HASH1AFC8E72 { get { return new ScannerFile("01_specific_location\\0x1AFC8E72.wav", "0x1AFC8E72", "specific_location"); } }
        public static ScannerFile HASH1B133696 { get { return new ScannerFile("01_specific_location\\0x1B133696.wav", "0x1B133696", "specific_location"); } }
        public static ScannerFile HASH1B1BCF62 { get { return new ScannerFile("01_specific_location\\0x1B1BCF62.wav", "0x1B1BCF62", "specific_location"); } }
        public static ScannerFile HASH1B2852E9 { get { return new ScannerFile("01_specific_location\\0x1B2852E9.wav", "0x1B2852E9", "specific_location"); } }
        public static ScannerFile HASH1B3FE498 { get { return new ScannerFile("01_specific_location\\0x1B3FE498.wav", "0x1B3FE498", "specific_location"); } }
        public static ScannerFile HASH1B7B1F49 { get { return new ScannerFile("01_specific_location\\0x1B7B1F49.wav", "0x1B7B1F49", "specific_location"); } }
        public static ScannerFile HASH1B81EF89 { get { return new ScannerFile("01_specific_location\\0x1B81EF89.wav", "0x1B81EF89", "specific_location"); } }
        public static ScannerFile HASH1BB48F43 { get { return new ScannerFile("01_specific_location\\0x1BB48F43.wav", "0x1BB48F43", "specific_location"); } }
        public static ScannerFile HASH1BC1EA80 { get { return new ScannerFile("01_specific_location\\0x1BC1EA80.wav", "0x1BC1EA80", "specific_location"); } }
        public static ScannerFile HASH1C5D1678 { get { return new ScannerFile("01_specific_location\\0x1C5D1678.wav", "0x1C5D1678", "specific_location"); } }
        public static ScannerFile HASH1C674794 { get { return new ScannerFile("01_specific_location\\0x1C674794.wav", "0x1C674794", "specific_location"); } }
        public static ScannerFile HASH1CD25A01 { get { return new ScannerFile("01_specific_location\\0x1CD25A01.wav", "0x1CD25A01", "specific_location"); } }
        public static ScannerFile HASH1D2407D2 { get { return new ScannerFile("01_specific_location\\0x1D2407D2.wav", "0x1D2407D2", "specific_location"); } }
        public static ScannerFile HASH1D532F53 { get { return new ScannerFile("01_specific_location\\0x1D532F53.wav", "0x1D532F53", "specific_location"); } }
        public static ScannerFile HASH1D6A83F9 { get { return new ScannerFile("01_specific_location\\0x1D6A83F9.wav", "0x1D6A83F9", "specific_location"); } }
        public static ScannerFile HASH1DA6BA84 { get { return new ScannerFile("01_specific_location\\0x1DA6BA84.wav", "0x1DA6BA84", "specific_location"); } }
        public static ScannerFile HASH1DB64CA3 { get { return new ScannerFile("01_specific_location\\0x1DB64CA3.wav", "0x1DB64CA3", "specific_location"); } }
        public static ScannerFile HASH1DE8D28D { get { return new ScannerFile("01_specific_location\\0x1DE8D28D.wav", "0x1DE8D28D", "specific_location"); } }
        public static ScannerFile HASH1DEC0BDE { get { return new ScannerFile("01_specific_location\\0x1DEC0BDE.wav", "0x1DEC0BDE", "specific_location"); } }
        public static ScannerFile HASH1DF81DFF { get { return new ScannerFile("01_specific_location\\0x1DF81DFF.wav", "0x1DF81DFF", "specific_location"); } }
        public static ScannerFile HASH1E2AE79B { get { return new ScannerFile("01_specific_location\\0x1E2AE79B.wav", "0x1E2AE79B", "specific_location"); } }
        public static ScannerFile HASH1E74A249 { get { return new ScannerFile("01_specific_location\\0x1E74A249.wav", "0x1E74A249", "specific_location"); } }
        public static ScannerFile HASH1E998193 { get { return new ScannerFile("01_specific_location\\0x1E998193.wav", "0x1E998193", "specific_location"); } }
        public static ScannerFile HASH1F07D876 { get { return new ScannerFile("01_specific_location\\0x1F07D876.wav", "0x1F07D876", "specific_location"); } }
        public static ScannerFile HASH1F0E27B7 { get { return new ScannerFile("01_specific_location\\0x1F0E27B7.wav", "0x1F0E27B7", "specific_location"); } }
        public static ScannerFile HASH1FB449F8 { get { return new ScannerFile("01_specific_location\\0x1FB449F8.wav", "0x1FB449F8", "specific_location"); } }
    }
    public class stand_down
    {
        public static ScannerFile Code4 { get { return new ScannerFile("01_stand_down\\0x05C0A263.wav", "0x05C0A263", "stand_down"); } }
        public static ScannerFile StandDownReturnToPatrol { get { return new ScannerFile("01_stand_down\\0x0A396B54.wav", "0x0A396B54", "stand_down"); } }
        public static ScannerFile AllUnitsCode4 { get { return new ScannerFile("01_stand_down\\0x1120B929.wav", "0x1120B929", "stand_down"); } }
        public static ScannerFile AllUnitsStandDown { get { return new ScannerFile("01_stand_down\\0x187347C8.wav", "0x187347C8", "stand_down"); } }
        public static ScannerFile ReturnToPatrol { get { return new ScannerFile("01_stand_down\\0x18CE0883.wav", "0x18CE0883", "stand_down"); } }
        public static ScannerFile ReturnToPatrol1 { get { return new ScannerFile("01_stand_down\\0x195C899A.wav", "0x195C899A", "stand_down"); } }
        public static ScannerFile ReturnToPatrol2 { get { return new ScannerFile("01_stand_down\\0x1F379557.wav", "0x1F379557", "stand_down"); } }
    }
    public class status_message
    {
        public static ScannerFile HASH01B72847 { get { return new ScannerFile("01_status_message\\0x01B72847.wav", "0x01B72847", "status_message"); } }
        public static ScannerFile CarryingSmallArms { get { return new ScannerFile("01_status_message\\0x01E7B2E2.wav", "Carrying small arms", "status_message"); } }
        public static ScannerFile HASH02E79882 { get { return new ScannerFile("01_status_message\\0x02E79882.wav", "0x02E79882", "status_message"); } }
        public static ScannerFile HASH04904F02 { get { return new ScannerFile("01_status_message\\0x04904F02.wav", "0x04904F02", "status_message"); } }
        public static ScannerFile HASH04F26EBD { get { return new ScannerFile("01_status_message\\0x04F26EBD.wav", "0x04F26EBD", "status_message"); } }
        public static ScannerFile HASH0717B309 { get { return new ScannerFile("01_status_message\\0x0717B309.wav", "0x0717B309", "status_message"); } }
        public static ScannerFile ArmedWithExplosives { get { return new ScannerFile("01_status_message\\0x0A14A6DC.wav", "Armed with Explosives", "status_message"); } }
        public static ScannerFile ExtremelyAgitated { get { return new ScannerFile("01_status_message\\0x0A3658CA.wav", "Extremely Agitated", "status_message"); } }
        public static ScannerFile FleeingTheSceneOfTheCrime { get { return new ScannerFile("01_status_message\\0x0B291B46.wav", "Fleeing the scene of the crime", "status_message"); } }
        public static ScannerFile ApparentlyWounded { get { return new ScannerFile("01_status_message\\0x0C00BCDC.wav", "Apparently Wounded", "status_message"); } }
        public static ScannerFile HASH0C5A6D4D { get { return new ScannerFile("01_status_message\\0x0C5A6D4D.wav", "0x0C5A6D4D", "status_message"); } }
        public static ScannerFile HASH0DDD2FA0 { get { return new ScannerFile("01_status_message\\0x0DDD2FA0.wav", "0x0DDD2FA0", "status_message"); } }
        public static ScannerFile HeavilyArmedAndDangerous { get { return new ScannerFile("01_status_message\\0x0E412264.wav", "Heavily armed and dangerous", "status_message"); } }
        public static ScannerFile HASH0FAD644B { get { return new ScannerFile("01_status_message\\0x0FAD644B.wav", "0x0FAD644B", "status_message"); } }
        public static ScannerFile HASH11C92887 { get { return new ScannerFile("01_status_message\\0x11C92887.wav", "0x11C92887", "status_message"); } }
        public static ScannerFile HASH11CEF782 { get { return new ScannerFile("01_status_message\\0x11CEF782.wav", "0x11CEF782", "status_message"); } }
        public static ScannerFile HASH12EEEBBF { get { return new ScannerFile("01_status_message\\0x12EEEBBF.wav", "0x12EEEBBF", "status_message"); } }
        public static ScannerFile HASH1325155D { get { return new ScannerFile("01_status_message\\0x1325155D.wav", "0x1325155D", "status_message"); } }
        public static ScannerFile ArmedWithAssaultWeapon { get { return new ScannerFile("01_status_message\\0x13F6EE1F.wav", "Armed with assault weapon", "status_message"); } }
        public static ScannerFile HASH14078CE8 { get { return new ScannerFile("01_status_message\\0x14078CE8.wav", "0x14078CE8", "status_message"); } }
        public static ScannerFile HASH16997FE5 { get { return new ScannerFile("01_status_message\\0x16997FE5.wav", "0x16997FE5", "status_message"); } }
        public static ScannerFile HASH16BAF1D4 { get { return new ScannerFile("01_status_message\\0x16BAF1D4.wav", "0x16BAF1D4", "status_message"); } }
        public static ScannerFile HASH1704B3DA { get { return new ScannerFile("01_status_message\\0x1704B3DA.wav", "0x1704B3DA", "status_message"); } }
        public static ScannerFile HeavilyArmed { get { return new ScannerFile("01_status_message\\0x1A13548C.wav", "Heavily armed", "status_message"); } }
        public static ScannerFile HASH1AA2C9DE { get { return new ScannerFile("01_status_message\\0x1AA2C9DE.wav", "0x1AA2C9DE", "status_message"); } }
        public static ScannerFile HASH1ABEDA57 { get { return new ScannerFile("01_status_message\\0x1ABEDA57.wav", "0x1ABEDA57", "status_message"); } }
        public static ScannerFile HASH1BF83C4F { get { return new ScannerFile("01_status_message\\0x1BF83C4F.wav", "0x1BF83C4F", "status_message"); } }
        public static ScannerFile ArmedWithRocketLauncher { get { return new ScannerFile("01_status_message\\0x1D60077B.wav", "Armed with rocket launcher", "status_message"); } }
        public static ScannerFile HASH1D77FFE3 { get { return new ScannerFile("01_status_message\\0x1D77FFE3.wav", "0x1D77FFE3", "status_message"); } }
        public static ScannerFile HASH1DDE80AE { get { return new ScannerFile("01_status_message\\0x1DDE80AE.wav", "0x1DDE80AE", "status_message"); } }
        public static ScannerFile ArmedAndDangerous { get { return new ScannerFile("01_status_message\\0x1E0B81F9.wav", "Armed and dangerous", "status_message"); } }
        public static ScannerFile HASH1FFCD3D4 { get { return new ScannerFile("01_status_message\\0x1FFCD3D4.wav", "0x1FFCD3D4", "status_message"); } }
    }
    public class streets
    {
        public static ScannerFile LindenDrive { get { return new ScannerFile("01_streets\\0x0001B096.wav", "0x0001B096", "streets"); } }
        public static ScannerFile LakeVinewoodEstate { get { return new ScannerFile("01_streets\\0x0049AED0.wav", "0x0049AED0", "streets"); } }
        public static ScannerFile SeaviewRd { get { return new ScannerFile("01_streets\\0x00BFEAA6.wav", "SeaviewRd", "streets"); } }
        public static ScannerFile IntergrityWy { get { return new ScannerFile("01_streets\\0x00C4CB28.wav", "IntergrityWy", "streets"); } }
        public static ScannerFile HawickAve { get { return new ScannerFile("01_streets\\0x00D1F515.wav", "HawickAve", "streets"); } }
        public static ScannerFile LakeVineWoodDrive { get { return new ScannerFile("01_streets\\0x00EB9A86.wav", "LakeVineWoodDrive", "streets"); } }
        public static ScannerFile DorsetPlace { get { return new ScannerFile("01_streets\\0x0102C67D.wav", "0x0102C67D", "streets"); } }
        public static ScannerFile HollywoodBlvd { get { return new ScannerFile("01_streets\\0x0103ABE0.wav", "0x0103ABE0", "streets"); } }
        public static ScannerFile MorningwoodBlvd { get { return new ScannerFile("01_streets\\0x010F58DC.wav", "0x010F58DC", "streets"); } }
        public static ScannerFile EastGalileoAve { get { return new ScannerFile("01_streets\\0x0123B12E.wav", "0x0123B12E", "streets"); } }
        public static ScannerFile BlaverPlace { get { return new ScannerFile("01_streets\\0x0136F8BE.wav", "0x0136F8BE", "streets"); } }
        public static ScannerFile NorthElRanchoBlvd { get { return new ScannerFile("01_streets\\0x01406583.wav", "0x01406583", "streets"); } }
        public static ScannerFile EastMirrorDrive { get { return new ScannerFile("01_streets\\0x0149BC83.wav", "0x0149BC83", "streets"); } }
        public static ScannerFile NorthBlvdDelPerro { get { return new ScannerFile("01_streets\\0x016ACF1D.wav", "0x016ACF1D", "streets"); } }
        public static ScannerFile LittleBighornAve { get { return new ScannerFile("01_streets\\0x016E5EB8.wav", "0x016E5EB8", "streets"); } }
        public static ScannerFile SouthSeasideAve { get { return new ScannerFile("01_streets\\0x019EDBDE.wav", "0x019EDBDE", "streets"); } }
        public static ScannerFile ElGouhaStreet { get { return new ScannerFile("01_streets\\0x01B1ECA2.wav", "0x01B1ECA2", "streets"); } }
        public static ScannerFile RichmondStreet { get { return new ScannerFile("01_streets\\0x01C6754A.wav", "0x01C6754A", "streets"); } }
        public static ScannerFile InventionCourt { get { return new ScannerFile("01_streets\\0x01C7D19F.wav", "0x01C7D19F", "streets"); } }
        public static ScannerFile MarathonAve { get { return new ScannerFile("01_streets\\0x020EADF0.wav", "0x020EADF0", "streets"); } }
        public static ScannerFile ConquistadorBlvd { get { return new ScannerFile("01_streets\\0x0214731C.wav", "0x0214731C", "streets"); } }
        public static ScannerFile MiriamTurnerOverpass { get { return new ScannerFile("01_streets\\0x021A099C.wav", "0x021A099C", "streets"); } }
        public static ScannerFile Route68 { get { return new ScannerFile("01_streets\\0x021CAA3D.wav", "0x021CAA3D", "streets"); } }
        public static ScannerFile PlayaVista { get { return new ScannerFile("01_streets\\0x022520F8.wav", "0x022520F8", "streets"); } }
        public static ScannerFile LowPowerStreet { get { return new ScannerFile("01_streets\\0x0240EC46.wav", "0x0240EC46", "streets"); } }
        public static ScannerFile SelmaAve { get { return new ScannerFile("01_streets\\0x026AE70A.wav", "0x026AE70A", "streets"); } }
        public static ScannerFile DelPierroFreeway { get { return new ScannerFile("01_streets\\0x0284E3F3.wav", "0x0284E3F3", "streets"); } }
        public static ScannerFile ProperityStreetPromenade { get { return new ScannerFile("01_streets\\0x0285B168.wav", "0x0285B168", "streets"); } }
        public static ScannerFile PowerStreet { get { return new ScannerFile("01_streets\\0x0299BFF3.wav", "0x0299BFF3", "streets"); } }
        public static ScannerFile CanneryStreet { get { return new ScannerFile("01_streets\\0x02E19C74.wav", "0x02E19C74", "streets"); } }
        public static ScannerFile NorthSycamoreAve { get { return new ScannerFile("01_streets\\0x02E1EA9E.wav", "0x02E1EA9E", "streets"); } }
        public static ScannerFile SouthShambleStreet { get { return new ScannerFile("01_streets\\0x033297CE.wav", "0x033297CE", "streets"); } }
        public static ScannerFile MutineeRoad { get { return new ScannerFile("01_streets\\0x033FD7E7.wav", "0x033FD7E7", "streets"); } }
        public static ScannerFile MirangeLane { get { return new ScannerFile("01_streets\\0x034E8F44.wav", "0x034E8F44", "streets"); } }
        public static ScannerFile StrawberryAve { get { return new ScannerFile("01_streets\\0x03582630.wav", "0x03582630", "streets"); } }
        public static ScannerFile OrchidvilleAve { get { return new ScannerFile("01_streets\\0x037B7DEE.wav", "0x037B7DEE", "streets"); } }
        public static ScannerFile ZancudoBaranca { get { return new ScannerFile("01_streets\\0x03C08D92.wav", "0x03C08D92", "streets"); } }
        public static ScannerFile HeritageWay { get { return new ScannerFile("01_streets\\0x03C75F8B.wav", "0x03C75F8B", "streets"); } }
        public static ScannerFile BergAve { get { return new ScannerFile("01_streets\\0x03CE8FB7.wav", "0x03CE8FB7", "streets"); } }
        public static ScannerFile BacklotBlvd { get { return new ScannerFile("01_streets\\0x03CF126F.wav", "0x03CF126F", "streets"); } }
        public static ScannerFile JilledLane { get { return new ScannerFile("01_streets\\0x03DE370F.wav", "0x03DE370F", "streets"); } }
        public static ScannerFile NormandyDrive { get { return new ScannerFile("01_streets\\0x03E746F6.wav", "0x03E746F6", "streets"); } }
        public static ScannerFile DorsetDrive { get { return new ScannerFile("01_streets\\0x03E8C91A.wav", "0x03E8C91A", "streets"); } }
        public static ScannerFile MiltonRoad { get { return new ScannerFile("01_streets\\0x043E9B50.wav", "0x043E9B50", "streets"); } }
        public static ScannerFile EnecinoRoad { get { return new ScannerFile("01_streets\\0x04421BC2.wav", "0x04421BC2", "streets"); } }
        public static ScannerFile PackersAve { get { return new ScannerFile("01_streets\\0x04BBC5F4.wav", "0x04BBC5F4", "streets"); } }
        public static ScannerFile CoxWay { get { return new ScannerFile("01_streets\\0x04F36858.wav", "0x04F36858", "streets"); } }
        public static ScannerFile StrangeWaysDrive { get { return new ScannerFile("01_streets\\0x04F7F00A.wav", "0x04F7F00A", "streets"); } }
        public static ScannerFile AlterStreet { get { return new ScannerFile("01_streets\\0x05000FA8.wav", "0x05000FA8", "streets"); } }
        public static ScannerFile OccupationAve { get { return new ScannerFile("01_streets\\0x050B6755.wav", "0x050B6755", "streets"); } }
        public static ScannerFile PopularStreet { get { return new ScannerFile("01_streets\\0x050C9979.wav", "0x050C9979", "streets"); } }
        public static ScannerFile LesbosLane { get { return new ScannerFile("01_streets\\0x0512495B.wav", "0x0512495B", "streets"); } }
        public static ScannerFile LibertyStreet { get { return new ScannerFile("01_streets\\0x057D565E.wav", "0x057D565E", "streets"); } }
        public static ScannerFile LasLegunasBlvd { get { return new ScannerFile("01_streets\\0x059B01E6.wav", "0x059B01E6", "streets"); } }
        public static ScannerFile AmarilloWay { get { return new ScannerFile("01_streets\\0x05C48E2D.wav", "0x05C48E2D", "streets"); } }
        public static ScannerFile HangmanAve { get { return new ScannerFile("01_streets\\0x05E9C1D2.wav", "0x05E9C1D2", "streets"); } }
        public static ScannerFile ElginAve { get { return new ScannerFile("01_streets\\0x05ED135D.wav", "0x05ED135D", "streets"); } }
        public static ScannerFile LaMoineStreet { get { return new ScannerFile("01_streets\\0x05EEED49.wav", "0x05EEED49", "streets"); } }
        public static ScannerFile FortZancudoApproachRoad { get { return new ScannerFile("01_streets\\0x05F099C8.wav", "0x05F099C8", "streets"); } }
        public static ScannerFile UnionStreet { get { return new ScannerFile("01_streets\\0x05FFB99E.wav", "0x05FFB99E", "streets"); } }
        public static ScannerFile MtHaanRoad { get { return new ScannerFile("01_streets\\0x060B40EB.wav", "0x060B40EB", "streets"); } }
        public static ScannerFile MtHaanDrive { get { return new ScannerFile("01_streets\\0x06170A98.wav", "0x06170A98", "streets"); } }
        public static ScannerFile BlvdDelPierro { get { return new ScannerFile("01_streets\\0x062B3516.wav", "0x062B3516", "streets"); } }
        public static ScannerFile ZancudoRoad { get { return new ScannerFile("01_streets\\0x0632CC3F.wav", "0x0632CC3F", "streets"); } }
        public static ScannerFile SignalStreet { get { return new ScannerFile("01_streets\\0x06428985.wav", "0x06428985", "streets"); } }
        public static ScannerFile CourtBIDK { get { return new ScannerFile("01_streets\\0x064A93B1.wav", "0x064A93B1", "streets"); } }
        public static ScannerFile AceJonesDrive { get { return new ScannerFile("01_streets\\0x064C6BCB.wav", "0x064C6BCB", "streets"); } }
        public static ScannerFile ShankStreet { get { return new ScannerFile("01_streets\\0x06895863.wav", "0x06895863", "streets"); } }
        public static ScannerFile NeelanAve { get { return new ScannerFile("01_streets\\0x068A7299.wav", "0x068A7299", "streets"); } }
        public static ScannerFile NorthRockfordDrive { get { return new ScannerFile("01_streets\\0x06992D16.wav", "0x06992D16", "streets"); } }
        public static ScannerFile CarsonAve { get { return new ScannerFile("01_streets\\0x06C26E31.wav", "0x06C26E31", "streets"); } }
        public static ScannerFile RodeoDrive { get { return new ScannerFile("01_streets\\0x06C7F383.wav", "0x06C7F383", "streets"); } }
        public static ScannerFile GomezStreet { get { return new ScannerFile("01_streets\\0x06F4E179.wav", "0x06F4E179", "streets"); } }
        public static ScannerFile DarrianAve { get { return new ScannerFile("01_streets\\0x071521F6.wav", "0x071521F6", "streets"); } }
        public static ScannerFile GrapseedMainStreet { get { return new ScannerFile("01_streets\\0x071EC39F.wav", "0x071EC39F", "streets"); } }
        public static ScannerFile NorthConkerAve { get { return new ScannerFile("01_streets\\0x072750C7.wav", "0x072750C7", "streets"); } }
        public static ScannerFile FenwellPlace { get { return new ScannerFile("01_streets\\0x0759F387.wav", "0x0759F387", "streets"); } }
        public static ScannerFile WestSilverlakeDrive { get { return new ScannerFile("01_streets\\0x075AB3CD.wav", "0x075AB3CD", "streets"); } }
        public static ScannerFile ZancudoAve { get { return new ScannerFile("01_streets\\0x07829707.wav", "0x07829707", "streets"); } }
        public static ScannerFile SanVitusBlvd { get { return new ScannerFile("01_streets\\0x07933135.wav", "SanVitusBlvd", "streets"); } }
        public static ScannerFile RubStreet { get { return new ScannerFile("01_streets\\0x07B5988A.wav", "0x07B5988A", "streets"); } }
        public static ScannerFile RunwayOne { get { return new ScannerFile("01_streets\\0x07F71C56.wav", "0x07F71C56", "streets"); } }
        public static ScannerFile ExceptionalistWay { get { return new ScannerFile("01_streets\\0x07FC1AAD.wav", "0x07FC1AAD", "streets"); } }
        public static ScannerFile KortzDrive { get { return new ScannerFile("01_streets\\0x07FCFC45.wav", "0x07FCFC45", "streets"); } }
        public static ScannerFile ZancudoGrandeValley { get { return new ScannerFile("01_streets\\0x08095013.wav", "0x08095013", "streets"); } }
        public static ScannerFile DunstableDrive { get { return new ScannerFile("01_streets\\0x083F0542.wav", "0x083F0542", "streets"); } }
        public static ScannerFile NorgowerStreet { get { return new ScannerFile("01_streets\\0x08409C07.wav", "0x08409C07", "streets"); } }
        public static ScannerFile ProcopioDrive { get { return new ScannerFile("01_streets\\0x0853209F.wav", "0x0853209F", "streets"); } }
        public static ScannerFile BearStreet { get { return new ScannerFile("01_streets\\0x08D99127.wav", "0x08D99127", "streets"); } }
        public static ScannerFile RockfordDrive { get { return new ScannerFile("01_streets\\0x08D9E251.wav", "0x08D9E251", "streets"); } }
        public static ScannerFile LindsayCircus { get { return new ScannerFile("01_streets\\0x08DE189A.wav", "0x08DE189A", "streets"); } }
        public static ScannerFile ElysianFieldsFreeway { get { return new ScannerFile("01_streets\\0x08E78CF6.wav", "0x08E78CF6", "streets"); } }
        public static ScannerFile MontanaAve { get { return new ScannerFile("01_streets\\0x08F4CA53.wav", "0x08F4CA53", "streets"); } }
        public static ScannerFile ElBurroBlvd { get { return new ScannerFile("01_streets\\0x090A8DDF.wav", "0x090A8DDF", "streets"); } }
        public static ScannerFile NorthLaCienega { get { return new ScannerFile("01_streets\\0x09231D7B.wav", "0x09231D7B", "streets"); } }
        public static ScannerFile BridgeStreet { get { return new ScannerFile("01_streets\\0x0947E0A1.wav", "0x0947E0A1", "streets"); } }
        public static ScannerFile CortezAve { get { return new ScannerFile("01_streets\\0x097B1644.wav", "0x097B1644", "streets"); } }
        public static ScannerFile NorthOrangeDrive { get { return new ScannerFile("01_streets\\0x098045CE.wav", "0x098045CE", "streets"); } }
        public static ScannerFile ForumDrive { get { return new ScannerFile("01_streets\\0x09B22349.wav", "0x09B22349", "streets"); } }
        public static ScannerFile RoyLowensteinBlvd { get { return new ScannerFile("01_streets\\0x0A1242A7.wav", "0x0A1242A7", "streets"); } }
        public static ScannerFile FantasticPlace { get { return new ScannerFile("01_streets\\0x0A359C18.wav", "0x0A359C18", "streets"); } }
        public static ScannerFile EastbourneWay { get { return new ScannerFile("01_streets\\0x0A4F62AD.wav", "EastborneWay", "streets"); } }
        public static ScannerFile CourtBIDK2 { get { return new ScannerFile("01_streets\\0x0A709BFF.wav", "0x0A709BFF", "streets"); } }
        public static ScannerFile NorthLaCienegaBlvd { get { return new ScannerFile("01_streets\\0x0A776023.wav", "0x0A776023", "streets"); } }
        public static ScannerFile AmarilloVista { get { return new ScannerFile("01_streets\\0x0A7B310C.wav", "AmarilloVista", "streets"); } }
        public static ScannerFile FringeDrive { get { return new ScannerFile("01_streets\\0x0AC29BE5.wav", "0x0AC29BE5", "streets"); } }
        public static ScannerFile ForceLaborPlace { get { return new ScannerFile("01_streets\\0x0B148304.wav", "0x0B148304", "streets"); } }
        public static ScannerFile OlympicFreeway { get { return new ScannerFile("01_streets\\0x0B292FF2.wav", "0x0B292FF2", "streets"); } }
        public static ScannerFile BanhamCanyon { get { return new ScannerFile("01_streets\\0x0B43DD81.wav", "0x0B43DD81", "streets"); } }
        public static ScannerFile NorthwiltonPlace { get { return new ScannerFile("01_streets\\0x0B4B1A02.wav", "0x0B4B1A02", "streets"); } }
        public static ScannerFile TowerWay { get { return new ScannerFile("01_streets\\0x0B991BCF.wav", "0x0B991BCF", "streets"); } }
        public static ScannerFile NopeStreet { get { return new ScannerFile("01_streets\\0x0BA78BC5.wav", "0x0BA78BC5", "streets"); } }
        public static ScannerFile SustanciaRoad { get { return new ScannerFile("01_streets\\0x0BA90F34.wav", "0x0BA90F34", "streets"); } }
        public static ScannerFile LagunaPlace { get { return new ScannerFile("01_streets\\0x0BCD8ABB.wav", "0x0BCD8ABB", "streets"); } }
        public static ScannerFile FreemarketStreet { get { return new ScannerFile("01_streets\\0x0BCF25BE.wav", "0x0BCF25BE", "streets"); } }
        public static ScannerFile ElectricAve { get { return new ScannerFile("01_streets\\0x0BDF5759.wav", "0x0BDF5759", "streets"); } }
        public static ScannerFile AvatorAve { get { return new ScannerFile("01_streets\\0x0BF59F04.wav", "0x0BF59F04", "streets"); } }
        public static ScannerFile ArgyleAve { get { return new ScannerFile("01_streets\\0x0C064C7C.wav", "0x0C064C7C", "streets"); } }
        public static ScannerFile CapitalBlvd { get { return new ScannerFile("01_streets\\0x0C2D5617.wav", "0x0C2D5617", "streets"); } }
        public static ScannerFile PalmwoodDrive { get { return new ScannerFile("01_streets\\0x0C536506.wav", "0x0C536506", "streets"); } }
        public static ScannerFile GingerStreet { get { return new ScannerFile("01_streets\\0x0C8D24B5.wav", "0x0C8D24B5", "streets"); } }
        public static ScannerFile UnionRoad { get { return new ScannerFile("01_streets\\0x0CE0CCEF.wav", "0x0CE0CCEF", "streets"); } }
        public static ScannerFile WestEclipseBlvd { get { return new ScannerFile("01_streets\\0x0D10195A.wav", "0x0D10195A", "streets"); } }
        public static ScannerFile EclipseBlvd { get { return new ScannerFile("01_streets\\0x0D1F9564.wav", "0x0D1F9564", "streets"); } }
        public static ScannerFile DutchLantonStreet { get { return new ScannerFile("01_streets\\0x0D232ACD.wav", "0x0D232ACD", "streets"); } }
        public static ScannerFile AltaPlace { get { return new ScannerFile("01_streets\\0x0D2643A4.wav", "0x0D2643A4", "streets"); } }
        public static ScannerFile VineDrive { get { return new ScannerFile("01_streets\\0x0D340899.wav", "0x0D340899", "streets"); } }
        public static ScannerFile CortezStreet { get { return new ScannerFile("01_streets\\0x0D34C14E.wav", "0x0D34C14E", "streets"); } }
        public static ScannerFile BanhamCanyonDrive { get { return new ScannerFile("01_streets\\0x0D558DA4.wav", "0x0D558DA4", "streets"); } }
        public static ScannerFile ImaginationCourt { get { return new ScannerFile("01_streets\\0x0D559440.wav", "0x0D559440", "streets"); } }
        public static ScannerFile HeartyWay { get { return new ScannerFile("01_streets\\0x0D68C123.wav", "0x0D68C123", "streets"); } }
        public static ScannerFile TeslaAve { get { return new ScannerFile("01_streets\\0x0DA66615.wav", "0x0DA66615", "streets"); } }
        public static ScannerFile SinnerStreet { get { return new ScannerFile("01_streets\\0x0DB7ED5A.wav", "0x0DB7ED5A", "streets"); } }
        public static ScannerFile PaliminoFreeway { get { return new ScannerFile("01_streets\\0x0DDB1753.wav", "0x0DDB1753", "streets"); } }
        public static ScannerFile GroveStreet { get { return new ScannerFile("01_streets\\0x0E14D472.wav", "0x0E14D472", "streets"); } }
        public static ScannerFile CatClawAve { get { return new ScannerFile("01_streets\\0x0E36940E.wav", "0x0E36940E", "streets"); } }
        public static ScannerFile SignalPlace { get { return new ScannerFile("01_streets\\0x0E4A65EE.wav", "0x0E4A65EE", "streets"); } }
        public static ScannerFile NicolaAve { get { return new ScannerFile("01_streets\\0x0E8C9E9E.wav", "0x0E8C9E9E", "streets"); } }
        public static ScannerFile SeeviewRoad { get { return new ScannerFile("01_streets\\0x0ED2C6CC.wav", "0x0ED2C6CC", "streets"); } }
        public static ScannerFile PicturePerfectWay { get { return new ScannerFile("01_streets\\0x0EF82A15.wav", "0x0EF82A15", "streets"); } }
        public static ScannerFile SouthChollaSpringsAve { get { return new ScannerFile("01_streets\\0x0F186293.wav", "0x0F186293", "streets"); } }
        public static ScannerFile GallileoRoad { get { return new ScannerFile("01_streets\\0x0F31A105.wav", "0x0F31A105", "streets"); } }
        public static ScannerFile DunstableLane { get { return new ScannerFile("01_streets\\0x0F6F2841.wav", "0x0F6F2841", "streets"); } }
        public static ScannerFile ApplinWay { get { return new ScannerFile("01_streets\\0x0F9320D2.wav", "0x0F9320D2", "streets"); } }
        public static ScannerFile MelanomaStreet { get { return new ScannerFile("01_streets\\0x0FE3AADE.wav", "0x0FE3AADE", "streets"); } }
        public static ScannerFile DutchLondonStreet { get { return new ScannerFile("01_streets\\0x100B2FFA.wav", "0x100B2FFA", "streets"); } }
        public static ScannerFile VitasStreet { get { return new ScannerFile("01_streets\\0x102CDEDE.wav", "0x102CDEDE", "streets"); } }
        public static ScannerFile MelroseAve { get { return new ScannerFile("01_streets\\0x103A9F18.wav", "0x103A9F18", "streets"); } }
        public static ScannerFile InnocenceBlvd { get { return new ScannerFile("01_streets\\0x103C12F0.wav", "0x103C12F0", "streets"); } }
        public static ScannerFile AtleyStreet { get { return new ScannerFile("01_streets\\0x104D6E7E.wav", "0x104D6E7E", "streets"); } }
        public static ScannerFile SouthPacificAve { get { return new ScannerFile("01_streets\\0x105CD3F7.wav", "0x105CD3F7", "streets"); } }
        public static ScannerFile PicturePerfectDrive { get { return new ScannerFile("01_streets\\0x1075F96B.wav", "0x1075F96B", "streets"); } }
        public static ScannerFile DutchThompsonStreet { get { return new ScannerFile("01_streets\\0x1097503F.wav", "0x1097503F", "streets"); } }
        public static ScannerFile AdamsAppleBlvd { get { return new ScannerFile("01_streets\\0x109F69A2.wav", "0x109F69A2", "streets"); } }
        public static ScannerFile EdwardWay { get { return new ScannerFile("01_streets\\0x10CF0302.wav", "0x10CF0302", "streets"); } }
        public static ScannerFile ChollaRoad { get { return new ScannerFile("01_streets\\0x10D7E34B.wav", "0x10D7E34B", "streets"); } }
        public static ScannerFile UtopiaGardens { get { return new ScannerFile("01_streets\\0x10E079B6.wav", "0x10E079B6", "streets"); } }
        public static ScannerFile CarcerWay { get { return new ScannerFile("01_streets\\0x10E8FF7B.wav", "0x10E8FF7B", "streets"); } }
        public static ScannerFile PeacefulStreet { get { return new ScannerFile("01_streets\\0x10F2DD3E.wav", "0x10F2DD3E", "streets"); } }
        public static ScannerFile AmericanoWay { get { return new ScannerFile("01_streets\\0x1101B476.wav", "0x1101B476", "streets"); } }
        public static ScannerFile PyriteAve { get { return new ScannerFile("01_streets\\0x11059B10.wav", "0x11059B10", "streets"); } }
        public static ScannerFile ArgyleAve2 { get { return new ScannerFile("01_streets\\0x11089681.wav", "0x11089681", "streets"); } }
        public static ScannerFile HawickAve2 { get { return new ScannerFile("01_streets\\0x11212529.wav", "0x11212529", "streets"); } }
        public static ScannerFile TangerineStreet { get { return new ScannerFile("01_streets\\0x112CCCA7.wav", "0x112CCCA7", "streets"); } }
        public static ScannerFile PortolaDrive { get { return new ScannerFile("01_streets\\0x1137CBEA.wav", "0x1137CBEA", "streets"); } }
        public static ScannerFile WispyMoundDrive { get { return new ScannerFile("01_streets\\0x1181A1F7.wav", "0x1181A1F7", "streets"); } }
        public static ScannerFile WildOatsDrive { get { return new ScannerFile("01_streets\\0x11A03703.wav", "0x11A03703", "streets"); } }
        public static ScannerFile GrunnichPlace { get { return new ScannerFile("01_streets\\0x11D24FED.wav", "0x11D24FED", "streets"); } }
        public static ScannerFile CockandGinDrive { get { return new ScannerFile("01_streets\\0x11FC3B6D.wav", "0x11FC3B6D", "streets"); } }
        public static ScannerFile SardineStreet { get { return new ScannerFile("01_streets\\0x1206C213.wav", "0x1206C213", "streets"); } }
        public static ScannerFile ClintonAve { get { return new ScannerFile("01_streets\\0x120AA4AF.wav", "0x120AA4AF", "streets"); } }
        public static ScannerFile CalapiaRoad { get { return new ScannerFile("01_streets\\0x126D3515.wav", "0x126D3515", "streets"); } }
        public static ScannerFile CourtA { get { return new ScannerFile("01_streets\\0x12853681.wav", "0x12853681", "streets"); } }
        public static ScannerFile GloryWay { get { return new ScannerFile("01_streets\\0x12A0D4E0.wav", "0x12A0D4E0", "streets"); } }
        public static ScannerFile SonoraRoad { get { return new ScannerFile("01_streets\\0x12A17392.wav", "0x12A17392", "streets"); } }
        public static ScannerFile NorthArcherAve { get { return new ScannerFile("01_streets\\0x12D34D96.wav", "0x12D34D96", "streets"); } }
        public static ScannerFile EqualityWay { get { return new ScannerFile("01_streets\\0x132C0BF1.wav", "0x132C0BF1", "streets"); } }
        public static ScannerFile ElRanchoBlvd { get { return new ScannerFile("01_streets\\0x132CC4F5.wav", "0x132CC4F5", "streets"); } }
        public static ScannerFile SouthRockfordDrive { get { return new ScannerFile("01_streets\\0x133D8310.wav", "0x133D8310", "streets"); } }
        public static ScannerFile MorningwoodBlvd2 { get { return new ScannerFile("01_streets\\0x135DBD79.wav", "0x135DBD79", "streets"); } }
        public static ScannerFile SouthMoMiltonDrive { get { return new ScannerFile("01_streets\\0x13B44AA4.wav", "0x13B44AA4", "streets"); } }
        public static ScannerFile MovieStarWay { get { return new ScannerFile("01_streets\\0x13D8BDBC.wav", "0x13D8BDBC", "streets"); } }
        public static ScannerFile DelouasAve { get { return new ScannerFile("01_streets\\0x13E17105.wav", "0x13E17105", "streets"); } }
        public static ScannerFile JoshuaRoad { get { return new ScannerFile("01_streets\\0x140CA80D.wav", "0x140CA80D", "streets"); } }
        public static ScannerFile HillcrestAve { get { return new ScannerFile("01_streets\\0x1435B4D1.wav", "0x1435B4D1", "streets"); } }
        public static ScannerFile ChumStreet { get { return new ScannerFile("01_streets\\0x143A5D42.wav", "0x143A5D42", "streets"); } }
        public static ScannerFile PalaminoAve { get { return new ScannerFile("01_streets\\0x144E6DB1.wav", "0x144E6DB1", "streets"); } }
        public static ScannerFile EmperialBlvd { get { return new ScannerFile("01_streets\\0x147A5B07.wav", "0x147A5B07", "streets"); } }
        public static ScannerFile SouthArsenalStreet { get { return new ScannerFile("01_streets\\0x14AA8C14.wav", "0x14AA8C14", "streets"); } }
        public static ScannerFile NowhereRoad { get { return new ScannerFile("01_streets\\0x14BD713A.wav", "0x14BD713A", "streets"); } }
        public static ScannerFile VineStreet { get { return new ScannerFile("01_streets\\0x14CD7C43.wav", "0x14CD7C43", "streets"); } }
        public static ScannerFile DellAve { get { return new ScannerFile("01_streets\\0x14D34F55.wav", "0x14D34F55", "streets"); } }
        public static ScannerFile CovenantAve { get { return new ScannerFile("01_streets\\0x14FE3C11.wav", "0x14FE3C11", "streets"); } }
        public static ScannerFile NorthKalafiaWay { get { return new ScannerFile("01_streets\\0x1527D88D.wav", "0x1527D88D", "streets"); } }
        public static ScannerFile ChollaSpringsAve { get { return new ScannerFile("01_streets\\0x1570583B.wav", "0x1570583B", "streets"); } }
        public static ScannerFile HeritageWay2 { get { return new ScannerFile("01_streets\\0x15858309.wav", "0x15858309", "streets"); } }
        public static ScannerFile EightMiltonParkway { get { return new ScannerFile("01_streets\\0x1587AE8A.wav", "0x1587AE8A", "streets"); } }
        public static ScannerFile DeckerStreet { get { return new ScannerFile("01_streets\\0x15D9D369.wav", "0x15D9D369", "streets"); } }
        public static ScannerFile MagellanAve { get { return new ScannerFile("01_streets\\0x15F419A1.wav", "0x15F419A1", "streets"); } }
        public static ScannerFile BacklotBlvd2 { get { return new ScannerFile("01_streets\\0x15F8B6C2.wav", "0x15F8B6C2", "streets"); } }
        public static ScannerFile MountainViewDrive { get { return new ScannerFile("01_streets\\0x164F3B7B.wav", "0x164F3B7B", "streets"); } }
        public static ScannerFile ElHamberDrive { get { return new ScannerFile("01_streets\\0x16A7EEAB.wav", "0x16A7EEAB", "streets"); } }
        public static ScannerFile TugStreet { get { return new ScannerFile("01_streets\\0x16BCC9F1.wav", "0x16BCC9F1", "streets"); } }
        public static ScannerFile BarracudaStreet { get { return new ScannerFile("01_streets\\0x16D97CC9.wav", "0x16D97CC9", "streets"); } }
        public static ScannerFile MountVinewoodDrive { get { return new ScannerFile("01_streets\\0x17410E3C.wav", "0x17410E3C", "streets"); } }
        public static ScannerFile VinewoodBlvd { get { return new ScannerFile("01_streets\\0x1743E9A3.wav", "0x1743E9A3", "streets"); } }
        public static ScannerFile JamestownStreet { get { return new ScannerFile("01_streets\\0x175B1386.wav", "0x175B1386", "streets"); } }
        public static ScannerFile SouthMagoFenderDrive { get { return new ScannerFile("01_streets\\0x1783A6E2.wav", "0x1783A6E2", "streets"); } }
        public static ScannerFile HighSpanishAve { get { return new ScannerFile("01_streets\\0x178FDF73.wav", "0x178FDF73", "streets"); } }
        public static ScannerFile SonoraFreeway { get { return new ScannerFile("01_streets\\0x17983309.wav", "0x17983309", "streets"); } }
        public static ScannerFile FortZancudoApproachRoad2 { get { return new ScannerFile("01_streets\\0x17AB7D3D.wav", "0x17AB7D3D", "streets"); } }
        public static ScannerFile BoinbinoRoad { get { return new ScannerFile("01_streets\\0x17ADEA2A.wav", "0x17ADEA2A", "streets"); } }
        public static ScannerFile YorkStreet { get { return new ScannerFile("01_streets\\0x17C66B96.wav", "0x17C66B96", "streets"); } }
        public static ScannerFile YouingStreet { get { return new ScannerFile("01_streets\\0x17F87FDC.wav", "0x17F87FDC", "streets"); } }
        public static ScannerFile BaseCityIncline { get { return new ScannerFile("01_streets\\0x18182B71.wav", "0x18182B71", "streets"); } }
        public static ScannerFile MeteorStreet { get { return new ScannerFile("01_streets\\0x181A3C88.wav", "0x181A3C88", "streets"); } }
        public static ScannerFile CrenshawBlvd { get { return new ScannerFile("01_streets\\0x181EE2FD.wav", "0x181EE2FD", "streets"); } }
        public static ScannerFile HamstonDrive { get { return new ScannerFile("01_streets\\0x18226A66.wav", "0x18226A66", "streets"); } }
        public static ScannerFile SouthBlvdDelPierro { get { return new ScannerFile("01_streets\\0x183EA3D8.wav", "0x183EA3D8", "streets"); } }
        public static ScannerFile PanoramaDrive { get { return new ScannerFile("01_streets\\0x184DB6D1.wav", "0x184DB6D1", "streets"); } }
        public static ScannerFile BaseCityAve { get { return new ScannerFile("01_streets\\0x18787617.wav", "0x18787617", "streets"); } }
        public static ScannerFile TackleStreet { get { return new ScannerFile("01_streets\\0x18903C12.wav", "0x18903C12", "streets"); } }
        public static ScannerFile SamAustinDrive { get { return new ScannerFile("01_streets\\0x18975F9C.wav", "0x18975F9C", "streets"); } }
        public static ScannerFile VespucciBlvd { get { return new ScannerFile("01_streets\\0x189DF2C9.wav", "0x189DF2C9", "streets"); } }
        public static ScannerFile ConquistadorStreet { get { return new ScannerFile("01_streets\\0x18C7869E.wav", "0x18C7869E", "streets"); } }
        public static ScannerFile SandcastleWay { get { return new ScannerFile("01_streets\\0x18DDA36A.wav", "0x18DDA36A", "streets"); } }
        public static ScannerFile SonoraWay { get { return new ScannerFile("01_streets\\0x18DFF9AE.wav", "0x18DFF9AE", "streets"); } }
        public static ScannerFile PerfStreet { get { return new ScannerFile("01_streets\\0x190212E1.wav", "0x190212E1", "streets"); } }
        public static ScannerFile LolitaAve { get { return new ScannerFile("01_streets\\0x190941AE.wav", "0x190941AE", "streets"); } }
        public static ScannerFile MirrorParkBlvd { get { return new ScannerFile("01_streets\\0x192B2F50.wav", "0x192B2F50", "streets"); } }
        public static ScannerFile HangarWay { get { return new ScannerFile("01_streets\\0x19300CFB.wav", "0x19300CFB", "streets"); } }
        public static ScannerFile ElBurroBlvd2 { get { return new ScannerFile("01_streets\\0x1941E833.wav", "0x1941E833", "streets"); } }
        public static ScannerFile BuccanierWay { get { return new ScannerFile("01_streets\\0x19553E1E.wav", "0x19553E1E", "streets"); } }
        public static ScannerFile SupplyStreet { get { return new ScannerFile("01_streets\\0x19602650.wav", "0x19602650", "streets"); } }
        public static ScannerFile PicturePerfectWay2 { get { return new ScannerFile("01_streets\\0x19743F10.wav", "0x19743F10", "streets"); } }
        public static ScannerFile DavisAve { get { return new ScannerFile("01_streets\\0x1987B393.wav", "0x1987B393", "streets"); } }
        public static ScannerFile SteelWay { get { return new ScannerFile("01_streets\\0x1987EA0F.wav", "0x1987EA0F", "streets"); } }
        public static ScannerFile SpanishAve { get { return new ScannerFile("01_streets\\0x19B6651E.wav", "0x19B6651E", "streets"); } }
        public static ScannerFile EastJoshuaRoad { get { return new ScannerFile("01_streets\\0x19D51F0C.wav", "0x19D51F0C", "streets"); } }
        public static ScannerFile NorthHighlandAve { get { return new ScannerFile("01_streets\\0x19EC1BD4.wav", "0x19EC1BD4", "streets"); } }
        public static ScannerFile MagwavevendorDrive { get { return new ScannerFile("01_streets\\0x19EEA2EB.wav", "0x19EEA2EB", "streets"); } }
        public static ScannerFile NorthLaBreaAve { get { return new ScannerFile("01_streets\\0x19F716C1.wav", "0x19F716C1", "streets"); } }
        public static ScannerFile TongvaDrive { get { return new ScannerFile("01_streets\\0x1A05AA29.wav", "0x1A05AA29", "streets"); } }
        public static ScannerFile SmokeTreeRoad { get { return new ScannerFile("01_streets\\0x1A3A317D.wav", "0x1A3A317D", "streets"); } }
        public static ScannerFile NorthMagiovendorDrive { get { return new ScannerFile("01_streets\\0x1A568E1F.wav", "0x1A568E1F", "streets"); } }
        public static ScannerFile NewEmpireWay { get { return new ScannerFile("01_streets\\0x1A56E636.wav", "0x1A56E636", "streets"); } }
        public static ScannerFile BarbarinoRoad { get { return new ScannerFile("01_streets\\0x1A970537.wav", "0x1A970537", "streets"); } }
        public static ScannerFile PerinoPlace { get { return new ScannerFile("01_streets\\0x1AC3B09F.wav", "0x1AC3B09F", "streets"); } }
        public static ScannerFile GrenwichWay { get { return new ScannerFile("01_streets\\0x1AD1A701.wav", "0x1AD1A701", "streets"); } }
        public static ScannerFile OneilWay { get { return new ScannerFile("01_streets\\0x1AE3822B.wav", "0x1AE3822B", "streets"); } }
        public static ScannerFile ProcopioPromenade { get { return new ScannerFile("01_streets\\0x1AEB1670.wav", "0x1AEB1670", "streets"); } }
        public static ScannerFile JamestownStreet2 { get { return new ScannerFile("01_streets\\0x1B1C2616.wav", "0x1B1C2616", "streets"); } }
        public static ScannerFile NorthPoplerStreet { get { return new ScannerFile("01_streets\\0x1B206290.wav", "0x1B206290", "streets"); } }
        public static ScannerFile AltopiaParkway { get { return new ScannerFile("01_streets\\0x1B406761.wav", "0x1B406761", "streets"); } }
        public static ScannerFile VinewoodParkDrive { get { return new ScannerFile("01_streets\\0x1B710698.wav", "0x1B710698", "streets"); } }
        public static ScannerFile HanwellAve { get { return new ScannerFile("01_streets\\0x1B8204A1.wav", "0x1B8204A1", "streets"); } }
        public static ScannerFile CenturyFreeway { get { return new ScannerFile("01_streets\\0x1B94A52A.wav", "0x1B94A52A", "streets"); } }
        public static ScannerFile ArgyleAve3 { get { return new ScannerFile("01_streets\\0x1BB8EBE1.wav", "0x1BB8EBE1", "streets"); } }
        public static ScannerFile CacobellAve { get { return new ScannerFile("01_streets\\0x1BDEB04B.wav", "0x1BDEB04B", "streets"); } }
        public static ScannerFile NikolaPlace { get { return new ScannerFile("01_streets\\0x1BF71FD3.wav", "0x1BF71FD3", "streets"); } }
        public static ScannerFile AmarilloWay2 { get { return new ScannerFile("01_streets\\0x1BFFBAA2.wav", "0x1BFFBAA2", "streets"); } }
        public static ScannerFile PaletoBlvd { get { return new ScannerFile("01_streets\\0x1C078234.wav", "0x1C078234", "streets"); } }
        public static ScannerFile WestGalileoAve { get { return new ScannerFile("01_streets\\0x1C2299E2.wav", "0x1C2299E2", "streets"); } }
        public static ScannerFile AlgonquinBlvd { get { return new ScannerFile("01_streets\\0x1C2849B4.wav", "0x1C2849B4", "streets"); } }
        public static ScannerFile SwissStreet { get { return new ScannerFile("01_streets\\0x1C7B9A4B.wav", "0x1C7B9A4B", "streets"); } }
        public static ScannerFile McDonaldStreet { get { return new ScannerFile("01_streets\\0x1D075A11.wav", "0x1D075A11", "streets"); } }
        public static ScannerFile ProsperityStreet { get { return new ScannerFile("01_streets\\0x1D12DF32.wav", "0x1D12DF32", "streets"); } }
        public static ScannerFile DryDockStreet { get { return new ScannerFile("01_streets\\0x1D2FFD5B.wav", "0x1D2FFD5B", "streets"); } }
        public static ScannerFile HillcrestRidgeAccessRoad { get { return new ScannerFile("01_streets\\0x1D3D5C62.wav", "0x1D3D5C62", "streets"); } }
        public static ScannerFile EastSilverlakeDrive { get { return new ScannerFile("01_streets\\0x1D4D9646.wav", "0x1D4D9646", "streets"); } }
        public static ScannerFile MirrorPlace { get { return new ScannerFile("01_streets\\0x1D5268E1.wav", "0x1D5268E1", "streets"); } }
        public static ScannerFile BaytreeCanyonRoad { get { return new ScannerFile("01_streets\\0x1D5D46A1.wav", "0x1D5D46A1", "streets"); } }
        public static ScannerFile CassidyTrail { get { return new ScannerFile("01_streets\\0x1D5F57C1.wav", "0x1D5F57C1", "streets"); } }
        public static ScannerFile ArmadilloAve { get { return new ScannerFile("01_streets\\0x1D600A1D.wav", "0x1D600A1D", "streets"); } }
        public static ScannerFile GrapeseedAve { get { return new ScannerFile("01_streets\\0x1D77E55F.wav", "0x1D77E55F", "streets"); } }
        public static ScannerFile CenturyBlvd { get { return new ScannerFile("01_streets\\0x1D910EAE.wav", "0x1D910EAE", "streets"); } }
        public static ScannerFile CrusadeRoad { get { return new ScannerFile("01_streets\\0x1DAC5255.wav", "0x1DAC5255", "streets"); } }
        public static ScannerFile SouthChollaSpringsAve2 { get { return new ScannerFile("01_streets\\0x1DEDC03E.wav", "0x1DEDC03E", "streets"); } }
        public static ScannerFile GrenwichParkway { get { return new ScannerFile("01_streets\\0x1DF52DF6.wav", "0x1DF52DF6", "streets"); } }
        public static ScannerFile VincentThomasBridge { get { return new ScannerFile("01_streets\\0x1DF6B4D3.wav", "0x1DF6B4D3", "streets"); } }
        public static ScannerFile CaliasAve { get { return new ScannerFile("01_streets\\0x1E03DBA4.wav", "0x1E03DBA4", "streets"); } }
        public static ScannerFile NullDrive { get { return new ScannerFile("01_streets\\0x1E2AF575.wav", "0x1E2AF575", "streets"); } }
        public static ScannerFile DiedianDrive { get { return new ScannerFile("01_streets\\0x1E2D9A08.wav", "0x1E2D9A08", "streets"); } }
        public static ScannerFile SouthSotoStreet { get { return new ScannerFile("01_streets\\0x1E53AAE0.wav", "0x1E53AAE0", "streets"); } }
        public static ScannerFile CapeCatfish { get { return new ScannerFile("01_streets\\0x1E55985D.wav", "0x1E55985D", "streets"); } }
        public static ScannerFile CougarAve { get { return new ScannerFile("01_streets\\0x1E61D52C.wav", "0x1E61D52C", "streets"); } }
        public static ScannerFile SanAndreasAve { get { return new ScannerFile("01_streets\\0x1E6AFCCC.wav", "0x1E6AFCCC", "streets"); } }
        public static ScannerFile FudgeLane { get { return new ScannerFile("01_streets\\0x1E763D7D.wav", "0x1E763D7D", "streets"); } }
        public static ScannerFile NorhtwesternAve { get { return new ScannerFile("01_streets\\0x1E8F5B45.wav", "0x1E8F5B45", "streets"); } }
        public static ScannerFile RedDesertAve { get { return new ScannerFile("01_streets\\0x1EBD7733.wav", "0x1EBD7733", "streets"); } }
        public static ScannerFile NorthSheldonAve { get { return new ScannerFile("01_streets\\0x1EC3CE23.wav", "0x1EC3CE23", "streets"); } }
        public static ScannerFile CatfishView { get { return new ScannerFile("01_streets\\0x1EC679E1.wav", "0x1EC679E1", "streets"); } }
        public static ScannerFile GreatOceanHighway { get { return new ScannerFile("01_streets\\0x1EE59AF9.wav", "0x1EE59AF9", "streets"); } }
        public static ScannerFile CaesarPlace { get { return new ScannerFile("01_streets\\0x1EE8F047.wav", "0x1EE8F047", "streets"); } }
        public static ScannerFile WestMirrorDrive { get { return new ScannerFile("01_streets\\0x1EFF682E.wav", "0x1EFF682E", "streets"); } }
        public static ScannerFile SinnersPassage { get { return new ScannerFile("01_streets\\0x1F027A0E.wav", "0x1F027A0E", "streets"); } }
        public static ScannerFile MarinaDrive { get { return new ScannerFile("01_streets\\0x1F053811.wav", "0x1F053811", "streets"); } }
        public static ScannerFile KimbalHillDrive { get { return new ScannerFile("01_streets\\0x1F14CC99.wav", "0x1F14CC99", "streets"); } }
        public static ScannerFile BraddockTunnel { get { return new ScannerFile("01_streets\\0x1F55F0A8.wav", "0x1F55F0A8", "streets"); } }
        public static ScannerFile WorfStreet { get { return new ScannerFile("01_streets\\0x1FB2B2AA.wav", "0x1FB2B2AA", "streets"); } }
        public static ScannerFile CockandGinDrive2 { get { return new ScannerFile("01_streets\\0x1FB496DF.wav", "0x1FB496DF", "streets"); } }
        public static ScannerFile Skyway { get { return new ScannerFile("01_streets\\0x1FF7BB6A.wav", "0x1FF7BB6A", "streets"); } }
        public static ScannerFile ColliseumStreet { get { return new ScannerFile("01_streets\\0x1FF7CB59.wav", "0x1FF7CB59", "streets"); } }
    }
    public class suspects_are
    {
        public static ScannerFile HASH0262F796 { get { return new ScannerFile("01_suspects_are\\0x0262F796.wav", "0x0262F796", "suspects_are"); } }
        public static ScannerFile HASH0323B926 { get { return new ScannerFile("01_suspects_are\\0x0323B926.wav", "0x0323B926", "suspects_are"); } }
        public static ScannerFile HASH0DE38E9B { get { return new ScannerFile("01_suspects_are\\0x0DE38E9B.wav", "0x0DE38E9B", "suspects_are"); } }
        public static ScannerFile HASH0EA45019 { get { return new ScannerFile("01_suspects_are\\0x0EA45019.wav", "0x0EA45019", "suspects_are"); } }
    }
    public class suspects_are_members_of
    {
        public static ScannerFile HASH05EBE717 { get { return new ScannerFile("01_suspects_are_members_of\\0x05EBE717.wav", "0x05EBE717", "suspects_are_members_of"); } }
        public static ScannerFile HASH17B5CAAB { get { return new ScannerFile("01_suspects_are_members_of\\0x17B5CAAB.wav", "0x17B5CAAB", "suspects_are_members_of"); } }
        public static ScannerFile HASH1A4BCF5F { get { return new ScannerFile("01_suspects_are_members_of\\0x1A4BCF5F.wav", "0x1A4BCF5F", "suspects_are_members_of"); } }
    }
    public class suspects_heading
    {
        public static ScannerFile HASH00F4821D { get { return new ScannerFile("01_suspects_heading\\0x00F4821D.wav", "0x00F4821D", "suspects_heading"); } }
        public static ScannerFile HASH0389C747 { get { return new ScannerFile("01_suspects_heading\\0x0389C747.wav", "0x0389C747", "suspects_heading"); } }
        public static ScannerFile HASH13312696 { get { return new ScannerFile("01_suspects_heading\\0x13312696.wav", "0x13312696", "suspects_heading"); } }
        public static ScannerFile HASH15CAEBC9 { get { return new ScannerFile("01_suspects_heading\\0x15CAEBC9.wav", "0x15CAEBC9", "suspects_heading"); } }
        public static ScannerFile HASH1A56B4E0 { get { return new ScannerFile("01_suspects_heading\\0x1A56B4E0.wav", "0x1A56B4E0", "suspects_heading"); } }
    }
    public class suspects_last_seen
    {
        public static ScannerFile SuspectLastSeen { get { return new ScannerFile("01_suspects_last_seen\\0x02637742.wav", "0x02637742", "suspects_last_seen"); } }
        public static ScannerFile HASH04E93C4D { get { return new ScannerFile("01_suspects_last_seen\\0x04E93C4D.wav", "0x04E93C4D", "suspects_last_seen"); } }
        public static ScannerFile HASH0BCB0A12 { get { return new ScannerFile("01_suspects_last_seen\\0x0BCB0A12.wav", "0x0BCB0A12", "suspects_last_seen"); } }
        public static ScannerFile HASH1D712D60 { get { return new ScannerFile("01_suspects_last_seen\\0x1D712D60.wav", "0x1D712D60", "suspects_last_seen"); } }
    }
    public class suspect_arrested
    {
        public static ScannerFile SuspectIncustody { get { return new ScannerFile("01_suspect_arrested\\0x0C51EC8C.wav", "Suspect in custody", "suspect_arrested"); } }
        public static ScannerFile SuspectApprehended { get { return new ScannerFile("01_suspect_arrested\\0x1627003A.wav", "Suspect Aprehended", "suspect_arrested"); } }
        public static ScannerFile SuspectIsTenFifteen { get { return new ScannerFile("01_suspect_arrested\\0x1A944911.wav", "Suspect is 10-15", "suspect_arrested"); } }
        public static ScannerFile TenFifteenSuspectInCustody { get { return new ScannerFile("01_suspect_arrested\\0x1EC51175.wav", "10-15 Suspect in Custody", "suspect_arrested"); } }
    }
    public class suspect_eluded_pt_1
    {
        public static ScannerFile SuspectLost { get { return new ScannerFile("01_suspect_eluded_pt_1\\0x036E44E1.wav", "Suspect Lost", "suspect_eluded_pt_1"); } }
        public static ScannerFile SuspectHasEvadedOfficers { get { return new ScannerFile("01_suspect_eluded_pt_1\\0x105E9EC1.wav", "Suspect Has Evaded Officers", "suspect_eluded_pt_1"); } }
        public static ScannerFile SuspectEvadedPursuingOfficiers { get { return new ScannerFile("01_suspect_eluded_pt_1\\0x15A02945.wav", "Suspect Evaded Pursuing Officiers", "suspect_eluded_pt_1"); } }
        public static ScannerFile OfficiersHaveLostVisualOnSuspect { get { return new ScannerFile("01_suspect_eluded_pt_1\\0x1FD43DAC.wav", "Officiers Have Lost Visual on Suspect", "suspect_eluded_pt_1"); } }
    }
    public class suspect_eluded_pt_2
    {
        public static ScannerFile AllUnitsStayInTheArea { get { return new ScannerFile("01_suspect_eluded_pt_2\\0x05A6E31C.wav", "All Units stay in the area", "suspect_eluded_pt_2"); } }
        public static ScannerFile AllUnitsRemainOnAlert { get { return new ScannerFile("01_suspect_eluded_pt_2\\0x145D808B.wav", "All Units Remain on alert", "suspect_eluded_pt_2"); } }
        public static ScannerFile AllUnitsStandby { get { return new ScannerFile("01_suspect_eluded_pt_2\\0x1EBB5543.wav", "All Units Standby", "suspect_eluded_pt_2"); } }
    }
    public class suspect_has
    {
        public static ScannerFile HASH004D3F60 { get { return new ScannerFile("01_suspect_has\\0x004D3F60.wav", "0x004D3F60", "suspect_has"); } }
        public static ScannerFile HASH0E6C9B9F { get { return new ScannerFile("01_suspect_has\\0x0E6C9B9F.wav", "0x0E6C9B9F", "suspect_has"); } }
        public static ScannerFile HASH13626589 { get { return new ScannerFile("01_suspect_has\\0x13626589.wav", "0x13626589", "suspect_has"); } }
    }
    public class suspect_heading
    {
        public static ScannerFile SuspectHeading { get { return new ScannerFile("01_suspect_heading\\0x04AE12BC.wav", "Suspect heading", "suspect_heading"); } }
        public static ScannerFile TargetSeenHeading { get { return new ScannerFile("01_suspect_heading\\0x0DA624A7.wav", "Target seen heading", "suspect_heading"); } }
        public static ScannerFile TargetLastSeenHeading { get { return new ScannerFile("01_suspect_heading\\0x1200ED5C.wav", "Target last seen heading", "suspect_heading"); } }
        public static ScannerFile TargetHeading { get { return new ScannerFile("01_suspect_heading\\0x1AD27F02.wav", "Target Heading", "suspect_heading"); } }
        public static ScannerFile TargetSpottedHeading { get { return new ScannerFile("01_suspect_heading\\0x1CD3C302.wav", "Target spotted heading", "suspect_heading"); } }
        public static ScannerFile TargetReportedHeading { get { return new ScannerFile("01_suspect_heading\\0x1F668829.wav", "Target reported heading", "suspect_heading"); } }
    }
    public class suspect_is
    {
        public static ScannerFile SuspectIs { get { return new ScannerFile("01_suspect_is\\0x09AB3AEA.wav", "SuspectIs", "suspect_is"); } }
        public static ScannerFile CriminalIs { get { return new ScannerFile("01_suspect_is\\0x0F630663.wav", "CriminalIs", "suspect_is"); } }
        public static ScannerFile TargetIs { get { return new ScannerFile("01_suspect_is\\0x19A5DADF.wav", "TargetIs", "suspect_is"); } }
    }
    public class suspect_is_a_member_of
    {
        public static ScannerFile HASH0789E155 { get { return new ScannerFile("01_suspect_is_a_member_of\\0x0789E155.wav", "0x0789E155", "suspect_is_a_member_of"); } }
        public static ScannerFile HASH14D37BED { get { return new ScannerFile("01_suspect_is_a_member_of\\0x14D37BED.wav", "0x14D37BED", "suspect_is_a_member_of"); } }
        public static ScannerFile HASH184F02E0 { get { return new ScannerFile("01_suspect_is_a_member_of\\0x184F02E0.wav", "0x184F02E0", "suspect_is_a_member_of"); } }
    }
    public class suspect_is_wearing
    {
        public static ScannerFile HASH04A18D33 { get { return new ScannerFile("01_suspect_is_wearing\\0x04A18D33.wav", "0x04A18D33", "suspect_is_wearing"); } }
        public static ScannerFile HASH0AE599BA { get { return new ScannerFile("01_suspect_is_wearing\\0x0AE599BA.wav", "0x0AE599BA", "suspect_is_wearing"); } }
        public static ScannerFile HASH12E369B6 { get { return new ScannerFile("01_suspect_is_wearing\\0x12E369B6.wav", "0x12E369B6", "suspect_is_wearing"); } }
        public static ScannerFile HASH1CA43D37 { get { return new ScannerFile("01_suspect_is_wearing\\0x1CA43D37.wav", "0x1CA43D37", "suspect_is_wearing"); } }
    }
    public class suspect_last_seen
    {
        public static ScannerFile TargetLastReported { get { return new ScannerFile("01_suspect_last_seen\\0x021E1830.wav", "TargetLastReported", "suspect_last_seen"); } }
        public static ScannerFile TargetSpotted { get { return new ScannerFile("01_suspect_last_seen\\0x09C86781.wav", "TargetSpotted", "suspect_last_seen"); } }
        public static ScannerFile SuspectSpotted { get { return new ScannerFile("01_suspect_last_seen\\0x0D84AEFA.wav", "SuspectSpotted", "suspect_last_seen"); } }
        public static ScannerFile TargetLastSeen { get { return new ScannerFile("01_suspect_last_seen\\0x110375FA.wav", "TargetLastSeen", "suspect_last_seen"); } }
        public static ScannerFile TargetIs { get { return new ScannerFile("01_suspect_last_seen\\0x17A6C33E.wav", "TargetIs", "suspect_last_seen"); } }
    }
    public class suspect_license_plate
    {
        public static ScannerFile TargetLicensePlate { get { return new ScannerFile("01_suspect_license_plate\\0x02F918C0.wav", "0x02F918C0", "target license plate"); } }
        public static ScannerFile SuspectsLicensePlate01 { get { return new ScannerFile("01_suspect_license_plate\\0x0C436B59.wav", "0x0C436B59", "suspects license plate"); } }
        public static ScannerFile SuspectLicensePlate { get { return new ScannerFile("01_suspect_license_plate\\0x123C7746.wav", "0x123C7746", "suspects license plate"); } }
        public static ScannerFile TargetsLicensePlate { get { return new ScannerFile("01_suspect_license_plate\\0x15A1BE11.wav", "0x15A1BE11", "targets license plate"); } }
        public static ScannerFile TargetVehicleLicensePlate { get { return new ScannerFile("01_suspect_license_plate\\0x17AB4229.wav", "0x17AB4229", "target vehicle license plate"); } }
        public static ScannerFile SuspectsLicensePlate02 { get { return new ScannerFile("01_suspect_license_plate\\0x1E8D8FEE.wav", "Suspects Licens ePlate", "suspects license plate"); } }
    }
    public class suspect_wasted
    {
        public static ScannerFile HASH018C67CD { get { return new ScannerFile("01_suspect_wasted\\0x018C67CD.wav", "0x018C67CD", "suspect_wasted"); } }
        public static ScannerFile HASH0A0FB8D4 { get { return new ScannerFile("01_suspect_wasted\\0x0A0FB8D4.wav", "0x0A0FB8D4", "suspect_wasted"); } }
        public static ScannerFile HASH129A89EB { get { return new ScannerFile("01_suspect_wasted\\0x129A89EB.wav", "0x129A89EB", "suspect_wasted"); } }
        public static ScannerFile HASH1BEBDC93 { get { return new ScannerFile("01_suspect_wasted\\0x1BEBDC93.wav", "0x1BEBDC93", "suspect_wasted"); } }
    }
    public class tattoos
    {
        public static ScannerFile HASH005473A0 { get { return new ScannerFile("01_tattoos\\0x005473A0.wav", "0x005473A0", "tattoos"); } }
        public static ScannerFile HASH05A8B900 { get { return new ScannerFile("01_tattoos\\0x05A8B900.wav", "0x05A8B900", "tattoos"); } }
        public static ScannerFile HASH06AB8D05 { get { return new ScannerFile("01_tattoos\\0x06AB8D05.wav", "0x06AB8D05", "tattoos"); } }
        public static ScannerFile HASH0B67567C { get { return new ScannerFile("01_tattoos\\0x0B67567C.wav", "0x0B67567C", "tattoos"); } }
        public static ScannerFile HASH102C7332 { get { return new ScannerFile("01_tattoos\\0x102C7332.wav", "0x102C7332", "tattoos"); } }
        public static ScannerFile HASH1989F2C1 { get { return new ScannerFile("01_tattoos\\0x1989F2C1.wav", "0x1989F2C1", "tattoos"); } }
        public static ScannerFile HASH1A23673D { get { return new ScannerFile("01_tattoos\\0x1A23673D.wav", "0x1A23673D", "tattoos"); } }
        public static ScannerFile HASH1D95AE22 { get { return new ScannerFile("01_tattoos\\0x1D95AE22.wav", "0x1D95AE22", "tattoos"); } }
        public static ScannerFile HASH1F2CD12F { get { return new ScannerFile("01_tattoos\\0x1F2CD12F.wav", "0x1F2CD12F", "tattoos"); } }
    }
    public class unit_responding_code
    {
        public static ScannerFile HASH0236BDBA { get { return new ScannerFile("01_unit_responding_code\\0x0236BDBA.wav", "0x0236BDBA", "unit_responding_code"); } }
        public static ScannerFile HASH04910270 { get { return new ScannerFile("01_unit_responding_code\\0x04910270.wav", "0x04910270", "unit_responding_code"); } }
        public static ScannerFile HASH0A904DB3 { get { return new ScannerFile("01_unit_responding_code\\0x0A904DB3.wav", "0x0A904DB3", "unit_responding_code"); } }
        public static ScannerFile HASH0E77EBB2 { get { return new ScannerFile("01_unit_responding_code\\0x0E77EBB2.wav", "0x0E77EBB2", "unit_responding_code"); } }
        public static ScannerFile HASH0F0F2CE4 { get { return new ScannerFile("01_unit_responding_code\\0x0F0F2CE4.wav", "0x0F0F2CE4", "unit_responding_code"); } }
        public static ScannerFile HASH10165979 { get { return new ScannerFile("01_unit_responding_code\\0x10165979.wav", "0x10165979", "unit_responding_code"); } }
        public static ScannerFile HASH10F970B8 { get { return new ScannerFile("01_unit_responding_code\\0x10F970B8.wav", "0x10F970B8", "unit_responding_code"); } }
        public static ScannerFile HASH16CD66E7 { get { return new ScannerFile("01_unit_responding_code\\0x16CD66E7.wav", "0x16CD66E7", "unit_responding_code"); } }
    }
    public class vehicle_category
    {
        public static ScannerFile ATV { get { return new ScannerFile("01_vehicle_category\\0x005D43A0.wav", "0x005D43A0", "vehicle_category"); } }
        public static ScannerFile APC01 { get { return new ScannerFile("01_vehicle_category\\0x0110CF57.wav", "0x0110CF57", "vehicle_category"); } }
        public static ScannerFile Train01 { get { return new ScannerFile("01_vehicle_category\\0x01426427.wav", "Train", "vehicle_category"); } }
        public static ScannerFile SpinVehicle { get { return new ScannerFile("01_vehicle_category\\0x014D3B35.wav", "0x014D3B35", "vehicle_category"); } }
        public static ScannerFile Bicycle01 { get { return new ScannerFile("01_vehicle_category\\0x01C22EAC.wav", "Bicycle", "vehicle_category"); } }
        public static ScannerFile Boat01 { get { return new ScannerFile("01_vehicle_category\\0x01D004AB.wav", "Boat", "vehicle_category"); } }
        public static ScannerFile StationWagon { get { return new ScannerFile("01_vehicle_category\\0x0216A584.wav", "0x0216A584", "vehicle_category"); } }
        public static ScannerFile APC02 { get { return new ScannerFile("01_vehicle_category\\0x0251C064.wav", "0x0251C064", "vehicle_category"); } }
        public static ScannerFile SemiTruck { get { return new ScannerFile("01_vehicle_category\\0x02917E7A.wav", "0x02917E7A", "vehicle_category"); } }
        public static ScannerFile Motorcycle01 { get { return new ScannerFile("01_vehicle_category\\0x02B37E94.wav", "Motorcycle", "vehicle_category"); } }
        public static ScannerFile HASH031106D2 { get { return new ScannerFile("01_vehicle_category\\0x031106D2.wav", "0x031106D2", "vehicle_category"); } }
        public static ScannerFile Hearse { get { return new ScannerFile("01_vehicle_category\\0x0384A11A.wav", "0x0384A11A", "vehicle_category"); } }
        public static ScannerFile ConstructionVehicle { get { return new ScannerFile("01_vehicle_category\\0x03A60E85.wav", "0x03A60E85", "vehicle_category"); } }
        public static ScannerFile DumpTruck { get { return new ScannerFile("01_vehicle_category\\0x043502FD.wav", "0x043502FD", "vehicle_category"); } }
        public static ScannerFile SailBoat { get { return new ScannerFile("01_vehicle_category\\0x0474F4A1.wav", "0x0474F4A1", "vehicle_category"); } }
        public static ScannerFile MotorizedSkateboard01 { get { return new ScannerFile("01_vehicle_category\\0x0483453B.wav", "Motorized skateboard", "vehicle_category"); } }
        public static ScannerFile HotRod { get { return new ScannerFile("01_vehicle_category\\0x04B923FA.wav", "0x04B923FA", "vehicle_category"); } }
        public static ScannerFile Service01 { get { return new ScannerFile("01_vehicle_category\\0x04DBBC64.wav", "Service Vehicle", "vehicle_category"); } }
        public static ScannerFile GolfCart { get { return new ScannerFile("01_vehicle_category\\0x04EE616F.wav", "0x04EE616F", "vehicle_category"); } }
        public static ScannerFile Convertable01 { get { return new ScannerFile("01_vehicle_category\\0x05163779.wav", "Convertable", "vehicle_category"); } }
        public static ScannerFile PickupTruck { get { return new ScannerFile("01_vehicle_category\\0x05D788EF.wav", "0x05D788EF", "vehicle_category"); } }
        public static ScannerFile Combine { get { return new ScannerFile("01_vehicle_category\\0x0656C643.wav", "0x0656C643", "vehicle_category"); } }
        public static ScannerFile RV { get { return new ScannerFile("01_vehicle_category\\0x06675D11.wav", "0x06675D11", "vehicle_category"); } }
        public static ScannerFile SUV03 { get { return new ScannerFile("01_vehicle_category\\0x06D6D733.wav", "0x06D6D733", "vehicle_category"); } }
        public static ScannerFile OffRoad01 { get { return new ScannerFile("01_vehicle_category\\0x06F2D611.wav", "0x06F2D611", "vehicle_category"); } }
        public static ScannerFile Coupe01 { get { return new ScannerFile("01_vehicle_category\\0x0709DF95.wav", "Coupe", "vehicle_category"); } }
        public static ScannerFile PoliceCar { get { return new ScannerFile("01_vehicle_category\\0x075FBBD8.wav", "0x075FBBD8", "vehicle_category"); } }
        public static ScannerFile RollerSkates { get { return new ScannerFile("01_vehicle_category\\0x07713DE4.wav", "0x07713DE4", "vehicle_category"); } }
        public static ScannerFile Trashmaster { get { return new ScannerFile("01_vehicle_category\\0x0797FA49.wav", "0x0797FA49", "vehicle_category"); } }
        public static ScannerFile ATV01 { get { return new ScannerFile("01_vehicle_category\\0x0808530B.wav", "ATV", "vehicle_category"); } }
        public static ScannerFile DuneBuggy { get { return new ScannerFile("01_vehicle_category\\0x08EE512E.wav", "0x08EE512E", "vehicle_category"); } }
        public static ScannerFile Digger { get { return new ScannerFile("01_vehicle_category\\0x099011AD.wav", "0x099011AD", "vehicle_category"); } }
        public static ScannerFile UtilityVehicle01 { get { return new ScannerFile("01_vehicle_category\\0x0ABDCE8E.wav", "Utility vehicle", "vehicle_category"); } }
        public static ScannerFile MuscleCar01 { get { return new ScannerFile("01_vehicle_category\\0x0B6A5D7E.wav", "Muscle Car", "vehicle_category"); } }
        public static ScannerFile MilitaryHelicopter { get { return new ScannerFile("01_vehicle_category\\0x0BEDDFB5.wav", "0x0BEDDFB5", "vehicle_category"); } }
        public static ScannerFile Limo { get { return new ScannerFile("01_vehicle_category\\0x0C26D3BD.wav", "0x0C26D3BD", "vehicle_category"); } }
        public static ScannerFile Scooter { get { return new ScannerFile("01_vehicle_category\\0x0CD9BE8E.wav", "0x0CD9BE8E", "vehicle_category"); } }
        public static ScannerFile Tank { get { return new ScannerFile("01_vehicle_category\\0x0CDB4A21.wav", "0x0CDB4A21", "vehicle_category"); } }
        public static ScannerFile Craine { get { return new ScannerFile("01_vehicle_category\\0x0D2D3165.wav", "0x0D2D3165", "vehicle_category"); } }
        public static ScannerFile Classic01 { get { return new ScannerFile("01_vehicle_category\\0x0DB9439E.wav", "Classic", "vehicle_category"); } }
        public static ScannerFile Tuner { get { return new ScannerFile("01_vehicle_category\\0x0DB9C91E.wav", "0x0DB9C91E", "vehicle_category"); } }
        public static ScannerFile Troller { get { return new ScannerFile("01_vehicle_category\\0x0E4D13AD.wav", "0x0E4D13AD", "vehicle_category"); } }
        public static ScannerFile Sedan { get { return new ScannerFile("01_vehicle_category\\0x0E63C9AE.wav", "Sedan", "vehicle_category"); } }
        public static ScannerFile Forklift { get { return new ScannerFile("01_vehicle_category\\0x0E826F9B.wav", "0x0E826F9B", "vehicle_category"); } }
        public static ScannerFile TroopTransport { get { return new ScannerFile("01_vehicle_category\\0x0E82768B.wav", "0x0E82768B", "vehicle_category"); } }
        public static ScannerFile IndustrialVehicle01 { get { return new ScannerFile("01_vehicle_category\\0x0E970FAF.wav", "industrail vehicle", "vehicle_category"); } }
        public static ScannerFile CivilianHelicopter { get { return new ScannerFile("01_vehicle_category\\0x0EBF5417.wav", "0x0EBF5417", "vehicle_category"); } }
        public static ScannerFile Moped { get { return new ScannerFile("01_vehicle_category\\0x103174F6.wav", "0x103174F6", "vehicle_category"); } }
        public static ScannerFile FourDoor01 { get { return new ScannerFile("01_vehicle_category\\0x1094F93E.wav", "Four Door", "vehicle_category"); } }
        public static ScannerFile HydroFoil { get { return new ScannerFile("01_vehicle_category\\0x10A9671C.wav", "0x10A9671C", "vehicle_category"); } }
        public static ScannerFile AgriculturalVehicle { get { return new ScannerFile("01_vehicle_category\\0x10E8D195.wav", "0x10E8D195", "vehicle_category"); } }
        public static ScannerFile GoKart { get { return new ScannerFile("01_vehicle_category\\0x10FB6CA3.wav", "0x10FB6CA3", "vehicle_category"); } }
        public static ScannerFile PontoonBoat { get { return new ScannerFile("01_vehicle_category\\0x1156A6A8.wav", "0x1156A6A8", "vehicle_category"); } }
        public static ScannerFile PeopleCarrier { get { return new ScannerFile("01_vehicle_category\\0x11DB655E.wav", "0x11DB655E", "vehicle_category"); } }
        public static ScannerFile MotorBike { get { return new ScannerFile("01_vehicle_category\\0x12A0A60B.wav", "0x12A0A60B", "vehicle_category"); } }
        public static ScannerFile DuneBuggy02 { get { return new ScannerFile("01_vehicle_category\\0x12BF64D1.wav", "0x12BF64D1", "vehicle_category"); } }
        public static ScannerFile TwoDoor01 { get { return new ScannerFile("01_vehicle_category\\0x1310A327.wav", "Two Door", "vehicle_category"); } }
        public static ScannerFile FanBoat { get { return new ScannerFile("01_vehicle_category\\0x131B49C2.wav", "0x131B49C2", "vehicle_category"); } }
        public static ScannerFile SubwayTrain { get { return new ScannerFile("01_vehicle_category\\0x13AFF107.wav", "0x13AFF107", "vehicle_category"); } }
        public static ScannerFile Rollerblades { get { return new ScannerFile("01_vehicle_category\\0x141522F8.wav", "0x141522F8", "vehicle_category"); } }
        public static ScannerFile Skateboard { get { return new ScannerFile("01_vehicle_category\\0x14BBA15E.wav", "0x14BBA15E", "vehicle_category"); } }
        public static ScannerFile FourByFour { get { return new ScannerFile("01_vehicle_category\\0x14D0951B.wav", "0x14D0951B", "vehicle_category"); } }
        public static ScannerFile Coupe02 { get { return new ScannerFile("01_vehicle_category\\0x14DE7B3E.wav", "Coupe", "vehicle_category"); } }
        public static ScannerFile SpeedBoat { get { return new ScannerFile("01_vehicle_category\\0x14EC0C45.wav", "0x14EC0C45", "vehicle_category"); } }
        public static ScannerFile MonsterTruck { get { return new ScannerFile("01_vehicle_category\\0x151A5E56.wav", "0x151A5E56", "vehicle_category"); } }
        public static ScannerFile MiniVan01 { get { return new ScannerFile("01_vehicle_category\\0x15349899.wav", "Minivan", "vehicle_category"); } }
        public static ScannerFile BackHoe { get { return new ScannerFile("01_vehicle_category\\0x1576E873.wav", "0x1576E873", "vehicle_category"); } }
        public static ScannerFile Van01 { get { return new ScannerFile("01_vehicle_category\\0x158B8FA5.wav", "Van", "vehicle_category"); } }
        public static ScannerFile JetSki { get { return new ScannerFile("01_vehicle_category\\0x1599B926.wav", "0x1599B926", "vehicle_category"); } }
        public static ScannerFile Hatchback { get { return new ScannerFile("01_vehicle_category\\0x160B9ADB.wav", "0x160B9ADB", "vehicle_category"); } }
        public static ScannerFile ArmoredTruck { get { return new ScannerFile("01_vehicle_category\\0x1620A2BC.wav", "0x1620A2BC", "vehicle_category"); } }
        public static ScannerFile Helicopter01 { get { return new ScannerFile("01_vehicle_category\\0x177E286D.wav", "Helicopter", "vehicle_category"); } }
        public static ScannerFile Pickup { get { return new ScannerFile("01_vehicle_category\\0x18092D52.wav", "0x18092D52", "vehicle_category"); } }
        public static ScannerFile SUV01 { get { return new ScannerFile("01_vehicle_category\\0x18101511.wav", "Sports Utility Vehicle", "vehicle_category"); } }
        public static ScannerFile Jallopy { get { return new ScannerFile("01_vehicle_category\\0x1831F800.wav", "0x1831F800", "vehicle_category"); } }
        public static ScannerFile Rig01 { get { return new ScannerFile("01_vehicle_category\\0x188168A9.wav", "Rig", "vehicle_category"); } }
        public static ScannerFile Yacht { get { return new ScannerFile("01_vehicle_category\\0x18BFD1A3.wav", "0x18BFD1A3", "vehicle_category"); } }
        public static ScannerFile PoliceSedan01 { get { return new ScannerFile("01_vehicle_category\\0x1919DF4D.wav", "Police Sedan", "vehicle_category"); } }
        public static ScannerFile QuadBike { get { return new ScannerFile("01_vehicle_category\\0x19F7C1E2.wav", "0x19F7C1E2", "vehicle_category"); } }
        public static ScannerFile Junker { get { return new ScannerFile("01_vehicle_category\\0x1A159577.wav", "0x1A159577", "vehicle_category"); } }
        public static ScannerFile Limo02 { get { return new ScannerFile("01_vehicle_category\\0x1A8BB088.wav", "0x1A8BB088", "vehicle_category"); } }
        public static ScannerFile Trackter { get { return new ScannerFile("01_vehicle_category\\0x1A991863.wav", "0x1A991863", "vehicle_category"); } }
        public static ScannerFile SportsCar01 { get { return new ScannerFile("01_vehicle_category\\0x1AEBB6E8.wav", "Sports car", "vehicle_category"); } }
        public static ScannerFile RV02 { get { return new ScannerFile("01_vehicle_category\\0x1B1A8683.wav", "0x1B1A8683", "vehicle_category"); } }
        public static ScannerFile BeachBuggy { get { return new ScannerFile("01_vehicle_category\\0x1C381231.wav", "0x1C381231", "vehicle_category"); } }
        public static ScannerFile FarmVehicle { get { return new ScannerFile("01_vehicle_category\\0x1C9760CF.wav", "0x1C9760CF", "vehicle_category"); } }
        public static ScannerFile Bus { get { return new ScannerFile("01_vehicle_category\\0x1CA6D8EC.wav", "0x1CA6D8EC", "vehicle_category"); } }
        public static ScannerFile PerformanceCar01 { get { return new ScannerFile("01_vehicle_category\\0x1D525CE1.wav", "Performance car", "vehicle_category"); } }
        public static ScannerFile Bulldozer { get { return new ScannerFile("01_vehicle_category\\0x1D8877D8.wav", "Bulldozer", "vehicle_category"); } }
        public static ScannerFile TroopTransport02 { get { return new ScannerFile("01_vehicle_category\\0x1DBED504.wav", "0x1DBED504", "vehicle_category"); } }
        public static ScannerFile MxBike { get { return new ScannerFile("01_vehicle_category\\0x1DBFB566.wav", "0x1DBFB566", "vehicle_category"); } }
        public static ScannerFile MetroTrain { get { return new ScannerFile("01_vehicle_category\\0x1DCAAC3C.wav", "0x1DCAAC3C", "vehicle_category"); } }
        public static ScannerFile FreightTrain { get { return new ScannerFile("01_vehicle_category\\0x1E24FC04.wav", "0x1E24FC04", "vehicle_category"); } }
        public static ScannerFile PoliceMotorcycle { get { return new ScannerFile("01_vehicle_category\\0x1E8CEE5F.wav", "0x1E8CEE5F", "vehicle_category"); } }
        public static ScannerFile Bike01 { get { return new ScannerFile("01_vehicle_category\\0x1FCD9DA9.wav", "Bike", "vehicle_category"); } }
        public static ScannerFile Truck { get { return new ScannerFile("01_vehicle_category\\0x1FD2B13F.wav", "0x1FD2B13F", "vehicle_category"); } }
    }
    public class we_have
    {
        public static ScannerFile UnitsReport_1 { get { return new ScannerFile("01_we_have\\0x03E3086B.wav", "0x03E3086B", "we_have"); } }
        public static ScannerFile We_Have_1 { get { return new ScannerFile("01_we_have\\0x0662DE99.wav", "0x0662DE99", "we_have"); } }
        public static ScannerFile CitizensReport_4 { get { return new ScannerFile("01_we_have\\0x07F7F36D.wav", "0x07F7F36D", "we_have"); } }
        public static ScannerFile CitizensReport_1 { get { return new ScannerFile("01_we_have\\0x0ACF391D.wav", "0x0ACF391D", "we_have"); } }
        public static ScannerFile OfficersReport_2 { get { return new ScannerFile("01_we_have\\0x128465AD.wav", "0x128465AD", "we_have"); } }
        public static ScannerFile OfficersReport_1 { get { return new ScannerFile("01_we_have\\0x1628ACF6.wav", "0x1628ACF6", "we_have"); } }
        public static ScannerFile We_Have_2 { get { return new ScannerFile("01_we_have\\0x1823821B.wav", "0x1823821B", "we_have"); } }
        public static ScannerFile CitizensReport_2 { get { return new ScannerFile("01_we_have\\0x1D1D5DBA.wav", "0x1D1D5DBA", "we_have"); } }
        public static ScannerFile CitizensReport_3 { get { return new ScannerFile("01_we_have\\0x1F72E264.wav", "0x1F72E264", "we_have"); } }
    }
    public class year
    {
        public static ScannerFile HASH0033BF81 { get { return new ScannerFile("01_year\\0x0033BF81.wav", "0x0033BF81", "year"); } }
        public static ScannerFile HASH00B67763 { get { return new ScannerFile("01_year\\0x00B67763.wav", "0x00B67763", "year"); } }
        public static ScannerFile HASH02B30383 { get { return new ScannerFile("01_year\\0x02B30383.wav", "0x02B30383", "year"); } }
        public static ScannerFile HASH036B8084 { get { return new ScannerFile("01_year\\0x036B8084.wav", "0x036B8084", "year"); } }
        public static ScannerFile HASH05C709B8 { get { return new ScannerFile("01_year\\0x05C709B8.wav", "0x05C709B8", "year"); } }
        public static ScannerFile HASH05FDCB16 { get { return new ScannerFile("01_year\\0x05FDCB16.wav", "0x05FDCB16", "year"); } }
        public static ScannerFile HASH0708009C { get { return new ScannerFile("01_year\\0x0708009C.wav", "0x0708009C", "year"); } }
        public static ScannerFile HASH070EFC25 { get { return new ScannerFile("01_year\\0x070EFC25.wav", "0x070EFC25", "year"); } }
        public static ScannerFile HASH0761B07A { get { return new ScannerFile("01_year\\0x0761B07A.wav", "0x0761B07A", "year"); } }
        public static ScannerFile HASH0821C9EF { get { return new ScannerFile("01_year\\0x0821C9EF.wav", "0x0821C9EF", "year"); } }
        public static ScannerFile HASH09AC8E8E { get { return new ScannerFile("01_year\\0x09AC8E8E.wav", "0x09AC8E8E", "year"); } }
        public static ScannerFile HASH0D1F985B { get { return new ScannerFile("01_year\\0x0D1F985B.wav", "0x0D1F985B", "year"); } }
        public static ScannerFile HASH0D68947F { get { return new ScannerFile("01_year\\0x0D68947F.wav", "0x0D68947F", "year"); } }
        public static ScannerFile HASH0F6B74E6 { get { return new ScannerFile("01_year\\0x0F6B74E6.wav", "0x0F6B74E6", "year"); } }
        public static ScannerFile HASH1088ED93 { get { return new ScannerFile("01_year\\0x1088ED93.wav", "0x1088ED93", "year"); } }
        public static ScannerFile HASH147616F3 { get { return new ScannerFile("01_year\\0x147616F3.wav", "0x147616F3", "year"); } }
        public static ScannerFile HASH1567247B { get { return new ScannerFile("01_year\\0x1567247B.wav", "0x1567247B", "year"); } }
        public static ScannerFile HASH15A77F9F { get { return new ScannerFile("01_year\\0x15A77F9F.wav", "0x15A77F9F", "year"); } }
        public static ScannerFile HASH166862C8 { get { return new ScannerFile("01_year\\0x166862C8.wav", "0x166862C8", "year"); } }
        public static ScannerFile HASH16A38189 { get { return new ScannerFile("01_year\\0x16A38189.wav", "0x16A38189", "year"); } }
        public static ScannerFile HASH17816D2D { get { return new ScannerFile("01_year\\0x17816D2D.wav", "0x17816D2D", "year"); } }
        public static ScannerFile HASH186D2F8B { get { return new ScannerFile("01_year\\0x186D2F8B.wav", "0x186D2F8B", "year"); } }
        public static ScannerFile HASH19B0C108 { get { return new ScannerFile("01_year\\0x19B0C108.wav", "0x19B0C108", "year"); } }
        public static ScannerFile HASH19C7D51C { get { return new ScannerFile("01_year\\0x19C7D51C.wav", "0x19C7D51C", "year"); } }
        public static ScannerFile HASH1B24717A { get { return new ScannerFile("01_year\\0x1B24717A.wav", "0x1B24717A", "year"); } }
        public static ScannerFile HASH1BCE8538 { get { return new ScannerFile("01_year\\0x1BCE8538.wav", "0x1BCE8538", "year"); } }
        public static ScannerFile HASH1D1B1043 { get { return new ScannerFile("01_year\\0x1D1B1043.wav", "0x1D1B1043", "year"); } }
        public static ScannerFile HASH1D979CE6 { get { return new ScannerFile("01_year\\0x1D979CE6.wav", "0x1D979CE6", "year"); } }
        public static ScannerFile HASH1E4E491E { get { return new ScannerFile("01_year\\0x1E4E491E.wav", "0x1E4E491E", "year"); } }
        public static ScannerFile HASH1EA0DEF4 { get { return new ScannerFile("01_year\\0x1EA0DEF4.wav", "0x1EA0DEF4", "year"); } }
        public static ScannerFile HASH1EC83C50 { get { return new ScannerFile("01_year\\0x1EC83C50.wav", "0x1EC83C50", "year"); } }
    }
    public class random_chat
    {
        public static ScannerFile RANDOMCHAT1 { get { return new ScannerFile("random_chat\\RANDOMCHAT1.wav", "RANDOMCHAT1", "random_chat"); } }
        public static ScannerFile RANDOMCHAT2 { get { return new ScannerFile("random_chat\\RANDOMCHAT2.wav", "RANDOMCHAT2", "random_chat"); } }
        public static ScannerFile RANDOMCHAT3 { get { return new ScannerFile("random_chat\\RANDOMCHAT3.wav", "RANDOMCHAT3", "random_chat"); } }
        public static ScannerFile RANDOMCHAT5 { get { return new ScannerFile("random_chat\\RANDOMCHAT5.wav", "RANDOMCHAT5", "random_chat"); } }
        public static ScannerFile RANDOMCHAT6 { get { return new ScannerFile("random_chat\\RANDOMCHAT6.wav", "RANDOMCHAT6", "random_chat"); } }
        public static ScannerFile RANDOMCHAT7 { get { return new ScannerFile("random_chat\\RANDOMCHAT7.wav", "RANDOMCHAT7", "random_chat"); } }
        public static ScannerFile RANDOMCHAT8 { get { return new ScannerFile("random_chat\\RANDOMCHAT8.wav", "RANDOMCHAT8", "random_chat"); } }
    }
    public class spot_suspect_cop_01
    {
        public static ScannerFile HASH0601EE8E { get { return new ScannerFile("spot_suspect_cop_01\\0x0601EE8E.wav", "0x0601EE8E", "spot_suspect_cop_01"); } }
        public static ScannerFile HASH06A36FCF { get { return new ScannerFile("spot_suspect_cop_01\\0x06A36FCF.wav", "0x06A36FCF", "spot_suspect_cop_01"); } }
        public static ScannerFile HASH08E3F451 { get { return new ScannerFile("spot_suspect_cop_01\\0x08E3F451.wav", "0x08E3F451", "spot_suspect_cop_01"); } }
        public static ScannerFile HASH0C703B6A { get { return new ScannerFile("spot_suspect_cop_01\\0x0C703B6A.wav", "0x0C703B6A", "spot_suspect_cop_01"); } }
        public static ScannerFile HASH13478918 { get { return new ScannerFile("spot_suspect_cop_01\\0x13478918.wav", "0x13478918", "spot_suspect_cop_01"); } }
        public static ScannerFile HASH17551134 { get { return new ScannerFile("spot_suspect_cop_01\\0x17551134.wav", "0x17551134", "spot_suspect_cop_01"); } }
        public static ScannerFile HASH1A3056EA { get { return new ScannerFile("spot_suspect_cop_01\\0x1A3056EA.wav", "0x1A3056EA", "spot_suspect_cop_01"); } }
        public static ScannerFile HASH1B3A58FF { get { return new ScannerFile("spot_suspect_cop_01\\0x1B3A58FF.wav", "0x1B3A58FF", "spot_suspect_cop_01"); } }
    }
    public class spot_suspect_cop_02
    {
        public static ScannerFile HASH06A36FCF { get { return new ScannerFile("spot_suspect_cop_02\\0x06A36FCF.wav", "0x06A36FCF", "spot_suspect_cop_02"); } }
        public static ScannerFile HASH08E3F451 { get { return new ScannerFile("spot_suspect_cop_02\\0x08E3F451.wav", "0x08E3F451", "spot_suspect_cop_02"); } }
        public static ScannerFile HASH0C703B6A { get { return new ScannerFile("spot_suspect_cop_02\\0x0C703B6A.wav", "0x0C703B6A", "spot_suspect_cop_02"); } }
        public static ScannerFile HASH13478918 { get { return new ScannerFile("spot_suspect_cop_02\\0x13478918.wav", "0x13478918", "spot_suspect_cop_02"); } }
        public static ScannerFile HASH17551134 { get { return new ScannerFile("spot_suspect_cop_02\\0x17551134.wav", "0x17551134", "spot_suspect_cop_02"); } }
        public static ScannerFile HASH1A3056EA { get { return new ScannerFile("spot_suspect_cop_02\\0x1A3056EA.wav", "0x1A3056EA", "spot_suspect_cop_02"); } }
        public static ScannerFile HASH1B3A58FF { get { return new ScannerFile("spot_suspect_cop_02\\0x1B3A58FF.wav", "0x1B3A58FF", "spot_suspect_cop_02"); } }
    }
    public class spot_suspect_cop_03
    {
        public static ScannerFile HASH0601EE8E { get { return new ScannerFile("spot_suspect_cop_03\\0x0601EE8E.wav", "0x0601EE8E", "spot_suspect_cop_03"); } }
        public static ScannerFile HASH06A36FCF { get { return new ScannerFile("spot_suspect_cop_03\\0x06A36FCF.wav", "0x06A36FCF", "spot_suspect_cop_03"); } }
        public static ScannerFile HASH08E3F451 { get { return new ScannerFile("spot_suspect_cop_03\\0x08E3F451.wav", "0x08E3F451", "spot_suspect_cop_03"); } }
        public static ScannerFile HASH0C703B6A { get { return new ScannerFile("spot_suspect_cop_03\\0x0C703B6A.wav", "0x0C703B6A", "spot_suspect_cop_03"); } }
        public static ScannerFile HASH13478918 { get { return new ScannerFile("spot_suspect_cop_03\\0x13478918.wav", "0x13478918", "spot_suspect_cop_03"); } }
        public static ScannerFile HASH17551134 { get { return new ScannerFile("spot_suspect_cop_03\\0x17551134.wav", "0x17551134", "spot_suspect_cop_03"); } }
        public static ScannerFile HASH1A3056EA { get { return new ScannerFile("spot_suspect_cop_03\\0x1A3056EA.wav", "0x1A3056EA", "spot_suspect_cop_03"); } }
        public static ScannerFile HASH1B3A58FF { get { return new ScannerFile("spot_suspect_cop_03\\0x1B3A58FF.wav", "0x1B3A58FF", "spot_suspect_cop_03"); } }
    }
    public class spot_suspect_cop_04
    {
        public static ScannerFile HASH0601EE8E { get { return new ScannerFile("spot_suspect_cop_04\\0x0601EE8E.wav", "0x0601EE8E", "spot_suspect_cop_04"); } }
        public static ScannerFile HASH06A36FCF { get { return new ScannerFile("spot_suspect_cop_04\\0x06A36FCF.wav", "0x06A36FCF", "spot_suspect_cop_04"); } }
        public static ScannerFile HASH08E3F451 { get { return new ScannerFile("spot_suspect_cop_04\\0x08E3F451.wav", "0x08E3F451", "spot_suspect_cop_04"); } }
        public static ScannerFile HASH0C703B6A { get { return new ScannerFile("spot_suspect_cop_04\\0x0C703B6A.wav", "0x0C703B6A", "spot_suspect_cop_04"); } }
        public static ScannerFile HASH13478918 { get { return new ScannerFile("spot_suspect_cop_04\\0x13478918.wav", "0x13478918", "spot_suspect_cop_04"); } }
        public static ScannerFile HASH17551134 { get { return new ScannerFile("spot_suspect_cop_04\\0x17551134.wav", "0x17551134", "spot_suspect_cop_04"); } }
        public static ScannerFile HASH1A3056EA { get { return new ScannerFile("spot_suspect_cop_04\\0x1A3056EA.wav", "0x1A3056EA", "spot_suspect_cop_04"); } }
        public static ScannerFile HASH1B3A58FF { get { return new ScannerFile("spot_suspect_cop_04\\0x1B3A58FF.wav", "0x1B3A58FF", "spot_suspect_cop_04"); } }
    }
    public class spot_suspect_cop_05
    {
        public static ScannerFile HASH06A36FCF { get { return new ScannerFile("spot_suspect_cop_05\\0x06A36FCF.wav", "0x06A36FCF", "spot_suspect_cop_05"); } }
        public static ScannerFile HASH08E3F451 { get { return new ScannerFile("spot_suspect_cop_05\\0x08E3F451.wav", "0x08E3F451", "spot_suspect_cop_05"); } }
        public static ScannerFile HASH0C703B6A { get { return new ScannerFile("spot_suspect_cop_05\\0x0C703B6A.wav", "0x0C703B6A", "spot_suspect_cop_05"); } }
        public static ScannerFile HASH13478918 { get { return new ScannerFile("spot_suspect_cop_05\\0x13478918.wav", "0x13478918", "spot_suspect_cop_05"); } }
        public static ScannerFile HASH17551134 { get { return new ScannerFile("spot_suspect_cop_05\\0x17551134.wav", "0x17551134", "spot_suspect_cop_05"); } }
        public static ScannerFile HASH1A3056EA { get { return new ScannerFile("spot_suspect_cop_05\\0x1A3056EA.wav", "0x1A3056EA", "spot_suspect_cop_05"); } }
        public static ScannerFile HASH1B3A58FF { get { return new ScannerFile("spot_suspect_cop_05\\0x1B3A58FF.wav", "0x1B3A58FF", "spot_suspect_cop_05"); } }
    }
    public class s_f_y_cop_black_full_01
    {
        public static ScannerFile HASH00FFC2DB { get { return new ScannerFile("s_f_y_cop_01_black_full_01\\0x00FFC2DB.wav", "0x00FFC2DB", "s_f_y_cop_black_full_01"); } }
        public static ScannerFile HASH017221AA { get { return new ScannerFile("s_f_y_cop_01_black_full_01\\0x017221AA.wav", "0x017221AA", "s_f_y_cop_black_full_01"); } }
        public static ScannerFile HASH01A10324 { get { return new ScannerFile("s_f_y_cop_01_black_full_01\\0x01A10324.wav", "0x01A10324", "s_f_y_cop_black_full_01"); } }
        public static ScannerFile HASH0846F27E { get { return new ScannerFile("s_f_y_cop_01_black_full_01\\0x0846F27E.wav", "0x0846F27E", "s_f_y_cop_black_full_01"); } }
        public static ScannerFile HASH0AF015F7 { get { return new ScannerFile("s_f_y_cop_01_black_full_01\\0x0AF015F7.wav", "0x0AF015F7", "s_f_y_cop_black_full_01"); } }
        public static ScannerFile HASH0D3B44AE { get { return new ScannerFile("s_f_y_cop_01_black_full_01\\0x0D3B44AE.wav", "0x0D3B44AE", "s_f_y_cop_black_full_01"); } }
        public static ScannerFile HASH0D59A0C8 { get { return new ScannerFile("s_f_y_cop_01_black_full_01\\0x0D59A0C8.wav", "0x0D59A0C8", "s_f_y_cop_black_full_01"); } }
        public static ScannerFile HASH0ED8BFA8 { get { return new ScannerFile("s_f_y_cop_01_black_full_01\\0x0ED8BFA8.wav", "0x0ED8BFA8", "s_f_y_cop_black_full_01"); } }
        public static ScannerFile HASH0FD39C10 { get { return new ScannerFile("s_f_y_cop_01_black_full_01\\0x0FD39C10.wav", "0x0FD39C10", "s_f_y_cop_black_full_01"); } }
        public static ScannerFile HASH1046DF8A { get { return new ScannerFile("s_f_y_cop_01_black_full_01\\0x1046DF8A.wav", "0x1046DF8A", "s_f_y_cop_black_full_01"); } }
        public static ScannerFile HASH137027BC { get { return new ScannerFile("s_f_y_cop_01_black_full_01\\0x137027BC.wav", "0x137027BC", "s_f_y_cop_black_full_01"); } }
        public static ScannerFile HASH144A2876 { get { return new ScannerFile("s_f_y_cop_01_black_full_01\\0x144A2876.wav", "0x144A2876", "s_f_y_cop_black_full_01"); } }
        public static ScannerFile HASH1AF6E025 { get { return new ScannerFile("s_f_y_cop_01_black_full_01\\0x1AF6E025.wav", "0x1AF6E025", "s_f_y_cop_black_full_01"); } }
        public static ScannerFile HASH1BD2FDBA { get { return new ScannerFile("s_f_y_cop_01_black_full_01\\0x1BD2FDBA.wav", "0x1BD2FDBA", "s_f_y_cop_black_full_01"); } }
    }
    public class s_f_y_cop_black_full_02
    {
        public static ScannerFile HASH00FFC2DB { get { return new ScannerFile("s_f_y_cop_01_black_full_02\\0x00FFC2DB.wav", "0x00FFC2DB", "s_f_y_cop_black_full_02"); } }
        public static ScannerFile HASH017221AA { get { return new ScannerFile("s_f_y_cop_01_black_full_02\\0x017221AA.wav", "0x017221AA", "s_f_y_cop_black_full_02"); } }
        public static ScannerFile HASH01A10324 { get { return new ScannerFile("s_f_y_cop_01_black_full_02\\0x01A10324.wav", "0x01A10324", "s_f_y_cop_black_full_02"); } }
        public static ScannerFile HASH0846F27E { get { return new ScannerFile("s_f_y_cop_01_black_full_02\\0x0846F27E.wav", "0x0846F27E", "s_f_y_cop_black_full_02"); } }
        public static ScannerFile HASH0AF015F7 { get { return new ScannerFile("s_f_y_cop_01_black_full_02\\0x0AF015F7.wav", "0x0AF015F7", "s_f_y_cop_black_full_02"); } }
        public static ScannerFile HASH0D3B44AE { get { return new ScannerFile("s_f_y_cop_01_black_full_02\\0x0D3B44AE.wav", "0x0D3B44AE", "s_f_y_cop_black_full_02"); } }
        public static ScannerFile HASH0D59A0C8 { get { return new ScannerFile("s_f_y_cop_01_black_full_02\\0x0D59A0C8.wav", "0x0D59A0C8", "s_f_y_cop_black_full_02"); } }
        public static ScannerFile HASH0ED8BFA8 { get { return new ScannerFile("s_f_y_cop_01_black_full_02\\0x0ED8BFA8.wav", "0x0ED8BFA8", "s_f_y_cop_black_full_02"); } }
        public static ScannerFile HASH0FD39C10 { get { return new ScannerFile("s_f_y_cop_01_black_full_02\\0x0FD39C10.wav", "0x0FD39C10", "s_f_y_cop_black_full_02"); } }
        public static ScannerFile HASH1046DF8A { get { return new ScannerFile("s_f_y_cop_01_black_full_02\\0x1046DF8A.wav", "0x1046DF8A", "s_f_y_cop_black_full_02"); } }
        public static ScannerFile HASH137027BC { get { return new ScannerFile("s_f_y_cop_01_black_full_02\\0x137027BC.wav", "0x137027BC", "s_f_y_cop_black_full_02"); } }
        public static ScannerFile HASH144A2876 { get { return new ScannerFile("s_f_y_cop_01_black_full_02\\0x144A2876.wav", "0x144A2876", "s_f_y_cop_black_full_02"); } }
        public static ScannerFile HASH1AF6E025 { get { return new ScannerFile("s_f_y_cop_01_black_full_02\\0x1AF6E025.wav", "0x1AF6E025", "s_f_y_cop_black_full_02"); } }
        public static ScannerFile HASH1B2B551D { get { return new ScannerFile("s_f_y_cop_01_black_full_02\\0x1B2B551D.wav", "0x1B2B551D", "s_f_y_cop_black_full_02"); } }
        public static ScannerFile HASH1BD2FDBA { get { return new ScannerFile("s_f_y_cop_01_black_full_02\\0x1BD2FDBA.wav", "0x1BD2FDBA", "s_f_y_cop_black_full_02"); } }
    }
    public class s_f_y_cop_white_full_01
    {
        public static ScannerFile HASH00FFC2DB { get { return new ScannerFile("s_f_y_cop_01_white_full_01\\0x00FFC2DB.wav", "0x00FFC2DB", "s_f_y_cop_white_full_01"); } }
        public static ScannerFile HASH017221AA { get { return new ScannerFile("s_f_y_cop_01_white_full_01\\0x017221AA.wav", "0x017221AA", "s_f_y_cop_white_full_01"); } }
        public static ScannerFile HASH01A10324 { get { return new ScannerFile("s_f_y_cop_01_white_full_01\\0x01A10324.wav", "0x01A10324", "s_f_y_cop_white_full_01"); } }
        public static ScannerFile HASH0846F27E { get { return new ScannerFile("s_f_y_cop_01_white_full_01\\0x0846F27E.wav", "0x0846F27E", "s_f_y_cop_white_full_01"); } }
        public static ScannerFile HASH0AF015F7 { get { return new ScannerFile("s_f_y_cop_01_white_full_01\\0x0AF015F7.wav", "0x0AF015F7", "s_f_y_cop_white_full_01"); } }
        public static ScannerFile HASH0D3B44AE { get { return new ScannerFile("s_f_y_cop_01_white_full_01\\0x0D3B44AE.wav", "0x0D3B44AE", "s_f_y_cop_white_full_01"); } }
        public static ScannerFile HASH0D59A0C8 { get { return new ScannerFile("s_f_y_cop_01_white_full_01\\0x0D59A0C8.wav", "0x0D59A0C8", "s_f_y_cop_white_full_01"); } }
        public static ScannerFile HASH0ED8BFA8 { get { return new ScannerFile("s_f_y_cop_01_white_full_01\\0x0ED8BFA8.wav", "0x0ED8BFA8", "s_f_y_cop_white_full_01"); } }
        public static ScannerFile HASH0FD39C10 { get { return new ScannerFile("s_f_y_cop_01_white_full_01\\0x0FD39C10.wav", "0x0FD39C10", "s_f_y_cop_white_full_01"); } }
        public static ScannerFile HASH1046DF8A { get { return new ScannerFile("s_f_y_cop_01_white_full_01\\0x1046DF8A.wav", "0x1046DF8A", "s_f_y_cop_white_full_01"); } }
        public static ScannerFile HASH137027BC { get { return new ScannerFile("s_f_y_cop_01_white_full_01\\0x137027BC.wav", "0x137027BC", "s_f_y_cop_white_full_01"); } }
        public static ScannerFile HASH144A2876 { get { return new ScannerFile("s_f_y_cop_01_white_full_01\\0x144A2876.wav", "0x144A2876", "s_f_y_cop_white_full_01"); } }
        public static ScannerFile HASH1AF6E025 { get { return new ScannerFile("s_f_y_cop_01_white_full_01\\0x1AF6E025.wav", "0x1AF6E025", "s_f_y_cop_white_full_01"); } }
        public static ScannerFile HASH1B2B551D { get { return new ScannerFile("s_f_y_cop_01_white_full_01\\0x1B2B551D.wav", "0x1B2B551D", "s_f_y_cop_white_full_01"); } }
        public static ScannerFile HASH1BD2FDBA { get { return new ScannerFile("s_f_y_cop_01_white_full_01\\0x1BD2FDBA.wav", "0x1BD2FDBA", "s_f_y_cop_white_full_01"); } }
    }
    public class s_f_y_cop_white_full_02
    {
        public static ScannerFile HASH00FFC2DB { get { return new ScannerFile("s_f_y_cop_01_white_full_02\\0x00FFC2DB.wav", "0x00FFC2DB", "s_f_y_cop_white_full_02"); } }
        public static ScannerFile HASH017221AA { get { return new ScannerFile("s_f_y_cop_01_white_full_02\\0x017221AA.wav", "0x017221AA", "s_f_y_cop_white_full_02"); } }
        public static ScannerFile HASH01A10324 { get { return new ScannerFile("s_f_y_cop_01_white_full_02\\0x01A10324.wav", "0x01A10324", "s_f_y_cop_white_full_02"); } }
        public static ScannerFile HASH0846F27E { get { return new ScannerFile("s_f_y_cop_01_white_full_02\\0x0846F27E.wav", "0x0846F27E", "s_f_y_cop_white_full_02"); } }
        public static ScannerFile HASH0AF015F7 { get { return new ScannerFile("s_f_y_cop_01_white_full_02\\0x0AF015F7.wav", "0x0AF015F7", "s_f_y_cop_white_full_02"); } }
        public static ScannerFile HASH0D3B44AE { get { return new ScannerFile("s_f_y_cop_01_white_full_02\\0x0D3B44AE.wav", "0x0D3B44AE", "s_f_y_cop_white_full_02"); } }
        public static ScannerFile HASH0D59A0C8 { get { return new ScannerFile("s_f_y_cop_01_white_full_02\\0x0D59A0C8.wav", "0x0D59A0C8", "s_f_y_cop_white_full_02"); } }
        public static ScannerFile HASH0ED8BFA8 { get { return new ScannerFile("s_f_y_cop_01_white_full_02\\0x0ED8BFA8.wav", "0x0ED8BFA8", "s_f_y_cop_white_full_02"); } }
        public static ScannerFile HASH0FD39C10 { get { return new ScannerFile("s_f_y_cop_01_white_full_02\\0x0FD39C10.wav", "0x0FD39C10", "s_f_y_cop_white_full_02"); } }
        public static ScannerFile HASH1046DF8A { get { return new ScannerFile("s_f_y_cop_01_white_full_02\\0x1046DF8A.wav", "0x1046DF8A", "s_f_y_cop_white_full_02"); } }
        public static ScannerFile HASH137027BC { get { return new ScannerFile("s_f_y_cop_01_white_full_02\\0x137027BC.wav", "0x137027BC", "s_f_y_cop_white_full_02"); } }
        public static ScannerFile HASH144A2876 { get { return new ScannerFile("s_f_y_cop_01_white_full_02\\0x144A2876.wav", "0x144A2876", "s_f_y_cop_white_full_02"); } }
        public static ScannerFile HASH1AF6E025 { get { return new ScannerFile("s_f_y_cop_01_white_full_02\\0x1AF6E025.wav", "0x1AF6E025", "s_f_y_cop_white_full_02"); } }
        public static ScannerFile HASH1B2B551D { get { return new ScannerFile("s_f_y_cop_01_white_full_02\\0x1B2B551D.wav", "0x1B2B551D", "s_f_y_cop_white_full_02"); } }
        public static ScannerFile HASH1BD2FDBA { get { return new ScannerFile("s_f_y_cop_01_white_full_02\\0x1BD2FDBA.wav", "0x1BD2FDBA", "s_f_y_cop_white_full_02"); } }
    }
    public class s_m_y_cop_black_full_01
    {
        public static ScannerFile HASH00FFC2DB { get { return new ScannerFile("s_m_y_cop_01_black_full_01\\0x00FFC2DB.wav", "0x00FFC2DB", "s_m_y_cop_black_full_01"); } }
        public static ScannerFile HASH017221AA { get { return new ScannerFile("s_m_y_cop_01_black_full_01\\0x017221AA.wav", "0x017221AA", "s_m_y_cop_black_full_01"); } }
        public static ScannerFile HASH01A10324 { get { return new ScannerFile("s_m_y_cop_01_black_full_01\\0x01A10324.wav", "0x01A10324", "s_m_y_cop_black_full_01"); } }
        public static ScannerFile SuspectsVehicleHasCrashed { get { return new ScannerFile("s_m_y_cop_01_black_full_01\\0x0846F27E.wav", "0x0846F27E", "s_m_y_cop_black_full_01"); } }
        public static ScannerFile HASH0AF015F7 { get { return new ScannerFile("s_m_y_cop_01_black_full_01\\0x0AF015F7.wav", "0x0AF015F7", "s_m_y_cop_black_full_01"); } }
        public static ScannerFile HASH0D3B44AE { get { return new ScannerFile("s_m_y_cop_01_black_full_01\\0x0D3B44AE.wav", "0x0D3B44AE", "s_m_y_cop_black_full_01"); } }
        public static ScannerFile HASH0D59A0C8 { get { return new ScannerFile("s_m_y_cop_01_black_full_01\\0x0D59A0C8.wav", "0x0D59A0C8", "s_m_y_cop_black_full_01"); } }
        public static ScannerFile HASH0ED8BFA8 { get { return new ScannerFile("s_m_y_cop_01_black_full_01\\0x0ED8BFA8.wav", "0x0ED8BFA8", "s_m_y_cop_black_full_01"); } }
        public static ScannerFile HASH0FD39C10 { get { return new ScannerFile("s_m_y_cop_01_black_full_01\\0x0FD39C10.wav", "0x0FD39C10", "s_m_y_cop_black_full_01"); } }
        public static ScannerFile HASH1046DF8A { get { return new ScannerFile("s_m_y_cop_01_black_full_01\\0x1046DF8A.wav", "0x1046DF8A", "s_m_y_cop_black_full_01"); } }
        public static ScannerFile HASH137027BC { get { return new ScannerFile("s_m_y_cop_01_black_full_01\\0x137027BC.wav", "0x137027BC", "s_m_y_cop_black_full_01"); } }
        public static ScannerFile HASH144A2876 { get { return new ScannerFile("s_m_y_cop_01_black_full_01\\0x144A2876.wav", "0x144A2876", "s_m_y_cop_black_full_01"); } }
        public static ScannerFile HASH1AF6E025 { get { return new ScannerFile("s_m_y_cop_01_black_full_01\\0x1AF6E025.wav", "0x1AF6E025", "s_m_y_cop_black_full_01"); } }
        public static ScannerFile HASH1B2B551D { get { return new ScannerFile("s_m_y_cop_01_black_full_01\\0x1B2B551D.wav", "0x1B2B551D", "s_m_y_cop_black_full_01"); } }
        public static ScannerFile HASH1BD2FDBA { get { return new ScannerFile("s_m_y_cop_01_black_full_01\\0x1BD2FDBA.wav", "0x1BD2FDBA", "s_m_y_cop_black_full_01"); } }
    }
    public class s_m_y_cop_black_full_02
    {
        public static ScannerFile HASH017221AA { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x017221AA.wav", "0x017221AA", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH01A10324 { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x01A10324.wav", "0x01A10324", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH030276D5 { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x030276D5.wav", "0x030276D5", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH046135A4 { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x046135A4.wav", "0x046135A4", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH0496792A { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x0496792A.wav", "0x0496792A", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH0513FA72 { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x0513FA72.wav", "0x0513FA72", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile WeHaveACollision { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x0846F27E.wav", "0x0846F27E", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH08DB544A { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x08DB544A.wav", "0x08DB544A", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH08E377EE { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x08E377EE.wav", "0x08E377EE", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH09D5A5ED { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x09D5A5ED.wav", "0x09D5A5ED", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH0AF015F7 { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x0AF015F7.wav", "0x0AF015F7", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH0B5B6665 { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x0B5B6665.wav", "0x0B5B6665", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH0B78C3D3 { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x0B78C3D3.wav", "0x0B78C3D3", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH0D3B44AE { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x0D3B44AE.wav", "0x0D3B44AE", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH0D59A0C8 { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x0D59A0C8.wav", "0x0D59A0C8", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH0ED8BFA8 { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x0ED8BFA8.wav", "0x0ED8BFA8", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH0F80B554 { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x0F80B554.wav", "0x0F80B554", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH0FD39C10 { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x0FD39C10.wav", "0x0FD39C10", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH1046DF8A { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x1046DF8A.wav", "0x1046DF8A", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH118A68BD { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x118A68BD.wav", "0x118A68BD", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH12461560 { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x12461560.wav", "0x12461560", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH137027BC { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x137027BC.wav", "0x137027BC", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH144A2876 { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x144A2876.wav", "0x144A2876", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH14C47BCF { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x14C47BCF.wav", "0x14C47BCF", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH16A41369 { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x16A41369.wav", "0x16A41369", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH16D11D9F { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x16D11D9F.wav", "0x16D11D9F", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH1A56373F { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x1A56373F.wav", "0x1A56373F", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH1AAF0500 { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x1AAF0500.wav", "0x1AAF0500", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH1AF6E025 { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x1AF6E025.wav", "0x1AF6E025", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH1B2B551D { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x1B2B551D.wav", "0x1B2B551D", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH1BCC27E6 { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x1BCC27E6.wav", "0x1BCC27E6", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH1BD2FDBA { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x1BD2FDBA.wav", "0x1BD2FDBA", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH1C086912 { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x1C086912.wav", "0x1C086912", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH1E65C2A3 { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x1E65C2A3.wav", "0x1E65C2A3", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH1EB82E73 { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x1EB82E73.wav", "0x1EB82E73", "s_m_y_cop_black_full_02"); } }
        public static ScannerFile HASH1F7C154B { get { return new ScannerFile("s_m_y_cop_01_black_full_02\\0x1F7C154B.wav", "0x1F7C154B", "s_m_y_cop_black_full_02"); } }
    }
    public class s_m_y_cop_black_mini_01
    {
        public static ScannerFile HASH017221AA { get { return new ScannerFile("s_m_y_cop_01_black_mini_01\\0x017221AA.wav", "0x017221AA", "s_m_y_cop_black_mini_01"); } }
        public static ScannerFile HASH030276D5 { get { return new ScannerFile("s_m_y_cop_01_black_mini_01\\0x030276D5.wav", "0x030276D5", "s_m_y_cop_black_mini_01"); } }
        public static ScannerFile HASH046135A4 { get { return new ScannerFile("s_m_y_cop_01_black_mini_01\\0x046135A4.wav", "0x046135A4", "s_m_y_cop_black_mini_01"); } }
        public static ScannerFile HASH0496792A { get { return new ScannerFile("s_m_y_cop_01_black_mini_01\\0x0496792A.wav", "0x0496792A", "s_m_y_cop_black_mini_01"); } }
        public static ScannerFile HASH0513FA72 { get { return new ScannerFile("s_m_y_cop_01_black_mini_01\\0x0513FA72.wav", "0x0513FA72", "s_m_y_cop_black_mini_01"); } }
        public static ScannerFile HASH08DB544A { get { return new ScannerFile("s_m_y_cop_01_black_mini_01\\0x08DB544A.wav", "0x08DB544A", "s_m_y_cop_black_mini_01"); } }
        public static ScannerFile HASH08E377EE { get { return new ScannerFile("s_m_y_cop_01_black_mini_01\\0x08E377EE.wav", "0x08E377EE", "s_m_y_cop_black_mini_01"); } }
        public static ScannerFile HASH09D5A5ED { get { return new ScannerFile("s_m_y_cop_01_black_mini_01\\0x09D5A5ED.wav", "0x09D5A5ED", "s_m_y_cop_black_mini_01"); } }
        public static ScannerFile HASH0AF015F7 { get { return new ScannerFile("s_m_y_cop_01_black_mini_01\\0x0AF015F7.wav", "0x0AF015F7", "s_m_y_cop_black_mini_01"); } }
        public static ScannerFile HASH0B5B6665 { get { return new ScannerFile("s_m_y_cop_01_black_mini_01\\0x0B5B6665.wav", "0x0B5B6665", "s_m_y_cop_black_mini_01"); } }
        public static ScannerFile HASH0B78C3D3 { get { return new ScannerFile("s_m_y_cop_01_black_mini_01\\0x0B78C3D3.wav", "0x0B78C3D3", "s_m_y_cop_black_mini_01"); } }
        public static ScannerFile HASH0D3B44AE { get { return new ScannerFile("s_m_y_cop_01_black_mini_01\\0x0D3B44AE.wav", "0x0D3B44AE", "s_m_y_cop_black_mini_01"); } }
        public static ScannerFile HASH0ED8BFA8 { get { return new ScannerFile("s_m_y_cop_01_black_mini_01\\0x0ED8BFA8.wav", "0x0ED8BFA8", "s_m_y_cop_black_mini_01"); } }
        public static ScannerFile HASH0F80B554 { get { return new ScannerFile("s_m_y_cop_01_black_mini_01\\0x0F80B554.wav", "0x0F80B554", "s_m_y_cop_black_mini_01"); } }
        public static ScannerFile HASH0FD39C10 { get { return new ScannerFile("s_m_y_cop_01_black_mini_01\\0x0FD39C10.wav", "0x0FD39C10", "s_m_y_cop_black_mini_01"); } }
        public static ScannerFile HASH1046DF8A { get { return new ScannerFile("s_m_y_cop_01_black_mini_01\\0x1046DF8A.wav", "0x1046DF8A", "s_m_y_cop_black_mini_01"); } }
        public static ScannerFile HASH118A68BD { get { return new ScannerFile("s_m_y_cop_01_black_mini_01\\0x118A68BD.wav", "0x118A68BD", "s_m_y_cop_black_mini_01"); } }
        public static ScannerFile HASH12461560 { get { return new ScannerFile("s_m_y_cop_01_black_mini_01\\0x12461560.wav", "0x12461560", "s_m_y_cop_black_mini_01"); } }
        public static ScannerFile HASH137027BC { get { return new ScannerFile("s_m_y_cop_01_black_mini_01\\0x137027BC.wav", "0x137027BC", "s_m_y_cop_black_mini_01"); } }
        public static ScannerFile HASH14C47BCF { get { return new ScannerFile("s_m_y_cop_01_black_mini_01\\0x14C47BCF.wav", "0x14C47BCF", "s_m_y_cop_black_mini_01"); } }
        public static ScannerFile HASH16A41369 { get { return new ScannerFile("s_m_y_cop_01_black_mini_01\\0x16A41369.wav", "0x16A41369", "s_m_y_cop_black_mini_01"); } }
        public static ScannerFile HASH16D11D9F { get { return new ScannerFile("s_m_y_cop_01_black_mini_01\\0x16D11D9F.wav", "0x16D11D9F", "s_m_y_cop_black_mini_01"); } }
        public static ScannerFile HASH1A56373F { get { return new ScannerFile("s_m_y_cop_01_black_mini_01\\0x1A56373F.wav", "0x1A56373F", "s_m_y_cop_black_mini_01"); } }
        public static ScannerFile HASH1AAF0500 { get { return new ScannerFile("s_m_y_cop_01_black_mini_01\\0x1AAF0500.wav", "0x1AAF0500", "s_m_y_cop_black_mini_01"); } }
        public static ScannerFile HASH1BCC27E6 { get { return new ScannerFile("s_m_y_cop_01_black_mini_01\\0x1BCC27E6.wav", "0x1BCC27E6", "s_m_y_cop_black_mini_01"); } }
        public static ScannerFile HASH1BD2FDBA { get { return new ScannerFile("s_m_y_cop_01_black_mini_01\\0x1BD2FDBA.wav", "0x1BD2FDBA", "s_m_y_cop_black_mini_01"); } }
        public static ScannerFile HASH1C086912 { get { return new ScannerFile("s_m_y_cop_01_black_mini_01\\0x1C086912.wav", "0x1C086912", "s_m_y_cop_black_mini_01"); } }
        public static ScannerFile HASH1E65C2A3 { get { return new ScannerFile("s_m_y_cop_01_black_mini_01\\0x1E65C2A3.wav", "0x1E65C2A3", "s_m_y_cop_black_mini_01"); } }
        public static ScannerFile HASH1EB82E73 { get { return new ScannerFile("s_m_y_cop_01_black_mini_01\\0x1EB82E73.wav", "0x1EB82E73", "s_m_y_cop_black_mini_01"); } }
        public static ScannerFile HASH1F7C154B { get { return new ScannerFile("s_m_y_cop_01_black_mini_01\\0x1F7C154B.wav", "0x1F7C154B", "s_m_y_cop_black_mini_01"); } }
    }
    public class s_m_y_cop_black_mini_02
    {
        public static ScannerFile HASH017221AA { get { return new ScannerFile("s_m_y_cop_01_black_mini_02\\0x017221AA.wav", "0x017221AA", "s_m_y_cop_black_mini_02"); } }
        public static ScannerFile HASH030276D5 { get { return new ScannerFile("s_m_y_cop_01_black_mini_02\\0x030276D5.wav", "0x030276D5", "s_m_y_cop_black_mini_02"); } }
        public static ScannerFile HASH046135A4 { get { return new ScannerFile("s_m_y_cop_01_black_mini_02\\0x046135A4.wav", "0x046135A4", "s_m_y_cop_black_mini_02"); } }
        public static ScannerFile HASH0496792A { get { return new ScannerFile("s_m_y_cop_01_black_mini_02\\0x0496792A.wav", "0x0496792A", "s_m_y_cop_black_mini_02"); } }
        public static ScannerFile HASH0513FA72 { get { return new ScannerFile("s_m_y_cop_01_black_mini_02\\0x0513FA72.wav", "0x0513FA72", "s_m_y_cop_black_mini_02"); } }
        public static ScannerFile HASH08DB544A { get { return new ScannerFile("s_m_y_cop_01_black_mini_02\\0x08DB544A.wav", "0x08DB544A", "s_m_y_cop_black_mini_02"); } }
        public static ScannerFile HASH08E377EE { get { return new ScannerFile("s_m_y_cop_01_black_mini_02\\0x08E377EE.wav", "0x08E377EE", "s_m_y_cop_black_mini_02"); } }
        public static ScannerFile HASH09D5A5ED { get { return new ScannerFile("s_m_y_cop_01_black_mini_02\\0x09D5A5ED.wav", "0x09D5A5ED", "s_m_y_cop_black_mini_02"); } }
        public static ScannerFile HASH0AF015F7 { get { return new ScannerFile("s_m_y_cop_01_black_mini_02\\0x0AF015F7.wav", "0x0AF015F7", "s_m_y_cop_black_mini_02"); } }
        public static ScannerFile HASH0B5B6665 { get { return new ScannerFile("s_m_y_cop_01_black_mini_02\\0x0B5B6665.wav", "0x0B5B6665", "s_m_y_cop_black_mini_02"); } }
        public static ScannerFile HASH0B78C3D3 { get { return new ScannerFile("s_m_y_cop_01_black_mini_02\\0x0B78C3D3.wav", "0x0B78C3D3", "s_m_y_cop_black_mini_02"); } }
        public static ScannerFile HASH0D3B44AE { get { return new ScannerFile("s_m_y_cop_01_black_mini_02\\0x0D3B44AE.wav", "0x0D3B44AE", "s_m_y_cop_black_mini_02"); } }
        public static ScannerFile HASH0ED8BFA8 { get { return new ScannerFile("s_m_y_cop_01_black_mini_02\\0x0ED8BFA8.wav", "0x0ED8BFA8", "s_m_y_cop_black_mini_02"); } }
        public static ScannerFile HASH0F80B554 { get { return new ScannerFile("s_m_y_cop_01_black_mini_02\\0x0F80B554.wav", "0x0F80B554", "s_m_y_cop_black_mini_02"); } }
        public static ScannerFile HASH0FD39C10 { get { return new ScannerFile("s_m_y_cop_01_black_mini_02\\0x0FD39C10.wav", "0x0FD39C10", "s_m_y_cop_black_mini_02"); } }
        public static ScannerFile HASH1046DF8A { get { return new ScannerFile("s_m_y_cop_01_black_mini_02\\0x1046DF8A.wav", "0x1046DF8A", "s_m_y_cop_black_mini_02"); } }
        public static ScannerFile HASH118A68BD { get { return new ScannerFile("s_m_y_cop_01_black_mini_02\\0x118A68BD.wav", "0x118A68BD", "s_m_y_cop_black_mini_02"); } }
        public static ScannerFile HASH12461560 { get { return new ScannerFile("s_m_y_cop_01_black_mini_02\\0x12461560.wav", "0x12461560", "s_m_y_cop_black_mini_02"); } }
        public static ScannerFile HASH137027BC { get { return new ScannerFile("s_m_y_cop_01_black_mini_02\\0x137027BC.wav", "0x137027BC", "s_m_y_cop_black_mini_02"); } }
        public static ScannerFile HASH14C47BCF { get { return new ScannerFile("s_m_y_cop_01_black_mini_02\\0x14C47BCF.wav", "0x14C47BCF", "s_m_y_cop_black_mini_02"); } }
        public static ScannerFile HASH16A41369 { get { return new ScannerFile("s_m_y_cop_01_black_mini_02\\0x16A41369.wav", "0x16A41369", "s_m_y_cop_black_mini_02"); } }
        public static ScannerFile HASH16D11D9F { get { return new ScannerFile("s_m_y_cop_01_black_mini_02\\0x16D11D9F.wav", "0x16D11D9F", "s_m_y_cop_black_mini_02"); } }
        public static ScannerFile HASH1A56373F { get { return new ScannerFile("s_m_y_cop_01_black_mini_02\\0x1A56373F.wav", "0x1A56373F", "s_m_y_cop_black_mini_02"); } }
        public static ScannerFile HASH1AAF0500 { get { return new ScannerFile("s_m_y_cop_01_black_mini_02\\0x1AAF0500.wav", "0x1AAF0500", "s_m_y_cop_black_mini_02"); } }
        public static ScannerFile HASH1BCC27E6 { get { return new ScannerFile("s_m_y_cop_01_black_mini_02\\0x1BCC27E6.wav", "0x1BCC27E6", "s_m_y_cop_black_mini_02"); } }
        public static ScannerFile HASH1BD2FDBA { get { return new ScannerFile("s_m_y_cop_01_black_mini_02\\0x1BD2FDBA.wav", "0x1BD2FDBA", "s_m_y_cop_black_mini_02"); } }
        public static ScannerFile HASH1C086912 { get { return new ScannerFile("s_m_y_cop_01_black_mini_02\\0x1C086912.wav", "0x1C086912", "s_m_y_cop_black_mini_02"); } }
        public static ScannerFile HASH1E65C2A3 { get { return new ScannerFile("s_m_y_cop_01_black_mini_02\\0x1E65C2A3.wav", "0x1E65C2A3", "s_m_y_cop_black_mini_02"); } }
        public static ScannerFile HASH1EB82E73 { get { return new ScannerFile("s_m_y_cop_01_black_mini_02\\0x1EB82E73.wav", "0x1EB82E73", "s_m_y_cop_black_mini_02"); } }
        public static ScannerFile HASH1F7C154B { get { return new ScannerFile("s_m_y_cop_01_black_mini_02\\0x1F7C154B.wav", "0x1F7C154B", "s_m_y_cop_black_mini_02"); } }
    }
    public class s_m_y_cop_black_mini_03
    {
        public static ScannerFile HASH017221AA { get { return new ScannerFile("s_m_y_cop_01_black_mini_03\\0x017221AA.wav", "0x017221AA", "s_m_y_cop_black_mini_03"); } }
        public static ScannerFile HASH030276D5 { get { return new ScannerFile("s_m_y_cop_01_black_mini_03\\0x030276D5.wav", "0x030276D5", "s_m_y_cop_black_mini_03"); } }
        public static ScannerFile HASH046135A4 { get { return new ScannerFile("s_m_y_cop_01_black_mini_03\\0x046135A4.wav", "0x046135A4", "s_m_y_cop_black_mini_03"); } }
        public static ScannerFile HASH0496792A { get { return new ScannerFile("s_m_y_cop_01_black_mini_03\\0x0496792A.wav", "0x0496792A", "s_m_y_cop_black_mini_03"); } }
        public static ScannerFile HASH0513FA72 { get { return new ScannerFile("s_m_y_cop_01_black_mini_03\\0x0513FA72.wav", "0x0513FA72", "s_m_y_cop_black_mini_03"); } }
        public static ScannerFile HASH08DB544A { get { return new ScannerFile("s_m_y_cop_01_black_mini_03\\0x08DB544A.wav", "0x08DB544A", "s_m_y_cop_black_mini_03"); } }
        public static ScannerFile HASH08E377EE { get { return new ScannerFile("s_m_y_cop_01_black_mini_03\\0x08E377EE.wav", "0x08E377EE", "s_m_y_cop_black_mini_03"); } }
        public static ScannerFile HASH09D5A5ED { get { return new ScannerFile("s_m_y_cop_01_black_mini_03\\0x09D5A5ED.wav", "0x09D5A5ED", "s_m_y_cop_black_mini_03"); } }
        public static ScannerFile HASH0AF015F7 { get { return new ScannerFile("s_m_y_cop_01_black_mini_03\\0x0AF015F7.wav", "0x0AF015F7", "s_m_y_cop_black_mini_03"); } }
        public static ScannerFile HASH0B5B6665 { get { return new ScannerFile("s_m_y_cop_01_black_mini_03\\0x0B5B6665.wav", "0x0B5B6665", "s_m_y_cop_black_mini_03"); } }
        public static ScannerFile HASH0B78C3D3 { get { return new ScannerFile("s_m_y_cop_01_black_mini_03\\0x0B78C3D3.wav", "0x0B78C3D3", "s_m_y_cop_black_mini_03"); } }
        public static ScannerFile HASH0D3B44AE { get { return new ScannerFile("s_m_y_cop_01_black_mini_03\\0x0D3B44AE.wav", "0x0D3B44AE", "s_m_y_cop_black_mini_03"); } }
        public static ScannerFile HASH0ED8BFA8 { get { return new ScannerFile("s_m_y_cop_01_black_mini_03\\0x0ED8BFA8.wav", "0x0ED8BFA8", "s_m_y_cop_black_mini_03"); } }
        public static ScannerFile HASH0F80B554 { get { return new ScannerFile("s_m_y_cop_01_black_mini_03\\0x0F80B554.wav", "0x0F80B554", "s_m_y_cop_black_mini_03"); } }
        public static ScannerFile HASH0FD39C10 { get { return new ScannerFile("s_m_y_cop_01_black_mini_03\\0x0FD39C10.wav", "0x0FD39C10", "s_m_y_cop_black_mini_03"); } }
        public static ScannerFile HASH1046DF8A { get { return new ScannerFile("s_m_y_cop_01_black_mini_03\\0x1046DF8A.wav", "0x1046DF8A", "s_m_y_cop_black_mini_03"); } }
        public static ScannerFile HASH118A68BD { get { return new ScannerFile("s_m_y_cop_01_black_mini_03\\0x118A68BD.wav", "0x118A68BD", "s_m_y_cop_black_mini_03"); } }
        public static ScannerFile HASH12461560 { get { return new ScannerFile("s_m_y_cop_01_black_mini_03\\0x12461560.wav", "0x12461560", "s_m_y_cop_black_mini_03"); } }
        public static ScannerFile HASH137027BC { get { return new ScannerFile("s_m_y_cop_01_black_mini_03\\0x137027BC.wav", "0x137027BC", "s_m_y_cop_black_mini_03"); } }
        public static ScannerFile HASH14C47BCF { get { return new ScannerFile("s_m_y_cop_01_black_mini_03\\0x14C47BCF.wav", "0x14C47BCF", "s_m_y_cop_black_mini_03"); } }
        public static ScannerFile HASH16A41369 { get { return new ScannerFile("s_m_y_cop_01_black_mini_03\\0x16A41369.wav", "0x16A41369", "s_m_y_cop_black_mini_03"); } }
        public static ScannerFile HASH16D11D9F { get { return new ScannerFile("s_m_y_cop_01_black_mini_03\\0x16D11D9F.wav", "0x16D11D9F", "s_m_y_cop_black_mini_03"); } }
        public static ScannerFile HASH1A56373F { get { return new ScannerFile("s_m_y_cop_01_black_mini_03\\0x1A56373F.wav", "0x1A56373F", "s_m_y_cop_black_mini_03"); } }
        public static ScannerFile HASH1AAF0500 { get { return new ScannerFile("s_m_y_cop_01_black_mini_03\\0x1AAF0500.wav", "0x1AAF0500", "s_m_y_cop_black_mini_03"); } }
        public static ScannerFile HASH1BCC27E6 { get { return new ScannerFile("s_m_y_cop_01_black_mini_03\\0x1BCC27E6.wav", "0x1BCC27E6", "s_m_y_cop_black_mini_03"); } }
        public static ScannerFile HASH1BD2FDBA { get { return new ScannerFile("s_m_y_cop_01_black_mini_03\\0x1BD2FDBA.wav", "0x1BD2FDBA", "s_m_y_cop_black_mini_03"); } }
        public static ScannerFile HASH1C086912 { get { return new ScannerFile("s_m_y_cop_01_black_mini_03\\0x1C086912.wav", "0x1C086912", "s_m_y_cop_black_mini_03"); } }
        public static ScannerFile HASH1E65C2A3 { get { return new ScannerFile("s_m_y_cop_01_black_mini_03\\0x1E65C2A3.wav", "0x1E65C2A3", "s_m_y_cop_black_mini_03"); } }
        public static ScannerFile HASH1EB82E73 { get { return new ScannerFile("s_m_y_cop_01_black_mini_03\\0x1EB82E73.wav", "0x1EB82E73", "s_m_y_cop_black_mini_03"); } }
        public static ScannerFile HASH1F7C154B { get { return new ScannerFile("s_m_y_cop_01_black_mini_03\\0x1F7C154B.wav", "0x1F7C154B", "s_m_y_cop_black_mini_03"); } }
    }
    public class s_m_y_cop_black_mini_04
    {
        public static ScannerFile HASH017221AA { get { return new ScannerFile("s_m_y_cop_01_black_mini_04\\0x017221AA.wav", "0x017221AA", "s_m_y_cop_black_mini_04"); } }
        public static ScannerFile HASH030276D5 { get { return new ScannerFile("s_m_y_cop_01_black_mini_04\\0x030276D5.wav", "0x030276D5", "s_m_y_cop_black_mini_04"); } }
        public static ScannerFile HASH046135A4 { get { return new ScannerFile("s_m_y_cop_01_black_mini_04\\0x046135A4.wav", "0x046135A4", "s_m_y_cop_black_mini_04"); } }
        public static ScannerFile HASH0496792A { get { return new ScannerFile("s_m_y_cop_01_black_mini_04\\0x0496792A.wav", "0x0496792A", "s_m_y_cop_black_mini_04"); } }
        public static ScannerFile HASH0513FA72 { get { return new ScannerFile("s_m_y_cop_01_black_mini_04\\0x0513FA72.wav", "0x0513FA72", "s_m_y_cop_black_mini_04"); } }
        public static ScannerFile HASH08DB544A { get { return new ScannerFile("s_m_y_cop_01_black_mini_04\\0x08DB544A.wav", "0x08DB544A", "s_m_y_cop_black_mini_04"); } }
        public static ScannerFile HASH08E377EE { get { return new ScannerFile("s_m_y_cop_01_black_mini_04\\0x08E377EE.wav", "0x08E377EE", "s_m_y_cop_black_mini_04"); } }
        public static ScannerFile HASH09D5A5ED { get { return new ScannerFile("s_m_y_cop_01_black_mini_04\\0x09D5A5ED.wav", "0x09D5A5ED", "s_m_y_cop_black_mini_04"); } }
        public static ScannerFile HASH0AF015F7 { get { return new ScannerFile("s_m_y_cop_01_black_mini_04\\0x0AF015F7.wav", "0x0AF015F7", "s_m_y_cop_black_mini_04"); } }
        public static ScannerFile HASH0B5B6665 { get { return new ScannerFile("s_m_y_cop_01_black_mini_04\\0x0B5B6665.wav", "0x0B5B6665", "s_m_y_cop_black_mini_04"); } }
        public static ScannerFile HASH0B78C3D3 { get { return new ScannerFile("s_m_y_cop_01_black_mini_04\\0x0B78C3D3.wav", "0x0B78C3D3", "s_m_y_cop_black_mini_04"); } }
        public static ScannerFile HASH0D3B44AE { get { return new ScannerFile("s_m_y_cop_01_black_mini_04\\0x0D3B44AE.wav", "0x0D3B44AE", "s_m_y_cop_black_mini_04"); } }
        public static ScannerFile HASH0ED8BFA8 { get { return new ScannerFile("s_m_y_cop_01_black_mini_04\\0x0ED8BFA8.wav", "0x0ED8BFA8", "s_m_y_cop_black_mini_04"); } }
        public static ScannerFile HASH0F80B554 { get { return new ScannerFile("s_m_y_cop_01_black_mini_04\\0x0F80B554.wav", "0x0F80B554", "s_m_y_cop_black_mini_04"); } }
        public static ScannerFile HASH0FD39C10 { get { return new ScannerFile("s_m_y_cop_01_black_mini_04\\0x0FD39C10.wav", "0x0FD39C10", "s_m_y_cop_black_mini_04"); } }
        public static ScannerFile HASH1046DF8A { get { return new ScannerFile("s_m_y_cop_01_black_mini_04\\0x1046DF8A.wav", "0x1046DF8A", "s_m_y_cop_black_mini_04"); } }
        public static ScannerFile HASH118A68BD { get { return new ScannerFile("s_m_y_cop_01_black_mini_04\\0x118A68BD.wav", "0x118A68BD", "s_m_y_cop_black_mini_04"); } }
        public static ScannerFile HASH12461560 { get { return new ScannerFile("s_m_y_cop_01_black_mini_04\\0x12461560.wav", "0x12461560", "s_m_y_cop_black_mini_04"); } }
        public static ScannerFile HASH137027BC { get { return new ScannerFile("s_m_y_cop_01_black_mini_04\\0x137027BC.wav", "0x137027BC", "s_m_y_cop_black_mini_04"); } }
        public static ScannerFile HASH14C47BCF { get { return new ScannerFile("s_m_y_cop_01_black_mini_04\\0x14C47BCF.wav", "0x14C47BCF", "s_m_y_cop_black_mini_04"); } }
        public static ScannerFile HASH16A41369 { get { return new ScannerFile("s_m_y_cop_01_black_mini_04\\0x16A41369.wav", "0x16A41369", "s_m_y_cop_black_mini_04"); } }
        public static ScannerFile HASH16D11D9F { get { return new ScannerFile("s_m_y_cop_01_black_mini_04\\0x16D11D9F.wav", "0x16D11D9F", "s_m_y_cop_black_mini_04"); } }
        public static ScannerFile HASH1A56373F { get { return new ScannerFile("s_m_y_cop_01_black_mini_04\\0x1A56373F.wav", "0x1A56373F", "s_m_y_cop_black_mini_04"); } }
        public static ScannerFile HASH1AAF0500 { get { return new ScannerFile("s_m_y_cop_01_black_mini_04\\0x1AAF0500.wav", "0x1AAF0500", "s_m_y_cop_black_mini_04"); } }
        public static ScannerFile HASH1BCC27E6 { get { return new ScannerFile("s_m_y_cop_01_black_mini_04\\0x1BCC27E6.wav", "0x1BCC27E6", "s_m_y_cop_black_mini_04"); } }
        public static ScannerFile HASH1BD2FDBA { get { return new ScannerFile("s_m_y_cop_01_black_mini_04\\0x1BD2FDBA.wav", "0x1BD2FDBA", "s_m_y_cop_black_mini_04"); } }
        public static ScannerFile HASH1C086912 { get { return new ScannerFile("s_m_y_cop_01_black_mini_04\\0x1C086912.wav", "0x1C086912", "s_m_y_cop_black_mini_04"); } }
        public static ScannerFile HASH1E65C2A3 { get { return new ScannerFile("s_m_y_cop_01_black_mini_04\\0x1E65C2A3.wav", "0x1E65C2A3", "s_m_y_cop_black_mini_04"); } }
        public static ScannerFile HASH1EB82E73 { get { return new ScannerFile("s_m_y_cop_01_black_mini_04\\0x1EB82E73.wav", "0x1EB82E73", "s_m_y_cop_black_mini_04"); } }
        public static ScannerFile HASH1F7C154B { get { return new ScannerFile("s_m_y_cop_01_black_mini_04\\0x1F7C154B.wav", "0x1F7C154B", "s_m_y_cop_black_mini_04"); } }
    }
    public class s_m_y_cop_white_full_01
    {
        public static ScannerFile SuspectIsOnFoot { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x00FFC2DB.wav", "SuspectIsOnFoot", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile SuspectInACar { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x017221AA.wav", "SuspectInACar", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile RequestingBackupWeNeedBackup { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x01A10324.wav", "RequestingBackupWeNeedBackup", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile MaydayWeAreGoingDown { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x030276D5.wav", "MaydayWeAreGoingDown", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile CopyThatWeAreInTheVecinity { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x046135A4.wav", "CopyThatWeAreInTheVecinity", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile WeHaveAVisualOnTheSuspectMovingSouth { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x0496792A.wav", "WeHaveAVisualOnTheSuspectMovingSouth", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile WeAreAirbornAndMovingIn { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x0513FA72.wav", "WeAreAirbornAndMovingIn", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile DispatchSuspectsVehicleInACollision { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x0846F27E.wav", "0x0846F27E", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile DoesAnybodyHaveAVisual { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x08DB544A.wav", "DoesAnybodyHaveAVisual", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile HaveVisualSuspectOnFoot { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x08E377EE.wav", "HaveVisualSuspectOnFoot", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile HASH09D5A5ED { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x09D5A5ED.wav", "0x09D5A5ED", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile WeNeedBackupNow { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x0AF015F7.wav", "WeNeedBackupNow", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile HASH0B5B6665 { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x0B5B6665.wav", "0x0B5B6665", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile HASH0B78C3D3 { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x0B78C3D3.wav", "0x0B78C3D3", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile SuspectEnteringTheFreeway { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x0D3B44AE.wav", "SuspectEnteringTheFreeway", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile SuspectLeavingTheFreeway { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x0D59A0C8.wav", "SuspectLeavingTheFreeway", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile SuspectHasFuckingCrashed { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x0ED8BFA8.wav", "SuspectHasFuckingCrashed", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile AirSupportSuspectIsHeadingNorth { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x0F80B554.wav", "AirSupportSuspectIsHeadingNorth", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile DispatchSuspectHasEneteredTheMetro { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x0FD39C10.wav", "DispatchSuspectHasEneteredTheMetro", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile SuspectOnAMotorcycle { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x1046DF8A.wav", "0x1046DF8A", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile AirSupportLostSuspect { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x118A68BD.wav", "AirSupportLostSuspect", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile HASH12461560 { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x12461560.wav", "0x12461560", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile DispatchSuspectOnFoot { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x137027BC.wav", "0x137027BC", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile RequestingBackup { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x144A2876.wav", "RequestingBackup", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile HASH14C47BCF { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x14C47BCF.wav", "0x14C47BCF", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile GotEyesHesOnFoot { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x16A41369.wav", "0x16A41369", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile HASH16D11D9F { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x16D11D9F.wav", "0x16D11D9F", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile DispatchYouHaveALocationForTheSuspect { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x1A56373F.wav", "DispatchYouHaveALocationForTheSuspect", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile HASH1AAF0500 { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x1AAF0500.wav", "0x1AAF0500", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile EnteredFreeway { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x1AF6E025.wav", "0x1AF6E025", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile SuspectIsInACarInPursuit { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x1B2B551D.wav", "0x1B2B551D", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile AirSupportIsInRoute { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x1BCC27E6.wav", "AirSupportIsInRoute", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile LeavingFreeway { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x1BD2FDBA.wav", "0x1BD2FDBA", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile CopyThatMovingRightNow { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x1C086912.wav", "CopyThatMovingRightNow", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile WeDoNotHaveAVisual { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x1E65C2A3.wav", "WeDoNotHaveAVisual", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile RogerThatWeAreOnOurWay { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x1EB82E73.wav", "RogerThatWeAreOnOurWay", "s_m_y_cop_white_full_01"); } }
        public static ScannerFile HASH1F7C154B { get { return new ScannerFile("s_m_y_cop_01_white_full_01\\0x1F7C154B.wav", "0x1F7C154B", "s_m_y_cop_white_full_01"); } }
    }
    public class s_m_y_cop_white_full_02
    {
        public static ScannerFile HASH00FFC2DB { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x00FFC2DB.wav", "0x00FFC2DB", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile HASH017221AA { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x017221AA.wav", "0x017221AA", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile MikeOscarSamInHotNeedOfBackup { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x01A10324.wav", "MikeOscarSamInHotNeedOfBackup", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile HASH030276D5 { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x030276D5.wav", "0x030276D5", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile CharlieFourRogerThatWereIntheArea { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x046135A4.wav", "CharlieFourRogerThatWereIntheArea", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile HASH0496792A { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x0496792A.wav", "0x0496792A", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile HASH0513FA72 { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x0513FA72.wav", "0x0513FA72", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile HASH0846F27E { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x0846F27E.wav", "0x0846F27E", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile HASH08DB544A { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x08DB544A.wav", "0x08DB544A", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile HASH08E377EE { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x08E377EE.wav", "0x08E377EE", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile HASH09D5A5ED { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x09D5A5ED.wav", "0x09D5A5ED", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile MikeOScarSamRequestingBackup { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x0AF015F7.wav", "MikeOScarSamRequestingBackup", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile HASH0B5B6665 { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x0B5B6665.wav", "0x0B5B6665", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile HASH0B78C3D3 { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x0B78C3D3.wav", "0x0B78C3D3", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile HASH0D3B44AE { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x0D3B44AE.wav", "0x0D3B44AE", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile HASH0D59A0C8 { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x0D59A0C8.wav", "0x0D59A0C8", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile HASH0ED8BFA8 { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x0ED8BFA8.wav", "0x0ED8BFA8", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile HASH0F80B554 { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x0F80B554.wav", "0x0F80B554", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile HASH0FD39C10 { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x0FD39C10.wav", "0x0FD39C10", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile HASH1046DF8A { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x1046DF8A.wav", "0x1046DF8A", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile HASH118A68BD { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x118A68BD.wav", "0x118A68BD", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile HASH12461560 { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x12461560.wav", "0x12461560", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile HASH137027BC { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x137027BC.wav", "0x137027BC", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile HASH144A2876 { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x144A2876.wav", "0x144A2876", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile HASH14C47BCF { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x14C47BCF.wav", "0x14C47BCF", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile HASH16A41369 { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x16A41369.wav", "0x16A41369", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile HASH16D11D9F { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x16D11D9F.wav", "0x16D11D9F", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile HASH1A56373F { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x1A56373F.wav", "0x1A56373F", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile HASH1AAF0500 { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x1AAF0500.wav", "0x1AAF0500", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile HASH1AF6E025 { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x1AF6E025.wav", "0x1AF6E025", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile HASH1B2B551D { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x1B2B551D.wav", "0x1B2B551D", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile HASH1BCC27E6 { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x1BCC27E6.wav", "0x1BCC27E6", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile HASH1BD2FDBA { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x1BD2FDBA.wav", "0x1BD2FDBA", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile CopyThatDIspatchWellFindThoseAnimals { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x1C086912.wav", "0x1C086912", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile HASH1E65C2A3 { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x1E65C2A3.wav", "0x1E65C2A3", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile Charlie4WellLookForThoseMaggots { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x1EB82E73.wav", "0x1EB82E73", "s_m_y_cop_white_full_02"); } }
        public static ScannerFile HASH1F7C154B { get { return new ScannerFile("s_m_y_cop_01_white_full_02\\0x1F7C154B.wav", "0x1F7C154B", "s_m_y_cop_white_full_02"); } }
    }
    public class s_m_y_cop_white_mini_01
    {
        public static ScannerFile HASH017221AA { get { return new ScannerFile("s_m_y_cop_01_white_mini_01\\0x017221AA.wav", "0x017221AA", "s_m_y_cop_white_mini_01"); } }
        public static ScannerFile HASH0AF015F7 { get { return new ScannerFile("s_m_y_cop_01_white_mini_01\\0x0AF015F7.wav", "0x0AF015F7", "s_m_y_cop_white_mini_01"); } }
        public static ScannerFile HASH0D3B44AE { get { return new ScannerFile("s_m_y_cop_01_white_mini_01\\0x0D3B44AE.wav", "0x0D3B44AE", "s_m_y_cop_white_mini_01"); } }
        public static ScannerFile HASH0ED8BFA8 { get { return new ScannerFile("s_m_y_cop_01_white_mini_01\\0x0ED8BFA8.wav", "0x0ED8BFA8", "s_m_y_cop_white_mini_01"); } }
        public static ScannerFile HASH0FD39C10 { get { return new ScannerFile("s_m_y_cop_01_white_mini_01\\0x0FD39C10.wav", "0x0FD39C10", "s_m_y_cop_white_mini_01"); } }
        public static ScannerFile HASH1046DF8A { get { return new ScannerFile("s_m_y_cop_01_white_mini_01\\0x1046DF8A.wav", "0x1046DF8A", "s_m_y_cop_white_mini_01"); } }
        public static ScannerFile HASH137027BC { get { return new ScannerFile("s_m_y_cop_01_white_mini_01\\0x137027BC.wav", "0x137027BC", "s_m_y_cop_white_mini_01"); } }
        public static ScannerFile HASH1BD2FDBA { get { return new ScannerFile("s_m_y_cop_01_white_mini_01\\0x1BD2FDBA.wav", "0x1BD2FDBA", "s_m_y_cop_white_mini_01"); } }
    }
    public class s_m_y_cop_white_mini_02
    {
        public static ScannerFile HASH017221AA { get { return new ScannerFile("s_m_y_cop_01_white_mini_02\\0x017221AA.wav", "0x017221AA", "s_m_y_cop_white_mini_02"); } }
        public static ScannerFile HASH030276D5 { get { return new ScannerFile("s_m_y_cop_01_white_mini_02\\0x030276D5.wav", "0x030276D5", "s_m_y_cop_white_mini_02"); } }
        public static ScannerFile HASH046135A4 { get { return new ScannerFile("s_m_y_cop_01_white_mini_02\\0x046135A4.wav", "0x046135A4", "s_m_y_cop_white_mini_02"); } }
        public static ScannerFile HASH0496792A { get { return new ScannerFile("s_m_y_cop_01_white_mini_02\\0x0496792A.wav", "0x0496792A", "s_m_y_cop_white_mini_02"); } }
        public static ScannerFile HASH0513FA72 { get { return new ScannerFile("s_m_y_cop_01_white_mini_02\\0x0513FA72.wav", "0x0513FA72", "s_m_y_cop_white_mini_02"); } }
        public static ScannerFile HASH08DB544A { get { return new ScannerFile("s_m_y_cop_01_white_mini_02\\0x08DB544A.wav", "0x08DB544A", "s_m_y_cop_white_mini_02"); } }
        public static ScannerFile HASH08E377EE { get { return new ScannerFile("s_m_y_cop_01_white_mini_02\\0x08E377EE.wav", "0x08E377EE", "s_m_y_cop_white_mini_02"); } }
        public static ScannerFile HASH09D5A5ED { get { return new ScannerFile("s_m_y_cop_01_white_mini_02\\0x09D5A5ED.wav", "0x09D5A5ED", "s_m_y_cop_white_mini_02"); } }
        public static ScannerFile INeedSomeSeriousBackupHere { get { return new ScannerFile("s_m_y_cop_01_white_mini_02\\0x0AF015F7.wav", "INeedSomeSeriousBackupHere", "s_m_y_cop_white_mini_02"); } }
        public static ScannerFile HASH0B5B6665 { get { return new ScannerFile("s_m_y_cop_01_white_mini_02\\0x0B5B6665.wav", "0x0B5B6665", "s_m_y_cop_white_mini_02"); } }
        public static ScannerFile HASH0B78C3D3 { get { return new ScannerFile("s_m_y_cop_01_white_mini_02\\0x0B78C3D3.wav", "0x0B78C3D3", "s_m_y_cop_white_mini_02"); } }
        public static ScannerFile HASH0D3B44AE { get { return new ScannerFile("s_m_y_cop_01_white_mini_02\\0x0D3B44AE.wav", "0x0D3B44AE", "s_m_y_cop_white_mini_02"); } }
        public static ScannerFile HASH0ED8BFA8 { get { return new ScannerFile("s_m_y_cop_01_white_mini_02\\0x0ED8BFA8.wav", "0x0ED8BFA8", "s_m_y_cop_white_mini_02"); } }
        public static ScannerFile HASH0F80B554 { get { return new ScannerFile("s_m_y_cop_01_white_mini_02\\0x0F80B554.wav", "0x0F80B554", "s_m_y_cop_white_mini_02"); } }
        public static ScannerFile HASH0FD39C10 { get { return new ScannerFile("s_m_y_cop_01_white_mini_02\\0x0FD39C10.wav", "0x0FD39C10", "s_m_y_cop_white_mini_02"); } }
        public static ScannerFile HASH1046DF8A { get { return new ScannerFile("s_m_y_cop_01_white_mini_02\\0x1046DF8A.wav", "0x1046DF8A", "s_m_y_cop_white_mini_02"); } }
        public static ScannerFile HASH118A68BD { get { return new ScannerFile("s_m_y_cop_01_white_mini_02\\0x118A68BD.wav", "0x118A68BD", "s_m_y_cop_white_mini_02"); } }
        public static ScannerFile HASH12461560 { get { return new ScannerFile("s_m_y_cop_01_white_mini_02\\0x12461560.wav", "0x12461560", "s_m_y_cop_white_mini_02"); } }
        public static ScannerFile HASH137027BC { get { return new ScannerFile("s_m_y_cop_01_white_mini_02\\0x137027BC.wav", "0x137027BC", "s_m_y_cop_white_mini_02"); } }
        public static ScannerFile HASH14C47BCF { get { return new ScannerFile("s_m_y_cop_01_white_mini_02\\0x14C47BCF.wav", "0x14C47BCF", "s_m_y_cop_white_mini_02"); } }
        public static ScannerFile HASH16A41369 { get { return new ScannerFile("s_m_y_cop_01_white_mini_02\\0x16A41369.wav", "0x16A41369", "s_m_y_cop_white_mini_02"); } }
        public static ScannerFile HASH16D11D9F { get { return new ScannerFile("s_m_y_cop_01_white_mini_02\\0x16D11D9F.wav", "0x16D11D9F", "s_m_y_cop_white_mini_02"); } }
        public static ScannerFile HASH1A56373F { get { return new ScannerFile("s_m_y_cop_01_white_mini_02\\0x1A56373F.wav", "0x1A56373F", "s_m_y_cop_white_mini_02"); } }
        public static ScannerFile HASH1AAF0500 { get { return new ScannerFile("s_m_y_cop_01_white_mini_02\\0x1AAF0500.wav", "0x1AAF0500", "s_m_y_cop_white_mini_02"); } }
        public static ScannerFile HASH1BCC27E6 { get { return new ScannerFile("s_m_y_cop_01_white_mini_02\\0x1BCC27E6.wav", "0x1BCC27E6", "s_m_y_cop_white_mini_02"); } }
        public static ScannerFile HASH1BD2FDBA { get { return new ScannerFile("s_m_y_cop_01_white_mini_02\\0x1BD2FDBA.wav", "0x1BD2FDBA", "s_m_y_cop_white_mini_02"); } }
        public static ScannerFile HASH1C086912 { get { return new ScannerFile("s_m_y_cop_01_white_mini_02\\0x1C086912.wav", "0x1C086912", "s_m_y_cop_white_mini_02"); } }
        public static ScannerFile HASH1E65C2A3 { get { return new ScannerFile("s_m_y_cop_01_white_mini_02\\0x1E65C2A3.wav", "0x1E65C2A3", "s_m_y_cop_white_mini_02"); } }
        public static ScannerFile HASH1EB82E73 { get { return new ScannerFile("s_m_y_cop_01_white_mini_02\\0x1EB82E73.wav", "0x1EB82E73", "s_m_y_cop_white_mini_02"); } }
        public static ScannerFile HASH1F7C154B { get { return new ScannerFile("s_m_y_cop_01_white_mini_02\\0x1F7C154B.wav", "0x1F7C154B", "s_m_y_cop_white_mini_02"); } }
    }
    public class s_m_y_cop_white_mini_03
    {
        public static ScannerFile HASH017221AA { get { return new ScannerFile("s_m_y_cop_01_white_mini_03\\0x017221AA.wav", "0x017221AA", "s_m_y_cop_white_mini_03"); } }
        public static ScannerFile HASH030276D5 { get { return new ScannerFile("s_m_y_cop_01_white_mini_03\\0x030276D5.wav", "0x030276D5", "s_m_y_cop_white_mini_03"); } }
        public static ScannerFile HASH046135A4 { get { return new ScannerFile("s_m_y_cop_01_white_mini_03\\0x046135A4.wav", "0x046135A4", "s_m_y_cop_white_mini_03"); } }
        public static ScannerFile HASH0496792A { get { return new ScannerFile("s_m_y_cop_01_white_mini_03\\0x0496792A.wav", "0x0496792A", "s_m_y_cop_white_mini_03"); } }
        public static ScannerFile HASH0513FA72 { get { return new ScannerFile("s_m_y_cop_01_white_mini_03\\0x0513FA72.wav", "0x0513FA72", "s_m_y_cop_white_mini_03"); } }
        public static ScannerFile DispatchNeedSomeGuidanceHere { get { return new ScannerFile("s_m_y_cop_01_white_mini_03\\0x08DB544A.wav", "0x08DB544A", "s_m_y_cop_white_mini_03"); } }
        public static ScannerFile HASH08E377EE { get { return new ScannerFile("s_m_y_cop_01_white_mini_03\\0x08E377EE.wav", "0x08E377EE", "s_m_y_cop_white_mini_03"); } }
        public static ScannerFile HASH09D5A5ED { get { return new ScannerFile("s_m_y_cop_01_white_mini_03\\0x09D5A5ED.wav", "0x09D5A5ED", "s_m_y_cop_white_mini_03"); } }
        public static ScannerFile OfficerInNeedofSomeBackupHere { get { return new ScannerFile("s_m_y_cop_01_white_mini_03\\0x0AF015F7.wav", "OfficerInNeedofSomeBackupHere", "s_m_y_cop_white_mini_03"); } }
        public static ScannerFile HASH0B5B6665 { get { return new ScannerFile("s_m_y_cop_01_white_mini_03\\0x0B5B6665.wav", "0x0B5B6665", "s_m_y_cop_white_mini_03"); } }
        public static ScannerFile HASH0B78C3D3 { get { return new ScannerFile("s_m_y_cop_01_white_mini_03\\0x0B78C3D3.wav", "0x0B78C3D3", "s_m_y_cop_white_mini_03"); } }
        public static ScannerFile HASH0D3B44AE { get { return new ScannerFile("s_m_y_cop_01_white_mini_03\\0x0D3B44AE.wav", "0x0D3B44AE", "s_m_y_cop_white_mini_03"); } }
        public static ScannerFile HASH0ED8BFA8 { get { return new ScannerFile("s_m_y_cop_01_white_mini_03\\0x0ED8BFA8.wav", "0x0ED8BFA8", "s_m_y_cop_white_mini_03"); } }
        public static ScannerFile HASH0F80B554 { get { return new ScannerFile("s_m_y_cop_01_white_mini_03\\0x0F80B554.wav", "0x0F80B554", "s_m_y_cop_white_mini_03"); } }
        public static ScannerFile HASH0FD39C10 { get { return new ScannerFile("s_m_y_cop_01_white_mini_03\\0x0FD39C10.wav", "0x0FD39C10", "s_m_y_cop_white_mini_03"); } }
        public static ScannerFile HASH1046DF8A { get { return new ScannerFile("s_m_y_cop_01_white_mini_03\\0x1046DF8A.wav", "0x1046DF8A", "s_m_y_cop_white_mini_03"); } }
        public static ScannerFile HASH118A68BD { get { return new ScannerFile("s_m_y_cop_01_white_mini_03\\0x118A68BD.wav", "0x118A68BD", "s_m_y_cop_white_mini_03"); } }
        public static ScannerFile HASH12461560 { get { return new ScannerFile("s_m_y_cop_01_white_mini_03\\0x12461560.wav", "0x12461560", "s_m_y_cop_white_mini_03"); } }
        public static ScannerFile HASH137027BC { get { return new ScannerFile("s_m_y_cop_01_white_mini_03\\0x137027BC.wav", "0x137027BC", "s_m_y_cop_white_mini_03"); } }
        public static ScannerFile HASH14C47BCF { get { return new ScannerFile("s_m_y_cop_01_white_mini_03\\0x14C47BCF.wav", "0x14C47BCF", "s_m_y_cop_white_mini_03"); } }
        public static ScannerFile HASH16A41369 { get { return new ScannerFile("s_m_y_cop_01_white_mini_03\\0x16A41369.wav", "0x16A41369", "s_m_y_cop_white_mini_03"); } }
        public static ScannerFile HASH16D11D9F { get { return new ScannerFile("s_m_y_cop_01_white_mini_03\\0x16D11D9F.wav", "0x16D11D9F", "s_m_y_cop_white_mini_03"); } }
        public static ScannerFile HASH1A56373F { get { return new ScannerFile("s_m_y_cop_01_white_mini_03\\0x1A56373F.wav", "0x1A56373F", "s_m_y_cop_white_mini_03"); } }
        public static ScannerFile HASH1AAF0500 { get { return new ScannerFile("s_m_y_cop_01_white_mini_03\\0x1AAF0500.wav", "0x1AAF0500", "s_m_y_cop_white_mini_03"); } }
        public static ScannerFile HASH1BCC27E6 { get { return new ScannerFile("s_m_y_cop_01_white_mini_03\\0x1BCC27E6.wav", "0x1BCC27E6", "s_m_y_cop_white_mini_03"); } }
        public static ScannerFile HASH1BD2FDBA { get { return new ScannerFile("s_m_y_cop_01_white_mini_03\\0x1BD2FDBA.wav", "0x1BD2FDBA", "s_m_y_cop_white_mini_03"); } }
        public static ScannerFile HASH1C086912 { get { return new ScannerFile("s_m_y_cop_01_white_mini_03\\0x1C086912.wav", "0x1C086912", "s_m_y_cop_white_mini_03"); } }
        public static ScannerFile HASH1E65C2A3 { get { return new ScannerFile("s_m_y_cop_01_white_mini_03\\0x1E65C2A3.wav", "0x1E65C2A3", "s_m_y_cop_white_mini_03"); } }
        public static ScannerFile AdamFourCopy { get { return new ScannerFile("s_m_y_cop_01_white_mini_03\\0x1EB82E73.wav", "AdamFourCopy", "s_m_y_cop_white_mini_03"); } }
        public static ScannerFile HASH1F7C154B { get { return new ScannerFile("s_m_y_cop_01_white_mini_03\\0x1F7C154B.wav", "0x1F7C154B", "s_m_y_cop_white_mini_03"); } }
    }
    public class s_m_y_cop_white_mini_04
    {
        public static ScannerFile HASH017221AA { get { return new ScannerFile("s_m_y_cop_01_white_mini_04\\0x017221AA.wav", "0x017221AA", "s_m_y_cop_white_mini_04"); } }
        public static ScannerFile HASH030276D5 { get { return new ScannerFile("s_m_y_cop_01_white_mini_04\\0x030276D5.wav", "0x030276D5", "s_m_y_cop_white_mini_04"); } }
        public static ScannerFile HASH046135A4 { get { return new ScannerFile("s_m_y_cop_01_white_mini_04\\0x046135A4.wav", "0x046135A4", "s_m_y_cop_white_mini_04"); } }
        public static ScannerFile HASH0496792A { get { return new ScannerFile("s_m_y_cop_01_white_mini_04\\0x0496792A.wav", "0x0496792A", "s_m_y_cop_white_mini_04"); } }
        public static ScannerFile HASH0513FA72 { get { return new ScannerFile("s_m_y_cop_01_white_mini_04\\0x0513FA72.wav", "0x0513FA72", "s_m_y_cop_white_mini_04"); } }
        public static ScannerFile HASH08DB544A { get { return new ScannerFile("s_m_y_cop_01_white_mini_04\\0x08DB544A.wav", "0x08DB544A", "s_m_y_cop_white_mini_04"); } }
        public static ScannerFile HASH08E377EE { get { return new ScannerFile("s_m_y_cop_01_white_mini_04\\0x08E377EE.wav", "0x08E377EE", "s_m_y_cop_white_mini_04"); } }
        public static ScannerFile HASH09D5A5ED { get { return new ScannerFile("s_m_y_cop_01_white_mini_04\\0x09D5A5ED.wav", "0x09D5A5ED", "s_m_y_cop_white_mini_04"); } }
        public static ScannerFile HASH0AF015F7 { get { return new ScannerFile("s_m_y_cop_01_white_mini_04\\0x0AF015F7.wav", "0x0AF015F7", "s_m_y_cop_white_mini_04"); } }
        public static ScannerFile HASH0B5B6665 { get { return new ScannerFile("s_m_y_cop_01_white_mini_04\\0x0B5B6665.wav", "0x0B5B6665", "s_m_y_cop_white_mini_04"); } }
        public static ScannerFile HASH0B78C3D3 { get { return new ScannerFile("s_m_y_cop_01_white_mini_04\\0x0B78C3D3.wav", "0x0B78C3D3", "s_m_y_cop_white_mini_04"); } }
        public static ScannerFile HASH0D3B44AE { get { return new ScannerFile("s_m_y_cop_01_white_mini_04\\0x0D3B44AE.wav", "0x0D3B44AE", "s_m_y_cop_white_mini_04"); } }
        public static ScannerFile HASH0ED8BFA8 { get { return new ScannerFile("s_m_y_cop_01_white_mini_04\\0x0ED8BFA8.wav", "0x0ED8BFA8", "s_m_y_cop_white_mini_04"); } }
        public static ScannerFile HASH0F80B554 { get { return new ScannerFile("s_m_y_cop_01_white_mini_04\\0x0F80B554.wav", "0x0F80B554", "s_m_y_cop_white_mini_04"); } }
        public static ScannerFile HASH0FD39C10 { get { return new ScannerFile("s_m_y_cop_01_white_mini_04\\0x0FD39C10.wav", "0x0FD39C10", "s_m_y_cop_white_mini_04"); } }
        public static ScannerFile HASH1046DF8A { get { return new ScannerFile("s_m_y_cop_01_white_mini_04\\0x1046DF8A.wav", "0x1046DF8A", "s_m_y_cop_white_mini_04"); } }
        public static ScannerFile HASH118A68BD { get { return new ScannerFile("s_m_y_cop_01_white_mini_04\\0x118A68BD.wav", "0x118A68BD", "s_m_y_cop_white_mini_04"); } }
        public static ScannerFile HASH12461560 { get { return new ScannerFile("s_m_y_cop_01_white_mini_04\\0x12461560.wav", "0x12461560", "s_m_y_cop_white_mini_04"); } }
        public static ScannerFile HASH137027BC { get { return new ScannerFile("s_m_y_cop_01_white_mini_04\\0x137027BC.wav", "0x137027BC", "s_m_y_cop_white_mini_04"); } }
        public static ScannerFile HASH14C47BCF { get { return new ScannerFile("s_m_y_cop_01_white_mini_04\\0x14C47BCF.wav", "0x14C47BCF", "s_m_y_cop_white_mini_04"); } }
        public static ScannerFile HASH16A41369 { get { return new ScannerFile("s_m_y_cop_01_white_mini_04\\0x16A41369.wav", "0x16A41369", "s_m_y_cop_white_mini_04"); } }
        public static ScannerFile HASH16D11D9F { get { return new ScannerFile("s_m_y_cop_01_white_mini_04\\0x16D11D9F.wav", "0x16D11D9F", "s_m_y_cop_white_mini_04"); } }
        public static ScannerFile HASH1A56373F { get { return new ScannerFile("s_m_y_cop_01_white_mini_04\\0x1A56373F.wav", "0x1A56373F", "s_m_y_cop_white_mini_04"); } }
        public static ScannerFile HASH1AAF0500 { get { return new ScannerFile("s_m_y_cop_01_white_mini_04\\0x1AAF0500.wav", "0x1AAF0500", "s_m_y_cop_white_mini_04"); } }
        public static ScannerFile HASH1BCC27E6 { get { return new ScannerFile("s_m_y_cop_01_white_mini_04\\0x1BCC27E6.wav", "0x1BCC27E6", "s_m_y_cop_white_mini_04"); } }
        public static ScannerFile HASH1BD2FDBA { get { return new ScannerFile("s_m_y_cop_01_white_mini_04\\0x1BD2FDBA.wav", "0x1BD2FDBA", "s_m_y_cop_white_mini_04"); } }
        public static ScannerFile HASH1C086912 { get { return new ScannerFile("s_m_y_cop_01_white_mini_04\\0x1C086912.wav", "0x1C086912", "s_m_y_cop_white_mini_04"); } }
        public static ScannerFile HASH1E65C2A3 { get { return new ScannerFile("s_m_y_cop_01_white_mini_04\\0x1E65C2A3.wav", "0x1E65C2A3", "s_m_y_cop_white_mini_04"); } }
        public static ScannerFile HASH1EB82E73 { get { return new ScannerFile("s_m_y_cop_01_white_mini_04\\0x1EB82E73.wav", "0x1EB82E73", "s_m_y_cop_white_mini_04"); } }
        public static ScannerFile HASH1F7C154B { get { return new ScannerFile("s_m_y_cop_01_white_mini_04\\0x1F7C154B.wav", "0x1F7C154B", "s_m_y_cop_white_mini_04"); } }
    }
    public class s_m_y_hwaycop_black_full_01
    {
        public static ScannerFile HASH00FFC2DB { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x00FFC2DB.wav", "0x00FFC2DB", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH017221AA { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x017221AA.wav", "0x017221AA", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH01A10324 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x01A10324.wav", "0x01A10324", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH030276D5 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x030276D5.wav", "0x030276D5", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH046135A4 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x046135A4.wav", "0x046135A4", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH0496792A { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x0496792A.wav", "0x0496792A", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH0513FA72 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x0513FA72.wav", "0x0513FA72", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH053ECA5F { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x053ECA5F.wav", "0x053ECA5F", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH0846F27E { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x0846F27E.wav", "0x0846F27E", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH08DB544A { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x08DB544A.wav", "0x08DB544A", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH08E377EE { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x08E377EE.wav", "0x08E377EE", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH09D5A5ED { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x09D5A5ED.wav", "0x09D5A5ED", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile WeNeedBackupQuick { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x0AF015F7.wav", "WeNeedBackupQuick", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH0B5B6665 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x0B5B6665.wav", "0x0B5B6665", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH0B78C3D3 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x0B78C3D3.wav", "0x0B78C3D3", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH0D3B44AE { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x0D3B44AE.wav", "0x0D3B44AE", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH0D59A0C8 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x0D59A0C8.wav", "0x0D59A0C8", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH0ED8BFA8 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x0ED8BFA8.wav", "0x0ED8BFA8", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH0F80B554 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x0F80B554.wav", "0x0F80B554", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH0FD39C10 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x0FD39C10.wav", "0x0FD39C10", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH1046DF8A { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x1046DF8A.wav", "0x1046DF8A", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH118A68BD { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x118A68BD.wav", "0x118A68BD", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH12461560 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x12461560.wav", "0x12461560", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH137027BC { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x137027BC.wav", "0x137027BC", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH144A2876 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x144A2876.wav", "0x144A2876", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH14C47BCF { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x14C47BCF.wav", "0x14C47BCF", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH16A41369 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x16A41369.wav", "0x16A41369", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH16D11D9F { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x16D11D9F.wav", "0x16D11D9F", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH17776ED0 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x17776ED0.wav", "0x17776ED0", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH1A56373F { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x1A56373F.wav", "0x1A56373F", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH1AAF0500 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x1AAF0500.wav", "0x1AAF0500", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH1AF6E025 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x1AF6E025.wav", "0x1AF6E025", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH1B2B551D { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x1B2B551D.wav", "0x1B2B551D", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH1BCC27E6 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x1BCC27E6.wav", "0x1BCC27E6", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH1BD2FDBA { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x1BD2FDBA.wav", "0x1BD2FDBA", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH1C086912 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x1C086912.wav", "0x1C086912", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH1E65C2A3 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x1E65C2A3.wav", "0x1E65C2A3", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH1EB82E73 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x1EB82E73.wav", "0x1EB82E73", "s_m_y_hwaycop_black_full_01"); } }
        public static ScannerFile HASH1F7C154B { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_01\\0x1F7C154B.wav", "0x1F7C154B", "s_m_y_hwaycop_black_full_01"); } }
    }
    public class s_m_y_hwaycop_black_full_02
    {
        public static ScannerFile HASH00FFC2DB { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x00FFC2DB.wav", "0x00FFC2DB", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH017221AA { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x017221AA.wav", "0x017221AA", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH01A10324 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x01A10324.wav", "0x01A10324", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH030276D5 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x030276D5.wav", "0x030276D5", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH046135A4 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x046135A4.wav", "0x046135A4", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH0496792A { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x0496792A.wav", "0x0496792A", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH0513FA72 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x0513FA72.wav", "0x0513FA72", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH0846F27E { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x0846F27E.wav", "0x0846F27E", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH08DB544A { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x08DB544A.wav", "0x08DB544A", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH08E377EE { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x08E377EE.wav", "0x08E377EE", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH09D5A5ED { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x09D5A5ED.wav", "0x09D5A5ED", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH0AF015F7 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x0AF015F7.wav", "0x0AF015F7", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH0B5B6665 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x0B5B6665.wav", "0x0B5B6665", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH0B78C3D3 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x0B78C3D3.wav", "0x0B78C3D3", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH0D3B44AE { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x0D3B44AE.wav", "0x0D3B44AE", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH0D59A0C8 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x0D59A0C8.wav", "0x0D59A0C8", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH0ED8BFA8 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x0ED8BFA8.wav", "0x0ED8BFA8", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH0F80B554 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x0F80B554.wav", "0x0F80B554", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH0FD39C10 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x0FD39C10.wav", "0x0FD39C10", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH1046DF8A { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x1046DF8A.wav", "0x1046DF8A", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH118A68BD { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x118A68BD.wav", "0x118A68BD", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH12461560 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x12461560.wav", "0x12461560", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH137027BC { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x137027BC.wav", "0x137027BC", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH144A2876 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x144A2876.wav", "0x144A2876", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH14C47BCF { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x14C47BCF.wav", "0x14C47BCF", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH16A41369 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x16A41369.wav", "0x16A41369", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH16D11D9F { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x16D11D9F.wav", "0x16D11D9F", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH1A56373F { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x1A56373F.wav", "0x1A56373F", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH1AAF0500 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x1AAF0500.wav", "0x1AAF0500", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH1AF6E025 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x1AF6E025.wav", "0x1AF6E025", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH1B2B551D { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x1B2B551D.wav", "0x1B2B551D", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH1BCC27E6 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x1BCC27E6.wav", "0x1BCC27E6", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH1BD2FDBA { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x1BD2FDBA.wav", "0x1BD2FDBA", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH1C086912 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x1C086912.wav", "0x1C086912", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH1E65C2A3 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x1E65C2A3.wav", "0x1E65C2A3", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH1EB82E73 { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x1EB82E73.wav", "0x1EB82E73", "s_m_y_hwaycop_black_full_02"); } }
        public static ScannerFile HASH1F7C154B { get { return new ScannerFile("s_m_y_hwaycop_01_black_full_02\\0x1F7C154B.wav", "0x1F7C154B", "s_m_y_hwaycop_black_full_02"); } }
    }
    public class s_m_y_hwaycop_white_full_01
    {
        public static ScannerFile HASH00FFC2DB { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x00FFC2DB.wav", "0x00FFC2DB", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH017221AA { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x017221AA.wav", "0x017221AA", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH01A10324 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x01A10324.wav", "0x01A10324", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH030276D5 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x030276D5.wav", "0x030276D5", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH046135A4 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x046135A4.wav", "0x046135A4", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH0496792A { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x0496792A.wav", "0x0496792A", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH0513FA72 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x0513FA72.wav", "0x0513FA72", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH0846F27E { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x0846F27E.wav", "0x0846F27E", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH08DB544A { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x08DB544A.wav", "0x08DB544A", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH08E377EE { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x08E377EE.wav", "0x08E377EE", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH09D5A5ED { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x09D5A5ED.wav", "0x09D5A5ED", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH0AF015F7 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x0AF015F7.wav", "0x0AF015F7", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH0B5B6665 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x0B5B6665.wav", "0x0B5B6665", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH0B78C3D3 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x0B78C3D3.wav", "0x0B78C3D3", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH0D3B44AE { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x0D3B44AE.wav", "0x0D3B44AE", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH0D59A0C8 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x0D59A0C8.wav", "0x0D59A0C8", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH0ED8BFA8 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x0ED8BFA8.wav", "0x0ED8BFA8", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH0F80B554 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x0F80B554.wav", "0x0F80B554", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH0FD39C10 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x0FD39C10.wav", "0x0FD39C10", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH1046DF8A { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x1046DF8A.wav", "0x1046DF8A", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH118A68BD { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x118A68BD.wav", "0x118A68BD", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH12461560 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x12461560.wav", "0x12461560", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH137027BC { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x137027BC.wav", "0x137027BC", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH144A2876 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x144A2876.wav", "0x144A2876", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH14C47BCF { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x14C47BCF.wav", "0x14C47BCF", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH16A41369 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x16A41369.wav", "0x16A41369", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH16D11D9F { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x16D11D9F.wav", "0x16D11D9F", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH1A56373F { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x1A56373F.wav", "0x1A56373F", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH1AAF0500 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x1AAF0500.wav", "0x1AAF0500", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH1AF6E025 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x1AF6E025.wav", "0x1AF6E025", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH1B2B551D { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x1B2B551D.wav", "0x1B2B551D", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH1BCC27E6 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x1BCC27E6.wav", "0x1BCC27E6", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH1BD2FDBA { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x1BD2FDBA.wav", "0x1BD2FDBA", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH1C086912 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x1C086912.wav", "0x1C086912", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH1E65C2A3 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x1E65C2A3.wav", "0x1E65C2A3", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH1EB82E73 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x1EB82E73.wav", "0x1EB82E73", "s_m_y_hwaycop_white_full_01"); } }
        public static ScannerFile HASH1F7C154B { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_01\\0x1F7C154B.wav", "0x1F7C154B", "s_m_y_hwaycop_white_full_01"); } }
    }
    public class s_m_y_hwaycop_white_full_02
    {
        public static ScannerFile HASH00FFC2DB { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x00FFC2DB.wav", "0x00FFC2DB", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH017221AA { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x017221AA.wav", "0x017221AA", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH01A10324 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x01A10324.wav", "0x01A10324", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH030276D5 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x030276D5.wav", "0x030276D5", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH046135A4 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x046135A4.wav", "0x046135A4", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH0496792A { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x0496792A.wav", "0x0496792A", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH0513FA72 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x0513FA72.wav", "0x0513FA72", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH0846F27E { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x0846F27E.wav", "0x0846F27E", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH08DB544A { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x08DB544A.wav", "0x08DB544A", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH08E377EE { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x08E377EE.wav", "0x08E377EE", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH09D5A5ED { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x09D5A5ED.wav", "0x09D5A5ED", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH0AF015F7 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x0AF015F7.wav", "0x0AF015F7", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH0B5B6665 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x0B5B6665.wav", "0x0B5B6665", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH0B78C3D3 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x0B78C3D3.wav", "0x0B78C3D3", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH0D3B44AE { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x0D3B44AE.wav", "0x0D3B44AE", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH0D59A0C8 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x0D59A0C8.wav", "0x0D59A0C8", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH0ED8BFA8 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x0ED8BFA8.wav", "0x0ED8BFA8", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH0F80B554 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x0F80B554.wav", "0x0F80B554", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH0FD39C10 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x0FD39C10.wav", "0x0FD39C10", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH1046DF8A { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x1046DF8A.wav", "0x1046DF8A", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH118A68BD { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x118A68BD.wav", "0x118A68BD", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH12461560 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x12461560.wav", "0x12461560", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH137027BC { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x137027BC.wav", "0x137027BC", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH144A2876 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x144A2876.wav", "0x144A2876", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH14C47BCF { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x14C47BCF.wav", "0x14C47BCF", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH16A41369 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x16A41369.wav", "0x16A41369", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH16D11D9F { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x16D11D9F.wav", "0x16D11D9F", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH1A56373F { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x1A56373F.wav", "0x1A56373F", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH1AAF0500 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x1AAF0500.wav", "0x1AAF0500", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH1AF6E025 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x1AF6E025.wav", "0x1AF6E025", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH1B2B551D { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x1B2B551D.wav", "0x1B2B551D", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH1BCC27E6 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x1BCC27E6.wav", "0x1BCC27E6", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH1BD2FDBA { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x1BD2FDBA.wav", "0x1BD2FDBA", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH1C086912 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x1C086912.wav", "0x1C086912", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH1E65C2A3 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x1E65C2A3.wav", "0x1E65C2A3", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH1EB82E73 { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x1EB82E73.wav", "0x1EB82E73", "s_m_y_hwaycop_white_full_02"); } }
        public static ScannerFile HASH1F7C154B { get { return new ScannerFile("s_m_y_hwaycop_01_white_full_02\\0x1F7C154B.wav", "0x1F7C154B", "s_m_y_hwaycop_white_full_02"); } }
    }
    public class s_m_y_sheriff_white_full_01
    {
        public static ScannerFile HASH00FFC2DB { get { return new ScannerFile("s_m_y_sheriff_01_white_full_01\\0x00FFC2DB.wav", "0x00FFC2DB", "s_m_y_sheriff_white_full_01"); } }
        public static ScannerFile HASH017221AA { get { return new ScannerFile("s_m_y_sheriff_01_white_full_01\\0x017221AA.wav", "0x017221AA", "s_m_y_sheriff_white_full_01"); } }
        public static ScannerFile HASH01A10324 { get { return new ScannerFile("s_m_y_sheriff_01_white_full_01\\0x01A10324.wav", "0x01A10324", "s_m_y_sheriff_white_full_01"); } }
        public static ScannerFile HASH0846F27E { get { return new ScannerFile("s_m_y_sheriff_01_white_full_01\\0x0846F27E.wav", "0x0846F27E", "s_m_y_sheriff_white_full_01"); } }
        public static ScannerFile HASH0AF015F7 { get { return new ScannerFile("s_m_y_sheriff_01_white_full_01\\0x0AF015F7.wav", "0x0AF015F7", "s_m_y_sheriff_white_full_01"); } }
        public static ScannerFile HASH0D3B44AE { get { return new ScannerFile("s_m_y_sheriff_01_white_full_01\\0x0D3B44AE.wav", "0x0D3B44AE", "s_m_y_sheriff_white_full_01"); } }
        public static ScannerFile HASH0D59A0C8 { get { return new ScannerFile("s_m_y_sheriff_01_white_full_01\\0x0D59A0C8.wav", "0x0D59A0C8", "s_m_y_sheriff_white_full_01"); } }
        public static ScannerFile HASH0ED8BFA8 { get { return new ScannerFile("s_m_y_sheriff_01_white_full_01\\0x0ED8BFA8.wav", "0x0ED8BFA8", "s_m_y_sheriff_white_full_01"); } }
        public static ScannerFile HASH0FD39C10 { get { return new ScannerFile("s_m_y_sheriff_01_white_full_01\\0x0FD39C10.wav", "0x0FD39C10", "s_m_y_sheriff_white_full_01"); } }
        public static ScannerFile HASH1046DF8A { get { return new ScannerFile("s_m_y_sheriff_01_white_full_01\\0x1046DF8A.wav", "0x1046DF8A", "s_m_y_sheriff_white_full_01"); } }
        public static ScannerFile HASH137027BC { get { return new ScannerFile("s_m_y_sheriff_01_white_full_01\\0x137027BC.wav", "0x137027BC", "s_m_y_sheriff_white_full_01"); } }
        public static ScannerFile HASH144A2876 { get { return new ScannerFile("s_m_y_sheriff_01_white_full_01\\0x144A2876.wav", "0x144A2876", "s_m_y_sheriff_white_full_01"); } }
        public static ScannerFile HASH1AF6E025 { get { return new ScannerFile("s_m_y_sheriff_01_white_full_01\\0x1AF6E025.wav", "0x1AF6E025", "s_m_y_sheriff_white_full_01"); } }
        public static ScannerFile HASH1B2B551D { get { return new ScannerFile("s_m_y_sheriff_01_white_full_01\\0x1B2B551D.wav", "0x1B2B551D", "s_m_y_sheriff_white_full_01"); } }
        public static ScannerFile HASH1BD2FDBA { get { return new ScannerFile("s_m_y_sheriff_01_white_full_01\\0x1BD2FDBA.wav", "0x1BD2FDBA", "s_m_y_sheriff_white_full_01"); } }
    }
    public class s_m_y_sheriff_white_full_02
    {
        public static ScannerFile HASH00FFC2DB { get { return new ScannerFile("s_m_y_sheriff_01_white_full_02\\0x00FFC2DB.wav", "0x00FFC2DB", "s_m_y_sheriff_white_full_02"); } }
        public static ScannerFile HASH017221AA { get { return new ScannerFile("s_m_y_sheriff_01_white_full_02\\0x017221AA.wav", "0x017221AA", "s_m_y_sheriff_white_full_02"); } }
        public static ScannerFile HASH01A10324 { get { return new ScannerFile("s_m_y_sheriff_01_white_full_02\\0x01A10324.wav", "0x01A10324", "s_m_y_sheriff_white_full_02"); } }
        public static ScannerFile HASH0846F27E { get { return new ScannerFile("s_m_y_sheriff_01_white_full_02\\0x0846F27E.wav", "0x0846F27E", "s_m_y_sheriff_white_full_02"); } }
        public static ScannerFile HASH0AF015F7 { get { return new ScannerFile("s_m_y_sheriff_01_white_full_02\\0x0AF015F7.wav", "0x0AF015F7", "s_m_y_sheriff_white_full_02"); } }
        public static ScannerFile HASH0D3B44AE { get { return new ScannerFile("s_m_y_sheriff_01_white_full_02\\0x0D3B44AE.wav", "0x0D3B44AE", "s_m_y_sheriff_white_full_02"); } }
        public static ScannerFile HASH0D59A0C8 { get { return new ScannerFile("s_m_y_sheriff_01_white_full_02\\0x0D59A0C8.wav", "0x0D59A0C8", "s_m_y_sheriff_white_full_02"); } }
        public static ScannerFile HASH0ED8BFA8 { get { return new ScannerFile("s_m_y_sheriff_01_white_full_02\\0x0ED8BFA8.wav", "0x0ED8BFA8", "s_m_y_sheriff_white_full_02"); } }
        public static ScannerFile HASH0FD39C10 { get { return new ScannerFile("s_m_y_sheriff_01_white_full_02\\0x0FD39C10.wav", "0x0FD39C10", "s_m_y_sheriff_white_full_02"); } }
        public static ScannerFile HASH1046DF8A { get { return new ScannerFile("s_m_y_sheriff_01_white_full_02\\0x1046DF8A.wav", "0x1046DF8A", "s_m_y_sheriff_white_full_02"); } }
        public static ScannerFile HASH137027BC { get { return new ScannerFile("s_m_y_sheriff_01_white_full_02\\0x137027BC.wav", "0x137027BC", "s_m_y_sheriff_white_full_02"); } }
        public static ScannerFile HASH144A2876 { get { return new ScannerFile("s_m_y_sheriff_01_white_full_02\\0x144A2876.wav", "0x144A2876", "s_m_y_sheriff_white_full_02"); } }
        public static ScannerFile HASH1AF6E025 { get { return new ScannerFile("s_m_y_sheriff_01_white_full_02\\0x1AF6E025.wav", "0x1AF6E025", "s_m_y_sheriff_white_full_02"); } }
        public static ScannerFile HASH1B2B551D { get { return new ScannerFile("s_m_y_sheriff_01_white_full_02\\0x1B2B551D.wav", "0x1B2B551D", "s_m_y_sheriff_white_full_02"); } }
        public static ScannerFile HASH1BD2FDBA { get { return new ScannerFile("s_m_y_sheriff_01_white_full_02\\0x1BD2FDBA.wav", "0x1BD2FDBA", "s_m_y_sheriff_white_full_02"); } }
        public static ScannerFile HASH1E193B2F { get { return new ScannerFile("s_m_y_sheriff_01_white_full_02\\0x1E193B2F.wav", "0x1E193B2F", "s_m_y_sheriff_white_full_02"); } }
        public static ScannerFile HASH1E99399B { get { return new ScannerFile("s_m_y_sheriff_01_white_full_02\\0x1E99399B.wav", "0x1E99399B", "s_m_y_sheriff_white_full_02"); } }
    }
    public class SpeedQuotes
    {
        public static ScannerFile PopQuizHotShot { get { return new ScannerFile("SpeedQuotes\\PopQuizHotShot.wav", "Pop quiz hot shot...", "Speed"); } }
    }
    public class AudioBeeps
    {
        public static ScannerFile Radio_Start_1 { get { return new ScannerFile("01_radio_beep\\Radio_Start_1.wav", "Radio_Start_1", "Radio_Start_1"); } }
        public static ScannerFile Radio_Start_2 { get { return new ScannerFile("01_radio_beep\\Radio_Start_2.wav", "Radio_Start_2", "Radio_Start_2"); } }
        public static ScannerFile Radio_End_1 { get { return new ScannerFile("01_radio_beep\\Radio_End_1.wav", "Radio_End_1", "Radio_End_1"); } }
        public static ScannerFile Radio_End_2 { get { return new ScannerFile("01_radio_beep\\Radio_End_2.wav", "Radio_End_2", "Radio_End_2"); } }
    }
    public class ScannerFile
    {
        public ScannerFile(string _FileName, string _Quote, string _GroupName)
        {
            FileName = _FileName;
            Quote = _Quote;
            GroupName = _GroupName;
        }
        public string FileName { get; set; }
        public string Quote { get; set; }
        public string GroupName { get; set; }
    }
}

