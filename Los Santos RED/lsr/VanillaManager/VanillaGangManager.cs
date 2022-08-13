using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
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
    private List<int> GangHashes = new List<int>() {


        653210662//MexGoon01GMY
        ,832784782//MexGoon02GMY
        ,-1773333796 //MexGoon03GMY
        ,-1109568186//MexGang01GMY
        ,1226102803//MexBoss02GMM
        ,1466037421//MexBoss01GMM
        ,1329576454//PoloGoon01GMY
        ,-1561829034//PoloGoon02GMY
        ,-1872961334//SalvaBoss01GMY
        ,663522487//SalvaGoon01GMY
        ,846439045//SalvaGoon02GMY
        ,62440720//SalvaGoon03GMY
        ,-48477765//StrPunk01GMY
        ,228715206//StrPunk02GMY
        ,-236444766//ArmBoss01GMM
        ,-39239064//ArmGoon01GMM
        ,-984709238//ArmGoon02GMY
        ,-412008429//ArmLieut01GMM
        ,1752208920//Azteca01GMY
        ,-198252413//BallaEast01GMY
        ,588969535//BallaOrig01GMY
        ,361513884//Ballas01GFY
        ,-1492432238//BallasOG
        ,599294057//BallaSout01GMY
        ,-1176698112//ChiBoss01GMM
        ,2119136831//ChiGoon01GMM
        ,-9308122//ChiGoon02GMM
        ,-398748745//FamCA01GMY
        ,-613248456//FamDNF01GMY
        ,-2077218039//FamFor01GMY
        ,1309468115//Families01GFY
        ,891945583//KorBoss01GMM
        ,611648169//Korean01GMY
        ,-1880237687//Korean02GMY
        ,2093736314//KorLieut01GMY
        ,1330042375//Lost01GMY
        ,1032073858//Lost02GMY
        ,850468060//Lost03GMY
        ,-44746786//Lost01GFY


        ,810804565//Mexthug01AMY
        ,0x3053E555//Mexthug01AMY (hash?)
        ,-605196176//Mexthug01AMY (hash?)



        ,1520708641//Vagos01GFY
        ,1370084608//Vagos01GFY
        ,0x5AA42C21//Vagos01GFY



        ,-1350144833//G_M_Y_BallaSout_01
        ,-912864237//G_M_Y_BallaEast_01
        ,-1142106906//A_M_M_Hillbilly_01



    };
    private List<uint> uGangHashes = new List<uint>() {

        1226102803//G_M_M_MexBoss_02
        ,0x4914D813//G_M_M_MexBoss_02

        ,1466037421//G_M_M_MexBoss_01
        ,0x5761F4AD//G_M_M_MexBoss_01



        ,3185399110//G_M_Y_MexGang_01
        ,0xBDDD5546//G_M_Y_MexGang_01




        ,810804565//a_m_y_mexthug_01
        ,0x3053E555//a_m_y_mexthug_01 (hash?)

        ,4096714883//G_M_Y_BallaEast_01
        ,0xF42EE883//G_M_Y_BallaEast_01

        ,361513884//G_F_Y_ballas_01
        ,0x158C439C//G_F_Y_ballas_01

        ,588969535//G_M_Y_BallaOrig_01
        ,0x231AF63F//G_M_Y_BallaOrig_01

        ,599294057//G_M_Y_BallaSout_01
        ,0x23B88069//G_M_Y_BallaSout_01

        ,1520708641//G_F_Y_Vagos_01
        ,0x5AA42C21//G_F_Y_Vagos_01
    };
    private IPlacesOfInterest PlacesOfInterest;
    private float ScenarioBlockingDistance = 200f; //goes in both directions so within 400 meters, no scenarios will be spawned
    private List<Vector3> GangBlockingAreas = new List<Vector3>();
    public VanillaGangManager(ISettingsProvideable settings, IPlacesOfInterest placesOfInterest)
    {
        Settings = settings;
        PlacesOfInterest = placesOfInterest;
        GangBlockingAreas = new List<Vector3>();
        GangBlockingAreas.Add(new Vector3(1193.61f, -1656.411f, 43.02641f));

        GangScenarios = new List<string>()
        {
                    "Chumash_14_Bikers".ToLower(),
        "Chumash_14_Bikers".ToLower(),
        "Chumash_14_Cops".ToLower(),
        "Chumash_14_Cops".ToLower(),
        "Chumash_14_Hookers".ToLower(),
        "Chumash_14_Hookers".ToLower(),
        "Chumash_14_Vagos".ToLower(),
        "Chumash_14_Vagos".ToLower(),
        "Del_Perro_16_Vagos".ToLower(),
        "Del_Perro_16_Vagos".ToLower(),
        "Del_Perro_16_Bikers".ToLower(),
        "Del_Perro_16_Bikers".ToLower(),
        "DowntownAlley_33_Vagos".ToLower(),
        "DowntownAlley_33_Vagos".ToLower(),
        "DowntownAlley_33_Bikers".ToLower(),
        "DowntownAlley_33_Bikers".ToLower(),
        "E_Canals_22_Vagos".ToLower(),
        "E_Cypress_05_Vagos".ToLower(),
        "E_Hollywood_39_Bikers".ToLower(),
        "E_Hollywood_39_Vagos".ToLower(),
        "E_PaletoBay_10_Bikers".ToLower(),
        "E_PaletoBay_10_Vagos".ToLower(),
        "E_Puerto_26_Bikers".ToLower(),
        "E_Puerto_26_Vagos".ToLower(),
        "E_SandyShores_12_Bikers".ToLower(),
        "E_SandyShores_12_Vagos".ToLower(),
        "E_Silverlake_40_Bikers".ToLower(),
        "E_Silverlake_40_Vagos".ToLower(),
        "EastLS_Chopshop_35_Bikers".ToLower(),
        "EastLS_Chopshop_35_Vagos".ToLower(),
        "EastLS_Skatepark_36_Bikers".ToLower(),
        "EastLS_Skatepark_36_Vagos".ToLower(),
        "Eclipse_32_Bikers".ToLower(),
        "Eclipse_32_Vagos".ToLower(),
        "ElBurro_Shed_38_Bikers".ToLower(),
        "ElBurro_Shed_38_Vagos".ToLower(),
        "ElBurro_Wreck_37_Bikers".ToLower(),
        "ElBurro_Wreck_37_Vagos".ToLower(),
        "Harmony_ChopShop_13_Bikers".ToLower(),
        "Harmony_ChopShop_13_Vagos".ToLower(),
        "LOST_BIKERS".ToLower(),
        "LOST_HANGOUT".ToLower(),
        "Mid_Seoul_24_Bikers".ToLower(),
        "Mid_Seoul_24_Vagos".ToLower(),
        "MirrorPark_41_Bikers".ToLower(),
        "MirrorPark_41_Vagos".ToLower(),
        "Morningwood_17_Bikers".ToLower(),
        "Morningwood_17_Vagos".ToLower(),
        "N_W_Hollywood_31_Bikers".ToLower(),
        "N_W_Hollywood_31_Vagos".ToLower(),
        "OceanHighway_15_Bikers".ToLower(),
        "OceanHighway_15_Vagos".ToLower(),
        "Pershing_04_Bikers".ToLower(),
        "Pershing_04_Vagos".ToLower(),
        "Pier_18_Bikers".ToLower(),
        "Pier_18_Vagos".ToLower(),
        "Racecourse_06_Bikers".ToLower(),
        "RaceCourse_06_Vagos".ToLower(),
        "S_SanPedro_28_Bikers".ToLower(),
        "S_SanPedro_28_Vagos".ToLower(),
        "S_Seoul_27_Bikers".ToLower(),
        "S_Seoul_27_Vagos".ToLower(),
        "SanPedroGarage_30_Bikers".ToLower(),
        "SanPedroGarage_30_Vagos".ToLower(),
        "Scrapyard_29_Bikers".ToLower(),
        "Scrapyard_29_Vagos".ToLower(),
        "SeoulPark_23_Bikers".ToLower(),
        "SeoulPark_23_Vagos".ToLower(),
        "StrawberryClub_34_Bikers".ToLower(),
        "StrawberryClub_34_Vagos".ToLower(),
        "VAGOS_HANGOUT".ToLower(),
        "Vespucci_20_Bikers".ToLower(),
        "Vespucci_20_Vagos".ToLower(),
        "VespucciBeach_19_Bikers".ToLower(),
        "VespucciBeach_19_Vagos".ToLower(),
        "W_Canals_21_Bikers".ToLower(),
        "W_Canals_21_Vagos".ToLower(),
        "W_PaletoBay_09_Bikers".ToLower(),
        "W_PaletoBay_09_Vagos".ToLower(),
        "W_Puerto_25_Bikers".ToLower(),
        "W_Puerto_25_Vagos".ToLower(),
        "W_SandyShores_11_Bikers".ToLower(),
        "W_SandyShores_11_Vagos".ToLower(),
    };
    }

    public void Setup()
    {
        SetGangScenarioBlocking(Settings.SettingsManager.VanillaSettings.BlockGangScenarios);
    }
    public void Dispose()
    {
        ActivateScenarioGangs();
        UnSupressVanillaGangPeds();
        SetGangScenarioBlocking(false);
    }
    public void Tick()
    {
        if(Settings.SettingsManager.VanillaSettings.BlockGangScenarios != isGangScenarioBlocked)
        {
            SetGangScenarioBlocking(Settings.SettingsManager.VanillaSettings.BlockGangScenarios);
        }
        if (Settings.SettingsManager.VanillaSettings.SuppressVanillaGangPeds)
        {
            SupressVanillaGangPeds();
        }
        else
        {
            UnSupressVanillaGangPeds();
        }
    }
    public void SupressVanillaGangPeds()
    {
        IsVanillaGangPedsSupressed = true;
        SetGangPedsSupression(true);
    }
    public void UnSupressVanillaGangPeds()
    {
        if (IsVanillaGangPedsSupressed)
        {
            IsVanillaGangPedsSupressed = false;
            SetGangPedsSupression(false);
        }
    }
    private void SetGangPedsSupression(bool isSuppressed)
    {
        foreach (int gangHash in GangHashes)
        {
            NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(gangHash, isSuppressed);
        }
    }
    public void ActivateScenarioGangs()
    {
        if (!IsVanillaScenarioGangsActive)
        {
            IsVanillaScenarioGangsActive = true;
            SetGangScenarios(true);
        }
    }
    public void TerminateScenarioGangs()
    {
        IsVanillaScenarioGangsActive = false;
        SetGangScenarios(false);
    }
    private void SetGangScenarios(bool Enabled)
    {
        foreach (string scenario in GangScenarios)
        {
            NativeFunction.Natives.SET_SCENARIO_TYPE_ENABLED(scenario, Enabled);
            NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED(scenario, Enabled);
        }
    }
    private void SetGangScenarioBlocking(bool IsBlocked)
    {

        //new Vector3(-223.1647f, -1601.309f, 34.88379f) Near Familes Houses

        if (IsBlocked)
        {
            foreach(GangDen gangDen in PlacesOfInterest.PossibleLocations.GangDens)
            {
                NativeFunction.Natives.ADD_SCENARIO_BLOCKING_AREA<int>(gangDen.EntrancePosition.X - ScenarioBlockingDistance, gangDen.EntrancePosition.Y - ScenarioBlockingDistance, gangDen.EntrancePosition.Z - ScenarioBlockingDistance, gangDen.EntrancePosition.X + ScenarioBlockingDistance, gangDen.EntrancePosition.Y + ScenarioBlockingDistance, gangDen.EntrancePosition.Z + ScenarioBlockingDistance, false, true, true, true);
            }
            foreach(Vector3 areas in GangBlockingAreas)
            {
                NativeFunction.Natives.ADD_SCENARIO_BLOCKING_AREA<int>(areas.X - ScenarioBlockingDistance, areas.Y - ScenarioBlockingDistance, areas.Z - ScenarioBlockingDistance, areas.X + ScenarioBlockingDistance, areas.Y + ScenarioBlockingDistance, areas.Z + ScenarioBlockingDistance, false, true, true, true);
            }


            //NativeFunction.Natives.ADD_SCENARIO_BLOCKING_AREA<int>(-323.1647f, -1701.309f, -34.88379f, -123.1647f, -1501.309f, 134.88379f, false, true, true, true);
        }
        else
        {
            NativeFunction.Natives.REMOVE_SCENARIO_BLOCKING_AREAS();
        }
    }
}

