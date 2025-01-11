using Blackjack;
using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Locations;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;


public class PlacesOfInterest : IPlacesOfInterest
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Locations.xml";
    private IShopMenus ShopMenus;
    private IGangs Gangs;
    private List<DeadDrop> DeadDrops;
    private List<CarCrusher> CarCrushers;
    private List<ScrapYard> ScrapYards;
    private List<GangDen> GangDens;
    private List<GunStore> GunStores;
    private List<Hotel> Hotels;
    private List<Residence> Residences;
    private List<CityHall> CityHalls;
    private List<PoliceStation> PoliceStations;
    private List<Prison> Prisons;
    private List<Hospital> Hospitals;
    private List<FireStation> FireStations;
    private List<Restaurant> Restaurants;
    private List<Pharmacy> Pharmacies;
    private List<Dispensary> Dispensaries;
    private List<HeadShop> HeadShops;
    private List<SportingGoodsStore> SportingGoodsStores;
    private List<HardwareStore> HardwareStores;
    private List<PawnShop> PawnShops;
    private List<Landmark> Landmarks;
    private List<SubwayStation> SubwayStations;
    //private List<BeautyShop> BeautyShops;
    private List<Bank> Banks;
    private List<ConvenienceStore> ConvenienceStores;
    private List<LiquorStore> LiquorStores;
    private List<GasStation> GasStations;
    private List<Bar> Bars;
    private List<FoodStand> FoodStands;
    private List<Forger> Forgers;
    private List<GamblingDen> GamblingDens;
    private List<RepairGarage> RepairGarages;
    private List<IllicitMarketplace> illicitMarketplaces;
    private List<Dealership> Dealerships;
    private List<VehicleExporter> VehicleExporters;
    private List<DriveThru> DriveThrus;
    private List<ClothingShop> ClothingShops;
    private List<BusStop> BusStops;
    private List<Morgue> Morgues;
    private List<Airport> Airports;
    private List<BlankLocation> BlankLocationPlaces;
    private List<ApartmentBuilding> ApartmentBuildings;
    private List<MilitaryBase> MilitaryBasePlaces;
    private List<StoredSpawn> TunnelSpawnPlaces;
    private List<TattooShop> TattooShopPlaces;
    private List<PlasticSurgeryClinic> PlasticSurgeryClinics;
    private List<BarberShop> BarberShopPlaces;
    private PedCustomizerLocation DefaultPedCustomizerLocation;

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
            DefaultConfig_SunshineDream();
            DefaultConfig();
            DefaultConfig_LibertyCity();
            DefaultConfig_2008();
        }
    }
    public List<GameLocation> InteractableLocations()
    {
        return PossibleLocations.InteractableLocations();
    }
    public List<ILocationSetupable> LocationsToSetup()
    {
        List<ILocationSetupable> AllLocations = new List<ILocationSetupable>();
        AllLocations.AddRange(PossibleLocations.Airports);
        AllLocations.AddRange(PossibleLocations.Residences);
        return AllLocations;
    }
    public List<GameLocation> AllLocations()
    {
        List<GameLocation> AllLocations = new List<GameLocation>();
        AllLocations.AddRange(InteractableLocations());
        return AllLocations;
    }
    public List<ILocationRespawnable> BustedRespawnLocations()
    {
        List<ILocationRespawnable> AllLocations = new List<ILocationRespawnable>();
        AllLocations.AddRange(PossibleLocations.PoliceStations);
        AllLocations.AddRange(PossibleLocations.Prisons);
        return AllLocations;
    }
    public List<ILocationAreaRestrictable> RestrictedAreaLocations()
    {
        List<ILocationAreaRestrictable> AllLocations = new List<ILocationAreaRestrictable>();
        AllLocations.AddRange(PossibleLocations.PoliceStations);
        AllLocations.AddRange(PossibleLocations.Prisons);
        AllLocations.AddRange(PossibleLocations.MilitaryBases);
        return AllLocations;
    }
    public List<ILocationImpoundable> VehicleImpoundLocations()
    {
        List<ILocationImpoundable> AllLocations = new List<ILocationImpoundable>();
        AllLocations.AddRange(PossibleLocations.PoliceStations);
        return AllLocations;
    }
    public List<ILocationRespawnable> HospitalRespawnLocations()
    {
        List<ILocationRespawnable> AllLocations = new List<ILocationRespawnable>();
        AllLocations.AddRange(PossibleLocations.Hospitals);
        return AllLocations;
    }
    public List<ILEDispatchableLocation> LEDispatchLocations()
    {
        List<ILEDispatchableLocation> AllLocations = new List<ILEDispatchableLocation>();
        AllLocations.AddRange(PossibleLocations.PoliceStations);
        AllLocations.AddRange(PossibleLocations.Prisons);
        AllLocations.AddRange(PossibleLocations.MilitaryBases);
        return AllLocations;
    }
    public DeadDrop GetUsableDeadDrop(bool IsMPMap, LocationData locationData)
    {
        return PossibleLocations.DeadDrops.Where(x => x.CanUse && x.IsCorrectMap(IsMPMap) && x.IsSameState(locationData?.CurrentZone?.GameState)).PickRandom();// IsMPMap == x.IsOnMPMap).PickRandom();
    }
    public GangDen GetMainDen(string gangID, bool IsMPMap, LocationData locationData)
    {
        GangDen mainDen = PossibleLocations.GangDens.Where(x => x.AssociatedGang?.ID == gangID && x.IsPrimaryGangDen && x.IsSameState(locationData?.CurrentZone?.GameState) && x.IsCorrectMap(IsMPMap)).FirstOrDefault();
        if (mainDen != null)
        {
            return mainDen;
        }
        return PossibleLocations.GangDens.Where(x => x.AssociatedGang?.ID == gangID && x.IsSameState(locationData?.CurrentZone?.GameState) && x.IsCorrectMap(IsMPMap)).PickRandom();// IsMPMap == x.IsOnMPMap).PickRandom();
    }
    private void DefaultConfig()
    {
        DefaultConfig_PedCustomizeLocation();
        DefaultConfig_DeadDrops();
        DefaultConfig_CarCrushers();
        DefaultConfig_ScrapYards();
        DefaultConfig_GangDens();
        DefaultConfig_GunStores();
        DefaultConfig_Hotels();
        DefaultConfig_Residences();
        DefaultConfig_CityHalls();
        DefaultConfig_PoliceStations();
        DefaultConfig_Prisons();
        DefaultConfig_Hospitals();
        DefaultConfig_FireStations();
        DefaultConfig_Restaurants();
        DefaultConfig_Pharmacies();
        DefaultConfig_Dispensaries();
        DefaultConfig_HeadShops();
        DefaultConfig_SportingGoodsStores();
        DefaultConfig_HardwareStores();
        DefaultConfig_PawnShops();

        DefaultConfig_Landmarks();
        DefaultConfig_SubwayStations();
        //DefaultConfig_BeautyShops();
        DefaultConfig_Banks();
        DefaultConfig_ConvenienceStores();
        DefaultConfig_LiquorStores();
        DefaultConfig_GasStations();
        DefaultConfig_Bars();
        DefaultConfig_FoodStands();
        DefaultConfig_IllicitMarketplaces();
        DefaultConfig_BlankLocations();
        DefaultConfig_Dealerships();
        DefaultConfig_VehicleExporters();
        DefaultConfig_Forgers();
        DefaultConfig_GamblingDens();
        DefaultConfig_PayNSprays();

        DefaultConfig_DriveThrus();
        DefaultConfig_ClothingShops();
        DefaultConfig_BusStops();
        DefaultConfig_Morgues();
        DefaultConfig_Airports();
        DefaultConfig_MilitaryBases();
        DefaultConfig_TunnelSpawns();
        DefaultConfig_BarberShops();
        DefaultConfig_PlasticSurgeryClinics();
        DefaultConfig_TattooShops();

        PossibleLocations.DeadDrops.AddRange(DeadDrops);
        PossibleLocations.CarCrushers.AddRange(CarCrushers);
        PossibleLocations.ScrapYards.AddRange(ScrapYards);
        PossibleLocations.GangDens.AddRange(GangDens);
        PossibleLocations.GunStores.AddRange(GunStores);
        PossibleLocations.Hotels.AddRange(Hotels);
        PossibleLocations.ApartmentBuildings.AddRange(ApartmentBuildings);
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
        //PossibleLocations.BeautyShops.AddRange(BeautyShops);
        PossibleLocations.Banks.AddRange(Banks);
        PossibleLocations.ConvenienceStores.AddRange(ConvenienceStores);
        PossibleLocations.LiquorStores.AddRange(LiquorStores);
        PossibleLocations.GasStations.AddRange(GasStations);
        PossibleLocations.Bars.AddRange(Bars);
        PossibleLocations.FoodStands.AddRange(FoodStands);
        PossibleLocations.CarDealerships.AddRange(Dealerships);
        PossibleLocations.VehicleExporters.AddRange(VehicleExporters);
        PossibleLocations.Forgers.AddRange(Forgers);
        PossibleLocations.GamblingDens.AddRange(GamblingDens);
        PossibleLocations.RepairGarages.AddRange(RepairGarages);
        PossibleLocations.DriveThrus.AddRange(DriveThrus);
        PossibleLocations.ClothingShops.AddRange(ClothingShops);
        PossibleLocations.BusStops.AddRange(BusStops);
        PossibleLocations.Prisons.AddRange(Prisons);
        PossibleLocations.SubwayStations.AddRange(SubwayStations);
        PossibleLocations.Morgues.AddRange(Morgues);
        PossibleLocations.SportingGoodsStores.AddRange(SportingGoodsStores);
        PossibleLocations.Airports.AddRange(Airports);
        PossibleLocations.IllicitMarketplaces.AddRange(illicitMarketplaces);
        PossibleLocations.BlankLocations.AddRange(BlankLocationPlaces);
        PossibleLocations.MilitaryBases.AddRange(MilitaryBasePlaces);
        PossibleLocations.StoredSpawns.AddRange(TunnelSpawnPlaces);
        PossibleLocations.BarberShops.AddRange(BarberShopPlaces);
        PossibleLocations.PlasticSurgeryClinics.AddRange(PlasticSurgeryClinics);
        PossibleLocations.TattooShops.AddRange(TattooShopPlaces);

        PossibleLocations.PedCustomizerLocation = DefaultPedCustomizerLocation;


       Serialization.SerializeParam(PossibleLocations, ConfigFileName);
    }
    private void DefaultConfig_PedCustomizeLocation()
    {
        DefaultPedCustomizerLocation = new PedCustomizerLocation();
        DefaultPedCustomizerLocation.DefaultModelPedPosition = new Vector3(402.8473f, -996.7224f, -99.00025f);
        DefaultPedCustomizerLocation.DefaultModelPedHeading = 182.7549f;
        DefaultPedCustomizerLocation.DefaultPlayerHoldingPosition = new Vector3(402.5164f, -1002.847f, -99.2587f);
        List<CameraCyclerPosition> CameraCyclerPositions = new List<CameraCyclerPosition>();
        CameraCyclerPositions.Add(new CameraCyclerPosition("Default", new Vector3(402.9301f, -998.267f, -98.51537f), new Vector3(0.004358141f, 0.9860916f, -0.1661458f), new Rotator(-9.5638f, -4.058472E-08f, -0.2532234f), 0));//new Vector3(402.8145f, -998.5043f, -98.29621f), new Vector3(-0.02121102f, 0.9286007f, -0.3704739f), new Rotator(-21.74485f, -5.170386E-07f, 1.308518f), 0));
        CameraCyclerPositions.Add(new CameraCyclerPosition("Face", new Vector3(402.8708f, -997.5441f, -98.30454f), new Vector3(-0.005195593f, 0.9991391f, -0.04116036f), new Rotator(-2.358982f, 2.136245E-06f, 0.2979394f), 1));
        CameraCyclerPositions.Add(new CameraCyclerPosition("Lower", new Vector3(402.9348f, -998.0379f, -99.38499f), new Vector3(0.02025275f, 0.9928887f, -0.117311f), new Rotator(-6.736939f, 1.611956E-07f, -1.168546f), 2));
        CameraCyclerPositions.Add(new CameraCyclerPosition("Torso", new Vector3(402.9301f, -998.267f, -98.51537f), new Vector3(0.004358141f, 0.9860916f, -0.1661458f), new Rotator(-9.5638f, -4.058472E-08f, -0.2532234f), 3));
        CameraCyclerPositions.Add(new CameraCyclerPosition("Hands", new Vector3(402.8127f, -997.4653f, -99.04851f), new Vector3(0.0355651f, 0.9914218f, -0.1257696f), new Rotator(-7.225204f, -5.647735E-07f, -2.054481f), 4));
        DefaultPedCustomizerLocation.CameraCyclerPositions = CameraCyclerPositions;
    }
    private void DefaultConfig_BarberShops()
    {
        BarberShopPlaces = new List<BarberShop>()
        {
            new BarberShop(new Vector3(187.7006f, -1812.874f, 28.94536f), 323.7488f, "Carson's Beauty Supply", "")
            {
                InteriorID = -99099,
                VendorPersonnelID = "HaircutPeds",
                TypeName = "Beauty Salon",
                PossiblePedSpawns = new List<ConditionalLocation>() 
                {
                    new GangConditionalLocation(new Vector3(186.5544f, -1811.605f, 28.99609f), 4.797177f,55f) { TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>() { "WORLD_HUMAN_BUM_SLUMPED", "WORLD_HUMAN_MUSICIAN" } },
                    new GangConditionalLocation(new Vector3(191.4009f, -1815.011f, 28.78865f), 326.5257f,55f) { TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>() { "WORLD_HUMAN_BUM_SLUMPED", "WORLD_HUMAN_MUSICIAN" } },
                }
            },
            new BarberShop(new Vector3(442.8802f, -2044.086f, 23.73479f), 317.6541f, "Chantelle's Beauty Salon", "") 
            {
                InteriorID = -99099,
                VendorPersonnelID = "HaircutPeds",
                TypeName = "Beauty Salon",
            },
            new BarberShop(new Vector3(-342.2452f, -1482.83f, 30.70707f), 269.4036f, "Discount Beauty Store", "") 
            {
                InteriorID = -99099,
                VendorPersonnelID = "HaircutPeds",
                TypeName = "Beauty Salon",
            },
            new BarberShop(new Vector3(-3050.165f, 625.066f, 7.269026f), 290.7953f, "Belinda May's Beauty Salon", "") 
            {
                InteriorID = -99099,
                VendorPersonnelID = "HaircutPeds",
                TypeName = "Beauty Salon",
            },
            new BarberShop(new Vector3(1705.34f, 3780.338f, 34.7582f), 214.8316f, "Aunt Tammy's Hair Salon", "")
            {
                InteriorID = -99099,
                VendorPersonnelID = "HaircutPeds",
            },

            new BarberShop(new Vector3(-281.8168f, 6232.678f, 31.69073f), 45.57799f,"Herr Kutz Barber","")//Paleto
            {
                DisableRegularInteract = true,
                VendorPersonnelID = "HaircutPeds",
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-277.812f, 6229.735f, 30.69552f), 50.82846f) },
                InteriorID = 13058,
                BannerImagePath = "stores\\herrkutz.png",
            },
            new BarberShop(new Vector3(-1288.16f, -1116.506f, 6.985249f), 88.56965f,"Beach Combover Barbers","")//Vespucci
            {
                DisableRegularInteract = true,
                VendorPersonnelID = "HaircutPeds",
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-1283.989f, -1116.012f, 6.5f), 84.55875f) },
                InteriorID = 113922,//,113922,
                BannerImagePath = "stores\\beachcombover.png",
            },
            new BarberShop(new Vector3(-823.5811f, -188.0995f, 37.61986f), 120.2507f,"Bob Mulet","")//Rockford
            {
                DisableRegularInteract = true,
                VendorPersonnelID = "BobMuletPeds",
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-816.2922f, -186.2247f, 37.06889f), 77.19087f) },
                InteriorID = 37378,
                BannerImagePath = "stores\\bobmulet.png",
            },

            //TBD
            new BarberShop(new Vector3(132.5413f, -1712.248f, 29.29169f), 138.2942f,"Herr Kutz Barber","")//Davis
            {
                DisableRegularInteract = true,
                VendorPersonnelID = "HaircutPeds",
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(135.5252f, -1708.4f, 28.79162f), 155.3674f) },
                InteriorID = 102146,
                BannerImagePath = "stores\\herrkutz.png",
            },
            new BarberShop(new Vector3(1206.914f, -470.6263f, 66.15982f), 75.04634f,"Herr Kutz Barber","")//Mirror park
            {
                DisableRegularInteract = true,
                VendorPersonnelID = "HaircutPeds",
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(1211.615f, -471.4896f, 65.70802f), 100.3488f) },
                InteriorID = 112642,
                BannerImagePath = "stores\\herrkutz.png",
            },
            new BarberShop(new Vector3(1934.142f, 3724.473f, 32.80219f), 208.5416f,"O'Sheas Barbers","")//Sandy Shores
            {
                DisableRegularInteract = true,
                VendorPersonnelID = "HaircutPeds",
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(1931.117f, 3729.002f, 32.34442f), 232.3928f) },
                InteriorID = 10242,
                BannerImagePath = "stores\\osheas.png",
            },
            new BarberShop(new Vector3(-30.23092f, -147.3315f, 57.0774f), 338.3362f,"Hair on Hawick","")//Hawick
            {
                DisableRegularInteract = true,
                VendorPersonnelID = "HaircutPeds",
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-31.60344f, -151.6067f, 56.57654f), 15.62799f) },
                InteriorID = 34306,
                BannerImagePath = "stores\\haironhawick.png",
            },
        };
    }
    private void DefaultConfig_PlasticSurgeryClinics()
    {
        PlasticSurgeryClinics = new List<PlasticSurgeryClinic>()
        {

        };
    }
    private void DefaultConfig_TattooShops()
    {
        TattooShopPlaces = new List<TattooShop>()
        {

        };
    }
    private void DefaultConfig_MilitaryBases()
    {
        MilitaryBasePlaces = new List<MilitaryBase>()
        { 
            new MilitaryBase(new Vector3(-1991.979f,3203.726f,32.81024f),180f,"Fort Zancudo","")
            {
                AssignedAssociationID = "ARMY",
                ActivateCells = 14,
                AssaultSpawnLocations = new List<SpawnPlace>()
                {
                    new SpawnPlace(new Vector3(-1787.544f, 3143.728f, 33.06939f), 60.1537f),
                    new SpawnPlace(new Vector3(-1763.003f, 3172.671f, 32.82656f), 65.32996f),
                    new SpawnPlace(new Vector3(-1879.845f, 3240.712f, 32.84465f), 243.7359f),
                    new SpawnPlace(new Vector3(-2122.645f, 3231.769f, 32.81014f), 359.4727f),
                    new SpawnPlace(new Vector3(-2241.937f, 3322.318f, 33.25841f), 150.9899f),
                    new SpawnPlace(new Vector3(-2323.133f, 3259.009f, 33.08135f), 59.05438f),
                    new SpawnPlace(new Vector3(-2409.102f, 3267.377f, 33.14949f), 50.25306f),
                    new SpawnPlace(new Vector3(-2302.279f, 3386.413f, 31.25652f), 143.0586f),
                    new SpawnPlace(new Vector3(-1592.959f, 2798.034f, 17.07146f), 131.2532f),
                    new SpawnPlace(new Vector3(-1732.277f, 2962.13f, 32.80659f), 128.1796f),

                },
                RestrictedAreas = new RestrictedAreas()
                {
                    VanillaRestrictedAreas = new List<VanillaRestrictedArea>()
                    {
                        new VanillaRestrictedArea()
                        {
                            AngledRestrictedAreas = new List<AngledRestrictedArea>()
                            {
                                new AngledRestrictedArea(new Vector3(-1773.944f, 3287.3342f, 30f),new Vector3(-2029.7765f, 2845.0833f, 250f),255f),
                                new AngledRestrictedArea(new Vector3(-2725.8894f, 3291.0986f, 30f),new Vector3(-2009.1815f, 2879.8352f, 250f),180f),
                                new AngledRestrictedArea(new Vector3(-2442.0261f, 3326.6987f, 30f),new Vector3(-2033.9279f, 3089.0488f, 250f),205f),
                                new AngledRestrictedArea(new Vector3(-1917.1654f, 3374.209f, 30f),new Vector3(-2016.7909f, 3195.058f, 250f),86.25f),
                                new AngledRestrictedArea(new Vector3(-2192.753f, 3373.2778f, 30f),new Vector3(-2191.5444f, 3150.4165f, 250f),150.5f),
                                new AngledRestrictedArea(new Vector3(-2077.6633f, 3344.5142f, 30f),new Vector3(-2191.5444f, 3150.4165f, 250f),140.5f),
                                new AngledRestrictedArea(new Vector3(-2861.7554f, 3352.6606f, 30f),new Vector3(-2715.8708f, 3269.9155f, 250f),90f),
                                new AngledRestrictedArea(new Vector3(-2005.5745f, 3364.5327f, 30f),new Vector3(-1977.5688f, 3330.8882f, 250f),100f),
                                new AngledRestrictedArea(new Vector3(-1682.235f, 3004.2852f, 30f),new Vector3(-1942.747f, 2947.4412f,  250f),248.75f),
                                new AngledRestrictedArea(new Vector3(-2393.2954f, 2936.406f, 31.680103f),new Vector3(-2453.0366f, 3006.863f, 52.310028f),128f),
                                new AngledRestrictedArea(new Vector3(-2347.1848f, 3023.8298f, 31.56573f),new Vector3( -2517.3298f, 2989.0635f, 49.956444f),140f),
                                new AngledRestrictedArea(new Vector3(-2259.9219f, 3358.0398f, 29.999718f),new Vector3(-2299.772f, 3385.79f, 38.060143f),16f),
                                new AngledRestrictedArea(new Vector3(-2476.3093f, 3363.914f, 31.679329f),new Vector3(-2431.9807f, 3287.6694f, 39.978264f),214.25f),
                                new AngledRestrictedArea(new Vector3(-2103.0813f, 2797.7834f, 29.37864f),new Vector3(-2096.8213f, 2874.4233f, 57.80989f ),65.75f),
                            },
                        },
                    },
                },

            },
        };
    }
    private void DefaultConfig_TunnelSpawns()
    {
        TunnelSpawnPlaces = new List<StoredSpawn>()
        {
            new StoredSpawn(new Vector3(1376.901f, -955.329f, 57.46247f), 59.50793f) { MinSpawnDistance = 150f,MaxSpawnDistance = 200f, },//murrieta train tunnel  a
            new StoredSpawn(new Vector3(854.5894f, -472.6249f, 29.40497f), 263.9887f){ MinSpawnDistance = 150f,MaxSpawnDistance = 200f, },//murrieta train tunnel  b
            new StoredSpawn(new Vector3(-66.77925f, -539.6862f, 31.24808f), 230.9879f){ MinSpawnDistance = 150f,MaxSpawnDistance = 200f, }, //missiontunnelentrance1downtown
            new StoredSpawn(new Vector3(1.71512f, -622.8944f, 15.21914f), 215.1988f){ MinSpawnDistance = 150f,MaxSpawnDistance = 200f, },  //missiontunnel2 cars ok
            new StoredSpawn(new Vector3(148.4744f, -599.6112f, 17.23636f), 268.7144f){ MinSpawnDistance = 150f,MaxSpawnDistance = 200f, },  //missiontunnel3 subway station cars ok
            new StoredSpawn(new Vector3(519.87f, -1113.665f, 28.66603f), 357.7936f){ MinSpawnDistance = 150f,MaxSpawnDistance = 200f, },  //missionrow police tunnel cars ok
            new StoredSpawn(new Vector3(-294.578f, -311.0045f, 10.06316f), 266.6049f) { IsPedestrianOnlySpawn = true,MinSpawnDistance = 50f,MaxSpawnDistance = 150f, },  //metro spawn 1 NO CARS
            new StoredSpawn(new Vector3(-294.5332f, -336.6915f, 10.0631f), 86.16338f) { IsPedestrianOnlySpawn = true,MinSpawnDistance = 50f,MaxSpawnDistance = 150f, },  //metro platform spawn NO CARS
            new StoredSpawn(new Vector3(-243.9805f, -337.3263f, 29.97777f), 10.97605f) { IsPedestrianOnlySpawn = true,MinSpawnDistance = 50f,MaxSpawnDistance = 150f, },  //subway entrance ground NO CARS
            new StoredSpawn(new Vector3(-795.9164f, -126.2513f, 19.9503f), 211.165f) { IsPedestrianOnlySpawn = true,MinSpawnDistance = 50f,MaxSpawnDistance = 150f, },  //metro platform 1 NO CARS
            new StoredSpawn(new Vector3(-817.4238f, -139.3606f, 19.95036f), 24.94082f) { IsPedestrianOnlySpawn = true,MinSpawnDistance = 50f,MaxSpawnDistance = 150f, },  //metro platfrom 2 NO CARS
            new StoredSpawn(new Vector3(-1363.438f, -444.9677f, 15.04533f), 297.7809f) { IsPedestrianOnlySpawn = true,MinSpawnDistance = 50f,MaxSpawnDistance = 150f, },  //metro platform 3 NO CARS
            new StoredSpawn(new Vector3(-1350.175f, -467.3073f, 15.04538f), 130.3696f) { IsPedestrianOnlySpawn = true,MinSpawnDistance = 50f,MaxSpawnDistance = 150f, }, //metro platform 3 NO CARS
            new StoredSpawn(new Vector3(-524.4281f, -672.548f, 11.80896f), 356.6177f) { IsPedestrianOnlySpawn = true,MinSpawnDistance = 50f,MaxSpawnDistance = 150f, },  //metro platform 4 NO CARS
            new StoredSpawn(new Vector3(-466.1653f, -673.5153f, 11.80903f), 188.0343f) { IsPedestrianOnlySpawn = true,MinSpawnDistance = 50f,MaxSpawnDistance = 150f, }, //metro platform 4 NO CARS
            new StoredSpawn(new Vector3(-195.8262f, -986.5693f, 28.79915f), 339.8409f){ MinSpawnDistance = 150f,MaxSpawnDistance = 200f, },  //metro top CARS OK
            new StoredSpawn(new Vector3(217.6737f, -1813.929f, 24.77368f), 229.9516f){ MinSpawnDistance = 150f,MaxSpawnDistance = 200f, },  //train entrance 1 cARS OK
            new StoredSpawn(new Vector3(598.8229f, -1954.532f, 19.0343f), 97.56085f){ MinSpawnDistance = 150f,MaxSpawnDistance = 200f, },  //treain generic CARS OK
            new StoredSpawn(new Vector3(-520.6234f, 5145.382f, 90.02027f), 163.0666f){ MinSpawnDistance = 150f,MaxSpawnDistance = 200f, },  //railroad tunnel CARS OK
            new StoredSpawn(new Vector3(-429.6222f, 4010.268f, 81.18268f), 8.762694f){ MinSpawnDistance = 150f,MaxSpawnDistance = 200f, }, //railroad tunnel cars ok
        };



    }
    private void DefaultConfig_Airports()
    {
        Airports = new List<Airport>()
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

                ,ScannerFilePath = "01_areas\\0x05E7E888.mp3"
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
                ,BannerImagePath = "stores\\lsia.png"
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
                ,StateID = StaticStrings.NorthYanktonStateID,
            },
            new CayoPericoAirport("CPA",new Vector3(4502.47f, -4530.506f, 4.17187f), 201.2939f,"Cayo Perico Airstrip",
                "Have this crap instead of ~r~REAL~s~ DLC!" +
                "~n~" +
                "~n~City: ~y~Cayo Perico~s~" +
                "~n~Country: ~p~Colombia~s~") 
            { 
                IsEnabled = true,
                StateID = StaticStrings.ColombiaStateID, 
                RequiresMPMap = true,
                OpenTime = 0,
                CloseTime = 24,
                ArrivalPosition = new Vector3(4524.132f, -4498.074f, 4.23596f),
                ArrivalHeading = 298.5098f,
                CameraPosition = new Vector3(4538.386f, -4544.42f, 25.30816f), 
                CameraDirection = new Vector3(-0.8255408f, 0.4359591f, -0.3583601f), 
                CameraRotation = new Rotator(-20.99952f, -1.005964E-05f, 62.16195f),
                AirArrivalPosition = new Vector3(3643.128f, -4779.568f, 63.15142f),
                AirArrivalHeading =  282.1072f,
                CommercialFlights = new List<AirportFlight>()
                {
                    new AirportFlight("LSIX",StaticStrings.AdiosAirlinesCarrierID,"You'll get there when you get there", 750, 10),
                }
            },



            new Airport("SFX",new Vector3(),0f,"San Fierro Intl.",
                "The city of psychedelic wonders." +
                "~n~" +
                "~n~City: ~y~San Fierro~s~" +
                "~n~State: ~p~San Andreas~s~"){ IsEnabled = false,StateID = StaticStrings.SanAndreasStateID },
            new Airport("FIA",new Vector3(),0f,"Francis Intl.",
                "Great To Visit, Even Better To Leave" +
                "~n~" +
                "~n~City: ~y~Dukes, Liberty City~s~" +
                "~n~State: ~p~Liberty~s~") { IsEnabled = false,StateID = StaticStrings.LibertyStateID },
            new Airport("VCIA",new Vector3(),0f,"Escobar Intl.",
                "A city run by dealers and weasels" +
                "~n~" +
                "~n~City: ~y~Vice City~s~" +
                "~n~State: ~p~Leonida~s~") { IsEnabled = false,StateID = StaticStrings.LeonidaStateID },

            };
    }
    private void DefaultConfig_Morgues()
    {
        Morgues = new List<Morgue>()
        {
            new Morgue(new Vector3(241.2085f, -1378.962f, 33.74176f), 140.41f,"Los Santos County Coroner Office", "","") {OpenTime = 0,CloseTime = 24, InteriorID = 60418,ScannerFilePath = "01_specific_location\\0x04F66C50.mp3" },
        };
    }
    private void DefaultConfig_BusStops()
    {
        BusStops = new List<BusStop>()
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
    }
    private void DefaultConfig_ClothingShops()
    {
        ClothingShops = new List<ClothingShop>()
        {
                new ClothingShop(new Vector3(430.0404f, -804.3267f, 29.49115f), 359.4608f, "Binco Textile City","Low-quality clothing at low prices.","LiquorStoreMenu",new Vector3(430.0404f, -804.3267f, 29.49115f))
                {
                    IsEnabled = false,

                    VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(427.1392f, -806.624f, 29.49114f), 78.23051f) },

                    //VendorModels = new List<string>() { "s_f_y_shop_low" },
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
                new ClothingShop(new Vector3(-837.588f, -161.6364f, 37.90956f), 0f, "Didier Sachs","Fashion never cost so much","",Vector3.Zero) { OpenTime = 8, CloseTime = 20, IsTemporarilyClosed = true, ScannerFilePath = "01_specific_location\\0x0092CBCB.mp3"},//rockford hills
                new ClothingShop(new Vector3(-717.36f, -157.29f, 38.2f), 117.6851f, "Ponsonbys","Catering to the Elite","",Vector3.Zero) { OpenTime = 8, CloseTime = 20, IsTemporarilyClosed = true, ScannerFilePath = "01_specific_location\\0x0289F802.mp3"},//rockford hills
        
                new ClothingShop(new Vector3(-622.108f, -230.744f, 38.05705f), 359.4608f, "Vangelico", "Retailer of high-end jewelry and blood diamonds", "", Vector3.Zero) {
                    Name = "Vangelico",
                    FullName = "Vangelico",
                    Description = "Retailer of high-end jewelry and blood diamonds",
                    EntrancePosition = new Vector3( - 622.108f, -230.744f, 38.05705f),
                    EntranceHeading = 359.4608f,
                    OpenTime = 4,
                    CloseTime = 22,
                    InteriorID = 82690,
                    IsOnSPMap = false,
	                //MenuID = "VangelicoMenu",
	                PossiblePedSpawns = new List < ConditionalLocation > () {
                        new SecurityConditionalLocation() {
                            Location = new Vector3( - 623.1132f, -225.5436f, 38.05704f),
                            Heading = 166.1438f,
                            Percentage = 95f,
                            AssociationID = "SECURO",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<string>() {"WORLD_HUMAN_GUARD_STAND",},
                            MinHourSpawn = 6,
                            MaxHourSpawn = 20,
                            ForceSidearm = true,
                            ForceLongGun = true,
                        },
                        new SecurityConditionalLocation() {
                            Location = new Vector3( - 617.5581f, -233.1718f, 38.05704f),
                            Heading = 94.85792f,
                            Percentage = 95f,
                            AssociationID = "SECURO",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<string>() {"WORLD_HUMAN_GUARD_STAND",
                            },
                            MinHourSpawn = 6,
                            MaxHourSpawn = 20,
                            LongGunAlwaysEquipped = true,
                            ForceSidearm = true,
                            ForceLongGun = true,
                        },
                    },
                    VendorLocations = new List<SpawnPlace>() 
                    {
                        new SpawnPlace(new Vector3(-622.4784f, -229.6247f, 38.05705f),305.908f)
                    },
                    VendorPersonnelID = "TellerPeds",
                },

        };
    }
    private void DefaultConfig_DriveThrus()
    {
        DriveThrus = new List<DriveThru>()
        {
            new DriveThru(new Vector3(95.41846f, 285.0295f, 110.2042f), 251.8247f, "Up-N-Atom Burger", "Never Frozen, Often Microwaved","UpNAtomMenu") {OpenTime = 0, CloseTime = 24, BannerImagePath = "stores\\upnatom.png"},
            new DriveThru(new Vector3(15.48935f, -1595.832f, 29.28254f), 319.2816f, "The Taco Farmer", "Open All Hours!","TacoFarmerMenu") { OpenTime = 0, CloseTime = 24, BannerImagePath = "stores\\tacofarmer.png" },
            new DriveThru(new Vector3(-576.9321f, -880.5195f, 25.70123f), 86.01214f, "Lucky Plucker", "Come be a real Lucky Plucker","LuckyPluckerMenu") { OpenTime = 5, CloseTime = 23,BannerImagePath = "stores\\luckyplucker.png",ScannerFilePath = "01_specific_location\\0x14B8A4DB.mp3" } ,
            new DriveThru(new Vector3(2591.213f, 478.8892f, 108.6423f), 270.9569f, "Bishop's Chicken", "Our chicken is a religious experience","BishopsChickenMenu") { OpenTime = 5, CloseTime = 23,BannerImagePath = "stores\\bishops.png" },
            new DriveThru(new Vector3(144.34f, -1541.275f, 28.36799f), 139.819f, "La Vaca Loca", "Whats wrong with a few mad cows?","LaVacaLocaMenu"){ OpenTime = 5, CloseTime = 23,BannerImagePath = "stores\\vacaloca.png" },
            new DriveThru(new Vector3(145.3499f, -1460.568f, 28.71129f), 49.75111f,  "Lucky Plucker", "Come be a real Lucky Plucker","LuckyPluckerMenu") { OpenTime = 5, CloseTime = 23,BannerImagePath = "stores\\luckyplucker.png" },
            new DriveThru(new Vector3(1256.509f, -357.1387f, 68.52029f), 347.8622f, "Horny's Burgers", "The beef with the bone!","HornysBurgersMenu"){ OpenTime = 5, CloseTime = 23, BannerImagePath = "stores\\hornys.png" },

        };
    }
    private void DefaultConfig_Dealerships()
    {
        Dealerships = new List<Dealership>()
        {
            new Dealership(new Vector3(-69.16984f, 63.42498f, 71.89044f), 150.3918f, "Benefactor/Gallivanter", "Take control","BenefactorGallavanterMenu") { 
                BannerImagePath = "stores\\benefactorgallivanter.png",
                LicensePlatePreviewText = "BENE GALA",
                AssignedAssociationID = "BOBCAT",
                PossiblePedSpawns = new List<ConditionalLocation>() 
                { 
                    new SecurityConditionalLocation(new Vector3(-72.16306f, 64.89938f, 71.84079f), 150.1162f, 100f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.StandardScenario, RequiredPedGroup = "UnarmedSecurity" } 
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>() 
                { 
                    new SecurityConditionalLocation(new Vector3(-97.9782f, 84.38512f, 71.20057f), 149.9098f, 75f), 
                },
                VehiclePreviewLocation = new SpawnPlace(new Vector3(226.205f, -992.613f, -98.99996f), 177.2006f),
                VehicleDeliveryLocations = new List<SpawnPlace>() {
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
            new Dealership(new Vector3(-176.7741f, -1158.648f, 23.81366f), 359.6327f, "Vapid of Los Santos", "Low quality mass produced vehicles","VapidMenu") 
            { 
                BannerImagePath = "stores\\vapid.png",
                LicensePlatePreviewText = "VAPID LS",
                VehiclePreviewLocation = new SpawnPlace(new Vector3(226.205f, -992.613f, -98.99996f), 177.2006f),
                VehicleDeliveryLocations = new List<SpawnPlace>() 
                {
                    new SpawnPlace(new Vector3(-223.3041f, -1166.967f, 22.99067f), 347.7626f),
                    new SpawnPlace(new Vector3(-237.1176f, -1176.55f, 22.12655f), 322.7791f),
                    new SpawnPlace(new Vector3(-237.2399f, -1165.014f, 22.14342f), 272.7086f),
                    new SpawnPlace(new Vector3(-224.3428f, -1177.964f, 22.16829f), 13.04972f),
                } 
            },
            new Dealership(new Vector3(286.8117f, -1148.615f, 29.29189f), 0.5211872f,"Sanders Motorcycles", "Feeling Old? Buy a Bike!","SandersMenu") 
            { 
                BannerImagePath = "stores\\sanders.png", 
                ScannerFilePath = "01_specific_location\\0x16677E71.mp3",
                LicensePlatePreviewText = "MOTO4YOU",
                VehiclePreviewLocation = new SpawnPlace(new Vector3(226.205f, -992.613f, -98.99996f), 177.2006f),
                VehicleDeliveryLocations = new List<SpawnPlace>() 
                {
                    new SpawnPlace(new Vector3(259.3008f, -1150.141f, 29.29169f), 358.0416f),
                    new SpawnPlace(new Vector3(262.3277f, -1161.748f, 28.36098f), 177.7536f),
                    new SpawnPlace(new Vector3(262.2397f, -1149.571f, 28.46074f), 180.2331f),
                    new SpawnPlace(new Vector3(256.4698f, -1161.971f, 28.34791f), 175.3772f),
                    new SpawnPlace(new Vector3(256.4955f, -1150.385f, 28.4598f), 181.1595f),
                    new SpawnPlace(new Vector3(250.4825f, -1161.918f, 28.31437f), 175.5457f),
                    new SpawnPlace(new Vector3(250.2824f, -1150.506f, 28.44035f), 177.726f),
                    new SpawnPlace(new Vector3(247.0185f, -1162.408f, 28.32439f), 175.2579f),
                } 
            },
            new Dealership(new Vector3(-247.2263f, 6213.266f, 31.93902f), 143.0866f, "Helmut's European Autos", "Only the best eurotrash","HelmutMenu") 
            { 
                BannerImagePath = "stores\\helmut.png",
                VehiclePreviewLocation = new SpawnPlace(new Vector3(226.205f, -992.613f, -98.99996f), 177.2006f),
                LicensePlatePreviewText = "HELMUTEU", 
                IsOnMPMap = false,
                VehicleDeliveryLocations = new List<SpawnPlace>() 
                {
                    new SpawnPlace(new Vector3(-214.548f, 6195.725f, 31.48873f), 314.937f),
                    new SpawnPlace(new Vector3(-229.8454f, 6197.135f, 30.65802f), 312.3795f),
                    new SpawnPlace(new Vector3(-240.5559f, 6199.209f, 30.65911f), 320.0063f),
                    new SpawnPlace(new Vector3(-245.7337f, 6203.578f, 30.65917f), 312.8283f),
                    new SpawnPlace(new Vector3(-247.8804f, 6205.941f, 30.65889f), 317.6494f),
                    new SpawnPlace(new Vector3(-204.6075f, 6204.576f, 30.65833f), 312.9748f),
                } 
            },
            new Dealership(new Vector3(-38.83289f, -1108.61f, 26.46652f), 158.283f, "Premium Deluxe Motorsport", "Let's make a deal","PremiumDeluxeMenu") 
            { 
                BannerImagePath = "stores\\pdx.png", 
                ScannerFilePath = "01_specific_location\\0x122B5EFF.mp3",
                CameraPosition = new Vector3(-73.69526f, -1125.221f, 33.51564f), 
                CameraDirection = new Vector3(0.834101f, 0.5192783f, -0.1860794f), 
                CameraRotation = new Rotator(-10.72407f, -1.7379E-06f, -58.09524f),
                VehiclePreviewCameraPosition = new Vector3(-46.13059f, -1103.091f, 27.9145f),
                VehiclePreviewCameraDirection = new Vector3(0.3461686f, 0.9154226f, -0.2053503f),
                VehiclePreviewCameraRotation = new Rotator(-11.85001f, -8.374705E-05f, -20.7142f),
                VehiclePreviewLocation = new SpawnPlace(new Vector3(-43.94203f, -1096.923f, 26.44f), 165.1469f),     
                InteriorID = 7170,
                LicensePlatePreviewText = "PDXMOTORS",
                VehicleDeliveryLocations = new List<SpawnPlace>() 
                {
                    new SpawnPlace(new Vector3(-56.35966f, -1116.532f, 26.4349f), 2.403779f),
                    new SpawnPlace(new Vector3(-57.66466f, -1107.103f, 25.60543f), 65.5113f),
                    new SpawnPlace(new Vector3(-47.87741f, -1115.788f, 25.60351f), 3.64787f),
                    new SpawnPlace(new Vector3(-50.5764f, -1116.585f, 25.60393f), 0.4485914f),
                    new SpawnPlace(new Vector3(-53.53827f, -1115.804f, 25.60398f), 1.730222f),
                    new SpawnPlace(new Vector3(-58.79285f, -1116.476f, 25.60338f), 2.648925f),
                    new SpawnPlace(new Vector3(-48.28887f, -1078.742f, 25.9386f), 260.1963f),
                    new SpawnPlace(new Vector3(-15.29717f, -1094.516f, 25.84302f), 173.1287f),
                } 
            },
            new Dealership(new Vector3(-802.8875f, -223.7307f, 37.21824f), 117.6851f, "Luxury Autos", "You sure you can afford this?","LuxuryAutosMenu") 
            {
                BannerImagePath = "stores\\luxuryautos.png",
                VehiclePreviewLocation = new SpawnPlace(new Vector3(226.205f, -992.613f, -98.99996f), 177.2006f),
                LicensePlatePreviewText = "LUX AUTO",
                AssignedAssociationID = "MERRY",
                PossiblePedSpawns = new List<ConditionalLocation>() 
                {
                    new SecurityConditionalLocation(new Vector3(-802.0759f, -227.486f, 37.19593f), 120.1107f, 75f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.StandardScenario, RequiredPedGroup = "UnarmedSecurity" },
                    new SecurityConditionalLocation(new Vector3(-805.9191f, -220.9726f, 37.25732f), 121.415f, 75f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.StandardScenario, RequiredPedGroup = "UnarmedSecurity" },
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                { 
                    new SecurityConditionalLocation(new Vector3(-811.8248f, -220.6809f, 36.87595f), 27.60655f, 65f) { },
                    new SecurityConditionalLocation(new Vector3(-804.566f, -235.1782f, 36.70311f), 29.39835f, 35f) {IsEmpty = false, RequiredPedGroup = "UnarmedSecurity" }, },
                VehicleDeliveryLocations = new List<SpawnPlace>() 
                {
                    new SpawnPlace(new Vector3(-768.5347f, -245.1849f, 37.2452f), 197.8712f),
                    new SpawnPlace(new Vector3(-811.3596f, -221.3954f, 36.38088f), 30.76537f),
                    new SpawnPlace(new Vector3(-803.5298f, -234.8655f, 36.26606f), 29.17092f),
                    new SpawnPlace(new Vector3(-755.7492f, -242.3515f, 36.36629f), 281.1064f),
                    new SpawnPlace(new Vector3(-760.1515f, -232.0841f, 36.45313f), 204.9191f),
                } 
            },
            new Dealership(new Vector3(1224.667f, 2728.353f, 38.00491f), 181.2344f, "Larry's RV Sales", "Need to disappear for a while?", "LarrysRVMenu")
            {
                BannerImagePath = "stores\\larrysrv.png",
                VehiclePreviewLocation = new SpawnPlace(new Vector3(226.205f, -992.613f, -98.99996f), 177.2006f),
                LicensePlatePreviewText = "LARRYSRV",
                VehicleDeliveryLocations = new List<SpawnPlace>() 
                {
                    new SpawnPlace(new Vector3(1236.887f, 2709.858f, 38.00579f), 201.5402f),
                    new SpawnPlace(new Vector3(1246.11f, 2712.359f, 37.17435f), 200.71f),
                    new SpawnPlace(new Vector3(1232.931f, 2699.977f, 37.18208f), 94.271f),
                    new SpawnPlace(new Vector3(1210.044f, 2706.118f, 37.17501f), 173.5784f),
                    new SpawnPlace(new Vector3(1254.536f, 2692.278f, 36.79386f), 224.2466f),
                } 
            },
            new Dealership(new Vector3(-703.8819f, -1398.048f, 5.495286f), 137.6665f, "Elitás Travel", "There's first class and then there's Elitas","ElitasMenu") 
            {
                CameraPosition = new Vector3(-722.9339f, -1401.834f, 11.11265f), 
                CameraDirection = new Vector3(0.956533f, 0.1323426f, -0.2598656f), 
                CameraRotation = new Rotator(-15.06209f, -2.453513E-05f, -82.12276f),
                BannerImagePath = "stores\\elitastravel.png",
                VehiclePreviewCameraPosition = new Vector3(-702.0489f, -1431.725f, 9.647567f),
                VehiclePreviewCameraDirection = new Vector3(-0.9063408f, -0.336776f, -0.2552025f),
                VehiclePreviewCameraRotation = new Rotator(-14.78558f, -0.03792449f, 110.3839f),
                VehiclePreviewLocation = new SpawnPlace(new Vector3(-714.89f, -1435.682f, 5.102043f), 227.1837f),
                VehicleDeliveryLocations = new List<SpawnPlace>() 
                {
                    //right outside the place
                    new SpawnPlace(new Vector3(-728.7345f, -1415.244f, 6.000524f), 140.5158f),
                    new SpawnPlace(new Vector3(-691.3646f, -1440.159f, 6.000525f), 52.68159f),
                    new SpawnPlace(new Vector3(-739.0837f, -1489.017f, 6.000519f), 40.92105f),

                    //LSIA, doesnt seem to load col or something
                    //new SpawnPlace(new Vector3(-1015.187f, -3401.692f, 13.8351f), 327.5063f),
                    //new SpawnPlace(new Vector3(-1066.172f, -3397.097f, 14.54819f), 330.6479f),
                    //new SpawnPlace(new Vector3(-1091.473f, -3384.792f, 14.54814f), 330.6455f),
                } 
            },
            new Dealership(new Vector3(396.8023f, -1170.86f, 29.39787f), 359.2079f, "Get Aweigh", "We can help you Get Aweigh!","GetAweighMenu") 
            {
                VehiclePreviewCameraPosition = new Vector3(-759.6039f, -1370.657f, 3.687305f),
                BannerImagePath = "stores\\getaweigh.png",
                VehiclePreviewCameraDirection = new Vector3(0.4598052f, 0.8650874f, -0.2005067f),
                VehiclePreviewCameraRotation = new Rotator(-11.56659f, 2.701561E-05f, -27.99123f),
                VehiclePreviewLocation = new SpawnPlace(new Vector3(-755.6831f, -1361.648f, -0.0738305f),229.675f),
                VehicleDeliveryLocations = new List<SpawnPlace>() 
                {
                    new SpawnPlace(new Vector3(-884.1031f, -1460.032f, 0.1196398f), 198.8394f),
                    new SpawnPlace(new Vector3(-892.0487f, -1464.138f, 0.1223292f), 200.6151f),
                    new SpawnPlace(new Vector3(-901.3271f, -1465.705f, 0.1061458f), 200.2641f),
                    new SpawnPlace(new Vector3(-832.6543f, -1406.055f, 0.09211653f), 293.3259f),
                    new SpawnPlace(new Vector3(-833.0579f, -1397.259f, 0.1287543f), 290.7657f),
                } 
            },
            //From discord
            new Dealership(new Vector3(1130.419f, -776.9326f, 57.60993f), 357.4348f, "Outlaw Motors", "Ride like an outlaw","OutlawMotorMenu") 
            {
                OpenTime = 6, 
                CloseTime = 20,
                BannerImagePath = "stores\\outlaw.png",
                VehiclePreviewLocation = new SpawnPlace(new Vector3(226.205f, -992.613f, -98.99996f), 177.2006f),
                LicensePlatePreviewText = "OUTLAWMC",
                VehicleDeliveryLocations = new List<SpawnPlace>() 
                {
                    new SpawnPlace(new Vector3(1122.58f, 784.47f, 57.68f), 91.32f),
                    new SpawnPlace(new Vector3(1121.96f, -773.24f, 57.75f), 359.68f),
                } },
            new Dealership(new Vector3(-41.04f, -1675.14f, 29.45f), 139.44f, "Albany", "From Liberty City with Love","AlbanyMenu") 
            {
                OpenTime = 6, CloseTime = 20,BannerImagePath = "stores\\albany.png",
                VehiclePreviewLocation = new SpawnPlace(new Vector3(226.205f, -992.613f, -98.99996f), 177.2006f),
                LicensePlatePreviewText = "ALBANY",
                VehicleDeliveryLocations = new List<SpawnPlace>() 
                {
                    new SpawnPlace(new Vector3(-45.12f, -1690.65f, 29.39f), 22.31f),
                    new SpawnPlace(new Vector3(-40.71f, -1687.56f, 29.39f), 20.33f),
                } 
            },
        };
    }
    private void DefaultConfig_VehicleExporters()
    {
        VehicleExporters = new List<VehicleExporter>()
        {
            new VehicleExporter(new Vector3(89.00745f,-2564.649f,6.004591f), 1.725455f, "Pacfic Bait Exports", "Take our Bait!","SunshineMenu")
            {
                ContactName = StaticStrings.VehicleExporterContactName,
                ParkingSpaces = new List<SpawnPlace>()
                 {
                    new SpawnPlace(new Vector3(70.56293f, -2559.652f, 6.000013f), 177.7919f),
                    new SpawnPlace(new Vector3(73.9108f, -2559.785f, 6.000012f), 178.2656f),
                    new SpawnPlace(new Vector3(77.10069f, -2560.043f, 6.000011f), 177.6563f),
                    new SpawnPlace(new Vector3(80.26527f, -2560.101f, 6.000011f), 179.8577f),
                    new SpawnPlace(new Vector3(83.70882f, -2560.141f, 6.000011f), 180.1441f),
                 },
                OpenTime = 0,
                CloseTime = 24,
                BannerImagePath = "stores\\pacificbait.png",
            },
            new VehicleExporter(new Vector3(845.3537f, -951.5338f, 26.52109f), 270.8987f, "National Transport", "When it needs to disappear!","NationalMenu")
            {
                 ContactName = StaticStrings.VehicleExporterContactName,
                 ParkingSpaces = new List<SpawnPlace>()
                 {
                    new SpawnPlace(new Vector3(853.4166f, -905.9677f, 25.3178f), 88.12348f),
                    new SpawnPlace(new Vector3(853.4921f, -902.6026f, 25.32147f), 90.01858f),
                    new SpawnPlace(new Vector3(853.5576f, -899.2606f, 25.32503f), 89.40579f),
                    new SpawnPlace(new Vector3(853.313f, -895.832f, 25.31771f), 84.42482f),
                    new SpawnPlace(new Vector3(853.5083f, -892.3186f, 25.32238f), 88.35712f),
                 },
                OpenTime = 0,
                CloseTime = 24,
                BannerImagePath = "stores\\nationaltransports.png",
            },
            new VehicleExporter(new Vector3(44.4949f, 6461.12f, 31.4253f), 226.2354f,"Paleto Exports","Turn Cars to CASH!","PaletoExportMenu")
            {
                 ContactName = StaticStrings.VehicleExporterContactName,
                 ParkingSpaces = new List<SpawnPlace>()
                 {
                    new SpawnPlace(new Vector3(29.69258f, 6438.792f, 31.42538f), 91.18743f),
                    new SpawnPlace(new Vector3(61.85519f, 6470.358f, 31.42395f), 349.3379f),
                    new SpawnPlace(new Vector3(68.2027f, 6471.051f, 31.2645f), 356.7883f),
                    new SpawnPlace(new Vector3(56.72287f, 6464.54f, 31.41228f), 137.1895f),
                    new SpawnPlace(new Vector3(39.3842f, 6447.123f, 31.40866f), 135.2603f),
                 },
                OpenTime = 0,
                CloseTime = 24,

            },
        };
    }
    private void DefaultConfig_Forgers()
    {
        Forgers = new List<Forger>()
        {
            new Forger(new Vector3(1480.319f, 3678.905f, 34.28013f), 198.993f, "Bell's Forgeries", "Is anything really original these days?")
            {
                CleanPlateSalesPrice = 200,
                WantedPlateSalesPrice = 50,
                CustomPlateCost = 550,
                RandomPlateCost = 300,
                OpenTime = 0,
                CloseTime = 24,
            },
            new Forger(new Vector3(-41.79562f, 6637.322f, 31.08751f), 216.5384f,"Zancone The Forger","Not a murmur to anyone!")
            {
                CleanPlateSalesPrice = 300,
                WantedPlateSalesPrice = 150,
                CustomPlateCost = 650,
                RandomPlateCost = 350,
                OpenTime = 0,
                CloseTime = 24,
            },
        };

        
    }
    public GamblingParameters GetParameters(int Scalar)
    {
        GamblingParameters defaultParameters = new GamblingParameters()
        {
            BlackJackGameRulesList = new List<BlackJackGameRules>()
                    {
                        new BlackJackGameRules("Low Stakes Blackjack","The House",25,500 * Scalar,true,true, false, false),
                        new BlackJackGameRules("Associate Blackjack","The House",100,1500 * Scalar,false,true, true, false),
                        new BlackJackGameRules("Members Blackjack","The House",500,5000 * Scalar,false,true, false, true),
                    },
            RouletteGameRulesList = new List<RouletteGameRules>()
                    {
                        new RouletteGameRules("Low Stakes Roulette","The House",50,500 * Scalar, false, false),
                        new RouletteGameRules("Associate Roulette","The House",250,2500* Scalar,true,false),
                        new RouletteGameRules("Members Roulette","The House",500,5000 * Scalar,false, true),
                    },
        };
        return defaultParameters;
    }
    private void DefaultConfig_GamblingDens()
    {
        GamblingParameters defaultParameters = GetParameters(1);
        //Scatino Casino, Davey's Dive,
        GamblingDens = new List<GamblingDen>()
        {
            new GamblingDen(new Vector3(929.9568f, 41.6748f, 81.09632f), 58.06394f,"Be Lucky Los Santos!","Finally Open!")//Regular Casino
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
                BannerImagePath = "stores\\beluckyls.png",
                CameraPosition = new Vector3(851.0151f, 50.54535f, 97.35239f), 
                CameraDirection = new Vector3(0.9805309f, -0.1052597f, -0.1657695f), 
                CameraRotation = new Rotator(-9.541942f, 7.250671E-06f, -96.12722f),
                IsOnMPMap = false,
                IsOnSPMap = true,
            },

            new GamblingDen(new Vector3(935.4767f, 47.33501f, 81.09575f), 137.8167f,"The Diamond Casino & Resort","Welcome to the Diamond")//Regular Casino
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
                BannerImagePath = "stores\\diamondcasino.png",
                CameraPosition = new Vector3(820.244f, 26.93432f, 106.9623f), 
                CameraDirection = new Vector3(0.9816462f, 0.02359661f, -0.1892456f), 
                CameraRotation = new Rotator(-10.90876f, 2.445427E-07f, -88.623f),
                IsOnMPMap = true,
                IsOnSPMap = false,
            },


            new GamblingDen(new Vector3(-1383.725f, 268.0395f, 61.23847f), 194.1238f,"The Scatino Casino","It was just a stutter step!")//Messina Casino, fancy
            {
                GamblingParameters = GetParameters(3),
                WinLimit = 45000,
                WinLimitResetHours = 24,
                OpenTime = 18,
                CloseTime = 4,
                AssignedAssociationID = "AMBIENT_GANG_MESSINA",
                CameraPosition = new Vector3(-1384.766f, 206.0768f, 86.17978f),
                CameraDirection = new Vector3(0.1247992f, 0.951241f, -0.2820741f),
                CameraRotation = new Rotator(-16.38403f, 2.247024E-05f, -7.474297f),
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(-1377.991f, 255.1484f, 60.3684f), 138.936f, 40f) { AssociationID = "AMBIENT_GANG_MESSINA"},
                    new GangConditionalLocation(new Vector3(-1377.991f, 255.1484f, 60.3684f), 138.936f, 40f) { AssociationID = "AMBIENT_GANG_MESSINA"},
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(-1351.666f, 246.2108f, 60.47062f), 186.2948f, 40f) { IsEmpty = true, AssociationID = "AMBIENT_GANG_MESSINA"},
                },
            },
            new GamblingDen(new Vector3(-358.2999f, 91.13467f, 70.5202f), 267.5716f,"The Hush Casino","We'll take care of you Win or Lose!")// Gambetti Casino, decnet condo
            {
                GamblingParameters = GetParameters(2),
                WinLimit = 35000,
                WinLimitResetHours = 24,
                OpenTime = 18,
                CloseTime = 4,
                AssignedAssociationID = "AMBIENT_GANG_GAMBETTI",
                CameraPosition = new Vector3(-338.0188f, 40.86187f, 81.43283f), 
                CameraDirection = new Vector3(-0.3936391f, 0.8768913f, -0.2758803f),
                CameraRotation = new Rotator(-16.01448f, 1.154718E-05f, 24.17545f),
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(-357.4808f, 88.34515f, 70.52021f), 326.8036f, 40f) { AssociationID = "AMBIENT_GANG_GAMBETTI"},
                    new GangConditionalLocation(new Vector3(-353.4513f, 109.2422f, 66.49493f), 322.5599f, 40f) { AssociationID = "AMBIENT_GANG_GAMBETTI"},
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(-345.571f, 107.8305f, 66.68494f), 182.9895f, 40f) { IsEmpty = true, AssociationID = "AMBIENT_GANG_GAMBETTI"},
                },
            },
            new GamblingDen(new Vector3(-84.90887f, 6362.521f, 35.50074f), 227.6975f,"Room 22 Casino","Not just for close encounters")//Lupisella Casino, low end hotel
            {
                GamblingParameters = defaultParameters,
                WinLimit = 25000,
                WinLimitResetHours = 24,
                OpenTime = 18,
                CloseTime = 4,
                AssignedAssociationID = "AMBIENT_GANG_LUPISELLA",
                CameraPosition = new Vector3(-57.9878f, 6354.977f, 41.67742f), 
                CameraDirection = new Vector3(-0.9333099f, 0.2792642f, -0.2257083f), 
                CameraRotation = new Rotator(-13.04453f, -2.190972E-06f, 73.34181f),
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(-81.87692f, 6364.329f, 31.49035f), 253.9082f, 40f) { AssociationID = "AMBIENT_GANG_LUPISELLA"},
                    new GangConditionalLocation(new Vector3(-86.91302f, 6360.526f, 35.50075f), 239.8991f, 40f) { AssociationID = "AMBIENT_GANG_LUPISELLA"},
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(-81.51994f, 6357.41f, 31.49035f), 40.8159f, 40f) { IsEmpty = true, AssociationID = "AMBIENT_GANG_LUPISELLA"},
                },
            },
            new GamblingDen(new Vector3(-3149.372f, 1043.472f, 20.69423f), 246.6364f,"The Hardware Casino","Lots of hammers nearby, remember that.")//Ancelotti Casino
            {
                GamblingParameters = defaultParameters,
                WinLimit = 20000,
                WinLimitResetHours = 24,
                OpenTime = 18,
                CloseTime = 4,
                AssignedAssociationID = "AMBIENT_GANG_ANCELOTTI",
                CameraPosition = new Vector3(-3124.243f, 1061.832f, 26.74071f), 
                CameraDirection = new Vector3(-0.9221458f, -0.3260281f, -0.2082135f), 
                CameraRotation = new Rotator(-12.01768f, 6.983239E-06f, 109.4712f),
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(-3150.791f, 1040.369f, 20.71614f), 211.2361f, 40f) { AssociationID = "AMBIENT_GANG_ANCELOTTI"},
                    new GangConditionalLocation(new Vector3(-3144.495f, 1051.607f, 20.67891f), 354.6758f, 40f) { AssociationID = "AMBIENT_GANG_ANCELOTTI"},
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(-3141.35f, 1074.772f, 20.61019f), 82.58844f, 40f) { IsEmpty = true, AssociationID = "AMBIENT_GANG_ANCELOTTI"},
                },
            },
            new GamblingDen(new Vector3(1366.062f, 4358.021f, 44.50027f), 356.1247f,"The Cove Casino","Out of the way, out of prying eyes!")//Pavano Casino
            {
                GamblingParameters = defaultParameters,
                WinLimit = 15000,
                WinLimitResetHours = 24,
                OpenTime = 18,
                CloseTime = 4,
                AssignedAssociationID = "AMBIENT_GANG_PAVANO",
                CameraPosition = new Vector3(1358.505f, 4371.237f, 48.73557f),
                CameraDirection = new Vector3(0.3603207f, -0.8906768f, -0.2772433f),
                CameraRotation = new Rotator(-16.09575f, -5.775947E-06f, -157.9743f),
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(1363.469f, 4360.977f, 44.4992f), 327.1165f, 40f) { AssociationID = "AMBIENT_GANG_PAVANO"},
                    new GangConditionalLocation(new Vector3(1372.259f, 4358.026f, 44.49716f), 327.7067f, 40f) { AssociationID = "AMBIENT_GANG_PAVANO"},
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(1369.516f, 4365.044f, 44.32616f), 78.89337f, 40f) { IsEmpty = true, AssociationID = "AMBIENT_GANG_PAVANO"},
                },
            },
        };
    }
    private void DefaultConfig_PayNSprays()
    {
        RepairGarages = new List<RepairGarage>()
        {
            new RepairGarage(new Vector3(-1358.503f, -756.4457f, 22.30451f), 301.2179f, "Pay 'n' Spray", "The Vespucci Treat!") { 
                GarageDoors = new List<InteriorDoor>() { new InteriorDoor(3082692265,new Vector3(-1355.819f,-754.4543f,23.49588f)) },
                CameraPosition = new Vector3(-1344.593f, -752.1688f, 24.59984f), 
                CameraDirection = new Vector3(-0.953465f, -0.2256246f, -0.1999952f), 
                CameraRotation = new Rotator(-11.53668f, 1.61205E-05f, 103.3134f),
                OpenTime = 0,
                CloseTime = 24,
                CanInteractWhenWanted = true,
                BannerImagePath = "stores\\paynspray.png",
            },
            new RepairGarage(new Vector3(517.7049f, 169.9352f, 99.36998f), 249.3513f, "Pay 'n' Spray", "Helpfully located in Downtown Vinewood") {
                GarageDoors = new List<InteriorDoor>() { new InteriorDoor(1991494706,new Vector3(523.8579f,167.7482f,100.5352f)) { NeedsDefaultUnlock = true} },
                CameraPosition = new Vector3(536.9068f, 157.7742f, 103.5797f), 
                CameraDirection = new Vector3(-0.7981344f, 0.5672539f, -0.2029887f), 
                CameraRotation = new Rotator(-11.71179f, -1.30789E-06f, 54.59765f),
                OpenTime = 0,
                CloseTime = 24,
                CanInteractWhenWanted = true,
                BannerImagePath = "stores\\paynspray.png",
            },
            new RepairGarage(new Vector3(964.051f, -1856.824f, 31.19692f), 82.4654f, "Pay 'n' Spray", "Be sure to lock your doors!") {
                GarageDoors = new List<InteriorDoor>() { new InteriorDoor(4104186511,new Vector3(958.9808f,-1855.851f,32.78582f)) },
                CameraPosition = new Vector3(945.6613f, -1849.031f, 36.08583f), 
                CameraDirection = new Vector3(0.8777003f, -0.4318189f, -0.2077849f), 
                CameraRotation = new Rotator(-11.99257f, -1.309235E-05f, -116.1967f),
                OpenTime = 0,
                CloseTime = 24,
                CanInteractWhenWanted = true,
                BannerImagePath = "stores\\paynspray.png",
            },
            new RepairGarage(new Vector3(-440.179f, -2180.787f, 10.32189f), 0.8532715f, "Pay 'n' Spray", "Need to get away?") {
                GarageDoors = new List<InteriorDoor>() {  
                    new InteriorDoor(4104186511,new Vector3(-440.0605f,-2171.847f,12.2417f)),
                    new InteriorDoor(4104186511,new Vector3(-445.3054f,-2171.818f,11.40392f)) },
                CameraPosition = new Vector3(-433.9502f, -2157.46f, 13.05093f), 
                CameraDirection = new Vector3(-0.365413f, -0.9212855f, -0.1330653f),
                CameraRotation = new Rotator(-7.646759f, 1.292151E-06f, 158.3651f),
                OpenTime = 0,
                CloseTime = 24,
                CanInteractWhenWanted = true,
                BannerImagePath = "stores\\paynspray.png",
            },

            new RepairGarage(new Vector3(258.2158f, 2590.954f, 44.4838f), 189.6097f, "Pay 'n' Spray", "In Beautiful Harmony") {
                GarageDoors = new List<InteriorDoor>() {
                    new InteriorDoor(4104186511,new Vector3(257.5422f,2596.084f,47.27481f)),
                },
                CameraPosition = new Vector3(258.9244f, 2608.994f, 48.34476f), 
                CameraDirection = new Vector3(-0.1198255f, -0.9714963f, -0.2045401f), 
                CameraRotation = new Rotator(-11.80258f, -6.541603E-06f, 172.9686f),
                OpenTime = 0,
                CloseTime = 24,
                CanInteractWhenWanted = true,
                BannerImagePath = "stores\\paynspray.png",
            },


            new RepairGarage(new Vector3(147.3954f, 320.8767f, 111.6681f), 293.5303f, "Pay 'n' Spray", "Serving Downtown Vinewood") {
                GarageDoors = new List<InteriorDoor>() {
                    new InteriorDoor(3379875310,new Vector3(143.4703f,319.201f,113.5349f)),
                },
                CameraPosition = new Vector3(136.044f, 306.7173f, 116.2294f), 
                CameraDirection = new Vector3(0.6137977f, 0.779637f, -0.124171f), 
                CameraRotation = new Rotator(-7.132885f, -4.302163E-06f, -38.21288f),
                OpenTime = 0,
                CloseTime = 24,
                CanInteractWhenWanted = true,
                BannerImagePath = "stores\\paynspray.png",
            },

            new RepairGarage(new Vector3(135.8826f, -1049.596f, 29.15182f), 338.5946f, "Pay 'n' Spray", "Serving Pillbox Hill") {
                GarageDoors = new List<InteriorDoor>() {
                    new InteriorDoor(3312435724,new Vector3(134.1454f,-1054.235f,31.3015f)),
                },
                CameraPosition = new Vector3(135.4085f, -1069.955f, 34.32735f), 
                CameraDirection = new Vector3(-0.06820366f, 0.9694805f, -0.2354905f), 
                CameraRotation = new Rotator(-13.62054f, -8.784793E-07f, 4.024169f),
                OpenTime = 0,
                CloseTime = 24,
                CanInteractWhenWanted = true,
                BannerImagePath = "stores\\paynspray.png",
            },

        };
    }
    private void DefaultConfig_IllicitMarketplaces()
    {
        illicitMarketplaces = new List<IllicitMarketplace>() {

            new IllicitMarketplace(new Vector3(-423.8026f, 185.627f, 80.80039f), 168.7502f, "Dealer Hangout 1", "Dealer Hangout 1","DealerHangoutMenu1") { 
                OpenTime = 0, CloseTime = 24, 
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-423.8026f, 185.627f, 80.80039f), 168.7502f) },//,VendorModels = new List<string>() { "IG_DrugDealer", "S_M_Y_Dealer_01" } ,
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
                }
            
            
            },


            new IllicitMarketplace(new Vector3(-1565.635f, -422.2258f, 37.94275f), 123.8133f, "Dealer Hangout 2", "Dealer Hangout 2","DealerHangoutMenu1") {
                OpenTime = 0, CloseTime = 24,
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-1565.635f, -422.2258f, 37.94275f), 123.8133f) },
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
                }


            },

            new IllicitMarketplace(new Vector3(737.3876f, -654.9383f, 28.26988f),  14.88211f, "Dealer Hangout 3", "Dealer Hangout 3","DealerHangoutMenu1") {
                OpenTime = 0, CloseTime = 24,
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(737.3876f, -654.9383f, 28.26988f), 14.88211f) },
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
                }


            },

            new IllicitMarketplace(new Vector3(1528.213f, 3590.099f, 35.45225f), 168.7502f, "Dealer Hangout 4", "Dealer Hangout 4","DealerHangoutMenu1") {
                OpenTime = 0, CloseTime = 24,
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(1528.213f, 3590.099f, 35.45225f), 357.4594f) },
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
                }


            },
            new IllicitMarketplace(new Vector3(-30.06856f, 6456.729f, 31.47281f), 243.118f, "Dealer Hangout 5", "Dealer Hangout 5","DealerHangoutMenu1") {
                OpenTime = 0, CloseTime = 24,
                VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-30.06856f, 6456.729f, 31.47281f), 243.118f) },
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
                }


            },
            //(-423.8026f, 185.627f, 80.80039f)
        };
    }
    private void DefaultConfig_BlankLocations()
    {
        BlankLocationsData blankLocationsData = new BlankLocationsData();
        blankLocationsData.DefaultConfig();
        BlankLocationPlaces = new List<BlankLocation>();
        BlankLocationPlaces.AddRange(blankLocationsData.BlankLocationPlaces);
    }
    private void DefaultConfig_FoodStands()
    {
        FoodStands = new List<FoodStand>()
        {
            new FoodStand(new Vector3(403.3527f, 106.0655f, 101.4575f), 241.199f, "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(403.3527f, 106.0655f, 101.4575f), 241.199f) }, BannerImagePath = "stores\\beefybills.png" },
            new FoodStand(new Vector3(245.8918f, 161.5893f, 104.9487f), 3.803493f, "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(245.8918f, 161.5893f, 104.9487f), 3.803493f) }, BannerImagePath = "stores\\beefybills.png" },
            new FoodStand(new Vector3(-1268.011f, -1432.715f, 4.353373f), 134.2259f, "Beefy Bills Burger Bar","Extra BEEFY!","BeefyBillsMenu"){ VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-1268.011f, -1432.715f, 4.353373f), 134.2259f) }, BannerImagePath = "stores\\beefybills.png" },
            new FoodStand(new Vector3(-1232.426f, -1485.006f, 4.362638f), 137.5475f, "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-1232.426f, -1485.006f, 4.362638f), 137.5475f) }, BannerImagePath = "stores\\beefybills.png" },
            new FoodStand(new Vector3(821.2138f, -2977.05f, 6.02066f), 272.7679f, "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(821.2138f, -2977.05f, 6.02066f), 272.7679f) }, BannerImagePath = "stores\\beefybills.png" },


             new FoodStand(new Vector3(-1693.241f, -1073.102f, 13.25018f), 47.97861f, "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-1693.241f, -1073.102f, 13.25018f), 47.97861f) }, BannerImagePath = "stores\\beefybills.png" },
              new FoodStand(new Vector3(-1784.236f, -1175.884f, 13.01774f), 55.52537f, "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-1784.236f, -1175.884f, 13.01774f), 55.52537f) }, BannerImagePath = "stores\\beefybills.png" },
               new FoodStand(new Vector3(-1857.076f, -1225.12f, 13.01722f), 316.8728f, "Beefy Bills Burger Bar", "Extra BEEFY!","BeefyBillsMenu"){ VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-1857.076f, -1225.12f, 13.01722f), 316.8728f) }, BannerImagePath = "stores\\beefybills.png" },

            new FoodStand(new Vector3(240.8329f, 167.2296f, 105.0605f), 167.5996f,  "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(240.8329f, 167.2296f, 105.0605f), 167.5996f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new FoodStand(new Vector3(-1516.382f, -952.5892f, 9.278718f), 317.7292f, "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu") { VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-1516.382f, -952.5892f, 9.278718f), 317.7292f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new FoodStand(new Vector3(1604.818f, 3822.332f, 34.69806f), 200.7076f, "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ VendorLocations = new List<SpawnPlace>() {
                new SpawnPlace(new Vector3(1607.50471f, 3823.54761f, 33.85147f), -152.151688f) 
            }, BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new FoodStand(new Vector3(-1248.932f, -1474.449f, 4.277946f), 306.3787f, "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-1248.932f, -1474.449f, 4.277946f), 306.3787f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new FoodStand(new Vector3(821.8197f, -2973.398f, 6.020657f), 276.5136f, "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(821.8197f, -2973.398f, 6.020657f), 276.5136f ) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new FoodStand(new Vector3(-1219.656f, -1504.36f, 4.36032f), 98.7149f, "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-1219.656f, -1504.36f, 4.36032f), 98.7149f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },
            //new Vector3(-1857.076f, -1225.12f, 13.01722f), 316.8728f

            new FoodStand(new Vector3(-1834.99f, -1234.289f, 13.01727f), 38.8311f, "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-1834.99f, -1234.289f, 13.01727f), 38.8311f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new FoodStand(new Vector3(-1772.159f, -1160.8f, 13.01804f), 50.64023f, "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-1772.159f, -1160.8f, 13.01804f), 50.64023f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new FoodStand(new Vector3(-1719.628f, -1103.968f, 13.01766f), 37.79702f, "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-1719.628f, -1103.968f, 13.01766f), 37.79702f) }, BannerImagePath = "stores\\chihuahuahotdogs.png" },



            //new FoodStand(new Vector3(821.7623f, -2973.566f, 6.020659f), 269.9576f, "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu"){ VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(821.7623f, -2973.566f, 6.020659f), VendorHeading = 269.9576f, BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new FoodStand(new Vector3(2106.954f, 4947.911f, 40.95187f), 319.9109f, "Attack-A-Taco", "Heavy Shelling!","TacoFarmerMenu") { BannerImagePath = "stores\\attackataco.png",VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(2106.954f, 4947.911f, 40.95187f), 319.9109f) }, },
            new FoodStand(new Vector3(-1148.969f, -1601.963f, 4.390241f), 35.73399f, "Gyro Day", "Gyro Day","GenericMenu") { BannerImagePath = "stores\\gyroday.png", VendorLocations = new List<SpawnPlace>() { 
                //new SpawnPlace(new Vector3(-1145.969f, -1602.963f, 4.390241f), 35.73399f),
            new SpawnPlace(new Vector3(-1143.96021f, -1600.45581f, 4.091243f), 35.73399f),
            new SpawnPlace(new Vector3(-1149.36f, -1604.32f, 4.091243f), 46.0f),
            },
            
            
            
            
            },
            new FoodStand(new Vector3(1604.578f, 3828.483f, 34.4987f), 142.3778f, "Tough Nut Donut", "Our DoNuts are Crazy!","DonutMenu"){ VendorLocations = new List<SpawnPlace>() { 
                //new SpawnPlace(new Vector3(1604.578f, 3828.483f, 34.4987f) , 142.3778f) }, 
            new SpawnPlace(new Vector3(1604.47949f, 3828.14526f, 33.5987f) , 142.3778f) },
            },
            new FoodStand(new Vector3(1087.509f, 6510.788f, 21.0551f), 185.487f, "Roadside Fruit", "Should Be OK To Eat","FruitMenu") { VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(1087.509f, 6510.788f, 21.0551f), 185.487f) }, },
            new FoodStand(new Vector3(2526.548f, 2037.936f, 19.82413f), 263.8982f, "Roadside Fruit", "Should Be OK To Eat","FruitMenu") { VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(2526.548f, 2037.936f, 19.82413f), 263.8982f) }, },
            new FoodStand(new Vector3(1263.013f, 3548.566f, 35.14751f), 187.8834f, "Roadside Fruit", "Should Be OK To Eat","FruitMenu") { VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(1263.013f, 3548.566f, 35.14751f), 187.8834f) }, },
            new FoodStand(new Vector3(1675.873f, 4883.532f, 42.06379f), 57.34329f, "Grapeseed Fruit", "Grapeseed Fruit","FruitMenu") { VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(1675.873f, 4883.532f, 42.06379f), 57.34329f) }, },
            new FoodStand(new Vector3(-462.6676f, 2861.85f, 34.90421f), 162.4888f, "Roadside Fruit", "Roadside Fruit","FruitMenu") { VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-462.6676f, 2861.85f, 34.90421f), 162.4888f) }, },
        };
    }
    private void DefaultConfig_Bars()
    {
        Bars = new List<Bar>()
        {
            new Bar(new Vector3(224.5178f, 336.3819f, 105.5973f), 340.0694f, "Pitchers", "","BarMenu") { OpenTime = 0, CloseTime = 24 } ,
            new Bar(new Vector3(219.5508f, 304.9488f, 105.5861f), 250.1051f, "Singletons", "","BarMenu") { OpenTime = 0, CloseTime = 24 } ,
            new Bar(new Vector3(1982.979f, 3053.596f, 47.21508f), 226.3188f, "Yellow Jacket Inn", "","BarMenu") { VendorFightPercentage = 45f,VendorFightPolicePercentage = 10f, OpenTime = 0, CloseTime = 24,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(1982.979f, 3053.596f, 47.21508f), 226.3188f) }, ScannerFilePath = "01_specific_location\\0x02C36B8B.mp3",VendorPersonnelID = "BarPeds",  },
            new Bar(new Vector3(-262.8396f, 6291.08f, 31.49327f), 222.9271f, "The Bay Bar", "","BarMenu") { OpenTime = 0, CloseTime = 24 } ,
            new Bar(new Vector3(-576.9105f, 239.0964f, 82.63644f), 354.0043f, "The Last Resort", "","BarMenu") { OpenTime = 0, CloseTime = 24 } ,
            new Bar(new Vector3(255.3016f, -1013.603f, 29.26964f), 70.28053f, "Shenanigan's Bar", "","BarMenu") { OpenTime = 0, CloseTime = 24 } ,
            new Bar(new Vector3(1218.175f, -416.5078f, 67.78294f), 74.95883f, "Mirror Park Tavern", "","BarMenu") { OpenTime = 0, CloseTime = 24 } ,
            new Bar(new Vector3(-1388.5f, -586.6741f, 30.21859f), 31.53231f, "Bahama Mama's", "","BarMenu") { OpenTime = 0, CloseTime = 24,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-1391.372f, -605.995f, 30.31955f), 116.404f) }, InteriorID = 107778, VendorPersonnelID = "BarPeds", },//TeleportEnterPosition = new Vector3(-1387.984f, -587.4419f, 30.31951f), TeleportEnterHeading = 210.6985f,
            new Bar(new Vector3(-564.6519f, 276.2436f, 83.12064f), 175.5771f,"Tequila-La", "","BarMenu") { VendorFightPercentage =  25f,VendorFightPolicePercentage = 10f, OpenTime = 0, CloseTime = 24,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-561.9947f, 284.9062f, 82.17636f), 262.2369f) }, InteriorID = 72706, VendorPersonnelID = "BarPeds", },//need better coordinates
            new Bar(new Vector3(-2193.238f, 4290.112f, 49.17442f), 37.55579f, "Hookies", "","BarMenu") { OpenTime = 0, CloseTime = 24 } ,
        };
    }
    private void DefaultConfig_GasStations()//VendorLocations = new List<SpawnPlace>() { new SpawnPlace() },
    {
        GasStations = new List<GasStation>()
        {
            new GasStation(new Vector3(-711.9264f, -917.7573f, 19.21472f), 180.3014f, "LtD", "unLTD great prices!","LTDMenu"){ BannerImagePath = "stores\\ltd.png",VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-705.7453f, -913.6598f, 19.21559f), 83.75771f) },OpenTime = 0, CloseTime = 24, InteriorID = 47874 },
            new GasStation(new Vector3(1698.097f, 4929.837f, 42.0781f), 48.2484f, "LtD", "unLTD great prices!","LTDMenu"){ BannerImagePath = "stores\\ltd.png",VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(1698.044f, 4922.526f, 42.06367f), 314.3236f) },OpenTime = 0, CloseTime = 24, InteriorID = 45570 },



            //BROKEN VENDOR POS?//new GasStation(new Vector3(1159.861f, -327.4188f, 69.21286f), 188.791f, "LtD","unLTD great prices!","LTDMenu"){ BannerImagePath = "stores\\ltd.png",VendorPosition = new Vector3(1164.927f, -323.7075f, 69.2051f), VendorHeading = 90.6181f, OpenTime = 0, CloseTime = 24, InteriorID = 2050 },
            new GasStation(new Vector3(1159.861f, -327.4188f, 69.21286f), 188.791f, "LtD","unLTD great prices!","LTDMenu"){ BannerImagePath = "stores\\ltd.png",VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(1165.12f, -322.5843f, 68.80514f), 98.88105f) },OpenTime = 0, CloseTime = 24, InteriorID = 2050, },



            new GasStation(new Vector3(-1817.871f,787.0063f,137.917f), 89.38248f, "LtD","unLTD great prices!","LTDMenu"){ BannerImagePath = "stores\\ltd.png",OpenTime = 0, CloseTime = 24, InteriorID = 82178,VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-1819.707f, 794.1723f, 138.0823f),122.8981f) },   },
            //new Vector3(1165.405f, -323.4462f, 69.20515f)
            new GasStation(new Vector3(166.2001f, -1553.691f, 29.26175f), 218.9514f, "Ron", "Put RON in your tank","RonMenu") {  BannerImagePath = "stores\\ron.png",OpenTime = 4, CloseTime = 22, ScannerFilePath = "01_specific_location\\0x0D6F777B.mp3", CameraPosition = new Vector3(175.2995f, -1593.878f, 39.27175f), CameraDirection = new Vector3(-0.1031758f, 0.9726905f, -0.2079136f), CameraRotation = new Rotator(-12.00011f, 0f, 6.054868f) },
            new GasStation(new Vector3(-1427.998f, -268.4702f, 46.2217f), 132.4002f, "Ron", "Put RON in your tank","RonMenu"){ BannerImagePath = "stores\\ron.png",OpenTime = 4, CloseTime = 22, ScannerFilePath = "01_specific_location\\0x056AF0EC.mp3" },
            new GasStation(new Vector3(2559.112f, 373.5359f, 108.6211f), 265.8011f, "Ron", "Put RON in your tank","RonMenu") { BannerImagePath = "stores\\ron.png",OpenTime = 4, CloseTime = 22 },
           // new GasStation(new Vector3(-1429.33f,-270.8909f,46.2077f), 325.7301f, "Ron","Put RON in your tank","RonMenu") { BannerImagePath = "stores\\ron.png",OpenTime = 4, CloseTime = 22 },
            new GasStation(new Vector3(-2544.116f, 2315.928f, 33.21614f), 3.216755f, "Ron", "Put RON in your tank","RonMenu"){ BannerImagePath = "stores\\ron.png",OpenTime = 4, CloseTime = 22 },
            new GasStation(new Vector3(818.2819f, -1040.907f, 26.75079f), 358.5326f, "Ron", "Put RON in your tank","RonMenu"){ BannerImagePath = "stores\\ron.png",OpenTime = 4, CloseTime = 22, ScannerFilePath = "01_specific_location\\0x0011827A.mp3" },
            new GasStation(new Vector3(1211.169f, -1388.923f, 35.3769f), 180.4454f, "Ron", "Put RON in your tank","RonMenu"){ BannerImagePath = "stores\\ron.png",OpenTime = 4, CloseTime = 22, ScannerFilePath = "01_specific_location\\0x0723E151.mp3" },

            new GasStation(new Vector3(-531.5529f, -1220.763f, 18.455f), 347.6858f, "Xero", "We have Xero gasoline","XeroMenu"){ OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\xero.png", ScannerFilePath = "01_specific_location\\0x04510C42.mp3" },
            new GasStation(new Vector3(289.5112f, -1266.584f, 29.44076f), 92.24692f, "Xero", "We have Xero gasoline","XeroMenu"){ OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\xero.png" },
            new GasStation(new Vector3(-92.79028f, 6409.667f, 31.64035f), 48.08112f, "Xero", "We have Xero gasoline","XeroMenu"){ OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\xero.png", ScannerFilePath = "01_specific_location\\0x1B81EF89.mp3" },
            new GasStation(new Vector3(46.75933f, 2789.635f, 58.10043f), 139.5097f, "Xero", "We have Xero gasoline","XeroMenu"){ OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\xero.png", ScannerFilePath = "01_specific_location\\0x18C6F152.mp3" },

            new GasStation(new Vector3(-2072.911f, -327.2192f, 13.31597f), 83.09109f, "Xero", "We have Xero gasoline","XeroMenu"){ OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\xero.png",  },


            new GasStation(new Vector3(1705.88f, 6425.68f, 33.37f), 153.9039f, "Globe Oil", "Changing the Climate, Powering The Future","GasStationMenu") { BannerImagePath = "stores\\globeoil.png",ScannerFilePath = "01_specific_location\\0x007AC3FC.mp3" },



            new GasStation(new Vector3(160.4977f,6635.249f,31.61175f), 70.88637f, "Dons Country Store & Gas","Country Manners!","GasStationMenu"),
            new GasStation(new Vector3(266.2746f,2599.669f,44.7383f), 231.9223f, "Harmony General Store & Gas","Always in Harmony!","GasStationMenu"),
            new GasStation(new Vector3(1039.753f,2666.26f,39.55253f), 143.6208f, "Grande Senora Cafe & Gas","Extra Grande!","GasStationMenu"),
            new GasStation(new Vector3(2001.239f, 3779.786f, 32.18078f), 208.5214f, "Sandy's Gas", "And Full Service!","GasStationMenu"){ OpenTime = 4, CloseTime = 22 },
            new GasStation(new Vector3(646.0997f, 267.417f, 103.2494f), 58.99448f, "Globe Oil", "Changing the Climate, Powering The Future","GasStationMenu") { BannerImagePath = "stores\\globeoil.png" },
            new GasStation(new Vector3(-342.2267f, -1475.199f, 30.74949f), 265.7801f, "Globe Oil", "Changing the Climate, Powering The Future","GasStationMenu"){ BannerImagePath = "stores\\globeoil.png" },
            new GasStation(new Vector3(1776.308f, 3327.483f, 41.43329f), 328.0875f, "Flywheels Gas", "Gas And More","GasStationMenu"),
            new GasStation(new Vector3(1201.978f, 2654.854f, 37.85188f), 315.5364f, "Route 68 Store", "Right off historic Route 68","GasStationMenu"),

        };
    }
    private void DefaultConfig_LiquorStores()
    {
        LiquorStores = new List<LiquorStore>()
        {
            new LiquorStore(new Vector3(-1226.09f, -896.166f, 12.4057f), 22.23846f, "Rob's Liquors","Thats My Name, Don't Rob Me!","LiquorStoreMenu") { VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-1221.119f, -908.5667f, 12.32635f),33.35855f) },OpenTime = 4, CloseTime = 22, InteriorID = 50178 },
            new LiquorStore(new Vector3(-2974.098f, 390.9085f, 15.03413f), 84.05217f, "Rob's Liquors", "Thats My Name, Don't Rob Me!","LiquorStoreMenu"){ VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-2966.361f, 390.1463f, 15.04331f), 88.73301f) },OpenTime = 4, CloseTime = 22, InteriorID = 19202 },
            new LiquorStore(new Vector3(-1491.638f, -384.0242f, 40.08308f), 136.3043f, "Rob's Liquors", "Thats My Name, Don't Rob Me!","LiquorStoreMenu") { VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-1486.196f, -377.7115f, 40.16343f), 133.357f) },OpenTime = 4, CloseTime = 22, InteriorID = 98818 },
            new LiquorStore(new Vector3(1141.463f, -980.9073f, 46.41084f), 275.826f,"Rob's Liquors", "Thats My Name, Don't Rob Me!","LiquorStoreMenu"){ VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(1133.534f, -983.1248f, 46.41584f), 276.1162f) },OpenTime = 4, CloseTime = 22, InteriorID = 73986 },
            new LiquorStore(new Vector3(1166.318f, 2702.173f, 38.17925f), 175.7192f, "Scoops Liquor Barn", "We've got the scoop on booze","LiquorStoreMenu"){ VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(1165.581f, 2710.774f, 38.15771f), 176.1093f) },OpenTime = 4, CloseTime = 22, InteriorID = 33026 },
            new LiquorStore(new Vector3(-596.1056f, 277.69f, 82.16035f), 170.2155f, "Eclipse Liquor.Deli", "You'll be seeing a real blackout","LiquorStoreMenu") ,
            new LiquorStore(new Vector3(-239.7314f, 244.0006f, 92.03992f), 318.9458f, "Ellen's Liquor Lover", "Generous Liquor","LiquorStoreMenu") ,
            new LiquorStore(new Vector3(807.3504f, -1073.531f, 28.92093f), 134.3175f, "Liquor Market", "","LiquorStoreMenu") ,
            new LiquorStore(new Vector3(234.4411f, -1946.841f, 22.95617f), 3.404058f, "Liquor Store", "","LiquorStoreMenu"),
            new LiquorStore(new Vector3(1212.343f, -445.0079f, 66.96259f), 74.05521f, "Liquor", "","LiquorStoreMenu") ,
            new LiquorStore(new Vector3(-1208.469f, -1384.053f, 4.085135f), 68.08948f, "Steamboat Beers", "","LiquorStoreMenu") ,
            new LiquorStore(new Vector3(-1106.07f, -1287.686f, 5.421459f), 161.3398f, "Vespucci Liquor Market", "","LiquorStoreMenu") ,
            new LiquorStore(new Vector3(-697.8242f, -1182.286f, 10.71113f), 132.7831f, "Liquor Market", "","LiquorStoreMenu") { OpenTime = 4, CloseTime = 22 },
            new LiquorStore(new Vector3(-882.7062f, -1155.351f, 5.162508f), 215.8305f, "Liquor Hole", "You know you want it!","LiquorStoreMenu") { OpenTime = 4, CloseTime = 22,BannerImagePath = "stores\\liquorhole.png" },
            new LiquorStore(new Vector3(-601.9684f, 244.0188f, 82.3046f), 358.6468f, "Liquor Hole", "You know you want it!","LiquorStoreMenu") { OpenTime = 4, CloseTime = 22,BannerImagePath = "stores\\liquorhole.png" },
            new LiquorStore(new Vector3(456.5478f, 130.5207f, 99.28537f), 162.9724f, "Vinewood Liquor", "","LiquorStoreMenu") { OpenTime = 4, CloseTime = 22 },
            new LiquorStore(new Vector3(1391.861f, 3606.275f, 34.98093f), 199.2899f, "Liquor Ace", "Now socially acceptable", "LiquorStoreMenu") { InteriorID = 104450, VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(1391.861f, 3606.275f, 34.98093f), 199.2899f) }, OpenTime = 4, CloseTime = 22, ScannerFilePath  = "01_specific_location\\0x1A23351D.mp3" },
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
    }
    private void DefaultConfig_ConvenienceStores()
    {
        ConvenienceStores = new List<ConvenienceStore>()
        {
            new ConvenienceStore(new Vector3(547f, 2678f, 42.1565f), 22.23846f,  "24/7","As fast as you","TwentyFourSevenMenu"){ OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\247.png", InteriorID = 41474, VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(549.6005f, 2669.846f, 42.1565f), 96.91093f) },},
            new ConvenienceStore(new Vector3(-3236.767f,1005.609f,12.33137f), 122.6316f,  "24/7","As fast as you","TwentyFourSevenMenu"){ OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\247.png", InteriorID = 16386, VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-3243.302f, 1000.005f, 12.83071f), 352.6259f) }, },
            new ConvenienceStore(new Vector3(2560f, 385f, 108f), 22.23846f,  "24/7","As fast as you","TwentyFourSevenMenu"){ OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\247.png", InteriorID = 62722, VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(2555.339f, 380.9034f, 108.6229f), 347.3629f) }, },
            new ConvenienceStore(new Vector3(29.32254f, -1350.485f, 29.33319f), 170.9901f,  "24/7", "As fast as you","TwentyFourSevenMenu"){ OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\247.png", InteriorID = 33282, VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(24.39647f, -1345.484f, 29.49702f), 252.9084f) }, },
            new ConvenienceStore(new Vector3(-3037.729f, 589.7671f, 7.814812f), 289.0175f,  "24/7", "As fast as you","TwentyFourSevenMenu"){ OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\247.png", InteriorID = 97538, VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-3039.787f, 584.1979f, 7.908929f), 12.80189f) }, },
            new ConvenienceStore(new Vector3(376.3202f, 322.694f, 103.4389f), 162.5363f,  "24/7", "As fast as you","TwentyFourSevenMenu"){ OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\247.png", InteriorID = 46850, VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(372.6485f, 327.0293f, 103.5664f), 257.6475f) },ScannerFilePath = "01_specific_location\\0x000E7300.mp3" },//Vinewood
            new ConvenienceStore(new Vector3(2682.938f, 3282.287f, 55.24056f), 243.885f,  "24/7", "As fast as you","TwentyFourSevenMenu") { OpenTime = 0, CloseTime = 24 ,BannerImagePath = "stores\\247.png", InteriorID = 13826, VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(2676.595f, 3280.101f, 55.24113f), 325.0921f) },},
            new ConvenienceStore(new Vector3(1730.507f, 6410.014f, 35.00065f), 153.9039f,  "24/7","As fast as you","TwentyFourSevenMenu") {  OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\247.png", InteriorID = 36354, VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(1728.436f, 6416.584f, 35.03722f), 241.2023f) }, },//Braddock pass
            new ConvenienceStore(new Vector3(1965.801f, 3739.945f, 32.322f), 207.564f,  "24/7","As fast as you","TwentyFourSevenMenu") { OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\247.png", InteriorID = 55554, VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(1959.352f, 3741.18f, 32.34374f), 303.8849f) }, },
            new ConvenienceStore(new Vector3(-53.5351f, -1757.196f, 29.43954f), 146.0623f,  "LtD", "unLTD great prices!","LTDMenu"){ BannerImagePath = "stores\\ltd.png",OpenTime = 0, CloseTime = 24, InteriorID = 80642, VendorLocations = new List<SpawnPlace>() { new SpawnPlace(new Vector3(-45.89098f, -1757.345f, 29.42101f), 52.66933f) }, },
            new ConvenienceStore(new Vector3(-578.0112f, -1012.898f, 22.32503f), 359.4114f,  "24/7", "As fast as you","TwentyFourSevenMenu"){ OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\247.png" },
            new ConvenienceStore(new Vector3(-696.9965f, -858.7673f, 23.69209f), 85.51252f,  "24/7", "24/7","TwentyFourSevenMenu"){ OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\247.png" },
            new ConvenienceStore(new Vector3(152.5101f, 237.4131f, 106.9718f), 165.2823f,  "24/7", "As fast as you","TwentyFourSevenMenu"){ OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\247.png" },
            new ConvenienceStore(new Vector3(201.8985f, -26.30606f, 69.90953f), 249.8224f,  "24/7", "As fast as you","TwentyFourSevenMenu"){ OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\247.png" },
            new ConvenienceStore(new Vector3(528.017f, -152.1372f, 57.20173f), 44.64286f,  "24/7", "As fast as you","TwentyFourSevenMenu"){ OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\247.png" },
            new ConvenienceStore(new Vector3(-1264.064f, -1162.599f, 6.764161f), 161.218f,  "Fruit Of The Vine", "Not just for winos","FruitVineMenu"){ BannerImagePath = "stores\\fruitofthevine.png" },
            new ConvenienceStore(new Vector3(-1270.649f, -304.9037f, 37.06938f), 257.2106f,  "Fruit Of The Vine", "Not just for winose","FruitVineMenu") { BannerImagePath = "stores\\fruitofthevine.png", IsOnMPMap = false},
            new ConvenienceStore(new Vector3(164.9962f, 351.1263f, 109.6859f), 4.847032f,  "Fruit Of The Vine", "Not just for winos","FruitVineMenu") { BannerImagePath = "stores\\fruitofthevine.png" },
            new ConvenienceStore(new Vector3(-144.3732f, -65.01408f, 54.60635f), 159.0404f,  "Fruit Of The Vine", "Not just for winos","FruitVineMenu") { BannerImagePath = "stores\\fruitofthevine.png" },
            new ConvenienceStore(new Vector3(-1412.015f, -320.1292f, 44.37897f), 92.48502f,  "The Grain Of Truth", "Seek the truth","GrainOfTruthMenu"){ BannerImagePath = "stores\\grainoftruth.png" },
            new ConvenienceStore(new Vector3(-1370.819f, -684.5463f, 25.01069f), 214.6929f,  "The Grain Of Truth", "Seek the truth","GrainOfTruthMenu"){ BannerImagePath = "stores\\grainoftruth.png" },
            new ConvenienceStore(new Vector3(1707.748f, 4792.387f, 41.98377f), 90.42564f,  "Supermarket", "","ConvenienceStoreMenu"),
            new ConvenienceStore(new Vector3(-1539.045f, -900.472f, 10.16951f), 129.0318f,  "Del Perro Food Market","No Robberies Please!","ConvenienceStoreMenu") { IsOnMPMap = false },
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
            new ConvenienceStore(new Vector3(-1113.532f, -1365.805f, 5.040397f), 207.2889f,  "Farmer Giles", "Organic Food Store","GrainOfTruthMenu"),
        };
    }
    private void DefaultConfig_Banks()
    {





        Banks = new List<Bank>()
        {
            new Bank(new Vector3(-1318f, -831.5065f, 16.97263f), 125.3848f, "Maze Bank", "Invest in the red", "Maze")
            {
                BannerImagePath = "stores\\maze.png",
            },
            new Bank(new Vector3(-813.9924f, -1114.698f, 11.18181f), 297.7995f, "Fleeca Bank", "Everything, at a price","Fleeca")
            {
                BannerImagePath = "stores\\fleeca.png",
            },
            new Bank(new Vector3(-350.1604f, -45.84864f, 49.03682f), 337.4063f, "Fleeca Bank", "Everything, at a price","Fleeca")
            {
                BannerImagePath = "stores\\fleeca.png",
                InteriorID = 71682,
                VendorLocations = new List<SpawnPlace>() 
                {
                    new SpawnPlace(new Vector3(-351.3789f, -51.64762f, 49.03649f), 336.6109f),
                },
                ActivateCells = 3,
                ActivateDistance = 75f,
                RestrictedAreas = new RestrictedAreas()
                {
                    RestrictedAreasList = new List<RestrictedArea>()
                    {
                        new RestrictedArea("Fleeca Vault Room",new Vector2[]
                                                                {
                                                                    new Vector2 { X = -352.0612f, Y = -53.76372f },
                                                                    new Vector2 { X = -349.4093f, Y = -55.09093f },
                                                                    new Vector2 { X = -350.9847f, Y = -60.41546f },
                                                                    new Vector2 { X = -354.1934f, Y = -58.88084f },
                                                                },
                                                                null,RestrictedAreaType.Bank) { IsCivilianReactableRestricted = true, },
                        new RestrictedArea("Fleeca Front Room",new Vector2[]
                                                                {
                                                                    new Vector2 { X = -354.5473f, Y = -50.24043f },
                                                                    new Vector2 { X = -348.8781f, Y = -52.28946f },
                                                                    new Vector2 { X = -356.3486f, Y = -53.77395f },
                                                                    new Vector2 { X = -349.7612f, Y = -56.4671f },
                                                                },
                                                                null,RestrictedAreaType.Bank) { IsCivilianReactableRestricted = true, },
                    }
                },
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new SecurityConditionalLocation(new Vector3(-352.5154f,-46.69918f,49.03637f),158.5911f,95f) { AssociationID = "GRP6",RequiredPedGroup = "ArmedSecurity", MinHourSpawn = 6, MaxHourSpawn = 20, TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>() { "WORLD_HUMAN_GUARD_STAND" }, },
                    new SecurityConditionalLocation(new Vector3(-350.0684f,-55.37014f,49.01481f),72.79137f,95f) { AssociationID = "GRP6",RequiredPedGroup = "ArmedSecurity", LongGunAlwaysEquipped = true, ForceSidearm = true,ForceLongGun = true, MinHourSpawn = 6, MaxHourSpawn = 20, TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>() { "WORLD_HUMAN_GUARD_STAND" }, },
                },
            },
            new Bank(new Vector3(150.9058f, -1036.347f, 29.33961f), 340.9843f,  "Fleeca Bank", "Everything, at a price","Fleeca")
            {
                BannerImagePath = "stores\\fleeca.png",
                InteriorID = 76802,
                VendorLocations = new List<SpawnPlace>() {
                    new SpawnPlace(new Vector3(147.8368f, -1041.57f, 29.36793f), 338.927f),
                    new SpawnPlace(new Vector3(149.4326f, -1042.337f, 29.368f), 340.193f),
                },
                ActivateCells = 3,
                ActivateDistance = 75f,
                RestrictedAreas = new RestrictedAreas()
                {
                    RestrictedAreasList = new List<RestrictedArea>()
                    {
                        new RestrictedArea("Fleeca Front Room",new Vector2[]
                                                                {
                                                                    new Vector2(146.3237f, -1041.013f),
                                                                    new Vector2(151.9075f, -1043.154f),
                                                                    new Vector2(151.6074f, -1044.077f),
                                                                    new Vector2(146.7068f, -1042.238f),
                                                                },
                                                                null,RestrictedAreaType.Bank) { IsCivilianReactableRestricted = true, },
                        new RestrictedArea("Fleeca Vault Room",new Vector2[]
                                                                {
                                                                    new Vector2(144.9341f, -1043.125f),
                                                                    new Vector2(151.3388f, -1045.964f),
                                                                    new Vector2(149.6541f, -1051.236f),
                                                                    new Vector2(144.7694f, -1045.295f),
                                                                },
                                                                null,RestrictedAreaType.Bank) { IsCivilianReactableRestricted = true, },
                    }
                },
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new SecurityConditionalLocation(new Vector3(148.3287f,-1037.451f,29.3679f),155.8272f,95f) { AssociationID = "GRP6",RequiredPedGroup = "ArmedSecurity", MinHourSpawn = 6, MaxHourSpawn = 20, TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>() { "WORLD_HUMAN_GUARD_STAND" }, },
                    new SecurityConditionalLocation(new Vector3(150.8173f,-1046.188f,29.34632f),69.36922f,95f) { AssociationID = "GRP6",RequiredPedGroup = "ArmedSecurity", LongGunAlwaysEquipped = true, ForceSidearm = true,ForceLongGun = true, MinHourSpawn = 6, MaxHourSpawn = 20, TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>() { "WORLD_HUMAN_GUARD_STAND" }, },
                },
            },
            new Bank(new Vector3(315.2256f, -275.1059f, 53.92431f), 345.6797f,  "Fleeca Bank", "Everything, at a price","Fleeca")
            {
                BannerImagePath = "stores\\fleeca.png",
                InteriorID = 11266,
                VendorLocations = new List<SpawnPlace>() {
                    new SpawnPlace(new Vector3(313.6212f, -280.8588f, 54.1647f), 335.8324f),
                    new SpawnPlace(new Vector3(312.5256f, -280.4068f, 54.1647f), 338.1261f),
                },
                ActivateCells = 3,
                ActivateDistance = 75f,
                RestrictedAreas = new RestrictedAreas()
                {
                    RestrictedAreasList = new List<RestrictedArea>()
                    {
                        new RestrictedArea("Fleeca Front Room",new Vector2[]
                                                                {
                                                                    new Vector2(310.6702f, -279.3873f),
                                                                    new Vector2(316.2504f, -281.5169f),
                                                                    new Vector2(315.9381f, -282.4389f),
                                                                    new Vector2(311.0368f, -280.6021f),
                                                                },
                                                                null,RestrictedAreaType.Bank) { IsCivilianReactableRestricted = true, },
                        new RestrictedArea("Fleeca Vault Room",new Vector2[]
                                                                {
                                                                    new Vector2(309.2658f, -281.4929f),
                                                                    new Vector2(315.6672f, -284.3145f),
                                                                    new Vector2(313.9878f, -289.6013f),
                                                                    new Vector2(310.8114f, -288.0096f),
                                                                    new Vector2(308.7519f, -282.864f),
                                                                },
                                                                null,RestrictedAreaType.Bank) { IsCivilianReactableRestricted = true, },
                    }
                },
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new SecurityConditionalLocation(new Vector3(312.5207f,-275.8242f,54.16462f),158.9338f,95f) { AssociationID = "GRP6",RequiredPedGroup = "ArmedSecurity", MinHourSpawn = 6, MaxHourSpawn = 20, TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>() { "WORLD_HUMAN_GUARD_STAND" }, },
                    new SecurityConditionalLocation(new Vector3(315.2496f,-284.7239f,54.14301f),70.262f,95f) { AssociationID = "GRP6",RequiredPedGroup = "ArmedSecurity", LongGunAlwaysEquipped = true, ForceSidearm = true,ForceLongGun = true, MinHourSpawn = 6, MaxHourSpawn = 20, TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>() { "WORLD_HUMAN_GUARD_STAND" }, },
                },
            },
            new Bank(new Vector3(-2966.905f, 483.1484f, 15.6927f), 86.25156f,  "Fleeca Bank", "Everything, at a price","Fleeca")
            {
                BannerImagePath = "stores\\fleeca.png",
                InteriorID = 20226,
                VendorLocations = new List<SpawnPlace>() {
                    new SpawnPlace(new Vector3(-2960.644f, 482.839f, 15.69701f), 81.83675f),
                },
                ActivateCells = 3,
                ActivateDistance = 75f,
                RestrictedAreas = new RestrictedAreas()
                {
                    RestrictedAreasList = new List<RestrictedArea>()
                    {
                        new RestrictedArea("Fleeca Front Room",new Vector2[]
                                                                {
                                                                    new Vector2(-2961.179f, 479.6267f),
                                                                    new Vector2(-2960.874f, 485.6059f),
                                                                    new Vector2(-2959.902f, 485.5941f),
                                                                    new Vector2(-2960.162f, 480.3656f),
                                                                },
                                                                null,RestrictedAreaType.Bank) { IsCivilianReactableRestricted = true, },
                        new RestrictedArea("Fleeca Vault Room",new Vector2[]
                                                                {
                                                                    new Vector2(-2958.778f, 478.9443f),
                                                                    new Vector2(-2958.035f, 485.9171f),
                                                                    new Vector2(-2952.487f, 485.9096f),
                                                                    new Vector2(-2953.033f, 482.3984f),
                                                                    new Vector2(-2957.316f, 478.8758f),
                                                                },
                                                                null,RestrictedAreaType.Bank) { IsCivilianReactableRestricted = true, },
                    }
                },
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new SecurityConditionalLocation(new Vector3(-2965.206f,480.3015f,15.6969f),268.606f,95f) { AssociationID = "GRP6",RequiredPedGroup = "ArmedSecurity", MinHourSpawn = 6, MaxHourSpawn = 20, TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>() { "WORLD_HUMAN_GUARD_STAND" }, },
                    new SecurityConditionalLocation(new Vector3(-2957.569f,485.632f,15.67533f),174.1285f,95f) { AssociationID = "GRP6",RequiredPedGroup = "ArmedSecurity", LongGunAlwaysEquipped = true, ForceSidearm = true,ForceLongGun = true, MinHourSpawn = 6, MaxHourSpawn = 20, TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>() { "WORLD_HUMAN_GUARD_STAND" }, },
                },
            },
            new Bank(new Vector3(1175.215f, 2702.15f, 38.17273f), 176.9885f, "Fleeca Bank", "Everything, at a price","Fleeca")
            {
                BannerImagePath = "stores\\fleeca.png",
                InteriorID = 90626,
                VendorLocations = new List<SpawnPlace>() {
                    new SpawnPlace(new Vector3(1175.087f, 2708.431f, 38.08793f), 177.2366f),
                },
                ActivateCells = 3,
                ActivateDistance = 75f,
                RestrictedAreas = new RestrictedAreas()
                {
                    RestrictedAreasList = new List<RestrictedArea>()
                    {
                        new RestrictedArea("Fleeca Front Room",new Vector2[]
                                                                {
                                                                    new Vector2(1178.291f, 2708.304f),
                                                                    new Vector2(1172.311f, 2708.387f),
                                                                    new Vector2(1172.286f, 2709.354f),
                                                                    new Vector2(1177.512f, 2709.322f),
                                                                },
                                                                null,RestrictedAreaType.Bank) { IsCivilianReactableRestricted = true, },
                        new RestrictedArea("Fleeca Vault Room",new Vector2[]
                                                                {
                                                                    new Vector2(1178.821f, 2710.764f),
                                                                    new Vector2(1171.883f, 2711.214f),
                                                                    new Vector2(1171.645f, 2716.751f),
                                                                    new Vector2(1175.179f, 2716.31f),
                                                                    new Vector2(1178.877f, 2712.224f),
                                                                },
                                                                null,RestrictedAreaType.Bank) { IsCivilianReactableRestricted = true, },
                    }
                },
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new SecurityConditionalLocation(new Vector3(1177.713f,2704.353f,38.08786f),0.2107314f,95f) { AssociationID = "GRP6",RequiredPedGroup = "ArmedSecurity", MinHourSpawn = 6, MaxHourSpawn = 20, TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>() { "WORLD_HUMAN_GUARD_STAND" }, },
                    new SecurityConditionalLocation(new Vector3(1172.005f,2711.791f,38.06627f), 270.1688f,95f) { AssociationID = "GRP6",RequiredPedGroup = "ArmedSecurity", LongGunAlwaysEquipped = true, ForceSidearm = true,ForceLongGun = true, MinHourSpawn = 6, MaxHourSpawn = 20, TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>() { "WORLD_HUMAN_GUARD_STAND" }, },
                },
            },
            new Bank(new Vector3(-1214.902f, -327.0157f, 37.6686f), 26.31765f, "Fleeca Bank", "Everything, at a price","Fleeca")
            {
                BannerImagePath = "stores\\fleeca.png",
                InteriorID = 87810,
                VendorLocations = new List<SpawnPlace>() {
                    new SpawnPlace(new Vector3(-1211.815f, -332.2156f, 37.78094f), 25.86222f),
                    new SpawnPlace(new Vector3(-1213.225f, -333.1036f, 37.78089f), 22.51491f),
                },
                ActivateCells = 3,
                ActivateDistance = 75f,
                RestrictedAreas = new RestrictedAreas()
                {
                    RestrictedAreasList = new List<RestrictedArea>()
                    {
                        new RestrictedArea("Fleeca Front Room",new Vector2[]
                                                                {
                                                                    new Vector2(-1214.961f, -333.622f),
                                                                    new Vector2(-1209.498f, -330.8935f),
                                                                    new Vector2(-1209.025f, -331.7485f),
                                                                    new Vector2(-1213.733f, -334.0669f),
                                                                },
                                                                null,RestrictedAreaType.Bank) { IsCivilianReactableRestricted = true, },
                        new RestrictedArea("Fleeca Vault Room",new Vector2[]
                                                                {
                                                                    new Vector2(-1214.247f, -335.963f),
                                                                    new Vector2(-1207.838f, -333.2203f),
                                                                    new Vector2(-1205.152f, -338.0285f),
                                                                    new Vector2(-1207.979f, -339.5f),
                                                                    new Vector2(-1213.623f, -337.2863f),
                                                                },
                                                                null,RestrictedAreaType.Bank) { IsCivilianReactableRestricted = true, },
                    }
                },
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new SecurityConditionalLocation(new Vector3(-1216.203f,-329.709f,37.78087f),205.3641f,95f) { AssociationID = "GRP6",RequiredPedGroup = "ArmedSecurity", MinHourSpawn = 6, MaxHourSpawn = 20, TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>() { "WORLD_HUMAN_GUARD_STAND" }, },
                    new SecurityConditionalLocation(new Vector3(-1207.684f,-333.7042f,37.75927f), 116.2364f,95f) { AssociationID = "GRP6",RequiredPedGroup = "ArmedSecurity", LongGunAlwaysEquipped = true, ForceSidearm = true,ForceLongGun = true, MinHourSpawn = 6, MaxHourSpawn = 20, TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>() { "WORLD_HUMAN_GUARD_STAND" }, },
                },
            },


            new Bank(new Vector3(230.2334f, 214.4399f, 105.552f), 115.9025f, "Pacific Standard Bank", "Since 1903","Pacific Std")
            {
                BannerImagePath = "stores\\pacificstandard.png",
                InteriorID = 103170,
                VendorLocations = new List<SpawnPlace>() {
                    new SpawnPlace(new Vector3(249.3135f, 224.6261f, 106.287f), 147.2702f),
                    new SpawnPlace(new Vector3(253.0091f, 223.5203f, 106.2868f), 151.6559f),
                },
                ActivateCells = 3,
                ActivateDistance = 100f,
                RestrictedAreas = new RestrictedAreas()
                {
                    RestrictedAreasList = new List<RestrictedArea>()
                    {
                        new RestrictedArea("Pacific Standard Teller Area",new Vector2[]
                                                                {
                                                                    new Vector2(240.7343f, 227.4087f),
                                                                    new Vector2(242.9524f, 233.4778f),
                                                                    new Vector2(259.9221f, 219.8555f),
                                                                    new Vector2(269.7702f, 223.7066f),
                                                                },
                                                                null,RestrictedAreaType.Bank) { IsCivilianReactableRestricted = true, },
                        new RestrictedArea("Pacific Standard Vault Stairs",new Vector2[]
                                                                {
                                                                    new Vector2(261.7909f, 219.8125f),
                                                                    new Vector2(267.5229f, 217.7241f),
                                                                    new Vector2(269.7004f, 223.6757f),
                                                                    new Vector2(253.7455f, 229.3237f),
                                                                },
                                                                null,RestrictedAreaType.Bank) { IsCivilianReactableRestricted = true, },
                        new RestrictedArea("Pacific Standard Vault",new Vector2[]
                                                                {
                                                                    new Vector2(251.6466f, 224.2327f),
                                                                    new Vector2(248.9319f, 216.9391f),
                                                                    new Vector2(265.0714f, 211.7376f),
                                                                    new Vector2(266.5878f, 215.077f),
                                                                    new Vector2(255.3204f, 222.8831f),
                                                                },
                                                                null,RestrictedAreaType.Bank) { IsZRestricted = true, ZRestrictionMin = 101.6835f - 3f,ZRestrictionMax = 101.6835f + 3f, IsCivilianReactableRestricted = true, },
                    }
                },
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new SecurityConditionalLocation(new Vector3(233.8826f,213.5529f,106.2868f),292.8797f,95f) { AssociationID = "GRP6",RequiredPedGroup = "ArmedSecurity", MinHourSpawn = 6, MaxHourSpawn = 20, TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>() { "WORLD_HUMAN_GUARD_STAND" }, },
                    new SecurityConditionalLocation(new Vector3(246.5008f,214.1364f,106.2868f),345.16f,95f) { AssociationID = "GRP6",RequiredPedGroup = "ArmedSecurity", MinHourSpawn = 6, MaxHourSpawn = 20, TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>() { "WORLD_HUMAN_GUARD_STAND" }, },
                    new SecurityConditionalLocation(new Vector3(240.2741f,214.4063f,110.283f),160.8276f,95f) { AssociationID = "GRP6",RequiredPedGroup = "ArmedSecurity", MinHourSpawn = 6, MaxHourSpawn = 20, TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>() { "WORLD_HUMAN_GUARD_STAND" }, },
                    new SecurityConditionalLocation(new Vector3(256.9109f, 226.4966f, 106.2868f), 162.127f, 95f){ AssociationID = "GRP6",RequiredPedGroup = "ArmedSecurity", MinHourSpawn = 6, MaxHourSpawn = 20, TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>() { "WORLD_HUMAN_GUARD_STAND" }, },
                    new SecurityConditionalLocation(new Vector3(262.8836f, 220.7263f, 101.6833f), 340.696f, 95f){ AssociationID = "GRP6",RequiredPedGroup = "ArmedSecurity", MinHourSpawn = 6, MaxHourSpawn = 20, TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>() { "WORLD_HUMAN_GUARD_STAND" }, },

                    //new SecurityConditionalLocation(new Vector3(256.9742f,220.4982f,106.2852f), 159.0666f,95f) { AssociationID = "GRP6", LongGunAlwaysEquipped = true, ForceSidearm = true,ForceLongGun = true, MinHourSpawn = 6, MaxHourSpawn = 20, TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>() { "WORLD_HUMAN_GUARD_STAND" }, },
                    new SecurityConditionalLocation(new Vector3(253.94f,221.3301f,101.6834f), 342.1928f,95f) { AssociationID = "GRP6",RequiredPedGroup = "ArmedSecurity", LongGunAlwaysEquipped = true, ForceSidearm = true,ForceLongGun = true, MinHourSpawn = 6, MaxHourSpawn = 20, TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>() { "WORLD_HUMAN_GUARD_STAND" }, },
                    new SecurityConditionalLocation(new Vector3(251.7139f,222.3622f,101.6834f), 332.5652f,95f) { AssociationID = "GRP6",RequiredPedGroup = "ArmedSecurity", LongGunAlwaysEquipped = true, ForceSidearm = true,ForceLongGun = true, MinHourSpawn = 6, MaxHourSpawn = 20, TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>() { "WORLD_HUMAN_GUARD_STAND" }, },


                },
            },

            new Bank(new Vector3(-3142.849f, 1131.727f, 20.84295f), 247.9002f, "Blaine County Savings", "Your nest egg!","Blaine CS")
            {
                BannerImagePath = "stores\\blainecountybank.png",
            },
            new Bank(new Vector3(-111.82f, 6461.803f, 31.63924f), 134.2345f, "Blaine County Savings", "Your nest egg!","Blaine CS")
            {
                BannerImagePath = "stores\\blainecountybank.png",
                InteriorID = 42754,
                VendorLocations = new List<SpawnPlace>() {
                    new SpawnPlace(new Vector3(-111.1494f, 6470.298f, 31.6267f), 133.0098f),
                },
                ActivateCells = 4,
                ActivateDistance = 150f,
                                RestrictedAreas = new RestrictedAreas()
                {
                    RestrictedAreasList = new List<RestrictedArea>()
                    {
                        new RestrictedArea("BCS Front Room",new Vector2[]
                                                                {
                                                                    new Vector2(-109.946f, 6468.715f),
                                                                    new Vector2(-114.6681f, 6473.577f),
                                                                    new Vector2(-114.1351f, 6474.118f),
                                                                    new Vector2(-109.2746f, 6469.363f),
                                                                },
                                                                null,RestrictedAreaType.Bank) { IsCivilianReactableRestricted = true, },
                        new RestrictedArea("BCS Vault Room",new Vector2[]
                                                                {
                                                                    new Vector2(-106.348f, 6472.824f),
                                                                    new Vector2(-107.8746f, 6475.334f),
                                                                    new Vector2(-105.0669f, 6479.51f),
                                                                    new Vector2(-102.1165f, 6476.42f),
                                                                },
                                                                null,RestrictedAreaType.Bank) { IsCivilianReactableRestricted = true, },
                    }
                },
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new SecurityConditionalLocation(new Vector3(-116.1009f,6471.834f,31.62671f),222.2569f,95f) { AssociationID = "GRP6",RequiredPedGroup = "ArmedSecurity", MinHourSpawn = 6, MaxHourSpawn = 20, TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>() { "WORLD_HUMAN_GUARD_STAND" }, },
                    new SecurityConditionalLocation(new Vector3(-107.3023f,6474.917f,31.62671f), 225.9335f,95f) { AssociationID = "GRP6",RequiredPedGroup = "ArmedSecurity", LongGunAlwaysEquipped = true, ForceSidearm = true,ForceLongGun = true, MinHourSpawn = 6, MaxHourSpawn = 20, TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>() { "WORLD_HUMAN_GUARD_STAND" }, },

                },
            },
            new Bank(new Vector3(-1103.88f, -1353.062f, 5.037252f), 206.2566f, "Lombank", "Our time is your money", "Lombank")
            {
                BannerImagePath = "stores\\lombank.png" 
            },
            new Bank(new Vector3(-863.493f, -193.3951f, 37.84323f), 299.7061f, "Lombank", "Our time is your money", "Lombank")
            {
                BannerImagePath = "stores\\lombank.png" 
            },
            new Bank(new Vector3(6.928352f, -932.4435f, 29.905f), 111.645f, "Lombank", "Our time is your money", "Lombank")
            {
                BannerImagePath = "stores\\lombank.png" 
            },
        };
    }
    private void DefaultConfig_SubwayStations()
    {
        SubwayStations = new List<SubwayStation>()
        {
            new SubwayStation(new Vector3(-245.8754f, -335.3599f, 29.97557f), 184.3399f, "Burton Subway Station",""),
            new SubwayStation(new Vector3(-490.9833f, -697.8607f, 33.24188f), 358.6056f, "Little Seoul Subway Station",""),
            new SubwayStation(new Vector3(-1368.768f, -528.9166f, 30.30007f), 125.7357f, "Del Perro Subway Station",""),
            new SubwayStation(new Vector3(-800.5429f, -101.6647f, 37.56856f), 294.5015f, "Portola Drive Subway Station",""),
            new SubwayStation(new Vector3(-1040.5f, -2742.292f, 13.92607f), 329.527f, "LSIA Terminal 4 Subway Station",""),
            new SubwayStation(new Vector3(-946.489f, -2329.504f, 6.763008f), 331.0874f, "LSIA Parking Subway Station",""),
        };
    }
    private void DefaultConfig_Landmarks()
    {
        Landmarks = new List<Landmark>()
        {
            //BOBCAT = BENEGALL
            //MERRY = LUX
            new Landmark(new Vector3(-248.491f, -2010.509f, 34.574f), 0f,"Maze Bank Arena","I heard Fame or Shame was filming there") 
            {
                OpenTime = 0,
                CloseTime = 24, 
                InteriorID = 78338,
                AssignedAssociationID = "GRP6",
                PossiblePedSpawns = new List<ConditionalLocation>() 
                {
                    new SecurityConditionalLocation(new Vector3(-259.8679f, -2034.324f, 29.94604f), 223.56f, 55f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.StandardScenario, RequiredPedGroup = "UnarmedSecurity" },
                    new SecurityConditionalLocation(new Vector3(-247.7519f, -2021.055f, 29.94604f), 234.7717f, 55f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.Patrol| TaskRequirements.StandardScenario, RequiredPedGroup = "UnarmedSecurity" },
                    new SecurityConditionalLocation(new Vector3(-246.484f, -2050.29f, 27.75544f), 272.5661f, 55f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.StandardScenario, RequiredPedGroup = "UnarmedSecurity" },
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>() 
                {
                    new SecurityConditionalLocation(new Vector3(-217.3633f, -2046.552f, 27.62042f), 140.1676f, 25f) { IsEmpty = false, RequiredPedGroup = "UnarmedSecurity" },
                    new SecurityConditionalLocation(new Vector3(-231.8785f, -2072.353f, 27.07749f), 215.4204f, 50f) {  },
                    new SecurityConditionalLocation(new Vector3(-217.1458f, -2034.899f, 27.0688f), 241.1897f, 50f) {  },
                    new SecurityConditionalLocation(new Vector3(-212.0767f, -2051.947f, 27.06881f), 238.1569f, 50f) {  },
                },
            },
            new Landmark(new Vector3(-3022.02f, 83.00665f, 10.64196f), 0f,"Pacific Bluffs Country Club","Members only")
            {
                OpenTime = 0,
                CloseTime = 24,
                ScannerFilePath = "01_specific_location\\0x0431FE2B.mp3",
                AssignedAssociationID = "MERRY",
                TypeName = "Country Club",
                MapIcon = 109,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new SecurityConditionalLocation(new Vector3(-3018.087f, 82.32686f, 11.6775f), 342.6718f, 75f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.Patrol | TaskRequirements.StandardScenario, RequiredPedGroup = "UnarmedSecurity" },
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new SecurityConditionalLocation(new Vector3(-2999.777f, 95.82683f, 11.26355f), 135.1711f, 55f),
                    new SecurityConditionalLocation(new Vector3(-3000.569f, 79.49763f, 11.2639f), 331.7827f, 55f),
                    new SecurityConditionalLocation(new Vector3(-3011.238f, 91.53729f, 11.26915f), 35.12966f, 35f) { IsEmpty = false,RequiredPedGroup = "UnarmedSecurity" },
                },
            },
            new Landmark(new Vector3(-2297.172f, 291.5488f, 169.6022f), 114.109f, "Kortz Center", "The center of enlightenment")
            {
                OpenTime = 0,
                CloseTime = 24,
                CanInteract = false,
                AssignedAssociationID = "SECURO",
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new SecurityConditionalLocation(new Vector3(-2315.212f, 271.6342f, 169.602f), 21.87641f, 55f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.Patrol| TaskRequirements.StandardScenario, RequiredPedGroup = "UnarmedSecurity" },
                    new SecurityConditionalLocation(new Vector3(-2289.921f, 272.7345f, 169.602f), 108.5267f, 55f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.Patrol| TaskRequirements.StandardScenario, RequiredPedGroup = "UnarmedSecurity" },
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new SecurityConditionalLocation(new Vector3(-2319.266f, 279.1095f, 169.1218f), 204.4258f, 55f),
                    new SecurityConditionalLocation(new Vector3(-2333.276f, 303.1464f, 169.1215f), 110.639f, 55f),
                    new SecurityConditionalLocation(new Vector3(-2321.64f, 294.0459f, 169.1222f), 23.37543f, 35f) { IsEmpty = false, RequiredPedGroup = "UnarmedSecurity" },
                },
            },
            new Landmark(new Vector3(-159.2471f, -153.9811f, 43.62119f), 160.782f,"Rockford Plaza","High end fashion meets drug addicts")
            { 
                OpenTime = 0, 
                CloseTime = 24,
                AssignedAssociationID = "BOBCAT",
                ActivateDistance = 100f,
                ActivateCells = 3,
                PossiblePedSpawns = new List<ConditionalLocation>() 
                {
                    new SecurityConditionalLocation(new Vector3(-163.7365f, -153.5678f, 43.6212f), 156.3999f, 65f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.StandardScenario, RequiredPedGroup = "UnarmedSecurity" },
                    new SecurityConditionalLocation(new Vector3(-152.4925f, -169.1785f, 43.6212f), 71.1814f, 65f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.Patrol | TaskRequirements.StandardScenario, RequiredPedGroup = "UnarmedSecurity" },
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>() 
                {
                    new SecurityConditionalLocation(new Vector3(-169.9185f, -153.0465f, 43.6212f), 70.66975f, 25f) { IsEmpty = false, RequiredPedGroup = "UnarmedSecurity" },
                    new SecurityConditionalLocation(new Vector3(-179.7954f, -149.7704f, 43.6212f), 71.97058f, 50f) {  },
                    new SecurityConditionalLocation(new Vector3(-187.0586f, -175.1785f, 43.62344f), 160.1578f, 50f) {  },
                    new SecurityConditionalLocation(new Vector3(-180.5878f, -177.1243f, 43.62268f), 164.2659f, 50f) {  },
                },
            },


            new Landmark(new Vector3(-1490.359f, 4981.503f, 63.35722f), 87.43992f, "Raton Canyon Ranger Station", "Not just a shack and a shitter!")
            {
                OpenTime = 0,
                CloseTime = 24,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(-1490.743f, 4984.296f, 63.20091f), 153.2589f, 55f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.Patrol| TaskRequirements.StandardScenario },
                    new LEConditionalLocation(new Vector3(-1493.811f, 4971.682f, 63.89991f), 264.6635f, 55f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.Patrol| TaskRequirements.StandardScenario },
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(-1491.661f, 4975.111f, 63.07887f), 43.19771f, 55f),
                    new LEConditionalLocation(new Vector3(-1507.015f, 4973.019f, 61.8642f), 55.27364f, 55f),
                },
            },
            new Landmark(new Vector3(895.3204f, -179.2765f, 74.70034f), 237.3235f,"Downtown Cab Co.","In transit since 1922") { OpenTime = 0,CloseTime = 24,CameraPosition = new Vector3(924.0997f, -175.3463f, 83.62648f), CameraDirection = new Vector3(-0.9416905f, 0.03393731f, -0.3347644f), CameraRotation = new Rotator(-19.55821f, -1.698846E-07f, 87.93603f) },
            new Landmark(new Vector3(2469.03f, 4955.278f, 45.11892f), 0f,"O'Neil Ranch","Need some meth?") { OpenTime = 0,CloseTime = 24, InteriorID = 31746, ScannerFilePath = "01_specific_location\\0x1E2AE79B.mp3" },
            new Landmark(new Vector3(-1045.065f, -230.3523f, 39.01435f), 294.2673f,"Lifeinvader","Get Stalked") {OpenTime = 0,CloseTime = 24, InteriorID = 3330 },
            new Landmark(new Vector3(2.69f, -667.01f, 16.13f), 0f,"Union Depository","") {IsTemporarilyClosed = true, OpenTime = 0,CloseTime = 24, InteriorID = 119042 },
            new Landmark(new Vector3(-34.58836f, 6287.814f, 31.38976f), 28.21855f,"Clucking Bell Farms","Know the saying about seeing how sausage is made?") {OpenTime = 0,CloseTime = 24, InteriorID = 28162, ScannerFilePath = "01_specific_location\\0x0D8D06A1.mp3" },
            new Landmark(new Vector3(718.2269f, -976.7165f, 24.71099f), 181.558f,"Darnell Bros. Garments","We make more than just garments") {IsTemporarilyClosed = true, OpenTime = 0,CloseTime = 24, InteriorID = 92674 },
            new Landmark(new Vector3(-598.1064f, -1610.67f, 26.01035f), 0f,"Rogers Salvage & Scrap","Taking your scrap since 1924") { IsTemporarilyClosed = true, OpenTime = 0,CloseTime = 24, InteriorID = -103 },
            new Landmark(new Vector3(562.8467f, 2741.614f, 42.86892f), 184.4624f,"Animal Ark","We have two of everything!") { IsTemporarilyClosed = true, OpenTime = 0,CloseTime = 24,ScannerFilePath = "01_specific_location\\0x147855FA.mp3" },
            new Landmark(new Vector3(-232.18f, -914.93f, 32.77f), 338.4021f,"Post Op Headquarters","No longer just mail") { IsTemporarilyClosed = true, OpenTime = 0,CloseTime = 24,ScannerFilePath = "01_specific_location\\0x02967EFD.mp3" },
            new Landmark(new Vector3(105.46f, 252.27f, 109.01f), 162.7602f,"Vinewood Star Tours","Stalk celebrities from the comfort of a bus") { IsTemporarilyClosed = true, OpenTime = 0,CloseTime = 24,ScannerFilePath = "01_specific_location\\0x04F6FA01.mp3" },
            new Landmark(new Vector3(-1261.62f, -347.33f, 37.22f), 53f,"Pump-N-Run Gym","Get ripped!") { IsTemporarilyClosed = true, OpenTime = 0,CloseTime = 24,ScannerFilePath = "01_specific_location\\0x06AD518B.mp3" },
            new Landmark(new Vector3(-247.62f, -1526.22f, 33.03f), 53f,"BJ Smith Rec Center","To win you have to annihilate everything in your path") { IsTemporarilyClosed = true, OpenTime = 0,CloseTime = 24,ScannerFilePath = "01_specific_location\\0x05AB836E.mp3" },
            new Landmark(new Vector3(3425.02f, 5174.67f, 8.13f), 0f,"El Gordo Lighthouse","Watch your step") { OpenTime = 0,CloseTime = 24,ScannerFilePath = "01_specific_location\\0x0761393B.mp3" },
            new Landmark(new Vector3(-1234.788f, -768.6721f, 17.95432f), 0f,"Prosperity Street Promenade","Come spend money like a rich person!") { OpenTime = 0,CloseTime = 24,ScannerFilePath = "01_specific_location\\0x077E335F.mp3" },
            new Landmark(new Vector3(847.05f, -1992.77f, 30.11f), 0f,"Pisswasser Factory","You're In, For A Good Time") { IsTemporarilyClosed = true, OpenTime = 0,CloseTime = 24,ScannerFilePath = "01_specific_location\\0x08AA4C64.mp3" },
            new Landmark(new Vector3(436.0983f, -645.8003f, 27.75121f), 100f,"Dashound Bus Center","Long journeys need short legs") { IsTemporarilyClosed = true, OpenTime = 0,CloseTime = 24,ScannerFilePath = "01_specific_location\\0x09A9666F.mp3" },
            new Landmark(new Vector3(129.8f, -1300.28f, 30.05f), 0f,"Vanilla Unicorn","Seeing is relieving") { OpenTime = 0,CloseTime = 24,ScannerFilePath = "01_specific_location\\0x0D1B649D.mp3" },
            new Landmark(new Vector3(712.96f, 1204.1f, 329.3f), 0f,"Vinewood Sign","Or was it Vinewoodland?") { OpenTime = 0,CloseTime = 24,ScannerFilePath = "01_specific_location\\0x0C57ACE0.mp3" },
            new Landmark(new Vector3(-200.26f, -1380.72f, 32.83f), 0f,"Glass Heroes Auto Repairs","We never crack under pressure") { IsTemporarilyClosed = true, OpenTime = 8,CloseTime = 17,ScannerFilePath = "01_specific_location\\0x105B95C3.mp3" },
            new Landmark(new Vector3(-2.34f, -1400.51f, 30.22f), 0f,"South LS Hand Car Wash","Let us give you a hand") { OpenTime = 0,CloseTime = 24,ScannerFilePath = "01_specific_location\\0x0CC361AF.mp3" },
            new Landmark(new Vector3(-698.5507f, 46.47984f, 44.03382f), 204.1632f, "The Epsilon Program", "Kifflom!"){ OpenTime = 0,CloseTime = 24 },


            new Landmark(new Vector3(-1465.816f, -1393.664f, 2.514868f), 116.3505f,"Vespucci Lifeguard Tower 1","")
            {
                OpenTime = 0,
                CloseTime = 24,
                AssignedAssociationID = "LSLFG",
                ActivateDistance = 100f,
                ActivateCells = 3,
                TypeName = "Lifeguard Tower",
                MapIcon = 767,
                IgnoreEntranceInteract = true,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new EMSConditionalLocation(new Vector3(-1465.816f, -1393.664f, 2.514868f), 116.3505f, 15f) { TaskRequirements = TaskRequirements.Guard,ForcedScenarios = new List<string>(){ "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_GUARD_STAND", "WORLD_HUMAN_BINOCULARS" }, },
                    new EMSConditionalLocation(new Vector3(-1466.981f, -1389.29f, 4.138116f), 102.6992f, 15f) { TaskRequirements = TaskRequirements.Guard,ForcedScenarios = new List<string>(){ "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_GUARD_STAND", "WORLD_HUMAN_BINOCULARS" }, },
                    new EMSConditionalLocation(new Vector3(-1470.935f, -1389.578f, 2.575922f), 81.81424f, 15f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.Patrol,ForcedScenarios = new List<string>(){ "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_GUARD_STAND", "WORLD_HUMAN_BINOCULARS" }, },
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new EMSConditionalLocation(new Vector3(-1460.457f, -1384.924f, 2.665404f), 200.9588f, 15f) { IsEmpty = true },
                },
            },

            new Landmark(new Vector3(-1435.112f, -1515.844f, 2.078523f), 103.4944f,"Vespucci Lifeguard Tower 2","")
            {
                OpenTime = 0,
                CloseTime = 24,
                AssignedAssociationID = "LSLFG",
                ActivateDistance = 100f,
                ActivateCells = 3,
                TypeName = "Lifeguard Tower",
                MapIcon = 767,
                IgnoreEntranceInteract = true,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new EMSConditionalLocation(new Vector3(-1435.112f, -1515.844f, 2.078523f), 103.4944f, 15f) { TaskRequirements = TaskRequirements.Guard,ForcedScenarios = new List<string>(){ "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_GUARD_STAND", "WORLD_HUMAN_BINOCULARS" }, },
                    new EMSConditionalLocation(new Vector3(-1435.203f, -1509.865f, 2.148914f), 104.9742f, 15f) { TaskRequirements = TaskRequirements.Guard,ForcedScenarios = new List<string>(){ "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_GUARD_STAND", "WORLD_HUMAN_BINOCULARS" }, },
                    new EMSConditionalLocation(new Vector3(-1431.285f, -1509.428f, 3.710496f), 119.8205f, 15f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.Patrol,ForcedScenarios = new List<string>(){ "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_GUARD_STAND", "WORLD_HUMAN_BINOCULARS" }, },
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new EMSConditionalLocation(new Vector3(-1424.351f, -1506.575f, 2.379463f), 205.7631f, 15f) { IsEmpty = true },
                },
            },
            new Landmark(new Vector3(-1376.505f, -1625.246f, 2.149354f), 110.8102f,"Vespucci Lifeguard Tower 3","")
            {
                OpenTime = 0,
                CloseTime = 24,
                AssignedAssociationID = "LSLFG",
                ActivateDistance = 100f,
                ActivateCells = 3,
                TypeName = "Lifeguard Tower",
                MapIcon = 767,
                IgnoreEntranceInteract = true,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new EMSConditionalLocation(new Vector3(-1376.505f, -1625.246f, 2.149354f), 110.8102f, 15f) { TaskRequirements = TaskRequirements.Guard,ForcedScenarios = new List<string>(){ "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_GUARD_STAND", "WORLD_HUMAN_BINOCULARS" }, },
                    new EMSConditionalLocation(new Vector3(-1373.504f, -1630.074f, 2.120056f), 119.2209f, 15f) { TaskRequirements = TaskRequirements.Guard,ForcedScenarios = new List<string>(){ "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_GUARD_STAND", "WORLD_HUMAN_BINOCULARS" }, },
                    new EMSConditionalLocation(new Vector3(-1372.801f, -1624.591f, 3.725401f), 108.1343f, 15f) { TaskRequirements = TaskRequirements.Guard| TaskRequirements.Patrol,ForcedScenarios = new List<string>(){ "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_GUARD_STAND", "WORLD_HUMAN_BINOCULARS" }, },
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new EMSConditionalLocation(new Vector3(-1366.556f, -1622.076f, 2.320448f), 21.83072f, 15f) { IsEmpty = true },
                },
            },
            new Landmark(new Vector3(-1294.84f, -1756.572f, 2.144996f), 139.373f,"Vespucci Lifeguard Tower 4","")
            {
                OpenTime = 0,
                CloseTime = 24,
                AssignedAssociationID = "LSLFG",
                ActivateDistance = 100f,
                ActivateCells = 3,
                TypeName = "Lifeguard Tower",
                MapIcon = 767,
                IgnoreEntranceInteract = true,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new EMSConditionalLocation(new Vector3(-1294.84f, -1756.572f, 2.144996f), 139.373f, 15f) { TaskRequirements = TaskRequirements.Guard,ForcedScenarios = new List<string>(){ "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_GUARD_STAND", "WORLD_HUMAN_BINOCULARS" }, },
                    new EMSConditionalLocation(new Vector3(-1290.009f, -1758.995f, 2.14688f), 156.2117f, 15f) { TaskRequirements = TaskRequirements.Guard,ForcedScenarios = new List<string>(){ "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_GUARD_STAND", "WORLD_HUMAN_BINOCULARS" }, },
                    new EMSConditionalLocation(new Vector3(-1290.237f, -1752.75f, 3.71381f), 129.6491f, 15f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.Patrol,ForcedScenarios = new List<string>(){ "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_GUARD_STAND", "WORLD_HUMAN_BINOCULARS" }, },
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new EMSConditionalLocation(new Vector3(-1283.365f, -1750.417f, 2.18823f), 33.92455f, 15f) { IsEmpty = true },
                },
            },

            new Landmark(new Vector3(-1518.476f, -1277.654f, 1.935055f), 96.05882f,"Vespucci Lifeguard Tower 5","")
            {
                OpenTime = 0,
                CloseTime = 24,
                AssignedAssociationID = "LSLFG",
                ActivateDistance = 100f,
                ActivateCells = 3,
                TypeName = "Lifeguard Tower",
                MapIcon = 767,
                IgnoreEntranceInteract = true,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new EMSConditionalLocation(new Vector3(-1518.476f, -1277.654f, 1.935055f), 96.05882f, 15f) { TaskRequirements = TaskRequirements.Guard,ForcedScenarios = new List<string>(){ "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_GUARD_STAND", "WORLD_HUMAN_BINOCULARS" }, },
                    new EMSConditionalLocation(new Vector3(-1523.732f, -1272.069f, 2.043999f), 94.24842f, 15f) { TaskRequirements = TaskRequirements.Guard,ForcedScenarios = new List<string>(){ "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_GUARD_STAND", "WORLD_HUMAN_BINOCULARS" }, },
                    new EMSConditionalLocation(new Vector3(-1518.378f, -1273.304f, 3.459242f), 84.8428f, 15f) { TaskRequirements = TaskRequirements.Guard| TaskRequirements.Patrol,ForcedScenarios = new List<string>(){ "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_GUARD_STAND", "WORLD_HUMAN_BINOCULARS" }, },
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new EMSConditionalLocation(new Vector3(-1511.761f, -1274.091f, 1.985983f), 4.970569f, 15f) { IsEmpty = true },
                },
            },
            new Landmark(new Vector3(-1557.982f, -1159.784f, 2.222632f), 127.8208f,"Vespucci Lifeguard Tower 6","")
            {
                OpenTime = 0,
                CloseTime = 24,
                AssignedAssociationID = "LSLFG",
                ActivateDistance = 100f,
                ActivateCells = 3,
                TypeName = "Lifeguard Tower",
                MapIcon = 767,
                IgnoreEntranceInteract = true,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new EMSConditionalLocation(new Vector3(-1557.982f, -1159.784f, 2.222632f), 127.8208f, 15f) { TaskRequirements = TaskRequirements.Guard,ForcedScenarios = new List<string>(){ "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_GUARD_STAND", "WORLD_HUMAN_BINOCULARS" }, },
                    new EMSConditionalLocation(new Vector3(-1564.454f, -1156.393f, 2.326566f), 130.6304f, 15f) { TaskRequirements = TaskRequirements.Guard,ForcedScenarios = new List<string>(){ "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_GUARD_STAND", "WORLD_HUMAN_BINOCULARS" }, },
                    new EMSConditionalLocation(new Vector3(-1560.814f, -1155.022f, 3.911213f), 125.2697f, 15f) { TaskRequirements = TaskRequirements.Guard,ForcedScenarios = new List<string>(){ "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_GUARD_STAND", "WORLD_HUMAN_BINOCULARS" }, },
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new EMSConditionalLocation(new Vector3(-1554.836f, -1151.656f, 2.336557f), 34.48763f, 15f) { IsEmpty = true },
                },
            },





            new Landmark(new Vector3(-1795.943f, -860.3762f, 7.491427f), 112.3975f,"Del Perro Lifeguard Tower 1","")
            {
                OpenTime = 0,
                CloseTime = 24,
                AssignedAssociationID = "LSLFG",
                ActivateDistance = 100f,
                ActivateCells = 3,
                TypeName = "Lifeguard Tower",
                MapIcon = 767,
                IgnoreEntranceInteract = true,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new EMSConditionalLocation(new Vector3(-1795.943f, -860.3762f, 7.491427f), 112.3975f, 15f) { TaskRequirements = TaskRequirements.Guard,ForcedScenarios = new List<string>(){ "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_GUARD_STAND", "WORLD_HUMAN_BINOCULARS" }, },
                    new EMSConditionalLocation(new Vector3(-1800.148f, -856.9856f, 7.514591f), 98.28096f, 15f) { TaskRequirements = TaskRequirements.Guard,ForcedScenarios = new List<string>(){ "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_GUARD_STAND", "WORLD_HUMAN_BINOCULARS" }, },
                    new EMSConditionalLocation(new Vector3(-1795.827f, -855.4499f, 9.2f), 106.6543f, 15f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.Patrol,ForcedScenarios = new List<string>(){ "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_GUARD_STAND", "WORLD_HUMAN_BINOCULARS" }, },
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new EMSConditionalLocation(new Vector3(-1789.967f, -854.0302f, 7.722801f), 205.7013f, 15f) { IsEmpty = true },
                },
            },

            new Landmark(new Vector3(-1899.692f, -714.9308f, 7.042809f), 119.4366f,"Del Perro Lifeguard Tower 2","")
            {
                OpenTime = 0,
                CloseTime = 24,
                AssignedAssociationID = "LSLFG",
                ActivateDistance = 100f,
                ActivateCells = 3,
                TypeName = "Lifeguard Tower",
                MapIcon = 767,
                IgnoreEntranceInteract = true,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new EMSConditionalLocation(new Vector3(-1899.692f, -714.9308f, 7.042809f), 119.4366f, 15f) { TaskRequirements = TaskRequirements.Guard,ForcedScenarios = new List<string>(){ "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_GUARD_STAND", "WORLD_HUMAN_BINOCULARS" }, },
                    new EMSConditionalLocation(new Vector3(-1908.717f, -712.2696f, 6.794119f), 120.6255f, 15f) { TaskRequirements = TaskRequirements.Guard,ForcedScenarios = new List<string>(){ "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_GUARD_STAND", "WORLD_HUMAN_BINOCULARS" }, },
                    new EMSConditionalLocation(new Vector3(-1903.039f, -711.0178f, 8.832595f), 109.4467f, 15f) { TaskRequirements = TaskRequirements.Guard| TaskRequirements.Patrol,ForcedScenarios = new List<string>(){ "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_GUARD_STAND", "WORLD_HUMAN_BINOCULARS" }, },
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new EMSConditionalLocation(new Vector3(-1897.314f, -707.5889f, 7.498323f), 35.62716f, 15f) { IsEmpty = true },
                },
            },
            new Landmark(new Vector3(-2004.317f, -562.2349f, 11.08815f), 120.6884f,"Del Perro Lifeguard Tower 3","")
            {
                OpenTime = 0,
                CloseTime = 24,
                AssignedAssociationID = "LSLFG",
                ActivateDistance = 100f,
                ActivateCells = 3,
                TypeName = "Lifeguard Tower",
                MapIcon = 767,
                IgnoreEntranceInteract = true,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new EMSConditionalLocation(new Vector3(-2004.317f, -562.2349f, 11.08815f), 120.6884f, 15f) { TaskRequirements = TaskRequirements.Guard,ForcedScenarios = new List<string>(){ "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_GUARD_STAND", "WORLD_HUMAN_BINOCULARS" }, },
                    new EMSConditionalLocation(new Vector3(-2011.097f, -559.4583f, 10.93443f), 120.7562f, 15f) { TaskRequirements = TaskRequirements.Guard,ForcedScenarios = new List<string>(){ "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_GUARD_STAND", "WORLD_HUMAN_BINOCULARS" }, },
                    new EMSConditionalLocation(new Vector3(-2006.434f, -557.8543f, 12.88624f), 121.3164f, 15f) { TaskRequirements = TaskRequirements.Guard,ForcedScenarios = new List<string>(){ "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_GUARD_STAND", "WORLD_HUMAN_BINOCULARS" }, },
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new EMSConditionalLocation(new Vector3(-2000.737f, -553.7861f, 11.47833f), 32.91222f, 15f) { IsEmpty = true },
                },
            },


            new Landmark(new Vector3(-1133.159f,-521.3668f,33.43165f),0f,"Richards Majestic Studios","")
            {
                RestrictedAreas = new RestrictedAreas()
                {
                    VanillaRestrictedAreas = new List<VanillaRestrictedArea>()
                    {
                        new VanillaRestrictedArea()
                        {
                            AngledRestrictedAreas = new List<AngledRestrictedArea>()
                            {
                                new AngledRestrictedArea(new Vector3(-1108.5497f, -570.87976f, 20f),new Vector3(-1187.8108f, -477.50366f, 50f),162f),
                                new AngledRestrictedArea(new Vector3(-1201.3776f, -485.96732f, 20f),new Vector3(-1215.7959f, -464.82806f, 50f),124f),
                                new AngledRestrictedArea(new Vector3(-985.6311f, -525.42334f, 20f),new Vector3(-1013.3932f, -475.20575f, 50f),55f),
                                new AngledRestrictedArea(new Vector3(-1055.8492f, -477.8226f, 20f),new Vector3(-1073.3325f, -498.717f, 50f),142f),
                            },
                        },
                    },
                },
            },
            new Landmark(new Vector3(3542.128f,3731.31f,36.44195f),0f,"Humane Labs","")
            {
                RestrictedAreas = new RestrictedAreas()
                {
                    VanillaRestrictedAreas = new List<VanillaRestrictedArea>()
                    {
                        new VanillaRestrictedArea()
                        {
                            AngledRestrictedAreas = new List<AngledRestrictedArea>()
                            {
                                new AngledRestrictedArea(new Vector3(3411.002f, 3663.1846f, 20f),new Vector3(3615.583f, 3626.1936f, 40f),45.75f),
                                new AngledRestrictedArea(new Vector3(3426.66f, 3733.078f, 20f),new Vector3(3643.8008f, 3694.3618f, 40f),99f),
                                new AngledRestrictedArea(new Vector3(3446.0364f, 3795.6882f, 20f),new Vector3(3650.9143f, 3766.1516f,  40f),81.5f),
                            },
                        },
                    },
                },
            },
        };
    }
    private void DefaultConfig_PawnShops()
    {
        PawnShops = new List<PawnShop>() {
            new PawnShop(new Vector3(412.5109f, 314.9815f, 103.1327f), 207.4105f, "F.T. Pawn", "","PawnShopMenuGeneric") { MinPriceRefreshHours = 12, MaxPriceRefreshHours = 24,MinRestockHours = 12, MaxRestockHours = 24,},
            new PawnShop(new Vector3(182.735f, -1318.845f, 29.31752f), 246.2635f, "Pawn & Jewelery", "","PawnShopMenuGeneric"){ MinPriceRefreshHours = 12, MaxPriceRefreshHours = 24,MinRestockHours = 12, MaxRestockHours = 24,},
        };
    }
    private void DefaultConfig_HardwareStores()
    {
        HardwareStores = new List<HardwareStore>() {
            new HardwareStore(new Vector3(2747.406f, 3473.213f, 55.67021f), 249.8152f, "You Tool", "Show your wife who the family tool is","ToolMenu") {BannerImagePath = "stores\\youtool.png", CameraPosition = new Vector3(2780.472f, 3473.511f, 73.06239f), CameraDirection = new Vector3(-0.9778581f, -0.02382228f, -0.2079087f), CameraRotation = new Rotator(-11.99983f, 0f, 91.39555f) },
            new HardwareStore(new Vector3(339.4021f, -776.9934f, 29.2665f), 68.51967f, "Krapea", "We fake it, you make it","ToolMenu"){ BannerImagePath = "stores\\krapea.png" },
            new HardwareStore(new Vector3(-10.88182f, 6499.395f, 31.50508f), 44.30542f, "Bay Hardware", "","ToolMenu"),
            new HardwareStore(new Vector3(-3153.697f, 1053.398f, 20.88735f), 338.4756f, "Hardware", "","ToolMenu"),
            new HardwareStore(new Vector3(343.2759f, -1297.948f, 32.5097f), 164.2121f, "Bert's Tool Supply", "","ToolMenu"),

        };
    }
    private void DefaultConfig_SportingGoodsStores()
    {
        SportingGoodsStores = new List<SportingGoodsStore>() {
            new SportingGoodsStore(new Vector3(-945.9442f, -1191.532f, 4.956469f), 168.678f, "Vespucci Sports", "Our rent is so high, we must have quality items!","VespucciSportsMenu") {
                BannerImagePath = "stores\\vespuccisports.png",
                VehiclePreviewLocation = new SpawnPlace(new Vector3(-956.9415f, -1185.061f, 4.22743f), 25.39798f),
                VehiclePreviewCameraPosition = new Vector3(-962.8305f, -1185.232f, 5.732603f), 
                VehiclePreviewCameraDirection = new Vector3(0.9816672f, 0.008348566f, -0.1904202f), 
                VehiclePreviewCameraRotation = new Rotator(-10.97731f, -3.336064E-06f, -89.51274f),
                VehicleDeliveryLocations = new List<SpawnPlace>()
                {
                    new SpawnPlace(new Vector3(-911.3941f, -1165.663f, 4.903689f), 31.58475f),
                    new SpawnPlace(new Vector3(-913.2894f, -1167.775f, 4.873601f), 209.063f),
                    new SpawnPlace(new Vector3(-914.4749f, -1160.993f, 4.809005f), 19.54775f),
                    new SpawnPlace(new Vector3(-918.4772f, -1160.179f, 4.782866f), 206.1096f),
                    new SpawnPlace(new Vector3(-921.7528f, -1158.282f, 4.754968f), 283.2098f),
                }
            },

            new SportingGoodsStore(new Vector3(-500.4163f, -19.29782f, 45.12749f), 38.92741f, "Bourgeois Bicycles", "Monocles Optional!","BourgeoisBicyclesMenu") {
                CameraPosition = new Vector3(-501.9579f, -10.62078f, 49.61808f), 
                CameraDirection = new Vector3(0.1908819f, -0.929588f, -0.3153257f), 
                CameraRotation = new Rotator(-18.38048f, 1.574425E-06f, -168.3962f),

                VehiclePreviewLocation = new SpawnPlace(new Vector3(-499.8291f, -14.4307f, 44.84824f), 262.8628f),
                VehiclePreviewCameraPosition = new Vector3(-497.8142f, -9.641495f, 47.0248f),
                VehiclePreviewCameraDirection = new Vector3(-0.3421925f, -0.8653485f, -0.3661644f),
                VehiclePreviewCameraRotation = new Rotator(-21.47926f, -4.587467E-07f, 158.4242f),

                VehicleDeliveryLocations = new List<SpawnPlace>() { 
                    new SpawnPlace(new Vector3(-528.1365f, -34.8563f, 44.51523f), 176.1417f),
                    new SpawnPlace(new Vector3(-524.8844f, -35.83997f, 44.517f), 176.0726f),
                    new SpawnPlace(new Vector3(-527.8739f, -28.25564f, 44.51612f), 36.77939f),
                    new SpawnPlace(new Vector3(-535.4172f, -32.23565f, 44.35883f), 152.8177f),
                    new SpawnPlace(new Vector3(-543.1857f, -36.58912f, 43.24746f), 42.25189f), }
            },
        };
    }
    private void DefaultConfig_HeadShops()
    {
        HeadShops = new List<HeadShop>() {
            new HeadShop(new Vector3(-1191.582f, -1197.779f, 7.617113f), 146.801f, "Pipe Dreams", "","HeadShopMenu"),
            new HeadShop(new Vector3(65.60603f, -137.4155f, 55.11251f), 214.0327f, "Pipe Dreams", "","HeadShopMenu"),
            new HeadShop(new Vector3(278.8327f, -1027.653f, 29.21136f), 184.1326f, "Pipe Down Cigars", "","HeadShopMenu"){ BannerImagePath = "stores\\pipedown.png" },
            new HeadShop(new Vector3(-1154.942f, -1373.176f, 5.061489f), 305.589f, "Amnesiac Smoke Shop", "","WeedAndCigMenu"),
            new HeadShop(new Vector3(-269.2553f, 243.7069f, 90.40055f), 1.693904f, "Pipe Down", "","HeadShopMenu"){ BannerImagePath = "stores\\pipedown.png" },

        };
    }
    private void DefaultConfig_Dispensaries()
    {
        Dispensaries = new List<Dispensary>() {
            new Dispensary(new Vector3(-1161.365f, -1427.646f, 4.623186f), 31.50553f, "Doctor Kush", "","WeedMenu"),
            new Dispensary(new Vector3(-502.4879f, 32.92564f, 44.71512f), 179.9803f, "Serenity Wellness", "","WeedMenu"),
            new Dispensary(new Vector3(169.5722f, -222.869f, 54.23643f), 342.0811f, "High Time", "","WeedMenu"),
            new Dispensary(new Vector3(-1381.142f, -941.0327f, 10.17387f), 126.4558f,"Seagrass Herbals", "Seagrass Herbals","WeedMenu"),
            new Dispensary(new Vector3(1175.02f, -437.1972f, 66.93162f), 259.358f, "Mile High Organics", "","WeedMenu"),
        };
    }
    private void DefaultConfig_Pharmacies()
    {
        Pharmacies = new List<Pharmacy>()
        {
            new Pharmacy(new Vector3(114.2954f, -4.942202f, 67.82149f), 195.4308f,  "Pop's Pills", "","PharmacyMenu") { BannerImagePath = "stores\\popspills.png", },
            new Pharmacy(new Vector3(68.94705f, -1570.043f, 29.59777f), 50.85398f, "Dollar Pills", "","PharmacyMenu") {ScannerFilePath = "01_specific_location\\0x017D2BE2.mp3" },
            new Pharmacy(new Vector3(326.7227f, -1074.448f, 29.47332f), 359.3641f, "Family Pharmacy", "","PharmacyMenu"),
            new Pharmacy(new Vector3(805.143f, -1063.586f, 28.42115f), 90.00111f, "Meltz's Pharmacy", "","PharmacyMenu"),
            new Pharmacy(new Vector3(1225.14f, -391.3563f, 68.68563f), 28.81875f, "Pharmacy", "","PharmacyMenu"),
            new Pharmacy(new Vector3(151.0329f, 6647.418f, 31.594f), 135.0961f, "Pop's Pills", "","PharmacyMenu"){ BannerImagePath = "stores\\popspills.png", },
            new Pharmacy(new Vector3(-172.4879f, 6381.202f, 31.47279f), 222.4285f, "Bay Side Drugs", "","PharmacyMenu"),
            new Pharmacy(new Vector3(214.0241f, -1835.08f, 27.54375f), 318.7183f, "Family Pharmacy", "","PharmacyMenu"),
            new Pharmacy(new Vector3(591.2585f, 2744.49f, 42.0425f), 184.8661f, "Dollar Pills", "","PharmacyMenu") { ScannerFilePath = "01_specific_location\\0x19E069DE.mp3" },
            new Pharmacy(new Vector3(825.8448f, -97.80138f, 80.59971f), 321.7153f, "Pharmacy", "","PharmacyMenu"),
            new Pharmacy(new Vector3(84.03228f, -810.2528f, 31.41642f), 350.9262f, "Family Pharmacy", "","PharmacyMenu"),
        };
    }
    private void DefaultConfig_Restaurants()
    {
        Restaurants = new List<Restaurant>()
        {
            //Generic and Sit Down
            new Restaurant(new Vector3(-2188.348f,-408.6776f, 13.17961f), 226.5375f, "Pipeline Inn", "Take a look, just take a look...","FancyFishMenu", FoodType.Seafood) { CloseTime = 24,OpenTime = 0 },
            new Restaurant(new Vector3(980.0559f, -1396.695f, 31.68536f), 219.9515f, "La Taqueria", "Autentica Comida Mexicana","MexicanMenu", FoodType.Mexican),
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
            new Restaurant(new Vector3(-1335.517f, -660.6063f, 26.51026f), 212.5534f, "The Fish Net", "Straight from the polluted Los Santos River","FancyFishMenu", FoodType.Seafood) { ScannerFilePath = "01_specific_location\\0x02249C78.mp3"},
            new Restaurant(new Vector3(95.79929f, -1682.817f, 29.25364f), 138.4551f, "Yum Fish", "We love fish pie! and our fishermen love to fillet!","FancyFishMenu", FoodType.Seafood),
            new Restaurant(new Vector3(502.6628f, 113.1527f, 96.62571f), 164.3104f, "Jazz Desserts", "Come get your soul food","FancyGenericMenu", FoodType.American),
            new Restaurant(new Vector3(16.29523f, -166.5288f, 55.82795f), 341.2336f, "The Fish Net", "What a catch","FancyFishMenu", FoodType.Seafood) { ScannerFilePath = "01_specific_location\\0x02249C78.mp3"},
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
            new Restaurant(new Vector3(-1249.812f, -296.1564f, 37.35062f), 206.9039f, "Bite!", "Have It Our Way","BiteMenu", FoodType.Sandwiches) {OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\bite.png" },
            new Restaurant(new Vector3(-1539.498f, -427.3804f, 35.59194f), 233.1319f, "Bite!", "Have It Our Way","BiteMenu", FoodType.Sandwiches) {OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\bite.png" },
            new Restaurant(new Vector3(229.5384f, -22.3363f, 74.98735f), 160.0777f, "Bite!", "Have It Our Way","BiteMenu", FoodType.Sandwiches) {OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\bite.png" },
            new Restaurant(new Vector3(-240.7315f, -346.1899f, 30.02782f), 47.8591f, "Bite!", "Have It Our Way","BiteMenu", FoodType.Sandwiches) {OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\bite.png" },
            new Restaurant(new Vector3(-263.1924f, -904.2821f, 32.3108f), 338.4021f, "Bite!", "Have It Our Way","BiteMenu", FoodType.Sandwiches) {OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\bite.png" },
            new Restaurant(new Vector3(385.958f, -1010.523f, 29.41794f), 271.5127f, "Bite!", "Have It Our Way","BiteMenu", FoodType.Sandwiches) {OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\bite.png" },
            new Restaurant(new Vector3(1139.359f, -463.8952f, 66.85857f), 261.1642f, "Bite!", "Have It Our Way","BiteMenu", FoodType.Sandwiches) {OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\bite.png" },
            new Restaurant(new Vector3(100.5837f, 209.4958f, 107.9911f), 342.4262f, "The Pink Sandwich", "The Pink Sandwich","SandwichMenu", FoodType.Sandwiches) {OpenTime = 0, CloseTime = 24 },
            //Asian
            new Restaurant(new Vector3(-798.0056f, -632.0029f, 29.02696f), 169.2606f, "S.Ho", "Become a real S. HO","NoodleMenu", FoodType.Korean) {BannerImagePath ="stores\\sho.png", ScannerFilePath ="01_specific_location\\0x1ABB2DE0.mp3" },
            new Restaurant(new Vector3(-638.5052f, -1249.646f, 11.81044f), 176.4081f, "S.Ho", "Become a real S. HO","NoodleMenu", FoodType.Korean) {BannerImagePath ="stores\\sho.png", ScannerFilePath = "01_specific_location\\0x1ABB2DE0.mp3" },
            new Restaurant(new Vector3(-700.9553f, -884.5563f, 23.79126f), 41.62328f, "S.Ho", "Become a real S. HO","NoodleMenu", FoodType.Korean) {BannerImagePath ="stores\\sho.png",ScannerFilePath = "01_specific_location\\0x1ABB2DE0.mp3" },
            new Restaurant(new Vector3(-1229.61f, -285.7077f, 37.73843f), 205.5755f, "Noodle Exchange", "You Won't Want To Share!","NoodleMenu", FoodType.Korean) { BannerImagePath = "stores\\noodleexchange.png" },
            new Restaurant(new Vector3(-1199.53f, -1162.439f, 7.696731f), 107.0593f, "Noodle Exchange", "You Won't Want To Share!","NoodleMenu", FoodType.Korean){ BannerImagePath = "stores\\noodleexchange.png" },
            new Restaurant(new Vector3(272.8409f, -965.4847f, 29.31605f), 27.34526f, "Noodle Exchange", "You Won't Want To Share!","NoodleMenu", FoodType.Korean){ BannerImagePath = "stores\\noodleexchange.png" },
            new Restaurant(new Vector3(-655.6034f, -880.3672f, 24.67554f), 265.7094f, "Wook Noodle House", "Way better than pancakes","NoodleMenu", FoodType.Korean),
            new Restaurant(new Vector3(-680.4404f, -945.5441f, 20.93157f), 180.6927f, "Wook Noodle House", "Way better than pancakes","NoodleMenu", FoodType.Korean),
            new Restaurant(new Vector3(-654.8373f, -885.7593f, 24.67703f), 273.4168f, "Park Jung Restaurant", "No parking available","GenericMenu", FoodType.Korean),
            new Restaurant(new Vector3(-163.0659f, -1440.267f, 31.42698f), 55.5593f, "Wok It Off", "Life got you down? Wok It Off!","GenericMenu", FoodType.Korean),
            new Restaurant(new Vector3(1894.635f, 3715.372f, 32.74969f), 119.2431f, "Chinese Food", "As generic as it gets","GenericMenu", FoodType.Chinese),
            //Italian
            new Restaurant(new Vector3(-1182.659f, -1410.577f, 4.499721f), 215.9843f, "Al Dente's", "Just like mama never used to make it","AlDentesMenu", FoodType.Italian | FoodType.Pizza) {BannerImagePath = "stores\\aldentes.png" },
            new Restaurant(new Vector3(-213.0357f, -40.15178f, 50.04371f), 157.8173f, "Al Dente's", "Just like mama never used to make it","AlDentesMenu", FoodType.Italian | FoodType.Pizza) {BannerImagePath = "stores\\aldentes.png" },
            new Restaurant(new Vector3(-1393.635f, -919.5128f, 11.24511f), 89.35195f, "Al Dente's", "Just like mama never used to make it","AlDentesMenu", FoodType.Italian | FoodType.Pizza) {BannerImagePath = "stores\\aldentes.png" },
            new Restaurant(new Vector3(215.2669f, -17.14256f, 74.98737f), 159.7144f, "Pizza This...", "Get stuffed","PizzaThisMenu", FoodType.Italian | FoodType.Pizza) {MapIcon = 889,TypeName ="Pizza",BannerImagePath = "stores\\pizzathis.png" },
            new Restaurant(new Vector3(538.3118f, 101.4798f, 96.52515f), 159.4801f, "Pizza This...", "Get stuffed","PizzaThisMenu", FoodType.Italian | FoodType.Pizza) {MapIcon = 889,TypeName ="Pizza",BannerImagePath = "stores\\pizzathis.png" },
            new Restaurant(new Vector3(287.5003f, -964.0207f, 29.41863f), 357.0406f, "Pizza This...", "Get stuffed","PizzaThisMenu", FoodType.Italian | FoodType.Pizza) {MapIcon = 889,TypeName ="Pizza",BannerImagePath = "stores\\pizzathis.png" },
            new Restaurant(new Vector3(-1529.252f, -908.6689f, 10.16963f), 137.3273f, "Pizza This...", "Get stuffed","PizzaThisMenu", FoodType.Italian | FoodType.Pizza) {MapIcon = 889,TypeName ="Pizza",BannerImagePath = "stores\\pizzathis.png" },
            new Restaurant(new Vector3(443.7377f, 135.1464f, 100.0275f), 161.2897f, "Guidos Takeout 24/7", "For the night owls","PizzaMenu", FoodType.Italian | FoodType.Pizza) {MapIcon = 889,TypeName ="Pizza",OpenTime = 0, CloseTime = 24},
            new Restaurant(new Vector3(-1320.907f, -1318.505f, 4.784881f), 106.5257f, "Pebble Dash Pizza", "Overpriced shitty pizza, but by the beach","PizzaMenu", FoodType.Italian | FoodType.Pizza) { MapIcon = 889,TypeName ="Pizza", },
            new Restaurant(new Vector3(-1334.007f, -1282.623f, 4.835985f), 115.3464f, "Slice N Dice Pizza","Slice UP!","PizzaMenu", FoodType.Italian | FoodType.Pizza | FoodType.FastFood) { MapIcon = 889,TypeName ="Pizza", },
            new Restaurant(new Vector3(-1296.815f, -1387.3f, 4.544102f), 112.4694f, "Sharkies Bites","Take A Bite Today!","PizzaMenu", FoodType.Italian | FoodType.Pizza | FoodType.FastFood){ IsWalkup = true },
            new Restaurant(new Vector3(-1654.415f, -1037.579f, 13.15282f), 48.0925f, "Sharkies Bites","Take A Bite Today!","PizzaMenu", FoodType.Italian | FoodType.Pizza | FoodType.FastFood){ IsWalkup = true },
            new Restaurant(new Vector3(-1814.037f, -1213.353f, 13.01751f), 43.14338f, "Sharkies Bites","Take A Bite Today!","PizzaMenu", FoodType.Italian | FoodType.Pizza | FoodType.FastFood){ IsWalkup = true },
            new Restaurant(new Vector3(-1342.607f, -872.2929f, 16.87064f), 312.7196f, "Giovanni's Italian", "There are no pocket monsters here","PizzaMenu", FoodType.Italian | FoodType.Pizza),
            //Burger
            new Restaurant(new Vector3(-1535.117f, -454.0615f, 35.92439f), 319.1095f, "Wigwam", "No need for reservations","WigwamMenu", FoodType.Burger | FoodType.FastFood) { OpenTime = 5, CloseTime = 23,BannerImagePath = "stores\\wigwam.png"},
            new Restaurant(new Vector3(-860.8414f, -1140.393f, 7.39234f), 171.7175f, "Wigwam", "No need for reservations","WigwamMenu", FoodType.Burger | FoodType.FastFood) { OpenTime = 5, CloseTime = 23,BannerImagePath = "stores\\wigwam.png",ScannerFilePath = "01_specific_location\\0x15151AB5.mp3" },
            new Restaurant(new Vector3(-1540.86f, -454.866f, 40.51906f), 321.1314f, "Up-N-Atom", "Never Frozen, Often Microwaved","UpNAtomMenu", FoodType.Burger | FoodType.FastFood) {OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\upnatom.png"},
            new Restaurant(new Vector3(81.31124f, 275.1125f, 110.2102f), 162.7602f, "Up-N-Atom", "Never Frozen, Often Microwaved","UpNAtomMenu", FoodType.Burger | FoodType.FastFood) {OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\upnatom.png"},
            new Restaurant(new Vector3(1591.054f, 6451.071f, 25.31714f), 158.0088f, "Up-N-Atom Diner", "Never Frozen, Often Microwaved","UpNAtomMenu", FoodType.Burger | FoodType.FastFood) {OpenTime = 0, CloseTime = 24,BannerImagePath = "stores\\upnatom.png", ScannerFilePath = "01_specific_location\\0x035776E6.mp3"},
            new Restaurant(new Vector3(-1183.638f, -884.3126f, 13.79987f), 303.1936f, "Burger Shot", "Kill your hunger! It's bleedin' tasty","BurgerShotMenu", FoodType.Burger | FoodType.FastFood) { OpenTime = 5, CloseTime = 23,BannerImagePath = "stores\\burgershot.png", ScannerFilePath = "01_specific_location\\0x14C89994.mp3" },
            new Restaurant(new Vector3(-1687.276f, -1091.789f, 13.15192f), 132.5498f, "Burger Shot", "Kill your hunger! It's bleedin' tasty","BurgerShotMenu", FoodType.Burger | FoodType.FastFood) { OpenTime = 5, CloseTime = 23,BannerImagePath = "stores\\burgershot.png", },
            new Restaurant(new Vector3(1241.453f, -366.7538f, 69.08221f), 164.3345f, "Horny's Burgers", "The beef with the bone!","HornysBurgersMenu", FoodType.Burger | FoodType.FastFood){ OpenTime = 5, CloseTime = 23,BannerImagePath = "stores\\hornys.png" },
            new Restaurant(new Vector3(-512.6821f, -683.3517f, 33.18555f), 3.720508f, "Snr. Buns", "Don't be a chump, taste our rump!","SnrBunsMenu", FoodType.Burger | FoodType.FastFood) {ScannerFilePath = "01_specific_location\\0x19532EA2.mp3" },
            new Restaurant(new Vector3(-526.9481f, -679.6907f, 33.67113f), 35.17997f, "Snr. Muffin", "Don't be a chump, taste our rump!","SnrBunsMenu", FoodType.Burger | FoodType.FastFood),//??? 
            new Restaurant(new Vector3(125.9558f, -1537.896f, 29.1772f), 142.693f, "La Vaca Loca", "Whats wrong with a few mad cows?","LaVacaLocaMenu", FoodType.Burger) {OpenTime = 5, CloseTime = 23, BannerImagePath = "stores\\vacaloca.png", CameraPosition = new Vector3(137.813f, -1561.211f, 37.43506f), CameraDirection = new Vector3(-0.1290266f, 0.9696004f, -0.2079113f), CameraRotation = new Rotator(-11.99998f, -2.182118E-07f, 7.579925f) },
            new Restaurant(new Vector3(-241.8231f, 279.747f, 92.04223f), 177.4421f, "Spitroasters Meathouse", "Come One, Come All","BeefyBillsMenu", FoodType.Burger),

            //Coffee
            new Restaurant(new Vector3(-238.903f, -777.356f, 34.09171f), 71.47642f, "Cafe Redemption", "Who needs head when we have the whole boar?","CoffeeMenu", FoodType.Coffee),
            new Restaurant(new Vector3(370.4181f, -1027.565f, 29.33361f), 184.4234f, "Ground & Pound Cafe", "We know how to take a pounding","CoffeeMenu", FoodType.Coffee),
            new Restaurant(new Vector3(55.28403f, -799.5469f, 31.58599f), 341.3315f, "Ground & Pound Cafe", "We know how to take a pounding","CoffeeMenu", FoodType.Coffee),
            new Restaurant(new Vector3(-1283.567f, -1130.118f, 6.795891f), 143.1178f, "Bean Machine Coffee", "Taking over the world one Gunkaccino at a time","BeanMachineMenu", FoodType.Coffee | FoodType.Bagels | FoodType.Donut) { BannerImagePath = "stores\\beanmachine.png" },
            new Restaurant(new Vector3(-1549.39f, -435.5105f, 35.88667f), 234.6563f, "Bean Machine Coffee", "Taking over the world one Gunkaccino at a time","BeanMachineMenu", FoodType.Coffee | FoodType.Bagels | FoodType.Donut) { BannerImagePath = "stores\\beanmachine.png" },
            new Restaurant(new Vector3(-835.4522f, -610.4766f, 29.02697f), 142.0655f, "Bean Machine Coffee", "Taking over the world one Gunkaccino at a time","BeanMachineMenu", FoodType.Coffee | FoodType.Bagels | FoodType.Donut) { BannerImagePath = "stores\\beanmachine.png" },
            new Restaurant(new Vector3(-602.2112f, -1105.766f, 22.32427f), 273.8795f, "Bean Machine Coffee", "Taking over the world one Gunkaccino at a time","BeanMachineMenu", FoodType.Coffee | FoodType.Bagels | FoodType.Donut) { BannerImagePath = "stores\\beanmachine.png" },
            new Restaurant(new Vector3(-659.5289f, -814.0433f, 24.53778f), 232.0023f, "Bean Machine Coffee", "Taking over the world one Gunkaccino at a time","BeanMachineMenu", FoodType.Coffee | FoodType.Bagels | FoodType.Donut) {BannerImagePath = "stores\\beanmachine.png"},
            new Restaurant(new Vector3(-687.0801f, -855.6792f, 23.89398f), 0.2374549f, "Bean Machine Coffee", "Taking over the world one Gunkaccino at a time","BeanMachineMenu", FoodType.Coffee | FoodType.Bagels | FoodType.Donut) {BannerImagePath = "stores\\beanmachine.png" },
            new Restaurant(new Vector3(-1345.296f, -609.976f, 28.61888f), 304.4266f, "Bean Machine Coffee", "Taking over the world one Gunkaccino at a time","BeanMachineMenu", FoodType.Coffee | FoodType.Bagels | FoodType.Donut) { BannerImagePath = "stores\\beanmachine.png" },
            new Restaurant(new Vector3(-270.3488f, -977.3488f, 31.21763f), 164.5747f, "Bean Machine Coffee", "Taking over the world one Gunkaccino at a time","BeanMachineMenu", FoodType.Coffee | FoodType.Bagels | FoodType.Donut) { BannerImagePath = "stores\\beanmachine.png" },
            new Restaurant(new Vector3(127.9072f, -1028.778f, 29.43674f), 336.4557f, "Bean Machine Coffee", "Taking over the world one Gunkaccino at a time","BeanMachineMenu", FoodType.Coffee | FoodType.Bagels | FoodType.Donut) { BannerImagePath = "stores\\beanmachine.png" },
            new Restaurant(new Vector3(-627.6302f, 239.2284f, 81.88939f), 86.57707f, "Bean Machine Coffee", "Taking over the world one Gunkaccino at a time","BeanMachineMenu", FoodType.Coffee | FoodType.Bagels | FoodType.Donut) { BannerImagePath = "stores\\beanmachine.png" },
            new Restaurant(new Vector3(280.5522f, -964.0756f, 29.41863f), 357.4615f, "Bean Machine Coffee", "Taking over the world one Gunkaccino at a time","BeanMachineMenu", FoodType.Coffee | FoodType.Bagels | FoodType.Donut) { BannerImagePath = "stores\\beanmachine.png" },
            new Restaurant(new Vector3(-1704.005f, -1101.831f, 13.15248f), 320.0907f, "Bean Machine Coffee", "Taking over the world one Gunkaccino at a time","BeanMachineMenu", FoodType.Coffee | FoodType.Bagels | FoodType.Donut) { BannerImagePath = "stores\\beanmachine.png" },
            new Restaurant(new Vector3(-1206.975f, -1135.029f, 7.693257f), 109.1408f, "Cool Beans Coffee Co", "Come flick our beans","CoffeeMenu", FoodType.Coffee){ BannerImagePath = "stores\\coolbeans.png" },
            new Restaurant(new Vector3(-1278.833f, -876.438f, 11.9303f), 123.2498f, "Cool Beans Coffee Co", "Come flick our beans","CoffeeMenu", FoodType.Coffee){ BannerImagePath = "stores\\coolbeans.png" },
            new Restaurant(new Vector3(1169.704f, -403.1992f, 72.24859f), 344.0863f, "Cool Beans Coffee Co", "Come flick our beans","CoffeeMenu", FoodType.Coffee){ BannerImagePath = "stores\\coolbeans.png" },
            new Restaurant(new Vector3(265.4628f, -981.3839f, 29.36569f), 72.07395f, "Cool Beans Coffee Co", "Come flick our beans","CoffeeMenu", FoodType.Coffee){ BannerImagePath = "stores\\coolbeans.png" },
            new Restaurant(new Vector3(-1108.847f, -1355.264f, 5.035112f), 206.1676f, "Crucial Fix Coffee", "Get your fix","CoffeeMenu", FoodType.Coffee),
            new Restaurant(new Vector3(189.0311f, -231.234f, 54.07472f), 340.4597f, "Crucial Fix Coffee", "Get your fix","CoffeeMenu", FoodType.Coffee),
            new Restaurant(new Vector3(273.174f, -833.0611f, 29.41237f), 185.6476f, "Crucial Fix Coffee", "Get your fix","CoffeeMenu", FoodType.Coffee),
            new Restaurant(new Vector3(-1109.055f, -1355.237f, 5.034323f), 208.2667f, "Crucial Fix Coffee", "Get your fix","CoffeeMenu", FoodType.Coffee),
            new Restaurant(new Vector3(-576.6631f, -677.8674f, 32.36259f), 306.9058f, "Hit-N-Run Coffee", "Vehicular assault discouraged","CoffeeMenu", FoodType.Coffee),
            new Restaurant(new Vector3(-1253.337f, -296.6488f, 37.31522f), 206.5786f, "{java.update();}", "Real coffee made by fake programmers","CoffeeMenu", FoodType.Coffee) { IsOnMPMap = false },
            new Restaurant(new Vector3(-509.1889f, -22.9895f, 45.60899f), 354.7263f, "Little Teapot", "The finest organic & free range teas exploited from the third world","CoffeeMenu", FoodType.Coffee) {ScannerFilePath = "01_specific_location\\0x1980DD57.mp3" },
            //Mexican
            new Restaurant(new Vector3(10.96682f, -1605.874f, 29.3931f), 229.8729f, "The Taco Farmer", "Open All Hours!","TacoFarmerMenu", FoodType.Mexican) { OpenTime = 0, CloseTime = 24, BannerImagePath = "stores\\tacofarmer.png" },
            new Restaurant(new Vector3(649.765f, 2728.621f, 41.9959f), 276.2882f, "The Taco Farmer", "Open All Hours!","TacoFarmerMenu", FoodType.Mexican) { OpenTime = 0, CloseTime = 24, BannerImagePath = "stores\\tacofarmer.png" },
            new Restaurant(new Vector3(-1168.281f, -1267.279f, 6.198249f), 111.9682f, "Taco Libre", "Autentica Comida Mexicana","MexicanMenu", FoodType.Mexican),
            new Restaurant(new Vector3(-657.5089f, -679.4656f, 31.46727f), 317.9819f, "Taco Bomb", "The taste explosion","TacoBombMenu", FoodType.Mexican) {BannerImagePath = "stores\\tacobomb.png" },
            new Restaurant(new Vector3(-1196.981f, -791.5534f, 16.40427f), 134.7115f, "Taco Bomb", "The taste explosion","TacoBombMenu", FoodType.Mexican) {BannerImagePath = "stores\\tacobomb.png" },
            new Restaurant(new Vector3(-1553.112f, -439.9938f, 40.51905f), 228.7506f, "Taco Bomb", "The taste explosion","TacoBombMenu", FoodType.Mexican) {BannerImagePath = "stores\\tacobomb.png" },
            new Restaurant(new Vector3(99.21678f, -1419.307f, 29.42156f), 323.9604f, "Aguila Burrito", "Best burritos in Strawberry*","MexicanMenu", FoodType.Mexican),
            new Restaurant(new Vector3(445.9454f, -1241.581f, 30.27799f), 179.553f, "Attack-A-Taco", "Kill that hunger","TacoFarmerMenu", FoodType.Mexican) { BannerImagePath = "stores\\attackataco.png" },
            new Restaurant(new Vector3(1093.13f, -362.9193f, 67.06821f), 168.6222f, "Hearty Taco", "Eat your heart out!","MexicanMenu", FoodType.Mexican),
            new Restaurant(new Vector3(438.8823f, -1465.908f, 29.35293f), 69.18111f, "Hearty Taco", "Eat your heart out!","MexicanMenu", FoodType.Mexican),
            new Restaurant(new Vector3(174.9638f, -2025.427f, 18.32407f), 123.4303f, "Mom's Tacos", "We love mom's taco!","MexicanMenu", FoodType.Mexican),
            new Restaurant(new Vector3(1138.658f, -962.4339f, 47.54031f), 330.8736f, "Mom's Tacos", "We love mom's taco!","MexicanMenu", FoodType.Mexican),
            //Dessert
            new Restaurant(new Vector3(-1193.966f, -1543.693f, 4.373522f), 124.3727f, "The Sundae Post", "Come read up on our sludgies","IceCreamMenu", FoodType.Dessert),
            new Restaurant(new Vector3(-1171.529f, -1435.118f, 4.461945f), 32.60835f, "Icemaiden", "Chill Out and Grill Out","IceCreamMenu", FoodType.Dessert) {IsWalkup = true },
            new Restaurant(new Vector3(-1689.895f, -1076.626f, 13.15219f), 52.58161f, "Cream Pie", "I just love getting creampied!","IceCreamMenu", FoodType.Dessert),
            new Restaurant(new Vector3(-1698.04f, -1105.689f, 13.15277f), 326.1168f, "The Cherry Popper", "You'll always remember your first","CherryPopperMenu", FoodType.Dessert),
            //Bagels&Donuts
            new Restaurant(new Vector3(-1318.507f, -282.2458f, 39.98732f), 115.4663f, "Dickies Bagels", "Holy Dick!","CoffeeMenu", FoodType.Bagels) { OpenTime = 4, CloseTime = 20,},
            new Restaurant(new Vector3(-1204.364f, -1146.246f, 7.699615f), 109.2444f, "Dickies Bagels", "Holy Dick!","CoffeeMenu", FoodType.Bagels) { OpenTime = 4, CloseTime = 20,},
            new Restaurant(new Vector3(354.0957f, -1028.134f, 29.33102f), 182.3497f, "Rusty Brown's", "Ring lickin' good!","RustyBrownsMenu", FoodType.Bagels | FoodType.Donut) { OpenTime = 4, CloseTime = 20,BannerImagePath = "stores\\rustybrowns.png" },

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
            new Restaurant(new Vector3(-584.761f, -872.753f, 25.91489f), 353.0746f, "Lucky Plucker", "Come be a real Lucky Plucker","LuckyPluckerMenu", FoodType.Chicken | FoodType.FastFood) {OpenTime = 5, CloseTime = 23,ScannerFilePath = "01_specific_location\\0x14B8A4DB.mp3", BannerImagePath = "stores\\luckyplucker.png" },
            new Restaurant(new Vector3(2580.543f, 464.6521f, 108.6232f), 176.5548f, "Bishop's Chicken", "Our chicken is a religious experience","BishopsChickenMenu", FoodType.Chicken | FoodType.FastFood){OpenTime = 5, CloseTime = 23, BannerImagePath = "stores\\bishops.png" },
            new Restaurant(new Vector3(169.3292f, -1634.163f, 29.29167f), 35.89598f, "Bishop's Chicken", "Our chicken is a religious experience","BishopsChickenMenu", FoodType.Chicken | FoodType.FastFood){ OpenTime = 5, CloseTime = 23,BannerImagePath = "stores\\bishops.png" },
            new Restaurant(new Vector3(133.0175f, -1462.702f, 29.35705f), 48.47223f, "Lucky Plucker", "Come be a real Lucky Plucker","LuckyPluckerMenu", FoodType.Chicken | FoodType.FastFood){OpenTime = 5, CloseTime = 23, BannerImagePath = "stores\\luckyplucker.png" },
            new Restaurant(new Vector3(-138.4921f, -256.509f, 43.59497f), 290.1001f, "Cluckin' Bell", "Taste the cock","CluckinBellMenu", FoodType.Chicken | FoodType.FastFood){OpenTime = 5, CloseTime = 23, BannerImagePath = "stores\\cluckin.png", },
            new Restaurant(new Vector3(-184.9376f, -1428.169f, 31.47968f), 33.8636f, "Cluckin' Bell", "Taste the cock","CluckinBellMenu", FoodType.Chicken | FoodType.FastFood){ OpenTime = 5, CloseTime = 23,BannerImagePath = "stores\\cluckin.png", },
            new Restaurant(new Vector3(-1681.603f, -1096.505f, 13.15227f), 180.3125f, "Cluckin' Bell", "Taste the cock","CluckinBellMenu", FoodType.Chicken | FoodType.FastFood) { OpenTime = 5, CloseTime = 23,BannerImagePath = "stores\\cluckin.png", },
            //General    
            new Restaurant(new Vector3(-1222.546f, -807.5845f, 16.59777f), 305.3918f, "Lettuce Be", "A real meat free experience","GenericMenu", FoodType.Generic),
            new Restaurant(new Vector3(-1196.705f, -1167.969f, 7.695099f), 108.4535f, "Lettuce Be", "A real meat free experience","GenericMenu", FoodType.Generic),
            new Restaurant(new Vector3(-1535.082f, -422.2449f, 35.59194f), 229.4618f, "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu", FoodType.FastFood) { BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new Restaurant(new Vector3(49.24896f, -1000.381f, 29.35741f), 335.6092f, "Chihuahua Hot Dogs", "Vegan? No. Meat? Yes.","ChihuahuaHotDogMenu", FoodType.FastFood){ BannerImagePath = "stores\\chihuahuahotdogs.png" },
            new Restaurant(new Vector3(-1271.224f, -1200.703f, 5.366248f), 70.19876f, "The Nut Buster", "Bust a Nut every day","GenericMenu", FoodType.Generic),
            new Restaurant(new Vector3(166.2677f, -1450.995f, 29.24164f), 142.858f, "Ring Of Fire Chili House", "Incinerate your insides","GenericMenu", FoodType.Generic) { ScannerFilePath = "01_specific_location\\0x0DA81BFE.mp3"},
        };
    }
    private void DefaultConfig_FireStations()
    {
        FireStations = new List<FireStation>()
        {
            new FireStation(new Vector3(1185.842f, -1464.118f, 34.90073f), 356.2903f, "LSCFD Fire Station 7", "") {
                OpenTime = 0,
                CloseTime = 24, 
                InteriorID = 81666,
                CameraPosition = new Vector3(1187.331f, -1441.786f, 41.12492f), 
                CameraDirection = new Vector3(0.3904384f, -0.8945707f, -0.2174881f), 
                CameraRotation = new Rotator(-12.56154f, -9.621827E-06f, -156.421f),
                PossiblePedSpawns = new List<ConditionalLocation>() {
                    new FireConditionalLocation(new Vector3(1187.994f, -1462.141f, 34.8952f), 343.8326f, 25f),
                    new FireConditionalLocation(new Vector3(1209.001f, -1461.391f, 34.8434f), 25.58578f, 25f), },
                VehiclePreviewLocation = new SpawnPlace(new Vector3(1201.047f, -1493.796f, 34.69257f), 176.5458f),
                VehicleDeliveryLocations = new List<SpawnPlace>(){ 
                    new SpawnPlace(new Vector3(1217.432f, -1515.697f, 34.69826f), 81.51995f),
                    new SpawnPlace(new Vector3(1191.577f, -1506.463f, 34.69253f), 92.352f)},
                VehiclePreviewCameraPosition = new Vector3(1206.029f, -1499.845f, 37.40165f), 
                VehiclePreviewCameraDirection = new Vector3(-0.5793664f, 0.757318f, -0.3013371f), 
                VehiclePreviewCameraRotation = new Rotator(-17.53793f, 3.581574E-06f, 37.41686f),
            },
            new FireStation(new Vector3(213.8019f, -1640.523f, 29.68287f), 319.3789f, "Davis Fire Station", "") {OpenTime = 0,CloseTime = 24, ScannerFilePath = "01_specific_location\\0x0AC416A0.mp3", PossiblePedSpawns = new List<ConditionalLocation>() {

                new FireConditionalLocation(new Vector3(220.1501f, -1643.144f, 29.59396f), 342.0382f, 25f),
                new FireConditionalLocation(new Vector3(200.316f, -1632.709f, 29.80013f), 317.4113f, 25f),
            } },
            new FireStation(new Vector3(-633.0533f, -122.0594f, 39.01375f), 79.69817f, "Rockford Hills Fire Station", "") {OpenTime = 0,CloseTime = 24, ScannerFilePath = "01_specific_location\\0x0A33D1E1.mp3", PossiblePedSpawns = new List<ConditionalLocation>() {

                new FireConditionalLocation(new Vector3(-636.2214f, -123.7732f, 39.01375f), 42.9723f, 25f),
                new FireConditionalLocation(new Vector3(-636.4733f, -117.1641f, 38.02922f), 78.61053f, 25f),
            } },

            new FireStation(new Vector3(-379.3594f, 6117.986f, 31.84872f), 46.27145f, "Blaine County Fire Station", "") 
            {
                OpenTime = 0,
                CloseTime = 24,
                AssignedAssociationID = "LSFD",
                PossiblePedSpawns = new List<ConditionalLocation>() 
                {
                    new FireConditionalLocation(new Vector3(-383.4373f, 6118.202f, 31.47953f), 81.63596f, 25f) { AssociationID = "BCFD" },
                    new FireConditionalLocation(new Vector3(-373.4855f, 6101.625f, 31.44381f), 114.8036f, 25f) { AssociationID = "BCFD" },
                    new FireConditionalLocation(new Vector3(-359.7747f, 6128.129f, 31.44016f), 338.6169f, 25f) { AssociationID = "BCFD" },
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(-377.4937f, 6127.635f, 31.43834f), 42.40747f, 25f) { AssociationID = "BCFD" },
                    new FireConditionalLocation(new Vector3(-374.1196f, 6131.559f, 31.4365f), 227.4514f, 25f) { AssociationID = "BCFD" },
                }
            },

            new FireStation(new Vector3(1690.468f, 3580.97f, 35.62085f), 210.2316f, "Sandy Shores Fire Station", "")
            {
                OpenTime = 0,
                CloseTime = 24,
                AssignedAssociationID = "LSFD",
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(1688.425f, 3578.468f, 35.59258f), 228.2383f, 25f) { AssociationID = "LSCoFD" },
                    new FireConditionalLocation(new Vector3(1700.156f, 3593.156f, 35.64092f), 307.3349f, 25f) { AssociationID = "LSCoFD" },
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new FireConditionalLocation(new Vector3(1697.492f, 3585.232f, 35.58413f), 206.995f, 25f) { AssociationID = "LSCoFD" },
                    new FireConditionalLocation(new Vector3(1694.652f, 3604.606f, 35.46653f), 222.9968f, 25f) { AssociationID = "LSCoFD" },
                }
            },

            //MISSING LSIA AND ZANCUDO FIRE STATIONS!
        };
    }
    private void DefaultConfig_Hospitals()
    {
        Hospitals = new List<Hospital>()
        {
            new Hospital(new Vector3(359.6708f, -584.4868f, 28.81813f), 255.1885f, "Pill Box Hill Medical Center","Some of the most famous people of LS have died here") { OpenTime = 0,CloseTime = 24, InteriorID = -787, PossiblePedSpawns = new List<ConditionalLocation>() {

                new EMSConditionalLocation(new Vector3(355.4031f, -598.8352f, 28.77448f), 250.9611f, 15f),
                new EMSConditionalLocation(new Vector3(364.3289f, -579.7377f, 28.84095f), 227.9576f, 15f),
                new EMSConditionalLocation(new Vector3(299.7511f, -579.8925f, 43.26084f), 72.9278f, 15f),
            }
            ,RespawnLocation = new Vector3(364.7124f, -583.1641f, 28.69318f),RespawnHeading = 280.637f
            ,CameraPosition = new Vector3(396.9802f, -582.7473f, 40.12638f), CameraDirection = new Vector3(-0.948761f, -0.1335131f, -0.2864031f), CameraRotation = new Rotator(-16.64274f, 1.069323E-05f, 98.01027f) },

            new Hospital(new Vector3(343.5177f, -1399.408f, 32.50927f), 49.67379f, "Central LS Medical Center","Gang Related Injury? We take Fleeca and Limit cards") {OpenTime = 0,CloseTime = 24, ScannerFilePath= "01_specific_location\\0x13583D6F.mp3", PossiblePedSpawns = new List<ConditionalLocation>() {

                new EMSConditionalLocation(new Vector3(333.7716f, -1381.868f, 32.50922f), 80.20363f, 15f),
                new EMSConditionalLocation(new Vector3(327.9682f, -1393.585f, 32.50924f), 23.40009f, 15f),
                new EMSConditionalLocation(new Vector3(297.9529f, -1448.031f, 29.96662f), 306.945f, 15f),
                new EMSConditionalLocation(new Vector3(361.8272f, -1465.073f, 29.28194f), 178.0938f, 15f),
            }
            ,RespawnLocation = new Vector3(338.208f, -1396.154f, 32.50927f), RespawnHeading = 77.07102f
            ,CameraPosition = new Vector3(304.5882f, -1362.572f, 45.39756f), CameraDirection = new Vector3(0.2721124f, -0.907807f, -0.3191258f), CameraRotation = new Rotator(-18.61007f, 2.792723E-05f, -163.3141f) },

            new Hospital(new Vector3(1839.211f, 3673.892f, 34.27668f), 215.6462f, "Sandy Shores Medical Center","Just as good as those REAL hospitals in LS") {OpenTime = 0,CloseTime = 24, PossiblePedSpawns = new List<ConditionalLocation>() {

                new EMSConditionalLocation(new Vector3(1835.39f, 3670.011f, 34.27673f), 172.621f, 15f),
                new EMSConditionalLocation(new Vector3(1844.322f, 3672.015f, 33.67998f), 196.141f, 15f),
            }
            ,RespawnLocation = new Vector3(1842.057f, 3668.679f, 33.67996f),RespawnHeading = 228.3818f
            ,CameraPosition = new Vector3(1851.532f, 3664.737f, 39.15179f), CameraDirection = new Vector3(-0.7719535f, 0.5178522f, -0.3686691f), CameraRotation = new Rotator(-21.63356f, -3.67388E-06f, 56.14497f) },

            new Hospital(new Vector3(-248.4412f, 6332.721f, 32.42618f), 225.41f, "The Bay Care Center","One stop shopping! Funeral Home and Crematorium on site") {OpenTime = 0,CloseTime = 24, PossiblePedSpawns = new List<ConditionalLocation>() {

                new EMSConditionalLocation(new Vector3(-242.6317f, 6324.892f, 32.42618f), 318.8682f, 15f),
                new EMSConditionalLocation(new Vector3(-238.915f, 6314.112f, 31.48628f), 227.0187f, 15f),
            }
            ,RespawnLocation = new Vector3(-244.3214f, 6328.575f, 32.42618f),RespawnHeading = 219.7734f
            ,CameraPosition = new Vector3(-229.6282f, 6315.992f, 36.41383f), CameraDirection = new Vector3(-0.9177954f, 0.3135513f, -0.2435924f), CameraRotation = new Rotator(-14.09866f, -7.922609E-06f, 71.13802f) },

            new Hospital(new Vector3(1151.386f, -1529.34f, 35.36609f), 337.4726f, "St. Fiacre Hospital", "Come see the most expensive specialists in LS") {OpenTime = 0,CloseTime = 24, PossiblePedSpawns = new List<ConditionalLocation>() {

                new EMSConditionalLocation(new Vector3(1150.269f, -1524.425f, 34.84344f), 316.4077f, 15f),
                new EMSConditionalLocation(new Vector3(1137.925f, -1514.329f, 34.84341f), 327.5841f, 15f),
                new EMSConditionalLocation(new Vector3(1171.36f, -1520.041f, 34.8434f), 319.8948f, 15f),
            }
            ,RespawnLocation = new Vector3(1151.386f, -1529.34f, 35.36609f),RespawnHeading = 337.4726f
            ,CameraPosition = new Vector3(1152.484f, -1510.302f, 41.42735f), CameraDirection = new Vector3(-0.1392823f, -0.9661027f, -0.2173616f), CameraRotation = new Rotator(-12.55411f, 2.186715E-07f, 171.7962f) },


            new Hospital(new Vector3(-449.76f, -341.03f, 35.5f), 106f, "Mount Zonah Medical Center", "The hospital with the largest advertising budget") { OpenTime = 0,CloseTime = 24,
                PossiblePedSpawns = new List<ConditionalLocation>() {
                    new EMSConditionalLocation(new Vector3(-448.7528f, -334.6505f, 33.5019f), 161.25828389f, 15f),
                    new EMSConditionalLocation(new Vector3(-447.4306f, -328.7594f, 33.50186f), 190.084495712696f,15f),
                    new EMSConditionalLocation(new Vector3(-447.0008f, -329.9945f, 33.50186f), 27.4845562493075f,15f),
                    new EMSConditionalLocation(new Vector3(-496.9766f, -331.2807f, 33.50167f), 223.606079066179f,15f),
                    new EMSConditionalLocation(new Vector3(-464.4201f, -353.7608f, 33.39479f), 111.44917199877f,15f),
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


            new Hospital(new Vector3(3132.073f, -4839.958f, 112.0312f), 354.8388f, "Ludendorff Clinic", "The service you'd expect!") { StateID = StaticStrings.NorthYanktonStateID, OpenTime = 0,CloseTime = 24,
                PossiblePedSpawns = new List<ConditionalLocation>() {
            }
            ,RespawnLocation = new Vector3(3132.073f, -4839.958f, 112.0312f),RespawnHeading = 354.8388f },

            //new Hospital(new Vector3(241.2085f, -1378.962f, 33.74176f), 140.41f, LocationType.Morgue, "Los Santos County Coroner Office", "") {OpenTime = 0,CloseTime = 24, InteriorID = 60418, TeleportEnterPosition = new Vector3(253.351f, -1364.622f, 39.53437f), TeleportEnterHeading = 327.1821f },
        };
    }
    private void DefaultConfig_Prisons()
    {
        Prisons = new List<Prison>()
        {
            new Prison(new Vector3(1846.258f, 2586.139f, 45.67202f), 269.2306f, "Bolingbroke Penitentiary","Where the scum of LS washes up")  
            { 
                OpenTime = 0,
                CloseTime = 24,
                ActivateCells = 8,
                AssignedAssociationID = "SASPA",
                RestrictedAreas = new RestrictedAreas()
                {
                    VanillaRestrictedAreas = new List<VanillaRestrictedArea>()
                    {
                        new VanillaRestrictedArea()
                        {
                            AngledRestrictedAreas = new List<AngledRestrictedArea>()
                            {
                                new AngledRestrictedArea(new Vector3(1541.6072f, 2527.555f, 40f),new Vector3(1815.5753f, 2535.0596f,150f),114f),
                                new AngledRestrictedArea(new Vector3(1788.8787f, 2445.7273f, 40f),new Vector3(1716.9603f, 2502.957f,150f),88.5f),
                                new AngledRestrictedArea(new Vector3(1601.1575f, 2436.2441f, 40f),new Vector3(1650.0776f, 2515.9226f,150f),133.25f),
                                new AngledRestrictedArea(new Vector3(1706.3307f, 2407.5972f, 40f),new Vector3(1698.5546f, 2460.2078f,150f),104.5f),
                                new AngledRestrictedArea(new Vector3(1712.4517f, 2756.2175f, 40f),new Vector3(1718.8477f, 2589.1616f, 150f),121.75f),
                                new AngledRestrictedArea(new Vector3(1830.2278f, 2661.2402f, 40f),new Vector3(1774.8124f, 2679.4187f,150f), 84.5f),
                                new AngledRestrictedArea(new Vector3(1559.0503f, 2632.2205f, 40f),new Vector3(1657.2083f, 2595.4844f, 150f),103.75f),
                                new AngledRestrictedArea(new Vector3(1612.0209f, 2716.869f, 40f),new Vector3(1657.1647f, 2669.721f, 150f),104.25f),
                                new AngledRestrictedArea(new Vector3(1809.8721f, 2729.827f, 40f),new Vector3(1789.8551f, 2705.0369f,150f),91f),
                                new AngledRestrictedArea(new Vector3(1818.7888f, 2605.9478f, 40f),new Vector3(1783.1143f, 2606.7832f,150f),51.25f),
                            },
                        },
                    },
                },
                PossiblePedSpawns = new List<ConditionalLocation>() 
                {
                    new LEConditionalLocation(new Vector3(1899.234f, 2605.696f, 44.96621f), 354f, 100f) { TaskRequirements = TaskRequirements.Guard,LongGunAlwaysEquipped = true },//guard facing towards entry
                    new LEConditionalLocation(new Vector3(1830.171f, 2602.584f, 44.88912f), 0f, 100f) { TaskRequirements = TaskRequirements.Guard,LongGunAlwaysEquipped = true },//guard facing towards entry second guard booth
                    new LEConditionalLocation(new Vector3(1846.473f, 2584.199f, 44.67195f), 295f, 40f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.EquipLongGunWhenIdle,LongGunAlwaysEquipped = true, ForceSidearm = true,ForceLongGun = true },//front of prison
                    new LEConditionalLocation(new Vector3(1845.851f, 2587.203f, 44.67231f), 290f, 40f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.EquipSidearmWhenIdle,LongGunAlwaysEquipped = true, ForceSidearm = true,ForceLongGun = true },
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>() 
                {
                    new LEConditionalLocation(new Vector3(1855.314f, 2578.854f, 46.42464f), 92f, 35f),//facing towards prison
                    new LEConditionalLocation(new Vector3(1869.82f, 2588.359f, 46.42464f), 269f, 35f),//facing away from prison
                    new LEConditionalLocation(new Vector3(1854.312f, 2620.756f, 46.42464f), 92f, 35f),//facing towards prison
                    new LEConditionalLocation( new Vector3(1869.65f, 2577.781f, 45.18463f), 269f, 35f),//facing away from prison
                }
            },
        };
    }
    private void DefaultConfig_PoliceStations()
    {
        float DavisPercentage = 35f;

        Vector2[] davisImpoundLot = new Vector2[]
                {
                    new Vector2 { X = 409.3969f, Y = -1616.724f },
                    new Vector2 { X = 423.1207f, Y = -1628.206f },
                    new Vector2 { X = 423.9051f, Y = -1634.224f },
                    new Vector2 { X = 423.3925f, Y = -1645.131f },
                    new Vector2 { X = 410.9243f, Y = -1660.619f },
                    new Vector2 { X = 409.476f, Y = -1660.212f },
                    new Vector2 { X = 388.094f, Y = -1641.983f },
                };
        List<InteriorDoor> davisImpoundGates = new List<InteriorDoor>() { new InteriorDoor(2811495845, new Vector3(413.364f, -1620.034f, 28.34158f)), new InteriorDoor(2811495845, new Vector3(418.291f, -1651.395f, 28.29171f)) };
        Vector2[] davisPoliceLot = new Vector2[]
        {
                    new Vector2 { X = 391.2724f, Y = -1602.033f },
                    new Vector2 { X = 408.1592f, Y = -1616.15f },
                    new Vector2 { X = 387.218f, Y = -1641.259f },

                    new Vector2 { X = 344.405f, Y = -1605.337f },
                    new Vector2 { X = 346.1739f, Y = -1616.41f },
                    new Vector2 { X = 361.7633f, Y = -1616.41f },
                    new Vector2 { X = 370.0233f, Y = -1606.763f },

                    new Vector2 { X = 373.6666f, Y = -1605.761f },
                    new Vector2 { X = 382.7977f, Y = -1612.137f },
        };
        List<InteriorDoor> davisPoliceGate = new List<InteriorDoor>() { new InteriorDoor(1286535678, new Vector3(397.8851f, -1607.386f, 28.34166f)) };
        PoliceStations = new List<PoliceStation>()
        {
            new PoliceStation(new Vector3(361.1365f, -1584.821f, 29.29195f), 48.07573f, "Davis Sheriff's Station","A Tradition of Suppression") {
                RespawnLocation = new Vector3(358.9726f, -1582.881f, 29.29195f),
                RespawnHeading = 323.5287f,
                MaxAssaultSpawns = 25,
                BannerImagePath = "agencies\\lssddavis.png",
                OpenTime = 0,
                CloseTime = 24,
                VehiclePreviewLocation = new SpawnPlace(new Vector3(383.6079f, -1623.259f, 29.29195f), 314.9681f),
                //CameraPosition = new Vector3(384.024f, -1617.086f, 31.70451f), 
               // CameraDirection = new Vector3(-0.115703f, -0.9545547f, -0.2746601f), 
                //CameraRotation = new Rotator(-15.94176f, -6.659415E-07f, 173.0888f),
                AssaultSpawnLocations = new List<SpawnPlace>()
                {
                    new SpawnPlace(new Vector3(371.7006f, -1609.662f, 29.29194f), 226.7574f),
                    new SpawnPlace(new Vector3(372.206f, -1612.077f, 29.29194f), 288.6986f),
                    new SpawnPlace(new Vector3(352.7339f, -1593.033f, 29.29193f), 356.82f),
                    new SpawnPlace(new Vector3(360.7824f, -1582.344f, 29.29195f), 16.9674f),
                },
                RestrictAssaultSpawningUsingPedSpawns = false,
                PossiblePedSpawns = new List<ConditionalLocation>() {
                    //new LEConditionalLocation(new Vector3(343.7892f, -1602.402f, 29.29194f), 336.6382f, DavisPercentage),
                    new LEConditionalLocation(new Vector3(373.5635f, -1612.563f, 29.29194f), 229.7581f, 35f),
                    //new LEConditionalLocation(new Vector3(375.4213f, -1609.626f, 29.29194f), 193.4761f, DavisPercentage),
                    new LEConditionalLocation(new Vector3(358.3576f, -1581.505f, 29.29195f), 321.0721f, 35f),
                    new LEConditionalLocation(new Vector3(370.2834f, -1579.717f, 29.29238f), 303.5159f, 35f),
                    new LEConditionalLocation(new Vector3(363.9216f, -1575.142f, 29.27452f), 350.0409f, 35f),
                    new LEConditionalLocation(new Vector3(337.8395f, -1596.521f, 29.31022f), 48.88332f, 35f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>() {
                    new LEConditionalLocation(new Vector3(388.9854f, -1612.977f, 29.21355f), 50f,45f),
                    new LEConditionalLocation(new Vector3(392.7548f, -1608.376f, 29.21355f), 50f,45f),
                    new LEConditionalLocation(new Vector3(399.393f, -1621.396f, 29.20119f), 50f,45f),
                    new LEConditionalLocation(new Vector3(351.3006f, -1556.711f, 29.24393f), 230f,45f),
                    new LEConditionalLocation(new Vector3(385.5102f, -1624.759f, 29.29195f), 312.3226f, 30f) { ForceVehicleGroup = true, AssociationID = "LSSD", RequiredVehicleGroup = "Historic" },
                },
                VehicleImpoundLot = new VehicleImpoundLot("Impound Lot 1",new List<SpawnPlace>()
                {
                    new SpawnPlace(new Vector3(404.3888f, -1649.421f, 28.82277f), 321.7852f),
                    new SpawnPlace(new Vector3(408.3622f, -1654.027f, 28.8215f), 322.5292f),
                    new SpawnPlace(new Vector3(420.4157f, -1639.205f, 28.82125f), 272.8717f),
                    new SpawnPlace(new Vector3(418.0018f, -1645.961f, 28.82127f), 230.7471f),
                    new SpawnPlace(new Vector3(419.366f, -1629.477f, 28.82145f), 319.8078f), }),
                RestrictedAreas = new RestrictedAreas()
                {
                    RestrictedAreasList = new List<RestrictedArea>()
                    {
                        new RestrictedArea("Vehicle Impound Lot", davisImpoundLot,davisImpoundGates, RestrictedAreaType.ImpoundLot) {
                            SecurityCameras = new List<SecurityCamera>() {
                                new SecurityCamera(Game.GetHashKey("prop_cctv_pole_04"), new Vector3(411.6299f, -1619.302f, 28.30813f),187.8307f) { Name = "Security Cam 1" },
                                new SecurityCamera(Game.GetHashKey("prop_cctv_pole_04"), new Vector3(409.8848f,-1660.358f,28.25814f),357.6599f) { Name = "Security Cam 2" },
                            } },
                        new RestrictedArea("Police Parking Lot",davisPoliceLot,davisPoliceGate,RestrictedAreaType.PoliceLot),

                    }
                }
            },
            new PoliceStation(new Vector3(1855.447f, 3682.999f, 34.26752f), 209.9094f,  "Sandy Shores Sheriff's Station","A Tradition of Suppression") {
                BannerImagePath = "agencies\\lssdmain.png",
                OpenTime = 0,
                CloseTime = 24,
                MaxAssaultSpawns = 10,
                RespawnLocation = new Vector3(1858.19f, 3679.873f, 33.75724f),
                RespawnHeading = 218.3256f,
                InteriorID = -707,
               // CameraPosition = new Vector3(1880.906f, 3686.685f, 36.28576f),
              //  CameraDirection = new Vector3(-0.8681535f, 0.4227323f, -0.2600135f),
               // CameraRotation = new Rotator(-15.07086f, 3.53674E-06f, 64.03703f),

                                IgnoreEntranceInteract = true,


                VehiclePreviewLocation = new SpawnPlace(new Vector3(1873.62f, 3690.187f, 33.55123f), 199.1265f),
                PossiblePedSpawns = new List<ConditionalLocation>() {
                    new LEConditionalLocation(new Vector3(1851.615f, 3679.759f, 34.26711f), 167.1253f, 65f),
                    new LEConditionalLocation(new Vector3(1866.224f, 3684.685f, 33.78798f), 229.2713f, 65f),
                    new LEConditionalLocation(new Vector3(1852.652f, 3689.82f, 34.26704f), 206.5214f, 100f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>() {
                    new LEConditionalLocation(new Vector3(1831.629f, 3662.91f, 33.92607f), 32f,55f),
                    new LEConditionalLocation(new Vector3(1835.127f, 3664.892f, 33.92607f), 32f,55f),
                    new LEConditionalLocation(new Vector3(1847.131f, 3672.587f, 33.92607f), 32f,55f),
                } },
            new PoliceStation(new Vector3(-443.3767f, 6016.792f, 31.71221f), 310.4416f, "Paleto Bay Sheriff's Office","A Tradition of Suppression") {
                BannerImagePath = "agencies\\lssdmain.png",
                OpenTime = 0,
                CloseTime = 24 ,
                InteriorID = 3842,
                RespawnLocation = new Vector3(-438.0854f, 6021.318f, 31.49011f),
                RespawnHeading = 310.0719f,
                VehiclePreviewLocation = new SpawnPlace(new Vector3(-468.9866f, 6019.28f, 31.34055f), 306.1413f),

                                IgnoreEntranceInteract = true,

              //  CameraPosition = new Vector3(-468.4744f, 6024.704f, 33.34053f), 
               // CameraDirection = new Vector3(-0.1096192f, -0.9549786f, -0.2756799f), 
               // CameraRotation = new Rotator(-16.00254f, -3.330718E-07f, 173.4518f),
                ScannerFilePath = "01_specific_location\\0x0E94FE38.mp3",
                PossiblePedSpawns = new List<ConditionalLocation>() {
                    new LEConditionalLocation(new Vector3(-438.606f, 6021.66f, 31.49011f), 358.9023f, 40f),
                    new LEConditionalLocation(new Vector3(-448.8453f, 6011.864f, 31.71639f), 310.0714f, 100f),//is front desk spawn, always if can
                    new LEConditionalLocation(new Vector3(-444.1118f, 6011.967f, 31.71639f), 359.0966f, 40f),
                    new LEConditionalLocation(new Vector3(-454.7975f, 6007.629f, 31.49011f), 131.4658f, 40f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>() {
                new LEConditionalLocation(new Vector3(-454.4307f, 6040.472f, 31.17424f), 317f,35f),
                new LEConditionalLocation(new Vector3(-457.8087f, 6043.849f, 31.17424f), 317f,35f),
                new LEConditionalLocation(new Vector3(-468.1928f, 6038.506f, 31.17422f), 45f,35f),
                new LEConditionalLocation(new Vector3(-475.1396f, 6031.421f, 31.17419f), 45f,35f),
                new LEConditionalLocation(new Vector3(-458.788f, 6005.529f, 31.17422f), 267f,35f),
                new LEConditionalLocation(new Vector3(-455.154f, 6001.894f, 31.17422f), 267f,35f),
                new LEConditionalLocation(new Vector3(-475.5325f, 5988.582f, 31.33671f), 311.222f,45f) { AllowAirVehicle = true,RequiredVehicleGroup = "Helicopter" },
            }},
            new PoliceStation(new Vector3(434.3387f, -981.8954f, 30.70984f), 89.51098f, "Mission Row Police Station","Obey and Survive") {
                OpenTime = 0,
                CloseTime = 24,
                InteriorID= 30978,
                MaxAssaultSpawns = 30,
                RespawnLocation = new Vector3(440.0835f, -982.3911f, 30.68966f),
                RespawnHeading = 47.88088f,
                ScannerFilePath = "01_specific_location\\0x0A45FA8A.mp3",
                VehiclePreviewLocation = new SpawnPlace(new Vector3(439.3361f, -1017.903f, 28.2705f), 89.54115f),
                BannerImagePath = "agencies\\lspdmain.png",
                IgnoreEntranceInteract = true,
                //CameraPosition = new Vector3(434.2917f, -1023.771f, 29.87288f), 
               // CameraDirection = new Vector3(0.6072524f, 0.7701081f, -0.1953922f), 
               // CameraRotation = new Rotator(-11.26764f, 1.262302E-05f, -38.25679f),

                RestrictedAreas = new RestrictedAreas()
                {
                    VanillaRestrictedAreas = new List<VanillaRestrictedArea>()
                    {
                        new VanillaRestrictedArea() 
                        { 
                            AngledRestrictedAreas = new List<AngledRestrictedArea>()
                            { 
                                new AngledRestrictedArea(new Vector3(461.5684f, -984.57196f, 29.439508f),new Vector3(471.17004f, -984.4292f, 40.14212f),7.75f),
                                new AngledRestrictedArea(new Vector3(457.3404f, -984.75604f, 34.439507f),new Vector3(457.20837f, -993.71893f, 29.389584f),14.75f),
                                new AngledRestrictedArea(new Vector3(477.62268f, -986.60004f, 40.00819f),new Vector3(424.8687f, -986.3279f, 48.712406f),31.5f),
                                new AngledRestrictedArea(new Vector3(474.38895f, -974.4613f, 39.557606f),new Vector3(474.0358f, -1021.9721f, 49.10033f),30.5f),
                                new AngledRestrictedArea(new Vector3(442.17685f, -974.1888f, 29.689508f),new Vector3(442.18552f, -979.8635f, 33.439507f),6.75f),
                            },
                        },
                    },
                },
                PossiblePedSpawns = new List<ConditionalLocation>() {
                    new LEConditionalLocation(new Vector3(427.7032f, -982.3438f, 30.7101f), 46.43887f, 40f),
                    new LEConditionalLocation(new Vector3(432.3652f, -973.1894f, 30.71074f), 60.75554f, 40f),
                    new LEConditionalLocation(new Vector3(441.3313f, -977.6323f, 30.68966f), 167.8129f, 100f),//Is front desk spawn, always if can
                    new LEConditionalLocation(new Vector3(426.1106f, -1003.349f, 30.71002f), 159.3153f, 40f),
                    new LEConditionalLocation(new Vector3(480.1246f, -974.9543f, 27.98389f), 332.2961f, 40f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>() {
                    new LEConditionalLocation(new Vector3(407.7554f, -984.2084f, 29.89806f), 228f,30f),
                    new LEConditionalLocation(new Vector3(408.0923f, -988.8488f, 29.85523f), 228f,30f),
                    new LEConditionalLocation(new Vector3(407.9883f, -998.3094f, 29.81112f), 228f,30f),
                    new LEConditionalLocation(new Vector3(427.5909f, -1027.707f, 29.22805f), 185f,30f),
                    new LEConditionalLocation(new Vector3(434.9848f, -1027.103f, 29.12844f), 185f,30f),
                    new LEConditionalLocation(new Vector3(442.5143f, -1026.687f, 28.98147f), 185f,30f),
                    new LEConditionalLocation(new Vector3(446.3985f, -1026.087f, 28.92508f), 185f,30f),
                    new LEConditionalLocation(new Vector3(496.3687f, -996.015f, 28.3387f), 178.4318f,30f),
                    new LEConditionalLocation(new Vector3(496.3721f, -1016.798f, 28.687f), 178.4318f,30f),

                     new LEConditionalLocation(new Vector3(449.7793f, -981.5451f, 43.69165f), 83.32257f,35f) { AllowAirVehicle = true,AssociationID = "LSPD-ASD" },
                     new LEConditionalLocation(new Vector3(454.1368f, -1016.551f, 28.43479f), 107.0836f, 30f) { RequiredVehicleGroup = "Historic",ForceVehicleGroup = true, },

                } },
            new PoliceStation(new Vector3(827.5469f, -1289.998f, 28.24066f), 86.95251f, "La Mesa Police Station","Obey and Survive") {
                BannerImagePath = "agencies\\lspdlamesa.png",
                OpenTime = 0,
                CloseTime = 24,
                MaxAssaultSpawns = 20,
                RespawnLocation = new Vector3(815.8774f, -1290.531f, 26.28391f),
                RespawnHeading = 74.91704f,
                VehiclePreviewLocation = new SpawnPlace(new Vector3(860.3976f, -1349.983f, 26.06512f),79.33445f),
               // CameraPosition = new Vector3(852.6959f, -1356.233f, 29.07957f), 
               // CameraDirection = new Vector3(0.7865043f, 0.5654681f, -0.2483079f), 
               // CameraRotation = new Rotator(-14.37741f, 1.806823E-05f, -54.28523f),
                PossiblePedSpawns = new List<ConditionalLocation>() {
                    new LEConditionalLocation(new Vector3(823.9117f, -1288.765f, 28.24066f), 74.91795f, 40f),
                    new LEConditionalLocation(new Vector3(823.3896f, -1291.954f, 28.24066f), 102.1528f, 40f),
                    new LEConditionalLocation(new Vector3(821.5445f, -1275.796f, 26.38955f), 61.38755f, 40f) { AssociationID = "SAHP" },
                    new LEConditionalLocation(new Vector3(843.4933f, -1314.606f, 26.25885f), 126.1716f, 40f),
                }
                ,PossibleVehicleSpawns = new List<ConditionalLocation>() {
                    new LEConditionalLocation(new Vector3(822.8635f, -1258.039f, 26.34347f), 0f,20f),
                    new LEConditionalLocation(new Vector3(828.0373f, -1258.039f, 26.34347f), 0f,20f),
                    new LEConditionalLocation(new Vector3(833.577f, -1258.954f, 26.34347f), 180f,20f) { AssociationID = "SAHP" },
                    new LEConditionalLocation(new Vector3(838.7137f, -1271.55f, 26.34347f), 180f,20f),
                    new LEConditionalLocation(new Vector3(833.4279f, -1271.55f, 26.34347f), 0f,20f),
                    new LEConditionalLocation(new Vector3(822.5837f, -1271.55f, 26.34347f), 0f,20f),
                    new LEConditionalLocation(new Vector3(828.1719f, -1333.792f, 26.18776f), 242f,20f) { AssociationID = "SAHP" },
                    new LEConditionalLocation(new Vector3(828.1719f, -1339.649f, 26.18776f), 242f,20f),
                    new LEConditionalLocation(new Vector3(828.1719f, -1345.815f, 26.18776f), 242f,20f) { AssociationID = "SAHP" },
                    new LEConditionalLocation(new Vector3(828.362f, -1351.482f, 26.21234f), 65f,20f),
                    new LEConditionalLocation(new Vector3(843.9627f, -1334.354f, 26.17253f), 65f,20f),
                    new LEConditionalLocation(new Vector3(843.874f, -1340.518f, 26.18776f), 242f,20f),
                    new LEConditionalLocation(new Vector3(844.3544f, -1346.3f, 26.21234f), 65f,20f),
                    new LEConditionalLocation(new Vector3(843.7897f, -1352.283f, 26.21234f), 65f,20f),
                    new LEConditionalLocation(new Vector3(865.7624f, -1378.407f, 26.21234f), 216f,20f) { AssociationID = "SAHP" },
                    new LEConditionalLocation(new Vector3(862.8425f, -1383.55f, 26.21234f), 216f,20f),
                    new LEConditionalLocation(new Vector3(859.8381f, -1388.58f, 26.21234f), 216f,20f),
                    new LEConditionalLocation(new Vector3(857.2202f, -1393.802f, 26.21234f), 216f,20f) { AssociationID = "SAHP" },
                    new LEConditionalLocation(new Vector3(854.2248f, -1398.952f, 26.21234f), 216f,20f),
                    new LEConditionalLocation(new Vector3(831.9248f, -1370.362f, 26.13427f), 266.9978f, 20f) { RequiredVehicleGroup = "Historic",AssociationID = "SAHP",ForceVehicleGroup = true, },
                } },
            new PoliceStation(new Vector3(638.4491f, 1.559977f, 82.78642f), 247.7514f,"Vinewood Police Station","Obey and Survive") {
                BannerImagePath = "agencies\\lspdmain.png",
                OpenTime = 0,
                CloseTime = 24,
                MaxAssaultSpawns = 30,
                RespawnLocation = new Vector3(644.8676f, -5.218857f, 82.7738f),
                RespawnHeading = 222.2603f,
                ActivateCells = 5,
                ActivateDistance = 300f,
                //CameraPosition = new Vector3(543.0516f, -41.45242f, 72.72662f), 
                //CameraDirection = new Vector3(-0.8347768f, 0.4947414f, -0.2416167f), 
               // CameraRotation = new Rotator(-13.98198f, 4.399209E-06f, 59.34634f),
                VehiclePreviewLocation = new SpawnPlace(new Vector3(536.6809f, -38.35585f, 70.72294f), 210.4488f),
                PossiblePedSpawns = new List<ConditionalLocation>() {
                    new LEConditionalLocation(new Vector3(641.4031f, 0.478787f, 82.78651f), 235.3406f, 55f),
                    new LEConditionalLocation(new Vector3(647.2952f, -11.50604f, 82.60114f), 223.3047f, 55f),
                    new LEConditionalLocation(new Vector3(624.8208f, 20.23717f, 87.97021f), 345.2158f, 55f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>() {
                    new LEConditionalLocation(new Vector3(666.4646f, -11.79645f, 82.76681f), 144f,45f),
                    new LEConditionalLocation(new Vector3(621.6934f, 26.27448f, 88.66011f), 193f,45f),
                    new LEConditionalLocation(new Vector3(615.3292f, 28.48364f, 89.28476f), 193f,45f),
                    new LEConditionalLocation(new Vector3(609.7616f, 30.84756f, 89.91243f), 193f,45f),
                    new LEConditionalLocation(new Vector3(597.8726f, 34.8121f, 91.07567f), 193f,45f),
                    new LEConditionalLocation(new Vector3(586.7384f, 37.78894f, 92.30818f), 193f,45f),
                    new LEConditionalLocation(new Vector3(580.8351f, 38.85468f, 92.82274f), 193f,45f),
                } },
            new PoliceStation(new Vector3(-557.0687f, -134.7315f, 38.20231f), 214.5968f, "Rockford Hills Police Station","Obey and Survive") {
                BannerImagePath = "agencies\\lspdmain.png",
                OpenTime = 0,
                CloseTime = 24, 
                MaxAssaultSpawns = 20,
                PossiblePedSpawns = new List<ConditionalLocation>() {
                    new LEConditionalLocation(new Vector3(-560.6538f, -134.251f, 38.11069f), 180.7514f, 55f),
                    new LEConditionalLocation(new Vector3(-563.0671f, -141.727f, 38.32593f), 192.5406f, 55f),
                    new LEConditionalLocation(new Vector3(-541.8491f, -134.7859f, 38.55503f), 208.2207f, 55f),

                },
                PossibleVehicleSpawns = new List<ConditionalLocation>() {
                    new LEConditionalLocation(new Vector3(-574.256f, -168.7862f, 38.49216f), 291f,45f),
                    new LEConditionalLocation(new Vector3(-557.7739f, -161.9565f, 38.62593f), 291f,45f),
                    new LEConditionalLocation(new Vector3(-559.7783f, -147.1107f, 38.65532f), 58f,45f),
                    new LEConditionalLocation(new Vector3(-555.6528f, -145.4567f, 38.72502f), 58f,45f),
                    new LEConditionalLocation(new Vector3(-551.0397f, -144.0132f, 38.65663f), 58f,45f),
                } },
            new PoliceStation(new Vector3(-1093.817f, -807.1993f, 19.28864f), 22.23846f, "Vespucci Police Station","Obey and Survive") {
                BannerImagePath = "agencies\\lspdmain.png",
                OpenTime = 0,
                CloseTime = 24, 
                MaxAssaultSpawns = 5,
                PossiblePedSpawns = new List<ConditionalLocation>() {
                    new LEConditionalLocation(new Vector3(-1114.355f, -822.5755f, 19.3168f), 14.61082f, 55f),
                    new LEConditionalLocation(new Vector3(-1135.939f, -847.7239f, 19.34725f), 31.25465f, 55f),
                    new LEConditionalLocation(new Vector3(-1056.813f, -820.9451f, 19.23504f), 300.4652f, 55f),

                },
                PossibleVehicleSpawns = new List<ConditionalLocation>() {
                    new LEConditionalLocation(new Vector3(-1072.822f, -880.3561f, 4.809089f), 208f,10f),
                    new LEConditionalLocation(new Vector3(-1075.95f, -882.3492f, 4.809089f), 208f,10f),
                    new LEConditionalLocation(new Vector3(-1051.726f, -867.1277f, 4.809089f), 230f,10f),
                    new LEConditionalLocation(new Vector3(-1045.53f, -861.5321f, 4.809089f), 230f,10f),
                    new LEConditionalLocation(new Vector3(-1042.226f, -857.9979f, 4.809089f), 230f,10f),
                    new LEConditionalLocation(new Vector3(-1047.814f, -846.7044f, 4.809089f), 37f,10f),
                    new LEConditionalLocation(new Vector3(-1052.352f, -846.8544f, 4.809089f), 37f,10f),
                    new LEConditionalLocation(new Vector3(-1055.076f, -849.5068f, 4.809089f), 37f,10f),
                    new LEConditionalLocation(new Vector3(-1058.752f, -851.4465f, 4.809089f), 37f,10f),
                    new LEConditionalLocation(new Vector3(-1126.5f, -864.8307f, 13.63185f), 220f,10f),
                    new LEConditionalLocation(new Vector3(-1122.896f, -863.4746f, 13.6122f), 220f,10f),
                    new LEConditionalLocation(new Vector3(-1115.937f, -857.7859f, 13.65187f), 220f,10f),
                    new LEConditionalLocation(new Vector3(-1138.613f, -845.7916f, 13.98058f), 220f,10f),
                } },
            new PoliceStation(new Vector3(-1633.314f, -1010.025f, 13.08503f), 351.7007f, "Del Perro Police Station","Obey and Survive") {
                BannerImagePath = "agencies\\lspdmain.png",
                OpenTime = 0,
                CloseTime = 24, 
                MaxAssaultSpawns = 2,
                PossiblePedSpawns = new List<ConditionalLocation>() {
                    new LEConditionalLocation(new Vector3(-1629.069f, -1013.071f, 13.11924f), 298.982f, 55f),
                    new LEConditionalLocation(new Vector3(-1639.286f, -1011.673f, 13.12066f), 24.4451f, 55f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>() {
                    new LEConditionalLocation(new Vector3(-1625.343f, -1013.629f, 13.89048f), 0f,75f),
                } },
            new PoliceStation(new Vector3(-1311.877f, -1528.808f, 4.410581f), 233.9121f, "Vespucci Beach Police Station","Obey and Survive") {
                BannerImagePath = "agencies\\lspdmain.png",
                OpenTime = 0,
                CloseTime = 24, 
                MaxAssaultSpawns = 5,
                ScannerFilePath = "01_specific_location\\0x13CBAB64.mp3", 
                PossiblePedSpawns = new List<ConditionalLocation>() {
                    new LEConditionalLocation(new Vector3(-1314.879f, -1532.083f, 4.423616f), 190.5698f, 60f),
                    new LEConditionalLocation(new Vector3(-1307.496f, -1526.527f, 4.359179f), 244.9207f, 60f),

                } },
            new PoliceStation(new Vector3(102.9145f, -743.9487f, 45.75473f), 79.8266f, "FIB Headquarters","We're corrupt in a good way") { 
                AssignedAssociationID = "FIB", 
                InteriorID = 58882, 
                OpenTime = 0,
                CloseTime = 24,
                MaxAssaultSpawns = 30,
                IgnoreEntranceInteract = true,
                ScannerFilePath = "01_specific_location\\0x1667D63F.mp3", 
                PossiblePedSpawns = new List<ConditionalLocation>() 
                {
                    new LEConditionalLocation(new Vector3(101.3534f, -745.8216f, 45.75475f), 76.27069f, 55f),
                    new LEConditionalLocation(new Vector3(117.8028f, -747.0616f, 45.75158f), 102.5501f, 100f),// { TaskRequirements = TaskRequirements.LocalScenario },//is front desk spawn
                   // new LEConditionalLocation(new Vector3(117.8712f, -748.6705f, 45.75157f), 111.3529f, 100f) { TaskRequirements = TaskRequirements.LocalScenario | TaskRequirements.Guard },//is front desk spawn
                    new LEConditionalLocation(new Vector3(113.9787f, -758.2271f, 45.75474f), 21.6145f, 55f),
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                { 
                    new LEConditionalLocation(new Vector3(74.06145f, -698.9698f, 43.63071f), 341.3815f, 55f),
                    new LEConditionalLocation(new Vector3(71.99015f, -704.8258f, 43.63755f), 340.0341f, 55f),
                    new LEConditionalLocation(new Vector3(76.40812f, -693.0204f, 43.62196f), 339.8584f, 55f), 
                }, 
            },
                    new PoliceStation(new Vector3(387.16f, 789.96f, 188.23f), 178f, "Beaver Bush Ranger Station","You won't get this bush!") { 
                AssignedAssociationID = "SAPR", 
                
                OpenTime = 0,
                CloseTime = 24, 
                PossiblePedSpawns = new List<ConditionalLocation>() {
                    new LEConditionalLocation(new Vector3(391.3105f, 789.5917f, 186.6761f), 178f,55f) { AssociationID = "LSDPR" },
                    new LEConditionalLocation(new Vector3(381.8302f, 789.8931f, 186.6757f), 178f,55f) { AssociationID = "LSDPR" },
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>(){ 
                    new LEConditionalLocation(new Vector3(374.2932f, 795.6959f, 186.5305f), 178f,70f) { AssociationID = "LSDPR" },

                    new LEConditionalLocation(new Vector3(366.0268f, 791.4189f, 187.3132f), 240.688f, 35f){ RequiredVehicleGroup = "Historic", AssociationID = "SAPR",ForceVehicleGroup = true, },

                } },
            new PoliceStation(new Vector3(-893.9146f, -2401.547f, 14.02436f), 148.8757f, "NOoSE LSIA","Let's tighten the noose!") { 
                AssignedAssociationID = "NOOSE", 
                OpenTime = 0,
                CloseTime = 24, 
                MaxAssaultSpawns = 30,
                PossiblePedSpawns = new List<ConditionalLocation>() {
                    new LEConditionalLocation(new Vector3(-896.5345f, -2388.417f, 14.02436f), 63.63145f, 45f){ AssociationID = "NOOSE-BP", TaskRequirements = TaskRequirements.Guard | TaskRequirements.StandardScenario ,LongGunAlwaysEquipped = true},
                    new LEConditionalLocation(new Vector3(-897.6145f, -2398.739f, 14.02436f), 117.328f, 45f){ AssociationID = "NOOSE-PIA", TaskRequirements = TaskRequirements.Guard | TaskRequirements.StandardScenario,LongGunAlwaysEquipped = true },
                    new LEConditionalLocation(new Vector3(-889.1294f, -2405.644f, 14.02639f), 117.7686f, 45f){ TaskRequirements = TaskRequirements.Patrol,LongGunAlwaysEquipped = true },
                    new LEConditionalLocation(new Vector3(-868.955f, -2417.757f, 14.02489f), 200.5734f, 45f){ AssociationID = "NOOSE-PIA",TaskRequirements = TaskRequirements.Guard | TaskRequirements.Patrol | TaskRequirements.StandardScenario,LongGunAlwaysEquipped = true },
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {

                    new LEConditionalLocation(new Vector3(-900.9653f, -2391.173f, 13.47696f), 148.7623f, 45f) { AssociationID = "NOOSE-PIA" },
                    new LEConditionalLocation(new Vector3(-862.1013f, -2414.928f, 13.47701f), 147.4044f, 45f),
                    new LEConditionalLocation(new Vector3(-858.2088f, -2397.289f, 13.47724f), 161.3987f, 45f) { AssociationID = "NOOSE-BP" },
                    new LEConditionalLocation(new Vector3(-890.4263f, -2377.496f, 13.47698f), 70.6132f, 45f),
                    new LEConditionalLocation(new Vector3(-873.713f, -2419.414f, 13.94444f), 53.94463f, 45f) { AssociationID = "NOOSE-PIA" },
                } },
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
    }
    private void DefaultConfig_CityHalls()
    {
        CityHalls = new List<CityHall>()
        {
            new CityHall(new Vector3(-609.1187f, -87.8642f, 42.93483f), 247.5173f, "Rockford Hills City Hall", "") 
            {
                OpenTime = 9,
                CloseTime = 18, 
                CameraPosition = new Vector3(-591.665f, -98.11681f, 51.31879f), 
                CameraDirection = new Vector3(-0.9335647f, -0.06657825f, -0.3521709f), 
                CameraRotation = new Rotator(-20.62015f, -7.411738E-06f, 94.07921f),
                AssignedAssociationID = "SECURO",
                PossiblePedSpawns = new List<ConditionalLocation>() 
                {
                    new SecurityConditionalLocation(new Vector3(-605.2585f, -94.14611f, 42.93483f), 241.535f, 100f){ TaskRequirements = TaskRequirements.Guard | TaskRequirements.Patrol| TaskRequirements.StandardScenario, RequiredPedGroup = "UnarmedSecurity" },
                },
            },
            new CityHall(new Vector3(233.2825f, -411.1329f, 48.11194f), 338.9129f, "Los Santos City Hall", "") { OpenTime = 9,CloseTime = 18, CameraPosition = new Vector3(236.4192f, -348.1718f, 79.68157f), CameraDirection = new Vector3(0.0002627021f, -0.9024973f, -0.4306954f), CameraRotation = new Rotator(-25.5117f, 6.462261E-07f, -179.9833f)},
            new CityHall(new Vector3(329.4892f, -1580.714f, 32.79719f), 135.1704f, "Davis City Hall", "") { OpenTime = 9,CloseTime = 18, CameraPosition = new Vector3(273.4156f, -1601.353f, 42.17516f), CameraDirection = new Vector3(0.9521951f, 0.2629484f, -0.1555077f), CameraRotation = new Rotator(-8.946239f, -9.507168E-06f, -74.56252f)},
            new CityHall(new Vector3(-1286.212f, -566.4031f, 31.7124f), 314.9816f, "Del Perro City Hall", "") { OpenTime = 9,CloseTime = 18, CameraPosition = new Vector3(-1262.947f, -546.7318f, 43.99615f), CameraDirection = new Vector3(-0.7403942f, -0.5856928f, -0.3298186f), CameraRotation = new Rotator(-19.25776f, -1.085254E-05f, 128.3459f)},
        };
    }
    private void DefaultConfig_Residences()
    {
        Residences = new List<Residence>()
        {

                        //Apartments 

            //new Residence(new Vector3(-773.63f, 313.08f, 85.70f), 180f, "Eclipse Towers Apt 401",""){ InteriorID = -669,OpenTime = 0,CloseTime = 24,RentalFee = 30000, RentalDays = 28,PurchasePrice = 2000000,SalesPrice = 1500000,},
            new Residence(new Vector3(77.15f, -871.53f, 31.51f), 65f, "Elgin Ave Apt 340",""){ InteriorID = 108290,OpenTime = 0,CloseTime = 24,RentalFee = 2150, RentalDays = 28},
            new Residence(new Vector3(-390.3866f, -187.2812f, 37.3177f), 207.2874f, "70W Carcer Way Apt 343",""){ InteriorID = 108290,OpenTime = 0,CloseTime = 24,RentalFee = 1950, RentalDays = 28,},
            new Residence(new Vector3(76.05615f, -86.96131f, 63.00647f), 249.8842f, "1144 Apartment 2B", "") { InteriorID = 108290,OpenTime = 0,CloseTime = 24, PurchasePrice = 380000,SalesPrice = 290000, RentalDays = 28, RentalFee = 1660  },
            new Residence(new Vector3(-37.54279f, 170.3245f, 95.35922f), 303.8834f, "Elgin House Apartment 23E", "") { InteriorID = 108290, OpenTime = 0,CloseTime = 24, PurchasePrice = 404000,SalesPrice = 350000, RentalDays = 28, RentalFee = 1650  },
            new Residence(new Vector3(9.063264f, 52.93087f, 71.64354f), 344.3057f, "0605 Apartment 4F", "") { InteriorID = 108290,OpenTime = 0,CloseTime = 24, PurchasePrice = 375000,SalesPrice = 280000, RentalDays = 28, RentalFee = 1570  },
            new Residence(new Vector3(-780.21f, -784.22f, 27.87f), 85f, "Dream Tower Apt 31",""){ InteriorID = 108290,OpenTime = 0,CloseTime = 24,RentalFee = 1550, RentalDays = 28},
            new Residence(new Vector3(-116.35f, -37.69f, 62.20f), 70f, "Las Lagunas Blvd Apt 21",""){ InteriorID = 108290,OpenTime = 0,CloseTime = 24,RentalFee = 1550, RentalDays = 28},
            new Residence(new Vector3(-766.33f, -917.05f, 21.28f), 270f, "1068 Ginger St. Apt 4",""){ InteriorID = 108290,OpenTime = 0,CloseTime = 24,RentalFee = 1450, RentalDays = 28,PurchasePrice = 350000,SalesPrice = 300000,},
            new Residence(new Vector3(-131.3305f, -1665.597f, 32.56437f), 316.4919f, "20W Strawberry Apt 2", ""){ InteriorID = 60162,OpenTime = 0,CloseTime = 24,RentalFee = 1200, RentalDays = 28,},
            new Residence(new Vector3(-114.5114f, -1660.003f, 32.56433f), 52.34958f, "20W Strawberry Apt 1", ""){ InteriorID = 60162,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},






            //Apartment With Matching With Interior SP or BOTH!
            new Residence(new Vector3(-59.58021f, -617.4625f, 37.35678f), 69.24533f, "4 Integrity Way, Apt 28",""){
                InteriorID = 21250, OpenTime = 0,CloseTime = 24,RentalFee = 5000, RentalDays = 28,PurchasePrice = 1200000,SalesPrice = 980000,HasHeaderApartmentBuilding = true,ResidenceID = 1, IsOnMPMap = false
            , CameraPosition = new Vector3(-103.6068f, -621.355f, 61.43004f), CameraDirection = new Vector3(0.9355325f, 0.094497f, -0.3403665f), CameraRotation = new Rotator(-19.89921f, -2.269968E-07f, -84.23219f)},

            new Residence(new Vector3(-59.58021f, -617.4625f, 37.35678f), 69.24533f, "4 Integrity Way, Apt 30",""){
                InteriorID = 47362, OpenTime = 0,CloseTime = 24,RentalFee = 5000, RentalDays = 28,PurchasePrice = 1200000,SalesPrice = 980000,HasHeaderApartmentBuilding = true,ResidenceID = 2, IsOnMPMap = false
            , CameraPosition = new Vector3(-103.6068f, -621.355f, 61.43004f), CameraDirection = new Vector3(0.9355325f, 0.094497f, -0.3403665f), CameraRotation = new Rotator(-19.89921f, -2.269968E-07f, -84.23219f)},

            new Residence(new Vector3(-1442.846f, -544.5376f, 34.74181f), 213.3763f, "Del Perro Heights, Apt 4",""){
                InteriorID = 76290, OpenTime = 0,CloseTime = 24,RentalFee = 5000, RentalDays = 28,PurchasePrice = 1200000,SalesPrice = 980000,HasHeaderApartmentBuilding = true,ResidenceID = 3, IsOnMPMap = false },

            new Residence(new Vector3(-1442.846f, -544.5376f, 34.74181f), 213.3763f, "Del Perro Heights, Apt 7",""){
                InteriorID = -673, OpenTime = 0,CloseTime = 24,RentalFee = 5000, RentalDays = 28,PurchasePrice = 1200000,SalesPrice = 980000,HasHeaderApartmentBuilding = true,ResidenceID = 4, IsOnMPMap = false, },

            new Residence(new Vector3(-773.63f, 313.08f, 85.70f), 180f, "Eclipse Towers, Apt 3",""){
                InteriorID = 61186, IsOnMPMap = false, OpenTime = 0,CloseTime = 24,RentalFee = 5000, RentalDays = 28,PurchasePrice = 1200000,SalesPrice = 980000,HasHeaderApartmentBuilding = true,ResidenceID = 5 },
            //new Residence(new Vector3(-773.63f, 313.08f, 85.70f), 180f, "Eclipse Towers, Apt 3",""){
            //    InteriorID = -674, IsOnSPMap = false, OpenTime = 0,CloseTime = 24,RentalFee = 5000, RentalDays = 28,PurchasePrice = 1200000,SalesPrice = 980000,HasHeaderApartmentBuilding = true,ResidenceID = 6 },

            new Residence(new Vector3(-936.8324f, -379.2408f, 38.96133f), 113.6606f, "Richard Majestic, Apt 2",""){
                InteriorID = -675, OpenTime = 0,CloseTime = 24,RentalFee = 5000, RentalDays = 28,PurchasePrice = 1200000,SalesPrice = 980000,HasHeaderApartmentBuilding = true,ResidenceID = 7 },
            //
            new Residence(new Vector3(-593.1158f, 37.89521f, 43.60775f), 179.0897f, "Tinsel Towers, Apt 42",""){
                InteriorID = 90882, IsOnMPMap = false, OpenTime = 0,CloseTime = 24,RentalFee = 5000, RentalDays = 28,PurchasePrice = 1200000,SalesPrice = 980000,HasHeaderApartmentBuilding = true,ResidenceID = 8 },






            //NEW
            //Vespucci
            new Residence(new Vector3(-1035.18f, -1146.53f, 2.16f), 32.45f, "68 Vespucci Canals", "") { InteriorID = 108290, OpenTime = 0,CloseTime = 24, PurchasePrice = 450000, SalesPrice = 350000, RentalDays = 28, RentalFee = 2400  },
            new Residence(new Vector3(-960.14f, -1109.21f, 2.15f), 33.94f, "87 Vespucci Canals", "") { InteriorID = 108290,OpenTime = 0,CloseTime = 24, PurchasePrice = 380000, SalesPrice = 300000,RentalDays = 28, RentalFee = 2000  },
            new Residence(new Vector3(-951.51f, -1078.93f, 2.15f), 207.97f, "88 Vespucci Canals", "") { InteriorID = 108290,OpenTime = 0,CloseTime = 24, PurchasePrice = 490000,SalesPrice = 350000, RentalDays = 28, RentalFee = 2500  },
            new Residence(new Vector3(-1183.19f, -1064.55f, 2.15f), 114.64f, "42 Vespucci Canals", "") { InteriorID = 108290,OpenTime = 0,CloseTime = 24, PurchasePrice = 400000,SalesPrice = 300000, RentalDays = 28, RentalFee = 2000  },
            new Residence(new Vector3(-983.25f, -1065.88f, 2.15f), 30.08f, "86 Vespucci Canals", "") { InteriorID = 108290,OpenTime = 0,CloseTime = 24, PurchasePrice = 440000,SalesPrice = 300000, RentalDays = 28, RentalFee = 2300  },
            new Residence(new Vector3(-1064.27f, -1159.15f, 2.16f), 31.65f, "65 Vespucci Canals", "") { InteriorID = 108290,OpenTime = 0,CloseTime = 24, PurchasePrice = 400000,SalesPrice = 300000, RentalDays = 28, RentalFee = 2000  },
            
            //Mirror Park
            new Residence(new Vector3(987.9448f, -525.9558f, 60.69062f), 208.7025f, "114 Nikola Ave", "") { InteriorID = 55042, OpenTime = 0,CloseTime = 24, PurchasePrice = 390000,SalesPrice = 310000, RentalDays = 28, RentalFee = 2000  },
            new Residence(new Vector3(976.0329f, -579.8205f, 59.63558f), 30.60654f, "115 Nikola Ave", "") { InteriorID = 55042,OpenTime = 0,CloseTime = 24, PurchasePrice = 360000,SalesPrice = 300000, RentalDays = 28, RentalFee = 2000  },
            new Residence(new Vector3(1323.468f, -583.0522f, 73.24638f), 334.7151f, "104 East Mirror Dr", "") { InteriorID = 55042,OpenTime = 0,CloseTime = 24, PurchasePrice = 480000,SalesPrice = 360000, RentalDays = 28, RentalFee = 2500  },
            new Residence(new Vector3(1372.544f, -555.1687f, 74.68565f), 153.1011f, "107 East Mirror Dr", "") { InteriorID = 55042,OpenTime = 0,CloseTime = 24, PurchasePrice = 500000,SalesPrice = 380000, RentalDays = 28, RentalFee = 2600  },
            new Residence(new Vector3(999.5336f, -593.9052f, 59.63857f), 259.7758f, "93 Mirror Pl", "") { InteriorID = 55042,OpenTime = 0,CloseTime = 24, PurchasePrice = 270000,SalesPrice = 210000, RentalDays = 28, RentalFee = 1500  },
            new Residence(new Vector3(979.8713f, -627.104f, 59.23589f), 124.2795f, "218 West Mirror Dr", "") { InteriorID = 55042,OpenTime = 0,CloseTime = 24, PurchasePrice = 400000,SalesPrice = 350000, RentalDays = 28, RentalFee = 3000  },

            new Residence(new Vector3(1060.462f, -378.1198f, 68.23125f), 229.2095f, "112 Bridge St", "") { InteriorID = 55042,OpenTime = 0,CloseTime = 24, PurchasePrice = 350000,SalesPrice = 310000, RentalDays = 28, RentalFee = 2000  },
            new Residence(new Vector3(1051.598f, -470.5955f, 63.91349f), 257.9751f, "118 Bridge St", "") { InteriorID = 55042,OpenTime = 0,CloseTime = 24, PurchasePrice = 335000,SalesPrice = 300000, RentalDays = 28, RentalFee = 1700  },
            new Residence(new Vector3(1090.538f, -484.3305f, 65.66045f), 76.35575f, "117 Bridge St", "") { InteriorID = 55042,OpenTime = 0,CloseTime = 24, PurchasePrice = 400000,SalesPrice = 350000, RentalDays = 28, RentalFee = 3000  },

            //Apartments
            new Residence(new Vector3(-1150.072f, -1521.705f, 10.62806f), 225.8192f, "7611 Goma St", "") { DisableInteractAfterPurchase = true, OpenTime = 0,CloseTime = 24, InteriorID = 24578, PurchasePrice = 550000,SalesPrice = 390000, RentalDays = 28, RentalFee = 2250 },

            //House
            new Residence(new Vector3(983.7811f, 2718.655f, 39.50342f), 175.7726f, "345 Route 68", "") { InteriorID = 55042,OpenTime = 0,CloseTime = 24, PurchasePrice = 234000,SalesPrice = 190000, RentalDays = 28, RentalFee = 1400  },
            new Residence(new Vector3(980.1745f, 2666.563f, 40.04342f), 1.699184f, "140 Route 68", "") { InteriorID = 55042,OpenTime = 0,CloseTime = 24, PurchasePrice = 225000,SalesPrice = 180000, RentalDays = 28, RentalFee = 1250  },
            new Residence(new Vector3(-3030.045f, 568.7484f, 7.823583f), 280.3508f, "566 Ineseno Rd", "") { InteriorID = 108290,OpenTime = 0,CloseTime = 24, PurchasePrice = 556000,SalesPrice = 320000, RentalDays = 28, RentalFee = 2200 },
            new Residence(new Vector3(-3031.865f, 525.1087f, 7.424246f), 267.6694f, "800 Ineseno Rd", "") { InteriorID = 108290,OpenTime = 0,CloseTime = 24, PurchasePrice = 467000,SalesPrice = 310000, RentalDays = 28, RentalFee = 1850  },
            new Residence(new Vector3(-3039.567f, 492.8512f, 6.772703f), 226.448f, "805 Ineseno Rd", "") { InteriorID = 108290,OpenTime = 0,CloseTime = 24, PurchasePrice = 450000,SalesPrice = 350000, RentalDays = 28, RentalFee = 1750  },
            new Residence(new Vector3(195.0935f, 3031.064f, 43.89068f), 273.1078f, "125 Joshua Rd", "") { InteriorID = 55042,OpenTime = 0,CloseTime = 24, PurchasePrice = 298000,SalesPrice = 210000, RentalDays = 28, RentalFee = 1580  },
            new Residence(new Vector3(191.0053f, 3082.26f, 43.47285f), 277.6117f, "610N Joshua Rd", "") { InteriorID = 55042,OpenTime = 0,CloseTime = 24, PurchasePrice = 289000,SalesPrice = 200000, RentalDays = 28, RentalFee = 1675  },
            new Residence(new Vector3(241.7666f, 3107.666f, 42.48719f), 93.76467f, "620N Joshua Rd", "") { InteriorID = 55042,OpenTime = 0,CloseTime = 24, PurchasePrice = 275000,SalesPrice = 190000, RentalDays = 28, RentalFee = 1610  },
            //new Residence(new Vector3(162.8214f, 3119.749f, 43.42594f), 192.0786f, "621N Joshua Rd", "") { InteriorID = 108290, OpenTime = 0,CloseTime = 24, PurchasePrice = 57000,SalesPrice = 35000, RentalDays = 28, RentalFee = 1550  },
            new Residence(new Vector3(247.5913f, 3169.535f, 42.78756f), 90.61945f, "630N Joshua Rd", "") { InteriorID = 55042,OpenTime = 0,CloseTime = 24, PurchasePrice = 267000, RentalDays = 28, RentalFee = 1580  },
            new Residence(new Vector3(-272.7155f, 6400.906f, 31.50496f), 215.1084f, "1275N Procopio Dr", "") { InteriorID = 55042,OpenTime = 0,CloseTime = 24, PurchasePrice = 470000,SalesPrice = 350000, RentalDays = 28, RentalFee = 1850  },
            new Residence(new Vector3(-247.7424f, 6370.079f, 31.84554f), 45.0573f, "1276N Procopio Dr", "") { InteriorID = 55042,OpenTime = 0,CloseTime = 24, PurchasePrice = 476000,SalesPrice = 350000, RentalDays = 28, RentalFee = 1790  },
            new Residence(new Vector3(-227.3544f, 6377.188f, 31.75924f), 47.93699f, "1278N Procopio Dr", "") { InteriorID = 55042,OpenTime = 0,CloseTime = 24, PurchasePrice = 414000,SalesPrice = 320000, RentalDays = 28, RentalFee = 1670  },
            new Residence(new Vector3(-245.4691f, 6413.878f, 31.4606f), 130.3203f, "1281N Procopio Dr", "") { InteriorID = 55042,OpenTime = 0,CloseTime = 24, PurchasePrice = 455000,SalesPrice = 330000, RentalDays = 28, RentalFee = 1770  },
            new Residence(new Vector3(-213.7807f, 6395.94f, 33.08509f), 40.99633f, "1282N Procopio Dr", "") { InteriorID = 55042,OpenTime = 0,CloseTime = 24, PurchasePrice = 435000,SalesPrice = 350000, RentalDays = 28, RentalFee = 1680  },
            new Residence(new Vector3(-189.0498f, 6409.355f, 32.29676f), 48.98f, "1285N Procopio Dr", "") { InteriorID = 55042,OpenTime = 0,CloseTime = 24, PurchasePrice = 445000,SalesPrice = 350000, RentalDays = 28, RentalFee = 1650  },
            new Residence(new Vector3(-280.4388f, 6350.718f, 32.60079f), 24.08426f, "1271N Procopio Dr", "") { InteriorID = 55042,OpenTime = 0,CloseTime = 24, PurchasePrice = 247000,SalesPrice = 210000, RentalDays = 28, RentalFee = 1380  },
            new Residence(new Vector3(-359.5872f, 6334.424f, 29.84736f), 226.0376f, "1260N Procopio Dr", "") { InteriorID = 108290,OpenTime = 0,CloseTime = 24, PurchasePrice = 565000,SalesPrice = 450000, RentalDays = 28, RentalFee = 2200  },
            new Residence(new Vector3(-332.7523f, 6301.959f, 33.08874f), 69.82259f, "1262N Procopio Dr", "") { InteriorID = 55042,OpenTime = 0,CloseTime = 24, PurchasePrice = 257000,SalesPrice = 240000, RentalDays = 28, RentalFee = 1480  },
            new Residence(new Vector3(-407.2561f, 6314.223f, 28.94128f), 230.5133f, "1259N Procopio Dr", "") { InteriorID = 108290,OpenTime = 0,CloseTime = 24, PurchasePrice = 414000,SalesPrice = 390000, RentalDays = 28, RentalFee = 1870  },
            new Residence(new Vector3(-437.4564f, 6261.807f, 30.06895f), 228.8042f, "1252N Procopio Dr", "") { InteriorID = 108290,OpenTime = 0,CloseTime = 24, PurchasePrice = 520000,SalesPrice = 410000, RentalDays = 28, RentalFee = 2100  },
            new Residence(new Vector3(1880.423f, 3921.08f, 33.21722f), 100.0703f, "785N Niland Ave", "") { InteriorID = 55042,OpenTime = 0,CloseTime = 24, PurchasePrice = 224000,SalesPrice = 210000, RentalDays = 28, RentalFee = 1280  },

            new Residence(new Vector3(1683.038f, 4689.635f, 43.06602f), 269.7168f, "330E Grapeseed", ""){ InteriorID = 55042,OpenTime = 0,CloseTime = 24,PurchasePrice = 200000,SalesPrice = 150000, RentalFee = 1200, RentalDays = 28,},
            new Residence(new Vector3(1674.201f, 4658.228f, 43.37114f), 270.4372f, "335E Grapeseed", ""){ InteriorID = 55042,OpenTime = 0,CloseTime = 24,PurchasePrice = 200000,SalesPrice = 150000, RentalFee = 1200, RentalDays = 28,},
            new Residence(new Vector3(1725.289f, 4642.35f, 43.87548f), 115.7792f, "340E Grapeseed", ""){ InteriorID = 55042,OpenTime = 0,CloseTime = 24,PurchasePrice = 250000,SalesPrice = 150000, RentalFee = 1350, RentalDays = 28,},
            new Residence(new Vector3(1881.219f, 3810.713f, 32.77882f), 301.1151f, "370E Niland ", ""){ InteriorID = 55042,OpenTime = 0,CloseTime = 24,PurchasePrice = 150000,SalesPrice = 90000, RentalFee = 950, RentalDays = 28,},
            new Residence(new Vector3(1899.071f, 3781.417f, 32.8769f), 299.5774f, "380E Niland", ""){ InteriorID = 55042,OpenTime = 0,CloseTime = 24,PurchasePrice = 175000,SalesPrice = 150000, RentalFee = 1000, RentalDays = 28,},
            new Residence(new Vector3(-3186.93f, 1274.122f, 12.66124f), 257.7415f, "630W Barbareno", ""){ InteriorID = 108290,OpenTime = 0,CloseTime = 24,PurchasePrice = 750000,SalesPrice = 650000, RentalFee = 3000, RentalDays = 28,},
            new Residence(new Vector3(-3194.785f, 1179.539f, 9.659509f), 258.9255f, "632W Barbareno", ""){ InteriorID = 108290,OpenTime = 0,CloseTime = 24,PurchasePrice = 650000,SalesPrice = 550000, RentalFee = 2700, RentalDays = 28,},
            new Residence(new Vector3(-3238.444f, 952.556f, 13.34334f), 281.2452f, "642W Barbareno", ""){ InteriorID = 108290,OpenTime = 0,CloseTime = 24,PurchasePrice = 670000,SalesPrice = 550000, RentalFee = 2800, RentalDays = 28,},
            new Residence(new Vector3(-3093.812f, 349.4576f, 7.544847f), 258.988f, "611W Ineseno", ""){ InteriorID = 108290,OpenTime = 0,CloseTime = 24,PurchasePrice = 1200000,SalesPrice = 980000, RentalFee = 4000, RentalDays = 28,},
            new Residence(new Vector3(312.721f, -1956.719f, 24.42436f), 229.223f, "61E Jamestown ", ""){ InteriorID = 55042,OpenTime = 0,CloseTime = 24,PurchasePrice = 330000,SalesPrice = 280000, RentalFee = 1400, RentalDays = 28,},
            new Residence(new Vector3(256.5888f, -2023.559f, 19.26631f), 227.9232f, "50E Jamestown", ""){ InteriorID = 55042,OpenTime = 0,CloseTime = 24,PurchasePrice = 400000,SalesPrice = 320000, RentalFee = 1800, RentalDays = 28,},
            new Residence(new Vector3(165.2183f, -1944.738f, 20.23543f), 228.8544f, "30E Roy Lowenstein", ""){ InteriorID = 55042,OpenTime = 0,CloseTime = 24,PurchasePrice = 425000,SalesPrice = 330000, RentalFee = 1870, RentalDays = 28,},
            new Residence(new Vector3(150.2098f, -1864.915f, 24.59134f), 153.5939f, "20E Covenant", ""){ InteriorID = 55042,OpenTime = 0,CloseTime = 24, PurchasePrice = 250000,SalesPrice = 190000, RentalFee = 1300, RentalDays = 28,},
            new Residence(new Vector3(114.7368f, -1887.42f, 23.92823f), 333.4948f, "21E Covenant", ""){ InteriorID = 55042,OpenTime = 0,CloseTime = 24,PurchasePrice = 275000,SalesPrice = 180000, RentalFee = 1000, RentalDays = 28,},
            new Residence(new Vector3(54.01227f, -1873.436f, 22.80583f), 128.9813f, "10E Grove Street", ""){ InteriorID = 55042,OpenTime = 0,CloseTime = 24, PurchasePrice = 225000,SalesPrice = 170000, RentalFee = 1200, RentalDays = 28,},
            new Residence(new Vector3(-5.184085f, -1871.247f, 24.15101f), 317.3572f, "3E Grove St", ""){ InteriorID = 55042,OpenTime = 0,CloseTime = 24, PurchasePrice = 325000,SalesPrice = 270000, RentalFee = 1500, RentalDays = 28,},

            new Residence(new Vector3(-64.19335f, -1450.097f, 32.52492f), 188.7294f, "280S Forum Dr No 1",""){ InteriorID = 55042,OpenTime = 0,CloseTime = 24,RentalFee = 1250, RentalDays = 28,},//lamar house
            new Residence(new Vector3(863.4708f, -1585.503f, 31.4171f), 89.42107f, "310S Popular Street",""){ InteriorID = 60162,OpenTime = 0,CloseTime = 24,RentalFee = 950, RentalDays = 28,},//lamar house

            new Residence(new Vector3(-1597.134f, -352.3153f, 45.97645f), 230.207f, "Pacific Buff Manor",""){ InteriorID = 55042,OpenTime = 0,CloseTime = 24,RentalFee = 5000, RentalDays = 28,PurchasePrice = 1200000,SalesPrice = 980000,},

            //from box in discord
            new Residence(new Vector3(-188.97f, 1008.84f, 232.13f), 87f, "202 Lake Vinewood Estate",""){ InteriorID = 55042,OpenTime = 0,CloseTime = 24,RentalFee = 12000, RentalDays = 28,PurchasePrice = 1550000,SalesPrice = 1200000,},
            new Residence(new Vector3(-232.71f, 588.12f, 190.54f), 355f, "3636 Kimble Hill Dr",""){ InteriorID = 55042,OpenTime = 0,CloseTime = 24,RentalFee = 9100, RentalDays = 28,PurchasePrice = 1250000,SalesPrice = 900000,},
            new Residence(new Vector3(-700.87f, 649.99f, 155.38f), 350f, "4057 Hillcrest Ave",""){ InteriorID = 55042,OpenTime = 0,CloseTime = 24,RentalFee = 8500, RentalDays = 28,PurchasePrice = 1100000,SalesPrice = 980000,},
            new Residence(new Vector3(-1009.54f, 479.05f, 79.60f), 330f, "2125 Cockingend Dr",""){ InteriorID = 108290,OpenTime = 0,CloseTime = 24,RentalFee = 14500, RentalDays = 28,PurchasePrice = 1800000,SalesPrice = 1200000,},
            new Residence(new Vector3(1289.23f, -1710.51f, 55.48f), 205f, "412 El Burro Heights",""){ InteriorID = 108290,OpenTime = 0,CloseTime = 24,RentalFee = 1200, RentalDays = 28,PurchasePrice = 200000,SalesPrice = 180000,},
            new Residence(new Vector3(-1243.89f, -1241.10f, 11.03f), 20f, "1533 Magellan Ave",""){ InteriorID = 60162,OpenTime = 0,CloseTime = 24,RentalFee = 5000, RentalDays = 28},

            //MP Houses with Interiors
            new Residence(new Vector3(346.3378f,440.5709f, 147.8333f),292.537f,"2044 North Conker Avenue","") { InteriorID = 206081, IsOnSPMap = false,OpenTime = 0,CloseTime = 24,RentalFee = 5000, RentalDays = 28,PurchasePrice = 762000, SalesPrice = 381000, },
            new Residence(new Vector3(373.9571f,427.8646f, 145.6842f),41.48535f,"2045 North Conker Avenue","") { InteriorID = 206337, IsOnSPMap = false,OpenTime = 0,CloseTime = 24,RentalFee = 5000, RentalDays = 28,PurchasePrice = 727000, SalesPrice = 363500, },
            new Residence(new Vector3(-174.7182f,502.512f, 137.4203f),79.66186f,"3655 Wild Oats Drive","") { InteriorID = 207105, IsOnSPMap = false,OpenTime = 0,CloseTime = 24,RentalFee = 5000, RentalDays = 28,PurchasePrice = 800000, SalesPrice = 400000, },

            new Residence() {
                RentalDays = 28,
                RentalFee = 5000,
                PurchasePrice = 1800000,
                SalesPrice = 1100000,
                Name = "3677 Whispymound Drive",
                EntrancePosition = new Vector3(119.3867f,563.9431f,183.9595f),
                EntranceHeading = 4.633287f,
                OpenTime = 0,
                CloseTime = 24,
                InteriorID = 206593,
                IsOnSPMap = false,
                },
            new Residence()
            {
                RentalDays = 28,
                RentalFee = 5000,
                PurchasePrice = 571000,
                SalesPrice = 285500,
                Name = "2874 Hillcrest Avenue",
                FullName = "2874 Hillcrest Avenue",
                EntrancePosition = new Vector3(-853.1187f, 695.2803f, 148.7875f),
                EntranceHeading = 0.8268144f,
                OpenTime = 0,
                CloseTime = 24,
                InteriorID = 207361,
                IsOnSPMap = false,
            },
            new Residence()
            {
                RentalDays = 28,
                RentalFee = 5000,
                PurchasePrice = 672000,
                SalesPrice = 336000,
                Name = "2868 Hillcrest Avenue",
                FullName = "2868 Hillcrest Avenue",
                EntrancePosition = new Vector3(-753.1683f, 620.38f, 142.7604f),
                EntranceHeading = 281.9315f,
                OpenTime = 0,
                CloseTime = 24,
                InteriorID = 207617,
                IsOnSPMap = false,
            },
            new Residence()
            {
                RentalDays = 28,
                RentalFee = 5000,
                PurchasePrice = 525000,
                SalesPrice = 262500,
                Name = "2866 Hillcrest Avenue",
                FullName = "2866 Hillcrest Avenue",
                EntrancePosition = new Vector3(-733.7498f, 592.4382f, 142.526f),
                EntranceHeading = 332.856f,
                OpenTime = 0,
                CloseTime = 24,
                InteriorID = 208641,
                IsOnSPMap = false,
            },
            new Residence()
            {
                RentalDays = 28,
                RentalFee = 5000,
                PurchasePrice = 705000,
                SalesPrice = 352500,
                Name = "2862 Hillcrest Avenue",
                FullName = "2862 Hillcrest Avenue",
                EntrancePosition = new Vector3(373.9571f, 427.8646f, 145.6842f),
                EntranceHeading = 41.48535f,
                OpenTime = 0,
                CloseTime = 24,
                InteriorID = 208129,
                IsOnSPMap = false,
            },
            new Residence()
            {
                RentalDays = 28,
                RentalFee = 5000,
                PurchasePrice = 608000,
                SalesPrice = 304000,
                Name = "2117 Milton Road",
                FullName = "2117 Milton Road",
                EntrancePosition = new Vector3(-559.8577f, 662.7262f, 145.4829f),
                EntranceHeading = 349.0018f,
                OpenTime = 0,
                CloseTime = 24,
                InteriorID = 207873,
                IsOnSPMap = false,
            },
            new Residence()
            {
                RentalDays = 28,
                RentalFee = 5000,
                PurchasePrice = 449000,
                SalesPrice = 224500,
                Name = "2113 Mad Wayne Thunder Drive",
                FullName = "2113 Mad Wayne Thunder Drive",
                EntrancePosition = new Vector3(-1294.153f, 453.1571f, 97.64121f),
                EntranceHeading = 1.065937f,
                OpenTime = 0,
                CloseTime = 24,
                InteriorID = 208385,
                IsOnSPMap = false,
            },


            //More MP Apartments
            new Residence(new Vector3(-614.8517f, 37.52992f, 43.58273f), 177.6369f, "Tinsel Towers, Apt 42","")
            { 
                InteriorID = 146689, 
                IsOnSPMap = false, 
                OpenTime = 0,
                CloseTime = 24,
                RentalFee = 5000, 
                RentalDays = 28,
                PurchasePrice = 1200000,
                SalesPrice = 980000,
                HasHeaderApartmentBuilding = true,
                ResidenceID = 9 
            },
            new Residence(new Vector3(-614.8517f, 37.52992f, 43.58273f), 177.6369f,"Tinsel Towers, Apt 45", "") 
            {
              RentalDays = 28,
              RentalFee = 5000,
              PurchasePrice = 270000,
              SalesPrice = 135000,
              HasHeaderApartmentBuilding = true,
              EntrancePosition = new Vector3(-614.8517f, 37.52992f, 43.58273f),
              EntranceHeading = 177.6369f,
              OpenTime = 0,
              CloseTime = 24,
              InteriorID = 144897,
              IsOnSPMap = false,
              ResidenceID = 10,
            },
            new Residence(new Vector3(-593.1158f, 37.89521f, 43.60775f), 179.0897f, "Tinsel Towers, Apt 29", "") {
                RentalDays = 28,
                RentalFee = 5000,
                PurchasePrice = 286000,
                SalesPrice = 143000,
                HasHeaderApartmentBuilding = true,
                EntrancePosition = new Vector3(-593.1158f, 37.89521f, 43.60775f),
                EntranceHeading = 179.0897f,
                OpenTime = 0,
                CloseTime = 24,
                InteriorID = 145153,
                ResidenceID = 11,
            },

            //Weasel Plaza
            new Residence(new Vector3(-913.8973f, -455.6648f, 39.59985f), 116.4933f,"Weazel Plaza Apartment 101", "") {
              RentalDays = 28,
              RentalFee = 5000,
              PurchasePrice = 335000,
              SalesPrice = 167500,
              HasHeaderApartmentBuilding = true,
              Name = "Weasel Plaza Apartment 101",
              EntrancePosition = new Vector3(-913.8973f, -455.6648f, 39.59985f),
              EntranceHeading = 116.4933f,
              OpenTime = 0,
              CloseTime = 24,
              InteriorID = 143617,
              IsOnSPMap = false,
              ResidenceID = 12,
            },
            new Residence(new Vector3(-913.8973f, -455.6648f, 39.59985f), 116.4933f,"Weazel Plaza Apartment 26", "") {
              RentalDays = 28,
              RentalFee = 5000,
              PurchasePrice = 304000,
              SalesPrice = 152000,
              HasHeaderApartmentBuilding = true,
              Name = "Weasel Plaza Apartment 26",
              EntrancePosition = new Vector3(-913.8973f, -455.6648f, 39.59985f),
              EntranceHeading = 116.4933f,
              OpenTime = 0,
              CloseTime = 24,
              InteriorID = 142593,
              IsOnSPMap = false,
              ResidenceID = 13,
            },
            new Residence(new Vector3(-913.8973f, -455.6648f, 39.59985f), 116.4933f,"Weazel Plaza Apartment 70", "") {
              RentalDays = 28,
              RentalFee = 5000,
              PurchasePrice = 319000,
              SalesPrice = 159500,
              HasHeaderApartmentBuilding = true,
              Name = "Weasel Plaza Apartment 70",
              EntrancePosition = new Vector3(-913.8973f, -455.6648f, 39.59985f),
              EntranceHeading = 116.4933f,
              OpenTime = 0,
              CloseTime = 24,
              InteriorID = 143105,
              IsOnSPMap = false,
              ResidenceID = 14,
            },

            //Eclipse Towers
            new Residence (new Vector3 (-773.63f, 313.08f, 85.7f), 180f,"Eclipse Towers, Apt 3", "") 
            {
              RentalDays = 28,
              RentalFee = 5000,
              PurchasePrice = 1200000,
              SalesPrice = 980000,
              ResidenceID = 21,
              HasHeaderApartmentBuilding = true,
              Name = "Eclipse Towers, Apt 3",
              EntrancePosition = new Vector3 (-773.63f, 313.08f, 85.7f),
              EntranceHeading = 180f,
              OpenTime = 0,
              CloseTime = 24,
              InteriorID = 300313503,
              IsOnSPMap = false,
            },
            new Residence (new Vector3 (-773.63f, 313.08f, 85.7f), 180f,"Eclipse Towers, Apt 31", "") 
            {
              RentalDays = 28,
              RentalFee = 5000,
              PurchasePrice = 1200000,
              SalesPrice = 980000,
              ResidenceID = 24,
              HasHeaderApartmentBuilding = true,
              Name = "Eclipse Towers, Apt 31",
              EntrancePosition = new Vector3 (-773.63f, 313.08f, 85.7f),
              EntranceHeading = 180f,
              OpenTime = 0,
              CloseTime = 24,
              InteriorID = 144385,
              IsOnSPMap = false,
            },
            new Residence (new Vector3 (-773.63f, 313.08f, 85.7f), 180f,"Eclipse Towers, Apt 40", "") {
              RentalDays = 28,
              RentalFee = 5000,
              PurchasePrice = 1200000,
              SalesPrice = 980000,
              ResidenceID = 25,
              HasHeaderApartmentBuilding = true,
              Name = "Eclipse Towers, Apt 40",
              EntrancePosition = new Vector3 (-773.63f, 313.08f, 85.7f),
              EntranceHeading = 180f,
              OpenTime = 0,
              CloseTime = 24,
              InteriorID = 144129,
              IsOnSPMap = false,
            },
            new Residence (new Vector3 (-773.63f, 313.08f, 85.7f), 180f,"Eclipse Towers, Apt 5", "") {
              RentalDays = 28,
              RentalFee = 5000,
              PurchasePrice = 1200000,
              SalesPrice = 980000,
              ResidenceID = 22,
              HasHeaderApartmentBuilding = true,
              Name = "Eclipse Towers, Apt 5",
              EntrancePosition = new Vector3 (-773.63f, 313.08f, 85.7f),
              EntranceHeading = 180f,
              OpenTime = 0,
              CloseTime = 24,
              InteriorID = 300313505,
              IsOnSPMap = false,
            },
            new Residence (new Vector3 (-773.63f, 313.08f, 85.7f), 180f,"Eclipse Towers, Apt 9", "") {
              RentalDays = 28,
              RentalFee = 5000,
              PurchasePrice = 1200000,
              SalesPrice = 980000,
              ResidenceID = 23,
              HasHeaderApartmentBuilding = true,
              Name = "Eclipse Towers, Apt 9",
              EntrancePosition = new Vector3 (-773.63f, 313.08f, 85.7f),
              EntranceHeading = 180f,
              OpenTime = 0,
              CloseTime = 24,
              InteriorID = 144641,
              IsOnSPMap = false,
            },
            new Residence (new Vector3 (-773.63f, 313.08f, 85.7f), 180f,"Eclipse Towers Penthouse 1", "") {
              RentalDays = 28,
              RentalFee = 5000,
              PurchasePrice = 1200000,
              SalesPrice = 980000,
              ResidenceID = 18,
              HasHeaderApartmentBuilding = true,
              Name = "Eclipse Towers Penthouse 1",
              EntrancePosition = new Vector3 (-773.63f, 313.08f, 85.7f),
              EntranceHeading = 180f,
              OpenTime = 0,
              CloseTime = 24,
              InteriorID = 227329,
              IsOnSPMap = false,
            },
            new Residence (new Vector3 (-773.63f, 313.08f, 85.7f), 180f,"Eclipse Towers Penthouse 2", "") 
            {
              RentalDays = 28,
              RentalFee = 5000,
              PurchasePrice = 1200000,
              SalesPrice = 980000,
              ResidenceID = 19,
              HasHeaderApartmentBuilding = true,
              Name = "Eclipse Towers Penthouse 2",
              EntrancePosition = new Vector3 (-773.63f, 313.08f, 85.7f),
              EntranceHeading = 180f,
              OpenTime = 0,
              CloseTime = 24,
              InteriorID = 229889,
              IsOnSPMap = false,
            },
            new Residence (new Vector3 (-773.63f, 313.08f, 85.7f), 180f,"Eclipse Towers Penthouse 3", "") 
            {
              RentalDays = 28,
              RentalFee = 5000,
              PurchasePrice = 1200000,
              SalesPrice = 980000,
              ResidenceID = 20,
              HasHeaderApartmentBuilding = true,
              Name = "Eclipse Towers Penthouse 3",
              EntrancePosition = new Vector3 (-773.63f, 313.08f, 85.7f),
              EntranceHeading = 180f,
              OpenTime = 0,
              CloseTime = 24,
              InteriorID = 230913,
              IsOnSPMap = false,
            },

            //Del Perro Heights MP
            new Residence(new Vector3(-1442.846f,-544.5376f,34.74181f),213.3763f,"Del Perro Heights Apt 4","")
            {
                RentalDays = 28,
                RentalFee = 5000,
                PurchasePrice = 1200000,
                SalesPrice = 980000,
                ResidenceID = 40,
                HasHeaderApartmentBuilding = true,
                Name = "Del Perro Heights Apt 4",
                EntrancePosition = new Vector3(-1442.846f,-544.5376f,34.74181f),
                EntranceHeading = 213.3763f,
                OpenTime = 0,
                CloseTime = 24,
                InteriorID = 145665,
                IsOnSPMap = false,
            },
            new Residence(new Vector3(-1442.846f,-544.5376f,34.74181f),213.3763f,"Del Perro Heights Apt 20","")
            {
                RentalDays = 28,
                RentalFee = 5000,
                PurchasePrice = 1200000,
                SalesPrice = 980000,
                ResidenceID = 41,
                HasHeaderApartmentBuilding = true,
                Name = "Del Perro Heights Apt 20",
                EntrancePosition = new Vector3(-1442.846f,-544.5376f,34.74181f),
                EntranceHeading = 213.3763f,
                OpenTime = 0,
                CloseTime = 24,
                InteriorID = 145409,
                IsOnSPMap = false,
            },
            //Alta Stret MP
            new Residence(new Vector3(-261.9663f,-970.8594f,31.21996f),203.518f,"3 Alta Street Apt 57","")
            {
                RentalDays = 28,
                RentalFee = 5000,
                PurchasePrice = 1200000,
                SalesPrice = 980000,
                ResidenceID = 42,
                HasHeaderApartmentBuilding = true,
                Name = "3 Alta Street Apt 57",
                EntrancePosition = new Vector3(-261.9663f,-970.8594f,31.21996f),
                EntranceHeading = 203.518f,
                OpenTime = 0,
                CloseTime = 24,
                InteriorID = 141569,
                IsOnSPMap = false,
            },
            new Residence(new Vector3(-261.9663f,-970.8594f,31.21996f),203.518f,"3 Alta Street Apt 10","")
            {
                RentalDays = 28,
                RentalFee = 5000,
                PurchasePrice = 1200000,
                SalesPrice = 980000,
                ResidenceID = 43,
                HasHeaderApartmentBuilding = true,
                Name = "3 Alta Street Apt 10",
                EntrancePosition = new Vector3(-261.9663f,-970.8594f,31.21996f),
                EntranceHeading = 203.518f,
                OpenTime = 0,
                CloseTime = 24,
                InteriorID = 141825,
                IsOnSPMap = false,
            },
            //Intergrity Way MP
            new Residence(new Vector3(-59.58021f,-617.4625f,37.35678f),69.24533f,"4 Intergrity Way Apt 28","")
            {
                RentalDays = 28,
                RentalFee = 5000,
                PurchasePrice = 1200000,
                SalesPrice = 980000,
                ResidenceID = 44,
                HasHeaderApartmentBuilding = true,
                Name = "4 Intergrity Way Apt 28",
                EntrancePosition = new Vector3(-59.58021f,-617.4625f,37.35678f),
                EntranceHeading = 69.24533f,
                OpenTime = 0,
                CloseTime = 24,
                InteriorID = 147201,
                IsOnSPMap = false,
            },
            new Residence(new Vector3(-59.58021f,-617.4625f,37.35678f),69.24533f,"4 Intergrity Way Apt 35","")
            {
                RentalDays = 28,
                RentalFee = 5000,
                PurchasePrice = 1200000,
                SalesPrice = 980000,
                ResidenceID = 45,
                HasHeaderApartmentBuilding = true,
                Name = "4 Intergrity Way Apt 35",
                EntrancePosition = new Vector3(-59.58021f,-617.4625f,37.35678f),
                EntranceHeading = 69.24533f,
                OpenTime = 0,
                CloseTime = 24,
                InteriorID = 147969,
                IsOnSPMap = false,
            },

        };

        ApartmentBuildings = new List<ApartmentBuilding>()
        {


            new ApartmentBuilding(new Vector3(-261.9663f, -970.8594f, 31.21996f), 203.518f, "3 Alta Street","")//MP
                {
                ResidenceIDs = new List<int>() { 42,43 },
                OpenTime = 0,CloseTime = 24,
                IsOnSPMap = false,
                CameraPosition = new Vector3(-258.0724f, -993.9235f, 39.27498f),
                CameraDirection = new Vector3(-0.08690932f, 0.9330165f, -0.3491804f),
                CameraRotation = new Rotator(-20.43719f, 0f, 5.321674f),

                },


            new ApartmentBuilding(new Vector3(-59.58021f, -617.4625f, 37.35678f), 69.24533f, "4 Integrity Way","")//SP
                {
                ResidenceIDs = new List<int>() { 1,2 },
                OpenTime = 0,CloseTime = 24,
                IsOnMPMap = false,
                CameraPosition = new Vector3(-103.6068f, -621.355f, 61.43004f),
                CameraDirection = new Vector3(0.9355325f, 0.094497f, -0.3403665f),
                CameraRotation = new Rotator(-19.89921f, -2.269968E-07f, -84.23219f),

                },

            new ApartmentBuilding(new Vector3(-59.58021f, -617.4625f, 37.35678f), 69.24533f, "4 Integrity Way","")//MP
                { 
                ResidenceIDs = new List<int>() { 44,45 },
                OpenTime = 0,CloseTime = 24, 
                IsOnSPMap = false,
                CameraPosition = new Vector3(-103.6068f, -621.355f, 61.43004f), 
                CameraDirection = new Vector3(0.9355325f, 0.094497f, -0.3403665f), 
                CameraRotation = new Rotator(-19.89921f, -2.269968E-07f, -84.23219f),
                
                },
            new ApartmentBuilding(new Vector3(-1442.846f, -544.5376f, 34.74181f), 213.3763f, "Del Perro Heights","")//SP
                {
                ResidenceIDs = new List<int>() { 3,4 },
                OpenTime = 0,CloseTime = 24,
                IsOnMPMap = false,
                CameraPosition = new Vector3(-1399.207f, -569.2247f, 54.83259f), 
                CameraDirection = new Vector3(-0.870531f, 0.4425829f, -0.2151656f), 
                CameraRotation = new Rotator(-12.42524f, 4.371253E-07f, 63.05096f),

                },
            new ApartmentBuilding(new Vector3(-1442.846f, -544.5376f, 34.74181f), 213.3763f, "Del Perro Heights","")//MP
                {
                ResidenceIDs = new List<int>() { 40,41 },
                OpenTime = 0,CloseTime = 24,
                IsOnSPMap = false,
                CameraPosition = new Vector3(-1399.207f, -569.2247f, 54.83259f),
                CameraDirection = new Vector3(-0.870531f, 0.4425829f, -0.2151656f),
                CameraRotation = new Rotator(-12.42524f, 4.371253E-07f, 63.05096f),

                },
            new ApartmentBuilding(new Vector3(-773.63f, 313.08f, 85.70f), 180f, "Eclipse Towers","")//SP
                {
                ResidenceIDs = new List<int>() { 5 },
                OpenTime = 0,CloseTime = 24,
                IsOnMPMap = false,
                CameraPosition = new Vector3(-759.3251f, 282.0685f, 92.48839f), 
                CameraDirection = new Vector3(-0.3824778f, 0.9124349f, -0.1455103f), 
                CameraRotation = new Rotator(-8.36683f, 9.061063E-06f, 22.74264f),

                },
            new ApartmentBuilding(new Vector3(-773.63f, 313.08f, 85.70f), 180f, "Eclipse Towers","")//MP
                {
                ResidenceIDs = new List<int>() { 18,19,20,21,22,23,24,25 },
                OpenTime = 0,CloseTime = 24,
                IsOnSPMap = false,
                CameraPosition = new Vector3(-759.3251f, 282.0685f, 92.48839f),
                CameraDirection = new Vector3(-0.3824778f, 0.9124349f, -0.1455103f),
                CameraRotation = new Rotator(-8.36683f, 9.061063E-06f, 22.74264f),
                },

            new ApartmentBuilding(new Vector3(-936.8324f, -379.2408f, 38.96133f), 113.6606f, "Richard Majestic","")//MP
                {
                ResidenceIDs = new List<int>() { 7 },
                OpenTime = 0,CloseTime = 24,
                CameraPosition = new Vector3(-959.3054f, -437.9868f, 59.45272f), 
                CameraDirection = new Vector3(0.3745578f, 0.8735601f, -0.3108042f), 
                CameraRotation = new Rotator(-18.1077f, -8.533479E-06f, -23.20828f)

                },


            new ApartmentBuilding(new Vector3(-593.1158f, 37.89521f, 43.60775f), 179.0897f, "Tinsel Towers","")//SP
                {
                ResidenceIDs = new List<int>() { 8, 11 },
                OpenTime = 0,CloseTime = 24,
                IsOnMPMap = false,
                CameraPosition = new Vector3(-567.3261f, -4.869073f, 57.82005f), 
                CameraDirection = new Vector3(-0.5004823f, 0.838339f, -0.216114f), 
                CameraRotation = new Rotator(-12.48089f, 2.186096E-06f, 30.83689f)
                },
            new ApartmentBuilding(new Vector3(-593.1158f, 37.89521f, 43.60775f), 179.0897f, "Tinsel Towers","")//MP
                {
                ResidenceIDs = new List<int>() { 9, 10, 11 },
                OpenTime = 0,CloseTime = 24,
                IsOnSPMap = false,
                CameraPosition = new Vector3(-567.3261f, -4.869073f, 57.82005f),
                CameraDirection = new Vector3(-0.5004823f, 0.838339f, -0.216114f),
                CameraRotation = new Rotator(-12.48089f, 2.186096E-06f, 30.83689f)
                },

            new ApartmentBuilding(new Vector3(-914.2256f, -455.1739f, 39.59988f), 116.7621f, "Weazel Plaza","")//MP
                {
                ResidenceIDs = new List<int>() { 12,13,14 },
                OpenTime = 0,CloseTime = 24,
                IsOnSPMap = false,
                CameraPosition = new Vector3(-931.6027f, -469.5208f, 43.69972f),
                CameraDirection = new Vector3(0.6736355f, 0.7187704f, -0.1720006f),
                CameraRotation = new Rotator(-9.904157f, 7.366865E-06f, -43.14341f)
                },
        };



    }
    private void DefaultConfig_Hotels()
    {
        Hotels = new List<Hotel>()
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
            new Hotel(new Vector3(-823.0718f, -1223.552f, 7.365416f), 54.09635f, "The Viceroy", "Demand a great experience","ViceroyMenu"){ BannerImagePath = "stores\\viceroy.png",OpenTime = 0, CloseTime = 24
                ,CameraPosition = new Vector3(-847.939f, -1207.791f, 7.15155f)
                ,CameraDirection = new Vector3(0.9588153f, -0.1468293f, 0.2431342f)
                ,CameraRotation = new Rotator(14.0716f, 0f, -98.70642f)
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
            new Hotel(new Vector3(317.7083f, 2623.256f, 44.46722f), 306.9629f, "Eastern Motel", "","CheapHotelMenu"){OpenTime = 0, CloseTime = 24, ScannerFilePath = "01_specific_location\\0x0B4EB13E.mp3" },
            new Hotel(new Vector3(1142.035f, 2664.177f, 38.16088f), 86.68575f, "The Motor Motel", "Motor on in","CheapHotelMenu"){OpenTime = 0, CloseTime = 24 },
            new Hotel(new Vector3(-477.0448f, 217.5538f, 83.70456f), 355.1573f, "The Generic Hotel", "","ExpensiveHotelMenu"){OpenTime = 0, CloseTime = 24 },
            new Hotel(new Vector3(-309.8708f, 221.5867f, 87.92822f), 6.029551f, "Pegasus", "","ExpensiveHotelMenu") { OpenTime = 0, CloseTime = 24 ,CameraPosition = new Vector3(-347.6738f, 229.8998f, 98.77297f), CameraDirection = new Vector3(0.9052147f, -0.3525268f, -0.2372999f), CameraRotation = new Rotator(-13.72723f, -5.712704E-06f, -111.2779f) },
            new Hotel(new Vector3(-60.74598f, 360.7194f, 113.0564f), 243.5531f, "Gentry Manor", "","ExpensiveHotelMenu"){ OpenTime = 0, CloseTime = 24, FullName = "The Gentry Manor Hotel", ScannerFilePath = "01_specific_location\\0x08E0DB6C.mp3" ,CameraPosition = new Vector3(40.25754f, 259.9668f, 126.4436f), CameraDirection = new Vector3(-0.6868073f, 0.7189223f, -0.1069881f), CameraRotation = new Rotator(-6.141719f, -9.445725E-06f, 43.69126f) },
            new Hotel(new Vector3(-1273.729f, 316.0054f, 65.51177f), 152.4087f, "The Richman", "","ExpensiveHotelMenu"){OpenTime = 0, CloseTime = 24, FullName = "The Richman" },
            new Hotel(new Vector3(286.5596f, -936.6477f, 29.46787f), 138.6224f, "Elkridge Hotel", "","ExpensiveHotelMenu") {OpenTime = 0, CloseTime = 24 ,CameraPosition = new Vector3(257.891f, -952.2925f, 43.25403f), CameraDirection = new Vector3(0.8390263f, 0.4567426f, -0.2956704f), CameraRotation = new Rotator(-17.19774f, -1.072479E-05f, -61.43736f) },
            new Hotel(new Vector3(329.0126f, -69.0122f, 73.03772f), 158.678f, "Vinewood Gardens", "","ExpensiveHotelMenu"){OpenTime = 0, CloseTime = 24 },
            new Hotel(new Vector3(63.68047f, -261.8232f, 52.35384f), 335.7221f, "Cheep Motel", "POOL!","CheapHotelMenu"){OpenTime = 0, CloseTime = 24 },
        };
    }
    private void DefaultConfig_GunStores()
    {
        GunStores = new List<GunStore>()
        {
            new GunStore(new Vector3(1049.596f, -2428.15f, 30.30457f), 84.97017f, "Guns #1", "General shop","GunShop1") { 
                IsEnabled = true, 
                ContactName = StaticStrings.UndergroundGunsContactName,
                ParkingSpaces = new List<SpawnPlace>() {
                    new SpawnPlace(new Vector3(1044.326f, -2404.086f, 29.69204f),352.682f),
                    new SpawnPlace(new Vector3(1044.763f, -2397.967f, 29.08815f), 355.825f),
                    new SpawnPlace(new Vector3(1045.676f, -2389.352f, 29.35636f), 355.9221f),
                    new SpawnPlace(new Vector3(1061.272f, -2445.152f, 28.1964f), 89.23737f),
                    new SpawnPlace(new Vector3(1066.94f, -2463.334f, 27.96432f), 357.0727f),
                } },
            new GunStore(new Vector3(-232.552f, -1311.643f, 31.29598f), 3.180501f, "Guns #2", "Specializes in ~o~Pistols~s~","GunShop2") { 
                IsEnabled = true, 
                ContactName = StaticStrings.UndergroundGunsContactName,
                ParkingSpaces = new List<SpawnPlace>() {
                    new SpawnPlace(new Vector3(-233.4157f, -1306.795f, 31.31846f),89.50895f),
                    new SpawnPlace(new Vector3(-221.8178f, -1291.497f, 30.46753f), 38.83605f),
                    new SpawnPlace(new Vector3(-215.6527f, -1306.28f, 30.48727f), 275.802f),
                    new SpawnPlace(new Vector3(-198.912f, -1300.158f, 30.46804f), 260.0593f),
                    new SpawnPlace(new Vector3(-223.2236f, -1308.06f, 30.46892f), 89.06057f),
            } },
            new GunStore(new Vector3(334.3036f, -1978.458f, 24.16728f), 49.9404f, "Guns #3", "Specializes in ~o~Sub-Machine Guns~s~","GunShop3") { 
                IsEnabled = false, 
                MoneyToUnlock = 10000, 
                ContactName = StaticStrings.UndergroundGunsContactName,
                ParkingSpaces = new List<SpawnPlace>() {
                    new SpawnPlace(new Vector3(322.9245f, -1981.151f, 23.22856f),305.8783f),
                    new SpawnPlace(new Vector3(317.3759f, -1986.411f, 21.63353f), 137.6388f),
                    new SpawnPlace(new Vector3(318.4567f, -2005.651f, 21.25648f), 205.8739f),
                    new SpawnPlace(new Vector3(308.2981f, -1978.542f, 21.6238f), 141.6945f),
            } },
            new GunStore(new Vector3(-258.3577f, 6247.281f, 31.48922f), 314.4655f, "Guns #4", "Specializes in ~o~Assault Rifles~s~","GunShop4") { 
                IsEnabled = false, 
                MoneyToUnlock = 20000, ContactName = StaticStrings.UndergroundGunsContactName,
                ParkingSpaces = new List<SpawnPlace>() {
                    new SpawnPlace(new Vector3(-258.1833f, 6250.695f, 31.48922f),25.17568f),
                    new SpawnPlace(new Vector3(-256.5694f, 6265.761f, 30.58578f), 317.2921f),
                    new SpawnPlace(new Vector3(-267.765f, 6251.046f, 30.61459f), 312.4225f),
                    new SpawnPlace(new Vector3(-269.5155f, 6274.559f, 30.46577f), 136.3388f),
                } },
            new GunStore(new Vector3(1673.425f, 4957.921f, 42.34893f), 227.3988f, "Guns #5", "Specializes in ~o~Heavy Weapons~s~","GunShop5") { 
                IsEnabled = false, 
                MoneyToUnlock = 35000, 
                ContactName = StaticStrings.UndergroundGunsContactName,
                ParkingSpaces = new List<SpawnPlace>() {
                    new SpawnPlace(new Vector3(1661.471f, 4951.514f, 42.07043f),217.9459f),
                    new SpawnPlace(new Vector3(1659.542f, 4946.245f, 41.20879f), 224.609f),
                    new SpawnPlace(new Vector3(1695.27f, 4973.078f, 42.8186f), 288.7947f),
                    new SpawnPlace(new Vector3(1703.823f, 4947.335f, 41.68751f), 54.86424f),
                } },
        };
    }
    private void DefaultConfig_GangDens()
    {
        GangDens = new List<GangDen>() {};
        DefaultConfig_GangDens_Cartel();
        DefaultConfig_GangDens_Armenian();
        DefaultConfig_GangDens_Yardies();
        DefaultConfig_GangDens_Diablos();
        DefaultConfig_GangDens_Varrios();
        DefaultConfig_GangDens_Marabunta();
        DefaultConfig_GangDens_Altruists();
        DefaultConfig_GangDens_Mafia();
        DefaultConfig_GangDens_Families();
        DefaultConfig_GangDens_Ballas();
        DefaultConfig_GangDens_Lost();
        DefaultConfig_GangDens_Triads();
        DefaultConfig_GangDens_Vagos();
        DefaultConfig_GangDens_Redneck();
        DefaultConfig_GangDens_Korean();
    }
    private void DefaultConfig_GangDens_Cartel()
    {
        GangDen Den1 = new GangDen(new Vector3(1389.966f, 1131.907f, 114.3344f), 91.72424f, "Madrazo Cartel Den", "", "MadrazoDenMenu", "AMBIENT_GANG_MADRAZO")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            BannerImagePath = "gangs\\madrazo.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            IsOnMPMap = true,
            IsOnSPMap = true,
            MaxAssaultSpawns = 30,
            DisableNearbyScenarios = true,
            DisableScenarioDistance = 100f,
            PossiblePedSpawns = new List<ConditionalLocation>() 
            {
                new GangConditionalLocation(new Vector3(1390.856f, 1139.184f, 114.4433f), 56.59644f, 65f) { TaskRequirements = TaskRequirements.Guard },
                new GangConditionalLocation(new Vector3(1383.559f, 1156.695f, 114.3345f), 170.1522f, 65f) { TaskRequirements = TaskRequirements.Guard },
                new GangConditionalLocation(new Vector3(1375.004f, 1149.209f, 113.9089f), 91.94662f, 65f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.CanMoveWhenGuarding, ForceSidearm = true,ForceLongGun = true },
                new GangConditionalLocation(new Vector3(1412.588f, 1138.611f, 114.3341f), 196.4261f, 65f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.CanMoveWhenGuarding, ForceSidearm = true,ForceLongGun = true },
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(1410.701f, 1119.38f, 114.5649f), 89.85777f, 85f),
                new GangConditionalLocation(new Vector3(1367.869f, 1147.611f, 113.4886f), 1.871943f, 85f),
            }
        };
        GangDens.Add(Den1);


        //maybe in the future, kinda shitty tho 

        //GangDen Den1MP = new GangDen(new Vector3(1389.966f, 1131.907f, 114.3344f), 91.72424f, "Madrazo Cartel Mansion", "", "MadrazoDenMenu", "AMBIENT_GANG_MADRAZO")
        //{
        //    IsPrimaryGangDen = true,
        //    CanInteractWhenWanted = true,
        //    BannerImagePath = "gangs\\madrazo.png",
        //    OpenTime = 0,
        //    CloseTime = 24,
        //    IsEnabled = true,
        //    IsOnMPMap = true,
        //    IsOnSPMap = false,
        //    InteriorID = -706,
        //    PossiblePedSpawns = new List<ConditionalLocation>()
        //    {
        //        new GangConditionalLocation(new Vector3(1390.856f, 1139.184f, 114.4433f), 56.59644f, 55f) { TaskRequirements = TaskRequirements.Guard },
        //        new GangConditionalLocation(new Vector3(1383.559f, 1156.695f, 114.3345f), 170.1522f, 55f) { TaskRequirements = TaskRequirements.Guard },
        //        new GangConditionalLocation(new Vector3(1375.004f, 1149.209f, 113.9089f), 91.94662f, 55f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.CanMoveWhenGuarding, ForceSidearm = true,ForceLongGun = true },
        //        new GangConditionalLocation(new Vector3(1412.588f, 1138.611f, 114.3341f), 196.4261f, 55f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.CanMoveWhenGuarding, ForceSidearm = true,ForceLongGun = true },
        //    },
        //    PossibleVehicleSpawns = new List<ConditionalLocation>()
        //    {
        //        new GangConditionalLocation(new Vector3(1410.701f, 1119.38f, 114.5649f), 89.85777f, 85f),
        //        new GangConditionalLocation(new Vector3(1367.869f, 1147.611f, 113.4886f), 1.871943f, 85f),
        //    }
        //};
        //GangDens.Add(Den1MP);


    }
    private void DefaultConfig_GangDens_Armenian()
    {
        GangDen Den1 = new GangDen(new Vector3(-615.221f, -1787.458f, 23.69615f), 210.6709f, "Armenian Hangout", "", "ArmenianDenMenu", "AMBIENT_GANG_ARMENIAN")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            //MapIcon = 76,
            BannerImagePath = "gangs\\armenian.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            MaxAssaultSpawns = 20,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(-608.3129f, -1786.241f, 23.63522f), 191.6332f, 45f),
                new GangConditionalLocation(new Vector3(-602.466f, -1784.252f, 23.64002f), 120.1152f, 45f),
                new GangConditionalLocation(new Vector3(-600.0588f, -1795.333f, 23.38985f), 155.3086f, 45f),
                new GangConditionalLocation(new Vector3(-622.9163f, -1792.963f, 23.80351f), 163.1899f, 45f),
                new GangConditionalLocation(new Vector3(-591.4926f, -1776.089f, 22.79006f), 214.9723f, 45f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(-597.8096f, -1791.076f, 22.92511f), 300.9409f, 50f),
                new GangConditionalLocation(new Vector3(-607.3063f, -1788.965f, 22.9275f), 162.6913f, 50f),
            },
           // CameraPosition = new Vector3(-589.6008f, -1798.514f, 25.45085f),
            //CameraDirection = new Vector3(0.586422f, 0.7246112f, -0.3620054f),
            //CameraRotation = new Rotator(-21.2234f, -7.327146E-06f, -38.98296f),
            VehiclePreviewLocation = new SpawnPlace(new Vector3(-585.7418f, -1792.97f, 22.85472f), 53.11781f),
            VehicleDeliveryLocations = new List<SpawnPlace>() 
            {
                new SpawnPlace(new Vector3(-581.0074f, -1797.569f, 22.8803f), 322.0219f),
                new SpawnPlace(new Vector3(-584.7271f, -1780.009f, 22.66464f), 144.9928f),
                new SpawnPlace(new Vector3(-586.3314f, -1804.048f, 22.58966f), 115.4426f),
            }
        };
        GangDens.Add(Den1);
    }
    private void DefaultConfig_GangDens_Yardies()
    {
        GangDen Den1 = new GangDen(new Vector3(-1157.501f, -1451.861f, 4.468448f), 216.5082f, "Yardies Chill Spot", "", "YardiesDenMenu", "AMBIENT_GANG_YARDIES")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            BannerImagePath = "gangs\\yardies.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            PossiblePedSpawns = new List<ConditionalLocation>() 
            {
                new GangConditionalLocation(new Vector3(-1155.236f, -1450.92f, 4.508356f), 203.7991f, 35f),
                new GangConditionalLocation(new Vector3(-1158.698f, -1454.306f, 4.346763f), 192.8651f, 35f),
                new GangConditionalLocation(new Vector3(-1166.395f, -1453.536f, 4.367522f), 122.2634f, 35f),
                new GangConditionalLocation(new Vector3(-1177.108f, -1437.649f, 4.379432f), 75.27589f, 35f),
                new GangConditionalLocation(new Vector3(-1171.944f, -1470.263f, 4.382764f), 290.1982f, 35f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(-1156.718f, -1462.597f, 3.70943f), 210.7057f, 45f),
                new GangConditionalLocation(new Vector3(-1150.134f, -1456.732f, 3.955124f), 215.4072f, 45f),
            }
        };//near shops on del perro beach
        GangDens.Add(Den1);
    }
    private void DefaultConfig_GangDens_Diablos()
    {
        GangDen Den1 = new GangDen(new Vector3(275.2381f, -3015.519f, 5.945963f), 91.01478f, "Diablos Hangout", "", "DiablosDenMenu", "AMBIENT_GANG_DIABLOS")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            //MapIcon = 355,
            BannerImagePath = "gangs\\diablos.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            MaxAssaultSpawns = 20,
            PossiblePedSpawns = new List<ConditionalLocation>() 
            {
                new GangConditionalLocation(new Vector3(274.3982f, -3017.684f, 5.699762f), 86.04555f, 65f),
                new GangConditionalLocation(new Vector3(274.2913f, -3010.913f, 5.70001f), 105.1246f, 65f),
                new GangConditionalLocation(new Vector3(281.5445f, -2997.272f, 5.672155f), 5.840774f, 65f),
                new GangConditionalLocation(new Vector3(285.892f, -3025.893f, 5.652033f), 304.8488f, 65f),
                new GangConditionalLocation(new Vector3(290.5836f, -3035.765f, 5.882213f), 174.9672f, 65f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(260.6703f, -3008.191f, 5.070713f), 87.13465f, 75f),
                new GangConditionalLocation(new Vector3(286.6504f, -2996.586f, 5.060739f), 85.31966f, 75f),
            }
        };//shitty shack in elysian
        GangDens.Add(Den1);
    }
    private void DefaultConfig_GangDens_Varrios()
    {
        GangDen Den1 = new GangDen(new Vector3(1193.61f, -1656.411f, 43.02641f), 31.55427f, "Varrios Los Aztecas Den", "", "VarriosDenMenu", "AMBIENT_GANG_SALVA")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            BannerImagePath = "gangs\\varrios.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            //HasVanillaGangSpawnedAroundToBeBlocked = true,
            PossiblePedSpawns = new List<ConditionalLocation>() 
            {
                new GangConditionalLocation(new Vector3(1193.946f, -1651.643f, 42.358f), 18.49724f, 75f),
                new GangConditionalLocation(new Vector3(1189.079f, -1655.169f, 42.358f), 338.5124f, 75f),
                new GangConditionalLocation(new Vector3(1190.508f, -1648.968f, 41.43568f), 34.16187f, 75f),
                new GangConditionalLocation(new Vector3(1172.624f, -1645.602f, 36.78029f), 94.79508f, 75f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(1166.678f, -1647.833f, 36.23235f), 132.0248f, 75f),
                new GangConditionalLocation(new Vector3(1157.749f, -1660.837f, 35.93925f), 205.3036f, 75f),
                new GangConditionalLocation(new Vector3(1184.424f, -1652.161f, 39.12302f), 312.0581f, 75f),
            }
        };
        GangDens.Add(Den1);
    }
    private void DefaultConfig_GangDens_Marabunta()
    {
        GangDen Den1 = new GangDen(new Vector3(1299.267f, -1752.92f, 53.88011f), 110.3803f, "Marabunta Grande Den", "", "MarabunteDenMenu", "AMBIENT_GANG_MARABUNTE")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            //MapIcon = 78,
            BannerImagePath = "gangs\\marabunta.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            //HasVanillaGangSpawnedAroundToBeBlocked = true,
            PossiblePedSpawns = new List<ConditionalLocation>() 
            {
                new GangConditionalLocation(new Vector3(1301.137f, -1754.575f, 53.87848f), 184.4178f, 50f),
                new GangConditionalLocation(new Vector3(1293.167f, -1748.509f, 53.87848f), 200.1636f, 50f),
                new GangConditionalLocation(new Vector3(1313.939f, -1771.984f, 54.81713f), 113.8552f, 50f),
                new GangConditionalLocation(new Vector3(1319.703f, -1775.434f, 54.5059f), 215.1372f, 50f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(1322.044f, -1768.506f, 54.82401f), 195.1561f, 75f),
                new GangConditionalLocation(new Vector3(1313.927f, -1783.216f, 51.72291f), 109.9802f, 75f),
            }
        };
        GangDens.Add(Den1);
    }
    private void DefaultConfig_GangDens_Altruists()
    {
        GangDen Den1 = new GangDen(new Vector3(-1144.041f, 4908.383f, 220.9688f), 33.69744f, "Altruist Cult Den", "", "GenericGangDenMenu", "AMBIENT_GANG_CULT") 
        { 
            IsPrimaryGangDen = true, 
            CanInteractWhenWanted = true,
            //MapIcon = 76, 
            BannerImagePath = "gangs\\altruist.png", 
            OpenTime = 0, 
            CloseTime = 24, 
            IsEnabled = false, 
        };
        GangDens.Add(Den1);
    }
    private void DefaultConfig_GangDens_Korean()
    {
        GangDen KoreanDen1 = new GangDen(new Vector3(-579.9809f, -778.5275f, 25.01723f), 90.93932f, "Kkangpae Den", "", "KkangpaeDenMenu", "AMBIENT_GANG_KKANGPAE")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            BannerImagePath = "gangs\\kkangpae.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            MaxAssaultSpawns = 20,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(-585.9933f, -776.1322f, 25.01723f), 167.2549f, 70f),
                new GangConditionalLocation(new Vector3(-581.7299f, -780.7718f, 25.01723f), 66.59444f, 70f),
                new GangConditionalLocation(new Vector3(-613.7438f, -782.4008f, 25.20246f), 42.77346f, 70f),
                new GangConditionalLocation(new Vector3(-622.1181f, -768.7686f, 25.95107f), 88.71659f, 70f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(-610.1835f, -756.0798f, 26.07551f), 266.9069f, 75f),
                new GangConditionalLocation(new Vector3(-618.8381f, -761.884f, 25.46778f), 89.04613f, 75f),
            }
        };
        GangDens.Add(KoreanDen1);
        //new GangDen(new Vector3(-766.3793f, -917.0612f, 21.29704f), 268.4079f, "Kkangpae Den", "","KkangpaeDenMenu", "AMBIENT_GANG_KKANGPAE") {  OpenTime = 0,CloseTime = 24, IsEnabled = true, PossiblePedSpawns = new List<ConditionalLocation>() {
        //    new GangConditionalLocation(new Vector3(-761.3151f, -910.3087f, 19.53444f), 237.0572f, 50f),
        //    new GangConditionalLocation(new Vector3(-762.3676f, -923.8786f, 18.74619f), 237.6777f, 50f),
        //    new GangConditionalLocation(new Vector3(-760.9077f, -927.9499f, 18.47775f), 292.1328f, 50f),
        //    new GangConditionalLocation(new Vector3(-764.5098f, -919.3218f, 20.20216f), 267.6916f, 50f),
        //} },
    }
    private void DefaultConfig_GangDens_Redneck()
    {
        GangDen RedneckDen1 = new GangDen(new Vector3(959.721f, 3618.905f, 32.67253f), 93.92658f, "Reckneck Den", "", "GenericGangDenMenu", "AMBIENT_GANG_HILLBILLY")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            BannerImagePath = "gangs\\rednecks.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            PossiblePedSpawns = new List<ConditionalLocation>() 
            {
                new GangConditionalLocation(new Vector3(957.8521f, 3616.273f, 32.75988f), 56.09721f, 75f),
                new GangConditionalLocation(new Vector3(959.2394f, 3612.307f, 32.75033f), 138.3261f, 75f),
                new GangConditionalLocation(new Vector3(969.4822f, 3626.386f, 32.33695f), 17.91204f, 75f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(950.5566f, 3618.955f, 31.88234f), 268.8919f, 75f),
                new GangConditionalLocation(new Vector3(950.8107f, 3622.341f, 31.76403f), 271.0765f, 75f),
                new GangConditionalLocation(new Vector3(950.5989f, 3615.652f, 31.93583f), 271.2046f, 75f),
            }
        };
        GangDens.Add(RedneckDen1);
    }
    private void DefaultConfig_GangDens_Mafia()
    {
        GangDen PavanoDen1 = new GangDen(new Vector3(1662.302f, 4776.384f, 42.00795f), 279.1427f, "Pavano Safehouse", "", "PavanoDenMenu", "AMBIENT_GANG_PAVANO")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            //MapIcon = 267,
            BannerImagePath = "gangs\\pavano.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            MaxAssaultSpawns = 10,
            PossiblePedSpawns = new List<ConditionalLocation>() 
            {
                new GangConditionalLocation(new Vector3(1664.384f, 4765.325f, 42.0682f), 290.2068f, 65f),
                new GangConditionalLocation(new Vector3(1662.094f, 4778.333f, 42.00935f), 259.006f, 65f),
                new GangConditionalLocation(new Vector3(1647.39f, 4779.691f, 42.01637f), 9.346325f, 65f),
                new GangConditionalLocation(new Vector3(1666.244f, 4773.587f, 41.93539f), 215.5277f, 65f),
                new GangConditionalLocation(new Vector3(1665.208f, 4778.518f, 41.94879f), 271.264f, 65f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(1669.677f, 4776.892f, 41.22393f), 4.480381f, 75f),
                new GangConditionalLocation(new Vector3(1662.887f, 4768.458f, 41.3252f), 278.0306f, 75f),
            }
        };//Grapeseed Shack
        GangDen LupisellaDen1 = new GangDen(new Vector3(-229.6159f, 6445.189f, 31.19745f), 139.3764f, "Lupisella Safehouse", "", "LupisellaDenMenu", "AMBIENT_GANG_LUPISELLA")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            //MapIcon = 77,
            BannerImagePath = "gangs\\lupisella.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            MaxAssaultSpawns = 10,
            PossiblePedSpawns = new List<ConditionalLocation>() 
            {
                new GangConditionalLocation(new Vector3(-233.3555f, 6447.341f, 31.19741f), 131.7259f, 65f),
                new GangConditionalLocation(new Vector3(-227.5764f, 6442.54f, 31.19769f), 141.2697f, 65f),
                new GangConditionalLocation(new Vector3(-217.9765f, 6432.755f, 31.19775f), 165.7642f, 65f),
                new GangConditionalLocation(new Vector3(-212.6066f, 6443.875f, 31.2976f), 326.0233f, 65f),
                new GangConditionalLocation(new Vector3(-215.7044f, 6445.802f, 31.3135f), 270.9214f, 65f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(-226.1172f, 6435.622f, 30.51782f), 232.3148f, 75f),
                new GangConditionalLocation(new Vector3(-210.5319f, 6437.373f, 30.72163f), 327.138f, 75f),
            }
        };//Beachhouse in Paleto
        GangDen MessinaDen1 = new GangDen(new Vector3(-1629.715f, 36.49737f, 62.93618f), 333.3146f, "Messina Safehouse", "", "MessinaDenMenu", "AMBIENT_GANG_MESSINA")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            //MapIcon = 78,
            BannerImagePath = "gangs\\messina.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            MaxAssaultSpawns = 20,
            PossiblePedSpawns = new List<ConditionalLocation>() 
            {
                new GangConditionalLocation(new Vector3(-1630.811f, 39.70509f, 62.54136f), 323.2733f, 65f),
                new GangConditionalLocation(new Vector3(-1626.534f, 37.39336f, 62.54136f), 342.5028f, 65f),
                new GangConditionalLocation(new Vector3(-1614.67f, 29.32284f, 62.54132f), 257.1678f, 65f),
                new GangConditionalLocation(new Vector3(-1620.178f, 19.3019f, 62.54137f), 269.8025f, 65f),
                new GangConditionalLocation(new Vector3(-1639.313f, 16.93647f, 62.53671f), 122.0091f, 65f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(-1613.357f, 22.61485f, 61.48566f), 155.361f, 75f),
                new GangConditionalLocation(new Vector3(-1625.055f, 65.92033f, 61.15781f), 237.2095f, 75f),
            }
        };//mansion in richman
        GangDen AncelottiDen1 = new GangDen(new Vector3(-3228.478f, 1092.326f, 10.76322f), 253.458f, "Ancelotti Safehouse", "", "AncelottiDenMenu", "AMBIENT_GANG_ANCELOTTI")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            //MapIcon = 76,
            BannerImagePath = "gangs\\ancelotti.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            MaxAssaultSpawns = 15,
            PossiblePedSpawns = new List<ConditionalLocation>() 
            {
                new GangConditionalLocation(new Vector3(-3228.078f, 1089.288f, 10.73698f), 243.1453f, 65f),
                new GangConditionalLocation(new Vector3(-3226.294f, 1095.004f, 10.69816f), 260.7955f, 65f),
                new GangConditionalLocation(new Vector3(-3240.798f, 1081.971f, 7.396926f), 157.9129f, 65f),
                new GangConditionalLocation(new Vector3(-3242.731f, 1087.94f, 7.478574f), 115.9338f, 65f),
                new GangConditionalLocation(new Vector3(-3247.704f, 1098.255f, 2.835759f), 70.83805f, 65f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(-3226.144f, 1086.399f, 9.993718f), 162.1115f, 75f),
                new GangConditionalLocation(new Vector3(-3229.512f, 1077.396f, 10.19788f), 260.5915f, 75f),
            }
        };//beachhousein chumash
        float spawnChance = 45f;
        GangDen GambettiDen1 = new GangDen(new Vector3(514.9427f, 190.9465f, 104.745f), 356.6495f, "Gambetti Safehouse", "", "GambettiDenMenu", "AMBIENT_GANG_GAMBETTI")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            //MapIcon = 541,
            BannerImagePath = "gangs\\gambetti.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            MaxAssaultSpawns = 10,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(517.7187f, 191.4421f, 104.745f), 344.8743f, spawnChance) { TaskRequirements = TaskRequirements.Guard, ForceSidearm = true },//right by thingo, alwayts has a gun
                new GangConditionalLocation(new Vector3(504.8874f, 201.7501f, 104.7445f), 334.6481f, spawnChance){ TaskRequirements = TaskRequirements.Guard, },
                new GangConditionalLocation(new Vector3(541.257f, 201.7959f, 101.5426f), 246.2837f, spawnChance){ TaskRequirements = TaskRequirements.Guard | TaskRequirements.CanMoveWhenGuarding },
                new GangConditionalLocation(new Vector3(546.3932f, 214.8331f, 102.2434f), 320.1989f, spawnChance){ TaskRequirements = TaskRequirements.Guard | TaskRequirements.CanMoveWhenGuarding },
                new GangConditionalLocation(new Vector3(539.8316f, 241.9526f, 103.1213f), 324.5678f, spawnChance){ TaskRequirements = TaskRequirements.Patrol},
                new GangConditionalLocation(new Vector3(516.8485f, 250.2658f, 103.1146f), 350.7859f, spawnChance){ TaskRequirements = TaskRequirements.Patrol },
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(464.1385f, 226.9263f, 102.5122f), 67.93208f, 75f),
                new GangConditionalLocation(new Vector3(462.139f, 222.349f, 102.2742f), 245.8917f, 75f),
            }
        };//Downtown Vinewood
        GangDens.Add(PavanoDen1);
        GangDens.Add(LupisellaDen1);
        GangDens.Add(MessinaDen1);
        GangDens.Add(AncelottiDen1);
        GangDens.Add(GambettiDen1);
    }
    private void DefaultConfig_GangDens_Ballas()
    {
        GangDen BallasDen1 = new GangDen(new Vector3(86.11255f, -1959.272f, 21.12167f), 318.5057f, "Ballas Den", "", "BallasDenMenu", "AMBIENT_GANG_BALLAS")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            //MapIcon = 106,
            BannerImagePath = "gangs\\ballas.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            //HasVanillaGangSpawnedAroundToBeBlocked = true,
            PossiblePedSpawns = new List<ConditionalLocation>() 
            {
                new GangConditionalLocation(new Vector3(84.76484f, -1953.536f, 20.8518f), 334.0088f, 35f),
                new GangConditionalLocation(new Vector3(87.02995f, -1947.637f, 20.74858f), 303.2596f, 35f),
                new GangConditionalLocation(new Vector3(95.30958f, -1954.979f, 20.75126f), 314.5049f, 35f),
                new GangConditionalLocation(new Vector3(84.23887f, -1932.319f, 20.74922f), 19.71852f, 35f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(94.70525f, -1960.741f, 20.06409f), 322.6508f, 45f),
                new GangConditionalLocation(new Vector3(108.7719f, -1951.384f, 20.01156f), 294.1027f, 45f),
                new GangConditionalLocation(new Vector3(113.3118f, -1933.905f, 19.9819f), 36.83981f, 45f),
                new GangConditionalLocation(new Vector3(68.90487f, -1922.226f, 20.57331f), 130.7354f, 45f),
            }
        };//This is in DAVIS near Grove Street
        GangDen BallasDen2 = new GangDen()
        {
            //MapIcon = 106,
            AssignedAssociationID = "AMBIENT_GANG_BALLAS",
            PossiblePedSpawns = new List<ConditionalLocation>() 
            {
                    new GangConditionalLocation(new Vector3(166.5394f,-1870.718f,24.05765f),166.9992f,35f),
                    new GangConditionalLocation(new Vector3(176.6803f,-1856.694f,24.0493f),17.00001f,35f),
                    new GangConditionalLocation(new Vector3(168.3999f,-1863.078f,24.09362f),89.99988f,35f),
                    new GangConditionalLocation(new Vector3(177.6906f,-1856.02f,24.07937f),120.7892f,35f),
                    new GangConditionalLocation(new Vector3(168.0899f,-1862.198f,24.10498f),-175.0005f,35f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>() 
            {
                new GangConditionalLocation(new Vector3(166.1798f,-1860.151f,23.61886f),-22.5675f, 75f),
            },
            MenuID = "BallasDenMenu",
            Name = "Ballas Grove Trap house",
            FullName = "Ballas Grove Trap house",
            Description = "",
            IsEnabled = true,
            BannerImagePath = "gangs\\ballas.png",
            EntrancePosition = new Vector3(171.3143f, -1871.577f, 24.40372f),
            EntranceHeading = 97.43147f,
            OpenTime = 0,
            CloseTime = 24,
        };
        GangDen BallasDen3 = new GangDen()
        {
            //MapIcon = 514,
            AssignedAssociationID = "AMBIENT_GANG_BALLAS",
            PossiblePedSpawns = new List<ConditionalLocation>() {
                    new GangConditionalLocation() {
                    Location = new Vector3(141.4837f,-1950.442f,19.1047f),
                    Heading = 21.99992f,
                    Percentage = 35f,
                    },
                    new GangConditionalLocation() {
                    Location = new Vector3(142.3741f,-1949.44f,19.1047f),
                    Heading = 99.99986f,
                    Percentage = 35f,
                    },
                    new GangConditionalLocation() {
                    Location = new Vector3(141.198f,-1949.059f,19.10614f),
                    Heading = -118.0001f,
                    Percentage = 35f,
                    },
                    new GangConditionalLocation() {
                    Location = new Vector3(125.5248f,-1956.646f,20.73793f),
                    Heading = -110.9998f,
                    Percentage = 35f,
                    },
                    new GangConditionalLocation() {
                    Location = new Vector3(132.3828f,-1959.473f,18.48615f),
                    Heading = 71.99995f,
                    Percentage = 35f,
                    },
                },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
                    new GangConditionalLocation() {
                    Location = new Vector3(152.3754f,-1965.335f,18.3794f),
                    Heading = 49.37751f,
                    Percentage = 45f,
                    },
                },
            MenuID = "BallasDenMenu",
            Name = "Ballas Grove street Trap house",
            FullName = "Ballas Grove street Trap house",
            Description = "",
            IsEnabled = true,
            BannerImagePath = "gangs\\ballas.png",
            EntrancePosition = new Vector3(148.9452f, -1960.762f, 19.46201f),
            EntranceHeading = -128.9999f,
            OpenTime = 0,
            CloseTime = 24,
        };
        GangDen BallasDen4 = new GangDen()
        {
            ShowsOnDirectory = false,
            TypeName = "Gang Den",
            //MapIcon = 514,
            MapIconScale = 1f,
            ButtonPromptText = "Enter Ballas Hangout",
            AssignedAssociationID = "AMBIENT_GANG_BALLAS",
            VehiclePreviewLocation = new SpawnPlace(new Vector3(0f, 0f, 0), 0f),
            PossiblePedSpawns = new List<ConditionalLocation>() {
                    new GangConditionalLocation() {
                    Location = new Vector3(7.144477f,-1894.97f,23.12855f),
                    Heading = 146.9997f,
                    Percentage = 35f,
                    },
                    new GangConditionalLocation() {
                    Location = new Vector3(5.740968f,-1895.117f,23.1351f),
                    Heading = -115.9998f,
                    Percentage = 35f,
                    },
                    new GangConditionalLocation() {
                    Location = new Vector3(6.539215f,-1896.222f,23.06272f),
                    Heading = 0f,
                    Percentage = 35f,
                    },
                    new GangConditionalLocation() {
                    Location = new Vector3(7.997568f,-1884.522f,23.3195f),
                    Heading = -11.00386f,
                    Percentage = 35f,
                    },
                    new GangConditionalLocation() {
                    Location = new Vector3(7.734192f,-1883.811f,23.3195f),
                    Heading = -117.9994f,
                    Percentage = 35f,
                    },
                },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
                    new GangConditionalLocation() {
                    Location = new Vector3(15.37083f,-1882.745f,22.59842f),
                    Heading = 141.488f,
                    Percentage = 45f,
                    },
                },
            MenuID = "BallasDenMenu",
            Name = "Ballas Covenant Trap house",
            FullName = "Ballas Covenant Trap house",
            Description = "",
            IsEnabled = true,
            BannerImagePath = "gangs\\ballas.png",
            EntrancePosition = new Vector3(5.248489f, -1883.882f, 23.70065f),
            EntranceHeading = -39.92797f,
            OpenTime = 0,
            CloseTime = 24,
        };
        GangDen BallasDen5 = new GangDen()
        {
            //MapIcon = 47,
            AssignedAssociationID = "AMBIENT_GANG_BALLAS",
            PossiblePedSpawns = new List<ConditionalLocation>() {
                    new GangConditionalLocation() {
                    Location = new Vector3(225.4509f,-1691.309f,29.29176f),
                    Heading = 0f,
                    Percentage = 35f,
                    },
                    new GangConditionalLocation() {
                    Location = new Vector3(224.7998f,-1690.349f,29.29176f),
                    Heading = -101.9999f,
                    Percentage = 35f,
                    },
                    new GangConditionalLocation() {
                    Location = new Vector3(216.9368f,-1690.644f,29.29578f),
                    Heading = 0f,
                    Percentage = 35f,
                    },
                    new GangConditionalLocation() {
                    Location = new Vector3(217.9461f,-1690.031f,29.29447f),
                    Heading = 91.9999f,
                    Percentage = 35f,
                    },
                    new GangConditionalLocation() {
                    Location = new Vector3(216.8228f,-1689.383f,29.32374f),
                    Heading = -137.9996f,
                    Percentage = 35f,
                    },
                },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
                    new GangConditionalLocation() {
                    Location = new Vector3(224.8163f,-1707.909f,28.75177f),
                    Heading = 34.13058f,
                    Percentage = 45f,
                    },
                },
            MenuID = "BallasDenMenu",
            Name = "Ballas hangout",
            FullName = "Ballas Covenant Trap house",
            Description = "",
            IsEnabled = true,
            BannerImagePath = "gangs\\ballas.png",
            EntrancePosition = new Vector3(223.0043f, -1703.303f, 29.69505f),
            EntranceHeading = -141.9996f,
            OpenTime = 0,
            CloseTime = 24,
        };
        GangDens.Add(BallasDen1);
        GangDens.Add(BallasDen2);
        //GangDens.Add(BallasDen3);
        //GangDens.Add(BallasDen4);
        //GangDens.Add(BallasDen5);
    }
    private void DefaultConfig_GangDens_Families()
    {
        GangDen FamiliesDen1 = new GangDen(new Vector3(-223.1647f, -1601.309f, 34.88379f), 266.3889f, "The Families Den", "The OGs", "FamiliesDenMenu", "AMBIENT_GANG_FAMILY")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            //MapIcon = 86,
            BannerImagePath = "gangs\\families.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            //HasVanillaGangSpawnedAroundToBeBlocked = true,
            PossiblePedSpawns = new List<ConditionalLocation>() 
            {
                new GangConditionalLocation(new Vector3(-219.9773f, -1594.828f, 34.86927f), 326.5532f, 35f),
                new GangConditionalLocation(new Vector3(-214.3197f, -1580.346f, 34.86931f), 147.0013f, 35f),
                new GangConditionalLocation(new Vector3(-234.661f, -1606.322f, 34.26423f), 70.74878f, 35f),
                new GangConditionalLocation(new Vector3(-234.2032f, -1602.192f, 34.30891f), 104.5916f, 35f),
                new GangConditionalLocation(new Vector3(-197.1071f, -1604.178f, 34.36404f), 260.1856f, 35f),
                new GangConditionalLocation(new Vector3(-191.8121f, -1595.623f, 34.5155f), 263.9107f, 35f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(-237.4984f, -1612.648f, 33.17708f), 179.3492f, 45f),
                new GangConditionalLocation(new Vector3(-221.4851f, -1633.947f, 32.93398f), 269.4803f, 45f),
                new GangConditionalLocation(new Vector3(-176.6605f, -1608.427f, 32.9703f), 342.4364f, 45f),
                new GangConditionalLocation(new Vector3(-189.6106f, -1611.435f, 33.20397f), 174.0037f, 45f),
            }
        };//This is in Chamberlain Hills
        GangDen FamiliesDen2 = new GangDen()
        {
            //MapIcon = 86,
            AssignedAssociationID = "AMBIENT_GANG_FAMILY",
            PossiblePedSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
                Location = new Vector3(11.67317f,-1434.168f,30.54151f),
                Heading = -108.9996f,
                Percentage = 35f,
            },
            new GangConditionalLocation() {
                Location = new Vector3(13.05488f,-1434.443f,30.54151f),
                Heading = 129.9996f,
                Percentage = 35f,
            },
            new GangConditionalLocation() {
                Location = new Vector3(22.77389f,-1428.475f,30.53836f),
                Heading = 175.9996f,
                Percentage = 35f,
            },
            new GangConditionalLocation() {
                Location = new Vector3(22.58638f,-1430.516f,30.54151f),
                Heading = 25.99998f,
                Percentage = 35f,
            },
            new GangConditionalLocation() {
                Location = new Vector3(24.02172f,-1429.669f,30.54151f),
                Heading = 75.99992f,
                Percentage = 35f,
            },
        },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
                Location = new Vector3(25.02328f,-1454.192f,29.77646f),
                Heading = -36.88154f,
                Percentage = 35f,
            },
        },
            MenuID = "FamiliesDenMenu",
            CanInteractWhenWanted = true,
            Name = "Families OG Hideout 1",
            FullName = "Families OG Hideout 1",
            Description = "",
            IsEnabled = true,
            BannerImagePath = "gangs\\families.png",
            EntrancePosition = new Vector3(16.29254f, -1444.204f, 30.95157f),
            EntranceHeading = 164.9996f,
            OpenTime = 0,
            CloseTime = 24,
        };
        GangDen FamiliesDen3 = new GangDen()
        {
            //MapIcon = 86,
            AssignedAssociationID = "AMBIENT_GANG_FAMILY",
            PossiblePedSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
                Location = new Vector3(-121.8506f,-1485.729f,33.82269f),
                Heading = 175.9996f,
                Percentage = 35f,
            },
            new GangConditionalLocation() {
                Location = new Vector3(-111.3437f,-1458.663f,33.52345f),
                Heading = -51.99996f,
                Percentage = 35f,
            },
            new GangConditionalLocation() {
                Location = new Vector3(-131.6255f,-1463.676f,33.82264f),
                Heading = -125.9998f,
                Percentage = 35f,
            },
            new GangConditionalLocation() {
                Location = new Vector3(-131.2158f,-1465.14f,33.82258f),
                Heading = 0f,
                Percentage = 35f,
            },
            new GangConditionalLocation() {
                Location = new Vector3(-130.0541f,-1463.692f,33.82257f),
                Heading = 122.9998f,
                Percentage = 35f,
            },
        },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
                Location = new Vector3(-109.2051f,-1456.626f,32.91021f),
                Heading = -129.7567f,
                Percentage = 35f,
            },
        },
            MenuID = "FamiliesDenMenu",
            Name = "Families OG Hideout 2",
            FullName = "Families OG Hideout 2",
            Description = "",
            IsEnabled = true,
            EntrancePosition = new Vector3(-138.0617f, -1470.718f, 36.99474f),
            EntranceHeading = -39.2744f,
            OpenTime = 0,
            CloseTime = 24,
        };
        GangDen FamiliesDen4 = new GangDen()
        {
            //MapIcon = 86,
            AssignedAssociationID = "AMBIENT_GANG_FAMILY",
            PossiblePedSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
                Location = new Vector3(-58.05992f,-1507.429f,32.11591f),
                Heading = -41.99997f,
                Percentage = 35f,
            },
            new GangConditionalLocation() {
                Location = new Vector3(-80.4858f,-1522.49f,34.2426f),
                Heading = 90.9976f,
                Percentage = 35f,
            },
            new GangConditionalLocation() {
                Location = new Vector3(-63.70837f,-1526.52f,34.24794f),
                Heading = 41.99997f,
                Percentage = 35f,
            },
            new GangConditionalLocation() {
                Location = new Vector3(-63.93343f,-1524.981f,34.24784f),
                Heading = 151.9997f,
                Percentage = 35f,
            },
            new GangConditionalLocation() {
                Location = new Vector3(-70.06671f,-1519.742f,34.248f),
                Heading = -24.99999f,
                Percentage = 35f,
            },
        },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
                Location = new Vector3(-57.86029f,-1493.169f,31.22089f),
                Heading = 133.9038f,
                Percentage = 35f,
            },
        },
            MenuID = "FamiliesDenMenu",
            Name = "Families OG Hideout3 ",
            FullName = "Families OG Hideout 3",
            Description = "",
            IsEnabled = true,
            BannerImagePath = "gangs\\families.png",
            EntrancePosition = new Vector3(-69.21278f, -1526.623f, 34.23756f),
            EntranceHeading = -49.99997f,
            OpenTime = 0,
            CloseTime = 24,
        };
        GangDen FamiliesDen5 = new GangDen()
        {
            //MapIcon = 86,
            AssignedAssociationID = "AMBIENT_GANG_FAMILY",
            PossiblePedSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
                Location = new Vector3(-144.4888f,-1586.785f,34.84586f),
                Heading = 117.9993f,
                Percentage = 35f,
            },
            new GangConditionalLocation() {
                Location = new Vector3(-134.4106f,-1591.487f,34.24623f),
                Heading = 146.9997f,
                Percentage = 35f,
            },
            new GangConditionalLocation() {
                Location = new Vector3(-134.3287f,-1592.902f,34.24641f),
                Heading = 0f,
                Percentage = 35f,
            },
            new GangConditionalLocation() {
                Location = new Vector3(-135.906f,-1592.25f,34.24649f),
                Heading = -70.99994f,
                Percentage = 35f,
            },
            new GangConditionalLocation() {
                Location = new Vector3(-114.2554f,-1596.416f,32.11221f),
                Heading = -124.9998f,
                Percentage = 35f,
            },
        },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
                Location = new Vector3(-111.1881f,-1597.381f,31.3905f),
                Heading = 138.7799f,
                Percentage = 35f,
            },
        },
            MenuID = "FamiliesDenMenu",
            Name = "Families OG Hideout 4",
            FullName = "Families OG Hideout 4",
            Description = "",
            IsEnabled = true,
            BannerImagePath = "gangs\\families.png",
            EntrancePosition = new Vector3(-140.2153f, -1599.549f, 34.83382f),
            EntranceHeading = -11f,
            OpenTime = 0,
            CloseTime = 24,
        };
        GangDens.Add(FamiliesDen1);
        //GangDens.Add(FamiliesDen2);
        //GangDens.Add(FamiliesDen3);
        //GangDens.Add(FamiliesDen4);
        //GangDens.Add(FamiliesDen5);
    }
    private void DefaultConfig_GangDens_Lost()
    {
        GangDen LostMainDen = new GangDen(new Vector3(981.8542f, -103.0203f, 74.84874f), 220.3094f, "Lost M.C. Clubhouse", "", "LostDenMenu", "AMBIENT_GANG_LOST")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            //MapIcon = 226,
            BannerImagePath = "gangs\\lostmc.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            IsOnMPMap = false,
            IsOnSPMap = true,
            MaxAssaultSpawns = 25,
            //HasVanillaGangSpawnedAroundToBeBlocked = true,
            PossiblePedSpawns = new List<ConditionalLocation>() 
            {
                new GangConditionalLocation(new Vector3(987.3098f, -107.0706f, 74.32984f), 138.8955f, 45f),
                new GangConditionalLocation(new Vector3(975.0338f, -112.2333f, 74.35313f), 188.1823f, 45f),
                new GangConditionalLocation(new Vector3(960.2211f, -123.5155f, 74.35313f), 181.6973f, 45f),
                new GangConditionalLocation(new Vector3(981.1419f, -142.9071f, 74.23688f), 80.7939f, 45f),
                new GangConditionalLocation(new Vector3(976.2161f, -144.2681f, 74.24052f), 0.7361088f, 45f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(976.4726f, -132.778f, 73.21405f), 58.86649f, 35f),
                new GangConditionalLocation(new Vector3(960.3214f, -132.2091f, 73.69121f), 189.5355f, 35f),
                new GangConditionalLocation(new Vector3(972.7709f, -142.4676f, 73.60614f), 239.6191f, 35f),
                new GangConditionalLocation(new Vector3(972.8246f, -155.1686f, 72.8443f), 52.89192f, 35f),
            },
            //CameraPosition = new Vector3(961.1312f, -131.4942f, 76.40847f),
           // CameraDirection = new Vector3(-0.7768034f, 0.5811784f, -0.2425037f),
           // CameraRotation = new Rotator(-14.03436f, 4.840233E-06f, 53.19739f),
            VehiclePreviewLocation = new SpawnPlace(new Vector3(955.3662f, -127.888f, 74.37495f), 150.3339f),
            VehicleDeliveryLocations = new List<SpawnPlace>() {
                    new SpawnPlace(new Vector3(955.2133f, -133.7419f, 74.44804f),239.5793f),
                    new SpawnPlace(new Vector3(967.6667f, -141.4849f, 74.39858f), 239.8698f),
                    new SpawnPlace(new Vector3(986.6042f, -138.3367f, 73.09078f), 59.13091f),
                    new SpawnPlace(new Vector3(968.5606f, -122.4432f, 74.35313f), 153.9187f),
                }
        };


        GangDen LostMainDenMP = new GangDen(new Vector3(981.8542f, -103.0203f, 74.84874f), 220.3094f, "Lost M.C. Clubhouse", "", "LostDenMenu", "AMBIENT_GANG_LOST")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            //MapIcon = 226,
            BannerImagePath = "gangs\\lostmc.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            InteriorID = 245761,
            IsOnMPMap = true,
            IsOnSPMap = false,
            MaxAssaultSpawns = 25,
            IgnoreEntranceInteract = true,
            //HasVanillaGangSpawnedAroundToBeBlocked = true,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                //Exterior
                new GangConditionalLocation(new Vector3(987.3098f, -107.0706f, 74.32984f), 138.8955f, 35f),
                new GangConditionalLocation(new Vector3(975.0338f, -112.2333f, 74.35313f), 188.1823f, 35f),
                new GangConditionalLocation(new Vector3(960.2211f, -123.5155f, 74.35313f), 181.6973f, 35f),
                new GangConditionalLocation(new Vector3(981.1419f, -142.9071f, 74.23688f), 80.7939f, 35f),
                new GangConditionalLocation(new Vector3(976.2161f, -144.2681f, 74.24052f), 0.7361088f, 35f),




                //Interior
                new GangConditionalLocation(new Vector3(971.816467f, -99.8478241f, 73.8474f), -91.0297010254408f, 15f)  { TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario  },
                new GangConditionalLocation(new Vector3(974.8826f, -102.941391f, 73.8451462f), -48.3885776299785f, 15f)  { TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario  },
                new GangConditionalLocation(new Vector3(975.626953f, -103.198563f, 73.8451462f), 12.9338919715037f, 15f)  { TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario  },
                new GangConditionalLocation(new Vector3(986.7446f, -96.68695f, 73.8463f), 173.034743819776f, 15f)  { TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario  },
                new GangConditionalLocation(new Vector3(986.6274f, -97.9422f, 73.84628f), -4.41515547349861f, 15f)  { TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario  },
                new GangConditionalLocation(new Vector3(972.4932f, -100.849388f, 73.8489456f), -48.7439833502982f, 15f)  { TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario  },
                new GangConditionalLocation(new Vector3(980.1498f, -96.69946f, 73.84508f), 22.6684449107764f, 15f)  { TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario  },
                new GangConditionalLocation(new Vector3(979.5529f, -97.06739f, 73.84508f), 87.3733135600339f, 15f)  { TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario  },
                new GangConditionalLocation(new Vector3(976.9317f, -102.2214f, 73.8451462f), -24.8327929818831f, 15f)  { TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario  },
                new GangConditionalLocation(new Vector3(978.093445f, -98.76374f, 73.84623f), 43.4646929301833f, 15f)  { TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario  },
                new GangConditionalLocation(new Vector3(977.76355f, -102.326866f, 73.8451462f), 44.4574569018065f, 15f)  { TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario  },
                new GangConditionalLocation(new Vector3(977.7962f, -101.360527f, 73.8451462f), 137.841613394778f, 15f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario  },
                new GangConditionalLocation(new Vector3(987.0077f, -96.86357f, 73.84618f), 132.933656921688f, 15f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario  },
                new GangConditionalLocation(new Vector3(972.1758f, -98.9215546f, 73.8468246f), -143.163302691733f, 15f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario  },
                new GangConditionalLocation(new Vector3(979.759766f, -92.25629f, 73.85039f), -137.889398074892f, 15f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario  },
                new GangConditionalLocation(new Vector3(978.332f, -92.69629f, 73.8880539f), 12.6807592176149f, 15f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario  },
                new GangConditionalLocation(new Vector3(974.4178f, -95.79578f, 73.84578f), -78.1144301822788f, 15f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario  },
                new GangConditionalLocation(new Vector3(974.7609f, -95.0500641f, 73.84506f), -171.647727589323f, 15f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario  },
                new GangConditionalLocation(new Vector3(978.042664f, -91.79474f, 73.845314f), -128.700243660805f, 15f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario  },
                new GangConditionalLocation(new Vector3(978.9906f, -92.40033f, 73.84508f), 57.2908520760442f, 15f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario  },
                new GangConditionalLocation(new Vector3(980.6458f, -92.52759f, 73.85923f), 135.731352539532f, 15f) { TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario  },


            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(976.4726f, -132.778f, 73.21405f), 58.86649f, 35f),
                new GangConditionalLocation(new Vector3(960.3214f, -132.2091f, 73.69121f), 189.5355f, 35f),
                new GangConditionalLocation(new Vector3(972.7709f, -142.4676f, 73.60614f), 239.6191f, 35f),
                new GangConditionalLocation(new Vector3(972.8246f, -155.1686f, 72.8443f), 52.89192f, 35f),
            },
            //CameraPosition = new Vector3(961.1312f, -131.4942f, 76.40847f),
            // CameraDirection = new Vector3(-0.7768034f, 0.5811784f, -0.2425037f),
            // CameraRotation = new Rotator(-14.03436f, 4.840233E-06f, 53.19739f),
            VehiclePreviewLocation = new SpawnPlace(new Vector3(955.3662f, -127.888f, 74.37495f), 150.3339f),
            VehicleDeliveryLocations = new List<SpawnPlace>() {
                    new SpawnPlace(new Vector3(955.2133f, -133.7419f, 74.44804f),239.5793f),
                    new SpawnPlace(new Vector3(967.6667f, -141.4849f, 74.39858f), 239.8698f),
                    new SpawnPlace(new Vector3(986.6042f, -138.3367f, 73.09078f), 59.13091f),
                    new SpawnPlace(new Vector3(968.5606f, -122.4432f, 74.35313f), 153.9187f),
                }
        };



        GangDen LostDen2 = new GangDen(new Vector3(84.04226f, 3718.52f, 39.78923f), 50.35764f, "Lost M.C. The Range", "", "LostDenMenu", "AMBIENT_GANG_LOST")
        {
            CanInteractWhenWanted = true,
            //MapIcon = 226,
            BannerImagePath = "gangs\\lostmc.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            MaxAssaultSpawns = 30,
           // HasVanillaGangSpawnedAroundToBeBlocked = true,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(84.73796f, 3718.092f, 40.33084f), 60.53624f, 20f),
                new GangConditionalLocation(new Vector3(80.1135f, 3721.855f, 39.74522f), 150.8561f, 20f),
                new GangConditionalLocation(new Vector3(78.30386f, 3722.036f, 39.74952f), 266.9375f, 20f),
                new GangConditionalLocation(new Vector3(67.40894f, 3703.18f, 39.75497f), 338.0555f, 20f),
                new GangConditionalLocation(new Vector3(68.87956f, 3704.888f, 39.75497f), 120.9388f, 20f),
                new GangConditionalLocation(new Vector3(47.73441f, 3704.631f, 39.75497f), 302.5045f, 20f),
                new GangConditionalLocation(new Vector3(47.20625f, 3705.412f, 39.75497f), 269.3896f, 20f),
                new GangConditionalLocation(new Vector3(46.69005f, 3715.68f, 39.73444f), 245.4069f, 20f),
                new GangConditionalLocation(new Vector3(57.10126f, 3724.255f, 39.72771f), 196.2136f, 20f),
                new GangConditionalLocation(new Vector3(58.31903f, 3724.607f, 39.72731f), 176.8168f, 20f),
                new GangConditionalLocation(new Vector3(66.5742f, 3726.808f, 39.71051f), 173.2846f, 20f),
                new GangConditionalLocation(new Vector3(64.92879f, 3726.43f, 39.7145f), 256.0423f, 20f),
                new GangConditionalLocation(new Vector3(58.89564f, 3698.918f, 39.75497f), 319.3528f, 20f),
                new GangConditionalLocation(new Vector3(57.78175f, 3699.333f, 39.75499f), 307.2412f, 20f),
            },
           // CameraPosition = new Vector3(88.20795f, 3694.343f, 40.96601f),
           // CameraDirection = new Vector3(-0.2029471f, 0.9696398f, -0.1364228f),
           // CameraRotation = new Rotator(-7.840899f, 0f, 11.82144f),
            VehiclePreviewLocation = new SpawnPlace(new Vector3(86.68027f, 3702.844f, 39.09414f), 150.3339f),
            VehicleDeliveryLocations = new List<SpawnPlace>() {
                    new SpawnPlace(new Vector3(86.68027f, 3702.844f, 39.09414f), 168.2432f),
                }
        };
        GangDens.Add(LostMainDen);
        GangDens.Add(LostMainDenMP);
        GangDens.Add(LostDen2);
    }
    private void DefaultConfig_GangDens_Triads()
    {
        GangDen TriadMainDen = new GangDen(new Vector3(101.6865f, -819.3801f, 31.31512f), 341.2845f, "Triad Den", "", "TriadsDenMenu", "AMBIENT_GANG_WEICHENG")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            BannerImagePath = "gangs\\triad.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            MaxAssaultSpawns = 15,
            PossiblePedSpawns = new List<ConditionalLocation>() 
            {
                new GangConditionalLocation(new Vector3(99.74276f, -816.0181f, 31.40174f), 2.775635f, 65f),
                new GangConditionalLocation(new Vector3(108.1241f, -817.763f, 31.36724f), 273.9551f, 65f),
                new GangConditionalLocation(new Vector3(92.21131f, -817.7866f, 31.31864f), 55.81263f, 65f),
                new GangConditionalLocation(new Vector3(114.8412f, -819.1249f, 31.32478f), 342.2915f, 65f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(85.50445f, -818.2582f, 30.74365f), 155.5453f, 55f),
                new GangConditionalLocation(new Vector3(59.77588f, -848.8732f, 30.33472f), 334.658f, 55f),
                new GangConditionalLocation(new Vector3(87.69022f, -827.7574f, 30.66308f), 340.578f, 55f),
            }
        };
        GangDens.Add(TriadMainDen);
    }
    private void DefaultConfig_GangDens_Vagos()
    {
        GangDen VagosMainDen = new GangDen(new Vector3(967.6899f, -1867.115f, 31.44757f), 176.7243f, "Vagos Den", "", "VagosDenMenu", "AMBIENT_GANG_MEXICAN")
        {
            IsPrimaryGangDen = true,
            CanInteractWhenWanted = true,
            BannerImagePath = "gangs\\vagos.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            MaxAssaultSpawns = 20,
           // HasVanillaGangSpawnedAroundToBeBlocked = true,
            PossiblePedSpawns = new List<ConditionalLocation>() 
            {
                new GangConditionalLocation(new Vector3(963.3468f, -1869.211f, 31.21046f), 130.5827f, 35f),
                new GangConditionalLocation(new Vector3(970.806f, -1869.026f, 31.31658f), 163.6367f, 35f),
                new GangConditionalLocation(new Vector3(968.0837f, -1893.529f, 31.14556f), 357.2057f, 35f),
                new GangConditionalLocation(new Vector3(979.5701f, -1867.066f, 31.19429f), 259.3384f, 35f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(966.6174f, -1873.199f, 30.44685f), 41.87259f, 35f),
                new GangConditionalLocation(new Vector3(971.0187f, -1873.486f, 30.48772f), 40.74247f, 35f),
                new GangConditionalLocation(new Vector3(983.9246f, -1860.417f, 30.36379f), 179.4304f, 35f),
                new GangConditionalLocation(new Vector3(990.0909f, -1892.571f, 29.7773f), 263.3322f, 35f),
            }
        };
        GangDen VagosHangout1 = new GangDen()
        {
            //MapIcon = 47,
            AssignedAssociationID = "AMBIENT_GANG_MEXICAN",
            PossiblePedSpawns = new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                Location = new Vector3(279.9768f,-1957.178f,23.844f),
                Heading = -166.0208f,
                Percentage = 35f,
                },
                new GangConditionalLocation() {
                Location = new Vector3(281.0413f,-1958.143f,23.8485f),
                Heading = 79.9999f,
                Percentage = 35f,
                },
                new GangConditionalLocation() {
                Location = new Vector3(289.5965f,-1960.205f,22.51619f),
                Heading = 55.99986f,
                Percentage = 35f,
                },
                new GangConditionalLocation() {
                Location = new Vector3(289.101f,-1958.489f,22.55319f),
                Heading = -159.0003f,
                Percentage = 35f,
                },
                new GangConditionalLocation() {
                Location = new Vector3(287.7115f,-1959.962f,22.54695f),
                Heading = -84.99989f,
                Percentage = 35f,
                },
                },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                Location = new Vector3(298.1687f,-1976.326f,21.81265f),
                Heading = 48.75335f,
                Percentage = 35f,
                },
                },
            MenuID = "VagosDenMenu",
            Name = "Vagos Trap House 1",
            FullName = "Vagos Trap House",
            Description = "",
            IsEnabled = true,
            IsTemporarilyClosed = false,
            BannerImagePath = "gangs\\vagos.png",
            EntrancePosition = new Vector3(295.676f, -1971.918f, 22.90398f),
            EntranceHeading = -136.9998f,
            OpenTime = 0,
            CloseTime = 24,
            InteriorID = -1,
        };
        GangDen VagosHangout2 = new GangDen()
        {
            //MapIcon = 47,
            AssignedAssociationID = "AMBIENT_GANG_MEXICAN",
            PossiblePedSpawns = new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                Location = new Vector3(344.1845f,-1850.748f,27.31188f),
                Heading = -154.9997f,
                Percentage = 35f,
                },
                new GangConditionalLocation() {
                Location = new Vector3(345.2848f,-1851.904f,27.3096f),
                Heading = 51.99996f,
                Percentage = 35f,
                },
                new GangConditionalLocation() {
                Location = new Vector3(341.3383f,-1858.817f,27.35466f),
                Heading = -126.9997f,
                Percentage = 35f,
                },
                new GangConditionalLocation() {
                Location = new Vector3(341.1977f,-1860.319f,27.37708f),
                Heading = 0f,
                Percentage = 35f,
                },
                new GangConditionalLocation() {
                Location = new Vector3(342.5609f,-1859.336f,27.39991f),
                Heading = 84.99989f,
                Percentage = 35f,
                },
                },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                Location = new Vector3(335.3903f,-1836.632f,26.78196f),
                Heading = -135.7316f,
                Percentage = 35f,
                },
                },
            MenuID = "VagosDenMenu",
            Name = "Vagos Trap House 2",
            FullName = "Vagos Trap House",
            Description = "",
            IsEnabled = true,
            IsTemporarilyClosed = false,
            BannerImagePath = "gangs\\vagos.png",
            EntrancePosition = new Vector3(329.2924f, -1846.029f, 27.75098f),
            EntranceHeading = 50.04726f,
            OpenTime = 0,
            CloseTime = 24,
        };
        GangDen VagosHangout3 = new GangDen()
        {
            //MapIcon = 47,
            AssignedAssociationID = "AMBIENT_GANG_MEXICAN",
            PossiblePedSpawns = new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                Location = new Vector3(473.6748f,-1729.119f,28.80654f),
                Heading = -39.00011f,
                Percentage = 35f,
                },
                new GangConditionalLocation() {
                Location = new Vector3(474.325f,-1727.832f,28.87496f),
                Heading = -160.0003f,
                Percentage = 35f,
                },
                new GangConditionalLocation() {
                Location = new Vector3(464.2597f,-1731.531f,28.81734f),
                Heading = 75.99986f,
                Percentage = 35f,
                },
                new GangConditionalLocation() {
                Location = new Vector3(463.274f,-1730.221f,28.87736f),
                Heading = -154.0003f,
                Percentage = 35f,
                },
                new GangConditionalLocation() {
                Location = new Vector3(462.4821f,-1731.81f,28.88963f),
                Heading = -60.99995f,
                Percentage = 35f,
                },
                },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                Location = new Vector3(469.5431f,-1741.667f,28.42751f),
                Heading = 70.88543f,
                Percentage = 35f,
                },
                },
            MenuID = "VagosDenMenu",
            Name = "Vagos Trap House 3",
            FullName = "Vagos Trap House",
            Description = "",
            IsEnabled = true,
            IsTemporarilyClosed = false,
            BannerImagePath = "gangs\\vagos.png",
            EntrancePosition = new Vector3(479.7474f, -1736.071f, 29.15427f),
            EntranceHeading = -162.0002f,
            OpenTime = 0,
            CloseTime = 24,
        };
        GangDen VagosHangout4 = new GangDen()
        {
            //MapIcon = 47,
            AssignedAssociationID = "AMBIENT_GANG_MEXICAN",
            PossiblePedSpawns = new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                Location = new Vector3(342.6061f,-1851.355f,27.31567f),
                Heading = -84.99982f,
                Percentage = 35f,
                },
                new GangConditionalLocation() {
                Location = new Vector3(343.716f,-1851.246f,27.3132f),
                Heading = 122.9998f,
                Percentage = 35f,
                },
                new GangConditionalLocation() {
                Location = new Vector3(341.6856f,-1858.757f,27.35947f),
                Heading = -125.9998f,
                Percentage = 35f,
                },
                new GangConditionalLocation() {
                Location = new Vector3(341.8779f,-1860.188f,27.41061f),
                Heading = 0f,
                Percentage = 35f,
                },
                new GangConditionalLocation() {
                Location = new Vector3(343.2036f,-1859.359f,27.40417f),
                Heading = 85.9999f,
                Percentage = 35f,
                },
                },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                Location = new Vector3(336.048f,-1836.994f,26.77941f),
                Heading = -133.1032f,
                Percentage = 35f,
                },
                },
            MenuID = "VagosDenMenu",
            Name = "Vagos Trap House 4",
            FullName = "Vagos Trap House",
            Description = "",
            IsEnabled = true,
            IsTemporarilyClosed = false,
            BannerImagePath = "gangs\\vagos.png",
            EntrancePosition = new Vector3(329.2668f, -1845.745f, 27.75121f),
            EntranceHeading = 48.99995f,
            OpenTime = 0,
            CloseTime = 24,
        };
        GangDen VagosHangout5 = new GangDen()
        {
            //MapIcon = 47,
            AssignedAssociationID = "AMBIENT_GANG_MEXICAN",
            PossiblePedSpawns = new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                Location = new Vector3(364.1315f,-2066.042f,21.74919f),
                Heading = 57.00518f,
                Percentage = 35f,
                },
                new GangConditionalLocation() {
                Location = new Vector3(356.4194f,-2066.269f,21.27436f),
                Heading = 53.0007f,
                Percentage = 35f,
                },
                new GangConditionalLocation() {
                Location = new Vector3(355.0307f,-2066.106f,21.15525f),
                Heading = -82.99987f,
                Percentage = 35f,
                },
                new GangConditionalLocation() {
                Location = new Vector3(363.3668f,-2056.781f,21.71887f),
                Heading = 96.99989f,
                Percentage = 35f,
                },
                new GangConditionalLocation() {
                Location = new Vector3(362.9595f,-2057.564f,21.68725f),
                Heading = 0f,
                Percentage = 35f,
                },
                },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                Location = new Vector3(331.9642f,-2044.172f,20.29884f),
                Heading = -40.4487f,
                Percentage = 35f,
                },
                },
            MenuID = "VagosDenMenu",
            Name = "Vagos Hood Hangout",
            FullName = "Vagos Hood Hangout",
            Description = "",
            IsEnabled = true,
            IsTemporarilyClosed = false,
            BannerImagePath = "gangs\\vagos.png",
            EntrancePosition = new Vector3(365.2514f, -2064.708f, 21.74765f),
            EntranceHeading = 51.00192f,
            OpenTime = 0,
            CloseTime = 24,
        };
        GangDen VagosHangout6 = new GangDen()
        {
            //MapIcon = 47,
            AssignedAssociationID = "AMBIENT_GANG_MEXICAN",
            PossiblePedSpawns = new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                Location = new Vector3(452.7909f,-1576.325f,32.79226f),
                Heading = -87.99966f,
                Percentage = 35f,
                },
                new GangConditionalLocation() {
                Location = new Vector3(454.013f,-1577.252f,32.79216f),
                Heading = 10.9994f,
                Percentage = 35f,
                },
                new GangConditionalLocation() {
                Location = new Vector3(454.2974f,-1575.76f,32.79229f),
                Heading = 135.9998f,
                Percentage = 35f,
                },
                new GangConditionalLocation() {
                Location = new Vector3(459.7767f,-1577.874f,32.79194f),
                Heading = -112.9996f,
                Percentage = 35f,
                },
                new GangConditionalLocation() {
                Location = new Vector3(447.8612f,-1568.847f,32.79542f),
                Heading = 54.99996f,
                Percentage = 35f,
                },
                },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                Location = new Vector3(467.802f,-1579.866f,28.58691f),
                Heading = -129.7965f,
                Percentage = 35f,
                },
                },
            MenuID = "VagosDenMenu",
            Name = "Vagos Projects",
            FullName = "Vagos Projects",
            Description = "",
            IsEnabled = true,
            IsTemporarilyClosed = false,
            BannerImagePath = "gangs\\vagos.png",
            EntrancePosition = new Vector3(454.996f, -1580.31f, 32.82156f),
            EntranceHeading = -47.00023f,
            OpenTime = 0,
            CloseTime = 24,
        };

        GangDens.Add(VagosMainDen);
        GangDens.Add(VagosHangout1);
        //GangDens.Add(VagosHangout2);
        //GangDens.Add(VagosHangout3);
        //GangDens.Add(VagosHangout4);
        //GangDens.Add(VagosHangout5);
        //GangDens.Add(VagosHangout6);
    }
    private void DefaultConfig_ScrapYards()
    {
        ScrapYards = new List<ScrapYard>() {
            new ScrapYard(new Vector3(1520.797f, -2113.375f, 76.86716f     ), 270.4797f, "Wesley's Scrap Yard", "Don't Ask, Don't Tell!") { OpenTime = 0, CloseTime = 24 },
            new ScrapYard(new Vector3(909.7432f, 3554.745f, 33.81702f), 211.2794f, "Marina Drive Scrap", "Top value for your 'questionable' provenance") { OpenTime = 0, CloseTime = 24 },
            new ScrapYard(new Vector3(-195.9066f, 6264.628f, 31.48937f), 41.33705f, "Red's Machine Supplies", "Parts Bought and Sold!") { OpenTime = 0, CloseTime = 24 },
        };     
    }
    private void DefaultConfig_CarCrushers()
    {
        CarCrushers = new List<CarCrusher>() {
            new CarCrusher(new Vector3(-538.0128f, -1720.554f, 19.4278f), 324.869f, "The Crushinator", "Dead skunk in the trunk?") { 
                OpenTime = 0, 
                CloseTime = 24,
                BannerImagePath = "stores\\crushinator.png",
                CameraPosition = new Vector3(-527.7763f, -1704.601f, 27.12083f), 
                CameraDirection = new Vector3(0.005943439f, -0.9625598f, -0.2710043f), 
                CameraRotation = new Rotator(-15.72404f, 1.940237E-07f, -179.6462f) 
            },
        };    
    }
    private void DefaultConfig_DeadDrops()
    {
        DeadDrops = new List<DeadDrop>() {
            new DeadDrop(new Vector3(76.75777f,-605.3703f,43.22094f), 68.89698f, "Dead Drop", "the trash can by the IAA building" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true,IsOnMPMap = false, },
            new DeadDrop(new Vector3(-174.7691f,-674.9272f,33.27862f), 249.5148f, "Dead Drop", "the phone booth by the Arcadius Center" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(-1460.144f,-627.849f,29.69636f), 209.8004f, "Dead Drop", "the dumpster by Swallow" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(-1438.361f,-722.1968f,22.61556f), 171.4681f, "Dead Drop", "the daily rag newspaper stand by Pescado Rojo" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(1047.76f,-2464.003f,27.51101f), 44.61864f, "Dead Drop", "the dumpster by the gun dealers" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true, },
            new DeadDrop(new Vector3(1382.768f,-2042.507f,51.00203f), 30.00222f, "Dead Drop", "the dumpster by Covington Engineering" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(373.6489f, 351.1104f, 102.8209f), 257.8268f,"Dead Drop", "the dumpster behind 24/7") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(207.6413f, 337.0614f, 105.5466f), 164.1638f, "Dead Drop", "the dumpster behind Pitchers") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(98.17767f, 298.2877f, 110.0032f), 162.2623f, "Dead Drop", "the dumpster behind Up-N-Atom") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(-1125.012f, -1616.193f, 4.398425f), 197.6703f, "Dead Drop", "the dumpster behind Vitamin Seaside Juice Bar") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            //new DeadDrop(new Vector3(2569.69f, 2727.281f, 43.17296f), 300.3225f, "Dead Drop", "the port-o-potty at Davis Quartz") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(1532.096f, 3797.162f, 33.51709f), 203.1575f, "Dead Drop", "the back of the Boathouse") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(1987.924f, 3789.832f, 32.18082f), 128.6473f, "Dead Drop", "the toolbox behind Sandy's Gas") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(923.066f, 3652.77f, 32.59369f), 91.28005f, "Dead Drop", "the dumpster behind the Liquor Market") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(-125.5132f, 6345.283f, 31.49037f), 231.2274f, "Dead Drop", "the dumpster behind the Dream View Motel") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(1731.202f, 4758.293f, 41.89993f), 265.2589f, "Dead Drop", "the dumpster behind the supermarket") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(1692.661f, 6431.451f, 32.76194f), 336.9627f, "Dead Drop", "the payphone by 24/7") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },
            new DeadDrop(new Vector3(-3165.1f, 1114.949f, 20.79292f), 334.1037f, "Dead Drop", "the dumpster behind Nelsons General Store") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true },

            new DeadDrop(new Vector3(74.97916f,-608.9933f,43.22042f), 249.4708f, "Dead Drop", "the LS 24 newspaper stand near the IAA building" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true,IsOnMPMap = false, },
            new DeadDrop(new Vector3(75.60421f,-607.5473f,43.22063f), 249.4708f, "Dead Drop", "the Daily Rag newspaper stand by the IAA building" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true,IsOnMPMap = false, },
            new DeadDrop(new Vector3(75.88783f,-606.5583f,43.22063f), 249.4708f, "Dead Drop", "the Las Mietras newspaper stand by the IAA building" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true,IsOnMPMap = false, },
            new DeadDrop(new Vector3(-223.6883f,-703.9772f,32.59268f), 70.00474f, "Dead Drop", "the mailbox by Schlongberg & Sachs" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true,IsOnMPMap = false, },
            new DeadDrop(new Vector3(-251.0725f,-739.3169f,31.99848f), 187.8322f, "Dead Drop", "the Daily Rag newspaper stand by Schlongberg & Sachs" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true,IsOnMPMap = false, },
            new DeadDrop(new Vector3(-263.7065f,-850.4099f,30.48533f), 160.0125f, "Dead Drop", "the Daily Rag newspaper stand by Go Postal" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true,IsOnMPMap = false, },
            new DeadDrop(new Vector3(-1364.685f,-795.9746f,18.32434f), 140.7789f, "Dead Drop", "the trash can in front of the Hedera Hotel" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true,IsOnMPMap = false, },
            new DeadDrop(new Vector3(-1183.647f,-1257.851f,5.911644f), 260.6368f, "Dead Drop", "the trash can by Taco Libre" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true,IsOnMPMap = false, },
            new DeadDrop(new Vector3(-1205.34f,-1377.286f,3.174809f), 76.95641f, "Dead Drop", "the trash can by Steamboat Beers" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true,IsOnMPMap = false, },
            new DeadDrop(new Vector3(803.5943f,-2052.02f,28.30254f), 275.8022f, "Dead Drop", "the trash can by the PiBwasser Plant" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true,IsOnMPMap = false, },
            new DeadDrop(new Vector3(1316.705f,-1657.766f,50.23988f), 309.4677f, "Dead Drop", "the dumpster behind Los Santos Tattoos" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true,IsOnMPMap = false, },
            new DeadDrop(new Vector3(426.0467f,100.2095f,99.24073f), 337.0558f, "Dead Drop", "the mailbox by Stargaze" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true,IsOnMPMap = false, },
            new DeadDrop(new Vector3(436.3528f,88.48115f,98.49297f), 159.5108f, "Dead Drop", "the dumpster by Wandlust" ) { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true,IsOnMPMap = false, },
            new DeadDrop(new Vector3(-69.79692f, 282.6463f, 105.0663f), 329.5786f, "Dead Drop", "the trash can outside The Gentry Manor Hotel") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true,IsOnMPMap = false, },
            new DeadDrop(new Vector3(-391.8292f, 294.4202f, 84.89133f), 86.40096f, "Dead Drop", "the dumpster behind The Last Train Diner") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true,IsOnMPMap = false, },
            new DeadDrop(new Vector3(-515.2796f, 26.77622f, 44.57598f), 85.66344f, "Dead Drop", "the Daily Rag newspaper stand by Serentiy Wellness") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true,IsOnMPMap = false, },
            new DeadDrop(new Vector3(-480.7322f, -12.00511f, 45.27599f), 358.5777f, "Dead Drop", "the mailbox in front of Fruit Machine") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true,IsOnMPMap = false, },
            new DeadDrop(new Vector3(-514.0649f, -40.31401f, 44.51626f), 353.1382f, "Dead Drop", "the dumpster behind The Little Teapot") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true,IsOnMPMap = false, },
            new DeadDrop(new Vector3(-792.2766f, -250.7747f, 37.20045f), 121.051f, "Dead Drop", "the mailbox in front of Luxury Autos") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true,IsOnMPMap = false, },
            new DeadDrop(new Vector3(-1237.857f, -554.2683f, 28.91684f), 127.322f, "Dead Drop", "the mailbox in front of City Hall") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true,IsOnMPMap = false, },
            new DeadDrop(new Vector3(-1323.729f, -675.4391f, 26.51783f), 36.19815f, "Dead Drop", "the Los Santos Meteor newspaper stand in front of Astro Theater") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true,IsOnMPMap = false, },
            new DeadDrop(new Vector3(2544.616f, 378.9412f, 108.6173f), 264.3849f, "Dead Drop", "the boxes behind the 24/7") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true,IsOnMPMap = false, },
            new DeadDrop(new Vector3(-678.5425f, 5833.054f, 17.33131f), 310.8695f, "Dead Drop", "the mailbox in front of The BayviewLodge") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true,IsOnMPMap = false, },
            new DeadDrop(new Vector3(1725.162f, 4732.389f, 42.1343f), 103.1211f, "Dead Drop", "the boxes behind The Feed Store") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true,IsOnMPMap = false, },
            new DeadDrop(new Vector3(-2297.884f, 248.9572f, 169.6021f), 213.5057f, "Dead Drop", "the trash can near the parking lot of the Kortz Center") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true,IsOnMPMap = false, },
            new DeadDrop(new Vector3(172.9656f, -1799.744f, 29.20135f), 133.2921f, "Dead Drop", "the mailbox in front of The Locksmith") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true,IsOnMPMap = false, },
            new DeadDrop(new Vector3(88.87651f, -1662.244f, 29.29333f), 28.54718f, "Dead Drop", "the Daily Rag newspaper stand near the Convenience Store") { OpenTime = 0,CloseTime = 24, IsEnabled = false,CanInteractWhenWanted = true,IsOnMPMap = false, },
        };     
    }
    private void DefaultConfig_LibertyCity()
    {
        PlacesOfInterest_Liberty placesOfInterest_Liberty = new PlacesOfInterest_Liberty(this);
        placesOfInterest_Liberty.DefaultConfig();
    }
    private void DefaultConfig_SunshineDream()
    {
        PlacesOfInterest_SunshineDream placesOfInterest_SunshineDream = new PlacesOfInterest_SunshineDream();
        placesOfInterest_SunshineDream.DefaultConfig();
    }
    public void Setup()
    {
        foreach (GameLocation bl in AllLocations())
        {
            if (bl.HasBannerImage)
            {
                if (!File.Exists($"Plugins\\LosSantosRED\\images\\{bl.BannerImagePath}"))
                {
                    bl.BannerImagePath = "";
                    EntryPoint.WriteToConsole($"Locations ERROR Banner Image file DOES NOT EXIST {bl.Name}, REMOVING FROM LOCATION", 0);
                }
            }
        }
    }
    private void DefaultConfig_2008()
    {
        PossibleLocations OldPossibleLocations = new PossibleLocations();
        List<GangDen> GangDens2008 = new List<GangDen>();
        foreach (GangDen gd in GangDens)
        {
            if (gd.AssignedAssociationID != "AMBIENT_GANG_BALLAS" && gd.AssignedAssociationID != "AMBIENT_GANG_FAMILY" && gd.AssignedAssociationID != "AMBIENT_GANG_SALVA"
                && gd.AssignedAssociationID != "AMBIENT_GANG_PAVANO"
                && gd.AssignedAssociationID != "AMBIENT_GANG_LUPISELLA"
                && gd.AssignedAssociationID != "AMBIENT_GANG_MESSINA"
                && gd.AssignedAssociationID != "AMBIENT_GANG_ANCELOTTI")
            {
                GangDens2008.Add(gd);
            }
        }
        OldPossibleLocations.DeadDrops.AddRange(DeadDrops);
        OldPossibleLocations.CarCrushers.AddRange(CarCrushers);
        OldPossibleLocations.ScrapYards.AddRange(ScrapYards);
        OldPossibleLocations.GangDens.AddRange(GangDens2008);
        OldPossibleLocations.GunStores.AddRange(GunStores);
        OldPossibleLocations.Hotels.AddRange(Hotels);
        OldPossibleLocations.ApartmentBuildings.AddRange(ApartmentBuildings);
        OldPossibleLocations.Residences.AddRange(Residences);
        OldPossibleLocations.CityHalls.AddRange(CityHalls);
        OldPossibleLocations.PoliceStations.AddRange(PoliceStations);
        OldPossibleLocations.Hospitals.AddRange(Hospitals);
        OldPossibleLocations.FireStations.AddRange(FireStations);
        OldPossibleLocations.Restaurants.AddRange(Restaurants);
        OldPossibleLocations.Pharmacies.AddRange(Pharmacies);
        OldPossibleLocations.Dispensaries.AddRange(Dispensaries);
        OldPossibleLocations.HeadShops.AddRange(HeadShops);
        OldPossibleLocations.HardwareStores.AddRange(HardwareStores);
        OldPossibleLocations.PawnShops.AddRange(PawnShops);
        OldPossibleLocations.Landmarks.AddRange(Landmarks);
        OldPossibleLocations.Banks.AddRange(Banks);
        OldPossibleLocations.ConvenienceStores.AddRange(ConvenienceStores);
        OldPossibleLocations.LiquorStores.AddRange(LiquorStores);
        OldPossibleLocations.GasStations.AddRange(GasStations);
        OldPossibleLocations.Bars.AddRange(Bars);
        OldPossibleLocations.FoodStands.AddRange(FoodStands);
        OldPossibleLocations.CarDealerships.AddRange(Dealerships);
        OldPossibleLocations.VehicleExporters.AddRange(VehicleExporters);
        OldPossibleLocations.GamblingDens.AddRange(GamblingDens);
        OldPossibleLocations.Forgers.AddRange(Forgers);
        OldPossibleLocations.RepairGarages.AddRange(RepairGarages);
        OldPossibleLocations.DriveThrus.AddRange(DriveThrus);
        OldPossibleLocations.ClothingShops.AddRange(ClothingShops);
        OldPossibleLocations.BusStops.AddRange(BusStops);
        OldPossibleLocations.Prisons.AddRange(Prisons);
        OldPossibleLocations.SubwayStations.AddRange(SubwayStations);
        OldPossibleLocations.Morgues.AddRange(Morgues);
        OldPossibleLocations.SportingGoodsStores.AddRange(SportingGoodsStores);
        OldPossibleLocations.Airports.AddRange(Airports);
        OldPossibleLocations.IllicitMarketplaces.AddRange(illicitMarketplaces);
        OldPossibleLocations.BlankLocations.AddRange(BlankLocationPlaces);
        OldPossibleLocations.MilitaryBases.AddRange(MilitaryBasePlaces);
        OldPossibleLocations.StoredSpawns.AddRange(TunnelSpawnPlaces);
        OldPossibleLocations.BarberShops.AddRange(BarberShopPlaces);
        OldPossibleLocations.PlasticSurgeryClinics.AddRange(PlasticSurgeryClinics);
        OldPossibleLocations.TattooShops.AddRange(TattooShopPlaces);
        PossibleLocations.PedCustomizerLocation = DefaultPedCustomizerLocation;

        //OldPossibleLocations.GangDens.RemoveAll(x => x.AssignedAssociationID == "AMBIENT_GANG_BALLAS" || x.AssignedAssociationID == "AMBIENT_GANG_FAMILY" || x.AssignedAssociationID == "AMBIENT_GANG_SALVA");
        OldPossibleLocations.GangDens.Add(new GangDen(new Vector3(393.403f, -782.4543f, 29.28772f), 269.1115f, "Ballas Den", "", "BallasDenMenu", "AMBIENT_GANG_BALLAS")
        {
            CanInteractWhenWanted = true,
            BannerImagePath = "gangs\\ballas.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            IsPrimaryGangDen = true,
            PossiblePedSpawns = new List<ConditionalLocation>() {

                new GangConditionalLocation(new Vector3(395.0874f, -779.8005f, 29.29059f), 295.3088f, 50f),
                new GangConditionalLocation(new Vector3(395.585f, -786.7841f, 29.28836f), 236.7289f, 50f),
                new GangConditionalLocation(new Vector3(393.6364f, -771.2427f, 29.2868f), 321.6246f, 50f),
                new GangConditionalLocation(new Vector3(385.2623f, -771.7565f, 29.2923f), 356.8999f, 50f),
                new GangConditionalLocation(new Vector3(398.6558f, -788.6539f, 29.28695f), 214.5293f, 50f),
            }
        });
        OldPossibleLocations.GangDens.Add(new GangDen(new Vector3(86.11255f, -1959.272f, 21.12167f), 318.5057f, "The Families Den", "The OGs", "FamiliesDenMenu", "AMBIENT_GANG_FAMILY")
        {
            CanInteractWhenWanted = true,
            BannerImagePath = "gangs\\families.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            IsPrimaryGangDen = true,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(84.76484f, -1953.536f, 20.8518f), 334.0088f, 35f),
                new GangConditionalLocation(new Vector3(87.02995f, -1947.637f, 20.74858f), 303.2596f, 35f),
                new GangConditionalLocation(new Vector3(95.30958f, -1954.979f, 20.75126f), 314.5049f, 35f),
                new GangConditionalLocation(new Vector3(84.23887f, -1932.319f, 20.74922f), 19.71852f, 35f),
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation(new Vector3(94.70525f, -1960.741f, 20.06409f), 322.6508f, 45f),
                new GangConditionalLocation(new Vector3(108.7719f, -1951.384f, 20.01156f), 294.1027f, 45f),
                new GangConditionalLocation(new Vector3(113.3118f, -1933.905f, 19.9819f), 36.83981f, 45f),
                new GangConditionalLocation(new Vector3(68.90487f, -1922.226f, 20.57331f), 130.7354f, 45f),
            }
        });//This is in DAVIS near Grove Street
        OldPossibleLocations.GangDens.Add(new GangDen(new Vector3(511.4065f, -1790.909f, 28.50743f), 90.88252f, "Varrios Los Aztecas Den", "", "VarriosDenMenu", "AMBIENT_GANG_SALVA")
        {
            CanInteractWhenWanted = true,
            BannerImagePath = "gangs\\varrios.png",
            OpenTime = 0,
            CloseTime = 24,
            IsEnabled = true,
            IsPrimaryGangDen = true,
            PossiblePedSpawns = new List<ConditionalLocation>() {

                new GangConditionalLocation(new Vector3(511.2142f, -1794.088f, 28.50298f), 102.9549f, 50f),
                new GangConditionalLocation(new Vector3(507.1f, -1787.592f, 28.4884f), 82.19876f, 50f),
                new GangConditionalLocation(new Vector3(506.4553f, -1793.272f, 28.49493f), 68.23071f, 50f),
                new GangConditionalLocation(new Vector3(503.4097f, -1782.823f, 28.49299f), 107.5499f, 50f),
                new GangConditionalLocation(new Vector3(529.1724f, -1793.627f, 28.50298f), 148.2789f, 50f),
            }
        });//THIS IS IN RANCHO Near a motel and the davis polcie station?

        Serialization.SerializeParam(OldPossibleLocations, "Plugins\\LosSantosRED\\AlternateConfigs\\LosSantos2008\\Locations_LosSantos2008.xml");
    }
}
