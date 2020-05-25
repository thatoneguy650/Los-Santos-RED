using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


public static class Streets
{
    private static string ConfigFileName = "Plugins\\LosSantosRED\\Streets.xml";
    public static List<Street> StreetsList;
    public static void Initialize()
    {
        ReadConfig();
    }
    public static void Dispose()
    {

    }
    public static void ReadConfig()
    {
        if (File.Exists(ConfigFileName))
        {
            StreetsList = General.DeserializeParams<Street>(ConfigFileName);
        }
        else
        {
            DefaultConfig();
            General.SerializeParams(StreetsList, ConfigFileName);
        }
    }
    public static void DefaultConfig()
    {
        StreetsList = new List<Street>
        {
            new Street("Joshua Rd", 50f, DispatchScannerFiles.streets.JoshuaRoad.FileName),
            new Street("East Joshua Road", 50f, DispatchScannerFiles.streets.EastJoshuaRoad.FileName),
            new Street("Marina Dr", 35f, DispatchScannerFiles.streets.MarinaDrive.FileName),
            new Street("Alhambra Dr", 35f, DispatchScannerFiles.streets.ElHamberDrive.FileName),
            new Street("Niland Ave", 35f, DispatchScannerFiles.streets.NeelanAve.FileName),
            new Street("Zancudo Ave", 35f, DispatchScannerFiles.streets.ZancudoAve.FileName),
            new Street("Armadillo Ave", 35f, DispatchScannerFiles.streets.ArmadilloAve.FileName),
            new Street("Algonquin Blvd", 35f, DispatchScannerFiles.streets.AlgonquinBlvd.FileName),
            new Street("Mountain View Dr", 35f, DispatchScannerFiles.streets.MountainViewDrive.FileName),
            new Street("Cholla Springs Ave", 35f, DispatchScannerFiles.streets.ChollaSpringsAve.FileName),
            new Street("Panorama Dr", 40f, DispatchScannerFiles.streets.PanoramaDrive.FileName),
            new Street("Lesbos Ln", 35f, DispatchScannerFiles.streets.LesbosLane.FileName),
            new Street("Calafia Rd", 30f, DispatchScannerFiles.streets.CalapiaRoad.FileName),
            new Street("North Calafia Way", 30f, DispatchScannerFiles.streets.NorthKalafiaWay.FileName),
            new Street("Cassidy Trail", 25f, DispatchScannerFiles.streets.CassidyTrail.FileName),
            new Street("Seaview Rd", 35f, DispatchScannerFiles.streets.SeaviewRd.FileName),
            new Street("Grapeseed Main St", 35f, DispatchScannerFiles.streets.GrapseedMainStreet.FileName),
            new Street("Grapeseed Ave", 35f, DispatchScannerFiles.streets.GrapeseedAve.FileName),
            new Street("Joad Ln", 35f, DispatchScannerFiles.streets.JilledLane.FileName),
            new Street("Union Rd", 40f, DispatchScannerFiles.streets.UnionRoad.FileName),
            new Street("O'Neil Way", 25f, DispatchScannerFiles.streets.OneilWay.FileName),
            new Street("Senora Fwy", 65f, DispatchScannerFiles.streets.SonoraFreeway.FileName, true),
            new Street("Catfish View", 35f, DispatchScannerFiles.streets.CatfishView.FileName),
            new Street("Great Ocean Hwy", 60f, DispatchScannerFiles.streets.GreatOceanHighway.FileName, true),
            new Street("Paleto Blvd", 35f, DispatchScannerFiles.streets.PaletoBlvd.FileName),
            new Street("Duluoz Ave", 35f, DispatchScannerFiles.streets.DelouasAve.FileName),
            new Street("Procopio Dr", 35f, DispatchScannerFiles.streets.ProcopioDrive.FileName),
            new Street("Cascabel Ave", 30f),
            new Street("Peaceful St", 30f,DispatchScannerFiles.streets.PeacefulStreet.FileName),
            new Street("Procopio Promenade", 25f, DispatchScannerFiles.streets.ProcopioPromenade.FileName),
            new Street("Pyrite Ave", 30f, DispatchScannerFiles.streets.PyriteAve.FileName),
            new Street("Fort Zancudo Approach Rd", 25f, DispatchScannerFiles.streets.FortZancudoApproachRoad.FileName),
            new Street("Barbareno Rd", 30f, DispatchScannerFiles.streets.BarbarinoRoad.FileName),
            new Street("Ineseno Road", 30f, DispatchScannerFiles.streets.EnecinoRoad.FileName),
            new Street("West Eclipse Blvd", 35f, DispatchScannerFiles.streets.WestEclipseBlvd.FileName),
            new Street("Playa Vista", 30f, DispatchScannerFiles.streets.PlayaVista.FileName),
            new Street("Bay City Ave", 30f, DispatchScannerFiles.streets.BaseCityAve.FileName),
            new Street("Del Perro Fwy", 65f, DispatchScannerFiles.streets.DelPierroFreeway.FileName, true),
            new Street("Equality Way", 30f, DispatchScannerFiles.streets.EqualityWay.FileName),
            new Street("Red Desert Ave", 30f, DispatchScannerFiles.streets.RedDesertAve.FileName),
            new Street("Magellan Ave", 25f, DispatchScannerFiles.streets.MagellanAve.FileName),
            new Street("Sandcastle Way", 30f, DispatchScannerFiles.streets.SandcastleWay.FileName),
            new Street("Vespucci Blvd", 40f, DispatchScannerFiles.streets.VespucciBlvd.FileName),
            new Street("Prosperity St", 30f, DispatchScannerFiles.streets.ProsperityStreet.FileName),
            new Street("San Andreas Ave", 40f, DispatchScannerFiles.streets.SanAndreasAve.FileName),
            new Street("North Rockford Dr", 35f, DispatchScannerFiles.streets.NorthRockfordDrive.FileName),
            new Street("South Rockford Dr", 35f, DispatchScannerFiles.streets.SouthRockfordDrive.FileName),
            new Street("Marathon Ave", 30f, DispatchScannerFiles.streets.MarathonAve.FileName),
            new Street("Boulevard Del Perro", 35f, DispatchScannerFiles.streets.BlvdDelPierro.FileName),
            new Street("Cougar Ave", 30f, DispatchScannerFiles.streets.CougarAve.FileName),
            new Street("Liberty St", 30f, DispatchScannerFiles.streets.LibertyStreet.FileName),
            new Street("Bay City Incline", 40f, DispatchScannerFiles.streets.BaseCityIncline.FileName),
            new Street("Conquistador St", 25f, DispatchScannerFiles.streets.ConquistadorStreet.FileName),
            new Street("Cortes St", 25f, DispatchScannerFiles.streets.CortezStreet.FileName),
            new Street("Vitus St", 25f, DispatchScannerFiles.streets.VitasStreet.FileName),
            new Street("Aguja St", 25f, DispatchScannerFiles.streets.ElGouhaStreet.FileName),/////maytbe????!?!?!
            new Street("Goma St", 25f, DispatchScannerFiles.streets.GomezStreet.FileName),
            new Street("Melanoma St", 25f, DispatchScannerFiles.streets.MelanomaStreet.FileName),
            new Street("Palomino Ave", 35f, DispatchScannerFiles.streets.PalaminoAve.FileName),
            new Street("Invention Ct", 25f, DispatchScannerFiles.streets.InventionCourt.FileName),
            new Street("Imagination Ct", 25f, DispatchScannerFiles.streets.ImaginationCourt.FileName),
            new Street("Rub St", 25f, DispatchScannerFiles.streets.RubStreet.FileName),
            new Street("Tug St", 25f, DispatchScannerFiles.streets.TugStreet.FileName),
            new Street("Ginger St", 30f, DispatchScannerFiles.streets.GingerStreet.FileName),
            new Street("Lindsay Circus", 30f, DispatchScannerFiles.streets.LindsayCircus.FileName),
            new Street("Calais Ave", 35f, DispatchScannerFiles.streets.CaliasAve.FileName),
            new Street("Adam's Apple Blvd", 40f, DispatchScannerFiles.streets.AdamsAppleBlvd.FileName),
            new Street("Alta St", 40f, DispatchScannerFiles.streets.AlterStreet.FileName),
            new Street("Integrity Way", 30f, DispatchScannerFiles.streets.IntergrityWy.FileName),
            new Street("Swiss St", 30f, DispatchScannerFiles.streets.SwissStreet.FileName),
            new Street("Strawberry Ave", 40f, DispatchScannerFiles.streets.StrawberryAve.FileName),
            new Street("Capital Blvd", 30f, DispatchScannerFiles.streets.CapitalBlvd.FileName),
            new Street("Crusade Rd", 30f, DispatchScannerFiles.streets.CrusadeRoad.FileName),
            new Street("Innocence Blvd", 40f, DispatchScannerFiles.streets.InnocenceBlvd.FileName),
            new Street("Davis Ave", 40f, DispatchScannerFiles.streets.DavisAve.FileName),
            new Street("Little Bighorn Ave", 35f, DispatchScannerFiles.streets.LittleBighornAve.FileName),
            new Street("Roy Lowenstein Blvd", 35f, DispatchScannerFiles.streets.RoyLowensteinBlvd.FileName),
            new Street("Jamestown St", 30f, DispatchScannerFiles.streets.JamestownStreet.FileName),
            new Street("Carson Ave", 35f, DispatchScannerFiles.streets.CarsonAve.FileName),
            new Street("Grove St", 30f, DispatchScannerFiles.streets.GroveStreet.FileName),
            new Street("Brouge Ave", 30f),
            new Street("Covenant Ave", 30f, DispatchScannerFiles.streets.CovenantAve.FileName),
            new Street("Dutch London St", 40f, DispatchScannerFiles.streets.DutchLondonStreet.FileName),
            new Street("Signal St", 30f, DispatchScannerFiles.streets.SignalStreet.FileName),
            new Street("Elysian Fields Fwy", 50f, DispatchScannerFiles.streets.ElysianFieldsFreeway.FileName, true),
            new Street("Plaice Pl", 30f),
            new Street("Chum St", 40f, DispatchScannerFiles.streets.ChumStreet.FileName),
            new Street("Chupacabra St", 30f),
            new Street("Miriam Turner Overpass", 60f, DispatchScannerFiles.streets.MiriamTurnerOverpass.FileName),
            new Street("Autopia Pkwy", 35f, DispatchScannerFiles.streets.AltopiaParkway.FileName),
            new Street("Exceptionalists Way", 35f, DispatchScannerFiles.streets.ExceptionalistWay.FileName),
            new Street("La Puerta Fwy", 60f, "", true),
            new Street("New Empire Way", 30f, DispatchScannerFiles.streets.NewEmpireWay.FileName),
            new Street("Runway1", 90f, DispatchScannerFiles.streets.RunwayOne.FileName),
            new Street("Greenwich Pkwy", 35f, DispatchScannerFiles.streets.GrenwichParkway.FileName),
            new Street("Kortz Dr", 30f, DispatchScannerFiles.streets.KortzDrive.FileName),
            new Street("Banham Canyon Dr", 40f, DispatchScannerFiles.streets.BanhamCanyonDrive.FileName),
            new Street("Buen Vino Rd", 40f),
            new Street("Route 68", 55f, DispatchScannerFiles.streets.Route68.FileName),
            new Street("Zancudo Grande Valley", 40f, DispatchScannerFiles.streets.ZancudoGrandeValley.FileName),
            new Street("Zancudo Barranca", 40f, DispatchScannerFiles.streets.ZancudoBaranca.FileName),
            new Street("Galileo Rd", 40f, DispatchScannerFiles.streets.GallileoRoad.FileName),
            new Street("Mt Vinewood Dr", 40f, DispatchScannerFiles.streets.MountVinewoodDrive.FileName),
            new Street("Marlowe Dr", 40f),
            new Street("Milton Rd", 35f, DispatchScannerFiles.streets.MiltonRoad.FileName),
            new Street("Kimble Hill Dr", 35f, DispatchScannerFiles.streets.KimbalHillDrive.FileName),
            new Street("Normandy Dr", 35f, DispatchScannerFiles.streets.NormandyDrive.FileName),
            new Street("Hillcrest Ave", 35f, DispatchScannerFiles.streets.HillcrestAve.FileName),
            new Street("Hillcrest Ridge Access Rd", 35f, DispatchScannerFiles.streets.HillcrestRidgeAccessRoad.FileName),
            new Street("North Sheldon Ave", 35f, DispatchScannerFiles.streets.NorthSheldonAve.FileName),
            new Street("Lake Vinewood Dr", 35f, DispatchScannerFiles.streets.LakeVineWoodDrive.FileName),
            new Street("Lake Vinewood Est", 35f, DispatchScannerFiles.streets.LakeVinewoodEstate.FileName),
            new Street("Baytree Canyon Rd", 40f, DispatchScannerFiles.streets.BaytreeCanyonRoad.FileName),
            new Street("North Conker Ave", 35f, DispatchScannerFiles.streets.NorthConkerAve.FileName),
            new Street("Wild Oats Dr", 35f, DispatchScannerFiles.streets.WildOatsDrive.FileName),
            new Street("Whispymound Dr", 35f, DispatchScannerFiles.streets.WispyMoundDrive.FileName),
            new Street("Didion Dr", 35f, DispatchScannerFiles.streets.DiedianDrive.FileName),
            new Street("Cox Way", 35f, DispatchScannerFiles.streets.CoxWay.FileName),
            new Street("Picture Perfect Drive", 35f, DispatchScannerFiles.streets.PicturePerfectDrive.FileName),
            new Street("South Mo Milton Dr", 35f, DispatchScannerFiles.streets.SouthMoMiltonDrive.FileName),
            new Street("Cockingend Dr", 35f, DispatchScannerFiles.streets.CockandGinDrive.FileName),
            new Street("Mad Wayne Thunder Dr", 35f, DispatchScannerFiles.streets.MagwavevendorDrive.FileName),
            new Street("Hangman Ave", 35f, DispatchScannerFiles.streets.HangmanAve.FileName),
            new Street("Dunstable Ln", 35f, DispatchScannerFiles.streets.DunstableLane.FileName),
            new Street("Dunstable Dr", 35f, DispatchScannerFiles.streets.DunstableDrive.FileName),
            new Street("Greenwich Way", 35f, DispatchScannerFiles.streets.GrenwichWay.FileName),
            new Street("Greenwich Pl", 35f, DispatchScannerFiles.streets.GrunnichPlace.FileName),
            new Street("Hardy Way", 35f),
            new Street("Richman St", 35f, DispatchScannerFiles.streets.RichmondStreet.FileName),
            new Street("Ace Jones Dr", 35f, DispatchScannerFiles.streets.AceJonesDrive.FileName),
            new Street("Los Santos Freeway", 65f, "", true),
            new Street("Senora Rd", 40f, DispatchScannerFiles.streets.SonoraRoad.FileName),
            new Street("Nowhere Rd", 25f, DispatchScannerFiles.streets.NowhereRoad.FileName),
            new Street("Smoke Tree Rd", 35f, DispatchScannerFiles.streets.SmokeTreeRoad.FileName),
            new Street("Cholla Rd", 35f, DispatchScannerFiles.streets.ChollaRoad.FileName),
            new Street("Cat-Claw Ave", 35f, DispatchScannerFiles.streets.CatClawAve.FileName),
            new Street("Senora Way", 40f, DispatchScannerFiles.streets.SonoraWay.FileName),
            new Street("Palomino Fwy", 60f, DispatchScannerFiles.streets.PaliminoFreeway.FileName, true),
            new Street("Shank St", 25f, DispatchScannerFiles.streets.ShankStreet.FileName),
            new Street("Macdonald St", 35f, DispatchScannerFiles.streets.McDonaldStreet.FileName),
            new Street("Route 68 Approach", 55f, DispatchScannerFiles.streets.Route68.FileName),
            new Street("Vinewood Park Dr", 35f, DispatchScannerFiles.streets.VinewoodParkDrive.FileName),
            new Street("Vinewood Blvd", 40f, DispatchScannerFiles.streets.VinewoodBlvd.FileName),
            new Street("Mirror Park Blvd", 35f, DispatchScannerFiles.streets.MirrorParkBlvd.FileName),
            new Street("Glory Way", 35f, DispatchScannerFiles.streets.GloryWay.FileName),
            new Street("Bridge St", 35f, DispatchScannerFiles.streets.BridgeStreet.FileName),
            new Street("West Mirror Drive", 35f, DispatchScannerFiles.streets.WestMirrorDrive.FileName),
            new Street("Nikola Ave", 35f, DispatchScannerFiles.streets.NicolaAve.FileName),
            new Street("East Mirror Dr", 35f, DispatchScannerFiles.streets.EastMirrorDrive.FileName),
            new Street("Nikola Pl", 25f, DispatchScannerFiles.streets.NikolaPlace.FileName),
            new Street("Mirror Pl", 35f, DispatchScannerFiles.streets.MirrorPlace.FileName),
            new Street("El Rancho Blvd", 40f, DispatchScannerFiles.streets.ElRanchoBlvd.FileName),
            new Street("Olympic Fwy", 60f, DispatchScannerFiles.streets.OlympicFreeway.FileName, true),
            new Street("Fudge Ln", 25f, DispatchScannerFiles.streets.FudgeLane.FileName),
            new Street("Amarillo Vista", 25f, DispatchScannerFiles.streets.AmarilloVista.FileName),
            new Street("Labor Pl", 35f, DispatchScannerFiles.streets.ForceLaborPlace.FileName),
            new Street("El Burro Blvd", 35f, DispatchScannerFiles.streets.ElBurroBlvd.FileName),
            new Street("Sustancia Rd", 45f, DispatchScannerFiles.streets.SustanciaRoad.FileName),
            new Street("South Shambles St", 30f, DispatchScannerFiles.streets.SouthShambleStreet.FileName),
            new Street("Hanger Way", 30f, DispatchScannerFiles.streets.HangarWay.FileName),
            new Street("Orchardville Ave", 30f, DispatchScannerFiles.streets.OrchidvilleAve.FileName),
            new Street("Popular St", 40f, DispatchScannerFiles.streets.PopularStreet.FileName),
            new Street("Buccaneer Way", 45f, DispatchScannerFiles.streets.BuccanierWay.FileName),
            new Street("Abattoir Ave", 35f, DispatchScannerFiles.streets.AvatorAve.FileName),
            new Street("Voodoo Place", 30f),
            new Street("Mutiny Rd", 35f, DispatchScannerFiles.streets.MutineeRoad.FileName),
            new Street("South Arsenal St", 35f, DispatchScannerFiles.streets.SouthArsenalStreet.FileName),
            new Street("Forum Dr", 35f, DispatchScannerFiles.streets.ForumDrive.FileName),
            new Street("Morningwood Blvd", 35f, DispatchScannerFiles.streets.MorningwoodBlvd.FileName),
            new Street("Dorset Dr", 40f, DispatchScannerFiles.streets.DorsetDrive.FileName),
            new Street("Caesars Place", 25f, DispatchScannerFiles.streets.CaesarPlace.FileName),
            new Street("Spanish Ave", 30f, DispatchScannerFiles.streets.SpanishAve.FileName),
            new Street("Portola Dr", 30f, DispatchScannerFiles.streets.PortolaDrive.FileName),
            new Street("Edwood Way", 25f, DispatchScannerFiles.streets.EdwardWay.FileName),
            new Street("San Vitus Blvd", 40f, DispatchScannerFiles.streets.SanVitusBlvd.FileName),
            new Street("Eclipse Blvd", 35f, DispatchScannerFiles.streets.EclipseBlvd.FileName),
            new Street("Gentry Lane", 30f),
            new Street("Las Lagunas Blvd", 40f, DispatchScannerFiles.streets.LasLegunasBlvd.FileName),
            new Street("Power St", 40f, DispatchScannerFiles.streets.PowerStreet.FileName),
            new Street("Mt Haan Rd", 40f, DispatchScannerFiles.streets.MtHaanRoad.FileName),
            new Street("Elgin Ave", 40f, DispatchScannerFiles.streets.ElginAve.FileName),
            new Street("Hawick Ave", 35f, DispatchScannerFiles.streets.HawickAve.FileName),
            new Street("Meteor St", 30f, DispatchScannerFiles.streets.MeteorStreet.FileName),
            new Street("Alta Pl", 30f, DispatchScannerFiles.streets.AltaPlace.FileName),
            new Street("Occupation Ave", 35f, DispatchScannerFiles.streets.OccupationAve.FileName),
            new Street("Carcer Way", 40f, DispatchScannerFiles.streets.CarcerWay.FileName),
            new Street("Eastbourne Way", 30f, DispatchScannerFiles.streets.EastbourneWay.FileName),
            new Street("Rockford Dr", 35f, DispatchScannerFiles.streets.RockfordDrive.FileName),
            new Street("Abe Milton Pkwy", 35f, DispatchScannerFiles.streets.EightMiltonParkway.FileName),
            new Street("Laguna Pl", 30f, DispatchScannerFiles.streets.LagunaPlace.FileName),
            new Street("Sinners Passage", 30f, DispatchScannerFiles.streets.SinnersPassage.FileName),
            new Street("Atlee St", 30f, DispatchScannerFiles.streets.AtleyStreet.FileName),
            new Street("Sinner St", 30f, DispatchScannerFiles.streets.SinnerStreet.FileName),
            new Street("Supply St", 30f, DispatchScannerFiles.streets.SupplyStreet.FileName),
            new Street("Amarillo Way", 35f, DispatchScannerFiles.streets.AmarilloWay.FileName),
            new Street("Tower Way", 35f, DispatchScannerFiles.streets.TowerWay.FileName),
            new Street("Decker St", 35f, DispatchScannerFiles.streets.DeckerStreet.FileName),
            new Street("Tackle St", 25f, DispatchScannerFiles.streets.TackleStreet.FileName),
            new Street("Low Power St", 35f, DispatchScannerFiles.streets.LowPowerStreet.FileName),
            new Street("Clinton Ave", 35f, DispatchScannerFiles.streets.ClintonAve.FileName),
            new Street("Fenwell Pl", 35f, DispatchScannerFiles.streets.FenwellPlace.FileName),
            new Street("Utopia Gardens", 25f, DispatchScannerFiles.streets.UtopiaGardens.FileName),
            new Street("Cavalry Blvd", 35f),
            new Street("South Boulevard Del Perro", 35f, DispatchScannerFiles.streets.SouthBlvdDelPierro.FileName),
            new Street("Americano Way", 25f, DispatchScannerFiles.streets.AmericanoWay.FileName),
            new Street("Sam Austin Dr", 25f, DispatchScannerFiles.streets.SamAustinDrive.FileName),
            new Street("East Galileo Ave", 35f, DispatchScannerFiles.streets.EastGalileoAve.FileName),
            new Street("Galileo Park", 35f),
            new Street("West Galileo Ave", 35f, DispatchScannerFiles.streets.WestGalileoAve.FileName),
            new Street("Tongva Dr", 40f, DispatchScannerFiles.streets.TongvaDrive.FileName),
            new Street("Zancudo Rd", 35f, DispatchScannerFiles.streets.ZancudoRoad.FileName),
            new Street("Movie Star Way", 35f, DispatchScannerFiles.streets.MovieStarWay.FileName),
            new Street("Heritage Way", 35f, DispatchScannerFiles.streets.HeritageWay.FileName),
            new Street("Perth St", 25f, DispatchScannerFiles.streets.PerfStreet.FileName),
            new Street("Chianski Passage", 30f),
            new Street("Lolita Ave", 35f, DispatchScannerFiles.streets.LolitaAve.FileName),
            new Street("Meringue Ln", 35f, DispatchScannerFiles.streets.MirangeLane.FileName),
            new Street("Strangeways Dr", 30f, DispatchScannerFiles.streets.StrangeWaysDrive.FileName),

            new Street("Mt Haan Dr", 40f, DispatchScannerFiles.streets.MtHaanDrive.FileName)
        };
    }
    public static Street GetStreetFromName(string StreetName)
    {
        return StreetsList.Where(x => x.Name == StreetName).FirstOrDefault();
    }
    public static string GetCurrentStreet(Vector3 Position)
    {
        int StreetHash = 0;
        int CrossingHash = 0;
        unsafe
        {
            NativeFunction.CallByName<uint>("GET_STREET_NAME_AT_COORD", Position.X, Position.Y, Position.Z, &StreetHash, &CrossingHash);
        }
        string StreetName = string.Empty;
        string CrossStreetName = string.Empty;
        if (StreetHash != 0)
        {
            unsafe
            {
                IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_STREET_NAME_FROM_HASH_KEY", StreetHash);

                StreetName = Marshal.PtrToStringAnsi(ptr);
            }
        }
        return StreetName;
    }
}
public class Street
{
    public string Name = "";
    public float SpeedLimit = 50f;
    public string DispatchFile = "";
    public bool IsHighway = false;
    public Street()
    {

    }
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
        IsHighway = _isFreeway;
    }
}

