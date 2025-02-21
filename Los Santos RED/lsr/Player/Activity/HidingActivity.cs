using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using Mod;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class HidingActivity : DynamicActivity
{
    private bool IsCancelled;
    private IActionable Player;
    private ILocationInteractable LocationInteractable;
    private ISettingsProvideable Settings;
    private Rage.Object HidingObject;
    private Vector3 FinalPlayerPos;
    private float FinalPlayerHeading;
    private string PlayingDict;
    private string PlayingAnim;
    private HideableObject HideableObject;

    public HidingActivity(IActionable player,ILocationInteractable locationInteractable, ISettingsProvideable settings, Rage.Object hidingObject, HideableObject hideableObject)
    {
        Player = player;
        LocationInteractable = locationInteractable;
        Settings = settings;
        HidingObject = hidingObject;
        HideableObject = hideableObject;
    }
    public override ModItem ModItem { get; set; }
    public override string DebugString => "";
    public override bool CanPause { get; set; } = false;
    public override bool CanCancel { get; set; } = true;
    public override bool IsUpperBodyOnly { get; set; } = false;
    public override string PausePrompt { get; set; } = "Pause Hiding";
    public override string CancelPrompt { get; set; } = "Stop Hiding";
    public override string ContinuePrompt { get; set; } = "Continue Hiding";
    public override void Cancel()
    {
        IsCancelled = true;
    }
    public override void Pause()
    {

    }
    public override bool IsPaused() => false;
    public override void Continue()
    {

    }
    public override void Start()
    {
        Setup();
        GameFiber ScenarioWatcher = GameFiber.StartNew(delegate
        {
            try
            {
                Enter();
                Exit();
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "HidingWatcher");
    }
    public override bool CanPerform(IActionable player)
    {
        if (Player.IsWanted && Player.AnyPoliceCanSeePlayer)
        {
            EntryPoint.WriteToConsole("CANNOT HIDE RAN");
            Game.DisplayHelp($"Cannot Hide When Wanted and Seen");
            return false;
        }
        else if (player.IsOnFoot && player.ActivityManager.CanPerformActivitesBase && !player.ActivityManager.IsResting)
        {
            return true;
        }
        EntryPoint.WriteToConsole("OTHER HIDE RAN");
        Game.DisplayHelp($"Cannot Hide");
        return false;
    }
    private void Enter()
    {
        if(!HidingObject.Exists())
        {
            return;
        }
        Player.ActivityManager.IsPerformingActivity = true;
        MachineOffsetResult machineInteraction = new MachineOffsetResult(LocationInteractable, HidingObject);
        machineInteraction.StandingOffsetPosition = 0.5f;
        machineInteraction.GetPropEntry();
        FinalPlayerPos = machineInteraction.PropEntryPosition;
        FinalPlayerHeading = machineInteraction.PropEntryHeading;
        MoveInteraction moveInteraction = new MoveInteraction(LocationInteractable, FinalPlayerPos, FinalPlayerHeading);
        if (!moveInteraction.MoveToMachine(4.0f))
        {
            return;
        }
        NativeFunction.Natives.DISABLE_CAM_COLLISION_FOR_OBJECT(HidingObject);
        StartClimb();
        uint GameTimeStarted = Game.GameTime;
        while (Player.ActivityManager.CanPerformActivitesBase && !IsCancelled && Game.GameTime - GameTimeStarted <= 800)
        {
            DisableControls();
            GameFiber.Yield();
        }
        if(!HidingObject.Exists())
        {
            return;
        }
        HidingObject.IsCollisionEnabled = false;
        Player.Character.Position = HidingObject.Position;
        Player.Character.Heading = FinalPlayerHeading - 180f;
        while (Player.ActivityManager.CanPerformActivitesBase && !IsCancelled && Game.GameTime - GameTimeStarted <= 800)
        {
            DisableControls();
            GameFiber.Yield();
        }
        if (!HidingObject.Exists())
        {
            return;
        }
        HidingObject.IsCollisionEnabled = true;
         Idle();        
    }
    private void Idle()
    {
        //StartCower();
        //GameFiber.Sleep(800);

        //if(Player.IsWanted && Player.AnyPoliceCanSeePlayer)
        //{
        //    Game.DisplayHelp("You were seen entering");
        //    EntryPoint.WriteToConsole("You were seen entering");

        //    return;
        //}
        CanCancel = true;

        Player.Character.IsVisible = false;

        Player.ActivityManager.IsHidingInObject = true;

        while (Player.ActivityManager.CanPerformActivitesBase && !IsCancelled)
        {
            DisableControls();
            //if(!Player.ButtonPrompts.HasPrompt("ExitHiding"))
            //{
            //    EntryPoint.WriteToConsole("ATTEMPTING TO ADD PROMPT");
            //    Player.ButtonPrompts.AttemptAddPrompt("HidingExit", "Exit Hiding", "ExitHiding", GameControl.Attack, 999);
            //}
            //if (Player.ButtonPrompts.IsPressed("ExitHiding") || Player.IsMoveControlPressed)
            //{
            //    Player.ButtonPrompts.RemovePrompts("HidingExit");
            //    break;
            //}
            if(Player.IsWanted && Player.IsInWantedActiveMode)// Player.AnyPoliceRecentlySeenPlayer)
            {
                Game.DisplayHelp("You have been found");
                EntryPoint.WriteToConsole("STOPPED HIDING Wanted and IsInWantedActiveMode");
                break;
            }
            GameFiber.Yield();
        }
    }
    private void Exit()
    {
        Player.ButtonPrompts.RemovePrompts("HidingExit");
        Player.Character.IsVisible = true;
        Player.ActivityManager.IsHidingInObject = false;
        StartClimb();
        uint GameTimeStarted = Game.GameTime;
        AnimationWatcher aw = new AnimationWatcher();
        while (!IsCancelled && Game.GameTime - GameTimeStarted <= 2400)
        {
            float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDict, PlayingAnim);
            if (!aw.IsAnimationRunning(AnimationTime))
            {
                break;
            }
            DisableControls();
            GameFiber.Yield();
        }
        if (!HidingObject.Exists())
        {
            return;
        }
        //Player.Character.Position = FinalPlayerPos;
        Player.ButtonPrompts.RemovePrompts("HidingExit");
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        Player.ActivityManager.IsPerformingActivity = false;
    }
    private void StartClimb()
    {
        PlayingDict = "move_climb";
        PlayingAnim = "standclimbup_80";
        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 1.0f, -1.0f, -1, 512, 0, false, false, false);
    }
    private void DisableControls()
    {
        Game.DisableControlAction(0, GameControl.Attack, true);// false);
        Game.DisableControlAction(0, GameControl.Attack2, true);// false);

        Game.DisableControlAction(0, GameControl.VehicleAttack, true);// false);
        Game.DisableControlAction(0, GameControl.VehicleAttack2, true);// false);





        NativeHelper.DisablePlayerMovementControl();
    }
    private void Setup()
    {
        CanCancel = false;
        AnimationDictionary.RequestAnimationDictionay("move_climb");
        Player.ButtonPrompts.RemovePrompts("Hiding");

        if(HideableObject == null)
        {
            CancelPrompt = $"Stop Hiding";
        }
        else
        {
            CancelPrompt = $"Exit {HideableObject.Name}";
        }
    }
}

