using LosSantosRED.lsr.Interface;
using Mod;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class StealthManager
{
    private IStealthManageable Player;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private ITimeControllable TimeControllable;
    private bool IsConsideredNight;
    private bool IsPoorWeather;
    private bool IsHidingInUnknownOffVehicle;
    private bool IsDoingActivityWithoutCrime;
    private bool IsHidingInUnknownVehicle;
    public StealthManager(Player player, IEntityProvideable world, ISettingsProvideable settings, ITimeControllable timeControllable)
    {
        Player = player;
        World = world;
        Settings = settings;
        TimeControllable = timeControllable;
    }
    public void Dispose()
    {

    }    
    public void Setup()
    {

    }
    public void Update()
    {
        //Updated at player update speed, not run each time a ped needs the current number
        if (Player.IsInVehicle && Player.CurrentVehicle != null && !Player.CurrentVehicle.IsWanted && !Player.CurrentVehicle.HasBeenSeenByPoliceDuringWanted)
        {
            IsHidingInUnknownOffVehicle = Player.IsDuckingInVehicle && !Player.CurrentVehicle.Engine.IsRunning;
            IsHidingInUnknownVehicle = !IsHidingInUnknownOffVehicle && Player.IsDuckingInVehicle;   
        }
        else
        {
            IsHidingInUnknownOffVehicle = false;
            IsHidingInUnknownVehicle = false;
        }
        IsConsideredNight = TimeControllable.IsNight;
        IsPoorWeather = Player.Weather != null && Player.Weather.IsPoorWeather;
        IsDoingActivityWithoutCrime = Player.ActivityManager.IsPerformingActivity && !Player.Violations.IsViolatingAnyCrimes;
        //if (TimeControllable.CurrentHour >= 18 || TimeControllable.CurrentHour <= 5)
        //{
        //    IsConsideredNight = true;
        //}
        //else
        //{
        //    IsConsideredNight = false;
        //}

        // EntryPoint.WriteToConsole($"PLAYER UPDATE: GetRecognizeTime{GetRecognizeTime(null)}  GetSightDistanceScalar{GetSightDistanceScalar(null)}");


        //Player.DebugString = $"Dist: {GetSightDistanceScalar(null)}% Time:{GetRecognizeTime(null)} CarSeen:{Player.CurrentVehicle?.HasBeenSeenByPoliceDuringWanted}";

    }
    public uint GetRecognizeTime(PedExt pedExt)
    {
        uint RecognizeTime = Settings.SettingsManager.CivilianSettings.BaseRecognizeTime;//1200;
        if (IsPoorWeather)
        {
            RecognizeTime += Settings.SettingsManager.CivilianSettings.PoorWeatherRecognizeIncreaseTime;// 500;
        }
        if (IsConsideredNight)
        {
            RecognizeTime += Settings.SettingsManager.CivilianSettings.NightTimeRecognizeIncreaseTime;// 500;
        }
        if(IsHidingInUnknownOffVehicle)
        {
            RecognizeTime += Settings.SettingsManager.CivilianSettings.HidingInVehicleRecognizeIncreaseTime;// 700;
        }
        if(IsDoingActivityWithoutCrime)
        {
            RecognizeTime += Settings.SettingsManager.CivilianSettings.ActivityWithoutViolationRecognizeIncreaseTime;
        }
        if (IsHidingInUnknownVehicle)
        {
            RecognizeTime += Settings.SettingsManager.CivilianSettings.HidingInUnknownVehicleRecognizeIncreaseTime;
        }
        if (pedExt != null && !Player.IsInVehicle)
        {
            if(pedExt.EverSeenPlayer)
            {
                RecognizeTime -= Settings.SettingsManager.CivilianSettings.SeenPlayerRecognizeDecreaseTime;
            }
            else if (!pedExt.EverSeenPlayer && pedExt.ClosestDistanceToPlayer >= 25f)
            {
                RecognizeTime += Settings.SettingsManager.CivilianSettings.NeverSeenPlayerRecognizeIncreaseTime;
            }
        }
        return RecognizeTime;
    }
    public float GetSightDistanceScalar(PedExt pedExt)
    {

        float expectedSightDistance = 1.0f;
        if (IsHidingInUnknownOffVehicle)
        {
            expectedSightDistance *= Settings.SettingsManager.CivilianSettings.HidingInVehicleSightDecreasePercentage;
        }
        if (IsConsideredNight)
        {
            expectedSightDistance *= Settings.SettingsManager.CivilianSettings.NightTimeSightDecreasePercentage;
        }
        if (IsPoorWeather)
        {
            expectedSightDistance *= Settings.SettingsManager.CivilianSettings.PoorWeatherSightDecreasePercentage;
        }
        if(IsDoingActivityWithoutCrime)
        {
            expectedSightDistance *= Settings.SettingsManager.CivilianSettings.ActivityWithoutViolationSightDecreasePercentage;
        }
        if (IsHidingInUnknownVehicle)
        {
            expectedSightDistance *= Settings.SettingsManager.CivilianSettings.HidingInUnknownVehicleSightDecreasePercentage;
        }
        if (pedExt != null && !pedExt.EverSeenPlayer && pedExt.ClosestDistanceToPlayer >= 25f && !Player.IsInVehicle)
        {
            expectedSightDistance *= Settings.SettingsManager.CivilianSettings.NeverSeenPlayerSightDecreasePercentage;
        }
        return expectedSightDistance;
    }
}

