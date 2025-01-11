using LosSantosRED.lsr.Helper;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


public class PlacesOfInterest_Liberty
{
    private PossibleLocations LibertyCityLocations;
    private PedCustomizerLocation DefaultPedCustomizerLocation;
    private PedCustomizerLocation AboveDefaultPedCustomizerLocation;
    private PlacesOfInterest PlacesOfInterest;

    public PlacesOfInterest_Liberty(PlacesOfInterest placesOfInterest)
    {
        PlacesOfInterest = placesOfInterest;
    }

    public void DefaultConfig()
    {
        LibertyCityLocations = new PossibleLocations();
        //These are for centered above, remove 200 from height
        DefaultConfig_Other();
        foreach (GameLocation bl in LibertyCityLocations.InteractableLocations())
        {
            bl.AddDistanceOffset(new Vector3(0f, 0f, -200f));
        }
        //These are for regular Centered
        DefaultConfig_PoliceStations();
        DefaultConfig_Hospitals();
        DefaultConfig_Prisons();
        DefaultConfig_GangDens();
        DefaultConfig_Banks();
        DefaultConfig_Restaurants();
        DefaultConfig_Sports();
        DefaultConfig_Dealerships();
        DefaultConfig_ConvenienceStores();
        DefaultConfig_BarberShops();
        DefaultConfig_FoodStands();
        DefaultConfig_Landmarks();
        DefaultConfig_Bars();
        DefaultConfig_Pharmacies();
        DefaultConfig_HardwardStores();
        DefaultConfig_PedCustomizeLocation();
        DefaultConfig_Airports();
        DefaultConfig_Garages();
        DefaultConfig_SubwayStations();
        DefaultConfig_LiquorStore();
        DefaultConfig_ClothingShops();
        DefaultConfig_Residences();
        DefaultConfig_PawnShops();
        DefaultConfig_FireStations();
        DefaultConfig_GamblingDens();
        DefaultConfig_CityHalls();
        DefaultConfig_HeadShops();
        DefaultConfig_DeadDrops();
        DefaultConfig_GunStores();
        DefaultConfig_BlankLocations();
        LibertyCityLocations.PedCustomizerLocation = DefaultPedCustomizerLocation;
        Serialization.SerializeParam(LibertyCityLocations, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LibertyConfigFolder}\\Locations_{StaticStrings.LibertyConfigSuffix}.xml");
        //PossibleLocations centeredAbove = LibertyCityLocations.Copy();
        //foreach (GameLocation bl in centeredAbove.InteractableLocations())//for centered above we want to add 200 of height
        //{
        //    bl.AddDistanceOffset(new Vector3(0f, 0f, 200f));
        //}
        //centeredAbove.PedCustomizerLocation.AddDistanceOffset(new Vector3(0f, 0f, 200f));
        //Serialization.SerializeParam(centeredAbove, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LibertyConfigFolder}\\Variations\\CenteredAbove\\Locations_{StaticStrings.LibertyConfigSuffix}CenteredAbove.xml");

        //PossibleLocations eastLC = LibertyCityLocations.Copy();
        //foreach (GameLocation bl in eastLC.InteractableLocations())//for centered above we want to add 200 of height
        //{
        //    bl.AddDistanceOffset(new Vector3(4949.959f, -1184.845f, -0.000109f));
        //}
        //eastLC.PedCustomizerLocation.AddDistanceOffset(new Vector3(4949.959f, -1184.845f, -0.000109f));
        //Serialization.SerializeParam(eastLC, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LibertyConfigFolder}\\Variations\\East\\Locations_{StaticStrings.LibertyConfigSuffix}East.xml");


        PossibleLocations lppLC = LibertyCityLocations.Copy();

        lppLC.PedCustomizerLocation = PlacesOfInterest.PossibleLocations.PedCustomizerLocation;

        foreach (GameLocation bl in lppLC.InteractableLocations())//for centered above we want to add 200 of height
        {
            bl.AddDistanceOffset(new Vector3(4949.947f, -3750.0441f, -0.000197f));
        }
        foreach(GameLocation gl in PlacesOfInterest.InteractableLocations())
        {
            gl.AddLocation(lppLC);
        }


        Serialization.SerializeParam(lppLC, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LPPConfigFolder}\\Locations_{StaticStrings.LPPConfigSuffix}.xml");
        //offset is (4949.947f, -3750.0441f, -0.000197f)); for lpp whenever
    }
    private void DefaultConfig_PawnShops()
    {
        LibertyCityLocations.PawnShops.AddRange(new List<PawnShop>()
        {
            new PawnShop(new Vector3(1166.26f, 221.4832f, 19.15601f), 94.3014f, "We Pay Cash Pawn", "","PawnShopMenuGeneric") {StateID = StaticStrings.LibertyStateID, MinPriceRefreshHours = 12, MaxPriceRefreshHours = 24,MinRestockHours = 12, MaxRestockHours = 24,},


            new PawnShop(new Vector3(390.1691f, 237.2966f, 14.75332f), 91.00311f, "Pawn Star", "","PawnShopMenuGeneric") {StateID = StaticStrings.LibertyStateID, MinPriceRefreshHours = 12, MaxPriceRefreshHours = 24,MinRestockHours = 12, MaxRestockHours = 24,},
            new PawnShop(new Vector3(339.6178f, 317.5372f, 14.54821f), 356.0719f, "Pawn Star", "","PawnShopMenuGeneric") {StateID = StaticStrings.LibertyStateID, MinPriceRefreshHours = 12, MaxPriceRefreshHours = 24,MinRestockHours = 12, MaxRestockHours = 24,},
            new PawnShop(new Vector3(-60.1551f, 172.431f, 11.79101f), 205.4112f, "Pawn Star", "","PawnShopMenuGeneric") {StateID = StaticStrings.LibertyStateID, MinPriceRefreshHours = 12, MaxPriceRefreshHours = 24,MinRestockHours = 12, MaxRestockHours = 24,},
            new PawnShop(new Vector3(1525.151f, 427.7028f, 27.82906f), 356.2724f, "XXX Pawn", "","PawnShopMenuGeneric") {StateID = StaticStrings.LibertyStateID, MinPriceRefreshHours = 12, MaxPriceRefreshHours = 24,MinRestockHours = 12, MaxRestockHours = 24,},


            new PawnShop(new Vector3(1312.964f, 538.3285f, 34.01828f), 269.4268f, "Pawn Shop", "","PawnShopMenuGeneric") {StateID = StaticStrings.LibertyStateID, MinPriceRefreshHours = 12, MaxPriceRefreshHours = 24,MinRestockHours = 12, MaxRestockHours = 24,},
            new PawnShop(new Vector3(2020.087f, 960.3093f, 28.17154f), 179.1037f, "We Pay Cash Pawn", "","PawnShopMenuGeneric") {StateID = StaticStrings.LibertyStateID, MinPriceRefreshHours = 12, MaxPriceRefreshHours = 24,MinRestockHours = 12, MaxRestockHours = 24,},
        });
    }
    private void DefaultConfig_Residences()
    {
        LibertyCityLocations.Residences.AddRange(new List<Residence>()
        {
            new Residence(new Vector3(1251.653f, 1.474908f, 19.76758f), 90.61584f, "Hove Beach Apt 124", ""){ StateID = StaticStrings.LibertyStateID,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(1058.779f, 192.1109f, 17.29779f), 272.7327f, "Hove Beach Apt 3B", ""){ StateID = StaticStrings.LibertyStateID,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(1172.407f, 158.2507f, 19.69745f), 90.64713f, "Hove Beach Apt 23", ""){ StateID = StaticStrings.LibertyStateID,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(1086.602f, 228.1993f, 16.91974f), 94.85487f, "Hove Beach Apt 76", ""){ StateID = StaticStrings.LibertyStateID, OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},


            new Residence(new Vector3(316.3927f, 789.0976f, 14.76556f), 358.0545f, "Prosperous Towers Apt 13", ""){ StateID = StaticStrings.LibertyStateID, InteriorID = -1,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(-203.3354f, 910.0369f, 10.16818f), 90.83221f, "Galveston Ave Apt 3", ""){ StateID = StaticStrings.LibertyStateID, InteriorID = -1,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(-48.09825f, 282.343f, 14.89098f), 92.79767f, "47-49 Back Passage Apt 5", ""){ StateID = StaticStrings.LibertyStateID, InteriorID = -1,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(339.9882f, 1330.448f, 15.8269f), 0.1776938f, "Albany Ave Apt 1", ""){ StateID = StaticStrings.LibertyStateID, InteriorID = -1,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(340.1831f, 1348.343f, 15.82798f), 177.875f, "Albany Ave Apt 2", ""){ StateID = StaticStrings.LibertyStateID, InteriorID = -1,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(471.2263f, 479.3755f, 15.32231f), 179.8213f, "67 Albany Ave Apt 9", ""){ StateID = StaticStrings.LibertyStateID, InteriorID = -1,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(397.0629f, 496.8959f, 15.38852f), 86.79634f, "69 Albany Ave Apt 24", ""){ StateID = StaticStrings.LibertyStateID, InteriorID = -1,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(404.7264f, 407.6198f, 15.38851f), 88.54283f, "71 Albany Ave Apt 16", ""){ StateID = StaticStrings.LibertyStateID, InteriorID = -1,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(480.7567f, 393.2626f, 15.38499f), 180.9357f, "73 Albany Ave Apt 16", ""){ StateID = StaticStrings.LibertyStateID, InteriorID = -1,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(-185.694f, 1441.29f, 12.76013f), 177.5637f, "Quartz Street Apt 5", ""){ StateID = StaticStrings.LibertyStateID, InteriorID = -1,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(-262.5976f, 2019.525f, 18.86199f), 250.2728f, "82 Galveston Drive Apt 12", ""){ StateID = StaticStrings.LibertyStateID, InteriorID = -1,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(-268.9766f, 1957.735f, 18.86181f), 2.764974f, "84 Galveston Drive Apt 14", ""){ StateID = StaticStrings.LibertyStateID, InteriorID = -1,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(-107.258f, 590.7473f, 14.8166f), 87.30843f, "15 Frankfort Apt 32", ""){ StateID = StaticStrings.LibertyStateID, InteriorID = -1,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(-145.0645f, 1164.007f, 15.59308f), 268.3113f, "Mammon Heights Apt 42", ""){ StateID = StaticStrings.LibertyStateID, InteriorID = -1,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},
            new Residence(new Vector3(-144.0693f, 1246.052f, 14.711f), 274.091f, "Mayfair Towers Apt 16", ""){ StateID = StaticStrings.LibertyStateID, InteriorID = -1,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},


            //interior residences
            new Residence(new Vector3(-189.5379f, 1965.567f, 20.16242f), 266.3529f, "Playboy X Penthouse", "Xenotime Street") { StateID = StaticStrings.LibertyStateID, InteriorID = 152578,OpenTime = 0,CloseTime = 24, PurchasePrice = 875000,SalesPrice = 380000, RentalDays = 28, RentalFee = 1570,

                CameraPosition = new Vector3(-188.5651f, 1968.313f, 21.96364f),
                CameraDirection = new Vector3(0.8637326f, -0.4273203f, -0.2671391f),
                CameraRotation = new Rotator(-15.4941f, -4.429858E-06f, -116.3233f)
                },
            new Residence(new Vector3(335.563f, 1334.307f, 15.79154f), 355.4688f, "Studio Apartment", "Albany Avenue") { StateID = StaticStrings.LibertyStateID, InteriorID = 142850,OpenTime = 0,CloseTime = 24, PurchasePrice = 575000,SalesPrice = 280000, RentalDays = 28, RentalFee = 1570,

                CameraPosition = new Vector3(357.1641f, 1338.852f, 18.29427f),
                CameraDirection = new Vector3(-0.9752057f, 0.001642297f, -0.2212941f),
                CameraRotation = new Rotator(-12.78506f, -1.709921E-09f, 89.90351f)
                },

            new Residence(new Vector3(834.5208f, 1894.839f, 11.17641f), 91.03838f, "South Bohan Apartment", "") {  DisableInteractAfterPurchase = true, IsWalkup = true, StateID = StaticStrings.LibertyStateID, InteriorID = 39170,OpenTime = 0,CloseTime = 24, PurchasePrice = 175000,SalesPrice = 80000, RentalDays = 28, RentalFee = 1270  },
            new Residence(new Vector3(1134.953f, -11.25595f, 15.19295f), 272.1486f, "Broker Apartment", "") {  DisableInteractAfterPurchase = true, IsWalkup = true, StateID = StaticStrings.LibertyStateID, InteriorID = 46850,OpenTime = 0,CloseTime = 24, PurchasePrice = 175000,SalesPrice = 80000, RentalDays = 28, RentalFee = 1270  },
            new Residence(new Vector3(-724.8428f, 1387.85f, 13.80493f), 2.633898f, "Alderney Apartment", "") {  DisableInteractAfterPurchase = true, IsWalkup = true, StateID = StaticStrings.LibertyStateID, InteriorID = 111874,OpenTime = 0,CloseTime = 24, PurchasePrice = 275000,SalesPrice = 120000, RentalDays = 28, RentalFee = 1270  },

        });
    }
    private void DefaultConfig_Airports()
    {
        List<Airport> AirportList_23 = new List<Airport>() {
            new Airport() {
                Name = "Francis Intl.",
                AirportID = "FIA",
                Description = "Great To Visit, Even Better To Leave~n~~n~City: ~y~Dukes, Liberty City~s~~n~State: ~p~Liberty~s~",
                EntrancePosition = new Vector3(2605.07f, 899.04f, 206.08f-200f),
                EntranceHeading = 91.74f,
                OpenTime = 0,
                CloseTime = 24,
                ArrivalPosition = new Vector3(2596.69f, 821.39f, 206.08f-200f),
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
        LibertyCityLocations.Airports.AddRange(AirportList_23);
        List<YanktonAiport> YanktonAiportList_24 = new List<YanktonAiport>() {
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
        LibertyCityLocations.Airports.AddRange(YanktonAiportList_24);
    }
    private void DefaultConfig_PedCustomizeLocation()
    {
        DefaultPedCustomizerLocation = new PedCustomizerLocation();
        //DefaultPedCustomizerLocation.DefaultModelPedPosition = new Vector3(-1257.695f, 1610.94f, 23.20882f);
        //DefaultPedCustomizerLocation.DefaultModelPedHeading = 0.3566196f;
        //DefaultPedCustomizerLocation.DefaultPlayerHoldingPosition = new Vector3(-1252.206f, 1613.172f, 23.20882f);
        //List<CameraCyclerPosition> CameraCyclerPositions = new List<CameraCyclerPosition>();
        //CameraCyclerPositions.Add(new CameraCyclerPosition("Default", new Vector3(-1257.73f, 1613.432f, 23.80165f), new Vector3(-0.0111506f, -0.9745571f, -0.2238618f), new Rotator(-12.93596f, -2.737518E-08f, 179.3445f), 0));//new Vector3(402.8145f, -998.5043f, -98.29621f), new Vector3(-0.02121102f, 0.9286007f, -0.3704739f), new Rotator(-21.74485f, -5.170386E-07f, 1.308518f), 0));
        //CameraCyclerPositions.Add(new CameraCyclerPosition("Face", new Vector3(-1257.73f, 1613.432f, 23.80165f), new Vector3(-0.0111506f, -0.9745571f, -0.2238618f), new Rotator(-12.93596f, -2.737518E-08f, 179.3445f), 1));
        //CameraCyclerPositions.Add(new CameraCyclerPosition("Lower", new Vector3(-1257.73f, 1613.432f, 23.80165f), new Vector3(-0.0111506f, -0.9745571f, -0.2238618f), new Rotator(-12.93596f, -2.737518E-08f, 179.3445f), 2));
        //CameraCyclerPositions.Add(new CameraCyclerPosition("Torso", new Vector3(-1257.73f, 1613.432f, 23.80165f), new Vector3(-0.0111506f, -0.9745571f, -0.2238618f), new Rotator(-12.93596f, -2.737518E-08f, 179.3445f), 3));
        //CameraCyclerPositions.Add(new CameraCyclerPosition("Hands", new Vector3(-1257.73f, 1613.432f, 23.80165f), new Vector3(-0.0111506f, -0.9745571f, -0.2238618f), new Rotator(-12.93596f, -2.737518E-08f, 179.3445f), 4));



        DefaultPedCustomizerLocation.DefaultModelPedPosition = new Vector3(1117.94f, 54.14467f, 15.85339f);
        DefaultPedCustomizerLocation.DefaultModelPedHeading = 184.7249f;
        DefaultPedCustomizerLocation.DefaultPlayerHoldingPosition = new Vector3(1125.923f, 52.98021f, 15.86571f);
        List<CameraCyclerPosition> CameraCyclerPositions = new List<CameraCyclerPosition>();
        CameraCyclerPositions.Add(new CameraCyclerPosition("Default", new Vector3(1118.04f, 51.75856f, 16.29714f), new Vector3(-0.007076373f, 0.9751766f, -0.2213152f), new Rotator(-12.78629f, 6.01895E-07f, 0.4157597f), 0));//new Vector3(402.8145f, -998.5043f, -98.29621f), new Vector3(-0.02121102f, 0.9286007f, -0.3704739f), new Rotator(-21.74485f, -5.170386E-07f, 1.308518f), 0));
        CameraCyclerPositions.Add(new CameraCyclerPosition("Face", new Vector3(1118.006f, 53.37799f, 16.5719f), new Vector3(-0.01283151f, 0.9927738f, -0.1193125f), new Rotator(-6.852426f, -4.030857E-08f, 0.7405014f), 1));
        CameraCyclerPositions.Add(new CameraCyclerPosition("Lower", new Vector3(1117.979f, 53.1876f, 15.26992f), new Vector3(-0.01943615f, 0.9749539f, -0.2215562f), new Rotator(-12.80045f, 2.73604E-08f, 1.142066f), 2));
        CameraCyclerPositions.Add(new CameraCyclerPosition("Torso", new Vector3(1118.019f, 52.51995f, 15.86643f), new Vector3(-0.004468728f, 0.9992217f, -0.03919213f), new Rotator(-2.246119f, 1.184854E-07f, 0.256237f), 3));
        CameraCyclerPositions.Add(new CameraCyclerPosition("Hands", new Vector3(1118.019f, 52.51995f, 15.86643f), new Vector3(-0.004468728f, 0.9992217f, -0.03919213f), new Rotator(-2.246119f, 1.184854E-07f, 0.256237f), 4));






        DefaultPedCustomizerLocation.CameraCyclerPositions = CameraCyclerPositions;
    }
    private void DefaultConfig_Other()
    {
        //ALL THESE NEED -200 to the Z for the centeres
        Bank BOL1 = new Bank(new Vector3(225.23f, 14.52f, 215.37f), 177.45f, "Bank Of Liberty", "Bleeding you dry","BOL") { StateID = StaticStrings.LibertyStateID, OpenTime = 6, CloseTime = 20};
        LibertyCityLocations.Banks.Add(BOL1);

        Bank BOL2 = new Bank(new Vector3(1388.10f, 259.31f, 223.59f), 0.02f, "Bank Of Liberty", "Bleeding you dry", "BOL") { StateID = StaticStrings.LibertyStateID, OpenTime = 6, CloseTime = 20 };
        LibertyCityLocations.Banks.Add(BOL2);

        Bank BOL3 = new Bank(new Vector3(1431.68f, 1137.02f, 238.78f), 90.68f, "Bank Of Liberty", "Bleeding you dry", "BOL") { StateID = StaticStrings.LibertyStateID, OpenTime = 6, CloseTime = 20 };
        LibertyCityLocations.Banks.Add(BOL3);

        Bank BOL4 = new Bank(new Vector3(323.26f, 1012.09f, 213.23f), 270.99f, "Bank Of Liberty", "Bleeding you dry", "BOL") { StateID = StaticStrings.LibertyStateID, OpenTime = 6, CloseTime = 20 };
        LibertyCityLocations.Banks.Add(BOL4);

        Bank BOL5 = new Bank(new Vector3(83.71f, 318.04f, 214.76f), 358.65f, "Bank Of Liberty", "Bleeding you dry", "BOL") { StateID = StaticStrings.LibertyStateID, OpenTime = 6, CloseTime = 20 };
        LibertyCityLocations.Banks.Add(BOL5);

        Bank BOL6 = new Bank(new Vector3(251.77f, -138.63f, 215.19f), 129.22f, "Bank Of Liberty", "Bleeding you dry", "BOL") { StateID = StaticStrings.LibertyStateID, OpenTime = 6, CloseTime = 20 };
        LibertyCityLocations.Banks.Add(BOL6);

        Bank BOL7 = new Bank(new Vector3(-11.50f, 1863.38f, 225.02f), 90.65f, "Bank Of Liberty", "Bleeding you dry", "BOL") { StateID = StaticStrings.LibertyStateID, OpenTime = 6, CloseTime = 20 };
        LibertyCityLocations.Banks.Add(BOL7);

 
        List<ScrapYard> ScrapYardList_5 = new List<ScrapYard>() {
            new ScrapYard() {
                Name = "Rusty Schit Salvage",
                Description = "We'll take it from you",
                EntrancePosition = new Vector3(-231.24f, 2242.77f, 208.75f),
                EntranceHeading = 117.67f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new ScrapYard() {
                Name = "Dukes Used Auto Parts",
                Description = "We buy and sell auto junk",
                EntrancePosition = new Vector3(1283.5f, 1019.19f, 224.47f),
                EntranceHeading = 181.02f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new ScrapYard() {
                Name = "Hairy Al's Scrap",
                Description = "Cars bought - parts sold",
                EntrancePosition = new Vector3(1676.12f,230.44f,217.19f),
                EntranceHeading = 222.11f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.ScrapYards.AddRange(ScrapYardList_5);
        List<CarCrusher> CarCrusherList_6 = new List<CarCrusher>() {
            new CarCrusher() {
                Name = "Colony Crusher",
                Description = "Dead skunk in the trunk?",
                EntrancePosition = new Vector3(786.45f, 700.37f, 208.73f),
                EntranceHeading = 90.32f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.CarCrushers.AddRange(CarCrusherList_6);


        List<Hotel> HotelList_9 = new List<Hotel>() {
            new Hotel() {
                MenuID = "ExpensiveHotelMenu",
                Name = "The Majestic",
                Description = "The best in town, unrivaled",
                EntrancePosition = new Vector3(71.9f, 1102.72f, 215.02f),
                EntranceHeading = 355.64f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Hotel() {
                MenuID = "ExpensiveHotelMenu",
                Name = "The Star Plaza Hotel",
                Description = "Sleep like a star",
                EntrancePosition = new Vector3(-47.83f,987.4f,214.78f),
                EntranceHeading = 264.06f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Hotel() {
                MenuID = "ExpensiveHotelMenu",
                Name = "Banner Hotel",
                Description = "Luxury frachised",
                EntrancePosition = new Vector3(64.25f,891.57f,214.82f),
                EntranceHeading = 179.24f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Hotel() {
                MenuID = "ExpensiveHotelMenu",
                Name = "The Nicoise Hotel",
                Description = "5 star hotel for 1 star people",
                EntrancePosition = new Vector3(263.52f,834.14f,214.7f),
                EntranceHeading = 268.48f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Hotel() {
                MenuID = "ExpensiveHotelMenu",
                Name = "Grand Northumbrian",
                Description = "The spot to crash out after being robbed in Mirror Park",
                EntrancePosition = new Vector3(208.61f,1452.78f,214.93f),
                EntranceHeading = 89.19f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Hotel() {
                MenuID = "ExpensiveHotelMenu",
                Name = "Castle Gardens Hotel",
                Description = "Resort experience in the heart of Liberty",
                EntrancePosition = new Vector3(127.6f,-406.53f,205.52f),
                EntranceHeading = 357.78f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Hotel() {
                MenuID = "ExpensiveHotelMenu",
                Name = "Hotel Hamilton",
                Description = "Top in the trash",
                EntrancePosition = new Vector3(-75.54f,1900.84f,215.45f),
                EntranceHeading = 0.81f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Hotel() {
                MenuID = "CheapHotelMenu",
                Name = "Beach Hotel",
                Description = "Enjoy the smell of the polluted ocean",
                EntrancePosition = new Vector3(1208.62f, -86.64f, 214.31f),
                EntranceHeading = 358.44f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Hotel() {
                MenuID = "CheapHotelMenu",
                Name = "Berchem Hotel",
                Description = "Stay in the heart of Alderney",
                EntrancePosition = new Vector3(-1226.15f, 1160.89f, 220.11f),
                EntranceHeading = 240.01f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.AlderneyStateID,
            },
            new Hotel() {
                MenuID = "ExpensiveHotelMenu",
                Name = "Opium Nights Hotel",
                Description = "Where Dreams Unwind and Luxury Awaits",
                EntrancePosition = new Vector3(206.7601f, 1240.835f, 214.96519f),
                EntranceHeading = 86.68562f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },

        };
        LibertyCityLocations.Hotels.AddRange(HotelList_9);
        List<Residence> ResidenceList_10 = new List<Residence>() {
            new Residence() {
                Name = "370 Galveston Ave",
                EntrancePosition = new Vector3( - 201.89f, 1883.25f, 216.45f),
                EntranceHeading = 90.35f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 2000,
                PurchasePrice = 400000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "1666 Valdez St",
                EntrancePosition = new Vector3(910.86f, 2233.65f, 237.76f),
                EntranceHeading = 180.02f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 1300,
                PurchasePrice = 260000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "Northern Gardens Apt 248",
                EntrancePosition = new Vector3(1307.97f, 2349.85f, 212.87f),
                EntranceHeading = 176.59f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 1200,
                PurchasePrice = 240000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "50 Shinnecock Ave",
                EntrancePosition = new Vector3(1561.38f, -308.4f, 208.42f),
                EntranceHeading = 267.59f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 3750,
                PurchasePrice = 750000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "35 Iroquois Ave",
                EntrancePosition = new Vector3(1087.62f, 221.67f, 216.97f),
                EntranceHeading = 84.59f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 1000,
                PurchasePrice = 180000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "216 Stillwater Ave",
                EntrancePosition = new Vector3(1827.85f, 1061.8f, 230.79f),
                EntranceHeading = 86.2f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 3000,
                PurchasePrice = 600000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "Steinway Projects Apt 98",
                EntrancePosition = new Vector3(1453.92f, 1347.01f, 236.52f),
                EntranceHeading = 222.8f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 1200,
                PurchasePrice = 240000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "560 Eastwood Apartments",
                EntrancePosition = new Vector3(735.87f, 572.61f, 208.73f),
                EntranceHeading = 269.21f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 1500,
                PurchasePrice = 300000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "3250 Bismarck Ave",
                EntrancePosition = new Vector3(266.08f, 1619.3f, 214.66f),
                EntranceHeading = 269.14f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 2500,
                PurchasePrice = 500000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "1090 Borlock Rd",
                EntrancePosition = new Vector3(442.06f, 28.18f, 211.46f),
                EntranceHeading = 269.36f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 1750,
                PurchasePrice = 350000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "511 Feldspar St",
                EntrancePosition = new Vector3(83.69f, 271.23f, 213.47f),
                EntranceHeading = 179.24f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 1875,
                PurchasePrice = 375000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "130 Columbus Ave Penthouse",
                EntrancePosition = new Vector3(152.75f, 669.86f, 215.47f),
                EntranceHeading = 272.89f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 5000,
                PurchasePrice = 1000000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "32 Panhandle Rd",
                EntrancePosition = new Vector3( - 968.8f, 1634.06f, 216.7f),
                EntranceHeading = 1.64f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 1000,
                PurchasePrice = 190000,
                StateID = StaticStrings.AlderneyStateID,
            },
            new Residence() {
                Name = "9 Cariboo Ave",
                EntrancePosition = new Vector3( - 930.59f, 2028.32f, 230.77f),
                EntranceHeading = 178.75f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 2875,
                PurchasePrice = 575000,
                StateID = StaticStrings.AlderneyStateID,
            },
            new Residence() {
                Name = "1396 Red Wing Ave",
                EntrancePosition = new Vector3( - 1760.63f, 470.83f, 207.5f),
                EntranceHeading = 358.88f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 1100,
                PurchasePrice = 220000,
                StateID = StaticStrings.AlderneyStateID,
            },
            new Residence() {
                Name = "46 Babbage Dr",
                EntrancePosition = new Vector3( - 1276.63f, 778.13f, 215.98f),
                EntranceHeading = 266.5f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 1900,
                PurchasePrice = 380000,
                StateID = StaticStrings.AlderneyStateID,
            },




            new Residence() {
                Name = "Firefly Projects Apt 153",
                EntrancePosition = new Vector3(1475.07f,68.07f,216.66f),
                EntranceHeading = 215.67f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 14,
                RentalFee = 1100,
                PurchasePrice = 220000,
                SalesPrice = 120000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "Greg Johnson Projects Apt 2235",
                EntrancePosition = new Vector3(117.59f,1993.17f,219.02f),
                EntranceHeading = 220.4f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 14,
                RentalFee = 1300,
                PurchasePrice = 260000,
                SalesPrice = 160000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "722 Savannah Ave",
                EntrancePosition = new Vector3(1621.41f,1029.14f,233.11f),
                EntranceHeading = 270.25f,
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
                EntrancePosition = new Vector3(-246.27f,1334.89f,211.02f),
                EntranceHeading = 357.57f,
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
                EntrancePosition = new Vector3(1153.67f,675.45f,236.45f),
                EntranceHeading = 267.69f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 14,
                RentalFee = 1800,
                PurchasePrice = 360000,
                SalesPrice = 260000,
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.Residences.AddRange(ResidenceList_10);
        List<CityHall> CityHallList_11 = new List<CityHall>() {
            new CityHall() {
                Name = "Civic Citadel",
                Description = "City hall",
                EntrancePosition = new Vector3(59.02f, 148.46f, 217.84f),
                EntranceHeading = 175.24f,
                OpenTime = 9,
                CloseTime = 18,
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.CityHalls.AddRange(CityHallList_11);
        List<Restaurant> RestaurantList_12 = new List<Restaurant>() {
            new Restaurant() {
                MenuID = "BeanMachineMenu",
                Name = "Superstar Cafe",
                Description = "The hotspot to not get shit done",
                EntrancePosition = new Vector3(3.17f, 533.78f, 214.71f),
                EntranceHeading = 176.29f,
                OpenTime = 6,
                CloseTime = 20,
                TypeName = "Coffee Shop",
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "BeanMachineMenu",
                Name = "Superstar Cafe",
                Description = "The hotspot to not get shit done",
                EntrancePosition = new Vector3(265.89f, 1474.1f, 214.66f),
                EntranceHeading = 265.13f,
                OpenTime = 6,
                CloseTime = 20,
                TypeName = "Coffee Shop",
                StateID = StaticStrings.LibertyStateID,
            },

            new Restaurant() {
                MenuID = "BeanMachineMenu",
                BannerImagePath = "stores\\beanmachine.png",
                Name = "Bean Machine Coffee",
                Description = "Taking over the world one Gunkaccino at a time",
                EntrancePosition = new Vector3(1276.44f,620.02f,232.89f),
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
                EntrancePosition = new Vector3(2032.61f,1124.75f,229.01f),
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
                EntrancePosition = new Vector3(349.13f,535.55f,214.82f),
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
                EntrancePosition = new Vector3(118.18f,981.08f,208.72f),
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
                EntrancePosition = new Vector3(114.61f,535.91f,214.81f),
                EntranceHeading = 89.48f,
                OpenTime = 0,
                CloseTime = 24,
                TypeName = "Coffee Shop",
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "CoffeeMenu",
                Name = "Coffee Shop",
                Description = "Pit-stop after the highway, or before",
                EntrancePosition = new Vector3(-977.69f, 1285.32f, 219.79f),
                EntranceHeading = 177.38f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.AlderneyStateID,
            },
            new Restaurant() {
                MenuID = "FancyGenericMenu",
                Name = "69th Street Diner",
                Description = "Who could resist its greasy allure?",
                EntrancePosition = new Vector3(1122.55f, 12.29f, 215.88f),
                EntranceHeading = 7.14f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "FancyGenericMenu",
                Name = "Creek St Diner",
                Description = "Even greasier than the one at 69th",
                EntrancePosition = new Vector3(1184.81f, 937.35f, 216.03f),
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
                EntrancePosition = new Vector3(-1327.56f, 1077.6f, 225.44f),
                EntranceHeading = 2.22f,
                OpenTime = 6,
                CloseTime = 20,

                StateID = StaticStrings.AlderneyStateID,
            },

            new Restaurant() {
                MenuID = "FancyGenericMenu",
                Name = "60 Diner",
                Description = "Serving quality food since 1960",
                EntrancePosition = new Vector3(351.02f, 1134.75f, 214.84f),
                EntranceHeading = 268.24f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "FancyGenericMenu",
                Name = "60 Diner",
                Description = "Serving quality food since 1960",
                EntrancePosition = new Vector3(-151.08f, 737.13f, 215.35f),
                EntranceHeading = 356.24f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "FancyGenericMenu",
                Name = "60 Diner",
                Description = "Serving quality food since 1960",
                EntrancePosition = new Vector3(-151.08f,737.13f,215.35f),
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
                EntrancePosition = new Vector3(-918.7f, 1896.49f, 224.63f),
                EntranceHeading = 176.4f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.AlderneyStateID,
            },
            new Restaurant() {
                MenuID = "PizzaThisMenu",
                Name = "Pizza This...",
                Description = "Get stuffed",
                BannerImagePath = "stores\\pizzathis.png",
                EntrancePosition = new Vector3(2059.47f, 1080.04f, 229.08f),
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
                EntrancePosition = new Vector3(2084.34f, 1185.76f, 226.66f),
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
                EntrancePosition = new Vector3(167.1f, -137.1f, 214.76f),
                EntranceHeading = 219.37f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "PizzaMenu",
                Name = "Colony Pizza",
                Description = "Colony Island's finest pizza",
                EntrancePosition = new Vector3(667.39f, 589.23f, 208.73f),
                EntranceHeading = 88.96f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "PizzaMenu",
                Name = "Drusilla's",
                Description = "Traditionally italian",
                EntrancePosition = new Vector3(118.01f, 237.25f, 212.69f),
                EntranceHeading = 89.29f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "NoodleMenu",
                Name = "Mr. Fuk's Rice Box",
                Description = "Quality korean food by Mr. Fuk himself",
                EntrancePosition = new Vector3(-1003.43f, 1566.3f, 219.78f),
                EntranceHeading = 137.64f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.AlderneyStateID,
            },
            new Restaurant() {
                MenuID = "FancyFishMenu",
                Name = "Fanny Crab's",
                Description = "You'll love the taste of our Fanny Crabs",
                EntrancePosition = new Vector3(207.47f, 428.7f, 215.2f),
                EntranceHeading = 88.77f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },


            new Restaurant() {
                MenuID = "NoodleMenu",
                Name = "Delicious Chinese Food",
                Description = "Speaks for itself",
                EntrancePosition = new Vector3(1361.27f,1161.68f,238.19f),
                EntranceHeading = 88.88f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "BeefyBillsMenu",
                Name = "Meat Factory",
                Description = "Put our meat in your mouth",
                EntrancePosition = new Vector3(-227.99f,1854.46f,217.47f),
                EntranceHeading = 268.31f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.Restaurants.AddRange(RestaurantList_12);
        List<Pharmacy> PharmacyList_13 = new List<Pharmacy>() {
            new Pharmacy() {
                MenuID = "PharmacyMenu",
                Name = "Pill Pharm",
                Description = "Big Pharma, big prices",
                EntrancePosition = new Vector3(-96.73f, 645.53f, 214.75f),
                EntranceHeading = 177.06f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Pharmacy() {
                MenuID = "PharmacyMenu",
                Name = "Pill Pharm",
                Description = "Big Pharma, big prices",
                EntrancePosition = new Vector3(2059.35f, 1098.74f, 228.87f),
                EntranceHeading = 86.71f,
                OpenTime = 9,
                CloseTime = 23,
                StateID = StaticStrings.LibertyStateID,
            },
            new Pharmacy() {
                MenuID = "PharmacyMenu",
                Name = "Pill Pharm",
                Description = "Big Pharma, big prices",
                EntrancePosition = new Vector3(1576.89f, 2138.1f, 216.85f),
                EntranceHeading = 265.4f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Pharmacy() {
                MenuID = "PharmacyMenu",
                Name = "Pill Pharm",
                Description = "Big Pharma, big prices",
                EntrancePosition = new Vector3(-1061.58f, 1323.19f, 219.81f),
                EntranceHeading = 87.01f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.AlderneyStateID,
            },
            new Pharmacy() {
                MenuID = "PharmacyMenu",
                Name = "Pill Pharm",
                Description = "Big Pharma, big prices",
                EntrancePosition = new Vector3(1481.8f,1223.48f,236.75f),
                EntranceHeading = 262.51f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Pharmacy() {
                MenuID = "PharmacyMenu",
                Name = "Aycehol",
                Description = "Same pills as anywhere else, but cheaper",
                EntrancePosition = new Vector3(-36.82f, 1770.41f, 224.71f),
                EntranceHeading = 226.09f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.Pharmacies.AddRange(PharmacyList_13);
        List<HeadShop> HeadShopList_14 = new List<HeadShop>() {
            new HeadShop() {
                MenuID = "HeadShopMenu",
                Name = "Tobacco Shop",
                Description = "Light it up",
                EntrancePosition = new Vector3(740.33f, 2253.64f, 228.46f),
                EntranceHeading = 0.03f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.HeadShops.AddRange(HeadShopList_14);
        List<HardwareStore> HardwareStoreList_15 = new List<HardwareStore>() {
            new HardwareStore() {
                MenuID = "ToolMenu",
                Name = "Cheapo's Hardware",
                Description = "Affordable tools for tools",
                EntrancePosition = new Vector3(-880.98f, 1866.26f, 224.52f),
                EntranceHeading = 2.11f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.AlderneyStateID,
            },
            new HardwareStore() {
                MenuID = "ToolMenu",
                Name = "BJ Hardware",
                Description = "Blowjobs aren't in the menu",
                EntrancePosition = new Vector3(1007.91f, -23.12f, 206.45f),
                EntranceHeading = 111.11f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new HardwareStore() {
                MenuID = "ToolMenu",
                Name = "Kack's Hardware",
                Description = "Vertified by Mr. Kack himself",
                EntrancePosition = new Vector3(845.51f, 1935.02f, 211.68f),
                EntranceHeading = 358.58f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.HardwareStores.AddRange(HardwareStoreList_15);
        List<PawnShop> PawnShopList_16 = new List<PawnShop>() {
            new PawnShop() {
                MenuID = "PawnShopMenuGeneric",
                Name = "Pawnbrokers",
                Description = "Buying and selling since '81",
                EntrancePosition = new Vector3(831.78f, 2133.7f, 230.31f),
                EntranceHeading = 3.1f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new PawnShop() {
                MenuID = "PawnShopMenuGeneric",
                Name = "Jamaican Pawn Shop",
                Description = "We won't ask where you got it from",
                EntrancePosition = new Vector3(1648.62f, 561.35f, 225.97f),
                EntranceHeading = 270.45f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new PawnShop() {
                MenuID = "PawnShopMenuGeneric",
                Name = "Alderney City Pawn",
                Description = "Find your bargain here",
                EntrancePosition = new Vector3(-1297.42f, 1500.52f, 226.85f),
                EntranceHeading = 88.45f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.AlderneyStateID,
            },
        };
        LibertyCityLocations.PawnShops.AddRange(PawnShopList_16);
        List<ConvenienceStore> ConvenienceStoreList_17 = new List<ConvenienceStore>() {
            new ConvenienceStore() {
                MenuID = "TwentyFourSevenMenu",
                Name = "24/7",
                Description = "Go all night",
                BannerImagePath = "stores\\247.png",
                EntrancePosition = new Vector3(-106.51f, 736.58f, 215.16f),
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
                EntrancePosition = new Vector3(1243.8f, 79.42f, 216.27f),
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
                EntrancePosition = new Vector3(450.18f, -11.98f, 209.43f),
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
                EntrancePosition = new Vector3(1241.57f, 2347.29f, 220.01f),
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
                EntrancePosition = new Vector3(282.81f, 271.54f, 214.76f),
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
                EntrancePosition = new Vector3(1313.52f, 542.68f, 234.08f),
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
                EntrancePosition = new Vector3(-99.04f, 1926.12f, 212.71f),
                EntranceHeading = 133.27f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new ConvenienceStore() {
                MenuID = "GasStationMenu",
                Name = "Globe Oil",
                Description = "The pumps might not work, but we'll still sell you a drink",
                EntrancePosition = new Vector3(-240.06f, 299.2f, 207.86f),
                EntranceHeading = 179.28f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new ConvenienceStore() {
                MenuID = "ConvenienceStoreMenu",
                Name = "Checkout!",
                Description = "Shopping for busy people",
                EntrancePosition = new Vector3(-1241.3f, 1887.28f, 213.52f),
                EntranceHeading = 271.59f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.AlderneyStateID,
            },
            new ConvenienceStore() {
                MenuID = "DeliGroceryMenu",
                Name = "Deli",
                Description = "Fresh food and fresh groceries",
                EntrancePosition = new Vector3(-1409.13f, 768.4f, 219.35f),
                EntranceHeading = 174.93f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.AlderneyStateID,
            },
            new ConvenienceStore() {
                MenuID = "DeliGroceryMenu",
                Name = "Deli",
                Description = "Fresh food and fresh groceries",
                EntrancePosition = new Vector3(967.88f, 2072.88f, 222.26f),
                EntranceHeading = 270.87f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new ConvenienceStore() {
                MenuID = "DeliGroceryMenu",
                Name = "Deli",
                Description = "Fresh food and fresh groceries",
                EntrancePosition = new Vector3(1646f, 738.7f, 226.26f),
                EntranceHeading = 358.25f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new ConvenienceStore() {
                MenuID = "DeliGroceryMenu",
                Name = "Deli",
                Description = "Fresh food and fresh groceries",
                EntrancePosition = new Vector3(-1358.5f, 1255.26f, 225.45f),
                EntranceHeading = 267.96f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.AlderneyStateID,
            },
            new ConvenienceStore() {
                MenuID = "DeliGroceryMenu",
                Name = "Superb Deli",
                Description = "Franchising your beloved corner stores",
                EntrancePosition = new Vector3(-202.13f, 1024.75f, 209.94f),
                EntranceHeading = 358.9f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },

            new ConvenienceStore() {
                MenuID = "DeliGroceryMenu",
                Name = "Deli",
                Description = "Fresh food and fresh groceries",
                EntrancePosition = new Vector3(1176.29f,535.71f,227.36f),
                EntranceHeading = 153.44f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new ConvenienceStore() {
                MenuID = "DeliGroceryMenu",
                Name = "Deli",
                Description = "Fresh food and fresh groceries",
                EntrancePosition = new Vector3(1432.97f,1154.37f,238.7f),
                EntranceHeading = 84.36f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new ConvenienceStore() {
                MenuID = "DeliGroceryMenu",
                Name = "DC's Mini Market",
                Description = "Food, groceries, anything you need",
                EntrancePosition = new Vector3(-790.86f,1845.34f,225.87f),
                EntranceHeading = 87.18f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.AlderneyStateID,
            },
        };
        LibertyCityLocations.ConvenienceStores.AddRange(ConvenienceStoreList_17);
        List<LiquorStore> LiquorStoreList_18 = new List<LiquorStore>() {
            new LiquorStore() {
                MenuID = "LiquorStoreMenu",
                Name = "Liquors",
                Description = "Yes, we sell liquor",
                EntrancePosition = new Vector3(1604.62f, 586.71f, 231.25f),
                EntranceHeading = 64.88f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new LiquorStore() {
                MenuID = "LiquorStoreMenu",
                Name = "Liquors",
                Description = "Yes, we sell liquor",
                EntrancePosition = new Vector3(799.21f, 2179.98f, 229.73f),
                EntranceHeading = 268.5f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.LiquorStores.AddRange(LiquorStoreList_18);
        List<GasStation> GasStationList_19 = new List<GasStation>() {
            new GasStation() {
                MenuID = "GasStationMenu",
                Name = "Globe Oil",
                Description = "Changing the Climate, Powering The Future",
                EntrancePosition = new Vector3(-1171.16f, 527.35f, 207.23f),
                EntranceHeading = 230.76f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.AlderneyStateID,
            },
            new GasStation() {
                MenuID = "XeroMenu",
                Name = "Terroil Station",
                Description = "We sell Xero gasoline though",
                BannerImagePath = "stores\\xero.png",
                EntrancePosition = new Vector3(344.17f, 1613.53f, 214.67f),
                EntranceHeading = 355.4f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new GasStation() {
                MenuID = "RonMenu",
                Name = "Ron Station",
                Description = "Put RON in your tank",
                BannerImagePath = "stores\\ron.png",
                EntrancePosition = new Vector3(1004.35f, 679.51f, 206.16f),
                EntranceHeading = 234.7f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new GasStation() {
                MenuID = "RonMenu",
                Name = "Ron Station",
                Description = "Put RON in your tank",
                BannerImagePath = "stores\\ron.png",
                EntrancePosition = new Vector3(1379.89f, 134.58f, 219.33f),
                EntranceHeading = 83.12f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new GasStation() {
                MenuID = "RonMenu",
                Name = "Ron Station",
                Description = "Put RON in your tank",
                BannerImagePath = "stores\\ron.png",
                EntrancePosition = new Vector3(2025.51f, 1326.49f, 216.58f),
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
                EntrancePosition = new Vector3(1691.96f, 2221.8f, 216.83f),
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
                EntrancePosition = new Vector3(-1075.94f, 2239.04f, 227.9f),
                EntranceHeading = 177.53f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.AlderneyStateID,
            },
            new GasStation() {
                MenuID = "RonMenu",
                Name = "Ron Station",
                Description = "Put RON in your tank",
                BannerImagePath = "stores\\ron.png",
                EntrancePosition = new Vector3(-194.07f, 490.83f, 209.97f),
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
                EntrancePosition = new Vector3(1374.29f, 821.24f, 229.79f),
                EntranceHeading = 80.35f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.GasStations.AddRange(GasStationList_19);
        List<Bar> BarList_20 = new List<Bar>() {
            new Bar() {
                MenuID = "BarMenu",
                Name = "Comrades Bar",
                Description = "Party like it's 1924",
                EntrancePosition = new Vector3(1166.28f, 3.65f, 215.49f),
                EntranceHeading = 91.39f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Bar() {
                MenuID = "BarMenu",
                Name = "Hallet's Sports Bar",
                Description = "Perfect for a drink after a funeral",
                EntrancePosition = new Vector3(682.95f, 531.97f, 208.86f),
                EntranceHeading = 60.15f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Bar() {
                MenuID = "BarMenu",
                Name = "Lucky Winkles",
                Description = "A classic Purgatory dive bar",
                EntrancePosition = new Vector3(-202.67f, 949.66f, 210.39f),
                EntranceHeading = 42.42f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Bar() {
                MenuID = "BarMenu",
                Name = "Jerkov's",
                Description = "Even the elite needs to get drunk",
                EntrancePosition = new Vector3(318.09f, 1052.21f, 218.63f),
                EntranceHeading = 178.83f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Bar() {
                MenuID = "BarMenu",
                Name = "Steinway Beer Garden",
                Description = "Take shots and shoot darts",
                EntrancePosition = new Vector3(1388.03f, 1229.34f, 235.4f),
                EntranceHeading = 224.33f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Bar() {
                MenuID = "BarMenu",
                Name = "Homebrew Cafe",
                Description = "A piece of Jamaica in dukes",
                EntrancePosition = new Vector3(1713.07f, 548.08f, 225.19f),
                EntranceHeading = 267.78f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(1704.374f, 554.5533f, 25.18571f), 271.6047f) }
            },

            new Bar() {
                MenuID = "BarMenu",
                Name = "The Triangle Club",
                Description = "Beautiful women and expensive drinks",
                EntrancePosition = new Vector3(1435.8f,2205.05f,217.48f),
                EntranceHeading = 312.4f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Bar() {
                MenuID = "BarMenu",
                Name = "Honkers",
                Description = "Beautiful women and expensive drinks",
                EntrancePosition = new Vector3(-1342.33f,520.23f,210.02f),
                EntranceHeading = 88.81f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.AlderneyStateID,
            },

        };
        LibertyCityLocations.Bars.AddRange(BarList_20);
        List<Dealership> DealershipList_21 = new List<Dealership>() {
            new Dealership() {
                MenuID = "PremiumDeluxeMenu",
                Name = "Big Paulie Budget Cars",
                Description = "Something affordable for everyone",
                EntrancePosition = new Vector3(-1414.99f, 425.34f, 206.79f),
                EntranceHeading = 176.06f,
                OpenTime = 6,
                CloseTime = 20,
                LicensePlatePreviewText = "PAULIE",
                VehiclePreviewLocation = new SpawnPlace(new Vector3(-1440.71f, 443.88f, 207.56f), 186.5f),
                CameraPosition = new Vector3(-1437.71f, 436.88f, 209.06f),
                CameraDirection = new Vector3(0.3461686f, 0.9154226f, -0.2053503f),
                VehicleDeliveryLocations = new List<SpawnPlace>() {
                    new SpawnPlace(new Vector3(-1417.26f, 496.72f, 209.71f), 132.01f),
                },
                StateID = StaticStrings.AlderneyStateID,
            },
        };
        LibertyCityLocations.CarDealerships.AddRange(DealershipList_21);
        List<VehicleExporter> ExporterList_94 = new List<VehicleExporter>()
        {
            new VehicleExporter(new Vector3(1024.06f,318.96f,206.09f),154.43f,"East Hook Exports","Deliver Me Timbers","SunshineMenu") { 
                ParkingSpaces = new List<SpawnPlace>() { 
                    new SpawnPlace(new Vector3(994.86f, 277.64f, 205.13f),336.13f),
                    new SpawnPlace(new Vector3(1053.45f, 333.03f, 205.46f),106.87f),
                    new SpawnPlace(new Vector3(1040.43f, 330.67f, 205.48f),277.79f),
                    new SpawnPlace(new Vector3(1009.73f, 304.31f, 205.38f),243.25f),
                    new SpawnPlace(new Vector3(1014.42f, 293.04f, 205.35f),296.14f),
                },
                StateID = StaticStrings.LibertyStateID,
                ContactName = StaticStrings.VehicleExporterContactName,
                OpenTime = 0,
                CloseTime = 24,
            },
            new VehicleExporter(new Vector3(692.99f,1586.14f,202.71f),175.41f,"Charge Island Exports","The boats are just a front","SunshineMenu") {
                ParkingSpaces = new List<SpawnPlace>() {
                new SpawnPlace() {
                Position = new Vector3(716.16f,1578.42f,202.23f),
                Heading = 298.87f,
                },
                new SpawnPlace() {
                Position = new Vector3(721.59f,1592.13f,202.48f),
                Heading = 178.47f,
                },
                new SpawnPlace() {
                Position = new Vector3(734.77f,1576.18f,202.23f),
                Heading = 269.27f,
                },
                new SpawnPlace() {
                Position = new Vector3(899.49f,1484.39f,202.45f),
                Heading = 88.51f,
                },
                },
                StateID = StaticStrings.LibertyStateID,
                ContactName = StaticStrings.VehicleExporterContactName,
                OpenTime = 0,
                CloseTime = 24,
            },
            new VehicleExporter(new Vector3(-79.2f,812.87f,214.82f),88.92f,"Star Junction Chop Shop","Hiding in plain sight, they'll never know","NationalMenu") {
                ParkingSpaces = new List<SpawnPlace>() {
                new SpawnPlace() {
                Position = new Vector3(-83.11f,816.82f,214.21f),
                Heading = 89.88f,
                },
                new SpawnPlace() {
                Position = new Vector3(-78.72f,807.16f,214.21f),
                Heading = 269.06f,
                },
                new SpawnPlace() {
                Position = new Vector3(-84.68f,832.47f,214.25f),
                Heading = 178.36f,
                },
                new SpawnPlace() {
                Position = new Vector3(-41.23f,838.85f,214.23f),
                Heading = 0.35f,
                },
                new SpawnPlace() {
                Position = new Vector3(-41.85f,797.67f,214.27f),
                Heading = 180.71f,
                },
                },
                StateID = StaticStrings.LibertyStateID,
                ContactName = StaticStrings.VehicleExporterContactName,
                OpenTime = 0,
                CloseTime = 24,
            },
            new VehicleExporter(new Vector3(1183.98f,2049.43f,216.76f),133.07f,"Bohan Chop Shop","Part em out, ship em off","NationalMenu") {
                ParkingSpaces = new List<SpawnPlace>() {
                new SpawnPlace() {
                Position = new Vector3(1172.04f,2047.07f,216.12f),
                Heading = 7.04f,
                },
                new SpawnPlace() {
                Position = new Vector3(1177.96f,2050.8f,216.12f),
                Heading = 100.94f,
                },
                new SpawnPlace() {
                Position = new Vector3(1158.17f,2049.91f,216f),
                Heading = 133.73f,
                },
                new SpawnPlace() {
                Position = new Vector3(1191.68f,2041.67f,216.17f),
                Heading = 315.94f,
                },
                new SpawnPlace() {
                Position = new Vector3(1164.95f,1980.8f,216.19f),
                Heading = 345.01f,
                },
                },
                StateID = StaticStrings.LibertyStateID,
                ContactName = StaticStrings.VehicleExporterContactName,
                OpenTime = 0,
                CloseTime = 24,
            },
            new VehicleExporter(new Vector3(-954.33f,2359.07f,206.62f),218.55f,"Alderney Chop Shop","Once a casino, now a chop shop","NationalMenu") {
                ParkingSpaces = new List<SpawnPlace>() {
                new SpawnPlace() {
                Position = new Vector3(-967.25f,2345.62f,205.79f),
                Heading = 267.58f,
                },
                new SpawnPlace() {
                Position = new Vector3(-944.4f,2350.9f,205.8f),
                Heading = 320.35f,
                },
                new SpawnPlace() {
                Position = new Vector3(-929.33f,2385.42f,205.91f),
                Heading = 176.78f,
                },
                new SpawnPlace() {
                Position = new Vector3(-987f,2329.23f,205.79f),
                Heading = 0.84f,
                },
                new SpawnPlace() {
                Position = new Vector3(-1062.38f,2333.47f,205.79f),
                Heading = 315.62f,
                },
                },
                StateID = StaticStrings.AlderneyStateID,
                ContactName = StaticStrings.VehicleExporterContactName,
                OpenTime = 0,
                CloseTime = 24,
            },
            new VehicleExporter(new Vector3(-976.66f,515.11f,204.31f),177.89f,"Port Tudor Exports","From Alderney to anywhere in the world","SunshineMenu") {
                ParkingSpaces = new List<SpawnPlace>() {
                new SpawnPlace() {
                Position = new Vector3(-977.07f,508.83f,203.62f),
                Heading = 89.44f,
                },
                new SpawnPlace() {
                Position = new Vector3(-978.59f,496.64f,203.62f),
                Heading = 0.26f,
                },
                new SpawnPlace() {
                Position = new Vector3(-974.75f,522.03f,203.59f),
                Heading = 359.96f,
                },
                new SpawnPlace() {
                Position = new Vector3(-865.64f,527.79f,203.53f),
                Heading = 180.46f,
                },
                new SpawnPlace() {
                Position = new Vector3(-837.83f,494.61f,203.53f),
                Heading = 89.19f,
                },
                new SpawnPlace() {
                Position = new Vector3(-1044.06f,350.99f,203.54f),
                Heading = 270.36f,
                },
                new SpawnPlace() {
                Position = new Vector3(-949.25f,860.91f,203.51f),
                Heading = 359.81f,
                },
                },
                StateID = StaticStrings.AlderneyStateID,
                ContactName = StaticStrings.VehicleExporterContactName,
                OpenTime = 0,
                CloseTime = 24,
            },
        };
        LibertyCityLocations.VehicleExporters.AddRange(ExporterList_94);

        List<Forger> ForgerList_95 = new List<Forger>()
        {
            new Forger(new Vector3(1163.51f,1076.8f,222.27f), 356.1f, "Dukes Forgeries", "More plates than the DMV")
            {
                CleanPlateSalesPrice = 200,
                WantedPlateSalesPrice = 50,
                CustomPlateCost = 550,
                RandomPlateCost = 300,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.Forgers.AddRange(ForgerList_95);



        

        List<SportingGoodsStore> SportingGoodsStoreList_22 = new List<SportingGoodsStore>() {
            new SportingGoodsStore() {
                MenuID = "SportingGoodsMenu",
                Name = "Sportsland Sporting",
                Description = "Land of sports, and they're all good",
                EntrancePosition = new Vector3(1132.65f, 196.33f, 218.42f),
                EntranceHeading = 269.42f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new SportingGoodsStore() {
                MenuID = "DepartmentStoreMenu",
                Name = "Spender's",
                Description = "Low quality products with prices jacked up to the stratosphere!",
                EntrancePosition = new Vector3(60.16f, 426.18f, 214.76f),
                EntranceHeading = 86.51f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new SportingGoodsStore() {
                MenuID = "MallMenu",
                Name = "Big Willis Mall",
                Description = "Get your window shopping on",
                EntrancePosition = new Vector3(2060.04f, 1112.64f, 228.91f),
                EntranceHeading = 86.7f,
                OpenTime = 9,
                CloseTime = 23,
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.SportingGoodsStores.AddRange(SportingGoodsStoreList_22);
        
        List<IllicitMarketplace> IllicitMarketplaceList_25 = new List<IllicitMarketplace>() {
            new IllicitMarketplace() {

                MenuID = "DealerHangoutMenu1",
                Name = "Dealer Hangout",
                Description = "Dealer Hangout",
                EntrancePosition = new Vector3(923.45f, 1265.04f, 202.79f),
                EntranceHeading = 83.74f,
                OpenTime = 11,
                CloseTime = 19,
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(923.45f, 1265.04f, 202.79f), 83.74f) },
                StateID = StaticStrings.LibertyStateID,
            },
            new IllicitMarketplace() {

                MenuID = "DealerHangoutMenu1",
                Name = "Dealer Hangout",
                Description = "Dealer Hangout 2",
                EntrancePosition = new Vector3(498.8051f, 76.59637f, 205.009362f),
                EntranceHeading = 181.692f,
                OpenTime = 11,
                CloseTime = 23,
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(498.8051f, 76.59637f, 205.009362f), 181.692f) },
                StateID = StaticStrings.LibertyStateID,
            },
            new IllicitMarketplace() {

                MenuID = "DealerHangoutMenu1",
                Name = "Dealer Hangout",
                Description = "Dealer Hangout 3",
                EntrancePosition = new Vector3(-376.9888f, 1180.735f, 204.746545f),
                EntranceHeading = 181.9404f,
                OpenTime = 18,
                CloseTime = 2,
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-376.9888f, 1180.735f, 204.746545f), 181.9404f) },
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.IllicitMarketplaces.AddRange(IllicitMarketplaceList_25);
    }


    private void DefaultConfig_DeadDrops()
    {
        List<DeadDrop> DeadDropList_4 = new List<DeadDrop>() {
            new DeadDrop(new Vector3(297.5366f, 295.988f, 14.72885f), 84.67217f,"Dead Drop", "the dumpster behind Fudz Diner") { OpenTime = 0,CloseTime = 24,IsEnabled = false,CanInteractWhenWanted = true,StateID = StaticStrings.LibertyStateID, },
            new DeadDrop() {
                Name = "Dead Drop",
                Description = "the dumpster behind the Pay'n'Spray in Outlook",
                EntrancePosition = new Vector3(1335.14f, 183.48f, 221.42f-200f),
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
                EntrancePosition = new Vector3(-56.96f,1874.73f,220.24f-200f),
                EntranceHeading = 84.76f,
                OpenTime = 0,
                CloseTime = 24,
                IsEnabled = false,
                CanInteractWhenWanted = true,
                StateID = StaticStrings.LibertyStateID,
            },
            new DeadDrop() {
                Name = "Dead Drop",
                Description = "under one of the big trees in the Colony Island graveyard parking lot",
                EntrancePosition = new Vector3(723.1f,781.87f,208.56f-200f),
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
                EntrancePosition = new Vector3(1698.11f,1023.4f,229.36f-200f),
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
                EntrancePosition = new Vector3(1368.7f,2080.5f,216.71f-200f),
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
                EntrancePosition = new Vector3(-86.15f,117.16f,192.01f-200f),
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
                EntrancePosition = new Vector3(-726.68f,2394.37f,222.51f-200f),
                EntranceHeading = 161.33f,
                OpenTime = 0,
                CloseTime = 24,
                IsEnabled = false,
                CanInteractWhenWanted = true,
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.DeadDrops.AddRange(DeadDropList_4);
    }
    private void DefaultConfig_GangDens()
    {
        float spawnChance = 45f;
        List<GangDen> LCMafiaDens = new List<GangDen>() {};
        GangDen GambettiDen1 = new GangDen(new Vector3(117.9776f, 237.2734f, 12.68547f), 91.83325f, "Gambetti Safehouse", "", "GambettiDenMenu", "AMBIENT_GANG_GAMBETTI")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            BannerImagePath = "gangs\\gambetti.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            StateID = StaticStrings.LibertyStateID,
            PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(116.7614f, 239.4333f, 12.68577f), 117.5893f, 55f) { TaskRequirements = TaskRequirements.Guard },//right by thingo
                    new GangConditionalLocation(new Vector3(116.0579f, 223.9395f, 12.72538f), 73.20693f, 55f){ TaskRequirements = TaskRequirements.Guard },
                },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(111.3983f, 218.4232f, 12.41919f), 1.662297f, 75f),
                }
        };//Little Italy Drrusellas
        GangDen PavanoDen1 = new GangDen(new Vector3(25.4392f, 1745.794f, 22.4494f), 359.2466f, "Pavano Safehouse", "", "PavanoDenMenu", "AMBIENT_GANG_PAVANO")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            BannerImagePath = "gangs\\pavano.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            MaxAssaultSpawns = 10,
            StateID = StaticStrings.LibertyStateID,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(29.01961f, 1747.132f, 22.26883f), 322.4916f, 65f),
                new GangConditionalLocation(new Vector3(31.25423f, 1740.376f, 22.36001f), 313.2928f, 65f),
                new GangConditionalLocation(new Vector3(16.37719f, 1747.927f, 23.13333f), 38.09092f, 65f),
                new GangConditionalLocation(new Vector3(36.76826f, 1747.077f, 22.02411f), 1.040331f, 65f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(33.99926f, 1751.237f, 21.87192f), 88.18089f, 75f),
            }
        };//East HOlland
        GangDen LupisellaDen1 = new GangDen(new Vector3(1642.493f, 2204.835f, 17.33413f), 270.3958f, "Lupisella Safehouse", "", "LupisellaDenMenu", "AMBIENT_GANG_LUPISELLA")
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
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(1641.377f, 2201.225f, 16.71346f), 244.8785f, 75f),
                new GangConditionalLocation(new Vector3(1640.861f, 2199.374f, 16.71352f), 272.0472f, 75f),
                new GangConditionalLocation(new Vector3(1644.047f, 2194.191f, 16.70902f), 276.2515f, 75f),
                new GangConditionalLocation(new Vector3(1644.123f, 2215.08f, 16.83813f), 286.4753f, 75f),
                new GangConditionalLocation(new Vector3(1662.775f, 2192.077f, 16.72728f), 60.91267f, 75f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(1667.617f, 2192.521f, 16.65262f), 185.392f, 75f),
                new GangConditionalLocation(new Vector3(1672.417f, 2192.223f, 16.65987f), 182.6264f, 75f),
            }
        };//Bohan Safehouse
        GangDen MessinaDen1 = new GangDen(new Vector3(-35.4356f, 1104.923f, 14.96771f), 359.9908f, "Messina Safehouse", "", "MessinaDenMenu", "AMBIENT_GANG_MESSINA")
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
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(-31.04382f, 1107.717f, 14.86459f), 6.084367f, 75f),
                new GangConditionalLocation(new Vector3(-39.05048f, 1107.48f, 14.84484f), 345.8169f, 75f),
                new GangConditionalLocation(new Vector3(-50.87765f, 1105.908f, 14.71404f), 26.121f, 75f),
                new GangConditionalLocation(new Vector3(-55.89527f, 1105.378f, 14.86431f), 349.0154f, 75f),
                new GangConditionalLocation(new Vector3(-17.65579f, 1106.781f, 14.86541f), 329.3782f, 75f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(-53.06663f, 1102.401f, 14.7076f), 0.8618788f, 75f),
            }
        };//star junction building by majestic
        GangDen AncelottiDen1 = new GangDen(new Vector3(-1226.005f, 919.8423f, 20.70229f), 91.75867f, "Ancelotti Safehouse", "", "AncelottiDenMenu", "AMBIENT_GANG_ANCELOTTI")
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
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(-1229.245f, 922.0545f, 19.56563f), 106.8709f, 75f),
                new GangConditionalLocation(new Vector3(-1230.677f, 912.7825f, 19.56561f), 73.49261f, 75f),
                new GangConditionalLocation(new Vector3(-1230.446f, 916.0844f, 19.56561f), 82.07771f, 75f),
                new GangConditionalLocation(new Vector3(-1225.514f, 910.3934f, 19.55676f), 76.71613f, 75f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(-1235.617f, 916.5689f, 19.0338f), 0.4052677f, 75f),
            }
        };//acter in aldery
        LCMafiaDens.Add(GambettiDen1);
        LCMafiaDens.Add(PavanoDen1);
        LCMafiaDens.Add(LupisellaDen1);
        LCMafiaDens.Add(MessinaDen1);
        LCMafiaDens.Add(AncelottiDen1);
        LibertyCityLocations.GangDens.AddRange(LCMafiaDens);


        GangDen LostMainDen = new GangDen(new Vector3(-1476.747f, 848.6306f, 26.33693f), 331.2753f, "Lost M.C. Clubhouse", "", "LostDenMenu", "AMBIENT_GANG_LOST")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            BannerImagePath = "gangs\\lostmc.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            MaxAssaultSpawns = 25,
            StateID = StaticStrings.AlderneyStateID,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(-1472.149f, 848.8211f, 25.44471f), 305.4796f, 75f),
                new GangConditionalLocation(new Vector3(-1477.812f, 852.5535f, 25.44471f), 340.0578f, 75f),
                new GangConditionalLocation(new Vector3(-1482.91f, 851.8522f, 25.44242f), 352.3685f, 75f),
                new GangConditionalLocation(new Vector3(-1486.606f, 858.3859f, 25.44471f), 299.3602f, 75f),
                new GangConditionalLocation(new Vector3(-1477.087f, 856.1881f, 25.44471f), 329.5667f, 75f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(-1471.557f, 853.3066f, 25.44471f), 245.9039f, 75f),
                new GangConditionalLocation(new Vector3(-1465.049f, 849.5966f, 25.02942f), 239.8058f, 75f),
            },
            VehiclePreviewLocation = new SpawnPlace(new Vector3(-1510.688f, 829.5602f, 25.02706f), 239.7735f),
            VehicleDeliveryLocations = new List<SpawnPlace>() {
                    new SpawnPlace(new Vector3(-1490.288f, 861.674f, 25.02752f), 55.41684f),
                    new SpawnPlace(new Vector3(-1498.98f, 867.0431f, 25.02857f), 58.18737f),
                }
        };

        LibertyCityLocations.GangDens.Add(LostMainDen);


        GangDen TriadMainDen = new GangDen(new Vector3(322.4745f, 98.86183f, 15.20138f), 182.0887f, "Triad Den", "", "TriadsDenMenu", "AMBIENT_GANG_WEICHENG")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            BannerImagePath = "gangs\\triad.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            MaxAssaultSpawns = 15,
            StateID = StaticStrings.LibertyStateID,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(319.0662f, 95.33968f, 14.76519f), 155.9272f, 75f),
                new GangConditionalLocation(new Vector3(325.3342f, 95.26466f, 14.76519f), 240.5921f, 75f),
                new GangConditionalLocation(new Vector3(332.0768f, 94.87815f, 14.76519f), 182.3631f, 75f),
                new GangConditionalLocation(new Vector3(313.3737f, 94.43867f, 14.76357f), 134.2366f, 75f),

            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {

            }
        };
        LibertyCityLocations.GangDens.Add(TriadMainDen);


        GangDen YardieDen1 = new GangDen(new Vector3(1456.727f, 604.7159f, 36.83776f), 273.4852f, "Yardies Chill Spot", "", "YardiesDenMenu", "AMBIENT_GANG_YARDIES")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            BannerImagePath = "gangs\\yardies.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            StateID = StaticStrings.LibertyStateID,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(1458.424f, 602.5609f, 36.42554f), 216.3955f, 75f),
                new GangConditionalLocation(new Vector3(1458.91f, 607.2742f, 36.269f), 293.9358f, 75f),
                new GangConditionalLocation(new Vector3(1458.515f, 615.5831f, 35.30829f), 292.8582f, 75f),
                new GangConditionalLocation(new Vector3(1465.213f, 620.0914f, 34.41097f), 57.3548f, 75f),
                new GangConditionalLocation(new Vector3(1464.749f, 595.3234f, 37.76828f), 174.5851f, 75f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(1463.775f, 624.2559f, 33.97698f), 355.722f, 75f),
                new GangConditionalLocation(new Vector3(1452.932f, 618.8113f, 34.93314f), 267.0037f, 75f),
            }
        };
        LibertyCityLocations.GangDens.Add(YardieDen1);

        GangDen SpanishLordsDen1 = new GangDen(new Vector3(996.7028f, 1865.197f, 14.88829f), 181.2785f, "Spanish Lords Den", "", "VarriosDenMenu", "AMBIENT_GANG_SPANISH")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            StateID = StaticStrings.LibertyStateID,
            BannerImagePath = "gangs\\petrovic.png",
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(998.1249f, 1863.178f, 14.24735f), 222.095f, 75f),
                new GangConditionalLocation(new Vector3(994.1127f, 1862.485f, 14.24717f), 135.7988f, 75f),
                new GangConditionalLocation(new Vector3(989.3598f, 1865.632f, 14.25257f), 99.79716f, 75f),
                new GangConditionalLocation(new Vector3(1007.358f, 1861.336f, 14.22742f), 261.6856f, 75f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {

            }
        };
        LibertyCityLocations.GangDens.Add(SpanishLordsDen1);

        GangDen KoreanDen1 = new GangDen(new Vector3(-927.1663f, 1546.542f, 13.7039f), 266.7077f, "Korean Mob Den", "", "KkangpaeDenMenu", "AMBIENT_GANG_KOREAN")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            MaxAssaultSpawns = 20,
            StateID = StaticStrings.AlderneyStateID,
            BannerImagePath = "gangs\\koreanmob.png",
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(-925.3502f, 1544.59f, 13.60226f), 211.9382f, 75f),
                new GangConditionalLocation(new Vector3(-922.923f, 1537.577f, 13.60214f), 245.0914f, 75f),
                new GangConditionalLocation(new Vector3(-924.1536f, 1551.493f, 13.60201f), 306.0096f, 75f),
                new GangConditionalLocation(new Vector3(-921.79f, 1554.534f, 13.60215f), 264.8454f, 75f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(-917.2498f, 1521.129f, 13.47092f), 204.1982f, 75f),
            }
        };
        LibertyCityLocations.GangDens.Add(KoreanDen1);


        GangDen NorthHollandDen1 = new GangDen(new Vector3(-167.3737f, 1901.367f, 12.33477f), 357.6532f, "The North Holland Hustlers Den", "The OGs", "FamiliesDenMenu", "AMBIENT_GANG_HOLHUST")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            StateID = StaticStrings.LibertyStateID,
            BannerImagePath = "gangs\\hollandhusters.png",
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(-165.5883f, 1902.758f, 12.31431f), 307.6587f, 75f),
                new GangConditionalLocation(new Vector3(-173.9109f, 1904.159f, 12.83977f), 23.44327f, 75f),
                new GangConditionalLocation(new Vector3(-180.7313f, 1902.531f, 13.42876f), 2.435143f, 75f),
                new GangConditionalLocation(new Vector3(-179.3212f, 1890.959f, 13.66216f), 291.3142f, 75f),
                new GangConditionalLocation(new Vector3(-174.2546f, 1882.677f, 14.37876f), 108.8811f, 75f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(-176.8966f, 1895.208f, 13.26385f), 359.4119f, 75f),
            }
        };
        LibertyCityLocations.GangDens.Add(NorthHollandDen1);


        GangDen PetrovicDen1 = new GangDen(new Vector3(1184.42f, 67.03922f, 16.17647f), 274.33f, "Petrovic Safehouse", "", "GambettiDenMenu", "AMBIENT_GANG_PETROVIC")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            MaxAssaultSpawns = 10,
            StateID = StaticStrings.LibertyStateID,
            BannerImagePath = "gangs\\petrovic.png",
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(1185.986f, 64.35268f, 16.10061f), 212.265f, 75f),
                new GangConditionalLocation(new Vector3(1188.175f, 71.32628f, 16.17285f), 273.377f, 75f),
                new GangConditionalLocation(new Vector3(1194.209f, 58.05311f, 15.95746f), 280.714f, 75f),
                new GangConditionalLocation(new Vector3(1195.461f, 71.88729f, 15.52232f), 177.8928f, 75f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(1198.347f, 65.8954f, 15.54268f), 266.819f, 75f),
                new GangConditionalLocation(new Vector3(1208.59f, 59.09538f, 15.9755f), 263.8091f, 75f),
            }
        };
        LibertyCityLocations.GangDens.Add(PetrovicDen1);


        GangDen AngelsDen = new GangDen(new Vector3(-385.5903f, 1701.063f, 6.09363f), 356.4827f, "Angles of Death Clubhouse", "", "LostDenMenu", "AMBIENT_GANG_ANGELS")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            MaxAssaultSpawns = 25,
            StateID = StaticStrings.LibertyStateID,
            BannerImagePath = "gangs\\angelsofdeath.png",
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(-383.5614f, 1702.414f, 6.055346f), 302.9006f, 75f),
                new GangConditionalLocation(new Vector3(-392.6819f, 1704.269f, 6.059426f), 54.89993f, 75f),
                new GangConditionalLocation(new Vector3(-397.4105f, 1702.661f, 6.059423f), 25.53698f, 75f),
                new GangConditionalLocation(new Vector3(-386.3522f, 1719.108f, 6.044559f), 107.3011f, 75f),
                new GangConditionalLocation(new Vector3(-383.4299f, 1712.491f, 6.046339f), 224.3586f, 75f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(-408.7182f, 1698.409f, 5.908772f), 176.1779f, 75f),
                new GangConditionalLocation(new Vector3(-405.0932f, 1698.177f, 5.909182f), 181.1461f, 75f),
            },
            VehiclePreviewLocation = new SpawnPlace(new Vector3(-398.0585f, 1720.009f, 5.908676f), 186.0972f),
            VehicleDeliveryLocations = new List<SpawnPlace>() {
                new SpawnPlace(new Vector3(-421.4973f, 1707.647f, 5.908159f), 253.3437f),
                new SpawnPlace(new Vector3(-425.2538f, 1717.559f, 5.908561f), 299.1917f),
                }
        };
        LibertyCityLocations.GangDens.Add(AngelsDen);

        GangDen UptownDen = new GangDen(new Vector3(25.66632f, 2129.53f, 18.74488f), 356.3734f, "Uptown Riders Clubhouse", "", "LostDenMenu", "AMBIENT_GANG_UPTOWN")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            MaxAssaultSpawns = 25,
            StateID = StaticStrings.LibertyStateID,
            BannerImagePath = "gangs\\uptownriders.png",
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(29.11273f, 2131.709f, 18.74378f), 338.1963f, 75f),
                new GangConditionalLocation(new Vector3(20.93227f, 2132.389f, 18.75438f), 54.82221f, 75f),
                new GangConditionalLocation(new Vector3(2.554915f, 2130.033f, 18.71214f), 21.89129f, 75f),
                new GangConditionalLocation(new Vector3(1.660987f, 2150.447f, 18.71214f), 156.5231f, 75f),
                new GangConditionalLocation(new Vector3(34.62012f, 2147.573f, 18.71214f), 218.4744f, 75f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(26.34993f, 2150.096f, 18.7349f), 177.5702f, 75f),
                new GangConditionalLocation(new Vector3(17.08085f, 2149.277f, 18.73664f), 175.8186f, 75f),
            },
            VehiclePreviewLocation = new SpawnPlace(new Vector3(5.633491f, 2140.862f, 18.51185f), 262.1412f),
            VehicleDeliveryLocations = new List<SpawnPlace>()
            {
                new SpawnPlace(new Vector3(31.26602f, 2137.29f, 18.61054f), 87.70872f),
                new SpawnPlace(new Vector3(31.61404f, 2143.351f, 18.58676f), 85.14955f),
            }
        };
        LibertyCityLocations.GangDens.Add(UptownDen);

    }
    private void DefaultConfig_Banks()
    {
        Bank Lombank1 = new Bank(new Vector3(1188.084f, -60.13863f, 14.61794f), 133.4215f, "Lombank", "Our time is your money", "Lombank")
        {
            BannerImagePath = "stores\\lombank.png",
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 6, 
            CloseTime = 20
        };
        LibertyCityLocations.Banks.Add(Lombank1);
        Bank Lombank2 = new Bank(new Vector3(-1061.145f, 1302.541f, 19.57709f), 88.54723f, "Lombank", "Our time is your money", "Lombank")
        {
            BannerImagePath = "stores\\lombank.png",
            StateID = StaticStrings.AlderneyStateID,
            OpenTime = 6,
            CloseTime = 20
        };
        LibertyCityLocations.Banks.Add(Lombank2);
        Bank Lombank3 = new Bank(new Vector3(207.6573f, -114.1872f, 14.76434f), 92.47134f, "Lombank", "Our time is your money", "Lombank")
        {
            BannerImagePath = "stores\\lombank.png",
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 6,
            CloseTime = 20
        };
        LibertyCityLocations.Banks.Add(Lombank3);
        Bank Lombank4 = new Bank(new Vector3(-160.6019f, 1691.884f, 14.76021f), 224.3296f, "Lombank", "Our time is your money", "Lombank")
        {
            BannerImagePath = "stores\\lombank.png",
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 6,
            CloseTime = 20
        };
        LibertyCityLocations.Banks.Add(Lombank4);
        Bank Fleeca1 = new Bank(new Vector3(211.6803f, -58.01148f, 16.23296f), 132.2257f, "Fleeca", "Everything, at a price", "Fleeca")
        {
            BannerImagePath = "stores\\fleeca.png",
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 6,
            CloseTime = 20
        };
        LibertyCityLocations.Banks.Add(Fleeca1);
        Bank WhanQBank = new Bank(new Vector3(1509.479f, 892.1535f, 22.2849f), 181.0821f, "Whan-Q Bank", "Don't beat off, Come Inside!", "WHANQ")
        {
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 6,
            CloseTime = 20
        };
        LibertyCityLocations.Banks.Add(WhanQBank);




        ATMMachine Lombankatm1 = new ATMMachine(new Vector3(1175.552f, -61.24877f, 14.12093f), 1.822495f, "Lombank", "Our time is your money","None",null,null)
        {
            BannerImagePath = "stores\\lombank.png",
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 0,
            CloseTime = 24,
        };
        LibertyCityLocations.ATMMachines.Add(Lombankatm1);
        ATMMachine Lombank4atm1 = new ATMMachine(new Vector3(-168.121f, 1683.874f, 14.76021f), 48.62359f, "Lombank", "Our time is your money", "None", null, null)
        {
            BannerImagePath = "stores\\lombank.png",
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 0,
            CloseTime = 24,
        };
        LibertyCityLocations.ATMMachines.Add(Lombank4atm1);
        ATMMachine Lombank4atm2 = new ATMMachine(new Vector3(-155.2926f, 1696.743f, 14.76021f), 45.13586f, "Lombank", "Our time is your money", "None", null, null)
        {
            BannerImagePath = "stores\\lombank.png",
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 0,
            CloseTime = 24,
        };
        LibertyCityLocations.ATMMachines.Add(Lombank4atm2);




        ATMMachine bolatm1 = new ATMMachine(new Vector3(1396.733f, 260.615f, 23.02633f), 176.1504f, "Bank Of Liberty", "Bleeding you dry", "None", null, null)
        {
            BannerImagePath = "stores\\lombank.png",
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 0,
            CloseTime = 24,
        };
        LibertyCityLocations.ATMMachines.Add(bolatm1);
        ATMMachine bolatm2 = new ATMMachine(new Vector3(1556.991f, 466.8633f, 28.24726f), 273.719f, "Bank Of Liberty", "Bleeding you dry", "None", null, null)
        {
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 0,
            CloseTime = 24,
        };
        LibertyCityLocations.ATMMachines.Add(bolatm2);
        ATMMachine bolatm3 = new ATMMachine(new Vector3(1436.292f, 1129.586f, 38.6717f), 1.147391f, "Bank Of Liberty", "Bleeding you dry", "None", null, null)
        {
            StateID = StaticStrings.LibertyStateID,
            OpenTime = 0,
            CloseTime = 24,
        };
        LibertyCityLocations.ATMMachines.Add(bolatm3);

    }
    private void DefaultConfig_Pharmacies()
    {
        LibertyCityLocations.Pharmacies.AddRange(new List<Pharmacy>()
        {
            new Pharmacy(new Vector3(-1226.868f, 1062.444f, 19.5674f), 80.39563f, "AYCEHOL", "","PharmacyMenu") { StateID = StaticStrings.AlderneyStateID },


            new Pharmacy(new Vector3(1166.186f, 85.03699f, 16.73859f), 90.65749f, "Hove Beach Pharmacy", "","PharmacyMenu") { StateID = StaticStrings.LibertyStateID },
            new Pharmacy(new Vector3(1211.575f, 88.40671f, 15.8945f), 2.371934f, "Hove Beach Pharmacy", "","PharmacyMenu") { StateID = StaticStrings.LibertyStateID },

            new Pharmacy(new Vector3(391.5427f, 194.5181f, 14.75278f), 177.306f, "Pill Pharm", "","PharmacyMenu") { StateID = StaticStrings.LibertyStateID },
            new Pharmacy(new Vector3(350.33f, 428.8872f, 14.76949f), 273.5124f, "Pill Pharm", "","PharmacyMenu") { StateID = StaticStrings.LibertyStateID },
            new Pharmacy(new Vector3(307.8423f, 271.3829f, 14.81721f), 271.8366f, "Pill Pharm", "","PharmacyMenu") { StateID = StaticStrings.LibertyStateID },
            new Pharmacy(new Vector3(-150.7478f, 1899.981f, 12.27885f), 299.9541f, "Pill Pharm", "","PharmacyMenu") { StateID = StaticStrings.LibertyStateID },
            new Pharmacy(new Vector3(-143.1255f, 1004.039f, 14.76909f), 268.809f, "Pill Pharm", "","PharmacyMenu") { StateID = StaticStrings.LibertyStateID },

            new Pharmacy(new Vector3(1413.217f, 261.5234f, 23.47336f), 359.0964f, "Pill Pharm", "","PharmacyMenu") { StateID = StaticStrings.LibertyStateID },
            new Pharmacy(new Vector3(1627.619f, 636.0948f, 28.46016f), 65.32055f, "Drugs", "","PharmacyMenu") { StateID = StaticStrings.LibertyStateID },

            new Pharmacy(new Vector3(1482.59f, 520.0873f, 36.07051f), 181.6015f, "Greavsy's Pharmacy", "","PharmacyMenu") { StateID = StaticStrings.LibertyStateID },
            new Pharmacy(new Vector3(2035.604f, 966.4045f, 28.03109f), 266.7413f, "Fullon Drugs", "","PharmacyMenu") { StateID = StaticStrings.LibertyStateID },
            new Pharmacy(new Vector3(1454.844f, 867.1164f, 21.94626f), 271.8613f, "Drugs", "","PharmacyMenu") { OpenTime = 0, CloseTime = 24, StateID = StaticStrings.LibertyStateID },
        });
    }
    private void DefaultConfig_HardwardStores()
    {
        LibertyCityLocations.HardwareStores.AddRange(new List<HardwareStore>()
        {
            new HardwareStore(new Vector3(-1439.889f, 772.345f, 20.74955f), 175.5223f, "Hardware Store", "","ToolMenu"){ StateID = StaticStrings.AlderneyStateID },
            new HardwareStore(new Vector3(1133.433f, -61.76066f, 14.17166f), 229.4212f, "645 Hardware", "","ToolMenu"){ StateID = StaticStrings.LibertyStateID },
            new HardwareStore(new Vector3(1249.492f, 179.6172f, 21.37342f), 94.49049f, "645 Hardware", "","ToolMenu"){ StateID = StaticStrings.LibertyStateID },

            new HardwareStore(new Vector3(1468.016f, 597.6271f, 38.65081f), 155.6264f, "D.I.Y", "In Your Arms Tonight","ToolMenu"){ StateID = StaticStrings.LibertyStateID },
            new HardwareStore(new Vector3(1469.622f, 571.1069f, 37.92224f), 358.6304f, "Discount Hardware", "","ToolMenu"){ StateID = StaticStrings.LibertyStateID },

            new HardwareStore(new Vector3(1106.917f, 323.2086f, 5.850764f), 315.2474f, "Builders Supplies", "","ToolMenu"){ StateID = StaticStrings.LibertyStateID },
            new HardwareStore(new Vector3(957.6523f, 2227.746f, 36.95779f), 202.1459f, "Hardware & Houseware", "","ToolMenu"){ StateID = StaticStrings.LibertyStateID },
        });
    }
    private void DefaultConfig_ClothingShops()
    {
        LibertyCityLocations.ClothingShops.AddRange(new List<ClothingShop>()
        {
            new ClothingShop(new Vector3(1131.292f, 50.72863f, 15.8602f), 274.2502f, "Russian Store", "","", new Vector3(1118.766f, 54.50769f, 15.85339f)) { StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(1127.265f, 46.71912f, 15.84883f), 355.0438f) } },
            new ClothingShop(new Vector3(1166.561f, 233.396f, 19.33681f), 89.94569f, "Jewelry Store", "","", Vector3.Zero) { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            new ClothingShop(new Vector3(248.9821f, -164.9303f, 14.76574f), 4.162239f, "Perseus", "","", new Vector3(245.6139f, -177.8401f, 14.86145f)) { IsTemporarilyClosed = true ,StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(249.2022f, -175.4658f, 14.86148f), 358.7083f) } },
            new ClothingShop(new Vector3(260.4522f, 1303.977f, 14.71333f), 1.235396f, "Perseus", "","", new Vector3(257.0355f, 1290.913f, 14.76171f)) { IsTemporarilyClosed = true , StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(260.4872f, 1293.259f, 14.76175f), 357.0343f) } },
            new ClothingShop(new Vector3(-42.46121f, 1848.396f, 25.63392f), 180.3579f, "Modo", "","", new Vector3(-37.88146f, 1858.274f, 25.63278f)) { IsTemporarilyClosed = true , StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-42.45911f, 1859.056f, 25.63309f), 177.3688f) } },
            new ClothingShop(new Vector3(1595.022f, 621.552f, 31.28567f), 240.849f, "Street Clothes", "","", Vector3.Zero) { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            new ClothingShop(new Vector3(1346.353f, -60.4495f, 13.52487f), 183.2335f, "Shirt World", "","", Vector3.Zero) { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            new ClothingShop(new Vector3(1492.907f, 417.7603f, 28.25696f), 90.98744f, "Ladies Fashion", "Clothes for Ladies","", Vector3.Zero) { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            new ClothingShop(new Vector3(1410.216f, 643.3562f, 32.54807f), 0.2708052f, "Derriere", "","", Vector3.Zero) { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            
            new ClothingShop(new Vector3(1548.968f, 942.1575f, 24.81256f), 86.65789f, "Jim's Jewels of Nondon", "","", Vector3.Zero) { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            new ClothingShop(new Vector3(1549.411f, 911.0147f, 22.64925f), 88.57362f, "Anna Jewelers", "","", Vector3.Zero) { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            new ClothingShop(new Vector3(1550.994f, 892.7451f, 22.44084f), 180.5751f, "Mama Jewelers", "","", Vector3.Zero) { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            new ClothingShop(new Vector3(1589.987f, 869.4638f, 22.78535f), 359.0088f, "Silk-N-Gold Jewelers", "","", Vector3.Zero) { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            new ClothingShop(new Vector3(1443.815f, 1244.394f, 36.11818f), 5.438942f, "Sexy Jewelry", "","", Vector3.Zero) { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            new ClothingShop(new Vector3(1982.235f, 923.6902f, 27.32143f), 91.85303f, "Clothes Corner", "","", Vector3.Zero) { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            new ClothingShop(new Vector3(1290.16f, 655.2253f, 31.87268f), 63.1114f, "Sneaky Footwear", "","", Vector3.Zero) { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            new ClothingShop(new Vector3(1356.416f, 548.6938f, 34.18115f), 88.93094f, "Girly Girl Ladies Fashion & Shoes", "","", Vector3.Zero) { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            new ClothingShop(new Vector3(1300.863f, 619.5544f, 33.59661f), 181.0713f, "Aeris", "","", Vector3.Zero) { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            new ClothingShop(new Vector3(1276.365f, 633.8016f, 32.15121f), 61.06564f, "Curbcrawler Skateboards", "","", Vector3.Zero) { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            new ClothingShop(new Vector3(1316.394f, 645.1326f, 32.63475f), 266.2737f, "Crevis", "","", Vector3.Zero) { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },


            new ClothingShop(new Vector3(688.3388f, 2183.006f, 16.25481f), 244.401f, "Discount Fashion", "","", Vector3.Zero) { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            new ClothingShop(new Vector3(711.078f, 2208.861f, 16.29925f), 139.2149f, "Ranch", "","", Vector3.Zero) { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
            new ClothingShop(new Vector3(968.1101f, 2058.304f, 22.26655f), 274.5147f, "El Pedro Jewelry", "","", Vector3.Zero) { StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
        });
    }
    private void DefaultConfig_LiquorStore()
    {
        LibertyCityLocations.LiquorStores.AddRange(new List<LiquorStore>()
        {
            new LiquorStore(new Vector3(1059.693f, 114.838f, 14.0993f), 271.99f, "Wine & Liquor", "","LiquorStoreMenu") { StateID = StaticStrings.LibertyStateID, },
            new LiquorStore(new Vector3(1166.318f, 39.6803f, 15.73573f), 88.17412f, "Wine & Liquor", "","LiquorStoreMenu") { StateID = StaticStrings.LibertyStateID, },
            new LiquorStore(new Vector3(350.2346f, 1565.316f, 14.71858f), 271.9344f, "Desratar Wine Store", "","LiquorStoreMenu") { StateID = StaticStrings.LibertyStateID, },
            new LiquorStore(new Vector3(-62.34354f, 1768.754f, 23.97709f), 176.9975f, "Lady Liquor", "","LiquorStoreMenu") { StateID = StaticStrings.LibertyStateID, },


            new LiquorStore(new Vector3(1529.293f, 480.4813f, 28.35725f), 243.7716f, "Bob's Liquors", "","LiquorStoreMenu") { StateID = StaticStrings.LibertyStateID, },
            new LiquorStore(new Vector3(1648.498f, 717.5427f, 26.1545f), 269.9614f, "Liquor", "","LiquorStoreMenu") { StateID = StaticStrings.LibertyStateID, },
            new LiquorStore(new Vector3(1433.099f, 309.8818f, 26.08633f), 268.741f, "Nik's Liquor & Wine", "","LiquorStoreMenu") { StateID = StaticStrings.LibertyStateID, },
            new LiquorStore(new Vector3(1594.681f, 362.6383f, 22.65849f), 186.0302f, "Jeff's Liquor & Wine", "Wash Those Cares Away!","LiquorStoreMenu") { StateID = StaticStrings.LibertyStateID, },


            new LiquorStore(new Vector3(1378.083f, 914.7954f, 29.81077f), 68.52505f, "Oriental Liquors", "","LiquorStoreMenu") { StateID = StaticStrings.LibertyStateID, },
            new LiquorStore(new Vector3(1500.053f, 892.6182f, 22.20075f), 178.4071f, "Oriental Liquors", "","LiquorStoreMenu") { StateID = StaticStrings.LibertyStateID, },
            new LiquorStore(new Vector3(996.9259f, 2046.939f, 21.60692f), 311.8166f, "N.O.N. Wines & Liquors", "","LiquorStoreMenu") { StateID = StaticStrings.LibertyStateID, },
        });
    }
    private void DefaultConfig_Bars()
    {
        LibertyCityLocations.Bars.AddRange(new List<Bar>()
        {
            new Bar(new Vector3(-1270.665f, 1033.902f, 19.56606f), 271.1126f, "Leprechauns Winklepicker", "Take a bite of our fanny","FancyFishMenu") { StateID = StaticStrings.AlderneyStateID },
            new Bar(new Vector3(-161.7981f, 890.8474f, 14.4101f), 178.4913f, "Bahama Mama's", "","BarMenu") { StateID = StaticStrings.LibertyStateID },
            new Bar(new Vector3(-295.2032f, 1846.985f, 17.47949f), 175.4991f, "Linen Lounge", "","BarMenu") { StateID = StaticStrings.LibertyStateID },
            new Bar(new Vector3(258.9634f, 57.45743f, 13.56177f), 89.61294f, "Big Boss Nightclub", "","BarMenu") { StateID = StaticStrings.LibertyStateID },

            new Bar(new Vector3(1432.41f, 305.5182f, 26.4194f), 270.3983f, "O'Reilly's Bar", "","BarMenu") { StateID = StaticStrings.LibertyStateID },
            new Bar(new Vector3(1670.621f, 513.1041f, 26.07587f), 85.41585f, "The Bay Bar", "","BarMenu") { StateID = StaticStrings.LibertyStateID },
            new Bar(new Vector3(1361.25f, 1164.545f, 38.02314f), 89.16649f, "8Ball Bar", "","BarMenu") { StateID = StaticStrings.LibertyStateID },
            new Bar(new Vector3(694.6603f, 2014.401f, 14.8535f), 10.01801f, "Whiskers", "","BarMenu") { StateID = StaticStrings.LibertyStateID },
        }
        );
    }
    private void DefaultConfig_Restaurants()
    {
        LibertyCityLocations.Restaurants.AddRange(new List<Restaurant>()
        {
            //Burger
            new Restaurant() {
                MenuID = "BurgerShotMenu",
                Name = "Burger Shot",
                Description = "Kill your hunger! It's bleedin' tasty",
                EntrancePosition = new Vector3(1890.87f, 719.66f, 25.21f),
                EntranceHeading = 269.4f,
                BannerImagePath = "stores\\burgershot.png",
                OpenTime = 6,
                CloseTime = 20,
                InteriorID = 124418,
                VendorPersonnelID = "BurgerShotPeds",
                VendorHeadDataGroupID = "AllHeads",
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(1875.883f, 718.8391f, 25.21189f), 267.9538f) },
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "BurgerShotMenu",
                Name = "Burger Shot",
                Description = "Kill your hunger! It's bleedin' tasty",
                EntrancePosition = new Vector3(680.63f, 2009.66f, 16.27f),
                EntranceHeading = 23.34f,
                BannerImagePath = "stores\\burgershot.png",
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
                InteriorID = 156674,
                VendorPersonnelID = "BurgerShotPeds",
                VendorHeadDataGroupID = "AllHeads",
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(688.6464f, 1996.255f, 16.31561f), 31.33071f) }
            },
            new Restaurant() {
                MenuID = "BurgerShotMenu",
                Name = "Burger Shot",
                Description = "Kill your hunger! It's bleedin' tasty",
                EntrancePosition = new Vector3(52.45f, 781.2f, 14.77f),
                EntranceHeading = 88.05f,
                OpenTime = 6,
                CloseTime = 20,
                BannerImagePath = "stores\\burgershot.png",
                VendorPersonnelID = "BurgerShotPeds",
                VendorHeadDataGroupID = "AllHeads",
                StateID = StaticStrings.LibertyStateID,
                InteriorID = 112642,
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(67.91145f, 781.9721f, 14.81999f), 90.90947f) }
            },
            new Restaurant() {
                MenuID = "BurgerShotMenu",
                Name = "Burger Shot",
                Description = "Kill your hunger! It's bleedin' tasty",
                EntrancePosition = new Vector3(-203.4f, 1687.77f, 13.05f),
                EntranceHeading = 95.42f,
                OpenTime = 6,
                CloseTime = 20,
                BannerImagePath = "stores\\burgershot.png",
                VendorPersonnelID = "BurgerShotPeds",
                VendorHeadDataGroupID = "AllHeads",
                StateID = StaticStrings.LibertyStateID,
                InteriorID = 109570,
                VendorLocations = new List<SpawnPlace>() {new SpawnPlace(new Vector3(-188.1336f, 1688.44f, 13.04693f), 89.43073f) }
            },
            new Restaurant() {
                MenuID = "BurgerShotMenu",
                Name = "Burger Shot",
                Description = "Kill your hunger! It's bleedin' tasty",
                EntrancePosition = new Vector3(1338.58f, 2088.76f, 16.9f),
                EntranceHeading = 40.91f,
                OpenTime = 6,
                CloseTime = 20,
                BannerImagePath = "stores\\burgershot.png",
                VendorPersonnelID = "BurgerShotPeds",
                VendorHeadDataGroupID = "AllHeads",
                StateID = StaticStrings.LibertyStateID,

                InteriorID = 134402,
                VendorLocations = new List<SpawnPlace>() {new SpawnPlace(new Vector3(1350.033f, 2078.295f, 16.90743f), 45.12792f) }
            },
            new Restaurant() {
                MenuID = "BurgerShotMenu",
                Name = "Burger Shot",
                Description = "Kill your hunger! It's bleedin' tasty",
                EntrancePosition = new Vector3(-379.51f, 640.95f, 4.81f),
                EntranceHeading = 358.48f,
                OpenTime = 6,
                CloseTime = 20,
                BannerImagePath = "stores\\burgershot.png",
                VendorPersonnelID = "BurgerShotPeds",
                VendorHeadDataGroupID = "AllHeads",
                StateID = StaticStrings.LibertyStateID,

                InteriorID = 105986,
                VendorLocations = new List<SpawnPlace>() {new SpawnPlace(new Vector3(-379.1163f, 625.4515f, 4.811082f), 357.4249f) }
            },
            new Restaurant() {
                MenuID = "BurgerShotMenu",
                Name = "Burger Shot",
                Description = "Kill your hunger! It's bleedin' tasty",
                EntrancePosition = new Vector3(-767.85f, 2107.42f, 24.31f),
                EntranceHeading = 180.48f,
                OpenTime = 6,
                CloseTime = 20,
                BannerImagePath = "stores\\burgershot.png",
                VendorPersonnelID = "BurgerShotPeds",
                VendorHeadDataGroupID = "AllHeads",
                StateID = StaticStrings.AlderneyStateID,

                InteriorID = 59650,
                VendorLocations = new List<SpawnPlace>() {new SpawnPlace(new Vector3(-768.1967f, 2122.303f, 24.31391f), 179.2184f) },

                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new MerchantConditionalLocation(new Vector3(-759.7379f, 2105.595f, 24.16283f), 209.8278f, 55f) { TaskRequirements = TaskRequirements.AnyScenario | TaskRequirements.Guard },
                    new MerchantConditionalLocation(new Vector3(-772.5197f, 2105.695f, 24.16283f), 187.217f, 55f){ TaskRequirements = TaskRequirements.AnyScenario | TaskRequirements.Guard },
                },
            },

            //Chicken
            new Restaurant(new Vector3(-1270.044f, 1061.385f, 19.56771f), 270.0156f, "Cluckin' Bell", "Taste the cock","CluckinBellMenu", FoodType.Chicken | FoodType.FastFood) {
                StateID =StaticStrings.AlderneyStateID,
                OpenTime = 5,
                CloseTime = 23,
                BannerImagePath = "stores\\cluckin.png",
                VendorPersonnelID = "CluckinBellPeds",
                VendorHeadDataGroupID = "AllHeads",
            },
            new Restaurant(new Vector3(1429.49f, 868.19f, 25.1f),2.2f,"Cluckin' Bell","Taste the cock","CluckinBellMenu", FoodType.Chicken | FoodType.FastFood) {
                MenuID = "CluckinBellMenu",
                Name = "Cluckin' Bell",
                Description = "Taste the cock",
                OpenTime = 6,
                CloseTime = 20,
                BannerImagePath = "stores\\cluckin.png",
                VendorPersonnelID = "CluckinBellPeds",
                VendorHeadDataGroupID = "AllHeads",
                StateID = StaticStrings.LibertyStateID,

                InteriorID = 143874,
                VendorLocations = new List<SpawnPlace>() {new SpawnPlace(new Vector3(1423.652f, 853.5104f, 25.10769f), 4.485676f) }
            },
            new Restaurant(new Vector3(101.04f, 566.46f, 14.8f),103.41f,"Cluckin' Bell","Taste the cock","CluckinBellMenu", FoodType.Chicken | FoodType.FastFood) {
                OpenTime = 6,
                CloseTime = 20,
                BannerImagePath = "stores\\cluckin.png",
                StateID = StaticStrings.LibertyStateID,
                VendorPersonnelID = "CluckinBellPeds",
                VendorHeadDataGroupID = "AllHeads",
                InteriorID = 124162,
                VendorLocations = new List<SpawnPlace>() {new SpawnPlace(new Vector3(116.7789f, 564.6393f, 14.80746f), 103.3126f) },

                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new MerchantConditionalLocation(new Vector3(99.30434f, 560.9355f, 14.75878f), 162.5283f, 55f){ TaskRequirements = TaskRequirements.AnyScenario | TaskRequirements.Guard },
                    new MerchantConditionalLocation(new Vector3(108.3584f, 545.4147f, 14.78257f), 184.0074f, 55f){ TaskRequirements = TaskRequirements.AnyScenario | TaskRequirements.Guard },
                },
            },

            //Coffee
            new Restaurant(new Vector3(-1060.514f, 1380.222f, 19.56642f), 184.7214f, "Craigs Coffee Shop", "","CoffeeMenu", FoodType.Coffee){ StateID = StaticStrings.AlderneyStateID },
            new Restaurant(new Vector3(1119.733f, -62.86261f, 13.75284f), 181.5454f, "Hot Coffee Shop", "Wanna come inside?","CoffeeMenu", FoodType.Coffee){ StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(322.1688f, 455.7006f, 14.83659f), 179.5892f,  "Bean Machine Coffee", "Taking over the world one Gunkaccino at a time","BeanMachineMenu", FoodType.Coffee | FoodType.Bagels | FoodType.Donut) { StateID = StaticStrings.LibertyStateID, BannerImagePath = "stores\\beanmachine.png" },
            new Restaurant(new Vector3(14.92347f, 457.2468f, 14.76526f), 219.7161f,  "Bean Machine Coffee", "Taking over the world one Gunkaccino at a time","BeanMachineMenu", FoodType.Coffee | FoodType.Bagels | FoodType.Donut) { StateID = StaticStrings.LibertyStateID, BannerImagePath = "stores\\beanmachine.png" },
            new Restaurant(new Vector3(169.3135f, 858.1989f, 14.77283f), 277.2627f,  "Bean Machine Coffee", "Taking over the world one Gunkaccino at a time","BeanMachineMenu", FoodType.Coffee | FoodType.Bagels | FoodType.Donut) { StateID = StaticStrings.LibertyStateID, BannerImagePath = "stores\\beanmachine.png" },
            new Restaurant(new Vector3(349.1903f, 488.417f, 14.82063f), 268.7347f,  "Stews Coffee Shop", "","CoffeeMenu", FoodType.Coffee) { StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(1532.004f, 427.4155f, 27.82907f), 356.5174f,  "Benito's Coffee and Comics", "","CoffeeMenu", FoodType.Coffee) { StateID = StaticStrings.LibertyStateID },

            //Donuts
            new Restaurant(new Vector3(436.5504f, 246.7506f, 10.31401f), 5.807813f, "Rusty Brown's", "Ring lickin' good!","RustyBrownsMenu", FoodType.Bagels | FoodType.Donut){ IsWalkup = true, StateID = StaticStrings.LibertyStateID, OpenTime = 4, CloseTime = 20,BannerImagePath = "stores\\rustybrowns.png" },

            //Ice Cream
            new Restaurant(new Vector3(1119.733f, -62.86261f, 13.75284f), 181.5454f, "Ned's Ice Cream", "","IceCreamMenu", FoodType.Dessert){ IsWalkup = true, StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(308.0074f, 316.5331f, 14.51933f), 304.3098f, "Fudz's Delicatessen", "Satisfaction served daily!","IceCreamMenu", FoodType.Dessert){ IsWalkup = true, StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(1470.412f, 1244.412f, 36.11839f), 359.4598f, "Cherry Popper Ice Cream", "","IceCreamMenu", FoodType.Dessert){ StateID = StaticStrings.LibertyStateID },

            //s.HO
            new Restaurant(new Vector3(1477.622f, 848.5602f, 21.41052f), 92.50406f, "S.Ho", "Become a real S. HO","NoodleMenu", FoodType.Korean) {BannerImagePath ="stores\\sho.png", ScannerFilePath ="01_specific_location\\0x1ABB2DE0.mp3" },

            //Wigwam
            new Restaurant(new Vector3(-63.62645f, 508.2122f, 14.70847f), 358.7118f, "Wigwam", "No need for reservations","WigwamMenu", FoodType.American | FoodType.Burger | FoodType.FastFood) { StateID =StaticStrings.AlderneyStateID, OpenTime = 6, CloseTime = 20, BannerImagePath = "stores\\wigwam.png", },
            
            //tw@
            new Restaurant(new Vector3(-1343.86f, 958.28f, 25.44f),63.7f,"tw@","nu-media caffeine solutions provider","InternetCafeMenu",FoodType.Snack) {
                BannerImagePath = "stores\\twat.png",
                OpenTime = 6,
                CloseTime = 20,
                InteriorID = 166914,
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-1337.807f, 960.3051f, 25.44378f), 146.1286f) },
                StateID = StaticStrings.AlderneyStateID,
                VendorPersonnelID = "TwatPeds",
                VendorHeadDataGroupID = "AllHeads",
            },
            new Restaurant(new Vector3(-101.45f, 1886.24f, 12.92f),88.88f,"tw@","nu-media caffeine solutions provider","InternetCafeMenu",FoodType.Snack) {
                BannerImagePath = "stores\\twat.png",
                OpenTime = 6,
                CloseTime = 20,
                InteriorID = 66562,
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-97.02089f, 1890.783f, 12.91313f), 176.6438f) },
                StateID = StaticStrings.LibertyStateID,
                VendorPersonnelID = "TwatPeds",
                VendorHeadDataGroupID = "AllHeads",
            },
            new Restaurant(new Vector3(1213.59f, 324.08f, 24.19f),280.19f,"tw@","nu-media caffeine solutions provider","InternetCafeMenu",FoodType.Snack) {
                OpenTime = 6,
                CloseTime = 20,
                InteriorID = 50178,
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(1210.156f, 318.7346f, 24.19368f), 14.776f) },
                StateID = StaticStrings.LibertyStateID,
                BannerImagePath = "stores\\twat.png",
                VendorPersonnelID = "TwatPeds",
                VendorHeadDataGroupID = "AllHeads",
            },

            //Italian
            new Restaurant(new Vector3(-1316.426f, 998.7839f, 25.72835f), 355.8445f, "Pizza This...", "Get stuffed","PizzaThisMenu", FoodType.Italian | FoodType.Pizza) { MapIcon = 889,TypeName ="Pizza", StateID = StaticStrings.AlderneyStateID ,BannerImagePath = "stores\\pizzathis.png" },
            new Restaurant(new Vector3(-105.2152f, 564.0861f, 14.81412f), 129.918f, "Al Dente's", "Just like mama never used to make it","AlDentesMenu", FoodType.Italian | FoodType.Pizza) { MapIcon = 889,TypeName ="Pizza",StateID = StaticStrings.LibertyStateID ,BannerImagePath = "stores\\aldentes.png" },
            new Restaurant(new Vector3(390.1504f, 18.13119f, 14.76246f), 90.53439f, "Al Dente's", "Just like mama never used to make it","AlDentesMenu", FoodType.Italian | FoodType.Pizza) { MapIcon = 889,TypeName ="Pizza",StateID = StaticStrings.LibertyStateID ,BannerImagePath = "stores\\aldentes.png" },
            new Restaurant(new Vector3(-161.157f, 187.2703f, 5.294047f), 143.4763f, "Pasta Le Vista", "Where every bite’s a getaway!","AlDentesMenu", FoodType.Italian | FoodType.Pizza) { MapIcon = 889,TypeName ="Pizza",StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(-176.5677f, 950.0205f, 12.40071f), 359.3528f, "Pasta Le Vista", "Where every bite’s a getaway!","AlDentesMenu", FoodType.Italian | FoodType.Pizza) { MapIcon = 889,TypeName ="Pizza",StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(366.7054f, 209.1764f, 14.98598f), 268.1589f, "Pasta Le Vista", "Where every bite’s a getaway!","AlDentesMenu", FoodType.Italian | FoodType.Pizza) { MapIcon = 889,TypeName ="Pizza",StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(1671.657f, 727.932f, 25.95271f), 87.05959f, "Al's Pizzeria", "Free Delivery","PizzaThisMenu", FoodType.Italian | FoodType.Pizza) { MapIcon = 889,TypeName ="Pizza",StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(1217.929f, 278.336f, 23.91655f), 321.847f, "Pizza Feast", "For a Real Feast","PizzaThisMenu", FoodType.Italian | FoodType.Pizza) { MapIcon = 889,TypeName ="Pizza",StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(2035.999f, 976.2667f, 28.14738f), 275.386f, "Pizza Feast", "For a Real Feast","PizzaThisMenu", FoodType.Italian | FoodType.Pizza) { MapIcon = 889,TypeName ="Pizza",StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(1373.067f, 896.9398f, 29.79031f), 74.50283f, "Italian Restaurant", "","AlDentesMenu", FoodType.Italian | FoodType.Pizza) { MapIcon = 889,TypeName ="Pizza",StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(1414.074f, 890.5122f, 25.53889f), 178.9763f, "Pizza & Deli", "","PizzaThisMenu", FoodType.Italian | FoodType.Pizza) { MapIcon = 889,TypeName ="Pizza",StateID = StaticStrings.LibertyStateID },     
            new Restaurant(new Vector3(591.9781f, 2205.841f, 17.17392f), 91.2168f, "Pizza", "","PizzaThisMenu", FoodType.Italian | FoodType.Pizza) { MapIcon = 889,TypeName ="Pizza",StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(779.9337f, 2013.644f, 15.67051f), 0.877494f, "Pizza & Gyro", "","PizzaThisMenu", FoodType.Italian | FoodType.Pizza) { MapIcon = 889,TypeName ="Pizza",StateID = StaticStrings.LibertyStateID },

            //Chinese
            new Restaurant(new Vector3(1505.384f, 425.5957f, 28.10432f), 358.0871f, "Green Dragon Chinese Food", "","NoodleMenu", FoodType.Asian){ StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(1563.347f, 364.9023f, 25.74242f), 182.9156f, "Wok Like An Egyptian", "","NoodleMenu", FoodType.Asian){ StateID = StaticStrings.LibertyStateID },

            //Sandwich
            new Restaurant(new Vector3(1313.493f, 576.736f, 34.20903f), 269.9303f, "Hero Shop", "","SandwichMenu", FoodType.Sandwiches){ StateID = StaticStrings.LibertyStateID },

            //Generic    
            new Restaurant(new Vector3(-1072.631f, 1380.906f, 19.56819f), 181.032f, "Healthy Food", "No bogeys about it","FancyGenericMenu", FoodType.American){ StateID = StaticStrings.AlderneyStateID },
            new Restaurant(new Vector3(107.2284f, 1900.707f, 20.42747f), 0.09704412f, "S&D Diner", "","FancyGenericMenu", FoodType.Generic){ IsWalkup = true, StateID = StaticStrings.LibertyStateID, OpenTime = 6, CloseTime = 20 },
            new Restaurant(new Vector3(1216.894f, 20.49133f, 16.10159f), 273.8801f, "Gulag Garden", "Fresh from the gulag","FancyGenericMenu", FoodType.Generic){ StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(1255.93f, 159.1167f, 19.50427f), 121.6593f, "Luncheonette Diner", "Fresh from the gulag","FancyGenericMenu", FoodType.Generic){ StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(1312.689f, 529.9832f, 34.01013f), 273.2307f, "Diner", "","FancyGenericMenu", FoodType.American){ StateID = StaticStrings.LibertyStateID },      
            new Restaurant(new Vector3(1375.332f, 905.6595f, 29.83094f), 75.98001f, "American & Spanish Food", "","FancyGenericMenu", FoodType.American){ StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(1397.564f, 890.1441f, 27.47992f), 181.499f, "Chicken Restaurant", "","FancyGenericMenu", FoodType.Chicken){ StateID = StaticStrings.LibertyStateID },    
            new Restaurant(new Vector3(1494.228f, 284.1609f, 25.23981f), 179.2637f, "Ali Mac's", "Traditional Familry Fare","FancyGenericMenu", FoodType.Generic){ StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(1436.119f, 259.1028f, 24.5536f), 270.7332f, "Coronary Corner Restuarant", "","FancyGenericMenu", FoodType.Generic){ StateID = StaticStrings.LibertyStateID },

            //Mexican
            new Restaurant(new Vector3(1030.558f, 143.9256f, 7.496456f), 178.6679f, "El Sobre", "","MexicanMenu", FoodType.Mexican){ StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(1579.539f, 364.8728f, 24.67028f), 178.3091f, "El Gringo Supremo's", "","MexicanMenu", FoodType.Mexican){ StateID = StaticStrings.LibertyStateID },

            //Korean
            new Restaurant(new Vector3(243.8853f, 120.3672f, 14.76545f), 90.41767f, "Rice & Fins", "New century fast food!","NoodleMenu", FoodType.Korean) { StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(346.6833f, 97.29707f, 14.76293f), 178.932f, "Dim Sum", "","NoodleMenu", FoodType.Korean) { StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(1979.965f, 1013.015f, 29.20576f), 49.90157f, "Silk Thai", "","NoodleMenu", FoodType.Asian){ StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(1549.102f, 924.5397f, 23.39649f), 90.51724f, "Kingston Restaurant", "","NoodleMenu", FoodType.Asian){ StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(1570.415f, 869.429f, 22.61242f), 2.052902f, "Indian Inn Restaurant", "","NoodleMenu", FoodType.Asian){ StateID = StaticStrings.LibertyStateID },


            //Seafood
            new Restaurant(new Vector3(-1274.281f, 1896.341f, 13.38669f), 357.4145f, "Fanny Crabs", "Take a bite of our fanny","FancyFishMenu", FoodType.Seafood) { StateID = StaticStrings.AlderneyStateID },
            new Restaurant(new Vector3(127.7461f, -534.7335f, 5.118071f), 185.1966f, "Poop Deck", "Seafood with a splash of salty humor","FancyFishMenu", FoodType.Seafood) { StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(287.4133f, 402.7248f, 15.83704f), 91.89918f, "Squid Row", "Get hooked on our big ones!","FancyFishMenu", FoodType.Seafood) { StateID = StaticStrings.LibertyStateID },
            new Restaurant(new Vector3(390.3653f, 246.1507f, 14.76013f), 86.89571f, "The Bearded Clam", "Dive in for a taste!","FancyFishMenu", FoodType.Seafood) { StateID = StaticStrings.LibertyStateID },
        });
    }
    private void DefaultConfig_Landmarks()
    {
        LibertyCityLocations.Landmarks.AddRange(new List<Landmark>()
        {
            new Landmark(new Vector3(-1274.281f, 1896.341f, 13.38669f), 357.4145f, "Schlongberg Sachs", "") { StateID = StaticStrings.AlderneyStateID },
            new Landmark(new Vector3(-1429.036f, 770.1529f, 20.177f), 176.9206f, "Satriale's Pork Store", "") { StateID = StaticStrings.AlderneyStateID },
            new Landmark(new Vector3(1195.817f, 203.6283f, 19.80799f), 182.1452f, "Perestroika", "") { BannerImagePath = "stores//perestroika.png", StateID = StaticStrings.LibertyStateID, InteriorID = 76290,IgnoreEntranceInteract = true, },
            new Landmark(new Vector3(1251.176f, 168.7538f, 20.23567f), 99.71825f, "Laundromat", "") { StateID = StaticStrings.LibertyStateID, InteriorID = 138498, IgnoreEntranceInteract = true, },
            new Landmark(new Vector3(1061.563f, 219.1207f, 15.34185f), 269.6217f, "Express Car Services", "") { BannerImagePath = "stores//expresscarservice.png",StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(-105.2177f, 393.1169f, 14.70457f), 90.96992f, "Rotterdam Tower", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(260.9259f, 454.8217f, 14.81293f), 182.7993f, "Grand Easton Terminal", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(-243.7175f, 1174.777f, 9.525352f), 86.67438f, "Randalf Art Center", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(206.7253f, 1070.856f, 14.66462f), 89.06384f, "Cleethorpes Tower", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(235.5885f, 901.4274f, 16.36535f), 182.2745f, "Columbus Cathedral", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(147.8214f, 1315.119f, 18.62602f), 265.9916f, "The Libertonian", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(406.73f, 861.5466f, 15.175f), 90.89143f, "Civilization Committee", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(-155.4752f, 1379.515f, 18.18823f), 269.9464f, "Natural History Museum", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(-284.2392f, 1539.379f, 15.50288f), 181.2027f, "Vespucci University", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(-341.9377f, 1327.754f, 12.82209f), 193.9257f, "Mausoleum", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(1685.215f, 634.5786f, 26.77688f), 357.5381f, "Beechwood Strip CLub", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(1742.516f, 611.2752f, 23.42088f), 96.11179f, "Car Wash N Lube", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(1589.831f, 248.0889f, 22.89358f), 249.1297f, "Swingset-O-Death", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(1317.437f, -184.1534f, 15.94499f), 36.31557f, "Funland", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(1437.048f, -185.5359f, 16.44068f), 178.8712f, "Memory Lanes", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(1340.066f, 422.8288f, 36.29833f), 178.2877f, "Soldier's Plaza", "To the defenders of the Union, 1861-1865") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(1377.24f, 457.3909f, 37.98289f), 144.9886f, "Broker Public Library", "Get that musty old book smell") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(1463.346f, 427.1826f, 28.85644f), 0.60189f, "Hanks Rent-A-Car", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(1468.75f, 412.0862f, 28.15043f), 270.7415f, "Seedy Rent-A-Van", "") { StateID = StaticStrings.LibertyStateID },


            new Landmark(new Vector3(1184.239f, 1381.721f, 23.11622f), 55.82177f, "Steinway Park", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(1458.338f, 1505.688f, 13.65962f), 230.6669f, "Gantry", "") { StateID = StaticStrings.LibertyStateID },
            //new Landmark(new Vector3(1599.037f, 1494.526f, 14.02291f), 229.9848f, "Dukes Bride Police 1", "") { StateID = StaticStrings.LibertyStateID },
            //new Landmark(new Vector3(1602.456f, 1499.64f, 14.02292f), 46.80567f, "Dukes Bridge Police 2", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(678.1191f, 2304.209f, 21.7027f), 111.7227f, "Welham Parkway Park", "") { StateID = StaticStrings.LibertyStateID },

            new Landmark(new Vector3(1489.198f, -86.42191f, 13.62094f), 359.5247f, "Beachgate Gate", "") 
            { 
                StateID = StaticStrings.LibertyStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new SecurityConditionalLocation(new Vector3(1489.062f, -91.56538f, 13.89239f), 273.5172f,95f) { AssociationID = "GRP6" },
                },
            },
            new Landmark(new Vector3(1301.529f, 319.8674f, 29.91103f), 257.7469f, "Outlook Park", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(1651.989f, 870.5731f, 19.03766f), 1.596962f, "Pillows Strip Club", "") { IsTemporarilyClosed = true, StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(1521.142f, 1097.451f, 38.99194f), 95.33509f, "24 LC", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(1626.669f, 1171.941f, 35.28984f), 267.5667f, "Meadows Park Church", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(1727.272f, 1173.833f, 29.37267f), 180.6092f, "Meadows Park Museum", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(2082.871f, 847.7047f, 22.5523f), 88.11333f, "Willis Wash N Lube", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(2018.704f, 937.2704f, 27.94974f), 0.9942721f, "After Dark Sex Shop", "") { OpenTime = 0,CloseTime =24, StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(2058.563f, 1060.605f, 29.0824f), 91.75621f, "Canyon Megaplex", "") { StateID = StaticStrings.LibertyStateID },
            new Landmark(new Vector3(1522.52f, 892.3311f, 22.40118f), 179.9744f, "JJ China Limited", "") { InteriorID = 24578, StateID = StaticStrings.LibertyStateID,IsTemporarilyClosed = true },
        });
    }
    private void DefaultConfig_SubwayStations()
    {
        LibertyCityLocations.SubwayStations.AddRange(new List<SubwayStation>()
        {
            new SubwayStation(new Vector3(1242.84f, -50.72684f, 14.91387f), 88.32802f,"Hove Beach Stop","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(308.1557f, -232.6066f, 4.957515f), 299.9689f,"Castle Garden","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(125.3678f, -7.447071f, 14.74463f), 268.1037f,"City Hall","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(202.4466f, 473.6303f, 14.76214f), 179.7628f,"Easton Station","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(202.5754f, 1129.601f, 14.65698f), 0.0795438f,"East Park","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(352.5324f, 190.8413f, 14.76215f), 88.94254f,"Emerald Station","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(-110.5889f, 156.5407f, 4.872895f), 223.6983f,"Feldspar Station","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(-139.1841f, 863.7511f, 14.75325f), 180.0942f,"Frankfort Avenue","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(-145.2569f, 1929.354f, 12.41804f), 359.6377f,"Frankfort High","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(-168.0409f, 1920.621f, 12.24848f), 272.5806f,"Frankfort Low","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(-310.9742f, 502.6911f, 4.696742f), 271.9692f,"Hematite Station","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(334.5522f, 1016.016f, 13.22489f), 271.9743f,"Manganese East","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(-223.2067f, 1022.376f, 9.851804f), 178.6376f,"Manganese West","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(105.0989f, 1647.643f, 14.76678f), 269.2798f,"North Park","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(374.0517f, 1406.466f, 14.71114f), 0.129852f,"Quartz St. East","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(-304.8709f, 1434.566f, 9.94086f), 270.5335f,"Quartz St. West","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(-17.76399f, 349.47f, 14.44452f), 90.58279f,"Suffolk Station","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(-247.2124f, 1827.577f, 17.47126f), 90.05782f,"Vauxite Station","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(168.7186f, 1905.537f, 20.41474f), 91.88655f,"Vespucci Circus","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(-112.0743f, 1171.449f, 14.68633f), 358.3388f,"West Park","") { StateID = StaticStrings.LibertyStateID, },
            new SubwayStation(new Vector3(1624.71f, 898.8407f, 22.94662f), 91.85176f, "Huntington St", "") { StateID = StaticStrings.LibertyStateID },
            new SubwayStation(new Vector3(1886.904f, 907.3792f, 31.21505f), 4.448621f, "Lynch St", "") { StateID = StaticStrings.LibertyStateID },
            new SubwayStation(new Vector3(646.8339f, 2153.647f, 16.23036f), 119.3207f, "San Quentin Ave", ""){ StateID = StaticStrings.LibertyStateID },
            new SubwayStation(new Vector3(988.4123f, 1941.501f, 14.24753f), 183.3002f, "Winmill St", ""){ StateID = StaticStrings.LibertyStateID },
        });
    }
    private void DefaultConfig_Sports()
    {
        LibertyCityLocations.SportingGoodsStores.AddRange(new List<SportingGoodsStore>()
        {
            new SportingGoodsStore(new Vector3(-1295.603f, 1896.611f, 13.17022f), 1.258699f, "Leftwood Sports", "Right our leftwood!", "VespucciSportsMenu")
            {
                StateID = StaticStrings.AlderneyStateID,
                VehiclePreviewLocation = new SpawnPlace(new Vector3(-1296.468f, 1909.253f, 13.04997f), 271.4031f),
                VehiclePreviewCameraPosition = new Vector3(-1296.634f, 1914.314f, 14.65471f),
                VehiclePreviewCameraDirection = new Vector3(-0.01058928f, -0.9803134f, -0.1971639f),
                VehiclePreviewCameraRotation = new Rotator(-11.37116f, 6.531513E-07f, 179.3811f),
                VehicleDeliveryLocations = new List<SpawnPlace>()
                    {
                        new SpawnPlace(new Vector3(-1301.706f, 1891.748f, 13.17023f), 2.433398f),
                    }
            },
             new SportingGoodsStore(new Vector3(-6.04857f, 1900.813f, 20.41824f), 359.3634f, "Sporting Corner", "Crevis Heat Prolaps", "VespucciSportsMenu")
            {
                StateID = StaticStrings.LibertyStateID,
                VehiclePreviewLocation = new SpawnPlace(new Vector3(-3.802421f, 1902.679f, 19.81482f), 57.26212f),
                VehiclePreviewCameraPosition = new Vector3(-4.888263f, 1905.166f, 21.12002f),
                VehiclePreviewCameraDirection = new Vector3(0.2524882f, -0.9221741f, -0.2929927f),
                VehiclePreviewCameraRotation = new Rotator(-17.03721f, -8.929615E-07f, -164.6879f),
                VehicleDeliveryLocations = new List<SpawnPlace>()
                    {
                        new SpawnPlace(new Vector3(-9.088917f, 1902.611f, 19.82075f), 86.86047f),
                    }
            },

            new SportingGoodsStore(new Vector3(255.1443f, 152.7309f, 14.77926f), 208.0794f, "People Sport", "", "VespucciSportsMenu")
            {
                StateID = StaticStrings.LibertyStateID,
                VehiclePreviewLocation = new SpawnPlace(new Vector3(257.4574f, 151.9436f, 14.14659f), 172.6058f),
                VehiclePreviewCameraPosition = new Vector3(259.384f, 150.0186f, 15.65188f),
                VehiclePreviewCameraDirection = new Vector3(-0.6260471f, 0.6886612f, -0.3658017f),
                VehiclePreviewCameraRotation = new Rotator(-21.45693f, -2.752058E-06f, 42.2733f),
                VehicleDeliveryLocations = new List<SpawnPlace>()
                    {
                        new SpawnPlace(new Vector3(260.6979f, 156.0527f, 14.1469f), 177.3499f),
                    }
            },
            new SportingGoodsStore(new Vector3(-35.38442f, 1799.093f, 24.73487f), 286.9402f, "Baskets", "", "VespucciSportsMenu")
            {
                StateID = StaticStrings.LibertyStateID,
                VehiclePreviewLocation = new SpawnPlace(new Vector3(-34.34518f, 1801.051f, 24.13146f), 319.212f),
                VehiclePreviewCameraPosition = new Vector3(-32.23541f, 1800.062f, 25.46301f),
                VehiclePreviewCameraDirection = new Vector3(-0.8196946f, 0.4454925f, -0.3600516f),
                VehiclePreviewCameraRotation = new Rotator(-21.10336f, 8.236356E-06f, 61.47654f),
                VehicleDeliveryLocations = new List<SpawnPlace>()
                    {
                        new SpawnPlace(new Vector3(-34.45256f, 1796.82f, 24.12298f), 209.1293f),
                    }
            },

            new SportingGoodsStore(new Vector3(2059.692f, 1015.153f, 28.7499f), 89.81208f, "Bicycles", "", "BourgeoisBicyclesMenu")
            {
                StateID = StaticStrings.LibertyStateID,
                VehiclePreviewLocation = new SpawnPlace(new Vector3(2059.692f, 1015.153f, 28.7499f), 89.81208f),
                VehicleDeliveryLocations = new List<SpawnPlace>()
                    {
                        new SpawnPlace(new Vector3(2059.692f, 1015.153f, 28.7499f), 89.81208f),
                    }
            },
        });
    }
    private void DefaultConfig_Prisons()
    {
        List<Prison> LCPrison = new List<Prison>()
        {
            new Prison(new Vector3(-903.9021f, 118.7461f, 3.080931f), 91.96844f, "Alderney State Correctional Facility","") { OpenTime = 0,CloseTime = 24, StateID = StaticStrings.AlderneyStateID },
        };
        LibertyCityLocations.Prisons.AddRange(LCPrison);
    }
    private void DefaultConfig_CityHalls()
    {
        List<CityHall> CityHalls = new List<CityHall>()
        { 
            new CityHall(new Vector3(-971.8344f, 1485.335f, 20.59385f), 179.2278f,"Alderney City Hall","") { OpenTime = 9,CloseTime = 18, StateID = StaticStrings.AlderneyStateID }
        
        
        };
        LibertyCityLocations.CityHalls.AddRange(CityHalls);
    }
    private void DefaultConfig_Hospitals()
    {
        List<Hospital> Hospitals = new List<Hospital>()
        {
            //Broker
            new Hospital(new Vector3(1438.552f, 690.5808f, 33.54961f), 90.19894f, "Schottler Medical Center","") { OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            //Dukes
            new Hospital(new Vector3(1486.106f, 977.1287f, 29.53525f), 33.23054f, "Cerveza Heights Medical Center","") { OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            //Bohan
            new Hospital(new Vector3(1220.561f, 2333.059f, 23.89302f), 92.1476f, "Bohan Medical & Dental Center","") { OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            //Algon
            new Hospital(new Vector3(-186.4241f, 1797.527f, 17.43241f), 85.08374f, "Holland Hospital Center","") { OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            new Hospital(new Vector3(334.043f, 643.3015f, 14.77393f), 178.5839f, "Lancet-Hospital Center","") { OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            new Hospital(new Vector3(51.70538f, -64.34246f, 4.941999f), 89.92042f, "City Hall Hospital","") { OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            //Alderny
            new Hospital(new Vector3(-1080.693f, 1769.972f, 23.37073f), 309.4161f, "Westdyke Memorial Hospital","") { OpenTime = 0,CloseTime = 24, StateID = StaticStrings.AlderneyStateID },
            new Hospital(new Vector3(-1277.354f, 890.3488f, 21.63032f), 313.0107f, "North Tudor Medical Center","") { OpenTime = 0,CloseTime = 24, StateID = StaticStrings.AlderneyStateID,
            
            AssignedAssociationID = "FDLC-EMS",
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new EMSConditionalLocation(new Vector3(-1279.262f, 895.407f, 19.80547f), 50.27678f, 25f),
                new EMSConditionalLocation(new Vector3(-1270.076f, 891.6854f, 19.56506f), 303.5465f, 25f),
                new EMSConditionalLocation(new Vector3(-1297.261f, 867.7f, 21.92857f), 62.32174f, 25f),
                new EMSConditionalLocation(new Vector3(-1297.161f, 862.2394f, 21.96812f), 137.8788f, 25f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new EMSConditionalLocation(new Vector3(-1326.832f, 861.529f, 23.35665f), 87.69072f, 25f),
                new EMSConditionalLocation(new Vector3(-1326.449f, 855.0043f, 23.35664f), 87.08761f, 25f),
                new EMSConditionalLocation(new Vector3(-1307.086f, 851.9523f, 23.35664f), 269.4883f, 25f),
                new EMSConditionalLocation(new Vector3(-1307.871f, 879.6849f, 23.35665f), 269.1915f, 25f),
            },

            
            
            
            
            
            },

            //North Yankton
            new Hospital(new Vector3(3132.073f, -4839.958f, 112.0312f), 354.8388f, "Ludendorff Clinic", "The service you'd expect!") { StateID = StaticStrings.NorthYanktonStateID, OpenTime = 0,CloseTime = 24,
                PossiblePedSpawns = new List<ConditionalLocation>() {
            }
            ,RespawnLocation = new Vector3(3132.073f, -4839.958f, 112.0312f),RespawnHeading = 354.8388f },

            //Clinic
            new Hospital(new Vector3(1216.88f, 44.20709f, 16.18041f), 271.8641f, "Russian Clinic","") { OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID },
        };
        LibertyCityLocations.Hospitals.AddRange(Hospitals);
    }
    private void DefaultConfig_FoodStands()
    {
        LibertyCityLocations.FoodStands.AddRange(new List<FoodStand>()
        {
           // new FoodStand(new Vector3(403.3527f, 106.0655f, 101.4575f), 241.199f, "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(403.3527f, 106.0655f, 101.4575f), 241.199f) }, BannerImagePath = "stores\\beefybills.png" },

            new FoodStand(new Vector3(-1188.392f, 1370.67f, 19.55794f), 264.8223f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.AlderneyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-1188.392f, 1370.67f, 19.55794f), 264.8223f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new FoodStand(new Vector3(-1022.393f, 1376.817f, 19.56642f), 176.6684f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.AlderneyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-1022.393f, 1376.817f, 19.56642f), 176.6684f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new FoodStand(new Vector3(-1026.179f, 1347.799f, 19.56661f), 356.207f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.AlderneyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-1026.179f, 1347.799f, 19.56661f), 356.207f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new FoodStand(new Vector3(699.7291f, -25.29841f, 5.818743f), 269.1714f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(699.7291f, -25.29841f, 5.418743f), 269.1714f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new FoodStand(new Vector3(604.3338f, -20.10917f, 5.818576f), 88.84018f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(604.3338f, -20.10917f, 5.418576f), 88.84018f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new FoodStand(new Vector3(53.0693f, -432.0549f, 4.677675f), 114.5513f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(53.0693f, -432.0549f, 4.277675f), 114.5513f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new FoodStand(new Vector3(158.0746f, -378.5169f, 5.117725f), 249.8867f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(158.0746f, -378.5169f, 4.717725f), 249.8867f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new FoodStand(new Vector3(422.386f, 1288.831f, 8.452396f), 269.7762f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(422.386f, 1288.831f, 8.052396f), 269.7762f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new FoodStand(new Vector3(377.1004f, 1375.288f, 14.71401f), 302.8472f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(377.1004f, 1375.288f, 14.31401f), 302.8472f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new FoodStand(new Vector3(381.1226f, 1436.722f, 14.71379f), 31.95643f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(381.1226f, 1436.722f, 14.31379f), 31.95643f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new FoodStand(new Vector3(171.0661f, 1279.87f, 14.68451f), 80.63454f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(171.0661f, 1279.87f, 14.28451f), 80.63454f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new FoodStand(new Vector3(169.8874f, 1309.959f, 14.68452f), 86.20712f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(169.8874f, 1309.959f, 14.28452f), 86.20712f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new FoodStand(new Vector3(169.8523f, 1348.913f, 14.68585f), 81.74162f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(169.8523f, 1348.913f, 14.28585f), 81.74162f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new FoodStand(new Vector3(20.05946f, 1199.422f, 10.18837f), 171.1226f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(20.05946f, 1199.422f, 9.78837f), 171.1226f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new FoodStand(new Vector3(-19.35653f, 1272.204f, 7.172366f), 81.36723f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-19.35653f, 1272.204f, 6.772366f), 81.36723f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new FoodStand(new Vector3(-461.9297f, 1478.355f, 4.104259f), 62.67839f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-461.9297f, 1478.355f, 3.704259f), 62.27839f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },

            new FoodStand(new Vector3(318.8633f, -278.6298f, 4.941455f), 29.98812f,  "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(318.7544f, -278.5309f, 4.24516f), 25.64678f) }, BannerImagePath = "stores\\beefybills.png" },
            new FoodStand(new Vector3(419.048f, -219.7135f, 5.080879f), 217.7193f,  "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(419.214f, -219.8842f, 4.251615f), 215.5863f) }, BannerImagePath = "stores\\beefybills.png" },
            new FoodStand(new Vector3(647.0709f, -49.74772f, 5.818226f), 0.467519f,  "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(647.0709f, -49.74772f, 5.818226f), 0.467519f) }, BannerImagePath = "stores\\beefybills.png" },
            new FoodStand(new Vector3(644.7056f, 24.07574f, 5.883476f), 177.2292f,  "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(644.7056f, 24.07574f, 5.883476f), 177.2292f) }, BannerImagePath = "stores\\beefybills.png" },
            new FoodStand(new Vector3(168.1629f, 1288.782f, 14.68979f), 272.9615f,  "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(168.1629f, 1288.782f, 14.28979f), 272.9615f) }, BannerImagePath = "stores\\beefybills.png" },
            new FoodStand(new Vector3(25.67185f, 1256.202f, 7.174278f), 80.46771f,  "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(25.67185f, 1256.202f, 6.774278f), 80.46771f) }, BannerImagePath = "stores\\beefybills.png" },
            //new FoodStand(new Vector3(-12.09861f, 1205.744f, 10.18844f), 117.7195f,  "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-12.09861f, 1205.744f, 9.88844f), 117.7195f) }, BannerImagePath = "stores\\beefybills.png" }, No Vendor Spawns - Map Issue?
        
                        new FoodStand(new Vector3(80.87234f, 645.4606f, 14.78925f), 182.3473f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(80.87234f, 645.4606f, 14.38925f), 182.3473f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new FoodStand(new Vector3(114.4671f, 529.1994f, 14.81366f), 184.2765f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(114.4671f, 529.1994f, 14.41366f), 184.2765f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new FoodStand(new Vector3(25.15249f, 849.4272f, 15.03401f), 109.0847f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(25.15249f, 849.4272f, 14.63401f), 109.0847f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new FoodStand(new Vector3(-29.55747f, 964.6931f, 14.80925f), 110.8132f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-29.55747f, 964.6931f, 14.40925f), 110.8132f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new FoodStand(new Vector3(-326.2726f, 1375.078f, 9.936646f), 73.67674f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-326.2726f, 1375.078f, 9.536646f), 73.67674f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new FoodStand(new Vector3(-402.7583f, 674.9507f, 4.811516f), 229.8443f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-402.7583f, 674.9507f, 4.411516f), 229.8443f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new FoodStand(new Vector3(-149.6297f, 1088.547f, 14.81764f), 144.2847f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-149.6297f, 1088.547f, 14.41764f), 144.2847f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new FoodStand(new Vector3(-208.2567f, 1638.888f, 12.87893f), 77.22648f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-149.6297f, 1088.547f, 14.41764f), 144.2847f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new FoodStand(new Vector3(-222.2786f, 1488.916f, 9.965118f), 108.8811f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ StateID = StaticStrings.LibertyStateID,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-222.2786f, 1488.916f, 9.565118f), 108.8811f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },
        });
    }
    private void DefaultConfig_ConvenienceStores()
    {
        LibertyCityLocations.ConvenienceStores.AddRange(new List<ConvenienceStore>()
        {
            new ConvenienceStore(new Vector3(-1274.281f, 1896.341f, 13.38669f), 357.4145f, "Happy Grocery", "","ConvenienceStoreMenu") { StateID = StaticStrings.AlderneyStateID },
            new ConvenienceStore(new Vector3(-1424.417f, 818.9971f, 25.6028f), 22.19808f, "Los Delicatessen", "","ConvenienceStoreMenu") { StateID = StaticStrings.AlderneyStateID },
            new ConvenienceStore(new Vector3(-1409.282f, 768.4227f, 19.35018f), 169.5838f, "Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.AlderneyStateID },

            new ConvenienceStore(new Vector3(1135.24f, 210.5948f, 18.60099f), 268.2418f, "Table Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1059.93f, 158.6778f, 16.0245f), 270.3828f, "Deli Sandwiches", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1060.557f, -58.18488f, 13.9834f), 266.3367f, "Table Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1135.24f, 210.5948f, 18.60099f), 268.2418f, "Timazone's Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1087.992f, -10.71362f, 12.43099f), 88.80842f, "Timazone's Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1087.953f, 8.265626f, 12.43058f), 93.63045f, "Hero Shop", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },



            new ConvenienceStore(new Vector3(1217.76f, -88.42326f, 14.501f), 3.454318f, "Island Grocery", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1134.031f, 179.1717f, 18.1881f), 270.1335f, "Russian Grocery", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1083.905f, -44.99024f, 14.1149f), 92.53188f, "Russian Grocery", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(-11.62165f, 247.2401f, 14.55747f), 357.287f, "Superb Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(99.27625f, 1848.561f, 20.41558f), 90.715f, "Superb Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(-203.2266f, 891.9241f, 9.843903f), 93.12379f, "Superb Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(-227.8028f, 1899.413f, 15.46196f), 264.9406f, "Prime Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },



            new ConvenienceStore(new Vector3(1648.562f, 543.776f, 25.90594f), 270.9548f, "Corner Store", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1714.324f, 694.9869f, 23.11282f), 270.6736f, "Deli Grocery", "","ConvenienceStoreMenu") { OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1610.871f, 660.1652f, 27.02329f), 317.7926f, "99 Cents Store", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1316.454f, 25.24125f, 13.79446f), 355.1226f, "Lopez Food Co", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1392.467f, -62.13634f, 13.93315f), 180.5255f, "Table Deli & Superior Market", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1378.399f, -61.39337f, 13.77891f), 183.8857f, "Russian Grocery", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1531.095f, 361.7825f, 27.3821f), 180.3469f, "1-Stop Deli Grocery", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1459.258f, 294.5624f, 25.71845f), 91.63822f, "Island Grocery", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1504.644f, 338.7794f, 27.53597f), 312.1838f, "Mardoll's Market", "","ConvenienceStoreMenu") { OpenTime = 0, CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1498.462f, 424.9238f, 28.10074f), 2.363338f, "Deli-A-Go-Go", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1649.088f, 569.5308f, 25.77537f), 270.7967f, "E-ZEE's Authentic Jamaican Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1648.354f, 688.7581f, 26.40527f), 269.6626f, "Deli GRocery", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1673.893f, 660.3806f, 26.96723f), 127.1551f, "Grocery Store", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1620.969f, 615.2239f, 31.26447f), 124.0797f, "American and West Indian Grocery", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1490.238f, 642.4111f, 32.79734f), 356.8299f, "Imperial Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1445.703f, 637.9631f, 32.75601f), 45.66153f, "Roadway Deli & Grocery Coop", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },

            new ConvenienceStore(new Vector3(1221.662f, 260.1613f, 23.18373f), 279.8268f,  "Deli &Grocery", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1492.887f, 525.0986f, 36.39328f), 268.9321f,  "Grocery Corp", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1416.269f, 524.3707f, 35.17557f), 270.1667f,  "2Tap Food Mart", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1353.188f, 666.3383f, 32.54428f), 140.4174f,  "Lopez Food Center", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1379.476f, 917.6197f, 29.78811f), 73.95114f,  "Deli Grocery", "","ConvenienceStoreMenu") { OpenTime = 0, CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1453.389f, 893.4844f, 22.54569f), 220.4071f,  "Superb Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1598.606f, 911.1912f, 23.50531f), 272.4103f,  "Candy & Grocery", "","ConvenienceStoreMenu") { OpenTime = 0, CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1431.425f, 1146.041f, 38.77233f), 89.20016f,  "Italian Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1432.227f, 1149.853f, 38.76752f), 92.27949f,  "Deli & Grocery", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(2036.27f, 911.4468f, 26.97294f), 267.462f,  "S & D's Grocery & Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(2058.805f, 950.127f, 27.93241f), 90.1776f,  "Willis Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(2059.178f, 1000.119f, 28.43221f), 89.89699f,  "49 Cent Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1993.144f, 1031.65f, 29.11987f), 49.71041f,  "Deli & Grocery", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },



            new ConvenienceStore(new Vector3(1283.189f, 1033.978f, 24.29405f), 6.854807f, "Deli & Grocery", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1359.871f, 1175.184f, 37.32677f), 89.43131f, "Ellikon Pantopoleon Deli & Grocery", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(1361.115f, 1141.195f, 39.04552f), 86.56087f, "Alpha Milk", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(627.8297f, 2181.352f, 16.1223f), 180.7094f, "Candy Store", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(657.1765f, 2180.79f, 16.09272f), 183.1182f, "Mini Market", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(711.2939f, 2254.736f, 27.66844f), 357.7273f, "Deli & Grocery", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(712.6467f, 2013.923f, 13.39466f), 357.5109f, "Sunshine Market","","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(744.9702f, 1998.711f, 12.39688f), 89.4341f, "Variety Shop", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(745.9528f, 2038.315f, 13.8276f), 136.6813f, "Deli Grocery", "","ConvenienceStoreMenu") { OpenTime = 0, CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(774.5649f, 2251.033f, 33.40094f), 317.3428f, "Deli.Grocery","","ConvenienceStoreMenu") { OpenTime = 0, CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(849.3564f, 2230.519f, 32.00209f), 184.0974f, "Convenience Store", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(968.6006f, 2048.956f, 22.11365f), 275.2551f, "Iron Belly Deli", "","ConvenienceStoreMenu") { StateID = StaticStrings.LibertyStateID },
            new ConvenienceStore(new Vector3(975.0884f, 2235.24f, 36.55573f), 196.1271f, "Deli Grocery","","ConvenienceStoreMenu") { OpenTime = 0, CloseTime = 24, StateID = StaticStrings.LibertyStateID },
        }
        );
    }
    private void DefaultConfig_BarberShops()
    {
        LibertyCityLocations.BarberShops.AddRange(new List<BarberShop>()
        {
            new BarberShop(new Vector3(-1214.096f, 1564.141f, 23.0377f), 92.31509f, "Unisex Salon", "")
            {
                StateID = StaticStrings.AlderneyStateID,
                IsTemporarilyClosed = true,
                VendorPersonnelID = "HaircutPeds",
                TypeName = "Beauty Salon",
            },
            new BarberShop(new Vector3(-1237.118f, 1464.819f, 23.0369f), 269.9279f, "Alderney Hair Co", "")
            {
                StateID = StaticStrings.AlderneyStateID,
                IsTemporarilyClosed = true,
                VendorPersonnelID = "HaircutPeds",
                TypeName = "Beauty Salon",
            },
            new BarberShop(new Vector3(-1237.505f, 1452.943f, 23.03641f), 269.7096f, "Spanky Beauty Supply", "")
            {
                StateID = StaticStrings.AlderneyStateID,
                IsTemporarilyClosed = true,
                VendorPersonnelID = "HaircutPeds",
                TypeName = "Beauty Salon",
            },
            new BarberShop(new Vector3(-1237.505f, 1452.943f, 23.03641f), 269.7096f, "Spanky Beauty Supply", "")
            {
                StateID = StaticStrings.AlderneyStateID,
                IsTemporarilyClosed = true,
                VendorPersonnelID = "HaircutPeds",
                TypeName = "Beauty Salon",
            },
            new BarberShop(new Vector3(-196.9043f, 865.2103f, 10.86149f), 4.296301f, "The Barber Shop", "")
            {
                StateID = StaticStrings.LibertyStateID,
                IsTemporarilyClosed = true,
                VendorPersonnelID = "HaircutPeds",
                TypeName = "Beauty Salon",
            },
            new BarberShop(new Vector3(46.25661f, 1900.294f, 20.41354f), 357.2557f, "Father & Son Barber Shop", "")
            {
                StateID = StaticStrings.LibertyStateID,
                IsTemporarilyClosed = true,
                VendorPersonnelID = "HaircutPeds",
                TypeName = "Beauty Salon",
            },
            new BarberShop(new Vector3(1672.063f, 673.9674f, 26.96723f), 85.82939f, "Unisex Salon", "")
            {
                StateID = StaticStrings.LibertyStateID,
                IsTemporarilyClosed = true,
                VendorPersonnelID = "HaircutPeds",
                TypeName = "Beauty Salon",
            },
            new BarberShop(new Vector3(1532.946f, 362.6311f, 27.24284f), 178.4335f, "The Barber Shop", "")
            {
                StateID = StaticStrings.LibertyStateID,
                IsTemporarilyClosed = true,
                VendorPersonnelID = "HaircutPeds",
                TypeName = "Beauty Salon",
            },


            new BarberShop(new Vector3(2037.097f, 904.048f, 26.86136f), 267.9283f, "Strictly Ink", "")
            {
                StateID = StaticStrings.LibertyStateID,
                IsTemporarilyClosed = true,
                VendorPersonnelID = "HaircutPeds",
                TypeName = "Beauty Salon",
            },
            new BarberShop(new Vector3(2036.156f, 896.9524f, 26.92537f), 269.7621f, "Harda's Nails", "")
            {
                StateID = StaticStrings.LibertyStateID,
                IsTemporarilyClosed = true,
                VendorPersonnelID = "HaircutPeds",
                TypeName = "Beauty Salon",
            },
            new BarberShop(new Vector3(1218.815f, 310.1832f, 24.11597f), 284.0233f, "Wil's Beauty Supply", "")
            {
                StateID = StaticStrings.LibertyStateID,
                IsTemporarilyClosed = true,
                VendorPersonnelID = "HaircutPeds",
                TypeName = "Beauty Salon",
            },
            new BarberShop(new Vector3(1223.848f, 241.2803f, 21.77414f), 273.9181f, "Liberty City Beauty Salon", "")
            {
                StateID = StaticStrings.LibertyStateID,
                IsTemporarilyClosed = true,
                VendorPersonnelID = "HaircutPeds",
                TypeName = "Beauty Salon",
            },
            new BarberShop(new Vector3(1361.179f, 1147.342f, 38.87286f), 89.97772f, "Hair Express Unisex", "")            
            {
                StateID = StaticStrings.LibertyStateID,
                IsTemporarilyClosed = true,
                VendorPersonnelID = "HaircutPeds",
                TypeName = "Beauty Salon",
            },
        });
    }
    private void DefaultConfig_Dealerships()
    {
        Dealership AutoEroticar = new Dealership(new Vector3(-1248.386f, 1612.826f, 23.06027f), 269.4143f, "Auto Eroticar", "Prestigious Automobiles", "PremiumDeluxeMenu") 
        {
            CameraPosition = new Vector3(-1220.093f, 1636.106f, 34.22548f),
            CameraDirection = new Vector3(-0.8637331f, -0.4456919f, -0.2352104f),
            CameraRotation = new Rotator(-13.60402f, 2.196045E-06f, 117.294f),
            VehiclePreviewCameraPosition = new Vector3(-1248.211f, 1624.571f, 23.72177f),
            VehiclePreviewCameraDirection = new Vector3(-0.9932797f, 0.03940188f, -0.1088254f),
            VehiclePreviewCameraRotation = new Rotator(-6.247609f, 1.073593E-07f, 87.72836f),
            VehiclePreviewLocation = new SpawnPlace(new Vector3(-1254.401f, 1624.775f, 22.89495f), 269.839f),
            LicensePlatePreviewText = "AUTO ERO",
            StateID = StaticStrings.AlderneyStateID,
            VehicleDeliveryLocations = new List<SpawnPlace>()
                {
                    new SpawnPlace(new Vector3(-1250.583f, 1640.089f, 23.0504f), 263.1514f),
                    new SpawnPlace(new Vector3(-1243.687f, 1645.933f, 23.30468f), 280.7747f),
                    new SpawnPlace(new Vector3(-1285.855f, 1621.441f, 26.50756f), 177.4582f),
                    new SpawnPlace(new Vector3(-1280.837f, 1605.042f, 26.1878f), 188.4852f),
                    new SpawnPlace(new Vector3(-1285.942f, 1592.272f, 26.51383f), 133.1344f),
                }
        };
        LibertyCityLocations.CarDealerships.Add(AutoEroticar);
        Dealership ElitásTravel = new Dealership(new Vector3(547.7309f, -202.5612f, 4.679811f), 61.76211f, "Elitás Travel", "There's first class and then there's Elitas", "ElitasMenu")
        {
            CameraPosition = new Vector3(527.8723f, -214.6277f, 17.683f),
            CameraDirection = new Vector3(0.9223651f, 0.2663169f, -0.2798534f),
            CameraRotation = new Rotator(-16.25146f, 5.335847E-06f, -73.89484f),
            BannerImagePath = "stores\\elitastravel.png",
            VehiclePreviewCameraPosition = new Vector3(598.1238f, -221.7465f, 9.52054f),
            VehiclePreviewCameraDirection = new Vector3(0.4390337f, -0.8624635f, -0.2518058f),
            VehiclePreviewCameraRotation = new Rotator(-14.58439f, 0.02697723f, -153.0218f),
            VehiclePreviewLocation = new SpawnPlace(new Vector3(606.1157f, -237.537f, 4.792892f), 329.992f),
            LicensePlatePreviewText = "Elitaz",
            StateID = StaticStrings.LibertyStateID,
            VehicleDeliveryLocations = new List<SpawnPlace>()
                {
                    new SpawnPlace(new Vector3(606.1157f, -237.537f, 4.792892f), 329.992f),
                    new SpawnPlace(new Vector3(628.3038f, -250.3824f, 4.793068f), 329.2202f),
                    new SpawnPlace(new Vector3(625.3674f, -209.7696f, 4.793066f), 334.5879f),
                    new SpawnPlace(new Vector3(638.3157f, -187.1084f, 4.793169f), 329.3659f),
                    new SpawnPlace(new Vector3(651.2589f, -164.6651f, 4.793118f), 330.0523f),
                }
        };
        LibertyCityLocations.CarDealerships.Add(ElitásTravel);
        Dealership LuxuryAutos = new Dealership(new Vector3(290.6169f, 1302.184f, 14.65709f), 40.51297f, "Luxury Autos", "You sure you can afford this?", "LuxuryAutosMenu")
        {
            CameraPosition = new Vector3(279.7256f, 1312.583f, 19.70246f),
            CameraDirection = new Vector3(0.6023207f, -0.7677315f, -0.2186278f),
            CameraRotation = new Rotator(-12.62845f, -1.662386E-05f, -141.8842f),
            BannerImagePath = "stores\\luxuryautos.png",
            VehiclePreviewCameraPosition = new Vector3(295.0954f, 1292.735f, 18.42866f),
            VehiclePreviewCameraDirection = new Vector3(-0.1208106f, -0.9627036f, -0.2420879f),
            VehiclePreviewCameraRotation = new Rotator(-14.00981f, 9.899419E-07f, 172.8473f),
            VehiclePreviewLocation = new SpawnPlace(new Vector3(294.427f, 1285.36f, 16.22498f), 59.47708f),
            LicensePlatePreviewText = "LUX AUTO",
            StateID = StaticStrings.LibertyStateID,
            VehicleDeliveryLocations = new List<SpawnPlace>()
                {
                    new SpawnPlace(new Vector3(317.818f, 1309.067f, 14.14753f), 90.88886f),
                    new SpawnPlace(new Vector3(283.8813f, 1291.495f, 14.17983f), 1.667649f),
                    new SpawnPlace(new Vector3(283.8477f, 1279.92f, 14.09944f), 1.466872f),
                    new SpawnPlace(new Vector3(283.8004f, 1273.479f, 14.10281f), 0.3184849f),
                }
        };
        LibertyCityLocations.CarDealerships.Add(LuxuryAutos);
    }
    private void DefaultConfig_PoliceStations()
    {
        float pedSpawnPercentage = 40f;
        float vehicleSpawnPercentage = 65f;
        List<PoliceStation> PoliceStations = new List<PoliceStation>()
        {
            //Broker
            new PoliceStation(new Vector3(1468.795f, 404.4823f, 28.02845f), 268.9079f, "South Slopes Police Station","") { MaxAssaultSpawns = 10,AssaultSpawnHeavyWeaponsPercent = 80f,OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID,      
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(1469.959f, 407.952f, 28.06162f), 334.1368f, 65f),
                    new LEConditionalLocation(new Vector3(1471.489f, 400.8039f, 28.00071f), 233.3257f, 65f),
                    new LEConditionalLocation(new Vector3(1472.287f, 413.687f, 28.16126f), 335.372f, 65f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(1445.06f, 393.4634f, 28.33191f), 88.59583f, 95f),
                },         
            },
            new PoliceStation(new Vector3(1132.495f, 135.204f, 18.18011f), 321.3237f, "Hove Beach Police Station","") {MaxAssaultSpawns = 5,AssaultSpawnHeavyWeaponsPercent = 80f,OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(1131.671f, 138.2387f, 17.40844f), 54.60419f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(1138.356f, 133.9466f, 17.49231f), 283.6084f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(1123.693f, 138.8339f, 16.70552f), 333.9313f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(1128.705f, 108.3261f, 17.1551f), 243.2021f, pedSpawnPercentage),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(1127.885f, 104.3547f, 16.9264f), 269.8351f, 85f),
                    new LEConditionalLocation(new Vector3(1140.719f, 132.05f, 17.29783f), 176.8967f, 85f),
                },         
            },


            //Dukes
            new PoliceStation(new Vector3(1471.042f, 1020.193f, 30.82781f), 179.7346f, "East Island City Police Station","") {MaxAssaultSpawns = 20,AssaultSpawnHeavyWeaponsPercent = 80f,OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(1473.851f, 1017.511f, 30.82764f), 183.1868f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(1467.423f, 1017.069f, 30.82807f), 156.0497f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(1460.437f, 1010.427f, 28.57376f), 139.4183f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(1488.792f, 1010.496f, 28.03296f), 267.2982f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(1470.244f, 1068.398f, 38.50281f), 292.8177f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(1487.186f, 1068.372f, 38.53416f), 332.2541f, pedSpawnPercentage),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(1471.367f, 1077.222f, 38.50308f), 52.43438f, 75f),
                    new LEConditionalLocation(new Vector3(1477.497f, 1072.767f, 38.50276f), 284.1264f, 75f),
                },         
            },
            new PoliceStation(new Vector3(2404.376f, 942.9011f, 7.022036f), 270.644f, "FIA Police Station","") { MaxAssaultSpawns = 25,AssaultSpawnHeavyWeaponsPercent = 80f,OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(2407.317f, 946.8757f, 6.080221f), 262.8414f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(2410.385f, 938.5565f, 6.204664f), 197.1165f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(2402.948f, 903.6752f, 6.08021f), 239.0134f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(2373.941f, 905.1735f, 6.080024f), 187.3566f, pedSpawnPercentage),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(2349.061f, 922.7029f, 6.078603f), 87.75613f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(2350.036f, 932.507f, 6.080106f), 86.69183f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(2350.425f, 952.7989f, 6.08022f), 83.99601f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(2349.991f, 959.1418f, 6.080325f), 89.76208f, vehicleSpawnPercentage),
                },         
            },

            //Bohan
            new PoliceStation(new Vector3(673.7206f, 2084.079f, 18.22777f), 2.178002f, "Fortside Police Station","") {MaxAssaultSpawns = 15,AssaultSpawnHeavyWeaponsPercent = 80f,OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(670.1032f, 2088.794f, 16.3161f), 334.3344f, 55f),
                    new LEConditionalLocation(new Vector3(677.0965f, 2091.434f, 16.25306f), 11.18726f, 55f),
                    new LEConditionalLocation(new Vector3(664.1884f, 2093.99f, 16.19413f), 45.96721f, 55f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(681.4908f, 2089.008f, 16.31622f), 358.0114f, 95f),
                },         
            },
            new PoliceStation(new Vector3(1220.499f, 2364.085f, 23.89311f), 88.64945f, "Northern Gardens LCPD Station","") {MaxAssaultSpawns = 20,AssaultSpawnHeavyWeaponsPercent = 80f,OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(1218.547f, 2361.659f, 23.89289f), 49.91943f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(1213.31f, 2368.823f, 23.06237f), 64.75815f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(1213.848f, 2358.921f, 23.07587f), 106.4697f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(1225.221f, 2370.886f, 23.8929f), 35.86892f, pedSpawnPercentage),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(1236.041f, 2386.234f, 23.89339f), 359.3186f, 75f),
                    new LEConditionalLocation(new Vector3(1228.21f, 2385.235f, 23.89339f), 178.7449f, 75f),
                    new LEConditionalLocation(new Vector3(1220.831f, 2385.9f, 23.89338f), 3.008273f, 75f),
                },         
            },

