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
        if (Settings.SettingsManager.PlayerSettings.ApplySway && Player.CurrentWeapon != null)// && !IsInVehicle)
        {
            if (Player.CurrentWeapon.Category == WeaponCategory.Throwable || Player.CurrentWeapon.Category == WeaponCategory.Vehicle || Player.CurrentWeapon.Category == WeaponCategory.Melee || Player.CurrentWeapon.Category == WeaponCategory.Misc || Player.CurrentWeapon.Category == WeaponCategory.Unknown)
            {
                return;
            }
            if (Player.IsInVehicle && !Settings.SettingsManager.PlayerSettings.ApplySwayInVehicle)
            {
                return;
            }
            if(Player.IsInVehicle)
            {
                SwayInVehicle();
            }
            else
            {
                SwayOnFoot();
            }
            
        }
    }
    private void SwayOnFoot()
    {
        if (GameTimeLastHorizontalChangedSwayDirection == 0 || Game.GameTime - GameTimeLastHorizontalChangedSwayDirection >= GameTimeBetweenHorizontalSwayChanges)
        {
            HorizontalSwayDirection = RandomItems.RandomPercent(50);
            GameTimeBetweenHorizontalSwayChanges = RandomItems.GetRandomNumber(200, 275); //RandomItems.GetRandomNumber(150, 250);
            GameTimeLastHorizontalChangedSwayDirection = Game.GameTime;
        }


        if (GameTimeLastVerticalChangedSwayDirection == 0 || Game.GameTime - GameTimeLastVerticalChangedSwayDirection >= GameTimeBetweenVerticalSwayChanges)
        {
            VerticalSwayDirection = RandomItems.RandomPercent(50);
            GameTimeBetweenVerticalSwayChanges = RandomItems.GetRandomNumber(200, 275); //RandomItems.GetRandomNumber(150, 250);
            GameTimeLastVerticalChangedSwayDirection = Game.GameTime;
        }
        float currentPitch = NativeFunction.Natives.GET_GAMEPLAY_CAM_RELATIVE_PITCH<float>();
        float AdjustedPitch = RandomItems.GetRandomNumber(Player.CurrentWeapon.MinVerticaSway, Player.CurrentWeapon.MaxVerticaSway);//RandomItems.GetRandomNumber(0.01f, 0.05f);//RandomItems.GetRandomNumber(0.02f, 0.05f);
        AdjustedPitch *= Settings.SettingsManager.PlayerSettings.VeritcalSwayAdjuster * 0.0075f * 20.0f;//want this to be near to 1.0 in the settings default;
        if (!VerticalSwayDirection)
        {
            AdjustedPitch *= -1.0f;
        }
        float speed = Player.Character.Speed;
        if (speed >= 0.2f)
        {
            AdjustedPitch *= speed;
        }
        //EntryPoint.WriteToConsole($"AdjustedPitch {AdjustedPitch}", 5);
        EntryPoint.WriteToConsole($"AdjustedPitch 1 currentPitch: {currentPitch} AdjustedPitch: {AdjustedPitch} VerticalSwayDirection {VerticalSwayDirection}", 5);
        NativeFunction.Natives.SET_GAMEPLAY_CAM_RELATIVE_PITCH(currentPitch + AdjustedPitch, Math.Abs(AdjustedPitch));
        currentPitch = NativeFunction.Natives.GET_GAMEPLAY_CAM_RELATIVE_PITCH<float>();
        EntryPoint.WriteToConsole($"AdjustedPitch 2 currentPitch: {currentPitch} AdjustedPitch: {AdjustedPitch} VerticalSwayDirection {VerticalSwayDirection}", 5);

        float currentHeading = NativeFunction.Natives.GET_GAMEPLAY_CAM_RELATIVE_HEADING<float>();
        float AdjustedHeading = RandomItems.GetRandomNumber(Player.CurrentWeapon.MinHorizontalSway, Player.CurrentWeapon.MaxHorizontalSway); //RandomItems.GetRandomNumber(0.02f, 0.05f);//RandomItems.GetRandomNumber(0.002f, 0.005f); //0.1f;//RandomItems.GetRandomNumber(CurrentWeapon.MinHorizontalRecoil, CurrentWeapon.MaxHorizontalRecoil);            
        if (!HorizontalSwayDirection)
        {
            AdjustedHeading *= -1.0f;
        }
        if (speed >= 0.2f)
        {
            AdjustedHeading *= speed;
        }   
        AdjustedHeading *= Settings.SettingsManager.PlayerSettings.HorizontalSwayAdjuster * 0.0075f;//want this to be near to 1.0 in the settings default;
        NativeFunction.Natives.SET_GAMEPLAY_CAM_RELATIVE_HEADING(currentHeading + AdjustedHeading);
    }
    private void SwayInVehicle()
    {
        if (GameTimeLastHorizontalChangedSwayDirection == 0 || Game.GameTime - GameTimeLastHorizontalChangedSwayDirection >= GameTimeBetweenHorizontalSwayChanges)
        {
            HorizontalSwayDirection = RandomItems.RandomPercent(50);
            GameTimeBetweenHorizontalSwayChanges = RandomItems.GetRandomNumber(200, 275); //RandomItems.GetRandomNumber(150, 250);
            GameTimeLastHorizontalChangedSwayDirection = Game.GameTime;
        }
        if (GameTimeLastVerticalChangedSwayDirection == 0 || Game.GameTime - GameTimeLastVerticalChangedSwayDirection >= GameTimeBetweenVerticalSwayChanges)
        {
            VerticalSwayDirection = RandomItems.RandomPercent(50);
            GameTimeBetweenVerticalSwayChanges = RandomItems.GetRandomNumber(200, 275); //RandomItems.GetRandomNumber(150, 250);
            GameTimeLastVerticalChangedSwayDirection = Game.GameTime;
        }
        float currentPitch = NativeFunction.Natives.GET_GAMEPLAY_CAM_RELATIVE_PITCH<float>();


        //currentPitch = Math.Abs(currentPitch);


        //Vector3 CurrentRotation = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(0);

        //float currentPitch2 = CurrentRotation.X;


        float AdjustedPitch = RandomItems.GetRandomNumber(Player.CurrentWeapon.MinVerticaSway, Player.CurrentWeapon.MaxVerticaSway);//RandomItems.GetRandomNumber(0.01f, 0.05f);//RandomItems.GetRandomNumber(0.02f, 0.05f);
        AdjustedPitch *= Settings.SettingsManager.PlayerSettings.VeritcalSwayAdjuster * 0.0075f * 20.0f;//want this to be near to 1.0 in the settings default;
        if (!VerticalSwayDirection)
        {
            AdjustedPitch *= -1.0f;
            currentPitch *= -1.0f;
        }
        AdjustedPitch *= 0.1f;   
        EntryPoint.WriteToConsole($"AdjustedPitch 1 In Vehicle currentPitch: {currentPitch} AdjustedPitch: {AdjustedPitch} VerticalSwayDirection {VerticalSwayDirection}", 5);
        NativeFunction.Natives.SET_GAMEPLAY_CAM_RELATIVE_PITCH(currentPitch + AdjustedPitch, Math.Abs(AdjustedPitch));
        currentPitch = NativeFunction.Natives.GET_GAMEPLAY_CAM_RELATIVE_PITCH<float>();
        EntryPoint.WriteToConsole($"AdjustedPitch 2 In Vehicle currentPitch: {currentPitch} AdjustedPitch: {AdjustedPitch} VerticalSwayDirection {VerticalSwayDirection}", 5);




        // NativeFunction.Natives.x759E13EBC1C15C5A(currentPitch + AdjustedPitch);



        float currentHeading = NativeFunction.Natives.GET_GAMEPLAY_CAM_RELATIVE_HEADING<float>();
        float AdjustedHeading = RandomItems.GetRandomNumber(Player.CurrentWeapon.MinHorizontalSway, Player.CurrentWeapon.MaxHorizontalSway); //RandomItems.GetRandomNumber(0.02f, 0.05f);//RandomItems.GetRandomNumber(0.002f, 0.005f); //0.1f;//RandomItems.GetRandomNumber(CurrentWeapon.MinHorizontalRecoil, CurrentWeapon.MaxHorizontalRecoil);            
        if (!HorizontalSwayDirection)
        {
            AdjustedHeading *= -1.0f;
        }
        AdjustedHeading *= 3.0f;
        AdjustedHeading *= Settings.SettingsManager.PlayerSettings.HorizontalSwayAdjuster * 0.0075f;//want this to be near to 1.0 in the settings default;
        NativeFunction.Natives.SET_GAMEPLAY_CAM_RELATIVE_HEADING(currentHeading + AdjustedHeading);
    }
    private enum SwayDirection
    {
        None,
        Positive,
        Negative,
    }
}
