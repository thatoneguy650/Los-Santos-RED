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
    private List<Street> StreetsList;
    public void ReadConfig()
    {
        if (File.Exists(ConfigFileName))
        {
            StreetsList = Serialization.DeserializeParams<Street>(ConfigFileName);
        }
        else
        {
            DefaultConfig();
            Serialization.SerializeParams(StreetsList, ConfigFileName);
        }
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
    private void DefaultConfig()
    {
        StreetsList = new List<Street>
        {
            new Street("Joshua Rd", 50f, "MPH"),
            new Street("East Joshua Road", 50f, "MPH"),
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
            new Street("Senora Rd", 40f, "MPH"),
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
            new Street("Abe Milton Pkwy", 35f, "MPH"),
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
    }
}

