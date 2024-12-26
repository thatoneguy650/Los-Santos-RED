using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


public class Streets : IStreets
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Streets.xml";
    public List<Street> StreetsList { get; private set; }
    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("Streets*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Streets config: {ConfigFile.FullName}",0);
            StreetsList = Serialization.DeserializeParams<Street>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Streets config  {ConfigFileName}",0);
            StreetsList = Serialization.DeserializeParams<Street>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Streets config found, creating default", 0);
            DefaultConfig_LibertyCity();
            DefaultConfig_SunshineDream();
            DefaultConfig();
        }
    }
    public string GetStreetNames(Vector3 Position, bool withCross)
    {
        string StreetName = GetStreetName(Position);
        Street street = StreetsList.Where(x => x.Name == StreetName).FirstOrDefault();
        string CrossStreetName = GetCrossStreetName(Position);
        Street crossStreet = StreetsList.Where(x => x.Name == CrossStreetName).FirstOrDefault();
        if(street != null)
        {
            if (crossStreet != null && withCross)
            {
                return $"~y~{street.ProperStreetName}~s~ at ~y~{crossStreet.ProperStreetName}~s~".Trim();
            }
            else
            {
                return $"~y~{street.ProperStreetName}~s~".Trim();
            }
        }
        return $"";
    }
    //public void GetStreets(Vector3 Position, out Street Street, out Street CrossStreet)
    //{
    //    string StreetName = GetStreetName(Position);
    //    Street = StreetsList.Where(x => x.Name == StreetName).FirstOrDefault();
    //    string CrossStreetName = GetCrossStreetName(Position);
    //    CrossStreet = StreetsList.Where(x => x.Name == CrossStreetName).FirstOrDefault();
    //}
    public Street GetStreet(int nodeID)
    {
        return StreetsList.Where(x => x.Nodes != null && x.Nodes.Contains(nodeID)).FirstOrDefault();
    }
    public Street GetStreet(string StreetName)
    {
        return StreetsList.Where(x => x.Name == StreetName).FirstOrDefault();
    }
    public Street GetStreet(Vector3 Position)
    {
        string StreetName = GetStreetName(Position);
        return StreetsList.Where(x => x.Name == StreetName).FirstOrDefault();
    }
    private string GetStreetName(Vector3 Position)
    {
        int StreetHash = 0;
        int CrossingHash = 0;
        NativeFunction.Natives.GET_STREET_NAME_AT_COORD<uint>(Position.X, Position.Y, Position.Z, out StreetHash, out CrossingHash);
        string StreetName = string.Empty;
        string CrossStreetName = string.Empty;
        if (StreetHash != 0)
        {
            IntPtr ptr = NativeFunction.Natives.GET_STREET_NAME_FROM_HASH_KEY<IntPtr>(StreetHash);
            StreetName = Marshal.PtrToStringAnsi(ptr);
        }
        return StreetName;
    }
    private string GetCrossStreetName(Vector3 Position)
    {
        int StreetHash = 0;
        int CrossingHash = 0;
        NativeFunction.Natives.GET_STREET_NAME_AT_COORD<uint>(Position.X, Position.Y, Position.Z, out StreetHash, out CrossingHash);
        string StreetName = string.Empty;
        string CrossStreetName = string.Empty;
        if (CrossingHash != 0)
        {
            IntPtr ptr = NativeFunction.Natives.GET_STREET_NAME_FROM_HASH_KEY<IntPtr>(CrossingHash);
            StreetName = Marshal.PtrToStringAnsi(ptr);
        }
        return StreetName;
    }
    private void DefaultConfig()
    {
        StreetsList = new List<Street>
        {
            new Street("Joshua Rd", 50f, "MPH"),
            new Street("East Joshua Road", 50f, "MPH") { DisplayName = "E Joshua Rd" },
            new Street("Marina Dr", 35f, "MPH"),
            new Street("Alhambra Dr", 35f, "MPH"),
            new Street("Niland Ave", 35f, "MPH"),
            new Street("Zancudo Ave", 35f, "MPH"),
            new Street("Armadillo Ave", 35f, "MPH"),
            new Street("Algonquin Blvd", 35f, "MPH"),
            new Street("Mountain View Dr", 35f, "MPH"),
            new Street("Cholla Springs Ave", 35f, "MPH"),
            new Street("Panorama Dr", 40f, "MPH"),
            new Street("Lesbos Ln", 35f, "MPH"),
            new Street("Calafia Rd", 30f, "MPH"),
            new Street("North Calafia Way", 30f, "MPH"),
            new Street("Cassidy Trail", 25f, "MPH"),
            new Street("Seaview Rd", 35f, "MPH"),
            new Street("Grapeseed Main St", 35f, "MPH"),
            new Street("Grapeseed Ave", 35f, "MPH"),
            new Street("Joad Ln", 35f, "MPH"),
            new Street("Union Rd", 40f, "MPH"),
            new Street("O'Neil Way", 25f, "MPH"),
            new Street("Senora Fwy", 65f, "MPH", true),
            new Street("Catfish View", 35f, "MPH"),
            new Street("Great Ocean Hwy", 60f, "MPH", true),
            new Street("Paleto Blvd", 35f, "MPH"),
            new Street("Duluoz Ave", 35f, "MPH"),
            new Street("Procopio Dr", 35f, "MPH"),
            new Street("Cascabel Ave", 30f, "MPH"),
            new Street("Peaceful St", 30f, "MPH"),
            new Street("Procopio Promenade", 25f, "MPH"),
            new Street("Pyrite Ave", 30f, "MPH"),
            new Street("Fort Zancudo Approach Rd", 25f, "MPH"),
            new Street("Barbareno Rd", 30f, "MPH"),
            new Street("Ineseno Road", 30f, "MPH"),
            new Street("West Eclipse Blvd", 35f, "MPH"),
            new Street("Playa Vista", 30f, "MPH"),
            new Street("Bay City Ave", 30f, "MPH"),
            new Street("Del Perro Fwy", 65f, "MPH", true),
            new Street("Equality Way", 30f, "MPH"),
            new Street("Red Desert Ave", 30f, "MPH"),
            new Street("Magellan Ave", 25f, "MPH"),
            new Street("Sandcastle Way", 30f, "MPH"),
            new Street("Vespucci Blvd", 40f, "MPH"),
            new Street("Prosperity St", 30f, "MPH"),
            new Street("San Andreas Ave", 40f, "MPH"),
            new Street("North Rockford Dr", 35f, "MPH"),
            new Street("South Rockford Dr", 35f, "MPH"),
            new Street("Marathon Ave", 30f, "MPH"),
            new Street("Boulevard Del Perro", 35f, "MPH"),
            new Street("Cougar Ave", 30f, "MPH"),
            new Street("Liberty St", 30f, "MPH"),
            new Street("Bay City Incline", 40f, "MPH"),
            new Street("Conquistador St", 25f, "MPH"),
            new Street("Cortes St", 25f, "MPH"),
            new Street("Vitus St", 25f, "MPH"),
            new Street("Aguja St", 25f, "MPH"),/////maybe????!?!?!
            new Street("Goma St", 25f, "MPH"),
            new Street("Melanoma St", 25f, "MPH"),
            new Street("Palomino Ave", 35f, "MPH"),
            new Street("Invention Ct", 25f, "MPH"),
            new Street("Imagination Ct", 25f, "MPH"),
            new Street("Rub St", 25f, "MPH"),
            new Street("Tug St", 25f, "MPH"),
            new Street("Ginger St", 30f, "MPH"),
            new Street("Lindsay Circus", 30f, "MPH"),
            new Street("Calais Ave", 35f, "MPH"),
            new Street("Adam's Apple Blvd", 40f, "MPH"),
            new Street("Alta St", 40f, "MPH"),
            new Street("Integrity Way", 30f, "MPH"),
            new Street("Swiss St", 30f, "MPH"),
            new Street("Strawberry Ave", 40f, "MPH"),
            new Street("Capital Blvd", 30f, "MPH"),
            new Street("Crusade Rd", 30f, "MPH"),
            new Street("Innocence Blvd", 40f, "MPH"),
            new Street("Davis Ave", 40f, "MPH"),
            new Street("Little Bighorn Ave", 35f, "MPH"),
            new Street("Roy Lowenstein Blvd", 35f, "MPH"),
            new Street("Jamestown St", 30f, "MPH"),
            new Street("Carson Ave", 35f, "MPH"),
            new Street("Grove St", 30f, "MPH"),
            new Street("Brouge Ave", 30f, "MPH"),
            new Street("Covenant Ave", 30f, "MPH"),
            new Street("Dutch London St", 40f, "MPH"),
            new Street("Signal St", 30f, "MPH"),
            new Street("Elysian Fields Fwy", 50f, "MPH", true),
            new Street("Plaice Pl", 30f, "MPH"),
            new Street("Chum St", 40f, "MPH"),
            new Street("Chupacabra St", 30f, "MPH"),
            new Street("Miriam Turner Overpass", 60f, "MPH"),
            new Street("Autopia Pkwy", 35f, "MPH"),
            new Street("Exceptionalists Way", 35f, "MPH"),
            new Street("La Puerta Fwy", 60f, "MPH", true),
            new Street("New Empire Way", 30f, "MPH"),
            new Street("Runway1", 90f, "MPH"),
            new Street("Greenwich Pkwy", 35f, "MPH"),
            new Street("Kortz Dr", 30f, "MPH"),
            new Street("Banham Canyon Dr", 40f, "MPH"),
            new Street("Buen Vino Rd", 40f, "MPH"),
            new Street("Route 68", 55f, "MPH",true),
            new Street("Zancudo Grande Valley", 40f, "MPH"),
            new Street("Zancudo Barranca", 40f, "MPH"),
            new Street("Galileo Rd", 40f, "MPH"),
            new Street("Mt Vinewood Dr", 40f, "MPH"),
            new Street("Marlowe Dr", 40f, "MPH"),
            new Street("Milton Rd", 35f, "MPH"),
            new Street("Kimble Hill Dr", 35f, "MPH"),
            new Street("Normandy Dr", 35f, "MPH"),
            new Street("Hillcrest Ave", 35f, "MPH"),
            new Street("Hillcrest Ridge Access Rd", 35f, "MPH"),
            new Street("North Sheldon Ave", 35f, "MPH"),
            new Street("Lake Vinewood Dr", 35f, "MPH"),
            new Street("Lake Vinewood Est", 35f, "MPH"),
            new Street("Baytree Canyon Rd", 40f, "MPH"),
            new Street("North Conker Ave", 35f, "MPH"),
            new Street("Wild Oats Dr", 35f, "MPH"),
            new Street("Whispymound Dr", 35f, "MPH"),
            new Street("Didion Dr", 35f, "MPH"),
            new Street("Cox Way", 35f, "MPH"),
            new Street("Picture Perfect Drive", 35f, "MPH"),
            new Street("South Mo Milton Dr", 35f, "MPH"),
            new Street("Cockingend Dr", 35f, "MPH"),
            new Street("Mad Wayne Thunder Dr", 35f, "MPH"),
            new Street("Hangman Ave", 35f, "MPH"),
            new Street("Dunstable Ln", 35f, "MPH"),
            new Street("Dunstable Dr", 35f, "MPH"),
            new Street("Greenwich Way", 35f, "MPH"),
            new Street("Greenwich Pl", 35f, "MPH"),
            new Street("Hardy Way", 35f, "MPH"),
            new Street("Richman St", 35f, "MPH"),
            new Street("Ace Jones Dr", 35f, "MPH"),
            new Street("Los Santos Freeway", 65f, "MPH", true),
            new Street("Senora Rd", 60f, "MPH"),
            new Street("Nowhere Rd", 25f, "MPH"),
            new Street("Smoke Tree Rd", 35f, "MPH"),
            new Street("Cholla Rd", 35f, "MPH"),
            new Street("Cat-Claw Ave", 35f, "MPH"),
            new Street("Senora Way", 40f, "MPH"),
            new Street("Palomino Fwy", 60f, "MPH", true),
            new Street("Shank St", 25f, "MPH"),
            new Street("Macdonald St", 35f, "MPH"),
            new Street("Route 68 Approach", 55f, "MPH"),
            new Street("Vinewood Park Dr", 35f, "MPH"),
            new Street("Vinewood Blvd", 40f, "MPH"),
            new Street("Mirror Park Blvd", 35f, "MPH"),
            new Street("Glory Way", 35f, "MPH"),
            new Street("Bridge St", 35f, "MPH"),
            new Street("West Mirror Drive", 35f, "MPH"),
            new Street("Nikola Ave", 35f, "MPH"),
            new Street("East Mirror Dr", 35f, "MPH"),
            new Street("Nikola Pl", 25f, "MPH"),
            new Street("Mirror Pl", 35f, "MPH"),
            new Street("El Rancho Blvd", 40f, "MPH"),
            new Street("Olympic Fwy", 60f, "MPH", true),
            new Street("Fudge Ln", 25f, "MPH"),
            new Street("Amarillo Vista", 25f, "MPH"),
            new Street("Labor Pl", 35f, "MPH"),
            new Street("El Burro Blvd", 35f, "MPH"),
            new Street("Sustancia Rd", 45f, "MPH"),
            new Street("South Shambles St", 30f, "MPH"),
            new Street("Hanger Way", 30f, "MPH"),
            new Street("Orchardville Ave", 30f, "MPH"),
            new Street("Popular St", 40f, "MPH"),
            new Street("Buccaneer Way", 45f, "MPH"),
            new Street("Abattoir Ave", 35f, "MPH"),
            new Street("Voodoo Place", 30f, "MPH"),
            new Street("Mutiny Rd", 35f, "MPH"),
            new Street("South Arsenal St", 35f, "MPH"),
            new Street("Forum Dr", 35f, "MPH"),
            new Street("Morningwood Blvd", 35f, "MPH"),
            new Street("Dorset Dr", 40f, "MPH"),
            new Street("Caesars Place", 25f, "MPH"),
            new Street("Spanish Ave", 30f, "MPH"),
            new Street("Portola Dr", 30f, "MPH"),
            new Street("Edwood Way", 25f, "MPH"),
            new Street("San Vitus Blvd", 40f, "MPH"),
            new Street("Eclipse Blvd", 35f, "MPH"),
            new Street("Gentry Lane", 30f, "MPH"),
            new Street("Las Lagunas Blvd", 40f, "MPH"),
            new Street("Power St", 40f, "MPH"),
            new Street("Mt Haan Rd", 40f, "MPH"),
            new Street("Elgin Ave", 40f, "MPH"),
            new Street("Hawick Ave", 35f, "MPH"),
            new Street("Meteor St", 30f, "MPH"),
            new Street("Alta Pl", 30f, "MPH"),
            new Street("Occupation Ave", 35f, "MPH"),
            new Street("Carcer Way", 40f, "MPH"),
            new Street("Eastbourne Way", 30f, "MPH"),
            new Street("Rockford Dr", 35f, "MPH"),
            new Street("Abe Milton Pkwy", 35f, "MPH") ,
            new Street("Laguna Pl", 30f, "MPH"),
            new Street("Sinners Passage", 30f, "MPH"),
            new Street("Atlee St", 30f, "MPH"),
            new Street("Sinner St", 30f, "MPH"),
            new Street("Supply St", 30f, "MPH"),
            new Street("Amarillo Way", 35f, "MPH"),
            new Street("Tower Way", 35f, "MPH"),
            new Street("Decker St", 35f, "MPH"),
            new Street("Tackle St", 25f, "MPH"),
            new Street("Low Power St", 35f, "MPH"),
            new Street("Clinton Ave", 35f, "MPH"),
            new Street("Fenwell Pl", 35f, "MPH"),
            new Street("Utopia Gardens", 25f, "MPH"),
            new Street("Cavalry Blvd", 35f, "MPH"),
            new Street("South Boulevard Del Perro", 35f, "MPH"),
            new Street("Americano Way", 25f, "MPH"),
            new Street("Sam Austin Dr", 25f, "MPH"),
            new Street("East Galileo Ave", 35f, "MPH"),
            new Street("Galileo Park", 35f, "MPH"),
            new Street("West Galileo Ave", 35f, "MPH"),
            new Street("Tongva Dr", 40f, "MPH"),
            new Street("Zancudo Rd", 35f, "MPH"),
            new Street("Movie Star Way", 35f, "MPH"),
            new Street("Heritage Way", 35f, "MPH"),
            new Street("Perth St", 25f, "MPH"),
            new Street("Chianski Passage", 30f, "MPH"),
            new Street("Lolita Ave", 35f, "MPH"),
            new Street("Meringue Ln", 35f, "MPH"),
            new Street("Strangeways Dr", 30f, "MPH"),
            new Street("Mt Haan Dr", 40f, "MPH")
        };
        Serialization.SerializeParams(StreetsList, ConfigFileName);
    }
    private void DefaultConfig_LibertyCity()
    {
        List<Street> LCStreetsList = new List<Street>
        {
            new Street("Albany Ave", 40f, "MPH"),
            new Street("Bismarck Ave", 40f, "MPH"),
            new Street("Columbus Ave", 40f, "MPH"),
            new Street("Denver-Exeter Ave", 40f, "MPH"),
            new Street("Denver Ave", 40f, "MPH"),
            new Street("Exeter Ave", 40f, "MPH"),
            new Street("Frankfort Ave", 40f, "MPH"),
            new Street("Galveston Ave", 40f, "MPH"),
            new Street("Aragon St", 40f, "MPH"),
            new Street("Asparagus Ave", 40f, "MPH"),
            new Street("Bart St", 40f, "MPH"),
            new Street("Bean St", 40f, "MPH"),
            new Street("Boleyn St", 40f, "MPH"),
            new Street("Boone St", 40f, "MPH"),
            new Street("Brandon Ave", 40f, "MPH"),
            new Street("Broccoli St", 40f, "MPH"),
            new Street("Brunner St", 40f, "MPH"),
            new Street("Bunker Hill Ave", 40f, "MPH"),
            new Street("Camden Ave", 40f, "MPH"),
            new Street("Carrollton St", 40f, "MPH"),
            new Street("Carson St", 40f, "MPH"),
            new Street("Cassidy St", 40f, "MPH"),
            new Street("Cayuga Ave", 40f, "MPH"),
            new Street("Charleston Ave", 40f, "MPH"),
            new Street("Chicory St", 40f, "MPH"),
            new Street("Chive St", 40f, "MPH"),
            new Street("Cisco St", 40f, "MPH"),
            new Street("Cleves Ave", 40f, "MPH"),
            new Street("Cody St", 40f, "MPH"),
            new Street("Concord Ave", 40f, "MPH"),
            new Street("Conoy Ave", 40f, "MPH"),
            new Street("Creek St", 40f, "MPH"),
            new Street("Crockett Ave", 40f, "MPH"),
            new Street("Deadwood St", 40f, "MPH"),
            new Street("Delaware Ave", 40f, "MPH"),
            new Street("Dillon St", 40f, "MPH"),
            new Street("Earp St", 40f, "MPH"),
            new Street("Ellery St", 40f, "MPH"),
            new Street("Erie Ave", 40f, "MPH"),
            new Street("Franklin St", 40f, "MPH"),
            new Street("Freetown Ave", 40f, "MPH"),
            new Street("Garrett St", 40f, "MPH"),
            new Street("Gibson St", 40f, "MPH"),
            new Street("Hancock St", 40f, "MPH"),
            new Street("Hardin St", 40f, "MPH"),
            new Street("Harrison St", 40f, "MPH"),
            new Street("Hewes St", 40f, "MPH"),
            new Street("Hickock St", 40f, "MPH"),
            new Street("Hooper St", 40f, "MPH"),
            new Street("Howard St", 40f, "MPH"),
            new Street("Huntington St", 40f, "MPH"),
            new Street("Inchon Ave", 40f, "MPH"),
            new Street("Iroquois Ave", 40f, "MPH"),
            new Street("James St", 40f, "MPH"),
            new Street("Kid St", 40f, "MPH"),
            new Street("Livingston St", 40f, "MPH"),
            new Street("Lynch St", 40f, "MPH"),
            new Street("Masterson St", 40f, "MPH"),
            new Street("Mohanet Ave", 40f, "MPH"),
            new Street("Mohawk Ave", 40f, "MPH"),
            new Street("Mohegan Ave", 40f, "MPH"),
            new Street("Montauk Ave", 40f, "MPH"),
            new Street("Morris St", 40f, "MPH"),
            new Street("Munsee Ave", 40f, "MPH"),
            new Street("Oakley St", 40f, "MPH"),
            new Street("Oneida Ave", 40f, "MPH"),
            new Street("Onion St", 40f, "MPH"),
            new Street("Onondaga Ave", 40f, "MPH"),
            new Street("Pancho St", 40f, "MPH"),
            new Street("Parr St", 40f, "MPH"),
            new Street("Ringo St", 40f, "MPH"),
            new Street("San Jacinto Ave", 40f, "MPH"),
            new Street("Saponi Ave", 40f, "MPH"),
            new Street("Saratoga Ave", 40f, "MPH"),
            new Street("Savannah Ave", 40f, "MPH"),
            new Street("Seneca Ave", 40f, "MPH"),
            new Street("Seymour Ave", 40f, "MPH"),
            new Street("Stillwater Ave", 40f, "MPH"),
            new Street("Stone St", 40f, "MPH"),
            new Street("Sundance St", 40f, "MPH"),
            new Street("Thornton St", 40f, "MPH"),
            new Street("Tinconderoga Ave", 40f, "MPH"),
            new Street("Trenton Ave", 40f, "MPH"),
            new Street("Tulsa St", 40f, "MPH"),
            new Street("Tutelo Ave", 40f, "MPH"),
            new Street("Valley Forge Ave", 40f, "MPH"),
            new Street("Wappinger Ave", 40f, "MPH"),
            new Street("Wenrohronon Ave", 40f, "MPH"),
            new Street("Yorktown Ave", 40f, "MPH"),
            new Street("1990 St", 40f, "MPH"),
            new Street("Alcatraz Ave", 40f, "MPH"),
            new Street("Altona Ave", 40f, "MPH"),
            new Street("Applejack St", 40f, "MPH"),
            new Street("Attica Ave", 40f, "MPH"),
            new Street("Beaumont Ave", 40f, "MPH"),
            new Street("Bronco St", 40f, "MPH"),
            new Street("Butterfly St", 40f, "MPH"),
            new Street("Caterpillar St", 40f, "MPH"),
            new Street("Darkhammer St", 40f, "MPH"),
            new Street("Drill St", 40f, "MPH"),
            new Street("Drop St", 40f, "MPH"),
            new Street("Elbow St", 40f, "MPH"),
            new Street("Flanger St", 40f, "MPH"),
            new Street("Gainer St", 40f, "MPH"),
            new Street("Greene Ave", 40f, "MPH"),
            new Street("Guantanamo Ave", 40f, "MPH"),
            new Street("Headspring St", 40f, "MPH"),
            new Street("Hollowback St", 40f, "MPH"),
            new Street("Jackhammer St", 40f, "MPH"),
            new Street("Joliet St", 40f, "MPH"),
            new Street("Leavenworth Ave", 40f, "MPH"),
            new Street("Lompoc Ave", 40f, "MPH"),
            new Street("Lotus St", 40f, "MPH"),
            new Street("Mill St", 40f, "MPH"),
            new Street("Planche St", 40f, "MPH"),
            new Street("Rocket St", 40f, "MPH"),
            new Street("Rykers Ave", 40f, "MPH"),
            new Street("San Quentin Ave", 40f, "MPH"),
            new Street("Sing Sing Ave", 40f, "MPH"),
            new Street("Spin St", 40f, "MPH"),
            new Street("Switch St", 40f, "MPH"),
            new Street("Turtle St", 40f, "MPH"),
            new Street("Uprock St", 40f, "MPH"),
            new Street("Valdez St", 40f, "MPH"),
            new Street("Wallkill Ave", 40f, "MPH"),
            new Street("Windmill St", 40f, "MPH"),
            new Street("Worm St", 40f, "MPH"),
            new Street("Castle Tunnel", 40f, "MPH"),
            new Street("Castle Drive", 40f, "MPH"),
            new Street("President Ave", 40f, "MPH"),
            new Street("Union Drive East", 40f, "MPH"),
            new Street("West Way", 40f, "MPH"),
            new Street("Astoria", 40f, "MPH"),
            new Street("President St", 40f, "MPH"),
            new Street("Union Drive West", 40f, "MPH"),
            new Street("Amsterdam Lane", 40f, "MPH"),
            new Street("Anvil Ave", 40f, "MPH"),
            new Street("Applewhite St", 40f, "MPH"),
            new Street("Argus St", 40f, "MPH"),
            new Street("Bear St", 40f, "MPH"),
            new Street("Beaverhead Ave", 40f, "MPH"),
            new Street("Bedrock St", 40f, "MPH"),
            new Street("Boyden Ave", 40f, "MPH"),
            new Street("Bridger St", 40f, "MPH"),
            new Street("Cariboo Ave", 40f, "MPH"),
            new Street("Catskill Ave", 40f, "MPH"),
            new Street("Cockerell Ave", 40f, "MPH"),
            new Street("Edison Ave", 40f, "MPH"),
            new Street("Emery St", 40f, "MPH"),
            new Street("Fleming St", 40f, "MPH"),
            new Street("Fulcrum Ave", 40f, "MPH"),
            new Street("Grenadier St", 40f, "MPH"),
            new Street("Grommet St", 40f, "MPH"),
            new Street("Hardtack Ave", 40f, "MPH"),
            new Street("Hubbard Ave", 40f, "MPH"),
            new Street("Julin Ave", 40f, "MPH"),
            new Street("Kemeny St", 40f, "MPH"),
            new Street("Keneckie Ave", 40f, "MPH"),
            new Street("Latchkey Ave", 40f, "MPH"),
            new Street("Lemhi St", 40f, "MPH"),
            new Street("Lockowski Ave", 40f, "MPH"),
            new Street("Long John Ave", 40f, "MPH"),
            new Street("Lyndon Ave", 40f, "MPH"),
            new Street("Mahesh Ave", 40f, "MPH"),
            new Street("Moog St", 40f, "MPH"),
            new Street("Muskteer Ave", 40f, "MPH"),
            new Street("Niblick St", 40f, "MPH"),
            new Street("Nougat St", 40f, "MPH"),
            new Street("Odhner Ave", 40f, "MPH"),
            new Street("Owl Creek Ave", 40f, "MPH"),
            new Street("Plumbbob Ave", 40f, "MPH"),
            new Street("Praetorian Ave", 40f, "MPH"),
            new Street("Rael Ave", 40f, "MPH"),
            new Street("Rand Ave", 40f, "MPH"),
            new Street("Red Wing Ave", 40f, "MPH"),
            new Street("Sacramento Ave", 40f, "MPH"),
            new Street("Schneider Ave", 40f, "MPH"),
            new Street("Sculpin Ave", 40f, "MPH"),
            new Street("Storax Rd", 40f, "MPH"),
            new Street("Strower Ave", 40f, "MPH"),
            new Street("Tenmile St", 40f, "MPH"),
            new Street("Tinderbox Ave", 40f, "MPH"),
            new Street("Toggle Ave", 40f, "MPH"),
            new Street("Vitullo Ave", 40f, "MPH"),
            new Street("Mueri St", 40f, "MPH"),
            new Street("Amethyst St", 40f, "MPH"),
            new Street("Jade St", 40f, "MPH"),
            new Street("Kunzite St", 40f, "MPH"),
            new Street("Lorimar St", 40f, "MPH"),
            new Street("Manganese St", 40f, "MPH"),
            new Street("Nickel St", 40f, "MPH"),
            new Street("Obsidian St", 40f, "MPH"),
            new Street("Pyrite St", 40f, "MPH"),
            new Street("Quartz St", 40f, "MPH"),
            new Street("Ruby St", 40f, "MPH"),
            new Street("Silicon St", 40f, "MPH"),
            new Street("Barium St", 40f, "MPH"),
            new Street("Topaz St", 40f, "MPH"),
            new Street("Uranium St", 40f, "MPH"),
            new Street("Vauxite St", 40f, "MPH"),
            new Street("Wardite St", 40f, "MPH"),
            new Street("Xenotime St", 40f, "MPH"),
            new Street("Calcium St", 40f, "MPH"),
            new Street("Diamond St", 40f, "MPH"),
            new Street("Emerald St", 40f, "MPH"),
            new Street("Feldspar St", 40f, "MPH"),
            new Street("Garnet St", 40f, "MPH"),
            new Street("Hematite St", 40f, "MPH"),
            new Street("Iron St", 40f, "MPH"),
        };
        Serialization.SerializeParams(LCStreetsList, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LibertyConfigFolder}\\Streets_{StaticStrings.LibertyConfigSuffix}.xml");

        Serialization.SerializeParams(LCStreetsList, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LPPConfigFolder}\\Streets_{StaticStrings.LPPConfigSuffix}.xml");
    }
    private void DefaultConfig_SunshineDream()
    {
        List<Street> LCStreetsList = new List<Street>
        {
            new Street("Alton Road", 40f, "MPH"),
            new Street("63rd Street",50f,"MPH"),

            new Street() {
            Name = "Alton Rd",

            },
            new Street() {
            Name = "63rd Street",
            SpeedLimit = 35f,
            DisplayName = "63rd St",
            },
            new Street() {
            Name = "41st Street",
            SpeedLimit = 35f,
            DisplayName = "41st St",
            },
            new Street() {
            Name = "23rd Street",
            SpeedLimit = 35f,
            DisplayName = "23rd St",
            },
            new Street() {
            Name = "21st Street",
            SpeedLimit = 35f,
            DisplayName = "21st St",
            },
            new Street() {
            Name = "17th Street",
            SpeedLimit = 30f,
            DisplayName = "17th St",
            },
            new Street() {
            Name = "16th Street",
            SpeedLimit = 30f,
            DisplayName = "16th St",
            },
            new Street() {
            Name = "15th Street",
            SpeedLimit = 30f,
            DisplayName = "15th St",
            },
            new Street() {
            Name = "14th Street",
            SpeedLimit = 30f,
            DisplayName = "14th St",
            },
            new Street() {
            Name = "14th Place",
            SpeedLimit = 30f,
            DisplayName = "14th Pl",
            },
            new Street() {
            Name = "13th Street",
            SpeedLimit = 30f,
            DisplayName = "13th St",
            },
            new Street() {
            Name = "12th Street",
            SpeedLimit = 30f,
            DisplayName = "12th St",
            },
            new Street() {
            Name = "11th Street",
            SpeedLimit = 30f,
            DisplayName = "11th St",
            },
            new Street() {
            Name = "10th Street",
            SpeedLimit = 30f,
            DisplayName = "10th St",
            },
            new Street() {
            Name = "9th Street",
            SpeedLimit = 30f,
            DisplayName = "9th St",
            },
            new Street() {
            Name = "8th Street",
            SpeedLimit = 30f,
            DisplayName = "8th St",
            },
            new Street() {
            Name = "7th Street",
            SpeedLimit = 30f,
            DisplayName = "7th St",
            },
            new Street() {
            Name = "6th Street",
            SpeedLimit = 30f,
            DisplayName = "6th St",
            },
            new Street() {
            Name = "5th Street",
            SpeedLimit = 30f,
            DisplayName = "5th St",
            },
            new Street() {
            Name = "4th Street",
            SpeedLimit = 30f,
            DisplayName = "4th St",
            },
            new Street() {
            Name = "3rd Street",
            SpeedLimit = 30f,
            DisplayName = "3rd St",
            },
            new Street() {
            Name = "2nd Street",
            SpeedLimit = 30f,
            DisplayName = "2nd St",
            },
            new Street() {
            Name = "1st Street",
            SpeedLimit = 30f,
            DisplayName = "1st St",
            },
            new Street() {
            Name = "South Point Drive",
            SpeedLimit = 30f,
            DisplayName = "South Point Dr",
            },
            new Street() {
            Name = "North Miami Drive",
            SpeedLimit = 30f,
            DisplayName = "N Miami Dr",
            },
            new Street() {
            Name = "Ocean Drive",
            SpeedLimit = 30f,
            DisplayName = "Ocean Dr",
            },
            new Street() {
            Name = "Washington Ave",
            SpeedLimit = 35f,

            },
            new Street() {
            Name = "Michigan Ave",
            SpeedLimit = 35f,

            },
            new Street() {
            Name = "6th Dilido Terrace",
            SpeedLimit = 25f,
            DisplayName = "6th Dilido Tr",
            },
            new Street() {
            Name = "5th Dilido Terrace",
            SpeedLimit = 25f,
            DisplayName = "5th Dilido Tr",
            },
            new Street() {
            Name = "4th Dilido Terrace",
            SpeedLimit = 25f,
            DisplayName = "4th Dilido Tr",
            },
            new Street() {
            Name = "3rd Dilido Terrace",
            SpeedLimit = 25f,
            DisplayName = "3rd Dilido Tr",
            },
            new Street() {
            Name = "2nd Dilido Terrace",
            SpeedLimit = 25f,
            DisplayName = "2nd Dilido Tr",
            },
            new Street() {
            Name = "1st Dilido Terrace",
            SpeedLimit = 25f,
            DisplayName = "1st Dilido Tr",
            },
            new Street() {
            Name = "East Dilido Drive",
            SpeedLimit = 25f,
            DisplayName = "E Dilido Dr",
            },
            new Street() {
            Name = "West Dilido Drive",
            SpeedLimit = 25f,
            DisplayName = "W Dilido Dr",
            },
            new Street() {
            Name = "2nd San Marino Terrace",
            SpeedLimit = 25f,
            DisplayName = "2nd San Marino Tr",
            },
            new Street() {
            Name = "1st San Marino Terrace",
            SpeedLimit = 25f,
            DisplayName = "1st San Marino Tr",
            },
            new Street() {
            Name = "East San Marino Drive",
            SpeedLimit = 25f,
            DisplayName = "E San Marino Dr",
            },
            new Street() {
            Name = "West San Marino Drive",
            SpeedLimit = 25f,
            DisplayName = "W San Marino Dr",
            },
            new Street() {
            Name = "Española Way",
            SpeedLimit = 30f,
            DisplayName = "Española Wy",
            },
            new Street() {
            Name = "Lenox Ave",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "Asia Way",
            SpeedLimit = 25f,
            DisplayName = "Asia Wy",
            },
            new Street() {
            Name = "Australia Way",
            SpeedLimit = 25f,
            DisplayName = "Australia Wy",
            },
            new Street() {
            Name = "Bridge Road",
            SpeedLimit = 30f,
            DisplayName = "Bridge Rd",
            },
            new Street() {
            Name = "Port Boulevard",
            SpeedLimit = 30f,
            DisplayName = "Port Blvd",
            },
            new Street() {
            Name = "Biscayne Boulevard",
            DisplayName = "Biscayne Blvd",
            },
            new Street() {
            Name = "Lincoln Road",
            SpeedLimit = 30f,
            DisplayName = "Lincoln Rd",
            },
            new Street() {
            Name = "Pine Tree Drive",
            SpeedLimit = 30f,
            DisplayName = "Pine Tree Dr",
            },
            new Street() {
            Name = "Indian Creek Drive",
            SpeedLimit = 30f,
            DisplayName = "Indian Creek Dr",
            },
            new Street() {
            Name = "Convention Center Drive",
            SpeedLimit = 25f,
            DisplayName = "Convention Center Dr",
            },
            new Street() {
            Name = "North Venetian Way",
            SpeedLimit = 30f,
            DisplayName = "N Venetian Wy",
            },
            new Street() {
            Name = "South Venetian Way",
            SpeedLimit = 30f,
            DisplayName = "S Venetian Wy",
            },
            new Street() {
            Name = "Venetian Causway",
            IsHighway = true,
            DisplayName = "Venetian Cswy",
            },
            new Street() {
            Name = "Venetian Way",
            IsHighway = true,
            DisplayName = "Venetian Wy",
            },
            new Street() {
            Name = "MacArthur Causeway",
            IsHighway = true,
            DisplayName = "Venetian Cswy",
            },
            new Street() {
            Name = "West Ave",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "Collins Ave",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "Meridian Ave",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "Prairie Ave",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "East Flagler St",
            SpeedLimit = 30f,
            DisplayName = "E Flagler St",
            },
            new Street() {
            Name = "West Flagler St",
            SpeedLimit = 30f,
            DisplayName = "W Flagler St",
            },
            new Street() {
            Name = "Brickell Ave",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "Coral Way",
            SpeedLimit = 30f,
            DisplayName = "Coral Wy",
            },
            new Street() {
            Name = "Hernando St",
            SpeedLimit = 25f,

            },
            new Street() {
            Name = "Biltmore Way",
            SpeedLimit = 25f,
            DisplayName = "Biltmore Wy",
            },
            new Street() {
            Name = "De Soto Blvd",
            SpeedLimit = 25f,

            },
            new Street() {
            Name = "Granada Blvd",
            SpeedLimit = 25f,

            },
            new Street() {
            Name = "Alhambra Circle",
            SpeedLimit = 25f,
            DisplayName = "Alhambra Cr",
            },
            new Street() {
            Name = "Ferdinand St",
            SpeedLimit = 25f,

            },
            new Street() {
            Name = "Chopin Plaza",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "Grand Ave",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "Oak Ave",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "Virginia St",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "Mary St",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "Tigertail Ave",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "Matilda St",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "Florida Ave",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "Dolphin East-West Expressway",
            IsHighway = true,
            DisplayName = "Dolphin Expwy",
            },
            new Street() {
            Name = "Interstate-95",
            IsHighway = true,
            DisplayName = "I-95",
            },
            new Street() {
            Name = "I-95 Northbound",
            IsHighway = true,
            DisplayName = "I-95 North",
            },
            new Street() {
            Name = "I-95 Southbound",
            IsHighway = true,
            DisplayName = "I-95 South",
            },
            new Street() {
            Name = "Dolphin Expressway Eastbound",
            IsHighway = true,
            DisplayName = "Dolphin Expwy East",
            },
            new Street() {
            Name = "Dolphin Expressway Westbound",
            IsHighway = true,
            DisplayName = "Dolphin Expwy West",
            },
            new Street() {
            Name = "South Dixie Highway",
            IsHighway = true,
            DisplayName = "S Dixie Hwy",
            },
            new Street() {
            Name = "Biscayne Blvd Way",
            SpeedLimit = 30f,
            DisplayName = "Biscayne Blvd Wy",
            },
            new Street() {
            Name = "NW 1st St",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "NW 3rd St",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "NW 6th St",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "NW 7th St",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "NW 10th St",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "NW 12th St",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "NW 17th St",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "NW 7th Ct",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "NW Miami Ct",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "SW 1st St",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "SW 3rd Ave",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "SW 4th Ave",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "SW 5th Ave",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "SW 7th Ave",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "SW 13th Ave",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "SW 27th Ave",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "SW 15th Road",
            SpeedLimit = 30f,
            DisplayName = "SW 15th Rd",
            },
            new Street() {
            Name = "NE 1st ave",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "NE 3rd St",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "NE 6th St",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "NE 8th St",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "NE 15th St",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "SE 1st St",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "SE 2nd St",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "SE 3rd St",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "SE 4th St",
            SpeedLimit = 30f,

            },
            new Street() {
            Name = "Mercy Way",
            SpeedLimit = 30f,
            DisplayName = "Mercy Wy",
            },
            new Street() {
            Name = "Tamiami Trail",
            SpeedLimit = 30f,
            DisplayName = "Tamiami Tr",
            },
            new Street() {
            Name = "South Miami Drive",
            SpeedLimit = 30f,
            DisplayName = "S Miami Dr",
            },
            new Street() {
            Name = "South Miami Ave",
            SpeedLimit = 30f,
            DisplayName = "S Miami Ave",
            },
            new Street() {
            Name = "South Bayshore Drive",
            SpeedLimit = 30f,
            DisplayName = "S Bayshore Dr",
            },
            new Street() {
            Name = "North Tamiami Trail",
            SpeedLimit = 30f,
            DisplayName = "N Tamiami Tr",
            },
            new Street() {
            Name = "South Tamiami Trail",
            SpeedLimit = 30f,
            DisplayName = "S Tamiami Tr",
            },
            new Street() {
            Name = "McDonald St",
            SpeedLimit = 30f,

            },



        };
        Serialization.SerializeParams(LCStreetsList, "Plugins\\LosSantosRED\\AlternateConfigs\\SunshineDream\\Streets_SunshineDream.xml");
    }
}

