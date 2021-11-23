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
    public List<GameLocation> GetAllStores()
    {
        return LocationsList.Where(x => x.CanPurchase).ToList();
    }
    public GameLocation GetClosestLocation(Vector3 Position,LocationType Type)
    {
        return LocationsList.Where(x => x.Type == Type).OrderBy(s => Position.DistanceTo2D(s.EntrancePosition)).FirstOrDefault();
    }
    public List<GameLocation> GetLocations(LocationType Type)
    {
        return LocationsList.Where(x => x.Type == Type).ToList();
    }
    private void DefaultConfig()
    {

        //Hot Dog Cart Menu
        ConsumableSubstance HotDog = ConsumableSubstances.Get("Hot Dog");
        ConsumableSubstance HotSausage = ConsumableSubstances.Get("Hot Sausage");
        ConsumableSubstance HotPretzel = ConsumableSubstances.Get("Hot Pretzel");
        ConsumableSubstance ThreeMiniPretzel = ConsumableSubstances.Get("3 Mini Pretzel");
        ConsumableSubstance nuts = ConsumableSubstances.Get("Nuts");
        ConsumableSubstance cansprunk = ConsumableSubstances.Get("Can of Sprunk");
        ConsumableSubstance bottleofwater = ConsumableSubstances.Get("Bottle of Water");
        ConsumableSubstance Burger = ConsumableSubstances.Get("Burger");
        ConsumableSubstance CheeseBurger = ConsumableSubstances.Get("Megacheese Burger");
        ConsumableSubstance DoubleBurger = ConsumableSubstances.Get("Double Burger");
        ConsumableSubstance KingsizeBurger = ConsumableSubstances.Get("Kingsize Burger");
        ConsumableSubstance BaconBurger = ConsumableSubstances.Get("Bacon Burger");
        ConsumableSubstance fries = ConsumableSubstances.Get("French Fries");
        ConsumableSubstance Cigarette = ConsumableSubstances.Get("Redwood Cigarette");
        ConsumableSubstance Joint = ConsumableSubstances.Get("Joint");
        ConsumableSubstance Cigar = ConsumableSubstances.Get("Estancia Cigar");
        ConsumableSubstance Donut = ConsumableSubstances.Get("Donut");
        ConsumableSubstance Pizza = ConsumableSubstances.Get("Slice of Pizza");
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
        ConsumableSubstance canecola = ConsumableSubstances.Get("Can of eCola");
        ConsumableSubstance cupecola = ConsumableSubstances.Get("Cup of eCola");
        ConsumableSubstance cupsprunk = ConsumableSubstances.Get("Cup of Sprunk");
        ConsumableSubstance coffee = ConsumableSubstances.Get("Cup of Coffee");    
        ConsumableSubstance banana = ConsumableSubstances.Get("Banana");
        ConsumableSubstance chips = ConsumableSubstances.Get("Phat Chips");


        ConsumableSubstance tripleburger = ConsumableSubstances.Get("Triple Burger");
        ConsumableSubstance bacontriplecheese = ConsumableSubstances.Get("Bacon Triple Cheese Melt");
        ConsumableSubstance jumboshake = ConsumableSubstances.Get("Jumbo Shake");

        ConsumableSubstance Taco = ConsumableSubstances.Get("Taco");


        List<ConsumableSubstance> GenericMenu = new List<ConsumableSubstance>() { Burger, chips, canecola, cansprunk, bottleofwater };


        List<ConsumableSubstance> HotDogMenu = new List<ConsumableSubstance>() { HotDog, HotSausage, HotPretzel, ThreeMiniPretzel, nuts, cansprunk, bottleofwater };
        List<ConsumableSubstance> BurgerMenu = new List<ConsumableSubstance>() { Burger, CheeseBurger, DoubleBurger, KingsizeBurger, BaconBurger, fries, canecola, cansprunk, bottleofwater };
        List<ConsumableSubstance> PizzaMenu = new List<ConsumableSubstance>() { Pizza, cupsprunk, ambeer, pibbeer, barrachobeer, blarneysbeer, jakeysbeer, strongbeer, duschebeer };
        List<ConsumableSubstance> DonutMenu = new List<ConsumableSubstance>() { HotDog, chips, Donut, canecola, cupecola, coffee };
        List<ConsumableSubstance> StoreMenu = new List<ConsumableSubstance>() { Joint, Cigarette, Cigar, cupsprunk, banana, Donut, HotPretzel, fortyoz, barrachobeer, pibbeer, blarneysbeer, loggerbeer, patriotbeer, pridebeer, strongbeer };
        List<ConsumableSubstance> FruitMenu = new List<ConsumableSubstance>() { banana, Joint, bottleofwater, nuts, chips, canecola, cansprunk };


        List<ConsumableSubstance> UpNAtomMenu = new List<ConsumableSubstance>() { tripleburger, bacontriplecheese, jumboshake, fries, BaconBurger, fries, cupecola, cupsprunk, coffee, bottleofwater };
        List<ConsumableSubstance> TacoFarmerMenu = new List<ConsumableSubstance>() { Taco, canecola, cansprunk, coffee, bottleofwater };
        List<ConsumableSubstance> HeadShopMenu = new List<ConsumableSubstance>() { Joint, Cigarette, Cigar, bottleofwater };

        List<ConsumableSubstance> LiquorStoreMenu = new List<ConsumableSubstance>() { fortyoz, barrachobeer, pibbeer, blarneysbeer, loggerbeer, patriotbeer, pridebeer, strongbeer, ambeer, jakeysbeer, duschebeer };

        List<ConsumableSubstance> BarMenu = new List<ConsumableSubstance>() { Burger,HotDog,bottleofwater,cupecola, fortyoz, barrachobeer, pibbeer, blarneysbeer, loggerbeer, patriotbeer, pridebeer, strongbeer, ambeer, jakeysbeer, duschebeer };

        List<ConsumableSubstance> CoffeeMenu = new List<ConsumableSubstance>() { coffee, Donut, Cigarette, bottleofwater };

        LocationsList = new List<GameLocation>
        {
            //Hospital
            new GameLocation(new Vector3(364.7124f, -583.1641f, 28.69318f), 280.637f, LocationType.Hospital, "Pill Box Hill Hospital",""),
            new GameLocation(new Vector3(338.208f, -1396.154f, 32.50927f), 77.07102f, LocationType.Hospital, "Central Los Santos Hospital",""),
            new GameLocation(new Vector3(1842.057f, 3668.679f, 33.67996f), 228.3818f, LocationType.Hospital, "Sandy Shores Hospital",""),
            new GameLocation(new Vector3(-244.3214f, 6328.575f, 32.42618f), 219.7734f, LocationType.Hospital, "Paleto Bay Hospital",""),

            //Grave
            new GameLocation(new Vector3(-1654.301f, -148.7047f, 59.91496f), 299.5774f, LocationType.Grave, "Grave 1",""),

            //Police
            new GameLocation(new Vector3(358.9726f, -1582.881f, 29.29195f), 323.5287f, LocationType.Police, "Davis Police Station",""),
            new GameLocation(new Vector3(1858.19f, 3679.873f, 33.75724f), 218.3256f, LocationType.Police, "Sandy Shores Police Station",""),
            new GameLocation(new Vector3(-437.973f, 6021.403f, 31.49011f), 316.3756f, LocationType.Police, "Paleto Bay Police Station",""),
            new GameLocation(new Vector3(440.0835f, -982.3911f, 30.68966f), 47.88088f, LocationType.Police, "Mission Row Police Station",""),
            new GameLocation(new Vector3(815.8774f, -1290.531f, 26.28391f), 74.91704f, LocationType.Police, "La Mesa Police Station",""),
            new GameLocation(new Vector3(642.1356f, -3.134667f, 82.78872f), 215.299f, LocationType.Police, "Vinewood Police Station",""),
            new GameLocation(new Vector3(-557.0687f, -134.7315f, 38.20231f), 214.5968f, LocationType.Police, "Rockford Hills Police Station",""),
            new GameLocation(new Vector3(-1093.817f, -807.1993f, 19.28864f), 22.23846f, LocationType.Police, "Vespucci Police Station",""),
            new GameLocation(new Vector3(-1633.314f, -1010.025f, 13.08503f), 351.7007f, LocationType.Police, "Del Perro Police Station",""),
            new GameLocation(new Vector3(-1311.877f, -1528.808f, 4.410581f), 233.9121f, LocationType.Police, "Vespucci Beach Police Station",""),
            
            
            //Stores

            //Liquor
            new GameLocation(new Vector3(-1226.09f, -896.166f, 12.4057f), 22.23846f,new Vector3(-1221.119f, -908.5667f, 12.32635f), 33.35855f, LocationType.LiquorStore, "Rob's Liquors","Thats My Name, Don't Rob Me!") { SellableItems = LiquorStoreMenu, OpenTime = 4, CloseTime = 22 },
            new GameLocation(new Vector3(-1208.469f, -1384.053f, 4.085135f), 68.08948f, LocationType.LiquorStore, "Steamboat Beers", "Steamboat Beers") { SellableItems = LiquorStoreMenu },
            new GameLocation(new Vector3(-1106.07f, -1287.686f, 5.421459f), 161.3398f, LocationType.LiquorStore, "Vespucci Liquor Market", "Vespucci Liquor Market") { SellableItems = LiquorStoreMenu },
            new GameLocation(new Vector3(-1486.196f, -377.7115f, 40.16343f), 133.357f,new Vector3(-1486.196f, -377.7115f, 40.16343f), 133.357f, LocationType.LiquorStore, "Rob's Liquors", "Rob's Liquors") { SellableItems = LiquorStoreMenu, OpenTime = 4, CloseTime = 22 },
            new GameLocation(new Vector3(-697.8242f, -1182.286f, 10.71113f), 132.7831f, LocationType.LiquorStore, "Liquor Market", "Liquor Market") { SellableItems = LiquorStoreMenu, OpenTime = 4, CloseTime = 22 },
            new GameLocation(new Vector3(-882.7062f, -1155.351f, 5.162508f), 215.8305f, LocationType.LiquorStore, "Liquor Hole", "Liquor Hole") { SellableItems = LiquorStoreMenu, OpenTime = 4, CloseTime = 22 },
            new GameLocation(new Vector3(456.5478f, 130.5207f, 99.28537f), 162.9724f, LocationType.LiquorStore, "Vinewood Liquor", "Vinewood Liquor") { SellableItems = LiquorStoreMenu, OpenTime = 4, CloseTime = 22 },
            new GameLocation(new Vector3(1391.861f, 3606.275f, 34.98093f), 199.2899f,new Vector3(1391.861f, 3606.275f, 34.98093f), 199.2899f, LocationType.LiquorStore, "Liquor Ace", "Liquor Ace") { SellableItems = LiquorStoreMenu, OpenTime = 4, CloseTime = 22 },
            new GameLocation(new Vector3(1952.552f, 3840.833f, 32.17612f), 298.8575f, LocationType.LiquorStore, "Sandy Shores Liquor", "Sandy Shores Liquor") { SellableItems = LiquorStoreMenu, OpenTime = 4, CloseTime = 22 },            
            new GameLocation(new Vector3(2455.443f, 4058.518f, 38.06472f), 250.6311f, LocationType.LiquorStore, "Liquor Market 24/7", "Liquor Market"){ SellableItems = LiquorStoreMenu, OpenTime = 0, CloseTime = 24 },
            new GameLocation(new Vector3(2481.348f, 4100.31f, 38.13171f), 249.6295f, LocationType.LiquorStore, "Liquor Store", "Liquor Store"){ SellableItems = LiquorStoreMenu, OpenTime = 4, CloseTime = 22 },
            new GameLocation(new Vector3(2566.804f, 4274.108f, 41.98908f), 239.0765f, LocationType.LiquorStore, "Grape Smuggler's Liquor", "Grape Liquor"){ SellableItems = LiquorStoreMenu, OpenTime = 4, CloseTime = 22 },

            //Head
            new GameLocation(new Vector3(-1191.582f, -1197.779f, 7.617113f), 146.801f, LocationType.Headshop, "Pipe Dreams", "Pipe Dreams") {SellableItems = HeadShopMenu },
            new GameLocation(new Vector3(-1161.365f, -1427.646f, 4.623186f), 31.50553f, LocationType.Headshop, "Doctor Kush", "Doctor kush"){SellableItems = HeadShopMenu },
            new GameLocation(new Vector3(-1154.942f, -1373.176f, 5.061489f), 305.589f, LocationType.Headshop, "Amnesiac Smoke Shop", "Amnesiac Smoke Shop"){SellableItems = HeadShopMenu },
            new GameLocation(new Vector3(-502.4879f, 32.92564f, 44.71512f), 179.9803f, LocationType.Headshop, "Serenity Wellness", "Serenity Wellness"){SellableItems = HeadShopMenu },

            //Convenience
            new GameLocation(new Vector3(547f, 2678f, 41f), 22.23846f, LocationType.ConvenienceStore, "24/7","As fast as you"){ SellableItems = StoreMenu, OpenTime = 0, CloseTime = 24 },
            new GameLocation(new Vector3(-3236.767f,1005.609f,12.33137f), 122.6316f, LocationType.ConvenienceStore, "24/7","As fast as you"){ SellableItems = StoreMenu, OpenTime = 0, CloseTime = 24 },
            new GameLocation(new Vector3(-578.0112f, -1012.898f, 22.32503f), 359.4114f, LocationType.ConvenienceStore, "24/7", "As fast as you"){ SellableItems = StoreMenu, OpenTime = 0, CloseTime = 24 },
            new GameLocation(new Vector3(-696.9965f, -858.7673f, 23.69209f), 85.51252f, LocationType.ConvenienceStore, "24/7", "24/7"){ SellableItems = StoreMenu, OpenTime = 0, CloseTime = 24 },
            new GameLocation(new Vector3(-551.5444f, -855.8014f, 28.28332f), 2.39254f, LocationType.ConvenienceStore, "Save-A-Cent", "Save-A-Cent"){ SellableItems = StoreMenu, OpenTime = 0, CloseTime = 24 },
            new GameLocation(new Vector3(-1412.015f, -320.1292f, 44.37897f), 92.48502f, LocationType.ConvenienceStore, "The Grain Of Truth", "The Grain Of Truth") { SellableItems = StoreMenu },
            new GameLocation(new Vector3(-1264.064f, -1162.599f, 6.764161f), 161.218f, LocationType.ConvenienceStore, "Fruit Of The Vine", "Fruit Of The Vine"){ SellableItems = StoreMenu },
            new GameLocation(new Vector3(-1270.649f, -304.9037f, 37.06938f), 257.2106f, LocationType.ConvenienceStore, "Fruit Of The Vine", "Fruit Of The Vine") { SellableItems = StoreMenu },
            new GameLocation(new Vector3(164.9962f, 351.1263f, 109.6859f), 4.847032f, LocationType.ConvenienceStore, "Fruit Of The Vine", "Fruit Of The Vine") { SellableItems = StoreMenu },
            new GameLocation(new Vector3(-1312.64f, -1181.899f, 4.890057f), 271.5434f, LocationType.ConvenienceStore, "Beach Buddie", "Beach Buddie"){ SellableItems = StoreMenu },
            new GameLocation(new Vector3(-661.5522f, -915.5651f, 24.61216f), 260.1033f, LocationType.ConvenienceStore, "Convenience Store", "Convenience Store"){ SellableItems = StoreMenu },
            new GameLocation(new Vector3(372.6485f, 327.0293f, 103.5664f), 257.6475f,new Vector3(372.6485f, 327.0293f, 103.5664f), 257.6475f, LocationType.ConvenienceStore, "24/7", "As fast as you"){ SellableItems = StoreMenu, OpenTime = 0, CloseTime = 24 },
            new GameLocation(new Vector3(152.5101f, 237.4131f, 106.9718f), 165.2823f, LocationType.ConvenienceStore, "24/7", "As fast as you"){ SellableItems = StoreMenu, OpenTime = 0, CloseTime = 24 },
            new GameLocation(new Vector3(201.8985f, -26.30606f, 69.90953f), 249.8224f, LocationType.ConvenienceStore, "24/7", "As fast as you"){ SellableItems = StoreMenu, OpenTime = 0, CloseTime = 24 },
            new GameLocation(new Vector3(528.017f, -152.1372f, 57.20173f), 44.64286f, LocationType.ConvenienceStore, "24/7", "As fast as you"){ SellableItems = StoreMenu, OpenTime = 0, CloseTime = 24 },
            new GameLocation(new Vector3(2560f, 385f, 107f), 22.23846f,new Vector3(2555.339f, 380.9034f, 108.6229f), 347.3629f, LocationType.ConvenienceStore, "24/7","As fast as you"){ SellableItems = StoreMenu, OpenTime = 0, CloseTime = 24 },
            new GameLocation(new Vector3(1707.748f, 4792.387f, 41.98377f), 90.42564f, LocationType.ConvenienceStore, "Supermarket", "Supermarket"){ SellableItems = StoreMenu },
            

            //Gas
            new GameLocation(new Vector3(2676.595f, 3280.101f, 55.24113f), 325.0921f,new Vector3(2676.595f, 3280.101f, 55.24113f), 325.0921f, LocationType.GasStation, "24/7 ", "As fast as you") { SellableItems = StoreMenu,OpenTime = 0, CloseTime = 24 },
            new GameLocation(new Vector3(1725f, 6410f, 35f), 22.23846f, LocationType.GasStation, "24/7 Mount Chilliad (Gas)","As fast as you") { SellableItems = StoreMenu, OpenTime = 0, CloseTime = 24 },
            new GameLocation(new Vector3(-1429.33f,-270.8909f,46.2077f), 325.7301f, LocationType.GasStation, "Ron Morningwood","Good Morningwood!") { SellableItems = StoreMenu },
            new GameLocation(new Vector3(160.4977f,6635.249f,31.61175f), 70.88637f, LocationType.GasStation, "Dons Country Store & Gas","Country Manners!") { SellableItems = StoreMenu },
            new GameLocation(new Vector3(266.2746f,2599.669f,44.7383f), 231.9223f, LocationType.GasStation, "Harmony General Store & Gas","Always in Harmony!") { SellableItems = StoreMenu },
            new GameLocation(new Vector3(1039.753f,2666.26f,39.55253f), 143.6208f, LocationType.GasStation, "Grande Senora Cafe & Gas","Extra Grande!") { SellableItems = StoreMenu },
            new GameLocation(new Vector3(-1817.871f,787.0063f,137.917f), 89.38248f, LocationType.GasStation, "LTD Richmond Glen","") { SellableItems = StoreMenu },
            new GameLocation(new Vector3(2001.239f, 3779.786f, 32.18078f), 208.5214f, LocationType.GasStation, "Sandy's Gas", "And Full Service!"){ SellableItems = StoreMenu, OpenTime = 4, CloseTime = 22 },
            new GameLocation(new Vector3(-1427.998f, -268.4702f, 46.2217f), 132.4002f, LocationType.GasStation, "Ron Gas", "Ron Gas"){ SellableItems = StoreMenu, OpenTime = 4, CloseTime = 22 },
            new GameLocation(new Vector3(-705.7453f, -913.6598f, 19.21559f), 83.75771f,new Vector3(-705.7453f, -913.6598f, 19.21559f), 83.75771f, LocationType.GasStation, "LtD Gasoline", "A one-stop shop!"){ SellableItems = StoreMenu,OpenTime = 0, CloseTime = 24 },
            new GameLocation(new Vector3(-531.5529f, -1220.763f, 18.455f), 347.6858f, LocationType.GasStation, "Xero Gas", "Xero Gas"){ SellableItems = StoreMenu,OpenTime = 0, CloseTime = 24 },
            new GameLocation(new Vector3(2559.112f, 373.5359f, 108.6211f), 265.8011f, LocationType.GasStation, "Ron", "Ron") { SellableItems = StoreMenu },
            new GameLocation(new Vector3(646.0997f, 267.417f, 103.2494f), 58.99448f, LocationType.GasStation, "Globe Oil", "Globe Oil") { SellableItems = StoreMenu },
            new GameLocation(new Vector3(1776.308f, 3327.483f, 41.43329f), 328.0875f, LocationType.GasStation, "Flywheels Gas", "Flywheels Gas") { SellableItems = StoreMenu },
            new GameLocation(new Vector3(1698.044f, 4922.526f, 42.06367f), 314.3236f,new Vector3(1698.044f, 4922.526f, 42.06367f), 314.3236f, LocationType.ConvenienceStore, "LtD", "LtD") { SellableItems = StoreMenu },

            //Food Stand
            new GameLocation(new Vector3(-1516.382f, -952.5892f, 9.278718f), 317.7292f,new Vector3(-1516.382f, -952.5892f, 9.278718f), 317.7292f, LocationType.FoodStand, "Del Perro Beach Hot Dog","Mostly Meat!") { SellableItems = HotDogMenu },
            new GameLocation(new Vector3(-1539.045f, -900.472f, 10.16951f), 129.0318f,new Vector3(-1539.045f, -900.472f, 10.16951f), 129.0318f, LocationType.FoodStand, "Del Perro Food Market","No Robberies Please!"){ SellableItems = StoreMenu },
            new GameLocation(new Vector3(-1334.007f, -1282.623f, 4.835985f), 115.3464f,new Vector3(-1334.007f, -1282.623f, 4.835985f), 115.3464f, LocationType.FoodStand, "Slice N Dice Pizza","Slice UP!"){ SellableItems = PizzaMenu},
            new GameLocation(new Vector3(-1296.815f, -1387.3f, 4.544102f), 112.4694f,new Vector3(-1296.815f, -1387.3f, 4.544102f), 112.4694f, LocationType.FoodStand, "Sharkies Bites","Take A Bite Today!"){ SellableItems = DonutMenu },
            new GameLocation(new Vector3(-1268.011f, -1432.715f, 4.353373f), 134.2259f,new Vector3(-1268.011f, -1432.715f, 4.353373f), 134.2259f, LocationType.FoodStand, "Beefy Bills Burger Bar","Extra BEEFY!"){ SellableItems = BurgerMenu },
            new GameLocation(new Vector3(-1248.932f, -1474.449f, 4.277946f), 306.3787f,new Vector3(-1248.932f, -1474.449f, 4.277946f), 306.3787f, LocationType.FoodStand, "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes."){ SellableItems = HotDogMenu },
            new GameLocation(new Vector3(-1232.426f, -1485.006f, 4.362638f), 137.5475f,new Vector3(-1232.426f, -1485.006f, 4.362638f), 137.5475f, LocationType.FoodStand, "Beefy Bills Burger Bar", "Extra BEEFY!"){ SellableItems = BurgerMenu },
            new GameLocation(new Vector3(-1219.656f, -1504.36f, 4.36032f), 98.7149f,new Vector3(-1219.656f, -1504.36f, 4.36032f), 98.7149f, LocationType.FoodStand, "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes."){ SellableItems = HotDogMenu },
            new GameLocation(new Vector3(821.2138f, -2977.05f, 6.02066f), 272.7679f,new Vector3(821.2138f, -2977.05f, 6.02066f), 272.7679f, LocationType.FoodStand, "Beefy Bills Burger Bar", "Extra BEEFY!"){ SellableItems = BurgerMenu },
            new GameLocation(new Vector3(821.8197f, -2973.398f, 6.020657f), 276.5136f,new Vector3(821.8197f, -2973.398f, 6.020657f), 276.5136f, LocationType.FoodStand, "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes."){ SellableItems = HotDogMenu },
            new GameLocation(new Vector3(403.3527f, 106.0655f, 101.4575f), 241.199f,new Vector3(403.3527f, 106.0655f, 101.4575f), 241.199f, LocationType.FoodStand, "Beefy Bills Burger Bar", "Extra BEEFY!"){ SellableItems = BurgerMenu },
            new GameLocation(new Vector3(245.8918f, 161.5893f, 104.9487f), 3.803493f,new Vector3(245.8918f, 161.5893f, 104.9487f), 3.803493f, LocationType.FoodStand, "Beefy Bills Burger Bar", "Extra BEEFY!"){ SellableItems = BurgerMenu },
            new GameLocation(new Vector3(240.8329f, 167.2296f, 105.0605f), 167.5996f,new Vector3(240.8329f, 167.2296f, 105.0605f), 167.5996f, LocationType.FoodStand, "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes."){ SellableItems = HotDogMenu },
            new GameLocation(new Vector3(1604.578f, 3828.483f, 34.4987f), 142.3778f,new Vector3(1604.578f, 3828.483f, 34.4987f), 142.3778f, LocationType.FoodStand, "Tough Nut Donut", "Our DoNuts are Crazy!"){ SellableItems = DonutMenu },
            new GameLocation(new Vector3(1604.818f, 3822.332f, 34.69806f), 200.7076f,new Vector3(1607.818f, 3822.332f, 34.69806f), 200.7076f, LocationType.FoodStand, "Chihuahua Hot Dog", "Vegan? No. Meat? Yes."){ SellableItems = HotDogMenu },
            new GameLocation(new Vector3(1087.509f, 6510.788f, 21.0551f), 185.487f,new Vector3(1087.509f, 6510.788f, 21.0551f), 185.487f, LocationType.FoodStand, "Roadside Fruit", "Should Be OK To Eat") { SellableItems = FruitMenu },
            new GameLocation(new Vector3(2106.954f, 4947.911f, 40.95187f), 319.9109f,new Vector3(2106.954f, 4947.911f, 40.95187f), 319.9109f, LocationType.FoodStand, "Attack-A-Taco", "Heavy Shelling!") { SellableItems = DonutMenu },
            new GameLocation(new Vector3(-1148.969f, -1601.963f, 4.390241f), 35.73399f,new Vector3(-1145.969f, -1602.963f, 4.390241f), 35.73399f, LocationType.FoodStand, "Gyro Day", "Gyro Day") { SellableItems = DonutMenu },
            new GameLocation(new Vector3(2526.548f, 2037.936f, 19.82413f), 263.8982f,new Vector3(2526.548f, 2037.936f, 19.82413f), 263.8982f, LocationType.FoodStand, "Roadside Fruit", "Should Be OK To Eat") { SellableItems = FruitMenu },
            new GameLocation(new Vector3(1263.013f, 3548.566f, 35.14751f), 187.8834f,new Vector3(1263.013f, 3548.566f, 35.14751f), 187.8834f, LocationType.FoodStand, "Roadside Fruit", "Should Be OK To Eat") { SellableItems = FruitMenu },
            new GameLocation(new Vector3(1675.873f, 4883.532f, 42.06379f), 57.34329f,new Vector3(1675.873f, 4883.532f, 42.06379f), 57.34329f, LocationType.FoodStand, "Grapeseed Fruit", "Grapeseed Fruit") { SellableItems = FruitMenu },



            //Bar
            new GameLocation(new Vector3(224.5178f, 336.3819f, 105.5973f), 340.0694f, LocationType.Bar, "Pitchers", "Pitchers") { SellableItems = BarMenu },
            new GameLocation(new Vector3(219.5508f, 304.9488f, 105.5861f), 250.1051f, LocationType.Bar, "Singletons", "Singletons") { SellableItems = BarMenu },
            new GameLocation(new Vector3(1982.979f, 3053.596f, 47.21508f), 226.3188f,new Vector3(1982.979f, 3053.596f, 47.21508f), 226.3188f, LocationType.Bar, "Yellow Jacket Inn", "Yellow Jacket Inn") { SellableItems = BarMenu },




            //Restaurant

                //Fancy
                new GameLocation(new Vector3(-1487.163f, -308.0127f, 47.02639f), 231.5184f, LocationType.Restaurant, "Las Cuadras Restaurant", "Las Cuadras Restaurant") {SellableItems = TacoFarmerMenu },
                new GameLocation(new Vector3(-1473.121f, -329.6028f, 44.81668f), 319.3725f, LocationType.Restaurant, "Las Cuadras Deli", "Las Cuadras Deli") {SellableItems = TacoFarmerMenu },
                new GameLocation(new Vector3(-1221.254f, -1095.873f, 8.115647f), 111.3174f, LocationType.Restaurant, "Prawn Vivant", "Prawn Vivant") {SellableItems = GenericMenu },
                new GameLocation(new Vector3(-1256.581f, -1079.491f, 8.398257f), 339.5968f, LocationType.Restaurant, "Surfries Diner", "Surfries Diner") {SellableItems = GenericMenu },
                new GameLocation(new Vector3(-1247.504f, -1105.777f, 8.109305f), 289.9685f, LocationType.Restaurant, "The Split Kipper Fish", "The Split Kipper Fish") {SellableItems = GenericMenu },
                new GameLocation(new Vector3(-1230.032f, -1174.862f, 7.700727f), 330.9398f, LocationType.Restaurant, "Pot Heads Seafood", "Pot Heads Seafood") {SellableItems = GenericMenu },
                new GameLocation(new Vector3(-1111.103f, -1454.387f, 5.582287f), 304.9954f, LocationType.Restaurant, "Coconut Cafe", "Coconut Cafe") {SellableItems = GenericMenu },
                new GameLocation(new Vector3(-1037.587f, -1397.168f, 5.553192f), 76.84702f, LocationType.Restaurant, "La Spada", "La Spada") {SellableItems = GenericMenu },
                new GameLocation(new Vector3(-1129.253f, -1373.276f, 5.056143f), 164.9213f, LocationType.Restaurant, "Marlin's Cafe", "Marlin's Cafe") {SellableItems = GenericMenu },
                new GameLocation(new Vector3(2561.869f, 2590.851f, 38.08311f), 294.6638f, LocationType.Restaurant, "Rex's Diner", "Rex's Diner") {SellableItems = GenericMenu },
                new GameLocation(new Vector3(2697.521f, 4324.264f, 45.98642f), 41.67124f, LocationType.Restaurant, "Park View Diner", "Park View Diner") {SellableItems = GenericMenu },



                //Sandwiches
                new GameLocation(new Vector3(-1249.812f, -296.1564f, 37.35062f), 206.9039f, LocationType.Restaurant, "Bite!", "Have It Our Way") {SellableItems = GenericMenu,OpenTime = 0, CloseTime = 24 },
                new GameLocation(new Vector3(-1539.498f, -427.3804f, 35.59194f), 233.1319f, LocationType.Restaurant, "Bite!", "Have It Our Way") {SellableItems = GenericMenu,OpenTime = 0, CloseTime = 24 },
                new GameLocation(new Vector3(229.5384f, -22.3363f, 74.98735f), 160.0777f, LocationType.Restaurant, "Bite!", "Have It Our Way") {SellableItems = GenericMenu,OpenTime = 0, CloseTime = 24 },
                new GameLocation(new Vector3(100.5837f, 209.4958f, 107.9911f), 342.4262f, LocationType.Restaurant, "The Pink Sandwich", "The Pink Sandwich") {SellableItems = GenericMenu,OpenTime = 0, CloseTime = 24 },

                //Asian
                new GameLocation(new Vector3(-798.0056f, -632.0029f, 29.02696f), 169.2606f, LocationType.Restaurant, "S.HO Noodles", "S.HO Noodles") {SellableItems = GenericMenu },
                new GameLocation(new Vector3(-638.5052f, -1249.646f, 11.81044f), 176.4081f, LocationType.Restaurant, "S.HO Noodles", "S.HO Noodles") {SellableItems = GenericMenu },
                new GameLocation(new Vector3(-700.9553f, -884.5563f, 23.79126f), 41.62328f, LocationType.Restaurant, "S.HO Noodles", "S.HO Noodles") {SellableItems = GenericMenu },
                new GameLocation(new Vector3(-1229.61f, -285.7077f, 37.73843f), 205.5755f, LocationType.Restaurant, "Noodle Exchange", "You Won't Want To Share!") {SellableItems = GenericMenu },
                new GameLocation(new Vector3(-1199.53f, -1162.439f, 7.696731f), 107.0593f, LocationType.Restaurant, "Noodle Exchange", "You Won't Want To Share!") {SellableItems = GenericMenu },
                new GameLocation(new Vector3(-655.6034f, -880.3672f, 24.67554f), 265.7094f, LocationType.Restaurant, "Wook Noodle House", "Wook Noodle House") {SellableItems = GenericMenu },
                new GameLocation(new Vector3(-680.4404f, -945.5441f, 20.93157f), 180.6927f, LocationType.Restaurant, "Wook Noodle House", "Wook Noodle House") {SellableItems = GenericMenu },
                new GameLocation(new Vector3(-654.8373f, -885.7593f, 24.67703f), 273.4168f, LocationType.Restaurant, "Park Jung Restaurant", "Park Jung Restaurant") {SellableItems = GenericMenu },
                new GameLocation(new Vector3(-661.5396f, -907.5895f, 24.60632f), 278.5222f, LocationType.Restaurant, "Hwan Cafe", "Hwan Cafe") {SellableItems = GenericMenu },

                //Italian
                new GameLocation(new Vector3(-1182.659f, -1410.577f, 4.499721f), 215.9843f, LocationType.Restaurant, "Al Dentes", "Al Dentes") {SellableItems = PizzaMenu },
                new GameLocation(new Vector3(-213.0357f, -40.15178f, 50.04371f), 157.8173f, LocationType.Restaurant, "Al Dentes", "Al Dentes") {SellableItems = PizzaMenu },
                new GameLocation(new Vector3(215.2669f, -17.14256f, 74.98737f), 159.7144f, LocationType.Restaurant, "Pizza This...", "Pizza This...") {SellableItems = PizzaMenu },
                new GameLocation(new Vector3(538.3118f, 101.4798f, 96.52515f), 159.4801f, LocationType.Restaurant, "Pizza This...", "Pizza This...") {SellableItems = PizzaMenu },
                new GameLocation(new Vector3(443.7377f, 135.1464f, 100.0275f), 161.2897f, LocationType.Restaurant, "Guidos Takeout 24/7", "Guidos Takeout 24/7") {SellableItems = PizzaMenu },

                //Burger
                new GameLocation(new Vector3(-1535.117f, -454.0615f, 35.92439f), 319.1095f, LocationType.Restaurant, "Wigwam", "Wigwam") {SellableItems = GenericMenu },
                new GameLocation(new Vector3(-860.8414f, -1140.393f, 7.39234f), 171.7175f, LocationType.Restaurant, "Wigwam", "Wigwam") {SellableItems = GenericMenu },
                new GameLocation(new Vector3(-1540.86f, -454.866f, 40.51906f), 321.1314f, LocationType.Restaurant, "Up-N-Atom", "Never Frozen, Often Microwaved") {SellableItems = UpNAtomMenu,OpenTime = 0, CloseTime = 24},
                new GameLocation(new Vector3(81.31124f, 275.1125f, 110.2102f), 162.7602f, LocationType.Restaurant, "Up-N-Atom", "Never Frozen, Often Microwaved") {SellableItems = UpNAtomMenu,OpenTime = 0, CloseTime = 24},
                new GameLocation(new Vector3(-512.6821f, -683.3517f, 33.18555f), 3.720508f, LocationType.Restaurant, "Snr. Buns", "Snr. Buns") {SellableItems = GenericMenu },
                new GameLocation(new Vector3(-526.9481f, -679.6907f, 33.67113f), 35.17997f, LocationType.Restaurant, "Snr. Muffin", "Snr. Muffin") {SellableItems = GenericMenu },//???

                //Bagels&Donuts
                new GameLocation(new Vector3(-1318.507f, -282.2458f, 39.98732f), 115.4663f, LocationType.Restaurant, "Dickies Bagels", "Holy Dick!") { SellableItems = CoffeeMenu },
                new GameLocation(new Vector3(-1204.364f, -1146.246f, 7.699615f), 109.2444f, LocationType.Restaurant, "Dickies Bagels", "Holy Dick!") { SellableItems = CoffeeMenu },


                //Coffee
                new GameLocation(new Vector3(-1283.567f, -1130.118f, 6.795891f), 143.1178f, LocationType.Restaurant, "The Bean Machine Coffee", "The Bean Machine Coffee") { SellableItems = CoffeeMenu },
                new GameLocation(new Vector3(-1549.39f, -435.5105f, 35.88667f), 234.6563f, LocationType.Restaurant, "The Bean Machine Coffee", "The Bean Machine Coffee") { SellableItems = CoffeeMenu },
                new GameLocation(new Vector3(-835.4522f, -610.4766f, 29.02697f), 142.0655f, LocationType.Restaurant, "The Bean Machine Coffee", "The Bean Machine Coffee") { SellableItems = CoffeeMenu },
                new GameLocation(new Vector3(-602.2112f, -1105.766f, 22.32427f), 273.8795f, LocationType.Restaurant, "The Bean Machine Coffee", "The Bean Machine Coffee") { SellableItems = CoffeeMenu },
                new GameLocation(new Vector3(-659.5289f, -814.0433f, 24.53778f), 232.0023f, LocationType.Restaurant, "The Bean Machine Coffee", "The Bean Machine Coffee") { SellableItems = CoffeeMenu },
                new GameLocation(new Vector3(-687.0801f, -855.6792f, 23.89398f), 0.2374549f, LocationType.Restaurant, "The Bean Machine Coffee", "The Bean Machine Coffee") { SellableItems = CoffeeMenu },

                new GameLocation(new Vector3(-1108.847f, -1355.264f, 5.035112f), 206.1676f, LocationType.Restaurant, "Crucial Fix Coffee", "Crucial Fix Coffee") { SellableItems = CoffeeMenu },
                new GameLocation(new Vector3(-576.6631f, -677.8674f, 32.36259f), 306.9058f, LocationType.Restaurant, "Hit-N-Run Coffee", "Hit-N-Run Coffee"){ SellableItems = CoffeeMenu },
                new GameLocation(new Vector3(-1253.337f, -296.6488f, 37.31522f), 206.5786f, LocationType.Restaurant, "{java.update();}", "Enjoy Hot Coffee") { SellableItems = CoffeeMenu },
                new GameLocation(new Vector3(-1206.975f, -1135.029f, 7.693257f), 109.1408f, LocationType.Restaurant, "Cool Beans Coffee", "Cool Beans Coffee") { SellableItems = CoffeeMenu },
                new GameLocation(new Vector3(-509.1889f, -22.9895f, 45.60899f), 354.7263f, LocationType.Restaurant, "Little Teapot", "Little Teapot") { SellableItems = CoffeeMenu },

                //Mexican
                new GameLocation(new Vector3(-1553.112f, -439.9938f, 40.51905f), 228.7506f, LocationType.Restaurant, "Taco Bomb", "Taco Bomb") {SellableItems = TacoFarmerMenu },
                new GameLocation(new Vector3(10.96682f, -1605.874f, 29.3931f), 229.8729f, LocationType.Restaurant, "The Taco Farmer", "Open All Hours!") {SellableItems = TacoFarmerMenu },
                new GameLocation(new Vector3(-1168.281f, -1267.279f, 6.198249f), 111.9682f, LocationType.Restaurant, "Taco Libre", "Taco Libre") {SellableItems = TacoFarmerMenu },
                new GameLocation(new Vector3(-657.5089f, -679.4656f, 31.46727f), 317.9819f, LocationType.Restaurant, "Taco Bomb", "Taco Bomb") {SellableItems = TacoFarmerMenu },

                //Ice Cream
                new GameLocation(new Vector3(-1193.966f, -1543.693f, 4.373522f), 124.3727f, LocationType.Restaurant, "The Sundae Post", "The Sundae Post") {SellableItems = DonutMenu },
                new GameLocation(new Vector3(-1171.529f, -1435.118f, 4.461945f), 32.60835f, LocationType.Restaurant, "Ice Maiden", "Ice Maiden") {SellableItems = GenericMenu },

                //Juice and Smoothies
                new GameLocation(new Vector3(-1137.926f, -1624.695f, 4.410712f), 127.6497f, LocationType.Restaurant, "Vitamin Seaside Juice Bar", "Vitamin Seaside Juice Bar"){ SellableItems = FruitMenu },
                new GameLocation(new Vector3(-1187.057f, -1536.73f, 4.379496f), 32.85152f, LocationType.Restaurant, "Muscle Peach Juice Bar", "Muscle Peach Juice Bar"){ SellableItems = FruitMenu },
                new GameLocation(new Vector3(-1236.263f, -287.9641f, 37.63038f), 205.8273f, LocationType.Restaurant, "Limey's Juice and Smoothies", "No Limes About It") {SellableItems = GenericMenu },
                new GameLocation(new Vector3(-1182.576f, -1248.037f, 6.991587f), 110.7639f, LocationType.Restaurant, "Limey's Juice Bar", "No Limes About It") {SellableItems = GenericMenu },
                new GameLocation(new Vector3(-471.7646f, -18.46761f, 45.75837f), 357.2652f, LocationType.Restaurant, "Fruit Machine", "Fruit Machine"){SellableItems = GenericMenu },
                new GameLocation(new Vector3(2741.563f, 4413.093f, 48.62326f), 201.5914f, LocationType.Restaurant, "Big Juice Stand", "Big Juice Stand"){SellableItems = GenericMenu },
                new GameLocation(new Vector3(1791.592f, 4594.844f, 37.68291f), 182.8134f, LocationType.Restaurant, "Alamo Fruit", "Alamo Fruit"){ SellableItems = FruitMenu },

                //Chicken
                new GameLocation(new Vector3(-584.761f, -872.753f, 25.91489f), 353.0746f, LocationType.Restaurant, "Lucky Plucker", "Lucky Plucker") {SellableItems = GenericMenu },
                new GameLocation(new Vector3(2580.543f, 464.6521f, 108.6232f), 176.5548f, LocationType.Restaurant, "Bishop's Chicken", "Bishop's Chicken") {SellableItems = GenericMenu },

                //General      
                new GameLocation(new Vector3(-1196.705f, -1167.969f, 7.695099f), 108.4535f, LocationType.Restaurant, "Lettuce Be", "Lettuce Be") {SellableItems = GenericMenu },
                new GameLocation(new Vector3(-1535.082f, -422.2449f, 35.59194f), 229.4618f, LocationType.Restaurant, "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes."){ SellableItems = HotDogMenu },
                new GameLocation(new Vector3(-1271.224f, -1200.703f, 5.366248f), 70.19876f, LocationType.Restaurant, "The Nut Buster", "The Nut Buster") {SellableItems = GenericMenu },
            


            //Drive Thru
            new GameLocation(new Vector3(95.41846f, 285.0295f, 110.2042f), 251.8247f, LocationType.DriveThru, "Up-N-Atom Burger", "Never Frozen, Often Microwaved") {SellableItems = UpNAtomMenu,OpenTime = 0, CloseTime = 24},
            new GameLocation(new Vector3(15.48935f, -1595.832f, 29.28254f), 319.2816f, LocationType.DriveThru, "The Taco Farmer", "Open All Hours!") {SellableItems = TacoFarmerMenu },
            new GameLocation(new Vector3(-576.9321f, -880.5195f, 25.70123f), 86.01214f, LocationType.DriveThru, "Lucky Plucker", "Lucky Plucker") {SellableItems = GenericMenu },
            new GameLocation(new Vector3(2591.213f, 478.8892f, 108.6423f), 270.9569f,LocationType.DriveThru, "Bishop's Chicken", "Bishop's Chicken") {SellableItems = GenericMenu },

            //Bank
            new GameLocation(new Vector3(-813.9924f, -1114.698f, 11.18181f), 297.7995f, LocationType.Bank, "Fleeca Bank", "Fleeca Bank"),
            new GameLocation(new Vector3(-350.1604f, -45.84864f, 49.03682f), 337.4063f, LocationType.Bank, "Fleeca Bank", "Fleeca Bank"),

            //Hotel
            new GameLocation(new Vector3(-1183.073f, -1556.673f, 5.036984f), 122.3785f, LocationType.Hotel, "Vespucci Hotel", "Vespucci Hotel"),

            //Pharmacy
            new GameLocation(new Vector3(114.2954f, -4.942202f, 67.82149f), 195.4308f, LocationType.Pharmacy, "Pop's Pills", "Pop's Pills"),

            //Hardware
            new GameLocation(new Vector3(2747.406f, 3473.213f, 55.67021f), 249.8152f, LocationType.HardwareStore, "You Tool", "You Tool"),

        };






        /*            new GameLocation(new Vector3(2683.969f,3282.098f,55.24052f), 89.53969f, LocationType.GasStation, "24/7 Supermarket Grande Senora" ,"As fast as you"),//,new List<Vector3>() { new Vector3(2678.073f, 3265.522f, 54.7076f),new Vector3(2681.173f, 3262.774f, 54.70736f) }),
            new GameLocation(new Vector3(1725f, 6410f, 35f), 22.23846f, LocationType.GasStation, "24/7 Mount Chilliad (Gas)","As fast as you"),//,new List<Vector3>() { new Vector3(1706.173f, 6412.223f, 32.22713f), new Vector3(1701.657f, 6414.528f, 32.1186f), new Vector3(1697.71f, 6416.565f, 32.08189f),new Vector3(1706.173f, 6412.223f, 32.22713f)
                                                                                                                                                //, new Vector3(1697.869f,6420.53f,32.05283f), new Vector3(1701.852f,6418.417f,32.05503f), new Vector3(1705.852f,6416.659f,32.05479f) }),
            new GameLocation(new Vector3(-1429.33f,-270.8909f,46.2077f), 325.7301f, LocationType.GasStation, "Ron Morningwood","Good Morningwood!"),//,new List<Vector3>() { new Vector3(-1428.23f,-277.0434f,45.79089f), new Vector3(-1436.362f,-267.6647f,45.79237f),
                                                                                                                                                         // new Vector3(-1440.16f,-270.164f,45.79181f),new Vector3(-1431.339f,-280.2279f,45.79009f),
                                                                                                                                                         // new Vector3(-1434.153f,-282.5898f,45.79139f), new Vector3(-1442.416f,-273.0687f,45.7986f),
                                                                                                                                                         // new Vector3(-1446.231f,-276.2419f,45.80196f),new Vector3(-1437.798f,-285.5625f,45.77643f) }),
            new GameLocation(new Vector3(160.4977f,6635.249f,31.61175f), 70.88637f, LocationType.GasStation, "Dons Country Store & Gas","Country Manners!"),//,new List<Vector3>() { new Vector3(188.7231f,6607.43f,31.84954f), new Vector3(184.8491f,6606.112f,31.85245f), new Vector3(181.3225f,6605.81f,31.84829f)
                                                                                                                                               // , new Vector3(178.1896f,6604.389f,31.89782f), new Vector3(174.0998f,6604.168f,31.84834f), new Vector3(171.1875f,6603.454f,32.04737f) }),
            new GameLocation(new Vector3(266.2746f,2599.669f,44.7383f), 231.9223f, LocationType.GasStation, "Harmony General Store & Gas","Always in Harmony!"), //,new List<Vector3>() { new Vector3(262.5423f, 2610.143f, 44.3814f),new Vector3(265.0521f, 2604.807f, 44.38421f) }),

            new GameLocation(new Vector3(1039.753f,2666.26f,39.55253f), 143.6208f, LocationType.GasStation, "Grande Senora Cafe & Gas","Extra Grande!"),//,new List<Vector3>() { new Vector3(1043.293f,2677.189f,38.90083f), new Vector3(1035.182f,2677.444f,38.90271f), new Vector3(1034.835f,2670.396f,38.9343f)
                                                                                                                                                //, new Vector3(1043.761f,2670.147f,38.94082f), new Vector3(1043.412f,2672.185f,38.95105f), new Vector3(1034.338f,2672.41f,38.94936f) }),


            new GameLocation(new Vector3(-1817.871f,787.0063f,137.917f), 89.38248f, LocationType.GasStation, "LTD Richmond Glen",""),//,new List<Vector3>() { new Vector3(-1804.758f,792.6874f,138.5142f), new Vector3(-1809.721f,798.1087f,138.5137f),
                                                                                                                                                        //  new Vector3(-1807.576f,801.251f,138.5144f),new Vector3(-1802.39f,796.0137f,138.5141f),
                                                                                                                                                       //   new Vector3(-1798.391f,798.9479f,138.5154f), new Vector3(-1802.964f,805.196f,138.5681f),
                                                                                                                                                       //   new Vector3(-1800.726f,807.3672f,138.515f),new Vector3(-1796.539f,803.0259f,138.5148f),
                                                                                                                                                        //  new Vector3(-1792.259f,804.6542f,138.5133f),new Vector3(-1796.816f,810.0197f,138.5144f),
                                                                                                                                                       //   new Vector3(-1794.411f,813.2302f,138.5146f),new Vector3(-1789.586f,808.2133f,138.5163f)}),*/
    }
}

