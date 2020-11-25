using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionsMethods;


public static class CarLockPicking
{
    private static string Animation = "std_force_entry_ds";
    private static int DoorIndex = 0;
    private static int WaitTime = 1750;
    private static VehicleLockStatus OriginalLockStatus;
    private static Rage.Object Screwdriver;
    private static Vehicle TargetVehicle;
    private static int SeatTryingToEnter;
    private static bool CanLockPick
    {
        get
        {
            if (!TargetVehicle.Exists())
                return false;
            int intVehicleClass = NativeFunction.CallByName<int>("GET_VEHICLE_CLASS", TargetVehicle);
            VehicleClass VehicleClass = (VehicleClass)intVehicleClass;
            if (VehicleClass == VehicleClass.Boats || VehicleClass == VehicleClass.Cycles || VehicleClass == VehicleClass.Industrial || VehicleClass == VehicleClass.Motorcycles
                || VehicleClass == VehicleClass.Planes || VehicleClass == VehicleClass.Service || VehicleClass == VehicleClass.Trailer || VehicleClass == VehicleClass.Trains
                || VehicleClass == VehicleClass.Helicopters)
                return false;//maybe add utility?
            else if (!TargetVehicle.Doors[0].IsValid() || !TargetVehicle.Doors[1].IsValid())
                return false;
            else
                return true;
        }
    }

    public static bool PlayerLockPicking { get; set; } = false;
    public static void PickLock(Vehicle VehicleToEnter, int EntrySeat)
    {
        TargetVehicle = VehicleToEnter;
        SeatTryingToEnter = EntrySeat;
        if (!PlayerState.IsHoldingEnter || !CanLockPick)
            return;

        try
        {
            GameFiber UnlockCarDoor = GameFiber.StartNew(delegate
            {
                if (!SetupLockPick())
                    return;

                if (!LockPickAnimation())
                    return;

                FinishLockPick();

            }, "PickLock");
            Debugging.GameFibers.Add(UnlockCarDoor);
        }
        catch (Exception e)
        {
            PlayerLockPicking = false;
            Debugging.WriteToLog("PickLock", e.Message);
        }
    }
    private static bool SetupLockPick()
    {
        OriginalLockStatus = TargetVehicle.LockStatus;
        General.AttemptLockStatus(TargetVehicle, (VehicleLockStatus)3);//Attempt to lock most car doors
        General.SetPedUnarmed(Game.LocalPlayer.Character, false);

        TargetVehicle.MustBeHotwired = true;
        uint GameTimeStartedStealing = Game.GameTime;
        bool StartAnimation = true;

        while (Game.LocalPlayer.Character.IsGettingIntoVehicle && Game.GameTime - GameTimeStartedStealing <= 3500)
        {
            if (Extensions.IsMoveControlPressed())
            {
                StartAnimation = false;
                break;
            }
            GameFiber.Yield();
        }

        if (!StartAnimation)
        {
            TargetVehicle.LockStatus = OriginalLockStatus;
            return false;
        }

        if (TargetVehicle.LockStatus == (VehicleLockStatus)1)
        {
            return false;
        }

        if (TargetVehicle.HasBone("door_dside_f") && TargetVehicle.HasBone("door_pside_f"))
        {
            if (Game.LocalPlayer.Character.DistanceTo2D(TargetVehicle.GetBonePosition("door_dside_f")) > Game.LocalPlayer.Character.DistanceTo2D(TargetVehicle.GetBonePosition("door_pside_f")))
            {
                Animation = "std_force_entry_ps";
                DoorIndex = 1;
                WaitTime = 2200;
            }
            else
            {
                Animation = "std_force_entry_ds";
                DoorIndex = 0;
                WaitTime = 1750;
            }
        }
        return true;
    }   
    private static bool LockPickAnimation()
    {
        PlayerLockPicking = true;
        bool Continue = true;

        Screwdriver = General.AttachScrewdriverToPed(Game.LocalPlayer.Character);

        General.RequestAnimationDictionay("veh@break_in@0h@p_m_one@");
        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "veh@break_in@0h@p_m_one@", Animation, 2.0f, -2.0f, -1, 0, 0, false, false, false);

        uint GameTimeStarted = Game.GameTime;
        while (Game.GameTime - GameTimeStarted <= WaitTime)
        {
            GameFiber.Yield();
            if (Extensions.IsMoveControlPressed())
            {
                Continue = false;
                break;
            }
        }

        if (!Continue)
        {
            Game.LocalPlayer.Character.Tasks.Clear();
            if (Screwdriver != null && Screwdriver.Exists())
                Screwdriver.Delete();
            PlayerLockPicking = false;
            TargetVehicle.LockStatus = OriginalLockStatus;
            return false;
        }

        TargetVehicle.LockStatus = VehicleLockStatus.Unlocked;
        TargetVehicle.Doors[DoorIndex].Open(true, false);

        return true;
    }
    private static bool FinishLockPick()
    {
        uint GameTimeStarted = Game.GameTime;
        Game.LocalPlayer.Character.Tasks.EnterVehicle(TargetVehicle, SeatTryingToEnter);
        while (!Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.GameTime - GameTimeStarted <= 10000)
        {
            GameFiber.Yield();
            if (Extensions.IsMoveControlPressed())
            {
                break;
            }
        }

        if (TargetVehicle.Doors[DoorIndex].IsValid())
            NativeFunction.CallByName<bool>("SET_VEHICLE_DOOR_CONTROL", TargetVehicle, DoorIndex, 4, 0f);
        if (DoorIndex == 0)//Driver side
            GameFiber.Sleep(5000);
        else
            GameFiber.Sleep(8000);//Passenger takes longer
        if (Screwdriver != null && Screwdriver.Exists())
            Screwdriver.Delete();

        PlayerLockPicking = false;

        return true;
    }
}

