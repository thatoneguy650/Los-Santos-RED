using LosSantosRED.lsr.Helper;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


public class PlacesOfInterest_SunshineDream
{
    private PossibleLocations SunshineDreamLocations;
    public void DefaultConfig()
    {
        SunshineDreamLocations = new PossibleLocations();
        //These are for centered above, remove 200 from height
        DefaultConfig_Other();
        //DefaultConfig_PoliceStations();
        //DefaultConfig_Hospitals();
        //DefaultConfig_Prisons();
        DefaultConfig_Supplied();
        DefaultConfig_GangDens();



        PedCustomizerLocation DefaultPedCustomizerLocation = new PedCustomizerLocation();
        DefaultPedCustomizerLocation.DefaultModelPedPosition = new Vector3(-1821.874f, -667.2667f, 2.660995f);
        DefaultPedCustomizerLocation.DefaultModelPedHeading = 121.216f;
        DefaultPedCustomizerLocation.DefaultPlayerHoldingPosition = new Vector3(-1835.039f, -661.7596f, 2.660995f);
        List<CameraCyclerPosition> CameraCyclerPositions = new List<CameraCyclerPosition>();
        CameraCyclerPositions.Add(new CameraCyclerPosition("Default", new Vector3(-1824.512f, -669.0907f, 3.254323f), new Vector3(0.851868f, 0.5014042f, -0.1513762f), new Rotator(-8.706689f, 4.750499E-06f, -59.5192f), 0));//new Vector3(402.8145f, -998.5043f, -98.29621f), new Vector3(-0.02121102f, 0.9286007f, -0.3704739f), new Rotator(-21.74485f, -5.170386E-07f, 1.308518f), 0));
        CameraCyclerPositions.Add(new CameraCyclerPosition("Face", new Vector3(-1824.512f, -669.0907f, 3.254323f), new Vector3(0.851868f, 0.5014042f, -0.1513762f), new Rotator(-8.706689f, 4.750499E-06f, -59.5192f), 1));
        CameraCyclerPositions.Add(new CameraCyclerPosition("Lower", new Vector3(-1824.512f, -669.0907f, 3.254323f), new Vector3(0.851868f, 0.5014042f, -0.1513762f), new Rotator(-8.706689f, 4.750499E-06f, -59.5192f), 2));
        CameraCyclerPositions.Add(new CameraCyclerPosition("Torso", new Vector3(-1824.512f, -669.0907f, 3.254323f), new Vector3(0.851868f, 0.5014042f, -0.1513762f), new Rotator(-8.706689f, 4.750499E-06f, -59.5192f), 3));
        CameraCyclerPositions.Add(new CameraCyclerPosition("Hands", new Vector3(-1824.512f, -669.0907f, 3.254323f), new Vector3(0.851868f, 0.5014042f, -0.1513762f), new Rotator(-8.706689f, 4.750499E-06f, -59.5192f), 4));
        DefaultPedCustomizerLocation.CameraCyclerPositions = CameraCyclerPositions;


        //CameraPosition = new Vector3(-1824.512f, -669.0907f, 3.254323f), CameraDirection = new Vector3(0.851868f, 0.5014042f, -0.1513762f), CameraRotation = new Rotator(-8.706689f, 4.750499E-06f, -59.5192f)

        SunshineDreamLocations.PedCustomizerLocation = DefaultPedCustomizerLocation;

        Serialization.SerializeParam(SunshineDreamLocations, "Plugins\\LosSantosRED\\AlternateConfigs\\SunshineDream\\Locations_SunshineDream.xml");
    }

