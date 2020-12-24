using LosSantosRED.lsr;
using Rage;
using System.Windows.Forms;

[assembly: Rage.Attributes.Plugin("Los Santos RED", Description = "Total Conversion", Author = "Greskrendtregk")]
public static class EntryPoint
{
    public static ModController ModController { get; set; }
    public static void Main()
    {
        while (Game.IsLoading)
        {
            GameFiber.Yield();
        }

        ModController = new ModController();
        ModController.Start();

        while (true)
        {
            if (!ModController.IsRunning && Game.IsKeyDown(Keys.F10))
            {
                ModController = new ModController();
                ModController.Start();
            }
            GameFiber.Yield();
        }
    }
}