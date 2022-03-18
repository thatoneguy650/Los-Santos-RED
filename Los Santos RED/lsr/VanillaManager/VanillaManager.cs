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
            if (IsVanillaScenarioCopsActive)
            {
                TerminateScenarioCops();
            }
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

        //if (Game.GameTime - GameTimeLastStoppedGang >= 10000)
        //{

        //    if (Settings.SettingsManager.VanillaSettings.TerminateScenarioGangs)
        //    {
        //        TerminateScenarioGangs();
        //    }
        //    else
        //    {
        //        if (!IsVanillaScenarioGangsActive)
        //        {
        //            ActivateScenarioGangs();
        //        }
        //    }
        //    if (Settings.SettingsManager.VanillaSettings.SuppressVanillaGangPeds)
        //    {
        //        SupressVanillaGangPeds();

        //    }
        //    else
        //    {
        //        if (IsVanillaGangPedsSupressed)
        //        {
        //            UnSupressVanillaGangPeds();
        //        }
        //    }
        //    GameTimeLastStoppedGang = Game.GameTime;
        //}

        if (Settings.SettingsManager.VanillaSettings.TerminateHealthRecharge)
        {
            TerminateHealthRecharge();
        }
        TerminateAudio();   
    }

    private void SupressVanillaGangPeds()
    {
        IsVanillaGangPedsSupressed = true;
        SetGangPedsSupression(true);
    }
    private void UnSupressVanillaGangPeds()
    {
        IsVanillaGangPedsSupressed = false;
        SetGangPedsSupression(false);
    }

    private void SetGangPedsSupression(bool isSuppressed)
    {
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("a_m_y_mexthug_01".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_mexgang_01".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_m_mexboss_02".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_m_mexboss_01".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_armgoon_02".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_m_armlieut_01".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_m_armgoon_01".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_m_armboss_01".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_korlieut_01".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_korean_02".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_korean_01".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_m_korboss_01".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_m_chigoon_02".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_m_chigoon_01".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_azteca_01".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_salvagoon_03".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_salvagoon_02".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_salvagoon_01".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_salvaboss_01".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_f_y_ballas_01".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_ballaorig_01".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_ballaeast_01".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_ballasout_01".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_f_y_families_01".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_famfor_01".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_famdnf_01".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_famca_01".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_f_y_vagos_01".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_mexgoon_03".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_mexgoon_02".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_mexgoon_01".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_f_y_lost_01".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_lost_03".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_lost_02".ToLower()), isSuppressed);
        NativeFunction.Natives.SET_PED_MODEL_IS_SUPPRESSED(Game.GetHashKey("g_m_y_lost_01".ToLower()), isSuppressed);


        EntryPoint.WriteToConsole($"                                SET                 Vanilla GANG PEDS SUPPRESSION: {isSuppressed}");
    }

    private void ActivateScenarioGangs()
    {
        IsVanillaScenarioGangsActive = true;
        SetGangScenarios(true);
    }

    private void TerminateScenarioGangs()
    {
        IsVanillaScenarioGangsActive = false;
        SetGangScenarios(false);
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

