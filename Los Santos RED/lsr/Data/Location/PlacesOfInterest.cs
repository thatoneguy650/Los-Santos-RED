using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PlacesOfInterest : IPlacesOfInterest
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Locations.xml";
    private List<GameLocation> LocationsList;
    private IConsumableSubstances ConsumableSubstances;

    public PlacesOfInterest(IConsumableSubstances consumableSubstances)
    {
        ConsumableSubstances = consumableSubstances;
    }

    public void ReadConfig()
    {
        if (File.Exists(ConfigFileName))
        {
            LocationsList = Serialization.DeserializeParams<GameLocation>(ConfigFileName);
        }
        else
        {
            DefaultConfig();
            Serialization.SerializeParams(LocationsList, ConfigFileName);
        }
    }
    public List<GameLocation> GetAllPlaces()
    {
        return LocationsList;
    }
    public GameLocation GetClosestLocation(Vector3 Position,LocationType Type)
    {
        return LocationsList.Where(x => x.Type == Type).OrderBy(s => Position.DistanceTo2D(s.LocationPosition)).FirstOrDefault();
    }
    public List<GameLocation> GetLocations(LocationType Type)
    {
        return LocationsList.Where(x => x.Type == Type).ToList();
    }
    private void DefaultConfig()
    {


        ConsumableSubstance Cigarette = ConsumableSubstances.Get("Redwood Cigarette");
        ConsumableSubstance Joint = ConsumableSubstances.Get("Joint");
        ConsumableSubstance Cigar = ConsumableSubstances.Get("Estancia Cigar");
        ConsumableSubstance Donut = ConsumableSubstances.Get("Donut");
        ConsumableSubstance Pizza = ConsumableSubstances.Get("Slice of Pizza");
        ConsumableSubstance HotDog = ConsumableSubstances.Get("Hot Dog");
        ConsumableSubstance Hamburger = ConsumableSubstances.Get("Hamburger");

        ConsumableSubstance fortyoz = ConsumableSubstances.Get("40 oz");
        ConsumableSubstance ambeer = ConsumableSubstances.Get("Bottle of A.M.");
        ConsumableSubstance pibbeer = ConsumableSubstances.Get("Bottle of PiBwasser");
        ConsumableSubstance barrachobeer = ConsumableSubstances.Get("Bottle of Barracho");
        ConsumableSubstance blarneysbeer = ConsumableSubstances.Get("Bottle of Blarneys");
        ConsumableSubstance jakeysbeer = ConsumableSubstances.Get("Bottle of Jakeys");
        ConsumableSubstance loggerbeer = ConsumableSubstances.Get("Bottle of Logger");
        ConsumableSubstance patriotbeer = ConsumableSubstances.Get("Bottle of Patriot");
        ConsumableSubstance pridebeer = ConsumableSubstances.Get("Bottle of Pride");
        ConsumableSubstance strongbeer = ConsumableSubstances.Get("Bottle of Stronz");
        ConsumableSubstance duschebeer = ConsumableSubstances.Get("Bottle of Dusche");

        ConsumableSubstance canexola = ConsumableSubstances.Get("Can of eCola");
        ConsumableSubstance cansprunk = ConsumableSubstances.Get("Can of Sprunk");
        ConsumableSubstance cupecola = ConsumableSubstances.Get("Cup of eCola");
        ConsumableSubstance cupsprunk = ConsumableSubstances.Get("Cup of Sprunk");

        ConsumableSubstance coffee = ConsumableSubstances.Get("Cup of Coffee");

        ConsumableSubstance fries = ConsumableSubstances.Get("French Fries");
        ConsumableSubstance banana = ConsumableSubstances.Get("Banana");
        ConsumableSubstance chips = ConsumableSubstances.Get("Phat Chips");

        LocationsList = new List<GameLocation>
        {
            //Hospital
            new GameLocation(new Vector3(364.7124f, -583.1641f, 28.69318f), 280.637f, LocationType.Hospital, "Pill Box Hill Hospital"),
            new GameLocation(new Vector3(338.208f, -1396.154f, 32.50927f), 77.07102f, LocationType.Hospital, "Central Los Santos Hospital"),
            new GameLocation(new Vector3(1842.057f, 3668.679f, 33.67996f), 228.3818f, LocationType.Hospital, "Sandy Shores Hospital"),
            new GameLocation(new Vector3(-244.3214f, 6328.575f, 32.42618f), 219.7734f, LocationType.Hospital, "Paleto Bay Hospital"),

            //Grave
            new GameLocation(new Vector3(-1654.301f, -148.7047f, 59.91496f), 299.5774f, LocationType.Grave, "Grave 1"),

            //Police
            new GameLocation(new Vector3(358.9726f, -1582.881f, 29.29195f), 323.5287f, LocationType.Police, "Davis Police Station"),
            new GameLocation(new Vector3(1858.19f, 3679.873f, 33.75724f), 218.3256f, LocationType.Police, "Sandy Shores Police Station"),
            new GameLocation(new Vector3(-437.973f, 6021.403f, 31.49011f), 316.3756f, LocationType.Police, "Paleto Bay Police Station"),
            new GameLocation(new Vector3(440.0835f, -982.3911f, 30.68966f), 47.88088f, LocationType.Police, "Mission Row Police Station"),
            new GameLocation(new Vector3(815.8774f, -1290.531f, 26.28391f), 74.91704f, LocationType.Police, "La Mesa Police Station"),
            new GameLocation(new Vector3(642.1356f, -3.134667f, 82.78872f), 215.299f, LocationType.Police, "Vinewood Police Station"),
            new GameLocation(new Vector3(-557.0687f, -134.7315f, 38.20231f), 214.5968f, LocationType.Police, "Rockford Hills Police Station"),
            new GameLocation(new Vector3(-1093.817f, -807.1993f, 19.28864f), 22.23846f, LocationType.Police, "Vespucci Police Station"),
            new GameLocation(new Vector3(-1633.314f, -1010.025f, 13.08503f), 351.7007f, LocationType.Police, "Del Perro Police Station"),
            new GameLocation(new Vector3(-1311.877f, -1528.808f, 4.410581f), 233.9121f, LocationType.Police, "Vespucci Beach Police Station"),
            //Stores
            new GameLocation(new Vector3(-1226.09f, -896.166f, 12.4057f), 22.23846f, LocationType.ConvenienceStore, "Rob's Liquors"),
            new GameLocation(new Vector3(-709.68f, -923.198f, 19.0193f), 22.23846f, LocationType.ConvenienceStore, "LTD Little Seoul","A one-stop shop!"),
            new GameLocation(new Vector3(2560f, 385f, 107f), 22.23846f, LocationType.ConvenienceStore, "24/7 Supermarket #3664","As fast as you"),
            new GameLocation(new Vector3(547f, 2678f, 41f), 22.23846f, LocationType.ConvenienceStore, "24/7 Supermarket Harmony","As fast as you"),
            new GameLocation(new Vector3(-3236.767f,1005.609f,12.33137f), 122.6316f, LocationType.ConvenienceStore, "24/7 Supermarket Chumash","As fast as you"),
            new GameLocation(new Vector3(2683.969f,3282.098f,55.24052f), 89.53969f, LocationType.GasStation, "24/7 Supermarket Grande Senora" ,new List<Vector3>() { new Vector3(2678.073f, 3265.522f, 54.7076f),new Vector3(2681.173f, 3262.774f, 54.70736f) }),
            new GameLocation(new Vector3(1725f, 6410f, 35f), 22.23846f, LocationType.GasStation, "24/7 Mount Chilliad (Gas)",new List<Vector3>() { new Vector3(1706.173f, 6412.223f, 32.22713f), new Vector3(1701.657f, 6414.528f, 32.1186f), new Vector3(1697.71f, 6416.565f, 32.08189f),new Vector3(1706.173f, 6412.223f, 32.22713f)
                                                                                                                                                , new Vector3(1697.869f,6420.53f,32.05283f), new Vector3(1701.852f,6418.417f,32.05503f), new Vector3(1705.852f,6416.659f,32.05479f) }),
            new GameLocation(new Vector3(-1429.33f,-270.8909f,46.2077f), 325.7301f, LocationType.GasStation, "Ron Morningwood",new List<Vector3>() { new Vector3(-1428.23f,-277.0434f,45.79089f), new Vector3(-1436.362f,-267.6647f,45.79237f),
                                                                                                                                                          new Vector3(-1440.16f,-270.164f,45.79181f),new Vector3(-1431.339f,-280.2279f,45.79009f),
                                                                                                                                                          new Vector3(-1434.153f,-282.5898f,45.79139f), new Vector3(-1442.416f,-273.0687f,45.7986f),
                                                                                                                                                          new Vector3(-1446.231f,-276.2419f,45.80196f),new Vector3(-1437.798f,-285.5625f,45.77643f) }),
            new GameLocation(new Vector3(160.4977f,6635.249f,31.61175f), 70.88637f, LocationType.GasStation, "Dons Country Store & Gas",new List<Vector3>() { new Vector3(188.7231f,6607.43f,31.84954f), new Vector3(184.8491f,6606.112f,31.85245f), new Vector3(181.3225f,6605.81f,31.84829f)
                                                                                                                                                , new Vector3(178.1896f,6604.389f,31.89782f), new Vector3(174.0998f,6604.168f,31.84834f), new Vector3(171.1875f,6603.454f,32.04737f) }),
            new GameLocation(new Vector3(266.2746f,2599.669f,44.7383f), 231.9223f, LocationType.GasStation, "Harmony General Store & Gas" ,new List<Vector3>() { new Vector3(262.5423f, 2610.143f, 44.3814f),new Vector3(265.0521f, 2604.807f, 44.38421f) }),

            new GameLocation(new Vector3(1039.753f,2666.26f,39.55253f), 143.6208f, LocationType.GasStation, "Grande Senora Cafe & Gas",new List<Vector3>() { new Vector3(1043.293f,2677.189f,38.90083f), new Vector3(1035.182f,2677.444f,38.90271f), new Vector3(1034.835f,2670.396f,38.9343f)
                                                                                                                                                , new Vector3(1043.761f,2670.147f,38.94082f), new Vector3(1043.412f,2672.185f,38.95105f), new Vector3(1034.338f,2672.41f,38.94936f) }),


            new GameLocation(new Vector3(-1817.871f,787.0063f,137.917f), 89.38248f, LocationType.GasStation, "LTD Richmond Glen",new List<Vector3>() { new Vector3(-1804.758f,792.6874f,138.5142f), new Vector3(-1809.721f,798.1087f,138.5137f),
                                                                                                                                                          new Vector3(-1807.576f,801.251f,138.5144f),new Vector3(-1802.39f,796.0137f,138.5141f),
                                                                                                                                                          new Vector3(-1798.391f,798.9479f,138.5154f), new Vector3(-1802.964f,805.196f,138.5681f),
                                                                                                                                                          new Vector3(-1800.726f,807.3672f,138.515f),new Vector3(-1796.539f,803.0259f,138.5148f),
                                                                                                                                                          new Vector3(-1792.259f,804.6542f,138.5133f),new Vector3(-1796.816f,810.0197f,138.5144f),
                                                                                                                                                          new Vector3(-1794.411f,813.2302f,138.5146f),new Vector3(-1789.586f,808.2133f,138.5163f)}),



            new GameLocation(new Vector3(-1516.382f, -952.5892f, 9.278718f), 317.7292f, LocationType.FoodStand, "Del Perro Beach Hot Dog","Mostly Meat!") { SellableItems = new List<ConsumableSubstance>(){ HotDog, cansprunk, banana } },
            new GameLocation(new Vector3(-1539.045f, -900.472f, 10.16951f), 129.0318f, LocationType.FoodStand, "Del Perro Food Market","No Robberies Please!"){ SellableItems = new List<ConsumableSubstance>(){ Joint, Cigarette, Cigar,cupsprunk, fortyoz, barrachobeer, pibbeer, blarneysbeer, loggerbeer, patriotbeer, pridebeer, strongbeer } },
            new GameLocation(new Vector3(-1334.007f, -1282.623f, 4.835985f), 115.3464f, LocationType.FoodStand, "Slice N Dice Pizza","Slice UP!"){ SellableItems = new List<ConsumableSubstance>(){ Pizza, cupsprunk, ambeer, pibbeer, barrachobeer, blarneysbeer, jakeysbeer, strongbeer, duschebeer }},
            new GameLocation(new Vector3(-1296.815f, -1387.3f, 4.544102f), 112.4694f, LocationType.FoodStand, "Sharkies Bites","Take A Bite Today!"){ SellableItems = new List<ConsumableSubstance>(){ HotDog, chips, Donut, cupecola, coffee } },
            new GameLocation(new Vector3(-1268.011f, -1432.715f, 4.353373f), 134.2259f, LocationType.FoodStand, "Beefy Bills Burgers","Extra BEEFY!"){ SellableItems = new List<ConsumableSubstance>(){ Hamburger, fries, cansprunk } },
            new GameLocation(new Vector3(-1248.932f, -1474.449f, 4.277946f), 306.3787f, LocationType.FoodStand, "Vespucci Hot Dogs","Vegan? No. Meat? Yes."){ SellableItems = new List<ConsumableSubstance>(){ HotDog, chips, canexola } },


    };
    }
}

