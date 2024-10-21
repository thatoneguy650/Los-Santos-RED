using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosSantosRED.lsr.Interface;

public class MachineOffsetResult 
{
    private ILocationInteractable Player;
    private Rage.Object MachingOject;

    public MachineOffsetResult(ILocationInteractable player, Rage.Object machingOject) 
    {
        Player = player;
        MachingOject = machingOject;
    }

    public bool IsDualSided { get; set; } = false;
    public bool IsSingleSided = false;
    public float StandingOffsetPosition { get; set; } = 1.0f;
    public Vector3 PropEntryPosition { get; private set; }
    public float PropEntryHeading { get; private set; }
    public void GetPropEntry()
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

}

