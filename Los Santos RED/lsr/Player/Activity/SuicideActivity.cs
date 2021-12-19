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
    private Mod.Player Player;
    private int SuicideScene;
    private float ScenePhase;
    private ISettingsProvideable Settings;
    public SuicideActivity(Mod.Player player, ISettingsProvideable settings)
    {
        Player = player;
        Settings = settings;
    }
    public override ModItem ModItem { get; set; }
    public override string DebugString => "";
    public override void Cancel()
    {
        IsCancelled = true;
    }
    public override void Pause()
    {

    }
    public override void Continue()
    {

    }
    public override void Start()
    {
        Setup();
        GameFiber ScenarioWatcher = GameFiber.StartNew(delegate
        {
            Enter();
        }, "SuicideWatcher");
    }
    private void Enter()
    {
        Player.IsCommitingSuicide = true;
        Player.IsPerformingActivity = true;
        if (Player.CurrentWeaponIsOneHanded)//Player.CurrentWeaponCategory == WeaponCategory.Pistol || CurrentWeaponIsOneHanded)//Shoot YOurself
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
        while (Player.CanPerformActivities && !IsCancelled && NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", SuicideScene) < 1.0f)
        {
            Player.SetUnarmed();
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
                    Player.ButtonPrompts.Add(new ButtonPrompt("Commit Suicide", "Suicide", "CommitSuicide", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 1));
                    Player.ButtonPrompts.Add(new ButtonPrompt("Cancel", "Suicide", "CancelSuicide", Settings.SettingsManager.KeySettings.InteractCancel, 2));
                    AddedPrompts = true;
                }    
                if (Player.ButtonPrompts.Any(x => x.Identifier == "CommitSuicide" && x.IsPressedNow))
                {
                    SwallowedPills = true;
                    Player.ButtonPrompts.RemoveAll(x => x.Group == "Suicide");
                    NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_RATE", SuicideScene, 1f);
                }
                else if (Player.ButtonPrompts.Any(x => x.Identifier == "CancelSuicide" && x.IsPressedNow))
                {
                    Player.ButtonPrompts.RemoveAll(x => x.Group == "Suicide");
                    break;
                }
            }
            if (ScenePhase >= 0.75f && SwallowedPills)
            {
                Player.Character.Kill();
            }
            GameFiber.Yield();
        }
    }
    private void IdlePistol()
    {
        bool AddedPrompts = false;
        while (Player.CanPerformActivities && !IsCancelled)
        {
            ScenePhase = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", SuicideScene);
            if (ScenePhase >= 0.3f)
            {
                NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_RATE", SuicideScene, 0f);
                if (!AddedPrompts)
                {
                    Player.ButtonPrompts.Add(new ButtonPrompt("Commit Suicide", "Suicide", "CommitSuicide", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 1));
                    Player.ButtonPrompts.Add(new ButtonPrompt("Cancel", "Suicide", "CancelSuicide", Settings.SettingsManager.KeySettings.InteractCancel, 2));
                    AddedPrompts = true;
                }
                if (Player.ButtonPrompts.Any(x => x.Identifier == "CommitSuicide" && x.IsPressedNow))
                {
                    Player.ButtonPrompts.RemoveAll(x => x.Group == "Suicide");
                    Vector3 HeadCoordinated = Player.Character.GetBonePosition(PedBoneId.Head);
                    NativeFunction.CallByName<bool>("SET_PED_SHOOTS_AT_COORD", Player.Character, HeadCoordinated.X, HeadCoordinated.Y, HeadCoordinated.Z, true);
                    Game.LocalPlayer.Character.Kill();
                    break;
                }
                else if (Player.ButtonPrompts.Any(x => x.Identifier == "CancelSuicide" && x.IsPressedNow))
                {
                    Player.ButtonPrompts.RemoveAll(x => x.Group == "Suicide");
                    break;
                }
            }
            GameFiber.Yield();
        }
    }
    private void Exit()
    {
        Player.ButtonPrompts.RemoveAll(x => x.Group == "Suicide");
        //Player.Character.Tasks.Clear();
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        Player.IsPerformingActivity = false;
        Player.IsCommitingSuicide = false;
    }
    private void Setup()
    {
        AnimationDictionary.RequestAnimationDictionay("mp_suicide");
        Player.ButtonPrompts.RemoveAll(x => x.Group == "Suicide");
    }
}

