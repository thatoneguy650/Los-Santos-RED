using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class WeaponSelector
{
    private IWeaponSelectable Player;
    private ISettingsProvideable Settings;
    private float CurrentPitch;
    private float AdjustedPitch;
    private float AdjustedHeading;
    private dynamic CurrentHeading;
    private int roundsFired;
    private bool canShoot;
    private bool isControlDisabled = false;
    public eSelectorSetting CurrentSelectorSetting { get; private set; } = eSelectorSetting.FullAuto;
    private WeaponInformation prevCurrentWeapon;
    private int BulletLimt
    {
        get
        {
            if(CurrentSelectorSetting == eSelectorSetting.Safe)
            {
                return 0;
            }
            else if (CurrentSelectorSetting == eSelectorSetting.SemiAuto)
            {
                return 1;
            }
            else if (CurrentSelectorSetting == eSelectorSetting.TwoRoundBurst)
            {
                return 2;
            }
            else if (CurrentSelectorSetting == eSelectorSetting.ThreeRoundBurst)
            {
                return 3;
            }
            else if (CurrentSelectorSetting == eSelectorSetting.FiveRoundBurst)
            {
                return 5;
            }
            else
            {
                return 9999;
            }
        }
    }
    public WeaponSelector(IWeaponSelectable player, ISettingsProvideable settings)
    {
        Player = player;
        Settings = settings;
    }
    public void Update()
    {
        if (Settings.SettingsManager.SelectorSettings.ApplySelector)
        {
            UpdateShooting();
        }
        else
        {
            if (isControlDisabled)
            {
                SetShootingEnabled(true);
            }
        }
        Player.DebugLine4 = $"Selector: {CurrentSelectorSetting} {roundsFired}/{BulletLimt}";
        //GameFiber.Yield();
    }
    public void SetSelectorSetting(eSelectorSetting eSelectorSetting)
    {
        if(CurrentSelectorSetting == eSelectorSetting.Safe && eSelectorSetting != eSelectorSetting.Safe)
        {
            canShoot = true;
        }
        CurrentSelectorSetting = eSelectorSetting;
        roundsFired = 0;
    }
    private void UpdateShooting()
    {
        if(prevCurrentWeapon?.Hash != Player.CurrentWeapon?.Hash || Player.CurrentWeapon == null)
        {
            Reset();
            prevCurrentWeapon = Player.CurrentWeapon;
        }


        if(BulletLimt == 0)
        {
            canShoot = false;
        }  
        if (Player.Character.IsShooting)
        {
            roundsFired++;
            if (roundsFired > BulletLimt - 1)
            {
                canShoot = false;
            }
        }
        else if (BulletLimt > 1 && BulletLimt < 9999 && roundsFired > 0 && (roundsFired <= BulletLimt - 1) && BulletLimt > 0)
        {
            if(Player.IsInVehicle)
            {
                NativeFunction.Natives.xE8A25867FBA3B05E(2, 69, 1.0f);
            }
            else
            {
                NativeFunction.Natives.xE8A25867FBA3B05E(2, 24, 1.0f);
            }
            
            //roundsFired++;
        }
        else if (Player.ReleasedFireWeapon && (roundsFired > BulletLimt - 1) && BulletLimt > 0)
        {
            roundsFired = 0;
            canShoot = true;
        }
        if (Player.CurrentWeapon == null)
        {
            Reset();
        }
        if(Player.Character.IsReloading)
        {
            canShoot = true;
            roundsFired = 0;
        }
        SetShootingEnabled(canShoot);
    }
    private void SetShootingEnabled(bool enabled)
    {
        if (enabled)
        {
            isControlDisabled = false;
            NativeFunction.Natives.ENABLE_CONTROL_ACTION(0, 24, true);
            NativeFunction.Natives.ENABLE_CONTROL_ACTION(0, 257, true);
            NativeFunction.Natives.ENABLE_CONTROL_ACTION(0, 69, true);
            NativeFunction.Natives.ENABLE_CONTROL_ACTION(0, 70, true);
            NativeFunction.Natives.ENABLE_CONTROL_ACTION(0, 92, true);
            NativeFunction.Natives.ENABLE_CONTROL_ACTION(0, 114, true);
            NativeFunction.Natives.ENABLE_CONTROL_ACTION(0, 331, true);
        }
        else
        {
            isControlDisabled = true;
            NativeFunction.Natives.DISABLE_CONTROL_ACTION(0, 24, true);
            NativeFunction.Natives.DISABLE_CONTROL_ACTION(0, 257, true);
            NativeFunction.Natives.DISABLE_CONTROL_ACTION(0, 69, true);
            NativeFunction.Natives.DISABLE_CONTROL_ACTION(0, 70, true);
            NativeFunction.Natives.DISABLE_CONTROL_ACTION(0, 92, true);
            NativeFunction.Natives.DISABLE_CONTROL_ACTION(0, 114, true);
            NativeFunction.Natives.DISABLE_CONTROL_ACTION(0, 331, true);
        }
    }
    private void Reset()
    {
        canShoot = true;
        roundsFired = 0;
        CurrentSelectorSetting = eSelectorSetting.FullAuto;
    }

    public void ToggleSelector()
    {
        if(CurrentSelectorSetting == eSelectorSetting.Safe)
        {
            SetSelectorSetting(eSelectorSetting.SemiAuto);
        }
        else if (CurrentSelectorSetting == eSelectorSetting.SemiAuto)
        {
            SetSelectorSetting(eSelectorSetting.TwoRoundBurst);
        }
        else if (CurrentSelectorSetting == eSelectorSetting.TwoRoundBurst)
        {
            SetSelectorSetting(eSelectorSetting.ThreeRoundBurst);
        }
        else if (CurrentSelectorSetting == eSelectorSetting.ThreeRoundBurst)
        {
            SetSelectorSetting(eSelectorSetting.FiveRoundBurst);
        }
        else if (CurrentSelectorSetting == eSelectorSetting.FiveRoundBurst)
        {
            SetSelectorSetting(eSelectorSetting.FullAuto);
        }
        else if (CurrentSelectorSetting == eSelectorSetting.FullAuto)
        {
            SetSelectorSetting(eSelectorSetting.Safe);
        }
        NativeFunction.Natives.PLAY_SOUND_FRONTEND(0, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);


    }
}

