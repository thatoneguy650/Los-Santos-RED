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
    bool isInVehicle = false;
    private ITargetable Target;
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
        //if (NativeFunction.Natives.IS_PED_USING_ANY_SCENARIO<bool>(Ped.Pedestrian))
        //{
        //    Vector3 PedPos = Ped.Pedestrian.Position;
        //    //NativeFunction.Natives.SET_PED_SHOULD_PLAY_FLEE_SCENARIO_EXIT(Ped.Pedestrian, PedPos.X, PedPos.Y, PedPos.Z);
        //    //NativeFunction.Natives.SET_PED_PANIC_EXIT_SCENARIO(Ped.Pedestrian, PedPos.X, PedPos.Y, PedPos.Z);
        //}
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
            NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.Panic);
            NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 100f);//new
            NativeFunction.Natives.SET_DRIVER_ABILITY(Ped.Pedestrian, 1.0f);
            NativeFunction.Natives.SET_DRIVER_AGGRESSIVENESS(Ped.Pedestrian, 1.0f);
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
        if (Ped.Pedestrian.Exists())
        {
            Ped.Pedestrian.BlockPermanentEvents = true;
            Ped.Pedestrian.KeepTasks = true;
            if (isInVehicle && Ped.IsDriver)
            {
                Vector3 CurrentPos = Ped.Pedestrian.Position;
                NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle,100f, (int)eCustomDrivingStyles.Panic);
                NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.Panic);
                NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 100f);//new
                NativeFunction.Natives.SET_DRIVER_ABILITY(Ped.Pedestrian, 1.0f);
                NativeFunction.Natives.SET_DRIVER_AGGRESSIVENESS(Ped.Pedestrian, 1.0f);
            }
            else
            {
                Vector3 CurrentPos = Ped.Pedestrian.Position;
                NativeFunction.CallByName<bool>("TASK_SMART_FLEE_COORD", Ped.Pedestrian, CurrentPos.X, CurrentPos.Y, CurrentPos.Z, 5000f, -1, true, false);
            }
            EntryPoint.WriteToConsole("RETASK FLEE");
        }
    }
}

