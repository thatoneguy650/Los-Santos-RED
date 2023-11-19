//using ExtensionsMethods;
//using LosSantosRED.lsr.Interface;
//using LosSantosRED.lsr.Player;
//using Rage;
//using Rage.Native;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Windows.Forms;

//public class HumanShield_Old : DynamicActivity
//{
//    private uint GameTimeStartedConversing;
//    private bool IsActivelyConversing;
//    private bool IsTasked;
//    private bool IsBlockedEvents;
//    private PedExt Ped;
//    private IInteractionable Player;
//    private bool CancelledConversation;
//    private ISettingsProvideable Settings;
//    private ICrimes Crimes;
//    private dynamic pedHeadshotHandle;
//    private IModItems ModItems;
//    private bool IsCancelled;
//    private WeaponInformation LastWeapon;
//    private bool isAnimationPaused = false;
//    private int storedViewMode;


//    private bool IsBackingUp;
//    private bool IsMovingForward;

//    private RelationshipGroup PreviousRelationshipGroup;
//    private RelationshipGroup HostageRG;
//    private bool wasPersist = false;
//    private bool wasSetPersistent = false;

//    private Vector3 AttachOffset;

//    private bool IsHostageValid => Ped.Pedestrian.Exists() && Ped.Pedestrian.IsAlive && !Ped.IsUnconscious && !Ped.Pedestrian.IsRagdoll && !Ped.Pedestrian.IsGettingUp;
//    private bool IsPlayerValidHostageTaker => Player.IsAliveAndFree && !Player.IsIncapacitated && Player.WeaponEquipment.CurrentWeapon != null && Player.WeaponEquipment.CurrentWeapon.CanPistolSuicide;
//    public HumanShield_Old(IInteractionable player, PedExt ped, ISettingsProvideable settings, ICrimes crimes, IModItems modItems)
//    {
//        Player = player;
//        Ped = ped;
//        Settings = settings;
//        Crimes = crimes;
//        ModItems = modItems;
//    }
//    public override string DebugString => $"TimesInsultedByPlayer {Ped.TimesInsultedByPlayer} FedUp {Ped.IsFedUpWithPlayer}";
//    public override ModItem ModItem { get; set; }
//    public override bool CanPause { get; set; } = false;
//    public override bool CanCancel { get; set; } = false;
//    public override bool IsUpperBodyOnly { get; set; } = false;
//    public override string PausePrompt { get; set; } = "Pause Activity";
//    public override string CancelPrompt { get; set; } = "Stop Activity";
//    public override string ContinuePrompt { get; set; } = "Continue Activity";
//    public override void Start()
//    {
//        if (Ped.Pedestrian.Exists())
//        {
//            GameFiber.StartNew(delegate
//            {
//                try
//                {
//                    Setup();
//                    if (!IsCancelled)
//                    {
//                        TakHostage();
//                    }
//                    Cancel();
//                }
//                catch (Exception ex)
//                {
//                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
//                    EntryPoint.ModController.CrashUnload();
//                }
//            }, "Conversation");
//        }
//    }
//    public override void Continue()
//    {

//    }
//    public override void Cancel()
//    {
//        //EntryPoint.WriteToConsoleTestLong("Human Shield - Cancel Pressed");
//        ResetHostage();
//        ResetPlayer();
//        ResetVariables();
//        GameFiber.StartNew(delegate
//        {
//            try
//            {
//                uint gametimeStarted = Game.GameTime;
//                while (EntryPoint.ModController.IsRunning)
//                {
//                    if (Game.GameTime - gametimeStarted >= 3000)
//                    {
//                        break;
//                    }
//                    GameFiber.Yield();
//                }

//                //GameFiber.Wait(3000);
//                if (Ped.Pedestrian.Exists())
//                {
//                    Ped.Pedestrian.CollisionIgnoredEntity = null;
//                }
//                //Cancel();
//            }
//            catch (Exception ex)
//            {
//                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
//                EntryPoint.ModController.CrashUnload();
//            }
//        }, "Conversation");

