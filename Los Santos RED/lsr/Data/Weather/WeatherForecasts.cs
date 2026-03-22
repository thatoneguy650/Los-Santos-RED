using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class WeatherForecasts : IWeatherForecasts
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\WeatherForecasts.xml";
    public List<WeatherForecast> WeatherForecastList { get; set; } = new List<WeatherForecast>();

    public void ReadConfig()
    {
        //DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        //FileInfo ConfigFile = LSRDirectory.GetFiles("WeatherForecasts*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        //if (ConfigFile != null)
        //{
        //    EntryPoint.WriteToConsole($"Loaded WeatherForecasts config: {ConfigFile.FullName}", 0);
        //    WeatherForecastList = Serialization.DeserializeParams<WeatherForecast>(ConfigFile.FullName);
        //}
        //else if (File.Exists(ConfigFileName))
        //{
        //    EntryPoint.WriteToConsole($"Loaded WeatherForecasts config  {ConfigFileName}", 0);
        //    WeatherForecastList = Serialization.DeserializeParams<WeatherForecast>(ConfigFileName);
        //}
        //else
        //{
        //    EntryPoint.WriteToConsole($"No WeatherForecasts config found, creating default", 0);
        //    DefaultConfig();
        //}
    }
    public void SetupDefaultOnly()
    {
        //DefaultConfig();
    }
    private void DefaultConfig()
    {
        WeatherForecastList = new List<WeatherForecast>()
        {
            new WeatherForecast(DateTime.Parse("01/01/2020 00:00"),55.4f,3.45f,40,"thin scattered")
                   };

        Serialization.SerializeParams(WeatherForecastList, ConfigFileName);
    }
}

