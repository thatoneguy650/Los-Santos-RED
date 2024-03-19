using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosSantosRED.lsr.Interface;
using ExtensionsMethods;

public class PedGeneralInteract
{
    protected PedExt PedExt;
    protected IRespawnable Player;
    protected Vector3 TargetPosition;
    protected float TargetHeading;
    protected float Offset;
    protected virtual bool HasTargetPositionChanged => false;// TargetPosition.DistanceTo2D(Player.Character.GetOffsetPositionFront(Offset)) >= 0.1f;
    public PedGeneralInteract(IRespawnable player, PedExt pedExt, float offset)
    {
        Player = player;
        PedExt = pedExt;
        Offset = offset;
    }
    public bool ForcePosition { get; set; } = true;
    public uint GameTimeLimit { get; set; } = 15000;
    public bool CanUseEitherSide { get; set; } = true;
    protected bool IsValid => EntryPoint.ModController.IsRunning && (Player.IsBusted || Player.IsArrested) && !Player.IsIncapacitated && Player.IsAlive && PedExt != null && PedExt.Pedestrian.Exists() && !PedExt.Pedestrian.IsDead && !PedExt.IsInWrithe && !PedExt.IsUnconscious;
    public bool IsInPosition { get; protected set; }
    public void Start()
    {
        if (!PedExt.Pedestrian.Exists())
        {
            return;
        }
        EntryPoint.WriteToConsole("PED GENERAL INTERACT START");
        GetDesiredPosition();
        TaskToPosition();
        MoveLoop();
        if (ForcePosition && IsInPosition)
        {
            SetForcePosition();
        }
    }
    public void SetForcePosition()
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
        uint GameTimeLastGotNavResult = Game.GameTime;
        bool hasGottenNavResult = false;
        uint GameTimeStoppedMoving = 0;
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
            if (!isMoving)
            {
                if (GameTimeStoppedMoving == 0)
                {
                    GameTimeStoppedMoving = Game.GameTime;
                }
            }
            else
            {
                GameTimeStoppedMoving = 0;
            }
            if (distanceToPos <= 0.2f && headingDiff <= 0.5f)
            {
                IsInPosition = true;
                EntryPoint.WriteToConsole("PedGeneralInteract VALID 1");
                break;
            }
            if (!isMoving && distanceToPos <= 0.5f && headingDiff <= 0.5f)
            {
                IsInPosition = true;
                EntryPoint.WriteToConsole("PedGeneralInteract VALID 2");
                break;
            }
            if (GameTimeStoppedMoving != 0 && !isMoving && Game.GameTime - GameTimeStoppedMoving >= 4000)
            {
                if (distanceToPos <= 1.0f && headingDiff <= 5f)
                {
                    IsInPosition = true;
                }
                EntryPoint.WriteToConsole("PedGeneralInteract NOT MOVING TIMEOUT");
                break;
            }
            if (Game.GameTime - GameTimeStartedWalking >= 15000 && distanceToPos <= 1.0f && headingDiff <= 5f)
            {
                IsInPosition = true;
                break;
            }
            if (Game.GameTime - GameTimeStartedWalking >= GameTimeLimit)
            {
                EntryPoint.WriteToConsole("PedGeneralInteract TIMEOUT");
                break;
            }
            if (!hasGottenNavResult && Game.GameTime - GameTimeLastGotNavResult >= 2000)
            {
                int RouteResult = NativeFunction.Natives.GET_NAVMESH_ROUTE_RESULT<int>(PedExt.Pedestrian);
                if (RouteResult != 3)//VALID ROUTE
                {
                    //SetForcePosition();
                    IsInPosition = true;
                    //just end this hell!

                    EntryPoint.WriteToConsole("PedGeneralInteract NO VALID POSITION");
                    break;
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