//    }
//    public override void Pause()
//    {
//        Cancel();
//    }
//    public override bool IsPaused() => false;
//    public override bool CanPerform(IActionable player)
//    {
//        if (!player.ActivityManager.CanGrabLookedAtPed)
//        {
//            Game.DisplayHelp($"Cannot take ped hostage");
//            return false;
//        }
//        if (player.IsOnFoot && player.ActivityManager.CanPerformActivitesBase)
//        {
//            return true;
//        }
//        Game.DisplayHelp($"Cannot Take Hostage");
//        return false;
//    }
//    private void Setup()
//    {
//        IsCancelled = false;
//        SetupVariables();
//        SetupPlayer();
//        SetupHostage();
//    }
//    private void SetupVariables()
//    {
//        Player.ButtonPrompts.RemovePrompts("Grab"); //Player.ButtonPromptList.RemoveAll(x => x.Group == "Grab");
//        HostageRG = new RelationshipGroup("HOSTAGE");
//        Game.LocalPlayer.Character.RelationshipGroup.SetRelationshipWith(HostageRG, Relationship.Companion);
//        HostageRG.SetRelationshipWith(Game.LocalPlayer.Character.RelationshipGroup, Relationship.Companion);
//        if (!AnimationDictionary.RequestAnimationDictionayResult("anim@gangops@hostage@"))
//        {
//            EntryPoint.WriteToConsole("ERROR HUMAN SHIELD COULD NOT LOAD ANIMATION DICTIONARY 1");
//            //IsCancelled = true;
//            //return;
//        }
//        if (!AnimationDictionary.RequestAnimationDictionayResult("move_action@generic@core"))
//        {
//            EntryPoint.WriteToConsole("ERROR HUMAN SHIELD COULD NOT LOAD ANIMATION DICTIONARY 2");
//            //IsCancelled = true;
//            //return;
//        }
//        if (!AnimationDictionary.RequestAnimationDictionayResult("move_strafe@generic"))
//        {
//            EntryPoint.WriteToConsole("ERROR HUMAN SHIELD COULD NOT LOAD ANIMATION DICTIONARY 3");
//            //IsCancelled = true;
//            //return;
//        }
//        AttachOffset = new Vector3(-0.31f, 0.12f, 0.04f);
//    }
//    private void SetupPlayer()
//    {
//        if (IsCancelled)
//        {
//            return;
//        }
//        Player.ActivityManager.IsPerformingActivity = true;
//        Player.ActivityManager.IsHoldingHostage = true;
//    }
//    private void SetupHostage()
//    {
//        if (IsCancelled)
//        {
//            return;
//        }
//        if (Ped.Pedestrian.Exists())
//        {
//            wasPersist = Ped.Pedestrian.IsPersistent;
//            wasSetPersistent = Ped.WasEverSetPersistent;
//            PreviousRelationshipGroup = Ped.Pedestrian.RelationshipGroup;
//            //EntryPoint.WriteToConsoleTestLong($"Grab Started PreviousRelationshipGroup {PreviousRelationshipGroup.Name}");
//            Ped.CanBeTasked = false;
//            NativeFunction.Natives.SET_PED_SHOULD_PLAY_IMMEDIATE_SCENARIO_EXIT(Ped.Pedestrian);
//            Ped.Pedestrian.Tasks.Clear();
//            Ped.Pedestrian.BlockPermanentEvents = true;
//            Ped.Pedestrian.KeepTasks = true;
//            Ped.Pedestrian.IsPersistent = true;
//            Ped.IsBeingHeldAsHostage = true;
//            NativeFunction.Natives.SET_ENABLE_HANDCUFFS(Ped.Pedestrian, true);
//            Ped.Pedestrian.RelationshipGroup = HostageRG;
//            NativeFunction.Natives.SET_PED_SHOULD_PLAY_IMMEDIATE_SCENARIO_EXIT(Ped.Pedestrian);
//        }
//        else
//        {
//            IsCancelled = true;
//        }
//    }
//    private void TakHostage()
//    {
//        //SetCloseCamera();
//        EntryPoint.WriteToConsole("Taking Hostage 1");
//        PlayInitialAnimation();
//        SetupPrompts();
//        EntryPoint.WriteToConsole("Taking Hostage 2");
//        //GameFiber.Sleep(1000);

