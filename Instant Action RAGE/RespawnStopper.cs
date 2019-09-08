using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

class RespawnStopper
{
    public bool IsRunning { get; set; }
    public RespawnStopper()
    {

    }
    public void Init()
    {
        IsRunning = true;
        Game.TerminateAllScripts("selector");
    }
    public void Run()
    {
        while (IsRunning)
        {
            Game.DisableAutomaticRespawn = true;
            Game.FadeScreenOutOnDeath = false;
            Game.TerminateAllScriptsWithName("respawn_controller");
            GameFiber.Yield();
            if (Game.IsKeyDown(Keys.NumPad0))
            {
                Game.DisplayNotification("Howdy");
            }
        }
    }

}

