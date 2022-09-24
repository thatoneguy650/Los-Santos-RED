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
    private RelationshipGroup PreviousRelationshipGroup;
    private RelationshipGroup HostageRG;
    private bool wasPersist = false;
    private bool wasSetPersistent = false;
    private bool IsMovingForward;
    private Vector3 AttachOffset;

    private bool IsHostageValid => Ped.Pedestrian.Exists() && Ped.Pedestrian.IsAlive && !Ped.IsUnconscious && !Ped.Pedestrian.IsRagdoll && !Ped.Pedestrian.IsGettingUp;
    private bool IsPlayerValidHostageTaker => Player.IsAliveAndFree && !Player.IsIncapacitated && Player.WeaponEquipment.CurrentWeapon != null && Player.WeaponEquipment.CurrentWeapon.CanPistolSuicide;
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
                if (!IsCancelled)
                {
                    TakHostage();
                }
                Cancel();
            }, "Conversation");
        }
    }
    public override void Continue()
    {

    }
    public override void Cancel()
    {
        EntryPoint.WriteToConsole("Human Shield - Cancel Pressed");
        ResetHostage();
        ResetPlayer();
        ResetVariables();
        GameFiber.StartNew(delegate
        {
            GameFiber.Wait(3000);
            if (Ped.Pedestrian.Exists())
            {
                Ped.Pedestrian.CollisionIgnoredEntity = null;
            }
            //Cancel();
        }, "Conversation");
        
    }
    public override void Pause()
    {
        Cancel();
    }
    public override bool IsPaused() => false;
    private void Setup()
    {
        IsCancelled = false;
        SetupPlayer();
        SetupVariables();
        SetupHostage();
    }
    private void SetupVariables()
    {
        Player.ButtonPrompts.RemovePrompts("Grab"); //Player.ButtonPromptList.RemoveAll(x => x.Group == "Grab");

        HostageRG = new RelationshipGroup("HOSTAGE");
        Game.LocalPlayer.Character.RelationshipGroup.SetRelationshipWith(HostageRG, Relationship.Companion);
        HostageRG.SetRelationshipWith(Game.LocalPlayer.Character.RelationshipGroup, Relationship.Companion);

        AnimationDictionary.RequestAnimationDictionayResult("anim@gangops@hostage@");
        AnimationDictionary.RequestAnimationDictionayResult("combat@drag_ped@");


        AnimationDictionary.RequestAnimationDictionayResult("move_action@generic@core");
        AnimationDictionary.RequestAnimationDictionayResult("move_strafe@generic");
        AnimationDictionary.RequestAnimationDictionayResult("move_strafe_melee_unarmed");





        AttachOffset = new Vector3(-0.31f, 0.12f, 0.04f);

       // AttachOffset = new Vector3(-0.5f, 0.5f, 0.04f);
        //weapons@unarmed
        //toolstest@
    }
    private void SetupPlayer()
    {
        Player.IsHoldingHostage = true;
    }
    private void SetupHostage()
    {
        if (Ped.Pedestrian.Exists())
        {
            wasPersist = Ped.Pedestrian.IsPersistent;
            wasSetPersistent = Ped.WasEverSetPersistent;
            PreviousRelationshipGroup = Ped.Pedestrian.RelationshipGroup;


            EntryPoint.WriteToConsole($"Grab Started PreviousRelationshipGroup {PreviousRelationshipGroup.Name}");


            Ped.CanBeTasked = false;
            Ped.Pedestrian.Tasks.Clear();
            Ped.Pedestrian.BlockPermanentEvents = true;
            Ped.Pedestrian.KeepTasks = true;
            Ped.Pedestrian.IsPersistent = true;


            NativeFunction.Natives.SET_ENABLE_HANDCUFFS(Ped.Pedestrian, true);
            Ped.Pedestrian.RelationshipGroup = HostageRG;
        }
        else
        {
            IsCancelled = true;
        }
    }
    private void TakHostage()
    {
        //SetCloseCamera();
        EntryPoint.WriteToConsole("Taking Hostage");
        PlayInitialAnimation();
        SetupPrompts();
        while (IsHostageValid && IsPlayerValidHostageTaker && !IsCancelled)
        {
            Ped.Pedestrian.CollisionIgnoredEntity = Game.LocalPlayer.Character;
            Ped.Pedestrian.Position = Player.Character.GetOffsetPosition(AttachOffset);
            Ped.Pedestrian.Heading = Game.LocalPlayer.Character.Heading;


            HeadingLoop();
            DirectionLoop();
            if(IsPromptPressed())
            {
                break;
            }
            AimingLoop();
            PlayerSettingsLoop();
            GameFiber.Yield();
        }

        if(Ped.Pedestrian.Exists())
        {
            Ped.Pedestrian.CollisionIgnoredEntity = Game.LocalPlayer.Character;
            Ped.Pedestrian.Position = Player.Character.GetOffsetPosition(AttachOffset);
            Ped.Pedestrian.Heading = Game.LocalPlayer.Character.Heading;
        }
    }
    private void PlayInitialAnimation()
    {
        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "anim@gangops@hostage@", "perp_idle", 8.0f, -8.0f, -1, 1 | 16 | 32, 0, false, false, false);//-1//NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "anim@gangops@hostage@", "perp_idle", 8.0f, -8.0f, -1, 1 | 16 | 32, 0, false, false, false);//-1
        if (Ped.Pedestrian.Exists())
        {
            //Ped.Pedestrian.AttachTo(Player.Character, 0, AttachOffset, new Rotator(0, 0, 0));
            Ped.Pedestrian.Position = Player.Character.GetOffsetPosition(AttachOffset);
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "anim@gangops@hostage@", "victim_idle", 8.0f, -8.0f, -1, 1 | 16 | 32, 0, false, false, false);//-1
        }
    }
    private void SetupPrompts()
    {
        Player.ButtonPrompts.RemovePrompts("Grab");
        if (!Player.ButtonPrompts.HasPrompt("Execute"))// if (!Player.ButtonPromptList.Any(x => x.Identifier == "Execute"))
        {
            Player.ButtonPrompts.AddPrompt("Hostage", "Execute", "Execute", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 1);//Player.ButtonPromptList.Add(new ButtonPrompt("Execute", "Hostage", "Execute", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 1));
        }
        if (!Player.ButtonPrompts.HasPrompt("Release"))//   !Player.ButtonPromptList.Any(x => x.Identifier == "Release"))
        {
            Player.ButtonPrompts.AddPrompt("Hostage", "Release", "Release", Settings.SettingsManager.KeySettings.InteractNegativeOrNo, 2);//Player.ButtonPromptList.Add(new ButtonPrompt("Release", "Hostage", "Release", Settings.SettingsManager.KeySettings.InteractNegativeOrNo, 2));
        }
    }
    private bool IsPromptPressed()
    {
        if (Player.ButtonPrompts.IsPressed("Execute")) //if (Player.ButtonPromptList.Any(x => x.Identifier == "Execute" && x.IsPressedNow))//demand cash?
        {
            Player.ButtonPrompts.RemovePrompts("Hostage");
            ExecuteHostageNew();
            return true;
        }
        else if (Player.ButtonPrompts.IsPressed("Release")) //else if (Player.ButtonPromptList.Any(x => x.Identifier == "Release" && x.IsPressedNow))//demand cash?
        {
            Player.ButtonPrompts.RemovePrompts("Hostage");
            ReleaseHostage();
            return true;
        }
        return false;
    }
    private void AimingLoop()
    {
        if (Player.Character.IsAiming)
        {
            if (!isAnimationPaused)
            {
                NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);

                //NativeFunction.Natives.SET_WEAPON_ANIMATION_OVERRIDE(Player.Character, Game.GetHashKey("Gang1H"));

                isAnimationPaused = true;
            }
        }
        else
        {
            if (isAnimationPaused)
            {
                // NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "combat@drag_ped@", "injured_drag_plyr", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                if (Ped.Pedestrian.Exists())
                {
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "anim@gangops@hostage@", "victim_idle", 8.0f, -8.0f, -1, 1 | 16, 0, false, false, false);
                }
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "anim@gangops@hostage@", "perp_idle", 8.0f, -8.0f, -1, 1 | 16 | 32, 0, false, false, false);

                //NativeFunction.Natives.SET_WEAPON_ANIMATION_OVERRIDE(Player.Character, Game.GetHashKey("Default"));
                isBackingUp = false;
                IsMovingForward = false;
                isAnimationPaused = false;
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
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "move_strafe@generic", "walk_bwd_180_loop", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                GameFiber.Wait(50);
                //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "combat@drag_ped@", "injured_drag_plyr", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                if (Ped.Pedestrian.Exists())
                {
                    //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "anim@gangops@hostage@", "victim_idle", 8.0f, -8.0f, -1, 1 | 16 | 32, 0, false, false, false);
                    //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "combat@drag_ped@", "injured_drag_plyr", 8.0f, -8.0f, -1, 1, 0, false, false, false);

                    //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "anim@gangops@hostage@", "victim_idle", 8.0f, -8.0f, -1, 1 | 16 | 32, 0, false, false, false);

                    //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "move_action@generic@core", "walk", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                    //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "weapons@unarmed", "walk_additive_backward", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                    //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "toolstest@", "walk_additive_backward", 8.0f, -8.0f, -1, 1, 0, false, false, false);

                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "move_strafe@generic", "walk_bwd_180_loop", 8.0f, -8.0f, -1, 1, 0, false, false, false);


                }
                isBackingUp = true;
            }
        }
        else if (isMoveUpPressed || Player.Character.IsWalking)
        {
            EntryPoint.WriteToConsole("SHIELD UP PRESSED");
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
            if (isBackingUp)
            {
                float AnimTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, "anim@gangops@hostage@", "perp_idle");
                NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "anim@gangops@hostage@", "perp_idle", 8.0f, -8.0f, -1, 1 | 16 | 32, 0, false, false, false);
                NativeFunction.Natives.SET_ENTITY_ANIM_CURRENT_TIME(Player.Character, "anim@gangops@hostage@", "perp_idle", AnimTime);
                if (Ped.Pedestrian.Exists())
                {
                    NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "anim@gangops@hostage@", "victim_idle", 8.0f, -8.0f, -1, 1 | 16 | 32, 0, false, false, false);
                    NativeFunction.Natives.SET_ENTITY_ANIM_CURRENT_TIME(Ped.Pedestrian, "anim@gangops@hostage@", "victim_idle", AnimTime);

                }
                isBackingUp = false;
            }
            if(IsMovingForward)
            {
                if (Ped.Pedestrian.Exists())
                {
                    NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "anim@gangops@hostage@", "victim_idle", 8.0f, -8.0f, -1, 1 | 16 | 32, 0, false, false, false);

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
    private void ReleaseHostage()
    {
        if (Ped.Pedestrian.Exists())
        {
            //Ped.Pedestrian.Detach();
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
    }
    private void ExecuteHostageNew()
    {
        if (Ped.Pedestrian.Exists())
        {
            Ped.Pedestrian.Detach();
            Vector3 HeadCoordinated = Ped.Pedestrian.GetBonePosition(PedBoneId.Head);
            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Ped.Pedestrian);
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "anim@gangops@hostage@", "perp_fail", 8.0f, -8.0f, -1, 0, 0, false, false, false);//-1
            if(Ped.Pedestrian.Exists())
            {
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "anim@gangops@hostage@", "victim_fail", 8.0f, -8.0f, -1, 0, 0, false, false, false);//-1
            }

            GameFiber.Wait(250);
            if (Ped.Pedestrian.Exists())
            {
                NativeFunction.CallByName<bool>("SET_PED_SHOOTS_AT_COORD", Player.Character, HeadCoordinated.X, HeadCoordinated.Y, HeadCoordinated.Z, true);
                //uint GameTimeReleased = Game.GameTime;
                //float AnimationTime = 0f;
                //while (AnimationTime < 0.5f && Game.GameTime - GameTimeReleased <= 3000)
                //{
                //    AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, "anim@gangops@hostage@", "perp_fail");
                //    GameFiber.Yield();
                //}
                if (Ped.Pedestrian.Exists())
                {
                    Ped.Pedestrian.Kill();
                }
            }
            
        }
    }
    private void ExecuteHostage()
    {

        
        if (Ped.Pedestrian.Exists())
        {
            Ped.Pedestrian.Detach();
            Vector3 HeadCoordinated = Ped.Pedestrian.GetBonePosition(PedBoneId.Head);
            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Ped.Pedestrian);



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
    private void ResetHostage()
    {
        if (Ped.Pedestrian.Exists())
        {
            
            //Ped.Pedestrian.Detach();
            Ped.Pedestrian.BlockPermanentEvents = false;
            Ped.Pedestrian.KeepTasks = false;
            Ped.Pedestrian.IsPersistent = wasPersist;
            NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
            Ped.Pedestrian.RelationshipGroup = PreviousRelationshipGroup;

            Ped.WasEverSetPersistent = wasSetPersistent;
            Ped.CanBeAmbientTasked = true;
            Ped.CanBeTasked = true;
            
            NativeFunction.Natives.SET_ENABLE_HANDCUFFS(Ped.Pedestrian, false);
            //if(Ped.Pedestrian.RelationshipGroup.Name == "HOSTAGE")
            //{
            //    Ped.Pedestrian.RelationshipGroup = new RelationshipGroup("CITIZEN");
            //}

            //EntryPoint.WriteToConsole($"PreviousRelationshipGroup {PreviousRelationshipGroup.Name} Ped.Pedestrian.RelationshipGroup {Ped.Pedestrian.RelationshipGroup.Name} 1");
        }
    }
    private void ResetPlayer()
    {
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        Player.IsHoldingHostage = false;

        Game.LocalPlayer.Character.RelationshipGroup.SetRelationshipWith(HostageRG, Relationship.Neutral);
        HostageRG.SetRelationshipWith(Game.LocalPlayer.Character.RelationshipGroup, Relationship.Neutral);

        Game.DisableControlAction(0, GameControl.Sprint, false);
        Game.DisableControlAction(0, GameControl.Jump, false);
    }
    private void ResetVariables()
    {
        Player.ButtonPrompts.RemovePrompts("Hostage");
    }

    //private void SetCloseCamera()
    //{
    //    int viewMode = NativeFunction.Natives.GET_FOLLOW_PED_CAM_VIEW_MODE<int>();
    //    if (viewMode != 4 && viewMode != 0)
    //    {
    //        storedViewMode = viewMode;
    //        NativeFunction.Natives.SET_FOLLOW_PED_CAM_VIEW_MODE(0);
    //        EntryPoint.WriteToConsole($"SetCloseCamera viewMode {viewMode} storedViewMode {storedViewMode}", 5);
    //    }
    //}
    //private void SetRegularCamera()
    //{
    //    int viewMode = NativeFunction.Natives.GET_FOLLOW_PED_CAM_VIEW_MODE<int>();
    //    if (viewMode != 4)
    //    {
    //        if (viewMode != storedViewMode)
    //        {
    //            NativeFunction.Natives.SET_FOLLOW_PED_CAM_VIEW_MODE(storedViewMode);
    //            storedViewMode = -1;
    //        }
    //        EntryPoint.WriteToConsole($"SetCloseCamera storedViewMode {storedViewMode}", 5);
    //    }
    //}
    //private bool PlayAnimation(string dictionary, string animation)
    //{
    //    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, dictionary, animation, 1.0f, -1.0f, -1, 50, 0, false, false, false);//-1
    //    NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, dictionary, animation, 8.0f, -8.0f, -1, 2, 0, false, false, false);
    //    uint GameTimeStartedLootAnimation = Game.GameTime;
    //    float AnimationTime = 0.0f;
    //    while (AnimationTime < 1.0f && !IsCancelled && Game.GameTime - GameTimeStartedLootAnimation <= 10000)
    //    {
    //        AnimationTime = NativeFunction.Natives.GET_ENTITY_ANIM_CURRENT_TIME<float>(Player.Character, dictionary, animation);
    //        if (Player.IsMoveControlPressed)
    //        {
    //            IsCancelled = true;
    //        }
    //        if (!Ped.Pedestrian.Exists() || !Player.IsAliveAndFree)
    //        {
    //            IsCancelled = true;
    //            break;
    //        }
    //        GameFiber.Yield();
    //    }
    //    if (!IsCancelled && AnimationTime >= 1.0f)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

}