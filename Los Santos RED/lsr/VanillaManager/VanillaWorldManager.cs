using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


public class VanillaWorldManager
{
    private bool isRandomEventsDisabled;
    private bool IsVanillaRespawnActive = true;
    private ISettingsProvideable Settings;
    private bool isVanillaShopsActive;
    private bool isVanillaBlipsActive;

    public VanillaWorldManager(ISettingsProvideable settings)
    {
        Settings = settings;
    }
    public void Setup()
    {

    }
    public void Dispose()
    {
        ActivateRespawn();
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

        if (Settings.SettingsManager.VanillaSettings.TerminateRespawn)
        {
            TerminateRespawnScripts();
        }
        if (Settings.SettingsManager.VanillaSettings.TerminateSelector)
        {
            TerminateSelectorScripts();
        }

        if (Settings.SettingsManager.VanillaSettings.TerminateHealthRecharge)
        {
            TerminateHealthRecharge();
        }

        if (Settings.SettingsManager.VanillaSettings.TerminateScenarioPeds)
        {
            TerminateScenarioPeds();
        }

        if (Settings.SettingsManager.VanillaSettings.TerminateRandomEvents)
        {
            if (!isRandomEventsDisabled)
            {
                TerminateRandomEvents();
            }
        }
        else if (isRandomEventsDisabled)
        {
            ActivateRandomEvents();
        }


        if(Settings.SettingsManager.VanillaSettings.TerminateVanillaShops)
        {
            if(isVanillaShopsActive)
            {
                TerminateShopController();
            }
        }
        else
        {
            if (!isVanillaShopsActive)
            {
                ActivateShopController();
            }
        }

        if (Settings.SettingsManager.VanillaSettings.TerminateVanillaBlips)
        {
            if (isVanillaBlipsActive)
            {
                TerminateBlipController();
            }
        }
        else
        {
            if (!isVanillaBlipsActive)
            {
                ActivateBlipController();
            }
        }

        TerminateAudio();
    }
    private void ActivateRandomEvents()
    {
        NativeFunction.Natives.SET_RANDOM_EVENT_FLAG(false);
        isRandomEventsDisabled = false;
    }
    private void TerminateRandomEvents()
    {
        NativeFunction.Natives.SET_RANDOM_EVENT_FLAG(false);
        isRandomEventsDisabled = true;
    }
    private void TerminateScenarioPeds()
    {
        NativeFunction.Natives.SET_SCENARIO_PED_DENSITY_MULTIPLIER_THIS_FRAME(0f);
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
        //Game.TerminateAllScriptsWithName("selector");
        NativeFunction.Natives.x1E0B4DC0D990A4E7(false);
        NativeFunction.Natives.x21FFB63D8C615361(true);
    }
    private void TerminateSelectorScripts()
    {
        Game.TerminateAllScriptsWithName("selector");
    }
    private void ActivateRespawn()
    {
        var MyPtr = Game.GetScriptGlobalVariableAddress(4); //the script id for respawn_controller
        Marshal.WriteInt32(MyPtr, 0); //setting it to 0 turns it on somehow?
        Game.StartNewScript("respawn_controller");
        Game.StartNewScript("selector");
        IsVanillaRespawnActive = true;
    }
    private void TerminateShopController()
    {
        Game.TerminateAllScriptsWithName("shop_controller");
        isVanillaShopsActive = false;
    }
    private void ActivateShopController()
    {
        Game.StartNewScript("shop_controller");
        isVanillaShopsActive = true;
    }


    private void TerminateBlipController()
    {
        Game.TerminateAllScriptsWithName("blip_controller");
        isVanillaBlipsActive = false;
    }
    private void ActivateBlipController()
    {
        Game.StartNewScript("blip_controller");
        isVanillaBlipsActive = true;
    }

}

