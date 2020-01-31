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

[assembly: Rage.Attributes.Plugin("Los Santos RED", Description = "Uh Oh", Author = "Greskrendtregk")]

public static class EntryPoint
{
    public static void Main()
    {
        while (Game.IsLoading)
        {
            GameFiber.Yield();
        }

        ScriptController.Initialize();
        LosSantosRED.Initialize();
        while (true)
        {
            if(!LosSantosRED.IsRunning)
            {
                if(Game.IsAltKeyDownRightNow && Game.IsKeyDown(Settings.MenuKey))
                {
                    ScriptController.Initialize();
                    LosSantosRED.Initialize();
                }
            }
            GameFiber.Yield();
        }


    }
       
}

