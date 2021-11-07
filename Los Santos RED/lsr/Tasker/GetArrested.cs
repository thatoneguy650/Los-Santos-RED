using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GetArrested : ComplexTask
{
    private uint GameTimeFinishedArrestedAnimation;
    private uint GameTimeFinishedUnArrestedAnimation;
    private bool PlayedArrestAnimation = false;
    private bool PlayedUnArrestAnimation = false;
    private bool StartedWalking = false;
    private VehicleExt VehicleToEnter;
    public GetArrested(IComplexTaskable ped, ITargetable player, VehicleExt toGoTo) : base(player, ped, 1000)
    {
        Name = "GetArrested";
        SubTaskName = "";
        VehicleToEnter = toGoTo;
    }
    public override void Start()
    {
        if (Ped.Pedestrian.Exists())
        {
            EntryPoint.WriteToConsole($"TASKER: GetArrested Start: {Ped.Pedestrian.Handle}", 3);
            Ped.Pedestrian.BlockPermanentEvents = true;
            Ped.Pedestrian.KeepTasks = true;
            uint currentWeapon;
            NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(Ped.Pedestrian, out currentWeapon, true);
            if (currentWeapon != 2725352035)
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Ped.Pedestrian, 2725352035, true);
                NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Ped.Pedestrian, false);
            }

            SetArrestedAnimation(Ped.Pedestrian, false, false);
        }
        GameTimeLastRan = Game.GameTime;
    }
    public override void Update()
    {
        if (Ped.Pedestrian.Exists())
        {
            if(PlayedArrestAnimation && !PlayedUnArrestAnimation && GameTimeFinishedArrestedAnimation > 0 && Game.GameTime - GameTimeFinishedArrestedAnimation >= 5000 && SubTaskName != "UnSetArrested")
            {
                EntryPoint.WriteToConsole($"TASKER: GetArrested Start UNArrest Animation: {Ped.Pedestrian.Handle}", 3);
                UnSetArrestedAnimation(Ped.Pedestrian);
            }
            if(PlayedArrestAnimation && PlayedUnArrestAnimation && SubTaskName != "WalkAway")
            {
                EntryPoint.WriteToConsole($"TASKER: GetArrested WalkAway: {Ped.Pedestrian.Handle}", 3);
                SubTaskName = "WalkAway";

                if(VehicleToEnter != null && VehicleToEnter.Vehicle.Exists())
                {
                    EntryPoint.WriteToConsole($"TASKER: GetArrested Start Walk WITH VEHICLE: {Ped.Pedestrian.Handle}", 3);
                    if (VehicleToEnter.Vehicle.IsSeatFree(1))
                    {
                        NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", Ped.Pedestrian, VehicleToEnter.Vehicle, -1, 1, 1.0f, 9);
                        EntryPoint.WriteToConsole($"TASKER: GetArrested Start Walk WITH VEHICLE SEAT 1: {Ped.Pedestrian.Handle}", 3);
                    }
                    else if (VehicleToEnter.Vehicle.IsSeatFree(2))
                    {
                        NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", Ped.Pedestrian, VehicleToEnter.Vehicle, -1, 2, 1.0f, 9);
                        EntryPoint.WriteToConsole($"TASKER: GetArrested Start Walk WITH VEHICLE SEAT 2: {Ped.Pedestrian.Handle}", 3);
                    }
                    else
                    {
                        Vector3 Pos = Ped.Pedestrian.Position;
                        NativeFunction.Natives.TASK_WANDER_IN_AREA(Ped.Pedestrian, Pos.X, Pos.Y, Pos.Z, 45f, 0f, 0f);
                        EntryPoint.WriteToConsole($"TASKER: GetArrested Start Walk NO VEHICLE: {Ped.Pedestrian.Handle}", 3);
                    }
                }
                else
                {
                    Vector3 Pos = Ped.Pedestrian.Position;
                    NativeFunction.Natives.TASK_WANDER_IN_AREA(Ped.Pedestrian, Pos.X, Pos.Y, Pos.Z, 45f, 0f, 0f);
                    EntryPoint.WriteToConsole($"TASKER: GetArrested Start Walk NO VEHICLE: {Ped.Pedestrian.Handle}", 3);
                }
            }
            if(Ped.IsInVehicle && !Ped.IsDriver)
            {
                Ped.SetWantedLevel(0);
            }
        }
        GameTimeLastRan = Game.GameTime;
    }
    public override void Stop()
    {

    }
    private void ReportCrime()
    {
        if (Ped.Pedestrian.Exists() && Ped.Pedestrian.IsAlive && !Ped.Pedestrian.IsRagdoll)
        {
            Crime ToReport = Ped.PlayerCrimesWitnessed.OrderBy(x => x.Priority).FirstOrDefault();
            foreach (Crime toReport in Ped.PlayerCrimesWitnessed)
            {
                Player.AddCrime(ToReport, false, Ped.PositionLastSeenCrime, Ped.VehicleLastSeenPlayerIn, Ped.WeaponLastSeenPlayerWith, Ped.EverSeenPlayer && Ped.ClosestDistanceToPlayer <= 10f, true, true);
            }
            Ped.PlayerCrimesWitnessed.Clear();
        }
    }
    private void SetArrestedAnimation(Ped PedToArrest, bool MarkAsNoLongerNeeded, bool StayStanding)
    {
        SubTaskName = "SetArrested";
        GameFiber SetArrestedAnimation = GameFiber.StartNew(delegate
        {
            AnimationDictionary.RequestAnimationDictionay("veh@busted_std");
            AnimationDictionary.RequestAnimationDictionay("busted");
            AnimationDictionary.RequestAnimationDictionay("ped");

            if (!PedToArrest.Exists())
            {
                return;
            }

            while (PedToArrest.Exists() && (PedToArrest.IsRagdoll || PedToArrest.IsStunned))
            {
                GameFiber.Yield();
            }

            if (!PedToArrest.Exists())
            {
                return;
            }


            if (PedToArrest.IsInAnyVehicle(false))
            {
                Vehicle oldVehicle = PedToArrest.CurrentVehicle;
                if (PedToArrest.Exists() && oldVehicle.Exists())
                {
                    NativeFunction.CallByName<uint>("TASK_LEAVE_VEHICLE", PedToArrest, oldVehicle, 256);
                    GameFiber.Wait(2500);
                }
            }

            NativeFunction.Natives.SET_PED_DROPS_WEAPON(PedToArrest);
            //Ped.SetWantedLevel(0);

            if (StayStanding)
            {
                if (!NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "ped", "handsup_enter", 3))
                {
                    NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "ped", "handsup_enter", 2.0f, -2.0f, -1, 2, 0, false, false, false);
                }
            }
            else
            {
                if (!NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_2_hands_up", 3) && !NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_a", 3))
                {
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "busted", "idle_2_hands_up", 8.0f, -8.0f, -1, 2, 0, false, false, false);
                    GameFiber.Wait(6000);

                    if (!PedToArrest.Exists() || (PedToArrest == Game.LocalPlayer.Character && !Player.IsBusted))
                    {
                        return;
                    }

                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "busted", "idle_a", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                }
            }
            PedToArrest.KeepTasks = true;

            if (MarkAsNoLongerNeeded)
            {
                PedToArrest.IsPersistent = false;
            }
            GameTimeFinishedArrestedAnimation = Game.GameTime;
            PlayedArrestAnimation = true;
            EntryPoint.WriteToConsole($"TASKER: GetArrested Played Arrest Animation: {Ped.Pedestrian.Handle}", 3);

        }, "SetArrestedAnimation");
    }
    public void UnSetArrestedAnimation(Ped PedToArrest)
    {
        SubTaskName = "UnSetArrested";
        GameFiber UnSetArrestedAnimationGF = GameFiber.StartNew(delegate
        {
            AnimationDictionary.RequestAnimationDictionay("random@arrests");
            AnimationDictionary.RequestAnimationDictionay("busted");
            AnimationDictionary.RequestAnimationDictionay("ped");
            if (NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_a", 3) || NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_2_hands_up", 3))
            {
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "random@arrests", "kneeling_arrest_escape", 8.0f, -8.0f, -1, 4096, 0, 0, 1, 0);//"random@arrests", "kneeling_arrest_escape", 8.0f, -8.0f, -1, 120, 0, 0, 1, 0);//"random@arrests", "kneeling_arrest_escape", 8.0f, -8.0f, -1, 4096, 0, 0, 1, 0);
                //while (NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", PedToArrest, "random@arrests", "kneeling_arrest_escape") < 1.0f)
                //{
                //    GameFiber.Yield();
                //}
                GameFiber.Wait(1000);//1250
                NativeFunction.Natives.CLEAR_PED_TASKS(PedToArrest);
            }
            else if (NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "ped", "handsup_enter", 3))
            {
                NativeFunction.Natives.CLEAR_PED_TASKS(PedToArrest);
            }
            PlayedUnArrestAnimation = true;
            EntryPoint.WriteToConsole($"TASKER: GetArrested Played UNArrest Animation: {Ped.Pedestrian.Handle}", 3);
        }, "UnSetArrestedAnimation");
    }
}

