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
            foreach (VehicleRaceTrack vrt in VehicleRaceTypeManager.VehicleRaceTracks)
            {
                vrt.AddDistanceOffset(new Vector3(4949.947f, -3750.0441f, -0.000197f));//lpp OFFSET
            }
            Serialization.SerializeParam(VehicleRaceTypeManager, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LPPConfigFolder}\\VehicleRaces_{StaticStrings.LPPConfigSuffix}.xml");
        }
    }
    // Liberty City
    private void AlderneyTracks()
    {

    }
    private void AlgonquinTracks()
    {
        List<VehicleRaceStartingPosition> MirrorParkStarting1 = new List<VehicleRaceStartingPosition>()
        {
            new VehicleRaceStartingPosition(0,new Vector3(69.72437f, 1658.126f, 14.20163f), 270.7849f),
            new VehicleRaceStartingPosition(1,new Vector3(59.86195f, 1657.848f, 14.39589f), 271.6442f),
            new VehicleRaceStartingPosition(2,new Vector3(65.47762f, 1652.7f, 14.14361f), 272.2311f),
            new VehicleRaceStartingPosition(3,new Vector3(55.55209f, 1652.535f, 14.15949f), 271.7588f),
        };
        List<VehicleRaceCheckpoint> MirrorParkCheckpoints1 = new List<VehicleRaceCheckpoint>()
        {
            new VehicleRaceCheckpoint(0,new Vector3(149.2451f, 1660.993f, 14.22393f)),
            new VehicleRaceCheckpoint(1,new Vector3(188.8991f, 1603.399f, 14.22536f)),
            new VehicleRaceCheckpoint(2,new Vector3(189.0238f, 1337.025f, 14.1711f)),
            new VehicleRaceCheckpoint(3,new Vector3(188.8321f, 1157.596f, 14.15705f)),
            new VehicleRaceCheckpoint(4,new Vector3(99.03735f, 1114.738f, 14.24363f)),
            new VehicleRaceCheckpoint(5,new Vector3(-59.34015f, 1115.071f, 14.29004f)),
            new VehicleRaceCheckpoint(6,new Vector3(-125.7141f, 1179.294f, 14.16112f)),
            new VehicleRaceCheckpoint(7,new Vector3(-125.2539f, 1382.75f, 14.1547f)),
            new VehicleRaceCheckpoint(8,new Vector3(-125.2586f, 1634.502f, 14.21259f)),
            new VehicleRaceCheckpoint(9,new Vector3(-50.03026f, 1661.24f, 14.21177f)),
            new VehicleRaceCheckpoint(10,new Vector3(139.9709f, 1661.193f, 14.21052f)),
            new VehicleRaceCheckpoint(11,new Vector3(188.8048f, 1599.231f, 14.19743f)),
            new VehicleRaceCheckpoint(12,new Vector3(188.7102f, 1439.857f, 14.17099f)),
        };
        VehicleRaceTrack middlepark1 = new VehicleRaceTrack("middlepark1", "Middle Park 1", "Middle Park Circuit", MirrorParkCheckpoints1, MirrorParkStarting1);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(middlepark1);
    }

    private void BohanTracks()
    {

    }
    private void BrokerTracks()
    {

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

