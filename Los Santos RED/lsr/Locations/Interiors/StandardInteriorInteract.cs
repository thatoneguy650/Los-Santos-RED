using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class StandardInteriorInteract : InteriorInteract
{
    public bool DisableCamera { get; set; } = false;
    public bool DisableMovement { get; set; } = false;
    public StandardInteriorInteract()
    {
    }

    public StandardInteriorInteract(string name, Vector3 position, float heading, string buttonPromptText) : base(name, position, heading, buttonPromptText)
    {

    }
    public override void OnInteract()
    {
        Interior.IsMenuInteracting = true;
        Interior?.RemoveButtonPrompts();
        RemovePrompt();
        uint GameTimeStarted = Game.GameTime;

        if (!DisableCamera)
        {

            SetupCamera(false);
        }
        else
        {
            if(Game.IsScreenFadedOut)
            {
                Game.FadeScreenIn(1500, true);
            }
        }

        if (!DisableMovement)
        {

            if (!MoveToPosition())
            {
                Interior.IsMenuInteracting = false;
                Game.DisplayHelp("Access Failed");
                LocationCamera?.StopImmediately(true);
                return;
            }
            while (Game.GameTime - GameTimeStarted <= 1500)
            {
                GameFiber.Yield();
            }
        }
        Player.InteriorManager.OnStartedInteriorInteract();
        EntryPoint.WriteToConsole($"StandardInteriorInteract GOT TO STANDARD INTERACT IsScreenFadedOut:{Game.IsScreenFadedOut}");
        InteractableLocation?.StandardInteract(LocationCamera, true);   
        while (Player.IsAliveAndFree && Player.ActivityManager.IsInteractingWithLocation)
        {
            GameFiber.Yield();
        }
        Interior.IsMenuInteracting = false;
        Player.InteriorManager.OnEndedInteriorInteract();
    }
    public override void AddPrompt()
    {
        if (Player == null)
        {
            return;
        }
        Player.ButtonPrompts.AttemptAddPrompt(Name, ButtonPromptText, Name, Settings.SettingsManager.KeySettings.InteractStart, 999);
    }
}

