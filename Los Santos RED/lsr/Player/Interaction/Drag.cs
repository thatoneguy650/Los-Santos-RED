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
    private int GameTimeLastCheckedVehicle;
    private bool IsNearBody;
    private bool IsBodyPickedUp;
    private Rage.Object leftHandObject;
    private bool IsRagdoll = false;
    private AnimationWatcher AnimationWatcher;
    private bool CloseTrunk = false;

    public Drag(IInteractionable player, PedExt ped, ISettingsProvideable settings, ICrimes crimes, IModItems modItems, IEntityProvideable world)
    {
        Player = player;
        Ped = ped;
        Settings = settings;
        Crimes = crimes;
        ModItems = modItems;
        World = world;
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
        if (!LoadBody)
        {
            DetachPeds();
        }
        if(leftHandObject.Exists())
        {
            leftHandObject.Delete();
        }
        ////DEBUG STUFFF
        if (Ped.Pedestrian.Exists())
        {
            //Ped.Pedestrian.IsVisible = true;
            //if (!LoadBody)
            //{
            //    if (!PedWasPersistent)
            //    {
            //        Ped.Pedestrian.IsPersistent = false;
            //    }
            //}
        }
        Player.Character.IsVisible = true;
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        Player.ActivityManager.IsDraggingBody = false;
    }
    public override void Pause()
    {
        Cancel();
    }
    public override bool IsPaused() => false;
    public override void Start()
    {
        if (Ped.Pedestrian.Exists())
        {
            EntryPoint.WriteToConsole($"Drag Started Money: {Ped.Money} Dead: {Ped.IsDead} Unconsc: {Ped.IsUnconscious}");
            Player.ActivityManager.IsDraggingBody = true;
            PedCanBeTasked = Ped.CanBeTasked;
            PedCanBeAmbientTasked = Ped.CanBeAmbientTasked;
            PedWasPersistent = Ped.Pedestrian.IsPersistent;

            Ped.CanBeTasked = false;
            Ped.CanBeAmbientTasked = false;
            Ped.Pedestrian.BlockPermanentEvents = true;
            Ped.Pedestrian.KeepTasks = true;
            LoadBody = false;
            GameFiber.StartNew(delegate
            {
                try
                {
                    BeginDrag();
                    if (LoadBody)
                    {
                        LoadBodyInCar();
                        Cancel();
                    }
                    else
                    {
                        Cancel();
                    }
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "Drag");
        }
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
    private void BeginDrag()
    {
        AnimationDictionary.RequestAnimationDictionay("combat@drag_ped@");
        EntryPoint.WriteToConsole("Begin Dragging Body");
        AnimationWatcher = new AnimationWatcher();
        AnimationWatcher.TimeBetweenCheck = 50;
        IsNearBody = MoveToBody();
        if (IsNearBody)
        {
            IsBodyPickedUp = PickupAndAttachPeds();
            RemovePrompts();
            if (IsBodyPickedUp)
            {
                EntryPoint.WriteToConsole("Drag, Finished, completed");
                PutDownPed();
            }
            else if (IsAttached)
            {
                EntryPoint.WriteToConsole("Drag, Finished, only attached");
                DetachPeds();
            }
            if (LastWeapon != null)
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)LastWeapon.Hash, true);
            }
        }
    }
    private void PutDownPed()
    {    
        if (Ped.Pedestrian.Exists() && Settings.SettingsManager.ActivitySettings.PlayDraggingPedAnimation)
        {
            NativeFunction.Natives.TASK_PLAY_ANIM(Ped.Pedestrian, "combat@drag_ped@", "injured_putdown_ped", 2.0f, -2.0f, -1, 2, 0, false, false, false);
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
            EntryPoint.WriteToConsole($"calcHeading 1 {calcHeading} calcHeading2  {calcHeading2}", 5);
            NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", Player.Character, Ped.Pedestrian, 1000);
            EntryPoint.WriteToConsole($"calcHeading 2 {calcHeading} calcHeading2 {calcHeading2}", 5);
            GameFiber.Sleep(1000);
            if (!Ped.Pedestrian.Exists() || !Player.IsAliveAndFree || Player.IsIncapacitated)
            {
                IsCancelled = true;
            }
            if (IsCloseEnough && IsFacingDirection && !IsCancelled)
            {
                EntryPoint.WriteToConsole($"MoveToBody IN POSITION {Game.LocalPlayer.Character.DistanceTo(DesiredPosition)} {Extensions.GetHeadingDifference(heading, DesiredHeading)} {heading} {DesiredHeading}", 5);
                return true;
            }
            else
            {
                NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                EntryPoint.WriteToConsole($"MoveToBody NOT IN POSITION EXIT {Game.LocalPlayer.Character.DistanceTo(DesiredPosition)} {Extensions.GetHeadingDifference(heading, DesiredHeading)} {heading} {DesiredHeading}", 5);
                return false;
            }
        }
        return false;
    }
    private void AttachPeds()
    {
        if (Ped.Pedestrian.Exists())
        {
            if(Ped.IsDead)
            {
                Ped.Pedestrian.Health = 200;
                Ped.Pedestrian.Resurrect();
                Ped.Pedestrian.Health = 200;
                NativeFunction.Natives.RESURRECT_PED(Ped.Pedestrian);
                Ped.Pedestrian.Health = 200;
                NativeFunction.Natives.REVIVE_INJURED_PED(Ped.Pedestrian);
                Ped.Pedestrian.Health = 200;
                NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
                NativeFunction.Natives.CLEAR_PED_TASKS_IMMEDIATELY(Ped.Pedestrian);

                //NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
                //NativeFunction.Natives.RESURRECT_PED(Ped.Pedestrian);
                //NativeFunction.Natives.REVIVE_INJURED_PED(Ped.Pedestrian);
                //NativeFunction.Natives.CLEAR_PED_TASKS_IMMEDIATELY(Ped.Pedestrian);






                NativeFunction.Natives.SET_ENTITY_COLLISION(Ped.Pedestrian, true, true);
                Ped.Pedestrian.BlockPermanentEvents = true;
                Ped.Pedestrian.KeepTasks = true;
            }
            IsAttached = true;
            DoRegularAttach();// NativeFunction.Natives.ATTACH_ENTITY_TO_ENTITY(Ped.Pedestrian, Player.Character, 11816, 0f, 0.6f, 0f, 0f, 0f, 0f, false, false, false, false, 2, false);
        }
    }
    private void DoRegularAttach()
    {
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
            Ped.Pedestrian.BlockPermanentEvents = false;
            Ped.Pedestrian.KeepTasks = false;
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
        if (!Player.ButtonPrompts.HasPrompt("Drop"))
        {
            Player.ButtonPrompts.AddPrompt("Drop", "Drop", "Drop", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 1);
        }
#if DEBUG

        if (!Player.ButtonPrompts.HasPrompt("Ragdoll"))
        {
            Player.ButtonPrompts.AddPrompt("Ragdoll", "Ragdoll", "Ragdoll", Settings.SettingsManager.KeySettings.InteractCancel, 10);
        }
#endif


        if (PlayAttachAnimation() && PlayDragAnimation())
        {
            EntryPoint.WriteToConsole("ATTACH RAN");
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool PlayAttachAnimation()
    {
        AttachPeds();
        GameFiber.Yield();
        if (Ped.Pedestrian.Exists() && Settings.SettingsManager.ActivitySettings.PlayDraggingPedAnimation)
        {
            NativeFunction.Natives.TASK_PLAY_ANIM(Ped.Pedestrian, "combat@drag_ped@", "injured_pickup_back_ped", 2.0f, -2.0f, -1, 2, 0, false, false, false);
        }
        if (PlayPlayerLoopingAnimation("combat@drag_ped@", "injured_pickup_back_plyr", false, 2, false, true))
        {
            //if(Ped.Pedestrian.Exists() && !Settings.SettingsManager.ActivitySettings.PlayDraggingPedAnimation)
            //{
            //    NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Ped.Pedestrian);
            //    Ped.Pedestrian.BlockPermanentEvents = true;
            //    Ped.Pedestrian.IsRagdoll = true;
            //    NativeFunction.CallByName<bool>("SET_PED_TO_RAGDOLL", Ped.Pedestrian, 10000, 2 * 10000, 0, true, true, true);
            //    //NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
            //}
            if (1==0 && Settings.SettingsManager.DebugSettings.Draw_DoInitialRagdoll)
            {
                DoRagdollDrag();// SetRagdollTest();
            }

            EntryPoint.WriteToConsole("DRAG ANIM RAN");

            return true;
        }
        else
        {
            return false;
        }
    }

    private void SetRagdollTest()
    {
        if (!Ped.Pedestrian.Exists())
        {
            return;
        }

        Ped.Pedestrian.Detach();
        NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
        //NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Ped.Pedestrian);
        Ped.Pedestrian.BlockPermanentEvents = true;
        Ped.Pedestrian.IsRagdoll = true;
        //NativeFunction.CallByName<bool>("SET_PED_TO_RAGDOLL", Ped.Pedestrian, -1, -1, 0, false, false, false);
        //NativeFunction.Natives.SET_PED_RAGDOLL_FORCE_FALL(Ped.Pedestrian);
        //AttachPeds();
        IsRagdoll = true;


        if (Settings.SettingsManager.DebugSettings.Drag_SetNoCollision)
        {
            NativeFunction.Natives.SET_ENTITY_NO_COLLISION_ENTITY(Ped.Pedestrian, Player.Character, false);
        }
        NativeFunction.Natives.SET_PED_TO_RAGDOLL(Ped.Pedestrian, -1, -1, 0, false, false, false);

        DoRegularAttach();
        EntryPoint.WriteToConsole("TEST");
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
        EntryPoint.WriteToConsole($"PlayPlayerLoopingAnimation START {animation} repeat {repeat}");
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
                else if (ClosestVehicle != null && Player.ButtonPrompts.IsPressed("Load"))//demand cash?
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
                if(forcePedIntroAnim)
                {
                    if (Ped.Pedestrian.Exists())
                    {
                        if (!AnimationWatcher.IsAnimationRunning(NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Ped.Pedestrian, "combat@drag_ped@", "injured_pickup_back_ped")))
                        {
                            NativeFunction.Natives.TASK_PLAY_ANIM(Ped.Pedestrian, "combat@drag_ped@", "injured_pickup_back_ped", 2.0f, -2.0f, -1, 2, 0, false, false, false);
                            NativeFunction.Natives.SET_ENTITY_ANIM_CURRENT_TIME(Ped.Pedestrian, "combat@drag_ped@", "injured_pickup_back_ped", AnimationTime);
                            
                        }
                    }
                }
            }
            GameFiber.Yield();
        }

        EntryPoint.WriteToConsole($"PlayPlayerLoopingAnimation END {animation} repeat {repeat}");
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
        if (Game.IsControlPressed(2, GameControl.MoveDownOnly) || Game.IsControlPressed(2, GameControl.MoveDown))
        {
            EntryPoint.WriteToConsole("DRAG DOWN PRESSED");
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
                        NativeFunction.Natives.TASK_PLAY_ANIM(Ped.Pedestrian, "combat@drag_ped@", "injured_putdown_ped", 2.0f, -2.0f, -1, 2, 0, false, false, false);
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
        if (GameTimeLastCheckedVehicle == 0 || Game.GameTime - GameTimeLastCheckedVehicle >= 500)
        {
            ClosestVehicle = World.Vehicles.GetClosestVehicleExt(Player.Character.Position, true, 5f);
            if (ClosestVehicle != null && ClosestVehicle.Vehicle.Exists() && ClosestVehicle.Vehicle.HasBone("boot"))
            {
                TrunkPosition = ClosestVehicle.Vehicle.GetBonePosition("boot");
                if (Player.Character.DistanceTo2D(TrunkPosition) > 2.5f)
                {
                    ClosestVehicle = null;
                    TrunkPosition = Vector3.Zero;
                }
            }
            else
            {
                ClosestVehicle = null;
                TrunkPosition = Vector3.Zero;
            }
        }


        if (ClosestVehicle != null && ClosestVehicle.Vehicle.Exists())//turned off for now
        {
            if (!Player.ButtonPrompts.HasPrompt("Load"))
            {
                Player.ButtonPrompts.AddPrompt("Load", "Load", "Load", Settings.SettingsManager.KeySettings.InteractNegativeOrNo, 1);
            }
        }
        else
        {
            Player.ButtonPrompts.RemovePrompts("Load");
        }
    }

    private void LoadBodyInCar()
    {
        EntryPoint.WriteToConsole("LoadBodyInCarStarted");
        if (ClosestVehicle == null || !ClosestVehicle.Vehicle.Exists() || !Ped.Pedestrian.Exists() || !ClosestVehicle.Vehicle.Doors[5].IsValid())
        {
            return;
        }

        if (Settings.SettingsManager.DebugSettings.Drag_FadeOut)
        {
            Game.FadeScreenOut(500, true);
        }

        AnimationDictionary.RequestAnimationDictionay("timetable@floyd@cryingonbed@base");
        // Ped.Pedestrian.IsPersistent = true;
        Ped.Pedestrian.Detach();



        Ped.Pedestrian.Health = 200;
        Ped.Pedestrian.Resurrect();
        Ped.Pedestrian.Health = 200;
        NativeFunction.Natives.RESURRECT_PED(Ped.Pedestrian);
        Ped.Pedestrian.Health = 200;
        NativeFunction.Natives.REVIVE_INJURED_PED(Ped.Pedestrian);
        Ped.Pedestrian.Health = 200;
        NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
        NativeFunction.Natives.CLEAR_PED_TASKS_IMMEDIATELY(Ped.Pedestrian);
        Ped.Pedestrian.BlockPermanentEvents = true;
       // Ped.Pedestrian.IsRagdoll = true;

        GameFiber.Sleep(100);




        if (ClosestVehicle == null || !ClosestVehicle.Vehicle.Exists() || !Ped.Pedestrian.Exists())
        {
            return;
        }

        
        if (!ClosestVehicle.Vehicle.Doors[5].IsFullyOpen)
        {
            ClosestVehicle.Vehicle.Doors[5].Open(false, false);
            GameFiber.Wait(750);
        }
        if (ClosestVehicle == null || !ClosestVehicle.Vehicle.Exists() || !Ped.Pedestrian.Exists())
        {
            return;
        }
        bool hasBoot = ClosestVehicle.Vehicle.HasBone("boot");
        Vector3 BootPosition = ClosestVehicle.Vehicle.GetBonePosition("boot");
        Vector3 RootPosition = ClosestVehicle.Vehicle.GetBonePosition(0);

        float YOffset = -1 * BootPosition.DistanceTo2D(RootPosition);
        float ZOffset = BootPosition.Z - RootPosition.Z;
        NativeFunction.Natives.ATTACH_ENTITY_TO_ENTITY(Ped.Pedestrian, ClosestVehicle.Vehicle, Settings.SettingsManager.DebugSettings.Draw_BoneIndex,
            Settings.SettingsManager.DebugSettings.Draw_LoadBodyXOffset,
            Settings.SettingsManager.DebugSettings.Draw_LoadBodyYOffset + YOffset,
            Settings.SettingsManager.DebugSettings.Draw_LoadBodyZOffset + ZOffset,
            Settings.SettingsManager.DebugSettings.Draw_LoadBodyXRotation,
            Settings.SettingsManager.DebugSettings.Draw_LoadBodyYRotation,
            Settings.SettingsManager.DebugSettings.Draw_LoadBodyZRotation,
            false, false, false, Settings.SettingsManager.DebugSettings.Drag_UseBasicAttachIfPed, Settings.SettingsManager.DebugSettings.Drag_Euler, Settings.SettingsManager.DebugSettings.Drag_OffsetIsRelative, false);
        IsAttached = false;
        GameFiber.Wait(100);
        if (Ped.Pedestrian.Exists())
        {
            EntryPoint.WriteToConsole("ANIMATION RAN");
            NativeFunction.Natives.TASK_PLAY_ANIM(Ped.Pedestrian, "timetable@floyd@cryingonbed@base", "base", 8.0f, -8.0f, -1, 2, 0, false, false, false);
            NativeFunction.Natives.SET_ENTITY_ANIM_CURRENT_TIME(Ped.Pedestrian, "timetable@floyd@cryingonbed@base", "base", 1.0f);
        }
        if (Settings.SettingsManager.DebugSettings.Drag_FadeOut)
        {
            Game.FadeScreenIn(500, true);
        }
        if (ClosestVehicle == null || !ClosestVehicle.Vehicle.Exists() || !Ped.Pedestrian.Exists())
        {
            return;
        }
        EntryPoint.WriteToConsole("GOT TO CLOSE TRUNK");
        GameFiber.Wait(1000);
        if (ClosestVehicle != null && ClosestVehicle.Vehicle.Exists() && Settings.SettingsManager.DebugSettings.Drag_AutoCloseTrunk)
        {
            ClosestVehicle.Vehicle.Doors[5].Close(false);
        }

        bool freezePos = false;

        //while (ClosestVehicle.Vehicle.Exists() && !Game.IsKeyDownRightNow(Keys.O) && Ped.Pedestrian.Exists())
        //{
            

        //    if (Game.IsKeyDownRightNow(Keys.I))
        //    {
        //        freezePos = !freezePos;
        //        if (freezePos)
        //        {
        //            NativeFunction.Natives.ATTACH_ENTITY_TO_ENTITY(Ped.Pedestrian, ClosestVehicle.Vehicle, Settings.SettingsManager.DebugSettings.Draw_BoneIndex,
        //                Settings.SettingsManager.DebugSettings.Draw_LoadBodyXOffset,
        //                Settings.SettingsManager.DebugSettings.Draw_LoadBodyYOffset + YOffset,
        //                Settings.SettingsManager.DebugSettings.Draw_LoadBodyZOffset + ZOffset,
        //                Settings.SettingsManager.DebugSettings.Draw_LoadBodyXRotation,
        //                Settings.SettingsManager.DebugSettings.Draw_LoadBodyYRotation,
        //                Settings.SettingsManager.DebugSettings.Draw_LoadBodyZRotation,
        //                false, false, false, Settings.SettingsManager.DebugSettings.Drag_UseBasicAttachIfPed, Settings.SettingsManager.DebugSettings.Drag_Euler, Settings.SettingsManager.DebugSettings.Drag_OffsetIsRelative, false);
        //            //NativeFunction.Natives.ATTACH_ENTITY_TO_ENTITY(coolguy, ClosestVehicle.Vehicle, bootBoneIndex, FinalPosition.X, FinalPosition.Y, FinalPosition.Z, 0.0f, 0.0f, 0.0f, false, false, false, false, 2, false, false);
        //            GameFiber.Wait(100);
        //            NativeFunction.Natives.TASK_PLAY_ANIM(Ped.Pedestrian, "timetable@floyd@cryingonbed@base", "base", 8.0f, -8.0f, -1, 2, 0, false, false, false);
        //            NativeFunction.Natives.SET_ENTITY_ANIM_CURRENT_TIME(Ped.Pedestrian, "timetable@floyd@cryingonbed@base", "base", 1.0f);
        //            Game.DisplaySubtitle("ATTACHED");
        //        }
        //        else
        //        {
        //            Ped.Pedestrian.Detach();
        //            Game.DisplaySubtitle("DETACHED");
        //        }
        //        GameFiber.Sleep(500);
        //    }
        //    Game.DisplayHelp("PRESS O TO CANCEL, I to Drop");
        //    GameFiber.Yield();
        //}
    

    }









    //private void LoadBodyInCar()
    //{
    //    EntryPoint.WriteToConsole("LoadBodyInCarStarted");
    //    if(ClosestVehicle == null || !ClosestVehicle.Vehicle.Exists() || !Ped.Pedestrian.Exists() || !ClosestVehicle.Vehicle.Doors[5].IsValid())
    //    {
    //        return;
    //    }



    //        Ped.Pedestrian.Detach();
    //        if (!ClosestVehicle.Vehicle.Doors[5].IsFullyOpen)
    //        {
    //            ClosestVehicle.Vehicle.Doors[5].Open(false, false);
    //            GameFiber.Wait(750);
    //            if (ClosestVehicle != null && ClosestVehicle.Vehicle.Exists() && Ped.Pedestrian.Exists())
    //            {
    //                //bool hasBoot = ClosestVehicle.Vehicle.HasBone("boot");
    //                //Vector3 trunkPosition = ClosestVehicle.Vehicle.GetBonePosition("boot");
    //                //EntryPoint.WriteToConsole($"hasBoot:{hasBoot}   trunkPosition1:{trunkPosition}");
    //                //Vector3 AboveVehiclePosition = ClosestVehicle.Vehicle.GetOffsetPositionUp(2f);
    //                //trunkPosition.Z = AboveVehiclePosition.Z;
    //                //EntryPoint.WriteToConsole($"hasBoot:{hasBoot}   trunkPosition2:{trunkPosition}");




    //                bool hasBoot = ClosestVehicle.Vehicle.HasBone("boot");
    //                Vector3 BootPosition = ClosestVehicle.Vehicle.GetBonePosition("boot");



    //                Vector3 AboveVehiclePosition = ClosestVehicle.Vehicle.GetOffsetPositionUp(Settings.SettingsManager.DebugSettings.Draw_LoadBodyZOffset);
    //                Vector3 FinalPosition = NativeHelper.GetOffsetPosition(NativeHelper.GetOffsetPosition(BootPosition, ClosestVehicle.Vehicle.Heading, Settings.SettingsManager.DebugSettings.Draw_LoadBodyXOffset), ClosestVehicle.Vehicle.Heading - 90f, Settings.SettingsManager.DebugSettings.Draw_LoadBodyYOffset);
    //                FinalPosition.Z = AboveVehiclePosition.Z;

    //                //Vector3 AboveVehiclePosition = ClosestVehicle.Vehicle.GetOffsetPosition(new Vector3(Settings.SettingsManager.DebugSettings.Draw_LoadBodyXOffset, Settings.SettingsManager.DebugSettings.Draw_LoadBodyYOffset, Settings.SettingsManager.DebugSettings.Draw_LoadBodyZOffset));




    //                ////Vector3 AboveVehiclePosition = ClosestVehicle.Vehicle.GetOffsetPositionUp(Settings.SettingsManager.DebugSettings.Draw_LoadBodyZOffset);
    //                //Vector3 FinalPosition;
    //                //FinalPosition.X = BootPosition.X + AboveVehiclePosition.X;
    //                //FinalPosition.Y = BootPosition.Y + AboveVehiclePosition.Y;
    //                //FinalPosition.Z = AboveVehiclePosition.Z;



    //                //Vector3 AboveVehiclePosition = ClosestVehicle.Vehicle.GetOffsetPosition(new Vector3(Settings.SettingsManager.DebugSettings.Draw_LoadBodyXOffset, Settings.SettingsManager.DebugSettings.Draw_LoadBodyYOffset,Settings.SettingsManager.DebugSettings.Draw_LoadBodyZOffset));





    //                //Vector3 FinalPosition;
    //                //FinalPosition.X = BootPosition.X;
    //                //FinalPosition.Y = BootPosition.Y;
    //                //FinalPosition.Z = AboveVehiclePosition.Z;

    //                //FinalPosition.X += Settings.SettingsManager.DebugSettings.Draw_LoadBodyXOffset;
    //                //FinalPosition.Y += Settings.SettingsManager.DebugSettings.Draw_LoadBodyYOffset;
    //                //FinalPosition.Z += Settings.SettingsManager.DebugSettings.Draw_LoadBodyZOffset;



    //                NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);

    //                Ped.Pedestrian.Position = FinalPosition;
    //                //NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Ped.Pedestrian);
    //                Ped.Pedestrian.BlockPermanentEvents = true;
    //                Ped.Pedestrian.IsRagdoll = true;
    //                NativeFunction.Natives.SET_PED_TO_RAGDOLL(Ped.Pedestrian, -1, -1, 0, false, false, false);
    //                Ped.Pedestrian.Position = FinalPosition;
    //                GameFiber.Wait(100);
    //            }
    //            GameFiber.Wait(1000);
    //            if (ClosestVehicle != null && ClosestVehicle.Vehicle.Exists())
    //            {
    //                ClosestVehicle.Vehicle.Doors[5].Close(false);
    //            }

    //    }
    //    Cancel();
    //}

    //else if (VehicleToChange.HasBone("boot"))
    //{
    //    EntryPoint.WriteToConsole("PLATE THEFT BONE: boot");
    //    Position = VehicleToChange.GetBonePosition("boot");
    //    Vector3 SpawnPosition = NativeHelper.GetOffsetPosition(Position, VehicleToChange.Heading - 90, 1.75f);
    //    return SpawnPosition;


    private void LoadBodyInCar_old()
    {
        EntryPoint.WriteToConsole("LoadBodyInCarStarted");
        if (ClosestVehicle != null && ClosestVehicle.Vehicle.Exists() && Ped.Pedestrian.Exists() && ClosestVehicle.Vehicle.Doors[5].IsValid())
        {
            Ped.Pedestrian.Detach();
            if (!ClosestVehicle.Vehicle.Doors[5].IsFullyOpen)
            {
                ClosestVehicle.Vehicle.Doors[5].Open(false, false);
                AnimationDictionary.RequestAnimationDictionay("timetable@floyd@cryingonbed@base");
                // the boot is the actual trunk lid, not the place inthe car, cant attach as it moves with the thingo
                //NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, "combat@drag_ped@", "injured_pickup_back_plyr", 2.0f, -2.0f, 5000, 2, 0, false, false, false);
                GameFiber.Wait(750);
                if (ClosestVehicle != null && ClosestVehicle.Vehicle.Exists() && Ped.Pedestrian.Exists())
                {
                    //NativeFunction.Natives.SET_ENTITY_COLLISION(Ped.Pedestrian, false, false);
                   // int trunkBone = ClosestVehicle.Vehicle.GetBoneIndex("boot");// NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", ClosestVehicle.Vehicle, "boot");
                   // int chassisBone = ClosestVehicle.Vehicle.GetBoneIndex("chassis");


                    NativeFunction.Natives.ATTACH_ENTITY_TO_ENTITY(Ped.Pedestrian, ClosestVehicle.Vehicle, -1, Settings.SettingsManager.DebugSettings.TrunkXOffset, Settings.SettingsManager.DebugSettings.TrunkYOffset, Settings.SettingsManager.DebugSettings.TrunkZOffset, 0.0f, 0.0f, 0.0f, false, false, false, false, 20, true);

                    


                    //NativeFunction.Natives.ATTACH_ENTITY_TO_ENTITY(Ped.Pedestrian, ClosestVehicle.Vehicle, trunkBone, 0.08f, 0.51f, 0.08f, 0f, -180f, -3f, false, false, false, false, 2, false);
                    //Ped.Pedestrian.AttachTo(ClosestVehicle.Vehicle, trunkBone, new Vector3(0.08f, 0.51f, 0.08f), new Rotator(0f, -180f, -3f));
                    //Ped.Pedestrian.AttachTo(ClosestVehicle.Vehicle, , new Vector3(0f,-2.2f,0.5f), new Rotator(0f,0f,0f));
                    IsAttached = false;
                    GameFiber.Wait(100);
                    NativeFunction.Natives.TASK_PLAY_ANIM(Ped.Pedestrian, "timetable@floyd@cryingonbed@base", "base", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                    NativeFunction.Natives.SET_ENTITY_ANIM_CURRENT_TIME(Ped.Pedestrian, "timetable@floyd@cryingonbed@base", "base", 0.99f);
                }
                GameFiber.Wait(1000);
                if (ClosestVehicle != null && ClosestVehicle.Vehicle.Exists())
                {
                    ClosestVehicle.Vehicle.Doors[5].Close(false);
                }
            }
        }
        Cancel();
    }
    private void GetTrunkBodyAttachPosition()
    {

    }


    private void DoRagdollDrag()
    {
        if (!Ped.Pedestrian.Exists())
        {
            return;
        }

        Ped.Pedestrian.Detach();
        NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
        //NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Ped.Pedestrian);
        Ped.Pedestrian.BlockPermanentEvents = true;
        Ped.Pedestrian.IsRagdoll = true;
        //NativeFunction.CallByName<bool>("SET_PED_TO_RAGDOLL", Ped.Pedestrian, -1, -1, 0, false, false, false);
        //NativeFunction.Natives.SET_PED_RAGDOLL_FORCE_FALL(Ped.Pedestrian);
        //AttachPeds();
        IsRagdoll = true;

        if (!leftHandObject.Exists())
        {
            leftHandObject = new Rage.Object("ng_proc_cigarette01a", Game.LocalPlayer.Character.GetOffsetPositionFront(2f).Around2D(2f));
        }
        if (leftHandObject.Exists() && Ped.Pedestrian.Exists())
        {
            if(Settings.SettingsManager.DebugSettings.Drag_SetPedsInvisible)
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


            if (Settings.SettingsManager.DebugSettings.Drag_SetNoCollision)
            {
                NativeFunction.Natives.SET_ENTITY_NO_COLLISION_ENTITY(Ped.Pedestrian, Player.Character, false);
            }
            NativeFunction.Natives.SET_PED_TO_RAGDOLL(Ped.Pedestrian, -1, -1, 0, false, false, false);


            if (Settings.SettingsManager.DebugSettings.Drag_RunAttach)
            {

                leftHandObject.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, Settings.SettingsManager.DebugSettings.Drag_ItemAttachBone), Vector3.Zero, Rotator.Zero);





                NativeFunction.Natives.ATTACH_ENTITY_TO_ENTITY_PHYSICALLY(Ped.Pedestrian, leftHandObject,
                    NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Ped.Pedestrian, Settings.SettingsManager.DebugSettings.Drag_PhysicalAttachBone1), //bone 1
                    0,//NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Ped.Pedestrian, Settings.SettingsManager.DebugSettings.Drag_PhysicalAttachBone2),// bone 2
                            Settings.SettingsManager.DebugSettings.DragAttach1X, Settings.SettingsManager.DebugSettings.DragAttach1Y, Settings.SettingsManager.DebugSettings.DragAttach1Z,
                            Settings.SettingsManager.DebugSettings.DragAttach2X, Settings.SettingsManager.DebugSettings.DragAttach2Y, Settings.SettingsManager.DebugSettings.DragAttach2Z,
                            Settings.SettingsManager.DebugSettings.DragAttach3X, Settings.SettingsManager.DebugSettings.DragAttach3Y, Settings.SettingsManager.DebugSettings.DragAttach3Z,
                    100000.0f,//break force
                    Settings.SettingsManager.DebugSettings.DragFixedRotation,//true, //fixed rotation
                    Settings.SettingsManager.DebugSettings.Drag_DoInitialWarp,//true, //DoInitialWarp
                     Settings.SettingsManager.DebugSettings.Drag_Collision,//false, //collision
                           Settings.SettingsManager.DebugSettings.Drag_Teleport,//false, //teleport
                        Settings.SettingsManager.DebugSettings.Drag_RotationOrder                                                                                                                             //2 //RotationORder
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