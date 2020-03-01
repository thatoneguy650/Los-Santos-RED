using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

public static class WeatherReporting
{
    private static List<WeatherFile> WeatherFiles;
    private static WeatherTypeHash NextWeather;
    private static WeatherTypeHash PrevNextWeather;
    private static WeatherTypeHash CurrentWeather;
    private static WeatherTypeHash PrevCurrentWeather;
    private static float CurrentWindSpeed;
    private static uint GameTimeLastReportedWeather;
    private static string RadioStationLastTuned;
    public static bool IsRunning { get; set; }
    public static bool IsReportingWeather { get; set; }
    public static bool CanReportWeather
    {
        get
        {
            return Game.GameTime - GameTimeLastReportedWeather >= 30000;
        }
    }
    public static bool CanReportWind
    {
        get
        {
            return Game.GameTime - GameTimeLastReportedWeather >= 180000;
        }
    }
    public static float WindSpeed
    {
        get
        {
            return CurrentWindSpeed;
        }
    }
    public static WeatherTypeHash ForecastedWeather
    {
        get
        {
            return NextWeather;
        }
    }
    public static void Initialize()
    {
        WeatherFiles = new List<WeatherFile>();
        NextWeather = WeatherTypeHash.Neutral;
        PrevNextWeather = WeatherTypeHash.Neutral;
        CurrentWeather = WeatherTypeHash.Neutral;
        PrevCurrentWeather = WeatherTypeHash.Neutral;
        CurrentWindSpeed = 0f;
        GameTimeLastReportedWeather = 0;
        IsRunning = true;
        IsReportingWeather = false;
        RadioStationLastTuned = "";
        WeatherFiles.Add(Cloudy.Cloudy1);
        WeatherFiles.Add(Cloudy.Cloudy2);
        WeatherFiles.Add(Cloudy.Cloudy3);
        WeatherFiles.Add(Cloudy.Cloudy4);
        WeatherFiles.Add(Cloudy.Cloudy5);
        WeatherFiles.Add(Cloudy.Cloudy6);
        WeatherFiles.Add(Cloudy.Cloudy7);
        WeatherFiles.Add(Cloudy.Cloudy8);
        WeatherFiles.Add(Cloudy.Cloudy10);
        WeatherFiles.Add(Cloudy.Cloudy11);

        WeatherFiles.Add(Foggy.Fog1);
        WeatherFiles.Add(Foggy.Fog2);
        WeatherFiles.Add(Foggy.Fog4);
        WeatherFiles.Add(Foggy.Fog5);
        WeatherFiles.Add(Foggy.Fog6);
        WeatherFiles.Add(Foggy.Fog7);
        WeatherFiles.Add(Foggy.Fog8);
        WeatherFiles.Add(Foggy.Fog9);
        WeatherFiles.Add(Foggy.Fog10);
        WeatherFiles.Add(Foggy.Fog11);

        WeatherFiles.Add(Rainy.Rain1);
        WeatherFiles.Add(Rainy.Rain2);
        WeatherFiles.Add(Rainy.Rain3);
        WeatherFiles.Add(Rainy.Rain5);
        WeatherFiles.Add(Rainy.Rain6);
        WeatherFiles.Add(Rainy.Rain8);
        WeatherFiles.Add(Rainy.Rain9);
        WeatherFiles.Add(Rainy.Rain10);
        WeatherFiles.Add(Rainy.Rain11);

        WeatherFiles.Add(Stormy.Storm1);

        WeatherFiles.Add(Windy.Windy1);
        WeatherFiles.Add(Windy.Windy2);
        WeatherFiles.Add(Windy.Windy3);
        WeatherFiles.Add(Windy.Windy4);
        WeatherFiles.Add(Windy.Windy6);
        WeatherFiles.Add(Windy.Windy7);
        WeatherFiles.Add(Windy.Windy8);

        WeatherFiles.Add(Sunny.Sunny1);
        WeatherFiles.Add(Sunny.Sunny2);
        WeatherFiles.Add(Sunny.Sunny3);
        WeatherFiles.Add(Sunny.Sunny4);
        WeatherFiles.Add(Sunny.Sunny5);
        WeatherFiles.Add(Sunny.Sunny6);
        WeatherFiles.Add(Sunny.Sunny7);
        WeatherFiles.Add(Sunny.Sunny8);
        WeatherFiles.Add(Sunny.Sunny9);
        WeatherFiles.Add(Sunny.Sunny10);
        WeatherFiles.Add(Sunny.Sunny11);
        WeatherFiles.Add(Sunny.Sunny12);
        GameTimeLastReportedWeather = Game.GameTime;
        CheckWeather();
        MainLoop();
    }
    public static void MainLoop()
    {
        //GameFiber.StartNew(delegate
        //{
        //    try
        //    {
        //        while (IsRunning)
        //        {


        //            if (Settings.DebugShowUI)
        //            {
        //                string Tasking = string.Format("Weather: Current: {0}, Next: {1}, Wind: {2}", CurrentWeather, NextWeather, WindSpeed);//string.Format("ToTask: {0}", CopsToTask.Count());
        //                UI.Text(Tasking, 0.78f, 0.16f, 0.35f, false, Color.White, UI.EFont.FontChaletComprimeCologne);
        //            }
        //            GameFiber.Yield();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Dispose();
        //        Debugging.WriteToLog("Error", e.Message + " : " + e.StackTrace);
        //    }
        //});
    }
    public static void Dispose()
    {
        IsRunning = false;
        DispatchAudio.AbortAllAudio();
    }
    public static void CheckWeather()
    {
        return;

        CurrentWindSpeed = NativeFunction.CallByName<float>("GET_WIND_SPEED");
        CurrentWeather = (WeatherTypeHash)NativeFunction.CallByName<int>("GET_PREV_WEATHER_TYPE_HASH_NAME");
        NextWeather = (WeatherTypeHash)NativeFunction.CallByName<int>("GET_NEXT_WEATHER_TYPE_HASH_NAME");
        if(CanReportWind)
        {
            if(CurrentWindSpeed >= 11.5)
            {
                ReportWindy();
            }
        }
        if (CanReportWeather)
        {
            if (PrevCurrentWeather != CurrentWeather)
            {
                CurrentWeatherChanged();
            }
        }
        if(CanReportWeather)
        {
            if (PrevNextWeather != NextWeather)
            {
                NextWeatherChanged();
            }
        }
    }
    private static void CurrentWeatherChanged()
    {
        Debugging.WriteToLog("CheckWeather", string.Format("Current Weather Changed from {0} to {1}", PrevCurrentWeather, CurrentWeather));
        PrevCurrentWeather = CurrentWeather;

    }
    private static void NextWeatherChanged()
    {
        if(NextWeather != CurrentWeather && LosSantosRED.PlayerWantedLevel == 0 && !DispatchAudio.IsPlayingAudio)
            ReportWeather(NextWeather);

        Debugging.WriteToLog("CheckWeather", string.Format("Next Weather Changed from {0} to {1}", PrevNextWeather, NextWeather));
        PrevNextWeather = NextWeather;
    }
    public static void ReportWeather(WeatherTypeHash WeatherToReport)
    {
        if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && LosSantosRED.PlayerWantedLevel == 0 && Police.PlayerHasBeenNotWantedFor > 15000)//Aren;t wanted and haven't been wanted for 15 seconds
        {
            string WeatherFile = GetAudioFromWeatherType(WeatherToReport);
            if (WeatherFile == "")
                return;

            StoredAndTurnOffRadio();
            GameTimeLastReportedWeather = Game.GameTime;
            DispatchAudio.PlayAudioList(new DispatchAudio.DispatchAudioEvent(new List<string> { Weazel.Outro2.FileName, WeatherFile, Weazel.Outro.FileName }, false, ""));      
        }
    }
    private static void StoredAndTurnOffRadio()
    {
        RadioStationLastTuned = "OFF";
        unsafe
        {
            IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_PLAYER_RADIO_STATION_NAME");
            RadioStationLastTuned = Marshal.PtrToStringAnsi(ptr);
        }
        if (RadioStationLastTuned != "OFF")
        {
            NativeFunction.CallByName<bool>("SET_VEH_RADIO_STATION", Game.LocalPlayer.Character.CurrentVehicle, "OFF");
            GameFiber ChangeRadioBack = GameFiber.StartNew(delegate
            {
                GameFiber.Sleep(2000);

                while (DispatchAudio.AudioPlaying)
                    GameFiber.Sleep(500);

                if (Game.LocalPlayer.Character.IsInAnyVehicle(false))
                    NativeFunction.CallByName<bool>("SET_VEH_RADIO_STATION", Game.LocalPlayer.Character.CurrentVehicle, RadioStationLastTuned);
            }, "ChangeRadioBack");
        }
    }
    private static void ReportWindy()
    {
        if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && LosSantosRED.PlayerWantedLevel == 0 && Police.PlayerHasBeenNotWantedFor > 15000)//Aren;t wanted and haven't been wanted for 15 seconds
        {
            StoredAndTurnOffRadio();
            GameTimeLastReportedWeather = Game.GameTime;
            DispatchAudio.PlayAudioList(new DispatchAudio.DispatchAudioEvent(new List<string> { Weazel.Outro2.FileName, new List<string> { Windy.Windy1.FileName, Windy.Windy2.FileName, Windy.Windy3.FileName, Windy.Windy4.FileName, Windy.Windy6.FileName, Windy.Windy7.FileName, Windy.Windy8.FileName }.PickRandom(), Weazel.Outro.FileName }, false, ""));
        }
    }
    public class Cloudy
    {
        public static WeatherFile Cloudy1 { get { return new WeatherFile("weather\\Cloudy1.wav",new List<WeatherTypeHash> { WeatherTypeHash.Clouds,WeatherTypeHash.Overcast,WeatherTypeHash.Smog },true,false);; } }//were getting some serious clouds to our east, expect them for a while
        public static WeatherFile Cloudy2 { get { return new WeatherFile("weather\\Cloudy2.wav", new List<WeatherTypeHash> { WeatherTypeHash.Clouds, WeatherTypeHash.Overcast, WeatherTypeHash.Smog }, true,false); } }//were getting some clouds in the areas, expect them to stay around for a bit then move out
        public static WeatherFile Cloudy3 { get { return new WeatherFile("weather\\Cloudy3.wav", new List<WeatherTypeHash> { WeatherTypeHash.Clouds, WeatherTypeHash.Overcast, WeatherTypeHash.Smog }, true,true); } }//cloudy right now. Low pressure coming in.
        public static WeatherFile Cloudy4 { get { return new WeatherFile("weather\\Cloudy4.wav", new List<WeatherTypeHash> { WeatherTypeHash.Clouds, WeatherTypeHash.Overcast }, true,true); } }//great night tonight if this weather holds, cloudy right now
        public static WeatherFile Cloudy5 { get { return new WeatherFile("weather\\Cloudy5.wav", new List<WeatherTypeHash> { WeatherTypeHash.Clouds, WeatherTypeHash.Overcast }, true,false); } }//little cloudy right now, but it will be a great week
        public static WeatherFile Cloudy6 { get { return new WeatherFile("weather\\Cloudy6.wav", new List<WeatherTypeHash> { WeatherTypeHash.Clouds, WeatherTypeHash.Overcast }, true,false); } }//its cloudy,
        public static WeatherFile Cloudy7 { get { return new WeatherFile("weather\\Cloudy7.wav", new List<WeatherTypeHash> { WeatherTypeHash.Clouds, WeatherTypeHash.Overcast }, true,true); } }//lots of clouds out there, gonna see some bright red clouds
        public static WeatherFile Cloudy8 { get { return new WeatherFile("weather\\Cloudy8.wav", new List<WeatherTypeHash> { WeatherTypeHash.Clouds, WeatherTypeHash.Overcast }, true,true); } }//sseing some cloudy skies right now, though it will clear up at some point
        public static WeatherFile Cloudy10 { get { return new WeatherFile("weather\\Cloudy10.wav", new List<WeatherTypeHash> { WeatherTypeHash.Clouds, WeatherTypeHash.Overcast }, true,false); } }//some clouds moving through the area
        public static WeatherFile Cloudy11 { get { return new WeatherFile("weather\\Cloudy11.wav", new List<WeatherTypeHash> { WeatherTypeHash.Clouds, WeatherTypeHash.Overcast }, false,true); } }//some clouds moving through the area
    }
    public class Foggy
    {
        public static WeatherFile Fog1 { get { return new WeatherFile("weather\\Fog1.wav", new List<WeatherTypeHash> { WeatherTypeHash.Foggy },true,true); ; } }//tracking a fog that has rolled in, going to be foggy for a while longer, expect a tropical storm?
        public static WeatherFile Fog2 { get { return new WeatherFile("weather\\Fog2.wav", new List<WeatherTypeHash> { WeatherTypeHash.Foggy }, true,false); } }//drier air moving in later, soupy with fog right now
        public static WeatherFile Fog4 { get { return new WeatherFile("weather\\Fog4.wav", new List<WeatherTypeHash> { WeatherTypeHash.Foggy }, true,false); } }//foggy as can be. 
        public static WeatherFile Fog5 { get { return new WeatherFile("weather\\Fog5.wav", new List<WeatherTypeHash> { WeatherTypeHash.Foggy }, true,false); } }//a lot of moisture, foggy right now
        public static WeatherFile Fog6 { get { return new WeatherFile("weather\\Fog6.wav", new List<WeatherTypeHash> { WeatherTypeHash.Foggy }, true,false); } }//severe fog in the area. 
        public static WeatherFile Fog7 { get { return new WeatherFile("weather\\Fog7.wav", new List<WeatherTypeHash> { WeatherTypeHash.Foggy }, true,false); } }//feeling a little foggy now
        public static WeatherFile Fog8 { get { return new WeatherFile("weather\\Fog8.wav", new List<WeatherTypeHash> { WeatherTypeHash.Foggy }, true,true); } }//fog going to be around for a while. 
        public static WeatherFile Fog9 { get { return new WeatherFile("weather\\Fog9.wav", new List<WeatherTypeHash> { WeatherTypeHash.Foggy }, true,false); } }//fog in the area currently
        public static WeatherFile Fog10 { get { return new WeatherFile("weather\\Fog10.wav", new List<WeatherTypeHash> { WeatherTypeHash.Foggy }, true,false); } }//serious delays as fog has blanketed the area
        public static WeatherFile Fog11 { get { return new WeatherFile("weather\\Fog11.wav", new List<WeatherTypeHash> { WeatherTypeHash.Foggy }, true,false); } }//cant see much of anything due to the fog
        public static WeatherFile Fog12 { get { return new WeatherFile("weather\\Fog12.wav", new List<WeatherTypeHash> { WeatherTypeHash.Foggy }, true, false); } }//foggy right now
    }
    public class Rainy
    {
        public static WeatherFile Rain1 { get { return new WeatherFile("weather\\Rain1.wav", new List<WeatherTypeHash> { WeatherTypeHash.Raining, WeatherTypeHash.ThunderStorm }, true,true); ; } }//raid disturbance, expect to see a real deep dicking
        public static WeatherFile Rain2 { get { return new WeatherFile("weather\\Rain2.wav", new List<WeatherTypeHash> { WeatherTypeHash.Raining ,WeatherTypeHash.ThunderStorm}, true,true); } }//expect some rain for a while
        public static WeatherFile Rain3 { get { return new WeatherFile("weather\\Rain3.wav", new List<WeatherTypeHash> { WeatherTypeHash.Raining, WeatherTypeHash.ThunderStorm }, true,true); } }//Gonna get some rain right now
        public static WeatherFile Rain5 { get { return new WeatherFile("weather\\Rain5.wav", new List<WeatherTypeHash> { WeatherTypeHash.Raining, WeatherTypeHash.ThunderStorm }, false,true); } }//frontaly boundary coming through a couple storms coming with them
        public static WeatherFile Rain6 { get { return new WeatherFile("weather\\Rain6.wav", new List<WeatherTypeHash> { WeatherTypeHash.Raining, WeatherTypeHash.ThunderStorm }, true,true); } }//Ridge of rain over the city
        public static WeatherFile Rain8 { get { return new WeatherFile("weather\\Rain8.wav", new List<WeatherTypeHash> { WeatherTypeHash.Raining, WeatherTypeHash.ThunderStorm }, false,true); } }//big depressions coming through causing some rain
        public static WeatherFile Rain9 { get { return new WeatherFile("weather\\Rain9.wav", new List<WeatherTypeHash> { WeatherTypeHash.Raining, WeatherTypeHash.ThunderStorm }, false,true); } }//remnanats of tropical storm coming through, lots of rain
        public static WeatherFile Rain10 { get { return new WeatherFile("weather\\Rain10.wav", new List<WeatherTypeHash> { WeatherTypeHash.Raining, WeatherTypeHash.ThunderStorm },true,true); } }//causing the skies to open up
        public static WeatherFile Rain11 { get { return new WeatherFile("weather\\Rain11.wav", new List<WeatherTypeHash> { WeatherTypeHash.Raining, WeatherTypeHash.ThunderStorm }, true,false); } }//man do we have some rain coming in now
    }
    public class Stormy
    {
        public static WeatherFile Storm1 { get { return new WeatherFile("weather\\Storm1.wav", new List<WeatherTypeHash> { WeatherTypeHash.ThunderStorm , WeatherTypeHash.Blizzard, WeatherTypeHash.Christmas,WeatherTypeHash.Halloween,WeatherTypeHash.Snowing}, true, true); ; } }//thousands of people could be losing power due to the storm
    }
    public class Windy
    {
        public static WeatherFile Windy1 { get { return new WeatherFile("weather\\Windy1.wav", new List<WeatherTypeHash> { WeatherTypeHash.None }, true, true); ; } }//wind is going to be with us for a while
        public static WeatherFile Windy2 { get { return new WeatherFile("weather\\Windy2.wav", new List<WeatherTypeHash> { WeatherTypeHash.None }, true, true); } }//some serious wind coming through next few days
        public static WeatherFile Windy3 { get { return new WeatherFile("weather\\Windy3.wav", new List<WeatherTypeHash> { WeatherTypeHash.None }, true, true); } }//tropical storms causing this wind right now
        public static WeatherFile Windy4 { get { return new WeatherFile("weather\\Windy4.wav", new List<WeatherTypeHash> { WeatherTypeHash.None }, true, true); } }//weve got some wind in the area
        public static WeatherFile Windy6 { get { return new WeatherFile("weather\\Windy6.wav", new List<WeatherTypeHash> { WeatherTypeHash.None }, true, true); } }//expect some wind then rain, then sun
        public static WeatherFile Windy7 { get { return new WeatherFile("weather\\Windy7.wav", new List<WeatherTypeHash> { WeatherTypeHash.None }, true, true); } }//its windy
        public static WeatherFile Windy8 { get { return new WeatherFile("weather\\Windy8.wav", new List<WeatherTypeHash> { WeatherTypeHash.None }, true, true); } }//windy now
    }
    public class Sunny
    {
        public static WeatherFile Sunny1 { get { return new WeatherFile("weather\\Sunny1.wav", new List<WeatherTypeHash> { WeatherTypeHash.ExtraSunny, WeatherTypeHash.Clear }, true,true); ; } }//five day forecast etc., sunny right now
        public static WeatherFile Sunny2 { get { return new WeatherFile("weather\\Sunny2.wav", new List<WeatherTypeHash> { WeatherTypeHash.ExtraSunny, WeatherTypeHash.Clear }, true, true); } }//patches of moisture coming down the coast, nice and sunny now
        public static WeatherFile Sunny3 { get { return new WeatherFile("weather\\Sunny3.wav", new List<WeatherTypeHash> { WeatherTypeHash.ExtraSunny, WeatherTypeHash.Clear }, true, true); } }//quiet right now, sunny and warm.
        public static WeatherFile Sunny4 { get { return new WeatherFile("weather\\Sunny4.wav", new List<WeatherTypeHash> { WeatherTypeHash.ExtraSunny, WeatherTypeHash.Clear }, true, true); } }//great weather, daytime heating
        public static WeatherFile Sunny5 { get { return new WeatherFile("weather\\Sunny5.wav", new List<WeatherTypeHash> { WeatherTypeHash.ExtraSunny, WeatherTypeHash.Clear }, true, true); } }//sunny right now with threat of storms.
        public static WeatherFile Sunny6 { get { return new WeatherFile("weather\\Sunny6.wav", new List<WeatherTypeHash> { WeatherTypeHash.ExtraSunny, WeatherTypeHash.Clear }, true, true); } }//sunshine for your soul right now
        public static WeatherFile Sunny7 { get { return new WeatherFile("weather\\Sunny7.wav", new List<WeatherTypeHash> { WeatherTypeHash.ExtraSunny, WeatherTypeHash.Clear }, true, true); } }//sun and clouds possible later, sunny right now
        public static WeatherFile Sunny8 { get { return new WeatherFile("weather\\Sunny8.wav", new List<WeatherTypeHash> { WeatherTypeHash.ExtraSunny, WeatherTypeHash.Clear }, true, true); } }//sunny right now
        public static WeatherFile Sunny9 { get { return new WeatherFile("weather\\Sunny9.wav", new List<WeatherTypeHash> { WeatherTypeHash.ExtraSunny, WeatherTypeHash.Clear }, true, true); } }//nice and sunny out there
        public static WeatherFile Sunny10 { get { return new WeatherFile("weather\\Sunny10.wav", new List<WeatherTypeHash> { WeatherTypeHash.ExtraSunny, WeatherTypeHash.Clear }, true, true); } }//sun right now
        public static WeatherFile Sunny11 { get { return new WeatherFile("weather\\Sunny11.wav", new List<WeatherTypeHash> { WeatherTypeHash.ExtraSunny, WeatherTypeHash.Clear }, true, true); } }//just a great day, lots of sun
        public static WeatherFile Sunny12 { get { return new WeatherFile("weather\\Sunny12.wav", new List<WeatherTypeHash> { WeatherTypeHash.ExtraSunny, WeatherTypeHash.Clear }, true, true); } }//right now its sunny.
    }
    public class Weazel
    {
        public static WeatherFile Intro { get { return new WeatherFile("weather\\Intro.wav", new List<WeatherTypeHash> { WeatherTypeHash.None }, false,false); ; } }
        public static WeatherFile Outro { get { return new WeatherFile("weather\\Outro.wav", new List<WeatherTypeHash> { WeatherTypeHash.None }, false,false); } }
        public static WeatherFile Outro2 { get { return new WeatherFile("weather\\Outro_News_03.wav", new List<WeatherTypeHash> { WeatherTypeHash.None }, false, false); } }
    }
    public static string GetAudioFromWeatherType(WeatherTypeHash MyWeather)
    {
        if ((MyWeather == WeatherTypeHash.Clear || MyWeather == WeatherTypeHash.ExtraSunny) && Police.IsNightTime)//Audio files don't really make sense at night
        {
            return "";
        }
        WeatherFile TheWeather = WeatherFiles.Where(p => p.WeatherTypes.Any(c => c == MyWeather)).PickRandom();
        if (TheWeather == null)
            return "";
        else
            return TheWeather.FileName;
    }
    public class WeatherFile
    {
        public WeatherFile(string fileName, List<WeatherTypeHash> weatherTypes, bool isCurrentWeather, bool isForecastedWeather)
        {
            FileName = fileName;
            WeatherTypes = weatherTypes;
            IsCurrentWeather = isCurrentWeather;
            IsForecastedWeather = isForecastedWeather;
        }

        public string FileName { get; set; }
        public List<WeatherTypeHash> WeatherTypes { get; set; } = new List<WeatherTypeHash>();
        public bool IsCurrentWeather { get; set; }
        public bool IsForecastedWeather { get; set; }
    }
    public enum WeatherTypeHash
    {
        None = 0,
        Unknown = -1,
        ExtraSunny = -1750463879,
        Clear = 916995460,
        Neutral = -1530260698,
        Smog = 282916021,
        Foggy = -1368164796,
        Clouds = 821931868,
        Overcast = -1148613331,
        Clearing = 1840358669,
        Raining = 1420204096,
        ThunderStorm = -1233681761,
        Blizzard = 669657108,
        Snowing = -273223690,
        Snowlight = 603685163,
        Christmas = -1429616491,
        Halloween = -921030142
    }
}
