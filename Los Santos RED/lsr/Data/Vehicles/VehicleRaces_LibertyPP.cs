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


public class VehicleRaces_LibertyPP
{

    public VehicleRaces_LibertyPP(VehicleRaces vehicleRaces)
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

            Serialization.SerializeParam(VehicleRaceTypeManager, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LPPConfigFolder}\\VehicleRaces_{StaticStrings.LPPConfigSuffix}.xml");

        }
    }

    // Liberty City
    private void AlderneyTracks()
    {
        List<VehicleRaceStartingPosition> westdykeStarting1 = new List<VehicleRaceStartingPosition>()
        {
                new VehicleRaceStartingPosition(0, new Vector3(3954.34985f, -1535.85522f, 29.7897739f), 91.44264f),
                new VehicleRaceStartingPosition(1, new Vector3(3954.349f,   -1541.09814f, 29.7948437f), 90.4465f),
                new VehicleRaceStartingPosition(2, new Vector3(3946.40576f, -1535.89209f, 29.085474f),  90.06337f),
                new VehicleRaceStartingPosition(3, new Vector3(3946.22876f, -1541.2063f,  29.0801544f), 90.2548f),
                new VehicleRaceStartingPosition(4, new Vector3(3938.04272f, -1536.02612f, 28.3631134f), 91.23151f),
                new VehicleRaceStartingPosition(5, new Vector3(3938.28784f, -1541.2793f,  28.3828545f), 90.26228f),
                new VehicleRaceStartingPosition(6, new Vector3(3930.05981f, -1536.02222f, 27.642004f),  90.08096f),
                new VehicleRaceStartingPosition(7, new Vector3(3930.40674f, -1541.28027f, 27.679163f),  90.98108f),
                new VehicleRaceStartingPosition(8, new Vector3(3922.49365f, -1536.11816f,  27.1543427f), 90.42159f),
                new VehicleRaceStartingPosition(9, new Vector3(3922.50781f, -1541.37622f, 27.1510944f), 90.00889f),
        };
        List<VehicleRaceCheckpoint> westdykeCheckpoints1 = new List<VehicleRaceCheckpoint>()
        {
                new VehicleRaceCheckpoint(0, new Vector3(3853.279f, -1664.805f, 26.7716f)),
                new VehicleRaceCheckpoint(1, new Vector3(3784.186f, -1710.111f, 16.57557f)),
                new VehicleRaceCheckpoint(2, new Vector3(3794.896f, -1542.249f, 18.12416f)),
                new VehicleRaceCheckpoint(3, new Vector3(3801.274f, -1496.944f, 18.15788f)),
                new VehicleRaceCheckpoint(4, new Vector3(3856.283f, -1449.296f, 18.15768f)),
                new VehicleRaceCheckpoint(5, new Vector3(3947.17f, -1447.374f, 17.21162f)),
                new VehicleRaceCheckpoint(6, new Vector3(4077.229f, -1452.523f, 10.29842f)),
                new VehicleRaceCheckpoint(7, new Vector3(4214.083f, -1594.015f, 21.86138f)),
                new VehicleRaceCheckpoint(8, new Vector3(4195.846f, -1665.351f, 23.23079f)),
                new VehicleRaceCheckpoint(9, new Vector3(4157.467f, -1663.906f, 24.67818f)),
                new VehicleRaceCheckpoint(10, new Vector3(4100.892f, -1601.551f, 33.98298f)),
                new VehicleRaceCheckpoint(11, new Vector3(3899.239f, -1538.656f, 27.04146f)),
        };
        VehicleRaceTrack westdyke1 = new VehicleRaceTrack("westdyke1", "Westdyke Circuit", "Westdyke Circuit", westdykeCheckpoints1, westdykeStarting1);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(westdyke1);

        List<VehicleRaceStartingPosition> plumskyStarting1 = new List<VehicleRaceStartingPosition>()
        {
                new VehicleRaceStartingPosition(0, new Vector3(4336.766f,   -2086.935f,   17.5224743f), 90.74471f),
                new VehicleRaceStartingPosition(1, new Vector3(4336.51367f, -2082.66919f, 17.5058937f), 90.48474f),
                new VehicleRaceStartingPosition(2, new Vector3(4328.401f,   -2087.09326f, 17.513464f),  90.49436f),
                new VehicleRaceStartingPosition(3, new Vector3(4328.28174f, -2082.791f,   17.5078545f), 91.5169f),
                new VehicleRaceStartingPosition(4, new Vector3(4319.65234f, -2087.45117f, 17.5141945f), 92.73031f),
                new VehicleRaceStartingPosition(5, new Vector3(4319.456f,   -2083.21118f, 17.5062943f), 91.82471f),
                new VehicleRaceStartingPosition(6, new Vector3(4310.78662f, -2087.93823f, 17.5203838f), 92.22294f),
                new VehicleRaceStartingPosition(7, new Vector3(4310.91748f, -2083.55713f, 17.4948845f), 93.1549f),
                new VehicleRaceStartingPosition(8, new Vector3(4301.818f,   -2088.204f,   17.5178127f), 92.64743f),
                new VehicleRaceStartingPosition(9, new Vector3(4301.718f,   -2083.90332f, 17.4554844f), 92.07905f),
        };
        List<VehicleRaceCheckpoint> plumskyCheckpoints1 = new List<VehicleRaceCheckpoint>()
        {
                new VehicleRaceCheckpoint(0, new Vector3(3925.19678f, -2086.04419f, 28.6873035f)),
                new VehicleRaceCheckpoint(1, new Vector3(3557.19678f, -2211.79419f, 29.6560535f)),
                new VehicleRaceCheckpoint(2, new Vector3(3556.19678f, -2376.79419f, 29.6560535f)),
                new VehicleRaceCheckpoint(3, new Vector3(3416.69678f, -2726.79419f, 22.1560535f)),
                new VehicleRaceCheckpoint(4, new Vector3(3376.497f, -2814.183f, 21.4208f)),
                new VehicleRaceCheckpoint(5, new Vector3(3213.53467f, -3011.819f,   22.7976131f)),
                new VehicleRaceCheckpoint(6, new Vector3(3141.938f, -3270.851f, 6.35153f)),
        };
        VehicleRaceTrack plumsky1 = new VehicleRaceTrack("plumsky1", "Plumbers Skyway Sprint", "Plumbers Skyway Sprint", plumskyCheckpoints1, plumskyStarting1);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(plumsky1);

        List<VehicleRaceStartingPosition> prison1Starting1 = new List<VehicleRaceStartingPosition>()
        {
                new VehicleRaceStartingPosition(0, new Vector3(3588.43066f, -3741.25342f, 2.379096f), 87.82319f),
                new VehicleRaceStartingPosition(1, new Vector3(3588.58984f, -3745.99121f, 2.42970514f), 88.8176f),
                new VehicleRaceStartingPosition(2, new Vector3(3579.89063f, -3741.06714f, 2.393133f), 89.75267f),
                new VehicleRaceStartingPosition(3, new Vector3(3580.06079f, -3745.83179f, 2.421359f), 88.51682f),
                new VehicleRaceStartingPosition(4, new Vector3(3570.817f,   -3740.97339f, 2.4061f), 90.11492f),
                new VehicleRaceStartingPosition(5, new Vector3(3571.03076f, -3745.81934f, 2.435344f), 90.0435f),
                new VehicleRaceStartingPosition(6, new Vector3(3561.93872f, -3740.9873f,  2.399062f), 90.21678f),
                new VehicleRaceStartingPosition(7, new Vector3(3562.04688f, -3745.86182f, 2.412979f), 90.47901f),
                new VehicleRaceStartingPosition(8, new Vector3(3553.26074f, -3740.88281f, 2.398143f), 90.98397f),
                new VehicleRaceStartingPosition(9, new Vector3(3553.609f,   -3746.019f,   2.404882f), 91.32798f),
        };
        List<VehicleRaceCheckpoint> prison1Checkpoints1 = new List<VehicleRaceCheckpoint>()
        {
                new VehicleRaceCheckpoint(0, new Vector3(3440.817f,    -3705.44141f, 2.277614f)),
                new VehicleRaceCheckpoint(1, new Vector3(3581.24365f,  -3509.27173f, 2.54979014f)),
                new VehicleRaceCheckpoint(2, new Vector3(3925.00781f,  -3477.458f,   2.410993f)),
                new VehicleRaceCheckpoint(3, new Vector3(4022.69678f, -3596.04419f, 2.249201f)),
                new VehicleRaceCheckpoint(4, new Vector3(4013.37476f,  -3663.04419f, 1.950109f)),
                new VehicleRaceCheckpoint(5, new Vector3(3963.94678f,  -3700.04419f, 1.94999743f)),
                new VehicleRaceCheckpoint(6, new Vector3(3974.69678f,  -3768.04419f, 1.937303f)),
                new VehicleRaceCheckpoint(7, new Vector3(3740.31885f,  -3768.4585f,  2.455429f)),
                new VehicleRaceCheckpoint(8, new Vector3(3550.71973f,  -3743.50317f, 2.28455114f)),
        };
        VehicleRaceTrack prison1 = new VehicleRaceTrack("prison1", "Prison Circuit", "Prison Circuit", prison1Checkpoints1, prison1Starting1);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(prison1);

        List<VehicleRaceStartingPosition> hardtack1Starting1 = new List<VehicleRaceStartingPosition>()
        {
                new VehicleRaceStartingPosition(0, new Vector3(3046.82568f, -3348.438f,  6.24733f), 269.6222f),
                new VehicleRaceStartingPosition(1, new Vector3(3046.83179f, -3353.628f,  6.235817f), 269.5677f),
                new VehicleRaceStartingPosition(2, new Vector3(3055.32373f, -3348.40381f,6.246484f),270.1324f),
                new VehicleRaceStartingPosition(3, new Vector3(3055.5127f,  -3353.635f,  6.210941f), 271.0771f),
                new VehicleRaceStartingPosition(4, new Vector3(3064.35278f, -3348.345f,  6.245744f), 271.1767f),
                new VehicleRaceStartingPosition(5, new Vector3(3064.45679f, -3353.59326f,6.184972f),269.761f),
                new VehicleRaceStartingPosition(6, new Vector3(3073.40576f, -3348.296f,  6.245282f), 269.2715f),
                new VehicleRaceStartingPosition(7, new Vector3(3073.4f,     -3353.604f,  6.212174f), 270.6089f),
                new VehicleRaceStartingPosition(8, new Vector3(3082.83179f, -3348.20752f,6.244012f), 270.2663f),
                new VehicleRaceStartingPosition(9, new Vector3(3083.024f,   -3353.47729f,6.240711f), 271.4607f),
        };
        List<VehicleRaceCheckpoint> hardtack1Checkpoints1 = new List<VehicleRaceCheckpoint>()
        {
                new VehicleRaceCheckpoint(0, new Vector3(3291.56274f, -3324.26929f, 6.512019f)),
                new VehicleRaceCheckpoint(1, new Vector3(3570.15186f, -3219.553f,   9.683235f)),
                new VehicleRaceCheckpoint(2, new Vector3(3601.90479f, -3058.121f,   12.848443f)),
                new VehicleRaceCheckpoint(3, new Vector3(3518.481f,   -3026.54443f, 18.3117142f)),
                new VehicleRaceCheckpoint(4, new Vector3(3302.74463f, -3086.12646f, 12.0916624f)),
                new VehicleRaceCheckpoint(5, new Vector3(3034.95776f, -3067.11f,    12.6997929f)),
                new VehicleRaceCheckpoint(6, new Vector3(2996.5918f, -3309.92261f, 6.775306f)),
                new VehicleRaceCheckpoint(7, new Vector3(3091.97876f, -3350.00146f, 6.758707f)),
        };
        VehicleRaceTrack hardtack1 = new VehicleRaceTrack("hardtack1", "Hardtack Circuit", "Hardtack Circuit", hardtack1Checkpoints1, hardtack1Starting1);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(hardtack1);
    }
    private void AlgonquinTracks()
    {
        List<VehicleRaceStartingPosition> MiddleParkStarting1 = new List<VehicleRaceStartingPosition>()
        {
                new VehicleRaceStartingPosition(0, new Vector3(5130.07861f, -2262.2522f,  14.455163f), 180.601f),
                new VehicleRaceStartingPosition(1, new Vector3(5135.5415f,  -2261.98315f, 14.1181822f),179.7664f),
                new VehicleRaceStartingPosition(2, new Vector3(5130.05957f, -2271.22314f, 14.1781826f),180.1682f),
                new VehicleRaceStartingPosition(3, new Vector3(5135.50439f, -2271.539f,   14.3621225f),180.1155f),
                new VehicleRaceStartingPosition(4, new Vector3(5130.1875f,  -2282.29932f, 14.2125626f),179.6347f),
                new VehicleRaceStartingPosition(5, new Vector3(5135.738f,   -2281.977f,   14.2225828f),180.5256f),
                new VehicleRaceStartingPosition(6, new Vector3(5130.19043f, -2292.88818f, 14.2443428f),180.1762f),
                new VehicleRaceStartingPosition(7, new Vector3(5135.653f,   -2292.42725f, 14.2814426f),180.2712f),
                new VehicleRaceStartingPosition(8, new Vector3(5130.255f,   -2303.47021f, 14.1411524f),180.4321f),
                new VehicleRaceStartingPosition(9, new Vector3(5135.356f,   -2303.206f,   14.3001127f),180.404f),
        };
        List<VehicleRaceCheckpoint> MiddleParkCheckpoints1 = new List<VehicleRaceCheckpoint>()
        {
                new VehicleRaceCheckpoint(0, new Vector3(5132.823f,   -2595.2832f, 14.3114624f)),
                new VehicleRaceCheckpoint(1, new Vector3(4879.33936f, -2635.00928f,14.5244522f)),
                new VehicleRaceCheckpoint(2, new Vector3(4830.364f,   -2133.895f,  14.4188328f)),
                new VehicleRaceCheckpoint(3, new Vector3(5092.769f,   -2094.73828f,14.4344425f)),
                new VehicleRaceCheckpoint(4, new Vector3(5132.751f,   -2276.38037f,14.0354824f)),
        };
        VehicleRaceTrack middlepark1 = new VehicleRaceTrack("middlepark1", "Middle Park Circuit", "Middle Park Circuit", MiddleParkCheckpoints1, MiddleParkStarting1);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(middlepark1);

        List<VehicleRaceStartingPosition> StarDragStarting = new List<VehicleRaceStartingPosition>()
        {
                new VehicleRaceStartingPosition(0, new Vector3(4979.481f,   -2690.249f,   14.5466223f), 180.4839f),
                new VehicleRaceStartingPosition(1, new Vector3(4982.839f,   -2690.17017f, 14.5466728f), 179.8986f),
                new VehicleRaceStartingPosition(2, new Vector3(4986.189f,   -2690.22925f, 14.5466728f), 180.2181f),
                new VehicleRaceStartingPosition(3, new Vector3(4989.56836f, -2690.23413f, 14.5466528f), 179.5335f),
                new VehicleRaceStartingPosition(4, new Vector3(4979.57373f, -2698.23218f, 14.5106926f), 182.4698f),
                new VehicleRaceStartingPosition(5, new Vector3(4982.816f,   -2698.17114f, 14.5098829f), 180.4668f),
                new VehicleRaceStartingPosition(6, new Vector3(4986.503f,   -2698.04834f, 14.5111427f),179.0784f),
                new VehicleRaceStartingPosition(7, new Vector3(4989.61035f, -2698.082f,   14.5135126f), 180.794f),
        };
        List<VehicleRaceCheckpoint> StarDragCheckpoints = new List<VehicleRaceCheckpoint>()
        {
                new VehicleRaceCheckpoint(0, new Vector3(4981.38672f, -3098.48242f, 14.4640722f)),
                new VehicleRaceCheckpoint(1, new Vector3(4983.25635f, -3398.519f,   14.1215925f)),
        };
        VehicleRaceTrack starjdrag1 = new VehicleRaceTrack("starjdrag1", "Star Junction Drag", "Star Junction Drag Race", StarDragCheckpoints, StarDragStarting);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(starjdrag1);

        List<VehicleRaceStartingPosition> LancasterStarting1 = new List<VehicleRaceStartingPosition>()
        {
                new VehicleRaceStartingPosition(0, new Vector3(5337.85156f, -2113.96826f, 14.2620831f), 181.9693f),
                new VehicleRaceStartingPosition(1, new Vector3(5333.202f,   -2114.07422f, 14.2067928f), 180.286f),
                new VehicleRaceStartingPosition(2, new Vector3(5338.06738f, -2123.69019f, 14.2614222f), 181.02f),
                new VehicleRaceStartingPosition(3, new Vector3(5333.23f,    -2123.65137f, 14.2068224f), 179.7527f),
                new VehicleRaceStartingPosition(4, new Vector3(5338.15332f, -2132.88135f, 14.2615728f), 179.6063f),
                new VehicleRaceStartingPosition(5, new Vector3(5333.14355f, -2133.06323f, 14.2035522f), 179.4742f),
                new VehicleRaceStartingPosition(6, new Vector3(5338.26855f, -2142.37622f, 14.2615023f), 181.1829f),
                new VehicleRaceStartingPosition(7, new Vector3(5333.08838f, -2142.58813f, 14.2021723f), 179.6008f),
                new VehicleRaceStartingPosition(8, new Vector3(5338.41357f, -2151.93018f, 14.2613831f), 180.7472f),
                new VehicleRaceStartingPosition(9, new Vector3(5333.113f,   -2152.08521f, 14.2028322f), 181.2684f),
        };
        List<VehicleRaceCheckpoint> LancasterCheckpoints1 = new List<VehicleRaceCheckpoint>()
        {
                new VehicleRaceCheckpoint(0, new Vector3(5343.783f,    -2389.0752f, 7.567871f)),
                new VehicleRaceCheckpoint(1, new Vector3(5492.947f,    -3505.29419f,14.0623026f)),
                new VehicleRaceCheckpoint(2, new Vector3(5498.697f,    -3774.04419f,14.0623026f)),
                new VehicleRaceCheckpoint(3, new Vector3(5197.55957f,  -4101.3833f, -6.995218f)),
                new VehicleRaceCheckpoint(4, new Vector3(4958.37158f,  -3812.41357f,4.319078f)),
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
                new VehicleRaceStartingPosition(0, new Vector3(7515.293f,   -2918.27563f, 5.559616f), 359.4457f),
                new VehicleRaceStartingPosition(1, new Vector3(7511.292f,   -2918.23486f, 5.556045f), 359.0019f),
                new VehicleRaceStartingPosition(2, new Vector3(7515.26367f, -2910.40137f, 5.563411f), 0.9221683f),
                new VehicleRaceStartingPosition(3, new Vector3(7511.2207f,  -2910.29517f, 5.555146f), 1.378026f),
                new VehicleRaceStartingPosition(4, new Vector3(7515.38965f, -2902.63477f, 5.559948f), 0.5928084f),
                new VehicleRaceStartingPosition(5, new Vector3(7511.26f,    -2902.69238f, 5.556325f), 0.7585424f),
                new VehicleRaceStartingPosition(6, new Vector3(7515.379f,   -2895.01025f, 5.559219f), 0.8882701f),
                new VehicleRaceStartingPosition(7, new Vector3(7511.336f,   -2895.00928f, 5.557793f), 0.5398135f),
                new VehicleRaceStartingPosition(8, new Vector3(7515.26758f, -2887.44629f, 5.561874f), 0.3054437f),
                new VehicleRaceStartingPosition(9, new Vector3(7511.36475f, -2887.43628f, 5.557805f), 359.5417f),
        };
        List<VehicleRaceCheckpoint> FIACheckpoints1 = new List<VehicleRaceCheckpoint>()
        {
                new VehicleRaceCheckpoint(0, new Vector3(7513.447f, -2763.41f, 5.230164f)),
                new VehicleRaceCheckpoint(1, new Vector3(7369.14f, -2767.69f, 5.339633f)),
                new VehicleRaceCheckpoint(2, new Vector3(7369.06f, -2870.122f, 5.282921f)),
                new VehicleRaceCheckpoint(3, new Vector3(7372.857f, -2991.981f, 5.446329f)),
                new VehicleRaceCheckpoint(4, new Vector3(7497.459f, -3033.752f, 5.43575f)),
                new VehicleRaceCheckpoint(5, new Vector3(7513.396f, -2984.252f, 5.241279f)),
                new VehicleRaceCheckpoint(6, new Vector3(7513.334f, -2879.528f, 5.341697f)),
        };
        VehicleRaceTrack fiahotring1 = new VehicleRaceTrack("fiahotring1", "FIA Hotring", "Francis International Oval", FIACheckpoints1, FIAStarting1);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(fiahotring1);

        List<VehicleRaceStartingPosition> FIAStarting2 = new List<VehicleRaceStartingPosition>()
        {
                new VehicleRaceStartingPosition(0, new Vector3(7515.293f,   -2918.27563f, 5.559616f), 359.4457f),
                new VehicleRaceStartingPosition(1, new Vector3(7511.292f,   -2918.23486f, 5.556045f), 359.0019f),
                new VehicleRaceStartingPosition(2, new Vector3(7515.26367f, -2910.40137f, 5.563411f), 0.9221683f),
                new VehicleRaceStartingPosition(3, new Vector3(7511.2207f,  -2910.29517f, 5.555146f), 1.378026f),
                new VehicleRaceStartingPosition(4, new Vector3(7515.38965f, -2902.63477f, 5.559948f), 0.5928084f),
                new VehicleRaceStartingPosition(5, new Vector3(7511.26f,    -2902.69238f, 5.556325f), 0.7585424f),
                new VehicleRaceStartingPosition(6, new Vector3(7515.379f,   -2895.01025f, 5.559219f), 0.8882701f),
                new VehicleRaceStartingPosition(7, new Vector3(7511.336f,   -2895.00928f, 5.557793f), 0.5398135f),
                new VehicleRaceStartingPosition(8, new Vector3(7515.26758f, -2887.44629f, 5.561874f), 0.3054437f),
                new VehicleRaceStartingPosition(9, new Vector3(7511.36475f, -2887.43628f, 5.557805f), 359.5417f),
        };
        List<VehicleRaceCheckpoint> FIACheckpoints2 = new List<VehicleRaceCheckpoint>()
        {
                new VehicleRaceCheckpoint(0, new Vector3(7513.447f, -2763.41f, 5.230164f)),
                new VehicleRaceCheckpoint(1, new Vector3(7369.14f, -2767.69f, 5.339633f)),
                new VehicleRaceCheckpoint(2, new Vector3(6913.1626f, -2972.21582f, 7.018955f)),
                new VehicleRaceCheckpoint(3, new Vector3(6440.697f,  -2967.73022f, 30.0528927f)),
                new VehicleRaceCheckpoint(4, new Vector3(6182.147f,  -2989.09058f, 47.76625f)),
                new VehicleRaceCheckpoint(5, new Vector3(5676.947f,  -2984.14014f, 43.956955f)),
                new VehicleRaceCheckpoint(6, new Vector3(4983.53174f, -2994.88965f, 14.3914824f)),

        };
        VehicleRaceTrack fiastarjunction = new VehicleRaceTrack("fiastarjunction", "FIA Star Junction", "FIA to Star Junction", FIACheckpoints2, FIAStarting2);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(fiastarjunction);
    }
    private void DukesTracks()
    {
        List<VehicleRaceStartingPosition> DukesHighwayStarting1 = new List<VehicleRaceStartingPosition>()
        {
                new VehicleRaceStartingPosition(0, new Vector3(6550.248f,   -2384.8252f,  13.205533f),  324.8135f),
                new VehicleRaceStartingPosition(1, new Vector3(6545.67969f, -2381.84033f, 13.2413425f), 324.9805f),
                new VehicleRaceStartingPosition(2, new Vector3(6552.864f,   -2372.644f,   13.2509327f), 323.1328f),
                new VehicleRaceStartingPosition(3, new Vector3(6556.906f,   -2375.23413f, 13.2010422f), 324.2783f),
                new VehicleRaceStartingPosition(4, new Vector3(6563.572f,   -2366.61035f, 13.2164621f), 319.7684f),
                new VehicleRaceStartingPosition(5, new Vector3(6559.921f,   -2363.58521f, 13.270463f), 315.8207f),
        };
        List<VehicleRaceCheckpoint> DukesHighwayCheckpoints1 = new List<VehicleRaceCheckpoint>()
        {
                new VehicleRaceCheckpoint(0, new Vector3(6557.353f, -2371.844f, 13.21317f)),
                new VehicleRaceCheckpoint(1, new Vector3(6717.319f, -2271.487f, 13.35466f)),
                new VehicleRaceCheckpoint(2, new Vector3(6829.653f, -2291.332f, 14.18538f)),
                new VehicleRaceCheckpoint(3, new Vector3(7042.986f, -2340.147f, 23.50827f)),
                new VehicleRaceCheckpoint(4, new Vector3(7120.486f, -2387.482f, 12.51296f)),
                new VehicleRaceCheckpoint(5, new Vector3(7224.004f, -2550.771f, 12.6413f)),
                new VehicleRaceCheckpoint(6, new Vector3(7261.188f, -2916.162f, 19.26021f)),
                new VehicleRaceCheckpoint(7, new Vector3(7209.49f, -3080.412f, 17.9143f)),
                new VehicleRaceCheckpoint(8, new Vector3(7023.65f, -3140.889f, 16.99864f)),


        };
        VehicleRaceTrack dukeshighway1 = new VehicleRaceTrack("dukeshighway1", "Dukes Expressway", "Dukes Expressway Sprint", DukesHighwayCheckpoints1, DukesHighwayStarting1);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(dukeshighway1);

        List<VehicleRaceStartingPosition> DukesBoulevardStarting = new List<VehicleRaceStartingPosition>()
        {
                new VehicleRaceStartingPosition(0, new Vector3(6589.509f,   -2314.07227f, 13.368803f), 138.2494f),
                new VehicleRaceStartingPosition(1, new Vector3(6586.318f,   -2311.24316f, 13.3649626f),138.578f),
                new VehicleRaceStartingPosition(2, new Vector3(6581.5625f,  -2323.05029f, 13.3652029f),138.023f),
                new VehicleRaceStartingPosition(3, new Vector3(6578.315f,   -2320.06934f, 13.3634529f),138.4367f),
                new VehicleRaceStartingPosition(4, new Vector3(6573.64648f, -2332.12622f, 13.3673925f),141.1688f),
                new VehicleRaceStartingPosition(5, new Vector3(6570.4707f,  -2329.17627f, 13.3496828f),140.7071f),
        };
        List<VehicleRaceCheckpoint> DukesBoulevardCheckpoints = new List<VehicleRaceCheckpoint>()
        {
                new VehicleRaceCheckpoint(0, new Vector3(6501.38965f, -2447.32031f, 28.0338135f)),
                new VehicleRaceCheckpoint(1, new Vector3(6337.05859f, -2766.406f,   28.4491234f)),
                new VehicleRaceCheckpoint(2, new Vector3(6280.333f,   -3008.29541f, 30.6902637f)),
                new VehicleRaceCheckpoint(3, new Vector3(6280.362f,   -3271.35f,    33.4019623f)),
                new VehicleRaceCheckpoint(4, new Vector3(6299.529f,   -3353.867f,   35.61914f)),
                new VehicleRaceCheckpoint(5, new Vector3(6292.51367f, -3217.946f,   33.4179726f)),
                new VehicleRaceCheckpoint(6, new Vector3(6293.11f,    -2960.39331f, 30.3715935f)),
                new VehicleRaceCheckpoint(7, new Vector3(6410.359f,   -2659.289f,   37.4652519f)),
                new VehicleRaceCheckpoint(8, new Vector3(6563.51563f, -2363.86621f, 13.240303f)),
        };
        VehicleRaceTrack dukesboule1 = new VehicleRaceTrack("dukesboule1", "Dukes Boulevard", "Dukes Boulevard Blast", DukesBoulevardCheckpoints, DukesBoulevardStarting);
        VehicleRaceTypeManager.VehicleRaceTracks.Add(dukesboule1);
    }
}

