using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System.Linq;

public class SurrenderActivity : DynamicActivity
{
    private IInputable Player;
    //private VehicleExt VehicleTryingToEnter;
    //private int SeatTryingToEnter;
    private IEntityProvideable World;
    //private Vehicle VehicleTaskedToEnter;
    //private int SeatTaskedToEnter;
    public SurrenderActivity(IInputable currentPlayer, IEntityProvideable world)
    {
        Player = currentPlayer;
        World = world;
    }
    public bool CanSurrender => !Player.HandsAreUp && !Player.IsAiming && (!Player.IsInVehicle || !Player.IsMoving);
    public override ModItem ModItem { get; set; }
    public override string DebugString => "";
    public bool IsCommitingSuicide { get; set; }
    public override void Cancel()
    {
        LowerHands();
    }
    public override void Pause()
    {

    }
    public override void Continue()
    {

    }
    public override void Start()
    {
        RaiseHands();
    }
    public void LowerHands()
    {
        EntryPoint.WriteToConsole($"PLAYER EVENT: Lower Hands", 3);
        Player.HandsAreUp = false; // You put your hands down
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        Player.IsPerformingActivity = false;
    }
    public void RaiseHands()
    {
        EntryPoint.WriteToConsole($"PLAYER EVENT: Raise Hands", 3);
        if (Player.Character.IsWearingHelmet)
        {
            Player.Character.RemoveHelmet(true);
        }
        if (Player.HandsAreUp)
        {
            return;
        }
        Player.SetUnarmed();
        Player.HandsAreUp = true;
        AnimationDictionary.RequestAnimationDictionay("ped");
        NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, "ped", "handsup_enter", 2.0f, -2.0f, -1, 2, 0, false, false, false);
    }
    public void SetArrestedAnimation(bool StayStanding)
    {
        //StayStanding = false;
        GameFiber SetArrestedAnimation = GameFiber.StartNew(delegate
        {
            AnimationDictionary.RequestAnimationDictionay("veh@busted_std");
            AnimationDictionary.RequestAnimationDictionay("busted");
            AnimationDictionary.RequestAnimationDictionay("ped");
            if (!Player.Character.Exists())
            {
                return;
            }
            while (Player.Character.Exists() && (Player.Character.IsRagdoll || Player.Character.IsStunned))
            {
                GameFiber.Yield();
            }
            if (!Player.Character.Exists())
            {
                return;
            }
            Player.SetUnarmed();
            if (Player.Character.IsInAnyVehicle(false))
            {
                Vehicle oldVehicle = Player.Character.CurrentVehicle;
                if (Player.Character.Exists() && oldVehicle.Exists())
                {
                    NativeFunction.Natives.TASK_LEAVE_VEHICLE(Player.Character, oldVehicle, 256);
                    while(Player.Character.IsInAnyVehicle(false) && Player.IsBusted)
                    {
                        GameFiber.Yield();
                    }
                    if (!Player.Character.Exists() || !Player.IsBusted)
                    {
                        return;
                    }
                }
            }
            if (StayStanding)
            {
                //Player.SetUnarmed();
                if (!NativeFunction.Natives.IS_ENTITY_PLAYING_ANIM<bool>(Player.Character, "ped", "handsup_enter", 3) || NativeFunction.Natives.GET_ENTITY_ANIM_CURRENT_TIME<float>(Player.Character, "ped", "handsup_enter") == 0f)
                {
                    NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, "ped", "handsup_enter", 2.0f, -2.0f, -1, 2, 0, false, false, false);
                    //GameFiber.Wait(1000);
                    //NativeFunction.Natives.SET_PED_DROPS_WEAPON(Player.Character);
                }
            }
            else
            {
                if (!NativeFunction.Natives.IS_ENTITY_PLAYING_ANIM<bool>(Player.Character, "busted", "idle_a", 3) || NativeFunction.Natives.GET_ENTITY_ANIM_CURRENT_TIME<float>(Player.Character, "busted", "idle_a") == 0f
                 || !NativeFunction.Natives.IS_ENTITY_PLAYING_ANIM<bool>(Player.Character, "busted", "idle_2_hands_up", 3) || NativeFunction.Natives.GET_ENTITY_ANIM_CURRENT_TIME<float>(Player.Character, "busted", "idle_2_hands_up") == 0f)
                {
                    NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, "busted", "idle_2_hands_up", 8.0f, -8.0f, -1, 2, 0, false, false, false);
                    GameFiber.Wait(1000);
                    //NativeFunction.Natives.SET_PED_DROPS_WEAPON(Player.Character);
                    GameFiber.Wait(5000);//was just 6000 here
                    if (!Player.Character.Exists() || !Player.IsBusted)
                    {
                        return;
                    }
                    NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, "busted", "idle_a", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                }
            }
            NativeFunction.Natives.SET_PED_KEEP_TASK(Player.Character, true);
            //Player.Character.KeepTasks = true;
        }, "SetArrestedAnimation");
    }
    public void UnSetArrestedAnimation()
    {
        GameFiber UnSetArrestedAnimationGF = GameFiber.StartNew(delegate
        {
            AnimationDictionary.RequestAnimationDictionay("random@arrests");
            AnimationDictionary.RequestAnimationDictionay("busted");
            AnimationDictionary.RequestAnimationDictionay("ped");
            if (NativeFunction.Natives.IS_ENTITY_PLAYING_ANIM<bool>(Player.Character, "busted", "idle_a", 3) || NativeFunction.Natives.GET_ENTITY_ANIM_CURRENT_TIME<float>(Player.Character, "busted", "idle_a") > 0f 
              || NativeFunction.Natives.IS_ENTITY_PLAYING_ANIM<bool>(Player.Character, "busted", "idle_2_hands_up", 3) || NativeFunction.Natives.GET_ENTITY_ANIM_CURRENT_TIME<float>(Player.Character, "busted", "idle_2_hands_up") > 0f)//do both to cover my bases, is for player anysways so can be expensive to be right
            {
                NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, "busted", "hands_up_2_idle", 4.0f, -4.0f, -1, 4096, 0, 0, 1, 0);
                GameFiber.Wait(1500);//1250
                if (!Player.Character.Exists() || !Player.IsBusted)
                {
                    return;
                }
                NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            }
            else if (NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", Player.Character, "ped", "handsup_enter", 3))
            {
                NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            }
        }, "UnSetArrestedAnimation");
    }
    //public void SetInPoliceCar()
    //{
    //    GameFiber SetPlayerInPoliceCarGF = GameFiber.StartNew(delegate
    //    {
    //        Player.ButtonPrompts.Add(new ButtonPrompt("Skip Ride", "Surrender", "PlayerSkipRide", System.Windows.Forms.Keys.O, 1));
    //        while (!Player.IsInVehicle && !Player.ButtonPrompts.Any(x => x.Identifier == "PlayerSkipRide" && x.IsPressedNow))
    //        {
    //            if (VehicleTaskedToEnter == null || !VehicleTaskedToEnter.Exists())
    //            {
    //                GetClosesetPoliceVehicle();
    //                EntryPoint.WriteToConsole($"PlayerArrested: Get in Car, Got New Car, was Blank", 3);
    //                GetInCarTask();
    //            }
    //            else if (VehicleTryingToEnter != null && VehicleTaskedToEnter.Exists() && !VehicleTaskedToEnter.IsSeatFree(SeatTaskedToEnter) && VehicleTaskedToEnter.GetPedOnSeat(SeatTaskedToEnter).Exists() && VehicleTaskedToEnter.GetPedOnSeat(SeatTaskedToEnter).Handle != Player.Character.Handle)// && (VehicleTryingToEnter.Vehicle.Handle != VehicleTaskedToEnter.Handle || SeatTaskedToEnter != SeatTryingToEnter) && Ped.Pedestrian.Exists() && !Ped.Pedestrian.IsInAnyVehicle(true))
    //            {
    //                GetClosesetPoliceVehicle();
    //                EntryPoint.WriteToConsole($"PlayerArrested: Get in Car Got New Car, was occupied?", 3);
    //                GetInCarTask();
    //            }
    //            else if (VehicleTryingToEnter != null && VehicleTaskedToEnter.Exists() && VehicleTaskedToEnter.Speed > 1.0f)// && (VehicleTryingToEnter.Vehicle.Handle != VehicleTaskedToEnter.Handle || SeatTaskedToEnter != SeatTryingToEnter) && Ped.Pedestrian.Exists() && !Ped.Pedestrian.IsInAnyVehicle(true))
    //            {
    //                GetClosesetPoliceVehicle();
    //                EntryPoint.WriteToConsole($"PlayerArrested: Get in Car Got New Car, was driving away?", 3);
    //                GetInCarTask();
    //            }
    //            GameFiber.Yield();
    //        }
    //        Player.ButtonPrompts.RemoveAll(x => x.Group == "Surrender");
    //        Player.SurrenderToPolice(null);
    //    }, "SetPlayerInPoliceCarGF");
    //}
    //private void GetInCarTask()
    //{
    //    if (Player.Character.Exists() && VehicleTryingToEnter != null && VehicleTryingToEnter.Vehicle.Exists())
    //    {
    //        EntryPoint.WriteToConsole($"GetArrested: Get in Car TASK START", 3);
    //        Player.Character.BlockPermanentEvents = true;
    //        Player.Character.KeepTasks = true;
    //        VehicleTaskedToEnter = VehicleTryingToEnter.Vehicle;
    //        SeatTaskedToEnter = SeatTryingToEnter;
    //        unsafe
    //        {
    //            int lol = 0;
    //            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
    //            NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", 0, VehicleTryingToEnter.Vehicle, -1, SeatTryingToEnter, 1f, 9);
    //            NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(4000, 8000));
    //            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
    //            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
    //            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Player.Character, lol);
    //            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
    //        }
    //    }
    //}
    //private void GetClosesetPoliceVehicle()
    //{
    //    VehicleExt ClosestAvailablePoliceVehicle = null;
    //    int OpenSeatInClosestAvailablePoliceVehicle = 9;
    //    float ClosestAvailablePoliceVehicleDistance = 999f;
    //    foreach (VehicleExt copCar in World.PoliceVehicleList)
    //    {
    //        if (copCar.Vehicle.Exists() && copCar.Vehicle.Model.NumberOfSeats >= 4 && copCar.Vehicle.Speed == 0f)//stopped 4 door car with at least one seat free in back
    //        {
    //            float DistanceTo = copCar.Vehicle.DistanceTo2D(Player.Character);
    //            if (DistanceTo <= 50f)
    //            {
    //                if (copCar.Vehicle.IsSeatFree(1))
    //                {
    //                    if (DistanceTo < ClosestAvailablePoliceVehicleDistance)
    //                    {
    //                        OpenSeatInClosestAvailablePoliceVehicle = 1;
    //                        ClosestAvailablePoliceVehicle = copCar;
    //                        ClosestAvailablePoliceVehicleDistance = DistanceTo;
    //                    }

    //                }
    //                else if (copCar.Vehicle.IsSeatFree(2))
    //                {
    //                    if (DistanceTo < ClosestAvailablePoliceVehicleDistance)
    //                    {
    //                        OpenSeatInClosestAvailablePoliceVehicle = 2;
    //                        ClosestAvailablePoliceVehicle = copCar;
    //                        ClosestAvailablePoliceVehicleDistance = DistanceTo;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    VehicleTryingToEnter = ClosestAvailablePoliceVehicle;
    //    SeatTryingToEnter = OpenSeatInClosestAvailablePoliceVehicle;
    //    if (ClosestAvailablePoliceVehicle != null && ClosestAvailablePoliceVehicle.Vehicle.Exists())
    //    {
    //        EntryPoint.WriteToConsole($"PlayerArrested: Seat Assigned Vehicle {VehicleTryingToEnter.Vehicle.Handle} Seat {SeatTryingToEnter}", 3);
    //    }
    //    else
    //    {
    //        EntryPoint.WriteToConsole($"PlayerArrested: Seat NOT Assigned", 3);
    //    }
    //}
}