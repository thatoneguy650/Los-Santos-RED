using LosSantosRED.lsr.Helper;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


public class PlacesOfInterest_LibertyPP
{
    private PossibleLocations LibertyCityLocations;
    private PedCustomizerLocation DefaultPedCustomizerLocation;
    private PlacesOfInterest PlacesOfInterest;
    private List<Airport> Airports;
    private List<BodyExport> BodyExports;
    private List<Business> Businesses;
    private List<CarCrusher> CarCrushers;
    private List<DeadDrop> DeadDrops;
    private List<Dealership> Dealerships;
    private List<RaceMeetup> RaceMeetups;
    private List<RepairGarage> RepairGarages;
    private List<VehicleModShop> VehicleModShops;
    private List<VehicleExporter> VehicleExporters;

    public PlacesOfInterest_LibertyPP(PlacesOfInterest placesOfInterest)
    {
        PlacesOfInterest = placesOfInterest;
    }

    public void DefaultConfig()
    {
        LibertyCityLocations = new PossibleLocations();

        DefaultConfig_Airports();
        DefaultConfig_Banks();
        DefaultConfig_Bars();
        DefaultConfig_BarberShops();
        DefaultConfig_BlankLocations();
        DefaultConfig_BodyExports();
        DefaultConfig_Businesses();
        DefaultConfig_CarCrushers();
        DefaultConfig_CityHalls();
        DefaultConfig_ClothingShops();
        DefaultConfig_ConvenienceStores();
        DefaultConfig_Dealerships();
        DefaultConfig_DeadDrops();
        DefaultConfig_FoodStands();
        DefaultConfig_Forgers();
        DefaultConfig_FireStations();
        DefaultConfig_GamblingDens();
        DefaultConfig_GangDens();
        DefaultConfig_GasStations();
        DefaultConfig_GunStores();
        DefaultConfig_HardwareStores();
        DefaultConfig_HeadShops();
        DefaultConfig_Hospitals();
        DefaultConfig_Hotels();
        DefaultConfig_IllicitMarketplaces();
        DefaultConfig_Landmarks();
        DefaultConfig_LiquorStore();
        DefaultConfig_ModShops();
        DefaultConfig_PawnShops();
        DefaultConfig_PedCustomizeLocation();
        DefaultConfig_Pharmacies();
        DefaultConfig_PoliceStations();
        DefaultConfig_Prisons();
        DefaultConfig_RaceMeetups();
        DefaultConfig_RepairGarages();
        DefaultConfig_Residences();
        DefaultConfig_Restaurants();
        DefaultConfig_ScrapYards();
        DefaultConfig_Sports();
        DefaultConfig_SubwayStations();
        DefaultConfig_VehicleExporters();
        LibertyCityLocations.PedCustomizerLocation = DefaultPedCustomizerLocation;

        PossibleLocations lppLC = LibertyCityLocations.Copy();

        lppLC.PedCustomizerLocation = PlacesOfInterest.PossibleLocations.PedCustomizerLocation;

        foreach (GameLocation gl in PlacesOfInterest.InteractableLocations())
        {
            gl.AddLocation(lppLC);
        }
        Serialization.SerializeParam(lppLC, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LPPConfigFolder}\\Locations_{StaticStrings.LPPConfigSuffix}.xml");

    }

