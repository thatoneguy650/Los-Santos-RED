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

        //ModController = new ModController();
        //ModController.Start();
        Game.DisplayNotification("~s~Los Santos ~r~RED ~s~v0.1 ~n~By ~g~Greskrendtregk ~n~~s~Press F10 to Start");
        while (true)
        {
            if(Game.IsKeyDown(Keys.F10))
            {
                if(ModController == null)
                {
                    ModController = new ModController();
                    ModController.Start();
                }
                else if (!ModController.IsRunning)
                {
                    ModController = new ModController();
                    ModController.Start();
                }
            }
            GameFiber.Yield();
        }
    }
    public static bool IsMoveControlPressed
    {
        get
        {
            if (Game.IsControlPressed(2, GameControl.MoveUpOnly) || Game.IsControlPressed(2, GameControl.MoveRight) || Game.IsControlPressed(2, GameControl.MoveDownOnly) || Game.IsControlPressed(2, GameControl.MoveLeft))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}