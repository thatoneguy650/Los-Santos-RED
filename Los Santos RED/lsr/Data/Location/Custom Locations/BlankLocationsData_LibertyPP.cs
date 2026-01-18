using Rage;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BlankLocationsData_LibertyPP
{
    private float defaultSpawnPercentage = 65;
    public BlankLocationsData_LibertyPP()
    {

    }
    public List<BlankLocation> BlankLocationPlaces { get; set; } = new List<BlankLocation>();

    public void DefaultConfig()
    {
        Checkpoints();
        OtherCops();
        RooftopSnipers();
        SpeedTraps();

        RaceMeetPeds();
        RandomPeds();

        AodGang();
        LostGang();
        UptownGang();

        AncelottinGang();
        GambettiGang();
        LupisellaGang();
        MessinaGang();
        PavanoGang();
        PetrovicGang();

        KhangpaeGang();
        TriadGang();

        HolHustGang();
        SpanishLordsGang();
        YardiesGang();
    }

    // vehicle types - ImportExportVehicles , HighEndVehicles,

    //  ------- Civilian Section --------

    private void RaceMeetPeds()
    {
        float defaultSpawnPercentage = 75f;
        BlankLocation lcracersspawn1 = new BlankLocation(new Vector3(6540.482f, -2346.8562f, 13.8257923f), 139.1308f, "lcracersspawn1", "")
        {
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateCells = 3,
            ActivateDistance = 75f,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new CivilianConditionalLocation(new Vector3(6538.14258f, -2347.73218f, 13.8131628f), 293.9003f, defaultSpawnPercentage) { OverrideDispatchablePersonGroupID = "VehicleRacePeds", TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>{ "WORLD_HUMAN_HANG_OUT_STREET", "WORLD_HUMAN_STAND_MOBILE" } },
                new CivilianConditionalLocation(new Vector3(6539.044f, -2348.75537f, 13.8216925f), 317.723f, defaultSpawnPercentage) { OverrideDispatchablePersonGroupID = "VehicleRacePeds", TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>{ "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE", "WORLD_HUMAN_STAND_MOBILE_UPRIGHT_CLUBHOUSE" } },
                new CivilianConditionalLocation(new Vector3(6540.482f, -2346.8562f, 13.8257923f), 139.1308f, defaultSpawnPercentage) { OverrideDispatchablePersonGroupID = "VehicleRacePeds", TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>{ "WORLD_HUMAN_LEANING_CASINO_TERRACE", "WORLD_HUMAN_MUSICIAN" } },
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new CivilianConditionalLocation(new Vector3(6539.507f, -2343.77515f, 13.198493f), 320.3771f, defaultSpawnPercentage) { OverrideDispatchableVehicleGroupID = "SportsCars_Racing" },
                new CivilianConditionalLocation(new Vector3(6532.839f, -2349.21216f, 13.0741425f), 358.1031f, defaultSpawnPercentage) { OverrideDispatchableVehicleGroupID = "SuperCars_Racing" },
            },
        };
        BlankLocationPlaces.Add(lcracersspawn1);

        BlankLocation lcracersspawn2 = new BlankLocation(new Vector3(4978.57959f, -2066.58716f, 14.6191921f), 2.980924f, "lcracersspawn2", "")
        {
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateCells = 3,
            ActivateDistance = 75f,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new CivilianConditionalLocation(new Vector3(4978.57959f, -2066.58716f, 14.6191921f), 2.980924f, defaultSpawnPercentage) { OverrideDispatchablePersonGroupID = "VehicleRacePeds", TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>{ "WORLD_HUMAN_HANG_OUT_STREET", "WORLD_HUMAN_STAND_MOBILE_UPRIGHT_CLUBHOUSE" } },
                new CivilianConditionalLocation(new Vector3(4979.556f, -2066.04614f, 14.4695129f), 41.70705f, defaultSpawnPercentage) { OverrideDispatchablePersonGroupID = "VehicleRacePeds", TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>{ "WORLD_HUMAN_HANG_OUT_STREET", "WORLD_HUMAN_STAND_MOBILE" } },
                new CivilianConditionalLocation(new Vector3(4972.92236f, -2065.61133f, 14.6191921f), 271.1073f, defaultSpawnPercentage) { OverrideDispatchablePersonGroupID = "VehicleRacePeds", TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>{ "WORLD_HUMAN_LEANING_CASINO_TERRACE", "WORLD_HUMAN_MUSICIAN" } },
                new CivilianConditionalLocation(new Vector3(4973.227f, -2064.474f, 14.6192026f), 216.5524f, defaultSpawnPercentage) { OverrideDispatchablePersonGroupID = "VehicleRacePeds", TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>{ "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE", "WORLD_HUMAN_STAND_MOBILE_FACILITY" } },
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new CivilianConditionalLocation(new Vector3(4976.38f, -2061.3252f, 13.7626429f), 271.5317f, defaultSpawnPercentage) { OverrideDispatchableVehicleGroupID = "SportsCars_Racing" },
                new CivilianConditionalLocation(new Vector3(4976.70166f, -2057.94336f, 13.8849325f), 91.17236f, defaultSpawnPercentage) { OverrideDispatchableVehicleGroupID = "SuperCars_Racing" },
            },
        };
        BlankLocationPlaces.Add(lcracersspawn2);

        BlankLocation lcracersspawn3 = new BlankLocation(new Vector3(7477.578f, -2907.88184f, 6.09556627f), 330.3031f, "lcracersspawn3", "")
        {
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateCells = 3,
            ActivateDistance = 75f,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new CivilianConditionalLocation(new Vector3(7477.578f, -2907.88184f, 6.09556627f), 330.3031f, defaultSpawnPercentage) { OverrideDispatchablePersonGroupID = "VehicleRacePeds", TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>{ "WORLD_HUMAN_HANG_OUT_STREET", "WORLD_HUMAN_STAND_MOBILE" } },
                new CivilianConditionalLocation(new Vector3(7478.734f, -2905.72656f, 6.094832f), 142.0916f, defaultSpawnPercentage) { OverrideDispatchablePersonGroupID = "VehicleRacePeds", TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>{ "WORLD_HUMAN_HANG_OUT_STREET", "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE" } },
                new CivilianConditionalLocation(new Vector3(7477.569f, -2905.27246f, 6.09598f), 172.0269f, defaultSpawnPercentage) { OverrideDispatchablePersonGroupID = "VehicleRacePeds", TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>{ "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE", "WORLD_HUMAN_STAND_MOBILE_FACILITY" } },
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new CivilianConditionalLocation(new Vector3(7476.874f, -2911.21924f, 5.348624f), 91.80183f, defaultSpawnPercentage) { OverrideDispatchableVehicleGroupID = "SportsCars_Racing" },
                new CivilianConditionalLocation(new Vector3(7476.487f, -2914.11279f, 5.34708929f), 272.0292f, defaultSpawnPercentage) { OverrideDispatchableVehicleGroupID = "SuperCars_Racing" },
            },
        };
        BlankLocationPlaces.Add(lcracersspawn3);

        BlankLocation lcracersspawn4 = new BlankLocation(new Vector3(3654.4978f, -2143.808f, 26.692934f), 58.73107f, "lcracersspawn4", "")
        {
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateCells = 3,
            ActivateDistance = 75f,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new CivilianConditionalLocation(new Vector3(3654.4978f, -2143.808f, 26.692934f), 58.73107f, defaultSpawnPercentage) { OverrideDispatchablePersonGroupID = "VehicleRacePeds", TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>{ "WORLD_HUMAN_HANG_OUT_STREET", "WORLD_HUMAN_STAND_MOBILE" } },
                new CivilianConditionalLocation(new Vector3(3652.55371f, -2143.312f, 26.6922035f), 253.594f, defaultSpawnPercentage) { OverrideDispatchablePersonGroupID = "VehicleRacePeds", TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>{ "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE", "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE" } },
                new CivilianConditionalLocation(new Vector3(3653.36377f, -2142.44531f, 26.692934f), 230.4179f, defaultSpawnPercentage) { OverrideDispatchablePersonGroupID = "VehicleRacePeds", TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>{ "WORLD_HUMAN_LEANING_CASINO_TERRACE", "WORLD_HUMAN_MUSICIAN" } },
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new CivilianConditionalLocation(new Vector3(3659.23975f, -2144.21313f, 25.9363728f), 14.57801f, defaultSpawnPercentage) { OverrideDispatchableVehicleGroupID = "SportsCars_Racing" },
                new CivilianConditionalLocation(new Vector3(3656.0918f, -2148.8772f, 26.1078644f), 21.5986f, defaultSpawnPercentage) { OverrideDispatchableVehicleGroupID = "SuperCars_Racing" },
            },
        };
        BlankLocationPlaces.Add(lcracersspawn4);
    }
    private void RandomPeds()
    {
        float defaultCarSpawnPercentage = 25f;
        BlankLocation lccivspawn1 = new BlankLocation(new Vector3(6549.89453f, -3946.657f, 7.796528f), 354.7676f, "lccivspawn1", "")
        {
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateCells = 3,
            ActivateDistance = 75f,
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new CivilianConditionalLocation(new Vector3(6550.541f, -3941.54517f, 7.093464f), 280.9801f, defaultCarSpawnPercentage) { OverrideDispatchableVehicleGroupID = "HighEndVehicles" },
            },
        };
        BlankLocationPlaces.Add(lccivspawn1);

        BlankLocation lccivspawn2 = new BlankLocation(new Vector3(6578.738f, -2679.02515f, 32.6590233f), 270.7252f, "lccivspawn2", "")
        {
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateCells = 3,
            ActivateDistance = 75f,
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new CivilianConditionalLocation(new Vector3(6577.031f, -2682.91919f, 30.8715343f), 92.10503f, defaultCarSpawnPercentage) { OverrideDispatchableVehicleGroupID = "HighEndVehicles" },
            },
        };
        BlankLocationPlaces.Add(lccivspawn2);

        BlankLocation lccivspawn3 = new BlankLocation(new Vector3(6813.0625f, -2564.955f, 28.9775639f), 4.027365f, "lccivspawn3", "")
        {
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateCells = 3,
            ActivateDistance = 75f,
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new CivilianConditionalLocation(new Vector3(6813.5376f, -2560.58936f, 28.4308033f), 91.05238f, defaultCarSpawnPercentage) { OverrideDispatchableVehicleGroupID = "HighEndVehicles" },
                new CivilianConditionalLocation(new Vector3(6822.39063f, -2560.42432f, 27.9382133f), 91.03718f, defaultCarSpawnPercentage) { OverrideDispatchableVehicleGroupID = "ImportExportVehicles" },
            },
        };
        BlankLocationPlaces.Add(lccivspawn3);

        BlankLocation lccivspawn4 = new BlankLocation(new Vector3(6166.53662f, -2445.1582f, 26.5752544f), 88.24339f, "lccivspawn4", "")
        {
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateCells = 3,
            ActivateDistance = 75f,
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new CivilianConditionalLocation(new Vector3(6168.30566f, -2441.19531f, 24.8075027f), 271.6276f, defaultCarSpawnPercentage) { OverrideDispatchableVehicleGroupID = "ImportExportVehicles" },
            },
        };
        BlankLocationPlaces.Add(lccivspawn4);

        BlankLocation lccivspawn5 = new BlankLocation(new Vector3(6412.09473f, -1533.32715f, 16.6691742f), 178.3405f, "lccivspawn5", "")
        {
            OpenTime = 12,
            CloseTime = 23,
            StateID = StaticStrings.LibertyStateID,
            ActivateCells = 3,
            ActivateDistance = 75f,
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new CivilianConditionalLocation(new Vector3(6412.51074f, -1538.5542f, 16.0069637f), 1.335895f, defaultCarSpawnPercentage) { OverrideDispatchableVehicleGroupID = "ImportExportVehicles" },
            },
        };
        BlankLocationPlaces.Add(lccivspawn5);

        BlankLocation lccivspawn6 = new BlankLocation(new Vector3(4023.89453f, -1607.23926f, 35.46405f), 106.3721f, "lccivspawn6", "")
        {
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateCells = 3,
            ActivateDistance = 75f,
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new CivilianConditionalLocation(new Vector3(4017.60547f, -1599.98926f, 34.9283524f), 0.8309714f, defaultCarSpawnPercentage) { OverrideDispatchableVehicleGroupID = "HighEndVehicles" },
                new CivilianConditionalLocation(new Vector3(4021.89966f, -1599.47021f, 34.922493f), 0.1210937f, defaultCarSpawnPercentage) { OverrideDispatchableVehicleGroupID = "HighEndVehicles" },
            },
        };
        BlankLocationPlaces.Add(lccivspawn6);

        BlankLocation lccivspawn7 = new BlankLocation(new Vector3(3854.13574f, -1910.49121f, 21.6332741f), 147.0407f, "lccivspawn7", "")
        {
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateCells = 3,
            ActivateDistance = 75f,
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new CivilianConditionalLocation(new Vector3(3854.04077f, -1914.56616f, 21.2279129f), 91.87346f, defaultCarSpawnPercentage) { OverrideDispatchableVehicleGroupID = "HighEndVehicles" },
                new CivilianConditionalLocation(new Vector3(3866.3667f, -1914.13721f, 21.2809944f), 91.92342f, defaultCarSpawnPercentage) { OverrideDispatchableVehicleGroupID = "ImportExportVehicles" },
                new CivilianConditionalLocation(new Vector3(3878.16772f, -1913.92419f, 21.2808342f), 91.81572f, defaultCarSpawnPercentage) { OverrideDispatchableVehicleGroupID = "HighEndVehicles" },
            },
        };
        BlankLocationPlaces.Add(lccivspawn7);

        BlankLocation lccivspawn8 = new BlankLocation(new Vector3(3599.8457f, -2977.70117f, 17.7658234f), 73.85777f, "lccivspawn8", "")
        {
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateCells = 3,
            ActivateDistance = 75f,
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new CivilianConditionalLocation(new Vector3(3598.776f, -2984.4978f, 16.294714f), 253.8885f, defaultCarSpawnPercentage) { OverrideDispatchableVehicleGroupID = "ImportExportVehicles" },
                new CivilianConditionalLocation(new Vector3(3600.3916f, -2981.31763f, 16.4383831f), 254.2497f, defaultCarSpawnPercentage) { OverrideDispatchableVehicleGroupID = "ImportExportVehicles" },
            },
        };
        BlankLocationPlaces.Add(lccivspawn8);

        BlankLocation lccivspawn9 = new BlankLocation(new Vector3(4238.23633f, -3743.629f, 2.839404f), 90.59621f, "lccivspawn9", "")
        {
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateCells = 3,
            ActivateDistance = 75f,
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new CivilianConditionalLocation(new Vector3(4234.27344f, -3742.541f, 2.334108f), 178.4972f, defaultCarSpawnPercentage) { OverrideDispatchableVehicleGroupID = "ImportExportVehicles" },
            },
        };
        BlankLocationPlaces.Add(lccivspawn9);

        BlankLocation lccivspawn10 = new BlankLocation(new Vector3(5200.72754f, -3871.46655f, 14.7464724f), 55.07816f, "lccivspawn10", "")
        {
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateCells = 3,
            ActivateDistance = 75f,
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new CivilianConditionalLocation(new Vector3(5195.46729f, -3866.81177f, 14.1214523f), 334.4337f, defaultCarSpawnPercentage) { OverrideDispatchableVehicleGroupID = "ImportExportVehicles" },
            },
        };
        BlankLocationPlaces.Add(lccivspawn10);

        BlankLocation lccivspawn11 = new BlankLocation(new Vector3(5202.64746f, -3367.0105f, 14.7741222f), 265.3253f, "lccivspawn11", "")
        {
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateCells = 3,
            ActivateDistance = 75f,
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new CivilianConditionalLocation(new Vector3(5206.062f, -3367.3623f, 14.1553926f), 179.6348f, defaultCarSpawnPercentage) { OverrideDispatchableVehicleGroupID = "HighEndVehicles" },
            },
        };
        BlankLocationPlaces.Add(lccivspawn11);

        BlankLocation lccivspawn12 = new BlankLocation(new Vector3(4683.12158f, -3614.062f, 5.779394f), 317.5302f, "lccivspawn12", "")
        {
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateCells = 3,
            ActivateDistance = 75f,
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new CivilianConditionalLocation(new Vector3(4688.92773f, -3609.03735f, 5.203979f), 136.8777f, defaultCarSpawnPercentage) { OverrideDispatchableVehicleGroupID = "ImportExportVehicles" },
                new CivilianConditionalLocation(new Vector3(4695.198f, -3616.20361f, 5.104164f), 130.5126f, defaultCarSpawnPercentage) { OverrideDispatchableVehicleGroupID = "HighEndVehicles" },
                new CivilianConditionalLocation(new Vector3(4673.54f, -3596.96582f, 5.361773f), 152.2519f, defaultCarSpawnPercentage) { OverrideDispatchableVehicleGroupID = "HighEndVehicles" },
            },
        };
        BlankLocationPlaces.Add(lccivspawn12);

        BlankLocation lccivspawn13 = new BlankLocation(new Vector3(4566.39453f, -3105.15723f, 4.813068f), 358.0971f, "lccivspawn13", "")
        {
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateCells = 3,
            ActivateDistance = 75f,
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new CivilianConditionalLocation(new Vector3(4563.51465f, -3094.2f, 4.147959f), 89.41318f, defaultCarSpawnPercentage) { OverrideDispatchableVehicleGroupID = "ImportExportVehicles" },
                new CivilianConditionalLocation(new Vector3(4582.46826f, -3089.30225f, 4.1462183f), 359.2759f, defaultCarSpawnPercentage) { OverrideDispatchableVehicleGroupID = "HighEndVehicles" },
            },
        };
        BlankLocationPlaces.Add(lccivspawn13);

        BlankLocation lccivspawn14 = new BlankLocation(new Vector3(5300.24f, -2359.99121f, 14.7124825f), 268.0731f, "lccivspawn14", "")
        {
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateCells = 3,
            ActivateDistance = 75f,
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new CivilianConditionalLocation(new Vector3(5306.772f, -2363.36133f, 14.0943623f), 178.8575f, defaultCarSpawnPercentage) { OverrideDispatchableVehicleGroupID = "HighEndVehicles" },
            },
        };
        BlankLocationPlaces.Add(lccivspawn14);

        BlankLocation lccivspawn15 = new BlankLocation(new Vector3(4907.78174f, -2070.12f, 14.7677927f), 179.3935f, "lccivspawn15", "")
        {
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateCells = 3,
            ActivateDistance = 75f,
            PossibleVehicleSpawns = new List<ConditionalLocation>()
            {
                new CivilianConditionalLocation(new Vector3(4902.783f, -2077.83f, 14.1413422f), 90.62903f, defaultCarSpawnPercentage) { OverrideDispatchableVehicleGroupID = "ImportExportVehicles" },
            },
        };
        BlankLocationPlaces.Add(lccivspawn15);
    }

    //  -------  Cop Section --------

    private void Checkpoints()
    {
        BlankLocation CICheckpoint1 = new BlankLocation(new Vector3(5290.352f, -2253.97217f, 14.7243929f), 268.257f, "CICheckpoint1", "Charge Island Incoming Checkpoint 1")
        {
            OpenTime = 8,
            CloseTime = 20,
            ActivateDistance = 200f,
            ActivateCells = 4,
            AssignedAssociationID = "NOOSE",
            StateID = StaticStrings.LibertyStateID,
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                new ConditionalGroup()
                {
                    Percentage = defaultSpawnPercentage,
                    OverrideNightPercentage = 0,
                    OverridePoorWeatherPercentage = 0,
                    PossiblePedSpawns = new List<ConditionalLocation>()
                    {
                        new LEConditionalLocation(new Vector3(5290.16f, -2236.05127f, 14.7116022f), 263.1698f, 0)
                        {
                            AssociationID = "NOOSE-BP",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<string> { "WORLD_HUMAN_BINOCULARS", "WORLD_HUMAN_CLIPBOARD" }
                        },
                        new LEConditionalLocation(new Vector3(5289.35f, -2237.46f, 14.70691f), 181.2608f, 0)
                        {
                            AssociationID = "NOOSE-BP",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<string> { "WORLD_HUMAN_CAR_PARK_ATTENDANT", "WORLD_HUMAN_CLIPBOARD" }
                        },
                        new LEConditionalLocation(new Vector3(5284.465f, -2252.644f, 14.71987f), 52.06823f, 0)
                        {
                            AssociationID = "NOOSE-BP",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<string> { "WORLD_HUMAN_COP_IDLES", "WORLD_HUMAN_CLIPBOARD" }
                        },
                        new LEConditionalLocation(new Vector3(5283.457f, -2253.04f, 14.72019f), 4.916255f, 0)
                        {
                            AssociationID = "NOOSE-BP",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<string> { "WORLD_HUMAN_CLIPBOARD", "WORLD_HUMAN_COP_IDLES" }
                        },
                    },
                    PossibleVehicleSpawns = new List<ConditionalLocation>()
                    {
                        new LEConditionalLocation(new Vector3(5286.425f, -2254.004f, 14.37315f), 326.7842f, 0) { AssociationID = "NOOSE-BP" },
                        new LEConditionalLocation(new Vector3(5288.405f, -2234.974f, 14.36191f), 151.1064f, 0) { AssociationID = "NOOSE-BP" },
                    },
                },
            },
        };
        BlankLocationPlaces.Add(CICheckpoint1);

        BlankLocation AirportCheckpoint1 = new BlankLocation(new Vector3(7165.131f, -2889.85474f, 17.8824635f), 83.68049f, "AirportCheckpoint1", "Francis Airport Checkpoint 2")
        {
            OpenTime = 8,
            CloseTime = 20,
            ActivateDistance = 200f,
            ActivateCells = 4,
            AssignedAssociationID = "NOOSE",
            StateID = StaticStrings.LibertyStateID,
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                new ConditionalGroup()
                {
                    Percentage = defaultSpawnPercentage,
                    OverrideNightPercentage = 0,
                    OverridePoorWeatherPercentage = 0,
                    PossiblePedSpawns = new List<ConditionalLocation>()
                    {
                        new LEConditionalLocation(new Vector3(7165.131f, -2889.85474f, 17.8824635f), 83.68049f, 0)
                        {
                            AssociationID = "NOOSE-BP",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<string> { "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_COP_IDLES" }
                        },
                        new LEConditionalLocation(new Vector3(7165.209f, -2888.39063f, 17.896204f), 84.53944f, 0)
                        {
                            AssociationID = "NOOSE-BP",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<string> { "WORLD_HUMAN_CLIPBOARD", "WORLD_HUMAN_COP_IDLES" }
                        },
                        new LEConditionalLocation(new Vector3(7166.13867f, -2874.067f, 17.9278831f), 129.9711f, 0)
                        {
                            AssociationID = "NOOSE-BP",
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<string> { "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_COP_IDLES" }
                        },
                    },
                    PossibleVehicleSpawns = new List<ConditionalLocation>()
                    {
                        new LEConditionalLocation(new Vector3(7164.78467f, -2894.20532f, 17.4478626f), 359.0083f, 0) { AssociationID = "NOOSE-BP" },
                        new LEConditionalLocation(new Vector3(7163.817f, -2871.38916f, 17.6260834f), 180.5637f, 0) { AssociationID = "NOOSE-BP" },
                    },
                },
            },
        };
        BlankLocationPlaces.Add(AirportCheckpoint1);
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
            EntrancePosition = new Vector3(4841.43f, -2587.62524f, 14.6491423f),
            EntranceHeading = 76.93037f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 150f,
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
                            MinHourSpawn = 12,
                            MaxHourSpawn = 14,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                     new LEConditionalLocation()
                                    {
                                            Location = new Vector3(4844.084f, -2596.553f, 12.7856f),
                                            Heading = 277.7819f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_COP_IDLES",
                                            },

                                    },
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(4843.019f, -2598.411f, 14.79367f),
                                            Heading = 70.17801f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                        Location = new Vector3(4835.062f, -2583.30615f, 14.2345228f),
                                        Heading = 0.1042213f,
                                        //AssociationID = "LCPD",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
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
            EntrancePosition = new Vector3(5073.723f, -3275.38f, 14.7713928f),
            EntranceHeading = 98.60426f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 150f,
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
                            MinHourSpawn = 18,
                            MaxHourSpawn = 2,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(5073.723f, -3275.38f, 14.8713923f),
                                            Heading = 98.60426f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(5073.39258f, -3274.21021f, 14.8713923f),
                                            Heading = 105.6469f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_CLIPBOARD",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
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
            EntrancePosition = new Vector3(5033.07861f, -3675.93384f, 14.8149624f),
            EntranceHeading = 333.8175f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
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
                            MinHourSpawn = 8,
                            MaxHourSpawn = 16,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(5033.07861f, -3675.93384f, 14.8149624f),
                                            Heading = 333.8175f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(5033.86572f, -3676.01318f, 14.8149624f),
                                            Heading = 359.9721f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
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
            EntrancePosition = new Vector3(5161.902f, -2853.12061f, 15.7657623f),
            EntranceHeading = 118.7021f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 150f,
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
                            MinHourSpawn = 8,
                            MaxHourSpawn = 18,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(5161.902f, -2853.12061f, 15.7657623f),
                                            Heading = 118.7021f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(5162.20166f, -2854.0603f, 15.7657528f),
                                            Heading = 106.0519f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
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
            EntrancePosition = new Vector3(5522.806f, -3879.71118f, 4.71392727f),
            EntranceHeading = 63.50958f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 150f,
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
                            MinHourSpawn = 8,
                            MaxHourSpawn = 18,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                    {
                                            Location = new Vector3(5522.806f, -3879.71118f, 4.71392727f),
                                            Heading = 63.50958f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(5523.44141f, -3878.69458f, 4.713316f),
                                            Heading = 66.54961f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_CLIPBOARD",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                        Location = new Vector3(5517.15234f, -3878.04468f, 4.258061f),
                                        Heading = 329.8925f,
                                        //AssociationID = "LCPD",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
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
            EntrancePosition = new Vector3(4642.719f, -3241.027f, 4.697802f),
            EntranceHeading = 88.52105f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 150f,
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
                            MinHourSpawn = 12,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                     new LEConditionalLocation()
                                    {
                                        Location = new Vector3(4642.719f, -3241.027f, 4.697802f),
                                            Heading = 68.71392f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                                    new LEConditionalLocation()
                                    {
                                        Location = new Vector3(4643.154f, -3239.599f, 4.662415f),
                                            Heading = 143.5684f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
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
            EntrancePosition = new Vector3(4817.81934f, -1838.0542f, 28.1472931f),
            EntranceHeading = 275.3827f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
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
                            MinHourSpawn = 8,
                            MaxHourSpawn = 16,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(4817.81934f, -1838.0542f, 28.1472931f),
                                            Heading = 275.3827f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(4817.75732f, -1839.01318f, 28.1472931f),
                                            Heading = 270.731f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
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
            EntrancePosition = new Vector3(5379.159f, -2167.205f, 8.447833f),
            EntranceHeading = 257.0893f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 150f,
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
                            MinHourSpawn = 8,
                            MaxHourSpawn = 18,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(5379.159f, -2167.205f, 8.447833f),
                                            Heading = 257.0893f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(5379.29443f, -2166.332f, 8.447833f),
                                            Heading = 267.9536f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
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
            EntrancePosition = new Vector3(4665.116f, -3602.47021f, 5.939279f),
            EntranceHeading = 333.0757f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 150f,
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
                            MinHourSpawn = 8,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(4665.1167f, -3602.47021f, 5.939279f),
                                            Heading = 333.0757f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(4665.86133f, -3602.92529f, 5.932936f),
                                            Heading = 336.958f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_CLIPBOARD",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                        Location = new Vector3(4670.55664f, -3595.79688f, 5.503239f),
                                        Heading = 335.2513f,
                                        //AssociationID = "LCPD",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
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
            EntrancePosition = new Vector3(4965.055f, -2836.37354f, 14.8124523f),
            EntranceHeading = 94.27563f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 150f,
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
                            MinHourSpawn = 18,
                            MaxHourSpawn = 2,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(4965.055f, -2836.37354f, 14.8124523f),
                                            Heading = 94.27563f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(4965.059f, -2837.37f, 14.8120127f),
                                            Heading = 93.1582f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
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
            EntrancePosition = new Vector3(5064.663f, -2230.582f, 6.159142f),
            EntranceHeading = 242.5591f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
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
                            MinHourSpawn = 8,
                            MaxHourSpawn = 16,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(5064.663f, -2230.582f, 6.159142f),
                                            Heading = 242.5591f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(5065.10547f, -2229.56519f, 6.159142f),
                                            Heading = 210.4014f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
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
            EntrancePosition = new Vector3(3708.18384f, -1852.0592f, 13.1694927f),
            EntranceHeading = 332.6969f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 150f,
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
                            MinHourSpawn = 16,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                    {
                                        Location = new Vector3(3708.18384f, -1852.0592f, 13.1694927f),
                                            Heading = 332.6969f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                        Location = new Vector3(3708.36768f, -1847.84216f, 12.6572828f),
                                        Heading = 170.9447f,
                                        //AssociationID = "LCPD",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
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
            EntrancePosition = new Vector3(3737.61865f, -2260.68823f, 23.0354939f),
            EntranceHeading = 180.8803f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 150f,
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
                            MinHourSpawn = 12,
                            MaxHourSpawn = 18,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new LEConditionalLocation()
                                    {
                                        Location = new Vector3(3737.61865f, -2260.68823f, 23.0354939f),
                                            Heading = 180.8803f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(3736.63379f, -2260.65015f, 23.0354939f),
                                            Heading = 176.989f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_AA_COFFEE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                        Location = new Vector3(3730.43677f, -2261.74121f, 22.5716534f),
                                        Heading = 359.0395f,
                                        //AssociationID = "LCPD",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
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
            EntrancePosition = new Vector3(4366.97949f, -2256.434f, 15.6273527f),
            EntranceHeading = 358.374f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 150f,
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
                            MinHourSpawn = 8,
                            MaxHourSpawn = 16,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(4366.97949f, -2256.434f, 15.6273527f),
                                            Heading = 358.374f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(4368.016f, -2256.413f, 15.6273422f),
                                            Heading = 5.169923f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_AA_COFFEE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
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
            EntrancePosition = new Vector3(3997.2583f, -1391.68823f, 6.619725f),
            EntranceHeading = 60.96603f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 150f,
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
                            MinHourSpawn = 18,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(3997.2583f, -1391.68823f, 6.619725f),
                                            Heading = 60.96603f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_INSPECT_STAND",
                                                "WORLD_HUMAN_INSPECT_CROUCH",
                                            },
                                    },
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(3998.477f, -1394.68115f, 6.47799f),
                                            Heading = 54.48252f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_INSPECT_STAND",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                        Location = new Vector3(4002.71069f, -1393.7373f, 6.086358f),
                                        Heading = 75.64349f,
                                        //AssociationID = "LCPD",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
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
            EntrancePosition = new Vector3(3238.57568f, -3337.74243f, 6.677045f),
            EntranceHeading = 357.9126f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 150f,
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
                            MinHourSpawn = 8,
                            MaxHourSpawn = 12,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(3238.57568f, -3337.74243f, 6.677045f),
                                            Heading = 357.9126f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_INSPECT_STAND",
                                                "WORLD_HUMAN_INSPECT_CROUCH",
                                            },
                                    },
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(-3237.65186f, -3339.14575f, 6.716856f),
                                            Heading = 172.6562f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_CLIPBOARD",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                        Location = new Vector3(3239.1416f, -3342.70239f, 6.269f),
                                        Heading = 93.25555f,
                                        //AssociationID = "LCPD",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
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
            EntrancePosition = new Vector3(3537.97681f, -2645.9502f, 29.8088436f),
            EntranceHeading = 276.2928f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 150f,
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
                            MinHourSpawn = 16,
                            MaxHourSpawn = 18,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(3537.97681f, -2645.9502f, 29.8088436f),
                                            Heading = 276.2928f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_CLIPBOARD",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(3535.93774f, -2646.69434f, 29.8088436f),
                                            Heading = 111.8419f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                        Location = new Vector3(3528.84961f, -2648.662f, 28.0372028f),
                                        Heading = 13.25766f,
                                        //AssociationID = "LCPD",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
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
            EntrancePosition = new Vector3(5630.73975f, -1736.64917f, 16.3354244f),
            EntranceHeading = 359.6194f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 150f,
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
                            MinHourSpawn = 12,
                            MaxHourSpawn = 16,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(5635.9f, -1751.614f, 14.31576f),
                                            Heading = 215.4751f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_AA_COFFEE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                        Location = new Vector3(5630.5874f, -1731.00122f, 15.7290926f),
                                        Heading = 269.4313f,
                                        //AssociationID = "LCPD",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
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
            EntrancePosition = new Vector3(6221.11963f, -1416.06323f, 13.6828823f),
            EntranceHeading = 251.5695f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
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
                            MinHourSpawn = 10,
                            MaxHourSpawn = 14,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(6221.11963f, -1416.06323f, 13.6828823f),
                                            Heading = 251.5695f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_AA_COFFEE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(6221.132f, -1417.17407f, 13.6828823f),
                                            Heading = 292.7367f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                    Location = new Vector3(6246.72656f, -1425.66724f, 12.2429428f),
                                        Heading = 338.8631f,
                                        //AssociationID = "LCPD",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
                                },
                            },
                    },
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = 0f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = 0f,
                            MinHourSpawn = 15,
                            MaxHourSpawn = 16,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                    Location = new Vector3(6246.72656f, -1425.66724f, 12.2429428f),
                                        Heading = 338.8631f,
                                        //AssociationID = "LCPD",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
                                        IsEmpty = false,
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
            EntrancePosition = new Vector3(6206.171f, -3078.36133f, 33.5525131f),
            EntranceHeading = 238.3846f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 150f,
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
                            MinHourSpawn = 8,
                            MaxHourSpawn = 14,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(6206.171f, -3078.36133f, 33.5525131f),
                                            Heading = 238.3846f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_COP_IDLES",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                    },
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(6206.60449f, -3077.47656f, 33.55252f),
                                            Heading = 236.3424f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                        Location = new Vector3(6215.171f, -3079.51978f, 31.474884f),
                                        Heading = 142.3437f,
                                        //AssociationID = "LCPD",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
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
            EntrancePosition = new Vector3(6026.30371f, -2949.64624f, 5.561987f),
            EntranceHeading = 268.3151f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 150f,
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
                            MinHourSpawn = 18,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                    {
                                        Location = new Vector3(6026.30371f, -2949.64624f, 5.561987f),
                                            Heading = 268.3151f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_INSPECT_CROUCH",
                                                "WORLD_HUMAN_INSPECT_STAND",
                                            },
                                    },
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(6024.55469f, -2950.16187f, 5.618442f),
                                            Heading = 85.91793f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                        Location = new Vector3(6019.906f, -2955.02979f, 5.500554f),
                                        Heading = 356.8531f,
                                        //AssociationID = "LCPD",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
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
            EntrancePosition = new Vector3(6069.27148f, -3287.4248f, 28.4227028f),
            EntranceHeading = 333.9765f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateCells = 3,
            ActivateDistance = 125f,
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
                            MinHourSpawn = 19,
                            MaxHourSpawn = 22,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(6069.27148f, -3287.4248f, 28.4227028f),
                                            Heading = 333.9765f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_CLIPBOARD",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                                    new LEConditionalLocation()
                                    {
                                        Location = new Vector3(6067.915f, -3288.10449f, 28.4227028f),
                                            Heading = 171.7107f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                        Location = new Vector3(6054.106f, -3292.08545f, 25.7688541f),
                                        Heading = 41.5046f,
                                        //AssociationID = "LCPD",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
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
            EntrancePosition = new Vector3(6425.95361f, -3691.70313f, 16.6556435f),
            EntranceHeading = 87.22088f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
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
                            MinHourSpawn = 12,
                            MaxHourSpawn = 16,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(6425.95361f, -3691.70313f, 16.6556435f),
                                            Heading = 87.22088f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_INSPECT_CROUCH",
                                                "WORLD_HUMAN_INSPECT_STAND",
                                            },
                                    },
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(6427.52148f, -3692.61816f, 16.6556435f),
                                            Heading = 229.6176f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                        Location = new Vector3(6423.191f, -3716.83472f, 13.9588823f),
                                        Heading = 270.8412f,
                                        //AssociationID = "LCPD",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
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
            EntrancePosition = new Vector3(6153.675f, -3867.615f, 14.231f),
            EntranceHeading = 148.319f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
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
                            MinHourSpawn = 8,
                            MaxHourSpawn = 16,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(6153.675f, -3867.615f, 14.231f),
                                            Heading = 148.319f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_COP_IDLES",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                    },
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(6153.611f, -3869.047f, 14.23106f),
                                            Heading = 64.70537f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                        Location = new Vector3(6147.729f, -3867.131f, 13.73446f),
                                        Heading = 359.3201f,
                                        //AssociationID = "LCPD",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
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
            EntrancePosition = new Vector3(5925.15332f, -3738.25537f, 5.978772f),
            EntranceHeading = 93.22203f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 150f,
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
                            MinHourSpawn = 8,
                            MaxHourSpawn = 16,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(5925.15332f, -3738.25537f, 5.978772f),
                                            Heading = 93.22203f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(5925.18555f, -3737.24854f, 5.981997f),
                                            Heading = 91.31911f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                        Location = new Vector3(5935.59863f, -3739.81519f, 5.486167f),
                                        Heading = 177.6516f,
                                        //AssociationID = "LCPD",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
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
            EntrancePosition = new Vector3(6415.947f, -2232.582f, 13.6327324f),
            EntranceHeading = 5.859095f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 150f,
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
                            MinHourSpawn = 12,
                            MaxHourSpawn = 18,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(6396.49f, -2240.512f, 13.64784f),
                                            Heading = 201.7947f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_AA_COFFEE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(6397.544f, -2240.544f, 13.64786f),
                                            Heading = 152.1024f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                        Location = new Vector3(6418.20361f, -2241.02222f, 13.1049623f),
                                        Heading = 359.9984f,
                                        //AssociationID = "LCPD",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
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
            EntrancePosition = new Vector3(6309.986f, -2576.6333f, 37.4425926f),
            EntranceHeading = 43.61218f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
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
                            MinHourSpawn = 8,
                            MaxHourSpawn = 12,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                    {
                                            Location = new Vector3(6309.986f, -2576.6333f, 37.4425926f),
                                            Heading = 43.61218f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_AA_COFFEE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                    Location = new Vector3(6305.155f, -2577.287f, 37.04027f),
                                        Heading = 354.9214f,
                                        //AssociationID = "LCPD",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
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
            EntrancePosition = new Vector3(6986.41064f, -2772.05078f, 28.1643734f),
            EntranceHeading = 279.9722f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
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
                            MinHourSpawn = 18,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(6986.41064f, -2772.05078f, 28.1643734f),
                                            Heading = 279.9722f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_UPRIGHT",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                        Location = new Vector3(6991.43262f, -2767.00952f, 27.7319927f),
                                        Heading = 171.6885f,
                                        //AssociationID = "LCPD",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
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
            EntrancePosition = new Vector3(5571.814f, -3137.51855f, 8.461428f),
            EntranceHeading = 89.96902f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
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
                            MinHourSpawn = 8,
                            MaxHourSpawn = 18,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(5571.814f, -3137.51855f, 8.461428f),
                                            Heading = 89.96902f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(5571.969f, -3135.83447f, 8.461274f),
                                            Heading = 89.24456f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
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
            Description = "2 Cops at Cable Car on Colony Island",
            MapIcon = 162,
            EntrancePosition = new Vector3(5642.215f, -3017.365f, 8.323179f),
            EntranceHeading = 4.00087f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 150f,
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
                            MinHourSpawn = 8,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(5635.66f, -3018.501f, 14.71103f),
                                            Heading = 254.4245f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_COP_IDLES",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                    },
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(5635.731f, -3020.052f, 14.71254f),
                                            Heading = 304.5145f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new LEConditionalLocation()
                                {
                                        Location = new Vector3(5642.215f, -3017.365f, 8.323179f),
                                        Heading = 88.67443f,
                                        //AssociationID = "LCPD",
                                        RequiredPedGroup = "",
                                        RequiredVehicleGroup = "",
                                },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(ColonyIslandCops2);
        // Happyness Island
        BlankLocation HappyIslandCops = new BlankLocation()
        {
            Name = "HappynessIslandCops",
            FullName = "HappynessIslandCops",
            Description = "Cops on Happyness Island",
            MapIcon = 162,
            EntrancePosition = new Vector3(4636.414f, -4215.933f, 4.83832f),
            EntranceHeading = 260.7795f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 150f,
            AssignedAssociationID = "LCPD",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = 0f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = 0f,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(4689.085f, -4209.68f, 4.838318f),
                                            Heading = 260.7795f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_COP_IDLES",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                    },
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(4635.6f, -4216.028f, 4.83832f),
                                            Heading = 276.41f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(4636.14f, -4215.154f, 4.83832f),
                                            Heading = 181.41f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(4636.414f, -4215.933f, 4.83832f),
                                            Heading = 96.41f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(4614.097f, -4219.415f, 4.83832f),
                                            Heading = 1.42f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(HappyIslandCops);
        BlankLocation HappyIslandStatueCops = new BlankLocation()
        {
            Name = "HappyIslandStatue",
            FullName = "HappyIslandStatue",
            Description = "Cops in Happy Island Statue",
            MapIcon = 162,
            EntrancePosition = new Vector3(4578.415f, -4041.265f, 4.839075f),
            EntranceHeading = 177.3806f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 150f,
            AssignedAssociationID = "LCPD",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            OverrideNightPercentage = 0f,
                            OverrideDayPercentage = -1f,
                            OverridePoorWeatherPercentage = 0f,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 20,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,                           
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(4578.415f, -4041.265f, 4.839075f),
                                            Heading = 181.41f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(4581.817f, -4041.288f, 4.83907f),
                                            Heading = 181.41f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(4574.606f, -4038.535f, 13.06676f),
                                            Heading = 181.41f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                                    new LEConditionalLocation()
                                    {
                                            Location = new Vector3(4573.622f, -4019.726f, 46.31398f),
                                            Heading = 181.41f,
                                            //AssociationID = "LCPD",
                                            RequiredPedGroup = "",
                                            RequiredVehicleGroup = "",
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(HappyIslandStatueCops);
    }
    private void RooftopSnipers()
    {
        float sniperSpawnPercentage = 65f;
        BlankLocation LCRoofTopSniper1 = new BlankLocation(new Vector3(4800.981f, -2691.937f, 40.9822121f), 318.6407f, "LCRoofTopSniper1", "LCRooftop Sniper 1")
        {
            ActivateDistance = 200f,
            ActivateCells = 8,
            StateID = StaticStrings.LibertyStateID,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new LEConditionalLocation(new Vector3(4800.981f, -2691.937f, 40.9822121f), 318.6407f, sniperSpawnPercentage)
                {
                    RequiredPedGroup = "Sniper",
                    MinWantedLevelSpawn = 4,
                    MaxWantedLevelSpawn = 6,
                    TaskRequirements = TaskRequirements.Guard | TaskRequirements.EquipLongGunWhenIdle,
                    LongGunAlwaysEquipped = true
                },
            },
        };
        BlankLocationPlaces.Add(LCRoofTopSniper1);

        BlankLocation LCRoofTopSniper2 = new BlankLocation(new Vector3(5356.26367f, -2932.79175f, 25.1729336f), 23.41006f, "LCRoofTopSniper2", "LCRooftop Sniper 2")
        {
            ActivateDistance = 200f,
            ActivateCells = 8,
            StateID = StaticStrings.LibertyStateID,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new LEConditionalLocation(new Vector3(5356.26367f, -2932.79175f, 25.1729336f), 23.41006f, sniperSpawnPercentage)
                {
                    RequiredPedGroup = "Sniper",
                    MinWantedLevelSpawn = 4,
                    MaxWantedLevelSpawn = 6,
                    TaskRequirements = TaskRequirements.Guard | TaskRequirements.EquipLongGunWhenIdle,
                    LongGunAlwaysEquipped = true
                },
            },
        };
        BlankLocationPlaces.Add(LCRoofTopSniper2);

        BlankLocation LCRoofTopSniper3 = new BlankLocation(new Vector3(5003.62842f, -2835.45117f, 36.9844322f), 124.0438f, "LCRoofTopSniper3", "LCRooftop Sniper 3")
        {
            ActivateDistance = 200f,
            ActivateCells = 8,
            StateID = StaticStrings.LibertyStateID,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new LEConditionalLocation(new Vector3(5003.62842f, -2835.45117f, 36.9844322f), 124.0438f, sniperSpawnPercentage)
                {
                    RequiredPedGroup = "Sniper",
                    MinWantedLevelSpawn = 4,
                    MaxWantedLevelSpawn = 6,
                    TaskRequirements = TaskRequirements.Guard | TaskRequirements.EquipLongGunWhenIdle,
                    LongGunAlwaysEquipped = true
                },
            },
        };
        BlankLocationPlaces.Add(LCRoofTopSniper3);

        BlankLocation LCRoofTopSniper4 = new BlankLocation(new Vector3(4996.56738f, -3599.74683f, 30.6522427f), 192.7635f, "LCRoofTopSniper4", "LCRooftop Sniper 4")
        {
            ActivateDistance = 200f,
            ActivateCells = 8,
            StateID = StaticStrings.LibertyStateID,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new LEConditionalLocation(new Vector3(4996.56738f, -3599.74683f, 30.6522427f), 192.7635f, sniperSpawnPercentage)
                {
                    RequiredPedGroup = "Sniper",
                    MinWantedLevelSpawn = 4,
                    MaxWantedLevelSpawn = 6,
                    TaskRequirements = TaskRequirements.Guard | TaskRequirements.EquipLongGunWhenIdle,
                    LongGunAlwaysEquipped = true
                },
            },
        };
        BlankLocationPlaces.Add(LCRoofTopSniper4);

        BlankLocation LCRoofTopSniper5 = new BlankLocation(new Vector3(5341.27539f, -3762.24683f, 40.67742f), 45.07904f, "LCRoofTopSniper5", "LCRooftop Sniper 5")
        {
            ActivateDistance = 200f,
            ActivateCells = 8,
            StateID = StaticStrings.LibertyStateID,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new LEConditionalLocation(new Vector3(5341.27539f, -3762.24683f, 40.67742f), 45.07904f, sniperSpawnPercentage)
                {
                    RequiredPedGroup = "Sniper",
                    MinWantedLevelSpawn = 4,
                    MaxWantedLevelSpawn = 6,
                    TaskRequirements = TaskRequirements.Guard | TaskRequirements.EquipLongGunWhenIdle,
                    LongGunAlwaysEquipped = true
                },
            },
        };
        BlankLocationPlaces.Add(LCRoofTopSniper5);

        BlankLocation LCRoofTopSniper6 = new BlankLocation(new Vector3(4655.459f, -2558.47021f, 30.4252644f), 41.20105f, "LCRoofTopSniper6", "LCRooftop Sniper 6")
        {
            ActivateDistance = 200f,
            ActivateCells = 8,
            StateID = StaticStrings.LibertyStateID,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new LEConditionalLocation(new Vector3(4655.459f, -2558.47021f, 30.4252644f), 41.20105f, sniperSpawnPercentage)
                {
                    RequiredPedGroup = "Sniper",
                    MinWantedLevelSpawn = 4,
                    MaxWantedLevelSpawn = 6,
                    TaskRequirements = TaskRequirements.Guard | TaskRequirements.EquipLongGunWhenIdle,
                    LongGunAlwaysEquipped = true
                },
            },
        };
        BlankLocationPlaces.Add(LCRoofTopSniper6);

        BlankLocation LCRoofTopSniper7 = new BlankLocation(new Vector3(5166.409f, -2365.1123f, 44.605423f), 94.32448f, "LCRoofTopSniper7", "LCRooftop Sniper 7")
        {
            ActivateDistance = 200f,
            ActivateCells = 8,
            StateID = StaticStrings.LibertyStateID,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new LEConditionalLocation(new Vector3(5166.409f, -2365.1123f, 44.605423f), 94.32448f, sniperSpawnPercentage)
                {
                    RequiredPedGroup = "Sniper",
                    MinWantedLevelSpawn = 4,
                    MaxWantedLevelSpawn = 6,
                    TaskRequirements = TaskRequirements.Guard | TaskRequirements.EquipLongGunWhenIdle,
                    LongGunAlwaysEquipped = true
                },
            },
        };
        BlankLocationPlaces.Add(LCRoofTopSniper7);
        BlankLocation LCHappyIslandSniper = new BlankLocation(new Vector3(4580.013f, -4019.247f, 46.305f), 175.3217f, "LCHappyIslandSniper", "LC Happy Island Sniper")
        {
            ActivateDistance = 200f,
            ActivateCells = 8,
            StateID = StaticStrings.LibertyStateID,
            PossiblePedSpawns = new List<ConditionalLocation>()
            {
                new LEConditionalLocation(new Vector3(4580.013f, -4019.247f, 46.305f), 175.3217f, sniperSpawnPercentage)
                {
                    RequiredPedGroup = "Sniper",
                    MinWantedLevelSpawn = 4,
                    MaxWantedLevelSpawn = 6,
                    TaskRequirements = TaskRequirements.Guard | TaskRequirements.EquipLongGunWhenIdle,
                    LongGunAlwaysEquipped = true
                },
            },
        };
        BlankLocationPlaces.Add(LCHappyIslandSniper);
    }
    private void SpeedTraps()
    {
        List<BlankLocation> blankLocationPlaces = new List<BlankLocation>() {
            //Algonquin
            new BlankLocation(new Vector3(4573.788f, -2445.35425f, 6.68325f), 32.41907f, "ALGSpeedTrap1", "Speed Trap Union Drive West 1") {
                ActivateDistance = 200f,
                StateID = StaticStrings.LibertyStateID,
                PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup(){
                        Percentage = defaultSpawnPercentage,
                        OverrideNightPercentage = 0.0f,
                        OverridePoorWeatherPercentage = 0.0f,
                        PossibleVehicleSpawns  = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(4571.94531f, -2441.7522f, 6.073362f), 162.4008f, 0f) {
                                AssociationID = "",
                                RequiredVehicleGroup = "Motorcycle", },
                        },
                        PossiblePedSpawns = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(4573.788f, -2445.35425f, 6.683259f), 32.41907f, 0f) {
                                AssociationID = "",
                                RequiredPedGroup = "MotorcycleCop",
                                TaskRequirements = TaskRequirements.Guard,
                                ForcedScenarios = new List<string>(){ "WORLD_HUMAN_BINOCULARS" } },
                        }
                    },

                }
            },
            new BlankLocation(new Vector3(4639.02344f, -2901.32422f, 6.658971f), 180.2172f, "ALGSpeedTrap2", "Speed Trap Union Drive West 2") {
                ActivateDistance = 200f,
                StateID = StaticStrings.LibertyStateID,
                PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup(){
                        Percentage = defaultSpawnPercentage,
                        OverrideNightPercentage = 0.0f,
                        OverridePoorWeatherPercentage = 0.0f,
                        PossibleVehicleSpawns  = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(4639.971f, -2902.5835f, 5.984298f), 282.4508f, 0f) {
                                AssociationID = "",
                                RequiredVehicleGroup = "Motorcycle", },
                        },
                        PossiblePedSpawns = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(4639.02344f, -2901.32422f, 6.658971f), 180.2172f, 0f) {
                                AssociationID = "",
                                RequiredPedGroup = "MotorcycleCop",
                                TaskRequirements = TaskRequirements.Guard,
                                ForcedScenarios = new List<string>(){ "WORLD_HUMAN_BINOCULARS" } },
                        }
                    },

                }
            },
            new BlankLocation(new Vector3(5331.81445f, -2307.36621f, 14.7138329f), 321.6855f, "ALGSpeedTrap3", "Covering Union Drive East") {
                ActivateDistance = 200f,
                StateID = StaticStrings.LibertyStateID,
                PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup(){
                        Percentage = defaultSpawnPercentage,
                        OverrideNightPercentage = 0.0f,
                        OverridePoorWeatherPercentage = 0.0f,
                        PossibleVehicleSpawns  = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(5328.57959f, -2307.03613f, 14.7139225f), 80.00423f, 0f) {
                                AssociationID = "",
                                RequiredVehicleGroup = "Motorcycle", },
                        },
                        PossiblePedSpawns = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(5331.81445f, -2307.36621f, 14.7138329f), 321.6855f, 0f) {
                                AssociationID = "",
                                RequiredPedGroup = "MotorcycleCop",
                                TaskRequirements = TaskRequirements.Guard,
                                ForcedScenarios = new List<string>(){ "WORLD_HUMAN_BINOCULARS" } },
                        }
                    },

                }
            },
            new BlankLocation(new Vector3(5405.5625f, -2923.811f, 8.603383f), 3.34502f, "ALGSpeedTrap4", "Covering Union Drive East") {
                ActivateDistance = 200f,
                StateID = StaticStrings.LibertyStateID,
                PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup(){
                        Percentage = defaultSpawnPercentage,
                        OverrideNightPercentage = 0.0f,
                        OverridePoorWeatherPercentage = 0.0f,
                        PossibleVehicleSpawns  = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(5403.299f, -2924.65112f, 8.080965f), 4.921218f, 0f)
                            {
                                AssociationID = "",
                                RequiredVehicleGroup = "Motorcycle",
                            },
                        },
                        PossiblePedSpawns = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(5405.5625f, -2923.811f, 8.603383f), 3.34502f, 0f)
                            {
                                AssociationID = "",
                                RequiredPedGroup = "MotorcycleCop",
                                TaskRequirements = TaskRequirements.Guard,
                                ForcedScenarios = new List<string>(){ "WORLD_HUMAN_BINOCULARS" }
                            },
                        }
                    },

                }
            },
            new BlankLocation(new Vector3(5353.141f, -2472.8623f, 7.64442f), 359.9992f, "ALGSpeedTrap5", "Union Drive East Tunnel") {
                ActivateDistance = 200f,
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(5353.141f, -2472.8623f, 7.64442f), 359.9992f, defaultSpawnPercentage) {
                        IsEmpty = false,
                        TaskRequirements = TaskRequirements.Guard,
                        OverrideNightPercentage = 55.0f,
                        OverridePoorWeatherPercentage = 0.0f },
                },
            },
            new BlankLocation(new Vector3(4954.19141f, -2942.731f, 14.3408527f), 267.0054f, "ALGSpeedTrap6", "Star Junction Alley") {
                ActivateDistance = 200f,
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(4954.19141f, -2942.731f, 14.3408527f), 267.0054f, defaultSpawnPercentage) {
                        IsEmpty = false,
                        TaskRequirements = TaskRequirements.Guard,
                        OverrideNightPercentage = 55.0f,
                        OverridePoorWeatherPercentage = 0.0f },
                },
            },
            new BlankLocation(new Vector3(4716.492f, -3029.17334f, 9.467564f), 269.5866f, "ALGSpeedTrap7", "Westminster Carpark") {
                ActivateDistance = 200f,
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(4716.492f, -3029.17334f, 9.467564f), 269.5866f, defaultSpawnPercentage) {
                        IsEmpty = false,
                        TaskRequirements = TaskRequirements.Guard,
                        OverrideNightPercentage = 55.0f,
                        OverridePoorWeatherPercentage = 0.0f },
                },
            },
            new BlankLocation(new Vector3(4878.94629f, -2066.17725f, 14.6141129f), 179.4064f, "ALGSpeedTrap8", "Middle Park - Topaz Street Alley") {
                ActivateDistance = 200f,
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(4878.94629f, -2066.17725f, 14.6141129f), 179.4064f, defaultSpawnPercentage) {
                        IsEmpty = false,
                        TaskRequirements = TaskRequirements.Guard,
                        OverrideNightPercentage = 55.0f,
                        OverridePoorWeatherPercentage = 0.0f },
                },
            },
            new BlankLocation(new Vector3(4964.486f, -2669.89624f, 14.3395329f), 270.0316f, "ALGSpeedTrap9", "Star Junction Alley across from The Majestic") {
                ActivateDistance = 200f,
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(4964.486f, -2669.89624f, 14.3395329f), 270.0316f, defaultSpawnPercentage) {
                        IsEmpty = false,
                        TaskRequirements = TaskRequirements.Guard,
                        OverrideNightPercentage = 55.0f,
                        OverridePoorWeatherPercentage = 0.0f },
                },
            },
            //Bohan
            new BlankLocation(new Vector3(5911.36865f, -1448.45825f, 39.4403229f), 314.965f, "BOHSpeedTrap1", "Valdez St. overlooking Grand Boulevard") {
                ActivateDistance = 200f,
                StateID = StaticStrings.LibertyStateID,
                PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup(){
                        Percentage = defaultSpawnPercentage,
                        OverrideNightPercentage = 0.0f,
                        OverridePoorWeatherPercentage = 0.0f,
                        PossibleVehicleSpawns  = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(5909.10547f, -1447.74927f, 38.9143524f), 64.47408f, 0f) {
                                AssociationID = "",
                                RequiredVehicleGroup = "Motorcycle", },
                        },
                        PossiblePedSpawns = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(5911.36865f, -1448.45825f, 39.4403229f), 314.965f, 0f) {
                                AssociationID = "",
                                RequiredPedGroup = "MotorcycleCop",
                                TaskRequirements = TaskRequirements.Guard,
                                ForcedScenarios = new List<string>(){ "WORLD_HUMAN_BINOCULARS" } },
                        }
                    },

                }
            },
            new BlankLocation(new Vector3(6062.002f, -1597.10425f, 16.6604633f), 227.3036f, "BOHSpeedTrap2", "Across from Alpha Mail - Industrial") {
                ActivateDistance = 200f,
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(6062.002f, -1597.10425f, 16.6604633f), 227.3036f, defaultSpawnPercentage) {
                        IsEmpty = false,
                        TaskRequirements = TaskRequirements.Guard,
                        OverrideNightPercentage = 55.0f,
                        OverridePoorWeatherPercentage = 0.0f },
                },
            },
            //Dukes
            new BlankLocation(new Vector3(7072.162f, -2327.28223f, 23.9681339f), 231.4356f, "DUKSpeedTrap1", "Dukes Expressway") {
                ActivateDistance = 200f,
                StateID = StaticStrings.LibertyStateID,
                PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup(){
                        Percentage = defaultSpawnPercentage,
                        OverrideNightPercentage = 0.0f,
                        OverridePoorWeatherPercentage = 0.0f,
                        PossibleVehicleSpawns  = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(7069.76953f, -2326.53418f, 23.4700241f), 75.3797f, 0f) {
                                AssociationID = "",
                                RequiredVehicleGroup = "Motorcycle", },
                        },
                        PossiblePedSpawns = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(7072.162f, -2327.28223f, 23.9681339f), 231.4356f, 0f) {
                                AssociationID = "",
                                RequiredPedGroup = "MotorcycleCop",
                                TaskRequirements = TaskRequirements.Guard,
                                ForcedScenarios = new List<string>(){ "WORLD_HUMAN_BINOCULARS" } },
                        }
                    },

                }
            },
            new BlankLocation(new Vector3(7160.093f, -3110.59058f, 18.248373f), 297.3787f, "DUKSpeedTrap2", "Dukes Expressway Alt Direction") {
                ActivateDistance = 200f,
                StateID = StaticStrings.LibertyStateID,
                PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup(){
                        Percentage = defaultSpawnPercentage,
                        OverrideNightPercentage = 0.0f,
                        OverridePoorWeatherPercentage = 0.0f,
                        PossibleVehicleSpawns  = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(7157.956f, -3111.72119f, 17.7185135f), 131.1609f, 0f) {
                                AssociationID = "",
                                RequiredVehicleGroup = "Motorcycle", },
                        },
                        PossiblePedSpawns = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(7160.093f, -3110.59058f, 18.248373f), 297.3787f, 0f) {
                                AssociationID = "",
                                RequiredPedGroup = "MotorcycleCop",
                                TaskRequirements = TaskRequirements.Guard,
                                ForcedScenarios = new List<string>(){ "WORLD_HUMAN_BINOCULARS" } },
                        }
                    },

                }
            },
            new BlankLocation(new Vector3(6951.463f, -2714.728f, 28.7992439f), 43.6178f, "DUKSpeedTrap3", "Howard St Willis") {
                ActivateDistance = 200f,
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(6951.463f, -2714.728f, 28.7992439f), 43.6178f, defaultSpawnPercentage) {
                        IsEmpty = false,
                        TaskRequirements = TaskRequirements.Guard,
                        OverrideNightPercentage = 55.0f,
                        OverridePoorWeatherPercentage = 0.0f },
                },
            },
            //Broker
            new BlankLocation(new Vector3(6701.859f, -3557.82617f, 14.4745922f), 37.95805f, "BROSpeedTrap1", "Dukes Expressway Broker") {
                ActivateDistance = 200f,
                StateID = StaticStrings.LibertyStateID,
                PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup(){
                        Percentage = defaultSpawnPercentage,
                        OverrideNightPercentage = 0.0f,
                        OverridePoorWeatherPercentage = 0.0f,
                        PossibleVehicleSpawns  = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(6701.836f, -3555.996f, 13.9513426f), 46.04592f, defaultSpawnPercentage) {
                                AssociationID = "",
                                RequiredVehicleGroup = "Motorcycle",
                                OverrideNightPercentage = 55.0f,
                                OverridePoorWeatherPercentage = 0.0f },
                        },
                        PossiblePedSpawns = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(6701.859f, -3557.82617f, 14.4745922f), 37.95805f, defaultSpawnPercentage) {
                                AssociationID = "",
                                RequiredPedGroup = "MotorcycleCop",
                                TaskRequirements = TaskRequirements.Guard,
                                OverrideNightPercentage = 55.0f,
                                OverridePoorWeatherPercentage = 0.0f,
                                ForcedScenarios = new List<string>(){ "WORLD_HUMAN_BINOCULARS" } },
                        }
                    },

                }
            },
            new BlankLocation(new Vector3(6307.552f, -2989.623f, 30.6187229f), 89.78445f, "BROSpeedTrap2", "Downtown Broker") {
                ActivateDistance = 200f,
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(6307.552f, -2989.623f, 30.6187229f), 89.78445f, defaultSpawnPercentage) {
                        IsEmpty = false,
                        TaskRequirements = TaskRequirements.Guard,
                        OverrideNightPercentage = 55.0f,
                        OverridePoorWeatherPercentage = 0.0f },
                },
            },
            new BlankLocation(new Vector3(6368.26758f, -3414.86523f, 28.4733429f), 359.7886f, "BROSpeedTrap3", "South Slopes Broker") {
                ActivateDistance = 200f,
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(6368.26758f, -3414.86523f, 28.4733429f), 359.7886f, defaultSpawnPercentage) {
                        IsEmpty = false,
                        TaskRequirements = TaskRequirements.Guard,
                        OverrideNightPercentage = 55.0f,
                        OverridePoorWeatherPercentage = 0.0f },
                },
            },
            //Alderney
            new BlankLocation(new Vector3(3450.09668f, -2540.11914f, 29.4871235f), 162.3214f, "ALDSpeedTrap1", "Plumbers Skyway Berchem") {
                ActivateDistance = 200f,
                StateID = StaticStrings.AlderneyStateID,
                PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup(){
                        Percentage = defaultSpawnPercentage,
                        OverrideNightPercentage = 0.0f,
                        OverridePoorWeatherPercentage = 0.0f,
                        PossibleVehicleSpawns  = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(3449.93774f, -2537.37524f, 28.2661438f), 326.2217f, 0f) {
                                AssociationID = "",
                                RequiredVehicleGroup = "Motorcycle", },
                        },
                        PossiblePedSpawns = new List<ConditionalLocation>()
                        {
                            new LEConditionalLocation(new Vector3(3450.09668f, -2540.11914f, 29.4871235f), 162.3214f, 0f) {
                                AssociationID = "",
                                RequiredPedGroup = "MotorcycleCop",
                                TaskRequirements = TaskRequirements.Guard,
                                ForcedScenarios = new List<string>(){ "WORLD_HUMAN_BINOCULARS" } },
                        }
                    },

                }
            },
            new BlankLocation(new Vector3(3924.03882f, -2435.56f, 19.1693344f), 267.0769f, "ALDSpeedTrap2", "Plumber's Skyway Alderney City") {
                ActivateDistance = 200f,
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(3924.03882f, -2435.56f, 19.1693344f), 267.0769f, defaultSpawnPercentage) {
                        IsEmpty = false,
                        TaskRequirements = TaskRequirements.Guard,
                        OverrideNightPercentage = 55.0f,
                        OverridePoorWeatherPercentage = 0.0f },
                },
            },
            new BlankLocation(new Vector3(3886.22876f, -3063.21631f, 7.65221f), 271.3782f, "ALDSpeedTrap3", "Roebuck Rd Tudor") {
                ActivateDistance = 200f,
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(3886.22876f, -3063.21631f, 7.65221f), 271.3782f, defaultSpawnPercentage) {
                        IsEmpty = false,
                        TaskRequirements = TaskRequirements.Guard,
                        OverrideNightPercentage = 55.0f,
                        OverridePoorWeatherPercentage = 0.0f },
                },
            },
            //Colony island
            new BlankLocation(new Vector3(5635.675f, -3095.39648f, 8.170388f), 89.80821f, "COLSpeedTrap1", "Colony Island") {
                ActivateDistance = 200f,
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new LEConditionalLocation(new Vector3(5635.675f, -3095.39648f, 8.170388f), 89.80821f, defaultSpawnPercentage) {
                        IsEmpty = false,
                        TaskRequirements = TaskRequirements.Guard,
                        OverrideNightPercentage = 55.0f,
                        OverridePoorWeatherPercentage = 0.0f },
                },
            },
        };
        BlankLocationPlaces.AddRange(blankLocationPlaces);
    }

    //  -------  Gangs Section  --------

    // -------  Biker Boys  --------
    private void AodGang()
    {
        BlankLocation AodDiner = new BlankLocation()
        {
            Name = "AodDiner",
            Description = "",
            EntrancePosition = new Vector3(4787.82568f, -3015.35742f, 13.2739725f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_ANGELS",
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
                                        Location = new Vector3(4792.94727f, -3012.324f, 14.2393322f),
                                            Heading = 277.9568f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(4794.455f, -3012.32642f, 14.3624325f),
                                            Heading = 74.67204f,
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
                                    Location = new Vector3(4788.929f, -3017.102f, 13.36138f),
                                        Heading = 231.6116f,
                                },
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(4789.741f, -3014.372f, 13.42917f),
                                        Heading = 224.5946f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(AodDiner);
        BlankLocation AodCommunity = new BlankLocation()
        {

            Name = "AodCommunity",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(6982.567f, -2510.28613f, 25.1842842f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_ANGELS",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6986.677f, -2507.30225f, 25.7043438f),
                                            Heading = 90.21704f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            }
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6984.63867f, -2506.308f, 25.7085934f),
                                            Heading = 237.9349f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRINKING_CASINO_TERRACE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(6984.292f, -2508.496f, 25.7073326f),
                                            Heading = 290.8024f,
                                            Percentage = 0f,
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
                                            MinHourSpawn = 18,
                                            MaxHourSpawn = 4,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 4,
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6987.70068f, -2500.11133f, 25.7133236f),
                                            Heading = 0.9797935f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STUPOR_CLUBHOUSE",
                                                "WORLD_HUMAN_STUPOR",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6982.567f, -2510.2861f, 25.1842842f),
                                            Heading = 0.4643726f,
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6981.72656f, -2505.31323f, 25.1794529f),
                                            Heading = 297.6171f,
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6981.749f, -2502.60815f, 25.1800232f),
                                            Heading = 296.4234f,
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(AodCommunity);
        BlankLocation AodLimbo = new BlankLocation()
        {
            Name = "AodLimbo",
            Description = "",
            EntrancePosition = new Vector3(4697.079f, -2895.60132f, 6.70761f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_ANGELS",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(4693.307f, -2895.25049f, 6.704528f),
                                            Heading = 265.0675f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(4693.20654f, -2896.34985f, 6.666885f),
                                            Heading = 299.0997f,
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
                                    Location = new Vector3(4696.2334f, -2891.597f, 6.342764f),
                                        Heading = 166.2191f,
                                },
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(4698.66357f, -2894.74f, 6.222509f),
                                        Heading = 137.594f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(AodLimbo);
        BlankLocation AodScrap = new BlankLocation()
        {
            Name = "AodScrap",
            Description = "",
            EntrancePosition = new Vector3(4699.105f, -1479.4873f, 8.149014f),
            EntranceHeading = 197.2567f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_ANGELS",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 20,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4699.62354f, -1479.4873f, 8.60018f),
                                            Heading = 193.0026f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4698.18555f, -1480.2312f, 8.600184f),
                                            Heading = 211.2636f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4704.482f, -1474.19629f, 8.600184f),
                                            Heading = 167.0297f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4704.105f, -1475.9082f, 8.600187f),
                                            Heading = 333.5514f,
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
                                        Location = new Vector3(4699.105f, -1485.64819f, 8.149014f),
                                        Heading = 13.78286f,
                                },
                                new GangConditionalLocation()
                                {
                                        Location = new Vector3(4703.058f, -1483.81128f, 8.159445f),
                                        Heading = 17.12824f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(AodScrap);
        BlankLocation AodScrap2 = new BlankLocation()
        {
            Name = "AodScrap2",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(6230.69873f, -2595.13818f, 26.0163441f),
            EntranceHeading = 10.12675f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_ANGELS",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6230.69873f, -2595.13818f, 26.0163441f),
                                            Heading = 358.3793f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6230.089f, -2598.61914f, 26.327034f),
                                            Heading = 183.2842f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRINKING_CASINO_TERRACE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6230.044f, -2600.96436f, 26.327034f),
                                            Heading = 359.2186f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6231.946f, -2601.19116f, 26.327034f),
                                            Heading = 25.82615f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STUPOR_CLUBHOUSE",
                                                "WORLD_HUMAN_STUPOR",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6225.567f, -2592.197f, 25.48455f),
                                            Heading = 220.8934f,
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6227.453f, -2589.292f, 25.48639f),
                                            Heading = 224.1899f,
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(AodScrap2);
        BlankLocation AodVarsity = new BlankLocation()
        {
            Name = "AodVarsity",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(4577.39453f, -2087.26416f, 9.416254f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_ANGELS",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4577.554f, -2083.75732f, 9.963086f),
                                            Heading = 34.17906f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRINKING_FACILITY",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4578.47168f, -2082.936f, 9.962026f),
                                            Heading = 44.30144f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRINKING_CASINO_TERRACE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                         Location = new Vector3(4575.39453f, -2081.08521f, 9.969229f),
                                            Heading = 218.0318f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4571.12256f, -2076.86816f, 9.978879f),
                                            Heading = 188.4748f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STUPOR_CLUBHOUSE",
                                                "WORLD_HUMAN_STUPOR",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4577.39453f, -2087.26416f, 9.416254f),
                                            Heading = 67.0379f,
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4581.06836f, -2085.019f, 9.418966f),
                                            Heading = 92.00579f,
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(AodVarsity);
    }
    private void LostGang()
    {
        BlankLocation LostHonkersCP = new BlankLocation()
        {
            Name = "LostHonkersCP",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(3573.85083f, -3291.72144f, 9.562108f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_LOST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 20,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3570.24463f, -3301.741f, 10.0431824f),
                                            Heading = 268.2005f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3570.357f, -3300.05518f, 10.0432024f),
                                            Heading = 250.9974f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3574.73877f, -3300.17285f, 10.043643f),
                                            Heading = 103.8994f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRINKING_FACILITY",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3572.78076f, -3298.68677f, 10.0433626f),
                                            Heading = 152.0862f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_POT",
                                                "WORLD_HUMAN_DRINKING_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3573.97876f, -3303.488f, 10.075573f),
                                            Heading = 44.6555f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRINKING_CASINO_TERRACE",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3573.85083f, -3291.72144f, 9.562108f),
                                            Heading = 91.08441f,
                                    },
                                new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3573.91772f, -3294.8877f, 9.561486f),
                                            Heading = 91.0808f,
                                    },
                                new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3581.72583f, -3304.343f, 9.562627f),
                                            Heading = 202.7643f,
                                    },
                                new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3584.66162f, -3302.8916f, 9.562606f),
                                            Heading = 203.0308f,
                                    },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(LostHonkersCP);
        BlankLocation LostHardtack = new BlankLocation()
        {
            Name = "LostHardtack",
            Description = "",
            EntrancePosition = new Vector3(3124.504f, -3084.66113f, 11.5656528f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_LOST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 12,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3121.79785f, -3089.98f, 12.5405025f),
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
                                            Location = new Vector3(3122.71777f, -3090.00537f, 12.5406723f),
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
                                        Location = new Vector3(3124.504f, -3084.66113f, 11.5656528f),
                                        Heading = 182.3113f,
                                },
                                new GangConditionalLocation()
                                {
                                        Location = new Vector3(3127.71777f, -3085.78931f, 11.5651722f),
                                        Heading = 184.7067f,
                                },
                            },
                    }
                },
        };
        BlankLocationPlaces.Add(LostHardtack);
        BlankLocation LostBlock = new BlankLocation()
        {
            Name = "LostBlock",
            Description = "",
            EntrancePosition = new Vector3(3104.817f, -3217.63184f, 6.635515f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 75f,
            AssignedAssociationID = "AMBIENT_GANG_LOST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3105.73877f, -3211.44116f, 7.101621f),
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
                                            Location = new Vector3(3105.74878f, -3210.5708f, 7.102871f),
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
                                        Location = new Vector3(3104.817f, -3217.63184f, 6.635515f),
                                        Heading = 137.6688f,
                                },
                                new GangConditionalLocation()
                                {
                                        Location = new Vector3(3105.13867f, -3219.487f, 6.652658f),
                                        Heading = 139.2734f,
                                },
                            },
                    }
                },
        };
        BlankLocationPlaces.Add(LostBlock);
        BlankLocation LostBlock2 = new BlankLocation()
        {
            Name = "LostBlock2",
            Description = "",
            EntrancePosition = new Vector3(3152.47778f, -3169.08154f, 6.299114f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 75f,
            AssignedAssociationID = "AMBIENT_GANG_LOST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3152.43481f, -3166.02832f, 6.809007f),
                                            Heading = 355.396f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_MUSICIAN",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3149.963f, -3165.13f, 6.804128f),
                                            Heading = 325.3308f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3151.008f, -3163.548f, 6.792513f),
                                            Heading = 150.9501f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                    },
                                new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3152.412f, -3163.783f, 6.788918f),
                                            Heading = 176.3229f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3153.941f, -3164.4f, 6.781555f),
                                            Heading = 133.9667f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_PARTYING",
                                                "WORLD_HUMAN_DRINKING_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3154.881f, -3165.729f, 6.785803f),
                                            Heading = 88.82678f,
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
                                        Location = new Vector3(3152.47778f, -3169.08154f, 6.299114f),
                                        Heading = 4.019263f,
                                },
                                new GangConditionalLocation()
                                {
                                        Location = new Vector3(3158.14478f, -3164.00781f, 6.22714f),
                                        Heading = 92.16045f,
                                },
                                new GangConditionalLocation()
                                {
                                        Location = new Vector3(3151.921f, -3159.03516f, 6.214825f),
                                        Heading = 185.919f,
                                },
                                new GangConditionalLocation()
                                {
                                        Location = new Vector3(3147.07666f, -3164.3396f, 6.264502f),
                                        Heading = 268.9051f,
                                },
                            },
                    }
                },
        };
        BlankLocationPlaces.Add(LostBlock2);
        BlankLocation LostDiner = new BlankLocation()
        {
            AssignedAssociationID = "AMBIENT_GANG_LOST",
            Name = "LostDiner",
            Description = "",
            EntrancePosition = new Vector3(6139.523f, -2808.59717f, 15.0105124f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6140.1626f, -2813.35f, 15.6634226f),
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
                                            Location = new Vector3(6139.18164f, -2813.19043f, 15.6210222f),
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
                                        Location = new Vector3(6139.523f, -2808.59717f, 15.0105124f),
                                        Heading = 140.0037f,
                                },
                                new GangConditionalLocation()
                                {
                                        Location = new Vector3(6142.18262f, -2810.86084f, 15.1825428f),
                                        Heading = 144.5579f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(LostDiner);
        BlankLocation LostBlock3 = new BlankLocation()
        {
            Name = "LostBlock3",
            Description = "",
            EntrancePosition = new Vector3(3322.95166f, -2981.99219f, 22.3839035f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_LOST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 12,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(3315.99463f, -2986.53638f, 22.9518833f),
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
                                        Location = new Vector3(3315.149f, -2986.612f, 22.9525642f),
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
                                    Location = new Vector3(3322.95166f, -2981.99219f, 22.3839035f),
                                        Heading = 91.32539f,
                                },
                            }
                    }
                },
        };
        BlankLocationPlaces.Add(LostBlock3);
        BlankLocation LostHouse = new BlankLocation()
        {
            Name = "LostHouse",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(3605.86572f, -2906.18115f, 24.270504f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_LOST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 16,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3598.34082f, -2902.268f, 25.4498844f),
                                            Heading = 186.2756f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_DRINKING_CASINO_TERRACE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3605.86572f, -2906.18115f, 24.270504f),
                                            Heading = 1.033443f,
                                    },
                            },
                    },
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3603.86768f, -2906.092f, 24.8715229f),
                                            Heading = 42.96614f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3602.106f, -2904.24561f, 25.1578045f),
                                            Heading = 217.0836f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3598.34082f, -2902.268f, 25.4498844f),
                                            Heading = 186.2756f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_DRINKING_CASINO_TERRACE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3602.20972f, -2902.82251f, 25.3679028f),
                                            Heading = 202.3455f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER",
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3605.86572f, -2906.18115f, 24.270504f),
                                            Heading = 1.033443f,
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
            EntrancePosition = new Vector3(3590.11865f, -2882.87671f, 25.4912434f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 75f,
            AssignedAssociationID = "AMBIENT_GANG_LOST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3590.11865f, -2882.87671f, 25.4912434f),
                                            Heading = 92.13306f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3588.317f, -2882.74951f, 25.4912434f),
                                            Heading = 267.7621f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                                "WORLD_HUMAN_DRINKING_FACILITY",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(LostAlley);
        BlankLocation LostPrison = new BlankLocation()
        {
            Name = "LostPrison",
            Description = "",
            EntrancePosition = new Vector3(4243.473f, -3746.04541f, 2.858823f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 150f,
            AssignedAssociationID = "AMBIENT_GANG_LOST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4243.473f, -3746.04541f, 2.858823f),
                                            Heading = 87.27906f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4243.52734f, -3747.25684f, 2.91554213f),
                                            Heading = 50.52862f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4241.092f, -3744.76929f, 2.89667416f),
                                            Heading = 241.0951f,
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
                                        Location = new Vector3(4238.511f, -3745.72339f, 2.309524f),
                                        Heading = 244.1893f,
                                },
                                new GangConditionalLocation()
                                {
                                        Location = new Vector3(4239.162f, 4239.162f, 2.321898f),
                                        Heading = 227.8739f,
                                },
                            }
                    }
                },
        };
        BlankLocationPlaces.Add(LostPrison);
        BlankLocation LostIndustrial = new BlankLocation()
        {
            Name = "LostIndustrial",
            Description = "",
            EntrancePosition = new Vector3(3269.95166f, -3560.4043f, 2.093902f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_LOST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3270.71484f, -3551.92651f, 2.63065f),
                                            Heading = 177.5765f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3271.546f, -3553.95728f, 2.63065f),
                                            Heading = 5.761901f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3270.00073f, -3553.89966f, 2.63065f),
                                            Heading = 352.9606f,
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
                                    Location = new Vector3(3269.95166f, -3560.4043f, 2.093902f),
                                        Heading = 0.6942713f,
                                },
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(3267.13477f, -3556.56787f, 2.13677716f),
                                        Heading = 336.5857f,
                                },
                            }
                    }
                },
        };
        BlankLocationPlaces.Add(LostIndustrial);
        BlankLocation LostIndustrial2 = new BlankLocation()
        {
            Name = "LostIndustrial2",
            Description = "",
            EntrancePosition = new Vector3(3602.19678f, -3557.29468f, -3.301312f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_LOST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(3588.35474f, -3561.52539f, -2.823536f),
                                            Heading = 220.2268f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(3586.99976f, -3554.64722f, -2.88182f),
                                            Heading = 307.1956f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(3588.26367f, -3553.75537f, -2.941649f),
                                            Heading = 125.545f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(3586.34863f, -3548.62134f, -2.86735487f),
                                            Heading = 46.35078f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(3602.19678f, -3557.29468f, -3.301312f),
                                        Heading = 19.21191f,
                                },
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(3597.96582f, -3558.36768f, -3.326995f),
                                        Heading = 45.02869f,
                                },
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(3594.70264f, -3564.552f, -3.508838f),
                                        Heading = 87.14633f,
                                },
                            }
                    }
                },
        };
        BlankLocationPlaces.Add(LostIndustrial2);
        BlankLocation LostIndustrial3 = new BlankLocation()
        {
            Name = "LostIndustrial3",
            Description = "",
            EntrancePosition = new Vector3(3719.086f, -3756.68628f, 2.563023f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_LOST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3716.169f, -3753.16162f, 3.099139f),
                                            Heading = 186.369f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3717.39575f, -3753.08545f, 3.099139f),
                                            Heading = 185.5175f,
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
                                        Location = new Vector3(3719.086f, -3756.68628f, 2.563023f),
                                        Heading = 126.9956f,
                                },
                                new GangConditionalLocation()
                                {
                                        Location = new Vector3(3714.899f, -3757.12427f, 2.559315f),
                                        Heading = 122.5434f,
                                },
                            }
                    }
                },
        };
        BlankLocationPlaces.Add(LostIndustrial3);
        BlankLocation LostIndustrial4 = new BlankLocation()
        {
            Name = "LostIndustrial4",
            Description = "",
            EntrancePosition = new Vector3(3913.4248f, -3848.586f, 2.310724f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_LOST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3903.13086f, -3855.22388f, 3.788324f),
                                            Heading = 298.4826f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT_UPRIGHT_FACILITY",
                                                "WORLD_HUMAN_MUSICIAN",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3905.54077f, -3853.64014f, 2.836261f),
                                            Heading = 120.449f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRINKING_FACILITY",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3904.608f, -3852.537f, 2.836261f),
                                            Heading = 145.7539f,
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
                                        Location = new Vector3(3913.4248f, -3848.586f, 2.310724f),
                                        Heading = 156.8912f,
                                },
                                new GangConditionalLocation()
                                {
                                        Location = new Vector3(3909.89673f, -3846.63867f, 2.305092f),
                                        Heading = 136.1819f,
                                },
                            }
                    }
                },
        };
        BlankLocationPlaces.Add(LostIndustrial4);
        BlankLocation LostIndustrial5 = new BlankLocation()
        {
            Name = "LostIndustrial5",
            Description = "",
            EntrancePosition = new Vector3(4121.51025f, -3847.511f, 2.316071f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_LOST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4124.89f, -3852.07983f, 3.010483f),
                                            Heading = 5.059163f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRINKING_FACILITY",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4123.74f, -3851.835f, 3.010483f),
                                            Heading = 317.9691f,
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
                                        Location = new Vector3(4121.51025f, -3847.511f, 2.316071f),
                                        Heading = 180.2107f,
                                },
                                new GangConditionalLocation()
                                {
                                        Location = new Vector3(4125.65234f, -3846.67871f, 2.316747f),
                                        Heading = 202.4576f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(LostIndustrial5);
        BlankLocation LostIndustrial6 = new BlankLocation()
        {
            Name = "LostIndustrial6",
            Description = "",
            EntrancePosition = new Vector3(3957.20166f, -3608.61914f, 2.93179917f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_LOST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3957.20166f, -3608.61914f, 2.93179917f),
                                            Heading = 178.1543f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRINKING_FACILITY",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3955.81641f, -3608.65356f, 2.932394f),
                                            Heading = 181.2816f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                            },
                    }
            },
        };
        BlankLocationPlaces.Add(LostIndustrial6);
        BlankLocation LostIndustrial7 = new BlankLocation()
        {
            Name = "LostIndustrial7",
            Description = "",
            EntrancePosition = new Vector3(3919.63965f, -3524.40112f, 2.923587f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 75f,
            AssignedAssociationID = "AMBIENT_GANG_LOST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3919.63965f, -3524.40112f, 2.923587f),
                                            Heading = 113.3279f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRINKING_FACILITY",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3920.08081f, -3525.36938f, 2.923587f),
                                            Heading = 114.6982f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                            },
                    }
            },
        };
        BlankLocationPlaces.Add(LostIndustrial7);
        BlankLocation LostBlock4 = new BlankLocation()
        {
            Name = "LostBlock4",
            Description = "",
            EntrancePosition = new Vector3(3816.68774f, -3329.77441f, 6.29778528f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_LOST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3816.68774f, -3329.77441f, 6.29778528f),
                                            Heading = 263.4167f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRINKING_FACILITY",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3816.733f, -3331.14551f, 6.29778528f),
                                            Heading = 267.2366f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                            },
                    }
            },
        };
        BlankLocationPlaces.Add(LostBlock4);
        BlankLocation LostDiner2 = new BlankLocation()
        {
            Name = "LostDiner2",
            Description = "",
            EntrancePosition = new Vector3(3699.81177f, -3320.14648f, 5.709759f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_LOST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3704.479f, -3317.14f, 6.420491f),
                                            Heading = 171.7371f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRINKING_FACILITY",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3704.155f, -3318.677f, 6.420492f),
                                            Heading = 344.4261f,
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
                                    Location = new Vector3(3699.81177f, -3320.14648f, 5.709759f),
                                        Heading = 270.4117f,
                                },
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(3699.58081f, -3317.00757f, 5.709965f),
                                        Heading = 272.6684f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(LostDiner2);

        // Beachwood City Lost
        BlankLocation LostStreets1 = new BlankLocation()
        {
            Name = "LostStreets1",
            FullName = "",
            Description = "Lost Street",
            EntrancePosition = new Vector3(6653.63867f, -3070.61157f, 24.5524139f),
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_LOST",
            PossibleGroupSpawns = new List<ConditionalGroup>() {
            new ConditionalGroup() {
            Name = "",
            Percentage = defaultSpawnPercentage,
            MinHourSpawn = 12,
            MaxHourSpawn = 20,
            PossiblePedSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(6664.49561f, -3073.18921f, 23.0878334f),
            Heading = -90.89314f,
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_HANG_OUT_STREET",
            "WORLD_HUMAN_STAND_MOBILE",
            },
            },
            new GangConditionalLocation() {
            Location = new Vector3(6664.51465f, -3073.96753f, 23.0051231f),
            Heading = -67.1198f,
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_STAND_IMPATIENT_FACILITY",
            "WORLD_HUMAN_STAND_MOBILE",
            },
            },
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(6653.63867f, -3070.61157f, 24.5524139f),
            Heading = 103.0923f,
            ForcedScenarios = new List<String>() {
            },
            },
            },
            },
            },
        };
        BlankLocationPlaces.Add(LostStreets1);
        BlankLocation LostStreets2 = new BlankLocation()
        {
            Name = "LostStreets2",
            FullName = "",
            Description = "Lost Street2",
            EntrancePosition = new Vector3(6719.817f, -3118.64746f, 22.805233f),
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_LOST",
            PossibleGroupSpawns = new List<ConditionalGroup>() {
            new ConditionalGroup() {
            Name = "",
            Percentage = defaultSpawnPercentage,
            MinHourSpawn = 18,
            MaxHourSpawn = 24,
            PossiblePedSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(6723.528f, -3117.538f, 23.1267338f),
            Heading = 104.7972f,
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_HANG_OUT_STREET",
            "WORLD_HUMAN_STAND_MOBILE",
            },
            },
            new GangConditionalLocation() {
            Location = new Vector3(6723.47363f, -3118.5542f, 23.1267338f),
            Heading = 82.25832f,
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_STAND_IMPATIENT_FACILITY",
            "WORLD_HUMAN_STAND_MOBILE",
            },
            },
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(6719.817f, -3118.64746f, 22.805233f),
            Heading = -164.1557f,
            ForcedScenarios = new List<String>() {
            },
            },
            },
            },
            },
        };
        BlankLocationPlaces.Add(LostStreets2);
        BlankLocation LostBurgershot = new BlankLocation()
        {
            Name = "LostBurgershot",
            Description = "",
            EntrancePosition = new Vector3(6810.957f, -3024.79736f, 21.5399132f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_LOST",
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
                                        Location = new Vector3(6815.925f, -3024.00146f, 22.3794727f),
                                            Heading = 355.8784f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(6815.89648f, -3022.833f, 22.3794727f),
                                            Heading = 180.3175f,
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
                                    Location = new Vector3(6810.957f, -3024.79736f, 21.5399132f),
                                    Heading = 272.5944f,
                                },
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(6810.863f, -3021.038f, 21.64194f),
                                    Heading = 267.1851f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(LostBurgershot);
        BlankLocation LostBlockBW = new BlankLocation()
        {
            Name = "LostBlockBW",
            Description = "",
            EntrancePosition = new Vector3(6735.277f, -3036.94116f, 24.2919044f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_LOST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6735.277f, -3036.94116f, 24.2919044f),
                                            Heading = 269.1364f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6735.28271f, -3037.71631f, 24.2927628f),
                                            Heading = 272.5494f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(LostBlockBW);
        BlankLocation LostBlockBW2 = new BlankLocation()
        {
            Name = "LostBlockBW2",
            Description = "",
            EntrancePosition = new Vector3(6718.63574f, -3084.89673f, 23.0185928f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_LOST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6718.63574f, -3084.89673f, 23.0185928f),
                                            Heading = 358.9816f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6717.78174f, -3084.82446f, 23.0185928f),
                                            Heading = 356.9689f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(LostBlockBW2);
        BlankLocation LostBlockBW3 = new BlankLocation()
        {
            Name = "LostBlockBW3",
            Description = "",
            EntrancePosition = new Vector3(6699.57861f, -2990.90869f, 24.2485733f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_LOST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(6694.483f, -3012.62939f, 24.3016129f),
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
                                        Location = new Vector3(6693.22852f, -3012.56274f, 24.302784f),
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
                                    Location = new Vector3(6699.57861f, -2990.90869f, 24.2485733f),
                                    Heading = 259.8718f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(LostBlockBW3);

    }
    private void UptownGang()
    {
        BlankLocation UptownCowboy = new BlankLocation()
        {
            Name = "UptownCowboy",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(4870.299f, -1697.24414f, 19.6389236f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_UPTOWN",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4869.255f, -1695.96411f, 20.2890835f),
                                            Heading = 213.0483f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_INSPECT_CROUCH",
                                                "WORLD_HUMAN_INSPECT_STAND",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4870.975f, -1695.3042f, 20.3054733f),
                                            Heading = 160.4086f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4870.127f, -1697.87109f, 19.6364841f),
                                            Heading = 297.8694f,
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4870.84473f, -1700.07422f, 19.6288929f),
                                            Heading = 294.9358f,
                                    },
                        },
                    },
            },
        };
        BlankLocationPlaces.Add(UptownCowboy);
        BlankLocation UptownBlock = new BlankLocation()
        {
            Name = "UptownBlock",
            Description = "",
            EntrancePosition = new Vector3(5046.39746f, -1616.66919f, 19.7636738f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_UPTOWN",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5046.109f, -1610.29224f, 20.566473f),
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
                                            Location = new Vector3(5047.49463f, -1610.62207f, 20.566473f),
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
                                    Location = new Vector3(5046.39746f, -1616.66919f, 19.7636738f),
                                        Heading = 29.50385f,
                                },
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(5048.933f, -1616.58228f, 19.920023f),
                                        Heading = 27.5295f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(UptownBlock);
        BlankLocation UptownBlock2 = new BlankLocation()
        {
            Name = "UptownBlock2",
            Description = "",
            EntrancePosition = new Vector3(5044.07666f, -1584.60815f, 18.7540035f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_UPTOWN",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 12,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5044.07666f, -1584.60815f, 18.7540035f),
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
                                            Location = new Vector3(5043.02441f, -1584.71118f, 18.7540035f),
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
                                        Location = new Vector3(5081.063f, -1584.19312f, 17.9842739f),
                                        Heading = 357.8248f,
                                        IsEmpty = false,
                                        MinHourSpawn = 20,
                                        MaxHourSpawn = 4,
                                },
                                new GangConditionalLocation()
                                {
                                        Location = new Vector3(5083.73975f, -1584.60522f, 18.0362835f),
                                        Heading = 1.011264f,
                                        IsEmpty = false,
                                        MinHourSpawn = 20,
                                        MaxHourSpawn = 4,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(UptownBlock2);
    }

    // -------  The Mafioso and The Russians --------
    private void AncelottinGang()
    {
        BlankLocation AncelottiHonkers = new BlankLocation()
        {
            Name = "AncelottiHonkers",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(3595.05884f, -3240.018f, 9.363475f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            InteriorID = 67586,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_ANCELOTTI",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3607.53979f, -3231.16187f, 10.0166426f),
                                            Heading = 72.11568f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_GUARD_STAND_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3607.40674f, -3228.35571f, 10.0180225f),
                                            Heading = 100.7823f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_GUARD_STAND_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3606.661f, -3226.41f, 10.02782f),
                                            Heading = 174.2357f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_CLIPBOARD","WORLD_HUMAN_DRUG_DEALER"
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3614.067f, -3233.104f, 10.0100126f),
                                            Heading = 91.55523f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_GUARD_STAND_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3605.816f, 3605.816f, 10.0100126f),
                                            Heading = 1.412311f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_GUARD_STAND_FACILITY",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3595.05884f, -3240.018f, 9.363475f),
                                            Heading = 181.2512f,
                                    },
                            },
                    },
                },
        }; //interior
        BlankLocationPlaces.Add(AncelottiHonkers);
        BlankLocation AncelottiHouse = new BlankLocation()
        {
            Name = "AncelottiHouse",
            Description = "",
            EntrancePosition = new Vector3(3675.752f, -2998.06152f, 12.0089226f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 75f,
            AssignedAssociationID = "AMBIENT_GANG_ANCELOTTI",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                                Location = new Vector3(3668.859f, -3001.102f, 14.028883f),
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
                                            Location = new Vector3(3668.72876f, -3002.85059f, 14.028883f),
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
                                    Location = new Vector3(3675.752f, -2998.06152f, 12.0089226f),
                                    Heading = 270.27f,
                                },
                            },
                    }
            },
        };
        BlankLocationPlaces.Add(AncelottiHouse);
        BlankLocation AncelottiPizzaThis = new BlankLocation()
        {
            Name = "AncelottiPizzaThis",
            Description = "",
            EntrancePosition = new Vector3(3630.33081f, -2744.49316f, 24.2709427f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_ANCELOTTI",
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
                                            Location = new Vector3(3631.05273f, -2750.038f, 24.8040638f),
                                            Heading = 79.92148f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3629.983f, -2749.976f, 24.8999329f),
                                            Heading = 266.2858f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 19,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3642.102f, -2767.25f, 23.07158f),
                                            Heading = 262.4773f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3643.628f, -2767.31f, 23.05955f),
                                            Heading = 85.96592f,
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
                                    Location = new Vector3(3645.67f, -2771.244f, 22.67202f),
                                    Heading = 106.3987f,
                                },
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(3650.169f, -2758.637f, 22.20404f),
                                    Heading = 1.509018f,
                                    IsEmpty = false,
                                },
                            },
                    }
            },
        };
        BlankLocationPlaces.Add(AncelottiPizzaThis);
        BlankLocation AncelottiHouse2 = new BlankLocation()
        {
            AssignedAssociationID = "AMBIENT_GANG_ANCELOTTI",
            Name = "AncelottiHouse2",
            Description = "",
            EntrancePosition = new Vector3(3677.10474f, -2515.9873f, 24.9013729f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 100f,
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3670.0166f, -2511.27124f, 25.7558136f),
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
                                            Location = new Vector3(3670.48975f, -2510.42725f, 25.7558136f),
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
                                    Location = new Vector3(3677.10474f, -2515.9873f, 24.9013729f),
                                    Heading = 155.8833f,
                                },
                            },
                    }
            },
        };
        BlankLocationPlaces.Add(AncelottiHouse2);
        BlankLocation AncelottiCleaners = new BlankLocation()
        {
            Name = "AncelottiCleaners",
            Description = "",
            EntrancePosition = new Vector3(3500.45068f, -2617.602f, 29.4669743f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_ANCELOTTI",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 22,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3500.45068f, -2617.602f, 29.4669743f),
                                            Heading = 284.3813f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3500.7478f, -2618.769f, 29.4667931f),
                                            Heading = 302.83f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_AA_SMOKE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(AncelottiCleaners);
        BlankLocation AncelottiGarages = new BlankLocation()
        {
            Name = "AncelottiGarages",
            Description = "",
            EntrancePosition = new Vector3(3734.56982f, -2482.73633f, 22.8923435f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 75f,
            AssignedAssociationID = "AMBIENT_GANG_ANCELOTTI",
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
                                            Location = new Vector3(3733.961f, -2485.67017f, 23.5466728f),
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
                                            Location = new Vector3(3732.18481f, -2484.77832f, 23.5476837f),
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
                                    Location = new Vector3(3734.56982f, -2482.73633f, 22.8923435f),
                                    Heading = 63.58024f,
                                },
                            },
                    }
                },
        };
        BlankLocationPlaces.Add(AncelottiGarages);
        BlankLocation AncelottiFuelDepot = new BlankLocation()
        {
            Name = "AncelottiFuelDepot",
            Description = "",
            EntrancePosition = new Vector3(4780.84961f, -1467.3501f, 8.127784f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_ANCELOTTI",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 20,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4776.28174f, -1462.20313f, 8.747082f),
                                            Heading = 174.4471f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4778.11963f, -1462.8811f, 8.747085f ),
                                            Heading = 170.4194f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4777.76465f, -1464.70215f, 8.747085f),
                                            Heading = 355.9569f,
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
                                    Location = new Vector3(4780.84961f, -1467.3501f, 8.127784f),
                                    Heading = 359.8764f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(AncelottiFuelDepot);
        BlankLocation AncelottiWaste = new BlankLocation()
        {
            Name = "AncelottiWaste",
            Description = "",
            EntrancePosition = new Vector3(5735.435f, -2985.355f, 8.225076f),
            EntranceHeading = 86.3039f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_ANCELOTTI",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5741.482f, -2982.31519f, 8.896468f),
                                            Heading = 96.6902f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5741.44629f, -2980.77173f, 8.934802f),
                                            Heading = 88.26804f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5737.471f, -2989.392f, 8.766745f),
                                            Heading = 28.52622f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5745.125f, -2986.489f, 11.79328f),
                                            Heading = 86.3039f,
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
                                    Location = new Vector3(5735.435f, -2985.355f, 8.225076f),
                                    Heading = 278.9428f,
                                    IsEmpty = true,
                                },
                            }
                    }
                },
        };
        BlankLocationPlaces.Add(AncelottiWaste);
        BlankLocation AncelottiIndustrial = new BlankLocation()
        {
            Name = "AncelottiIndustrial",
            Description = "",
            EntrancePosition = new Vector3(3966.856f, -3448.02759f, 3.30133915f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_ANCELOTTI",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3966.87451f, -3453.65112f, 3.752679f),
                                            Heading = 2.721417f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3963.789f, -3453.7666f, 4.652634f),
                                            Heading = 292.1777f,
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
                                    Location = new Vector3(3966.856f, -3448.02759f, 3.30133915f),
                                    Heading = 93.84917f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(AncelottiIndustrial);
        BlankLocation AncelottiDiner = new BlankLocation()
        {
            Name = "AncelottiDiner",
            Description = "",
            EntrancePosition = new Vector3(3821.48877f, -2743.74414f, 11.8599625f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_ANCELOTTI",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3824.4458f, -2739.44824f, 12.4793425f),
                                            Heading = 88.76724f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3824.56787f, -2738.23413f, 12.4793425f),
                                            Heading = 89.70014f,
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
                                    Location = new Vector3(3821.48877f, -2743.74414f, 11.8599625f),
                                    Heading = 272.4017f,
                                },
                            },
                    }
                },
        };
        BlankLocationPlaces.Add(AncelottiDiner);
        BlankLocation AncelottiAlley = new BlankLocation()
        {
            Name = "AncelottiAlley",
            Description = "",
            EntrancePosition = new Vector3(3669.25586f, -2611.09619f, 23.2729645f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 75f,
            AssignedAssociationID = "AMBIENT_GANG_ANCELOTTI",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(3658.71777f, -2624.22925f, 24.6590843f),
                                            Heading = 304.5884f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(3659.731f, -2624.94019f, 24.6590843f),
                                            Heading = 323.7039f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(3660.982f, -2623.04517f, 24.6590843f),
                                            Heading = 131.4742f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(3669.25586f, -2611.09619f, 23.2729645f),
                                    Heading = 147.0124f,
                                },
                            },
                    }
                },
        };
        BlankLocationPlaces.Add(AncelottiAlley);
        BlankLocation AncelottiResta = new BlankLocation()
        {
            Name = "AncelottiResta",
            Description = "",
            EntrancePosition = new Vector3(3856.73779f, -3010.98267f, 9.59382248f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_ANCELOTTI",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3854.4707f, -3016.50757f, 10.1125221f),
                                            Heading = 4.50831f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3853.56177f, -3016.71143f, 10.1268225f),
                                            Heading = 3.604644f,
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
                                    Location = new Vector3(3856.73779f, -3010.98267f, 9.59382248f),
                                    Heading = 268.5591f,
                                },
                            },
                    }
            },
        };
        BlankLocationPlaces.Add(AncelottiResta);
        BlankLocation AncelottiMail = new BlankLocation()
        {
            Name = "AncelottiMail",
            Description = "",
            EntrancePosition = new Vector3(3993.106f, -2766.931f, 4.102825f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_ANCELOTTI",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3994.41919f, -2770.23975f, 4.500678f),
                                            Heading = 93.01224f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3994.4458f, -2771.07324f, 4.500678f),
                                            Heading = 95.53416f,
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
                                    Location = new Vector3(3993.106f, -2766.931f, 4.102825f),
                                    Heading = 307.7268f,
                                },
                            },
                    }
                },
        };
        BlankLocationPlaces.Add(AncelottiMail);
    }
    private void GambettiGang()
    {
        BlankLocation GambettiMarioBY = new BlankLocation()
        {
            Name = "GambettiMarioBY",
            Description = "",
            EntrancePosition = new Vector3(5022.429f, -3517.10815f, 14.6786127f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 75f,
            AssignedAssociationID = "AMBIENT_GANG_GAMBETTI",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5022.429f, -3517.10815f, 14.6786127f),
                                            Heading = 91.11575f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5021.187f, -3517.12451f, 14.6732826f),
                                            Heading = 268.7274f,
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
                                            MaxWantedLevelSpawn = 4,
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(GambettiMarioBY);
        BlankLocation GambettiStreet = new BlankLocation()
        {
            Name = "GambettiStreet",
            Description = "",
            EntrancePosition = new Vector3(4987.18555f, -3514.442f, 14.0562925f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_GAMBETTI",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4982.729f, -3513.509f, 14.6709423f),
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
                                            Location = new Vector3(4983.759f, -3513.601f, 14.6750927f),
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
                                    Location = new Vector3(4987.18555f, -3514.442f, 14.0562925f),
                                    Heading = 180.3486f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(GambettiStreet);
        BlankLocation GambettiAlDenti = new BlankLocation()
        {
            Name = "GambettiAlDenti",
            Description = "",
            EntrancePosition = new Vector3(5126.393f, -3887.39038f, 14.1199121f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_GAMBETTI",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5120.26855f, -3886.89453f, 14.761323f),
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
                                            Location = new Vector3(5119.23535f, -3887.3313f, 14.7610226f),
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
                                    Location = new Vector3(5126.393f, 5126.393f, 14.1199121f),
                                    Heading = 181.1783f,
                                },
                            }
                    }
                },
        };
        BlankLocationPlaces.Add(GambettiAlDenti);
        BlankLocation GambettiSuffolk = new BlankLocation()
        {
            Name = "GambettiSuffolk",
            Description = "",
            EntrancePosition = new Vector3(4918.6543f, -3541.03955f, 13.6867723f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_GAMBETTI",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                     new ConditionalGroup()
                     {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4911.374f, -3542.4978f, 14.6200628f),
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
                                            Location = new Vector3(4911.32861f, -3541.58179f, 14.6196327f),
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
                                    Location = new Vector3(4918.6543f, -3541.03955f, 13.6867723f),
                                    Heading = 359.9147f,
                                },
                            }
                     }
            },
        };
        BlankLocationPlaces.Add(GambettiSuffolk);
        BlankLocation GambettiCityHall = new BlankLocation()
        {
            Name = "GambettiCityHall",
            Description = "",
            EntrancePosition = new Vector3(5077.96973f, -3585.2312f, 15.2022429f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 75f,
            AssignedAssociationID = "AMBIENT_GANG_GAMBETTI",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5077.96973f, -3585.2312f, 15.2022429f),
                                            Heading = 177.4202f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5076.863f, -3585.29736f, 15.2022429f),
                                            Heading = 181.4519f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(GambettiCityHall);
        BlankLocation GambettiCityHall2 = new BlankLocation()
        {

            Name = "GambettiCityHall2",
            Description = "",
            EntrancePosition = new Vector3(4969.00146f, -3571.48877f, 14.7656126f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_GAMBETTI",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4969.00146f, -3571.48877f, 14.7656126f),
                                            Heading = 1.074511f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4970.23975f, -3571.55762f, 14.7644825f),
                                            Heading = 359.1416f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(GambettiCityHall2);
        BlankLocation GambettiFeldspar = new BlankLocation()
        {
            Name = "GambettiFeldspar",
            Description = "",
            EntrancePosition = new Vector3(4855.1875f, -3591.66528f, 5.26729727f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_GAMBETTI",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4855.1875f, -3591.66528f, 5.26729727f),
                                            Heading = 114.5125f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4854.821f, -3590.92285f, 5.227274f),
                                            Heading = 113.219f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(GambettiFeldspar);
        BlankLocation GambettiErsatz = new BlankLocation()
        {
            Name = "GambettiErsatz",
            Description = "",
            EntrancePosition = new Vector3(4952.01074f, -3460.74829f, 13.8397722f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_GAMBETTI",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4957.46875f, -3461.68213f, 14.4833422f),
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
                                            Location = new Vector3(4957.602f, -3462.619f, 14.4843826f),
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
                                    Location = new Vector3(4952.01074f, -3460.74829f, 13.8397722f),
                                    Heading = 358.2276f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(GambettiErsatz);
        BlankLocation GambettiTriangle = new BlankLocation()
        {
            Name = "GambettiTriangle",
            Description = "",
            EntrancePosition = new Vector3(5038.50732f, -3173.0144f, 14.120223f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_GAMBETTI",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5044.98633f, -3169.22f, 14.7681026f),
                                            Heading = 108.1155f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5045.194f, -3170.40771f, 14.7653027f),
                                            Heading = 105.3394f,
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
                                    Location = new Vector3(5038.50732f, -3173.0144f, 14.120223f),
                                    Heading = 195.2053f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(GambettiTriangle);
        BlankLocation GambettiStreet2 = new BlankLocation()
        {
            Name = "GambettiStreet2",
            Description = "",
            EntrancePosition = new Vector3(4760.035f, -3468.899f, 8.707564f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_GAMBETTI",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4768.40234f, -3472.89551f, 9.624664f),
                                            Heading = 79.02915f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4768.3457f, -3474.09253f, 9.613324f),
                                            Heading = 93.95224f,
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
                                    Location = new Vector3(4760.035f, -3468.899f, 8.707564f),
                                    Heading = 177.9075f,
                                },
                            }
                    }
                },
        };
        BlankLocationPlaces.Add(GambettiStreet2);
        BlankLocation GambettiStreet3 = new BlankLocation()
        {
            Name = "GambettiStreet3",
            Description = "",
            EntrancePosition = new Vector3(4813.361f, -3359.01978f, 13.9668026f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_GAMBETTI",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4807.439f, -3357.938f, 14.7016525f),
                                            Heading = 275.0408f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4807.46436f, -3359.22021f, 14.6981926f),
                                            Heading = 269.3138f,
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
                                    Location = new Vector3(4813.361f, -3359.01978f, 13.9668026f),
                                    Heading = 179.4978f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(GambettiStreet3);
        BlankLocation GambettiCityHall3 = new BlankLocation()
        {
            Name = "GambettiCityHall3",
            Description = "",
            EntrancePosition = new Vector3(5032.78f, -3699.68335f, 14.5638027f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_GAMBETTI",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5032.78f, -3699.68335f, 14.5638027f),
                                            Heading = 92.41549f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5031.459f, -3699.59253f, 14.5609827f),
                                            Heading = 265.8537f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                            },
                    }
            },
        };
        BlankLocationPlaces.Add(GambettiCityHall3);
    }
    private void LupisellaGang()
    {
        BlankLocation LupisellaBlock = new BlankLocation()
        {
            Name = "LupisellaBlock",
            Description = "",
            EntrancePosition = new Vector3(6256.155f, -1416.13428f, 12.0531225f),
            EntranceHeading = 228.7458f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_LUPISELLA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6250.963f, -1402.88208f, 20.0865726f),
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
                                            Location = new Vector3(6251.97363f, -1402.85718f, 20.0865726f),
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
                                    Location = new Vector3(6256.155f, -1416.13428f, 12.0531225f),
                                    Heading = 114.3722f,
                                },
                            },
                    }
            },
        };
        BlankLocationPlaces.Add(LupisellaBlock);
        BlankLocation LupisellaCabin = new BlankLocation()
        {
            Name = "LupisellaCabin",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(6400.206f, -1704.81714f, 16.226284f),
            EntranceHeading = 129.2462f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_LUPISELLA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6408.143f, -1707.29f, 16.79671f),
                                            Heading = 142.4327f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6406.517f, -1706.247f, 16.79917f),
                                            Heading = 186.9817f,
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
                                            Location = new Vector3(6413.278f, -1717.431f, 16.50339f),
                                            Heading = 316.243f,
                                    },
                            },
                    },
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 22,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(6407.167f, -1710.362f, 16.81114f),
                                            Heading = 343.3308f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6408.429f, -1710.371f, 16.80642f),
                                            Heading = 1.177412f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_CLIPBOARD_FACILITY",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6405.738f, -1716.678f, 16.40956f),
                                            Heading = 4.626913f,
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
            EntrancePosition = new Vector3(6401.674f, -1538.52612f, 15.9541922f),
            EntranceHeading = 190.3999f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_LUPISELLA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6330.737f, -1586.2312f, 16.7147541f),
                                            Heading = 140.3605f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_GUARD_STAND_CASINO",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6390.011f, -1544.149f, 16.6618f),
                                            Heading = 308.26f,
                                            MinHourSpawn = 18,
                                            MaxHourSpawn = 4,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_CLIPBOARD_FACILITY",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6391.029f, -1542.61328f, 16.6621742f),
                                            Heading = 309.0697f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6388.542f, -1540.10718f, 16.6621742f),
                                            Heading = 254.0794f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_GUARD_STAND_CASINO",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6401.674f, -1538.52612f, 15.9541922f),
                                            Heading = 1.046725f,
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6342.504f, -1592.15112f, 16.2579441f),
                                            Heading = 226.5761f,
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
            EntrancePosition = new Vector3(6385.11963f, -1559.45728f, 17.7215939f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            InteriorID = 113666,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 75f,
            AssignedAssociationID = "AMBIENT_GANG_LUPISELLA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6385.11963f, -1559.45728f, 17.7215939f),
                                            Heading = 222.4488f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_GUARD_STAND_CASINO",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6359.259f, -1583.84131f, 17.7217636f),
                                            Heading = 227.2596f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_GUARD_STAND_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6359.259f, -1583.84131f, 17.7217636f),
                                            Heading = 227.2596f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_GUARD_STAND_FACILITY",
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6338.67871f, -1582.08325f, 16.7217941f),
                                            Heading = 43.06167f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_GUARD_STAND_ARMY",
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        }; //interior
        BlankLocationPlaces.Add(LupisellaTriangleInside);
        BlankLocation LupisellaBlocks2 = new BlankLocation()
        {
            Name = "LupisellaBlocks2",
            Description = "",
            EntrancePosition = new Vector3(6357.53271f, -1448.31421f, 10.9145927f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_LUPISELLA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6357.53271f, -1448.31421f, 10.9145927f),
                                            Heading = 268.5659f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6358.917f, -1448.02124f, 10.9145823f),
                                            Heading = 94.06642f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(LupisellaBlocks2);
        BlankLocation LupisellaFlanger = new BlankLocation()
        {
            Name = "LupisellaFlanger",
            Description = "",
            EntrancePosition = new Vector3(6457.57f, -1425.00708f, 10.9643326f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 75f,
            AssignedAssociationID = "AMBIENT_GANG_LUPISELLA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6457.57f, -1425.00708f, 10.9643326f),
                                            Heading = 271.7417f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6457.59668f, -1423.63428f, 10.9643326f),
                                            Heading = 267.8455f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(LupisellaFlanger);
        BlankLocation LupisellaAlcatraz = new BlankLocation()
        {
            Name = "LupisellaAlcatraz",
            Description = "",
            EntrancePosition = new Vector3(6553.0376f, -1626.77515f, 17.8644733f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_LUPISELLA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 18,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6553.0376f, -1626.77515f, 17.8644733f),
                                            Heading = 90.12063f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6553.114f, -1628.49609f, 17.8644733f),
                                            Heading = 89.7944f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(LupisellaAlcatraz);
        BlankLocation LupisellaBlock3 = new BlankLocation()
        {
            Name = "LupisellaBlock3",
            Description = "",
            EntrancePosition = new Vector3(6285.89258f, -1495.74023f, 10.704813f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_LUPISELLA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6285.89258f, -1495.74023f, 10.704813f),
                                            Heading = 246.3346f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6285.32568f, -1496.70215f, 10.704813f),
                                            Heading = 247.3597f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_AA_SMOKE",
                                            },
                                   },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(LupisellaBlock3);
        BlankLocation LupisellaBodyDump = new BlankLocation()
        {
            Name = "LupisellaBodyDump",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(6548.123f, -1309.47925f, 4.55129f),
            EntranceHeading = 172.5893f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_LUPISELLA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 20,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6546.9917f, -1305.09009f, 5.061245f),
                                            Heading = 325.5471f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_GUARD_STAND_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6545.797f, -1304.2002f, 5.017497f),
                                            Heading = 323.5613f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_COP_IDLES",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6560.72559f, -1280.66626f, 3.190115f),
                                            Heading = 320.2166f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6563.135f, -1278.69507f, 2.81363916f),
                                            Heading = 314.1112f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_INSPECT_STAND",
                                                "WORLD_HUMAN_INSPECT_CROUCH",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6548.123f, -1309.47925f, 4.55129f),
                                            Heading = 338.9202f,
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
            EntrancePosition = new Vector3(5033.682f, -2771.7146f, 14.7130823f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_MESSINA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5033.682f, -2771.7146f, 14.7130823f),
                                            Heading = 267.7945f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5033.69141f, -2772.6377f, 14.7130423f),
                                            Heading = 268.5063f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(MessinaSquare);
        BlankLocation MessinaJunction = new BlankLocation()
        {
            Name = "MessinaJunction",
            Description = "",
            EntrancePosition = new Vector3(4973.509f, -2802.79053f, 14.0497122f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_MESSINA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(4965.269f, -2800.16138f, 14.8159924f),
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
                                        Location = new Vector3(4965.14453f, -2801.29736f, 14.8159924f),
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
                                    Location = new Vector3(4973.509f, -2802.79053f, 14.0497122f),
                                    Heading = 179.2383f,
                                    IsEmpty = true,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(MessinaJunction);
        BlankLocation MessinaPizza = new BlankLocation()
        {
            Name = "MessinaPizza",
            Description = "",
            EntrancePosition = new Vector3(4856.76074f, -2724.09619f, 14.7607622f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_MESSINA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4856.76074f, -2724.09619f, 14.7607622f),
                                            Heading = 358.7756f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4855.98145f, -2724.1062f, 14.7607622f),
                                            Heading = 0.6930794f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(MessinaPizza);
        BlankLocation MessinaTower = new BlankLocation()
        {
            Name = "MessinaTower",
            Description = "",
            EntrancePosition = new Vector3(4776.907f, -2664.8772f, 15.0254726f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_MESSINA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4776.907f, -2664.8772f, 15.0254726f),
                                            Heading = 266.5729f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4776.89941f, -2664.00928f, 15.0254726f),
                                            Heading = 269.2255f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(MessinaTower);
        BlankLocation MessinaHome = new BlankLocation()
        {
            Name = "MessinaHome",
            Description = "",
            EntrancePosition = new Vector3(4775.32129f, -2876.45361f, 11.5795031f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_MESSINA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(4774.32227f, -2886.97241f, 14.0402222f),
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
                                        Location = new Vector3(4773.37939f, -2887.071f, 14.0402222f),
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
                                    Location = new Vector3(4775.32129f, -2876.45361f, 11.5795031f),
                                    Heading = 86.57639f,
                                    IsEmpty = true,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(MessinaHome);
        BlankLocation MessinaRimmers = new BlankLocation()
        {
            Name = "MessinaRimmers",
            Description = "",
            EntrancePosition = new Vector3(4886.59668f, -2855.45264f, 14.0409622f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_MESSINA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(4897.59668f, -2860.34f, 14.8149729f),
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
                                        Location = new Vector3(4898.52734f, -2860.30322f, 14.8149424f),
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
                                    Location = new Vector3(4886.59668f, -2855.45264f, 14.0409622f),
                                    Heading = 357.8925f,
                                    IsEmpty = false,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(MessinaRimmers);
        BlankLocation MessinaCentral = new BlankLocation()
        {
            Name = "MessinaCentral",
            Description = "",
            EntrancePosition = new Vector3(5102.7915f, -2858.49023f, 14.7579823f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_MESSINA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5102.7915f, -2858.49023f, 14.757982f),
                                            Heading = 179.5025f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5101.904f, -2858.51318f, 14.7579622f),
                                            Heading = 178.4214f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(MessinaCentral);
        BlankLocation MessinaCathedral = new BlankLocation()
        {
            Name = "MessinaCathedral",
            Description = "",
            EntrancePosition = new Vector3(5180.7583f, -2758.40137f, 15.7643127f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_MESSINA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5180.7583f, -2758.40137f, 15.7643127f),
                                            Heading = 40.34569f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5179.814f, -2759.175f, 15.7643127f),
                                            Heading = 48.32359f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(MessinaCathedral);
        BlankLocation MessinaPark = new BlankLocation()
        {
            Name = "MessinaPark",
            Description = "",
            EntrancePosition = new Vector3(5087.99951f, -2597.44019f, 13.0766525f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_MESSINA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 12,
                            MaxHourSpawn = 22,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5087.99951f, -2597.44019f, 13.0766525f),
                                            Heading = 100.0038f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5088.253f, -2598.246f, 13.0766525f),
                                            Heading = 89.23592f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(MessinaPark);
        BlankLocation MessinaPark2 = new BlankLocation()
        {
            Name = "MessinaPark2",
            Description = "",
            EntrancePosition = new Vector3(4982.716f, -2585.9873f, 12.4038725f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_MESSINA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 18,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4982.716f, -2585.9873f, 12.4038725f),
                                            Heading = 359.7913f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4983.446f, -2585.97119f, 12.4038725f),
                                            Heading = 358.7981f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(MessinaPark2);
        BlankLocation MessinaWilbert = new BlankLocation()
        {
            Name = "MessinaWilbert",
            Description = "",
            EntrancePosition = new Vector3(4904.515f, -2521.67017f, 10.9607725f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_MESSINA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4904.515f, -2521.67017f, 10.9607725f),
                                            Heading = 1.423842f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4903.70068f, -2521.65527f, 10.9607725f),
                                            Heading = 0.5861487f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_AA_COFFEE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(MessinaWilbert);
        BlankLocation MessinaOpium = new BlankLocation()
        {
            Name = "MessinaOpium",
            Description = "",
            EntrancePosition = new Vector3(5155.132f, -2514.145f, 14.6703129f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_MESSINA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5155.132f, -2514.145f, 14.6703129f),
                                            Heading = 90.11057f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_DRUG_DEALER",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5155.25049f, -2513.0293f, 14.6792231f),
                                            Heading = 88.46743f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },

                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(MessinaOpium);
        BlankLocation MessinaCompound = new BlankLocation()
        {
            Name = "MessinaCompound",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(5494.206f, -1397.44727f, 17.1477127f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_MESSINA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5490.69238f, -1392.41724f, 18.1613731f),
                                            Heading = 62.83598f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5494.32031f, -1392.22412f, 17.7711239f),
                                            Heading = 273.0719f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5487.64844f, -1393.38818f, 18.1613731f),
                                            Heading = 298.1473f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5487.553f, -1391.45825f, 18.1613731f),
                                            Heading = 244.238f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_POT",
                                                "WORLD_HUMAN_DRUG_DEALER",
                                                "WORLD_HUMAN_DRINKING_FACILITY",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5494.206f, -1397.44727f, 17.1477127f),
                                            Heading = 43.74239f,
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5490.75146f, -1403.64722f, 17.0709133f),
                                            Heading = 95.20958f,
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(MessinaCompound);
        BlankLocation MessinaJerkov = new BlankLocation()
        {
            Name = "MessinaJerkov",
            Description = "",
            EntrancePosition = new Vector3(5330.291f, -2689.43921f, 13.9792824f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_MESSINA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5277.628f, -2701.85718f, 18.6297226f),
                                            Heading = 185.2094f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5279.109f, -2701.95923f, 18.6297226f),
                                            Heading = 180.6683f,
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
                                    Location = new Vector3(5330.291f, -2689.43921f, 13.9792824f),
                                    Heading = 268.1642f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(MessinaJerkov);
        BlankLocation MessinaParking = new BlankLocation()
        {
            Name = "MessinaParking",
            Description = "",
            EntrancePosition = new Vector3(4713.545f, -2817.924f, 9.434187f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_MESSINA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4719.98242f, -2825.25342f, 10.0068922f),
                                            Heading = 274.1806f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_GUARD_STAND",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4718.625f, -2826.27539f, 10.0068922f),
                                            Heading = 7.971545f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4718.56738f, -2824.3833f, 10.0068922f),
                                            Heading = 179.5679f,
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
                                    Location = new Vector3(4713.545f, -2817.924f, 9.434187f),
                                    Heading = 92.48305f,
                                },
                            }
                    }
                },
        };
        BlankLocationPlaces.Add(MessinaParking);
        BlankLocation MessinaPizza2 = new BlankLocation()
        {
            Name = "MessinaPizza2",
            Description = "",
            EntrancePosition = new Vector3(4835.39453f, -2903.80786f, 14.0333023f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_MESSINA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4842.558f, -2904.5957f, 14.7636328f),
                                            Heading = 95.16219f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4842.592f, -2905.30957f, 14.7636023f),
                                            Heading = 91.09475f,
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
                                    Location = new Vector3(4835.39453f, -2903.80786f, 14.0333023f),
                                    Heading = 1.531931f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(MessinaPizza2);
        BlankLocation MessinaStreet = new BlankLocation()
        {
            Name = "MessinaStreet",
            Description = "",
            EntrancePosition = new Vector3(5191.22656f, -3005.77979f, 14.344533f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_MESSINA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5191.083f, -3011.3833f, 14.8626022f),
                                            Heading = 358.564f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_COP_IDLES",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5186.98633f, -3016.31836f, 18.5370731f),
                                            Heading = 87.81215f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5185.023f, -3016.15479f, 18.5370731f),
                                            Heading = 262.006f,
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
                                    Location = new Vector3(5191.22656f, -3005.77979f, 14.344533f),
                                    Heading = 271.6368f,
                                },
                            }
                    }
                },
        };
        BlankLocationPlaces.Add(MessinaStreet);
        BlankLocation MessinaProsper = new BlankLocation()
        {
            Name = "MessinaProsper",
            Description = "",
            EntrancePosition = new Vector3(5284.98438f, -2954.89f, 14.7557926f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_MESSINA",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5284.98438f, -2954.89f, 14.7557926f),
                                            Heading = 130.9118f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5283.552f, -2956.14014f, 14.7558022f),
                                            Heading = 311.8979f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5285.049f, -2955.82446f, 14.7558022f),
                                            Heading = 80.49931f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                            },
                                    },
                            },
                    }
            },
        };
        BlankLocationPlaces.Add(MessinaProsper);
    }
    private void PavanoGang()
    {
        BlankLocation PavanoTower1 = new BlankLocation()
        {
            Name = "PavanoTower1",
            Description = "",
            EntrancePosition = new Vector3(5042.919f, -1949.80115f, 19.7376328f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_PAVANO",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5052.585f, -1954.76819f, 20.4395428f),
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
                                            Location = new Vector3(5053.59863f, -1954.82422f, 20.4395428f),
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
                                    Location = new Vector3(5042.919f, -1949.80115f, 19.7376328f),
                                    Heading = 0.2984261f,
                                    IsEmpty = false,
                                },
                            }
                    }
                },
        };
        BlankLocationPlaces.Add(PavanoTower1);
        BlankLocation PavanoTower2 = new BlankLocation()
        {

            Name = "PavanoTower2",
            Description = "",
            EntrancePosition = new Vector3(5088.237f, -1956.42822f, 26.0278244f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_PAVANO",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 18,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5088.237f, -1956.42822f, 26.0278244f),
                                            Heading = 182.411f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5087.374f, -1956.44421f, 26.027113f),
                                            Heading = 182.2232f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(PavanoTower2);
        BlankLocation PavanoPark = new BlankLocation()
        {
            Name = "PavanoPark",
            Description = "",
            EntrancePosition = new Vector3(4956.80469f, -2099.79321f, 14.0241022f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_PAVANO",
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
                                        Location = new Vector3(4957.56348f, -2105.82813f, 14.6976528f),
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
                                        Location = new Vector3(4958.29053f, -2105.86914f, 14.7002325f),
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
                                    Location = new Vector3(4956.80469f, -2099.79321f, 14.0241022f),
                                    Heading = 268.3399f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(PavanoPark);
        BlankLocation PavanoPark2 = new BlankLocation()
        {
            Name = "PavanoPark2",
            Description = "",
            EntrancePosition = new Vector3(4862.861f, -2111.871f, 13.5757122f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_PAVANO",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 10,
                            MaxHourSpawn = 18,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4862.861f, -2111.871f, 13.5757122f),
                                            Heading = 156.6432f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4863.47656f, -2112.20313f, 13.5778627f),
                                            Heading = 139.2261f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(PavanoPark2);
        BlankLocation PavanoStateB = new BlankLocation()
        {
            Name = "PavanoStateB",
            Description = "",
            EntrancePosition = new Vector3(5160.44336f, -2018.79822f, 20.4355831f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_PAVANO",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 10,
                            MaxHourSpawn = 22,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5160.44336f, -2018.79822f, 20.4355831f),
                                            Heading = 1.306429f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5161.63232f, -2018.77917f, 20.4355526f),
                                            Heading = 359.2593f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },                                   
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(PavanoStateB);
        BlankLocation PavanoAlley = new BlankLocation()
        {
            Name = "PavanoAlley",
            Description = "",
            EntrancePosition = new Vector3(5265.3877f, -2198.12622f, 13.9401722f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_PAVANO",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(5260.497f, -2196.391f, -2196.391f),
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
                                        Location = new Vector3(5262.283f, -2196.458f, 14.7134523f),
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
                                    Location = new Vector3(5265.3877f, -2198.12622f, 13.9401722f),
                                    Heading = 268.0463f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(PavanoAlley);
        BlankLocation PavanoDeKoch = new BlankLocation()
        {
            Name = "PavanoDeKoch",
            Description = "",
            EntrancePosition = new Vector3(5301.33838f, -2283.329f, 14.7136526f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_PAVANO",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5301.33838f, -2283.329f, 14.7136526f),
                                            Heading = 269.9776f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5301.213f, -2282.458f, 14.7136526f),
                                            Heading = 269.0881f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(PavanoDeKoch);
        BlankLocation PavanoView = new BlankLocation()
        {
            Name = "PavanoView",
            Description = "",
            EntrancePosition = new Vector3(5327.049f, -2403.27319f, 14.7131023f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_PAVANO",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5327.049f, -2403.27319f, 14.7131023f),
                                            Heading = 267.6965f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5327.047f, -2404.15918f, 14.7123222f),
                                            Heading = 270.2363f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(PavanoView);
        BlankLocation PavanoAlley2 = new BlankLocation()
        {
            Name = "PavanoAlley2",
            Description = "",
            EntrancePosition = new Vector3(5194.019f, -1961.74719f, 20.3817329f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_PAVANO",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5194.019f, -1961.74719f, 20.3817329f),
                                            Heading = 357.0328f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5193.988f, -1960.33716f, 20.3877544f),
                                            Heading = 179.8688f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(PavanoAlley2);
        BlankLocation PavanoColumbus = new BlankLocation()
        {
            Name = "PavanoColumbus",
            Description = "",
            EntrancePosition = new Vector3(5073.038f, -2071.873f, 14.768693f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_PAVANO",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5073.038f, -2071.873f, 14.768693f),
                                            Heading = 182.8932f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING",
                                            }
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5072.184f, 5072.184f, 14.7681227f),
                                            Heading = 181.8349f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(PavanoColumbus);
        BlankLocation PavanoPyrite = new BlankLocation()
        {
            Name = "PavanoPyrite",
            Description = "",
            EntrancePosition = new Vector3(5297.32031f, -2514.582f, 13.9838829f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_PAVANO",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5300.44043f, -2505.66821f, 14.7139225f),
                                            Heading = 271.3646f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5300.379f, -2504.67334f, 14.7139225f),
                                            Heading = 266.9984f,
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
                                    Location = new Vector3(5297.32031f, -2514.582f, 13.9838829f),
                                    Heading = 274.0092f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(PavanoPyrite);
        BlankLocation PavanoDiner = new BlankLocation()
        {
            Name = "PavanoDiner",
            Description = "",
            EntrancePosition = new Vector3(5291.239f, -2624.101f, 14.7164431f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_PAVANO",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5291.239f, -2624.101f, 14.7164431f),
                                            Heading = 181.8378f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5292.082f, -2623.916f, 14.7164526f),
                                            Heading = 181.5779f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(PavanoDiner);
        BlankLocation PavanoPerseus = new BlankLocation()
        {
            Name = "PavanoPerseus",
            Description = "",
            EntrancePosition = new Vector3(5213.44531f, -2445.77124f, 14.6825523f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_PAVANO",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5213.44531f, -2445.77124f, 14.6825523f),
                                            Heading = 359.9861f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5214.18945f, -2445.77515f, 14.673893f),
                                            Heading = 0.9142291f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(PavanoPerseus);
        BlankLocation PavanoUGCP = new BlankLocation()
        {
            Name = "PavanoUGCP",
            Description = "",
            EntrancePosition = new Vector3(5293.937f, -2140.93018f, 2.326204f),
            EntranceHeading = 83.7203f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_PAVANO",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5291.52344f, -2138.11719f, 2.912857f),
                                            Heading = 87.16341f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_GUARD_STAND_CLUBHOUSE",
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5297.143f, -2139.26025f, 2.912857f),
                                            Heading = 176.0741f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5297.002f,-2140.79517f, 2.912857f),
                                            Heading = 350.1467f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5298.65625f, -2140.42334f, 2.912857f),
                                            Heading = 56.44848f,
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
                                    Location = new Vector3(5293.937f, -2140.93018f, 2.326204f),
                                    Heading = 179.3236f,
                                },
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(5288.535f, -2140.913f, 2.32655716f),
                                    Heading = 247.8056f,
                                },
                                new GangConditionalLocation()
                                {
                                    Location = new Vector3(5291.86865f, -2132.34814f, 2.326647f),
                                    Heading = 274.7059f,
                                    IsEmpty = false,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(PavanoUGCP);
    }
    private void PetrovicGang()
    {
        BlankLocation PetrovicCarServices = new BlankLocation()
        {
            Name = "PetrovicCarServices",
            FullName = "",
            Description = "Petrovic at Express Car Services",
            EntrancePosition = new Vector3(6010.504f, -3525.67236f, 15.5983829f),
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
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
            Location = new Vector3(6010.504f, -3525.67236f, 15.5983829f),
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
                Location = new Vector3(6010.712f, -3526.91626f, 15.5983829f),
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
            Location = new Vector3(6006.78271f, -3529.61523f, 14.6627626f),
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
            Location = new Vector3(6001.149f, -3517.44922f, 15.3373623f),
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
            Location = new Vector3(6001.291f, -3519.157f, 15.3373623f),
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
            Location = new Vector3(6002.173f, -3517.914f, 15.3373623f),
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
            Location = new Vector3(5999.718f, -3524.35376f, 15.3373623f),
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
            Location = new Vector3(5998.32275f, -3525.63623f, 15.0156126f),
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
            EntrancePosition = new Vector3(6066.7417f, -3737.38916f, 14.933033f),
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
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
            Location = new Vector3(6066.7417f, -3737.38916f, 14.933033f),
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
            Location = new Vector3(6068.2207f, -3737.52539f, 14.9750729f),
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
            Location = new Vector3(6062.447f, -3739.13965f, 14.1098824f),
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
            EntrancePosition = new Vector3(6115.982f, -3749.97827f, 15.2584429f),
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
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
            Location = new Vector3(6115.982f, -3749.97827f, 15.2584429f),
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
            Location = new Vector3(6115.921f, -3748.887f, 15.2800522f),
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
            Location = new Vector3(6119.64355f, -3756.90771f, 14.4979925f),
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
            EntrancePosition = new Vector3(5972.085f, -3798.73853f, 9.500408f),
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
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
            Location = new Vector3(5972.085f, -3798.73853f, 9.500408f),
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
            Location = new Vector3(5966.089f, -3806.402f, 9.2140255f),
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
            Description = "4 Petrovic at home",
            EntrancePosition = new Vector3(6303.506f, -3704.00171f, 17.797554f),
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 75f,
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
            Location = new Vector3(6303.506f, -3704.00171f, 17.797554f),
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
            Location = new Vector3(6291.7207f, -3707.09424f, 13.6058722f),
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
            Location = new Vector3(6291.751f, -3705.36548f, 13.6047926f),
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
            Location = new Vector3(6290.36963f, -3706.41284f, 13.6064825f),
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
            Location = new Vector3(6301.13867f, -3699.732f, 13.4228325f),
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
            Location = new Vector3(6297.47754f, -3702.12256f, 12.8220329f),
            Heading = 271.1961f,
            AssociationID = "",
            RequiredPedGroup = "",
            RequiredVehicleGroup = "",
            ForcedScenarios = new List<String>() {
            },
            },
            new GangConditionalLocation() {
            Location = new Vector3(6297.10156f, 6297.10156f, 12.9916124f),
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
            EntrancePosition = new Vector3(6130.486f, -3935.19629f, 16.4401932f),
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
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
            Location = new Vector3(6130.486f, -3935.19629f, 16.4401932f),
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
            Location = new Vector3(6131.4375f, -3935.14478f, 16.4401932f),
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
            EntrancePosition = new Vector3(6280.529f, -3899.51172f, 12.9647923f),
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
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
            Location = new Vector3(6279.738f, -3935.312f, 16.4402332f),
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
            Location = new Vector3(6280.778f, -3935.27051f, 16.4402332f),
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
            Location = new Vector3(6280.529f, -3899.51172f, 12.9647923f),
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
            Location = new Vector3(6280.529f, -3899.51172f, 12.9647923f),
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
            EntrancePosition = new Vector3(6148.44f, -3546.67188f, 19.9121132f),
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
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
            Location = new Vector3(6148.44f, -3546.67188f, 19.9121132f),
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
            Location = new Vector3(6148.044f, -3547.643f, 19.816843f),
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
            EntrancePosition = new Vector3(6338.06348f, -3680.08521f, 16.5159035f),
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
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
            Location = new Vector3(6338.06348f, -3680.08521f, 16.5159035f),
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
            Location = new Vector3(6339.129f, -3680.1814f, 16.5389843f),
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
            Location = new Vector3(6342.60449f, -3690.5127f, 16.4390545f),
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
            Location = new Vector3(6342.544f, -3691.45337f, 16.4390545f),
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
            Location = new Vector3(6343.65f, -3690.91333f, 16.4390545f),
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
            EntrancePosition = new Vector3(6507.384f, -3868.146f, 12.2399921f),
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
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
            Location = new Vector3(6490.668f, -3862.169f, 12.889883f),
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
            Location = new Vector3(6507.384f, -3868.146f, 12.2399921f),
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
            Location = new Vector3(6502.487f, -3872.789f, 12.7183828f),
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
            Location = new Vector3(6503.674f, -3872.079f, 12.7180729f),
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
            Location = new Vector3(6503.86865f, -3873.13916f, 12.7172728f),
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
            Location = new Vector3(6507.384f, -3868.146f, 12.2399921f),
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
            EntrancePosition = new Vector3(6026.8f, -3775.46826f, 13.3605328f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            InteriorID = 88834,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_PETROVIC",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 12,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6052.46973f, -3771.694f, 16.277504f),
                                            Heading = 270.0482f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "PROP_HUMAN_BBQ",
                                                "WORLD_HUMAN_DRUG_PROCESSORS_COKE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6042.206f, -3769.6792f, 16.277504f),
                                            Heading = 188.9128f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT",
                                                "WORLD_HUMAN_AA_SMOKE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6042.2417f, -3771.972f, 16.277504f),
                                            Heading = 358.8919f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6043.42773f, -3772.1145f, 16.2774944f),
                                            Heading = 12.9685f,
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
                                            Location = new Vector3(6026.8f, -3775.46826f, 13.3605328f),
                                            Heading = 4.276759f,
                                    },
                            },
                    },
            },
        }; //interior
        BlankLocationPlaces.Add(PetrovicHome);
        BlankLocation PetrovicMasterson = new BlankLocation()
        {
            Name = "PetrovicMasterson",
            Description = "",
            EntrancePosition = new Vector3(6375.11865f, -3557.60132f, 20.7832127f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_PETROVIC",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 12,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(6368.82f, -3555.8728f, 21.5290642f),
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
                                        Location = new Vector3(6368.70264f, -3554.325f, 21.5418739f),
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
                                    Location = new Vector3(6375.11865f, -3557.60132f, 20.7832127f),
                                        Heading = 359.0621f,
                                },
                            },
                    }
                },
        };
        BlankLocationPlaces.Add(PetrovicMasterson);
        BlankLocation PetrovicAlexeis = new BlankLocation()
        {
            Name = "PetrovicAlexeis",
            Description = "",
            EntrancePosition = new Vector3(6085.092f, -3534.85571f, 18.7697029f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_PETROVIC",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 10,
                            MaxHourSpawn = 16,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6085.092f, -3534.85571f, 18.7697029f),
                                            Heading = 265.6195f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6085.05664f, -3533.68433f, 18.8111839f),
                                            Heading = 268.7603f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(PetrovicAlexeis);
        BlankLocation PetrovicLaundromat = new BlankLocation()
        {
            Name = "PetrovicLaundromat",
            Description = "",
            EntrancePosition = new Vector3(6200.51563f, -3578.79761f, 20.2760544f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_PETROVIC",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 12,
                            MaxHourSpawn = 20,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6200.51563f, -3578.79761f, 20.2760544f),
                                            Heading = 104.3423f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6200.86865f, -3579.994f, 20.2505226f),
                                            Heading = 101.0594f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(PetrovicLaundromat);
        BlankLocation PetrovicHome2 = new BlankLocation()
        {
            Name = "PetrovicHome2",
            Description = "",
            EntrancePosition = new Vector3(6143.43359f, -3451.37671f, 24.7161827f),
            EntranceHeading = 0f,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_PETROVIC",
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
                                            Location = new Vector3(6143.43359f, -3451.37671f, 24.7161827f),
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
                                        Location = new Vector3(6146.443f, -3447.19531f, 21.9920044f),
                                        Heading = 197.944f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(PetrovicHome2);
        BlankLocation PetrovicPerestroika2 = new BlankLocation()
        {
            Name = "PetrovicPerestroika2",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(6145.39355f, -3519.32886f, 18.7707729f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            InteriorID = 78338,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 75f,
            ActivateCells = 3,
            AssignedAssociationID = "AMBIENT_GANG_PETROVIC",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 23,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6145.39355f, -3519.32886f, 18.7707729f),
                                            Heading = 179.5305f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_MUSICIAN",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6138.863f, -3505.67578f, 15.8570328f),
                                            Heading = 182.7292f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_COP_IDLES",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6140.759f, -3504.515f, 15.8570223f),
                                            Heading = 141.7805f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6137.24463f, -3514.667f, 18.264904f),
                                            Heading = 181.415f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_GUARD_STAND_CLUBHOUSE",
                                                "WORLD_HUMAN_LEANING",
                                            },
                                    },
                            },
                    },
            },
        }; //interior
        BlankLocationPlaces.Add(PetrovicPerestroika2);
    }

    // -------  The Oriental Connection  --------
    private void KhangpaeGang()
    {
        BlankLocation KkangpaeWaterFront = new BlankLocation()
        {
            Name = "KkangpaeWaterFront",
            Description = "",
            EntrancePosition = new Vector3(4378.45752f, -2268.415f, 4.36652f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 10,
                            MaxHourSpawn = 22,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4378.45752f, -2268.415f, 4.36652f),
                                            Heading = 90.34357f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4376.963f, 4376.963f, 4.36652f),
                                            Heading = 267.5033f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING",
                                                "WORLD_HUMAN_STAND_IMPATIENT",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(KkangpaeWaterFront);
        BlankLocation KkangpaeWaterFront2 = new BlankLocation()
        {
            Name = "KkangpaeWaterFront2",
            Description = "",
            EntrancePosition = new Vector3(4348.03467f, -2419.27734f, 4.21588326f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 75f,
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 16,
                            MaxHourSpawn = 22,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4348.03467f, -2419.27734f, 4.21588326f),
                                            Heading = 213.3994f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4346.884f, -2420.0332f, 4.21588326f),
                                            Heading = 225.0585f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(KkangpaeWaterFront2);
        BlankLocation KkangpaeWaterFront3 = new BlankLocation()
        {
            Name = "KkangpaeWaterFront3",
            Description = "",
            EntrancePosition = new Vector3(4259.32227f, -2503.205f, 3.461267f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 10,
                            MaxHourSpawn = 22,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4259.32227f, -2503.205f, 3.461267f),
                                            Heading = 183.2816f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4258.32227f, -2503.21021f, 3.461267f),
                                            Heading = 187.6818f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(KkangpaeWaterFront3);
        BlankLocation KkangpaeLockup = new BlankLocation()
        {
            Name = "KkangpaeLockup",
            Description = "",
            EntrancePosition = new Vector3(4172.38135f, -2210.04224f, 12.9901428f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4169.5f, -2206.8623f, -2206.8623f),
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
                                            Location = new Vector3(4169.421f, -2207.75928f, 13.5603628f),
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
                                    Location = new Vector3(4172.38135f, -2210.04224f, 12.9901428f),
                                    Heading = 271.9701f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(KkangpaeLockup);
        BlankLocation KkangpaeTattoo = new BlankLocation()
        {
            Name = "KkangpaeTattoo",
            Description = "",
            EntrancePosition = new Vector3(3949.275f, -2134.71436f, 19.4717731f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 18,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3949.275f, -2134.71436f, 19.4717731f),
                                            Heading = 181.2966f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3949.89282f, -2134.52637f, 19.4547844f),
                                            Heading = 186.6648f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(KkangpaeTattoo);
        BlankLocation KkangpaeMrFuk = new BlankLocation()
        {
            Name = "KkangpaeMrFuk",
            Description = "",
            EntrancePosition = new Vector3(3939.05786f, -2179.68921f, 19.0458927f),
            EntranceHeading = 0f,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
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
                                            Location = new Vector3(3944.14478f, -2178.97925f, 19.7664242f),
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
                                            Location = new Vector3(3944.18481f, -2177.92529f, 19.767374f),
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
                                    Location = new Vector3(3939.05786f, -2179.68921f, 19.0458927f),
                                    Heading = 359.7769f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(KkangpaeMrFuk);
        BlankLocation KkangpaeKakagawa = new BlankLocation()
        {
            Name = "KkangpaeKakagawa",
            Description = "",
            EntrancePosition = new Vector3(3794.65381f, -2259.478f, 23.2207832f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 10,
                            MaxHourSpawn = 22,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3794.65381f, -2259.478f, 23.2207832f),
                                            Heading = 271.8781f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3794.67773f, -2261.01221f, 23.2206135f),
                                            Heading = 272.9294f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(KkangpaeKakagawa);
        BlankLocation KkangpaeSpanky = new BlankLocation()
        {
            Name = "KkangpaeSpanky",
            Description = "",
            EntrancePosition = new Vector3(3808.06177f, -2357.36621f, 19.1352139f),
            EntranceHeading = 0f,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
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
                                            Location = new Vector3(3802.60986f, -2359.11035f, 19.8158741f),
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
                                            Location = new Vector3(3802.47266f, -2358.1333f, 19.8163528f),
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
                                    Location = new Vector3(3808.06177f, -2357.36621f, 19.1352139f),
                                    Heading = 178.5647f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(KkangpaeSpanky);
        BlankLocation KkangpaeKoreshSq = new BlankLocation()
        {
            Name = "KkangpaeKoreshSq",
            Description = "",
            EntrancePosition = new Vector3(3869.44f, -2344.892f, 23.5683231f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 100f,
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
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(3869.44f, -2344.892f, 23.5683231f),
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
                                            MaxWantedLevelSpawn = 4,
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(3869.4668f, -2345.809f, 23.5683041f),
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
                                            MaxWantedLevelSpawn = 4,
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
            EntrancePosition = new Vector3(4033.441f, -2425.33618f, 19.5583038f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 12,
                            MaxHourSpawn = 20,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4033.441f, -2425.33618f, 19.5583038f),
                                            Heading = 90.53297f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4033.45752f, -2426.20313f, 19.5583038f),
                                            Heading = 91.35294f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(KkangpaeOffices);
        BlankLocation KkangpaeAlleySteps = new BlankLocation()
        {
            Name = "KkangpaeAlleySteps",
            Description = "",
            EntrancePosition = new Vector3(3739.75684f, -2376.23218f, 22.9851437f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 10,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3739.75684f, -2376.23218f, 22.9851437f),
                                            Heading = 2.181629f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3738.94287f, -2376.26025f, 22.985323f),
                                            Heading = 357.224f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(KkangpaeAlleySteps);
        BlankLocation KkangpaeErotica = new BlankLocation()
        {
            Name = "KkangpaeErotica",
            Description = "",
            EntrancePosition = new Vector3(3704.60376f, -2132.27f, 22.4229527f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3700.19385f, -2133.46533f, 23.0096245f),
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
                                            Location = new Vector3(3700.13867f, -2132.17627f, 23.0092239f),
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
                                    Location = new Vector3(3704.60376f, -2132.27f, 22.4229527f),
                                    Heading = 79.68229f,
                                    IsEmpty = true,
                                },
                            }
                    }
            },

        };
        BlankLocationPlaces.Add(KkangpaeErotica);
        BlankLocation KkangpaeGarages2 = new BlankLocation()
        {
            Name = "KkangpaeGarages2",
            Description = "",
            EntrancePosition = new Vector3(3887.25586f, -2107.64014f, 17.8753338f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3890.42f, -2104.11719f, 18.8047543f),
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
                                            Location = new Vector3(3889.15381f, -2104.13428f, 18.708334f),
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
                                            Location = new Vector3(3890.32178f, -2105.083f, 18.7988338f),
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
                                    Location = new Vector3(3887.25586f, -2107.64014f, 17.8753338f),
                                    Heading = 2.616594f,
                                    IsEmpty = true,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(KkangpaeGarages2);
        BlankLocation KkangpaeLaundry = new BlankLocation()
        {
            Name = "KkangpaeLaundry",
            Description = "",
            EntrancePosition = new Vector3(4144.04248f, -2131.519f, 13.6032524f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4144.04248f, -2131.519f, 13.6032524f),
                                            Heading = 181.4285f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4144.969f, -2131.541f, 13.6032524f),
                                            Heading = 186.241f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(KkangpaeLaundry);
        BlankLocation KkangpaeGozushi = new BlankLocation()
        {
            Name = "KkangpaeGozushi",
            Description = "",
            EntrancePosition = new Vector3(3923.023f, -1944.86523f, 21.172924f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
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
                                            Location = new Vector3(3936.38867f, -1940.47424f, 21.7758541f),
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
                                            Location = new Vector3(3935.63379f, -1940.46619f, 21.7756634f),
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
                                    Location = new Vector3(3923.023f, -1944.86523f, 21.172924f),
                                    Heading = 178.0261f,
                                    IsEmpty = true,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(KkangpaeGozushi);
        BlankLocation KkangpaeSqaure = new BlankLocation()
        {
            Name = "KkangpaeSqaure",
            Description = "",
            EntrancePosition = new Vector3(3725.34766f, -1893.39917f, 20.5725842f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 75f,
            ActivateCells = 3,
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 10,
                            MaxHourSpawn = 20,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3725.34766f, -1893.39917f, 20.5725842f),
                                            Heading = 91.46455f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3725.32275f, -1892.54224f, 20.5725842f),
                                            Heading = 89.68976f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(KkangpaeSqaure);
        BlankLocation KkangpaeBighorn = new BlankLocation()
        {
            Name = "KkangpaeBighorn",
            Description = "",
            EntrancePosition = new Vector3(3847.24072f, -1582.6311f, 26.9756432f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
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
                                            Location = new Vector3(3841.66162f, -1583.85913f, 27.7485542f),
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
                                            Location = new Vector3(3841.60083f, -1584.90723f, 27.749464f),
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
                                    Location = new Vector3(3847.24072f, -1582.6311f, 26.9756432f),
                                    Heading = 177.6403f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(KkangpaeBighorn);
        BlankLocation KkangpaeBite = new BlankLocation()
        {
            Name = "KkangpaeBite",
            Description = "",
            EntrancePosition = new Vector3(4195.087f, -1567.59131f, 19.780323f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 12,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4198.619f, -1553.21313f, 20.5123329f),
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
                                            Location = new Vector3(4198.65674f, -1552.48413f, 20.5123329f),
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
                                    Location = new Vector3(4195.087f, -1567.59131f, 19.780323f),
                                    Heading = 272.3572f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(KkangpaeBite);
        BlankLocation KkangpaeCuisine = new BlankLocation()
        {
            Name = "KkangpaeCuisine",
            Description = "",
            EntrancePosition = new Vector3(4165.75537f, -1987.54114f, 26.4641228f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 10,
                            MaxHourSpawn = 22,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4165.75537f, -1987.54114f, 26.4641228f),
                                            Heading = 182.5372f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(4166.928f, -1987.51917f, 26.465704f),
                                            Heading = 178.1389f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(KkangpaeCuisine);
        BlankLocation KkangpaeCorner = new BlankLocation()
        {
            Name = "KkangpaeCorner",
            Description = "",
            EntrancePosition = new Vector3(3733.01367f, -2452.68018f, 19.1697826f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3730.525f, -2452.32227f, 19.8358135f),
                                            Heading = 283.4012f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3730.52686f, -2453.39917f, 19.8358135f),
                                            Heading = 274.675f,
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
                                        Location = new Vector3(3733.01367f, -2452.68018f, 19.1697826f),
                                        Heading = 359.6692f,
                                },
                            }
                    }
                },
        };
        BlankLocationPlaces.Add(KkangpaeCorner);
        BlankLocation KkangpaeGarages = new BlankLocation()
        {
            Name = "KkangpaeGarages",
            Description = "",
            EntrancePosition = new Vector3(3575.68774f, -2451.75732f, 29.3371143f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(3580.33276f, -2448.09424f, 29.7334232f),
                                            Heading = 89.14315f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(3578.253f, -2447.24414f, 29.7334328f),
                                            Heading = 253.3317f,
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
                                    Location = new Vector3(3575.68774f, -2451.75732f, 29.3371143f),
                                    Heading = 286.43f,
                                },
                            }
                    }
                },
        };
        BlankLocationPlaces.Add(KkangpaeGarages);
        BlankLocation KkangpaeAlley = new BlankLocation()
        {
            Name = "KkangpaeAlley",
            Description = "",
            EntrancePosition = new Vector3(3568.62183f, -2536.59326f, 27.1361637f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3569.95361f, -2541.74219f, 27.4042435f),
                                            Heading = 13.09881f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3571.55469f, -2541.19922f, 27.3564529f),
                                            Heading = 42.62768f,
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
                                        Location = new Vector3(3568.62183f, -2536.59326f, 27.1361637f),
                                        Heading = 307.5577f,
                                },
                            }
                    }
                },
        };
        BlankLocationPlaces.Add(KkangpaeAlley);
        BlankLocation KkangpaeAlley2 = new BlankLocation()
        {
            Name = "KkangpaeAlley2",
            Description = "",
            EntrancePosition = new Vector3(3715.24878f, -2559.78613f, 19.1964436f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.AlderneyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_KKANGPAE",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3720.526f, -2563.98535f, 19.5890427f),
                                            Heading = 150.4714f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(3719.6167f, -2565.7793f, 19.5898037f),
                                            Heading = 333.2072f,
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
                                    Location = new Vector3(3715.24878f, -2559.78613f, 19.1964436f),
                                    Heading = 141.4918f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(KkangpaeAlley2);
    }
    private void TriadGang()
    {
        BlankLocation TriadStreet = new BlankLocation()
        {
            Name = "TriadStreet",
            Description = "",
            EntrancePosition = new Vector3(5197.69f, -3618.168f, 15.2012129f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 20,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5197.69f, -3618.168f, 15.2012129f),
                                            Heading = 29.75146f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5196.883f, -3618.67847f, 15.2012129f),
                                            Heading = 31.13165f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_UPRIGHT_CLUBHOUSE",
                                            },

                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(TriadStreet);
        BlankLocation TriadStreet2 = new BlankLocation()
        {
            Name = "TriadStreet2",
            Description = "",
            EntrancePosition = new Vector3(5206.90527f, -3694.49756f, 14.759903f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5206.90527f, -3694.49756f, 14.759903f),
                                            Heading = 87.65568f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5206.94629f, -3695.269f, 14.7590523f),
                                            Heading = 91.00845f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_UPRIGHT_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(TriadStreet2);
        BlankLocation TriadStreet3 = new BlankLocation()
        {
            Name = "TriadStreet3",
            Description = "",
            EntrancePosition = new Vector3(5270.776f, -3737.42773f, 14.7714329f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 20,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5270.776f, -3737.42773f, 14.7714329f),
                                            Heading = 175.3779f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5270.05957f, -3737.43335f, 14.7714329f),
                                            Heading = 182.1518f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_UPRIGHT_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(TriadStreet3);
        BlankLocation TriadStreet4 = new BlankLocation()
        {
            Name = "TriadStreet4",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(5263.40234f, -3705.26563f, 14.7622623f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5263.40234f, -3705.26563f, 14.7622623f),
                                            Heading = 108.6177f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5262.74072f, -3703.86084f, 14.7594128f),
                                            Heading = 132.4179f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5259.25537f, -3707.286f, 14.7657127f),
                                            Heading = 294.6251f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_IMPATIENT",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5256.574f, -3705.95752f, 14.7648525f),
                                            Heading = 264.2014f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                            },
                                    }
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5249.17773f, -3698.485f, 14.070653f),
                                            Heading = 358.6606f,
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(TriadStreet4);
        BlankLocation TriadStreet5 = new BlankLocation()
        {
            Name = "TriadStreet5",
            Description = "",
            EntrancePosition = new Vector3(5184.01074f, -3625.29761f, 14.018343f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5180.15137f, -3618.04858f, 15.5133629f),
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
                                            Location = new Vector3(5180.068f, -3618.92236f, 15.5133629f),
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
                                    Location = new Vector3(5184.01074f, -3625.29761f, 14.018343f),
                                    Heading = 181.2395f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(TriadStreet5);
        BlankLocation TriadStreet6 = new BlankLocation()
        {
            Name = "TriadStreet6",
            Description = "",
            EntrancePosition = new Vector3(5193.68359f, -3604.106f, 14.7637329f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 22,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5193.68359f, -3604.106f, 14.7637329f),
                                            Heading = 155.0832f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5192.93164f, -3603.428f, 14.7633724f),
                                            Heading = 159.182f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_IMPATIENT",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(TriadStreet6);
        BlankLocation TriadStreet7 = new BlankLocation()
        {
            Name = "TriadStreet7",
            Description = "",
            EntrancePosition = new Vector3(5197.246f, -3697.752f, 14.7521629f),
            EntranceHeading = 0f,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
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
                                            Location = new Vector3(5197.246f, -3697.752f, 14.7521629f),
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
                                            Location = new Vector3(5197.16455f, -3699.16553f, 14.7544327f),
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
                                    Location = new Vector3(5193.07f, -3703.18042f, 14.0838823f),
                                    Heading = 269.894f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(TriadStreet7);
        BlankLocation TriadStreet8 = new BlankLocation()
        {
            Name = "TriadStreet8",
            Description = "",
            EntrancePosition = new Vector3(5204.13f, -3636.178f, 14.7623425f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5204.13f, -3636.178f, 14.7623425f),
                                            Heading = 354.4641f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5203.893f, -3634.63013f, 14.7640123f),
                                            Heading = 186.1902f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_IMPATIENT",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(TriadStreet8);
        BlankLocation TriadStreet9 = new BlankLocation()
        {
            Name = "TriadStreet9",
            Description = "",
            EntrancePosition = new Vector3(5187.095f, -3678.444f, 14.7617626f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 18,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5187.095f, -3678.444f, 14.7617626f),
                                            Heading = 357.6227f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5188.44971f, -3678.52148f, 14.7623329f),
                                            Heading = 358.8286f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(TriadStreet9);
        BlankLocation TriadParking = new BlankLocation()
        {
            Name = "TriadParking",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(5283.46338f, -3601.36377f, 13.9370022f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5284.30127f, -3603.469f, 14.6116428f),
                                            Heading = 177.0485f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5284.43457f, -3606.162f, 14.7295628f),
                                            Heading = 3.626181f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5283.5625f, -3606.25684f, 14.7212029f),
                                            Heading = 359.9373f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_IMPATIENT",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5281.60254f, -3607.14771f, 17.3605728f),
                                            Heading = 355.1356f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER",
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5283.46338f, -3601.36377f, 13.9370022f),
                                            Heading = 115.5975f,
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(TriadParking);
        BlankLocation TriadRestaurant = new BlankLocation()
        {
            Name = "TriadRestaurant",
            Description = "",
            EntrancePosition = new Vector3(5356.145f, -3573.3064f, 12.7963228f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5357.4043f, -3579.18066f, 13.9088726f),
                                            Heading = 356.5626f,
                                            MinHourSpawn = 8,
                                            MaxHourSpawn = 4,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5356.515f, -3579.254f, 13.9088726f),
                                            Heading = 4.507515f,
                                            MinHourSpawn = 8,
                                            MaxHourSpawn = 4,
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
                                    Location = new Vector3(5356.145f, -3573.3064f, 12.7963228f),
                                    Heading = 267.5997f,
                                    IsEmpty = true,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(TriadRestaurant);
        BlankLocation TriadCorner = new BlankLocation()
        {
            Name = "TriadCorner",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(5369.509f, -3704.73779f, 10.0281725f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5397.735f, -3707.449f, 11.2559223f),
                                            Heading = 91.74756f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5396.906f, -3704.65259f, 11.2559223f),
                                            Heading = 96.38853f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5395.26855f, -3704.83472f, 11.2559223f),
                                            Heading = 274.5788f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5377.77539f, -3711.427f, 11.2559223f),
                                            Heading = 45.66113f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_UPRIGHT_CLUBHOUSE",
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5383.35449f, -3705.26563f, 11.2559128f),
                                            Heading = 26.24784f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5384.732f, -3704.641f, 11.2559128f),
                                            Heading = 37.15827f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_UPRIGHT_CLUBHOUSE",
                                                "WORLD_HUMAN_DRUG_DEALER",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5369.509f, -3704.73779f, 10.0281725f),
                                            Heading = 302.4837f,
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(TriadCorner);
        BlankLocation TriadFDLCTower = new BlankLocation()
        {
            Name = "TriadFDLCTower",
            Description = "",
            EntrancePosition = new Vector3(5428.035f, -3635.993f, 9.070204f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 22,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5428.035f, -3635.993f, 9.070204f),
                                            Heading = 138.3945f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_IMPATIENT_UPRIGHT_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5427.01367f, -3635.094f, 9.070727f),
                                            Heading = 139.2963f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(TriadFDLCTower);
        BlankLocation TriadStorage = new BlankLocation()
        {
            Name = "TriadStorage",
            Description = "",
            EntrancePosition = new Vector3(5443.63232f, -3530.09229f, 4.193806f),
            EntranceHeading = 0f,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(5438.8667f, -3521.16455f, 4.953754f),
                                            Heading = 271.9642f,
                                            MinHourSpawn = 18,
                                            MaxHourSpawn = 4,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(5438.876f, -3519.991f, 4.953802f),
                                            Heading = 271.7615f,
                                            MinHourSpawn = 18,
                                            MaxHourSpawn = 4,
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
                                    Location = new Vector3(5443.63232f, -3530.09229f, 4.193806f),
                                    Heading = 359.9367f,
                                    IsEmpty = true,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(TriadStorage);
        BlankLocation TriadSkatePark = new BlankLocation()
        {
            Name = "TriadSkatePark",
            Description = "",
            EntrancePosition = new Vector3(5471.55566f, -3673.353f, 5.009169f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5471.55566f, -3673.353f, 5.009169f),
                                            Heading = 273.7695f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_IMPATIENT_UPRIGHT_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5471.427f, -3674.52441f, 5.009168f),
                                            Heading = 267.8065f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(TriadSkatePark);
        BlankLocation TriadPier45 = new BlankLocation()
        {
            Name = "TriadPier45",
            Description = "",
            EntrancePosition = new Vector3(5551.32227f, -3730.71484f, 5.819651f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5551.32227f, -3730.71484f, 5.819651f),
                                            Heading = 88.66313f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_IMPATIENT_UPRIGHT_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5551.34668f, -3729.51685f, 5.819651f),
                                            Heading = 90.56909f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(TriadPier45);
        BlankLocation TriadPier45b = new BlankLocation()
        {
            Name = "TriadPier45b",
            Description = "",
            EntrancePosition = new Vector3(5643.383f, -3793.029f, 10.8805628f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5643.383f, -3793.029f, 10.8805628f),
                                            Heading = 271.1587f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_IMPATIENT_UPRIGHT_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5643.428f, -3791.8877f, 10.8805628f),
                                            Heading = 228.5475f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(TriadPier45b);
        BlankLocation TriadPier45c = new BlankLocation()
        {
            Name = "TriadPier45c",
            Description = "",
            EntrancePosition = new Vector3(5665.26855f, -3745.12549f, 4.692849f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5665.26855f, -3745.12549f, 4.692849f),
                                            Heading = 91.77491f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_DRUG_DEALER",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5665.289f, -3744.26416f, 4.69284725f),
                                            Heading = 95.26043f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(TriadPier45c);
        BlankLocation TriadExchange = new BlankLocation()
        {
            Name = "TriadExchange",
            Description = "",
            EntrancePosition = new Vector3(5375.84033f, -3919.5376f, 4.218406f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 75f,
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5363.5166f, -3907.056f, 8.114711f),
                                            Heading = 180.4981f,
                                            MinHourSpawn = 8,
                                            MaxHourSpawn = 4,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5362.62842f, -3907.123f, 8.114986f),
                                            Heading = 181.8094f,
                                            MinHourSpawn = 8,
                                            MaxHourSpawn = 4,
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
                                    Location = new Vector3(5375.84033f, -3919.5376f, 4.218406f),
                                    Heading = 104.4474f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(TriadExchange);
        BlankLocation TriadGLSOfficeF = new BlankLocation()
        {
            Name = "TriadGLSOfficeF",
            Description = "",
            EntrancePosition = new Vector3(5263.788f, -3971.41772f, 4.954299f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5263.788f, -3971.41772f, 4.954299f),
                                            Heading = 210.5127f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_DRUG_DEALER",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5264.761f, -3970.56274f, 4.954123f),
                                            Heading = 206.3595f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(TriadGLSOfficeF);
        BlankLocation TriadBOL = new BlankLocation()
        {
            Name = "TriadBOL",
            Description = "",
            EntrancePosition = new Vector3(5210.095f, -3891.52051f, 14.7677822f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 20,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5210.095f, -3891.52051f, 14.7677822f),
                                            Heading = 179.5031f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_DRUG_DEALER",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(5208.80371f, -3891.41748f, 14.7677822f),
                                            Heading = 177.9857f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(TriadBOL);
        BlankLocation TriadAlleyCP = new BlankLocation()
        {
            Name = "TriadAlleyCP",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(5203.12646f, -3972.39624f, 9.402303f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5203.12646f, -3972.39624f, 9.402303f),
                                            Heading = 4.410272f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5200.927f, -3968.38745f, 9.402303f),
                                            Heading = 37.85015f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5199.38135f, -3966.09155f, 9.402303f),
                                            Heading = 218.8375f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5198.633f, -3966.697f, 9.402303f),
                                            Heading = 213.9943f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_UPRIGHT_CLUBHOUSE",
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5195.69629f, -3947.72217f, 8.834116f),
                                            Heading = 0.1249077f,
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5197.792f, -3964.4126f, 8.838069f),
                                            Heading = 310.3144f,
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(TriadAlleyCP);
        BlankLocation TriadStreet10 = new BlankLocation()
        {
            Name = "TriadStreet10",
            Description = "",
            EntrancePosition = new Vector3(5157.226f, -3975.52832f, 9.185032f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5157.226f, -3975.52832f, 9.185032f),
                                            Heading = 92.56901f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_DRUG_DEALER",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(5157.31152f, -3976.54932f, 9.095883f),
                                            Heading = 86.35593f,
                                            Percentage = 0f,
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
                                            MinHourSpawn = 8,
                                            MaxHourSpawn = 4,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 4,
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(TriadStreet10);
        BlankLocation TriadSumYungGai = new BlankLocation()
        {
            Name = "TriadSumYungGai",
            Description = "",
            EntrancePosition = new Vector3(6439.278f, -2874.98755f, 21.4265842f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6436.275f, -2880.135f, 22.0782528f),
                                            Heading = 1.467323f,
                                            MinHourSpawn = 8,
                                            MaxHourSpawn = 2,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6437.465f, -2880.23486f, 22.0888443f),
                                            Heading = 358.8883f,
                                            MinHourSpawn = 8,
                                            MaxHourSpawn = 2,
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
                                        Location = new Vector3(6439.278f, -2874.98755f, 21.4265842f),
                                        Heading = 270.6619f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(TriadSumYungGai);
        BlankLocation TriadZhouMing = new BlankLocation()
        {
            Name = "TriadZhouMing",
            Description = "",
            EntrancePosition = new Vector3(6414.32275f, -2554.63916f, 36.66255f),
            EntranceHeading = 0f,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6416.455f, -2561.96924f, 37.47961f),
                                            Heading = 49.94909f,
                                            MinHourSpawn = 18,
                                            MaxHourSpawn = 4,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6417.53369f, -2559.579f, 37.41634f),
                                            Heading = 71.81964f,
                                            MinHourSpawn = 18,
                                            MaxHourSpawn = 4,
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
                                        Location = new Vector3(6414.32275f, -2554.63916f, 36.66255f),
                                        Heading = 93.21703f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(TriadZhouMing);
        BlankLocation TriadRsHaul = new BlankLocation()
        {
            Name = "TriadRsHaul",
            Description = "",
            EntrancePosition = new Vector3(5533.0625f, -3505.04175f, 3.84958315f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5540.33838f, -3506.859f, 4.461677f),
                                            Heading = 79.7878f,
                                            MinHourSpawn = 8,
                                            MaxHourSpawn = 2,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5540.56934f, -3505.51587f, 4.466565f),
                                            Heading = 82.79868f,
                                            MinHourSpawn = 8,
                                            MaxHourSpawn = 2,
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
                                        Location = new Vector3(5533.0625f, -3505.04175f, 3.84958315f),
                                        Heading = 271.5189f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(TriadRsHaul);
        BlankLocation TriadKennys = new BlankLocation()
        {
            Name = "TriadKennys",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(6095.903f, -2671.85913f, 21.6536942f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6072.049f, -2692.936f, 22.2225933f),
                                            Heading = 180.3808f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6070.418f, -2692.999f, 22.2230434f),
                                            Heading = 227.0985f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6089.34961f, -2663.57324f, 22.2459927f),
                                            Heading = 84.82274f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6087.717f, -2663.47925f, 22.2430134f),
                                            Heading = 266.2775f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_UPRIGHT_CLUBHOUSE",
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6095.903f, -2671.85913f, 21.6536942f),
                                            Heading = 329.3496f,
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6076.3877f, -2698.249f, 21.548233f),
                                            Heading = 90.52228f,
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(TriadKennys);
        BlankLocation TriadMechMechanic = new BlankLocation()
        {
            Name = "TriadMechMechanic",
            Description = "",
            EntrancePosition = new Vector3(6242.317f, -2709.037f, 24.628273f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_WEICHENG",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 20,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6246.192f, -2715.75415f, 25.8186626f),
                                            Heading = 0.8625223f,
                                            MinHourSpawn = 8,
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
                                            Location = new Vector3(6247.60352f, -2715.68921f, 25.9110737f),
                                            Heading = 8.233461f,
                                            MinHourSpawn = 8,
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
                                        Location = new Vector3(6242.317f, -2709.037f, 24.628273f),
                                        Heading = 269.6652f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(TriadMechMechanic);
    }

    // -------  Street Squads  --------
    private void HolHustGang()
    {
        BlankLocation HustlersSprunk = new BlankLocation()
        {
            Name = "HustlersSprunk",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(5883.83252f, -1792.74121f, 14.8475027f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            InteriorID = 172034,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_HOLHUST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 20,
                            MaxHourSpawn = 24,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5883.83252f, -1792.74121f, 14.8475027f),
                                            Heading = 337.5721f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5902.841f, -1798.96313f, 14.8475027f),
                                            Heading = 21.77995f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5901.81738f, -1796.49219f, 14.8475027f),
                                            Heading = 191.2004f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRINKING_FACILITY",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5900.34229f, -1797.23718f, 14.8475027f),
                                            Heading = 232.3982f,
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5892.928f, -1787.51721f, 16.2484436f),
                                            Heading = 183.3846f,
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5875.659f, -1804.33118f, 14.5057325f),
                                            Heading = 0.8748494f,
                                    },
                            },
                    },
            },
        }; //interior
        BlankLocationPlaces.Add(HustlersSprunk);
        BlankLocation HustlersBlock = new BlankLocation()
        {
            Name = "HustlersBlock",
            Description = "",
            EntrancePosition = new Vector3(4675.71143f, -1780.04016f, 18.1611538f),
            EntranceHeading = 0f,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_HOLHUST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4677.007f, -1789.33118f, 18.8615932f),
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
                                            Location = new Vector3(4676.024f, -1789.31714f, 18.8615932f),
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
                                        Location = new Vector3(4675.71143f, -1780.04016f, 18.1611538f),
                                        Heading = 180.378f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(HustlersBlock);
        BlankLocation HustlersBlock2 = new BlankLocation()
        {
            Name = "HustlersBlock2",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(4698.187f, -1735.73315f, 18.1421032f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_HOLHUST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4689.21973f, -1725.59814f, 18.8618031f),
                                            Heading = 249.6301f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4691.05273f, -1725.09521f, 18.8538132f),
                                            Heading = 101.2989f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4697.659f, -1727.08813f, 18.8009644f),
                                            Heading = 158.8806f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4697.13f, -1728.59314f, 18.7998829f),
                                            Heading = 341.1935f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4698.187f, -1735.73315f, 18.1421032f),
                                            Heading = 160.3956f,
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4703.54f, -1728.73816f, 18.1538734f),
                                            Heading = 47.43872f,
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(HustlersBlock2);
        BlankLocation HustlersBlock3 = new BlankLocation()
        {
            Name = "HustlersBlock3",
            Description = "",
            EntrancePosition = new Vector3(5105.244f, -1717.74219f, 17.9603443f),
            EntranceHeading = 0f,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_HOLHUST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5108.4917f, -1739.27014f, 18.8304844f),
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
                                            Location = new Vector3(5109.167f, -1740.39417f, 18.8304844f),
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
                                        Location = new Vector3(5105.244f, -1717.74219f, 17.9603443f),
                                        Heading = 266.9731f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(HustlersBlock3);
        BlankLocation HustlersBlock4 = new BlankLocation()
        {
            Name = "HustlersBlock4",
            Description = "",
            EntrancePosition = new Vector3(5064.607f, -1761.54016f, 18.4338226f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_HOLHUST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5064.607f, -1761.54016f, 18.4338226f),
                                            Heading = 146.2155f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5063.584f, -1762.83813f, 18.438303f),
                                            Heading = 308.1097f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(HustlersBlock4);
        BlankLocation HustlersBBCourts = new BlankLocation()
        {

            Name = "HustlersBBCourts",
            Description = "",
            EntrancePosition = new Vector3(4977.617f, -1802.4762f, 20.4493427f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_HOLHUST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 22,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4977.617f, -1802.4762f, 20.4493427f),
                                            Heading = 89.55726f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4977.70264f, -1801.47717f, 20.4493427f),
                                            Heading = 91.50203f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(HustlersBBCourts);
        BlankLocation HustlersTwat = new BlankLocation()
        {
            Name = "HustlersTwat",
            Description = "",
            EntrancePosition = new Vector3(4848.02f, -1859.70117f, 12.7367926f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_HOLHUST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4848.02f, -1859.70117f, 12.7367926f),
                                            Heading = 90.22813f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4847.963f, -1860.74316f, 12.8022728f),
                                            Heading = 95.24944f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(HustlersTwat);
        BlankLocation HustlersBlock5 = new BlankLocation()
        {
            Name = "HustlersBlock5",
            Description = "",
            EntrancePosition = new Vector3(4707.116f, -2001.35315f, 17.4787827f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_HOLHUST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(4707.116f, -2001.35315f, 17.4787827f),
                                            Heading = 270.7826f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(4707.059f, -2000.69116f, 17.4781628f),
                                            Heading = 270.6313f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },

                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(HustlersBlock5);
        BlankLocation HustlersPark = new BlankLocation()
        {
            Name = "HustlersPark",
            Description = "",
            EntrancePosition = new Vector3(4943.44336f, -2541.71021f, 3.42386818f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_HOLHUST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4943.44336f, -2541.71021f, 3.42386818f),
                                            Heading = 272.9309f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4943.247f, -2540.65332f, 3.421695f),
                                            Heading = 273.3445f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(HustlersPark);
        BlankLocation HustlersBankCorner = new BlankLocation()
        {
            Name = "HustlersBankCorner",
            Description = "",
            EntrancePosition = new Vector3(4804.163f, -2066.60815f, 14.7600021f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 150f,
            AssignedAssociationID = "AMBIENT_GANG_HOLHUST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4804.163f, -2066.60815f, 14.7600021f),
                                            Heading = 198.4298f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4802.779f, -2067.09131f, 14.7594824f),
                                            Heading = 237.7976f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
                },
        };
        BlankLocationPlaces.Add(HustlersBankCorner);
        BlankLocation HustlersBurger = new BlankLocation()
        {
            Name = "HustlersBurger",
            Description = "",
            EntrancePosition = new Vector3(4746.17871f, -2066.57715f, 12.8842831f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_HOLHUST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 22,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4746.17871f, -2066.57715f, 12.8842831f),
                                            Heading = 94.22587f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4746.159f, -2065.85522f, 12.8852024f),
                                            Heading = 94.18714f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(HustlersBurger);
        BlankLocation HustlersStyle = new BlankLocation()
        {
            Name = "HustlersStyle",
            Description = "",
            EntrancePosition = new Vector3(4722.80664f, -1860.35913f, 15.4715424f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_HOLHUST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 24,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4722.80664f, -1860.35913f, -1860.35913f),
                                            Heading = 274.434f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4722.80566f, -1859.29016f, 15.4715424f),
                                            Heading = 272.0949f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(HustlersStyle);
        BlankLocation HustlersBlock6 = new BlankLocation()
        {
            Name = "HustlersBlock6",
            Description = "",
            EntrancePosition = new Vector3(4819.28564f, -1642.86523f, 20.4229527f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_HOLHUST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4819.28564f, -1642.86523f, 20.4229527f),
                                            Heading = 210.2232f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4820.067f, -1642.33325f, 20.4239826f),
                                            Heading = 212.858f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },

                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(HustlersBlock6);
        BlankLocation HustlersUnderBridge = new BlankLocation()
        {
            Name = "HustlersUnderBridge",
            Description = "",
            EntrancePosition = new Vector3(5376.729f, -2234.60522f, 8.448587f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_HOLHUST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(5376.729f, -2234.60522f, 8.448587f),
                                            Heading = 270.9568f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(5376.67871f, -2235.96216f, 8.448587f),
                                            Heading = 266.329f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_POT",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                            },

                    },
            },
        };
        BlankLocationPlaces.Add(HustlersUnderBridge);
        BlankLocation HustlersPark2 = new BlankLocation()
        {

            Name = "HustlersPark2",
            Description = "",
            EntrancePosition = new Vector3(4888.30469f, -2147.26123f, 12.3112431f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_HOLHUST",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4888.30469f, -2147.26123f, 12.3112431f),
                                            Heading = 288.2362f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(4888.492f, -2148.456f, 12.3717928f),
                                            Heading = 295.6778f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(HustlersPark2);
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
            EntrancePosition = new Vector3(5761.621f, -1880.42322f, 10.9795027f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            MenuID = "",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                new ConditionalGroup()
                {
                    Name = "",
                    Percentage = defaultSpawnPercentage,
                    MinHourSpawn = 8,
                    MaxHourSpawn = 4,
                    PossiblePedSpawns = new List<ConditionalLocation>()
                    {
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(5761.621f, -1880.42322f, 10.9795027f),
                            Heading = 262.1925f,
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>()
                            {
                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                            },
                        },
                        new GangConditionalLocation()
                        {
                            Location = new Vector3(5761.6123f, -1879.28516f, 10.9626827f),
                            Heading = 258.0417f,
                            TaskRequirements = TaskRequirements.Guard,
                            ForcedScenarios = new List<String>()
                            {
                                "WORLD_HUMAN_HANG_OUT_STREET",
                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                            },
                        },
                    },
                },
            },
        };
        BlankLocationPlaces.Add(SpanLordsMem);
        BlankLocation SpanishLordsBasketball = new BlankLocation()
        {
            Name = "SpanishLordsBasketball",
            Description = "",
            EntrancePosition = new Vector3(5507.93848f, -1715.90417f, 14.5393028f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5507.93848f, -1715.90417f, 14.5393028f),
                                            Heading = 221.0592f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_GUARD_STAND_CASINO",
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5507.37061f, -1716.91113f, 14.5393028f),
                                            Heading = 300.942f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(SpanishLordsBasketball);
        BlankLocation SpanishLordsBlocks = new BlankLocation()
        {
            Name = "SpanishLordsBlocks",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(5567.078f, -1726.36816f, 16.9479427f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            { 
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 10,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5567.078f, -1726.36816f, 16.9479427f),
                                            Heading = 342.796f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5550.36133f, -1748.40015f, 16.9647331f),
                                            Heading = 249.5234f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5551.99268f, -1749.28223f, 16.9652233f),
                                            Heading = 57.3554f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5550.94434f, -1750.20618f, 16.9642544f),
                                            Heading = 16.9324f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_POT",
                                                "WORLD_HUMAN_DRUG_DEALER",
                                                "WORLD_HUMAN_DRINKING_FACILITY",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5565.57764f, -1718.31323f, 15.8474922f),
                                            Heading = 144.0499f,
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5561.79443f, -1716.08118f, 16.1015034f),
                                            Heading = 141.1463f,
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
            EntrancePosition = new Vector3(5625.95947f, -1804.19019f, 9.84877f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5625.95947f, -1804.19019f, 9.84877f),
                                            Heading = 306.0933f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5625.64355f, -1803.36816f, 9.884068f),
                                            Heading = 269.9267f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(SpanishLordsBlocks2);
        BlankLocation SpanishLordsSwitchSt = new BlankLocation()
        {
            Name = "SpanishLordsSwitchSt",
            Description = "",
            EntrancePosition = new Vector3(5748.26855f, -1704.34814f, 18.1792145f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5748.26855f, -1704.34814f, 18.1792145f),
                                            Heading = 205.9291f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5749.13037f, -1703.84924f, 18.1792145f),
                                            Heading = 185.5197f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(SpanishLordsSwitchSt);
        BlankLocation SpanishLordsFolsom = new BlankLocation()
        {
            Name = "SpanishLordsFolsom",
            Description = "",
            EntrancePosition = new Vector3(5656.25049f, -1590.65918f, 16.9147339f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5653.578f, -1598.52319f, 17.226284f),
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
                                            Location = new Vector3(5655.137f, -1598.58618f, 17.3918934f),
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
                                        Location = new Vector3(5656.25049f, -1590.65918f, 16.9147339f),
                                        Heading = 77.53199f,
                                },
                            },
                    }
            },
        };
        BlankLocationPlaces.Add(SpanishLordsFolsom);
        BlankLocation SpanishLordsValdez = new BlankLocation()
        {
            Name = "SpanishLordsValdez",
            Description = "",
            EntrancePosition = new Vector3(5974.425f, -1453.54126f, 36.70187f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5974.425f, -1453.54126f, 36.70187f),
                                            Heading = 262.1475f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5974.051f, -1454.2312f, 36.68005f),
                                            Heading = 239.2251f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(SpanishLordsValdez);
        BlankLocation SpanishLordsCorner = new BlankLocation()
        {
            Name = "SpanishLordsCorner",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(5947.766f, -1776.8822f, 13.5272026f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5944.297f, -1786.64917f, 15.8507423f),
                                            Heading = 92.3622f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5940.9917f, -1784.98315f, 14.2481327f),
                                            Heading = 90.06734f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5940.98047f, -1785.90222f, 14.2481327f),
                                            Heading = 71.95461f,
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
                                            Location = new Vector3(5947.766f, -1776.8822f, 13.5272026f),
                                            Heading = 271.2206f,
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5966.612f, -1782.77917f, 13.5238123f),
                                            Heading = 3.723292f,
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
            EntrancePosition = new Vector3(5687.663f, -1387.78931f, 26.1680641f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 12,
                            MaxHourSpawn = 20,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5687.663f, -1387.78931f, 26.1680641f),
                                            Heading = 8.662255f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5688.34766f, -1387.69824f, 26.1847744f),
                                            Heading = 8.363561f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(SpanishLordsPark);
        BlankLocation SpanishLordsLotus = new BlankLocation()
        {
            Name = "SpanishLordsLotus",
            Description = "",
            EntrancePosition = new Vector3(5823.93945f, -1658.19116f, 24.6028328f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5823.93945f, -1658.19116f, 24.6028328f),
                                            Heading = 178.2518f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5822.62842f, -1658.3291f, 24.6028328f),
                                            Heading = 185.8716f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(SpanishLordsLotus);
        BlankLocation SpanishLordsBlocks3 = new BlankLocation()
        {
            Name = "SpanishLordsBlocks3",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(6142.544f, -1478.23608f, 20.2424145f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 10,
                            MaxHourSpawn = 23,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(6139.46875f, -1484.19922f, 20.8638134f),
                                            Heading = 219.3602f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6140.044f, -1483.46021f, 20.8638039f),
                                            Heading = 183.3793f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6140.43359f, -1485.08228f, 20.8638134f),
                                            Heading = 19.84033f,
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
                                            Location = new Vector3(6142.544f, -1478.23608f, 20.2424145f),
                                            Heading = 81.88879f,
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6151.648f, -1491.53516f, 20.2587242f),
                                            Heading = 226.1502f,
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
            EntrancePosition = new Vector3(6171.883f, -1547.94824f, 16.8448944f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6171.883f, -1547.94824f, 16.8448944f),
                                            Heading = 3.811977f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6170.317f, -1547.92212f, 16.8448944f),
                                            Heading = 358.8205f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(SpanishLordsBlocks4);
        BlankLocation SpanishLordsAutoShop = new BlankLocation()
        {
            Name = "SpanishLordsAutoShop",
            Description = "",
            MapIcon = 162,
            EntrancePosition = new Vector3(6155.857f, -1652.74609f, 16.0929031f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6163.026f, -1650.5083f, 16.9361629f),
                                            Heading = 34.94679f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_GUARD_STAND_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6163.95361f, -1649.83813f, 16.8998432f),
                                            Heading = 37.67293f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6168.39453f, -1656.43921f, 16.8567429f),
                                            Heading = 342.1092f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_IMPATIENT_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6167.22168f, -1655.64429f, 16.8557339f),
                                            Heading = 325.0153f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_CLIPBOARD_FACILITY",
                                            },
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6158.376f, -1650.233f, 16.03096f),
                                            Heading = 122.2056f,
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6169.165f, -1652.3313f, 16.2904243f),
                                            Heading = 226.278f,
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
            EntrancePosition = new Vector3(6094.319f, -1485.74707f, 16.8429031f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6094.319f, -1485.74707f, 16.8429031f),
                                            Heading = 136.5653f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6095.11475f, -1486.52515f, 16.8329639f),
                                            Heading = 138.1465f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(SpanishLordsHammer);
        BlankLocation SpanishLordsAttica = new BlankLocation()
        {
            Name = "SpanishLordsAttica",
            Description = "",
            EntrancePosition = new Vector3(5875.786f, -1652.99219f, 26.5772228f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                        Location = new Vector3(5875.786f, -1652.99219f, 26.5772228f),
                                            Heading = 106.7113f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5875.90137f, -1653.96631f, 26.5674133f),
                                            Heading = 101.6826f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(SpanishLordsAttica);
        BlankLocation SpanishLordsBlocks5 = new BlankLocation()
        {
            Name = "SpanishLordsBlocks5",
            Description = "",
            EntrancePosition = new Vector3(5624.258f, -1883.13721f, 11.6079626f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5624.258f, -1883.13721f, 11.6079626f),
                                            Heading = 319.6241f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5625.16553f, -1883.83813f, 11.6079626f),
                                            Heading = 327.9651f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(SpanishLordsBlocks5);
        BlankLocation SpanishLordsStreet = new BlankLocation()
        {
            Name = "SpanishLordsStreet",
            Description = "",
            EntrancePosition = new Vector3(5852.71875f, -1859.02917f, 13.9544725f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5852.71875f, -1859.02917f, 13.9544725f),
                                            Heading = 265.2629f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5852.526f, -1858.00122f, 13.9544725f),
                                            Heading = 274.0445f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(SpanishLordsStreet);
        BlankLocation SpanishLordsStreet2 = new BlankLocation()
        {
            Name = "SpanishLordsStreet2",
            Description = "",
            EntrancePosition = new Vector3(5800.81055f, -1790.92822f, 11.6690331f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5800.81055f, -1790.92822f, 11.6690331f),
                                            Heading = 180.0671f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5799.86035f, -1790.94214f, 11.6690722f),
                                            Heading = 181.0231f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(SpanishLordsStreet2);
        BlankLocation SpanishLordsStreet3 = new BlankLocation()
        {
            Name = "SpanishLordsStreet3",
            Description = "",
            EntrancePosition = new Vector3(5697.93164f, -1786.66614f, 10.4183521f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5697.744f, -1785.95215f, 10.431983f),
                                            Heading = 56.90393f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },

                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5696.64063f, -1785.13318f, 10.4466629f),
                                            Heading = 223.7968f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(SpanishLordsStreet3);
        BlankLocation SpanishLordsStreet4 = new BlankLocation()
        {
            Name = "SpanishLordsStreet4",
            Description = "",
            EntrancePosition = new Vector3(5694.77539f, -1667.11523f, 19.7455139f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5694.77539f, -1667.11523f, 19.7455139f),
                                            Heading = 94.118f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_STAND_MOBILE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5694.79932f, -1665.97314f, 19.8614826f),
                                            Heading = 92.55875f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(SpanishLordsStreet4);
        BlankLocation SpanishLordsStreet5 = new BlankLocation()
        {
            Name = "SpanishLordsStreet5",
            Description = "",
            EntrancePosition = new Vector3(5540.64258f, -1512.63623f, 15.968133f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5540.64258f, -1512.63623f, 15.968133f),
                                            Heading = 177.9854f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5541.637f, -1512.73413f, 15.9442225f),
                                            Heading = 116.4593f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_DRUG_DEALER_HARD",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(SpanishLordsStreet5);
        BlankLocation SpanishLordsStreet6 = new BlankLocation()
        {
            Name = "SpanishLordsStreet6",
            Description = "",
            EntrancePosition = new Vector3(5614.27637f, -1541.57129f, 16.2199039f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5614.27637f, -1541.57129f, 16.2199039f),
                                            Heading = 317.0581f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_LEANING",
                                                "WORLD_HUMAN_DRUG_DEALER",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5614.9834f, -1542.37012f, 16.2216034f),
                                            Heading = 316.6674f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                            },

                    },
            },
        };
        BlankLocationPlaces.Add(SpanishLordsStreet6);
        BlankLocation SpanishLordsStreet7 = new BlankLocation()
        {
            Name = "SpanishLordsStreet7",
            Description = "",
            EntrancePosition = new Vector3(5727.14258f, -1502.62207f, 32.6418533f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5727.14258f, -1502.62207f, 32.6418533f),
                                            Heading = 272.9043f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_DRUG_DEALER",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5727.13867f, -1503.46118f, 32.63031f),
                                            Heading = 271.0912f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(SpanishLordsStreet7);
        BlankLocation SpanishLordsStreet8 = new BlankLocation()
        {
            Name = "SpanishLordsStreet8",
            Description = "",
            EntrancePosition = new Vector3(5830.026f, -1468.82129f, 38.4858f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5830.026f, -1468.82129f, 38.4858f),
                                            Heading = 178.9518f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_DRUG_DEALER",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(5830.00635f, -1470.30322f, 38.4857826f),
                                            Heading = 356.9098f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(SpanishLordsStreet8);
        BlankLocation SpanishLordsStreet9 = new BlankLocation()
        {
            Name = "SpanishLordsStreet9",
            Description = "",
            EntrancePosition = new Vector3(5857.366f, -1523.04419f, 36.12139f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_SPANISH",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 2,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5857.366f, -1523.04419f, 36.12139f),
                                            Heading = 177.5932f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_IMPATIENT_UPRIGHT_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(5858.085f, -1523.02417f, 36.1206932f),
                                            Heading = 177.9422f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(SpanishLordsStreet9);
    }
    private void YardiesGang()
    {
        BlankLocation YardiesBumbaclot = new BlankLocation()
        {
            Name = "YardiesBumbaclot",
            FullName = "",
            Description = "Yardies behind Bumbaclots",
            EntrancePosition = new Vector3(6576.60254f, -3192.16943f, 25.5843143f),
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
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
            Location = new Vector3(6580.213f, -3192.7312f, 25.9060535f),
            Heading = 168.2886f,
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            },
            },
            },
            PossibleVehicleSpawns = new List<ConditionalLocation>() {
            new GangConditionalLocation() {
            Location = new Vector3(6576.60254f, -3192.16943f, 25.5843143f),
            Heading = -25.60437f,
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
            Location = new Vector3(6580.213f, -3192.7312f, 25.9060535f),
            Heading = 168.2886f,
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_DRINKING",
            "WORLD_HUMAN_SMOKING",
            },
            },
            new GangConditionalLocation() {
            Location = new Vector3(6578.884f, -3193.82275f, 25.9060535f),
            Heading = -39.99989f,
            TaskRequirements = TaskRequirements.Guard,
            ForcedScenarios = new List<String>() {
            "WORLD_HUMAN_STAND_IMPATIENT_FACILITY",
            "WORLD_HUMAN_HANG_OUT_STREET",
            "WORLD_HUMAN_STAND_MOBILE",
            },
            },
            new GangConditionalLocation() {
            Location = new Vector3(6579.531f, -3194.769f, 25.9058743f),
            Heading = -19.42001f,
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
            Location = new Vector3(6576.60254f, -3192.16943f, 25.5843143f),
            Heading = 179.6279f,
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
            EntrancePosition = new Vector3(6550.384f, -3209.82373f, 32.23263f),
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
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
            Location = new Vector3(6550.384f, -3209.82373f, 32.23263f),
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
            Location = new Vector3(6550.384f, -3209.82373f, 32.23263f),
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
            Location = new Vector3(6551.30664f, -3210.10522f, 32.22161f),
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
            Location = new Vector3(6550.552f, -3211.04126f, 32.2257f),
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
            EntrancePosition = new Vector3(6465.318f, -3188.632f, 38.10287f),
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
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
            Location = new Vector3(6465.318f, -3188.632f, 38.10287f),
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
            Location = new Vector3(6466.34961f, -3188.99585f, 38.0644035f),
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
            Location = new Vector3(6465.18652f, -3190.37183f, 37.9616127f),
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
            Location = new Vector3(6466.14258f, -3190.97827f, 37.90226f),
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
            Location = new Vector3(6464.984f, -3191.245f, 37.8934326f),
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
            Location = new Vector3(6460.424f, -3191.2605f, 37.56671f),
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
        // EndOfStreamException of xml converts
        BlankLocation YardiesAlley1 = new BlankLocation()
        {
            Name = "YardiesAlley1",
            FullName = "",
            Description = "",
            EntrancePosition = new Vector3(6962.59863f, -2735.81616f, 28.629673f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            PossibleGroupSpawns =
              new List<ConditionalGroup>()
              {
                    new ConditionalGroup()
                    {
                      Name = "",
                      Percentage = defaultSpawnPercentage,
                      MinHourSpawn = 10,
                      MaxHourSpawn = 22,
                      PossiblePedSpawns = new List<ConditionalLocation>()
                      {
                                new GangConditionalLocation()
                                {
                                      Location = new Vector3(6967.113f, -2737.88525f, 28.6265144f),
                                      Heading = 93.53442f,
                                      TaskRequirements = TaskRequirements.Guard,
                                      ForcedScenarios =
                                      new List<String>()
                                      {
                                        "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                        "WORLD_HUMAN_DRUG_DEALER_HARD",
                                      },
                                },
                                new GangConditionalLocation() {
                                      Location = new Vector3(6965.65f, -2737.95825f, 28.7178345f),
                                      TaskRequirements = TaskRequirements.Guard,
                                      ForcedScenarios =
                                      new List<String>() {
                                        "WORLD_HUMAN_HANG_OUT_STREET",
                                        "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                      },
                                },
                      },
                      PossibleVehicleSpawns = new List<ConditionalLocation>() 
                      {
                                new GangConditionalLocation() 
                                {
                                  Location = new Vector3(6962.59863f, -2735.81616f, 28.629673f),
                                  Heading = 316.3719f,
                                },
                      },
                    },
              },
        };
        BlankLocationPlaces.Add(YardiesAlley1);
        BlankLocation YardiesStreet5 = new BlankLocation()
        {

            Name = "YardiesStreet5",
            FullName = "",
            Description = "",
            EntrancePosition = new Vector3(7012.43262f, -2851.54f, 26.4684639f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            MenuID = "",

            PossibleGroupSpawns =
            new List<ConditionalGroup>()
            {
                new ConditionalGroup()
                {
                  Name = "",
                  Percentage = defaultSpawnPercentage,
                  MinHourSpawn = 16,
                  MaxHourSpawn = 4,
                  PossiblePedSpawns = new List<ConditionalLocation>() 
                  {
                            new GangConditionalLocation() 
                            {
                              Location = new Vector3(7013.593f, -2846.854f, 26.7858639f),
                              Heading = 94.53841f,
                              TaskRequirements = TaskRequirements.Guard,
                              ForcedScenarios =
                                  new List<String>() 
                                  {
                                    "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                    "WORLD_HUMAN_DRUG_DEALER_HARD",
                                  },
                            },
                            new GangConditionalLocation() 
                            {
                              Location = new Vector3(7013.578f, -2847.9292f, 26.7690029f),
                              Heading = 82.53756f,
                              TaskRequirements = TaskRequirements.Guard,
                              ForcedScenarios =
                                  new List<String>() 
                                  {
                                    "WORLD_HUMAN_HANG_OUT_STREET",
                                    "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                  },
                            },
                  },
                  PossibleVehicleSpawns = new List<ConditionalLocation>() 
                  {
                            new GangConditionalLocation() 
                            {
                              Location = new Vector3(7012.43262f, -2851.54f, 26.4684639f),
                              Heading = 270.4164f,
                            },
                  },
                },
            },
        };
        BlankLocationPlaces.Add(YardiesStreet5);
        BlankLocation YardiesAlley2 = new BlankLocation()
        {
            Name = "YardiesAlley2",
            FullName = "",
            Description = "",
            EntrancePosition = new Vector3(6876.649f, -2615.162f, 28.5842133f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            PossibleGroupSpawns = new List<ConditionalGroup>() 
            {
                new ConditionalGroup() 
                {
                  Name = "",
                  Percentage = defaultSpawnPercentage,
                  MinHourSpawn = 12,
                  MaxHourSpawn = 20,
                  PossiblePedSpawns = new List<ConditionalLocation>()
                  {
                            new GangConditionalLocation() {
                              Location = new Vector3(6880.22559f, -2614.874f, 29.0171032f),
                              Heading = 93.33098f,
                              TaskRequirements = TaskRequirements.Guard,
                              ForcedScenarios =
                                  new List<String>() {
                                    "WORLD_HUMAN_LEANING_CASINO_TERRACE",
                                    "WORLD_HUMAN_DRUG_DEALER_HARD",
                                  },
                            },
                            new GangConditionalLocation() {
                              Location = new Vector3(6880.302f, -2613.51318f, 29.0024338f),
                              Heading = 120.6347f,
                              TaskRequirements = TaskRequirements.Guard,
                              ForcedScenarios =
                                  new List<String>() {
                                    "WORLD_HUMAN_HANG_OUT_STREET",
                                    "WORLD_HUMAN_SMOKING_POT_CLUBHOUSE",
                                  },
                            },
                  },
                  PossibleVehicleSpawns = new List<ConditionalLocation>()
                  {
                            new GangConditionalLocation()
                            {
                              Location = new Vector3(6876.649f, -2615.162f, 28.5842133f),
                              Heading = 178.8307f,
                            },
                  },
                },
            },
        };
        BlankLocationPlaces.Add(YardiesAlley2);
        BlankLocation YardiesAlPizza = new BlankLocation()
        {
            Name = "YardiesAlPizza",
            Description = "",
            EntrancePosition = new Vector3(6626.498f, -3034.597f, 25.1828136f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 18,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(6623.957f, -3031.406f, 26.0781727f),
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
                                        Location = new Vector3(6624.753f, -3031.56177f, 26.0366344f),
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
                                    Location = new Vector3(6626.498f, -3034.597f, 25.1828136f),
                                    Heading = 91.5689f,
                                },
                            }
                    }
                },
        };
        BlankLocationPlaces.Add(YardiesAlPizza);
        BlankLocation YardiesStore = new BlankLocation()
        {
            Name = "YardiesStore",
            Description = "",
            EntrancePosition = new Vector3(6568.148f, -3134.876f, 31.2642937f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 12,
                            MaxHourSpawn = 20,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6568.148f, -3134.876f, 31.2642937f),
                                            Heading = 64.86868f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6568.76172f, -3134.108f, 31.2642937f),
                                            Heading = 62.36359f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_SMOKING_POT",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(YardiesStore);
        BlankLocation YardiesEarpSt = new BlankLocation()
        {
            Name = "YardiesEarpSt",
            Description = "",
            EntrancePosition = new Vector3(6413.83f, -3224.645f, 34.91577f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 125f,
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(6410.486f, -3226.05737f, 35.5914f),
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
                                        Location = new Vector3(6410.43262f, -3225.202f, 35.590313f),
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
                                    Location = new Vector3(6413.83f, -3224.645f, 34.91577f),
                                    Heading = 180.7651f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(YardiesEarpSt);
        BlankLocation YardiesStreet6 = new BlankLocation()
        {

            Name = "YardiesStreet6",
            Description = "",
            EntrancePosition = new Vector3(6526.76465f, -3137.89063f, 31.5747643f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
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
                            MaxHourSpawn = 4,
                            MinWantedLevelSpawn = 0,
                            MaxWantedLevelSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(6526.76465f, -3137.89063f, 31.5747643f),
                                            Heading = 179.4212f,
                                            Percentage = 0f,
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
                                                "WORLD_HUMAN_DRUG_DEALER",
                                            },
                                            OverrideNightPercentage = -1f,
                                            OverrideDayPercentage = -1f,
                                            OverridePoorWeatherPercentage = -1f,
                                            MinHourSpawn = 8,
                                            MaxHourSpawn = 4,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 4,
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(6525.416f, -3137.86426f, 31.6656036f),
                                            Heading = 178.3786f,
                                            Percentage = 0f,
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
                                            MinHourSpawn = 8,
                                            MaxHourSpawn = 4,
                                            MinWantedLevelSpawn = 0,
                                            MaxWantedLevelSpawn = 4,
                                    },
                            },
                            PossibleVehicleSpawns = new List<ConditionalLocation>()
                            {},
                    },
            },
        };
        BlankLocationPlaces.Add(YardiesStreet6);
        BlankLocation YardiesStreet7 = new BlankLocation()
        {
            Name = "YardiesStreet7",
            Description = "",
            EntrancePosition = new Vector3(6509.4f, -3267.95044f, 28.6446629f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6509.4f, -3267.95044f, 28.6446629f),
                                            Heading = 181.8402f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_DRUG_DEALER",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6510.565f, -3267.97266f, 28.6290226f),
                                            Heading = 136.7053f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(YardiesStreet7);
        BlankLocation YardiesStreet8 = new BlankLocation()
        {

            Name = "YardiesStreet8",
            Description = "",
            EntrancePosition = new Vector3(6331.961f, -3226.96436f, 34.52973f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6331.961f, -3226.96436f, 34.52973f),
                                            Heading = 179.9618f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_DRUG_DEALER",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6330.98975f, -3227.01343f, 34.50624f),
                                            Heading = 181.4044f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(YardiesStreet8);
        BlankLocation YardiesStreet9 = new BlankLocation()
        {
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            Name = "YardiesStreet9",
            Description = "",
            EntrancePosition = new Vector3(6360.84668f, -3185.70435f, 35.80646f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(6364.39453f, -3184.8623f, 36.5632f),
                                            Heading = 275.6622f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(6364.56152f, -3183.6582f, 36.6213226f),
                                            Heading = 244.9385f,
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
                                    Location = new Vector3(6360.84668f, -3185.70435f, 35.80646f),
                                    Heading = 223.2164f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(YardiesStreet9);
        BlankLocation YardiesStreet10 = new BlankLocation()
        {
            Name = "YardiesStreet10",
            Description = "",
            EntrancePosition = new Vector3(6485.42f, -3000.87354f, 36.6706924f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6485.42f, -3000.87354f, 36.6706924f),
                                            Heading = 186.1092f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_DRUG_DEALER",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6486.708f, -3000.80664f, 36.555542f),
                                            Heading = 174.5735f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(YardiesStreet10);
        BlankLocation YardiesStreet11 = new BlankLocation()
        {
            Name = "YardiesStreet11",
            Description = "",
            EntrancePosition = new Vector3(6934.406f, -2888.51221f, 21.765564f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(6934.293f, -2890.686f, 22.273243f),
                                            Heading = 179.6925f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                        Location = new Vector3(6933.07666f, -2890.653f, 22.3035927f),
                                            Heading = 179.4467f,
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
                                    Location = new Vector3(6934.406f, -2888.51221f, 21.765564f),
                                    Heading = 270.1775f,
                                },
                            }
                    }
            },
        };
        BlankLocationPlaces.Add(YardiesStreet11);
        BlankLocation YardiesStreet12 = new BlankLocation()
        {

            Name = "YardiesStreet12",
            Description = "",
            EntrancePosition = new Vector3(6919.155f, -2793.72852f, 27.7628136f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            PossibleGroupSpawns = new List<ConditionalGroup>()
            {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6919.155f, -2793.72852f, 27.7628136f),
                                            Heading = 183.4438f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_DRUG_DEALER",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6917.82568f, -2793.68f, 27.740963f),
                                            Heading = 185.5538f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        };
        BlankLocationPlaces.Add(YardiesStreet12);
        BlankLocation YardiesHomebrew = new BlankLocation()
        {

            Name = "YardiesHomebrew",
            Description = "",
            EntrancePosition = new Vector3(6662.651f, -3198.91113f, 25.1854439f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            InteriorID = 121346,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 75f,
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                        Name = "",
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 23,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6662.651f, -3198.91113f, 25.1854439f),
                                            Heading = 91.25574f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_DRUG_DEALER",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6662.68262f, -3197.86963f, 25.1854343f),
                                            Heading = 91.63627f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_CLUBHOUSE",
                                            },
                                    },
                            },
                    },
            },
        }; // interior
        BlankLocationPlaces.Add(YardiesHomebrew);
        BlankLocation YardiesDrugFactory = new BlankLocation()
        {
            Name = "YardiesDrugFactory",
            Description = "",
            EntrancePosition = new Vector3(6529.997f, -3435.56958f, 22.5102444f),
            EntranceHeading = 0f,
            OpenTime = 0,
            CloseTime = 24,
            StateID = StaticStrings.LibertyStateID,
            ActivateDistance = 100f,
            AssignedAssociationID = "AMBIENT_GANG_YARDIES",
            PossibleGroupSpawns = new List<ConditionalGroup>()
                {
                    new ConditionalGroup()
                    {
                            Percentage = defaultSpawnPercentage,
                            MinHourSpawn = 8,
                            MaxHourSpawn = 4,
                            PossiblePedSpawns = new List<ConditionalLocation>()
                            {
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6533.2666f, -3435.27637f, 22.9084034f),
                                            Heading = 82.05704f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_SMOKING_CLUBHOUSE",
                                                "WORLD_HUMAN_STAND_MOBILE_FACILITY",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6537.424f, -3436.03857f, 22.4912033f),
                                            Heading = 358.1976f,
                                            TaskRequirements = TaskRequirements.Guard,
                                            ForcedScenarios = new List<string>()
                                            {
                                                "WORLD_HUMAN_HANG_OUT_STREET",
                                                "WORLD_HUMAN_SMOKING",
                                            },
                                    },
                                    new GangConditionalLocation()
                                    {
                                            Location = new Vector3(6537.402f, -3434.64722f, 22.5304127f),
                                            Heading = 173.7161f,
                                            MinHourSpawn = 8,
                                            MaxHourSpawn = 4,
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
                                    Location = new Vector3(6529.997f, -3435.56958f, 22.5102444f),
                                    Heading = 175.0275f,
                                },
                            }
                    }
                },
        };
        BlankLocationPlaces.Add(YardiesDrugFactory);
    }

}