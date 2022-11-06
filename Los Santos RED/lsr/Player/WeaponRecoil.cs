using LosSantosRED.lsr.Interface;
using Rage;
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
    private float CurrentHeading;
    private int TotalShots = 0;
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
            if (Player.IsInFirstPerson && !Settings.SettingsManager.RecoilSettings.ApplyRecoilInFirstPerson)
            {
                return;
            }
            if (Player.WeaponEquipment.CurrentWeapon.Category == WeaponCategory.Sniper && !Settings.SettingsManager.RecoilSettings.ApplyRecoilToSnipers)
            {
                return;
            }
            if (Game.LocalPlayer.Character.IsReloading)
            {
                return;
            }
            ApplyRecoil();
            TotalShots++;
        } 
       // Player.DebugString = $"R P: {Math.Round(CurrentPitch,6)} AdjP: {Math.Round(AdjustedPitch,6)} H: {Math.Round(CurrentHeading,6)} AdjH: {Math.Round(AdjustedHeading,6)} TS: {TotalShots}";

    }
    private void ApplyRecoil()
    {
        CurrentPitch = NativeFunction.Natives.GET_GAMEPLAY_CAM_RELATIVE_PITCH<float>();
        AdjustPitch();

        if(Player.IsInVehicle)
        {
            AdjustedPitch *= -1.0f;
        }

        if (Settings.SettingsManager.RecoilSettings.UseAlternateCalculation)//Player.IsInFirstPerson)
        {
            NativeFunction.Natives.SET_GAMEPLAY_CAM_RELATIVE_PITCH(CurrentPitch + AdjustedPitch, 1.0f);
        }
        else
        {
            NativeFunction.Natives.SET_GAMEPLAY_CAM_RELATIVE_PITCH(CurrentPitch + AdjustedPitch, Math.Abs(AdjustedPitch)); 
        }

        if (Player.IsInVehicle || 1==1)
        {
            CurrentHeading = NativeFunction.Natives.GET_GAMEPLAY_CAM_RELATIVE_HEADING<float>();
        }
        else
        {
            CurrentHeading = 0f;
        }
        AdjustHeading();
        if (Math.Abs(AdjustedHeading) > 0)
        {
            NativeFunction.Natives.SET_GAMEPLAY_CAM_RELATIVE_HEADING(CurrentHeading + AdjustedHeading);
        }
    }
    private void AdjustPitch()
    {
        AdjustedPitch = RandomItems.GetRandomNumber(Player.WeaponEquipment.CurrentWeapon.MinVerticalRecoil, Player.WeaponEquipment.CurrentWeapon.MaxVerticalRecoil);
        if (Settings.SettingsManager.RecoilSettings.UseAlternateCalculation)
        {
            AdjustedPitch *= 2.0f;//want this to be near to 1.0 in the settings default;//Settings.SettingsManager.SwaySettings.VeritcalSwayAdjuster * 0.0075f * 20.0f * 1.25f;//want this to be near to 1.0 in the settings default;
        }
        if (Player.IsInVehicle)
        {
            AdjustedPitch *= 0.2f;//2.0f;//5.0 is good with pistol too much for automatic
            AdjustedPitch *= Settings.SettingsManager.RecoilSettings.VerticalInVehicleRecoilAdjuster;
        }
        else
        {
            AdjustedPitch *= Settings.SettingsManager.RecoilSettings.VerticalOnFootRecoilAdjuster;
        }
        if(Player.IsInFirstPerson)
        {
            AdjustedPitch *= Settings.SettingsManager.RecoilSettings.VerticalFirstPersonRecoilAdjuster;
        }
        AdjustedPitch *= Settings.SettingsManager.RecoilSettings.VerticalRecoilAdjuster;
    }
    private void AdjustHeading()
    {
        AdjustedHeading = RandomItems.GetRandomNumber(Player.WeaponEquipment.CurrentWeapon.MinHorizontalRecoil, Player.WeaponEquipment.CurrentWeapon.MaxHorizontalRecoil);
        if (Settings.SettingsManager.RecoilSettings.UseAlternateCalculation)
        {
            AdjustedHeading *= 1.5f;
        }
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
        if(Player.IsInFirstPerson)
        {
            AdjustedHeading *= Settings.SettingsManager.RecoilSettings.HorizontalFirstPersonRecoilAdjuster;
        }

        AdjustedHeading *= Settings.SettingsManager.RecoilSettings.HorizontalRecoilAdjuster;
    }
}

