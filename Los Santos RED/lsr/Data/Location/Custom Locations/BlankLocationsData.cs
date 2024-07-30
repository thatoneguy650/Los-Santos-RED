using Rage;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BlankLocationsData
{
    private float defaultSpawnPercentage = 60f;
    public BlankLocationsData()
    {
    }

    public List<BlankLocation> BlankLocationPlaces { get; set; } = new List<BlankLocation>();
    public void DefaultConfig()
    {
        SpeedTraps();
        Checkpoints();
        RooftopSnipers();
        OtherCops();
        FamiliesGang();
        AncelottinGang();
        ArmenianGang();
        CartelGang();
        DiablosGang();
        GambettiGang();
        KhangpaeGang();
        LupisellaGang();
        MarabuntaGang();
        MessinaGang();
        PavanoGang();
        TriadGang();
        YardiesGang();
        RedneckGang();
        VarriosGang();
    }
    private void RooftopSnipers()
    {
        float sniperSpawnPercentage = 65f;
        List<BlankLocation> blankLocationPlaces = new List<BlankLocation>() {
            new BlankLocation(new Vector3(-1004.588f,-2451.93f,25.63272f), 294.0977f, "RoofTopSniper1", "Rooftop Sniper 1") {  //Near LSIA
                ActivateDistance = 300f,ActivateCells = 8,
                AssignedAssociationID = "LSIAPD",
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(-1004.588f,-2451.93f,25.63272f), 294.0977f, sniperSpawnPercentage) {
                        MaxWantedLevelSpawn = 4,
                        RequiredPedGroup = "Sniper",
                        TaskRequirements = TaskRequirements.Guard | TaskRequirements.EquipLongGunWhenIdle,LongGunAlwaysEquipped = true,},},
                },
            new BlankLocation(new Vector3(-544.9591f, -2225.018f, 122.3655f), 56.47693f, "RoofTopSniper2", "Rooftop Sniper 2") {  //Top of Bridge outside LSIA
                ActivateDistance = 300f,ActivateCells = 8,
                AssignedAssociationID = "LSIAPD",
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(-544.9591f, -2225.018f, 122.3655f), 56.47693f, sniperSpawnPercentage) {
                        MinWantedLevelSpawn = 2,
                        MaxWantedLevelSpawn = 4,

                        RequiredPedGroup = "Sniper",
                        TaskRequirements = TaskRequirements.Guard | TaskRequirements.EquipLongGunWhenIdle,LongGunAlwaysEquipped = true,},},
                },
            new BlankLocation(new Vector3(132.3552f, -1032.493f, 57.79759f), 336.7522f, "RoofTopSniper3", "Rooftop Sniper 3") {  //Pillbox hill rooftop
                ActivateDistance = 300f,ActivateCells = 8,
                AssignedAssociationID = "LSPD",
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(132.3552f, -1032.493f, 57.79759f), 336.7522f, sniperSpawnPercentage) {
                        MinWantedLevelSpawn = 2,
                        MaxWantedLevelSpawn = 4,
                        RequiredPedGroup = "Sniper",
                        TaskRequirements = TaskRequirements.Guard | TaskRequirements.EquipLongGunWhenIdle,LongGunAlwaysEquipped = true,},},
                },
            new BlankLocation(new Vector3(-62.03125f, -707.9713f, 55.52032f), 94.052f, "RoofTopSniper4", "Rooftop Sniper 4") {  //Pillbox hill rooftop
                ActivateDistance = 300f,ActivateCells = 8,
                AssignedAssociationID = "LSPD",
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(-62.03125f, -707.9713f, 55.52032f), 94.052f, sniperSpawnPercentage) {
                        MinWantedLevelSpawn = 2,
                        MaxWantedLevelSpawn = 4,
                        RequiredPedGroup = "Sniper",
                        TaskRequirements = TaskRequirements.Guard | TaskRequirements.EquipLongGunWhenIdle,LongGunAlwaysEquipped = true,},},
                },
            new BlankLocation(new Vector3(-547.1264f, -625.6401f, 56.11749f), 172.5318f, "RoofTopSniper5", "Rooftop Sniper 5") { //rockford hills rooftop
                ActivateDistance = 300f,ActivateCells = 8,
                //AssignedAssociationID = "RHPD",//what about NON FEJ "LSPD-RH"
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(-547.1264f, -625.6401f, 56.11749f), 172.5318f, sniperSpawnPercentage) {
                        MinWantedLevelSpawn = 2,
                        MaxWantedLevelSpawn = 4,
                        RequiredPedGroup = "Sniper",
                        TaskRequirements = TaskRequirements.Guard | TaskRequirements.EquipLongGunWhenIdle,LongGunAlwaysEquipped = true,},},
                },
            new BlankLocation(new Vector3(-771.976f, -302.2982f, 54.00434f), 109.9027f, "RoofTopSniper6", "Rooftop Sniper 6") {  //rockford hills rooftop
                ActivateDistance = 300f,ActivateCells = 8,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(-771.976f, -302.2982f, 54.00434f), 109.9027f, sniperSpawnPercentage) {
                        MinWantedLevelSpawn = 2,
                        MaxWantedLevelSpawn = 4,
                        RequiredPedGroup = "Sniper",
                        TaskRequirements = TaskRequirements.Guard | TaskRequirements.EquipLongGunWhenIdle,LongGunAlwaysEquipped = true,},},
                },
            new BlankLocation(new Vector3(-1332.035f, -500.6434f, 40.44254f), 260.7096f, "RoofTopSniper7", "Rooftop Sniper 7") {  //del perro rooftop
                ActivateDistance = 300f,ActivateCells = 8,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(-1332.035f, -500.6434f, 40.44254f), 260.7096f, sniperSpawnPercentage) {
                        MinWantedLevelSpawn = 2,
                        MaxWantedLevelSpawn = 4,
                        RequiredPedGroup = "Sniper",
                        TaskRequirements = TaskRequirements.Guard | TaskRequirements.EquipLongGunWhenIdle,LongGunAlwaysEquipped = true},},
                },
        };
        BlankLocationPlaces.AddRange(blankLocationPlaces);
    }
    private void Checkpoints()
    {
        List<BlankLocation> blankLocationPlaces = new List<BlankLocation>() {
            new BlankLocation(new Vector3(1561.604f, 2776.656f, 37.73339f), 25.09373f, "BPCheckpoint1", "BP Checkpoint 1")
            {
                ActivateDistance = 300f,
                ActivateCells = 8,
                AssignedAssociationID = "NOOSE",
                PossibleGroupSpawns = new List<ConditionalGroup>()
                    {
                        new ConditionalGroup(){
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = 0.0f,
                            OverridePoorWeatherPercentage = 0.0f,
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation(new Vector3(1550.248f, 2792.72f, 37.75742f), 200.7081f, 0f){
                                    AssociationID = "NOOSE-BP",  },
                                new LEConditionalLocation(new Vector3(1561.604f, 2776.656f, 37.73339f), 25.09373f, 0f){
                                    AssociationID = "NOOSE-BP",  },
                            },
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation(new Vector3(1552.776f, 2793.424f, 38.20471f), 283.7819f, 0f){
                                    AssociationID = "NOOSE-BP",
                                    TaskRequirements = TaskRequirements.Guard,
                                    ForcedScenarios = new List<string>(){ "WORLD_HUMAN_BINOCULARS", "WORLD_HUMAN_CLIPBOARD" } },
                                new LEConditionalLocation(new Vector3(1557.192f, 2776.484f, 38.0505f), 107.7628f, 0f){
                                    AssociationID = "NOOSE-BP",
                                    TaskRequirements = TaskRequirements.Guard,
                                    ForcedScenarios = new List<string>(){ "WORLD_HUMAN_BINOCULARS", "WORLD_HUMAN_CLIPBOARD" } },
                            }
                        },
                    }
            },
        };
        BlankLocationPlaces.AddRange(blankLocationPlaces);
    }
    private void SpeedTraps()
    {
        List<BlankLocation> blankLocationPlaces = new List<BlankLocation>() {
            new BlankLocation(new Vector3(-2453.876f, 3720.782f, 15.35273f), 348.2538f, "SpeedTrap1", "Speed Trap Great Ocean Highway 1") {
                ActivateDistance = 300f,ActivateCells = 8,
                PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup(){
                        Percentage = defaultSpawnPercentage,
                        OverrideNightPercentage = 0.0f,
                        OverridePoorWeatherPercentage = 0.0f,
                        PossibleVehicleSpawns  = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(-2453.876f, 3720.782f, 15.35273f), 348.2538f, 0f) {
                                AssociationID = "SAHP",
                                RequiredVehicleGroup = "Motorcycle", },
                        },
                        PossiblePedSpawns = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(-2456.923f, 3717.698f, 15.68384f), 170.4713f, 0f) {
                                AssociationID = "SAHP",
                                RequiredPedGroup = "MotorcycleCop",
                                TaskRequirements = TaskRequirements.Guard,
                                ForcedScenarios = new List<string>(){ "WORLD_HUMAN_BINOCULARS" } },
                        }
                    },
                }
            },
            new BlankLocation(new Vector3(-732.3002f, 5503.62f, 36.00393f), 101.8113f, "SpeedTrap2", "Speed Trap Great Ocean Highway 2") {
                ActivateDistance = 300f,
                ActivateCells = 8,
                PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup(){
                        Percentage = defaultSpawnPercentage,
                        OverrideNightPercentage = 0.0f,
                        OverridePoorWeatherPercentage = 0.0f,
                        PossibleVehicleSpawns = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(-727.5372f, 5504.277f, 35.65791f), 304.7107f, 0f) {
                                AssociationID = "SAHP",
                                RequiredVehicleGroup = "Motorcycle",  },
                        },
                        PossiblePedSpawns = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(-732.3002f, 5503.62f, 36.00393f), 101.8113f, 0f) {
                                AssociationID = "SAHP",
                                RequiredPedGroup = "MotorcycleCop",
                                TaskRequirements = TaskRequirements.Guard,
                                ForcedScenarios = new List<string>(){ "WORLD_HUMAN_BINOCULARS" } },
                        }
                    },
                }
            },
            new BlankLocation(new Vector3(-22.44862f, 6405.367f, 30.99126f), 221.6609f, "SpeedTrap3", "Speed Trap In Paleto Bay") {
                ActivateDistance = 300f,
                ActivateCells = 8,
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(-22.44862f, 6405.367f, 30.99126f), 221.6609f, defaultSpawnPercentage) {
                        IsEmpty = false,
                        TaskRequirements = TaskRequirements.Guard,
                        OverrideNightPercentage = 55.0f,
                        OverridePoorWeatherPercentage = 0.0f },
                },
            },
            new BlankLocation(new Vector3(2389.632f, 5837.942f, 46.33297f), 60.37991f, "SpeedTrap4", "Speed Trap Braddock Tunnel") {
                ActivateDistance = 300f,
                ActivateCells = 8,
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(2389.632f, 5837.942f, 46.33297f), 60.37991f, defaultSpawnPercentage) {
                        AssociationID = "SAHP",
                        //RequiredVehicleGroup = "StandardSAHP",
                        IsEmpty = false,
                        TaskRequirements = TaskRequirements.Guard,
                        OverrideNightPercentage = 55.0f,
                        OverridePoorWeatherPercentage = 0.0f },
                },
            },
            new BlankLocation(new Vector3(2806.941f, 4280.857f, 49.73266f), 199.6163f, "SpeedTrap5", "Speed Trap Great Ocean Highway 3") {
                ActivateDistance = 300f,
                ActivateCells = 8,
                PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup(){
                        Percentage = defaultSpawnPercentage,
                        OverrideNightPercentage = 0.0f,
                        OverridePoorWeatherPercentage = 0.0f,
                        PossibleVehicleSpawns = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(2806.941f, 4280.857f, 49.73266f), 199.6163f, 0f) {
                                AssociationID = "SAHP",
                                RequiredVehicleGroup = "Motorcycle", },
                        },
                        PossiblePedSpawns = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(2807.574f, 4284.726f, 50.22674f), 11.87567f, 0f) {
                                AssociationID = "SAHP",
                                RequiredPedGroup = "MotorcycleCop",
                                TaskRequirements = TaskRequirements.Guard,
                                ForcedScenarios = new List<string>(){ "WORLD_HUMAN_BINOCULARS" } },
                        }
                    },
                }
            },
            new BlankLocation(new Vector3(2716.348f, 3428.547f, 55.57796f), 248.8778f, "SpeedTrap6", "Speed Trap You Tool") {  ActivateDistance = 300f,ActivateCells = 8,
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(2716.348f, 3428.547f, 55.57796f), 248.8778f, defaultSpawnPercentage) {
                        AssociationID = "SAHP",
                        //RequiredVehicleGroup = "StandardSAHP",
                        IsEmpty = false,
                        TaskRequirements = TaskRequirements.Guard,
                        OverrideNightPercentage = 55.0f,
                        OverridePoorWeatherPercentage = 0.0f },
                },
            },
            new BlankLocation(new Vector3(2240.881f, 2742.619f, 44.46104f), 303.4088f, "SpeedTrap7", "Speed Trap Great Ocean Highway 4") {
                ActivateDistance = 300f,
                ActivateCells = 8,
                PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup(){
                        Percentage = defaultSpawnPercentage,
                        OverrideNightPercentage = 0.0f,
                        OverridePoorWeatherPercentage = 0.0f,
                        PossibleVehicleSpawns = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(2240.881f, 2742.619f, 44.46104f), 303.4088f, 0f) {
                                AssociationID = "SAHP",
                                RequiredVehicleGroup = "Motorcycle",  },
                        },
                        PossiblePedSpawns = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(2237.851f, 2742.785f, 45.01079f), 121.1667f, 0f) {
                                AssociationID = "SAHP",
                                RequiredPedGroup = "MotorcycleCop",
                                TaskRequirements = TaskRequirements.Guard,
                                ForcedScenarios = new List<string>(){ "WORLD_HUMAN_BINOCULARS" } },
                        }
                    },
                }
            },
            new BlankLocation(new Vector3(1896.709f, 1787.596f, 63.94535f), 199.7011f, "SpeedTrap8", "Speed Trap Great Ocean Highway 5") {
                ActivateDistance = 300f,
                ActivateCells = 8,
                PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup(){
                        Percentage = defaultSpawnPercentage,
                        OverrideNightPercentage = 0.0f,
                        OverridePoorWeatherPercentage = 0.0f,
                        PossibleVehicleSpawns = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(1896.709f, 1787.596f, 63.94535f), 199.7011f, 0f) {
                                AssociationID = "SAHP",
                                RequiredVehicleGroup = "Motorcycle", },
                        },
                        PossiblePedSpawns = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(1898.374f, 1789.177f, 64.64346f), 12.35284f, 0f) {
                                AssociationID = "SAHP",
                                RequiredPedGroup = "MotorcycleCop",
                                TaskRequirements = TaskRequirements.Guard,
                                ForcedScenarios = new List<string>(){ "WORLD_HUMAN_BINOCULARS" } },
                        }
                    },
                }
            },
            new BlankLocation(new Vector3(2515.903f, 607.1796f, 108.1216f), 189.9624f, "SpeedTrap9", "Speed Trap Great Ocean Highway 6") {
                ActivateDistance = 300f,
                ActivateCells = 8,
                PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup(){
                        Percentage = defaultSpawnPercentage,
                        OverrideNightPercentage = 0.0f,
                        OverridePoorWeatherPercentage = 0.0f,
                        PossibleVehicleSpawns = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(2515.903f, 607.1796f, 108.1216f), 189.9624f, 0f) {
                                AssociationID = "SAHP",
                                RequiredVehicleGroup = "Motorcycle", },
                        },
                        PossiblePedSpawns = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(2517.579f, 610.0076f, 108.436f), 2.938543f, 0f) {
                                AssociationID = "SAHP",
                                RequiredPedGroup = "MotorcycleCop",
                                TaskRequirements = TaskRequirements.Guard,
                                ForcedScenarios = new List<string>(){ "WORLD_HUMAN_BINOCULARS" } },
                        }
                    },
                }
            },
            new BlankLocation(new Vector3(1863.335f, -754.3698f, 81.10484f), 223.2417f, "SpeedTrap10", "Speed Trap Train Bridge") {
                ActivateDistance = 300f,
                ActivateCells = 8,
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(1863.335f, -754.3698f, 81.10484f), 223.2417f, defaultSpawnPercentage) {
                        AssociationID = "SAHP",
                        //RequiredVehicleGroup = "StandardSAHP",
                        IsEmpty = false,
                        OverrideNightPercentage = 55.0f,
                        OverridePoorWeatherPercentage = 0.0f,
                        TaskRequirements = TaskRequirements.Guard, },
                },
            },
            new BlankLocation(new Vector3(1290.652f, -2548.193f, 42.99783f), 8.77529f, "SpeedTrap11", "Speed Trap East LS") {
                ActivateDistance = 300f,
                ActivateCells = 8,
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(1290.652f, -2548.193f, 42.99783f), 8.77529f, defaultSpawnPercentage) {
                        AssociationID = "SAHP",
                        //RequiredVehicleGroup = "StandardSAHP",
                        IsEmpty = false,
                        OverrideNightPercentage = 55.0f,
                        OverridePoorWeatherPercentage = 0.0f,
                        TaskRequirements = TaskRequirements.Guard,  },
                },
            },
        };
        BlankLocationPlaces.AddRange(blankLocationPlaces);
    }
    private void SecuritySpawns()
    {
        BlankLocation HornyBurgersCops = new BlankLocation()
        {
            Name = "HornyBurgersCops",
            FullName = "HornyBurgersCops",
            Description = "SpeedTrap Cops at the Horny Burgers shop",
            MapIcon = 162,
            EntrancePosition = new Vector3(1245.835f, -334.4011f, 68.78214f),
            EntranceHeading = 171.9635f,
            OpenTime = 6,
            CloseTime = 20,
            StateID = StaticStrings.SanAndreasStateID,
            AssignedAssociationID = "",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            { },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation()
                    {
                        Location = new Vector3(1245.835f, -334.4011f, 68.78214f),
                            Heading = 171.9635f,
                            Percentage = defaultSpawnPercentage,
                            //AssociationID = "LSPD-ELS",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            IsEmpty = false,

                            AllowAirVehicle = false,
                            AllowBoat = false,
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<string>()
                            {},
                            OverrideNightPercentage = 55f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = 0f,
                            MinHourSpawn = 0,
                            MaxHourSpawn = 24,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 3,
                            LongGunAlwaysEquipped = false,


                            ForceLongGun = false,
                    },
                },
        };
        BlankLocationPlaces.Add(HornyBurgersCops);
    }
    private void OtherCops()
    {
        BlankLocation HornyBurgersCops = new BlankLocation()
        {
            Name = "HornyBurgersCops",
            FullName = "HornyBurgersCops",
            Description = "SpeedTrap Cops at the Horny Burgers shop",
            MapIcon = 162,
            EntrancePosition = new Vector3(1245.835f, -334.4011f, 68.78214f),
            EntranceHeading = 171.9635f,
            OpenTime = 6,
            CloseTime = 20,
            StateID = StaticStrings.SanAndreasStateID,
            AssignedAssociationID = "",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            { },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation()
                    {
                        Location = new Vector3(1245.835f, -334.4011f, 68.78214f),
                            Heading = 171.9635f,
                            Percentage = defaultSpawnPercentage,
                            //AssociationID = "LSPD-ELS",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            IsEmpty = false,
                            
                            AllowAirVehicle = false,
                            AllowBoat = false,
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<string>()
                            {},
                            OverrideNightPercentage = 55f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = 0f,
                            MinHourSpawn = 0,
                            MaxHourSpawn = 24,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 3,
                            LongGunAlwaysEquipped = false,


                            ForceLongGun = false,
                    },
                },
        };
        BlankLocationPlaces.Add(HornyBurgersCops);
        BlankLocation RustyBrownsCops = new BlankLocation()
        {
            Name = "RustyBrownsCops",
            FullName = "RustyBrownsCops",
            Description = "2 Cops at the Rusty Brown's donut shop",
            MapIcon = 162,
            EntrancePosition = new Vector3(355.1435f, -1036.628f, 28.81483f),
            EntranceHeading = 88.71296f,
            OpenTime = 8,
            CloseTime = 16,
            StateID = StaticStrings.SanAndreasStateID,
            AssignedAssociationID = "",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = 0f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = 0f,
                            MinHourSpawn = 0,
                            MaxHourSpawn = 24,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 3,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                    {
                                        Location = new Vector3(355.8614f, -1034.715f, 29.33121f),
                                            Heading = 26.82541f,
                                            Percentage = 0f,
                                            //AssociationID = "LSPD-ASD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_AA_COFFEE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new LEConditionalLocation()
                                    {
                                        Location = new Vector3(355.0406f, -1034.046f, 29.3311f),
                                            Heading = -123.7173f,
                                            Percentage = 0f,
                                            // AssociationID = "LSPD-ASD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_AA_COFFEE",
                                                "WORLD_HUMAN_CLIPBOARD",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                    Location = new Vector3(355.1435f, -1036.628f, 28.81483f),
                                        Heading = 88.71296f,
                                        Percentage = 0f,
                                        AssociationID = "",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
                                        IsEmpty = true,
                                        
                                        AllowAirVehicle = false,
                                        AllowBoat = false,
                                        ForcedScenarios = new List<string>()
                                        {},
                                        OverrideNightPercentage = -1f,
                                        OverrideDayPercentage = -1f,
                                        OverridePoorWeatherPercentage = -1f,
                                        MinHourSpawn = 0,
                                        MaxHourSpawn = 24,
                                        MinWantedLevelSpawn = 0,
                                        MaxWantedLevelSpawn = 3,
                                        LongGunAlwaysEquipped = false,
            
            
                                        ForceLongGun = false,
                                },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(RustyBrownsCops);
        BlankLocation BishopsChickenCop = new BlankLocation()
        {
            Name = "BishopsChickenCop",
            FullName = "BishopsChickenCop",
            Description = "A Cops at the Bishop's Chicken Shop",
            MapIcon = 162,
            EntrancePosition = new Vector3(162.7488f, -1635.171f, 29.09935f),
            EntranceHeading = -165.3666f,
            OpenTime = 16,
            CloseTime = 20,
            StateID = StaticStrings.SanAndreasStateID,
            AssignedAssociationID = "",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = 0f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = 0f,
                            MinHourSpawn = 0,
                            MaxHourSpawn = 24,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 3,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                    Location = new Vector3(165.5919f, -1633.865f, 29.54167f),
                                        Heading = 9.506291f,
                                        Percentage = 0f,
                                        //AssociationID = "LSSD-DV",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
                                        IsEmpty = true,
                                        
                                        AllowAirVehicle = false,
                                        AllowBoat = false,
                                        TaskRequirements = TaskRequirements.Guard,
                                        ForcedScenarios = new List<string>()
                                        {
                                            "WORLD_HUMAN_AA_COFFEE",
                                            "WORLD_HUMAN_CLIPBOARD",
                                        },
                                        OverrideNightPercentage = -1f,
                                        OverrideDayPercentage = -1f,
                                        OverridePoorWeatherPercentage = -1f,
                                        MinHourSpawn = 0,
                                        MaxHourSpawn = 24,
                                        MinWantedLevelSpawn = 0,
                                        MaxWantedLevelSpawn = 3,
                                        LongGunAlwaysEquipped = false,
            
            
                                        ForceLongGun = false,
                                },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                    Location = new Vector3(162.7488f, -1635.171f, 29.09935f),
                                        Heading = -165.3666f,
                                        Percentage = 0f,
                                        AssociationID = "",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
                                        IsEmpty = true,
                                        
                                        AllowAirVehicle = false,
                                        AllowBoat = false,
                                        ForcedScenarios = new List<string>()
                                        {},
                                        OverrideNightPercentage = -1f,
                                        OverrideDayPercentage = -1f,
                                        OverridePoorWeatherPercentage = -1f,
                                        MinHourSpawn = 0,
                                        MaxHourSpawn = 24,
                                        MinWantedLevelSpawn = 0,
                                        MaxWantedLevelSpawn = 3,
                                        LongGunAlwaysEquipped = false,
            
            
                                        ForceLongGun = false,
                                },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(BishopsChickenCop);
    }
    private void AncelottinGang()
    {
        BlankLocation AncelottiGasStation = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 12,
                            MaxHourSpawn = 20,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-2576.103f, 2334.192f, 33.06499f),
                                            Heading = -111.3623f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_AA_COFFEE",
                                                "WORLD_HUMAN_SMOKING"
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-2575.808f, 2333.149f, 33.06003f),
                                            Heading = 0.1772228f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_TOURIST_MAP",
                                                "WORLD_HUMAN_AA_COFFEE",
                                                "WORLD_HUMAN_SMOKING"
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-2573.125f, 2334.611f, 32.76001f),
                                        Heading = 150.4924f,
                                },
                            },
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_ANCELOTTI",
            Name = "AncelottiGasStation",
            Description = "Ancelotti's having a break at the Gas Station",
            EntrancePosition = new Vector3(-2576.103f, 2334.192f, 33.06499f),
            EntranceHeading = -111.3623f,
        };
        BlankLocationPlaces.Add(AncelottiGasStation);
        BlankLocation AncelottiMomsPie = new BlankLocation()
        {
            
            Name = "AncelottiMomsPie",
            Description = "2 Ancelotti's outside Moms Pie Diner",
            MapIcon = 76,
            EntrancePosition = new Vector3(-3047.745f, 610.1703f, 6.908777f),
            EntranceHeading = 21.76928f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            AssignedAssociationID = "AMBIENT_GANG_ANCELOTTI",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-3045.746f, 609.939f, 7.377979f),
                                            Heading = 12.87239f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_AA_COFFEE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-3046.378f, 611.1282f, 7.359444f),
                                            Heading = -113.3274f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-3047.745f, 610.1703f, 6.908777f),
                                        Heading = 21.76928f,
                                        Percentage = 0f,
                                        AssociationID = "",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
                                        IsEmpty = true,
                                        
                                        AllowAirVehicle = false,
                                        AllowBoat = false,
                                        ForcedScenarios = new List<string>() {},
                                        OverrideNightPercentage = -1f,
                                        OverrideDayPercentage = -1f,
                                        OverridePoorWeatherPercentage = -1f,
                                        MinHourSpawn = 0,
                                        MaxHourSpawn = 24,
                                        MinWantedLevelSpawn = 0,
                                        MaxWantedLevelSpawn = 6,
                                        LongGunAlwaysEquipped = false,
            
            
                                        ForceLongGun = false,
                                },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(AncelottiMomsPie);
        BlankLocation AncelottiOdea = new BlankLocation()
        {
            Name = "AncelottiOdea",
            Description = "2 Ancelotti's outside Odea's",
            MapIcon = 76,
            EntrancePosition = new Vector3(-3151.205f, 1095.979f, 20.16063f),
            EntranceHeading = 102.04f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            AssignedAssociationID = "AMBIENT_GANG_ANCELOTTI",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-3155.077f, 1098.002f, 20.85437f),
                                            Heading = 107.5466f,
                                            Percentage = 0f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_AA_COFFEE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-3156.236f, 1098.123f, 20.85246f),
                                            Heading = -109.456f,
                                            Percentage = 0f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-3151.205f, 1095.979f, 20.16063f),
                                        Heading = 102.04f,
                                        Percentage = 0f,
                                        ForcedScenarios = new List<string>()
                                        {},
                                },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(AncelottiOdea);
        BlankLocation AncelottiRobsLiquor = new BlankLocation()
        {
            Name = "AncelottiRobsLiquor",
            Description = "3 Ancelotti's at the side Robs Liquor",
            MapIcon = 76,
            EntrancePosition = new Vector3(-2958.849f, 371.1895f, 14.05147f),
            EntranceHeading = -145.9505f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            AssignedAssociationID = "AMBIENT_GANG_ANCELOTTI",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-2959.074f, 376.1139f, 14.98954f),
                                            Heading = 74.39281f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-2960.297f, 376.3011f, 15.00359f),
                                            Heading = -122.6558f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-2957.748f, 372.1963f, 14.77398f),
                                            Heading = 22.06928f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_GUARD_STAND_CASINO",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-2958.849f, 371.1895f, 14.05147f),
                                        Heading = -145.9505f,
                                        Percentage = 0f,
                                        AssociationID = "",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
                                        IsEmpty = true,
                                        
                                        AllowAirVehicle = false,
                                        AllowBoat = false,
                                        ForcedScenarios = new List<string>()
                                        {},
                                        OverrideNightPercentage = -1f,
                                        OverrideDayPercentage = -1f,
                                        OverridePoorWeatherPercentage = -1f,
                                        MinHourSpawn = 0,
                                        MaxHourSpawn = 24,
                                        MinWantedLevelSpawn = 0,
                                        MaxWantedLevelSpawn = 6,
                                        LongGunAlwaysEquipped = false,
            
            
                                        ForceLongGun = false,
                                },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(AncelottiRobsLiquor);
        BlankLocation AncelottiWalkway = new BlankLocation()
        {
            
            Name = "AncelottiWalkway",
            Description = "2 Ancelotti's the beach walkway",
            MapIcon = 76,
            EntrancePosition = new Vector3(-3252.466f, 981.5923f, 12.60575f),
            EntranceHeading = -144.1428f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_ANCELOTTI",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-3252.466f, 981.5923f, 12.60575f),
                                            Heading = -144.1428f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_AA_SMOKE",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-3251.473f, 981.5574f, 12.60574f),
                                            Heading = 159.0457f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_AA_COFFEE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {},
                    },
                },
        };
        BlankLocationPlaces.Add(AncelottiWalkway);
    }
    private void ArmenianGang()
    {
        BlankLocation ArmenianGasStation = new BlankLocation()
        {
            
            Name = "ArmenianGasStation",
            Description = "Armenian's at the Globe Oil Gas Station",
            MapIcon = 162,
            EntrancePosition = new Vector3(-339.7834f, -1463.811f, 30.29497f),
            EntranceHeading = -111.3623f,
            OpenTime = 6,
            CloseTime = 20,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_ARMENIAN",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 12,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 3,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-341.0255f, -1460.561f, 30.75321f),
                                            Heading = -175.1071f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_AA_COFFEE",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-342.3172f, -1460.754f, 30.75426f),
                                            Heading = -126.9581f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_AA_COFFEE",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-342.5989f, -1462.05f, 30.61145f),
                                            Heading = -89.48071f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_AA_COFFEE",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-340.3219f, -1462.547f, 30.60857f),
                                            Heading = 29.69213f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_AA_COFFEE",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-339.7834f, -1463.811f, 30.29497f),
                                            Heading = -82.86436f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-337.9729f, -1459.476f, 30.27439f),
                                            Heading = 99.35658f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(ArmenianGasStation);
        BlankLocation ArmenianArsenalSt = new BlankLocation()
        {
            
            Name = "ArmenianArsenalSt",
            FullName = "",
            Description = "Armenian's at South Arsenal Street Container Depot",
            MapIconScale = 1f,
            EntrancePosition = new Vector3(-656.3889f, -1715.312f, 24.06793f),
            EntranceHeading = 78.28706f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = "SanAndreas",
            
            AssignedAssociationID = "AMBIENT_GANG_ARMENIAN",
            MenuID = "",

            PossibleGroupSpawns =
              new List<ConditionalGroup>() {
        new ConditionalGroup() {
          Name = "",
          Percentage = defaultSpawnPercentage,
          MinHourSpawn = 18,
          MaxHourSpawn = 4,
          PossiblePedSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(-656.1321f, -1713.218f, 24.77905f),
                  Heading = -6.13898f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_HANG_OUT_STREET",
                      },
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-656.1573f, -1712.042f, 24.78583f),
                  Heading = -164.4525f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_SMOKING",
                      },
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-655.3751f, -1705.817f, 24.80922f),
                  Heading = 129.2851f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_HANG_OUT_STREET",
                      },
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-656.647f, -1705.551f, 24.83137f),
                  Heading = -168.5f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                      },
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-655.1232f, -1707.04f, 24.79778f),
                  Heading = 49.42152f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_STAND_MOBILE",
                        "WORLD_HUMAN_HANG_OUT_STREET",
                      },
                },
              },
          PossibleVehicleSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(-640.0317f, -1722.396f, 24.07564f),
                  Heading = 5.763398f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  IsEmpty = false,
                  ForcedScenarios = new List<String>() {},
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-656.3889f, -1715.312f, 24.06793f),
                  Heading = 78.28706f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  ForcedScenarios = new List<String>() {},
                },
              },
        },
              },
            PossiblePedSpawns = new List<ConditionalLocation>() { },
            PossibleVehicleSpawns = new List<ConditionalLocation>() { },
            VehiclePreviewLocation = new SpawnPlace() { },
            VehicleDeliveryLocations = new List<SpawnPlace>() { },
        };
        BlankLocationPlaces.Add(ArmenianArsenalSt);
        BlankLocation ArmenianArsenalSt2 = new BlankLocation()
        {
            
            Name = "ArmenianArsenalSt2",
            FullName = "",
            Description = "2 Armenian's at South Arsenal Street Small Warehouse",
            MapIconScale = 1f,
            EntrancePosition = new Vector3(-559.4757f, -1798.599f, 22.30198f),
            EntranceHeading = 155.9304f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = "SanAndreas",
            
            AssignedAssociationID = "AMBIENT_GANG_ARMENIAN",
            MenuID = "",

            PossibleGroupSpawns =
              new List<ConditionalGroup>() {
        new ConditionalGroup() {
          Name = "",
          Percentage = defaultSpawnPercentage,
          MinHourSpawn = 8,
          MaxHourSpawn = 20,
          PossiblePedSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(-559.7857f, -1802.833f, 22.62625f),
                  Heading = 36.72789f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_HANG_OUT_STREET",
                        "WORLD_HUMAN_SMOKING",
                      },
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-560.9921f, -1801.961f, 22.6387f),
                  Heading = -126.6469f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_SMOKING",
                        "WORLD_HUMAN_HANG_OUT_STREET",
                      },
                },
              },
          PossibleVehicleSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(-559.4757f, -1798.599f, 22.30198f),
                  Heading = 155.9304f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  ForcedScenarios = new List<String>() {},
                },
              },
        },
              },
            PossiblePedSpawns = new List<ConditionalLocation>() { },
            PossibleVehicleSpawns = new List<ConditionalLocation>() { },
            VehiclePreviewLocation = new SpawnPlace() { },
            VehicleDeliveryLocations = new List<SpawnPlace>() { },
        };
        BlankLocationPlaces.Add(ArmenianArsenalSt2);
        BlankLocation ArmenianRecycling = new BlankLocation()
        {
            
            Name = "ArmenianRecycling",
            FullName = "",
            Description = "Armenian's at the Ls Recycling Depot",
            MapIconScale = 1f,
            EntrancePosition = new Vector3(-333.7245f, -1530.58f, 27.25197f),
            EntranceHeading = 173.2087f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = "SanAndreas",
            
            AssignedAssociationID = "AMBIENT_GANG_ARMENIAN",
            MenuID = "",

            PossibleGroupSpawns =
              new List<ConditionalGroup>() {
        new ConditionalGroup() {
          Name = "",
          Percentage = defaultSpawnPercentage,
          MinHourSpawn = 8,
          MaxHourSpawn = 20,
          PossiblePedSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(-347.6989f, -1548.758f, 27.72134f),
                  Heading = 173.2165f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_HANG_OUT_STREET",
                      },
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-357.338f, -1514.539f, 27.71653f),
                  Heading = 166.3024f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_SMOKING",
                      },
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-348.6397f, -1548.776f, 27.72133f),
                  Heading = -156.5172f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_HANG_OUT_STREET",
                      },
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-321.0858f, -1545.992f, 31.02085f),
                  Heading = -90.79362f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_SMOKING",
                      },
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-358.2265f, -1514.672f, 27.71616f),
                  Heading = -156.0986f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_SMOKING",
                      },
                },
              },
          PossibleVehicleSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(-368.7641f, -1525.959f, 27.46676f),
                  Heading = 177.6489f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  IsEmpty = false,
                  ForcedScenarios = new List<String>() {},
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-333.7245f, -1530.58f, 27.25197f),
                  Heading = 173.2087f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  ForcedScenarios = new List<String>() {},
                },
              },
        },
              },
            PossiblePedSpawns = new List<ConditionalLocation>() { },
            PossibleVehicleSpawns = new List<ConditionalLocation>() { },
            VehiclePreviewLocation = new SpawnPlace() { },
            VehicleDeliveryLocations = new List<SpawnPlace>() { },
        };
        BlankLocationPlaces.Add(ArmenianRecycling);
        BlankLocation ArmenianScrapYard = new BlankLocation()
        {
            
            Name = "ArmenianScrapYard",
            FullName = "",
            Description = "Armenian's at the Ls Scrapyard",
            MapIconScale = 1f,
            EntrancePosition = new Vector3(-435.2249f, -1704.951f, 18.67037f),
            EntranceHeading = -123.6972f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = "SanAndreas",
            
            AssignedAssociationID = "AMBIENT_GANG_ARMENIAN",
            MenuID = "",

            PossibleGroupSpawns =
              new List<ConditionalGroup>() {
        new ConditionalGroup() {
          Name = "",
          Percentage = defaultSpawnPercentage,
          MinHourSpawn = 18,
          MaxHourSpawn = 4,
          PossiblePedSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(-428.7203f, -1725.486f, 19.78611f),
                  Heading = 20.00664f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_SMOKING",
                        "WORLD_HUMAN_HANG_OUT_STREET",
                      },
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-416.3674f, -1720.184f, 19.47032f),
                  Heading = -107.6902f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                      },
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-468.1467f, -1721.084f, 18.65658f),
                  Heading = -83.77507f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_HANG_OUT_STREET",
                        "WORLD_HUMAN_SMOKING",
                      },
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-466.5977f, -1720.721f, 18.63776f),
                  Heading = 90.70687f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_HANG_OUT_STREET",
                        "WORLD_HUMAN_SMOKING",
                      },
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-467.5121f, -1719.971f, 18.66377f),
                  Heading = -151.282f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_HANG_OUT_STREET",
                        "WORLD_HUMAN_SMOKING",
                      },
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-467.3311f, -1721.822f, 18.65141f),
                  Heading = -1.655458f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_HANG_OUT_STREET",
                        "WORLD_HUMAN_SMOKING",
                      },
                },
              },
          PossibleVehicleSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(-435.2249f, -1704.951f, 18.67037f),
                  Heading = -123.6972f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  IsEmpty = false,
                  ForcedScenarios = new List<String>() {},
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-432.2829f, -1723.119f, 18.6497f),
                  Heading = 133.6777f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  ForcedScenarios = new List<String>() {},
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-466.9138f, -1717.272f, 18.38978f),
                  Heading = 104.61f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  ForcedScenarios = new List<String>() {},
                },
              },
        },
              },
            PossiblePedSpawns = new List<ConditionalLocation>() { },
            PossibleVehicleSpawns = new List<ConditionalLocation>() { },
            VehiclePreviewLocation = new SpawnPlace() { },
            VehicleDeliveryLocations = new List<SpawnPlace>() { },
        };
        BlankLocationPlaces.Add(ArmenianScrapYard);
    }
    private void CartelGang()
    {
        BlankLocation CARTELOldBarn = new BlankLocation()
        {
            
            Name = "CARTELOldBarn",
            Description = "Cartel Group at a Old Barn",
            MapIcon = 78,
            EntrancePosition = new Vector3(1522.238f, 1720.615f, 109.4364f),
            EntranceHeading = -179.7648f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_MADRAZO",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 3,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1531.928f, 1729.447f, 109.9141f),
                                            Heading = 102.1779f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_IMPATIENT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1530.796f, 1726.67f, 109.9862f),
                                            Heading = 134.1091f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_POT",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_IMPATIENT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1530.417f, 1725.227f, 110.1038f),
                                            Heading = -10.73689f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_DRINKING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1529.722f, 1725.993f, 110.0219f),
                                            Heading = -49.00603f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_IMPATIENT",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1527.046f, 1712.292f, 109.9839f),
                                            Heading = -9.08828f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_IMPATIENT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1528.48f, 1722.249f, 109.4625f),
                                            Heading = -163.8078f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1525.137f, 1731.465f, 109.5932f),
                                            Heading = -106.9038f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1522.238f, 1720.615f, 109.4364f),
                                            Heading = 8.214884f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = false,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(CARTELOldBarn);
        BlankLocation CARTELOldHouse = new BlankLocation()
        {
            
            Name = "CARTELOldHouse",
            Description = "2 Cartel members at a Old House",
            MapIcon = 78,
            EntrancePosition = new Vector3(1402.705f, 2163.271f, 97.63835f),
            EntranceHeading = 125.4879f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_MADRAZO",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 3,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1402.995f, 2170.238f, 97.7655f),
                                            Heading = 57.22536f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1400.78f, 2171.063f, 97.89244f),
                                            Heading = -89.37797f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1402.705f, 2163.271f, 97.63835f),
                                            Heading = 125.4879f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1412.348f, 2168.658f, 96.83696f),
                                            Heading = 5.329985f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = false,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(CARTELOldHouse);
        BlankLocation CartelCementWorks = new BlankLocation()
        {
            
            Name = "CartelCementWorks",
            Description = "2 Cartel members at the Cement Works, located on Senora Road near the cow field",
            MapIcon = 78,
            EntrancePosition = new Vector3(1198.32f, 1839.745f, 78.7323f),
            EntranceHeading = 118.9358f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_MADRAZO",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 6,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 3,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1198.689f, 1836.506f, 78.85123f),
                                            Heading = 26.25036f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_AA_COFFEE",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1198.32f, 1839.745f, 78.7323f),
                                            Heading = 118.9358f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_CLIPBOARD",
                                                "WORLD_HUMAN_AA_COFFEE",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(1198.727f, 1834.982f, 78.53899f),
                                        Heading = -78.52052f,
                                        Percentage = 0f,
                                        AssociationID = "",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
                                        IsEmpty = true,
                                        
                                        AllowAirVehicle = false,
                                        AllowBoat = false,
                                        ForcedScenarios = new List<string>()
                                        {},
                                        OverrideNightPercentage = -1f,
                                        OverrideDayPercentage = -1f,
                                        OverridePoorWeatherPercentage = -1f,
                                        MinHourSpawn = 0,
                                        MaxHourSpawn = 24,
                                        MinWantedLevelSpawn = 0,
                                        MaxWantedLevelSpawn = 3,
                                        LongGunAlwaysEquipped = false,
            
            
                                        ForceLongGun = false,
                                },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(CartelCementWorks);
        BlankLocation CARTELMarlowe2 = new BlankLocation()
        {
            
            Name = "CARTELMarlowe2",
            FullName = "",
            Description = "2 Cartel members on a 2nd location along Marlowe Drive",
            MapIcon = 78,
            MapIconScale = 1f,
            EntrancePosition = new Vector3(-1158.082f, 932.1671f, 197.6742f),
            EntranceHeading = 142.4552f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = "SanAndreas",
            
            AssignedAssociationID = "AMBIENT_GANG_MADRAZO",
            MenuID = "",

            PossibleGroupSpawns =
new List<ConditionalGroup>() {
        new ConditionalGroup() {
          Name = "",
          Percentage = defaultSpawnPercentage,
          MinHourSpawn = 18,
          MaxHourSpawn = 4,
          PossiblePedSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(-1157.173f, 925.0801f, 197.9952f),
                  Heading = -118.8341f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_HANG_OUT_STREET",
                        "WORLD_HUMAN_SMOKING",
                      },
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-1156.176f, 925.6647f, 198.0741f),
                  Heading = -171.9546f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_HANG_OUT_STREET",
                        "WORLD_HUMAN_SMOKING",
                      },
                },
              },
          PossibleVehicleSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(-1158.082f, 932.1671f, 197.6742f),
                  Heading = 142.4552f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  ForcedScenarios = new List<String>() {},
                },
              },
        },
},
            PossiblePedSpawns = new List<ConditionalLocation>() { },
            PossibleVehicleSpawns = new List<ConditionalLocation>() { },
            VehiclePreviewLocation = new SpawnPlace() { },
            VehicleDeliveryLocations = new List<SpawnPlace>() { },
        };
        BlankLocationPlaces.Add(CARTELMarlowe2);
        BlankLocation CARTELMarlowe = new BlankLocation()
        {
            
            Name = "CARTELMarlowe",
            FullName = "",
            Description = "2 Cartel members on Marlowe Drive",
            MapIcon = 78,
            MapIconScale = 1f,
            EntrancePosition = new Vector3(-625.1735f, 976.2534f, 240.4749f),
            EntranceHeading = 108.9041f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = "SanAndreas",
            
            AssignedAssociationID = "AMBIENT_GANG_MADRAZO",
            MenuID = "",

            PossibleGroupSpawns =
              new List<ConditionalGroup>() {
        new ConditionalGroup() {
          Name = "",
          Percentage = defaultSpawnPercentage,
          PossiblePedSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(-629.1226f, 969.8096f, 242.4207f),
                  Heading = -168.7026f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_HANG_OUT_STREET",
                        "WORLD_HUMAN_SMOKING",
                      },
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-630.0687f, 969.2487f, 242.3873f),
                  Heading = -147.6762f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_HANG_OUT_STREET",
                        "WORLD_HUMAN_SMOKING",
                      },
                },
              },
          PossibleVehicleSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(-625.1735f, 976.2534f, 240.4749f),
                  Heading = 108.9041f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  ForcedScenarios = new List<String>() {},
                },
              },
        },
              },
            PossiblePedSpawns = new List<ConditionalLocation>() { },
            PossibleVehicleSpawns = new List<ConditionalLocation>() { },
            VehiclePreviewLocation = new SpawnPlace() { },
            VehicleDeliveryLocations = new List<SpawnPlace>() { },
        };
        BlankLocationPlaces.Add(CARTELMarlowe);
    }
    private void DiablosGang()
    {
        BlankLocation DiablosCarPark = new BlankLocation()
        {
            
            Name = "DiablosCarPark",
            Description = "10 Diablo Members 4 Vehicles Car park Gathering, Located on Chum Street just off Signal Street bridge",
            MapIcon = 355,
            EntrancePosition = new Vector3(276.3214f, -2510.965f, 6.371347f),
            EntranceHeading = 88.9995f,
            OpenTime = 18,
            CloseTime = 4,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_DIABLOS",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 3,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(276.3214f, -2510.965f, 6.371347f),
                                            Heading = 88.9995f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_PARTYING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(275.7917f, -2512.047f, 6.355332f),
                                            Heading = 22.03003f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_PARTYING",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_DRUG_DEALER",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(275.0528f, -2510.693f, 6.38229f),
                                            Heading = -135.0179f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRINKING",
                                                "WORLD_HUMAN_PARTYING",
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(270.8466f, -2494.71f, 6.441306f),
                                            Heading = -155.2137f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(270.9318f, -2495.784f, 6.441038f),
                                            Heading = -0.06394381f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_PARTYING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(287.2843f, -2506.751f, 6.373237f),
                                            Heading = -35.7513f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_PARTYING",
                                                "WORLD_HUMAN_DRUG_DEALER",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(288.4875f, -2505.98f, 6.390196f),
                                            Heading = 88.44595f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(287.412f, -2505.062f, 6.390697f),
                                            Heading = -164.4998f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                                "WORLD_HUMAN_PARTYING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(286.6503f, -2505.741f, 6.376956f),
                                            Heading = -84.04057f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRINKING",
                                                "WORLD_HUMAN_SMOKING_POT",
                                                "WORLD_HUMAN_PARTYING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(279.3178f, -2508.228f, 6.3907f),
                                            Heading = -152.5385f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                                "WORLD_HUMAN_PARTYING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(272.556f, -2511.219f, 5.679735f),
                                            Heading = -154.4095f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(277.5428f, -2509.067f, 5.678845f),
                                            Heading = -165.3026f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(281.6294f, -2507.202f, 5.673219f),
                                            Heading = -171.1318f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(275.1067f, -2497.054f, 5.728101f),
                                            Heading = 95.83862f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(DiablosCarPark);
        BlankLocation DiablosUnderBridge = new BlankLocation()
        {
            
            Name = "DiablosUnderBridge",
            Description = "4 Diablo members 1 vehicle,located Under the Elysian Fields Freeway near Voodoo Place",
            MapIcon = 355,
            EntrancePosition = new Vector3(226.5622f, -2649.648f, 6.013142f),
            EntranceHeading = -119.7073f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_DIABLOS",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 0,
                            MaxHourSpawn = 24,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 3,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        TerritorySpawnsForceMainGang = false,
                                            Location = new Vector3(226.5622f, -2649.648f, 6.013142f),
                                            Heading = -119.7073f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                                "WORLD_HUMAN_DRINKING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        TerritorySpawnsForceMainGang = false,
                                            Location = new Vector3(227.3563f, -2649.262f, 6.011755f),
                                            Heading = 173.2949f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_DRINKING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        TerritorySpawnsForceMainGang = false,
                                            Location = new Vector3(227.2546f, -2658.765f, 5.996807f),
                                            Heading = 8.154132f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_SMOKING_POT",
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        TerritorySpawnsForceMainGang = false,
                                            Location = new Vector3(227.1466f, -2650.605f, 6.008636f),
                                            Heading = 6.473634f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_PARTYING",
                                                "WORLD_HUMAN_SMOKING_POT",
                                                "WORLD_HUMAN_DRUG_DEALER",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    TerritorySpawnsForceMainGang = false,
                                        Location = new Vector3(227.7079f, -2660.669f, 5.294679f),
                                        Heading = 124.0635f,
                                        Percentage = 0f,
                                        AssociationID = "",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
                                        IsEmpty = true,
                                        
                                        AllowAirVehicle = false,
                                        AllowBoat = false,
                                        ForcedScenarios = new List<string>()
                                        {},
                                        OverrideNightPercentage = -1f,
                                        OverrideDayPercentage = -1f,
                                        OverridePoorWeatherPercentage = -1f,
                                        MinHourSpawn = 0,
                                        MaxHourSpawn = 24,
                                        MinWantedLevelSpawn = 0,
                                        MaxWantedLevelSpawn = 3,
                                        LongGunAlwaysEquipped = false,
            
            
                                        ForceLongGun = false,
                                },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(DiablosUnderBridge);
        BlankLocation DiablosAutopia = new BlankLocation()
        {
            
            Name = "DiablosAutopia",
            FullName = "",
            Description = "3 Diablo members 1 vehicle,located Autopia Parkway",
            MapIcon = 355,
            EntrancePosition = new Vector3(-338.4319f, -2187.327f, 9.565862f),
            EntranceHeading = 95.96698f,
            OpenTime = 0,
            CloseTime = 24,

            
            AssignedAssociationID = "AMBIENT_GANG_DIABLOS",
            MenuID = "",

            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                new ConditionalGroup()
                {
                    Name = "",
                    Percentage = defaultSpawnPercentage,
                    PossiblePedSpawns = new List<ConditionalLocation>()
                    {
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(-338.0668f, -2190.645f, 9.617875f),
                            Heading = -10.54824f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>()
                            {
                                "WORLD_HUMAN_HANG_OUT_STREET",
                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                "WORLD_HUMAN_DRINKING",
                            },
                        },
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(-337.0664f, -2189.442f, 9.700167f),
                            Heading = 152.0498f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>()
                            {
                                "WORLD_HUMAN_DRUG_DEALER",
                                "WORLD_HUMAN_HANG_OUT_STREET",
                                "WORLD_HUMAN_DRINKING",
                            },
                        },
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(-338.441f, -2189.338f, 9.714742f),
                            Heading = -141.2649f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>()
                            {
                                "WORLD_HUMAN_STAND_MOBILE",
                                "WORLD_HUMAN_SMOKING_POT",
                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                            },
                        },
                    },
                    PossibleVehicleSpawns = new List<ConditionalLocation>()
                    {
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(-338.4319f, -2187.327f, 9.565862f),
                            Heading = 95.96698f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            ForcedScenarios = new List<String>() {  },
                        },
                    },
                },
            },
            PossiblePedSpawns = new List<ConditionalLocation>() { },
            PossibleVehicleSpawns = new List<ConditionalLocation>() { },
            VehiclePreviewLocation = new SpawnPlace() { },
        };
        BlankLocationPlaces.Add(DiablosAutopia);
    }
    private void GambettiGang()
    {
        BlankLocation GambettiEclipse = new BlankLocation()
        {
            
            Name = "GambettiEclipse",
            Description = "2 Gambetti outside the Eclipse Lounge",
            EntrancePosition = new Vector3(-80.93258f, 239.8747f, 100.8947f),
            EntranceHeading = 59.99377f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_GAMBETTI",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-80.93258f, 239.8747f, 100.8947f),
                                            Heading = 59.99377f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_GUARD_STAND_CASINO",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-82.01189f, 240.0104f, 100.7592f),
                                            Heading = -69.79543f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {},
                    },
                },
        };
        BlankLocationPlaces.Add(GambettiEclipse);
        BlankLocation GambettiGuidos = new BlankLocation()
        {
            
            Name = "GambettiGuidos",
            Description = "2 Gambetti outside Guidos Pizza Store",
            EntrancePosition = new Vector3(443.7656f, 121.4294f, 98.95059f),
            EntranceHeading = 69.60268f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_GAMBETTI",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(445.0337f, 131.4324f, 99.83509f),
                                            Heading = 139.7243f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_AA_SMOKE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(443.825f, 131.9116f, 99.90434f),
                                            Heading = -162.0078f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_AA_SMOKE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(443.7656f, 121.4294f, 98.95059f),
                                        Heading = 69.60268f,
                                        Percentage = 0f,
                                        AssociationID = "",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
                                        IsEmpty = true,
                                        
                                        AllowAirVehicle = false,
                                        AllowBoat = false,
                                        ForcedScenarios = new List<string>()
                                        {},
                                        OverrideNightPercentage = -1f,
                                        OverrideDayPercentage = -1f,
                                        OverridePoorWeatherPercentage = -1f,
                                        MinHourSpawn = 0,
                                        MaxHourSpawn = 24,
                                        MinWantedLevelSpawn = 0,
                                        MaxWantedLevelSpawn = 6,
                                        LongGunAlwaysEquipped = false,
            
            
                                        ForceLongGun = false,
                                },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(GambettiGuidos);
        BlankLocation GambettiLastTrain = new BlankLocation()
        {
            
            Name = "GambettiLastTrain",
            Description = "2 Gambetti outside Last Train Diner",
            EntrancePosition = new Vector3(-362.5037f, 265.7051f, 84.09846f),
            EntranceHeading = 2.661887f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_GAMBETTI",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-365.7397f, 265.9775f, 84.8446f),
                                            Heading = -155.5171f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING",
                                                "WORLD_HUMAN_STAND_IMPATIENT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-365.8124f, 265.1453f, 84.8446f),
                                            Heading = -39.20266f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-362.5037f, 265.7051f, 84.09846f),
                                        Heading = 2.661887f,
                                        Percentage = 0f,
                                        AssociationID = "",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
                                        IsEmpty = true,
                                        
                                        AllowAirVehicle = false,
                                        AllowBoat = false,
                                        ForcedScenarios = new List<string>()
                                        {},
                                        OverrideNightPercentage = -1f,
                                        OverrideDayPercentage = -1f,
                                        OverridePoorWeatherPercentage = -1f,
                                        MinHourSpawn = 0,
                                        MaxHourSpawn = 24,
                                        MinWantedLevelSpawn = 0,
                                        MaxWantedLevelSpawn = 6,
                                        LongGunAlwaysEquipped = false,
            
            
                                        ForceLongGun = false,
                                },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(GambettiLastTrain);
        BlankLocation GambettiNorthArcher = new BlankLocation()
        {
            
            Name = "GambettiNorthArcher",
            Description = "2 Gambetti outside an apartment off North Archer Road",
            EntrancePosition = new Vector3(-148.7277f, 210.5457f, 92.31926f),
            EntranceHeading = 2.555956f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_GAMBETTI",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-154.4334f, 208.9687f, 98.33158f),
                                            Heading = -45.96962f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING",
                                                "WORLD_HUMAN_STAND_IMPATIENT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-154.1916f, 209.8495f, 98.33158f),
                                            Heading = -146.4765f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 18,
                                            MaxHourSpawn = 4,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-148.7277f, 210.5457f, 92.31926f),
                                        Heading = 2.555956f,
                                        Percentage = 0f,
                                        AssociationID = "",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
                                        IsEmpty = true,
                                        
                                        AllowAirVehicle = false,
                                        AllowBoat = false,
                                        ForcedScenarios = new List<string>()
                                        {},
                                        OverrideNightPercentage = -1f,
                                        OverrideDayPercentage = -1f,
                                        OverridePoorWeatherPercentage = -1f,
                                        MinHourSpawn = 0,
                                        MaxHourSpawn = 24,
                                        MinWantedLevelSpawn = 0,
                                        MaxWantedLevelSpawn = 6,
                                        LongGunAlwaysEquipped = false,
            
            
                                        ForceLongGun = false,
                                },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(GambettiNorthArcher);
        BlankLocation GambettiPower = new BlankLocation()
        {
            
            Name = "GambettiPower",
            Description = "3 Gambetti in a alley off Power Street",
            EntrancePosition = new Vector3(443.7656f, 121.4294f, 98.95059f),
            EntranceHeading = 69.60268f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_GAMBETTI",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        TerritorySpawnsForceMainGang = false,
                                            Location = new Vector3(403.1715f, 58.50986f, 97.97793f),
                                            Heading = 71.05602f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_GUARD_STAND_CASINO",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        TerritorySpawnsForceMainGang = false,
                                            Location = new Vector3(401.2886f, 58.41864f, 97.97788f),
                                            Heading = -65.97093f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        TerritorySpawnsForceMainGang = false,
                                            Location = new Vector3(401.3389f, 59.40113f, 97.97728f),
                                            Heading = -135.7666f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_AA_SMOKE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        TerritorySpawnsForceMainGang = false,
                                            Location = new Vector3(390.4741f, 57.62064f, 97.26919f),
                                            Heading = 66.49467f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = false,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        TerritorySpawnsForceMainGang = false,
                                            Location = new Vector3(400.6472f, 60.93935f, 97.26937f),
                                            Heading = -69.48891f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(GambettiPower);
    }
    private void KhangpaeGang()
    {
        BlankLocation KKANGPAECarPark = new BlankLocation()
        {
            
            Name = "KKANGPAECarPark",
            Description = "KKANGPAE group on a car park",
            EntrancePosition = new Vector3(-809.8347f, -578.8478f, 29.82631f),
            EntranceHeading = 71.65619f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 3,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-816.9778f, -579.8116f, 30.27627f),
                                            Heading = -47.23438f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-815.8612f, -579.4198f, 30.27627f),
                                            Heading = 88.00085f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_AA_COFFEE",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-821.6752f, -571.7261f, 30.27627f),
                                            Heading = -53.9781f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-821.0618f, -570.7906f, 30.27627f),
                                            Heading = 171.2854f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_AA_COFFEE",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-820.5128f, -571.5859f, 30.27627f),
                                            Heading = 90.01759f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_AA_COFFEE",
                                                "WORLD_HUMAN_STAND_IMPATIENT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-811.2783f, -576.5577f, 30.12628f),
                                            Heading = -77.80688f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_AA_COFFEE",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-810.0571f, -577.1219f, 30.12626f),
                                            Heading = 30.12626f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_AA_COFFEE",
                                                "WORLD_HUMAN_TOURIST_MOBILE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-809.8108f, -575.6605f, 30.12626f),
                                            Heading = 163.444f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_AA_COFFEE",
                                                "WORLD_HUMAN_TOURIST_MOBILE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-809.8347f, -578.8478f, 29.82631f),
                                            Heading = 71.65619f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-811.8729f, -574.8732f, 29.82591f),
                                            Heading = 119.8215f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-813.915f, -571.4519f, 29.82613f),
                                            Heading = 110.2028f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(KKANGPAECarPark);
        BlankLocation KKANGPAELuckyPlucker = new BlankLocation()
        {
            
            Name = "KKANGPAELuckyPlucker",
            Description = "KKANGPAE members at Lucky Plucker",
            EntrancePosition = new Vector3(-606.5583f, -872.8243f, 25.1454f),
            EntranceHeading = 78.5521f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 3,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        TerritorySpawnsForceMainGang = false,
                                            Location = new Vector3(-598.1215f, -880.1085f, 25.5592f),
                                            Heading = 90.99896f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        TerritorySpawnsForceMainGang = false,
                                            Location = new Vector3(-608.9788f, -875.8167f, 25.26858f),
                                            Heading = -39.9175f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_AA_COFFEE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        TerritorySpawnsForceMainGang = false,
                                            Location = new Vector3(-608.3752f, -874.5851f, 25.28517f),
                                            Heading = 169.3897f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        TerritorySpawnsForceMainGang = false,
                                            Location = new Vector3(-607.7589f, -875.4822f, 25.28885f),
                                            Heading = 110.2557f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        TerritorySpawnsForceMainGang = false,
                                            Location = new Vector3(-606.5583f, -872.8243f, 25.1454f),
                                            Heading = 78.5521f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        TerritorySpawnsForceMainGang = false,
                                            Location = new Vector3(-610.6986f, -877.7935f, 25.02717f),
                                            Heading = 55.70324f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        TerritorySpawnsForceMainGang = false,
                                            Location = new Vector3(-596.3806f, -878.9075f, 25.43968f),
                                            Heading = 170.324f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(KKANGPAELuckyPlucker);
        BlankLocation KKANGPAENoodleShop = new BlankLocation()
        {
            
            Name = "KKANGPAENoodleShop",
            Description = "KKANGPAE at the S.Ho Noodle Shop",
            EntrancePosition = new Vector3(-704.4111f, -886.5811f, 23.80389f),
            EntranceHeading = 11.49827f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 3,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        TerritorySpawnsForceMainGang = false,
                                            Location = new Vector3(-704.4111f, -886.5811f, 23.80389f),
                                            Heading = 11.49827f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        TerritorySpawnsForceMainGang = false,
                                            Location = new Vector3(-703.976f, -885.8718f, 23.80213f),
                                            Heading = 136.1898f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_AA_COFFEE",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        TerritorySpawnsForceMainGang = false,
                                            Location = new Vector3(-705.2665f, -886.1356f, 23.80288f),
                                            Heading = -103.3947f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_AA_COFFEE",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        TerritorySpawnsForceMainGang = false,
                                            Location = new Vector3(-705.9953f, -878.0908f, 23.57499f),
                                            Heading = 27.52635f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_AA_COFFEE",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        TerritorySpawnsForceMainGang = false,
                                            Location = new Vector3(-708.7655f, -880.3814f, 23.30313f),
                                            Heading = -178.5557f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        TerritorySpawnsForceMainGang = false,
                                            Location = new Vector3(-703.8621f, -879.8784f, 23.3033f),
                                            Heading = -130.2227f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(KKANGPAENoodleShop);
        BlankLocation KKANGPAEPlaza = new BlankLocation()
        {
            
            Name = "KKANGPAEPlaza",
            FullName = "",
            Description = "3 KKANGPAE members outside betta Life in Korean Plaza",
            MapIcon = 47,
            MapIconScale = 1f,
            EntrancePosition = new Vector3(-595.3588f, -1133.895f, 21.87822f),
            EntranceHeading = 91.83308f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = "SanAndreas",
            
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            MenuID = "",

            PossibleGroupSpawns =
      new List<ConditionalGroup>() {
        new ConditionalGroup() {
          Name = "",
          Percentage = defaultSpawnPercentage,
          MinHourSpawn = 8,
          MaxHourSpawn = 18,
          PossiblePedSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(-602.244f, -1133.862f, 22.32653f),
                  Heading = -86.62172f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                        "WORLD_HUMAN_HANG_OUT_STREET",
                      },
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-600.095f, -1134.601f, 22.32925f),
                  Heading = 72.00099f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_STAND_MOBILE",
                        "WORLD_HUMAN_HANG_OUT_STREET",
                      },
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-599.962f, -1133.365f, 22.32926f),
                  Heading = 116.0017f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_SMOKING",
                        "WORLD_HUMAN_HANG_OUT_STREET",
                      },
                },
              },
          PossibleVehicleSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(-595.3588f, -1133.895f, 21.87822f),
                  Heading = 91.83308f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  ForcedScenarios = new List<String>() {},
                },
              },
        },
      },
            PossiblePedSpawns = new List<ConditionalLocation>() { },
            PossibleVehicleSpawns = new List<ConditionalLocation>() { },
            VehiclePreviewLocation = new SpawnPlace() { },
            VehicleDeliveryLocations = new List<SpawnPlace>() { },
        };
        BlankLocationPlaces.Add(KKANGPAEPlaza);
        BlankLocation KKANGPAENoodleHouse = new BlankLocation()
        {
            
            Name = "KKANGPAENoodleHouse",
            FullName = "",
            Description = "3 KKANGPAE members at S.Ho Korean Noodle House",
            MapIcon = 47,
            MapIconScale = 1f,
            EntrancePosition = new Vector3(-648.0073f, -1211.422f, 10.96987f),
            EntranceHeading = 118.1369f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = "SanAndreas",
            
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            MenuID = "",

            PossibleGroupSpawns =
              new List<ConditionalGroup>() {
        new ConditionalGroup() {
          Name = "",
          Percentage = defaultSpawnPercentage,
          MinHourSpawn = 18,
          MaxHourSpawn = 4,
          PossiblePedSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(-656.5244f, -1212.779f, 10.94525f),
                  Heading = 95.46139f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_SMOKING",
                        "WORLD_HUMAN_HANG_OUT_STREET",
                      },
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-657.2998f, -1213.471f, 10.92f),
                  Heading = 9.810444f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_STAND_MOBILE",
                        "WORLD_HUMAN_HANG_OUT_STREET",
                      },
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-642.3582f, -1251.081f, 11.37462f),
                  Heading = 153.0649f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_STAND_MOBILE",
                        "WORLD_HUMAN_STAND_IMPATIENT",
                      },
                },
              },
          PossibleVehicleSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(-648.0073f, -1211.422f, 10.96987f),
                  Heading = 118.1369f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  ForcedScenarios = new List<String>() {},
                },
              },
        },
              },
            PossiblePedSpawns = new List<ConditionalLocation>() { },
            PossibleVehicleSpawns = new List<ConditionalLocation>() { },
            VehiclePreviewLocation = new SpawnPlace() { },
            VehicleDeliveryLocations = new List<SpawnPlace>() { },
        };
        BlankLocationPlaces.Add(KKANGPAENoodleHouse);
        BlankLocation KKANGPAEVespMall = new BlankLocation()
        {
            
            Name = "KKANGPAEVespMall",
            FullName = "",
            Description = "KKANGPAE members at the Vespucci Mall",
            MapIcon = 47,
            MapIconScale = 1f,
            EntrancePosition = new Vector3(-819.4919f, -1091.693f, 10.46899f),
            EntranceHeading = 110.5755f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = "SanAndreas",
            
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            MenuID = "",

            PossibleGroupSpawns =
              new List<ConditionalGroup>() {
        new ConditionalGroup() {
          Name = "",
          Percentage = defaultSpawnPercentage,
          MinHourSpawn = 18,
          MaxHourSpawn = 4,
          PossiblePedSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(-824.3732f, -1091.685f, 11.14515f),
                  Heading = 108.346f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_SMOKING",
                        "WORLD_HUMAN_HANG_OUT_STREET",
                      },
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-825.5837f, -1092.322f, 11.14712f),
                  Heading = -74.99149f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_SMOKING",
                        "WORLD_HUMAN_HANG_OUT_STREET",
                      },
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-826.0804f, -1093.807f, 11.15045f),
                  Heading = -58.99226f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_HANG_OUT_STREET",
                        "WORLD_HUMAN_STAND_MOBILE",
                      },
                },
              },
          PossibleVehicleSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(-819.4919f, -1091.693f, 10.46899f),
                  Heading = 110.5755f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  ForcedScenarios = new List<String>() {},
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-821.3574f, -1088.547f, 10.50175f),
                  Heading = 118.2286f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  ForcedScenarios = new List<String>() {},
                },
              },
        },
              },
            PossiblePedSpawns = new List<ConditionalLocation>() { },
            PossibleVehicleSpawns = new List<ConditionalLocation>() { },
            VehiclePreviewLocation = new SpawnPlace() { },
            VehicleDeliveryLocations = new List<SpawnPlace>() { },
        };
        BlankLocationPlaces.Add(KKANGPAEVespMall);
        BlankLocation KKANGPAEWigWam = new BlankLocation()
        {
            
            Name = "KKANGPAEWigWam",
            FullName = "",
            Description = "2 KKANGPAE members outside WigWam",
            MapIcon = 47,
            MapIconScale = 1f,
            EntrancePosition = new Vector3(-853.7135f, -1143.815f, 5.577652f),
            EntranceHeading = 115.6326f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = "SanAndreas",
            
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            MenuID = "",

            PossibleGroupSpawns =
              new List<ConditionalGroup>() {
        new ConditionalGroup() {
          Name = "",
          Percentage = defaultSpawnPercentage,
          MinHourSpawn = 8,
          MaxHourSpawn = 18,
          PossiblePedSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(-857.3314f, -1146.041f, 6.293494f),
                  Heading = -158.9977f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_SMOKING",
                        "WORLD_HUMAN_HANG_OUT_STREET",
                      },
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-858.36f, -1146.604f, 6.235434f),
                  Heading = -129.5918f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_STAND_MOBILE",
                        "WORLD_HUMAN_HANG_OUT_STREET",
                      },
                },
              },
          PossibleVehicleSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(-853.7135f, -1143.815f, 5.577652f),
                  Heading = 115.6326f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  ForcedScenarios = new List<String>() {},
                },
              },
        },
              },
            PossiblePedSpawns = new List<ConditionalLocation>() { },
            PossibleVehicleSpawns = new List<ConditionalLocation>() { },
            VehiclePreviewLocation = new SpawnPlace() { },
            VehicleDeliveryLocations = new List<SpawnPlace>() { },
        };
        BlankLocationPlaces.Add(KKANGPAEWigWam);

        BlankLocation KKANGPAESteps = new BlankLocation(new Vector3(-759.5464f, -919.3508f, 20.60988f), 0f, "KKangpaeSteps", "KKangpae Steps")
        {
            Name = "KKangpaeSteps",
            Description = "KKangpae Steps",
            EntrancePosition = new Vector3(-759.5464f, -919.3508f, 20.60988f),
            OpenTime = 0,
            CloseTime = 24,
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                //Group1
                new GangConditionalLocation()
                {
                    MinHourSpawn = 20,
                    MaxHourSpawn = 5,
                    Percentage = 45f,
                    Location = new Vector3(-757.2037f,-920.0898f,18.05869f),
                    Heading = -84.28564f,
                    TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario,
                },
                new GangConditionalLocation()
                {
                    MinHourSpawn = 20,
                    MaxHourSpawn = 5,
                    Percentage = 45f,
                    Location = new Vector3(-755.9536f,-919.7582f,18.08333f),
                    Heading = 107.8247f,
                    TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario,
                },
                new GangConditionalLocation()
                {
                    MinHourSpawn = 20,
                    MaxHourSpawn = 5,
                    Percentage = 45f,
                    Location = new Vector3(-761.7447f,-919.8821f,18.54978f),
                    Heading = -89.37133f,
                    TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario,
                },
                new GangConditionalLocation()
                {
                    MinHourSpawn = 20,
                    MaxHourSpawn = 5,
                    Percentage = 45f,
                    Location = new Vector3(-762.7174f,-919.2241f,19.20684f),
                    Heading = -89.37133f,
                    TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario,
                },
                new GangConditionalLocation()
                {
                    MinHourSpawn = 20,
                    MaxHourSpawn = 5,
                    Percentage = 45f,
                    Location = new Vector3(-764.3067f,-920.2347f,19.20683f),
                    Heading = -5.322605f,
                    TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario,
                },
                new GangConditionalLocation()
                {
                    MinHourSpawn = 20,
                    MaxHourSpawn = 5,
                    Percentage = 45f,
                    Location = new Vector3(-764.5144f,-919.2105f,19.20683f),
                    Heading = -84.28564f,
                    TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario,
                },
                new GangConditionalLocation()
                {
                    MinHourSpawn = 20,
                    MaxHourSpawn = 5,
                    Percentage = 45f,
                    Location = new Vector3(-764.2292f,-931.306f,17.16749f),
                    Heading = -116.4253f,
                    TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario,
                },
                new GangConditionalLocation()
                {
                    MinHourSpawn = 20,
                    MaxHourSpawn = 5,
                    Percentage = 45f,
                    Location = new Vector3(-764.2276f,-932.6341f,17.08447f),
                    Heading = -65.558f,
                    TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario,
                },


                //Group2
                new GangConditionalLocation()
                {
                    MinHourSpawn = 10,
                    MaxHourSpawn = 20,
                    Percentage = 45f,
                    Location = new Vector3(-765.4142f,-917.1601f,20.0831f),
                    Heading = -89.37133f,
                    TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario,
                },
                new GangConditionalLocation()
                {
                    MinHourSpawn = 10,
                    MaxHourSpawn = 20,
                    Percentage = 45f,
                    Location = new Vector3(-763.5844f,-918.8759f,19.20683f),
                    Heading = -105.295f,
                    TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario,
                },
                new GangConditionalLocation()
                {
                    MinHourSpawn = 10,
                    MaxHourSpawn = 20,
                    Percentage = 45f,
                    Location = new Vector3(-763.5086f,-919.8815f,19.20683f),
                    Heading = -86.19594f,
                    TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario,
                },
                new GangConditionalLocation()
                {
                    MinHourSpawn = 10,
                    MaxHourSpawn = 20,
                    Percentage = 45f,
                    Location = new Vector3(-762.7452f,-922.7858f,17.76118f),
                    Heading = 116.6984f,
                    TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario,
                },
                new GangConditionalLocation()
                {
                    MinHourSpawn = 10,
                    MaxHourSpawn = 20,
                    Percentage = 45f,
                    Location = new Vector3(-764.3322f,-923.4336f,17.72986f),
                    Heading = -70.54628f,
                    TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario,
                },
                new GangConditionalLocation()
                {
                    MinHourSpawn = 10,
                    MaxHourSpawn = 20,
                    Percentage = 45f,
                    Location = new Vector3(-763.4446f,-922.2467f,18.85892f),
                    Heading = -178.1328f,
                    TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario,
                },
                new GangConditionalLocation()
                {
                    MinHourSpawn = 10,
                    MaxHourSpawn = 20,
                    Percentage = 45f,
                    Location = new Vector3(-760.6191f,-915.1982f,18.27511f),
                    Heading = -6.779637f,
                    TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario,
                },
                new GangConditionalLocation()
                {
                    MinHourSpawn = 10,
                    MaxHourSpawn = 20,
                    Percentage = 45f,
                    Location = new Vector3(-761.1669f,-914.61f,18.33595f),
                    Heading = -77.92151f,
                    TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario,
                },

            },

            //PossibleGroupSpawns = new List<ConditionalGroup>() 
            //{    
            //    new ConditionalGroup() {
            //        Name = "",
            //        Percentage = 100f,
            //        MinHourSpawn = 20,
            //        MaxHourSpawn = 5,
            //        PossiblePedSpawns = new List<ConditionalLocation>() 
            //        {
            //            new GangConditionalLocation() 
            //            {
            //            Location = new Vector3(-757.2037f,-920.0898f,18.05869f),
            //            Heading = -84.28564f,
            //            TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario,
            //            },
            //            new GangConditionalLocation() 
            //            {
            //                Location = new Vector3(-755.9536f,-919.7582f,18.08333f),
            //                Heading = 107.8247f,
            //                TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario,
            //            },
            //            new GangConditionalLocation() 
            //            {
            //                Location = new Vector3(-761.7447f,-919.8821f,18.54978f),
            //                Heading = -89.37133f,
            //                TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario,
            //            },
            //            new GangConditionalLocation() 
            //            {
            //                Location = new Vector3(-762.7174f,-919.2241f,19.20684f),
            //                Heading = -89.37133f,
            //                TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario,
            //            },
            //            new GangConditionalLocation() 
            //            {
            //                Location = new Vector3(-764.3067f,-920.2347f,19.20683f),
            //                Heading = -5.322605f,
            //                TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario,
            //            },
            //            new GangConditionalLocation() 
            //            {
            //                Location = new Vector3(-764.5144f,-919.2105f,19.20683f),
            //                Heading = -84.28564f,
            //                TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario,
            //            },
            //            new GangConditionalLocation() 
            //            {
            //                Location = new Vector3(-764.2292f,-931.306f,17.16749f),
            //                Heading = -116.4253f,
            //                TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario,
            //            },
            //            new GangConditionalLocation() 
            //            {
            //                Location = new Vector3(-764.2276f,-932.6341f,17.08447f),
            //                Heading = -65.558f,
            //                TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario,
            //            },
            //        },
            //    },
            //    new ConditionalGroup() 
            //    {
            //        Name = "",
            //        Percentage = 100f,
            //        MinHourSpawn = 10,
            //        MaxHourSpawn = 20,
            //        PossiblePedSpawns = new List<ConditionalLocation>() 
            //        {
            //            new GangConditionalLocation() 
            //            {
            //                Location = new Vector3(-765.4142f,-917.1601f,20.0831f),
            //                Heading = -89.37133f,
            //                TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario,
            //            },
            //            new GangConditionalLocation() 
            //            {
            //                Location = new Vector3(-763.5844f,-918.8759f,19.20683f),
            //                Heading = -105.295f,
            //                TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario,
            //            },
            //            new GangConditionalLocation() 
            //            {
            //                Location = new Vector3(-763.5086f,-919.8815f,19.20683f),
            //                Heading = -86.19594f,
            //                TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario,
            //            },
            //            new GangConditionalLocation() 
            //            {
            //                Location = new Vector3(-762.7452f,-922.7858f,17.76118f),
            //                Heading = 116.6984f,
            //                TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario,
            //            },
            //            new GangConditionalLocation() 
            //            {
            //                Location = new Vector3(-764.3322f,-923.4336f,17.72986f),
            //                Heading = -70.54628f,
            //                TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario,
            //            },
            //            new GangConditionalLocation() 
            //            {
            //                Location = new Vector3(-763.4446f,-922.2467f,18.85892f),
            //                Heading = -178.1328f,
            //                TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario,
            //            },
            //            new GangConditionalLocation() 
            //            {
            //                Location = new Vector3(-760.6191f,-915.1982f,18.27511f),
            //                Heading = -6.779637f,
            //                TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario,
            //            },
            //            new GangConditionalLocation() 
            //            {
            //                Location = new Vector3(-761.1669f,-914.61f,18.33595f),
            //                Heading = -77.92151f,
            //                TaskRequirements = TaskRequirements.Guard | TaskRequirements.LocalScenario,
            //            },
            //        },
            //    },
            //},
        };
        //BlankLocationPlaces.Add(KKANGPAESteps);



    }
    private void LupisellaGang()
    {
        BlankLocation LupisellaBayviewLodge = new BlankLocation()
        {
            
            Name = "LupisellaBayviewLodge",
            Description = "2 Lupisella's outside Bayview Lodge",
            EntrancePosition = new Vector3(-706.9418f, 5784.884f, 17.03095f),
            EntranceHeading = -25.75707f,
            OpenTime = 6,
            CloseTime = 20,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_LUPISELLA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-702.2889f, 5791.816f, 17.52386f),
                                            Heading = 148.0714f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-702.2361f, 5790.67f, 17.52402f),
                                            Heading = 46.21632f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_AA_COFFEE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-706.9418f, 5784.884f, 17.03095f),
                                        Heading = -25.75707f,
                                        Percentage = 0f,
                                        AssociationID = "",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
                                        IsEmpty = true,
                                        
                                        AllowAirVehicle = false,
                                        AllowBoat = false,
                                        ForcedScenarios = new List<string>()
                                        {},
                                        OverrideNightPercentage = -1f,
                                        OverrideDayPercentage = -1f,
                                        OverridePoorWeatherPercentage = -1f,
                                        MinHourSpawn = 0,
                                        MaxHourSpawn = 24,
                                        MinWantedLevelSpawn = 0,
                                        MaxWantedLevelSpawn = 6,
                                        LongGunAlwaysEquipped = false,
            
            
                                        ForceLongGun = false,
                                },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(LupisellaBayviewLodge);
        BlankLocation LupisellaCleaners = new BlankLocation()
        {
            
            Name = "LupisellaCleaners",
            Description = "2 Lupisella's outside No Mark Cleaners",
            EntrancePosition = new Vector3(-54.82728f, 6469.723f, 31.02168f),
            EntranceHeading = 135.2057f,
            OpenTime = 6,
            CloseTime = 20,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_LUPISELLA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-48.36329f, 6462.51f, 31.50292f),
                                            Heading = 56.16458f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-49.48232f, 6462.323f, 31.50379f),
                                            Heading = -56.60118f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-54.82728f, 6469.723f, 31.02168f),
                                            Heading = 135.2057f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-47.38145f, 6445.597f, 31.16079f),
                                            Heading = 43.22633f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = false,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(LupisellaCleaners);
        BlankLocation LupisellaDreamView = new BlankLocation()
        {
            
            Name = "LupisellaDreamView",
            Description = "3 Lupisella's outside Dream View Motel",
            EntrancePosition = new Vector3(-82.15873f, 6358.285f, 30.99591f),
            EntranceHeading = 39.88159f,
            OpenTime = 6,
            CloseTime = 20,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_LUPISELLA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-83.23766f, 6365.269f, 31.49033f),
                                            Heading = -45.41143f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-81.05659f, 6368.31f, 31.49037f),
                                            Heading = -159.8493f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_AA_COFFEE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-80.17055f, 6366.876f, 31.4904f),
                                            Heading = 43.25166f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_AA_COFFEE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-82.15873f, 6358.285f, 30.99591f),
                                        Heading = 39.88159f,
                                        Percentage = 0f,
                                        AssociationID = "",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
                                        IsEmpty = true,
                                        
                                        AllowAirVehicle = false,
                                        AllowBoat = false,
                                        ForcedScenarios = new List<string>()
                                        {},
                                        OverrideNightPercentage = -1f,
                                        OverrideDayPercentage = -1f,
                                        OverridePoorWeatherPercentage = -1f,
                                        MinHourSpawn = 0,
                                        MaxHourSpawn = 24,
                                        MinWantedLevelSpawn = 0,
                                        MaxWantedLevelSpawn = 6,
                                        LongGunAlwaysEquipped = false,
            
            
                                        ForceLongGun = false,
                                },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(LupisellaDreamView);
        BlankLocation LupisellaForestCabin = new BlankLocation()
        {
            
            Name = "LupisellaForestCabin",
            Description = "2 Lupisella's outside a cabin at the Peleto Forest Works",
            EntrancePosition = new Vector3(-837.5941f, 5408.731f, 34.03185f),
            EntranceHeading = 135.7674f,
            OpenTime = 6,
            CloseTime = 20,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_LUPISELLA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-841.2857f, 5404.244f, 34.61536f),
                                            Heading = 160.506f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-842.0833f, 5403.106f, 34.61521f),
                                            Heading = -53.53604f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_AA_COFFEE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-837.5941f, 5408.731f, 34.03185f),
                                        Heading = 135.7674f,
                                        Percentage = 0f,
                                        AssociationID = "",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
                                        IsEmpty = true,
                                        
                                        AllowAirVehicle = false,
                                        AllowBoat = false,
                                        ForcedScenarios = new List<string>()
                                        {},
                                        OverrideNightPercentage = -1f,
                                        OverrideDayPercentage = -1f,
                                        OverridePoorWeatherPercentage = -1f,
                                        MinHourSpawn = 0,
                                        MaxHourSpawn = 24,
                                        MinWantedLevelSpawn = 0,
                                        MaxWantedLevelSpawn = 6,
                                        LongGunAlwaysEquipped = false,
            
            
                                        ForceLongGun = false,
                                },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(LupisellaForestCabin);
        BlankLocation LupisellaMojitoInn = new BlankLocation()
        {
            
            Name = "LupisellaMojitoInn",
            Description = "2 Lupisella's outside Mojito Inn",
            EntrancePosition = new Vector3(-119.8819f, 6396.609f, 31.1784f),
            EntranceHeading = -137.0415f,
            OpenTime = 6,
            CloseTime = 20,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_LUPISELLA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-121.8238f, 6392.171f, 31.48633f),
                                            Heading = -29.99005f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-120.9473f, 6392.521f, 31.49091f),
                                            Heading = 101.2144f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_AA_COFFEE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-119.8819f, 6396.609f, 31.1784f),
                                        Heading = -137.0415f,
                                        Percentage = 0f,
                                        AssociationID = "",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
                                        IsEmpty = true,
                                        
                                        AllowAirVehicle = false,
                                        AllowBoat = false,
                                        ForcedScenarios = new List<string>()
                                        {},
                                        OverrideNightPercentage = -1f,
                                        OverrideDayPercentage = -1f,
                                        OverridePoorWeatherPercentage = -1f,
                                        MinHourSpawn = 0,
                                        MaxHourSpawn = 24,
                                        MinWantedLevelSpawn = 0,
                                        MaxWantedLevelSpawn = 6,
                                        LongGunAlwaysEquipped = false,
            
            
                                        ForceLongGun = false,
                                },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(LupisellaMojitoInn);
    }
    private void MarabuntaGang()
    {
        BlankLocation MarabunteCov = new BlankLocation()
        {
            
            Name = "MarabunteCov",
            Description = "Marabunte Group at Covington industries carpark",
            MapIcon = 78,
            EntrancePosition = new Vector3(1393.407f, -2052.183f, 51.69846f),
            EntranceHeading = 80.12449f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_MARABUNTE",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1415.304f, -2048.288f, 51.9987f),
                                            Heading = 28.08802f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1409.528f, -2049.636f, 51.99854f),
                                            Heading = 53.12953f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1409.19f, -2048.742f, 51.99854f),
                                            Heading = 126.3349f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRINKING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1415.303f, -2047.234f, 51.9987f),
                                            Heading = 169.9342f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1411.732f, -2041.453f, 51.99854f),
                                            Heading = 173.8058f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1409.996f, -2043.661f, 51.99854f),
                                            Heading = -45.68226f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1411.706f, -2043.447f, 51.99854f),
                                            Heading = 13.09387f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1393.407f, -2052.183f, 51.69846f),
                                            Heading = 80.12449f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = false,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1412.171f, -2052.095f, 51.69852f),
                                            Heading = -71.06799f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1413.701f, -2055.817f, 51.69852f),
                                            Heading = -77.48627f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(MarabunteCov);
        BlankLocation MarabunteElRancho = new BlankLocation()
        {
            
            Name = "MarabunteElRancho",
            FullName = "",
            Description = "3 Marabunte on El Rancho blvd",
            MapIcon = 78,
            EntrancePosition = new Vector3(1384.687f, -1724.289f, 65.41503f),
            EntranceHeading = -161.0745f,
            OpenTime = 0,
            CloseTime = 24,
            
            
            AssignedAssociationID = "AMBIENT_GANG_MARABUNTE",
            MenuID = "",

            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                new ConditionalGroup()
                {
                    Name = "",
                    Percentage = defaultSpawnPercentage,
                    MaxWantedLevelSpawn = 6,
                    PossiblePedSpawns = new List<ConditionalLocation>()
                    {
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(1384.687f, -1724.289f, 65.41503f),
                            Heading = -161.0745f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>() { "WORLD_HUMAN_DRUG_DEALER_HARD", },
                            MaxWantedLevelSpawn = 6,
                        },
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(1383.464f, -1725.513f, 65.42934f),
                            Heading = -130.6673f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>() { "WORLD_HUMAN_HANG_OUT_STREET", },
                            MaxWantedLevelSpawn = 6,
                        },
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(1383.361f, -1726.442f, 65.44674f),
                            Heading = -74.39036f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>() { "WORLD_HUMAN_GUARD_STAND_CASINO", },
                            MaxWantedLevelSpawn = 6,
                        },
                    },
                    PossibleVehicleSpawns = new List<ConditionalLocation>() {  },
                },
            },
            PossiblePedSpawns = new List<ConditionalLocation>() { },
            PossibleVehicleSpawns = new List<ConditionalLocation>() { },
            VehiclePreviewLocation = new SpawnPlace() { },
        };
        BlankLocationPlaces.Add(MarabunteElRancho);
        BlankLocation MarabunteGasStation = new BlankLocation()
        {
            
            Name = "MarabunteGasStation",
            FullName = "",
            Description = "2 Marabunte at a Gas Station",
            MapIcon = 78,
            EntrancePosition = new Vector3(1218.259f, -1387.44f, 34.87067f),
            EntranceHeading = -0.6632997f,
            OpenTime = 0,
            CloseTime = 24,
            
            
            AssignedAssociationID = "AMBIENT_GANG_MARABUNTE",
            MenuID = "",

            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                new ConditionalGroup()
                {
                    Name = "",
                    Percentage = defaultSpawnPercentage,
                    MinHourSpawn = 8,
                    MaxHourSpawn = 20,
                    MaxWantedLevelSpawn = 6,
                    PossiblePedSpawns = new List<ConditionalLocation>()
                    {
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(1214.634f, -1388.464f, 35.37428f),
                            Heading = -104.529f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>() { "WORLD_HUMAN_SMOKING_POT", },
                            MaxWantedLevelSpawn = 6,
                        },
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(1215.164f, -1387.683f, 35.36345f),
                            Heading = 175.8869f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>() { "WORLD_HUMAN_HANG_OUT_STREET", },
                            MaxWantedLevelSpawn = 6,
                        },
                    },
                    PossibleVehicleSpawns = new List<ConditionalLocation>()
                    {
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(1218.259f, -1387.44f, 34.87067f),
                            Heading = -0.6632997f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            ForcedScenarios = new List<String>() {  },
                            MaxWantedLevelSpawn = 6,
                        },
                    },
                },
            },
            PossiblePedSpawns = new List<ConditionalLocation>() { },
            PossibleVehicleSpawns = new List<ConditionalLocation>() { },
            VehiclePreviewLocation = new SpawnPlace() { },
        };
        BlankLocationPlaces.Add(MarabunteGasStation);
        BlankLocation MarabunteLsTattoo = new BlankLocation()
        {
            
            Name = "MarabunteLsTattoo",
            FullName = "",
            Description = "2 Marabunte outside Los Santos Tattoos",
            MapIcon = 78,
            EntrancePosition = new Vector3(1326.727f, -1645.607f, 52.14602f),
            EntranceHeading = 39.81541f,
            OpenTime = 0,
            CloseTime = 24,
            
            
            AssignedAssociationID = "AMBIENT_GANG_MARABUNTE",
            MenuID = "",

            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                new ConditionalGroup()
                {
                    Name = "",
                    Percentage = defaultSpawnPercentage,
                    MinHourSpawn = 8,
                    MaxHourSpawn = 20,
                    MaxWantedLevelSpawn = 6,
                    PossiblePedSpawns = new List<ConditionalLocation>()
                    {
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(1326.727f, -1645.607f, 52.14602f),
                            Heading = 39.81541f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>()
                            {
                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                            },
                            MaxWantedLevelSpawn = 6,
                        },
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(1325.66f, -1646.106f, 52.14637f),
                            Heading = 10.92023f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>() { "WORLD_HUMAN_HANG_OUT_STREET", },
                            MaxWantedLevelSpawn = 6,
                        },
                    },
                    PossibleVehicleSpawns = new List<ConditionalLocation>() {  },
                },
            },
            PossiblePedSpawns = new List<ConditionalLocation>() { },
            PossibleVehicleSpawns = new List<ConditionalLocation>() { },
            VehiclePreviewLocation = new SpawnPlace() { },
        };
        BlankLocationPlaces.Add(MarabunteLsTattoo);
        BlankLocation MarabunteTaco = new BlankLocation()
        {
            
            Name = "MarabunteTaco",
            FullName = "",
            Description = "3 Marabunte at Moms Taco",
            MapIcon = 78,
            EntrancePosition = new Vector3(1117.474f, -971.5587f, 46.27171f),
            EntranceHeading = -141.6932f,
            OpenTime = 0,
            CloseTime = 24,
            
            
            AssignedAssociationID = "AMBIENT_GANG_MARABUNTE",
            MenuID = "",

            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                new ConditionalGroup()
                {
                    Name = "",
                    Percentage = defaultSpawnPercentage,
                    MinHourSpawn = 8,
                    MaxHourSpawn = 20,
                    MaxWantedLevelSpawn = 6,
                    PossiblePedSpawns = new List<ConditionalLocation>()
                    {
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(1121.75f, -969.1599f, 46.69672f),
                            Heading = 101.8158f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>()
                            {
                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                            },
                            MaxWantedLevelSpawn = 6,
                        },
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(1121.109f, -968.1682f, 46.7372f),
                            Heading = 131.3832f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>() { "WORLD_HUMAN_HANG_OUT_STREET", },
                            MaxWantedLevelSpawn = 6,
                        },
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(1119.432f, -969.8031f, 46.64055f),
                            Heading = -62.19331f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>() { "WORLD_HUMAN_SMOKING_POT", },
                            MaxWantedLevelSpawn = 6,
                        },
                    },
                    PossibleVehicleSpawns = new List<ConditionalLocation>()
                    {
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(1117.3f, -970.8827f, 46.29099f),
                            Heading = -135.015f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            ForcedScenarios = new List<String>() {  },
                            MaxWantedLevelSpawn = 6,
                        },
                    },
                },
            },
            PossiblePedSpawns = new List<ConditionalLocation>() { },
            PossibleVehicleSpawns = new List<ConditionalLocation>() { },
            VehiclePreviewLocation = new SpawnPlace() { },
        };
        BlankLocationPlaces.Add(MarabunteTaco);
    }
    private void MessinaGang()
    {
        BlankLocation MessinaAmmunation = new BlankLocation()
        {
            
            Name = "MessinaAmmunation",
            Description = "2 Messina's outside Ammunation",
            MapIcon = 78,
            EntrancePosition = new Vector3(-1322.7f, -393.5449f, 36.4208f),
            EntranceHeading = -148.2554f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_MESSINA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1316.872f, -394.3947f, 36.59461f),
                                            Heading = 167.5968f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_AA_COFFEE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1316.88f, -395.524f, 36.59184f),
                                            Heading = 30.17929f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-1322.7f, -393.5449f, 36.4208f),
                                        Heading = -148.2554f,
                                        Percentage = 0f,
                                        AssociationID = "",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
                                        IsEmpty = true,
                                        
                                        AllowAirVehicle = false,
                                        AllowBoat = false,
                                        ForcedScenarios = new List<string>()
                                        {},
                                        OverrideNightPercentage = -1f,
                                        OverrideDayPercentage = -1f,
                                        OverridePoorWeatherPercentage = -1f,
                                        MinHourSpawn = 0,
                                        MaxHourSpawn = 24,
                                        MinWantedLevelSpawn = 0,
                                        MaxWantedLevelSpawn = 6,
                                        LongGunAlwaysEquipped = false,
            
            
                                        ForceLongGun = false,
                                },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(MessinaAmmunation);
        BlankLocation MessinaBeanMachine = new BlankLocation()
        {
            
            Name = "MessinaBeanMachine",
            Description = "3 Messina's outside Bean Machine",
            MapIcon = 78,
            EntrancePosition = new Vector3(-1535.508f, -434.5028f, 35.14352f),
            EntranceHeading = 45.2971f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_MESSINA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1541.759f, -432.183f, 35.5913f),
                                            Heading = -54.94258f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_AA_COFFEE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1540.763f, -430.9757f, 35.59424f),
                                            Heading = 140.8866f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT",
                                                "WORLD_HUMAN_AA_SMOKE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-1535.508f, -434.5028f, 35.14352f),
                                        Heading = 45.2971f,
                                        Percentage = 0f,
                                        AssociationID = "",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
                                        IsEmpty = true,
                                        
                                        AllowAirVehicle = false,
                                        AllowBoat = false,
                                        ForcedScenarios = new List<string>()
                                        {},
                                        OverrideNightPercentage = -1f,
                                        OverrideDayPercentage = -1f,
                                        OverridePoorWeatherPercentage = -1f,
                                        MinHourSpawn = 0,
                                        MaxHourSpawn = 24,
                                        MinWantedLevelSpawn = 0,
                                        MaxWantedLevelSpawn = 6,
                                        LongGunAlwaysEquipped = false,
            
            
                                        ForceLongGun = false,
                                },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(MessinaBeanMachine);
        BlankLocation MessinaLSGents = new BlankLocation()
        {
            
            Name = "MessinaLSGents",
            Description = "2 Messina outside the LS Gentstyle",
            MapIcon = 78,
            EntrancePosition = new Vector3(-1367.202f, -277.5847f, 42.57183f),
            EntranceHeading = 5.040933f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_MESSINA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1367.68f, -276.3194f, 42.56871f),
                                            Heading = -145.3672f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1367.202f, -277.5847f, 42.57183f),
                                            Heading = 5.040933f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {},
                    },
                },
        };
        BlankLocationPlaces.Add(MessinaLSGents);
        BlankLocation MessinaPosonby = new BlankLocation()
        {
            
            Name = "MessinaPosonby",
            Description = "2 Messina's outside Posonbys",
            MapIcon = 78,
            EntrancePosition = new Vector3(-1456.012f, -226.0376f, 48.75779f),
            EntranceHeading = -40.88657f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_MESSINA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1452.701f, -227.3664f, 49.13041f),
                                            Heading = 44.61046f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1453.512f, -227.1425f, 49.15277f),
                                            Heading = -93.40581f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-1456.012f, -226.0376f, 48.75779f),
                                        Heading = -40.88657f,
                                        Percentage = 0f,
                                        AssociationID = "",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
                                        IsEmpty = true,
                                        
                                        AllowAirVehicle = false,
                                        AllowBoat = false,
                                        ForcedScenarios = new List<string>()
                                        {},
                                        OverrideNightPercentage = -1f,
                                        OverrideDayPercentage = -1f,
                                        OverridePoorWeatherPercentage = -1f,
                                        MinHourSpawn = 0,
                                        MaxHourSpawn = 24,
                                        MinWantedLevelSpawn = 0,
                                        MaxWantedLevelSpawn = 6,
                                        LongGunAlwaysEquipped = false,
            
            
                                        ForceLongGun = false,
                                },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(MessinaPosonby);
    }
    private void PavanoGang()
    {
        BlankLocation PavanoAlamoFruit = new BlankLocation()
        {
            
            Name = "PavanoAlamoFruit",
            Description = "2 Pavano outside Alamo Fruit Market",
            EntrancePosition = new Vector3(1785.255f, 4584.821f, 37.20624f),
            EntranceHeading = 3.471748f,
            OpenTime = 6,
            CloseTime = 20,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_PAVANO",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1784.07f, 4591.093f, 37.68299f),
                                            Heading = -147.9982f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1785.032f, 4591.322f, 37.68299f),
                                            Heading = -179.6894f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(1785.255f, 4584.821f, 37.20624f),
                                        Heading = 3.471748f,
                                        Percentage = 0f,
                                        AssociationID = "",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
                                        IsEmpty = true,
                                        
                                        AllowAirVehicle = false,
                                        AllowBoat = false,
                                        ForcedScenarios = new List<string>()
                                        {},
                                        OverrideNightPercentage = -1f,
                                        OverrideDayPercentage = -1f,
                                        OverridePoorWeatherPercentage = -1f,
                                        MinHourSpawn = 0,
                                        MaxHourSpawn = 24,
                                        MinWantedLevelSpawn = 0,
                                        MaxWantedLevelSpawn = 6,
                                        LongGunAlwaysEquipped = false,
            
            
                                        ForceLongGun = false,
                                },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(PavanoAlamoFruit);
        BlankLocation PavanoUnionGrain = new BlankLocation()
        {
            
            Name = "PavanoUnionGrain",
            Description = "2 Pavano outside UnionGrain",
            EntrancePosition = new Vector3(2024.766f, 4974.324f, 40.88439f),
            EntranceHeading = 57.40964f,
            OpenTime = 6,
            CloseTime = 20,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_PAVANO",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(2026.098f, 4976.521f, 41.166f),
                                            Heading = -74.69242f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_GUARD_STAND_CASINO",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(2032.79f, 4983.199f, 40.98968f),
                                            Heading = -179.275f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(2023.895f, 4974.527f, 40.90429f),
                                        Heading = 63.66342f,
                                        Percentage = 0f,
                                        AssociationID = "",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
                                        IsEmpty = true,
                                        
                                        AllowAirVehicle = false,
                                        AllowBoat = false,
                                        ForcedScenarios = new List<string>()
                                        {},
                                        OverrideNightPercentage = -1f,
                                        OverrideDayPercentage = -1f,
                                        OverridePoorWeatherPercentage = -1f,
                                        MinHourSpawn = 0,
                                        MaxHourSpawn = 24,
                                        MinWantedLevelSpawn = 0,
                                        MaxWantedLevelSpawn = 6,
                                        LongGunAlwaysEquipped = false,
            
            
                                        ForceLongGun = false,
                                },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(PavanoUnionGrain);
        BlankLocation PavanoAirport = new BlankLocation()
        {
            
            Name = "PavanoAirport",
            FullName = "",
            Description = "Pavano group at the Grapeseed Airport",
            MapIcon = 267,
            EntrancePosition = new Vector3(2153.416f, 4795.97f, 40.86898f),
            EntranceHeading = -142.5477f,
            OpenTime = 0,
            CloseTime = 24,
            
            
            AssignedAssociationID = "AMBIENT_GANG_PAVANO",
            MenuID = "",

            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                new ConditionalGroup()
                {
                    Name = "",
                    Percentage = defaultSpawnPercentage,
                    MinHourSpawn = 18,
                    MaxHourSpawn = 4,
                    MaxWantedLevelSpawn = 6,
                    PossiblePedSpawns = new List<ConditionalLocation>()
                    {
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(2155.05f, 4796.923f, 41.18592f),
                            Heading = -98.30238f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>() { "WORLD_HUMAN_STAND_IMPATIENT", },
                            MaxWantedLevelSpawn = 6,
                        },
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(2147.093f, 4801.103f, 41.07898f),
                            Heading = 78.20504f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>() { "WORLD_HUMAN_STAND_IMPATIENT", },
                            MaxWantedLevelSpawn = 6,
                        },
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(2156.818f, 4796.323f, 41.18224f),
                            Heading = 54.83329f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>() { "WORLD_HUMAN_CLIPBOARD", },
                            MaxWantedLevelSpawn = 6,
                        },
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(2146.332f, 4802.325f, 41.13522f),
                            Heading = 109.8892f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>() { "WORLD_HUMAN_BINOCULARS", },
                            MaxWantedLevelSpawn = 6,
                        },
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(2155.904f, 4797.563f, 41.17632f),
                            Heading = -160.9346f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>() { "WORLD_HUMAN_SMOKING", },
                            MaxWantedLevelSpawn = 6,
                        },
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(2154.087f, 4801.979f, 41.06139f),
                            Heading = 22.90802f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>() { "WORLD_HUMAN_GUARD_STAND", },
                            MaxWantedLevelSpawn = 6,
                            ForceSidearm = true,
                            ForceLongGun = true,
                        },
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(2144.564f, 4796.351f, 41.09901f),
                            Heading = 12.6271f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>() { "WORLD_HUMAN_GUARD_STAND", },
                            MaxWantedLevelSpawn = 6,
                            ForceSidearm = true,
                            ForceLongGun = true,
                        },
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(2156.143f, 4795.569f, 41.17105f),
                            Heading = 25.26263f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>() { "WORLD_HUMAN_GUARD_STAND", },
                            MaxWantedLevelSpawn = 6,
                            ForceSidearm = true,
                            ForceLongGun = true,
                        },
                    },
                    PossibleVehicleSpawns = new List<ConditionalLocation>()
                    {
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(2158.846f, 4810.209f, 40.89357f),
                            Heading = -113.571f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            IsEmpty = false,
                            ForcedScenarios = new List<String>() {  },
                            MaxWantedLevelSpawn = 6,
                        },
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(2153.421f, 4795.948f, 40.86859f),
                            Heading = -142.5614f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            ForcedScenarios = new List<String>() {  },
                            MaxWantedLevelSpawn = 6,
                        },
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(2147.887f, 4798.083f, 40.79086f),
                            Heading = 140.454f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            ForcedScenarios = new List<String>() {  },
                            MaxWantedLevelSpawn = 6,
                        },
                    },
                },
            },
            PossiblePedSpawns = new List<ConditionalLocation>() { },
            PossibleVehicleSpawns = new List<ConditionalLocation>() { },
            VehiclePreviewLocation = new SpawnPlace() { },
        };
        BlankLocationPlaces.Add(PavanoAirport);
        BlankLocation PavanoFarmEggs = new BlankLocation()
        {
            
            Name = "PavanoFarmEggs",
            FullName = "",
            Description = "5 Pavano outside derelict Farm Eggs building",
            MapIcon = 267,
            EntrancePosition = new Vector3(2552.402f, 4673.885f, 33.6469f),
            EntranceHeading = 19.66658f,
            OpenTime = 0,
            CloseTime = 24,
            
            
            AssignedAssociationID = "AMBIENT_GANG_PAVANO",
            MenuID = "",

            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                new ConditionalGroup()
                {
                    Name = "",
                    Percentage = defaultSpawnPercentage,
                    MinHourSpawn = 18,
                    MaxHourSpawn = 4,
                    MaxWantedLevelSpawn = 6,
                    PossiblePedSpawns = new List<ConditionalLocation>()
                    {
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(2555.109f, 4652.64f, 34.07679f),
                            Heading = 114.9075f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>() { "WORLD_HUMAN_GUARD_STAND_CASINO", },
                            MaxWantedLevelSpawn = 6,
                        },
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(2551.27f, 4668.713f, 34.07679f),
                            Heading = -173.3012f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>() { "WORLD_HUMAN_SMOKING", },
                            MaxWantedLevelSpawn = 6,
                        },
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(2551.574f, 4667.034f, 34.07679f),
                            Heading = 27.89994f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>()
                            {
                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                            },
                            MaxWantedLevelSpawn = 6,
                        },
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(2548.861f, 4657.532f, 34.07679f),
                            Heading = 32.28136f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>() { "WORLD_HUMAN_STAND_MOBILE", },
                            MaxWantedLevelSpawn = 6,
                        },
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(2550.346f, 4668.052f, 34.07679f),
                            Heading = -125.7162f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>() { "WORLD_HUMAN_SMOKING", },
                            MaxWantedLevelSpawn = 6,
                        },
                    },
                    PossibleVehicleSpawns = new List<ConditionalLocation>()
                    {
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(2552.402f, 4673.885f, 33.6469f),
                            Heading = 19.66658f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            IsEmpty = false,
                            ForcedScenarios = new List<String>() {  },
                            MaxWantedLevelSpawn = 6,
                        },
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(2551.396f, 4655.577f, 33.77676f),
                            Heading = -127.3213f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            ForcedScenarios = new List<String>() {  },
                            MaxWantedLevelSpawn = 6,
                        },
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(2552.898f, 4649.568f, 33.77676f),
                            Heading = -108.6875f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            ForcedScenarios = new List<String>() {  },
                            MaxWantedLevelSpawn = 6,
                        },
                    },
                },
            },
            PossiblePedSpawns = new List<ConditionalLocation>() { },
            PossibleVehicleSpawns = new List<ConditionalLocation>() { },
            VehiclePreviewLocation = new SpawnPlace() { },
        };
        BlankLocationPlaces.Add(PavanoFarmEggs);
        BlankLocation PavanoGasStation = new BlankLocation()
        {
            
            Name = "PavanoGasStation",
            FullName = "",
            Description = "2 Pavano members outside Old Gas Station",
            MapIcon = 267,
            EntrancePosition = new Vector3(1695.865f, 4925.759f, 42.23172f),
            EntranceHeading = 77.33406f,
            OpenTime = 0,
            CloseTime = 24,
            
            
            AssignedAssociationID = "AMBIENT_GANG_PAVANO",
            MenuID = "",

            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                new ConditionalGroup()
                {
                    Name = "",
                    Percentage = defaultSpawnPercentage,
                    MinHourSpawn = 18,
                    MaxHourSpawn = 4,
                    MaxWantedLevelSpawn = 6,
                    PossiblePedSpawns = new List<ConditionalLocation>()
                    {
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(1695.865f, 4925.759f, 42.23172f),
                            Heading = 77.33406f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>() { "WORLD_HUMAN_SMOKING", },
                            MaxWantedLevelSpawn = 6,
                        },
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(1695.47f, 4924.892f, 42.23172f),
                            Heading = 19.46928f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>() { "WORLD_HUMAN_STAND_MOBILE", },
                            MaxWantedLevelSpawn = 6,
                        },
                    },
                    PossibleVehicleSpawns = new List<ConditionalLocation>() {  },
                },
            },
            PossiblePedSpawns = new List<ConditionalLocation>() { },
            PossibleVehicleSpawns = new List<ConditionalLocation>() { },
            VehiclePreviewLocation = new SpawnPlace() { },
        };
        BlankLocationPlaces.Add(PavanoGasStation);
        BlankLocation PavanoStore = new BlankLocation()
        {
            
            Name = "PavanoStore",
            FullName = "",
            Description = "2 Pavano members outside an old Store",
            MapIcon = 267,
            EntrancePosition = new Vector3(1655.66f, 4873.412f, 42.03714f),
            EntranceHeading = -29.68659f,
            OpenTime = 0,
            CloseTime = 24,
            
            
            AssignedAssociationID = "AMBIENT_GANG_PAVANO",
            MenuID = "",

            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                new ConditionalGroup()
                {
                    Name = "",
                    Percentage = defaultSpawnPercentage,
                    MinHourSpawn = 8,
                    MaxHourSpawn = 18,
                    MaxWantedLevelSpawn = 6,
                    PossiblePedSpawns = new List<ConditionalLocation>()
                    {
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(1655.66f, 4873.412f, 42.03714f),
                            Heading = -29.68659f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>() { "WORLD_HUMAN_SMOKING", },
                            MaxWantedLevelSpawn = 6,
                        },
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(1655.326f, 4874.587f, 42.03675f),
                            Heading = -89.606f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>() { "WORLD_HUMAN_STAND_MOBILE", },
                            MaxWantedLevelSpawn = 6,
                        },
                    },
                    PossibleVehicleSpawns = new List<ConditionalLocation>() {  },
                },
            },
            PossiblePedSpawns = new List<ConditionalLocation>() { },
            PossibleVehicleSpawns = new List<ConditionalLocation>() { },
            VehiclePreviewLocation = new SpawnPlace() { },
        };
        BlankLocationPlaces.Add(PavanoStore);
    }
    private void TriadGang()
    {
        BlankLocation TRIADSConstructionSite = new BlankLocation()
        {
            
            Name = "TRIADSConstructionSite",
            Description = "Triads's meeting in a construction site pit",
            EntrancePosition = new Vector3(-96.0987f, -981.312f, 20.97673f),
            EntranceHeading = -55.66203f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 20,
                            MaxHourSpawn = 4,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 3,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-95.48102f, -972.4038f, 21.27685f),
                                            Heading = 171.8519f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-95.61737f, -975.4075f, 21.27685f),
                                            Heading = -0.4084174f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_IMPATIENT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-96.51656f, -973.0062f, 21.27685f),
                                            Heading = -139.5084f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-96.0987f, -981.312f, 20.97673f),
                                            Heading = -55.66203f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-98.90174f, -972.0678f, 20.97684f),
                                            Heading = 163.0585f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(TRIADSConstructionSite);
        BlankLocation TRIADSUnderGroundParking = new BlankLocation()
        {
            
            Name = "TRIADSUnderGroundParking",
            Description = "Triads's meeting in Underground Parking",
            EntrancePosition = new Vector3(-148.7391f, -615.175f, 32.42778f),
            EntranceHeading = 24.18717f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 20,
                            MaxHourSpawn = 4,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 3,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-144.9849f, -613.7332f, 32.42448f),
                                            Heading = -56.97806f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_IMPATIENT",
                                                "WORLD_HUMAN_STAND_MOBILE_UPRIGHT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-143.3552f, -613.566f, 32.58904f),
                                            Heading = 66.68578f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_IMPATIENT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-144.1747f, -612.4281f, 32.42451f),
                                            Heading = 168.0162f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_DRINKING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-149.8138f, -607.623f, 32.42457f),
                                            Heading = 160.985f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_IMPATIENT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-148.7391f, -615.175f, 32.42778f),
                                            Heading = 24.18717f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-149.4514f, -608.7309f, 32.42451f),
                                            Heading = 34.62045f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_IMPATIENT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-147.6679f, -610.4272f, 31.81277f),
                                            Heading = 0.7192951f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-148.1147f, -616.1925f, 31.81277f),
                                            Heading = -61.16846f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-155.7619f, -606.876f, 31.81277f),
                                            Heading = 159.7329f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = false,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,

                
                

                                    },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(TRIADSUnderGroundParking);
        BlankLocation TRIADSLegionSquare = new BlankLocation()
        {
            
            Name = "TRIADSLegionSquare",
            FullName = "",
            Description = "2 Triads's on Legion Square",
            MapIcon = 47,
            MapIconScale = 1f,
            EntrancePosition = new Vector3(185.3129f, -1015.362f, 28.60604f),
            EntranceHeading = 26.83933f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = "SanAndreas",
            
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
            MenuID = "",

            PossibleGroupSpawns =
      new List<ConditionalGroup>() {
        new ConditionalGroup() {
          Name = "",
          Percentage = defaultSpawnPercentage,
          MinHourSpawn = 20,
          MaxHourSpawn = 4,
          PossiblePedSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(183.4249f, -1010.787f, 29.32248f),
                  Heading = 173.0021f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_HANG_OUT_STREET",
                      },
                },
                new GangConditionalLocation() {
                  Location = new Vector3(184.5667f, -1011.243f, 29.32097f),
                  Heading = 125.9036f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_HANG_OUT_STREET",
                        "WORLD_HUMAN_STAND_IMPATIENT",
                      },
                },
              },
          PossibleVehicleSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(185.3129f, -1015.362f, 28.60604f),
                  Heading = 26.83933f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  ForcedScenarios = new List<String>() {},
                },
              },
        },
      },
            PossiblePedSpawns = new List<ConditionalLocation>() { },
            PossibleVehicleSpawns = new List<ConditionalLocation>() { },
            VehiclePreviewLocation = new SpawnPlace() { },
            VehicleDeliveryLocations = new List<SpawnPlace>() { },
        };
        BlankLocationPlaces.Add(TRIADSLegionSquare);
    }
    private void YardiesGang()
    {
        BlankLocation YARDIESAlDente = new BlankLocation()
        {
            
            Name = "YARDIESAlDente",
            Description = "5 Yardies hanging around the parking nextto Al Dente",
            EntrancePosition = new Vector3(-1168.661f, -1394.101f, 4.297443f),
            EntranceHeading = -54.6035f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1171.235f, -1402.458f, 4.791392f),
                                            Heading = -134.3366f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1162.057f, -1395.071f, 4.982103f),
                                            Heading = -132.9119f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1160.886f, -1395.882f, 4.987898f),
                                            Heading = 69.5871f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1170.755f, -1401.114f, 4.798217f),
                                            Heading = -30.60785f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1169.988f, -1400.296f, 4.809137f),
                                            Heading = 114.2852f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1168.661f, -1394.101f, 4.297443f),
                                            Heading = -54.6035f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1176.128f, -1394.281f, 4.159958f),
                                            Heading = -162.7868f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = false,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(YARDIESAlDente);
        BlankLocation YARDIESAlleyway = new BlankLocation()
        {
            
            Name = "YARDIESAlleyway",
            Description = "5 Yardies hanging around in a Alleyway",
            EntrancePosition = new Vector3(-1309.823f, -1253.378f, 4.213122f),
            EntranceHeading = -116.2065f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1312.861f, -1265.839f, 4.56466f),
                                            Heading = 18.83071f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1314.071f, -1265.487f, 4.568307f),
                                            Heading = -71.99977f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1313.826f, -1261.572f, 4.569271f),
                                            Heading = -6.832188f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1313.586f, -1260.51f, 4.566921f),
                                            Heading = 154.9569f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1313.07f, -1264.061f, 4.565801f),
                                            Heading = 170.9982f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1309.823f, -1253.378f, 4.213122f),
                                            Heading = -116.2065f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = false,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1315.709f, -1262.779f, 4.274897f),
                                            Heading = -177.752f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1310.752f, -1260.001f, 4.250442f),
                                            Heading = 146.7783f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(YARDIESAlleyway);
        BlankLocation YARDIESBayCity = new BlankLocation()
        {
            
            Name = "YARDIESBayCity",
            Description = "2 Yardies on Bay City Ave",
            EntrancePosition = new Vector3(-997.6016f, -1603.78f, 4.366539f),
            EntranceHeading = -101.3723f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-987.2784f, -1606.027f, 5.177531f),
                                            Heading = 92.35553f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-987.6706f, -1605.108f, 5.177553f),
                                            Heading = 149.2433f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-997.6016f, -1603.78f, 4.366539f),
                                        Heading = -101.3723f,
                                        Percentage = 0f,
                                        AssociationID = "",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
                                        IsEmpty = true,
                                        
                                        AllowAirVehicle = false,
                                        AllowBoat = false,
                                        ForcedScenarios = new List<string>()
                                        {},
                                        OverrideNightPercentage = -1f,
                                        OverrideDayPercentage = -1f,
                                        OverridePoorWeatherPercentage = -1f,
                                        MinHourSpawn = 0,
                                        MaxHourSpawn = 24,
                                        MinWantedLevelSpawn = 0,
                                        MaxWantedLevelSpawn = 6,
                                        LongGunAlwaysEquipped = false,
            
            
                                        ForceLongGun = false,
                                },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(YARDIESBayCity);
        BlankLocation YARDIESBeach = new BlankLocation()
        {
            
            Name = "YARDIESBeach",
            Description = "5 Yardies hanging out at the beach",
            EntrancePosition = new Vector3(-1346.998f, -1515.024f, 3.89973f),
            EntranceHeading = 147.8038f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1347.855f, -1511.24f, 4.330812f),
                                            Heading = 58.06147f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1348.046f, -1510.096f, 4.31727f),
                                            Heading = 152.5544f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1348.914f, -1511.522f, 4.36415f),
                                            Heading = -20.31668f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1348.367f, -1518.251f, 4.462275f),
                                            Heading = 135.5194f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1349.315f, -1517.844f, 4.485174f),
                                            Heading = 177.2622f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-1346.998f, -1515.024f, 3.89973f),
                                        Heading = 147.8038f,
                                        Percentage = 0f,
                                        AssociationID = "",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
                                        IsEmpty = true,
                                        
                                        AllowAirVehicle = false,
                                        AllowBoat = false,
                                        ForcedScenarios = new List<string>()
                                        {},
                                        OverrideNightPercentage = -1f,
                                        OverrideDayPercentage = -1f,
                                        OverridePoorWeatherPercentage = -1f,
                                        MinHourSpawn = 0,
                                        MaxHourSpawn = 24,
                                        MinWantedLevelSpawn = 0,
                                        MaxWantedLevelSpawn = 6,
                                        LongGunAlwaysEquipped = false,
            
            
                                        ForceLongGun = false,
                                },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(YARDIESBeach);
        BlankLocation YARDIESBeanMachine = new BlankLocation()
        {
            
            Name = "YARDIESBeanMachine",
            Description = "3 Yardies near Bean Machine",
            EntrancePosition = new Vector3(-1279.127f, -1145.539f, 5.873619f),
            EntranceHeading = -74.50941f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1274.925f, -1145.413f, 6.781503f),
                                            Heading = 122.7415f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1274.598f, -1146.178f, 6.780774f),
                                            Heading = 85.48699f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1276.667f, -1146.568f, 6.442255f),
                                            Heading = -67.12011f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-1279.127f, -1145.539f, 5.873619f),
                                        Heading = -74.50941f,
                                        Percentage = 0f,
                                        AssociationID = "",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
                                        IsEmpty = true,
                                        
                                        AllowAirVehicle = false,
                                        AllowBoat = false,
                                        ForcedScenarios = new List<string>()
                                        {},
                                        OverrideNightPercentage = -1f,
                                        OverrideDayPercentage = -1f,
                                        OverridePoorWeatherPercentage = -1f,
                                        MinHourSpawn = 0,
                                        MaxHourSpawn = 24,
                                        MinWantedLevelSpawn = 0,
                                        MaxWantedLevelSpawn = 6,
                                        LongGunAlwaysEquipped = false,
            
            
                                        ForceLongGun = false,
                                },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(YARDIESBeanMachine);
        BlankLocation YARDIESMagellanAve = new BlankLocation()
        {
            
            Name = "YARDIESMagellanAve",
            Description = "4 Yardies hanging around Magellan Ave",
            EntrancePosition = new Vector3(-1260.589f, -1241.573f, 4.723385f),
            EntranceHeading = 112.8918f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1241.811f, -1234.128f, 10.62588f),
                                            Heading = 32.14138f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1247.411f, -1236.228f, 6.956614f),
                                            Heading = 106.167f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1244.219f, -1235.904f, 10.62586f),
                                            Heading = -28.03158f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1243.557f, -1234.832f, 10.62588f),
                                            Heading = 120.9905f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1240.455f, -1225.581f, 6.232487f),
                                            Heading = 107.4703f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = false,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1260.589f, -1241.573f, 4.723385f),
                                            Heading = 112.8918f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            ForcedScenarios = new List<string>()
                                            {},
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(YARDIESMagellanAve);
        BlankLocation YARDIESPolomino = new BlankLocation()
        {
            
            Name = "YARDIESPolomino",
            Description = "3 Yardies on the corner of Polomino Ave",
            EntrancePosition = new Vector3(-1219.959f, -1353.113f, 4.133864f),
            EntranceHeading = -174.9922f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1219.959f, -1353.113f, 4.133864f),
                                            Heading = -174.9922f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1219.314f, -1354.162f, 4.129272f),
                                            Heading = 52.99246f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1219.075f, -1352.959f, 4.150446f),
                                            Heading = 160.3237f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {},
                    },
                },
        };
        BlankLocationPlaces.Add(YARDIESPolomino);
        BlankLocation YARDIESharkBites = new BlankLocation()
        {
            
            Name = "YARDIESharkBites",
            Description = "2 Yardies next to Shark Bites",
            EntrancePosition = new Vector3(-1284.461f, -1400.887f, 4.083547f),
            EntranceHeading = -69.54259f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1291.066f, -1403.797f, 4.576171f),
                                            Heading = 144.7734f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1290.953f, -1404.865f, 4.540293f),
                                            Heading = 62.02555f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-1284.461f, -1400.887f, 4.083547f),
                                        Heading = -69.54259f,
                                        Percentage = 0f,
                                        AssociationID = "",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
                                        IsEmpty = true,
                                        
                                        AllowAirVehicle = false,
                                        AllowBoat = false,
                                        ForcedScenarios = new List<string>()
                                        {},
                                        OverrideNightPercentage = -1f,
                                        OverrideDayPercentage = -1f,
                                        OverridePoorWeatherPercentage = -1f,
                                        MinHourSpawn = 0,
                                        MaxHourSpawn = 24,
                                        MinWantedLevelSpawn = 0,
                                        MaxWantedLevelSpawn = 6,
                                        LongGunAlwaysEquipped = false,
            
            
                                        ForceLongGun = false,
                                },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(YARDIESharkBites);
        BlankLocation YARDIESlicenDice = new BlankLocation()
        {
            
            Name = "YARDIESlicenDice",
            Description = "3 Yardies outside Slice N Dice",
            EntrancePosition = new Vector3(-1341.714f, -1302.357f, 4.836801f),
            EntranceHeading = 84.03486f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1341.714f, -1302.357f, 4.836801f),
                                            Heading = 84.03486f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1342.897f, -1302.667f, 4.838303f),
                                            Heading = -39.28256f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1343.118f, -1301.398f, 4.834866f),
                                            Heading = -91.57836f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {},
                    },
                },
        };
        BlankLocationPlaces.Add(YARDIESlicenDice);
        BlankLocation YARDIESmokeWater = new BlankLocation()
        {
            
            Name = "YARDIESmokeWater",
            Description = "3 Yardies outside Smoke on the Water",
            EntrancePosition = new Vector3(-1190.044f, -1573.374f, 4.367838f),
            EntranceHeading = -49.86687f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1189.846f, -1572.24f, 4.352987f),
                                            Heading = -113.0345f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1190.044f, -1573.374f, 4.367838f),
                                            Heading = -49.86687f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1188.584f, -1572.457f, 4.329321f),
                                            Heading = 110.9064f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {},
                    },
                },
        };
        BlankLocationPlaces.Add(YARDIESmokeWater);
        BlankLocation YARDIESVespLiquor = new BlankLocation()
        {
            
            Name = "YARDIESVespLiquor",
            Description = "3 Yardies outside Vespucci Liquor Market",
            EntrancePosition = new Vector3(-1103.732f, -1288.805f, 5.425843f),
            EntranceHeading = -40.54066f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1103.732f, -1288.805f, 5.425843f),
                                            Heading = -40.54066f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1103.177f, -1287.8f, 5.432659f),
                                            Heading = -178.2888f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1102.349f, -1288.668f, 5.437f),
                                            Heading = 78.69747f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {},
                    },
                },
        };
        BlankLocationPlaces.Add(YARDIESVespLiquor);
        BlankLocation YARDIESteamBoat = new BlankLocation()
        {
            
            Name = "YARDIESteamBoat",
            Description = "2 Yardies outside SteamBoat Beers",
            EntrancePosition = new Vector3(-1210.031f, -1386.722f, 4.079804f),
            EntranceHeading = -136.9996f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.SanAndreasStateID,
            
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1210.031f, -1386.722f, 4.079804f),
                                            Heading = -136.9996f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1209.042f, -1387.469f, 4.073753f),
                                            Heading = 60.15644f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_DRINKING",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1188.584f, -1572.457f, 4.329321f),
                                            Heading = 110.9064f,
                                            Percentage = 0f,
                                            AssociationID = "",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,
                                            
                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 6,

                
                

                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {},
                    },
                },
        };
        BlankLocationPlaces.Add(YARDIESteamBoat);
    }
    private void FamiliesGang()
    {
        BlankLocation FamiliesBaseballField = new BlankLocation()
        {
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation()
                {
                    TerritorySpawnsForceMainGang = false,
                    Location = new Vector3(-301.9806f, -1641.12f, 32.12749f),
                    Heading = -122.441f,
                    Percentage = defaultSpawnPercentage,
                    MinHourSpawn = 20,
                    MaxHourSpawn = 4,
                },
                new GangConditionalLocation()
                {
                    Location = new Vector3(-314.6769f, -1640.273f, 31.84881f),
                    Heading = -6.660462f,
                    Percentage = defaultSpawnPercentage,
                    MinHourSpawn = 20,
                    MaxHourSpawn = 4,
                },
                new GangConditionalLocation()
                {
                    TerritorySpawnsForceMainGang = false,
                    Location = new Vector3(-315.531f, -1639.359f, 31.84881f),
                    Heading = -86.77119f,
                    Percentage = defaultSpawnPercentage,
                    MinHourSpawn = 20,
                    MaxHourSpawn = 4,
                },
                new GangConditionalLocation()
                {
                    TerritorySpawnsForceMainGang = false,
                    Location = new Vector3(-314.3615f, -1638.53f, 31.84881f),
                    Heading = 160.9149f,
                    Percentage = defaultSpawnPercentage,
                    MinHourSpawn = 20,
                    MaxHourSpawn = 4,
                },
                new GangConditionalLocation()
                {
                    TerritorySpawnsForceMainGang = false,
                    Location = new Vector3(-314.5116f, -1631.374f, 31.84881f),
                    Heading = -85.18255f,
                    Percentage = defaultSpawnPercentage,
                    MinHourSpawn = 20,
                    MaxHourSpawn = 4,
                },
                new GangConditionalLocation()
                {
                    TerritorySpawnsForceMainGang = false,
                    Location = new Vector3(-313.6505f, -1630.512f, 31.84881f),
                    Heading = 174.248f,
                    Percentage = defaultSpawnPercentage,
                    MinHourSpawn = 20,
                    MaxHourSpawn = 4,
                },
                new GangConditionalLocation()
                {
                    TerritorySpawnsForceMainGang = false,
                    Location = new Vector3(-312.4868f, -1630.584f, 31.84881f),
                    Heading = -92.0695f,
                    Percentage = defaultSpawnPercentage,
                    MinHourSpawn = 20,
                    MaxHourSpawn = 4,
                },
                new GangConditionalLocation()
                {
                    TerritorySpawnsForceMainGang = false,
                    Location = new Vector3(-300.5563f, -1658.229f, 31.84879f),
                    Heading = -72.46208f,
                    Percentage = defaultSpawnPercentage,
                    MinHourSpawn = 20,
                    MaxHourSpawn = 4,
                },
                new GangConditionalLocation()
                {
                    TerritorySpawnsForceMainGang = false,
                    Location = new Vector3(-299.8108f, -1659.356f, 31.84879f),
                    Heading = -0.9050441f,
                    Percentage = defaultSpawnPercentage,
                    MinHourSpawn = 20,
                    MaxHourSpawn = 4,
                },
                new GangConditionalLocation()
                {
                    TerritorySpawnsForceMainGang = false,
                    Location = new Vector3(-298.2735f, -1658.152f, 31.8488f),
                    Heading = 117.1161f,
                    Percentage = defaultSpawnPercentage,
                    MinHourSpawn = 20,
                    MaxHourSpawn = 4,
                },
            },

            AssignedAssociationID = "AMBIENT_GANG_FAMILY",
            Name = "FamiliesBaseballField",
            Description = "Families Baseball Field",
            EntrancePosition = new Vector3(-301.9806f, -1641.12f, 32.12749f),
            EntranceHeading = -122.441f,
        };
        BlankLocationPlaces.Add(FamiliesBaseballField);
        BlankLocation FamiliesMeetup = new BlankLocation()
        {
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation()
                {
                    Location = new Vector3(-176.7435f, -1430.187f, 31.28525f),
                    Heading = -179.4815f,
                    Percentage = defaultSpawnPercentage,
                    MinHourSpawn = 18,
                    MaxHourSpawn = 3,
                    TaskRequirements = TaskRequirements.Guard,
                    ForcedScenarios = new List<String>()
                    {
                        "WORLD_HUMAN_DRUG_DEALER","WORLD_HUMAN_HANG_OUT_STREET","WORLD_HUMAN_DRINKING","WORLD_HUMAN_SMOKING","WORLD_HUMAN_SMOKING_POT"
                    },
                },
                new GangConditionalLocation()
                {
                    Location = new Vector3(-174.276f, -1431.17f, 31.2626f),
                    Heading = -127.0762f,
                    Percentage = defaultSpawnPercentage,
                    MinHourSpawn = 18,
                    MaxHourSpawn = 3,
                    TaskRequirements = TaskRequirements.Guard,
                    ForcedScenarios = new List<String>()
                    {
                        "WORLD_HUMAN_DRUG_DEALER","WORLD_HUMAN_HANG_OUT_STREET","WORLD_HUMAN_DRINKING","WORLD_HUMAN_SMOKING","WORLD_HUMAN_SMOKING_POT"
                    },
                },
                new GangConditionalLocation()
                {
                    Location = new Vector3(-173.2247f, -1432.683f, 31.26154f),
                    Heading = 3.284423f,
                    Percentage = defaultSpawnPercentage,
                    MinHourSpawn = 18,
                    MaxHourSpawn = 3,
                    TaskRequirements = TaskRequirements.Guard,
                    ForcedScenarios = new List<String>()
                    {
                        "WORLD_HUMAN_DRUG_DEALER","WORLD_HUMAN_HANG_OUT_STREET","WORLD_HUMAN_DRINKING","WORLD_HUMAN_SMOKING","WORLD_HUMAN_SMOKING_POT"
                    },
                },
                new GangConditionalLocation()
                {
                    Location = new Vector3(-172.6694f, -1431.62f, 31.24565f),
                    Heading = 118.8518f,
                    Percentage = defaultSpawnPercentage,
                    MinHourSpawn = 18,
                    MaxHourSpawn = 3,
                    ForcedScenarios = new List<String>()
                    {
                        "WORLD_HUMAN_DRUG_DEALER","WORLD_HUMAN_HANG_OUT_STREET","WORLD_HUMAN_DRINKING","WORLD_HUMAN_SMOKING","WORLD_HUMAN_SMOKING_POT"
                    },
                },
                new GangConditionalLocation()
                {
                    Location = new Vector3(-176.829f, -1431.623f, 31.28296f),
                    Heading = -6.896521f,
                    Percentage = defaultSpawnPercentage,
                    MinHourSpawn = 18,
                    MaxHourSpawn = 3,
                    TaskRequirements = TaskRequirements.Guard,
                    ForcedScenarios = new List<String>()
                    {
                        "WORLD_HUMAN_DRUG_DEALER","WORLD_HUMAN_HANG_OUT_STREET","WORLD_HUMAN_DRINKING","WORLD_HUMAN_SMOKING","WORLD_HUMAN_SMOKING_POT"
                    },
                },
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new GangConditionalLocation()
                {
                    Location = new Vector3(-174.8521f, -1438.027f, 30.81062f),
                    Heading = -38.86021f,
                    Percentage = defaultSpawnPercentage,
                    MinHourSpawn = 18,
                    MaxHourSpawn = 3,
                },
                new GangConditionalLocation()
                {
                    Location = new Vector3(-177.9715f, -1435.419f, 30.82336f),
                    Heading = 139.328f,
                    Percentage = defaultSpawnPercentage,
                    MinHourSpawn = 18,
                    MaxHourSpawn = 3,
                },
            },
            AssignedAssociationID = "AMBIENT_GANG_FAMILY",
            Name = "FamiliesMeetUp",
            Description = "Families Meet Up",
            EntrancePosition = new Vector3(-176.7435f, -1430.187f, 31.28525f),
            EntranceHeading = -179.4815f,
        };
        BlankLocationPlaces.Add(FamiliesMeetup);
    }
    private void RedneckGang()
    {
        BlankLocation RedneckTrailerHome2 = new BlankLocation()
        {
            
            Name = "RedneckTrailerHome2",
            FullName = "",
            Description = "2 4 Rednecks at a Trailer Home",
            MapIcon = 47,
            MapIconScale = 1f,
            EntrancePosition = new Vector3(1647.225f, 3735.363f, 33.9863f),
            EntranceHeading = -150.9536f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = "SanAndreas",
            
            AssignedAssociationID = "AMBIENT_GANG_HILLBILLY",
            MenuID = "",

            PossibleGroupSpawns =
      new List<ConditionalGroup>() {
        new ConditionalGroup() {
          Name = "",
         Percentage = defaultSpawnPercentage,
          MinHourSpawn = 18,
          MaxHourSpawn = 4,
          PossiblePedSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(1641.842f, 3728.282f, 35.06772f),
                  Heading = -43.25674f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_STUPOR_CLUBHOUSE",
                      },
                  MaxWantedLevelSpawn = 6,
                },
                new GangConditionalLocation() {
                  Location = new Vector3(1642.127f, 3730.09f, 35.06772f),
                  Heading = -56.33468f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_DRINKING",
                        "WORLD_HUMAN_SMOKING",
                      },
                  MaxWantedLevelSpawn = 6,
                },
              },
          PossibleVehicleSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(1647.225f, 3735.363f, 33.9863f),
                  Heading = -150.9536f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  ForcedScenarios = new List<String>() {},
                  MaxWantedLevelSpawn = 6,
                },
              },
        },
      },
            PossiblePedSpawns = new List<ConditionalLocation>() { },
            PossibleVehicleSpawns = new List<ConditionalLocation>() { },
            VehiclePreviewLocation = new SpawnPlace() { },
            VehicleDeliveryLocations = new List<SpawnPlace>() { },
        };
        BlankLocationPlaces.Add(RedneckTrailerHome2);
        BlankLocation RednecksAutoShop = new BlankLocation()
        {
            
            Name = "RednecksAutoShop",
            FullName = "",
            Description = "2 Rednecks outside a AutoShop",
            MapIcon = 47,
            MapIconScale = 1f,
            EntrancePosition = new Vector3(435.9423f, 3576.459f, 32.93855f),
            EntranceHeading = 141.9928f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = "SanAndreas",
            
            AssignedAssociationID = "AMBIENT_GANG_HILLBILLY",
            MenuID = "",

            PossibleGroupSpawns =
              new List<ConditionalGroup>() {
        new ConditionalGroup() {
          Name = "",
          Percentage = defaultSpawnPercentage,
          MinHourSpawn = 8,
          MaxHourSpawn = 20,
          PossiblePedSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(439.6789f, 3571.475f, 33.23857f),
                  Heading = -17.9575f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                      },
                  MaxWantedLevelSpawn = 6,
                },
                new GangConditionalLocation() {
                  Location = new Vector3(440.8992f, 3572.117f, 33.23857f),
                  Heading = 79.47934f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_SMOKING",
                      },
                  MaxWantedLevelSpawn = 6,
                },
              },
          PossibleVehicleSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(435.9423f, 3576.459f, 32.93855f),
                  Heading = 141.9928f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  ForcedScenarios = new List<String>() {},
                  MaxWantedLevelSpawn = 6,
                },
              },
        },
              },
            PossiblePedSpawns = new List<ConditionalLocation>() { },
            PossibleVehicleSpawns = new List<ConditionalLocation>() { },
            VehiclePreviewLocation = new SpawnPlace() { },
            VehicleDeliveryLocations = new List<SpawnPlace>() { },
        };
        BlankLocationPlaces.Add(RednecksAutoShop);
        BlankLocation RedneckFishing = new BlankLocation()
        {
            
            Name = "RedneckFishing",
            FullName = "",
            Description =
              "3 Redneck members 1 vehicle, Located alongside the Alamo Sea on 'Marina Drive' in Sandy Shores",
            MapIcon = 47,
            MapIconScale = 1f,
            EntrancePosition = new Vector3(788.3149f, 3649.923f, 31.83295f),
            EntranceHeading = 115.3078f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = "SanAndreas",
            
            AssignedAssociationID = "AMBIENT_GANG_HILLBILLY",
            MenuID = "",

            PossibleGroupSpawns =
              new List<ConditionalGroup>() {
        new ConditionalGroup() {
          Name = "",
          Percentage = defaultSpawnPercentage,
          MinHourSpawn = 8,
          MaxHourSpawn = 14,
          PossiblePedSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(784.4203f, 3659.302f, 32.26934f),
                  Heading = 7.872441f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_STAND_FISHING",
                      },
                  MaxWantedLevelSpawn = 6,
                },
                new GangConditionalLocation() {
                  Location = new Vector3(782.5395f, 3649.606f, 31.98152f),
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_DRINKING",
                        "WORLD_HUMAN_SMOKING",
                      },
                  MaxWantedLevelSpawn = 6,
                },
                new GangConditionalLocation() {
                  Location = new Vector3(783.4971f, 3649.846f, 31.99263f),
                  Heading = 34.22563f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_DRINKING",
                        "WORLD_HUMAN_SMOKING",
                      },
                  MaxWantedLevelSpawn = 6,
                },
              },
          PossibleVehicleSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(788.3149f, 3649.923f, 31.83295f),
                  Heading = 115.3078f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  ForcedScenarios = new List<String>() {},
                  MaxWantedLevelSpawn = 6,
                },
              },
        },
              },
            PossiblePedSpawns = new List<ConditionalLocation>() { },
            PossibleVehicleSpawns = new List<ConditionalLocation>() { },
            VehiclePreviewLocation = new SpawnPlace() { },
            VehicleDeliveryLocations = new List<SpawnPlace>() { },
        };
        BlankLocationPlaces.Add(RedneckFishing);
        BlankLocation RedneckGasStation = new BlankLocation()
        {
            
            Name = "RedneckGasStation",
            FullName = "",
            Description = "2 Redneck members at the local gas station, located on the corner of 'Alhambra Drive' and 'Marina Drive' in Sandy Shores",
            MapIcon = 47,
            MapIconScale = 1f,
            EntrancePosition = new Vector3(1984.251f, 3781.3f, 31.74913f),
            EntranceHeading = 118.9358f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = "SanAndreas",
            
            AssignedAssociationID = "AMBIENT_GANG_HILLBILLY",
            MenuID = "",

            PossibleGroupSpawns =
              new List<ConditionalGroup>() {
        new ConditionalGroup() {
          Name = "",
          Percentage = defaultSpawnPercentage,
          MinHourSpawn = 18,
          MaxHourSpawn = 4,
          PossiblePedSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(1983.932f, 3782.45f, 32.1808f),
                  Heading = 131.9805f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_DRINKING",
                        "WORLD_HUMAN_SMOKING",
                      },
                  MaxWantedLevelSpawn = 6,
                },
                new GangConditionalLocation() {
                  Location = new Vector3(1982.629f, 3782.644f, 32.1808f),
                  Heading = -153.7699f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_DRINKING",
                        "WORLD_HUMAN_SMOKING",
                      },
                  MaxWantedLevelSpawn = 6,
                },
              },
          PossibleVehicleSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(1981.157f, 3783.118f, 31.74942f),
                  Heading = -21.27289f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  ForcedScenarios = new List<String>() {},
                  MaxWantedLevelSpawn = 6,
                },
              },
        },
              },
            PossiblePedSpawns = new List<ConditionalLocation>() { },
            PossibleVehicleSpawns = new List<ConditionalLocation>() { },
            VehiclePreviewLocation = new SpawnPlace() { },
            VehicleDeliveryLocations = new List<SpawnPlace>() { },
        };
        BlankLocationPlaces.Add(RedneckGasStation);
        BlankLocation RednecksOttosAuto = new BlankLocation()
        {
            
            Name = "RednecksOttosAuto",
            FullName = "",
            Description = "2 Rednecks outside Ottos AutoParts",
            MapIcon = 47,
            MapIconScale = 1f,
            EntrancePosition = new Vector3(1930.142f, 3715.775f, 32.21063f),
            EntranceHeading = 120.9f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = "SanAndreas",
            
            AssignedAssociationID = "AMBIENT_GANG_HILLBILLY",
            MenuID = "",

            PossibleGroupSpawns =
              new List<ConditionalGroup>() {
        new ConditionalGroup() {
          Name = "",
          Percentage = defaultSpawnPercentage,
          MinHourSpawn = 12,
          MaxHourSpawn = 20,
          PossiblePedSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(1927.956f, 3718.756f, 32.85894f),
                  Heading = -124.9988f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_SMOKING",
                      },
                  MaxWantedLevelSpawn = 6,
                },
                new GangConditionalLocation() {
                  Location = new Vector3(1928.772f, 3719.26f, 32.8513f),
                  Heading = -160.4967f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_SMOKING",
                      },
                  MaxWantedLevelSpawn = 6,
                },
              },
          PossibleVehicleSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(1930.142f, 3715.775f, 32.21063f),
                  Heading = 120.9f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  ForcedScenarios = new List<String>() {},
                  MaxWantedLevelSpawn = 6,
                },
              },
        },
              },
            PossiblePedSpawns = new List<ConditionalLocation>() { },
            PossibleVehicleSpawns = new List<ConditionalLocation>() { },
            VehiclePreviewLocation = new SpawnPlace() { },
            VehicleDeliveryLocations = new List<SpawnPlace>() { },
        };
        BlankLocationPlaces.Add(RednecksOttosAuto);
        BlankLocation RednecksRooftop = new BlankLocation()
        {
            
            Name = "RednecksRooftop",
            FullName = "",
            Description = "4 Rednecks on a Trailer Rooftop",
            MapIcon = 47,
            MapIconScale = 1f,
            EntrancePosition = new Vector3(1771.012f, 3786.912f, 33.50421f),
            EntranceHeading = 10.61678f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = "SanAndreas",
            
            AssignedAssociationID = "AMBIENT_GANG_HILLBILLY",
            MenuID = "",

            PossibleGroupSpawns =
              new List<ConditionalGroup>() {
        new ConditionalGroup() {
          Name = "",
          Percentage = defaultSpawnPercentage,
          MinHourSpawn = 18,
          MaxHourSpawn = 4,
          PossiblePedSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(1777.382f, 3804.975f, 38.3717f),
                  Heading = -168.973f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_DRINKING",
                        "WORLD_HUMAN_SMOKING",
                      },
                  MaxWantedLevelSpawn = 6,
                },
                new GangConditionalLocation() {
                  Location = new Vector3(1776.444f, 3804.161f, 38.3717f),
                  Heading = -107.7791f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_SMOKING",
                        "WORLD_HUMAN_DRINKING",
                      },
                  MaxWantedLevelSpawn = 6,
                },
                new GangConditionalLocation() {
                  Location = new Vector3(1779.232f, 3796.893f, 38.36992f),
                  Heading = 138.812f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_SMOKING",
                        "WORLD_HUMAN_DRINKING",
                      },
                  MaxWantedLevelSpawn = 6,
                },
                new GangConditionalLocation() {
                  Location = new Vector3(1777.14f, 3802.75f, 38.3717f),
                  Heading = -32.01409f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_SMOKING",
                        "WORLD_HUMAN_DRINKING",
                      },
                  MaxWantedLevelSpawn = 6,
                },
              },
          PossibleVehicleSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(1771.012f, 3786.912f, 33.50421f),
                  Heading = 10.61678f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  ForcedScenarios = new List<String>() {},
                  MaxWantedLevelSpawn = 6,
                },
              },
        },
              },
            PossiblePedSpawns = new List<ConditionalLocation>() { },
            PossibleVehicleSpawns = new List<ConditionalLocation>() { },
            VehiclePreviewLocation = new SpawnPlace() { },
            VehicleDeliveryLocations = new List<SpawnPlace>() { },
        };
        BlankLocationPlaces.Add(RednecksRooftop);
        BlankLocation RednecksTrailerHome = new BlankLocation()
        {
            
            Name = "RednecksTrailerHome",
            FullName = "",
            Description = "4 Rednecks at a Trailer Home",
            MapIcon = 47,
            MapIconScale = 1f,
            EntrancePosition = new Vector3(2166.039f, 3366.001f, 45.2073f),
            EntranceHeading = 61.83692f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = "SanAndreas",
            
            AssignedAssociationID = "AMBIENT_GANG_HILLBILLY",
            MenuID = "",

            PossibleGroupSpawns =
              new List<ConditionalGroup>() {
        new ConditionalGroup() {
          Name = "",
          Percentage = defaultSpawnPercentage,
          PossiblePedSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(2167.788f, 3379.139f, 46.43515f),
                  Heading = -139.515f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_DRINKING",
                        "WORLD_HUMAN_SMOKING",
                      },
                  MaxWantedLevelSpawn = 6,
                },
                new GangConditionalLocation() {
                  Location = new Vector3(2171.722f, 3384.602f, 45.27925f),
                  Heading = -62.30351f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_SMOKING",
                        "WORLD_HUMAN_DRINKING",
                      },
                  MaxWantedLevelSpawn = 6,
                },
                new GangConditionalLocation() {
                  Location = new Vector3(2164.474f, 3373.998f, 45.28975f),
                  Heading = -129.9006f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_DRINKING",
                        "WORLD_HUMAN_HANG_OUT_STREET",
                      },
                  MaxWantedLevelSpawn = 6,
                },
                new GangConditionalLocation() {
                  Location = new Vector3(2167.141f, 3377.952f, 46.43515f),
                  Heading = -97.85981f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_HANG_OUT_STREET",
                        "WORLD_HUMAN_SMOKING",
                      },
                  MaxWantedLevelSpawn = 6,
                },
              },
          PossibleVehicleSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(2166.039f, 3366.001f, 45.2073f),
                  Heading = 61.83692f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  ForcedScenarios = new List<String>() {},
                  MaxWantedLevelSpawn = 6,
                },
                new GangConditionalLocation() {
                  Location = new Vector3(2172.058f, 3370.323f, 45.04405f),
                  Heading = 104.7006f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  ForcedScenarios = new List<String>() {},
                  MaxWantedLevelSpawn = 6,
                },
              },
        },
              },
            PossiblePedSpawns = new List<ConditionalLocation>() { },
            PossibleVehicleSpawns = new List<ConditionalLocation>() { },
            VehiclePreviewLocation = new SpawnPlace() { },
            VehicleDeliveryLocations = new List<SpawnPlace>() { },
        };
        BlankLocationPlaces.Add(RednecksTrailerHome);
        BlankLocation RednecksYellowJack = new BlankLocation()
        {
            
            Name = "RednecksYellowJack",
            FullName = "",
            Description = "4 Rednecks at a Yellow Jack Bar",
            MapIcon = 47,
            MapIconScale = 1f,
            EntrancePosition = new Vector3(2008.518f, 3075.025f, 46.76134f),
            EntranceHeading = -39.04045f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = "SanAndreas",
            
            AssignedAssociationID = "AMBIENT_GANG_HILLBILLY",
            MenuID = "",

            PossibleGroupSpawns =
              new List<ConditionalGroup>() {
        new ConditionalGroup() {
          Name = "",
          Percentage = defaultSpawnPercentage,
          MinHourSpawn = 18,
          MaxHourSpawn = 4,
          PossiblePedSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(1994.297f, 3052.58f, 47.21453f),
                  Heading = 7.996953f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_DRINKING",
                        "WORLD_HUMAN_SMOKING",
                      },
                  MaxWantedLevelSpawn = 6,
                },
                new GangConditionalLocation() {
                  Location = new Vector3(1993.31f, 3053.033f, 47.21463f),
                  Heading = -52.82331f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_SMOKING",
                        "WORLD_HUMAN_DRINKING",
                      },
                  MaxWantedLevelSpawn = 6,
                },
                new GangConditionalLocation() {
                  Location = new Vector3(2008.421f, 3077.022f, 47.06045f),
                  Heading = 61.92813f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_STUPOR_CLUBHOUSE",
                        "WORLD_HUMAN_DRINKING",
                      },
                  MaxWantedLevelSpawn = 6,
                },
                new GangConditionalLocation() {
                  Location = new Vector3(2006.853f, 3077.347f, 47.06097f),
                  Heading = -91.97454f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_SMOKING",
                        "WORLD_HUMAN_DRINKING",
                      },
                  MaxWantedLevelSpawn = 6,
                },
              },
          PossibleVehicleSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(2008.518f, 3075.025f, 46.76134f),
                  Heading = -39.04045f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  ForcedScenarios = new List<String>() {},
                  MaxWantedLevelSpawn = 6,
                },
                new GangConditionalLocation() {
                  Location = new Vector3(1995.843f, 3055.668f, 46.75429f),
                  Heading = -144.7406f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  ForcedScenarios = new List<String>() {},
                  MaxWantedLevelSpawn = 6,
                },
              },
        },
              },
            PossiblePedSpawns = new List<ConditionalLocation>() { },
            PossibleVehicleSpawns = new List<ConditionalLocation>() { },
            VehiclePreviewLocation = new SpawnPlace() { },
            VehicleDeliveryLocations = new List<SpawnPlace>() { },
        };
        BlankLocationPlaces.Add(RednecksYellowJack);
    }
    private void VarriosGang()
    {
        BlankLocation VarriosArenaCP = new BlankLocation()
        {
            
            Name = "VarriosArenaCP",
            FullName = "",
            Description = "7 Varrios members at Maze Bank Arena CarPark",
            MapIcon = 47,
            MapIconScale = 1f,
            EntrancePosition = new Vector3(-70.29264f, -2009.713f, 17.71691f),
            EntranceHeading = 29.88061f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = "SanAndreas",
            
            AssignedAssociationID = "AMBIENT_GANG_SALVA",
            MenuID = "",

            PossibleGroupSpawns =
              new List<ConditionalGroup>() {
        new ConditionalGroup() {
          Name = "",
          Percentage = defaultSpawnPercentage,
          MinHourSpawn = 18,
          MaxHourSpawn = 4,
          PossiblePedSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(-73.52132f, -2006.46f, 18.01696f),
                  Heading = 177.1098f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                      },
                  MaxWantedLevelSpawn = 6,
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-73.03613f, -2007.4f, 18.01695f),
                  Heading = 44.47162f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                        "WORLD_HUMAN_DRINKING",
                      },
                  MaxWantedLevelSpawn = 6,
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-69.9006f, -2006.079f, 18.01695f),
                  Heading = 38.83946f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_SMOKING_POT",
                        "WORLD_HUMAN_DRINKING_FACILITY",
                      },
                  MaxWantedLevelSpawn = 6,
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-68.86002f, -2005.374f, 18.01695f),
                  Heading = 64.1187f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_SMOKING_POT",
                        "WORLD_HUMAN_DRINKING",
                      },
                  MaxWantedLevelSpawn = 6,
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-71.58179f, -2004.387f, 18.01695f),
                  Heading = -105.5124f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_MUSICIAN",
                        "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                      },
                  MaxWantedLevelSpawn = 6,
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-68.98801f, -2003.032f, 18.01695f),
                  Heading = 115.7049f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                        "WORLD_HUMAN_DRINKING_FACILITY",
                      },
                  MaxWantedLevelSpawn = 6,
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-68.62316f, -2004.062f, 18.01695f),
                  Heading = 99.51196f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                        "WORLD_HUMAN_DRINKING_CASINO_TERRACE",
                      },
                  MaxWantedLevelSpawn = 6,
                },
              },
          PossibleVehicleSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(-70.29264f, -2009.713f, 17.71691f),
                  Heading = 29.88061f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  ForcedScenarios = new List<String>() {},
                  MaxWantedLevelSpawn = 6,
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-73.22175f, -2011.03f, 17.71679f),
                  Heading = 22.1294f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  ForcedScenarios = new List<String>() {},
                  MaxWantedLevelSpawn = 6,
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-64.20248f, -2007.571f, 17.71692f),
                  Heading = -178.1039f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  ForcedScenarios = new List<String>() {},
                  MaxWantedLevelSpawn = 6,
                },
                new GangConditionalLocation() {
                  Location = new Vector3(-65.01774f, -2014.222f, 17.71691f),
                  Heading = 109.9944f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  IsEmpty = false,
                  ForcedScenarios = new List<String>() {},
                  MaxWantedLevelSpawn = 6,
                },
              },
        },
              },
            PossiblePedSpawns = new List<ConditionalLocation>() { },
            PossibleVehicleSpawns = new List<ConditionalLocation>() { },
            VehiclePreviewLocation = new SpawnPlace() { },
            VehicleDeliveryLocations = new List<SpawnPlace>() { },
        };
        BlankLocationPlaces.Add(VarriosArenaCP);

    }
}

