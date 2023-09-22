using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Flee : ComplexTask
{
    private bool isInVehicle = false;
    private ITargetable Target;
    private bool isCowering = false;
    private bool IsWithinCowerDistance => Ped.DistanceToPlayer <= Ped.CowerDistance;
    private bool ShouldCower => Ped.WillCower && IsWithinCowerDistance && !Player.RecentlyShot;
    public Flee(IComplexTaskable ped, ITargetable player) : base(player, ped, 5000)
    {
        Name = "Flee";
        SubTaskName = "";
        Target = player;
    }
    public override void Start()
    {
        if (!Ped.Pedestrian.Exists())
        {
            return;
        }
        NativeFunction.Natives.SET_PED_SHOULD_PLAY_IMMEDIATE_SCENARIO_EXIT(Ped.Pedestrian);
        isInVehicle = Ped.Pedestrian.IsInAnyVehicle(false);
        Retask();
        GameTimeLastRan = Game.GameTime;    
    }
    public override void Update()
    {
        if (Ped.Pedestrian.Exists() && isInVehicle != Ped.Pedestrian.IsInAnyVehicle(false))
        {
            isInVehicle = Ped.Pedestrian.IsInAnyVehicle(false);
            Retask();
        }
        if(isInVehicle && Ped.IsDriver)
        {
            NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.Alerted);
            NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 100f);//new
            NativeFunction.Natives.SET_DRIVER_ABILITY(Ped.Pedestrian, 1.0f);
            NativeFunction.Natives.SET_DRIVER_AGGRESSIVENESS(Ped.Pedestrian, 1.0f);
        }
        if(ShouldCower != isCowering)
        {
            Retask();
        }
        GameTimeLastRan = Game.GameTime;
    }
    public override void Stop()
    {

    }
    public override void ReTask()
    {

    }
    private void Retask()
    {
        if (!Ped.Pedestrian.Exists())
        {
            return;
        }
        Ped.Pedestrian.BlockPermanentEvents = true;
        Ped.Pedestrian.KeepTasks = true;
        if (isInVehicle && Ped.IsDriver)
        {
            Vector3 CurrentPos = Ped.Pedestrian.Position;
            NativeFunction.CallByName<bool>("TASK_SMART_FLEE_COORD", Ped.Pedestrian, CurrentPos.X, CurrentPos.Y, CurrentPos.Z, 5000f, -1, true, false);
            NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.Alerted);
            NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 100f);//new
            NativeFunction.Natives.SET_DRIVER_ABILITY(Ped.Pedestrian, 1.0f);
            NativeFunction.Natives.SET_DRIVER_AGGRESSIVENESS(Ped.Pedestrian, 1.0f);
        }
        else
        {
            Vector3 CurrentPos = Ped.Pedestrian.Position;
            if(Ped.WillCower && IsWithinCowerDistance)
            {
                NativeFunction.Natives.TASK_COWER(Ped.Pedestrian, -1);
                EntryPoint.WriteToConsole("FLEE SET PED COWER");
            }
            else
            {
                NativeFunction.CallByName<bool>("TASK_SMART_FLEE_COORD", Ped.Pedestrian, CurrentPos.X, CurrentPos.Y, CurrentPos.Z, 5000f, -1, true, false);
            }     
        }
        isCowering = ShouldCower;
    }
}

