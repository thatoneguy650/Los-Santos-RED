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

namespace Mod
{
    public class Time : ITimeControllable, ITimeReportable
    {
        private int ClockMultiplier = 1;
        private uint GameTimeLastSetClock;
        private int Interval = 1000;
        private bool IsPaused;
        private int StoredClockHours;
        private int StoredClockMinutes;
        private int StoredClockSeconds;
        private ISettingsProvideable Settings;
        private int DefaultMultiplier = 1;
        private int DefaultInterval = 1000;
        private int FastForwardMultiplier = 300;
        private int FastForwardInterval = 10;
        private DateTime TimeToStopFastForwarding;
        public Time(ISettingsProvideable settings)
        {
            Settings = settings;
            NativeFunction.CallByName<int>("PAUSE_CLOCK", true);
        }
        public string CurrentTime => Settings.SettingsManager.UISettings.PlayerStatusSimpleTime ? CurrentDateTime.ToString("ddd hh:mm tt") : CurrentDateTime.ToString("ddd, dd MMM yyyy hh:mm tt");// + (CurrentTimeMultiplier != "1x" ? " (" + CurrentTimeMultiplier + ")" : "");
        public string CurrentTimeMultiplier => (ClockMultiplier * 1000 / Interval).ToString() + "x";
        public DateTime CurrentDateTime { get; private set; }
        public int CurrentHour { get; private set; } = 0;
        public int CurrentMinute { get; private set; } = 0;
        public int CurrentSecond { get; private set; } = 0;
        public int CurrentDay { get; private set; } = 1;
        public int CurrentYear { get; private set; } = 2021;
        public int CurrentMonth { get; private set; } = 1;
        public bool IsNight { get; private set; } = false;
        public bool IsFastForwarding { get; private set; } = false;   
        public void Dispose()
        {
            NativeFunction.CallByName<int>("PAUSE_CLOCK", false);
        }
        public void PauseTime()
        {
            StoreTime();
            IsPaused = true;
        }
        public void UnPauseTime()
        {
            IsPaused = false;
            GameFiber UnPauseTime = GameFiber.StartNew(delegate
            {
                uint GameTimeStartedResettingTime = Game.GameTime;
                while (Game.GameTime - GameTimeStartedResettingTime <= 3000)
                {
                    SetToStoredTime();
                    GameFiber.Yield();
                }
            }, "UnPauseTime");

        }
        public void Tick()
        {
            if (IsPaused)
            {
                SetToStoredTime();
            }
            else
            {
                if (Settings.SettingsManager.TimeSettings.ScaleTime)
                {
                    GetIntervalAndMultiplier();
                }
                CheckTimeInterval();
            }
        }
        public void SetDateToToday()
        {
            NativeFunction.Natives.SET_CLOCK_DATE(DateTime.Now.Day, DateTime.Now.Month-1, DateTime.Now.Year);
        }
        public void FastForward(DateTime untilTime)
        {
            if (!IsFastForwarding)
            {
                IsFastForwarding = true;
                GetIntervalAndMultiplier();
                TimeToStopFastForwarding = untilTime;
            }
        }
        public void FastForward(int HoursTo)
        {
            if (!IsFastForwarding)
            {
                IsFastForwarding = true;
                GetIntervalAndMultiplier();
                TimeToStopFastForwarding = CurrentDateTime.AddHours(HoursTo);
            }
        }
        private void CheckTimeInterval()
        {
            if (Game.GameTime - GameTimeLastSetClock >= Interval)
            {
                if (IsFastForwarding)
                {
                    if (DateTime.Compare(CurrentDateTime.AddSeconds(ClockMultiplier), TimeToStopFastForwarding) >= 0)
                    {
                        EntryPoint.WriteToConsole($"CURRENT TIME SLOWING FAST FORWARD {CurrentTime}", 5);
                        ClockMultiplier = 10;

                    }
                    if (DateTime.Compare(CurrentDateTime, TimeToStopFastForwarding) >= 0)
                    {
                        EntryPoint.WriteToConsole($"CURRENT TIME STOPPING FAST FORWARD {CurrentTime}", 5);
                        IsFastForwarding = false;
                        ClockMultiplier = DefaultMultiplier;
                        Interval = DefaultInterval;
                    }
                }
                if (Settings.SettingsManager.TimeSettings.ScaleTime || IsFastForwarding)
                {
                    NativeFunction.CallByName<int>("ADD_TO_CLOCK_TIME", 0, 0, ClockMultiplier);
                }
                CurrentHour = NativeFunction.CallByName<int>("GET_CLOCK_HOURS");
                CurrentMinute = NativeFunction.CallByName<int>("GET_CLOCK_MINUTES");
                CurrentSecond = NativeFunction.CallByName<int>("GET_CLOCK_SECONDS");
                CurrentDay = NativeFunction.Natives.GET_CLOCK_DAY_OF_MONTH<int>();
                CurrentMonth = NativeFunction.Natives.GET_CLOCK_MONTH<int>() + 1;
                CurrentYear = NativeFunction.Natives.GET_CLOCK_YEAR<int>();
                CurrentDateTime = new DateTime(CurrentYear, CurrentMonth, CurrentDay, CurrentHour, CurrentMinute, CurrentSecond);
                if (CurrentHour > 19 || (CurrentHour == 19 && CurrentMinute >= 30) || CurrentHour < 6 || (CurrentHour == 6 && CurrentMinute < 30))//7:30pm to 6:30 am lights need to be on
                {
                    IsNight = true;
                }
                else
                {
                    IsNight = false;
                }
                GameTimeLastSetClock = Game.GameTime;
            }
        }
        private void GetIntervalAndMultiplier()
        {
            if (!IsFastForwarding)
            {
                float Speed = Game.LocalPlayer.Character.Speed;
                if (Speed <= 4.0f)
                {
                    Interval = DefaultInterval;
                    ClockMultiplier = 1;
                }
                else if (Speed <= 10.0f)
                {
                    Interval = 100;
                    ClockMultiplier = 1;
                }
                else if (Speed <= 15.0f)
                {
                    Interval = 100;
                    ClockMultiplier = 2;
                }
                else if (Speed <= 30.0f)
                {
                    Interval = 10;
                    ClockMultiplier = 1;
                }
                else if (Speed <= 40.0f)
                {
                    Interval = 10;
                    ClockMultiplier = 2;
                }
                else
                {
                    Interval = 10;
                    ClockMultiplier = 3;
                }
            }
            else
            {
                Interval = FastForwardInterval;
                ClockMultiplier = FastForwardMultiplier;
            }
        }
        private void SetToStoredTime()
        {
            NativeFunction.CallByName<int>("SET_CLOCK_TIME", StoredClockHours, StoredClockMinutes, StoredClockSeconds);
        }
        private void StoreTime()
        {
            StoredClockSeconds = NativeFunction.CallByName<int>("GET_CLOCK_SECONDS");
            StoredClockMinutes = NativeFunction.CallByName<int>("GET_CLOCK_MINUTES");
            StoredClockHours = NativeFunction.CallByName<int>("GET_CLOCK_HOURS");
        }
    }
}