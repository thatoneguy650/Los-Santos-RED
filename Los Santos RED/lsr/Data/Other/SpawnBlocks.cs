using LosSantosRED.lsr.Helper;
using NAudio.CoreAudioApi.Interfaces;
using Rage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SpawnBlocks : ISpawnBlocks
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\SpawnBlocks.xml";
    public SpawnBlocks()
    {
        PossibleSpawnBlocks = new PossibleSpawnBlocks();
    }
    public PossibleSpawnBlocks PossibleSpawnBlocks { get; private set; }
    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("SpawnBlocks*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Spawn Blocks config: {ConfigFile.FullName}", 0);
            PossibleSpawnBlocks = Serialization.DeserializeParam<PossibleSpawnBlocks>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Spawn Blocks config  {ConfigFileName}", 0);
            PossibleSpawnBlocks = Serialization.DeserializeParam<PossibleSpawnBlocks>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Spawn Blocks config found, creating default", 0);
            DefaultConfig_LibertyCity();
            DefaultConfig_SunshineDream();
            DefaultConfig();
        }
    }
    private void DefaultConfig_LibertyCity()
    {
        PossibleSpawnBlocks spawnBlocksLC = new PossibleSpawnBlocks();
        Serialization.SerializeParam(spawnBlocksLC, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LibertyConfigFolder}\\SpawnBlocks_{StaticStrings.LibertyConfigSuffix}.xml");
        Serialization.SerializeParam(spawnBlocksLC, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LPPConfigFolder}\\SpawnBlocks_{StaticStrings.LPPConfigSuffix}.xml");
    }
    private void DefaultConfig_SunshineDream()
    {
        PossibleSpawnBlocks spawnBlocksLC = new PossibleSpawnBlocks();
        Serialization.SerializeParam(spawnBlocksLC, "Plugins\\LosSantosRED\\AlternateConfigs\\SunshineDream\\SpawnBlocks_SunshineDream.xml");
    }
    public void DefaultConfig()
    {
        CarGenerators();
        Scenarios();
        Serialization.SerializeParam(PossibleSpawnBlocks, ConfigFileName);
    }
    private void CarGenerators()
    {
        PossibleSpawnBlocks.CarGeneratorBlock.AddRange(new List<CarGeneratorBlock>()
        {
            new CarGeneratorBlock(new Vector3(407.7554f, -984.2084f, 29.89806f),"MissionRowPolice"),
            new CarGeneratorBlock(new Vector3(408.0923f, -988.8488f, 29.85523f),"MissionRowPolice"),
            new CarGeneratorBlock(new Vector3(407.9883f, -998.3094f, 29.81112f),"MissionRowPolice"),
            new CarGeneratorBlock(new Vector3(427.5909f, -1027.707f, 29.22805f),"MissionRowPolice"),
            new CarGeneratorBlock(new Vector3(434.9848f, -1027.103f, 29.12844f),"MissionRowPolice"),
            new CarGeneratorBlock(new Vector3(442.5143f, -1026.687f, 28.98147f),"MissionRowPolice"),
            new CarGeneratorBlock(new Vector3(446.3985f, -1026.087f, 28.92508f),"MissionRowPolice"),
            new CarGeneratorBlock(new Vector3(496.3687f, -996.015f, 28.3387f),"MissionRowPolice"),
            new CarGeneratorBlock(new Vector3(496.3721f, -1016.798f, 28.687f),"MissionRowPolice"),
            new CarGeneratorBlock(new Vector3(388.9854f, -1612.977f, 29.21355f),"DavisPolice"),
            new CarGeneratorBlock(new Vector3(392.7548f, -1608.376f, 29.21355f),"DavisPolice"),
            new CarGeneratorBlock(new Vector3(399.393f, -1621.396f, 29.20119f),"DavisPolice"),
            new CarGeneratorBlock(new Vector3(351.3006f, -1556.711f, 29.24393f),"DavisPolice"),
            new CarGeneratorBlock(new Vector3(-1625.343f, -1013.629f, 13.89048f),"DelPerroPierPolice"),
            new CarGeneratorBlock(new Vector3(-1072.822f, -880.3561f, 4.809089f),"VespucciPolice"),
            new CarGeneratorBlock(new Vector3(-1075.95f, -882.3492f, 4.809089f),"VespucciPolice"),
            new CarGeneratorBlock(new Vector3(-1051.726f, -867.1277f, 4.809089f),"VespucciPolice"),
            new CarGeneratorBlock(new Vector3(-1045.53f, -861.5321f, 4.809089f),"VespucciPolice"),
            new CarGeneratorBlock(new Vector3(-1042.226f, -857.9979f, 4.809089f),"VespucciPolice"),
            new CarGeneratorBlock(new Vector3(-1047.814f, -846.7044f, 4.809089f),"VespucciPolice"),
            new CarGeneratorBlock(new Vector3(-1052.352f, -846.8544f, 4.809089f),"VespucciPolice"),
            new CarGeneratorBlock(new Vector3(-1055.076f, -849.5068f, 4.809089f),"VespucciPolice"),
            new CarGeneratorBlock(new Vector3(-1058.752f, -851.4465f, 4.809089f),"VespucciPolice"),
            new CarGeneratorBlock(new Vector3(-1126.5f, -864.8307f, 13.63185f),"VespucciPolice"),
            new CarGeneratorBlock(new Vector3(-1122.896f, -863.4746f, 13.6122f),"VespucciPolice"),
            new CarGeneratorBlock(new Vector3(-1115.937f, -857.7859f, 13.65187f),"VespucciPolice"),
            new CarGeneratorBlock(new Vector3(-1138.613f, -845.7916f, 13.98058f),"VespucciPolice"),
            new CarGeneratorBlock(new Vector3(-574.256f, -168.7862f, 38.49216f),"RichmanPolice"),
            new CarGeneratorBlock(new Vector3(-557.7739f, -161.9565f, 38.62593f),"RichmanPolice"),
            new CarGeneratorBlock(new Vector3(-559.7783f, -147.1107f, 38.65532f),"RichmanPolice"),
            new CarGeneratorBlock(new Vector3(-555.6528f, -145.4567f, 38.72502f),"RichmanPolice"),
            new CarGeneratorBlock(new Vector3(-551.0397f, -144.0132f, 38.65663f),"RichmanPolice"),
            new CarGeneratorBlock(new Vector3(-454.4307f, 6040.472f, 31.17424f),"PaletoPolice"),
            new CarGeneratorBlock(new Vector3(-457.8087f, 6043.849f, 31.17424f),"PaletoPolice"),
            new CarGeneratorBlock(new Vector3(-468.1928f, 6038.506f, 31.17422f),"PaletoPolice"),
            new CarGeneratorBlock(new Vector3(-475.1396f, 6031.421f, 31.17419f),"PaletoPolice"),
            new CarGeneratorBlock(new Vector3(-458.788f, 6005.529f, 31.17422f),"PaletoPolice"),
            new CarGeneratorBlock(new Vector3(-455.154f, 6001.894f, 31.17422f),"PaletoPolice"),
            new CarGeneratorBlock(new Vector3(1831.629f, 3662.91f, 33.92607f),"SandyPolice"),
            new CarGeneratorBlock(new Vector3(1835.127f, 3664.892f, 33.92607f),"SandyPolice"),
            new CarGeneratorBlock(new Vector3(1847.131f, 3672.587f, 33.92607f),"SandyPolice"),
            new CarGeneratorBlock(new Vector3(666.4646f, -11.79645f, 82.76681f),"VinewoodPolice"),
            new CarGeneratorBlock(new Vector3(621.6934f, 26.27448f, 88.66011f),"VinewoodPolice"),
            new CarGeneratorBlock(new Vector3(615.3292f, 28.48364f, 89.28476f),"VinewoodPolice"),
            new CarGeneratorBlock(new Vector3(609.7616f, 30.84756f, 89.91243f),"VinewoodPolice"),
            new CarGeneratorBlock(new Vector3(597.8726f, 34.8121f, 91.07567f),"VinewoodPolice"),
            new CarGeneratorBlock(new Vector3(586.7384f, 37.78894f, 92.30818f),"VinewoodPolice"),
            new CarGeneratorBlock(new Vector3(580.8351f, 38.85468f, 92.82274f),"VinewoodPolice"),
            new CarGeneratorBlock(new Vector3(822.8635f, -1258.039f, 26.34347f),"LaMesaPolice"),
            new CarGeneratorBlock(new Vector3(828.0373f, -1258.039f, 26.34347f),"LaMesaPolice"),
            new CarGeneratorBlock(new Vector3(833.577f, -1258.954f, 26.34347f),"LaMesaPolice"),
            new CarGeneratorBlock(new Vector3(838.7137f, -1271.55f, 26.34347f),"LaMesaPolice"),
            new CarGeneratorBlock(new Vector3(833.4279f, -1271.55f, 26.34347f),"LaMesaPolice"),
            new CarGeneratorBlock(new Vector3(822.5837f, -1271.55f, 26.34347f),"LaMesaPolice"),
            new CarGeneratorBlock(new Vector3(828.1719f, -1333.792f, 26.18776f),"LaMesaPolice"),
            new CarGeneratorBlock(new Vector3(828.1719f, -1339.649f, 26.18776f),"LaMesaPolice"),
            new CarGeneratorBlock(new Vector3(828.1719f, -1345.815f, 26.18776f),"LaMesaPolice"),
            new CarGeneratorBlock(new Vector3(828.362f, -1351.482f, 26.21234f),"LaMesaPolice"),
            new CarGeneratorBlock(new Vector3(843.9627f, -1334.354f, 26.17253f),"LaMesaPolice"),
            new CarGeneratorBlock(new Vector3(843.874f, -1340.518f, 26.18776f),"LaMesaPolice"),
            new CarGeneratorBlock(new Vector3(844.3544f, -1346.3f, 26.21234f),"LaMesaPolice"),
            new CarGeneratorBlock(new Vector3(843.7897f, -1352.283f, 26.21234f),"LaMesaPolice"),
            new CarGeneratorBlock(new Vector3(865.7624f, -1378.407f, 26.21234f),"LaMesaPolice"),
            new CarGeneratorBlock(new Vector3(862.8425f, -1383.55f, 26.21234f),"LaMesaPolice"),
            new CarGeneratorBlock(new Vector3(859.8381f, -1388.58f, 26.21234f),"LaMesaPolice"),
            new CarGeneratorBlock(new Vector3(857.2202f, -1393.802f, 26.21234f),"LaMesaPolice"),
            new CarGeneratorBlock(new Vector3(854.2248f, -1398.952f, 26.21234f),"LaMesaPolice"),

            new CarGeneratorBlock(new Vector3(1855.314f, 2578.854f, 46.42464f),"BoilingbrokePrison"),
            new CarGeneratorBlock(new Vector3(1799.826f, 2600.908f, 45.58898f),"BoilingbrokePrison"),



            //
            new CarGeneratorBlock(new Vector3(-178.2882f, -1607.417f, 33.43019f),"familesgang1"),
        });
    }
    private void Scenarios()
    {
        PossibleSpawnBlocks.ScenarioBlocks.AddRange(new List<ScenarioBlock>()
        {
            new ScenarioBlock(new Vector3(838.9286f, -1259.166f, 25.38143f),"GeneralPolice"),
            new ScenarioBlock(new Vector3(837.0612f, -1257.972f, 25.36833f),"GeneralPolice"),
            new ScenarioBlock(new Vector3(824.9189f, -1263.116f, 25.26017f),"GeneralPolice"),
            new ScenarioBlock(new Vector3(455.6195f, -1026.146f, 27.47295f),"GeneralPolice"),
            new ScenarioBlock(new Vector3(438.6183f, -1026.667f, 27.7867f),"GeneralPolice"),
            new ScenarioBlock(new Vector3(407.7591f, -1005.385f, 28.26613f),"GeneralPolice"),
            new ScenarioBlock(new Vector3(407.9486f, -1008.231f, 28.26636f),"GeneralPolice"),
            new ScenarioBlock(new Vector3(-562.8094f, -148.4117f, 37.01614f),"GeneralPolice"),
            new ScenarioBlock(new Vector3(-548.7076f, -141.6616f, 37.29326f),"GeneralPolice"),
            new ScenarioBlock(new Vector3(-1039.343f, -855.5767f, 3.875723f),"GeneralPolice"),
            new ScenarioBlock(new Vector3(-1048.334f, -864.5882f, 3.992599f),"GeneralPolice"),
            new ScenarioBlock(new Vector3(-1069.269f, -878.6844f, 3.855752f),"GeneralPolice"),
            new ScenarioBlock(new Vector3(-1128.474f, -866.9715f, 12.51155f),"GeneralPolice"),
            new ScenarioBlock(new Vector3(-1120.375f, -858.11f, 12.52164f),"GeneralPolice"),
            new ScenarioBlock(new Vector3(-1112.189f, -855.1146f, 12.53287f),"GeneralPolice"),
            new ScenarioBlock(new Vector3(-1125.52f, -816.9467f, 14.99161f),"GeneralPolice"),
            new ScenarioBlock(new Vector3(-1104.038f, -797.9f, 17.30989f),"GeneralPolice"),
            new ScenarioBlock(new Vector3(-1112.705f, -804.9168f, 16.46241f),"GeneralPolice"),
            new ScenarioBlock(new Vector3(1868.631f, 3685.344f, 32.75556f),"GeneralPolice"),
            new ScenarioBlock(new Vector3(1866.067f, 3683.386f, 32.72648f),"GeneralPolice"),
            new ScenarioBlock(new Vector3(1862.838f, 3682.098f, 32.71787f),"GeneralPolice"),
            new ScenarioBlock(new Vector3(1853.62f, 3675.808f, 32.75896f),"GeneralPolice"),
            new ScenarioBlock(new Vector3(-482.731f, 6024.876f, 30.34054f),"GeneralPolice"),
            new ScenarioBlock(new Vector3(-479.5346f, 6028.117f, 30.34054f),"GeneralPolice"),
            new ScenarioBlock(new Vector3(-437.7312f, 6087.632f, 30.42623f),"GeneralPolice"),
            new ScenarioBlock(new Vector3(-431.8985f, 6032.511f, 30.34053f),"GeneralPolice"),

            new ScenarioBlock(new Vector3(374.2932f, 795.6959f, 187.5605f),"RangeStationPolice"),


            new ScenarioBlock(new Vector3(-256.2089f, -2029.314f, 28.94582f),"MazeBankSecurity"),
            new ScenarioBlock(new Vector3(-252.9283f, -2025.2f, 28.94582f),"MazeBankSecurity"),
            new ScenarioBlock(new Vector3(-241.3938f, -2023.062f, 28.94582f),"MazeBankSecurity"),
            new ScenarioBlock(new Vector3(-285.3585f, -2056.294f, 28.94583f),"MazeBankSecurity"),
            new ScenarioBlock(new Vector3(-336.9773f, -2046.85f, 28.94642f),"MazeBankSecurity"),
            new ScenarioBlock(new Vector3(-372.0268f, -2018.85f, 28.94642f),"MazeBankSecurity"),
            new ScenarioBlock(new Vector3(-395.8358f, -1997.162f, 28.94642f),"MazeBankSecurity"),
            new ScenarioBlock(new Vector3(-402.0461f, -1905.517f, 28.94614f),"MazeBankSecurity"),
            new ScenarioBlock(new Vector3(-337.9023f, -1880.646f, 28.94614f),"MazeBankSecurity"),
            new ScenarioBlock(new Vector3(-309.1234f, -1891.761f, 28.94639f),"MazeBankSecurity"),
            new ScenarioBlock(new Vector3(-283.2107f, -1913.418f, 28.94639f),"MazeBankSecurity"),
            new ScenarioBlock(new Vector3(-277.8984f, -1918.732f, 28.94639f),"MazeBankSecurity"),
            new ScenarioBlock(new Vector3(-233.965f, -1973.487f, 28.94639f),"MazeBankSecurity"),

            new ScenarioBlock(new Vector3(-802.0435f, -225.7791f, 36.20076f),"LuxuryAutoSecurity"),


            new ScenarioBlock(new Vector3(1862.066f, 2593.06f, 45.67203f),"PrisonMain") { Distance = 50f },

            new ScenarioBlock(new Vector3(1803.225f, 2623.1f, 44.5028f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1727.828f, 2439.606f, 44.56514f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1799.826f, 2600.908f, 45.58898f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1803.225f, 2623.1f, 44.5028f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1784.646f, 2626.016f, 44.56556f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1727.828f, 2439.606f, 44.56514f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1885.322f, 2626.629f, 44.6721f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1846.473f, 2584.199f, 44.67195f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1848.088f, 2602.314f, 44.63724f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1848.083f, 2601.134f, 44.62651f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1847.456f, 2603.807f, 44.65017f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1955.374f, 2622.803f, 44.90855f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1787.849f, 2622.388f, 44.56556f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1774.152f, 2532.529f, 44.56549f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1899.234f, 2605.696f, 44.96621f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1847.575f, 2584.629f, 45.67206f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1846.473f, 2584.199f, 44.67195f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1847.933f, 2584.933f, 44.67241f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1845.851f, 2587.203f, 44.67231f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1850.889f, 2584.074f, 44.67213f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1851.738f, 2598.987f, 44.67213f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1850.146f, 2599.688f, 44.648f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1848.083f, 2601.134f, 44.62651f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1848.088f, 2602.314f, 44.63724f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1847.456f, 2603.807f, 44.65017f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1850.018f, 2603.395f, 44.64648f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1852.137f, 2616.886f, 44.67213f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1955.374f, 2622.803f, 44.90855f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1899.234f, 2605.696f, 44.96621f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1830.171f, 2602.584f, 44.88912f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1815.909f, 2604.376f, 44.65796f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1814.352f, 2604.741f, 44.66116f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1813.006f, 2605.019f, 44.66391f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1787.849f, 2622.388f, 44.56556f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1751.283f, 2563.569f, 54.44793f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1737.642f, 2561.494f, 54.43274f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1737.16f, 2562.172f, 54.43274f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1751.283f, 2563.569f, 54.44793f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1733.784f, 2563.54f, 54.44675f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1722.093f, 2563.476f, 54.44793f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1701.468f, 2564.186f, 54.43274f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1674.055f, 2564.076f, 54.4313f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1662.51f, 2568.876f, 54.44578f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1611.629f, 2570.046f, 54.43207f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1606.765f, 2543.014f, 54.43266f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1611.519f, 2537.367f, 54.43208f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1621.769f, 2511.902f, 54.38113f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1621.278f, 2511.366f, 54.43208f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1639.437f, 2490.519f, 54.40924f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1646.959f, 2490.754f, 54.43208f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1676.462f, 2482.589f, 54.40998f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1703.719f, 2477.959f, 54.3774f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1709.409f, 2482.831f, 54.40908f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1733.246f, 2505.29f, 54.39078f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1740.667f, 2504.186f, 54.35909f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1759.357f, 2514.872f, 54.3591f),"PrisonGeneral"),
            new ScenarioBlock(new Vector3(1761.915f, 2521.84f, 54.39047f),"PrisonGeneral"),

            //Korean/Triad Steps
            new ScenarioBlock(new Vector3(-755.6989f, -920.1201f, 19.07203f), "KoreanSteps") { Distance = 10.0f },
            new ScenarioBlock(new Vector3(-762.5551f, -919.5652f, 20.15879f), "KoreanSteps") { Distance = 10.0f },
            new ScenarioBlock(new Vector3(-764.7227f, -917.3176f, 20.80979f), "KoreanSteps") { Distance = 10.0f },
            new ScenarioBlock(new Vector3(-764.3928f, -923.0778f, 18.72722f), "KoreanSteps") { Distance = 10.0f },
            new ScenarioBlock(new Vector3(-763.5527f, -932.0254f, 18.14868f), "KoreanSteps") { Distance = 10.0f },


            //Cartel Mansion
            new ScenarioBlock(new Vector3(1382.979f, 1147.941f, 113.3342f),"VagosCartel") { Distance = 10.0f },
            new ScenarioBlock(new Vector3(1382.747f, 1146.689f, 113.3342f),"VagosCartel") { Distance = 10.0f },
            new ScenarioBlock(new Vector3(1392.486f, 1154.698f, 113.4431f),"VagosCartel") { Distance = 10.0f },
            new ScenarioBlock(new Vector3(1392.104f, 1155.521f, 113.4431f),"VagosCartel") { Distance = 10.0f },
            new ScenarioBlock(new Vector3(1391.452f, 1157.046f, 113.4431f),"VagosCartel") { Distance = 10.0f },
            new ScenarioBlock(new Vector3(1390.901f, 1157.986f, 113.4431f),"VagosCartel") { Distance = 10.0f },
            new ScenarioBlock(new Vector3(1387.192f, 1158.324f, 113.3342f),"VagosCartel") { Distance = 10.0f },
            new ScenarioBlock(new Vector3(1413.152f, 1153.463f, 113.3341f),"VagosCartel") { Distance = 10.0f },
            new ScenarioBlock(new Vector3(1419.978f, 1151.363f, 113.6739f),"VagosCartel") { Distance = 10.0f },
            new ScenarioBlock(new Vector3(1416.658f, 1163.833f, 114.3532f),"VagosCartel") { Distance = 10.0f },
        });
    }
}

