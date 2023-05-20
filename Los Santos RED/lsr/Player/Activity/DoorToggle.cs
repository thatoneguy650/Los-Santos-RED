using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;

public class DoorToggle : DynamicActivity
{
    private Vector3 DoorTogglePosition;
    private float DoorToggleHeading;
    private IActionable Player;
    private ISettingsProvideable Settings;
    private VehicleExt TargetVehicle;
    private IEntityProvideable World;
    private int DoorID;
    private string animDict;
    private string anim;
    private uint DoorToggleWaitTime;
    private float AnimationToggleTime;
    private bool isDriverSide = false;
    private bool ForceDoorState = false;
    private bool DoorForcedState = false;


    private bool IsDoorAlreadyOpen = false;

    public DoorToggle(IActionable player, ISettingsProvideable settings, IEntityProvideable world, VehicleExt vehicleExt, int doorID, bool forceDoorState, bool doorForcedState)
    {
        Player = player;
        Settings = settings;
        World = world;
        TargetVehicle = vehicleExt;
        DoorID = doorID;
        ForceDoorState = forceDoorState;
        DoorForcedState = doorForcedState;
    }
    public override string DebugString => "";
    public override ModItem ModItem { get; set; }
    public override bool CanPause { get; set; } = false;
    public override bool CanCancel { get; set; } = false;
    public override bool IsUpperBodyOnly { get; set; } = false;
    public override string PausePrompt { get; set; } = "Pause Activity";
    public override string CancelPrompt { get; set; } = "Stop Activity";
    public override string ContinuePrompt { get; set; } = "Continue Activity";


