using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class WeatherSettings : ISettingsDefaultable
{
    [Description("Allows displaying a notification and or audio when the weather changes.")]
    public bool ReportWeather { get; set; }
    [Description("Minimum time between weather changed notifications.")]
    public uint ReportWeather_MinimumTimeBetweenReports { get; set; }
    [Description("Minimum time between weather changed audio notifications. (Currently Disabled)")]
    public uint ReportWeather_MinimumTimeBetweenAudioReports { get; set; }
    [Description("Will only report when the current weather changes.")]
    public bool ReportChangedCurrentWeather { get; set; }
    [Description("Will only report when the forecasted weather changes.")]
    public bool ReportChangedForecastedWeather { get; set; }
    [Description("Minimum time between windy weather notifications.")]
    public uint ReportWindyWeather_MinimumTimeBetweenReports { get; set; }
    [Description("Minimum time between windy weather audio notifications. (Currently Disabled)")]
    public uint ReportWindyWeather_MinimumTimeBetweenAudioReports { get; set; }
    [Description("Minimum wind speed in meters per second required to be considered windy.")]
    public float ReportWindyWeather_MinimumSpeed { get; set; }
    [Description("Show a notification when the weather changes.")]
    public bool ShowWeatherNotifications { get; set; }
    [Description("(Currently Disabled)")]
    public bool ChangeWeatherByForecast { get; set; }
    [Description("(Currently Disabled)")]
    public float StartChangeWeatherPrcentage { get; set; }
    [Description("(Currently Disabled)")]
    public float ChangeWeatherPrcentage { get; set; }

    public WeatherSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        ReportWeather = true;
        ReportWeather_MinimumTimeBetweenReports = 10000;
        ReportWeather_MinimumTimeBetweenAudioReports = 180000;
        ReportChangedCurrentWeather = false;
        ReportChangedForecastedWeather = true;
        ReportWindyWeather_MinimumTimeBetweenReports = 120000;
        ReportWindyWeather_MinimumTimeBetweenAudioReports = 180000;
        ReportWindyWeather_MinimumSpeed = 11.5f;
        ShowWeatherNotifications = true;

        ChangeWeatherByForecast = false;
        StartChangeWeatherPrcentage = 0.01f;
        ChangeWeatherPrcentage = 0.03f;
    }
}

