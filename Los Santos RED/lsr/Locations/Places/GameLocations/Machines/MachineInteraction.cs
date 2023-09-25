using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosSantosRED.lsr.Interface;

public class MachineInteraction
{
    private ILocationInteractable Player;
    private Rage.Object MachingOject;
    private string PlayingDict;
    private string PlayingAnim;
    private bool IsCancelled;
    private Vector3 PropEntryPosition;
    private float PropEntryHeading;


    public MachineInteraction(ILocationInteractable player, Rage.Object machingOject)
    {
        Player = player;
        MachingOject = machingOject;
    }

    public bool IsDualSided { get; set; } = false;
    public bool IsSingleSided = false;
    public float StandingOffsetPosition { get; set; } = 1.0f;
    public float CloseDistance { get; set; } = 0.2f;
    public float CloseHeading { get; set; } = 0.5f;
    private void GetPropEntry()
    {
        if (MachingOject == null || !MachingOject.Exists())
        {
            return;
        }
        float DistanceToFront = Player.Position.DistanceTo2D(MachingOject.GetOffsetPositionFront(-1f * StandingOffsetPosition));
        float DistanceToRear = Player.Position.DistanceTo2D(MachingOject.GetOffsetPositionFront(1f * StandingOffsetPosition));
        if(IsSingleSided)
        {
            DistanceToRear = 999f;
        }
        if (DistanceToFront <= DistanceToRear)
        {
            PropEntryPosition = MachingOject.GetOffsetPositionFront(-1.0f * StandingOffsetPosition);
            PropEntryPosition = new Vector3(PropEntryPosition.X, PropEntryPosition.Y, Game.LocalPlayer.Character.Position.Z);
            float ObjectHeading = MachingOject.Heading - 180f;
            if (ObjectHeading >= 180f)
            {
                PropEntryHeading = ObjectHeading - 180f;
            }
            else
            {
                PropEntryHeading = ObjectHeading + 180f;
            }
        }
        else
        {
            //EntryPoint.WriteToConsoleTestLong("Gas Pump You are Closer to the REAR, using that side");
            PropEntryPosition = MachingOject.GetOffsetPositionFront(StandingOffsetPosition);
            PropEntryPosition = new Vector3(PropEntryPosition.X, PropEntryPosition.Y, Game.LocalPlayer.Character.Position.Z);
            float ObjectHeading = MachingOject.Heading;
            if (ObjectHeading >= 180f)
            {
                PropEntryHeading = ObjectHeading - 180f;
            }
            else
            {
                PropEntryHeading = ObjectHeading + 180f;
            }
        }
    }
    public bool MoveToMachine()
    {
        GetPropEntry();
        if (PropEntryPosition == Vector3.Zero)
        {
            return false;
        }
        //NativeFunction.Natives.TASK_GO_STRAIGHT_TO_COORD(Game.LocalPlayer.Character, PropEntryPosition.X, PropEntryPosition.Y, PropEntryPosition.Z, 1.0f, -1, PropEntryHeading, 0.2f);

        NativeFunction.Natives.TASK_FOLLOW_NAV_MESH_TO_COORD(Player.Character, PropEntryPosition.X, PropEntryPosition.Y, PropEntryPosition.Z, 1.0f, -1, 0.2f, 0, PropEntryHeading);


        /*NATIVE PROC TASK_GO_STRAIGHT_TO_COORD(PED_INDEX PedIndex, VECTOR VecCoors,  FLOAT MoveBlendRatio, INT Time = DEFAULT_TIME_BEFORE_WARP, FLOAT FinalHeading = DEFAULT_NAVMESH_FINAL_HEADING, FLOAT TargetRadius = 0.5) = "0x13c3030981ea7c3b"
         
         NATIVE PROC TASK_FOLLOW_NAV_MESH_TO_COORD(PED_INDEX PedIndex,VECTOR VecCoors, FLOAT MoveBlendRatio, INT Time = DEFAULT_TIME_BEFORE_WARP, FLOAT Radius = DEFAULT_NAVMESH_RADIUS, ENAV_SCRIPT_FLAGS NavFlags = ENAV_DEFAULT, FLOAT FinalHeading = DEFAULT_NAVMESH_FINAL_HEADING ) = "0x7d1424753688ee7a"
         
         */


        uint GameTimeStartedSitting = Game.GameTime;
        float heading = Game.LocalPlayer.Character.Heading;
        bool IsFacingDirection = false;
        bool IsCloseEnough = false;
        while (Game.GameTime - GameTimeStartedSitting <= 5000 && !IsCloseEnough && !IsCancelled)
        {
            if (Player.IsMoveControlPressed)
            {
                IsCancelled = true;
            }
            IsCloseEnough = Game.LocalPlayer.Character.DistanceTo2D(PropEntryPosition) < CloseDistance;
            Game.DisplaySubtitle($"Distance: {Game.LocalPlayer.Character.DistanceTo2D(PropEntryPosition)} IsCloseEnough{IsCloseEnough}");
            GameFiber.Yield();
        }
        GameFiber.Sleep(250);
        GameTimeStartedSitting = Game.GameTime;
        while (Game.GameTime - GameTimeStartedSitting <= 5000 && !IsFacingDirection && !IsCancelled)
        {
            if (Player.IsMoveControlPressed)
            {
                IsCancelled = true;
            }
            heading = Game.LocalPlayer.Character.Heading;
            if (Math.Abs(ExtensionsMethods.Extensions.GetHeadingDifference(heading, PropEntryHeading)) <= CloseHeading)//0.5f)
            {
                IsFacingDirection = true;
            }
            Game.DisplaySubtitle($"Current Heading: {heading} PropEntryHeading: {PropEntryHeading}");
            GameFiber.Yield();
        }
        GameFiber.Sleep(250);
        if (IsCloseEnough && IsFacingDirection && !IsCancelled)
        {
            return true;
        }
        else
        {
            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            return false;
        }
    }   
}

