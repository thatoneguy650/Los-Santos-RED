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
    private bool isVanillaShopsActive = true;
    private bool isVanillaBlipsActive = true;
    private bool isVanillaVendingActive;
    private bool hasSetMaxWanted;
    private uint GameTimeLastTerminatedAudio;
    private bool isVanillaCarRaceActive;
    private uint GameTimeLastSetMaxWanted;

    public VanillaWorldManager(ISettingsProvideable settings)
    {
        Settings = settings;
    }
    public void Setup()
    {
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("City_Banks", true);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Countryside_Banks", true);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("AMMUNATION", true);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("YellowJackInn", true);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("VANGELICO", true);
        //NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("lost_mc", true);
        //NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("lost_hangout", true); 
        //NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("LOST_HANGOUT", true);
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


        //if (Settings.SettingsManager.VanillaSettings.TerminateVanillaCarRaces)
        //{
        //    if (isVanillaCarRaceActive)
        //    {
        //        TerminateVanillaCarRace();
        //    }
        //}
        //else
        //{
        //    if (!isVanillaCarRaceActive)
        //    {
        //        ActivateVanillaCarRace();
        //    }
        //}



        if (Settings.SettingsManager.PoliceSettings.TakeExclusiveControlOverWantedLevel)
        {
            if(!hasSetMaxWanted || Game.GameTime - GameTimeLastSetMaxWanted >= 20000)
            {
                NativeFunction.CallByName<bool>("SET_MAX_WANTED_LEVEL", 0);
                hasSetMaxWanted = true;
                GameTimeLastSetMaxWanted = Game.GameTime;
            }
        }
        else
        {
            if(hasSetMaxWanted)
            {
                NativeFunction.CallByName<bool>("SET_MAX_WANTED_LEVEL", 6);
                hasSetMaxWanted = false;
            }
        }
    }
    private void TerminateScenarioPeds()
    {
        NativeFunction.Natives.SET_SCENARIO_PED_DENSITY_MULTIPLIER_THIS_FRAME(0f);
    }
    private void TerminateAudio()
    {
        if (Game.GameTime - GameTimeLastTerminatedAudio >= 2000)
        {
            if (Settings.SettingsManager.VanillaSettings.TerminateScanner)
            {
                NativeFunction.Natives.xB9EFD5C25018725A("PoliceScannerDisabled", true);
            }
            if (Settings.SettingsManager.VanillaSettings.TerminateWantedMusic)
            {
                NativeFunction.Natives.xB9EFD5C25018725A("WantedMusicDisabled", true);
            }
            GameTimeLastTerminatedAudio = Game.GameTime;
        }
    }
    private void TerminateHealthRecharge()
    {
        NativeFunction.Natives.SET_PLAYER_HEALTH_RECHARGE_MULTIPLIER(Game.LocalPlayer, 0.0f);
        NativeFunction.Natives.SET_PLAYER_HEALTH_RECHARGE_MAX_PERCENT(Game.LocalPlayer,0.0f);
    }
    private void TerminateRespawnController()
    {
        var MyPtr = Game.GetScriptGlobalVariableAddress(Settings.SettingsManager.VanillaSettings.TerminateRespawnGlobalID);// 5);// 4); //the script id for respawn_controller
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
        var MyPtr = Game.GetScriptGlobalVariableAddress(5);// 4); //the script id for respawn_controller
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



    private void TerminateVanillaCarRace()
    {
        Game.TerminateAllScriptsWithName("controller_races");
        Game.TerminateAllScriptsWithName("country_race_controller");
        isVanillaCarRaceActive = false;
    }
    private void ActivateVanillaCarRace()
    {
        Game.StartNewScript("controller_races");
        Game.StartNewScript("country_race_controller");
        isVanillaCarRaceActive = true;
    }



    private void TerminateVanillaMachines()
    {
        Game.TerminateAllScriptsWithName("ob_vend1");//doesnt work, the brain is attached to the object, this doesnt actually stop it
        Game.TerminateAllScriptsWithName("ob_vend2");
        Game.TerminateAllScriptsWithName("atm_trigger");
        Game.TerminateAllScriptsWithName("ob_cashregister");
        isVanillaVendingActive = false;
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

