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
    public float StandingOffsetPosition { get; set; } = 1.0f;
    private void GetPropEntry()
    {
        if (MachingOject == null || !MachingOject.Exists())
        {
            return;
        }
        float DistanceToFront = Player.Position.DistanceTo2D(MachingOject.GetOffsetPositionFront(-1f * StandingOffsetPosition));
        float DistanceToRear = Player.Position.DistanceTo2D(MachingOject.GetOffsetPositionFront(1f * StandingOffsetPosition));
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
        NativeFunction.Natives.TASK_GO_STRAIGHT_TO_COORD(Game.LocalPlayer.Character, PropEntryPosition.X, PropEntryPosition.Y, PropEntryPosition.Z, 1.0f, -1, PropEntryHeading, 0.2f);
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
            IsCloseEnough = Game.LocalPlayer.Character.DistanceTo2D(PropEntryPosition) < 0.2f;
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
            if (Math.Abs(ExtensionsMethods.Extensions.GetHeadingDifference(heading, PropEntryHeading)) <= 0.5f)//0.5f)
            {
                IsFacingDirection = true;
            }
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

