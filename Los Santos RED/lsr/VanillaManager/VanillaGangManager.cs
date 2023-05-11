using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

public class VanillaGangManager
{
    private bool isGangScenarioBlocked = false;
    private bool IsVanillaScenarioGangsActive = true;
    private ISettingsProvideable Settings;
    private List<string> GangScenarios;
    private bool IsVanillaGangPedsSupressed;
    private uint GameTimeLastStoppedGang;

    private IPlacesOfInterest PlacesOfInterest;
    private List<Vector3> LargeGangBlockingAreas;
    private float SmallScenarioDistance => 2f;
    private float ScenarioBlockingDistance => Settings.SettingsManager.VanillaSettings.BlockGangScenariosAroundDensDistance; //goes in both directions so within 400 meters, no scenarios will be spawned
    private List<Vector3> GangBlockingAreas = new List<Vector3>();
    public VanillaGangManager(ISettingsProvideable settings, IPlacesOfInterest placesOfInterest)
    {
        Settings = settings;
        PlacesOfInterest = placesOfInterest;
        LargeGangBlockingAreas = new List<Vector3>();
        LargeGangBlockingAreas.AddRange(new List<Vector3>() { 
            new Vector3(1193.61f, -1656.411f, 43.02641f),
            new Vector3(361.514f, -2016.978f, 21.33359f),
        });
        GangBlockingAreas = new List<Vector3>();
        GangBlockingAreas.AddRange(new List<Vector3>() { 
            new Vector3(392.4433f, -1994.735f, 22.54048f),//Vagos
            new Vector3(316.9356f, -1990.046f, 21.26461f),
            new Vector3(309.5228f, -1997.361f, 20.25304f),
            new Vector3(307.9317f, -2002.374f, 19.88714f),
            new Vector3(305.6269f, -2004.964f, 19.5512f),
            new Vector3(305.8982f, -2006.304f, 19.50441f),
            new Vector3(304.9736f, -2007.407f, 19.39462f),
            new Vector3(305.5439f, -2008.695f, 19.41691f),
            new Vector3(304.5901f, -2004.278f, 19.50809f),
            new Vector3(305.6269f, -2004.964f, 19.5512f),
            new Vector3(310.3595f, -2005.16f, 19.83792f),
            new Vector3(295.7217f, -2015.289f, 18.88056f),
            new Vector3(293.6593f, -2017.885f, 18.79945f),
            new Vector3(292.6368f, -2017.716f, 18.74973f),
            new Vector3(293.0543f, -2019.038f, 18.80048f),
            new Vector3(292.0335f, -2018.452f, 18.80031f),
            new Vector3(279.4013f, -2035.138f, 17.64792f),
            new Vector3(265.0269f, -2052.744f, 16.58372f),
            new Vector3(265.2896f, -2052.073f, 16.60909f),
            new Vector3(265.7442f, -2051.099f, 16.64469f),
            new Vector3(282.2816f, -2055.373f, 17.83146f),
            new Vector3(282.8216f, -2056.286f, 17.7879f),
            new Vector3(282.1391f, -2057.283f, 17.71234f),
            new Vector3(282.8859f, -2057.587f, 17.69218f),
            new Vector3(286.6902f, -2053.277f, 18.4479f),
            new Vector3(288.2463f, -2053.168f, 18.07687f),
            new Vector3(280.4756f, -2062.61f, 16.57709f),
            new Vector3(281.144f, -2063.059f, 16.63224f),
            new Vector3(281.0872f, -2064.012f, 16.49914f),
            new Vector3(281.924f, -2066.268f, 16.55604f),
            new Vector3(280.873f, -2066.568f, 16.43075f),
            new Vector3(297.1865f, -2057.775f, 17.75588f),
            new Vector3(297.9219f, -2057.149f, 17.80005f),
            new Vector3(298.5323f, -2057.77f, 17.80304f),
            new Vector3(297.9274f, -2058.548f, 17.73769f),
            new Vector3(301.8858f, -2061.076f, 17.28592f),
            new Vector3(302.3918f, -2062.077f, 17.28612f),
            new Vector3(303.5901f, -2062.876f, 17.25527f),
            new Vector3(303.0811f, -2062.351f, 17.27842f),
            new Vector3(298.1942f, -2067.917f, 16.73282f),
            new Vector3(312.2584f, -2056.027f, 19.30214f),
            new Vector3(312.1742f, -2054.6f, 19.70512f),
            new Vector3(324.1629f, -2063.822f, 19.70484f),
            new Vector3(330.5526f, -2071.241f, 19.24667f),
            new Vector3(331.858f, -2071.095f, 19.70353f),
            new Vector3(331.3872f, -2072.749f, 19.06667f),
            new Vector3(324.1631f, -2089.291f, 16.76774f),
            new Vector3(325.0252f, -2090.036f, 16.75523f),
            new Vector3(325.7984f, -2091.121f, 16.75485f),
            new Vector3(326.9599f, -2091.274f, 16.77712f),
            new Vector3(326.5845f, -2090.346f, 16.78034f),
            new Vector3(327.7125f, -2090.273f, 16.80989f),
            new Vector3(326.765f, -2089.42f, 16.7969f),
            new Vector3(321.0186f, -2100.583f, 17.24894f),
            new Vector3(319.8471f, -2101.021f, 17.24894f),
            new Vector3(306.6964f, -2100.499f, 16.52534f),
            new Vector3(370.6535f, -2075.239f, 20.76009f),
            new Vector3(397.5355f, -2010.16f, 22.3088f),
            new Vector3(390.6531f, -2004.877f, 22.59427f),
            new Vector3(390.679f, -2003.343f, 22.56685f),
            new Vector3(392.4433f, -1994.735f, 22.54048f),
            new Vector3(393.2065f, -1995.493f, 22.50089f),
            new Vector3(393.758f, -1994.536f, 22.50253f),
            new Vector3(393.758f, -1994.536f, 22.50253f),
            new Vector3(357.5214f, -1981.13f, 23.31644f),
            new Vector3(356.761f, -1979.089f, 23.32077f),
            new Vector3(355.1402f, -1979.272f, 23.32718f),
            new Vector3(354.8138f, -1977.691f, 23.37239f),
            new Vector3(343.3667f, -1960.416f, 23.49348f),
            new Vector3(341.7528f, -1959.559f, 23.48692f),
            new Vector3(339.9916f, -1961.184f, 23.49368f),
            new Vector3(341.9448f, -1961.606f, 23.51126f),
            new Vector3(340.8537f, -1962.1f, 23.51357f),
            new Vector3(342.5603f, -1962.538f, 23.50275f),
            new Vector3(341.4838f, -1963.491f, 23.50401f),
            new Vector3(349.1188f, -1951.431f, 23.41994f),
            new Vector3(349.886f, -1950.729f, 23.4564f),
            new Vector3(351.2018f, -1949.78f, 23.47158f),
            new Vector3(351.776f, -1949.087f, 23.42973f),
            new Vector3(326.0322f, -1955.318f, 23.39419f),
            new Vector3(323.2572f, -1952.835f, 23.5227f),
            new Vector3(323.0382f, -1951.968f, 23.56685f),
            new Vector3(422.6576f, -2026.561f, 22.12029f),
            new Vector3(423.8715f, -2025.57f, 22.04238f),
            new Vector3(423.7046f, -2024.244f, 22.05896f),
            new Vector3(423.0401f, -2021.845f, 22.07076f),
            new Vector3(423.7558f, -2026.33f, 23.61898f),
            new Vector3(457.7604f, -2057.756f, 23.09805f),
            new Vector3(458.7604f, -2057.802f, 23.14327f),
            new Vector3(458.3245f, -2058.571f, 23.0685f),
            new Vector3(458.9694f, -2061.615f, 22.92887f),
            new Vector3(457.8723f, -2062.138f, 22.86517f),
            new Vector3(455.6201f, -2063.873f, 22.62046f),
            new Vector3(455.1259f, -2064.674f, 22.56774f),
            new Vector3(385.5409f, -1892.321f, 24.15725f),
            new Vector3(384.1115f, -1892.948f, 24.12367f),
            new Vector3(384.1791f, -1894.289f, 24.07714f),
            new Vector3(383.2886f, -1893.773f, 24.10502f),
            new Vector3(381.4803f, -1895.502f, 24.04661f),
            new Vector3(387.5752f, -1889.911f, 24.26877f),
            new Vector3(386.7265f, -1890.356f, 24.24751f),
            new Vector3(387.5752f, -1889.911f, 24.26877f),
            new Vector3(391.5532f, -1885.042f, 25.67244f),
            new Vector3(390.5208f, -1886.083f, 24.48816f),
            new Vector3(391.2015f, -1886.407f, 24.48876f),
            new Vector3(390.9937f, -1887.646f, 24.39243f),
            new Vector3(415.8138f, -1894.987f, 24.90992f),
            new Vector3(416.3756f, -1894.456f, 24.94747f),
            new Vector3(416.8386f, -1893.73f, 24.98651f),
            new Vector3(396.2281f, -1873.671f, 25.22363f),
            new Vector3(397.1217f, -1872.883f, 25.22164f),
            new Vector3(460.9929f, -1850.348f, 26.86265f),
            new Vector3(460.9394f, -1849.516f, 26.8489f),
            new Vector3(343.4177f, -1814.855f, 27.25144f),
            new Vector3(342.3925f, -1815.652f, 27.19483f),
            new Vector3(340.2542f, -1818.779f, 27.07004f),
        });
 
    }
    public void Setup()
    {
        if (isGangScenarioBlocked)
        {
            SetGangScenarioBlocking(Settings.SettingsManager.VanillaSettings.BlockGangScenarios);
        }
    }
    public void Dispose()
    {
        //ActivateScenarioGangs();
        //UnSupressVanillaGangPeds();
        if (isGangScenarioBlocked)
        {
            SetGangScenarioBlocking(false);
        }
    }
    public void Tick()
    {
        if(Settings.SettingsManager.VanillaSettings.BlockGangScenarios != isGangScenarioBlocked)
        {
            SetGangScenarioBlocking(Settings.SettingsManager.VanillaSettings.BlockGangScenarios);
        }
    }
    private void SetGangScenarioBlocking(bool IsBlocked)
    {
        if (IsBlocked)
        {
            foreach(GangDen gangDen in PlacesOfInterest.PossibleLocations.GangDens.Where(x=>x.HasVanillaGangSpawnedAroundToBeBlocked))
            {
                NativeFunction.Natives.ADD_SCENARIO_BLOCKING_AREA<int>(gangDen.EntrancePosition.X - ScenarioBlockingDistance, gangDen.EntrancePosition.Y - ScenarioBlockingDistance, gangDen.EntrancePosition.Z - ScenarioBlockingDistance, gangDen.EntrancePosition.X + ScenarioBlockingDistance, gangDen.EntrancePosition.Y + ScenarioBlockingDistance, gangDen.EntrancePosition.Z + ScenarioBlockingDistance, false, true, true, true);
            }
            foreach(Vector3 areas in LargeGangBlockingAreas)
            {
                NativeFunction.Natives.ADD_SCENARIO_BLOCKING_AREA<int>(areas.X - ScenarioBlockingDistance, areas.Y - ScenarioBlockingDistance, areas.Z - ScenarioBlockingDistance, areas.X + ScenarioBlockingDistance, areas.Y + ScenarioBlockingDistance, areas.Z + ScenarioBlockingDistance, false, true, true, true);
            }
            foreach (Vector3 areas in GangBlockingAreas)
            {
                NativeFunction.Natives.ADD_SCENARIO_BLOCKING_AREA<int>(areas.X - SmallScenarioDistance, areas.Y - SmallScenarioDistance, areas.Z - SmallScenarioDistance, areas.X + SmallScenarioDistance, areas.Y + SmallScenarioDistance, areas.Z + SmallScenarioDistance, false, true, true, true);
            }
        }
        else
        {
            NativeFunction.Natives.REMOVE_SCENARIO_BLOCKING_AREAS();
        }
        isGangScenarioBlocked = IsBlocked;
        //EntryPoint.WriteToConsoleTestLong("GANG SCENARIO BLOCK RAN");
    }
}

