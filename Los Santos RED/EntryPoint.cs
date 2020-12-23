using LosSantosRED.lsr;
using Rage;
using System.Windows.Forms;

[assembly: Rage.Attributes.Plugin("Los Santos RED", Description = "Total Conversion", Author = "Greskrendtregk")]
public static class EntryPoint
{
    public static void Main()
    {
        while (Game.IsLoading)
        {
            GameFiber.Yield();
        }

        Mod.Start();

        while (true)
        {
            if (!Mod.IsRunning && Game.IsKeyDown(Keys.F10))
            {
                Mod.Start();
            }
            GameFiber.Yield();
        }
    }
}