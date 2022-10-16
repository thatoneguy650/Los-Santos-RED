using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class WeatherForecast
{
    private string CleanedDescription => Description.ToLower();
    public DateTime DateTime { get; set; }
    public float AirTemperature { get; set; }
    public float WindSpeed { get; set; }
    public float WindDirection { get; set; }
    public string Description { get; set; }
    public float WindSpeedMetersPerSecond => WindSpeed * 0.44704f;
    public float WindDirectionRadians => WindDirection * 0.0174533f;
    public WeatherForecast()
    {
    }

    public WeatherForecast(DateTime dateTime, float airTemperature, float windSpeed, float windDirection, string description)
    {
        DateTime = dateTime;
        AirTemperature = airTemperature;
        WindSpeed = windSpeed;
        WindDirection = windDirection;
        Description = description;
    }
    public eWeatherTypeHash GTAWeather
    {
        get
        {
            if(CleanedDescription == "clear")
            {
                return eWeatherTypeHash.Clear;
            }
            else if (CleanedDescription == "broken")
            {
                return eWeatherTypeHash.Clearing;
            }
            else if (CleanedDescription == "thin scattered")
            {
                return eWeatherTypeHash.Clouds;
            }
            else if (CleanedDescription == "scattered")
            {
                return eWeatherTypeHash.Clouds;
            }
            else if (CleanedDescription == "mist" || CleanedDescription == "mist,fog")
            {
                return eWeatherTypeHash.Foggy;
            }
            else if (CleanedDescription == "fog")
            {
                return eWeatherTypeHash.Foggy;
            }
            else if (CleanedDescription == "overcast")
            {
                return eWeatherTypeHash.Overcast;
            }
            else if (CleanedDescription == "light rain")
            {
                return eWeatherTypeHash.Raining;
            }
            else if (CleanedDescription == "haze")
            {
                return eWeatherTypeHash.Smog;
            }
            else if (CleanedDescription == "rain")
            {
                return eWeatherTypeHash.Raining;
            }
            else if (CleanedDescription == "light rain,mist")
            {
                return eWeatherTypeHash.Raining;
            }
            else if (CleanedDescription == "rain,mist")
            {
                return eWeatherTypeHash.Raining;
            }
            else if (CleanedDescription == "heavy rain,mist")
            {
                return eWeatherTypeHash.Raining;
            } 
            else if (CleanedDescription == "obscured")
            {
                return eWeatherTypeHash.Clouds;
            }
            else if (CleanedDescription == "fog,mist")
            {
                return eWeatherTypeHash.Foggy;
            }
            else if (CleanedDescription == "thunder")
            {
                return eWeatherTypeHash.ThunderStorm;
            }
            else if (CleanedDescription == "heavy rain")
            {
                return eWeatherTypeHash.Raining;
            }
            else if (CleanedDescription == "light drizzle")
            {
                return eWeatherTypeHash.Raining;
            }
            else if (CleanedDescription == "light drizzle,mist")
            {
                return eWeatherTypeHash.Raining;
            }
            else if (CleanedDescription == "drizzle")
            {
                return eWeatherTypeHash.Raining;
            }
            else if (CleanedDescription == "smoke")
            {
                return eWeatherTypeHash.Smog;
            }
            else if (CleanedDescription == "haze,smoke")
            {
                return eWeatherTypeHash.Smog;
            }
            else if (CleanedDescription == "mist,smoke")
            {
                return eWeatherTypeHash.Smog;
            }
            else if (CleanedDescription == "smoke,mist")
            {
                return eWeatherTypeHash.Smog;
            }
            else if (CleanedDescription == "heavy rain/thunderstorm")
            {
                return eWeatherTypeHash.ThunderStorm;
            }
            else if (CleanedDescription == "light rain/thunderstorm")
            {
                return eWeatherTypeHash.ThunderStorm;
            }
            else if (CleanedDescription == "thunderstorm")
            {
                return eWeatherTypeHash.ThunderStorm;
            }
            else if (CleanedDescription == "thunderstorm,mist")
            {
                return eWeatherTypeHash.ThunderStorm;
            }
            else if (CleanedDescription == "heavy rain/thunderstorm,mist")
            {
                return eWeatherTypeHash.ThunderStorm;
            }
            else
            {
                return eWeatherTypeHash.Clear;
            }
        }
    }
    public float RainLevel
    {
        get
        {
            if (CleanedDescription == "light rain")
            {
                return 0.2f;
            }
            else if (CleanedDescription == "rain")
            {
                return 0.5f;
            }
            else if (CleanedDescription == "light rain,mist")
            {
                return 0.2f;
            }
            else if (CleanedDescription == "rain,mist")
            {
                return 0.3f;
            }
            else if (CleanedDescription == "heavy rain,mist")
            {
                return 1.0f;
            }
            else if (CleanedDescription == "heavy rain")
            {
                return 1.0f;
            }
            else if (CleanedDescription == "light drizzle")
            {
                return 0.1f;
            }
            else if (CleanedDescription == "light drizzle,mist")
            {
                return 0.1f;
            }
            else if (CleanedDescription == "drizzle")
            {
                return 0.1f;
            }
            else if (CleanedDescription == "heavy rain/thunderstorm")
            {
                return 1.0f;
            }
            else if (CleanedDescription == "light rain/thunderstorm")
            {
                return 0.25f;
            }
            else if (CleanedDescription == "thunderstorm")
            {
                return 0.25f;
            }
            else if (CleanedDescription == "thunderstorm,mist")
            {
                return 0.25f;
            }
            else if (CleanedDescription == "heavy rain/thunderstorm,mist")
            {
                return 1.0f;
            }
            return -1.0f;
        }
    }
    public string GTAWeatherType
    {
        get
        {
            if(GTAWeather == eWeatherTypeHash.Blizzard)
            {
                return "BLIZZARD";
            }
            else if (GTAWeather == eWeatherTypeHash.Christmas)
            {
                return "XMAS";
            }
            else if (GTAWeather == eWeatherTypeHash.Clear)
            {
                return "CLEAR";
            }
            else if (GTAWeather == eWeatherTypeHash.Clearing)
            {
                return "CLEARING";
            }
            else if (GTAWeather == eWeatherTypeHash.Clouds)
            {
                return "CLOUDS";
            }
            else if (GTAWeather == eWeatherTypeHash.ExtraSunny)
            {
                return "EXTRASUNNY";
            }
            else if (GTAWeather == eWeatherTypeHash.Foggy)
            {
                return "FOGGY";
            }
            else if (GTAWeather == eWeatherTypeHash.Halloween)
            {
                return "HALLOWEEN";
            }
            else if (GTAWeather == eWeatherTypeHash.Neutral)
            {
                return "NEUTRAL";
            }
            else if (GTAWeather == eWeatherTypeHash.None)
            {
                return "NEUTRAL";
            }
            else if (GTAWeather == eWeatherTypeHash.Overcast)
            {
                return "OVERCAST";
            }
            else if (GTAWeather == eWeatherTypeHash.Raining)
            {
                return "RAIN";
            }
            else if (GTAWeather == eWeatherTypeHash.Smog)
            {
                return "SMOG";
            }
            else if (GTAWeather == eWeatherTypeHash.Snowing)
            {
                return "SNOW";
            }
            else if (GTAWeather == eWeatherTypeHash.Snowlight)
            {
                return "SNOWLIGHT";
            }
            else if (GTAWeather == eWeatherTypeHash.ThunderStorm)
            {
                return "THUNDER";
            }
            else if (GTAWeather == eWeatherTypeHash.Unknown)
            {
                return "NEUTRAL";
            }
            return "NEUTRAL";
        }
    }

}

