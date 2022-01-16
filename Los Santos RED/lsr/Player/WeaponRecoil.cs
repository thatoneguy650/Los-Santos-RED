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

    public WeaponRecoil(IWeaponRecoilable player, ISettingsProvideable settings)
    {
        Player = player;
        Settings = settings;
    }
    public void Update()
    {
        Recoil();
    }
    private void Recoil()
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
            float currentPitch = NativeFunction.Natives.GET_GAMEPLAY_CAM_RELATIVE_PITCH<float>();
            float currentHeading = NativeFunction.Natives.GET_GAMEPLAY_CAM_RELATIVE_HEADING<float>();
            float AdjustedPitch = RandomItems.GetRandomNumber(Player.CurrentWeapon.MinVerticalRecoil, Player.CurrentWeapon.MaxVerticalRecoil);
            float AdjustedHeading = RandomItems.GetRandomNumber(Player.CurrentWeapon.MinHorizontalRecoil, Player.CurrentWeapon.MaxHorizontalRecoil);
            if (Player.IsInVehicle)
            {
                AdjustedPitch *= 3.0f;//5.0 is good with pistol too much for automatic
                currentPitch *= -1.0f;
            }
            AdjustedPitch *= Settings.SettingsManager.PlayerSettings.VerticalRecoilAdjuster;


            //if(Player.IsInVehicle)
            //{
            //    NativeFunction.Natives.SET_GAMEPLAY_CAM_RELATIVE_PITCH(currentPitch + AdjustedPitch, AdjustedPitch);
            //}
            //else
            //{
                NativeFunction.Natives.SET_GAMEPLAY_CAM_RELATIVE_PITCH(currentPitch + AdjustedPitch, Math.Abs(AdjustedPitch));
            //}
            
            if (RandomItems.RandomPercent(50))
            {
                AdjustedHeading *= -1.0f;
            }
            AdjustedHeading *= Settings.SettingsManager.PlayerSettings.HorizontalRecoilAdjuster;
            NativeFunction.Natives.SET_GAMEPLAY_CAM_RELATIVE_HEADING(currentHeading + AdjustedHeading);
        }
    }
}

