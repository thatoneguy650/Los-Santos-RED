using System;
using System.Collections.Generic;


public class DispatchableVehicles_RaceCars
{
    private DispatchableVehicles DispatchableVehicles;


    public List<DispatchableVehicleGroup> GroupsToAdd { get; private set; } = new List<DispatchableVehicleGroup>();

    public List<DispatchableVehicle> RegularSports_Group { get; private set; }
    public List<DispatchableVehicle> RegularMuscle_Group { get; private set; }
    public List<DispatchableVehicle> RegularSuper_Group { get; private set; }
    public List<DispatchableVehicle> BuffaloRegular_Group { get; private set; }
    public List<DispatchableVehicle> GauntletRegular_Group { get; private set; }
    public List<DispatchableVehicle> VigeroRegular_Group { get; private set; }
    public List<DispatchableVehicle> DominatorRegular_Group { get; private set; }
    public List<DispatchableVehicle> OtherMuscleRegular_Group { get; private set; }
    public List<DispatchableVehicle> OldMuscleRegular_Group { get; private set; }
    public List<DispatchableVehicle> MuscleRace_Group { get; private set; }
    public List<DispatchableVehicle> RegularRally_Group { get; private set; }
    public List<DispatchableVehicle> RegularMotorcycle_Group { get; private set; }
    public List<DispatchableVehicle> ChopperMotorcycleRegular_Group { get; private set; }
    public List<DispatchableVehicle> SportsMotorcycleRegular_Group { get; private set; }
    public List<DispatchableVehicle> OtherMotorcycleRegular_Group { get; private set; }
    public List<DispatchableVehicle> RegularOffRoad_Group { get; private set; }
    public List<DispatchableVehicle> Regular4x4OffRoad_Group { get; private set; }
    public List<DispatchableVehicle> ATVOffRoad_Group { get; private set; }
    public List<DispatchableVehicle> TrophyTruckOffRoad_Group { get; private set; }
    public List<DispatchableVehicle> HotringTruck_Group { get; private set; }
    public List<DispatchableVehicle> Hotring_Group { get; private set; }
    public List<DispatchableVehicle> LowRider_Group { get; private set; }
    public List<DispatchableVehicle> OtherSportsRegular_Group { get; private set; }
    public List<DispatchableVehicle> RetroSportsRegular_Group { get; private set; }
    public List<DispatchableVehicle> TunerSports_Group { get; private set; }
    public List<DispatchableVehicle> EuroSports_Group { get; private set; }
    public List<DispatchableVehicle> UsSports_Group { get; private set; }
    public List<DispatchableVehicle> ClassicSports_Group { get; private set; }
    public List<DispatchableVehicle> MotoXOffRoad_Group { get; private set; }
    public List<DispatchableVehicle> OffRoadMotorcycle_Group { get; private set; }
    public List<DispatchableVehicle> BuggyOffRoad_Group { get; private set; }
    public List<DispatchableVehicle> BoatsWater_Group { get; private set; }
    public List<DispatchableVehicle> JetskiWater_Group { get; private set; }
    public List<DispatchableVehicle> GoKarts_Group { get; private set; }
    public List<DispatchableVehicle> StreetKart_Group { get; private set; }
    public List<DispatchableVehicle> RaceKart_Group { get; private set; }
    public List<DispatchableVehicle> RegularCompact_Group { get; private set; }
    public List<DispatchableVehicle> Compact_Group { get; private set; }
    public List<DispatchableVehicle> ATVvsMOTO_OffRoad_Group { get; private set; }

