using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WorldSettings
{
    public bool AddPOIBlipsToMap { get; set; } = true;
    public bool UpdateVehiclePlates { get; set; } = true;
    public bool CleanupVehicles { get; set; } = true;
    public bool ReportWeather { get; set; } = true;
    public uint ReportWeather_MinimumTimeBetweenReports { get; set; } = 60000;
    public bool ReportChangedCurrentWeather { get; set; } = false;
    public bool ReportChangedForecastedWeather { get; set; } = true;
    public uint ReportWindyWeather_MinimumTimeBetweenReports { get; set; } = 180000;
    public float ReportWindyWeather_MinimumSpeed { get; set; } = 11.5f;
    public bool ShowWeatherNotifications { get; set; } = true;
    public bool PlayWeatherAudio { get; set; } = true;
    public bool RequireVehicleForAudio { get; set; } = false;
    public bool WeahterAudio_MuteRadio { get; set; } = true;
    public WorldSettings()
    {

    }

}