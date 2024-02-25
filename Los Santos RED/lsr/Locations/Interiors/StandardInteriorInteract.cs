using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class StandardInteriorInteract : InteriorInteract
{
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
        SetupCamera(false);
        if (!MoveToPosition())
        {
            Interior.IsMenuInteracting = false;
            Game.DisplayHelp("Access Failed");
            LocationCamera?.StopImmediately(true);
            return;
        }
        while(Game.GameTime - GameTimeStarted <= 1500)
        {
            GameFiber.Yield();
        }
        Player.InteriorManager.OnStartedInteriorInteract();
        InteractableLocation?.StandardInteract(LocationCamera, true);   
        while (Player.IsAliveAndFree && Player.ActivityManager.IsInteractingWithLocation)
        {
            GameFiber.Yield();
        }
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