    public DispatchableVehicles_RaceCars(DispatchableVehicles dispatchableVehicles)
    {
        DispatchableVehicles = dispatchableVehicles;
    }
    public void DefaultConfig()
    {
        Hotring();
        LowRider();
        Motorcycle();
        MuscleCars();
        Offroad();
        Rally();
        SuperCars();
        SportsCars();
        Karts();
        CompactCars();
        //WaterSports();

        //General Groups
        GroupsToAdd.Add(new DispatchableVehicleGroup("SportsCars_Racing", "Sports Cars", "Collection of sports cars", RegularSports_Group, DispatchbleVehicleGroupType.Racing)); // DEFAULT FIRST GROUP

        GroupsToAdd.Add(new DispatchableVehicleGroup("ATVvsMOTO_Racing", "ATVs vs Motorcycles", "Collection of ATVs and motorcycles for off-road racing", ATVvsMOTO_OffRoad_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("CompactCars_Racing", "Compact Cars", "Collection of Compact vehicles", RegularCompact_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("GoKarts_Racing", "Go Karts", "Collection of Karts", GoKarts_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("LowRiders_Racing", "LowRider Racing", "Collection of LowRiders's with customizations", LowRider_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("MuscleCars_Racing", "Muscle Cars", "Collection of muscle cars", RegularMuscle_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("Motorcycles_Racing", "Motorcycles", "Collection of racing motorcycles", RegularMotorcycle_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("OffRoad_Racing", "Off-Road Vehicles", "Collection of off-road vehicles", RegularOffRoad_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("RallyCars_Racing", "Rally Cars", "Collection of rally cars", RegularRally_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("SuperCars_Racing", "Super Cars", "Collection of super cars", RegularSuper_Group, DispatchbleVehicleGroupType.Racing));

        // Compacts
        GroupsToAdd.Add(new DispatchableVehicleGroup("Compacts_Racing", "Compacts", "Collection of compacts", Compact_Group, DispatchbleVehicleGroupType.Racing));

        //specialized muscle
        GroupsToAdd.Add(new DispatchableVehicleGroup("Buffalo_Racing", "Bravado Buffalos", "Collection of different generations of Bravado Buffalo", BuffaloRegular_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("Gauntlet_Racing", "Bravado Gauntlets", "Collection of different generations of Bravado Gauntlet", GauntletRegular_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("Vigero_Racing", "Declasse Vigero", "Collection of different generations of Declasse Ruiner", VigeroRegular_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("Dominator_Racing", "Vapid Dominators", "Collection of different generations of Vapid Dominator", DominatorRegular_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("OtherMuscle_Racing", "Other Muscle Cars", "Collection of all muscle cars", OtherMuscleRegular_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("OldMuscle_Racing", "Old Muscle Cars", "Collection of old muscle cars", OldMuscleRegular_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("MuscleRace_Racing", "Race Muscle Cars", "Collection of muscle cars with racing liveries", MuscleRace_Group, DispatchbleVehicleGroupType.Racing));

        //specialized sports
        GroupsToAdd.Add(new DispatchableVehicleGroup("OtherSportsRegular_Group", "Regular Sports Cars", "Collection of all sports cars", RegularSports_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("TunerSports_Racing", "Asian Sports Cars", "Collection of modified sports cars", TunerSports_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("EuroSports_Racing", "Euro Sports Cars", "Collection of European sports cars", EuroSports_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("UsSports_Racing", "US Sports Cars", "Collection of US sports cars", UsSports_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("ClassicSports_Racing", "Classic Sports Cars", "Collection of classic sports cars", ClassicSports_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("RetroSports_Racing", "Retro Sports Cars", "Collection of retro sports cars", RetroSportsRegular_Group, DispatchbleVehicleGroupType.Racing));
        //specialized super 

        //specialized motorcycles
        GroupsToAdd.Add(new DispatchableVehicleGroup("SportMotorcycles_Racing", "Sport Motorcycles", "Collection of sport motorcycles", SportsMotorcycleRegular_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("OtherMotorcycles_Racing", "Other Motorcycles", "Collection of all motorcycles", OtherMotorcycleRegular_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("ChopperMotorcycles_Racing", "Chopper Motorcycles", "Collection of chopper motorcycles", ChopperMotorcycleRegular_Group, DispatchbleVehicleGroupType.Racing));

        //Offroad
        GroupsToAdd.Add(new DispatchableVehicleGroup("ATV_Racing", "ATVs", "Collection of ATV vehicles", ATVOffRoad_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("Buggy_Racing", "Buggies", "Collection of Offroad Buggies vehicles", BuggyOffRoad_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("MotoX_Racing", "Moto X", "Collection of Moto X vehicles", MotoXOffRoad_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("OffRoadMotorcycles_Racing", "Off-Road Motorcycles", "Collection of off-road motorcycles", OffRoadMotorcycle_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("TrophyTruck_Racing", "Trophy Trucks", "Collection of trophy trucks", TrophyTruckOffRoad_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("4x4OffRoad_Racing", "4x4 Off-Road", "Collection of off-road vehicles", Regular4x4OffRoad_Group, DispatchbleVehicleGroupType.Racing));

        //Hotring
        GroupsToAdd.Add(new DispatchableVehicleGroup("Hotring_Racing", "Hotring Racing", "Collection of Hotring's with racing liveries", Hotring_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("Everon_Racing", "Everon Racing", "Collection of Everon's with racing liveries", HotringTruck_Group, DispatchbleVehicleGroupType.Racing));

        //Karts
        GroupsToAdd.Add(new DispatchableVehicleGroup("StreetKarts_Racing", "Street Karts", "Collection of street karts", StreetKart_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("RaceKarts_Racing", "Race Karts", "Collection of race karts ", RaceKart_Group, DispatchbleVehicleGroupType.Racing));

        ////Water Sports
        //GroupsToAdd.Add(new DispatchableVehicleGroup("Boats_Racing", "Boats", "Collection of boats", BoatsWater_Group, DispatchbleVehicleGroupType.Racing));
        //GroupsToAdd.Add(new DispatchableVehicleGroup("Jetskis_Racing", "Jetskis", "Collection of jetskis", JetskiWater_Group, DispatchbleVehicleGroupType.Racing));

    }
    private void Motorcycle()
    {
        RegularMotorcycle_Group = new List<DispatchableVehicle>()
        {

        };
        ChopperMotorcycleRegular_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("avarus", 100, 100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 64, 73, 88, 107, 134, 138 } },
            new DispatchableVehicle("daemon", 100, 100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 36, 38, 41, 49, 64, 73, 88, 107, 134, 138 } },
            new DispatchableVehicle("daemon2", 100, 100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 36, 38, 41, 49, 64, 73, 88, 107, 134, 138 } },
            new DispatchableVehicle("hexer", 100, 100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 36, 38, 41, 49, 64, 73, 88, 107, 134, 138 } },
            new DispatchableVehicle("nightblade", 100, 100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 5, 12, 27, 64, 73, 88, 111, 117, 118, 138 } },
            new DispatchableVehicle("sanctus", 100, 100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 5, 12, 27, 64, 73, 88, 111, 117, 118, 138 } },
            new DispatchableVehicle("zombiea", 100, 100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 36, 38, 41, 49, 64, 73, 88, 107, 134, 138 } },
            new DispatchableVehicle("zombieb", 100, 100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 36, 38, 41, 49, 64, 73, 88, 107, 134, 138 } },
        };
        SportsMotorcycleRegular_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("akuma",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 12, 27, 28, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("bati",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 12, 27, 64, 75, 88, 107, 134, 138 } },
            new DispatchableVehicle("bati2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 12, 27, 64, 75, 88, 107, 134, 138 } },
            new DispatchableVehicle("carbonrs",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 64, 75, 88, 111, 134 } },
            new DispatchableVehicle("defiler",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 12, 27, 28, 64, 75, 88, 107, 134, 138 } },
            new DispatchableVehicle("diablous",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 12, 27, 28, 64, 75, 88, 107, 134, 138 } },
            new DispatchableVehicle("diablous2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 12, 27, 28, 64, 75, 88, 107, 134, 138 } },
            new DispatchableVehicle("double",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 12, 27, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("fcr",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 12, 27, 64, 75, 88, 111, 134 } },
            new DispatchableVehicle("fcr2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 12, 27, 64, 75, 88, 111, 134 } },
            new DispatchableVehicle("hakuchou",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 12, 27, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("hakuchou2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 12, 27, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("lectro",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 4, 12, 27, 28, 64, 75, 88, 111, 117, 138 } },
            new DispatchableVehicle("shinobi",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 4, 12, 27, 28, 64, 75, 88, 111, 134, 138 } },
        };
        OtherMotorcycleRegular_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("chimera", 100, 100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 64, 75, 88, 107, 134, 138 } },
            new DispatchableVehicle("innovation", 100, 100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 28, 39, 64, 75, 88, 107, 134, 138 } },
            new DispatchableVehicle("reever", 100, 100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 12, 27, 28, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("rrocket", 100, 100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 4, 12, 27, 28, 64, 75, 88, 111, 117, 134, 138 } },
            new DispatchableVehicle("stryder", 100, 100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 64, 75, 88, 107, 134, 138 } },
            new DispatchableVehicle("thrust", 100, 100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 64, 75, 88, 107, 134 } },
            new DispatchableVehicle("vindicator", 100, 100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 64, 75, 88, 107, 134, 138 } },
        };
        RegularMotorcycle_Group = new List<DispatchableVehicle>() { };
        RegularMotorcycle_Group.AddRange(ChopperMotorcycleRegular_Group);
        RegularMotorcycle_Group.AddRange(SportsMotorcycleRegular_Group);
        RegularMotorcycle_Group.AddRange(OtherMotorcycleRegular_Group);


    }
    private void Rally()
    {
        RegularRally_Group = new List<DispatchableVehicle>()
        {          
            new DispatchableVehicle("calico",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f },
            new DispatchableVehicle("comet4",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f },
            new DispatchableVehicle("flashgt",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f },
            new DispatchableVehicle("gb200",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f },
            new DispatchableVehicle("issi8",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f },
            new DispatchableVehicle("omnis",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f },
            new DispatchableVehicle("retinue2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f },
            new DispatchableVehicle("tropos",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f },
            new DispatchableVehicle("uranus",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f },
        };
    }
    private void Offroad()
    {
        RegularOffRoad_Group = new List<DispatchableVehicle>()
        {

        };

        Regular4x4OffRoad_Group = new List<DispatchableVehicle>()
{
    new DispatchableVehicle("Bodhi2",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 3, 4, 12, 27, 28, 39, 52, 61, 64, 75, 88, 111, 117, 118, 134, 138 } },
    new DispatchableVehicle("brawler",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 39, 64, 75, 88, 111, 134, 138 } },
    new DispatchableVehicle("caracara2",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 3, 4, 6, 12, 27, 28, 64, 73, 88, 111, 112, 117, 118, 135, 138 } },
    new DispatchableVehicle("contender",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 39, 64, 75, 88, 111, 134, 138 } },
    new DispatchableVehicle("draugur",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 64, 75, 88, 111, 134, 138 } },
    new DispatchableVehicle("dubsta3",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 3, 4, 12, 27, 28, 52, 61, 64, 75, 88, 111, 134, 138 } },
    new DispatchableVehicle("everon",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 3, 4, 6, 12, 27, 28, 39, 64, 73, 88, 111, 112, 117, 118, 135, 138 } },
    new DispatchableVehicle("freecrawler",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 3, 4, 12, 27, 28, 39, 64, 75, 88, 111, 134, 138 } },
    new DispatchableVehicle("hellion",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 3, 4, 6, 12, 27, 28, 39, 64, 73, 88, 111, 112, 117, 118, 134, 138 } },
    new DispatchableVehicle("l35",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 39, 52, 61, 64, 75, 88, 111, 134, 138 } },
    new DispatchableVehicle("mesa3",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 3, 4, 12, 27, 28, 39, 64, 75, 88, 111, 134, 138 } },
    new DispatchableVehicle("monstrociti",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 3, 4, 6, 12, 27, 28, 39, 64, 73, 88, 111, 112, 117, 118, 134, 138 } },
    new DispatchableVehicle("patriot3",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 39, 64, 75, 88, 111, 134, 138 } },
    new DispatchableVehicle("rebel2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 39, 52, 61, 64, 75, 88, 111, 134, 138 } },
    new DispatchableVehicle("riata",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 3, 4, 12, 27, 28, 39, 64, 75, 88, 111, 134, 138 } },
    new DispatchableVehicle("seminole2",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 3, 4, 6, 12, 27, 28, 39, 64, 73, 88, 111, 112, 117, 118, 134, 138 } },
    new DispatchableVehicle("terminus",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 3, 4, 6, 12, 27, 28, 39, 64, 73, 88, 111, 112, 117, 118, 134, 138 } },
    new DispatchableVehicle("yosemite3",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 3, 4, 12, 27, 28, 39, 52, 61, 64, 75, 88, 111, 134, 138 } },
    new DispatchableVehicle("yosemite1500",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 3, 4, 12, 27, 28, 39, 52, 61, 64, 75, 88, 111, 134, 138 } },
};

        ATVOffRoad_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("blazer",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f },
            new DispatchableVehicle("blazer3",100,100) {SetRandomCustomization = true, RandomCustomizationPercentage = 85f},
            new DispatchableVehicle("blazer4",100,100) {SetRandomCustomization = true, RandomCustomizationPercentage = 85f},
            new DispatchableVehicle("verus",100,100) {SetRandomCustomization = true, RandomCustomizationPercentage = 85f},
        };

        BuggyOffRoad_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("dune",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 3, 4, 12, 27, 28, 39, 52, 61, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("outlaw",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 3, 4, 12, 27, 28, 39, 52, 61, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("ratel",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 3, 4, 6, 12, 27, 28, 39, 64, 73, 88, 111, 112, 117, 118, 134, 138 } },
            new DispatchableVehicle("vagrant",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 3, 4, 12, 27, 28, 39, 52, 61, 64, 75, 88, 111, 134, 138 } },
        };

        MotoXOffRoad_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("manchez",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f,RequiredPedGroup = "MotoX"},// RequiredGroupIsDriverOnly = true is needed/required to make 
            new DispatchableVehicle("sanchez",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,RequiredPedGroup = "MotoX" }, //  Sure these guys get put on the moto x vehicle group
            new DispatchableVehicle("sanchez2",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f,RequiredPedGroup = "MotoX" }, //  and not the regular race peds 
        };

        OffRoadMotorcycle_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("bf400",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f },
            new DispatchableVehicle("cliffhanger",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f },
            new DispatchableVehicle("enduro",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f},
            new DispatchableVehicle("gargoyle",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f },
            new DispatchableVehicle("manchez",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f},
            new DispatchableVehicle("manchez2",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f},
            new DispatchableVehicle("manchez3",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f},
            new DispatchableVehicle("sanchez",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f}, 
            new DispatchableVehicle("sanchez2",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 85f},
        };

        TrophyTruckOffRoad_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("trophytruck",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 75f },
            new DispatchableVehicle("trophytruck2",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 75f },
        };

        RegularOffRoad_Group = new List<DispatchableVehicle>() { };
        RegularOffRoad_Group.AddRange(BuggyOffRoad_Group);
        RegularOffRoad_Group.AddRange(Regular4x4OffRoad_Group);
        RegularOffRoad_Group.AddRange(TrophyTruckOffRoad_Group);

        ATVvsMOTO_OffRoad_Group = new List<DispatchableVehicle>() { };
        ATVvsMOTO_OffRoad_Group.AddRange(ATVOffRoad_Group);
        ATVvsMOTO_OffRoad_Group.AddRange(OffRoadMotorcycle_Group);

    }
    private void SportsCars()
    {

        RegularSports_Group = new List<DispatchableVehicle>()
        {

        };

        TunerSports_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("calico",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 4, 12, 27, 64, 75, 88, 111, 134 } },
            new DispatchableVehicle("elegy",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 12, 27, 28, 64, 75, 88, 111, 134, 138, 135 } },
            new DispatchableVehicle("elegy2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 12, 27, 28, 64, 75, 88, 111, 134, 138, 135 } },
            new DispatchableVehicle("euros",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 4, 5, 12, 64, 75, 88, 111, 134 } },
            new DispatchableVehicle("eurosX32",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 4, 5, 12, 64, 75, 88, 111, 134 } },
            new DispatchableVehicle("futo2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 12, 27, 28, 64, 73, 88 } },
            new DispatchableVehicle("fr36",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 12, 27, 28, 64, 73, 88 } },
            new DispatchableVehicle("jester",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 12, 27, 28, 64, 75, 88, 111, 134, 138, 135 } },
            new DispatchableVehicle("jester3",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 12, 27, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("jester4",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 12, 27, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("kanjosj",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 4, 5, 6, 12, 64, 73, 88, 111, 135, 138, 117 } },
            new DispatchableVehicle("kuruma",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 12, 27, 28, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("penumbra",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 4, 5, 12, 27, 28, 64, 73, 88, 111, 134, 138, 140 } },
            new DispatchableVehicle("penumbra2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 4, 5, 12, 27, 28, 64, 73, 88, 111, 134, 138, 140 } },
            new DispatchableVehicle("previon",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 4, 12, 27, 49, 64, 75, 88, 111, 134 } },
            new DispatchableVehicle("r300",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 4, 12, 27, 64, 75, 88, 111, 134 } },
            new DispatchableVehicle("remus",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 12, 27, 28, 64, 73, 88, 111, 134, 138, 135 } },
            new DispatchableVehicle("rt3000",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 4, 12, 27, 64, 75, 88, 111, 134 } },
            new DispatchableVehicle("sugoi",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 4, 5, 12, 64, 73, 88, 111, 134, 138, 135 } },
            new DispatchableVehicle("sultan2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 12, 27, 28, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("sultan3",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 12, 27, 28, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("vectre",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 12, 27, 49, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("zr350",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 12, 27, 28, 64, 73, 88, 111, 134, 138, 135 } },
        };
        EuroSports_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("bestiagts",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 12, 27, 28, 64, 88, 111, 117, 134, 138 } },
            new DispatchableVehicle("cinquemila",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 1, 3, 4, 5, 12, 27, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("comet2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 1, 4, 5, 12, 27, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("comet5",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 1, 4, 5, 12, 27, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("comet6",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 1, 4, 5, 12, 27, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("comet7",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 1, 4, 5, 12, 27, 64, 75, 88, 111, 134, 138, 139 } },
            new DispatchableVehicle("cypher",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 4, 5, 12, 27, 64, 88, 111, 117, 138 } },
            new DispatchableVehicle("drafter",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 1, 4, 5, 12, 27, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("komoda",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 1, 4, 5, 12, 27, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("feltzer2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 1, 4, 5, 12, 27, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("neon",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 1, 3, 4, 5, 12, 27, 64, 88, 111, 117, 138 } },
            new DispatchableVehicle("omnisegt",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 4, 5, 12, 27, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("paragon",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 5, 6, 12, 52, 61, 64, 73, 111, 112, 117, 118, 138 } },
            new DispatchableVehicle("paragon3",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 5, 6, 12, 52, 61, 64, 73, 111, 112, 117, 118, 138 } },
            new DispatchableVehicle("pariah",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 12, 27, 28, 64, 88, 111, 117, 134, 138 } },
            new DispatchableVehicle("rhinehart",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 1, 4, 5, 12, 27, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("schafter3",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 1, 3, 4, 5, 12, 27, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("schlagen",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 4, 5, 12, 27, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("schwarzer",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 1, 3, 4, 5, 12, 27, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("sentinel5",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 1, 3, 4, 5, 12, 27, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("sm722",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 4, 5, 12, 27, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("tailgater",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 1, 3, 4, 5, 12, 27, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("tailgater2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 1, 3, 4, 5, 12, 27, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("tenf2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 1, 4, 5, 12, 27, 64, 75, 88, 111, 134, 138 } },
        };
        UsSports_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("alpha",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 12, 27, 64, 75, 88, 134, 138 } },
            new DispatchableVehicle("banshee",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 39, 64, 75, 88, 107, 134, 138 } },
            new DispatchableVehicle("banshee2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 39, 64, 75, 88, 107, 134, 138 } },
            new DispatchableVehicle("buffalo2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 12, 27, 39, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("buffalo4",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 12, 27, 39, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("coquette",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 39, 64, 75, 88, 107, 134, 138 } },
            new DispatchableVehicle("coquette4",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 36, 38, 41, 64, 75, 88, 89, 134, 138 } },
            new DispatchableVehicle("coquette6",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 36, 38, 41, 64, 75, 88, 89, 134, 138 } },
            new DispatchableVehicle("dominator",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 12, 27, 39, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("dominator3",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 36, 38, 41, 49, 64, 75, 88, 89, 107, 134, 138 } },
            new DispatchableVehicle("dominator7", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 4, 5, 6, 12, 27, 28, 64, 73, 88, 111, 112, 117, 118, 135, 138 } },
            new DispatchableVehicle("dominator9",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 36, 38, 41, 49, 64, 75, 88, 89, 107, 134, 138 } },
            new DispatchableVehicle("fusilade",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 12, 27, 39, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("gauntlet",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 39, 64, 75, 88, 107, 134, 138 } },
            new DispatchableVehicle("gauntlet4",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 36, 38, 41, 64, 75, 88, 89, 134, 138 } },
            new DispatchableVehicle("raiden",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 5, 12, 64, 111, 112, 117, 118, 138, 140 } },
            new DispatchableVehicle("revolter",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 36, 38, 41, 49, 64, 75, 88, 89, 107, 134, 138 } },
            new DispatchableVehicle("vigero2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 12, 27, 39, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("vigero3",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 12, 27, 39, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("vstr",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 12, 27, 64, 73, 88, 111, 112, 122, 135, 138 } },
        };
        OtherSportsRegular_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("banshee3",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 64, 75, 88, 89, 107, 134, 138 } },
            new DispatchableVehicle("stingertt",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 6, 12, 27, 38, 64, 73, 111, 112, 117, 118, 135 } },
            new DispatchableVehicle("panthere",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 36, 38, 41, 49, 64, 75, 88, 89, 107, 111, 134, 138, 140, 142, 148 } },
            new DispatchableVehicle("tenf",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 5, 6, 12, 61, 64, 111, 112, 117, 118, 138, 140 } },
            new DispatchableVehicle("tenf2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 5, 6, 12, 61, 64, 111, 112, 117, 118, 138, 140 } },
            new DispatchableVehicle("corsita",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("growler",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 64, 75, 88, 89, 111, 134, 138, 140 } },
            new DispatchableVehicle("vectre",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 12, 27, 49, 64, 75, 88, 89, 107, 134, 138, 140 } },
            new DispatchableVehicle("comet6",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 5, 12, 27, 28, 64, 75, 88, 89, 111, 134, 138, 140 } },
            new DispatchableVehicle("jester4",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 12, 27, 64, 75, 88, 89, 107, 111, 134, 138, 140 } },
            new DispatchableVehicle("euros",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 12, 27, 28, 64, 75, 88, 89, 107, 111, 134, 138 } },
            new DispatchableVehicle("vstr",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 5, 12, 64, 73, 111, 112, 122, 135, 138, 140 } },
            new DispatchableVehicle("komoda",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 5, 6, 12, 64, 88, 111, 112, 117, 118, 138, 140 } },
            new DispatchableVehicle("jugular",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 5, 6, 12, 52, 61, 64, 73, 111, 112, 117, 118, 138 } },
            new DispatchableVehicle("neo",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 12, 27, 28, 36, 38, 41, 49, 64, 75, 88, 89, 107, 111, 134, 138, 140, 142, 148 } },
            new DispatchableVehicle("rapidgt4",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 5, 6, 12, 52, 61, 64, 73, 111, 112, 117, 118, 138 } },
            new DispatchableVehicle("schlagen",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 5, 12, 27, 64, 75, 111, 134, 138, 140 } },
            new DispatchableVehicle("pariah",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 5, 12, 64, 73, 111, 112, 122, 135, 138, 140 } },
            new DispatchableVehicle("raiden",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 5, 6, 12, 64, 111, 112, 117, 118, 138, 140 } },
        };

        RetroSportsRegular_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("astrale",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 5, 12, 27, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("comet3",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 1, 4, 5, 12, 27, 64, 75, 88, 111, 134, 138, 139 } },
            new DispatchableVehicle("eurosX32",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 4, 5, 12, 64, 75, 88, 111, 134 } },
            new DispatchableVehicle("hardy",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 12, 27, 64, 73, 88, 111, 135, 138 } },
            new DispatchableVehicle("nebula",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 12, 27, 64, 73, 88, 111, 112, 122, 135, 138 } },
            new DispatchableVehicle("pigalle",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 4, 5, 12, 27, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("retinue2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 36, 38, 41, 49, 64, 75, 88, 89, 107, 134, 138 } },
            new DispatchableVehicle("savestra",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 36, 38, 41, 49, 64, 75, 88, 89, 107, 134, 138 } },
            new DispatchableVehicle("sentinel3",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 5, 12, 27, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("sentinel4",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 5, 12, 27, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("sentinel6",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 5, 12, 27, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("uranus",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 36, 38, 41, 49, 64, 75, 88, 89, 107, 134, 138 } },
            new DispatchableVehicle("vorschlaghammer",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f,OptionalColors = new List<int>() { 0, 1, 3, 4, 5, 12, 27, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("warrener",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 36, 38, 41, 49, 64, 75, 88, 89, 107, 134, 138 } },
            new DispatchableVehicle("z190",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 12, 27, 64, 73, 88, 111, 134 } },
            new DispatchableVehicle("zion3",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 5, 12, 27, 64, 75, 88, 111, 134, 138 } },
        };


        RegularSports_Group = new List<DispatchableVehicle>() { };
        RegularSports_Group.AddRange(TunerSports_Group);
        RegularSports_Group.AddRange(EuroSports_Group);
        RegularSports_Group.AddRange(UsSports_Group);
        RegularSports_Group.AddRange(OtherSportsRegular_Group);
        RegularSports_Group.AddRange(RetroSportsRegular_Group);

        ClassicSports_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("casco",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f },
            new DispatchableVehicle("cheetah2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f }, 
            new DispatchableVehicle("cheetah3",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f },
            new DispatchableVehicle("coquette2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f },
            new DispatchableVehicle("feltzer3",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f },
            new DispatchableVehicle("gt500",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f },
            new DispatchableVehicle("infernus2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f },
            new DispatchableVehicle("itali2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f },
            new DispatchableVehicle("jb7002",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f },
            new DispatchableVehicle("mamba",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f },
            new DispatchableVehicle("monroe",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f },
            new DispatchableVehicle("nightshade",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f },
            new DispatchableVehicle("stingergt",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f },
            new DispatchableVehicle("stromberg",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f },
            new DispatchableVehicle("swinger",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f },
            new DispatchableVehicle("torero",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f },
            new DispatchableVehicle("turismo2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f },
            new DispatchableVehicle("viseris",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f },
        };

    }
    private void SuperCars()
    {
        RegularSuper_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("pipistrello",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 64, 75, 88, 111, 117, 138 } },
            new DispatchableVehicle("virtue",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 5, 12, 27, 64, 73, 88, 111, 117, 118, 138 } },
            new DispatchableVehicle("turismo3",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("entity3",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 5, 6, 12, 64, 73, 88, 111, 117, 118, 135, 138 } },
            new DispatchableVehicle("zeno",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 5, 12, 64, 73, 88, 111, 117, 138 } },
            new DispatchableVehicle("ignus",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 5, 6, 12, 27, 64, 73, 88, 111, 112, 117, 118, 138, 140 } },
            new DispatchableVehicle("champion",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 5, 6, 12, 27, 64, 73, 88, 111, 112, 117, 118, 135, 138 } },
            new DispatchableVehicle("furia",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("emerus",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 5, 6, 12, 27, 28, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("tyrant",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 5, 6, 12, 27, 64, 73, 88, 111, 117, 118, 138 } },
            new DispatchableVehicle("sc1",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("cyclone",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 5, 6, 12, 64, 73, 88, 111, 117, 118, 138 } },
            new DispatchableVehicle("italigtb2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 5, 6, 12, 27, 28, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("nero2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 5, 6, 12, 27, 64, 73, 88, 111, 112, 117, 118, 138, 140 } },
            new DispatchableVehicle("italigtb",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 5, 6, 12, 27, 28, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("tempesta",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 5, 6, 12, 27, 64, 73, 88, 111, 117, 118, 138 } },
            new DispatchableVehicle("penetrator",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("fmj",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 5, 6, 12, 27, 64, 73, 88, 111, 117, 118, 138 } },
            new DispatchableVehicle("reaper",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 5, 6, 12, 27, 64, 73, 88, 111, 117, 118, 138 } },
            new DispatchableVehicle("banshee2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 64, 75, 88, 89, 107, 134, 138 } },
            new DispatchableVehicle("zentorno",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 5, 6, 12, 27, 64, 73, 88, 111, 117, 118, 138 } },
            new DispatchableVehicle("vacca",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 5, 12, 27, 64, 75, 88, 111, 134, 138 } },
            new DispatchableVehicle("cheetah",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 4, 5, 6, 12, 27, 64, 73, 88, 111, 117, 118, 138 } },
        };
        //cyclone2 removed, exists only on Enhanced.
    }
    private void MuscleCars()
    {
        RegularMuscle_Group = new List<DispatchableVehicle>()
        {

        };
        BuffaloRegular_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("buffalo", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },//og
            new DispatchableVehicle("buffalo2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },//upgrade
            new DispatchableVehicle("buffalo4", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },//stx
        };
        GauntletRegular_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("gauntlet", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },//og 
            new DispatchableVehicle("gauntlet3", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },//old one
            new DispatchableVehicle("gauntlet4", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },//gauntlet hellfire
            new DispatchableVehicle("gauntlet5",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },//old with custom
        };
        VigeroRegular_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("vigero",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },//OLD camaro 
            new DispatchableVehicle("vigero2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },//camaro non convertible
            new DispatchableVehicle("vigero3", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },//camaro convertible                  
        };
        DominatorRegular_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("ellie", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("dominator", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },//OG 05            
            new DispatchableVehicle("dominator3", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },//new non convertible one
            new DispatchableVehicle("dominator7", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },//dom 90s
            new DispatchableVehicle("dominator8", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },//dom gtt 60s
            new DispatchableVehicle("dominator9", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },//new convertible one
            new DispatchableVehicle("dominator10", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },//dom gtx  80s                
        };

        OtherMuscleRegular_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("dominator7", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("dominator10",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("ruiner4", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
        };

        OldMuscleRegular_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("blade", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 12, 28, 38, 41, 49, 64, 75, 88, 107, 134, 138, 145 } },
            new DispatchableVehicle("buccaneer",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 36, 38, 41, 49, 64, 73, 88, 89, 107, 111, 134, 138 } },
            new DispatchableVehicle("deviant", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 4, 5, 6, 12, 27, 28, 64, 75, 88, 111, 117, 118, 135, 138 } },
            new DispatchableVehicle("dominator8", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 4, 5, 6, 12, 27, 28, 64, 73, 88, 111, 112, 117, 118, 135, 138 } },
            new DispatchableVehicle("dukes", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 4, 5, 6, 12, 27, 28, 64, 75, 88, 107, 111, 134, 138 } },
            new DispatchableVehicle("faction",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 36, 38, 41, 49, 64, 75, 88, 89, 107, 111, 134, 138 } },
            new DispatchableVehicle("gauntlet3", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 4, 5, 6, 12, 27, 28, 36, 38, 41, 49, 64, 75, 88, 89, 107, 134, 142, 148 } },
            new DispatchableVehicle("gauntlet5", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 4, 5, 6, 12, 27, 28, 36, 38, 41, 49, 64, 75, 88, 89, 107, 134, 142, 148 } },
            new DispatchableVehicle("impaler",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 36, 38, 41, 49, 64, 75, 88, 89, 107, 111, 134, 138 } },
            new DispatchableVehicle("peyote2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 36, 38, 41, 49, 64, 73, 88, 89, 111, 134, 138 } },
            new DispatchableVehicle("phoenix", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 4, 5, 6, 12, 27, 28, 64, 75, 88, 107, 111, 134, 138 } },
            new DispatchableVehicle("ruiner", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 4, 5, 6, 12, 27, 28, 64, 73, 88, 111, 112, 117, 118, 135, 138 } },
            new DispatchableVehicle("sabregt", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 36, 38, 41, 49, 64, 75, 88, 89, 107, 134, 138 } },
            new DispatchableVehicle("stalion", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 4, 5, 6, 12, 27, 28, 64, 75, 88, 107, 111, 134, 138 } },
            new DispatchableVehicle("tampa", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 4, 5, 6, 12, 27, 28, 64, 73, 88, 111, 112, 117, 118, 135, 138 } },
            new DispatchableVehicle("tornado6", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 36, 38, 41, 49, 64, 75, 88, 89, 107, 111, 134, 138 } },
            new DispatchableVehicle("tulip", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 36, 38, 41, 49, 64, 75, 88, 89, 107, 134, 138 } },
            new DispatchableVehicle("tulip2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 36, 38, 41, 49, 64, 75, 88, 89, 107, 134, 138 } },
            new DispatchableVehicle("vamos", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 4, 5, 6, 12, 27, 28, 64, 73, 88, 111, 112, 117, 118, 135, 138, 140 } },
            new DispatchableVehicle("vigero", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 4, 5, 6, 12, 27, 28, 64, 75, 88, 107, 111, 134, 138 } },
        };

        RegularMuscle_Group = new List<DispatchableVehicle>() { };
        RegularMuscle_Group.AddRange(BuffaloRegular_Group);
        RegularMuscle_Group.AddRange(GauntletRegular_Group);
        RegularMuscle_Group.AddRange(VigeroRegular_Group);
        RegularMuscle_Group.AddRange(DominatorRegular_Group);
        RegularMuscle_Group.AddRange(OtherMuscleRegular_Group);
        RegularMuscle_Group.AddRange(OldMuscleRegular_Group);

        MuscleRace_Group = new List<DispatchableVehicle>
        {
            new DispatchableVehicle("gauntlet2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("dominator2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },//pisswasser old dominator
            new DispatchableVehicle("buffalo3", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },//Race car
            new DispatchableVehicle("stalion2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },//Race car
        };
    }
    private void LowRider()
    {
        LowRider_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("buccaneer2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 36, 38, 41, 49, 64, 73, 88, 89, 107, 111, 134, 138 } },
            new DispatchableVehicle("chino2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 36, 38, 41, 49, 64, 73, 88, 89, 107, 111, 134, 138 } },
            new DispatchableVehicle("eudora", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 36, 38, 41, 49, 64, 73, 88, 89, 107, 111, 134, 138 } },
            new DispatchableVehicle("faction2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 36, 38, 41, 49, 64, 73, 88, 89, 107, 111, 134, 138 } },
            new DispatchableVehicle("glendale2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 36, 38, 41, 49, 64, 73, 88, 89, 107, 111, 134, 138 } },
            new DispatchableVehicle("manana2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 36, 38, 41, 49, 64, 73, 88, 89, 107, 111, 134, 138 } },
            new DispatchableVehicle("minivan2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 36, 38, 41, 49, 64, 73, 88, 89, 107, 111, 134, 138 } },
            new DispatchableVehicle("moonbeam2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 36, 38, 41, 49, 64, 73, 88, 89, 107, 111, 134, 138 } },
            new DispatchableVehicle("peyote3", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 36, 38, 41, 49, 64, 73, 88, 89, 107, 111, 134, 138 } },
            new DispatchableVehicle("primo2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 36, 38, 41, 49, 64, 73, 88, 89, 107, 111, 134, 138 } },
            new DispatchableVehicle("sabregt2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 36, 38, 41, 49, 64, 73, 88, 89, 107, 111, 134, 138 } },
            new DispatchableVehicle("slamvan3", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 36, 38, 41, 49, 64, 73, 88, 89, 107, 111, 134, 138 } },
            new DispatchableVehicle("tornado5", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 36, 38, 41, 49, 64, 73, 88, 89, 107, 111, 134, 138 } },
            new DispatchableVehicle("voodoo", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 36, 38, 41, 49, 64, 73, 88, 89, 107, 111, 134, 138 } },
            new DispatchableVehicle("voodoo2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 36, 38, 41, 49, 64, 73, 88, 89, 107, 111, 134, 138 } },
            new DispatchableVehicle("virgo2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 85f, OptionalColors = new List<int>() { 0, 1, 3, 4, 12, 27, 28, 36, 38, 41, 49, 64, 73, 88, 89, 107, 111, 134, 138 } },
        };
    }
    private void Hotring()
    {
        Hotring_Group = new List<DispatchableVehicle>
        {
            new DispatchableVehicle("gauntlet6",100,100)
            {
                RequiredPrimaryColorID = -1,
                RequiredSecondaryColorID = -1,
                MaxRandomDirtLevel = 0f,
                SetRandomCustomization = true,
                RandomCustomizationPercentage = 100f,
                VehicleMods = new List<DispatchableVehicleMod>()
                {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(0,15),
                            new DispatchableVehicleModValue(1,15),
                            new DispatchableVehicleModValue(2,15),
                            new DispatchableVehicleModValue(3,15),
                            new DispatchableVehicleModValue(4,15),
                            new DispatchableVehicleModValue(5,15),
                            new DispatchableVehicleModValue(6,15),
                            new DispatchableVehicleModValue(7,15),
                            new DispatchableVehicleModValue(8,15),
                            new DispatchableVehicleModValue(9,15),
                            new DispatchableVehicleModValue(10,15),
                            new DispatchableVehicleModValue(11,15),
                            new DispatchableVehicleModValue(12,15),
                            new DispatchableVehicleModValue(13,15),
                            new DispatchableVehicleModValue(14,15),
                            new DispatchableVehicleModValue(15,15),
                            new DispatchableVehicleModValue(16,15),
                            new DispatchableVehicleModValue(17,15),
                            new DispatchableVehicleModValue(18,15),
                            new DispatchableVehicleModValue(19,15),
                            new DispatchableVehicleModValue(20,15),
                            new DispatchableVehicleModValue(21,15),
                            new DispatchableVehicleModValue(22,15),
                            new DispatchableVehicleModValue(23,15),
                            new DispatchableVehicleModValue(24,15),
                            new DispatchableVehicleModValue(25,15),
                            new DispatchableVehicleModValue(26,15),
                            new DispatchableVehicleModValue(27,15),
                            new DispatchableVehicleModValue(28,15),
                            new DispatchableVehicleModValue(29,15),
                            new DispatchableVehicleModValue(30,15),
                        },
                    },
                },
            },
            new DispatchableVehicle("hotring", 100, 100)
            {
                RequiredPrimaryColorID = -1,
                RequiredSecondaryColorID = -1,
                MaxRandomDirtLevel = 0f,
                SetRandomCustomization = true,
                RandomCustomizationPercentage = 100f,
                VehicleMods = new List<DispatchableVehicleMod>()
    {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(0,15),
                            new DispatchableVehicleModValue(1,15),
                            new DispatchableVehicleModValue(2,15),
                            new DispatchableVehicleModValue(3,15),
                            new DispatchableVehicleModValue(4,15),
                            new DispatchableVehicleModValue(5,15),
                            new DispatchableVehicleModValue(6,15),
                            new DispatchableVehicleModValue(7,15),
                            new DispatchableVehicleModValue(8,15),
                            new DispatchableVehicleModValue(9,15),
                            new DispatchableVehicleModValue(10,15),
                            new DispatchableVehicleModValue(11,15),
                            new DispatchableVehicleModValue(12,15),
                            new DispatchableVehicleModValue(13,15),
                            new DispatchableVehicleModValue(14,15),
                            new DispatchableVehicleModValue(15,15),
                            new DispatchableVehicleModValue(16,15),
                            new DispatchableVehicleModValue(17,15),
                            new DispatchableVehicleModValue(18,15),
                            new DispatchableVehicleModValue(19,15),
                            new DispatchableVehicleModValue(20,15),
                            new DispatchableVehicleModValue(21,15),
                            new DispatchableVehicleModValue(22,15),
                            new DispatchableVehicleModValue(23,15),
                            new DispatchableVehicleModValue(24,15),
                            new DispatchableVehicleModValue(25,15),
                            new DispatchableVehicleModValue(26,15),
                            new DispatchableVehicleModValue(27,15),
                            new DispatchableVehicleModValue(28,15),
                            new DispatchableVehicleModValue(29,15),
                            new DispatchableVehicleModValue(30,15),
                        },
                    },
                },
            },
        };

        HotringTruck_Group = new List<DispatchableVehicle>()
       {
           new DispatchableVehicle("everon2", 100, 100)
           {
               RequiredPrimaryColorID = -1,
               RequiredSecondaryColorID = -1,
               MaxRandomDirtLevel = 0f,
               SetRandomCustomization = true,
                RandomCustomizationPercentage = 100f,

               VehicleMods = new List<DispatchableVehicleMod>()
           {
                    new DispatchableVehicleMod(48,100)
                    {
                        DispatchableVehicleModValues = new List<DispatchableVehicleModValue>()
                        {
                            new DispatchableVehicleModValue(0,15),
                            new DispatchableVehicleModValue(1,15),
                            new DispatchableVehicleModValue(2,15),
                            new DispatchableVehicleModValue(3,15),
                            new DispatchableVehicleModValue(4,15),
                            new DispatchableVehicleModValue(5,15),
                            new DispatchableVehicleModValue(6,15),
                            new DispatchableVehicleModValue(7,15),
                            new DispatchableVehicleModValue(8,15),
                            new DispatchableVehicleModValue(9,15),
                            new DispatchableVehicleModValue(10,15),
                            new DispatchableVehicleModValue(11,15),
                            new DispatchableVehicleModValue(12,15),
                            new DispatchableVehicleModValue(13,15),
                            new DispatchableVehicleModValue(14,15),
                            new DispatchableVehicleModValue(15,15),
                            new DispatchableVehicleModValue(16,15),
                            new DispatchableVehicleModValue(17,15),
                            new DispatchableVehicleModValue(18,15),
                            new DispatchableVehicleModValue(19,15),
                            new DispatchableVehicleModValue(20,15),
                            new DispatchableVehicleModValue(21,15),
                            new DispatchableVehicleModValue(22,15),
                            new DispatchableVehicleModValue(23,15),
                            new DispatchableVehicleModValue(24,15),
                            new DispatchableVehicleModValue(25,15),
                            new DispatchableVehicleModValue(26,15),
                            new DispatchableVehicleModValue(27,15),
                            new DispatchableVehicleModValue(28,15),
                        },
                    },
               },
           },
       };
    }
    private void Karts()
    {
        GoKarts_Group = new List<DispatchableVehicle>()
        {

        };
        StreetKart_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("veto",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 95f },

        };
        RaceKart_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("veto2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 95f },
        };

        GoKarts_Group = new List<DispatchableVehicle>() { };
        StreetKart_Group.AddRange(StreetKart_Group);
        RaceKart_Group.AddRange(RaceKart_Group);
    }
    private void CompactCars()
    {
        RegularCompact_Group = new List<DispatchableVehicle>()
        {
        };
        Compact_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("asbo",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("blista",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("brioso",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("brioso2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("brioso3",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("club",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("issi2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("issi3",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("kanjo",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("panto",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("prairie",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("rhapsody",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("weevil",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
        };
        RegularCompact_Group.AddRange(Compact_Group);
    }

    //private void WaterSports()
    //{
    //    BoatsWater_Group = new List<DispatchableVehicle>()
    //    {
    //        new DispatchableVehicle("dinghy",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
    //        new DispatchableVehicle("dinghy2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
    //        new DispatchableVehicle("dinghy3",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
    //        new DispatchableVehicle("dinghy4",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
    //        new DispatchableVehicle("jetmax",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
    //        new DispatchableVehicle("marquis",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
    //        new DispatchableVehicle("predator",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
    //        new DispatchableVehicle("revo",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
    //        new DispatchableVehicle("speeder",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
    //        new DispatchableVehicle("squalo",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
    //        new DispatchableVehicle("suntrap",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
    //        new DispatchableVehicle("toro",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
    //        new DispatchableVehicle("toro2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
    //        new DispatchableVehicle("tropic",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
    //        new DispatchableVehicle("tropic2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
    //    };

    //    JetskiWater_Group = new List<DispatchableVehicle>()
    //    {
    //        new DispatchableVehicle("seashark",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
    //        new DispatchableVehicle("seashark2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
    //        new DispatchableVehicle("seashark3",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
    //    };

    //}
}

