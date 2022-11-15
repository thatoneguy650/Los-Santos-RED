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
        if (Ped.Pedestrian.Exists())
        {
            if(NativeFunction.Natives.IS_PED_USING_ANY_SCENARIO<bool>(Ped.Pedestrian))
            {
                Vector3 PedPos = Ped.Pedestrian.Position;
                NativeFunction.Natives.SET_PED_SHOULD_PLAY_FLEE_SCENARIO_EXIT(Ped.Pedestrian, PedPos.X, PedPos.Y, PedPos.Z);
                NativeFunction.Natives.SET_PED_PANIC_EXIT_SCENARIO(Ped.Pedestrian, PedPos.X, PedPos.Y, PedPos.Z);
            }
            isInVehicle = Ped.Pedestrian.IsInAnyVehicle(false);
            Retask();
            GameTimeLastRan = Game.GameTime;
        }
    }
    public override void Update()
    {
        //if (Ped.Pedestrian.Exists() && Ped.IsDriver)
        //{
        //    NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.Code3Close);
        //}


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
                //unsafe
                //{
                //    int lol = 0;
                //    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                //    NativeFunction.CallByName<bool>("TASK_VEHICLE_MISSION_COORS_TARGET", 0, Ped.Pedestrian.CurrentVehicle, 358.9726f, -1582.881f, 29.29195f, 8, 50f, (int)eCustomDrivingStyles.Code3, 0f, 2f, true);//8f
                //    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                //    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                //    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                //    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                //}
                Vector3 CurrentPos = Ped.Pedestrian.Position;
                NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle,100f, (int)eCustomDrivingStyles.Panic);

                // NativeFunction.CallByName<bool>("TASK_SMART_FLEE_COORD", Ped.Pedestrian, CurrentPos.X, CurrentPos.Y, CurrentPos.Z, 5000f, -1, true, false);
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

