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
    private float defaultSpawnPercentage = 60;
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
            //Algonquin
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
            new BlankLocation(new Vector3(340.4053f, 1496.072f, 14.72459f), 268.257f, "CICheckpoint1", "Charge Island Incoming Checkpoint 1")
            { //Algonquin - Lancaster - Watching incoming traffic from Charge Island
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
            new BlankLocation(new Vector3(2215.184f, 860.1894f, 17.88266f), 83.68049f, "AirportCheckpoint1", "Francis Airport Checkpoint 2")
            { //Dukes - Francis Airport
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
                                new LEConditionalLocation(new Vector3(2214.838f, 855.8389f, 17.44806f), 359.0083f, 0f){
                                    AssociationID = "NOOSE-BP",  },
                                new LEConditionalLocation(new Vector3(2213.87f, 878.655f, 17.62628f), 180.5637f, 0f){
                                    AssociationID = "NOOSE-BP",  },
                            },
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation(new Vector3(2215.184f, 860.1894f, 17.88266f), 83.68049f, 0f){
                                    AssociationID = "NOOSE-BP",
                                    TaskRequirements = TaskRequirements.Guard,
                                    ForcedScenarios = new List<string>(){ "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_COP_IDLES" } },
                                new LEConditionalLocation(new Vector3(2215.262f, 861.6537f, 17.8964f), 84.53944f, 0f){
                                    AssociationID = "NOOSE-BP",
                                    TaskRequirements = TaskRequirements.Guard,
                                    ForcedScenarios = new List<string>(){ "WORLD_HUMAN_CLIPBOARD", "WORLD_HUMAN_COP_IDLES" } },
                                new LEConditionalLocation(new Vector3(2216.192f, 875.9774f, 17.92808f), 129.9711f, 0f){
                                    AssociationID = "NOOSE-BP",
                                    TaskRequirements = TaskRequirements.Guard,
                                    ForcedScenarios = new List<string>(){ "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_COP_IDLES" } },
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
            //Algonquin
            new BlankLocation(new Vector3(-376.1587f, 1304.69f, 6.683456f), 32.41907f, "ALGSpeedTrap1", "Speed Trap Union Drive West 1") {
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
            new BlankLocation(new Vector3(-310.9234f, 848.72f, 6.659168f), 180.2172f, "ALGSpeedTrap2", "Speed Trap Union Drive West 2") {
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
            new BlankLocation(new Vector3(381.8676f, 1442.678f, 14.71403f), 321.6855f, "ALGSpeedTrap3", "Covering Union Drive East") {
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
            new BlankLocation(new Vector3(455.6155f, 826.2332f, 8.60358f), 3.34502f, "ALGSpeedTrap4", "Covering Union Drive East") {
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
            new BlankLocation(new Vector3(403.1945f, 1277.182f, 7.644617f), 359.9992f, "ALGSpeedTrap5", "Union Drive East Tunnel") {
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
            new BlankLocation(new Vector3(4.244688f, 807.3133f, 14.34105f), 267.0054f, "ALGSpeedTrap6", "Star Junction Alley") {
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
            new BlankLocation(new Vector3(-233.4547f, 720.8707f, 9.467761f), 269.5866f, "ALGSpeedTrap7", "Westminster Carpark") {
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
            new BlankLocation(new Vector3(-71.00031f, 1683.867f, 14.61431f), 179.4064f, "ALGSpeedTrap8", "Middle Park - Topaz Street Alley") {
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
            new BlankLocation(new Vector3(14.5392f, 1080.148f, 14.33973f), 270.0316f, "ALGSpeedTrap9", "Star Junction Alley across from The Majestic") {
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
            //Bohan
            new BlankLocation(new Vector3(961.422f, 2301.586f, 39.44052f), 314.965f, "BOHSpeedTrap1", "Valdez St. overlooking Grand Boulevard") {
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
                            new LEConditionalLocation(new Vector3(959.1589f, 2302.295f, 38.91455f), 64.47408f, 0f) {
                                AssociationID = "",
                                RequiredVehicleGroup = "Motorcycle", },
                        },
                        PossiblePedSpawns = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(961.422f, 2301.586f, 39.44052f), 314.965f, 0f) {
                                AssociationID = "",
                                RequiredPedGroup = "MotorcycleCop",
                                TaskRequirements = TaskRequirements.Guard,
                                ForcedScenarios = new List<string>(){ "WORLD_HUMAN_BINOCULARS" } },
                        }
                    },

                }
            },
            new BlankLocation(new Vector3(1112.055f, 2152.94f, 16.66066f), 227.3036f, "BOHSpeedTrap2", "Across from Alpha Mail - Industrial") {
                ActivateDistance = 300f,
                ActivateCells = 8,
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(1112.055f, 2152.94f, 16.66066f), 227.3036f, defaultSpawnPercentage) {
                        IsEmpty = false,
                        TaskRequirements = TaskRequirements.Guard,
                        OverrideNightPercentage = 55.0f,
                        OverridePoorWeatherPercentage = 0.0f },
                },
            },
            //Dukes
            new BlankLocation(new Vector3(2122.215f, 1422.762f, 23.96833f), 231.4356f, "DUKSpeedTrap1", "Dukes Expressway") {
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
                            new LEConditionalLocation(new Vector3(2119.823f, 1423.51f, 23.47022f), 75.3797f, 0f) {
                                AssociationID = "",
                                RequiredVehicleGroup = "Motorcycle", },
                        },
                        PossiblePedSpawns = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(2122.215f, 1422.762f, 23.96833f), 231.4356f, 0f) {
                                AssociationID = "",
                                RequiredPedGroup = "MotorcycleCop",
                                TaskRequirements = TaskRequirements.Guard,
                                ForcedScenarios = new List<string>(){ "WORLD_HUMAN_BINOCULARS" } },
                        }
                    },

                }
            },
            new BlankLocation(new Vector3(2210.146f, 639.4536f, 18.24857f), 297.3787f, "DUKSpeedTrap2", "Dukes Expressway Alt Direction") {
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
                            new LEConditionalLocation(new Vector3(2208.009f, 638.3229f, 17.71871f), 131.1609f, 0f) {
                                AssociationID = "",
                                RequiredVehicleGroup = "Motorcycle", },
                        },
                        PossiblePedSpawns = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(2210.146f, 639.4536f, 18.24857f), 297.3787f, 0f) {
                                AssociationID = "",
                                RequiredPedGroup = "MotorcycleCop",
                                TaskRequirements = TaskRequirements.Guard,
                                ForcedScenarios = new List<string>(){ "WORLD_HUMAN_BINOCULARS" } },
                        }
                    },

                }
            },
            new BlankLocation(new Vector3(2001.516f, 1035.316f, 28.79944f), 43.6178f, "DUKSpeedTrap3", "Howard St Willis") {
                ActivateDistance = 300f,
                ActivateCells = 8,
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(2001.516f, 1035.316f, 28.79944f), 43.6178f, defaultSpawnPercentage) {
                        IsEmpty = false,
                        TaskRequirements = TaskRequirements.Guard,
                        OverrideNightPercentage = 55.0f,
                        OverridePoorWeatherPercentage = 0.0f },
                },
            },
            //Broker
            new BlankLocation(new Vector3(1751.912f, 192.218f, 14.47479f), 37.95805f, "BROSpeedTrap1", "Dukes Expressway Broker") {
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
                            new LEConditionalLocation(new Vector3(1751.889f, 194.048f, 13.95154f), 46.04592f, 0f) {
                                AssociationID = "",
                                RequiredVehicleGroup = "Motorcycle", },
                        },
                        PossiblePedSpawns = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(1751.912f, 192.218f, 14.47479f), 37.95805f, 0f) {
                                AssociationID = "",
                                RequiredPedGroup = "MotorcycleCop",
                                TaskRequirements = TaskRequirements.Guard,
                                ForcedScenarios = new List<string>(){ "WORLD_HUMAN_BINOCULARS" } },
                        }
                    },

                }
            },
            new BlankLocation(new Vector3(1357.605f, 760.4212f, 30.61892f), 89.78445f, "BROSpeedTrap2", "Downtown Broker") {
                ActivateDistance = 300f,
                ActivateCells = 8,
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(1357.605f, 760.4212f, 30.61892f), 89.78445f, defaultSpawnPercentage) {
                        IsEmpty = false,
                        TaskRequirements = TaskRequirements.Guard,
                        OverrideNightPercentage = 55.0f,
                        OverridePoorWeatherPercentage = 0.0f },
                },
            },
            new BlankLocation(new Vector3(1418.321f, 335.1789f, 28.47354f), 359.7886f, "BROSpeedTrap3", "South Slopes Broker") {
                ActivateDistance = 300f,
                ActivateCells = 8,
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(1418.321f, 335.1789f, 28.47354f), 359.7886f, defaultSpawnPercentage) {
                        IsEmpty = false,
                        TaskRequirements = TaskRequirements.Guard,
                        OverrideNightPercentage = 55.0f,
                        OverridePoorWeatherPercentage = 0.0f },
                },
            },
            //Alderney
            new BlankLocation(new Vector3(-1499.85f, 1209.925f, 29.48732f), 162.3214f, "ALDSpeedTrap1", "Plumbers Skyway Berchem") {
                ActivateDistance = 300f,ActivateCells = 8,
                StateID = StaticStrings.AlderneyStateID,
                PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup(){
                        Percentage = defaultSpawnPercentage,
                        OverrideNightPercentage = 0.0f,
                        OverridePoorWeatherPercentage = 0.0f,
                        PossibleVehicleSpawns  = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(-1500.009f, 1212.669f, 28.26634f), 326.2217f, 0f) {
                                AssociationID = "",
                                RequiredVehicleGroup = "Motorcycle", },
                        },
                        PossiblePedSpawns = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(-1499.85f, 1209.925f, 29.48732f), 162.3214f, 0f) {
                                AssociationID = "",
                                RequiredPedGroup = "MotorcycleCop",
                                TaskRequirements = TaskRequirements.Guard,
                                ForcedScenarios = new List<string>(){ "WORLD_HUMAN_BINOCULARS" } },
                        }
                    },

                }
            },
            new BlankLocation(new Vector3(-1025.908f, 1314.484f, 19.16953f), 267.0769f, "ALDSpeedTrap2", "Plumber's Skyway Alderney City") {
                ActivateDistance = 300f,
                ActivateCells = 8,
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(-1025.908f, 1314.484f, 19.16953f), 267.0769f, defaultSpawnPercentage) {
                        IsEmpty = false,
                        TaskRequirements = TaskRequirements.Guard,
                        OverrideNightPercentage = 55.0f,
                        OverridePoorWeatherPercentage = 0.0f },
                },
            },
            new BlankLocation(new Vector3(-1063.718f, 686.8278f, 7.652407f), 271.3782f, "ALDSpeedTrap3", "Roebuck Rd Tudor") {
                ActivateDistance = 300f,
                ActivateCells = 8,
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(-1063.718f, 686.8278f, 7.652407f), 271.3782f, defaultSpawnPercentage) {
                        IsEmpty = false,
                        TaskRequirements = TaskRequirements.Guard,
                        OverrideNightPercentage = 55.0f,
                        OverridePoorWeatherPercentage = 0.0f },
                },
            },
            //Colony island
            new BlankLocation(new Vector3(685.7279f, 654.6477f, 8.170586f), 89.80821f, "COLSpeedTrap1", "Colony Island") {
                ActivateDistance = 300f,
                ActivateCells = 8,
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(685.7279f, 654.6477f, 8.170586f), 89.80821f, defaultSpawnPercentage) {
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
        //Algonquin
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
                                        Location = new Vector3(123.7763f, 474.6643f, 14.87159f),
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
                                        Location = new Vector3(123.4458f, 475.8339f, 14.87159f),
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
        BlankLocation MiddleParkCops = new BlankLocation()
        {
            Name = "MiddleParkCops",
            FullName = "MiddleParkCops",
            Description = "2 Cops inside Middle Park",
            MapIcon = 162,
            EntrancePosition = new Vector3(114.7161f, 1519.462f, 6.159339f),
            EntranceHeading = 242.5591f,
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
                                        Location = new Vector3(114.7161f, 1519.462f, 6.159339f),
                                            Heading = 242.5591f,
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
                                        Location = new Vector3(115.1586f, 1520.479f, 6.159339f),
                                            Heading = 210.4014f,
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
        BlankLocationPlaces.Add(MiddleParkCops);
        //Alderney
        BlankLocation LeftwoodShopCop = new BlankLocation()
        {
            Name = "LeftwoodShopCop",
            FullName = "LeftwoodShopCop",
            Description = "1 Cop at Checkout store in Westdyke",
            MapIcon = 162,
            EntrancePosition = new Vector3(-1241.763f, 1897.985f, 13.16969f),
            EntranceHeading = 332.6969f,
            OpenTime = 18,
            CloseTime = 22,
            StateID = StaticStrings.AlderneyStateID,
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
                                        Location = new Vector3(-1241.763f, 1897.985f, 13.16969f),
                                            Heading = 332.6969f,
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
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                    Location = new Vector3(-1241.579f, 1902.202f, 12.65748f),
                                        Heading = 170.9447f,
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
        BlankLocationPlaces.Add(LeftwoodShopCop);
        BlankLocation AlderneyCityCops = new BlankLocation()
        {
            Name = "AlderneyCityCops",
            FullName = "AlderneyCityCops",
            Description = "2 Cops on Lockowski Ave",
            MapIcon = 162,
            EntrancePosition = new Vector3(-1212.328f, 1489.356f, 23.03569f),
            EntranceHeading = 180.8803f,
            OpenTime = 12,
            CloseTime = 18,
            StateID = StaticStrings.AlderneyStateID,
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
                                        Location = new Vector3(-1212.328f, 1489.356f, 23.03569f),
                                            Heading = 180.8803f,
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
                                        Location = new Vector3(-1213.313f, 1489.394f, 23.03569f),
                                            Heading = 176.989f,
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
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                    Location = new Vector3(-1219.51f, 1488.303f, 22.57185f),
                                        Heading = 359.0395f,
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
        BlankLocationPlaces.Add(AlderneyCityCops);
        BlankLocation AlderneyCityWaterCops = new BlankLocation()
        {
            Name = "AlderneyCityWaterCops",
            FullName = "AlderneyCityWaterCops",
            Description = "2 Cops on Aderney City Waterfront",
            MapIcon = 162,
            EntrancePosition = new Vector3(-582.9671f, 1493.61f, 15.62755f),
            EntranceHeading = 358.374f,
            OpenTime = 8,
            CloseTime = 16,
            StateID = StaticStrings.AlderneyStateID,
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
                                        Location = new Vector3(-582.9671f, 1493.61f, 15.62755f),
                                            Heading = 358.374f,
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
                                        Location = new Vector3(-581.9308f, 1493.631f, 15.62754f),
                                            Heading = 5.169923f,
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
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(AlderneyCityWaterCops);
        BlankLocation WestdykBreakinCops = new BlankLocation()
        {
            Name = "WestdykBreakinCops",
            FullName = "WestdykBreakinCops",
            Description = "2 Cops inspecting chopshop",
            MapIcon = 162,
            EntrancePosition = new Vector3(-952.6885f, 2358.356f, 6.619922f),
            EntranceHeading = 60.96603f,
            OpenTime = 22,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
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
                                        Location = new Vector3(-952.6885f, 2358.356f, 6.619922f),
                                            Heading = 60.96603f,
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
                                                "WORLD_HUMAN_INSPECT_STAND",
                                                "WORLD_HUMAN_INSPECT_CROUCH",
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
                                        Location = new Vector3(-951.4698f, 2355.363f, 6.478187f),
                                            Heading = 54.48252f,
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
                                                "WORLD_HUMAN_INSPECT_STAND",
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
                                    Location = new Vector3(-947.236f, 2356.307f, 6.086555f),
                                        Heading = 75.64349f,
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
        BlankLocationPlaces.Add(WestdykBreakinCops);
        BlankLocation TudorInspectCops = new BlankLocation()
        {
            Name = "TudorInspectCops",
            FullName = "TudorInspectCops",
            Description = "2 Cops inspecting an abandoned home",
            MapIcon = 162,
            EntrancePosition = new Vector3(-1711.371f, 412.3018f, 6.677242f),
            EntranceHeading = 357.9126f,
            OpenTime = 8,
            CloseTime = 12,
            StateID = StaticStrings.AlderneyStateID,
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
                                        Location = new Vector3(-1711.371f, 412.3018f, 6.677242f),
                                            Heading = 357.9126f,
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
                                                "WORLD_HUMAN_INSPECT_STAND",
                                                "WORLD_HUMAN_INSPECT_CROUCH",
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
                                        Location = new Vector3(-1712.295f, 410.8985f, 6.717053f),
                                            Heading = 172.6562f,
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
                                    Location = new Vector3(-1710.805f, 407.3419f, 6.269197f),
                                        Heading = 93.25555f,
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
        BlankLocationPlaces.Add(TudorInspectCops);
        BlankLocation BerchemHomeCops = new BlankLocation()
        {
            Name = "BerchemHomeCops",
            FullName = "BerchemHomeCops",
            Description = "2 Cops home visit",
            MapIcon = 162,
            EntrancePosition = new Vector3(-1411.97f, 1104.094f, 29.80904f),
            EntranceHeading = 276.2928f,
            OpenTime = 17,
            CloseTime = 21,
            StateID = StaticStrings.AlderneyStateID,
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
                                        Location = new Vector3(-1411.97f, 1104.094f, 29.80904f),
                                            Heading = 276.2928f,
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
                                    new LEConditionalLocation()
                                    {
                                        Location = new Vector3(-1414.009f, 1103.35f, 29.80904f),
                                            Heading = 111.8419f,
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
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                    Location = new Vector3(-1421.097f, 1101.382f, 28.0374f),
                                        Heading = 13.25766f,
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
        BlankLocationPlaces.Add(BerchemHomeCops);
        //Bohan
        BlankLocation BohanBurgerCop = new BlankLocation()
        {
            Name = "BohanBurgerCop",
            FullName = "BohanBurgerCop",
            Description = "1 Cop at Burger Shot in Fortside",
            MapIcon = 162,
            EntrancePosition = new Vector3(680.7929f, 2013.395f, 16.33562f),
            EntranceHeading = 359.6194f,
            OpenTime = 12,
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
                                        Location = new Vector3(680.7929f, 2013.395f, 16.33562f),
                                            Heading = 359.6194f,
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
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                    Location = new Vector3(680.6405f, 2019.043f, 15.72929f),
                                        Heading = 269.4313f,
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
        BlankLocationPlaces.Add(BohanBurgerCop);
        BlankLocation NGardensCops = new BlankLocation()
        {
            Name = "NGardensCops",
            FullName = "NGardensCops",
            Description = "2 Cops in Northern Gardens",
            MapIcon = 162,
            EntrancePosition = new Vector3(1271.173f, 2333.981f, 13.68308f),
            EntranceHeading = 251.5695f,
            OpenTime = 10,
            CloseTime = 14,
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
                                        Location = new Vector3(1271.173f, 2333.981f, 13.68308f),
                                            Heading = 251.5695f,
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
                                        Location = new Vector3(1271.185f, 2332.87f, 13.68308f),
                                            Heading = 292.7367f,
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
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                    Location = new Vector3(1296.78f, 2324.377f, 12.24314f),
                                        Heading = 338.8631f,
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
        BlankLocationPlaces.Add(NGardensCops);
        //Broker
        BlankLocation DowntownCops = new BlankLocation()
        {
            Name = "DowntownCops",
            FullName = "DowntownCops",
            Description = "2 Cops outside Munsee Building West Downtown",
            MapIcon = 162,
            EntrancePosition = new Vector3(1256.224f, 671.6829f, 33.55271f),
            EntranceHeading = 238.3846f,
            OpenTime = 8,
            CloseTime = 14,
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
                                        Location = new Vector3(1256.224f, 671.6829f, 33.55271f),
                                            Heading = 238.3846f,
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
                                                "WORLD_HUMAN_COP_IDLES",
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
                                    new LEConditionalLocation()
                                    {
                                        Location = new Vector3(1256.658f, 672.5675f, 33.55272f),
                                            Heading = 236.3424f,
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
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                    Location = new Vector3(1265.224f, 670.5244f, 31.47508f),
                                        Heading = 142.3437f,
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
        BlankLocationPlaces.Add(DowntownCops);
        BlankLocation RotterdamCops = new BlankLocation()
        {
            Name = "RotterdamCops",
            FullName = "RotterdamCops",
            Description = "2 Cops in Rotterdam Hill Broker",
            MapIcon = 162,
            EntrancePosition = new Vector3(1076.357f, 800.3979f, 5.562184f),
            EntranceHeading = 268.3151f,
            OpenTime = 18,
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
                                        Location = new Vector3(1076.357f, 800.3979f, 5.562184f),
                                            Heading = 268.3151f,
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
                                                "WORLD_HUMAN_INSPECT_CROUCH",
                                                "WORLD_HUMAN_INSPECT_STAND",
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
                                        Location = new Vector3(1074.608f, 799.8823f, 5.618639f),
                                            Heading = 85.91793f,
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
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                    Location = new Vector3(1069.959f, 795.0145f, 5.500751f),
                                        Heading = 356.8531f,
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
        BlankLocationPlaces.Add(RotterdamCops);
        BlankLocation RotterdamCops2 = new BlankLocation()
        {
            Name = "RotterdamCops2",
            FullName = "RotterdamCops2",
            Description = "2 Cops Home inspection Rotterdam Hill Broker",
            MapIcon = 162,
            EntrancePosition = new Vector3(1119.325f, 462.6194f, 28.4229f),
            EntranceHeading = 333.9765f,
            OpenTime = 19,
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
                                        Location = new Vector3(1119.325f, 462.6194f, 28.4229f),
                                            Heading = 333.9765f,
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
                                    new LEConditionalLocation()
                                    {
                                        Location = new Vector3(1117.968f, 461.9398f, 28.4229f),
                                            Heading = 171.7107f,
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
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                    Location = new Vector3(1104.159f, 457.9587f, 25.76905f),
                                        Heading = 41.5046f,
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
        BlankLocationPlaces.Add(RotterdamCops2);
        BlankLocation FireFlyProjectCops = new BlankLocation()
        {
            Name = "FireFlyProjectCops",
            FullName = "FireFlyProjectCops",
            Description = "2 Cops in FireFly Projects Broker",
            MapIcon = 162,
            EntrancePosition = new Vector3(1476.007f, 58.34098f, 16.65584f),
            EntranceHeading = 87.22088f,
            OpenTime = 12,
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
                                        Location = new Vector3(1476.007f, 58.34098f, 16.65584f),
                                            Heading = 87.22088f,
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
                                                "WORLD_HUMAN_INSPECT_CROUCH",
                                                "WORLD_HUMAN_INSPECT_STAND",
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
                                        Location = new Vector3(1477.575f, 57.42613f, 16.65584f),
                                            Heading = 229.6176f,
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
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                    Location = new Vector3(1473.244f, 33.20957f, 13.95908f),
                                        Heading = 270.8412f,
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
        BlankLocationPlaces.Add(FireFlyProjectCops);
        BlankLocation FireFlyIslandCops = new BlankLocation()
        {
            Name = "FireFlyIslandCops",
            FullName = "FireFlyIslandCops",
            Description = "2 Cops in FireFly Island Broker",
            MapIcon = 162,
            EntrancePosition = new Vector3(1330.144f, -161.7497f, 13.41232f),
            EntranceHeading = 10.67722f,
            OpenTime = 18,
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
                                        Location = new Vector3(1330.144f, -161.7497f, 13.41232f),
                                            Heading = 10.67722f,
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
                                                "WORLD_HUMAN_COP_IDLES",
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
                                    new LEConditionalLocation()
                                    {
                                        Location = new Vector3(1328.95f, -161.8607f, 13.41232f),
                                            Heading = 359.4761f,
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
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                    Location = new Vector3(1328.072f, -156.5112f, 12.89903f),
                                        Heading = 297.7558f,
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
        BlankLocationPlaces.Add(FireFlyIslandCops);
        BlankLocation HoveBeachCops = new BlankLocation()
        {
            Name = "HoveBeachCops",
            FullName = "HoveBeachCops",
            Description = "2 Cops in Hove Beach W Broker",
            MapIcon = 162,
            EntrancePosition = new Vector3(975.2064f, 11.78891f, 5.978969f),
            EntranceHeading = 93.22203f,
            OpenTime = 10,
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
                                        Location = new Vector3(975.2064f, 11.78891f, 5.978969f),
                                            Heading = 93.22203f,
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
                                        Location = new Vector3(975.2388f, 12.79555f, 5.982194f),
                                            Heading = 91.31911f,
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
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                    Location = new Vector3(985.6517f, 10.22909f, 5.486364f),
                                        Heading = 177.6516f,
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
        BlankLocationPlaces.Add(HoveBeachCops);
        //Dukes
        BlankLocation GantryParkCops = new BlankLocation()
        {
            Name = "GantryParkCops",
            FullName = "GantryParkCops",
            Description = "2 Cops in Ganty Park Steinway",
            MapIcon = 162,
            EntrancePosition = new Vector3(1466f, 1517.462f, 13.63293f),
            EntranceHeading = 5.859095f,
            OpenTime = 12,
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
                                        Location = new Vector3(1466f, 1517.462f, 13.63293f),
                                            Heading = 5.859095f,
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
                                        Location = new Vector3(1467.38f, 1517.43f, 13.63298f),
                                            Heading = 359.4882f,
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
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                    Location = new Vector3(1468.257f, 1509.022f, 13.10516f),
                                        Heading = 359.9984f,
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
        BlankLocationPlaces.Add(GantryParkCops);
        BlankLocation EllikonDeliCop = new BlankLocation()
        {
            Name = "EllikonDeliCop",
            FullName = "EllikonDeliCop",
            Description = "1 Cop at Ellikon Pantopoleon Deli Steinway",
            MapIcon = 162,
            EntrancePosition = new Vector3(1360.039f, 1173.411f, 37.44279f),
            EntranceHeading = 43.61218f,
            OpenTime = 8,
            CloseTime = 12,
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
                                        Location = new Vector3(1360.039f, 1173.411f, 37.44279f),
                                            Heading = 43.61218f,
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
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                    Location = new Vector3(1355.208f, 1172.757f, 37.04047f),
                                        Heading = 354.9214f,
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
        BlankLocationPlaces.Add(EllikonDeliCop);
        BlankLocation PizzaFeastCop = new BlankLocation()
        {
            Name = "PizzaFeastCop",
            FullName = "PizzaFeastCop",
            Description = "1 Cop at Pizza Feast Willis",
            MapIcon = 162,
            EntrancePosition = new Vector3(2036.464f, 977.9933f, 28.16457f),
            EntranceHeading = 279.9722f,
            OpenTime = 18,
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
                                        Location = new Vector3(2036.464f, 977.9933f, 28.16457f),
                                            Heading = 279.9722f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_UPRIGHT",
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
                                    Location = new Vector3(2041.486f, 983.0346f, 27.73219f),
                                        Heading = 171.6885f,
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
        BlankLocationPlaces.Add(PizzaFeastCop);
        // Colony Island
        BlankLocation ColonyIslandCops = new BlankLocation()
        {
            Name = "ColonyIslandCops",
            FullName = "ColonyIslandCops",
            Description = "2 Cops on Colony Island waterfront",
            MapIcon = 162,
            EntrancePosition = new Vector3(621.8672f, 612.5256f, 8.461625f),
            EntranceHeading = 89.96902f,
            OpenTime = 10,
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
                                        Location = new Vector3(621.8672f, 612.5256f, 8.461625f),
                                            Heading = 89.96902f,
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
                                        Location = new Vector3(622.0223f, 614.2097f, 8.461472f),
                                            Heading = 89.24456f,
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
        BlankLocationPlaces.Add(ColonyIslandCops);
        BlankLocation ColonyIslandCops2 = new BlankLocation()
        {
            Name = "ColonyIslandCops2",
            FullName = "ColonyIslandCops2",
            Description = "2 Cops at Graveyard on Colony Island",
            MapIcon = 162,
            EntrancePosition = new Vector3(708.3907f, 798.3633f, 8.562534f),
            EntranceHeading = 4.00087f,
            OpenTime = 18,
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
                                        Location = new Vector3(708.3907f, 798.3633f, 8.562534f),
                                            Heading = 4.00087f,
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
                                                "WORLD_HUMAN_COP_IDLES",
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
                                    new LEConditionalLocation()
                                    {
                                        Location = new Vector3(706.341f, 798.0322f, 8.561304f),
                                            Heading = 344.5756f,
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
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                    Location = new Vector3(706.9757f, 795.6805f, 8.186477f),
                                        Heading = 64.12108f,
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
        BlankLocationPlaces.Add(ColonyIslandCops2);
    }
}

