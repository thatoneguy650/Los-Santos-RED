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
using LosSantosRED.lsr.Interface;

public class CarLockPick
{
    private ICarStealable Player;
    private string Animation = "std_force_entry_ds";
    private int DoorIndex = 0;
    private int WaitTime = 1750;
    private VehicleLockStatus OriginalLockStatus;
    private Rage.Object Screwdriver;
    private Vehicle TargetVehicle;
    private int SeatTryingToEnter;

    public CarLockPick(ICarStealable player, Vehicle targetVehicle, int seatTryingToEnter)
    {
        Player = player;
        TargetVehicle = targetVehicle;
        SeatTryingToEnter = seatTryingToEnter;
    }

    private bool CanLockPick
    {
        get
        {
            if (!TargetVehicle.Exists())
            {
                return false;
            }
            int intVehicleClass = NativeFunction.CallByName<int>("GET_VEHICLE_CLASS", TargetVehicle);
            VehicleClass VehicleClass = (VehicleClass)intVehicleClass;
            //EntryPoint.WriteToConsole($"PLAYER EVENT: LockPick VehicleClass {VehicleClass}", 3);
            if (VehicleClass == VehicleClass.Boat || VehicleClass == VehicleClass.Cycle || VehicleClass == VehicleClass.Industrial || VehicleClass == VehicleClass.Motorcycle || VehicleClass == VehicleClass.Plane || VehicleClass == VehicleClass.Service || VehicleClass == VehicleClass.Helicopter)
            {
                return false;//maybe add utility?
            }
            else if (!TargetVehicle.Doors[0].IsValid() || !TargetVehicle.Doors[1].IsValid())
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public void PickLock()
    {
        if (!CanLockPick)
            return;
        try
        {
            GameFiber UnlockCarDoor = GameFiber.StartNew(delegate
            {
                try
                {
                    GameFiber.Yield();
                    if (!SetupLockPick())
                    {
                        //EntryPoint.WriteToConsoleTestLong("PickLock Setup Failed");
                        return;
                    }
                    GameFiber.Yield();
                    if (!LockPickAnimation())
                    {
                        //EntryPoint.WriteToConsoleTestLong("PickLock Animation Failed");
                        return;
                    }
                    GameFiber.Yield();
                    FinishLockPick();
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "PickLock");
        }
        catch (Exception e)
        {
            Player.IsLockPicking = false;
            EntryPoint.WriteToConsole("PickLock" + e.Message + e.StackTrace,0);
        }
    }
    private bool SetupLockPick()
    {
        OriginalLockStatus = TargetVehicle.LockStatus;
        TargetVehicle.SetLock((VehicleLockStatus)3);//Attempt to lock most car doors
        Player.WeaponEquipment.SetUnarmed();
        if(TargetVehicle.LockStatus != (VehicleLockStatus)3)
        {
            //EntryPoint.WriteToConsoleTestLong($"SetupLockPick Failed, Could Not Set Lock Status to 3 Current Status {(int)TargetVehicle.LockStatus}");//some IV pack cars fail even with the door open.....
            NativeFunction.Natives.TASK_ENTER_VEHICLE(Player.Character, TargetVehicle, -1, SeatTryingToEnter, 2.0f, 1, 0);
            return false;
        }
        TargetVehicle.MustBeHotwired = true;
        uint GameTimeStartedStealing = Game.GameTime;
        bool StartAnimation = true;
        while (Game.LocalPlayer.Character.IsGettingIntoVehicle && Game.GameTime - GameTimeStartedStealing <= 3500)
        {
            if (Player.IsMoveControlPressed)
            {
                StartAnimation = false;
                break;
            }
            GameFiber.Yield();
        }
        if (!StartAnimation)
        {
            TargetVehicle.LockStatus = OriginalLockStatus;
            //EntryPoint.WriteToConsoleTestLong("SetupLockPick Failed, Move Control Pressed");
            return false;
        }
        if (TargetVehicle.LockStatus == (VehicleLockStatus)1)
        {
            //EntryPoint.WriteToConsoleTestLong("SetupLockPick Failed, Lock Status = 1");
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
        Player.IsLockPicking = true;
        bool Continue = true;
        //EntryPoint.WriteToConsoleTestLong($"LOCK PICK ENTRY: LockPickAnimation START");
        Screwdriver = AttachScrewdriverToPed(Game.LocalPlayer.Character);

        AnimationDictionary.RequestAnimationDictionay("veh@break_in@0h@p_m_one@");
        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "veh@break_in@0h@p_m_one@", Animation, 2.0f, -2.0f, -1, 0, 0, false, false, false);

        uint GameTimeStarted = Game.GameTime;
        while (Game.GameTime - GameTimeStarted <= WaitTime)
        {
            GameFiber.Yield();
            if (Player.IsMoveControlPressed || TargetVehicle.Doors[DoorIndex].IsOpen)
            {
                //EntryPoint.WriteToConsoleTestLong($"LOCK PICK ENTRY: {Player.IsMoveControlPressed} TargetVehicle.Doors[DoorIndex].IsOpen {TargetVehicle.Doors[DoorIndex].IsOpen}");
                Continue = false;
                break;
            }
        }
        Player.IsLockPicking = false;
        if (!Continue)
        {
            //Game.LocalPlayer.Character.Tasks.Clear();
            NativeFunction.Natives.CLEAR_PED_TASKS(Game.LocalPlayer.Character);
            if (Screwdriver != null && Screwdriver.Exists())
            {
                Screwdriver.Delete();
            }
            Player.IsLockPicking = false;
            TargetVehicle.LockStatus = OriginalLockStatus;
            //EntryPoint.WriteToConsoleTestLong($"LOCK PICK ENTRY: CANNOT CONTINUE");
            return false;
        }

        TargetVehicle.LockStatus = VehicleLockStatus.Unlocked;

        if (RandomItems.RandomPercent(50))
        {
            TargetVehicle.Doors[DoorIndex].Open(true, false);
        }
        //EntryPoint.WriteToConsoleTestLong($"LOCK PICK ENTRY: LockPickAnimation FINISH TRUE");
        return true;
    }
    private Rage.Object AttachScrewdriverToPed(Ped Pedestrian)
    {
        //Vector3 position = Pedestrian.GetOffsetPositionUp(50f);
        //Model modelToCreate = new Model(Game.GetHashKey("prop_tool_screwdvr01"));
        //modelToCreate.LoadAndWait();
        //Rage.Object Screwdriver = NativeFunction.Natives.CREATE_OBJECT<Rage.Object>(Game.GetHashKey("prop_tool_screwdvr01"), position.X, position.Y, position.Z, 0f);//
        Rage.Object Screwdriver = null;
        try
        {
            Screwdriver = new Rage.Object("prop_tool_screwdvr01", Pedestrian.GetOffsetPositionUp(50f));
        }
        catch (Exception ex)
        {
            //EntryPoint.WriteToConsoleTestLong($"Error Spawning Model {ex.Message} {ex.StackTrace}");
        }
        if (!Screwdriver.Exists())
        {
            return null;
        }
        int BoneIndexRightHand = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Pedestrian, 57005);
        Screwdriver.AttachTo(Pedestrian, BoneIndexRightHand, new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f));
        return Screwdriver;
    }
    private bool FinishLockPick()
    {
        //Game.LocalPlayer.Character.Tasks.EnterVehicle(TargetVehicle, SeatTryingToEnter);
        NativeFunction.Natives.TASK_ENTER_VEHICLE(Player.Character, TargetVehicle, -1, SeatTryingToEnter, 2.0f, 1, 0);
        if (Screwdriver != null && Screwdriver.Exists())
        {
            Screwdriver.Delete();
        }

        Player.IsLockPicking = false;

        return true;
    }
}

