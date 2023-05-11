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


public class SuicideActivity : DynamicActivity
{
    private uint GameTimeStartedSuicide;
    private bool IsCancelled;
    private IActionable Player;
    private int SuicideScene;
    private float ScenePhase;
    private ISettingsProvideable Settings;
    public SuicideActivity(IActionable player, ISettingsProvideable settings)
    {
        Player = player;
        Settings = settings;
    }
    public override ModItem ModItem { get; set; }
    public override string DebugString => "";
    public override bool CanPause { get; set; } = false;
    public override bool CanCancel { get; set; } = false;
    public override bool IsUpperBodyOnly { get; set; } = false;
    public override string PausePrompt { get; set; } = "Pause Activity";
    public override string CancelPrompt { get; set; } = "Stop Activity";
    public override string ContinuePrompt { get; set; } = "Continue Activity";
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
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "SuicideWatcher");
    }
    public override bool CanPerform(IActionable player)
    {
        if (player.IsOnFoot && player.ActivityManager.CanPerformActivitiesExtended && !player.ActivityManager.IsResting)
        {
            return true;
        }
        Game.DisplayHelp($"Cannot Suicide");
        return false;
    }
    private void Enter()
    {
        Player.ActivityManager.IsCommitingSuicide = true;
        Player.ActivityManager.IsPerformingActivity = true;
        if (Player.WeaponEquipment.CurrentWeaponIsOneHanded)//Player.CurrentWeaponCategory == WeaponCategory.Pistol || CurrentWeaponIsOneHanded)//Shoot YOurself
        {
            Enter("pistol");
            IdlePistol();
        }
        else
        {
            Enter("pill");
            IdlePill();
        }
        Exit();
    }

    private void Enter(string enterAnimation)
    {
        Vector3 SuicidePosition = Player.Character.Position;
        SuicideScene = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", SuicidePosition.X, SuicidePosition.Y, SuicidePosition.Z, 0.0f, 0.0f, Player.Character.Heading, 2);//270f //old
        NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_LOOPED", SuicideScene, false);
        NativeFunction.CallByName<bool>("TASK_SYNCHRONIZED_SCENE", Player.Character, SuicideScene, "mp_suicide", enterAnimation, 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);//std_perp_ds_a
        NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_PHASE", SuicideScene, 0.0f);
        GameTimeStartedSuicide = Game.GameTime;
    }
    private void IdlePill()
    {
        bool SwallowedPills = false;
        bool AddedPrompts = false;
        while (Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled && NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", SuicideScene) < 1.0f)
        {
            Player.WeaponEquipment.SetUnarmed();
            ScenePhase = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", SuicideScene);
            if (ScenePhase >= 0.2f)
            {
                NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_RATE", SuicideScene, 0.8f);
            }
            if (ScenePhase >= 0.25f && !SwallowedPills)
            {
                NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_RATE", SuicideScene, 0f);
                if (!AddedPrompts)
                {
                    Player.ButtonPrompts.AddPrompt("Suicide", "Commit Suicide", "CommitSuicide", GameControl.Attack, 1);
                    Player.ButtonPrompts.AddPrompt("Suicide", "Cancel", "CancelSuicide", Settings.SettingsManager.KeySettings.InteractCancel, 2);
                    AddedPrompts = true;
                }    
                if (Player.ButtonPrompts.IsPressed("CommitSuicide") && !Player.IsShowingActionWheel)
                {
                    SwallowedPills = true;
                    Player.ButtonPrompts.RemovePrompts("Suicide");
                    NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_RATE", SuicideScene, 1f);
                }
                else if (Player.ButtonPrompts.IsPressed("CancelSuicide"))
                {
                    Player.ButtonPrompts.RemovePrompts("Suicide");
                    break;
                }
            }
            if (ScenePhase >= 0.75f && SwallowedPills)
            {
                Player.Character.Kill();
            }
            DisableControls();
            GameFiber.Yield();
        }
    }

    private void DisableControls()
    {
        Game.DisableControlAction(0, GameControl.Attack, true);// false);
        Game.DisableControlAction(0, GameControl.Attack2, true);// false);

        Game.DisableControlAction(0, GameControl.VehicleAttack, true);// false);
        Game.DisableControlAction(0, GameControl.VehicleAttack2, true);// false);
    }

    private void IdlePistol()
    {
        bool AddedPrompts = false;
        while (Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled)
        {
            ScenePhase = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", SuicideScene);
            if (ScenePhase >= 0.3f)
            {
                NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_RATE", SuicideScene, 0f);
                if (!AddedPrompts)
                {
                    Player.ButtonPrompts.AddPrompt("Suicide", "Commit Suicide", "CommitSuicide", GameControl.Attack, 1);
                    Player.ButtonPrompts.AddPrompt("Suicide", "Cancel", "CancelSuicide", Settings.SettingsManager.KeySettings.InteractCancel, 2);
                    AddedPrompts = true;
                }
                if (Player.ButtonPrompts.IsPressed("CommitSuicide"))
                {
                    Player.ButtonPrompts.RemovePrompts("Suicide");
                    Vector3 HeadCoordinated = Player.Character.GetBonePosition(PedBoneId.Head);
                    NativeFunction.CallByName<bool>("SET_PED_SHOOTS_AT_COORD", Player.Character, HeadCoordinated.X, HeadCoordinated.Y, HeadCoordinated.Z, true);
                    Game.LocalPlayer.Character.Kill();
                    break;
                }
                else if (Player.ButtonPrompts.IsPressed("CancelSuicide"))
                {
                    Player.ButtonPrompts.RemovePrompts("Suicide");
                    break;
                }
            }
            GameFiber.Yield();
        }
    }
    private void Exit()
    {
        Player.ButtonPrompts.RemovePrompts("Suicide");
        //Player.Character.Tasks.Clear();
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        Player.ActivityManager.IsPerformingActivity = false;
        Player.ActivityManager.IsCommitingSuicide = false;
    }
    private void Setup()
    {
        AnimationDictionary.RequestAnimationDictionay("mp_suicide");
        Player.ButtonPrompts.RemovePrompts("Suicide");
    }
}

