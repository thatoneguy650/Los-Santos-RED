using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;


public class VehicleRaces_Liberty
{

    public VehicleRaces_Liberty(VehicleRaces vehicleRaces)
    {
        VehicleRaceTypeManager = VehicleRaceTypeManager;
    }


    public VehicleRaceTypeManager VehicleRaceTypeManager { get; private set; }

    public void DefaultConfig()
    {
        {
            VehicleRaceTypeManager = new VehicleRaceTypeManager();
            VehicleRaceTypeManager.VehicleRaceTracks = new List<VehicleRaceTrack>();

            AlderneyTracks();
            AlgonquinTracks();
            BohanTracks();
            BrokerTracks();
            DukesTracks();

            Serialization.SerializeParam(VehicleRaceTypeManager, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LibertyConfigFolder}\\VehicleRaces_{StaticStrings.LibertyConfigSuffix}.xml");
            //foreach (VehicleRaceTrack vrt in VehicleRaceTypeManager.VehicleRaceTracks)
            //{
            //    vrt.AddDistanceOffset(new Vector3(4949.947f, -3750.0441f, -0.000197f));//lpp OFFSET
            //}
            //Serialization.SerializeParam(VehicleRaceTypeManager, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LPPConfigFolder}\\VehicleRaces_{StaticStrings.LPPConfigSuffix}.xml");
            // Your services are no longer required

        }
    }