    private void DefaultConfig_Other()
    {
        //ped customize places
        //new Vector3(-2017.507f, -781.601f, 8.676593f,), 46.41839f  //LocationType Gold Coast Hotel Room


        //new Interior(3842, "Name Gold Coast Hotel"),


        //alt1 
        //new Vector3(1043.011f, 2848.557f, 2.434071f), 191.2893f  //LocationType Mortain Mall
    }
    private void DefaultConfig_GangDens()
    {
        List<GangDen> LCGangDens = new List<GangDen>()
        {
            new GangDen(new Vector3(-1683.321f, 984.329f, 6.509592f), 252.1058f, "Armenian Hangout", "", "ArmenianDenMenu", "AMBIENT_GANG_ARMENIAN")
            {
                IsPrimaryGangDen = true,
                CanInteractWhenWanted = true,
                MapIcon = 541,
                BannerImagePath = "gangs\\armenian.png",
                OpenTime = 0,
                CloseTime = 24,
                IsEnabled = true,
                StateID = StaticStrings.LeonidaStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(-1672.252f, 987.6925f, 2.679666f), 254.2377f, 75f){ TaskRequirements = TaskRequirements.Guard },
                    new GangConditionalLocation(new Vector3(-1676.534f, 995.0093f, 2.679703f), 308.6421f, 75f){ TaskRequirements = TaskRequirements.Guard },
                    new GangConditionalLocation(new Vector3(-1700.064f, 978.8469f, 2.679667f), 122.4722f, 75f){ TaskRequirements = TaskRequirements.Guard },
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(-1682.491f, 970.5074f, 2.476591f), 344.0008f, 75f),
                    new GangConditionalLocation(new Vector3(-1689.95f, 972.4074f, 2.477526f), 344.2486f, 75f),
                }
            },
        };
        SunshineDreamLocations.GangDens.AddRange(LCGangDens);
    }
    //private void DefaultConfig_Prisons()
    //{
    //    List<Prison> VicePrison = new List<Prison>()
    //    {
    //        //new Prison(new Vector3(-903.9021f, 118.7461f, 3.080931f), 91.96844f, "Vice Correctional Facility","") { OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LeonidaStateID },
    //    };
    //    SunshineDreamLocations.Prisons.AddRange(VicePrison);
    //}
    //private void DefaultConfig_Hospitals()
    //{
    //    List<Hospital> Hospitals = new List<Hospital>()
    //    {
    //        new Hospital(new Vector3(-1797.433f, -624.8724f, 2.661737f), 210.8405f, "Vice Beach Pharmacy","") { OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LeonidaStateID },
    //    };
    //    SunshineDreamLocations.Hospitals.AddRange(Hospitals);
    //}
    //private void DefaultConfig_PoliceStations()
    //{
    //    List<PoliceStation> PoliceStations = new List<PoliceStation>()
    //    {
    //        new PoliceStation(new Vector3(-1656.784f, 178.722f, 3.530202f), 57.65264f, "Vice Beach Police Department","") {OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LeonidaStateID },
    //    };
    //    SunshineDreamLocations.PoliceStations.AddRange(PoliceStations);
    //}
    private void DefaultConfig_Supplied()
    {
        List<PoliceStation> PoliceStations = new List<PoliceStation>()
        {
            new PoliceStation(new Vector3(-1656.784f, 178.722f, 3.530202f), 57.65264f, "Vice Beach Police Department", "")
            {
                Name = "Vice Beach Police Department",
                EntrancePosition = new Vector3(-1656.784f, 178.722f, 3.530202f),
                EntranceHeading = 57.65264f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LeonidaStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(-1660.57f, 174.2597f, 3.531038f), 77.53107f, 75f),
                    new LEConditionalLocation(new Vector3(-1655.28f, 183.5535f, 3.531041f), 50.96299f, 75f),
                    new LEConditionalLocation(new Vector3(-1657.841f, 198.5535f, 2.594363f), 10.96714f, 75f),
                    new LEConditionalLocation(new Vector3(-1667.451f, 165.6819f, 2.610533f), 104.9924f, 75f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(-1667.955f, 150.2497f, 2.559577f), 272.0714f, 75f) { IsEmpty = true, },
                    new LEConditionalLocation(new Vector3(-1667.522f, 146.124f, 2.549562f), 273.4885f, 75f) { IsEmpty = true, },
                    new LEConditionalLocation(new Vector3(-1657.689f, 142.7119f, 2.525447f), 89.22874f, 75f) { IsEmpty = true, },
                },
            },
            new PoliceStation(new Vector3(843.763f, 820.71f, 3.759f), 88.369f, "Downtown Courthouse", "")
            {
                Name = "Downtown Courthouse",
                EntrancePosition = new Vector3(843.763f, 820.71f, 3.759f),
                EntranceHeading = 88.369f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LeonidaStateID,
            },
            new PoliceStation(new Vector3(2225.321f, 1014.969f, 3.808f), 93.134f, "Coral City Police Department", "")
            {
                Name = "Coral City Police Department",
                EntrancePosition = new Vector3(2225.321f, 1014.969f, 3.808f),
                EntranceHeading = 93.134f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LeonidaStateID,
            },
            new PoliceStation(new Vector3(1015.738f, 700.687f, 2.634f), 310.108f, "Vice City Police Department", "")
            {
                Name = "Vice City Police Department",
                EntrancePosition = new Vector3(1015.738f, 700.687f, 2.634f),
                EntranceHeading = 310.108f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LeonidaStateID,
            },
            new PoliceStation(new Vector3(578.864f, -49.636f, 2.681f), 193.102f, "Downtown Police Department", "")
            {
                Name = "Downtown Police Department",
                EntrancePosition = new Vector3(578.864f, -49.636f, 2.681f),
                EntranceHeading = 193.102f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LeonidaStateID,
            },
        };
        SunshineDreamLocations.PoliceStations.AddRange(PoliceStations);
        List<Hospital> Hospitals = new List<Hospital>()
        {
            new Hospital(new Vector3(-1071.904f, 485.812f, 2.682f), 262.232f, "Vice Beach Hospital", "")
            {
                Name = "Vice Beach Hospital",
                EntrancePosition = new Vector3(-1071.904f, 485.812f, 2.682f),
                EntranceHeading = 262.232f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LeonidaStateID,
            },
            new Hospital(new Vector3(-1737.824f, 704.377f, 2.692f), 281.271f, "Vice Clinic", "")
            {
                Name = "Vice Clinic",
                EntrancePosition = new Vector3(-1737.824f, 704.377f, 2.692f),
                EntranceHeading = 281.271f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LeonidaStateID,
            },
            new Hospital(new Vector3(723.331f, 2135.241f, 2.678f), 275.403f, "Mercy Hospital", "")
            {
                Name = "Mercy Hospital",
                EntrancePosition = new Vector3(723.331f, 2135.241f, 2.678f),
                EntranceHeading = 275.403f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LeonidaStateID,
            },
        };
        SunshineDreamLocations.Hospitals.AddRange(Hospitals);

        List<Bank> Banks = new List<Bank>()
        {
            new Bank(new Vector3(-1531.095f, 9.961f, 2.682f), 2.764f, "Beach Bank", "Our time is your money","BB")
            {
                Name = "Beach Bank",
                Description = "Our time is your money",
                EntrancePosition = new Vector3(-1531.095f, 9.961f, 2.682f),
                EntranceHeading = 2.764f,
                StateID = StaticStrings.LeonidaStateID,
            },
            new Bank(new Vector3(407.459f, 221.496f, 2.681f), 82.788f, "Downtown Bank", "Our time is your money","DWNT")
            {
                Name = "Downtown Bank",
                Description = "Our time is your money",
                EntrancePosition = new Vector3(407.459f, 221.496f, 2.681f),
                EntranceHeading = 82.788f,
                StateID = StaticStrings.LeonidaStateID,
            },
            new Bank(new Vector3(1157.523f, 3036.054f, 2.675f), 76.988f, "Coco Grove Bank HQ", "Our time is your money","COCO")
            {
                Name = "Coco Grove Bank HQ",
                Description = "Our time is your money",
                EntrancePosition = new Vector3(1157.523f, 3036.054f, 2.675f),
                EntranceHeading = 76.988f,
                StateID = StaticStrings.LeonidaStateID,
            },
            new Bank(new Vector3(1331.051f, 3019.667f, 2.688f), 181.62f, "Coco Grove Bank Small", "Our time is your money","COCO")
            {
                Name = "Coco Grove Bank Small",
                Description = "Our time is your money",
                EntrancePosition = new Vector3(1331.051f, 3019.667f, 2.688f),
                EntranceHeading = 181.62f,
                StateID = StaticStrings.LeonidaStateID,
            },
        };
        SunshineDreamLocations.Banks.AddRange(Banks);
        List<Landmark> Landmarks = new List<Landmark>()
        {
            new Landmark(new Vector3(316.474f, 19.419f, 8.665f), 0f, "Hyman Memorial Arena", "I heard Phil Collins sung here.")
            {
                Name = "Hyman Memorial Arena",
                Description = "I heard Phil Collins sung here.",
                EntrancePosition = new Vector3(316.474f, 19.419f, 8.665f),
                OpenTime = 0,
                CloseTime = 24,
                AssignedAssociationID = "GRP6",
                StateID = StaticStrings.LeonidaStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new SecurityConditionalLocation(new Vector3(307.700f,42.293f,10.664f),239.393f,55) { RequiredPedGroup = "UnarmedSecurity",TaskRequirements = TaskRequirements.Guard | TaskRequirements.StandardScenario },
                    new SecurityConditionalLocation(new Vector3(303.517f,26.559f,10.668f),248.851f,55) { RequiredPedGroup = "UnarmedSecurity",TaskRequirements = TaskRequirements.Guard | TaskRequirements.StandardScenario },
                    new SecurityConditionalLocation(new Vector3(296.837f,11.148f,10.668f),248.264f,55) { RequiredPedGroup = "UnarmedSecurity",TaskRequirements = TaskRequirements.Guard | TaskRequirements.StandardScenario },
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new SecurityConditionalLocation(new Vector3(290.904f,-10.297f,1.970f),159.386f,55) { IsEmpty = true, RequiredPedGroup = "UnarmedSecurity",TaskRequirements = TaskRequirements.Guard | TaskRequirements.StandardScenario },
                },
            },
        };
        SunshineDreamLocations.Landmarks.AddRange(Landmarks);
        List<GunStore> GunStores = new List<GunStore>()
        {
            new GunStore(new Vector3(-1408.402f, 1042.958f, 2.748f), 281.271f, "Stonewall Js", "General shop","GunShop1")
            {
                Name = "Stonewall Js",
                Description = "General shop",
                EntrancePosition = new Vector3(-1408.402f, 1042.958f, 2.748f),
                EntranceHeading = 281.271f,
                StateID = StaticStrings.LeonidaStateID,
            },
        };
        SunshineDreamLocations.GunStores.AddRange(GunStores);
        List<Hotel> Hotels = new List<Hotel>()
        {
            new Hotel(new Vector3(-1976.206f, 257.672f, 2.692f), 57.65264f, "Colony Hotel", "Oceanfront Hotel Located Along The Famed Ocean Drive!","CheapHotelMenu")
            {
                Name = "Colony Hotel",
                Description = "Oceanfront Hotel Located Along The Famed Ocean Drive!",
                EntrancePosition = new Vector3(-1976.206f, 257.672f, 2.692f),
                EntranceHeading = 57.65264f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LeonidaStateID,
            },
            new Hotel(new Vector3(-1972.939f, 156.074f, 2.692f), 57.65264f, "Hotel Breakwater", "In the Heart of Ocean Beach","CheapHotelMenu")
            {
                Name = "Hotel Breakwater",
                Description = "In the Heart of Ocean Beach",
                EntrancePosition = new Vector3(-1972.939f, 156.074f, 2.692f),
                EntranceHeading = 57.65264f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LeonidaStateID,
            },
            new Hotel(new Vector3(-1879.58f, -443.545f, 2.989f), 94.491f, "Loews Hotel", "A Summer to Remember!","CheapHotelMenu")
            {
                Name = "Loews Hotel",
                Description = "A Summer to Remember!",
                EntrancePosition = new Vector3(-1879.58f, -443.545f, 2.989f),
                EntranceHeading = 94.491f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LeonidaStateID,
            },
            new Hotel(new Vector3(-1972.768f, 71.861f, 2.782f), 57.65264f, "Clevelander Hotel", "Paradise in Miami!","CheapHotelMenu")
            {
                Name = "Clevelander Hotel",
                Description = "Paradise in Miami!",
                EntrancePosition = new Vector3(-1972.768f, 71.861f, 2.782f),
                EntranceHeading = 57.65264f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LeonidaStateID,
            },
            new Hotel(new Vector3(-1972.432f, 26.34f, 3.066f), 57.65264f, "Decowalk Hotel", "A Summer to Remember!","CheapHotelMenu")
            {
                Name = "Decowalk Hotel",
                Description = "A Summer to Remember!",
                EntrancePosition = new Vector3(-1972.432f, 26.34f, 3.066f),
                EntranceHeading = 57.65264f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LeonidaStateID,
            },
            new Hotel(new Vector3(177.24f, 1116.312f, 2.942f), 2.443f, "Hotel Twinn", "Paradise in Miami!","CheapHotelMenu")
            {
                Name = "Hotel Twinn",
                Description = "Paradise in Miami!",
                EntrancePosition = new Vector3(177.24f, 1116.312f, 2.942f),
                EntranceHeading = 2.443f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LeonidaStateID,
            },
            new Hotel(new Vector3(2433.271f, 1796.652f, 2.717f), 358.874f, "Biltmore Hotel", "A Summer to Remember!","CheapHotelMenu")
            {
                Name = "Biltmore Hotel",
                Description = "A Summer to Remember!",
                EntrancePosition = new Vector3(2433.271f, 1796.652f, 2.717f),
                EntranceHeading = 358.874f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LeonidaStateID,
            },
            new Hotel(new Vector3(-1973.665f, -277.137f, 2.692f), 57.65264f, "Cavalier Hotel", "Paradise in Miami!","CheapHotelMenu")
            {
                Name = "Cavalier Hotel",
                Description = "Paradise in Miami!",
                EntrancePosition = new Vector3(-1973.665f, -277.137f, 2.692f),
                EntranceHeading = 57.65264f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LeonidaStateID,
            },
            new Hotel(new Vector3(-245.26f, -242.102f, 5.897f), 281.271f, "Venetian Hotel", "Paradise in Miami!","CheapHotelMenu")
            {
                Name = "Venetian Hotel",
                Description = "Paradise in Miami!",
                EntrancePosition = new Vector3(-245.26f, -242.102f, 5.897f),
                EntranceHeading = 281.271f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LeonidaStateID,
            },
        };
        SunshineDreamLocations.Hotels.AddRange(Hotels);
        List<Residence> Residences = new List<Residence>()
        {
            new Residence(new Vector3(-1915.371f, 573.504f, 2.585f), 69.24533f, "4 Ocean Drive Apt 35", "")
            {
                Name = "4 Ocean Drive Apt 35",
                EntrancePosition = new Vector3(-1915.371f, 573.504f, 2.585f),
                EntranceHeading = 69.24533f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LeonidaStateID,
            },
        };
        SunshineDreamLocations.Residences.AddRange(Residences);
        List<ApartmentBuilding> ApartmentBuildings = new List<ApartmentBuilding>()
        {
            new ApartmentBuilding(new Vector3(-1874.441f, -604.984f, 2.663f), 243.528f, "Alamo Apartments", "")
            {
                Name = "Alamo Apartments",
                EntrancePosition = new Vector3(-1874.441f, -604.984f, 2.663f),
                EntranceHeading = 243.528f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LeonidaStateID,
            },
            new ApartmentBuilding(new Vector3(-1415.912f, 416.574f, 2.592f), 5.139f, "Parkside Apartments", "")
            {
                Name = "Parkside Apartments",
                EntrancePosition = new Vector3(-1415.912f, 416.574f, 2.592f),
                EntranceHeading = 5.139f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LeonidaStateID,
            },
            new ApartmentBuilding(new Vector3(1190.228f, 3078.698f, 2.678f), 136.376f, "Coco Grove Apartments", "")
            {
                Name = "Coco Grove Apartments",
                EntrancePosition = new Vector3(1190.228f, 3078.698f, 2.678f),
                EntranceHeading = 136.376f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LeonidaStateID,
            },
        };
        SunshineDreamLocations.ApartmentBuildings.AddRange(ApartmentBuildings);

        List<CityHall> CityHalls = new List<CityHall>()
        {
            new CityHall(new Vector3(-1671.281f, 108.342f, 2.672f), 57.65264f, "Vice Beach City Hall", "")
            {
                Name = "Vice Beach City Hall",
                EntrancePosition = new Vector3(-1671.281f, 108.342f, 2.672f),
                EntranceHeading = 57.65264f,
                OpenTime = 9,
                CloseTime = 18,
                AssignedAssociationID = "SECURO",
                StateID = StaticStrings.LeonidaStateID,
            },
            new CityHall(new Vector3(783.612f, 2920.142f, 2.678f), 59.267f, "Vice City Hall", "")
            {
                Name = "Vice City Hall",
                EntrancePosition = new Vector3(783.612f, 2920.142f, 2.678f),
                EntranceHeading = 59.267f,
                OpenTime = 9,
                CloseTime = 18,
                AssignedAssociationID = "SECURO",
                StateID = StaticStrings.LeonidaStateID,
            },
            new CityHall(new Vector3(2116.246f, 979.475f, 4.078f), 149.415f, "Coral City Hall", "")
            {
                Name = "Coral City Hall",
                EntrancePosition = new Vector3(2116.246f, 979.475f, 4.078f),
                EntranceHeading = 149.415f,
                OpenTime = 9,
                CloseTime = 18,
                AssignedAssociationID = "SECURO",
                StateID = StaticStrings.LeonidaStateID,
            },
            new CityHall(new Vector3(1004.699f, 792.522f, 5.536f), 137.771f, "Government Center", "")
            {
                Name = "Government Center",
                EntrancePosition = new Vector3(1004.699f, 792.522f, 5.536f),
                EntranceHeading = 137.771f,
                OpenTime = 9,
                CloseTime = 18,
                AssignedAssociationID = "SECURO",
                StateID = StaticStrings.LeonidaStateID,
            },
        };
        SunshineDreamLocations.CityHalls.AddRange(CityHalls);

        List<Restaurant> Restaurants = new List<Restaurant>()
        {
            new Restaurant(new Vector3(-1204.641f, 150.633f, 2.837f), 281.271f, "Parkside Cocktail Lounge", "Drink til you pop!","FancyGenericMenu",FoodType.Generic)
            {
                Name = "Parkside Cocktail Lounge",
                Description = "Drink til you pop!",
                EntrancePosition = new Vector3(-1204.641f, 150.633f, 2.837f),
                EntranceHeading = 281.271f,
                StateID = StaticStrings.LeonidaStateID,
            },
            new Restaurant(new Vector3(-1697.073f, 119.988f, 2.682f), 281.271f, "Washington Sushi", "The finest from Japan!","FancyGenericMenu",FoodType.Japanese)
            {
                Name = "Washington Sushi",
                Description = "The finest from Japan!",
                EntrancePosition = new Vector3(-1697.073f, 119.988f, 2.682f),
                EntranceHeading = 281.271f,
                StateID = StaticStrings.LeonidaStateID,
            },
            new Restaurant(new Vector3(-1524.24f, 1333.983f, 2.848f), 171.359f, "Ocean Heights Steakhouse", "A steakhouse to end all arguments.","FancyGenericMenu",FoodType.American)
            {
                Name = "Ocean Heights Steakhouse",
                Description = "A steakhouse to end all arguments.",
                EntrancePosition = new Vector3(-1524.24f, 1333.983f, 2.848f),
                EntranceHeading = 171.359f,
                StateID = StaticStrings.LeonidaStateID,
            },
            new Restaurant(new Vector3(-1547.686f, -6.785f, 2.68f), 221.569f, "Paesano Pasta House", "Here is an order you can't refuse!","PizzaMenu",FoodType.Pizza)
            {
                Name = "Paesano Pasta House",
                Description = "Here is an order you can't refuse!",
                EntrancePosition = new Vector3(-1547.686f, -6.785f, 2.68f),
                EntranceHeading = 221.569f,
                StateID = StaticStrings.LeonidaStateID,
            },
            new Restaurant(new Vector3(607.166f, 259.214f, 2.681f), 193.675f, "Burger Queen", "Royal Beef!","WigwamMenu",FoodType.American)
            {
                Name = "Burger Queen",
                Description = "Royal Beef!",
                EntrancePosition = new Vector3(607.166f, 259.214f, 2.681f),
                EntranceHeading = 193.675f,
                StateID = StaticStrings.LeonidaStateID,
            },
            new Restaurant(new Vector3(595.478f, 2288.89f, 2.828f), 46.324f, "Los Tres Juanes", "A taste of Cuba!","TacoFarmerMenu",FoodType.Mexican)
            {
                Name = "Los Tres Juanes",
                Description = "A taste of Cuba!",
                EntrancePosition = new Vector3(595.478f, 2288.89f, 2.828f),
                EntranceHeading = 46.324f,
                StateID = StaticStrings.LeonidaStateID,
            },
        };
        SunshineDreamLocations.Restaurants.AddRange(Restaurants);

        List<ConvenienceStore> ConvenienceStores = new List<ConvenienceStore>()
        {
            new ConvenienceStore(new Vector3(-1933.332f, 528.623f, 2.69f), 57.65264f, "Beachside Market", "Local Convenience Store","GrainOfTruthMenu")
            {
                Name = "Beachside Market",
                Description = "Local Convenience Store",
                EntrancePosition = new Vector3(-1933.332f, 528.623f, 2.69f),
                EntranceHeading = 57.65264f,
                StateID = StaticStrings.LeonidaStateID,
            },
        };
        SunshineDreamLocations.ConvenienceStores.AddRange(ConvenienceStores);

        List<GasStation> GasStations = new List<GasStation>()
        {
            new GasStation(new Vector3(-1814.758f, -506.155f, 2.512f), 187.039f, "Miami Beach Ron", "Put RON in your tank","RonMenu")
            {
                Name = "Miami Beach Ron",
                Description = "Put RON in your tank",
                EntrancePosition = new Vector3(-1814.758f, -506.155f, 2.512f),
                EntranceHeading = 187.039f,
                OpenTime = 4,
                CloseTime = 22,
                StateID = StaticStrings.LeonidaStateID,
            },
            new GasStation(new Vector3(613.994f, -148.807f, 2.681f), 314.996f, "Downtown Ron", "Put RON in your tank","RonMenu")
            {
                Name = "Downtown Ron",
                Description = "Put RON in your tank",
                EntrancePosition = new Vector3(613.994f, -148.807f, 2.681f),
                EntranceHeading = 314.996f,
                OpenTime = 4,
                CloseTime = 22,
                StateID = StaticStrings.LeonidaStateID,
            },
        };
        SunshineDreamLocations.GasStations.AddRange(GasStations);

        List<Bar> Bars = new List<Bar>()
        {
            new Bar(new Vector3(1267.517f, 2895.548f, 2.688f), 114.843f, "Senor Spiky","", "BarMenu")
            {
                Name = "Senor Spiky",
                EntrancePosition = new Vector3(1267.517f, 2895.548f, 2.688f),
                EntranceHeading = 114.843f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LeonidaStateID,
            },
        };
        SunshineDreamLocations.Bars.AddRange(Bars);

        List<Forger> Forgers = new List<Forger>()
        {
            new Forger(new Vector3(-1674.394f, 487.539f, 2.681f), 57.65264f, "Vice Forgeries", "Fake it til you Make it!")
            {
                Name = "Vice Forgeries",
                Description = "Fake it til you Make it!",
                EntrancePosition = new Vector3(-1674.394f, 487.539f, 2.681f),
                EntranceHeading = 57.65264f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LeonidaStateID,
            },
        };
        SunshineDreamLocations.Forgers.AddRange(Forgers);

        List<ClothingShop> ClothingShops = new List<ClothingShop>()
        {
            new ClothingShop(new Vector3(-1198.206f, 202.154f, 2.836f), 57.65264f, "Stages", "Catering to the Elite","",Vector3.Zero)
            {
                Name = "Stages",
                Description = "Catering to the Elite",
                EntrancePosition = new Vector3(-1198.206f, 202.154f, 2.836f),
                EntranceHeading = 57.65264f,
                OpenTime = 8,
                StateID = StaticStrings.LeonidaStateID,
            },
            new ClothingShop(new Vector3(1311.919f, 3100.109f, 2.688f), 59.587f, "A Rexia Fashions", "Fashion to die for!","",Vector3.Zero)
            {
                Name = "A Rexia Fashions",
                Description = "Fashion to die for!",
                EntrancePosition = new Vector3(1311.919f, 3100.109f, 2.688f),
                EntranceHeading = 59.587f,
                OpenTime = 8,
                StateID = StaticStrings.LeonidaStateID,
            },
            new ClothingShop(new Vector3(687.73f, 846.63f, 2.541f), 238.845f, "Burdines", "Catering to the Elite", "", Vector3.Zero)
            {
                Name = "Burdines",
                Description = "Catering to the Elite",
                EntrancePosition = new Vector3(687.73f, 846.63f, 2.541f),
                EntranceHeading = 238.845f,
                OpenTime = 8,
                StateID = StaticStrings.LeonidaStateID,
            },
        };
        SunshineDreamLocations.ClothingShops.AddRange(ClothingShops);

        List<IllicitMarketplace> IllicitMarketplaces = new List<IllicitMarketplace>()
        {
            new IllicitMarketplace(new Vector3(-30.06856f, 6456.729f, 31.47281f), 243.118f, "Dealer Hangout WP", "Dealer Hangout","DealerHangoutMenu1")
            {
                Name = "Dealer Hangout WP",
                Description = "Dealer Hangout",
                EntrancePosition = new Vector3(-30.06856f, 6456.729f, 31.47281f),
                EntranceHeading = 243.118f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LeonidaStateID,
            },
        };
        SunshineDreamLocations.IllicitMarketplaces.AddRange(IllicitMarketplaces);
    }
}

