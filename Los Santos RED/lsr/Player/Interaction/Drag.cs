using ExtensionsMethods;
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
    private IEntityProvideable World;
    private bool PedWasDead;
    private VehicleExt ClosestVehicle;
    private Vector3 TrunkPosition;
    private bool LoadBody;
    private bool isBackingUp;

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
    public override void Start()
    {
        if (Ped.Pedestrian.Exists())
        {
            EntryPoint.WriteToConsole($"Drag Started Money: {Ped.Money} Dead: {Ped.IsDead} Unconsc: {Ped.IsUnconscious}");

            Player.IsDraggingBody = true;
            PedCanBeTasked = Ped.CanBeTasked;
            PedCanBeAmbientTasked = Ped.CanBeAmbientTasked;

            Ped.CanBeTasked = false;
            Ped.CanBeAmbientTasked = false;
            Ped.Pedestrian.BlockPermanentEvents = true;
            Ped.Pedestrian.KeepTasks = true;
            LoadBody = false;
            // NativeFunction.Natives.SET_GAMEPLAY_PED_HINT(Ped.Pedestrian, 0f, 0f, 0f, true, -1, 2000, 2000);
            GameFiber.StartNew(delegate
            {
                DragBody();
                

                if (LoadBody)
                {
                    LoadBodyInCar();
                }
                else
                {
                    Cancel();
                }


            }, "Drag");
        }
    }
    private void DragBody()
    {
        EntryPoint.WriteToConsole("Dragging Body");
        if (MoveToBody())
        {
            AnimationDictionary.RequestAnimationDictionay("combat@drag_ped@");
            
            //AttachPeds();
            bool hasCompletedTasks = StartDrag();
            Player.ButtonPromptList.RemoveAll(x => x.Group == "Drop");
            Player.ButtonPromptList.RemoveAll(x => x.Group == "Load");
            if (hasCompletedTasks)
            {
                EntryPoint.WriteToConsole("Drag, Finished, completed");
                if (Ped.Pedestrian.Exists())
                {
                    NativeFunction.Natives.TASK_PLAY_ANIM(Ped.Pedestrian, "combat@drag_ped@", "injured_putdown_ped", 2.0f, -2.0f, -1, 2, 0, false, false, false);
                }
                PlayAnimation("combat@drag_ped@", "injured_putdown_plyr", false, 2, true);
                DetachPeds();
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

    private void AttachPeds()
    {
        if (Ped.Pedestrian.Exists())
        {
            if(Ped.IsDead)
            {
                NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
                NativeFunction.Natives.RESURRECT_PED(Ped.Pedestrian);
                NativeFunction.Natives.REVIVE_INJURED_PED(Ped.Pedestrian);
                NativeFunction.Natives.CLEAR_PED_TASKS_IMMEDIATELY(Ped.Pedestrian);
                NativeFunction.Natives.SET_ENTITY_COLLISION(Ped.Pedestrian, true, true);
                Ped.Pedestrian.BlockPermanentEvents = true;
                Ped.Pedestrian.KeepTasks = true;
            }
            IsAttached = true;
            NativeFunction.Natives.ATTACH_ENTITY_TO_ENTITY(Ped.Pedestrian, Player.Character, 11816, 0f, 0.6f, 0f, 0f, 0f, 0f, false, false, false, false, 2, false);
        }
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
    private bool MoveToBody()
    {
        pedHeadshotHandle = NativeFunction.Natives.RegisterPedheadshot<uint>(Ped.Pedestrian);
        Vector3 DesiredPosition = NativeFunction.CallByName<Vector3>("GET_WORLD_POSITION_OF_ENTITY_BONE", Ped.Pedestrian, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Ped.Pedestrian, 0));
        float DesiredHeading = Game.LocalPlayer.Character.Heading;
        //NativeFunction.Natives.TASK_GO_STRAIGHT_TO_COORD(Game.LocalPlayer.Character, DesiredPosition.X, DesiredPosition.Y, DesiredPosition.Z, 1.0f, -1, DesiredHeading, 0.2f);
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
            if (!Ped.Pedestrian.Exists() || !Player.IsAliveAndFree)
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
            //var Heading = (PedRoot - Player.Character.Position);
            float calcHeading = (float)GetHeading(Player.Character.Position, PedRoot);
            float calcHeading2 = (float)CalculeAngle(PedRoot, Player.Character.Position);
            DesiredHeading = calcHeading2;
            EntryPoint.WriteToConsole($"calcHeading 1 {calcHeading} calcHeading2  {calcHeading2}", 5);
            //calcHeading = -(90 - calcHeading);
            NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", Player.Character, Ped.Pedestrian, 1000);
            EntryPoint.WriteToConsole($"calcHeading 2 {calcHeading} calcHeading2 {calcHeading2}", 5);
            //NativeFunction.Natives.TASK_ACHIEVE_HEADING(Player.Character, calcHeading2, -1);//1200
            GameFiber.Sleep(1000);

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
    private bool StartDrag()
    {
        
        if (Player.CurrentWeapon != null)
        {
            LastWeapon = Player.CurrentWeapon;
        }
        else
        {
            LastWeapon = null;
        }
        Player.SetUnarmed();
        Player.ButtonPromptList.RemoveAll(x => x.Group == "Drop");
        if (!Player.ButtonPromptList.Any(x => x.Identifier == "Drop"))
        {
            Player.ButtonPromptList.Add(new ButtonPrompt("Drop", "Drop", "Drop", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 1));
        }





        if (PlayAttachAnimation() && PlayDragAnimation())
        {

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
        if (Ped.Pedestrian.Exists())
        {
            NativeFunction.Natives.TASK_PLAY_ANIM(Ped.Pedestrian, "combat@drag_ped@", "injured_pickup_back_ped", 2.0f, -2.0f, -1, 2, 0, false, false, false);
        }
        if (PlayAnimation("combat@drag_ped@", "injured_pickup_back_plyr", false, 2, false))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool PlayDragAnimation()
    {
        if (Ped.Pedestrian.Exists())
        {
            NativeFunction.Natives.ATTACH_ENTITY_TO_ENTITY(Ped.Pedestrian, Player.Character, 11816, 0f, 0.5f, 0f, 0f, 0f, 0f, false, false, false, false, 2, false);
            //NativeFunction.Natives.TASK_PLAY_ANIM(Ped.Pedestrian, "combat@drag_ped@", "injured_drag_ped", 8.0f, -8.0f, -1, 1, 0, false, false, false);
        }
        if (PlayAnimation("combat@drag_ped@", "injured_drag_plyr", true, 1, false))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool PlayAnimation(string dictionary, string animation, bool repeat, int flag, bool moveCancels)
    {
        if (!repeat)
        {
            NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, dictionary, animation, 2.0f, -2.0f, -1, flag, 0, false, false, false);
        }
        uint GameTimeStartedAnimation = Game.GameTime;
        float AnimationTime = 0.0f;
        uint GameTimeLastCheckedVehicle = 0;
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
                if (GameTimeLastCheckedVehicle == 0 || Game.GameTime - GameTimeLastCheckedVehicle >= 500)
                {
                    ClosestVehicle = World.Vehicles.GetClosestVehicleExt(Player.Character.Position, true, 5f);
                    if (ClosestVehicle != null && ClosestVehicle.Vehicle.Exists() && ClosestVehicle.Vehicle.HasBone("boot"))
                    {
                        TrunkPosition = ClosestVehicle.Vehicle.GetBonePosition("boot");
                        if (Player.Character.DistanceTo2D(TrunkPosition) > 1.5f)
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
                    //if (!Player.ButtonPromptList.Any(x => x.Identifier == "Load"))
                    //{
                    //    Player.ButtonPromptList.Add(new ButtonPrompt("Load", "Load", "Load", Settings.SettingsManager.KeySettings.InteractNegativeOrNo, 1));
                    //}
                }
                else
                {
                    Player.ButtonPromptList.RemoveAll(x => x.Group == "Load");
                }


                if (Player.ButtonPromptList.Any(x => x.Identifier == "Drop" && x.IsPressedNow))//demand cash?
                {
                    Player.ButtonPromptList.RemoveAll(x => x.Group == "Drop");
                    Player.ButtonPromptList.RemoveAll(x => x.Group == "Load");
                    return true;
                }
                else if (ClosestVehicle != null && Player.ButtonPromptList.Any(x => x.Identifier == "Load" && x.IsPressedNow))//demand cash?
                {
                    Player.ButtonPromptList.RemoveAll(x => x.Group == "Drop");
                    Player.ButtonPromptList.RemoveAll(x => x.Group == "Load");
                    LoadBody = true;
                    return true;
                }

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

    private void LoadBodyInCar()
    {
        EntryPoint.WriteToConsole("LoadBodyInCarStarted");
        if(ClosestVehicle != null && ClosestVehicle.Vehicle.Exists() && Ped.Pedestrian.Exists() && ClosestVehicle.Vehicle.Doors[5].IsValid())
        {

            Ped.Pedestrian.Detach();
            

            if (!ClosestVehicle.Vehicle.Doors[5].IsFullyOpen)
            {
                ClosestVehicle.Vehicle.Doors[5].Open(false, false);
                AnimationDictionary.RequestAnimationDictionay("timetable@floyd@cryingonbed@base");




                // the boot is the actual trunk lid, not the place inthe car, cant attach as it moves with the thingo


                //NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, "combat@drag_ped@", "injured_pickup_back_plyr", 2.0f, -2.0f, 5000, 2, 0, false, false, false);

                GameFiber.Wait(750);
                if(ClosestVehicle != null && ClosestVehicle.Vehicle.Exists() && Ped.Pedestrian.Exists())
                {



                    //NativeFunction.Natives.SET_ENTITY_COLLISION(Ped.Pedestrian, false, false);
                    int trunkBone = ClosestVehicle.Vehicle.GetBoneIndex("boot");// NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", ClosestVehicle.Vehicle, "boot");
                   NativeFunction.Natives.ATTACH_ENTITY_TO_ENTITY(Ped.Pedestrian, ClosestVehicle.Vehicle, trunkBone, 0.08f, 0.51f, 0.08f, 0f, -180f, -3f, false, false, false, false, 2, false);
                    //Ped.Pedestrian.AttachTo(ClosestVehicle.Vehicle, trunkBone, new Vector3(0.08f, 0.51f, 0.08f), new Rotator(0f, -180f, -3f));
                    //Ped.Pedestrian.AttachTo(ClosestVehicle.Vehicle, , new Vector3(0f,-2.2f,0.5f), new Rotator(0f,0f,0f));
                    IsAttached = false;
                    NativeFunction.Natives.TASK_PLAY_ANIM(Ped.Pedestrian, "timetable@floyd@cryingonbed@base", "base", 8.0f, -8.0f, -1, 2, 0, false, false, false);


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
            EntryPoint.WriteToConsole("DRAG DOWN PRESSED");
            if (!isBackingUp)
            {
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "combat@drag_ped@", "injured_drag_plyr", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                if (Ped.Pedestrian.Exists())
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
                    // NativeFunction.Natives.SET_ENTITY_ANIM_CURRENT_TIME(Player.Character, "combat@drag_ped@", "injured_pickup_back_plyr", 0.1f);
                    // NativeFunction.Natives.TASK_PLAY_ANIM(Ped.Pedestrian, "combat@drag_ped@", "injured_drag_ped", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                    if (Ped.Pedestrian.Exists())
                    {
                        NativeFunction.Natives.TASK_PLAY_ANIM(Ped.Pedestrian, "combat@drag_ped@", "injured_putdown_ped", 2.0f, -2.0f, -1, 2, 0, false, false, false);
                    }
                    isBackingUp = false;
                }
            }
            else if(!isBackingUp)
            {
                NativeFunction.Natives.SET_ENTITY_ANIM_CURRENT_TIME(Player.Character, "combat@drag_ped@", "injured_putdown_plyr", 0.0f);
                if (Ped.Pedestrian.Exists())
                {
                    NativeFunction.Natives.SET_ENTITY_ANIM_CURRENT_TIME(Ped.Pedestrian, "combat@drag_ped@", "injured_putdown_ped", 0.0f);
                }
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
            //EntryPoint.WriteToConsole("DRAG RIGHT PRESSED");
            //Player.Character.Heading -= 1f;
            //Ped.Pedestrian.Heading -= 1f;
        }
        if (Game.IsControlPressed(2, GameControl.MoveLeftOnly) || Game.IsControlPressed(2, GameControl.MoveLeft))
        {
            isLeftPressed = true;
            //EntryPoint.WriteToConsole("DRAG LEFT PRESSED");
            //Player.Character.Heading += 1f;
            //Ped.Pedestrian.Heading += 1f;
        }

        if (isRightPressed)
        {
            //EntryPoint.WriteToConsole("DRAG RIGHT PRESSED");
            Player.Character.Heading -= 0.7f;
        }
        else if (isLeftPressed)
        {
            // EntryPoint.WriteToConsole("DRAG LEFT PRESSED");
            Player.Character.Heading += 0.7f;
        }
    }

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
        if(!LoadBody)
        {
            DetachPeds();
        }
        
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        Player.IsDraggingBody = false;
    }
    public override void Pause()
    {
        Cancel();
    }
    public override bool IsPaused() => false;

    //private void AttachPedsOld()
    //{
    //    if (Ped.Pedestrian.Exists())
    //    {
    //        if (Ped.IsDead)
    //        {
    //            NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
    //            NativeFunction.Natives.RESURRECT_PED(Ped.Pedestrian);
    //            NativeFunction.Natives.REVIVE_INJURED_PED(Ped.Pedestrian);
    //            NativeFunction.Natives.CLEAR_PED_TASKS_IMMEDIATELY(Ped.Pedestrian);
    //            NativeFunction.Natives.SET_ENTITY_COLLISION(Ped.Pedestrian, true, true);
    //        }



    //        IsAttached = true;
    //        //Vector3 PedPosition = Ped.Pedestrian.GetOffsetPosition(new Vector3(0f, 1.2f, 0f));

    //        //float resultArg = PedPosition.Z;
    //        //NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD(PedPosition.X, PedPosition.Y, PedPosition.Z + 2f, out resultArg, false);


    //        //PedPosition = new Vector3(PedPosition.X, PedPosition.Y, resultArg);

    //        //Player.Character.Position = PedPosition;// Ped.Pedestrian.GetOffsetPosition(new Vector3(0f, 1.2f, 0f));
    //        //Player.Character.Heading = Ped.Pedestrian.Heading;
    //        //Ped.Pedestrian.IsRagdoll = true;

    //        //Vector3 TargetPosition = Player.Character.Position;
    //        //NativeFunction.Natives.SET_ENTITY_COORDS_NO_OFFSET(Ped.Pedestrian, TargetPosition.X, TargetPosition.Y, TargetPosition.Z, false, false, true);
    //        //Ped.Pedestrian.Heading = Player.Character.Heading;


    //        //Vector3 TargetPosition = Ped.Pedestrian.Position;
    //        //NativeFunction.Natives.SET_ENTITY_COORDS_NO_OFFSET(Player.Character, TargetPosition.X, TargetPosition.Y, TargetPosition.Z, false, false, true);
    //        //Player.Character.Heading = Ped.Pedestrian.Heading;

    //        if (Ped.Pedestrian.Exists())
    //        {


    //            //Vector3 rotation = NativeFunction.Natives.GET_ENTITY_ROTATION<Vector3>(Ped.Pedestrian, 2);

    //            //Rotator rotator = Ped.Pedestrian.Rotation;
    //            //NativeFunction.Natives.ATTACH_ENTITY_TO_ENTITY(Ped.Pedestrian, Player.Character, 11816, 0f, 0.5f, rotation.X, rotation.Y, rotation.Z, 0f, false, false, true, false, 2, false);
    //            //Ped.Pedestrian.AttachTo(Player.Character, 11816, new Vector3(0f, 0.5f, 0f), new Rotator(0, 0, 0));
    //            //NativeFunction.Natives.ATTACH_ENTITY_TO_ENTITY(Ped.Pedestrian, Player.Character, 11816, 0f, 0.48f, 0f, 0f, 0f, 0f, false, true, true, false, 2, false);
    //            NativeFunction.Natives.ATTACH_ENTITY_TO_ENTITY(Ped.Pedestrian, Player.Character, 11816, 0f, 0.48f, 0f, 0f, 0f, 0f, false, false, false, false, 2, false);
    //        }
    //    }
    //    // Ped.Pedestrian.AttachTo(Player.Character, 11816, new Vector3(0f, 0.5f, 0f), new Rotator(0, 0, 0));
    //}


}