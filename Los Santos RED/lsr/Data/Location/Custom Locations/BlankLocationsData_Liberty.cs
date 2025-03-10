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
        AncelottinGang();
        GambettiGang();
        LupisellaGang();
        MessinaGang();
        PavanoGang();
        KhangpaeGang();
        TriadGang();
        PetrovicGang();
        SpanishLordsGang();
        UptownGang();
        HolHustGang();
        AodGang();
        LostGang();
        YardiesGang();

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
                            MaxWantedLevelSpawn = 4,
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
                                            MaxWantedLevelSpawn = 4,




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
                                            MaxWantedLevelSpawn = 4,




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
                                        MaxWantedLevelSpawn = 4,
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
                            MaxWantedLevelSpawn = 4,
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
                                            MaxWantedLevelSpawn = 4,




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
                                            MaxWantedLevelSpawn = 4,




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
                            MaxWantedLevelSpawn = 4,
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
                                            MaxWantedLevelSpawn = 4,




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
                                            MaxWantedLevelSpawn = 4,




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
                            MaxWantedLevelSpawn = 4,
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
                                            MaxWantedLevelSpawn = 4,




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
                                            MaxWantedLevelSpawn = 4,




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
                            MaxWantedLevelSpawn = 4,
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
                                            MaxWantedLevelSpawn = 4,




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
                                            MaxWantedLevelSpawn = 4,




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
                                        MaxWantedLevelSpawn = 4,
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
                            MaxWantedLevelSpawn = 4,
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
                                            MaxWantedLevelSpawn = 4,




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
                                            MaxWantedLevelSpawn = 4,




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
                            MaxWantedLevelSpawn = 4,
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
                                            MaxWantedLevelSpawn = 4,




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
                                            MaxWantedLevelSpawn = 4,




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
                            MaxWantedLevelSpawn = 4,
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
                                            MaxWantedLevelSpawn = 4,




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
                                            MaxWantedLevelSpawn = 4,




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
                            MaxWantedLevelSpawn = 4,
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
                                            MaxWantedLevelSpawn = 4,




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
                                            MaxWantedLevelSpawn = 4,




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
                                        MaxWantedLevelSpawn = 4,
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
                            MaxWantedLevelSpawn = 4,
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
                                            MaxWantedLevelSpawn = 4,




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
                                            MaxWantedLevelSpawn = 4,




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
                            MaxWantedLevelSpawn = 4,
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
                                            MaxWantedLevelSpawn = 4,




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
                                            MaxWantedLevelSpawn = 4,




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
                            MaxWantedLevelSpawn = 4,
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
                                            MaxWantedLevelSpawn = 4,




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
                                        MaxWantedLevelSpawn = 4,
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
                            MaxWantedLevelSpawn = 4,
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
                                            MaxWantedLevelSpawn = 4,




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
                                            MaxWantedLevelSpawn = 4,




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
                                        MaxWantedLevelSpawn = 4,
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
                            MaxWantedLevelSpawn = 4,
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
                                            MaxWantedLevelSpawn = 4,




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
                                            MaxWantedLevelSpawn = 4,




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
                            MaxWantedLevelSpawn = 4,
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
                                            MaxWantedLevelSpawn = 4,




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
                                            MaxWantedLevelSpawn = 4,




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
                                        MaxWantedLevelSpawn = 4,
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
                            MaxWantedLevelSpawn = 4,
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
                                            MaxWantedLevelSpawn = 4,




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
                                            MaxWantedLevelSpawn = 4,




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
                                        MaxWantedLevelSpawn = 4,
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
                            MaxWantedLevelSpawn = 4,
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
                                            MaxWantedLevelSpawn = 4,




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
                                            MaxWantedLevelSpawn = 4,




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
                                        MaxWantedLevelSpawn = 4,
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
                            MaxWantedLevelSpawn = 4,
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
                                            MaxWantedLevelSpawn = 4,




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
                                        MaxWantedLevelSpawn = 4,
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
                            MaxWantedLevelSpawn = 4,
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
                                            MaxWantedLevelSpawn = 4,




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
                                            MaxWantedLevelSpawn = 4,




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
                                        MaxWantedLevelSpawn = 4,
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
                            MaxWantedLevelSpawn = 4,
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
                                            MaxWantedLevelSpawn = 4,




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
                                            MaxWantedLevelSpawn = 4,




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
                                        MaxWantedLevelSpawn = 4,
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
                            MaxWantedLevelSpawn = 4,
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
                                            MaxWantedLevelSpawn = 4,




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
                                            MaxWantedLevelSpawn = 4,




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
                                        MaxWantedLevelSpawn = 4,
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
                            MaxWantedLevelSpawn = 4,
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
                                            MaxWantedLevelSpawn = 4,




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
                                            MaxWantedLevelSpawn = 4,




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
                                        MaxWantedLevelSpawn = 4,
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
                            MaxWantedLevelSpawn = 4,
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
                                            MaxWantedLevelSpawn = 4,




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
                                            MaxWantedLevelSpawn = 4,




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
                                        MaxWantedLevelSpawn = 4,
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
                            MaxWantedLevelSpawn = 4,
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
                                            MaxWantedLevelSpawn = 4,




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
                                            MaxWantedLevelSpawn = 4,




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
                                        MaxWantedLevelSpawn = 4,
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
                            MaxWantedLevelSpawn = 4,
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
                                            MaxWantedLevelSpawn = 4,




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
                                            MaxWantedLevelSpawn = 4,




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
                                        MaxWantedLevelSpawn = 4,
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
                            MaxWantedLevelSpawn = 4,
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
                                            MaxWantedLevelSpawn = 4,




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
                                            MaxWantedLevelSpawn = 4,




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
                                        MaxWantedLevelSpawn = 4,
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
                            MaxWantedLevelSpawn = 4,
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
                                            MaxWantedLevelSpawn = 4,




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
                                        MaxWantedLevelSpawn = 4,
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
                            MaxWantedLevelSpawn = 4,
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
                                            MaxWantedLevelSpawn = 4,




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
                                        MaxWantedLevelSpawn = 4,
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
                            MaxWantedLevelSpawn = 4,
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
                                            MaxWantedLevelSpawn = 4,




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
                                            MaxWantedLevelSpawn = 4,




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
                            MaxWantedLevelSpawn = 4,
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
                                            MaxWantedLevelSpawn = 4,




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
                                            MaxWantedLevelSpawn = 4,




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
                                        MaxWantedLevelSpawn = 4,
                                        LongGunAlwaysEquipped = false,


                                        ForceLongGun = false,
                                },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(ColonyIslandCops2);
    }


    private void AncelottinGang()
    {
        BlankLocation AncelottiHonkers = new BlankLocation()
        {

            Name = "AncelottiHonkers",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(-1354.888f, 510.0261f, 9.363672f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            InteriorID = 67586,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 10,
                            MaxHourSpawn = 24,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1342.407f, 518.8824f, 10.01684f),
                                            Heading = 72.11568f,
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
                                                "WORLD_HUMAN_GUARD_STAND_CLUBHOUSE",
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
                                        Location = new Vector3(-1342.54f, 521.6884f, 10.01822f),
                                            Heading = 100.7823f,
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
                                                "WORLD_HUMAN_GUARD_STAND_CLUBHOUSE",
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
                                        Location = new Vector3(-1335.88f, 516.9401f, 10.01021f),
                                            Heading = 91.55523f,
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
                                                "WORLD_HUMAN_GUARD_STAND_FACILITY",
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
                                        Location = new Vector3(-1344.131f, 498.4292f, 10.01021f),
                                            Heading = 1.412311f,
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
                                                "WORLD_HUMAN_GUARD_STAND_FACILITY",
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
                                        Location = new Vector3(-1354.888f, 510.0261f, 9.363672f),
                                            Heading = 181.2512f,
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
        }; //interior
        BlankLocationPlaces.Add(AncelottiHonkers);
        BlankLocation AncelottiHouse = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 12,
                            MaxHourSpawn = 22,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1281.088f, 748.9422f, 14.02908f),
                                            Heading = 261.0979f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1281.218f, 747.1935f, 14.02908f),
                                            Heading = 279.7828f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-1274.195f, 751.9827f, 12.00912f),
                                        Heading = 270.27f,
                                },
                            },
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_ANCELOTTI",
            Name = "AncelottiHouse",
            Description = "",
            EntrancePosition = new Vector3(-1274.195f, 751.9827f, 12.00912f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(AncelottiHouse);
        BlankLocation AncelottiPizzaThis = new BlankLocation()
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
                                        Location = new Vector3(-1318.894f, 1000.006f, 24.80426f),
                                            Heading = 358.1175f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1319.964f, 1000.068f, 24.90013f),
                                            Heading = 0.9251531f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-1319.616f, 1005.551f, 24.27114f),
                                        Heading = 270.1079f,
                                },
                            },
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_ANCELOTTI",
            Name = "AncelottiPizzaThis",
            Description = "",
            EntrancePosition = new Vector3(-1319.616f, 1005.551f, 24.27114f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(AncelottiPizzaThis);
        BlankLocation AncelottiHouse2 = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 12,
                            MaxHourSpawn = 22,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1279.93f, 1238.773f, 25.75601f),
                                            Heading = 246.2866f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1279.457f, 1239.617f, 25.75601f),
                                            Heading = 234.0314f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-1272.842f, 1234.057f, 24.90157f),
                                        Heading = 155.8833f,
                                },
                            },
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_ANCELOTTI",
            Name = "AncelottiHouse2",
            Description = "",
            EntrancePosition = new Vector3(-1272.842f, 1234.057f, 24.90157f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(AncelottiHouse2);
        BlankLocation AncelottiCleaners = new BlankLocation()
        {

            Name = "AncelottiCleaners",
            Description = "",
            EntrancePosition = new Vector3(-1449.496f, 1132.442f, 29.46717f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 12,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1449.496f, 1132.442f, 29.46717f),
                                            Heading = 284.3813f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(-1449.199f, 1131.275f, 29.46699f),
                                            Heading = 302.83f,
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
                            {},
                    },
                },
        };
        BlankLocationPlaces.Add(AncelottiCleaners);
        BlankLocation AncelottiGarages = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 12,
                            MaxHourSpawn = 22,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1215.986f, 1264.374f, 23.54687f),
                                            Heading = 62.88677f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1217.762f, 1265.266f, 23.54788f),
                                            Heading = 241.0338f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-1215.377f, 1267.308f, 22.89254f),
                                        Heading = 63.58024f,
                                },
                            },
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_ANCELOTTI",
            Name = "AncelottiGarages",
            Description = "",
            EntrancePosition = new Vector3(-1215.377f, 1267.308f, 22.89254f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(AncelottiGarages);
    }

    private void GambettiGang()
    {
        BlankLocation GambettiMarioBY = new BlankLocation()
        {

            Name = "GambettiMarioBY",
            Description = "",
            EntrancePosition = new Vector3(72.48227f, 232.936f, 14.67881f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 12,
                            MaxHourSpawn = 22,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(72.48227f, 232.936f, 14.67881f),
                                            Heading = 91.11575f,
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
                                        Location = new Vector3(71.24015f, 232.9196f, 14.67348f),
                                            Heading = 268.7274f,
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
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
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
        BlankLocationPlaces.Add(GambettiMarioBY);
        BlankLocation GambettiStreet = new BlankLocation()
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
                                        Location = new Vector3(32.78203f, 236.5352f, 14.67114f),
                                            Heading = 1.491724f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(33.81213f, 236.4431f, 14.67529f),
                                            Heading = 358.7962f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(37.23869f, 235.6022f, 14.05649f),
                                        Heading = 180.3486f,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_GAMBETTI",
            Name = "GambettiStreet",
            Description = "",
            EntrancePosition = new Vector3(37.23869f, 235.6022f, 14.05649f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(GambettiStreet);
        BlankLocation GambettiAlDenti = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 16,
                            MaxHourSpawn = 20,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(170.3216f, -136.8503f, 14.76152f),
                                            Heading = 225.8048f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(169.2885f, -137.287f, 14.76122f),
                                            Heading = 216.6167f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(176.4462f, -137.3461f, 14.12011f),
                                        Heading = 181.1783f,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_GAMBETTI",
            Name = "GambettiAlDenti",
            Description = "",
            EntrancePosition = new Vector3(176.4462f, -137.3461f, 14.12011f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(GambettiAlDenti);
        BlankLocation GambettiSuffolk = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 10,
                            MaxHourSpawn = 16,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-38.57289f, 207.5464f, 14.62026f),
                                            Heading = 275.0419f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-38.61805f, 208.4624f, 14.61983f),
                                            Heading = 265.6258f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-31.29272f, 209.0046f, 13.68697f),
                                        Heading = 359.9147f,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_GAMBETTI",
            Name = "GambettiSuffolk",
            Description = "",
            EntrancePosition = new Vector3(-31.29272f, 209.0046f, 13.68697f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(GambettiSuffolk);
        BlankLocation GambettiCityHall = new BlankLocation()
        {

            Name = "GambettiCityHall",
            Description = "",
            EntrancePosition = new Vector3(128.0231f, 164.8131f, 15.20244f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MaxHourSpawn = 24,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(128.0231f, 164.8131f, 15.20244f),
                                            Heading = 177.4202f,
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
                                        Location = new Vector3(126.9162f, 164.7469f, 15.20244f),
                                            Heading = 181.4519f,
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
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
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
        BlankLocationPlaces.Add(GambettiCityHall);
        BlankLocation GambettiCityHall2 = new BlankLocation()
        {

            Name = "GambettiCityHall2",
            Description = "",
            EntrancePosition = new Vector3(19.05459f, 178.5555f, 14.76581f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 12,
                            MaxHourSpawn = 22,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(19.05459f, 178.5555f, 14.76581f),
                                            Heading = 1.074511f,
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
                                        Location = new Vector3(20.29286f, 178.4865f, 14.76468f),
                                            Heading = 359.1416f,
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
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
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
        BlankLocationPlaces.Add(GambettiCityHall2);
        BlankLocation GambettiFeldspar = new BlankLocation()
        {

            Name = "GambettiFeldspar",
            Description = "",
            EntrancePosition = new Vector3(-94.75925f, 158.3789f, 5.267494f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 14,
                            MaxHourSpawn = 22,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-94.75925f, 158.3789f, 5.267494f),
                                            Heading = 114.5125f,
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
                                        Location = new Vector3(-95.12589f, 159.1213f, 5.227471f),
                                            Heading = 113.219f,
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
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
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
        BlankLocationPlaces.Add(GambettiFeldspar);
        BlankLocation GambettiErsatz = new BlankLocation()
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
                                        Location = new Vector3(7.521808f, 288.3621f, 14.48354f),
                                            Heading = 92.85258f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(7.655271f, 287.4253f, 14.48458f),
                                            Heading = 93.03582f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(2.063804f, 289.296f, 13.83997f),
                                        Heading = 358.2276f,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_GAMBETTI",
            Name = "GambettiErsatz",
            Description = "",
            EntrancePosition = new Vector3(2.063804f, 289.296f, 13.83997f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(GambettiErsatz);

    }

    private void LupisellaGang()
    {
        BlankLocation LupisellaBlock = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 10,
                            MaxHourSpawn = 20,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1301.016f, 2347.162f, 20.08677f),
                                            Heading = 183.1393f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1302.027f, 2347.187f, 20.08677f),
                                            Heading = 177.5564f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(1306.208f, 2333.91f, 12.05332f),
                                        Heading = 114.3722f,
                                },
                            },
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_LUPISELLA",
            Name = "LupisellaBlock",
            Description = "",
            EntrancePosition = new Vector3(1306.208f, 2333.91f, 12.05332f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(LupisellaBlock);
        BlankLocation LupisellaCabin = new BlankLocation()
        {

            Name = "LupisellaCabin",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(1450.259f, 2045.227f, 16.22648f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 10,
                            MaxHourSpawn = 23,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 3,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1461.685f, 2038.687f, 16.79448f),
                                            Heading = 136.3462f,
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
                                                "WORLD_HUMAN_GUARD_STAND_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(1461.182f, 2041.537f, 16.86924f),
                                            Heading = 19.00734f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(1460.365f, 2044.947f, 16.86924f),
                                            Heading = 200.4964f,
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
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
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
                                        Location = new Vector3(1459.135f, 2043.909f, 16.86924f),
                                            Heading = 221.2324f,
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
                                                "WORLD_HUMAN_CLIPBOARD_FACILITY",
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
                                        Location = new Vector3(1450.259f, 2045.227f, 16.22648f),
                                            Heading = 47.14442f,
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
                                        Location = new Vector3(1456.749f, 2035.957f, 16.26124f),
                                            Heading = 3.279457f,
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
        BlankLocationPlaces.Add(LupisellaCabin);
        BlankLocation LupisellaTriangle = new BlankLocation()
        {

            Name = "LupisellaTriangle",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(1451.727f, 2211.518f, 15.95439f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 10,
                            MaxHourSpawn = 23,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 3,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1380.79f, 2163.813f, 16.71495f),
                                            Heading = 140.3605f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(1441.082f, 2207.431f, 16.66237f),
                                            Heading = 309.0697f,
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
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
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
                                        Location = new Vector3(1438.595f, 2209.937f, 16.66237f),
                                            Heading = 254.0794f,
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
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
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
                                        Location = new Vector3(1451.727f, 2211.518f, 15.95439f),
                                            Heading = 1.046725f,
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
                                            MinHourSpawn = 16,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,




                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1392.557f, 2157.893f, 16.25814f),
                                            Heading = 226.5761f,
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
        BlankLocationPlaces.Add(LupisellaTriangle);
        BlankLocation LupisellaTriangleInside = new BlankLocation()
        {

            Name = "LupisellaTriangleInside",
            Description = "",
            EntrancePosition = new Vector3(1435.173f, 2190.587f, 17.72179f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            InteriorID = 113666,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 10,
                            MaxHourSpawn = 24,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1435.173f, 2190.587f, 17.72179f),
                                            Heading = 222.4488f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(1409.312f, 2166.203f, 17.72196f),
                                            Heading = 227.2596f,
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
                                                "WORLD_HUMAN_GUARD_STAND_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
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
                                        Location = new Vector3(1409.312f, 2166.203f, 17.72196f),
                                            Heading = 227.2596f,
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
                                                "WORLD_HUMAN_GUARD_STAND_FACILITY",
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
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
                                        Location = new Vector3(1388.732f, 2167.961f, 16.72199f),
                                            Heading = 43.06167f,
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
                                                "WORLD_HUMAN_GUARD_STAND_ARMY",
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
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
        }; //interior
        BlankLocationPlaces.Add(LupisellaTriangleInside);
        BlankLocation LupisellaBlocks2 = new BlankLocation()
        {

            Name = "LupisellaBlocks2",
            Description = "",
            EntrancePosition = new Vector3(1407.586f, 2301.73f, 10.91479f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 12,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1407.586f, 2301.73f, 10.91479f),
                                            Heading = 268.5659f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(1408.97f, 2302.023f, 10.91478f),
                                            Heading = 94.06642f,
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
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
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
        BlankLocationPlaces.Add(LupisellaBlocks2);
        BlankLocation LupisellaFlanger = new BlankLocation()
        {

            Name = "LupisellaFlanger",
            Description = "",
            EntrancePosition = new Vector3(1507.623f, 2325.037f, 10.96453f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 12,
                            MaxHourSpawn = 23,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1507.623f, 2325.037f, 10.96453f),
                                            Heading = 271.7417f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(1507.65f, 2326.41f, 10.96453f),
                                            Heading = 267.8455f,
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
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
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
        BlankLocationPlaces.Add(LupisellaFlanger);
        BlankLocation LupisellaAlcatraz = new BlankLocation()
        {

            Name = "LupisellaAlcatraz",
            Description = "",
            EntrancePosition = new Vector3(1603.091f, 2123.269f, 17.86467f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 10,
                            MaxHourSpawn = 18,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1603.091f, 2123.269f, 17.86467f),
                                            Heading = 90.12063f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(1603.167f, 2121.548f, 17.86467f),
                                            Heading = 89.7944f,
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
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
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
        BlankLocationPlaces.Add(LupisellaAlcatraz);
        BlankLocation LupisellaBlock3 = new BlankLocation()
        {

            Name = "LupisellaBlock3",
            Description = "",
            EntrancePosition = new Vector3(1335.946f, 2254.304f, 10.70501f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 10,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1335.946f, 2254.304f, 10.70501f),
                                            Heading = 246.3346f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(1335.379f, 2253.342f, 10.70501f),
                                            Heading = 247.3597f,
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
                            {},
                    },
                },
        };
        BlankLocationPlaces.Add(LupisellaBlock3);
        BlankLocation LupisellaBodyDump = new BlankLocation()
        {

            Name = "LupisellaBodyDump",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(1598.176f, 2440.565f, 4.551487f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 20,
                            MaxHourSpawn = 24,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1597.045f, 2444.954f, 5.061442f),
                                            Heading = 325.5471f,
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
                                                "WORLD_HUMAN_GUARD_STAND_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(1595.85f, 2445.844f, 5.017694f),
                                            Heading = 323.5613f,
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
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1610.779f, 2469.378f, 3.190312f),
                                            Heading = 320.2166f,
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
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(1613.188f, 2471.349f, 2.813836f),
                                            Heading = 314.1112f,
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
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1598.176f, 2440.565f, 4.551487f),
                                            Heading = 338.9202f,
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
        BlankLocationPlaces.Add(LupisellaBodyDump);

    }
    private void MessinaGang()
    {
        BlankLocation MessinaSquare = new BlankLocation()
        {

            Name = "MessinaSquare",
            Description = "",
            EntrancePosition = new Vector3(83.73531f, 978.3295f, 14.71328f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 10,
                            MaxHourSpawn = 22,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(83.73531f, 978.3295f, 14.71328f),
                                            Heading = 267.7945f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
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
                                        Location = new Vector3(83.7447f, 977.4066f, 14.71324f),
                                            Heading = 268.5063f,
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
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
        BlankLocationPlaces.Add(MessinaSquare);
        BlankLocation MessinaJunction = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 12,
                            MaxHourSpawn = 24,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(15.32245f, 949.8828f, 14.81619f),
                                            Heading = 252.2207f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(15.19765f, 948.7469f, 14.81619f),
                                            Heading = 278.3394f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(23.56182f, 947.2537f, 14.04991f),
                                        Heading = 179.2383f,
                                    IsEmpty = true,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_MESSINA",
            Name = "MessinaJunction",
            Description = "",
            EntrancePosition = new Vector3(23.56182f, 947.2537f, 14.04991f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(MessinaJunction);
        BlankLocation MessinaPizza = new BlankLocation()
        {

            Name = "MessinaPizza",
            Description = "",
            EntrancePosition = new Vector3(-93.18594f, 1025.948f, 14.76096f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 12,
                            MaxHourSpawn = 22,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-93.18594f, 1025.948f, 14.76096f),
                                            Heading = 358.7756f,
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
                                        Location = new Vector3(-93.96526f, 1025.938f, 14.76096f),
                                            Heading = 0.6930794f,
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
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
        BlankLocationPlaces.Add(MessinaPizza);
        BlankLocation MessinaTower = new BlankLocation()
        {

            Name = "MessinaTower",
            Description = "",
            EntrancePosition = new Vector3(-173.0394f, 1085.167f, 15.02567f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 12,
                            MaxHourSpawn = 22,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-173.0394f, 1085.167f, 15.02567f),
                                            Heading = 266.5729f,
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
                                        Location = new Vector3(-173.0472f, 1086.035f, 15.02567f),
                                            Heading = 269.2255f,
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
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
        BlankLocationPlaces.Add(MessinaTower);
        BlankLocation MessinaHome = new BlankLocation()
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
                                        Location = new Vector3(-175.6243f, 863.0717f, 14.04042f),
                                            Heading = 3.256542f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-176.5672f, 862.9732f, 14.04042f),
                                            Heading = 0.8035054f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-174.6256f, 873.5905f, 11.5797f),
                                        Heading = 86.57639f,
                                    IsEmpty = true,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_MESSINA",
            Name = "MessinaHome",
            Description = "",
            EntrancePosition = new Vector3(-174.6256f, 873.5905f, 11.5797f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(MessinaHome);
        BlankLocation MessinaRimmers = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 10,
                            MaxHourSpawn = 22,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-52.35004f, 889.7041f, 14.81517f),
                                            Heading = 179.6572f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-51.41967f, 889.7411f, 14.81514f),
                                            Heading = 143.5801f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-63.35028f, 894.5917f, 14.04116f),
                                        Heading = 179.9867f,
                                    IsEmpty = false,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_MESSINA",
            Name = "MessinaRimmers",
            Description = "",
            EntrancePosition = new Vector3(-63.35028f, 894.5917f, 14.04116f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(MessinaRimmers);
        BlankLocation MessinaCentral = new BlankLocation()
        {

            Name = "MessinaCentral",
            Description = "",
            EntrancePosition = new Vector3(152.8445f, 891.554f, 14.75818f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 16,
                            MaxHourSpawn = 23,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(152.8445f, 891.554f, 14.75818f),
                                            Heading = 179.5025f,
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
                                        Location = new Vector3(151.957f, 891.5311f, 14.75816f),
                                            Heading = 178.4214f,
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
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
        BlankLocationPlaces.Add(MessinaCentral);
        BlankLocation MessinaCathedral = new BlankLocation()
        {

            Name = "MessinaCathedral",
            Description = "",
            EntrancePosition = new Vector3(230.8114f, 991.6428f, 15.76451f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 12,
                            MaxHourSpawn = 22,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(230.8114f, 991.6428f, 15.76451f),
                                            Heading = 40.34569f,
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
                                        Location = new Vector3(229.8671f, 990.8692f, 15.76451f),
                                            Heading = 48.32359f,
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
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
        BlankLocationPlaces.Add(MessinaCathedral);
        BlankLocation MessinaPark = new BlankLocation()
        {

            Name = "MessinaPark",
            Description = "",
            EntrancePosition = new Vector3(138.0528f, 1152.604f, 13.07685f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 12,
                            MaxHourSpawn = 22,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(138.0528f, 1152.604f, 13.07685f),
                                            Heading = 100.0038f,
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
                                        Location = new Vector3(138.3062f, 1151.798f, 13.07685f),
                                            Heading = 89.23592f,
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
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
        BlankLocationPlaces.Add(MessinaPark);
        BlankLocation MessinaPark2 = new BlankLocation()
        {

            Name = "MessinaPark2",
            Description = "",
            EntrancePosition = new Vector3(32.76894f, 1164.057f, 12.40407f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 10,
                            MaxHourSpawn = 18,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(32.76894f, 1164.057f, 12.40407f),
                                            Heading = 359.7913f,
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
                                        Location = new Vector3(33.49879f, 1164.073f, 12.40407f),
                                            Heading = 358.7981f,
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
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
        BlankLocationPlaces.Add(MessinaPark2);
        BlankLocation MessinaWilbert = new BlankLocation()
        {

            Name = "MessinaWilbert",
            Description = "",
            EntrancePosition = new Vector3(-45.43157f, 1228.374f, 10.96097f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 12,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-45.43157f, 1228.374f, 10.96097f),
                                            Heading = 1.423842f,
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
                                        Location = new Vector3(-46.24593f, 1228.389f, 10.96097f),
                                            Heading = 0.5861487f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
        BlankLocationPlaces.Add(MessinaWilbert);

    }

    private void PavanoGang()
    {
        BlankLocation PavanoTower1 = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 24,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(102.6382f, 1795.276f, 20.43974f),
                                            Heading = 0.6414985f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(103.652f, 1795.22f, 20.43974f),
                                            Heading = 0.8030295f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(92.97209f, 1800.243f, 19.73783f),
                                        Heading = 0.2984261f,
                                    IsEmpty = false,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_PAVANO",
            Name = "PavanoTower1",
            Description = "",
            EntrancePosition = new Vector3(92.97209f, 1800.243f, 19.73783f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(PavanoTower1);
        BlankLocation PavanoTower2 = new BlankLocation()
        {

            Name = "PavanoTower2",
            Description = "",
            EntrancePosition = new Vector3(138.29f, 1793.616f, 26.02802f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MaxHourSpawn = 18,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(138.29f, 1793.616f, 26.02802f),
                                            Heading = 182.411f,
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
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
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
                                        Location = new Vector3(137.4272f, 1793.6f, 26.02731f),
                                            Heading = 182.2232f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
        BlankLocationPlaces.Add(PavanoTower2);
        BlankLocation PavanoPark = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 12,
                            MaxHourSpawn = 22,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(7.616632f, 1644.216f, 14.69785f),
                                            Heading = 359.3174f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(8.343884f, 1644.175f, 14.70043f),
                                            Heading = 6.950731f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(6.858032f, 1650.251f, 14.0243f),
                                        Heading = 268.3399f,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_PAVANO",
            Name = "PavanoPark",
            Description = "",
            EntrancePosition = new Vector3(6.858032f, 1650.251f, 14.0243f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(PavanoPark);
        BlankLocation PavanoPark2 = new BlankLocation()
        {

            Name = "PavanoPark2",
            Description = "",
            EntrancePosition = new Vector3(-87.08617f, 1638.173f, 13.57591f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 10,
                            MaxHourSpawn = 18,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-87.08617f, 1638.173f, 13.57591f),
                                            Heading = 156.6432f,
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
                                        Location = new Vector3(-86.47011f, 1637.841f, 13.57806f),
                                            Heading = 139.2261f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
        BlankLocationPlaces.Add(PavanoPark2);
        BlankLocation PavanoStateB = new BlankLocation()
        {

            Name = "PavanoStateB",
            Description = "",
            EntrancePosition = new Vector3(210.4964f, 1731.246f, 20.43578f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 10,
                            MaxHourSpawn = 22,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(210.4964f, 1731.246f, 20.43578f),
                                            Heading = 1.306429f,
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
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
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
                                        Location = new Vector3(211.6855f, 1731.265f, 20.43575f),
                                            Heading = 359.2593f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
        BlankLocationPlaces.Add(PavanoStateB);
        BlankLocation PavanoAlley = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 24,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(310.5503f, 1553.653f, 14.71365f),
                                            Heading = 273.3993f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(312.3363f, 1553.586f, 14.71365f),
                                            Heading = 84.30576f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(315.4411f, 1551.918f, 13.94037f),
                                        Heading = 268.0463f,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_PAVANO",
            Name = "PavanoAlley",
            Description = "",
            EntrancePosition = new Vector3(315.4411f, 1551.918f, 13.94037f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(PavanoAlley);
        BlankLocation PavanoDeKoch = new BlankLocation()
        {

            Name = "PavanoDeKoch",
            Description = "",
            EntrancePosition = new Vector3(351.3917f, 1466.715f, 14.71385f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 12,
                            MaxHourSpawn = 22,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(351.3917f, 1466.715f, 14.71385f),
                                            Heading = 269.9776f,
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
                                        Location = new Vector3(351.2662f, 1467.586f, 14.71385f),
                                            Heading = 269.0881f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
        BlankLocationPlaces.Add(PavanoDeKoch);
        BlankLocation PavanoView = new BlankLocation()
        {

            Name = "PavanoView",
            Description = "",
            EntrancePosition = new Vector3(377.1019f, 1346.771f, 14.7133f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 12,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(377.1019f, 1346.771f, 14.7133f),
                                            Heading = 267.6965f,
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
                                        Location = new Vector3(377.0999f, 1345.885f, 14.71252f),
                                            Heading = 270.2363f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
        BlankLocationPlaces.Add(PavanoView);
        BlankLocation PavanoAlley2 = new BlankLocation()
        {

            Name = "PavanoAlley2",
            Description = "",
            EntrancePosition = new Vector3(244.0724f, 1788.297f, 20.38193f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 16,
                            MaxHourSpawn = 23,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(244.0724f, 1788.297f, 20.38193f),
                                            Heading = 357.0328f,
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
                                        Location = new Vector3(244.041f, 1789.707f, 20.38795f),
                                            Heading = 179.8688f,
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
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
        BlankLocationPlaces.Add(PavanoAlley2);
        BlankLocation PavanoColumbus = new BlankLocation()
        {

            Name = "PavanoColumbus",
            Description = "",
            EntrancePosition = new Vector3(123.0912f, 1678.171f, 14.76889f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 12,
                            MaxHourSpawn = 22,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(123.0912f, 1678.171f, 14.76889f),
                                            Heading = 182.8932f,
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
                                        Location = new Vector3(122.2373f, 1678.145f, 14.76832f),
                                            Heading = 181.8349f,
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
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
        BlankLocationPlaces.Add(PavanoColumbus);

    }

    private void KhangpaeGang()
    {
        BlankLocation KkangpaeWaterFront = new BlankLocation()
        {

            Name = "KkangpaeWaterFront",
            Description = "",
            EntrancePosition = new Vector3(-571.4893f, 1481.629f, 4.366717f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 10,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-571.4893f, 1481.629f, 4.366717f),
                                            Heading = 90.34357f,
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
                                        Location = new Vector3(-572.9838f, 1481.644f, 4.366717f),
                                            Heading = 267.5033f,
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
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {},
                    },
                },
        };
        BlankLocationPlaces.Add(KkangpaeWaterFront);
        BlankLocation KkangpaeWaterFront2 = new BlankLocation()
        {

            Name = "KkangpaeWaterFront2",
            Description = "",
            EntrancePosition = new Vector3(-601.9121f, 1330.767f, 4.21608f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 16,
                            MaxHourSpawn = 22,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-601.9121f, 1330.767f, 4.21608f),
                                            Heading = 213.3994f,
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
                                        Location = new Vector3(-603.0631f, 1330.011f, 4.21608f),
                                            Heading = 225.0585f,
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
                            {},
                    },
                },
        };
        BlankLocationPlaces.Add(KkangpaeWaterFront2);
        BlankLocation KkangpaeWaterFront3 = new BlankLocation()
        {

            Name = "KkangpaeWaterFront3",
            Description = "",
            EntrancePosition = new Vector3(-690.6245f, 1246.839f, 3.461464f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 10,
                            MaxHourSpawn = 18,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-690.6245f, 1246.839f, 3.461464f),
                                            Heading = 183.2816f,
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
                                        Location = new Vector3(-691.6246f, 1246.834f, 3.461464f),
                                            Heading = 187.6818f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
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
        BlankLocationPlaces.Add(KkangpaeWaterFront3);
        BlankLocation KkangpaeLockup = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 24,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-780.4469f, 1543.182f, 13.56419f),
                                            Heading = 95.26931f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-780.5257f, 1542.285f, 13.56056f),
                                            Heading = 96.30148f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_DRUG_DEALER",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-777.5654f, 1540.002f, 12.99034f),
                                        Heading = 271.9701f,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            Name = "KkangpaeGarage",
            Description = "",
            EntrancePosition = new Vector3(-777.5654f, 1540.002f, 12.99034f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(KkangpaeLockup);
        BlankLocation KkangpaeTattoo = new BlankLocation()
        {

            Name = "KkangpaeTattoo",
            Description = "",
            EntrancePosition = new Vector3(-1000.672f, 1615.33f, 19.47197f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MaxHourSpawn = 18,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1000.672f, 1615.33f, 19.47197f),
                                            Heading = 181.2966f,
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
                                        Location = new Vector3(-1000.054f, 1615.518f, 19.45498f),
                                            Heading = 186.6648f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
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
        BlankLocationPlaces.Add(KkangpaeTattoo);
        BlankLocation KkangpaeMrFuk = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 24,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1005.802f, 1571.065f, 19.76662f),
                                            Heading = 91.42207f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1005.762f, 1572.119f, 19.76757f),
                                            Heading = 87.69986f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_DRUG_DEALER",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-1010.889f, 1570.355f, 19.04609f),
                                        Heading = 359.7769f,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            Name = "KkangpaeMrFuk",
            Description = "",
            EntrancePosition = new Vector3(-1010.889f, 1570.355f, 19.04609f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(KkangpaeMrFuk);
        BlankLocation KkangpaeKakagawa = new BlankLocation()
        {

            Name = "KkangpaeKakagawa",
            Description = "",
            EntrancePosition = new Vector3(-1155.293f, 1490.566f, 23.22098f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 10,
                            MaxHourSpawn = 22,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1155.293f, 1490.566f, 23.22098f),
                                            Heading = 271.8781f,
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
                                        Location = new Vector3(-1155.269f, 1489.032f, 23.22081f),
                                            Heading = 272.9294f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
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
        BlankLocationPlaces.Add(KkangpaeKakagawa);
        BlankLocation KkangpaeSpanky = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 18,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1147.337f, 1390.934f, 19.81607f),
                                            Heading = 269.4826f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1147.474f, 1391.911f, 19.81655f),
                                            Heading = 271.9419f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_DRUG_DEALER",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-1141.885f, 1392.678f, 19.13541f),
                                        Heading = 178.5647f,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            Name = "KkangpaeSpanky",
            Description = "",
            EntrancePosition = new Vector3(-1141.885f, 1392.678f, 19.13541f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(KkangpaeSpanky);
        BlankLocation KkangpaeKoreshSq = new BlankLocation()
        {

            Name = "KkangpaeKoreshSq",
            Description = "",
            EntrancePosition = new Vector3(-1080.507f, 1405.152f, 23.56852f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MaxHourSpawn = 24,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1080.507f, 1405.152f, 23.56852f),
                                            Heading = 91.18325f,
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
                                        Location = new Vector3(-1080.48f, 1404.235f, 23.5685f),
                                            Heading = 91.6984f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
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
        BlankLocationPlaces.Add(KkangpaeKoreshSq);
        BlankLocation KkangpaeOffices = new BlankLocation()
        {

            Name = "KkangpaeOffices",
            Description = "",
            EntrancePosition = new Vector3(-916.506f, 1324.708f, 19.5585f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 12,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-916.506f, 1324.708f, 19.5585f),
                                            Heading = 90.53297f,
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
                                        Location = new Vector3(-916.4893f, 1323.841f, 19.5585f),
                                            Heading = 91.35294f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
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
        BlankLocationPlaces.Add(KkangpaeOffices);
        BlankLocation KkangpaeAlleySteps = new BlankLocation()
        {

            Name = "KkangpaeAlleySteps",
            Description = "",
            EntrancePosition = new Vector3(-1210.19f, 1373.812f, 22.98534f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 10,
                            MaxHourSpawn = 18,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1210.19f, 1373.812f, 22.98534f),
                                            Heading = 2.181629f,
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
                                        Location = new Vector3(-1211.004f, 1373.784f, 22.98552f),
                                            Heading = 357.224f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
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
        BlankLocationPlaces.Add(KkangpaeAlleySteps);
        BlankLocation KkangpaeErotica = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 10,
                            MaxHourSpawn = 18,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1249.753f, 1616.579f, 23.00982f),
                                            Heading = 272.9919f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1249.808f, 1617.868f, 23.00942f),
                                            Heading = 273.5631f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_DRUG_DEALER",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-1245.343f, 1617.774f, 22.42315f),
                                        Heading = 79.68229f,
                                    IsEmpty = false,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            Name = "KkangpaeErotica",
            Description = "",
            EntrancePosition = new Vector3(-1245.343f, 1617.774f, 22.42315f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(KkangpaeErotica);
        BlankLocation KkangpaeGarages2 = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 24,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1059.527f, 1645.927f, 18.80495f),
                                            Heading = 90.15751f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1060.793f, 1645.91f, 18.70853f),
                                            Heading = 268.0052f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_DRUG_DEALER",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1059.625f, 1644.961f, 18.79903f),
                                            Heading = 39.4474f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-1062.691f, 1642.404f, 17.87553f),
                                        Heading = 2.616594f,
                                    IsEmpty = false,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            Name = "KkangpaeGarages2",
            Description = "",
            EntrancePosition = new Vector3(-1062.691f, 1642.404f, 17.87553f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(KkangpaeGarages2);
        BlankLocation KkangpaeLaundry = new BlankLocation()
        {

            Name = "KkangpaeLaundry",
            Description = "",
            EntrancePosition = new Vector3(-805.9043f, 1618.525f, 13.60345f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 12,
                            MaxHourSpawn = 24,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-805.9043f, 1618.525f, 13.60345f),
                                            Heading = 181.4285f,
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
                                        Location = new Vector3(-804.9774f, 1618.503f, 13.60345f),
                                            Heading = 186.241f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
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
        BlankLocationPlaces.Add(KkangpaeLaundry);
        BlankLocation KkangpaeGozushi = new BlankLocation()
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
                                        Location = new Vector3(-1013.558f, 1809.57f, 21.77605f),
                                            Heading = 359.337f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1014.313f, 1809.578f, 21.77586f),
                                            Heading = 356.7967f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_DRUG_DEALER",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-1026.924f, 1805.179f, 21.17312f),
                                        Heading = 178.0261f,
                                    IsEmpty = false,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            Name = "KkangpaeGozushi",
            Description = "",
            EntrancePosition = new Vector3(-1026.924f, 1805.179f, 21.17312f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(KkangpaeGozushi);
        BlankLocation KkangpaeSqaure = new BlankLocation()
        {

            Name = "KkangpaeSqaure",
            Description = "",
            EntrancePosition = new Vector3(-1224.599f, 1856.645f, 20.57278f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 10,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1224.599f, 1856.645f, 20.57278f),
                                            Heading = 91.46455f,
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
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1224.624f, 1857.502f, 20.57278f),
                                            Heading = 89.68976f,
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
                            {},
                    },
                },
        };
        BlankLocationPlaces.Add(KkangpaeSqaure);
        BlankLocation KkangpaeBighorn = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 10,
                            MaxHourSpawn = 20,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1108.285f, 2166.185f, 27.74875f),
                                            Heading = 271.3477f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1108.346f, 2165.137f, 27.74966f),
                                            Heading = 269.337f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-1102.706f, 2167.413f, 26.97584f),
                                        Heading = 177.6403f,
                                },
                            }
                    }
            },
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            Name = "KkangpaeBighorn",
            Description = "",
            EntrancePosition = new Vector3(-1102.706f, 2167.413f, 26.97584f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(KkangpaeBighorn);
        BlankLocation KkangpaeBite = new BlankLocation()
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
                                        Location = new Vector3(-751.3274f, 2196.831f, 20.51253f),
                                            Heading = 89.9465f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-751.29f, 2197.56f, 20.51253f),
                                            Heading = 89.77574f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-754.8597f, 2182.453f, 19.78052f),
                                        Heading = 272.3572f,
                                    IsEmpty = false,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            Name = "KkangpaeBite",
            Description = "",
            EntrancePosition = new Vector3(-754.8597f, 2182.453f, 19.78052f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(KkangpaeBite);
        BlankLocation KkangpaeCuisine = new BlankLocation()
        {

            Name = "KkangpaeCuisine",
            Description = "",
            EntrancePosition = new Vector3(-784.1915f, 1762.503f, 26.46432f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 10,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-784.1915f, 1762.503f, 26.46432f),
                                            Heading = 182.5372f,
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
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-783.0187f, 1762.525f, 26.4659f),
                                            Heading = 178.1389f,
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
                            {},
                    },
                },
        };
        BlankLocationPlaces.Add(KkangpaeCuisine);

    }

    private void TriadGang()
    {
        BlankLocation TriadStreet = new BlankLocation()
        {

            Name = "TriadStreet",
            Description = "",
            EntrancePosition = new Vector3(247.7431f, 131.8762f, 15.20141f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 10,
                            MaxHourSpawn = 18,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(247.7431f, 131.8762f, 15.20141f),
                                            Heading = 29.75146f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(246.9359f, 131.3658f, 15.20141f),
                                            Heading = 31.13165f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_UPRIGHT_CLUBHOUSE",
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
        BlankLocationPlaces.Add(TriadStreet);
        BlankLocation TriadStreet2 = new BlankLocation()
        {

            Name = "TriadStreet2",
            Description = "",
            EntrancePosition = new Vector3(256.9587f, 55.54659f, 14.7601f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 18,
                            MaxHourSpawn = 24,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(256.9587f, 55.54659f, 14.7601f),
                                            Heading = 87.65568f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(256.9993f, 54.77523f, 14.75925f),
                                            Heading = 91.00845f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_UPRIGHT_CLUBHOUSE",
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
        BlankLocationPlaces.Add(TriadStreet2);
        BlankLocation TriadStreet3 = new BlankLocation()
        {

            Name = "TriadStreet3",
            Description = "",
            EntrancePosition = new Vector3(320.829f, 12.61634f, 14.77163f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 8,
                            MaxHourSpawn = 18,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(320.829f, 12.61634f, 14.77163f),
                                            Heading = 175.3779f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(320.113f, 12.61075f, 14.77163f),
                                            Heading = 182.1518f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_UPRIGHT_CLUBHOUSE",
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
        BlankLocationPlaces.Add(TriadStreet3);
        BlankLocation TriadStreet4 = new BlankLocation()
        {

            Name = "TriadStreet4",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(313.4558f, 44.77863f, 14.76246f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 18,
                            MaxHourSpawn = 24,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(313.4558f, 44.77863f, 14.76246f),
                                            Heading = 108.6177f,
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
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
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
                                        Location = new Vector3(312.794f, 46.18323f, 14.75961f),
                                            Heading = 132.4179f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(309.3086f, 42.75834f, 14.76591f),
                                            Heading = 294.6251f,
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
                                        Location = new Vector3(306.6272f, 44.08677f, 14.76505f),
                                            Heading = 264.2014f,
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
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,




                                    }

                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(299.0294f, 41.41854f, 13.99496f),
                                            Heading = 178.3689f,
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
        BlankLocationPlaces.Add(TriadStreet4);
        BlankLocation TriadStreet5 = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 24,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(230.2047f, 131.9957f, 15.51356f),
                                            Heading = 270.9149f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(230.1209f, 131.1217f, 15.51356f),
                                            Heading = 267.269f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(234.0642f, 124.7465f, 14.01854f),
                                        Heading = 181.2395f,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
            Name = "TriadStreet5",
            Description = "",
            EntrancePosition = new Vector3(234.0642f, 124.7465f, 14.01854f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(TriadStreet5);
        BlankLocation TriadStreet6 = new BlankLocation()
        {

            Name = "TriadStreet6",
            Description = "",
            EntrancePosition = new Vector3(243.7366f, 145.9383f, 14.76393f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 8,
                            MaxHourSpawn = 17,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(243.7366f, 145.9383f, 14.76393f),
                                            Heading = 155.0832f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(242.9847f, 146.6161f, 14.76357f),
                                            Heading = 159.182f,
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
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {},
                    },
                },
        };
        BlankLocationPlaces.Add(TriadStreet6);
        BlankLocation TriadStreet7 = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 16,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(247.2991f, 52.29225f, 14.75236f),
                                            Heading = 268.5446f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(247.218f, 50.87874f, 14.75463f),
                                            Heading = 268.4127f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(243.1232f, 46.86388f, 14.08408f),
                                        Heading = 269.894f,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
            Name = "TriadStreet7",
            Description = "",
            EntrancePosition = new Vector3(247.2991f, 52.29225f, 14.75236f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(TriadStreet7);
        BlankLocation TriadStreet8 = new BlankLocation()
        {

            Name = "TriadStreet8",
            Description = "",
            EntrancePosition = new Vector3(254.183f, 113.8661f, 14.76254f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 18,
                            MaxHourSpawn = 24,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(254.183f, 113.8661f, 14.76254f),
                                            Heading = 354.4641f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(253.9465f, 115.4141f, 14.76421f),
                                            Heading = 186.1902f,
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
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {},
                    },
                },
        };
        BlankLocationPlaces.Add(TriadStreet8);
        BlankLocation TriadStreet9 = new BlankLocation()
        {

            Name = "TriadStreet9",
            Description = "",
            EntrancePosition = new Vector3(237.1486f, 71.6002f, 14.76196f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 8,
                            MaxHourSpawn = 18,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(237.1486f, 71.6002f, 14.76196f),
                                            Heading = 357.6227f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(238.5029f, 71.5227f, 14.76253f),
                                            Heading = 358.8286f,
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
        BlankLocationPlaces.Add(TriadStreet9);
        BlankLocation TriadParking = new BlankLocation()
        {

            Name = "TriadParking",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(333.5168f, 148.6803f, 13.9372f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 12,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(334.3547f, 146.5753f, 14.61184f),
                                            Heading = 177.0485f,
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
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
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
                                        Location = new Vector3(334.4878f, 143.8822f, 14.72976f),
                                            Heading = 3.626181f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(333.6156f, 143.7873f, 14.7214f),
                                            Heading = 359.9373f,
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
                                        Location = new Vector3(331.6558f, 142.8965f, 17.36077f),
                                            Heading = 355.1356f,
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
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
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
                                        Location = new Vector3(333.5168f, 148.6803f, 13.9372f),
                                            Heading = 115.5975f,
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
        BlankLocationPlaces.Add(TriadParking);

    }

    private void PetrovicGang()
    {
        BlankLocation PetrovicCarServices = new BlankLocation()
        {
            Name = "PetrovicCarServices",
            FullName = "",
            Description = "Petrovic at Express Car Services",
            EntrancePosition = new Vector3(1060.557f, 224.3717f, 15.59858f),
            OpenTime = 0,
            CloseTime = 24,
            StateID = "Liberty",
            AssignedAssociationID = "AMBIENT_GANG_PETROVIC",
            MenuID = "",
            PossibleGroupSpawns = new List<ConditionalGroup>() {
            new ConditionalGroup() {
            Name = "",
            Percentage = defaultSpawnPercentage,
            MinHourSpawn = 8,
            MaxHourSpawn = 16,
            PossiblePedSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1060.557f,224.3717f,15.59858f),
            Heading = 253.0275f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_STAND_MOBILE_FACILITY",
            "WORLD_HUMAN_SMOKING",
            },
            },
                new GangConditionalLocation() {
                Location = new Vector3(1060.765f,223.128f,15.59858f),
                Heading = 266.1909f,
                AssociationID = "",
                RequiredPedGroup = "",
                RequiredVehicleGroup = "",
                TaskRequirements = TaskRequirements.Guard,
                ForcedScenarios = new List<String>() {
                "WORLD_HUMAN_STAND_MOBILE",
                "WORLD_HUMAN_SMOKING",
                },
                },
                },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1056.836f,220.429f,14.66296f),
            Heading = 270.1932f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            ForcedScenarios = new List<String>() {
            },
            },
            },
            },
            new ConditionalGroup() {
            Name = "",
            Percentage = defaultSpawnPercentage,
            MinHourSpawn = 18,
            MaxHourSpawn = 24,
            PossiblePedSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1051.202f,232.595f,15.33756f),
            Heading = 163.001f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_HANG_OUT_STREET",
            "WORLD_HUMAN_SMOKING",
            },
            },
            new GangConditionalLocation() {
            Location = new Vector3(1051.344f,230.8872f,15.33756f),
            Heading = -16.033f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_STAND_IMPATIENT_FACILITY",
            "WORLD_HUMAN_HANG_OUT_STREET",
            },
            },
            new GangConditionalLocation() {
            Location = new Vector3(1052.226f,232.1301f,15.33756f),
            Heading = 134.217f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_STAND_MOBILE",
            "WORLD_HUMAN_SMOKING_CLUBHOUSE",
            },
            },
            new GangConditionalLocation() {
            Location = new Vector3(1049.771f,225.6905f,15.33756f),
            Heading = -37.78978f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_GUARD_STAND_CLUBHOUSE",
            "WORLD_HUMAN_SMOKING",
            },
            },
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1048.376f,224.4079f,15.01581f),
            Heading = -149.631f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            ForcedScenarios = new List<String>() {
            },
            },
            },
            },
            },
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
            },
            VendorLocations = new List<SpawnPlace>()
            {
            },
            VendorHeadDataGroupID = "",
            VehiclePreviewLocation = new SpawnPlace()
            {
            },
            VehicleDeliveryLocations = new List<SpawnPlace>()
            {
            },
        };
        BlankLocationPlaces.Add(PetrovicCarServices);
        BlankLocation Petrovic69erDiner = new BlankLocation()
        {
            Name = "Petrovic69erDiner",
            FullName = "",
            Description = "2 Petrovic at 69th Street Diner",
            EntrancePosition = new Vector3(1116.795f, 12.65497f, 14.93323f),
            OpenTime = 0,
            CloseTime = 24,
            StateID = "Liberty",
            AssignedAssociationID = "AMBIENT_GANG_PETROVIC",
            MenuID = "",
            PossibleGroupSpawns = new List<ConditionalGroup>() {
            new ConditionalGroup() {
            Name = "",
            Percentage = defaultSpawnPercentage,
            MinHourSpawn = 8,
            MaxHourSpawn = 16,
            PossiblePedSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1116.795f,12.65497f,14.93323f),
            Heading = 337.7535f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_STAND_MOBILE_FACILITY",
            "WORLD_HUMAN_SMOKING",
            },
            },
            new GangConditionalLocation() {
            Location = new Vector3(1118.274f,12.5189f,14.97527f),
            Heading = 27.80906f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_STAND_MOBILE",
            "WORLD_HUMAN_SMOKING",
            },
            },
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1112.5f,10.90448f,14.11008f),
            Heading = 181.7274f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            ForcedScenarios = new List<String>() {
            },
            },
            },
            },
            },
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
            },
            VendorLocations = new List<SpawnPlace>()
            {
            },
            VendorHeadDataGroupID = "",
            VehiclePreviewLocation = new SpawnPlace()
            {
            },
            VehicleDeliveryLocations = new List<SpawnPlace>()
            {
            },
        };
        BlankLocationPlaces.Add(Petrovic69erDiner);
        BlankLocation PetrovicComradesBar = new BlankLocation()
        {
            Name = "PetrovicComradesBar",
            FullName = "",
            Description = "2 Petrovic at Comrades Bar",
            EntrancePosition = new Vector3(1166.035f, 0.06602886f, 15.25864f),
            OpenTime = 0,
            CloseTime = 24,
            StateID = "Liberty",
            AssignedAssociationID = "AMBIENT_GANG_PETROVIC",
            MenuID = "",
            PossibleGroupSpawns = new List<ConditionalGroup>() {
            new ConditionalGroup() {
            Name = "",
            Percentage = defaultSpawnPercentage,
            MinHourSpawn = 18,
            MaxHourSpawn = 23,
            PossiblePedSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1166.035f,0.06602886f,15.25864f),
            Heading = 91.14774f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_STAND_MOBILE_FACILITY",
            "WORLD_HUMAN_LEANING_CASINO_TERRACE",
            },
            },
            new GangConditionalLocation() {
            Location = new Vector3(1165.974f,1.157122f,15.28025f),
            Heading = 129.3841f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_STAND_MOBILE",
            "WORLD_HUMAN_SMOKING",
            },
            },
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1169.697f,-6.863485f,14.49819f),
            Heading = 88.63706f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            ForcedScenarios = new List<String>() {
            },
            },
            },
            },
            },
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
            },
            VendorLocations = new List<SpawnPlace>()
            {
            },
            VendorHeadDataGroupID = "",
            VehiclePreviewLocation = new SpawnPlace()
            {
            },
            VehicleDeliveryLocations = new List<SpawnPlace>()
            {
            },
        };
        BlankLocationPlaces.Add(PetrovicComradesBar);
        BlankLocation PetrovicSexShop = new BlankLocation()
        {
            Name = "PetrovicSexShop",
            FullName = "",
            Description = "1 Petrovic at Sex Shop",
            EntrancePosition = new Vector3(1022.138f, -48.6943f, 9.500606f),
            OpenTime = 0,
            CloseTime = 24,
            StateID = "Liberty",
            AssignedAssociationID = "AMBIENT_GANG_PETROVIC",
            MenuID = "",
            PossibleGroupSpawns = new List<ConditionalGroup>() {
            new ConditionalGroup() {
            Name = "",
            Percentage = defaultSpawnPercentage,
            MinHourSpawn = 12,
            MaxHourSpawn = 20,
            PossiblePedSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1022.138f,-48.6943f,9.500606f),
            Heading = 134.7647f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_STAND_MOBILE_FACILITY",
            "WORLD_HUMAN_SMOKING",
            },
            },
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1016.142f,-56.35802f,9.214223f),
            Heading = 49.11929f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            ForcedScenarios = new List<String>() {
            },
            },
            },
            },
            },
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
            },
            VendorLocations = new List<SpawnPlace>()
            {
            },
            VendorHeadDataGroupID = "",
            VehiclePreviewLocation = new SpawnPlace()
            {
            },
            VehicleDeliveryLocations = new List<SpawnPlace>()
            {
            },
        };
        BlankLocationPlaces.Add(PetrovicSexShop);
        BlankLocation PetrovicHomeGroup = new BlankLocation()
        {
            Name = "PetrovicHomeGroup",
            FullName = "",
            Description = "4 Petrovic at random home",
            EntrancePosition = new Vector3(1353.559f, 46.04258f, 17.79775f),
            OpenTime = 0,
            CloseTime = 24,
            StateID = "Liberty",
            AssignedAssociationID = "AMBIENT_GANG_PETROVIC",
            MenuID = "",
            PossibleGroupSpawns = new List<ConditionalGroup>() {
            new ConditionalGroup() {
            Name = "",
            Percentage = defaultSpawnPercentage,
            MinHourSpawn = 18,
            MaxHourSpawn = 24,
            PossiblePedSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1353.559f,46.04258f,17.79775f),
            Heading = 92.58981f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_GUARD_STAND_CLUBHOUSE",
            },
            },
            new GangConditionalLocation() {
            Location = new Vector3(1341.774f,42.94996f,13.60607f),
            Heading = 50.88604f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_STAND_MOBILE_UPRIGHT",
            "WORLD_HUMAN_SMOKING_CLUBHOUSE",
            },
            },
            new GangConditionalLocation() {
            Location = new Vector3(1341.804f,44.67862f,13.60499f),
            Heading = 123.1636f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
            "WORLD_HUMAN_SMOKING",
            },
            },
            new GangConditionalLocation() {
            Location = new Vector3(1340.423f,43.63142f,13.60668f),
            Heading = 278.5757f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
            "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
            },
            },
            new GangConditionalLocation() {
            Location = new Vector3(1351.192f,50.31235f,13.42303f),
            Heading = 91.44496f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_LEANING_CASINO_TERRACE",
            "WORLD_HUMAN_SMOKING",
            },
            MinHourSpawn = 18,
            },
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1347.531f,47.9216f,12.82223f),
            Heading = 271.1961f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            ForcedScenarios = new List<String>() {
            },
            },
            new GangConditionalLocation() {
            Location = new Vector3(1347.155f,52.65498f,12.99181f),
            Heading = 90.49146f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            ForcedScenarios = new List<String>() {
            },
            MinHourSpawn = 20,
            },
            },
            },
            },
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
            },
            VendorLocations = new List<SpawnPlace>()
            {
            },
            VendorHeadDataGroupID = "",
            VehiclePreviewLocation = new SpawnPlace()
            {
            },
            VehicleDeliveryLocations = new List<SpawnPlace>()
            {
            },
        };
        BlankLocationPlaces.Add(PetrovicHomeGroup);
        BlankLocation PetrovicBeachboys = new BlankLocation()
        {
            Name = "PetrovicBeachboys",
            FullName = "",
            Description = "2 Petrovic at Firefly walkway",
            EntrancePosition = new Vector3(1180.539f, -185.1522f, 16.44039f),
            OpenTime = 0,
            CloseTime = 24,
            StateID = "Liberty",
            AssignedAssociationID = "AMBIENT_GANG_PETROVIC",
            MenuID = "",
            PossibleGroupSpawns = new List<ConditionalGroup>() {
            new ConditionalGroup() {
            Name = "",
            Percentage = defaultSpawnPercentage,
            MinHourSpawn = 10,
            MaxHourSpawn = 18,
            PossiblePedSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1180.539f,-185.1522f,16.44039f),
            Heading = -144.6719f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
            "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
            "WORLD_HUMAN_SMOKING_CLUBHOUSE",
            },
            },
            new GangConditionalLocation() {
            Location = new Vector3(1181.491f,-185.1007f,16.44039f),
            Heading = 165.3603f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_STAND_MOBILE",
            "WORLD_HUMAN_HANG_OUT_STREET",
            "WORLD_HUMAN_SMOKING",
            },
            },
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
            },
            },
            },
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
            },
            VendorLocations = new List<SpawnPlace>()
            {
            },
            VendorHeadDataGroupID = "",
            VehiclePreviewLocation = new SpawnPlace()
            {
            },
            VehicleDeliveryLocations = new List<SpawnPlace>()
            {
            },
        };
        BlankLocationPlaces.Add(PetrovicBeachboys);
        BlankLocation PetrovicBeachboys2 = new BlankLocation()
        {
            Name = "PetrovicBeachboys2",
            FullName = "",
            Description = "2 Petrovic at Firefly Walkway2",
            EntrancePosition = new Vector3(1330.582f, -149.4676f, 12.96499f),
            OpenTime = 0,
            CloseTime = 24,
            StateID = "Liberty",
            AssignedAssociationID = "AMBIENT_GANG_PETROVIC",
            MenuID = "",
            PossibleGroupSpawns = new List<ConditionalGroup>() {
            new ConditionalGroup() {
            Name = "",
            Percentage = defaultSpawnPercentage,
            MinHourSpawn = 10,
            MaxHourSpawn = 18,
            PossiblePedSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1329.791f,-185.2679f,16.44043f),
            Heading = -88.09063f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_STAND_IMPATIENT",
            "WORLD_HUMAN_HANG_OUT",
            "WORLD_HUMAN_STAND_MOBILE_FACILITY",
            "WORLD_HUMAN_SMOKING",
            },
            },
            new GangConditionalLocation() {
            Location = new Vector3(1330.831f,-185.2264f,16.44043f),
            Heading = 91.29375f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_STAND_IMPATIENT_FACILITY",
            "WORLD_HUMAN_HANG_OUT_STREET",
            "WORLD_HUMAN_STAND_MOBILE",
            },
            },
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1330.582f,-149.4676f,12.96499f),
            Heading = 0.9169335f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            ForcedScenarios = new List<String>() {
            },
            },
            },
            },
            new ConditionalGroup() {
            Name = "",
            Percentage = defaultSpawnPercentage,
            MinHourSpawn = 18,
            MaxHourSpawn = 23,
            PossiblePedSpawns = new List<ConditionalLocation>() {
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1330.582f,-149.4676f,12.96499f),
            Heading = 0.9169335f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            IsEmpty = false,
            ForcedScenarios = new List<String>() {
            },
            },
            },
            },
            },
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
            },
            VendorLocations = new List<SpawnPlace>()
            {
            },
            VendorHeadDataGroupID = "",
            VehiclePreviewLocation = new SpawnPlace()
            {
            },
            VehicleDeliveryLocations = new List<SpawnPlace>()
            {
            },
        };
        BlankLocationPlaces.Add(PetrovicBeachboys2);
        BlankLocation PetrovicPerestroika = new BlankLocation()
        {
            Name = "PetrovicPerestroika",
            FullName = "",
            Description = "2 Petrovic at Perestroika",
            EntrancePosition = new Vector3(1198.493f, 203.3723f, 19.91231f),
            OpenTime = 0,
            CloseTime = 24,
            StateID = "Liberty",
            AssignedAssociationID = "AMBIENT_GANG_PETROVIC",
            MenuID = "",
            PossibleGroupSpawns = new List<ConditionalGroup>() {
            new ConditionalGroup() {
            Name = "",
            Percentage = defaultSpawnPercentage,
            MinHourSpawn = 10,
            MaxHourSpawn = 18,
            PossiblePedSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1198.493f,203.3723f,19.91231f),
            Heading = 131.9387f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_LEANING_CASINO_TERRACE",
            "WORLD_HUMAN_SMOKING",
            },
            },
            new GangConditionalLocation() {
            Location = new Vector3(1198.097f,202.4012f,19.81704f),
            Heading = -14.91232f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_STAND_IMPATIENT_FACILITY",
            "WORLD_HUMAN_HANG_OUT_STREET",
            "WORLD_HUMAN_STAND_MOBILE",
            },
            },
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
            },
            },
            },
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
            },
            VendorLocations = new List<SpawnPlace>()
            {
            },
            VendorHeadDataGroupID = "",
            VehiclePreviewLocation = new SpawnPlace()
            {
            },
            VehicleDeliveryLocations = new List<SpawnPlace>()
            {
            },
        };
        BlankLocationPlaces.Add(PetrovicPerestroika);
        BlankLocation PetrovicBackAlley = new BlankLocation()
        {
            Name = "PetrovicBackAlley",
            FullName = "",
            Description = "Petrovic Courtyard",
            EntrancePosition = new Vector3(1388.117f, 69.95895f, 16.5161f),
            OpenTime = 0,
            CloseTime = 24,
            StateID = "Liberty",
            AssignedAssociationID = "AMBIENT_GANG_PETROVIC",
            MenuID = "",
            PossibleGroupSpawns = new List<ConditionalGroup>() {
            new ConditionalGroup() {
            Name = "",
            Percentage = defaultSpawnPercentage,
            MinHourSpawn = 10,
            MaxHourSpawn = 16,
            PossiblePedSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1388.117f,69.95895f,16.5161f),
            Heading = -98.90459f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_HANG_OUT_STREET",
            "WORLD_HUMAN_SMOKING",
            },
            },
            new GangConditionalLocation() {
            Location = new Vector3(1389.182f,69.86279f,16.53918f),
            Heading = 92.34665f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_STAND_IMPATIENT_FACILITY",
            "WORLD_HUMAN_HANG_OUT_STREET",
            "WORLD_HUMAN_STAND_MOBILE",
            },
            },
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
            },
            },
            new ConditionalGroup() {
            Name = "",
            Percentage = defaultSpawnPercentage,
            MinHourSpawn = 16,
            MaxHourSpawn = 23,
            PossiblePedSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1392.658f,59.53156f,16.43925f),
            Heading = -112.4795f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_HANG_OUT_STREET",
            "WORLD_HUMAN_SMOKING",
            },
            },
            new GangConditionalLocation() {
            Location = new Vector3(1392.597f,58.59082f,16.43925f),
            Heading = -66.63788f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_HANG_OUT_STREET",
            "WORLD_HUMAN_STAND_MOBILE",
            },
            },
            new GangConditionalLocation() {
            Location = new Vector3(1393.703f,59.13092f,16.43925f),
            Heading = 87.99725f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
            "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
            "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
            },
            },
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
            },
            },
            },
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
            },
            VendorLocations = new List<SpawnPlace>()
            {
            },
            VendorHeadDataGroupID = "",
            VehiclePreviewLocation = new SpawnPlace()
            {
            },
            VehicleDeliveryLocations = new List<SpawnPlace>()
            {
            },
        };
        BlankLocationPlaces.Add(PetrovicBackAlley);
        BlankLocation PetrovicBeachgate = new BlankLocation()
        {
            Name = "PetrovicBeachgate",
            FullName = "",
            Description = "Petrovic BeagateParking",
            EntrancePosition = new Vector3(1557.437f, -118.1017f, 12.24019f),
            OpenTime = 0,
            CloseTime = 24,
            StateID = "Liberty",
            AssignedAssociationID = "AMBIENT_GANG_PETROVIC",
            MenuID = "",
            PossibleGroupSpawns = new List<ConditionalGroup>() {
            new ConditionalGroup() {
            Name = "",
            Percentage = defaultSpawnPercentage,
            MinHourSpawn = 10,
            MaxHourSpawn = 16,
            PossiblePedSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1540.721f,-112.1248f,12.89008f),
            Heading = -178.1683f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_LEANING",
            "WORLD_HUMAN_SMOKING",
            },
            },
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1557.437f,-118.1017f,12.24019f),
            Heading = 0.9169335f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            ForcedScenarios = new List<String>() {
            },
            },
            },
            },
            new ConditionalGroup() {
            Name = "",
            Percentage = defaultSpawnPercentage,
            MinHourSpawn = 16,
            MaxHourSpawn = 23,
            PossiblePedSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1552.54f,-122.7449f,12.71858f),
            Heading = -84.1668f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_LEANING_CASINO_TERRACE",
            "WORLD_HUMAN_SMOKING",
            },
            },
            new GangConditionalLocation() {
            Location = new Vector3(1553.727f,-122.035f,12.71827f),
            Heading = 119.951f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_STAND_IMPATIENT_FACILITY",
            "WORLD_HUMAN_HANG_OUT_STREET",
            "WORLD_HUMAN_STAND_MOBILE",
            },
            },
            new GangConditionalLocation() {
            Location = new Vector3(1553.922f,-123.095f,12.71747f),
            Heading = 65.00776f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_STAND_IMPATIENT_FACILITY",
            "WORLD_HUMAN_HANG_OUT_STREET",
            "WORLD_HUMAN_STAND_MOBILE",
            },
            },
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1557.437f,-118.1017f,12.24019f),
            Heading = 179.6279f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            IsEmpty = false,
            ForcedScenarios = new List<String>() {
            },
            },
            },
            },
            },
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
            },
            VendorLocations = new List<SpawnPlace>()
            {
            },
            VendorHeadDataGroupID = "",
            VehiclePreviewLocation = new SpawnPlace()
            {
            },
            VehicleDeliveryLocations = new List<SpawnPlace>()
            {
            },
        };
        BlankLocationPlaces.Add(PetrovicBeachgate);
        //more!!! non converts
        BlankLocation PetrovicHome = new BlankLocation()
        {

            Name = "PetrovicHome",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(1076.853f, -25.42395f, 13.36073f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            InteriorID = 88834,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_PETROVIC",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 16,
                            MaxHourSpawn = 24,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1102.523f, -21.64999f, 16.2777f),
                                            Heading = 270.0482f,
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
                                                "PROP_HUMAN_BBQ",
                                                "WORLD_HUMAN_DRUG_PROCESSORS_COKE",
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
                                        Location = new Vector3(1092.259f, -19.63508f, 16.2777f),
                                            Heading = 188.9128f,
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
                                            MaxWantedLevelSpawn = 3,




                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1092.295f, -21.92762f, 16.2777f),
                                            Heading = 358.8919f,
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
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(1093.481f, -22.07043f, 16.27769f),
                                            Heading = 12.9685f,
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
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
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
                                        Location = new Vector3(1076.853f, -25.42395f, 13.36073f),
                                            Heading = 4.276759f,
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
        }; //interior
        BlankLocationPlaces.Add(PetrovicHome);
        BlankLocation PetrovicMasterson = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 12,
                            MaxHourSpawn = 22,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1418.873f, 194.1715f, 21.52926f),
                                            Heading = 269.3707f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1418.756f, 195.7192f, 21.54207f),
                                            Heading = 267.6179f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(1425.172f, 192.4428f, 20.78341f),
                                        Heading = 359.0621f,
                                },
                            },
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_PETROVIC",
            Name = "PetrovicMasterson",
            Description = "",
            EntrancePosition = new Vector3(1425.172f, 192.4428f, 20.78341f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(PetrovicMasterson);
        BlankLocation PetrovicAlexeis = new BlankLocation()
        {

            Name = "PetrovicAlexeis",
            Description = "",
            EntrancePosition = new Vector3(1135.145f, 215.1884f, 18.7699f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_PETROVIC",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 10,
                            MaxHourSpawn = 16,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1135.145f, 215.1884f, 18.7699f),
                                            Heading = 265.6195f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(1135.11f, 216.3598f, 18.81138f),
                                            Heading = 268.7603f,
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
                            {},
                    },
                },
        };
        BlankLocationPlaces.Add(PetrovicAlexeis);
        BlankLocation PetrovicLaundromat = new BlankLocation()
        {

            Name = "PetrovicLaundromat",
            Description = "",
            EntrancePosition = new Vector3(1250.569f, 171.2465f, 20.27625f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_PETROVIC",
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
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1250.569f, 171.2465f, 20.27625f),
                                            Heading = 104.3423f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(1250.922f, 170.0503f, 20.25072f),
                                            Heading = 101.0594f,
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
                            {},
                    },
                },
        };
        BlankLocationPlaces.Add(PetrovicLaundromat);
        BlankLocation PetrovicHome2 = new BlankLocation()
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
                                        Location = new Vector3(1193.487f, 298.6674f, 24.71638f),
                                            Heading = 219.7444f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(1196.496f, 302.849f, 21.9922f),
                                        Heading = 197.944f,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_PETROVIC",
            Name = "PetrovicHome2",
            Description = "",
            EntrancePosition = new Vector3(1193.487f, 298.6674f, 24.71638f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(PetrovicHome2);

    }

    private void SpanishLordsGang()
    {
        BlankLocation SpanLordsMem = new BlankLocation()
        {

            Name = "SpanLordsMem",
            FullName = "",
            Description = "2 Spanish Lords at Hernan Memorial",
            MapIcon = 78,
            MapIconScale = 1f,
            EntrancePosition = new Vector3(811.6744f, 1869.621f, 10.9797f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = "Liberty",


            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            MenuID = "",

            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                new ConditionalGroup()
                {
                    Name = "",
                    Percentage = defaultSpawnPercentage,
                    MinHourSpawn = 8,
                    MaxHourSpawn = 20,
                    MaxWantedLevelSpawn = 4,
                    PossiblePedSpawns = new List<ConditionalLocation>()
                    {
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(811.6744f, 1869.621f, 10.9797f),
                            Heading = 262.1925f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>()
                            {
                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                            },
                            MaxWantedLevelSpawn = 4,
                        },
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(811.6653f, 1870.759f, 10.96288f),
                            Heading = 258.0417f,
                            AssociationID = "",
                            RequiredPedGroup = "",
                            RequiredVehicleGroup = "",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>()
                            {
                                "WORLD_HUMAN_HANG_OUT_STREET",
                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                            },
                            MaxWantedLevelSpawn = 4,
                        },
                    },
                    PossibleVehicleSpawns = new List<ConditionalLocation>() {
                   },
                },
            },
            PossiblePedSpawns = new List<ConditionalLocation>() { },
            PossibleVehicleSpawns = new List<ConditionalLocation>() { },
            VehiclePreviewLocation = new SpawnPlace() { },
        };
        BlankLocationPlaces.Add(SpanLordsMem);
        BlankLocation SpanishLordsBasketball = new BlankLocation()
        {

            Name = "SpanishLordsBasketball",
            Description = "",
            EntrancePosition = new Vector3(557.9919f, 2034.14f, 14.5395f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
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
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(557.9919f, 2034.14f, 14.5395f),
                                            Heading = 221.0592f,
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
                                            MaxWantedLevelSpawn = 4,




                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(557.4238f, 2033.133f, 14.5395f),
                                            Heading = 300.942f,
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
                                            MaxWantedLevelSpawn = 4,




                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {},
                    },
                },
        };
        BlankLocationPlaces.Add(SpanishLordsBasketball);
        BlankLocation SpanishLordsBlocks = new BlankLocation()
        {

            Name = "SpanishLordsBlocks",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(617.1314f, 2023.676f, 16.94814f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 10,
                            MaxHourSpawn = 23,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(617.1314f, 2023.676f, 16.94814f),
                                            Heading = 342.796f,
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
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 4,




                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(600.4146f, 2001.644f, 16.96493f),
                                            Heading = 249.5234f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 4,




                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(602.0458f, 2000.762f, 16.96542f),
                                            Heading = 57.3554f,
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
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 4,




                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(600.9976f, 1999.838f, 16.96445f),
                                            Heading = 16.9324f,
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
                                                "WORLD_HUMAN_DRUG_DEALER",
                                                "WORLD_HUMAN_DRINKING_FACILITY",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 4,




                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(615.6309f, 2031.731f, 15.84769f),
                                            Heading = 144.0499f,
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
                                            MaxWantedLevelSpawn = 4,




                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(611.8477f, 2033.963f, 16.1017f),
                                            Heading = 141.1463f,
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
                                            MaxWantedLevelSpawn = 4,




                                    },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(SpanishLordsBlocks);
        BlankLocation SpanishLordsBlocks2 = new BlankLocation()
        {

            Name = "SpanishLordsBlocks2",
            Description = "",
            EntrancePosition = new Vector3(676.0126f, 1945.854f, 9.848968f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 10,
                            MaxHourSpawn = 18,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(676.0126f, 1945.854f, 9.848968f),
                                            Heading = 306.0933f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 4,




                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(675.697f, 1946.676f, 9.884265f),
                                            Heading = 269.9267f,
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
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 4,




                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {},
                    },
                },
        };
        BlankLocationPlaces.Add(SpanishLordsBlocks2);
        BlankLocation SpanishLordsSwitchSt = new BlankLocation()
        {

            Name = "SpanishLordsSwitchSt",
            Description = "",
            EntrancePosition = new Vector3(798.3217f, 2045.696f, 18.17941f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
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
                            MaxHourSpawn = 16,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(798.3217f, 2045.696f, 18.17941f),
                                            Heading = 205.9291f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 4,




                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(799.1834f, 2046.195f, 18.17941f),
                                            Heading = 185.5197f,
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
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 4,




                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {},
                    },
                },
        };
        BlankLocationPlaces.Add(SpanishLordsSwitchSt);
        BlankLocation SpanishLordsFolsom = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 12,
                            MaxHourSpawn = 22,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(703.6315f, 2151.521f, 17.22648f),
                                            Heading = 359.7403f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_DRINKING_FACILITY"
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(705.1905f, 2151.458f, 17.39209f),
                                            Heading = 10.98074f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT"
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(706.3038f, 2159.385f, 16.91493f),
                                        Heading = 77.53199f,
                                },
                            },
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            Name = "SpanishLordsFolsom",
            Description = "",
            EntrancePosition = new Vector3(706.3038f, 2159.385f, 16.91493f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(SpanishLordsFolsom);
        BlankLocation SpanishLordsButterflyCabin = new BlankLocation()
        {

            Name = "SpanishLordsButterflyCabin",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(544.2592f, 2352.597f, 17.14791f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
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
                            MaxHourSpawn = 23,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(540.7457f, 2357.627f, 18.16157f),
                                            Heading = 62.83598f,
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
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 4,




                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(544.3738f, 2357.82f, 17.77132f),
                                            Heading = 273.0719f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 4,




                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(537.7017f, 2356.656f, 18.16157f),
                                            Heading = 298.1473f,
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
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 4,




                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(537.6066f, 2358.586f, 18.16157f),
                                            Heading = 244.238f,
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
                                                "WORLD_HUMAN_DRUG_DEALER",
                                                "WORLD_HUMAN_DRINKING_FACILITY",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 4,




                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(544.2592f, 2352.597f, 17.14791f),
                                            Heading = 43.74239f,
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
                                            MaxWantedLevelSpawn = 4,




                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(540.8048f, 2346.397f, 17.07111f),
                                            Heading = 95.20958f,
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
                                            MaxWantedLevelSpawn = 4,




                                    },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(SpanishLordsButterflyCabin);
        BlankLocation SpanishLordsValdez = new BlankLocation()
        {

            Name = "SpanishLordsValdez",
            Description = "",
            EntrancePosition = new Vector3(1024.478f, 2296.503f, 36.70207f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
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
                            MaxHourSpawn = 22,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1024.478f, 2296.503f, 36.70207f),
                                            Heading = 262.1475f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(1024.104f, 2295.813f, 36.68025f),
                                            Heading = 239.2251f,
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
        BlankLocationPlaces.Add(SpanishLordsValdez);
        BlankLocation SpanishLordsCorner = new BlankLocation()
        {

            Name = "SpanishLordsCorner",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(997.8193f, 1973.162f, 13.5274f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
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
                            MaxHourSpawn = 24,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 3,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(994.3502f, 1963.395f, 15.85094f),
                                            Heading = 92.3622f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(991.045f, 1965.061f, 14.24833f),
                                            Heading = 90.06734f,
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
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
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
                                        Location = new Vector3(991.0338f, 1964.142f, 14.24833f),
                                            Heading = 71.95461f,
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
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
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
                                        Location = new Vector3(997.8193f, 1973.162f, 13.5274f),
                                            Heading = 271.2206f,
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
                                        Location = new Vector3(1016.665f, 1967.265f, 13.52401f),
                                            Heading = 3.723292f,
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
        BlankLocationPlaces.Add(SpanishLordsCorner);
        BlankLocation SpanishLordsPark = new BlankLocation()
        {

            Name = "SpanishLordsPark",
            Description = "",
            EntrancePosition = new Vector3(737.7163f, 2362.255f, 26.16826f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
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
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(737.7163f, 2362.255f, 26.16826f),
                                            Heading = 8.662255f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(738.4009f, 2362.346f, 26.18497f),
                                            Heading = 8.363561f,
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
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
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
        BlankLocationPlaces.Add(SpanishLordsPark);
        BlankLocation SpanishLordsLotus = new BlankLocation()
        {

            Name = "SpanishLordsLotus",
            Description = "",
            EntrancePosition = new Vector3(873.9929f, 2091.853f, 24.60303f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
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
                            MaxHourSpawn = 22,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(873.9929f, 2091.853f, 24.60303f),
                                            Heading = 178.2518f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(872.6816f, 2091.715f, 24.60303f),
                                            Heading = 185.8716f,
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
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
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
        BlankLocationPlaces.Add(SpanishLordsLotus);
        BlankLocation SpanishLordsBlocks3 = new BlankLocation()
        {

            Name = "SpanishLordsBlocks3",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(1192.597f, 2271.808f, 20.24261f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 10,
                            MaxHourSpawn = 23,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 3,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1189.522f, 2265.845f, 20.86401f),
                                            Heading = 219.3602f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(1190.097f, 2266.584f, 20.864f),
                                            Heading = 183.3793f,
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
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
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
                                        Location = new Vector3(1190.487f, 2264.962f, 20.86401f),
                                            Heading = 19.84033f,
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
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
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
                                        Location = new Vector3(1192.597f, 2271.808f, 20.24261f),
                                            Heading = 81.88879f,
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
                                            MinHourSpawn = 16,
                                            MaxHourSpawn = 24,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 3,




                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1201.701f, 2258.509f, 20.25892f),
                                            Heading = 226.1502f,
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
        BlankLocationPlaces.Add(SpanishLordsBlocks3);
        BlankLocation SpanishLordsBlocks4 = new BlankLocation()
        {

            Name = "SpanishLordsBlocks4",
            Description = "",
            EntrancePosition = new Vector3(1221.936f, 2202.096f, 16.84509f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 16,
                            MaxHourSpawn = 23,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1221.936f, 2202.096f, 16.84509f),
                                            Heading = 3.811977f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(1220.37f, 2202.122f, 16.84509f),
                                            Heading = 358.8205f,
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
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
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
        BlankLocationPlaces.Add(SpanishLordsBlocks4);
        BlankLocation SpanishLordsAutoShop = new BlankLocation()
        {

            Name = "SpanishLordsAutoShop",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(1205.91f, 2097.298f, 16.0931f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 16,
                            MaxHourSpawn = 23,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1213.079f, 2099.536f, 16.93636f),
                                            Heading = 34.94679f,
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
                                                "WORLD_HUMAN_GUARD_STAND_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(1214.007f, 2100.206f, 16.90004f),
                                            Heading = 37.67293f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(1218.448f, 2093.605f, 16.85694f),
                                            Heading = 342.1092f,
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
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
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
                                        Location = new Vector3(1217.275f, 2094.4f, 16.85593f),
                                            Heading = 325.0153f,
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
                                                "WORLD_HUMAN_CLIPBOARD_FACILITY",
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
                                        Location = new Vector3(1205.91f, 2097.298f, 16.0931f),
                                            Heading = 315.5219f,
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
                                        Location = new Vector3(1219.218f, 2097.713f, 16.29062f),
                                            Heading = 226.278f,
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
        BlankLocationPlaces.Add(SpanishLordsAutoShop);
        BlankLocation SpanishLordsHammer = new BlankLocation()
        {

            Name = "SpanishLordsHammer",
            Description = "",
            EntrancePosition = new Vector3(1144.372f, 2264.297f, 16.8431f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 10,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1144.372f, 2264.297f, 16.8431f),
                                            Heading = 136.5653f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(1145.168f, 2263.519f, 16.83316f),
                                            Heading = 138.1465f,
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
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
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
        BlankLocationPlaces.Add(SpanishLordsHammer);
        BlankLocation SpanishLordsAttica = new BlankLocation()
        {

            Name = "SpanishLordsAttica",
            Description = "",
            EntrancePosition = new Vector3(925.8395f, 2097.052f, 26.57742f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 10,
                            MaxHourSpawn = 18,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(925.8395f, 2097.052f, 26.57742f),
                                            Heading = 106.7113f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(925.9544f, 2096.078f, 26.56761f),
                                            Heading = 101.6826f,
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
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {},
                    },
                },
        };
        BlankLocationPlaces.Add(SpanishLordsAttica);
        BlankLocation SpanishLordsBlocks5 = new BlankLocation()
        {

            Name = "SpanishLordsBlocks5",
            Description = "",
            EntrancePosition = new Vector3(674.3111f, 1866.907f, 11.60816f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
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
                            MaxHourSpawn = 24,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(674.3111f, 1866.907f, 11.60816f),
                                            Heading = 319.6241f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(675.2188f, 1866.206f, 11.60816f),
                                            Heading = 327.9651f,
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
                            {},
                    },
                },
        };
        BlankLocationPlaces.Add(SpanishLordsBlocks5);

    }

    private void HolHustGang()
    {
        BlankLocation HustlersSprunk = new BlankLocation()
        {

            Name = "HustlersSprunk",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(933.8857f, 1957.303f, 14.8477f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            InteriorID = 172034,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_HOLHUST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 19,
                            MaxHourSpawn = 24,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(933.8857f, 1957.303f, 14.8477f),
                                            Heading = 337.5721f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(952.8941f, 1951.081f, 14.8477f),
                                            Heading = 21.77995f,
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
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
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
                                        Location = new Vector3(951.8707f, 1953.552f, 14.8477f),
                                            Heading = 191.2004f,
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
                                                "WORLD_HUMAN_DRINKING_FACILITY",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
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
                                        Location = new Vector3(950.3953f, 1952.807f, 14.8477f),
                                            Heading = 232.3982f,
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
                                                "WORLD_HUMAN_DRINKING_FACILITY",
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
                                        Location = new Vector3(942.9815f, 1962.527f, 16.24864f),
                                            Heading = 183.3846f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT",
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
                                        Location = new Vector3(925.7125f, 1945.713f, 14.50593f),
                                            Heading = 0.8748494f,
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
        }; //interior
        BlankLocationPlaces.Add(HustlersSprunk);
        BlankLocation HustlersBlock = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 10,
                            MaxHourSpawn = 18,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-272.9401f, 1960.713f, 18.86179f),
                                            Heading = 4.910076f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-273.9228f, 1960.727f, 18.86179f),
                                            Heading = 347.3505f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_POT",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-274.2355f, 1970.004f, 18.16135f),
                                        Heading = 180.378f,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_HOLHUST",
            Name = "HustlersBlock",
            Description = "",
            EntrancePosition = new Vector3(-274.2355f, 1970.004f, 18.16135f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(HustlersBlock);
        BlankLocation HustlersBlock2 = new BlankLocation()
        {

            Name = "HustlersBlock2",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(-251.7597f, 2014.311f, 18.1423f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_HOLHUST",
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
                            MaxHourSpawn = 24,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-260.7271f, 2024.446f, 18.862f),
                                            Heading = 249.6301f,
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
                                        Location = new Vector3(-258.894f, 2024.949f, 18.85401f),
                                            Heading = 101.2989f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(-252.2874f, 2022.956f, 18.80116f),
                                            Heading = 158.8806f,
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
                                            MaxWantedLevelSpawn = 3,




                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-252.8168f, 2021.451f, 18.80008f),
                                            Heading = 341.1935f,
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
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
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
                                        Location = new Vector3(-251.7597f, 2014.311f, 18.1423f),
                                            Heading = 160.3956f,
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
                                        Location = new Vector3(-246.4068f, 2021.306f, 18.15407f),
                                            Heading = 47.43872f,
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
        BlankLocationPlaces.Add(HustlersBlock2);
        BlankLocation HustlersBlock3 = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 24,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(158.5448f, 2010.774f, 18.83068f),
                                            Heading = 125.3245f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(159.2203f, 2009.65f, 18.83068f),
                                            Heading = 106.8842f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_POT",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(155.2973f, 2032.302f, 17.96054f),
                                        Heading = 266.9731f,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_HOLHUST",
            Name = "HustlersBlock3",
            Description = "",
            EntrancePosition = new Vector3(155.2973f, 2032.302f, 17.96054f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(HustlersBlock3);
        BlankLocation HustlersBlock4 = new BlankLocation()
        {

            Name = "HustlersBlock4",
            Description = "",
            EntrancePosition = new Vector3(114.6603f, 1988.504f, 18.43402f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_HOLHUST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 10,
                            MaxHourSpawn = 22,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(114.6603f, 1988.504f, 18.43402f),
                                            Heading = 146.2155f,
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
                                        Location = new Vector3(113.6372f, 1987.206f, 18.4385f),
                                            Heading = 308.1097f,
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
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
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
        BlankLocationPlaces.Add(HustlersBlock4);
        BlankLocation HustlersBBCourts = new BlankLocation()
        {

            Name = "HustlersBBCourts",
            Description = "",
            EntrancePosition = new Vector3(27.67035f, 1947.568f, 20.44954f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_HOLHUST",
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
                            MaxHourSpawn = 22,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(27.67035f, 1947.568f, 20.44954f),
                                            Heading = 89.55726f,
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
                                        Location = new Vector3(27.75593f, 1948.567f, 20.44954f),
                                            Heading = 91.50203f,
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
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
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
        BlankLocationPlaces.Add(HustlersBBCourts);
        BlankLocation HustlersTwat = new BlankLocation()
        {

            Name = "HustlersTwat",
            Description = "",
            EntrancePosition = new Vector3(-101.9269f, 1890.343f, 12.73699f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_HOLHUST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 10,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-101.9269f, 1890.343f, 12.73699f),
                                            Heading = 90.22813f,
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
                                        Location = new Vector3(-101.9837f, 1889.301f, 12.80247f),
                                            Heading = 95.24944f,
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
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
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
        BlankLocationPlaces.Add(HustlersTwat);
        BlankLocation HustlersBlock5 = new BlankLocation()
        {

            Name = "HustlersBlock5",
            Description = "",
            EntrancePosition = new Vector3(-242.8304f, 1748.691f, 17.47898f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_HOLHUST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 10,
                            MaxHourSpawn = 22,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-242.8304f, 1748.691f, 17.47898f),
                                            Heading = 270.7826f,
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
                                        Location = new Vector3(-242.8879f, 1749.353f, 17.47836f),
                                            Heading = 270.6313f,
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
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
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
        BlankLocationPlaces.Add(HustlersBlock5);
        BlankLocation HustlersPark = new BlankLocation()
        {

            Name = "HustlersPark",
            Description = "",
            EntrancePosition = new Vector3(-6.503558f, 1208.334f, 3.424065f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_HOLHUST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 10,
                            MaxHourSpawn = 18,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-6.503558f, 1208.334f, 3.424065f),
                                            Heading = 272.9309f,
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
                                        Location = new Vector3(-6.699773f, 1209.391f, 3.421892f),
                                            Heading = 273.3445f,
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
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
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
        BlankLocationPlaces.Add(HustlersPark);
        BlankLocation HustlersBankCorner = new BlankLocation()
        {

            Name = "HustlersBankCorner",
            Description = "",
            EntrancePosition = new Vector3(-145.7838f, 1683.436f, 14.7602f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_HOLHUST",
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
                            MaxHourSpawn = 22,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-145.7838f, 1683.436f, 14.7602f),
                                            Heading = 198.4298f,
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
                                        Location = new Vector3(-147.1679f, 1682.953f, 14.75968f),
                                            Heading = 237.7976f,
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
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
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
        BlankLocationPlaces.Add(HustlersBankCorner);
        BlankLocation HustlersBurger = new BlankLocation()
        {

            Name = "HustlersBurger",
            Description = "",
            EntrancePosition = new Vector3(-203.7681f, 1683.467f, 12.88448f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_HOLHUST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 14,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-203.7681f, 1683.467f, 12.88448f),
                                            Heading = 94.22587f,
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
                                        Location = new Vector3(-203.7875f, 1684.189f, 12.8854f),
                                            Heading = 94.18714f,
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
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
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
        BlankLocationPlaces.Add(HustlersBurger);
        BlankLocation HustlersStyle = new BlankLocation()
        {

            Name = "HustlersStyle",
            Description = "",
            EntrancePosition = new Vector3(-227.14f, 1889.685f, 15.47174f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_HOLHUST",
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
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-227.14f, 1889.685f, 15.47174f),
                                            Heading = 274.434f,
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
                                        Location = new Vector3(-227.1412f, 1890.754f, 15.47174f),
                                            Heading = 272.0949f,
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
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
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
        BlankLocationPlaces.Add(HustlersStyle);
        BlankLocation HustlersBlock6 = new BlankLocation()
        {

            Name = "HustlersBlock6",
            Description = "",
            EntrancePosition = new Vector3(-130.6612f, 2107.179f, 20.42315f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_HOLHUST",
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
                            MaxHourSpawn = 24,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-130.6612f, 2107.179f, 20.42315f),
                                            Heading = 210.2232f,
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
                                        Location = new Vector3(-129.8799f, 2107.711f, 20.42418f),
                                            Heading = 212.858f,
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
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
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
        BlankLocationPlaces.Add(HustlersBlock6);
        BlankLocation HustlersUnderBridge = new BlankLocation()
        {

            Name = "HustlersUnderBridge",
            Description = "",
            EntrancePosition = new Vector3(426.7823f, 1515.439f, 8.448785f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_HOLHUST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 10,
                            MaxHourSpawn = 22,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(426.7823f, 1515.439f, 8.448785f),
                                            Heading = 270.9568f,
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
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
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
                                        Location = new Vector3(426.7317f, 1514.082f, 8.448785f),
                                            Heading = 266.329f,
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
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
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
        BlankLocationPlaces.Add(HustlersUnderBridge);
        BlankLocation HustlersPark2 = new BlankLocation()
        {

            Name = "HustlersPark2",
            Description = "",
            EntrancePosition = new Vector3(-61.64191f, 1602.783f, 12.31144f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_HOLHUST",
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
                            MaxHourSpawn = 24,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-61.64191f, 1602.783f, 12.31144f),
                                            Heading = 288.2362f,
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
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
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
                                        Location = new Vector3(-61.45448f, 1601.588f, 12.37199f),
                                            Heading = 295.6778f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
        BlankLocationPlaces.Add(HustlersPark2);
    }

    private void AodGang()
    {
        BlankLocation AodDiner = new BlankLocation()
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
                                        Location = new Vector3(-156.9996f, 737.7203f, 14.23953f),
                                            Heading = 13.56679f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-155.4918f, 737.7178f, 14.36263f),
                                            Heading = 4.593013f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-162.1211f, 734.6867f, 13.27417f),
                                        Heading = 359.336f,
                                },
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-158.1828f, 743.571f, 13.45133f),
                                        Heading = 267.0504f,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_ANGELS",
            Name = "AodDiner",
            Description = "",
            EntrancePosition = new Vector3(-162.1211f, 734.6867f, 13.27417f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(AodDiner);
        BlankLocation AodCommunity = new BlankLocation()
        {

            Name = "AodCommunity",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(2032.62f, 1239.758f, 25.18448f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_ANGELS",
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
                            MaxHourSpawn = 24,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(2036.73f, 1242.742f, 25.70454f),
                                            Heading = 90.21704f,
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
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
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
                                        Location = new Vector3(-78.97172f, 2054.74f, 20.30567f),
                                            Heading = 160.4086f,
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
                                                "WORLD_HUMAN_DRINKING_CASINO_TERRACE",
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
                                        Location = new Vector3(-76.90724f, 2051.268f, 20.29469f),
                                            Heading = 89.77721f,
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
                                            MaxWantedLevelSpawn = 3,




                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-76.8615f, 2052.595f, 20.29335f),
                                            Heading = 96.04617f,
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
                                                "WORLD_HUMAN_STUPOR_CLUBHOUSE",
                                                "WORLD_HUMAN_STUPOR",
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
                                        Location = new Vector3(2032.62f, 1239.758f, 25.18448f),
                                            Heading = 0.4643726f,
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
                                        Location = new Vector3(2031.78f, 1244.731f, 25.17965f),
                                            Heading = 297.6171f,
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
                                        Location = new Vector3(2031.802f, 1247.436f, 25.18022f),
                                            Heading = 296.4234f,
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
        BlankLocationPlaces.Add(AodCommunity);
        BlankLocation AodLimbo = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 24,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-751.3274f, 2196.831f, 20.51253f),
                                            Heading = 89.9465f,
                                            MinHourSpawn = 18,
                                            MaxHourSpawn = 20,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-751.29f, 2197.56f, 20.51253f),
                                            Heading = 89.77574f,
                                            MinHourSpawn = 18,
                                            MaxHourSpawn = 20,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-754.8597f, 2182.453f, 19.78052f),
                                        Heading = 272.3572f,
                                        IsEmpty = false,
                                        MinHourSpawn = 20,
                                        MaxHourSpawn = 24,
                                },
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-754.8597f, 2182.453f, 19.78052f),
                                        Heading = 272.3572f,
                                        IsEmpty = false,
                                        MinHourSpawn = 20,
                                        MaxHourSpawn = 24,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_ANGELS",
            Name = "AodLimbo",
            Description = "",
            EntrancePosition = new Vector3(-754.8597f, 2182.453f, 19.78052f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(AodLimbo);
        BlankLocation AodScrap = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 16,
                            MaxHourSpawn = 22,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-751.3274f, 2196.831f, 20.51253f),
                                            Heading = 89.9465f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-751.29f, 2197.56f, 20.51253f),
                                            Heading = 89.77574f,
                                            MinHourSpawn = 0,
                                            MaxHourSpawn = 24,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-252.8676f, 854.4429f, 6.707807f),
                                        Heading = 349.2722f,
                                },
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-254.204f, 854.6855f, 6.714098f),
                                        Heading = 345.8728f,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_ANGELS",
            Name = "AodScrap",
            Description = "",
            EntrancePosition = new Vector3(-252.8676f, 854.4429f, 6.707807f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(AodScrap);
        BlankLocation AodScrap2 = new BlankLocation()
        {

            Name = "AodScrap2",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(1280.752f, 1154.906f, 26.01654f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_ANGELS",
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
                            MaxHourSpawn = 22,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1280.752f, 1154.906f, 26.01654f),
                                            Heading = 358.3793f,
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
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
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
                                        Location = new Vector3(1280.142f, 1151.425f, 26.32723f),
                                            Heading = 183.2842f,
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
                                                "WORLD_HUMAN_DRINKING_CASINO_TERRACE",
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
                                        Location = new Vector3(1280.097f, 1149.08f, 26.32723f),
                                            Heading = 359.2186f,
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
                                            MaxWantedLevelSpawn = 3,




                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1281.999f, 1148.853f, 26.32723f),
                                            Heading = 25.82615f,
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
                                                "WORLD_HUMAN_STUPOR_CLUBHOUSE",
                                                "WORLD_HUMAN_STUPOR",
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
                                        Location = new Vector3(1276.881f, 1158.206f, 25.4833f),
                                            Heading = 258.5092f,
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
                                        Location = new Vector3(1281.008f, 1159.129f, 25.48474f),
                                            Heading = 221.6832f,
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
        BlankLocationPlaces.Add(AodScrap2);










    }

    private void LostGang()
    {
        BlankLocation LostHonkersCP = new BlankLocation()
        {

            Name = "LostHonkersCP",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(-1376.096f, 458.3227f, 9.562305f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_LOST",
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
                            MaxHourSpawn = 24,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1379.702f, 448.3032f, 10.04338f),
                                            Heading = 268.2005f,
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
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
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
                                        Location = new Vector3(-1379.59f, 449.989f, 10.0434f),
                                            Heading = 250.9974f,
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
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
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
                                        Location = new Vector3(-1375.208f, 449.8713f, 10.04384f),
                                            Heading = 103.8994f,
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
                                                "WORLD_HUMAN_DRINKING_FACILITY",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
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
                                        Location = new Vector3(-1377.166f, 451.3575f, 10.04356f),
                                            Heading = 152.0862f,
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
                                                "WORLD_HUMAN_DRINKING_FACILITY",
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
                                        Location = new Vector3(-1375.968f, 446.5561f, 10.07577f),
                                            Heading = 44.6555f,
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
                                                "WORLD_HUMAN_DRINKING_CASINO_TERRACE",
                                                "WORLD_HUMAN_SMOKING_POT",
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
                                        Location = new Vector3(-1376.096f, 458.3227f, 9.562305f),
                                            Heading = 91.08441f,
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
                                        Location = new Vector3(-1376.029f, 455.1564f, 9.561684f),
                                            Heading = 91.0808f,
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
                                        Location = new Vector3(-1368.221f, 445.7012f, 9.562824f),
                                            Heading = 202.7643f,
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
                                        Location = new Vector3(-1365.285f, 447.1527f, 9.562803f),
                                            Heading = 203.0308f,
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
        BlankLocationPlaces.Add(LostHonkersCP);
        BlankLocation LostHardtack = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 12,
                            MaxHourSpawn = 23,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1828.149f, 660.0641f, 12.5407f),
                                            Heading = 348.4327f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1827.229f, 660.0388f, 12.54087f),
                                            Heading = 359.165f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-1825.443f, 665.3831f, 11.56585f),
                                        Heading = 182.3113f,
                                },
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-1822.229f, 664.2549f, 11.56537f),
                                        Heading = 184.7067f,
                                },
                            },
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_LOST",
            Name = "LostHardtack",
            Description = "",
            EntrancePosition = new Vector3(-1825.443f, 665.3831f, 11.56585f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(LostHardtack);
        BlankLocation LostBlock = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 12,
                            MaxHourSpawn = 18,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1844.208f, 538.6031f, 7.101818f),
                                            Heading = 91.03706f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1844.198f, 539.4734f, 7.103068f),
                                            Heading = 90.96889f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-1845.13f, 532.4124f, 6.635712f),
                                        Heading = 137.6688f,
                                },
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-1844.808f, 530.5572f, 6.652855f),
                                        Heading = 139.2734f,
                                },
                            },
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_LOST",
            Name = "LostBlock",
            Description = "",
            EntrancePosition = new Vector3(-1845.13f, 532.4124f, 6.635712f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(LostBlock);
        BlankLocation LostBlock2 = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 23,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1797.512f, 584.0159f, 6.809204f),
                                            Heading = 355.396f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_MUSICIAN",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1797.902f, 587.575f, 6.779629f),
                                            Heading = 178.8566f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1799.961f, 587.0161f, 6.788938f),
                                            Heading = 208.4347f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                    },
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1800.81f, 585.3819f, 6.800952f),
                                            Heading = 229.1242f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1796.119f, 586.7354f, 6.772004f),
                                            Heading = 155.9949f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_PARTYING",
                                                "WORLD_HUMAN_DRINKING_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1794.688f, 585.5372f, 6.770759f),
                                            Heading = 126.4429f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_DRINKING",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-1797.469f, 580.9626f, 6.299311f),
                                        Heading = 4.019263f,
                                },
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-1791.802f, 586.0363f, 6.227337f),
                                        Heading = 92.16045f,
                                },
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-1798.026f, 591.009f, 6.215022f),
                                        Heading = 185.919f,
                                },
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-1802.87f, 585.7045f, 6.264699f),
                                        Heading = 268.9051f,
                                },
                            },
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_LOST",
            Name = "LostBlock2",
            Description = "",
            EntrancePosition = new Vector3(-1797.469f, 580.9626f, 6.299311f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(LostBlock2);
        BlankLocation LostDiner = new BlankLocation()
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
                                        Location = new Vector3(1190.216f, 936.6941f, 15.66362f),
                                            Heading = 340.5001f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1189.235f, 936.8538f, 15.62122f),
                                            Heading = 342.7975f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(1189.576f, 941.447f, 15.01071f),
                                        Heading = 140.0037f,
                                },
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(1192.236f, 939.1833f, 15.18274f),
                                        Heading = 144.5579f,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_LOST",
            Name = "LostDiner",
            Description = "",
            EntrancePosition = new Vector3(1189.576f, 941.447f, 15.01071f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(LostDiner);
        BlankLocation LostBlock3 = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 12,
                            MaxHourSpawn = 24,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1633.952f, 763.5078f, 22.95208f),
                                            Heading = 2.493968f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER",
                                                "WORLD_HUMAN_DRINKING_CASINO_TERRACE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1634.798f, 763.4322f, 22.95276f),
                                            Heading = 358.4658f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRINKING_FACILITY",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(-1626.995f, 768.0521f, 22.3841f),
                                        Heading = 91.32539f,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_LOST",
            Name = "LostBlock3",
            Description = "",
            EntrancePosition = new Vector3(-1626.995f, 768.0521f, 22.3841f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(LostBlock3);
        BlankLocation LostHouse = new BlankLocation()
        {

            Name = "LostHouse",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(-1344.081f, 843.863f, 24.2707f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_LOST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = -1f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = -1f,
                            MinHourSpawn = 16,
                            MaxHourSpawn = 24,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1346.079f, 843.9521f, 24.87172f),
                                            Heading = 42.96614f,
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
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
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
                                        Location = new Vector3(-1347.841f, 845.7986f, 25.158f),
                                            Heading = 217.0836f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(-1351.606f, 847.7762f, 25.45008f),
                                            Heading = 186.2756f,
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
                                                "WORLD_HUMAN_DRINKING_CASINO_TERRACE",
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
                                        Location = new Vector3(-1347.737f, 847.2216f, 25.3681f),
                                            Heading = 202.3455f,
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
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
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
                                        Location = new Vector3(-1344.081f, 843.863f, 24.2707f),
                                            Heading = 1.033443f,
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
        BlankLocationPlaces.Add(LostHouse);
        BlankLocation LostAlley = new BlankLocation()
        {

            Name = "LostAlley",
            Description = "",
            EntrancePosition = new Vector3(-1359.828f, 867.1675f, 25.49144f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_LOST",
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
                            MaxHourSpawn = 22,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-1359.828f, 867.1675f, 25.49144f),
                                            Heading = 92.13306f,
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
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
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
                                        Location = new Vector3(-1361.63f, 867.2947f, 25.49144f),
                                            Heading = 267.7621f,
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
                                                "WORLD_HUMAN_DRINKING_FACILITY",
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
        BlankLocationPlaces.Add(LostAlley);

    }

    private void UptownGang()
    {
        BlankLocation UptownCowboy = new BlankLocation()
        {

            Name = "UptownCowboy",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(-79.64777f, 2052.8f, 19.63912f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

            AssignedAssociationID = "AMBIENT_GANG_UPTOWN",
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
                            MaxHourSpawn = 24,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-80.69177f, 2054.08f, 20.28928f),
                                            Heading = 213.0483f,
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
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(-78.97172f, 2054.74f, 20.30567f),
                                            Heading = 160.4086f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(-79.81969f, 2052.173f, 19.63668f),
                                            Heading = 297.8694f,
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
                                        Location = new Vector3(-79.10217f, 2049.97f, 19.62909f),
                                            Heading = 294.9358f,
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
        BlankLocationPlaces.Add(UptownCowboy);
        BlankLocation UptownBlock = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 12,
                            MaxHourSpawn = 24,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(96.16191f, 2139.752f, 20.56667f),
                                            Heading = 267.9662f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(97.5478f, 2139.422f, 20.56667f),
                                            Heading = 81.44434f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_DRUG_DEALER",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(96.45052f, 2133.375f, 19.76387f),
                                        Heading = 29.50385f,
                                },
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(98.98656f, 2133.462f, 19.92022f),
                                        Heading = 27.5295f,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_UPTOWN",
            Name = "UptownBlock",
            Description = "",
            EntrancePosition = new Vector3(96.45052f, 2133.375f, 19.76387f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(UptownBlock);
        BlankLocation UptownBlock2 = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 12,
                            MaxHourSpawn = 24,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(94.12988f, 2165.436f, 18.7542f),
                                            Heading = 3.855219f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(93.07771f, 2165.333f, 18.7542f),
                                            Heading = 3.675136f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_DRUG_DEALER",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(131.116f, 2165.851f, 17.98447f),
                                        Heading = 357.8248f,
                                    IsEmpty = false,
                                            MinHourSpawn = 22,
                                            MaxHourSpawn = 24,
                                },
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(133.7932f, 2165.439f, 18.03648f),
                                        Heading = 1.011264f,
                                    IsEmpty = false,
                                            MinHourSpawn = 22,
                                            MaxHourSpawn = 24,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_UPTOWN",
            Name = "UptownBlock2",
            Description = "",
            EntrancePosition = new Vector3(94.12988f, 2165.436f, 18.7542f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(UptownBlock2);









    }

    private void YardiesGang()
    {
        BlankLocation YardiesBumbaclot = new BlankLocation()
        {
            Name = "YardiesBumbaclot",
            FullName = "",
            Description = "Yardies behind Bumbaclots",
            EntrancePosition = new Vector3(1626.656f, 557.8749f, 25.58451f),
            OpenTime = 0,
            CloseTime = 24,
            StateID = "Liberty",
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            MenuID = "",
            PossibleGroupSpawns = new List<ConditionalGroup>() {
            new ConditionalGroup() {
            Name = "",
            Percentage = defaultSpawnPercentage,
            MinHourSpawn = 10,
            MaxHourSpawn = 16,
            PossiblePedSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1630.266f,557.313f,25.90625f),
            Heading = 168.2886f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            },
            },
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1626.656f,557.8749f,25.58451f),
            Heading = -25.60437f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            ForcedScenarios = new List<String>() {
            },
            },
            },
            },
            new ConditionalGroup() {
            Name = "",
            Percentage = defaultSpawnPercentage,
            MinHourSpawn = 16,
            MaxHourSpawn = 23,
            PossiblePedSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1630.266f,557.313f,25.90625f),
            Heading = 168.2886f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_DRINKING",
            "WORLD_HUMAN_SMOKING",
            },
            },
            new GangConditionalLocation() {
            Location = new Vector3(1628.937f,556.2213f,25.90625f),
            Heading = -39.99989f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_STAND_IMPATIENT_FACILITY",
            "WORLD_HUMAN_HANG_OUT_STREET",
            "WORLD_HUMAN_STAND_MOBILE",
            },
            },
            new GangConditionalLocation() {
            Location = new Vector3(1629.584f,555.275f,25.90607f),
            Heading = -19.42001f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_STAND_IMPATIENT_FACILITY",
            "WORLD_HUMAN_HANG_OUT_STREET",
            "WORLD_HUMAN_STAND_MOBILE",
            },
            },
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1626.656f,557.8749f,25.58451f),
            Heading = 179.6279f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            ForcedScenarios = new List<String>() {
            },
            },
            },
            },
            },
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
            },
            VendorLocations = new List<SpawnPlace>()
            {
            },
            VendorHeadDataGroupID = "",
            VehiclePreviewLocation = new SpawnPlace()
            {
            },
            VehicleDeliveryLocations = new List<SpawnPlace>()
            {
            },
        };
        BlankLocationPlaces.Add(YardiesBumbaclot);
        BlankLocation YardiesStreets = new BlankLocation()
        {
            Name = "YardiesStreets",
            FullName = "",
            Description = "Yardies Street Setup",
            EntrancePosition = new Vector3(1600.437f, 540.2205f, 32.23283f),
            OpenTime = 0,
            CloseTime = 24,
            StateID = "Liberty",
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            MenuID = "",
            PossibleGroupSpawns = new List<ConditionalGroup>() {
            new ConditionalGroup() {
            Name = "",
            Percentage = defaultSpawnPercentage,
            MinHourSpawn = 10,
            MaxHourSpawn = 18,
            PossiblePedSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1600.437f,540.2205f,32.23283f),
            Heading = -173.8112f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_LEANING_CASINO_TERRACE",
            "WORLD_HUMAN_SMOKING",
            },
            },
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
            },
            },
            new ConditionalGroup() {
            Name = "",
            Percentage = defaultSpawnPercentage,
            MinHourSpawn = 18,
            MaxHourSpawn = 23,
            PossiblePedSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1600.437f,540.2205f,32.23283f),
            Heading = -173.8112f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_LEANING_CASINO_TERRACE",
            "WORLD_HUMAN_SMOKING",
            },
            },
            new GangConditionalLocation() {
            Location = new Vector3(1601.36f,539.939f,32.22181f),
            Heading = 154.9978f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_STAND_IMPATIENT_FACILITY",
            "WORLD_HUMAN_HANG_OUT_STREET",
            "WORLD_HUMAN_STAND_MOBILE",
            },
            },
            new GangConditionalLocation() {
            Location = new Vector3(1600.605f,539.003f,32.2259f),
            Heading = -9.000006f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_STAND_IMPATIENT_FACILITY",
            "WORLD_HUMAN_HANG_OUT_STREET",
            "WORLD_HUMAN_STAND_MOBILE",
            },
            },
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
            },
            },
            },
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
            },
            VendorLocations = new List<SpawnPlace>()
            {
            },
            VendorHeadDataGroupID = "",
            VehiclePreviewLocation = new SpawnPlace()
            {
            },
            VehicleDeliveryLocations = new List<SpawnPlace>()
            {
            },
        };
        BlankLocationPlaces.Add(YardiesStreets);
        BlankLocation YardiesStreets2 = new BlankLocation()
        {
            Name = "YardiesStreets2",
            FullName = "",
            Description = "Yardies Street Setup2",
            EntrancePosition = new Vector3(1515.371f, 561.4121f, 38.10307f),
            OpenTime = 0,
            CloseTime = 24,
            StateID = "Liberty",
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            MenuID = "",
            PossibleGroupSpawns = new List<ConditionalGroup>() {
            new ConditionalGroup() {
            Name = "",
            Percentage = defaultSpawnPercentage,
            MinHourSpawn = 10,
            MaxHourSpawn = 18,
            PossiblePedSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1515.371f,561.4121f,38.10307f),
            Heading = -129.4167f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_HANG_OUT_STREET",
            "WORLD_HUMAN_STAND_MOBILE",
            },
            },
            new GangConditionalLocation() {
            Location = new Vector3(1516.403f,561.0484f,38.0646f),
            Heading = 83.4598f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_STAND_IMPATIENT_FACILITY",
            "WORLD_HUMAN_HANG_OUT_STREET",
            },
            },
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
            },
            },
            new ConditionalGroup() {
            Name = "",
            Percentage = defaultSpawnPercentage,
            MinHourSpawn = 18,
            MaxHourSpawn = 23,
            PossiblePedSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1515.24f,559.6724f,37.96181f),
            Heading = -129.174f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_HANG_OUT_STREET",
            "WORLD_HUMAN_STAND_MOBILE",
            },
            },
            new GangConditionalLocation() {
            Location = new Vector3(1516.196f,559.0659f,37.90246f),
            Heading = 83.46609f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_STAND_IMPATIENT_FACILITY",
            "WORLD_HUMAN_STAND_MOBILE",
            },
            },
            new GangConditionalLocation() {
            Location = new Vector3(1515.037f,558.7992f,37.89363f),
            Heading = -82.79473f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
            "WORLD_HUMAN_DRINKING_CASINO_TERRACE",
            },
            },
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1510.477f,558.7837f,37.56691f),
            Heading = 0.01087305f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            ForcedScenarios = new List<String>() {
            },
            },
            },
            },
            },
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
            },
            VendorLocations = new List<SpawnPlace>()
            {
            },
            VendorHeadDataGroupID = "",
            VehiclePreviewLocation = new SpawnPlace()
            {
            },
            VehicleDeliveryLocations = new List<SpawnPlace>()
            {
            },
        };
        BlankLocationPlaces.Add(YardiesStreets2);
        BlankLocation YardiesStreets3 = new BlankLocation()
        {
            Name = "YardiesStreets3",
            FullName = "",
            Description = "Yardies Street Setup3",
            EntrancePosition = new Vector3(1703.692f, 679.4326f, 24.55261f),
            OpenTime = 0,
            CloseTime = 24,
            StateID = "Liberty",
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            MenuID = "",
            PossibleGroupSpawns = new List<ConditionalGroup>() {
            new ConditionalGroup() {
            Name = "",
            Percentage = defaultSpawnPercentage,
            MinHourSpawn = 12,
            MaxHourSpawn = 20,
            PossiblePedSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1714.549f,676.8549f,23.08803f),
            Heading = -90.89314f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_HANG_OUT_STREET",
            "WORLD_HUMAN_STAND_MOBILE",
            },
            },
            new GangConditionalLocation() {
            Location = new Vector3(1714.568f,676.0766f,23.00532f),
            Heading = -67.1198f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_STAND_IMPATIENT_FACILITY",
            "WORLD_HUMAN_STAND_MOBILE",
            },
            },
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1703.692f,679.4326f,24.55261f),
            Heading = 103.0923f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            ForcedScenarios = new List<String>() {
            },
            },
            },
            },
            },
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
            },
            VendorLocations = new List<SpawnPlace>()
            {
            },
            VendorHeadDataGroupID = "",
            VehiclePreviewLocation = new SpawnPlace()
            {
            },
            VehicleDeliveryLocations = new List<SpawnPlace>()
            {
            },
        };
        BlankLocationPlaces.Add(YardiesStreets3);
        BlankLocation YardiesStreets4 = new BlankLocation()
        {
            Name = "YardiesStreets4",
            FullName = "",
            Description = "Yardies Street Setup4",
            EntrancePosition = new Vector3(1769.87f, 631.3967f, 22.80543f),
            OpenTime = 0,
            CloseTime = 24,
            StateID = "Liberty",
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            MenuID = "",
            PossibleGroupSpawns = new List<ConditionalGroup>() {
            new ConditionalGroup() {
            Name = "",
            Percentage = defaultSpawnPercentage,
            MinHourSpawn = 18,
            MaxHourSpawn = 24,
            PossiblePedSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1773.581f,632.506f,23.12693f),
            Heading = 104.7972f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_HANG_OUT_STREET",
            "WORLD_HUMAN_STAND_MOBILE",
            },
            },
            new GangConditionalLocation() {
            Location = new Vector3(1773.527f,631.49f,23.12693f),
            Heading = 82.25832f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_STAND_IMPATIENT_FACILITY",
            "WORLD_HUMAN_STAND_MOBILE",
            },
            },
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(1769.87f,631.3967f,22.80543f),
            Heading = -164.1557f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            ForcedScenarios = new List<String>() {
            },
            },
            },
            },
            },
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
            },
            VendorLocations = new List<SpawnPlace>()
            {
            },
            VendorHeadDataGroupID = "",
            VehiclePreviewLocation = new SpawnPlace()
            {
            },
            VehicleDeliveryLocations = new List<SpawnPlace>()
            {
            },
        };
        BlankLocationPlaces.Add(YardiesStreets4);
        // EndOfStreamException of xml converts
        BlankLocation YardiesAlley1 = new BlankLocation()
        {

            Name = "YardiesAlley1",
            FullName = "",
            Description = "",
            MapIcon = 78,
            MapIconScale = 1f,
            EntrancePosition = new Vector3(2012.652f, 1014.228f, 28.62987f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = "Liberty",

            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            MenuID = "",

            PossibleGroupSpawns =
              new List<ConditionalGroup>() {
        new ConditionalGroup() {
          Name = "",
          Percentage = defaultSpawnPercentage,
          MinHourSpawn = 10,
          MaxHourSpawn = 22,
          PossiblePedSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(2017.166f, 1012.159f, 28.62671f),
                  Heading = 93.53442f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                        "WORLD_HUMAN_DRUG_DEALER_HARD",
                      },
                },
                new GangConditionalLocation() {
                  Location = new Vector3(2015.703f, 1012.086f, 28.71803f),
                  Heading = 282.9243f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_HANG_OUT_STREET",
                        "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                      },
                },
              },
          PossibleVehicleSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(2012.652f, 1014.228f, 28.62987f),
                  Heading = 316.3719f,
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
        BlankLocationPlaces.Add(YardiesAlley1);
        BlankLocation YardiesStreet5 = new BlankLocation()
        {

            Name = "YardiesStreet5",
            FullName = "",
            Description = "",
            MapIcon = 78,
            MapIconScale = 1f,
            EntrancePosition = new Vector3(2062.486f, 898.504f, 26.46866f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = "Liberty",

            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            MenuID = "",

            PossibleGroupSpawns =
          new List<ConditionalGroup>() {
        new ConditionalGroup() {
          Name = "",
          Percentage = defaultSpawnPercentage,
          MinHourSpawn = 16,
          MaxHourSpawn = 24,
          PossiblePedSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(2063.646f, 903.1901f, 26.78606f),
                  Heading = 94.53841f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                        "WORLD_HUMAN_DRUG_DEALER_HARD",
                      },
                },
                new GangConditionalLocation() {
                  Location = new Vector3(2063.631f, 902.1149f, 26.7692f),
                  Heading = 82.53756f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_HANG_OUT_STREET",
                        "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                      },
                },
              },
          PossibleVehicleSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(2062.486f, 898.504f, 26.46866f),
                  Heading = 270.4164f,
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
        BlankLocationPlaces.Add(YardiesStreet5);
        BlankLocation YardiesAlley2 = new BlankLocation()
        {

            Name = "YardiesAlley2",
            FullName = "",
            Description = "",
            MapIcon = 78,
            MapIconScale = 1f,
            EntrancePosition = new Vector3(1926.702f, 1134.882f, 28.58441f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = "Liberty",

            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
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
                  Location = new Vector3(1930.279f, 1135.17f, 29.0173f),
                  Heading = 93.33098f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                        "WORLD_HUMAN_DRUG_DEALER_HARD",
                      },
                },
                new GangConditionalLocation() {
                  Location = new Vector3(1930.355f, 1136.531f, 29.00263f),
                  Heading = 120.6347f,
                  AssociationID = "",
                  RequiredPedGroup = "",
                  RequiredVehicleGroup = "",
                  TaskRequirements = TaskRequirements.Guard,
                  ForcedScenarios =
                      new List<String>() {
                        "WORLD_HUMAN_HANG_OUT_STREET",
                        "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                      },
                },
              },
          PossibleVehicleSpawns =
              new List<ConditionalLocation>() {
                new GangConditionalLocation() {
                  Location = new Vector3(1926.702f, 1134.882f, 28.58441f),
                  Heading = 178.8307f,
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
        BlankLocationPlaces.Add(YardiesAlley2);
        BlankLocation YardiesBurgershot = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 12,
                            MaxHourSpawn = 24,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1865.978f, 726.0427f, 22.37967f),
                                            Heading = 91.44577f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1865.95f, 727.2113f, 22.37967f),
                                            Heading = 92.96597f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRINKING",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(1861.01f, 725.2467f, 21.54011f),
                                        Heading = 272.5944f,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            Name = "YardiesBurgershot",
            Description = "",
            EntrancePosition = new Vector3(1861.01f, 725.2467f, 21.54011f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(YardiesBurgershot);
        BlankLocation YardiesAlPizza = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 24,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1674.01f, 718.6381f, 26.07837f),
                                            Heading = 182.6806f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1674.806f, 718.4825f, 26.03683f),
                                            Heading = 178.369f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_POT",
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(1676.551f, 715.4472f, 25.18301f),
                                        Heading = 91.5689f,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            Name = "YardiesAlPizza",
            Description = "",
            EntrancePosition = new Vector3(1676.551f, 715.4472f, 25.18301f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(YardiesAlPizza);
        BlankLocation YardiesBlock = new BlankLocation()
        {

            Name = "YardiesBlock",
            Description = "",
            EntrancePosition = new Vector3(1785.33f, 713.1031f, 24.2921f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 14,
                            MaxHourSpawn = 23,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1785.33f, 713.1031f, 24.2921f),
                                            Heading = 269.1364f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(1785.336f, 712.3278f, 24.29296f),
                                            Heading = 272.5494f,
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
                            {},
                    },
                },
        };
        BlankLocationPlaces.Add(YardiesBlock);
        BlankLocation YardiesBlock2 = new BlankLocation()
        {

            Name = "YardiesBlock2",
            Description = "",
            EntrancePosition = new Vector3(1768.689f, 665.1475f, 23.01879f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 10,
                            MaxHourSpawn = 18,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1768.689f, 665.1475f, 23.01879f),
                                            Heading = 358.9816f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(1767.835f, 665.2197f, 23.01879f),
                                            Heading = 356.9689f,
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
                            {},
                    },
                },
        };
        BlankLocationPlaces.Add(YardiesBlock2);
        BlankLocation YardiesStore = new BlankLocation()
        {

            Name = "YardiesStore",
            Description = "",
            EntrancePosition = new Vector3(1618.201f, 615.1682f, 31.26449f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,

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
                            MinHourSpawn = 12,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 6,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1618.201f, 615.1682f, 31.26449f),
                                            Heading = 64.86868f,
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
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
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
                                        Location = new Vector3(1618.815f, 615.9364f, 31.26449f),
                                            Heading = 62.36359f,
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
                            {},
                    },
                },
        };
        BlankLocationPlaces.Add(YardiesStore);
        BlankLocation YardiesEarpSt = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 12,
                            MaxHourSpawn = 24,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1460.539f, 523.9868f, 35.5916f),
                                            Heading = 271.2274f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1460.486f, 524.8422f, 35.59051f),
                                            Heading = 273.0463f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_POT",
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(1463.883f, 525.3993f, 34.91597f),
                                        Heading = 180.7651f,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            Name = "YardiesEarpSt",
            Description = "",
            EntrancePosition = new Vector3(1463.883f, 525.3993f, 34.91597f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(YardiesEarpSt);
        BlankLocation YardiesBlock3 = new BlankLocation()
        {
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 16,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1744.536f, 737.4148f, 24.30181f),
                                            Heading = 0.7416951f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(1743.282f, 737.4814f, 24.30298f),
                                            Heading = 1.621169f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_POT",
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(1749.632f, 759.1354f, 24.24877f),
                                        Heading = 259.8718f,
                                },
                            }
                    }
                },
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            Name = "YardiesBlock3",
            Description = "",
            EntrancePosition = new Vector3(1749.632f, 759.1354f, 24.24877f),
            EntranceHeading = 0f,
        };
        BlankLocationPlaces.Add(YardiesBlock3);

    }
}