//        while (IsHostageValid && IsPlayerValidHostageTaker && !IsCancelled)
//        {
//            //EntryPoint.WriteToConsole("Taking Hostage 3");
//            Ped.Pedestrian.CollisionIgnoredEntity = Game.LocalPlayer.Character;
//            Ped.Pedestrian.Position = Player.Character.GetOffsetPosition(AttachOffset);
//            Ped.Pedestrian.Heading = Game.LocalPlayer.Character.Heading;
//            HeadingLoop();
//            DirectionLoop();
//            if (IsPromptPressed())
//            {
//                break;
//            }

//            AimingLoop();
//            PlayerSettingsLoop();
//            DisableControls();
//            GameFiber.Yield();
//        }
//        EntryPoint.WriteToConsole("Taking Hostage 4");
//        if (Ped.Pedestrian.Exists())
//        {
//            Ped.Pedestrian.CollisionIgnoredEntity = Game.LocalPlayer.Character;
//            Ped.Pedestrian.Position = Player.Character.GetOffsetPosition(AttachOffset);
//            Ped.Pedestrian.Heading = Game.LocalPlayer.Character.Heading;
//        }
//    }
//    private void PlayInitialAnimation()
//    {
//        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "anim@gangops@hostage@", "perp_idle", 8.0f, -8.0f, -1, 1 | 8 | 16 | 32, 0, false, false, false);//-1//NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "anim@gangops@hostage@", "perp_idle", 8.0f, -8.0f, -1, 1 | 16 | 32, 0, false, false, false);//-1
//        if (Ped.Pedestrian.Exists())
//        {
//            Ped.Pedestrian.CollisionIgnoredEntity = Game.LocalPlayer.Character;
//            Ped.Pedestrian.Position = Player.Character.GetOffsetPosition(AttachOffset);
//            Ped.Pedestrian.Heading = Game.LocalPlayer.Character.Heading;
//            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "anim@gangops@hostage@", "victim_idle", 8.0f, -8.0f, -1, (int)(eAnimationFlags.AF_LOOPING | eAnimationFlags.AF_NOT_INTERRUPTABLE | eAnimationFlags.AF_UPPERBODY | eAnimationFlags.AF_SECONDARY | eAnimationFlags.AF_FORCE_START), 0, false, false, false);//-1
//        }
//    }


//    private void SetupPrompts()
//    {
//        Player.ButtonPrompts.RemovePrompts("Grab");
//        if (!Player.ButtonPrompts.HasPrompt("Execute"))// if (!Player.ButtonPromptList.Any(x => x.Identifier == "Execute"))
//        {
//            Player.ButtonPrompts.AddPrompt("Hostage", "Execute", "Execute", GameControl.Attack, 1);//Player.ButtonPromptList.Add(new ButtonPrompt("Execute", "Hostage", "Execute", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 1));
//        }
//        if (!Player.ButtonPrompts.HasPrompt("Release"))//   !Player.ButtonPromptList.Any(x => x.Identifier == "Release"))
//        {
//            Player.ButtonPrompts.AddPrompt("Hostage", "Release", "Release", Settings.SettingsManager.KeySettings.InteractNegativeOrNo, 2);//Player.ButtonPromptList.Add(new ButtonPrompt("Release", "Hostage", "Release", Settings.SettingsManager.KeySettings.InteractNegativeOrNo, 2));
//        }
//    }
//    private bool IsPromptPressed()
//    {
//        if (Player.IsShowingActionWheel)
//        {
//            return false;
//        }
//        if (Player.ButtonPrompts.IsPressed("Execute")) //if (Player.ButtonPromptList.Any(x => x.Identifier == "Execute" && x.IsPressedNow))//demand cash?
//        {
//            Player.ButtonPrompts.RemovePrompts("Hostage");
//            ExecuteHostageNew();
//            return true;
//        }
//        else if (Player.ButtonPrompts.IsPressed("Release")) //else if (Player.ButtonPromptList.Any(x => x.Identifier == "Release" && x.IsPressedNow))//demand cash?
//        {
//            Player.ButtonPrompts.RemovePrompts("Hostage");
//            ReleaseHostage();
//            return true;
//        }
//        return false;
//    }
//    private void AimingLoop()
//    {
//        if (Player.Character.IsAiming)
//        {
//            if (!isAnimationPaused)
//            {
//                NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
//                //NativeFunction.Natives.SET_WEAPON_ANIMATION_OVERRIDE(Player.Character, Game.GetHashKey("Gang1H"));
//                isAnimationPaused = true;
//                Player.ButtonPrompts.RemovePrompts("Hostage");
//            }

