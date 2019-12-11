using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

internal static class RespawnStopper
{
    public static bool IsRunning { get; set; } = true;
    public static void Initialize()
    {
        IntPtr MyPtr = Game.GetScriptGlobalVariableAddress(4);
        Marshal.WriteInt32(MyPtr, 1);
        Game.TerminateAllScriptsWithName("respawn_controller");
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
                        if (Settings.AllowDeathMenus)
                        {
                            Game.DisableAutomaticRespawn = true;
                            //GameFiber.Yield();
                            Game.FadeScreenOutOnDeath = false;
                            //GameFiber.Yield();
                            //Game.TerminateAllScriptsWithName("respawn_controller");
                            //GameFiber.Yield();
                            Game.TerminateAllScriptsWithName("selector");
                            // GameFiber.Yield();

                            //NativeFunction.Natives.x4A18E01DF2C87B86(false);
                            //Function.Call(Hash.SET_FADE_OUT_AFTER_DEATH, false);
                            NativeFunction.Natives.x1E0B4DC0D990A4E7(false);
                            // GameFiber.Yield();
                            //Function.Call(Hash.SET_FADE_OUT_AFTER_ARREST, false);
                            NativeFunction.Natives.x21FFB63D8C615361(true);
                            // GameFiber.Yield();
                            //Function.Call(Hash.IGNORE_NEXT_RESTART, true);
                            //NativeFunction.Natives.x2C2B3493FBF51C71(true);
                            //Function.Call(Hash._DISABLE_AUTOMATIC_RESPAWN, true);
                            //NativeFunction.Natives.x9DC711BC69C548DF("respawn_controller");
                            //Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "respawn_controller");

                            //NativeFunction.Natives.x9DC711BC69C548DF("selector");
                        }
                        else
                        {
                            //Game.DisableAutomaticRespawn = false;
                            //Game.FadeScreenOutOnDeath = true;
                            //Game.StartNewScript("respawn_controller");
                            //Game.StartNewScript("selector");
                            //NativeFunction.Natives.x1E0B4DC0D990A4E7(true);
                            //NativeFunction.Natives.x21FFB63D8C615361(false);
                        }
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    InstantAction.Dispose();
                    Debugging.WriteToLog("Error", e.Message + " : " + e.StackTrace);
                }
            });
    }
    // Returns pointer to global variable
    // make sure that you check game version before accessing globals because
    // ids may differ between patches
    private static IntPtr GetGlobalPtr(int globalId)
    {
        return Game.GetScriptGlobalVariableAddress(globalId);
    }
}


