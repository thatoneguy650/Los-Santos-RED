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

    public ExitInteriorInteract(string name, Vector3 position, float heading, string buttonPromptText) : base(name, position, heading, buttonPromptText)
    {

    }
    public override void OnInteract()
    {
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
        if (Player == null)
        {
            return;
        }
        Player.ButtonPrompts.AttemptAddPrompt(Name, ButtonPromptText, Name, Settings.SettingsManager.KeySettings.InteractCancel, 999);
    }
}

