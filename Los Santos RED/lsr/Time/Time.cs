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
        private int HourToStop = 0;
        private int ClockMultiplier = 1;
        private uint GameTimeLastSetClock;
        private int Interval = 1000;
        private bool IsPaused;
        private int StoredClockHours;
        private int StoredClockMinutes;
        private int StoredClockSeconds;
        private ISettingsProvideable Settings;
        private string TimeTestString = "";
        public Time(ISettingsProvideable settings)
        {
            Settings = settings;
            NativeFunction.CallByName<int>("PAUSE_CLOCK", true);
        }
        public string CurrentTime => CurrentDateTime.ToString("ddd, dd MMM yyyy hh:mm tt") + (CurrentTimeMultiplier != "1x" ? " (" + CurrentTimeMultiplier + ")" : "");
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
        public void FastForward(int hoursToFastForward)
        {

            //Game.DisplayHelp($"Fastforward Started Press O to Stop");

            GetIntervalAndMultiplier();
            if (!IsFastForwarding)
            {
                IsFastForwarding = true;
                int HoursFastForwarded = 0;
                int prevCurrentHour = CurrentHour;
                uint GameTimeStartedFastForwarding = Game.GameTime;
                Game.TimeScale = 10f;
                EntryPoint.WriteToConsole($"FASTFORWARD START CurrentHour {CurrentHour}  CurrentTime {CurrentTime} HoursFastForwarded {HoursFastForwarded} ClockMultiplier {ClockMultiplier} Interval {Interval}", 5);
                GameFiber FastForwardTime = GameFiber.StartNew(delegate
                {
                    while (HoursFastForwarded < hoursToFastForward)// && Game.GameTime - GameTimeStartedFastForwarding <= 60000)// && !Game.IsKeyDown(Keys.O))
                    {
                        if(prevCurrentHour != CurrentHour)
                        {
                            HoursFastForwarded++;
                            prevCurrentHour = CurrentHour;
                            EntryPoint.WriteToConsole($"FASTFORWARD INCREASE CurrentHour {CurrentHour}  CurrentTime {CurrentTime} HoursFastForwarded {HoursFastForwarded} ClockMultiplier {ClockMultiplier} Interval {Interval}", 5);
                        }
                        GameFiber.Yield();
                    }
                    IsFastForwarding = false;
                    Game.TimeScale = 1f;
                    //Game.DisplayHelp($"Fastforward Stopped");
                }, "FastForwardTime");

            }
        }
        public void UnPauseTime()
        {
            //EntryPoint.WriteToConsole(string.Format("Unpaused Time At: {0}:{1}:{2}", StoredClockHours, StoredClockMinutes, StoredClockSeconds));
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
        private void CheckTimeInterval()
        {
            if (Game.GameTime - GameTimeLastSetClock >= Interval)
            {
                NativeFunction.CallByName<int>("ADD_TO_CLOCK_TIME", 0, 0, ClockMultiplier);
                CurrentHour = NativeFunction.CallByName<int>("GET_CLOCK_HOURS");
                CurrentMinute = NativeFunction.CallByName<int>("GET_CLOCK_MINUTES");
                CurrentSecond = NativeFunction.CallByName<int>("GET_CLOCK_SECONDS");


                CurrentDay = NativeFunction.Natives.GET_CLOCK_DAY_OF_MONTH<int>();
                CurrentMonth = NativeFunction.Natives.GET_CLOCK_MONTH<int>() + 1;
                CurrentYear = NativeFunction.Natives.GET_CLOCK_YEAR<int>();
                //TimeTestString = CurrentYear.ToString() + "-" + CurrentMonth.ToString() + "-" + CurrentDay.ToString() + "-   {}   " + CurrentHour.ToString() + "-" + CurrentMinute.ToString() + "-" + CurrentSecond.ToString() + "-";
                CurrentDateTime = new DateTime(CurrentYear, CurrentMonth, CurrentDay, CurrentHour, CurrentMinute, CurrentSecond);

                if (CurrentHour >= 20 || CurrentHour <= 6)//8?7?pm to 6 am lights need to be on
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
                    Interval = 1000;
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
                Interval = 10;
                ClockMultiplier = 300;
            }
        }
        private void SetToStoredTime()
        {
            //Mod.Debugging.WriteToLog("StoreTime", string.Format("Time: {0}:{1}:{2}", StoredClockHours, StoredClockMinutes, StoredClockSeconds));
            NativeFunction.CallByName<int>("SET_CLOCK_TIME", StoredClockHours, StoredClockMinutes, StoredClockSeconds);
        }
        private void StoreTime()
        {
            StoredClockSeconds = NativeFunction.CallByName<int>("GET_CLOCK_SECONDS");
            StoredClockMinutes = NativeFunction.CallByName<int>("GET_CLOCK_MINUTES");
            StoredClockHours = NativeFunction.CallByName<int>("GET_CLOCK_HOURS");
            //EntryPoint.WriteToConsole(string.Format("Paused Time At: {0}:{1}:{2}", StoredClockHours, StoredClockMinutes, StoredClockSeconds));
        }
    }
}