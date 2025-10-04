using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using Mod;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;


public class ForceDoorActivity : DynamicActivity
{
    private bool IsCancelled;
    private IActionable Player;
    private ILocationInteractable LocationInteractable;
    private ISettingsProvideable Settings;
    private Rage.Object DoorObject;
    private Vector3 FinalPlayerPos;
    private float FinalPlayerHeading;
    private string PlayingDict;
    private string PlayingAnim;
    private InteriorDoor InteriorDoor;
    private uint GameTimeStartedForcingDoor;
    private MenuPool MenuPool;
    private UIMenu ForceOpenMenu;
    private IBasicUseable BasicUseable;
    private Interior Interior;
    private float alarmPercentDrill => Settings.SettingsManager.ActivitySettings.AlarmPercentageDrill;// 30f;
    private float alarmPercentLockpick => Settings.SettingsManager.ActivitySettings.AlarmPercentageLockpick;//10f;
    private float alarmPercentBash => Settings.SettingsManager.ActivitySettings.AlarmPercentageBash;//80f;
    public ForceDoorActivity(IActionable player, ILocationInteractable locationInteractable, ISettingsProvideable settings, Rage.Object doorObject, InteriorDoor interiorDoor,
        IBasicUseable basicUseable, Interior interior)
    {
        Player = player;
        LocationInteractable = locationInteractable;
        Settings = settings;
        DoorObject = doorObject;
        InteriorDoor = interiorDoor;
        BasicUseable = basicUseable;
        Interior = interior;
    }
    public override ModItem ModItem { get; set; }
    public override string DebugString => "";
    public override bool CanPause { get; set; } = false;
    public override bool CanCancel { get; set; } = true;
    public override bool IsUpperBodyOnly { get; set; } = false;
    public override string PausePrompt { get; set; } = "Pause Forcing";
    public override string CancelPrompt { get; set; } = "Stop Forcing";
    public override string ContinuePrompt { get; set; } = "Continue Forcing";
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
        if (player.IsOnFoot && player.ActivityManager.CanPerformActivitesBase && !player.ActivityManager.IsResting)
        {
            return true;
        }
        Game.DisplayHelp($"Cannot Force Door");
        return false;
    }
    private void Enter()
    {
        //if (!DoorObject.Exists())
        //{
        //    return;
        //}
        Player.ActivityManager.IsPerformingActivity = true;
        if(InteriorDoor != null)
        {
            InteriorDoor.GetObject();
            if(InteriorDoor.DoorObject != null)
            {
                DoorObject = InteriorDoor.DoorObject;
            }
        }

        if(InteriorDoor != null && InteriorDoor.HasCustomInteractPosition)
        {
            FinalPlayerPos = InteriorDoor.InteractPostion;//DOORS ARE TOO FUCKY FOR THIS SHIT NativeHelper.GetOffsetPosition(machineInteraction.PropEntryPosition, machineInteraction.PropEntryHeading + Settings.SettingsManager.DebugSettings.DoorEntryAngle, Settings.SettingsManager.DebugSettings.DoorEntryValue);
            FinalPlayerHeading = InteriorDoor.InteractHeader;
            MoveInteraction moveInteraction = new MoveInteraction(LocationInteractable, FinalPlayerPos, FinalPlayerHeading);
            if (!moveInteraction.MoveToMachine(4.0f))
            {
                return;
            }
        }
        else if (DoorObject.Exists())
        {


            MachineOffsetResult machineInteraction = new MachineOffsetResult(LocationInteractable, DoorObject);
            machineInteraction.StandingOffsetPosition = 0.5f;
            machineInteraction.GetPropEntry();




            FinalPlayerPos = machineInteraction.PropEntryPosition;//DOORS ARE TOO FUCKY FOR THIS SHIT NativeHelper.GetOffsetPosition(machineInteraction.PropEntryPosition, machineInteraction.PropEntryHeading + Settings.SettingsManager.DebugSettings.DoorEntryAngle, Settings.SettingsManager.DebugSettings.DoorEntryValue);
            FinalPlayerHeading = machineInteraction.PropEntryHeading;
            MoveInteraction moveInteraction = new MoveInteraction(LocationInteractable, FinalPlayerPos, FinalPlayerHeading);
            if (!moveInteraction.MoveToMachine(4.0f))
            {
                return;
            }
        }
        else
        {
            MoveInteraction moveInteraction = new MoveInteraction(LocationInteractable, InteriorDoor.Position, 0f);
            moveInteraction.CloseDistance = 1.0f;
            if (!moveInteraction.MoveToMachine(4.0f))
            {
                return;
            }
        }
        ShowMenu();
        //Idle();
    }

    private void ShowMenu()
    {
        MenuPool = new MenuPool();
        ForceOpenMenu = new UIMenu("Force Open", "Select an Option");
        ForceOpenMenu.RemoveBanner();
        MenuPool.Add(ForceOpenMenu);


        UIMenuItem pickLockMenuItem = new UIMenuItem("Pick Lock", "Select to attempt to pick the lock. Quiet, but can be slow. Requires a screwdriver.");
        pickLockMenuItem.Activated += (menu, item) =>
        {
            menu.Visible = false;
            PickLock();
        };
        ForceOpenMenu.AddItem(pickLockMenuItem);
        pickLockMenuItem.Enabled = Player.Inventory.Has(typeof(ScrewdriverItem));



        UIMenuItem drillLockMenuItem = new UIMenuItem("Drill Lock", "Select to attempt to drill the lock. Loud and fast. Requires a drill.");
        drillLockMenuItem.Activated += (menu, item) =>
        {
            menu.Visible = false;
            DrillLock();
        };
        ForceOpenMenu.AddItem(drillLockMenuItem);
        drillLockMenuItem.Enabled = Player.Inventory.Has(typeof(DrillItem));
        UIMenuItem bashDoorMenuItem = new UIMenuItem("Bash Door", "Run Into the door like a brute.");
        bashDoorMenuItem.Activated += (menu, item) =>
        {
            menu.Visible = false;
            BashDoor();
        };
        ForceOpenMenu.AddItem(bashDoorMenuItem);
        ForceOpenMenu.Visible = true;
        while (MenuPool.IsAnyMenuOpen())
        {
            MenuPool.ProcessMenus();
            Player.IsSetDisabledControls = true;
            GameFiber.Yield();
        }
        Player.IsSetDisabledControls = false;
    }

    private void DrillLock()
    {
        DrillItem drillItem = Player.ActivityManager.CurrentDrill;// 
        if (drillItem == null)
        {
            List<ModItem> drillItems = Player.Inventory.GetAll(typeof(DrillItem)).Select(x => x.ModItem).ToList();
            uint lowest = 999999999;
            foreach (ModItem item in drillItems)
            {
                DrillItem mydrill = (DrillItem)item;
                if (mydrill.MinDoorDrillTime <= lowest)
                {
                    lowest = mydrill.MinDoorDrillTime;
                    drillItem = mydrill;
                }
            }
        }
        if (drillItem == null)
        {
            return;
        }
        Player.Violations.SetContinuouslyViolating(StaticStrings.BreakingEnteringAudibleCrimeID);
        drillItem.PerformDrillingAnimation(LocationInteractable, InteriorDoor.UnLockDoor, false, Interior);
        Player.Violations.StopContinuouslyViolating(StaticStrings.BreakingEnteringAudibleCrimeID);
    }
    private void BashDoor()
    {
        PlayingDict = "melee@unarmed@streamed_core";
        PlayingAnim = "running_shove";// "kick_close_a";
        AnimationDictionary.RequestAnimationDictionay(PlayingDict);
        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 8.0f, -8.0f, -1, 1, 0, false, false, false);
        GameTimeStartedForcingDoor = Game.GameTime;
        Player.Violations.SetContinuouslyViolating(StaticStrings.BreakingEnteringAudibleCrimeID);
        uint GameTimeBetweenBashCheckes = 5000;
        uint GameTimeLastCheckedBashFail = Game.GameTime;
        while (Player.ActivityManager.CanPerformActivitesBase && !IsCancelled)
        {
            Player.WeaponEquipment.SetUnarmed();
            if (Player.IsMoveControlPressed || !Player.Character.IsAlive)
            {
                break;
            }
            if (RandomItems.RandomLargePercent(Settings.SettingsManager.ActivitySettings.BashDoorUnlockPercentage))
            {
                InteriorDoor.UnLockDoor();
                Game.DisplayHelp("Door Opened");
                break;
            }
            if(Game.GameTime - GameTimeLastCheckedBashFail >= GameTimeBetweenBashCheckes )
            {
                GameTimeLastCheckedBashFail = Game.GameTime;
                if(RandomItems.RandomPercent(alarmPercentBash) && Interior != null)
                {
                    Interior.SetOffAlarm();
                    Player.HasSetOffAlarm(Interior.GameLocation);
                }
            }
            DisableControls();
            GameFiber.Yield();
        }
        Player.Violations.StopContinuouslyViolating(StaticStrings.BreakingEnteringAudibleCrimeID);
    }

    private void PickLock()
    {
        ScrewdriverItem screwdriverItem = (ScrewdriverItem)Player.Inventory.Get(typeof(ScrewdriverItem))?.ModItem;
        if (screwdriverItem == null)
        {
            return;
        }
        Player.Violations.SetContinuouslyViolating(StaticStrings.BreakingEnteringCrimeID);
       //screwdriverItem.PickDoorLock(LocationInteractable, BasicUseable, InteriorDoor.UnLockDoor);
        screwdriverItem.DoLockpickAnimation(LocationInteractable, BasicUseable, InteriorDoor.UnLockDoor, Settings, true, false, Interior);
        Player.Violations.StopContinuouslyViolating(StaticStrings.BreakingEnteringCrimeID);
    }
    private void Exit()
    {
        EntryPoint.WriteToConsole("Force Door EXIT RAN");
        Player.ButtonPrompts.RemovePrompts("DoorInteract");
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        Player.ActivityManager.IsPerformingActivity = false;
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

        Player.ButtonPrompts.RemovePrompts("DoorInteract");
        CancelPrompt = $"Stop Forcing Door";
    }


    //private void PickLock()
    //{
    //CanCancel = true;
    //PlayingDict = "missheistfbisetup1";
    //PlayingAnim = "hassle_intro_loop_f";
    //AnimationDictionary.RequestAnimationDictionay(PlayingDict);
    //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 2.0f, -2.0f, -1, 1, 0, false, false, false);
    //GameTimeStartedForcingDoor = Game.GameTime;
    //Rage.Object ScrewDriverObject = null;
    //uint TimeToPick = RandomItems.GetRandomNumber(7000, 12000);
    //ScrewdriverItem screwdriverItem = (ScrewdriverItem)Player.Inventory.Get(typeof(ScrewdriverItem))?.ModItem;
    //if(screwdriverItem != null)
    //{
    //    ScrewDriverObject = screwdriverItem.SpawnAndAttachItem(BasicUseable, true, true);
    //    TimeToPick = RandomItems.GetRandomNumber(screwdriverItem.MinDoorPickTime, screwdriverItem.MaxDoorPickTime);
    //}
    //Player.Violations.SetContinuouslyViolating(StaticStrings.BreakingEnteringCrimeID);
    //while (Player.ActivityManager.CanPerformActivitesBase && !IsCancelled)
    //{

    //    if (Game.GameTime - GameTimeStartedForcingDoor >= TimeToPick)
    //    {
    //        InteriorDoor.UnLockDoor();
    //        Game.DisplayHelp("Door Opened");
    //        break;
    //    }

    //    DisableControls();
    //    GameFiber.Yield();
    //}
    //if(screwdriverItem != null && ScrewDriverObject.Exists())
    //{
    //    ScrewDriverObject.Delete();
    //}
    //Player.Violations.StopContinuouslyViolating(StaticStrings.BreakingEnteringCrimeID);
//}
}

