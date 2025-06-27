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
    public List<DispatchableVehicle> HotringTruck_Group { get; private set; }
    public List<DispatchableVehicle> Hotring_Group { get; private set; }
    public List<DispatchableVehicle> LowRider_Group { get; private set; }
    public List<DispatchableVehicle> OtherSportsRegular_Group { get; private set; }
    public List<DispatchableVehicle> TunerSports_Group { get; private set; }
    public List<DispatchableVehicle> EuroSports_Group { get; private set; }
    public List<DispatchableVehicle> UsSports_Group { get; private set; }
    public List<DispatchableVehicle> ClassicSports_Group { get; private set; }
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

        //General Groups
        GroupsToAdd.Add(new DispatchableVehicleGroup("SportsCars_Racing", "Sports Cars", "Collection of sports cars", RegularSports_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("MuscleCars_Racing", "Muscle Cars", "Collection of muscle cars", RegularMuscle_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("SuperCars_Racing", "Super Cars", "Collection of super cars", RegularSuper_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("RallyCars_Racing", "Rally Cars", "Collection of rally cars", RegularRally_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("Motorcycles_Racing", "Motorcycles", "Collection of racing motorcycles", RegularMotorcycle_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("OffRoad_Racing", "Off-Road", "Collection of off-road vehicles", RegularOffRoad_Group, DispatchbleVehicleGroupType.Racing));

        //Hotring
        GroupsToAdd.Add(new DispatchableVehicleGroup("Hotring_Racing", "Hotring Racing", "Collection of Hotring's with racing liveries", Hotring_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("Everon_Racing", "Everon Racing", "Collection of Everon's with racing liveries", HotringTruck_Group, DispatchbleVehicleGroupType.Racing));

        //LowRiders
        GroupsToAdd.Add(new DispatchableVehicleGroup("LowRiders_Racing", "LowRider Racing", "Collection of LowRiders's with customizations", LowRider_Group, DispatchbleVehicleGroupType.Racing));

        //specialized muscle
        GroupsToAdd.Add(new DispatchableVehicleGroup("Buffalo_Racing", "Bravado Buffalos", "Collection of different generations of Bravado Buffalo", BuffaloRegular_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("Gauntlet_Racing", "Bravado Gauntlets", "Collection of different generations of Bravado Gauntlet", GauntletRegular_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("Vigero_Racing", "Declasse Vigero", "Collection of different generations of Declasse Ruiner", VigeroRegular_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("Dominator_Racing", "Vapid Dominators", "Collection of different generations of Vapid Dominator", DominatorRegular_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("OtherMuscle_Racing", "Other Muscle Cars", "Collection of all muscle cars", OtherMuscleRegular_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("OldMuscle_Racing", "Old Muscle Cars", "Collection of old muscle cars", OldMuscleRegular_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("MuscleRace_Racing", "Race Muscle Cars", "Collection of muscle cars with racing liveries", MuscleRace_Group, DispatchbleVehicleGroupType.Racing));

        //specialized motorcycles
        GroupsToAdd.Add(new DispatchableVehicleGroup("SportMotorcycles_Racing", "Sport Motorcycles", "Collection of sport motorcycles", SportsMotorcycleRegular_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("OtherMotorcycles_Racing", "Other Motorcycles", "Collection of all motorcycles", OtherMotorcycleRegular_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("ChopperMotorcycles_Racing", "Chopper Motorcycles", "Collection of chopper motorcycles", ChopperMotorcycleRegular_Group, DispatchbleVehicleGroupType.Racing));

        //specialized sports
        GroupsToAdd.Add(new DispatchableVehicleGroup("OtherSportsRegular_Group", "Regular Sports Cars", "Collection of all sports cars", RegularSports_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("TunerSports_Racing", "Asian Sports Cars", "Collection of modified sports cars", TunerSports_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("EuroSports_Racing", "Euro Sports Cars", "Collection of European sports cars", EuroSports_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("UsSports_Racing", "US Sports Cars", "Collection of US sports cars", UsSports_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("ClassicSports_Racing", "Classic Sports Cars", "Collection of classic sports cars", ClassicSports_Group, DispatchbleVehicleGroupType.Racing));

        //specialized super 


    }
    private void Motorcycle()
    {
        RegularMotorcycle_Group = new List<DispatchableVehicle>()
        {

        };
        ChopperMotorcycleRegular_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("avarus", 100, 100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("daemon", 100, 100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("daemon2", 100, 100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("hexer", 100, 100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("nightblade", 100, 100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("sanctus", 100, 100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("zombiea", 100, 100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("zombieb", 100, 100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
        };
        SportsMotorcycleRegular_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("akuma",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("bati",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("bati2",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("carbonrs",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("defiler",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("diablous",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("diablous2",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("double",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("fcr",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("fcr2",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("hakuchou",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("hakuchou2",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("lectro",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("shinobi",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
        };
        OtherMotorcycleRegular_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("chimera", 100, 100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("innovation", 100, 100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("reever", 100, 100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("rrocket", 100, 100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("stryder", 100, 100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("reever", 100, 100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("thrust", 100, 100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("vindicator", 100, 100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
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
            new DispatchableVehicle("brawler",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("calico",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("flashgt",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("gb200",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("issi8",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("omnis",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("uranus",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
        };
    }
    private void Offroad()
    {
        RegularOffRoad_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("trophytruck2",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("draugur",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("hellion",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("trophytruck",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("ratel",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("patriot3",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("outlaw",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("vagrant",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
        };
    }
    private void SportsCars()
    {

        RegularSports_Group = new List<DispatchableVehicle>()
        {

        };

        TunerSports_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("elegy",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("elegy2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("euros",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("futo",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("futo2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("hardy",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("jester",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("jester3",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("jester4",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("kanjosj",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("kuruma",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("penumbra",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("penumbra2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("previon",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("r300",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("remus",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("rt3000",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("sugoi",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("sultan2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("sultan3",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("vectre",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("z190",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("zr350",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
        };
        EuroSports_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("bestiagts",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("cinquemila",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("comet2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("comet5",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("comet6",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("comet7",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("cypher",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("drafter",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("komoda",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("feltzer2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("neon",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("omnisegt",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("rhinehart",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("schafter3",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("schlagen",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("schwarzer",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("sentinel5",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("tailgater",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("tailgater2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("tenf2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },

        };
        UsSports_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("alpha",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("banshee",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("banshee2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("buffalo2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("buffalo4",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("coquette",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("coquette4",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("coquette6",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("fusilade",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("raiden",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("revolter",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("vstr",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
        };
        OtherSportsRegular_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("banshee3",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            //new DispatchableVehicle("niobe",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },HSW Upgrade mod Issue
            new DispatchableVehicle("stingertt",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("panthere",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("tenf",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("tenf2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("corsita",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("growler",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("vectre",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("comet6",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("jester4",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("euros",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("vstr",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("komoda",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("jugular",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("neo",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("schlagen",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("pariah",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("raiden",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
        };

        RegularSports_Group = new List<DispatchableVehicle>() { };
        RegularSports_Group.AddRange(TunerSports_Group);
        RegularSports_Group.AddRange(EuroSports_Group);
        RegularSports_Group.AddRange(UsSports_Group);
        RegularSports_Group.AddRange(OtherSportsRegular_Group);


        ClassicSports_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("casco",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("cheetah2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("coquette2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("feltzer3",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("gt500",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("infernus2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("jb7002",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("monroe",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("stingergt",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("stromberg",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("swinger",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("torero",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("turismo2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("viseris",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
        };

    }
    private void SuperCars()
    {
        RegularSuper_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("pipistrello",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("virtue",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("turismo3",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("entity3",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("cyclone2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("zeno",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("ignus",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("champion",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("furia",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("emerus",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("tyrant",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("sc1",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("cyclone",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("italigtb2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("nero2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("italigtb",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("tempesta",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("penetrator",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("fmj",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("reaper",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("banshee2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("zentorno",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("vacca",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("cheetah",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
        };

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
            new DispatchableVehicle("blade", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("buccaneer",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("deviant", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("dominator8", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("dukes", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("faction",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("gauntlet3", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },//need optional color
            new DispatchableVehicle("gauntlet5", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },//need optional color
            new DispatchableVehicle("impaler",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("peyote2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("phoenix", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("ruiner", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("sabregt", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("stalion", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("tampa", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            //new DispatchableVehicle("tampa4", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f }, HSW Upgrade mod Issue
            new DispatchableVehicle("tornado6", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("tulip", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("tulip2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("vamos", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("vigero", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
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
            new DispatchableVehicle("buccaneer2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("chino2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("eudora", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("faction2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("glendale2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("manana2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("minivan2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("moonbeam2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("peyote3", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("primo2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("sabregt2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("slamvan3", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("tornado5", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("voodoo", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("voodoo2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("virgo2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
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
}

