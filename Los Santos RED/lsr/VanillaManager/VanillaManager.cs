using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


public class VanillaManager
{
    private bool IsVanillaRespawnActive = true;
    private bool IsVanillaDispatchActive = true;
    private bool IsVanillaScenarioCopsActive = true;

    private bool IsVanillaScenarioGangsActive = true;



    private uint GameTimeLastTerminatedVanillaDispatch;
    private ISettingsProvideable Settings;
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




    private bool IsTimeToTerminatedVanillaDispatch => GameTimeLastTerminatedVanillaDispatch == 0 || Game.GameTime - GameTimeLastTerminatedVanillaDispatch >= 5000;
    public VanillaManager(ISettingsProvideable settings)
    {
        Settings = settings;
    }
    public void Dispose()
    {
        ActivateRespawn();
        ActivateDispatch();
        ActivateScenarioCops();
        ActivateScenarioGangs();
        UnSupressVanillaGangPeds();
    }
    public void Tick()
    {
        if (Settings.SettingsManager.VanillaSettings.TerminateRespawn)
        {
            if (IsVanillaRespawnActive)
            {
                TerminateRespawnController();
            }
        }
        else if (!Settings.SettingsManager.VanillaSettings.TerminateRespawn)
        {
            if (!IsVanillaRespawnActive)
            {
                ActivateRespawn();
            }
        }
        if (Settings.SettingsManager.VanillaSettings.TerminateDispatch)
        {
            if (IsVanillaDispatchActive || IsTimeToTerminatedVanillaDispatch)
            {
                TerminateDispatch();
            }
        }
        else if (!Settings.SettingsManager.VanillaSettings.TerminateDispatch)
        {
            if (!IsVanillaDispatchActive)
            {
                ActivateDispatch();
            }
        }
        if (Settings.SettingsManager.VanillaSettings.TerminateScenarioCops)
        {
            //if (IsVanillaScenarioCopsActive)
            //{
                TerminateScenarioCops();
            //}
        }
        else if (!Settings.SettingsManager.VanillaSettings.TerminateScenarioCops)
        {
            if (!IsVanillaScenarioCopsActive)
            {
                ActivateScenarioCops();
            }
        }
        if (Settings.SettingsManager.VanillaSettings.TerminateRespawn)
        {
            TerminateRespawnScripts();
        }
        if (Settings.SettingsManager.VanillaSettings.TerminateScenarioGangs)
        {
            TerminateScenarioGangs();
        }
        else
        {
            ActivateScenarioGangs();
        }
        if (Settings.SettingsManager.VanillaSettings.SuppressVanillaGangPeds)
        {
            SupressVanillaGangPeds();
        }
        else
        {
            UnSupressVanillaGangPeds();
        }

        if (Settings.SettingsManager.VanillaSettings.TerminateHealthRecharge)
        {
            TerminateHealthRecharge();
        }
        TerminateAudio();   
    }
    public void SupressVanillaGangPeds()
    {
        IsVanillaGangPedsSupressed = true;
        //SetGangPedsSupression(true);
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
        
        foreach(int gangHash in GangHashes)
        {
            NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(gangHash, isSuppressed);
        }





        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("a_m_y_mexthug_01".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_mexgang_01".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_m_mexboss_02".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_m_mexboss_01".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_armgoon_02".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_m_armlieut_01".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_m_armgoon_01".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_m_armboss_01".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_korlieut_01".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_korean_02".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_korean_01".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_m_korboss_01".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_m_chigoon_02".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_m_chigoon_01".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_azteca_01".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_salvagoon_03".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_salvagoon_02".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_salvagoon_01".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_salvaboss_01".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_f_y_ballas_01".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_ballaorig_01".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_ballaeast_01".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_ballasout_01".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_f_y_families_01".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_famfor_01".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_famdnf_01".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_famca_01".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_f_y_vagos_01".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_mexgoon_03".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_mexgoon_02".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_mexgoon_01".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_f_y_lost_01".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_lost_03".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_lost_02".ToLower()), isSuppressed);
        //NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_lost_01".ToLower()), isSuppressed);
        //EntryPoint.WriteToConsole($"                                SET                 Vanilla GANG PEDS SUPPRESSION: {isSuppressed}");
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
        //SetGangScenarios(false);
    }
    private void SetGangScenarios(bool Enabled)
    {
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Chumash_14_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Chumash_14_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Chumash_14_Cops".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Chumash_14_Cops".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Chumash_14_Hookers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Chumash_14_Hookers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Chumash_14_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Chumash_14_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Del_Perro_16_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Del_Perro_16_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Del_Perro_16_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Del_Perro_16_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("DowntownAlley_33_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("DowntownAlley_33_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("DowntownAlley_33_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("DowntownAlley_33_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("E_Canals_22_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("E_Cypress_05_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("E_Hollywood_39_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("E_Hollywood_39_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("E_PaletoBay_10_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("E_PaletoBay_10_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("E_Puerto_26_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("E_Puerto_26_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("E_SandyShores_12_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("E_SandyShores_12_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("E_Silverlake_40_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("E_Silverlake_40_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("EastLS_Chopshop_35_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("EastLS_Chopshop_35_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("EastLS_Skatepark_36_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("EastLS_Skatepark_36_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Eclipse_32_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Eclipse_32_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("ElBurro_Shed_38_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("ElBurro_Shed_38_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("ElBurro_Wreck_37_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("ElBurro_Wreck_37_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Harmony_ChopShop_13_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Harmony_ChopShop_13_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("LOST_BIKERS".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("LOST_HANGOUT".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Mid_Seoul_24_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Mid_Seoul_24_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("MirrorPark_41_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("MirrorPark_41_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Morningwood_17_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Morningwood_17_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("N_W_Hollywood_31_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("N_W_Hollywood_31_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("OceanHighway_15_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("OceanHighway_15_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Pershing_04_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Pershing_04_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Pier_18_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Pier_18_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Racecourse_06_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("RaceCourse_06_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("S_SanPedro_28_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("S_SanPedro_28_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("S_Seoul_27_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("S_Seoul_27_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("SanPedroGarage_30_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("SanPedroGarage_30_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Scrapyard_29_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Scrapyard_29_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("SeoulPark_23_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("SeoulPark_23_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("StrawberryClub_34_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("StrawberryClub_34_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("VAGOS_HANGOUT".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Vespucci_20_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Vespucci_20_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("VespucciBeach_19_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("VespucciBeach_19_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("W_Canals_21_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("W_Canals_21_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("W_PaletoBay_09_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("W_PaletoBay_09_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("W_Puerto_25_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("W_Puerto_25_Vagos".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("W_SandyShores_11_Bikers".ToLower(), Enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("W_SandyShores_11_Vagos".ToLower(), Enabled);
    }
    private void TerminateDispatch()
    {
        SetDispatch(false);
        IsVanillaDispatchActive = false;
        GameTimeLastTerminatedVanillaDispatch = Game.GameTime;
    }
    private void TerminateScenarioCops()
    {
        IsVanillaScenarioCopsActive = false;
        SetScenarioCops(false);
    }
    private void ActivateDispatch()
    {
        SetDispatch(true);
        IsVanillaDispatchActive = true;
    }
    private void TerminateAudio()
    {
        if (Settings.SettingsManager.VanillaSettings.TerminateScanner)
        {
            NativeFunction.Natives.xB9EFD5C25018725A("PoliceScannerDisabled", true);
        }
        if (Settings.SettingsManager.VanillaSettings.TerminateWantedMusic)
        {
            NativeFunction.Natives.xB9EFD5C25018725A("WantedMusicDisabled", true);
        }
    }
    private void TerminateHealthRecharge()
    {
        NativeFunction.CallByName<bool>("SET_PLAYER_HEALTH_RECHARGE_MULTIPLIER", Game.LocalPlayer, 0f);
    }
    private void TerminateRespawnController()
    {
        var MyPtr = Game.GetScriptGlobalVariableAddress(4); //the script id for respawn_controller
        Marshal.WriteInt32(MyPtr, 1); //setting it to 1 turns it off somehow?
        Game.TerminateAllScriptsWithName("respawn_controller");
        IsVanillaRespawnActive = false;
    }
    private void TerminateRespawnScripts()
    {
        Game.DisableAutomaticRespawn = true;
        Game.FadeScreenOutOnDeath = false;
        Game.TerminateAllScriptsWithName("selector");
        NativeFunction.Natives.x1E0B4DC0D990A4E7(false);
        NativeFunction.Natives.x21FFB63D8C615361(true);
    }
    private void ActivateRespawn()
    {
        var MyPtr = Game.GetScriptGlobalVariableAddress(4); //the script id for respawn_controller
        Marshal.WriteInt32(MyPtr, 0); //setting it to 0 turns it on somehow?
        Game.StartNewScript("respawn_controller");
       Game.StartNewScript("selector");
        IsVanillaRespawnActive = true;
    }
    private void ActivateScenarioCops()
    {
        IsVanillaScenarioCopsActive = true;
        SetScenarioCops(true);
    }
    private void SetDispatch(bool Enabled)
    {
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.PoliceAutomobile, Enabled);
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.PoliceHelicopter, Enabled);
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.PoliceVehicleRequest, Enabled);
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.SwatAutomobile, Enabled);
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.SwatHelicopter, Enabled);
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.PoliceRiders, Enabled);
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.PoliceRoadBlock, Enabled);
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.PoliceAutomobileWaitCruising, Enabled);
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.PoliceAutomobileWaitPulledOver, Enabled);
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.AmbulanceDepartment, Enabled);
        NativeFunction.Natives.ENABLE_DISPATCH_SERVICE<bool>((int)VanillaDispatchType.FireDepartment, Enabled);     
        NativeFunction.Natives.SET_DISPATCH_COPS_FOR_PLAYER(Enabled);
    }
    private void SetScenarioCops(bool Enabled)
    {
        NativeFunction.Natives.SET_CREATE_RANDOM_COPS(Enabled);
        NativeFunction.Natives.SET_CREATE_RANDOM_COPS_ON_SCENARIOS(Enabled);
        NativeFunction.Natives.SET_CREATE_RANDOM_COPS_NOT_ON_SCENARIOS(Enabled);
    }






}