    // Liberty City
    private void AlderneyTracks()
    {
        List<VehicleRaceStartingPosition> westdykeStarting1 = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0,new Vector3(-995.597f, 2214.189f, 29.78997f), 91.44264f),
            new VehicleRaceStartingPosition(1,new Vector3(-995.5977f, 2208.946f, 29.79504f), 90.4465f),
            new VehicleRaceStartingPosition(2,new Vector3(-1003.541f, 2214.152f, 29.08567f), 90.06337f),
            new VehicleRaceStartingPosition(3,new Vector3(-1003.718f, 2208.838f, 29.08035f), 90.2548f),
            new VehicleRaceStartingPosition(4,new Vector3(-1011.904f, 2214.018f, 28.36331f), 91.23151f),
            new VehicleRaceStartingPosition(5,new Vector3(-1011.659f, 2208.765f, 28.38305f), 90.26228f),
            new VehicleRaceStartingPosition(6,new Vector3(-1019.887f, 2214.022f, 27.6422f), 90.08096f),
            new VehicleRaceStartingPosition(7,new Vector3(-1019.54f, 2208.764f, 27.67936f), 90.98108f),
            new VehicleRaceStartingPosition(8,new Vector3(-1027.453f, 2213.926f, 27.15454f), 90.42159f),
            new VehicleRaceStartingPosition(9,new Vector3(-1027.439f, 2208.668f, 27.15129f), 90.00889f),
        };
        List<VehicleRaceCheckpoint> westdykeCheckpoints1 = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-1113.636f, 2037.948f, 21.31051f)),
            new VehicleRaceCheckpoint(1, new Vector3(-1155.167f, 2199.934f, 17.58205f)),
            new VehicleRaceCheckpoint(2, new Vector3(-869.878f, 2298.023f, 10.0496f)),
            new VehicleRaceCheckpoint(3, new Vector3(-754.3835f, 2084.997f, 23.23106f)),
            new VehicleRaceCheckpoint(4, new Vector3(-849.2402f, 2151.781f, 33.98346f)),
            new VehicleRaceCheckpoint(5, new Vector3(-1060.451f, 2211.42f, 27.03767f)),
        };
        VehicleRaceTrack westdyke1 = new VehicleRaceTrack("westdyke1", "Westdyke Circuit", "Westdyke Circuit", westdykeCheckpoints1, westdykeStarting1);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(westdyke1);

        List<VehicleRaceStartingPosition> plumskyStarting1 = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0,new Vector3(-613.1808f, 1663.109f, 17.52267f), 90.74471f),
            new VehicleRaceStartingPosition(1,new Vector3(-613.433f, 1667.375f, 17.50609f), 90.48474f),
            new VehicleRaceStartingPosition(2,new Vector3(-621.5459f, 1662.951f, 17.51366f), 90.49436f),
            new VehicleRaceStartingPosition(3,new Vector3(-621.6651f, 1667.253f, 17.50805f), 91.5169f),
            new VehicleRaceStartingPosition(4,new Vector3(-630.2946f, 1662.593f, 17.51439f), 92.73031f),
            new VehicleRaceStartingPosition(5,new Vector3(-630.4907f, 1666.833f, 17.50649f), 91.82471f),
            new VehicleRaceStartingPosition(6,new Vector3(-639.1602f, 1662.106f, 17.52058f), 92.22294f),
            new VehicleRaceStartingPosition(7,new Vector3(-639.0291f, 1666.487f, 17.49508f), 93.1549f),
            new VehicleRaceStartingPosition(8,new Vector3(-648.1289f, 1661.84f, 17.51801f), 92.64743f),
            new VehicleRaceStartingPosition(9,new Vector3(-648.2291f, 1666.141f, 17.45568f), 92.07905f),
        };
        List<VehicleRaceCheckpoint> plumskyCheckpoints1 = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-1024.75f, 1664.0f, 28.6875f)),
            new VehicleRaceCheckpoint(1, new Vector3(-1392.75f, 1538.25f, 29.65625f)),
            new VehicleRaceCheckpoint(2, new Vector3(-1393.75f, 1373.25f, 29.65625f)),
            new VehicleRaceCheckpoint(3, new Vector3(-1533.25f, 1023.25f, 22.15625f)),
            new VehicleRaceCheckpoint(4, new Vector3(-1736.412f, 738.2251f, 22.79781f)),
            new VehicleRaceCheckpoint(5, new Vector3(-1776.513f, 485.5643f, 6.89457f)),
        };
        VehicleRaceTrack plumsky1 = new VehicleRaceTrack("plumsky1", "Plumbers Skyway Sprint", "Plumbers Skyway Sprint", plumskyCheckpoints1, plumskyStarting1);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(plumsky1);

        List<VehicleRaceStartingPosition> prison1Starting1 = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0,new Vector3(-1361.516f, 8.790751f, 2.379293f), 87.82319f),
            new VehicleRaceStartingPosition(1,new Vector3(-1361.357f, 4.053098f, 2.429902f), 88.8176f),
            new VehicleRaceStartingPosition(2,new Vector3(-1370.056f, 8.976987f, 2.39333f), 89.75267f),
            new VehicleRaceStartingPosition(3,new Vector3(-1369.886f, 4.212513f, 2.421556f), 88.51682f),
            new VehicleRaceStartingPosition(4,new Vector3(-1379.13f, 9.070815f, 2.406297f), 90.11492f),
            new VehicleRaceStartingPosition(5,new Vector3(-1378.916f, 4.224892f, 2.435541f), 90.0435f),
            new VehicleRaceStartingPosition(6,new Vector3(-1388.008f, 9.056837f, 2.399259f), 90.21678f),
            new VehicleRaceStartingPosition(7,new Vector3(-1387.9f, 4.182292f, 2.413176f), 90.47901f),
            new VehicleRaceStartingPosition(8,new Vector3(-1396.686f, 9.161465f, 2.39834f), 90.98397f),
            new VehicleRaceStartingPosition(9,new Vector3(-1396.338f, 4.025148f, 2.405079f), 91.32798f),
        };
        List<VehicleRaceCheckpoint> prison1Checkpoints1 = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-1509.13f, 44.60282f, 2.277811f)),
            new VehicleRaceCheckpoint(1, new Vector3(-1368.703f, 240.7724f, 2.549987f)),
            new VehicleRaceCheckpoint(2, new Vector3(-1024.939f, 272.5863f, 2.41119f)),
            new VehicleRaceCheckpoint(3, new Vector3(-927.25f, 154.0f, 2.249398f)),
            new VehicleRaceCheckpoint(4, new Vector3(-936.572f, 87.0f, 1.950306f)),
            new VehicleRaceCheckpoint(5, new Vector3(-986.0f, 50.0f, 1.95019448f)),
            new VehicleRaceCheckpoint(6, new Vector3(-975.25f, -18.0f, 1.9375f)),
            new VehicleRaceCheckpoint(7, new Vector3(-1209.628f, -18.41422f, 2.455626f)),
            new VehicleRaceCheckpoint(8, new Vector3(-1399.227f, 6.540926f, 2.284748f)),
        };
        VehicleRaceTrack prison1 = new VehicleRaceTrack("prison1", "Prison Circuit", "Prison Circuit", prison1Checkpoints1, prison1Starting1);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(prison1);

        List<VehicleRaceStartingPosition> hardtack1Starting1 = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0,new Vector3(-1903.121f, 401.6062f, 6.247527f), 269.6222f),
            new VehicleRaceStartingPosition(1,new Vector3(-1903.115f, 396.4162f, 6.236014f), 269.5677f),
            new VehicleRaceStartingPosition(2,new Vector3(-1894.623f, 401.6404f, 6.246681f), 270.1324f),
            new VehicleRaceStartingPosition(3,new Vector3(-1894.434f, 396.4092f, 6.211138f), 271.0771f),
            new VehicleRaceStartingPosition(4,new Vector3(-1885.594f, 401.6992f, 6.245941f), 271.1767f),
            new VehicleRaceStartingPosition(5,new Vector3(-1885.49f, 396.4508f, 6.185169f), 269.761f),
            new VehicleRaceStartingPosition(6,new Vector3(-1876.541f, 401.7483f, 6.245479f), 269.2715f),
            new VehicleRaceStartingPosition(7,new Vector3(-1876.547f, 396.4403f, 6.212371f), 270.6089f),
            new VehicleRaceStartingPosition(8,new Vector3(-1867.115f, 401.8368f, 6.244209f), 270.2663f),
            new VehicleRaceStartingPosition(9,new Vector3(-1866.923f, 396.5668f, 6.240908f), 271.4607f),
        };
        List<VehicleRaceCheckpoint> hardtack1Checkpoints1 = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(-1658.384f, 425.775f, 6.512216f)),
            new VehicleRaceCheckpoint(1, new Vector3(-1379.795f, 530.4913f, 9.683433f)),
            new VehicleRaceCheckpoint(2, new Vector3(-1348.042f, 691.9232f, 12.84864f)),
            new VehicleRaceCheckpoint(3, new Vector3(-1431.466f, 723.4999f, 18.31191f)),
            new VehicleRaceCheckpoint(4, new Vector3(-1647.202f, 663.9177f, 12.09186f)),
            new VehicleRaceCheckpoint(5, new Vector3(-1914.989f, 682.934f, 12.69999f)),
            new VehicleRaceCheckpoint(6, new Vector3(-1953.355f, 440.1216f, 6.775503f)),
            new VehicleRaceCheckpoint(7, new Vector3(-1857.968f, 400.0427f, 6.758904f)),
        };
        VehicleRaceTrack hardtack1 = new VehicleRaceTrack("hardtack1", "Hardtack Circuit", "Hardtack Circuit", hardtack1Checkpoints1, hardtack1Starting1);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(hardtack1);
    }
    private void AlgonquinTracks()
    {
        List<VehicleRaceStartingPosition> MiddleParkStarting1 = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0,new Vector3(180.1318f, 1487.792f, 14.45536f), 180.601f),
            new VehicleRaceStartingPosition(1,new Vector3(185.5946f, 1488.061f, 14.11838f), 179.7664f),
            new VehicleRaceStartingPosition(2,new Vector3(180.1129f, 1478.821f, 14.17838f), 180.1682f),
            new VehicleRaceStartingPosition(3,new Vector3(185.5576f, 1478.505f, 14.36232f), 180.1155f),
            new VehicleRaceStartingPosition(4,new Vector3(180.2405f, 1467.745f, 14.21276f), 179.6347f),
            new VehicleRaceStartingPosition(5,new Vector3(185.7909f, 1468.067f, 14.22278f), 180.5256f),
            new VehicleRaceStartingPosition(6,new Vector3(180.2435f, 1457.156f, 14.24454f), 180.1762f),
            new VehicleRaceStartingPosition(7,new Vector3(185.7061f, 1457.617f, 14.28164f), 180.2712f),
            new VehicleRaceStartingPosition(8,new Vector3(180.3083f, 1446.574f, 14.14135f), 180.4321f),
            new VehicleRaceStartingPosition(9,new Vector3(185.409f, 1446.838f, 14.30031f), 180.404f),
        };
        List<VehicleRaceCheckpoint> MiddleParkCheckpoints1 = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(182.8764f, 1154.761f, 14.31166f)),
            new VehicleRaceCheckpoint(1, new Vector3(-70.60763f, 1115.035f, 14.52465f)),
            new VehicleRaceCheckpoint(2, new Vector3(-119.5832f, 1616.149f, 14.41903f)),
            new VehicleRaceCheckpoint(3, new Vector3(142.8221f, 1655.306f, 14.43464f)),
            new VehicleRaceCheckpoint(4, new Vector3(182.804f, 1473.664f, 14.03568f)),
        };
        VehicleRaceTrack middlepark1 = new VehicleRaceTrack("middlepark1", "Middle Park Circuit", "Middle Park Circuit", MiddleParkCheckpoints1, MiddleParkStarting1);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(middlepark1);

        List<VehicleRaceStartingPosition> StarDragStarting = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0,new Vector3(29.53401f, 1059.795f, 14.54682f), 180.4839f),
            new VehicleRaceStartingPosition(1,new Vector3(32.8921f, 1059.874f, 14.54687f), 179.8986f),
            new VehicleRaceStartingPosition(2,new Vector3(36.24197f, 1059.815f, 14.54687f), 180.2181f),
            new VehicleRaceStartingPosition(3,new Vector3(39.62174f, 1059.81f, 14.54685f), 179.5335f),
            new VehicleRaceStartingPosition(4,new Vector3(29.62701f, 1051.812f, 14.51089f), 182.4698f),
            new VehicleRaceStartingPosition(5,new Vector3(32.86932f, 1051.873f, 14.51008f), 180.4668f),
            new VehicleRaceStartingPosition(6,new Vector3(36.55596f, 1051.996f, 14.51134f), 179.0784f),
            new VehicleRaceStartingPosition(7,new Vector3(39.66347f, 1051.962f, 14.51371f), 180.794f),
        };
        List<VehicleRaceCheckpoint> StarDragCheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(31.43972f, 651.5617f, 14.46427f)),
            new VehicleRaceCheckpoint(1, new Vector3(33.30955f, 351.5252f, 14.12179f)),
        };
        VehicleRaceTrack starjdrag1 = new VehicleRaceTrack("starjdrag1", "Star Junction Drag", "Star Junction Drag Race", StarDragCheckpoints, StarDragStarting);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(starjdrag1);

        List<VehicleRaceStartingPosition> LancasterStarting1 = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0,new Vector3(387.905f, 1636.076f, 14.26228f), 181.9693f),
            new VehicleRaceStartingPosition(1,new Vector3(383.2553f, 1635.97f, 14.20699f), 180.286f),
            new VehicleRaceStartingPosition(2,new Vector3(388.1204f, 1626.354f, 14.26162f), 181.02f),
            new VehicleRaceStartingPosition(3,new Vector3(383.2834f, 1626.393f, 14.20702f), 179.7527f),
            new VehicleRaceStartingPosition(4,new Vector3(388.2068f, 1617.163f, 14.26177f), 179.6063f),
            new VehicleRaceStartingPosition(5,new Vector3(383.1967f, 1616.981f, 14.20375f), 179.4742f),
            new VehicleRaceStartingPosition(6,new Vector3(388.3218f, 1607.668f, 14.2617f), 181.1829f),
            new VehicleRaceStartingPosition(7,new Vector3(383.1417f, 1607.456f, 14.20237f), 179.6008f),
            new VehicleRaceStartingPosition(8,new Vector3(388.4666f, 1598.114f, 14.26158f), 180.7472f),
            new VehicleRaceStartingPosition(9,new Vector3(383.1661f, 1597.959f, 14.20303f), 181.2684f),
        };
        List<VehicleRaceCheckpoint> LancasterCheckpoints1 = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(393.8363f, 1360.969f, 7.568068f)),
            new VehicleRaceCheckpoint(1, new Vector3(543.0f, 244.75f, 14.0625f)),
            new VehicleRaceCheckpoint(2, new Vector3(548.75f, -24.0f, 14.0625f)),
            new VehicleRaceCheckpoint(3, new Vector3(247.6128f, -351.3391f, -6.995021f)),
            new VehicleRaceCheckpoint(4, new Vector3(8.424798f, -62.36927f, 4.319275f)),
        };
        VehicleRaceTrack Lancaster1 = new VehicleRaceTrack("Lancaster1", "Race to Liberty", "Race to Liberty", LancasterCheckpoints1, LancasterStarting1);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(Lancaster1);



    }

    private void BohanTracks()
    {

    }
    private void BrokerTracks()
    {
        List<VehicleRaceStartingPosition> FIAStarting1 = new List<VehicleRaceStartingPosition>()
        {

            new VehicleRaceStartingPosition(0, new Vector3(2565.346f, 831.7686f, 5.559813f), 359.4457f),
            new VehicleRaceStartingPosition(1, new Vector3(2561.345f, 831.8093f, 5.556242f), 359.0019f),
            new VehicleRaceStartingPosition(2, new Vector3(2565.317f, 839.6428f, 5.563608f), 0.9221683f),
            new VehicleRaceStartingPosition(3, new Vector3(2561.274f, 839.749f, 5.555343f), 1.378026f),
            new VehicleRaceStartingPosition(4, new Vector3(2565.443f, 847.4093f, 5.560145f), 0.5928084f),
            new VehicleRaceStartingPosition(5, new Vector3(2561.313f, 847.3519f, 5.556522f), 0.7585424f),
            new VehicleRaceStartingPosition(6, new Vector3(2565.432f, 855.0338f, 5.559416f), 0.8882701f),
            new VehicleRaceStartingPosition(7, new Vector3(2561.389f, 855.035f, 5.55799f), 0.5398135f),
            new VehicleRaceStartingPosition(8, new Vector3(2565.321f, 862.5979f, 5.562071f), 0.3054437f),
            new VehicleRaceStartingPosition(9, new Vector3(2561.418f, 862.6079f, 5.558002f), 359.5417f),
        };
        List<VehicleRaceCheckpoint> FIACheckpoints1 = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(2419.194f, 985.8267f, 5.519069f)),
            new VehicleRaceCheckpoint(1, new Vector3(2563.401f, 768.4531f, 5.392743f)),
            new VehicleRaceCheckpoint(2, new Vector3(2563.485f, 873.2144f, 5.518051f)),

        };
        VehicleRaceTrack fiahotring1 = new VehicleRaceTrack("fiahotring1", "FIA Hotring", "Francis International Oval", FIACheckpoints1, FIAStarting1);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(fiahotring1);

        List<VehicleRaceStartingPosition> FIAStarting2 = new List<VehicleRaceStartingPosition>()
        {

            new VehicleRaceStartingPosition(0, new Vector3(2565.346f, 831.7686f, 5.559813f), 359.4457f),
            new VehicleRaceStartingPosition(1, new Vector3(2561.345f, 831.8093f, 5.556242f), 359.0019f),
            new VehicleRaceStartingPosition(2, new Vector3(2565.317f, 839.6428f, 5.563608f), 0.9221683f),
            new VehicleRaceStartingPosition(3, new Vector3(2561.274f, 839.749f, 5.555343f), 1.378026f),
            new VehicleRaceStartingPosition(4, new Vector3(2565.443f, 847.4093f, 5.560145f), 0.5928084f),
            new VehicleRaceStartingPosition(5, new Vector3(2561.313f, 847.3519f, 5.556522f), 0.7585424f),
            new VehicleRaceStartingPosition(6, new Vector3(2565.432f, 855.0338f, 5.559416f), 0.8882701f),
            new VehicleRaceStartingPosition(7, new Vector3(2561.389f, 855.035f, 5.55799f), 0.5398135f),
            new VehicleRaceStartingPosition(8, new Vector3(2565.321f, 862.5979f, 5.562071f), 0.3054437f),
            new VehicleRaceStartingPosition(9, new Vector3(2561.418f, 862.6079f, 5.558002f), 359.5417f),
        };
        List<VehicleRaceCheckpoint> FIACheckpoints2 = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(2419.194f, 985.8267f, 5.519069f)),
            new VehicleRaceCheckpoint(1, new Vector3(1963.216f, 777.8285f, 7.019152f)),
            new VehicleRaceCheckpoint(2, new Vector3(1490.75f, 782.314f, 30.05309f)),
            new VehicleRaceCheckpoint(3, new Vector3(1232.20032f, 760.953552f, 47.76645f)),
            new VehicleRaceCheckpoint(4, new Vector3(727.0f, 765.903931f, 43.9571533f)),
            new VehicleRaceCheckpoint(5, new Vector3(33.58518f, 755.1545f, 14.39168f)),

        };
        VehicleRaceTrack fiastarjunction = new VehicleRaceTrack("fiastarjunction", "FIA Star Junction", "FIA to Star Junction", FIACheckpoints2, FIAStarting2);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(fiastarjunction);
    }
    private void DukesTracks()
    {
        List<VehicleRaceStartingPosition> DukesHighwayStarting1 = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(5,new Vector3(1609.974f, 1386.459f, 13.27066f), 315.8207f),
            new VehicleRaceStartingPosition(4,new Vector3(1613.625f, 1383.434f, 13.21666f), 319.7684f),
            new VehicleRaceStartingPosition(3,new Vector3(1606.959f, 1374.81f, 13.20124f), 324.2783f),
            new VehicleRaceStartingPosition(2,new Vector3(1602.917f, 1377.4f, 13.25113f), 323.1328f),
            new VehicleRaceStartingPosition(1,new Vector3(1595.733f, 1368.204f, 13.24154f), 324.9805f),
            new VehicleRaceStartingPosition(0,new Vector3(1600.301f, 1365.219f, 13.20573f), 324.8135f),
        };
        List<VehicleRaceCheckpoint> DukesHighwayCheckpoints1 = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(1756.516f, 1478.477f, 13.38546f)),
            new VehicleRaceCheckpoint(1, new Vector3(2229.733f, 1294.777f, 8.724512f)),
            new VehicleRaceCheckpoint(2, new Vector3(2310.997f, 830.6649f, 19.23256f)),
            new VehicleRaceCheckpoint(3, new Vector3(2077.346f, 609.8213f, 17.09718f)),

        };
        VehicleRaceTrack dukeshighway1 = new VehicleRaceTrack("dukeshighway1", "Dukes Expressway", "Dukes Expressway Sprint", DukesHighwayCheckpoints1, DukesHighwayStarting1);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(dukeshighway1);

        List<VehicleRaceStartingPosition> DukesBoulevardStarting = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0, new Vector3(1639.562f, 1435.972f, 13.369f), 138.2494f),
            new VehicleRaceStartingPosition(1, new Vector3(1636.371f, 1438.801f, 13.36516f), 138.578f),
            new VehicleRaceStartingPosition(2, new Vector3(1631.616f, 1426.994f, 13.3654f), 138.023f),
            new VehicleRaceStartingPosition(3, new Vector3(1628.368f, 1429.975f, 13.36365f), 138.4367f),
            new VehicleRaceStartingPosition(4, new Vector3(1623.7f, 1417.918f, 13.36759f), 141.1688f),
            new VehicleRaceStartingPosition(5, new Vector3(1620.524f, 1420.868f, 13.34988f), 140.7071f),
        };
        List<VehicleRaceCheckpoint> DukesBoulevardCheckpoints = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0, new Vector3(1551.443f, 1302.724f, 28.03401f)),
            new VehicleRaceCheckpoint(1, new Vector3(1387.112f, 983.6382f, 28.44932f)),
            new VehicleRaceCheckpoint(2, new Vector3(1330.386f, 741.7487f, 30.69046f)),
            new VehicleRaceCheckpoint(3, new Vector3(1330.415f, 478.6942f, 33.40216f)),
            new VehicleRaceCheckpoint(4, new Vector3(1349.582f, 396.1772f, 35.61934f)),
            new VehicleRaceCheckpoint(5, new Vector3(1342.567f, 532.0981f, 33.41817f)),
            new VehicleRaceCheckpoint(6, new Vector3(1343.163f, 789.6508f, 30.37179f)),
            new VehicleRaceCheckpoint(7, new Vector3(1460.412f, 1090.755f, 37.46545f)),
            new VehicleRaceCheckpoint(8, new Vector3(1613.569f, 1386.178f, 13.2405f)),
        };
        VehicleRaceTrack dukesboule1 = new VehicleRaceTrack("dukesboule1", "Dukes Boulevard", "Dukes Boulevard Blast", DukesBoulevardCheckpoints, DukesBoulevardStarting);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(dukesboule1);
    }
}

