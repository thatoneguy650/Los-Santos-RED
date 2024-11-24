using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

[XmlInclude(typeof(StandardInteriorInteract))]
[XmlInclude(typeof(ExitInteriorInteract))]
[XmlInclude(typeof(AnimationInteract))]
[XmlInclude(typeof(ToiletInteract))]
[XmlInclude(typeof(SinkInteract))]


[XmlInclude(typeof(TheftInteract))]
[XmlInclude(typeof(MoneyTheftInteract))]
[XmlInclude(typeof(ItemTheftInteract))]
[XmlInclude(typeof(SalonInteract))]
//UrinalInteract
//ToiletInteract
public class InteriorInteract
{
    protected IInteractionable Player;
    protected ILocationInteractable LocationInteractable;
    protected ISettingsProvideable Settings;
    protected IModItems ModItems;
    protected GameLocation InteractableLocation;
    protected Interior Interior;
    protected LocationCamera LocationCamera;
    protected IClothesNames ClothesNames;
    protected bool canAddPrompt = false;
    protected float distanceTo;
    protected float GroundZ;
    protected bool HasGroundZ = false;
   
    public InteriorInteract()
    {

    }
    public InteriorInteract(string name, Vector3 position, float heading, string buttonPromptText)
    {
        Name = name;
        Position = position;
        Heading = heading;
        ButtonPromptText = buttonPromptText;
    }
    public string Name { get; set; }
    public Vector3 Position { get; set; }
    public float Heading { get; set; }
    public float InteractDistance { get; set; } = 1f;
    public Vector3 CameraPosition { get; set; } = Vector3.Zero;
    public Vector3 CameraDirection { get; set; } = Vector3.Zero;
    public Rotator CameraRotation { get; set; }
    public virtual string ButtonPromptText { get; set; } = "Interact";
    public bool AutoCamera { get; set; } = true;
    public float DistanceTo => distanceTo;
    public bool CanAddPrompt => canAddPrompt;
    public bool HasCustomCamera => CameraPosition != Vector3.Zero;

    public bool UseNavmesh { get; set; } = true;
    public bool WithWarp { get; set; } = false;
    public bool ForceIsntantCamera { get; set; } = false;