    private void DefaultConfig_Airports()
    {
        List<Airport> LCAirportList = new List<Airport>() {
            new Airport() {
                Name = "Francis Intl.",
                AirportID = "FIA",
                Description = "Great To Visit, Even Better To Leave~n~~n~City: ~y~Dukes, Liberty City~s~~n~State: ~p~Liberty~s~",
                EntrancePosition = new Vector3(7555.017f, -2851.004f, 6.08004f),
                EntranceHeading = 91.74f,
                OpenTime = 0,
                CloseTime = 24,
                ArrivalPosition = new Vector3(7546.63672f, -2928.6543f, 6.079805f),
                ArrivalHeading = 73.94f,
                BannerImagePath = "stores\\fia.png",
                CommercialFlights = new List<AirportFlight>()
                {
                    new AirportFlight("LDR",StaticStrings.AirHerlerCarrierID,"Relax on one of our state of the art jets and arrive in luxury. ~n~~n~Taxi service to downtown Ludendorff included.", 1500, 5),
                    new AirportFlight("LDR",StaticStrings.CaipiraAirwaysCarrierID,"Only three connections and 12 hours for a 5 hour flight! What else could you ask for? ~n~~n~Taxi service to downtown Ludendorff included.", 550, 12),
                },
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.Airports.AddRange(LCAirportList);
        List<YanktonAiport> YanktonAiportList = new List<YanktonAiport>() {
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
                ,BannerImagePath = "stores\\caipira.png"
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
                    new AirportFlight("FIA",StaticStrings.CaipiraAirwaysCarrierID,"You'll get there when you get there", 850, 14),
                }
                ,RoadToggels = new HashSet<RoadToggler>()
                {
                    new RoadToggler(new Vector3(5526.24f, -5137.23f, 61.78925f),new Vector3(3679.327f, -4973.879f, 125.0828f),192.0f),
                    new RoadToggler(new Vector3(3691.211f, -4941.24f, 94.59368f),new Vector3(3511.115f, -4689.191f, 126.7621f),16.0f),
                    new RoadToggler(new Vector3(3510.004f, -4865.81f, 94.69557f),new Vector3(3204.424f, -4833.8147f, 126.8152f),16.0f),
                    new RoadToggler(new Vector3(3186.534f, -4832.798f, 109.8148f),new Vector3(3204.187f, -4833.993f, 114.815f),16.0f),
                }
                ,ZonesToEnable = new HashSet<string>() { "PrLog" }
                ,StateID = StaticStrings.NorthYanktonStateID,
            },
        };
        LibertyCityLocations.Airports.AddRange(YanktonAiportList);
    }
    private void DefaultConfig_Banks()
    {
        //ATMs
        //Lombank
        ATMMachine Lombankatm1 = new ATMMachine(new Vector3(6125.629f, -3811.401f, 14.11966f), 2.570131f, "Lombank", "Our time is your money", "None", null, null)
        {
            BannerImagePath = "stores\\lombank.png",
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 0,
            CloseTime = 24,
        };
        LibertyCityLocations.ATMMachines.Add(Lombankatm1);
        ATMMachine Lombank4atm1 = new ATMMachine(new Vector3(4781.826f, -2066.17f, 14.75928f), 48.62358f, "Lombank", "Our time is your money", "None", null, null)
        {
            BannerImagePath = "stores\\lombank.png",
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 0,
            CloseTime = 24,
        };
        LibertyCityLocations.ATMMachines.Add(Lombank4atm1);
        ATMMachine Lombank4atm2 = new ATMMachine(new Vector3(4794.666f, -2053.22f, 14.75883f), 45.28886f, "Lombank", "Our time is your money", "None", null, null)
        {
            BannerImagePath = "stores\\lombank.png",
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 0,
            CloseTime = 24,
        };
        LibertyCityLocations.ATMMachines.Add(Lombank4atm2);

        // Bank Of Liberty
        ATMMachine bolatm1 = new ATMMachine(new Vector3(6346.622f, -3489.316f, 23.02382f), 178.4412f, "Bank Of Liberty", "Bleeding you dry", "None", null, null)
        {
            BannerImagePath = "stores\\lombank.png",
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 0,
            CloseTime = 24,
        };
        LibertyCityLocations.ATMMachines.Add(bolatm1);
        ATMMachine bolatm2 = new ATMMachine(new Vector3(6506.938f, -3283.181f, 28.24679f), 273.719f, "Bank Of Liberty", "Bleeding you dry", "None", null, null)
        {
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 0,
            CloseTime = 24,
        };
        LibertyCityLocations.ATMMachines.Add(bolatm2);
        ATMMachine bolatm3 = new ATMMachine(new Vector3(6386.239f, -2620.458f, 38.67291f), 1.147391f, "Bank Of Liberty", "Bleeding you dry", "None", null, null)
        {
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 0,
            CloseTime = 24,
        };
        LibertyCityLocations.ATMMachines.Add(bolatm3);

        //BOL Inside Interior
        ATMMachine bolbankatm1 = new ATMMachine(new Vector3(5161.468f, -3715.405f, 15.40719f), 3.777143f, "Bank Of Liberty", "Bleeding you dry", "None", null, null)
        {
            BannerImagePath = "stores\\lombank.png",
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 0,
            CloseTime = 24,
        };
        LibertyCityLocations.ATMMachines.Add(bolbankatm1);
        ATMMachine bolbankatm1a = new ATMMachine(new Vector3(5162.582f, -3715.386f, 15.40724f), 355.9296f, "Bank Of Liberty", "Bleeding you dry", "None", null, null)
        {
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 0,
            CloseTime = 24,
        };
        LibertyCityLocations.ATMMachines.Add(bolbankatm1a);
        ATMMachine bolbankatm1b = new ATMMachine(new Vector3(5163.613f, -3715.336f, 15.40724f), 0.5806472f, "Bank Of Liberty", "Bleeding you dry", "None", null, null)
        {
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 0,
            CloseTime = 24,
        };
        LibertyCityLocations.ATMMachines.Add(bolbankatm1b);
        ATMMachine bolbankatm2 = new ATMMachine(new Vector3(5161.456f, -3730.109f, 15.40718f), 182.2762f, "Bank Of Liberty", "Bleeding you dry", "None", null, null)
        {
            BannerImagePath = "stores\\lombank.png",
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 0,
            CloseTime = 24,
        };
        LibertyCityLocations.ATMMachines.Add(bolbankatm2);
        ATMMachine bolbankatm2a = new ATMMachine(new Vector3(5162.528f, -3730.087f, 15.40724f), 181.5201f, "Bank Of Liberty", "Bleeding you dry", "None", null, null)
        {
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 0,
            CloseTime = 24,
        };
        LibertyCityLocations.ATMMachines.Add(bolbankatm2a);
        ATMMachine bolbankatm2b = new ATMMachine(new Vector3(5163.552f, -3730.239f, 15.40724f), 179.72f, "Bank Of Liberty", "Bleeding you dry", "None", null, null)
        {
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 0,
            CloseTime = 24,
        };
        LibertyCityLocations.ATMMachines.Add(bolbankatm2b);
        ATMMachine bolbankatm3 = new ATMMachine(new Vector3(5170.833f, -3731.292f, 15.40718f), 177.6457f, "Bank Of Liberty", "Bleeding you dry", "None", null, null)
        {
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 0,
            CloseTime = 24,
        };
        LibertyCityLocations.ATMMachines.Add(bolbankatm3);

        //Hatton Gardons
        ATMMachine Hatbolatm1a = new ATMMachine(new Vector3(5240.616f, -2734.026f, 14.65816f), 271.1967f, "Bank Of Liberty", "Bleeding you dry", "None", null, null)
        {
            BannerImagePath = "stores\\lombank.png",
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 0,
            CloseTime = 24,
        };
        LibertyCityLocations.ATMMachines.Add(Hatbolatm1a);
        ATMMachine Hatbolatm1b = new ATMMachine(new Vector3(5240.601f, -2741.928f, 14.65844f), 270.1448f, "Bank Of Liberty", "Bleeding you dry", "None", null, null)
        {
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 0,
            CloseTime = 24,
        };
        LibertyCityLocations.ATMMachines.Add(Hatbolatm1b);
        ATMMachine Hatbolatm1c = new ATMMachine(new Vector3(5275.307f, -2733.964f, 13.22654f), 84.82664f, "Bank Of Liberty", "Bleeding you dry", "None", null, null)
        {
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 0,
            CloseTime = 24,
        };
        LibertyCityLocations.ATMMachines.Add(Hatbolatm1c);
        ATMMachine Hatbolatm1d = new ATMMachine(new Vector3(5275.318f, -2741.852f, 13.2272f), 87.66302f, "Bank Of Liberty", "Bleeding you dry", "None", null, null)
        {
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 0,
            CloseTime = 24,
        };
        LibertyCityLocations.ATMMachines.Add(Hatbolatm1d);

        //East Holland - Exeter Ave
        ATMMachine EHbolatm1a = new ATMMachine(new Vector3(4937.937f, -1893.804f, 25.12434f), 271.2054f, "Bank Of Liberty", "Bleeding you dry", "None", null, null)
        {
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 0,
            CloseTime = 24,
        };
        LibertyCityLocations.ATMMachines.Add(EHbolatm1a);
        ATMMachine EHbolatm1b = new ATMMachine(new Vector3(4938.177f, -1883.542f, 25.017f), 271.6325f, "Bank Of Liberty", "Bleeding you dry", "None", null, null)
        {
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 0,
            CloseTime = 24,
        };
        LibertyCityLocations.ATMMachines.Add(EHbolatm1b);

        // Banks
        // Bank of Liberty
        Bank BOL2 = new Bank(new Vector3(6338.047f, -3490.734f, 23.59051f), 0.02f, "Bank Of Liberty", "Bleeding you dry", "BOL")
        {
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 6,
            CloseTime = 20
        };
        LibertyCityLocations.Banks.Add(BOL2);

        Bank BOL3 = new Bank(new Vector3(6381.627f, -2613.024f, 38.78111f), 90.68f, "Bank Of Liberty", "Bleeding you dry", "BOL")
        {
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 6,
            CloseTime = 20
        };
        LibertyCityLocations.Banks.Add(BOL3);

        Bank BOL4 = new Bank(new Vector3(5273.201f, -2737.772f, 13.22551f), 267.4793f, "Bank Of Liberty", "Bleeding you dry", "BOL")
        {
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 6,
            CloseTime = 20
        };
        LibertyCityLocations.Banks.Add(BOL4);

        Bank BOL5 = new Bank(new Vector3(5033.66f, -3431.971f, 14.76172f), 358.6499f, "Bank Of Liberty", "Bleeding you dry", "BOL")
        {
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 6,
            CloseTime = 20
        };
        LibertyCityLocations.Banks.Add(BOL5);

        Bank BOL6 = new Bank(new Vector3(5201.84f, -3888.449f, 15.40521f), 132.1199f, "Bank Of Liberty", "Bleeding you dry", "BOL")
        {
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 6,
            CloseTime = 20
        };
        LibertyCityLocations.Banks.Add(BOL6);

        Bank BOL7 = new Bank(new Vector3(4938.436f, -1886.664f, 25.01589f), 88.66853f, "Bank Of Liberty", "Bleeding you dry", "BOL")
        {
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 6,
            CloseTime = 20
        };
        LibertyCityLocations.Banks.Add(BOL7);

        //Lombank
        Bank Lombank1 = new Bank(new Vector3(6138.155f, -3809.99f, 14.61777f), 135.8565f, "Lombank", "Our time is your money", "Lombank")
        {
            BannerImagePath = "stores\\lombank.png",
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 6,
            CloseTime = 20
        };
        LibertyCityLocations.Banks.Add(Lombank1);
        Bank Lombank2 = new Bank(new Vector3(3888.802f, -2447.503f, 19.57722f), 88.54723f, "Lombank", "Our time is your money", "Lombank")
        {
            BannerImagePath = "stores\\lombank.png",
            StateID = StaticStrings.AlderneyStateID,
            OpenTime = 6,
            CloseTime = 20
        };
        LibertyCityLocations.Banks.Add(Lombank2);
        Bank Lombank3 = new Bank(new Vector3(5157.604f, -3864.231f, 14.7641f), 92.47133f, "Lombank", "Our time is your money", "Lombank")
        {
            BannerImagePath = "stores\\lombank.png",
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 6,
            CloseTime = 20
        };
        LibertyCityLocations.Banks.Add(Lombank3);
        Bank Lombank4 = new Bank(new Vector3(4789.345f, -2058.16f, 14.75947f), 224.3296f, "Lombank", "Our time is your money", "Lombank")
        {
            BannerImagePath = "stores\\lombank.png",
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 6,
            CloseTime = 20
        };
        LibertyCityLocations.Banks.Add(Lombank4);
        Bank Fleeca1 = new Bank(new Vector3(5161.627f, -3808.056f, 16.23184f), 132.2257f, "Fleeca", "Everything, at a price", "Fleeca")
        {
            BannerImagePath = "stores\\fleeca.png",
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 6,
            CloseTime = 20
        };
        LibertyCityLocations.Banks.Add(Fleeca1);
        Bank Fleeca2 = new Bank(new Vector3(4938.306f, -1932.056f, 25.60661f), 93.53471f, "Fleeca", "Everything, at a price", "Fleeca")
        {
            BannerImagePath = "stores\\fleeca.png",
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 6,
            CloseTime = 20
        };
        LibertyCityLocations.Banks.Add(Fleeca2);
        Bank Fleeca3 = new Bank(new Vector3(5157.435f, -1840.307f, 20.42588f), 164.2225f, "Fleeca", "Everything, at a price", "Fleeca")
        {
            BannerImagePath = "stores\\fleeca.png",
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 6,
            CloseTime = 20
        };
        LibertyCityLocations.Banks.Add(Fleeca3);
        Bank Fleeca4 = new Bank(new Vector3(4780.868f, -2775.838f, 12.5917f), 184.7551f, "Fleeca", "Everything, at a price", "Fleeca")
        {
            BannerImagePath = "stores\\fleeca.png",
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 6,
            CloseTime = 20
        };
        LibertyCityLocations.Banks.Add(Fleeca4);
        Bank WhanQBank = new Bank(new Vector3(6459.41f, -2857.36f, 22.2841f), 177.8995f, "Whan-Q Bank", "Don't beat off, Come Inside!", "WHANQ")
        {
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 6,
            CloseTime = 20
        };
        LibertyCityLocations.Banks.Add(WhanQBank);

        Bank BOLbank1 = new Bank(new Vector3(5159.549f, -3724.783f, 15.40793f), 86.99619f, "Bank Of Liberty", "Bleeding you dry", "BOL")
        {
            BannerImagePath = "",
            InteriorID = 25858,
            StateID = StaticStrings.LibertyStateID,
            VendorLocations = new List<SpawnPlace>() {
                    new SpawnPlace(new Vector3(5180.64551f, -3723.559f, 14.4142027f), 124.0465f),
                    new SpawnPlace(new Vector3(5184.464f, -3722.74219f, 14.4142027f), 44.2513f),
                    new SpawnPlace(new Vector3(5181.42432f, -3718.69f, 14.4142027f), 31.6572f),
                    new SpawnPlace(new Vector3(5182.34131f, -3720.258f, 14.4142027f), 212.2828f),
                    new SpawnPlace(new Vector3(5182.22461f, -3725.69287f, 14.4142027f), 116.5878f),
                    new SpawnPlace(new Vector3(5177.877f, -3719.38257f, 14.4142027f), 125.155f),
                    new SpawnPlace(new Vector3(5179.19971f, -3721.38354f, 14.4142027f), 128.9464f),
                    new SpawnPlace(new Vector3(5174.94043f, -3715.1145f, 14.4142027f), 124.5325f),
                    new SpawnPlace(new Vector3(5176.401f, -3717.375f, 14.4142027f), 129.4322f),
                    new SpawnPlace(new Vector3(5179.348f, -3715.91357f, 14.4142027f), 223.7352f),
                    new SpawnPlace(new Vector3(5171.04932f, -3713.58081f, 14.457613f), 178.3889f),
                },
            ActivateCells = 3,
            ActivateDistance = 75f,
            RestrictedAreas = new RestrictedAreas()
            {
                RestrictedAreasList = new List<RestrictedArea>()
                    {
                        new RestrictedArea("BOL Front Room",new Vector2[]
                        {
                               new Vector2(5182.109f, -3732.1f),
                               new Vector2(5181.555f, -3712.389f),
                               new Vector2(5189.692f, -3712.29272f),
                               new Vector2(5188.98047f, -3733.34277f),
                        },
                        null,RestrictedAreaType.Bank) { IsCivilianReactableRestricted = true, },
                        new RestrictedArea("Bol Vault Area",new Vector2[]
                        {
                               new Vector2(5166.774f, -3733.95923f),
                               new Vector2(5166.73242f, -3726.76855f),
                               new Vector2(5171.307f, -3726.47583f),
                               new Vector2(5189.338f, -3728.8396f),
                               new Vector2(5189.336f, -3731.89575f),
                               new Vector2(5171.31543f, -3734.28223f),

                        },

                        null,RestrictedAreaType.Bank) { IsZRestricted = true, ZRestrictionMin = 82.5f - 2f,ZRestrictionMax = 82.5f + 2f, IsCivilianReactableRestricted = true, },
                    }
            },
            PossiblePedSpawns = new List<ConditionalLocation>() //new SecurityConditionalLocation(new Vector3(315.2496f,-284.7239f,54.14301f),70.262f,95f)
                {
                    new SecurityConditionalLocation(new Vector3(5182.184f, -3727.55933f, 14.4142027f), 179.0006f,95f) { AssociationID = "GRP6",RequiredPedGroup = "ArmedSecurity", MinHourSpawn = 6, MaxHourSpawn = 20, TaskRequirements = TaskRequirements.LocalScenario, },
                    new SecurityConditionalLocation(new Vector3(5165.533f, -3720.22168f, 15.4071226f),262.5291f,95f) { AssociationID = "GRP6",RequiredPedGroup = "ArmedSecurity", MinHourSpawn = 6, MaxHourSpawn = 20, TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>() { "WORLD_HUMAN_GUARD_STAND" }, },
                    new SecurityConditionalLocation(new Vector3(5188.75537f, -3723.827f, 15.4071226f),97.86108f,95f) { AssociationID = "GRP6",RequiredPedGroup = "ArmedSecurity", MinHourSpawn = 6, MaxHourSpawn = 20, TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>() { "WORLD_HUMAN_GUARD_STAND" }, },
                    new SecurityConditionalLocation(new Vector3(5176.65674f, -3731.84546f, 15.4071121f),356.3171f,95f) { AssociationID = "GRP6",RequiredPedGroup = "ArmedSecurity", LongGunAlwaysEquipped = true, ForceSidearm = true,ForceLongGun = true, MinHourSpawn = 6, MaxHourSpawn = 20, TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>() { "WORLD_HUMAN_GUARD_STAND" }, },
                    new SecurityConditionalLocation(new Vector3(5187.666f, -3728.77881f, 8.907082f), 180.259f,95f) { AssociationID = "GRP6",RequiredPedGroup = "ArmedSecurity", LongGunAlwaysEquipped = true, ForceSidearm = true,ForceLongGun = true, MinHourSpawn = 6, MaxHourSpawn = 20, TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>() { "WORLD_HUMAN_GUARD_STAND" }, },
                    new SecurityConditionalLocation(new Vector3(5187.8457f, -3731.83032f, 8.907082f), 0.09673272f,95f) { AssociationID = "GRP6",RequiredPedGroup = "ArmedSecurity", LongGunAlwaysEquipped = true, ForceSidearm = true,ForceLongGun = true, MinHourSpawn = 6, MaxHourSpawn = 20, TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>() { "WORLD_HUMAN_GUARD_STAND" }, },
                },
        };
        LibertyCityLocations.Banks.Add(BOLbank1);
    }
    private void DefaultConfig_BarberShops()
    {
        LibertyCityLocations.BarberShops.AddRange(new List<BarberShop>()
        {
            new BarberShop(new Vector3(3736.402f, -2185.702f, 23.28668f), 92.74634f, "Unisex Salon", "")
            {
                StateID = StaticStrings.AlderneyStateID,
                IsTemporarilyClosed = true,
                VendorPersonnelID = "HaircutPeds",
                TypeName = "Beauty Salon",
            },
            new BarberShop(new Vector3(3712.829f, -2285.225f, 23.03666f), 269.9279f, "Alderney Hair Co", "")
            {
                StateID = StaticStrings.AlderneyStateID,
                IsTemporarilyClosed = true,
                VendorPersonnelID = "HaircutPeds",
                TypeName = "Beauty Salon",
            },
            new BarberShop(new Vector3(3712.442f, -2297.101f, 23.03628f), 269.7096f, "Spanky Beauty Supply", "")
            {
                StateID = StaticStrings.AlderneyStateID,
                IsTemporarilyClosed = true,
                VendorPersonnelID = "HaircutPeds",
                TypeName = "Beauty Salon",
            },
            new BarberShop(new Vector3(4753.042f, -2884.834f, 11.04266f), 4.2963f, "The Barber Shop", "")
            {
                StateID = StaticStrings.LibertyStateID,
                IsTemporarilyClosed = true,
                VendorPersonnelID = "HaircutPeds",
                TypeName = "Beauty Salon",
            },
            new BarberShop(new Vector3(4996.204f, -1849.75f, 20.41311f), 357.2557f, "Father & Son Barber Shop", "")
            {
                StateID = StaticStrings.LibertyStateID,
                IsTemporarilyClosed = true,
                VendorPersonnelID = "HaircutPeds",
                TypeName = "Beauty Salon",
            },
            new BarberShop(new Vector3(6622.539f, -3076.319f, 27.1837f), 90.42361f, "Unisex Salon", "")
            {
                StateID = StaticStrings.LibertyStateID,
                IsTemporarilyClosed = true,
                VendorPersonnelID = "HaircutPeds",
                TypeName = "Beauty Salon",
            },
            new BarberShop(new Vector3(6482.893f, -3387.413f, 27.24434f), 178.4335f, "The Barber Shop", "")
            {
                StateID = StaticStrings.LibertyStateID,
                IsTemporarilyClosed = true,
                VendorPersonnelID = "HaircutPeds",
                TypeName = "Beauty Salon",
            },


            new BarberShop(new Vector3(6986.595f, -2846.433f, 26.85387f), 269.5889f, "Strictly Ink", "")
            {
                StateID = StaticStrings.LibertyStateID,
                IsTemporarilyClosed = true,
                VendorPersonnelID = "HaircutPeds",
                TypeName = "Beauty Salon",
            },
            new BarberShop(new Vector3(6986.103f, -2853.092f, 26.92659f), 269.7621f, "Harda's Nails", "")
            {
                StateID = StaticStrings.LibertyStateID,
                IsTemporarilyClosed = true,
                VendorPersonnelID = "HaircutPeds",
                TypeName = "Beauty Salon",
            },
            new BarberShop(new Vector3(6168.045f, -3439.792f, 24.10083f), 286.8f, "Wil's Beauty Supply", "")
            {
                StateID = StaticStrings.LibertyStateID,
                IsTemporarilyClosed = true,
                VendorPersonnelID = "HaircutPeds",
                TypeName = "Beauty Salon",
            },
            new BarberShop(new Vector3(6173.375f, -3509.156f, 22.02839f), 276.7236f, "Liberty City Beauty Salon", "")
            {
                StateID = StaticStrings.LibertyStateID,
                IsTemporarilyClosed = true,
                VendorPersonnelID = "HaircutPeds",
                TypeName = "Beauty Salon",
            },
            new BarberShop(new Vector3(6311.125f, -2602.284f, 38.86154f), 91.24779f, "Hair Express Unisex", "")
            {
                StateID = StaticStrings.LibertyStateID,
                IsTemporarilyClosed = true,
                VendorPersonnelID = "HaircutPeds",
                TypeName = "Beauty Salon",
            },
        });
    }
    private void DefaultConfig_Bars()
    {
        LibertyCityLocations.Bars.AddRange(new List<Bar>()
        {
            //Vendored Bars
            new Bar() {
                MenuID = "BarMenu",
                Name = "Comrades Bar",
                Description = "Party like it's 1924",
                EntrancePosition = new Vector3(6116.227f, -3746.394f, 15.48697f),
                EntranceHeading = 91.39f,
                OpenTime = 8,
                CloseTime = 24,
                InteriorID = 164866,
                IsWalkup = true,
                VendorPersonnelID = "BarPeds",
                ExtaVendorSpawnPercentage = 0f,
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(6122.559f, -3746.585f, 13.48475f), 176.0122f) },
                ActivateCells = 3,
                ActivateDistance = 75f,
                StateID = StaticStrings.LibertyStateID,
            },
            new Bar() {
                MenuID = "BarMenu",
                Name = "Lucky Winkles",
                Description = "A classic Purgatory dive bar",
                EntrancePosition = new Vector3(4747.006f, -2799.981f, 10.36142f),
                EntranceHeading = 39.96938f,
                OpenTime = 8,
                CloseTime = 24,
                InteriorID = 54018,
                IsWalkup = true,
                VendorPersonnelID = "BarPeds",
                ExtaVendorSpawnPercentage = 75f,
                VendorLocations = new List<SpawnPlace>()
                {
                new SpawnPlace(new Vector3(4754.746f, -2805.157f, 8.39501f), 0.8224639f),
                new SpawnPlace(new Vector3(4750.163f, -2805.139f, 8.39505f), 0.7455646f)
                },
                ActivateCells = 3,
                ActivateDistance = 75f,
                StateID = StaticStrings.LibertyStateID,
            },
            new Bar() {
                MenuID = "BarMenu",
                Name = "Steinway Beer Garden",
                Description = "Take shots and shoot darts",
                EntrancePosition = new Vector3(6338.716f, -2520.763f, 35.39922f),
                EntranceHeading = 224.33f,
                OpenTime = 8,
                CloseTime = 24,
                InteriorID = 80386,
                IsWalkup = true,
                VendorPersonnelID = "BarPeds",
                ExtaVendorSpawnPercentage = 75f,
                VendorLocations = new List<SpawnPlace>()
                {
                new SpawnPlace(new Vector3(6330.663f, -2515.596f, 33.51512f), 182.208f),
                new SpawnPlace(new Vector3(6335.267f, -2515.52f, 33.51512f), 183.8725f)
                },
                ActivateCells = 3,
                ActivateDistance = 75f,
                StateID = StaticStrings.LibertyStateID,
            },
            new Bar() {
                MenuID = "BarMenu",
                Name = "Homebrew Cafe",
                Description = "A piece of Jamaica in dukes",
                EntrancePosition = new Vector3(6663.519f, -3201.899f, 25.17551f),
                EntranceHeading = 270.0753f,
                OpenTime = 8,
                CloseTime = 24,
                InteriorID = 121346,
                IsWalkup = true,
                VendorPersonnelID = "BarPeds",
                ExtaVendorSpawnPercentage = 0f,
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(6654.771f, -3196.343f, 23.18558f), 272.2538f) },
                ActivateCells = 3,
                ActivateDistance = 75f,
                StateID = StaticStrings.LibertyStateID,
            },
            new Bar() {
                MenuID = "BarMenu",
                Name = "Perestroika Bar",
                Description = "Back when sawing women in half was cabaret gold!",
                BannerImagePath = "stores//perestroika.png",
                EntrancePosition = new Vector3(6145.686f, -3545.817f, 19.85674f),
                EntranceHeading = 181.0387f,
                OpenTime = 8,
                CloseTime = 2,
                InteriorID = 78338,
                IsWalkup = true,
                VendorPersonnelID = "BarPeds",
                ExtaVendorSpawnPercentage = 0f,
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(6136.197f, -3526.964f, 16.27434f), 268.7266f) },
                ActivateCells = 3,
                ActivateDistance = 75f,
                StateID = StaticStrings.LibertyStateID,
            },
            new Bar() {
                MenuID = "BarMenu",
                Name = "The Triangle Club",
                Description = "Beautiful women and expensive drinks",
                EntrancePosition = new Vector3(6385.851f, -1544.901f, 17.48216f),
                EntranceHeading = 311.6369f,
                OpenTime = 8,
                CloseTime = 2,
                InteriorID = 113666,
                IsWalkup = true,
                VendorPersonnelID = "BarPeds",
                ExtaVendorSpawnPercentage = 75f, 
                VendorLocations = new List<SpawnPlace>()
                {
                    new SpawnPlace(new Vector3(6376.991f, -1550.331f, 15.72202f), 127.1123f),
                    new SpawnPlace(new Vector3(6378.778f, -1553.282f, 15.72204f), 137.1951f)
                },
                ActivateCells = 3,
                ActivateDistance = 75f,
                StateID = StaticStrings.LibertyStateID,
            },
            new Bar() {
                MenuID = "BarMenu",
                Name = "Honkers",
                Description = "Beautiful women and expensive drinks",
                EntrancePosition = new Vector3(3607.6167f,-3229.81421f,10.0198069f),
                EntranceHeading = 88.81f,
                OpenTime = 8,
                CloseTime = 2,
                InteriorID = 67586,
                IsWalkup = true,
                VendorPersonnelID = "BarPeds",
                ExtaVendorSpawnPercentage = 75f,
                VendorLocations = new List<SpawnPlace>()
                {
                    new SpawnPlace(new Vector3(3612.80762f, -3243.654f, 8.0099955f), 47.77391f),
                    new SpawnPlace(new Vector3(3622.72681f, -3243.58887f, 8.0099955f), 357.8109f),
                    new SpawnPlace(new Vector3(3617.8418f, -3247.95068f, 8.0099955f), 181.4274f)
                },
                ActivateCells = 3,
                ActivateDistance = 75f,
                StateID = StaticStrings.AlderneyStateID,
            },
            new Bar() {
                MenuID = "BarMenu",
                Name = "Hercules",
                Description = "A slice of Olympus here on Earth",
                EntrancePosition = new Vector3(4747.581f, -2899.38672f, 11.7137461f),
                EntranceHeading = 86.27088f,
                OpenTime = 18,
                CloseTime = 2,
                InteriorID = 12034,
                IsWalkup = true,
                VendorPersonnelID = "BarPeds",
                ExtaVendorSpawnPercentage = 75f,
                VendorLocations = new List<SpawnPlace>()
                {
                    new SpawnPlace(new Vector3(4755.039f, -2901.779f, 9.7114573f), 217.491f),
                    new SpawnPlace(new Vector3(4752.865f, -2901.731f, 9.7014475f), 189.6392f)
                },
                ActivateCells = 3,
                ActivateDistance = 75f,
                StateID = StaticStrings.LibertyStateID,
            },
            new Bar() {
                MenuID = "BarMenu",
                Name = "Maisonette 9",
                Description = "A slice of Olympus here on Earth",
                EntrancePosition = new Vector3(4720.461f, -3102.987f, 9.859278f),
                EntranceHeading = 269.6452f,
                OpenTime = 18,
                CloseTime = 2,
                InteriorID = 126210,
                IsWalkup = true,
                VendorPersonnelID = "BarPeds",
                ExtaVendorSpawnPercentage = 75f,
                VendorLocations = new List<SpawnPlace>()
                {
                    new SpawnPlace(new Vector3(4705.25f, -3097.199f, 5.683848f), 183.9442f),
                    new SpawnPlace(new Vector3(4703.629f, -3097.244f, 5.68385f), 183.9442f)
                },
                ActivateCells = 3,
                ActivateDistance = 75f,
                StateID = StaticStrings.LibertyStateID,
            },
            new Bar(new Vector3(4789.221f, -2859.199f, 14.40985f), 183.1031f, "Bahama Mama's", "","BarMenu")
            {
                OpenTime = 18,
                CloseTime = 2,
                InteriorID = 130818,
                IsWalkup = true,
                VendorPersonnelID = "BarPeds",
                ExtaVendorSpawnPercentage = 75f,
                VendorLocations = new List<SpawnPlace>()
                {
                    new SpawnPlace(new Vector3(4793.736f, -2848.192f, 3.822031f), 179.5796f),
                    new SpawnPlace(new Vector3(4796.532f, -2843.472f, 3.669319f), 271.8682f),
                    new SpawnPlace(new Vector3(4799.743f, -2816.704f, 4.169299f), 182.1243f),
                    new SpawnPlace(new Vector3(4792.512f, -2816.932f, 4.169302f), 180.2005f)
                },
                ActivateCells = 3,
                ActivateDistance = 75f,
                StateID = StaticStrings.LibertyStateID
            },
            // NonVendor Bars
            new Bar() {
                MenuID = "BarMenu",
                Name = "Hallet's Sports Bar",
                Description = "Perfect for a drink after a funeral",
                EntrancePosition = new Vector3(5632.897f, -3218.074f, 8.859234f),
                EntranceHeading = 60.15f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Bar() {
                MenuID = "BarMenu",
                Name = "Jerkov's",
                Description = "Even the elite needs to get drunk",
                EntrancePosition = new Vector3(5268.037f, -2697.834f, 18.63017f),
                EntranceHeading = 178.83f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Bar(new Vector3(3679.422f, -2716.388f, 19.56574f), 267.1285f, "Leprechauns Winklepicker", "Take a bite of our fanny","FancyFishMenu") { StateID = StaticStrings.AlderneyStateID },
            new Bar(new Vector3(4654.713f, -1902.926f, 17.47365f), 180.2428f, "Linen Lounge", "","BarMenu") { StateID = StaticStrings.LibertyStateID },
            new Bar(new Vector3(5208.91f, -3692.587f, 13.56118f), 89.61293f, "Big Boss Nightclub", "","BarMenu") { StateID = StaticStrings.LibertyStateID },
            new Bar(new Vector3(6382.357f, -3444.526f, 26.42121f), 270.3983f, "O'Reilly's Bar", "","BarMenu") { StateID = StaticStrings.LibertyStateID },
            new Bar(new Vector3(6620.784f, -3236.558f, 26.07549f), 90.40106f, "The Bay Bar", "","BarMenu") { StateID = StaticStrings.LibertyStateID },
            new Bar(new Vector3(6311.128f, -2585.644f, 38.03498f), 90.91409f, "8Ball Bar", "","BarMenu") { StateID = StaticStrings.LibertyStateID },
            new Bar(new Vector3(5644.609f, -1735.917f, 14.8801f), 6.018074f, "Whiskers", "","BarMenu") { StateID = StaticStrings.LibertyStateID },
        }
        );
    }
    private void DefaultConfig_BlankLocations()
    {
        BlankLocationsData_LibertyPP blankLocationsData_Liberty = new BlankLocationsData_LibertyPP();
        blankLocationsData_Liberty.DefaultConfig();
        LibertyCityLocations.BlankLocations.AddRange(blankLocationsData_Liberty.BlankLocationPlaces);
    }
    private void DefaultConfig_BodyExports()
    {
        BodyExports = new List<BodyExport>()
        {
            new BodyExport(new Vector3(5248.9f, -3068.368f, 14.75021f), 90.79181f, "Easton Medical Center", "Always looking for fresh meat!") {StateID = StaticStrings.LibertyStateID, OpenTime = 20, CloseTime = 4 },
            new BodyExport(new Vector3(6184.368f, -1427.732f, 20.11838f), 174.3487f, "Bohan Medical Research", "Always looking for fresh meat!") {StateID = StaticStrings.LibertyStateID, OpenTime = 20, CloseTime = 4 },
            new BodyExport(new Vector3(3658.467f, -2884.291f, 21.63115f), 89.27634f, "North Tudor Medical Center", "Always looking for fresh meat!") {StateID = StaticStrings.AlderneyStateID, OpenTime = 20, CloseTime = 4 },
        };
        LibertyCityLocations.BodyExports.AddRange(BodyExports);
    }
    private void DefaultConfig_Businesses()
    {
        LibertyCityLocations.Businesses.AddRange(new List<Business>()
        {
        new Business(new Vector3(3867.972f, -2985.559f, 11.27661f), 223.659f, "Axels Chop Shop", "")
        {
            OpenTime = 0,
            CloseTime = 24,
            PurchasePrice = 386000,
            SalesPrice = 150000,
            PayoutMin = 1000,
            PayoutMax = 8000,
            PayoutFrequency = 5,
            FullName = "Axels Port Tudor, Chop Shop",
            StateID = StaticStrings.LibertyStateID,
        },
        new Business(new Vector3(6262.109f, -3546.1f, 21.35021f), 359.8495f, "Native Chop Shop", "")
        {
            OpenTime = 0,
            CloseTime = 24,
            PurchasePrice = 260000,
            SalesPrice = 120000,
            PayoutMin = 1000,
            PayoutMax = 5000,
            PayoutFrequency = 5,
            FullName = "Native Engine, Chop Shop",
            StateID = StaticStrings.LibertyStateID,
        },
        new Business(new Vector3(6271.508f, -2621.261f, 38.64142f), 180.5914f, "East Island Laundromat", "")
        {
            IgnoreEntranceInteract = true,
            IsWalkup = true,
            InteriorID = 37634,
            OpenTime = 0,
            CloseTime = 24,
            PurchasePrice = 180000,
            SalesPrice = 75000,
            PayoutMin = 500,
            PayoutMax = 3000,
            PayoutFrequency = 5,
            FullName = "East Island Laundromat, Money Laundering",
            StateID = StaticStrings.LibertyStateID,
        },// Laundromat interior
        new Business(new Vector3(4146.933f, -2131.434f, 13.60034f), 181.5922f, "Alderney City Laundromat", "")
        {
            IgnoreEntranceInteract = true,
            IsWalkup = true,
            InteriorID = 150530,
            OpenTime = 0,
            CloseTime = 24,
            PurchasePrice = 180000,
            SalesPrice = 75000,
            PayoutMin = 500,
            PayoutMax = 3000,
            PayoutFrequency = 5,
            FullName = "Alderney City Laundromat, Money Laundering",
            StateID = StaticStrings.LibertyStateID,
        },// Laundromat interior
        new Business(new Vector3(6201.06f, -3581.436f, 20.24536f), 96.40791f, "Hove Beach Laundromat", "")
        {
            IgnoreEntranceInteract = true,
            IsWalkup = true,
            InteriorID = 158466,
            OpenTime = 0,
            CloseTime = 24,
            PurchasePrice = 180000,
            SalesPrice = 75000,
            PayoutMin = 500,
            PayoutMax = 3000,
            PayoutFrequency = 5,
            FullName = "Hove Beach Laundromat, Money Laundering",
            StateID = StaticStrings.LibertyStateID,
        },// Laundromat interior
        new Business(new Vector3(5438.008f, -3514.112f, 5.550913f), 267.6571f,"Document Forgery", "")
        {
            OpenTime = 0,
            CloseTime = 24,
            PurchasePrice = 196500,
            SalesPrice = 73000,
            FullName = "Fishmarket North, Document Forgery",
            PossibleModItemPayouts = new List<string>() { "Drivers License" },
            ModItemPayoutAmount = 10,
            StateID = StaticStrings.LibertyStateID,
        },
        new Business(new Vector3(3916.384f, -2418.1f, 20.56711f), 0.6744787f,"Document Forgery", "")
        {
            OpenTime = 0,
            CloseTime = 24,
            PurchasePrice = 196500,
            SalesPrice = 73000,
            FullName = "Alderney City, Document Forgery",
            PossibleModItemPayouts = new List<string>() { "Drivers License" },
            ModItemPayoutAmount = 10,
            CameraPosition = new Vector3(3915.175f, -2403.54f, 25.02996f),
            CameraDirection = new Vector3(0.1375061f, -0.9379529f, -0.318334f),
            CameraRotation = new Rotator(-18.5622f, 6.754691E-07f, -171.6597f),
            StateID = StaticStrings.LibertyStateID,
        },
        new Business(new Vector3(4205.692f, -1388.405f, 22.96781f), 255.2065f,"Trap House", "Selling misery since the 80's")
        {
            OpenTime = 0,
            CloseTime = 24,
            PurchasePrice = 1100000,
            SalesPrice = 465000,
            FullName = "Westdyke, Trap House",
            PossibleModItemPayouts = new List<string>() { "Marijuana", "Cocaine", "Crack", "Methamphetamine", "Heroin", "SPANK" },
            ModItemPayoutAmount = 150,
            CraftingFlag = "DrugLab",
            StateID = StaticStrings.LibertyStateID,
        },
        new Business(new Vector3(3328.745f, -3043.681f, 16.95441f), 196.3285f,"Trap House", "Selling misery since the 80's")
        {
            OpenTime = 0,
            CloseTime = 24,
            PurchasePrice = 535000,
            SalesPrice = 250000,
            FullName = "Acter, Trap House",
            PossibleModItemPayouts = new List<string>() { "Marijuana", "Cocaine", "Crack", "Methamphetamine", "Heroin", "SPANK" },
            ModItemPayoutAmount = 100,
            CraftingFlag = "DrugLab",
            StateID = StaticStrings.LibertyStateID,
        },
        new Business(new Vector3(6541.515f, -3437.749f, 22.15429f), 86.45231f,"Trap House", "Selling misery since the 80's")
        {
            OpenTime = 0,
            CloseTime = 24,
            PurchasePrice = 1500000,
            SalesPrice = 650000,
            FullName = "South Slopes, Trap House",
            PossibleModItemPayouts = new List<string>() { "Marijuana", "Cocaine", "Crack", "Methamphetamine", "Heroin", "SPANK" },
            ModItemPayoutAmount = 200,
            CraftingFlag = "DrugLab",
            StateID = StaticStrings.LibertyStateID,
        },
        new Business(new Vector3(6405.359f, -1609.939f, 18.1663f), 313.2379f,"Trap House", "Selling misery since the 80's")
        {
            OpenTime = 0,
            CloseTime = 24,
            PurchasePrice = 240000,
            SalesPrice = 80000,
            FullName = "Industrial, Trap House",
            PossibleModItemPayouts = new List<string>() { "Marijuana", "Cocaine", "Crack", "Methamphetamine", "Heroin", "SPANK" },
            ModItemPayoutAmount = 70,
            CraftingFlag = "DrugLab",
            StateID = StaticStrings.LibertyStateID,
        },
        new Business(new Vector3(6588.573f, -3261.12f, 25.77531f), 272.6096f,"Meth Factory", "")
        {
            OpenTime = 0,
            CloseTime = 24,
            PurchasePrice = 1200000,
            SalesPrice = 550000,
            FullName = "Schottler, Meth Factory",
            PossibleModItemPayouts = new List<string>() { "Methamphetamine" },
            ModItemPayoutAmount = 200,
            StateID = StaticStrings.LibertyStateID,
        },
        new Business(new Vector3(3387.673f, -3523.928f, 2.715489f), 270.0141f,"Meth Lab", "")
        {
            OpenTime = 0,
            CloseTime = 24,
            PurchasePrice = 730000,
            SalesPrice = 320000,
            FullName = "Actor Industrial, Meth Lab",
            PossibleModItemPayouts = new List<string>() { "Methamphetamine" },
            ModItemPayoutAmount = 200,
            StateID = StaticStrings.LibertyStateID,
        },
        new Business(new Vector3(6075.692f, -3512.467f, 18.92244f), 358.1287f,"Marijuana Grow House", "")
            {
                OpenTime = 0,
                CloseTime = 24,
                PurchasePrice = 150000,
                SalesPrice = 85000,
                FullName = "Hove Beach, Marijuana Grow House",
                PossibleModItemPayouts = new List<string>() { "Marijuana" },
                ModItemPayoutAmount = 100,
                StateID = StaticStrings.LibertyStateID,
            },
        new Business(new Vector3(3613.078f, -3053.406f, 13.06341f), 91.73339f,"Weed Farm", "")
            {
                OpenTime = 0,
                CloseTime = 24,
                PurchasePrice = 350000,
                SalesPrice = 120000,
                FullName = "Tudor, Marijuana Lockup",
                PossibleModItemPayouts = new List<string>() { "Marijuana" },
                ModItemPayoutAmount = 200,
                StateID = StaticStrings.LibertyStateID,
            },
        new Business(new Vector3(4707.243f, -3375.117f, 8.982045f), 88.7366f,"Cocaine Factory", "")
            {
                OpenTime = 0,
                CloseTime = 24,
                PurchasePrice = 1800000,
                SalesPrice = 780000,
                FullName = "The Meat Quarter, Cocaine Factory",
                PossibleModItemPayouts = new List<string>() { "Cocaine" },
                ModItemPayoutAmount = 200,
                CameraPosition = new Vector3(4693.954f, -3367.281f, 12.07276f),
                CameraDirection = new Vector3(0.738019f, -0.6181095f, -0.2706819f),
                CameraRotation = new Rotator(-15.70485f, -2.039829E-05f, -129.947f),
                StateID = StaticStrings.LibertyStateID,
            },
        new Business(new Vector3(6026.611f, -2945.404f, 5.678048f), 90.10968f,"Cocaine Lockup", "")
            {
                OpenTime = 0,
                CloseTime = 24,
                PurchasePrice = 1200000,
                SalesPrice = 420000,
                FullName = "Rotterdam, Cocaine Lockup",
                PossibleModItemPayouts = new List<string>() { "Cocaine" },
                ModItemPayoutAmount = 200,
                StateID = StaticStrings.LibertyStateID,
            },
        });
    }
    private void DefaultConfig_CarCrushers()
    {
        CarCrushers = new List<CarCrusher>()
        {
        new CarCrusher(new Vector3(5736.397f, -3049.674f, 8.728484f), 90.31999f, "Colony Crusher", "Dead skunk in the trunk?") { StateID = StaticStrings.LibertyStateID, OpenTime = 20, CloseTime = 4 },
        };
        LibertyCityLocations.CarCrushers.AddRange(CarCrushers);
    }
    private void DefaultConfig_CityHalls()
    {
        List<CityHall> CityHalls = new List<CityHall>()
        {
            new CityHall(new Vector3(3978.112f, -2264.709f, 20.59385f), 179.2278f,"Alderney City Hall","") { OpenTime = 9,CloseTime = 18, StateID = StaticStrings.AlderneyStateID },
            new CityHall(new Vector3(5008.967f, -3601.584f, 17.84076f), 175.24f,"Civic Citadel","City hall") { OpenTime = 9,CloseTime = 18, StateID = StaticStrings.LibertyStateID },
        };
        LibertyCityLocations.CityHalls.AddRange(CityHalls);
    }
    private void DefaultConfig_ClothingShops()
    {
        LibertyCityLocations.ClothingShops.AddRange(new List<ClothingShop>()
        {
            new ClothingShop(new Vector3(6081.239f, -3699.316f, 15.86007f), 274.2502f, "Russian Store", "","") { StateID = StaticStrings.LibertyStateID,InteriorID = 120834,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(6077.284f, -3703.221f, 15.8473f), 20.45024f) },MinPriceRefreshHours= 0 ,MaxPriceRefreshHours = 0,MinRestockHours = 0 ,MaxRestockHours = 0,ActivateCells = 3, ActivateDistance = 75 },
            new ClothingShop(new Vector3(6116.508f, -3516.648f, 19.33661f), 89.94569f, "Jewelry Store", "","") { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            new ClothingShop(new Vector3(5198.929f, -3914.975f, 14.7644f), 4.162239f, "Perseus", "","") { IsTemporarilyClosed = true ,StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(5200.377f, -3922.09f, 14.86155f), 48.44085f) },ActivateCells = 3, ActivateDistance = 75 },
            new ClothingShop(new Vector3(5210.399f, -2446.067f, 14.71319f), 1.235396f, "Perseus", "","") { IsTemporarilyClosed = true , StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(5212.094f, -2453.245f, 14.76183f), 24.39743f) },ActivateCells = 3, ActivateDistance = 75 },
            new ClothingShop(new Vector3(4907.484f, -1901.719f, 25.63186f), 177.0755f, "Modo", "","") { IsTemporarilyClosed = true , StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4907.553f, -1891.094f, 25.63248f), 177.7734f) },ActivateCells = 3, ActivateDistance = 75 },
            new ClothingShop(new Vector3(6544.969f, -3128.492f, 31.28652f), 240.849f, "Street Clothes", "","") { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            new ClothingShop(new Vector3(6296.3f, -3810.494f, 13.52442f), 183.2335f, "Shirt World", "","") { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            new ClothingShop(new Vector3(6442.854f, -3332.284f, 28.2578f), 90.98743f, "Ladies Fashion", "Clothes for Ladies","") { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            new ClothingShop(new Vector3(6360.163f, -3106.688f, 32.54764f), 0.2708052f, "Derriere", "","") { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },

            new ClothingShop(new Vector3(6499.333f, -2808.028f, 24.79664f), 85.29411f, "Jim's Jewels of Nondon", "","") { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            new ClothingShop(new Vector3(6499.358f, -2839.03f, 22.64841f), 88.57362f, "Anna Jewelers", "","") { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            new ClothingShop(new Vector3(6500.941f, -2857.299f, 22.44048f), 180.5751f, "Mama Jewelers", "","") { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            new ClothingShop(new Vector3(6539.934f, -2880.58f, 22.785f), 359.0088f, "Silk-N-Gold Jewelers", "","") { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            new ClothingShop(new Vector3(6393.762f, -2505.643f, 36.11805f), 5.438942f, "Sexy Jewelry", "","") { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            new ClothingShop(new Vector3(6932.182f, -2826.354f, 27.32128f), 91.85302f, "Clothes Corner", "","") { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            new ClothingShop(new Vector3(6240.107f, -3094.819f, 31.87289f), 63.1114f, "Sneaky Footwear", "","") { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            new ClothingShop(new Vector3(6307.688f, -3201.353f, 34.20401f), 93.61601f, "Girly Girl Ladies Fashion & Shoes", "","") { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            new ClothingShop(new Vector3(6250.81f, -3130.49f, 33.59566f), 181.0713f, "Aeris", "","") { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            new ClothingShop(new Vector3(6227.926f, -3116.326f, 32.12412f), 53.89827f, "Curbcrawler Skateboards", "","") { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            new ClothingShop(new Vector3(6266.341f, -3104.912f, 32.63473f), 266.2737f, "Crevis", "","") { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },


            new ClothingShop(new Vector3(5637.803f, -1566.641f, 16.20274f), 252.747f, "Discount Fashion", "","") { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            new ClothingShop(new Vector3(5661.025f, -1541.183f, 16.2991f), 139.2149f, "Ranch", "","") { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            new ClothingShop(new Vector3(5918.007f, -1691.954f, 22.26619f), 269.7938f, "El Pedro Jewelry", "","") { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
        });
    }
    private void DefaultConfig_ConvenienceStores()
    {
        LibertyCityLocations.ConvenienceStores.AddRange(new List<ConvenienceStore>()
        {
            new ConvenienceStore() {
                MenuID = "TwentyFourSevenMenu",
                Name = "24/7",
                Description = "Go all night",
                BannerImagePath = "stores\\247.png",
                EntrancePosition = new Vector3(4843.389f, -3013.419f, 15.146f),
                EntranceHeading = 46.57f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new ConvenienceStore() {
                MenuID = "TwentyFourSevenMenu",
                Name = "24/7",
                Description = "Go all night",
                BannerImagePath = "stores\\247.png",
                EntrancePosition = new Vector3(6193.696f, -3670.569f, 16.2596f),
                EntranceHeading = 38.36f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new ConvenienceStore() {
                MenuID = "TwentyFourSevenMenu",
                Name = "24/7",
                Description = "Go all night",
                BannerImagePath = "stores\\247.png",
                EntrancePosition = new Vector3(5400.169f, -3761.981f, 9.428221f),
                EntranceHeading = 310.69f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new ConvenienceStore() {
                MenuID = "TwentyFourSevenMenu",
                Name = "24/7",
                Description = "Go all night",
                BannerImagePath = "stores\\247.png",
                EntrancePosition = new Vector3(6191.569f, -1402.755f, 20.11112f),
                EntranceHeading = 269.46f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new ConvenienceStore() {
                MenuID = "TwentyFourSevenMenu",
                Name = "24/7",
                Description = "Go all night",
                BannerImagePath = "stores\\247.png",
                EntrancePosition = new Vector3(5232.757f, -3478.504f, 14.76142f),
                EntranceHeading = 177.77f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new ConvenienceStore() {
                MenuID = "TwentyFourSevenMenu",
                Name = "24/7",
                Description = "Go all night",
                BannerImagePath = "stores\\247.png",
                EntrancePosition = new Vector3(6263.472f, -3207.364f, 34.07859f),
                EntranceHeading = 267.1f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new ConvenienceStore() {
                MenuID = "TwentyFourSevenMenu",
                Name = "24/7",
                Description = "Go all night",
                BannerImagePath = "stores\\247.png",
                EntrancePosition = new Vector3(4850.907f, -1823.924f, 12.70474f),
                EntranceHeading = 133.27f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new ConvenienceStore() {
                MenuID = "GasStationMenu",
                Name = "Globe Oil",
                Description = "The pumps might not work, but we'll still sell you a drink",
                EntrancePosition = new Vector3(4709.887f, -3450.844f, 7.857933f),
                EntranceHeading = 179.28f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new ConvenienceStore() {
                MenuID = "ConvenienceStoreMenu",
                Name = "Checkout!",
                Description = "Shopping for busy people",
                EntrancePosition = new Vector3(3708.647f, -1862.764f, 13.52485f),
                EntranceHeading = 271.59f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.AlderneyStateID,
            },
            new ConvenienceStore() {
                MenuID = "DeliGroceryMenu",
                Name = "Deli",
                Description = "Fresh food and fresh groceries",
                EntrancePosition = new Vector3(3540.817f, -2981.644f, 19.34992f),
                EntranceHeading = 174.93f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.AlderneyStateID,
            },
            new ConvenienceStore() {
                MenuID = "DeliGroceryMenu",
                Name = "Deli",
                Description = "Fresh food and fresh groceries",
                EntrancePosition = new Vector3(5917.866f, -1677.164f, 22.25874f),
                EntranceHeading = 270.87f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new ConvenienceStore() {
                MenuID = "DeliGroceryMenu",
                Name = "Deli",
                Description = "Fresh food and fresh groceries",
                EntrancePosition = new Vector3(6595.947f, -3011.305f, 26.22284f),
                EntranceHeading = 358.25f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new ConvenienceStore() {
                MenuID = "DeliGroceryMenu",
                Name = "Deli",
                Description = "Fresh food and fresh groceries",
                EntrancePosition = new Vector3(3591.448f, -2494.784f, 25.44854f),
                EntranceHeading = 267.96f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.AlderneyStateID,
            },
            new ConvenienceStore() {
                MenuID = "DeliGroceryMenu",
                Name = "Superb Deli",
                Description = "Franchising your beloved corner stores",
                EntrancePosition = new Vector3(4747.817f, -2725.256f, 9.939112f),
                EntranceHeading = 358.9f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },

            new ConvenienceStore() {
                MenuID = "DeliGroceryMenu",
                Name = "Deli",
                Description = "Fresh food and fresh groceries",
                EntrancePosition = new Vector3(6126.237f, -3214.334f, 27.3598f),
                EntranceHeading = 153.44f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new ConvenienceStore() {
                MenuID = "DeliGroceryMenu",
                Name = "Deli",
                Description = "Fresh food and fresh groceries",
                EntrancePosition = new Vector3(6382.917f, -2595.674f, 38.69456f),
                EntranceHeading = 84.36f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new ConvenienceStore() {
                MenuID = "DeliGroceryMenu",
                Name = "DC's Mini Market",
                Description = "Food, groceries, anything you need",
                EntrancePosition = new Vector3(4159.087f, -1904.704f, 25.86139f),
                EntranceHeading = 87.18f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.AlderneyStateID,
            },
            new ConvenienceStore() {
                MenuID = "DeliGroceryMenu",
                Name = "Happy Grocery",
                Description = "Food, groceries, anything you need",
                EntrancePosition = new Vector3(4048.67f, -2173.663f, 13.7752f),
                EntranceHeading = 90.78683f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.AlderneyStateID,
            },
            new ConvenienceStore(new Vector3(3675.666f, -1853.703f, 13.38639f), 357.4145f, "Happy Grocery", "","ConvenienceStoreMenu") { StateID = StaticStrings.AlderneyStateID },
            new ConvenienceStore(new Vector3(3525.999f, -2931.018f, 25.60732f), 14.01424f, "Los Delicatessen", "","ConvenienceStoreMenu") { StateID = StaticStrings.AlderneyStateID },
            new ConvenienceStore(new Vector3(3540.855f, -2981.797f, 19.35002f), 178.2625f, "Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.AlderneyStateID },
            new ConvenienceStore(new Vector3(3711.387f, -2273.386f, 23.1919f), 269.3182f, "Happy Grocery", "","ConvenienceStoreMenu") { StateID = StaticStrings.AlderneyStateID },
            new ConvenienceStore(new Vector3(3680.008f, -2549.904f, 25.4408f), 60.25175f, "Cheepo's", "","ConvenienceStoreMenu") { StateID = StaticStrings.AlderneyStateID },
            new ConvenienceStore(new Vector3(4009.066f, -1827.736f, 20.93378f), 90.02924f,  "Ruhni Poohs Indian Grocery", "","ConvenienceStoreMenu") { StateID = StaticStrings.AlderneyStateID },
            new ConvenienceStore(new Vector3(4065.872f, -1997.249f, 24.55272f), 138.7513f,  "Pedro's Mini Market", "","ConvenienceStoreMenu") { StateID = StaticStrings.AlderneyStateID },
            new ConvenienceStore(new Vector3(6084.508f, -3539.242f, 18.70319f), 278.4318f, "Table Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(6009.93f, -3591.366f, 16.02432f), 270.3828f, "Deli Sandwiches", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(6037.938f, -3760.758f, 12.43063f), 88.80841f, "Timazone's Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(6037.9f, -3741.779f, 12.43046f), 93.63044f, "Hero Shop", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(4938.378f, -1977.58f, 24.72915f), 89.82444f, "Corner Store", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },

            new ConvenienceStore(new Vector3(6167.707f, -3838.468f, 14.50082f), 3.454318f, "Island Grocery", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(6084.088f, -3571.116f, 18.18771f), 272.1106f, "Russian Grocery", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(6033.721f, -3794.609f, 13.95529f), 92.70599f, "Russian Grocery", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(4938.325f, -3502.804f, 14.55667f), 357.287f, "Superb Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(5049.246f, -1901.252f, 20.41524f), 90.58846f, "Superb Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(4744.966f, -3134.265f, 9.859381f), 91.26402f, "Superb Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(4746.72f, -2858.12f, 9.843159f), 93.12379f, "Superb Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(4722.144f, -1850.631f, 15.46146f), 264.9406f, "Prime Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(4848.276f, -1930.493f, 16.39029f), 94.80539f, "Moonshine Deli & Grocery","","ConvenienceStoreMenu") { OpenTime = 0, CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(4772.141f, -2775.985f, 12.38256f), 186.3018f, "Moonshine Deli & Grocery","","ConvenienceStoreMenu") { OpenTime = 0, CloseTime = 24, StateID = StaticStrings.LibertyStateID },


            new ConvenienceStore(new Vector3(6598.563f, -3206.268f, 25.90561f), 270.9548f, "Corner Store", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(6664.292f, -3055.057f, 23.13057f), 270.6736f, "Deli Grocery", "","ConvenienceStoreMenu") { OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(6560.913f, -3090.068f, 27.03068f), 315.8384f, "99 Cents Store", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(6266.401f, -3724.803f, 13.79101f), 355.1226f, "Lopez Food Co", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(6342.343f, -3810.305f, 13.93049f), 181.8152f, "Table Deli & Superior Market", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(6328.346f, -3811.438f, 13.77855f), 183.8857f, "Russian Grocery", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(6481.042f, -3388.262f, 27.38347f), 180.3469f, "1-Stop Deli Grocery", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(6409.205f, -3455.482f, 25.71779f), 91.63822f, "Island Grocery", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(6453.204f, -3412.29f, 27.56129f), 320.7452f, "Mardoll's Market", "","ConvenienceStoreMenu") { OpenTime = 0, CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(6448.409f, -3325.12f, 28.10044f), 2.363338f, "Deli-A-Go-Go", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(6598.296f, -3180.83f, 25.90015f), 273.9716f, "E-ZEE's Authentic Jamaican Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(6598.301f, -3061.286f, 26.40359f), 269.6626f, "Deli Grocery", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(6624.077f, -3089.122f, 26.96709f), 137.2647f, "Grocery Store", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(6570.984f, -3134.741f, 31.26461f), 121.3596f, "American and West Indian Grocery", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(6440.026f, -3107.9f, 32.79728f), 1.841843f, "Imperial Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(6395.542f, -3112.656f, 32.78489f), 70.67506f, "Roadway Deli & Grocery Coop", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },

            new ConvenienceStore(new Vector3(6171.993f, -3490.429f, 23.15311f), 275.4216f,  "Deli &Grocery", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(6442.863f, -3225.274f, 36.39338f), 274.2772f,  "Grocery Corp", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(6366.313f, -3226.065f, 35.17668f), 274.3954f,  "2Tap Food Mart", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(6303.443f, -3083.306f, 32.62494f), 134.5374f,  "Lopez Food Center", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(6329.423f, -2832.425f, 29.7883f), 73.95113f,  "Deli Grocery", "","ConvenienceStoreMenu") { OpenTime = 0, CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(6403.417f, -2856.488f, 22.5453f), 226.9586f,  "Superb Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(6548.112f, -2839.098f, 23.49522f), 268.584f,  "Candy & Grocery", "","ConvenienceStoreMenu") { OpenTime = 0, CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(6381.372f, -2604.003f, 38.77254f), 89.20015f,  "Italian Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(6382.93f, -2595.615f, 38.68592f), 95.04757f,  "Deli & Grocery", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(6986.601f, -2838.426f, 26.96443f), 271.3959f,  "S & D's Grocery & Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(7009.211f, -2799.716f, 27.93221f), 90.33557f,  "Willis Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(7009.403f, -2749.227f, 28.4416f), 92.979f,  "49 Cent Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(6944.12f, -2718.902f, 29.1284f), 52.94498f,  "Deli & Grocery", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },



            new ConvenienceStore(new Vector3(6233.389f, -2715.969f, 24.12768f), 3.654163f, "Deli & Grocery", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(6311.005f, -2574.59f, 37.31149f), 88.1508f, "Ellikon Pantopoleon Deli & Grocery", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(6311.054f, -2608.977f, 39.04198f), 88.21486f, "Alpha Milk", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(5577.339f, -1567.321f, 16.12238f), 184.3269f, "Candy Store", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(5606.698f, -1567.945f, 16.17415f), 182.4812f, "Mini Market", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(5657.558f, -1496.35f, 27.15479f), 357.5372f, "Deli & Grocery", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(5662.741f, -1736.632f, 13.4754f), 359.235f, "Sunshine Market","","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(5694.917f, -1751.333f, 12.39546f), 89.43409f, "Variety Shop", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(5695.899f, -1711.729f, 13.82744f), 136.6813f, "Deli Grocery", "","ConvenienceStoreMenu") { OpenTime = 0, CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(5724.248f, -1498.571f, 33.39884f), 297.3789f, "Deli.Grocery","","ConvenienceStoreMenu") { OpenTime = 0, CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(5798.999f, -1519.578f, 31.89469f), 183.5373f, "Convenience Store", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(5917.881f, -1701.879f, 22.16112f), 274.2349f, "Iron Belly Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(5925.136f, -1514.73f, 36.55355f), 198.3448f, "Deli Grocery","","ConvenienceStoreMenu") { OpenTime = 0, CloseTime = 24, StateID = StaticStrings.LibertyStateID },
        }
        );
    }
    private void DefaultConfig_DeadDrops()
    {
        List<DeadDrop> DeadDropsLC = new List<DeadDrop>()
        {
            new DeadDrop(new Vector3(5247.232f, -3453.85f, 14.72835f), 267.1549f,"Dead Drop", "the dumpster behind Fudz Diner")
            {
                OpenTime = 0,
                CloseTime = 24,
                IsEnabled = false,
                CanInteractWhenWanted = true,
                StateID = StaticStrings.LibertyStateID,
            },
            new DeadDrop() {
                Name = "Dead Drop",
                Description = "the dumpster behind the Pay'n'Spray in Outlook",
                EntrancePosition = new Vector3(6284.959f, -3566.579f, 21.96635f),
                EntranceHeading = 98.63f,
                OpenTime = 0,
                CloseTime = 24,
                IsEnabled = false,
                CanInteractWhenWanted = true,
                StateID = StaticStrings.LibertyStateID,
            },
            new DeadDrop() {
                Name = "Dead Drop",
                Description = "in between two dumpsters in the alley behind the Modo, in North Holland",
                EntrancePosition = new Vector3(4891.674f, -1874.921f, 20.23218f),
                EntranceHeading = 247.7246f,
                OpenTime = 0,
                CloseTime = 24,
                IsEnabled = false,
                CanInteractWhenWanted = true,
                StateID = StaticStrings.LibertyStateID,
            },
            new DeadDrop() {
                Name = "Dead Drop",
                Description = "under one of the big trees in the Colony Island graveyard parking lot",
                EntrancePosition = new Vector3(5673.196f, -2968.034f, 8.560134f),
                EntranceHeading = 307.34f,
                OpenTime = 0,
                CloseTime = 24,
                IsEnabled = false,
                CanInteractWhenWanted = true,
                StateID = StaticStrings.LibertyStateID,
            },
            new DeadDrop() {
                Name = "Dead Drop",
                Description = "trash can near the globe in Meadows Park",
                EntrancePosition = new Vector3(6648.057f, -2726.644f, 29.3575f),
                EntranceHeading = 324.12f,
                OpenTime = 0,
                CloseTime = 24,
                IsEnabled = false,
                CanInteractWhenWanted = true,
                StateID = StaticStrings.LibertyStateID,
            },
            new DeadDrop() {
                Name = "Dead Drop",
                Description = "the dumpster behind Burger Shot in Industrial Bohan",
                EntrancePosition = new Vector3(6318.646f, -1669.544f, 16.70793f),
                EntranceHeading = 133.86f,
                OpenTime = 0,
                CloseTime = 24,
                IsEnabled = false,
                CanInteractWhenWanted = true,
                StateID = StaticStrings.LibertyStateID,
            },
            new DeadDrop() {
                Name = "Dead Drop",
                Description = "under one of the seats in Feldspar Subway Station",
                EntrancePosition = new Vector3(4863.797f, -3632.884f, -7.989231f),
                EntranceHeading = 214.98f,
                OpenTime = 0,
                CloseTime = 24,
                IsEnabled = false,
                CanInteractWhenWanted = true,
                StateID = StaticStrings.LibertyStateID,
            },
            new DeadDrop() {
                Name = "Dead Drop",
                Description = "under the tires in Westdyke's abandoned mansion",
                EntrancePosition = new Vector3(4223.267f, -1355.674f, 22.50094f),
                EntranceHeading = 161.33f,
                OpenTime = 0,
                CloseTime = 24,
                IsEnabled = false,
                CanInteractWhenWanted = true,
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.DeadDrops.AddRange(DeadDropsLC);
    }
    private void DefaultConfig_Dealerships()
    {
        Dealership PaulieBudgetcar = new Dealership(new Vector3(3534.966f, -3324.852f, 6.786107f), 187.5414f, "Big Paulie Budget Cars", "Something affordable for everyone", "IVBudgetCarsMenu")
        {
            LicensePlatePreviewText = "PAULIE",
            StateID = StaticStrings.AlderneyStateID,
            CameraPosition = new Vector3(3504.261f, -3335.099f, 13.5847f),
            CameraDirection = new Vector3(-0.4841194f, 0.8641266f, -0.1375269f),
            CameraRotation = new Rotator(-7.904766f, -1.896321E-05f, 29.25935f),
            VehiclePreviewCameraPosition = new Vector3(3518.775f, -3320.294f, 7.912162f),
            VehiclePreviewCameraDirection = new Vector3(0.6039281f, 0.7640558f, -0.2269132f),
            VehiclePreviewCameraRotation = new Rotator(-13.1154f, 1.183465E-05f, -38.32369f),
            VehiclePreviewLocation = new SpawnPlace(new Vector3(3521.991f, -3314.987f, 6.556807f), 179.5185f),
            VehicleDeliveryLocations = new List<SpawnPlace>()
            {
                    new SpawnPlace(new Vector3(3497.139f, -3318.313f, 6.990633f), 268.3723f),
                    new SpawnPlace(new Vector3(3497.339f, -3312.033f, 7.465704f), 270.5613f),
                    new SpawnPlace(new Vector3(3497.117f, -3305.452f, 7.841705f), 270.3168f),
                    new SpawnPlace(new Vector3(3496.885f, -3298.975f, 8.191967f), 271.9184f),
                    new SpawnPlace(new Vector3(3497.333f, -3292.742f, 8.53085f), 271.5356f),
            },
        };
        LibertyCityLocations.CarDealerships.Add(PaulieBudgetcar);
        Dealership AutoEroticar = new Dealership(new Vector3(3700.829f, -2137.202f, 23.05553f), 271.584f, "Auto Eroticar", "Prestigious Automobiles", "IVEroticarMenu")
        {
            CameraPosition = new Vector3(3717.649f, -2145.816f, 30.69428f),
            CameraDirection = new Vector3(-0.8539895f, 0.4877165f, -0.1812029f),
            CameraRotation = new Rotator(-10.43983f, 5.20887E-06f, 60.26916f),
            VehiclePreviewCameraPosition = new Vector3(3696.354f, -2128.625f, 24.48018f),
            VehiclePreviewCameraDirection = new Vector3(-0.6823828f, 0.6801425f, -0.2678804f),
            VehiclePreviewCameraRotation = new Rotator(-15.53818f, 1.59509E-05f, 45.09421f),
            VehiclePreviewLocation = new SpawnPlace(new Vector3(3691.785f, -2124.905f, 22.82873f), 268.2128f),
            LicensePlatePreviewText = "AUTO ERO",
            StateID = StaticStrings.AlderneyStateID,
            VehicleDeliveryLocations = new List<SpawnPlace>()
            {
                    new SpawnPlace(new Vector3(3710.192f, -2145.086f, 22.66184f), 359.2362f),
                    new SpawnPlace(new Vector3(3706.958f, -2144.313f, 22.66813f), 358.105f),
                    new SpawnPlace(new Vector3(3709.795f, -2115.735f, 22.66945f), 181.9883f),
                    new SpawnPlace(new Vector3(3705.328f, -2116.343f, 22.67257f), 181.2317f),
                    new SpawnPlace(new Vector3(3701.817f, -2115.998f, 22.64801f), 177.9948f),
            }
        };
        LibertyCityLocations.CarDealerships.Add(AutoEroticar);
        Dealership ElitásTravel = new Dealership(new Vector3(7346.312f, -2520.609f, 5.808244f), 226.975f, "Elitás Travel", "There's first class and then there's Elitas", "LCElitasMenu")
        {
            CameraPosition = new Vector3(7349.475f, -2529.174f, 9.519826f),
            CameraDirection = new Vector3(-0.2934668f, 0.9230703f, -0.2486332f),
            CameraRotation = new Rotator(-14.39665f, 4.186903E-05f, 17.63674f),
            BannerImagePath = "stores\\elitastravel.png",
            VehiclePreviewCameraPosition = new Vector3(7380.006f, -2505.675f, 9.491469f),
            VehiclePreviewCameraDirection = new Vector3(-0.1739272f, -0.9525952f, -0.249623f),
            VehiclePreviewCameraRotation = new Rotator(-14.4552f, 1.080064E-05f, 169.6528f),
            VehiclePreviewLocation = new SpawnPlace(new Vector3(7378.077f, -2519.028f, 5.701517f), 318.653f),
            LicensePlatePreviewText = "Elitaz",
            StateID = StaticStrings.LibertyStateID,
            VehicleDeliveryLocations = new List<SpawnPlace>()
            {
                    new SpawnPlace(new Vector3(7425.58f, -2466.061f, 5.69969f), 318.255f),
                    new SpawnPlace(new Vector3(7450.189f, -2478.324f, 5.701986f), 0.5567576f),
            }
        };
        LibertyCityLocations.CarDealerships.Add(ElitásTravel);
        Dealership LuxuryAutos = new Dealership(new Vector3(5240.563f, -2447.86f, 14.65696f), 40.51297f, "Luxury Autos", "You sure you can afford this?", "IVLuxuryMenu")
        {
            CameraPosition = new Vector3(5232.968f, -2440.033f, 20.04572f),
            CameraDirection = new Vector3(0.6726631f, -0.7206753f, -0.1677838f),
            CameraRotation = new Rotator(-9.658987f, -5.629331E-06f, -136.9735f),
            BannerImagePath = "stores\\luxuryautos.png",
            VehiclePreviewCameraPosition = new Vector3(5247.191f, -2457.397f, 17.72164f),
            VehiclePreviewCameraDirection = new Vector3(-0.3629273f, -0.9310324f, -0.03824268f),
            VehiclePreviewCameraRotation = new Rotator(-2.191679f, -3.748674E-05f, 158.7036f),
            VehiclePreviewLocation = new SpawnPlace(new Vector3(5244.374f, -2464.684f, 16.2247829f), 59.47708f),
            LicensePlatePreviewText = "LUX AUTO",
            StateID = StaticStrings.LibertyStateID,
            VehicleDeliveryLocations = new List<SpawnPlace>()
            {
                    new SpawnPlace(new Vector3(5233.603f, -2478.32f, 14.22272f), 358.9026f),
                    new SpawnPlace(new Vector3(5233.819f, -2470.182f, 14.21285f), 358.4313f),
                    new SpawnPlace(new Vector3(5268.92f, -2440.774f, 14.21239f), 91.20781f),
                    new SpawnPlace(new Vector3(5249.99f, -2440.365f, 14.22216f), 90.36872f),
            }
        };
        LibertyCityLocations.CarDealerships.Add(LuxuryAutos);
        Dealership WestdykeAutos = new Dealership(new Vector3(4119.343f, -1795.858f, 24.56643f), 217.3433f, "Westdyke Autos", "", "IVMotorcyclesMenu")
        {
            CameraPosition = new Vector3(4132.356f, -1793.774f, 29.45f),
            CameraDirection = new Vector3(-0.8982348f, 0.3947144f, -0.1933256f),
            CameraRotation = new Rotator(-11.14693f, 2.828118E-05f, 66.27771f),
            VehiclePreviewCameraPosition = new Vector3(4107.402f, -1788.207f, 26.57295f),
            VehiclePreviewCameraDirection = new Vector3(0.05000588f, 0.9061947f, -0.4198935f),
            VehiclePreviewCameraRotation = new Rotator(-24.82786f, 7.055409E-07f, -3.158508f),
            VehiclePreviewLocation = new SpawnPlace(new Vector3(4107.165f, -1784.14f, 24.13877f), 225.756f),
            LicensePlatePreviewText = "DYKEAUTO",
            StateID = StaticStrings.AlderneyStateID,
            VehicleDeliveryLocations = new List<SpawnPlace>()
            {
                    new SpawnPlace(new Vector3(4130.476f, -1788.203f, 23.6726f), 356.8591f),
                    new SpawnPlace(new Vector3(4112.023f, -1803.687f, 23.67073f), 93.08408f),
                    new SpawnPlace(new Vector3(4102.874f, -1803.63f, 23.67176f), 89.73994f),
                    new SpawnPlace(new Vector3(4091.624f, -1791.169f, 23.91193f), 4.39786f),
            }
        };
        LibertyCityLocations.CarDealerships.Add(WestdykeAutos);
        Dealership HappyIslandBoats = new Dealership(new Vector3(4839.993f, -3761.354f, 4.943236f), 118.4938f, "Happyness Island Boats", "", "HappyBoatMenu")
        {
            CameraPosition = new Vector3(4835.881f, -3767.187f, 7.664476f),
            CameraDirection = new Vector3(-0.8783931f, -0.4651018f, -0.1100269f),
            CameraRotation = new Rotator(-6.316864f, -8.375142E-06f, 117.9008f),
            VehiclePreviewCameraPosition = new Vector3(4799.101f, -3790.096f, 4.373492f),
            VehiclePreviewCameraDirection = new Vector3(0.1013948f, 0.9340025f, -0.3425763f),
            VehiclePreviewCameraRotation = new Rotator(-20.03391f, -6.815721E-07f, -6.195738f),
            VehiclePreviewLocation = new SpawnPlace(new Vector3(4799.704f, -3779.6f, 0.3023784f), 117.4393f),
            LicensePlatePreviewText = "HAPPY",
            StateID = StaticStrings.LibertyStateID,
            VehicleDeliveryLocations = new List<SpawnPlace>()
            {
                    new SpawnPlace(new Vector3(4805.647f, -3790.102f, 0.2267452f), 26.97948f),
                    new SpawnPlace(new Vector3(4814.057f, -3806.635f, 0.3559909f), 26.90115f),
                    new SpawnPlace(new Vector3(4810.625f, -3823.273f, 0.2772601f), 29.95905f),
            }
        };
        LibertyCityLocations.CarDealerships.Add(HappyIslandBoats);
    }
    private void DefaultConfig_FoodStands()
    {
        LibertyCityLocations.FoodStands.AddRange(new List<FoodStand>()
        {
            new FoodStand(new Vector3(3764.164f, -2379.106f, 19.5643f), 94.47754f,   "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.AlderneyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(3764.164f, -2379.106f, 17.5643f), 94.47754f) }, BannerImagePath = "stores\\chihuahuahotdogs.png", ActivateDistance = 125 },
            new FoodStand(new Vector3(3927.782f, -2374.98f, 19.56618f), 0.5199404f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.AlderneyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(3927.782f, -2374.98f, 17.56618f), 0.5199404f) }, BannerImagePath = "stores\\chihuahuahotdogs.png", ActivateDistance = 125 },
            new FoodStand(new Vector3(3924f, -2400.369f, 19.56748f), 183.4882f,      "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.AlderneyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(3924f, -2400.369f, 17.56748f), 183.4882f) }, BannerImagePath = "stores\\chihuahuahotdogs.png", ActivateDistance = 125 },
            new FoodStand(new Vector3(3923.427f, -2373.106f, 19.56552f), 182.0979f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.AlderneyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(3923.427f, -2373.106f, 17.56552f), 182.0979f) }, BannerImagePath = "stores\\chihuahuahotdogs.png", ActivateDistance = 125 },
            new FoodStand(new Vector3(3961.095f, -2326.865f, 19.56738f), 270.0331f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.AlderneyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(3961.095f, -2326.865f, 17.56738f), 270.0331f) }, BannerImagePath = "stores\\chihuahuahotdogs.png", ActivateDistance = 125 },
            new FoodStand(new Vector3(4014.677f, -2322.67f, 19.55834f), 92.29472f,   "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.AlderneyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4014.677f, -2322.67f, 17.55834f), 92.29472f) }, BannerImagePath = "stores\\chihuahuahotdogs.png", ActivateDistance = 125 },
            new FoodStand(new Vector3(4049.692f, -2149.427f, 13.71902f), 180.2094f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.AlderneyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4049.692f, -2149.427f, 11.71902f), 180.2094f) }, BannerImagePath = "stores\\chihuahuahotdogs.png", ActivateDistance = 125 },
            new FoodStand(new Vector3(4116.685f, -1800.947f, 24.47414f), 177.1595f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.AlderneyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4116.685f, -1800.947f, 22.47414f), 177.1595f) }, BannerImagePath = "stores\\chihuahuahotdogs.png", ActivateDistance = 125 },
            new FoodStand(new Vector3(4369.37f, -2256.757f, 15.62734f), 172.7887f,   "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.AlderneyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4369.37f, -2256.757f, 13.62734f), 172.7887f) }, BannerImagePath = "stores\\chihuahuahotdogs.png", ActivateDistance = 125 },
            new FoodStand(new Vector3(4376.152f, -2280.124f, 15.63034f), 357.1571f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.AlderneyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4376.152f, -2280.124f, 13.63034f), 357.1571f) }, BannerImagePath = "stores\\chihuahuahotdogs.png", ActivateDistance = 125 },

            new FoodStand(new Vector3(4964.538f, -4073.773f, 5.275001f), 155.9896f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4964.538f, -4073.773f, 3.275001f), 155.9896f) }, BannerImagePath = "stores\\chihuahuahotdogs.png", ActivateDistance = 125 },
            new FoodStand(new Vector3(5649.894f, -3775.677f, 5.818119f), 271.4748f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(5649.894f, -3775.677f, 3.818119f), 271.4748f) }, BannerImagePath = "stores\\chihuahuahotdogs.png", ActivateDistance = 125 },
            new FoodStand(new Vector3(5554.281f, -3770.153f, 5.818308f), 88.84018f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(5554.281f, -3770.153f, 3.818308f), 88.84018f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(5002.949f, -4182.1f, 4.678008f), 89.2317f,     "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(5002.949f, -4182.1f, 2.678008f), 89.2317f) }, BannerImagePath = "stores\\chihuahuahotdogs.png", ActivateDistance = 125 },
            new FoodStand(new Vector3(5108.134f, -4128.581f, 5.118035f), 249.8867f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(5108.134f, -4128.581f, 3.118035f), 249.8867f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(5373.667f, -2460.935f, 8.450697f), 93.10441f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(5373.667f, -2460.935f, 6.450697f), 93.10441f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(5327.785f, -2373.776f, 14.71362f), 127.8357f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(5327.785f, -2373.776f, 12.71362f), 127.8357f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(5330.492f, -2312.42f, 14.71319f), 218.3461f,   "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(5330.492f, -2312.42f, 12.71319f), 218.3461f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(5119.802f, -2470.044f, 14.68384f), 257.922f,   "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(5119.802f, -2470.044f, 12.68384f), 257.922f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(5118.813f, -2440.076f, 14.68385f), 265.2967f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(5118.813f, -2440.076f, 12.68385f), 265.2967f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(5118.624f, -2401.291f, 14.6858f), 264.8643f,   "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(5118.624f, -2401.291f, 12.6858f), 264.8643f) }, BannerImagePath = "stores\\chihuahuahotdogs.png", ActivateDistance = 125 },

            new FoodStand(new Vector3(4486.943f, -2271.326f, 4.104059f), 248.2815f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4486.943f, -2271.326f, 2.104059f), 248.2815f) }, BannerImagePath = "stores\\chihuahuahotdogs.png", ActivateDistance = 125 },
            new FoodStand(new Vector3(4841.846f, -2119.576f, 14.7864f), 47.56853f,   "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4841.846f, -2119.576f, 12.7864f), 47.56853f) }, BannerImagePath = "stores\\chihuahuahotdogs.png", ActivateDistance = 125},
            new FoodStand(new Vector3(4871.243f, -2573.714f, 12.40497f), 173.105f,   "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4871.243f, -2573.714f, 10.40497f), 173.105f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(4896.509f, -2583.252f, 12.40314f), 119.0982f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4896.509f, -2583.252f, 10.40314f), 119.0982f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(4941.343f, -2551.693f, 10.18839f), 4.123949f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4941.343f, -2551.693f, 8.18839f), 4.123949f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(4727.716f, -2572.558f, 9.980865f), 96.82053f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4727.716f, -2572.558f, 7.980865f), 96.82053f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },

            new FoodStand(new Vector3(4845.509f, -2596.294f, 14.77549f), 96.88358f,   "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4845.509f, -2596.294f, 12.77549f), 96.88358f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(4895.825f, -2504.093f, 10.96102f), 94.84795f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4895.825f, -2504.093f, 8.96102f), 94.84795f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(4970.145f, -2551.682f, 10.18838f), 349.4966f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4970.145f, -2551.682f, 8.18838f), 349.4966f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(4976.159f, -2520.493f, 7.171853f), 88.18343f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4976.159f, -2520.493f, 5.171853f), 88.18343f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(4929.512f, -2478.018f, 7.171856f), 270.2258f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4929.512f, -2478.018f, 5.171856f), 270.2258f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(4929.168f, -2499.263f, 7.171856f), 267.7767f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4929.168f, -2499.263f, 5.171856f), 267.7767f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(5125.712f, -3066.832f, 14.77077f), 98.81847f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(5125.712f, -3066.832f, 12.77077f), 98.81847f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(5031.34f, -3105.664f, 14.79097f), 2.084716f,   "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(5031.34f, -3105.664f, 12.79097f), 2.084716f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(5065.052f, -3221.884f, 14.81567f), 3.154422f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(5065.052f, -3221.884f, 12.81567f), 3.154422f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(4974.084f, -2901.484f, 15.03517f), 301.1096f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4974.084f, -2901.484f, 13.03517f), 301.1096f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" ,ActivateDistance = 125},
            new FoodStand(new Vector3(4919.353f, -2786.124f, 14.80801f), 292.7915f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4919.353f, -2786.124f, 12.80801f), 292.7915f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(4622.376f, -2374.878f, 9.937297f), 259.0821f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4622.376f, -2374.878f, 7.937297f), 259.0821f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(4843.623f, -3586.408f, 5.006824f), 2.013458f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4843.623f, -3586.408f, 3.006824f), 2.013458f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },

            new FoodStand(new Vector3(4643.13f, -3123.829f, 4.662454f), 286.6137f,   "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4643.13f, -3123.829f, 2.662454f), 286.6137f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(4638.184f, -3292.149f, 4.662499f), 238.3148f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4638.184f, -3292.149f, 2.662499f), 238.3148f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(4636.031f, -3191.277f, 4.662412f), 227.1086f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4636.031f, -3191.277f, 2.662412f), 227.1086f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(4547.341f, -3075.049f, 4.811528f), 234.4335f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4547.341f, -3075.049f, 2.811528f), 234.4335f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(4800.129f, -2662.632f, 14.81484f), 340.9631f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4800.129f, -2662.632f, 12.81484f), 340.9631f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(4740.749f, -2110.356f, 12.87756f), 223.3621f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4740.749f, -2110.356f, 10.87756f), 223.3621f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(4726.802f, -2261.841f, 9.963533f), 291.2136f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4726.802f, -2261.841f, 7.963533f), 291.2136f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(4857.351f, -3111.072f, 14.75848f), 6.963999f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4857.351f, -3111.072f, 12.75848f), 6.963999f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(4824.064f, -3762.798f, 4.342792f), 72.6163f,   "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4824.064f, -3762.798f, 2.342792f), 72.6163f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(6088.357f, -3922.165f, 15.79217f), 9.639594f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(6088.357f, -3922.165f, 13.79217f), 9.639594f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(6166.597f, -3937.217f, 16.44052f), 106.8126f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(6166.597f, -3937.217f, 14.44052f), 106.8126f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(6258.046f, -3059.1f, 31.45988f), 48.02742f,    "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(6258.046f, -3059.1f, 29.45988f), 48.02742f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
        
            // Happiness Island
            new FoodStand(new Vector3(4701.242f, -4203.466f, 4.838318f), 178.6166f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4701.242f, -4203.466f, 2.838318f), 178.6166f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(4601.658f, -4187.598f, 4.838317f), 115.3873f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4601.658f, -4187.598f, 2.838317f), 115.3873f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(4600.629f, -4072.384f, 4.838336f), 148.2185f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4600.629f, -4072.384f, 2.838336f), 148.2185f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(4553.761f, -4092.374f, 4.838315f), 275.4155f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4553.761f, -4092.374f, 2.838315f), 275.4155f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(4519.171f, -4193.805f, 4.836827f), 303.0159f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4519.171f, -4193.805f, 2.836827f), 303.0159f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(4475.929f, -4073.099f, 4.83774f), 87.76134f,   "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4475.929f, -4073.099f, 2.83774f), 87.76134f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(4489.527f, -4027.499f, 4.837744f), 124.5004f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4489.527f, -4027.499f, 2.837744f), 124.5004f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(4512.241f, -3956.745f, 4.836913f), 53.89818f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4512.241f, -3956.745f, 2.836913f), 53.89818f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(4579.527f, -3916.3f, 4.836993f), 124.7635f,    "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4579.527f, -3916.3f, 2.836993f), 124.7635f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(4663.002f, -4033.245f, 4.837823f), 258.235f,   "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4663.002f, -4033.245f, 2.837823f), 258.235f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(4674.569f, -4150.211f, 4.838315f), 178.4433f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4674.569f, -4150.211f, 2.838315f), 178.4433f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            //Meadow Park
            new FoodStand(new Vector3(6604.738f, -2652.622f, 32.01118f), 90.88161f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(6604.738f, -2652.622f, 30.01118f), 90.88161f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(6615.871f, -2690.543f, 29.29945f), 239.7237f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(6615.871f, -2690.543f, 27.29945f), 239.7237f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(6674.147f, -2650.532f, 29.28254f), 196.5724f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(6674.147f, -2650.532f, 27.28254f), 196.5724f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(6734.191f, -2713.041f, 29.29984f), 174.3951f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(6734.191f, -2713.041f, 27.29984f), 174.3951f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(6676.568f, -2753.618f, 29.29869f), 17.62746f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(6676.568f, -2753.618f, 27.29869f), 17.62746f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(6715.57f, -2477.022f, 28.39465f), 171.0414f,   "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(6715.57f, -2477.022f, 26.39465f), 171.0414f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(6622.551f, -2475.772f, 28.14632f), 197.3101f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(6622.551f, -2475.772f, 26.14632f), 197.3101f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            new FoodStand(new Vector3(6739.272f, -2356.551f, 15.90711f), 169.0592f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(6739.272f, -2356.551f, 13.90711f), 169.0592f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },
            //Steinway Park
            new FoodStand(new Vector3(6041.902f, -2531.711f, 7.893699f), 271.1903f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(6041.902f, -2531.711f, 5.893699f), 271.1903f) }, BannerImagePath = "stores\\chihuahuahotdogs.png",ActivateDistance = 125 },


            // Beefy Locations
            new FoodStand(new Vector3(4954.202f, -2143.93f, 4.92061f), 302.7637f,    "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4954.202f, -2143.93f, 4.92061f), 302.7637f) }, BannerImagePath = "stores\\beefybills.png", ActivateDistance = 125 },
            new FoodStand(new Vector3(4854.289f, -2106.064f, 14.78162f), 318.5865f,  "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4854.289f, -2106.064f, 12.78162f), 318.5865f) }, BannerImagePath = "stores\\beefybills.png", ActivateDistance = 125 },
            new FoodStand(new Vector3(4879.539f, -2218.058f, 10.01271f), 236.0195f,  "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4879.539f, -2218.058f, 8.01271f), 236.0195f) }, BannerImagePath = "stores\\beefybills.png", ActivateDistance = 125 },
            new FoodStand(new Vector3(4930.285f, -2251.552f, 6.161006f), 215.1984f,  "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4930.285f, -2251.552f, 4.161006f), 215.1984f) }, BannerImagePath = "stores\\beefybills.png", ActivateDistance = 125 },
            new FoodStand(new Vector3(4965.702f, -2228.463f, 6.158602f), 229.2811f,  "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4965.702f, -2228.463f, 4.158602f), 229.2811f) }, BannerImagePath = "stores\\beefybills.png", ActivateDistance = 125 },
            new FoodStand(new Vector3(4904.267f, -2514.642f, 10.96101f), 272.042f,   "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4904.267f, -2514.642f, 8.96101f), 272.042f) }, BannerImagePath = "stores\\beefybills.png", ActivateDistance = 125 },
            new FoodStand(new Vector3(5083.331f, -2596.737f, 13.06989f), 99.01041f,  "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(5083.331f, -2596.737f, 11.06989f), 99.01041f) }, BannerImagePath = "stores\\beefybills.png", ActivateDistance = 125},
            new FoodStand(new Vector3(5070.358f, -2225.922f, 6.308374f), 354.0518f,  "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(5070.358f, -2225.922f, 4.308374f), 354.0518f) }, BannerImagePath = "stores\\beefybills.png", ActivateDistance = 125},
            new FoodStand(new Vector3(5267.058f, -4028.505f, 4.940442f), 301.3558f,  "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(5267.058f, -4028.505f, 2.940442f), 301.3558f) }, BannerImagePath = "stores\\beefybills.png", ActivateDistance = 125 },
            new FoodStand(new Vector3(5370.958f, -3969.699f, 4.93968f), 125.3503f,   "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(5370.958f, -3969.699f, 2.93968f), 125.3503f) }, BannerImagePath = "stores\\beefybills.png", ActivateDistance = 125 },
            new FoodStand(new Vector3(6427.57f, -2761.347f, 28.56134f), 87.07668f,   "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(6427.57f, -2761.347f, 26.56134f), 87.07668f) }, BannerImagePath = "stores\\beefybills.png", ActivateDistance = 125 },
            new FoodStand(new Vector3(5024.62f, -3681.164f, 14.57278f), 29.9035f,    "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(5024.62f, -3681.164f, 12.57278f), 29.9035f) }, BannerImagePath = "stores\\beefybills.png", ActivateDistance = 125 },
            new FoodStand(new Vector3(6509.898f, -2862.332f, 22.51945f), 86.63126f,  "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(6509.898f, -2862.332f, 20.51945f), 86.63126f) }, BannerImagePath = "stores\\beefybills.png", ActivateDistance = 125 },
            new FoodStand(new Vector3(4604.935f, -3494.341f, 6.999878f), 27.44824f,  "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4604.935f, -3494.341f, 4.999878f), 27.44824f) }, BannerImagePath = "stores\\beefybills.png", ActivateDistance = 125 },

          //new FoodStand(new Vector3(5596.587f, -3799.461f, 5.817865f), 181.1278f,  "The Nut House", "Totally NUTS!","NeedAMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(5596.587f, -3799.461f, 3.817865f), 181.1278f) },ActivateCells = 3, ActivateDistance = 125 },
          //new FoodStand(new Vector3(5595.608f, -3726.252f, 5.818119f), 2.443711f,  "The Nut House", "Totally NUTS!","NeedAMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(5595.608f, -3726.252f, 3.818119f), 2.443711f) },ActivateCells = 3, ActivateDistance = 125 },
          //new FoodStand(new Vector3(5118.388f, -2460.465f, 14.68384f), 99.073f,    "The Nut House", "Totally NUTS!","NeedAMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(5118.388f, -2460.465f, 12.68384f), 99.073f) },ActivateCells = 3, ActivateDistance = 125 },
          //new FoodStand(new Vector3(4975.313f, -2494.541f, 7.171856f), 79.82909f,  "The Nut House", "Totally NUTS!","NeedAMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4975.313f, -2494.541f, 5.171856f), 79.82909f) },ActivateCells = 3, ActivateDistance = 125 },
          //new FoodStand(new Vector3(4895.808f, -2492.492f, 10.96046f), 3.447456f,  "The Nut House", "Totally NUTS!","NeedAMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4895.808f, -2492.492f, 8.96046f), 3.447456f) },ActivateCells = 3, ActivateDistance = 125 },
          //new FoodStand(new Vector3(4937.849f, -2544.546f, 10.18839f), 250.9697f,  "The Nut House", "Totally NUTS!","NeedAMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4937.849f, -2544.546f, 8.18839f), 250.9697f) },ActivateCells = 3, ActivateDistance = 125 },
          //new FoodStand(new Vector3(4965.651f, -2218.049f, 6.180485f), 192.1613f,  "The Nut House", "Totally NUTS!","NeedAMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4965.651f, -2218.049f, 4.180485f), 192.1613f) },ActivateCells = 3, ActivateDistance = 125 },
          //new FoodStand(new Vector3(4847.869f, -2111.39f, 14.78163f), 46.97323f,   "The Nut House", "Totally NUTS!","NeedAMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4847.869f, -2111.39f, 12.78163f), 46.97323f) },ActivateCells = 3, ActivateDistance = 125 },
          //new FoodStand(new Vector3(4654.063f, -4218.773f, 4.838318f), 11.61439f,  "The Nut House", "Totally NUTS!","NeedAMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4654.063f, -4218.773f, 2.838318f), 11.61439f) },ActivateCells = 3, ActivateDistance = 125 },
          //new FoodStand(new Vector3(4693.203f, -4149.691f, 4.838315f), 203.7223f,  "The Nut House", "Totally NUTS!","NeedAMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4693.203f, -4149.691f, 2.838315f), 203.7223f) },ActivateCells = 3, ActivateDistance = 125 },
        });
    }
    private void DefaultConfig_FireStations()
    {
        List<FireStation> FireStations = new List<FireStation>()
        {
            new FireStation(new Vector3(3027.585f, -3092.118f, 12.04712f), 2.321193f,"Tudor Fire Station","")
            {
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.AlderneyStateID,
                ActivateDistance = 200,
                AssignedAssociationID = "FDLC",
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(3031.13281f, -3090.189f, 12.0479326f), 323.3341f, 35f),
                    new FireConditionalLocation(new Vector3(3022.66675f, -3090.08545f, 12.0479326f), 43.30471f, 35f),
                    new FireConditionalLocation(new Vector3(3019.776f, -3102.211f, 12.0479221f), 128.3218f, 35f),
                    new FireConditionalLocation(new Vector3(3019.67773f, -3116.85059f, 12.0479326f), 102.2719f, 35f),
                    new FireConditionalLocation(new Vector3(3045.16162f, -3094.07813f, 12.0479221f), 335.8226f, 35f),
                    new FireConditionalLocation(new Vector3(3076.6958f, -3098.45459f, 12.0479326f), 353.951f, 35f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(3070.81372f, -3095.446f, 12.0479326f), 179.0583f, 35f),
                    new FireConditionalLocation(new Vector3(3065.26074f, -3096.06885f, 12.0479326f), 0.8778898f, 35f),
                    new FireConditionalLocation(new Vector3(3058.5127f, -3095.23877f, 12.0479326f), 0.4668479f, 35f),
                    new FireConditionalLocation(new Vector3(3049.8208f, -3091.55054f, 12.0479326f), 177.1563f, 35f),
                    new FireConditionalLocation(new Vector3(3030.94482f, -3104.813f, 12.0479221f), 89.35287f, 35f),
                    new FireConditionalLocation(new Vector3(3030.83081f, -3110.735f, 12.0479221f), 87.76639f, 35f),
                },
            },
            new FireStation(new Vector3(3617.33f, -2705.854f, 25.44455f), 90.39238f,"Berchem Fire Station","")
            {
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.AlderneyStateID,
                ActivateDistance = 200,
                AssignedAssociationID = "FDLC",
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(3615.067f, -2697.017f, 25.4441242f), 61.3048f, 75f),
                    new FireConditionalLocation(new Vector3(3615.01172f, -2708.456f, 25.4441242f), 125.7424f, 75f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(3611.38184f, -2702.39722f, 25.3589935f), 89.76939f, 40f),
                    new FireConditionalLocation(new Vector3(3608.27173f, -2710.59229f, 25.3340645f), 355.698f, 40f),
                },
            },
            new FireStation(new Vector3(6133.727f, -3154.344f, 35.04887f), 1.255172f,"Broker Fire Station","")
            {
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
                ActivateDistance = 200,
                InteriorID = 160514,
                AssignedAssociationID = "FDLC",
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(6140.86865f, -3151.375f, 34.74216f), 311.8359f, 40f),
                    new FireConditionalLocation(new Vector3(6126.045f, -3151.17627f, 35.13439f), 34.28785f, 40f),
                    new FireConditionalLocation(new Vector3(6137.64551f, -3179.85815f, 34.999f), 54.14355f, 40f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(6137.59766f, -3167.17188f, 34.99892f), 1.017775f, 40f),
                    new FireConditionalLocation(new Vector3(6129.541f, -3168.156f, 34.99905f), 2.09237f, 40f),
                },
            },
            new FireStation(new Vector3(7528.424f, -3071.573f, 5.808009f), 63.17135f,"FIA Fire Station","")
            {
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
                ActivateDistance = 200,
                AssignedAssociationID = "FDLC",
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(7524.008f, -3075.93848f, 5.80775928f), 140.599f, 40f),
                    new FireConditionalLocation(new Vector3(7519.35645f, -3072.985f, 5.807754f), 90.42297f, 40f),
                    new FireConditionalLocation(new Vector3(7525.627f, -3084.75513f, 5.807754f), 122.4748f, 40f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(7511.292f, -3082.12256f, 5.807756f), 89.19281f, 40f),
                    new FireConditionalLocation(new Vector3(7502.56055f, -3071.94043f, 5.807756f), 137.5261f, 40f),
                },
            },
            new FireStation(new Vector3(6302.511f, -1538.736f, 10.56036f), 138.4435f,"Bohan Fire Station","")
            {
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
                ActivateDistance = 200,
                AssignedAssociationID = "FDLC",
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(6304.819f, -1543.89624f, 10.5296526f), 181.0731f, 40f),
                    new FireConditionalLocation(new Vector3(6298.464f, -1541.17407f, 10.5602322f), 128.9855f, 40f),
                    new FireConditionalLocation(new Vector3(6322.46875f, -1527.61011f, 10.4753723f), 260.1082f, 40f),
                    new FireConditionalLocation(new Vector3(6293.84375f, -1514.76221f, 10.5261822f), 213.5798f, 40f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(6304.751f, -1519.90918f, 10.5261927f), 224.0606f, 40f),
                    new FireConditionalLocation(new Vector3(6299.83f, -1525.99219f, 10.525753f), 226.8957f, 40f),
                    new FireConditionalLocation(new Vector3(6317.92f, -1510.98828f, 10.4436426f), 224.0108f, 40f),
                },
            },
            new FireStation(new Vector3(4915.746f, -1714.24f, 20.41517f), 266.0019f,"Northwood Fire Station","")
            {
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
                ActivateDistance = 200,
                AssignedAssociationID = "FDLC",
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(4918.067f, -1717.14514f, 20.4150429f), 249.7648f, 40f),
                    new FireConditionalLocation(new Vector3(4918.338f, -1709.2262f, 20.4150429f), 286.9591f, 40f),
                    new FireConditionalLocation(new Vector3(4914.73535f, -1746.20923f, 20.4359436f), 231.1253f, 40f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(4918.4f, -1732.27222f, 20.2653027f), 269.4188f, 40f),
                    new FireConditionalLocation(new Vector3(4918.62842f, -1723.27319f, 20.2645035f), 268.5182f, 40f),
                },
            },
            new FireStation(new Vector3(5473.807f, -3636.023f, 4.959463f), 268.3077f,"Fish Market Fire Station","")
            {
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
                ActivateDistance = 200,
                AssignedAssociationID = "FDLC",
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(5476.01025f, -3638.743f, 4.959816f), 251.4846f, 40f),
                    new FireConditionalLocation(new Vector3(5477.672f, -3632.352f, 4.959814f), 288.2192f, 40f),
                    new FireConditionalLocation(new Vector3(5477.52734f, -3594.25415f, 4.956955f), 354.2037f, 40f),
                    new FireConditionalLocation(new Vector3(5476.60938f, -3578.00977f, 4.946926f), 350.0849f, 40f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(5477.172f, -3621.26221f, 4.80979824f), 180.3352f, 35f),
                    new FireConditionalLocation(new Vector3(5477.628f, -3609.425f, 4.809795f), 173.7421f, 35f),
                },
            },
            new FireStation(new Vector3(4806.832f, -3031.623f, 14.76467f), 271.6995f,"Star Junction Fire Station","")
            {
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
                ActivateDistance = 200,
                AssignedAssociationID = "FDLC",
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(4808.263f, -3038.14038f, 14.7681923f), 253.1116f, 35f),
                    new FireConditionalLocation(new Vector3(4809.552f, -3026.56616f, 14.7681923f), 314.6575f, 35f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(4813.45166f, -3026.63965f, 14.6518726f), 178.2443f, 35f),
                },
            },
        };
        LibertyCityLocations.FireStations.AddRange(FireStations);
    }
    private void DefaultConfig_Forgers()
    {
        List<Forger> ForgerLC = new List<Forger>()
        {
            new Forger(new Vector3(6113.57f, -2673.297f, 22.27298f), 4.426835f, "Dukes Forgeries", "More plates than the DMV")
            {
                CleanPlateSalesPrice = 200,
                WantedPlateSalesPrice = 50,
                CustomPlateCost = 550,
                RandomPlateCost = 300,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Forger(new Vector3(3720.566f, -2443.306f, 19.56631f), 315.8226f, "Alderney Forgeries", "Who are you?!")
            {
                CleanPlateSalesPrice = 200,
                WantedPlateSalesPrice = 50,
                CustomPlateCost = 550,
                RandomPlateCost = 300,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.AlderneyStateID,
            },
        };
        LibertyCityLocations.Forgers.AddRange(ForgerLC);
    }
    private void DefaultConfig_GamblingDens()
    {
        GamblingParameters defaultParameters = PlacesOfInterest.GetParameters(1);
        List<GamblingDen> GamblingDens = new List<GamblingDen>()
        {
            new GamblingDen(new Vector3(3837.577f, -2349.666f, 19.81439f), 177.9918f,"Alderney City Casino","Test Your Luck... In more ways than one")//Regular Casino
            {
                GamblingParameters = new GamblingParameters()
                {
                    BlackJackGameRulesList = new List<BlackJackGameRules>()
                    {
                        new BlackJackGameRules("Blackjack","The Dealer",15,400,true,true, false, false),
                    },
                    RouletteGameRulesList = new List<RouletteGameRules>()
                    {
                        new RouletteGameRules("Roulette","The Dealer",15,450, false, false),
                    },
                },
                OpenTime = 0,
                CloseTime = 24,
                ShowsOnDirectory = true,
                WinLimit = 1000000,
                WinLimitResetHours = 24,
                TypeName = "Casino",
                MapIcon = 679,
                StateID = StaticStrings.AlderneyStateID,
                CameraPosition = new Vector3(3840.7f, -2359.488f, 23.90756f),
                CameraDirection = new Vector3(-0.5123138f, 0.8319463f, -0.2130725f),
                CameraRotation = new Rotator(-12.30247f, 1.74768E-05f, 31.62483f),
            },
            new GamblingDen(new Vector3(6409.233f, -2505.53f, 36.11827f), 359.6075f, "Inside Track", "")
            {
                GamblingParameters = new GamblingParameters()
                {
                    BlackJackGameRulesList = new List<BlackJackGameRules>()
                    {
                        new BlackJackGameRules("Blackjack","The Dealer",15,400,true,true, false, false),
                    },
                    RouletteGameRulesList = new List<RouletteGameRules>()
                    {
                        new RouletteGameRules("Roulette","The Dealer",15,450, false, false),
                    },
                },
                OpenTime = 0,
                CloseTime = 24,
                ShowsOnDirectory = true,
                WinLimit = 1000000,
                WinLimitResetHours = 24,
                TypeName = "Casino",
                MapIcon = 679,
                StateID = StaticStrings.LibertyStateID,
            },
            new GamblingDen(new Vector3(4923.345f, -2700.518f, 15.02516f), 178.7795f,"Messina Gentlemans Club","Take a load off!")//Messina Casino, fancy
            {
                GamblingParameters = PlacesOfInterest.GetParameters(3),
                WinLimit = 45000,
                WinLimitResetHours = 24,
                OpenTime = 18,
                CloseTime = 4,
                AssignedAssociationID = "AMBIENT_GANG_MESSINA",
                CameraPosition = new Vector3(4919.573f, -2708.145f, 18.70527f),
                CameraDirection = new Vector3(0.5012826f, 0.8067239f, -0.3129094f),
                CameraRotation = new Rotator(-18.23465f, 2.696743E-06f, -31.85602f),
                StateID = StaticStrings.LibertyStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(4918.90332f, -2703.976f, 14.8615026f), 154.4864f, 75f) { AssociationID = "AMBIENT_GANG_MESSINA"},
                    new GangConditionalLocation(new Vector3(4929.86f, -2704.05518f, 14.8614922f), 216.8216f, 75f) { AssociationID = "AMBIENT_GANG_MESSINA"},
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(3598.28076f, -3503.8335f, 60.47042f), 186.2948f, 40f) { IsEmpty = true, AssociationID = "AMBIENT_GANG_MESSINA"},
                },
            },
            new GamblingDen(new Vector3(5119.438f, -3803.626f, 16.27476f), 273.7027f,"Old Block Casino","A game of tradition")// Gambetti Casino, decnet condo
            {
                GamblingParameters = PlacesOfInterest.GetParameters(2),
                WinLimit = 35000,
                WinLimitResetHours = 24,
                OpenTime = 18,
                CloseTime = 4,
                AssignedAssociationID = "AMBIENT_GANG_GAMBETTI",
                CameraPosition = new Vector3(5136.774f, -3807.022f, 21.05791f),
                CameraDirection = new Vector3(-0.9252443f, 0.3698952f, -0.08426415f),
                CameraRotation = new Rotator(-4.833712f, -3.534386E-06f, 68.2094f),
                StateID = StaticStrings.LibertyStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(5122.17041f, -3801.48022f, 14.778573f), 237.9585f, 40f) { AssociationID = "AMBIENT_GANG_GAMBETTI"},
                    new GangConditionalLocation(new Vector3(5122.584f, -3807.777f, 14.7634325f), 244.0623f, 40f) { AssociationID = "AMBIENT_GANG_GAMBETTI"},
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {

                },
            },
            new GamblingDen(new Vector3(6429.076f, -1460.531f, 12.09885f), 89.52517f,"Garden Casino","See green in the garden")//Lupisella Casino, low end hotel
            {
                GamblingParameters = defaultParameters,
                WinLimit = 25000,
                WinLimitResetHours = 24,
                OpenTime = 18,
                CloseTime = 4,
                AssignedAssociationID = "AMBIENT_GANG_LUPISELLA",
                CameraPosition = new Vector3(6419.526f, -1453.811f, 15.72663f),
                CameraDirection = new Vector3(0.7008455f, -0.7028511f, -0.1217211f),
                CameraRotation = new Rotator(-6.991443f, 8.601696E-07f, -135.0819f),
                StateID = StaticStrings.LibertyStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(6423.39648f, -1463.75317f, 10.1228123f), 124.9453f, 40f) { AssociationID = "AMBIENT_GANG_LUPISELLA"},
                    new GangConditionalLocation(new Vector3(6426.21f, -1457.35718f, 10.0848122f), 40.22756f, 40f) { AssociationID = "AMBIENT_GANG_LUPISELLA"},
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(6420.842f, -1481.76709f, 10.3445225f), 0.8881978f, 40f) { IsEmpty = true, AssociationID = "AMBIENT_GANG_LUPISELLA"},
                },
            },
            new GamblingDen(new Vector3(3732.77f, -2749.18f, 20.79508f), 355.4877f,"Upstairs Casino","Make sure you bring cash")//Ancelotti Casino
            {
                GamblingParameters = defaultParameters,
                WinLimit = 20000,
                WinLimitResetHours = 24,
                OpenTime = 18,
                CloseTime = 4,
                AssignedAssociationID = "AMBIENT_GANG_ANCELOTTI",
                CameraPosition = new Vector3(3734.786f, -2740.788f, 24.69319f),
                CameraDirection = new Vector3(-0.4298181f, -0.8897368f, -0.153704f),
                CameraRotation = new Rotator(-8.841638f, -2.376113E-06f, 154.2155f),
                StateID = StaticStrings.AlderneyStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(3726.84473f, -2746.665f, 19.2951927f), 47.04644f, 40f) { AssociationID = "AMBIENT_GANG_ANCELOTTI"},
                    new GangConditionalLocation(new Vector3(3736.52783f, -2746.84814f, 18.3506432f), 305.5476f, 40f) { AssociationID = "AMBIENT_GANG_ANCELOTTI"},
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(3737.63379f, -2743.17114f, 18.2204037f), 272.0731f, 40f) { IsEmpty = true, AssociationID = "AMBIENT_GANG_ANCELOTTI"},
                },
            },
            new GamblingDen(new Vector3(5077.217f, -2007.957f, 22.79433f), 359.5596f,"Pavano Games","")//Pavano Casino
            {
                GamblingParameters = defaultParameters,
                WinLimit = 15000,
                WinLimitResetHours = 24,
                OpenTime = 18,
                CloseTime = 4,
                AssignedAssociationID = "AMBIENT_GANG_PAVANO",
                CameraPosition = new Vector3(5081.266f, -1998.263f, 24.7534f),
                CameraDirection = new Vector3(-0.5666944f, -0.8134838f, -0.1307729f),
                CameraRotation = new Rotator(-7.514255f, 4.951721E-06f, 145.1379f),
                StateID = StaticStrings.LibertyStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(5074.714f, -2003.0282f, 20.4311543f), 26.97159f, 40f) { AssociationID = "AMBIENT_GANG_PAVANO"},
                    new GangConditionalLocation(new Vector3(5081.56738f, -2001.85217f, 20.4312439f), 336.5046f, 40f) { AssociationID = "AMBIENT_GANG_PAVANO"},
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(5087.085f, -1998.28613f, 20.2966633f), 90.64448f, 40f) { IsEmpty = true, AssociationID = "AMBIENT_GANG_PAVANO"},
                },
            },
        };
        LibertyCityLocations.GamblingDens.AddRange(GamblingDens);
    }
    private void DefaultConfig_GangDens()
    {
        float spawnChance = 55f;
        List<GangDen> LCMafiaDens = new List<GangDen>() { };
        GangDen GambettiDen1 = new GangDen(new Vector3(5089.248f, -3518.861f, 12.25257f), 271.4136f, "Gambetti Safehouse", "", "GambettiDenMenu", "AMBIENT_GANG_GAMBETTI")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            BannerImagePath = "gangs\\gambetti.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 150,
            PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(5089.444f, -3520.56f, 12.25145f), 271.6726f, 65f) { TaskRequirements = TaskRequirements.Guard,ForcedScenarios = new List<string>() { "WORLD_HUMAN_LEANING_CASINO_TERRACE","WORLD_HUMAN_SMOKING_CLUBHOUSE"}, },
                    new GangConditionalLocation(new Vector3(5065.704f, -3532.146f, 13.1248f), 146.8777f, 55f) { TaskRequirements = TaskRequirements.Guard },//right by thingo
                    new GangConditionalLocation(new Vector3(5066.663f, -3510.517f, 12.68589f), 86.87569f, 55f){ TaskRequirements = TaskRequirements.Guard },
                },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(5061.394f, -3531.513f, 12.91168f), 1.40538f, 75f),
                }
        };//Little Italy Drusellas - Moved to rear entrance to remove conflicts with Restaurant
        GangDen PavanoDen1 = new GangDen(new Vector3(4975.327f, -2004.443f, 22.44869f), 358.6787f, "Pavano Safehouse", "", "PavanoDenMenu", "AMBIENT_GANG_PAVANO")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            BannerImagePath = "gangs\\pavano.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            MaxAssaultSpawns = 10,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 150,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(4977.396f, -2003.686f, 22.31105f), 0.471641f, 65f),
                new GangConditionalLocation(new Vector3(4966.706f, -2002.455f, 23.13915f), 41.84434f, 55f),
                new GangConditionalLocation(new Vector3(4980.801f, -2010.049f, 22.34935f), 305.2558f, 65f),
                new GangConditionalLocation(new Vector3(4982.159f, -2008.781f, 22.34982f), 122.4671f, 65f),
                new GangConditionalLocation(new Vector3(4975.710f, -2022.372f, 22.33404f), 180.6116f, 55f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(4971.017f, -1998.774f, 22.35652f), 91.5307f, 75f),
            }
        };//East HOlland
        GangDen LupisellaDen1 = new GangDen(new Vector3(6592.523f, -1545.071f, 17.33255f), 268.2316f, "Lupisella Safehouse", "", "LupisellaDenMenu", "AMBIENT_GANG_LUPISELLA")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            //MapIcon = 77,
            BannerImagePath = "gangs\\lupisella.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            MaxAssaultSpawns = 10,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 150,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(6613.499f, -1557.955f, 16.72649f), 88.71317f, 75f),
                new GangConditionalLocation(new Vector3(6593.395f, -1555.943f, 16.70844f), 269.6464f, 75f),
                new GangConditionalLocation(new Vector3(6590.334f, -1550.56f, 16.75762f), 277.2389f, 75f),
                new GangConditionalLocation(new Vector3(6590.795f, -1548.79f, 16.71884f), 256.7724f, 75f),
                new GangConditionalLocation(new Vector3(6593.578f, -1535.063f, 16.83609f), 270.6367f, 75f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(6622.432f, -1559.466f, 16.41534f), 4.778854f, 75f),
                new GangConditionalLocation(new Vector3(6617.45f, -1558.731f, 16.06796f), 180.0602f, 75f),
            }
        };//Bohan Safehouse
        GangDen MessinaDen1 = new GangDen(new Vector3(4914.885f, -2645.041f, 14.96604f), 0.8165467f, "Messina Safehouse", "", "MessinaDenMenu", "AMBIENT_GANG_MESSINA")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            //MapIcon = 78,
            BannerImagePath = "gangs\\messina.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            MaxAssaultSpawns = 20,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 150,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(4930.746f, -2643.422f, 14.86297f), 343.8556f, 75f),
                new GangConditionalLocation(new Vector3(4918.93f, -2642.828f, 14.8629f), 2.950402f, 75f),
                new GangConditionalLocation(new Vector3(4910.859f, -2643.086f, 14.83368f), 359.0121f, 75f),
                new GangConditionalLocation(new Vector3(4899.113f, -2644.731f, 14.71136f), 2.277319f, 75f),
                new GangConditionalLocation(new Vector3(4894.109f, -2645.163f, 14.86276f), 358.995f, 75f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(4897.256f, -2650.036f, 14.20194f), 359.0729f, 75f),
            }
        };//star junction building by majestic
        GangDen AncelottiDen1 = new GangDen(new Vector3(3723.785f, -2830.097f, 20.53486f), 89.17658f, "Ancelotti Safehouse", "", "AncelottiDenMenu", "AMBIENT_GANG_ANCELOTTI")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            //MapIcon = 76,
            BannerImagePath = "gangs\\ancelotti.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            MaxAssaultSpawns = 15,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 150,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(3720.001f, -2833.973f, 19.56542f), 91.13457f, 75f),
                new GangConditionalLocation(new Vector3(3719.764f, -2837.358f, 19.56542f), 85.07284f, 75f),
                new GangConditionalLocation(new Vector3(3723.958f, -2839.774f, 19.56543f), 91.41199f, 75f),
                new GangConditionalLocation(new Vector3(3745.322f, -2839.323f, 17.90656f), 2.805331f, 75f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(3714.355f, -2833.476f, 18.9657f), 0.4063269f, 75f),
            }
        };//acter in aldery
        LCMafiaDens.Add(GambettiDen1);
        LCMafiaDens.Add(PavanoDen1);
        LCMafiaDens.Add(LupisellaDen1);
        LCMafiaDens.Add(MessinaDen1);
        LCMafiaDens.Add(AncelottiDen1);
        LibertyCityLocations.GangDens.AddRange(LCMafiaDens);

        GangDen LostMainDen = new GangDen(new Vector3(3472.964f, -2902.062f, 26.15645f), 326.0742f, "Lost M.C. Clubhouse", "", "LostDenMenu", "AMBIENT_GANG_LOST")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            BannerImagePath = "gangs\\lostmc.png",
            OpenTime = 0,
            CloseTime = 24,
            InteriorID = 398593,
            IsEnabled = true,
            MaxAssaultSpawns = 25,
            IgnoreEntranceInteract = true,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 150,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(3476.736f, -2900.702f, 25.44468f), 328.574f, 75f),
                new GangConditionalLocation(new Vector3(3472.677f, -2894.33f, 25.4447f), 331.0665f, 75f),
                new GangConditionalLocation(new Vector3(3459.293f, -2914.137f, 25.4447f), 58.88255f, 75f),
                new GangConditionalLocation(new Vector3(3467.125f, -2897.624f, 25.42279f), 351.9369f, 75f),
                new GangConditionalLocation(new Vector3(3472.183f, -2897.778f, 25.44468f), 330.8472f, 75f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(3478.01f, -2896.451f, 24.75788f), 204.2542f, 75f),
                new GangConditionalLocation(new Vector3(3482.075f, -2898.846f, 24.75222f), 205.1445f, 75f),
            },
            // Lost Interior disables/removes map outside  // Preventing Vehicle Preview from working properly 
            VehiclePreviewLocation = new SpawnPlace(new Vector3(3439.695f, -2920.223f, 25.30804f), 239.213f),
            VehiclePreviewCameraPosition = new Vector3(3442.314f, -2925.696f, 27.24959f),
            VehiclePreviewCameraDirection = new Vector3(-0.2890621f, 0.9210953f, -0.260819f),
            VehiclePreviewCameraRotation = new Rotator(-15.11866f, -4.42192E-07f, 17.42312f),
            VehicleDeliveryLocations = new List<SpawnPlace>() {
                    new SpawnPlace(new Vector3(3426.549f, -2914.406f, 25.25528f), 240.7795f),
                    new SpawnPlace(new Vector3(3427.536f, -2910.832f, 25.25538f), 241.3223f),
                }
        };
        LibertyCityLocations.GangDens.Add(LostMainDen);

        GangDen TriadMainDen = new GangDen(new Vector3(5272.325f, -3650.976f, 15.20069f), 179.5049f, "Triad Den", "", "TriadsDenMenu", "AMBIENT_GANG_WEICHENG")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            BannerImagePath = "gangs\\triad.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            MaxAssaultSpawns = 15,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 150,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(5269.016f, -3654.193f, 14.76311f), 172.1063f, 75f),
                new GangConditionalLocation(new Vector3(5274.29f, -3653.62f, 14.76237f), 180.5162f, 75f),
                new GangConditionalLocation(new Vector3(5281.958f, -3654.64f, 14.76311f), 181.3041f, 75f),
                new GangConditionalLocation(new Vector3(5272.411f, -3652.855f, 14.76237f), 179.435f, 75f),

            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {

            }
        };
        LibertyCityLocations.GangDens.Add(TriadMainDen);

        GangDen YardieDen1 = new GangDen(new Vector3(6406.775f, -3145.563f, 36.8376f), 267.7253f, "Yardies Chill Spot", "", "YardiesDenMenu", "AMBIENT_GANG_YARDIES")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            BannerImagePath = "gangs\\yardies.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 150,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(6415.319f, -3130.903f, 34.51391f), 356.5761f, 75f),
                new GangConditionalLocation(new Vector3(6408.061f, -3143.409f, 36.30283f), 262.2985f, 75f),
                new GangConditionalLocation(new Vector3(6408.017f, -3147.08f, 36.41984f), 216.166f, 75f),
                new GangConditionalLocation(new Vector3(6405.749f, -3156.992f, 37.36814f), 171.4058f, 75f),
                new GangConditionalLocation(new Vector3(6413.933f, -3154.758f, 37.73215f), 173.0238f, 75f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(6403.137f, -3131.181f, 34.36509f), 266.7974f, 75f),
                new GangConditionalLocation(new Vector3(6413.722f, -3125.767f, 33.17024f), 355.7358f, 75f),
            }
        };
        LibertyCityLocations.GangDens.Add(YardieDen1);

        GangDen SpanishLordsDen1 = new GangDen(new Vector3(5946.654f, -1884.947f, 14.8882f), 181.282f, "Spanish Lords Den", "", "VarriosDenMenu", "AMBIENT_GANG_SPANISH")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            OpenTime = 0,
            CloseTime = 24,
            BannerImagePath = "gangs\\spanishlords.png",
            IsEnabled = true,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 150,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(5939.712f, -1882.605f, 14.25052f), 93.9593f, 75f),
                new GangConditionalLocation(new Vector3(5944.304f, -1887.127f, 14.24621f), 131.4944f, 75f),
                new GangConditionalLocation(new Vector3(5948.515f, -1886.514f, 14.24669f), 220.6338f, 75f),
                new GangConditionalLocation(new Vector3(5956.719f, -1888.828f, 14.23047f), 260.3401f, 75f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {

            }
        };
        LibertyCityLocations.GangDens.Add(SpanishLordsDen1);

        GangDen KoreanDen1 = new GangDen(new Vector3(4022.524f, -2203.372f, 13.7021f), 270.1788f, "Korean Mob Den", "", "KkangpaeDenMenu", "AMBIENT_GANG_KOREAN")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            OpenTime = 0,
            CloseTime = 24,
            BannerImagePath = "gangs\\koreanmob.png",
            IsEnabled = true,
            MaxAssaultSpawns = 20,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 150,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(4024.06f, -2216.289f, 13.60258f), 237.1797f, 75f),
                new GangConditionalLocation(new Vector3(4025.039f, -2205.917f, 13.60259f), 212.661f, 75f),
                new GangConditionalLocation(new Vector3(4025.279f, -2198.672f, 13.6023f), 299.7038f, 75f),
                new GangConditionalLocation(new Vector3(4028.146f, -2196.224f, 13.60259f), 272.5771f, 75f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(4029.467f, -2211.23f, 12.95334f), 178.0083f, 75f),
            }
        };
        LibertyCityLocations.GangDens.Add(KoreanDen1);

        GangDen NorthHollandDen1 = new GangDen(new Vector3(4782.549f, -1848.88f, 12.46692f), 359.1933f, "The North Holland Hustlers Den", "The OGs", "FamiliesDenMenu", "AMBIENT_GANG_HOLHUST")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            OpenTime = 0,
            CloseTime = 24,
            BannerImagePath = "gangs\\hollandhusters.png",
            IsEnabled = true,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 150,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(4783.868f, -1847.491f, 12.31792f), 294.3403f, 75f),
                new GangConditionalLocation(new Vector3(4776.034f, -1846.393f, 12.84437f), 352.9904f, 75f),
                new GangConditionalLocation(new Vector3(4769.24f, -1848.037f, 13.41644f), 355.9222f, 75f),
                new GangConditionalLocation(new Vector3(4770.575f, -1858.53f, 13.62674f), 277.4354f, 75f),
                new GangConditionalLocation(new Vector3(4775.585f, -1867.426f, 14.26672f), 92.31076f, 75f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(4773.052f, -1854.812f, 12.68792f), 359.3972f, 75f),
            }
        };
        LibertyCityLocations.GangDens.Add(NorthHollandDen1);

        GangDen PetrovicDen1 = new GangDen(new Vector3(6134.367f, -3683.005f, 16.17614f), 274.33f, "Petrovic Safehouse", "", "GambettiDenMenu", "AMBIENT_GANG_PETROVIC")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            OpenTime = 0,
            CloseTime = 24,
            BannerImagePath = "gangs\\petrovic.png",
            IsEnabled = true,
            MaxAssaultSpawns = 10,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 150,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(6135.896f, -3685.098f, 16.12366f), 264.6502f, 75f),
                new GangConditionalLocation(new Vector3(6136.663f, -3678.611f, 16.18747f), 243.3841f, 75f),
                new GangConditionalLocation(new Vector3(6146.086f, -3678.163f, 15.52352f), 180.9384f, 75f),
                new GangConditionalLocation(new Vector3(6143.573f, -3691.987f, 15.94602f), 262.384f, 75f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(6148.294f, -3684.13f, 14.97494f), 266.8151f, 75f),
                new GangConditionalLocation(new Vector3(6151.397f, -3692.32f, 15.39304f), 90.80344f, 75f),
            }
        };
        LibertyCityLocations.GangDens.Add(PetrovicDen1);

        GangDen AngelsDen = new GangDen(new Vector3(4564.356f, -2048.981f, 6.094771f), 356.4827f, "Angles of Death Clubhouse", "", "AngelsDenMenu", "AMBIENT_GANG_ANGELS")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            OpenTime = 0,
            CloseTime = 24,
            BannerImagePath = "gangs\\angelsofdeath.png",
            IsEnabled = true,
            InteriorID = 555777,
            MaxAssaultSpawns = 25,
            IgnoreEntranceInteract = true,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 150,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(4567.638f, -2047.026f, 6.134629f), 285.3344f, 75f),
                new GangConditionalLocation(new Vector3(4567.779f, -2036.331f, 6.141073f), 242.5716f, 75f),
                new GangConditionalLocation(new Vector3(4556.231f, -2046.971f, 6.059202f), 30.1048f, 75f),
                new GangConditionalLocation(new Vector3(4554.792f, -2047.266f, 6.059202f), 330.3784f, 75f),
                new GangConditionalLocation(new Vector3(4563.886f, -2032.357f, 6.060932f), 90.42188f, 75f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(4547.398f, -2053.243f, 5.723994f), 176.603f, 75f),
                new GangConditionalLocation(new Vector3(4542.602f, -2052.051f, 5.723258f), 180.0852f, 75f),
            },
            VehiclePreviewLocation = new SpawnPlace(new Vector3(4561.993f, -2022.106f, 5.720048f), 34.04589f),
            VehiclePreviewCameraPosition = new Vector3(4555.944f, -2022.406f, 7.466713f),
            VehiclePreviewCameraDirection = new Vector3(0.9867437f, 0.01829198f, -0.1612519f),
            VehiclePreviewCameraRotation = new Rotator(-9.279566f, -2.433079E-07f, -88.93799f),
            VehicleDeliveryLocations = new List<SpawnPlace>() {
                new SpawnPlace(new Vector3(4560.015f, -2031.753f, 5.37559f), 0.06439044f),
                new SpawnPlace(new Vector3(4555.645f, -2031.722f, 5.376274f), 358.5541f),
                }
        };
        LibertyCityLocations.GangDens.Add(AngelsDen);

        GangDen UptownDen = new GangDen(new Vector3(4975.615f, -1620.471f, 18.73886f), 357.6026f, "Uptown Riders Clubhouse", "", "UptownDenMenu", "AMBIENT_GANG_UPTOWN")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            OpenTime = 0,
            CloseTime = 24,
            BannerImagePath = "gangs\\uptownriders.png",
            IsEnabled = true,
            MaxAssaultSpawns = 25,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 150,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(4952.712f, -1620.564f, 18.71144f), 13.36115f, 75f),
                new GangConditionalLocation(new Vector3(4961.893f, -1602.494f, 18.71144f), 180.839f, 75f),
                new GangConditionalLocation(new Vector3(4971.321f, -1617.895f, 18.75394f), 51.9642f, 75f),
                new GangConditionalLocation(new Vector3(4978.939f, -1618.867f, 18.74073f), 342.7836f, 75f),
                new GangConditionalLocation(new Vector3(4984.217f, -1601.986f, 18.71144f), 213.7034f, 75f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(4989.479f, -1615.09f, 18.04426f), 203.8015f, 75f),
                new GangConditionalLocation(new Vector3(4977.424f, -1600.658f, 18.07033f), 182.3812f, 75f),
                new GangConditionalLocation(new Vector3(4967.054f, -1600.769f, 18.23459f), 175.9698f, 75f),
            },
            VehiclePreviewLocation = new SpawnPlace(new Vector3(4975.513f, -1612.918f, 17.99163f), 92.08076f),
            VehiclePreviewCameraPosition = new Vector3(4973.038f, -1611.145f, 19.25755f),
            VehiclePreviewCameraDirection = new Vector3(0.6549445f, -0.7009627f, -0.2823102f),
            VehiclePreviewCameraRotation = new Rotator(-16.39813f, 4.449875E-06f, -136.9438f),
            VehicleDeliveryLocations = new List<SpawnPlace>()
            {
                new SpawnPlace(new Vector3(4962.028f, -1618.067f, 18.05247f), 0.2070662f),
                new SpawnPlace(new Vector3(4957.323f, -1618.098f, 18.05359f), 0.1297245f),
            }
        };
        LibertyCityLocations.GangDens.Add(UptownDen);
    }
    private void DefaultConfig_GasStations()
    {
        List<GasStation> GasStations = new List<GasStation>() {
            new GasStation() {
                MenuID = "GasStationMenu",
                Name = "Globe Oil",
                Description = "Changing the Climate, Powering The Future",
                EntrancePosition = new Vector3(3778.81f, -3222.713f, 7.232538f),
                EntranceHeading = 238.1547f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.AlderneyStateID,
            },
            new GasStation() {
                MenuID = "XeroMenu",
                Name = "Terroil Station",
                Description = "We sell Xero gasoline though",
                BannerImagePath = "stores\\xero.png",
                EntrancePosition = new Vector3(5294.217f, -2136.259f, 14.66519f),
                EntranceHeading = 1.165429f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new GasStation() {
                MenuID = "RonMenu",
                Name = "Ron Station",
                Description = "Put RON in your tank",
                BannerImagePath = "stores\\ron.png",
                EntrancePosition = new Vector3(5954.501f, -3070.447f, 6.156714f),
                EntranceHeading = 239.3434f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new GasStation() {
                MenuID = "RonMenu",
                Name = "Ron Station",
                Description = "Put RON in your tank",
                BannerImagePath = "stores\\ron.png",
                EntrancePosition = new Vector3(6329.701f, -3615.399f, 19.32823f),
                EntranceHeading = 92.82973f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new GasStation() {
                MenuID = "RonMenu",
                Name = "Ron Station",
                Description = "Put RON in your tank",
                BannerImagePath = "stores\\ron.png",
                EntrancePosition = new Vector3(6975.457f, -2423.554f, 16.57794f),
                EntranceHeading = 72.49f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new GasStation() {
                MenuID = "RonMenu",
                Name = "Ron Station",
                Description = "Put RON in your tank",
                BannerImagePath = "stores\\ron.png",
                EntrancePosition = new Vector3(6641.907f, -1528.244f, 16.83064f),
                EntranceHeading = 181.2f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new GasStation() {
                MenuID = "RonMenu",
                Name = "Ron Station",
                Description = "Put RON in your tank",
                BannerImagePath = "stores\\ron.png",
                EntrancePosition = new Vector3(3874.007f, -1511.06f, 27.89451f),
                EntranceHeading = 181.5274f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.AlderneyStateID,
            },
            new GasStation() {
                MenuID = "RonMenu",
                Name = "Ron Station",
                Description = "Put RON in your tank",
                BannerImagePath = "stores\\ron.png",
                EntrancePosition = new Vector3(4755.877f, -3259.214f, 9.970059f),
                EntranceHeading = 178.63f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new GasStation() {
                MenuID = "RonMenu",
                Name = "Ron Station",
                Description = "Put RON in your tank",
                BannerImagePath = "stores\\ron.png",
                EntrancePosition = new Vector3(6324.172f, -2928.793f, 29.78701f),
                EntranceHeading = 81.58755f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.GasStations.AddRange(GasStations);
    }
    private void DefaultConfig_GunStores()
    {
        List<GunStore> GunStores = new List<GunStore>()
        {
            new GunStore(new Vector3(6245.22f, -3169.324f, 34.22475f), 87.59087f,"Choppy Shop","GRAH GRAH BOOM","GunShop1")
            {
                OpenTime = 0,
                CloseTime = 24,
                IsEnabled = true,
                InteriorID = 108290,
                ContactName = StaticStrings.UndergroundGunsContactName,
                //VendorsShowItemsPreview = true,
                VendorFightPercentage = 100,
                VendorFightPolicePercentage = 100,
                VendorCallPolicePercentage = 0,
                VendorMoneyMin = 500,
                VendorMoneyMax = 5000,
                VendorSidearmPercent = 100,
                VendorLongGunPercent = 100,
                VendorLongGunWeaponsID = "AllLongGuns",
                VendorSidearmWeaponsID ="AllSidearms",
                VendorPersonnelID = "GunshopPeds",
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(6254.468f, -3164.325f, 32.24626f), 87.8f) },
                ParkingSpaces = new List<SpawnPlace>() { new SpawnPlace(new Vector3(6241.186f, -3165.323f, 33.04713f), 359.0082f),},
                StateID = StaticStrings.LibertyStateID,
                ActivateCells= 3,
                ActivateDistance = 50f,
            },
            new GunStore(new Vector3(5260.614f, -3598.574f, 11.16248f), 84.67265f,"Blicky Bodega","Protect yourself from the opposition","GunShop2")//new Vector3(311.1f, 151.3f, 11.16f),86.28f
            {
                OpenTime = 0,
                CloseTime = 24,
                IsEnabled = true,
                InteriorID = 92674,
                ContactName = StaticStrings.UndergroundGunsContactName,
                //VendorsShowItemsPreview = true,
                VendorFightPercentage = 100,
                VendorFightPolicePercentage = 100,
                VendorCallPolicePercentage = 0,
                VendorMoneyMin = 500,
                VendorMoneyMax = 5000,
                VendorSidearmPercent = 100,
                VendorLongGunPercent = 100,
                VendorLongGunWeaponsID = "AllLongGuns",
                VendorSidearmWeaponsID ="AllSidearms",
                VendorPersonnelID = "GunshopPeds",
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(5271.382f, -3593.383f, 11.17401f), 106.4831f) },
                ParkingSpaces = new List<SpawnPlace>() { new SpawnPlace(new Vector3(5249.438f, -3598.927f, 14.26976f), 1.05223f),},
                //CameraPosition = new Vector3(317.0506f, 153.9417f, 12.91824f), 
                //CameraDirection = new Vector3(0.378016f, 0.9001398f, -0.2164539f), 
                //CameraRotation = new Rotator(-12.50084f, -1.093132E-05f, -22.78009f),
                StateID = StaticStrings.LibertyStateID,
                ActivateCells= 3,
                ActivateDistance = 50f,
            },
            new GunStore(new Vector3(3855.104f, -2947.847f, 14.62102f), 183.8422f,"Knocks Central","Essential business","GunShop3")
            {
                OpenTime = 0,
                CloseTime = 24,
                IsEnabled = true,
                InteriorID = 101890,
                ContactName = StaticStrings.UndergroundGunsContactName,
                VendorFightPercentage = 100,
                VendorFightPolicePercentage = 100,
                VendorCallPolicePercentage = 0,
                VendorMoneyMin = 500,
                VendorMoneyMax = 5000,
                VendorSidearmPercent = 100,
                VendorLongGunPercent = 100,
                VendorLongGunWeaponsID = "AllLongGuns",
                VendorSidearmWeaponsID ="AllSidearms",
                VendorPersonnelID = "GunshopPeds",
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(3849.976f, -2936.79f, 14.61744f), 187.4391f) },
                ParkingSpaces = new List<SpawnPlace>() { new SpawnPlace(new Vector3(3848.023f, -2949.838f, 13.01697f), 87.7153f),},
                StateID = StaticStrings.AlderneyStateID,
                ActivateCells= 3,
                ActivateDistance = 50f,
            },
                };
        LibertyCityLocations.GunStores.AddRange(GunStores);
    }
    private void DefaultConfig_HardwareStores()
    {
        LibertyCityLocations.HardwareStores.AddRange(new List<HardwareStore>()
        {
            new HardwareStore(new Vector3(4068.96f, -1883.662f, 24.51367f), 3.99266f, "Cheapo's Hardware", "Affordable tools for tools","ToolMenu"){ StateID = StaticStrings.AlderneyStateID },
            new HardwareStore(new Vector3(5957.734f, -3773.127f, 6.453359f), 118.241f, "BJ Hardware", "Blowjobs aren't in the menu","ToolMenu"){ StateID = StaticStrings.LibertyStateID },
            new HardwareStore(new Vector3(5795.628f, -1814.794f, 11.66868f), 3.217404f, "Kack's Hardware", "Vertified by Mr. Kack himself","ToolMenu"){ StateID = StaticStrings.LibertyStateID },


            new HardwareStore(new Vector3(3510.058f, -2977.699f, 20.74425f), 175.5223f, "Hardware Store", "","ToolMenu"){ StateID = StaticStrings.AlderneyStateID },
            new HardwareStore(new Vector3(6083.38f, -3811.805f, 14.17142f), 229.4212f, "645 Hardware", "","ToolMenu"){ StateID = StaticStrings.LibertyStateID },
            new HardwareStore(new Vector3(6199.438f, -3570.427f, 21.37279f), 94.49049f, "645 Hardware", "","ToolMenu"){ StateID = StaticStrings.LibertyStateID },

            new HardwareStore(new Vector3(6417.963f, -3152.417f, 38.6512f), 155.6264f, "D.I.Y", "In Your Arms Tonight","ToolMenu"){ StateID = StaticStrings.LibertyStateID },
            new HardwareStore(new Vector3(6419.569f, -3178.937f, 37.92084f), 358.6304f, "Discount Hardware", "","ToolMenu"){ StateID = StaticStrings.LibertyStateID },

            new HardwareStore(new Vector3(6056.661f, -3427.649f, 5.850422f), 316.6017f, "Builders Supplies", "","ToolMenu"){ StateID = StaticStrings.LibertyStateID },
            new HardwareStore(new Vector3(5907.46f, -1521.614f, 36.96003f), 200.451f, "Hardware & Houseware", "","ToolMenu"){ StateID = StaticStrings.LibertyStateID },
            new HardwareStore(new Vector3(3813.605f, -2784.477f, 14.60903f), 178.2576f, "Hardware Store", "","ToolMenu"){ StateID = StaticStrings.LibertyStateID },
        });
    }
    private void DefaultConfig_HeadShops()
    {
        List<HeadShop> HeadShops = new List<HeadShop>()
        {
            new HeadShop(new Vector3(5690.363f, -1495.672f, 28.19053f), 353.9259f,"Tobacco Shop","","HeadShopMenu") { StateID = StaticStrings.LibertyStateID },
            new HeadShop(new Vector3(6439.603f, -3411.228f, 27.62828f), 2.925252f,"69 Brand Cigar Shop","","HeadShopMenu") { StateID = StaticStrings.LibertyStateID },
            new HeadShop(new Vector3(5555.736f, -1565.727f, 17.05517f), 175.8156f, "Tobacco Shop 24/7", "","HeadShopMenu") { StateID = StaticStrings.LibertyStateID },

        };
        LibertyCityLocations.HeadShops.AddRange(HeadShops);
    }
    private void DefaultConfig_Hospitals()
    {
        List<Hospital> Hospitals = new List<Hospital>()
        {
            //Broker
            new Hospital(new Vector3(6388.865f, -3059.423f, 33.54911f), 86.58257f, "Schottler Medical Center","")
            { 
                OpenTime = 0,
                CloseTime = 24,
                InteriorID = 375553,
                IsWalkup = true,
                AssignedAssociationID = "FDLC-EMS",
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new EMSConditionalLocation(new Vector3(6383.956f, -3057.632f, 32.56467f), 92.21245f, 25f),
                    new EMSConditionalLocation(new Vector3(6395.862f, -3068.285f, 33.54847f), 238.0733f, 25f),
                    new EMSConditionalLocation(new Vector3(6412.011f, -3049.816f, 33.54834f), 166.3082f, 25f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new EMSConditionalLocation(new Vector3(6377.37f, -3070.168f, 32.23043f), 8.695308f, 25f),
                    new EMSConditionalLocation(new Vector3(6377.356f, -3061.929f, 32.22757f), 357.7849f, 25f),
                    new EMSConditionalLocation(new Vector3(6347.031f, -3071.549f, 32.11538f), 179.8152f, 25f),
                },
                StateID = StaticStrings.LibertyStateID,
                ActivateDistance = 200,
            },

            //Dukes
            new Hospital(new Vector3(6436.007f, -2772.835f, 29.53508f), 30.69128f, "Cerveza Heights Medical Center","")
            { 
                OpenTime = 0,
                CloseTime = 24,
                AssignedAssociationID = "FDLC-EMS",
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new EMSConditionalLocation(new Vector3(6431.942f, -2773.196f, 29.53509f), 31.74258f, 25f),
                    new EMSConditionalLocation(new Vector3(6450.973f, -2767.846f, 29.53509f), 355.86f, 25f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new EMSConditionalLocation(new Vector3(6421.754f, -2777.353f, 27.41948f), 359.0115f, 25f),
                },
                StateID = StaticStrings.LibertyStateID,
                ActivateDistance = 200,
            },
            //Bohan
            new Hospital(new Vector3(6170.463f, -1416.738f, 23.89281f), 83.303f, "Bohan Medical & Dental Center","")
            { 
                OpenTime = 0,
                CloseTime = 24,
                AssignedAssociationID = "FDLC-EMS",
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new EMSConditionalLocation(new Vector3(6165.432f, -1429.543f, 20.1851f), 94.76711f, 25f),
                    new EMSConditionalLocation(new Vector3(6167.273f, -1419.846f, 23.8928f), 95.25718f, 25f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new EMSConditionalLocation(new Vector3(6160.431f, -1430.471f, 19.8805f), 359.6386f, 25f),
                    new EMSConditionalLocation(new Vector3(6160.653f, -1443.627f, 19.81282f), 1.360374f, 25f),
                },
                StateID = StaticStrings.LibertyStateID,
                ActivateDistance = 200,
            },
            //Algon
            new Hospital(new Vector3(4763.522f, -1952.517f, 17.43271f), 85.08374f, "Holland Hospital Center","")
            {
                OpenTime = 0,
                CloseTime = 24,
                AssignedAssociationID = "FDLC-EMS",
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new EMSConditionalLocation(new Vector3(4764.122f, -1948.083f, 17.43329f), 95.98059f, 25f),
                    new EMSConditionalLocation(new Vector3(4768.461f, -1937.056f, 17.31978f), 319.657f, 25f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new EMSConditionalLocation(new Vector3(4758.878f, -1948.046f, 17.18217f), 0.5565929f, 25f),
                    new EMSConditionalLocation(new Vector3(4750.354f, -1945.418f, 17.169f), 181.2125f, 25f),
                },
                StateID = StaticStrings.LibertyStateID,
                ActivateDistance = 200,
            },
            new Hospital(new Vector3(5283.987f, -3106.773f, 14.77349f), 176.6039f, "Lancet-Hospital Center","")
            {
                OpenTime = 0,
                CloseTime = 24,
                AssignedAssociationID = "FDLC-EMS",
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new EMSConditionalLocation(new Vector3(5289.078f, -3107.346f, 14.7786f), 178.6843f, 15f),
                    new EMSConditionalLocation(new Vector3(5275.543f, -3107.18f, 14.76301f), 176.3426f, 15f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new EMSConditionalLocation(new Vector3(5243.347f, -3082.671f, 14.51815f), 89.92236f, 25f),
                    new EMSConditionalLocation(new Vector3(5243.688f, -3078.541f, 14.51897f), 91.67643f, 25f),
                },
                StateID = StaticStrings.LibertyStateID,
                ActivateDistance = 200,
            },
            new Hospital(new Vector3(5003.084f, -3814.478f, 4.937624f), 87.88223f, "City Hall Hospital","")
            { 
                OpenTime = 0,
                CloseTime = 24,
                AssignedAssociationID = "FDLC-EMS",
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new EMSConditionalLocation(new Vector3(5000.801f, -3820.096f, 4.937622f), 90.86433f, 15f),
                    new EMSConditionalLocation(new Vector3(5000.966f, -3807.338f, 4.937626f), 115.3829f, 15f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new EMSConditionalLocation(new Vector3(4995.163f, -3817.281f, 4.55712f), 1.71807f, 25f),
                    new EMSConditionalLocation(new Vector3(4978.876f, -3808.461f, 4.556808f), 182.1808f, 25f),
                    new EMSConditionalLocation(new Vector3(4983.008f, -3806.187f, 4.557001f), 179.764f, 25f),
                },
                StateID = StaticStrings.LibertyStateID,
                ActivateDistance = 200,
            },
            //Alderny
            new Hospital(new Vector3(3867.802f, -1981.043f, 23.37117f), 309.2765f, "Westdyke Memorial Hospital","") 
            { 
                OpenTime = 0,
                CloseTime = 24, 
                InteriorID = 387585,
                IsWalkup = true,
                AssignedAssociationID = "FDLC-EMS",
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new EMSConditionalLocation(new Vector3(3872.955f, -1979.246f, 22.36574f), 326.4178f, 15f),
                    new EMSConditionalLocation(new Vector3(3847.145f, -1983.012f, 23.37015f), 287.17f, 15f),
                    new EMSConditionalLocation(new Vector3(3857.314f, -1977.765f, 23.37022f), 309.5089f, 15f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new EMSConditionalLocation(new Vector3(3879.984f, -1992.371f, 22.13453f), 130.4863f, 25f),
                    new EMSConditionalLocation(new Vector3(3876.864f, -1988.306f, 22.1355f), 129.7404f, 25f),
                },
                StateID = StaticStrings.AlderneyStateID,
                ActivateDistance = 200,
            },
            new Hospital(new Vector3(3672.727f, -2859.523f, 21.39725f), 315.3995f, "North Tudor Medical Center","")
            {
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.AlderneyStateID,
                ActivateDistance = 200,
                AssignedAssociationID = "FDLC-EMS",
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new EMSConditionalLocation(new Vector3(3670.68481f, -2854.63721f, 19.80547f), 19.8052731f, 25f),
                new EMSConditionalLocation(new Vector3(3679.87061f, -2858.359f, 19.5648632f), 303.5465f, 25f),
                new EMSConditionalLocation(new Vector3(3652.68579f, -2882.34424f, 21.9283733f), 62.32174f, 25f),
                new EMSConditionalLocation(new Vector3(3652.78564f, -2887.80469f, 21.9679241f), 137.8788f, 25f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new EMSConditionalLocation(new Vector3(3623.11475f, -2888.51514f, 23.3564529f), 87.69072f, 25f),
                new EMSConditionalLocation(new Vector3(3623.4978f, -2895.0f, 23.35664f), 23.3564434f, 25f),
                new EMSConditionalLocation(new Vector3(3642.86084f, -2898.0918f, 23.3564434f), 269.4883f, 25f),
                new EMSConditionalLocation(new Vector3(3642.07568f, -2870.35938f, 23.3564529f), 269.1915f, 25f),
            },

            },

            //North Yankton
            new Hospital(new Vector3(3132.073f, -4839.958f, 112.0312f), 354.8388f, "Ludendorff Clinic", "The service you'd expect!") { StateID = StaticStrings.NorthYanktonStateID, OpenTime = 0,CloseTime = 24,
                PossiblePedSpawns = new List<ConditionalLocation>()
            { },
            RespawnLocation = new Vector3(3132.073f, -4839.958f, 112.0312f),RespawnHeading = 354.8388f },

            //Clinic
            new Hospital(new Vector3(6166.863f, -3705.837f, 16.18029f), 269.9752f, "Russian Clinic","") { OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID },
        };
        LibertyCityLocations.Hospitals.AddRange(Hospitals);
    }
    private void DefaultConfig_Hotels()
    {
        List<Hotel> HotelsList = new List<Hotel>()
        {
            new Hotel() {
                MenuID = "ExpensiveHotelMenu",
                Name = "The Majestic",
                Description = "The best in town, unrivaled",
                EntrancePosition = new Vector3(5021.847f, -2647.282f, 15.01635f),
                EntranceHeading = 355.64f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Hotel() {
                MenuID = "ExpensiveHotelMenu",
                Name = "The Star Plaza Hotel",
                Description = "Sleep like a star",
                EntrancePosition = new Vector3(4902.628f, -2762.603f, 14.78495f),
                EntranceHeading = 272.4973f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Hotel() {
                MenuID = "ExpensiveHotelMenu",
                Name = "Banner Hotel",
                Description = "Luxury frachised",
                EntrancePosition = new Vector3(5014.2f, -2858.714f, 14.81578f),
                EntranceHeading = 180.6733f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Hotel() {
                MenuID = "ExpensiveHotelMenu",
                Name = "The Nicoise Hotel",
                Description = "5 star hotel for 1 star people",
                EntrancePosition = new Vector3(5213.641f, -2915.875f, 14.69753f),
                EntranceHeading = 271.4881f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Hotel() {
                MenuID = "ExpensiveHotelMenu",
                Name = "Grand Northumbrian",
                Description = "The spot to crash out after being robbed in Mirror Park",
                EntrancePosition = new Vector3(5158.348f, -2297.17f, 14.92191f),
                EntranceHeading = 90.90194f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Hotel() {
                MenuID = "ExpensiveHotelMenu",
                Name = "Castle Gardens Hotel",
                Description = "Resort experience in the heart of Liberty",
                EntrancePosition = new Vector3(5077.548f, -4156.431f, 5.522742f),
                EntranceHeading = 3.538459f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Hotel() {
                MenuID = "ExpensiveHotelMenu",
                Name = "Hotel Hamilton",
                Description = "Top in the trash",
                EntrancePosition = new Vector3(4874.407f, -1849.136f, 15.44401f),
                EntranceHeading = 0.3452598f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Hotel() {
                MenuID = "CheapHotelMenu",
                Name = "Beach Hotel",
                Description = "Enjoy the smell of the polluted ocean",
                EntrancePosition = new Vector3(6158.567f, -3836.603f, 14.3063f),
                EntranceHeading = 3.777269f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Hotel() {
                MenuID = "CheapHotelMenu",
                Name = "Berchem Hotel",
                Description = "Stay in the heart of Alderney",
                EntrancePosition = new Vector3(3723.973f, -2589.292f, 20.11098f),
                EntranceHeading = 239.4318f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.AlderneyStateID,
            },
            new Hotel() {
                MenuID = "ExpensiveHotelMenu",
                Name = "Opium Nights Hotel",
                Description = "Where Dreams Unwind and Luxury Awaits",
                EntrancePosition = new Vector3(5156.642f, -2509.206f, 14.95662f),
                EntranceHeading = 89.19273f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.Hotels.AddRange(HotelsList);
    }
    private void DefaultConfig_IllicitMarketplaces()
    {
        List<IllicitMarketplace> IllicitMarketplaces = new List<IllicitMarketplace>() {
            new IllicitMarketplace() {

                MenuID = "DealerHangoutMenu3",
                Name = "Charge Island Dealer",
                Description = "Charge Island Dealer Hangout",
                EntrancePosition = new Vector3(5870.598f, -2482.968f, 2.784798f),
                EntranceHeading = 83.8096f,
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(5870.598f, -2482.968f, 2.784798f), 83.8096f) },
                AppearPercentages = new List<AppearPercentage>()
                {
                    new AppearPercentage(0,85),
                    new AppearPercentage(1,75),
                    new AppearPercentage(2,75),
                    new AppearPercentage(3,75),
                    new AppearPercentage(4,50),
                    new AppearPercentage(5,50),
                    new AppearPercentage(6,25),
                    new AppearPercentage(7,25),
                    new AppearPercentage(8,25),
                    new AppearPercentage(9,25),
                    new AppearPercentage(10,25),
                    new AppearPercentage(11,25),
                    new AppearPercentage(12,25),
                    new AppearPercentage(13,25),
                    new AppearPercentage(14,25),
                    new AppearPercentage(15,25),
                    new AppearPercentage(16,50),
                    new AppearPercentage(17,50),
                    new AppearPercentage(18,50),
                    new AppearPercentage(19,50),
                    new AppearPercentage(20,75),
                    new AppearPercentage(21,75),
                    new AppearPercentage(22,75),
                    new AppearPercentage(23,85),
                },
                StateID = StaticStrings.LibertyStateID,
                ActivateCells= 3,
                ActivateDistance = 75f,
            },
            new IllicitMarketplace() {

                MenuID = "DealerHangoutMenu2",
                Name = "Fish Market Dealer",
                Description = "Fish Market Dealer Hangout",
                EntrancePosition = new Vector3(5448.752f, -3673.448f, 5.006905f),
                EntranceHeading = 181.692f,
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(5448.752f, -3673.448f, 5.006905f), 181.692f) },
                AppearPercentages = new List<AppearPercentage>()
                {
                    new AppearPercentage(0,85),
                    new AppearPercentage(1,75),
                    new AppearPercentage(2,75),
                    new AppearPercentage(3,75),
                    new AppearPercentage(4,50),
                    new AppearPercentage(5,50),
                    new AppearPercentage(6,25),
                    new AppearPercentage(7,25),
                    new AppearPercentage(8,25),
                    new AppearPercentage(9,25),
                    new AppearPercentage(10,25),
                    new AppearPercentage(11,25),
                    new AppearPercentage(12,25),
                    new AppearPercentage(13,25),
                    new AppearPercentage(14,25),
                    new AppearPercentage(15,25),
                    new AppearPercentage(16,50),
                    new AppearPercentage(17,50),
                    new AppearPercentage(18,50),
                    new AppearPercentage(19,50),
                    new AppearPercentage(20,75),
                    new AppearPercentage(21,75),
                    new AppearPercentage(22,75),
                    new AppearPercentage(23,85),
                },
                StateID = StaticStrings.LibertyStateID,
                ActivateCells= 3,
                ActivateDistance = 75f,
            },
            new IllicitMarketplace() {

                MenuID = "DealerHangoutMenu2",
                Name = "Middle Park W Dealer",
                Description = "Middle Park West Dealer Hangout",
                EntrancePosition = new Vector3(4572.958f, -2569.309f, 4.746498f),
                EntranceHeading = 181.9404f,
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4572.958f, -2569.309f, 4.746498f), 181.9404f) },
                AppearPercentages = new List<AppearPercentage>()
                {
                    new AppearPercentage(0,85),
                    new AppearPercentage(1,75),
                    new AppearPercentage(2,75),
                    new AppearPercentage(3,75),
                    new AppearPercentage(4,50),
                    new AppearPercentage(5,50),
                    new AppearPercentage(6,25),
                    new AppearPercentage(7,25),
                    new AppearPercentage(8,25),
                    new AppearPercentage(9,25),
                    new AppearPercentage(10,25),
                    new AppearPercentage(11,25),
                    new AppearPercentage(12,25),
                    new AppearPercentage(13,25),
                    new AppearPercentage(14,25),
                    new AppearPercentage(15,25),
                    new AppearPercentage(16,50),
                    new AppearPercentage(17,50),
                    new AppearPercentage(18,50),
                    new AppearPercentage(19,50),
                    new AppearPercentage(20,75),
                    new AppearPercentage(21,75),
                    new AppearPercentage(22,75),
                    new AppearPercentage(23,85),
                },
                StateID = StaticStrings.LibertyStateID,
                ActivateCells= 3,
                ActivateDistance = 75f,
            },
            new IllicitMarketplace() {

                MenuID = "DealerHangoutMenu2",
                Name = " Industrial Dealer",
                Description = "Industrial Dealer Hangout",
                EntrancePosition = new Vector3(6398.862f, -1787.617f, 16.72004f),
                EntranceHeading = 215.8636f,
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(6398.862f, -1787.617f, 16.72004f), 215.8636f) },
                AppearPercentages = new List<AppearPercentage>()
                {
                    new AppearPercentage(0,85),
                    new AppearPercentage(1,75),
                    new AppearPercentage(2,75),
                    new AppearPercentage(3,75),
                    new AppearPercentage(4,50),
                    new AppearPercentage(5,50),
                    new AppearPercentage(6,25),
                    new AppearPercentage(7,25),
                    new AppearPercentage(8,25),
                    new AppearPercentage(9,25),
                    new AppearPercentage(10,25),
                    new AppearPercentage(11,25),
                    new AppearPercentage(12,25),
                    new AppearPercentage(13,25),
                    new AppearPercentage(14,25),
                    new AppearPercentage(15,25),
                    new AppearPercentage(16,50),
                    new AppearPercentage(17,50),
                    new AppearPercentage(18,50),
                    new AppearPercentage(19,50),
                    new AppearPercentage(20,75),
                    new AppearPercentage(21,75),
                    new AppearPercentage(22,75),
                    new AppearPercentage(23,85),
                },
                StateID = StaticStrings.LibertyStateID,
                ActivateCells= 3,
                ActivateDistance = 75f,
            },
            new IllicitMarketplace() {

                MenuID = "DealerHangoutMenu1",
                Name = "Boabo Dealer",
                Description = "Boabo Dealer Hangout",
                EntrancePosition = new Vector3(5884.643f, -3143.506f, 6.00323f),
                EntranceHeading = 98.96494f,
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(5884.643f, -3143.506f, 6.00323f), 98.96494f) },
                AppearPercentages = new List<AppearPercentage>()
                {
                    new AppearPercentage(0,85),
                    new AppearPercentage(1,75),
                    new AppearPercentage(2,75),
                    new AppearPercentage(3,75),
                    new AppearPercentage(4,50),
                    new AppearPercentage(5,50),
                    new AppearPercentage(6,25),
                    new AppearPercentage(7,25),
                    new AppearPercentage(8,25),
                    new AppearPercentage(9,25),
                    new AppearPercentage(10,25),
                    new AppearPercentage(11,25),
                    new AppearPercentage(12,25),
                    new AppearPercentage(13,25),
                    new AppearPercentage(14,25),
                    new AppearPercentage(15,25),
                    new AppearPercentage(16,50),
                    new AppearPercentage(17,50),
                    new AppearPercentage(18,50),
                    new AppearPercentage(19,50),
                    new AppearPercentage(20,75),
                    new AppearPercentage(21,75),
                    new AppearPercentage(22,75),
                    new AppearPercentage(23,85),
                },
                StateID = StaticStrings.LibertyStateID,
                ActivateCells= 3,
                ActivateDistance = 75f,
            },
            new IllicitMarketplace() {

                MenuID = "DealerHangoutMenu2",
                Name = "Willis Dealer",
                Description = "Willis Dealer Hangout 3",
                EntrancePosition = new Vector3(6952.94f, -2600.8f, 28.26978f),
                EntranceHeading = 329.3248f,
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(6952.94f, -2600.8f, 28.26978f), 329.3248f) },
                AppearPercentages = new List<AppearPercentage>()
                {
                    new AppearPercentage(0,85),
                    new AppearPercentage(1,75),
                    new AppearPercentage(2,75),
                    new AppearPercentage(3,75),
                    new AppearPercentage(4,50),
                    new AppearPercentage(5,50),
                    new AppearPercentage(6,25),
                    new AppearPercentage(7,25),
                    new AppearPercentage(8,25),
                    new AppearPercentage(9,25),
                    new AppearPercentage(10,25),
                    new AppearPercentage(11,25),
                    new AppearPercentage(12,25),
                    new AppearPercentage(13,25),
                    new AppearPercentage(14,25),
                    new AppearPercentage(15,25),
                    new AppearPercentage(16,50),
                    new AppearPercentage(17,50),
                    new AppearPercentage(18,50),
                    new AppearPercentage(19,50),
                    new AppearPercentage(20,75),
                    new AppearPercentage(21,75),
                    new AppearPercentage(22,75),
                    new AppearPercentage(23,85),
                },
                StateID = StaticStrings.LibertyStateID,
                ActivateCells= 3,
                ActivateDistance = 75f,
            },
            new IllicitMarketplace() {

                MenuID = "DealerHangoutMenu1",
                Name = "FireFly Projects Dealer",
                Description = "FireFly Projects Dealer Hangout",
                EntrancePosition = new Vector3(6449.146f, -3595.29f, 21.54639f),
                EntranceHeading = 267.4221f,
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(6449.146f, -3595.29f, 21.54639f), 267.4221f) },
                AppearPercentages = new List<AppearPercentage>()
                {
                    new AppearPercentage(0,85),
                    new AppearPercentage(1,75),
                    new AppearPercentage(2,75),
                    new AppearPercentage(3,75),
                    new AppearPercentage(4,50),
                    new AppearPercentage(5,50),
                    new AppearPercentage(6,25),
                    new AppearPercentage(7,25),
                    new AppearPercentage(8,25),
                    new AppearPercentage(9,25),
                    new AppearPercentage(10,25),
                    new AppearPercentage(11,25),
                    new AppearPercentage(12,25),
                    new AppearPercentage(13,25),
                    new AppearPercentage(14,25),
                    new AppearPercentage(15,25),
                    new AppearPercentage(16,50),
                    new AppearPercentage(17,50),
                    new AppearPercentage(18,50),
                    new AppearPercentage(19,50),
                    new AppearPercentage(20,75),
                    new AppearPercentage(21,75),
                    new AppearPercentage(22,75),
                    new AppearPercentage(23,85),
                },
                StateID = StaticStrings.LibertyStateID,
                ActivateCells= 3,
                ActivateDistance = 75f,
            },
            new IllicitMarketplace() {

                MenuID = "DealerHangoutMenu2",
                Name = " Westdyke Dealer",
                Description = "Westdyke Dealer Hangout",
                EntrancePosition = new Vector3(4173.589f, -2036.236f, 25.11742f),
                EntranceHeading = 178.2498f,
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(4173.589f, -2036.236f, 25.11742f), 178.2498f) },
                AppearPercentages = new List<AppearPercentage>()
                {
                    new AppearPercentage(0,85),
                    new AppearPercentage(1,75),
                    new AppearPercentage(2,75),
                    new AppearPercentage(3,75),
                    new AppearPercentage(4,50),
                    new AppearPercentage(5,50),
                    new AppearPercentage(6,25),
                    new AppearPercentage(7,25),
                    new AppearPercentage(8,25),
                    new AppearPercentage(9,25),
                    new AppearPercentage(10,25),
                    new AppearPercentage(11,25),
                    new AppearPercentage(12,25),
                    new AppearPercentage(13,25),
                    new AppearPercentage(14,25),
                    new AppearPercentage(15,25),
                    new AppearPercentage(16,50),
                    new AppearPercentage(17,50),
                    new AppearPercentage(18,50),
                    new AppearPercentage(19,50),
                    new AppearPercentage(20,75),
                    new AppearPercentage(21,75),
                    new AppearPercentage(22,75),
                    new AppearPercentage(23,85),
                },
                StateID = StaticStrings.AlderneyStateID,
                ActivateCells= 3,
                ActivateDistance = 75f,
            },
            new IllicitMarketplace() {

                MenuID = "DealerHangoutMenu4",
                Name = "Tudor Dealer",
                Description = "Tudor Dealer Hangout",
                EntrancePosition = new Vector3(3664.368f, -3098.644f, 23.11918f),
                EntranceHeading = 270.1395f,
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(3664.368f, -3098.644f, 23.11918f), 270.1395f) },
                AppearPercentages = new List<AppearPercentage>()
                {
                    new AppearPercentage(0,85),
                    new AppearPercentage(1,75),
                    new AppearPercentage(2,75),
                    new AppearPercentage(3,75),
                    new AppearPercentage(4,50),
                    new AppearPercentage(5,50),
                    new AppearPercentage(6,25),
                    new AppearPercentage(7,25),
                    new AppearPercentage(8,25),
                    new AppearPercentage(9,25),
                    new AppearPercentage(10,25),
                    new AppearPercentage(11,25),
                    new AppearPercentage(12,25),
                    new AppearPercentage(13,25),
                    new AppearPercentage(14,25),
                    new AppearPercentage(15,25),
                    new AppearPercentage(16,50),
                    new AppearPercentage(17,50),
                    new AppearPercentage(18,50),
                    new AppearPercentage(19,50),
                    new AppearPercentage(20,75),
                    new AppearPercentage(21,75),
                    new AppearPercentage(22,75),
                    new AppearPercentage(23,85),
                },
                StateID = StaticStrings.AlderneyStateID,
                ActivateCells= 3,
                ActivateDistance = 75f,
            },
            new IllicitMarketplace() {

                MenuID = "DealerHangoutMenu4",
                Name = "Acter Ind Dealer",
                Description = "Acter Industrial Dealer Hangout",
                EntrancePosition = new Vector3(3706.577f, -3528.275f, 2.817671f),
                EntranceHeading = 177.8051f,
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(3706.577f, -3528.275f, 2.817671f), 177.8051f) },
                AppearPercentages = new List<AppearPercentage>()
                {
                    new AppearPercentage(0,85),
                    new AppearPercentage(1,75),
                    new AppearPercentage(2,75),
                    new AppearPercentage(3,75),
                    new AppearPercentage(4,50),
                    new AppearPercentage(5,50),
                    new AppearPercentage(6,25),
                    new AppearPercentage(7,25),
                    new AppearPercentage(8,25),
                    new AppearPercentage(9,25),
                    new AppearPercentage(10,25),
                    new AppearPercentage(11,25),
                    new AppearPercentage(12,25),
                    new AppearPercentage(13,25),
                    new AppearPercentage(14,25),
                    new AppearPercentage(15,25),
                    new AppearPercentage(16,50),
                    new AppearPercentage(17,50),
                    new AppearPercentage(18,50),
                    new AppearPercentage(19,50),
                    new AppearPercentage(20,75),
                    new AppearPercentage(21,75),
                    new AppearPercentage(22,75),
                    new AppearPercentage(23,85),
                },
                StateID = StaticStrings.AlderneyStateID,
                ActivateCells= 3,
                ActivateDistance = 75f,
            },
        };
        LibertyCityLocations.IllicitMarketplaces.AddRange(IllicitMarketplaces);
    }
    private void DefaultConfig_Landmarks()
    {
        LibertyCityLocations.Landmarks.AddRange(new List<Landmark>()
        {
            new Landmark(new Vector3(3675.627f, -1853.742f, 13.38626f), 357.7893f, "Schlongberg Sachs", "") { StateID = StaticStrings.AlderneyStateID },
            new Landmark(new Vector3(3520.911f, -2979.891f, 20.1767f), 176.9206f, "Satriale's Pork Store", "") { StateID = StaticStrings.AlderneyStateID },
            new Landmark(new Vector3(6145.764f, -3546.416f, 19.80574f), 182.1452f, "Perestroika", "") { BannerImagePath = "stores//perestroika.png", StateID = StaticStrings.LibertyStateID, InteriorID = 78338,IgnoreEntranceInteract = true, },// previously 76290
            //new Landmark(new Vector3(1251.176f, 168.7538f, 20.23567f), 99.71825f, "Laundromat", "") { StateID = StaticStrings.LibertyStateID, InteriorID = 138498, IgnoreEntranceInteract = true, },
            new Landmark(new Vector3(6011.51f, -3530.924f, 15.34174f), 269.6217f, "Express Car Services", "") { BannerImagePath = "stores//expresscarservice.png",StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(4844.729f, -3356.927f, 14.70356f), 90.96991f, "Rotterdam Tower", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(5210.873f, -3295.222f, 14.81279f), 182.7993f, "Grand Easton Terminal", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(4706.229f, -2575.267f, 9.524785f), 86.67437f, "Randalf Art Center", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(5156.672f, -2679.188f, 14.66228f), 89.06384f, "Cleethorpes Tower", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(5185.535f, -2848.617f, 16.36455f), 182.2745f, "Columbus Cathedral", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(5097.768f, -2434.925f, 18.62561f), 265.9916f, "The Libertonian", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(5357.371f, -2888.666f, 15.17426f), 90.2771f, "Civilization Committee", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(4794.582f, -2370.279f, 18.18758f), 268.519f, "Natural History Museum", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(4665.708f, -2210.665f, 15.50251f), 181.2027f, "Vespucci University", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(4608.009f, -2422.29f, 12.82129f), 193.9257f, "Mausoleum", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(6635.063f, -3115.353f, 26.77674f), 1.31848f, "Beechwood Strip CLub", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(6696.922f, -3123.339f, 23.4205f), 4.141726f, "Car Wash N Lube", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(6539.778f, -3501.955f, 23.22417f), 249.1297f, "Swingset-O-Death", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(6267.726f, -3935.29f, 16.44037f), 50.83215f, "Funland", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(6386.995f, -3935.58f, 16.44063f), 178.8712f, "Memory Lanes", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(6290.013f, -3327.215f, 36.29831f), 178.2877f, "Soldier's Plaza", "To the defenders of the Union, 1861-1865") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(6327.187f, -3292.653f, 37.98269f), 144.9886f, "Broker Public Library", "Get that musty old book smell") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(6413.447f, -3322.806f, 28.84715f), 3.837793f, "Hanks Rent-A-Car", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(6418.697f, -3337.958f, 28.14982f), 270.7415f, "Seedy Rent-A-Van", "") { StateID = StaticStrings.LibertyStateID },


            new Landmark(new Vector3(6134.163f, -2368.377f, 23.10462f), 142.2128f, "Steinway Park", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(6409.239f, -2245.774f, 13.66386f), 91.01848f, "Gantry", "") { StateID = StaticStrings.LibertyStateID },
            //new Landmark(new Vector3(1599.037f, 1494.526f, 14.02291f), 229.9848f, "Dukes Bride Police 1", "") { StateID = StaticStrings.LibertyStateID },
            //new Landmark(new Vector3(1602.456f, 1499.64f, 14.02292f), 46.80567f, "Dukes Bridge Police 2", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(5627.548f, -1446.114f, 21.99759f), 130.022f, "Welham Parkway Park", "") { StateID = StaticStrings.LibertyStateID },

            new Landmark(new Vector3(6438.934f, -3838.86f, 13.77634f), 355.7192f, "Beachgate Gate", "")
            {
                StateID = StaticStrings.LibertyStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new SecurityConditionalLocation(new Vector3(6439.015f, -3842.461f, 13.8653f), 359.2562f,95f) { AssociationID = "GRP6" },
                },
            },
            new Landmark(new Vector3(6251.476f, -3430.177f, 29.91086f), 257.7469f, "Outlook Park", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(6602.102f, -2879.724f, 19.10373f), 354.3592f, "Pillows Strip Club", "") { IsTemporarilyClosed = true, StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(6471.089f, -2652.593f, 38.9913f), 95.33508f, "24 LC", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(6576.882f, -2578.042f, 35.28962f), 267.2f, "Meadows Park Church", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(6677.194f, -2576.518f, 29.37299f), 177.6977f, "Meadows Park Museum", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(7034.282f, -2901.815f, 22.55224f), 88.27064f, "Willis Wash N Lube", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(6968.609f, -2813.507f, 27.96141f), 3.603584f, "After Dark Sex Shop", "") { OpenTime = 0,CloseTime =24, StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(7008.896f, -2689.25f, 29.08155f), 88.87626f, "Canyon Megaplex", "") { StateID = StaticStrings.LibertyStateID },
            //new Landmark(new Vector3(1522.52f, 892.3311f, 22.40118f), 179.9744f, "JJ China Limited", "") { InteriorID = 24578, StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
        });
    }
    private void DefaultConfig_LiquorStore()
    {
        LibertyCityLocations.LiquorStores.AddRange(new List<LiquorStore>()
        {
            new LiquorStore(new Vector3(6554.567f, -3163.334f, 31.24951f), 64.87999f, "Liquors", "","LiquorStoreMenu") { StateID = StaticStrings.LibertyStateID, },
            new LiquorStore(new Vector3(5749.263f, -1570.063f, 29.72702f), 273.3701f, "Liquors", "","LiquorStoreMenu") { StateID = StaticStrings.LibertyStateID, },
            new LiquorStore(new Vector3(6009.64f, -3635.206f, 14.09876f), 271.99f, "Wine & Liquor", "","LiquorStoreMenu") { StateID = StaticStrings.LibertyStateID, },
            new LiquorStore(new Vector3(6116.265f, -3710.364f, 15.7355f), 88.17411f, "Wine & Liquor", "","LiquorStoreMenu") { StateID = StaticStrings.LibertyStateID, },
            new LiquorStore(new Vector3(5300.218f, -2184.728f, 14.71821f), 271.9344f, "Desratar Wine Store", "","LiquorStoreMenu") { StateID = StaticStrings.LibertyStateID, },
            new LiquorStore(new Vector3(4887.603f, -1981.29f, 23.97714f), 176.9975f, "Lady Liquor", "","LiquorStoreMenu") { StateID = StaticStrings.LibertyStateID, },
            new LiquorStore(new Vector3(3634.365f, -2588.989f, 25.44484f), 333.9839f, "Wine & Liquor", "","LiquorStoreMenu") { StateID = StaticStrings.AlderneyStateID, },

            new LiquorStore(new Vector3(6479.24f, -3269.563f, 28.35642f), 243.7716f, "Bob's Liquors", "","LiquorStoreMenu") { StateID = StaticStrings.LibertyStateID, },
            new LiquorStore(new Vector3(6598.446f, -3032.501f, 26.15483f), 269.9614f, "Liquor", "","LiquorStoreMenu") { StateID = StaticStrings.LibertyStateID, },
            new LiquorStore(new Vector3(6382.511f, -3440.119f, 26.08682f), 272.2683f, "Nik's Liquor & Wine", "","LiquorStoreMenu") { StateID = StaticStrings.LibertyStateID, },
            new LiquorStore(new Vector3(6544.805f, -3387.518f, 22.53954f), 175.8956f, "Jeff's Liquor & Wine", "Wash Those Cares Away!","LiquorStoreMenu") { StateID = StaticStrings.LibertyStateID, },

            new LiquorStore(new Vector3(6328.519f, -2834.988f, 29.80314f), 70.64351f, "Oriental Liquors", "","LiquorStoreMenu") { StateID = StaticStrings.LibertyStateID, },
            new LiquorStore(new Vector3(6450f, -2857.426f, 22.20033f), 178.4071f, "Oriental Liquors", "","LiquorStoreMenu") { StateID = StaticStrings.LibertyStateID, },
            new LiquorStore(new Vector3(5946.3f, -1703.553f, 22.02752f), 314.5961f, "N.O.N. Wines & Liquors", "","LiquorStoreMenu") { StateID = StaticStrings.LibertyStateID, },
        });
    }
    private void DefaultConfig_ModShops()
    {
        VehicleModShops = new List<VehicleModShop>()
    {
        new VehicleModShop(new Vector3(4880.421f, -1717.268f, 19.92557f), 1.035645f, "Auto Cowboys", "Servicing Holland since 1979")
        {
            OpenTime = 0,
            CloseTime = 24,
            GarageDoors = new List<InteriorDoor>() { new InteriorDoor(2948754995,new Vector3(4880.308f, -1711.296f, 21.25111f)) },
            CanInteractWhenWanted = true,
            HasNoGarageDoors = false,
            StateID = StaticStrings.LibertyStateID,
            VehiclePreviewLocation = new SpawnPlace(new Vector3(4880.421f, -1717.268f, 19.92557f), 1.035645f),
        },
        new VehicleModShop(new Vector3(4674.521f, -2889.668f, 6.105102f), 1.953825f, "Auto Limbo", "Where all the Union Drive accidents go")
        {
            OpenTime = 0,
            CloseTime = 24,
            GarageDoors = new List<InteriorDoor>() { new InteriorDoor(3242725694,new Vector3(4674.543f, -2883.918f, 151477f)) },
            CanInteractWhenWanted = true,
            HasNoGarageDoors = false,
            StateID = StaticStrings.LibertyStateID,
            VehiclePreviewLocation = new SpawnPlace(new Vector3(4674.521f, -2889.668f, 6.105102f), 1.953825f),
        },
        new VehicleModShop(new Vector3(4040.126f, -2067.116f, 17.12306f), 181.7977f, "Axel's Pay'n'Spray", "Franchising car repairs")
        {
            OpenTime = 0,
            CloseTime = 24,
            GarageDoors = new List<InteriorDoor>() { new InteriorDoor(2790644556,new Vector3(4040.036f, -2075.698f, 18.15517f)) },
            CanInteractWhenWanted = true,
            HasNoGarageDoors = false,
            StateID = StaticStrings.AlderneyStateID,
            VehiclePreviewLocation = new SpawnPlace(new Vector3(4040.126f, -2067.116f, 17.12306f), 181.7977f),
        },
    };
        LibertyCityLocations.VehicleModShops.AddRange(VehicleModShops);
    }
    private void DefaultConfig_PawnShops()
    {
        LibertyCityLocations.PawnShops.AddRange(new List<PawnShop>()
        {
            new PawnShop(new Vector3(5781.732f, -1616.116f, 30.22302f), 359.4117f, "Pawnbrokers", "Buying and selling since '81","PawnShopMenuGeneric") {StateID = StaticStrings.LibertyStateID, MinPriceRefreshHours = 12, MaxPriceRefreshHours = 24,MinRestockHours = 12, MaxRestockHours = 24,MapIcon =642,},
            new PawnShop(new Vector3(6598.953f, -3188.694f, 25.84211f), 269.9366f, "Jamaican Pawn Shop", "We won't ask where you got it from","PawnShopMenuGeneric") {StateID = StaticStrings.LibertyStateID, MinPriceRefreshHours = 12, MaxPriceRefreshHours = 24,MinRestockHours = 12, MaxRestockHours = 24,MapIcon =642,},
            new PawnShop(new Vector3(3652.446f, -2249.346f, 26.84918f), 90.97377f, "Alderney City Pawn", "Find your bargain here","PawnShopMenuGeneric") {StateID = StaticStrings.AlderneyStateID, MinPriceRefreshHours = 12, MaxPriceRefreshHours = 24,MinRestockHours = 12, MaxRestockHours = 24,MapIcon =642,},
            new PawnShop(new Vector3(6116.207f, -3528.561f, 19.15461f), 94.30139f, "We Pay Cash Pawn", "","PawnShopMenuGeneric") {StateID = StaticStrings.LibertyStateID, MinPriceRefreshHours = 12, MaxPriceRefreshHours = 24,MinRestockHours = 12, MaxRestockHours = 24,MapIcon =642,},
            new PawnShop(new Vector3(5340.116f, -3512.748f, 14.75265f), 91.00311f, "Pawn Star", "","PawnShopMenuGeneric") {StateID = StaticStrings.LibertyStateID, MinPriceRefreshHours = 12, MaxPriceRefreshHours = 24,MinRestockHours = 12, MaxRestockHours = 24,MapIcon =642,},
            new PawnShop(new Vector3(5289.789f, -3432.477f, 14.54754f), 358.9048f, "Pawn Star", "","PawnShopMenuGeneric") {StateID = StaticStrings.LibertyStateID, MinPriceRefreshHours = 12, MaxPriceRefreshHours = 24,MinRestockHours = 12, MaxRestockHours = 24,MapIcon =642,},
            new PawnShop(new Vector3(4889.88f, -3577.672f, 11.58263f), 203.7976f, "Pawn Star", "","PawnShopMenuGeneric") {StateID = StaticStrings.LibertyStateID, MinPriceRefreshHours = 12, MaxPriceRefreshHours = 24,MinRestockHours = 12, MaxRestockHours = 24,MapIcon =642,},
            new PawnShop(new Vector3(6474.695f, -3322.353f, 27.82882f), 357.6094f, "XXX Pawn", "","PawnShopMenuGeneric") {StateID = StaticStrings.LibertyStateID, MinPriceRefreshHours = 12, MaxPriceRefreshHours = 24,MinRestockHours = 12, MaxRestockHours = 24,MapIcon =642,},
            new PawnShop(new Vector3(6263.058f, -3211.447f, 34.0183f), 271.6818f, "Pawn Shop", "","PawnShopMenuGeneric") {StateID = StaticStrings.LibertyStateID, MinPriceRefreshHours = 12, MaxPriceRefreshHours = 24,MinRestockHours = 12, MaxRestockHours = 24,MapIcon =642,},
            new PawnShop(new Vector3(6970.346f, -2789.782f, 28.17134f), 183.9617f, "We Pay Cash Pawn", "","PawnShopMenuGeneric") {StateID = StaticStrings.LibertyStateID, MinPriceRefreshHours = 12, MaxPriceRefreshHours = 24,MinRestockHours = 12, MaxRestockHours = 24,MapIcon =642,},
        });
    }
    private void DefaultConfig_PedCustomizeLocation() 
    {

        DefaultPedCustomizerLocation = new PedCustomizerLocation();
        //6068.041f, -3695.71f, 15.85331f), 176.317f
        DefaultPedCustomizerLocation.DefaultModelPedPosition = new Vector3(6067.887f, -3695.89943f, 15.85339f);
        DefaultPedCustomizerLocation.DefaultModelPedHeading = 184.7249f;
        DefaultPedCustomizerLocation.DefaultPlayerHoldingPosition = new Vector3(6075.87f, -3697.06389f, 15.86571f);
        List<CameraCyclerPosition> CameraCyclerPositions = new List<CameraCyclerPosition>();
        CameraCyclerPositions.Add(new CameraCyclerPosition("Default", new Vector3(6067.987f, -3698.28554f, 16.29714f), new Vector3(-0.007076373f, 0.9751766f, -0.2213152f), new Rotator(-12.78629f, 6.01895E-07f, 0.4157597f), 0));
        CameraCyclerPositions.Add(new CameraCyclerPosition("Face", new Vector3(6067.953f, -3696.66611f, 16.5719f), new Vector3(-0.01283151f, 0.9927738f, -0.1193125f), new Rotator(-6.852426f, -4.030857E-08f, 0.7405014f), 1));
        CameraCyclerPositions.Add(new CameraCyclerPosition("Lower", new Vector3(6067.926f, -3696.8565f, 15.26992f), new Vector3(-0.01943615f, 0.9749539f, -0.2215562f), new Rotator(-12.80045f, 2.73604E-08f, 1.142066f), 2));
        CameraCyclerPositions.Add(new CameraCyclerPosition("Torso", new Vector3(6067.966f, -3697.52415f, 15.86643f), new Vector3(-0.004468728f, 0.9992217f, -0.03919213f), new Rotator(-2.246119f, 1.184854E-07f, 0.256237f), 3));
        CameraCyclerPositions.Add(new CameraCyclerPosition("Hands", new Vector3(6067.966f, -3697.52415f, 15.86643f), new Vector3(-0.004468728f, 0.9992217f, -0.03919213f), new Rotator(-2.246119f, 1.184854E-07f, 0.256237f), 4));
        DefaultPedCustomizerLocation.CameraCyclerPositions = CameraCyclerPositions;

    }
    private void DefaultConfig_Pharmacies()
    {
        LibertyCityLocations.Pharmacies.AddRange(new List<Pharmacy>()
        {
                        new Pharmacy() {
                MenuID = "PharmacyMenu",
                Name = "Pill Pharm",
                Description = "Big Pharma, big prices",
                EntrancePosition = new Vector3(4853.413f, -3104.616f, 14.75428f),
                EntranceHeading = 177.06f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Pharmacy() {
                MenuID = "PharmacyMenu",
                Name = "Pill Pharm",
                Description = "Big Pharma, big prices",
                EntrancePosition = new Vector3(7009.23f, -2651.301f, 28.8674f),
                EntranceHeading = 88.21826f,
                OpenTime = 9,
                CloseTime = 23,
                StateID = StaticStrings.LibertyStateID,
            },
            new Pharmacy() {
                MenuID = "PharmacyMenu",
                Name = "Pill Pharm",
                Description = "Big Pharma, big prices",
                EntrancePosition = new Vector3(6527.049f, -1611.949f, 16.83739f),
                EntranceHeading = 266.0045f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Pharmacy() {
                MenuID = "PharmacyMenu",
                Name = "Pill Pharm",
                Description = "Big Pharma, big prices",
                EntrancePosition = new Vector3(3888.257f, -2426.708f, 19.80579f),
                EntranceHeading = 89.4726f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.AlderneyStateID,
            },
            new Pharmacy() {
                MenuID = "PharmacyMenu",
                Name = "Pill Pharm",
                Description = "Big Pharma, big prices",
                EntrancePosition = new Vector3(6431.979f, -2527.004f, 36.74868f),
                EntranceHeading = 272.0062f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Pharmacy() {
                MenuID = "PharmacyMenu",
                Name = "Aycehol",
                Description = "Same pills as anywhere else, but cheaper",
                EntrancePosition = new Vector3(4913.194f, -1979.594f, 24.71129f),
                EntranceHeading = 219.3275f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Pharmacy(new Vector3(3724.937f, -2687.53f, 19.56816f), 80.00731f, "AYCEHOL", "","PharmacyMenu") { StateID = StaticStrings.AlderneyStateID },
            new Pharmacy(new Vector3(3680.956f, -2191.346f, 25.37675f), 0.3532917f, "Thrush Boy", "","PharmacyMenu") { StateID = StaticStrings.AlderneyStateID },
            new Pharmacy(new Vector3(4008.871f, -2370.227f, 19.77452f), 268.4793f, "Thrush Boy", "","PharmacyMenu") { StateID = StaticStrings.AlderneyStateID },
            new Pharmacy(new Vector3(4170.564f, -2160.994f, 13.77298f), 90.19047f, "Thrush Boy", "","PharmacyMenu") { StateID = StaticStrings.AlderneyStateID },
            new Pharmacy(new Vector3(3706.968f, -2328.875f, 23.2621f), 359.0571f, "Novella Drugs", "","PharmacyMenu") { StateID = StaticStrings.AlderneyStateID },
            new Pharmacy(new Vector3(3966.223f, -2416.274f, 19.7807f), 88.70953f, "Novella Drugs", "","PharmacyMenu") { StateID = StaticStrings.AlderneyStateID },

            new Pharmacy(new Vector3(6116.034f, -3665.003f, 16.67797f), 87.6747f, "Hove Beach Pharmacy", "","PharmacyMenu") { StateID = StaticStrings.LibertyStateID },
            new Pharmacy(new Vector3(6161.523f, -3661.628f, 15.89524f), 357.1013f, "Hove Beach Pharmacy", "","PharmacyMenu") { StateID = StaticStrings.LibertyStateID },

            new Pharmacy(new Vector3(5216.05f, -2525.226f, 14.66381f), 270.2766f, "Pill Pharm", "","PharmacyMenu") { StateID = StaticStrings.LibertyStateID },
            new Pharmacy(new Vector3(5341.674f, -3555.333f, 14.75203f), 188.8304f, "Pill Pharm", "","PharmacyMenu") { StateID = StaticStrings.LibertyStateID },
            new Pharmacy(new Vector3(5300.449f, -3321.144f, 14.76408f), 268.9501f, "Pill Pharm", "","PharmacyMenu") { StateID = StaticStrings.LibertyStateID },
            new Pharmacy(new Vector3(5257.861f, -3478.663f, 14.81673f), 263.2702f, "Pill Pharm", "","PharmacyMenu") { StateID = StaticStrings.LibertyStateID },
            new Pharmacy(new Vector3(4799.768f, -1850.502f, 12.30257f), 311.136f, "Pill Pharm", "","PharmacyMenu") { StateID = StaticStrings.LibertyStateID },
            new Pharmacy(new Vector3(4806.884f, -2745.959f, 14.76466f), 267.493f, "Pill Pharm", "","PharmacyMenu") { StateID = StaticStrings.LibertyStateID },
            new Pharmacy(new Vector3(6363.617f, -3488.316f, 23.50771f), 358.195f, "Pill Pharm", "","PharmacyMenu") { StateID = StaticStrings.LibertyStateID },

            new Pharmacy(new Vector3(4843.26f, -2892.96f, 14.7629f), 87.98587f, "Ned's Drugs", "","PharmacyMenu") { StateID = StaticStrings.LibertyStateID },
            new Pharmacy(new Vector3(4938.552f, -1946.829f, 24.73111f), 93.01106f, "Ned's Drugs", "","PharmacyMenu") { StateID = StaticStrings.LibertyStateID },
            new Pharmacy(new Vector3(4851.434f, -2724.476f, 14.76022f), 1.005996f, "Ned's Drugs", "","PharmacyMenu") { StateID = StaticStrings.LibertyStateID },
            new Pharmacy(new Vector3(6577.845f, -3113.875f, 28.57576f), 63.08421f, "Drugs", "","PharmacyMenu") { StateID = StaticStrings.LibertyStateID },

            new Pharmacy(new Vector3(6432.845f, -3229.279f, 36.07855f), 179.4188f, "Greavsy's Pharmacy", "","PharmacyMenu") { StateID = StaticStrings.LibertyStateID },
            new Pharmacy(new Vector3(6985.764f, -2783.645f, 28.02918f), 270.1378f, "Fullon Drugs", "","PharmacyMenu") { StateID = StaticStrings.LibertyStateID },
            new Pharmacy(new Vector3(6404.74f, -2883.522f, 21.92743f), 268.4826f, "Drugs", "","PharmacyMenu") { OpenTime = 0, CloseTime = 24, StateID = StaticStrings.LibertyStateID },
        });
    }
    private void DefaultConfig_PoliceStations()
    {
        float pedSpawnPercentage = 40f;
        float vehicleSpawnPercentage = 65f;
        Vector2[] LeftwoodImpoundLot = new Vector2[]
        {
                    new Vector2 { X = 4274.84668f, Y = -1920.6532f },
                    new Vector2 { X = 4347.869f,    Y = -1920.43823f },
                    new Vector2 { X = 4347.869f,    Y = -1973.52515f },
                    new Vector2 { X = 4305.06348f, Y = -1973.15125f },
                    new Vector2 { X = 4302.661f,   Y = -1940.26318f },
                    new Vector2 { X = 4291.53027f, Y = -1940.20215f },
                    new Vector2 { X = 4291.362f,   Y = -1929.94922f },
                    new Vector2 { X = 4273.633f,   Y = -1930.27124f }
        };
        Vector2[] EastHollandPoliceLot = new Vector2[]
        {
                    new Vector2 { X = 5274.10938f, Y = -1994.61121f },
                    new Vector2 { X = 5295.161f, Y = -2031.73523f },
                    new Vector2 { X = 5283.483f, Y = -2034.06519f },
                    new Vector2 { X = 5283.397f, Y = -2045.16919f },
                    new Vector2 { X = 5271.621f, Y = -2045.26819f },
                    new Vector2 { X = 5271.46143f, Y = -2031.99817f },
                    new Vector2 { X = 5261.175f, Y = -2028.34021f },
                    new Vector2 { X = 5260.71436f, Y = -2013.95117f },
                    new Vector2 { X = 5251.10254f, Y = -2012.48022f },
                    new Vector2 { X = 5251.104f, Y = -1994.99023f },
        };
        Vector2[] LowerEastonPoliceLot = new Vector2[]
        {
                    new Vector2 { X = 5473.349f, Y = -3458.075f },
                    new Vector2 { X = 5472.941f, Y = -3433.946f },
                    new Vector2 { X = 5421.408f, Y = -3433.877f },
                    new Vector2 { X = 5394.594f, Y = -3431.283f },
                    new Vector2 { X = 5402.604f, Y = -3448.904f },
                    new Vector2 { X = 5422.244f, Y = -3445.142f },
                    new Vector2 { X = 5441.926f, Y = -3452.233f },
        };
        List<PoliceStation> PoliceStations = new List<PoliceStation>()
        {
        //Broker
            new PoliceStation(new Vector3(6418.988f, -3345.562f, 28.02763f), 272.2312f, "South Slopes Police Station","") { MaxAssaultSpawns = 10,AssaultSpawnHeavyWeaponsPercent = 80f,OpenTime = 0,CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
                ActivateDistance = 200,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(6419.906f, -3342.09229f, 28.0614243f), 334.1368f, 65f),
                    new LEConditionalLocation(new Vector3(6421.43555f, -3349.24023f, 28.0005131f), 233.3257f, 65f),
                    new LEConditionalLocation(new Vector3(6422.234f, -3336.35718f, 28.1610641f), 335.372f, 65f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(6395.007f, -3356.58081f, 28.3317127f), 88.59583f, 95f),
                },
            },
            new PoliceStation(new Vector3(6082.613f, -3614.49f, 18.17972f), 319.244f, "Hove Beach Police Station","") {MaxAssaultSpawns = 5,AssaultSpawnHeavyWeaponsPercent = 80f,OpenTime = 0,CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
                ActivateDistance = 200,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(6081.61768f, -3611.80542f, 17.4082432f), 54.60419f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(6088.30273f, -3616.09766f, 17.4921131f), 283.6084f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(6073.63965f, -3611.21021f, 16.7053242f), 333.9313f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(6078.652f, -3641.718f, 17.1549034f), 243.2021f, pedSpawnPercentage),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(6077.839f, -3645.71f, 16.52364f), 269.8352f, 85f),
                    new LEConditionalLocation(new Vector3(6090.566f, -3621.61f, 16.9441f), 179.1692f, 85f),
                },
            },

        //Dukes
            new PoliceStation(new Vector3(6420.789f, -2730.289f, 30.82755f), 181.1308f, "East Island City Police Station","") {MaxAssaultSpawns = 20,AssaultSpawnHeavyWeaponsPercent = 80f,OpenTime = 0,CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
                ActivateDistance = 200,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(6423.798f, -2732.5332f, 30.827444f), 183.1868f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(6417.36963f, -2732.975f, 30.8278732f), 156.0497f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(6410.384f, -2739.61719f, 28.5735626f), 139.4183f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(6438.739f, -2739.54834f, 28.0327644f), 267.2982f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(6420.191f, -2681.64624f, 38.5026131f), 292.8177f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(6437.133f, -2681.67236f, 38.5339622f), 332.2541f, pedSpawnPercentage),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(6421.313f, -2672.82f, 38.36271f), 52.43164f, 75f),
                    new LEConditionalLocation(new Vector3(6427.447f, -2677.277f, 38.3623f), 284.1253f, 75f),
                    new LEConditionalLocation(new Vector3(6416.841f, -2742.642f, 28.36619f), 87.88049f, 45f),
                },
            },
            new PoliceStation(new Vector3(7354.333f, -2807.144f, 7.022233f), 266.4873f, "FIA Police Station","") { MaxAssaultSpawns = 25,AssaultSpawnHeavyWeaponsPercent = 80f,OpenTime = 0,CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
                ActivateDistance = 200,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(7357.26367f, -2803.16846f, 6.08002424f), 262.8414f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(7357.086f, -2812.613f, 6.080184f), 199.5635f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(7352.356f, -2846.254f, 6.080178f), 242.6387f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(7324.84f, -2844.88f, 6.079988f), 181.5603f, pedSpawnPercentage),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(7299.006f, -2827.341f, 5.938364f), 87.76071f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(7299.983f, -2817.535f, 5.833596f), 86.69381f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(7300.365f, -2797.244f, 5.738731f), 83.99401f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(7299.936f, -2790.902f, 5.940259f), 89.7596f, vehicleSpawnPercentage),
                },
            },

        //Bohan
            new PoliceStation(new Vector3(5623.664f, -1665.839f, 18.22757f), 2.324484f, "Fortside Police Station","") {MaxAssaultSpawns = 15,AssaultSpawnHeavyWeaponsPercent = 80f,OpenTime = 0,CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
                ActivateDistance = 200,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(5616.484f, -1657.716f, 16.22927f), 41.3483f, 55f),
                    new LEConditionalLocation(new Vector3(5627.199f, -1659.081f, 16.26539f), 3.362263f, 55f),
                    new LEConditionalLocation(new Vector3(5620.432f, -1661.793f, 16.33f), 323.5594f, 55f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(5631.418f, -1661.035f, 16.1045f), 358.032f, 95f),
                },
            },
            new PoliceStation(new Vector3(6170.458f, -1386.052f, 23.89312f), 91.60741f, "Northern Gardens LCPD Station","") {MaxAssaultSpawns = 20,AssaultSpawnHeavyWeaponsPercent = 80f,OpenTime = 0,CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
                ActivateDistance = 200,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(6163.853f, -1381.281f, 23.06227f), 57.77385f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(6176.521f, -1379.698f, 23.89314f), 33.39428f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(6164.289f, -1391.006f, 23.07558f), 127.6019f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(6168.975f, -1388.537f, 23.89312f), 54.13597f, pedSpawnPercentage),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(6185.989f, -1363.811f, 23.54665f), 359.3115f, 75f),
                    new LEConditionalLocation(new Vector3(6178.155f, -1364.812f, 23.7536f), 178.744f, 75f),
                    new LEConditionalLocation(new Vector3(6170.779f, -1364.142f, 23.75362f), 3.000886f, 75f),
                },
            },

        //Algonquin
            new PoliceStation(new Vector3(4803.21f, -3520.635f, 13.04974f), 270.1865f, "Suffolk Police Station","") {MaxAssaultSpawns = 20,AssaultSpawnHeavyWeaponsPercent = 80f,OpenTime = 0,CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
                ActivateDistance = 200,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(4806.738f, -3517.064f, 11.46553f), 283.2104f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(4805.037f, -3503.164f, 12.83038f), 357.0255f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(4808.298f, -3525.039f, 10.3001f), 214.2638f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(4790.75f, -3528.714f, 12.82989f), 94.81232f, pedSpawnPercentage),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(4796.667f, -3507.423f, 12.56685f), 264.8217f, 75f),
                    new LEConditionalLocation(new Vector3(4786.825f, -3525.738f, 12.55947f), 270.245f, 75f),
                    new LEConditionalLocation(new Vector3(4775.915f, -3520.134f, 12.45303f), 2.609038f, 75f),
                },
            },
            new PoliceStation(new Vector3(5399.731f, -3462.816f, 10.75111f), 207.7276f, "Lower Easton Police Station","") {MaxAssaultSpawns = 20,AssaultSpawnHeavyWeaponsPercent = 80f,OpenTime = 0,CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
                ActivateDistance = 200,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(5490.302f, -3474.18f, 4.988965f), 86.89471f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5460.186f, -3474.509f, 6.785522f), 87.48795f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5431.714f, -3471.65f, 10.75093f), 83.85837f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5412.937f, -3470.078f, 10.73796f), 180.9898f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5411.72f, -3470.086f, 10.7378f), 177.4162f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5399.535f, -3466.974f, 10.74682f), 200.7389f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5360.249f, -3466.353f, 14.84581f), 113.2131f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5402.582f, -3447.374f, 5.057099f), 354.0423f, pedSpawnPercentage),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(5348.926f, -3465.929f, 14.36937f), 296.8697f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5346.023f, -3459.965f, 14.3681f), 294.577f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5346.884f, -3463.181f, 14.38679f), 294.4719f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5343.792f, -3453.773f, 14.33414f), 295.4254f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5452.258f, -3461.14f, 5.251266f), 206.4337f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5406.19f, -3440.302f, 4.69532f), 297.6694f, vehicleSpawnPercentage),
                },
                VehicleImpoundLot = new VehicleImpoundLot("Lower Easton Impound spots",new List<SpawnPlace>()
                {
                    new SpawnPlace(new Vector3(5489.677f, -3450.136f, 4.571483f), 272.8017f),
                    new SpawnPlace(new Vector3(5499.376f, -3448.265f, 4.354964f), 270.0375f),
                    new SpawnPlace(new Vector3(5499.812f, -3467.443f, 4.337479f), 271.6652f),

                }),
                RestrictedAreas = new RestrictedAreas()
                {
                    RestrictedAreasList = new List<RestrictedArea>()
                    {
                        new RestrictedArea("Lower Easton Police Rear Parking", LowerEastonPoliceLot,new List<InteriorDoor>(), RestrictedAreaType.PoliceLot)
                        {
                            SecurityCameras = new List<SecurityCamera>()
                            {
                                new SecurityCamera(Game.GetHashKey("prop_cctv_pole_04"), new Vector3(5503.21338f, -3433.68848f, 3.57940388f), 107f) { Name = "Security Cam 1",IsManuallyCreated = true,PropHeading = 142.7063f-90f-45f, },
                                new SecurityCamera(Game.GetHashKey("prop_cctv_pole_04"), new Vector3(5503.23438f, -3477.8064f, 3.57630682f), 36f) { Name = "Security Cam 2",IsManuallyCreated = true,PropHeading = 40.4807f-90f-45f, },
                            }
                        },
                        new RestrictedArea("Lower Easton Police Impound",new Vector2[]
                        {
                               new Vector2(5503.751f, -3478.199f),
                               new Vector2(5503.56f, -3433.331f),
                               new Vector2(5475.683f, -3434f),
                               new Vector2(5475.159f, -3477.238f),
                        },
                        null,RestrictedAreaType.ImpoundLot) { IsZRestricted = true, ZRestrictionMin = 5.6835f - 3f,ZRestrictionMax = 5.6835f + 3f, IsCivilianReactableRestricted = false},

                    },
                }
            },
            new PoliceStation(new Vector3(5003.299f, -3156.025f, 14.77005f), 358.7707f, "Star Junction Police Station","") {MaxAssaultSpawns = 5,AssaultSpawnHeavyWeaponsPercent = 50f,OpenTime = 0,CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
                ActivateDistance = 200,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(5006.094f, -3152.473f, 14.76411f), 337.0898f, 65f),
                    new LEConditionalLocation(new Vector3(5001.479f, -3155.006f, 14.76911f), 3.176514f, 65f),
                    new LEConditionalLocation(new Vector3(4999.102f, -3168.316f, 14.76231f), 98.34142f, 65f),
                    new LEConditionalLocation(new Vector3(5007.502f, -3167.843f, 14.7714f), 284.8917f, 65f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(5001.57f, -3142.866f, 14.37389f), 117.0759f, 95f),
                },
            },
            new PoliceStation(new Vector3(4781.495f, -2973.722f, 13.45294f), 179.5452f, "Westminster Police Station","") {MaxAssaultSpawns = 20,AssaultSpawnHeavyWeaponsPercent = 80f,OpenTime = 0,CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
                ActivateDistance = 200,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(4772.436f, -2976.816f, 12.01621f), 170.0619f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(4781.962f, -2976.162f, 13.16354f), 211.1363f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(4784.074f, -2976.237f, 13.39003f), 188.4643f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(4790.628f, -2975.454f, 14.03321f), 132.8757f, pedSpawnPercentage),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(4787.458f, -2980.963f, 13.35588f), 87.22291f, 75f),
                    new LEConditionalLocation(new Vector3(4779.461f, -2980.923f, 12.58321f), 75.18746f, 75f),
                },
            },
            new PoliceStation(new Vector3(5239.78f, -2576.575f, 15.3212f), 87.41449f, "Middle Park East Police Station","") {MaxAssaultSpawns = 20,AssaultSpawnHeavyWeaponsPercent = 80f,OpenTime = 0,CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
                ActivateDistance = 200,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(5239.163f, -2570.285f, 14.66386f), 87.57539f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5238.496f, -2571.656f, 14.66386f), 71.29387f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5270.519f, -2583.627f, 14.66458f), 219.5064f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5271.993f, -2583.531f, 14.66458f), 184.318f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5275.406f, -2578.736f, 14.66457f), 268.6536f, pedSpawnPercentage),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(5281.892f, -2584.685f, 14.37539f), 269.6314f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5278.247f, -2572.756f, 14.27209f), 1.050404f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5281.412f, -2573.05f, 14.27423f), 358.7294f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5284.697f, -2573.392f, 14.27586f), 1.053917f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5287.776f, -2573.229f, 14.27812f), 0.9146708f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5290.925f, -2573.06f, 14.27742f), 359.4021f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5294.504f, -2573.088f, 14.3835f), 3.732607f, vehicleSpawnPercentage),

                },
            },
            new PoliceStation(new Vector3(4771.737f, -2153.025f, 11.76445f), 177.4595f, "Varsity Heights Police Station","") {MaxAssaultSpawns = 20,AssaultSpawnHeavyWeaponsPercent = 80f,OpenTime = 0,CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
                ActivateDistance = 200,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(4774.241f, -2122.102f, 13.10175f), 8.474848f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(4760.966f, -2136.496f, 11.5754f), 123.7708f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(4760.956f, -2138.759f, 11.44596f), 73.34553f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(4774.623f, -2152.885f, 12.07393f), 173.8114f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(4775.393f, -2153.268f, 12.15638f), 162.5813f, pedSpawnPercentage),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(4770.717f, -2158.374f, 11.41333f), 78.20446f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(4751.282f, -2145.686f, 10.1115f), 89.66927f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(4751.356f, -2139.225f, 10.72994f), 90.79951f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(4751.459f, -2132.601f, 11.318f), 90.80878f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(4751.492f, -2125.232f, 12.05329f), 90.83681f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(4751.803f, -2118.78f, 12.51267f), 90.72213f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(4752.157f, -2112.294f, 12.73819f), 88.57416f, vehicleSpawnPercentage),
                },
            },
            new PoliceStation(new Vector3(5273.322f, -2064.258f, 14.74501f), 171.6931f, "East Holland LCPD Station","") {MaxAssaultSpawns = 20,AssaultSpawnHeavyWeaponsPercent = 80f,OpenTime = 0,CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
                ActivateDistance = 200,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(5276.288f, -2066.956f, 14.75865f), 179.2575f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5278.191f, -2066.81f, 14.76007f), 144.7317f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5241.702f, -2045.996f, 18.7387f), 87.88927f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5257.355f, -2026.198f, 16.06882f), 274.3711f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5283.914f, -2007.254f, 15.82695f), 302.058f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5279.971f, -2036.554f, 16.00098f), 2.217796f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5278.912f, -2036.635f, 16.00099f), 2.363437f, pedSpawnPercentage),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(5269.278f, -2006.503f, 15.77565f), 205.8322f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5281.202f, -2013.787f, 15.52427f), 302.726f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5282.837f, -2016.316f, 15.7746f), 301.6384f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5286.387f, -2021.763f, 15.76788f), 298.4713f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5288.567f, -2027.491f, 15.65973f), 295.0797f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5257.193f, -1999.357f, 15.75821f), 269.8339f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(5282.896f, -2074.248f, 14.36401f), 48.12418f, vehicleSpawnPercentage),
                },
                RestrictedAreas = new RestrictedAreas()
                {
                    RestrictedAreasList = new List<RestrictedArea>()
                    {
                        new RestrictedArea("East Holland Police Parking", EastHollandPoliceLot,new List<InteriorDoor>(), RestrictedAreaType.PoliceLot)
                        {
                            SecurityCameras = new List<SecurityCamera>()
                            {
                                new SecurityCamera(Game.GetHashKey("prop_cctv_pole_04"), new Vector3(5295.06836f, -2031.65308f, 14.7418852f), 68.50541f) { Name = "Security Cam 1",IsManuallyCreated = true,PropHeading = 63.87578f-90f-45f, },
                                new SecurityCamera(Game.GetHashKey("prop_cctv_pole_04"), new Vector3(5274.04f, -1994.53564f, 15.0351963f), 180.5152f) { Name = "Security Cam 2",IsManuallyCreated = true,PropHeading = 153.6937f-90f-45f, },
                            }
                        },
                    },
                }
            },

            //Alderney
            new PoliceStation(new Vector3(3469.428f, -2978.233f, 22.91861f), 238.3614f, "West District Mini Precinct","") { MaxAssaultSpawns = 5,AssaultSpawnHeavyWeaponsPercent = 50f,OpenTime = 0,CloseTime = 24,
                StateID = StaticStrings.AlderneyStateID,
                ActivateDistance = 200,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(3472.816f, -2976.814f, 22.30948f), 243.3378f, 75f),
                    new LEConditionalLocation(new Vector3(3470.434f, -2981.651f, 22.13208f), 199.5405f, 75f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(3482.916f, -2960.329f, 22.78678f), 236.5009f, 95f),
                },
            },
            new PoliceStation(new Vector3(3963.007f, -3491.965f, 2.993861f), 315.4824f, "Acter Police Station","") { MaxAssaultSpawns = 20,AssaultSpawnHeavyWeaponsPercent = 80f, OpenTime = 0,CloseTime = 24,
                StateID = StaticStrings.AlderneyStateID,
                ActivateDistance = 200,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(-982.7232f, 254.9283f, 3.053053f), 278.8697f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-991.2359f, 262.9456f, 2.998651f), 35.81685f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-981.2977f, 245.785f, 3.053054f), 276.9689f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-1008.636f, 236.9965f, 2.923784f), 116.7921f, pedSpawnPercentage),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(3953.22f, -3532.989f, 2.682328f), 271.6151f, 75f),
                    new LEConditionalLocation(new Vector3(3950.209f, -3543.437f, 2.681542f), 188.6341f, 75f),
                    new LEConditionalLocation(new Vector3(3955.905f, -3542.423f, 2.681196f), 176.9486f, 75f),
                    new LEConditionalLocation(new Vector3(3930.74f, -3526.112f, 2.681715f), 0.8127336f, 75f),
                    new LEConditionalLocation(new Vector3(3930.617f, -3517.44f, 2.681326f), 0.8951726f, 75f),
                },
                RestrictedAreas = new RestrictedAreas()
                {
                    RestrictedAreasList = new List<RestrictedArea>()
                    {
                        new RestrictedArea("Acter Police Parking Lot",new Vector2[]
                        {
                            new Vector2(3930.11377f, -3490.95361f),
                            new Vector2(3943.09668f, -3503.92651f),
                            new Vector2(3943.109f, -3528.89038f),
                            new Vector2(3963.87524f, -3528.891f),
                            new Vector2(3963.97485f, -3537.97241f),
                            new Vector2(3958.15283f, -3538.398f),
                            new Vector2(3958.13257f, -3551.59814f),
                            new Vector2(3948.36377f, -3551.60156f),
                            new Vector2(3948.31372f, -3542.09546f),
                            new Vector2(3928.628f, -3528.22119f),
                            new Vector2(3928.54272f, -3504.09961f),
                            new Vector2(3922.51172f, -3504.05078f),
                            new Vector2(3914.26465f, -3495.80151f),
                            new Vector2(3914.21582f, -3491.32227f)
                        },
                        null,RestrictedAreaType.PoliceLot),
                    },
                }
            },
            new PoliceStation(new Vector3(4265.065f, -1985.915f, 25.56437f), 134.8335f, "Leftwood Police Station","") { MaxAssaultSpawns = 20,AssaultSpawnHeavyWeaponsPercent = 80f,OpenTime = 0,CloseTime = 24,
                StateID = StaticStrings.AlderneyStateID,
                ActivateDistance = 200,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(4265.18f, -1994.86f, 24.60876f), 161.9344f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(4263.538f, -1994.7f, 24.60744f), 184.9278f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(4260.898f, -1988.13f, 24.56688f), 102.4546f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(4260.29f, -1959.62f, 24.56137f), 91.51962f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(4302.233f, -1993.985f, 24.54015f), 178.0969f, pedSpawnPercentage),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(4265.761f, -1949.633f, 24.20132f), 243.3829f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(4266.456f, -1942.89f, 24.31766f), 249.6494f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(4266.572f, -1935.726f, 24.2798f), 246.6627f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(4317.634f, -1938.183f, 21.83139f), 243.957f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(4317.496f, -1945.345f, 21.83187f), 245.8395f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(4317.915f, -1966.812f, 21.83265f), 180.7851f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(4330.919f, -1966.612f, 21.83206f), 178.9781f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(4309.465f, -1964.524f, 21.7186f), 181.8762f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(4326.279f, -1953.267f, 21.72579f), 63.84319f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(4326.494f, -1946.083f, 21.72463f), 61.51813f, vehicleSpawnPercentage),
                },
                VehicleImpoundLot = new VehicleImpoundLot("Leftwood Police Impound spots",new List<SpawnPlace>()
                {
                    new SpawnPlace(new Vector3(4343.00732f, -1954.33423f, 21.5938129f), 273.3782f),
                    new SpawnPlace(new Vector3(4342.936f, -1957.97424f, 21.5940838f), 266.869f),
                    new SpawnPlace(new Vector3(4343.73975f, -1962.61523f, 21.5942535f), 269.0397f),

                }),
                RestrictedAreas = new RestrictedAreas()
                {
                    RestrictedAreasList = new List<RestrictedArea>()
                    {
                        new RestrictedArea("Leftwood Police Parking", LeftwoodImpoundLot,new List<InteriorDoor>(), RestrictedAreaType.ImpoundLot) {
                            SecurityCameras = new List<SecurityCamera>() {
                                new SecurityCamera(Game.GetHashKey("prop_cctv_pole_04"), new Vector3(4347.167f, -1973.02417f, 21.0898037f), 44.0f) { Name = "Security Cam 1",IsManuallyCreated = true,PropHeading = 32.37564f-90f-45f, },
                                new SecurityCamera(Game.GetHashKey("prop_cctv_pole_04"), new Vector3(4302.627f, -1930.66418f, 20.9698029f), 180.0f) { Name = "Security Cam 2",IsManuallyCreated = true,PropHeading = 217.9528f-90f-45f, },
                            } },

                    }
                }
            },

            //North Yankton
            new PoliceStation(new Vector3(3142.471f, -4840.832f, 112.0291f), 349.9769f, "NYSP Office Ludendorff","The return of the Keystone Cops") {
                StateID = StaticStrings.NorthYanktonStateID,
                AssignedAssociationID = "NYSP",
                OpenTime = 0,
                CloseTime = 24,
                MaxAssaultSpawns = 2,
                PossiblePedSpawns = new List<ConditionalLocation>() {

                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                 {  } },
        };
        LibertyCityLocations.PoliceStations.AddRange(PoliceStations);
    }
    private void DefaultConfig_Prisons()
    {
        List<Prison> LCPrison = new List<Prison>()
        {
            new Prison(new Vector3(4046.045f, -3631.298f, 3.082006f), 91.96843f, "Alderney State Correctional Facility","")
            {
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.AlderneyStateID,
                ActivateCells = 8,
                AssignedAssociationID = "SASPA",
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(4076.59937f, -3631.146f, 3.178797f), 88.80982f, 100f) { TaskRequirements = TaskRequirements.Guard,LongGunAlwaysEquipped = true },//front dooor main building
                    new LEConditionalLocation(new Vector3(4102.956f, -3608.64917f, 3.41224f), 355.3642f, 100f) { TaskRequirements = TaskRequirements.Guard,LongGunAlwaysEquipped = true },//side door to main building
                    new LEConditionalLocation(new Vector3(4050.059f, -3660.26465f, 13.3016024f), 268.2614f, 40f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.EquipSidearmWhenIdle,LongGunAlwaysEquipped = true, ForceSidearm = true,ForceLongGun = true},//guard facing towards entry
                    new LEConditionalLocation(new Vector3(4049.93164f, -3600.3916f, 13.3016024f), 86.85568f, 40f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.EquipSidearmWhenIdle,LongGunAlwaysEquipped = true, ForceSidearm = true,ForceLongGun = true},//guard facing towards entry second guard tower
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(4069.67236f, -3639.63916f, 2.75095916f), 180.6921f, 35f),//near  main enterance
                    new LEConditionalLocation(new Vector3(4069.35449f, -3727.69775f, 1.82121789f), 182.1497f, 35f),//prison carpark
                    new LEConditionalLocation(new Vector3(4059.37f, -3709.02783f, 1.821126f), 3.043505f, 35f),//prison carpark
                    new LEConditionalLocation( new Vector3(4052.29175f, -3709.56885f, 1.8211019f), 1.890841f, 35f),//prison carpark
                },
                RestrictedAreas = new RestrictedAreas()
                {
                    RestrictedAreasList = new List<RestrictedArea>()
                    {
                        new RestrictedArea("PrisonRear",new Vector2[]
                        {
                               new Vector2(4290.33643f, -3713.63843f),
                               new Vector2(4290.44141f, -3605.75562f),
                               new Vector2(4246.951f, -3605.79614f),
                               new Vector2(4246.695f, -3579.869f),
                               new Vector2(4156.734f, -3579.83838f),
                               new Vector2(4158.64648f, -3713.75f),
                        },
                        null,RestrictedAreaType.Jail),
                        new RestrictedArea("PrisonFront",new Vector2[]
                        {
                               new Vector2(4158.332f, -3755.06f),
                               new Vector2(4006.66724f, -3755.04883f),
                               new Vector2(4006.729f, -3693.67969f),
                               new Vector2(4047.87939f, -3693.61572f),
                               new Vector2(4047.95435f, -3579.9f),
                               new Vector2(4157.89258f, -3579.799f),
                        },
                        null,RestrictedAreaType.Jail)
                    },
                }
            },
        };
        LibertyCityLocations.Prisons.AddRange(LCPrison);
    }
    private void DefaultConfig_RaceMeetups()
    {
        RaceMeetups = new List<RaceMeetup>()
        {
            new RaceMeetup(new Vector3(3828.541f, -3241.27f, 7.163781f), 150.4955f, "Tudor Meetup", "Meetup with other racers from the city","")
            {
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.AlderneyStateID,
                SupportedTracks = new List<string>() { "prison1" , "hardtack1"},

            },
            new RaceMeetup(new Vector3(3653.176f, -2142.247f, 26.69221f), 236.4845f, "Alderney Meetup", "Meetup with other racers from the city","")
            {
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.AlderneyStateID,
                SupportedTracks = new List<string>() { "westdyke1" , "plumsky1"},

            },
            new RaceMeetup(new Vector3(4976.788f, -2066.643f, 14.61924f), 1.220425f, "MiddlePark Meetup", "Meetup with other racers from the city","")
            {
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
                CameraPosition = new Vector3(4976.431f, -2069.929f, 20.84595f),
                CameraDirection = new Vector3(0.01231408f, -0.9983855f, -0.05544958f),
                CameraRotation = new Rotator(-3.178657f, 1.336077E-07f, -179.2934f),
                SupportedTracks = new List<string>() { "middlepark1" , "starjdrag1" , "Lancaster1" },

            },
            new RaceMeetup(new Vector3(6541.196f, -2347.363f, 13.83027f), 141.175f, "Dukes Race Meetup", "Meetup with other racers from the city","")
            {
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
                CameraPosition = new Vector3(6537.807f, -2348.978f, 17.92535f),
                CameraDirection = new Vector3(0.9890541f, 0.1072537f, -0.1013345f),
                CameraRotation = new Rotator(-5.816022f, -2.681847E-07f, -83.81099f),
                SupportedTracks = new List<string>() { "dukeshighway1" , "dukesboule1" },

            },
            new RaceMeetup(new Vector3(7474.419f, -2904.387f, 6.093733f), 274.0861f, "FIA Race Meetup", "Meetup with other racers from the city","")
            {
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
                CameraPosition = new Vector3(7475.025f, -2903.615f, 7.334247f),
                CameraDirection = new Vector3(-0.8769872f, -0.4365083f, -0.2008831f),
                CameraRotation = new Rotator(-11.58861f, -7.843859E-06f, 116.4612f),
                SupportedTracks = new List<string>() { "fiahotring1" , "fiastarjunction" },

            },
        };
        LibertyCityLocations.RaceMeetups.AddRange(RaceMeetups);
    }
    private void DefaultConfig_RepairGarages()
    {
        RepairGarages = new List<RepairGarage>()
    {
        new RepairGarage(new Vector3(5900.374f, -1750.027f, 14.45882f), 3.357487f, "Muscle Mary's", "We service imports too")
        {
            CameraPosition = new Vector3(5887.211f, -1761.411f, 18.65158f),
            CameraDirection = new Vector3(-0.3118665f, 0.9345616f, -0.1712713f),
            CameraRotation = new Rotator(-9.861743f, -3.899601E-06f, 18.45404f),
            OpenTime = 0,
            CloseTime = 24,
            CanInteractWhenWanted = true,
            HasNoGarageDoors = true,
            StateID = StaticStrings.LibertyStateID,
        },
        new RepairGarage(new Vector3(6942.331f, -2891.763f, 22.10875f), 179.815f, "The East Boys Auto Shop", "")
        {
            CameraPosition = new Vector3(6940.29f, -2896.64f, 28.42227f),
            CameraDirection = new Vector3(0.2205064f, 0.9739193f, -0.05346161f),
            CameraRotation = new Rotator(-3.064586f, -1.474869E-05f, -12.75733f),
            OpenTime = 0,
            CloseTime = 24,
            CanInteractWhenWanted = true,
            HasNoGarageDoors = true,
            StateID = StaticStrings.LibertyStateID,
        },
        new RepairGarage(new Vector3(6982.14f, -3061.779f, 20.70853f), 338.3272f, "Auto Repairs", "")
        {
            CameraPosition = new Vector3(6972.773f, -3068.515f, 25.59239f),
            CameraDirection = new Vector3(0.1797911f, 0.9785702f, -0.1003773f),
            CameraRotation = new Rotator(-5.760899f, -4.719591E-06f, -10.41076f),
            OpenTime = 0,
            CloseTime = 24,
            CanInteractWhenWanted = true,
            HasNoGarageDoors = true,
            StateID = StaticStrings.LibertyStateID,
        },
        new RepairGarage(new Vector3(6174.149f, -2720.973f, 21.85636f), 1.375183f, "Auto Back", "")
        {
            OpenTime = 0,
            CloseTime = 24,
            GarageDoors = new List<InteriorDoor>() { new InteriorDoor(812637989,new Vector3(6173.836f, -2728.155f, 23.31911f)) },
            CameraPosition = new Vector3(6175.165f, -2710.72f, 26.77394f),
            CameraDirection = new Vector3(0.001100729f, -0.9734253f, -0.229002f),
            CameraRotation = new Rotator(-13.23832f, -1.153739E-05f, -179.9352f),
            CanInteractWhenWanted = true,
            HasNoGarageDoors = false,
            StateID = StaticStrings.LibertyStateID,
        },
        new RepairGarage(new Vector3(3887.906f, -2973.136f, 10.42538f), 0.5014428f, "Axel's Pay'n'Spray", "Franchising car repairs")
        {
            OpenTime = 0,
            CloseTime = 24,
            GarageDoors = new List<InteriorDoor>() { new InteriorDoor(3489144381,new Vector3(3887.951f, -2978.066f, 11.88733f)) },
            CameraPosition = new Vector3(3884.542f, -2986.402f, 19.0602f),
            CameraDirection = new Vector3(-0.01630908f, 0.9946328f, -0.1021738f),
            CameraRotation = new Rotator(-5.86436f, 8.046237E-08f, 0.9393993f),
            CanInteractWhenWanted = true,
            HasNoGarageDoors = false,
            StateID = StaticStrings.LibertyStateID,
        },
        new RepairGarage(new Vector3(6246.36f, -3553.446f, 20.97638f), 181.632f,"Native Engines", "The best repair shop in the whole Broker")
        {
            OpenTime = 0,
            CloseTime = 24,
            GarageDoors = new List<InteriorDoor>() { new InteriorDoor(1206631488,new Vector3(6246.409f, -3546.857f, 22.31999f)) },
            CanInteractWhenWanted = true,
            HasNoGarageDoors = false,
            CameraPosition = new Vector3(6254.094f, -3538.66f, 27.27728f),
            CameraDirection = new Vector3(0.02348175f, -0.9969262f, -0.07474472f),
            CameraRotation = new Rotator(-4.286555f, 1.80598E-07f, -178.6507f),
            StateID = StaticStrings.LibertyStateID,
        },
    };
        LibertyCityLocations.RepairGarages.AddRange(RepairGarages);
    }
    private void DefaultConfig_Residences()
    {
        LibertyCityLocations.Residences.AddRange(new List<Residence>()
        {
            new Residence() {
                Name = "370 Galveston Ave",
                EntrancePosition = new Vector3(4747.902f, -1866.701f, 16.43912f),
                EntranceHeading = 87.14632f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 2000,
                PurchasePrice = 800000,
                SalesPrice = 450000,
                InteriorID = 1538,
                IsWalkup = true,
                DisableInteractAfterPurchase = true,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "1666 Valdez St",
                EntrancePosition = new Vector3(5861.33f, -1516.513f, 37.76231f),
                EntranceHeading = 181.9694f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 1300,
                PurchasePrice = 260000,
                SalesPrice = 160000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "Northern Gardens Apt 248",
                EntrancePosition = new Vector3(6255.571f, -1398.374f, 20.08655f),
                EntranceHeading = 175.4034f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 1200,
                PurchasePrice = 240000,
                SalesPrice = 160000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "50 Shinnecock Ave",
                EntrancePosition = new Vector3(6511.473f, -4058.444f, 8.417623f),
                EntranceHeading = 273.2906f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 3750,
                PurchasePrice = 1750000,
                SalesPrice = 850000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "35 Iroquois Ave",
                EntrancePosition = new Vector3(6037.512f, -3528.369f, 16.97446f),
                EntranceHeading = 85.62566f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 1000,
                PurchasePrice = 180000,
                SalesPrice = 120000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "216 Stillwater Ave",
                EntrancePosition = new Vector3(6777.688f, -2688.21f, 30.79469f),
                EntranceHeading = 89.26095f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 3000,
                PurchasePrice = 600000,
                SalesPrice = 400000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "Steinway Projects Apt 98",
                EntrancePosition = new Vector3(6404.154f, -2403.216f, 36.51644f),
                EntranceHeading = 226.2889f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 1200,
                PurchasePrice = 240000,
                SalesPrice = 160000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "560 Eastwood Apartments",
                EntrancePosition = new Vector3(5685.946f, -3177.808f, 8.726898f),
                EntranceHeading = 269.1595f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 1500,
                PurchasePrice = 300000,
                SalesPrice = 200000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "3250 Bismarck Ave",
                EntrancePosition = new Vector3(5216.333f, -2130.717f, 14.66313f),
                EntranceHeading = 267.9098f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 2500,
                PurchasePrice = 500000,
                SalesPrice = 350000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "1090 Borlock Rd",
                EntrancePosition = new Vector3(5392.138f, -3721.862f, 11.45505f),
                EntranceHeading = 280.4356f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 1750,
                PurchasePrice = 350000,
                SalesPrice = 250000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "511 Feldspar St",
                EntrancePosition = new Vector3(5033.633f, -3478.844f, 13.47041f),
                EntranceHeading = 176.2509f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 1875,
                PurchasePrice = 375000,
                SalesPrice = 250000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "130 Columbus Ave Penthouse",
                EntrancePosition = new Vector3(5103.279f, -3080.148f, 15.4746f),
                EntranceHeading = 272.7282f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 5000,
                PurchasePrice = 1000000,
                SalesPrice = 750000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "32 Panhandle Rd",
                EntrancePosition = new Vector3(3981.144f, -2115.821f, 16.68839f),
                EntranceHeading =  1.562779f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 1000,
                PurchasePrice = 190000,
                SalesPrice = 120000,
                StateID = StaticStrings.AlderneyStateID,
            },
            new Residence() {
                Name = "9 Cariboo Ave",
                EntrancePosition = new Vector3(4019.516f, -1721.802f, 30.76874f),
                EntranceHeading = 179.785f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 2875,
                PurchasePrice = 575000,
                SalesPrice = 400000,
                StateID = StaticStrings.AlderneyStateID,
            },
            new Residence() {
                Name = "1396 Red Wing Ave",
                EntrancePosition = new Vector3(3189.431f, -3279.049f, 7.505039f),
                EntranceHeading = 0.05095427f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 1100,
                PurchasePrice = 220000,
                SalesPrice = 150000,
                StateID = StaticStrings.AlderneyStateID,
            },
            new Residence() {
                Name = "46 Babbage Dr",
                EntrancePosition = new Vector3(3673.502f, -2971.557f, 15.98284f),
                EntranceHeading = 271.3226f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 1900,
                PurchasePrice = 380000,
                SalesPrice = 250000,
                StateID = StaticStrings.AlderneyStateID,
            },
            new Residence() {
                Name = "Firefly Projects Apt 153",
                EntrancePosition = new Vector3(6425.175f, -3682.032f, 16.66108f),
                EntranceHeading = 213.2364f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 14,
                RentalFee = 1100,
                PurchasePrice = 220000,
                SalesPrice = 120000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "722 Savannah Ave",
                EntrancePosition = new Vector3(6571.439f, -2720.886f, 33.11459f),
                EntranceHeading = 267.291f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 14,
                RentalFee = 3100,
                PurchasePrice = 620000,
                SalesPrice = 520000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "480 Pyrite St",
                EntrancePosition = new Vector3(4703.888f, -2414.933f, 11.02365f),
                EntranceHeading = 359.853f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 14,
                RentalFee = 2100,
                PurchasePrice = 420000,
                SalesPrice = 320000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "85 Mohawk Ave",
                EntrancePosition = new Vector3(6103.617f, -3074.594f, 36.44982f),
                EntranceHeading = 267.69f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 14,
                RentalFee = 1800,
                PurchasePrice = 360000,
                SalesPrice = 260000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence(new Vector3(6201.605f, -3748.677f, 19.76769f), 96.72688f, "Hove Beach Apt 124", ""){ StateID = StaticStrings.LibertyStateID,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(6008.782f, -3558.194f, 17.29763f), 269.2437f, "Hove Beach Apt 3B", ""){ StateID = StaticStrings.LibertyStateID,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(6122.254f, -3591.789f, 19.4756f), 88.91209f, "Hove Beach Apt 23", ""){ StateID = StaticStrings.LibertyStateID,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(6036.525f, -3521.544f, 16.91993f), 90.02948f, "Hove Beach Apt 151", ""){ StateID = StaticStrings.LibertyStateID, OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},

            new Residence(new Vector3(5266.339f, -2960.947f, 14.84979f), 358.0545f, "Prosperous Towers Apt 13", ""){ StateID = StaticStrings.LibertyStateID, InteriorID = -1,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(4746.68f, -2839.811f, 10.16748f), 85.4495f, "Galveston Ave Apt 3", ""){ StateID = StaticStrings.LibertyStateID, InteriorID = -1,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(4901.803f, -3467.72f, 14.722f), 88.14339f, "47-49 Back Passage Apt 5", ""){ StateID = StaticStrings.LibertyStateID, InteriorID = -1,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(5289.939f, -2419.559f, 15.82657f), 356.2834f, "Albany Ave Apt 1", ""){ StateID = StaticStrings.LibertyStateID, InteriorID = -1,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(5290.13f, -2401.701f, 15.8276f), 177.875f, "Albany Ave Apt 2", ""){ StateID = StaticStrings.LibertyStateID, InteriorID = -1,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(5421.174f, -3270.733f, 15.32201f), 184.5214f, "67 Albany Ave Apt 9", ""){ StateID = StaticStrings.LibertyStateID, InteriorID = -1,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(5346.969f, -3253.147f, 15.38562f), 96.5175f, "69 Albany Ave Apt 24", ""){ StateID = StaticStrings.LibertyStateID, InteriorID = -1,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(5354.673f, -3342.424f, 15.38705f), 88.54282f, "71 Albany Ave Apt 16", ""){ StateID = StaticStrings.LibertyStateID, InteriorID = -1,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(5430.704f, -3356.781f, 15.38482f), 180.9357f, "73 Albany Ave Apt 16", ""){ StateID = StaticStrings.LibertyStateID, InteriorID = -1,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(4764.228f, -2308.616f, 12.75781f), 180.8441f, "Quartz Street Apt 5", ""){ StateID = StaticStrings.LibertyStateID, InteriorID = -1,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(4687.349f, -1730.519f, 18.86129f), 250.2728f, "82 Galveston Drive Apt 12", ""){ StateID = StaticStrings.LibertyStateID, InteriorID = -1,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(4681.192f, -1792.288f, 18.86143f), 1.733164f, "84 Galveston Drive Apt 14", ""){ StateID = StaticStrings.LibertyStateID, InteriorID = -1,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(4842.686f, -3159.248f, 14.81477f), 89.41557f, "15 Frankfort Apt 32", ""){ StateID = StaticStrings.LibertyStateID, InteriorID = -1,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(4804.542f, -2585.953f, 15.58589f), 271.0529f, "Mammon Heights Apt 42", ""){ StateID = StaticStrings.LibertyStateID, InteriorID = -1,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(4805.877f, -2503.992f, 14.70955f), 274.091f, "Mayfair Towers Apt 16", ""){ StateID = StaticStrings.LibertyStateID, InteriorID = -1,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},

            //interior residences
            new Residence(new Vector3(4760.573f, -1784.506f, 20.16148f), 269.3358f, "Playboy X Penthouse", "Xenotime Street") { StateID = StaticStrings.LibertyStateID, InteriorID = 152578,OpenTime = 0,CloseTime = 24, PurchasePrice = 1275000,SalesPrice = 580000, RentalDays = 28, RentalFee = 10000,

                CameraPosition = new Vector3(4764.73f, -1782.586f, 21.10646f),
                CameraDirection = new Vector3(0.7119601f, -0.681676f, -0.1686142f),
                CameraRotation = new Rotator(-9.707253f, 8.661754E-07f, -133.7551f)
                },
            new Residence(new Vector3(5285.526f, -2415.472f, 15.78899f), 356.4554f, "Albany Studio Apartment", "Albany Avenue") { StateID = StaticStrings.LibertyStateID, InteriorID = 142850,OpenTime = 0,CloseTime = 24, PurchasePrice = 1075000,SalesPrice = 480000, RentalDays = 28, RentalFee = 10000,

                CameraPosition = new Vector3(5290.346f, -2412.433f, 16.75525f),
                CameraDirection = new Vector3(-0.7060308f, -0.704756f, -0.06956572f),
                CameraRotation = new Rotator(-3.989044f, -1.711694E-06f, 134.9482f)
                },
            new Residence(new Vector3(5981.187f, -3129.01f, 6.038285f), 318.4304f, "Broker Studio Apartment", "Mohanet Avenue") { StateID = StaticStrings.LibertyStateID, InteriorID = 113154,OpenTime = 0,CloseTime = 24, PurchasePrice = 1100000,SalesPrice = 480000, RentalDays = 28, RentalFee = 10000,

                CameraPosition = new Vector3(5970.087f, -3129.94f, 38.61262f),
                CameraDirection = new Vector3(0.6485046f, 0.639755f, -0.4124989f),
                CameraRotation = new Rotator(-24.36191f, 1.218394E-05f, -45.38913f)
                },

            new Residence(new Vector3(5158.848f, -2463.702f, 14.95425f), 84.9242f, "Middle Park Penthouse", "Columbus Avenue") { StateID = StaticStrings.LibertyStateID, InteriorID = 100610,OpenTime = 0,CloseTime = 24, PurchasePrice = 2275000,SalesPrice = 1100000, RentalDays = 28, RentalFee = 15000,

                CameraPosition = new Vector3(5149.497f, -2461.782f, 17.28627f),
                CameraDirection = new Vector3(0.9464633f, -0.2554365f, -0.1973814f),
                CameraRotation = new Rotator(-11.38387f, 1.676497E-05f, -105.1034f)
                },
            new Residence(new Vector3(5049.03f, -1872.895f, 20.78432f), 94.10531f, "8 Denver Avenue", "") { StateID = StaticStrings.LibertyStateID, InteriorID = 216578,OpenTime = 0,CloseTime = 24, PurchasePrice = 65000,SalesPrice = 30000, RentalDays = 28, RentalFee = 1570,

                CameraPosition = new Vector3(5044.43f, -1873.151f, 34.99313f),
                CameraDirection = new Vector3(0.9213463f, -0.2853073f, -0.264047f),
                CameraRotation = new Rotator(-15.31033f, 4.425946E-07f, -107.2058f)
                },

            new Residence(new Vector3(5784.468f, -1855.205f, 11.17343f), 91.03838f, "South Bohan Apartment", "") {  DisableInteractAfterPurchase = true, IsWalkup = true, StateID = StaticStrings.LibertyStateID, InteriorID = 39170,OpenTime = 0,CloseTime = 24, PurchasePrice = 175000,SalesPrice = 80000, RentalDays = 28, RentalFee = 4270  },
            new Residence(new Vector3(6084.9f, -3761.3f, 15.19304f), 272.1486f, "Broker Apartment", "") {  DisableInteractAfterPurchase = true, IsWalkup = true, StateID = StaticStrings.LibertyStateID, InteriorID = 46850,OpenTime = 0,CloseTime = 24, PurchasePrice = 175000,SalesPrice = 80000, RentalDays = 28, RentalFee = 4270  },
            new Residence(new Vector3(4225.104f, -2362.194f, 13.80571f), 2.633898f, "Alderney Apartment", "") {  DisableInteractAfterPurchase = true, IsWalkup = true, StateID = StaticStrings.AlderneyStateID, InteriorID = 111874,OpenTime = 0,CloseTime = 24, PurchasePrice = 275000,SalesPrice = 120000, RentalDays = 28, RentalFee = 4270  },
            new Residence(new Vector3(4660.642f, -1994.996f, 17.47787f), 260.9493f, "Apartment 204", "Westminister Towers") { StateID = StaticStrings.LibertyStateID, InteriorID = 38402,OpenTime = 0,CloseTime = 24, PurchasePrice = 875000,SalesPrice = 380000, RentalDays = 28, RentalFee = 8570,

                CameraPosition = new Vector3(4709.918f, -1997.484f, 18.79484f),
                CameraDirection = new Vector3(-0.9945877f, -0.02653906f, 0.1004535f),
                CameraRotation = new Rotator(5.765284f, 2.815687E-07f, 91.52849f)
                },
            new Residence(new Vector3(6571.691f, -2726.371f, 33.11459f), 273.7906f, "720 Savannah Ave", "Rundown House") { DisableInteractAfterPurchase = true, IsWalkup = true, StateID = StaticStrings.LibertyStateID, InteriorID = 133890,OpenTime = 0,CloseTime = 24, PurchasePrice = 575000,SalesPrice = 260000, RentalDays = 28, RentalFee = 3570,

                CameraPosition = new Vector3(6578.621f, -2733.208f, 37.67945f),
                CameraDirection = new Vector3(-0.8461401f, 0.4780189f, -0.2356794f),
                CameraRotation = new Rotator(-13.63168f, -6.149646E-06f, 60.53613f)
                },
            new Residence(new Vector3(3941.703f, -1717.08f, 25.93528f), 179.4532f, "15 Cariboo Ave", "") { DisableInteractAfterPurchase = true, IsWalkup = true, StateID = StaticStrings.LibertyStateID, InteriorID = 113666,OpenTime = 0,CloseTime = 24, PurchasePrice = 875000,SalesPrice = 460000, RentalDays = 28, RentalFee = 3570,

                CameraPosition = new Vector3(3942.109f, -1729.522f, 31.4396f),
                CameraDirection = new Vector3(-0.07080722f, 0.9743345f, -0.2136785f),
                CameraRotation = new Rotator(-12.33801f, -3.823567E-07f, 4.156514f)
                },
            new Residence(new Vector3(3235.01f, -3316.938f, 7.174203f), 4.576244f, "13 Emery Street", "") {  DisableInteractAfterPurchase = true, IsWalkup = true, StateID = StaticStrings.LibertyStateID, InteriorID = 107522,OpenTime = 0,CloseTime = 24, PurchasePrice = 575000,SalesPrice = 240000, RentalDays = 28, RentalFee = 3570,

                CameraPosition = new Vector3(3248.532f, -3341.053f, 14.18333f),
                CameraDirection = new Vector3(-0.6948197f, 0.6153328f, -0.3722783f),
                CameraRotation = new Rotator(-21.85619f, 1.011884E-05f, 48.47187f)
                },

            // ApartmentBuildings - Governor Greg Johnson Projects
            new Residence(new Vector3(5061.92f, -1755.482f, 22.77748f), 316.9951f, "GGJ Projects B2, Apt 1a","")
            {
                InteriorID = 19202,
                StateID = StaticStrings.LibertyStateID,
                OpenTime = 0,
                CloseTime = 24,
                RentalFee = 1200,
                RentalDays = 28,
                PurchasePrice = 260000,
                SalesPrice = 160000,
                IsWalkup = true,
                DisableInteractAfterPurchase = true,
                HasHeaderApartmentBuilding = true,
                ResidenceID = 1
            },
            new Residence(new Vector3(5066.864f, -1744.737f, 22.8021f), 225.988f, "GGJ Projects B2, Apt 1b","")
            {
                InteriorID = 71426,
                OpenTime = 0,
                CloseTime = 24,
                RentalFee = 1200,
                RentalDays = 28,
                PurchasePrice = 260000,
                SalesPrice = 160000,
                IsWalkup = true,
                DisableInteractAfterPurchase = true,
                HasHeaderApartmentBuilding = true,
                ResidenceID = 2
            },
            new Residence(new Vector3(5097.293f, -1787.802f, 22.77364f), 227.7625f, "GGJ Projects B3, Apt 1a","")
            {
                InteriorID = 140290,
                StateID = StaticStrings.LibertyStateID,
                OpenTime = 0,
                CloseTime = 24,
                RentalFee = 1200,
                RentalDays = 28,
                PurchasePrice = 260000,
                SalesPrice = 160000,
                IsWalkup = true,
                DisableInteractAfterPurchase = true,
                HasHeaderApartmentBuilding = true,
                ResidenceID = 3
            },
            new Residence(new Vector3(5108.095f, -1792.676f, 22.79658f), 135.2123f, "GGJ Projects B3, Apt 1b","")
            {
                InteriorID = 90114,
                StateID = StaticStrings.LibertyStateID,
                OpenTime = 0,
                CloseTime = 24,
                RentalFee = 1200,
                RentalDays = 28,
                PurchasePrice = 260000,
                SalesPrice = 160000,
                IsWalkup = true,
                DisableInteractAfterPurchase = true,
                HasHeaderApartmentBuilding = true,
                ResidenceID = 4
            },
            new Residence(new Vector3(5109.609f, -1800.067f, 22.77368f), 46.51126f, "GGJ Projects B3, Apt 1c","")
            {
                InteriorID = 62210,
                StateID = StaticStrings.LibertyStateID,
                OpenTime = 0,
                CloseTime = 24,
                RentalFee = 1200,
                RentalDays = 28,
                PurchasePrice = 260000,
                SalesPrice = 160000,
                IsWalkup = true,
                DisableInteractAfterPurchase = true,
                HasHeaderApartmentBuilding = true,
                ResidenceID = 5
            },
        });
        LibertyCityLocations.ApartmentBuildings.AddRange(new List<ApartmentBuilding>()
        {
            new ApartmentBuilding(new Vector3(5067.693f, -1756.976f, 19.02137f), 223.7912f, "Gov G Johnson Projects Tower 2","")
                {
                ResidenceIDs = new List<int>() { 1, 2 },
                OpenTime = 0,CloseTime = 24,
                MapIcon = 476,
                StateID = StaticStrings.LibertyStateID,
                CameraPosition = new Vector3(5076.389f, -1808.072f, 19.84389f),
                CameraDirection = new Vector3(-0.009362662f, 0.9988467f, -0.04709176f),
                CameraRotation = new Rotator(-2.699157f, -9.223317E-06f, 0.5370447f)
                },
            new ApartmentBuilding(new Vector3(5095.904f, -1793.49f, 19.0215f), 134.8743f, "Gov G Johnson Projects Tower 3","")
                {
                ResidenceIDs = new List<int>() { 3, 4, 5 },
                OpenTime = 0,CloseTime = 24,
                MapIcon = 476,
                StateID = StaticStrings.LibertyStateID,
                CameraPosition = new Vector3(5076.389f, -1808.072f, 19.84389f),
                CameraDirection = new Vector3(-0.009362662f, 0.9988467f, -0.04709176f),
                CameraRotation = new Rotator(-2.699157f, -9.223317E-06f, 0.5370447f)
                },
        });
    }
    private void DefaultConfig_Restaurants()
    {
        LibertyCityLocations.Restaurants.AddRange(new List<Restaurant>()
        {
            // SuperStar Cafes - Coffee Shops
            new Restaurant() {
                MenuID = "BeanMachineMenu",
                Name = "Superstar Cafe",
                Description = "The hotspot to not get shit done",
                EntrancePosition = new Vector3(4953.137f, -3216.419f, 14.70763f),
                EntranceHeading = 182.5473f,
                OpenTime = 6,
                CloseTime = 20,
                TypeName = "Coffee Shop",
                InteriorID = 71682,
                IsWalkup = true,
                ExtaVendorSpawnPercentage = 75,
                VendorLocations = new List<SpawnPlace>()
                {
                    new SpawnPlace(new Vector3(4951.496f, -3204.496f, 13.70811f), 177.54f),
                    new SpawnPlace(new Vector3(4954.721f, -3204.496f, 13.70811f), 177.54f)
                },
                StateID = StaticStrings.LibertyStateID,
                ActivateCells= 3,
                ActivateDistance = 75f,
            },
            new Restaurant() {
                MenuID = "BeanMachineMenu",
                Name = "Superstar Cafe",
                Description = "The hotspot to not get shit done",
                EntrancePosition = new Vector3(5215.837f, -2275.944f, 14.66181f),
                EntranceHeading = 265.13f,
                OpenTime = 6,
                CloseTime = 20,
                TypeName = "Coffee Shop",
                InteriorID= 133890,
                IsWalkup = true,
                ExtaVendorSpawnPercentage = 75,
                VendorLocations = new List<SpawnPlace>()
                {
                    new SpawnPlace(new Vector3(5204.04f, -2277.699f, 13.64895f), 270f),
                    new SpawnPlace(new Vector3(5204.04f, -2274.307f, 13.64895f), 270f)
                },
                StateID = StaticStrings.LibertyStateID,
                ActivateCells= 3,
                ActivateDistance = 75f,
            },
            // Bean Machines - Coffee Shops
            new Restaurant() {
                MenuID = "BeanMachineMenu",
                BannerImagePath = "stores\\beanmachine.png",
                Name = "Bean Machine Coffee",
                Description = "Taking over the world one Gunkaccino at a time",
                EntrancePosition = new Vector3(6226.387f, -3130.024f, 32.89014f),
                EntranceHeading = 182.21f,
                OpenTime = 0,
                CloseTime = 24,
                TypeName = "Coffee Shop",
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "BeanMachineMenu",
                BannerImagePath = "stores\\beanmachine.png",
                Name = "Bean Machine Coffee",
                Description = "Taking over the world one Gunkaccino at a time",
                EntrancePosition = new Vector3(6982.557f, -2625.294f, 28.99623f),
                EntranceHeading = 175.34f,
                OpenTime = 0,
                CloseTime = 24,
                TypeName = "Coffee Shop",
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "BeanMachineMenu",
                BannerImagePath = "stores\\beanmachine.png",
                Name = "Bean Machine Coffee",
                Description = "Taking over the world one Gunkaccino at a time",
                EntrancePosition = new Vector3(5299.14f, -3214.494f, 14.81693f),
                EntranceHeading = 266.82f,
                OpenTime = 0,
                CloseTime = 24,
                TypeName = "Coffee Shop",
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "BeanMachineMenu",
                BannerImagePath = "stores\\beanmachine.png",
                Name = "Bean Machine Coffee",
                Description = "Taking over the world one Gunkaccino at a time",
                EntrancePosition = new Vector3(5068.127f, -2768.964f, 8.720499f),
                EntranceHeading = 177.67f,
                OpenTime = 0,
                CloseTime = 24,
                TypeName = "Coffee Shop",
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "BeanMachineMenu",
                BannerImagePath = "stores\\beanmachine.png",
                Name = "Bean Machine Coffee",
                Description = "Taking over the world one Gunkaccino at a time",
                EntrancePosition = new Vector3(5064.557f, -3214.134f, 14.81133f),
                EntranceHeading = 89.48f,
                OpenTime = 0,
                CloseTime = 24,
                TypeName = "Coffee Shop",
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "CoffeeMenu",
                Name = "Craigs Coffee Shop",
                Description = "Pit-stop after the highway, or before",
                EntrancePosition = new Vector3(3972.257f, -2464.724f, 19.78755f),
                EntranceHeading = 177.38f,
                OpenTime = 6,
                CloseTime = 20,
                TypeName = "Coffee Shop",
                StateID = StaticStrings.AlderneyStateID,
            },
            // Restaurants
            new Restaurant() {
                MenuID = "FancyGenericMenu",
                Name = "69th Street Diner",
                Description = "Who could resist its greasy allure?",
                EntrancePosition = new Vector3(6072.497f, -3737.754f, 15.8818f),
                EntranceHeading = 7.14f,
                OpenTime = 6,
                CloseTime = 20,
                InteriorID = 101634,
                IsWalkup = true,
                ExtaVendorSpawnPercentage = 0f,
                VendorLocations = new List<SpawnPlace>() {new SpawnPlace(new Vector3(6073.145f, -3743.317f, 15.88293f), 0.05432305f) },
                StateID = StaticStrings.LibertyStateID,
                ActivateCells= 3,
                ActivateDistance = 50f,
            },
            new Restaurant() {
                MenuID = "FancyGenericMenu",
                Name = "Creek St Diner",
                Description = "Even greasier than the one at 69th",
                EntrancePosition = new Vector3(6134.772f, -2812.64f, 16.03048f),
                EntranceHeading = 348.6f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "WigwamMenu",
                Name = "Wigwam",
                BannerImagePath = "stores\\wigwam.png",
                Description = "No need for reservations",
                EntrancePosition = new Vector3(3622.387f, -2672.399f, 25.44182f),
                EntranceHeading = 2.22f,
                OpenTime = 6,
                CloseTime = 20,

                StateID = StaticStrings.AlderneyStateID,
            },

            new Restaurant() {
                MenuID = "FancyGenericMenu",
                Name = "60 Diner",
                Description = "Serving quality food since 1960",
                EntrancePosition = new Vector3(5301.016f, -2615.294f, 14.84125f),
                EntranceHeading = 268.24f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "FancyGenericMenu",
                Name = "60 Diner",
                Description = "Serving quality food since 1960",
                EntrancePosition = new Vector3(4798.867f, -3012.869f, 15.34717f),
                EntranceHeading = 356.24f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "PizzaThisMenu",
                Name = "Pizza This...",
                Description = "Get stuffed",
                BannerImagePath = "stores\\pizzathis.png",
                EntrancePosition = new Vector3(4031.224f, -1853.784f, 24.46054f),
                EntranceHeading = 181.396f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.AlderneyStateID,
            },
            new Restaurant() {
                MenuID = "PizzaThisMenu",
                Name = "Pizza This...",
                Description = "Get stuffed",
                BannerImagePath = "stores\\pizzathis.png",
                EntrancePosition = new Vector3(7009.417f, -2670.004f, 29.0778f),
                EntranceHeading = 86.71f,
                OpenTime = 9,
                CloseTime = 23,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "AlDentesMenu",
                Name = "Al Dente's",
                Description = "Just like mama never used to make it",
                BannerImagePath = "stores\\aldentes.png",
                EntrancePosition = new Vector3(7034.287f, -2564.284f, 26.66244f),
                EntranceHeading = 86.71f,
                OpenTime = 9,
                CloseTime = 23,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "AlDentesMenu",
                Name = "Al Dente's",
                Description = "Just like mama never used to make it",
                BannerImagePath = "stores\\aldentes.png",
                EntrancePosition = new Vector3(5117.05f, -3887.147f, 14.76031f),
                EntranceHeading = 219.37f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "PizzaMenu",
                Name = "Colony Pizza",
                Description = "Colony Island's finest pizza",
                EntrancePosition = new Vector3(5617.337f, -3160.814f, 8.728373f),
                EntranceHeading = 88.96f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "PizzaMenu",
                Name = "Drusilla's",
                Description = "Traditionally italian",
                EntrancePosition = new Vector3(5067.957f, -3512.794f, 12.68762f),
                EntranceHeading = 89.29f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "NoodleMenu",
                Name = "Mr. Fuk's Rice Box",
                Description = "Quality korean food by Mr. Fuk himself",
                EntrancePosition = new Vector3(3946.517f, -2183.744f, 19.78059f),
                EntranceHeading = 137.64f,
                OpenTime = 6,
                CloseTime = 20,
                ExtaVendorSpawnPercentage = 0f,
                VendorLocations = new List<SpawnPlace>() {new SpawnPlace(new Vector3(3948.189f, -2168.545f, 19.78026f), 177.5162f) },
                StateID = StaticStrings.AlderneyStateID,
                ActivateCells= 3,
                ActivateDistance = 75f,
            },
            new Restaurant() {
                MenuID = "FancyFishMenu",
                Name = "Fanny Crab's",
                Description = "You'll love the taste of our Fanny Crabs",
                EntrancePosition = new Vector3(5157.417f, -3321.344f, 15.20218f),
                EntranceHeading = 88.77f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },


            new Restaurant() {
                MenuID = "NoodleMenu",
                Name = "Delicious Chinese Food",
                Description = "Speaks for itself",
                EntrancePosition = new Vector3(6311.217f, -2588.364f, 38.19183f),
                EntranceHeading = 88.88f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "BeefyBillsMenu",
                Name = "Meat Factory",
                Description = "Put our meat in your mouth",
                EntrancePosition = new Vector3(4722.013f, -1895.584f, 17.47035f),
                EntranceHeading = 268.31f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "FancyGenericMenu",
                Name = "Port Tudor Diner",
                Description = "A gritty Port diner",
                EntrancePosition = new Vector3(3861.672f, -3017.276f, 10.3819f),
                EntranceHeading = 359.6497f,
                OpenTime = 6,
                CloseTime = 20,

                StateID = StaticStrings.AlderneyStateID,
            },
            new Restaurant() {
                MenuID = "FancyGenericMenu",
                Name = "Skyway Diner",
                Description = "A roadside diner under the Skyway",
                EntrancePosition = new Vector3(3707.284f, -3310.105f, 7.558681f),
                EntranceHeading = 44.23306f,
                OpenTime = 6,
                CloseTime = 20,

                StateID = StaticStrings.AlderneyStateID,
            },
            new Restaurant() {
                MenuID = "FancyGenericMenu",
                Name = "Greasy Joe's Cafe",
                Description = "greasy comfort food to anyone brave enough to try it",
                EntrancePosition = new Vector3(3889.723f, -2227.158f, 19.57111f),
                EntranceHeading = 1.286601f,
                OpenTime = 6,
                CloseTime = 20,

                StateID = StaticStrings.AlderneyStateID,
            },
            //Burger
            new Restaurant() {
                MenuID = "BurgerShotMenu",
                Name = "Burger Shot",
                Description = "Kill your hunger! It's bleedin' tasty",
                EntrancePosition = new Vector3(6841.368f, -3030.382f, 25.21253f),
                EntranceHeading = 271.4003f,
                BannerImagePath = "stores\\burgershot.png",
                OpenTime = 6,
                CloseTime = 22,
                InteriorID = 126722,
                VendorPersonnelID = "BurgerShotPeds",
                VendorHeadDataGroupID = "AllHeads",
                ExtaVendorSpawnPercentage = 75f,
                VendorLocations = new List<SpawnPlace>()
                {
                    new SpawnPlace(new Vector3(6826.013f, -3029.573f, 23.21199f), 266.86f),
                    new SpawnPlace(new Vector3(6825.861f, -3032.349f, 23.21199f), 266.86f),
                },
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new MerchantConditionalLocation(new Vector3(6844.336f, -3032.349f, 22.9823f), 1.86f, 75f) { TaskRequirements = TaskRequirements.AnyScenario | TaskRequirements.Guard, MinHourSpawn = 6,MaxHourSpawn = 22 },
                },
                StateID = StaticStrings.LibertyStateID,
                ActivateCells= 3,
                ActivateDistance = 75f,
            },
            new Restaurant() {
                MenuID = "BurgerShotMenu",
                Name = "Burger Shot",
                Description = "Kill your hunger! It's bleedin' tasty",
                EntrancePosition = new Vector3(5630.5f, -1740.112f, 16.27698f),
                EntranceHeading = 23.34f,
                BannerImagePath = "stores\\burgershot.png",
                OpenTime = 6,
                CloseTime = 22,
                InteriorID = 156674,
                IsWalkup = true,
                VendorPersonnelID = "BurgerShotPeds",
                VendorHeadDataGroupID = "AllHeads",
                ExtaVendorSpawnPercentage = 75f,
                VendorLocations = new List<SpawnPlace>()
                {
                    new SpawnPlace(new Vector3(5639.249f, -1752.694f, 14.3157f), 31.23f),
                    new SpawnPlace(new Vector3(5641.179f, -1751.524f, 14.3157f), 31.23f)
                },
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new MerchantConditionalLocation(new Vector3(5634.027f, -1754.147f, 14.3157f), 31.23f, 75f) { TaskRequirements = TaskRequirements.AnyScenario | TaskRequirements.Guard, MinHourSpawn = 6,MaxHourSpawn = 22  },
                    new MerchantConditionalLocation(new Vector3(5628.905f, -1740.021f, 14.3494f), 123.32f, 75f) { TaskRequirements = TaskRequirements.AnyScenario | TaskRequirements.Guard, MinHourSpawn = 6,MaxHourSpawn = 22 },
                },
                StateID = StaticStrings.LibertyStateID,
                ActivateCells= 3,
                ActivateDistance = 75f,
            },
            new Restaurant() {
                MenuID = "BurgerShotMenu",
                Name = "Burger Shot",
                Description = "Kill your hunger! It's bleedin' tasty",
                EntrancePosition = new Vector3(5002.397f, -2968.844f, 14.7723f),
                EntranceHeading = 88.05f,
                OpenTime = 6,
                CloseTime = 22,
                InteriorID = 112642,
                IsWalkup = true,
                BannerImagePath = "stores\\burgershot.png",
                VendorPersonnelID = "BurgerShotPeds",
                VendorHeadDataGroupID = "AllHeads",
                ExtaVendorSpawnPercentage = 0f,
                VendorLocations = new List<SpawnPlace>()
                {
                    new SpawnPlace(new Vector3(5017.363f, -2967.939f, 12.82008f), 87.54f),
                },
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new MerchantConditionalLocation(new Vector3(5000.951f, -2971.517f, 12.77567f), 7.54f, 75f) { TaskRequirements = TaskRequirements.AnyScenario | TaskRequirements.Guard, MinHourSpawn = 6,MaxHourSpawn = 22  },
                },
                StateID = StaticStrings.LibertyStateID,
                ActivateCells= 3,
                ActivateDistance = 75f,
            },
            new Restaurant() {
                MenuID = "BurgerShotMenu",
                Name = "Burger Shot",
                Description = "Kill your hunger! It's bleedin' tasty",
                EntrancePosition = new Vector3(4746.547f, -2062.274f, 13.04971f),
                EntranceHeading = 95.42f,
                OpenTime = 6,
                CloseTime = 22,
                InteriorID = 109570,
                IsWalkup = true,
                BannerImagePath = "stores\\burgershot.png",
                VendorPersonnelID = "BurgerShotPeds",
                VendorHeadDataGroupID = "AllHeads",
                ExtaVendorSpawnPercentage = 0f,
                VendorLocations = new List<SpawnPlace>()
                {
                    new SpawnPlace(new Vector3(4761.273f, -2060.571f, 11.04702f), 94.60755f)
                },
                StateID = StaticStrings.LibertyStateID,
                ActivateCells= 3,
                ActivateDistance = 75f,
            },
            new Restaurant() {
                MenuID = "BurgerShotMenu",
                Name = "Burger Shot",
                Description = "Kill your hunger! It's bleedin' tasty",
                EntrancePosition = new Vector3(6288.527f, -1661.284f, 16.89896f),
                EntranceHeading = 40.91f,
                OpenTime = 6,
                CloseTime = 22,
                InteriorID = 134402,
                IsWalkup = true,
                BannerImagePath = "stores\\burgershot.png",
                VendorPersonnelID = "BurgerShotPeds",
                VendorHeadDataGroupID = "AllHeads",
                ExtaVendorSpawnPercentage = 75f,
                VendorLocations = new List<SpawnPlace>()
                {
                    new SpawnPlace(new Vector3(6300.729f, -1671.217f, 14.90752f), 47.83f),
                    new SpawnPlace(new Vector3(6298.64f, -1673.277f, 14.90752f), 47.83f)
                },
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new MerchantConditionalLocation(new Vector3(6296.493f, -1674.306f, 14.90752f), 47.83f, 75f) { TaskRequirements = TaskRequirements.AnyScenario | TaskRequirements.Guard, MinHourSpawn = 6,MaxHourSpawn = 22 },
                },
                StateID = StaticStrings.LibertyStateID,
                ActivateCells= 3,
                ActivateDistance = 75f,
            },
            new Restaurant() {
                MenuID = "BurgerShotMenu",
                Name = "Burger Shot",
                Description = "Kill your hunger! It's bleedin' tasty",
                EntrancePosition = new Vector3(4570.437f, -3109.028f, 4.811677f),
                EntranceHeading = 358.48f,
                OpenTime = 6,
                CloseTime = 22,
                InteriorID = 105986,
                IsWalkup = true,
                BannerImagePath = "stores\\burgershot.png",
                VendorPersonnelID = "BurgerShotPeds",
                VendorHeadDataGroupID = "AllHeads",
                ExtaVendorSpawnPercentage = 0f,
                VendorLocations = new List<SpawnPlace>() {new SpawnPlace(new Vector3(4571.398f, -3123.869f, 2.811164f), 358.8464f) },
                StateID = StaticStrings.LibertyStateID,
                ActivateCells= 3,
                ActivateDistance = 75f,
            },
            new Restaurant() {
                MenuID = "BurgerShotMenu",
                Name = "Burger Shot",
                Description = "Kill your hunger! It's bleedin' tasty",
                EntrancePosition = new Vector3(4182.229f, -1643.417f, 24.01295f),
                EntranceHeading = 180.48f,
                OpenTime = 6,
                CloseTime = 22,
                InteriorID = 59650,
                IsWalkup = true,
                BannerImagePath = "stores\\burgershot.png",
                VendorPersonnelID = "BurgerShotPeds",
                VendorHeadDataGroupID = "AllHeads",
                ExtaVendorSpawnPercentage = 0f,
                VendorLocations = new List<SpawnPlace>() {new SpawnPlace(new Vector3(4180.869f, -1628.156f, 22.314f), 180.7f) },

                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new MerchantConditionalLocation(new Vector3(4190.209f, -1644.44922f, 22.1626339f), 209.8278f, 75f) { TaskRequirements = TaskRequirements.AnyScenario | TaskRequirements.Guard ,MinHourSpawn = 6,MaxHourSpawn = 22},
                    new MerchantConditionalLocation(new Vector3(4177.42725f, -1644.34912f, 22.1626339f), 187.217f, 75f){ TaskRequirements = TaskRequirements.AnyScenario | TaskRequirements.Guard ,MinHourSpawn = 6,MaxHourSpawn = 22},
                },
                StateID = StaticStrings.AlderneyStateID,
                ActivateCells= 3,
                ActivateDistance = 75f,
            },
            //Burgershot Bowling
            new Restaurant() {
                MenuID = "BurgerShotMenu",
                Name = "Burger Shot",
                Description = "Kill your hunger! It's bleedin' tasty",
                EntrancePosition = new Vector3(4590.571f, -3175.661f, 5.217072f),
                EntranceHeading = 269.3177f,
                OpenTime = 6,
                CloseTime = 20,
                InteriorID = 92162,
                IsWalkup = true,
                BannerImagePath = "stores\\burgershot.png",
                VendorPersonnelID = "BurgerShotPeds",
                VendorHeadDataGroupID = "AllHeads",
                ExtaVendorSpawnPercentage = 0f,
                VendorLocations = new List<SpawnPlace>() {new SpawnPlace(new Vector3(4590.571f, -3175.661f, 5.217072f), 250.1382f) },
                StateID = StaticStrings.LibertyStateID,
                ActivateCells= 3,
                ActivateDistance = 75f,
            }, //Golden Pier Bowling
            new Restaurant() {
                MenuID = "BurgerShotMenu",
                Name = "Burger Shot",
                Description = "Kill your hunger! It's bleedin' tasty",
                EntrancePosition = new Vector3(6389.916f, -3910.461f, 16.84486f),
                EntranceHeading = 179.2322f,
                OpenTime = 6,
                CloseTime = 20,
                InteriorID = 38146,
                IsWalkup = true,
                BannerImagePath = "stores\\burgershot.png",
                VendorPersonnelID = "BurgerShotPeds",
                VendorHeadDataGroupID = "AllHeads",
                ExtaVendorSpawnPercentage = 0f,
                VendorLocations = new List<SpawnPlace>() {new SpawnPlace(new Vector3(6389.916f, -3910.461f, 16.84486f), 155.9372f) },
                StateID = StaticStrings.LibertyStateID,
                ActivateCells= 3,
                ActivateDistance = 75f,
            }, // Firefly Bowling


            //Chicken
            new Restaurant(new Vector3(3679.903f, -2688.659f, 19.5674f), 270.0156f, "Cluckin' Bell", "Taste the cock","CluckinBellMenu", FoodType.Chicken | FoodType.FastFood) {
                StateID =StaticStrings.AlderneyStateID,
                OpenTime = 6,
                CloseTime = 22,
                BannerImagePath = "stores\\cluckin.png",
            },
            new Restaurant(new Vector3(6379.456f, -2881.191f, 25.09852f), 359.5992f,"Cluckin' Bell","Taste the cock","CluckinBellMenu", FoodType.Chicken | FoodType.FastFood) {
                MenuID = "CluckinBellMenu",
                Name = "Cluckin' Bell",
                Description = "Taste the cock",
                OpenTime = 6,
                CloseTime = 22,
                InteriorID = 143874,
                IsWalkup = true,
                BannerImagePath = "stores\\cluckin.png",
                VendorPersonnelID = "CluckinBellPeds",
                VendorHeadDataGroupID = "AllHeads",
                ExtaVendorSpawnPercentage = 75f,
                VendorLocations = new List<SpawnPlace>()
                {
                    new SpawnPlace(new Vector3(6371.438f, -2896f, 23.1033f), 0.8869792f),
                    new SpawnPlace(new Vector3(6374.712f, -2896.072f, 23.10331f), 358.9843f)
                },
                StateID = StaticStrings.LibertyStateID,
                ActivateCells= 3,
                ActivateDistance = 75f,
            },
            new Restaurant(new Vector3(5050.218f, -3183.636f, 14.76417f),103.41f,"Cluckin' Bell","Taste the cock","CluckinBellMenu", FoodType.Chicken | FoodType.FastFood) {
                OpenTime = 6,
                CloseTime = 22,
                InteriorID = 124162,
                IsWalkup = true,
                BannerImagePath = "stores\\cluckin.png",
                VendorPersonnelID = "CluckinBellPeds",
                VendorHeadDataGroupID = "AllHeads",
                ExtaVendorSpawnPercentage = 75f,
                VendorLocations = new List<SpawnPlace>()
                {
                    new SpawnPlace(new Vector3(5065.712f, -3183.676f, 12.80308f), 105.0725f),
                    new SpawnPlace(new Vector3(5066.809f, -3187.47f, 12.80307f), 106.7305f)
                },
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new MerchantConditionalLocation(new Vector3(5049.251f, -3189.10864f, 12.7585821f), 162.5283f, 75f){ TaskRequirements = TaskRequirements.AnyScenario | TaskRequirements.Guard, MinHourSpawn = 6,MaxHourSpawn = 22  },
                    new MerchantConditionalLocation(new Vector3(5058.305f, -3204.62939f, 12.7823725f), 184.0074f, 75f){ TaskRequirements = TaskRequirements.AnyScenario | TaskRequirements.Guard, MinHourSpawn = 6,MaxHourSpawn = 22  },
                },
                StateID = StaticStrings.LibertyStateID,
                ActivateCells= 3,
                ActivateDistance = 150f,
            },

            //Coffee
            new Restaurant(new Vector3(3889.34f, -2369.084f, 19.56951f), 181.2315f, "Craigs Coffee Shop", "","CoffeeMenu", FoodType.Coffee){ StateID = StaticStrings.AlderneyStateID },
            new Restaurant(new Vector3(3694.515f, -2519.081f, 25.44073f), 64.09496f, "Columbian Coffee House", "","CoffeeMenu", FoodType.Coffee){ StateID = StaticStrings.AlderneyStateID },
            new Restaurant(new Vector3(6069.68f, -3812.907f, 13.73957f), 181.5454f, "Hot Coffee Shop", "Wanna come inside?","CoffeeMenu", FoodType.Coffee){ StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(5271.809f, -3294.312f, 14.83245f), 183.6429f,  "Bean Machine Coffee", "Taking over the world one Gunkaccino at a time","BeanMachineMenu", FoodType.Coffee | FoodType.Bagels | FoodType.Donut) { StateID = StaticStrings.LibertyStateID, BannerImagePath = "stores\\beanmachine.png" },
            new Restaurant(new Vector3(4964.879f, -3292.926f, 14.76437f), 222.3711f,  "Bean Machine Coffee", "Taking over the world one Gunkaccino at a time","BeanMachineMenu", FoodType.Coffee | FoodType.Bagels | FoodType.Donut) { StateID = StaticStrings.LibertyStateID, BannerImagePath = "stores\\beanmachine.png" },
            new Restaurant(new Vector3(5119.275f, -2891.845f, 14.77013f), 277.2627f,  "Bean Machine Coffee", "Taking over the world one Gunkaccino at a time","BeanMachineMenu", FoodType.Coffee | FoodType.Bagels | FoodType.Donut) { StateID = StaticStrings.LibertyStateID, BannerImagePath = "stores\\beanmachine.png" },
            new Restaurant(new Vector3(5299.137f, -3261.627f, 14.81983f), 268.7347f,  "Stews Coffee Shop", "","CoffeeMenu", FoodType.Coffee) { StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(6481.951f, -3322.629f, 27.82883f), 356.5174f,  "Benito's Coffee and Comics", "","CoffeeMenu", FoodType.Coffee) { StateID = StaticStrings.LibertyStateID },

            //Donuts
            new Restaurant(new Vector3(5386.497f, -3503.294f, 10.31388f), 5.807813f, "Rusty Brown's", "Ring lickin' good!","RustyBrownsMenu", FoodType.Bagels | FoodType.Donut){ IsWalkup = true, StateID = StaticStrings.LibertyStateID, OpenTime = 4, CloseTime = 20,BannerImagePath = "stores\\rustybrowns.png" },

            //Ice Cream
            new Restaurant(new Vector3(6069.68f, -3812.907f, 13.73209f), 181.5454f, "Ned's Ice Cream", "","IceCreamMenu", FoodType.Dessert){ IsWalkup = true, StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(5257.97f, -3433.251f, 14.51972f), 284.153f, "Fudz's Delicatessen", "Satisfaction served daily!","IceCreamMenu", FoodType.Dessert){ IsWalkup = true, StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(6420.359f, -2505.632f, 36.11824f), 359.4598f, "Cherry Popper Ice Cream", "","IceCreamMenu", FoodType.Dessert){ StateID = StaticStrings.LibertyStateID },

            //s.HO
            new Restaurant(new Vector3(6427.569f, -2901.484f, 21.40972f), 92.50405f, "S.Ho", "Become a real S. HO","NoodleMenu", FoodType.Korean) {BannerImagePath ="stores\\sho.png", ScannerFilePath ="01_specific_location\\0x1ABB2DE0.mp3" },

            //Wigwam
            new Restaurant(new Vector3(4886.781f, -3241.666f, 14.70797f), 359.4828f, "Wigwam", "No need for reservations","WigwamMenu", FoodType.American | FoodType.Burger | FoodType.FastFood) { StateID =StaticStrings.AlderneyStateID, OpenTime = 6, CloseTime = 20, BannerImagePath = "stores\\wigwam.png", },
            
            //tw@
            new Restaurant(new Vector3(3606.053f, -2791.577f, 25.44485f), 66.50378f,"tw@","nu-media caffeine solutions provider","InternetCafeMenu",FoodType.Snack) {
                BannerImagePath = "stores\\twat.png",
                OpenTime = 6,
                CloseTime = 20,
                InteriorID = 166914,
                IsWalkup = true,
                VendorPersonnelID = "TwatPeds",
                VendorHeadDataGroupID = "AllHeads",
                ExtaVendorSpawnPercentage = 75f,
                VendorLocations = new List<SpawnPlace>()
                {
                   new SpawnPlace(new Vector3(3611.932f, -2790.614f, 23.4438f), 151.6612f),
                   new SpawnPlace(new Vector3(3609.369f, -2797.919f, 23.44383f), 337.0622f)
                },
                StateID = StaticStrings.AlderneyStateID,
                ActivateCells= 3,
                ActivateDistance = 75f,
            },
            new Restaurant(new Vector3(4848.44f, -1863.803f, 12.91685f), 89.72078f,"tw@","nu-media caffeine solutions provider","InternetCafeMenu",FoodType.Snack) {
                BannerImagePath = "stores\\twat.png",
                OpenTime = 6,
                CloseTime = 20,
                InteriorID = 66562,
                IsWalkup = true,
                VendorPersonnelID = "TwatPeds",
                VendorHeadDataGroupID = "AllHeads",
                ExtaVendorSpawnPercentage = 75f,
                VendorLocations = new List<SpawnPlace>()
                {
                    new SpawnPlace(new Vector3(4852.911f, -1860.169f, 10.91317f), 182.5274f),
                    new SpawnPlace(new Vector3(4854.048f, -1867.899f, 10.91319f), 0.8041525f)
                },
                StateID = StaticStrings.LibertyStateID,
                ActivateCells= 3,
                ActivateDistance = 75f,
            },
            new Restaurant(new Vector3(6163.752f, -3425.905f, 23.96916f), 290.0018f,"tw@","nu-media caffeine solutions provider","InternetCafeMenu",FoodType.Snack) {
                OpenTime = 6,
                CloseTime = 20,
                InteriorID = 50178,
                IsWalkup = true,
                BannerImagePath = "stores\\twat.png",
                VendorPersonnelID = "TwatPeds",
                VendorHeadDataGroupID = "AllHeads",
                ExtaVendorSpawnPercentage = 75f,
                VendorLocations = new List<SpawnPlace>()
                {
                   new SpawnPlace(new Vector3(6160.429f, -3430.661f, 22.1937f), 17.51742f),
                   new SpawnPlace(new Vector3(6157.035f, -3423.569f, 22.19372f), 196.9963f)
                },
                StateID = StaticStrings.LibertyStateID,
                ActivateCells= 3,
                ActivateDistance = 75f,
            },

            //Italian
            new Restaurant(new Vector3(3633.521f, -2751.26f, 25.90831f), 355.8445f, "Pizza This...", "Get stuffed","PizzaThisMenu", FoodType.Italian | FoodType.Pizza) { MapIcon = 889,TypeName ="Pizza", StateID = StaticStrings.AlderneyStateID ,BannerImagePath = "stores\\pizzathis.png" },
            new Restaurant(new Vector3(4844.719f, -3185.969f, 14.81251f), 129.8922f, "Al Dente's", "Just like mama never used to make it","AlDentesMenu", FoodType.Italian | FoodType.Pizza) { MapIcon = 889,TypeName ="Pizza",StateID = StaticStrings.LibertyStateID ,BannerImagePath = "stores\\aldentes.png" },
            new Restaurant(new Vector3(5340.097f, -3731.913f, 14.76148f), 90.53439f, "Al Dente's", "Just like mama never used to make it","AlDentesMenu", FoodType.Italian | FoodType.Pizza) { MapIcon = 889,TypeName ="Pizza",StateID = StaticStrings.LibertyStateID ,BannerImagePath = "stores\\aldentes.png" },
            new Restaurant(new Vector3(4788.79f, -3562.774f, 5.290552f), 143.4763f, "Pasta Le Vista", "Where every bite’s a getaway!","AlDentesMenu", FoodType.Italian | FoodType.Pizza) { MapIcon = 889,TypeName ="Pizza",StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(4773.379f, -2800.022f, 12.40089f), 359.3528f, "Pasta Le Vista", "Where every bite’s a getaway!","AlDentesMenu", FoodType.Italian | FoodType.Pizza) { MapIcon = 889,TypeName ="Pizza",StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(5316.652f, -3540.868f, 14.98561f), 268.1589f, "Pasta Le Vista", "Where every bite’s a getaway!","AlDentesMenu", FoodType.Italian | FoodType.Pizza) { MapIcon = 889,TypeName ="Pizza",StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(6622.493f, -3021.823f, 26.25504f), 92.7175f, "Al's Pizzeria", "Free Delivery","PizzaThisMenu", FoodType.Italian | FoodType.Pizza) { MapIcon = 889,TypeName ="Pizza",StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(6167.876f, -3471.708f, 23.91444f), 321.847f, "Pizza Feast", "For a Real Feast","PizzaThisMenu", FoodType.Italian | FoodType.Pizza) { MapIcon = 889,TypeName ="Pizza",StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(6985.954f, -2773.777f, 28.14703f), 275.386f, "Pizza Feast", "For a Real Feast","PizzaThisMenu", FoodType.Italian | FoodType.Pizza) { MapIcon = 889,TypeName ="Pizza",StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(6323.014f, -2853.104f, 29.79031f), 74.50282f, "Italian Restaurant", "","AlDentesMenu", FoodType.Italian | FoodType.Pizza) { MapIcon = 889,TypeName ="Pizza",StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(6364.021f, -2859.532f, 25.5423f), 178.9763f, "Pizza & Deli", "","PizzaThisMenu", FoodType.Italian | FoodType.Pizza) { MapIcon = 889,TypeName ="Pizza",StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(5542.278f, -1543.707f, 17.17674f), 84.35246f, "Pizza", "","PizzaThisMenu", FoodType.Italian | FoodType.Pizza) { MapIcon = 889,TypeName ="Pizza",StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(5729.88f, -1736.4f, 15.67086f), 0.8774939f, "Pizza & Gyro", "","PizzaThisMenu", FoodType.Italian | FoodType.Pizza) { MapIcon = 889,TypeName ="Pizza",StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(3664.921f, -2579.406f, 25.41677f), 106.7861f, "Nonna Pina's", "","AlDentesMenu", FoodType.Italian | FoodType.Pizza) { MapIcon = 889,TypeName ="Pizza",StateID = StaticStrings.AlderneyStateID },
            new Restaurant(new Vector3(4858.985f, -2724.5f, 14.75652f), 359.0387f, "Pizza", "","PizzaThisMenu", FoodType.Italian | FoodType.Pizza) { MapIcon = 889,TypeName ="Pizza",StateID = StaticStrings.LibertyStateID },

            //Chinese
            new Restaurant(new Vector3(6455.289f, -3324.406f, 28.10382f), 357.3897f, "Green Dragon Chinese Food", "","NoodleMenu", FoodType.Asian){ StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(6513.294f, -3385.142f, 25.74195f), 182.9156f, "Wok Like An Egyptian", "","NoodleMenu", FoodType.Asian){ StateID = StaticStrings.LibertyStateID },

            //Generic    
            new Restaurant(new Vector3(3877.316f, -2369.138f, 19.56862f), 181.032f, "Healthy Food", "No bogeys about it","FancyGenericMenu", FoodType.American){ StateID = StaticStrings.AlderneyStateID },
            new Restaurant(new Vector3(5057.175f, -1849.337f, 20.42709f), 0.09704411f, "S&D Diner", "","FancyGenericMenu", FoodType.Generic){ IsWalkup = true, StateID = StaticStrings.LibertyStateID, OpenTime = 6, CloseTime = 20 },
            new Restaurant(new Vector3(6166.843f, -3729.553f, 16.10103f), 273.8801f, "Gulag Garden", "Fresh from the gulag","FancyGenericMenu", FoodType.Generic){ StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(6205.877f, -3590.927f, 19.50217f), 121.6593f, "Luncheonette Diner", "Fresh from the gulag","FancyGenericMenu", FoodType.Generic){ StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(6262.65f, -3220.061f, 34.00982f), 273.2307f, "Diner", "","FancyGenericMenu", FoodType.American){ StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(6325.279f, -2844.385f, 29.83092f), 75.98f, "American & Spanish Food", "","FancyGenericMenu", FoodType.American){ StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(6347.511f, -2859.9f, 27.48295f), 181.499f, "Chicken Restaurant", "","FancyGenericMenu", FoodType.Chicken){ StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(6444.175f, -3465.883f, 25.23859f), 179.2637f, "Ali Mac's", "Traditional Familry Fare","FancyGenericMenu", FoodType.Generic){ StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(6385.285f, -3490.843f, 24.55093f), 268.1076f, "Coronary Corner Restuarant", "","FancyGenericMenu", FoodType.Generic){ StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(3824.19f, -2730.174f, 13.55818f), 5.33826f, "Diner, Bar, Cocktails", "","FancyGenericMenu", FoodType.American){ StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(5024.721f, -1897.72f, 20.41535f), 268.2972f, "Rubys Diner", "","FancyGenericMenu", FoodType.American){ StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(5175.167f, -1843.52f, 20.42544f), 175.6631f, "The Flaming Fez", "","FancyGenericMenu", FoodType.American){ StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(4914.081f, -1779.037f, 20.43114f), 267.7406f, "Spoon Up", "","FancyGenericMenu", FoodType.American){ StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(4746.434f, -2760.067f, 9.853194f), 89.15031f, "The Flaming Fez", "","FancyGenericMenu", FoodType.American){ StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(4842.253f, -2733.665f, 14.76149f), 90.12207f, "The Haitian Kitchen", "","FancyGenericMenu", FoodType.American){ StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(4842.347f, -2778.69f, 14.7784f), 91.49546f, "The Haitian Kitchen", "","FancyGenericMenu", FoodType.American){ StateID = StaticStrings.LibertyStateID },

            //Mexican
            new Restaurant(new Vector3(5980.922f, -3605.667f, 7.496796f), 183.4195f, "El Sobre", "","MexicanMenu", FoodType.Mexican){ StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(6529.486f, -3385.171f, 24.66998f), 178.3091f, "El Gringo Supremo's", "","MexicanMenu", FoodType.Mexican){ StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(4859.748f, -1980.554f, 23.21977f), 179.3529f, "Da Cruz", "","MexicanMenu", FoodType.Mexican){ StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(4746.355f, -2206.857f, 9.962692f), 92.56332f, "Da Cruz", "","MexicanMenu", FoodType.Mexican){ StateID = StaticStrings.LibertyStateID },

            //Korean
            new Restaurant(new Vector3(5193.832f, -3629.677f, 14.76479f), 90.41766f, "Rice & Fins", "New century fast food!","NoodleMenu", FoodType.Korean) { StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(5296.65f, -3652.863f, 14.7628f), 180.7549f, "Dim Sum", "","NoodleMenu", FoodType.Korean) { StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(6929.934f, -2736.758f, 29.10781f), 50.33625f, "Silk Thai", "","NoodleMenu", FoodType.Asian){ StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(6499.485f, -2825.356f, 23.40433f), 86.97023f, "Kingston Restaurant", "","NoodleMenu", FoodType.Asian){ StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(6520.362f, -2880.615f, 22.61206f), 2.052902f, "Indian Inn Restaurant", "","NoodleMenu", FoodType.Asian){ StateID = StaticStrings.LibertyStateID },

            //Sandwiches
            new Restaurant(new Vector3(6263.44f, -3173.308f, 34.20807f), 269.9303f, "Hero Shop", "","SandwichMenu", FoodType.Sandwiches){ StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(4189.276f, -1545.106f, 20.66914f), 266.9386f, "Bite!", "Have It Our Way","BiteMenu", FoodType.Sandwiches) {StateID = StaticStrings.AlderneyStateID, OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\bite.png" },
            new Restaurant(new Vector3(4280.596f, -1842.389f, 25.43676f), 90.69621f, "Bite!", "Have It Our Way","BiteMenu", FoodType.Sandwiches) {StateID = StaticStrings.AlderneyStateID, OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\bite.png" },
            new Restaurant(new Vector3(3899.524f, -1733.591f, 22.43855f), 268.5789f, "Bite!", "Have It Our Way","BiteMenu", FoodType.Sandwiches) {StateID = StaticStrings.AlderneyStateID, OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\bite.png" },
            new Restaurant(new Vector3(4088.492f, -1942.156f, 24.7234f), 353.7918f,  "Bite!", "Have It Our Way","BiteMenu", FoodType.Sandwiches) {StateID = StaticStrings.AlderneyStateID, OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\bite.png" },
            new Restaurant(new Vector3(6311.173f, -1478.079f, 10.87776f), 1.770366f, "Bite!", "Have It Our Way","BiteMenu", FoodType.Sandwiches) {StateID = StaticStrings.LibertyStateID, OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\bite.png" },

            //Seafood
            new Restaurant(new Vector3(3675.666f, -1853.703f, 13.38639f), 357.4145f, "Fanny Crabs", "Take a bite of our fanny","FancyFishMenu", FoodType.Seafood) { StateID = StaticStrings.AlderneyStateID },
            new Restaurant(new Vector3(5077.693f, -4284.778f, 5.118348f), 185.1966f, "Poop Deck", "Seafood with a splash of salty humor","FancyFishMenu", FoodType.Seafood) { StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(5237.36f, -3347.319f, 15.83374f), 91.89917f, "Squid Row", "Get hooked on our big ones!","FancyFishMenu", FoodType.Seafood) { StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(5340.312f, -3503.894f, 14.75966f), 86.89571f, "The Bearded Clam", "Dive in for a taste!","FancyFishMenu", FoodType.Seafood) { StateID = StaticStrings.LibertyStateID },
        });
    }
    private void DefaultConfig_ScrapYards()
    {
        List<ScrapYard> ScrapYardsList = new List<ScrapYard>() {
            new ScrapYard() {
                Name = "Rusty Schit Salvage",
                Description = "We'll take it from you",
                EntrancePosition = new Vector3(4717.753f, -1507.41f, 8.750113f),
                EntranceHeading = 121.5364f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
                ActivateCells = 3,
                ActivateDistance = 75,
            },
            new ScrapYard() {
                Name = "Dukes Used Auto Parts",
                Description = "We buy and sell auto junk",
                EntrancePosition = new Vector3(6233.401f, -2731.289f, 24.47015f),
                EntranceHeading = 181.02f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new ScrapYard() {
                Name = "Hairy Al's Scrap",
                Description = "Cars bought - parts sold",
                EntrancePosition = new Vector3(6620.79f, -3519.023f, 17.31241f),
                EntranceHeading = 155.3214f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.ScrapYards.AddRange(ScrapYardsList);
    }
    private void DefaultConfig_Sports()
    {
        LibertyCityLocations.SportingGoodsStores.AddRange(new List<SportingGoodsStore>()
        {
            new SportingGoodsStore() {
                MenuID = "SportingGoodsMenu",
                Name = "Sportsland Sporting",
                Description = "Land of sports, and they're all good",
                EntrancePosition = new Vector3(6085.547f, -3553.609f, 18.30759f),
                EntranceHeading = 266.9957f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new SportingGoodsStore() {
                MenuID = "DepartmentStoreMenu",
                Name = "Spender's",
                Description = "Low quality products with prices jacked up to the stratosphere!",
                EntrancePosition = new Vector3(5010.107f, -3323.864f, 14.76192f),
                EntranceHeading = 86.51f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new SportingGoodsStore() {
                MenuID = "MallMenu",
                Name = "Big Willis Mall",
                Description = "Get your window shopping on",
                EntrancePosition = new Vector3(7009.987f, -2637.404f, 28.91098f),
                EntranceHeading = 86.7f,
                OpenTime = 9,
                CloseTime = 23,
                StateID = StaticStrings.LibertyStateID,
            },
            new SportingGoodsStore(new Vector3(3654.558f, -1853.832f, 13.16973f), 358.829f, "Leftwood Sports", "Right our leftwood!", "VespucciSportsMenu")
            {
                StateID = StaticStrings.AlderneyStateID,
                VehiclePreviewLocation = new SpawnPlace(new Vector3(3647.738f, -1855.155f, 13.16971f), 0.2969824f),
                VehiclePreviewCameraPosition = new Vector3(3649.621f, -1851.998f, 14.23655f),
                VehiclePreviewCameraDirection = new Vector3(-0.5692362f, -0.7171011f, -0.4021644f),
                VehiclePreviewCameraRotation = new Rotator(-23.71356f, 2.704272E-05f, 141.5574f),
                VehicleDeliveryLocations = new List<SpawnPlace>()
                    {
                        new SpawnPlace(new Vector3(3651.075f, -1849.927f, 13.04998f), 323.6865f),
                    }
            },
            new SportingGoodsStore(new Vector3(4943.898f, -1849.231f, 20.41765f), 359.3634f, "Sporting Corner", "Crevis Heat Prolaps", "VespucciSportsMenu")
            {
                StateID = StaticStrings.LibertyStateID,
                VehiclePreviewLocation = new SpawnPlace(new Vector3(4946.14453f, -1847.36523f, 19.8146229f), 57.26212f),
                VehiclePreviewCameraPosition = new Vector3(4945.05859f, -1844.87817f, 21.1198235f),
                VehiclePreviewCameraDirection = new Vector3(0.2524882f, -0.9221741f, -0.2929927f),
                VehiclePreviewCameraRotation = new Rotator(-17.03721f, -8.929615E-07f, -164.6879f),
                VehicleDeliveryLocations = new List<SpawnPlace>()
                    {
                        new SpawnPlace(new Vector3(4940.858f, -1847.43323f, 19.8205528f), 86.86047f),
                    }
            },
            new SportingGoodsStore(new Vector3(5205.104f, -3597.213f, 14.78605f), 211.2678f, "People Sport", "", "VespucciSportsMenu")
            {
                StateID = StaticStrings.LibertyStateID,
                VehiclePreviewLocation = new SpawnPlace(new Vector3(5207.4043f, -3598.10059f, 14.1463928f), 172.6058f),
                VehiclePreviewCameraPosition = new Vector3(5209.33057f, -3600.02563f, 15.6516829f),
                VehiclePreviewCameraDirection = new Vector3(-0.6260471f, 0.6886612f, -0.3658017f),
                VehiclePreviewCameraRotation = new Rotator(-21.45693f, -2.752058E-06f, 42.2733f),
                VehicleDeliveryLocations = new List<SpawnPlace>()
                    {
                        new SpawnPlace(new Vector3(5210.64453f, -3593.99146f, 14.1467028f), 177.3499f),
                    }
            },
            new SportingGoodsStore(new Vector3(4914.595f, -1950.951f, 24.73437f), 286.9402f, "Baskets", "", "VespucciSportsMenu")
            {
                StateID = StaticStrings.LibertyStateID,
                VehiclePreviewLocation = new SpawnPlace(new Vector3(4915.60156f, -1948.99316f, 24.1312637f), 319.212f),
                VehiclePreviewCameraPosition = new Vector3(4917.71143f, -1949.98218f, 25.4628143f),
                VehiclePreviewCameraDirection = new Vector3(-0.8196946f, 0.4454925f, -0.3600516f),
                VehiclePreviewCameraRotation = new Rotator(-21.10336f, 8.236356E-06f, 61.47654f),
                VehicleDeliveryLocations = new List<SpawnPlace>()
                    {
                        new SpawnPlace(new Vector3(4915.494f, -1953.22424f, 24.1227837f), 209.1293f),
                    }
            },
            new SportingGoodsStore(new Vector3(7009.639f, -2734.891f, 28.74955f), 89.81207f, "Bicycles", "", "BourgeoisBicyclesMenu")
            {
                StateID = StaticStrings.LibertyStateID,
                VehiclePreviewLocation = new SpawnPlace(new Vector3(7008.299f, -2737.167f, 27.90784f), 150.526f),
                VehicleDeliveryLocations = new List<SpawnPlace>()
                    {
                        new SpawnPlace(new Vector3(7007.257f, -2730.859f, 28.03461f), 89.60334f),
                    }
            },
        });
    }
    private void DefaultConfig_SubwayStations()
    {
        LibertyCityLocations.SubwayStations.AddRange(new List<SubwayStation>()
        {
            new SubwayStation(new Vector3(6194.12f, -3800.076f, 14.89172f), 88.83874f,"Hove Beach Stop","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(5258.134f, -3982.534f, 4.957565f), 299.7816f,"Castle Garden","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(5075.314f, -3757.491f, 14.74453f), 268.1037f,"City Hall","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(5152.494f, -3276.569f, 14.76219f), 179.8285f,"Easton Station","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(5152.522f, -2620.443f, 14.65688f), 0.07954379f,"East Park","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(5302.479f, -3559.203f, 14.76205f), 88.94254f,"Emerald Station","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(4839.358f, -3593.503f, 4.872783f), 223.6983f,"Feldspar Station","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(4810.763f, -2886.293f, 14.75316f), 180.0942f,"Frankfort Avenue","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(4804.69f, -1820.69f, 12.41786f), 359.6377f,"Frankfort High","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(4781.906f, -1829.423f, 12.24839f), 272.5806f,"Frankfort Low","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(4638.973f, -3247.353f, 4.696638f), 271.9692f,"Hematite Station","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(5284.499f, -2734.028f, 13.22478f), 271.9743f,"Manganese East","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(4726.74f, -2727.668f, 9.851699f), 178.6376f,"Manganese West","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(5055.046f, -2102.401f, 14.76669f), 269.2798f,"North Park","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(5323.999f, -2343.578f, 14.71103f), 0.129852f,"Quartz St. East","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(4645.076f, -2315.478f, 9.940755f), 270.5335f,"Quartz St. West","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(4932.183f, -3400.574f, 14.44443f), 90.58279f,"Suffolk Station","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(4702.734f, -1922.467f, 17.47117f), 90.05782f,"Vauxite Station","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(5118.666f, -1844.507f, 20.41463f), 91.88654f,"Vespucci Circus","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(4837.873f, -2578.595f, 14.68624f), 358.3388f,"West Park","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(6576.017f, -2850.9f, 22.94693f), 88.28077f, "Huntington St", "") { StateID = StaticStrings.LibertyStateID },
            new SubwayStation(new Vector3(6836.851f, -2842.665f, 31.21511f), 4.448621f, "Lynch St", "") { StateID = StaticStrings.LibertyStateID },
            new SubwayStation(new Vector3(5595.888f, -1596.771f, 16.24634f), 139.8675f, "San Quentin Ave", ""){ StateID = StaticStrings.LibertyStateID },
            new SubwayStation(new Vector3(5938.165f, -1807.366f, 14.24811f), 0.09387907f, "Winmill St", ""){ StateID = StaticStrings.LibertyStateID },
        });
    }
    private void DefaultConfig_VehicleExporters()
    {
        List<VehicleExporter> ExportersList = new List<VehicleExporter>()
        {
            new VehicleExporter(new Vector3(5973.893f, -3431.343f, 6.089474f), 155.8235f,"East Hook Exports","Deliver Me Timbers","IVExportMidHighMenu") {
                ParkingSpaces = new List<SpawnPlace>() {
                    new SpawnPlace(new Vector3(5944.80664f, -3472.4043f, 5.129808f),336.13f),
                    new SpawnPlace(new Vector3(6003.39648f, -3417.01416f, 5.45981f),106.87f),
                    new SpawnPlace(new Vector3(5990.377f, -3419.37427f, 5.479799f),277.79f),
                    new SpawnPlace(new Vector3(5959.677f, -3445.73413f, 5.379808f),243.25f),
                    new SpawnPlace(new Vector3(5964.3667f, -3457.00415f, 5.349809f),296.14f),
                },
                StateID = StaticStrings.LibertyStateID,
                ContactName = StaticStrings.VehicleExporterContactName,
                OpenTime = 0,
                CloseTime = 24,
            },
            new VehicleExporter(new Vector3(5642.857f, -2164.316f, 2.714376f),175.517f,"Charge Island Exports","The boats are just a front","IVExportMidHighMenu") {
                ParkingSpaces = new List<SpawnPlace>() {
                new SpawnPlace() {
                Position = new Vector3(5666.107f, -2171.624f, 2.22979879f),
                Heading = 298.87f,
                },
                new SpawnPlace() {
                Position = new Vector3(5671.53662f, -2157.914f, 2.47979879f),
                Heading = 178.47f,
                },
                new SpawnPlace() {
                Position = new Vector3(5684.717f, -2173.86426f, 2.22979879f),
                Heading = 269.27f,
                },
                new SpawnPlace() {
                Position = new Vector3(5849.43652f, -2265.6543f, 2.4498f),
                Heading = 88.51f,
                },
                },
                StateID = StaticStrings.LibertyStateID,
                ContactName = StaticStrings.VehicleExporterContactName,
                OpenTime = 0,
                CloseTime = 24,
            },
            new VehicleExporter(new Vector3(4870.594f, -2937.176f, 14.81769f),90.90815f,"Star Junction Chop Shop","Hiding in plain sight, they'll never know","IVExportLowMidMenu") {
                ParkingSpaces = new List<SpawnPlace>() {
                new SpawnPlace() {
                Position = new Vector3(4866.837f, -2933.224f, 14.2098093f),
                Heading = 89.88f,
                },
                new SpawnPlace() {
                Position = new Vector3(4871.22656f, -2942.88428f, 14.2098093f),
                Heading = 269.06f,
                },
                new SpawnPlace() {
                Position = new Vector3(4865.2666f, -2917.57422f, 14.2498026f),
                Heading = 178.36f,
                },
                new SpawnPlace() {
                Position = new Vector3(4908.717f, -2911.19434f, 14.2297983f),
                Heading = 0.35f,
                },
                new SpawnPlace() {
                Position = new Vector3(4908.09668f, -2952.37427f, 14.2698069f),
                Heading = 180.71f,
                },
                },
                StateID = StaticStrings.LibertyStateID,
                ContactName = StaticStrings.VehicleExporterContactName,
                OpenTime = 0,
                CloseTime = 24,
            },
            new VehicleExporter(new Vector3(6133.665f, -1700.628f, 16.79538f), 90.47171f,"Bohan Chop Shop","Part em out, ship em off","IVExportLowMidMenu") {
                ParkingSpaces = new List<SpawnPlace>() {
                new SpawnPlace() {
                Position = new Vector3(6121.987f, -1702.97424f, 16.1197987f),
                Heading = 7.04f,
                },
                new SpawnPlace() {
                Position = new Vector3(6127.90674f, -1699.24414f, 16.1197987f),
                Heading = 100.94f,
                },
                new SpawnPlace() {
                Position = new Vector3(6108.1167f, -1700.13428f, 15.9998026f),
                Heading = 133.73f,
                },
                new SpawnPlace() {
                Position = new Vector3(6141.627f, -1708.37415f, 16.1698017f),
                Heading = 315.94f,
                },
                new SpawnPlace() {
                Position = new Vector3(6114.89648f, -1769.24414f, 16.189806f),
                Heading = 345.01f,
                },
                },
                StateID = StaticStrings.LibertyStateID,
                ContactName = StaticStrings.VehicleExporterContactName,
                OpenTime = 0,
                CloseTime = 24,
            },
            new VehicleExporter(new Vector3(3995.672f, -1391.044f, 6.619572f), 218.6314f,"Alderney Chop Shop","Once a casino, now a chop shop","IVExportLowMidMenu") {
                ParkingSpaces = new List<SpawnPlace>() {
                new SpawnPlace() {
                Position = new Vector3(3982.69678f, -1404.42407f, 5.78979635f),
                Heading = 267.58f,
                },
                new SpawnPlace() {
                Position = new Vector3(4005.54688f, -1399.14429f, 5.799806f),
                Heading = 320.35f,
                },
                new SpawnPlace() {
                Position = new Vector3(4020.6167f, -1364.62427f, 5.90980673f),
                Heading = 176.78f,
                },
                new SpawnPlace() {
                Position = new Vector3(3962.94678f, -1420.81421f, 5.78979635f),
                Heading = 0.84f,
                },
                new SpawnPlace() {
                Position = new Vector3(3887.567f, -1416.57422f, 5.78979635f),
                Heading = 315.62f,
                },
                },
                StateID = StaticStrings.AlderneyStateID,
                ContactName = StaticStrings.VehicleExporterContactName,
                OpenTime = 0,
                CloseTime = 24,
            },
            new VehicleExporter(new Vector3(3953.194f, -3221.18f, 4.285601f), 268.8973f,"Port Tudor Exports","From Alderney to anywhere in the world","IVExportMidHighMenu") {
                ParkingSpaces = new List<SpawnPlace>() {
                new SpawnPlace() {
                Position = new Vector3(3972.877f, -3241.214f, 4.303302f),
                Heading = 89.44f,
                },
                new SpawnPlace() {
                Position = new Vector3(3971.35669f, -3253.4043f, 4.303302f),
                Heading = 0.26f,
                },
                new SpawnPlace() {
                Position = new Vector3(3975.19678f, -3228.01416f, 4.303302f),
                Heading = 359.96f,
                },
                new SpawnPlace() {
                Position = new Vector3(4084.30664f, -3222.25415f, 4.303302f),
                Heading = 180.46f,
                },
                new SpawnPlace() {
                Position = new Vector3(4112.1167f, -3255.434f, 4.303302f),
                Heading = 89.19f,
                },
                new SpawnPlace() {
                Position = new Vector3(3905.88672f, -3399.0542f, 4.303302f),
                Heading = 270.36f,
                },
                new SpawnPlace() {
                Position = new Vector3(4000.69678f, -2889.13428f, 4.303302f),
                Heading = 359.81f,
                },
                },
                StateID = StaticStrings.AlderneyStateID,
                ContactName = StaticStrings.VehicleExporterContactName,
                OpenTime = 0,
                CloseTime = 24,
            },
        };
        LibertyCityLocations.VehicleExporters.AddRange(ExportersList);
    }

}
