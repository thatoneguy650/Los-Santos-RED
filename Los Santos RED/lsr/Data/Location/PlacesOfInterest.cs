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
    private IShopMenus ShopMenus;
    private IGangs Gangs;
    public PossibleLocations PossibleLocations { get; private set; }
    public PlacesOfInterest(IShopMenus shopMenus, IGangs gangs)
    {
        ShopMenus = shopMenus;
        Gangs = gangs;
        PossibleLocations = new PossibleLocations();
    }
    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("Locations*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Locations config: {ConfigFile.FullName}", 0);
            PossibleLocations = Serialization.DeserializeParam<PossibleLocations>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Locations config  {ConfigFileName}", 0);
            PossibleLocations = Serialization.DeserializeParam<PossibleLocations>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Locations config found, creating default", 0);
            DefaultConfig_LibertyCity();
            DefaultConfig();
        }
    }
    public List<InteractableLocation> InteractableLocations()
    {
        List<InteractableLocation> AllLocations = new List<InteractableLocation>();



        AllLocations.AddRange(PossibleLocations.PoliceStations);
        AllLocations.AddRange(PossibleLocations.Hospitals);
        AllLocations.AddRange(PossibleLocations.FireStations);
        AllLocations.AddRange(PossibleLocations.Banks);
        AllLocations.AddRange(PossibleLocations.BeautyShops);
        AllLocations.AddRange(PossibleLocations.Landmarks);
        AllLocations.AddRange(PossibleLocations.Prisons);
        AllLocations.AddRange(PossibleLocations.SubwayStations);

        AllLocations.AddRange(PossibleLocations.DeadDrops);
        AllLocations.AddRange(PossibleLocations.ScrapYards);
        AllLocations.AddRange(PossibleLocations.CarCrushers);
        AllLocations.AddRange(PossibleLocations.GangDens);
        AllLocations.AddRange(PossibleLocations.GunStores);
        AllLocations.AddRange(PossibleLocations.Hotels);
        AllLocations.AddRange(PossibleLocations.Residences);
        AllLocations.AddRange(PossibleLocations.CityHalls);
        AllLocations.AddRange(PossibleLocations.VendingMachines);
        AllLocations.AddRange(PossibleLocations.Restaurants);
        AllLocations.AddRange(PossibleLocations.Pharmacies);
        AllLocations.AddRange(PossibleLocations.Dispensaries);
        AllLocations.AddRange(PossibleLocations.HeadShops);
        AllLocations.AddRange(PossibleLocations.HardwareStores);
        AllLocations.AddRange(PossibleLocations.PawnShops);
        AllLocations.AddRange(PossibleLocations.ConvenienceStores);
        AllLocations.AddRange(PossibleLocations.LiquorStores);
        AllLocations.AddRange(PossibleLocations.GasStations);
        AllLocations.AddRange(PossibleLocations.Bars);
        AllLocations.AddRange(PossibleLocations.FoodStands);
        AllLocations.AddRange(PossibleLocations.CarDealerships);
        AllLocations.AddRange(PossibleLocations.DriveThrus);
        AllLocations.AddRange(PossibleLocations.ClothingShops);
        AllLocations.AddRange(PossibleLocations.BusStops);
        AllLocations.AddRange(PossibleLocations.Morgues);
        AllLocations.AddRange(PossibleLocations.SportingGoodsStores);

        AllLocations.AddRange(PossibleLocations.Airports);

        return AllLocations;   
    }

    public List<ILocationSetupable> LocationsToSetup()
    {
        List<ILocationSetupable> AllLocations = new List<ILocationSetupable>();
        AllLocations.AddRange(PossibleLocations.Airports);
        return AllLocations;
    }
    public List<BasicLocation> AllLocations()
    {
        List<BasicLocation> AllLocations = new List<BasicLocation>();
        AllLocations.AddRange(InteractableLocations());
        return AllLocations;
    }
    public List<ILocationDispatchable> PoliceDispatchLocations()
    {
        List<ILocationDispatchable> AllLocations = new List<ILocationDispatchable>();
        AllLocations.AddRange(PossibleLocations.PoliceStations);
        AllLocations.AddRange(PossibleLocations.Prisons);
        return AllLocations;
    }
    public List<ILocationRespawnable> BustedRespawnLocations()
    {
        List<ILocationRespawnable> AllLocations = new List<ILocationRespawnable>();
        AllLocations.AddRange(PossibleLocations.PoliceStations);
        AllLocations.AddRange(PossibleLocations.Prisons);
        return AllLocations;
    }
    public List<ILocationAgencyAssignable> AgencyAssignableLocations()
    {
        List<ILocationAgencyAssignable> AllLocations = new List<ILocationAgencyAssignable>();
        AllLocations.AddRange(PossibleLocations.PoliceStations);
        AllLocations.AddRange(PossibleLocations.Prisons);
        AllLocations.AddRange(PossibleLocations.Hospitals);
        AllLocations.AddRange(PossibleLocations.FireStations);
        return AllLocations;    
    }
    public List<ILocationGangAssignable> GangAssignableLocations()
    {
        List<ILocationGangAssignable> AllLocations = new List<ILocationGangAssignable>();
        AllLocations.AddRange(PossibleLocations.GangDens);
        return AllLocations;
    }
    private void DefaultConfig()
    {
        List<DeadDrop> DeadDrops = new List<DeadDrop>() {

            new DeadDrop(new Vector3(74.97916f,-608.9933f,43.22042f), 249.4708f, "Dead Drop", "the LS 24 newspaper stand near the IAA building" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(75.60421f,-607.5473f,43.22063f), 249.4708f, "Dead Drop", "the Daily Rag newspaper stand by the IAA building" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(75.88783f,-606.5583f,43.22063f), 249.4708f, "Dead Drop", "the Las Mietras newspaper stand by the IAA building" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(76.75777f,-605.3703f,43.22094f), 68.89698f, "Dead Drop", "the trash can by the IAA building" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(-174.7691f,-674.9272f,33.27862f), 249.5148f, "Dead Drop", "the phone booth by the Arcadius Center" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(-223.6883f,-703.9772f,32.59268f), 70.00474f, "Dead Drop", "the mailbox by Schlongberg & Sachs" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(-251.0725f,-739.3169f,31.99848f), 187.8322f, "Dead Drop", "the Daily Rag newspaper stand by Schlongberg & Sachs" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(-263.7065f,-850.4099f,30.48533f), 160.0125f, "Dead Drop", "the Daily Rag newspaper stand by Go Postal" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(-1460.144f,-627.849f,29.69636f), 209.8004f, "Dead Drop", "the dumpster by Swallow" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(-1438.361f,-722.1968f,22.61556f), 171.4681f, "Dead Drop", "the daily rag newspaper stand by Pescado Rojo" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(-1364.685f,-795.9746f,18.32434f), 140.7789f, "Dead Drop", "the trash can in front of the Hedera Hotel" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(-1183.647f,-1257.851f,5.911644f), 260.6368f, "Dead Drop", "the trash can by Taco Libre" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(-1205.34f,-1377.286f,3.174809f), 76.95641f, "Dead Drop", "the trash can by Steamboat Beers" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(803.5943f,-2052.02f,28.30254f), 275.8022f, "Dead Drop", "the trash can by the PiBwasser Plant" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(1047.76f,-2464.003f,27.51101f), 44.61864f, "Dead Drop", "the dumpster by the gun dealers" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(1382.768f,-2042.507f,51.00203f), 30.00222f, "Dead Drop", "the dumpster by Covington Engineering" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(1316.705f,-1657.766f,50.23988f), 309.4677f, "Dead Drop", "the dumpster behind Los Santos Tattoos" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(426.0467f,100.2095f,99.24073f), 337.0558f, "Dead Drop", "the mailbox by Stargaze" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(436.3528f,88.48115f,98.49297f), 159.5108f, "Dead Drop", "the dumpster by Wandlust" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(373.6489f, 351.1104f, 102.8209f), 257.8268f,"Dead Drop", "the dumpster behind 24/7") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(207.6413f, 337.0614f, 105.5466f), 164.1638f, "Dead Drop", "the dumpster behind Pitchers") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(98.17767f, 298.2877f, 110.0032f), 162.2623f, "Dead Drop", "the dumpster behind Up-N-Atom") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(-69.79692f, 282.6463f, 105.0663f), 329.5786f, "Dead Drop", "the trash can outside The Gentry Manor Hotel") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(-391.8292f, 294.4202f, 84.89133f), 86.40096f, "Dead Drop", "the dumpster behind The Last Train Diner") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(-515.2796f, 26.77622f, 44.57598f), 85.66344f, "Dead Drop", "the Daily Rag newspaper stand by Serentiy Wellness") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(-480.7322f, -12.00511f, 45.27599f), 358.5777f, "Dead Drop", "the mailbox in front of Fruit Machine") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(-514.0649f, -40.31401f, 44.51626f), 353.1382f, "Dead Drop", "the dumpster behind The Little Teapot") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(-792.2766f, -250.7747f, 37.20045f), 121.051f, "Dead Drop", "the mailbox in front of Luxury Autos") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(-1237.857f, -554.2683f, 28.91684f), 127.322f, "Dead Drop", "the mailbox in front of City Hall") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(-1323.729f, -675.4391f, 26.51783f), 36.19815f, "Dead Drop", "the Los Santos Meteor newspaper stand in front of Astro Theater") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(-1125.012f, -1616.193f, 4.398425f), 197.6703f, "Dead Drop", "the dumpster behind Vitamin Seaside Juice Bar") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(2544.616f, 378.9412f, 108.6173f), 264.3849f, "Dead Drop", "the boxes behind the 24/7") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(2569.69f, 2727.281f, 43.17296f), 300.3225f, "Dead Drop", "the port-o-potty at Davis Quartz") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(1532.096f, 3797.162f, 33.51709f), 203.1575f, "Dead Drop", "the back of the Boathouse") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(1987.924f, 3789.832f, 32.18082f), 128.6473f, "Dead Drop", "the toolbox behind Sandy's Gas") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(923.066f, 3652.77f, 32.59369f), 91.28005f, "Dead Drop", "the dumpster behind the Liquor Market") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(-678.5425f, 5833.054f, 17.33131f), 310.8695f, "Dead Drop", "the mailbox in front of The BayviewLodge") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(-125.5132f, 6345.283f, 31.49037f), 231.2274f, "Dead Drop", "the dumpster behind the Dream View Motel") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(1725.162f, 4732.389f, 42.1343f), 103.1211f, "Dead Drop", "the boxes behind The Feed Store") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(1731.202f, 4758.293f, 41.89993f), 265.2589f, "Dead Drop", "the dumpster behind the supermarket") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(1692.661f, 6431.451f, 32.76194f), 336.9627f, "Dead Drop", "the payphone by 24/7") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(-3165.1f, 1114.949f, 20.79292f), 334.1037f, "Dead Drop", "the dumpster behind Nelsons General Store") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(-2297.884f, 248.9572f, 169.6021f), 213.5057f, "Dead Drop", "the trash can near the parking lot of the Kortz Center") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(172.9656f, -1799.744f, 29.20135f), 133.2921f, "Dead Drop", "the mailbox in front of The Locksmith") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(88.87651f, -1662.244f, 29.29333f), 28.54718f, "Dead Drop", "the Daily Rag newspaper stand near the Convenience Store") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
        };

        List<CarCrusher> CarCrushers = new List<CarCrusher>() {
            new CarCrusher(new Vector3(-538.0128f, -1720.554f, 19.4278f), 324.869f, "The Crushinator", "Dead skunk in the trunk?") { OpenTime = 0, CloseTime = 24, 
                CameraPosition = new Vector3(-527.7763f, -1704.601f, 27.12083f), CameraDirection = new Vector3(0.005943439f, -0.9625598f, -0.2710043f), CameraRotation = new Rotator(-15.72404f, 1.940237E-07f, -179.6462f) },
            //new CarCrusher(new Vector3(909.7432f, 3554.745f, 33.81702f), 211.2794f, "Marina Drive Scrap", "Top value for your 'questionable' provenance ") { OpenTime = 0, CloseTime = 24 },
            //new CarCrusher(new Vector3(-195.9066f, 6264.628f, 31.48937f), 41.33705f, "Red's Machine Supplies", "Parts Bought and Sold!") { OpenTime = 0, CloseTime = 24 },
        };


        List<ScrapYard> ScrapYards = new List<ScrapYard>() {
            new ScrapYard(new Vector3(1520.797f, -2113.375f, 76.86716f), 270.4797f, "Wesley's Scrap Yard", "Don't Ask, Don't Tell!") { OpenTime = 0, CloseTime = 24 },
            new ScrapYard(new Vector3(909.7432f, 3554.745f, 33.81702f), 211.2794f, "Marina Drive Scrap", "Top value for your 'questionable' provenance ") { OpenTime = 0, CloseTime = 24 },
            new ScrapYard(new Vector3(-195.9066f, 6264.628f, 31.48937f), 41.33705f, "Red's Machine Supplies", "Parts Bought and Sold!") { OpenTime = 0, CloseTime = 24 },
        };
        List<GangDen> GangDens = new List<GangDen>()
        {
            new GangDen(new Vector3(1662.302f, 4776.384f, 42.00795f), 279.1427f, "Pavano Safehouse", "","PavanoDenMenu", "AMBIENT_GANG_PAVANO") { CanInteractWhenWanted = true, MapIcon = 267, BannerImagePath = "pavano.png", OpenTime = 0,CloseTime = 24, IsEnabled = false, PossiblePedSpawns = new List<ConditionalLocation>() {

                new ConditionalLocation(new Vector3(1664.384f, 4765.325f, 42.0682f), 290.2068f, 50f),
                new ConditionalLocation(new Vector3(1662.094f, 4778.333f, 42.00935f), 259.006f, 50f),
                new ConditionalLocation(new Vector3(1647.39f, 4779.691f, 42.01637f), 9.346325f, 50f),
                new ConditionalLocation(new Vector3(1666.244f, 4773.587f, 41.93539f), 215.5277f, 50f),
                new ConditionalLocation(new Vector3(1665.208f, 4778.518f, 41.94879f), 271.264f, 50f),

            }, PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new ConditionalLocation(new Vector3(1669.677f, 4776.892f, 41.22393f), 4.480381f, 75f),
                new ConditionalLocation(new Vector3(1662.887f, 4768.458f, 41.3252f), 278.0306f, 75f), 
            }
            },//Grapeseed Shack
            new GangDen(new Vector3(-229.6159f, 6445.189f, 31.19745f), 139.3764f, "Lupisella Safehouse", "","LupisellaDenMenu", "AMBIENT_GANG_LUPISELLA") { CanInteractWhenWanted = true, MapIcon = 77, BannerImagePath = "lupisella.png", OpenTime = 0,CloseTime = 24, IsEnabled = false, PossiblePedSpawns = new List<ConditionalLocation>() {

                new ConditionalLocation(new Vector3(-233.3555f, 6447.341f, 31.19741f), 131.7259f, 50f),
                new ConditionalLocation(new Vector3(-227.5764f, 6442.54f, 31.19769f), 141.2697f, 50f),
                new ConditionalLocation(new Vector3(-217.9765f, 6432.755f, 31.19775f), 165.7642f, 50f),
                new ConditionalLocation(new Vector3(-212.6066f, 6443.875f, 31.2976f), 326.0233f, 50f),
                new ConditionalLocation(new Vector3(-215.7044f, 6445.802f, 31.3135f), 270.9214f, 50f),

            }, PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new ConditionalLocation(new Vector3(-226.1172f, 6435.622f, 30.51782f), 232.3148f, 75f),
                new ConditionalLocation(new Vector3(-210.5319f, 6437.373f, 30.72163f), 327.138f, 75f),
            } },//Beachhouse in Paleto
            new GangDen(new Vector3(-1629.715f, 36.49737f, 62.93618f), 333.3146f, "Messina Safehouse", "","MessinaDenMenu", "AMBIENT_GANG_MESSINA") { CanInteractWhenWanted = true, MapIcon = 78, BannerImagePath = "messina.png", OpenTime = 0,CloseTime = 24, IsEnabled = false, PossiblePedSpawns = new List<ConditionalLocation>() {

                new ConditionalLocation(new Vector3(-1630.811f, 39.70509f, 62.54136f), 323.2733f, 50f),
                new ConditionalLocation(new Vector3(-1626.534f, 37.39336f, 62.54136f), 342.5028f, 50f),
                new ConditionalLocation(new Vector3(-1614.67f, 29.32284f, 62.54132f), 257.1678f, 50f),
                new ConditionalLocation(new Vector3(-1620.178f, 19.3019f, 62.54137f), 269.8025f, 50f),
                new ConditionalLocation(new Vector3(-1639.313f, 16.93647f, 62.53671f), 122.0091f, 50f),

            }, PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new ConditionalLocation(new Vector3(-1613.357f, 22.61485f, 61.48566f), 155.361f, 75f),
                new ConditionalLocation(new Vector3(-1625.055f, 65.92033f, 61.15781f), 237.2095f, 75f),
            } },//mansion in richman
            new GangDen(new Vector3(-3228.478f, 1092.326f, 10.76322f), 253.458f, "Ancelotti Safehouse", "","AncelottiDenMenu", "AMBIENT_GANG_ANCELOTTI") { CanInteractWhenWanted = true, MapIcon = 76, BannerImagePath = "ancelotti.png", OpenTime = 0,CloseTime = 24, IsEnabled = false, PossiblePedSpawns = new List<ConditionalLocation>() {

                new ConditionalLocation(new Vector3(-3228.078f, 1089.288f, 10.73698f), 243.1453f, 50f),
                new ConditionalLocation(new Vector3(-3226.294f, 1095.004f, 10.69816f), 260.7955f, 50f),
                new ConditionalLocation(new Vector3(-3240.798f, 1081.971f, 7.396926f), 157.9129f, 50f),
                new ConditionalLocation(new Vector3(-3242.731f, 1087.94f, 7.478574f), 115.9338f, 50f),
                new ConditionalLocation(new Vector3(-3247.704f, 1098.255f, 2.835759f), 70.83805f, 50f),

            }, PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new ConditionalLocation(new Vector3(-3226.144f, 1086.399f, 9.993718f), 162.1115f, 75f),
                new ConditionalLocation(new Vector3(-3229.512f, 1077.396f, 10.19788f), 260.5915f, 75f),

            } },//beachhousein chumash
            new GangDen(new Vector3(-1157.501f, -1451.861f, 4.468448f), 216.5082f, "Yardies Chill Spot", "","YardiesDenMenu", "AMBIENT_GANG_YARDIES") { CanInteractWhenWanted = true, BannerImagePath = "yardies.png", MapIconColor = Color.Green, OpenTime = 0,CloseTime = 24, IsEnabled = false, PossiblePedSpawns = new List<ConditionalLocation>() {

                new ConditionalLocation(new Vector3(-1155.236f, -1450.92f, 4.508356f), 203.7991f, 50f),
                new ConditionalLocation(new Vector3(-1158.698f, -1454.306f, 4.346763f), 192.8651f, 50f),
                new ConditionalLocation(new Vector3(-1166.395f, -1453.536f, 4.367522f), 122.2634f, 50f),
                new ConditionalLocation(new Vector3(-1177.108f, -1437.649f, 4.379432f), 75.27589f, 50f),
                new ConditionalLocation(new Vector3(-1171.944f, -1470.263f, 4.382764f), 290.1982f, 50f),

            }, PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new ConditionalLocation(new Vector3(-1156.718f, -1462.597f, 3.70943f), 210.7057f, 75f),
                new ConditionalLocation(new Vector3(-1150.134f, -1456.732f, 3.955124f), 215.4072f, 75f),

            } },//near shops on del perro beach
            new GangDen(new Vector3(275.2381f, -3015.519f, 5.945963f), 91.01478f, "Diablos Hangout", "","DiablosDenMenu", "AMBIENT_GANG_DIABLOS") { CanInteractWhenWanted = true, MapIcon = 355, BannerImagePath = "diablos.png", OpenTime = 0,CloseTime = 24, IsEnabled = false, PossiblePedSpawns = new List<ConditionalLocation>() {

                new ConditionalLocation(new Vector3(274.3982f, -3017.684f, 5.699762f), 86.04555f, 50f),
                new ConditionalLocation(new Vector3(274.2913f, -3010.913f, 5.70001f), 105.1246f, 50f),
                new ConditionalLocation(new Vector3(281.5445f, -2997.272f, 5.672155f), 5.840774f, 50f),
                new ConditionalLocation(new Vector3(285.892f, -3025.893f, 5.652033f), 304.8488f, 50f),
                new ConditionalLocation(new Vector3(290.5836f, -3035.765f, 5.882213f), 174.9672f, 50f),

            }, PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new ConditionalLocation(new Vector3(260.6703f, -3008.191f, 5.070713f), 87.13465f, 75f),
                new ConditionalLocation(new Vector3(286.6504f, -2996.586f, 5.060739f), 85.31966f, 75f),
            } },//shitty shack in elysian
            new GangDen(new Vector3(514.9427f, 190.9465f, 104.745f), 356.6495f, "Gambetti Safehouse", "","GambettiDenMenu", "AMBIENT_GANG_GAMBETTI") { CanInteractWhenWanted = true, MapIcon = 541, BannerImagePath = "gambetti.png", OpenTime = 0,CloseTime = 24, IsEnabled = false, PossiblePedSpawns = new List<ConditionalLocation>() {

                new ConditionalLocation(new Vector3(517.7187f, 191.4421f, 104.745f), 344.8743f, 50f),
                new ConditionalLocation(new Vector3(504.8874f, 201.7501f, 104.7445f), 334.6481f, 50f),
                new ConditionalLocation(new Vector3(541.257f, 201.7959f, 101.5426f), 246.2837f, 50f),

                new ConditionalLocation(new Vector3(546.3932f, 214.8331f, 102.2434f), 320.1989f, 50f),
                new ConditionalLocation(new Vector3(539.8316f, 241.9526f, 103.1213f), 324.5678f, 50f),
                new ConditionalLocation(new Vector3(516.8485f, 250.2658f, 103.1146f), 350.7859f, 50f),

            }, PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new ConditionalLocation(new Vector3(464.1385f, 226.9263f, 102.5122f), 67.93208f, 75f),
                new ConditionalLocation(new Vector3(462.139f, 222.349f, 102.2742f), 245.8917f, 75f),

            } },//Downtown Vinewood
            new GangDen(new Vector3(1389.966f, 1131.907f, 114.3344f), 91.72424f, "Madrazo Cartel Den", "","MadrazoDenMenu", "AMBIENT_GANG_MADRAZO") { CanInteractWhenWanted = true, MapIcon = 78, BannerImagePath = "madrazo.png", OpenTime = 0,CloseTime = 24, IsEnabled = false, PossiblePedSpawns = new List<ConditionalLocation>() {

                new ConditionalLocation(new Vector3(1390.856f, 1139.184f, 114.4433f), 56.59644f, 50f),
                new ConditionalLocation(new Vector3(1383.559f, 1156.695f, 114.3345f), 170.1522f, 50f),
                new ConditionalLocation(new Vector3(1375.004f, 1149.209f, 113.9089f), 91.94662f, 50f),
                new ConditionalLocation(new Vector3(1412.588f, 1138.611f, 114.3341f), 196.4261f, 50f),
            }, PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
new ConditionalLocation(new Vector3(1410.701f, 1119.38f, 114.5649f), 89.85777f, 75f),
new ConditionalLocation(new Vector3(1367.869f, 1147.611f, 113.4886f), 1.871943f, 75f),
            } },
            new GangDen(new Vector3(-615.221f, -1787.458f, 23.69615f), 210.6709f, "Armenian Hangout", "","ArmenianDenMenu", "AMBIENT_GANG_ARMENIAN") { CanInteractWhenWanted = true, MapIcon = 76, BannerImagePath ="armenian.png", OpenTime = 0,CloseTime = 24, IsEnabled = false, 
                PossiblePedSpawns = new List<ConditionalLocation>() {

                new ConditionalLocation(new Vector3(-608.3129f, -1786.241f, 23.63522f), 191.6332f, 50f),
                new ConditionalLocation(new Vector3(-602.466f, -1784.252f, 23.64002f), 120.1152f, 50f),
                new ConditionalLocation(new Vector3(-600.0588f, -1795.333f, 23.38985f), 155.3086f, 50f),
                new ConditionalLocation(new Vector3(-622.9163f, -1792.963f, 23.80351f), 163.1899f, 50f),
                new ConditionalLocation(new Vector3(-591.4926f, -1776.089f, 22.79006f), 214.9723f, 50f),
            }, 
                PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
new ConditionalLocation(new Vector3(-597.8096f, -1791.076f, 22.92511f), 300.9409f, 75f),
new ConditionalLocation(new Vector3(-607.3063f, -1788.965f, 22.9275f), 162.6913f, 75f),
            },
                CameraPosition = new Vector3(-589.6008f, -1798.514f, 25.45085f), 
                CameraDirection = new Vector3(0.586422f, 0.7246112f, -0.3620054f), 
                CameraRotation = new Rotator(-21.2234f, -7.327146E-06f, -38.98296f),
                ItemPreviewPosition = new Vector3(-585.7418f, -1792.97f, 22.85472f),
                ItemPreviewHeading = 53.11781f,
                ItemDeliveryLocations = new List<SpawnPlace>() {
                    new SpawnPlace(new Vector3(-581.0074f, -1797.569f, 22.8803f), 322.0219f),
                    new SpawnPlace(new Vector3(-584.7271f, -1780.009f, 22.66464f), 144.9928f),
                    new SpawnPlace(new Vector3(-586.3314f, -1804.048f, 22.58966f), 115.4426f),



                } },

            


            new GangDen(new Vector3(-223.1647f, -1601.309f, 34.88379f), 266.3889f, "The Families Den", "The OGs","FamiliesDenMenu", "AMBIENT_GANG_FAMILY") { CanInteractWhenWanted = true, MapIcon = 86, BannerImagePath = "families.png",OpenTime = 0,CloseTime = 24, IsEnabled = false, PossiblePedSpawns = new List<ConditionalLocation>() {

                new ConditionalLocation(new Vector3(-219.9773f, -1594.828f, 34.86927f), 326.5532f, 50f),
                new ConditionalLocation(new Vector3(-214.3197f, -1580.346f, 34.86931f), 147.0013f, 50f),
                new ConditionalLocation(new Vector3(-234.661f, -1606.322f, 34.26423f), 70.74878f, 50f),
                new ConditionalLocation(new Vector3(-234.2032f, -1602.192f, 34.30891f), 104.5916f, 50f),
                new ConditionalLocation(new Vector3(-197.1071f, -1604.178f, 34.36404f), 260.1856f, 50f),
                new ConditionalLocation(new Vector3(-191.8121f, -1595.623f, 34.5155f), 263.9107f, 50f),
            }, PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new ConditionalLocation(new Vector3(-237.4984f, -1612.648f, 33.17708f), 179.3492f, 75f),
                new ConditionalLocation(new Vector3(-221.4851f, -1633.947f, 32.93398f), 269.4803f, 75f),
                new ConditionalLocation(new Vector3(-176.6605f, -1608.427f, 32.9703f), 342.4364f, 75f),
                new ConditionalLocation(new Vector3(-189.6106f, -1611.435f, 33.20397f), 174.0037f, 75f),

            } },//This is in Chamberlain Hills
            new GangDen(new Vector3(86.11255f, -1959.272f, 21.12167f), 318.5057f, "Ballas Den", "","BallasDenMenu", "AMBIENT_GANG_BALLAS") { CanInteractWhenWanted = true, MapIcon = 106, BannerImagePath = "ballas.png",OpenTime = 0,CloseTime = 24, IsEnabled = false, PossiblePedSpawns = new List<ConditionalLocation>() {

                new ConditionalLocation(new Vector3(84.76484f, -1953.536f, 20.8518f), 334.0088f, 50f),
                new ConditionalLocation(new Vector3(87.02995f, -1947.637f, 20.74858f), 303.2596f, 50f),
                new ConditionalLocation(new Vector3(95.30958f, -1954.979f, 20.75126f), 314.5049f, 50f),
                new ConditionalLocation(new Vector3(84.23887f, -1932.319f, 20.74922f), 19.71852f, 50f),
            }, PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
new ConditionalLocation(new Vector3(94.70525f, -1960.741f, 20.06409f), 322.6508f, 75f),
new ConditionalLocation(new Vector3(108.7719f, -1951.384f, 20.01156f), 294.1027f, 75f),
new ConditionalLocation(new Vector3(113.3118f, -1933.905f, 19.9819f), 36.83981f, 75f),
new ConditionalLocation(new Vector3(68.90487f, -1922.226f, 20.57331f), 130.7354f, 75f),
            } },//This is in DAVIS near Grove Street
            new GangDen(new Vector3(967.6899f, -1867.115f, 31.44757f), 176.7243f, "Vagos Den", "","VagosDenMenu", "AMBIENT_GANG_MEXICAN") {  CanInteractWhenWanted = true, BannerImagePath = "vagos.png",OpenTime = 0,CloseTime = 24, IsEnabled = false, PossiblePedSpawns = new List<ConditionalLocation>() {

                new ConditionalLocation(new Vector3(963.3468f, -1869.211f, 31.21046f), 130.5827f, 50f),
                new ConditionalLocation(new Vector3(970.806f, -1869.026f, 31.31658f), 163.6367f, 50f),
                new ConditionalLocation(new Vector3(968.0837f, -1893.529f, 31.14556f), 357.2057f, 50f),
                new ConditionalLocation(new Vector3(979.5701f, -1867.066f, 31.19429f), 259.3384f, 50f),
            }, PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
new ConditionalLocation(new Vector3(966.6174f, -1873.199f, 30.44685f), 41.87259f, 75f),
new ConditionalLocation(new Vector3(971.0187f, -1873.486f, 30.48772f), 40.74247f, 75f),
new ConditionalLocation(new Vector3(983.9246f, -1860.417f, 30.36379f), 179.4304f, 75f),
new ConditionalLocation(new Vector3(990.0909f, -1892.571f, 29.7773f), 263.3322f, 75f),
            } },
            new GangDen(new Vector3(1193.61f, -1656.411f, 43.02641f), 31.55427f, "Varrios Los Aztecas Den", "","VarriosDenMenu", "AMBIENT_GANG_SALVA") { CanInteractWhenWanted = true, BannerImagePath = "varrios.png",OpenTime = 0,CloseTime = 24, IsEnabled = false, PossiblePedSpawns = new List<ConditionalLocation>() {

                new ConditionalLocation(new Vector3(1193.946f, -1651.643f, 42.358f), 18.49724f, 50f),
                new ConditionalLocation(new Vector3(1189.079f, -1655.169f, 42.358f), 338.5124f, 50f),
                new ConditionalLocation(new Vector3(1190.508f, -1648.968f, 41.43568f), 34.16187f, 50f),
                new ConditionalLocation(new Vector3(1172.624f, -1645.602f, 36.78029f), 94.79508f, 50f),
            }, PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
new ConditionalLocation(new Vector3(1166.678f, -1647.833f, 36.23235f), 132.0248f, 75f),
new ConditionalLocation(new Vector3(1157.749f, -1660.837f, 35.93925f), 205.3036f, 75f),
new ConditionalLocation(new Vector3(1184.424f, -1652.161f, 39.12302f), 312.0581f, 75f),
            } },
            new GangDen(new Vector3(1299.267f, -1752.92f, 53.88011f), 110.3803f, "Marabunta Grande Den", "","MarabunteDenMenu", "AMBIENT_GANG_MARABUNTE") { CanInteractWhenWanted = true, MapIcon = 78, BannerImagePath = "marabunta.png", OpenTime = 0,CloseTime = 24, IsEnabled = false, PossiblePedSpawns = new List<ConditionalLocation>() {

                new ConditionalLocation(new Vector3(1301.137f, -1754.575f, 53.87848f), 184.4178f, 50f),
                new ConditionalLocation(new Vector3(1293.167f, -1748.509f, 53.87848f), 200.1636f, 50f),
                new ConditionalLocation(new Vector3(1313.939f, -1771.984f, 54.81713f), 113.8552f, 50f),
                new ConditionalLocation(new Vector3(1319.703f, -1775.434f, 54.5059f), 215.1372f, 50f),
            }, PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
new ConditionalLocation(new Vector3(1322.044f, -1768.506f, 54.82401f), 195.1561f, 75f),
new ConditionalLocation(new Vector3(1313.927f, -1783.216f, 51.72291f), 109.9802f, 75f),
            } },
            new GangDen(new Vector3(-1144.041f, 4908.383f, 220.9688f), 33.69744f, "Altruist Cult Den", "","GenericGangDenMenu", "AMBIENT_GANG_CULT") { CanInteractWhenWanted = true,MapIcon = 76, BannerImagePath = "altruist.png",OpenTime = 0,CloseTime = 24, IsEnabled = false, },

            //new GangDen(new Vector3(-766.3793f, -917.0612f, 21.29704f), 268.4079f, "Kkangpae Den", "","KkangpaeDenMenu", "AMBIENT_GANG_KKANGPAE") {  OpenTime = 0,CloseTime = 24, IsEnabled = false, PossiblePedSpawns = new List<ConditionalLocation>() {

            //    new ConditionalLocation(new Vector3(-761.3151f, -910.3087f, 19.53444f), 237.0572f, 50f),
            //    new ConditionalLocation(new Vector3(-762.3676f, -923.8786f, 18.74619f), 237.6777f, 50f),
            //    new ConditionalLocation(new Vector3(-760.9077f, -927.9499f, 18.47775f), 292.1328f, 50f),
            //    new ConditionalLocation(new Vector3(-764.5098f, -919.3218f, 20.20216f), 267.6916f, 50f),
            //} },

            new GangDen(new Vector3(-579.9809f, -778.5275f, 25.01723f), 90.93932f, "Kkangpae Den", "","KkangpaeDenMenu", "AMBIENT_GANG_KKANGPAE") { CanInteractWhenWanted = true, BannerImagePath = "kkangpae.png",  OpenTime = 0,CloseTime = 24, IsEnabled = false, PossiblePedSpawns = new List<ConditionalLocation>() {

                new ConditionalLocation(new Vector3(-585.9933f, -776.1322f, 25.01723f), 167.2549f, 50f),
                new ConditionalLocation(new Vector3(-581.7299f, -780.7718f, 25.01723f), 66.59444f, 50f),
                new ConditionalLocation(new Vector3(-613.7438f, -782.4008f, 25.20246f), 42.77346f, 50f),
                new ConditionalLocation(new Vector3(-622.1181f, -768.7686f, 25.95107f), 88.71659f, 50f),
            }, PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
new ConditionalLocation(new Vector3(-610.1835f, -756.0798f, 26.07551f), 266.9069f, 75f),
new ConditionalLocation(new Vector3(-618.8381f, -761.884f, 25.46778f), 89.04613f, 75f),

            } },
            new GangDen(new Vector3(959.721f, 3618.905f, 32.67253f), 93.92658f, "Reckneck Den", "","GenericGangDenMenu", "AMBIENT_GANG_HILLBILLY") { CanInteractWhenWanted = true, BannerImagePath = "rednecks.png", OpenTime = 0,CloseTime = 24, IsEnabled = false, PossiblePedSpawns = new List<ConditionalLocation>() {

                new ConditionalLocation(new Vector3(957.8521f, 3616.273f, 32.75988f), 56.09721f, 50f),
                new ConditionalLocation(new Vector3(959.2394f, 3612.307f, 32.75033f), 138.3261f, 50f),
                new ConditionalLocation(new Vector3(969.4822f, 3626.386f, 32.33695f), 17.91204f, 50f),
            }, PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
new ConditionalLocation(new Vector3(950.5566f, 3618.955f, 31.88234f), 268.8919f, 75f),
new ConditionalLocation(new Vector3(950.8107f, 3622.341f, 31.76403f), 271.0765f, 75f),
new ConditionalLocation(new Vector3(950.5989f, 3615.652f, 31.93583f), 271.2046f, 75f),

            } },
            new GangDen(new Vector3(981.8542f, -103.0203f, 74.84874f), 220.3094f,  "Lost M.C. Clubhouse", "","LostDenMenu", "AMBIENT_GANG_LOST") { CanInteractWhenWanted = true, MapIcon = 226, BannerImagePath = "lostmc.png", OpenTime = 0,CloseTime = 24, IsEnabled = false, 
                PossiblePedSpawns = new List<ConditionalLocation>() {

                new ConditionalLocation(new Vector3(987.3098f, -107.0706f, 74.32984f), 138.8955f, 50f),
                new ConditionalLocation(new Vector3(975.0338f, -112.2333f, 74.35313f), 188.1823f, 50f),
                new ConditionalLocation(new Vector3(960.2211f, -123.5155f, 74.35313f), 181.6973f, 50f),
                new ConditionalLocation(new Vector3(981.1419f, -142.9071f, 74.23688f), 80.7939f, 50f),
                new ConditionalLocation(new Vector3(976.2161f, -144.2681f, 74.24052f), 0.7361088f, 50f),
            }, 
                PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new ConditionalLocation(new Vector3(976.4726f, -132.778f, 73.21405f), 58.86649f, 75f),
                new ConditionalLocation(new Vector3(960.3214f, -132.2091f, 73.69121f), 189.5355f, 75f),
                new ConditionalLocation(new Vector3(972.7709f, -142.4676f, 73.60614f), 239.6191f, 75f),
                new ConditionalLocation(new Vector3(972.8246f, -155.1686f, 72.8443f), 52.89192f, 75f),

            },
                CameraPosition = new Vector3(961.1312f, -131.4942f, 76.40847f), 
                CameraDirection = new Vector3(-0.7768034f, 0.5811784f, -0.2425037f), 
                CameraRotation = new Rotator(-14.03436f, 4.840233E-06f, 53.19739f),
                ItemPreviewPosition = new Vector3(955.3662f, -127.888f, 74.37495f), 
                ItemPreviewHeading = 150.3339f,
                ItemDeliveryLocations = new List<SpawnPlace>() {
                    new SpawnPlace(new Vector3(955.2133f, -133.7419f, 74.44804f),239.5793f),
                    new SpawnPlace(new Vector3(967.6667f, -141.4849f, 74.39858f), 239.8698f),
                    new SpawnPlace(new Vector3(986.6042f, -138.3367f, 73.09078f), 59.13091f),
                    new SpawnPlace(new Vector3(968.5606f, -122.4432f, 74.35313f), 153.9187f),
                } }, 



            new GangDen(new Vector3(101.6865f, -819.3801f, 31.31512f), 341.2845f,  "Triad Den", "","TriadsDenMenu", "AMBIENT_GANG_WEICHENG") { CanInteractWhenWanted = true, BannerImagePath = "triad.png", OpenTime = 0,CloseTime = 24,IsEnabled = false, PossiblePedSpawns = new List<ConditionalLocation>() {

                new ConditionalLocation(new Vector3(99.74276f, -816.0181f, 31.40174f), 2.775635f, 50f),
                new ConditionalLocation(new Vector3(108.1241f, -817.763f, 31.36724f), 273.9551f, 50f),
                new ConditionalLocation(new Vector3(92.21131f, -817.7866f, 31.31864f), 55.81263f, 50f),
                new ConditionalLocation(new Vector3(114.8412f, -819.1249f, 31.32478f), 342.2915f, 50f),
            }, PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
new ConditionalLocation(new Vector3(85.50445f, -818.2582f, 30.74365f), 155.5453f, 75f),
new ConditionalLocation(new Vector3(59.77588f, -848.8732f, 30.33472f), 334.658f, 75f),
new ConditionalLocation(new Vector3(87.69022f, -827.7574f, 30.66308f), 340.578f, 75f),

            } },



        };
        List<GunStore> GunStores = new List<GunStore>()

        {
            new GunStore(new Vector3(1049.596f, -2428.15f, 30.30457f), 84.97017f, "Guns #1", "General shop","GunShop1") { IsEnabled = true, ContactName = StaticStrings.UndergroundGunsContactName, 
                ParkingSpaces = new List<SpawnPlace>() { 
                    new SpawnPlace(new Vector3(1044.326f, -2404.086f, 29.69204f),352.682f),

                    new SpawnPlace(new Vector3(1044.763f, -2397.967f, 29.08815f), 355.825f),
                    new SpawnPlace(new Vector3(1045.676f, -2389.352f, 29.35636f), 355.9221f),
                    new SpawnPlace(new Vector3(1061.272f, -2445.152f, 28.1964f), 89.23737f),
                    new SpawnPlace(new Vector3(1066.94f, -2463.334f, 27.96432f), 357.0727f),
                } },
            new GunStore(new Vector3(-232.552f, -1311.643f, 31.29598f), 3.180501f, "Guns #2", "Specializes in ~o~Pistols~s~","GunShop5") { IsEnabled = false, MoneyToUnlock = 5000, ContactName = StaticStrings.UndergroundGunsContactName,                
                ParkingSpaces = new List<SpawnPlace>() {
                    new SpawnPlace(new Vector3(-233.4157f, -1306.795f, 31.31846f),89.50895f),

                    new SpawnPlace(new Vector3(-221.8178f, -1291.497f, 30.46753f), 38.83605f),
                    new SpawnPlace(new Vector3(-215.6527f, -1306.28f, 30.48727f), 275.802f),
                    new SpawnPlace(new Vector3(-198.912f, -1300.158f, 30.46804f), 260.0593f),
                    new SpawnPlace(new Vector3(-223.2236f, -1308.06f, 30.46892f), 89.06057f),
            } },
            new GunStore(new Vector3(334.3036f, -1978.458f, 24.16728f), 49.9404f, "Guns #3", "Specializes in ~o~Sub-Machine Guns~s~","GunShop3") { IsEnabled = false, MoneyToUnlock = 10000, ContactName = StaticStrings.UndergroundGunsContactName,                
                ParkingSpaces = new List<SpawnPlace>() {
                    new SpawnPlace(new Vector3(322.9245f, -1981.151f, 23.22856f),305.8783f),

                    new SpawnPlace(new Vector3(317.3759f, -1986.411f, 21.63353f), 137.6388f),
                    new SpawnPlace(new Vector3(318.4567f, -2005.651f, 21.25648f), 205.8739f),
                    new SpawnPlace(new Vector3(308.2981f, -1978.542f, 21.6238f), 141.6945f),
            } },
            new GunStore(new Vector3(-258.3577f, 6247.281f, 31.48922f), 314.4655f, "Guns #4", "Specializes in ~o~Assault Rifles~s~","GunShop4") { IsEnabled = false, MoneyToUnlock = 15000, ContactName = StaticStrings.UndergroundGunsContactName,                
                ParkingSpaces = new List<SpawnPlace>() {
                    new SpawnPlace(new Vector3(-258.1833f, 6250.695f, 31.48922f),25.17568f),

                    new SpawnPlace(new Vector3(-256.5694f, 6265.761f, 30.58578f), 317.2921f),
                    new SpawnPlace(new Vector3(-267.765f, 6251.046f, 30.61459f), 312.4225f),
                    new SpawnPlace(new Vector3(-269.5155f, 6274.559f, 30.46577f), 136.3388f),
                } },
            new GunStore(new Vector3(1673.425f, 4957.921f, 42.34893f), 227.3988f, "Guns #5", "Specializes in ~o~Heavy Weapons~s~","GunShop2") { IsEnabled = false, MoneyToUnlock = 25000, ContactName = StaticStrings.UndergroundGunsContactName,                
                ParkingSpaces = new List<SpawnPlace>() {
                    new SpawnPlace(new Vector3(1661.471f, 4951.514f, 42.07043f),217.9459f),

                    new SpawnPlace(new Vector3(1659.542f, 4946.245f, 41.20879f), 224.609f),
                    new SpawnPlace(new Vector3(1695.27f, 4973.078f, 42.8186f), 288.7947f),
                    new SpawnPlace(new Vector3(1703.823f, 4947.335f, 41.68751f), 54.86424f),
                } },

        };
        List<Hotel> Hotels = new List<Hotel>()

        {

            new Hotel(new Vector3(-1183.073f, -1556.673f, 5.036984f), 122.3785f, "Vespucci", "Free Cable","CheapHotelMenu") {OpenTime = 0, CloseTime = 24, FullName = "Vespucci Hotel" },
            new Hotel(new Vector3(-1343.127f, -1091.096f, 6.936333f), 299.9456f, "Venetian", "Get the dirty water experience","ExpensiveHotelMenu") {OpenTime = 0, CloseTime = 24 },
            new Hotel(new Vector3(-1309.048f, -931.2507f, 13.35856f), 23.25741f, "Crown Jewel Hotel", "The best of our family jewels","CheapHotelMenu") {OpenTime = 0, CloseTime = 24, FullName = "The Crown Jewel Hotel" },
            new Hotel(new Vector3(-1660.706f, -533.756f, 36.02398f), 141.6077f, "Banner", "Luxury as only a faceless corp can provide","ExpensiveHotelMenu") {OpenTime = 0, CloseTime = 24, FullName = "Banner Hotel & Spa", CameraPosition = new Vector3(-1660.326f, -566.6978f, 39.62436f), CameraRotation = new Rotator(-11.99999f, 1.091059E-07f, 4.528234f) },//, new Vector3(0f, 0f, 0f), //Camera Position LocationName: bannerhotel
            new Hotel(new Vector3(-287.0405f, -1060.003f, 27.20538f), 252.0524f, "Banner", "Luxury as only a faceless corp can provide","ExpensiveHotelMenu") {OpenTime = 0, CloseTime = 24, FullName = "Banner Hotel & Spa", CameraPosition = new Vector3(-233.506f, -1048.275f, 34.58431f), CameraDirection = new Vector3(-0.9516708f, -0.2260422f, -0.2079124f), CameraRotation = new Rotator(-12.00004f, 0f, 103.3614f) },

            new Hotel(new Vector3(-1856.868f, -347.9391f, 49.83775f), 141.5183f, "Von Crastenburg", "From the family you love","ExpensiveHotelMenu") {OpenTime = 0, CloseTime = 24, CameraPosition = new Vector3(-1883.016f, -389.9088f, 59.78009f), CameraDirection = new Vector3(0.4398794f, 0.8545721f, -0.2760664f), CameraRotation = new Rotator(-16.02558f, -1.154782E-05f, -27.23653f) },//needs zoom out
            new Hotel(new Vector3(-875.8169f, -2110.466f, 9.918293f), 41.67873f, "Von Crastenburg", "From the family you love","ExpensiveHotelMenu"){OpenTime = 0, CloseTime = 24,CameraPosition = new Vector3(-911.9993f, -2080.375f, 17.51382f), CameraDirection = new Vector3(0.7078345f, -0.6861629f, -0.1677819f), CameraRotation = new Rotator(-9.658877f, 2.468244E-05f, -134.1093f) },
            new Hotel(new Vector3(435.6202f, 214.7496f, 103.1663f), 340.5429f, "Von Crastenburg","From the family you love","ExpensiveHotelMenu"){OpenTime = 0, CloseTime = 24, CameraPosition = new Vector3(427.1386f, 246.8979f, 113.1878f), CameraDirection = new Vector3(0.1895126f, -0.9617067f, -0.1980029f), CameraRotation = new Rotator(-11.4202f, 3.919582E-06f, -168.8522f) },

            new Hotel(new Vector3(68.509f, -958.8935f, 29.80383f), 161.9325f, "The Emissary", "","ExpensiveHotelMenu"){OpenTime = 0, CloseTime = 24 ,CameraPosition = new Vector3(81.53342f, -1010.819f, 63.66661f), CameraDirection = new Vector3(-0.1635272f, 0.9643815f, -0.2079113f), CameraRotation = new Rotator(-11.99998f, 2.182118E-07f, 9.623925f) },
            new Hotel(new Vector3(104.8123f, -932.9781f, 29.81516f), 248.7484f, "The Emissary", "","ExpensiveHotelMenu"){OpenTime = 0, CloseTime = 24 },

            new Hotel(new Vector3(-1356.452f, -791.2153f, 20.24218f), 129.4868f, "Hedera", "Climb to new heights","ExpensiveHotelMenu") {OpenTime = 0, CloseTime = 24 },//needs zoom out
            new Hotel(new Vector3(-2007.835f, -314.862f, 32.09708f), 46.05545f, "The Jetty", "Stick out","ExpensiveHotelMenu") {OpenTime = 0, CloseTime = 24 },//needs zoome out
            new Hotel(new Vector3(-823.0718f, -1223.552f, 7.365416f), 54.09635f, "The Viceroy", "Demand a great experience","ViceroyMenu"){BannerImagePath = "viceroy.png",OpenTime = 0, CloseTime = 24,
                CameraPosition = new Vector3(-847.939f, -1207.791f, 7.15155f), CameraDirection = new Vector3(0.9588153f, -0.1468293f, 0.2431342f), CameraRotation = new Rotator(14.0716f, 0f, -98.70642f) 
            
            ,HotelRooms = new List<HotelRoom>() { 
                new HotelRoom("Test1", new Vector3(-816.1378f, -1190.457f, 47.3128f), new Vector3(-0.5813852f, -0.7932132f, -0.1811189f), new Rotator(-10.43494f, -5.642855E-06f, 143.7605f)),
                new HotelRoom("Test1", new Vector3(-808.8917f, -1196.301f, 40.57087f), new Vector3(-0.5952156f, -0.7941399f, -0.1227201f), new Rotator(-7.049112f, -7.097279E-06f, 143.1481f)),
                new HotelRoom("Test1", new Vector3(-754.9518f, -1220.714f, 31.45628f), new Vector3(0.7947346f, -0.5947848f, -0.1209462f), new Rotator(-6.946714f, 1.720175E-06f, -126.8114f)),
                new HotelRoom("Test1", new Vector3(-778.7416f, -1187.476f, 25.2662f), new Vector3(0.6681148f, 0.73847f, -0.09102001f), new Rotator(-5.22229f, -2.143331E-07f, -42.13654f)),

            }
            
            
            },//needs zoome out

            new Hotel(new Vector3(313.3858f, -225.0208f, 54.22117f), 160.1122f, "Pink Cage", "","CheapHotelMenu"){OpenTime = 0, CloseTime = 24 },
            new Hotel(new Vector3(307.3867f, -727.7486f, 29.31678f), 254.8814f, "Alesandro", "","CheapHotelMenu"){OpenTime = 0, CloseTime = 24, FullName = "The Alesandro Hotel" },
            new Hotel(new Vector3(-702.4747f, -2274.476f, 13.45538f), 225.7683f, "Opium Nights", "Don't your head","ExpensiveHotelMenu") {OpenTime = 0, CloseTime = 24 },
            new Hotel(new Vector3(379.4438f, -1781.435f, 29.46008f), 47.01642f, "Motel & Beauty", "","CheapHotelMenu"){OpenTime = 0, CloseTime = 24 },
            new Hotel(new Vector3(570.0554f, -1745.989f, 29.22319f), 260.0757f, "Billings Gate Motel", "","CheapHotelMenu"){OpenTime = 0, CloseTime = 24 },
            new Hotel(new Vector3(-104.5376f, 6315.921f, 31.57622f), 141.414f, "Dream View Motel", "Mostly Bug Free!","CheapHotelMenu"){OpenTime = 0, CloseTime = 24 },
            new Hotel(new Vector3(317.7083f, 2623.256f, 44.46722f), 306.9629f, "Eastern Motel", "","CheapHotelMenu"){OpenTime = 0, CloseTime = 24, ScannerFilePath = "01_specific_location\\0x0B4EB13E.wav" },
            new Hotel(new Vector3(1142.035f, 2664.177f, 38.16088f), 86.68575f, "The Motor Motel", "Motor on in","CheapHotelMenu"){OpenTime = 0, CloseTime = 24 },
            new Hotel(new Vector3(-477.0448f, 217.5538f, 83.70456f), 355.1573f, "The Generic Hotel", "","ExpensiveHotelMenu"){OpenTime = 0, CloseTime = 24 },
            new Hotel(new Vector3(-309.8708f, 221.5867f, 87.92822f), 6.029551f, "Pegasus", "","ExpensiveHotelMenu") { OpenTime = 0, CloseTime = 24 ,CameraPosition = new Vector3(-347.6738f, 229.8998f, 98.77297f), CameraDirection = new Vector3(0.9052147f, -0.3525268f, -0.2372999f), CameraRotation = new Rotator(-13.72723f, -5.712704E-06f, -111.2779f) },
            new Hotel(new Vector3(-60.74598f, 360.7194f, 113.0564f), 243.5531f, "Gentry Manor", "","ExpensiveHotelMenu"){ OpenTime = 0, CloseTime = 24, FullName = "The Gentry Manor Hotel", ScannerFilePath = "01_specific_location\\0x08E0DB6C.wav" ,CameraPosition = new Vector3(40.25754f, 259.9668f, 126.4436f), CameraDirection = new Vector3(-0.6868073f, 0.7189223f, -0.1069881f), CameraRotation = new Rotator(-6.141719f, -9.445725E-06f, 43.69126f) },
            new Hotel(new Vector3(-1273.729f, 316.0054f, 65.51177f), 152.4087f, "The Richman", "","ExpensiveHotelMenu"){OpenTime = 0, CloseTime = 24, FullName = "The Richman" },
            new Hotel(new Vector3(286.5596f, -936.6477f, 29.46787f), 138.6224f, "Elkridge Hotel", "","ExpensiveHotelMenu") {OpenTime = 0, CloseTime = 24 ,CameraPosition = new Vector3(257.891f, -952.2925f, 43.25403f), CameraDirection = new Vector3(0.8390263f, 0.4567426f, -0.2956704f), CameraRotation = new Rotator(-17.19774f, -1.072479E-05f, -61.43736f) },
            
            new Hotel(new Vector3(329.0126f, -69.0122f, 73.03772f), 158.678f, "Vinewood Gardens", "","ExpensiveHotelMenu"){OpenTime = 0, CloseTime = 24 },
            new Hotel(new Vector3(63.68047f, -261.8232f, 52.35384f), 335.7221f, "Cheep Motel", "POOL!","CheapHotelMenu"){OpenTime = 0, CloseTime = 24 },

        };
        List<Residence> Residences = new List<Residence>()
        {
            //NEW
            //Vespucci
            new Residence(new Vector3(-1035.18f, -1146.53f, 2.16f), 32.45f, "68 Vespucci Canals", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 450000, RentalDays = 28, RentalFee = 2400  },
            //new Residence(new Vector3(-1061.66f, -1099.24f, 2.19f), 31.68f, "23 Vespucci Canals", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 380000, RentalDays = 28, RentalFee = 1660  },//is a houseboat?
            new Residence(new Vector3(-960.14f, -1109.21f, 2.15f), 33.94f, "87 Vespucci Canals", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 380000, RentalDays = 28, RentalFee = 2000  },
            new Residence(new Vector3(-951.51f, -1078.93f, 2.15f), 207.97f, "88 Vespucci Canals", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 490000, RentalDays = 28, RentalFee = 2500  },
            new Residence(new Vector3(-1183.19f, -1064.55f, 2.15f), 114.64f, "42 Vespucci Canals", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 400000, RentalDays = 28, RentalFee = 2000  },
            new Residence(new Vector3(-983.25f, -1065.88f, 2.15f), 30.08f, "86 Vespucci Canals", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 440000, RentalDays = 28, RentalFee = 2300  },
            new Residence(new Vector3(-1064.27f, -1159.15f, 2.16f), 31.65f, "65 Vespucci Canals", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 400000, RentalDays = 28, RentalFee = 2000  },
            

            //Mirror Park
            new Residence(new Vector3(987.9448f, -525.9558f, 60.69062f), 208.7025f, "114 Nikola Ave", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 390000, RentalDays = 28, RentalFee = 2000  },
            new Residence(new Vector3(976.0329f, -579.8205f, 59.63558f), 30.60654f, "115 Nikola Ave", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 360000, RentalDays = 28, RentalFee = 2000  },
            new Residence(new Vector3(1323.468f, -583.0522f, 73.24638f), 334.7151f, "104 East Mirror Dr", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 480000, RentalDays = 28, RentalFee = 2500  },
            new Residence(new Vector3(1372.544f, -555.1687f, 74.68565f), 153.1011f, "107 East Mirror Dr", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 500000, RentalDays = 28, RentalFee = 2600  },
            new Residence(new Vector3(999.5336f, -593.9052f, 59.63857f), 259.7758f, "93 Mirror Pl", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 270000, RentalDays = 28, RentalFee = 1500  },
            new Residence(new Vector3(979.8713f, -627.104f, 59.23589f), 124.2795f, "218 West Mirror Dr", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 400000, RentalDays = 28, RentalFee = 3000  },

            new Residence(new Vector3(1060.462f, -378.1198f, 68.23125f), 229.2095f, "112 Bridge St", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 350000, RentalDays = 28, RentalFee = 2000  },
            new Residence(new Vector3(1051.598f, -470.5955f, 63.91349f), 257.9751f, "118 Bridge St", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 335000, RentalDays = 28, RentalFee = 1700  },
            new Residence(new Vector3(1090.538f, -484.3305f, 65.66045f), 76.35575f, "117 Bridge St", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 400000, RentalDays = 28, RentalFee = 3000  },



            //Apartments
            new Residence(new Vector3(-1150.072f, -1521.705f, 10.62806f), 225.8192f, "7611 Goma St", "") { OpenTime = 0,CloseTime = 24, InteriorID = 24578, PurchasePrice = 550000, RentalDays = 28, RentalFee = 2250 },
            //new Residence(new Vector3(-1221.032f, -1232.806f, 11.02771f), 12.79515f, "Del Pierro Apartments", "") {OpenTime = 0,CloseTime = 24, InteriorID = -1, TeleportEnterPosition = new Vector3(266.1081f, -1007.534f, -101.0086f), TeleportEnterHeading = 358.3953f},
            
           


            //House
            new Residence(new Vector3(983.7811f, 2718.655f, 39.50342f), 175.7726f, "345 Route 68", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 234000, RentalDays = 28, RentalFee = 1400  },
            new Residence(new Vector3(980.1745f, 2666.563f, 40.04342f), 1.699184f, "140 Route 68", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 225000, RentalDays = 28, RentalFee = 1250  },
            new Residence(new Vector3(-3030.045f, 568.7484f, 7.823583f), 280.3508f, "566 Ineseno Rd", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 556000, RentalDays = 28, RentalFee = 2200 },
            new Residence(new Vector3(-3031.865f, 525.1087f, 7.424246f), 267.6694f, "800 Ineseno Rd", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 467000, RentalDays = 28, RentalFee = 1850  },
            new Residence(new Vector3(-3039.567f, 492.8512f, 6.772703f), 226.448f, "805 Ineseno Rd", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 450000, RentalDays = 28, RentalFee = 1750  },
            new Residence(new Vector3(195.0935f, 3031.064f, 43.89068f), 273.1078f, "125 Joshua Rd", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 298000, RentalDays = 28, RentalFee = 1580  },


            new Residence(new Vector3(191.0053f, 3082.26f, 43.47285f), 277.6117f, "610N Joshua Rd", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 289, RentalDays = 28, RentalFee = 1675  },
            new Residence(new Vector3(241.7666f, 3107.666f, 42.48719f), 93.76467f, "620N Joshua Rd", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 275000, RentalDays = 28, RentalFee = 1610  },
            new Residence(new Vector3(162.8214f, 3119.749f, 43.42594f), 192.0786f, "621N Joshua Rd", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 57000, RentalDays = 28, RentalFee = 850  },
            new Residence(new Vector3(247.5913f, 3169.535f, 42.78756f), 90.61945f, "630N Joshua Rd", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 267000, RentalDays = 28, RentalFee = 1580  },



           // new Residence(new Vector3(-214.8853f, 6444.098f, 31.31351f), 315.979f, "1280N Procopio Drive", "Paleto Bay, nice almost beach house 450K") {OpenTime = 0,CloseTime = 24, PurchasePrice = 460000, RentalDays = 28, RentalFee = 1700  },//used as lupi savehouse? in paleto
            new Residence(new Vector3(-272.7155f, 6400.906f, 31.50496f), 215.1084f, "1275N Procopio Dr", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 470000, RentalDays = 28, RentalFee = 1850  },
            new Residence(new Vector3(-247.7424f, 6370.079f, 31.84554f), 45.0573f, "1276N Procopio Dr", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 476000, RentalDays = 28, RentalFee = 1790  },
            new Residence(new Vector3(-227.3544f, 6377.188f, 31.75924f), 47.93699f, "1278N Procopio Dr", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 414000, RentalDays = 28, RentalFee = 1670  },
            new Residence(new Vector3(-245.4691f, 6413.878f, 31.4606f), 130.3203f, "1281N Procopio Dr", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 455000, RentalDays = 28, RentalFee = 1770  },
            new Residence(new Vector3(-213.7807f, 6395.94f, 33.08509f), 40.99633f, "1282N Procopio Dr", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 435000, RentalDays = 28, RentalFee = 1680  },
            new Residence(new Vector3(-189.0498f, 6409.355f, 32.29676f), 48.98f, "1285N Procopio Dr", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 445000, RentalDays = 28, RentalFee = 1650  },
            new Residence(new Vector3(-280.4388f, 6350.718f, 32.60079f), 24.08426f, "1271N Procopio Dr", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 247000, RentalDays = 28, RentalFee = 1380  },
            new Residence(new Vector3(-359.5872f, 6334.424f, 29.84736f), 226.0376f, "1260N Procopio Dr", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 565000, RentalDays = 28, RentalFee = 2200  },
            new Residence(new Vector3(-332.7523f, 6301.959f, 33.08874f), 69.82259f, "1262N Procopio Dr", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 257000, RentalDays = 28, RentalFee = 1480  },
            new Residence(new Vector3(-407.2561f, 6314.223f, 28.94128f), 230.5133f, "1259N Procopio Dr", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 414000, RentalDays = 28, RentalFee = 1870  },
            new Residence(new Vector3(-437.4564f, 6261.807f, 30.06895f), 228.8042f, "1252N Procopio Dr", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 520000, RentalDays = 28, RentalFee = 2100  },

            new Residence(new Vector3(1880.423f, 3921.08f, 33.21722f), 100.0703f, "785N Niland Ave", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 224000, RentalDays = 28, RentalFee = 1280  },
            new Residence(new Vector3(-37.54279f, 170.3245f, 95.35922f), 303.8834f, "Elgin House Apartment 23E", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 404000, RentalDays = 28, RentalFee = 1650  },
            new Residence(new Vector3(9.063264f, 52.93087f, 71.64354f), 344.3057f, "0605 Apartment 4F", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 375000, RentalDays = 28, RentalFee = 1570  },
            new Residence(new Vector3(76.05615f, -86.96131f, 63.00647f), 249.8842f, "1144 Apartment 2B", "") {OpenTime = 0,CloseTime = 24, PurchasePrice = 380000, RentalDays = 28, RentalFee = 1660  },







new Residence(new Vector3(1683.038f, 4689.635f, 43.06602f), 269.7168f, "330E Grapeseed", ""){ OpenTime = 0,CloseTime = 24,PurchasePrice = 200000, RentalFee = 1200, RentalDays = 28,},
new Residence(new Vector3(1674.201f, 4658.228f, 43.37114f), 270.4372f, "335E Grapeseed", ""){ OpenTime = 0,CloseTime = 24,PurchasePrice = 200000, RentalFee = 1200, RentalDays = 28,},
new Residence(new Vector3(1725.289f, 4642.35f, 43.87548f), 115.7792f, "340E Grapeseed", ""){ OpenTime = 0,CloseTime = 24,PurchasePrice = 250000, RentalFee = 1350, RentalDays = 28,},
new Residence(new Vector3(1881.219f, 3810.713f, 32.77882f), 301.1151f, "370E Niland ", ""){ OpenTime = 0,CloseTime = 24,PurchasePrice = 150000, RentalFee = 950, RentalDays = 28,},
new Residence(new Vector3(1899.071f, 3781.417f, 32.8769f), 299.5774f, "380E Niland", ""){ OpenTime = 0,CloseTime = 24,PurchasePrice = 175000, RentalFee = 1000, RentalDays = 28,},
new Residence(new Vector3(-3186.93f, 1274.122f, 12.66124f), 257.7415f, "630W Barbareno", ""){ OpenTime = 0,CloseTime = 24,PurchasePrice = 750000, RentalFee = 3000, RentalDays = 28,},
new Residence(new Vector3(-3194.785f, 1179.539f, 9.659509f), 258.9255f, "632W Barbareno", ""){ OpenTime = 0,CloseTime = 24,PurchasePrice = 650000, RentalFee = 2700, RentalDays = 28,},
new Residence(new Vector3(-3238.444f, 952.556f, 13.34334f), 281.2452f, "642W Barbareno", ""){ OpenTime = 0,CloseTime = 24,PurchasePrice = 670000, RentalFee = 2800, RentalDays = 28,},
new Residence(new Vector3(-3093.812f, 349.4576f, 7.544847f), 258.988f, "611W Ineseno", ""){ OpenTime = 0,CloseTime = 24,PurchasePrice = 1200000, RentalFee = 4000, RentalDays = 28,},
new Residence(new Vector3(312.721f, -1956.719f, 24.42436f), 229.223f, "61E Jamestown ", ""){ OpenTime = 0,CloseTime = 24,PurchasePrice = 330000, RentalFee = 1400, RentalDays = 28,},
new Residence(new Vector3(256.5888f, -2023.559f, 19.26631f), 227.9232f, "50E Jamestown", ""){ OpenTime = 0,CloseTime = 24,PurchasePrice = 400000, RentalFee = 1800, RentalDays = 28,},
new Residence(new Vector3(165.2183f, -1944.738f, 20.23543f), 228.8544f, "30E Roy Lowenstein", ""){ OpenTime = 0,CloseTime = 24,PurchasePrice = 425000, RentalFee = 1870, RentalDays = 28,},
new Residence(new Vector3(150.2098f, -1864.915f, 24.59134f), 153.5939f, "20E Covenant", ""){OpenTime = 0,CloseTime = 24, PurchasePrice = 250000, RentalFee = 1300, RentalDays = 28,},
new Residence(new Vector3(114.7368f, -1887.42f, 23.92823f), 333.4948f, "21E Covenant", ""){ OpenTime = 0,CloseTime = 24,PurchasePrice = 275000, RentalFee = 1000, RentalDays = 28,},
new Residence(new Vector3(54.01227f, -1873.436f, 22.80583f), 128.9813f, "10E Grove Street", ""){OpenTime = 0,CloseTime = 24, PurchasePrice = 225000, RentalFee = 1200, RentalDays = 28,},
new Residence(new Vector3(-5.184085f, -1871.247f, 24.15101f), 317.3572f, "3E Grove St", ""){OpenTime = 0,CloseTime = 24, PurchasePrice = 325000, RentalFee = 1500, RentalDays = 28,},
new Residence(new Vector3(-131.3305f, -1665.597f, 32.56437f), 316.4919f, "20W Strawberry Apt 2", ""){ OpenTime = 0,CloseTime = 24,RentalFee = 1200, RentalDays = 28,},
new Residence(new Vector3(-114.5114f, -1660.003f, 32.56433f), 52.34958f, "20W Strawberry Apt 1", ""){ OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},



new Residence(new Vector3(-64.19335f, -1450.097f, 32.52492f), 188.7294f, "280S Forum Dr No 1",""){ OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},//lamar house
new Residence(new Vector3(863.4708f, -1585.503f, 31.4171f), 89.42107f, "310S Popular Street",""){ OpenTime = 0,CloseTime = 24,RentalFee = 950, RentalDays = 28,},//lamar house

new Residence(new Vector3(-390.3866f, -187.2812f, 37.3177f), 207.2874f, "70W Carcer Way Apt 343",""){ OpenTime = 0,CloseTime = 24,RentalFee = 1950, RentalDays = 28,},

        };
        List<CityHall> CityHalls = new List<CityHall>()
        {
            new CityHall(new Vector3(-609.1187f, -87.8642f, 42.93483f), 247.5173f, "Rockford Hills City Hall", "") { OpenTime = 9,CloseTime = 18, CameraPosition = new Vector3(-591.665f, -98.11681f, 51.31879f), CameraDirection = new Vector3(-0.9335647f, -0.06657825f, -0.3521709f), CameraRotation = new Rotator(-20.62015f, -7.411738E-06f, 94.07921f)},
            new CityHall(new Vector3(233.2825f, -411.1329f, 48.11194f), 338.9129f, "Los Santos City Hall", "") { OpenTime = 9,CloseTime = 18, CameraPosition = new Vector3(236.4192f, -348.1718f, 79.68157f), CameraDirection = new Vector3(0.0002627021f, -0.9024973f, -0.4306954f), CameraRotation = new Rotator(-25.5117f, 6.462261E-07f, -179.9833f)},


            new CityHall(new Vector3(329.4892f, -1580.714f, 32.79719f), 135.1704f, "Davis City Hall", "") { OpenTime = 9,CloseTime = 18, CameraPosition = new Vector3(273.4156f, -1601.353f, 42.17516f), CameraDirection = new Vector3(0.9521951f, 0.2629484f, -0.1555077f), CameraRotation = new Rotator(-8.946239f, -9.507168E-06f, -74.56252f)},
            new CityHall(new Vector3(-1286.212f, -566.4031f, 31.7124f), 314.9816f, "Del Perro City Hall", "") { OpenTime = 9,CloseTime = 18, CameraPosition = new Vector3(-1262.947f, -546.7318f, 43.99615f), CameraDirection = new Vector3(-0.7403942f, -0.5856928f, -0.3298186f), CameraRotation = new Rotator(-19.25776f, -1.085254E-05f, 128.3459f)},






        };
        List<PoliceStation> PoliceStations = new List<PoliceStation>()
        {
            new PoliceStation(new Vector3(358.9726f, -1582.881f, 29.29195f), 323.5287f, "Davis Sheriff's Station","A Tradition of Suppression") {OpenTime = 0,CloseTime = 24, 
                PossiblePedSpawns = new List<ConditionalLocation>() {
                new ConditionalLocation(new Vector3(358.3576f, -1581.505f, 29.29195f), 321.0721f, 50f),
                new ConditionalLocation(new Vector3(370.2834f, -1579.717f, 29.29238f), 303.5159f, 50f),
                new ConditionalLocation(new Vector3(363.9216f, -1575.142f, 29.27452f), 350.0409f, 50f),
            },PossibleVehicleSpawns = new List<ConditionalLocation>() {
                new ConditionalLocation(new Vector3(388.9854f, -1612.977f, 29.21355f), 50f,50f),
                new ConditionalLocation(new Vector3(392.7548f, -1608.376f, 29.21355f), 50f,50f),
                new ConditionalLocation(new Vector3(399.393f, -1621.396f, 29.20119f), 50f,50f),
                new ConditionalLocation(new Vector3(351.3006f, -1556.711f, 29.24393f), 230f,50f),
            }
                },

            new PoliceStation(new Vector3(1858.19f, 3679.873f, 33.75724f), 218.3256f,  "Sandy Shores Sheriff's Station","A Tradition of Suppression") {OpenTime = 0,CloseTime = 24, PossiblePedSpawns = new List<ConditionalLocation>() {
                new ConditionalLocation(new Vector3(1851.615f, 3679.759f, 34.26711f), 167.1253f, 50f),
                new ConditionalLocation(new Vector3(1866.224f, 3684.685f, 33.78798f), 229.2713f, 50f),
            },PossibleVehicleSpawns = new List<ConditionalLocation>() {
                new ConditionalLocation(new Vector3(1831.629f, 3662.91f, 33.92607f), 32f,50f),
                new ConditionalLocation(new Vector3(1835.127f, 3664.892f, 33.92607f), 32f,50f),
                new ConditionalLocation(new Vector3(1847.131f, 3672.587f, 33.92607f), 32f,50f),
            } },

            new PoliceStation(new Vector3(-437.973f, 6021.403f, 31.49011f), 316.3756f, "Paleto Bay Sheriff's Office","A Tradition of Suppression") {OpenTime = 0,CloseTime = 24 ,InteriorID = 3842, ScannerFilePath = "01_specific_location\\0x0E94FE38.wav", PossiblePedSpawns = new List<ConditionalLocation>() {
                new ConditionalLocation(new Vector3(-438.606f, 6021.66f, 31.49011f), 358.9023f, 30f),
                new ConditionalLocation(new Vector3(-448.8453f, 6011.864f, 31.71639f), 310.0714f, 30f),
                new ConditionalLocation(new Vector3(-444.1118f, 6011.967f, 31.71639f), 359.0966f, 30f),
                new ConditionalLocation(new Vector3(-454.7975f, 6007.629f, 31.49011f), 131.4658f, 30f),
            },PossibleVehicleSpawns = new List<ConditionalLocation>() {
                new ConditionalLocation(new Vector3(-454.4307f, 6040.472f, 31.17424f), 317f,50f),
                new ConditionalLocation(new Vector3(-457.8087f, 6043.849f, 31.17424f), 317f,50f),
                new ConditionalLocation(new Vector3(-468.1928f, 6038.506f, 31.17422f), 45f,50f),
                new ConditionalLocation(new Vector3(-475.1396f, 6031.421f, 31.17419f), 45f,50f),
                new ConditionalLocation(new Vector3(-458.788f, 6005.529f, 31.17422f), 267f,50f),
                new ConditionalLocation(new Vector3(-455.154f, 6001.894f, 31.17422f), 267f,50f),
            }},

            new PoliceStation(new Vector3(440.0835f, -982.3911f, 30.68966f), 47.88088f, "Mission Row Police Station","Obey and Survive") {OpenTime = 0,CloseTime = 24, ScannerFilePath = "01_specific_location\\0x0A45FA8A.wav", PossiblePedSpawns = new List<ConditionalLocation>() {
                new ConditionalLocation(new Vector3(427.7032f, -982.3438f, 30.7101f), 46.43887f, 80f),
                new ConditionalLocation(new Vector3(432.3652f, -973.1894f, 30.71074f), 60.75554f, 80f),
                new ConditionalLocation(new Vector3(441.3313f, -977.6323f, 30.68966f), 167.8129f, 80f),


                new ConditionalLocation(new Vector3(426.1106f, -1003.349f, 30.71002f), 159.3153f, 80f),
                new ConditionalLocation(new Vector3(480.1246f, -974.9543f, 27.98389f), 332.2961f, 80f),
            },PossibleVehicleSpawns = new List<ConditionalLocation>() {
                new ConditionalLocation(new Vector3(407.7554f, -984.2084f, 29.89806f), 228f,50f),
                new ConditionalLocation(new Vector3(408.0923f, -988.8488f, 29.85523f), 228f,50f),
                new ConditionalLocation(new Vector3(407.9883f, -998.3094f, 29.81112f), 228f,50f),
                new ConditionalLocation(new Vector3(427.5909f, -1027.707f, 29.22805f), 185f,50f),
                new ConditionalLocation(new Vector3(434.9848f, -1027.103f, 29.12844f), 185f,50f),
                new ConditionalLocation(new Vector3(442.5143f, -1026.687f, 28.98147f), 185f,50f),
                new ConditionalLocation(new Vector3(446.3985f, -1026.087f, 28.92508f), 185f,50f),
                new ConditionalLocation(new Vector3(496.3687f, -996.015f, 28.3387f), 178.4318f,50f),
                new ConditionalLocation(new Vector3(496.3721f, -1016.798f, 28.687f), 178.4318f,50f),
            } },

            new PoliceStation(new Vector3(815.8774f, -1290.531f, 26.28391f), 74.91704f, "La Mesa Police Station","Obey and Survive") {OpenTime = 0,CloseTime = 24, PossiblePedSpawns = new List<ConditionalLocation>() {
                new ConditionalLocation(new Vector3(823.9117f, -1288.765f, 28.24066f), 74.91795f, 30f),
                new ConditionalLocation(new Vector3(823.3896f, -1291.954f, 28.24066f), 102.1528f, 30f),
                new ConditionalLocation(new Vector3(821.5445f, -1275.796f, 26.38955f), 61.38755f, 30f),
                new ConditionalLocation(new Vector3(843.4933f, -1314.606f, 26.25885f), 126.1716f, 30f),

            },PossibleVehicleSpawns = new List<ConditionalLocation>() {
                new ConditionalLocation(new Vector3(822.8635f, -1258.039f, 26.34347f), 0f,50f),
                new ConditionalLocation(new Vector3(828.0373f, -1258.039f, 26.34347f), 0f,50f),
                new ConditionalLocation(new Vector3(833.577f, -1258.954f, 26.34347f), 180f,50f),
                new ConditionalLocation(new Vector3(838.7137f, -1271.55f, 26.34347f), 180f,50f),
                new ConditionalLocation(new Vector3(833.4279f, -1271.55f, 26.34347f), 0f,50f),
                new ConditionalLocation(new Vector3(822.5837f, -1271.55f, 26.34347f), 0f,50f),
                new ConditionalLocation(new Vector3(828.1719f, -1333.792f, 26.18776f), 242f,50f),
                new ConditionalLocation(new Vector3(828.1719f, -1339.649f, 26.18776f), 242f,50f),
                new ConditionalLocation(new Vector3(828.1719f, -1345.815f, 26.18776f), 242f,50f),
                new ConditionalLocation(new Vector3(828.362f, -1351.482f, 26.21234f), 65f,50f),
                new ConditionalLocation(new Vector3(843.9627f, -1334.354f, 26.17253f), 65f,50f),
                new ConditionalLocation(new Vector3(843.874f, -1340.518f, 26.18776f), 242f,50f),
                new ConditionalLocation(new Vector3(844.3544f, -1346.3f, 26.21234f), 65f,50f),
                new ConditionalLocation(new Vector3(843.7897f, -1352.283f, 26.21234f), 65f,50f),
                new ConditionalLocation(new Vector3(865.7624f, -1378.407f, 26.21234f), 216f,50f),
                new ConditionalLocation(new Vector3(862.8425f, -1383.55f, 26.21234f), 216f,50f),
                new ConditionalLocation(new Vector3(859.8381f, -1388.58f, 26.21234f), 216f,50f),
                new ConditionalLocation(new Vector3(857.2202f, -1393.802f, 26.21234f), 216f,50f),
                new ConditionalLocation(new Vector3(854.2248f, -1398.952f, 26.21234f), 216f,50f),
            } },

            new PoliceStation(new Vector3(642.1356f, -3.134667f, 82.78872f), 215.299f,"Vinewood Police Station","Obey and Survive") {OpenTime = 0,CloseTime = 24, PossiblePedSpawns = new List<ConditionalLocation>() {
                new ConditionalLocation(new Vector3(641.4031f, 0.478787f, 82.78651f), 235.3406f, 50f),
                new ConditionalLocation(new Vector3(647.2952f, -11.50604f, 82.60114f), 223.3047f, 50f),
                new ConditionalLocation(new Vector3(624.8208f, 20.23717f, 87.97021f), 345.2158f, 50f),
            },PossibleVehicleSpawns = new List<ConditionalLocation>() {
                new ConditionalLocation(new Vector3(666.4646f, -11.79645f, 82.76681f), 144f,50f),
                new ConditionalLocation(new Vector3(621.6934f, 26.27448f, 88.66011f), 193f,50f),
                new ConditionalLocation(new Vector3(615.3292f, 28.48364f, 89.28476f), 193f,50f),
                new ConditionalLocation(new Vector3(609.7616f, 30.84756f, 89.91243f), 193f,50f),
                new ConditionalLocation(new Vector3(597.8726f, 34.8121f, 91.07567f), 193f,50f),
                new ConditionalLocation(new Vector3(586.7384f, 37.78894f, 92.30818f), 193f,50f),
                new ConditionalLocation(new Vector3(580.8351f, 38.85468f, 92.82274f), 193f,50f),
            } },

            new PoliceStation(new Vector3(-557.0687f, -134.7315f, 38.20231f), 214.5968f, "Rockford Hills Police Station","Obey and Survive") {OpenTime = 0,CloseTime = 24, PossiblePedSpawns = new List<ConditionalLocation>() {
                new ConditionalLocation(new Vector3(-560.6538f, -134.251f, 38.11069f), 180.7514f, 50f),
                new ConditionalLocation(new Vector3(-563.0671f, -141.727f, 38.32593f), 192.5406f, 50f),
                new ConditionalLocation(new Vector3(-541.8491f, -134.7859f, 38.55503f), 208.2207f, 50f),

            },PossibleVehicleSpawns = new List<ConditionalLocation>() {
                new ConditionalLocation(new Vector3(-574.256f, -168.7862f, 38.49216f), 291f,50f),
                new ConditionalLocation(new Vector3(-557.7739f, -161.9565f, 38.62593f), 291f,50f),
                new ConditionalLocation(new Vector3(-559.7783f, -147.1107f, 38.65532f), 58f,50f),
                new ConditionalLocation(new Vector3(-555.6528f, -145.4567f, 38.72502f), 58f,50f),
                new ConditionalLocation(new Vector3(-551.0397f, -144.0132f, 38.65663f), 58f,50f),
            } },

            new PoliceStation(new Vector3(-1093.817f, -807.1993f, 19.28864f), 22.23846f, "Vespucci Police Station","Obey and Survive") {OpenTime = 0,CloseTime = 24, PossiblePedSpawns = new List<ConditionalLocation>() {
                new ConditionalLocation(new Vector3(-1114.355f, -822.5755f, 19.3168f), 14.61082f, 50f),
                new ConditionalLocation(new Vector3(-1135.939f, -847.7239f, 19.34725f), 31.25465f, 50f),
                new ConditionalLocation(new Vector3(-1056.813f, -820.9451f, 19.23504f), 300.4652f, 50f),

            },PossibleVehicleSpawns = new List<ConditionalLocation>() {
                new ConditionalLocation(new Vector3(-1072.822f, -880.3561f, 4.809089f), 208f,50f),
                new ConditionalLocation(new Vector3(-1075.95f, -882.3492f, 4.809089f), 208f,50f),
                new ConditionalLocation(new Vector3(-1051.726f, -867.1277f, 4.809089f), 230f,50f),
                new ConditionalLocation(new Vector3(-1045.53f, -861.5321f, 4.809089f), 230f,50f),
                new ConditionalLocation(new Vector3(-1042.226f, -857.9979f, 4.809089f), 230f,50f),
                new ConditionalLocation(new Vector3(-1047.814f, -846.7044f, 4.809089f), 37f,50f),
                new ConditionalLocation(new Vector3(-1052.352f, -846.8544f, 4.809089f), 37f,50f),
                new ConditionalLocation(new Vector3(-1055.076f, -849.5068f, 4.809089f), 37f,50f),
                new ConditionalLocation(new Vector3(-1058.752f, -851.4465f, 4.809089f), 37f,50f),
                new ConditionalLocation(new Vector3(-1126.5f, -864.8307f, 13.63185f), 220f,50f),
                new ConditionalLocation(new Vector3(-1122.896f, -863.4746f, 13.6122f), 220f,50f),
                new ConditionalLocation(new Vector3(-1115.937f, -857.7859f, 13.65187f), 220f,50f),
                new ConditionalLocation(new Vector3(-1138.613f, -845.7916f, 13.98058f), 220f,50f),
            } },

            new PoliceStation(new Vector3(-1633.314f, -1010.025f, 13.08503f), 351.7007f, "Del Perro Police Station","Obey and Survive") {OpenTime = 0,CloseTime = 24, PossiblePedSpawns = new List<ConditionalLocation>() {
                new ConditionalLocation(new Vector3(-1629.069f, -1013.071f, 13.11924f), 298.982f, 50f),
                new ConditionalLocation(new Vector3(-1639.286f, -1011.673f, 13.12066f), 24.4451f, 50f),

            },PossibleVehicleSpawns = new List<ConditionalLocation>() {
                new ConditionalLocation(new Vector3(-1625.343f, -1013.629f, 13.89048f), 0f,50f),
            } },

            new PoliceStation(new Vector3(-1311.877f, -1528.808f, 4.410581f), 233.9121f, "Vespucci Beach Police Station","Obey and Survive") {OpenTime = 0,CloseTime = 24, ScannerFilePath = "01_specific_location\\0x13CBAB64.wav", PossiblePedSpawns = new List<ConditionalLocation>() {
                new ConditionalLocation(new Vector3(-1314.879f, -1532.083f, 4.423616f), 190.5698f, 50f),
                new ConditionalLocation(new Vector3(-1307.496f, -1526.527f, 4.359179f), 244.9207f, 50f),

            } },

            new PoliceStation(new Vector3(102.9145f, -743.9487f, 45.75473f), 79.8266f, "FIB Headquarters","We're corrupt in a good way") { AssignedAgencyID = "FIB", InteriorID = 58882, OpenTime = 0,CloseTime = 24, ScannerFilePath = "01_specific_location\\0x1667D63F.wav", PossiblePedSpawns = new List<ConditionalLocation>() {
                new ConditionalLocation(new Vector3(101.3534f, -745.8216f, 45.75475f), 76.27069f, 50f),
                new ConditionalLocation(new Vector3(117.8028f, -747.0616f, 45.75158f), 102.5501f, 50f),
                new ConditionalLocation(new Vector3(113.9787f, -758.2271f, 45.75474f), 21.6145f, 50f),
            } },

            new PoliceStation(new Vector3(387.16f, 789.96f, 188.23f), 178f, "Beaver Bush Ranger Station","You won't get this bush!") { AssignedAgencyID = "SAPR", OpenTime = 0,CloseTime = 24, PossiblePedSpawns = new List<ConditionalLocation>() {
                new ConditionalLocation(new Vector3(391.3105f, 789.5917f, 186.6761f), 178f,75f),
                new ConditionalLocation(new Vector3(381.8302f, 789.8931f, 186.6757f), 178f,75f),


            },PossibleVehicleSpawns = new List<ConditionalLocation>()
            { new ConditionalLocation(new Vector3(374.2932f, 795.6959f, 186.5305f), 178f,100f), } },




            new PoliceStation(new Vector3(-893.9146f, -2401.547f, 14.02436f), 148.8757f, "NOoSE LSIA","Let's tighten the noose!") { AssignedAgencyID = "NOOSE", OpenTime = 0,CloseTime = 24, PossiblePedSpawns = new List<ConditionalLocation>() {
                new ConditionalLocation(new Vector3(-897.6145f, -2398.739f, 14.02436f), 117.328f, 75f),
                new ConditionalLocation(new Vector3(-889.1294f, -2405.644f, 14.02639f), 117.7686f, 75f),
                new ConditionalLocation(new Vector3(-868.955f, -2417.757f, 14.02489f), 200.5734f, 75f),


            },PossibleVehicleSpawns = new List<ConditionalLocation>()
            { new ConditionalLocation(new Vector3(-873.713f, -2419.414f, 13.94444f), 53.94463f, 75f), } },



            new PoliceStation(new Vector3(3142.471f, -4840.832f, 112.0291f), 349.9769f, "NYSP Office Ludendorff","The return of the Keystone Cops") { StateLocation = "North Yankton",AssignedAgencyID = "NYSP", OpenTime = 0,CloseTime = 24, PossiblePedSpawns = new List<ConditionalLocation>() {

            },PossibleVehicleSpawns = new List<ConditionalLocation>()
            {  } },


        };
        List<Prison> Prisons = new List<Prison>()
        {
            new Prison(new Vector3(1846.258f, 2586.139f, 45.67202f), 269.2306f, "Bolingbroke Penitentiary","Where the scum of LS washes up")  { OpenTime = 0,CloseTime = 24,AssignedAgencyID = "SASPA",
                PossiblePedSpawns = new List<ConditionalLocation>() {
                new ConditionalLocation(new Vector3(1899.234f, 2605.696f, 44.96621f), 354f, 100f),//guard facing towards entry

                new ConditionalLocation(new Vector3(1830.171f, 2602.584f, 44.88912f), 0f, 100f),//guard facing towards entry second guard booth


                //
                new ConditionalLocation(new Vector3(1846.473f, 2584.199f, 44.67195f), 295f, 75f),//front of prison
                new ConditionalLocation(new Vector3(1845.851f, 2587.203f, 44.67231f), 290f, 75f),
            },PossibleVehicleSpawns = new List<ConditionalLocation>() {

                new ConditionalLocation(new Vector3(1855.314f, 2578.854f, 46.42464f), 92f, 75f),//facing towards prison
                new ConditionalLocation(new Vector3(1869.82f, 2588.359f, 46.42464f), 269f, 75f),//facing away from prison
                 new ConditionalLocation(new Vector3(1854.312f, 2620.756f, 46.42464f), 92f, 75f),//facing towards prison
                 new ConditionalLocation( new Vector3(1869.65f, 2577.781f, 45.18463f), 269f, 75f),//facing away from prison


                

            }
            },
        };
        List<Hospital> Hospitals = new List<Hospital>()
        {
            new Hospital(new Vector3(359.6708f, -584.4868f, 28.81813f), 255.1885f, "Pill Box Hill Medical Center","Some of the most famous people of LS have died here") { OpenTime = 0,CloseTime = 24, InteriorID = -787, PossiblePedSpawns = new List<ConditionalLocation>() {

                new ConditionalLocation(new Vector3(355.4031f, -598.8352f, 28.77448f), 250.9611f, 50f),
                new ConditionalLocation(new Vector3(364.3289f, -579.7377f, 28.84095f), 227.9576f, 50f),
                new ConditionalLocation(new Vector3(299.7511f, -579.8925f, 43.26084f), 72.9278f, 50f),
            }
            ,RespawnLocation = new Vector3(364.7124f, -583.1641f, 28.69318f),RespawnHeading = 280.637f
            ,CameraPosition = new Vector3(396.9802f, -582.7473f, 40.12638f), CameraDirection = new Vector3(-0.948761f, -0.1335131f, -0.2864031f), CameraRotation = new Rotator(-16.64274f, 1.069323E-05f, 98.01027f) },

            new Hospital(new Vector3(343.5177f, -1399.408f, 32.50927f), 49.67379f, "Central LS Medical Center","Gang Related Injury? We take Fleeca and Limit cards") {OpenTime = 0,CloseTime = 24, ScannerFilePath= "01_specific_location\\0x13583D6F.wav", PossiblePedSpawns = new List<ConditionalLocation>() {

                new ConditionalLocation(new Vector3(333.7716f, -1381.868f, 32.50922f), 80.20363f, 50f),
                new ConditionalLocation(new Vector3(327.9682f, -1393.585f, 32.50924f), 23.40009f, 50f),
                new ConditionalLocation(new Vector3(297.9529f, -1448.031f, 29.96662f), 306.945f, 50f),
                new ConditionalLocation(new Vector3(361.8272f, -1465.073f, 29.28194f), 178.0938f, 50f),
            }
            ,RespawnLocation = new Vector3(338.208f, -1396.154f, 32.50927f), RespawnHeading = 77.07102f
            ,CameraPosition = new Vector3(304.5882f, -1362.572f, 45.39756f), CameraDirection = new Vector3(0.2721124f, -0.907807f, -0.3191258f), CameraRotation = new Rotator(-18.61007f, 2.792723E-05f, -163.3141f) },

            new Hospital(new Vector3(1839.211f, 3673.892f, 34.27668f), 215.6462f, "Sandy Shores Medical Center","Just as good as those REAL hospitals in LS") {OpenTime = 0,CloseTime = 24, PossiblePedSpawns = new List<ConditionalLocation>() {

                new ConditionalLocation(new Vector3(1835.39f, 3670.011f, 34.27673f), 172.621f, 50f),
                new ConditionalLocation(new Vector3(1844.322f, 3672.015f, 33.67998f), 196.141f, 50f),
            }
            ,RespawnLocation = new Vector3(1842.057f, 3668.679f, 33.67996f),RespawnHeading = 228.3818f
            ,CameraPosition = new Vector3(1851.532f, 3664.737f, 39.15179f), CameraDirection = new Vector3(-0.7719535f, 0.5178522f, -0.3686691f), CameraRotation = new Rotator(-21.63356f, -3.67388E-06f, 56.14497f) },

            new Hospital(new Vector3(-248.4412f, 6332.721f, 32.42618f), 225.41f, "The Bay Care Center","One stop shopping! Funeral Home and Crematorium on site") {OpenTime = 0,CloseTime = 24, PossiblePedSpawns = new List<ConditionalLocation>() {

                new ConditionalLocation(new Vector3(-242.6317f, 6324.892f, 32.42618f), 318.8682f, 50f),
                new ConditionalLocation(new Vector3(-238.915f, 6314.112f, 31.48628f), 227.0187f, 50f),
            }
            ,RespawnLocation = new Vector3(-244.3214f, 6328.575f, 32.42618f),RespawnHeading = 219.7734f
            ,CameraPosition = new Vector3(-229.6282f, 6315.992f, 36.41383f), CameraDirection = new Vector3(-0.9177954f, 0.3135513f, -0.2435924f), CameraRotation = new Rotator(-14.09866f, -7.922609E-06f, 71.13802f) },

            new Hospital(new Vector3(1151.386f, -1529.34f, 35.36609f), 337.4726f, "St. Fiacre Hospital", "Come see the most expensive specialists in LS") {OpenTime = 0,CloseTime = 24, PossiblePedSpawns = new List<ConditionalLocation>() {

                new ConditionalLocation(new Vector3(1150.269f, -1524.425f, 34.84344f), 316.4077f, 50f),
                new ConditionalLocation(new Vector3(1137.925f, -1514.329f, 34.84341f), 327.5841f, 50f),
                new ConditionalLocation(new Vector3(1171.36f, -1520.041f, 34.8434f), 319.8948f, 50f),
            }
            ,RespawnLocation = new Vector3(1151.386f, -1529.34f, 35.36609f),RespawnHeading = 337.4726f
            ,CameraPosition = new Vector3(1152.484f, -1510.302f, 41.42735f), CameraDirection = new Vector3(-0.1392823f, -0.9661027f, -0.2173616f), CameraRotation = new Rotator(-12.55411f, 2.186715E-07f, 171.7962f) },


            new Hospital(new Vector3(-449.76f, -341.03f, 35.5f), 106f, "Mount Zonah Medical Center", "The hospital with the largest advertising budget") { OpenTime = 0,CloseTime = 24, 
                PossiblePedSpawns = new List<ConditionalLocation>() {
                    new ConditionalLocation(new Vector3(-448.7528f, -334.6505f, 33.5019f), 161.25828389f, 50f),
                    new ConditionalLocation(new Vector3(-447.4306f, -328.7594f, 33.50186f), 190.084495712696f,50f),
                    new ConditionalLocation(new Vector3(-447.0008f, -329.9945f, 33.50186f), 27.4845562493075f,50f),
                    new ConditionalLocation(new Vector3(-496.9766f, -331.2807f, 33.50167f), 223.606079066179f,50f),
                    new ConditionalLocation(new Vector3(-464.4201f, -353.7608f, 33.39479f), 111.44917199877f,50f),
            }
            ,RespawnLocation = new Vector3(-449.76f, -341.03f, 35.5f),RespawnHeading = 106f
            ,CameraPosition = new Vector3(-461.4951f, -347.7999f, 36.37646f), CameraDirection = new Vector3(0.8014859f, 0.59798f, 0.006344283f), CameraRotation = new Rotator(0.3635031f, -9.738551E-07f, -53.27378f) },

            new Hospital(new Vector3(-675.6584f, 313.8624f, 83.08415f), 175.4087f, "Eclipse Medical Tower", "Not for emergencies") { OpenTime = 0,CloseTime = 24,
                PossiblePedSpawns = new List<ConditionalLocation>() {

            }
            ,RespawnLocation = new Vector3(-677.44f, 309.2f, 84.33f),RespawnHeading = 189.027617763797f
            ,CameraPosition = new Vector3(-684.0377f, 287.5522f, 95.29053f), CameraDirection = new Vector3(0.2572458f, 0.9077324f, -0.3314311f), CameraRotation = new Rotator(-19.35566f, 4.072142E-06f, -15.82241f) },

            new Hospital(new Vector3(-874.9105f, -309.6224f, 39.53273f), 348.7842f, "Portola Trinity Medical Center", "Bring Insurance") { OpenTime = 0,CloseTime = 24,
                PossiblePedSpawns = new List<ConditionalLocation>() {

            }
            ,RespawnLocation = new Vector3(-873.8141f, -305.9774f, 39.5518f),RespawnHeading = 345.7174f
            ,CameraPosition = new Vector3(-859.3407f, -294.1456f, 52.37224f), CameraDirection = new Vector3(-0.7405583f, -0.5703712f, -0.3553168f), CameraRotation = new Rotator(-20.81286f, -2.740125E-06f, 127.6031f) },


            new Hospital(new Vector3(3132.073f, -4839.958f, 112.0312f), 354.8388f, "Ludendorff Clinic", "The service you'd expect!") { StateLocation = "North Yankton", OpenTime = 0,CloseTime = 24,
                PossiblePedSpawns = new List<ConditionalLocation>() {
            }
            ,RespawnLocation = new Vector3(3132.073f, -4839.958f, 112.0312f),RespawnHeading = 354.8388f },

            //new Hospital(new Vector3(241.2085f, -1378.962f, 33.74176f), 140.41f, LocationType.Morgue, "Los Santos County Coroner Office", "") {OpenTime = 0,CloseTime = 24, InteriorID = 60418, TeleportEnterPosition = new Vector3(253.351f, -1364.622f, 39.53437f), TeleportEnterHeading = 327.1821f },
        };
        List<FireStation> FireStations = new List<FireStation>()
        {
            new FireStation(new Vector3(1185.842f, -1464.118f, 34.90073f), 356.2903f, "LSCFD Fire Station 7", "") {OpenTime = 0,CloseTime = 24, InteriorID = 81666, PossiblePedSpawns = new List<ConditionalLocation>() {

                new ConditionalLocation(new Vector3(1187.994f, -1462.141f, 34.8952f), 343.8326f, 50f),
                new ConditionalLocation(new Vector3(1209.001f, -1461.391f, 34.8434f), 25.58578f, 50f),
            } },
            new FireStation(new Vector3(213.8019f, -1640.523f, 29.68287f), 319.3789f, "Davis Fire Station", "") {OpenTime = 0,CloseTime = 24, ScannerFilePath = "01_specific_location\\0x0AC416A0.wav", PossiblePedSpawns = new List<ConditionalLocation>() {

                new ConditionalLocation(new Vector3(220.1501f, -1643.144f, 29.59396f), 342.0382f, 50f),
                new ConditionalLocation(new Vector3(200.316f, -1632.709f, 29.80013f), 317.4113f, 50f),
            } },
            new FireStation(new Vector3(-633.0533f, -122.0594f, 39.01375f), 79.69817f, "Rockford Hills Fire Station", "") {OpenTime = 0,CloseTime = 24, ScannerFilePath = "01_specific_location\\0x0A33D1E1.wav", PossiblePedSpawns = new List<ConditionalLocation>() {

                new ConditionalLocation(new Vector3(-636.2214f, -123.7732f, 39.01375f), 42.9723f, 50f),
                new ConditionalLocation(new Vector3(-636.4733f, -117.1641f, 38.02922f), 78.61053f, 50f),
            } },
        };
        List<Restaurant> Restaurants = new List<Restaurant>()
        {
            new Restaurant(new Vector3(-1487.163f, -308.0127f, 47.02639f), 231.5184f, "Las Cuadras Restaurant", "No horsing around!","FancyDeliMenu", FoodType.Mexican),
            new Restaurant(new Vector3(-1473.121f, -329.6028f, 44.81668f), 319.3725f, "Las Cuadras Deli", "No horsing around!","FancyDeliMenu", FoodType.Mexican | FoodType.Sandwiches),
            new Restaurant(new Vector3(-1221.254f, -1095.873f, 8.115647f), 111.3174f, "Prawn Vivant", "Come eat our sea bugs","FancyFishMenu", FoodType.Seafood),
            new Restaurant(new Vector3(-1256.581f, -1079.491f, 8.398257f), 339.5968f, "Surfries Diner", "Eat in or wipe out","FancyDeliMenu", FoodType.American),
            new Restaurant(new Vector3(-1247.504f, -1105.777f, 8.109305f), 289.9685f, "The Split Kipper", "Only the best splits","FancyFishMenu", FoodType.Seafood),
            new Restaurant(new Vector3(-1230.032f, -1174.862f, 7.700727f), 330.9398f, "Pot-Heads", "We know our pots","FancyFishMenu", FoodType.Seafood),
            new Restaurant(new Vector3(-1111.103f, -1454.387f, 5.582287f), 304.9954f, "Coconut Cafe", "We're nuts!","FancyDeliMenu", FoodType.Generic),
            new Restaurant(new Vector3(-1037.587f, -1397.168f, 5.553192f), 76.84702f, "La Spada", "Stab your catch today!","FancyFishMenu", FoodType.Seafood),
            new Restaurant(new Vector3(-1129.253f, -1373.276f, 5.056143f), 164.9213f, "Marlin's Cafe", "Trust the Marlin","CoffeeMenu", FoodType.Coffee),
            new Restaurant(new Vector3(2561.869f, 2590.851f, 38.08311f), 294.6638f, "Rex's Diner", "Do not sit on Rex","FancyDeliMenu", FoodType.American),
            new Restaurant(new Vector3(2697.521f, 4324.264f, 45.98642f), 41.67124f, "Park View Diner", "Real American Food","FancyGenericMenu", FoodType.American),
            new Restaurant(new Vector3(-1389.63f, -744.4225f, 24.62544f), 127.01f, "Haute", "Bringing high-class dining to the lower class","FancyDeliMenu", FoodType.American),
            new Restaurant(new Vector3(-1392.125f, -732.2938f, 24.64698f), 37.13289f, "Les Bianco's", "Come take a whiff","FancyFishMenu", FoodType.Seafood),
            new Restaurant(new Vector3(-1420.425f, -709.2584f, 24.60311f), 126.5269f, "Pescado Rojo", "Overpriced fish served with a scowl","FancyFishMenu", FoodType.Seafood),
            new Restaurant(new Vector3(-1335.517f, -660.6063f, 26.51026f), 212.5534f, "The Fish Net", "Straight from the polluted Los Santos River","FancyFishMenu", FoodType.Seafood) { ScannerFilePath = "01_specific_location\\0x02249C78.wav"},
            new Restaurant(new Vector3(95.79929f, -1682.817f, 29.25364f), 138.4551f, "Yum Fish", "We love fish pie! and our fishermen love to fillet!","FancyFishMenu", FoodType.Seafood),

            new Restaurant(new Vector3(502.6628f, 113.1527f, 96.62571f), 164.3104f, "Jazz Desserts", "Come get your soul food","FancyGenericMenu", FoodType.American),
            new Restaurant(new Vector3(16.29523f, -166.5288f, 55.82795f), 341.2336f, "The Fish Net", "What a catch","FancyFishMenu", FoodType.Seafood) { ScannerFilePath = "01_specific_location\\0x02249C78.wav"},
            new Restaurant(new Vector3(281.4706f, -800.5342f, 29.31682f), 227.4364f, "Pescado Azul", "From factory fish farm to plate","FancyFishMenu", FoodType.Seafood),
            new Restaurant(new Vector3(-630.0808f, -2266.577f, 5.933444f), 242.2394f, "Poppy House", "Don't lose your head","FancyGenericMenu", FoodType.American),
            new Restaurant(new Vector3(-122.6395f, 6389.315f, 32.17757f), 44.91608f, "Mojito Inn", "Famous burgers (and liquor)","FancyGenericMenu", FoodType.American),
            new Restaurant(new Vector3(793.877f, -735.7111f, 27.96293f), 89.91832f, "Casey's Diner", "Who is casey? Don' ask us","FancyDeliMenu", FoodType.American),
            new Restaurant(new Vector3(-361.3964f, 275.7073f, 86.42198f), 212.2804f, "Last Train Diner", "A relic of a bygone era. Like Los Santos","FancyDeliMenu", FoodType.American),
            new Restaurant(new Vector3(-3047.75f, 615.6793f, 7.405877f), 248.6187f, "Mom's Pie Diner", "No comments!","FancyDeliMenu", FoodType.American),
            new Restaurant(new Vector3(-40.46507f, 228.3116f, 107.968f), 78.85908f, "Haute", "Bringing high-class dining to the lower class.","FancyFishMenu", FoodType.Seafood),
            new Restaurant(new Vector3(398.0291f, 175.6789f, 103.8558f), 71.74517f, "Clappers", "Clap On!","FancyGenericMenu", FoodType.American),


            new Restaurant(new Vector3(-1608.83f, -985.9873f, 13.01739f), 51.95325f, "The Big Puffa", "Puffed up flavor","FancyFishMenu", FoodType.Seafood),       
            new Restaurant(new Vector3(-1500.207f, -933.6969f, 10.17892f), 140.8582f, "Dune-O's Beach Cafe", "No bogeys about it","FancyGenericMenu", FoodType.American),
            new Restaurant(new Vector3(-1830.564f, -1180.585f, 14.32305f), 335.8232f, "Pearls Seafood", "Get your pearls from us","FancyFishMenu", FoodType.Seafood),
            new Restaurant(new Vector3(-1693.88f, -1084.884f, 13.15254f), 124.4757f, "Lagoons Diner", "Come on Over to My Plaice","FancyGenericMenu", FoodType.American),
            new Restaurant(new Vector3(-1654.534f, -999.7692f, 13.01745f), 230.1636f, "Out of Towner", "Out of Towners Welcome","FancyGenericMenu", FoodType.American),
            new Restaurant(new Vector3(-661.5396f, -907.5895f, 24.60632f), 278.5222f, "Hwan Cafe", "Coffee with a scowl","CoffeeMenu", FoodType.Coffee),

            //Sandwiches
            new Restaurant(new Vector3(-1249.812f, -296.1564f, 37.35062f), 206.9039f, "Bite!", "Have It Our Way","BiteMenu", FoodType.Sandwiches) {OpenTime = 0, CloseTime = 24,BannerImagePath = "bite.png" },
            new Restaurant(new Vector3(-1539.498f, -427.3804f, 35.59194f), 233.1319f, "Bite!", "Have It Our Way","BiteMenu", FoodType.Sandwiches) {OpenTime = 0, CloseTime = 24,BannerImagePath = "bite.png" },
            new Restaurant(new Vector3(229.5384f, -22.3363f, 74.98735f), 160.0777f, "Bite!", "Have It Our Way","BiteMenu", FoodType.Sandwiches) {OpenTime = 0, CloseTime = 24,BannerImagePath = "bite.png" },
            new Restaurant(new Vector3(-240.7315f, -346.1899f, 30.02782f), 47.8591f, "Bite!", "Have It Our Way","BiteMenu", FoodType.Sandwiches) {OpenTime = 0, CloseTime = 24,BannerImagePath = "bite.png" },
            new Restaurant(new Vector3(-263.1924f, -904.2821f, 32.3108f), 338.4021f, "Bite!", "Have It Our Way","BiteMenu", FoodType.Sandwiches) {OpenTime = 0, CloseTime = 24,BannerImagePath = "bite.png" },
            new Restaurant(new Vector3(385.958f, -1010.523f, 29.41794f), 271.5127f, "Bite!", "Have It Our Way","BiteMenu", FoodType.Sandwiches) {OpenTime = 0, CloseTime = 24,BannerImagePath = "bite.png" },
            new Restaurant(new Vector3(1139.359f, -463.8952f, 66.85857f), 261.1642f, "Bite!", "Have It Our Way","BiteMenu", FoodType.Sandwiches) {OpenTime = 0, CloseTime = 24,BannerImagePath = "bite.png" },
            new Restaurant(new Vector3(100.5837f, 209.4958f, 107.9911f), 342.4262f, "The Pink Sandwich", "The Pink Sandwich","SandwichMenu", FoodType.Sandwiches) {OpenTime = 0, CloseTime = 24 },
            //Asian
            new Restaurant(new Vector3(-798.0056f, -632.0029f, 29.02696f), 169.2606f, "S.Ho", "Become a real S. HO","NoodleMenu", FoodType.Korean) {BannerImagePath ="sho.png", ScannerFilePath ="01_specific_location\\0x1ABB2DE0.wav" },
            new Restaurant(new Vector3(-638.5052f, -1249.646f, 11.81044f), 176.4081f, "S.Ho", "Become a real S. HO","NoodleMenu", FoodType.Korean) {BannerImagePath ="sho.png", ScannerFilePath = "01_specific_location\\0x1ABB2DE0.wav" },
            new Restaurant(new Vector3(-700.9553f, -884.5563f, 23.79126f), 41.62328f, "S.Ho", "Become a real S. HO","NoodleMenu", FoodType.Korean) {BannerImagePath ="sho.png",ScannerFilePath = "01_specific_location\\0x1ABB2DE0.wav" },
            new Restaurant(new Vector3(-1229.61f, -285.7077f, 37.73843f), 205.5755f, "Noodle Exchange", "You Won't Want To Share!","NoodleMenu", FoodType.Korean),
            new Restaurant(new Vector3(-1199.53f, -1162.439f, 7.696731f), 107.0593f, "Noodle Exchange", "You Won't Want To Share!","NoodleMenu", FoodType.Korean),
            new Restaurant(new Vector3(272.8409f, -965.4847f, 29.31605f), 27.34526f, "Noodle Exchange", "You Won't Want To Share!","NoodleMenu", FoodType.Korean),
            new Restaurant(new Vector3(-655.6034f, -880.3672f, 24.67554f), 265.7094f, "Wook Noodle House", "Way better than pancakes","NoodleMenu", FoodType.Korean),
            new Restaurant(new Vector3(-680.4404f, -945.5441f, 20.93157f), 180.6927f, "Wook Noodle House", "Way better than pancakes","NoodleMenu", FoodType.Korean),
            new Restaurant(new Vector3(-654.8373f, -885.7593f, 24.67703f), 273.4168f, "Park Jung Restaurant", "No parking available","GenericMenu", FoodType.Korean),       
            new Restaurant(new Vector3(-163.0659f, -1440.267f, 31.42698f), 55.5593f, "Wok It Off", "Life got you down? Wok It Off!","GenericMenu", FoodType.Korean),
            new Restaurant(new Vector3(1894.635f, 3715.372f, 32.74969f), 119.2431f, "Chinese Food", "As generic as it gets","GenericMenu", FoodType.Chinese),
            //Italian
            new Restaurant(new Vector3(-1182.659f, -1410.577f, 4.499721f), 215.9843f, "Al Dente's", "Just like mama never used to make it","AlDentesMenu", FoodType.Italian | FoodType.Pizza) {BannerImagePath = "aldentes.png" },
            new Restaurant(new Vector3(-213.0357f, -40.15178f, 50.04371f), 157.8173f, "Al Dente's", "Just like mama never used to make it","AlDentesMenu", FoodType.Italian | FoodType.Pizza) {BannerImagePath = "aldentes.png" },
            new Restaurant(new Vector3(-1393.635f, -919.5128f, 11.24511f), 89.35195f, "Al Dente's", "Just like mama never used to make it","AlDentesMenu", FoodType.Italian | FoodType.Pizza) {BannerImagePath = "aldentes.png" },
            new Restaurant(new Vector3(215.2669f, -17.14256f, 74.98737f), 159.7144f, "Pizza This...", "Get stuffed","PizzaThisMenu", FoodType.Italian | FoodType.Pizza) {BannerImagePath = "pizzathis.png" },
            new Restaurant(new Vector3(538.3118f, 101.4798f, 96.52515f), 159.4801f, "Pizza This...", "Get stuffed","PizzaThisMenu", FoodType.Italian | FoodType.Pizza) {BannerImagePath = "pizzathis.png" },
            new Restaurant(new Vector3(287.5003f, -964.0207f, 29.41863f), 357.0406f, "Pizza This...", "Get stuffed","PizzaThisMenu", FoodType.Italian | FoodType.Pizza) {BannerImagePath = "pizzathis.png" },
            new Restaurant(new Vector3(-1529.252f, -908.6689f, 10.16963f), 137.3273f, "Pizza This...", "Get stuffed","PizzaThisMenu", FoodType.Italian | FoodType.Pizza) {BannerImagePath = "pizzathis.png" },
            new Restaurant(new Vector3(443.7377f, 135.1464f, 100.0275f), 161.2897f, "Guidos Takeout 24/7", "For the night owls","PizzaMenu", FoodType.Italian | FoodType.Pizza),
            new Restaurant(new Vector3(-1320.907f, -1318.505f, 4.784881f), 106.5257f, "Pebble Dash Pizza", "Overpriced shitty pizza, but by the beach","PizzaMenu", FoodType.Italian | FoodType.Pizza),
            new Restaurant(new Vector3(-1334.007f, -1282.623f, 4.835985f), 115.3464f, "Slice N Dice Pizza","Slice UP!","PizzaMenu", FoodType.Italian | FoodType.Pizza | FoodType.FastFood),
            new Restaurant(new Vector3(-1296.815f, -1387.3f, 4.544102f), 112.4694f, "Sharkies Bites","Take A Bite Today!","PizzaMenu", FoodType.Italian | FoodType.Pizza | FoodType.FastFood){ IsWalkup = true },
            new Restaurant(new Vector3(-1654.415f, -1037.579f, 13.15282f), 48.0925f, "Sharkies Bites","Take A Bite Today!","PizzaMenu", FoodType.Italian | FoodType.Pizza | FoodType.FastFood){ IsWalkup = true },
            new Restaurant(new Vector3(-1814.037f, -1213.353f, 13.01751f), 43.14338f, "Sharkies Bites","Take A Bite Today!","PizzaMenu", FoodType.Italian | FoodType.Pizza | FoodType.FastFood){ IsWalkup = true },
            new Restaurant(new Vector3(-1342.607f, -872.2929f, 16.87064f), 312.7196f, "Giovanni's Italian", "There are no pocket monsters here","PizzaMenu", FoodType.Italian | FoodType.Pizza),
            //Burger
            new Restaurant(new Vector3(-1535.117f, -454.0615f, 35.92439f), 319.1095f, "Wigwam", "No need for reservations","WigwamMenu", FoodType.Burger | FoodType.FastFood) {BannerImagePath = "wigwam.png"},
            new Restaurant(new Vector3(-860.8414f, -1140.393f, 7.39234f), 171.7175f, "Wigwam", "No need for reservations","WigwamMenu", FoodType.Burger | FoodType.FastFood) { BannerImagePath = "wigwam.png",ScannerFilePath = "01_specific_location\\0x15151AB5.wav" },
            new Restaurant(new Vector3(-1540.86f, -454.866f, 40.51906f), 321.1314f, "Up-N-Atom", "Never Frozen, Often Microwaved","UpNAtomMenu", FoodType.Burger | FoodType.FastFood) {OpenTime = 0, CloseTime = 24,BannerImagePath = "upnatom.png"},
            new Restaurant(new Vector3(81.31124f, 275.1125f, 110.2102f), 162.7602f, "Up-N-Atom", "Never Frozen, Often Microwaved","UpNAtomMenu", FoodType.Burger | FoodType.FastFood) {OpenTime = 0, CloseTime = 24,BannerImagePath = "upnatom.png"},
            new Restaurant(new Vector3(1591.054f, 6451.071f, 25.31714f), 158.0088f, "Up-N-Atom Diner", "Never Frozen, Often Microwaved","UpNAtomMenu", FoodType.Burger | FoodType.FastFood) {OpenTime = 0, CloseTime = 24,BannerImagePath = "upnatom.png", ScannerFilePath = "01_specific_location\\0x035776E6.wav"},
            new Restaurant(new Vector3(-1183.638f, -884.3126f, 13.79987f), 303.1936f, "Burger Shot", "Kill your hunger! It's bleedin' tasty","BurgerShotMenu", FoodType.Burger | FoodType.FastFood) { ScannerFilePath = "01_specific_location\\0x14C89994.wav" },
            new Restaurant(new Vector3(-1687.276f, -1091.789f, 13.15192f), 132.5498f, "Burger Shot", "Kill your hunger! It's bleedin' tasty","BurgerShotMenu", FoodType.Burger | FoodType.FastFood),


            new Restaurant(new Vector3(1241.453f, -366.7538f, 69.08221f), 164.3345f, "Horny's Burgers", "The beef with the bone!","BeefyBillsMenu", FoodType.Burger | FoodType.FastFood),
            new Restaurant(new Vector3(-512.6821f, -683.3517f, 33.18555f), 3.720508f, "Snr. Buns", "Don't be a chump, taste our rump!","GenericMenu", FoodType.Burger | FoodType.FastFood) {ScannerFilePath = "01_specific_location\\0x19532EA2.wav" },
            new Restaurant(new Vector3(-526.9481f, -679.6907f, 33.67113f), 35.17997f, "Snr. Muffin", "Don't be a chump, taste our rump!","GenericMenu", FoodType.Burger | FoodType.FastFood),//??? 
            new Restaurant(new Vector3(125.9558f, -1537.896f, 29.1772f), 142.693f, "La Vaca Loca", "Whats wrong with a few mad cows?","BeefyBillsMenu", FoodType.Burger) {CameraPosition = new Vector3(137.813f, -1561.211f, 37.43506f), CameraDirection = new Vector3(-0.1290266f, 0.9696004f, -0.2079113f), CameraRotation = new Rotator(-11.99998f, -2.182118E-07f, 7.579925f) },
            new Restaurant(new Vector3(-241.8231f, 279.747f, 92.04223f), 177.4421f, "Spitroasters Meathouse", "Come One, Come All","BeefyBillsMenu", FoodType.Burger),
            //Bagels&Donuts
            new Restaurant(new Vector3(-1318.507f, -282.2458f, 39.98732f), 115.4663f, "Dickies Bagels", "Holy Dick!","CoffeeMenu", FoodType.Bagels),
            new Restaurant(new Vector3(-1204.364f, -1146.246f, 7.699615f), 109.2444f, "Dickies Bagels", "Holy Dick!","CoffeeMenu", FoodType.Bagels),
            new Restaurant(new Vector3(354.0957f, -1028.134f, 29.33102f), 182.3497f, "Rusty Brown's", "Ring lickin' good!","CoffeeMenu", FoodType.Bagels | FoodType.Donut) {BannerImagePath = "rustybrowns.png" },

            //Coffee
            new Restaurant(new Vector3(-238.903f, -777.356f, 34.09171f), 71.47642f, "Cafe Redemption", "Who needs head when we have the whole boar?","CoffeeMenu", FoodType.Coffee),
            new Restaurant(new Vector3(370.4181f, -1027.565f, 29.33361f), 184.4234f, "Ground & Pound Cafe", "We know how to take a pounding","CoffeeMenu", FoodType.Coffee),
            new Restaurant(new Vector3(55.28403f, -799.5469f, 31.58599f), 341.3315f, "Ground & Pound Cafe", "We know how to take a pounding","CoffeeMenu", FoodType.Coffee),
            new Restaurant(new Vector3(-1283.567f, -1130.118f, 6.795891f), 143.1178f, "Bean Machine Coffee", "Taking over the world one Gunkaccino at a time","BeanMachineMenu", FoodType.Coffee | FoodType.Bagels | FoodType.Donut) { BannerImagePath = "beanmachine.png" },
            new Restaurant(new Vector3(-1549.39f, -435.5105f, 35.88667f), 234.6563f, "Bean Machine Coffee", "Taking over the world one Gunkaccino at a time","BeanMachineMenu", FoodType.Coffee | FoodType.Bagels | FoodType.Donut) { BannerImagePath = "beanmachine.png" },
            new Restaurant(new Vector3(-835.4522f, -610.4766f, 29.02697f), 142.0655f, "Bean Machine Coffee", "Taking over the world one Gunkaccino at a time","BeanMachineMenu", FoodType.Coffee | FoodType.Bagels | FoodType.Donut) { BannerImagePath = "beanmachine.png" },
            new Restaurant(new Vector3(-602.2112f, -1105.766f, 22.32427f), 273.8795f, "Bean Machine Coffee", "Taking over the world one Gunkaccino at a time","BeanMachineMenu", FoodType.Coffee | FoodType.Bagels | FoodType.Donut) { BannerImagePath = "beanmachine.png" },
            new Restaurant(new Vector3(-659.5289f, -814.0433f, 24.53778f), 232.0023f, "Bean Machine Coffee", "Taking over the world one Gunkaccino at a time","BeanMachineMenu", FoodType.Coffee | FoodType.Bagels | FoodType.Donut) {BannerImagePath = "beanmachine.png"},
            new Restaurant(new Vector3(-687.0801f, -855.6792f, 23.89398f), 0.2374549f, "Bean Machine Coffee", "Taking over the world one Gunkaccino at a time","BeanMachineMenu", FoodType.Coffee | FoodType.Bagels | FoodType.Donut) {BannerImagePath = "beanmachine.png" },
            new Restaurant(new Vector3(-1345.296f, -609.976f, 28.61888f), 304.4266f, "Bean Machine Coffee", "Taking over the world one Gunkaccino at a time","BeanMachineMenu", FoodType.Coffee | FoodType.Bagels | FoodType.Donut) { BannerImagePath = "beanmachine.png" },
            new Restaurant(new Vector3(-270.3488f, -977.3488f, 31.21763f), 164.5747f, "Bean Machine Coffee", "Taking over the world one Gunkaccino at a time","BeanMachineMenu", FoodType.Coffee | FoodType.Bagels | FoodType.Donut) { BannerImagePath = "beanmachine.png" },
            new Restaurant(new Vector3(127.9072f, -1028.778f, 29.43674f), 336.4557f, "Bean Machine Coffee", "Taking over the world one Gunkaccino at a time","BeanMachineMenu", FoodType.Coffee | FoodType.Bagels | FoodType.Donut) { BannerImagePath = "beanmachine.png" },
            new Restaurant(new Vector3(-627.6302f, 239.2284f, 81.88939f), 86.57707f, "Bean Machine Coffee", "Taking over the world one Gunkaccino at a time","BeanMachineMenu", FoodType.Coffee | FoodType.Bagels | FoodType.Donut) { BannerImagePath = "beanmachine.png" },
            new Restaurant(new Vector3(280.5522f, -964.0756f, 29.41863f), 357.4615f, "Bean Machine Coffee", "Taking over the world one Gunkaccino at a time","BeanMachineMenu", FoodType.Coffee | FoodType.Bagels | FoodType.Donut) { BannerImagePath = "beanmachine.png" },

            new Restaurant(new Vector3(-1704.005f, -1101.831f, 13.15248f), 320.0907f, "Bean Machine Coffee", "Taking over the world one Gunkaccino at a time","BeanMachineMenu", FoodType.Coffee | FoodType.Bagels | FoodType.Donut) { BannerImagePath = "beanmachine.png" },

            new Restaurant(new Vector3(-1206.975f, -1135.029f, 7.693257f), 109.1408f, "Cool Beans Coffee Co", "Come flick our beans","CoffeeMenu", FoodType.Coffee),
            new Restaurant(new Vector3(-1278.833f, -876.438f, 11.9303f), 123.2498f, "Cool Beans Coffee Co", "Come flick our beans","CoffeeMenu", FoodType.Coffee),
            new Restaurant(new Vector3(1169.704f, -403.1992f, 72.24859f), 344.0863f, "Cool Beans Coffee Co", "Come flick our beans","CoffeeMenu", FoodType.Coffee),
            new Restaurant(new Vector3(265.4628f, -981.3839f, 29.36569f), 72.07395f, "Cool Beans Coffee Co", "Come flick our beans","CoffeeMenu", FoodType.Coffee),
            new Restaurant(new Vector3(-1108.847f, -1355.264f, 5.035112f), 206.1676f, "Crucial Fix Coffee", "Get your fix","CoffeeMenu", FoodType.Coffee),
            new Restaurant(new Vector3(189.0311f, -231.234f, 54.07472f), 340.4597f, "Crucial Fix Coffee", "Get your fix","CoffeeMenu", FoodType.Coffee),
            new Restaurant(new Vector3(273.174f, -833.0611f, 29.41237f), 185.6476f, "Crucial Fix Coffee", "Get your fix","CoffeeMenu", FoodType.Coffee),
            new Restaurant(new Vector3(-576.6631f, -677.8674f, 32.36259f), 306.9058f, "Hit-N-Run Coffee", "Vehicular assault discouraged","CoffeeMenu", FoodType.Coffee),
            new Restaurant(new Vector3(-1253.337f, -296.6488f, 37.31522f), 206.5786f, "{java.update();}", "Real coffee made by fake programmers","CoffeeMenu", FoodType.Coffee),
            new Restaurant(new Vector3(-509.1889f, -22.9895f, 45.60899f), 354.7263f, "Little Teapot", "The finest organic & free range teas exploited from the third world","CoffeeMenu", FoodType.Coffee) {ScannerFilePath = "01_specific_location\\0x1980DD57.wav" },
            //Mexican
            new Restaurant(new Vector3(10.96682f, -1605.874f, 29.3931f), 229.8729f, "The Taco Farmer", "Open All Hours!","TacoFarmerMenu", FoodType.Mexican) { OpenTime = 0, CloseTime = 24 },
            new Restaurant(new Vector3(649.765f, 2728.621f, 41.9959f), 276.2882f, "Taco Farmer", "Open All Hours!","TacoFarmerMenu", FoodType.Mexican) { OpenTime = 0, CloseTime = 24 },
            new Restaurant(new Vector3(-1168.281f, -1267.279f, 6.198249f), 111.9682f, "Taco Libre", "Autentica Comida Mexicana","TacoFarmerMenu", FoodType.Mexican),
            new Restaurant(new Vector3(-657.5089f, -679.4656f, 31.46727f), 317.9819f, "Taco Bomb", "The taste explosion","TacoBombMenu", FoodType.Mexican) {BannerImagePath = "tacobomb.png" },
            new Restaurant(new Vector3(-1196.981f, -791.5534f, 16.40427f), 134.7115f, "Taco Bomb", "The taste explosion","TacoBombMenu", FoodType.Mexican) {BannerImagePath = "tacobomb.png" },
            new Restaurant(new Vector3(-1553.112f, -439.9938f, 40.51905f), 228.7506f, "Taco Bomb", "The taste explosion","TacoBombMenu", FoodType.Mexican) {BannerImagePath = "tacobomb.png" },
            new Restaurant(new Vector3(99.21678f, -1419.307f, 29.42156f), 323.9604f, "Aguila Burrito", "Best burritos in Strawberry*","TacoFarmerMenu", FoodType.Mexican),
            new Restaurant(new Vector3(445.9454f, -1241.581f, 30.27799f), 179.553f, "Attack-A-Taco", "Kill that hunger","TacoFarmerMenu", FoodType.Mexican),
            new Restaurant(new Vector3(1093.13f, -362.9193f, 67.06821f), 168.6222f, "Hearty Taco", "Eat your heart out!","TacoFarmerMenu", FoodType.Mexican),
            new Restaurant(new Vector3(438.8823f, -1465.908f, 29.35293f), 69.18111f, "Hearty Taco", "Eat your heart out!","TacoFarmerMenu", FoodType.Mexican),
            new Restaurant(new Vector3(174.9638f, -2025.427f, 18.32407f), 123.4303f, "Mom's Tacos", "We love mom's taco!","TacoFarmerMenu", FoodType.Mexican),
            new Restaurant(new Vector3(1138.658f, -962.4339f, 47.54031f), 330.8736f, "Mom's Tacos", "We love mom's taco!","TacoFarmerMenu", FoodType.Mexican),
            //Dessert
            new Restaurant(new Vector3(-1193.966f, -1543.693f, 4.373522f), 124.3727f, "The Sundae Post", "Come read up on our sludgies","DonutMenu", FoodType.Dessert),
            new Restaurant(new Vector3(-1171.529f, -1435.118f, 4.461945f), 32.60835f, "Icemaiden", "Chill Out and Grill Out","GenericMenu", FoodType.Dessert) {IsWalkup = true },
            new Restaurant(new Vector3(-1689.895f, -1076.626f, 13.15219f), 52.58161f, "Cream Pie", "I just love getting creampied!","FancyGenericMenu", FoodType.Dessert),
            new Restaurant(new Vector3(-1698.04f, -1105.689f, 13.15277f), 326.1168f, "The Cherry Popper", "You'll always remember your first","FancyGenericMenu", FoodType.Dessert),
            //Juice and Smoothies
            new Restaurant(new Vector3(-1137.926f, -1624.695f, 4.410712f), 127.6497f, "Seaside Juice", "Allegedly healthy!","FruitMenu", FoodType.Smoothies) { FullName = "Vitamin Seaside Juice Bar" },
            new Restaurant(new Vector3(-1187.057f, -1536.73f, 4.379496f), 32.85152f, "Muscle Peach", "Come oggle something new","FruitMenu", FoodType.Smoothies) { FullName = "Muscle Peach Juice Bar" },
            new Restaurant(new Vector3(-1236.263f, -287.9641f, 37.63038f), 205.8273f, "Limey's", "No Limes About It","GenericMenu", FoodType.Smoothies) { FullName = "Limey's Juice and Smoothies" },
            new Restaurant(new Vector3(250.3842f, -1026.535f, 29.25663f), 124.0668f, "Limey's", "No Limes About It","GenericMenu", FoodType.Smoothies) { FullName = "Limey's Juice and Smoothies" },
            new Restaurant(new Vector3(-1182.576f, -1248.037f, 6.991587f), 110.7639f, "Limey's", "No Limes About It","GenericMenu", FoodType.Smoothies) { FullName = "Limey's Juice Bar" },
            new Restaurant(new Vector3(-471.7646f, -18.46761f, 45.75837f), 357.2652f, "Fruit Machine", "Serving Cherry Popper","GenericMenu", FoodType.Smoothies),
            new Restaurant(new Vector3(2741.563f, 4413.093f, 48.62326f), 201.5914f, "Big Juice Stand", "So big","GenericMenu", FoodType.Smoothies),
            new Restaurant(new Vector3(1791.592f, 4594.844f, 37.68291f), 182.8134f, "Alamo Fruit", "You'll remember our fruit!","FruitMenu", FoodType.Smoothies),
            new Restaurant(new Vector3(1199.951f, -501.2592f, 65.17791f), 113.7728f, "Squeeze One Out", "Always time for a quickie","FruitMenu", FoodType.Smoothies),
            //Chicken
            new Restaurant(new Vector3(-584.761f, -872.753f, 25.91489f), 353.0746f, "Lucky Plucker", "Come be a real Lucky Plucker","GenericMenu", FoodType.Chicken | FoodType.FastFood) {ScannerFilePath = "01_specific_location\\0x14B8A4DB.wav" },
            new Restaurant(new Vector3(2580.543f, 464.6521f, 108.6232f), 176.5548f, "Bishop's Chicken", "Our chicken is a religious experience","GenericMenu", FoodType.Chicken | FoodType.FastFood),
            new Restaurant(new Vector3(169.3292f, -1634.163f, 29.29167f), 35.89598f, "Bishop's Chicken", "Our chicken is a religious experience","GenericMenu", FoodType.Chicken | FoodType.FastFood),
            new Restaurant(new Vector3(133.0175f, -1462.702f, 29.35705f), 48.47223f, "Lucky Plucker", "Come be a real Lucky Plucker","GenericMenu", FoodType.Chicken | FoodType.FastFood),
            new Restaurant(new Vector3(-138.4921f, -256.509f, 43.59497f), 290.1001f, "Cluckin' Bell", "Taste the cock","CluckinBellMenu", FoodType.Chicken | FoodType.FastFood),
            new Restaurant(new Vector3(-184.9376f, -1428.169f, 31.47968f), 33.8636f, "Cluckin' Bell", "Taste the cock","CluckinBellMenu", FoodType.Chicken | FoodType.FastFood),
            new Restaurant(new Vector3(-1681.603f, -1096.505f, 13.15227f), 180.3125f, "Cluckin' Bell", "Taste the cock","CluckinBellMenu", FoodType.Chicken | FoodType.FastFood),
            //General    
            new Restaurant(new Vector3(-1222.546f, -807.5845f, 16.59777f), 305.3918f, "Lettuce Be", "A real meat free experience","GenericMenu", FoodType.Generic),
            new Restaurant(new Vector3(-1196.705f, -1167.969f, 7.695099f), 108.4535f, "Lettuce Be", "A real meat free experience","GenericMenu", FoodType.Generic),
            new Restaurant(new Vector3(-1535.082f, -422.2449f, 35.59194f), 229.4618f, "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu", FoodType.FastFood),
            new Restaurant(new Vector3(49.24896f, -1000.381f, 29.35741f), 335.6092f, "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu", FoodType.FastFood),
            new Restaurant(new Vector3(-1271.224f, -1200.703f, 5.366248f), 70.19876f, "The Nut Buster", "Bust a Nut every day","GenericMenu", FoodType.Generic),
            new Restaurant(new Vector3(166.2677f, -1450.995f, 29.24164f), 142.858f, "Ring Of Fire Chili House", "Incinerate your insides","GenericMenu", FoodType.Generic) { ScannerFilePath = "01_specific_location\\0x0DA81BFE.wav"},
        };
        List<Pharmacy> Pharmacies = new List<Pharmacy>()
        {
            new Pharmacy(new Vector3(114.2954f, -4.942202f, 67.82149f), 195.4308f,  "Pop's Pills", "","PharmacyMenu"),
            new Pharmacy(new Vector3(68.94705f, -1570.043f, 29.59777f), 50.85398f, "Dollar Pills", "","PharmacyMenu") {ScannerFilePath = "01_specific_location\\0x017D2BE2.wav" },
            new Pharmacy(new Vector3(326.7227f, -1074.448f, 29.47332f), 359.3641f, "Family Pharmacy", "","PharmacyMenu"),
            new Pharmacy(new Vector3(805.143f, -1063.586f, 28.42115f), 90.00111f, "Meltz's Pharmacy", "","PharmacyMenu"),
            new Pharmacy(new Vector3(1225.14f, -391.3563f, 68.68563f), 28.81875f, "Pharmacy", "","PharmacyMenu"),
            new Pharmacy(new Vector3(151.0329f, 6647.418f, 31.594f), 135.0961f, "Pop's Pills", "","PharmacyMenu"),
            new Pharmacy(new Vector3(-172.4879f, 6381.202f, 31.47279f), 222.4285f, "Bay Side Drugs", "","PharmacyMenu"),
            new Pharmacy(new Vector3(214.0241f, -1835.08f, 27.54375f), 318.7183f, "Family Pharmacy", "","PharmacyMenu"),
            new Pharmacy(new Vector3(591.2585f, 2744.49f, 42.0425f), 184.8661f, "Dollar Pills", "","PharmacyMenu") { ScannerFilePath = "01_specific_location\\0x19E069DE.wav" },
            new Pharmacy(new Vector3(825.8448f, -97.80138f, 80.59971f), 321.7153f, "Pharmacy", "","PharmacyMenu"),
            new Pharmacy(new Vector3(84.03228f, -810.2528f, 31.41642f), 350.9262f, "Family Pharmacy", "","PharmacyMenu"),
        };
        List<Dispensary> Dispensaries = new List<Dispensary>() {
            new Dispensary(new Vector3(-1161.365f, -1427.646f, 4.623186f), 31.50553f, "Doctor Kush", "","WeedMenu"),
            new Dispensary(new Vector3(-502.4879f, 32.92564f, 44.71512f), 179.9803f, "Serenity Wellness", "","WeedMenu"),
            new Dispensary(new Vector3(169.5722f, -222.869f, 54.23643f), 342.0811f, "High Time", "","WeedMenu"),
            new Dispensary(new Vector3(-1381.142f, -941.0327f, 10.17387f), 126.4558f,"Seagrass Herbals", "Seagrass Herbals","WeedMenu"),
            new Dispensary(new Vector3(1175.02f, -437.1972f, 66.93162f), 259.358f, "Mile High Organics", "","WeedMenu"),
        };
        List<HeadShop> HeadShops = new List<HeadShop>() {
            new HeadShop(new Vector3(-1191.582f, -1197.779f, 7.617113f), 146.801f, "Pipe Dreams", "","HeadShopMenu"),
            new HeadShop(new Vector3(65.60603f, -137.4155f, 55.11251f), 214.0327f, "Pipe Dreams", "","HeadShopMenu"),
            new HeadShop(new Vector3(278.8327f, -1027.653f, 29.21136f), 184.1326f, "Pipe Down Cigars", "","HeadShopMenu"),
            new HeadShop(new Vector3(-1154.942f, -1373.176f, 5.061489f), 305.589f, "Amnesiac Smoke Shop", "","WeedAndCigMenu"),
            new HeadShop(new Vector3(-269.2553f, 243.7069f, 90.40055f), 1.693904f, "Pipe Down", "","HeadShopMenu"),

        };
        List<SportingGoodsStore> SportingGoodsStores = new List<SportingGoodsStore>() {
            new SportingGoodsStore(new Vector3(-945.9442f, -1191.532f, 4.956469f), 168.678f, "Vespucci Sports", "Our rent is so high, we must have quality items!","SportingGoodsMenu") { BannerImagePath = "vespuccisports.png" },

        };
        List<HardwareStore> HardwareStores = new List<HardwareStore>() {
            new HardwareStore(new Vector3(2747.406f, 3473.213f, 55.67021f), 249.8152f, "You Tool", "Show your wife who the family tool is","ToolMenu") {BannerImagePath = "youtool.png", CameraPosition = new Vector3(2780.472f, 3473.511f, 73.06239f), CameraDirection = new Vector3(-0.9778581f, -0.02382228f, -0.2079087f), CameraRotation = new Rotator(-11.99983f, 0f, 91.39555f) },
            new HardwareStore(new Vector3(339.4021f, -776.9934f, 29.2665f), 68.51967f, "Krapea", "We fake it, you make it","ToolMenu"),
            new HardwareStore(new Vector3(-10.88182f, 6499.395f, 31.50508f), 44.30542f, "Bay Hardware", "","ToolMenu"),
            new HardwareStore(new Vector3(-3153.697f, 1053.398f, 20.88735f), 338.4756f, "Hardware", "","ToolMenu"),
            new HardwareStore(new Vector3(343.2759f, -1297.948f, 32.5097f), 164.2121f, "Bert's Tool Supply", "","ToolMenu"),

        };
        List<PawnShop> PawnShops = new List<PawnShop>() {
            new PawnShop(new Vector3(412.5109f, 314.9815f, 103.1327f), 207.4105f, "F.T. Pawn", "","ToolMenu"),
            new PawnShop(new Vector3(182.735f, -1318.845f, 29.31752f), 246.2635f, "Pawn & Jewelery", "","ToolMenu"),
        };
        List<Landmark> Landmarks = new List<Landmark>()
        {
            new Landmark(new Vector3(-248.491f, -2010.509f, 34.574f), 0f,"Maze Bank Arena","I heard Fame or Shame was filming there") {OpenTime = 0,CloseTime = 24, InteriorID = 78338 },

            new Landmark(new Vector3(2469.03f, 4955.278f, 45.11892f), 0f,"O'Neil Ranch","Need some meth?") { OpenTime = 0,CloseTime = 24, InteriorID = 31746, ScannerFilePath = "01_specific_location\\0x1E2AE79B.wav" },
            new Landmark(new Vector3(-1045.065f, -230.3523f, 39.01435f), 294.2673f,"Lifeinvader","Get Stalked") {OpenTime = 0,CloseTime = 24, InteriorID = 3330 },
            new Landmark(new Vector3(2.69f, -667.01f, 16.13f), 0f,"Union Depository","") {IsTemporarilyClosed = true, OpenTime = 0,CloseTime = 24, InteriorID = 119042 },

            new Landmark(new Vector3(-34.58836f, 6287.814f, 31.38976f), 28.21855f,"Clucking Bell Farms","Know the saying about seeing how sausage is made?") {OpenTime = 0,CloseTime = 24, InteriorID = 28162, ScannerFilePath = "01_specific_location\\0x0D8D06A1.wav" },
            new Landmark(new Vector3(718.2269f, -976.7165f, 24.71099f), 181.558f,"Darnell Bros. Garments","We make more than just garments") {IsTemporarilyClosed = true, OpenTime = 0,CloseTime = 24, InteriorID = 92674 },
            new Landmark(new Vector3(-598.1064f, -1610.67f, 26.01035f), 0f,"Rogers Salvage & Scrap","Taking your scrap since 1924") { IsTemporarilyClosed = true, OpenTime = 0,CloseTime = 24, InteriorID = -103 },

            new Landmark(new Vector3(562.8467f, 2741.614f, 42.86892f), 184.4624f,"Animal Ark","We have two of everything!") { IsTemporarilyClosed = true, OpenTime = 0,CloseTime = 24,ScannerFilePath = "01_specific_location\\0x147855FA.wav" },
            new Landmark(new Vector3(-232.18f, -914.93f, 32.77f), 338.4021f,"Post Op Headquarters","No longer just mail") { IsTemporarilyClosed = true, OpenTime = 0,CloseTime = 24,ScannerFilePath = "01_specific_location\\0x02967EFD.wav" },
            new Landmark(new Vector3(-3022.02f, 83.00665f, 10.64196f), 0f,"Pacific Bluffs Country Club","Members only") { OpenTime = 0,CloseTime = 24,ScannerFilePath = "01_specific_location\\0x0431FE2B.wav" },
            new Landmark(new Vector3(105.46f, 252.27f, 109.01f), 162.7602f,"Vinewood Star Tours","Stalk celebrities from the comfort of a bus") { IsTemporarilyClosed = true, OpenTime = 0,CloseTime = 24,ScannerFilePath = "01_specific_location\\0x04F6FA01.wav" },
            new Landmark(new Vector3(-1261.62f, -347.33f, 37.22f), 53f,"Pump-N-Run Gym","Get ripped!") { IsTemporarilyClosed = true, OpenTime = 0,CloseTime = 24,ScannerFilePath = "01_specific_location\\0x06AD518B.wav" },
            new Landmark(new Vector3(-247.62f, -1526.22f, 33.03f), 53f,"BJ Smith Rec Center","To win you have to annihilate everything in your path") { IsTemporarilyClosed = true, OpenTime = 0,CloseTime = 24,ScannerFilePath = "01_specific_location\\0x05AB836E.wav" },

            new Landmark(new Vector3(3425.02f, 5174.67f, 8.13f), 0f,"El Gordo Lighthouse","Watch your step") { OpenTime = 0,CloseTime = 24,ScannerFilePath = "01_specific_location\\0x0761393B.wav" },
            //new Landmark(new Vector3(241.2085f, -1378.962f, 33.74176f), 140.41f,"Los Santos County Coroner Office","Check out the gift shop!") { IsTemporarilyClosed = true, OpenTime = 0,CloseTime = 24,InteriorID = 60418,ScannerFilePath = "01_specific_location\\0x04F66C50.wav" },

            new Landmark(new Vector3(-1234.788f, -768.6721f, 17.95432f), 0f,"Prosperity Street Promenade","Come spend money like a rich person!") { OpenTime = 0,CloseTime = 24,ScannerFilePath = "01_specific_location\\0x077E335F.wav" },
            new Landmark(new Vector3(847.05f, -1992.77f, 30.11f), 0f,"Pisswasser Factory","You're In, For A Good Time") { IsTemporarilyClosed = true, OpenTime = 0,CloseTime = 24,ScannerFilePath = "01_specific_location\\0x08AA4C64.wav" },
            new Landmark(new Vector3(436.0983f, -645.8003f, 27.75121f), 100f,"Dashound Bus Center","Long journeys need short legs") { IsTemporarilyClosed = true, OpenTime = 0,CloseTime = 24,ScannerFilePath = "01_specific_location\\0x09A9666F.wav" },
            new Landmark(new Vector3(129.8f, -1300.28f, 30.05f), 0f,"Vanilla Unicorn","Seeing is relieving") { OpenTime = 0,CloseTime = 24,ScannerFilePath = "01_specific_location\\0x0D1B649D.wav" },

            new Landmark(new Vector3(712.96f, 1204.1f, 329.3f), 0f,"Vinewood Sign","Or was it Vinewoodland?") { OpenTime = 0,CloseTime = 24,ScannerFilePath = "01_specific_location\\0x0C57ACE0.wav" },
            new Landmark(new Vector3(-200.26f, -1380.72f, 32.83f), 0f,"Glass Heroes Auto Repairs","We never crack under pressure") { IsTemporarilyClosed = true, OpenTime = 8,CloseTime = 17,ScannerFilePath = "01_specific_location\\0x105B95C3.wav" },
            new Landmark(new Vector3(-2.34f, -1400.51f, 30.22f), 0f,"South LS Hand Car Wash","Let us give you a hand") { OpenTime = 0,CloseTime = 24,ScannerFilePath = "01_specific_location\\0x0CC361AF.wav" },
            new Landmark(new Vector3(-698.5507f, 46.47984f, 44.03382f), 204.1632f, "The Epsilon Program", "Kifflom!"){ OpenTime = 0,CloseTime = 24 },
        };
        List<SubwayStation> SubwayStations = new List<SubwayStation>()
        {
            new SubwayStation(new Vector3(-245.8754f, -335.3599f, 29.97557f), 184.3399f, "Burton Subway Station",""),
            new SubwayStation(new Vector3(-490.9833f, -697.8607f, 33.24188f), 358.6056f, "Little Seoul Subway Station",""),
            new SubwayStation(new Vector3(-1368.768f, -528.9166f, 30.30007f), 125.7357f, "Del Perro Subway Station",""),
            new SubwayStation(new Vector3(-800.5429f, -101.6647f, 37.56856f), 294.5015f, "Portola Drive Subway Station",""),
            new SubwayStation(new Vector3(-1040.5f, -2742.292f, 13.92607f), 329.527f, "LSIA Terminal 4 Subway Station",""),
            new SubwayStation(new Vector3(-946.489f, -2329.504f, 6.763008f), 331.0874f, "LSIA Parking Subway Station",""),
        };
        List<BeautyShop> BeautyShops = new List<BeautyShop>()
        {
            new BeautyShop(new Vector3(187.7006f, -1812.874f, 28.94536f), 323.7488f, "Carson's Beauty Supply", "") { IsTemporarilyClosed = true },
            new BeautyShop(new Vector3(442.8802f, -2044.086f, 23.73479f), 317.6541f, "Chantelle's Beauty Salon", "") { IsTemporarilyClosed = true },
            new BeautyShop(new Vector3(-342.2452f, -1482.83f, 30.70707f), 269.4036f, "Discount Beauty Store", "") { IsTemporarilyClosed = true },
            new BeautyShop(new Vector3(-3050.165f, 625.066f, 7.269026f), 290.7953f, "Belinda May's Beauty Salon", "") { IsTemporarilyClosed = true },
            new BeautyShop(new Vector3(1705.34f, 3780.338f, 34.7582f), 214.8316f, "Aunt Tammy's Hair Salon", "") { IsTemporarilyClosed = true },
        };
        List<Bank> Banks = new List<Bank>()
        {
            new Bank(new Vector3(-813.9924f, -1114.698f, 11.18181f), 297.7995f, "Fleeca Bank", "") { IsEnabled = false },
            new Bank(new Vector3(-350.1604f, -45.84864f, 49.03682f), 337.4063f, "Fleeca Bank", "") { IsEnabled = false },
            new Bank(new Vector3(-1318f, -831.5065f, 16.97263f), 125.3848f, "Maze Bank", "") { IsEnabled = false },
            new Bank(new Vector3(150.9058f, -1036.347f, 29.33961f), 340.9843f,  "Fleeca Bank", "") { IsEnabled = false },
            new Bank(new Vector3(315.2256f, -275.1059f, 53.92431f), 345.6797f,  "Fleeca Bank", "") { IsEnabled = false },
            new Bank(new Vector3(-3142.849f, 1131.727f, 20.84295f), 247.9002f, "Blaine County Savings", "") { IsEnabled = false },
            new Bank(new Vector3(-2966.905f, 483.1484f, 15.6927f), 86.25156f,  "Fleeca Bank", "") { IsEnabled = false },
            new Bank(new Vector3(1175.215f, 2702.15f, 38.17273f), 176.9885f, "Fleeca Bank", "") { IsEnabled = false },
        };
        List<ConvenienceStore> ConvenienceStores = new List<ConvenienceStore>()
        {
            new ConvenienceStore(new Vector3(547f, 2678f, 42.1565f), 22.23846f,  "24/7","As fast as you","TwentyFourSevenMenu"){ OpenTime = 0, CloseTime = 24,BannerImagePath = "247.png", InteriorID = 41474, VendorPosition = new Vector3(549.6005f, 2669.846f, 42.1565f), VendorHeading = 96.91093f,},
            new ConvenienceStore(new Vector3(-3236.767f,1005.609f,12.33137f), 122.6316f,  "24/7","As fast as you","TwentyFourSevenMenu"){ OpenTime = 0, CloseTime = 24,BannerImagePath = "247.png", InteriorID = 16386, VendorPosition = new Vector3(-3243.302f, 1000.005f, 12.83071f), VendorHeading = 352.6259f, },
            new ConvenienceStore(new Vector3(2560f, 385f, 108f), 22.23846f,  "24/7","As fast as you","TwentyFourSevenMenu"){ OpenTime = 0, CloseTime = 24,BannerImagePath = "247.png", InteriorID = 62722, VendorPosition = new Vector3(2555.339f, 380.9034f, 108.6229f), VendorHeading = 347.3629f, },
            new ConvenienceStore(new Vector3(29.32254f, -1350.485f, 29.33319f), 170.9901f,  "24/7", "As fast as you","TwentyFourSevenMenu"){ OpenTime = 0, CloseTime = 24,BannerImagePath = "247.png", InteriorID = 33282, VendorPosition = new Vector3(24.39647f, -1345.484f, 29.49702f), VendorHeading = 252.9084f, },
            new ConvenienceStore(new Vector3(-3037.729f, 589.7671f, 7.814812f), 289.0175f,  "24/7", "As fast as you","TwentyFourSevenMenu"){ OpenTime = 0, CloseTime = 24,BannerImagePath = "247.png", InteriorID = 97538, VendorPosition = new Vector3(-3039.787f, 584.1979f, 7.908929f), VendorHeading = 12.80189f, },
            new ConvenienceStore(new Vector3(376.3202f, 322.694f, 103.4389f), 162.5363f,  "24/7", "As fast as you","TwentyFourSevenMenu"){ OpenTime = 0, CloseTime = 24,BannerImagePath = "247.png", InteriorID = 46850, VendorPosition = new Vector3(372.6485f, 327.0293f, 103.5664f), VendorHeading = 257.6475f,ScannerFilePath = "01_specific_location\\0x000E7300.wav" },//Vinewood
            new ConvenienceStore(new Vector3(2682.938f, 3282.287f, 55.24056f), 243.885f,  "24/7", "As fast as you","TwentyFourSevenMenu") { OpenTime = 0, CloseTime = 24 ,BannerImagePath = "247.png", InteriorID = 13826, VendorPosition = new Vector3(2676.595f, 3280.101f, 55.24113f), VendorHeading = 325.0921f,},
            new ConvenienceStore(new Vector3(1730.507f, 6410.014f, 35.00065f), 153.9039f,  "24/7","As fast as you","TwentyFourSevenMenu") {  OpenTime = 0, CloseTime = 24,BannerImagePath = "247.png", InteriorID = 36354, VendorPosition = new Vector3(1728.436f, 6416.584f, 35.03722f), VendorHeading = 241.2023f, },//Braddock pass
            new ConvenienceStore(new Vector3(1965.801f, 3739.945f, 32.322f), 207.564f,  "24/7","As fast as you","TwentyFourSevenMenu") { OpenTime = 0, CloseTime = 24,BannerImagePath = "247.png", InteriorID = 55554, VendorPosition = new Vector3(1959.352f, 3741.18f, 32.34374f), VendorHeading = 303.8849f, },
            new ConvenienceStore(new Vector3(-53.5351f, -1757.196f, 29.43954f), 146.0623f,  "LtD", "unLTD great prices!","LTDMenu"){ BannerImagePath = "ltd.png",OpenTime = 0, CloseTime = 24, InteriorID = 80642, VendorPosition = new Vector3(-45.89098f, -1757.345f, 29.42101f), VendorHeading = 52.66933f, },
            new ConvenienceStore(new Vector3(-578.0112f, -1012.898f, 22.32503f), 359.4114f,  "24/7", "As fast as you","TwentyFourSevenMenu"){ OpenTime = 0, CloseTime = 24,BannerImagePath = "247.png" },
            new ConvenienceStore(new Vector3(-696.9965f, -858.7673f, 23.69209f), 85.51252f,  "24/7", "24/7","TwentyFourSevenMenu"){ OpenTime = 0, CloseTime = 24,BannerImagePath = "247.png" },
            new ConvenienceStore(new Vector3(152.5101f, 237.4131f, 106.9718f), 165.2823f,  "24/7", "As fast as you","TwentyFourSevenMenu"){ OpenTime = 0, CloseTime = 24,BannerImagePath = "247.png" },
            new ConvenienceStore(new Vector3(201.8985f, -26.30606f, 69.90953f), 249.8224f,  "24/7", "As fast as you","TwentyFourSevenMenu"){ OpenTime = 0, CloseTime = 24,BannerImagePath = "247.png" },
            new ConvenienceStore(new Vector3(528.017f, -152.1372f, 57.20173f), 44.64286f,  "24/7", "As fast as you","TwentyFourSevenMenu"){ OpenTime = 0, CloseTime = 24,BannerImagePath = "247.png" },
            new ConvenienceStore(new Vector3(-1264.064f, -1162.599f, 6.764161f), 161.218f,  "Fruit Of The Vine", "Not just for winos","FruitVineMenu"),
            new ConvenienceStore(new Vector3(-1270.649f, -304.9037f, 37.06938f), 257.2106f,  "Fruit Of The Vine", "Not just for winose","FruitVineMenu"),
            new ConvenienceStore(new Vector3(164.9962f, 351.1263f, 109.6859f), 4.847032f,  "Fruit Of The Vine", "Not just for winos","FruitVineMenu") ,
            new ConvenienceStore(new Vector3(-144.3732f, -65.01408f, 54.60635f), 159.0404f,  "Fruit Of The Vine", "Not just for winos","FruitVineMenu") ,
            new ConvenienceStore(new Vector3(-1412.015f, -320.1292f, 44.37897f), 92.48502f,  "The Grain Of Truth", "Seek the truth","GrainOfTruthMenu"),
            new ConvenienceStore(new Vector3(-1370.819f, -684.5463f, 25.01069f), 214.6929f,  "The Grain Of Truth", "Seek the truth","GrainOfTruthMenu"),
            new ConvenienceStore(new Vector3(1707.748f, 4792.387f, 41.98377f), 90.42564f,  "Supermarket", "","ConvenienceStoreMenu"),
            new ConvenienceStore(new Vector3(-1539.045f, -900.472f, 10.16951f), 129.0318f,  "Del Perro Food Market","No Robberies Please!","ConvenienceStoreMenu"),
            new ConvenienceStore(new Vector3(-1359.607f, -963.3494f, 9.699487f), 124.3222f,  "A&R Market", "","ConvenienceStoreMenu"),
            new ConvenienceStore(new Vector3(53.28459f, -1478.863f, 29.28546f), 187.4217f,  "Gabriela's Market", "","ConvenienceStoreMenu"),
            new ConvenienceStore(new Vector3(-551.5444f, -855.8014f, 28.28332f), 2.39254f,  "Save-A-Cent", "","ConvenienceStoreMenu"),
            new ConvenienceStore(new Vector3(-1312.64f, -1181.899f, 4.890057f), 271.5434f,  "Beach Buddie", "","ConvenienceStoreMenu"),
            new ConvenienceStore(new Vector3(-661.5522f, -915.5651f, 24.61216f), 260.1033f,  "Convenience Store", "","ConvenienceStoreMenu"),
            new ConvenienceStore(new Vector3(393.8511f, -804.2294f, 29.29397f), 272.0436f,  "Food Market", "","ConvenienceStoreMenu"),
            new ConvenienceStore(new Vector3(410.4163f, -1910.432f, 25.45381f), 88.30214f,  "Long Pig Mini Mart", "","ConvenienceStoreMenu"),
            new ConvenienceStore(new Vector3(-170.9145f, -1449.77f, 31.64507f), 50.18372f,  "Cert-L Market", "","ConvenienceStoreMenu"),
            new ConvenienceStore(new Vector3(-3152.666f, 1110.093f, 20.87176f), 245.5282f,  "Nelsons", "","ConvenienceStoreMenu"),
            new ConvenienceStore(new Vector3(1331.857f, -1643.031f, 52.13754f), 352.7884f,  "Convenience Store", "","ConvenienceStoreMenu"),
            new ConvenienceStore(new Vector3(1089.146f, -776.6399f, 58.34211f), 357.3385f,  "Chico's Hypermarket", "","ConvenienceStoreMenu"),
            new ConvenienceStore(new Vector3(1168.982f, -291.6786f, 69.02177f), 324.2124f,  "Gabriela's Market", "","ConvenienceStoreMenu"),
            new ConvenienceStore(new Vector3(105.3277f, -1689.715f, 29.29169f), 138.498f,  "Convenience Store", "","ConvenienceStoreMenu"),
            new ConvenienceStore(new Vector3(-297.9112f, -1332.895f, 31.29597f), 314.8022f,  "Long Pig Mini Market", "","ConvenienceStoreMenu"),
            new ConvenienceStore(new Vector3(87.1592f, -1670.73f, 29.16286f), 75.61347f,  "Convenience Store", "","ConvenienceStoreMenu"),
            new ConvenienceStore(new Vector3(877.7466f, -132.4732f, 78.59199f), 322.8992f,  "B.J.'s Market", "","ConvenienceStoreMenu"),
            new ConvenienceStore(new Vector3(-59.0772f, 6523.902f, 31.49085f), 314.5225f,  "Willie's Supermarket", "","ConvenienceStoreMenu"),
            new ConvenienceStore(new Vector3(-1478.208f, -949.8577f, 10.21891f), 187.2803f, "The Beachfront Market", "","ConvenienceStoreMenu"),
        };
        List<LiquorStore> LiquorStores = new List<LiquorStore>()
        {
                       new LiquorStore(new Vector3(-1226.09f, -896.166f, 12.4057f), 22.23846f, "Rob's Liquors","Thats My Name, Don't Rob Me!","LiquorStoreMenu") { VendorPosition = new Vector3(-1221.119f, -908.5667f, 12.32635f), VendorHeading = 33.35855f,OpenTime = 4, CloseTime = 22, InteriorID = 50178 },
            new LiquorStore(new Vector3(-2974.098f, 390.9085f, 15.03413f), 84.05217f, "Rob's Liquors", "Thats My Name, Don't Rob Me!","LiquorStoreMenu"){ VendorPosition = new Vector3(-2966.361f, 390.1463f, 15.04331f), VendorHeading = 88.73301f,OpenTime = 4, CloseTime = 22, InteriorID = 19202 },
            new LiquorStore(new Vector3(-1491.638f, -384.0242f, 40.08308f), 136.3043f, "Rob's Liquors", "Thats My Name, Don't Rob Me!","LiquorStoreMenu") { VendorPosition = new Vector3(-1486.196f, -377.7115f, 40.16343f), VendorHeading = 133.357f,OpenTime = 4, CloseTime = 22, InteriorID = 98818 },
            new LiquorStore(new Vector3(1141.463f, -980.9073f, 46.41084f), 275.826f,"Rob's Liquors", "Thats My Name, Don't Rob Me!","LiquorStoreMenu"){ VendorPosition = new Vector3(1133.534f, -983.1248f, 46.41584f), VendorHeading = 276.1162f,OpenTime = 4, CloseTime = 22, InteriorID = 73986 },
            new LiquorStore(new Vector3(1166.318f, 2702.173f, 38.17925f), 175.7192f, "Scoops Liquor Barn", "We've got the scoop on booze","LiquorStoreMenu"){ VendorPosition = new Vector3(1165.581f, 2710.774f, 38.15771f), VendorHeading = 176.1093f,OpenTime = 4, CloseTime = 22, InteriorID = 33026 },
            new LiquorStore(new Vector3(-596.1056f, 277.69f, 82.16035f), 170.2155f, "Eclipse Liquor.Deli", "You'll be seeing a real blackout","LiquorStoreMenu") ,
            new LiquorStore(new Vector3(-239.7314f, 244.0006f, 92.03992f), 318.9458f, "Ellen's Liquor Lover", "Generous Liquor","LiquorStoreMenu") ,
            new LiquorStore(new Vector3(807.3504f, -1073.531f, 28.92093f), 134.3175f, "Liquor Market", "","LiquorStoreMenu") ,
            new LiquorStore(new Vector3(234.4411f, -1946.841f, 22.95617f), 3.404058f, "Liquor Store", "","LiquorStoreMenu"),
            new LiquorStore(new Vector3(1212.343f, -445.0079f, 66.96259f), 74.05521f, "Liquor", "","LiquorStoreMenu") ,
            new LiquorStore(new Vector3(-1208.469f, -1384.053f, 4.085135f), 68.08948f, "Steamboat Beers", "","LiquorStoreMenu") ,
            new LiquorStore(new Vector3(-1106.07f, -1287.686f, 5.421459f), 161.3398f, "Vespucci Liquor Market", "","LiquorStoreMenu") ,
            new LiquorStore(new Vector3(-697.8242f, -1182.286f, 10.71113f), 132.7831f, "Liquor Market", "","LiquorStoreMenu") { OpenTime = 4, CloseTime = 22 },
            new LiquorStore(new Vector3(-882.7062f, -1155.351f, 5.162508f), 215.8305f, "Liquor Hole", "You know you want it!","LiquorStoreMenu") { OpenTime = 4, CloseTime = 22,BannerImagePath = "liquorhole.png" },
            new LiquorStore(new Vector3(-601.9684f, 244.0188f, 82.3046f), 358.6468f, "Liquor Hole", "You know you want it!","LiquorStoreMenu") { OpenTime = 4, CloseTime = 22,BannerImagePath = "liquorhole.png" },
            new LiquorStore(new Vector3(456.5478f, 130.5207f, 99.28537f), 162.9724f, "Vinewood Liquor", "","LiquorStoreMenu") { OpenTime = 4, CloseTime = 22 },
            new LiquorStore(new Vector3(1391.861f, 3606.275f, 34.98093f), 199.2899f, "Liquor Ace", "Now socially acceptable", "LiquorStoreMenu") { VendorPosition = new Vector3(1391.861f, 3606.275f, 34.98093f), VendorHeading = 199.2899f, OpenTime = 4, CloseTime = 22, ScannerFilePath  = "01_specific_location\\0x1A23351D.wav" },
            new LiquorStore(new Vector3(1952.552f, 3840.833f, 32.17612f), 298.8575f, "Sandy Shores Liquor", "","LiquorStoreMenu") { OpenTime = 4, CloseTime = 22 },
            new LiquorStore(new Vector3(2455.443f, 4058.518f, 38.06472f), 250.6311f, "Liquor Market 24/7", "","LiquorStoreMenu"){ OpenTime = 0, CloseTime = 24 },
            new LiquorStore(new Vector3(2481.348f, 4100.31f, 38.13171f), 249.6295f, "Liquor Store", "","LiquorStoreMenu"){ OpenTime = 4, CloseTime = 22 },
            new LiquorStore(new Vector3(2566.804f, 4274.108f, 41.98908f), 239.0765f, "Grape Smuggler's Liquor", "","LiquorStoreMenu"){ OpenTime = 4, CloseTime = 22 },
            new LiquorStore(new Vector3(98.82836f, -1308.733f, 29.27369f), 121.1285f, "The Brewer's Drop", "","LiquorStoreMenu"){ OpenTime = 4, CloseTime = 22 },
            new LiquorStore(new Vector3(169.9896f, -1336.975f, 29.30038f), 284.8854f, "Liquor Beer & Wine", "","LiquorStoreMenu"){ OpenTime = 4, CloseTime = 22 },
            new LiquorStore(new Vector3(-43.36616f, -1042.139f, 28.33997f), 76.74245f, "Downtown Liquor.Deli", "","LiquorStoreMenu"){ OpenTime = 4, CloseTime = 22 },
            new LiquorStore(new Vector3(382.3881f, -1076.69f, 29.42185f), 267.7594f, "Downtown Liquor.Deli", "","LiquorStoreMenu"){ OpenTime = 4, CloseTime = 22 },
            new LiquorStore(new Vector3(-57.89392f, -91.45875f, 57.75766f), 118.0359f, "Liquor", "","LiquorStoreMenu"){ OpenTime = 4, CloseTime = 22 },
            new LiquorStore(new Vector3(456.2009f, -2059.208f, 23.9267f), 274.6541f, "South LS Liquor", "","LiquorStoreMenu"){ OpenTime = 4, CloseTime = 22 },
            new LiquorStore(new Vector3(129.7891f, -1643.382f, 29.29159f), 38.8853f, "Liquor", "","LiquorStoreMenu"){ OpenTime = 4, CloseTime = 22 },
            new LiquorStore(new Vector3(463.9041f, -1852.109f, 27.79801f), 3.177461f, "Liquor Store", "","LiquorStoreMenu"){ OpenTime = 4, CloseTime = 22 },
            new LiquorStore(new Vector3(-156.2888f, 6327.238f, 31.58083f), 316.2542f, "Del Vecchio Liquor", "","LiquorStoreMenu"){ OpenTime = 4, CloseTime = 22 },
            new LiquorStore(new Vector3(-406.0117f, 6062.374f, 31.50013f), 132.2045f, "Liquor", "","LiquorStoreMenu"){ OpenTime = 4, CloseTime = 22 },
            new LiquorStore(new Vector3(579.2075f, 2677.982f, 41.84144f), 10.21047f, "Jr. Market Liquors", "","LiquorStoreMenu"){ OpenTime = 4, CloseTime = 22 },
            new LiquorStore(new Vector3(910.836f, 3644.788f, 32.67847f), 180.167f, "Liquor Market", "","LiquorStoreMenu"){ OpenTime = 4, CloseTime = 22 },

        };
        List<GasStation> GasStations = new List<GasStation>()
        {
            new GasStation(new Vector3(-711.9264f, -917.7573f, 19.21472f), 180.3014f, "LtD", "unLTD great prices!","LTDMenu"){ BannerImagePath = "ltd.png",VendorPosition = new Vector3(-705.7453f, -913.6598f, 19.21559f), VendorHeading = 83.75771f, OpenTime = 0, CloseTime = 24, InteriorID = 47874 },
            new GasStation(new Vector3(1698.097f, 4929.837f, 42.0781f), 48.2484f, "LtD", "unLTD great prices!","LTDMenu"){ BannerImagePath = "ltd.png",VendorPosition = new Vector3(1698.044f, 4922.526f, 42.06367f), VendorHeading = 314.3236f, OpenTime = 0, CloseTime = 24, InteriorID = 45570 },



            //BROKEN VENDOR POS?//new GasStation(new Vector3(1159.861f, -327.4188f, 69.21286f), 188.791f, "LtD","unLTD great prices!","LTDMenu"){ BannerImagePath = "ltd.png",VendorPosition = new Vector3(1164.927f, -323.7075f, 69.2051f), VendorHeading = 90.6181f, OpenTime = 0, CloseTime = 24, InteriorID = 2050 },
            new GasStation(new Vector3(1159.861f, -327.4188f, 69.21286f), 188.791f, "LtD","unLTD great prices!","LTDMenu"){ BannerImagePath = "ltd.png",VendorPosition = new Vector3(1165.12f, -322.5843f, 68.80514f), VendorHeading = 98.88105f, OpenTime = 0, CloseTime = 24, InteriorID = 2050,VendorModels = new List<string>() { "a_m_y_yoga_01" } },



            new GasStation(new Vector3(-1817.871f,787.0063f,137.917f), 89.38248f, "LtD","unLTD great prices!","LTDMenu"){ BannerImagePath = "ltd.png",OpenTime = 0, CloseTime = 24, InteriorID = 82178, VendorPosition = new Vector3(-1819.707f, 794.1723f, 138.0823f),VendorHeading =  122.8981f },
            //new Vector3(1165.405f, -323.4462f, 69.20515f)
            new GasStation(new Vector3(166.2001f, -1553.691f, 29.26175f), 218.9514f, "Ron", "Put RON in your tank","RonMenu") {  BannerImagePath = "ron.png",OpenTime = 4, CloseTime = 22, ScannerFilePath = "01_specific_location\\0x0D6F777B.wav", CameraPosition = new Vector3(175.2995f, -1593.878f, 39.27175f), CameraDirection = new Vector3(-0.1031758f, 0.9726905f, -0.2079136f), CameraRotation = new Rotator(-12.00011f, 0f, 6.054868f) },
            new GasStation(new Vector3(-1427.998f, -268.4702f, 46.2217f), 132.4002f, "Ron", "Put RON in your tank","RonMenu"){ BannerImagePath = "ron.png",OpenTime = 4, CloseTime = 22, ScannerFilePath = "01_specific_location\\0x056AF0EC.wav" },
            new GasStation(new Vector3(2559.112f, 373.5359f, 108.6211f), 265.8011f, "Ron", "Put RON in your tank","RonMenu") { BannerImagePath = "ron.png",OpenTime = 4, CloseTime = 22 },
           // new GasStation(new Vector3(-1429.33f,-270.8909f,46.2077f), 325.7301f, "Ron","Put RON in your tank","RonMenu") { BannerImagePath = "ron.png",OpenTime = 4, CloseTime = 22 },
            new GasStation(new Vector3(-2544.116f, 2315.928f, 33.21614f), 3.216755f, "Ron", "Put RON in your tank","RonMenu"){ BannerImagePath = "ron.png",OpenTime = 4, CloseTime = 22 },
            new GasStation(new Vector3(818.2819f, -1040.907f, 26.75079f), 358.5326f, "Ron", "Put RON in your tank","RonMenu"){ BannerImagePath = "ron.png",OpenTime = 4, CloseTime = 22, ScannerFilePath = "01_specific_location\\0x0011827A.wav" },
            new GasStation(new Vector3(1211.169f, -1388.923f, 35.3769f), 180.4454f, "Ron", "Put RON in your tank","RonMenu"){ BannerImagePath = "ron.png",OpenTime = 4, CloseTime = 22, ScannerFilePath = "01_specific_location\\0x0723E151.wav" },
            
            new GasStation(new Vector3(-531.5529f, -1220.763f, 18.455f), 347.6858f, "Xero", "We have Xero gasoline","XeroMenu"){ OpenTime = 0, CloseTime = 24,BannerImagePath = "xero.png", ScannerFilePath = "01_specific_location\\0x04510C42.wav" },
            new GasStation(new Vector3(289.5112f, -1266.584f, 29.44076f), 92.24692f, "Xero", "We have Xero gasoline","XeroMenu"){ OpenTime = 0, CloseTime = 24,BannerImagePath = "xero.png" },
            new GasStation(new Vector3(-92.79028f, 6409.667f, 31.64035f), 48.08112f, "Xero", "We have Xero gasoline","XeroMenu"){ OpenTime = 0, CloseTime = 24,BannerImagePath = "xero.png", ScannerFilePath = "01_specific_location\\0x1B81EF89.wav" },
            new GasStation(new Vector3(46.75933f, 2789.635f, 58.10043f), 139.5097f, "Xero", "We have Xero gasoline","XeroMenu"){ OpenTime = 0, CloseTime = 24,BannerImagePath = "xero.png", ScannerFilePath = "01_specific_location\\0x18C6F152.wav" },

            new GasStation(new Vector3(1705.88f, 6425.68f, 33.37f), 153.9039f, "Globe Oil", "Changing the Climate, Powering The Future","GasStationMenu") {ScannerFilePath = "01_specific_location\\0x007AC3FC.wav" },
            


            new GasStation(new Vector3(160.4977f,6635.249f,31.61175f), 70.88637f, "Dons Country Store & Gas","Country Manners!","GasStationMenu"),
            new GasStation(new Vector3(266.2746f,2599.669f,44.7383f), 231.9223f, "Harmony General Store & Gas","Always in Harmony!","GasStationMenu"),
            new GasStation(new Vector3(1039.753f,2666.26f,39.55253f), 143.6208f, "Grande Senora Cafe & Gas","Extra Grande!","GasStationMenu"),
            new GasStation(new Vector3(2001.239f, 3779.786f, 32.18078f), 208.5214f, "Sandy's Gas", "And Full Service!","GasStationMenu"){ OpenTime = 4, CloseTime = 22 },
            new GasStation(new Vector3(646.0997f, 267.417f, 103.2494f), 58.99448f, "Globe Oil", "Changing the Climate, Powering The Future","GasStationMenu"),
            new GasStation(new Vector3(-342.2267f, -1475.199f, 30.74949f), 265.7801f, "Globe Oil", "Changing the Climate, Powering The Future","GasStationMenu"),
            new GasStation(new Vector3(1776.308f, 3327.483f, 41.43329f), 328.0875f, "Flywheels Gas", "Gas And More","GasStationMenu"),
            new GasStation(new Vector3(1201.978f, 2654.854f, 37.85188f), 315.5364f, "Route 68 Store", "Right off historic Route 68","GasStationMenu"),

        };
        List<Bar> Bars = new List<Bar>()
        {
            new Bar(new Vector3(224.5178f, 336.3819f, 105.5973f), 340.0694f, "Pitchers", "","BarMenu") { OpenTime = 0, CloseTime = 24 } ,
            new Bar(new Vector3(219.5508f, 304.9488f, 105.5861f), 250.1051f, "Singletons", "","BarMenu") { OpenTime = 0, CloseTime = 24 } ,
            new Bar(new Vector3(1982.979f, 3053.596f, 47.21508f), 226.3188f, "Yellow Jacket Inn", "","BarMenu") { OpenTime = 0, CloseTime = 24,VendorPosition =new Vector3(1982.979f, 3053.596f, 47.21508f), VendorHeading = 226.3188f, ScannerFilePath = "01_specific_location\\0x02C36B8B.wav"  },
            new Bar(new Vector3(-262.8396f, 6291.08f, 31.49327f), 222.9271f, "The Bay Bar", "","BarMenu") { OpenTime = 0, CloseTime = 24 } ,
            new Bar(new Vector3(-576.9105f, 239.0964f, 82.63644f), 354.0043f, "The Last Resort", "","BarMenu") { OpenTime = 0, CloseTime = 24 } ,
            new Bar(new Vector3(255.3016f, -1013.603f, 29.26964f), 70.28053f, "Shenanigan's Bar", "","BarMenu") { OpenTime = 0, CloseTime = 24 } ,
            new Bar(new Vector3(1218.175f, -416.5078f, 67.78294f), 74.95883f, "Mirror Park Tavern", "","BarMenu") { OpenTime = 0, CloseTime = 24 } ,
            new Bar(new Vector3(-1388.5f, -586.6741f, 30.21859f), 31.53231f, "Bahama Mama's", "","BarMenu") { OpenTime = 0, CloseTime = 24,VendorPosition = new Vector3(-1391.372f, -605.995f, 30.31955f), VendorHeading = 116.404f, InteriorID = 107778,  VendorModels = new List<string>() {"s_f_y_clubbar_01","s_m_y_clubbar_01","a_f_y_clubcust_01" } },//TeleportEnterPosition = new Vector3(-1387.984f, -587.4419f, 30.31951f), TeleportEnterHeading = 210.6985f,
            new Bar(new Vector3(-564.6519f, 276.2436f, 83.12064f), 175.5771f,"Tequila-La", "","BarMenu") { OpenTime = 0, CloseTime = 24,VendorPosition = new Vector3(-561.9947f, 284.9062f, 82.17636f), VendorHeading = 262.2369f, InteriorID = 72706, VendorModels = new List<string>() {"s_f_y_clubbar_01","s_m_y_clubbar_01","a_f_y_clubcust_01" } },//need better coordinates
            

        };
        List<FoodStand> FoodStands = new List<FoodStand>()
        {
            new FoodStand(new Vector3(403.3527f, 106.0655f, 101.4575f), 241.199f, "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ VendorPosition = new Vector3(403.3527f, 106.0655f, 101.4575f), VendorHeading = 241.199f, BannerImagePath = "beefybills.png" },
            new FoodStand(new Vector3(245.8918f, 161.5893f, 104.9487f), 3.803493f, "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ VendorPosition = new Vector3(245.8918f, 161.5893f, 104.9487f), VendorHeading = 3.803493f, BannerImagePath = "beefybills.png" },
            new FoodStand(new Vector3(-1268.011f, -1432.715f, 4.353373f), 134.2259f, "Beefy Bills Burger Bar","Extra BEEFY!","BeefyBillsMenu"){ VendorPosition = new Vector3(-1268.011f, -1432.715f, 4.353373f), VendorHeading =  134.2259f, BannerImagePath = "beefybills.png" },
            new FoodStand(new Vector3(-1232.426f, -1485.006f, 4.362638f), 137.5475f, "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ VendorPosition = new Vector3(-1232.426f, -1485.006f, 4.362638f), VendorHeading = 137.5475f, BannerImagePath = "beefybills.png" },
            new FoodStand(new Vector3(821.2138f, -2977.05f, 6.02066f), 272.7679f, "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ VendorPosition = new Vector3(821.2138f, -2977.05f, 6.02066f), VendorHeading = 272.7679f, BannerImagePath = "beefybills.png" },


             new FoodStand(new Vector3(-1693.241f, -1073.102f, 13.25018f), 47.97861f, "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ VendorPosition = new Vector3(-1693.241f, -1073.102f, 13.25018f), VendorHeading = 47.97861f, BannerImagePath = "beefybills.png" },
              new FoodStand(new Vector3(-1784.236f, -1175.884f, 13.01774f), 55.52537f, "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ VendorPosition = new Vector3(-1784.236f, -1175.884f, 13.01774f), VendorHeading = 55.52537f, BannerImagePath = "beefybills.png" },
               new FoodStand(new Vector3(-1857.076f, -1225.12f, 13.01722f), 316.8728f, "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ VendorPosition = new Vector3(-1857.076f, -1225.12f, 13.01722f), VendorHeading = 316.8728f, BannerImagePath = "beefybills.png" },

            new FoodStand(new Vector3(240.8329f, 167.2296f, 105.0605f), 167.5996f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ VendorPosition = new Vector3(240.8329f, 167.2296f, 105.0605f), VendorHeading = 167.5996f, BannerImagePath = "chihuahuahotdogs.png" },
            new FoodStand(new Vector3(-1516.382f, -952.5892f, 9.278718f), 317.7292f, "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu") { VendorPosition = new Vector3(-1516.382f, -952.5892f, 9.278718f), VendorHeading = 317.7292f, BannerImagePath = "chihuahuahotdogs.png" },
            new FoodStand(new Vector3(1604.818f, 3822.332f, 34.69806f), 200.7076f, "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ VendorPosition = new Vector3(1607.818f, 3822.332f, 34.69806f), VendorHeading = 200.7076f, BannerImagePath = "chihuahuahotdogs.png" },
            new FoodStand(new Vector3(-1248.932f, -1474.449f, 4.277946f), 306.3787f, "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ VendorPosition = new Vector3(-1248.932f, -1474.449f, 4.277946f), VendorHeading = 306.3787f, BannerImagePath = "chihuahuahotdogs.png" },
            new FoodStand(new Vector3(821.8197f, -2973.398f, 6.020657f), 276.5136f, "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ VendorPosition = new Vector3(821.8197f, -2973.398f, 6.020657f), VendorHeading = 276.5136f , BannerImagePath = "chihuahuahotdogs.png" },
            new FoodStand(new Vector3(-1219.656f, -1504.36f, 4.36032f), 98.7149f, "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ VendorPosition = new Vector3(-1219.656f, -1504.36f, 4.36032f), VendorHeading = 98.7149f, BannerImagePath = "chihuahuahotdogs.png" },
            //new Vector3(-1857.076f, -1225.12f, 13.01722f), 316.8728f

            new FoodStand(new Vector3(-1834.99f, -1234.289f, 13.01727f), 38.8311f, "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ VendorPosition = new Vector3(-1834.99f, -1234.289f, 13.01727f), VendorHeading = 38.8311f, BannerImagePath = "chihuahuahotdogs.png" },
            new FoodStand(new Vector3(-1772.159f, -1160.8f, 13.01804f), 50.64023f, "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ VendorPosition = new Vector3(-1772.159f, -1160.8f, 13.01804f), VendorHeading = 50.64023f, BannerImagePath = "chihuahuahotdogs.png" },
            new FoodStand(new Vector3(-1719.628f, -1103.968f, 13.01766f), 37.79702f, "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ VendorPosition = new Vector3(-1719.628f, -1103.968f, 13.01766f), VendorHeading = 37.79702f, BannerImagePath = "chihuahuahotdogs.png" },



            //new FoodStand(new Vector3(821.7623f, -2973.566f, 6.020659f), 269.9576f, "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ VendorPosition = new Vector3(821.7623f, -2973.566f, 6.020659f), VendorHeading = 269.9576f, BannerImagePath = "chihuahuahotdogs.png" },
            new FoodStand(new Vector3(2106.954f, 4947.911f, 40.95187f), 319.9109f, "Attack-A-Taco", "Heavy Shelling!","TacoFarmerMenu") {VendorPosition = new Vector3(2106.954f, 4947.911f, 40.95187f), VendorHeading = 319.9109f, },
            new FoodStand(new Vector3(-1148.969f, -1601.963f, 4.390241f), 35.73399f, "Gyro Day", "Gyro Day","GenericMenu") { VendorPosition = new Vector3(-1145.969f, -1602.963f, 4.390241f), VendorHeading = 35.73399f, },
            new FoodStand(new Vector3(1604.578f, 3828.483f, 34.4987f), 142.3778f, "Tough Nut Donut", "Our DoNuts are Crazy!","DonutMenu"){ VendorPosition = new Vector3(1604.578f, 3828.483f, 34.4987f) , VendorHeading = 142.3778f, },
            new FoodStand(new Vector3(1087.509f, 6510.788f, 21.0551f), 185.487f, "Roadside Fruit", "Should Be OK To Eat","FruitMenu") { VendorPosition = new Vector3(1087.509f, 6510.788f, 21.0551f), VendorHeading = 185.487f, },
            new FoodStand(new Vector3(2526.548f, 2037.936f, 19.82413f), 263.8982f, "Roadside Fruit", "Should Be OK To Eat","FruitMenu") { VendorPosition = new Vector3(2526.548f, 2037.936f, 19.82413f), VendorHeading = 263.8982f, },
            new FoodStand(new Vector3(1263.013f, 3548.566f, 35.14751f), 187.8834f, "Roadside Fruit", "Should Be OK To Eat","FruitMenu") { VendorPosition = new Vector3(1263.013f, 3548.566f, 35.14751f), VendorHeading =  187.8834f, },
            new FoodStand(new Vector3(1675.873f, 4883.532f, 42.06379f), 57.34329f, "Grapeseed Fruit", "Grapeseed Fruit","FruitMenu") { VendorPosition = new Vector3(1675.873f, 4883.532f, 42.06379f), VendorHeading = 57.34329f, },
            new FoodStand(new Vector3(-462.6676f, 2861.85f, 34.90421f), 162.4888f, "Roadside Fruit", "Roadside Fruit","FruitMenu") { VendorPosition = new Vector3(-462.6676f, 2861.85f, 34.90421f), VendorHeading = 162.4888f, },
        };
        List<Dealership> Dealerships = new List<Dealership>()
        {
            new Dealership(new Vector3(-69.16984f, 63.42498f, 71.89044f), 150.3918f, "Benefactor/Gallivanter", "Take control","BenefactorGallavanterMenu") { BannerImagePath = "benefactorgallivanter.png",
                CameraPosition = new Vector3(231.7523f, -993.08f, -97.99996f), CameraDirection = new Vector3(-0.9534805f, 0.1368595f, -0.2685973f), CameraRotation = new Rotator(-15.58081f, 0f, 81.83174f),
                ItemPreviewPosition = new Vector3(226.205f, -992.613f, -98.99996f), ItemPreviewHeading = 177.2006f,
                ItemDeliveryLocations = new List<SpawnPlace>() { 
                    new SpawnPlace(new Vector3(-83.40893f, 80.80059f, 71.08399f),150.8571f),
                    new SpawnPlace(new Vector3(-91.7436f, 80.85535f, 70.77888f), 332.7139f),
                    new SpawnPlace(new Vector3(-85.95417f, 92.15825f, 71.52161f), 335.7605f),
                    new SpawnPlace(new Vector3(-91.628f, 94.93055f, 71.50634f), 336.9444f),
                    new SpawnPlace(new Vector3(-94.94605f, 96.75835f, 71.55168f), 334.6241f),
                    new SpawnPlace(new Vector3(-97.94375f, 98.12598f, 71.58062f), 333.6715f),
                    new SpawnPlace(new Vector3(-101.6502f, 99.49832f, 71.58549f), 337.4208f),
                    new SpawnPlace(new Vector3(-106.9904f, 89.61737f, 70.6558f), 330.9741f),
                    new SpawnPlace(new Vector3(-100.8206f, 86.50267f, 70.71719f), 329.1442f),
                    new SpawnPlace(new Vector3(-97.96925f, 84.45586f, 70.71719f), 333.292f),
                    new SpawnPlace(new Vector3(-76.91218f, 94.65022f, 72.13159f), 249.3525f),
                    new SpawnPlace(new Vector3(-62.50578f, 88.61669f, 72.6437f), 247.8105f),
                } },

            new Dealership(new Vector3(-176.7741f, -1158.648f, 23.81366f), 359.6327f, "Vapid of Los Santos", "Low quality mass produced vehicles","VapidMenu") { BannerImagePath = "vapid.png"
                ,CameraPosition = new Vector3(231.7523f, -993.08f, -97.99996f), CameraDirection = new Vector3(-0.9534805f, 0.1368595f, -0.2685973f), CameraRotation = new Rotator(-15.58081f, 0f, 81.83174f),
                ItemPreviewPosition = new Vector3(226.205f, -992.613f, -98.99996f), ItemPreviewHeading = 177.2006f,
                ItemDeliveryLocations = new List<SpawnPlace>() { 
                    new SpawnPlace(new Vector3(-223.3041f, -1166.967f, 22.99067f), 347.7626f),

                    new SpawnPlace(new Vector3(-237.1176f, -1176.55f, 22.12655f), 322.7791f),
                    new SpawnPlace(new Vector3(-237.2399f, -1165.014f, 22.14342f), 272.7086f),
                    new SpawnPlace(new Vector3(-224.3428f, -1177.964f, 22.16829f), 13.04972f),
                } },

            new Dealership(new Vector3(286.8117f, -1148.615f, 29.29189f), 0.5211872f,"Sanders Motorcycles", "Feeling Old? Buy a Bike!","SandersMenu") { BannerImagePath = "sanders.png", ScannerFilePath = "01_specific_location\\0x16677E71.wav"
                ,CameraPosition = new Vector3(231.7523f, -993.08f, -97.99996f), CameraDirection = new Vector3(-0.9534805f, 0.1368595f, -0.2685973f), CameraRotation = new Rotator(-15.58081f, 0f, 81.83174f),
                ItemPreviewPosition = new Vector3(226.205f, -992.613f, -98.99996f), ItemPreviewHeading = 177.2006f,
                ItemDeliveryLocations = new List<SpawnPlace>() { 
                    new SpawnPlace(new Vector3(259.3008f, -1150.141f, 29.29169f), 358.0416f),

                    new SpawnPlace(new Vector3(262.3277f, -1161.748f, 28.36098f), 177.7536f),
                    new SpawnPlace(new Vector3(262.2397f, -1149.571f, 28.46074f), 180.2331f),
                    new SpawnPlace(new Vector3(256.4698f, -1161.971f, 28.34791f), 175.3772f),
                    new SpawnPlace(new Vector3(256.4955f, -1150.385f, 28.4598f), 181.1595f),
                    new SpawnPlace(new Vector3(250.4825f, -1161.918f, 28.31437f), 175.5457f),
                    new SpawnPlace(new Vector3(250.2824f, -1150.506f, 28.44035f), 177.726f),
                    new SpawnPlace(new Vector3(247.0185f, -1162.408f, 28.32439f), 175.2579f),
                } },

            new Dealership(new Vector3(-247.2263f, 6213.266f, 31.93902f), 143.0866f, "Helmut's European Autos", "Only the best eurotrash","HelmutMenu") { BannerImagePath = "helmut.png"
                ,CameraPosition = new Vector3(231.7523f, -993.08f, -97.99996f), CameraDirection = new Vector3(-0.9534805f, 0.1368595f, -0.2685973f), CameraRotation = new Rotator(-15.58081f, 0f, 81.83174f),
                ItemPreviewPosition = new Vector3(226.205f, -992.613f, -98.99996f), ItemPreviewHeading = 177.2006f,
                ItemDeliveryLocations = new List<SpawnPlace>() { 
                    new SpawnPlace(new Vector3(-214.548f, 6195.725f, 31.48873f), 314.937f),

                    new SpawnPlace(new Vector3(-229.8454f, 6197.135f, 30.65802f), 312.3795f),
                    new SpawnPlace(new Vector3(-240.5559f, 6199.209f, 30.65911f), 320.0063f),
                    new SpawnPlace(new Vector3(-245.7337f, 6203.578f, 30.65917f), 312.8283f),
                    new SpawnPlace(new Vector3(-247.8804f, 6205.941f, 30.65889f), 317.6494f),
                    new SpawnPlace(new Vector3(-204.6075f, 6204.576f, 30.65833f), 312.9748f),
                } },

            new Dealership(new Vector3(-38.83289f, -1108.61f, 26.46652f), 158.283f, "Premium Deluxe Motorsport", "Let's make a deal","PremiumDeluxeMenu") { BannerImagePath = "pdx.png", ScannerFilePath = "01_specific_location\\0x122B5EFF.wav",
                CameraPosition = new Vector3(-46.13059f, -1103.091f, 27.9145f), CameraDirection = new Vector3(0.3461686f, 0.9154226f, -0.2053503f), CameraRotation = new Rotator(-11.85001f, -8.374705E-05f, -20.7142f),
                ItemPreviewPosition = new Vector3(-43.94203f, -1096.923f, 26.44f), ItemPreviewHeading = 165.1469f,InteriorID = 7170,
                ItemDeliveryLocations = new List<SpawnPlace>() { 
                    new SpawnPlace(new Vector3(-56.35966f, -1116.532f, 26.4349f), 2.403779f),

                    new SpawnPlace(new Vector3(-57.66466f, -1107.103f, 25.60543f), 65.5113f),
                    new SpawnPlace(new Vector3(-47.87741f, -1115.788f, 25.60351f), 3.64787f),
                    new SpawnPlace(new Vector3(-50.5764f, -1116.585f, 25.60393f), 0.4485914f),
                    new SpawnPlace(new Vector3(-53.53827f, -1115.804f, 25.60398f), 1.730222f),
                    new SpawnPlace(new Vector3(-58.79285f, -1116.476f, 25.60338f), 2.648925f),
                    new SpawnPlace(new Vector3(-48.28887f, -1078.742f, 25.9386f), 260.1963f),
                    new SpawnPlace(new Vector3(-15.29717f, -1094.516f, 25.84302f), 173.1287f),
                } },

            new Dealership(new Vector3(-802.8875f, -223.7307f, 37.21824f), 117.6851f, "Luxury Autos", "You sure you can afford this?","LuxuryAutosMenu") {BannerImagePath = "luxuryautos.png",
                CameraPosition = new Vector3(231.7523f, -993.08f, -97.99996f), CameraDirection = new Vector3(-0.9534805f, 0.1368595f, -0.2685973f), CameraRotation = new Rotator(-15.58081f, 0f, 81.83174f),
                ItemPreviewPosition = new Vector3(226.205f, -992.613f, -98.99996f), ItemPreviewHeading = 177.2006f,
                ItemDeliveryLocations = new List<SpawnPlace>() { 
                    new SpawnPlace(new Vector3(-768.5347f, -245.1849f, 37.2452f), 197.8712f),

                    new SpawnPlace(new Vector3(-811.3596f, -221.3954f, 36.38088f), 30.76537f),
                    new SpawnPlace(new Vector3(-803.5298f, -234.8655f, 36.26606f), 29.17092f),
                    new SpawnPlace(new Vector3(-755.7492f, -242.3515f, 36.36629f), 281.1064f),
                    new SpawnPlace(new Vector3(-760.1515f, -232.0841f, 36.45313f), 204.9191f),
                } },

            new Dealership(new Vector3(1224.667f, 2728.353f, 38.00491f), 181.2344f, "Larry's RV Sales", "Need to disappear for a while?", "LarrysRVMenu"){BannerImagePath = "larrysrv.png",
                CameraPosition = new Vector3(231.7523f, -993.08f, -97.99996f), CameraDirection = new Vector3(-0.9534805f, 0.1368595f, -0.2685973f), CameraRotation = new Rotator(-15.58081f, 0f, 81.83174f),
                ItemPreviewPosition = new Vector3(226.205f, -992.613f, -98.99996f), ItemPreviewHeading = 177.2006f,

             //CameraPosition = new Vector3(1224.612f, 2711.322f, 39.84733f), CameraDirection = new Vector3(0.03426333f, 0.9912365f, -0.1275782f), CameraRotation = new Rotator(-7.329671f, 2.152019E-07f, -1.979712f),

              //  ItemPreviewPosition = new Vector3(1224.737f, 2720.088f, 37.65635f), ItemPreviewHeading = 90.58456f,



                ItemDeliveryLocations = new List<SpawnPlace>() { 
                    new SpawnPlace(new Vector3(1236.887f, 2709.858f, 38.00579f), 201.5402f),

                    new SpawnPlace(new Vector3(1246.11f, 2712.359f, 37.17435f), 200.71f),
                    new SpawnPlace(new Vector3(1232.931f, 2699.977f, 37.18208f), 94.271f),
                    new SpawnPlace(new Vector3(1210.044f, 2706.118f, 37.17501f), 173.5784f),
                    new SpawnPlace(new Vector3(1254.536f, 2692.278f, 36.79386f), 224.2466f),
                } },

        };
        List<DriveThru> DriveThrus = new List<DriveThru>()
        {
            new DriveThru(new Vector3(95.41846f, 285.0295f, 110.2042f), 251.8247f, "Up-N-Atom Burger", "Never Frozen, Often Microwaved","UpNAtomMenu") {OpenTime = 0, CloseTime = 24, BannerImagePath = "upnatom.png"},
            new DriveThru(new Vector3(15.48935f, -1595.832f, 29.28254f), 319.2816f, "The Taco Farmer", "Open All Hours!","TacoFarmerMenu") ,
            new DriveThru(new Vector3(-576.9321f, -880.5195f, 25.70123f), 86.01214f, "Lucky Plucker", "Come be a real Lucky Plucker","GenericMenu") {ScannerFilePath = "01_specific_location\\0x14B8A4DB.wav" } ,
            new DriveThru(new Vector3(2591.213f, 478.8892f, 108.6423f), 270.9569f, "Bishop's Chicken", "Our chicken is a religious experience","GenericMenu"),
            new DriveThru(new Vector3(144.34f, -1541.275f, 28.36799f), 139.819f, "La Vaca Loca", "Whats wrong with a few mad cows?","BeefyBillsMenu"),
            new DriveThru(new Vector3(145.3499f, -1460.568f, 28.71129f), 49.75111f,  "Lucky Plucker", "Come be a real Lucky Plucker","GenericMenu"),
            new DriveThru(new Vector3(1256.509f, -357.1387f, 68.52029f), 347.8622f, "Horny's Burgers", "The beef with the bone!","BeefyBillsMenu"),

        };
        List<ClothingShop> ClothingShops = new List<ClothingShop>()
        {
                new ClothingShop(new Vector3(430.0404f, -804.3267f, 29.49115f), 359.4608f, "Binco Textile City","Low-quality clothing at low prices.","LiquorStoreMenu",new Vector3(430.0404f, -804.3267f, 29.49115f))
                {
                    IsEnabled = false,
                    VendorModels = new List<string>() { "s_f_y_shop_low" }, VendorPosition = new Vector3(427.1392f, -806.624f, 29.49114f), VendorHeading = 78.23051f,
                    OpenTime = 4, CloseTime = 22,
                    InteriorID = 22786,
                },
                new ClothingShop(new Vector3(618.1533f, 2749.669f, 42.08868f), 181.2038f, "Suburban Harmony","Low-quality clothing at low prices.","LiquorStoreMenu",new Vector3(618.1533f, 2749.669f, 42.08868f))
                {
                    IsEnabled = false,
                    //VendorModels = new List<string>() { "s_f_y_shop_low" }, VendorPosition = new Vector3(427.1392f, -806.624f, 29.49114f), VendorHeading = 78.23051f,
                    OpenTime = 4, CloseTime = 22,
                    InteriorID = 96258,
                },
                new ClothingShop(new Vector3(-837.588f, -161.6364f, 37.90956f), 0f, "Didier Sachs","Fashion never cost so much","",Vector3.Zero) { OpenTime = 8, CloseTime = 20, IsTemporarilyClosed = true, ScannerFilePath = "01_specific_location\\0x0092CBCB.wav"},//rockford hills
                new ClothingShop(new Vector3(-717.36f, -157.29f, 38.2f), 117.6851f, "Ponsonbys","Catering to the Elite","",Vector3.Zero) { OpenTime = 8, CloseTime = 20, IsTemporarilyClosed = true, ScannerFilePath = "01_specific_location\\0x0289F802.wav"},//rockford hills
        };
        List<BusStop> BusStops = new List<BusStop>()
        {

                new BusStop(new Vector3(355.6272f, -1064.027f, 28.86697f), 270.2965f, "La Mesa Bus Stop 1","") { OpenTime = 0, CloseTime = 24,IsEnabled = false },

                new BusStop(new Vector3(-107.1481f, -1687.019f, 28.4079f), 141.5815f, "Chamberlain Hills Stop 1","") { OpenTime = 0, CloseTime = 24,IsEnabled = false },

                new BusStop(new Vector3(307.3152f, -766.6166f, 29.24787f), 155.4713f, "Pillbox Hill Bus Stop 1","") { OpenTime = 0, CloseTime = 24,IsEnabled = false },
                new BusStop(new Vector3(-251.2524f, -882.9617f, 29.76333f), 249.6444f, "Pillbox Hill Bus Stop 2","") { OpenTime = 0, CloseTime = 24,IsEnabled = false },
                new BusStop(new Vector3(-271.6416f, -824.817f, 30.89726f), 340.778f, "Pillbox Hill Bus Stop 3","") { OpenTime = 0, CloseTime = 24,IsEnabled = false },
                new BusStop(new Vector3(-243.9823f, -712.2824f, 32.61737f), 160.4338f, "Pillbox Hill Bus Stop 4","") { OpenTime = 0, CloseTime = 24,IsEnabled = false },
                new BusStop(new Vector3(117.3493f, -784.5579f, 31.25886f), 69.6745f, "Pillbox Hill Bus Stop 5","") { OpenTime = 0, CloseTime = 24,IsEnabled = false },


                new BusStop(new Vector3(-506.5057f, -667.5629f, 32.20013f), 269.3689f, "Little Seoul Bus Stop 1","") { OpenTime = 0, CloseTime = 24,IsEnabled = false },
                new BusStop(new Vector3(-692.9445f, -667.7023f, 29.95664f), 267.5733f, "Little Seoul Bus Stop 2","") { OpenTime = 0, CloseTime = 24,IsEnabled = false },

                new BusStop(new Vector3(-1213.239f, -1216.875f, 6.736316f), 191.0029f, "Vespucci Beach Bus Stop 1","") { OpenTime = 0, CloseTime = 24,IsEnabled = false },
                new BusStop(new Vector3(-1168.73f, -1471.557f, 3.438046f), 215.5927f, "Vespucci Beach Bus Stop 2","") { OpenTime = 0, CloseTime = 24,IsEnabled = false },

                new BusStop(new Vector3(-1421.381f, -87.43785f, 51.512f), 298.6868f, "Richman Bus Stop 1","") { OpenTime = 0, CloseTime = 24,IsEnabled = false },
                new BusStop(new Vector3(-645.8302f, -139.4489f, 36.86523f), 31.0542f, "Rockford Hills Bus Stop 2","") { OpenTime = 0, CloseTime = 24,IsEnabled = false },
        };
        List<Morgue> Morgues = new List<Morgue>()
        {
            new Morgue(new Vector3(241.2085f, -1378.962f, 33.74176f), 140.41f,"Los Santos County Coroner Office", "","") {OpenTime = 0,CloseTime = 24, InteriorID = 60418,ScannerFilePath = "01_specific_location\\0x04F66C50.wav" },
        };
        List<Airport> Airports = new List<Airport>()
        {
            new Airport("LSIX",new Vector3(-1079.419f, -2726.025f, 14.39801f),330.0377f,"Los Santos Intl.","Los Santos International brings third-world service to first-world travel" +
            "~n~" +
            "~n~City: ~y~Los Santos~s~" +
            "~n~State: ~p~San Andreas~s~")
            {
                OpenTime = 0
                ,CloseTime = 24
                ,ArrivalPosition = new Vector3(-1042.862f, -2746.254f, 21.3594f)
                ,ArrivalHeading = 329.611f


                ,AirArrivalPosition = new Vector3(-2154.954f, -2662.385f, 81.9974f)
                ,AirArrivalHeading = 239.2646f

                ,ScannerFilePath = "01_areas\\0x05E7E888.wav"
                ,CameraPosition = new Vector3(-1047.655f, -2676.447f, 44.60207f), CameraDirection = new Vector3(-0.05335984f, -0.9665251f, -0.2509621f), CameraRotation = new Rotator(-14.53445f, 6.118878E-06f, 176.84f)
                ,CommercialFlights = new List<AirportFlight>()
                {
                    new AirportFlight("LDR",StaticStrings.AirHerlerCarrierID,"Relax on one of our state of the art jets and arrive in luxury. ~n~~n~Taxi service to downtown Ludendorff included.", 1500, 5),
                    new AirportFlight("LDR",StaticStrings.CaipiraAirwaysCarrierID,"Only three connections and 12 hours for a 5 hour flight! What else could you ask for? ~n~~n~Taxi service to downtown Ludendorff included.", 550, 12),

                    new AirportFlight("SFX",StaticStrings.SanFierroAirCarrierID,"When driving just isn't an option and your company is paying.", 350, 1),
                    new AirportFlight("SFX",StaticStrings.LosSantosAirCarrierID,"Experience the luxury of a small regional carriers lowest end fare.", 325, 2),

                    new AirportFlight("FIA",StaticStrings.FlyUSCarrierID,"Need to get FAR away? FlyUS and see the difference deregulation made!", 450, 6),
                    new AirportFlight("VCIA",StaticStrings.FlyUSCarrierID,"Nonstop service to the sunniest state in the nation!", 425, 7),
                    new AirportFlight("CPA",StaticStrings.AdiosAirlinesCarrierID,"We won't be the only ones saying 'Adios' to you!", 500, 10),

                    new AirportFlight("SSA",StaticStrings.LosSantosAirCarrierID,"Just a hop skip and a jump away, LSIX is proud to provide service to our rural neighbors to the north.", 200, 1),
                }
            },
            new Airport("SSA",new Vector3(1759.512f, 3298.777f, 41.95529f), 144.954f,"Sandy Shores Airstrip",
            "A beach town nowhere near the ocean where the town is a trailer park and the beach is a mixture of toxic sludge and pulverized fish bones." +
            "~n~" +
            "~n~City: ~y~Sandy Shores~s~" +
            "~n~State: ~p~San Andreas~s~")
            {
                OpenTime = 0
                ,CloseTime = 24
                ,ArrivalPosition = new Vector3(1759.512f, 3298.777f, 41.95529f)
                ,ArrivalHeading = 144.954f

                ,AirArrivalPosition = new Vector3(759.6899f, 2999.477f, 120.8867f)
                ,AirArrivalHeading = 290.1536f
                ,CameraPosition = new Vector3(1783.648f, 3279.038f, 53.9618f), CameraDirection = new Vector3(-0.9425761f, 0.2726336f, -0.1929279f), CameraRotation = new Rotator(-11.1237f, 6.525906E-07f, 73.86785f)
                ,CommercialFlights = new List<AirportFlight>()
                {
                    new AirportFlight("LSIX",StaticStrings.LosSantosAirCarrierID,"Need to get to the big city fast? Don't care about safety records? Feeling lucky? If you answered yes to all of these questions, welcome aboard!", 200, 1),
                }
            },
            new YanktonAiport("LDR",new Vector3(3153.898f, -4840.879f, 111.8725f),354.7703f,"Ludendorff Regional",
            "The best fish boiled in lye in all of North Yankton!" +
            "~n~" +
            "~n~City: ~y~Ludendorff~s~" +
            "~n~State: ~p~North Yankton~s~")
            {
                OpenTime = 0
                ,CloseTime = 24
                ,ArrivalPosition = new Vector3(3153.898f, -4840.879f, 111.8725f)
                ,ArrivalHeading = 354.7703f

                ,AirArrivalPosition = new Vector3(4538.156f, -5345.569f, 230.4282f)
                ,AirArrivalHeading = 43.45281f
                ,CameraPosition = new Vector3(3142.449f, -4831.813f, 118.558f), CameraDirection = new Vector3(0.5385267f, -0.7833802f, -0.3103296f), CameraRotation = new Rotator(-18.0791f, -1.796229E-06f, -145.4938f)
                ,RequestIPLs = new List<string>() {
                            "plg_01",
                            "prologue01",
                            "prologue01_lod",
                            "prologue01c",
                            "prologue01c_lod",
                            "prologue01d",
                            "prologue01d_lod",
                            "prologue01e",
                            "prologue01e_lod",
                            "prologue01f",
                            "prologue01f_lod",
                            "prologue01g",
                            "prologue01h",
                            "prologue01h_lod",
                            "prologue01i",
                            "prologue01i_lod",
                            "prologue01j",
                            "prologue01j_lod",
                            "prologue01k",
                            "prologue01k_lod",
                            "prologue01z",
                            "prologue01z_lod",
                            "plg_02",
                            "prologue02",
                            "prologue02_lod",
                            "plg_03",
                            "prologue03",
                            "prologue03_lod",
                            "prologue03b",
                            "prologue03b_lod",
                            "prologue03_grv_dug",
                            "prologue03_grv_dug_lod",
                            "prologue_grv_torch",
                            "plg_04",
                            "prologue04",
                            "prologue04_lod",
                            "prologue04b",
                            "prologue04b_lod",
                            "prologue04_cover",
                            "des_protree_end",
                            "des_protree_start",
                            "des_protree_start_lod",
                            "plg_05",
                            "prologue05",
                            "prologue05_lod",
                            "prologue05b",
                            "prologue05b_lod",
                            "plg_06",
                            "prologue06",
                            "prologue06_lod",
                            "prologue06b",
                            "prologue06b_lod",
                            "prologue06_int",
                            "prologue06_int_lod",
                            "prologue06_pannel",
                            "prologue06_pannel_lod",
                            "prologue_m2_door",
                            "prologue_m2_door_lod",
                            "plg_occl_00",
                            "prologue_occl",
                            "plg_rd",
                            "prologuerd",
                            "prologuerdb",
                            "prologuerd_lod",
                        }
                ,RemoveIPLs = new List<string>(){
                            "plg_01",
                            "prologue01",
                            "prologue01_lod",
                            "prologue01c",
                            "prologue01c_lod",
                            "prologue01d",
                            "prologue01d_lod",
                            "prologue01e",
                            "prologue01e_lod",
                            "prologue01f",
                            "prologue01f_lod",
                            "prologue01g",
                            "prologue01h",
                            "prologue01h_lod",
                            "prologue01i",
                            "prologue01i_lod",
                            "prologue01j",
                            "prologue01j_lod",
                            "prologue01k",
                            "prologue01k_lod",
                            "prologue01z",
                            "prologue01z_lod",
                            "plg_02",
                            "prologue02",
                            "prologue02_lod",
                            "plg_03",
                            "prologue03",
                            "prologue03_lod",
                            "prologue03b",
                            "prologue03b_lod",
                            "prologue03_grv_cov",
                            "prologue03_grv_cov_lod",
                            "prologue03_grv_dug",
                            "prologue03_grv_dug_lod",
                            "prologue03_grv_fun",
                            "prologue_grv_torch",
                            "plg_04",
                            "prologue04",
                            "prologue04_lod",
                            "prologue04b",
                            "prologue04b_lod",
                            "prologue04_cover",
                            "des_protree_end",
                            "des_protree_start",
                            "des_protree_start_lod",
                            "plg_05",
                            "prologue05",
                            "prologue05_lod",
                            "prologue05b",
                            "prologue05b_lod",
                            "plg_06",
                            "prologue06",
                            "prologue06_lod",
                            "prologue06b",
                            "prologue06b_lod",
                            "prologue06_int",
                            "prologue06_int_lod",
                            "prologue06_pannel",
                            "prologue06_pannel_lod",
                            "prologue_m2_door",
                            "prologue_m2_door_lod",
                            "plg_occl_00",
                            "prologue_occl",
                            "plg_rd",
                            "prologuerd",
                            "prologuerdb",
                            "prologuerd_lod",

                        }
                ,CommercialFlights = new List<AirportFlight>()
                {
                    new AirportFlight("LSIX",StaticStrings.CaipiraAirwaysCarrierID,"You'll get there when you get there", 650, 14),
                    new AirportFlight("SFX",StaticStrings.SanFierroAirCarrierID,"Its the San Fierro Treat!", 680, 15),
                }
                ,RoadToggels = new HashSet<RoadToggler>()
                {
                    new RoadToggler(new Vector3(5526.24f, -5137.23f, 61.78925f),new Vector3(3679.327f, -4973.879f, 125.0828f),192.0f),
                    new RoadToggler(new Vector3(3691.211f, -4941.24f, 94.59368f),new Vector3(3511.115f, -4689.191f, 126.7621f),16.0f),
                    new RoadToggler(new Vector3(3510.004f, -4865.81f, 94.69557f),new Vector3(3204.424f, -4833.8147f, 126.8152f),16.0f),
                    new RoadToggler(new Vector3(3186.534f, -4832.798f, 109.8148f),new Vector3(3204.187f, -4833.993f, 114.815f),16.0f),
                }
                ,ZonesToEnable = new HashSet<string>() { "PrLog" }
                ,StateLocation = StaticStrings.NorthYanktonStateID,
            },
            new CayoPericoAirport("CPA",new Vector3(),0f,"Cayo Perico Airstrip",
                "Have this crap instead of ~r~REAL~s~ DLC!" +
                "~n~" +
                "~n~City: ~y~Cayo Perico~s~" +
                "~n~Country: ~p~Colombia~s~") { IsEnabled = false,StateLocation = StaticStrings.ColombiaStateID },
            new Airport("SFX",new Vector3(),0f,"San Fierro Intl.",
                "The city of psychedelic wonders." +
                "~n~" +
                "~n~City: ~y~San Fierro~s~" +
                "~n~State: ~p~San Andreas~s~"){ IsEnabled = false,StateLocation = StaticStrings.SanAndreasStateID },
            new Airport("FIA",new Vector3(),0f,"Francis Intl.",
                "Great To Visit, Even Better To Leave" +
                "~n~" +
                "~n~City: ~y~Dukes, Liberty City~s~" +
                "~n~State: ~p~Liberty~s~") { IsEnabled = false,StateLocation = StaticStrings.LibertyStateID },
            new Airport("VCIA",new Vector3(),0f,"Escobar Intl.",
                "A city run by dealers and weasels" +
                "~n~" +
                "~n~City: ~y~Vice City~s~" +
                "~n~State: ~p~Florida~s~") { IsEnabled = false,StateLocation = StaticStrings.FloridaStateID },

            };

        /*
 *                     new AirportFlights("NYRA","Air Herler","For the utmost in luxury", 5000, 4),//shamal, high end regional
            new AirportFlights("NYRA","Caipira Airways","You'll get there when you get there", 599, 12),

            new AirportFlights("NYRA","San Fierro Air","Its the San Fierro Treat!", 599, 12),//regional low end
            new AirportFlights("NYRA","Air Emu","Take our birds anywhere you like", 599, 12),//shamal, high end regional

            new AirportFlights("NYRA","MyFly","Optimal Service, for a price", 599, 12),//shamal, high end regional
            new AirportFlights("NYRA","Adios Airlines","Say your goodbyes!", 599, 12),//low end, major carrier

            new AirportFlights("NYRA","FlyUS","Live A Little, Fly With US", 599, 12),//low end, major carrier
            new AirportFlights("NYRA","Los Santos Air","You'll get there when you get there", 599, 12),//regional low end*/
        PossibleLocations.DeadDrops.AddRange(DeadDrops);
        PossibleLocations.ScrapYards.AddRange(ScrapYards);
        PossibleLocations.CarCrushers.AddRange(CarCrushers);
        PossibleLocations.GangDens.AddRange(GangDens);
        PossibleLocations.GunStores.AddRange(GunStores);
        PossibleLocations.Hotels.AddRange(Hotels);
        PossibleLocations.Residences.AddRange(Residences);
        PossibleLocations.CityHalls.AddRange(CityHalls);
        PossibleLocations.PoliceStations.AddRange(PoliceStations);
        PossibleLocations.Hospitals.AddRange(Hospitals);
        PossibleLocations.FireStations.AddRange(FireStations);
        PossibleLocations.Restaurants.AddRange(Restaurants);
        PossibleLocations.Pharmacies.AddRange(Pharmacies);
        PossibleLocations.Dispensaries.AddRange(Dispensaries);
        PossibleLocations.HeadShops.AddRange(HeadShops);
        PossibleLocations.HardwareStores.AddRange(HardwareStores);
        PossibleLocations.PawnShops.AddRange(PawnShops);
        PossibleLocations.Landmarks.AddRange(Landmarks);
        PossibleLocations.BeautyShops.AddRange(BeautyShops);
        PossibleLocations.Banks.AddRange(Banks);
        PossibleLocations.ConvenienceStores.AddRange(ConvenienceStores);
        PossibleLocations.LiquorStores.AddRange(LiquorStores);
        PossibleLocations.GasStations.AddRange(GasStations);
        PossibleLocations.Bars.AddRange(Bars);
        PossibleLocations.FoodStands.AddRange(FoodStands);
        PossibleLocations.CarDealerships.AddRange(Dealerships);
        PossibleLocations.DriveThrus.AddRange(DriveThrus);
        PossibleLocations.ClothingShops.AddRange(ClothingShops);
        PossibleLocations.BusStops.AddRange(BusStops);
        PossibleLocations.Prisons.AddRange(Prisons);

        PossibleLocations.SubwayStations.AddRange(SubwayStations);
        PossibleLocations.Morgues.AddRange(Morgues);
        PossibleLocations.SportingGoodsStores.AddRange(SportingGoodsStores);
        PossibleLocations.Airports.AddRange(Airports);
        //PossibleLocations.YanktonAiports.AddRange(YanktonAiports);


        Serialization.SerializeParam(PossibleLocations, ConfigFileName);

        PossibleLocations OldPossibleLocations = new PossibleLocations();
        List<GangDen> GangDens2008 = new List<GangDen>();
        foreach(GangDen gd in GangDens)
        {
            if (gd.GangID != "AMBIENT_GANG_BALLAS" && gd.GangID != "AMBIENT_GANG_FAMILY" && gd.GangID != "AMBIENT_GANG_SALVA")
            {
                GangDens2008.Add(gd);
            }
        }
        GangDens2008.Add(new GangDen(new Vector3(393.403f, -782.4543f, 29.28772f), 269.1115f, "Ballas Den", "", "BallasDenMenu", "AMBIENT_GANG_BALLAS") { CanInteractWhenWanted = true, BannerImagePath = "ballas.png", OpenTime = 0, CloseTime = 24, IsEnabled = false, PossiblePedSpawns = new List<ConditionalLocation>() {

                new ConditionalLocation(new Vector3(395.0874f, -779.8005f, 29.29059f), 295.3088f, 50f),
                new ConditionalLocation(new Vector3(395.585f, -786.7841f, 29.28836f), 236.7289f, 50f),
                new ConditionalLocation(new Vector3(393.6364f, -771.2427f, 29.2868f), 321.6246f, 50f),
                new ConditionalLocation(new Vector3(385.2623f, -771.7565f, 29.2923f), 356.8999f, 50f),
                new ConditionalLocation(new Vector3(398.6558f, -788.6539f, 29.28695f), 214.5293f, 50f),
            } });
        GangDens2008.Add(new GangDen(new Vector3(86.11255f, -1959.272f, 21.12167f), 318.5057f, "The Families Den", "The OGs", "FamiliesDenMenu", "AMBIENT_GANG_FAMILY") { CanInteractWhenWanted = true,BannerImagePath = "families.png", OpenTime = 0, CloseTime = 24, IsEnabled = false, PossiblePedSpawns = new List<ConditionalLocation>() {

                new ConditionalLocation(new Vector3(84.76484f, -1953.536f, 20.8518f), 334.0088f, 50f),
                new ConditionalLocation(new Vector3(87.02995f, -1947.637f, 20.74858f), 303.2596f, 50f),
                new ConditionalLocation(new Vector3(95.30958f, -1954.979f, 20.75126f), 314.5049f, 50f),
                new ConditionalLocation(new Vector3(84.23887f, -1932.319f, 20.74922f), 19.71852f, 50f),
            } });//This is in DAVIS near Grove Street
        GangDens2008.Add(new GangDen(new Vector3(511.4065f, -1790.909f, 28.50743f), 90.88252f, "Varrios Los Aztecas Den", "","VarriosDenMenu", "AMBIENT_GANG_SALVA") { CanInteractWhenWanted = true,BannerImagePath = "varrios.png",OpenTime = 0,CloseTime = 24, IsEnabled = false, PossiblePedSpawns = new List<ConditionalLocation>() {

                new ConditionalLocation(new Vector3(511.2142f, -1794.088f, 28.50298f), 102.9549f, 50f),
                new ConditionalLocation(new Vector3(507.1f, -1787.592f, 28.4884f), 82.19876f, 50f),
                new ConditionalLocation(new Vector3(506.4553f, -1793.272f, 28.49493f), 68.23071f, 50f),
                new ConditionalLocation(new Vector3(503.4097f, -1782.823f, 28.49299f), 107.5499f, 50f),
                new ConditionalLocation(new Vector3(529.1724f, -1793.627f, 28.50298f), 148.2789f, 50f),
            } });//THIS IS IN RANCHO Near a motel and the davis polcie station?

        OldPossibleLocations.DeadDrops.AddRange(DeadDrops);
        OldPossibleLocations.ScrapYards.AddRange(ScrapYards);
        OldPossibleLocations.CarCrushers.AddRange(CarCrushers);
        OldPossibleLocations.GangDens.AddRange(GangDens2008);
        OldPossibleLocations.GunStores.AddRange(GunStores);
        OldPossibleLocations.Hotels.AddRange(Hotels);
        OldPossibleLocations.Residences.AddRange(Residences);
        OldPossibleLocations.CityHalls.AddRange(CityHalls);
        OldPossibleLocations.PoliceStations.AddRange(PoliceStations);
        OldPossibleLocations.Hospitals.AddRange(Hospitals);
        OldPossibleLocations.FireStations.AddRange(FireStations);
        OldPossibleLocations.Restaurants.AddRange(Restaurants);
        OldPossibleLocations.Pharmacies.AddRange(Pharmacies);
        //OldPossibleLocations.Dispensaries.AddRange(Dispensaries);//weed wasnt legal there?
        OldPossibleLocations.HeadShops.AddRange(HeadShops);
        OldPossibleLocations.HardwareStores.AddRange(HardwareStores);
        OldPossibleLocations.PawnShops.AddRange(PawnShops);
        OldPossibleLocations.Landmarks.AddRange(Landmarks);
        OldPossibleLocations.BeautyShops.AddRange(BeautyShops);
        OldPossibleLocations.Banks.AddRange(Banks);
        OldPossibleLocations.ConvenienceStores.AddRange(ConvenienceStores);
        OldPossibleLocations.LiquorStores.AddRange(LiquorStores);
        OldPossibleLocations.GasStations.AddRange(GasStations);
        OldPossibleLocations.Bars.AddRange(Bars);
        OldPossibleLocations.FoodStands.AddRange(FoodStands);
        OldPossibleLocations.CarDealerships.AddRange(Dealerships);
        OldPossibleLocations.DriveThrus.AddRange(DriveThrus);
        OldPossibleLocations.ClothingShops.AddRange(ClothingShops);
        OldPossibleLocations.BusStops.AddRange(BusStops);
        OldPossibleLocations.Prisons.AddRange(Prisons);
        OldPossibleLocations.SubwayStations.AddRange(SubwayStations);
        OldPossibleLocations.Morgues.AddRange(Morgues);
        OldPossibleLocations.SportingGoodsStores.AddRange(SportingGoodsStores);
        OldPossibleLocations.Airports.AddRange(Airports);
        Serialization.SerializeParam(OldPossibleLocations, "Plugins\\LosSantosRED\\AlternateConfigs\\LosSantos2008\\Locations_LosSantos2008.xml");

    }
    private void DefaultConfig_LibertyCity()
    {
        PossibleLocations LibertyCityLocations = new PossibleLocations();
        List<PoliceStation> PoliceStations = new List<PoliceStation>()
        {
            new PoliceStation(new Vector3(0f, 0f, 0f), 323.5287f, "LCPD Station", "LCPD") {OpenTime = 0,CloseTime = 24 } 
        };
        List<Hospital> Hospitals = new List<Hospital>() 
        {
            new Hospital(new Vector3(25f, 25f, 25f), 280.637f, "LC Hospital","LC Hosptial") { OpenTime = 0,CloseTime = 24 }, 
        };
        LibertyCityLocations.PoliceStations.AddRange(PoliceStations);
        LibertyCityLocations.Hospitals.AddRange(Hospitals);
        Serialization.SerializeParam(LibertyCityLocations, "Plugins\\LosSantosRED\\AlternateConfigs\\LibertyCity\\Locations_LibertyCity.xml");
    }
    public void Setup()
    {
        foreach(BasicLocation bl in AllLocations())
        {
            if(bl.HasBannerImage)
            {
                if(!File.Exists($"Plugins\\LosSantosRED\\images\\{bl.BannerImagePath}"))
                {
                    bl.BannerImagePath = "";
                    EntryPoint.WriteToConsole($"Locations ERROR Banner Image file DOES NOT EXIST {bl.Name}, REMOVING FROM LOCATION", 0);
                }
            }
        }
    }
}
