using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Time
{
    private uint GameTimeLastSetClock;
    private bool IsPaused;
    private int StoredClockSeconds;
    private int StoredClockMinutes;
    private int StoredClockHours;
    private int Interval = 1000;
    private int ClockMultiplier = 1;

    public Time()
    {
        NativeFunction.CallByName<int>("PAUSE_CLOCK", true);
    }
    public bool IsNight { get; private set; }
    public string CurrentTime
    {
        get
        {
            return string.Format("Current Time: {0}:{1}:{2}", NativeFunction.CallByName<int>("GET_CLOCK_HOURS"), NativeFunction.CallByName<int>("GET_CLOCK_MINUTES"), NativeFunction.CallByName<int>("GET_CLOCK_SECONDS"));
        }
    }
    public void Dispose()
    {
        NativeFunction.CallByName<int>("PAUSE_CLOCK", false);
    }
    public void Tick()
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
    public void PauseTime()
    {
        StoreTime();
        IsPaused = true;
    }
    public void UnpauseTime()
    {
        Game.Console.Print(string.Format("Unpaused Time At: {0}:{1}:{2}", StoredClockHours, StoredClockMinutes, StoredClockSeconds));
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
    private void StoreTime()
    {
        StoredClockSeconds = NativeFunction.CallByName<int>("GET_CLOCK_SECONDS");
        StoredClockMinutes = NativeFunction.CallByName<int>("GET_CLOCK_MINUTES");
        StoredClockHours = NativeFunction.CallByName<int>("GET_CLOCK_HOURS");
        Game.Console.Print(string.Format("Paused Time At: {0}:{1}:{2}", StoredClockHours, StoredClockMinutes, StoredClockSeconds));
    }
    private void SetToStoredTime()
    {
        //Mod.Debugging.WriteToLog("StoreTime", string.Format("Time: {0}:{1}:{2}", StoredClockHours, StoredClockMinutes, StoredClockSeconds));
        NativeFunction.CallByName<int>("SET_CLOCK_TIME", StoredClockHours, StoredClockMinutes, StoredClockSeconds);
    }
    private void GetIntervalAndMultiplier()
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
    private void CheckTimeInterval()
    {

        if (Game.GameTime - GameTimeLastSetClock >= Interval)
        {
            NativeFunction.CallByName<int>("ADD_TO_CLOCK_TIME", 0, 0, ClockMultiplier);         
            var HourOfDay = NativeFunction.CallByName<int>("GET_CLOCK_HOURS");
            if (HourOfDay >= 19 || HourOfDay <= 6)//7pm to 6 am lights need to be on
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
}