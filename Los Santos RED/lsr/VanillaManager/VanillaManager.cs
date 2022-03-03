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
    private uint GameTimeLastTerminatedVanillaDispatch;
    private ISettingsProvideable Settings;
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
            TerminateScenarioCops();
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
        

        if (Settings.SettingsManager.VanillaSettings.TerminateHealthRecharge)
        {
            TerminateHealthRecharge();
        }
        TerminateAudio();   
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

