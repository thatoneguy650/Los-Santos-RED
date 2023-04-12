using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


public class WeatherManager 
{
    private bool EverSetWeather = false;
    private ISettingsProvideable Settings;
    private ITimeReportable Time;
    private IWeatherForecasts WeatherForecasts;
    private uint GameTimeLastCheckedWeather;
    private uint GameTimeBetweenWeatherChecks = 10000;


    private eWeatherTypeHash CurrentSetWeather = eWeatherTypeHash.None;
    private WeatherForecast closestForecast;
    private float CurrentSetRainLevel = 0.0f;
    private float CurrentSetWindSpeed = 0.0f;
    private float CurrentSetWindDirection = 0.0f;


    private uint GameTimeStartedTransitioningWeather;
    private float TransitionToWeatherPercent;
    private bool IsTransitioningWeather;
    private eWeatherTypeHash TransitionToWeather;
    private eWeatherTypeHash TransitionFromWeather = eWeatherTypeHash.Clear;
    private uint GameTimeLastSetWeatherTransition;
    private bool ShouldCheckWeather => Game.GameTime - GameTimeLastCheckedWeather >= GameTimeBetweenWeatherChecks;
    public WeatherManager(ISettingsProvideable settings, ITimeReportable time, IWeatherForecasts weatherForecasts)
    {
        Settings = settings;
        Time = time;
        WeatherForecasts = weatherForecasts;
    }
    public void Setup()
    {
        try
        {

        }
        catch (ArgumentOutOfRangeException ex)
        {
            Game.DisplayNotification($"Error Starting Mod, Time Set Incorrect, Setting to Current {ex.Message}");
        }
    }
    public void Dispose()
    {
        IsTransitioningWeather = false;
        if (EverSetWeather)
        {
            NativeFunction.Natives.CLEAR_WEATHER_TYPE_PERSIST();
            NativeFunction.Natives.SET_RANDOM_WEATHER_TYPE();
            NativeFunction.Natives.x643E26EA6E024D92(-1.0f);
            NativeFunction.Natives.SET_WIND_SPEED(-1.0f);
            NativeFunction.Natives.SET_WIND_DIRECTION(1.0f);
        }
        EverSetWeather = false;
    }      
    public void Update()
    {

        return;

        if(Settings.SettingsManager.WeatherSettings.ChangeWeatherByForecast && ShouldCheckWeather)
        {
            DateTime currentOffsetDateTime = new DateTime(2020, Time.CurrentDateTime.Month, Time.CurrentDateTime.Day, Time.CurrentDateTime.Hour, Time.CurrentDateTime.Minute, Time.CurrentDateTime.Second);
            closestForecast = WeatherForecasts.WeatherForecastList.OrderBy(x => Math.Abs(x.DateTime.Ticks - currentOffsetDateTime.Ticks)).ThenBy(x=> x.DateTime).FirstOrDefault();//WeatherForecasts.WeatherForecastList.OrderBy(x => (x.DateTime - currentOffsetDateTime).Duration()).ThenBy(y=>y.DateTime).FirstOrDefault();
            if (closestForecast != null)
            {
                Game.DisplaySubtitle($"Time is {Time.CurrentDateTime} and the closest forcast is {closestForecast.DateTime} {closestForecast.AirTemperature} F {closestForecast.Description} ");
                UpdateWeather();
            }
            GameTimeLastCheckedWeather = Game.GameTime;
        }
        UpdateWeatherTransition();
    }
    private void UpdateWeather()
    {
        EverSetWeather = true;
        if (CurrentSetWeather != closestForecast.GTAWeather)
        {
            //EntryPoint.WriteToConsoleTestLong($"Updating Weather to {closestForecast.GTAWeather} from {CurrentSetWeather}");



            TransitionToNewWeather();

            //NativeFunction.Natives.SET_WEATHER_TYPE_NOW(closestForecast.GTAWeatherType);






            CurrentSetWeather = closestForecast.GTAWeather;
        }
        if (CurrentSetRainLevel != closestForecast.RainLevel)
        {
            //EntryPoint.WriteToConsoleTestLong($"Updating Rain to {closestForecast.RainLevel} from {CurrentSetRainLevel}");

            NativeFunction.Natives.x643E26EA6E024D92(closestForecast.RainLevel);
            CurrentSetRainLevel = closestForecast.RainLevel;
        }
        if (CurrentSetWindSpeed != closestForecast.WindSpeedMetersPerSecond)
        {
            //EntryPoint.WriteToConsoleTestLong($"Updating WindSpeed to {closestForecast.WindSpeedMetersPerSecond} from {CurrentSetWindSpeed}");

            NativeFunction.Natives.SET_WIND_SPEED(closestForecast.WindSpeedMetersPerSecond);

            CurrentSetWindSpeed = closestForecast.WindSpeed;
        }
        if(CurrentSetWindDirection != closestForecast.WindDirectionRadians)
        {
            //EntryPoint.WriteToConsoleTestLong($"Updating WindDirection to {closestForecast.WindDirectionRadians} from {CurrentSetWindDirection}");

            NativeFunction.Natives.SET_WIND_DIRECTION(closestForecast.WindDirectionRadians);

            CurrentSetWindDirection = closestForecast.WindDirection;
        }
    }

    private void TransitionToNewWeather()
    {
        TransitionToWeather = closestForecast.GTAWeather;
        TransitionFromWeather = CurrentSetWeather;
        GameTimeStartedTransitioningWeather = Game.GameTime;
        GameTimeLastSetWeatherTransition = Game.GameTime;
        TransitionToWeatherPercent = Settings.SettingsManager.WeatherSettings.StartChangeWeatherPrcentage;
        IsTransitioningWeather = true;
        NativeFunction.Natives.SET_CURR_WEATHER_STATE((uint)TransitionFromWeather, (uint)TransitionToWeather, TransitionToWeatherPercent);
        //EntryPoint.WriteToConsoleTestLong($"Started Weather Transition from:{TransitionFromWeather} to {TransitionToWeather} percent {TransitionToWeatherPercent}");
    }
    private void UpdateWeatherTransition()
    {
        if(IsTransitioningWeather)
        {
            if(TransitionToWeatherPercent >= 1.0f)
            {
                IsTransitioningWeather = false;
                //EntryPoint.WriteToConsoleTestLong($"Stopped Weather Transition");
            }
            else
            {
                if (Game.GameTime - GameTimeLastSetWeatherTransition >= 1000)
                {
                    TransitionToWeatherPercent += Settings.SettingsManager.WeatherSettings.ChangeWeatherPrcentage;
                    GameTimeLastSetWeatherTransition = Game.GameTime;
                    NativeFunction.Natives.SET_CURR_WEATHER_STATE((uint)TransitionFromWeather, (uint)TransitionToWeather, TransitionToWeatherPercent);
                    //EntryPoint.WriteToConsoleTestLong($"Updated Weather Transition from:{TransitionFromWeather} to {TransitionToWeather} percent {TransitionToWeatherPercent}");
                }
            }
        }
    }
}
