using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class ClockSystem
{
    private static int ClockSeconds;
    private static int ClockMinutes;
    private static int ClockHours;
    private static uint GameTimeLastSetClock;
    private static int ClockMultiplier;

    public static bool IsRunning { get; set; }
    public static void Initialize()
    {
        IsRunning = true;

        NativeFunction.CallByName<int>("PAUSE_CLOCK",true);

        ClockSeconds = NativeFunction.CallByName<int>("GET_CLOCK_SECONDS");
        ClockMinutes = NativeFunction.CallByName<int>("GET_CLOCK_MINUTES");
        ClockHours = NativeFunction.CallByName<int>("GET_CLOCK_HOURS");
        MainLoop();
    }
    public static void MainLoop()
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                while (IsRunning)
                {
                    ClockTick();
                    GameFiber.Yield();
                }
            }
            catch (Exception e)
            {
                Dispose();
                Debugging.WriteToLog("Error", e.Message + " : " + e.StackTrace);
            }
        });
    }
    public static void Dispose()
    {
        IsRunning = false;
        NativeFunction.CallByName<int>("PAUSE_CLOCK", false);
    }
    private static void ClockTick()
    {
        float Speed = Game.LocalPlayer.Character.Speed;
        int Interval = 1000;
        if (Speed <= 4.0f)
        {
            Interval = 1000;
            ClockMultiplier = 1;
        }
        else if(Speed <= 10.0f)
        {
            Interval = 100;
            ClockMultiplier = 1;
        }
        else if (Speed <= 15.0f)
        {
            Interval = 10;
            ClockMultiplier = 2;
        }
        else if (Speed <= 30.0f)
        {
            Interval = 10;
            ClockMultiplier = 3;
        }
        else if (Speed <= 40.0f)
        {
            Interval = 10;
            ClockMultiplier = 4;
        }
        else 
        {
            Interval = 10;
            ClockMultiplier = 5;

        }

        if (Game.GameTime - GameTimeLastSetClock >= Interval)
        {
            NativeFunction.CallByName<int>("ADD_TO_CLOCK_TIME", 0, 0, ClockMultiplier);
            GameTimeLastSetClock = Game.GameTime;
        }


        ClockSeconds = NativeFunction.CallByName<int>("GET_CLOCK_SECONDS");
        ClockMinutes = NativeFunction.CallByName<int>("GET_CLOCK_MINUTES");
        ClockHours = NativeFunction.CallByName<int>("GET_CLOCK_HOURS");

        UI.Text(string.Format("Current Time: {0}:{1}:{2}  Multiplier: {3}",ClockHours,ClockMinutes,ClockSeconds, ClockMultiplier), 0.82f, 0.16f, 0.35f, false, Color.White, UI.EFont.FontChaletComprimeCologne);
    }
}

