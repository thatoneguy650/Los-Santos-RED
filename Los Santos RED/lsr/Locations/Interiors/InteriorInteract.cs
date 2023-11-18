using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

[XmlInclude(typeof(StandardInteriorInteract))]
[XmlInclude(typeof(ExitInteriorInteract))]
public class InteriorInteract
{
    protected IInteractionable Player;
    protected ISettingsProvideable Settings;
    protected GameLocation InteractableLocation;
    protected Interior Interior;
    protected string ButtonPromptIndetifierText = "ButtonPromptText";
    public InteriorInteract()
    {

    }
    public InteriorInteract(Vector3 position, float heading, string buttonPromptText)
    {
        Position = position;
        Heading = heading;
        ButtonPromptText = buttonPromptText;
    }
    public Vector3 Position { get; set; }
    public float Heading { get; set; }
    public float InteractDistance { get; set; } = 3.0f;
    public Vector3 CameraPosition { get; set; } = Vector3.Zero;
    public Vector3 CameraDirection { get; set; } = Vector3.Zero;
    public Rotator CameraRotation { get; set; }
    public virtual string ButtonPromptText { get; set; } = "Interact";

    public virtual void Setup()
    {

    }

    public virtual void Update(IInteractionable player, ISettingsProvideable settings, GameLocation interactableLocation, Interior interior)
    {
        Player = player;
        Settings = settings;
        InteractableLocation = interactableLocation;
        Interior = interior;
        if (InteractableLocation == null)
        {
            return;
        }
        float distanceTo = Player.Character.DistanceTo(Position);
        //EntryPoint.WriteToConsole($"InteriorInteract UPDATE RAN {distanceTo}");
        if (!Interior.IsMenuInteracting && distanceTo <= InteractDistance)
        {
            AddPrompt();
        }
        else
        {
            RemovePrompt();
            return;
        }
        if (Player.ButtonPrompts.IsPressed(ButtonPromptIndetifierText))
        {
            OnInteract();
        }
    }
    public virtual void OnInteract()
    {

    }
    public virtual void RemovePrompt()
    {
        if (Player == null)
        {
            return;
        }
        Player.ButtonPrompts.RemovePrompts(ButtonPromptIndetifierText);
    }
    protected virtual void AddPrompt()
    {

    }
}

