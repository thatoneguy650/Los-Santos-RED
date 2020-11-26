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
    private static int ClockYear;
    private static int ClockMonth;
    private static int ClockDayOfMonth;
    private static int ClockDayOfWeek;
    private static int ClockSeconds;
    private static int ClockMinutes;
    private static int ClockHours;
    private static uint GameTimeLastSetClock;
    private static bool IsPaused;

    private static int StoredClockSeconds;
    private static int StoredClockMinutes;
    private static int StoredClockHours;

    private static int Interval = 1000;
    private static int ClockMultiplier = 1;

    public static bool IsRunning { get; set; }
    public static bool OverrideToFastest { get; set; } = false;
    public static string DayOfWeek
    {
        get
        {
            if (ClockDayOfWeek == 0)
                return "Sunday";
            else if (ClockDayOfWeek == 1)
                return "Monday";
            else if (ClockDayOfWeek == 2)
                return "Tuesday";
            else if (ClockDayOfWeek == 3)
                return "Wednesday";
            else if (ClockDayOfWeek == 4)
                return "Thrusday";
            else if (ClockDayOfWeek == 5)
                return "Friday";
            else if (ClockDayOfWeek == 6)
                return "Saturday";
            else
                return "Sunday";
        }
    }
   public static string CurrentTime
    {
        get
        {
            return string.Format("Current Time: {0}:{1}:{2}", NativeFunction.CallByName<int>("GET_CLOCK_HOURS"), NativeFunction.CallByName<int>("GET_CLOCK_MINUTES"), NativeFunction.CallByName<int>("GET_CLOCK_SECONDS"));
        }
    }
    
    public static void Initialize()
    {
        IsRunning = true;

        ClockSeconds = NativeFunction.CallByName<int>("GET_CLOCK_SECONDS");
        ClockMinutes = NativeFunction.CallByName<int>("GET_CLOCK_MINUTES");
        ClockHours = NativeFunction.CallByName<int>("GET_CLOCK_HOURS");
        NativeFunction.CallByName<int>("PAUSE_CLOCK",true);      
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
            if (IsPaused)
            {
                SetToStoredTime();
            }
            else
            {
                GetIntervalAndMultiplier();
                CheckTimeInterval();
            }
        }
    }
    public static void PauseTime()
    {
        StoreTime();
        IsPaused = true;
    }
    public static void UnpauseTime()
    {
        Debugging.WriteToLog("Clock", string.Format("Unpaused Time At: {0}:{1}:{2}", StoredClockHours, StoredClockMinutes, StoredClockSeconds));
        IsPaused = false;


        GameFiber UnPauseTime = GameFiber.StartNew(delegate
        {
            uint GameTimeStartedResettingTime = Game.GameTime;
            while(Game.GameTime - GameTimeStartedResettingTime <= 3000)
            {
                SetToStoredTime();
                GameFiber.Yield();
            }
        }, "UnPauseTime");
        Debugging.GameFibers.Add(UnPauseTime);

    }
    private static void StoreTime()
    {
        StoredClockSeconds = NativeFunction.CallByName<int>("GET_CLOCK_SECONDS");
        StoredClockMinutes = NativeFunction.CallByName<int>("GET_CLOCK_MINUTES");
        StoredClockHours = NativeFunction.CallByName<int>("GET_CLOCK_HOURS");
        Debugging.WriteToLog("Clock", string.Format("Paused Time At: {0}:{1}:{2}", StoredClockHours, StoredClockMinutes, StoredClockSeconds));
    }
    private static void SetToStoredTime()
    {
        //Debugging.WriteToLog("StoreTime", string.Format("Time: {0}:{1}:{2}", StoredClockHours, StoredClockMinutes, StoredClockSeconds));
        NativeFunction.CallByName<int>("SET_CLOCK_TIME", StoredClockHours, StoredClockMinutes, StoredClockSeconds); 
    }
    private static void GetIntervalAndMultiplier()
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
        if (OverrideToFastest)
        {
            Interval = 1;
            ClockMultiplier = 30;
        }  
    }
    private static void CheckTimeInterval()
    {
        if (Game.GameTime - GameTimeLastSetClock >= Interval)
        {
            NativeFunction.CallByName<int>("ADD_TO_CLOCK_TIME", 0, 0, ClockMultiplier);
            GameTimeLastSetClock = Game.GameTime;
        }
    }
    private static void StoreDate()
    {
        ClockYear = NativeFunction.CallByName<int>("GET_CLOCK_YEAR");
        ClockMonth = NativeFunction.CallByName<int>("GET_CLOCK_MONTH");
        ClockDayOfMonth = NativeFunction.CallByName<int>("GET_CLOCK_DAY_OF_MONTH");
        ClockDayOfWeek = NativeFunction.CallByName<int>("GET_CLOCK_DAY_OF_WEEK");
    }
}

