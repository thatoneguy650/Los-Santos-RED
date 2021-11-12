using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Kill : ComplexTask
{
    private bool TargettingCar = false;
    public Kill(IComplexTaskable cop, ITargetable player) : base(player, cop, 1000)
    {
        Name = "Kill";
        SubTaskName = "";
    }
    public override void Start()
    {
        if (Ped.Pedestrian.Exists())
        {
            //EntryPoint.WriteToConsole($"TASKER: Kill Start: {Ped.Pedestrian.Handle}");
            ClearTasks();
            NativeFunction.Natives.SET_PED_SHOOT_RATE(Ped.Pedestrian, 100);//30
            NativeFunction.Natives.SET_PED_ALERTNESS(Ped.Pedestrian, 3);//very altert
            NativeFunction.Natives.SET_PED_COMBAT_ABILITY(Ped.Pedestrian, 2);//professional
            NativeFunction.Natives.SET_PED_COMBAT_RANGE(Ped.Pedestrian, 2);//far
            NativeFunction.Natives.SET_PED_COMBAT_MOVEMENT(Ped.Pedestrian, 2);//offensinve
            NativeFunction.Natives.SET_DRIVER_ABILITY(Ped.Pedestrian, 100f);

            if (Ped.IsDriver && (Ped.IsInHelicopter || Ped.IsInBoat))
            {
                if (Ped.IsInHelicopter)
                {
                    Vector3 pedPos = Player.Character.Position;
                    if(Player.Character.CurrentVehicle.Exists())
                    {
                        NativeFunction.Natives.TASK_HELI_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, Player.Character.CurrentVehicle, Player.Character, pedPos.X, pedPos.Y, pedPos.Z, 9, 50f, 150f, -1f, -1, 30, -1.0f, 0);//NativeFunction.Natives.TASK_HELI_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, Player.Character.CurrentVehicle, Player.Character, pedPos.X, pedPos.Y, pedPos.Z, 9, 50f, 150f, -1f, -1, 30, -1.0f, 0);
                    }
                    else
                    {
                        NativeFunction.Natives.TASK_HELI_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 0, Player.Character, pedPos.X, pedPos.Y, pedPos.Z, 9, 50f, 150f, -1f, -1, 30, -1.0f, 0);//NativeFunction.Natives.TASK_HELI_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 0, Player.Character, pedPos.X, pedPos.Y, pedPos.Z, 9, 50f, 150f, -1f, -1, 30, -1.0f, 0);
                    }
                }
                else if (Ped.IsInBoat)
                {
                    NativeFunction.Natives.TASK_VEHICLE_CHASE(Ped.Pedestrian, Player.Character);
                }
            }
            else
            {

                if(!Ped.IsDriver && (Ped.IsInHelicopter || Ped.IsInBoat))
                {

                    if (Ped.IsInHelicopter)
                    {
                        NativeFunction.Natives.TASK_COMBAT_PED(Ped.Pedestrian, Player.Character, 0, 16);
                        int valTurret = NativeFunction.Natives.GET_HASH_KEY<int>("VEHICLE_WEAPON_TURRET_VALKYRIE");
                        NativeFunction.Natives.SET_CURRENT_PED_VEHICLE_WEAPON(Ped.Pedestrian, valTurret);
                    }


                    if (Player.IsInVehicle && Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists() && !TargettingCar)
                    {
                        NativeFunction.Natives.SET_MOUNTED_WEAPON_TARGET(Ped.Pedestrian, 0, Player.CurrentVehicle.Vehicle, 0f, 0f, 0f, 0f, 0f);
                        TargettingCar = true;
                        EntryPoint.WriteToConsole($"Kill: {Ped.Pedestrian.Handle} Start Targetting Player Vehicle", 5);
                    }
                    else if (TargettingCar)
                    {
                        NativeFunction.Natives.SET_MOUNTED_WEAPON_TARGET(Ped.Pedestrian, Player.Character, 0, 0f, 0f, 0f, 0f, 0f);
                        TargettingCar = false;
                        EntryPoint.WriteToConsole($"Kill: {Ped.Pedestrian.Handle} Start Targetting Player Ped", 5);
                    }
                    //NativeFunction.Natives.TASK_COMBAT_PED(Ped.Pedestrian, Player.Character, 0, 16);
                }
                else
                {
                    NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.FullContact, true);
                    int DesiredStyle = (int)eDrivingStyles.AvoidEmptyVehicles | (int)eDrivingStyles.AvoidPeds | (int)eDrivingStyles.AvoidObject | (int)eDrivingStyles.AllowWrongWay | (int)eDrivingStyles.ShortestPath;
                    NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, DesiredStyle);
                    NativeFunction.Natives.SET_DRIVER_ABILITY(Ped.Pedestrian, 100f);
                    NativeFunction.Natives.TASK_COMBAT_PED(Ped.Pedestrian, Player.Character, 0, 16);
                }
            }
        }
        
    }
    public override void Update()
    {
        if (Ped.Pedestrian.Exists())
        {
            NativeFunction.Natives.SET_DRIVER_ABILITY(Ped.Pedestrian, 100f);
            if(Ped.IsInHelicopter)
            {
                if(Ped.IsDriver)
                {
                    if (Ped.DistanceToPlayer <= 100f && Player.Character.Speed < 32f)//70 mph
                    {
                        NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 10f);
                    }
                    else
                    {
                        NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 100f);
                    }
                }
                else
                {
                    if (Player.IsInVehicle && Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists() && !TargettingCar)
                    {
                        NativeFunction.Natives.SET_MOUNTED_WEAPON_TARGET(Ped.Pedestrian, 0, Player.CurrentVehicle.Vehicle, 0f, 0f, 0f, 0f, 0f);
                        TargettingCar = true;
                        EntryPoint.WriteToConsole($"Kill: {Ped.Pedestrian.Handle} Updated Targetting Player Vehicle", 5);
                    }
                    else if(TargettingCar)
                    {
                        NativeFunction.Natives.SET_MOUNTED_WEAPON_TARGET(Ped.Pedestrian, Player.Character, 0, 0f, 0f, 0f, 0f, 0f);
                        TargettingCar = false;
                        EntryPoint.WriteToConsole($"Kill: {Ped.Pedestrian.Handle} Updated Targetting Player Ped",5);
                    }
                }
            }
        }
    }
    public void ClearTasks()//temp public
    {
        if (Ped.Pedestrian.Exists())
        {
            int seatIndex = 0;
            Vehicle CurrentVehicle = null;
            bool WasInVehicle = false;
            if (Ped.Pedestrian.IsInAnyVehicle(false))
            {
                WasInVehicle = true;
                CurrentVehicle = Ped.Pedestrian.CurrentVehicle;
                seatIndex = Ped.Pedestrian.SeatIndex;
            }
            //Ped.Pedestrian.Tasks.Clear();
            NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
            Ped.Pedestrian.BlockPermanentEvents = false;
            Ped.Pedestrian.KeepTasks = false;
            Ped.Pedestrian.RelationshipGroup.SetRelationshipWith(RelationshipGroup.Player, Relationship.Neutral);
            if (WasInVehicle && !Ped.Pedestrian.IsInAnyVehicle(false) && CurrentVehicle.Exists())
            {
                Ped.Pedestrian.WarpIntoVehicle(CurrentVehicle, seatIndex);
            }            
            //EntryPoint.WriteToConsole(string.Format("     ClearedTasks: {0}", Ped.Pedestrian.Handle));
        }
    }
    public override void Stop()
    {

    }
}

