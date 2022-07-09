using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

public class HumanShield : DynamicActivity
{
    private uint GameTimeStartedConversing;
    private bool IsActivelyConversing;
    private bool IsTasked;
    private bool IsBlockedEvents;
    private PedExt Ped;
    private IInteractionable Player;
    private bool CancelledConversation;
    private ISettingsProvideable Settings;
    private ICrimes Crimes;
    private dynamic pedHeadshotHandle;
    private IModItems ModItems;
    private bool IsCancelled;
    private WeaponInformation LastWeapon;


    private bool isAnimationPaused = false;
    private int storedViewMode;
    private bool isBackingUp;

    //private WeaponInformation previousWeapon;

    public HumanShield(IInteractionable player, PedExt ped, ISettingsProvideable settings, ICrimes crimes, IModItems modItems)
    {
        Player = player;
        Ped = ped;
        Settings = settings;
        Crimes = crimes;
        ModItems = modItems;
    }
    public override string DebugString => $"TimesInsultedByPlayer {Ped.TimesInsultedByPlayer} FedUp {Ped.IsFedUpWithPlayer}";
    public override ModItem ModItem { get; set; }
    public override bool CanPause { get; set; } = false;
    public override bool CanCancel { get; set; } = false;
    public override string PausePrompt { get; set; } = "Pause Activity";
    public override string CancelPrompt { get; set; } = "Stop Activity";
    public override string ContinuePrompt { get; set; } = "Continue Activity";
    public override void Start()
    {
        if (Ped.Pedestrian.Exists())
        {
            GameFiber.StartNew(delegate
            {
                Setup();
                TakHostage();
                Cancel();
            }, "Conversation");
        }
    }


