using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

[assembly: Rage.Attributes.Plugin("Los Santos RED", Description = "Total Conversion", Author = "Greskrendtregk")]
public static class EntryPoint
{
    private static int LogLevel = 4;//most non spammy stuff + all debug
    /*enum? Use built in trace stuff?
     *  0 = Errors
        1 = ?
        2 = ?
        3 = On Demand
        4 = Debug
        5 = Common Trace */
    public static ModController ModController { get; set; }
    public static void Main()
    {
        while (Game.IsLoading)
        {
            GameFiber.Yield();
        }  
        Loop();
    }
    private static void Loop()
    {
        Game.DisplayNotification("~s~Los Santos ~r~RED ~s~v0.1 ~n~By ~g~Greskrendtregk ~n~~s~Press F10 to Start");
        while (true)
        {
            if (Game.IsKeyDown(Keys.F10))
            {
                if (ModController == null || !ModController.IsRunning)
                {
                    ModController = new ModController();
                    ModController.Start();
                }
            }
            GameFiber.Yield();
        }
    }
    public static void WriteToConsole(string Message, int level)
    {
        if (level <= LogLevel)
        {
            Game.Console.Print(Message);
        }
    }
}