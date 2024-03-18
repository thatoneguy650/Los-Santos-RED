using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosSantosRED.lsr.Interface;
using ExtensionsMethods;

public class PedPlayerInteract : PedGeneralInteract
{
    protected override bool HasTargetPositionChanged => TargetPosition.DistanceTo2D(Player.Character.GetOffsetPositionFront(Offset)) >= 0.1f;
    public PedPlayerInteract(IRespawnable player, PedExt pedExt, float offset) : base(player,pedExt,offset)
    {
        Player = player;
        PedExt = pedExt;
        Offset = offset;
    }
    protected override void GetDesiredPosition()
    {
        if (PedExt == null || !PedExt.Pedestrian.Exists())
        {
            return;
        }
        bool isFront = false;
        if (CanUseEitherSide)
        {
            float distanceToFront = PedExt.Pedestrian.DistanceTo(Player.Character.GetOffsetPositionFront(-1f * Offset));
            float distanceToRear = PedExt.Pedestrian.DistanceTo(Player.Character.GetOffsetPositionFront(Offset));
            if(distanceToFront < distanceToRear)
            {
                Offset = -1f * Offset;
                isFront = true;
                EntryPoint.WriteToConsole("PED PLAYER INTERACT IS USING FRONT INSTEAD OF REAR");
            }
        }
        TargetPosition = Player.Character.GetOffsetPositionFront(Offset);
        if (isFront)
        {
            float reverseHeading = Player.Character.Heading - 180f;
            if (reverseHeading >= 180f)
            {
                TargetHeading = reverseHeading - 180f;
            }
            else
            {
                TargetHeading = reverseHeading + 180f;
            }

            TargetHeading = reverseHeading;

        }
        else
        {
            TargetHeading = Player.Character.Heading;
        }


        EntryPoint.WriteToConsole($"Player Heading: {Player.Character.Heading} TargetHeading: {TargetHeading} reverseHeading:{Player.Character.Heading - 180f}");
    }
    public void SetPlayerInFront()
    {
        if (PedExt == null || !PedExt.Pedestrian.Exists())
        {
            return;
        }
        Player.Character.Position = PedExt.Pedestrian.GetOffsetPositionFront(Offset);// 0.9f);
        Player.Character.Heading = PedExt.Pedestrian.Heading;
        EntryPoint.WriteToConsole("SET PLAYER IN FRONT OF PedExt!");
    }

}

