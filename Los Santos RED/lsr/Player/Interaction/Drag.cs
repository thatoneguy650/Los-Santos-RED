using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;



//Class is mostly based off Icemallow see https://github.com/Icemallow/Icemallow-bodydrag
public class Drag : DynamicActivity
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
    private bool IsAttached;
    private bool PedCanBeTasked;
    private bool PedCanBeAmbientTasked;
    private bool PedWasPersistent;
    private IEntityProvideable World;
    private bool PedWasDead;
    private VehicleExt ClosestVehicle;
    private Vector3 TrunkPosition;
    private bool LoadBody;
    private bool isBackingUp;
    private uint GameTimeLastCheckedVehicle;
    private bool IsNearBody;
    private bool IsBodyPickedUp;
    private Rage.Object leftHandObject;
    private bool IsRagdoll = false;
    private AnimationWatcher AnimationWatcher;
    private bool CloseTrunk = false;
    private VehicleDoorSeatData VehicleDoorSeatData;
    private IVehicleSeatAndDoorLookup VehicleSeatDoorData;

    public Drag(IInteractionable player, PedExt ped, ISettingsProvideable settings, ICrimes crimes, IModItems modItems, IEntityProvideable world, IVehicleSeatAndDoorLookup vehicleSeatDoorData)
    {
        Player = player;
        Ped = ped;
        Settings = settings;
        Crimes = crimes;
        ModItems = modItems;
        World = world;
        VehicleSeatDoorData = vehicleSeatDoorData;
    }
    public override string DebugString => $"TimesInsultedByPlayer {Ped.TimesInsultedByPlayer} FedUp {Ped.IsFedUpWithPlayer}";
    public override ModItem ModItem { get; set; }
    public override bool CanPause { get; set; } = false;
    public override bool CanCancel { get; set; } = false;
    public override bool IsUpperBodyOnly { get; set; } = true;
    public override string PausePrompt { get; set; } = "Pause Activity";
    public override string CancelPrompt { get; set; } = "Stop Activity";
    public override string ContinuePrompt { get; set; } = "Continue Activity";
    public override void Continue()
    {

    }
    public override void Cancel()
    {
        if (PedCanBeTasked)
        {
            Ped.CanBeTasked = true;
        }
        if (PedCanBeAmbientTasked)
        {
            Ped.CanBeAmbientTasked = true;
        }
        //if(PedWasPersistent && Ped.Pedestrian.Exists())
        //{
        //    Ped.Pedestrian.IsPersistent = true;
        //}
        //if (!LoadBody)
        //{
        //    DetachPeds();
        //}
        if(IsAttached)
        {
            DetachPeds();
        }
        if(leftHandObject.Exists())
        {
            leftHandObject.Delete();
        }
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        //if(Ped.WasKilledByPlayer || Ped.HasBeenHurtByPlayer)
        //{

        //}
        Player.ActivityManager.IsDraggingBody = false;
        Player.ActivityManager.IsPerformingActivity = false;
    }
    public override void Pause()
    {
        Cancel();
    }
    public override bool IsPaused() => false;
    public override void Start()
    {
        if (!Ped.Pedestrian.Exists())
        {
                return;
        }
        //EntryPoint.WriteToConsoleTestLong($"Drag Started Money: {Ped.Money} Dead: {Ped.IsDead} Unconsc: {Ped.IsUnconscious}");
        Player.ActivityManager.IsDraggingBody = true;
        SetupPed();
        GameFiber.StartNew(delegate
        {
            try
            {
                BeginDrag();
                if(LoadBody)
                {
                    ClosestVehicle.OpenDoorLoose(VehicleDoorSeatData.DoorID, true);
                    if(Settings.SettingsManager.DragSettings.FadeOut && !Game.IsScreenFadedOut)
                    {
                        Game.FadeScreenOut(500, true);
                    }
                }
                Cancel();
                if (LoadBody)
                {
                    if(ClosestVehicle.VehicleBodyManager.LoadBody(Ped, VehicleDoorSeatData, true, World))
                    {
                        Player.ActivityManager.SetDoor(VehicleDoorSeatData.DoorID, false, false, ClosestVehicle);
                    }
                    else if (Game.IsScreenFadedOut)
                    {
                        Game.FadeScreenIn(500, true);
                    }
                }
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "Drag");
    }
    public override bool CanPerform(IActionable player)
    {
        if (!player.ActivityManager.CanDragLookedAtPed)
        {
            Game.DisplayHelp("Cannot drag ped");
            return false;
        }
        if (player.IsOnFoot && player.ActivityManager.CanPerformActivitiesExtended)
        {
            return true;
        }
        Game.DisplayHelp($"Cannot Drag");
        return false;
    }
    private void SetupPed()
    {
        PedCanBeTasked = Ped.CanBeTasked;
        PedCanBeAmbientTasked = Ped.CanBeAmbientTasked;
       // PedWasPersistent = Ped.Pedestrian.IsPersistent;
        NativeFunction.Natives.SET_PED_SHOULD_PLAY_IMMEDIATE_SCENARIO_EXIT(Ped.Pedestrian);
        Ped.CanBeTasked = false;
        Ped.CanBeAmbientTasked = false;
      //  Ped.Pedestrian.IsPersistent = false;
        Ped.Pedestrian.BlockPermanentEvents = true;
        Ped.Pedestrian.KeepTasks = true;
        LoadBody = false;
    }
    private void BeginDrag()
    {
        AnimationDictionary.RequestAnimationDictionay("combat@drag_ped@");
        //EntryPoint.WriteToConsoleTestLong("Begin Dragging Body");
        AnimationWatcher = new AnimationWatcher();
        AnimationWatcher.TimeBetweenCheck = 0;
        IsNearBody = MoveToBody();
        if(!IsNearBody)
        {
            return;
        }
        IsBodyPickedUp = PickupAndAttachPeds();
        RemovePrompts();
        if (IsBodyPickedUp && !LoadBody)
        {
            //EntryPoint.WriteToConsoleTestLong("Drag, Finished, completed");
            PutDownPed();
        }
        else if (IsAttached && !LoadBody)
        {
            //EntryPoint.WriteToConsoleTestLong("Drag, Finished, only attached");
            DetachPeds();
        }
        if (LastWeapon != null && !LoadBody)
        {
            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)LastWeapon.Hash, true);
        }    
    }
    private void PutDownPed()
    {
        //EntryPoint.WriteToConsoleTestLong("DRAG PUT DOWN PED RAN");
        if (Ped.Pedestrian.Exists() && Settings.SettingsManager.ActivitySettings.PlayDraggingPedAnimation)
        {
            NativeFunction.Natives.TASK_PLAY_ANIM(Ped.Pedestrian, "combat@drag_ped@", "injured_putdown_ped", 2.0f, -2.0f, -1, 2 | 8, 0, false, false, false);
        }
        PlayPlayerLoopingAnimation("combat@drag_ped@", "injured_putdown_plyr", false, 2, true, false);
        DetachPeds();
    }
    private void RemovePrompts()
    {
        Player.ButtonPrompts.RemovePrompts("Drop");
        Player.ButtonPrompts.RemovePrompts("Load");
        Player.ButtonPrompts.RemovePrompts("Ragdoll");
    }
    private bool MoveToBody()
    {
        pedHeadshotHandle = NativeFunction.Natives.RegisterPedheadshot<uint>(Ped.Pedestrian);
        Vector3 DesiredPosition = NativeFunction.CallByName<Vector3>("GET_WORLD_POSITION_OF_ENTITY_BONE", Ped.Pedestrian, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Ped.Pedestrian, 0));
        float DesiredHeading = Game.LocalPlayer.Character.Heading;
        NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", Player.Character, Ped.Pedestrian, -1, 1.75f, 0.75f, 1073741824, 1); //Original and works ok
        uint GameTimeStartedMovingToBody = Game.GameTime;
        float heading = Game.LocalPlayer.Character.Heading;
        bool IsFacingDirection = true;
        bool IsCloseEnough = false;
        while (Game.GameTime - GameTimeStartedMovingToBody <= 5000 && !IsCloseEnough && !IsCancelled)
        {
            if (Player.IsMoveControlPressed)
            {
                IsCancelled = true;
            }
            if (!Ped.Pedestrian.Exists() || !Player.IsAliveAndFree || Player.IsIncapacitated)
            {
                IsCancelled = true;
                break;
            }
            IsCloseEnough = Game.LocalPlayer.Character.DistanceTo2D(Ped.Pedestrian) <= 1.85f;
            GameFiber.Yield();
        }
        if (Ped.Pedestrian.Exists())
        {
            Vector3 PedRoot = NativeFunction.CallByName<Vector3>("GET_WORLD_POSITION_OF_ENTITY_BONE", Ped.Pedestrian, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Ped.Pedestrian, 0));
            float calcHeading = (float)GetHeading(Player.Character.Position, PedRoot);
            float calcHeading2 = (float)CalculeAngle(PedRoot, Player.Character.Position);
            DesiredHeading = calcHeading2;
            //EntryPoint.WriteToConsole($"calcHeading 1 {calcHeading} calcHeading2  {calcHeading2}");
            NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", Player.Character, Ped.Pedestrian, 1000);
            //EntryPoint.WriteToConsole($"calcHeading 2 {calcHeading} calcHeading2 {calcHeading2}");
            GameFiber.Sleep(1000);
            if (!Ped.Pedestrian.Exists() || !Player.IsAliveAndFree || Player.IsIncapacitated)
            {
                IsCancelled = true;
            }
            if (IsCloseEnough && IsFacingDirection && !IsCancelled)
            {
                //EntryPoint.WriteToConsole($"MoveToBody IN POSITION {Game.LocalPlayer.Character.DistanceTo(DesiredPosition)} {Extensions.GetHeadingDifference(heading, DesiredHeading)} {heading} {DesiredHeading}");
                return true;
            }
            else
            {
                NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                //EntryPoint.WriteToConsole($"MoveToBody NOT IN POSITION EXIT {Game.LocalPlayer.Character.DistanceTo(DesiredPosition)} {Extensions.GetHeadingDifference(heading, DesiredHeading)} {heading} {DesiredHeading}");
                return false;
            }
        }
        return false;
    }
    private void AttachPeds()
    {
        if (!Ped.Pedestrian.Exists())
        {
            return;
        }
        if (Ped.IsDead)
        {
           Ped.CurrentHealthState.ResurrectPed();
            Ped.Pedestrian.BlockPermanentEvents = true;
            Ped.Pedestrian.KeepTasks = true;
            //GameFiber.Sleep(100);
            if (!Ped.Pedestrian.Exists())
            {
                return;
            }
            NativeFunction.Natives.SET_PED_SHOULD_PLAY_IMMEDIATE_SCENARIO_EXIT(Ped.Pedestrian);
        }

            NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
            NativeFunction.Natives.CLEAR_PED_TASKS_IMMEDIATELY(Ped.Pedestrian);
        
        NativeFunction.Natives.SET_ENTITY_COLLISION(Ped.Pedestrian, true, true);
        Ped.Pedestrian.BlockPermanentEvents = true;
        Ped.Pedestrian.KeepTasks = true;
        IsAttached = true;
        NativeFunction.Natives.ATTACH_ENTITY_TO_ENTITY(Ped.Pedestrian, Player.Character, 11816, 0f, 0.6f, 0f, 0f, 0f, 0f, false, false, false, false, 2, false); 
    }
    private void DetachPeds()
    {
        if (Ped.Pedestrian.Exists())
        {
            if(Ped.IsDead)
            {
                Ped.Pedestrian.Kill();
            }
            Ped.Pedestrian.Detach();
            NativeFunction.Natives.SET_ENTITY_COLLISION(Ped.Pedestrian, true, true);
        }
        IsAttached = false;
    }
    private double GetHeading(Vector3 a, Vector3 b)
    {
        double x = b.X - a.X;
        double y = b.Y - a.Y;
        return 270 - Math.Atan2(y, x) * (180 / Math.PI);
    }
    private double CalculeAngle(Vector3 start, Vector3 arrival)
    {
        var deltaX = Math.Pow((arrival.X - start.X), 2);
        var deltaY = Math.Pow((arrival.Y - start.Y), 2);

        var radian = Math.Atan2((arrival.Y - start.Y), (arrival.X - start.X));
        var angle = (radian * (180 / Math.PI) + 360) % 360;

        return angle;
    }
    private bool PickupAndAttachPeds()
    {  
        if (Player.WeaponEquipment.CurrentWeapon != null)
        {
            LastWeapon = Player.WeaponEquipment.CurrentWeapon;
        }
        else
        {
            LastWeapon = null;
        }
        Player.WeaponEquipment.SetUnarmed();
        Player.ButtonPrompts.RemovePrompts("Drop");
        Player.ButtonPrompts.RemovePrompts("Ragdoll");
        if(PlayAttachAnimation())
        {
            if (!Player.ButtonPrompts.HasPrompt("Drop"))
            {
                Player.ButtonPrompts.AddPrompt("Drop", "Drop", "Drop", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 1);
            }
            if (Settings.SettingsManager.DragSettings.AllowRagdolling && !Player.ButtonPrompts.HasPrompt("Ragdoll"))
            {
                Player.ButtonPrompts.AddPrompt("Ragdoll", "Ragdoll", "Ragdoll", Settings.SettingsManager.KeySettings.InteractCancel, 10);
            }
            if(PlayDragAnimation())
            {
                return true;
            }
        }
        return false;     
    }
    private bool PlayAttachAnimation()
    {
        AttachPeds();
        GameFiber.Yield();

        //GameFiber.Sleep(500);

        if (Ped.Pedestrian.Exists() && Settings.SettingsManager.ActivitySettings.PlayDraggingPedAnimation)
        {
            EntryPoint.WriteToConsole("PLAYING DRAGGING ANIM START FOR PED");
            NativeFunction.Natives.TASK_PLAY_ANIM(Ped.Pedestrian, "combat@drag_ped@", "injured_pickup_back_ped", 2.0f, -2.0f, -1, (int)(eAnimationFlags.AF_HOLD_LAST_FRAME |  eAnimationFlags.AF_NOT_INTERRUPTABLE | eAnimationFlags.AF_FORCE_START), 0, false, false, false);
        }
        if (PlayPlayerLoopingAnimation("combat@drag_ped@", "injured_pickup_back_plyr", false, 2, false, true))
        {
            if (!Settings.SettingsManager.ActivitySettings.PlayDraggingPedAnimation)
            {
                DoRagdollDrag();
            }
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool PlayDragAnimation()
    {
        AnimationDictionary.RequestAnimationDictionay("combat@drag_ped@");
        if (Ped.Pedestrian.Exists() && !IsRagdoll)
        {
            NativeFunction.Natives.ATTACH_ENTITY_TO_ENTITY(Ped.Pedestrian, Player.Character, 11816, 0f, 0.5f, 0f, 0f, 0f, 0f, false, false, false, false, 2, false);
        }
        if (PlayPlayerLoopingAnimation("combat@drag_ped@", "injured_drag_plyr", true, 1, false, false))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool PlayPlayerLoopingAnimation(string dictionary, string animation, bool repeat, int flag, bool moveCancels, bool forcePedIntroAnim)
    {
        //EntryPoint.WriteToConsoleTestLong($"PlayPlayerLoopingAnimation START {animation} repeat {repeat}");
        if (!repeat)
        {
            NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, dictionary, animation, 2.0f, -2.0f, -1, flag, 0, false, false, false);
        }
        uint GameTimeStartedAnimation = Game.GameTime;
        float AnimationTime = 0.0f;
        AnimationWatcher.Reset();
        GameTimeLastCheckedVehicle = 0;
        while (AnimationTime < 1.0f && !IsCancelled && (repeat||  Game.GameTime - GameTimeStartedAnimation <= 10000))
        {
            AnimationTime = NativeFunction.Natives.GET_ENTITY_ANIM_CURRENT_TIME<float>(Player.Character, dictionary, animation);
            if (moveCancels && Player.IsMoveControlPressed)
            {
                IsCancelled = true;
            }
            if (!Ped.Pedestrian.Exists() || !Player.IsAliveAndFree || Player.IsIncapacitated)
            {
                IsCancelled = true;
                break;
            }
            if(repeat)
            {
                HeadingLoop();
                DirectionLoop();
                LoadingLoop();
                if (Player.ButtonPrompts.IsPressed("Drop"))//demand cash?
                {
                    RemovePrompts();
                    return true;
                }
                else if (ClosestVehicle != null && Player.ButtonPrompts.IsGroupPressed("Load"))//demand cash?
                {
                    RemovePrompts();
                    LoadBody = true;
                    return true;
                }
                else if(Player.ButtonPrompts.IsPressed("Ragdoll"))
                {
                    if (Ped.Pedestrian.Exists())
                    {
                        DoRagdollDrag();
                        Game.DisplaySubtitle("RAGDOLLED");
                    }
                }
            }
            else
            {
                if(forcePedIntroAnim && Ped.Pedestrian.Exists())
                {
                    float introPedAnimTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Ped.Pedestrian, "combat@drag_ped@", "injured_pickup_back_ped");
                    //EntryPoint.WriteToConsole($"CURRENT INTRO AnimTime {introPedAnimTime} Player Anim Time {AnimationTime}");
                    if (!AnimationWatcher.IsAnimationRunning(introPedAnimTime) || introPedAnimTime == 0.0f)
                    {

                        //NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
                        //NativeFunction.Natives.CLEAR_PED_TASKS_IMMEDIATELY(Ped.Pedestrian);

                        NativeFunction.Natives.TASK_PLAY_ANIM(Ped.Pedestrian, "combat@drag_ped@", "injured_pickup_back_ped", 2.0f, -2.0f, -1, (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_NOT_INTERRUPTABLE), 0, false, false, false);
                       NativeFunction.Natives.SET_ENTITY_ANIM_CURRENT_TIME(Ped.Pedestrian, "combat@drag_ped@", "injured_pickup_back_ped", AnimationTime);
                        EntryPoint.WriteToConsole("FORCING AS NO ANIM IS PLAYING");
                    }
                }


                //if(Ped.Pedestrian.Exists() && Game.IsKeyDownRightNow(Keys.N))
                //{
                //    Ped.CanBeAmbientTasked = false;
                //    Ped.CanBeTasked = false;
                //    NativeFunction.Natives.TASK_PLAY_ANIM(Ped.Pedestrian, "combat@drag_ped@", "injured_pickup_back_ped", 2.0f, -2.0f, -1, (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_NOT_INTERRUPTABLE), 0, false, false, false);
                //    Game.DisplaySubtitle("STARTED");
                //}

                //if (Ped.Pedestrian.Exists())
                //{
                //    float ANimTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Ped.Pedestrian, "combat@drag_ped@", "injured_pickup_back_ped");
                //    EntryPoint.WriteToConsole($"ANimTime {ANimTime}");
                //}
            }



            //if (Ped.Pedestrian.Exists() && Game.IsKeyDownRightNow(Keys.N))
            //{
            //    Ped.CanBeAmbientTasked = false;
            //    Ped.CanBeTasked = false;
            //    NativeFunction.Natives.TASK_PLAY_ANIM(Ped.Pedestrian, "combat@drag_ped@", "injured_pickup_back_ped", 2.0f, -2.0f, -1, (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_NOT_INTERRUPTABLE), 0, false, false, false);
            //    Game.DisplaySubtitle("STARTED");
            //}


            GameFiber.Yield();
        }
        //EntryPoint.WriteToConsoleTestLong($"PlayPlayerLoopingAnimation END {animation} repeat {repeat}");
        if(!IsCancelled && IsRagdoll)
        {
            return true;
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
    private void DirectionLoop()
    {
        if(LoadBody)
        {
            return;
        }
        if (Game.IsControlPressed(2, GameControl.MoveDownOnly) || Game.IsControlPressed(2, GameControl.MoveDown))
        {
            //EntryPoint.WriteToConsole("DRAG DOWN PRESSED");
            if (!isBackingUp)
            {
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "combat@drag_ped@", "injured_drag_plyr", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                if (Ped.Pedestrian.Exists() && Settings.SettingsManager.ActivitySettings.PlayDraggingPedAnimation && !IsRagdoll)
                {
                    NativeFunction.Natives.TASK_PLAY_ANIM(Ped.Pedestrian, "combat@drag_ped@", "injured_drag_ped", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                }
                isBackingUp = true;
            }
        }
        else
        {
            if (isBackingUp)
            {
                float AnimationTime = 1.0f;// NativeFunction.Natives.GET_ENTITY_ANIM_CURRENT_TIME<float>(Player.Character, "combat@drag_ped@", "injured_drag_plyr");
                if (AnimationTime >= 0.95f)
                {
                    NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "combat@drag_ped@", "injured_putdown_plyr", 8.0f, -8.0f, -1, 2, 0, true, true, true);
                    if (Ped.Pedestrian.Exists() && Settings.SettingsManager.ActivitySettings.PlayDraggingPedAnimation && !IsRagdoll)
                    {
                        //NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
                        NativeFunction.Natives.TASK_PLAY_ANIM(Ped.Pedestrian, "combat@drag_ped@", "injured_putdown_ped", 2.0f, -2.0f, -1, (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_NOT_INTERRUPTABLE | eAnimationFlags.AF_FORCE_START), 0, false, false, false);
                    }
                    isBackingUp = false;
                }
            }
            else if(!isBackingUp)
            {
                NativeFunction.Natives.SET_ENTITY_ANIM_CURRENT_TIME(Player.Character, "combat@drag_ped@", "injured_putdown_plyr", 0.0f);
                if (Ped.Pedestrian.Exists() && Settings.SettingsManager.ActivitySettings.PlayDraggingPedAnimation && !IsRagdoll)
                {
                    NativeFunction.Natives.SET_ENTITY_ANIM_CURRENT_TIME(Ped.Pedestrian, "combat@drag_ped@", "injured_putdown_ped", 0.0f);
                }
            }
        }
    }
    private void HeadingLoop()
    {
        if (Game.IsControlPressed(2, GameControl.MoveRightOnly) || Game.IsControlPressed(2, GameControl.MoveRight))
        {
            Player.Character.Heading -= 0.7f;
        }
        else if (Game.IsControlPressed(2, GameControl.MoveLeftOnly) || Game.IsControlPressed(2, GameControl.MoveLeft))
        {
            Player.Character.Heading += 0.7f;
        }
    }
    private void LoadingLoop()
    {
        if (!(GameTimeLastCheckedVehicle == 0 || Game.GameTime - GameTimeLastCheckedVehicle >= 500))
        {
            return;
        }
        GameTimeLastCheckedVehicle = Game.GameTime;
        ClosestVehicle = World.Vehicles.GetClosestVehicleExt(Player.Character.Position, true, 5f);
        if(ClosestVehicle == null || !ClosestVehicle.Vehicle.Exists())
        {
            VehicleDoorSeatData = null;
            Player.ButtonPrompts.RemovePrompts("Load");
            return;
        }
        VehicleDoorSeatData = ClosestVehicle.GetClosestPedStorageBone(Player, 2.5f, VehicleSeatDoorData);
        if(VehicleDoorSeatData == null)
        {
            Player.ButtonPrompts.RemovePrompts("Load");
            return;
        }
        Player.ButtonPrompts.RemovePrompts("Load");
        if (Settings.SettingsManager.PedLoadingSettings.AllowLoadingBodies && !Player.ButtonPrompts.HasPrompt($"Load into {VehicleDoorSeatData.SeatName}"))
        {
            Player.ButtonPrompts.AddPrompt("Load", $"Load into {VehicleDoorSeatData.SeatName}", $"Load into {VehicleDoorSeatData.SeatName}", Settings.SettingsManager.KeySettings.InteractNegativeOrNo, 1);
        }
    }
    private void DoRagdollDrag()
    {
        if (!Ped.Pedestrian.Exists())
        {
            return;
        }
        Ped.Pedestrian.Detach();
        Ped.Pedestrian.Resurrect();
        NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
        Ped.Pedestrian.BlockPermanentEvents = true;
        Ped.Pedestrian.IsRagdoll = true;
        IsRagdoll = true;

        if (!leftHandObject.Exists())
        {
            leftHandObject = new Rage.Object("ng_proc_cigarette01a", Game.LocalPlayer.Character.GetOffsetPositionFront(2f).Around2D(2f));
        }
        if (leftHandObject.Exists() && Ped.Pedestrian.Exists())
        {
            if (Settings.SettingsManager.DragSettings.RagdollSetPedsInvisible)
            {
                Ped.Pedestrian.IsVisible = false;
                Player.Character.IsVisible = false;
            }
            else
            {
                Ped.Pedestrian.IsVisible = true;
                Player.Character.IsVisible = true;
            }
            leftHandObject.IsVisible = false;
            if (Settings.SettingsManager.DragSettings.RagdollSetNoCollision)
            {
                NativeFunction.Natives.SET_ENTITY_NO_COLLISION_ENTITY(Ped.Pedestrian, Player.Character, false);
            }
            NativeFunction.Natives.SET_PED_TO_RAGDOLL(Ped.Pedestrian, -1, -1, 0, false, false, false);
            if (Settings.SettingsManager.DragSettings.RagdollRunAttach)
            {

                leftHandObject.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, Settings.SettingsManager.DragSettings.RagdollItemAttachBone), Vector3.Zero, Rotator.Zero);
                NativeFunction.Natives.ATTACH_ENTITY_TO_ENTITY_PHYSICALLY(Ped.Pedestrian, leftHandObject,
                    NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Ped.Pedestrian, Settings.SettingsManager.DragSettings.RagdollPhysicalAttachBone1), //bone 1
                    0,//NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Ped.Pedestrian, Settings.SettingsManager.DebugSettings.Drag_PhysicalAttachBone2),// bone 2
                            Settings.SettingsManager.DragSettings.RagdollAttach1X, Settings.SettingsManager.DragSettings.RagdollAttach1Y, Settings.SettingsManager.DragSettings.RagdollAttach1Z,
                            Settings.SettingsManager.DragSettings.RagdollAttach2X, Settings.SettingsManager.DragSettings.RagdollAttach2Y, Settings.SettingsManager.DragSettings.RagdollAttach2Z,
                            Settings.SettingsManager.DragSettings.RagdollAttach3X, Settings.SettingsManager.DragSettings.RagdollAttach3Y, Settings.SettingsManager.DragSettings.RagdollAttach3Z,
                    100000.0f,//break force
                    Settings.SettingsManager.DragSettings.RagdollFixedRotation,//true, //fixed rotation
                    Settings.SettingsManager.DragSettings.RagdollDoInitialWarp,//true, //DoInitialWarp
                     Settings.SettingsManager.DragSettings.RagdollCollision,//false, //collision
                           Settings.SettingsManager.DragSettings.RagdollTeleport,//false, //teleport
                        Settings.SettingsManager.DragSettings.RagdollRotationOrder                                                                                                                             //2 //RotationORder
                    );
            }
            //"BONETAG_SPINE3"
            //"BONETAG_PELVIS"
            //0.1f,0.3f,-0.1f,
            //0f,0f,0f,
            //180f,90f,0f,
        }
    }
}