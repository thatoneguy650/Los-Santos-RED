using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class Streets
{
    public static List<Street> StreetsList;
    public static void Initialize()
    {
        StreetsList = new List<Street>
        {
            new Street("Joshua Rd", 50f, ScannerAudio.streets.JoshuaRoad.FileName),
            new Street("East Joshua Road", 50f, ScannerAudio.streets.EastJoshuaRoad.FileName),
            new Street("Marina Dr", 35f, ScannerAudio.streets.MarinaDrive.FileName),
            new Street("Alhambra Dr", 35f, ScannerAudio.streets.ElHamberDrive.FileName),
            new Street("Niland Ave", 35f, ScannerAudio.streets.NeelanAve.FileName),
            new Street("Zancudo Ave", 35f, ScannerAudio.streets.ZancudoAve.FileName),
            new Street("Armadillo Ave", 35f, ScannerAudio.streets.ArmadilloAve.FileName),
            new Street("Algonquin Blvd", 35f, ScannerAudio.streets.AlgonquinBlvd.FileName),
            new Street("Mountain View Dr", 35f, ScannerAudio.streets.MountainViewDrive.FileName),
            new Street("Cholla Springs Ave", 35f, ScannerAudio.streets.ChollaSpringsAve.FileName),
            new Street("Panorama Dr", 40f, ScannerAudio.streets.PanoramaDrive.FileName),
            new Street("Lesbos Ln", 35f, ScannerAudio.streets.LesbosLane.FileName),
            new Street("Calafia Rd", 30f, ScannerAudio.streets.CalapiaRoad.FileName),
            new Street("North Calafia Way", 30f, ScannerAudio.streets.NorthKalafiaWay.FileName),
            new Street("Cassidy Trail", 25f, ScannerAudio.streets.CassidyTrail.FileName),
            new Street("Seaview Rd", 35f, ScannerAudio.streets.SeaviewRd.FileName),
            new Street("Grapeseed Main St", 35f, ScannerAudio.streets.GrapseedMainStreet.FileName),
            new Street("Grapeseed Ave", 35f, ScannerAudio.streets.GrapeseedAve.FileName),
            new Street("Joad Ln", 35f, ScannerAudio.streets.JilledLane.FileName),
            new Street("Union Rd", 40f, ScannerAudio.streets.UnionRoad.FileName),
            new Street("O'Neil Way", 25f, ScannerAudio.streets.OneilWay.FileName),
            new Street("Senora Fwy", 65f, ScannerAudio.streets.SonoraFreeway.FileName, true),
            new Street("Catfish View", 35f, ScannerAudio.streets.CatfishView.FileName),
            new Street("Great Ocean Hwy", 60f, ScannerAudio.streets.GreatOceanHighway.FileName, true),
            new Street("Paleto Blvd", 35f, ScannerAudio.streets.PaletoBlvd.FileName),
            new Street("Duluoz Ave", 35f, ScannerAudio.streets.DelouasAve.FileName),
            new Street("Procopio Dr", 35f, ScannerAudio.streets.ProcopioDrive.FileName),
            new Street("Cascabel Ave", 30f),
            new Street("Peaceful St", 30f,ScannerAudio.streets.PeacefulStreet.FileName),
            new Street("Procopio Promenade", 25f, ScannerAudio.streets.ProcopioPromenade.FileName),
            new Street("Pyrite Ave", 30f, ScannerAudio.streets.PyriteAve.FileName),
            new Street("Fort Zancudo Approach Rd", 25f, ScannerAudio.streets.FortZancudoApproachRoad.FileName),
            new Street("Barbareno Rd", 30f, ScannerAudio.streets.BarbarinoRoad.FileName),
            new Street("Ineseno Road", 30f, ScannerAudio.streets.EnecinoRoad.FileName),
            new Street("West Eclipse Blvd", 35f, ScannerAudio.streets.WestEclipseBlvd.FileName),
            new Street("Playa Vista", 30f, ScannerAudio.streets.PlayaVista.FileName),
            new Street("Bay City Ave", 30f, ScannerAudio.streets.BaseCityAve.FileName),
            new Street("Del Perro Fwy", 65f, ScannerAudio.streets.DelPierroFreeway.FileName, true),
            new Street("Equality Way", 30f, ScannerAudio.streets.EqualityWay.FileName),
            new Street("Red Desert Ave", 30f, ScannerAudio.streets.RedDesertAve.FileName),
            new Street("Magellan Ave", 25f, ScannerAudio.streets.MagellanAve.FileName),
            new Street("Sandcastle Way", 30f, ScannerAudio.streets.SandcastleWay.FileName),
            new Street("Vespucci Blvd", 40f, ScannerAudio.streets.VespucciBlvd.FileName),
            new Street("Prosperity St", 30f, ScannerAudio.streets.ProsperityStreet.FileName),
            new Street("San Andreas Ave", 40f, ScannerAudio.streets.SanAndreasAve.FileName),
            new Street("North Rockford Dr", 35f, ScannerAudio.streets.NorthRockfordDrive.FileName),
            new Street("South Rockford Dr", 35f, ScannerAudio.streets.SouthRockfordDrive.FileName),
            new Street("Marathon Ave", 30f, ScannerAudio.streets.MarathonAve.FileName),
            new Street("Boulevard Del Perro", 35f, ScannerAudio.streets.BlvdDelPierro.FileName),
            new Street("Cougar Ave", 30f, ScannerAudio.streets.CougarAve.FileName),
            new Street("Liberty St", 30f, ScannerAudio.streets.LibertyStreet.FileName),
            new Street("Bay City Incline", 40f, ScannerAudio.streets.BaseCityIncline.FileName),
            new Street("Conquistador St", 25f, ScannerAudio.streets.ConquistadorStreet.FileName),
            new Street("Cortes St", 25f, ScannerAudio.streets.CortezStreet.FileName),
            new Street("Vitus St", 25f, ScannerAudio.streets.VitasStreet.FileName),
            new Street("Aguja St", 25f, ScannerAudio.streets.ElGouhaStreet.FileName),/////maytbe????!?!?!
            new Street("Goma St", 25f, ScannerAudio.streets.GomezStreet.FileName),
            new Street("Melanoma St", 25f, ScannerAudio.streets.MelanomaStreet.FileName),
            new Street("Palomino Ave", 35f, ScannerAudio.streets.PalaminoAve.FileName),
            new Street("Invention Ct", 25f, ScannerAudio.streets.InventionCourt.FileName),
            new Street("Imagination Ct", 25f, ScannerAudio.streets.ImaginationCourt.FileName),
            new Street("Rub St", 25f, ScannerAudio.streets.RubStreet.FileName),
            new Street("Tug St", 25f, ScannerAudio.streets.TugStreet.FileName),
            new Street("Ginger St", 30f, ScannerAudio.streets.GingerStreet.FileName),
            new Street("Lindsay Circus", 30f, ScannerAudio.streets.LindsayCircus.FileName),
            new Street("Calais Ave", 35f, ScannerAudio.streets.CaliasAve.FileName),
            new Street("Adam's Apple Blvd", 40f, ScannerAudio.streets.AdamsAppleBlvd.FileName),
            new Street("Alta St", 40f, ScannerAudio.streets.AlterStreet.FileName),
            new Street("Integrity Way", 30f, ScannerAudio.streets.IntergrityWy.FileName),
            new Street("Swiss St", 30f, ScannerAudio.streets.SwissStreet.FileName),
            new Street("Strawberry Ave", 40f, ScannerAudio.streets.StrawberryAve.FileName),
            new Street("Capital Blvd", 30f, ScannerAudio.streets.CapitalBlvd.FileName),
            new Street("Crusade Rd", 30f, ScannerAudio.streets.CrusadeRoad.FileName),
            new Street("Innocence Blvd", 40f, ScannerAudio.streets.InnocenceBlvd.FileName),
            new Street("Davis Ave", 40f, ScannerAudio.streets.DavisAve.FileName),
            new Street("Little Bighorn Ave", 35f, ScannerAudio.streets.LittleBighornAve.FileName),
            new Street("Roy Lowenstein Blvd", 35f, ScannerAudio.streets.RoyLowensteinBlvd.FileName),
            new Street("Jamestown St", 30f, ScannerAudio.streets.JamestownStreet.FileName),
            new Street("Carson Ave", 35f, ScannerAudio.streets.CarsonAve.FileName),
            new Street("Grove St", 30f, ScannerAudio.streets.GroveStreet.FileName),
            new Street("Brouge Ave", 30f),
            new Street("Covenant Ave", 30f, ScannerAudio.streets.CovenantAve.FileName),
            new Street("Dutch London St", 40f, ScannerAudio.streets.DutchLondonStreet.FileName),
            new Street("Signal St", 30f, ScannerAudio.streets.SignalStreet.FileName),
            new Street("Elysian Fields Fwy", 50f, ScannerAudio.streets.ElysianFieldsFreeway.FileName, true),
            new Street("Plaice Pl", 30f),
            new Street("Chum St", 40f, ScannerAudio.streets.ChumStreet.FileName),
            new Street("Chupacabra St", 30f),
            new Street("Miriam Turner Overpass", 30f, ScannerAudio.streets.MiriamTurnerOverpass.FileName),
            new Street("Autopia Pkwy", 35f, ScannerAudio.streets.AltopiaParkway.FileName),
            new Street("Exceptionalists Way", 35f, ScannerAudio.streets.ExceptionalistWay.FileName),
            new Street("La Puerta Fwy", 60f, "", true),
            new Street("New Empire Way", 30f, ScannerAudio.streets.NewEmpireWay.FileName),
            new Street("Runway1", 90f, ScannerAudio.streets.RunwayOne.FileName),
            new Street("Greenwich Pkwy", 35f, ScannerAudio.streets.GrenwichParkway.FileName),
            new Street("Kortz Dr", 30f, ScannerAudio.streets.KortzDrive.FileName),
            new Street("Banham Canyon Dr", 40f, ScannerAudio.streets.BanhamCanyonDrive.FileName),
            new Street("Buen Vino Rd", 40f),
            new Street("Route 68", 55f, ScannerAudio.streets.Route68.FileName),
            new Street("Zancudo Grande Valley", 40f, ScannerAudio.streets.ZancudoGrandeValley.FileName),
            new Street("Zancudo Barranca", 40f, ScannerAudio.streets.ZancudoBaranca.FileName),
            new Street("Galileo Rd", 40f, ScannerAudio.streets.GallileoRoad.FileName),
            new Street("Mt Vinewood Dr", 40f, ScannerAudio.streets.MountVinewoodDrive.FileName),
            new Street("Marlowe Dr", 40f),
            new Street("Milton Rd", 35f, ScannerAudio.streets.MiltonRoad.FileName),
            new Street("Kimble Hill Dr", 35f, ScannerAudio.streets.KimbalHillDrive.FileName),
            new Street("Normandy Dr", 35f, ScannerAudio.streets.NormandyDrive.FileName),
            new Street("Hillcrest Ave", 35f, ScannerAudio.streets.HillcrestAve.FileName),
            new Street("Hillcrest Ridge Access Rd", 35f, ScannerAudio.streets.HillcrestRidgeAccessRoad.FileName),
            new Street("North Sheldon Ave", 35f, ScannerAudio.streets.NorthSheldonAve.FileName),
            new Street("Lake Vinewood Dr", 35f, ScannerAudio.streets.LakeVineWoodDrive.FileName),
            new Street("Lake Vinewood Est", 35f, ScannerAudio.streets.LakeVinewoodEstate.FileName),
            new Street("Baytree Canyon Rd", 40f, ScannerAudio.streets.BaytreeCanyonRoad.FileName),
            new Street("North Conker Ave", 35f, ScannerAudio.streets.NorthConkerAve.FileName),
            new Street("Wild Oats Dr", 35f, ScannerAudio.streets.WildOatsDrive.FileName),
            new Street("Whispymound Dr", 35f, ScannerAudio.streets.WispyMoundDrive.FileName),
            new Street("Didion Dr", 35f, ScannerAudio.streets.DiedianDrive.FileName),
            new Street("Cox Way", 35f, ScannerAudio.streets.CoxWay.FileName),
            new Street("Picture Perfect Drive", 35f, ScannerAudio.streets.PicturePerfectDrive.FileName),
            new Street("South Mo Milton Dr", 35f, ScannerAudio.streets.SouthMoMiltonDrive.FileName),
            new Street("Cockingend Dr", 35f, ScannerAudio.streets.CockandGinDrive.FileName),
            new Street("Mad Wayne Thunder Dr", 35f, ScannerAudio.streets.MagwavevendorDrive.FileName),
            new Street("Hangman Ave", 35f, ScannerAudio.streets.HangmanAve.FileName),
            new Street("Dunstable Ln", 35f, ScannerAudio.streets.DunstableLane.FileName),
            new Street("Dunstable Dr", 35f, ScannerAudio.streets.DunstableDrive.FileName),
            new Street("Greenwich Way", 35f, ScannerAudio.streets.GrenwichWay.FileName),
            new Street("Greenwich Pl", 35f, ScannerAudio.streets.GrunnichPlace.FileName),
            new Street("Hardy Way", 35f),
            new Street("Richman St", 35f, ScannerAudio.streets.RichmondStreet.FileName),
            new Street("Ace Jones Dr", 35f, ScannerAudio.streets.AceJonesDrive.FileName),
            new Street("Los Santos Freeway", 65f, "", true),
            new Street("Senora Rd", 40f, ScannerAudio.streets.SonoraRoad.FileName),
            new Street("Nowhere Rd", 25f, ScannerAudio.streets.NowhereRoad.FileName),
            new Street("Smoke Tree Rd", 35f, ScannerAudio.streets.SmokeTreeRoad.FileName),
            new Street("Cholla Rd", 35f, ScannerAudio.streets.ChollaRoad.FileName),
            new Street("Cat-Claw Ave", 35f, ScannerAudio.streets.CatClawAve.FileName),
            new Street("Senora Way", 40f, ScannerAudio.streets.SonoraWay.FileName),
            new Street("Palomino Fwy", 60f, ScannerAudio.streets.PaliminoFreeway.FileName, true),
            new Street("Shank St", 25f, ScannerAudio.streets.ShankStreet.FileName),
            new Street("Macdonald St", 35f, ScannerAudio.streets.McDonaldStreet.FileName),
            new Street("Route 68 Approach", 55f, ScannerAudio.streets.Route68.FileName),
            new Street("Vinewood Park Dr", 35f, ScannerAudio.streets.VinewoodParkDrive.FileName),
            new Street("Vinewood Blvd", 40f, ScannerAudio.streets.VinewoodBlvd.FileName),
            new Street("Mirror Park Blvd", 35f, ScannerAudio.streets.MirrorParkBlvd.FileName),
            new Street("Glory Way", 35f, ScannerAudio.streets.GloryWay.FileName),
            new Street("Bridge St", 35f, ScannerAudio.streets.BridgeStreet.FileName),
            new Street("West Mirror Drive", 35f, ScannerAudio.streets.WestMirrorDrive.FileName),
            new Street("Nikola Ave", 35f, ScannerAudio.streets.NicolaAve.FileName),
            new Street("East Mirror Dr", 35f, ScannerAudio.streets.EastMirrorDrive.FileName),
            new Street("Nikola Pl", 25f, ScannerAudio.streets.NikolaPlace.FileName),
            new Street("Mirror Pl", 35f, ScannerAudio.streets.MirrorPlace.FileName),
            new Street("El Rancho Blvd", 40f, ScannerAudio.streets.ElRanchoBlvd.FileName),
            new Street("Olympic Fwy", 60f, ScannerAudio.streets.OlympicFreeway.FileName, true),
            new Street("Fudge Ln", 25f, ScannerAudio.streets.FudgeLane.FileName),
            new Street("Amarillo Vista", 25f, ScannerAudio.streets.AmarilloVista.FileName),
            new Street("Labor Pl", 35f, ScannerAudio.streets.ForceLaborPlace.FileName),
            new Street("El Burro Blvd", 35f, ScannerAudio.streets.ElBurroBlvd.FileName),
            new Street("Sustancia Rd", 45f, ScannerAudio.streets.SustanciaRoad.FileName),
            new Street("South Shambles St", 30f, ScannerAudio.streets.SouthShambleStreet.FileName),
            new Street("Hanger Way", 30f, ScannerAudio.streets.HangarWay.FileName),
            new Street("Orchardville Ave", 30f, ScannerAudio.streets.OrchidvilleAve.FileName),
            new Street("Popular St", 40f, ScannerAudio.streets.PopularStreet.FileName),
            new Street("Buccaneer Way", 45f, ScannerAudio.streets.BuccanierWay.FileName),
            new Street("Abattoir Ave", 35f, ScannerAudio.streets.AvatorAve.FileName),
            new Street("Voodoo Place", 30f),
            new Street("Mutiny Rd", 35f, ScannerAudio.streets.MutineeRoad.FileName),
            new Street("South Arsenal St", 35f, ScannerAudio.streets.SouthArsenalStreet.FileName),
            new Street("Forum Dr", 35f, ScannerAudio.streets.ForumDrive.FileName),
            new Street("Morningwood Blvd", 35f, ScannerAudio.streets.MorningwoodBlvd.FileName),
            new Street("Dorset Dr", 40f, ScannerAudio.streets.DorsetDrive.FileName),
            new Street("Caesars Place", 25f, ScannerAudio.streets.CaesarPlace.FileName),
            new Street("Spanish Ave", 30f, ScannerAudio.streets.SpanishAve.FileName),
            new Street("Portola Dr", 30f, ScannerAudio.streets.PortolaDrive.FileName),
            new Street("Edwood Way", 25f, ScannerAudio.streets.EdwardWay.FileName),
            new Street("San Vitus Blvd", 40f, ScannerAudio.streets.SanVitusBlvd.FileName),
            new Street("Eclipse Blvd", 35f, ScannerAudio.streets.EclipseBlvd.FileName),
            new Street("Gentry Lane", 30f),
            new Street("Las Lagunas Blvd", 40f, ScannerAudio.streets.LasLegunasBlvd.FileName),
            new Street("Power St", 40f, ScannerAudio.streets.PowerStreet.FileName),
            new Street("Mt Haan Rd", 40f, ScannerAudio.streets.MtHaanRoad.FileName),
            new Street("Elgin Ave", 40f, ScannerAudio.streets.ElginAve.FileName),
            new Street("Hawick Ave", 35f, ScannerAudio.streets.HawickAve.FileName),
            new Street("Meteor St", 30f, ScannerAudio.streets.MeteorStreet.FileName),
            new Street("Alta Pl", 30f, ScannerAudio.streets.AltaPlace.FileName),
            new Street("Occupation Ave", 35f, ScannerAudio.streets.OccupationAve.FileName),
            new Street("Carcer Way", 40f, ScannerAudio.streets.CarcerWay.FileName),
            new Street("Eastbourne Way", 30f, ScannerAudio.streets.EastbourneWay.FileName),
            new Street("Rockford Dr", 35f, ScannerAudio.streets.RockfordDrive.FileName),
            new Street("Abe Milton Pkwy", 35f, ScannerAudio.streets.EightMiltonParkway.FileName),
            new Street("Laguna Pl", 30f, ScannerAudio.streets.LagunaPlace.FileName),
            new Street("Sinners Passage", 30f, ScannerAudio.streets.SinnersPassage.FileName),
            new Street("Atlee St", 30f, ScannerAudio.streets.AtleyStreet.FileName),
            new Street("Sinner St", 30f, ScannerAudio.streets.SinnerStreet.FileName),
            new Street("Supply St", 30f, ScannerAudio.streets.SupplyStreet.FileName),
            new Street("Amarillo Way", 35f, ScannerAudio.streets.AmarilloWay.FileName),
            new Street("Tower Way", 35f, ScannerAudio.streets.TowerWay.FileName),
            new Street("Decker St", 35f, ScannerAudio.streets.DeckerStreet.FileName),
            new Street("Tackle St", 25f, ScannerAudio.streets.TackleStreet.FileName),
            new Street("Low Power St", 35f, ScannerAudio.streets.LowPowerStreet.FileName),
            new Street("Clinton Ave", 35f, ScannerAudio.streets.ClintonAve.FileName),
            new Street("Fenwell Pl", 35f, ScannerAudio.streets.FenwellPlace.FileName),
            new Street("Utopia Gardens", 25f, ScannerAudio.streets.UtopiaGardens.FileName),
            new Street("Cavalry Blvd", 35f),
            new Street("South Boulevard Del Perro", 35f, ScannerAudio.streets.SouthBlvdDelPierro.FileName),
            new Street("Americano Way", 25f, ScannerAudio.streets.AmericanoWay.FileName),
            new Street("Sam Austin Dr", 25f, ScannerAudio.streets.SamAustinDrive.FileName),
            new Street("East Galileo Ave", 35f, ScannerAudio.streets.EastGalileoAve.FileName),
            new Street("Galileo Park", 35f),
            new Street("West Galileo Ave", 35f, ScannerAudio.streets.WestGalileoAve.FileName),
            new Street("Tongva Dr", 40f, ScannerAudio.streets.TongvaDrive.FileName),
            new Street("Zancudo Rd", 35f, ScannerAudio.streets.ZancudoRoad.FileName),
            new Street("Movie Star Way", 35f, ScannerAudio.streets.MovieStarWay.FileName),
            new Street("Heritage Way", 35f, ScannerAudio.streets.HeritageWay.FileName),
            new Street("Perth St", 25f, ScannerAudio.streets.PerfStreet.FileName),
            new Street("Chianski Passage", 30f),
            new Street("Lolita Ave", 35f, ScannerAudio.streets.LolitaAve.FileName),
            new Street("Meringue Ln", 35f, ScannerAudio.streets.MirangeLane.FileName),
            new Street("Strangeways Dr", 30f, ScannerAudio.streets.StrangeWaysDrive.FileName)
        };
    }
    public static void Dispose()
    {

    }

    public static Street GetStreetFromName(string StreetName)
    {
        return StreetsList.Where(x => x.Name == StreetName).FirstOrDefault();
    }
}
public class Street
{
    public string Name = "";
    public float SpeedLimit = 50f;
    public string DispatchFile = "";
    public bool isFreeway = false;
    public Street(string _Name, float _SpeedLimit)
    {
        Name = _Name;
        SpeedLimit = _SpeedLimit;
    }
    public Street(string _Name, float _SpeedLimit, string _DispatchFile)
    {
        Name = _Name;
        SpeedLimit = _SpeedLimit;
        DispatchFile = _DispatchFile;
    }
    public Street(string _Name, float _SpeedLimit, string _DispatchFile, bool _isFreeway)
    {
        Name = _Name;
        SpeedLimit = _SpeedLimit;
        DispatchFile = _DispatchFile;
        isFreeway = _isFreeway;
    }
}

