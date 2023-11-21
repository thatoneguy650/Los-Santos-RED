using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
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
    protected ILocationInteractable LocationInteractable;
    protected ISettingsProvideable Settings;
    protected GameLocation InteractableLocation;
    protected Interior Interior;
    protected string ButtonPromptIndetifierText = "ButtonPromptText";
    protected bool canAddPrompt = false;
    protected float distanceTo;
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
    public float DistanceTo => distanceTo;
    public bool CanAddPrompt => canAddPrompt;

    public virtual void Setup()
    {

    }

    public virtual void Update(IInteractionable player, ISettingsProvideable settings, GameLocation interactableLocation, Interior interior, ILocationInteractable locationInteractable)
    {
        Player = player;
        Settings = settings;
        InteractableLocation = interactableLocation;
        Interior = interior;
        LocationInteractable = locationInteractable;
        if (InteractableLocation == null)
        {
            return;
        }
        distanceTo = Player.Character.DistanceTo(Position);
        //EntryPoint.WriteToConsole($"InteriorInteract UPDATE RAN {distanceTo}");
        if (!Interior.IsMenuInteracting && distanceTo <= InteractDistance)
        {
            canAddPrompt = true;
            //AddPrompt();
        }
        else
        {
            canAddPrompt = false;
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
    public virtual void AddPrompt()
    {

    }
    protected virtual void WaitForAnimation(string animDict, string animName)
    {
        string PlayingDict = animDict;
        string PlayingAnim = animName;
        AnimationWatcher aw = new AnimationWatcher();
        uint GameTimeStartedAnimation = Game.GameTime;
        while (EntryPoint.ModController.IsRunning && Player.IsAliveAndFree)
        {
            Player.WeaponEquipment.SetUnarmed();
            float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDict, PlayingAnim);
            if (AnimationTime >= 1.0f)
            {
                break;
            }
            if (!aw.IsAnimationRunning(AnimationTime))
            {
                break;
            }
            if(Game.GameTime - GameTimeStartedAnimation >= 1000 && !aw.HasStartedAnimation)
            {
                break;
            }
            GameFiber.Yield();
        }
    }
    protected virtual bool MoveToPosition()
    {
        NativeFunction.Natives.TASK_FOLLOW_NAV_MESH_TO_COORD(Player.Character, Position.X, Position.Y, Position.Z, 1.0f, -1, 0.1f, 0, Heading);
        uint GameTimeStartedMoving = Game.GameTime;
        bool IsCancelled = false;
        float prevDistanceToPos = 0f;
        bool isMoving = false;
        bool isInPosition = false;

        while (Game.GameTime - GameTimeStartedMoving <= 15000 && !IsCancelled)
        {
            if (Player.IsMoveControlPressed)
            {
                IsCancelled = true;
            }
            float distanceToPos = Game.LocalPlayer.Character.DistanceTo2D(Position);
            float headingDiff = Math.Abs(ExtensionsMethods.Extensions.GetHeadingDifference(Game.LocalPlayer.Character.Heading, Heading));
            if (distanceToPos != prevDistanceToPos)
            {
                isMoving = true;
                prevDistanceToPos = distanceToPos;
            }
            else
            {
                isMoving = false;
            }

            if (distanceToPos <= 0.2f && headingDiff <= 0.5f)
            {
                isInPosition = true;
                break;
            }
            if (!isMoving && distanceToPos <= 0.5f && headingDiff <= 0.5f)
            {
                isInPosition = true;
                break;
            }
            if (Game.GameTime - GameTimeStartedMoving >= 15000 && distanceToPos <= 1.0f && headingDiff <= 5f)
            {
                isInPosition = true;
                break;
            }
            GameFiber.Yield();
        }
        if(IsCancelled)
        {
            return false;
        }
        return isInPosition;
    }
}