    private void Setup()
    {
        Player.IsHoldingHostage = true;
        EntryPoint.WriteToConsole($"Grab Started");
        Ped.CanBeTasked = false;
        Ped.Pedestrian.Tasks.Clear();
        Ped.Pedestrian.BlockPermanentEvents = true;
        Ped.Pedestrian.KeepTasks = true;
        Ped.Pedestrian.IsPersistent = true;
        AnimationDictionary.RequestAnimationDictionay("anim@gangops@hostage@");


        AnimationDictionary.RequestAnimationDictionay("combat@drag_ped@");

    }
    private void TakHostage()
    {
        SetCloseCamera();
        EntryPoint.WriteToConsole("Taking Hostage");
        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "anim@gangops@hostage@", "perp_idle", 8.0f, -8.0f, -1, 1 | 16 | 32, 0, false, false, false);//-1//NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "anim@gangops@hostage@", "perp_idle", 8.0f, -8.0f, -1, 1 | 16 | 32, 0, false, false, false);//-1
        Ped.Pedestrian.AttachTo(Player.Character, 0, new Vector3(-0.31f, 0.12f, 0.04f), new Rotator(0, 0, 0));
        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "anim@gangops@hostage@", "victim_idle", 8.0f, -8.0f, -1, 1, 0, false, false, false);//-1
        uint GameTimeStarted = Game.GameTime;
        Player.ButtonPromptList.RemoveAll(x => x.Group == "Grab");
        if (!Player.ButtonPromptList.Any(x => x.Identifier == "Execute"))
        {
            Player.ButtonPromptList.Add(new ButtonPrompt("Execute", "Hostage", "Execute", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 1));
        }
        if (!Player.ButtonPromptList.Any(x => x.Identifier == "Release"))
        {
            Player.ButtonPromptList.Add(new ButtonPrompt("Release", "Hostage", "Release", Settings.SettingsManager.KeySettings.InteractNegativeOrNo, 2));
        }
        while (Ped.Pedestrian.Exists() && Ped.Pedestrian.IsAlive && Player.IsAliveAndFree && !Player.IsIncapacitated)
        {
            HeadingLoop();
            DirectionLoop();
            if (Player.ButtonPromptList.Any(x => x.Identifier == "Execute" && x.IsPressedNow))//demand cash?
            {
                Player.ButtonPromptList.RemoveAll(x => x.Group == "Hostage");
                ExecuteHostage();
                break;
            }
            else if (Player.ButtonPromptList.Any(x => x.Identifier == "Release" && x.IsPressedNow))//demand cash?
            {
                Player.ButtonPromptList.RemoveAll(x => x.Group == "Hostage");
                ReleaseHostage();
                break;
            }
            if(Player.Character.IsAiming)
            {
                if (!isAnimationPaused)
                {
                    NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
                    isAnimationPaused = true;
                }
            }
            else
            {
                if(isAnimationPaused)
                {
                   // NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "combat@drag_ped@", "injured_drag_plyr", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "anim@gangops@hostage@", "perp_idle", 8.0f, -8.0f, -1, 1 | 16 | 32, 0, false, false, false);
                    isAnimationPaused = false;
                }
            }

            NativeFunction.Natives.SET_PED_MOVE_RATE_OVERRIDE<uint>(Player.Character, 0.75f);
            NativeFunction.Natives.SET_PED_CONFIG_FLAG(Player.Character, (int)PedConfigFlags.PED_FLAG_NO_PLAYER_MELEE, true);
            GameFiber.Yield();
        }
        Player.ButtonPromptList.RemoveAll(x => x.Group == "Hostage");

        SetRegularCamera();
        NativeFunction.Natives.SET_PED_CONFIG_FLAG(Player.Character, (int)PedConfigFlags.PED_FLAG_NO_PLAYER_MELEE, false);
    }
    private void DirectionLoop()
    {
        bool isMoveUpPressed = false;
        bool isMoveDownPressed = false;

        if (Game.IsControlPressed(2, GameControl.MoveDownOnly) || Game.IsControlPressed(2, GameControl.MoveDown))
        {
            isMoveDownPressed = true;
        }
        if (Game.IsControlPressed(2, GameControl.MoveUpOnly) || Game.IsControlPressed(2, GameControl.MoveUp))
        {
            isMoveUpPressed = true;
        }
        if (isMoveDownPressed)
        {
            EntryPoint.WriteToConsole("SHIELD DOWN PRESSED");
            if(!isBackingUp)
            {
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "combat@drag_ped@", "injured_drag_plyr", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                isBackingUp = true;
            }
        }
        //else if (isMoveUpPressed)
        //{
        //    EntryPoint.WriteToConsole("SHIELD UP PRESSED");
        //}
        else
        {
            if (isBackingUp)
            {

                float AnimTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, "anim@gangops@hostage@", "perp_idle");



                NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "anim@gangops@hostage@", "perp_idle", 8.0f, -8.0f, -1, 1 | 16 | 32, 0, false, false, false);
                NativeFunction.Natives.SET_ENTITY_ANIM_CURRENT_TIME(Player.Character, "anim@gangops@hostage@", "perp_idle", AnimTime);
                isBackingUp = false;
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

    private void ReleaseHostage()
    {
        Ped.Pedestrian.Detach();
        NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
        NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Ped.Pedestrian);
        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "anim@gangops@hostage@", "perp_fail", 8.0f, -8.0f, -1, 0, 0, false, false, false);//-1
        GameFiber.Sleep(500);
        if (Ped.Pedestrian.Exists())
        {
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "anim@gangops@hostage@", "victim_success", 8.0f, -8.0f, -1, 0, 0, false, false, false);//-1

            uint GameTimeReleased = Game.GameTime;
            float AnimationTime = 0f;
            while (AnimationTime < 0.5f && Game.GameTime - GameTimeReleased <= 3000)
            {
                AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, "anim@gangops@hostage@", "perp_fail");
                GameFiber.Yield();
            }
        }
    }
    private void ExecuteHostage()
    {
        Ped.Pedestrian.Detach();
        Vector3 HeadCoordinated = Ped.Pedestrian.GetBonePosition(PedBoneId.Head);
        NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
        NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Ped.Pedestrian);
        
        if (Ped.Pedestrian.Exists())
        {
            //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "anim@gangops@hostage@", "victim_fail", 8.0f, -8.0f, -1, 0, 0, false, false, false);//-1
           // GameFiber.Sleep(500);
            NativeFunction.CallByName<bool>("SET_PED_SHOOTS_AT_COORD", Player.Character, HeadCoordinated.X, HeadCoordinated.Y, HeadCoordinated.Z, true);
            if (Ped.Pedestrian.Exists())
            {
                Ped.Pedestrian.Kill();
                //uint GameTimeReleased = Game.GameTime;
                //float AnimationTime = 0f;


                //GameFiber.Sleep(500);
                //NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
                //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "anim@gangops@hostage@", "perp_fail", 8.0f, -8.0f, -1, 0, 0, false, false, false);//-1

                //while (AnimationTime < 0.5f && Game.GameTime - GameTimeReleased <= 3000)
                //{
                //    AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, "anim@gangops@hostage@", "perp_fail");
                //    GameFiber.Yield();
                //}
            }
        }
    }

    private void SetCloseCamera()
    {
        int viewMode = NativeFunction.Natives.GET_FOLLOW_PED_CAM_VIEW_MODE<int>();
        if (viewMode != 4 && viewMode != 0)
        {
            storedViewMode = viewMode;
            NativeFunction.Natives.SET_FOLLOW_PED_CAM_VIEW_MODE(0);
            EntryPoint.WriteToConsole($"SetCloseCamera viewMode {viewMode} storedViewMode {storedViewMode}", 5);
        }
    }
    private void SetRegularCamera()
    {
        int viewMode = NativeFunction.Natives.GET_FOLLOW_PED_CAM_VIEW_MODE<int>();
        if (viewMode != 4)
        {
            if (viewMode != storedViewMode)
            {
                NativeFunction.Natives.SET_FOLLOW_PED_CAM_VIEW_MODE(storedViewMode);
                storedViewMode = -1;
            }
            EntryPoint.WriteToConsole($"SetCloseCamera storedViewMode {storedViewMode}", 5);
        }
    }


    private bool PlayAnimation(string dictionary, string animation)
    {
        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, dictionary, animation, 1.0f, -1.0f, -1, 50, 0, false, false, false);//-1
        NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, dictionary, animation, 8.0f, -8.0f, -1, 2, 0, false, false, false);
        uint GameTimeStartedLootAnimation = Game.GameTime;
        float AnimationTime = 0.0f;
        while (AnimationTime < 1.0f && !IsCancelled && Game.GameTime - GameTimeStartedLootAnimation <= 10000)
        {
            AnimationTime = NativeFunction.Natives.GET_ENTITY_ANIM_CURRENT_TIME<float>(Player.Character, dictionary, animation);
            if (Player.IsMoveControlPressed)
            {
                IsCancelled = true;
            }
            if (!Ped.Pedestrian.Exists() || !Player.IsAliveAndFree)
            {
                IsCancelled = true;
                break;
            }
            GameFiber.Yield();
        }
        if (!IsCancelled && AnimationTime >= 1.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public override void Continue()
    {

    }
    public override void Cancel()
    {
        if(Ped.Pedestrian.Exists())
        {
            Ped.Pedestrian.Detach();
            Ped.Pedestrian.BlockPermanentEvents = false;
            Ped.Pedestrian.KeepTasks = false;
            Ped.Pedestrian.IsPersistent = false;
            NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
        }
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        Player.IsHoldingHostage = false;
    }
    public override void Pause()
    {
        Cancel();
    }
    public override bool IsPaused() => false;
}