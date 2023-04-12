using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

public class WeaponSelector
{
    private uint GameTimeLastToggledSelector;
    private bool canShoot;
    private bool isControlDisabled = false;
    private IWeaponSelectable Player;
    private WeaponInformation prevCurrentWeapon;
    private int roundsFired;
    private ISettingsProvideable Settings;
    private List<SelectorHistory> LastWeaponSelections = new List<SelectorHistory>();
    public WeaponSelector(IWeaponSelectable player, ISettingsProvideable settings)
    {
        Player = player;
        Settings = settings;
    }
    public SelectorOptions CurrentSelectorSetting { get; private set; } = SelectorOptions.FullAuto;
    private int BulletLimt
    {
        get
        {
            if (CurrentSelectorSetting == SelectorOptions.Safe)
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
    public void SetSelectorSetting(SelectorOptions eSelectorSetting)
    {
        if (CurrentSelectorSetting == SelectorOptions.Safe && eSelectorSetting != SelectorOptions.Safe)
        {
            canShoot = true;
        }
        if (Player.WeaponEquipment.CurrentWeapon != null)
        {
            SelectorHistory LastSetting = LastWeaponSelections.FirstOrDefault(x => x.WeaponHash == Player.WeaponEquipment.CurrentWeapon.Hash);
            if (LastSetting == null)
            {
                LastSetting = new SelectorHistory(Player.WeaponEquipment.CurrentWeapon.Hash, eSelectorSetting);
                LastWeaponSelections.Add(LastSetting);
            }
            else
            {
                LastSetting.SelectorSetting = eSelectorSetting;
            }
        }
        CurrentSelectorSetting = eSelectorSetting;
        roundsFired = 0;
    }
    public void ToggleSelector()
    {
        if (Player.WeaponEquipment.CurrentWeapon != null)
        {
            if (Game.GameTime - GameTimeLastToggledSelector >= 200)//delayso it doesnt get toggled like crazy
            {
                //EntryPoint.WriteToConsole($"ToggleSelector Initial Setting {CurrentSelectorSetting}");
                bool found1 = false;
                bool found2 = false;
                foreach (SelectorOptions x in Enum.GetValues(typeof(SelectorOptions)))
                {
                    if (CurrentSelectorSetting == x)
                    {
                        found1 = true;
                    }
                    else if (found1 && Player.WeaponEquipment.CurrentWeapon.SelectorOptions.HasFlag(x))
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
                //EntryPoint.WriteToConsole($"ToggleSelector Final Setting {CurrentSelectorSetting}");
                NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                GameTimeLastToggledSelector = Game.GameTime;
            }
        }
    }
    public void Update()
    {
        if (Settings.SettingsManager.SelectorSettings.ApplySelector && (!Player.IsUsingController || Settings.SettingsManager.SelectorSettings.ApplySelectorWithController))
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
        //Player.DebugLine4 = $"Selector: {CurrentSelectorSetting} {roundsFired}/{BulletLimt}";
        //GameFiber.Yield();
    }
    private void Reset()
    {
        canShoot = true;
        roundsFired = 0;

        if (Player.WeaponEquipment.CurrentWeapon != null)
        {
            SelectorHistory LastSetting = LastWeaponSelections.FirstOrDefault(x => x.WeaponHash == Player.WeaponEquipment.CurrentWeapon.Hash);
            if(LastSetting != null)
            {
                CurrentSelectorSetting = LastSetting.SelectorSetting;
            }
            else if (Player.WeaponEquipment.CurrentWeapon.SelectorOptions.HasFlag(SelectorOptions.FullAuto))
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
    private void UpdateShooting()
    {
        if (prevCurrentWeapon?.Hash != Player.WeaponEquipment.CurrentWeapon?.Hash || Player.WeaponEquipment.CurrentWeapon == null)
        {
            Reset();
            prevCurrentWeapon = Player.WeaponEquipment.CurrentWeapon;
        }

        if (BulletLimt == 0)
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
            if (Player.IsInVehicle)
            {
                NativeFunction.Natives.xE8A25867FBA3B05E(2, 69, 1.0f);
            }
            else
            {
                NativeFunction.Natives.xE8A25867FBA3B05E(2, 24, 1.0f);
            }
            //EntryPoint.WriteToConsoleTestLong("Fired Round");
            //roundsFired++;
        }
        else if (Player.ReleasedFireWeapon && (roundsFired > BulletLimt - 1) && BulletLimt > 0)
        {
            roundsFired = 0;
            canShoot = true;

            //EntryPoint.WriteToConsoleTestLong("Released Fire");
        }
        if (Player.WeaponEquipment.CurrentWeapon == null)
        {
            Reset();
        }
        if (Player.Character.IsReloading)
        {
            canShoot = true;
            roundsFired = 0;

            //EntryPoint.WriteToConsole("Started Reloading");
        }
        SetShootingEnabled(canShoot);
    }
    private void UpdateSelectorHistory()
    {

    }


    private class SelectorHistory
    {
        public SelectorHistory(uint hash, SelectorOptions eSelectorSetting)
        {
            WeaponHash = hash;
            SelectorSetting = eSelectorSetting;
        }

        public uint WeaponHash { get; set; }
        public SelectorOptions SelectorSetting { get; set; }
    }
}