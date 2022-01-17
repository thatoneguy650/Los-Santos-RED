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
        if (Settings.SettingsManager.SwaySettings.ApplySway && Player.CurrentWeapon != null)// && !IsInVehicle)
        {
            if (Player.CurrentWeapon.Category == WeaponCategory.Throwable || Player.CurrentWeapon.Category == WeaponCategory.Vehicle || Player.CurrentWeapon.Category == WeaponCategory.Melee || Player.CurrentWeapon.Category == WeaponCategory.Misc || Player.CurrentWeapon.Category == WeaponCategory.Unknown)
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
            ApplySway();
        }
    }
    private void ApplySway()
    {
        Speed = Player.Character.Speed;
        UpdateDirection();

        CurrentPitch = NativeFunction.Natives.GET_GAMEPLAY_CAM_RELATIVE_PITCH<float>();
        AdjustPitch();
        if (Player.IsInVehicle && !VerticalSwayDirection)
        {
            CurrentPitch *= -1.0f;//weird shit needed for in the car for some reason....
        }
        NativeFunction.Natives.SET_GAMEPLAY_CAM_RELATIVE_PITCH(CurrentPitch + AdjustedPitch, Math.Abs(AdjustedPitch));

        CurrentHeading = NativeFunction.Natives.GET_GAMEPLAY_CAM_RELATIVE_HEADING<float>();
        AdjustHeading();
        NativeFunction.Natives.SET_GAMEPLAY_CAM_RELATIVE_HEADING(CurrentHeading + AdjustedHeading);

        Player.DebugLine4 = $"Speed {Speed:N2} % Hor {PercentHorizontalSwayElapsed:P2} %Ver {PercentVeritcalSwayElapsed:P2} VerDIr {VerticalSwayDirection} HorDIr {HorizontalSwayDirection}";
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
        AdjustedPitch = RandomItems.GetRandomNumber(Player.CurrentWeapon.MinVerticaSway, Player.CurrentWeapon.MaxVerticaSway);
        AdjustedPitch *= Settings.SettingsManager.SwaySettings.VeritcalSwayAdjuster * 0.0075f * 20.0f * 1.25f;//want this to be near to 1.0 in the settings default;
        if (!VerticalSwayDirection)
        {
            AdjustedPitch *= -1.0f;
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
        AdjustedHeading = RandomItems.GetRandomNumber(Player.CurrentWeapon.MinHorizontalSway, Player.CurrentWeapon.MaxHorizontalSway);
        AdjustedHeading *= Settings.SettingsManager.SwaySettings.HorizontalSwayAdjuster * 0.0075f * 1.25f;//want this to be near to 1.0 in the settings default;
        if (!HorizontalSwayDirection)
        {
            AdjustedHeading *= -1.0f;
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


//private void SwayOnFoot()
//{
//    if (GameTimeLastHorizontalChangedSwayDirection == 0 || Game.GameTime - GameTimeLastHorizontalChangedSwayDirection >= GameTimeBetweenHorizontalSwayChanges)
//    {
//        HorizontalSwayDirection = RandomItems.RandomPercent(50);
//        GameTimeBetweenHorizontalSwayChanges = RandomItems.GetRandomNumber(200, 275); //RandomItems.GetRandomNumber(150, 250);
//        GameTimeLastHorizontalChangedSwayDirection = Game.GameTime;
//    }


//    if (GameTimeLastVerticalChangedSwayDirection == 0 || Game.GameTime - GameTimeLastVerticalChangedSwayDirection >= GameTimeBetweenVerticalSwayChanges)
//    {
//        VerticalSwayDirection = RandomItems.RandomPercent(50);
//        GameTimeBetweenVerticalSwayChanges = RandomItems.GetRandomNumber(200, 275); //RandomItems.GetRandomNumber(150, 250);
//        GameTimeLastVerticalChangedSwayDirection = Game.GameTime;
//    }
//    float currentPitch = NativeFunction.Natives.GET_GAMEPLAY_CAM_RELATIVE_PITCH<float>();
//    float AdjustedPitch = RandomItems.GetRandomNumber(Player.CurrentWeapon.MinVerticaSway, Player.CurrentWeapon.MaxVerticaSway);//RandomItems.GetRandomNumber(0.01f, 0.05f);//RandomItems.GetRandomNumber(0.02f, 0.05f);
//    AdjustedPitch *= Settings.SettingsManager.PlayerSettings.VeritcalSwayAdjuster * 0.0075f * 20.0f;//want this to be near to 1.0 in the settings default;
//    if (!VerticalSwayDirection)
//    {
//        AdjustedPitch *= -1.0f;
//    }
//    float speed = Player.Character.Speed;
//    if (speed >= 0.2f)
//    {
//        AdjustedPitch *= speed;
//    }
//    EntryPoint.WriteToConsole($"AdjustedPitch 1 currentPitch: {currentPitch} AdjustedPitch: {AdjustedPitch} VerticalSwayDirection {VerticalSwayDirection}", 5);
//    NativeFunction.Natives.SET_GAMEPLAY_CAM_RELATIVE_PITCH(currentPitch + AdjustedPitch, Math.Abs(AdjustedPitch));
//    currentPitch = NativeFunction.Natives.GET_GAMEPLAY_CAM_RELATIVE_PITCH<float>();
//    EntryPoint.WriteToConsole($"AdjustedPitch 2 currentPitch: {currentPitch} AdjustedPitch: {AdjustedPitch} VerticalSwayDirection {VerticalSwayDirection}", 5);
//    float currentHeading = NativeFunction.Natives.GET_GAMEPLAY_CAM_RELATIVE_HEADING<float>();
//    float AdjustedHeading = RandomItems.GetRandomNumber(Player.CurrentWeapon.MinHorizontalSway, Player.CurrentWeapon.MaxHorizontalSway); //RandomItems.GetRandomNumber(0.02f, 0.05f);//RandomItems.GetRandomNumber(0.002f, 0.005f); //0.1f;//RandomItems.GetRandomNumber(CurrentWeapon.MinHorizontalRecoil, CurrentWeapon.MaxHorizontalRecoil);            
//    if (!HorizontalSwayDirection)
//    {
//        AdjustedHeading *= -1.0f;
//    }
//    if (speed >= 0.2f)
//    {
//        AdjustedHeading *= speed;
//    }
//    AdjustedHeading *= Settings.SettingsManager.PlayerSettings.HorizontalSwayAdjuster * 0.0075f;//want this to be near to 1.0 in the settings default;
//    NativeFunction.Natives.SET_GAMEPLAY_CAM_RELATIVE_HEADING(currentHeading + AdjustedHeading);
//}
//private void SwayInVehicle()
//{
//    if (GameTimeLastHorizontalChangedSwayDirection == 0 || Game.GameTime - GameTimeLastHorizontalChangedSwayDirection >= GameTimeBetweenHorizontalSwayChanges)
//    {
//        HorizontalSwayDirection = RandomItems.RandomPercent(50);
//        GameTimeBetweenHorizontalSwayChanges = RandomItems.GetRandomNumber(200, 275); //RandomItems.GetRandomNumber(150, 250);
//        GameTimeLastHorizontalChangedSwayDirection = Game.GameTime;
//    }
//    if (GameTimeLastVerticalChangedSwayDirection == 0 || Game.GameTime - GameTimeLastVerticalChangedSwayDirection >= GameTimeBetweenVerticalSwayChanges)
//    {
//        VerticalSwayDirection = RandomItems.RandomPercent(50);
//        GameTimeBetweenVerticalSwayChanges = RandomItems.GetRandomNumber(200, 275); //RandomItems.GetRandomNumber(150, 250);
//        GameTimeLastVerticalChangedSwayDirection = Game.GameTime;
//    }
//    float currentPitch = NativeFunction.Natives.GET_GAMEPLAY_CAM_RELATIVE_PITCH<float>();


//    //currentPitch = Math.Abs(currentPitch);


//    //Vector3 CurrentRotation = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(0);

//    //float currentPitch2 = CurrentRotation.X;


//    float AdjustedPitch = RandomItems.GetRandomNumber(Player.CurrentWeapon.MinVerticaSway, Player.CurrentWeapon.MaxVerticaSway);//RandomItems.GetRandomNumber(0.01f, 0.05f);//RandomItems.GetRandomNumber(0.02f, 0.05f);
//    AdjustedPitch *= Settings.SettingsManager.PlayerSettings.VeritcalSwayAdjuster * 0.0075f * 20.0f;//want this to be near to 1.0 in the settings default;
//    if (!VerticalSwayDirection)
//    {
//        AdjustedPitch *= -1.0f;
//        currentPitch *= -1.0f;
//    }
//    AdjustedPitch *= 0.1f;
//    EntryPoint.WriteToConsole($"AdjustedPitch 1 In Vehicle currentPitch: {currentPitch} AdjustedPitch: {AdjustedPitch} VerticalSwayDirection {VerticalSwayDirection}", 5);
//    NativeFunction.Natives.SET_GAMEPLAY_CAM_RELATIVE_PITCH(currentPitch + AdjustedPitch, Math.Abs(AdjustedPitch));
//    currentPitch = NativeFunction.Natives.GET_GAMEPLAY_CAM_RELATIVE_PITCH<float>();
//    EntryPoint.WriteToConsole($"AdjustedPitch 2 In Vehicle currentPitch: {currentPitch} AdjustedPitch: {AdjustedPitch} VerticalSwayDirection {VerticalSwayDirection}", 5);




//    // NativeFunction.Natives.x759E13EBC1C15C5A(currentPitch + AdjustedPitch);



//    float currentHeading = NativeFunction.Natives.GET_GAMEPLAY_CAM_RELATIVE_HEADING<float>();
//    float AdjustedHeading = RandomItems.GetRandomNumber(Player.CurrentWeapon.MinHorizontalSway, Player.CurrentWeapon.MaxHorizontalSway); //RandomItems.GetRandomNumber(0.02f, 0.05f);//RandomItems.GetRandomNumber(0.002f, 0.005f); //0.1f;//RandomItems.GetRandomNumber(CurrentWeapon.MinHorizontalRecoil, CurrentWeapon.MaxHorizontalRecoil);            
//    if (!HorizontalSwayDirection)
//    {
//        AdjustedHeading *= -1.0f;
//    }
//    AdjustedHeading *= 3.0f;
//    AdjustedHeading *= Settings.SettingsManager.PlayerSettings.HorizontalSwayAdjuster * 0.0075f;//want this to be near to 1.0 in the settings default;
//    NativeFunction.Natives.SET_GAMEPLAY_CAM_RELATIVE_HEADING(currentHeading + AdjustedHeading);
//}