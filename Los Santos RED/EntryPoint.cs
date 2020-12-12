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
        Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~g~Loaded", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~v0.1 Loaded by Greskrendtregk");
        LosSantosRED.lsr.Mod.Initialize();
        Debugging.Initialize();


        while(true)
        {
            if(!LosSantosRED.lsr.Mod.IsRunning && Game.IsKeyDown(Keys.F10))
            {
                LosSantosRED.lsr.Mod.Initialize();
                Debugging.Initialize();
            }
            GameFiber.Yield();
        }
    }
       
}

