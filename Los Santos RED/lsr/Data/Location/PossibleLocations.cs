using System;
using System.Collections.Generic;


public class PossibleLocations
{


    public PossibleLocations()
    {

    }
    public List<DeadDrop> DeadDrops { get; private set; } = new List<DeadDrop>();
    public List<ScrapYard> ScrapYards { get; private set; } = new List<ScrapYard>();
    public List<CarCrusher> CarCrushers { get; private set; } = new List<CarCrusher>();
    public List<GangDen> GangDens { get; private set; } = new List<GangDen>();
    public List<GunStore> GunStores { get; private set; } = new List<GunStore>();
    public List<Hotel> Hotels { get; private set; } = new List<Hotel>();
    public List<Residence> Residences { get; private set; } = new List<Residence>();
    public List<CityHall> CityHalls { get; private set; } = new List<CityHall>();
    public List<VendingMachine> VendingMachines { get; private set; } = new List<VendingMachine>();
    public List<PoliceStation> PoliceStations { get; private set; } = new List<PoliceStation>();
    public List<Hospital> Hospitals { get; private set; } = new List<Hospital>();
    public List<FireStation> FireStations { get; private set; } = new List<FireStation>();
    public List<Restaurant> Restaurants { get; private set; } = new List<Restaurant>();
    public List<Pharmacy> Pharmacies { get; private set; } = new List<Pharmacy>();
    public List<Dispensary> Dispensaries { get; private set; } = new List<Dispensary>();
    public List<HeadShop> HeadShops { get; private set; } = new List<HeadShop>();
    public List<HardwareStore> HardwareStores { get; private set; } = new List<HardwareStore>();
    public List<PawnShop> PawnShops { get; private set; } = new List<PawnShop>();
    public List<Landmark> Landmarks { get; private set; } = new List<Landmark>();
    public List<BeautyShop> BeautyShops { get; private set; } = new List<BeautyShop>();
    public List<Bank> Banks { get; private set; } = new List<Bank>();
    public List<ConvenienceStore> ConvenienceStores { get; private set; } = new List<ConvenienceStore>();
    public List<GasStation> GasStations { get; private set; } = new List<GasStation>();
    public List<LiquorStore> LiquorStores { get; private set; } = new List<LiquorStore>();
    public List<FoodStand> FoodStands { get; private set; } = new List<FoodStand>();
    public List<Dealership> CarDealerships { get; private set; } = new List<Dealership>();
    public List<VehicleExporter> VehicleExporters { get; private set; } = new List<VehicleExporter>();
    public List<Forger> Forgers { get; private set; } = new List<Forger>();
    public List<RepairGarage> RepairGarages { get; private set; } = new List<RepairGarage>();
    public List<Bar> Bars { get; private set; } = new List<Bar>();
    public List<DriveThru> DriveThrus { get; private set; } = new List<DriveThru>();
    public List<ClothingShop> ClothingShops { get; private set; } = new List<ClothingShop>();
    public List<BusStop> BusStops { get; private set; } = new List<BusStop>();
    public List<SubwayStation> SubwayStations { get; private set; } = new List<SubwayStation>();
    public List<Prison> Prisons { get; private set; } = new List<Prison>();
    public List<Morgue> Morgues { get; private set; } = new List<Morgue>();
    public List<SportingGoodsStore> SportingGoodsStores { get; private set; } = new List<SportingGoodsStore>();
    public List<Airport> Airports { get; private set; } = new List<Airport>();
    public List<IllicitMarketplace> IllicitMarketplaces { get; private set; } = new List<IllicitMarketplace>();
    public List<BlankLocation> BlankLocations { get; private set; } = new List<BlankLocation>();

  //  public List<InteractableLocation> AllLocationsList { get; private set; } = new List<InteractableLocation>();


