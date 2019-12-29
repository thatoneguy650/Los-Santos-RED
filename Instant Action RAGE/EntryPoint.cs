using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

[assembly: Rage.Attributes.Plugin("Instant Action", Description = "My first mod, please don't hurt me", Author = "Greskrendtregk")]

public static class EntryPoint
{
    public static void Main()
    {
        while (Game.IsLoading)
        {
            GameFiber.Yield();
        }

        ScriptController.Initialize();
        InstantAction.Initialize();
        while (true)
        {
            if(!InstantAction.IsRunning)
            {
                if(Game.IsAltKeyDownRightNow && Game.IsKeyDown(Settings.MenuKey))
                {
                    ScriptController.Initialize();
                    InstantAction.Initialize();
                }
            }
            GameFiber.Yield();
        }


    }
       
}

