using LosSantosRED.lsr.Interface;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class WeaponRecoil
{
    private IWeaponRecoilable Player;
    private ISettingsProvideable Settings;
    private float CurrentPitch;
    private float AdjustedPitch;
    private float AdjustedHeading;
    private dynamic CurrentHeading;

    public WeaponRecoil(IWeaponRecoilable player, ISettingsProvideable settings)
    {
        Player = player;
        Settings = settings;
    }
    public void Update()
    {
        if (Settings.SettingsManager.PlayerSettings.ApplyRecoil && Player.CurrentWeapon != null)// && !IsInVehicle)
        {
            if (Player.CurrentWeapon.Category == WeaponCategory.Throwable || Player.CurrentWeapon.Category == WeaponCategory.Vehicle || Player.CurrentWeapon.Category == WeaponCategory.Melee || Player.CurrentWeapon.Category == WeaponCategory.Misc || Player.CurrentWeapon.Category == WeaponCategory.Unknown)
            {
                return;
            }
            if (Player.IsInVehicle && !Settings.SettingsManager.PlayerSettings.ApplyRecoilInVehicle)
            {
                return;
            }
            ApplyRecoil();
        } 
    }
    private void ApplyRecoil()
    {
        CurrentPitch = NativeFunction.Natives.GET_GAMEPLAY_CAM_RELATIVE_PITCH<float>();
        AdjustPitch();
        if (Player.IsInVehicle)
        {
            CurrentPitch *= -1.0f;
        }
        NativeFunction.Natives.SET_GAMEPLAY_CAM_RELATIVE_PITCH(CurrentPitch + AdjustedPitch, Math.Abs(AdjustedPitch));

        CurrentHeading = NativeFunction.Natives.GET_GAMEPLAY_CAM_RELATIVE_HEADING<float>();
        AdjustHeading();
        NativeFunction.Natives.SET_GAMEPLAY_CAM_RELATIVE_HEADING(CurrentHeading + AdjustedHeading);
    }
    private void AdjustPitch()
    {
        AdjustedPitch = RandomItems.GetRandomNumber(Player.CurrentWeapon.MinVerticalRecoil, Player.CurrentWeapon.MaxVerticalRecoil);
        if (Player.IsInVehicle)
        {
            AdjustedPitch *= 2.0f;//5.0 is good with pistol too much for automatic
        }
        AdjustedPitch *= Settings.SettingsManager.PlayerSettings.VerticalRecoilAdjuster;
    }
    private void AdjustHeading()
    {
        AdjustedHeading = RandomItems.GetRandomNumber(Player.CurrentWeapon.MinHorizontalRecoil, Player.CurrentWeapon.MaxHorizontalRecoil);
        if (RandomItems.RandomPercent(50))
        {
            AdjustedHeading *= -1.0f;
        }
        AdjustedHeading *= Settings.SettingsManager.PlayerSettings.HorizontalRecoilAdjuster;
    }
}