    public List<GameLocation> InteractableLocations()
    {
        List<GameLocation> AllLocations = new List<GameLocation>();
        AllLocations.AddRange(PoliceStations);
        AllLocations.AddRange(Hospitals);
        AllLocations.AddRange(FireStations);
        AllLocations.AddRange(Banks);
        AllLocations.AddRange(BeautyShops);
        AllLocations.AddRange(Landmarks);
        AllLocations.AddRange(Prisons);
        AllLocations.AddRange(SubwayStations);
        AllLocations.AddRange(DeadDrops);
        AllLocations.AddRange(ScrapYards);
        AllLocations.AddRange(CarCrushers);
        AllLocations.AddRange(GangDens);
        AllLocations.AddRange(GunStores);
        AllLocations.AddRange(Hotels);
        AllLocations.AddRange(Residences);
        AllLocations.AddRange(CityHalls);
        AllLocations.AddRange(VendingMachines);
        AllLocations.AddRange(Restaurants);
        AllLocations.AddRange(Pharmacies);
        AllLocations.AddRange(Dispensaries);
        AllLocations.AddRange(HeadShops);
        AllLocations.AddRange(HardwareStores);
        AllLocations.AddRange(PawnShops);
        AllLocations.AddRange(ConvenienceStores);
        AllLocations.AddRange(LiquorStores);
        AllLocations.AddRange(GasStations);
        AllLocations.AddRange(Bars);
        AllLocations.AddRange(FoodStands);
        AllLocations.AddRange(CarDealerships);
        AllLocations.AddRange(VehicleExporters);
        AllLocations.AddRange(Forgers);
        AllLocations.AddRange(RepairGarages);
        AllLocations.AddRange(DriveThrus);
        AllLocations.AddRange(ClothingShops);
        AllLocations.AddRange(BusStops);
        AllLocations.AddRange(Morgues);
        AllLocations.AddRange(SportingGoodsStores);
        AllLocations.AddRange(Airports);
        AllLocations.AddRange(IllicitMarketplaces);
        AllLocations.AddRange(BlankLocations);
        return AllLocations;
    }

    public List<GameLocation> GenericTaskLocations()
    {
        List<GameLocation> AllLocations = new List<GameLocation>();
        AllLocations.AddRange(Banks);
        AllLocations.AddRange(BeautyShops);
        AllLocations.AddRange(Hotels);
        AllLocations.AddRange(Residences);
        AllLocations.AddRange(Restaurants);
        AllLocations.AddRange(Pharmacies);
        AllLocations.AddRange(Dispensaries);
        AllLocations.AddRange(HeadShops);
        AllLocations.AddRange(HardwareStores);
        AllLocations.AddRange(PawnShops);
        AllLocations.AddRange(ConvenienceStores);
        AllLocations.AddRange(LiquorStores);
        AllLocations.AddRange(GasStations);
        AllLocations.AddRange(Bars);
        AllLocations.AddRange(SportingGoodsStores);
        return AllLocations;
    }


    public List<GameLocation> WitnessTaskLocations()
    {
        List<GameLocation> AllLocations = new List<GameLocation>();
        AllLocations.AddRange(Banks);
        AllLocations.AddRange(Bars);
        AllLocations.AddRange(BeautyShops);
        AllLocations.AddRange(CarDealerships);
        AllLocations.AddRange(CityHalls);
        AllLocations.AddRange(ConvenienceStores);
        AllLocations.AddRange(Dispensaries);
        AllLocations.AddRange(GasStations);
        AllLocations.AddRange(HardwareStores);
        AllLocations.AddRange(HeadShops);
        AllLocations.AddRange(Hospitals);
        AllLocations.AddRange(Hotels);
        AllLocations.AddRange(LiquorStores);
        AllLocations.AddRange(PawnShops);
        AllLocations.AddRange(Pharmacies);
        AllLocations.AddRange(Restaurants);
        AllLocations.AddRange(Landmarks);
        AllLocations.AddRange(SportingGoodsStores);
        return AllLocations;
    }
    public List<GameLocation> RobberyTaskLocations()
    {
        List<GameLocation> AllLocations = new List<GameLocation>();
        AllLocations.AddRange(Banks);
        AllLocations.AddRange(ConvenienceStores);
        AllLocations.AddRange(Dispensaries);
        AllLocations.AddRange(GasStations);
        AllLocations.AddRange(HardwareStores);
        AllLocations.AddRange(HeadShops);
        AllLocations.AddRange(LiquorStores);
        AllLocations.AddRange(PawnShops);
        AllLocations.AddRange(Pharmacies);
        AllLocations.AddRange(SportingGoodsStores);
        return AllLocations;
    }
}