    public override void Cancel()
    {
        Player.ActivityManager.IsPerformingActivity = false;
    }
    public override void Continue()
    {

    }
    public override void Pause()
    {
    }
    public override bool IsPaused() => false;
    public override void Start()
    {
        //EntryPoint.WriteToConsole($"PLAYER EVENT: STARTED TOGGLE DOOR");
        if (TargetVehicle == null || !TargetVehicle.Vehicle.Exists())
        {
            Game.DisplayHelp("No vehicle found");
            Player.ActivityManager.IsPerformingActivity = false;
            return;
        }
        if(!Setup())
        {
            Game.DisplayHelp("Cannot Toggle Door");
            Player.ActivityManager.IsPerformingActivity = false;
            return;
        }
        if (DoorTogglePosition == Vector3.Zero)
        {
            Game.DisplayHelp("Cannot Toggle Door");
            Player.ActivityManager.IsPerformingActivity = false;
            return;
        }
        if(ForceDoorState && !DoorForcedState && !IsDoorAlreadyOpen)
        {
            //tasked to close and door is already closed, do nothing
            EntryPoint.WriteToConsole($"DOOR TOGGLE DOOR IS ALREADY CLOSED ForceDoorState{ForceDoorState} DoorForcedState{DoorForcedState} IsDoorAlreadyOpen{IsDoorAlreadyOpen} DoorID{DoorID}");
            Player.ActivityManager.IsPerformingActivity = false;
            return;
        }
        Enter();
    }
    public override bool CanPerform(IActionable player)
    {
        if (player.IsOnFoot && player.ActivityManager.CanPerformActivitiesExtended && !player.ActivityManager.IsResting)
        {
            return true;
        }
        Game.DisplayHelp($"Cannot Toggle Door");
        return false;
    }
    private void Enter()
    {
        GameFiber ToggleDoorAnimation = GameFiber.StartNew(delegate
        {
            try
            {
                Player.WeaponEquipment.SetUnarmed();        
                if (!MovePedToCarPosition())
                {
                    EntryPoint.WriteToConsole($"DOOR TOGGLE CANNOT MOVE TO POSITION ForceDoorState{ForceDoorState} DoorForcedState{DoorForcedState} IsDoorAlreadyOpen{IsDoorAlreadyOpen} DoorID{DoorID}");
                    //EntryPoint.WriteToConsoleTestLong($"DOOR TOGGLE: CAN NOT MOVE TO POSITION");
                    Player.ActivityManager.IsPerformingActivity = false;
                    return;
                }
                if(ForceDoorState && DoorForcedState && IsDoorAlreadyOpen)
                {
                    EntryPoint.WriteToConsole($"DOOR TOGGLE DOOR IS ALREADY OPEN ForceDoorState{ForceDoorState} DoorForcedState{DoorForcedState} IsDoorAlreadyOpen{IsDoorAlreadyOpen} DoorID{DoorID}");
                    //wanted to open the door, but its already open
                    Player.ActivityManager.IsPerformingActivity = false;
                    return;
                }

                GetAnimation();
                //EntryPoint.WriteToConsoleTestLong($"DOOR TOGGLE: GET ANIMATION");
                AnimationDictionary.RequestAnimationDictionay(animDict);
                AnimationWatcher aw = new AnimationWatcher();
                uint GameTimeStartedAnimation = Game.GameTime;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, animDict, anim, 2.0f, -2.0f, -1, (int)(AnimationFlags.StayInEndFrame), 0, false, false, false);
                bool toggledDoor = false;
                //EntryPoint.WriteToConsoleTestLong($"DOOR TOGGLE: STARTED ANIM");
                while (Player.IsAliveAndFree && !Player.IsMoveControlPressed)
                {
                    float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, animDict, anim);
                    if (AnimationTime >= 1.0f)
                    {
                        //EntryPoint.WriteToConsoleTestLong($"DOOR TOGGLE: FINISHED ANIM ANIM TIME TIME OUT");
                        break;
                    }
                    if (!aw.IsAnimationRunning(AnimationTime))
                    {
                        //EntryPoint.WriteToConsoleTestLong($"DOOR TOGGLE: FINISHED ANIM ANIM NOT RUNNING");
                        break;
                    }
                    if (!toggledDoor && AnimationTime >= AnimationToggleTime)
                    {
                        DoDoorMovement();
                        toggledDoor = true;
                    }
                    GameFiber.Yield();
                }
                NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                Player.ActivityManager.IsPerformingActivity = false;
                //EntryPoint.WriteToConsoleTestLong($"DOOR TOGGLE: FINISHED ANIM");
            }
            catch (Exception e)
            {
                Player.ActivityManager.IsPerformingActivity = false;
            }
        }, "PlayDispatchQueue");
    }
    private void GetAnimation()
    {
        //EntryPoint.WriteToConsoleTestLong($"DoorID: {DoorID}");
        animDict = "veh@std@ds@enter_exit";
        anim = "d_close_out";
        AnimationToggleTime = Settings.SettingsManager.DoorToggleSettings.DefaultAnimationTime;
        bool isOpen = TargetVehicle != null && TargetVehicle.Vehicle.Exists() && TargetVehicle.Vehicle.Doors[DoorID].IsOpen;
        if (DoorID == 5 || DoorID == 4)
        {      
            animDict = "anim_heist@hs4f@ig14_open_car_trunk@male@";
            anim = isOpen ? "close_trunk" : "open_trunk_rushed";

            if(isOpen)
            {
                AnimationToggleTime = Settings.SettingsManager.DoorToggleSettings.CloseHoodAnimationTime;
            }
            else
            {
                AnimationToggleTime = Settings.SettingsManager.DoorToggleSettings.OpenHoodAnimationTime;
            }
        }
        else
        {
            if (DoorID == 0)
            {
                animDict = "veh@std@ds@enter_exit";
            }
            else if (DoorID == 1)
            {
                animDict = "veh@std@ps@enter_exit";
            }
            else if (DoorID == 3)
            {
                animDict = "veh@std@rps@enter_exit";
            }
            else if (DoorID == 2)
            {
                animDict = "veh@std@rds@enter_exit";
            }
            anim = isOpen ? "d_close_out" : "d_open_out";
        }
        //EntryPoint.WriteToConsoleTestLong($"isDriverSide: {isDriverSide} anim {anim} animDict {animDict}");
    }
    private bool FloatIsWithin(float value, float minimum, float maximum)
    {
        return value >= minimum && value <= maximum;
    }
    private bool MovePedToCarPosition()
    {
        bool Continue = true;
        bool StopDriver = false;
        EntryPoint.WriteToConsole($"DOOR TOGGLE: STARTED MOVE TO POSITION");
        NativeFunction.CallByName<uint>("TASK_PED_SLIDE_TO_COORD", Player.Character, DoorTogglePosition.X, DoorTogglePosition.Y, DoorTogglePosition.Z, DoorToggleHeading, -1);
        while (!(Player.Character.DistanceTo2D(DoorTogglePosition) <= 0.25f && FloatIsWithin(Player.Character.Heading, DoorToggleHeading - 5f, DoorToggleHeading + 5f)))//while (!(PedToMove.DistanceTo2D(PositionToMoveTo) <= 0.15f && FloatIsWithin(PedToMove.Heading, DesiredHeading - 5f, DesiredHeading + 5f)))
        {
            GameFiber.Yield();
            if (Player.IsMoveControlPressed)
            {
                Continue = false;
                break;
            }
            if (StopDriver && TargetVehicle != null && TargetVehicle.Vehicle.Exists() && TargetVehicle.Vehicle.Driver.Exists())
            {
                NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", TargetVehicle.Vehicle.Driver, TargetVehicle.Vehicle, 27, -1);
            }
            if (Settings.SettingsManager.DoorToggleSettings.ShowMarker)
            {
                Rage.Debug.DrawArrowDebug(DoorTogglePosition, Vector3.Zero, Rotator.Zero, 1f, System.Drawing.Color.Red);
                Game.DisplaySubtitle($"DesiredHeading: {DoorToggleHeading} Current:{Math.Round(Player.Character.Heading, 2)}");
            }

        }
        EntryPoint.WriteToConsole($"DOOR TOGGLE: END MOVE TO POSITION Continue{Continue}");
        if (!Continue)
        {
            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            return false;
        }
        return true;
    }
    private void DoDoorMovement()
    {
        if (TargetVehicle.Vehicle.Doors[DoorID].IsOpen)
        {
            TargetVehicle.Vehicle.Doors[DoorID].Close(false);
        }
        else
        {
            TargetVehicle.Vehicle.Doors[DoorID].Open(false, false);
        }
    }
    private bool Setup()
    {
        EntryPoint.WriteToConsole($"DOOR TOGGLE SETUP ForceDoorState{ForceDoorState} DoorForcedState{DoorForcedState} DoorID{DoorID}");
        IsDoorAlreadyOpen = false;
        isDriverSide = DoorID == 0 || DoorID == 2;
        if (TargetVehicle == null || !TargetVehicle.Vehicle.Exists())
        {
            return false;
        }
        if (DoorID == 5)//is trunk
        {
            //Vector3 BumperPosition = TargetVehicle.Vehicle.GetBonePosition("bumper_r");
            //Vector3 BootPosition = TargetVehicle.Vehicle.GetBonePosition("boot");
            //float Difference = Math.Abs(BumperPosition.Y - BootPosition.Y);

            //DoorTogglePosition = BootPosition;
            //DoorTogglePosition = NativeHelper.GetOffsetPosition(DoorTogglePosition, TargetVehicle.Vehicle.Heading + Settings.SettingsManager.DebugSettings.DoorToggle_TrunkHeading, Settings.SettingsManager.DebugSettings.DoorToggle_TrunkOffset - Difference);
            //DoorToggleHeading = TargetVehicle.Vehicle.Heading;



            float length = TargetVehicle.Vehicle.Model.Dimensions.Y;
            DoorTogglePosition = TargetVehicle.Vehicle.Position;
            DoorTogglePosition = NativeHelper.GetOffsetPosition(DoorTogglePosition, TargetVehicle.Vehicle.Heading + Settings.SettingsManager.DoorToggleSettings.TrunkHeading, (-1 * length / 2) + Settings.SettingsManager.DoorToggleSettings.TrunkOffset);
            DoorToggleHeading = TargetVehicle.Vehicle.Heading;





        }
        else if (DoorID == 4)//is hood
        {
            float length = TargetVehicle.Vehicle.Model.Dimensions.Y;
            DoorTogglePosition = TargetVehicle.Vehicle.Position;
            DoorTogglePosition = NativeHelper.GetOffsetPosition(DoorTogglePosition, TargetVehicle.Vehicle.Heading + Settings.SettingsManager.DoorToggleSettings.HoodHeading, (length / 2) + Settings.SettingsManager.DoorToggleSettings.HoodOffset);
            DoorToggleHeading = TargetVehicle.Vehicle.Heading - 180f;



            //Vector3 BumperPosition = TargetVehicle.Vehicle.GetBonePosition("bumper_f");
            //Vector3 BonnetPosition = TargetVehicle.Vehicle.GetBonePosition("bonnet");
            //float Difference = Math.Abs(BumperPosition.Y - BonnetPosition.Y);
            //DoorTogglePosition = BonnetPosition;
            //DoorTogglePosition = NativeHelper.GetOffsetPosition(DoorTogglePosition, TargetVehicle.Vehicle.Heading + Settings.SettingsManager.DebugSettings.DoorToggle_HoodHeading, Settings.SettingsManager.DebugSettings.DoorToggle_HoodOffset + Difference);
            //DoorToggleHeading = TargetVehicle.Vehicle.Heading-180f;
        }
        else
        {
            DoorTogglePosition = NativeFunction.Natives.GET_ENTRY_POINT_POSITION<Vector3>(TargetVehicle.Vehicle, DoorID);
            if(isDriverSide)
            {
                //EntryPoint.WriteToConsoleTestLong("DRIVER SIDE");
                DoorToggleHeading = TargetVehicle.Vehicle.Heading - 90f;
            }
            else
            {
                //EntryPoint.WriteToConsoleTestLong("PASSENGER SIDE");
                DoorToggleHeading = TargetVehicle.Vehicle.Heading + 90f;
            }            
        }
        if(DoorToggleHeading >= 360f)
        {
            //EntryPoint.WriteToConsoleTestLong("TOO LARGE< LOWERING");
            DoorToggleHeading = DoorToggleHeading - 360f;
        }

        if (TargetVehicle.Vehicle.Doors[DoorID].IsValid())
        {
            IsDoorAlreadyOpen = TargetVehicle.Vehicle.Doors[DoorID].IsOpen;
        }


        //EntryPoint.WriteToConsoleTestLong($"DOOR TOGGLE: FINISHED SETUP");
        return true;
    }
}