using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class Streets
{
    public static List<Street> StreetsList = new List<Street>();
    public static void Initialize()
    {
        StreetsList.Add(new Street("Joshua Rd", 50f, ScannerAudio.streets.JoshuaRoad.FileName));
        StreetsList.Add(new Street("East Joshua Road", 50f, ScannerAudio.streets.EastJoshuaRoad.FileName));
        StreetsList.Add(new Street("Marina Dr", 35f, ScannerAudio.streets.MarinaDrive.FileName));
        StreetsList.Add(new Street("Alhambra Dr", 35f, ScannerAudio.streets.ElHamberDrive.FileName));
        StreetsList.Add(new Street("Niland Ave", 35f, ScannerAudio.streets.NeelanAve.FileName));
        StreetsList.Add(new Street("Zancudo Ave", 35f, ScannerAudio.streets.ZancudoAve.FileName));
        StreetsList.Add(new Street("Armadillo Ave", 35f, ScannerAudio.streets.ArmadilloAve.FileName));
        StreetsList.Add(new Street("Algonquin Blvd", 35f, ScannerAudio.streets.AlgonquinBlvd.FileName));
        StreetsList.Add(new Street("Mountain View Dr", 35f, ScannerAudio.streets.MountainViewDrive.FileName));
        StreetsList.Add(new Street("Cholla Springs Ave", 35f, ScannerAudio.streets.ChollaSpringsAve.FileName));
        StreetsList.Add(new Street("Panorama Dr", 40f, ScannerAudio.streets.PanoramaDrive.FileName));
        StreetsList.Add(new Street("Lesbos Ln", 35f, ScannerAudio.streets.LesbosLane.FileName));
        StreetsList.Add(new Street("Calafia Rd", 30f, ScannerAudio.streets.CalapiaRoad.FileName));
        StreetsList.Add(new Street("North Calafia Way", 30f, ScannerAudio.streets.NorthKalafiaWay.FileName));
        StreetsList.Add(new Street("Cassidy Trail", 25f, ScannerAudio.streets.CassidyTrail.FileName));
        StreetsList.Add(new Street("Seaview Rd", 35f, ScannerAudio.streets.SeaviewRd.FileName));
        StreetsList.Add(new Street("Grapeseed Main St", 35f, ScannerAudio.streets.GrapseedMainStreet.FileName));
        StreetsList.Add(new Street("Grapeseed Ave", 35f, ScannerAudio.streets.GrapeseedAve.FileName));
        StreetsList.Add(new Street("Joad Ln", 35f, ScannerAudio.streets.JilledLane.FileName));
        StreetsList.Add(new Street("Union Rd", 40f, ScannerAudio.streets.UnionRoad.FileName));
        StreetsList.Add(new Street("O'Neil Way", 25f, ScannerAudio.streets.OneilWay.FileName));
        StreetsList.Add(new Street("Senora Fwy", 65f, ScannerAudio.streets.SonoraFreeway.FileName, true));
        StreetsList.Add(new Street("Catfish View", 35f, ScannerAudio.streets.CatfishView.FileName));
        StreetsList.Add(new Street("Great Ocean Hwy", 60f, ScannerAudio.streets.GreatOceanHighway.FileName, true));
        StreetsList.Add(new Street("Paleto Blvd", 35f, ScannerAudio.streets.PaletoBlvd.FileName));
        StreetsList.Add(new Street("Duluoz Ave", 35f, ScannerAudio.streets.DelouasAve.FileName));
        StreetsList.Add(new Street("Procopio Dr", 35f, ScannerAudio.streets.ProcopioDrive.FileName));
        StreetsList.Add(new Street("Cascabel Ave", 30f));
        StreetsList.Add(new Street("Procopio Promenade", 25f, ScannerAudio.streets.ProcopioPromenade.FileName));
        StreetsList.Add(new Street("Pyrite Ave", 30f, ScannerAudio.streets.PyriteAve.FileName));
        StreetsList.Add(new Street("Fort Zancudo Approach Rd", 25f, ScannerAudio.streets.FortZancudoApproachRoad.FileName));
        StreetsList.Add(new Street("Barbareno Rd", 30f, ScannerAudio.streets.BarbarinoRoad.FileName));
        StreetsList.Add(new Street("Ineseno Road", 30f, ScannerAudio.streets.EnecinoRoad.FileName));
        StreetsList.Add(new Street("West Eclipse Blvd", 35f, ScannerAudio.streets.WestEclipseBlvd.FileName));
        StreetsList.Add(new Street("Playa Vista", 30f, ScannerAudio.streets.PlayaVista.FileName));
        StreetsList.Add(new Street("Bay City Ave", 30f, ScannerAudio.streets.BaseCityAve.FileName));
        StreetsList.Add(new Street("Del Perro Fwy", 65f, ScannerAudio.streets.DelPierroFreeway.FileName, true));
        StreetsList.Add(new Street("Equality Way", 30f, ScannerAudio.streets.EqualityWay.FileName));
        StreetsList.Add(new Street("Red Desert Ave", 30f, ScannerAudio.streets.RedDesertAve.FileName));
        StreetsList.Add(new Street("Magellan Ave", 25f, ScannerAudio.streets.MagellanAve.FileName));
        StreetsList.Add(new Street("Sandcastle Way", 30f, ScannerAudio.streets.SandcastleWay.FileName));
        StreetsList.Add(new Street("Vespucci Blvd", 40f, ScannerAudio.streets.VespucciBlvd.FileName));
        StreetsList.Add(new Street("Prosperity St", 30f, ScannerAudio.streets.ProsperityStreet.FileName));
        StreetsList.Add(new Street("San Andreas Ave", 40f, ScannerAudio.streets.SanAndreasAve.FileName));
        StreetsList.Add(new Street("North Rockford Dr", 35f, ScannerAudio.streets.NorthRockfordDrive.FileName));
        StreetsList.Add(new Street("South Rockford Dr", 35f, ScannerAudio.streets.SouthRockfordDrive.FileName));
        StreetsList.Add(new Street("Marathon Ave", 30f, ScannerAudio.streets.MarathonAve.FileName));
        StreetsList.Add(new Street("Boulevard Del Perro", 35f, ScannerAudio.streets.BlvdDelPierro.FileName));
        StreetsList.Add(new Street("Cougar Ave", 30f, ScannerAudio.streets.CougarAve.FileName));
        StreetsList.Add(new Street("Liberty St", 30f, ScannerAudio.streets.LibertyStreet.FileName));
        StreetsList.Add(new Street("Bay City Incline", 40f, ScannerAudio.streets.BaseCityIncline.FileName));
        StreetsList.Add(new Street("Conquistador St", 25f, ScannerAudio.streets.ConquistadorStreet.FileName));
        StreetsList.Add(new Street("Cortes St", 25f, ScannerAudio.streets.CortezStreet.FileName));
        StreetsList.Add(new Street("Vitus St", 25f, ScannerAudio.streets.VitasStreet.FileName));
        StreetsList.Add(new Street("Aguja St", 25f, ScannerAudio.streets.ElGouhaStreet.FileName));/////maytbe????!?!?!
        StreetsList.Add(new Street("Goma St", 25f, ScannerAudio.streets.GomezStreet.FileName));
        StreetsList.Add(new Street("Melanoma St", 25f, ScannerAudio.streets.MelanomaStreet.FileName));
        StreetsList.Add(new Street("Palomino Ave", 35f, ScannerAudio.streets.PalaminoAve.FileName));
        StreetsList.Add(new Street("Invention Ct", 25f, ScannerAudio.streets.InventionCourt.FileName));
        StreetsList.Add(new Street("Imagination Ct", 25f, ScannerAudio.streets.ImaginationCourt.FileName));
        StreetsList.Add(new Street("Rub St", 25f, ScannerAudio.streets.RubStreet.FileName));
        StreetsList.Add(new Street("Tug St", 25f, ScannerAudio.streets.TugStreet.FileName));
        StreetsList.Add(new Street("Ginger St", 30f, ScannerAudio.streets.GingerStreet.FileName));
        StreetsList.Add(new Street("Lindsay Circus", 30f, ScannerAudio.streets.LindsayCircus.FileName));
        StreetsList.Add(new Street("Calais Ave", 35f, ScannerAudio.streets.CaliasAve.FileName));
        StreetsList.Add(new Street("Adam's Apple Blvd", 40f, ScannerAudio.streets.AdamsAppleBlvd.FileName));
        StreetsList.Add(new Street("Alta St", 40f, ScannerAudio.streets.AlterStreet.FileName));
        StreetsList.Add(new Street("Integrity Way", 30f, ScannerAudio.streets.IntergrityWy.FileName));
        StreetsList.Add(new Street("Swiss St", 30f, ScannerAudio.streets.SwissStreet.FileName));
        StreetsList.Add(new Street("Strawberry Ave", 40f, ScannerAudio.streets.StrawberryAve.FileName));
        StreetsList.Add(new Street("Capital Blvd", 30f, ScannerAudio.streets.CapitalBlvd.FileName));
        StreetsList.Add(new Street("Crusade Rd", 30f, ScannerAudio.streets.CrusadeRoad.FileName));
        StreetsList.Add(new Street("Innocence Blvd", 40f, ScannerAudio.streets.InnocenceBlvd.FileName));
        StreetsList.Add(new Street("Davis Ave", 40f, ScannerAudio.streets.DavisAve.FileName));
        StreetsList.Add(new Street("Little Bighorn Ave", 35f, ScannerAudio.streets.LittleBighornAve.FileName));
        StreetsList.Add(new Street("Roy Lowenstein Blvd", 35f, ScannerAudio.streets.RoyLowensteinBlvd.FileName));
        StreetsList.Add(new Street("Jamestown St", 30f, ScannerAudio.streets.JamestownStreet.FileName));
        StreetsList.Add(new Street("Carson Ave", 35f, ScannerAudio.streets.CarsonAve.FileName));
        StreetsList.Add(new Street("Grove St", 30f, ScannerAudio.streets.GroveStreet.FileName));
        StreetsList.Add(new Street("Brouge Ave", 30f));
        StreetsList.Add(new Street("Covenant Ave", 30f, ScannerAudio.streets.CovenantAve.FileName));
        StreetsList.Add(new Street("Dutch London St", 40f, ScannerAudio.streets.DutchLondonStreet.FileName));
        StreetsList.Add(new Street("Signal St", 30f, ScannerAudio.streets.SignalStreet.FileName));
        StreetsList.Add(new Street("Elysian Fields Fwy", 50f, ScannerAudio.streets.ElysianFieldsFreeway.FileName, true));
        StreetsList.Add(new Street("Plaice Pl", 30f));
        StreetsList.Add(new Street("Chum St", 40f, ScannerAudio.streets.ChumStreet.FileName));
        StreetsList.Add(new Street("Chupacabra St", 30f));
        StreetsList.Add(new Street("Miriam Turner Overpass", 30f, ScannerAudio.streets.MiriamTurnerOverpass.FileName));
        StreetsList.Add(new Street("Autopia Pkwy", 35f, ScannerAudio.streets.AltopiaParkway.FileName));
        StreetsList.Add(new Street("Exceptionalists Way", 35f, ScannerAudio.streets.ExceptionalistWay.FileName));
        StreetsList.Add(new Street("La Puerta Fwy", 60f, "", true));
        StreetsList.Add(new Street("New Empire Way", 30f, ScannerAudio.streets.NewEmpireWay.FileName));
        StreetsList.Add(new Street("Runway1", 90f, ScannerAudio.streets.RunwayOne.FileName));
        StreetsList.Add(new Street("Greenwich Pkwy", 35f, ScannerAudio.streets.GrenwichParkway.FileName));
        StreetsList.Add(new Street("Kortz Dr", 30f, ScannerAudio.streets.KortzDrive.FileName));
        StreetsList.Add(new Street("Banham Canyon Dr", 40f, ScannerAudio.streets.BanhamCanyonDrive.FileName));
        StreetsList.Add(new Street("Buen Vino Rd", 40f));
        StreetsList.Add(new Street("Route 68", 55f, ScannerAudio.streets.Route68.FileName));
        StreetsList.Add(new Street("Zancudo Grande Valley", 40f, ScannerAudio.streets.ZancudoGrandeValley.FileName));
        StreetsList.Add(new Street("Zancudo Barranca", 40f, ScannerAudio.streets.ZancudoBaranca.FileName));
        StreetsList.Add(new Street("Galileo Rd", 40f, ScannerAudio.streets.GallileoRoad.FileName));
        StreetsList.Add(new Street("Mt Vinewood Dr", 40f, ScannerAudio.streets.MountVinewoodDrive.FileName));
        StreetsList.Add(new Street("Marlowe Dr", 40f));
        StreetsList.Add(new Street("Milton Rd", 35f, ScannerAudio.streets.MiltonRoad.FileName));
        StreetsList.Add(new Street("Kimble Hill Dr", 35f, ScannerAudio.streets.KimbalHillDrive.FileName));
        StreetsList.Add(new Street("Normandy Dr", 35f, ScannerAudio.streets.NormandyDrive.FileName));
        StreetsList.Add(new Street("Hillcrest Ave", 35f, ScannerAudio.streets.HillcrestAve.FileName));
        StreetsList.Add(new Street("Hillcrest Ridge Access Rd", 35f, ScannerAudio.streets.HillcrestRidgeAccessRoad.FileName));
        StreetsList.Add(new Street("North Sheldon Ave", 35f, ScannerAudio.streets.NorthSheldonAve.FileName));
        StreetsList.Add(new Street("Lake Vinewood Dr", 35f, ScannerAudio.streets.LakeVineWoodDrive.FileName));
        StreetsList.Add(new Street("Lake Vinewood Est", 35f, ScannerAudio.streets.LakeVinewoodEstate.FileName));
        StreetsList.Add(new Street("Baytree Canyon Rd", 40f, ScannerAudio.streets.BaytreeCanyonRoad.FileName));
        StreetsList.Add(new Street("North Conker Ave", 35f, ScannerAudio.streets.NorthConkerAve.FileName));
        StreetsList.Add(new Street("Wild Oats Dr", 35f, ScannerAudio.streets.WildOatsDrive.FileName));
        StreetsList.Add(new Street("Whispymound Dr", 35f, ScannerAudio.streets.WispyMoundDrive.FileName));
        StreetsList.Add(new Street("Didion Dr", 35f, ScannerAudio.streets.DiedianDrive.FileName));
        StreetsList.Add(new Street("Cox Way", 35f, ScannerAudio.streets.CoxWay.FileName));
        StreetsList.Add(new Street("Picture Perfect Drive", 35f, ScannerAudio.streets.PicturePerfectDrive.FileName));
        StreetsList.Add(new Street("South Mo Milton Dr", 35f, ScannerAudio.streets.SouthMoMiltonDrive.FileName));
        StreetsList.Add(new Street("Cockingend Dr", 35f, ScannerAudio.streets.CockandGinDrive.FileName));
        StreetsList.Add(new Street("Mad Wayne Thunder Dr", 35f, ScannerAudio.streets.MagwavevendorDrive.FileName));
        StreetsList.Add(new Street("Hangman Ave", 35f, ScannerAudio.streets.HangmanAve.FileName));
        StreetsList.Add(new Street("Dunstable Ln", 35f, ScannerAudio.streets.DunstableLane.FileName));
        StreetsList.Add(new Street("Dunstable Dr", 35f, ScannerAudio.streets.DunstableDrive.FileName));
        StreetsList.Add(new Street("Greenwich Way", 35f, ScannerAudio.streets.GrenwichWay.FileName));
        StreetsList.Add(new Street("Greenwich Pl", 35f, ScannerAudio.streets.GrunnichPlace.FileName));
        StreetsList.Add(new Street("Hardy Way", 35f));
        StreetsList.Add(new Street("Richman St", 35f, ScannerAudio.streets.RichmondStreet.FileName));
        StreetsList.Add(new Street("Ace Jones Dr", 35f, ScannerAudio.streets.AceJonesDrive.FileName));
        StreetsList.Add(new Street("Los Santos Freeway", 65f, "", true));
        StreetsList.Add(new Street("Senora Rd", 40f, ScannerAudio.streets.SonoraRoad.FileName));
        StreetsList.Add(new Street("Nowhere Rd", 25f, ScannerAudio.streets.NowhereRoad.FileName));
        StreetsList.Add(new Street("Smoke Tree Rd", 35f, ScannerAudio.streets.SmokeTreeRoad.FileName));
        StreetsList.Add(new Street("Cholla Rd", 35f, ScannerAudio.streets.ChollaRoad.FileName));
        StreetsList.Add(new Street("Cat-Claw Ave", 35f, ScannerAudio.streets.CatClawAve.FileName));
        StreetsList.Add(new Street("Senora Way", 40f, ScannerAudio.streets.SonoraWay.FileName));
        StreetsList.Add(new Street("Palomino Fwy", 60f, ScannerAudio.streets.PaliminoFreeway.FileName, true));
        StreetsList.Add(new Street("Shank St", 25f, ScannerAudio.streets.ShankStreet.FileName));
        StreetsList.Add(new Street("Macdonald St", 35f, ScannerAudio.streets.McDonaldStreet.FileName));
        StreetsList.Add(new Street("Route 68 Approach", 55f, ScannerAudio.streets.Route68.FileName));
        StreetsList.Add(new Street("Vinewood Park Dr", 35f, ScannerAudio.streets.VinewoodParkDrive.FileName));
        StreetsList.Add(new Street("Vinewood Blvd", 40f, ScannerAudio.streets.VinewoodBlvd.FileName));
        StreetsList.Add(new Street("Mirror Park Blvd", 35f, ScannerAudio.streets.MirrorParkBlvd.FileName));
        StreetsList.Add(new Street("Glory Way", 35f, ScannerAudio.streets.GloryWay.FileName));
        StreetsList.Add(new Street("Bridge St", 35f, ScannerAudio.streets.BridgeStreet.FileName));
        StreetsList.Add(new Street("West Mirror Drive", 35f, ScannerAudio.streets.WestMirrorDrive.FileName));
        StreetsList.Add(new Street("Nikola Ave", 35f, ScannerAudio.streets.NicolaAve.FileName));
        StreetsList.Add(new Street("East Mirror Dr", 35f, ScannerAudio.streets.EastMirrorDrive.FileName));
        StreetsList.Add(new Street("Nikola Pl", 25f, ScannerAudio.streets.NikolaPlace.FileName));
        StreetsList.Add(new Street("Mirror Pl", 35f, ScannerAudio.streets.MirrorPlace.FileName));
        StreetsList.Add(new Street("El Rancho Blvd", 40f, ScannerAudio.streets.ElRanchoBlvd.FileName));
        StreetsList.Add(new Street("Olympic Fwy", 60f, ScannerAudio.streets.OlympicFreeway.FileName, true));
        StreetsList.Add(new Street("Fudge Ln", 25f, ScannerAudio.streets.FudgeLane.FileName));
        StreetsList.Add(new Street("Amarillo Vista", 25f, ScannerAudio.streets.AmarilloVista.FileName));
        StreetsList.Add(new Street("Labor Pl", 35f, ScannerAudio.streets.ForceLaborPlace.FileName));
        StreetsList.Add(new Street("El Burro Blvd", 35f, ScannerAudio.streets.ElBurroBlvd.FileName));
        StreetsList.Add(new Street("Sustancia Rd", 45f, ScannerAudio.streets.SustanciaRoad.FileName));
        StreetsList.Add(new Street("South Shambles St", 30f, ScannerAudio.streets.SouthShambleStreet.FileName));
        StreetsList.Add(new Street("Hanger Way", 30f, ScannerAudio.streets.HangarWay.FileName));
        StreetsList.Add(new Street("Orchardville Ave", 30f, ScannerAudio.streets.OrchidvilleAve.FileName));
        StreetsList.Add(new Street("Popular St", 40f, ScannerAudio.streets.PopularStreet.FileName));
        StreetsList.Add(new Street("Buccaneer Way", 45f, ScannerAudio.streets.BuccanierWay.FileName));
        StreetsList.Add(new Street("Abattoir Ave", 35f, ScannerAudio.streets.AvatorAve.FileName));
        StreetsList.Add(new Street("Voodoo Place", 30f));
        StreetsList.Add(new Street("Mutiny Rd", 35f, ScannerAudio.streets.MutineeRoad.FileName));
        StreetsList.Add(new Street("South Arsenal St", 35f, ScannerAudio.streets.SouthArsenalStreet.FileName));
        StreetsList.Add(new Street("Forum Dr", 35f, ScannerAudio.streets.ForumDrive.FileName));
        StreetsList.Add(new Street("Morningwood Blvd", 35f, ScannerAudio.streets.MorningwoodBlvd.FileName));
        StreetsList.Add(new Street("Dorset Dr", 40f, ScannerAudio.streets.DorsetDrive.FileName));
        StreetsList.Add(new Street("Caesars Place", 25f, ScannerAudio.streets.CaesarPlace.FileName));
        StreetsList.Add(new Street("Spanish Ave", 30f, ScannerAudio.streets.SpanishAve.FileName));
        StreetsList.Add(new Street("Portola Dr", 30f, ScannerAudio.streets.PortolaDrive.FileName));
        StreetsList.Add(new Street("Edwood Way", 25f, ScannerAudio.streets.EdwardWay.FileName));
        StreetsList.Add(new Street("San Vitus Blvd", 40f, ScannerAudio.streets.SanVitusBlvd.FileName));
        StreetsList.Add(new Street("Eclipse Blvd", 35f, ScannerAudio.streets.EclipseBlvd.FileName));
        StreetsList.Add(new Street("Gentry Lane", 30f));
        StreetsList.Add(new Street("Las Lagunas Blvd", 40f, ScannerAudio.streets.LasLegunasBlvd.FileName));
        StreetsList.Add(new Street("Power St", 40f, ScannerAudio.streets.PowerStreet.FileName));
        StreetsList.Add(new Street("Mt Haan Rd", 40f, ScannerAudio.streets.MtHaanRoad.FileName));
        StreetsList.Add(new Street("Elgin Ave", 40f, ScannerAudio.streets.ElginAve.FileName));
        StreetsList.Add(new Street("Hawick Ave", 35f, ScannerAudio.streets.HawickAve.FileName));
        StreetsList.Add(new Street("Meteor St", 30f, ScannerAudio.streets.MeteorStreet.FileName));
        StreetsList.Add(new Street("Alta Pl", 30f, ScannerAudio.streets.AltaPlace.FileName));
        StreetsList.Add(new Street("Occupation Ave", 35f, ScannerAudio.streets.OccupationAve.FileName));
        StreetsList.Add(new Street("Carcer Way", 40f, ScannerAudio.streets.CarcerWay.FileName));
        StreetsList.Add(new Street("Eastbourne Way", 30f, ScannerAudio.streets.EastbourneWay.FileName));
        StreetsList.Add(new Street("Rockford Dr", 35f, ScannerAudio.streets.RockfordDrive.FileName));
        StreetsList.Add(new Street("Abe Milton Pkwy", 35f, ScannerAudio.streets.EightMiltonParkway.FileName));
        StreetsList.Add(new Street("Laguna Pl", 30f, ScannerAudio.streets.LagunaPlace.FileName));
        StreetsList.Add(new Street("Sinners Passage", 30f, ScannerAudio.streets.SinnersPassage.FileName));
        StreetsList.Add(new Street("Atlee St", 30f, ScannerAudio.streets.AtleyStreet.FileName));
        StreetsList.Add(new Street("Sinner St", 30f, ScannerAudio.streets.SinnerStreet.FileName));
        StreetsList.Add(new Street("Supply St", 30f, ScannerAudio.streets.SupplyStreet.FileName));
        StreetsList.Add(new Street("Amarillo Way", 35f, ScannerAudio.streets.AmarilloWay.FileName));
        StreetsList.Add(new Street("Tower Way", 35f, ScannerAudio.streets.TowerWay.FileName));
        StreetsList.Add(new Street("Decker St", 35f, ScannerAudio.streets.DeckerStreet.FileName));
        StreetsList.Add(new Street("Tackle St", 25f, ScannerAudio.streets.TackleStreet.FileName));
        StreetsList.Add(new Street("Low Power St", 35f, ScannerAudio.streets.LowPowerStreet.FileName));
        StreetsList.Add(new Street("Clinton Ave", 35f, ScannerAudio.streets.ClintonAve.FileName));
        StreetsList.Add(new Street("Fenwell Pl", 35f, ScannerAudio.streets.FenwellPlace.FileName));
        StreetsList.Add(new Street("Utopia Gardens", 25f, ScannerAudio.streets.UtopiaGardens.FileName));
        StreetsList.Add(new Street("Cavalry Blvd", 35f));
        StreetsList.Add(new Street("South Boulevard Del Perro", 35f, ScannerAudio.streets.SouthBlvdDelPierro.FileName));
        StreetsList.Add(new Street("Americano Way", 25f, ScannerAudio.streets.AmericanoWay.FileName));
        StreetsList.Add(new Street("Sam Austin Dr", 25f, ScannerAudio.streets.SamAustinDrive.FileName));
        StreetsList.Add(new Street("East Galileo Ave", 35f, ScannerAudio.streets.EastGalileoAve.FileName));
        StreetsList.Add(new Street("Galileo Park", 35f));
        StreetsList.Add(new Street("West Galileo Ave", 35f, ScannerAudio.streets.WestGalileoAve.FileName));
        StreetsList.Add(new Street("Tongva Dr", 40f, ScannerAudio.streets.TongvaDrive.FileName));
        StreetsList.Add(new Street("Zancudo Rd", 35f, ScannerAudio.streets.ZancudoRoad.FileName));
        StreetsList.Add(new Street("Movie Star Way", 35f, ScannerAudio.streets.MovieStarWay.FileName));
        StreetsList.Add(new Street("Heritage Way", 35f, ScannerAudio.streets.HeritageWay.FileName));
        StreetsList.Add(new Street("Perth St", 25f, ScannerAudio.streets.PerfStreet.FileName));
        StreetsList.Add(new Street("Chianski Passage", 30f));
        StreetsList.Add(new Street("Lolita Ave", 35f, ScannerAudio.streets.LolitaAve.FileName));
        StreetsList.Add(new Street("Meringue Ln", 35f, ScannerAudio.streets.MirangeLane.FileName));
        StreetsList.Add(new Street("Strangeways Dr", 30f, ScannerAudio.streets.StrangeWaysDrive.FileName));
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