//        }
//        else
//        {
//            if (isAnimationPaused)
//            {
//                if (Ped.Pedestrian.Exists())
//                {
//                    // NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "anim@gangops@hostage@", "victim_idle", 8.0f, -8.0f, -1, 1 | 16, 0, false, false, false);
//                }
//                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "anim@gangops@hostage@", "perp_idle", 8.0f, -8.0f, -1, 1 | 16 | 32, 0, false, false, false);
//                //NativeFunction.Natives.SET_WEAPON_ANIMATION_OVERRIDE(Player.Character, Game.GetHashKey("Default"));
//                IsBackingUp = false;
//                IsMovingForward = false;
//                isAnimationPaused = false;
//                SetupPrompts();
//            }
//        }
//    }
//    private void PlayerSettingsLoop()
//    {
//        NativeFunction.Natives.SET_PED_MOVE_RATE_OVERRIDE<uint>(Player.Character, 0.75f);
//        Player.Stance.SetActionMode(false);
//        Player.Stance.SetStealthMode(false);
//        Game.DisableControlAction(0, GameControl.Sprint, true);
//        Game.DisableControlAction(0, GameControl.Jump, true);
//        Player.ButtonPrompts.RemovePrompts("Grab");
//    }
//    private void DirectionLoop()
//    {
//        bool IsMoveUpPressed = false;
//        bool IsMoveDownPressed = false;

//        if (Game.IsControlPressed(2, GameControl.MoveDownOnly) || Game.IsControlPressed(2, GameControl.MoveDown))
//        {
//            IsMoveDownPressed = true;
//        }
//        if (Game.IsControlPressed(2, GameControl.MoveUpOnly) || Game.IsControlPressed(2, GameControl.MoveUp))
//        {
//            IsMoveUpPressed = true;
//        }


//        if (IsMoveDownPressed)
//        {
//            //EntryPoint.WriteToConsoleTestLong("SHIELD DOWN PRESSED");
//            if (!IsBackingUp)
//            {
//                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "move_strafe@generic", "walk_bwd_180_loop", 8.0f, -8.0f, -1, 1, 0, false, false, false);
//                GameFiber.Wait(50);
//                if (Ped.Pedestrian.Exists())
//                {
//                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "move_strafe@generic", "walk_bwd_180_loop", 8.0f, -8.0f, -1, 1, 0, false, false, false);
//                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "anim@gangops@hostage@", "victim_idle", 8.0f, -8.0f, -1, 1 | 16 | 32, 0, false, false, false);
//                }
//                IsBackingUp = true;
//            }
//        }
//        else if (IsMoveUpPressed || Player.Character.IsWalking)
//        {
//            //EntryPoint.WriteToConsoleTestLong("SHIELD UP PRESSED");
//            if (!IsMovingForward)
//            {
//                if (Ped.Pedestrian.Exists())
//                {
//                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "move_action@generic@core", "walk", 8.0f, -8.0f, -1, 1, 0, false, false, false);
//                }
//                IsMovingForward = true;
//            }
//        }
//        else
//        {
//            if (IsBackingUp)
//            {
//                float AnimTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, "anim@gangops@hostage@", "perp_idle");
//                NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
//                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "anim@gangops@hostage@", "perp_idle", 8.0f, -8.0f, -1, 1 | 16 | 32, 0, false, false, false);
//                NativeFunction.Natives.SET_ENTITY_ANIM_CURRENT_TIME(Player.Character, "anim@gangops@hostage@", "perp_idle", AnimTime);
//                if (Ped.Pedestrian.Exists())
//                {
//                    NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
//                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "anim@gangops@hostage@", "victim_idle", 8.0f, -8.0f, -1, 1 | 16 | 32, 0, false, false, false);
//                    NativeFunction.Natives.SET_ENTITY_ANIM_CURRENT_TIME(Ped.Pedestrian, "anim@gangops@hostage@", "victim_idle", AnimTime);

