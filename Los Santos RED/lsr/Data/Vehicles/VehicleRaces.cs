using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


public class VehicleRaces : IVehicleRaces
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\VehicleRaces.xml";
    public VehicleRaceTypeManager VehicleRaceTypeManager { get; private set; }
    public void ReadConfig(string configName)
    {
        string fileName = string.IsNullOrEmpty(configName) ? "VehicleRaces_*.xml" : $"VehicleRaces_{configName}.xml";

        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles(fileName).Where(x => !x.Name.Contains("+")).OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null && !configName.Equals("Default"))
        {
            EntryPoint.WriteToConsole($"Loaded VehicleRaces config: {ConfigFile.FullName}", 0);
            VehicleRaceTypeManager = Serialization.DeserializeParam<VehicleRaceTypeManager>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded VehicleRaces config  {ConfigFileName}", 0);
            VehicleRaceTypeManager = Serialization.DeserializeParam<VehicleRaceTypeManager>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No VehicleRaces config found, creating default", 0);
            DefaultConfig();
            //DefaultConfig_Liberty();
            DefaultConfig_LibertyPP();
        }
        foreach (FileInfo fileInfo in LSRDirectory.GetFiles("VehicleRaces+_*.xml").OrderByDescending(x => x.Name))
        {
            EntryPoint.WriteToConsole($"Loaded ADDITIVE Vehicle Races config  {fileInfo.FullName}", 0);
            VehicleRaceTypeManager additivePossibleItems = Serialization.DeserializeParam<VehicleRaceTypeManager>(fileInfo.FullName);
            foreach (VehicleRaceTrack newItem in additivePossibleItems.VehicleRaceTracks)
            {
                VehicleRaceTypeManager.VehicleRaceTracks.RemoveAll(x => x.ID == newItem.ID);
                VehicleRaceTypeManager.VehicleRaceTracks.Add(newItem);
            }
        }
    }

    private void DefaultConfig_Liberty()
    {
        VehicleRaces_Liberty vehicleRaces_Liberty = new VehicleRaces_Liberty(this);
        vehicleRaces_Liberty.DefaultConfig();
    }
    private void DefaultConfig_LibertyPP()
    {
        VehicleRaces_LibertyPP vehicleRaces_Liberty = new VehicleRaces_LibertyPP(this);
        vehicleRaces_Liberty.DefaultConfig();
    }
    private void DefaultConfig()
    {
        VehicleRaceTypeManager = new VehicleRaceTypeManager();
        VehicleRaceTypeManager.VehicleRaceTracks = new List<VehicleRaceTrack>();

        SandyTracks();
        VineWoodTracks();
        PaletoTracks();
        CentralTracks();
        OffroadTracks();
        WaterTracks();

        Serialization.SerializeParam(VehicleRaceTypeManager, ConfigFileName);
    }
    private void SandyTracks()
    {
         // Circuit

        List<VehicleRaceStartingPosition> alamoseaposistionsstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(2127.538f, 3763.589f, 32.104f), 119.643f),
            new VehicleRaceStartingPosition(1, new Vector3(2129.022f, 3760.982f, 32.125f), 119.643f),
            new VehicleRaceStartingPosition(2, new Vector3(2136.229f, 3768.535f, 32.156f), 119.643f),
            new VehicleRaceStartingPosition(3, new Vector3(2137.713f, 3765.928f, 32.180f), 119.643f),
            new VehicleRaceStartingPosition(4, new Vector3(2144.920f, 3773.481f, 32.216f), 119.643f),
            new VehicleRaceStartingPosition(5, new Vector3(2146.404f, 3770.874f, 32.239f), 119.643f),
            new VehicleRaceStartingPosition(6, new Vector3(2153.612f, 3778.427f, 32.281f), 119.643f),
            new VehicleRaceStartingPosition(7, new Vector3(2155.095f, 3775.819f, 32.303f), 119.643f),
        };
        List<VehicleRaceCheckpoint> alamoseaposistionscheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(1854.954f, 3600.867f, 34.106f)),
            new VehicleRaceCheckpoint(1, new Vector3(1552.818f, 3482.490f, 35.876f)),
            new VehicleRaceCheckpoint(2, new Vector3(1222.984f, 3536.908f, 34.534f)),
            new VehicleRaceCheckpoint(3, new Vector3(801.730f, 3532.102f, 33.563f)),
            new VehicleRaceCheckpoint(4, new Vector3(361.287f, 3461.293f, 34.831f)),
            new VehicleRaceCheckpoint(5, new Vector3(142.001f, 3414.932f, 39.858f)),
            new VehicleRaceCheckpoint(6, new Vector3(-27.763f, 3604.390f, 42.533f)),
            new VehicleRaceCheckpoint(7, new Vector3(-221.838f, 3853.239f, 38.569f)),
            new VehicleRaceCheckpoint(8, new Vector3(-224.501f, 4146.389f, 40.902f)),
            new VehicleRaceCheckpoint(9, new Vector3(-80.151f, 4321.305f, 48.238f)),
            new VehicleRaceCheckpoint(10, new Vector3(18.310f, 4453.384f, 59.340f)),
            new VehicleRaceCheckpoint(11, new Vector3(244.790f, 4490.145f, 66.660f)),
            new VehicleRaceCheckpoint(12, new Vector3(397.567f, 4397.190f, 62.068f)),
            new VehicleRaceCheckpoint(13, new Vector3(629.607f, 4239.562f, 53.573f)),
            new VehicleRaceCheckpoint(14, new Vector3(858.100f, 4302.244f, 50.538f)),
            new VehicleRaceCheckpoint(15, new Vector3(881.925f, 4470.798f, 51.294f)),
            new VehicleRaceCheckpoint(16, new Vector3(1107.221f, 4425.840f, 63.092f)),
            new VehicleRaceCheckpoint(17, new Vector3(1282.405f, 4467.675f, 60.434f)),
            new VehicleRaceCheckpoint(18, new Vector3(1480.628f, 4511.907f, 51.875f)),
            new VehicleRaceCheckpoint(19, new Vector3(1643.532f, 4570.073f, 43.461f)),
            new VehicleRaceCheckpoint(20, new Vector3(1946.609f, 4596.724f, 38.948f)),
            new VehicleRaceCheckpoint(21, new Vector3(2221.387f, 4738.859f, 39.460f)),
            new VehicleRaceCheckpoint(22, new Vector3(2482.120f, 4511.285f, 34.051f)),
            new VehicleRaceCheckpoint(23, new Vector3(2466.968f, 4177.150f, 36.619f)),
            new VehicleRaceCheckpoint(24, new Vector3(2493.392f, 4096.705f, 37.494f)),
            new VehicleRaceCheckpoint(25, new Vector3(2310.300f, 3864.002f, 34.152f)),
            new VehicleRaceCheckpoint(26, new Vector3(2128.230f, 3758.647f, 32.451f)),
        };
        VehicleRaceTrack alamoseaposistions = new VehicleRaceTrack("alamo_sea1", "Circuit - The Alamo Run", "Race Around the Alamo Sea", alamoseaposistionscheckpoints, alamoseaposistionsstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(alamoseaposistions);

        List<VehicleRaceStartingPosition> grandsenloop1StartingPositions = new List<VehicleRaceStartingPosition>()
        {
            //new VehicleRaceStartingPosition(0, new Vector3(1868.027f, 3226.604f, 44.5677f), 39.27186f),
            //new VehicleRaceStartingPosition(1, new Vector3(1876.75f, 3215.834f, 44.83164f), 39.09522f),
            //new VehicleRaceStartingPosition(2, new Vector3(1866.469f, 3220.694f, 44.6372f), 43.98711f),
            //new VehicleRaceStartingPosition(3, new Vector3(1874.058f, 3211.186f, 44.86393f), 38.87733f),

            new VehicleRaceStartingPosition(0, new Vector3(1762.471f, 3383.790f, 38.173f), 208.951f),
            new VehicleRaceStartingPosition(1, new Vector3(1765.096f, 3385.242f, 38.195f), 208.951f),
            new VehicleRaceStartingPosition(2, new Vector3(1757.630f, 3392.540f, 37.819f), 208.951f),
            new VehicleRaceStartingPosition(3, new Vector3(1760.255f, 3393.992f, 37.839f), 208.951f),
            new VehicleRaceStartingPosition(4, new Vector3(1752.790f, 3401.291f, 37.476f), 208.951f),
            new VehicleRaceStartingPosition(5, new Vector3(1755.415f, 3402.743f, 37.496f), 208.951f),
            new VehicleRaceStartingPosition(6, new Vector3(1747.949f, 3410.041f, 37.143f), 208.951f),
            new VehicleRaceStartingPosition(7, new Vector3(1750.574f, 3411.493f, 37.166f), 208.951f),
        };
        List<VehicleRaceCheckpoint> grandsenloop1Checkpoints = new List<VehicleRaceCheckpoint>()
        {
            //new VehicleRaceCheckpoint(0, new Vector3(1885.493f, 3201.551f, 45.120f)),
            //new VehicleRaceCheckpoint(1, new Vector3(2055.093f, 3042.086f, 45.293f)),
            //new VehicleRaceCheckpoint(2, new Vector3(1891.462f, 2960.925f, 45.093f)),
            //new VehicleRaceCheckpoint(3, new Vector3(1504.752f, 2750.472f, 37.249f)),
            //new VehicleRaceCheckpoint(4, new Vector3(1088.613f, 2687.126f, 38.163f)),
            //new VehicleRaceCheckpoint(5, new Vector3(682.750f, 2700.623f, 39.931f)),
            //new VehicleRaceCheckpoint(6, new Vector3(394.135f, 2667.029f, 43.715f)),
            //new VehicleRaceCheckpoint(7, new Vector3(272.162f, 2686.608f, 43.595f)),
            //new VehicleRaceCheckpoint(8, new Vector3(220.088f, 3067.667f, 41.627f)),
            //new VehicleRaceCheckpoint(9, new Vector3(328.536f, 3442.644f, 35.556f)),
            //new VehicleRaceCheckpoint(10, new Vector3(792.373f, 3531.243f, 33.566f)),
            //new VehicleRaceCheckpoint(11, new Vector3(1203.844f, 3536.679f, 34.493f)),
            //new VehicleRaceCheckpoint(12, new Vector3(1593.483f, 3480.125f, 35.977f)),
            //new VehicleRaceCheckpoint(13, new Vector3(1724.797f, 3460.826f, 38.333f)),

            new VehicleRaceCheckpoint(0, new Vector3(1912.500f, 3173.500f, 45.375f)),
            new VehicleRaceCheckpoint(1, new Vector3(2052.236f, 3035.109f, 44.777f)),
            new VehicleRaceCheckpoint(2, new Vector3(1867.519f, 2956.421f, 44.757f)),
            //new VehicleRaceCheckpoint(3, new Vector3(1418.775f, 2706.741f, 36.544f)),
            new VehicleRaceCheckpoint(3, new Vector3(993.525f, 2695.115f, 38.790f)),
            new VehicleRaceCheckpoint(4, new Vector3(377.337f, 2665.131f, 43.641f)),
            new VehicleRaceCheckpoint(5, new Vector3(274.845f, 2686.428f, 43.259f)),
            new VehicleRaceCheckpoint(6, new Vector3(218.272f, 3197.761f, 41.613f)),
            new VehicleRaceCheckpoint(7, new Vector3(390.465f, 3469.182f, 33.894f)),
            new VehicleRaceCheckpoint(8, new Vector3(794.304f, 3528.805f, 33.221f)),
            new VehicleRaceCheckpoint(9, new Vector3(1203.761f, 3534.222f, 34.152f)),
            new VehicleRaceCheckpoint(10, new Vector3(1595.541f, 3477.658f, 35.638f)),
            new VehicleRaceCheckpoint(11, new Vector3(1725.000f, 3460.500f, 38.000f)),

        };
        VehicleRaceTrack grandsen = new VehicleRaceTrack("grandsen1", "Circuit - Senora Rush", "Race across the Grand Senora desert roads", grandsenloop1Checkpoints, grandsenloop1StartingPositions);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(grandsen);

        List<VehicleRaceStartingPosition> grapeseedloopstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(2049.423f, 4664.459f, 40.171f), 314.548f),
            new VehicleRaceStartingPosition(1, new Vector3(2047.319f, 4666.597f, 40.195f), 314.548f),
            new VehicleRaceStartingPosition(2, new Vector3(2042.296f, 4657.444f, 40.177f), 314.548f),
            new VehicleRaceStartingPosition(3, new Vector3(2040.192f, 4659.582f, 40.198f), 314.548f),
            new VehicleRaceStartingPosition(4, new Vector3(2035.170f, 4650.429f, 40.175f), 314.548f),
            new VehicleRaceStartingPosition(5, new Vector3(2033.065f, 4652.567f, 40.200f), 314.548f),
            new VehicleRaceStartingPosition(6, new Vector3(2028.043f, 4643.414f, 40.174f), 314.548f),
            new VehicleRaceStartingPosition(7, new Vector3(2025.938f, 4645.552f, 40.199f), 314.548f),
        };
        List<VehicleRaceCheckpoint> grapeseedloopcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(2192.760f, 4805.758f, 43.921f)),
            new VehicleRaceCheckpoint(1, new Vector3(2125.926f, 4954.750f, 40.524f)),
            new VehicleRaceCheckpoint(2, new Vector3(1854.648f, 5093.819f, 52.413f)),
            new VehicleRaceCheckpoint(3, new Vector3(1673.074f, 4820.303f, 41.509f)),
            new VehicleRaceCheckpoint(4, new Vector3(1813.701f, 4574.302f, 35.693f)),
            new VehicleRaceCheckpoint(5, new Vector3(2020.495f, 4641.950f, 40.677f)),
        };
        VehicleRaceTrack grapeseedloop1 = new VehicleRaceTrack("grapeseed_loop", "Circuit - The Grapeseed Run", "Race Around Grapeseed", grapeseedloopcheckpoints, grapeseedloopstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(grapeseedloop1);

        List<VehicleRaceStartingPosition> grapseedrevstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(2074.142f, 4701.584f, 40.172f), 134.952f),
            new VehicleRaceStartingPosition(1, new Vector3(2076.261f, 4699.461f, 40.197f), 134.952f),
            new VehicleRaceStartingPosition(2, new Vector3(2079.803f, 4707.236f, 40.173f), 134.952f),
            new VehicleRaceStartingPosition(3, new Vector3(2081.922f, 4705.113f, 40.196f), 134.952f),
            new VehicleRaceStartingPosition(4, new Vector3(2085.464f, 4712.889f, 40.175f), 134.952f),
            new VehicleRaceStartingPosition(5, new Vector3(2087.584f, 4710.766f, 40.195f), 134.952f),
            new VehicleRaceStartingPosition(6, new Vector3(2091.125f, 4718.542f, 40.172f), 134.952f),
            new VehicleRaceStartingPosition(7, new Vector3(2093.245f, 4716.418f, 40.197f), 134.952f),
        };
        List<VehicleRaceCheckpoint> grapseedrevcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(1951.750f, 4598.250f, 38.813f)),
            new VehicleRaceCheckpoint(1, new Vector3(1809.500f, 4573.750f, 35.250f)),
            new VehicleRaceCheckpoint(2, new Vector3(1717.750f, 4613.250f, 41.469f)),
            new VehicleRaceCheckpoint(3, new Vector3(1692.000f, 4725.500f, 41.250f)),
            new VehicleRaceCheckpoint(4, new Vector3(1665.750f, 4878.250f, 41.063f)),
            new VehicleRaceCheckpoint(5, new Vector3(1769.664f, 5019.698f, 52.885f)),
            new VehicleRaceCheckpoint(6, new Vector3(1886.364f, 5118.755f, 46.546f)),
            new VehicleRaceCheckpoint(7, new Vector3(2003.250f, 5087.250f, 41.500f)),
            new VehicleRaceCheckpoint(8, new Vector3(2125.250f, 4955.000f, 40.031f)),
            new VehicleRaceCheckpoint(9, new Vector3(2196.750f, 4829.000f, 43.844f)),
            new VehicleRaceCheckpoint(10, new Vector3(2137.500f, 4751.250f, 40.188f)),
            new VehicleRaceCheckpoint(11, new Vector3(2049.500f, 4670.750f, 40.188f)),
        };
        VehicleRaceTrack grapseedrev = new VehicleRaceTrack("grapseedrev", "Circuit - The Grapeseed Run Reverse", "Race Around Grapeseed", grapseedrevcheckpoints, grapseedrevstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(grapseedrev);

        List<VehicleRaceStartingPosition> grapeseed2start = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(2549.023f, 4190.158f, 39.376f), 329.049f),
            new VehicleRaceStartingPosition(1, new Vector3(2546.450f, 4191.701f, 39.472f), 329.049f),
            new VehicleRaceStartingPosition(2, new Vector3(2543.880f, 4181.582f, 39.071f), 329.049f),
            new VehicleRaceStartingPosition(3, new Vector3(2541.307f, 4183.125f, 39.169f), 329.049f),
            new VehicleRaceStartingPosition(4, new Vector3(2538.737f, 4173.005f, 38.750f), 329.049f),
            new VehicleRaceStartingPosition(5, new Vector3(2536.164f, 4174.548f, 38.776f), 329.049f),
            new VehicleRaceStartingPosition(6, new Vector3(2533.594f, 4164.430f, 38.450f), 329.049f),
            new VehicleRaceStartingPosition(7, new Vector3(2531.021f, 4165.973f, 38.526f), 329.049f),
        };
        List<VehicleRaceCheckpoint> grapeseed2checkpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(2690.750f, 4368.750f, 45.625f)),
            new VehicleRaceCheckpoint(1, new Vector3(2874.000f, 4433.500f, 47.750f)),
            new VehicleRaceCheckpoint(2, new Vector3(2983.250f, 4552.750f, 50.219f)),
            new VehicleRaceCheckpoint(3, new Vector3(2840.750f, 4820.250f, 47.063f)),
            new VehicleRaceCheckpoint(4, new Vector3(2769.250f, 4977.000f, 32.719f)),
            new VehicleRaceCheckpoint(5, new Vector3(2671.250f, 5123.000f, 43.781f)),
            new VehicleRaceCheckpoint(6, new Vector3(2474.750f, 5109.250f, 45.188f)),
            new VehicleRaceCheckpoint(7, new Vector3(2320.500f, 5203.500f, 58.813f)),
            new VehicleRaceCheckpoint(8, new Vector3(2146.500f, 5225.250f, 58.344f)),
            new VehicleRaceCheckpoint(9, new Vector3(1998.250f, 5159.250f, 45.125f)),
            new VehicleRaceCheckpoint(10, new Vector3(1842.250f, 5083.750f, 53.938f)),
            new VehicleRaceCheckpoint(11, new Vector3(1702.000f, 4964.500f, 42.844f)),
            new VehicleRaceCheckpoint(12, new Vector3(1676.750f, 4771.000f, 40.969f)),
            new VehicleRaceCheckpoint(13, new Vector3(1717.750f, 4613.250f, 41.469f)),
            new VehicleRaceCheckpoint(14, new Vector3(1839.250f, 4577.750f, 35.156f)),
            new VehicleRaceCheckpoint(15, new Vector3(2049.500f, 4670.750f, 40.188f)),
            new VehicleRaceCheckpoint(16, new Vector3(2231.250f, 4737.250f, 38.781f)),
            new VehicleRaceCheckpoint(17, new Vector3(2417.500f, 4628.250f, 35.938f)),
            new VehicleRaceCheckpoint(18, new Vector3(2486.250f, 4444.250f, 34.500f)),
            new VehicleRaceCheckpoint(19, new Vector3(2449.000f, 4233.750f, 36.000f)),
            new VehicleRaceCheckpoint(20, new Vector3(2546.500f, 4194.500f, 39.375f)),
        };
        VehicleRaceTrack grapeseed2 = new VehicleRaceTrack("grapeseed_loop2", "Circuit - The Grapeseed Run Long", "Race Around Grapeseed Long", grapeseed2checkpoints, grapeseed2start);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(grapeseed2);

        List<VehicleRaceStartingPosition> prisonrunstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(1860.350f, 2616.155f, 44.772f), 180.179f),
            new VehicleRaceStartingPosition(1, new Vector3(1864.850f, 2616.168f, 44.772f), 180.179f),
            new VehicleRaceStartingPosition(2, new Vector3(1860.319f, 2626.155f, 44.772f), 180.179f),
            new VehicleRaceStartingPosition(3, new Vector3(1864.819f, 2626.168f, 44.772f), 180.179f),
            new VehicleRaceStartingPosition(4, new Vector3(1860.287f, 2636.155f, 44.772f), 180.179f),
            new VehicleRaceStartingPosition(5, new Vector3(1864.787f, 2636.168f, 44.772f), 180.179f),
            new VehicleRaceStartingPosition(6, new Vector3(1860.256f, 2646.154f, 44.772f), 180.179f),
            new VehicleRaceStartingPosition(7, new Vector3(1864.756f, 2646.168f, 44.772f), 180.179f),
        };
        List<VehicleRaceCheckpoint> prisonruncheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(1825.500f, 2446.750f, 44.938f)),
            new VehicleRaceCheckpoint(1, new Vector3(1573.000f, 2425.250f, 44.969f)),
            new VehicleRaceCheckpoint(2, new Vector3(1537.000f, 2639.750f, 44.938f)),
            new VehicleRaceCheckpoint(3, new Vector3(1710.250f, 2775.250f, 44.969f)),
            new VehicleRaceCheckpoint(4, new Vector3(1862.750f, 2606.000f, 44.656f)),
        };
        VehicleRaceTrack prisonrun = new VehicleRaceTrack("prisonrun", "Circuit - Prison Run", "Take Your Chance and Race Around the Prison", prisonruncheckpoints, prisonrunstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(prisonrun); // Slow...yawn

        List<VehicleRaceStartingPosition> prisonrun2start = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(1864.713f, 2594.006f, 44.772f), 359.712f),
            new VehicleRaceStartingPosition(1, new Vector3(1860.213f, 2594.028f, 44.772f), 359.712f),
            new VehicleRaceStartingPosition(2, new Vector3(1864.663f, 2584.006f, 44.772f), 359.712f),
            new VehicleRaceStartingPosition(3, new Vector3(1860.163f, 2584.028f, 44.772f), 359.712f),
            new VehicleRaceStartingPosition(4, new Vector3(1864.613f, 2574.006f, 44.772f), 359.712f),
            new VehicleRaceStartingPosition(5, new Vector3(1860.113f, 2574.029f, 44.772f), 359.712f),
            new VehicleRaceStartingPosition(6, new Vector3(1864.563f, 2564.006f, 44.772f), 359.712f),
            new VehicleRaceStartingPosition(7, new Vector3(1860.063f, 2564.029f, 44.772f), 359.712f),
        };
        List<VehicleRaceCheckpoint> prisonrun2checkpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(1808.250f, 2757.750f, 44.938f)),
            new VehicleRaceCheckpoint(1, new Vector3(1592.000f, 2724.500f, 44.969f)),
            new VehicleRaceCheckpoint(2, new Vector3(1523.250f, 2490.000f, 44.969f)),
            new VehicleRaceCheckpoint(3, new Vector3(1616.250f, 2399.250f, 44.938f)),
            new VehicleRaceCheckpoint(4, new Vector3(1787.250f, 2410.250f, 44.938f)),
            new VehicleRaceCheckpoint(5, new Vector3(1862.750f, 2606.000f, 44.656f)),
        };
        VehicleRaceTrack prisonrun2 = new VehicleRaceTrack("prisonrun2", "Circuit - Prison Run Reverse", "Take Your Chance and Race Around the Prison", prisonrun2checkpoints, prisonrun2start);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(prisonrun2); // Slow..yawn

        List<VehicleRaceStartingPosition> sandyLoop1StartingPositions = new List<VehicleRaceStartingPosition>()
        {
            //new VehicleRaceStartingPosition(0, new Vector3(1868.027f, 3226.604f, 44.5677f), 39.27186f),
            //new VehicleRaceStartingPosition(1, new Vector3(1876.75f, 3215.834f, 44.83164f), 39.09522f),
            //new VehicleRaceStartingPosition(2, new Vector3(1866.469f, 3220.694f, 44.6372f), 43.98711f),
            //new VehicleRaceStartingPosition(3, new Vector3(1874.058f, 3211.186f, 44.86393f), 38.87733f),

            new VehicleRaceStartingPosition(0, new Vector3(1756.488f, 3548.531f, 35.032f), 119.860f),
            new VehicleRaceStartingPosition(1, new Vector3(1757.981f, 3545.930f, 35.055f), 119.860f),
            new VehicleRaceStartingPosition(2, new Vector3(1765.160f, 3553.510f, 34.958f), 119.860f),
            new VehicleRaceStartingPosition(3, new Vector3(1766.654f, 3550.909f, 34.983f), 119.860f),
            new VehicleRaceStartingPosition(4, new Vector3(1773.833f, 3558.489f, 34.875f), 119.860f),
            new VehicleRaceStartingPosition(5, new Vector3(1775.326f, 3555.887f, 34.900f), 119.860f),
            new VehicleRaceStartingPosition(6, new Vector3(1782.505f, 3563.468f, 34.783f), 119.860f),
            new VehicleRaceStartingPosition(7, new Vector3(1783.999f, 3560.866f, 34.804f), 119.860f),
        };
        List<VehicleRaceCheckpoint> sandyLoop1Checkpoints = new List<VehicleRaceCheckpoint>()
        {
            //new VehicleRaceCheckpoint(0, new Vector3(1775.803f, 3375.738f, 39.1007f)),
            //new VehicleRaceCheckpoint(1,new Vector3(1582.85f, 3480.25f, 36.21559f)),
            //new VehicleRaceCheckpoint(2,new Vector3(984.0595f, 3535.763f, 33.54565f)),
            //new VehicleRaceCheckpoint(3,new Vector3(1979.965f, 3889.917f, 31.88055f)),

            new VehicleRaceCheckpoint(0, new Vector3(1697.084f, 3509.094f, 35.793f)),
            new VehicleRaceCheckpoint(1, new Vector3(1558.436f, 3481.920f, 35.898f)),
            new VehicleRaceCheckpoint(2, new Vector3(1228.741f, 3536.875f, 34.539f)),
            new VehicleRaceCheckpoint(3, new Vector3(1004.028f, 3536.439f, 33.316f)),
            new VehicleRaceCheckpoint(4, new Vector3(928.631f, 3579.964f, 32.846f)),
            new VehicleRaceCheckpoint(5, new Vector3(1022.277f, 3630.133f, 32.089f)),
            new VehicleRaceCheckpoint(6, new Vector3(1385.016f, 3680.888f, 33.017f)),
            new VehicleRaceCheckpoint(7, new Vector3(1635.506f, 3830.415f, 34.332f)),
            new VehicleRaceCheckpoint(8, new Vector3(1773.904f, 3933.774f, 33.862f)),
            new VehicleRaceCheckpoint(9, new Vector3(1911.877f, 3944.526f, 31.855f)),
            new VehicleRaceCheckpoint(10, new Vector3(2020.714f, 3793.704f, 31.601f)),
            new VehicleRaceCheckpoint(11, new Vector3(2024.079f, 3698.767f, 32.503f)),
            new VehicleRaceCheckpoint(12, new Vector3(1850.979f, 3598.394f, 34.172f)),

        };
        VehicleRaceTrack sandyDebug2 = new VehicleRaceTrack("sandyloop2", "Circuit - Sandy Shores Run", "Race through the roads of Sandy Shores", sandyLoop1Checkpoints, sandyLoop1StartingPositions);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(sandyDebug2);


        //Drag

        List<VehicleRaceStartingPosition> sandymilestart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(1672.647f, 3617.894f, 34.520f), 300.156f),
            new VehicleRaceStartingPosition(1, new Vector3(1671.140f, 3620.488f, 34.538f), 300.156f),
            new VehicleRaceStartingPosition(2, new Vector3(1665.730f, 3613.875f, 34.499f), 300.156f),
            new VehicleRaceStartingPosition(3, new Vector3(1664.223f, 3616.469f, 34.521f), 300.156f),
            new VehicleRaceStartingPosition(4, new Vector3(1658.813f, 3609.856f, 34.479f), 300.156f),
            new VehicleRaceStartingPosition(5, new Vector3(1657.306f, 3612.450f, 34.495f), 300.156f),
            new VehicleRaceStartingPosition(6, new Vector3(1651.895f, 3605.838f, 34.461f), 300.156f),
            new VehicleRaceStartingPosition(7, new Vector3(1650.388f, 3608.432f, 34.478f), 300.156f),
        };
        List<VehicleRaceCheckpoint> sandymilecheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(2019.750f, 3821.750f, 31.500f)),
        };
        VehicleRaceTrack sandyquartermile = new VehicleRaceTrack("sandymile", "Drag - Zancudo Strip", "Quarter mile drag race along Zancudo Avenue", sandymilecheckpoints, sandymilestart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(sandyquartermile);


        // Point to Point

        List<VehicleRaceCheckpoint> grapestabcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(1951.750f, 4598.250f, 38.813f)),
            new VehicleRaceCheckpoint(1, new Vector3(1693.000f, 4570.750f, 40.500f)),
            new VehicleRaceCheckpoint(2, new Vector3(1558.250f, 4567.250f, 48.719f)),
            new VehicleRaceCheckpoint(3, new Vector3(1445.250f, 4497.500f, 49.500f)),
            new VehicleRaceCheckpoint(4, new Vector3(1285.000f, 4469.250f, 60.406f)),
            new VehicleRaceCheckpoint(5, new Vector3(1113.000f, 4424.750f, 62.719f)),
            new VehicleRaceCheckpoint(6, new Vector3(980.250f, 4422.750f, 45.406f)),
            new VehicleRaceCheckpoint(7, new Vector3(854.750f, 4490.750f, 52.281f)),
            new VehicleRaceCheckpoint(8, new Vector3(832.500f, 4396.750f, 51.188f)),
            new VehicleRaceCheckpoint(9, new Vector3(826.750f, 4255.250f, 53.469f)),
            new VehicleRaceCheckpoint(10, new Vector3(648.000f, 4240.750f, 53.281f)),
            new VehicleRaceCheckpoint(11, new Vector3(494.250f, 4314.250f, 54.938f)),
            new VehicleRaceCheckpoint(12, new Vector3(349.000f, 4486.750f, 61.375f)),
            new VehicleRaceCheckpoint(13, new Vector3(233.750f, 4476.750f, 67.250f)),
            new VehicleRaceCheckpoint(14, new Vector3(118.750f, 4430.000f, 70.750f)),
            new VehicleRaceCheckpoint(15, new Vector3(-49.750f, 4410.000f, 56.125f)),
            new VehicleRaceCheckpoint(16, new Vector3(-169.750f, 4243.500f, 43.906f)),
            new VehicleRaceCheckpoint(17, new Vector3(-235.750f, 4109.750f, 37.938f)),
            new VehicleRaceCheckpoint(18, new Vector3(-226.500f, 3923.250f, 36.438f)),
            new VehicleRaceCheckpoint(19, new Vector3(-196.500f, 3743.750f, 41.750f)),
            new VehicleRaceCheckpoint(20, new Vector3(-91.250f, 3610.500f, 43.813f)),
            new VehicleRaceCheckpoint(21, new Vector3(48.500f, 3594.000f, 38.781f)),
        };
        VehicleRaceTrack grapestab = new VehicleRaceTrack("grapestab", "P2P - County Lines", "Race from Grapeseed to Stab City", grapestabcheckpoints, grapseedrevstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(grapestab);

        List<VehicleRaceStartingPosition> grapep2ptwostart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(3287.949f, 3626.903f, 51.593f), 127.671f),
            new VehicleRaceStartingPosition(1, new Vector3(3289.793f, 3624.537f, 51.611f), 127.671f),
            new VehicleRaceStartingPosition(2, new Vector3(3295.048f, 3632.436f, 50.491f), 127.671f),
            new VehicleRaceStartingPosition(3, new Vector3(3296.892f, 3630.069f, 50.511f), 127.671f),
            new VehicleRaceStartingPosition(4, new Vector3(3302.147f, 3637.968f, 49.404f), 127.671f),
            new VehicleRaceStartingPosition(5, new Vector3(3303.991f, 3635.602f, 49.431f), 127.671f),
            new VehicleRaceStartingPosition(6, new Vector3(3309.246f, 3643.500f, 48.336f), 127.671f),
            new VehicleRaceStartingPosition(7, new Vector3(3311.090f, 3641.134f, 48.371f), 127.671f),
        };
        List<VehicleRaceCheckpoint> grapep2ptwocheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(3184.250f, 3517.750f, 69.281f)),
            new VehicleRaceCheckpoint(1, new Vector3(3119.000f, 3417.750f, 77.344f)),
            new VehicleRaceCheckpoint(2, new Vector3(3003.500f, 3464.750f, 70.563f)),
            new VehicleRaceCheckpoint(3, new Vector3(3028.750f, 3664.500f, 70.813f)),
            new VehicleRaceCheckpoint(4, new Vector3(3082.500f, 3860.500f, 74.813f)),
            new VehicleRaceCheckpoint(5, new Vector3(3059.000f, 4048.250f, 67.500f)),
            new VehicleRaceCheckpoint(6, new Vector3(2985.750f, 4244.500f, 55.719f)),
            new VehicleRaceCheckpoint(7, new Vector3(2869.500f, 4479.750f, 47.313f)),
            new VehicleRaceCheckpoint(8, new Vector3(2807.500f, 4515.000f, 45.781f)),
            new VehicleRaceCheckpoint(9, new Vector3(2774.000f, 4620.250f, 44.094f)),
            new VehicleRaceCheckpoint(10, new Vector3(2742.250f, 4759.750f, 43.719f)),
            new VehicleRaceCheckpoint(11, new Vector3(2746.250f, 4901.000f, 32.688f)),
            new VehicleRaceCheckpoint(12, new Vector3(2856.500f, 5011.250f, 31.188f)),
            new VehicleRaceCheckpoint(13, new Vector3(3082.000f, 5039.000f, 23.375f)),
            new VehicleRaceCheckpoint(14, new Vector3(3278.750f, 4996.250f, 22.375f)),
            new VehicleRaceCheckpoint(15, new Vector3(3415.750f, 4885.250f, 34.188f)),
            new VehicleRaceCheckpoint(16, new Vector3(3479.250f, 4679.250f, 52.313f)),
            new VehicleRaceCheckpoint(17, new Vector3(3500.750f, 4598.750f, 54.875f)),
        };
        VehicleRaceTrack grapep2ptwo = new VehicleRaceTrack("grapep2ptwo", "P2P - Humane Labs Escape", "Race from Humane Labs to the coast", grapep2ptwocheckpoints, grapep2ptwostart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(grapep2ptwo);

        List<VehicleRaceStartingPosition> grapeseedp2pstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(2497.450f, 4646.159f, 32.811f), 314.981f),
            new VehicleRaceStartingPosition(1, new Vector3(2495.329f, 4648.281f, 32.834f), 314.981f),
            new VehicleRaceStartingPosition(2, new Vector3(2490.376f, 4639.090f, 33.287f), 314.981f),
            new VehicleRaceStartingPosition(3, new Vector3(2488.256f, 4641.212f, 33.312f), 314.981f),
            new VehicleRaceStartingPosition(4, new Vector3(2483.303f, 4632.021f, 33.994f), 314.981f),
            new VehicleRaceStartingPosition(5, new Vector3(2481.182f, 4634.143f, 34.023f), 314.981f),
            new VehicleRaceStartingPosition(6, new Vector3(2476.229f, 4624.953f, 34.749f), 314.981f),
            new VehicleRaceStartingPosition(7, new Vector3(2474.109f, 4627.075f, 34.784f), 314.981f),
        };
        List<VehicleRaceCheckpoint> grapeseedp2pcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(2648.924f, 4803.567f, 33.013f)),
            new VehicleRaceCheckpoint(1, new Vector3(2827.213f, 4981.922f, 32.953f)),
            new VehicleRaceCheckpoint(2, new Vector3(3095.461f, 5038.721f, 23.078f)),
            new VehicleRaceCheckpoint(3, new Vector3(3282.581f, 4994.948f, 23.000f)),
            new VehicleRaceCheckpoint(4, new Vector3(3398.079f, 4901.997f, 35.348f)),
            new VehicleRaceCheckpoint(5, new Vector3(3471.987f, 4728.255f, 48.306f)),
            new VehicleRaceCheckpoint(6, new Vector3(3500.288f, 4600.400f, 55.245f)),
        };
        VehicleRaceTrack grapeseedp2p1 = new VehicleRaceTrack("grapeseed_p2p", "P2P - Grapeseed to Coast", "Race from Grapeseed to the coast", grapeseedp2pcheckpoints, grapeseedp2pstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(grapeseedp2p1);

        List<VehicleRaceStartingPosition> sandya2bstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(1137.213f, 3620.222f, 32.859f), 266.983f),
            new VehicleRaceStartingPosition(1, new Vector3(1137.371f, 3623.218f, 32.883f), 266.983f),
            new VehicleRaceStartingPosition(2, new Vector3(1127.227f, 3620.748f, 32.882f), 266.983f),
            new VehicleRaceStartingPosition(3, new Vector3(1127.385f, 3623.744f, 32.906f), 266.983f),
            new VehicleRaceStartingPosition(4, new Vector3(1117.241f, 3621.274f, 32.891f), 266.983f),
            new VehicleRaceStartingPosition(5, new Vector3(1117.399f, 3624.270f, 32.917f), 266.983f),
            new VehicleRaceStartingPosition(6, new Vector3(1107.255f, 3621.801f, 32.880f), 266.983f),
            new VehicleRaceStartingPosition(7, new Vector3(1107.413f, 3624.797f, 32.906f), 266.983f),
        };
        List<VehicleRaceCheckpoint> sandya2bcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(1261.129f, 3636.998f, 32.729f)),
            new VehicleRaceCheckpoint(1, new Vector3(1324.828f, 3603.537f, 33.572f)),
            new VehicleRaceCheckpoint(2, new Vector3(1427.933f, 3595.143f, 34.285f)),
            new VehicleRaceCheckpoint(3, new Vector3(1452.133f, 3665.516f, 33.584f)),
            new VehicleRaceCheckpoint(4, new Vector3(1506.223f, 3730.362f, 33.795f)),
            new VehicleRaceCheckpoint(5, new Vector3(1560.482f, 3734.897f, 33.830f)),
            new VehicleRaceCheckpoint(6, new Vector3(1630.478f, 3757.145f, 34.148f)),
            new VehicleRaceCheckpoint(7, new Vector3(1822.958f, 3881.880f, 33.120f)),
            new VehicleRaceCheckpoint(8, new Vector3(1876.595f, 3865.805f, 32.065f)),
            new VehicleRaceCheckpoint(9, new Vector3(1856.233f, 3825.328f, 31.882f)),
            new VehicleRaceCheckpoint(10, new Vector3(1638.251f, 3699.569f, 33.466f)),
            new VehicleRaceCheckpoint(11, new Vector3(1622.606f, 3640.641f, 34.450f)),
            new VehicleRaceCheckpoint(12, new Vector3(1677.355f, 3623.897f, 34.840f)),
            new VehicleRaceCheckpoint(13, new Vector3(1813.490f, 3701.239f, 33.340f)),
            new VehicleRaceCheckpoint(14, new Vector3(1970.599f, 3793.450f, 31.627f)),
            new VehicleRaceCheckpoint(15, new Vector3(2039.946f, 3833.268f, 35.314f)),
        };
        VehicleRaceTrack sandya2btrack1 = new VehicleRaceTrack("shadya2btrack1", "P2P - Sandy Shores Dash", "Race across Sandy Shores", sandya2bcheckpoints, sandya2bstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(sandya2btrack1);

        //Single checkpoint tracks P2P - R Conversion + Extras
        // You can go your own way
        // Sandy Shores Senora National Park Starting point

        // LOCATION 1: Observatory
        List<VehicleRaceStartingPosition> AlamoStart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(1636.797f, 3872.803f, 33.097f), 243.257f),
            new VehicleRaceStartingPosition(1, new Vector3(1638.822f, 3876.822f, 33.013f), 243.257f),
            new VehicleRaceStartingPosition(2, new Vector3(1629.206f, 3876.628f, 32.597f), 243.257f),
            new VehicleRaceStartingPosition(3, new Vector3(1631.231f, 3880.646f, 32.525f), 243.257f),
            new VehicleRaceStartingPosition(4, new Vector3(1621.615f, 3880.453f, 32.059f), 243.257f),
            new VehicleRaceStartingPosition(5, new Vector3(1623.640f, 3884.471f, 32.009f), 243.257f),
            new VehicleRaceStartingPosition(6, new Vector3(1614.025f, 3884.278f, 31.681f), 243.257f),
            new VehicleRaceStartingPosition(7, new Vector3(1616.050f, 3888.296f, 31.682f), 243.257f),
        };
        List<VehicleRaceCheckpoint> observatoryCheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-408.3208f, 1184.4412f, 324.5297f)),
        };
        VehicleRaceTrack sandyobservatory = new VehicleRaceTrack("sandyobservatorySprint", "Sprint - Observatory Sprint", "Race to the Observatory", observatoryCheckpoints, AlamoStart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(sandyobservatory);

        // LOCATION 2: Kortz Center
        List<VehicleRaceCheckpoint> kortzCheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-2297.7310f, 379.1689f, 173.4667f)),
        };
        VehicleRaceTrack sandykortz = new VehicleRaceTrack("sandykortzSprint", "Sprint - Kortz Center Sprint", "Race to the Kortz Center", kortzCheckpoints, AlamoStart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(sandykortz);

        // LOCATION 3: Stab City
        List<VehicleRaceCheckpoint> stabCityCheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(54.1055f, 3734.0286f, 38.7018f)),
        };
        VehicleRaceTrack sandystabCity = new VehicleRaceTrack("sandystabCitySprint", "Sprint - Stab City Sprint", "Race to Stab City", stabCityCheckpoints, AlamoStart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(sandystabCity);

        // LOCATION 4: Vinewood Sign
        List<VehicleRaceCheckpoint> vinewoodCheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(744.1607f, 1199.4257f, 325.4021f)),
        };
        VehicleRaceTrack sandyvinewood = new VehicleRaceTrack("sandyvinewoodSprint", "Sprint - Vinewood Sign Sprint", "Race to the Vinewood Sign", vinewoodCheckpoints, AlamoStart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(sandyvinewood);

        // LOCATION 5: Elysian Island
        List<VehicleRaceCheckpoint> elysianCheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(231.6537f, -3326.9919f, 4.7973f)),
        };
        VehicleRaceTrack sandyelysian = new VehicleRaceTrack("sandyelysianSprint", "Sprint - Elysian Island Sprint", "Race to Elysian Island", elysianCheckpoints, AlamoStart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(sandyelysian);

        // LOCATION 6: Power Station
        List<VehicleRaceCheckpoint> powerStationCheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(2791.8086f, 1518.2588f, 23.5166f)),
        };
        VehicleRaceTrack sandypowerStation = new VehicleRaceTrack("sandypowerStationSprint", "Sprint - Power Station Sprint", "Race to the Power Station", powerStationCheckpoints, AlamoStart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(sandypowerStation);

        // LOCATION 7: LSIA
        List<VehicleRaceCheckpoint> lsiaCheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-1027.1440f, -2714.6970f, 12.8181f)),
        };
        VehicleRaceTrack sandylsia = new VehicleRaceTrack("sandylsiaSprint", "Sprint - LSIA Sprint", "Race to LSIA", lsiaCheckpoints, AlamoStart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(sandylsia);

        // LCATION 8: Maze Bank Arena
        List<VehicleRaceCheckpoint> mazeBankCheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-189.250f, -2005.000f, 26.625f)),
        };
        VehicleRaceTrack sandymazeBank = new VehicleRaceTrack("sandymazeBankSprint", "Sprint - Maze Bank Arena Sprint", "Race to Maze Bank Arena", mazeBankCheckpoints, AlamoStart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(sandymazeBank);

        // LCATION 9: Puerto del Sol Marina 
        List<VehicleRaceCheckpoint> puertoDelSolCheckpoints = new List<VehicleRaceCheckpoint>()
        {
           new VehicleRaceCheckpoint(0, new Vector3(-766.750f, -1295.500f, 4.000f)),
        };
        VehicleRaceTrack sandyPuertoDelSol = new VehicleRaceTrack("sandyPuertoDelSolSprint", "Sprint - Puerto del Sol Marina Sprint", "Race to Puerto del Sol Marina", puertoDelSolCheckpoints, AlamoStart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(sandyPuertoDelSol);

        // lOCATION 10: Del Perro Pier
        List<VehicleRaceCheckpoint> delPerroCheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-1600.500f, -949.000f, 12.000f))
        };
        VehicleRaceTrack sandyDelPerro = new VehicleRaceTrack("sandyDelPerroSprint", "Sprint - Del Perro Pier Sprint", "Race to Del Perro Pier", delPerroCheckpoints, AlamoStart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(sandyDelPerro);

        // "sandyobservatorySprint", "sandykortzSprint", "sandystabCitySprint", "sandyvinewoodSprint", "sandyelysianSprint", "sandypowerStationSprint", "sandylsiaSprint", "sandymazeBankSprint", "sandyPuertoDelSolSprint", "sandyDelPerroSprint"
    } 

    private void PaletoTracks()
    {
        // Circuit

        List<VehicleRaceStartingPosition> paletoLoop1Starting = new List<VehicleRaceStartingPosition>()
        {
            //new VehicleRaceStartingPosition(0,new Vector3(218.0589f, 6565.234f, 31.52061f), 106.4273f),
            //new VehicleRaceStartingPosition(1,new Vector3(205.2844f, 6560.81f, 31.64036f), 109.7968f),
            //new VehicleRaceStartingPosition(2,new Vector3(190.9381f, 6561.395f, 31.71398f), 110.7484f),
            //new VehicleRaceStartingPosition(3,new Vector3(182.8164f, 6552.707f, 31.65496f), 120.4496f),


            new VehicleRaceStartingPosition(0, new Vector3(159.637f, 6540.995f, 30.838f), 127.006f),
            new VehicleRaceStartingPosition(1, new Vector3(161.443f, 6538.600f, 30.811f), 127.006f),
            new VehicleRaceStartingPosition(2, new Vector3(167.623f, 6547.014f, 30.906f), 127.006f),
            new VehicleRaceStartingPosition(3, new Vector3(169.428f, 6544.618f, 30.889f), 127.006f),
            new VehicleRaceStartingPosition(4, new Vector3(175.608f, 6553.033f, 30.977f), 127.006f),
            new VehicleRaceStartingPosition(5, new Vector3(177.414f, 6550.637f, 30.958f), 127.006f),
            new VehicleRaceStartingPosition(6, new Vector3(183.594f, 6559.052f, 31.027f), 127.006f),
            new VehicleRaceStartingPosition(7, new Vector3(185.400f, 6556.656f, 31.011f), 127.006f),

        };
        List<VehicleRaceCheckpoint> paletoLoop1Checkpoints = new List<VehicleRaceCheckpoint>()
        {
            //old
            //new VehicleRaceCheckpoint(0,new Vector3(143.2037f, 6526.385f, 31.3439f)),
            //new VehicleRaceCheckpoint(1,new Vector3(-202.2245f, 6173.541f, 30.57405f)),
            //new VehicleRaceCheckpoint(2,new Vector3(-181.1622f, 6468.749f, 30.21145f)),
            //new VehicleRaceCheckpoint(3,new Vector3(150.0628f, 6533.176f, 31.4289f)),

            // Already commentedout below
            //new VehicleRaceCheckpoint(2,new Vector3(-341.7607f, 6269.313f, 30.86405f)),
            //new VehicleRaceCheckpoint(3,new Vector3(-358.0514f, 6293.811f, 29.61264f)),
            //2 - 3
            //new VehicleRaceCheckpoint(0,new Vector3(143.2037f, 6526.385f, 31.3439f)),
            //new VehicleRaceCheckpoint(1,new Vector3(-215.3243f, 6169.238f, 30.88095f)),
            //new VehicleRaceCheckpoint(2,new Vector3(-294.1931f, 6220.725f, 31.19704f)),
            //new VehicleRaceCheckpoint(3,new Vector3(-358.0514f, 6293.811f, 29.61264f)),
            //new VehicleRaceCheckpoint(4,new Vector3(-181.1622f, 6468.749f, 30.21145f)),
            //new VehicleRaceCheckpoint(5,new Vector3(150.0628f, 6533.176f, 31.4289f)),

            new VehicleRaceCheckpoint(0, new Vector3(91.137f, 6473.549f, 30.921f)),
            new VehicleRaceCheckpoint(1, new Vector3(-187.700f, 6194.844f, 30.755f)),
            new VehicleRaceCheckpoint(2, new Vector3(-255.678f, 6182.249f, 31.006f)),
            new VehicleRaceCheckpoint(3, new Vector3(-332.893f, 6259.959f, 31.136f)),
            new VehicleRaceCheckpoint(4, new Vector3(-334.125f, 6333.259f, 29.752f)),
            new VehicleRaceCheckpoint(5, new Vector3(-77.900f, 6583.973f, 29.158f)),
            new VehicleRaceCheckpoint(6, new Vector3(112.571f, 6569.387f, 31.184f)),

        };
        VehicleRaceTrack paletoLoop1 = new VehicleRaceTrack("paletoloop1", "Circuit - Paleto Bay Loop", "Race around Paleto Bay", paletoLoop1Checkpoints, paletoLoop1Starting);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(paletoLoop1);

        List<VehicleRaceStartingPosition> paletoLoop2Starting = new List<VehicleRaceStartingPosition>()
        {
            //new VehicleRaceStartingPosition(0,new Vector3(218.0589f, 6565.234f, 31.52061f), 106.4273f),
            //new VehicleRaceStartingPosition(1,new Vector3(205.2844f, 6560.81f, 31.64036f), 109.7968f),
            //new VehicleRaceStartingPosition(2,new Vector3(190.9381f, 6561.395f, 31.71398f), 110.7484f),
            //new VehicleRaceStartingPosition(3,new Vector3(182.8164f, 6552.707f, 31.65496f), 120.4496f),


            new VehicleRaceStartingPosition(0, new Vector3(246.731f, 6574.009f, 30.265f), 99.718f),
            new VehicleRaceStartingPosition(1, new Vector3(247.238f, 6571.052f, 30.229f), 99.718f),
            new VehicleRaceStartingPosition(2, new Vector3(256.588f, 6575.697f, 29.980f), 99.718f),
            new VehicleRaceStartingPosition(3, new Vector3(257.094f, 6572.740f, 29.943f), 99.718f),
            new VehicleRaceStartingPosition(4, new Vector3(266.444f, 6577.385f, 29.683f), 99.718f),
            new VehicleRaceStartingPosition(5, new Vector3(266.951f, 6574.428f, 29.654f), 99.718f),
            new VehicleRaceStartingPosition(6, new Vector3(276.301f, 6579.073f, 29.386f), 99.718f),
            new VehicleRaceStartingPosition(7, new Vector3(276.807f, 6576.117f, 29.369f), 99.718f),

        };
        List<VehicleRaceCheckpoint> paletoLoop2Checkpoints = new List<VehicleRaceCheckpoint>()
        {
            //old
            //new VehicleRaceCheckpoint(0,new Vector3(141.9506f, 6543.649f, 31.27871f)),
            //new VehicleRaceCheckpoint(1,new Vector3(87.16878f, 6598.012f, 31.2261f)),
            //new VehicleRaceCheckpoint(2,new Vector3(-290.8184f, 6247.322f, 31.10103f)),
            //new VehicleRaceCheckpoint(3,new Vector3(-237.6497f, 6163.285f, 31.16849f)),
            //new VehicleRaceCheckpoint(4,new Vector3(152.6836f, 6522.836f, 31.30534f)),


            new VehicleRaceCheckpoint(0, new Vector3(100.584f, 6581.964f, 31.184f)),
            new VehicleRaceCheckpoint(1, new Vector3(33.868f, 6569.206f, 31.000f)),
            new VehicleRaceCheckpoint(2, new Vector3(-254.884f, 6279.208f, 31.050f)),
            new VehicleRaceCheckpoint(3, new Vector3(-262.779f, 6189.532f, 31.016f)),
            new VehicleRaceCheckpoint(4, new Vector3(-182.802f, 6189.950f, 30.752f)),
            new VehicleRaceCheckpoint(5, new Vector3(75.396f, 6445.447f, 30.879f)),
        };
        VehicleRaceTrack paletoLoop2 = new VehicleRaceTrack("paletoloop2", "Circuit - Paleto Bay Loop (Reverse)", "Race around Paleto Bay", paletoLoop2Checkpoints, paletoLoop2Starting);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(paletoLoop2);

        // Drag
        List<VehicleRaceStartingPosition> paletoquarterstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-14.023f, 6526.995f, 30.314f), 135.327f),
            new VehicleRaceStartingPosition(1, new Vector3(-12.245f, 6525.237f, 30.416f), 135.327f),
            new VehicleRaceStartingPosition(2, new Vector3(-7.691f, 6533.390f, 30.334f), 135.327f),
            new VehicleRaceStartingPosition(3, new Vector3(-5.913f, 6531.632f, 30.415f), 135.327f),
            new VehicleRaceStartingPosition(4, new Vector3(-1.358f, 6539.786f, 30.329f), 135.327f),
            new VehicleRaceStartingPosition(5, new Vector3(0.419f, 6538.028f, 30.415f), 135.327f),
            new VehicleRaceStartingPosition(6, new Vector3(4.974f, 6546.181f, 30.323f), 135.327f),
            new VehicleRaceStartingPosition(7, new Vector3(6.751f, 6544.423f, 30.414f), 135.327f),
        };
        List<VehicleRaceCheckpoint> paletoquartercheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-295.250f, 6239.250f, 30.375f)),
        };
        VehicleRaceTrack paletoquarter = new VehicleRaceTrack("paletoquarter", "Drag - Paleto Quarter Mile", "Quarter Mile Drag Race", paletoquartercheckpoints, paletoquarterstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(paletoquarter);

        List<VehicleRaceStartingPosition> paletoDrag1Starting = new List<VehicleRaceStartingPosition>()
        {
            //new VehicleRaceStartingPosition(0,new Vector3(218.0589f, 6565.234f, 31.52061f), 106.4273f),
            //new VehicleRaceStartingPosition(1,new Vector3(205.2844f, 6560.81f, 31.64036f), 109.7968f),
            //new VehicleRaceStartingPosition(2,new Vector3(190.9381f, 6561.395f, 31.71398f), 110.7484f),
            //new VehicleRaceStartingPosition(3,new Vector3(182.8164f, 6552.707f, 31.65496f), 120.4496f),

            new VehicleRaceStartingPosition(0, new Vector3(1679.603f, 6390.540f, 29.850f), 76.240f),
            new VehicleRaceStartingPosition(1, new Vector3(1678.294f, 6385.197f, 29.857f), 76.240f),
            new VehicleRaceStartingPosition(2, new Vector3(1689.316f, 6388.161f, 30.507f), 76.240f),
            new VehicleRaceStartingPosition(3, new Vector3(1688.007f, 6382.819f, 30.514f), 76.240f),
            new VehicleRaceStartingPosition(4, new Vector3(1699.029f, 6385.782f, 31.218f), 76.240f),
            new VehicleRaceStartingPosition(5, new Vector3(1697.720f, 6380.440f, 31.222f), 76.240f),
            new VehicleRaceStartingPosition(6, new Vector3(1708.742f, 6383.404f, 31.983f), 76.240f),
            new VehicleRaceStartingPosition(7, new Vector3(1707.433f, 6378.062f, 31.982f), 76.240f),

        };
        List<VehicleRaceCheckpoint> paletoDrag1Checkpoints = new List<VehicleRaceCheckpoint>()
        {
            //new VehicleRaceCheckpoint(0,new Vector3(143.2037f, 6526.385f, 31.3439f)),
            //new VehicleRaceCheckpoint(1, new Vector3(-769.4749f, 5497.793f, 34.48269f)),

            new VehicleRaceCheckpoint(0, new Vector3(1537.639f, 6439.318f, 23.158f)),
            new VehicleRaceCheckpoint(1, new Vector3(1160.191f, 6492.126f, 20.605f)),
            new VehicleRaceCheckpoint(2, new Vector3(615.282f, 6536.960f, 27.776f)),
            new VehicleRaceCheckpoint(3, new Vector3(89.228f, 6471.545f, 30.907f)),
            new VehicleRaceCheckpoint(4, new Vector3(-455.263f, 5909.874f, 32.377f)),
            new VehicleRaceCheckpoint(5, new Vector3(-621.533f, 5602.841f, 38.632f)),
            new VehicleRaceCheckpoint(6, new Vector3(-805.678f, 5475.634f, 33.488f)),
        };
        VehicleRaceTrack paletoDrag1 = new VehicleRaceTrack("paletodrag1", "Drag - Paleto Bay", "Drag race across Paleto Bay", paletoDrag1Checkpoints, paletoDrag1Starting);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(paletoDrag1);


        //P2P

        List<VehicleRaceStartingPosition> hookiespipelinestart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-2269.725f, 4244.181f, 42.840f), 145.789f),
            new VehicleRaceStartingPosition(1, new Vector3(-2267.244f, 4242.494f, 42.819f), 145.789f),
            new VehicleRaceStartingPosition(2, new Vector3(-2265.227f, 4250.796f, 43.394f), 145.789f),
            new VehicleRaceStartingPosition(3, new Vector3(-2262.747f, 4249.110f, 43.391f), 145.789f),
            new VehicleRaceStartingPosition(4, new Vector3(-2260.729f, 4257.412f, 43.830f), 145.789f),
            new VehicleRaceStartingPosition(5, new Vector3(-2258.249f, 4255.726f, 43.825f), 145.789f),
            new VehicleRaceStartingPosition(6, new Vector3(-2256.231f, 4264.028f, 44.395f), 145.789f),
            new VehicleRaceStartingPosition(7, new Vector3(-2253.750f, 4262.341f, 44.367f), 145.789f),
        };
        List<VehicleRaceCheckpoint> hookiespipelinecheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-2451.250f, 3792.000f, 19.500f)),
            new VehicleRaceCheckpoint(1, new Vector3(-2591.500f, 3126.000f, 13.844f)),
            new VehicleRaceCheckpoint(2, new Vector3(-2662.000f, 2609.000f, 15.656f)),
            new VehicleRaceCheckpoint(3, new Vector3(-2915.500f, 2153.500f, 38.219f)),
            new VehicleRaceCheckpoint(4, new Vector3(-3080.250f, 1348.500f, 19.219f)),
            new VehicleRaceCheckpoint(5, new Vector3(-3121.250f, 823.750f, 16.063f)),
            new VehicleRaceCheckpoint(6, new Vector3(-3024.250f, 271.750f, 14.594f)),
            new VehicleRaceCheckpoint(7, new Vector3(-2618.000f, -122.500f, 19.250f)),
            new VehicleRaceCheckpoint(8, new Vector3(-2208.000f, -352.750f, 12.281f)),
        };
        VehicleRaceTrack hookiespipeline = new VehicleRaceTrack("hookiespipeline", "P2P - Hookies Pipeline", "Hookies Bar to the Pipleine Inn", hookiespipelinecheckpoints, hookiespipelinestart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(hookiespipeline);

        //List<VehicleRaceStartingPosition> paletop2pstart = new List<VehicleRaceStartingPosition>()
        //{
        //    new VehicleRaceStartingPosition(0, new Vector3(1423.600f, 6505.159f, 18.710f), 84.947f),
        //    new VehicleRaceStartingPosition(1, new Vector3(1423.865f, 6508.147f, 18.636f), 84.947f),
        //    new VehicleRaceStartingPosition(2, new Vector3(1433.562f, 6504.278f, 18.860f), 84.947f),
        //    new VehicleRaceStartingPosition(3, new Vector3(1433.826f, 6507.266f, 18.776f), 84.947f),
        //    new VehicleRaceStartingPosition(4, new Vector3(1443.523f, 6503.397f, 19.040f), 84.947f),
        //    new VehicleRaceStartingPosition(5, new Vector3(1443.787f, 6506.385f, 18.980f), 84.947f),
        //    new VehicleRaceStartingPosition(6, new Vector3(1453.484f, 6502.517f, 19.318f), 84.947f),
        //    new VehicleRaceStartingPosition(7, new Vector3(1453.748f, 6505.505f, 19.260f), 84.947f),
        //};
        List<VehicleRaceCheckpoint> paletop2pcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(1223.365f, 6493.224f, 19.831f)),
            new VehicleRaceCheckpoint(1, new Vector3(973.938f, 6492.054f, 20.000f)),
            new VehicleRaceCheckpoint(2, new Vector3(750.272f, 6508.311f, 25.220f)),
            new VehicleRaceCheckpoint(3, new Vector3(555.985f, 6550.128f, 26.795f)),
            new VehicleRaceCheckpoint(4, new Vector3(313.108f, 6579.061f, 28.337f)),
            new VehicleRaceCheckpoint(5, new Vector3(90.184f, 6477.026f, 30.363f)),
            new VehicleRaceCheckpoint(6, new Vector3(-356.500f, 6036.000f, 30.219f)),
            new VehicleRaceCheckpoint(7, new Vector3(-431.000f, 6066.250f, 30.469f)),
            new VehicleRaceCheckpoint(8, new Vector3(-279.250f, 6255.500f, 30.469f)),
            new VehicleRaceCheckpoint(9, new Vector3(-97.750f, 6433.250f, 30.469f)),
            new VehicleRaceCheckpoint(10, new Vector3(32.750f, 6567.750f, 30.438f)),
            new VehicleRaceCheckpoint(11, new Vector3(46.750f, 6635.000f, 30.625f)),
            new VehicleRaceCheckpoint(12, new Vector3(-66.250f, 6592.750f, 28.625f)),
            new VehicleRaceCheckpoint(13, new Vector3(-195.500f, 6445.750f, 30.250f)),
            new VehicleRaceCheckpoint(14, new Vector3(-330.000f, 6337.750f, 29.250f)),
            new VehicleRaceCheckpoint(15, new Vector3(-412.500f, 6224.250f, 30.438f)),
            new VehicleRaceCheckpoint(16, new Vector3(-408.500f, 6128.250f, 30.438f)),
            new VehicleRaceCheckpoint(17, new Vector3(-416.250f, 6019.000f, 30.438f)),
            new VehicleRaceCheckpoint(18, new Vector3(-501.547f, 5848.522f, 33.065f)),
            new VehicleRaceCheckpoint(19, new Vector3(-597.994f, 5642.833f, 37.704f)),
            new VehicleRaceCheckpoint(20, new Vector3(-724.229f, 5530.399f, 35.772f)),
            new VehicleRaceCheckpoint(21, new Vector3(-792.250f, 5545.000f, 32.344f)),
            new VehicleRaceCheckpoint(22, new Vector3(-747.000f, 5728.000f, 18.875f)),
            new VehicleRaceCheckpoint(23, new Vector3(-675.000f, 5936.000f, 15.000f)),
            new VehicleRaceCheckpoint(24, new Vector3(-597.750f, 6106.750f, 6.750f)),
            new VehicleRaceCheckpoint(25, new Vector3(-521.750f, 6277.500f, 8.875f)),
            new VehicleRaceCheckpoint(26, new Vector3(-393.500f, 6388.750f, 13.063f)),
            new VehicleRaceCheckpoint(27, new Vector3(-221.250f, 6517.000f, 10.031f)),
        };
        VehicleRaceTrack paletop2p = new VehicleRaceTrack("paletop2p", "P2P - Paleto Rush", "Race through Paleto Bay", paletop2pcheckpoints, paletoDrag1Starting);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(paletop2p);

        // Single checkpoint tracks P2P - R Conversion + Extras
        // Go your own way
        // Procopio Promenade - Paleto  Starting Area

        // LOCATION 1: Observatory
        List<VehicleRaceStartingPosition> PromenadStart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-245.116f, 6495.623f, 10.250f), 135.294f),
            new VehicleRaceStartingPosition(1, new Vector3(-242.984f, 6493.512f, 10.270f), 135.294f),
            new VehicleRaceStartingPosition(2, new Vector3(-239.488f, 6501.308f, 10.116f), 135.294f),
            new VehicleRaceStartingPosition(3, new Vector3(-237.356f, 6499.198f, 10.227f), 135.294f),
            new VehicleRaceStartingPosition(4, new Vector3(-233.860f, 6506.994f, 10.011f), 135.294f),
            new VehicleRaceStartingPosition(5, new Vector3(-231.728f, 6504.884f, 10.101f), 135.294f),
            new VehicleRaceStartingPosition(6, new Vector3(-228.232f, 6512.680f, 9.993f), 135.294f),
            new VehicleRaceStartingPosition(7, new Vector3(-226.100f, 6510.569f, 10.143f), 135.294f),
        };
        List<VehicleRaceCheckpoint> observatoryCheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-408.3208f, 1184.4412f, 324.5297f)),
        };
        VehicleRaceTrack paletoobservatory = new VehicleRaceTrack("paletoobservatorySprint", "Sprint - Observatory Sprint", "Race to the Observatory", observatoryCheckpoints, PromenadStart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(paletoobservatory);

        // LOCATION 2: Kortz Center
        List<VehicleRaceCheckpoint> kortzCheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-2297.7310f, 379.1689f, 173.4667f)),
        };
        VehicleRaceTrack paletokortz = new VehicleRaceTrack("paletokortzSprint", "Sprint - Kortz Center Sprint", "Race to the Kortz Center", kortzCheckpoints, PromenadStart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(paletokortz);

        // LOCATION 3: Stab City
        List<VehicleRaceCheckpoint> stabCityCheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(54.1055f, 3734.0286f, 38.7018f)),
        };
        VehicleRaceTrack paletostabCity = new VehicleRaceTrack("paletostabCitySprint", "Sprint - Stab City Sprint", "Race to Stab City", stabCityCheckpoints, PromenadStart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(paletostabCity);

        // LOCATION 4: Vinewood Sign
        List<VehicleRaceCheckpoint> vinewoodCheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(744.1607f, 1199.4257f, 325.4021f)),
        };
        VehicleRaceTrack paletovinewood = new VehicleRaceTrack("paletovinewoodSprint", "Sprint - Vinewood Sign Sprint", "Race to the Vinewood Sign", vinewoodCheckpoints, PromenadStart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(paletovinewood);

        // LOCATION 5: Elysian Island
        List<VehicleRaceCheckpoint> elysianCheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(231.6537f, -3326.9919f, 4.7973f)),
        };
        VehicleRaceTrack paletoelysian = new VehicleRaceTrack("paletoelysianSprint", "Sprint - Elysian Island Sprint", "Race to Elysian Island", elysianCheckpoints, PromenadStart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(paletoelysian);

        // LOCATION 6: Power Station
        List<VehicleRaceCheckpoint> powerStationCheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(2791.8086f, 1518.2588f, 23.5166f)),
        };
        VehicleRaceTrack paletopowerStation = new VehicleRaceTrack("paletopowerStationSprint", "Sprint - Power Station Sprint", "Race to the Power Station", powerStationCheckpoints, PromenadStart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(paletopowerStation);

        // LOCATION 7: LSIA
        List<VehicleRaceCheckpoint> lsiaCheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-1027.1440f, -2714.6970f, 12.8181f)),
        };
        VehicleRaceTrack paletolsia = new VehicleRaceTrack("paletolsiaSprint", "Sprint - LSIA Sprint", "Race to LSIA", lsiaCheckpoints, PromenadStart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(paletolsia);

        // LCATION 8: Maze Bank Arena
        List<VehicleRaceCheckpoint> mazeBankCheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-189.250f, -2005.000f, 26.625f)),
        };
        VehicleRaceTrack paletomazeBank = new VehicleRaceTrack("paletomazeBankSprint", "Sprint - Maze Bank Arena Sprint", "Race to Maze Bank Arena", mazeBankCheckpoints, PromenadStart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(paletomazeBank);

        // LCATION 9: Puerto del Sol Marina 
        List<VehicleRaceCheckpoint> puertoDelSolCheckpoints = new List<VehicleRaceCheckpoint>()
        {
           new VehicleRaceCheckpoint(0, new Vector3(-766.750f, -1295.500f, 4.000f)),
        };
        VehicleRaceTrack paletoPuertoDelSol = new VehicleRaceTrack("paletoPuertoDelSolSprint", "Sprint - Puerto del Sol Marina Sprint", "Race to Puerto del Sol Marina", puertoDelSolCheckpoints, PromenadStart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(paletoPuertoDelSol);

        // lOCATION 10: Del Perro Pier
        List<VehicleRaceCheckpoint> delPerroCheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-1600.500f, -949.000f, 12.000f))
        };
        VehicleRaceTrack paletoDelPerro = new VehicleRaceTrack("paletoDelPerroSprint", "Sprint - Del Perro Pier Sprint", "Race to Del Perro Pier", delPerroCheckpoints, PromenadStart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(paletoDelPerro);

        // "paletoobservatorySprint", "paletokortzSprint", "paletostabCitySprint", "paletovinewoodSprint", "paletoelysianSprint", "paletopowerStationSprint", "paletolsiaSprint", "paletomazeBankSprint", "paletoPuertoDelSolSprint" ,"paletoDelPerroSprint"
    }

    private void VineWoodTracks()
    {
        // Circuit

        List<VehicleRaceStartingPosition> casinocorsestart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(1110.790f, 189.645f, 80.923f), 148.327f),
            new VehicleRaceStartingPosition(1, new Vector3(1113.768f, 187.807f, 80.843f), 148.327f),
            new VehicleRaceStartingPosition(2, new Vector3(1116.041f, 198.156f, 80.924f), 148.327f),
            new VehicleRaceStartingPosition(3, new Vector3(1119.019f, 196.318f, 80.842f), 148.327f),
            new VehicleRaceStartingPosition(4, new Vector3(1121.291f, 206.666f, 80.925f), 148.327f),
            new VehicleRaceStartingPosition(5, new Vector3(1124.270f, 204.828f, 80.841f), 148.327f),
            new VehicleRaceStartingPosition(6, new Vector3(1126.542f, 215.177f, 80.927f), 148.327f),
            new VehicleRaceStartingPosition(7, new Vector3(1129.521f, 213.339f, 80.843f), 148.327f),
        };
        List<VehicleRaceCheckpoint> casinocorsecheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(1129.726f, -30.558f, 81.334f)),
            new VehicleRaceCheckpoint(1, new Vector3(1090.827f, 151.854f, 81.339f)),
        };
        VehicleRaceTrack casinocorse = new VehicleRaceTrack("casino_course", "Circuit -  Casino Track", "Race Around the Casino Track", casinocorsecheckpoints, casinocorsestart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(casinocorse); // disappointing...

        List<VehicleRaceStartingPosition> citycircuit1start = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(278.314f, 338.450f, 104.385f), 75.886f),
            new VehicleRaceStartingPosition(1, new Vector3(277.583f, 335.540f, 104.594f), 75.886f),
            new VehicleRaceStartingPosition(2, new Vector3(286.073f, 336.499f, 104.483f), 75.886f),
            new VehicleRaceStartingPosition(3, new Vector3(285.341f, 333.589f, 104.627f), 75.886f),
            new VehicleRaceStartingPosition(4, new Vector3(293.831f, 334.548f, 104.435f), 75.886f),
            new VehicleRaceStartingPosition(5, new Vector3(293.100f, 331.639f, 104.653f), 75.886f),
            new VehicleRaceStartingPosition(6, new Vector3(301.590f, 332.597f, 104.461f), 75.886f),
            new VehicleRaceStartingPosition(7, new Vector3(300.858f, 329.688f, 104.597f), 75.886f),
        };
        List<VehicleRaceCheckpoint> citycircuit1checkpoints = new List<VehicleRaceCheckpoint>()
        {

            //new VehicleRaceCheckpoint(0, new Vector3(177.666f, 368.222f, 108.106f)),
            //new VehicleRaceCheckpoint(1, new Vector3(73.012f, 326.216f, 111.538f)),
            //new VehicleRaceCheckpoint(2, new Vector3(41.919f, 280.842f, 109.091f)),
            //new VehicleRaceCheckpoint(3, new Vector3(-4.735f, 269.514f, 108.254f)),
            //new VehicleRaceCheckpoint(4, new Vector3(-157.621f, 261.035f, 93.293f)),
            //new VehicleRaceCheckpoint(5, new Vector3(-303.634f, 264.867f, 87.455f)),
            //new VehicleRaceCheckpoint(6, new Vector3(-481.751f, 255.939f, 82.439f)),
            //new VehicleRaceCheckpoint(7, new Vector3(-692.408f, 288.121f, 82.255f)),
            //new VehicleRaceCheckpoint(8, new Vector3(-827.445f, 291.315f, 85.761f)),
            //new VehicleRaceCheckpoint(9, new Vector3(-854.526f, 468.639f, 86.773f)),
            //new VehicleRaceCheckpoint(10, new Vector3(-879.638f, 538.347f, 91.332f)),
            //new VehicleRaceCheckpoint(11, new Vector3(-1005.900f, 596.591f, 102.200f)),
            //new VehicleRaceCheckpoint(12, new Vector3(-1207.925f, 547.502f, 97.329f)),
            //new VehicleRaceCheckpoint(13, new Vector3(-1247.867f, 489.762f, 93.411f)),
            //new VehicleRaceCheckpoint(14, new Vector3(-1208.542f, 469.339f, 89.148f)),
            //new VehicleRaceCheckpoint(15, new Vector3(-1099.988f, 457.192f, 77.626f)),
            //new VehicleRaceCheckpoint(16, new Vector3(-1077.077f, 380.638f, 68.359f)),
            //new VehicleRaceCheckpoint(17, new Vector3(-1078.983f, 318.492f, 65.264f)),
            //new VehicleRaceCheckpoint(18, new Vector3(-1118.803f, 270.975f, 64.869f)),
            //new VehicleRaceCheckpoint(19, new Vector3(-1277.795f, 228.833f, 59.851f)),
            //new VehicleRaceCheckpoint(20, new Vector3(-1425.542f, 172.818f, 55.958f)),
            //new VehicleRaceCheckpoint(21, new Vector3(-1433.139f, -22.658f, 51.802f)),
            //new VehicleRaceCheckpoint(22, new Vector3(-1471.993f, -96.275f, 50.273f)),
            //new VehicleRaceCheckpoint(23, new Vector3(-1596.088f, -201.221f, 54.537f)),
            //new VehicleRaceCheckpoint(24, new Vector3(-1638.350f, -310.050f, 50.433f)),
            //new VehicleRaceCheckpoint(25, new Vector3(-1726.750f, -392.000f, 44.813f)),
            //new VehicleRaceCheckpoint(26, new Vector3(-1752.232f, -433.623f, 42.174f)),
            //new VehicleRaceCheckpoint(27, new Vector3(-1678.369f, -568.024f, 33.843f)),
            //new VehicleRaceCheckpoint(28, new Vector3(-1559.142f, -660.692f, 28.405f)),
            //new VehicleRaceCheckpoint(29, new Vector3(-1442.120f, -760.598f, 22.916f)),
            //new VehicleRaceCheckpoint(30, new Vector3(-1332.496f, -855.945f, 16.256f)),
            //new VehicleRaceCheckpoint(31, new Vector3(-1263.815f, -1030.615f, 8.362f)),
            //new VehicleRaceCheckpoint(32, new Vector3(-1286.536f, -1076.801f, 6.757f)),
            //new VehicleRaceCheckpoint(33, new Vector3(-1302.682f, -1149.200f, 4.972f)),
            //new VehicleRaceCheckpoint(34, new Vector3(-1280.243f, -1234.415f, 3.813f)),
            //new VehicleRaceCheckpoint(35, new Vector3(-1227.298f, -1258.795f, 5.712f)),
            //new VehicleRaceCheckpoint(36, new Vector3(-1154.211f, -1301.339f, 4.526f)),
            //new VehicleRaceCheckpoint(37, new Vector3(-935.568f, -1223.151f, 4.570f)),
            //new VehicleRaceCheckpoint(38, new Vector3(-818.984f, -1153.104f, 7.257f)),
            //new VehicleRaceCheckpoint(39, new Vector3(-654.068f, -1047.704f, 16.542f)),
            //new VehicleRaceCheckpoint(40, new Vector3(-634.776f, -895.517f, 24.238f)),
            //new VehicleRaceCheckpoint(41, new Vector3(-632.493f, -736.958f, 26.875f)),
            //new VehicleRaceCheckpoint(42, new Vector3(-626.991f, -583.328f, 33.988f)),
            //new VehicleRaceCheckpoint(43, new Vector3(-624.718f, -421.252f, 34.185f)),
            //new VehicleRaceCheckpoint(44, new Vector3(-579.061f, -379.510f, 34.248f)),
            //new VehicleRaceCheckpoint(45, new Vector3(-438.692f, -389.104f, 32.542f)),
            //new VehicleRaceCheckpoint(46, new Vector3(-232.339f, -410.861f, 29.943f)),
            //new VehicleRaceCheckpoint(47, new Vector3(-145.706f, -365.493f, 33.504f)),
            //new VehicleRaceCheckpoint(48, new Vector3(-110.976f, -280.200f, 41.426f)),
            //new VehicleRaceCheckpoint(49, new Vector3(-43.532f, -260.911f, 45.401f)),
            //new VehicleRaceCheckpoint(50, new Vector3(82.776f, -308.408f, 45.920f)),
            //new VehicleRaceCheckpoint(51, new Vector3(219.578f, -356.824f, 43.608f)),
            //new VehicleRaceCheckpoint(52, new Vector3(363.047f, -402.764f, 45.047f)),
            //new VehicleRaceCheckpoint(53, new Vector3(497.878f, -283.225f, 46.153f)),
            //new VehicleRaceCheckpoint(54, new Vector3(523.839f, -204.869f, 51.339f)),
            //new VehicleRaceCheckpoint(55, new Vector3(549.497f, -97.168f, 65.414f)),
            //new VehicleRaceCheckpoint(56, new Vector3(644.645f, -54.005f, 77.036f)),
            //new VehicleRaceCheckpoint(57, new Vector3(713.584f, 33.030f, 83.585f)),
            //new VehicleRaceCheckpoint(58, new Vector3(766.244f, 107.410f, 78.279f)),
            //new VehicleRaceCheckpoint(59, new Vector3(764.742f, 155.118f, 80.326f)),
            //new VehicleRaceCheckpoint(60, new Vector3(732.685f, 190.893f, 84.875f)),
            //new VehicleRaceCheckpoint(61, new Vector3(607.294f, 234.148f, 101.706f)),
            //new VehicleRaceCheckpoint(62, new Vector3(371.631f, 311.801f, 102.875f)),




            new VehicleRaceCheckpoint(0, new Vector3(176.5163f, 368.2185f, 107.7932f)),
            new VehicleRaceCheckpoint(1, new Vector3(74.6353f, 326.5884f, 111.1838f)),
            new VehicleRaceCheckpoint(2, new Vector3(40.982517f, 278.166382f, 108.628006f)),
            new VehicleRaceCheckpoint(3, new Vector3(-6.5086f, 269.1777f, 107.8002f)),
            new VehicleRaceCheckpoint(4, new Vector3(-157.2158f, 260.5422f, 92.9641f)),
            new VehicleRaceCheckpoint(5, new Vector3(-306.6567f, 264.1079f, 86.8654f)),
            new VehicleRaceCheckpoint(6, new Vector3(-485.000f, 256.500f, 82.063f)),
            new VehicleRaceCheckpoint(7, new Vector3(-677.1512f, 285.4961f, 81.0311f)),
            new VehicleRaceCheckpoint(8, new Vector3(-829.2744f, 291.7629f, 85.4059f)),
            new VehicleRaceCheckpoint(9, new Vector3(-854.597778f, 467.941254f, 86.396263f)),
            new VehicleRaceCheckpoint(10, new Vector3(-877.540222f, 535.083862f, 90.457161f)),
            new VehicleRaceCheckpoint(11, new Vector3(-1006.2338f, 596.4271f, 101.8474f)),
            new VehicleRaceCheckpoint(12, new Vector3(-1211.739502f, 545.427917f, 96.671173f)),
            new VehicleRaceCheckpoint(13, new Vector3(-1247.7463f, 481.3893f, 92.6580f)),
            new VehicleRaceCheckpoint(14, new Vector3(-1209.5111f, 468.7086f, 88.8673f)),
            new VehicleRaceCheckpoint(15, new Vector3(-1096.5419f, 456.5062f, 76.7926f)),
            new VehicleRaceCheckpoint(16, new Vector3(-1077.0458f, 385.2891f, 68.0071f)),
            new VehicleRaceCheckpoint(17, new Vector3(-1079.000f, 317.000f, 64.750f)),
            new VehicleRaceCheckpoint(18, new Vector3(-1128.9888f, 267.3287f, 64.9992f)),
            new VehicleRaceCheckpoint(19, new Vector3(-1274.6458f, 228.7755f, 59.8090f)),
            new VehicleRaceCheckpoint(20, new Vector3(-1408.2948f, 193.1421f, 57.5883f)),
            new VehicleRaceCheckpoint(21, new Vector3(-1429.7986f, -18.8745f, 51.4889f)),
            new VehicleRaceCheckpoint(22, new Vector3(-1466.3798f, -91.9526f, 49.9463f)),
            new VehicleRaceCheckpoint(23, new Vector3(-1582.2281f, -187.6987f, 54.6562f)),
            new VehicleRaceCheckpoint(24, new Vector3(-1633.1866f, -302.6338f, 50.4393f)),
            new VehicleRaceCheckpoint(25, new Vector3(-1752.000f, -433.000f, 41.813f)),
            new VehicleRaceCheckpoint(26, new Vector3(-1675.6215f, -568.3997f, 33.2031f)),
            new VehicleRaceCheckpoint(27, new Vector3(-1556.6298f, -662.1553f, 27.9556f)),
            new VehicleRaceCheckpoint(28, new Vector3(-1441.3038f, -761.7834f, 22.5293f)),
            new VehicleRaceCheckpoint(29, new Vector3(-1327.7562f, -861.1824f, 15.5558f)),
            new VehicleRaceCheckpoint(30, new Vector3(-1254.9f, -1062.5f, 7.4798f)),
            new VehicleRaceCheckpoint(31, new Vector3(-1306.2f, -1091.1f, 6.0f)),
            new VehicleRaceCheckpoint(32, new Vector3(-1266.1f, -1265.2f, 3.0f)),
            new VehicleRaceCheckpoint(33, new Vector3(-1206.7f, -1251.5f, 6.0f)),
            new VehicleRaceCheckpoint(34, new Vector3(-1149.226685f, -1305.180054f, 4.164155f)),
            new VehicleRaceCheckpoint(35, new Vector3(-933.4516f, -1221.1724f, 4.1712f)),
            new VehicleRaceCheckpoint(36, new Vector3(-804.6487f, -1141.6809f, 8.1794f)),
            new VehicleRaceCheckpoint(37, new Vector3(-653.9498f, -1049.0057f, 16.1054f)),
            new VehicleRaceCheckpoint(38, new Vector3(-635.1398f, -891.9207f, 23.9038f)),
            new VehicleRaceCheckpoint(39, new Vector3(-633.6005f, -739.1533f, 26.3700f)),
            new VehicleRaceCheckpoint(40, new Vector3(-627.4288f, -582.9346f, 33.6256f)),
            new VehicleRaceCheckpoint(41, new Vector3(-625.0278f, -397.2912f, 33.7955f)),
            new VehicleRaceCheckpoint(42, new Vector3(-586.7f, -378.4f, 33.9005f)),
            new VehicleRaceCheckpoint(43, new Vector3(-437.4909f, -388.7413f, 32.1535f)),
            new VehicleRaceCheckpoint(44, new Vector3(-231.9090f, -410.7465f, 29.6085f)),
            new VehicleRaceCheckpoint(45, new Vector3(-147.8854f, -373.2199f, 32.7875f)),
            new VehicleRaceCheckpoint(46, new Vector3(-115.0438f, -285.8302f, 40.5101f)),
            new VehicleRaceCheckpoint(47, new Vector3(-51.4044f, -257.3596f, 44.7924f)),
            new VehicleRaceCheckpoint(48, new Vector3(85.9933f, -309.2740f, 45.4642f)),
            new VehicleRaceCheckpoint(49, new Vector3(225.0007f, -356.0629f, 43.2898f)),
            new VehicleRaceCheckpoint(50, new Vector3(370.0895f, -402.5069f, 44.9239f)),
            new VehicleRaceCheckpoint(51, new Vector3(481.0543f, -305.9488f, 45.6763f)),
            new VehicleRaceCheckpoint(52, new Vector3(527.0013f, -220.0025f, 49.7887f)),
            new VehicleRaceCheckpoint(53, new Vector3(546.504150f, -101.520279f, 64.260414f)),
            new VehicleRaceCheckpoint(54, new Vector3(636.0579f, -57.7903f, 75.5052f)),
            new VehicleRaceCheckpoint(55, new Vector3(703.7552f, 18.0953f, 83.1893f)),
            new VehicleRaceCheckpoint(56, new Vector3(778.6013f, 128.2524f, 78.3677f)),
            new VehicleRaceCheckpoint(57, new Vector3(751.6663f, 181.9566f, 81.9156f)),
            new VehicleRaceCheckpoint(58, new Vector3(609.8065f, 232.9736f, 101.0741f)),
            new VehicleRaceCheckpoint(59, new Vector3(369.29f, 312.41f, 103.24f)),
        };
        VehicleRaceTrack citycircuit1 = new VehicleRaceTrack("citycircuit1", "Circuit - City Circuit", "Race around the City", citycircuit1checkpoints, citycircuit1start);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(citycircuit1); // R Conversion - Needs to be reworked.

        List<VehicleRaceStartingPosition> observortyminiloopstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-403.112f, 1186.880f, 324.692f), 73.661f),
            new VehicleRaceStartingPosition(1, new Vector3(-403.956f, 1184.002f, 324.634f), 73.661f),
            new VehicleRaceStartingPosition(2, new Vector3(-393.516f, 1184.067f, 324.691f), 73.661f),
            new VehicleRaceStartingPosition(3, new Vector3(-394.360f, 1181.188f, 324.645f), 73.661f),
            new VehicleRaceStartingPosition(4, new Vector3(-383.920f, 1181.254f, 324.766f), 73.661f),
            new VehicleRaceStartingPosition(5, new Vector3(-384.764f, 1178.375f, 324.774f), 73.661f),
            new VehicleRaceStartingPosition(6, new Vector3(-374.324f, 1178.441f, 324.833f), 73.661f),
            new VehicleRaceStartingPosition(7, new Vector3(-375.168f, 1175.562f, 324.859f), 73.661f),
        };
        List<VehicleRaceCheckpoint> observortyminiloopcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-458.258f, 1316.552f, 308.661f)),
            new VehicleRaceCheckpoint(1, new Vector3(-296.483f, 1470.189f, 288.496f)),
            new VehicleRaceCheckpoint(2, new Vector3(-190.121f, 1403.590f, 291.606f)),
            new VehicleRaceCheckpoint(3, new Vector3(-408.100f, 1185.787f, 325.115f)),
        };
        VehicleRaceTrack observortyminiloop = new VehicleRaceTrack("observortyminiloop", "Circuit - The Observatory Mini", "Race at the Gallileo Observatory", observortyminiloopcheckpoints, observortyminiloopstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(observortyminiloop);

        List<VehicleRaceStartingPosition> vinewoodguantletstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-1072.870f, 377.730f, 67.842f), 358.093f),
            new VehicleRaceStartingPosition(1, new Vector3(-1075.369f, 377.814f, 67.948f), 358.093f),
            new VehicleRaceStartingPosition(2, new Vector3(-1073.142f, 369.735f, 67.653f), 358.093f),
            new VehicleRaceStartingPosition(3, new Vector3(-1075.640f, 369.818f, 67.759f), 358.093f),
            new VehicleRaceStartingPosition(4, new Vector3(-1073.413f, 361.740f, 67.231f), 358.093f),
            new VehicleRaceStartingPosition(5, new Vector3(-1075.912f, 361.823f, 67.336f), 358.093f),
            new VehicleRaceStartingPosition(6, new Vector3(-1073.685f, 353.744f, 66.674f), 358.093f),
            new VehicleRaceStartingPosition(7, new Vector3(-1076.184f, 353.828f, 66.776f), 358.093f),
        };
        List<VehicleRaceCheckpoint> vinewoodguantletcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-1073.750f, 425.000f, 70.750f)),
            new VehicleRaceCheckpoint(1, new Vector3(-1186.000f, 469.000f, 86.625f)),
            new VehicleRaceCheckpoint(2, new Vector3(-1296.250f, 460.500f, 96.375f)),
            new VehicleRaceCheckpoint(3, new Vector3(-1400.750f, 469.500f, 105.969f)),
            new VehicleRaceCheckpoint(4, new Vector3(-1430.000f, 561.750f, 123.031f)),
            new VehicleRaceCheckpoint(5, new Vector3(-1326.000f, 627.250f, 135.094f)),
            new VehicleRaceCheckpoint(6, new Vector3(-1211.250f, 685.000f, 145.031f)),
            new VehicleRaceCheckpoint(7, new Vector3(-1165.000f, 749.000f, 153.438f)),
            new VehicleRaceCheckpoint(8, new Vector3(-1079.250f, 785.500f, 164.375f)),
            new VehicleRaceCheckpoint(9, new Vector3(-1043.750f, 760.000f, 166.406f)),
            new VehicleRaceCheckpoint(10, new Vector3(-946.500f, 700.500f, 152.406f)),
            new VehicleRaceCheckpoint(11, new Vector3(-842.750f, 710.750f, 147.469f)),
            new VehicleRaceCheckpoint(12, new Vector3(-749.750f, 639.500f, 141.750f)),
            new VehicleRaceCheckpoint(13, new Vector3(-679.250f, 660.500f, 149.406f)),
            new VehicleRaceCheckpoint(14, new Vector3(-654.000f, 695.000f, 151.563f)),
            new VehicleRaceCheckpoint(15, new Vector3(-558.750f, 674.000f, 144.313f)),
            new VehicleRaceCheckpoint(16, new Vector3(-509.500f, 634.250f, 133.188f)),
            new VehicleRaceCheckpoint(17, new Vector3(-521.750f, 552.750f, 113.313f)),
            new VehicleRaceCheckpoint(18, new Vector3(-596.500f, 507.000f, 105.500f)),
            new VehicleRaceCheckpoint(19, new Vector3(-714.750f, 480.000f, 107.063f)),
            new VehicleRaceCheckpoint(20, new Vector3(-823.500f, 439.750f, 87.688f)),
            new VehicleRaceCheckpoint(21, new Vector3(-994.250f, 404.000f, 72.500f)),
        };
        VehicleRaceTrack vinewoodguantlet = new VehicleRaceTrack("vinewoodguantlet", "Circuit - The Vinewood Gauntlet", "Race through the narrow roads of the Vinewood Hills", vinewoodguantletcheckpoints, vinewoodguantletstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(vinewoodguantlet);

        List<VehicleRaceStartingPosition> lakevinewoodstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-74.435f, 1049.740f, 223.050f), 270.325f),
            new VehicleRaceStartingPosition(1, new Vector3(-74.452f, 1052.740f, 223.222f), 270.325f),
            new VehicleRaceStartingPosition(2, new Vector3(-82.435f, 1049.695f, 223.925f), 270.325f),
            new VehicleRaceStartingPosition(3, new Vector3(-82.452f, 1052.694f, 224.091f), 270.325f),
            new VehicleRaceStartingPosition(4, new Vector3(-90.435f, 1049.649f, 224.646f), 270.325f),
            new VehicleRaceStartingPosition(5, new Vector3(-90.452f, 1052.649f, 224.776f), 270.325f),
            new VehicleRaceStartingPosition(6, new Vector3(-98.435f, 1049.604f, 225.429f), 270.325f),
            new VehicleRaceStartingPosition(7, new Vector3(-98.452f, 1052.604f, 225.503f), 270.325f),
        };
        List<VehicleRaceCheckpoint> lakevinewoodcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(34.000f, 1038.250f, 217.594f)),
            new VehicleRaceCheckpoint(1, new Vector3(261.250f, 982.250f, 209.531f)),
            new VehicleRaceCheckpoint(2, new Vector3(315.250f, 955.500f, 207.625f)),
            new VehicleRaceCheckpoint(3, new Vector3(295.000f, 875.000f, 196.750f)),
            new VehicleRaceCheckpoint(4, new Vector3(254.250f, 812.500f, 194.625f)),
            new VehicleRaceCheckpoint(5, new Vector3(116.000f, 712.250f, 208.063f)),
            new VehicleRaceCheckpoint(6, new Vector3(19.250f, 630.750f, 206.375f)),
            new VehicleRaceCheckpoint(7, new Vector3(-122.000f, 637.500f, 207.500f)),
            new VehicleRaceCheckpoint(8, new Vector3(-263.500f, 736.000f, 206.781f)),
            new VehicleRaceCheckpoint(9, new Vector3(-371.250f, 863.000f, 225.656f)),
            new VehicleRaceCheckpoint(10, new Vector3(-315.750f, 1005.750f, 232.500f)),
            new VehicleRaceCheckpoint(11, new Vector3(-71.750f, 1054.250f, 222.875f)),
        };
        VehicleRaceTrack lakevinewood = new VehicleRaceTrack("lakevinewood", "Circuit - Lakeside Rush", "Race around the scenic Vinewood Lake", lakevinewoodcheckpoints, lakevinewoodstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(lakevinewood);

        List<VehicleRaceStartingPosition> vinewoodhills2start = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(1107.625f, 819.249f, 151.705f), 331.537f),
            new VehicleRaceStartingPosition(1, new Vector3(1104.987f, 820.678f, 151.750f), 331.537f),
            new VehicleRaceStartingPosition(2, new Vector3(1103.812f, 812.216f, 151.823f), 331.537f),
            new VehicleRaceStartingPosition(3, new Vector3(1101.174f, 813.645f, 151.876f), 331.537f),
            new VehicleRaceStartingPosition(4, new Vector3(1099.999f, 805.183f, 151.979f), 331.537f),
            new VehicleRaceStartingPosition(5, new Vector3(1097.362f, 806.612f, 152.044f), 331.537f),
            new VehicleRaceStartingPosition(6, new Vector3(1096.186f, 798.150f, 152.200f), 331.537f),
            new VehicleRaceStartingPosition(7, new Vector3(1093.549f, 799.579f, 152.220f), 331.537f),
        };
        List<VehicleRaceCheckpoint> vinewoodhill2scheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(1193.500f, 938.000f, 146.250f)),
            new VehicleRaceCheckpoint(1, new Vector3(1194.000f, 1058.250f, 157.094f)),
            new VehicleRaceCheckpoint(2, new Vector3(1183.000f, 1191.500f, 160.688f)),
            new VehicleRaceCheckpoint(3, new Vector3(1161.750f, 1376.500f, 153.219f)),
            new VehicleRaceCheckpoint(4, new Vector3(1036.000f, 1599.500f, 167.438f)),
            new VehicleRaceCheckpoint(5, new Vector3(848.500f, 1708.750f, 170.031f)),
            new VehicleRaceCheckpoint(6, new Vector3(653.750f, 1755.500f, 190.625f)),
            new VehicleRaceCheckpoint(7, new Vector3(376.250f, 1728.000f, 238.781f)),
            new VehicleRaceCheckpoint(8, new Vector3(185.250f, 1667.500f, 229.219f)),
            new VehicleRaceCheckpoint(9, new Vector3(145.750f, 1585.500f, 228.625f)),
            new VehicleRaceCheckpoint(10, new Vector3(245.500f, 1307.250f, 235.375f)),
            new VehicleRaceCheckpoint(11, new Vector3(301.000f, 1071.750f, 212.969f)),
            new VehicleRaceCheckpoint(12, new Vector3(359.250f, 1000.000f, 209.406f)),
            new VehicleRaceCheckpoint(13, new Vector3(486.250f, 868.250f, 197.094f)),
            new VehicleRaceCheckpoint(14, new Vector3(791.000f, 893.500f, 223.438f)),
            new VehicleRaceCheckpoint(15, new Vector3(928.000f, 982.500f, 230.094f)),
            new VehicleRaceCheckpoint(16, new Vector3(927.500f, 722.750f, 180.969f)),
            new VehicleRaceCheckpoint(17, new Vector3(1032.750f, 701.250f, 158.063f)),
            new VehicleRaceCheckpoint(18, new Vector3(1100.000f, 815.000f, 151.844f)),
        };
        VehicleRaceTrack vinewoodhills2 = new VehicleRaceTrack("Vinewoodhills2", "Circuit - Highline Run", "Race around the scenic Vinewood Hills", vinewoodhill2scheckpoints, vinewoodhills2start);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(vinewoodhills2);

        List<VehicleRaceStartingPosition> roadrunner1start = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(181.964f, 197.748f, 104.682f), 249.500f),
            new VehicleRaceStartingPosition(1, new Vector3(180.561f, 194.002f, 104.646f), 249.500f),
            new VehicleRaceStartingPosition(2, new Vector3(172.599f, 201.255f, 105.107f), 249.500f),
            new VehicleRaceStartingPosition(3, new Vector3(171.196f, 197.509f, 105.074f), 249.500f),
            new VehicleRaceStartingPosition(4, new Vector3(163.234f, 204.761f, 105.374f), 249.500f),
            new VehicleRaceStartingPosition(5, new Vector3(161.831f, 201.015f, 105.356f), 249.500f),
            new VehicleRaceStartingPosition(6, new Vector3(153.869f, 208.268f, 105.651f), 249.500f),
            new VehicleRaceStartingPosition(7, new Vector3(152.466f, 204.522f, 105.627f), 249.500f),
        };
        List<VehicleRaceCheckpoint> roadrunner1checkpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(322.201f, 144.015f, 102.570f)),
            new VehicleRaceCheckpoint(1, new Vector3(545.657f, 65.718f, 94.935f)),
            new VehicleRaceCheckpoint(2, new Vector3(742.844f, -30.362f, 81.279f)),
            new VehicleRaceCheckpoint(3, new Vector3(974.995f, -176.604f, 71.892f)),
            new VehicleRaceCheckpoint(4, new Vector3(1124.051f, -245.459f, 68.140f)),
            new VehicleRaceCheckpoint(5, new Vector3(1191.750f, -436.500f, 66.094f)),
            new VehicleRaceCheckpoint(6, new Vector3(1189.750f, -677.750f, 60.063f)),
            new VehicleRaceCheckpoint(7, new Vector3(1151.000f, -910.500f, 50.219f)),
            new VehicleRaceCheckpoint(8, new Vector3(1212.000f, -1140.500f, 36.531f)),
            new VehicleRaceCheckpoint(9, new Vector3(1239.500f, -1410.000f, 34.063f)),
            new VehicleRaceCheckpoint(10, new Vector3(1370.500f, -1670.000f, 56.063f)),
            new VehicleRaceCheckpoint(11, new Vector3(1419.500f, -1889.750f, 69.844f)),
            new VehicleRaceCheckpoint(12, new Vector3(1217.250f, -2060.250f, 43.344f)),
            new VehicleRaceCheckpoint(13, new Vector3(1023.250f, -2082.000f, 30.031f)),
            new VehicleRaceCheckpoint(14, new Vector3(716.000f, -2051.500f, 28.313f)),
            new VehicleRaceCheckpoint(15, new Vector3(487.250f, -2059.500f, 24.094f)),
            new VehicleRaceCheckpoint(16, new Vector3(367.500f, -2157.250f, 13.594f)),
            new VehicleRaceCheckpoint(17, new Vector3(255.750f, -2088.000f, 16.063f)),
            new VehicleRaceCheckpoint(18, new Vector3(60.750f, -2034.000f, 17.313f)),
            new VehicleRaceCheckpoint(19, new Vector3(-198.000f, -2114.000f, 22.719f)),
            new VehicleRaceCheckpoint(20, new Vector3(-397.250f, -2069.750f, 25.531f)),
            new VehicleRaceCheckpoint(21, new Vector3(-750.250f, -1730.250f, 28.313f)),
            new VehicleRaceCheckpoint(22, new Vector3(-669.750f, -1555.750f, 14.719f)),
            new VehicleRaceCheckpoint(23, new Vector3(-648.500f, -1401.000f, 9.656f)),
            new VehicleRaceCheckpoint(24, new Vector3(-636.250f, -1281.500f, 9.625f)),
            new VehicleRaceCheckpoint(25, new Vector3(-700.000f, -1199.000f, 9.656f)),
            new VehicleRaceCheckpoint(26, new Vector3(-844.250f, -977.750f, 13.844f)),
            new VehicleRaceCheckpoint(27, new Vector3(-1021.250f, -795.750f, 16.031f)),
            new VehicleRaceCheckpoint(28, new Vector3(-1150.500f, -669.000f, 21.313f)),
            new VehicleRaceCheckpoint(29, new Vector3(-1325.000f, -463.500f, 32.438f)),
            new VehicleRaceCheckpoint(30, new Vector3(-1471.750f, -274.250f, 47.438f)),
            new VehicleRaceCheckpoint(31, new Vector3(-1526.750f, -178.750f, 53.750f)),
            new VehicleRaceCheckpoint(32, new Vector3(-1489.000f, -130.750f, 50.625f)),
            new VehicleRaceCheckpoint(33, new Vector3(-1435.500f, -80.750f, 51.156f)),
            new VehicleRaceCheckpoint(34, new Vector3(-1419.500f, 18.250f, 51.563f)),
            new VehicleRaceCheckpoint(35, new Vector3(-1323.500f, 208.250f, 57.594f)),
            new VehicleRaceCheckpoint(36, new Vector3(-1029.000f, 255.250f, 64.219f)),
            new VehicleRaceCheckpoint(37, new Vector3(-820.250f, 208.250f, 73.781f)),
            new VehicleRaceCheckpoint(38, new Vector3(-623.000f, 263.750f, 80.688f)),
            new VehicleRaceCheckpoint(39, new Vector3(-462.750f, 240.750f, 82.063f)),
            new VehicleRaceCheckpoint(40, new Vector3(-177.750f, 249.750f, 91.906f)),
            new VehicleRaceCheckpoint(41, new Vector3(46.750f, 244.750f, 108.563f)),
        };
        VehicleRaceTrack roadrunner1 = new VehicleRaceTrack("roadrunner1", "Circuit - Road Runner", "Race though the streets of LS", roadrunner1checkpoints, roadrunner1start);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(roadrunner1);

        // Drag

        List<VehicleRaceStartingPosition> vinewoodquarterstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(414.309f, 127.973f, 100.256f), 69.795f),
            new VehicleRaceStartingPosition(1, new Vector3(412.928f, 124.219f, 100.351f), 69.795f),
            new VehicleRaceStartingPosition(2, new Vector3(421.817f, 125.210f, 99.948f), 69.795f),
            new VehicleRaceStartingPosition(3, new Vector3(420.435f, 121.456f, 99.923f), 69.795f),
            new VehicleRaceStartingPosition(4, new Vector3(429.325f, 122.447f, 99.367f), 69.795f),
            new VehicleRaceStartingPosition(5, new Vector3(427.943f, 118.693f, 99.555f), 69.795f),
            new VehicleRaceStartingPosition(6, new Vector3(436.832f, 119.684f, 99.032f), 69.795f),
            new VehicleRaceStartingPosition(7, new Vector3(435.451f, 115.930f, 99.034f), 69.795f),
        };
        List<VehicleRaceCheckpoint> vinewoodquartercheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(34.206f, 261.336f, 108.933f)),
        };
        VehicleRaceTrack vinewoodquarter = new VehicleRaceTrack("vinewoodquarter", "Drag - Vinewood Blvd", "Vinewood Blvd Quarter Mile Drag Race", vinewoodquartercheckpoints, vinewoodquarterstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(vinewoodquarter);

        // Point to Point

        List<VehicleRaceStartingPosition> tongvahillsstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-2509.602f, 2287.917f, 31.556f), 276.917f),
            new VehicleRaceStartingPosition(1, new Vector3(-2509.903f, 2290.399f, 31.566f), 276.917f),
            new VehicleRaceStartingPosition(2, new Vector3(-2519.032f, 2286.772f, 31.821f), 276.917f),
            new VehicleRaceStartingPosition(3, new Vector3(-2519.333f, 2289.254f, 31.832f), 276.917f),
            new VehicleRaceStartingPosition(4, new Vector3(-2528.463f, 2285.627f, 31.957f), 276.917f),
            new VehicleRaceStartingPosition(5, new Vector3(-2528.764f, 2288.109f, 31.969f), 276.917f),
            new VehicleRaceStartingPosition(6, new Vector3(-2537.894f, 2284.483f, 31.896f), 276.917f),
            new VehicleRaceStartingPosition(7, new Vector3(-2538.195f, 2286.965f, 31.907f), 276.917f),
        };
        List<VehicleRaceCheckpoint> tongvahillscheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-2385.750f, 2264.250f, 32.094f)),
            new VehicleRaceCheckpoint(1, new Vector3(-2141.750f, 2307.500f, 35.563f)),
            new VehicleRaceCheckpoint(2, new Vector3(-2021.466f, 2277.203f, 45.494f)),
            new VehicleRaceCheckpoint(3, new Vector3(-1810.250f, 2303.500f, 69.000f)),
            new VehicleRaceCheckpoint(4, new Vector3(-1669.000f, 2223.250f, 86.000f)),
            new VehicleRaceCheckpoint(5, new Vector3(-1742.250f, 2089.000f, 116.000f)),
            new VehicleRaceCheckpoint(6, new Vector3(-1843.796f, 2033.744f, 134.068f)),
            new VehicleRaceCheckpoint(7, new Vector3(-1831.000f, 1931.500f, 144.875f)),
            new VehicleRaceCheckpoint(8, new Vector3(-1836.000f, 1837.750f, 158.531f)),
            new VehicleRaceCheckpoint(9, new Vector3(-1978.000f, 1832.500f, 181.656f)),
            new VehicleRaceCheckpoint(10, new Vector3(-2021.500f, 1916.500f, 185.688f)),
            new VehicleRaceCheckpoint(11, new Vector3(-2093.250f, 2001.500f, 189.281f)),
            new VehicleRaceCheckpoint(12, new Vector3(-2220.250f, 1919.500f, 186.906f)),
            new VehicleRaceCheckpoint(13, new Vector3(-2407.750f, 1945.500f, 178.063f)),
            new VehicleRaceCheckpoint(14, new Vector3(-2508.250f, 1835.750f, 163.656f)),
            new VehicleRaceCheckpoint(15, new Vector3(-2546.000f, 1670.500f, 145.250f)),
            new VehicleRaceCheckpoint(16, new Vector3(-2642.250f, 1562.250f, 120.063f)),
            new VehicleRaceCheckpoint(17, new Vector3(-2712.750f, 1486.000f, 102.781f)),
            new VehicleRaceCheckpoint(18, new Vector3(-2791.000f, 1321.250f, 74.281f)),
            new VehicleRaceCheckpoint(19, new Vector3(-2978.750f, 1322.500f, 36.594f)),
            new VehicleRaceCheckpoint(20, new Vector3(-3063.500f, 1191.500f, 21.031f)),
        };
        VehicleRaceTrack tongvahills = new VehicleRaceTrack("tongva_hills", "P2P - Tongva Hills (Short)", "Race through Tongva Hills", tongvahillscheckpoints, tongvahillsstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(tongvahills);

        List<VehicleRaceCheckpoint> tongvahillslongcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-2385.750f, 2264.250f, 32.094f)),
            new VehicleRaceCheckpoint(1, new Vector3(-2141.750f, 2307.500f, 35.563f)),
            new VehicleRaceCheckpoint(2, new Vector3(-2021.466f, 2277.203f, 45.494f)),
            new VehicleRaceCheckpoint(3, new Vector3(-1810.250f, 2303.500f, 69.000f)),
            new VehicleRaceCheckpoint(4, new Vector3(-1669.000f, 2223.250f, 86.000f)),
            new VehicleRaceCheckpoint(5, new Vector3(-1742.250f, 2089.000f, 116.000f)),
            new VehicleRaceCheckpoint(6, new Vector3(-1843.796f, 2033.744f, 134.068f)),
            new VehicleRaceCheckpoint(7, new Vector3(-1831.000f, 1931.500f, 144.875f)),
            new VehicleRaceCheckpoint(8, new Vector3(-1836.000f, 1837.750f, 158.531f)),
            new VehicleRaceCheckpoint(9, new Vector3(-1978.000f, 1832.500f, 181.656f)),
            new VehicleRaceCheckpoint(10, new Vector3(-2021.500f, 1916.500f, 185.688f)),
            new VehicleRaceCheckpoint(11, new Vector3(-2093.250f, 2001.500f, 189.281f)),
            new VehicleRaceCheckpoint(12, new Vector3(-2220.250f, 1919.500f, 186.906f)),
            new VehicleRaceCheckpoint(13, new Vector3(-2407.750f, 1945.500f, 178.063f)),
            new VehicleRaceCheckpoint(14, new Vector3(-2508.250f, 1835.750f, 163.656f)),
            new VehicleRaceCheckpoint(15, new Vector3(-2546.000f, 1670.500f, 145.250f)),
            new VehicleRaceCheckpoint(16, new Vector3(-2642.250f, 1562.250f, 120.063f)),
            new VehicleRaceCheckpoint(17, new Vector3(-2627.250f, 1438.000f, 129.750f)),
            new VehicleRaceCheckpoint(18, new Vector3(-2628.000f, 1164.250f, 157.375f)),
            new VehicleRaceCheckpoint(19, new Vector3(-2446.000f, 1039.000f, 192.938f)),
            new VehicleRaceCheckpoint(20, new Vector3(-2279.750f, 1056.250f, 196.531f)),
            new VehicleRaceCheckpoint(21, new Vector3(-2180.750f, 1026.750f, 190.000f)),
            new VehicleRaceCheckpoint(22, new Vector3(-2017.250f, 839.500f, 163.406f)),
            new VehicleRaceCheckpoint(23, new Vector3(-1921.000f, 737.750f, 140.281f)),
            new VehicleRaceCheckpoint(24, new Vector3(-1764.250f, 824.000f, 140.313f)),
        };
        VehicleRaceTrack tongvahillslong = new VehicleRaceTrack("tongva_hills_long", "P2P - Tongva Hills (Long)", "Race through Tongva Hills", tongvahillslongcheckpoints, tongvahillsstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(tongvahillslong);

        List<VehicleRaceStartingPosition> tongvahillsrevstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-1857.759f, 798.735f, 138.902f), 132.780f),
            new VehicleRaceStartingPosition(1, new Vector3(-1855.721f, 796.534f, 138.926f), 132.780f),
            new VehicleRaceStartingPosition(2, new Vector3(-1851.888f, 804.169f, 138.685f), 132.780f),
            new VehicleRaceStartingPosition(3, new Vector3(-1849.849f, 801.968f, 138.711f), 132.780f),
            new VehicleRaceStartingPosition(4, new Vector3(-1846.016f, 809.603f, 138.455f), 132.780f),
            new VehicleRaceStartingPosition(5, new Vector3(-1843.978f, 807.402f, 138.480f), 132.780f),
            new VehicleRaceStartingPosition(6, new Vector3(-1840.145f, 815.037f, 138.298f), 132.780f),
            new VehicleRaceStartingPosition(7, new Vector3(-1838.107f, 812.835f, 138.323f), 132.780f),
        };
        List<VehicleRaceCheckpoint> tongvahillsrevcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-1965.000f, 708.500f, 140.969f)),
            new VehicleRaceCheckpoint(1, new Vector3(-2017.250f, 839.500f, 163.406f)),
            new VehicleRaceCheckpoint(2, new Vector3(-2103.000f, 954.250f, 183.656f)),
            new VehicleRaceCheckpoint(3, new Vector3(-2199.750f, 1048.000f, 192.594f)),
            new VehicleRaceCheckpoint(4, new Vector3(-2289.000f, 1045.000f, 195.750f)),
            new VehicleRaceCheckpoint(5, new Vector3(-2367.500f, 1028.500f, 194.844f)),
            new VehicleRaceCheckpoint(6, new Vector3(-2510.500f, 1052.250f, 184.969f)),
            new VehicleRaceCheckpoint(7, new Vector3(-2628.000f, 1164.250f, 157.375f)),
            new VehicleRaceCheckpoint(8, new Vector3(-2625.603f, 1405.004f, 133.957f)),
            new VehicleRaceCheckpoint(9, new Vector3(-2644.500f, 1527.750f, 117.719f)),
            new VehicleRaceCheckpoint(10, new Vector3(-2624.000f, 1635.000f, 132.063f)),
            new VehicleRaceCheckpoint(11, new Vector3(-2504.500f, 1693.000f, 152.344f)),
            new VehicleRaceCheckpoint(12, new Vector3(-2483.500f, 1810.000f, 160.219f)),
            new VehicleRaceCheckpoint(13, new Vector3(-2541.250f, 1877.750f, 165.906f)),
            new VehicleRaceCheckpoint(14, new Vector3(-2467.000f, 1952.250f, 172.906f)),
            new VehicleRaceCheckpoint(15, new Vector3(-2350.000f, 1893.500f, 182.344f)),
            new VehicleRaceCheckpoint(16, new Vector3(-2209.000f, 1923.750f, 187.313f)),
            new VehicleRaceCheckpoint(17, new Vector3(-2093.250f, 2001.500f, 189.281f)),
            new VehicleRaceCheckpoint(18, new Vector3(-2050.500f, 1960.750f, 187.969f)),
            new VehicleRaceCheckpoint(19, new Vector3(-1997.750f, 1913.250f, 185.219f)),
            new VehicleRaceCheckpoint(20, new Vector3(-1991.250f, 1809.000f, 180.875f)),
            new VehicleRaceCheckpoint(21, new Vector3(-1881.000f, 1802.000f, 165.719f)),
            new VehicleRaceCheckpoint(22, new Vector3(-1824.000f, 1838.000f, 157.281f)),
            new VehicleRaceCheckpoint(23, new Vector3(-1823.250f, 1922.250f, 145.375f)),
            new VehicleRaceCheckpoint(24, new Vector3(-1864.750f, 1965.500f, 143.094f)),
            new VehicleRaceCheckpoint(25, new Vector3(-1823.250f, 2034.750f, 130.875f)),
            new VehicleRaceCheckpoint(26, new Vector3(-1759.000f, 2072.000f, 119.375f)),
            new VehicleRaceCheckpoint(27, new Vector3(-1681.000f, 2135.250f, 104.375f)),
            new VehicleRaceCheckpoint(28, new Vector3(-1678.506f, 2227.518f, 84.966f)),
            new VehicleRaceCheckpoint(29, new Vector3(-1792.500f, 2302.750f, 70.531f)),
            new VehicleRaceCheckpoint(30, new Vector3(-1917.000f, 2300.750f, 58.063f)),
            new VehicleRaceCheckpoint(31, new Vector3(-2068.250f, 2279.250f, 39.844f)),
        };
        VehicleRaceTrack tongvahillsrev = new VehicleRaceTrack("tongva_hills_rev", "P2P - Tongva Hills Reverse", "Race through Tongva Hills", tongvahillsrevcheckpoints, tongvahillsrevstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(tongvahillsrev);

        List<VehicleRaceStartingPosition> vehicleRaceStartingPositions2 = new List<VehicleRaceStartingPosition>()
        {
            //new VehicleRaceStartingPosition(0,new Vector3(594.9255f, 237.1765f, 102.4954f), 69.08444f),
            //new VehicleRaceStartingPosition(1,new Vector3(593.8934f, 231.983f, 102.473f), 76.16933f),
            //new VehicleRaceStartingPosition(2,new Vector3(603.9206f, 229.4215f, 101.9056f), 75.98711f),
            //new VehicleRaceStartingPosition(3,new Vector3(601.6921f, 235.5364f, 102.1932f), 76.84195f),


            new VehicleRaceStartingPosition(0, new Vector3(592.511f, 239.268f, 102.164f), 74.301f),
            new VehicleRaceStartingPosition(1, new Vector3(591.834f, 236.861f, 102.143f), 74.301f),
            new VehicleRaceStartingPosition(2, new Vector3(600.213f, 237.103f, 101.866f), 74.301f),
            new VehicleRaceStartingPosition(3, new Vector3(599.536f, 234.697f, 101.882f), 74.301f),
            new VehicleRaceStartingPosition(4, new Vector3(607.914f, 234.939f, 101.285f), 74.301f),
            new VehicleRaceStartingPosition(5, new Vector3(607.237f, 232.532f, 101.326f), 74.301f),
            new VehicleRaceStartingPosition(6, new Vector3(615.616f, 232.774f, 100.511f), 74.301f),
            new VehicleRaceStartingPosition(7, new Vector3(614.939f, 230.367f, 100.570f), 74.301f),

        };
        List<VehicleRaceCheckpoint> vehicleRaceCheckpoints2 = new List<VehicleRaceCheckpoint>()
        {
            //new VehicleRaceCheckpoint(0,new Vector3(423.3498f, 295.5466f, 102.4895f)),
            //new VehicleRaceCheckpoint(1,new Vector3(283.6923f, -73.34487f, 69.56183f)),
            //new VehicleRaceCheckpoint(2,new Vector3(-38.75998f, 32.98601f, 71.58444f)),
            //new VehicleRaceCheckpoint(3,new Vector3(-82.37482f, -110.0097f, 57.30961f)),
            //new VehicleRaceCheckpoint(4,new Vector3(-111.9359f, -220.4267f, 44.26889f)),
            //new VehicleRaceCheckpoint(5,new Vector3(-280.4997f, -172.5392f, 39.44448f)),
            //new VehicleRaceCheckpoint(6,new Vector3(-343.272f, -191.9327f, 37.79195f)),
            //new VehicleRaceCheckpoint(7,new Vector3(-418.2509f, -72.76836f, 42.19181f)),
            //new VehicleRaceCheckpoint(8,new Vector3(-391.8105f, 122.2213f, 65.06055f)),
            //new VehicleRaceCheckpoint(9,new Vector3(-643.3143f, 130.383f, 56.60829f)),
            //new VehicleRaceCheckpoint(10,new Vector3(-996.1489f, 71.36115f, 51.30039f)),


            //new VehicleRaceCheckpoint(0, new Vector3(467.806f, 279.618f, 102.451f)),
            //new VehicleRaceCheckpoint(1, new Vector3(385.774f, 213.875f, 102.448f)),
            //new VehicleRaceCheckpoint(2, new Vector3(348.032f, 111.168f, 102.110f)),
            //new VehicleRaceCheckpoint(3, new Vector3(312.919f, 14.082f, 82.591f)),
            //new VehicleRaceCheckpoint(4, new Vector3(222.850f, -61.809f, 68.711f)),
            //new VehicleRaceCheckpoint(5, new Vector3(18.952f, 11.798f, 69.847f)),
            //new VehicleRaceCheckpoint(6, new Vector3(-91.485f, -127.899f, 57.197f)),
            //new VehicleRaceCheckpoint(7, new Vector3(-128.882f, -215.658f, 44.191f)),
            //new VehicleRaceCheckpoint(8, new Vector3(-330.207f, -184.122f, 38.348f)),
            //new VehicleRaceCheckpoint(9, new Vector3(-418.537f, -70.090f, 42.420f)),
            //new VehicleRaceCheckpoint(10, new Vector3(-390.899f, 70.795f, 58.081f)),
            //new VehicleRaceCheckpoint(11, new Vector3(-453.634f, 128.309f, 63.866f)),
            //new VehicleRaceCheckpoint(12, new Vector3(-692.969f, 120.730f, 55.783f)),
            //new VehicleRaceCheckpoint(13, new Vector3(-878.130f, 79.802f, 51.369f)),
            //new VehicleRaceCheckpoint(14, new Vector3(-984.524f, 78.406f, 51.258f)),

            new VehicleRaceCheckpoint(0, new Vector3(467.806f, 279.618f, 102.451f)),
            new VehicleRaceCheckpoint(1, new Vector3(385.774f, 213.875f, 102.448f)),
            new VehicleRaceCheckpoint(2, new Vector3(348.032f, 111.168f, 102.110f)),
            new VehicleRaceCheckpoint(3, new Vector3(312.919f, 14.082f, 82.591f)),
            new VehicleRaceCheckpoint(4, new Vector3(222.850f, -61.809f, 68.711f)),
            new VehicleRaceCheckpoint(5, new Vector3(18.952f, 11.798f, 69.847f)),
            new VehicleRaceCheckpoint(6, new Vector3(-91.485f, -127.899f, 57.197f)),
            new VehicleRaceCheckpoint(7, new Vector3(-128.882f, -215.658f, 44.191f)),
            new VehicleRaceCheckpoint(8, new Vector3(-330.207f, -184.122f, 38.348f)),
            new VehicleRaceCheckpoint(9, new Vector3(-418.537f, -70.090f, 42.420f)),
            new VehicleRaceCheckpoint(10, new Vector3(-390.899f, 70.795f, 58.081f)),
            new VehicleRaceCheckpoint(11, new Vector3(-422.250f, 126.500f, 64.094f)),
            new VehicleRaceCheckpoint(12, new Vector3(-507.250f, 129.000f, 62.438f)),
            new VehicleRaceCheckpoint(13, new Vector3(-548.250f, 73.250f, 54.156f)),
            new VehicleRaceCheckpoint(14, new Vector3(-597.500f, 4.500f, 42.281f)),
            new VehicleRaceCheckpoint(15, new Vector3(-652.250f, 45.250f, 40.031f)),
            new VehicleRaceCheckpoint(16, new Vector3(-649.000f, 198.500f, 69.844f)),
            new VehicleRaceCheckpoint(17, new Vector3(-685.000f, 259.500f, 80.219f)),
            new VehicleRaceCheckpoint(18, new Vector3(-733.250f, 228.750f, 75.813f)),
            new VehicleRaceCheckpoint(19, new Vector3(-755.000f, 193.500f, 74.031f)),
            new VehicleRaceCheckpoint(20, new Vector3(-746.500f, 81.500f, 54.594f)),
            new VehicleRaceCheckpoint(21, new Vector3(-729.250f, 5.750f, 36.875f)),
            new VehicleRaceCheckpoint(22, new Vector3(-747.000f, -38.250f, 36.875f)),
            new VehicleRaceCheckpoint(23, new Vector3(-799.500f, -45.250f, 36.906f)),
            new VehicleRaceCheckpoint(24, new Vector3(-852.250f, 52.750f, 49.281f)),
            new VehicleRaceCheckpoint(25, new Vector3(-862.000f, 167.000f, 65.938f)),
            new VehicleRaceCheckpoint(26, new Vector3(-876.250f, 233.250f, 72.125f)),
            new VehicleRaceCheckpoint(27, new Vector3(-1041.250f, 264.750f, 63.594f)),
            new VehicleRaceCheckpoint(28, new Vector3(-1080.750f, 203.000f, 60.375f)),
            new VehicleRaceCheckpoint(29, new Vector3(-984.750f, 37.750f, 49.781f)),
            new VehicleRaceCheckpoint(30, new Vector3(-915.500f, -65.000f, 37.531f)),
            new VehicleRaceCheckpoint(31, new Vector3(-927.750f, -132.500f, 36.750f)),
            new VehicleRaceCheckpoint(32, new Vector3(-1012.750f, -160.000f, 36.813f)),
            new VehicleRaceCheckpoint(33, new Vector3(-1091.750f, -157.000f, 36.969f)),
            new VehicleRaceCheckpoint(34, new Vector3(-1302.250f, -52.250f, 46.781f)),
        };
        VehicleRaceTrack vinewoodDebug = new VehicleRaceTrack("vinewoodRace1", "P2P - Vinewood Twisted", "Race through Vinewood to the Golf Course", vehicleRaceCheckpoints2, vehicleRaceStartingPositions2);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(vinewoodDebug);

        List<VehicleRaceStartingPosition> vehicleRaceStartingPositions3 = new List<VehicleRaceStartingPosition>()
        {
            //new VehicleRaceStartingPosition(0,new Vector3(594.9255f, 237.1765f, 102.4954f), 69.08444f),
            //new VehicleRaceStartingPosition(1,new Vector3(593.8934f, 231.983f, 102.473f), 76.16933f),
            //new VehicleRaceStartingPosition(2,new Vector3(603.9206f, 229.4215f, 101.9056f), 75.98711f),
            //new VehicleRaceStartingPosition(3,new Vector3(601.6921f, 235.5364f, 102.1932f), 76.84195f),


            new VehicleRaceStartingPosition(0, new Vector3(593.364f, 239.539f, 102.158f), 72.820f),
            new VehicleRaceStartingPosition(1, new Vector3(592.478f, 236.673f, 102.131f), 72.820f),
            new VehicleRaceStartingPosition(2, new Vector3(602.918f, 236.586f, 101.692f), 72.820f),
            new VehicleRaceStartingPosition(3, new Vector3(602.032f, 233.720f, 101.729f), 72.820f),
            new VehicleRaceStartingPosition(4, new Vector3(612.472f, 233.632f, 100.845f), 72.820f),
            new VehicleRaceStartingPosition(5, new Vector3(611.586f, 230.766f, 100.917f), 72.820f),
            new VehicleRaceStartingPosition(6, new Vector3(622.026f, 230.678f, 99.832f), 72.820f),
            new VehicleRaceStartingPosition(7, new Vector3(621.140f, 227.812f, 99.946f), 72.820f),

        };
        List<VehicleRaceCheckpoint> vehicleRaceCheckpoints3 = new List<VehicleRaceCheckpoint>()
        {
            //new VehicleRaceCheckpoint(0,new Vector3(31.48836f, 256.0826f, 109.0203f)),
            //new VehicleRaceCheckpoint(1,new Vector3(-2172.701f, -344.3154f, 12.60608f)),


            new VehicleRaceCheckpoint(0, new Vector3(491.113f, 271.424f, 102.416f)),
            new VehicleRaceCheckpoint(1, new Vector3(204.131f, 360.940f, 105.804f)),
            new VehicleRaceCheckpoint(2, new Vector3(82.481f, 329.350f, 111.630f)),
            new VehicleRaceCheckpoint(3, new Vector3(-55.513f, 266.378f, 103.449f)),
            new VehicleRaceCheckpoint(4, new Vector3(-308.982f, 263.666f, 87.056f)),
            new VehicleRaceCheckpoint(5, new Vector3(-628.429f, 269.934f, 80.954f)),
            new VehicleRaceCheckpoint(6, new Vector3(-795.564f, 223.233f, 75.391f)),
            new VehicleRaceCheckpoint(7, new Vector3(-1011.238f, 271.756f, 65.611f)),
            new VehicleRaceCheckpoint(8, new Vector3(-1081.009f, 204.132f, 60.783f)),
            new VehicleRaceCheckpoint(9, new Vector3(-987.863f, 43.572f, 50.572f)),
            new VehicleRaceCheckpoint(10, new Vector3(-915.250f, -64.955f, 37.902f)),
            new VehicleRaceCheckpoint(11, new Vector3(-954.467f, -146.237f, 37.118f)),
            new VehicleRaceCheckpoint(12, new Vector3(-1091.218f, -157.411f, 37.281f)),
            new VehicleRaceCheckpoint(13, new Vector3(-1304.783f, -51.542f, 47.249f)),
            new VehicleRaceCheckpoint(14, new Vector3(-1523.678f, -138.510f, 52.090f)),
            new VehicleRaceCheckpoint(15, new Vector3(-1625.075f, -279.914f, 51.810f)),
            new VehicleRaceCheckpoint(16, new Vector3(-1739.398f, -347.597f, 45.916f)),
            new VehicleRaceCheckpoint(17, new Vector3(-1952.897f, -166.781f, 32.557f)),
            new VehicleRaceCheckpoint(18, new Vector3(-2172.460f, -326.146f, 12.480f)),
        };
        VehicleRaceTrack vinewoodLONGDebug = new VehicleRaceTrack("vinewoodRace2", "P2P - Vinewood Cross Town", "Race through Vinewood to the coast", vehicleRaceCheckpoints3, vehicleRaceStartingPositions3);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(vinewoodLONGDebug);

        List<VehicleRaceStartingPosition> vinewood3start = new List<VehicleRaceStartingPosition>()
        {
            //new VehicleRaceStartingPosition(0, new Vector3(-1543.546f, -197.2341f, 54.76277f), 38.42368f),
            //new VehicleRaceStartingPosition(1, new Vector3(-1535.816f, -207.0271f, 54.03721f), 38.3179f),
            //new VehicleRaceStartingPosition(2, new Vector3(-1538.248f, -195.0614f, 54.58556f), 43.03611f),
            //new VehicleRaceStartingPosition(3, new Vector3(-1528.094f, -206.51f, 53.52767f), 43.48877f),

            new VehicleRaceStartingPosition(0, new Vector3(-1539.606f, -192.602f, 54.286f), 41.733f),
            new VehicleRaceStartingPosition(1, new Vector3(-1543.337f, -195.931f, 54.408f), 41.733f),
            new VehicleRaceStartingPosition(2, new Vector3(-1532.949f, -200.065f, 53.821f), 41.733f),
            new VehicleRaceStartingPosition(3, new Vector3(-1536.681f, -203.393f, 53.921f), 41.733f),
            new VehicleRaceStartingPosition(4, new Vector3(-1526.293f, -207.527f, 52.914f), 41.733f),
            new VehicleRaceStartingPosition(5, new Vector3(-1530.024f, -210.856f, 53.013f), 41.733f),
            new VehicleRaceStartingPosition(6, new Vector3(-1519.636f, -214.990f, 51.786f), 41.733f),
            new VehicleRaceStartingPosition(7, new Vector3(-1523.367f, -218.318f, 51.902f), 41.733f),

        };
        List<VehicleRaceCheckpoint> vinewood3checkpoints = new List<VehicleRaceCheckpoint>()
        {
            //new VehicleRaceCheckpoint(0, new Vector3(-1592.985f, -134.3829f, 55.47266f)),
            //new VehicleRaceCheckpoint(1, new Vector3(-1851.486f, 140.209f, 78.69758f)),
            //new VehicleRaceCheckpoint(2, new Vector3(-1746.352f, 812.6461f, 141.1693f)),
            //new VehicleRaceCheckpoint(3, new Vector3(-1633.375f, 1043.433f, 152.5267f)),
            //new VehicleRaceCheckpoint(4, new Vector3(-1471.54f, 1795.392f, 83.93646f)),
            //new VehicleRaceCheckpoint(5, new Vector3(-1531.019f, 2142.836f, 54.96165f)),
            //new VehicleRaceCheckpoint(6, new Vector3(-1654.215f, 2383.382f, 35.76158f)),
            //new VehicleRaceCheckpoint(7, new Vector3(-1754.635f, 2429.498f, 30.93673f)),
            //new VehicleRaceCheckpoint(8, new Vector3(-2012.238f, 2343.076f, 33.18634f)),


            new VehicleRaceCheckpoint(0, new Vector3(-1606.016f, -109.239f, 57.241f)),
            new VehicleRaceCheckpoint(1, new Vector3(-1779.288f, 84.953f, 69.394f)),
            new VehicleRaceCheckpoint(2, new Vector3(-1899.201f, 177.258f, 81.851f)),
            new VehicleRaceCheckpoint(3, new Vector3(-1977.112f, 438.280f, 99.459f)),
            new VehicleRaceCheckpoint(4, new Vector3(-1872.541f, 716.501f, 129.096f)),
            new VehicleRaceCheckpoint(5, new Vector3(-1708.338f, 865.341f, 146.084f)),
            new VehicleRaceCheckpoint(6, new Vector3(-1628.329f, 1062.243f, 152.495f)),
            new VehicleRaceCheckpoint(7, new Vector3(-1608.546f, 1337.047f, 132.748f)),
            new VehicleRaceCheckpoint(8, new Vector3(-1485.156f, 1510.304f, 114.724f)),
            new VehicleRaceCheckpoint(9, new Vector3(-1480.054f, 1771.965f, 86.603f)),
            new VehicleRaceCheckpoint(10, new Vector3(-1490.308f, 2050.227f, 61.804f)),
            new VehicleRaceCheckpoint(11, new Vector3(-1539.825f, 2184.522f, 54.722f)),
            new VehicleRaceCheckpoint(12, new Vector3(-1651.947f, 2378.044f, 36.826f)),
            new VehicleRaceCheckpoint(13, new Vector3(-1756.626f, 2427.221f, 31.015f)),
            new VehicleRaceCheckpoint(14, new Vector3(-2003.061f, 2346.842f, 33.066f)),

        };
        VehicleRaceTrack vinewood3 = new VehicleRaceTrack("vinewoodRace3", "P2P - Morning Wood Escape", "Race through the mountain freeways to Lago Zancudo", vinewood3checkpoints, vinewood3start);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(vinewood3);

        List<VehicleRaceStartingPosition> vinewoodhillsstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(1107.625f, 819.249f, 151.705f), 331.537f),
            new VehicleRaceStartingPosition(1, new Vector3(1104.987f, 820.678f, 151.750f), 331.537f),
            new VehicleRaceStartingPosition(2, new Vector3(1103.812f, 812.216f, 151.823f), 331.537f),
            new VehicleRaceStartingPosition(3, new Vector3(1101.174f, 813.645f, 151.876f), 331.537f),
            new VehicleRaceStartingPosition(4, new Vector3(1099.999f, 805.183f, 151.979f), 331.537f),
            new VehicleRaceStartingPosition(5, new Vector3(1097.362f, 806.612f, 152.044f), 331.537f),
            new VehicleRaceStartingPosition(6, new Vector3(1096.186f, 798.150f, 152.200f), 331.537f),
            new VehicleRaceStartingPosition(7, new Vector3(1093.549f, 799.579f, 152.220f), 331.537f),
        };
        List<VehicleRaceCheckpoint> vinewoodhillscheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(1193.500f, 938.000f, 146.250f)),
            new VehicleRaceCheckpoint(1, new Vector3(1194.000f, 1058.250f, 157.094f)),
            new VehicleRaceCheckpoint(2, new Vector3(1183.000f, 1191.500f, 160.688f)),
            new VehicleRaceCheckpoint(3, new Vector3(1161.750f, 1376.500f, 153.219f)),
            new VehicleRaceCheckpoint(4, new Vector3(1036.000f, 1599.500f, 167.438f)),
            new VehicleRaceCheckpoint(5, new Vector3(848.500f, 1708.750f, 170.031f)),
            new VehicleRaceCheckpoint(6, new Vector3(653.750f, 1755.500f, 190.625f)),
            new VehicleRaceCheckpoint(7, new Vector3(376.250f, 1728.000f, 238.781f)),
            new VehicleRaceCheckpoint(8, new Vector3(168.500f, 1661.000f, 228.406f)),
        };
        VehicleRaceTrack vinewoodhills = new VehicleRaceTrack("Vinewoodhills", "P2P - Rich Man's Run", "Race through the Vinewood Hills", vinewoodhillscheckpoints, vinewoodhillsstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(vinewoodhills);

    }

    private void CentralTracks()
    {
        //Circuit

        List<VehicleRaceStartingPosition> lsairportstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-824.876f, -2575.069f, 12.666f), 329.470f),
            new VehicleRaceStartingPosition(1, new Vector3(-827.030f, -2573.799f, 12.749f), 329.470f),
            new VehicleRaceStartingPosition(2, new Vector3(-828.940f, -2581.960f, 12.707f), 329.470f),
            new VehicleRaceStartingPosition(3, new Vector3(-831.093f, -2580.690f, 12.749f), 329.470f),
            new VehicleRaceStartingPosition(4, new Vector3(-833.004f, -2588.851f, 12.625f), 329.470f),
            new VehicleRaceStartingPosition(5, new Vector3(-835.157f, -2587.581f, 12.749f), 329.470f),
            new VehicleRaceStartingPosition(6, new Vector3(-837.068f, -2595.742f, 12.702f), 329.470f),
            new VehicleRaceStartingPosition(7, new Vector3(-839.221f, -2594.472f, 12.785f), 329.470f),
        };
        List<VehicleRaceCheckpoint> lsairportcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-807.6469f, -2469.8474f, 12.7351f)),
            new VehicleRaceCheckpoint(1, new Vector3(-846.8392f, -2324.6980f, 16.9914f)),
            new VehicleRaceCheckpoint(2, new Vector3(-762.6910f, -2198.8743f, 14.9078f)),
            new VehicleRaceCheckpoint(3, new Vector3(-656.6849f, -2105.3574f, 14.5170f)),
            new VehicleRaceCheckpoint(4, new Vector3(-562.5864f, -2083.1013f, 26.3678f)),
            new VehicleRaceCheckpoint(5, new Vector3(-336.5846f, -2111.5476f, 22.7184f)),
            new VehicleRaceCheckpoint(6, new Vector3(-238.7528f, -2134.7715f, 21.7367f)),
            new VehicleRaceCheckpoint(7, new Vector3(-18.5043f, -2050.7546f, 18.0635f)),
            new VehicleRaceCheckpoint(8, new Vector3(109.6071f, -2047.1046f, 17.3701f)),
            new VehicleRaceCheckpoint(9, new Vector3(162.2125f, -2017.6981f, 17.2636f)),
            new VehicleRaceCheckpoint(10, new Vector3(215.2578f, -1946.2358f, 20.9418f)),
            new VehicleRaceCheckpoint(11, new Vector3(200.6974f, -1907.0240f, 22.7627f)),
            new VehicleRaceCheckpoint(12, new Vector3(113.3993f, -1865.5662f, 23.4617f)),
            new VehicleRaceCheckpoint(13, new Vector3(68.2166f, -1891.0560f, 20.7378f)),
            new VehicleRaceCheckpoint(14, new Vector3(-111.4028f, -1759.6040f, 28.8334f)),
            new VehicleRaceCheckpoint(15, new Vector3(-242.8844f, -1813.0552f, 28.7495f)),
            new VehicleRaceCheckpoint(16, new Vector3(-364.1700f, -1821.9215f, 21.5683f)),
            new VehicleRaceCheckpoint(17, new Vector3(-478.9611f, -1885.9553f, 16.6741f)),
            new VehicleRaceCheckpoint(18, new Vector3(-598.0135f, -1999.6506f, 16.4316f)),
            new VehicleRaceCheckpoint(19, new Vector3(-720.7257f, -2133.1033f, 12.3082f)),
            new VehicleRaceCheckpoint(20, new Vector3(-777.2783f, -2181.1165f, 14.8988f)),
            new VehicleRaceCheckpoint(21, new Vector3(-863.5436f, -2251.4424f, 17.3272f)),
            new VehicleRaceCheckpoint(22, new Vector3(-948.9650f, -2368.0957f, 19.2050f)),
            new VehicleRaceCheckpoint(23, new Vector3(-990.7916f, -2440.4375f, 19.2041f)),
            new VehicleRaceCheckpoint(24, new Vector3(-1089.3838f, -2613.6155f, 19.2104f)),
            new VehicleRaceCheckpoint(25, new Vector3(-1084.4546f, -2680.6423f, 19.2111f)),
            new VehicleRaceCheckpoint(26, new Vector3(-996.7110f, -2742.9341f, 19.2060f)),
            new VehicleRaceCheckpoint(27, new Vector3(-926.4546f, -2730.2039f, 19.2052f)),
            new VehicleRaceCheckpoint(28, new Vector3(-874.8831f, -2665.6255f, 18.6777f)),
            new VehicleRaceCheckpoint(29, new Vector3(-813.0546f, -2546.7380f, 12.7888f)),
        };
        VehicleRaceTrack lsairport = new VehicleRaceTrack("lsairport", "Circuit - Airport", "Race from LSIA", lsairportcheckpoints, lsairportstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(lsairport); // R Conversion

        List<VehicleRaceStartingPosition> groovingstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(90.221f, -1923.247f, 19.743f), 52.033f),
            new VehicleRaceStartingPosition(1, new Vector3(88.375f, -1925.612f, 19.802f), 52.033f),
            new VehicleRaceStartingPosition(2, new Vector3(96.528f, -1928.169f, 19.746f), 52.033f),
            new VehicleRaceStartingPosition(3, new Vector3(94.682f, -1930.534f, 19.804f), 52.033f),
            new VehicleRaceStartingPosition(4, new Vector3(102.835f, -1933.090f, 19.804f), 52.033f),
            new VehicleRaceStartingPosition(5, new Vector3(100.989f, -1935.456f, 19.804f), 52.033f),
            new VehicleRaceStartingPosition(6, new Vector3(109.141f, -1938.012f, 19.804f), 52.033f),
            new VehicleRaceStartingPosition(7, new Vector3(107.296f, -1940.377f, 19.804f), 52.033f),
        };
        List<VehicleRaceCheckpoint> groovingcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-67.500f, -1795.250f, 26.844f)),
            new VehicleRaceCheckpoint(1, new Vector3(-159.500f, -1709.500f, 29.781f)),
            new VehicleRaceCheckpoint(2, new Vector3(-183.500f, -1632.250f, 32.313f)),
            new VehicleRaceCheckpoint(3, new Vector3(-143.750f, -1552.750f, 33.500f)),
            new VehicleRaceCheckpoint(4, new Vector3(-97.500f, -1502.250f, 32.594f)),
            new VehicleRaceCheckpoint(5, new Vector3(-28.000f, -1463.250f, 30.031f)),
            new VehicleRaceCheckpoint(6, new Vector3(30.000f, -1478.000f, 28.906f)),
            new VehicleRaceCheckpoint(7, new Vector3(107.750f, -1543.250f, 28.375f)),
            new VehicleRaceCheckpoint(8, new Vector3(230.000f, -1634.250f, 28.313f)),
            new VehicleRaceCheckpoint(9, new Vector3(297.000f, -1686.250f, 28.406f)),
            new VehicleRaceCheckpoint(10, new Vector3(397.000f, -1770.250f, 28.406f)),
            new VehicleRaceCheckpoint(11, new Vector3(430.500f, -1798.250f, 27.500f)),
            new VehicleRaceCheckpoint(12, new Vector3(445.250f, -1849.750f, 26.906f)),
            new VehicleRaceCheckpoint(13, new Vector3(384.500f, -1908.500f, 23.844f)),
            new VehicleRaceCheckpoint(14, new Vector3(332.750f, -1959.000f, 23.469f)),
            new VehicleRaceCheckpoint(15, new Vector3(267.000f, -2037.000f, 17.313f)),
            new VehicleRaceCheckpoint(16, new Vector3(205.750f, -2046.250f, 17.313f)),
            new VehicleRaceCheckpoint(17, new Vector3(169.500f, -2009.000f, 17.313f)),
            new VehicleRaceCheckpoint(18, new Vector3(212.000f, -1953.750f, 20.438f)),
            new VehicleRaceCheckpoint(19, new Vector3(192.000f, -1903.750f, 22.719f)),
            new VehicleRaceCheckpoint(20, new Vector3(121.500f, -1871.000f, 23.125f)),
            new VehicleRaceCheckpoint(21, new Vector3(81.250f, -1878.750f, 22.219f)),
            new VehicleRaceCheckpoint(22, new Vector3(38.000f, -1884.000f, 21.375f)),
        };
        VehicleRaceTrack grooving = new VehicleRaceTrack("hoodrun", "Circuit - Around the Hood", "Race Around the Hood", groovingcheckpoints, groovingstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(grooving);

        List<VehicleRaceStartingPosition> lsialoopstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-899.544f, -2665.689f, 12.745f), 330.090f),
            new VehicleRaceStartingPosition(1, new Vector3(-895.644f, -2667.935f, 12.811f), 330.090f),
            new VehicleRaceStartingPosition(2, new Vector3(-903.532f, -2672.625f, 12.743f), 330.090f),
            new VehicleRaceStartingPosition(3, new Vector3(-899.632f, -2674.870f, 12.811f), 330.090f),
            new VehicleRaceStartingPosition(4, new Vector3(-907.519f, -2679.560f, 12.750f), 330.090f),
            new VehicleRaceStartingPosition(5, new Vector3(-903.620f, -2681.805f, 12.811f), 330.090f),
            new VehicleRaceStartingPosition(6, new Vector3(-911.507f, -2686.495f, 12.750f), 330.090f),
            new VehicleRaceStartingPosition(7, new Vector3(-907.607f, -2688.740f, 12.811f), 330.090f),
        };
        List<VehicleRaceCheckpoint> lsialoopcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-831.250f, -2547.500f, 12.719f)),
            new VehicleRaceCheckpoint(1, new Vector3(-860.250f, -2438.000f, 12.781f)),
            new VehicleRaceCheckpoint(2, new Vector3(-992.500f, -2475.250f, 12.781f)),
            new VehicleRaceCheckpoint(3, new Vector3(-1064.500f, -2600.250f, 12.781f)),
            new VehicleRaceCheckpoint(4, new Vector3(-1024.750f, -2709.250f, 12.781f)),
            new VehicleRaceCheckpoint(5, new Vector3(-916.000f, -2699.750f, 12.781f)),
            new VehicleRaceCheckpoint(6, new Vector3(-886.000f, -2647.750f, 12.781f)),
        };
        VehicleRaceTrack lsialoop = new VehicleRaceTrack("lsia_loop", "Circuit - Baggage Claim", "Race around LSIA inner loop", lsialoopcheckpoints, lsialoopstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(lsialoop);

        List<VehicleRaceStartingPosition> mesabridgesstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(264.817f, -857.453f, 28.336f), 249.948f),
            new VehicleRaceStartingPosition(1, new Vector3(266.360f, -853.226f, 28.336f), 249.948f),
            new VehicleRaceStartingPosition(2, new Vector3(257.302f, -854.710f, 28.490f), 249.948f),
            new VehicleRaceStartingPosition(3, new Vector3(258.845f, -850.483f, 28.466f), 249.948f),
            new VehicleRaceStartingPosition(4, new Vector3(249.787f, -851.967f, 28.687f), 249.948f),
            new VehicleRaceStartingPosition(5, new Vector3(251.330f, -847.740f, 28.654f), 249.948f),
            new VehicleRaceStartingPosition(6, new Vector3(242.272f, -849.224f, 28.862f), 249.948f),
            new VehicleRaceStartingPosition(7, new Vector3(243.815f, -844.997f, 28.824f), 249.948f),
        };
        List<VehicleRaceCheckpoint> mesabridgescheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(355.250f, -857.250f, 28.313f)),
            new VehicleRaceCheckpoint(1, new Vector3(555.319f, -856.695f, 39.882f)),
            new VehicleRaceCheckpoint(2, new Vector3(847.181f, -853.567f, 42.662f)),
            new VehicleRaceCheckpoint(3, new Vector3(951.250f, -880.500f, 42.563f)),
            new VehicleRaceCheckpoint(4, new Vector3(995.750f, -1028.500f, 41.188f)),
            new VehicleRaceCheckpoint(5, new Vector3(847.750f, -1086.250f, 27.031f)),
            new VehicleRaceCheckpoint(6, new Vector3(781.750f, -1032.500f, 25.406f)),
            new VehicleRaceCheckpoint(7, new Vector3(711.393f, -1006.208f, 29.873f)),
            new VehicleRaceCheckpoint(8, new Vector3(513.335f, -1030.500f, 35.886f)),
            new VehicleRaceCheckpoint(9, new Vector3(313.987f, -1041.185f, 28.254f)),
            new VehicleRaceCheckpoint(10, new Vector3(159.163f, -1012.428f, 28.392f)),
            new VehicleRaceCheckpoint(11, new Vector3(127.750f, -963.500f, 28.531f)),
            new VehicleRaceCheckpoint(12, new Vector3(165.000f, -868.500f, 29.719f)),
            new VehicleRaceCheckpoint(13, new Vector3(222.250f, -844.750f, 29.313f)),
        };
        VehicleRaceTrack mesabridges = new VehicleRaceTrack("mesabridges", "Circuit - Bridge Work", "Race around La Mesa", mesabridgescheckpoints, mesabridgesstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(mesabridges);

        List<VehicleRaceStartingPosition> cypressflatstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(665.087f, -2049.536f, 28.349f), 85.073f),
            new VehicleRaceStartingPosition(1, new Vector3(665.473f, -2045.053f, 28.289f), 85.073f),
            new VehicleRaceStartingPosition(2, new Vector3(673.057f, -2050.223f, 28.342f), 85.073f),
            new VehicleRaceStartingPosition(3, new Vector3(673.444f, -2045.740f, 28.288f), 85.073f),
            new VehicleRaceStartingPosition(4, new Vector3(681.028f, -2050.910f, 28.341f), 85.073f),
            new VehicleRaceStartingPosition(5, new Vector3(681.414f, -2046.427f, 28.288f), 85.073f),
            new VehicleRaceStartingPosition(6, new Vector3(688.998f, -2051.597f, 28.340f), 85.073f),
            new VehicleRaceStartingPosition(7, new Vector3(689.385f, -2047.114f, 28.288f), 85.073f),
        };
        List<VehicleRaceCheckpoint> cypressflatcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(567.000f, -2038.500f, 28.313f)),
            new VehicleRaceCheckpoint(1, new Vector3(460.500f, -2022.000f, 22.938f)),
            new VehicleRaceCheckpoint(2, new Vector3(325.000f, -1886.000f, 25.313f)),
            new VehicleRaceCheckpoint(3, new Vector3(106.500f, -1705.000f, 28.188f)),
            new VehicleRaceCheckpoint(4, new Vector3(108.500f, -1639.250f, 28.313f)),
            new VehicleRaceCheckpoint(5, new Vector3(239.750f, -1566.500f, 28.313f)),
            new VehicleRaceCheckpoint(6, new Vector3(390.750f, -1480.250f, 28.313f)),
            new VehicleRaceCheckpoint(7, new Vector3(462.500f, -1444.000f, 28.344f)),
            new VehicleRaceCheckpoint(8, new Vector3(584.000f, -1441.750f, 28.781f)),
            new VehicleRaceCheckpoint(9, new Vector3(716.000f, -1441.750f, 30.563f)),
            new VehicleRaceCheckpoint(10, new Vector3(801.079f, -1482.870f, 26.798f)),
            new VehicleRaceCheckpoint(11, new Vector3(830.750f, -1648.250f, 28.906f)),
            new VehicleRaceCheckpoint(12, new Vector3(812.000f, -1775.250f, 28.375f)),
            new VehicleRaceCheckpoint(13, new Vector3(783.250f, -1928.000f, 28.313f)),
            new VehicleRaceCheckpoint(14, new Vector3(765.750f, -2017.250f, 28.281f)),
            new VehicleRaceCheckpoint(15, new Vector3(716.000f, -2051.500f, 28.313f)),
            new VehicleRaceCheckpoint(16, new Vector3(644.750f, -2045.250f, 28.313f)),
        };
        VehicleRaceTrack cypressflat1 = new VehicleRaceTrack("cypressflat1", "Circuit - Cypress Flats", "Race around Cypress Flats", cypressflatcheckpoints, cypressflatstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(cypressflat1);

        List<VehicleRaceStartingPosition> delperroloopstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-1517.759f, -681.994f, 27.663f), 51.086f),
            new VehicleRaceStartingPosition(1, new Vector3(-1520.585f, -685.496f, 27.569f), 51.086f),
            new VehicleRaceStartingPosition(2, new Vector3(-1511.534f, -687.019f, 27.224f), 51.086f),
            new VehicleRaceStartingPosition(3, new Vector3(-1514.361f, -690.521f, 27.239f), 51.086f),
            new VehicleRaceStartingPosition(4, new Vector3(-1505.309f, -692.045f, 26.779f), 51.086f),
            new VehicleRaceStartingPosition(5, new Vector3(-1508.136f, -695.546f, 26.789f), 51.086f),
            new VehicleRaceStartingPosition(6, new Vector3(-1499.084f, -697.070f, 26.362f), 51.086f),
            new VehicleRaceStartingPosition(7, new Vector3(-1501.911f, -700.571f, 26.366f), 51.086f),
        };
        List<VehicleRaceCheckpoint> delperroloopcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            //new VehicleRaceCheckpoint(0, new Vector3(-1681.000f, -549.250f, 34.594f)),
            //new VehicleRaceCheckpoint(1, new Vector3(-1703.500f, -397.250f, 45.531f)),
            //new VehicleRaceCheckpoint(2, new Vector3(-1594.250f, -226.250f, 53.594f)),
            //new VehicleRaceCheckpoint(3, new Vector3(-1450.750f, -89.250f, 50.031f)),
            //new VehicleRaceCheckpoint(4, new Vector3(-1310.750f, -66.750f, 47.156f)),
            //new VehicleRaceCheckpoint(5, new Vector3(-1141.750f, -150.000f, 38.281f)),
            //new VehicleRaceCheckpoint(6, new Vector3(-948.250f, -247.250f, 37.688f)),
            //new VehicleRaceCheckpoint(7, new Vector3(-713.750f, -359.000f, 33.688f)),
            //new VehicleRaceCheckpoint(8, new Vector3(-633.000f, -427.000f, 33.813f)),
            //new VehicleRaceCheckpoint(9, new Vector3(-642.750f, -593.500f, 33.125f)),
            //new VehicleRaceCheckpoint(10, new Vector3(-637.750f, -771.250f, 24.563f)),
            //new VehicleRaceCheckpoint(11, new Vector3(-645.750f, -892.750f, 23.813f)),
            //new VehicleRaceCheckpoint(12, new Vector3(-684.000f, -1056.500f, 14.469f)),
            //new VehicleRaceCheckpoint(13, new Vector3(-832.250f, -1144.750f, 6.375f)),
            //new VehicleRaceCheckpoint(14, new Vector3(-1009.000f, -1246.250f, 4.938f)),
            //new VehicleRaceCheckpoint(15, new Vector3(-1142.750f, -1304.750f, 4.125f)),
            //new VehicleRaceCheckpoint(16, new Vector3(-1213.750f, -1184.750f, 6.750f)),
            //new VehicleRaceCheckpoint(17, new Vector3(-1267.000f, -1014.750f, 8.375f)),
            //new VehicleRaceCheckpoint(18, new Vector3(-1323.000f, -842.000f, 16.031f)),
            //new VehicleRaceCheckpoint(19, new Vector3(-1495.750f, -706.500f, 25.906f)),


            //new VehicleRaceCheckpoint(0, new Vector3(-1681.000f, -549.250f, 34.594f)),
            //new VehicleRaceCheckpoint(1, new Vector3(-1744.500f, -476.250f, 39.406f)),
            //new VehicleRaceCheckpoint(2, new Vector3(-1703.500f, -397.250f, 45.531f)),
            //new VehicleRaceCheckpoint(3, new Vector3(-1601.250f, -241.250f, 53.156f)),
            //new VehicleRaceCheckpoint(4, new Vector3(-1426.988f, -85.263f, 51.768f)),
            //new VehicleRaceCheckpoint(5, new Vector3(-1291.092f, -71.797f, 46.054f)),
            //new VehicleRaceCheckpoint(6, new Vector3(-1163.000f, -138.750f, 38.688f)),
            //new VehicleRaceCheckpoint(7, new Vector3(-912.250f, -265.750f, 39.594f)),
            //new VehicleRaceCheckpoint(8, new Vector3(-693.713f, -366.576f, 33.774f)),
            //new VehicleRaceCheckpoint(9, new Vector3(-640.685f, -421.789f, 34.185f)),
            //new VehicleRaceCheckpoint(10, new Vector3(-643.103f, -609.802f, 32.652f)),
            //new VehicleRaceCheckpoint(11, new Vector3(-643.217f, -774.061f, 24.814f)),
            //new VehicleRaceCheckpoint(12, new Vector3(-646.043f, -902.298f, 23.963f)),
            //new VehicleRaceCheckpoint(13, new Vector3(-684.000f, -1056.500f, 14.469f)),
            //new VehicleRaceCheckpoint(14, new Vector3(-832.250f, -1144.750f, 6.375f)),
            //new VehicleRaceCheckpoint(15, new Vector3(-998.500f, -1240.250f, 4.719f)),
            //new VehicleRaceCheckpoint(16, new Vector3(-1148.750f, -1303.250f, 4.156f)),
            //new VehicleRaceCheckpoint(17, new Vector3(-1207.156f, -1194.173f, 7.116f)),
            //new VehicleRaceCheckpoint(18, new Vector3(-1261.805f, -1014.320f, 8.728f)),
            //new VehicleRaceCheckpoint(19, new Vector3(-1311.000f, -856.750f, 14.688f)),
            //new VehicleRaceCheckpoint(20, new Vector3(-1400.987f, -774.296f, 20.360f)),
            //new VehicleRaceCheckpoint(21, new Vector3(-1511.265f, -690.456f, 27.462f)),

            //new VehicleRaceCheckpoint(0, new Vector3(-1680.170f, -549.642f, 34.901f)),
            //new VehicleRaceCheckpoint(1, new Vector3(-1744.962f, -473.473f, 39.986f)),
            //new VehicleRaceCheckpoint(2, new Vector3(-1709.270f, -404.303f, 45.083f)),
            //new VehicleRaceCheckpoint(3, new Vector3(-1603.231f, -247.552f, 53.330f)),
            //new VehicleRaceCheckpoint(4, new Vector3(-1499.942f, -139.856f, 51.799f)),
            //new VehicleRaceCheckpoint(5, new Vector3(-1363.931f, -65.016f, 51.014f)),
            //new VehicleRaceCheckpoint(6, new Vector3(-1232.212f, -101.489f, 42.341f)),
            //new VehicleRaceCheckpoint(7, new Vector3(-1019.637f, -211.355f, 37.292f)),
            //new VehicleRaceCheckpoint(8, new Vector3(-921.646f, -261.266f, 39.766f)),
            //new VehicleRaceCheckpoint(9, new Vector3(-699.729f, -364.361f, 33.813f)),
            //new VehicleRaceCheckpoint(10, new Vector3(-640.865f, -424.295f, 34.182f)),
            //new VehicleRaceCheckpoint(11, new Vector3(-643.126f, -610.073f, 32.639f)),
            //new VehicleRaceCheckpoint(12, new Vector3(-643.210f, -781.648f, 24.711f)),
            //new VehicleRaceCheckpoint(13, new Vector3(-646.166f, -900.877f, 24.000f)),
            //new VehicleRaceCheckpoint(14, new Vector3(-692.471f, -1062.013f, 14.171f)),
            //new VehicleRaceCheckpoint(15, new Vector3(-831.799f, -1144.484f, 6.818f)),
            //new VehicleRaceCheckpoint(16, new Vector3(-993.130f, -1237.245f, 5.014f)),
            //new VehicleRaceCheckpoint(17, new Vector3(-1148.523f, -1305.121f, 4.562f)),
            //new VehicleRaceCheckpoint(18, new Vector3(-1206.776f, -1195.009f, 7.102f)),
            //new VehicleRaceCheckpoint(19, new Vector3(-1254.655f, -1038.759f, 8.054f)),
            //new VehicleRaceCheckpoint(20, new Vector3(-1298.872f, -873.166f, 11.895f)),
            //new VehicleRaceCheckpoint(21, new Vector3(-1401.603f, -773.881f, 20.425f)),
            //new VehicleRaceCheckpoint(22, new Vector3(-1517.466f, -682.359f, 27.985f)),

            //new VehicleRaceCheckpoint(0, new Vector3(-1673.849f, -554.613f, 34.234f)),
            //new VehicleRaceCheckpoint(1, new Vector3(-1742.856f, -484.706f, 39.271f)),
            //new VehicleRaceCheckpoint(2, new Vector3(-1708.071f, -402.931f, 45.244f)),
            //new VehicleRaceCheckpoint(3, new Vector3(-1605.078f, -254.668f, 53.084f)),
            //new VehicleRaceCheckpoint(4, new Vector3(-1502.115f, -141.529f, 51.887f)),
            //new VehicleRaceCheckpoint(5, new Vector3(-1370.634f, -66.066f, 51.310f)),
            //new VehicleRaceCheckpoint(6, new Vector3(-1199.826f, -119.260f, 40.409f)),
            //new VehicleRaceCheckpoint(7, new Vector3(-924.729f, -259.741f, 39.598f)),
            //new VehicleRaceCheckpoint(8, new Vector3(-706.681f, -361.711f, 33.905f)),
            //new VehicleRaceCheckpoint(9, new Vector3(-640.784f, -420.822f, 34.185f)),
            //new VehicleRaceCheckpoint(10, new Vector3(-643.114f, -608.695f, 32.712f)),
            //new VehicleRaceCheckpoint(11, new Vector3(-643.173f, -774.485f, 24.810f)),
            //new VehicleRaceCheckpoint(12, new Vector3(-646.130f, -898.869f, 24.055f)),
            //new VehicleRaceCheckpoint(13, new Vector3(-686.968f, -1058.323f, 14.648f)),
            //new VehicleRaceCheckpoint(14, new Vector3(-830.704f, -1143.813f, 6.901f)),
            //new VehicleRaceCheckpoint(15, new Vector3(-991.818f, -1236.503f, 4.989f)),
            //new VehicleRaceCheckpoint(16, new Vector3(-1148.168f, -1305.365f, 4.556f)),
            //new VehicleRaceCheckpoint(17, new Vector3(-1206.281f, -1198.140f, 7.116f)),
            //new VehicleRaceCheckpoint(18, new Vector3(-1258.249f, -1035.073f, 8.188f)),
            //new VehicleRaceCheckpoint(19, new Vector3(-1292.218f, -882.530f, 10.880f)),
            //new VehicleRaceCheckpoint(20, new Vector3(-1401.528f, -773.708f, 20.428f)),
            //new VehicleRaceCheckpoint(21, new Vector3(-1515.632f, -683.794f, 27.852f)),

            new VehicleRaceCheckpoint(0, new Vector3(-1676.250f, -552.500f, 34.125f)),
            new VehicleRaceCheckpoint(1, new Vector3(-1744.500f, -476.250f, 39.406f)),
            new VehicleRaceCheckpoint(2, new Vector3(-1711.000f, -406.500f, 44.438f)),
            new VehicleRaceCheckpoint(3, new Vector3(-1601.250f, -241.250f, 53.156f)),
            new VehicleRaceCheckpoint(4, new Vector3(-1498.250f, -138.500f, 51.375f)),
            new VehicleRaceCheckpoint(5, new Vector3(-1314.750f, -66.250f, 47.438f)),
            new VehicleRaceCheckpoint(6, new Vector3(-1163.000f, -138.750f, 38.688f)),
            new VehicleRaceCheckpoint(7, new Vector3(-912.250f, -265.750f, 39.594f)),
            new VehicleRaceCheckpoint(8, new Vector3(-703.750f, -362.750f, 33.469f)),
            new VehicleRaceCheckpoint(9, new Vector3(-640.667f, -418.750f, 33.816f)),
            new VehicleRaceCheckpoint(10, new Vector3(-642.750f, -609.500f, 32.250f)),
            new VehicleRaceCheckpoint(11, new Vector3(-643.156f, -747.826f, 25.637f)),
            new VehicleRaceCheckpoint(12, new Vector3(-645.750f, -902.000f, 23.625f)),
            new VehicleRaceCheckpoint(13, new Vector3(-692.750f, -1062.000f, 13.750f)),
            new VehicleRaceCheckpoint(14, new Vector3(-853.000f, -1156.750f, 5.063f)),
            new VehicleRaceCheckpoint(15, new Vector3(-1045.000f, -1267.000f, 5.344f)),
            new VehicleRaceCheckpoint(16, new Vector3(-1148.750f, -1303.250f, 4.156f)),
            new VehicleRaceCheckpoint(17, new Vector3(-1206.113f, -1197.699f, 6.736f)),
            new VehicleRaceCheckpoint(18, new Vector3(-1260.500f, -1037.750f, 7.750f)),
            new VehicleRaceCheckpoint(19, new Vector3(-1293.250f, -880.750f, 10.563f)),
            new VehicleRaceCheckpoint(20, new Vector3(-1394.526f, -779.538f, 19.247f)),
            new VehicleRaceCheckpoint(21, new Vector3(-1516.948f, -682.971f, 27.563f)),


        };
        VehicleRaceTrack delperroloop = new VehicleRaceTrack("delperroloop", "Circuit - Del Perro", "Race around the Upper Class area of LS", delperroloopcheckpoints, delperroloopstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(delperroloop);

        List<VehicleRaceStartingPosition> downtownstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-564.620f, -662.780f, 32.276f), 270.199f),
            new VehicleRaceStartingPosition(1, new Vector3(-564.608f, -666.280f, 32.220f), 270.199f),
            new VehicleRaceStartingPosition(2, new Vector3(-572.620f, -662.807f, 32.152f), 270.199f),
            new VehicleRaceStartingPosition(3, new Vector3(-572.608f, -666.307f, 32.086f), 270.199f),
            new VehicleRaceStartingPosition(4, new Vector3(-580.620f, -662.835f, 31.821f), 270.199f),
            new VehicleRaceStartingPosition(5, new Vector3(-580.608f, -666.335f, 31.763f), 270.199f),
            new VehicleRaceStartingPosition(6, new Vector3(-588.620f, -662.863f, 31.458f), 270.199f),
            new VehicleRaceStartingPosition(7, new Vector3(-588.608f, -666.363f, 31.410f), 270.199f),
        };
        List<VehicleRaceCheckpoint> downtowncheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-362.454f, -664.050f, 30.637f)),
            new VehicleRaceCheckpoint(1, new Vector3(-93.750f, -731.750f, 33.531f)),
            new VehicleRaceCheckpoint(2, new Vector3(105.000f, -802.250f, 30.438f)),
            new VehicleRaceCheckpoint(3, new Vector3(145.500f, -868.000f, 29.688f)),
            new VehicleRaceCheckpoint(4, new Vector3(57.912f, -1109.039f, 28.410f)),
            new VehicleRaceCheckpoint(5, new Vector3(68.250f, -1344.250f, 28.281f)),
            new VehicleRaceCheckpoint(6, new Vector3(-62.250f, -1364.250f, 28.375f)),
            new VehicleRaceCheckpoint(7, new Vector3(-253.250f, -1422.500f, 30.313f)),
            new VehicleRaceCheckpoint(8, new Vector3(-479.750f, -1406.500f, 28.469f)),
            new VehicleRaceCheckpoint(9, new Vector3(-495.558f, -1282.375f, 25.658f)),
            new VehicleRaceCheckpoint(10, new Vector3(-516.500f, -1137.500f, 19.188f)),
            new VehicleRaceCheckpoint(11, new Vector3(-531.355f, -1037.748f, 21.758f)),
            new VehicleRaceCheckpoint(12, new Vector3(-495.024f, -805.616f, 29.569f)),
            new VehicleRaceCheckpoint(13, new Vector3(-539.761f, -712.444f, 32.251f)),
            new VehicleRaceCheckpoint(14, new Vector3(-493.333f, -663.556f, 31.960f)),
        };
        VehicleRaceTrack downtown = new VehicleRaceTrack("Downtown1", "Circuit - Little Seoul", "Race around Little Seoul", downtowncheckpoints, downtownstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(downtown);

        List<VehicleRaceStartingPosition> strawberrylsiastart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(84.541f, -1132.721f, 28.213f), 89.109f),
            new VehicleRaceStartingPosition(1, new Vector3(84.580f, -1130.221f, 28.309f), 89.109f),
            new VehicleRaceStartingPosition(2, new Vector3(92.540f, -1132.845f, 28.214f), 89.109f),
            new VehicleRaceStartingPosition(3, new Vector3(92.579f, -1130.345f, 28.315f), 89.109f),
            new VehicleRaceStartingPosition(4, new Vector3(100.539f, -1132.969f, 28.209f), 89.109f),
            new VehicleRaceStartingPosition(5, new Vector3(100.578f, -1130.470f, 28.318f), 89.109f),
            new VehicleRaceStartingPosition(6, new Vector3(108.538f, -1133.094f, 28.207f), 89.109f),
            new VehicleRaceStartingPosition(7, new Vector3(108.577f, -1130.594f, 28.306f), 89.109f),
        };
        List<VehicleRaceCheckpoint> strawberrylsiacheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-139.750f, -1132.250f, 23.781f)),
            new VehicleRaceCheckpoint(1, new Vector3(-337.257f, -1131.688f, 25.849f)),
            new VehicleRaceCheckpoint(2, new Vector3(-541.935f, -1117.725f, 20.643f)),
            new VehicleRaceCheckpoint(3, new Vector3(-617.392f, -1271.998f, 9.870f)),
            new VehicleRaceCheckpoint(4, new Vector3(-696.267f, -1484.278f, 10.034f)),
            new VehicleRaceCheckpoint(5, new Vector3(-827.499f, -1681.603f, 16.996f)),
            new VehicleRaceCheckpoint(6, new Vector3(-1008.313f, -1865.555f, 15.586f)),
            new VehicleRaceCheckpoint(7, new Vector3(-1048.276f, -2069.163f, 12.195f)),
            new VehicleRaceCheckpoint(8, new Vector3(-925.549f, -2121.981f, 8.323f)),
            new VehicleRaceCheckpoint(9, new Vector3(-793.958f, -1990.534f, 8.136f)),
            new VehicleRaceCheckpoint(10, new Vector3(-615.262f, -2018.648f, 5.209f)),
            new VehicleRaceCheckpoint(11, new Vector3(-410.487f, -2157.834f, 9.297f)),
            new VehicleRaceCheckpoint(12, new Vector3(-185.775f, -2190.961f, 9.277f)),
            new VehicleRaceCheckpoint(13, new Vector3(-46.559f, -2141.294f, 9.311f)),
            new VehicleRaceCheckpoint(14, new Vector3(-73.196f, -1965.030f, 17.036f)),
            new VehicleRaceCheckpoint(15, new Vector3(-205.026f, -1807.645f, 28.939f)),
            new VehicleRaceCheckpoint(16, new Vector3(-54.753f, -1723.677f, 28.225f)),
            new VehicleRaceCheckpoint(17, new Vector3(119.513f, -1633.511f, 28.355f)),
            new VehicleRaceCheckpoint(18, new Vector3(140.672f, -1567.116f, 28.329f)),
            new VehicleRaceCheckpoint(19, new Vector3(105.114f, -1537.763f, 28.336f)),
            new VehicleRaceCheckpoint(20, new Vector3(104.782f, -1469.419f, 28.272f)),
            new VehicleRaceCheckpoint(21, new Vector3(137.743f, -1413.071f, 28.305f)),
            new VehicleRaceCheckpoint(22, new Vector3(77.381f, -1303.238f, 28.342f)),
            new VehicleRaceCheckpoint(23, new Vector3(69.326f, -1170.917f, 28.317f)),
            new VehicleRaceCheckpoint(24, new Vector3(-70.000f, -1134.105f, 24.823f)),
        };
        VehicleRaceTrack strawberrylsia = new VehicleRaceTrack("strawberrylsia", "Circuit - Downtown", "Race around Downtown", strawberrylsiacheckpoints, strawberrylsiastart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(strawberrylsia);

        List<VehicleRaceStartingPosition> eastsidecircuitstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(738.125f, -2072.087f, 28.285f), 264.914f),
            new VehicleRaceStartingPosition(1, new Vector3(738.524f, -2067.605f, 28.336f), 264.914f),
            new VehicleRaceStartingPosition(2, new Vector3(728.165f, -2071.201f, 28.279f), 264.914f),
            new VehicleRaceStartingPosition(3, new Vector3(728.564f, -2066.719f, 28.337f), 264.914f),
            new VehicleRaceStartingPosition(4, new Vector3(718.204f, -2070.314f, 28.278f), 264.914f),
            new VehicleRaceStartingPosition(5, new Vector3(718.603f, -2065.832f, 28.337f), 264.914f),
            new VehicleRaceStartingPosition(6, new Vector3(708.244f, -2069.428f, 28.279f), 264.914f),
            new VehicleRaceStartingPosition(7, new Vector3(708.642f, -2064.946f, 28.337f), 264.914f),
        };
        List<VehicleRaceCheckpoint> eastsidecircuitcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(902.495f, -2085.112f, 30.185f)),
            new VehicleRaceCheckpoint(1, new Vector3(1201.152f, -2075.433f, 43.236f)),
            new VehicleRaceCheckpoint(2, new Vector3(1424.770f, -1926.111f, 68.782f)),
            new VehicleRaceCheckpoint(3, new Vector3(1370.577f, -1646.587f, 53.516f)),
            new VehicleRaceCheckpoint(4, new Vector3(1283.761f, -1497.003f, 39.148f)),
            new VehicleRaceCheckpoint(5, new Vector3(1113.002f, -1430.964f, 35.697f)),
            new VehicleRaceCheckpoint(6, new Vector3(914.822f, -1431.196f, 30.673f)),
            new VehicleRaceCheckpoint(7, new Vector3(822.277f, -1536.138f, 28.752f)),
            new VehicleRaceCheckpoint(8, new Vector3(820.964f, -1724.173f, 28.778f)),
            new VehicleRaceCheckpoint(9, new Vector3(799.634f, -1855.705f, 28.809f)),
        };
        VehicleRaceTrack eastsidecircuit = new VehicleRaceTrack("eastsideCircuit", "Circuit - Eastside", "Race around the Eastside of LS", eastsidecircuitcheckpoints, eastsidecircuitstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(eastsidecircuit);
        // ai prone to losing control coming over the hill,no shortcut on a corner causing a significant slow down of ai

        List<VehicleRaceStartingPosition> freewaystart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(780.403f, -1151.822f, 27.933f), 90.855f),
            new VehicleRaceStartingPosition(1, new Vector3(780.328f, -1146.823f, 27.967f), 90.855f),
            new VehicleRaceStartingPosition(2, new Vector3(788.402f, -1151.703f, 27.906f), 90.855f),
            new VehicleRaceStartingPosition(3, new Vector3(788.328f, -1146.703f, 27.923f), 90.855f),
            new VehicleRaceStartingPosition(4, new Vector3(796.401f, -1151.583f, 27.943f), 90.855f),
            new VehicleRaceStartingPosition(5, new Vector3(796.327f, -1146.584f, 27.976f), 90.855f),
            new VehicleRaceStartingPosition(6, new Vector3(804.400f, -1151.464f, 27.842f), 90.855f),
            new VehicleRaceStartingPosition(7, new Vector3(804.326f, -1146.464f, 27.862f), 90.855f),
        };
        List<VehicleRaceCheckpoint> freewaycheckpoints = new List<VehicleRaceCheckpoint>()
        {

            // R setup
            //new VehicleRaceCheckpoint(0, new Vector3(660.2601f, -1154.4073f, 40.6410f)),
            //new VehicleRaceCheckpoint(1, new Vector3(571.5685f, -1178.6960f, 41.2734f)),
            //new VehicleRaceCheckpoint(2, new Vector3(421.5113f, -1184.2124f, 39.7127f)),
            //new VehicleRaceCheckpoint(3, new Vector3(268.5206f, -1176.5718f, 37.1933f)),
            //new VehicleRaceCheckpoint(4, new Vector3(186.8177f, -1162.0254f, 37.1079f)),
            //new VehicleRaceCheckpoint(5, new Vector3(71.0f, -1163.3f, 28.7f)),
            //new VehicleRaceCheckpoint(6, new Vector3(74.9529f, -1267.3383f, 28.1937f)),
            //new VehicleRaceCheckpoint(7, new Vector3(244.1904f, -1226.6989f, 37.3049f)),
            //new VehicleRaceCheckpoint(8, new Vector3(394.5264f, -1224.6189f, 39.1224f)),
            //new VehicleRaceCheckpoint(9, new Vector3(519.6835f, -1228.2539f, 40.9569f)),
            //new VehicleRaceCheckpoint(10, new Vector3(643.6398f, -1244.2620f, 40.9174f)),
            //new VehicleRaceCheckpoint(11, new Vector3(765.5765f, -1242.7665f, 25.9457f)),
            //new VehicleRaceCheckpoint(12, new Vector3(851.6f, -1254.2f, 26.9066f)),
            //new VehicleRaceCheckpoint(13, new Vector3(859.1f, -1326.3f, 37.0286f)),
            //new VehicleRaceCheckpoint(14, new Vector3(739.5747f, -1347.0132f, 38.9030f)),
            //new VehicleRaceCheckpoint(15, new Vector3(695.9282f, -1292.1365f, 41.1269f)),
            //new VehicleRaceCheckpoint(16, new Vector3(729.7197f, -1235.6235f, 43.9479f)),
            //new VehicleRaceCheckpoint(17, new Vector3(755.8483f, -1195.9965f, 44.0202f)),
            //new VehicleRaceCheckpoint(18, new Vector3(724.6743f, -1156.9841f, 43.7047f)),
            //new VehicleRaceCheckpoint(19, new Vector3(688.6f, -1101.7f, 40.8716f)),
            //new VehicleRaceCheckpoint(20, new Vector3(732.4061f, -1045.3109f, 39.4973f)),
            //new VehicleRaceCheckpoint(21, new Vector3(815.3089f, -1042.0963f, 41.2541f)),
            //new VehicleRaceCheckpoint(22, new Vector3(865.2093f, -1093.3431f, 35.9246f)),
            //new VehicleRaceCheckpoint(23, new Vector3(777.4238f, -1148.4053f, 28.0788f)),


            // Alt Route since R's won't play nice
            new VehicleRaceCheckpoint(0, new Vector3(630.750f, -1161.500f, 40.844f)),
            new VehicleRaceCheckpoint(1, new Vector3(545.250f, -1178.250f, 41.125f)),
            new VehicleRaceCheckpoint(2, new Vector3(425.250f, -1184.250f, 39.781f)),
            new VehicleRaceCheckpoint(3, new Vector3(293.250f, -1182.500f, 37.281f)),
            new VehicleRaceCheckpoint(4, new Vector3(215.250f, -1165.000f, 37.219f)),
            new VehicleRaceCheckpoint(5, new Vector3(118.250f, -1160.000f, 32.188f)),
            new VehicleRaceCheckpoint(6, new Vector3(53.750f, -1233.250f, 28.313f)),
            new VehicleRaceCheckpoint(7, new Vector3(104.000f, -1266.000f, 30.219f)),
            new VehicleRaceCheckpoint(8, new Vector3(251.250f, -1226.000f, 37.281f)),
            new VehicleRaceCheckpoint(9, new Vector3(401.250f, -1224.000f, 39.281f)),
            new VehicleRaceCheckpoint(10, new Vector3(664.250f, -1224.500f, 41.563f)),
            new VehicleRaceCheckpoint(11, new Vector3(834.750f, -1217.500f, 44.531f)),
            new VehicleRaceCheckpoint(12, new Vector3(964.500f, -1230.000f, 41.313f)),
            new VehicleRaceCheckpoint(13, new Vector3(1038.500f, -1369.000f, 30.688f)),
            new VehicleRaceCheckpoint(14, new Vector3(1041.500f, -1586.000f, 27.844f)),
            new VehicleRaceCheckpoint(15, new Vector3(1055.000f, -1706.000f, 33.781f)),
            new VehicleRaceCheckpoint(16, new Vector3(1107.000f, -1742.750f, 34.688f)),
            new VehicleRaceCheckpoint(17, new Vector3(1119.500f, -1673.000f, 32.750f)),
            new VehicleRaceCheckpoint(18, new Vector3(1091.500f, -1563.250f, 27.313f)),
            new VehicleRaceCheckpoint(19, new Vector3(1086.250f, -1472.750f, 27.563f)),
            new VehicleRaceCheckpoint(20, new Vector3(1087.000f, -1341.250f, 33.063f)),
            new VehicleRaceCheckpoint(21, new Vector3(1050.750f, -1207.000f, 34.594f)),
            new VehicleRaceCheckpoint(22, new Vector3(898.750f, -1165.750f, 41.000f)),
            new VehicleRaceCheckpoint(23, new Vector3(719.750f, -1154.750f, 43.594f)),
            new VehicleRaceCheckpoint(24, new Vector3(688.750f, -1100.500f, 40.844f)),
            new VehicleRaceCheckpoint(25, new Vector3(777.000f, -1042.750f, 40.438f)),
            new VehicleRaceCheckpoint(26, new Vector3(863.250f, -1105.500f, 34.188f)),
            new VehicleRaceCheckpoint(27, new Vector3(767.500f, -1148.750f, 28.125f)),

        };
        VehicleRaceTrack freeway = new VehicleRaceTrack("freeway", "Circuit - Freeway", "Race Around LS Freeway", freewaycheckpoints, freewaystart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(freeway); // R Conversion (Altered)

        List<VehicleRaceStartingPosition> elyinnerloopracestart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(1219.694f, -2040.624f, 43.247f), 200.867f),
            new VehicleRaceStartingPosition(1, new Vector3(1223.899f, -2039.021f, 43.359f), 200.867f),
            new VehicleRaceStartingPosition(2, new Vector3(1216.132f, -2031.280f, 43.193f), 200.867f),
            new VehicleRaceStartingPosition(3, new Vector3(1220.337f, -2029.677f, 43.300f), 200.867f),
            new VehicleRaceStartingPosition(4, new Vector3(1212.570f, -2021.936f, 43.132f), 200.867f),
            new VehicleRaceStartingPosition(5, new Vector3(1216.775f, -2020.333f, 43.228f), 200.867f),
            new VehicleRaceStartingPosition(6, new Vector3(1209.008f, -2012.592f, 42.876f), 200.867f),
            new VehicleRaceStartingPosition(7, new Vector3(1213.213f, -2010.989f, 42.964f), 200.867f),
        };
        List<VehicleRaceCheckpoint> elyinnerloopcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(1313.554f, -2266.942f, 51.223f)),
            new VehicleRaceCheckpoint(1, new Vector3(1171.941f, -2548.154f, 34.421f)),
            new VehicleRaceCheckpoint(2, new Vector3(587.644f, -2656.130f, 42.081f)),
            new VehicleRaceCheckpoint(3, new Vector3(160.177f, -2650.540f, 18.123f)),
            new VehicleRaceCheckpoint(4, new Vector3(-179.086f, -2481.369f, 50.810f)),
            new VehicleRaceCheckpoint(5, new Vector3(-473.321f, -2275.368f, 61.799f)),
            new VehicleRaceCheckpoint(6, new Vector3(-746.785f, -2083.715f, 35.268f)),
            new VehicleRaceCheckpoint(7, new Vector3(-912.182f, -1890.140f, 29.900f)),
            new VehicleRaceCheckpoint(8, new Vector3(-794.279f, -1746.105f, 38.573f)),
            new VehicleRaceCheckpoint(9, new Vector3(-450.455f, -1619.040f, 38.645f)),
            new VehicleRaceCheckpoint(10, new Vector3(-386.593f, -1369.009f, 36.474f)),
            new VehicleRaceCheckpoint(11, new Vector3(-72.344f, -1235.352f, 36.616f)),
            new VehicleRaceCheckpoint(12, new Vector3(280.197f, -1226.321f, 37.628f)),
            new VehicleRaceCheckpoint(13, new Vector3(713.174f, -1223.104f, 43.867f)),
            new VehicleRaceCheckpoint(14, new Vector3(944.091f, -1222.530f, 41.751f)),
            new VehicleRaceCheckpoint(15, new Vector3(1038.979f, -1400.508f, 29.078f)),
            new VehicleRaceCheckpoint(16, new Vector3(1117.636f, -1804.823f, 28.594f)),
            new VehicleRaceCheckpoint(17, new Vector3(1260.596f, -2141.861f, 46.625f)),
        };
        VehicleRaceTrack elyinnerlooprace = new VehicleRaceTrack("ely_freeloop", "Circuit - LS Freeway Inner Short", "Over the Bridge and Back Around", elyinnerloopcheckpoints, elyinnerloopracestart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(elyinnerlooprace);

        List<VehicleRaceStartingPosition> central3start = new List<VehicleRaceStartingPosition>()
        {
            //new VehicleRaceStartingPosition(0, new Vector3(1180.448f, -2556.885f, 34.84329f), 289.645f),
            //new VehicleRaceStartingPosition(1, new Vector3(1171.138f, -2559.885f, 34.25787f), 287.5368f),
            //new VehicleRaceStartingPosition(2, new Vector3(1181.931f, -2560.759f, 34.80701f), 279.2413f),
            //new VehicleRaceStartingPosition(3, new Vector3(1171.136f, -2563.416f, 34.28861f), 286.6383f),

            new VehicleRaceStartingPosition(0, new Vector3(1084.416f, -2585.015f, 32.845f), 280.920f),
            new VehicleRaceStartingPosition(1, new Vector3(1083.563f, -2580.597f, 32.855f), 280.920f),
            new VehicleRaceStartingPosition(2, new Vector3(1074.597f, -2586.909f, 34.130f), 280.920f),
            new VehicleRaceStartingPosition(3, new Vector3(1073.744f, -2582.491f, 34.139f), 280.920f),
            new VehicleRaceStartingPosition(4, new Vector3(1064.778f, -2588.804f, 35.357f), 280.920f),
            new VehicleRaceStartingPosition(5, new Vector3(1063.925f, -2584.385f, 35.365f), 280.920f),
            new VehicleRaceStartingPosition(6, new Vector3(1054.959f, -2590.698f, 36.545f), 280.920f),
            new VehicleRaceStartingPosition(7, new Vector3(1054.107f, -2586.280f, 36.553f), 280.920f),
        };
        List<VehicleRaceCheckpoint> central3checkpoints = new List<VehicleRaceCheckpoint>()
        {
            //new VehicleRaceCheckpoint(0, new Vector3(1247.396f, -2525.508f, 41.12943f)),
            //new VehicleRaceCheckpoint(1, new Vector3(1240.541f, -2021.258f, 43.51336f)),
            //new VehicleRaceCheckpoint(2, new Vector3(1068.375f, -1284.138f, 25.21841f)),
            //new VehicleRaceCheckpoint(3, new Vector3(715.1257f, -598.6426f, 35.34497f)),
            //new VehicleRaceCheckpoint(4, new Vector3(-111.2042f, -495.9614f, 29.78688f)),
            //new VehicleRaceCheckpoint(5, new Vector3(-237.5093f, -477.4375f, 25.16191f)),
            //new VehicleRaceCheckpoint(6, new Vector3(-414.4864f, -694.7096f, 36.58041f)),
            //new VehicleRaceCheckpoint(7, new Vector3(-410.9584f, -1352.381f, 36.67314f)),
            //new VehicleRaceCheckpoint(8, new Vector3(-675.6441f, -1716.931f, 36.8155f)),
            //new VehicleRaceCheckpoint(9, new Vector3(-764.5367f, -2088.595f, 33.70555f)),
            //new VehicleRaceCheckpoint(10, new Vector3(189.1148f, -2666.291f, 17.49789f)),


            new VehicleRaceCheckpoint(0, new Vector3(1327.597f, -2427.613f, 49.625f)),
            //new VehicleRaceCheckpoint(1, new Vector3(1234.920f, -2011.846f, 43.528f)),
            new VehicleRaceCheckpoint(1, new Vector3(1166.987f, -1866.478f, 30.877f)),
            new VehicleRaceCheckpoint(2, new Vector3(1091.047f, -1652.672f, 28.792f)),
            new VehicleRaceCheckpoint(3, new Vector3(1071.511f, -1363.538f, 28.431f)),
            new VehicleRaceCheckpoint(4, new Vector3(1018.357f, -901.596f, 29.853f)),
            new VehicleRaceCheckpoint(5, new Vector3(690.556f, -588.277f, 35.374f)),
            new VehicleRaceCheckpoint(6, new Vector3(384.841f, -498.243f, 34.346f)),
            new VehicleRaceCheckpoint(7, new Vector3(-0.748f, -496.042f, 33.133f)),
            new VehicleRaceCheckpoint(8, new Vector3(-172.361f, -483.356f, 27.390f)),
            new VehicleRaceCheckpoint(9, new Vector3(-318.186f, -476.817f, 32.911f)),
            new VehicleRaceCheckpoint(10, new Vector3(-412.963f, -758.419f, 36.633f)),
            new VehicleRaceCheckpoint(11, new Vector3(-413.164f, -1177.380f, 36.601f)),
            new VehicleRaceCheckpoint(12, new Vector3(-462.470f, -1607.070f, 38.656f)),
            new VehicleRaceCheckpoint(13, new Vector3(-913.739f, -1821.048f, 35.154f)),
            new VehicleRaceCheckpoint(14, new Vector3(-670.355f, -2154.294f, 47.292f)),
            new VehicleRaceCheckpoint(15, new Vector3(-465.309f, -2297.850f, 62.304f)),
            new VehicleRaceCheckpoint(16, new Vector3(-155.647f, -2514.686f, 47.267f)),
            new VehicleRaceCheckpoint(17, new Vector3(159.477f, -2664.617f, 17.998f)),
            new VehicleRaceCheckpoint(18, new Vector3(339.492f, -2676.335f, 19.197f)),
            new VehicleRaceCheckpoint(19, new Vector3(607.086f, -2666.979f, 43.691f)),
            new VehicleRaceCheckpoint(20, new Vector3(1086.766f, -2582.347f, 32.887f)),

        };
        VehicleRaceTrack central3 = new VehicleRaceTrack("centralRace3", "Circuit - LS Freeway Outer Loop", "Race around the Outer Loop of the central LS freeway system", central3checkpoints, central3start);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(central3);

        List<VehicleRaceStartingPosition> midtown1start = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(310.991f, -488.568f, 42.320f), 339.459f),
            new VehicleRaceStartingPosition(1, new Vector3(306.309f, -486.812f, 42.373f), 339.459f),
            new VehicleRaceStartingPosition(2, new Vector3(308.182f, -496.058f, 42.303f), 339.459f),
            new VehicleRaceStartingPosition(3, new Vector3(303.500f, -494.303f, 42.357f), 339.459f),
            new VehicleRaceStartingPosition(4, new Vector3(305.374f, -503.549f, 42.286f), 339.459f),
            new VehicleRaceStartingPosition(5, new Vector3(300.692f, -501.794f, 42.340f), 339.459f),
            new VehicleRaceStartingPosition(6, new Vector3(302.565f, -511.040f, 42.269f), 339.459f),
            new VehicleRaceStartingPosition(7, new Vector3(297.883f, -509.285f, 42.324f), 339.459f),
        };
        List<VehicleRaceCheckpoint> midtown1checkpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(406.000f, -398.250f, 45.875f)),
            new VehicleRaceCheckpoint(1, new Vector3(521.750f, -254.000f, 47.281f)),
            new VehicleRaceCheckpoint(2, new Vector3(584.451f, -82.793f, 68.989f)),
            new VehicleRaceCheckpoint(3, new Vector3(716.196f, 36.983f, 83.255f)),
            new VehicleRaceCheckpoint(4, new Vector3(764.750f, 156.750f, 80.094f)),
            new VehicleRaceCheckpoint(5, new Vector3(713.250f, 196.750f, 87.563f)),
            new VehicleRaceCheckpoint(6, new Vector3(629.250f, 225.250f, 99.031f)),
            new VehicleRaceCheckpoint(7, new Vector3(474.500f, 276.750f, 102.063f)),
            new VehicleRaceCheckpoint(8, new Vector3(388.500f, 212.750f, 102.063f)),
            new VehicleRaceCheckpoint(9, new Vector3(467.250f, 88.750f, 96.906f)),
            new VehicleRaceCheckpoint(10, new Vector3(471.250f, 8.500f, 85.688f)),
            new VehicleRaceCheckpoint(11, new Vector3(391.500f, -97.000f, 65.875f)),
            new VehicleRaceCheckpoint(12, new Vector3(244.250f, -70.000f, 68.781f)),
            new VehicleRaceCheckpoint(13, new Vector3(21.000f, 11.000f, 69.406f)),
            new VehicleRaceCheckpoint(14, new Vector3(-58.500f, -4.250f, 69.625f)),
            new VehicleRaceCheckpoint(15, new Vector3(-91.250f, -127.250f, 56.875f)),
            new VehicleRaceCheckpoint(16, new Vector3(-95.250f, -184.500f, 47.625f)),
            new VehicleRaceCheckpoint(17, new Vector3(-66.878f, -250.313f, 44.294f)),
            new VehicleRaceCheckpoint(18, new Vector3(5.000f, -290.250f, 46.344f)),
            new VehicleRaceCheckpoint(19, new Vector3(-33.500f, -431.750f, 39.344f)),
            new VehicleRaceCheckpoint(20, new Vector3(-79.250f, -571.500f, 36.125f)),
            new VehicleRaceCheckpoint(21, new Vector3(-99.000f, -653.500f, 35.094f)),
            new VehicleRaceCheckpoint(22, new Vector3(-103.500f, -729.000f, 33.750f)),
            new VehicleRaceCheckpoint(23, new Vector3(54.750f, -783.750f, 30.781f)),
            new VehicleRaceCheckpoint(24, new Vector3(127.000f, -801.750f, 30.313f)),
            new VehicleRaceCheckpoint(25, new Vector3(188.750f, -772.250f, 31.313f)),
            new VehicleRaceCheckpoint(26, new Vector3(271.071f, -584.932f, 42.210f)),
        };
        VehicleRaceTrack midtown1 = new VehicleRaceTrack("midtown1", "Circuit - Midtown Madness", "Race around Midtown", midtown1checkpoints, midtown1start);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(midtown1);

        List<VehicleRaceStartingPosition> midtown2start = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-169.187f, -447.739f, 32.988f), 340.737f),
            new VehicleRaceStartingPosition(1, new Vector3(-173.434f, -446.253f, 33.051f), 340.737f),
            new VehicleRaceStartingPosition(2, new Vector3(-172.486f, -457.179f, 33.066f), 340.737f),
            new VehicleRaceStartingPosition(3, new Vector3(-176.733f, -455.693f, 33.135f), 340.737f),
            new VehicleRaceStartingPosition(4, new Vector3(-175.785f, -466.619f, 33.162f), 340.737f),
            new VehicleRaceStartingPosition(5, new Vector3(-180.032f, -465.134f, 33.234f), 340.737f),
            new VehicleRaceStartingPosition(6, new Vector3(-179.084f, -476.059f, 33.277f), 340.737f),
            new VehicleRaceStartingPosition(7, new Vector3(-183.331f, -474.574f, 33.356f), 340.737f),
        };
        List<VehicleRaceCheckpoint> midtown2checkpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-98.500f, -267.500f, 42.375f)),
            new VehicleRaceCheckpoint(1, new Vector3(69.000f, -303.500f, 45.969f)),
            new VehicleRaceCheckpoint(2, new Vector3(240.053f, -363.937f, 43.388f)),
            new VehicleRaceCheckpoint(3, new Vector3(298.497f, -449.597f, 42.553f)),
            new VehicleRaceCheckpoint(4, new Vector3(255.123f, -577.066f, 42.344f)),
            new VehicleRaceCheckpoint(5, new Vector3(214.250f, -612.250f, 40.656f)),
            new VehicleRaceCheckpoint(6, new Vector3(146.750f, -587.500f, 43.063f)),
            new VehicleRaceCheckpoint(7, new Vector3(83.750f, -605.000f, 43.250f)),
            new VehicleRaceCheckpoint(8, new Vector3(44.750f, -712.000f, 43.250f)),
            new VehicleRaceCheckpoint(9, new Vector3(-5.500f, -747.250f, 43.281f)),
            new VehicleRaceCheckpoint(10, new Vector3(-202.500f, -682.750f, 32.875f)),
            new VehicleRaceCheckpoint(11, new Vector3(-237.364f, -624.807f, 32.814f)),
            new VehicleRaceCheckpoint(12, new Vector3(-169.492f, -441.357f, 33.011f)),
        };
        VehicleRaceTrack midtown2 = new VehicleRaceTrack("midtown2", "Circuit - Midtown Madness 2", "Race around Midtown", midtown2checkpoints, midtown2start);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(midtown2);

        List<VehicleRaceStartingPosition> mirrorparkstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(1110.768f, -763.718f, 56.664f), 270.150f),
            new VehicleRaceStartingPosition(1, new Vector3(1110.760f, -760.718f, 56.719f), 270.150f),
            new VehicleRaceStartingPosition(2, new Vector3(1102.768f, -763.739f, 56.628f), 270.150f),
            new VehicleRaceStartingPosition(3, new Vector3(1102.760f, -760.739f, 56.798f), 270.150f),
            new VehicleRaceStartingPosition(4, new Vector3(1094.768f, -763.760f, 56.622f), 270.150f),
            new VehicleRaceStartingPosition(5, new Vector3(1094.760f, -760.760f, 56.791f), 270.150f),
            new VehicleRaceStartingPosition(6, new Vector3(1086.768f, -763.781f, 56.621f), 270.150f),
            new VehicleRaceStartingPosition(7, new Vector3(1086.760f, -760.781f, 56.792f), 270.150f),
        };
        List<VehicleRaceCheckpoint> mirrorparkcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(1242.500f, -753.000f, 59.781f)),
            new VehicleRaceCheckpoint(1, new Vector3(1270.250f, -514.000f, 68.125f)),
            new VehicleRaceCheckpoint(2, new Vector3(1249.000f, -386.000f, 68.125f)),
            new VehicleRaceCheckpoint(3, new Vector3(1168.500f, -363.000f, 66.938f)),
            new VehicleRaceCheckpoint(4, new Vector3(1054.750f, -412.250f, 65.906f)),
            new VehicleRaceCheckpoint(5, new Vector3(914.750f, -506.500f, 57.656f)),
            new VehicleRaceCheckpoint(6, new Vector3(939.250f, -614.000f, 56.500f)),
            new VehicleRaceCheckpoint(7, new Vector3(1006.750f, -713.000f, 56.500f)),
            new VehicleRaceCheckpoint(8, new Vector3(1126.500f, -759.500f, 56.813f)),
        };
        VehicleRaceTrack mirrorpark = new VehicleRaceTrack("mirrorpark", "Circuit - Mirror Park", "Race around Mirror Park", mirrorparkcheckpoints, mirrorparkstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(mirrorpark);

        List<VehicleRaceStartingPosition> mirrorparkrevstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(1141.892f, -754.988f, 56.732f), 90.165f),
            new VehicleRaceStartingPosition(1, new Vector3(1141.901f, -757.988f, 56.770f), 90.165f),
            new VehicleRaceStartingPosition(2, new Vector3(1149.892f, -754.965f, 56.727f), 90.165f),
            new VehicleRaceStartingPosition(3, new Vector3(1149.901f, -757.965f, 56.812f), 90.165f),
            new VehicleRaceStartingPosition(4, new Vector3(1157.892f, -754.942f, 56.747f), 90.165f),
            new VehicleRaceStartingPosition(5, new Vector3(1157.901f, -757.942f, 56.826f), 90.165f),
            new VehicleRaceStartingPosition(6, new Vector3(1165.892f, -754.919f, 56.786f), 90.165f),
            new VehicleRaceStartingPosition(7, new Vector3(1165.901f, -757.919f, 56.849f), 90.165f),
        };
        List<VehicleRaceCheckpoint> mirrorparkrevcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(1014.500f, -722.000f, 56.625f)),
            new VehicleRaceCheckpoint(1, new Vector3(890.250f, -579.250f, 56.375f)),
            new VehicleRaceCheckpoint(2, new Vector3(1007.500f, -449.250f, 63.156f)),
            new VehicleRaceCheckpoint(3, new Vector3(1116.000f, -363.000f, 66.094f)),
            new VehicleRaceCheckpoint(4, new Vector3(1249.000f, -386.000f, 68.125f)),
            new VehicleRaceCheckpoint(5, new Vector3(1262.250f, -569.750f, 68.063f)),
            new VehicleRaceCheckpoint(6, new Vector3(1283.250f, -707.750f, 63.813f)),
            new VehicleRaceCheckpoint(7, new Vector3(1153.250f, -759.250f, 56.813f)),
        };
        VehicleRaceTrack mirrorparkrev = new VehicleRaceTrack("mirrorparkrev", "Circuit - Mirror Park Reverse", "Race around Mirror Park", mirrorparkrevcheckpoints, mirrorparkrevstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(mirrorparkrev);

        List<VehicleRaceStartingPosition> missionrow1start = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(497.333f, -810.129f, 23.779f), 176.944f),
            new VehicleRaceStartingPosition(1, new Vector3(500.329f, -810.289f, 23.903f), 176.944f),
            new VehicleRaceStartingPosition(2, new Vector3(497.766f, -802.141f, 23.784f), 176.944f),
            new VehicleRaceStartingPosition(3, new Vector3(500.761f, -802.301f, 23.902f), 176.944f),
            new VehicleRaceStartingPosition(4, new Vector3(498.198f, -794.152f, 23.785f), 176.944f),
            new VehicleRaceStartingPosition(5, new Vector3(501.194f, -794.313f, 23.899f), 176.944f),
            new VehicleRaceStartingPosition(6, new Vector3(498.631f, -786.164f, 23.693f), 176.944f),
            new VehicleRaceStartingPosition(7, new Vector3(501.626f, -786.324f, 23.896f), 176.944f),
        };
        List<VehicleRaceCheckpoint> missionrow1checkpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(500.750f, -990.000f, 26.688f)),
            new VehicleRaceCheckpoint(1, new Vector3(502.250f, -1233.500f, 28.313f)),
            new VehicleRaceCheckpoint(2, new Vector3(535.750f, -1388.500f, 28.313f)),
            new VehicleRaceCheckpoint(3, new Vector3(510.500f, -1536.500f, 28.250f)),
            new VehicleRaceCheckpoint(4, new Vector3(406.000f, -1685.000f, 28.313f)),
            new VehicleRaceCheckpoint(5, new Vector3(303.000f, -1804.250f, 26.500f)),
            new VehicleRaceCheckpoint(6, new Vector3(228.500f, -1810.000f, 26.625f)),
            new VehicleRaceCheckpoint(7, new Vector3(110.750f, -1708.500f, 28.188f)),
            new VehicleRaceCheckpoint(8, new Vector3(108.500f, -1639.250f, 28.313f)),
            new VehicleRaceCheckpoint(9, new Vector3(217.500f, -1576.250f, 28.313f)),
            new VehicleRaceCheckpoint(10, new Vector3(277.750f, -1537.250f, 28.313f)),
            new VehicleRaceCheckpoint(11, new Vector3(272.000f, -1463.000f, 28.313f)),
            new VehicleRaceCheckpoint(12, new Vector3(181.750f, -1392.750f, 28.281f)),
            new VehicleRaceCheckpoint(13, new Vector3(229.500f, -1223.750f, 28.313f)),
            new VehicleRaceCheckpoint(14, new Vector3(245.250f, -998.250f, 28.313f)),
            new VehicleRaceCheckpoint(15, new Vector3(340.626f, -729.179f, 28.331f)),
            new VehicleRaceCheckpoint(16, new Vector3(444.750f, -677.000f, 27.594f)),
            new VehicleRaceCheckpoint(17, new Vector3(505.000f, -738.750f, 23.906f)),
            new VehicleRaceCheckpoint(18, new Vector3(500.750f, -871.750f, 24.344f)),
        };
        VehicleRaceTrack missionrow1 = new VehicleRaceTrack("missionrow", "Circuit - Mission Row", "Race around Mission Row", missionrow1checkpoints, missionrow1start);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(missionrow1);

        List<VehicleRaceStartingPosition> rockydelperrostart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-227.577f, -45.615f, 48.545f), 70.832f),
            new VehicleRaceStartingPosition(1, new Vector3(-228.892f, -49.393f, 48.587f), 70.832f),
            new VehicleRaceStartingPosition(2, new Vector3(-220.020f, -48.241f, 48.835f), 70.832f),
            new VehicleRaceStartingPosition(3, new Vector3(-221.335f, -52.019f, 48.871f), 70.832f),
            new VehicleRaceStartingPosition(4, new Vector3(-212.463f, -50.866f, 49.199f), 70.832f),
            new VehicleRaceStartingPosition(5, new Vector3(-213.778f, -54.644f, 49.242f), 70.832f),
            new VehicleRaceStartingPosition(6, new Vector3(-204.906f, -53.492f, 49.606f), 70.832f),
            new VehicleRaceStartingPosition(7, new Vector3(-206.221f, -57.270f, 49.647f), 70.832f),
        };
        List<VehicleRaceCheckpoint> rockydelperrocheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-470.500f, 11.000f, 44.500f)),
            new VehicleRaceCheckpoint(1, new Vector3(-747.000f, -38.250f, 36.875f)),
            new VehicleRaceCheckpoint(2, new Vector3(-986.500f, -163.000f, 36.906f)),
            new VehicleRaceCheckpoint(3, new Vector3(-1237.750f, -303.750f, 36.531f)),
            new VehicleRaceCheckpoint(4, new Vector3(-1425.000f, -408.500f, 35.188f)),
            new VehicleRaceCheckpoint(5, new Vector3(-1620.250f, -538.000f, 33.469f)),
            new VehicleRaceCheckpoint(6, new Vector3(-1685.500f, -545.750f, 35.125f)),
            new VehicleRaceCheckpoint(7, new Vector3(-1703.500f, -397.250f, 45.531f)),
            new VehicleRaceCheckpoint(8, new Vector3(-1601.250f, -241.250f, 53.156f)),
            new VehicleRaceCheckpoint(9, new Vector3(-1489.000f, -130.750f, 50.625f)),
            new VehicleRaceCheckpoint(10, new Vector3(-1366.500f, -65.500f, 50.813f)),
            new VehicleRaceCheckpoint(11, new Vector3(-1125.500f, -158.250f, 37.875f)),
            new VehicleRaceCheckpoint(12, new Vector3(-959.250f, -241.500f, 37.094f)),
            new VehicleRaceCheckpoint(13, new Vector3(-748.250f, -345.750f, 34.844f)),
            new VehicleRaceCheckpoint(14, new Vector3(-578.750f, -376.750f, 33.875f)),
            new VehicleRaceCheckpoint(15, new Vector3(-435.500f, -389.250f, 32.094f)),
            new VehicleRaceCheckpoint(16, new Vector3(-242.000f, -410.750f, 29.281f)),
            new VehicleRaceCheckpoint(17, new Vector3(-140.500f, -349.750f, 34.563f)),
            new VehicleRaceCheckpoint(18, new Vector3(-88.000f, -192.000f, 46.656f)),
            new VehicleRaceCheckpoint(19, new Vector3(-117.500f, -86.750f, 55.688f)),
            new VehicleRaceCheckpoint(20, new Vector3(-229.250f, -47.250f, 48.531f)),
        };
        VehicleRaceTrack rockydelperro = new VehicleRaceTrack("rockydelperro", "Circuit - Rocky Del Perro", "Race Through Rockford & Del Perro", rockydelperrocheckpoints, rockydelperrostart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(rockydelperro);

        List<VehicleRaceStartingPosition> southlossantosstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-95.547f, -1495.488f, 32.262f), 140.304f),
            new VehicleRaceStartingPosition(1, new Vector3(-91.700f, -1498.682f, 32.398f), 140.304f),
            new VehicleRaceStartingPosition(2, new Vector3(-90.437f, -1489.332f, 31.927f), 140.304f),
            new VehicleRaceStartingPosition(3, new Vector3(-86.590f, -1492.526f, 31.920f), 140.304f),
            new VehicleRaceStartingPosition(4, new Vector3(-85.328f, -1483.177f, 31.422f), 140.304f),
            new VehicleRaceStartingPosition(5, new Vector3(-81.480f, -1486.370f, 31.440f), 140.304f),
            new VehicleRaceStartingPosition(6, new Vector3(-80.218f, -1477.021f, 31.138f), 140.304f),
            new VehicleRaceStartingPosition(7, new Vector3(-76.371f, -1480.215f, 31.187f), 140.304f),
        };
        List<VehicleRaceCheckpoint> southlossantoscheckpoints = new List<VehicleRaceCheckpoint>()
        {

            //new VehicleRaceCheckpoint(0, new Vector3(-168.750f, -1583.500f, 33.938f)),
            //new VehicleRaceCheckpoint(1, new Vector3(-183.000f, -1681.250f, 32.438f)),
            //new VehicleRaceCheckpoint(2, new Vector3(-67.500f, -1795.250f, 26.844f)),
            //new VehicleRaceCheckpoint(3, new Vector3(10.250f, -1860.750f, 22.875f)),
            //new VehicleRaceCheckpoint(4, new Vector3(108.500f, -1865.000f, 23.438f)),
            //new VehicleRaceCheckpoint(5, new Vector3(244.750f, -1919.500f, 23.781f)),
            //new VehicleRaceCheckpoint(6, new Vector3(287.000f, -1883.000f, 25.969f)),
            //new VehicleRaceCheckpoint(7, new Vector3(413.250f, -1947.250f, 23.219f)),
            //new VehicleRaceCheckpoint(8, new Vector3(547.500f, -1884.250f, 24.469f)),
            //new VehicleRaceCheckpoint(9, new Vector3(600.250f, -1701.250f, 21.750f)),
            //new VehicleRaceCheckpoint(10, new Vector3(602.750f, -1593.000f, 25.656f)),
            //new VehicleRaceCheckpoint(11, new Vector3(480.500f, -1429.250f, 28.313f)),
            //new VehicleRaceCheckpoint(12, new Vector3(405.500f, -1457.500f, 28.313f)),
            //new VehicleRaceCheckpoint(13, new Vector3(287.750f, -1523.500f, 28.281f)),
            //new VehicleRaceCheckpoint(14, new Vector3(217.500f, -1564.000f, 28.281f)),
            //new VehicleRaceCheckpoint(15, new Vector3(107.750f, -1543.250f, 28.375f)),
            //new VehicleRaceCheckpoint(16, new Vector3(19.000f, -1468.750f, 29.594f)),
            //new VehicleRaceCheckpoint(17, new Vector3(-79.750f, -1481.250f, 31.281f)),
            //new VehicleRaceCheckpoint(18, new Vector3(-135.250f, -1542.750f, 33.313f)),



            new VehicleRaceCheckpoint(0, new Vector3(-177.4019f, -1594.6443f, 33.4994f)),
            new VehicleRaceCheckpoint(1, new Vector3(-181.9884f, -1688.1002f, 32.2323f)),
            new VehicleRaceCheckpoint(2, new Vector3(-68.2538f, -1793.9515f, 26.8799f)),
            new VehicleRaceCheckpoint(3, new Vector3(10.250f, -1860.750f, 22.875f)),
            new VehicleRaceCheckpoint(4, new Vector3(102.9353f, -1868.2292f, 23.1889f)),
            new VehicleRaceCheckpoint(5, new Vector3(236.2683f, -1924.7115f, 23.2964f)),
            new VehicleRaceCheckpoint(6, new Vector3(287.2185f, -1884.0795f, 25.9619f)),
            new VehicleRaceCheckpoint(7, new Vector3(397.2169f, -1948.5200f, 23.3175f)),
            new VehicleRaceCheckpoint(8, new Vector3(552.7326f, -1879.7982f, 24.4769f)),
            new VehicleRaceCheckpoint(9, new Vector3(599.2975f, -1699.4381f, 21.9581f)),
            new VehicleRaceCheckpoint(10, new Vector3(603.7488f, -1595.4388f, 25.5479f)),
            new VehicleRaceCheckpoint(11, new Vector3(514.6337f, -1443.9539f, 28.3433f)),
            new VehicleRaceCheckpoint(12, new Vector3(471.4342f, -1433.9247f, 28.3421f)),
            new VehicleRaceCheckpoint(13, new Vector3(293.2f, -1521.2f, 28.3415f)),
            new VehicleRaceCheckpoint(14, new Vector3(217.500f, -1564.000f, 28.281f)),
            new VehicleRaceCheckpoint(15, new Vector3(40.250f, -1486.500f, 28.344f)),
            new VehicleRaceCheckpoint(16, new Vector3(-89.000f, -1492.250f, 32.063f)),
            new VehicleRaceCheckpoint(17, new Vector3(-155.8476f, -1566.9800f, 34.0001f)),
            //new VehicleRaceCheckpoint(15, new Vector3(16.9636f, -1465.6825f, 29.5632f)),
            //new VehicleRaceCheckpoint(16, new Vector3(-73.9895f, -1476.3289f, 31.1385f)),
            //new VehicleRaceCheckpoint(17, new Vector3(-155.8476f, -1566.9800f, 34.0001f)),
        };
        VehicleRaceTrack southlossantos = new VehicleRaceTrack("southlossantos", "Circuit - South Los Santos", "Race Around South Los Santos", southlossantoscheckpoints, southlossantosstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(southlossantos); // R Conversion

        List<VehicleRaceStartingPosition> southshamblesststart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(1041.074f, -2303.690f, 29.484f), 175.475f),
            new VehicleRaceStartingPosition(1, new Vector3(1043.566f, -2303.888f, 29.524f), 175.475f),
            new VehicleRaceStartingPosition(2, new Vector3(1041.704f, -2295.715f, 29.484f), 175.475f),
            new VehicleRaceStartingPosition(3, new Vector3(1044.197f, -2295.913f, 29.527f), 175.475f),
            new VehicleRaceStartingPosition(4, new Vector3(1042.335f, -2287.740f, 29.485f), 175.475f),
            new VehicleRaceStartingPosition(5, new Vector3(1044.827f, -2287.938f, 29.530f), 175.475f),
            new VehicleRaceStartingPosition(6, new Vector3(1042.965f, -2279.765f, 29.491f), 175.475f),
            new VehicleRaceStartingPosition(7, new Vector3(1045.458f, -2279.962f, 29.529f), 175.475f),
        };
        List<VehicleRaceCheckpoint> southshamblesstcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(1034.750f, -2422.750f, 28.000f)),
            new VehicleRaceCheckpoint(1, new Vector3(940.000f, -2463.250f, 27.594f)),
            new VehicleRaceCheckpoint(2, new Vector3(905.250f, -2390.500f, 29.219f)),
            new VehicleRaceCheckpoint(3, new Vector3(923.500f, -2159.000f, 29.250f)),
            new VehicleRaceCheckpoint(4, new Vector3(936.000f, -2033.500f, 29.281f)),
            new VehicleRaceCheckpoint(5, new Vector3(954.000f, -1815.250f, 30.313f)),
            new VehicleRaceCheckpoint(6, new Vector3(906.500f, -1759.500f, 29.594f)),
            new VehicleRaceCheckpoint(7, new Vector3(810.500f, -1787.000f, 28.344f)),
            new VehicleRaceCheckpoint(8, new Vector3(772.250f, -1988.000f, 28.344f)),
            new VehicleRaceCheckpoint(9, new Vector3(834.250f, -2078.500f, 28.844f)),
            new VehicleRaceCheckpoint(10, new Vector3(1051.250f, -2097.000f, 30.969f)),
            new VehicleRaceCheckpoint(11, new Vector3(1048.750f, -2261.250f, 29.531f)),
        };
        VehicleRaceTrack southshamblesst = new VehicleRaceTrack("south_shambles_st", "Circuit - South Shambles St", "Watch out for the crossover", southshamblesstcheckpoints, southshamblesststart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(southshamblesst);

        List<VehicleRaceStartingPosition> canalsstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-1289.399f, -990.018f, 1.206f), 269.536f),
            new VehicleRaceStartingPosition(1, new Vector3(-1289.378f, -987.518f, 1.273f), 269.536f),
            new VehicleRaceStartingPosition(2, new Vector3(-1297.398f, -989.953f, 2.458f), 269.536f),
            new VehicleRaceStartingPosition(3, new Vector3(-1297.378f, -987.453f, 2.559f), 269.536f),
            new VehicleRaceStartingPosition(4, new Vector3(-1305.398f, -989.888f, 3.592f), 269.536f),
            new VehicleRaceStartingPosition(5, new Vector3(-1305.378f, -987.389f, 3.721f), 269.536f),
            new VehicleRaceStartingPosition(6, new Vector3(-1313.398f, -989.824f, 4.847f), 269.536f),
            new VehicleRaceStartingPosition(7, new Vector3(-1313.377f, -987.324f, 4.937f), 269.536f),
        };
        List<VehicleRaceCheckpoint> canalscheckpoints = new List<VehicleRaceCheckpoint>()
        {
            // R Conversion uses area without nodes for vehicles to cross (canal bridges)
            //new VehicleRaceCheckpoint(0, new Vector3(-925.4882f, -1072.8036f, 1.1502f)),
            //new VehicleRaceCheckpoint(1, new Vector3(-838.9020f, -1020.6968f, 12.2795f)),
            //new VehicleRaceCheckpoint(2, new Vector3(-752.4019f, -968.1661f, 15.5933f)),
            //new VehicleRaceCheckpoint(3, new Vector3(-660.0514f, -959.4323f, 20.3359f)),
            //new VehicleRaceCheckpoint(4, new Vector3(-643.1226f, -996.0269f, 19.6655f)),
            //new VehicleRaceCheckpoint(5, new Vector3(-755.0204f, -1100.5715f, 9.7347f)),
            //new VehicleRaceCheckpoint(6, new Vector3(-844.4481f, -1152.1702f, 5.5481f)),
            //new VehicleRaceCheckpoint(7, new Vector3(-934.6541f, -1204.1019f, 4.1492f)),
            //new VehicleRaceCheckpoint(8, new Vector3(-968.2922f, -1181.9056f, 2.9090f)),
            //new VehicleRaceCheckpoint(9, new Vector3(-1023.3292f, -1087.9523f, 1.0397f)),
            //new VehicleRaceCheckpoint(10, new Vector3(-1078.6326f, -994.3664f, 1.2191f)),
            //new VehicleRaceCheckpoint(11, new Vector3(-1170.7820f, -837.8701f, 13.2119f)),
            //new VehicleRaceCheckpoint(12, new Vector3(-1312.0331f, -658.3402f, 25.5365f)),
            //new VehicleRaceCheckpoint(13, new Vector3(-1375.9105f, -560.5161f, 29.2340f)),
            //new VehicleRaceCheckpoint(14, new Vector3(-1446.2238f, -460.4871f, 34.1264f)),
            //new VehicleRaceCheckpoint(15, new Vector3(-1487.2517f, -447.6395f, 34.5940f)),
            //new VehicleRaceCheckpoint(16, new Vector3(-1638.2157f, -562.0701f, 32.4537f)),
            //new VehicleRaceCheckpoint(17, new Vector3(-1625.8223f, -610.9568f, 31.6803f)),
            //new VehicleRaceCheckpoint(18, new Vector3(-1531.3903f, -684.8500f, 27.8725f)),
            //new VehicleRaceCheckpoint(19, new Vector3(-1424.3597f, -771.3326f, 21.8328f)),
            //new VehicleRaceCheckpoint(20, new Vector3(-1300.4484f, -901.4435f, 10.3951f)),
            //new VehicleRaceCheckpoint(21, new Vector3(-1257.0120f, -1048.0339f, 7.5077f)),
            //new VehicleRaceCheckpoint(22, new Vector3(-1211.1119f, -1198.4872f, 6.7558f)),
            //new VehicleRaceCheckpoint(23, new Vector3(-1104.3783f, -1177.5079f, 1.2615f)),
            //new VehicleRaceCheckpoint(24, new Vector3(-1062.9856f, -1153.3645f, 1.1295f)),

            // Alt route 1
            //new VehicleRaceCheckpoint(0, new Vector3(-1059.250f, -1028.000f, 1.188f)),
            //new VehicleRaceCheckpoint(1, new Vector3(-979.500f, -1166.000f, 3.125f)),
            //new VehicleRaceCheckpoint(2, new Vector3(-907.250f, -1206.750f, 4.000f)),
            //new VehicleRaceCheckpoint(3, new Vector3(-802.000f, -1137.250f, 8.688f)),
            //new VehicleRaceCheckpoint(4, new Vector3(-787.000f, -1083.250f, 10.000f)),
            //new VehicleRaceCheckpoint(5, new Vector3(-813.250f, -1036.000f, 12.188f)),
            //new VehicleRaceCheckpoint(6, new Vector3(-750.750f, -967.500f, 15.688f)),
            //new VehicleRaceCheckpoint(7, new Vector3(-678.750f, -957.000f, 19.813f)),
            //new VehicleRaceCheckpoint(8, new Vector3(-646.250f, -1000.000f, 19.406f)),
            //new VehicleRaceCheckpoint(9, new Vector3(-737.250f, -1089.750f, 10.438f)),
            //new VehicleRaceCheckpoint(10, new Vector3(-841.000f, -1149.750f, 5.781f)),
            //new VehicleRaceCheckpoint(11, new Vector3(-920.500f, -1195.750f, 4.031f)),
            //new VehicleRaceCheckpoint(12, new Vector3(-976.500f, -1171.250f, 3.063f)),
            //new VehicleRaceCheckpoint(13, new Vector3(-1075.750f, -999.250f, 1.188f)),
            //new VehicleRaceCheckpoint(14, new Vector3(-1140.750f, -890.750f, 6.438f)),
            //new VehicleRaceCheckpoint(15, new Vector3(-1119.250f, -795.250f, 16.594f)),
            //new VehicleRaceCheckpoint(16, new Vector3(-1111.250f, -714.250f, 19.531f)),
            //new VehicleRaceCheckpoint(17, new Vector3(-1204.750f, -616.500f, 25.188f)),
            //new VehicleRaceCheckpoint(18, new Vector3(-1287.750f, -638.750f, 25.688f)),
            //new VehicleRaceCheckpoint(19, new Vector3(-1370.000f, -697.750f, 23.813f)),
            //new VehicleRaceCheckpoint(20, new Vector3(-1425.250f, -726.250f, 22.438f)),
            //new VehicleRaceCheckpoint(21, new Vector3(-1503.281f, -692.793f, 26.973f)),
            //new VehicleRaceCheckpoint(22, new Vector3(-1483.000f, -624.500f, 29.625f)),
            //new VehicleRaceCheckpoint(23, new Vector3(-1418.500f, -584.500f, 29.531f)),
            //new VehicleRaceCheckpoint(24, new Vector3(-1418.500f, -502.500f, 31.906f)),
            //new VehicleRaceCheckpoint(25, new Vector3(-1496.000f, -452.750f, 34.656f)),
            //new VehicleRaceCheckpoint(26, new Vector3(-1626.362f, -551.577f, 33.412f)),
            //new VehicleRaceCheckpoint(27, new Vector3(-1617.975f, -614.104f, 31.521f)),
            //new VehicleRaceCheckpoint(28, new Vector3(-1513.315f, -700.317f, 27.189f)),
            //new VehicleRaceCheckpoint(29, new Vector3(-1415.969f, -779.623f, 20.948f)),
            //new VehicleRaceCheckpoint(30, new Vector3(-1316.515f, -880.081f, 12.504f)),
            //new VehicleRaceCheckpoint(31, new Vector3(-1267.000f, -1014.750f, 8.375f)),
            //new VehicleRaceCheckpoint(32, new Vector3(-1279.250f, -1072.500f, 6.625f)),
            //new VehicleRaceCheckpoint(33, new Vector3(-1342.750f, -1025.750f, 6.938f)),
            //new VehicleRaceCheckpoint(34, new Vector3(-1332.000f, -986.250f, 6.625f)),
            //new VehicleRaceCheckpoint(35, new Vector3(-1202.500f, -975.250f, 4.500f)),
            //new VehicleRaceCheckpoint(36, new Vector3(-1160.750f, -951.250f, 1.906f)),


            //Alt route 2
            new VehicleRaceCheckpoint(0, new Vector3(-1181.750f, -963.250f, 3.250f)),
            new VehicleRaceCheckpoint(1, new Vector3(-1051.500f, -1041.250f, 1.188f)),
            new VehicleRaceCheckpoint(2, new Vector3(-988.750f, -1150.250f, 1.281f)),
            new VehicleRaceCheckpoint(3, new Vector3(-907.250f, -1206.750f, 4.000f)),
            new VehicleRaceCheckpoint(4, new Vector3(-820.500f, -1153.500f, 6.750f)),
            new VehicleRaceCheckpoint(5, new Vector3(-801.250f, -1057.250f, 11.375f)),
            new VehicleRaceCheckpoint(6, new Vector3(-750.750f, -967.500f, 15.688f)),
            new VehicleRaceCheckpoint(7, new Vector3(-690.250f, -957.000f, 19.063f)),
            new VehicleRaceCheckpoint(8, new Vector3(-646.250f, -1000.000f, 19.406f)),
            new VehicleRaceCheckpoint(9, new Vector3(-737.250f, -1089.750f, 10.438f)),
            new VehicleRaceCheckpoint(10, new Vector3(-822.000f, -1138.750f, 7.156f)),
            new VehicleRaceCheckpoint(11, new Vector3(-901.250f, -1184.750f, 3.875f)),
            new VehicleRaceCheckpoint(12, new Vector3(-976.500f, -1171.250f, 3.063f)),
            new VehicleRaceCheckpoint(13, new Vector3(-1069.750f, -1009.750f, 1.188f)),
            new VehicleRaceCheckpoint(14, new Vector3(-1140.750f, -890.750f, 6.438f)),
            new VehicleRaceCheckpoint(15, new Vector3(-1125.500f, -800.750f, 15.906f)),
            new VehicleRaceCheckpoint(16, new Vector3(-1103.250f, -723.250f, 19.156f)),
            new VehicleRaceCheckpoint(17, new Vector3(-1193.000f, -630.000f, 23.219f)),
            new VehicleRaceCheckpoint(18, new Vector3(-1272.750f, -627.750f, 25.969f)),
            new VehicleRaceCheckpoint(19, new Vector3(-1416.000f, -725.000f, 22.500f)),
            new VehicleRaceCheckpoint(20, new Vector3(-1495.750f, -706.500f, 25.906f)),
            new VehicleRaceCheckpoint(21, new Vector3(-1501.000f, -637.500f, 28.906f)),
            new VehicleRaceCheckpoint(22, new Vector3(-1418.500f, -584.500f, 29.531f)),
            new VehicleRaceCheckpoint(23, new Vector3(-1418.500f, -502.500f, 31.906f)),
            new VehicleRaceCheckpoint(24, new Vector3(-1496.000f, -452.750f, 34.656f)),
            new VehicleRaceCheckpoint(25, new Vector3(-1610.750f, -530.500f, 33.813f)),
            new VehicleRaceCheckpoint(26, new Vector3(-1618.271f, -613.152f, 31.574f)),
            new VehicleRaceCheckpoint(27, new Vector3(-1480.675f, -725.914f, 24.948f)),
            new VehicleRaceCheckpoint(28, new Vector3(-1318.750f, -877.250f, 12.750f)),
            new VehicleRaceCheckpoint(29, new Vector3(-1267.000f, -1014.750f, 8.375f)),
            new VehicleRaceCheckpoint(30, new Vector3(-1279.250f, -1072.500f, 6.625f)),
            new VehicleRaceCheckpoint(31, new Vector3(-1330.750f, -1047.000f, 6.656f)),
            new VehicleRaceCheckpoint(32, new Vector3(-1332.000f, -986.250f, 6.625f)),
            new VehicleRaceCheckpoint(33, new Vector3(-1212.750f, -981.250f, 4.531f)),
        };
        VehicleRaceTrack vespcanals = new VehicleRaceTrack("vespCanals", "Circuit - Vespucci Canals", "Race Around Vespucci Canals", canalscheckpoints, canalsstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(vespcanals); // R Conversion (Altered)

        List<VehicleRaceStartingPosition> welcometolsstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-2210.580f, -339.095f, 12.488f), 84.284f),
            new VehicleRaceStartingPosition(1, new Vector3(-2210.132f, -334.617f, 12.496f), 84.284f),
            new VehicleRaceStartingPosition(2, new Vector3(-2202.620f, -339.892f, 12.437f), 84.284f),
            new VehicleRaceStartingPosition(3, new Vector3(-2202.171f, -335.414f, 12.452f), 84.284f),
            new VehicleRaceStartingPosition(4, new Vector3(-2194.659f, -340.689f, 12.393f), 84.284f),
            new VehicleRaceStartingPosition(5, new Vector3(-2194.211f, -336.211f, 12.408f), 84.284f),
            new VehicleRaceStartingPosition(6, new Vector3(-2186.699f, -341.485f, 12.362f), 84.284f),
            new VehicleRaceStartingPosition(7, new Vector3(-2186.251f, -337.008f, 12.370f), 84.284f),
        };
        List<VehicleRaceCheckpoint> welcometolscheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-2464.500f, -211.500f, 16.281f)),
            new VehicleRaceCheckpoint(1, new Vector3(-3004.500f, 631.250f, 19.938f)),
            new VehicleRaceCheckpoint(2, new Vector3(-3009.500f, 1637.500f, 30.906f)),
            new VehicleRaceCheckpoint(3, new Vector3(-2665.250f, 2585.250f, 15.656f)),
            new VehicleRaceCheckpoint(4, new Vector3(-2475.000f, 3632.750f, 12.938f)),
            new VehicleRaceCheckpoint(5, new Vector3(-1944.500f, 4574.250f, 56.000f)),
            new VehicleRaceCheckpoint(6, new Vector3(-1081.750f, 5319.250f, 46.313f)),
            new VehicleRaceCheckpoint(7, new Vector3(-272.000f, 6096.250f, 30.219f)),
            new VehicleRaceCheckpoint(8, new Vector3(661.250f, 6520.000f, 27.125f)),
            new VehicleRaceCheckpoint(9, new Vector3(1778.000f, 6340.750f, 36.063f)),
            new VehicleRaceCheckpoint(10, new Vector3(2535.000f, 5384.250f, 43.563f)),
            new VehicleRaceCheckpoint(11, new Vector3(2802.000f, 4321.750f, 49.094f)),
            new VehicleRaceCheckpoint(12, new Vector3(2708.250f, 3249.500f, 53.938f)),
            new VehicleRaceCheckpoint(13, new Vector3(1967.000f, 2541.250f, 53.719f)),
            new VehicleRaceCheckpoint(14, new Vector3(2020.750f, 1529.000f, 74.469f)),
            new VehicleRaceCheckpoint(15, new Vector3(2531.500f, 542.250f, 111.219f)),
            new VehicleRaceCheckpoint(16, new Vector3(2266.000f, -436.000f, 88.156f)),
            new VehicleRaceCheckpoint(17, new Vector3(1276.000f, -1118.000f, 49.813f)),
            new VehicleRaceCheckpoint(18, new Vector3(1302.750f, -2234.750f, 50.281f)),
            new VehicleRaceCheckpoint(19, new Vector3(179.500f, -2653.250f, 17.375f)),
            new VehicleRaceCheckpoint(20, new Vector3(-842.500f, -1765.000f, 36.531f)),
            new VehicleRaceCheckpoint(21, new Vector3(-396.250f, -652.000f, 36.250f)),
            new VehicleRaceCheckpoint(22, new Vector3(-514.500f, -479.750f, 28.813f)),
            new VehicleRaceCheckpoint(23, new Vector3(-1963.250f, -466.000f, 10.844f)),
        };
        VehicleRaceTrack roundtrip = new VehicleRaceTrack("welcometols", "Circuit - Welcome to Los Santos", "Race around Los Santos", welcometolscheckpoints, welcometolsstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(roundtrip);

        List<VehicleRaceCheckpoint> welcometolsshortcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-2464.500f, -211.500f, 16.281f)),
            new VehicleRaceCheckpoint(1, new Vector3(-3004.500f, 631.250f, 19.938f)),
            new VehicleRaceCheckpoint(2, new Vector3(-3009.500f, 1637.500f, 30.906f)),
            new VehicleRaceCheckpoint(3, new Vector3(-2665.250f, 2585.250f, 15.656f)),
            new VehicleRaceCheckpoint(4, new Vector3(-2475.000f, 3632.750f, 12.938f)),
            new VehicleRaceCheckpoint(5, new Vector3(-1944.500f, 4574.250f, 56.000f)),
            new VehicleRaceCheckpoint(6, new Vector3(-873.500f, 5429.750f, 34.219f)),
            new VehicleRaceCheckpoint(7, new Vector3(11.750f, 6389.750f, 30.313f)),
            new VehicleRaceCheckpoint(8, new Vector3(1031.000f, 6484.750f, 19.969f)),
            new VehicleRaceCheckpoint(9, new Vector3(2119.000f, 6025.500f, 50.031f)),
            new VehicleRaceCheckpoint(10, new Vector3(2594.750f, 5158.750f, 43.781f)),
            new VehicleRaceCheckpoint(11, new Vector3(2891.250f, 4070.000f, 49.813f)),
            new VehicleRaceCheckpoint(12, new Vector3(2451.000f, 2958.500f, 39.719f)),
            new VehicleRaceCheckpoint(13, new Vector3(1885.250f, 2423.000f, 53.594f)),
            new VehicleRaceCheckpoint(14, new Vector3(2283.000f, 1142.250f, 77.344f)),
            new VehicleRaceCheckpoint(15, new Vector3(2482.500f, -26.500f, 92.563f)),
            new VehicleRaceCheckpoint(16, new Vector3(1686.750f, -906.250f, 66.094f)),
            new VehicleRaceCheckpoint(17, new Vector3(827.000f, -1185.000f, 44.719f)),
            new VehicleRaceCheckpoint(18, new Vector3(-246.250f, -1186.750f, 36.281f)),
            new VehicleRaceCheckpoint(19, new Vector3(-432.750f, -492.250f, 32.188f)),
            new VehicleRaceCheckpoint(20, new Vector3(-1963.250f, -466.000f, 10.844f)),
        };
        VehicleRaceTrack welcometolsshort = new VehicleRaceTrack("welcometolsshort", "Circuit - Welcome to Los Santos 2", "Race around Los Santos Short Route", welcometolsshortcheckpoints, welcometolsstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(welcometolsshort);


        // Drag Race

        List<VehicleRaceStartingPosition> central2start = new List<VehicleRaceStartingPosition>()
        {
            //new VehicleRaceStartingPosition(0, new Vector3(-414.8134f, -5.814307f, 46.03317f), 264.2367f),
            //new VehicleRaceStartingPosition(1, new Vector3(-426.5735f, -4.847809f, 45.74813f), 265.0832f),
            //new VehicleRaceStartingPosition(2, new Vector3(-415.077f, -10.34778f, 46.00951f), 259.6769f),
            //new VehicleRaceStartingPosition(3, new Vector3(-426.7999f, -8.770337f, 45.73085f), 259.6824f),

            new VehicleRaceStartingPosition(0, new Vector3(-413.490f, -11.086f, 45.663f), 266.325f),
            new VehicleRaceStartingPosition(1, new Vector3(-413.202f, -6.595f, 45.699f), 266.325f),
            new VehicleRaceStartingPosition(2, new Vector3(-423.469f, -10.445f, 45.390f), 266.325f),
            new VehicleRaceStartingPosition(3, new Vector3(-423.181f, -5.954f, 45.443f), 266.325f),
            new VehicleRaceStartingPosition(4, new Vector3(-433.449f, -9.804f, 45.175f), 266.325f),
            new VehicleRaceStartingPosition(5, new Vector3(-433.160f, -5.314f, 45.233f), 266.325f),
            new VehicleRaceStartingPosition(6, new Vector3(-443.428f, -9.163f, 44.983f), 266.325f),
            new VehicleRaceStartingPosition(7, new Vector3(-443.140f, -4.673f, 45.041f), 266.325f),

        };
        List<VehicleRaceCheckpoint> central2checkpoints = new List<VehicleRaceCheckpoint>()
        {
            //new VehicleRaceCheckpoint(0, new Vector3(-323.7401f, -25.45497f, 47.50704f)),
            //new VehicleRaceCheckpoint(1, new Vector3(514.7919f, -333.9487f, 42.97514f)),

            //new VehicleRaceCheckpoint(0, new Vector3(-103.346f, -106.970f, 57.130f)),
            //new VehicleRaceCheckpoint(1, new Vector3(148.608f, -200.315f, 53.806f)),
            //new VehicleRaceCheckpoint(2, new Vector3(387.736f, -290.311f, 52.400f)),
            new VehicleRaceCheckpoint(0, new Vector3(540.135f, -351.532f, 43.056f)),
        };
        VehicleRaceTrack central2 = new VehicleRaceTrack("centralRace2", "Drag - Burton Cross Town Drag", "Drag race across central LS starting at Burton", central2checkpoints, central2start);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(central2);

        List<VehicleRaceStartingPosition> vagosdragstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(948.248f, -1837.391f, 30.147f), 175.974f),
            new VehicleRaceStartingPosition(1, new Vector3(951.241f, -1837.601f, 30.278f), 175.974f),
            new VehicleRaceStartingPosition(2, new Vector3(948.810f, -1829.411f, 30.164f), 175.974f),
            new VehicleRaceStartingPosition(3, new Vector3(951.802f, -1829.621f, 30.300f), 175.974f),
            new VehicleRaceStartingPosition(4, new Vector3(949.372f, -1821.430f, 30.163f), 175.974f),
            new VehicleRaceStartingPosition(5, new Vector3(952.364f, -1821.641f, 30.268f), 175.974f),
            new VehicleRaceStartingPosition(6, new Vector3(949.933f, -1813.450f, 30.164f), 175.974f),
            new VehicleRaceStartingPosition(7, new Vector3(952.926f, -1813.661f, 30.294f), 175.974f),
        };
        List<VehicleRaceCheckpoint> vagosdragcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(916.250f, -2238.250f, 29.500f)),
        };
        VehicleRaceTrack vagosdrag = new VehicleRaceTrack("vagos_drag", "Drag  - Cyperess Flats", "Cyperess Flats Quarter Mile Drag Race", vagosdragcheckpoints, vagosdragstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(vagosdrag);

        List<VehicleRaceStartingPosition> elyfreewaystart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(1149.017f, -2551.787f, 32.620f), 103.299f),
            new VehicleRaceStartingPosition(1, new Vector3(1149.937f, -2555.680f, 32.731f), 103.299f),
            new VehicleRaceStartingPosition(2, new Vector3(1158.749f, -2549.487f, 33.266f), 103.299f),
            new VehicleRaceStartingPosition(3, new Vector3(1159.669f, -2553.380f, 33.365f), 103.299f),
            new VehicleRaceStartingPosition(4, new Vector3(1168.480f, -2547.187f, 33.859f), 103.299f),
            new VehicleRaceStartingPosition(5, new Vector3(1169.401f, -2551.079f, 33.905f), 103.299f),
            new VehicleRaceStartingPosition(6, new Vector3(1178.212f, -2544.886f, 34.548f), 103.299f),
            new VehicleRaceStartingPosition(7, new Vector3(1179.132f, -2548.779f, 34.576f), 103.299f),
        };
        List<VehicleRaceCheckpoint> elyfreewaycheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(794.229f, -2619.861f, 52.165f)),
            new VehicleRaceCheckpoint(1, new Vector3(251.792f, -2656.256f, 17.833f)),
            new VehicleRaceCheckpoint(2, new Vector3(-149.193f, -2502.287f, 47.548f)),
            new VehicleRaceCheckpoint(3, new Vector3(-473.686f, -2274.928f, 61.939f)),
            new VehicleRaceCheckpoint(4, new Vector3(-839.609f, -2016.375f, 27.230f)),
        };
        VehicleRaceTrack elyfreeway = new VehicleRaceTrack("ely_freeway", "Drag - Elysian Freeway", "Over the Bridge and Drag Away", elyfreewaycheckpoints, elyfreewaystart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(elyfreeway);

        List<VehicleRaceStartingPosition> lsfreewaydragstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(1181.666f, -1895.952f, 33.169f), 27.895f),
            new VehicleRaceStartingPosition(1, new Vector3(1176.805f, -1898.526f, 33.200f), 27.895f),
            new VehicleRaceStartingPosition(2, new Vector3(1186.345f, -1904.790f, 34.116f), 27.895f),
            new VehicleRaceStartingPosition(3, new Vector3(1181.484f, -1907.364f, 34.148f), 27.895f),
            new VehicleRaceStartingPosition(4, new Vector3(1191.023f, -1913.628f, 35.089f), 27.895f),
            new VehicleRaceStartingPosition(5, new Vector3(1186.162f, -1916.202f, 35.121f), 27.895f),
            new VehicleRaceStartingPosition(6, new Vector3(1195.702f, -1922.466f, 36.069f), 27.895f),
            new VehicleRaceStartingPosition(7, new Vector3(1190.841f, -1925.040f, 36.101f), 27.895f),
        };
        List<VehicleRaceCheckpoint> lsfreewaydragcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(1077.277f, -1589.104f, 28.750f)),
            new VehicleRaceCheckpoint(1, new Vector3(1067.916f, -1291.552f, 25.754f)),
            new VehicleRaceCheckpoint(2, new Vector3(1017.853f, -900.812f, 30.098f)),
            new VehicleRaceCheckpoint(3, new Vector3(791.397f, -635.828f, 38.991f)),
            new VehicleRaceCheckpoint(4, new Vector3(364.814f, -495.167f, 33.937f)),
            new VehicleRaceCheckpoint(5, new Vector3(19.593f, -495.979f, 33.673f)),
            new VehicleRaceCheckpoint(6, new Vector3(-301.537f, -499.462f, 24.944f)),
            new VehicleRaceCheckpoint(7, new Vector3(-704.658f, -499.638f, 24.776f)),
            new VehicleRaceCheckpoint(8, new Vector3(-1138.336f, -640.478f, 11.442f)),
            new VehicleRaceCheckpoint(9, new Vector3(-1749.647f, -639.876f, 10.125f)),
            new VehicleRaceCheckpoint(10, new Vector3(-1968.95f, -461.5478f, 11.23273f)),
        };
        VehicleRaceTrack lsfreewaydrag = new VehicleRaceTrack("freewaydrag", "Drag - LS Freeway", "Drag from El Burro to Pacific Bluffs", lsfreewaydragcheckpoints, lsfreewaydragstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(lsfreewaydrag);

        List<VehicleRaceStartingPosition> occupationavequartermulestart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-137.183f, -227.581f, 43.841f), 252.262f),
            new VehicleRaceStartingPosition(1, new Vector3(-136.113f, -224.248f, 43.867f), 252.262f),
            new VehicleRaceStartingPosition(2, new Vector3(-144.803f, -225.145f, 44.149f), 252.262f),
            new VehicleRaceStartingPosition(3, new Vector3(-143.733f, -221.813f, 44.157f), 252.262f),
            new VehicleRaceStartingPosition(4, new Vector3(-152.423f, -222.710f, 44.731f), 252.262f),
            new VehicleRaceStartingPosition(5, new Vector3(-151.353f, -219.377f, 44.689f), 252.262f),
            new VehicleRaceStartingPosition(6, new Vector3(-160.044f, -220.274f, 45.410f), 252.262f),
            new VehicleRaceStartingPosition(7, new Vector3(-158.974f, -216.942f, 45.311f), 252.262f),
        };
        List<VehicleRaceCheckpoint> occupationavequartermulecheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(240.856f, -364.254f, 43.397f)),
        };
        VehicleRaceTrack occupationavequartermule = new VehicleRaceTrack("occupaveQuartermile", "Drag - Occupation Ave", "Occupation Ave Quarter Mile Drag Race", occupationavequartermulecheckpoints, occupationavequartermulestart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(occupationavequartermule);

        List<VehicleRaceStartingPosition> popdragstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(797.598f, -1853.602f, 28.299f), 170.350f),
            new VehicleRaceStartingPosition(1, new Vector3(802.035f, -1854.356f, 28.310f), 170.350f),
            new VehicleRaceStartingPosition(2, new Vector3(799.275f, -1843.743f, 28.418f), 170.350f),
            new VehicleRaceStartingPosition(3, new Vector3(803.711f, -1844.498f, 28.372f), 170.350f),
            new VehicleRaceStartingPosition(4, new Vector3(800.951f, -1833.885f, 28.413f), 170.350f),
            new VehicleRaceStartingPosition(5, new Vector3(805.387f, -1834.639f, 28.376f), 170.350f),
            new VehicleRaceStartingPosition(6, new Vector3(802.627f, -1824.026f, 28.396f), 170.350f),
            new VehicleRaceStartingPosition(7, new Vector3(807.063f, -1824.781f, 28.357f), 170.350f),
        };
        List<VehicleRaceCheckpoint> popdragcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(746.750f, -2252.000f, 28.313f)),
        };
        VehicleRaceTrack popdrag = new VehicleRaceTrack("popDrag", "Drag - Popular Street", "Popular Street Quarter Mile Drag Race", popdragcheckpoints, popdragstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(popdrag); // large bump halfway down that can causes ai drivers to lose control/crash

        List<VehicleRaceStartingPosition> rockforddragstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-1220.533f, -105.681f, 41.306f), 240.975f),
            new VehicleRaceStartingPosition(1, new Vector3(-1222.474f, -109.178f, 41.341f), 240.975f),
            new VehicleRaceStartingPosition(2, new Vector3(-1227.528f, -101.799f, 41.714f), 240.975f),
            new VehicleRaceStartingPosition(3, new Vector3(-1229.469f, -105.297f, 41.745f), 240.975f),
            new VehicleRaceStartingPosition(4, new Vector3(-1234.523f, -97.918f, 42.132f), 240.975f),
            new VehicleRaceStartingPosition(5, new Vector3(-1236.464f, -101.415f, 42.157f), 240.975f),
            new VehicleRaceStartingPosition(6, new Vector3(-1241.519f, -94.036f, 42.557f), 240.975f),
            new VehicleRaceStartingPosition(7, new Vector3(-1243.459f, -97.534f, 42.578f), 240.975f),
        };
        List<VehicleRaceCheckpoint> rockforddragcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-864.552f, -292.092f, 39.077f)),
        };
        VehicleRaceTrack rockforddrag = new VehicleRaceTrack("rockford_drag", "Drag - Rockford Ave", "Rockford Ave Quarter Mile Drag Race", rockforddragcheckpoints, rockforddragstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(rockforddrag);

        List<VehicleRaceStartingPosition> strawberryquartermilestart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-115.413f, -1725.977f, 29.002f), 320.428f),
            new VehicleRaceStartingPosition(1, new Vector3(-117.340f, -1724.384f, 28.990f), 320.428f),
            new VehicleRaceStartingPosition(2, new Vector3(-120.510f, -1732.144f, 29.134f), 320.428f),
            new VehicleRaceStartingPosition(3, new Vector3(-122.437f, -1730.551f, 29.137f), 320.428f),
            new VehicleRaceStartingPosition(4, new Vector3(-125.606f, -1738.310f, 29.134f), 320.428f),
            new VehicleRaceStartingPosition(5, new Vector3(-127.533f, -1736.718f, 29.136f), 320.428f),
            new VehicleRaceStartingPosition(6, new Vector3(-130.702f, -1744.477f, 29.136f), 320.428f),
            new VehicleRaceStartingPosition(7, new Vector3(-132.629f, -1742.884f, 29.137f), 320.428f),
        };
        List<VehicleRaceCheckpoint> strawberryquartermilecheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(144.681f, -1417.443f, 28.599f)),
        };
        VehicleRaceTrack strawberryquartermile = new VehicleRaceTrack("strawberryquartermile", "Drag - Strawberry Ave", "Strawberry Ave Quarter Mile Drag Race", strawberryquartermilecheckpoints, strawberryquartermilestart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(strawberryquartermile);


        // Point to Point
        //List<VehicleRaceStartingPosition> sideroutesstart = new List<VehicleRaceStartingPosition>()
        //{
        //    new VehicleRaceStartingPosition(0, new Vector3(-267.670f, -1325.707f, 30.290f), 359.829f),
        //    new VehicleRaceStartingPosition(1, new Vector3(-271.670f, -1325.695f, 30.346f), 359.829f),
        //    new VehicleRaceStartingPosition(2, new Vector3(-267.693f, -1333.707f, 30.291f), 359.829f),
        //    new VehicleRaceStartingPosition(3, new Vector3(-271.693f, -1333.695f, 30.348f), 359.829f),
        //    new VehicleRaceStartingPosition(4, new Vector3(-267.717f, -1341.706f, 30.292f), 359.829f),
        //    new VehicleRaceStartingPosition(5, new Vector3(-271.717f, -1341.694f, 30.346f), 359.829f),
        //    new VehicleRaceStartingPosition(6, new Vector3(-267.740f, -1349.706f, 30.292f), 359.829f),
        //    new VehicleRaceStartingPosition(7, new Vector3(-271.740f, -1349.694f, 30.346f), 359.829f),
        //};
        //List<VehicleRaceCheckpoint> sideroutescheckpoints = new List<VehicleRaceCheckpoint>()
        //{
        //    new VehicleRaceCheckpoint(0, new Vector3(-268.500f, -1207.750f, 23.750f)),
        //    new VehicleRaceCheckpoint(1, new Vector3(-216.750f, -1117.750f, 21.938f)),
        //    new VehicleRaceCheckpoint(2, new Vector3(-158.250f, -1038.250f, 26.250f)),
        //    new VehicleRaceCheckpoint(3, new Vector3(-89.000f, -1023.500f, 26.813f)),
        //    new VehicleRaceCheckpoint(4, new Vector3(-49.000f, -994.000f, 28.219f)),
        //    new VehicleRaceCheckpoint(5, new Vector3(-93.585f, -918.268f, 28.362f)),
        //    new VehicleRaceCheckpoint(6, new Vector3(-235.468f, -866.646f, 29.466f)),
        //    new VehicleRaceCheckpoint(7, new Vector3(-327.683f, -841.245f, 30.646f)),
        //    new VehicleRaceCheckpoint(8, new Vector3(-354.250f, -826.250f, 30.500f)),
        //    new VehicleRaceCheckpoint(9, new Vector3(-349.500f, -759.750f, 32.938f)),
        //    new VehicleRaceCheckpoint(10, new Vector3(-342.000f, -702.500f, 31.688f)),
        //    new VehicleRaceCheckpoint(11, new Vector3(-309.000f, -633.500f, 32.219f)),
        //    new VehicleRaceCheckpoint(12, new Vector3(-274.500f, -616.250f, 32.281f)),
        //    new VehicleRaceCheckpoint(13, new Vector3(-210.750f, -635.500f, 32.219f)),
        //    new VehicleRaceCheckpoint(14, new Vector3(-149.250f, -658.250f, 31.531f)),
        //    new VehicleRaceCheckpoint(15, new Vector3(-103.500f, -729.000f, 33.750f)),
        //    new VehicleRaceCheckpoint(16, new Vector3(-5.750f, -753.250f, 31.219f)),
        //    new VehicleRaceCheckpoint(17, new Vector3(59.750f, -719.500f, 30.625f)),
        //    new VehicleRaceCheckpoint(18, new Vector3(84.000f, -693.250f, 30.656f)),
        //    new VehicleRaceCheckpoint(19, new Vector3(175.750f, -721.500f, 32.094f)),
        //    new VehicleRaceCheckpoint(20, new Vector3(227.750f, -737.250f, 33.375f)),
        //    new VehicleRaceCheckpoint(21, new Vector3(264.500f, -751.500f, 33.625f)),
        //    new VehicleRaceCheckpoint(22, new Vector3(229.750f, -743.250f, 29.813f)),
        //    new VehicleRaceCheckpoint(23, new Vector3(226.500f, -810.750f, 29.500f)),
        //    new VehicleRaceCheckpoint(24, new Vector3(271.250f, -879.500f, 28.188f)),
        //    new VehicleRaceCheckpoint(25, new Vector3(204.000f, -1076.500f, 28.313f)),
        //    new VehicleRaceCheckpoint(26, new Vector3(150.750f, -1129.500f, 28.344f)),
        //    new VehicleRaceCheckpoint(27, new Vector3(53.750f, -1178.250f, 28.375f)),
        //    new VehicleRaceCheckpoint(28, new Vector3(34.250f, -1303.750f, 28.188f)),
        //    new VehicleRaceCheckpoint(29, new Vector3(-34.250f, -1302.500f, 28.031f)),
        //    new VehicleRaceCheckpoint(30, new Vector3(-53.500f, -1317.000f, 28.063f)),
        //    new VehicleRaceCheckpoint(31, new Vector3(-109.000f, -1296.750f, 28.344f)),
        //    new VehicleRaceCheckpoint(32, new Vector3(-205.250f, -1305.250f, 30.344f)),
        //    new VehicleRaceCheckpoint(33, new Vector3(-257.670f, -1307.038f, 30.301f)),
        //};
        //VehicleRaceTrack mycustomrace = new VehicleRaceTrack("sideroutes", "P2P - Alley Cat", "In and Out", sideroutescheckpoints, sideroutesstart);
        //VehicleRaceTypeManager.VehicleRaceTracks.Add(mycustomrace); // testing sideroads and alleyways for point to point, Most sideroads end abruptly causing AI to get confused and find alt route



        List<VehicleRaceStartingPosition> docks2delperrostart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(744.921f, -2935.991f, 4.801f), 0.306f),
            new VehicleRaceStartingPosition(1, new Vector3(739.921f, -2936.018f, 4.801f), 0.306f),
            new VehicleRaceStartingPosition(2, new Vector3(744.964f, -2943.991f, 4.801f), 0.306f),
            new VehicleRaceStartingPosition(3, new Vector3(739.964f, -2944.018f, 4.801f), 0.306f),
            new VehicleRaceStartingPosition(4, new Vector3(745.007f, -2951.991f, 4.801f), 0.306f),
            new VehicleRaceStartingPosition(5, new Vector3(740.007f, -2952.017f, 4.801f), 0.306f),
            new VehicleRaceStartingPosition(6, new Vector3(745.049f, -2959.991f, 4.801f), 0.306f),
            new VehicleRaceStartingPosition(7, new Vector3(740.049f, -2960.017f, 4.801f), 0.306f),
        };
        List<VehicleRaceCheckpoint> docks2delperrocheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(739.500f, -2764.750f, 5.531f)),
            new VehicleRaceCheckpoint(1, new Vector3(738.750f, -2595.000f, 17.500f)),
            new VehicleRaceCheckpoint(2, new Vector3(758.750f, -2397.250f, 20.031f)),
            new VehicleRaceCheckpoint(3, new Vector3(780.250f, -2152.500f, 28.313f)),
            new VehicleRaceCheckpoint(4, new Vector3(827.000f, -1844.000f, 28.188f)),
            new VehicleRaceCheckpoint(5, new Vector3(844.500f, -1674.000f, 28.344f)),
            new VehicleRaceCheckpoint(6, new Vector3(817.921f, -1496.441f, 27.186f)),
            new VehicleRaceCheckpoint(7, new Vector3(803.750f, -1309.250f, 25.250f)),
            new VehicleRaceCheckpoint(8, new Vector3(793.766f, -1072.765f, 27.341f)),
            new VehicleRaceCheckpoint(9, new Vector3(781.374f, -873.052f, 24.187f)),
            new VehicleRaceCheckpoint(10, new Vector3(784.250f, -608.500f, 27.719f)),
            new VehicleRaceCheckpoint(11, new Vector3(639.026f, -379.028f, 41.947f)),
            new VehicleRaceCheckpoint(12, new Vector3(427.000f, -288.500f, 49.531f)),
            new VehicleRaceCheckpoint(13, new Vector3(255.377f, -224.090f, 53.042f)),
            new VehicleRaceCheckpoint(14, new Vector3(134.750f, -178.000f, 53.594f)),
            new VehicleRaceCheckpoint(15, new Vector3(-25.250f, -117.750f, 56.063f)),
            new VehicleRaceCheckpoint(16, new Vector3(-189.000f, -61.750f, 50.688f)),
            new VehicleRaceCheckpoint(17, new Vector3(-359.872f, -4.791f, 46.149f)),
            new VehicleRaceCheckpoint(18, new Vector3(-482.500f, 11.750f, 44.313f)),
            new VehicleRaceCheckpoint(19, new Vector3(-603.667f, 4.277f, 41.865f)),
            new VehicleRaceCheckpoint(20, new Vector3(-752.250f, -41.000f, 36.875f)),
            new VehicleRaceCheckpoint(21, new Vector3(-986.500f, -163.000f, 36.906f)),
            new VehicleRaceCheckpoint(22, new Vector3(-1147.000f, -247.750f, 36.813f)),
            new VehicleRaceCheckpoint(23, new Vector3(-1338.250f, -355.500f, 35.719f)),
            new VehicleRaceCheckpoint(24, new Vector3(-1496.000f, -452.750f, 34.656f)),
            new VehicleRaceCheckpoint(25, new Vector3(-1646.500f, -559.500f, 32.438f)),
        };
        VehicleRaceTrack docks2delperro = new VehicleRaceTrack("docks2delperro", "P2P - Docks to Del Perro", "Race From the Docks to Del Perro", docks2delperrocheckpoints, docks2delperrostart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(docks2delperro);

        List<VehicleRaceStartingPosition> fleecarunstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(356.671f, -1329.025f, 31.547f), 50.056f),
            new VehicleRaceStartingPosition(1, new Vector3(354.745f, -1331.325f, 31.543f), 50.056f),
            new VehicleRaceStartingPosition(2, new Vector3(362.805f, -1334.161f, 31.468f), 50.056f),
            new VehicleRaceStartingPosition(3, new Vector3(360.879f, -1336.461f, 31.464f), 50.056f),
            new VehicleRaceStartingPosition(4, new Vector3(368.938f, -1339.297f, 31.278f), 50.056f),
            new VehicleRaceStartingPosition(5, new Vector3(367.012f, -1341.597f, 31.299f), 50.056f),
            new VehicleRaceStartingPosition(6, new Vector3(375.072f, -1344.434f, 31.017f), 50.056f),
            new VehicleRaceStartingPosition(7, new Vector3(373.145f, -1346.734f, 31.003f), 50.056f),
        };
        List<VehicleRaceCheckpoint> fleecaruncheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(159.377f, -1012.836f, 28.717f)),
            new VehicleRaceCheckpoint(1, new Vector3(324.981f, -250.772f, 53.206f)),
            new VehicleRaceCheckpoint(2, new Vector3(-337.294f, -10.375f, 47.061f)),
            new VehicleRaceCheckpoint(3, new Vector3(-1227.250f, -298.250f, 36.750f)),
            new VehicleRaceCheckpoint(4, new Vector3(-2979.841f, 483.927f, 14.592f)),
            new VehicleRaceCheckpoint(5, new Vector3(1175.108f, 2679.945f, 37.291f)),
        };
        VehicleRaceTrack fleecarun = new VehicleRaceTrack("fleecarun", "P2P - Fleeca Run", "Withdrawls Optional", fleecaruncheckpoints, fleecarunstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(fleecarun);

        List<VehicleRaceStartingPosition> elburropointstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(1323.004f, -2592.555f, 46.650f), 285.619f),
            new VehicleRaceStartingPosition(1, new Vector3(1322.196f, -2589.666f, 46.694f), 285.619f),
            new VehicleRaceStartingPosition(2, new Vector3(1313.373f, -2595.248f, 46.241f), 285.619f),
            new VehicleRaceStartingPosition(3, new Vector3(1312.565f, -2592.358f, 46.285f), 285.619f),
            new VehicleRaceStartingPosition(4, new Vector3(1303.742f, -2597.940f, 45.796f), 285.619f),
            new VehicleRaceStartingPosition(5, new Vector3(1302.934f, -2595.051f, 45.844f), 285.619f),
            new VehicleRaceStartingPosition(6, new Vector3(1294.111f, -2600.632f, 45.326f), 285.619f),
            new VehicleRaceStartingPosition(7, new Vector3(1293.304f, -2597.743f, 45.377f), 285.619f),
        };
        List<VehicleRaceCheckpoint> elburropointcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(1451.340f, -2579.668f, 47.779f)),
            new VehicleRaceCheckpoint(1, new Vector3(1618.824f, -2534.294f, 70.578f)),
            new VehicleRaceCheckpoint(2, new Vector3(1632.008f, -2431.224f, 90.022f)),
            new VehicleRaceCheckpoint(3, new Vector3(1677.693f, -2266.232f, 110.673f)),
            new VehicleRaceCheckpoint(4, new Vector3(1681.042f, -2122.632f, 106.833f)),
            new VehicleRaceCheckpoint(5, new Vector3(1726.409f, -1947.849f, 116.160f)),
            new VehicleRaceCheckpoint(6, new Vector3(1719.182f, -1793.383f, 110.499f)),
            new VehicleRaceCheckpoint(7, new Vector3(1807.052f, -1668.285f, 117.398f)),
            new VehicleRaceCheckpoint(8, new Vector3(1813.757f, -1508.802f, 116.337f)),
            new VehicleRaceCheckpoint(9, new Vector3(1894.101f, -1376.753f, 135.831f)),
            new VehicleRaceCheckpoint(10, new Vector3(1917.956f, -1193.461f, 114.702f)),
            new VehicleRaceCheckpoint(11, new Vector3(1966.270f, -1039.826f, 90.024f)),
            new VehicleRaceCheckpoint(12, new Vector3(1974.646f, -926.345f, 78.651f)),
        };
        VehicleRaceTrack elburropoint = new VehicleRaceTrack("ElBurroPoint", "P2P - Palomino Highland View", "Race along the scenic Palomino Highlands", elburropointcheckpoints, elburropointstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(elburropoint);

        List<VehicleRaceStartingPosition> central1start = new List<VehicleRaceStartingPosition>()
        {
            //new VehicleRaceStartingPosition(0, new Vector3(68.09341f, -1183.503f, 28.72084f), 2.301179f),
            //new VehicleRaceStartingPosition(1, new Vector3(72.77296f, -1185.764f, 28.65525f), 8.259281f),
            //new VehicleRaceStartingPosition(2, new Vector3(67.94074f, -1197.815f, 28.72175f), 1.318574f),
            //new VehicleRaceStartingPosition(3, new Vector3(73.90773f, -1198.595f, 28.61956f), 0.1617375f),

            new VehicleRaceStartingPosition(0, new Vector3(72.461f, -1182.386f, 28.286f), 3.144f),
            new VehicleRaceStartingPosition(1, new Vector3(67.968f, -1182.633f, 28.342f), 3.144f),
            new VehicleRaceStartingPosition(2, new Vector3(73.009f, -1192.371f, 28.279f), 3.144f),
            new VehicleRaceStartingPosition(3, new Vector3(68.516f, -1192.618f, 28.342f), 3.144f),
            new VehicleRaceStartingPosition(4, new Vector3(73.558f, -1202.356f, 28.261f), 3.144f),
            new VehicleRaceStartingPosition(5, new Vector3(69.064f, -1202.603f, 28.342f), 3.144f),
            new VehicleRaceStartingPosition(6, new Vector3(74.106f, -1212.341f, 28.231f), 3.144f),
            new VehicleRaceStartingPosition(7, new Vector3(69.613f, -1212.588f, 28.342f), 3.144f),
        };
        List<VehicleRaceCheckpoint> central1checkpoints = new List<VehicleRaceCheckpoint>()
        {
            //new VehicleRaceCheckpoint(0, new Vector3(80.30048f, -1067.49f, 28.79053f)),
            //new VehicleRaceCheckpoint(1, new Vector3(172.9085f, -818.7173f, 30.55356f)),
            //new VehicleRaceCheckpoint(2, new Vector3(303.2614f, -470.1808f, 42.71596f)),
            //new VehicleRaceCheckpoint(3, new Vector3(183.5113f, -337.3925f, 43.44069f)),
            //new VehicleRaceCheckpoint(4, new Vector3(-61.29679f, -245.9679f, 44.78765f)),
            //new VehicleRaceCheckpoint(5, new Vector3(-506.2202f, -272.2784f, 35.05164f)),

            //new VehicleRaceCheckpoint(0, new Vector3(167.667f, -854.560f, 30.313f)),
            //new VehicleRaceCheckpoint(1, new Vector3(269.549f, -588.946f, 42.592f)),
            //new VehicleRaceCheckpoint(2, new Vector3(307.401f, -451.388f, 42.946f)),
            //new VehicleRaceCheckpoint(3, new Vector3(128.366f, -310.709f, 44.708f)),
            //new VehicleRaceCheckpoint(4, new Vector3(-127.608f, -216.089f, 44.195f)),
            //new VehicleRaceCheckpoint(5, new Vector3(-334.408f, -185.479f, 38.215f)),
            //new VehicleRaceCheckpoint(6, new Vector3(-509.182f, -269.209f, 35.042f)),

            new VehicleRaceCheckpoint(0, new Vector3(147.207f, -911.106f, 29.547f)),
            new VehicleRaceCheckpoint(1, new Vector3(269.077f, -589.667f, 42.542f)),
            new VehicleRaceCheckpoint(2, new Vector3(307.197f, -458.166f, 42.682f)),
            new VehicleRaceCheckpoint(3, new Vector3(216.328f, -344.796f, 43.485f)),
            new VehicleRaceCheckpoint(4, new Vector3(5.863f, -266.014f, 46.710f)),
            new VehicleRaceCheckpoint(5, new Vector3(-179.623f, -191.580f, 43.154f)),
            new VehicleRaceCheckpoint(6, new Vector3(-332.902f, -185.091f, 38.211f)),
            new VehicleRaceCheckpoint(7, new Vector3(-507.941f, -268.664f, 35.003f)),


        };
        VehicleRaceTrack central1 = new VehicleRaceTrack("centralRace1", "P2P - Strawberry To Rockford City Hall", "Race to Rockford City Hall from the Strawberry underpass", central1checkpoints, central1start);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(central1);

        List<VehicleRaceStartingPosition> sustanciasprintstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(1398.971f, -1564.691f, 55.209f), 307.868f),
            new VehicleRaceStartingPosition(1, new Vector3(1397.130f, -1562.323f, 55.296f), 307.868f),
            new VehicleRaceStartingPosition(2, new Vector3(1392.656f, -1569.602f, 54.560f), 307.868f),
            new VehicleRaceStartingPosition(3, new Vector3(1390.814f, -1567.234f, 54.612f), 307.868f),
            new VehicleRaceStartingPosition(4, new Vector3(1386.340f, -1574.513f, 53.873f), 307.868f),
            new VehicleRaceStartingPosition(5, new Vector3(1384.499f, -1572.145f, 53.916f), 307.868f),
            new VehicleRaceStartingPosition(6, new Vector3(1380.025f, -1579.424f, 53.171f), 307.868f),
            new VehicleRaceStartingPosition(7, new Vector3(1378.183f, -1577.055f, 53.242f), 307.868f),
        };
        List<VehicleRaceCheckpoint> sustanciasprintcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(1597.000f, -1385.750f, 80.719f)),
            new VehicleRaceCheckpoint(1, new Vector3(2022.250f, -895.000f, 78.156f)),
            new VehicleRaceCheckpoint(2, new Vector3(2465.000f, -505.000f, 68.688f)),
        };
        VehicleRaceTrack sustanciasprint = new VehicleRaceTrack("sustanciasprint", "P2P - Sustancia Road", "Race along Sustancia Road", sustanciasprintcheckpoints, sustanciasprintstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(sustanciasprint);

        List<VehicleRaceCheckpoint> greatoceanhighwaycheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-2464.500f, -211.500f, 16.281f)),
            new VehicleRaceCheckpoint(1, new Vector3(-3009.000f, 648.500f, 20.625f)),
            new VehicleRaceCheckpoint(2, new Vector3(-3042.000f, 1800.250f, 33.281f)),
            new VehicleRaceCheckpoint(3, new Vector3(-2641.750f, 2745.500f, 15.656f)),
            new VehicleRaceCheckpoint(4, new Vector3(-2257.250f, 4246.500f, 43.406f)),
        };
        VehicleRaceTrack greatoceanhighway = new VehicleRaceTrack("greatoceanhighway", "P2P - The Great Ocean Hwy", "Enjoy the view while racing along the Great Ocean Hwy", greatoceanhighwaycheckpoints, welcometolsstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(greatoceanhighway);

        List<VehicleRaceStartingPosition> roadtoharmonystart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-713.784f, 977.774f, 236.976f), 16.551f),
            new VehicleRaceStartingPosition(1, new Vector3(-717.139f, 976.777f, 237.008f), 16.551f),
            new VehicleRaceStartingPosition(2, new Vector3(-711.505f, 970.106f, 236.865f), 16.551f),
            new VehicleRaceStartingPosition(3, new Vector3(-714.860f, 969.109f, 236.970f), 16.551f),
            new VehicleRaceStartingPosition(4, new Vector3(-709.226f, 962.437f, 236.474f), 16.551f),
            new VehicleRaceStartingPosition(5, new Vector3(-712.581f, 961.440f, 236.544f), 16.551f),
            new VehicleRaceStartingPosition(6, new Vector3(-706.947f, 954.769f, 235.961f), 16.551f),
            new VehicleRaceStartingPosition(7, new Vector3(-710.302f, 953.772f, 235.999f), 16.551f),
        };
        List<VehicleRaceCheckpoint> roadtoharmonycheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-709.750f, 1072.500f, 249.563f)),
            new VehicleRaceCheckpoint(1, new Vector3(-777.750f, 1244.250f, 260.031f)),
            new VehicleRaceCheckpoint(2, new Vector3(-806.250f, 1406.250f, 244.313f)),
            new VehicleRaceCheckpoint(3, new Vector3(-766.000f, 1558.750f, 216.906f)),
            new VehicleRaceCheckpoint(4, new Vector3(-839.750f, 1673.000f, 193.250f)),
            new VehicleRaceCheckpoint(5, new Vector3(-766.250f, 1848.500f, 160.875f)),
            new VehicleRaceCheckpoint(6, new Vector3(-768.250f, 1993.750f, 128.063f)),
            new VehicleRaceCheckpoint(7, new Vector3(-776.000f, 2215.250f, 89.813f)),
            new VehicleRaceCheckpoint(8, new Vector3(-745.750f, 2328.750f, 70.563f)),
            new VehicleRaceCheckpoint(9, new Vector3(-653.000f, 2457.000f, 56.375f)),
            new VehicleRaceCheckpoint(10, new Vector3(-558.000f, 2597.500f, 46.188f)),
            new VehicleRaceCheckpoint(11, new Vector3(-495.000f, 2781.250f, 38.719f)),
            new VehicleRaceCheckpoint(12, new Vector3(-327.000f, 2889.500f, 44.188f)),
            new VehicleRaceCheckpoint(13, new Vector3(-153.750f, 2854.250f, 48.000f)),
            new VehicleRaceCheckpoint(14, new Vector3(39.250f, 2759.750f, 56.844f)),
            new VehicleRaceCheckpoint(15, new Vector3(271.750f, 2631.000f, 43.625f)),
        };
        VehicleRaceTrack roadtoHarmony = new VehicleRaceTrack("roadtoHarmony", "P2P - The Road To Harmony", "Race to Harmony", roadtoharmonycheckpoints, roadtoharmonystart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(roadtoHarmony);

        List<VehicleRaceStartingPosition> richmanroadstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-1590.069f, 966.373f, 155.278f), 246.891f),
            new VehicleRaceStartingPosition(1, new Vector3(-1588.891f, 969.133f, 155.301f), 246.891f),
            new VehicleRaceStartingPosition(2, new Vector3(-1599.266f, 970.298f, 154.196f), 246.891f),
            new VehicleRaceStartingPosition(3, new Vector3(-1598.089f, 973.057f, 154.220f), 246.891f),
            new VehicleRaceStartingPosition(4, new Vector3(-1608.464f, 974.223f, 153.347f), 246.891f),
            new VehicleRaceStartingPosition(5, new Vector3(-1607.286f, 976.982f, 153.371f), 246.891f),
            new VehicleRaceStartingPosition(6, new Vector3(-1617.662f, 978.148f, 152.718f), 246.891f),
            new VehicleRaceStartingPosition(7, new Vector3(-1616.484f, 980.907f, 152.744f), 246.891f),
        };
        List<VehicleRaceCheckpoint> richmanroadcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-1456.206f, 851.726f, 183.369f)),
            new VehicleRaceCheckpoint(1, new Vector3(-1291.362f, 812.474f, 188.959f)),
            new VehicleRaceCheckpoint(2, new Vector3(-1152.320f, 1009.529f, 204.338f)),
            new VehicleRaceCheckpoint(3, new Vector3(-896.671f, 1044.975f, 222.797f)),
            new VehicleRaceCheckpoint(4, new Vector3(-663.543f, 996.416f, 238.328f)),
            new VehicleRaceCheckpoint(5, new Vector3(-359.193f, 960.200f, 232.935f)),
            new VehicleRaceCheckpoint(6, new Vector3(-226.630f, 1054.702f, 234.657f)),
            new VehicleRaceCheckpoint(7, new Vector3(45.139f, 1034.633f, 217.759f)),
            new VehicleRaceCheckpoint(8, new Vector3(337.203f, 1003.782f, 210.017f)),
            new VehicleRaceCheckpoint(9, new Vector3(479.491f, 870.007f, 197.614f)),
            new VehicleRaceCheckpoint(10, new Vector3(790.911f, 892.179f, 223.598f)),
            new VehicleRaceCheckpoint(11, new Vector3(968.575f, 843.676f, 202.091f)),
            new VehicleRaceCheckpoint(12, new Vector3(1027.239f, 691.696f, 159.013f)),
            new VehicleRaceCheckpoint(13, new Vector3(1125.765f, 638.303f, 115.322f)),
        };
        VehicleRaceTrack richmanroad = new VehicleRaceTrack("richman_road", "P2P - The Rich Man's Road", "Race along the scenic Richman Road", richmanroadcheckpoints, richmanroadstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(richmanroad);


        //Single checkpoint tracks P2P - R Conversion + Extras
        // You can call it another lonely day
        // You can go your own way
        // La Mesa - Docks Starting Area

        // LOCATION 1: Observatory
        List<VehicleRaceStartingPosition> DocksStart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(744.227f, -2898.523f, 4.611f), 0.363f),
            new VehicleRaceStartingPosition(1, new Vector3(739.228f, -2898.555f, 4.612f), 0.363f),
            new VehicleRaceStartingPosition(2, new Vector3(744.278f, -2906.523f, 3.232f), 0.363f),
            new VehicleRaceStartingPosition(3, new Vector3(739.278f, -2906.555f, 3.232f), 0.363f),
            new VehicleRaceStartingPosition(4, new Vector3(744.329f, -2914.523f, 3.182f), 0.363f),
            new VehicleRaceStartingPosition(5, new Vector3(739.329f, -2914.555f, 4.538f), 0.363f),
            new VehicleRaceStartingPosition(6, new Vector3(744.380f, -2922.523f, 3.148f), 0.363f),
            new VehicleRaceStartingPosition(7, new Vector3(739.380f, -2922.554f, 4.504f), 0.363f),
        };
        List<VehicleRaceCheckpoint> observatoryCheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-408.3208f, 1184.4412f, 324.5297f)),
        };
        VehicleRaceTrack observatory = new VehicleRaceTrack("observatorySprint", "Sprint - Observatory Sprint", "Race to the Observatory", observatoryCheckpoints, DocksStart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(observatory);

        // LOCATION 2: Kortz Center
        List<VehicleRaceCheckpoint> kortzCheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-2297.7310f, 379.1689f, 173.4667f)),
        };
        VehicleRaceTrack kortz = new VehicleRaceTrack("kortzSprint", "Sprint - Kortz Center Sprint", "Race to the Kortz Center", kortzCheckpoints, DocksStart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(kortz);

        // LOCATION 3: Stab City
        List<VehicleRaceCheckpoint> stabCityCheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(54.1055f, 3734.0286f, 38.7018f)),
        };
        VehicleRaceTrack stabCity = new VehicleRaceTrack("stabCitySprint", "Sprint - Stab City Sprint", "Race to Stab City", stabCityCheckpoints, DocksStart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(stabCity);

        // LOCATION 4: Vinewood Sign
        List<VehicleRaceCheckpoint> vinewoodCheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(744.1607f, 1199.4257f, 325.4021f)),
        };
        VehicleRaceTrack vinewood = new VehicleRaceTrack("vinewoodSprint", "Sprint - Vinewood Sign Sprint", "Race to the Vinewood Sign", vinewoodCheckpoints, DocksStart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(vinewood);

        // LOCATION 5: Elysian Island
        List<VehicleRaceCheckpoint> elysianCheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(231.6537f, -3326.9919f, 4.7973f)),
        };
        VehicleRaceTrack elysian = new VehicleRaceTrack("elysianSprint", "Sprint - Elysian Island Sprint", "Race to Elysian Island", elysianCheckpoints, DocksStart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(elysian);

        // LOCATION 6: Power Station
        List<VehicleRaceCheckpoint> powerStationCheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(2791.8086f, 1518.2588f, 23.5166f)),
        };
        VehicleRaceTrack powerStation = new VehicleRaceTrack("powerStationSprint", "Sprint - Power Station Sprint", "Race to the Power Station", powerStationCheckpoints, DocksStart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(powerStation);

        // LOCATION 7: LSIA
        List<VehicleRaceCheckpoint> lsiaCheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-1027.1440f, -2714.6970f, 12.8181f)),
        };
        VehicleRaceTrack lsia = new VehicleRaceTrack("lsiaSprint", "Sprint - LSIA Sprint", "Race to LSIA", lsiaCheckpoints, DocksStart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(lsia);

        // LCATION 8: Maze Bank Arena
        List<VehicleRaceCheckpoint> mazeBankCheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-189.250f, -2005.000f, 26.625f)),
        };
        VehicleRaceTrack mazeBank = new VehicleRaceTrack("mazeBankSprint", "Sprint - Maze Bank Arena Sprint", "Race to Maze Bank Arena", mazeBankCheckpoints, DocksStart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(mazeBank);

        // LOCATION 9: Puerto del Sol Marina
        List<VehicleRaceCheckpoint> puertoDelSolCheckpoints = new List<VehicleRaceCheckpoint>()
        {
           new VehicleRaceCheckpoint(0, new Vector3(-766.750f, -1295.500f, 4.000f)),
        };
        VehicleRaceTrack PuertoDelSol = new VehicleRaceTrack("PuertoDelSolSprint", "Sprint - Puerto del Sol Marina Sprint", "Race to Puerto del Sol Marina", puertoDelSolCheckpoints, DocksStart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(PuertoDelSol);

        // lOCATION 10: Del Perro Pier
        List<VehicleRaceCheckpoint> delPerroCheckpoints = new List<VehicleRaceCheckpoint>()
        {
           new VehicleRaceCheckpoint(0, new Vector3(-1600.500f, -949.000f, 12.000f))
        };
        VehicleRaceTrack delPerro = new VehicleRaceTrack("delPerroSprint", "Sprint - Del Perro Pier Sprint", "Race to Del Perro Pier", delPerroCheckpoints, DocksStart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(delPerro);

        // "observatorySprint", "kortzSprint", "stabCitySprint", "vinewoodSprint", "elysianSprint", "powerStationSprint", "lsiaSprint", "mazeBankSprint", "PuertoDelSolSprint" , "delPerroSprint"

    }


    private void OffroadTracks()
    {

        // Circuit
        List<VehicleRaceStartingPosition> chillstatewildernessloopstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-596.209f, 5022.456f, 140.371f), 171.313f),
            new VehicleRaceStartingPosition(1, new Vector3(-599.174f, 5022.909f, 140.278f), 171.313f),
            new VehicleRaceStartingPosition(2, new Vector3(-594.698f, 5032.341f, 139.309f), 171.313f),
            new VehicleRaceStartingPosition(3, new Vector3(-597.664f, 5032.794f, 139.274f), 171.313f),
            new VehicleRaceStartingPosition(4, new Vector3(-593.188f, 5042.227f, 138.242f), 171.313f),
            new VehicleRaceStartingPosition(5, new Vector3(-596.153f, 5042.680f, 138.188f), 171.313f),
            new VehicleRaceStartingPosition(6, new Vector3(-591.677f, 5052.112f, 137.028f), 171.313f),
            new VehicleRaceStartingPosition(7, new Vector3(-594.643f, 5052.565f, 137.005f), 171.313f),
        };
        List<VehicleRaceCheckpoint> chillstatewildernessloopcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-524.500f, 4949.000f, 146.531f)),
            new VehicleRaceCheckpoint(1, new Vector3(-562.750f, 4855.500f, 173.063f)),
            new VehicleRaceCheckpoint(2, new Vector3(-560.500f, 4791.750f, 200.594f)),
            new VehicleRaceCheckpoint(3, new Vector3(-608.000f, 4729.000f, 220.781f)),
            new VehicleRaceCheckpoint(4, new Vector3(-711.250f, 4774.000f, 221.531f)),
            new VehicleRaceCheckpoint(5, new Vector3(-794.500f, 4866.250f, 256.656f)),
            new VehicleRaceCheckpoint(6, new Vector3(-866.500f, 4828.250f, 297.063f)),
            new VehicleRaceCheckpoint(7, new Vector3(-938.750f, 4741.750f, 281.438f)),
            new VehicleRaceCheckpoint(8, new Vector3(-1046.500f, 4753.750f, 234.813f)),
            new VehicleRaceCheckpoint(9, new Vector3(-1117.000f, 4834.750f, 206.344f)),
            new VehicleRaceCheckpoint(10, new Vector3(-1198.000f, 4854.250f, 187.000f)),
            new VehicleRaceCheckpoint(11, new Vector3(-1316.500f, 4868.250f, 143.594f)),
            new VehicleRaceCheckpoint(12, new Vector3(-1276.250f, 4967.500f, 150.188f)),
            new VehicleRaceCheckpoint(13, new Vector3(-1156.000f, 5033.500f, 156.688f)),
            new VehicleRaceCheckpoint(14, new Vector3(-1035.750f, 5094.750f, 150.156f)),
            new VehicleRaceCheckpoint(15, new Vector3(-968.250f, 5194.000f, 120.750f)),
            new VehicleRaceCheckpoint(16, new Vector3(-823.750f, 5178.750f, 111.219f)),
            new VehicleRaceCheckpoint(17, new Vector3(-682.250f, 5147.000f, 118.719f)),
            new VehicleRaceCheckpoint(18, new Vector3(-598.500f, 5019.750f, 140.531f)),
        };
        VehicleRaceTrack chillstatewildernessloop = new VehicleRaceTrack("chillstatewildernessloop", "Circuit - Chiliad State Wilderness", "Race around the scenic Chiliad State Wilderness", chillstatewildernessloopcheckpoints, chillstatewildernessloopstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(chillstatewildernessloop);

        List<VehicleRaceStartingPosition> landactresoviorcircuitstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(2308.087f, 992.207f, 85.042f), 230.882f),
            new VehicleRaceStartingPosition(1, new Vector3(2310.295f, 994.923f, 85.053f), 230.882f),
            new VehicleRaceStartingPosition(2, new Vector3(2301.880f, 997.255f, 83.398f), 230.882f),
            new VehicleRaceStartingPosition(3, new Vector3(2304.088f, 999.970f, 83.456f), 230.882f),
            new VehicleRaceStartingPosition(4, new Vector3(2295.673f, 1002.302f, 81.703f), 230.882f),
            new VehicleRaceStartingPosition(5, new Vector3(2297.882f, 1005.017f, 81.781f), 230.882f),
            new VehicleRaceStartingPosition(6, new Vector3(2289.467f, 1007.349f, 80.217f), 230.882f),
            new VehicleRaceStartingPosition(7, new Vector3(2291.675f, 1010.065f, 80.110f), 230.882f),
        };
        List<VehicleRaceCheckpoint> landactresoviorcircuitcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(2379.750f, 879.500f, 111.219f)),
            new VehicleRaceCheckpoint(1, new Vector3(2413.250f, 727.250f, 127.313f)),
            new VehicleRaceCheckpoint(2, new Vector3(2421.000f, 565.500f, 145.094f)),
            new VehicleRaceCheckpoint(3, new Vector3(2379.500f, 401.500f, 172.406f)),
            new VehicleRaceCheckpoint(4, new Vector3(2335.250f, 235.250f, 195.594f)),
            new VehicleRaceCheckpoint(5, new Vector3(2197.000f, 110.750f, 227.875f)),
            new VehicleRaceCheckpoint(6, new Vector3(2075.500f, -5.000f, 212.281f)),
            new VehicleRaceCheckpoint(7, new Vector3(1980.500f, -84.000f, 209.531f)),
            new VehicleRaceCheckpoint(8, new Vector3(1837.750f, -89.000f, 185.781f)),
            new VehicleRaceCheckpoint(9, new Vector3(1705.500f, -80.250f, 175.969f)),
            new VehicleRaceCheckpoint(10, new Vector3(1664.500f, -11.250f, 172.750f)),
            new VehicleRaceCheckpoint(11, new Vector3(1679.250f, 66.250f, 170.844f)),
            new VehicleRaceCheckpoint(12, new Vector3(1767.500f, 113.000f, 170.063f)),
            new VehicleRaceCheckpoint(13, new Vector3(1811.750f, 239.000f, 171.938f)),
            new VehicleRaceCheckpoint(14, new Vector3(1800.000f, 397.500f, 171.250f)),
            new VehicleRaceCheckpoint(15, new Vector3(1910.750f, 508.750f, 170.875f)),
            new VehicleRaceCheckpoint(16, new Vector3(1924.250f, 713.750f, 188.500f)),
            new VehicleRaceCheckpoint(17, new Vector3(1978.000f, 941.750f, 212.688f)),
            new VehicleRaceCheckpoint(18, new Vector3(2023.250f, 1103.250f, 197.438f)),
            new VehicleRaceCheckpoint(19, new Vector3(1971.750f, 1252.000f, 175.094f)),
            new VehicleRaceCheckpoint(20, new Vector3(1830.250f, 1276.750f, 143.406f)),
            new VehicleRaceCheckpoint(21, new Vector3(1717.000f, 1170.250f, 127.250f)),
            new VehicleRaceCheckpoint(22, new Vector3(1630.000f, 999.000f, 103.313f)),
            new VehicleRaceCheckpoint(23, new Vector3(1595.250f, 918.750f, 82.406f)),
            new VehicleRaceCheckpoint(24, new Vector3(1574.750f, 966.250f, 77.500f)),
            new VehicleRaceCheckpoint(25, new Vector3(1645.750f, 1171.000f, 83.375f)),
            new VehicleRaceCheckpoint(26, new Vector3(1704.250f, 1348.000f, 85.531f)),
            new VehicleRaceCheckpoint(27, new Vector3(1753.250f, 1554.750f, 83.531f)),
            new VehicleRaceCheckpoint(28, new Vector3(1949.750f, 1603.250f, 76.844f)),
            new VehicleRaceCheckpoint(29, new Vector3(2095.750f, 1385.250f, 74.531f)),
            new VehicleRaceCheckpoint(30, new Vector3(2197.500f, 1214.000f, 74.063f)),
            new VehicleRaceCheckpoint(31, new Vector3(2272.500f, 1093.750f, 64.469f)),
            new VehicleRaceCheckpoint(32, new Vector3(2313.500f, 990.750f, 86.219f)),
        };
        VehicleRaceTrack landactresoviorcircuit = new VehicleRaceTrack("landactresoviorcircuit", "Circuit - Land Act Reservoir", "Race at the Land Act Reservoir", landactresoviorcircuitcheckpoints, landactresoviorcircuitstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(landactresoviorcircuit);

        List<VehicleRaceStartingPosition> redwoodlightsstart = new List<VehicleRaceStartingPosition>()
        {
            //new VehicleRaceStartingPosition(0, new Vector3(1164.244f, 2350.652f, 56.613f), 0.837f),
            //new VehicleRaceStartingPosition(1, new Vector3(1167.244f, 2350.696f, 56.622f), 0.837f),
            //new VehicleRaceStartingPosition(2, new Vector3(1164.390f, 2340.653f, 56.608f), 0.837f),
            //new VehicleRaceStartingPosition(3, new Vector3(1167.390f, 2340.697f, 56.665f), 0.837f),
            //new VehicleRaceStartingPosition(4, new Vector3(1164.536f, 2330.655f, 56.762f), 0.837f),
            //new VehicleRaceStartingPosition(5, new Vector3(1167.536f, 2330.698f, 56.770f), 0.837f),
            //new VehicleRaceStartingPosition(6, new Vector3(1164.682f, 2320.656f, 56.104f), 0.837f),
            //new VehicleRaceStartingPosition(7, new Vector3(1167.682f, 2320.699f, 56.105f), 0.837f),
            //new VehicleRaceStartingPosition(8, new Vector3(1164.828f, 2310.656f, 54.828f), 0.837f),
            //new VehicleRaceStartingPosition(9, new Vector3(1167.828f, 2310.700f, 54.845f), 0.837f),

            new VehicleRaceStartingPosition(0, new Vector3(1113.235f, 2126.771f, 52.416f), 329.213f),
            new VehicleRaceStartingPosition(1, new Vector3(1109.369f, 2129.074f, 52.364f), 329.213f),
            new VehicleRaceStartingPosition(2, new Vector3(1105.503f, 2131.377f, 52.312f), 329.213f),
            new VehicleRaceStartingPosition(3, new Vector3(1101.637f, 2133.681f, 52.301f), 329.213f),
            new VehicleRaceStartingPosition(4, new Vector3(1108.116f, 2118.180f, 52.435f), 329.213f),
            new VehicleRaceStartingPosition(5, new Vector3(1104.250f, 2120.483f, 52.216f), 329.213f),
            new VehicleRaceStartingPosition(6, new Vector3(1100.385f, 2122.787f, 52.379f), 329.213f),
            new VehicleRaceStartingPosition(7, new Vector3(1096.519f, 2125.090f, 52.366f), 329.213f),


            //proper start and checkpoints - ai ruin it - jumps too far on second jump after start
            //new VehicleRaceStartingPosition(0, new Vector3(1143.596f, 2340.700f, 53.888f), 178.057f),
            //new VehicleRaceStartingPosition(1, new Vector3(1141.597f, 2340.768f, 53.510f), 178.057f),
            //new VehicleRaceStartingPosition(2, new Vector3(1139.598f, 2340.836f, 53.462f), 178.057f),
            //new VehicleRaceStartingPosition(3, new Vector3(1137.599f, 2340.904f, 53.445f), 178.057f),
            //new VehicleRaceStartingPosition(4, new Vector3(1143.867f, 2348.696f, 53.757f), 178.057f),
            //new VehicleRaceStartingPosition(5, new Vector3(1141.868f, 2348.764f, 53.631f), 178.057f),
            //new VehicleRaceStartingPosition(6, new Vector3(1139.869f, 2348.832f, 53.605f), 178.057f),
            //new VehicleRaceStartingPosition(7, new Vector3(1137.870f, 2348.899f, 53.453f), 178.057f),
        };
        List<VehicleRaceCheckpoint> redwoodlightscheckpoints = new List<VehicleRaceCheckpoint>()
        {
            //new VehicleRaceCheckpoint(0, new Vector3(1164.515f, 2430.691f, 56.025f)),
            //new VehicleRaceCheckpoint(1, new Vector3(1102.386f, 2466.584f, 49.329f)),
            //new VehicleRaceCheckpoint(2, new Vector3(1021.597f, 2439.427f, 44.347f)),
            //new VehicleRaceCheckpoint(3, new Vector3(935.913f, 2485.307f, 49.808f)),
            //new VehicleRaceCheckpoint(4, new Vector3(892.683f, 2381.586f, 50.233f)),
            //new VehicleRaceCheckpoint(5, new Vector3(939.000f, 2248.406f, 44.757f)),
            //new VehicleRaceCheckpoint(6, new Vector3(1091.823f, 2190.635f, 49.019f)),
            //new VehicleRaceCheckpoint(7, new Vector3(1170.194f, 2176.593f, 53.584f)),
            //new VehicleRaceCheckpoint(8, new Vector3(1106.881f, 2252.506f, 48.682f)),
            //new VehicleRaceCheckpoint(9, new Vector3(1000.951f, 2255.023f, 46.824f)),
            //new VehicleRaceCheckpoint(10, new Vector3(973.927f, 2394.788f, 51.027f)),
            //new VehicleRaceCheckpoint(11, new Vector3(1118.158f, 2371.393f, 49.472f)),
            //new VehicleRaceCheckpoint(12, new Vector3(1165.624f, 2338.872f, 57.130f)),

            //new VehicleRaceCheckpoint(0, new Vector3(1170.230f, 2181.292f, 53.388f)),
            //new VehicleRaceCheckpoint(1, new Vector3(1102.884f, 2252.252f, 48.170f)),
            //new VehicleRaceCheckpoint(2, new Vector3(978.391f, 2269.582f, 47.687f)),
            //new VehicleRaceCheckpoint(3, new Vector3(970.784f, 2392.916f, 51.015f)),
            //new VehicleRaceCheckpoint(4, new Vector3(1117.921f, 2384.810f, 49.770f)),
            //new VehicleRaceCheckpoint(5, new Vector3(1165.601f, 2338.336f, 57.145f)),
            //new VehicleRaceCheckpoint(6, new Vector3(1096.280f, 2461.916f, 49.132f)),
            //new VehicleRaceCheckpoint(7, new Vector3(938.511f, 2484.254f, 49.651f)),
            //new VehicleRaceCheckpoint(8, new Vector3(892.969f, 2384.714f, 50.173f)),
            //new VehicleRaceCheckpoint(9, new Vector3(944.063f, 2246.684f, 44.878f)),
            //new VehicleRaceCheckpoint(10, new Vector3(1118.787f, 2153.987f, 52.924f)),

            // Big Jump/Crashes after
            //new VehicleRaceCheckpoint(0, new Vector3(1170.268f, 2181.037f, 53.412f)),
            //new VehicleRaceCheckpoint(1, new Vector3(964.952f, 2276.705f, 46.916f)),
            //new VehicleRaceCheckpoint(2, new Vector3(952.043f, 2354.239f, 48.126f)),
            //new VehicleRaceCheckpoint(3, new Vector3(999.786f, 2407.614f, 50.659f)),
            //new VehicleRaceCheckpoint(4, new Vector3(1165.632f, 2338.842f, 57.129f)),
            //new VehicleRaceCheckpoint(5, new Vector3(1098.847f, 2464.341f, 49.205f)),
            //new VehicleRaceCheckpoint(6, new Vector3(954.351f, 2478.291f, 49.089f)),
            //new VehicleRaceCheckpoint(7, new Vector3(892.767f, 2387.504f, 50.003f)),
            //new VehicleRaceCheckpoint(8, new Vector3(924.080f, 2254.702f, 45.026f)),
            //new VehicleRaceCheckpoint(9, new Vector3(1119.017f, 2154.571f, 52.979f)),


            //No Big Jump
            new VehicleRaceCheckpoint(0, new Vector3(1169.786f, 2181.430f, 53.349f)),
            new VehicleRaceCheckpoint(1, new Vector3(1121.631f, 2234.475f, 48.087f)),
            new VehicleRaceCheckpoint(2, new Vector3(979.995f, 2269.322f, 47.847f)),
            new VehicleRaceCheckpoint(3, new Vector3(952.061f, 2355.033f, 48.117f)),
            new VehicleRaceCheckpoint(4, new Vector3(1042.610f, 2410.226f, 52.973f)),
            new VehicleRaceCheckpoint(5, new Vector3(1165.728f, 2338.941f, 57.128f)),
            new VehicleRaceCheckpoint(6, new Vector3(1096.535f, 2461.734f, 49.146f)),
            new VehicleRaceCheckpoint(7, new Vector3(966.638f, 2466.768f, 49.790f)),
            new VehicleRaceCheckpoint(8, new Vector3(892.723f, 2385.208f, 50.138f)),
            new VehicleRaceCheckpoint(9, new Vector3(1028.928f, 2188.221f, 44.952f)),
            new VehicleRaceCheckpoint(10, new Vector3(1119.400f, 2154.566f, 52.972f)),


            //proper start and checkpoints - ai ruins it
            //new VehicleRaceCheckpoint(0, new Vector3(1165.250f, 2336.250f, 56.719f)),
            //new VehicleRaceCheckpoint(1, new Vector3(1099.750f, 2465.750f, 48.813f)),
            //new VehicleRaceCheckpoint(2, new Vector3(934.250f, 2485.250f, 49.375f)),
            //new VehicleRaceCheckpoint(3, new Vector3(872.500f, 2334.500f, 50.656f)),
            //new VehicleRaceCheckpoint(4, new Vector3(940.250f, 2247.000f, 44.406f)),
            //new VehicleRaceCheckpoint(5, new Vector3(1019.250f, 2192.250f, 44.156f)),
            //new VehicleRaceCheckpoint(6, new Vector3(1128.000f, 2156.750f, 52.594f)),
            //new VehicleRaceCheckpoint(7, new Vector3(1121.250f, 2238.250f, 47.938f)),
            //new VehicleRaceCheckpoint(8, new Vector3(970.750f, 2275.000f, 46.719f)),
            //new VehicleRaceCheckpoint(9, new Vector3(1021.750f, 2410.750f, 53.031f)),

        };
        VehicleRaceTrack redwoodlights = new VehicleRaceTrack("redwoodlights", "Circuit - Redwood Lights MotoX", "Dirt, Danger, Domination on Bikes", redwoodlightscheckpoints, redwoodlightsstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(redwoodlights);


        // Drag
        List<VehicleRaceStartingPosition> paletocovestart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-1273.619f, 5364.494f, 2.341f), 279.431f),
            new VehicleRaceStartingPosition(1, new Vector3(-1273.127f, 5361.534f, 2.399f), 279.431f),
            new VehicleRaceStartingPosition(2, new Vector3(-1283.484f, 5362.855f, 2.005f), 279.431f),
            new VehicleRaceStartingPosition(3, new Vector3(-1282.992f, 5359.896f, 2.054f), 279.431f),
            new VehicleRaceStartingPosition(4, new Vector3(-1293.348f, 5361.216f, 2.023f), 279.431f),
            new VehicleRaceStartingPosition(5, new Vector3(-1292.857f, 5358.257f, 2.179f), 279.431f),
            new VehicleRaceStartingPosition(6, new Vector3(-1303.213f, 5359.578f, 2.112f), 279.431f),
            new VehicleRaceStartingPosition(7, new Vector3(-1302.722f, 5356.618f, 2.296f), 279.431f),
        };
        List<VehicleRaceCheckpoint> paletocovecheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-1089.750f, 5445.500f, 2.656f)),
            new VehicleRaceCheckpoint(1, new Vector3(-937.000f, 5575.000f, 2.094f)),
            new VehicleRaceCheckpoint(2, new Vector3(-862.250f, 5829.000f, 2.156f)),
        };
        VehicleRaceTrack paletocove1 = new VehicleRaceTrack("paletocove", "Drag - Paleto Cove Beach", "Race Along the Paleto Cove Beach", paletocovecheckpoints, paletocovestart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(paletocove1);

        List<VehicleRaceStartingPosition> procopiobeachstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(1267.750f, 6576.631f, 1.675f), 97.790f),
            new VehicleRaceStartingPosition(1, new Vector3(1267.275f, 6580.099f, 1.583f), 97.790f),
            new VehicleRaceStartingPosition(2, new Vector3(1277.657f, 6577.987f, 1.697f), 97.790f),
            new VehicleRaceStartingPosition(3, new Vector3(1277.183f, 6581.455f, 1.605f), 97.790f),
            new VehicleRaceStartingPosition(4, new Vector3(1287.565f, 6579.342f, 1.657f), 97.790f),
            new VehicleRaceStartingPosition(5, new Vector3(1287.091f, 6582.810f, 1.634f), 97.790f),
            new VehicleRaceStartingPosition(6, new Vector3(1297.473f, 6580.697f, 1.473f), 97.790f),
            new VehicleRaceStartingPosition(7, new Vector3(1296.999f, 6584.165f, 1.473f), 97.790f),
        };
        List<VehicleRaceCheckpoint> procopiobeachcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(1121.000f, 6590.750f, 1.563f)),
            new VehicleRaceCheckpoint(1, new Vector3(911.000f, 6587.000f, 5.875f)),
            new VehicleRaceCheckpoint(2, new Vector3(704.000f, 6609.750f, 3.813f)),
            new VehicleRaceCheckpoint(3, new Vector3(503.750f, 6681.250f, 7.719f)),
            new VehicleRaceCheckpoint(4, new Vector3(370.500f, 6823.250f, 3.469f)),
        };
        VehicleRaceTrack procopiobeach = new VehicleRaceTrack("procopio_beach", "Drag - Procopio Beach", "Race Along the Procopio Beach", procopiobeachcheckpoints, procopiobeachstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(procopiobeach);

        List<VehicleRaceStartingPosition> sandyoffroaddragstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(2045.519f, 3661.220f, 33.398f), 120.939f),
            new VehicleRaceStartingPosition(1, new Vector3(2043.977f, 3663.793f, 33.436f), 120.939f),
            new VehicleRaceStartingPosition(2, new Vector3(2054.097f, 3666.361f, 33.387f), 120.939f),
            new VehicleRaceStartingPosition(3, new Vector3(2052.554f, 3668.934f, 33.391f), 120.939f),
            new VehicleRaceStartingPosition(4, new Vector3(2062.674f, 3671.502f, 33.697f), 120.939f),
            new VehicleRaceStartingPosition(5, new Vector3(2061.131f, 3674.075f, 33.569f), 120.939f),
            new VehicleRaceStartingPosition(6, new Vector3(2071.251f, 3676.644f, 34.202f), 120.939f),
            new VehicleRaceStartingPosition(7, new Vector3(2069.708f, 3679.217f, 33.887f), 120.939f),
        };
        List<VehicleRaceCheckpoint> sandyoffroaddragcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(1884.148f, 3573.171f, 36.086f)),
            new VehicleRaceCheckpoint(1, new Vector3(1651.201f, 3445.719f, 36.202f)),
            new VehicleRaceCheckpoint(2, new Vector3(1396.961f, 3331.467f, 38.040f)),
            new VehicleRaceCheckpoint(3, new Vector3(1008.324f, 3237.230f, 37.886f)),
            new VehicleRaceCheckpoint(4, new Vector3(666.655f, 3196.230f, 38.854f)),
            new VehicleRaceCheckpoint(5, new Vector3(315.164f, 3278.099f, 43.054f)),
        };
        VehicleRaceTrack sandyoffroaddrag = new VehicleRaceTrack("sandyoffroaddrag", "Drag - Sandy Shores Offroad Drag", "Race Alongside the Railway through Sandy Shores", sandyoffroaddragcheckpoints, sandyoffroaddragstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(sandyoffroaddrag);


        // Point 2 Point
        List<VehicleRaceStartingPosition> canyoncliffsstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-1891.716f, 4430.739f, 44.045f), 250.115f),
            new VehicleRaceStartingPosition(1, new Vector3(-1892.736f, 4427.917f, 43.995f), 250.115f),
            new VehicleRaceStartingPosition(2, new Vector3(-1899.239f, 4433.460f, 42.750f), 250.115f),
            new VehicleRaceStartingPosition(3, new Vector3(-1900.259f, 4430.639f, 42.746f), 250.115f),
            new VehicleRaceStartingPosition(4, new Vector3(-1906.762f, 4436.181f, 41.317f), 250.115f),
            new VehicleRaceStartingPosition(5, new Vector3(-1907.782f, 4433.360f, 41.144f), 250.115f),
            new VehicleRaceStartingPosition(6, new Vector3(-1914.285f, 4438.902f, 40.074f), 250.115f),
            new VehicleRaceStartingPosition(7, new Vector3(-1915.305f, 4436.081f, 40.127f), 250.115f),
        };
        List<VehicleRaceCheckpoint> canyoncliffscheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-1866.1970f, 4416.5698f, 47.6783f)),
            new VehicleRaceCheckpoint(1, new Vector3(-1719.3030f, 4323.6265f, 64.5414f)),
            new VehicleRaceCheckpoint(2, new Vector3(-1649.4390f, 4210.0781f, 82.7008f)),
            new VehicleRaceCheckpoint(3, new Vector3(-1559.9771f, 4206.7441f, 75.5405f)),
            new VehicleRaceCheckpoint(4, new Vector3(-1468.9580f, 4225.5361f, 52.2794f)),
            new VehicleRaceCheckpoint(5, new Vector3(-1386.4840f, 4165.8062f, 51.7286f)),
            new VehicleRaceCheckpoint(6, new Vector3(-1345.5850f, 4126.9629f, 61.8040f)),
            new VehicleRaceCheckpoint(7, new Vector3(-1321.0000f, 4186.4370f, 62.0636f)),
            new VehicleRaceCheckpoint(8, new Vector3(-1276.7581f, 4278.8252f, 65.3138f)),
            new VehicleRaceCheckpoint(9, new Vector3(-1185.7729f, 4291.9038f, 78.4686f)),
            new VehicleRaceCheckpoint(10, new Vector3(-1072.9340f, 4272.6211f, 100.6904f)),
            new VehicleRaceCheckpoint(11, new Vector3(-1038.1479f, 4229.4219f, 115.7768f)),
            new VehicleRaceCheckpoint(12, new Vector3(-1032.5160f, 4165.0562f, 118.9887f)),
            new VehicleRaceCheckpoint(13, new Vector3(-938.7848f, 4148.5850f, 140.8003f)),
            new VehicleRaceCheckpoint(14, new Vector3(-891.4792f, 4096.8848f, 161.8249f)),
            new VehicleRaceCheckpoint(15, new Vector3(-823.6310f, 4051.6890f, 162.4706f)),
            new VehicleRaceCheckpoint(16, new Vector3(-753.7779f, 4038.2029f, 147.1398f)),
            new VehicleRaceCheckpoint(17, new Vector3(-654.0638f, 4013.6890f, 126.4406f)),
            new VehicleRaceCheckpoint(18, new Vector3(-591.6994f, 3972.3940f, 113.3035f)),
            new VehicleRaceCheckpoint(19, new Vector3(-515.1089f, 3960.3130f, 87.3917f)),
            new VehicleRaceCheckpoint(20, new Vector3(-425.4944f, 3943.4951f, 65.9412f)),
            new VehicleRaceCheckpoint(21, new Vector3(-379.7583f, 3981.8640f, 52.2577f)),
            new VehicleRaceCheckpoint(22, new Vector3(-330.6125f, 4012.5090f, 45.0532f)),
            new VehicleRaceCheckpoint(23, new Vector3(-252.5999f, 3920.9768f, 39.3600f)),
        };
        VehicleRaceTrack canyoncliffs = new VehicleRaceTrack("canyon_cliffs", "P2P - Canyon Cliffs", "Race Through the Mount Josiah Trail", canyoncliffscheckpoints, canyoncliffsstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(canyoncliffs); // R Conversion

        List<VehicleRaceStartingPosition> canyoncliffs2start = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-278.793f, 3950.895f, 41.968f), 38.797f),
            new VehicleRaceStartingPosition(1, new Vector3(-276.065f, 3953.088f, 42.014f), 38.797f),
            new VehicleRaceStartingPosition(2, new Vector3(-273.780f, 3944.660f, 41.703f), 38.797f),
            new VehicleRaceStartingPosition(3, new Vector3(-271.052f, 3946.853f, 41.795f), 38.797f),
            new VehicleRaceStartingPosition(4, new Vector3(-268.768f, 3938.425f, 41.536f), 38.797f),
            new VehicleRaceStartingPosition(5, new Vector3(-266.040f, 3940.618f, 41.554f), 38.797f),
            new VehicleRaceStartingPosition(6, new Vector3(-263.755f, 3932.190f, 41.034f), 38.797f),
            new VehicleRaceStartingPosition(7, new Vector3(-261.027f, 3934.383f, 41.078f), 38.797f),
            new VehicleRaceStartingPosition(8, new Vector3(-258.743f, 3925.955f, 40.498f), 38.797f),
            new VehicleRaceStartingPosition(9, new Vector3(-256.015f, 3928.148f, 40.462f), 38.797f),
        };
        List<VehicleRaceCheckpoint> canyoncliffs2checkpoints = new List<VehicleRaceCheckpoint>()
        {

            new VehicleRaceCheckpoint(0, new Vector3(-330.6125f, 4012.5090f, 45.0532f)),
            new VehicleRaceCheckpoint(1, new Vector3(-379.7583f, 3981.8640f, 52.2577f)),
            new VehicleRaceCheckpoint(2, new Vector3(-425.4944f, 3943.4951f, 65.9412f)),
            new VehicleRaceCheckpoint(3, new Vector3(-515.1089f, 3960.3130f, 87.3917f)),
            new VehicleRaceCheckpoint(4, new Vector3(-591.6994f, 3972.3940f, 113.3035f)),
            new VehicleRaceCheckpoint(5, new Vector3(-654.0638f, 4013.6890f, 126.4406f)),
            new VehicleRaceCheckpoint(6, new Vector3(-753.7779f, 4038.2029f, 147.1398f)),
            new VehicleRaceCheckpoint(7, new Vector3(-823.6310f, 4051.6890f, 162.4706f)),
            new VehicleRaceCheckpoint(8, new Vector3(-891.4792f, 4096.8848f, 161.8249f)),
            new VehicleRaceCheckpoint(9, new Vector3(-938.7848f, 4148.5850f, 140.8003f)),
            new VehicleRaceCheckpoint(10, new Vector3(-1032.5160f, 4165.0562f, 118.9887f)),
            new VehicleRaceCheckpoint(11, new Vector3(-1038.1479f, 4229.4219f, 115.7768f)),
            new VehicleRaceCheckpoint(12, new Vector3(-1072.9340f, 4272.6211f, 100.6904f)),
            new VehicleRaceCheckpoint(13, new Vector3(-1185.7729f, 4291.9038f, 78.4686f)),
            new VehicleRaceCheckpoint(14, new Vector3(-1276.7581f, 4278.8252f, 65.3138f)),
            new VehicleRaceCheckpoint(15, new Vector3(-1321.0000f, 4186.4370f, 62.0636f)),
            new VehicleRaceCheckpoint(16, new Vector3(-1345.5850f, 4126.9629f, 61.8040f)),
            new VehicleRaceCheckpoint(17, new Vector3(-1386.4840f, 4165.8062f, 51.7286f)),
            new VehicleRaceCheckpoint(18, new Vector3(-1468.9580f, 4225.5361f, 52.2794f)),
            new VehicleRaceCheckpoint(19, new Vector3(-1559.9771f, 4206.7441f, 75.5405f)),
            new VehicleRaceCheckpoint(20, new Vector3(-1649.4390f, 4210.0781f, 82.7008f)),
            new VehicleRaceCheckpoint(21, new Vector3(-1719.3030f, 4323.6265f, 64.5414f)),
            new VehicleRaceCheckpoint(22, new Vector3(-1866.1970f, 4416.5698f, 47.6783f)),
        };
        VehicleRaceTrack canyoncliffs2 = new VehicleRaceTrack("canyoncliffs2", "P2P - Canyon Cliffs Reverse", "Race Through the Mount Josiah Trail", canyoncliffs2checkpoints, canyoncliffs2start);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(canyoncliffs2);


        List<VehicleRaceStartingPosition> chiliadtrail1start = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-1071.856f, 5320.836f, 45.542f), 290.372f),
            new VehicleRaceStartingPosition(1, new Vector3(-1073.075f, 5324.117f, 45.524f), 290.372f),
            new VehicleRaceStartingPosition(2, new Vector3(-1081.231f, 5317.355f, 46.343f), 290.372f),
            new VehicleRaceStartingPosition(3, new Vector3(-1082.449f, 5320.636f, 46.333f), 290.372f),
            new VehicleRaceStartingPosition(4, new Vector3(-1090.605f, 5313.874f, 47.173f), 290.372f),
            new VehicleRaceStartingPosition(5, new Vector3(-1091.824f, 5317.155f, 47.163f), 290.372f),
            new VehicleRaceStartingPosition(6, new Vector3(-1099.980f, 5310.393f, 48.022f), 290.372f),
            new VehicleRaceStartingPosition(7, new Vector3(-1101.198f, 5313.673f, 47.996f), 290.372f),
        };
        List<VehicleRaceCheckpoint> chiliadtrail1checkpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-942.327f, 5407.709f, 37.812f)),
            new VehicleRaceCheckpoint(1, new Vector3(-763.618f, 5436.007f, 37.357f)),
            new VehicleRaceCheckpoint(2, new Vector3(-658.253f, 5375.653f, 55.169f)),
            new VehicleRaceCheckpoint(3, new Vector3(-732.610f, 5318.406f, 72.291f)),
            new VehicleRaceCheckpoint(4, new Vector3(-942.603f, 5270.009f, 81.186f)),
            new VehicleRaceCheckpoint(5, new Vector3(-794.194f, 5263.945f, 88.358f)),
            new VehicleRaceCheckpoint(6, new Vector3(-734.391f, 5214.547f, 100.853f)),
            new VehicleRaceCheckpoint(7, new Vector3(-664.066f, 5134.274f, 123.681f)),
            new VehicleRaceCheckpoint(8, new Vector3(-596.028f, 5037.023f, 139.113f)),
            new VehicleRaceCheckpoint(9, new Vector3(-520.028f, 4946.719f, 146.786f)),
            new VehicleRaceCheckpoint(10, new Vector3(-105.683f, 4594.683f, 122.503f)),
        };
        VehicleRaceTrack chiliadtrail1 = new VehicleRaceTrack("chiliad_trail1", "P2P - Chiliad Trail", "Race through Chiliad Trail", chiliadtrail1checkpoints, chiliadtrail1start);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(chiliadtrail1);

        List<VehicleRaceStartingPosition> grandsenoratrail1start = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(40.533f, 1857.261f, 195.131f), 302.929f),
            new VehicleRaceStartingPosition(1, new Vector3(42.979f, 1853.484f, 195.253f), 302.929f),
            new VehicleRaceStartingPosition(2, new Vector3(32.139f, 1851.825f, 198.235f), 302.929f),
            new VehicleRaceStartingPosition(3, new Vector3(34.585f, 1848.048f, 198.272f), 302.929f),
            new VehicleRaceStartingPosition(4, new Vector3(23.746f, 1846.389f, 201.322f), 302.929f),
            new VehicleRaceStartingPosition(5, new Vector3(26.192f, 1842.612f, 201.326f), 302.929f),
            new VehicleRaceStartingPosition(6, new Vector3(15.352f, 1840.953f, 204.361f), 302.929f),
            new VehicleRaceStartingPosition(7, new Vector3(17.798f, 1837.176f, 204.542f), 302.929f),
        };
        List<VehicleRaceCheckpoint> grandsenoratrail1checkpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(100.745f, 1948.350f, 168.497f)),
            new VehicleRaceCheckpoint(1, new Vector3(-23.447f, 2092.312f, 152.962f)),
            new VehicleRaceCheckpoint(2, new Vector3(2.894f, 2269.544f, 113.630f)),
            new VehicleRaceCheckpoint(3, new Vector3(218.759f, 2266.377f, 83.998f)),
            new VehicleRaceCheckpoint(4, new Vector3(211.775f, 2388.959f, 67.888f)),
            new VehicleRaceCheckpoint(5, new Vector3(317.958f, 2472.817f, 49.495f)),
        };
        VehicleRaceTrack grandsenoratrail1 = new VehicleRaceTrack("grandsenoratrail1", "P2P - Grand Senora Downhill Trail", "Fast downhill trail race", grandsenoratrail1checkpoints, grandsenoratrail1start);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(grandsenoratrail1);


        //List<VehicleRaceStartingPosition> greatchaparralstart = new List<VehicleRaceStartingPosition>()
        //{
        //    new VehicleRaceStartingPosition(0, new Vector3(-172.942f, 2840.683f, 33.420f), 182.860f),
        //    new VehicleRaceStartingPosition(1, new Vector3(-176.937f, 2840.483f, 33.407f), 182.860f),
        //    new VehicleRaceStartingPosition(2, new Vector3(-173.441f, 2850.670f, 32.161f), 182.860f),
        //    new VehicleRaceStartingPosition(3, new Vector3(-177.436f, 2850.471f, 32.187f), 182.860f),
        //    new VehicleRaceStartingPosition(4, new Vector3(-173.940f, 2860.658f, 30.898f), 182.860f),
        //    new VehicleRaceStartingPosition(5, new Vector3(-177.935f, 2860.458f, 30.914f), 182.860f),
        //    new VehicleRaceStartingPosition(6, new Vector3(-174.439f, 2870.646f, 29.401f), 182.860f),
        //    new VehicleRaceStartingPosition(7, new Vector3(-178.434f, 2870.446f, 29.368f), 182.860f),
        //};
        //List<VehicleRaceCheckpoint> greatchaparralcheckpoints = new List<VehicleRaceCheckpoint>()
        //{
        //    new VehicleRaceCheckpoint(0, new Vector3(-177.905f, 2417.912f, 87.150f)),
        //    new VehicleRaceCheckpoint(1, new Vector3(-183.563f, 2257.925f, 116.746f)),
        //    new VehicleRaceCheckpoint(2, new Vector3(-255.557f, 2149.871f, 131.624f)),
        //    new VehicleRaceCheckpoint(3, new Vector3(-269.243f, 2060.877f, 139.570f)),
        //    new VehicleRaceCheckpoint(4, new Vector3(-311.070f, 1966.487f, 153.018f)),
        //    new VehicleRaceCheckpoint(5, new Vector3(-280.814f, 1953.431f, 162.752f)),
        //    new VehicleRaceCheckpoint(6, new Vector3(-249.335f, 1951.614f, 177.589f)),
        //    new VehicleRaceCheckpoint(7, new Vector3(-209.576f, 1910.055f, 193.765f)),
        //};
        //VehicleRaceTrack greatchaparral = new VehicleRaceTrack("greatchaparral", "Great Chaparral Trail", "Race up the Great Chaparral Trail", greatchaparralcheckpoints, greatchaparralstart);
        //VehicleRaceTypeManager.VehicleRaceTracks.Add(greatchaparral);

        //List<VehicleRaceStartingPosition> mountjosiahstart = new List<VehicleRaceStartingPosition>()
        //{
        //    new VehicleRaceStartingPosition(0, new Vector3(-277.174f, 3948.329f, 41.880f), 40.293f),
        //    new VehicleRaceStartingPosition(1, new Vector3(-274.123f, 3950.915f, 41.946f), 40.293f),
        //    new VehicleRaceStartingPosition(2, new Vector3(-270.707f, 3940.701f, 41.625f), 40.293f),
        //    new VehicleRaceStartingPosition(3, new Vector3(-267.656f, 3943.288f, 41.623f), 40.293f),
        //    new VehicleRaceStartingPosition(4, new Vector3(-264.240f, 3933.073f, 41.155f), 40.293f),
        //    new VehicleRaceStartingPosition(5, new Vector3(-261.189f, 3935.660f, 41.150f), 40.293f),
        //    new VehicleRaceStartingPosition(6, new Vector3(-257.773f, 3925.446f, 40.149f), 40.293f),
        //    new VehicleRaceStartingPosition(7, new Vector3(-254.722f, 3928.033f, 40.233f), 40.293f),
        //};
        //List<VehicleRaceCheckpoint> mountjosiahcheckpoints = new List<VehicleRaceCheckpoint>()
        //{
        //    new VehicleRaceCheckpoint(0, new Vector3(-311.750f, 3996.000f, 42.656f)),
        //    new VehicleRaceCheckpoint(1, new Vector3(-451.000f, 3959.500f, 69.375f)),
        //    new VehicleRaceCheckpoint(2, new Vector3(-628.750f, 3996.250f, 121.156f)),
        //    new VehicleRaceCheckpoint(3, new Vector3(-809.000f, 4051.000f, 160.375f)),
        //    new VehicleRaceCheckpoint(4, new Vector3(-926.000f, 4132.750f, 148.375f)),
        //    new VehicleRaceCheckpoint(5, new Vector3(-1037.000f, 4187.500f, 118.219f)),
        //    new VehicleRaceCheckpoint(6, new Vector3(-1172.000f, 4289.000f, 80.969f)),
        //    new VehicleRaceCheckpoint(7, new Vector3(-1307.750f, 4211.000f, 60.063f)),
        //    new VehicleRaceCheckpoint(8, new Vector3(-1417.750f, 4199.500f, 47.094f)),
        //    new VehicleRaceCheckpoint(9, new Vector3(-1592.500f, 4200.750f, 80.656f)),
        //    new VehicleRaceCheckpoint(10, new Vector3(-1719.250f, 4323.250f, 64.563f)),
        //    new VehicleRaceCheckpoint(11, new Vector3(-1897.500f, 4431.000f, 43.063f)),
        //};
        //VehicleRaceTrack mountjosiahtrail = new VehicleRaceTrack("mountjosiah1", "Mount Josiah Trail", "Race Through the Mount Josiah Trail", mountjosiahcheckpoints, mountjosiahstart);
        //VehicleRaceTypeManager.VehicleRaceTracks.Add(mountjosiahtrail);

        //List<VehicleRaceStartingPosition> mountjosiahrevstart = new List<VehicleRaceStartingPosition>()
        //{
        //    new VehicleRaceStartingPosition(0, new Vector3(-1863.854f, 4417.406f, 47.998f), 240.950f),
        //    new VehicleRaceStartingPosition(1, new Vector3(-1865.553f, 4414.346f, 47.833f), 240.950f),
        //    new VehicleRaceStartingPosition(2, new Vector3(-1872.595f, 4422.262f, 46.923f), 240.950f),
        //    new VehicleRaceStartingPosition(3, new Vector3(-1874.295f, 4419.202f, 46.749f), 240.950f),
        //    new VehicleRaceStartingPosition(4, new Vector3(-1881.337f, 4427.117f, 45.681f), 240.950f),
        //    new VehicleRaceStartingPosition(5, new Vector3(-1883.037f, 4424.058f, 45.537f), 240.950f),
        //    new VehicleRaceStartingPosition(6, new Vector3(-1890.079f, 4431.973f, 44.089f), 240.950f),
        //    new VehicleRaceStartingPosition(7, new Vector3(-1891.779f, 4428.913f, 44.118f), 240.950f),
        //};
        //List<VehicleRaceCheckpoint> mountjosiahrevcheckpoints = new List<VehicleRaceCheckpoint>()
        //{
        //    new VehicleRaceCheckpoint(0, new Vector3(-1771.750f, 4367.500f, 55.875f)),
        //    new VehicleRaceCheckpoint(1, new Vector3(-1684.250f, 4262.000f, 75.219f)),
        //    new VehicleRaceCheckpoint(2, new Vector3(-1532.250f, 4212.750f, 67.469f)),
        //    new VehicleRaceCheckpoint(3, new Vector3(-1392.250f, 4170.500f, 50.000f)),
        //    new VehicleRaceCheckpoint(4, new Vector3(-1252.500f, 4287.000f, 69.125f)),
        //    new VehicleRaceCheckpoint(5, new Vector3(-1037.000f, 4220.750f, 116.688f)),
        //    new VehicleRaceCheckpoint(6, new Vector3(-913.750f, 4115.500f, 155.125f)),
        //    new VehicleRaceCheckpoint(7, new Vector3(-742.000f, 4033.500f, 143.969f)),
        //    new VehicleRaceCheckpoint(8, new Vector3(-521.750f, 3958.250f, 89.625f)),
        //    new VehicleRaceCheckpoint(9, new Vector3(-368.000f, 3995.500f, 49.438f)),
        //    new VehicleRaceCheckpoint(10, new Vector3(-288.000f, 3965.000f, 42.125f)),
        //};
        //VehicleRaceTrack mountjosiahrev = new VehicleRaceTrack("mountjosiahrev1", "Mount Josiah Trail (Reverse)", "Race Through the Mount Josiah Trail", mountjosiahrevcheckpoints, mountjosiahrevstart);
        //VehicleRaceTypeManager.VehicleRaceTracks.Add(mountjosiahrev);


        List<VehicleRaceStartingPosition> landactresoviorroute1start = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(1255.547f, -285.845f, 76.887f), 267.651f),
            new VehicleRaceStartingPosition(1, new Vector3(1255.690f, -282.348f, 76.899f), 267.651f),
            new VehicleRaceStartingPosition(2, new Vector3(1247.554f, -285.517f, 75.498f), 267.651f),
            new VehicleRaceStartingPosition(3, new Vector3(1247.697f, -282.020f, 75.372f), 267.651f),
            new VehicleRaceStartingPosition(4, new Vector3(1239.560f, -285.189f, 72.964f), 267.651f),
            new VehicleRaceStartingPosition(5, new Vector3(1239.704f, -281.692f, 72.969f), 267.651f),
            new VehicleRaceStartingPosition(6, new Vector3(1231.567f, -284.861f, 70.377f), 267.651f),
            new VehicleRaceStartingPosition(7, new Vector3(1231.710f, -281.364f, 71.058f), 267.651f),
        };
        List<VehicleRaceCheckpoint> landactresoviorroute1checkpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(1284.250f, -241.250f, 96.313f)),
            new VehicleRaceCheckpoint(1, new Vector3(1390.500f, -120.250f, 125.719f)),
            new VehicleRaceCheckpoint(2, new Vector3(1596.000f, -73.500f, 162.094f)),
            new VehicleRaceCheckpoint(3, new Vector3(1713.750f, -88.000f, 176.938f)),
            new VehicleRaceCheckpoint(4, new Vector3(1869.750f, -87.250f, 188.531f)),
            new VehicleRaceCheckpoint(5, new Vector3(2075.500f, -5.000f, 212.281f)),
            new VehicleRaceCheckpoint(6, new Vector3(2197.000f, 110.750f, 227.875f)),
            new VehicleRaceCheckpoint(7, new Vector3(2312.000f, 212.250f, 201.750f)),
            new VehicleRaceCheckpoint(8, new Vector3(2381.500f, 362.750f, 175.781f)),
            new VehicleRaceCheckpoint(9, new Vector3(2414.750f, 506.500f, 152.500f)),
            new VehicleRaceCheckpoint(10, new Vector3(2414.000f, 732.750f, 126.938f)),
            new VehicleRaceCheckpoint(11, new Vector3(2379.750f, 885.250f, 110.750f)),
            new VehicleRaceCheckpoint(12, new Vector3(2268.500f, 1065.750f, 68.500f)),
        };
        VehicleRaceTrack landactresoviorroute1 = new VehicleRaceTrack("landactresoviorroute1", "P2P - Land Act Reservoir Route 1", "Race at the Land Act Reservoir", landactresoviorroute1checkpoints, landactresoviorroute1start);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(landactresoviorroute1);

        List<VehicleRaceStartingPosition> landactresoviorroute2start = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(1255.547f, -285.845f, 76.887f), 267.651f),
            new VehicleRaceStartingPosition(1, new Vector3(1255.690f, -282.348f, 76.899f), 267.651f),
            new VehicleRaceStartingPosition(2, new Vector3(1247.554f, -285.517f, 75.498f), 267.651f),
            new VehicleRaceStartingPosition(3, new Vector3(1247.697f, -282.020f, 75.372f), 267.651f),
            new VehicleRaceStartingPosition(4, new Vector3(1239.560f, -285.189f, 72.964f), 267.651f),
            new VehicleRaceStartingPosition(5, new Vector3(1239.704f, -281.692f, 72.969f), 267.651f),
            new VehicleRaceStartingPosition(6, new Vector3(1231.567f, -284.861f, 70.377f), 267.651f),
            new VehicleRaceStartingPosition(7, new Vector3(1231.710f, -281.364f, 71.058f), 267.651f),
        };
        List<VehicleRaceCheckpoint> landactresoviorroute2checkpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(1284.250f, -241.250f, 96.313f)),
            new VehicleRaceCheckpoint(1, new Vector3(1390.500f, -120.250f, 125.719f)),
            new VehicleRaceCheckpoint(2, new Vector3(1596.000f, -73.500f, 162.094f)),
            new VehicleRaceCheckpoint(3, new Vector3(1666.000f, -14.750f, 172.750f)),
            new VehicleRaceCheckpoint(4, new Vector3(1669.500f, 61.000f, 171.031f)),
            new VehicleRaceCheckpoint(5, new Vector3(1824.500f, 208.250f, 172.313f)),
            new VehicleRaceCheckpoint(6, new Vector3(1835.250f, 476.000f, 171.313f)),
            new VehicleRaceCheckpoint(7, new Vector3(1946.750f, 613.250f, 175.594f)),
            new VehicleRaceCheckpoint(8, new Vector3(1913.500f, 779.000f, 193.344f)),
            new VehicleRaceCheckpoint(9, new Vector3(1972.750f, 935.000f, 213.000f)),
            new VehicleRaceCheckpoint(10, new Vector3(2023.250f, 1103.250f, 197.438f)),
            new VehicleRaceCheckpoint(11, new Vector3(1971.750f, 1252.000f, 175.094f)),
            new VehicleRaceCheckpoint(12, new Vector3(1838.250f, 1277.500f, 144.281f)),
            new VehicleRaceCheckpoint(13, new Vector3(1720.500f, 1184.500f, 128.406f)),
            new VehicleRaceCheckpoint(14, new Vector3(1648.750f, 1033.500f, 112.438f)),
        };
        VehicleRaceTrack landactresoviorroute2 = new VehicleRaceTrack("landactresoviorroute2", "P2P - Land Act Reservoir Route 2", "Race at the Land Act Reservoir", landactresoviorroute2checkpoints, landactresoviorroute2start);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(landactresoviorroute2);

        List<VehicleRaceStartingPosition> minewardspiralstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(2976.699f, 2752.051f, 42.131f), 310.453f),
            new VehicleRaceStartingPosition(1, new Vector3(2974.753f, 2754.334f, 42.194f), 310.453f),
            new VehicleRaceStartingPosition(2, new Vector3(2970.612f, 2746.861f, 42.379f), 310.453f),
            new VehicleRaceStartingPosition(3, new Vector3(2968.666f, 2749.144f, 42.369f), 310.453f),
            new VehicleRaceStartingPosition(4, new Vector3(2964.524f, 2741.670f, 42.554f), 310.453f),
            new VehicleRaceStartingPosition(5, new Vector3(2962.578f, 2743.953f, 42.587f), 310.453f),
            new VehicleRaceStartingPosition(6, new Vector3(2958.437f, 2736.479f, 43.110f), 310.453f),
            new VehicleRaceStartingPosition(7, new Vector3(2956.490f, 2738.762f, 43.165f), 310.453f),
        };
        List<VehicleRaceCheckpoint> minewardspiralcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(2979.9241f, 2810.3779f, 43.1119f)),
            new VehicleRaceCheckpoint(1, new Vector3(2948.2971f, 2840.8083f, 46.0740f)),
            new VehicleRaceCheckpoint(2, new Vector3(2907.5552f, 2819.5347f, 52.9237f)),
            new VehicleRaceCheckpoint(3, new Vector3(2896.7947f, 2785.0933f, 53.6336f)),
            new VehicleRaceCheckpoint(4, new Vector3(2923.7598f, 2744.9326f, 52.6429f)),
            new VehicleRaceCheckpoint(5, new Vector3(2978.9668f, 2728.0071f, 53.3941f)),
            new VehicleRaceCheckpoint(6, new Vector3(3017.3799f, 2777.6450f, 52.6012f)),
            new VehicleRaceCheckpoint(7, new Vector3(2972.6665f, 2855.1680f, 55.8068f)),
            new VehicleRaceCheckpoint(8, new Vector3(2929.9902f, 2878.2051f, 59.7260f)),
            new VehicleRaceCheckpoint(9, new Vector3(2899.2019f, 2852.9939f, 63.2669f)),
            new VehicleRaceCheckpoint(10, new Vector3(2851.1094f, 2865.7749f, 55.7528f)),
            new VehicleRaceCheckpoint(11, new Vector3(2817.6074f, 2901.4016f, 44.9261f)),
            new VehicleRaceCheckpoint(12, new Vector3(2833.6816f, 2871.7195f, 47.2565f)),
            new VehicleRaceCheckpoint(13, new Vector3(2851.9697f, 2827.0210f, 51.5760f)),
            new VehicleRaceCheckpoint(14, new Vector3(2877.8904f, 2780.0374f, 57.3181f)),
            new VehicleRaceCheckpoint(15, new Vector3(2913.5400f, 2730.8879f, 61.8753f)),
            new VehicleRaceCheckpoint(16, new Vector3(2956.1931f, 2682.4861f, 63.3001f)),
            new VehicleRaceCheckpoint(17, new Vector3(3010.5291f, 2715.8569f, 63.2056f)),
            new VehicleRaceCheckpoint(18, new Vector3(3047.1565f, 2767.0901f, 66.9343f)),
            new VehicleRaceCheckpoint(19, new Vector3(3021.0540f, 2807.9880f, 65.2655f)),
            new VehicleRaceCheckpoint(20, new Vector3(2991.9707f, 2884.7349f, 60.2772f)),
            new VehicleRaceCheckpoint(21, new Vector3(3011.7900f, 2945.6609f, 65.9224f)),
            new VehicleRaceCheckpoint(22, new Vector3(2955.9341f, 2911.6580f, 70.9026f)),
            new VehicleRaceCheckpoint(23, new Vector3(2984.3101f, 2945.6201f, 78.5753f)),
            new VehicleRaceCheckpoint(24, new Vector3(3034.9080f, 2967.0039f, 70.6033f)),
            new VehicleRaceCheckpoint(25, new Vector3(3014.1589f, 2879.1411f, 71.7657f)),
            new VehicleRaceCheckpoint(26, new Vector3(3039.5911f, 2812.7000f, 70.0086f)),
            new VehicleRaceCheckpoint(27, new Vector3(3023.2141f, 2733.8689f, 60.6356f)),
            new VehicleRaceCheckpoint(28, new Vector3(2944.2222f, 2713.8491f, 53.4180f)),
            new VehicleRaceCheckpoint(29, new Vector3(2896.5930f, 2789.7310f, 53.8335f)),
            new VehicleRaceCheckpoint(30, new Vector3(2859.381f, 2817.899f, 52.93278f)),
        };
        VehicleRaceTrack minewardspiral = new VehicleRaceTrack("minewardspiral", "P2P - Mineward Spiral", "Race through the Davis Quartz Quarry", minewardspiralcheckpoints, minewardspiralstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(minewardspiral); // R Conversion

        List<VehicleRaceStartingPosition> minewardspiralrevstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(2836.948f, 2868.343f, 47.547f), 207.456f),
            new VehicleRaceStartingPosition(1, new Vector3(2834.286f, 2866.959f, 47.575f), 207.456f),
            new VehicleRaceStartingPosition(2, new Vector3(2833.260f, 2875.442f, 47.111f), 207.456f),
            new VehicleRaceStartingPosition(3, new Vector3(2830.598f, 2874.059f, 46.973f), 207.456f),
            new VehicleRaceStartingPosition(4, new Vector3(2829.572f, 2882.541f, 46.538f), 207.456f),
            new VehicleRaceStartingPosition(5, new Vector3(2826.909f, 2881.157f, 46.529f), 207.456f),
            new VehicleRaceStartingPosition(6, new Vector3(2825.883f, 2889.640f, 45.975f), 207.456f),
            new VehicleRaceStartingPosition(7, new Vector3(2823.221f, 2888.256f, 45.939f), 207.456f),
        };
        List<VehicleRaceCheckpoint> minewardspiralrevcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(2859.381f, 2817.899f, 52.93278f)),
            new VehicleRaceCheckpoint(1, new Vector3(2896.5930f, 2789.7310f, 53.8335f)),
            new VehicleRaceCheckpoint(2, new Vector3(2944.2222f, 2713.8491f, 53.4180f)),
            new VehicleRaceCheckpoint(3, new Vector3(3023.2141f, 2733.8689f, 60.6356f)),
            new VehicleRaceCheckpoint(4, new Vector3(3039.5911f, 2812.7000f, 70.0086f)),
            new VehicleRaceCheckpoint(5, new Vector3(3014.1589f, 2879.1411f, 71.7657f)),
            new VehicleRaceCheckpoint(6, new Vector3(3034.9080f, 2967.0039f, 70.6033f)),
            new VehicleRaceCheckpoint(7, new Vector3(2984.3101f, 2945.6201f, 78.5753f)),
            new VehicleRaceCheckpoint(8, new Vector3(2955.9341f, 2911.6580f, 70.9026f)),
            new VehicleRaceCheckpoint(9, new Vector3(3011.7900f, 2945.6609f, 65.9224f)),
            new VehicleRaceCheckpoint(10, new Vector3(2991.9707f, 2884.7349f, 60.2772f)),
            new VehicleRaceCheckpoint(11, new Vector3(3021.0540f, 2807.9880f, 65.2655f)),
            new VehicleRaceCheckpoint(12, new Vector3(3047.1565f, 2767.0901f, 66.9343f)),
            new VehicleRaceCheckpoint(13, new Vector3(3010.5291f, 2715.8569f, 63.2056f)),
            new VehicleRaceCheckpoint(14, new Vector3(2956.1931f, 2682.4861f, 63.3001f)),
            new VehicleRaceCheckpoint(15, new Vector3(2913.5400f, 2730.8879f, 61.8753f)),
            new VehicleRaceCheckpoint(16, new Vector3(2877.8904f, 2780.0374f, 57.3181f)),
            new VehicleRaceCheckpoint(17, new Vector3(2851.9697f, 2827.0210f, 51.5760f)),
            new VehicleRaceCheckpoint(18, new Vector3(2833.6816f, 2871.7195f, 47.2565f)),
            new VehicleRaceCheckpoint(19, new Vector3(2817.6074f, 2901.4016f, 44.9261f)),
            new VehicleRaceCheckpoint(20, new Vector3(2851.1094f, 2865.7749f, 55.7528f)),
            new VehicleRaceCheckpoint(21, new Vector3(2899.2019f, 2852.9939f, 63.2669f)),
            new VehicleRaceCheckpoint(22, new Vector3(2929.9902f, 2878.2051f, 59.7260f)),
            new VehicleRaceCheckpoint(23, new Vector3(2972.6665f, 2855.1680f, 55.8068f)),
            new VehicleRaceCheckpoint(24, new Vector3(3017.3799f, 2777.6450f, 52.6012f)),
            new VehicleRaceCheckpoint(25, new Vector3(2978.9668f, 2728.0071f, 53.3941f)),
            new VehicleRaceCheckpoint(26, new Vector3(2923.7598f, 2744.9326f, 52.6429f)),
            new VehicleRaceCheckpoint(27, new Vector3(2896.7947f, 2785.0933f, 53.6336f)),
            new VehicleRaceCheckpoint(28, new Vector3(2907.5552f, 2819.5347f, 52.9237f)),
            new VehicleRaceCheckpoint(29, new Vector3(2948.2971f, 2840.8083f, 46.0740f)),
            new VehicleRaceCheckpoint(30, new Vector3(2979.9241f, 2810.3779f, 43.1119f)),
        };
        VehicleRaceTrack minewardspiralrev = new VehicleRaceTrack("minewardspiral2", "P2P - Mineward Spiral Reverse", "Race through the Davis Quartz Quarry", minewardspiralrevcheckpoints, minewardspiralrevstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(minewardspiralrev);

        List<VehicleRaceStartingPosition> ratoncanyon1start = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-1545.598f, 4887.471f, 62.039f), 139.666f),
            new VehicleRaceStartingPosition(1, new Vector3(-1542.930f, 4885.206f, 62.017f), 139.666f),
            new VehicleRaceStartingPosition(2, new Vector3(-1539.125f, 4895.094f, 63.132f), 139.666f),
            new VehicleRaceStartingPosition(3, new Vector3(-1536.457f, 4892.829f, 63.188f), 139.666f),
            new VehicleRaceStartingPosition(4, new Vector3(-1532.653f, 4902.717f, 64.436f), 139.666f),
            new VehicleRaceStartingPosition(5, new Vector3(-1529.985f, 4900.452f, 64.543f), 139.666f),
            new VehicleRaceStartingPosition(6, new Vector3(-1526.181f, 4910.340f, 65.404f), 139.666f),
            new VehicleRaceStartingPosition(7, new Vector3(-1523.513f, 4908.075f, 65.421f), 139.666f),
        };
        List<VehicleRaceCheckpoint> ratoncanyon1checkpoints = new List<VehicleRaceCheckpoint>()
        {
            //new VehicleRaceCheckpoint(0, new Vector3(-1498.189f, 4969.542f, 62.872f)),
            //new VehicleRaceCheckpoint(1, new Vector3(-1585.592f, 4753.021f, 50.262f)),
            //new VehicleRaceCheckpoint(2, new Vector3(-1497.267f, 4698.597f, 36.336f)),
            //new VehicleRaceCheckpoint(3, new Vector3(-1338.677f, 4699.587f, 64.533f)),
            //new VehicleRaceCheckpoint(4, new Vector3(-1292.168f, 4649.376f, 102.071f)),
            //new VehicleRaceCheckpoint(5, new Vector3(-1223.851f, 4623.399f, 132.116f)),
            //new VehicleRaceCheckpoint(6, new Vector3(-1154.807f, 4624.247f, 145.273f)),
            //new VehicleRaceCheckpoint(7, new Vector3(-1078.256f, 4598.176f, 122.458f)),
            //new VehicleRaceCheckpoint(8, new Vector3(-992.169f, 4550.842f, 127.924f)),
            //new VehicleRaceCheckpoint(9, new Vector3(-902.313f, 4537.295f, 115.807f)),
            //new VehicleRaceCheckpoint(10, new Vector3(-812.167f, 4534.252f, 92.650f)),
            //new VehicleRaceCheckpoint(11, new Vector3(-731.763f, 4548.635f, 79.476f)),
            //new VehicleRaceCheckpoint(12, new Vector3(-669.754f, 4518.832f, 84.655f)),
            //new VehicleRaceCheckpoint(13, new Vector3(-526.784f, 4505.484f, 78.487f)),
            //new VehicleRaceCheckpoint(14, new Vector3(-421.413f, 4529.701f, 97.341f)),
            //new VehicleRaceCheckpoint(15, new Vector3(-365.924f, 4511.603f, 81.380f)),
            //new VehicleRaceCheckpoint(16, new Vector3(-293.463f, 4455.497f, 57.657f)),
            //new VehicleRaceCheckpoint(17, new Vector3(-251.538f, 4398.027f, 42.030f)),
            //new VehicleRaceCheckpoint(18, new Vector3(-195.920f, 4321.305f, 31.075f)),
            //new VehicleRaceCheckpoint(19, new Vector3(-136.789f, 4293.628f, 40.991f)),
            //new VehicleRaceCheckpoint(20, new Vector3(-164.684f, 4249.083f, 44.315f)),
            //new VehicleRaceCheckpoint(21, new Vector3(-257.911f, 4228.823f, 44.021f)),
            //new VehicleRaceCheckpoint(22, new Vector3(-424.348f, 4294.519f, 58.246f)),
            //new VehicleRaceCheckpoint(23, new Vector3(-569.796f, 4358.000f, 57.749f)),
            //new VehicleRaceCheckpoint(24, new Vector3(-706.422f, 4399.723f, 22.862f)),
            //new VehicleRaceCheckpoint(25, new Vector3(-883.902f, 4386.569f, 18.968f)),
            //new VehicleRaceCheckpoint(26, new Vector3(-1031.980f, 4361.969f, 11.177f)),
            //new VehicleRaceCheckpoint(27, new Vector3(-1170.729f, 4364.292f, 6.929f)),
            //new VehicleRaceCheckpoint(28, new Vector3(-1334.376f, 4324.266f, 6.590f)),
            //new VehicleRaceCheckpoint(29, new Vector3(-1487.669f, 4305.037f, 4.006f)),
            //new VehicleRaceCheckpoint(30, new Vector3(-1630.605f, 4411.642f, 1.880f)),
            //new VehicleRaceCheckpoint(31, new Vector3(-1776.362f, 4472.657f, 9.026f)),
            //new VehicleRaceCheckpoint(32, new Vector3(-1872.066f, 4487.008f, 24.102f)),
            //new VehicleRaceCheckpoint(33, new Vector3(-1969.197f, 4485.495f, 32.573f)),
            //new VehicleRaceCheckpoint(34, new Vector3(-2100.889f, 4511.319f, 28.080f)),
            //new VehicleRaceCheckpoint(35, new Vector3(-2228.452f, 4443.554f, 38.823f)),
            //new VehicleRaceCheckpoint(36, new Vector3(-2257.247f, 4328.040f, 44.077f)),


            new VehicleRaceCheckpoint(0, new Vector3(-1560.498f, 4726.184f, 49.044f)),
            new VehicleRaceCheckpoint(1, new Vector3(-1494.566f, 4698.646f, 36.451f)),
            new VehicleRaceCheckpoint(2, new Vector3(-1307.392f, 4688.354f, 74.541f)),
            new VehicleRaceCheckpoint(3, new Vector3(-1279.872f, 4655.157f, 97.417f)),
            new VehicleRaceCheckpoint(4, new Vector3(-1214.985f, 4630.162f, 134.144f)),
            new VehicleRaceCheckpoint(5, new Vector3(-1043.240f, 4590.004f, 120.342f)),
            new VehicleRaceCheckpoint(6, new Vector3(-862.532f, 4534.273f, 106.429f)),
            new VehicleRaceCheckpoint(7, new Vector3(-704.886f, 4551.459f, 83.262f)),
            new VehicleRaceCheckpoint(8, new Vector3(-579.010f, 4535.941f, 78.723f)),
            new VehicleRaceCheckpoint(9, new Vector3(-496.748f, 4521.550f, 86.178f)),
            new VehicleRaceCheckpoint(10, new Vector3(-384.202f, 4518.240f, 85.998f)),
            new VehicleRaceCheckpoint(11, new Vector3(-204.486f, 4330.212f, 31.297f)),
            new VehicleRaceCheckpoint(12, new Vector3(-128.545f, 4294.745f, 43.159f)),
            new VehicleRaceCheckpoint(13, new Vector3(-164.747f, 4248.466f, 44.400f)),
            new VehicleRaceCheckpoint(14, new Vector3(-253.677f, 4228.718f, 44.174f)),
            new VehicleRaceCheckpoint(15, new Vector3(-452.046f, 4320.366f, 60.508f)),
            new VehicleRaceCheckpoint(16, new Vector3(-613.580f, 4370.220f, 43.667f)),
            new VehicleRaceCheckpoint(17, new Vector3(-776.050f, 4399.063f, 17.728f)),
            new VehicleRaceCheckpoint(18, new Vector3(-883.303f, 4386.445f, 19.071f)),
            new VehicleRaceCheckpoint(19, new Vector3(-1012.041f, 4355.261f, 11.479f)),
            new VehicleRaceCheckpoint(20, new Vector3(-1205.769f, 4366.018f, 6.726f)),
            new VehicleRaceCheckpoint(21, new Vector3(-1445.218f, 4302.614f, 1.848f)),
            new VehicleRaceCheckpoint(22, new Vector3(-1616.751f, 4385.329f, 1.878f)),
            new VehicleRaceCheckpoint(23, new Vector3(-1772.564f, 4471.154f, 8.306f)),
            new VehicleRaceCheckpoint(24, new Vector3(-1898.412f, 4480.509f, 27.667f)),
            new VehicleRaceCheckpoint(25, new Vector3(-1974.769f, 4491.547f, 32.123f)),
            new VehicleRaceCheckpoint(26, new Vector3(-2136.426f, 4512.093f, 29.476f)),
            new VehicleRaceCheckpoint(27, new Vector3(-2226.438f, 4444.603f, 38.880f)),
            new VehicleRaceCheckpoint(28, new Vector3(-2256.553f, 4322.905f, 44.546f)),
        };
        VehicleRaceTrack ratoncanyon1 = new VehicleRaceTrack("raton_canyon", "P2P - Raton Canyon 1", "Hard Route. Will you make it to the end! MotorBikes Recommended!", ratoncanyon1checkpoints, ratoncanyon1start);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(ratoncanyon1);

        List<VehicleRaceCheckpoint> ratoncanyon2checkpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-1572.000f, 4735.500f, 49.594f)),
            new VehicleRaceCheckpoint(1, new Vector3(-1514.250f, 4656.500f, 30.250f)),
            new VehicleRaceCheckpoint(2, new Vector3(-1568.000f, 4547.250f, 16.219f)),
            new VehicleRaceCheckpoint(3, new Vector3(-1483.750f, 4473.000f, 15.969f)),
            new VehicleRaceCheckpoint(4, new Vector3(-1294.500f, 4493.750f, 19.969f)),
            new VehicleRaceCheckpoint(5, new Vector3(-1170.250f, 4458.000f, 20.313f)),
            new VehicleRaceCheckpoint(6, new Vector3(-1030.250f, 4422.500f, 25.094f)),
            new VehicleRaceCheckpoint(7, new Vector3(-882.250f, 4415.500f, 19.875f)),
            new VehicleRaceCheckpoint(8, new Vector3(-937.750f, 4360.000f, 12.313f)),
            new VehicleRaceCheckpoint(9, new Vector3(-1188.250f, 4364.500f, 5.938f)),
            new VehicleRaceCheckpoint(10, new Vector3(-1304.500f, 4340.500f, 5.688f)),
            new VehicleRaceCheckpoint(11, new Vector3(-1440.750f, 4302.500f, 1.563f)),
            new VehicleRaceCheckpoint(12, new Vector3(-1542.000f, 4321.000f, 3.625f)),
            new VehicleRaceCheckpoint(13, new Vector3(-1630.750f, 4412.000f, 1.531f)),
            new VehicleRaceCheckpoint(14, new Vector3(-1780.000f, 4474.500f, 9.406f)),
            new VehicleRaceCheckpoint(15, new Vector3(-1869.500f, 4489.000f, 23.344f)),
            new VehicleRaceCheckpoint(16, new Vector3(-1972.000f, 4488.000f, 31.844f)),
            new VehicleRaceCheckpoint(17, new Vector3(-2204.500f, 4473.750f, 35.344f)),
            new VehicleRaceCheckpoint(18, new Vector3(-2257.250f, 4329.250f, 43.625f)),
        };
        VehicleRaceTrack ratoncanyon2 = new VehicleRaceTrack("raton_canyon2", "P2P - Raton Canyon 2", "Easy Route, Down and over the bridge", ratoncanyon2checkpoints, ratoncanyon1start);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(ratoncanyon2);


        //List<VehicleRaceStartingPosition> ratoncanyon3start = new List<VehicleRaceStartingPosition>()
        //{
        //    new VehicleRaceStartingPosition(0, new Vector3(-234.714f, 4225.494f, 43.961f), 75.241f),
        //    new VehicleRaceStartingPosition(1, new Vector3(-233.822f, 4228.878f, 44.013f), 75.241f),
        //    new VehicleRaceStartingPosition(2, new Vector3(-225.044f, 4222.947f, 43.934f), 75.241f),
        //    new VehicleRaceStartingPosition(3, new Vector3(-224.152f, 4226.331f, 44.038f), 75.241f),
        //    new VehicleRaceStartingPosition(4, new Vector3(-215.374f, 4220.399f, 43.878f), 75.241f),
        //    new VehicleRaceStartingPosition(5, new Vector3(-214.482f, 4223.784f, 43.896f), 75.241f),
        //    new VehicleRaceStartingPosition(6, new Vector3(-205.704f, 4217.852f, 43.844f), 75.241f),
        //    new VehicleRaceStartingPosition(7, new Vector3(-204.812f, 4221.236f, 43.863f), 75.241f),
        //};
        //List<VehicleRaceCheckpoint> ratoncanyon3checkpoints = new List<VehicleRaceCheckpoint>()
        //{
        //    new VehicleRaceCheckpoint(0, new Vector3(-335.500f, 4249.000f, 42.344f)),
        //    new VehicleRaceCheckpoint(1, new Vector3(-447.750f, 4314.500f, 59.969f)),
        //    new VehicleRaceCheckpoint(2, new Vector3(-592.500f, 4364.250f, 49.438f)),
        //    new VehicleRaceCheckpoint(3, new Vector3(-699.250f, 4395.250f, 23.688f)),
        //    new VehicleRaceCheckpoint(4, new Vector3(-798.000f, 4406.000f, 17.656f)),
        //    new VehicleRaceCheckpoint(5, new Vector3(-882.750f, 4386.500f, 18.375f)),
        //    new VehicleRaceCheckpoint(6, new Vector3(-984.750f, 4349.250f, 11.906f)),
        //    new VehicleRaceCheckpoint(7, new Vector3(-1174.000f, 4363.250f, 6.406f)),
        //    new VehicleRaceCheckpoint(8, new Vector3(-1322.750f, 4331.250f, 6.344f)),
        //    new VehicleRaceCheckpoint(9, new Vector3(-1490.500f, 4304.250f, 3.813f)),
        //    new VehicleRaceCheckpoint(10, new Vector3(-1630.750f, 4412.000f, 1.531f)),
        //    new VehicleRaceCheckpoint(11, new Vector3(-1772.250f, 4471.750f, 7.813f)),
        //    new VehicleRaceCheckpoint(12, new Vector3(-1900.000f, 4480.750f, 27.281f)),
        //    new VehicleRaceCheckpoint(13, new Vector3(-1978.000f, 4493.750f, 31.125f)),
        //    new VehicleRaceCheckpoint(14, new Vector3(-2104.750f, 4511.000f, 27.531f)),
        //    new VehicleRaceCheckpoint(15, new Vector3(-2225.750f, 4444.750f, 38.406f)),
        //    new VehicleRaceCheckpoint(16, new Vector3(-2255.500f, 4323.250f, 44.000f)),
        //};
        //VehicleRaceTrack ratoncanyon3 = new VehicleRaceTrack("raton_canyon3", "Raton Canyon 3", "From the Alamo Sea through Raton Canyon to North Chumash", ratoncanyon3checkpoints, ratoncanyon3start);
        //VehicleRaceTypeManager.VehicleRaceTracks.Add(ratoncanyon3);




        List<VehicleRaceStartingPosition> ratontolumbermillstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-1526.239f, 4904.773f, 65.234f), 141.039f),
            new VehicleRaceStartingPosition(1, new Vector3(-1529.349f, 4907.289f, 65.200f), 141.039f),
            new VehicleRaceStartingPosition(2, new Vector3(-1519.951f, 4912.549f, 65.676f), 141.039f),
            new VehicleRaceStartingPosition(3, new Vector3(-1523.061f, 4915.064f, 65.688f), 141.039f),
            new VehicleRaceStartingPosition(4, new Vector3(-1513.663f, 4920.325f, 65.622f), 141.039f),
            new VehicleRaceStartingPosition(5, new Vector3(-1516.773f, 4922.840f, 65.678f), 141.039f),
            new VehicleRaceStartingPosition(6, new Vector3(-1507.375f, 4928.101f, 65.026f), 141.039f),
            new VehicleRaceStartingPosition(7, new Vector3(-1510.485f, 4930.616f, 64.993f), 141.039f),
        };
        List<VehicleRaceCheckpoint> ratontolumbermillcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-1535.250f, 4739.000f, 50.906f)),
            new VehicleRaceCheckpoint(1, new Vector3(-1464.750f, 4804.250f, 83.188f)),
            new VehicleRaceCheckpoint(2, new Vector3(-1350.500f, 4824.250f, 134.906f)),
            new VehicleRaceCheckpoint(3, new Vector3(-1317.500f, 4890.750f, 144.250f)),
            new VehicleRaceCheckpoint(4, new Vector3(-1292.000f, 4939.250f, 151.250f)),
            new VehicleRaceCheckpoint(5, new Vector3(-1238.250f, 5012.750f, 153.969f)),
            new VehicleRaceCheckpoint(6, new Vector3(-1147.500f, 5036.500f, 156.250f)),
            new VehicleRaceCheckpoint(7, new Vector3(-1035.750f, 5094.750f, 150.156f)),
            new VehicleRaceCheckpoint(8, new Vector3(-943.750f, 5194.500f, 119.250f)),
            new VehicleRaceCheckpoint(9, new Vector3(-828.750f, 5177.750f, 111.750f)),
            new VehicleRaceCheckpoint(10, new Vector3(-689.500f, 5151.750f, 116.906f)),
            new VehicleRaceCheckpoint(11, new Vector3(-596.500f, 5033.250f, 138.938f)),
            new VehicleRaceCheckpoint(12, new Vector3(-496.500f, 4929.750f, 146.156f)),
        };
        VehicleRaceTrack ratontolumbermill = new VehicleRaceTrack("ratontolumbermill", "P2P - Raton to Lumbermill", "Race the Trail to the Lumbermill", ratontolumbermillcheckpoints, ratontolumbermillstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(ratontolumbermill);

        List<VehicleRaceStartingPosition> ridgerunstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-522.936f, 2018.363f, 202.631f), 23.394f),
            new VehicleRaceStartingPosition(1, new Vector3(-519.724f, 2019.752f, 202.631f), 23.394f),
            new VehicleRaceStartingPosition(2, new Vector3(-519.760f, 2011.020f, 203.835f), 23.394f),
            new VehicleRaceStartingPosition(3, new Vector3(-516.547f, 2012.410f, 204.227f), 23.394f),
            new VehicleRaceStartingPosition(4, new Vector3(-516.583f, 2003.678f, 204.746f), 23.394f),
            new VehicleRaceStartingPosition(5, new Vector3(-513.371f, 2005.068f, 204.780f), 23.394f),
            new VehicleRaceStartingPosition(6, new Vector3(-513.407f, 1996.335f, 205.433f), 23.394f),
            new VehicleRaceStartingPosition(7, new Vector3(-510.194f, 1997.725f, 205.536f), 23.394f),
        };
        List<VehicleRaceCheckpoint> ridgeruncheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-573.6789f, 2037.1670f, 187.8733f)),
            new VehicleRaceCheckpoint(1, new Vector3(-595.3907f, 1963.8900f, 171.9366f)),
            new VehicleRaceCheckpoint(2, new Vector3(-633.6208f, 2025.6998f, 158.2910f)),
            new VehicleRaceCheckpoint(3, new Vector3(-572.8118f, 2078.2720f, 149.4540f)),
            new VehicleRaceCheckpoint(4, new Vector3(-570.9627f, 2162.0359f, 134.7248f)),
            new VehicleRaceCheckpoint(5, new Vector3(-598.9607f, 2125.7041f, 127.2645f)),
            new VehicleRaceCheckpoint(6, new Vector3(-676.9645f, 2168.9529f, 104.7812f)),
            new VehicleRaceCheckpoint(7, new Vector3(-720.4925f, 2272.2900f, 75.6379f)),
            new VehicleRaceCheckpoint(8, new Vector3(-707.4363f, 2461.5571f, 61.3191f)),
            new VehicleRaceCheckpoint(9, new Vector3(-691.5254f, 2530.9070f, 54.5241f)),
            new VehicleRaceCheckpoint(10, new Vector3(-729.2248f, 2646.3401f, 57.4402f)),
            new VehicleRaceCheckpoint(11, new Vector3(-785.4009f, 2668.7661f, 52.7145f)),
            new VehicleRaceCheckpoint(12, new Vector3(-863.5698f, 2623.9851f, 56.1686f)),
            new VehicleRaceCheckpoint(13, new Vector3(-915.1938f, 2593.0200f, 55.3226f)),
            new VehicleRaceCheckpoint(14, new Vector3(-944.2743f, 2699.0779f, 37.1145f)),
            new VehicleRaceCheckpoint(15, new Vector3(-1040.9342f, 2649.8801f, 35.7990f)),
        };
        VehicleRaceTrack ridgerun = new VehicleRaceTrack("ridge_run", "P2P - Ridge Run", "Race Through the Great Chaparral Trail", ridgeruncheckpoints, ridgerunstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(ridgerun); // R Conversion

        List<VehicleRaceStartingPosition> ridgerunrevstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-1096.683f, 2630.229f, 25.263f), 277.115f),
            new VehicleRaceStartingPosition(1, new Vector3(-1096.312f, 2627.252f, 25.276f), 277.115f),
            new VehicleRaceStartingPosition(2, new Vector3(-1104.622f, 2629.238f, 23.344f), 277.115f),
            new VehicleRaceStartingPosition(3, new Vector3(-1104.250f, 2626.261f, 23.393f), 277.115f),
            new VehicleRaceStartingPosition(4, new Vector3(-1112.560f, 2628.247f, 21.663f), 277.115f),
            new VehicleRaceStartingPosition(5, new Vector3(-1112.188f, 2625.270f, 22.175f), 277.115f),
            new VehicleRaceStartingPosition(6, new Vector3(-1120.498f, 2627.256f, 20.540f), 277.115f),
            new VehicleRaceStartingPosition(7, new Vector3(-1120.127f, 2624.279f, 20.495f), 277.115f),
        };
        List<VehicleRaceCheckpoint> ridgerunrevcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-944.2743f, 2699.0779f, 37.1145f)),
            new VehicleRaceCheckpoint(1, new Vector3(-915.1938f, 2593.0200f, 55.3226f)),
            new VehicleRaceCheckpoint(2, new Vector3(-863.5698f, 2623.9851f, 56.1686f)),
            new VehicleRaceCheckpoint(3, new Vector3(-785.4009f, 2668.7661f, 52.7145f)),
            new VehicleRaceCheckpoint(4, new Vector3(-729.2248f, 2646.3401f, 57.4402f)),
            new VehicleRaceCheckpoint(5, new Vector3(-691.5254f, 2530.9070f, 54.5241f)),
            new VehicleRaceCheckpoint(6, new Vector3(-707.4363f, 2461.5571f, 61.3191f)),
            new VehicleRaceCheckpoint(7, new Vector3(-720.4925f, 2272.2900f, 75.6379f)),
            new VehicleRaceCheckpoint(8, new Vector3(-676.9645f, 2168.9529f, 104.7812f)),
            new VehicleRaceCheckpoint(9, new Vector3(-598.9607f, 2125.7041f, 127.2645f)),
            new VehicleRaceCheckpoint(10, new Vector3(-570.9627f, 2162.0359f, 134.7248f)),
            new VehicleRaceCheckpoint(11, new Vector3(-572.8118f, 2078.2720f, 149.4540f)),
            new VehicleRaceCheckpoint(12, new Vector3(-633.6208f, 2025.6998f, 158.2910f)),
            new VehicleRaceCheckpoint(13, new Vector3(-595.3907f, 1963.8900f, 171.9366f)),
            new VehicleRaceCheckpoint(14, new Vector3(-573.6789f, 2037.1670f, 187.8733f)),
        };
        VehicleRaceTrack ridgerunrev = new VehicleRaceTrack("ridge_run2", "P2P - Ridge Run Reverse", "Race Through the Great Chaparral Trail", ridgerunrevcheckpoints, ridgerunrevstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(ridgerunrev);

        List<VehicleRaceStartingPosition> ronswindfarm1start = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(2481.386f, 2094.213f, 31.147f), 355.337f),
            new VehicleRaceStartingPosition(1, new Vector3(2477.399f, 2094.538f, 31.202f), 355.337f),
            new VehicleRaceStartingPosition(2, new Vector3(2480.573f, 2084.246f, 30.428f), 355.337f),
            new VehicleRaceStartingPosition(3, new Vector3(2476.586f, 2084.572f, 30.475f), 355.337f),
            new VehicleRaceStartingPosition(4, new Vector3(2479.760f, 2074.279f, 29.744f), 355.337f),
            new VehicleRaceStartingPosition(5, new Vector3(2475.773f, 2074.604f, 29.789f), 355.337f),
            new VehicleRaceStartingPosition(6, new Vector3(2478.947f, 2064.313f, 29.083f), 355.337f),
            new VehicleRaceStartingPosition(7, new Vector3(2474.960f, 2064.638f, 29.030f), 355.337f),
        };
        List<VehicleRaceCheckpoint> ronswindfarm1checkpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(2451.765f, 2220.128f, 43.971f)),
            new VehicleRaceCheckpoint(1, new Vector3(2369.982f, 2412.068f, 61.351f)),
            new VehicleRaceCheckpoint(2, new Vector3(2320.717f, 2379.631f, 68.351f)),
            new VehicleRaceCheckpoint(3, new Vector3(2300.604f, 2240.692f, 75.952f)),
            new VehicleRaceCheckpoint(4, new Vector3(2247.855f, 2236.679f, 80.206f)),
            new VehicleRaceCheckpoint(5, new Vector3(2214.563f, 2408.982f, 80.775f)),
            new VehicleRaceCheckpoint(6, new Vector3(2141.206f, 2445.855f, 88.476f)),
            new VehicleRaceCheckpoint(7, new Vector3(2032.399f, 2386.927f, 86.276f)),
            new VehicleRaceCheckpoint(8, new Vector3(2032.828f, 2346.905f, 93.718f)),
            new VehicleRaceCheckpoint(9, new Vector3(2122.227f, 2409.989f, 100.126f)),
            new VehicleRaceCheckpoint(10, new Vector3(2169.426f, 2282.029f, 103.555f)),
            new VehicleRaceCheckpoint(11, new Vector3(2172.403f, 2175.885f, 116.186f)),
            new VehicleRaceCheckpoint(12, new Vector3(2204.937f, 2090.243f, 129.027f)),
            new VehicleRaceCheckpoint(13, new Vector3(2270.647f, 2021.904f, 130.316f)),
            new VehicleRaceCheckpoint(14, new Vector3(2266.813f, 1893.799f, 119.748f)),
            new VehicleRaceCheckpoint(15, new Vector3(2179.412f, 1799.173f, 106.577f)),
            new VehicleRaceCheckpoint(16, new Vector3(2096.994f, 1737.813f, 102.693f)),
            new VehicleRaceCheckpoint(17, new Vector3(2120.613f, 1662.973f, 95.682f)),
            new VehicleRaceCheckpoint(18, new Vector3(2186.861f, 1720.572f, 91.563f)),
            new VehicleRaceCheckpoint(19, new Vector3(2183.870f, 1664.134f, 84.989f)),
            new VehicleRaceCheckpoint(20, new Vector3(2181.694f, 1488.116f, 82.419f)),
            new VehicleRaceCheckpoint(21, new Vector3(2222.542f, 1463.313f, 80.332f)),
            new VehicleRaceCheckpoint(22, new Vector3(2221.770f, 1603.994f, 75.146f)),
            new VehicleRaceCheckpoint(23, new Vector3(2263.293f, 1626.050f, 68.851f)),
            new VehicleRaceCheckpoint(24, new Vector3(2263.924f, 1422.923f, 74.758f)),
            new VehicleRaceCheckpoint(25, new Vector3(2302.334f, 1368.844f, 67.674f)),
            new VehicleRaceCheckpoint(26, new Vector3(2299.943f, 1614.990f, 57.413f)),
            new VehicleRaceCheckpoint(27, new Vector3(2343.113f, 1627.075f, 50.617f)),
            new VehicleRaceCheckpoint(28, new Vector3(2341.982f, 1364.592f, 60.444f)),
            new VehicleRaceCheckpoint(29, new Vector3(2385.094f, 1338.767f, 57.366f)),
            new VehicleRaceCheckpoint(30, new Vector3(2384.438f, 1465.508f, 41.528f)),
        };
        VehicleRaceTrack ronswindfarm1 = new VehicleRaceTrack("ronswindfarm1", "P2P - Ron Windfarm Point 2 Point", "Race through RON Alternates Wind Farm", ronswindfarm1checkpoints, ronswindfarm1start);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(ronswindfarm1);

        List<VehicleRaceStartingPosition> ronswindfarm2start = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(2382.632f, 1440.826f, 44.545f), 179.949f),
            new VehicleRaceStartingPosition(1, new Vector3(2386.632f, 1440.822f, 44.548f), 179.949f),
            new VehicleRaceStartingPosition(2, new Vector3(2382.641f, 1450.826f, 43.158f), 179.949f),
            new VehicleRaceStartingPosition(3, new Vector3(2386.641f, 1450.822f, 43.173f), 179.949f),
            new VehicleRaceStartingPosition(4, new Vector3(2382.650f, 1460.826f, 41.756f), 179.949f),
            new VehicleRaceStartingPosition(5, new Vector3(2386.650f, 1460.822f, 41.781f), 179.949f),
            new VehicleRaceStartingPosition(6, new Vector3(2382.659f, 1470.826f, 40.503f), 179.949f),
            new VehicleRaceStartingPosition(7, new Vector3(2386.659f, 1470.822f, 40.468f), 179.949f),
        };
        List<VehicleRaceCheckpoint> ronswindfarm2checkpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(2384.620f, 1331.421f, 58.200f)),
            new VehicleRaceCheckpoint(1, new Vector3(2340.309f, 1322.680f, 63.355f)),
            new VehicleRaceCheckpoint(2, new Vector3(2341.474f, 1598.400f, 52.442f)),
            new VehicleRaceCheckpoint(3, new Vector3(2299.618f, 1614.100f, 57.648f)),
            new VehicleRaceCheckpoint(4, new Vector3(2302.704f, 1387.211f, 66.822f)),
            new VehicleRaceCheckpoint(5, new Vector3(2263.204f, 1386.453f, 76.138f)),
            new VehicleRaceCheckpoint(6, new Vector3(2263.788f, 1605.388f, 67.861f)),
            new VehicleRaceCheckpoint(7, new Vector3(2221.903f, 1635.595f, 75.553f)),
            new VehicleRaceCheckpoint(8, new Vector3(2222.602f, 1466.164f, 80.352f)),
            new VehicleRaceCheckpoint(9, new Vector3(2182.519f, 1483.830f, 82.518f)),
            new VehicleRaceCheckpoint(10, new Vector3(2184.283f, 1643.605f, 84.175f)),
            new VehicleRaceCheckpoint(11, new Vector3(2191.719f, 1725.581f, 91.198f)),
            new VehicleRaceCheckpoint(12, new Vector3(2079.986f, 1718.407f, 102.627f)),
            new VehicleRaceCheckpoint(13, new Vector3(2262.957f, 1885.707f, 118.160f)),
            new VehicleRaceCheckpoint(14, new Vector3(2272.802f, 2017.725f, 130.978f)),
            new VehicleRaceCheckpoint(15, new Vector3(2205.685f, 2093.016f, 128.828f)),
            new VehicleRaceCheckpoint(16, new Vector3(2174.962f, 2179.595f, 116.107f)),
            new VehicleRaceCheckpoint(17, new Vector3(2161.203f, 2298.706f, 103.248f)),
            new VehicleRaceCheckpoint(18, new Vector3(2063.913f, 2369.109f, 96.626f)),
            new VehicleRaceCheckpoint(19, new Vector3(2017.962f, 2370.277f, 87.828f)),
            new VehicleRaceCheckpoint(20, new Vector3(2133.036f, 2446.675f, 88.453f)),
            new VehicleRaceCheckpoint(21, new Vector3(2221.964f, 2391.912f, 77.755f)),
            new VehicleRaceCheckpoint(22, new Vector3(2245.774f, 2241.768f, 80.454f)),
            new VehicleRaceCheckpoint(23, new Vector3(2302.118f, 2250.768f, 75.391f)),
            new VehicleRaceCheckpoint(24, new Vector3(2365.319f, 2414.955f, 61.581f)),
            new VehicleRaceCheckpoint(25, new Vector3(2435.143f, 2279.662f, 52.433f)),
            new VehicleRaceCheckpoint(26, new Vector3(2477.849f, 2091.855f, 31.604f)),
        };
        VehicleRaceTrack ronswindfarm2 = new VehicleRaceTrack("ronswindfarm2", "P2P - Ron Windfarm Point 2 Point Reverse", "Race through RON Alternates Wind Farm", ronswindfarm2checkpoints, ronswindfarm2start);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(ronswindfarm2);

        List<VehicleRaceStartingPosition> valleytrailstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-225.824f, 4223.352f, 43.961f), 75.230f),
            new VehicleRaceStartingPosition(1, new Vector3(-224.931f, 4226.736f, 44.093f), 75.230f),
            new VehicleRaceStartingPosition(2, new Vector3(-218.088f, 4221.312f, 43.910f), 75.230f),
            new VehicleRaceStartingPosition(3, new Vector3(-217.196f, 4224.696f, 43.964f), 75.230f),
            new VehicleRaceStartingPosition(4, new Vector3(-210.352f, 4219.272f, 43.825f), 75.230f),
            new VehicleRaceStartingPosition(5, new Vector3(-209.460f, 4222.657f, 43.859f), 75.230f),
            new VehicleRaceStartingPosition(6, new Vector3(-202.617f, 4217.233f, 43.827f), 75.230f),
            new VehicleRaceStartingPosition(7, new Vector3(-201.724f, 4220.617f, 43.849f), 75.230f),
        };
        List<VehicleRaceCheckpoint> valleytrailcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-269.5131f, 4227.9741f, 43.1428f)),
            new VehicleRaceCheckpoint(1, new Vector3(-330.7353f, 4242.2539f, 42.3877f)),
            new VehicleRaceCheckpoint(2, new Vector3(-417.2221f, 4290.2358f, 56.6259f)),
            new VehicleRaceCheckpoint(3, new Vector3(-506.6945f, 4359.4482f, 66.3928f)),
            new VehicleRaceCheckpoint(4, new Vector3(-566.9642f, 4357.0962f, 58.1420f)),
            new VehicleRaceCheckpoint(5, new Vector3(-733.3773f, 4412.3481f, 20.3150f)),
            new VehicleRaceCheckpoint(6, new Vector3(-825.9259f, 4411.5889f, 19.3628f)),
            new VehicleRaceCheckpoint(7, new Vector3(-898.7642f, 4377.7212f, 16.3963f)),
            new VehicleRaceCheckpoint(8, new Vector3(-974.6707f, 4349.1821f, 11.7338f)),
            new VehicleRaceCheckpoint(9, new Vector3(-1107.4510f, 4379.5420f, 11.8522f)),
            new VehicleRaceCheckpoint(10, new Vector3(-1222.2150f, 4364.5879f, 7.0459f)),
            new VehicleRaceCheckpoint(11, new Vector3(-1303.5140f, 4340.4722f, 5.7083f)),
            new VehicleRaceCheckpoint(12, new Vector3(-1365.6801f, 4298.6089f, 1.4209f)),
            new VehicleRaceCheckpoint(13, new Vector3(-1453.2111f, 4302.7642f, 1.4281f)),
            new VehicleRaceCheckpoint(14, new Vector3(-1509.8950f, 4308.5171f, 4.6830f)),
            new VehicleRaceCheckpoint(15, new Vector3(-1593.9070f, 4349.8140f, 1.8808f)),
            new VehicleRaceCheckpoint(16, new Vector3(-1657.4840f, 4445.5562f, 1.6662f)),
            new VehicleRaceCheckpoint(17, new Vector3(-1756.2560f, 4463.1221f, 4.7861f)),
            new VehicleRaceCheckpoint(18, new Vector3(-1813.3616f, 4479.8638f, 16.5623f)),
            new VehicleRaceCheckpoint(19, new Vector3(-1846.5181f, 4500.3193f, 21.1740f)),
            new VehicleRaceCheckpoint(20, new Vector3(-1879.9010f, 4482.6572f, 25.0771f)),
            new VehicleRaceCheckpoint(21, new Vector3(-1925.7230f, 4468.9521f, 31.4250f)),
        };
        VehicleRaceTrack valleytrail = new VehicleRaceTrack("valley_trail", "P2P - Valley Trail", "Race Along Cassidy Creek", valleytrailcheckpoints, valleytrailstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(valleytrail); // R Conversion

        List<VehicleRaceStartingPosition> valleytrailrevstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-1891.506f, 4430.490f, 43.988f), 64.394f),
            new VehicleRaceStartingPosition(1, new Vector3(-1892.802f, 4427.785f, 43.993f), 64.394f),
            new VehicleRaceStartingPosition(2, new Vector3(-1884.291f, 4427.033f, 45.391f), 64.394f),
            new VehicleRaceStartingPosition(3, new Vector3(-1885.588f, 4424.328f, 45.200f), 64.394f),
            new VehicleRaceStartingPosition(4, new Vector3(-1877.077f, 4423.575f, 46.294f), 64.394f),
            new VehicleRaceStartingPosition(5, new Vector3(-1878.374f, 4420.870f, 46.060f), 64.394f),
            new VehicleRaceStartingPosition(6, new Vector3(-1869.863f, 4420.118f, 47.130f), 64.394f),
            new VehicleRaceStartingPosition(7, new Vector3(-1871.159f, 4417.413f, 47.106f), 64.394f),
        };
        List<VehicleRaceCheckpoint> valleytrailrevcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-1879.9010f, 4482.6572f, 25.0771f)),
            new VehicleRaceCheckpoint(1, new Vector3(-1846.5181f, 4500.3193f, 21.1740f)),
            new VehicleRaceCheckpoint(2, new Vector3(-1813.3616f, 4479.8638f, 16.5623f)),
            new VehicleRaceCheckpoint(3, new Vector3(-1756.2560f, 4463.1221f, 4.7861f)),
            new VehicleRaceCheckpoint(4, new Vector3(-1657.4840f, 4445.5562f, 1.6662f)),
            new VehicleRaceCheckpoint(5, new Vector3(-1593.9070f, 4349.8140f, 1.8808f)),
            new VehicleRaceCheckpoint(6, new Vector3(-1509.8950f, 4308.5171f, 4.6830f)),
            new VehicleRaceCheckpoint(7, new Vector3(-1453.2111f, 4302.7642f, 1.4281f)),
            new VehicleRaceCheckpoint(8, new Vector3(-1365.6801f, 4298.6089f, 1.4209f)),
            new VehicleRaceCheckpoint(9, new Vector3(-1303.5140f, 4340.4722f, 5.7083f)),
            new VehicleRaceCheckpoint(10, new Vector3(-1222.2150f, 4364.5879f, 7.0459f)),
            new VehicleRaceCheckpoint(11, new Vector3(-1107.4510f, 4379.5420f, 11.8522f)),
            new VehicleRaceCheckpoint(12, new Vector3(-974.6707f, 4349.1821f, 11.7338f)),
            new VehicleRaceCheckpoint(13, new Vector3(-898.7642f, 4377.7212f, 16.3963f)),
            new VehicleRaceCheckpoint(14, new Vector3(-825.9259f, 4411.5889f, 19.3628f)),
            new VehicleRaceCheckpoint(15, new Vector3(-733.3773f, 4412.3481f, 20.3150f)),
            new VehicleRaceCheckpoint(16, new Vector3(-566.9642f, 4357.0962f, 58.1420f)),
            new VehicleRaceCheckpoint(17, new Vector3(-506.6945f, 4359.4482f, 66.3928f)),
            new VehicleRaceCheckpoint(18, new Vector3(-417.2221f, 4290.2358f, 56.6259f)),
            new VehicleRaceCheckpoint(19, new Vector3(-330.7353f, 4242.2539f, 42.3877f)),
            new VehicleRaceCheckpoint(20, new Vector3(-269.5131f, 4227.9741f, 43.1428f)),
        };
        VehicleRaceTrack valleytrailrev = new VehicleRaceTrack("valley_trail2", "P2P - Valley Trail Reverse", "Race Along Cassidy Creek", valleytrailrevcheckpoints, valleytrailrevstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(valleytrailrev); 

        List<VehicleRaceStartingPosition> zancudotrailstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-2542.066f, 3593.425f, 10.937f), 162.277f),
            new VehicleRaceStartingPosition(1, new Vector3(-2545.400f, 3594.490f, 10.923f), 162.277f),
            new VehicleRaceStartingPosition(2, new Vector3(-2539.022f, 3602.950f, 11.045f), 162.277f),
            new VehicleRaceStartingPosition(3, new Vector3(-2542.356f, 3604.016f, 11.043f), 162.277f),
            new VehicleRaceStartingPosition(4, new Vector3(-2535.978f, 3612.476f, 11.200f), 162.277f),
            new VehicleRaceStartingPosition(5, new Vector3(-2539.312f, 3613.541f, 11.196f), 162.277f),
            new VehicleRaceStartingPosition(6, new Vector3(-2532.934f, 3622.001f, 11.350f), 162.277f),
            new VehicleRaceStartingPosition(7, new Vector3(-2536.267f, 3623.066f, 11.376f), 162.277f),
        };
        List<VehicleRaceCheckpoint> zancudotrailcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-2755.298f, 3494.014f, 10.196f)),
            new VehicleRaceCheckpoint(1, new Vector3(-3005.805f, 3419.319f, 9.305f)),
            new VehicleRaceCheckpoint(2, new Vector3(-2821.654f, 3125.532f, 9.006f)),
            new VehicleRaceCheckpoint(3, new Vector3(-2657.333f, 2987.691f, 8.374f)),
            new VehicleRaceCheckpoint(4, new Vector3(-2449.570f, 2835.242f, 2.844f)),
            new VehicleRaceCheckpoint(5, new Vector3(-2209.234f, 2790.107f, 3.944f)),
            new VehicleRaceCheckpoint(6, new Vector3(-1982.470f, 2713.190f, 2.889f)),
            new VehicleRaceCheckpoint(7, new Vector3(-1731.217f, 2747.517f, 4.858f)),
            new VehicleRaceCheckpoint(8, new Vector3(-1551.576f, 2710.070f, 3.823f)),
            new VehicleRaceCheckpoint(9, new Vector3(-1374.735f, 2700.872f, 4.404f)),
            new VehicleRaceCheckpoint(10, new Vector3(-1162.568f, 2833.314f, 14.492f)),
            new VehicleRaceCheckpoint(11, new Vector3(-980.281f, 2892.511f, 13.965f)),
            new VehicleRaceCheckpoint(12, new Vector3(-753.396f, 2936.688f, 25.232f)),
            new VehicleRaceCheckpoint(13, new Vector3(-592.009f, 3019.042f, 24.214f)),
            new VehicleRaceCheckpoint(14, new Vector3(-453.953f, 2971.124f, 24.230f)),
        };
        VehicleRaceTrack zancudotrail = new VehicleRaceTrack("zancudotrailshort", "P2P - Zancudo River Trail(Short)", "Enjoy a scenic route along Zancudo River", zancudotrailcheckpoints, zancudotrailstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(zancudotrail);

        List<VehicleRaceStartingPosition> zancudorivertrailstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-2545.106f, 3577.170f, 10.726f), 162.910f),
            new VehicleRaceStartingPosition(1, new Vector3(-2548.452f, 3578.198f, 10.741f), 162.910f),
            new VehicleRaceStartingPosition(2, new Vector3(-2542.168f, 3586.728f, 10.801f), 162.910f),
            new VehicleRaceStartingPosition(3, new Vector3(-2545.513f, 3587.757f, 10.860f), 162.910f),
            new VehicleRaceStartingPosition(4, new Vector3(-2539.229f, 3596.287f, 10.932f), 162.910f),
            new VehicleRaceStartingPosition(5, new Vector3(-2542.574f, 3597.315f, 10.986f), 162.910f),
            new VehicleRaceStartingPosition(6, new Vector3(-2536.290f, 3605.845f, 11.034f), 162.910f),
            new VehicleRaceStartingPosition(7, new Vector3(-2539.636f, 3606.874f, 11.124f), 162.910f),
        };
        List<VehicleRaceCheckpoint> zancudorivertrailcheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-2725.704f, 3484.048f, 11.441f)),
            new VehicleRaceCheckpoint(1, new Vector3(-3004.497f, 3424.067f, 9.178f)),
            new VehicleRaceCheckpoint(2, new Vector3(-2852.428f, 3150.948f, 9.840f)),
            new VehicleRaceCheckpoint(3, new Vector3(-2555.447f, 2865.150f, 2.433f)),
            new VehicleRaceCheckpoint(4, new Vector3(-2364.563f, 2843.066f, 3.014f)),
            new VehicleRaceCheckpoint(5, new Vector3(-2126.191f, 2713.751f, 2.800f)),
            new VehicleRaceCheckpoint(6, new Vector3(-1856.863f, 2690.851f, 3.452f)),
            new VehicleRaceCheckpoint(7, new Vector3(-1566.246f, 2718.901f, 4.435f)),
            new VehicleRaceCheckpoint(8, new Vector3(-1368.278f, 2702.273f, 4.623f)),
            new VehicleRaceCheckpoint(9, new Vector3(-1128.444f, 2852.524f, 14.156f)),
            new VehicleRaceCheckpoint(10, new Vector3(-753.760f, 2936.659f, 25.215f)),
            new VehicleRaceCheckpoint(11, new Vector3(-549.387f, 3008.625f, 25.793f)),
            new VehicleRaceCheckpoint(12, new Vector3(-430.518f, 3049.415f, 28.209f)),
            new VehicleRaceCheckpoint(13, new Vector3(-235.814f, 3117.676f, 35.693f)),
            new VehicleRaceCheckpoint(14, new Vector3(-2.476f, 3233.748f, 42.529f)),
            new VehicleRaceCheckpoint(15, new Vector3(102.623f, 3374.698f, 34.285f)),
        };
        VehicleRaceTrack zancudorivertrail = new VehicleRaceTrack("zancudorivertrail1", "P2P - Zancudo River Trail", "Enjoy a scenic route along Zancudo River", zancudorivertrailcheckpoints, zancudorivertrailstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(zancudorivertrail);


    }

    private void WaterTracks() // Testing out water tracks
    {
        List<VehicleRaceStartingPosition> watersportsstart = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(1890.304f, 4202.177f, 30.423f), 97.472f),
            new VehicleRaceStartingPosition(1, new Vector3(1889.524f, 4208.126f, 30.526f), 97.472f),
            new VehicleRaceStartingPosition(2, new Vector3(1902.202f, 4203.738f, 29.892f), 97.472f),
            new VehicleRaceStartingPosition(3, new Vector3(1901.422f, 4209.687f, 29.902f), 97.472f),
            new VehicleRaceStartingPosition(4, new Vector3(1914.100f, 4205.298f, 30.088f), 97.472f),
            new VehicleRaceStartingPosition(5, new Vector3(1913.320f, 4211.247f, 30.294f), 97.472f),
            new VehicleRaceStartingPosition(6, new Vector3(1925.998f, 4206.859f, 29.606f), 97.472f),
            new VehicleRaceStartingPosition(7, new Vector3(1925.218f, 4212.808f, 29.855f), 97.472f),
        };

        List<VehicleRaceCheckpoint> watersportscheckpoints = new List<VehicleRaceCheckpoint>()
        {
            //new VehicleRaceCheckpoint(0, new Vector3(1780.75f, 4190.5f, 30f)),
            //new VehicleRaceCheckpoint(1, new Vector3(1589.25f, 4125.25f, 30f)),
            //new VehicleRaceCheckpoint(2, new Vector3(1312f, 4038.5f, 30f)),
            //new VehicleRaceCheckpoint(3, new Vector3(1054.75f, 3974.75f, 30f)),
            //new VehicleRaceCheckpoint(4, new Vector3(883.25f, 3932.25f, 30f)),
            //new VehicleRaceCheckpoint(5, new Vector3(711.5f, 3889.75f, 30f)),
            //new VehicleRaceCheckpoint(6, new Vector3(575.5f, 3940f, 30f)),
            //new VehicleRaceCheckpoint(7, new Vector3(484.75f, 3973.75f, 30f)),
            //new VehicleRaceCheckpoint(8, new Vector3(348.75f, 4024.25f, 30f)),
            //new VehicleRaceCheckpoint(9, new Vector3(167.25f, 4091.5f, 30f)),
            //new VehicleRaceCheckpoint(10, new Vector3(76.75f, 4125f, 30f)),
            new VehicleRaceCheckpoint(0, new Vector3(-54.5f, 4178.75f, 30f)),
        };

        VehicleRaceTrack watersports = new VehicleRaceTrack("water_sports2", "Alamo Sea Race2", "Alamo Sea Race", watersportscheckpoints, watersportsstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(watersports);

        List<VehicleRaceCheckpoint> watersports2checkpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(1780.75f, 4190.5f, 30f)),
            new VehicleRaceCheckpoint(1, new Vector3(1589.25f, 4125.25f, 30f)),
            new VehicleRaceCheckpoint(2, new Vector3(1312f, 4038.5f, 30f)),
            new VehicleRaceCheckpoint(3, new Vector3(1054.75f, 3974.75f, 30f)),
            new VehicleRaceCheckpoint(4, new Vector3(883.25f, 3932.25f, 30f)),
            new VehicleRaceCheckpoint(5, new Vector3(711.5f, 3889.75f, 30f)),
            new VehicleRaceCheckpoint(6, new Vector3(575.5f, 3940f, 30f)),
            new VehicleRaceCheckpoint(7, new Vector3(484.75f, 3973.75f, 30f)),
            new VehicleRaceCheckpoint(8, new Vector3(348.75f, 4024.25f, 30f)),
            new VehicleRaceCheckpoint(9, new Vector3(167.25f, 4091.5f, 30f)),
            new VehicleRaceCheckpoint(10, new Vector3(76.75f, 4125f, 30f)),

        };

        VehicleRaceTrack watersports2 = new VehicleRaceTrack("water_sports", "Alamo Sea Race", "Alamo Sea Race", watersports2checkpoints, watersportsstart);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(watersports2);
    }
}

