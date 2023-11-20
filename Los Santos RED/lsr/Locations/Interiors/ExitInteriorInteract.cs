using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ExitInteriorInteract : InteriorInteract
{
    public ExitInteriorInteract()
    {
    }

    public ExitInteriorInteract(Vector3 position, float heading, string buttonPromptText) : base(position, heading, buttonPromptText)
    {

    }
    public override void OnInteract()
    {
        //Interior.IsMenuInteracting = true;
        //NativeFunction.Natives.TASK_FOLLOW_NAV_MESH_TO_COORD(Player.Character, Position.X, Position.Y, Position.Z, 1.0f, -1, 0.1f, 0, Heading);
        //GameFiber.Sleep(500);
        MoveToPosition();
        RemovePrompt();
        if (Interior != null)
        {
            Interior.RemoveButtonPrompts();
            Interior.Exit();
        }
    }
    public override void AddPrompt()
    {
        ButtonPromptIndetifierText = "ExitTeleport";
        if (Player == null)
        {
            return;
        }
        Player.ButtonPrompts.AddPrompt(ButtonPromptIndetifierText, ButtonPromptText, ButtonPromptIndetifierText, Settings.SettingsManager.KeySettings.InteractCancel, 999);
    }
}

