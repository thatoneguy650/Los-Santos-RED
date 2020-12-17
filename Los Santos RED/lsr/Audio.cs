using LosSantosRED.lsr;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Audio
{
    public bool IsMobileRadioEnabled { get; private set; }

    public Audio()
    {

    }

    public void Tick()
    {
        StopVanillaMusic();
        CheckMobileRadio();
    }
    private void CheckMobileRadio()
    {
        if (Mod.Player.CurrentVehicle != null && Mod.Player.CurrentVehicle.Vehicle.IsEngineOn && Mod.Player.CurrentVehicle.Vehicle.IsPoliceVehicle && !IsMobileRadioEnabled)
        {
            IsMobileRadioEnabled = true;
            NativeFunction.CallByName<bool>("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", true);
        }
        else
        {
            IsMobileRadioEnabled = false;
            NativeFunction.CallByName<bool>("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", false);
        }
    }
    private void StopVanillaMusic()
    {
        if (Mod.DataMart.Settings.SettingsManager.Police.DisableAmbientScanner)
        {
            NativeFunction.Natives.xB9EFD5C25018725A("PoliceScannerDisabled", true);
        }
        if (Mod.DataMart.Settings.SettingsManager.Police.WantedMusicDisable)
        {
            NativeFunction.Natives.xB9EFD5C25018725A("WantedMusicDisabled", true);
        }
    }
}

