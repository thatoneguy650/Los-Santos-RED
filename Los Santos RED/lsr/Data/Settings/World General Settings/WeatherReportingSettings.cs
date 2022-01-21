using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class WeatherReportingSettings : ISettingsDefaultable
{

    public bool ReportWeather { get; set; }
    public uint ReportWeather_MinimumTimeBetweenReports { get; set; }
    public uint ReportWeather_MinimumTimeBetweenAudioReports { get; set; }
    public bool ReportChangedCurrentWeather { get; set; }
    public bool ReportChangedForecastedWeather { get; set; }
    public uint ReportWindyWeather_MinimumTimeBetweenReports { get; set; }
    public uint ReportWindyWeather_MinimumTimeBetweenAudioReports { get; set; }
    public float ReportWindyWeather_MinimumSpeed { get; set; }
    public bool ShowWeatherNotifications { get; set; }
    public bool PlayWeatherAudio { get; set; }
    public bool RequireVehicleForAudio { get; set; }
    public bool WeatherAudio_MuteRadio { get; set; }
    public bool PlayWeatherAudioInVehicleOnly { get; set; }
    public WeatherReportingSettings()
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
        PlayWeatherAudio = false;
        RequireVehicleForAudio = true;
        WeatherAudio_MuteRadio = true;
        PlayWeatherAudioInVehicleOnly = true;
    }
}

