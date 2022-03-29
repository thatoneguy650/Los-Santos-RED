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
    }
    private void TakHostage()
    {
        EntryPoint.WriteToConsole("Taking Hostage");
        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "anim@gangops@hostage@", "perp_idle", 8.0f, -8.0f, -1, 50, 0, false, false, false);//-1
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
        while (Ped.Pedestrian.Exists() && Ped.Pedestrian.IsAlive && Player.IsAliveAndFree)
        {
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
                //Player.Character.IsArmIkEnabled = true;
            }
            else
            {
                if(isAnimationPaused)
                {
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "anim@gangops@hostage@", "perp_idle", 8.0f, -8.0f, -1, 50, 0, false, false, false);
                    isAnimationPaused = false;
                }
                //Player.Character.IsArmIkEnabled = false;
            }
            


            GameFiber.Yield();
        }
        Player.ButtonPromptList.RemoveAll(x => x.Group == "Hostage");
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
}