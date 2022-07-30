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
        if (Settings.SettingsManager.RecoilSettings.ApplyRecoil && Player.WeaponEquipment.CurrentWeapon != null)// && !IsInVehicle)
        {
            if (Player.WeaponEquipment.CurrentWeapon.Category == WeaponCategory.Throwable || Player.WeaponEquipment.CurrentWeapon.Category == WeaponCategory.Vehicle || Player.WeaponEquipment.CurrentWeapon.Category == WeaponCategory.Melee || Player.WeaponEquipment.CurrentWeapon.Category == WeaponCategory.Misc || Player.WeaponEquipment.CurrentWeapon.Category == WeaponCategory.Unknown)
            {
                return;
            }
            if(Player.IsRagdoll || Player.IsStunned)
            {
                return;
            }
            if (Player.IsInVehicle && !Settings.SettingsManager.RecoilSettings.ApplyRecoilInVehicle)
            {
                return;
            }
            if(!Player.IsInVehicle && !Settings.SettingsManager.RecoilSettings.ApplyRecoilOnFoot)
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
        if (Math.Abs(AdjustedPitch) > 0)
        {
            NativeFunction.Natives.SET_GAMEPLAY_CAM_RELATIVE_PITCH(CurrentPitch + AdjustedPitch, Math.Abs(AdjustedPitch));
        }

        CurrentHeading = NativeFunction.Natives.GET_GAMEPLAY_CAM_RELATIVE_HEADING<float>();
        AdjustHeading();
        if (Math.Abs(AdjustedHeading) > 0)
        {
            NativeFunction.Natives.SET_GAMEPLAY_CAM_RELATIVE_HEADING(CurrentHeading + AdjustedHeading);
        }
    }
    private void AdjustPitch()
    {
        AdjustedPitch = RandomItems.GetRandomNumber(Player.WeaponEquipment.CurrentWeapon.MinVerticalRecoil, Player.WeaponEquipment.CurrentWeapon.MaxVerticalRecoil);
        if (Player.IsInVehicle)
        {
            AdjustedPitch *= 0.2f;//2.0f;//5.0 is good with pistol too much for automatic
            AdjustedPitch *= Settings.SettingsManager.RecoilSettings.VerticalInVehicleRecoilAdjuster;
        }
        else
        {
            AdjustedPitch *= Settings.SettingsManager.RecoilSettings.VerticalOnFootRecoilAdjuster;
        }
        AdjustedPitch *= Settings.SettingsManager.RecoilSettings.VerticalRecoilAdjuster;
    }
    private void AdjustHeading()
    {
        AdjustedHeading = RandomItems.GetRandomNumber(Player.WeaponEquipment.CurrentWeapon.MinHorizontalRecoil, Player.WeaponEquipment.CurrentWeapon.MaxHorizontalRecoil);
        if (RandomItems.RandomPercent(50))
        {
            AdjustedHeading *= -1.0f;
        }
        if(Player.IsInVehicle)
        {
            AdjustedHeading *= 0.2f;
            AdjustedHeading *= Settings.SettingsManager.RecoilSettings.HorizontalInVehicleRecoilAdjuster;
        }
        else
        {
            AdjustedHeading *= Settings.SettingsManager.RecoilSettings.HorizontalOnFootRecoilAdjuster;
        }
        AdjustedHeading *= Settings.SettingsManager.RecoilSettings.HorizontalRecoilAdjuster;
    }
}

