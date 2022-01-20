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
    public SelectorOptions CurrentSelectorSetting { get; private set; } = SelectorOptions.FullAuto;
    private WeaponInformation prevCurrentWeapon;
    private int BulletLimt
    {
        get
        {
            if(CurrentSelectorSetting == SelectorOptions.Safe)
            {
                return 0;
            }
            else if (CurrentSelectorSetting == SelectorOptions.SemiAuto)
            {
                return 1;
            }
            else if (CurrentSelectorSetting == SelectorOptions.TwoRoundBurst)
            {
                return 2;
            }
            else if (CurrentSelectorSetting == SelectorOptions.ThreeRoundBurst)
            {
                return 3;
            }
            else if (CurrentSelectorSetting == SelectorOptions.FiveRoundBurst)
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
    public void SetSelectorSetting(SelectorOptions eSelectorSetting)
    {
        if(CurrentSelectorSetting == SelectorOptions.Safe && eSelectorSetting != SelectorOptions.Safe)
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

        if(Player.CurrentWeapon != null)
        {
            if(Player.CurrentWeapon.SelectorOptions.HasFlag(SelectorOptions.FullAuto))
            {
                CurrentSelectorSetting = SelectorOptions.FullAuto;
            }
            else
            {
                CurrentSelectorSetting = SelectorOptions.SemiAuto;
            }
        }
        else
        {
            CurrentSelectorSetting = SelectorOptions.FullAuto;
        }
    }

    public void ToggleSelector()
    {
        if (Player.CurrentWeapon != null)
        {
            EntryPoint.WriteToConsole($"ToggleSelector Initial Setting {CurrentSelectorSetting}", 5);
            bool found1 = false;
            bool found2 = false;
            foreach (SelectorOptions x in Enum.GetValues(typeof(SelectorOptions)))
            {
                if (CurrentSelectorSetting == x)
                {
                    found1 = true;
                }
                else if (found1 && Player.CurrentWeapon.SelectorOptions.HasFlag(x))
                {
                    found2 = true;
                    SetSelectorSetting(x);
                    break;
                }
            }
            if (!found2)
            {
                SetSelectorSetting(SelectorOptions.Safe);
            }
            EntryPoint.WriteToConsole($"ToggleSelector Final Setting {CurrentSelectorSetting}", 5);
            NativeFunction.Natives.PLAY_SOUND_FRONTEND(0, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
        }
    }
}

