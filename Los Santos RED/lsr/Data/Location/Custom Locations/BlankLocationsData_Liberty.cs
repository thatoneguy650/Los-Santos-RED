using Rage;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BlankLocationsData_Liberty
{
    private float defaultSpawnPercentage = 75;
    public BlankLocationsData_Liberty()
    {

    }
    public List<BlankLocation> BlankLocationPlaces { get; set; } = new List<BlankLocation>();
    public void DefaultConfig()
    {
        SpeedTraps();
        Checkpoints();
        RooftopSnipers();
        OtherCops();
    }
    private void RooftopSnipers()
    {
        float sniperSpawnPercentage = 65f;
        List<BlankLocation> blankLocationPlaces = new List<BlankLocation>() {
            //LC Snipers
            new BlankLocation(new Vector3(-148.9657f, 1058.107f, 40.98241f), 318.6407f, "LCRoofTopSniper1", "LCRooftop Sniper 1")
            {  //Middlepark North
                ActivateDistance = 300f,
                ActivateCells = 8,
                StateID = StaticStrings.LibertyStateID,

                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(-148.9657f, 1058.107f, 40.98241f), 318.6407f, sniperSpawnPercentage) {
                        MinWantedLevelSpawn = 2,
                        MaxWantedLevelSpawn = 4,
                        RequiredPedGroup = "Sniper",
                        TaskRequirements = TaskRequirements.Guard | TaskRequirements.EquipLongGunWhenIdle, LongGunAlwaysEquipped = true }, },
                },
            new BlankLocation(new Vector3(406.317f, 817.2524f, 25.17313f), 23.41006f, "LCRoofTopSniper2", "LCRooftop Sniper 2")
            {  //Civilian Committee
                ActivateDistance = 300f,
                ActivateCells = 8,
                StateID = StaticStrings.LibertyStateID,

                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(406.317f, 817.2524f, 25.17313f), 23.41006f, sniperSpawnPercentage) {
                        MinWantedLevelSpawn = 2,
                        MaxWantedLevelSpawn = 4,
                        RequiredPedGroup = "Sniper",
                        TaskRequirements = TaskRequirements.Guard | TaskRequirements.EquipLongGunWhenIdle, LongGunAlwaysEquipped = true }, },
                },
            new BlankLocation(new Vector3(53.68168f, 914.5931f, 36.98463f), 124.0438f, "LCRoofTopSniper3", "LCRooftop Sniper 3")
            {  //Star Junction
                ActivateDistance = 300f,
                ActivateCells = 8,
                StateID = StaticStrings.LibertyStateID,

                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(53.68168f, 914.5931f, 36.98463f), 124.0438f, sniperSpawnPercentage) {
                        MinWantedLevelSpawn = 2,
                        MaxWantedLevelSpawn = 4,
                        RequiredPedGroup = "Sniper",
                        TaskRequirements = TaskRequirements.Guard | TaskRequirements.EquipLongGunWhenIdle, LongGunAlwaysEquipped = true }, },
                },
            new BlankLocation(new Vector3(46.62069f, 150.2974f, 30.65244f), 192.7635f, "LCRoofTopSniper4", "LCRooftop Sniper 4")
            {  //Civic Civillian
                ActivateDistance = 300f,
                ActivateCells = 8,
                StateID = StaticStrings.LibertyStateID,

                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(46.62069f, 150.2974f, 30.65244f), 192.7635f, sniperSpawnPercentage) {
                        MinWantedLevelSpawn = 2,
                        MaxWantedLevelSpawn = 4,
                        RequiredPedGroup = "Sniper",
                        TaskRequirements = TaskRequirements.Guard | TaskRequirements.EquipLongGunWhenIdle, LongGunAlwaysEquipped = true }, },
                },
            new BlankLocation(new Vector3(391.3286f, -12.20265f, 40.67762f), 45.07904f, "LCRoofTopSniper5", "LCRooftop Sniper 5")
            {  //The Exchange - Chinatown
                ActivateDistance = 300f,
                ActivateCells = 8,
                StateID = StaticStrings.LibertyStateID,

                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(391.3286f, -12.20265f, 40.67762f), 45.07904f, sniperSpawnPercentage) {
                        MinWantedLevelSpawn = 2,
                        MaxWantedLevelSpawn = 4,
                        RequiredPedGroup = "Sniper",
                        TaskRequirements = TaskRequirements.Guard | TaskRequirements.EquipLongGunWhenIdle, LongGunAlwaysEquipped = true }, },
                },
            new BlankLocation(new Vector3(-294.488f, 1191.574f, 30.42546f), 41.20105f, "LCRoofTopSniper6", "LCRooftop Sniper 6")
            {  //Art Center - Mausoleum Overlook
                ActivateDistance = 300f,
                ActivateCells = 8,
                StateID = StaticStrings.LibertyStateID,

                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(-294.488f, 1191.574f, 30.42546f), 41.20105f, sniperSpawnPercentage) {
                        MinWantedLevelSpawn = 2,
                        MaxWantedLevelSpawn = 4,
                        RequiredPedGroup = "Sniper",
                        TaskRequirements = TaskRequirements.Guard | TaskRequirements.EquipLongGunWhenIdle, LongGunAlwaysEquipped = true }, },
                },
            new BlankLocation(new Vector3(216.4622f, 1384.932f, 44.60562f), 94.32448f, "LCRoofTopSniper7", "LCRooftop Sniper 7")
            {  //On top of Liberty City Delivery
                ActivateDistance = 300f,
                ActivateCells = 8,
                StateID = StaticStrings.LibertyStateID,

                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(216.4622f, 1384.932f, 44.60562f), 94.32448f, sniperSpawnPercentage) {
                        MinWantedLevelSpawn = 2,
                        MaxWantedLevelSpawn = 4,
                        RequiredPedGroup = "Sniper",
                        TaskRequirements = TaskRequirements.Guard | TaskRequirements.EquipLongGunWhenIdle, LongGunAlwaysEquipped = true }, },
                },
        };
        BlankLocationPlaces.AddRange(blankLocationPlaces);
    }
    private void Checkpoints()
    {
        List<BlankLocation> blankLocationPlaces = new List<BlankLocation>() {
            new BlankLocation(new Vector3(340.4053f, 1496.072f, 14.72459f), 268.257f, "BPCheckpoint1", "BP Checkpoint 1")
            { // Lancaster - Watching incoming traffic from Charge Island
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
                                new LEConditionalLocation(new Vector3(338.6601f, 1495.907f, 14.655f), 0.6866842f, 0f){
                                    AssociationID = "NOOSE-BP",  },
                                new LEConditionalLocation(new Vector3(338.6086f, 1514.7f, 14.64345f), 181.0613f, 0f){
                                    AssociationID = "NOOSE-BP",  },
                            },
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation(new Vector3(340.4053f, 1496.072f, 14.72459f), 268.257f, 0f){
                                    AssociationID = "NOOSE-BP",
                                    TaskRequirements = TaskRequirements.Guard,
                                    ForcedScenarios = new List<string>(){ "WORLD_HUMAN_BINOCULARS", "WORLD_HUMAN_CLIPBOARD" } },
                                new LEConditionalLocation(new Vector3(340.2132f, 1513.993f, 14.7118f), 263.1698f, 0f){
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
            new BlankLocation(new Vector3(-376.1587f, 1304.69f, 6.683456f), 32.41907f, "SpeedTrap1", "Speed Trap Union Drive West 1") {
                ActivateDistance = 300f,ActivateCells = 8,
                StateID = StaticStrings.LibertyStateID,
                PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup(){
                        Percentage = defaultSpawnPercentage,
                        OverrideNightPercentage = 0.0f,
                        OverridePoorWeatherPercentage = 0.0f,
                        PossibleVehicleSpawns  = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(-378.0014f, 1308.292f, 6.073559f), 162.4008f, 0f) {
                                AssociationID = "",
                                RequiredVehicleGroup = "Motorcycle", },
                        },
                        PossiblePedSpawns = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(-376.1587f, 1304.69f, 6.683456f), 32.41907f, 0f) {
                                AssociationID = "",
                                RequiredPedGroup = "MotorcycleCop",
                                TaskRequirements = TaskRequirements.Guard,
                                ForcedScenarios = new List<string>(){ "WORLD_HUMAN_BINOCULARS" } },
                        }
                    },

                }
            },
            new BlankLocation(new Vector3(-310.9234f, 848.72f, 6.659168f), 180.2172f, "SpeedTrap2", "Speed Trap Union Drive West 2") {
                ActivateDistance = 300f,ActivateCells = 8,
                StateID = StaticStrings.LibertyStateID,
                PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup(){
                        Percentage = defaultSpawnPercentage,
                        OverrideNightPercentage = 0.0f,
                        OverridePoorWeatherPercentage = 0.0f,
                        PossibleVehicleSpawns  = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(-309.9755f, 847.4606f, 5.984495f), 282.4508f, 0f) {
                                AssociationID = "",
                                RequiredVehicleGroup = "Motorcycle", },
                        },
                        PossiblePedSpawns = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(-310.9234f, 848.72f, 6.659168f), 180.2172f, 0f) {
                                AssociationID = "",
                                RequiredPedGroup = "MotorcycleCop",
                                TaskRequirements = TaskRequirements.Guard,
                                ForcedScenarios = new List<string>(){ "WORLD_HUMAN_BINOCULARS" } },
                        }
                    },

                }
            },
            new BlankLocation(new Vector3(381.8676f, 1442.678f, 14.71403f), 321.6855f, "SpeedTrap3", "Covering Union Drive East") {
                ActivateDistance = 300f,ActivateCells = 8,
                StateID = StaticStrings.LibertyStateID,
                PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup(){
                        Percentage = defaultSpawnPercentage,
                        OverrideNightPercentage = 0.0f,
                        OverridePoorWeatherPercentage = 0.0f,
                        PossibleVehicleSpawns  = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(378.6328f, 1443.008f, 14.71412f), 80.00423f, 0f) {
                                AssociationID = "",
                                RequiredVehicleGroup = "Motorcycle", },
                        },
                        PossiblePedSpawns = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(381.8676f, 1442.678f, 14.71403f), 321.6855f, 0f) {
                                AssociationID = "",
                                RequiredPedGroup = "MotorcycleCop",
                                TaskRequirements = TaskRequirements.Guard,
                                ForcedScenarios = new List<string>(){ "WORLD_HUMAN_BINOCULARS" } },
                        }
                    },

                }
            },
            new BlankLocation(new Vector3(455.6155f, 826.2332f, 8.60358f), 3.34502f, "SpeedTrap4", "Covering Union Drive East") {
                ActivateDistance = 300f,ActivateCells = 8,
                StateID = StaticStrings.LibertyStateID,
                PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup(){
                        Percentage = defaultSpawnPercentage,
                        OverrideNightPercentage = 0.0f,
                        OverridePoorWeatherPercentage = 0.0f,
                        PossibleVehicleSpawns  = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(453.3518f, 825.3931f, 8.081162f), 4.921218f, 0f) {
                                AssociationID = "",
                                RequiredVehicleGroup = "Motorcycle", },
                        },
                        PossiblePedSpawns = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(455.6155f, 826.2332f, 8.60358f), 3.34502f, 0f) {
                                AssociationID = "",
                                RequiredPedGroup = "MotorcycleCop",
                                TaskRequirements = TaskRequirements.Guard,
                                ForcedScenarios = new List<string>(){ "WORLD_HUMAN_BINOCULARS" } },
                        }
                    },

                }
            },
            new BlankLocation(new Vector3(403.1945f, 1277.182f, 7.644617f), 359.9992f, "SpeedTrap5", "Union Drive East Tunnel") {
                ActivateDistance = 300f,
                ActivateCells = 8,
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(403.1945f, 1277.182f, 7.644617f), 359.9992f, defaultSpawnPercentage) {
                        IsEmpty = false,
                        TaskRequirements = TaskRequirements.Guard,
                        OverrideNightPercentage = 55.0f,
                        OverridePoorWeatherPercentage = 0.0f },
                },
            },
            new BlankLocation(new Vector3(4.244688f, 807.3133f, 14.34105f), 267.0054f, "SpeedTrap6", "Star Junction Alley") {
                ActivateDistance = 300f,
                ActivateCells = 8,
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(4.244688f, 807.3133f, 14.34105f), 267.0054f, defaultSpawnPercentage) {
                        IsEmpty = false,
                        TaskRequirements = TaskRequirements.Guard,
                        OverrideNightPercentage = 55.0f,
                        OverridePoorWeatherPercentage = 0.0f },
                },
            },
            new BlankLocation(new Vector3(-233.4547f, 720.8707f, 9.467761f), 269.5866f, "SpeedTrap7", "Westminster Carpark") {
                ActivateDistance = 300f,
                ActivateCells = 8,
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(-233.4547f, 720.8707f, 9.467761f), 269.5866f, defaultSpawnPercentage) {
                        IsEmpty = false,
                        TaskRequirements = TaskRequirements.Guard,
                        OverrideNightPercentage = 55.0f,
                        OverridePoorWeatherPercentage = 0.0f },
                },
            },
            new BlankLocation(new Vector3(-71.00031f, 1683.867f, 14.61431f), 179.4064f, "SpeedTrap8", "Middle Park - Topaz Steet Alley") {
                ActivateDistance = 300f,
                ActivateCells = 8,
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(-71.00031f, 1683.867f, 14.61431f), 179.4064f, defaultSpawnPercentage) {
                        IsEmpty = false,
                        TaskRequirements = TaskRequirements.Guard,
                        OverrideNightPercentage = 55.0f,
                        OverridePoorWeatherPercentage = 0.0f },
                },
            },
            new BlankLocation(new Vector3(14.5392f, 1080.148f, 14.33973f), 270.0316f, "SpeedTrap9", "Star Junction Alley across from The Majestic") {
                ActivateDistance = 300f,
                ActivateCells = 8,
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(14.5392f, 1080.148f, 14.33973f), 270.0316f, defaultSpawnPercentage) {
                        IsEmpty = false,
                        TaskRequirements = TaskRequirements.Guard,
                        OverrideNightPercentage = 55.0f,
                        OverridePoorWeatherPercentage = 0.0f },
                },
            },
        };
        BlankLocationPlaces.AddRange(blankLocationPlaces);
    }
    private void OtherCops()
    {
        BlankLocation WestParkCops = new BlankLocation()
        {
            Name = "WestParkCops",
            FullName = "WestParkCops",
            Description = "2 Cops at West Park Subway",
            MapIcon = 162,
            EntrancePosition = new Vector3(-108.5168f, 1162.419f, 14.64934f),
            EntranceHeading = 76.93037f,
            OpenTime = 8,
            CloseTime = 16,
            StateID = StaticStrings.LibertyStateID,
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
                                        Location = new Vector3(-108.5168f, 1162.419f, 14.64934f),
                                            Heading = 76.93037f,
                                            Percentage = 0f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,

                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
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
                                        Location = new Vector3(-108.4362f, 1163.476f, 14.65312f),
                                            Heading = 97.58524f,
                                            Percentage = 0f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,

                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
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
                                    Location = new Vector3(-114.8848f, 1166.738f, 14.23472f),
                                        Heading = 0.1042213f,
                                        Percentage = 0f,
                                        //AssociationID = "LCPD",
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
        BlankLocationPlaces.Add(WestParkCops);
        BlankLocation EastonStationCops = new BlankLocation()
        {
            Name = "EastonStationCops",
            FullName = "EastonStationCops",
            Description = "2 Cops outside Easton Station on Star Junction",
            MapIcon = 162,
            EntrancePosition = new Vector3(123.7763f, 474.6643f, 14.77159f),
            EntranceHeading = 98.60426f,
            OpenTime = 8,
            CloseTime = 18,
            StateID = StaticStrings.LibertyStateID,
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
                                        Location = new Vector3(123.7763f, 474.6643f, 14.77159f),
                                            Heading = 98.60426f,
                                            Percentage = 0f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,

                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
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
                                        Location = new Vector3(123.4458f, 475.8339f, 14.77159f),
                                            Heading = 105.6469f,
                                            Percentage = 0f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,

                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
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
                    },
                },
        };
        BlankLocationPlaces.Add(EastonStationCops);
        BlankLocation CityHallCops = new BlankLocation()
        {
            Name = "CityHallCop",
            FullName = "CityHallCop",
            Description = "2 Cops outside a small park near City Hall",
            MapIcon = 162,
            EntrancePosition = new Vector3(83.132f, 74.11024f, 14.81516f),
            EntranceHeading = 333.8175f,
            OpenTime = 8,
            CloseTime = 18,
            StateID = StaticStrings.LibertyStateID,
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
                                        Location = new Vector3(83.132f, 74.11024f, 14.81516f),
                                            Heading = 333.8175f,
                                            Percentage = 0f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,

                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
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
                                        Location = new Vector3(83.91899f, 74.03098f, 14.81516f),
                                            Heading = 359.9721f,
                                            Percentage = 0f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,

                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
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
                    },
                },
        };
        BlankLocationPlaces.Add(CityHallCops);
        BlankLocation ColumbusCops = new BlankLocation()
        {
            Name = "ColumbusCops",
            FullName = "ColumbusCops",
            Description = "2 Cops outside Columbus Cathedral",
            MapIcon = 162,
            EntrancePosition = new Vector3(211.955f, 896.9236f, 15.76596f),
            EntranceHeading = 118.7021f,
            OpenTime = 12,
            CloseTime = 20,
            StateID = StaticStrings.LibertyStateID,
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
                                        Location = new Vector3(211.955f, 896.9236f, 15.76596f),
                                            Heading = 118.7021f,
                                            Percentage = 0f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,

                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
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
                                        Location = new Vector3(212.2547f, 895.9838f, 15.76595f),
                                            Heading = 106.0519f,
                                            Percentage = 0f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,

                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
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
                    },
                },
        };
        BlankLocationPlaces.Add(ColumbusCops);
        BlankLocation Pier45Cops = new BlankLocation()
        {
            Name = "Pier45Cops",
            FullName = "Pier45Cops",
            Description = "2 Cops at Pier 45 South Fish Market",
            MapIcon = 162,
            EntrancePosition = new Vector3(572.8592f, -129.6671f, 4.714124f),
            EntranceHeading = 63.50958f,
            OpenTime = 10,
            CloseTime = 18,
            StateID = StaticStrings.LibertyStateID,
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
                                        Location = new Vector3(572.8592f, -129.6671f, 4.714124f),
                                            Heading = 63.50958f,
                                            Percentage = 0f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,

                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
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
                                        Location = new Vector3(573.4944f, -128.6504f, 4.713513f),
                                            Heading = 66.54961f,
                                            Percentage = 0f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,

                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
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
                                    Location = new Vector3(567.2054f, -128.0005f, 4.258258f),
                                        Heading = 329.8925f,
                                        Percentage = 0f,
                                        //AssociationID = "LCPD",
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
        BlankLocationPlaces.Add(Pier45Cops);
        BlankLocation GoldenPierCops = new BlankLocation()
        {
            Name = "GoldenPierCops",
            FullName = "GoldenPierCops",
            Description = "2 Cops at Golden Pier",
            MapIcon = 162,
            EntrancePosition = new Vector3(-305.3127f, 562.3554f, 4.662493f),
            EntranceHeading = 88.52105f,
            OpenTime = 12,
            CloseTime = 20,
            StateID = StaticStrings.LibertyStateID,
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
                                        Location = new Vector3(-305.3127f, 562.3554f, 4.662493f),
                                            Heading = 88.52105f,
                                            Percentage = 0f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,

                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
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
                                        Location = new Vector3(-305.2951f, 563.2542f, 4.662493f),
                                            Heading = 91.95438f,
                                            Percentage = 0f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,

                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
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
                    },
                },
        };
        BlankLocationPlaces.Add(GoldenPierCops);
        BlankLocation FrankfortHighCops = new BlankLocation()
        {
            Name = "FrankfortHighCops",
            FullName = "FrankfortHighCops",
            Description = "2 Cops at Frankfort High Station",
            MapIcon = 162,
            EntrancePosition = new Vector3(-132.1275f, 1911.99f, 28.14749f),
            EntranceHeading = 275.3827f,
            OpenTime = 12,
            CloseTime = 22,
            StateID = StaticStrings.LibertyStateID,
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
                                        Location = new Vector3(-132.1275f, 1911.99f, 28.14749f),
                                            Heading = 275.3827f,
                                            Percentage = 0f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,

                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
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
                                        Location = new Vector3(-132.1894f, 1911.031f, 28.14749f),
                                            Heading = 270.731f,
                                            Percentage = 0f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,

                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
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
                    },
                },
        };
        BlankLocationPlaces.Add(FrankfortHighCops);
        BlankLocation WestminsterCops = new BlankLocation()
        {
            Name = "WestminsterCops",
            FullName = "WestminsterCops",
            Description = "2 Cops along the Westminister promenade",
            MapIcon = 162,
            EntrancePosition = new Vector3(429.2126f, 1582.839f, 8.44803f),
            EntranceHeading = 257.0893f,
            OpenTime = 10,
            CloseTime = 18,
            StateID = StaticStrings.LibertyStateID,
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
                                        Location = new Vector3(429.2126f, 1582.839f, 8.44803f),
                                            Heading = 257.0893f,
                                            Percentage = 0f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,

                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
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
                                        Location = new Vector3(429.3478f, 1583.712f, 8.44803f),
                                            Heading = 267.9536f,
                                            Percentage = 0f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,

                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
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
                    },
                },
        };
        BlankLocationPlaces.Add(WestminsterCops);
        BlankLocation CastleGardensCityCops = new BlankLocation()
        {
            Name = "CastleGardensCityCops",
            FullName = "CastleGardensCityCops",
            Description = "2 Cops on Castle Drive",
            MapIcon = 162,
            EntrancePosition = new Vector3(-284.8303f, 147.5739f, 5.939476f),
            EntranceHeading = 333.0757f,
            OpenTime = 8,
            CloseTime = 20,
            StateID = StaticStrings.LibertyStateID,
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
                                        Location = new Vector3(-284.8303f, 147.5739f, 5.939476f),
                                            Heading = 333.0757f,
                                            Percentage = 0f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,

                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
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
                                        Location = new Vector3(-284.0855f, 147.1189f, 5.933133f),
                                            Heading = 336.958f,
                                            Percentage = 0f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,

                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
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
                                    Location = new Vector3(-279.3899f, 154.2472f, 5.503436f),
                                        Heading = 335.2513f,
                                        Percentage = 0f,
                                        //AssociationID = "LCPD",
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
        BlankLocationPlaces.Add(CastleGardensCityCops);
        BlankLocation StarJunctionCops = new BlankLocation()
        {
            Name = "StarJunctionCops",
            FullName = "StarJunctionCops",
            Description = "2 Cops on Star Junction - Night",
            MapIcon = 162,
            EntrancePosition = new Vector3(15.10842f, 913.6707f, 14.81265f),
            EntranceHeading = 94.27563f,
            OpenTime = 18,
            CloseTime = 2,
            StateID = StaticStrings.LibertyStateID,
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
                                        Location = new Vector3(15.10842f, 913.6707f, 14.81265f),
                                            Heading = 94.27563f,
                                            Percentage = 0f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,

                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
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
                                        Location = new Vector3(15.11247f, 912.674f, 14.81221f),
                                            Heading = 93.1582f,
                                            Percentage = 0f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            IsEmpty = true,

                                            AllowAirVehicle = false,
                                            AllowBoat = false,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
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
                    },
                },
        };
        BlankLocationPlaces.Add(StarJunctionCops);
    }
}

