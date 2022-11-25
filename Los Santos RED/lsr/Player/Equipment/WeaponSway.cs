using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class WeaponSway
{
    private IWeaponSwayable Player;
    private ISettingsProvideable Settings;
    private bool VerticalSwayDirection;
    private bool HorizontalSwayDirection;
    private uint GameTimeLastHorizontalChangedSwayDirection;
    private uint GameTimeBetweenHorizontalSwayChanges = 200;
    private uint GameTimeLastVerticalChangedSwayDirection;
    private uint GameTimeBetweenVerticalSwayChanges = 200;
    private float CurrentPitch;
    private float CurrentHeading;
    private float AdjustedPitch;
    private float AdjustedHeading;
    private float PercentVeritcalSwayElapsed;
    private float PercentHorizontalSwayElapsed;
    private float Speed;
    public bool IsDisabled { get; set; } = false;

    public WeaponSway(IWeaponSwayable player, ISettingsProvideable settings)
    {
        Player = player;
        Settings = settings;
    }
    public void Update()
    {
        Sway();
    }
    public void Reset()
    {
        GameTimeLastHorizontalChangedSwayDirection = 0;
        GameTimeLastVerticalChangedSwayDirection = 0;
    }
    private void Sway()
    {
        if (Settings.SettingsManager.SwaySettings.ApplySway && Player.WeaponEquipment.CurrentWeapon != null && !IsDisabled)// && !IsInVehicle)
        {
            if (Player.WeaponEquipment.CurrentWeapon.Category == WeaponCategory.Throwable || Player.WeaponEquipment.CurrentWeapon.Category == WeaponCategory.Vehicle || Player.WeaponEquipment.CurrentWeapon.Category == WeaponCategory.Melee || Player.WeaponEquipment.CurrentWeapon.Category == WeaponCategory.Misc || Player.WeaponEquipment.CurrentWeapon.Category == WeaponCategory.Unknown)
            {
                return;
            }
            if (Player.IsRagdoll || Player.IsStunned)
            {
                return;
            }
            if (Player.IsInVehicle && !Settings.SettingsManager.SwaySettings.ApplySwayInVehicle)
            {
                return;
            }
            if(!Player.IsInVehicle && !Settings.SettingsManager.SwaySettings.ApplySwayOnFoot)
            {
                return;
            }
            if(Player.IsInFirstPerson && !Settings.SettingsManager.SwaySettings.ApplySwayInFirstPerson)
            {
                return;
            }
            if(Player.IsInFirstPerson && !Game.LocalPlayer.IsFreeAiming)
            {
                return;
            }
            if(Game.LocalPlayer.Character.IsReloading)
            {
                return;
            }
            if(Player.WeaponEquipment.CurrentWeapon.Category == WeaponCategory.Sniper && !Settings.SettingsManager.SwaySettings.ApplySwayToSnipers)
            {
                return;
            }
            ApplySway();
            //Player.DebugString = $"P: {Math.Round(CurrentPitch,4)} AdjP: {Math.Round(AdjustedPitch,4)} H: {Math.Round(CurrentHeading,4)} AdjH: {Math.Round(AdjustedHeading,4)} HD: {HorizontalSwayDirection} VD: {VerticalSwayDirection} ~n~HC: {GameTimeLastHorizontalChangedSwayDirection}  VC: {GameTimeLastVerticalChangedSwayDirection} FA:{Game.LocalPlayer.IsFreeAiming} A: {Game.LocalPlayer.Character.IsAiming}";
        }
    }
    private void ApplySway()
    {
        Speed = Player.Character.Speed;
        UpdateDirection();
        CurrentPitch = NativeFunction.Natives.GET_GAMEPLAY_CAM_RELATIVE_PITCH<float>();
        AdjustPitch();
        if (Player.IsInVehicle)
        {
            AdjustedPitch *= -1.0f;
        }
        if (Settings.SettingsManager.SwaySettings.UseAlternateCalculation)//Player.IsInFirstPerson)
        {
            NativeFunction.Natives.SET_GAMEPLAY_CAM_RELATIVE_PITCH(CurrentPitch + AdjustedPitch, 1.0f);
        }
        else
        {
            NativeFunction.Natives.SET_GAMEPLAY_CAM_RELATIVE_PITCH(CurrentPitch + AdjustedPitch, Math.Abs(AdjustedPitch));
        }
        CurrentHeading = NativeFunction.Natives.GET_GAMEPLAY_CAM_RELATIVE_HEADING<float>();
        AdjustHeading();
        if (Math.Abs(AdjustedHeading) > 0f)
        {
            NativeFunction.Natives.SET_GAMEPLAY_CAM_RELATIVE_HEADING(CurrentHeading + AdjustedHeading);
        }
        //Player.DebugLine4 = $"Speed {Speed:N2} % Hor {PercentHorizontalSwayElapsed:P2} %Ver {PercentVeritcalSwayElapsed:P2} VerDIr {VerticalSwayDirection} HorDIr {HorizontalSwayDirection}";
    }
    private void UpdateDirection()
    {
        if (GameTimeLastHorizontalChangedSwayDirection == 0 || Game.GameTime - GameTimeLastHorizontalChangedSwayDirection >= GameTimeBetweenHorizontalSwayChanges)
        {
            int HorizontalMin;
            int HorizontalMax;
            if(Player.IsInVehicle || Speed <= 0.2f)
            {
                HorizontalMin = 500;
                HorizontalMax = 1000;
            }
            else if (Speed <= 2.0f)
            {
                HorizontalMin = 350;
                HorizontalMax = 650;
            }
            else
            {
                HorizontalMin = 250;
                HorizontalMax = 500;
            }
            HorizontalSwayDirection = RandomItems.RandomPercent(50);
            GameTimeBetweenHorizontalSwayChanges = RandomItems.GetRandomNumber(HorizontalMin, HorizontalMax); //RandomItems.GetRandomNumber(150, 250);
            GameTimeLastHorizontalChangedSwayDirection = Game.GameTime;
        }
        if (GameTimeLastVerticalChangedSwayDirection == 0 || Game.GameTime - GameTimeLastVerticalChangedSwayDirection >= GameTimeBetweenVerticalSwayChanges)
        {
            int VerticalMin;
            int VerticalMax;
            if (Player.IsInVehicle || Speed <= 0.2f)
            {
                VerticalMin = 500;
                VerticalMax = 1000;
            }
            else if (Speed <= 1.4f)
            {
                VerticalMin = 350;
                VerticalMax = 650;
            }
            else
            {
                VerticalMin = 250;
                VerticalMax = 500;
            }
            VerticalSwayDirection = RandomItems.RandomPercent(50);
            GameTimeBetweenVerticalSwayChanges = RandomItems.GetRandomNumber(VerticalMin, VerticalMax); //RandomItems.GetRandomNumber(150, 250);
            GameTimeLastVerticalChangedSwayDirection = Game.GameTime;
        }
    }
    private void AdjustPitch()
    {
        AdjustedPitch = RandomItems.GetRandomNumber(Player.WeaponEquipment.CurrentWeapon.MinVerticaSway, Player.WeaponEquipment.CurrentWeapon.MaxVerticaSway);
        AdjustedPitch *= Settings.SettingsManager.SwaySettings.VeritcalSwayAdjuster * 0.0075f * 20.0f * 1.25f;//want this to be near to 1.0 in the settings default;//Settings.SettingsManager.SwaySettings.VeritcalSwayAdjuster * 0.0075f * 20.0f * 1.25f;//want this to be near to 1.0 in the settings default;
        if (!VerticalSwayDirection)
        {
            AdjustedPitch *= -1.0f;
        }

        if(Player.IsInFirstPerson)
        {
            AdjustedPitch *= 0.1f;
            AdjustedPitch *= Settings.SettingsManager.SwaySettings.VeritcalFirstPersonSwayAdjuster;
        }


        if (Player.IsInVehicle)
        {       
            AdjustedPitch *= 0.1f;
            AdjustedPitch *= Settings.SettingsManager.SwaySettings.VeritcalInVehicleSwayAdjuster;
        }
        else
        {
            if(Speed >= 2.0f)
            {
                AdjustedPitch *= 2.0f;
            }
            else if (Speed >= 0.2f)
            {
                AdjustedPitch *= Speed;
            }
            AdjustedPitch *= Settings.SettingsManager.SwaySettings.VeritcalOnFootSwayAdjuster;
        }
        uint SwayTime = Game.GameTime - GameTimeLastVerticalChangedSwayDirection;
        if (SwayTime >= GameTimeBetweenVerticalSwayChanges)
        {
            PercentVeritcalSwayElapsed = 1.0f;
        }
        else
        {
            PercentVeritcalSwayElapsed = (float)SwayTime / (float)GameTimeBetweenVerticalSwayChanges;
        }

        if(PercentVeritcalSwayElapsed > 0.5f)
        {
            PercentVeritcalSwayElapsed = 1.0f - PercentVeritcalSwayElapsed;
        }
        AdjustedPitch *= PercentVeritcalSwayElapsed * 2.0f;
    }
    private void AdjustHeading()
    {
        AdjustedHeading = RandomItems.GetRandomNumber(Player.WeaponEquipment.CurrentWeapon.MinHorizontalSway, Player.WeaponEquipment.CurrentWeapon.MaxHorizontalSway);
        AdjustedHeading *= Settings.SettingsManager.SwaySettings.HorizontalSwayAdjuster * 0.0075f * 2.0f * 1.25f;//want this to be near to 1.0 in the settings default;
        if (!HorizontalSwayDirection)
        {
            AdjustedHeading *= -1.0f;
        }

        if(Player.IsInFirstPerson)
        {
            AdjustedHeading *= Settings.SettingsManager.SwaySettings.HorizontalFirstPersonSwayAdjuster;
        }

        if (Player.IsInVehicle)
        {
            AdjustedHeading *= 3.0f;
            AdjustedHeading *= Settings.SettingsManager.SwaySettings.HorizontalInVehicleSwayAdjuster;
        }
        else
        {
            if (Speed >= 2.0f)
            {
                AdjustedHeading *= 2.0f * 2.0f;
            }
            else if (Speed >= 0.2f)
            {
                AdjustedHeading *= Speed * 2.0f;
            }
            AdjustedHeading *= Settings.SettingsManager.SwaySettings.HorizontalOnFootSwayAdjuster;
        }
        uint SwayTime = Game.GameTime - GameTimeLastHorizontalChangedSwayDirection;
        if (SwayTime >= GameTimeBetweenHorizontalSwayChanges)
        {
            PercentHorizontalSwayElapsed = 1.0f;
        }
        else
        {
            PercentHorizontalSwayElapsed = (float)SwayTime / (float)GameTimeBetweenHorizontalSwayChanges;
        }
        if (PercentHorizontalSwayElapsed > 0.5f)
        {
            PercentHorizontalSwayElapsed = 1.0f - PercentHorizontalSwayElapsed;
        }
        AdjustedHeading *= PercentHorizontalSwayElapsed * 2.0f;
    }
    private enum SwayDirection
    {
        None,
        Positive,
        Negative,
    }
}