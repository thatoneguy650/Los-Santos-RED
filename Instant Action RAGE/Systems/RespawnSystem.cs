using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

internal static class RespawnStopper
{
    public static bool IsRunning { get; set; } = true;
    public static void Initialize()
    {
        //Game.TerminateAllScripts("selector");
        MainLoop();
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void MainLoop()
        {
            GameFiber.StartNew(delegate
            {
                try
                {
                    while (IsRunning)
                    {

                        //if (Game.IsKeyDown(Keys.E)) // Our menu on/off switch.
                        //{
                        //    Game.TerminateAllScripts("selector");
                        //}
                        Game.DisableAutomaticRespawn = true;
                        Game.FadeScreenOutOnDeath = false;
                        Game.TerminateAllScriptsWithName("respawn_controller");

                        NativeFunction.Natives.x4A18E01DF2C87B86(false);
                        //Function.Call(Hash.SET_FADE_OUT_AFTER_DEATH, false);
                        NativeFunction.Natives.x1E0B4DC0D990A4E7(false);
                        //Function.Call(Hash.SET_FADE_OUT_AFTER_ARREST, false);
                        NativeFunction.Natives.x21FFB63D8C615361(true);
                        //Function.Call(Hash.IGNORE_NEXT_RESTART, true);
                        NativeFunction.Natives.x2C2B3493FBF51C71(true);
                        //Function.Call(Hash._DISABLE_AUTOMATIC_RESPAWN, true);
                        NativeFunction.Natives.x9DC711BC69C548DF("respawn_controller");
                        //Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "respawn_controller");


                        NativeFunction.Natives.x9DC711BC69C548DF("selector");
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    InstantAction.Dispose();
                    InstantAction.WriteToLog("Error", e.Message + " : " + e.StackTrace);
                }
            });
        }
    }