    public bool IsAutoInteract { get; set; } = false;
    public virtual bool ShouldAddPrompt => !Interior.IsMenuInteracting && distanceTo <= InteractDistance && !Player.ActivityManager.IsInteracting && Player.ActivityManager.CanPerformActivitiesOnFoot;
    public virtual void Setup(IModItems modItems, IClothesNames clothesNames)
    {
        ModItems = modItems;
        ClothesNames = clothesNames;
    }
    public virtual void OnInteriorLoaded()
    {

    }
    public void DisplayMarker(int markerType, float zOffset, float markerScale)
    {
        if(DistanceTo >= 30)
        {
            return;
        }
        if (!HasGroundZ)
        {
            float entranceZPosition = Position.Z;
            NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD(Position.X, Position.Y, Position.Z, out entranceZPosition, false);
            GroundZ = entranceZPosition;
            HasGroundZ = true;
        }
        NativeFunction.Natives.DRAW_MARKER(markerType, 
            Position.X, Position.Y, GroundZ + zOffset, 
            0f, 0f, 0f, 
            0f, 0f, 0f,
            markerScale, markerScale, markerScale,
            EntryPoint.LSRedColor.R, EntryPoint.LSRedColor.G, EntryPoint.LSRedColor.B, EntryPoint.LSRedColor.A,
            false, false, 2, true, 0, 0, false);//false, true, 2, true, 0, 0, false);
    }
    public virtual void UpdateDistances(IInteractionable player)
    {
        Player = player;
        distanceTo = Player.Character.DistanceTo(Position);
        canAddPrompt = distanceTo <= InteractDistance;     
    }
    public virtual void UpdateActivated(IInteractionable player, ISettingsProvideable settings, GameLocation interactableLocation, Interior interior, ILocationInteractable locationInteractable)
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
        if (Player.ButtonPrompts.IsPressed(Name))
        {
            OnInteract();
        }
    }
    public virtual void SetupFake(IInteractionable player, ISettingsProvideable settings, GameLocation interactableLocation, ILocationInteractable locationInteractable)
    {
        Player = player;
        Settings = settings;
        InteractableLocation = interactableLocation;
        LocationInteractable = locationInteractable;
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
        if (ShouldAddPrompt)
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
        if (Player.ButtonPrompts.IsPressed(Name))
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
        Player.ButtonPrompts.RemovePrompt(Name);
    }
    public virtual void AddPrompt()
    {

    }
    protected virtual void SetupCamera(bool wait)
    {
        if (LocationCamera == null)
        {
            LocationCamera = new LocationCamera(InteractableLocation, LocationInteractable, Settings, true);
        }
        if (CameraPosition != Vector3.Zero)
        {
            LocationCamera.MoveToPosition(CameraPosition, CameraDirection, CameraRotation, wait, true, ForceIsntantCamera);
        }
        else if (CameraPosition == Vector3.Zero && AutoCamera)
        {
            LocationCamera.AutoInterior(Position, Heading, wait, true);
        }        
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
    protected virtual bool UseScenario()
    {
        if (WithWarp)
        {
            NativeFunction.Natives.TASK_USE_NEAREST_SCENARIO_TO_COORD_WARP(Player.Character, Position.X, Position.Y, Position.Z, 3f, 0);
        }
        else
        {
            NativeFunction.Natives.TASK_USE_NEAREST_SCENARIO_TO_COORD(Player.Character, Position.X, Position.Y, Position.Z, 3f, 0);
        }
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

            if(NativeFunction.Natives.IS_PED_USING_ANY_SCENARIO<bool>(Player.Character))
            {
                isInPosition = true;
                break;
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
        if (IsCancelled)
        {
            return false;
        }

        if(!isInPosition)
        {
            NativeFunction.Natives.TASK_USE_NEAREST_SCENARIO_TO_COORD_WARP(Player.Character, Position.X, Position.Y, Position.Z, 3f, 0);
            isInPosition = true;
        }

        return isInPosition;
    }
    protected virtual bool MoveToPosition() => MoveToPosition(1.0f);
    protected virtual bool MoveToPosition(float speed)
    {
        if (UseNavmesh)
        {
            NativeFunction.Natives.TASK_FOLLOW_NAV_MESH_TO_COORD(Player.Character, Position.X, Position.Y, Position.Z, speed, -1, 0.1f, 0, Heading);
        }
        else
        {
            NativeFunction.Natives.TASK_GO_STRAIGHT_TO_COORD(Player.Character, Position.X, Position.Y, Position.Z, speed, 5000, Heading, 0.5f);
        }
        uint GameTimeStartedMoving = Game.GameTime;
        bool IsCancelled = false;
        float prevDistanceToPos = 0f;
        bool isMoving = false;
        bool isInPosition = false;
        uint GameTimeLastPrint = 0;
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
            if (!isMoving && distanceToPos <= 0.7f && headingDiff <= 2.5f)
            {
                isInPosition = true;
                break;
            }
            if (Game.GameTime - GameTimeStartedMoving >= 15000 && distanceToPos <= 1.0f && headingDiff <= 5f)
            {
                isInPosition = true;
                break;
            }

            //if (Game.GameTime - GameTimeLastPrint >= 500)
            //{
            //    Game.DisplaySubtitle($"{distanceToPos} {headingDiff}");
            //    GameTimeLastPrint = Game.GameTime;
            //}
            GameFiber.Yield();
        }
        if(IsCancelled)
        {
            return false;
        }
        return isInPosition;
    }

    public virtual void AddDistanceOffset(Vector3 offsetToAdd)
    {
        Position += offsetToAdd;
        CameraPosition += offsetToAdd;
    }
}

