using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionsMethods;
using LSR.Vehicles;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using Mod;

public class CarLockPick
{
    private string Animation = "std_force_entry_ds";
    private int DoorIndex = 0;
    private int WaitTime = 1750;
    private VehicleLockStatus OriginalLockStatus;
    private Rage.Object Screwdriver;
    private Vehicle TargetVehicle;
    private int SeatTryingToEnter;

    public CarLockPick(Vehicle targetVehicle, int seatTryingToEnter)
    {
        TargetVehicle = targetVehicle;
        SeatTryingToEnter = seatTryingToEnter;
    }

    private bool CanLockPick
    {
        get
        {
            if (!TargetVehicle.Exists())
                return false;
            int intVehicleClass = NativeFunction.CallByName<int>("GET_VEHICLE_CLASS", TargetVehicle);
            VehicleClass VehicleClass = (VehicleClass)intVehicleClass;
            if (VehicleClass == VehicleClass.Boat || VehicleClass == VehicleClass.Cycle || VehicleClass == VehicleClass.Industrial || VehicleClass == VehicleClass.Motorcycle|| VehicleClass == VehicleClass.Plane || VehicleClass == VehicleClass.Service || VehicleClass == VehicleClass.Helicopter)
                return false;//maybe add utility?
            else if (!TargetVehicle.Doors[0].IsValid() || !TargetVehicle.Doors[1].IsValid())
                return false;
            else
                return true;
        }
    }

    public void PickLock()
    {
        if (!Input.Instance.IsHoldingEnter || !CanLockPick)
            return;
        try
        {
            GameFiber UnlockCarDoor = GameFiber.StartNew(delegate
            {
                if (!SetupLockPick())
                {
                    Debug.Instance.WriteToLog("PickLock", "Setup Failed");
                    return;
                }

                if (!LockPickAnimation())
                {
                    Debug.Instance.WriteToLog("PickLock", "Animation Failed");
                    return;
                }

                FinishLockPick();

            }, "PickLock");
            Debug.Instance.GameFibers.Add(UnlockCarDoor);
        }
        catch (Exception e)
        {
            Mod.Player.Instance.IsLockPicking = false;
            Debug.Instance.WriteToLog("PickLock", e.Message);
        }
    }
    private bool SetupLockPick()
    {
        OriginalLockStatus = TargetVehicle.LockStatus;
        TargetVehicle.SetLock((VehicleLockStatus)3);//Attempt to lock most car doors
        Mod.Player.Instance.SetUnarmed();

        TargetVehicle.MustBeHotwired = true;
        uint GameTimeStartedStealing = Game.GameTime;
        bool StartAnimation = true;

        while (Game.LocalPlayer.Character.IsGettingIntoVehicle && Game.GameTime - GameTimeStartedStealing <= 3500)
        {
            if (Input.Instance.IsMoveControlPressed)
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
    private bool LockPickAnimation()
    {
        Mod.Player.Instance.IsLockPicking = true;
        bool Continue = true;

        Screwdriver = AttachScrewdriverToPed(Game.LocalPlayer.Character);

        AnimationDictionary AnimDictionary = new AnimationDictionary("veh@break_in@0h@p_m_one@");
        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "veh@break_in@0h@p_m_one@", Animation, 2.0f, -2.0f, -1, 0, 0, false, false, false);

        uint GameTimeStarted = Game.GameTime;
        while (Game.GameTime - GameTimeStarted <= WaitTime)
        {
            GameFiber.Yield();
            if (Input.Instance.IsMoveControlPressed)
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
            Mod.Player.Instance.IsLockPicking = false;
            TargetVehicle.LockStatus = OriginalLockStatus;
            return false;
        }

        TargetVehicle.LockStatus = VehicleLockStatus.Unlocked;

        if (RandomItems.RandomPercent(50))
        {
            TargetVehicle.Doors[DoorIndex].Open(true, false);
        }

        return true;
    }
    private Rage.Object AttachScrewdriverToPed(Ped Pedestrian)
    {
        Rage.Object Screwdriver = new Rage.Object("prop_tool_screwdvr01", Pedestrian.GetOffsetPositionUp(50f));
        if (!Screwdriver.Exists())
            return null;
        int BoneIndexRightHand = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Pedestrian, 57005);
        Screwdriver.AttachTo(Pedestrian, BoneIndexRightHand, new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f));
        return Screwdriver;
    }
    private bool FinishLockPick()
    {
       // uint GameTimeStarted = Game.GameTime;
        Game.LocalPlayer.Character.Tasks.EnterVehicle(TargetVehicle, SeatTryingToEnter);

        //while (!Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.GameTime - GameTimeStarted <= 10000)
        //{
        //    GameFiber.Yield();
        //    if (Extensions.IsMoveControlPressed())
        //    {
        //        break;
        //    }
        //}

        //if (TargetVehicle.Doors[DoorIndex].IsValid())
        //{
        //    NativeFunction.CallByName<bool>("SET_VEHICLE_DOOR_CONTROL", TargetVehicle, DoorIndex, 4, 0f);
        //}

        //if (DoorIndex == 0)//Driver side
        //{
        //    GameFiber.Sleep(5000);
        //}
        //else
        //{
        //    GameFiber.Sleep(8000);//Passenger takes longer
        //}
        if (Screwdriver != null && Screwdriver.Exists())
            Screwdriver.Delete();

        Mod.Player.Instance.IsLockPicking = false;

        return true;
    }
}

