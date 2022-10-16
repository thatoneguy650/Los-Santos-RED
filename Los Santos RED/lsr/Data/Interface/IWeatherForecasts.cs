using Rage;
using System.Collections.Generic;

namespace LosSantosRED.lsr.Interface
{
    public interface IWeatherForecasts
    {
        List<WeatherForecast> WeatherForecastList { get; }

    }
}