//                }
//                IsBackingUp = false;
//            }
//            if (IsMovingForward)
//            {
//                if (Ped.Pedestrian.Exists())
//                {
//                    NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
//                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "anim@gangops@hostage@", "victim_idle", 8.0f, -8.0f, -1, 1 | 16 | 32, 0, false, false, false);

//                }
//                IsMovingForward = false;
//            }
//        }
//    }
//    private void HeadingLoop()
//    {
//        bool isLeftPressed = false;
//        bool isRightPressed = false;

//        if (Game.IsControlPressed(2, GameControl.MoveRightOnly) || Game.IsControlPressed(2, GameControl.MoveRight))
//        {
//            isRightPressed = true;
//        }
//        if (Game.IsControlPressed(2, GameControl.MoveLeftOnly) || Game.IsControlPressed(2, GameControl.MoveLeft))
//        {
//            isLeftPressed = true;
//        }
//        if (isRightPressed)
//        {
//            Player.Character.Heading -= 0.7f;
//        }
//        else if (isLeftPressed)
//        {
//            Player.Character.Heading += 0.7f;
//        }
//    }
//    private void ReleaseHostage()
//    {
//        if (Ped.Pedestrian.Exists())
//        {
//            //Ped.Pedestrian.Detach();
//            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
//            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Ped.Pedestrian);
//            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "anim@gangops@hostage@", "perp_fail", 8.0f, -8.0f, -1, 0, 0, false, false, false);//-1
//            GameFiber.Sleep(500);
//            if (Ped.Pedestrian.Exists())
//            {
//                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "anim@gangops@hostage@", "victim_success", 8.0f, -8.0f, -1, 0, 0, false, false, false);//-1

//                uint GameTimeReleased = Game.GameTime;
//                float AnimationTime = 0f;
//                while (AnimationTime < 0.5f && Game.GameTime - GameTimeReleased <= 3000)
//                {
//                    AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, "anim@gangops@hostage@", "perp_fail");
//                    GameFiber.Yield();
//                }
//            }
//        }
//    }
//    private void ExecuteHostageNew()
//    {
//        if (Ped.Pedestrian.Exists())
//        {
//            Ped.Pedestrian.Detach();
//            Vector3 HeadCoordinated = Ped.Pedestrian.GetBonePosition(PedBoneId.Head);
//            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
//            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Ped.Pedestrian);
//            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "anim@gangops@hostage@", "perp_fail", 8.0f, -8.0f, -1, 0, 0, false, false, false);//-1
//            if (Ped.Pedestrian.Exists())
//            {
//                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "anim@gangops@hostage@", "victim_fail", 8.0f, -8.0f, -1, 0, 0, false, false, false);//-1
//            }

//            GameFiber.Wait(250);
//            if (Ped.Pedestrian.Exists())
//            {
//                NativeFunction.CallByName<bool>("SET_PED_SHOOTS_AT_COORD", Player.Character, HeadCoordinated.X, HeadCoordinated.Y, HeadCoordinated.Z, true);
//                //uint GameTimeReleased = Game.GameTime;
//                //float AnimationTime = 0f;
//                //while (AnimationTime < 0.5f && Game.GameTime - GameTimeReleased <= 3000)
//                //{
//                //    AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, "anim@gangops@hostage@", "perp_fail");
//                //    GameFiber.Yield();
//                //}
//                if (Ped.Pedestrian.Exists())
//                {
//                    Ped.Pedestrian.Kill();
//                }
//            }

