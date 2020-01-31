using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Speed
{
    private static bool SpeedModeBombActive;
    private static Vehicle SpeedBus;
    public static bool IsRunning;
    public static void Initialize()
    {
        IsRunning = true;
        SpeedModeBombActive = false;
        SpeedBus = null;
        MainLoop();
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    private static void MainLoop()
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                while (IsRunning)
                {
                    CheckSpeed();
                    GameFiber.Sleep(200);
                }
            }
            catch (Exception e)
            {
                LosSantosRED.Dispose();
                Debugging.WriteToLog("Error", e.Message + " : " + e.StackTrace);
            }
        });
    }
    public static void CheckSpeed()
    {
        if (Settings.Keanu)
        {
            if (SpeedModeBombActive)
            {
                if (!SpeedBus.Exists())
                {
                    SpeedModeBombActive = false;
                }
                if (SpeedBus.Speed < 22.352f)
                {
                    SpeedBus.Explode();
                    SpeedModeBombActive = false;
                    Game.DisplaySubtitle("BOOM");
                }
            }
            else if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Model.Name == "BUS" && Game.LocalPlayer.Character.CurrentVehicle.Speed >= 22.352f)
            {
                SpeedBus = Game.LocalPlayer.Character.CurrentVehicle;
                SpeedModeBombActive = true;
                Game.DisplaySubtitle("Bomb Activated, Don't Drop Below 50 MPH");
                DispatchAudio.PopQuizHotShot();
            }
        }
    }

}

