using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosSantosRED.lsr.Interface;
using ExtensionsMethods;

public class PedInteractSetup
{

    private PedExt PedExt;
    private IRespawnable Player;
    private Vector3 TargetPosition;
    private float TargetHeading;
    private float Offset;
    protected virtual bool HasTargetPositionChanged => TargetPosition.DistanceTo2D(Player.Character.GetOffsetPositionFront(Offset)) >= 0.1f;
    public PedInteractSetup(IRespawnable player, PedExt pedExt, float offset)
    {
        Player = player;
        PedExt = pedExt;
        Offset = offset;
    }
    public bool ForcePosition { get; set; } = true;
    public uint GameTimeLimit { get; set; } = 15000;
    private bool IsValid => EntryPoint.ModController.IsRunning && (Player.IsBusted || Player.IsArrested) && !Player.IsIncapacitated && Player.IsAlive && PedExt != null && PedExt.Pedestrian.Exists() && !PedExt.Pedestrian.IsDead && !PedExt.IsInWrithe && !PedExt.IsUnconscious;
    public bool IsInPosition { get; private set; }
    public void Start()
    {
        if (!PedExt.Pedestrian.Exists())
        {
            return;
        }
        GetDesiredPosition();
        TaskToPosition();
        MoveLoop();
        if(!ForcePosition)
        {
            return;
        }
        SetForcePosition();
    }
    private void SetForcePosition()
    {
        if (PedExt == null || !PedExt.Pedestrian.Exists())
        {
            return;
        }
        PedExt.Pedestrian.Position = TargetPosition;
        PedExt.Pedestrian.Heading = TargetHeading;
    }
    protected virtual void GetDesiredPosition()
    {
        TargetPosition = Player.Character.GetOffsetPositionFront(Offset);
        TargetHeading = Player.Character.Heading;
    }
    private void TaskToPosition()
    {
        NativeFunction.Natives.CLEAR_PED_TASKS(PedExt.Pedestrian);
        NativeFunction.Natives.TASK_FOLLOW_NAV_MESH_TO_COORD(PedExt.Pedestrian, TargetPosition.X, TargetPosition.Y, TargetPosition.Z, 1.0f, -1, 0.1f, 0, TargetHeading);
    }
    private void MoveLoop()
    {
        IsInPosition = false;
        uint GameTimeStartedWalking = Game.GameTime;
        float prevDistanceToPos = 0f;
        bool isMoving = false;
        uint GameTimeLastGotNavResult = Game.GameTime ;
        bool hasGottenNavResult = false;
        while (IsValid)
        {
            if (HasTargetPositionChanged)
            {
                GetDesiredPosition();
                TaskToPosition();
            }
            float distanceToPos = PedExt.Pedestrian.DistanceTo2D(TargetPosition);
            float headingDiff = Math.Abs(Extensions.GetHeadingDifference(PedExt.Pedestrian.Heading, TargetHeading));
            if (distanceToPos != prevDistanceToPos)
            {
                isMoving = true;
                prevDistanceToPos = distanceToPos;
            }
            else
            {
                isMoving = false;
            }
            if (distanceToPos <= 0.2f && headingDiff <= 0.5f)
            {
                IsInPosition = true;
                break;
            }
            if (!isMoving && distanceToPos <= 0.5f && headingDiff <= 0.5f)
            {
                IsInPosition = true;
                break;
            }
            if (Game.GameTime - GameTimeStartedWalking >= 15000 && distanceToPos <= 1.0f && headingDiff <= 5f)
            {
                IsInPosition = true;
                break;
            }
            if(Game.GameTime - GameTimeStartedWalking >= GameTimeLimit)
            {
                break;
            }


            if(!hasGottenNavResult && Game.GameTime - GameTimeLastGotNavResult >= 2000)
            {
                int RouteResult = NativeFunction.Natives.GET_NAVMESH_ROUTE_RESULT<int>(PedExt.Pedestrian);
                if(RouteResult != 3)//VALID ROUTE
                {
                    SetForcePosition();

                }
                hasGottenNavResult = true;
                GameTimeLastGotNavResult = Game.GameTime;
            }
            GameFiber.Yield();
        }
        if (IsInPosition)
        {
            GameFiber.Wait(500);
        }
    }
}

