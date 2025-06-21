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
    public List<DispatchableVehicle> MuscleRace_Group { get; private set; }
    public List<DispatchableVehicle> RegularRally_Group { get; private set; }
    public List<DispatchableVehicle> RegularMotorcycle_Group { get; private set; }
    public List<DispatchableVehicle> RegularOffRoad_Group { get; private set; }

    public List<DispatchableVehicle> HotringTruck_Group { get; private set; }
    public List<DispatchableVehicle> Hotring_Group { get; private set; }

    public DispatchableVehicles_RaceCars(DispatchableVehicles dispatchableVehicles)
    {
        DispatchableVehicles = dispatchableVehicles;
    }
    public void DefaultConfig()
    {
        MuscleCars();
        SuperCars();
        SportsCars();
        Offroad();
        Rally();
        Motorcycle();
        Hotring();

        //General Groups
        GroupsToAdd.Add(new DispatchableVehicleGroup("SportsCars_Racing","Sports Cars","Collection of sports cars", RegularSports_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("MuscleCars_Racing", "Muscle Cars", "Collection of muscle cars", RegularMuscle_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("SuperCars_Racing", "Super Cars", "Collection of super cars", RegularSuper_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("RallyCars_Racing", "Rally Cars", "Collection of rally cars", RegularRally_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("Motorcycles_Racing", "Motorcycles", "Collection of racing motorcycles", RegularMotorcycle_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("OffRoad_Racing", "Off-Road", "Collection of off-road vehicles", RegularOffRoad_Group, DispatchbleVehicleGroupType.Racing));

        //specialized
        GroupsToAdd.Add(new DispatchableVehicleGroup("Buffalo_Racing", "Bravado Buffalos", "Collection of different generations of Bravado Buffalo", BuffaloRegular_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("Gauntlet_Racing", "Bravado Gauntlets", "Collection of different generations of Bravado Gauntlet", GauntletRegular_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("Vigero_Racing", "Declasse Vigero", "Collection of different generations of Declasse Ruiner", VigeroRegular_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("Dominator_Racing", "Vapid Dominators", "Collection of different generations of Vapid Dominator", DominatorRegular_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("OtherMuscle_Racing", "Other Muscle Cars", "Collection of all muscle cars", OtherMuscleRegular_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("MuscleRace_Racing", "Race Muscle Cars", "Collection of muscle cars with racing liveries", MuscleRace_Group, DispatchbleVehicleGroupType.Racing));

        //Hotring
        GroupsToAdd.Add(new DispatchableVehicleGroup("Hotring_Racing", "Hotring Racing", "Collection of Hotring's with racing liveries", Hotring_Group, DispatchbleVehicleGroupType.Racing));
        GroupsToAdd.Add(new DispatchableVehicleGroup("Everon_Racing", "Everon Racing", "Collection of Everon's with racing liveries", HotringTruck_Group, DispatchbleVehicleGroupType.Racing));

    }
    private void Motorcycle()
    {
        RegularMotorcycle_Group = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("shinobi",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("hakuchou",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("hakuchou2",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("double",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("carbonrs",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("bati2",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("bati",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
        };
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
            new DispatchableVehicle("banshee3",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("niobe",100,100) { SetRandomCustomization = true, RandomCustomizationPercentage = 65f },
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
            new DispatchableVehicle("ruiner4", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },//90 firebird
            new DispatchableVehicle("deviant",100,100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },//older chrsyler
            new DispatchableVehicle("stalion", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },//Race car
        };

        RegularMuscle_Group = new List<DispatchableVehicle>() { };
        RegularMuscle_Group.AddRange(BuffaloRegular_Group);
        RegularMuscle_Group.AddRange(GauntletRegular_Group);
        RegularMuscle_Group.AddRange(VigeroRegular_Group);
        RegularMuscle_Group.AddRange(DominatorRegular_Group);
        RegularMuscle_Group.AddRange(OtherMuscleRegular_Group);

        MuscleRace_Group = new List<DispatchableVehicle>
        {
            new DispatchableVehicle("gauntlet2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },
            new DispatchableVehicle("dominator2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },//pisswasser old dominator
            new DispatchableVehicle("buffalo3", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },//Race car
            new DispatchableVehicle("stalion2", 100, 100){ SetRandomCustomization = true,RandomCustomizationPercentage = 65f },//Race car
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