//        }
//    }
//    private void ExecuteHostage()
//    {


//        if (Ped.Pedestrian.Exists())
//        {
//            Ped.Pedestrian.Detach();
//            Vector3 HeadCoordinated = Ped.Pedestrian.GetBonePosition(PedBoneId.Head);
//            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
//            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Ped.Pedestrian);



//            //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "anim@gangops@hostage@", "victim_fail", 8.0f, -8.0f, -1, 0, 0, false, false, false);//-1
//            // GameFiber.Sleep(500);
//            NativeFunction.CallByName<bool>("SET_PED_SHOOTS_AT_COORD", Player.Character, HeadCoordinated.X, HeadCoordinated.Y, HeadCoordinated.Z, true);
//            if (Ped.Pedestrian.Exists())
//            {
//                Ped.Pedestrian.Kill();
//                //uint GameTimeReleased = Game.GameTime;
//                //float AnimationTime = 0f;


//                //GameFiber.Sleep(500);
//                //NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
//                //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "anim@gangops@hostage@", "perp_fail", 8.0f, -8.0f, -1, 0, 0, false, false, false);//-1

//                //while (AnimationTime < 0.5f && Game.GameTime - GameTimeReleased <= 3000)
//                //{
//                //    AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, "anim@gangops@hostage@", "perp_fail");
//                //    GameFiber.Yield();
//                //}
//            }
//        }
//    }
//    private void ResetHostage()
//    {
//        if (Ped.Pedestrian.Exists())
//        {
//            //Ped.Pedestrian.Detach();
//            Ped.Pedestrian.BlockPermanentEvents = false;
//            Ped.Pedestrian.KeepTasks = false;
//            Ped.Pedestrian.IsPersistent = wasPersist;
//            NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
//            Ped.Pedestrian.RelationshipGroup = PreviousRelationshipGroup;
//            Ped.WasEverSetPersistent = wasSetPersistent;
//            Ped.CanBeAmbientTasked = true;
//            Ped.CanBeTasked = true;
//            Ped.IsBeingHeldAsHostage = false;
//            NativeFunction.Natives.SET_ENABLE_HANDCUFFS(Ped.Pedestrian, false);
//            //if(Ped.Pedestrian.RelationshipGroup.Name == "HOSTAGE")
//            //{
//            //    Ped.Pedestrian.RelationshipGroup = new RelationshipGroup("CITIZEN");
//            //}
//            //EntryPoint.WriteToConsole($"PreviousRelationshipGroup {PreviousRelationshipGroup.Name} Ped.Pedestrian.RelationshipGroup {Ped.Pedestrian.RelationshipGroup.Name} 1");
//        }
//    }
//    private void ResetPlayer()
//    {
//        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
//        Player.ActivityManager.IsHoldingHostage = false;
//        Player.ActivityManager.IsPerformingActivity = false;
//        Game.LocalPlayer.Character.RelationshipGroup.SetRelationshipWith(HostageRG, Relationship.Neutral);
//        HostageRG.SetRelationshipWith(Game.LocalPlayer.Character.RelationshipGroup, Relationship.Neutral);

//        Game.DisableControlAction(0, GameControl.Sprint, false);
//        Game.DisableControlAction(0, GameControl.Jump, false);
//    }
//    private void ResetVariables()
//    {
//        Player.ButtonPrompts.RemovePrompts("Hostage");
//    }
//    private void DisableControls()
//    {
//        if (!Player.Character.IsAiming)
//        {
//            Game.DisableControlAction(0, GameControl.Attack, true);// false);
//            Game.DisableControlAction(0, GameControl.Attack2, true);// false);

//            Game.DisableControlAction(0, GameControl.VehicleAttack, true);// false);
//            Game.DisableControlAction(0, GameControl.VehicleAttack2, true);// false);
//        }
//    }
//}