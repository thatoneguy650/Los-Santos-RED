using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosSantosRED.lsr.Interface;

public class ResidenceInteriorRestInteract : InteriorInteract
{
    protected Residence Residence;

    public ResidenceInteriorRestInteract()
    {
    }

    public ResidenceInteriorRestInteract(Vector3 position, float heading, string buttonPromptText) : base(position, heading, buttonPromptText)
    {

    }

    public override void OnInteract()
    {
        Interior?.RemoveButtonPrompts();
        RemovePrompt();
        Interior.IsMenuInteracting = true;
        NativeFunction.Natives.TASK_FOLLOW_NAV_MESH_TO_COORD(Player.Character, Position.X, Position.Y, Position.Z, 1.0f, -1, 0.1f, 0, Heading);

        //Need to residence here to either limit the choices, and dont know it until RUNTIME!
        if (CameraPosition != Vector3.Zero)
        {
            InteractableLocation?.InteractWithNewCamera(CameraPosition, CameraDirection, CameraRotation);
        }
        else
        {
            InteractableLocation?.StandardInteract(null, true);
        }

    }
    protected override void AddPrompt()
    {
        ButtonPromptIndetifierText = "RestInteriorLocation";
        if (Player == null)
        {
            return;
        }
        Player.ButtonPrompts.AddPrompt(ButtonPromptIndetifierText, ButtonPromptText, ButtonPromptIndetifierText, Settings.SettingsManager.KeySettings.InteractStart, 999);
    }
}

