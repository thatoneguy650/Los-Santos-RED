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
    public VanillaManager()
    {
    }

    public void Dispose()
    {
        ActivateVanillaRespawn();
    }
    public void Tick()
    {
        if (IsVanillaRespawnActive)
        {
            TerminateVanillaRespawnController();
        }
        TerminateVanillaRespawnScripts();
        TerminateVanillaHealthRecharge();
        TerminateVanillaAudio();
    }
    private void TerminateVanillaAudio()
    {
        NativeFunction.Natives.xB9EFD5C25018725A("PoliceScannerDisabled", true);
        NativeFunction.Natives.xB9EFD5C25018725A("WantedMusicDisabled", true);
    }
    private void TerminateVanillaHealthRecharge()
    {
        NativeFunction.CallByName<bool>("SET_PLAYER_HEALTH_RECHARGE_MULTIPLIER", Game.LocalPlayer, 0f);
    }
    private void TerminateVanillaRespawnController()
    {
        var MyPtr = Game.GetScriptGlobalVariableAddress(4); //the script id for respawn_controller
        Marshal.WriteInt32(MyPtr, 1); //setting it to 1 turns it off somehow?
        Game.TerminateAllScriptsWithName("respawn_controller");
        IsVanillaRespawnActive = false;
    }
    private void TerminateVanillaRespawnScripts()
    {
        Game.DisableAutomaticRespawn = true;
        Game.FadeScreenOutOnDeath = false;
        Game.TerminateAllScriptsWithName("selector");
        NativeFunction.Natives.x1E0B4DC0D990A4E7(false);
        NativeFunction.Natives.x21FFB63D8C615361(true);
    }
    private void ActivateVanillaRespawn()
    {
        var MyPtr = Game.GetScriptGlobalVariableAddress(4); //the script id for respawn_controller
        Marshal.WriteInt32(MyPtr, 0); //setting it to 0 turns it on somehow?
        Game.StartNewScript("respawn_controller");
        Game.StartNewScript("selector");
        IsVanillaRespawnActive = true;
    }
}

