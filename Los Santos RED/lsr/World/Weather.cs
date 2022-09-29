using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

public class Weather
{
    private List<WeatherFile> WeatherFiles;
    private WeatherTypeHash NextWeather;
    private WeatherTypeHash PrevNextWeather;
    private WeatherTypeHash CurrentWeather;
    private WeatherTypeHash PrevCurrentWeather;
    private float CurrentWindSpeed;
    private uint GameTimeLastReportedWeather;
    private string RadioStationLastTuned;
    private IAudioPlayable AudioPlayer;
    private ISettingsProvideable Settings;
    private ITimeReportable Time;
    private List<uint> NotificationHandles = new List<uint>();
    private readonly string NotificationPicture = "CHAR_LS_TOURIST_BOARD";//"WEB_ECOLASOFTDRINK"//needs request, brought to you by ecola?//"WEB_LOGGERLIGHT"//same?//"WEB_REDWOODCIGARETTES"//"WEB_SPRUNK"//"WEB_WHIZWIRELESS"
    private readonly string NotificationTitle = "Weazel News";
    private readonly string NotificationSubtitle = "~b~Weather Report";
    private List<Sponsor> Sponsors;
    private IWeatherReportable Player;
    private bool isPlayingAudio = false;
    private uint GameTimeLastAudioReportedWeather;
    public Weather(IAudioPlayable audio, ISettingsProvideable settings, ITimeReportable time, IWeatherReportable player)
    {
        AudioPlayer = audio;
        Settings = settings;
        Time = time;
        Player = player;
    }
    public bool IsReportingWeather { get; set; }
    private bool CanAudioReportWeather => Game.GameTime - GameTimeLastAudioReportedWeather >= Settings.SettingsManager.WeatherReportingSettings.ReportWeather_MinimumTimeBetweenAudioReports;
    private bool CanAudioReportWind => Game.GameTime - GameTimeLastAudioReportedWeather >= Settings.SettingsManager.WeatherReportingSettings.ReportWindyWeather_MinimumTimeBetweenAudioReports;
    private bool CanReportWeather => Game.GameTime - GameTimeLastReportedWeather >= Settings.SettingsManager.WeatherReportingSettings.ReportWeather_MinimumTimeBetweenReports;
    private bool CanReportWind => Game.GameTime - GameTimeLastReportedWeather >= Settings.SettingsManager.WeatherReportingSettings.ReportWindyWeather_MinimumTimeBetweenReports;
    public WeatherTypeHash ForecastedWeather => NextWeather;
    public void Setup()
    {
        WeatherFiles = new List<WeatherFile>();
        NextWeather = WeatherTypeHash.Neutral;
        PrevNextWeather = WeatherTypeHash.Neutral;
        CurrentWeather = WeatherTypeHash.Neutral;
        PrevCurrentWeather = WeatherTypeHash.Neutral;
        CurrentWindSpeed = 0f;
        GameTimeLastReportedWeather = 0;
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

        Sponsors = new List<Sponsor>()
        {
            new Sponsor("PDX Motors","WEB_PREMIUMDELUXEMOTORSPORT"),
            new Sponsor("Redwood Cigarettes","WEB_REDWOODCIGARETTES"),
            new Sponsor("Rusty Browns","WEB_RUSTYBROWNSDONUTS"),
            new Sponsor("Sprunk","WEB_SPRUNK"),
            new Sponsor("Toe Shoes","WEB_TOESHOES"),
            new Sponsor("Whiz Wireless","WEB_WHIZWIRELESS"),
            new Sponsor("Logger Light","WEB_LOGGERLIGHT"),
            new Sponsor("Lom Bank","WEB_LOMBANK"),
            new Sponsor("eCola","WEB_ECOLASOFTDRINK"),
            new Sponsor("Eris","WEB_ERIS"),


            new Sponsor("Ammunation","CHAR_AMMUNATION"),
            new Sponsor("Bank of Liberty","CHAR_BANK_BOL"),
            new Sponsor("Fleeca Bank","CHAR_BANK_FLEECA"),
            new Sponsor("Maze Bank","CHAR_BANK_MAZE"),
            new Sponsor("The Diamond Casino","CHAR_CASINO"),
            new Sponsor("Epsilon","CHAR_EPSILON"),
            new Sponsor("Life Invader","CHAR_LIFEINVADER"),
            new Sponsor("Minotaur","CHAR_MINOTAUR"),
            new Sponsor("Merryweather","CHAR_MP_MERRYWEATHER"),
            new Sponsor("Mors Mutual","CHAR_MP_MORS_MUTUAL"),



            new Sponsor("HomeCremation.com","WEB_YOURDEADFAMILY"),
            new Sponsor("HugAnimals.com","DIA_EXT1_RETRIEVER"),

            new Sponsor("Smoke on the Water","CHAR_PROPERTY_WEED_SHOP"),


            new Sponsor("Vig Insurance","DIA_JANET"),
            new Sponsor("Wind From Below","WEB_DIGIFARM"),
            

        };

        foreach(Sponsor sp in Sponsors)
        {
            NativeFunction.Natives.REQUEST_STREAMED_TEXTURE_DICT(sp.NotificationPicture, true);
        }

    }
    public void Update()
    {
        if (Settings.SettingsManager.WeatherReportingSettings.ReportWeather)
        {
            CurrentWeather = (WeatherTypeHash)NativeFunction.Natives.GET_PREV_WEATHER_TYPE_HASH_NAME<int>();
            NextWeather = (WeatherTypeHash)NativeFunction.Natives.GET_NEXT_WEATHER_TYPE_HASH_NAME<int>();
            if (CanReportWeather)
            {
                if (PrevCurrentWeather != CurrentWeather)
                {
                    CurrentWeatherChanged();
                }
            }
            if (CanReportWeather)
            {
                if (PrevNextWeather != NextWeather)
                {
                    NextWeatherChanged();
                }
            }
            CurrentWindSpeed = NativeFunction.Natives.GET_WIND_SPEED<float>();
            if (CanReportWind)
            {
                if (CurrentWindSpeed >= Settings.SettingsManager.WeatherReportingSettings.ReportWindyWeather_MinimumSpeed && !Time.IsFastForwarding && Player.IsNotWanted && Player.IsAliveAndFree)
                {
                    GameFiber.Yield();
                    ReportWindy();
                }
            }
        }
    }
    public void Dispose()
    {
        if (IsReportingWeather)
        {
            AudioPlayer.Abort();
        }
    }
    public void DebugPlayReport()
    {
        Dispose();
        GameTimeLastReportedWeather = 0;
        if(RandomItems.RandomPercent(10))
        {
            ReportWindy();
        }
        else
        {

            Array values = Enum.GetValues(typeof(WeatherTypeHash));
            Random random = new Random();
            WeatherTypeHash randomBar = (WeatherTypeHash)values.GetValue(random.Next(values.Length));

            ReportWeather(randomBar);
        }
       // Update();
        
       // ReportWeather(WeatherTypeHash.Raining);
    }
    private void ReportWeather(WeatherTypeHash WeatherToReport)
    {
        WeatherFile weatherFile = GetRandomWeatherFile(WeatherToReport);
        if (weatherFile == null)
        {
            return;
        }
        GameTimeLastReportedWeather = Game.GameTime;
        if (Settings.SettingsManager.WeatherReportingSettings.ShowWeatherNotifications)
        {
            DisplayNotification(weatherFile.ForcedSponsorName, false);
        }
        //if (Settings.SettingsManager.WeatherReportingSettings.PlayWeatherAudio && CanAudioReportWeather && (Player.IsInVehicle || !Settings.SettingsManager.WeatherReportingSettings.PlayWeatherAudioInVehicleOnly))
        //{
        //    if (Settings.SettingsManager.WeatherReportingSettings.RequireVehicleForAudio && !Player.IsInVehicle)
        //    {
        //        return;
        //    }
        //    if (Settings.SettingsManager.WeatherReportingSettings.WeatherAudio_MuteRadio && !Settings.SettingsManager.VehicleSettings.KeepRadioAutoTuned && Player.IsInVehicle)
        //    {
        //        StoredAndTurnOffRadio();
        //    }
        //    PlayAudioList(new List<string> { Weazel.Outro2.FileName, weatherFile.FileName, Weazel.Outro.FileName });
        //    GameTimeLastAudioReportedWeather = Game.GameTime;
        //}
    }
    private void ReportWindy()
    {
        WeatherFile toReport = new List<WeatherFile> { Windy.Windy1, Windy.Windy2, Windy.Windy3, Windy.Windy4, Windy.Windy6, Windy.Windy7, Windy.Windy8 }.PickRandom();
        GameTimeLastReportedWeather = Game.GameTime;
        if (Settings.SettingsManager.WeatherReportingSettings.ShowWeatherNotifications)
        {
            DisplayNotification(toReport.ForcedSponsorName, true);
        }
        //if (Settings.SettingsManager.WeatherReportingSettings.PlayWeatherAudio && CanAudioReportWind && (Player.IsInVehicle || !Settings.SettingsManager.WeatherReportingSettings.PlayWeatherAudioInVehicleOnly))
        //{
        //    if (Settings.SettingsManager.WeatherReportingSettings.RequireVehicleForAudio && !Player.IsInVehicle)
        //    {
        //        return;
        //    }
        //    if (Settings.SettingsManager.WeatherReportingSettings.WeatherAudio_MuteRadio && !Settings.SettingsManager.VehicleSettings.KeepRadioAutoTuned && Player.IsInVehicle)
        //    {
        //        StoredAndTurnOffRadio();
        //    }
        //    PlayAudioList(new List<string> { Weazel.Outro2.FileName, toReport.FileName, Weazel.Outro.FileName });
        //    GameTimeLastAudioReportedWeather = Game.GameTime;
        //}
        //EntryPoint.WriteToConsole($"ReportWindy", 5);
    }
    private void DisplayNotification(string ForcedSponsorName, bool isWindy)
    {
        RemoveAllNotifications();
        double WindSpeed = Math.Round(CurrentWindSpeed, 0);
        string WindText;
        if(WindSpeed <= 2)
        {
            WindText = "~s~" + WindSpeed.ToString() + "~s~";
        }
        else if (WindSpeed <= 5)
        {
            WindText = "~y~" + WindSpeed.ToString() + "~s~";
        }
        else if (WindSpeed <= 15)
        {
            WindText = "~o~" + WindSpeed.ToString() + "~s~";
        }
        else
        {
            WindText = "~r~" + WindSpeed.ToString() + "~s~";
        }
        string NotificationText = $"Currently: {NameFromHash(CurrentWeather)}~n~Forecasted: {NameFromHash(NextWeather)}";
        if(isWindy)
        {
            NotificationText = $"Currently: ~s~Windy: {WindText} m/s~n~Forecasted: {NameFromHash(NextWeather)}";
        }    
        Sponsor sponsor = Sponsors.PickRandom();

        if(ForcedSponsorName != "")
        {
            sponsor = Sponsors.Where(x => x.SponsorName == ForcedSponsorName).FirstOrDefault();
        }
        if(sponsor != null)
        {
            NotificationText += "~s~~n~Brought To You By ~n~~h~" + sponsor.SponsorName + "~s~";
            NotificationHandles.Add(Game.DisplayNotification(sponsor.NotificationPicture, sponsor.NotificationPicture, NotificationTitle, NotificationSubtitle, NotificationText));
        }
        else
        {
            NotificationHandles.Add(Game.DisplayNotification(NotificationPicture, NotificationPicture, NotificationTitle, NotificationSubtitle, NotificationText));
        }
    }
    private void PlayAudioList(List<string> AudioToPlay)
    {
        GameFiber.Yield();
        GameFiber PlayAudioList = GameFiber.StartNew(delegate
        {
            GameFiber.Yield();
            while (AudioPlayer.IsAudioPlaying)
            {
                GameFiber.Yield();
            }
            foreach (string audioname in AudioToPlay)
            {
                if(Player.IsWanted || Player.Investigation.IsActive)
                {
                    if(isPlayingAudio)
                    {
                        AudioPlayer.Abort();
                    }
                    break;
                }
                if (Settings.SettingsManager.ScannerSettings.SetVolume)
                {
                    isPlayingAudio = true;
                    AudioPlayer.Play(audioname, Settings.SettingsManager.ScannerSettings.AudioVolume, true, false);
                }
                else
                {
                    isPlayingAudio = true;
                    AudioPlayer.Play(audioname, true, false);
                }
                while (AudioPlayer.IsAudioPlaying)
                {
                    GameFiber.Yield();
                }
            }
            isPlayingAudio = false;
        }, "PlayAudioList");
    }
    private void RemoveAllNotifications()
    {
        foreach (uint handles in NotificationHandles)
        {
            Game.RemoveNotification(handles);
        }
        NotificationHandles.Clear();
    }
    private void CurrentWeatherChanged()
    {
        if (Settings.SettingsManager.WeatherReportingSettings.ReportChangedCurrentWeather && !Time.IsFastForwarding && Player.IsNotWanted && Player.IsAliveAndFree)//if (!AudioPlayer.IsAudioPlaying && Settings.SettingsManager.WorldSettings.ReportChangedCurrentWeather && !Time.IsFastForwarding && Player.IsNotWanted && Player.IsAliveAndFree)
        {
            GameFiber.Yield();
            ReportWeather(CurrentWeather);
        }
        EntryPoint.WriteToConsole($"Current Weather Changed from {PrevCurrentWeather} to {CurrentWeather}", 5);
        PrevCurrentWeather = CurrentWeather;
    }
    private void NextWeatherChanged()
    {
        if (NextWeather != CurrentWeather && Settings.SettingsManager.WeatherReportingSettings.ReportChangedForecastedWeather && !Time.IsFastForwarding && Player.IsNotWanted && Player.IsAliveAndFree)//if (NextWeather != CurrentWeather && !AudioPlayer.IsAudioPlaying && Settings.SettingsManager.WorldSettings.ReportChangedForecastedWeather && !Time.IsFastForwarding && Player.IsNotWanted && Player.IsAliveAndFree)
        {
            GameFiber.Yield();
            ReportWeather(NextWeather);
        }
        EntryPoint.WriteToConsole($"Next Weather Changed from {PrevNextWeather} to {NextWeather}", 5);
        PrevNextWeather = NextWeather;
    }
    private void StoredAndTurnOffRadio()
    {
        if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Exists())
        {
            RadioStationLastTuned = "OFF";
            unsafe
            {
                IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_PLAYER_RADIO_STATION_NAME");
                RadioStationLastTuned = Marshal.PtrToStringAnsi(ptr);
            }
            if (RadioStationLastTuned != "OFF" && RadioStationLastTuned != "")
            {
                NativeFunction.Natives.SET_VEH_RADIO_STATION(Game.LocalPlayer.Character.CurrentVehicle, "OFF");
                GameFiber ChangeRadioBack = GameFiber.StartNew(delegate
                {
                    GameFiber.Sleep(2000);
                    while (AudioPlayer.IsAudioPlaying)
                    {
                        GameFiber.Sleep(100);
                    }
                    try
                    {
                        if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Exists())
                        {
                            NativeFunction.Natives.SET_VEH_RADIO_STATION(Game.LocalPlayer.Character.CurrentVehicle, RadioStationLastTuned);
                        }
                    }
                    catch (Exception ex)
                    {
                        Game.DisplayNotification("ERROR Changing radio back");
                        EntryPoint.WriteToConsole($"ERROR Changing radio back, {ex.Message} {ex.StackTrace}", 0);
                    }
                }, "ChangeRadioBack");
            }
        }
    }
    private string GetAudioFromWeatherType(WeatherTypeHash MyWeather)
    {
        if ((MyWeather == WeatherTypeHash.Clear || MyWeather == WeatherTypeHash.ExtraSunny) && Time.IsNight)//Audio files don't really make sense at night
        {
            return "";
        }
        WeatherFile TheWeather = WeatherFiles.Where(p => p.WeatherTypes.Any(c => c == MyWeather)).PickRandom();
        if (TheWeather == null)
        {
            return "";
        }
        else
        {
            return TheWeather.FileName;
        }
    }

    private WeatherFile GetRandomWeatherFile(WeatherTypeHash MyWeather)
    {
        if ((MyWeather == WeatherTypeHash.Clear || MyWeather == WeatherTypeHash.ExtraSunny) && Time.IsNight)//Audio files don't really make sense at night
        {
            return null;
        }
        return WeatherFiles.Where(p => p.WeatherTypes.Any(c => c == MyWeather)).PickRandom();
    }
    private string NameFromHash(WeatherTypeHash weatherTypeHash)
    {
        switch(weatherTypeHash)
        {
            case WeatherTypeHash.Blizzard:
                return "~h~Blizzard Conditions~s~";
            case WeatherTypeHash.Christmas:
                return "Snowfall~s~";
            case WeatherTypeHash.Clear:
                return "~p~Clear~s~";
            case WeatherTypeHash.Clearing:
                return "~p~Clear~s~";
            case WeatherTypeHash.Clouds:
                return "~p~Cloudy~s~";
            case WeatherTypeHash.ExtraSunny:
                return "~o~Extra Sunny~s~";
            case WeatherTypeHash.Foggy:
                return "~c~Thick Fog~s~";
            case WeatherTypeHash.Halloween:
                return "~o~Spooky~s~";
            case WeatherTypeHash.Neutral:
                return "~p~Clear~s~";
            case WeatherTypeHash.None:
                return "~s~None~s~";
            case WeatherTypeHash.Overcast:
                return "~p~Overcast~s~";
            case WeatherTypeHash.Raining:
                return "~b~Rainfall~s~";
            case WeatherTypeHash.Smog:
                return "~c~Thick Smog~s~";
            case WeatherTypeHash.Snowing:
                return "~s~Snowfall~s~";
            case WeatherTypeHash.Snowlight:
                return "~s~Snowfall~s~";
            case WeatherTypeHash.ThunderStorm:
                return "~b~Thunderstorms~s~";
            case WeatherTypeHash.Unknown:
                return "~s~Unknown~s~";                    
        }
        return "Unknown";
    }
    private class Cloudy
    {
        public static WeatherFile Cloudy1 { get { return new WeatherFile("weather\\Cloudy1.wav", new List<WeatherTypeHash> { WeatherTypeHash.Clouds, WeatherTypeHash.Overcast, WeatherTypeHash.Smog }, true, false) { ForcedSponsorName = "Merryweather" }; } }//were getting some serious clouds to our east, expect them for a while, Merryweather Sponsor
        public static WeatherFile Cloudy2 { get { return new WeatherFile("weather\\Cloudy2.wav", new List<WeatherTypeHash> { WeatherTypeHash.Clouds, WeatherTypeHash.Overcast, WeatherTypeHash.Smog }, true, false); } }//were getting some clouds in the areas, expect them to stay around for a bit then move out
        public static WeatherFile Cloudy3 { get { return new WeatherFile("weather\\Cloudy3.wav", new List<WeatherTypeHash> { WeatherTypeHash.Clouds, WeatherTypeHash.Overcast, WeatherTypeHash.Smog }, true, true); } }//cloudy right now. Low pressure coming in.
        public static WeatherFile Cloudy4 { get { return new WeatherFile("weather\\Cloudy4.wav", new List<WeatherTypeHash> { WeatherTypeHash.Clouds, WeatherTypeHash.Overcast }, true, true); } }//great night tonight if this weather holds, cloudy right now, too many apple martinis, booze sponsor?
        public static WeatherFile Cloudy5 { get { return new WeatherFile("weather\\Cloudy5.wav", new List<WeatherTypeHash> { WeatherTypeHash.Clouds, WeatherTypeHash.Overcast }, true, false); } }//little cloudy right now, but it will be a great week
        public static WeatherFile Cloudy6 { get { return new WeatherFile("weather\\Cloudy6.wav", new List<WeatherTypeHash> { WeatherTypeHash.Clouds, WeatherTypeHash.Overcast }, true, false); } }//its cloudy,
        public static WeatherFile Cloudy7 { get { return new WeatherFile("weather\\Cloudy7.wav", new List<WeatherTypeHash> { WeatherTypeHash.Clouds, WeatherTypeHash.Overcast }, true, true); } }//lots of clouds out there, gonna see some bright red clouds
        public static WeatherFile Cloudy8 { get { return new WeatherFile("weather\\Cloudy8.wav", new List<WeatherTypeHash> { WeatherTypeHash.Clouds, WeatherTypeHash.Overcast }, true, true); } }//sseing some cloudy skies right now, though it will clear up at some point
        public static WeatherFile Cloudy10 { get { return new WeatherFile("weather\\Cloudy10.wav", new List<WeatherTypeHash> { WeatherTypeHash.Clouds, WeatherTypeHash.Overcast }, true, false) {ForcedSponsorName = "HugAnimals.com" }; } }//some clouds moving through the area, HugAnimals.Com
        public static WeatherFile Cloudy11 { get { return new WeatherFile("weather\\Cloudy11.wav", new List<WeatherTypeHash> { WeatherTypeHash.Clouds, WeatherTypeHash.Overcast }, false, true) { ForcedSponsorName = "HomeCremation.com" }; } }//some clouds moving through the area, HomeCremation.Com
    }
    private class Foggy
    {
        public static WeatherFile Fog1 { get { return new WeatherFile("weather\\Fog1.wav", new List<WeatherTypeHash> { WeatherTypeHash.Foggy }, true, true) { ForcedSponsorName = "Smoke on the Water" }; } }//tracking a fog that has rolled in, going to be foggy for a while longer, expect a tropical storm?, bong hit
        public static WeatherFile Fog2 { get { return new WeatherFile("weather\\Fog2.wav", new List<WeatherTypeHash> { WeatherTypeHash.Foggy }, true, false); } }//drier air moving in later, soupy with fog right now
        public static WeatherFile Fog4 { get { return new WeatherFile("weather\\Fog4.wav", new List<WeatherTypeHash> { WeatherTypeHash.Foggy }, true, false); } }//foggy as can be. 
        public static WeatherFile Fog5 { get { return new WeatherFile("weather\\Fog5.wav", new List<WeatherTypeHash> { WeatherTypeHash.Foggy }, true, false); } }//a lot of moisture, foggy right now
        public static WeatherFile Fog6 { get { return new WeatherFile("weather\\Fog6.wav", new List<WeatherTypeHash> { WeatherTypeHash.Foggy }, true, false); } }//severe fog in the area. 
        public static WeatherFile Fog7 { get { return new WeatherFile("weather\\Fog7.wav", new List<WeatherTypeHash> { WeatherTypeHash.Foggy }, true, false); } }//feeling a little foggy now
        public static WeatherFile Fog8 { get { return new WeatherFile("weather\\Fog8.wav", new List<WeatherTypeHash> { WeatherTypeHash.Foggy }, true, true) { ForcedSponsorName = "Eris" }; } }//fog going to be around for a while. Rob that shoe store, Eris
        public static WeatherFile Fog9 { get { return new WeatherFile("weather\\Fog9.wav", new List<WeatherTypeHash> { WeatherTypeHash.Foggy }, true, false); } }//fog in the area currently
        public static WeatherFile Fog10 { get { return new WeatherFile("weather\\Fog10.wav", new List<WeatherTypeHash> { WeatherTypeHash.Foggy }, true, false); } }//serious delays as fog has blanketed the area
        public static WeatherFile Fog11 { get { return new WeatherFile("weather\\Fog11.wav", new List<WeatherTypeHash> { WeatherTypeHash.Foggy }, true, false); } }//cant see much of anything due to the fog
        public static WeatherFile Fog12 { get { return new WeatherFile("weather\\Fog12.wav", new List<WeatherTypeHash> { WeatherTypeHash.Foggy }, true, false); } }//foggy right now
    }
    private class Rainy
    {
        public static WeatherFile Rain1 { get { return new WeatherFile("weather\\Rain1.wav", new List<WeatherTypeHash> { WeatherTypeHash.Raining, WeatherTypeHash.ThunderStorm }, true, true); ; } }//raid disturbance, expect to see a real deep dicking
        public static WeatherFile Rain2 { get { return new WeatherFile("weather\\Rain2.wav", new List<WeatherTypeHash> { WeatherTypeHash.Raining, WeatherTypeHash.ThunderStorm }, true, true); } }//expect some rain for a while
        public static WeatherFile Rain3 { get { return new WeatherFile("weather\\Rain3.wav", new List<WeatherTypeHash> { WeatherTypeHash.Raining, WeatherTypeHash.ThunderStorm }, true, true); } }//Gonna get some rain right now
        public static WeatherFile Rain5 { get { return new WeatherFile("weather\\Rain5.wav", new List<WeatherTypeHash> { WeatherTypeHash.Raining, WeatherTypeHash.ThunderStorm }, false, true); } }//frontaly boundary coming through a couple storms coming with them
        public static WeatherFile Rain6 { get { return new WeatherFile("weather\\Rain6.wav", new List<WeatherTypeHash> { WeatherTypeHash.Raining, WeatherTypeHash.ThunderStorm }, true, true); } }//Ridge of rain over the city
        public static WeatherFile Rain8 { get { return new WeatherFile("weather\\Rain8.wav", new List<WeatherTypeHash> { WeatherTypeHash.Raining, WeatherTypeHash.ThunderStorm }, false, true); } }//big depressions coming through causing some rain
        public static WeatherFile Rain9 { get { return new WeatherFile("weather\\Rain9.wav", new List<WeatherTypeHash> { WeatherTypeHash.Raining, WeatherTypeHash.ThunderStorm }, false, true); } }//remnanats of tropical storm coming through, lots of rain
        public static WeatherFile Rain10 { get { return new WeatherFile("weather\\Rain10.wav", new List<WeatherTypeHash> { WeatherTypeHash.Raining, WeatherTypeHash.ThunderStorm }, true, true); } }//causing the skies to open up
        public static WeatherFile Rain11 { get { return new WeatherFile("weather\\Rain11.wav", new List<WeatherTypeHash> { WeatherTypeHash.Raining, WeatherTypeHash.ThunderStorm }, true, false); } }//man do we have some rain coming in now
    }
    private class Stormy
    {
        public static WeatherFile Storm1 { get { return new WeatherFile("weather\\Storm1.wav", new List<WeatherTypeHash> { WeatherTypeHash.ThunderStorm, WeatherTypeHash.Blizzard, WeatherTypeHash.Christmas, WeatherTypeHash.Halloween, WeatherTypeHash.Snowing }, true, true); ; } }//thousands of people could be losing power due to the storm
    }
    private class Windy
    {
        public static WeatherFile Windy1 { get { return new WeatherFile("weather\\Windy1.wav", new List<WeatherTypeHash> { WeatherTypeHash.None }, true, true); ; } }//wind is going to be with us for a while
        public static WeatherFile Windy2 { get { return new WeatherFile("weather\\Windy2.wav", new List<WeatherTypeHash> { WeatherTypeHash.None }, true, true); } }//some serious wind coming through next few days
        public static WeatherFile Windy3 { get { return new WeatherFile("weather\\Windy3.wav", new List<WeatherTypeHash> { WeatherTypeHash.None }, true, true); } }//tropical storms causing this wind right now
        public static WeatherFile Windy4 { get { return new WeatherFile("weather\\Windy4.wav", new List<WeatherTypeHash> { WeatherTypeHash.None }, true, true) { ForcedSponsorName = "Wind From Below" }; } }//weve got some wind in the area 
        public static WeatherFile Windy6 { get { return new WeatherFile("weather\\Windy6.wav", new List<WeatherTypeHash> { WeatherTypeHash.None }, true, true); } }//expect some wind then rain, then sun
        public static WeatherFile Windy7 { get { return new WeatherFile("weather\\Windy7.wav", new List<WeatherTypeHash> { WeatherTypeHash.None }, true, true); } }//its windy
        public static WeatherFile Windy8 { get { return new WeatherFile("weather\\Windy8.wav", new List<WeatherTypeHash> { WeatherTypeHash.None }, true, true); } }//windy now
    }
    private class Sunny
    {
        public static WeatherFile Sunny1 { get { return new WeatherFile("weather\\Sunny1.wav", new List<WeatherTypeHash> { WeatherTypeHash.ExtraSunny, WeatherTypeHash.Clear }, true, true) { ForcedSponsorName = "Bank of Liberty" };  } }//five day forecast etc., sunny right now, bank of liverty
        public static WeatherFile Sunny2 { get { return new WeatherFile("weather\\Sunny2.wav", new List<WeatherTypeHash> { WeatherTypeHash.ExtraSunny, WeatherTypeHash.Clear }, true, true); } }//patches of moisture coming down the coast, nice and sunny now
        public static WeatherFile Sunny3 { get { return new WeatherFile("weather\\Sunny3.wav", new List<WeatherTypeHash> { WeatherTypeHash.ExtraSunny, WeatherTypeHash.Clear }, true, true); } }//quiet right now, sunny and warm.
        public static WeatherFile Sunny4 { get { return new WeatherFile("weather\\Sunny4.wav", new List<WeatherTypeHash> { WeatherTypeHash.ExtraSunny, WeatherTypeHash.Clear }, true, true); } }//great weather, daytime heating
        public static WeatherFile Sunny5 { get { return new WeatherFile("weather\\Sunny5.wav", new List<WeatherTypeHash> { WeatherTypeHash.ExtraSunny, WeatherTypeHash.Clear }, true, true); } }//sunny right now with threat of storms.
        public static WeatherFile Sunny6 { get { return new WeatherFile("weather\\Sunny6.wav", new List<WeatherTypeHash> { WeatherTypeHash.ExtraSunny, WeatherTypeHash.Clear }, true, true); } }//sunshine for your soul right now
        public static WeatherFile Sunny7 { get { return new WeatherFile("weather\\Sunny7.wav", new List<WeatherTypeHash> { WeatherTypeHash.ExtraSunny, WeatherTypeHash.Clear }, true, true); } }//sun and clouds possible later, sunny right now
        public static WeatherFile Sunny8 { get { return new WeatherFile("weather\\Sunny8.wav", new List<WeatherTypeHash> { WeatherTypeHash.ExtraSunny, WeatherTypeHash.Clear }, true, true); } }//sunny right now
        public static WeatherFile Sunny9 { get { return new WeatherFile("weather\\Sunny9.wav", new List<WeatherTypeHash> { WeatherTypeHash.ExtraSunny, WeatherTypeHash.Clear }, true, true) { ForcedSponsorName = "Vig Insurance" }; } }//nice and sunny out there, "Vig Insurance"
        public static WeatherFile Sunny10 { get { return new WeatherFile("weather\\Sunny10.wav", new List<WeatherTypeHash> { WeatherTypeHash.ExtraSunny, WeatherTypeHash.Clear }, true, true) { ForcedSponsorName = "Ammunation" }; } }//sun right now
        public static WeatherFile Sunny11 { get { return new WeatherFile("weather\\Sunny11.wav", new List<WeatherTypeHash> { WeatherTypeHash.ExtraSunny, WeatherTypeHash.Clear }, true, true); } }//just a great day, lots of sun
        public static WeatherFile Sunny12 { get { return new WeatherFile("weather\\Sunny12.wav", new List<WeatherTypeHash> { WeatherTypeHash.ExtraSunny, WeatherTypeHash.Clear }, true, true); } }//right now its sunny.
    }
    private class Weazel
    {
        public static WeatherFile Intro { get { return new WeatherFile("weather\\Intro.wav", new List<WeatherTypeHash> { WeatherTypeHash.None }, false, false); ; } }
        public static WeatherFile Outro { get { return new WeatherFile("weather\\Outro.wav", new List<WeatherTypeHash> { WeatherTypeHash.None }, false, false); } }
        public static WeatherFile Outro2 { get { return new WeatherFile("weather\\Outro_News_03.wav", new List<WeatherTypeHash> { WeatherTypeHash.None }, false, false); } }
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
        public string ForcedSponsorName { get; set; } = "";

    }
    private class Sponsor
    {
        public Sponsor(string sponsorName, string notificationPicture)
        {
            SponsorName = sponsorName;
            NotificationPicture = notificationPicture;
        }

        public string SponsorName { get; set; }
        public string NotificationPicture { get; set; }
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