            //Algonquin
            new PoliceStation(new Vector3(-146.7365f, 229.4094f, 13.04987f), 270.1865f, "Suffolk Police Station","") {MaxAssaultSpawns = 20,AssaultSpawnHeavyWeaponsPercent = 80f,OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(-142.6722f, 233.2056f, 11.49896f), 297.2696f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-141.2132f, 224.5515f, 10.20658f), 237.6309f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-141.8323f, 248.6599f, 12.83091f), 330.3247f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-158.308f, 249.8716f, 12.21221f), 322.5742f, pedSpawnPercentage),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(-153.2814f, 242.6229f, 12.70591f), 264.8219f, 75f),
                    new LEConditionalLocation(new Vector3(-174.0335f, 229.9114f, 12.69959f), 2.602433f, 75f),
                    new LEConditionalLocation(new Vector3(-163.1242f, 224.3065f, 12.69959f), 270.2501f, 75f),
                },         
            },
            new PoliceStation(new Vector3(449.9062f, 286.9244f, 10.75325f), 208.419f, "Lower Easton Police Station","") {MaxAssaultSpawns = 20,AssaultSpawnHeavyWeaponsPercent = 80f,OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(453.639f, 285.0331f, 10.74954f), 177.6642f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(449.8034f, 281.1576f, 10.75124f), 248.6064f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(471.2408f, 280.9908f, 10.74291f), 161.0854f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(480.5587f, 276.0337f, 10.7518f), 132.277f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(510.189f, 277.6776f, 6.751965f), 113.8463f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(415.3839f, 285.3393f, 14.87019f), 122.8338f, pedSpawnPercentage),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(398.4545f, 283.5684f, 14.75398f), 296.0812f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(396.9294f, 286.8604f, 14.74307f), 294.4711f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(395.6099f, 289.9241f, 14.75308f), 293.0849f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(394.3639f, 292.6385f, 14.75081f), 296.2747f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(392.7619f, 295.8325f, 14.70148f), 295.9965f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(504.4927f, 281.2288f, 6.597221f), 183.3899f, vehicleSpawnPercentage),
                },         
            },
            new PoliceStation(new Vector3(53.50484f, 593.811f, 14.76916f), 357.1598f, "Star Junction Police Station","") {MaxAssaultSpawns = 5,AssaultSpawnHeavyWeaponsPercent = 50f,OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(50.96454f, 595.6664f, 14.76701f), 23.6899f, 65f),
                    new LEConditionalLocation(new Vector3(56.45068f, 598.0446f, 14.76187f), 331.0646f, 65f),
                    new LEConditionalLocation(new Vector3(59.91758f, 582.8718f, 14.73059f), 277.8942f, 65f),
                    new LEConditionalLocation(new Vector3(48.64334f, 581.6539f, 14.76144f), 99.23477f, 65f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(51.62194f, 607.1779f, 14.75878f), 117.0722f, 95f),
                },         
            },
            new PoliceStation(new Vector3(-168.6235f, 776.5378f, 13.6791f), 178.2846f, "Westminster Police Station","") {MaxAssaultSpawns = 20,AssaultSpawnHeavyWeaponsPercent = 80f,OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(-171.4672f, 773.8879f, 12.69119f), 173.3235f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-164.9593f, 774.0068f, 13.49523f), 216.3236f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-158.9926f, 770.3032f, 14.05752f), 220.6159f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-179.402f, 773.2516f, 11.78918f), 148.3038f, pedSpawnPercentage),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(-170.3779f, 769.1196f, 12.73803f), 75.14408f, 75f),
                    new LEConditionalLocation(new Vector3(-162.4109f, 769.0792f, 13.61853f), 87.21024f, 75f),
                },         
            },
            new PoliceStation(new Vector3(290.0559f, 1174.093f, 15.32371f), 85.63059f, "Middle Park East Police Station","") {MaxAssaultSpawns = 20,AssaultSpawnHeavyWeaponsPercent = 80f,OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(287.5326f, 1169.215f, 14.69146f), 115.9246f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(286.9623f, 1179.041f, 14.66739f), 65.71052f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(293.907f, 1185.153f, 14.62776f), 11.44679f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(321.694f, 1165.259f, 14.66715f), 192.1326f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(325.5107f, 1168.46f, 14.66715f), 251.7568f, pedSpawnPercentage),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(328.2254f, 1176.702f, 14.51741f), 0.4844545f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(331.4904f, 1176.841f, 14.51946f), 359.1905f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(334.673f, 1176.806f, 14.52053f), 359.7402f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(337.8275f, 1176.815f, 14.52219f), 0.9160225f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(341.1281f, 1176.879f, 14.5238f), 357.2642f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(344.5563f, 1176.952f, 14.52575f), 3.734199f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(334.6956f, 1161.298f, 14.54932f), 92.00898f, vehicleSpawnPercentage),

                },         
            },
            new PoliceStation(new Vector3(-178.7366f, 1597.181f, 11.9273f), 176.7212f, "Varsity Heights Police Station","") {MaxAssaultSpawns = 20,AssaultSpawnHeavyWeaponsPercent = 80f,OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(-184.1453f, 1596.33f, 11.24726f), 104.9129f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-188.899f, 1593.148f, 10.88357f), 140.0583f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-170.9266f, 1594.829f, 12.58487f), 204.1209f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-189.2326f, 1615.316f, 11.68808f), 77.01761f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-184.4212f, 1629.36f, 12.85684f), 19.22638f, pedSpawnPercentage),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(-197.7826f, 1637.769f, 12.88037f), 88.58164f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-198.1044f, 1634.417f, 12.77739f), 92.46103f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-198.1422f, 1631.311f, 12.66841f), 90.73277f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-198.3575f, 1628.211f, 12.47059f), 91.66457f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-198.6988f, 1625.159f, 12.23071f), 92.23808f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-198.6242f, 1621.406f, 11.86491f), 91.21073f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-198.5256f, 1617.305f, 11.4495f), 86.02792f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-198.6718f, 1614.119f, 11.12441f), 91.38914f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-198.5444f, 1610.547f, 10.86039f), 86.48497f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-198.2461f, 1607.494f, 10.67246f), 88.98672f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-198.7656f, 1604.295f, 10.49834f), 89.56476f, vehicleSpawnPercentage),
                },         
            },
            new PoliceStation(new Vector3(323.3206f, 1685.935f, 14.74491f), 180.4846f, "East Holland LCPD Station","") {MaxAssaultSpawns = 20,AssaultSpawnHeavyWeaponsPercent = 80f,OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(320.6393f, 1682.101f, 14.75296f), 140.735f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(330.8164f, 1681.063f, 14.75958f), 227.0982f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(348.2731f, 1713.972f, 15.61183f), 280.0514f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(325.3313f, 1713.931f, 16.00125f), 302.9459f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(285.5443f, 1707.673f, 16.94057f), 85.26598f, pedSpawnPercentage),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(319.3295f, 1743.545f, 15.91363f), 205.8302f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(331.2584f, 1736.257f, 15.90359f), 302.7219f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(332.7829f, 1733.547f, 15.91277f), 297.2332f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(334.3795f, 1730.983f, 15.90455f), 297.6017f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(336.0804f, 1728.12f, 15.90191f), 296.8231f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(337.3104f, 1725.38f, 15.90105f), 295.1454f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(338.6208f, 1722.555f, 15.90161f), 295.0473f, vehicleSpawnPercentage),
                },         
            },

            //Alderney
            new PoliceStation(new Vector3(-1480.706f, 771.6721f, 22.91864f), 240.3791f, "West District Mini Precinct","") { MaxAssaultSpawns = 5,AssaultSpawnHeavyWeaponsPercent = 50f,OpenTime = 0,CloseTime = 24, StateID = StaticStrings.AlderneyStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(-1479.285f, 767.872f, 22.09835f), 204.5331f, 75f),
                    new LEConditionalLocation(new Vector3(-1473.666f, 775.3422f, 22.32182f), 269.4045f, 75f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(-1467.071f, 789.783f, 22.93336f), 236.4416f, 95f),
                },         
            },
            new PoliceStation(new Vector3(-986.9969f, 257.8285f, 2.994598f), 311.2633f, "Acter Police Station","") { MaxAssaultSpawns = 20,AssaultSpawnHeavyWeaponsPercent = 80f, OpenTime = 0,CloseTime = 24, StateID = StaticStrings.AlderneyStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(-982.7232f, 254.9283f, 3.053053f), 278.8697f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-991.2359f, 262.9456f, 2.998651f), 35.81685f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-981.2977f, 245.785f, 3.053054f), 276.9689f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-1008.636f, 236.9965f, 2.923784f), 116.7921f, pedSpawnPercentage),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(-999.2963f, 206.3592f, 2.923783f), 194.2897f, 75f),
                    new LEConditionalLocation(new Vector3(-993.6766f, 209.552f, 2.923782f), 6.087631f, 75f),
                    new LEConditionalLocation(new Vector3(-1004.175f, 216.1478f, 2.923783f), 273.368f, 75f),
                },         
            },
            new PoliceStation(new Vector3(-684.4249f, 1763.837f, 25.79581f), 134.1164f, "Leftwood Police Station","") { MaxAssaultSpawns = 20,AssaultSpawnHeavyWeaponsPercent = 80f,OpenTime = 0,CloseTime = 24, StateID = StaticStrings.AlderneyStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(-685.2141f, 1759.955f, 24.57136f), 190.5793f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-690.9417f, 1761.98f, 24.56702f), 145.3687f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-691.2444f, 1784.005f, 24.56167f), 43.59f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-692.3284f, 1796.127f, 24.56199f), 86.39127f, pedSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-699.9471f, 1833.337f, 24.3741f), 121.7336f, pedSpawnPercentage),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(-684.2045f, 1800.412f, 24.44139f), 243.3803f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-683.9017f, 1807.903f, 24.47196f), 243.2513f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-684.6254f, 1814.217f, 24.45251f), 265.2226f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-640.4788f, 1785.514f, 21.97303f), 181.8763f, vehicleSpawnPercentage),
                    new LEConditionalLocation(new Vector3(-631.6221f, 1784.284f, 21.97381f), 183.7322f, vehicleSpawnPercentage),
                },         
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
    private void DefaultConfig_Garages()
    {
        List<RepairGarage> RepairGarageList_96 = new List<RepairGarage>()
        {
            new RepairGarage(new Vector3(1296.495f, 197.1002f, 20.92298f), 180.8648f, "Native Engines", "The best repair shop in the whole Broker")
            {
                OpenTime = 0,
                CloseTime = 24,
                CanInteractWhenWanted = true,
                StateID = StaticStrings.LibertyStateID,
                CameraPosition = new Vector3(1288.726f, 223.5908f, 27.29563f),
                HasNoGarageDoors = true,
                CameraDirection = new Vector3(0.5266379f, -0.8357221f, -0.1556312f),
                CameraRotation = new Rotator(-8.953408f, 2.160762E-06f, -147.7825f)
            },
            new RepairGarage(new Vector3(-69.7f,2042.61f,220.27f-200f), 0f, "Auto Cowboys", "Servicing Holland since 1979")
            {
                CameraPosition = new Vector3(-113.01f,1998.94f,235.47f-200f),
                OpenTime = 0,
                CloseTime = 24,
                CanInteractWhenWanted = true,
                HasNoGarageDoors = true,
                StateID = StaticStrings.LibertyStateID,
            },
            new RepairGarage(new Vector3(-275.82f,868.98f,206.63f-200f), 0f, "Auto Limbo", "Where all the Union Drive accidents go")
            {
                CameraPosition = new Vector3(-227f,934.29f,222.85f-200f),
                OpenTime = 0,
                CloseTime = 24,
                CanInteractWhenWanted = true,
                HasNoGarageDoors = true,
                StateID = StaticStrings.LibertyStateID,
            },
            new RepairGarage(new Vector3(948.62f,1998.77f,214.84f-200f), 0f, "Muscle Mary's", "We service imports too")
            {
                CameraPosition = new Vector3(885.52f,1988.27f,220.1f-200f),
                OpenTime = 0,
                CloseTime = 24,
                CanInteractWhenWanted = true,
                HasNoGarageDoors = true,
                StateID = StaticStrings.LibertyStateID,
            },
            new RepairGarage(new Vector3(-887.16f,1679.61f,217.16f-200f), 0f, "Axel's Pay'n'Spray", "Franchising car repairs")
            {
                CameraPosition = new Vector3(-968.8f,1634.06f,255.7f-200f),
                OpenTime = 0,
                CloseTime = 24,
                CanInteractWhenWanted = true,
                HasNoGarageDoors = true,
                StateID = StaticStrings.AlderneyStateID,
            },
            new RepairGarage(new Vector3(-1063.54f,766.97f,210.8f-200f), 0f, "Axel's Pay'n'Spray", "Franchising car repairs")
            {
                CameraPosition = new Vector3(-1034.16f,779.04f,219.08f-200f),
                OpenTime = 0,
                CloseTime = 24,
                CanInteractWhenWanted = true,
                HasNoGarageDoors = true,
                StateID = StaticStrings.AlderneyStateID,
            },


            new RepairGarage(new Vector3(1992.384f, 858.2811f, 22.10884f), 179.815f, "The East Boys Auto Shop", "")
            {
                CameraPosition = new Vector3(1977.735f, 885.1741f, 29.30753f),
                CameraDirection = new Vector3(0.665975f, -0.7356181f, -0.1238685f),
                CameraRotation = new Rotator(-7.115416f, -1.0755E-05f, -137.8446f),
                OpenTime = 0,
                CloseTime = 24,
                CanInteractWhenWanted = true,
                HasNoGarageDoors = true,
                StateID = StaticStrings.LibertyStateID,
            },
            new RepairGarage(new Vector3(2032.354f, 688.2342f, 21.08754f), 334.0668f, "Auto Repairs", "")
            {
                CameraPosition = new Vector3(1996.28f, 706.5511f, 30.3878f), 
                CameraDirection = new Vector3(0.906232f, -0.3647959f, -0.2136997f),
                CameraRotation = new Rotator(-12.33925f, 3.49585E-06f, -111.9268f),
                OpenTime = 0,
                CloseTime = 24,

                CanInteractWhenWanted = true,
                HasNoGarageDoors = true,
                StateID = StaticStrings.LibertyStateID,
            },
            new RepairGarage(new Vector3(1211.589f, 1113.392f, 26.02176f), 89.59235f, "Auto Repair & Body Shop", "")
            {
                CameraPosition = new Vector3(1220.912f, 1173.546f, 32.08664f), 
                CameraDirection = new Vector3(0.7703393f, -0.6277416f, -0.111883f), 
                CameraRotation = new Rotator(-6.423876f, 1.181356E-05f, -129.1762f),
                OpenTime = 0,
                CloseTime = 24,
                CanInteractWhenWanted = true,
                HasNoGarageDoors = true,
                StateID = StaticStrings.LibertyStateID,
            },

        };
        LibertyCityLocations.RepairGarages.AddRange(RepairGarageList_96);



    }
    private void DefaultConfig_FireStations()
    {
        List<FireStation> FireStations = new List<FireStation>()
        {
            new FireStation(new Vector3(-1922.663f, 658.1226f, 12.04813f), 3.062835f,"Tudor Fire Station","") 
            { 
                OpenTime = 0,
                CloseTime = 24, 
                StateID = StaticStrings.AlderneyStateID,
                AssignedAssociationID = "FDLC",
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(-1918.814f, 659.8553f, 12.04813f), 323.3341f, 35f),
                    new FireConditionalLocation(new Vector3(-1927.28f, 659.9587f, 12.04813f), 43.30471f, 35f),
                    new FireConditionalLocation(new Vector3(-1930.171f, 647.8334f, 12.04812f), 128.3218f, 35f),
                    new FireConditionalLocation(new Vector3(-1930.269f, 633.1935f, 12.04813f), 102.2719f, 35f),
                    new FireConditionalLocation(new Vector3(-1904.785f, 655.9661f, 12.04812f), 335.8226f, 35f),
                    new FireConditionalLocation(new Vector3(-1873.251f, 651.5896f, 12.04813f), 353.951f, 35f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(-1879.133f, 654.5981f, 12.04813f), 179.0583f, 35f),
                    new FireConditionalLocation(new Vector3(-1884.686f, 653.9754f, 12.04813f), 0.8778898f, 35f),
                    new FireConditionalLocation(new Vector3(-1891.434f, 654.8054f, 12.04813f), 0.4668479f, 35f),
                    new FireConditionalLocation(new Vector3(-1900.126f, 658.4937f, 12.04813f), 177.1563f, 35f),
                    new FireConditionalLocation(new Vector3(-1919.002f, 645.2313f, 12.04812f), 89.35287f, 35f),
                    new FireConditionalLocation(new Vector3(-1919.116f, 639.309f, 12.04812f), 87.76639f, 35f),
                },
            },

            new FireStation(new Vector3(-1332.796f, 1043.983f, 25.44432f), 89.78783f,"Berchem Fire Station","")
            {
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.AlderneyStateID,
                AssignedAssociationID = "FDLC",
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(-1334.88f, 1053.027f, 25.44432f), 61.3048f, 75f),
                    new FireConditionalLocation(new Vector3(-1334.935f, 1041.588f, 25.44432f), 125.7424f, 75f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(-1338.565f, 1047.647f, 25.35919f), 89.76939f, 40f),
                    new FireConditionalLocation(new Vector3(-1341.675f, 1039.452f, 25.33426f), 355.698f, 40f),
                },
            },

            new FireStation(new Vector3(1183.457f, 596.6918f, 35.0109f), 0.9240291f,"Broker Fire Station","")
            {
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
                InteriorID = 160514,
                AssignedAssociationID = "FDLC",
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(1190.922f, 598.6693f, 34.74236f), 311.8359f, 40f),
                    new FireConditionalLocation(new Vector3(1176.098f, 598.8678f, 35.13459f), 34.28785f, 40f),
                    new FireConditionalLocation(new Vector3(1187.699f, 570.186f, 34.9992f), 54.14355f, 40f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(1187.651f, 582.8724f, 34.99912f), 1.017775f, 40f),
                    new FireConditionalLocation(new Vector3(1179.594f, 581.8882f, 34.99925f), 2.09237f, 40f),
                },
            },
            new FireStation(new Vector3(2577.95f, 677.7576f, 5.807951f), 64.98926f,"FIA Fire Station","")
            {
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
                AssignedAssociationID = "FDLC",
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(2574.061f, 674.1057f, 5.807956f), 140.599f, 40f),
                    new FireConditionalLocation(new Vector3(2569.41f, 677.0591f, 5.807951f), 90.42297f, 40f),
                    new FireConditionalLocation(new Vector3(2575.68f, 665.2891f, 5.807951f), 122.4748f, 40f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(2561.345f, 667.9217f, 5.807953f), 89.19281f, 40f),
                    new FireConditionalLocation(new Vector3(2552.614f, 678.1039f, 5.807953f), 137.5261f, 40f),
                },
            },

            new FireStation(new Vector3(1352.599f, 2211.427f, 10.56046f), 134.0749f,"Bohan Fire Station","")
            {
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
                AssignedAssociationID = "FDLC",
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(1354.872f, 2206.148f, 10.52985f), 181.0731f, 40f),
                    new FireConditionalLocation(new Vector3(1348.517f, 2208.87f, 10.56043f), 128.9855f, 40f),
                    new FireConditionalLocation(new Vector3(1372.522f, 2222.434f, 10.47557f), 260.1082f, 40f),
                    new FireConditionalLocation(new Vector3(1343.897f, 2235.282f, 10.52638f), 213.5798f, 40f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(1354.804f, 2230.135f, 10.52639f), 224.0606f, 40f),
                    new FireConditionalLocation(new Vector3(1349.883f, 2224.052f, 10.52595f), 226.8957f, 40f),
                    new FireConditionalLocation(new Vector3(1367.973f, 2239.056f, 10.44384f), 224.0108f, 40f),
                },
            },
            new FireStation(new Vector3(-34.20112f, 2035.804f, 20.41524f), 266.0019f,"Northwood Fire Station","")
            {
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
                AssignedAssociationID = "FDLC",
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(-31.87979f, 2032.899f, 20.41524f), 249.7648f, 40f),
                    new FireConditionalLocation(new Vector3(-31.60878f, 2040.818f, 20.41524f), 286.9591f, 40f),
                    new FireConditionalLocation(new Vector3(-35.21124f, 2003.835f, 20.43614f), 231.1253f, 40f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(-31.54707f, 2017.772f, 20.2655f), 269.4188f, 40f),
                    new FireConditionalLocation(new Vector3(-31.31855f, 2026.771f, 20.2647f), 268.5182f, 40f),
                },
            },

            new FireStation(new Vector3(523.7645f, 114.0217f, 4.960016f), 270.8604f,"Fishmarket Fire Station","")
            {
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
                AssignedAssociationID = "FDLC",
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(526.0635f, 111.3013f, 4.960013f), 251.4846f, 40f),
                    new FireConditionalLocation(new Vector3(527.7251f, 117.6922f, 4.960011f), 288.2192f, 40f),
                    new FireConditionalLocation(new Vector3(527.5803f, 155.7901f, 4.957152f), 354.2037f, 40f),
                    new FireConditionalLocation(new Vector3(526.6627f, 172.0345f, 4.947123f), 350.0849f, 40f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(527.2249f, 128.7819f, 4.809995f), 180.3352f, 35f),
                    new FireConditionalLocation(new Vector3(527.681f, 140.6192f, 4.809992f), 173.7421f, 35f),
                },
            },

            new FireStation(new Vector3(-142.8153f, 718.3762f, 14.76839f), 268.843f,"Fishmarket Fire Station","")
            {
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
                AssignedAssociationID = "FDLC",
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(-141.6838f, 711.9039f, 14.76839f), 253.1116f, 35f),
                    new FireConditionalLocation(new Vector3(-140.3948f, 723.478f, 14.76839f), 314.6575f, 35f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(-136.4951f, 723.4045f, 14.65207f), 178.2443f, 35f),
                },
            },
        };
        LibertyCityLocations.FireStations.AddRange(FireStations);
    }
    private void DefaultConfig_GamblingDens()
    {
        GamblingParameters defaultParameters = PlacesOfInterest.GetParameters(1);
        List<GamblingDen> GamblingDens = new List<GamblingDen>()
        {
            new GamblingDen(new Vector3(-1111.794f, 1399.861f, 19.81154f), 175.4774f,"Alderney City Casino","Test Your Luck... In more ways than one")//Regular Casino
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
                CameraPosition = new Vector3(851.0151f, 50.54535f, 97.35239f),
                CameraDirection = new Vector3(0.9805309f, -0.1052597f, -0.1657695f),
                CameraRotation = new Rotator(-9.541942f, 7.250671E-06f, -96.12722f),
            },

            new GamblingDen(new Vector3(1459.422f, 1244.673f, 36.11836f), 357.617f, "Inside Track", "")
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


            new GamblingDen(new Vector3(-26.60195f, 1049.526f, 15.02625f), 178.7795f,"Messina Gentlemans Club","Take a load off!")//Messina Casino, fancy
            {
                GamblingParameters = PlacesOfInterest.GetParameters(3),
                WinLimit = 45000,
                WinLimitResetHours = 24,
                OpenTime = 18,
                CloseTime = 4,
                AssignedAssociationID = "AMBIENT_GANG_MESSINA",
                CameraPosition = new Vector3(-27.55852f, 994.0565f, 59.2492f), 
                CameraDirection = new Vector3(-0.04450776f, 0.9161568f, -0.3983414f), 
                CameraRotation = new Rotator(-23.47453f, 2.094323E-05f, 2.781296f),
                StateID = StaticStrings.LibertyStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(-31.04354f, 1046.068f, 14.8617f), 154.4864f, 75f) { AssociationID = "AMBIENT_GANG_MESSINA"},
                    new GangConditionalLocation(new Vector3(-20.08696f, 1045.989f, 14.86169f), 216.8216f, 75f) { AssociationID = "AMBIENT_GANG_MESSINA"},
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(-1351.666f, 246.2108f, 60.47062f), 186.2948f, 40f) { IsEmpty = true, AssociationID = "AMBIENT_GANG_MESSINA"},
                },
            },
            new GamblingDen(new Vector3(169.4202f, -53.90068f, 16.27729f), 270.7705f,"Old Block Casino","A game of tradition")// Gambetti Casino, decnet condo
            {
                GamblingParameters = PlacesOfInterest.GetParameters(2),
                WinLimit = 35000,
                WinLimitResetHours = 24,
                OpenTime = 18,
                CloseTime = 4,
                AssignedAssociationID = "AMBIENT_GANG_GAMBETTI",
                CameraPosition = new Vector3(194.5283f, -44.69314f, 29.22834f), 
                CameraDirection = new Vector3(-0.8022859f, -0.4778223f, -0.3578032f), 
                CameraRotation = new Rotator(-20.96535f, -6.400122E-06f, 120.777f),
                StateID = StaticStrings.LibertyStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(172.2236f, -51.43612f, 14.77877f), 237.9585f, 40f) { AssociationID = "AMBIENT_GANG_GAMBETTI"},
                    new GangConditionalLocation(new Vector3(172.6373f, -57.73302f, 14.76363f), 244.0623f, 40f) { AssociationID = "AMBIENT_GANG_GAMBETTI"},
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    
                },
            },
            new GamblingDen(new Vector3(1479.129f, 2289.513f, 12.09924f), 89.52518f,"Garden Casino","See green in the garden")//Lupisella Casino, low end hotel
            {
                GamblingParameters = defaultParameters,
                WinLimit = 25000,
                WinLimitResetHours = 24,
                OpenTime = 18,
                CloseTime = 4,
                AssignedAssociationID = "AMBIENT_GANG_LUPISELLA",
                CameraPosition = new Vector3(1460.727f, 2302.028f, 20.22127f), 
                CameraDirection = new Vector3(0.7319992f, -0.6185485f, -0.2856135f), 
                CameraRotation = new Rotator(-16.59552f, 1.781767E-06f, -130.1982f),
                StateID = StaticStrings.LibertyStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(1473.45f, 2286.291f, 10.12301f), 124.9453f, 40f) { AssociationID = "AMBIENT_GANG_LUPISELLA"},
                    new GangConditionalLocation(new Vector3(1476.263f, 2292.687f, 10.08501f), 40.22756f, 40f) { AssociationID = "AMBIENT_GANG_LUPISELLA"},
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(1470.895f, 2268.277f, 10.34472f), 0.8881978f, 40f) { IsEmpty = true, AssociationID = "AMBIENT_GANG_LUPISELLA"},
                },
            },
            new GamblingDen(new Vector3(-1217.177f, 1000.864f, 20.79525f), 355.4877f,"Upstairs Casino","Make sure you bring cash")//Ancelotti Casino
            {
                GamblingParameters = defaultParameters,
                WinLimit = 20000,
                WinLimitResetHours = 24,
                OpenTime = 18,
                CloseTime = 4,
                AssignedAssociationID = "AMBIENT_GANG_ANCELOTTI",
                CameraPosition = new Vector3(-1199.969f, 1017.442f, 25.87575f), 
                CameraDirection = new Vector3(-0.646277f, -0.7271133f, -0.2315867f), 
                CameraRotation = new Rotator(-13.39051f, 6.143427E-06f, 138.3685f),
                StateID = StaticStrings.AlderneyStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(-1223.102f, 1003.379f, 19.29539f), 47.04644f, 40f) { AssociationID = "AMBIENT_GANG_ANCELOTTI"},
                    new GangConditionalLocation(new Vector3(-1213.419f, 1003.196f, 18.35084f), 305.5476f, 40f) { AssociationID = "AMBIENT_GANG_ANCELOTTI"},
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(-1212.313f, 1006.873f, 18.2206f), 272.0731f, 40f) { IsEmpty = true, AssociationID = "AMBIENT_GANG_ANCELOTTI"},
                },
            },
            new GamblingDen(new Vector3(127.2694f, 1742.047f, 22.79446f), 359.5596f,"Pavano Games","")//Pavano Casino
            {
                GamblingParameters = defaultParameters,
                WinLimit = 15000,
                WinLimitResetHours = 24,
                OpenTime = 18,
                CloseTime = 4,
                AssignedAssociationID = "AMBIENT_GANG_PAVANO",
                CameraPosition = new Vector3(144.5005f, 1761.812f, 27.52961f), 
                CameraDirection = new Vector3(-0.6862326f, -0.7077096f, -0.1680235f), 
                CameraRotation = new Rotator(-9.67292f, -8.227825E-06f, 135.8827f),
                StateID = StaticStrings.LibertyStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(124.767f, 1747.016f, 20.43135f), 26.97159f, 40f) { AssociationID = "AMBIENT_GANG_PAVANO"},
                    new GangConditionalLocation(new Vector3(131.6205f, 1748.192f, 20.43144f), 336.5046f, 40f) { AssociationID = "AMBIENT_GANG_PAVANO"},
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(137.138f, 1751.758f, 20.29686f), 90.64448f, 40f) { IsEmpty = true, AssociationID = "AMBIENT_GANG_PAVANO"},
                },
            },
        };
        LibertyCityLocations.GamblingDens.AddRange(GamblingDens);
    }
    private void DefaultConfig_HeadShops()
    {
        List<HeadShop> HeadShops = new List<HeadShop>()
        {
            new HeadShop(new Vector3(1489.411f, 339.0406f, 27.63f), 2.845504f,"69 Brand Cigar Shop","","HeadShopMenu") { StateID = StaticStrings.LibertyStateID },
            new HeadShop(new Vector3(605.5053f, 2183.451f, 16.52885f), 172.005f, "Tobacco Shop 24/7", "","HeadShopMenu") { StateID = StaticStrings.LibertyStateID },

        };
        LibertyCityLocations.HeadShops.AddRange(HeadShops);
    }

    private void DefaultConfig_GunStores()
    {
        List<GunStore> GunStores = new List<GunStore>()
        {
            new GunStore(new Vector3(1295.57f, 580.55f, 34.26f),90.56f,"Choppy Shop","GRAH GRAH BOOM","GunShop1")
            {
                OpenTime = 0,
                CloseTime = 24,
                IsEnabled = true,
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
                ContactName = StaticStrings.UndergroundGunsContactName,
                ParkingSpaces = new List<SpawnPlace>() {
                        new SpawnPlace(new Vector3(1284.30f, 550.90f, 33.12f),181.34f),
                    },
                StateID = StaticStrings.LibertyStateID,
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(1305.167f, 585.7399f, 34.24616f), 121.8806f) },
            },
            new GunStore(new Vector3(311.1f, 151.3f, 11.16f),86.28f,"Blicky Bodega","Protect yourself from the opposition","GunShop2")//new Vector3(311.1f, 151.3f, 11.16f),86.28f
            {
                OpenTime = 0,
                CloseTime = 24,
                IsEnabled = true,
                ContactName = StaticStrings.UndergroundGunsContactName,
                ParkingSpaces = new List<SpawnPlace>() 
                {
                    new SpawnPlace(new Vector3(332.38f, 148.26f, 13.98f),177.80f),
                },
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
                //CameraPosition = new Vector3(317.0506f, 153.9417f, 12.91824f), 
                //CameraDirection = new Vector3(0.378016f, 0.9001398f, -0.2164539f), 
                //CameraRotation = new Rotator(-12.50084f, -1.093132E-05f, -22.78009f),
                StateID = StaticStrings.LibertyStateID,
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(321.4965f, 156.7243f, 11.17347f), 122.1338f) },
            },
            new GunStore(new Vector3(-1501.22f, 846.25f, 25.45f),234.43f,"Knocks Central","Essential business","GunShop3")
            {
                OpenTime = 0,
                CloseTime = 24,
                IsEnabled = true,
                ContactName = StaticStrings.UndergroundGunsContactName,
                ParkingSpaces = new List<SpawnPlace>() {
                    new SpawnPlace(new Vector3(-1499.26f, 840.65f, 25.13f),329.66f),
                },
                StateID = StaticStrings.LibertyStateID,
            },
                };
        LibertyCityLocations.GunStores.AddRange(GunStores);

    }
    private void DefaultConfig_BlankLocations()
    {
        BlankLocationsData_Liberty blankLocationsData_Liberty = new BlankLocationsData_Liberty();
        blankLocationsData_Liberty.DefaultConfig();
        LibertyCityLocations.BlankLocations.AddRange(blankLocationsData_Liberty.BlankLocationPlaces);
    }

}

