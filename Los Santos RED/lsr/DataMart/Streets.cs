using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


public class Streets
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
    //public void GetStreetPositionandHeading(Vector3 PositionNear, out Vector3 SpawnPosition, out float Heading, bool MainRoadsOnly)
    //{
    //    Vector3 pos = PositionNear;
    //    SpawnPosition = Vector3.Zero;
    //    Heading = 0f;

    //    Vector3 outPos;
    //    float heading;
    //    float val;

    //    if (MainRoadsOnly)
    //    {
    //        unsafe
    //        {
    //            NativeFunction.CallByName<bool>("GET_CLOSEST_VEHICLE_NODE_WITH_HEADING", pos.X, pos.Y, pos.Z, &outPos, &heading, 0, 3, 0);
    //        }

    //        SpawnPosition = outPos;
    //        Heading = heading;
    //    }
    //    else
    //    {
    //        for (int i = 1; i < 40; i++)
    //        {
    //            unsafe
    //            {
    //                NativeFunction.CallByName<bool>("GET_NTH_CLOSEST_VEHICLE_NODE_WITH_HEADING", pos.X, pos.Y, pos.Z, i, &outPos, &heading, &val, 1, 0x40400000, 0);
    //            }
    //            if (!NativeFunction.CallByName<bool>("IS_POINT_OBSCURED_BY_A_MISSION_ENTITY", outPos.X, outPos.Y, outPos.Z, 5.0f, 5.0f, 5.0f, 0))
    //            {
    //                SpawnPosition = outPos;
    //                Heading = heading;
    //                break;
    //            }
    //        }
    //    }
    //}
    //public void GetSidewalkPositionAndHeading(Vector3 PositionNear, out Vector3 SpawnPosition)
    //{
    //    Vector3 pos = PositionNear;
    //    SpawnPosition = Vector3.Zero;
    //    Vector3 outPos;
    //    unsafe
    //    {
    //        NativeFunction.CallByName<bool>("GET_SAFE_COORD_FOR_PED", pos.X, pos.Y, pos.Z, true, &outPos, 16);
    //    }

    //    SpawnPosition = outPos;
    //}
    private string GetStreetName(Vector3 Position)
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
    private void DefaultConfig()
    {
        StreetsList = new List<Street>
        {
            new Street("Joshua Rd", 50f),
            new Street("East Joshua Road", 50f),
            new Street("Marina Dr", 35f),
            new Street("Alhambra Dr", 35f),
            new Street("Niland Ave", 35f),
            new Street("Zancudo Ave", 35f),
            new Street("Armadillo Ave", 35f),
            new Street("Algonquin Blvd", 35f),
            new Street("Mountain View Dr", 35f),
            new Street("Cholla Springs Ave", 35f),
            new Street("Panorama Dr", 40f),
            new Street("Lesbos Ln", 35f),
            new Street("Calafia Rd", 30f),
            new Street("North Calafia Way", 30f),
            new Street("Cassidy Trail", 25f),
            new Street("Seaview Rd", 35f),
            new Street("Grapeseed Main St", 35f),
            new Street("Grapeseed Ave", 35f),
            new Street("Joad Ln", 35f),
            new Street("Union Rd", 40f),
            new Street("O'Neil Way", 25f),
            new Street("Senora Fwy", 65f, true),
            new Street("Catfish View", 35f),
            new Street("Great Ocean Hwy", 60f, true),
            new Street("Paleto Blvd", 35f),
            new Street("Duluoz Ave", 35f),
            new Street("Procopio Dr", 35f),
            new Street("Cascabel Ave", 30f),
            new Street("Peaceful St", 30f),
            new Street("Procopio Promenade", 25f),
            new Street("Pyrite Ave", 30f),
            new Street("Fort Zancudo Approach Rd", 25f),
            new Street("Barbareno Rd", 30f),
            new Street("Ineseno Road", 30f),
            new Street("West Eclipse Blvd", 35f),
            new Street("Playa Vista", 30f),
            new Street("Bay City Ave", 30f),
            new Street("Del Perro Fwy", 65f, true),
            new Street("Equality Way", 30f),
            new Street("Red Desert Ave", 30f),
            new Street("Magellan Ave", 25f),
            new Street("Sandcastle Way", 30f),
            new Street("Vespucci Blvd", 40f),
            new Street("Prosperity St", 30f),
            new Street("San Andreas Ave", 40f),
            new Street("North Rockford Dr", 35f),
            new Street("South Rockford Dr", 35f),
            new Street("Marathon Ave", 30f),
            new Street("Boulevard Del Perro", 35f),
            new Street("Cougar Ave", 30f),
            new Street("Liberty St", 30f),
            new Street("Bay City Incline", 40f),
            new Street("Conquistador St", 25f),
            new Street("Cortes St", 25f),
            new Street("Vitus St", 25f),
            new Street("Aguja St", 25f),/////maytbe????!?!?!
            new Street("Goma St", 25f),
            new Street("Melanoma St", 25f),
            new Street("Palomino Ave", 35f),
            new Street("Invention Ct", 25f),
            new Street("Imagination Ct", 25f),
            new Street("Rub St", 25f),
            new Street("Tug St", 25f),
            new Street("Ginger St", 30f),
            new Street("Lindsay Circus", 30f),
            new Street("Calais Ave", 35f),
            new Street("Adam's Apple Blvd", 40f),
            new Street("Alta St", 40f),
            new Street("Integrity Way", 30f),
            new Street("Swiss St", 30f),
            new Street("Strawberry Ave", 40f),
            new Street("Capital Blvd", 30f),
            new Street("Crusade Rd", 30f),
            new Street("Innocence Blvd", 40f),
            new Street("Davis Ave", 40f),
            new Street("Little Bighorn Ave", 35f),
            new Street("Roy Lowenstein Blvd", 35f),
            new Street("Jamestown St", 30f),
            new Street("Carson Ave", 35f),
            new Street("Grove St", 30f),
            new Street("Brouge Ave", 30f),
            new Street("Covenant Ave", 30f),
            new Street("Dutch London St", 40f),
            new Street("Signal St", 30f),
            new Street("Elysian Fields Fwy", 50f, true),
            new Street("Plaice Pl", 30f),
            new Street("Chum St", 40f),
            new Street("Chupacabra St", 30f),
            new Street("Miriam Turner Overpass", 60f),
            new Street("Autopia Pkwy", 35f),
            new Street("Exceptionalists Way", 35f),
            new Street("La Puerta Fwy", 60f, true),
            new Street("New Empire Way", 30f),
            new Street("Runway1", 90f),
            new Street("Greenwich Pkwy", 35f),
            new Street("Kortz Dr", 30f),
            new Street("Banham Canyon Dr", 40f),
            new Street("Buen Vino Rd", 40f),
            new Street("Route 68", 55f,true),
            new Street("Zancudo Grande Valley", 40f),
            new Street("Zancudo Barranca", 40f),
            new Street("Galileo Rd", 40f),
            new Street("Mt Vinewood Dr", 40f),
            new Street("Marlowe Dr", 40f),
            new Street("Milton Rd", 35f),
            new Street("Kimble Hill Dr", 35f),
            new Street("Normandy Dr", 35f),
            new Street("Hillcrest Ave", 35f),
            new Street("Hillcrest Ridge Access Rd", 35f),
            new Street("North Sheldon Ave", 35f),
            new Street("Lake Vinewood Dr", 35f),
            new Street("Lake Vinewood Est", 35f),
            new Street("Baytree Canyon Rd", 40f),
            new Street("North Conker Ave", 35f),
            new Street("Wild Oats Dr", 35f),
            new Street("Whispymound Dr", 35f),
            new Street("Didion Dr", 35f),
            new Street("Cox Way", 35f),
            new Street("Picture Perfect Drive", 35f),
            new Street("South Mo Milton Dr", 35f),
            new Street("Cockingend Dr", 35f),
            new Street("Mad Wayne Thunder Dr", 35f),
            new Street("Hangman Ave", 35f),
            new Street("Dunstable Ln", 35f),
            new Street("Dunstable Dr", 35f),
            new Street("Greenwich Way", 35f),
            new Street("Greenwich Pl", 35f),
            new Street("Hardy Way", 35f),
            new Street("Richman St", 35f),
            new Street("Ace Jones Dr", 35f),
            new Street("Los Santos Freeway", 65f, true),
            new Street("Senora Rd", 40f),
            new Street("Nowhere Rd", 25f),
            new Street("Smoke Tree Rd", 35f),
            new Street("Cholla Rd", 35f),
            new Street("Cat-Claw Ave", 35f),
            new Street("Senora Way", 40f),
            new Street("Palomino Fwy", 60f, true),
            new Street("Shank St", 25f),
            new Street("Macdonald St", 35f),
            new Street("Route 68 Approach", 55f),
            new Street("Vinewood Park Dr", 35f),
            new Street("Vinewood Blvd", 40f),
            new Street("Mirror Park Blvd", 35f),
            new Street("Glory Way", 35f),
            new Street("Bridge St", 35f),
            new Street("West Mirror Drive", 35f),
            new Street("Nikola Ave", 35f),
            new Street("East Mirror Dr", 35f),
            new Street("Nikola Pl", 25f),
            new Street("Mirror Pl", 35f),
            new Street("El Rancho Blvd", 40f),
            new Street("Olympic Fwy", 60f, true),
            new Street("Fudge Ln", 25f),
            new Street("Amarillo Vista", 25f),
            new Street("Labor Pl", 35f),
            new Street("El Burro Blvd", 35f),
            new Street("Sustancia Rd", 45f),
            new Street("South Shambles St", 30f),
            new Street("Hanger Way", 30f),
            new Street("Orchardville Ave", 30f),
            new Street("Popular St", 40f),
            new Street("Buccaneer Way", 45f),
            new Street("Abattoir Ave", 35f),
            new Street("Voodoo Place", 30f),
            new Street("Mutiny Rd", 35f),
            new Street("South Arsenal St", 35f),
            new Street("Forum Dr", 35f),
            new Street("Morningwood Blvd", 35f),
            new Street("Dorset Dr", 40f),
            new Street("Caesars Place", 25f),
            new Street("Spanish Ave", 30f),
            new Street("Portola Dr", 30f),
            new Street("Edwood Way", 25f),
            new Street("San Vitus Blvd", 40f),
            new Street("Eclipse Blvd", 35f),
            new Street("Gentry Lane", 30f),
            new Street("Las Lagunas Blvd", 40f),
            new Street("Power St", 40f),
            new Street("Mt Haan Rd", 40f),
            new Street("Elgin Ave", 40f),
            new Street("Hawick Ave", 35f),
            new Street("Meteor St", 30f),
            new Street("Alta Pl", 30f),
            new Street("Occupation Ave", 35f),
            new Street("Carcer Way", 40f),
            new Street("Eastbourne Way", 30f),
            new Street("Rockford Dr", 35f),
            new Street("Abe Milton Pkwy", 35f),
            new Street("Laguna Pl", 30f),
            new Street("Sinners Passage", 30f),
            new Street("Atlee St", 30f),
            new Street("Sinner St", 30f),
            new Street("Supply St", 30f),
            new Street("Amarillo Way", 35f),
            new Street("Tower Way", 35f),
            new Street("Decker St", 35f),
            new Street("Tackle St", 25f),
            new Street("Low Power St", 35f),
            new Street("Clinton Ave", 35f),
            new Street("Fenwell Pl", 35f),
            new Street("Utopia Gardens", 25f),
            new Street("Cavalry Blvd", 35f),
            new Street("South Boulevard Del Perro", 35f),
            new Street("Americano Way", 25f),
            new Street("Sam Austin Dr", 25f),
            new Street("East Galileo Ave", 35f),
            new Street("Galileo Park", 35f),
            new Street("West Galileo Ave", 35f),
            new Street("Tongva Dr", 40f),
            new Street("Zancudo Rd", 35f),
            new Street("Movie Star Way", 35f),
            new Street("Heritage Way", 35f),
            new Street("Perth St", 25f),
            new Street("Chianski Passage", 30f),
            new Street("Lolita Ave", 35f),
            new Street("Meringue Ln", 35f),
            new Street("Strangeways Dr", 30f),

            new Street("Mt Haan Dr", 40f)
        };
    }
}

