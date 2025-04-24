using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class VehicleRaces : IVehicleRaces
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\VehicleRaces.xml";
    public VehicleRaceTypeManager VehicleRaceTypeManager { get; private set; }
    public void ReadConfig(string configName)
    {
        string fileName = string.IsNullOrEmpty(configName) ? "VehicleRaces_*.xml" : $"VehicleRaces_{configName}.xml";

        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED"); 
        FileInfo ConfigFile = LSRDirectory.GetFiles(fileName).OrderByDescending(x => x.Name).FirstOrDefault();
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
            DefaultConfig_Liberty();
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

    private void DefaultConfig()
    {
        VehicleRaceTypeManager = new VehicleRaceTypeManager();
        VehicleRaceTypeManager.VehicleRaceTracks = new List<VehicleRaceTrack>();

        SandyTracks();
        VineWoodTracks();
        PaletoTracks();
        CentralTracks();

        Serialization.SerializeParam(VehicleRaceTypeManager, ConfigFileName);
    }
    private void SandyTracks()
    {
        List<VehicleRaceStartingPosition> vehicleRaceStartingPositions = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(1868.027f, 3226.604f, 44.5677f), 39.27186f),
            new VehicleRaceStartingPosition(1, new Vector3(1876.75f, 3215.834f, 44.83164f), 39.09522f),
            new VehicleRaceStartingPosition(2, new Vector3(1866.469f, 3220.694f, 44.6372f), 43.98711f),
            new VehicleRaceStartingPosition(3, new Vector3(1874.058f, 3211.186f, 44.86393f), 38.87733f),
        };
        List<VehicleRaceCheckpoint> vehicleRaceCheckpoints = new List<VehicleRaceCheckpoint>()
        {
            //new VehicleRaceCheckpoint(0, new Vector3(1775.803f, 3375.738f, 39.1007f)),
            //new VehicleRaceCheckpoint(1, new Vector3(1583.795f, 3483.779f, 36.04169f)),
            //new VehicleRaceCheckpoint(2, new Vector3(951.4559f, 3536.375f, 33.44981f)),
            //new VehicleRaceCheckpoint(3,new Vector3(927.9109f, 3630.1f, 31.85251f)),
            //new VehicleRaceCheckpoint(4,new Vector3(1309.275f, 3653.17f, 32.52422f)),
            //new VehicleRaceCheckpoint(5,new Vector3(1663.282f, 3858.711f, 34.23738f)),
            //new VehicleRaceCheckpoint(6,new Vector3(1743.253f, 3758.429f, 33.24446f)),
            //new VehicleRaceCheckpoint(7,new Vector3(1968.987f, 3878.23f, 31.663f)),





            new VehicleRaceCheckpoint(0, new Vector3(1775.803f, 3375.738f, 39.1007f)),
            new VehicleRaceCheckpoint(1,new Vector3(1582.85f, 3480.25f, 36.21559f)),
            new VehicleRaceCheckpoint(2,new Vector3(984.0595f, 3535.763f, 33.54565f)),
            new VehicleRaceCheckpoint(3,new Vector3(956.1648f, 3634.594f, 32.08199f)),
            new VehicleRaceCheckpoint(4,new Vector3(1308.131f, 3652.568f, 32.76283f)),
            new VehicleRaceCheckpoint(5,new Vector3(1628.509f, 3824.556f, 34.63056f)),
            new VehicleRaceCheckpoint(6,new Vector3(1740.17f, 3764.768f, 33.48697f)),
            new VehicleRaceCheckpoint(7,new Vector3(1975.885f, 3888.242f, 31.93978f)),


            //new VehicleRaceCheckpoint(0, new Vector3(1775.803f, 3375.738f, 39.1007f)),
            //new VehicleRaceCheckpoint(1,new Vector3(1702.318f, 3499.852f, 35.91494f)),
            //new VehicleRaceCheckpoint(2,new Vector3(935.6216f, 3535.946f, 33.43148f)),
            //new VehicleRaceCheckpoint(3,new Vector3(931.0949f, 3626.907f, 31.86987f)),
            //new VehicleRaceCheckpoint(4,new Vector3(1540.138f, 3751.758f, 33.91103f)),
            //new VehicleRaceCheckpoint(5,new Vector3(1603.251f, 3673.23f, 33.89013f)),
            //new VehicleRaceCheckpoint(6,new Vector3(1979.965f, 3889.917f, 31.88055f)),

        };
        VehicleRaceTrack sandyDebug = new VehicleRaceTrack("sandyloop1", "Sandy Shores Loop", "Simple loop around Sandy Shores", vehicleRaceCheckpoints, vehicleRaceStartingPositions);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(sandyDebug);

        List<VehicleRaceCheckpoint> vehicleRaceCheckpoints7 = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(1775.803f, 3375.738f, 39.1007f)),
            new VehicleRaceCheckpoint(1,new Vector3(1582.85f, 3480.25f, 36.21559f)),
            new VehicleRaceCheckpoint(2,new Vector3(984.0595f, 3535.763f, 33.54565f)),
            new VehicleRaceCheckpoint(3,new Vector3(1979.965f, 3889.917f, 31.88055f)),
        };
        VehicleRaceTrack sandyDebug2 = new VehicleRaceTrack("sandyloop2", "Sandy Shores Small Loop", "Smaller loop around Sandy Shores", vehicleRaceCheckpoints7, vehicleRaceStartingPositions);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(sandyDebug2);
    }
    private void VineWoodTracks()
    {
        List<VehicleRaceStartingPosition> vehicleRaceStartingPositions2 = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0,new Vector3(594.9255f, 237.1765f, 102.4954f), 69.08444f),
            new VehicleRaceStartingPosition(1,new Vector3(593.8934f, 231.983f, 102.473f), 76.16933f),
            new VehicleRaceStartingPosition(2,new Vector3(603.9206f, 229.4215f, 101.9056f), 75.98711f),
            new VehicleRaceStartingPosition(3,new Vector3(601.6921f, 235.5364f, 102.1932f), 76.84195f),
        };
        List<VehicleRaceCheckpoint> vehicleRaceCheckpoints2 = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0,new Vector3(423.3498f, 295.5466f, 102.4895f)),
            new VehicleRaceCheckpoint(1,new Vector3(283.6923f, -73.34487f, 69.56183f)),
            new VehicleRaceCheckpoint(2,new Vector3(-38.75998f, 32.98601f, 71.58444f)),
            new VehicleRaceCheckpoint(3,new Vector3(-82.37482f, -110.0097f, 57.30961f)),
            new VehicleRaceCheckpoint(4,new Vector3(-111.9359f, -220.4267f, 44.26889f)),
            new VehicleRaceCheckpoint(5,new Vector3(-280.4997f, -172.5392f, 39.44448f)),
            new VehicleRaceCheckpoint(6,new Vector3(-343.272f, -191.9327f, 37.79195f)),
            new VehicleRaceCheckpoint(7,new Vector3(-418.2509f, -72.76836f, 42.19181f)),
            new VehicleRaceCheckpoint(8,new Vector3(-391.8105f, 122.2213f, 65.06055f)),
            new VehicleRaceCheckpoint(9,new Vector3(-643.3143f, 130.383f, 56.60829f)),
            new VehicleRaceCheckpoint(10,new Vector3(-996.1489f, 71.36115f, 51.30039f)),
        };
        VehicleRaceTrack vinewoodDebug = new VehicleRaceTrack("vinewoodRace1", "Vinewood Twisted Loop", "Long race through Vinewood to the coast", vehicleRaceCheckpoints2, vehicleRaceStartingPositions2);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(vinewoodDebug);

        List<VehicleRaceStartingPosition> vehicleRaceStartingPositions3 = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0,new Vector3(594.9255f, 237.1765f, 102.4954f), 69.08444f),
            new VehicleRaceStartingPosition(1,new Vector3(593.8934f, 231.983f, 102.473f), 76.16933f),
            new VehicleRaceStartingPosition(2,new Vector3(603.9206f, 229.4215f, 101.9056f), 75.98711f),
            new VehicleRaceStartingPosition(3,new Vector3(601.6921f, 235.5364f, 102.1932f), 76.84195f),
        };
        List<VehicleRaceCheckpoint> vehicleRaceCheckpoints3 = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0,new Vector3(31.48836f, 256.0826f, 109.0203f)),
            new VehicleRaceCheckpoint(1,new Vector3(-2172.701f, -344.3154f, 12.60608f)),

        };
        VehicleRaceTrack vinewoodLONGDebug = new VehicleRaceTrack("vinewoodRace2", "Vinewood Cross Town", "Long race through Vinewood to the coast without many checkpoints.", vehicleRaceCheckpoints3, vehicleRaceStartingPositions3);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(vinewoodLONGDebug);



        List<VehicleRaceStartingPosition> vinewood3start = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-1543.546f, -197.2341f, 54.76277f), 38.42368f),
            new VehicleRaceStartingPosition(1, new Vector3(-1535.816f, -207.0271f, 54.03721f), 38.3179f),
            new VehicleRaceStartingPosition(2, new Vector3(-1538.248f, -195.0614f, 54.58556f), 43.03611f),
            new VehicleRaceStartingPosition(3, new Vector3(-1528.094f, -206.51f, 53.52767f), 43.48877f),
        };
        List<VehicleRaceCheckpoint> vinewood3checkpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-1592.985f, -134.3829f, 55.47266f)),
            new VehicleRaceCheckpoint(1, new Vector3(-1851.486f, 140.209f, 78.69758f)),
            new VehicleRaceCheckpoint(2, new Vector3(-1746.352f, 812.6461f, 141.1693f)),
            new VehicleRaceCheckpoint(3, new Vector3(-1633.375f, 1043.433f, 152.5267f)),
            new VehicleRaceCheckpoint(4, new Vector3(-1471.54f, 1795.392f, 83.93646f)),
            new VehicleRaceCheckpoint(5, new Vector3(-1531.019f, 2142.836f, 54.96165f)),
            new VehicleRaceCheckpoint(6, new Vector3(-1654.215f, 2383.382f, 35.76158f)),
            new VehicleRaceCheckpoint(7, new Vector3(-1754.635f, 2429.498f, 30.93673f)),
            new VehicleRaceCheckpoint(8, new Vector3(-2012.238f, 2343.076f, 33.18634f)),

        };
        VehicleRaceTrack vinewood3 = new VehicleRaceTrack("vinewoodRace3", "Morning Wood Mountain Route", "Race through the mountain freeways to Lago Zancudo", vinewood3checkpoints, vinewood3start);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(vinewood3);


    }
    private void PaletoTracks()
    {
        List<VehicleRaceStartingPosition> paletoLoop1Starting = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0,new Vector3(218.0589f, 6565.234f, 31.52061f), 106.4273f),
            new VehicleRaceStartingPosition(1,new Vector3(205.2844f, 6560.81f, 31.64036f), 109.7968f),
            new VehicleRaceStartingPosition(2,new Vector3(190.9381f, 6561.395f, 31.71398f), 110.7484f),
            new VehicleRaceStartingPosition(3,new Vector3(182.8164f, 6552.707f, 31.65496f), 120.4496f),
        };
        List<VehicleRaceCheckpoint> paletoLoop1Checkpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0,new Vector3(143.2037f, 6526.385f, 31.3439f)),
            new VehicleRaceCheckpoint(1,new Vector3(-202.2245f, 6173.541f, 30.57405f)),
            //new VehicleRaceCheckpoint(2,new Vector3(-341.7607f, 6269.313f, 30.86405f)),
            //new VehicleRaceCheckpoint(3,new Vector3(-358.0514f, 6293.811f, 29.61264f)),
            new VehicleRaceCheckpoint(2,new Vector3(-181.1622f, 6468.749f, 30.21145f)),
            new VehicleRaceCheckpoint(3,new Vector3(150.0628f, 6533.176f, 31.4289f)),
            //new VehicleRaceCheckpoint(0,new Vector3(143.2037f, 6526.385f, 31.3439f)),
            //new VehicleRaceCheckpoint(1,new Vector3(-215.3243f, 6169.238f, 30.88095f)),
            //new VehicleRaceCheckpoint(2,new Vector3(-294.1931f, 6220.725f, 31.19704f)),
            //new VehicleRaceCheckpoint(3,new Vector3(-358.0514f, 6293.811f, 29.61264f)),
            //new VehicleRaceCheckpoint(4,new Vector3(-181.1622f, 6468.749f, 30.21145f)),
            //new VehicleRaceCheckpoint(5,new Vector3(150.0628f, 6533.176f, 31.4289f)),
        };
        VehicleRaceTrack paletoLoop1 = new VehicleRaceTrack("paletoloop1", "Paleto Bay Loop", "Do a loop around Paleto Bay", paletoLoop1Checkpoints, paletoLoop1Starting);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(paletoLoop1);



        List<VehicleRaceStartingPosition> paletoLoop2Starting = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0,new Vector3(218.0589f, 6565.234f, 31.52061f), 106.4273f),
            new VehicleRaceStartingPosition(1,new Vector3(205.2844f, 6560.81f, 31.64036f), 109.7968f),
            new VehicleRaceStartingPosition(2,new Vector3(190.9381f, 6561.395f, 31.71398f), 110.7484f),
            new VehicleRaceStartingPosition(3,new Vector3(182.8164f, 6552.707f, 31.65496f), 120.4496f),
        };
        List<VehicleRaceCheckpoint> paletoLoop2Checkpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0,new Vector3(141.9506f, 6543.649f, 31.27871f)),
            new VehicleRaceCheckpoint(1,new Vector3(87.16878f, 6598.012f, 31.2261f)),
            new VehicleRaceCheckpoint(2,new Vector3(-290.8184f, 6247.322f, 31.10103f)),
            new VehicleRaceCheckpoint(3,new Vector3(-237.6497f, 6163.285f, 31.16849f)),
            new VehicleRaceCheckpoint(4,new Vector3(152.6836f, 6522.836f, 31.30534f)),
        };
        VehicleRaceTrack paletoLoop2 = new VehicleRaceTrack("paletoloop2", "Paleto Bay Alternate Loop", "Do a loop around Paleto Bay", paletoLoop2Checkpoints, paletoLoop2Starting);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(paletoLoop2);



        List<VehicleRaceStartingPosition> paletoDrag1Starting = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0,new Vector3(218.0589f, 6565.234f, 31.52061f), 106.4273f),
            new VehicleRaceStartingPosition(1,new Vector3(205.2844f, 6560.81f, 31.64036f), 109.7968f),
            new VehicleRaceStartingPosition(2,new Vector3(190.9381f, 6561.395f, 31.71398f), 110.7484f),
            new VehicleRaceStartingPosition(3,new Vector3(182.8164f, 6552.707f, 31.65496f), 120.4496f),
        };
        List<VehicleRaceCheckpoint> paletoDrag1Checkpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0,new Vector3(143.2037f, 6526.385f, 31.3439f)),
            new VehicleRaceCheckpoint(1, new Vector3(-769.4749f, 5497.793f, 34.48269f)),


        };
        VehicleRaceTrack paletoDrag1 = new VehicleRaceTrack("paletodrag1", "Paleto Bay Drag", "Drag race across paleto bay", paletoDrag1Checkpoints, paletoDrag1Starting);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(paletoDrag1);

    }



    private void CentralTracks()
    {
        List<VehicleRaceStartingPosition> central1start = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(68.09341f, -1183.503f, 28.72084f), 2.301179f),
            new VehicleRaceStartingPosition(1, new Vector3(72.77296f, -1185.764f, 28.65525f), 8.259281f),
            new VehicleRaceStartingPosition(2, new Vector3(67.94074f, -1197.815f, 28.72175f), 1.318574f),
            new VehicleRaceStartingPosition(3, new Vector3(73.90773f, -1198.595f, 28.61956f), 0.1617375f),
        };
        List<VehicleRaceCheckpoint> central1checkpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(80.30048f, -1067.49f, 28.79053f)),
            new VehicleRaceCheckpoint(1, new Vector3(172.9085f, -818.7173f, 30.55356f)),
            new VehicleRaceCheckpoint(2, new Vector3(303.2614f, -470.1808f, 42.71596f)),
            new VehicleRaceCheckpoint(3, new Vector3(183.5113f, -337.3925f, 43.44069f)),
            new VehicleRaceCheckpoint(4, new Vector3(-61.29679f, -245.9679f, 44.78765f)),
            new VehicleRaceCheckpoint(5, new Vector3(-506.2202f, -272.2784f, 35.05164f)),

        };
        VehicleRaceTrack central1 = new VehicleRaceTrack("centralRace1", "Strawberry To Rockford City Hall", "Race to rockford city hall from the Strawberry underpass", central1checkpoints, central1start);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(central1);


        List<VehicleRaceStartingPosition> central2start = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(-414.8134f, -5.814307f, 46.03317f), 264.2367f),
            new VehicleRaceStartingPosition(1, new Vector3(-426.5735f, -4.847809f, 45.74813f), 265.0832f),
            new VehicleRaceStartingPosition(2, new Vector3(-415.077f, -10.34778f, 46.00951f), 259.6769f),
            new VehicleRaceStartingPosition(3, new Vector3(-426.7999f, -8.770337f, 45.73085f), 259.6824f),
        };
        List<VehicleRaceCheckpoint> central2checkpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-323.7401f, -25.45497f, 47.50704f)),
            new VehicleRaceCheckpoint(1, new Vector3(514.7919f, -333.9487f, 42.97514f)),

        };
        VehicleRaceTrack central2 = new VehicleRaceTrack("centralRace2", "Burton Cross Town Drag", "Drag race across central LS starting at Burton", central2checkpoints, central2start);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(central2);

        List<VehicleRaceStartingPosition> central3start = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(1180.448f, -2556.885f, 34.84329f), 289.645f),
            new VehicleRaceStartingPosition(1, new Vector3(1171.138f, -2559.885f, 34.25787f), 287.5368f),
            new VehicleRaceStartingPosition(2, new Vector3(1181.931f, -2560.759f, 34.80701f), 279.2413f),
            new VehicleRaceStartingPosition(3, new Vector3(1171.136f, -2563.416f, 34.28861f), 286.6383f),
        };
        List<VehicleRaceCheckpoint> central3checkpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(1247.396f, -2525.508f, 41.12943f)),
            new VehicleRaceCheckpoint(1, new Vector3(1240.541f, -2021.258f, 43.51336f)),
            new VehicleRaceCheckpoint(2, new Vector3(1068.375f, -1284.138f, 25.21841f)),
            new VehicleRaceCheckpoint(3, new Vector3(715.1257f, -598.6426f, 35.34497f)),
            new VehicleRaceCheckpoint(4, new Vector3(-111.2042f, -495.9614f, 29.78688f)),
            new VehicleRaceCheckpoint(5, new Vector3(-237.5093f, -477.4375f, 25.16191f)),
            new VehicleRaceCheckpoint(6, new Vector3(-414.4864f, -694.7096f, 36.58041f)),
            new VehicleRaceCheckpoint(7, new Vector3(-410.9584f, -1352.381f, 36.67314f)),
            new VehicleRaceCheckpoint(8, new Vector3(-675.6441f, -1716.931f, 36.8155f)),
            new VehicleRaceCheckpoint(9, new Vector3(-764.5367f, -2088.595f, 33.70555f)),
            new VehicleRaceCheckpoint(10, new Vector3(189.1148f, -2666.291f, 17.49789f)),

        };
        VehicleRaceTrack central3 = new VehicleRaceTrack("centralRace3", "LS Freeway Loop", "Loop around the central LS freeway system", central3checkpoints, central3start);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(central3);
    }
}

