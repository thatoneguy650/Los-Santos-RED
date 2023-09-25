using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

public class PedCuff : DynamicActivity
{
    protected string FailMessage;
    protected uint GameTimeStartedConversing;
    protected bool IsActivelyConversing;
    protected bool IsTasked;
    protected bool IsBlockedEvents;
    protected PedExt Ped;
    protected IInteractionable Player;
    protected bool CancelledConversation;
    protected ISettingsProvideable Settings;
    protected ICrimes Crimes;
    protected dynamic pedHeadshotHandle;
    protected IModItems ModItems;
    protected bool IsCancelled;
    protected WeaponInformation LastWeapon;
    protected bool isAnimationPaused = false;
    protected int storedViewMode;
    protected bool IsBackingUp;
    protected bool IsMovingForward;
    protected RelationshipGroup PreviousRelationshipGroup;
    protected RelationshipGroup HostageRG;
    protected bool wasPersist = false;
    protected bool wasSetPersistent = false;
    protected Vector3 AttachOffset;
    protected string PlayerIdleDictionary;
    protected string PlayerIdleAnimation;
    protected int PlayerIdleFlags;
    protected string PedIdleDictionary;
    protected string PedIdleAnimation;
    protected IEntityProvideable World;


    protected virtual bool IsPedValid => Ped.Pedestrian.Exists() && Ped.Pedestrian.IsAlive && !Ped.IsUnconscious && !Ped.Pedestrian.IsRagdoll && !Ped.Pedestrian.IsGettingUp;
    protected virtual bool IsPlayerValid => Player.IsAliveAndFree && !Player.IsIncapacitated && Player.WeaponEquipment.CurrentWeapon == null;// && Player.WeaponEquipment.CurrentWeapon.CanPistolSuicide;
    public PedCuff(IInteractionable player, PedExt ped, ISettingsProvideable settings, ICrimes crimes, IModItems modItems, IEntityProvideable world)
    {
        Player = player;
        Ped = ped;
        Settings = settings;
        Crimes = crimes;
        ModItems = modItems;
        FailMessage = "Cannot Cuff Ped";
        World = world;
    }
    public override string DebugString => $"";
    public override ModItem ModItem { get; set; }
    public override bool CanPause { get; set; } = false;
    public override bool CanCancel { get; set; } = false;
    public override bool IsUpperBodyOnly { get; set; } = false;
    public override string PausePrompt { get; set; } = "Pause Activity";
    public override string CancelPrompt { get; set; } = "Stop Activity";
    public override string ContinuePrompt { get; set; } = "Continue Activity";
    public override void Start()
    {
        if (!Ped.Pedestrian.Exists())
        {
            return;
        }
        GameFiber.StartNew(delegate
        {
            try
            {
                Setup();
                if (!IsCancelled)
                {
                    StartGrab();
                }
                Cancel();
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "CuffPed");

    }
    public override void Continue()
    {

    }
    public override void Cancel()
    {
        if (IsCancelled)
        {
            return;
        }
        IsCancelled = true;
        ResetPed();
        ResetPlayer();
        ResetVariables();
        GameFiber.StartNew(delegate
        {
            try
            {
                uint gametimeStarted = Game.GameTime;
                while (EntryPoint.ModController.IsRunning)
                {
                    if (Game.GameTime - gametimeStarted >= 3000)
                    {
                        break;
                    }
                    GameFiber.Yield();
                }
                if (Ped.Pedestrian.Exists())
                {
                    Ped.Pedestrian.CollisionIgnoredEntity = null;
                }
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "GrabPed");

    }
    public override void Pause()
    {
        Cancel();
    }
    public override bool IsPaused() => false;
    public override bool CanPerform(IActionable player)
    {
        if (!player.ActivityManager.CanGrabLookedAtPed)
        {
            Game.DisplayHelp(FailMessage);
            return false;
        }
        if (player.IsOnFoot && player.ActivityManager.CanPerformActivitesBase)
        {
            return true;
        }
        Game.DisplayHelp(FailMessage);
        return false;
    }
    private void Setup()
    {
        IsCancelled = false;
        SetupVariables();
        SetupPlayer();
        SetupPed();
    }
    private void SetupVariables()
    {
        Player.ButtonPrompts.RemovePrompts("Grab"); //Player.ButtonPromptList.RemoveAll(x => x.Group == "Grab");
        HostageRG = new RelationshipGroup("HOSTAGE");
        Game.LocalPlayer.Character.RelationshipGroup.SetRelationshipWith(HostageRG, Relationship.Companion);
        HostageRG.SetRelationshipWith(Game.LocalPlayer.Character.RelationshipGroup, Relationship.Companion);
        if (!AnimationDictionary.RequestAnimationDictionayResult("move_action@generic@core") || !AnimationDictionary.RequestAnimationDictionayResult("move_strafe@generic"))
        {
            EntryPoint.WriteToConsole("ERROR GRAB SHIELD COULD NOT LOAD ANIMATION DICTIONARY 1");
        }
        SetupSpecific();
    }
    protected virtual void SetupSpecific()
    {
        AttachOffset = new Vector3(Settings.SettingsManager.ActivitySettings.GrabAttachX, Settings.SettingsManager.ActivitySettings.GrabAttachY, Settings.SettingsManager.ActivitySettings.GrabAttachZ);
        PlayerIdleDictionary = "doors@";
        PlayerIdleAnimation = "door_sweep_l_hand_medium";

        PedIdleDictionary = "mp_arresting";
        PedIdleAnimation = "idle";
        //PedIdleDictionary = "anim@gangops@hostage@";
        //PedIdleAnimation = "victim_idle";
        PlayerIdleFlags = 2 | 8 | 16 | 32;//maybe 8?
        //doors@
        //door_sweep_l_hand_medium
        if (!AnimationDictionary.RequestAnimationDictionayResult(PedIdleDictionary) || !AnimationDictionary.RequestAnimationDictionayResult(PlayerIdleDictionary))
        {
            EntryPoint.WriteToConsole("ERROR GRAB SHIELD COULD NOT LOAD ANIMATION DICTIONARY 1");
        }
    }
    protected virtual void SetupPlayer()
    {
        if (IsCancelled)
        {
            return;
        }
        Player.ActivityManager.IsPerformingActivity = true;
    }
    protected virtual void SetupPed()
    {
        if (IsCancelled)
        {
            return;
        }
        if (Ped.Pedestrian.Exists())
        {
            wasPersist = Ped.Pedestrian.IsPersistent;
            wasSetPersistent = Ped.WasEverSetPersistent;
            PreviousRelationshipGroup = Ped.Pedestrian.RelationshipGroup;
            Ped.CanBeTasked = false;
            NativeFunction.Natives.SET_PED_SHOULD_PLAY_IMMEDIATE_SCENARIO_EXIT(Ped.Pedestrian);
            Ped.Pedestrian.Tasks.Clear();
            Ped.Pedestrian.BlockPermanentEvents = true;
            Ped.Pedestrian.KeepTasks = true;
            Ped.Pedestrian.IsPersistent = true;
            NativeFunction.Natives.SET_ENABLE_HANDCUFFS(Ped.Pedestrian, true);
            Ped.Pedestrian.RelationshipGroup = HostageRG;
            NativeFunction.Natives.SET_PED_SHOULD_PLAY_IMMEDIATE_SCENARIO_EXIT(Ped.Pedestrian);
        }
        else
        {
            IsCancelled = true;
        }
    }
    private void StartGrab()
    {
        PlayInitialAnimation();
        SetupPrompts();
        while (IsPedValid && IsPlayerValid && !IsCancelled)
        {
            Ped.Pedestrian.CollisionIgnoredEntity = Game.LocalPlayer.Character;
            Ped.Pedestrian.Position = Player.Character.GetOffsetPosition(AttachOffset);
            Ped.Pedestrian.Heading = Game.LocalPlayer.Character.Heading;
            HeadingLoop();
            DirectionLoop();
            if (IsPromptPressed())
            {
                break;
            }
            AimingLoop();
            PlayerSettingsLoop();
            DisableControls();
            GameFiber.Yield();
        }
        if (Ped.Pedestrian.Exists())
        {
            Ped.Pedestrian.CollisionIgnoredEntity = Game.LocalPlayer.Character;
            Ped.Pedestrian.Position = Player.Character.GetOffsetPosition(AttachOffset);
            Ped.Pedestrian.Heading = Game.LocalPlayer.Character.Heading;
        }
    }
    private void PlayInitialAnimation()
    {
        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayerIdleDictionary, PlayerIdleAnimation, 8.0f, -8.0f, -1, PlayerIdleFlags, 0, false, false, false);//THIS HAD 8
        if (Ped.Pedestrian.Exists())
        {
            Ped.Pedestrian.CollisionIgnoredEntity = Game.LocalPlayer.Character;
            Ped.Pedestrian.Position = Player.Character.GetOffsetPosition(AttachOffset);
            Ped.Pedestrian.Heading = Game.LocalPlayer.Character.Heading;
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, PedIdleDictionary, PedIdleAnimation, 8.0f, -8.0f, -1, (int)(eAnimationFlags.AF_LOOPING | eAnimationFlags.AF_NOT_INTERRUPTABLE | eAnimationFlags.AF_UPPERBODY | eAnimationFlags.AF_SECONDARY | eAnimationFlags.AF_FORCE_START), 0, false, false, false);//-1
        }
    }
    private void AimingLoop()
    {
        if (Player.Character.IsAiming)
        {
            if (!isAnimationPaused)
            {
                NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
                isAnimationPaused = true;
                Player.ButtonPrompts.RemovePrompts("Hostage");
            }
        }
        else
        {
            if (isAnimationPaused)
            {
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayerIdleDictionary, PlayerIdleAnimation, 8.0f, -8.0f, -1, PlayerIdleFlags, 0, false, false, false);
                IsBackingUp = false;
                IsMovingForward = false;
                isAnimationPaused = false;
                SetupPrompts();
            }
        }
    }
    private void PlayerSettingsLoop()
    {
        NativeFunction.Natives.SET_PED_MOVE_RATE_OVERRIDE<uint>(Player.Character, 0.75f);
        Player.Stance.SetActionMode(false);
        Player.Stance.SetStealthMode(false);
        Game.DisableControlAction(0, GameControl.Sprint, true);
        Game.DisableControlAction(0, GameControl.Jump, true);
        Player.ButtonPrompts.RemovePrompts("Grab");
    }
    private void DirectionLoop()
    {
        bool IsMoveUpPressed = false;
        bool IsMoveDownPressed = false;
        if (Game.IsControlPressed(2, GameControl.MoveDownOnly) || Game.IsControlPressed(2, GameControl.MoveDown))
        {
            IsMoveDownPressed = true;
        }
        if (Game.IsControlPressed(2, GameControl.MoveUpOnly) || Game.IsControlPressed(2, GameControl.MoveUp))
        {
            IsMoveUpPressed = true;
        }
        if (IsMoveDownPressed)
        {
            if (!IsBackingUp)
            {
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "move_strafe@generic", "walk_bwd_180_loop", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                GameFiber.Wait(50);
                if (Ped.Pedestrian.Exists())
                {
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "move_strafe@generic", "walk_bwd_180_loop", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, PedIdleDictionary, PedIdleAnimation, 8.0f, -8.0f, -1, 1 | 16 | 32, 0, false, false, false);
                }
                IsBackingUp = true;
            }
        }
        else if (IsMoveUpPressed || Player.Character.IsWalking)
        {
            if (!IsMovingForward)
            {
                if (Ped.Pedestrian.Exists())
                {
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "move_action@generic@core", "walk", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                }
                IsMovingForward = true;
            }
        }
        else
        {
            if (IsBackingUp)
            {
                float AnimTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayerIdleDictionary, PlayerIdleAnimation);
                NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayerIdleDictionary, PlayerIdleAnimation, 8.0f, -8.0f, -1, PlayerIdleFlags, 0, false, false, false);
                NativeFunction.Natives.SET_ENTITY_ANIM_CURRENT_TIME(Player.Character, PlayerIdleDictionary, PlayerIdleAnimation, AnimTime);
                if (Ped.Pedestrian.Exists())
                {
                    NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, PedIdleDictionary, PedIdleAnimation, 8.0f, -8.0f, -1, 1 | 16 | 32, 0, false, false, false);
                    NativeFunction.Natives.SET_ENTITY_ANIM_CURRENT_TIME(Ped.Pedestrian, PedIdleDictionary, PedIdleAnimation, AnimTime);

                }
                IsBackingUp = false;
            }
            if (IsMovingForward)
            {
                if (Ped.Pedestrian.Exists())
                {
                    NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, PedIdleDictionary, PedIdleAnimation, 8.0f, -8.0f, -1, 1 | 16 | 32, 0, false, false, false);

                }
                IsMovingForward = false;
            }
        }
    }
    private void HeadingLoop()
    {
        bool isLeftPressed = false;
        bool isRightPressed = false;

        if (Game.IsControlPressed(2, GameControl.MoveRightOnly) || Game.IsControlPressed(2, GameControl.MoveRight))
        {
            isRightPressed = true;
        }
        if (Game.IsControlPressed(2, GameControl.MoveLeftOnly) || Game.IsControlPressed(2, GameControl.MoveLeft))
        {
            isLeftPressed = true;
        }
        if (isRightPressed)
        {
            Player.Character.Heading -= 0.7f;
        }
        else if (isLeftPressed)
        {
            Player.Character.Heading += 0.7f;
        }
    }
    protected virtual void ResetPed()
    {
        if (Ped.Pedestrian.Exists())
        {
            Ped.Pedestrian.BlockPermanentEvents = false;
            Ped.Pedestrian.KeepTasks = false;
            Ped.Pedestrian.IsPersistent = wasPersist;
            NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
            Ped.Pedestrian.RelationshipGroup = PreviousRelationshipGroup;
            Ped.WasEverSetPersistent = wasSetPersistent;
            if (!Ped.IsBusted)
            {
                Ped.CanBeAmbientTasked = true;
                Ped.CanBeTasked = true;
            }
            NativeFunction.Natives.SET_ENABLE_HANDCUFFS(Ped.Pedestrian, false);
        }
    }
    protected virtual void ResetPlayer()
    {
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        Player.ActivityManager.IsPerformingActivity = false;
        Game.LocalPlayer.Character.RelationshipGroup.SetRelationshipWith(HostageRG, Relationship.Neutral);
        HostageRG.SetRelationshipWith(Game.LocalPlayer.Character.RelationshipGroup, Relationship.Neutral);

        Game.DisableControlAction(0, GameControl.Sprint, false);
        Game.DisableControlAction(0, GameControl.Jump, false);
    }
    private void ResetVariables()
    {
        Player.ButtonPrompts.RemovePrompts("Hostage");
    }
    private void DisableControls()
    {
        if (!Player.Character.IsAiming)
        {
            Game.DisableControlAction(0, GameControl.Attack, true);
            Game.DisableControlAction(0, GameControl.Attack2, true);
            Game.DisableControlAction(0, GameControl.VehicleAttack, true);
            Game.DisableControlAction(0, GameControl.VehicleAttack2, true);
        }
    }
    protected virtual void SetupPrompts()
    {
        Player.ButtonPrompts.RemovePrompts("Grab");

        if (!Player.ButtonPrompts.HasPrompt("Load"))
        {
            Player.ButtonPrompts.AddPrompt("Hostage", "Load", "Load", GameControl.Attack, 1);
        }


        if (!Player.ButtonPrompts.HasPrompt("Release"))
        {
            Player.ButtonPrompts.AddPrompt("Hostage", "Release", "Release", Settings.SettingsManager.KeySettings.InteractNegativeOrNo, 2);
        }
    }
    protected virtual bool IsPromptPressed()
    {
        if (Player.IsShowingActionWheel)
        {
            return false;
        }
        if (Player.ButtonPrompts.IsPressed("Release"))
        {
            Player.ButtonPrompts.RemovePrompts("Hostage");
            Cancel();
            return true;
        }
        if (Player.ButtonPrompts.IsPressed("Load"))
        {
            Player.ButtonPrompts.RemovePrompts("Hostage");
            return LoadPed();
        }
        return false;
    }
    private bool LoadPed()
    {
        if (!Ped.Pedestrian.Exists())
        {
            Game.DisplayHelp("Cannot Load Ped");
            return false;
        }
        SeatAssigner sa = new SeatAssigner(Ped, World, World.Vehicles.SimplePoliceVehicles);
        sa.AssignPrisonerSeat();
        if (sa.VehicleAssigned == null || !sa.VehicleAssigned.Vehicle.Exists() || sa.VehicleAssigned.Vehicle.DistanceTo2D(Ped.Pedestrian) >= 10f)
        {
            return false;
        }
        Cancel();
        Ped.Pedestrian.BlockPermanentEvents = false;
        Ped.Pedestrian.KeepTasks = false;
        NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", Ped.Pedestrian, sa.VehicleAssigned.Vehicle, -1, sa.SeatAssigned, 1f, 9);
        return true;
    }
}