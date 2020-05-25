using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class Clock
{
    private static int ClockDayOfWeek;
    private static int ClockSeconds;
    private static int ClockMinutes;
    private static int ClockHours;
    private static uint GameTimeLastSetClock;


    private static int StoredClockSeconds;
    private static int StoredClockMinutes;
    private static int StoredClockHours;

    private static int Interval = 1000;
    private static int ClockMultiplier = 1;

    public static bool IsRunning { get; set; }
    public static string ClockSpeed { get; set; }
    public static string ClockTime { get; set; }
    public static bool OverrideToFastest { get; set; } = false;
    public static void Initialize()
    {
        IsRunning = true;

        NativeFunction.CallByName<int>("PAUSE_CLOCK",true);

        ClockSeconds = NativeFunction.CallByName<int>("GET_CLOCK_SECONDS");
        ClockMinutes = NativeFunction.CallByName<int>("GET_CLOCK_MINUTES");
        ClockHours = NativeFunction.CallByName<int>("GET_CLOCK_HOURS");
    }
    public static void Dispose()
    {
        IsRunning = false;
        NativeFunction.CallByName<int>("PAUSE_CLOCK", false);
    }
    public static void Tick()
    {
        if (IsRunning)
        {
            float Speed = Game.LocalPlayer.Character.Speed;
            if (Speed <= 4.0f)
            {
                Interval = 1000;
                ClockMultiplier = 1;
                ClockSpeed = "1x";
            }
            else if (Speed <= 10.0f)
            {
                Interval = 100;
                ClockMultiplier = 1;
                ClockSpeed = "10x";
            }
            else if (Speed <= 15.0f)
            {
                Interval = 100;
                ClockMultiplier = 2;
                ClockSpeed = "20x";
            }
            else if (Speed <= 30.0f)
            {
                Interval = 10;
                ClockMultiplier = 1;
                ClockSpeed = "100x";
            }
            else if (Speed <= 40.0f)
            {
                Interval = 10;
                ClockMultiplier = 2;
                ClockSpeed = "200x";
            }
            else
            {
                Interval = 10;
                ClockMultiplier = 3;
                ClockSpeed = "300x";
            }

            if (OverrideToFastest)
            {
                Interval = 1;
                ClockMultiplier = 30;
                ClockSpeed = "30000x";
            }

            if (Game.GameTime - GameTimeLastSetClock >= Interval)
            {
                NativeFunction.CallByName<int>("ADD_TO_CLOCK_TIME", 0, 0, ClockMultiplier);
                GameTimeLastSetClock = Game.GameTime;
            }

            //ClockYear = NativeFunction.CallByName<int>("GET_CLOCK_YEAR");
            //ClockMonth = NativeFunction.CallByName<int>("GET_CLOCK_MONTH");
            //ClockDayOfMonth = NativeFunction.CallByName<int>("GET_CLOCK_DAY_OF_MONTH");
            ClockDayOfWeek = NativeFunction.CallByName<int>("GET_CLOCK_DAY_OF_WEEK");
            ClockSeconds = NativeFunction.CallByName<int>("GET_CLOCK_SECONDS");
            ClockMinutes = NativeFunction.CallByName<int>("GET_CLOCK_MINUTES");
            ClockHours = NativeFunction.CallByName<int>("GET_CLOCK_HOURS");

            DateTime MyTime = new DateTime(2020, 1, 1, ClockHours, ClockMinutes, ClockSeconds);

            string DOW = "Sunday";
            if (ClockDayOfWeek == 0)
                DOW = "Sunday";
            else if (ClockDayOfWeek == 1)
                DOW = "Monday";
            else if (ClockDayOfWeek == 2)
                DOW = "Tuesday";
            else if (ClockDayOfWeek == 3)
                DOW = "Wednesday";
            else if (ClockDayOfWeek == 4)
                DOW = "Thrusday";
            else if (ClockDayOfWeek == 5)
                DOW = "Friday";
            else if (ClockDayOfWeek == 6)
                DOW = "Saturday";

            ClockTime = string.Format("{0} {1}", DOW, MyTime.ToString("hh:mm tt"));
        }
    }
    public static void StoreTime()
    {
        StoredClockSeconds = NativeFunction.CallByName<int>("GET_CLOCK_SECONDS");
        StoredClockMinutes = NativeFunction.CallByName<int>("GET_CLOCK_MINUTES");
        StoredClockHours = NativeFunction.CallByName<int>("GET_CLOCK_HOURS");
    }
    public static void ResetTime()
    {
        NativeFunction.CallByName<int>("SET_CLOCK_TIME", StoredClockHours, StoredClockMinutes, StoredClockSeconds); 
    }

}

