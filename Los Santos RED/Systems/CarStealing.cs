﻿using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionsMethods;


public static class CarStealing
{

    private static uint GameTimeLastTriedCarJacking;
    private static Random rnd;

    public static bool PlayerBreakingIntoCar { get; set; } = false;


    static CarStealing()
    {
        rnd = new Random();
    }
    public static void UnlockCarDoor(Vehicle TargetVehicle, int SeatTryingToEnter)
    {
        //if (!Game.IsControlPressed(2, GameControl.Enter))//holding enter go thru normal
        //    return;
        if (!LosSantosRED.PlayerHoldingEnter)
        {
            return;
        }

        if (TargetVehicle.Exists() && (TargetVehicle.IsBike || TargetVehicle.IsBoat || TargetVehicle.IsHelicopter || TargetVehicle.IsPlane || TargetVehicle.IsBicycle))
            return;

        if (!CanLockPick(TargetVehicle))
            return;

        try
        {
            GameFiber UnlockCarDoor = GameFiber.StartNew(delegate
            {


                VehicleLockStatus LockStatusBefore = TargetVehicle.LockStatus;


                LockCarDoor(TargetVehicle, (VehicleLockStatus)3);//Attempt to lock most car doors


                LosSantosRED.SetPedUnarmed(Game.LocalPlayer.Character, false);
                bool Continue = true;
                TargetVehicle.MustBeHotwired = true;
                string AnimationName = "std_force_entry_ds";
                int DoorIndex = 0;
                int WaitTime = 1750;
                float DesiredHeading = TargetVehicle.Heading - 90f;



                Debugging.WriteToLog("UnlockCarDoor", "Pre Sleep");

                uint GameTimeStartedStealing = Game.GameTime;
                bool StartAnimation = true;
                while(Game.LocalPlayer.Character.IsGettingIntoVehicle && Game.GameTime - GameTimeStartedStealing <= 3500)
                {
                    if (Extensions.IsMoveControlPressed())
                    {
                        StartAnimation = false;
                        break;
                    }
                    GameFiber.Yield();
                }

                if(!StartAnimation)
                {
                    TargetVehicle.LockStatus = LockStatusBefore;
                    return;
                }


                if(TargetVehicle.LockStatus == (VehicleLockStatus)1)
                {
                    Debugging.WriteToLog("UnlockCarDoor", "Vehicle was unlocked?");
                    return;
                }
                Debugging.WriteToLog("UnlockCarDoor", "Post Sleep");

                if (TargetVehicle.HasBone("door_dside_f") && TargetVehicle.HasBone("door_pside_f"))
                {
                    if (Game.LocalPlayer.Character.DistanceTo2D(TargetVehicle.GetBonePosition("door_dside_f")) > Game.LocalPlayer.Character.DistanceTo2D(TargetVehicle.GetBonePosition("door_pside_f")))
                    {
                        AnimationName = "std_force_entry_ps";
                        DoorIndex = 1;
                        WaitTime = 2200;
                        DesiredHeading = TargetVehicle.Heading + 90f;
                    }
                    else
                    {
                        AnimationName = "std_force_entry_ds";
                        DoorIndex = 0;
                        WaitTime = 1750;
                    }
                }


                //LosSantosRED.MovePedToCarPosition(TargetVehicle, Game.LocalPlayer.Character, DesiredHeading, Game.LocalPlayer.Character.Position, false);
                //Vector3 CameraPosition = GetCameraPosition(TargetVehicle, DoorIndex == 0);
                //CameraSystem.TransitionToAltCam(TargetVehicle, CameraPosition);              

                Debugging.WriteToLog("UnlockCarDoor", string.Format("DoorIndex: {0},AnimationName: {1}", DoorIndex, AnimationName));
                Rage.Object Screwdriver = LosSantosRED.AttachScrewdriverToPed(Game.LocalPlayer.Character);
                LosSantosRED.RequestAnimationDictionay("veh@break_in@0h@p_m_one@");
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "veh@break_in@0h@p_m_one@", AnimationName, 2.0f, -2.0f, -1, 0, 0, false, false, false);

                PlayerBreakingIntoCar = true;

                uint GameTimeStarted = Game.GameTime;
                while (Game.GameTime - GameTimeStarted <= WaitTime)
                {
                    GameFiber.Yield();
                    if (Extensions.IsMoveControlPressed())
                    {
                        Continue = false;
                        //if (CameraSystem.UsingOtherCamera)
                            //CameraSystem.UnHighLightCarjacking(ToEnter, DoorIndex == 0);
                        break;

                    }
                }

                if (!Continue)
                {
                    Game.LocalPlayer.Character.Tasks.Clear();
                    if (Screwdriver != null && Screwdriver.Exists())
                        Screwdriver.Delete();
                    PlayerBreakingIntoCar = false;
                    TargetVehicle.LockStatus = LockStatusBefore;
                    return;
                }

                TargetVehicle.LockStatus = VehicleLockStatus.Unlocked;
                TargetVehicle.Doors[DoorIndex].Open(true, false);

                //GameFiber.Sleep(500);

                Debugging.WriteToLog("UnlockCarDoor", string.Format("Open Door: {0}", DoorIndex));
                GameTimeStarted = Game.GameTime;
                Game.LocalPlayer.Character.Tasks.EnterVehicle(TargetVehicle, SeatTryingToEnter);
                while (!Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.GameTime - GameTimeStarted <= 10000)
                {
                    GameFiber.Yield();
                    if (Extensions.IsMoveControlPressed())
                    {
                        Continue = false;
                        //if (CameraSystem.UsingOtherCamera)
                            //CameraSystem.UnHighLightCarjacking(ToEnter, DoorIndex == 0);
                        break;
                    }
                }
                //if (CameraSystem.UsingOtherCamera)
                    //CameraSystem.UnHighLightCarjacking(ToEnter, DoorIndex == 0);

                if (TargetVehicle.Doors[DoorIndex].IsValid())
                    NativeFunction.CallByName<bool>("SET_VEHICLE_DOOR_CONTROL", TargetVehicle, DoorIndex, 4, 0f);
                if(DoorIndex == 0)//Driver side
                    GameFiber.Sleep(5000);
                else
                    GameFiber.Sleep(8000);//Passengfer takes longer
                if (Screwdriver != null && Screwdriver.Exists())
                    Screwdriver.Delete();
                PlayerBreakingIntoCar = false;
                Debugging.WriteToLog("UnlockCarDoor", string.Format("Made it to the end: {0}", SeatTryingToEnter));

                
            }, "UnlockCarDoor");
            Debugging.GameFibers.Add(UnlockCarDoor);
        }
        catch (Exception e)
        {
            PlayerBreakingIntoCar = false;
            Debugging.WriteToLog("UnlockCarDoor", e.Message);
        }
    }
    private static Vector3 GetCameraPosition(Vehicle VehicleToLookAt, bool IsDriverSide)
    {
        Vector3 CameraPosition;
        if (IsDriverSide)
            CameraPosition = VehicleToLookAt.GetOffsetPositionRight(-6f);
        else
            CameraPosition = VehicleToLookAt.GetOffsetPositionRight(6f);
        CameraPosition += new Vector3(0f, 0f, 1.8f);
        return CameraPosition;
    }
    public static void LockCarDoor(Vehicle ToLock, VehicleLockStatus DesiredLockStatus)
    {
        Debugging.WriteToLog("LockCarDoor", string.Format("Start, Lock Status {0}", ToLock.LockStatus));
        if (ToLock.LockStatus != (VehicleLockStatus)1 && ToLock.LockStatus != (VehicleLockStatus)7)//if (ToLock.LockStatus != (VehicleLockStatus)1) //unlocked
            return;
        //Debugging.WriteToLog("LockCarDoor", "1");
        if (ToLock.HasDriver)//If they have a driver 
            return;
        //Debugging.WriteToLog("LockCarDoor", "2");
        foreach (VehicleDoor myDoor in ToLock.GetDoors())
        {
            if (!myDoor.IsValid() || myDoor.IsOpen)
                return;//invalid doors make the car not locked
        }
        //Debugging.WriteToLog("LockCarDoor", "3");
        if (!NativeFunction.CallByName<bool>("ARE_ALL_VEHICLE_WINDOWS_INTACT", ToLock))
            return;//broken windows == not locked
        //Debugging.WriteToLog("LockCarDoor", "4");
        if (LosSantosRED.TrackedVehicles.Any(x => x.VehicleEnt.Handle == ToLock.Handle))
            return; //previously entered vehicle arent locked
        if (ToLock.IsConvertible && ToLock.ConvertibleRoofState == VehicleConvertibleRoofState.Lowered)
            return;
        if (ToLock.IsBike || ToLock.IsPlane || ToLock.IsHelicopter)
            return;

        Debugging.WriteToLog("LockCarDoor", "Locked");
        ToLock.LockStatus = DesiredLockStatus;//Locked for player
        //ToLock.LockStatus = (VehicleLockStatus)7;//CanBeBrokenInto
    }
    public static bool CanLockPick(Vehicle ToEnter)
    {
        int intVehicleClass = NativeFunction.CallByName<int>("GET_VEHICLE_CLASS", ToEnter);
        Vehicles.VehicleClass VehicleClass = (Vehicles.VehicleClass)intVehicleClass;
        if (VehicleClass == Vehicles.VehicleClass.Boats || VehicleClass == Vehicles.VehicleClass.Cycles || VehicleClass == Vehicles.VehicleClass.Industrial || VehicleClass == Vehicles.VehicleClass.Motorcycles 
            || VehicleClass == Vehicles.VehicleClass.Planes || VehicleClass == Vehicles.VehicleClass.Service || VehicleClass == Vehicles.VehicleClass.Trailer || VehicleClass == Vehicles.VehicleClass.Trains 
            || VehicleClass == Vehicles.VehicleClass.Helicopters)
            return false;//maybe add utility?
        else if(!ToEnter.Doors[0].IsValid() || !ToEnter.Doors[1].IsValid())
            return false;
        else
            return true;
    }
    public static void EnterVehicleEvent()
    {
        Vehicle TargetVeh = Game.LocalPlayer.Character.VehicleTryingToEnter;
        int SeatTryingToEnter = Game.LocalPlayer.Character.SeatIndexTryingToEnter;
        LockCarDoor(TargetVeh,(VehicleLockStatus)7);//Attempt to lock most car doors
        int LockStatus = (int)TargetVeh.LockStatus;//Get the result of the function


        Debugging.WriteToLog("EnterVehicleEvent", string.Format("Lockstatus: {0}", LockStatus));
        //if (LockStatus == 7)//Locked but can be broken into
        //{
        //    UnlockCarDoor(TargetVeh, SeatTryingToEnter);
        //}
        if (LockStatus == 7)//Locked For Player
        {
            UnlockCarDoor(TargetVeh, SeatTryingToEnter);
        }

        if (TargetVeh != null && SeatTryingToEnter == -1)
        {
            Ped Driver = TargetVeh.Driver;
            if (Driver != null && Driver.IsAlive)
            {
                CarJackPed(TargetVeh, Driver, SeatTryingToEnter);
                Debugging.WriteToLog("EnterVehicle", "CarJacking");
            }
            else
            {
                Debugging.WriteToLog("EnterVehicle", "Regular Enter No Driver");
            }
        }
        else
        {
            Debugging.WriteToLog("EnterVehicle", "Regular Enter");
        }
    }
    public static void UpdateStolenStatus()
    {
        GTAVehicle MyVehicle = LosSantosRED.GetPlayersCurrentTrackedVehicle();
        if (MyVehicle == null || MyVehicle.IsStolen)
            return;

        if (PedSwapping.OwnedCar == null)
            MyVehicle.IsStolen = true;
        else if (MyVehicle.VehicleEnt.Handle != PedSwapping.OwnedCar.Handle && !MyVehicle.IsStolen)
            MyVehicle.IsStolen = true;
    }
    public static Vector3 GetHandlePosition(Vehicle TargetVehicle,string Bone)
    {
        Vector3 GameEntryPosition = Vector3.Zero;
        if (TargetVehicle.HasBone(Bone))
        {
            GameEntryPosition = TargetVehicle.GetBonePosition(Bone);
        }
        return GameEntryPosition;
    }
    public static Vector3 GetEntryPosition(Vehicle TargetVehicle)
    {
        return NativeFunction.CallByHash<Vector3>(0xC0572928C0ABFDA3, TargetVehicle, 0);
    }
    public static void CarJackPed(Vehicle TargetVehicle, Ped Driver, int SeatTryingToEnter)
    {
        if (SeatTryingToEnter != -1)
            return;

        if (TargetVehicle.HasBone("door_dside_f") && TargetVehicle.HasBone("door_pside_f"))
        {
            if (Game.LocalPlayer.Character.DistanceTo2D(TargetVehicle.GetBonePosition("door_dside_f")) > Game.LocalPlayer.Character.DistanceTo2D(TargetVehicle.GetBonePosition("door_pside_f")))
            {
                return;//Closer to passenger side, animations dont work
            }
        }
        GTAWeapon myGun = LosSantosRED.GetCurrentWeapon();
        if (LosSantosRED.PlayerHoldingEnter && Game.GameTime - GameTimeLastTriedCarJacking > 500 && myGun != null && myGun.Category != GTAWeapon.WeaponCategory.Melee)
        {
            Debugging.WriteToLog("CarJackPed", "CarJackPedWithWeapon");
            CarJackPedWithWeapon(TargetVehicle, Driver, SeatTryingToEnter, myGun);
        }
        else
        {
            Debugging.WriteToLog("CarJackPed", "CarJackPedRegular");
            CarJackPedRegular(TargetVehicle, Driver);
        }


    }
    public static void CarJackPedWithWeapon(Vehicle TargetVehicle, Ped Driver, int SeatTryingToEnter,GTAWeapon myGun)
    {
        try
        {
            GameFiber CarJackPedWithWeapon = GameFiber.StartNew(delegate
            {
                LosSantosRED.SetPlayerToLastWeapon();
                NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", Driver, TargetVehicle, 27, -1);
                Driver.BlockPermanentEvents = true;

                Vector3 GameEntryPosition = GetEntryPosition(TargetVehicle);
                float DesiredHeading = TargetVehicle.Heading - 90f;

                string dict = "";
                string PerpAnim = "";
                string VictimAnim = "";
                int BoneIndexSpine = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Driver, 57597);//11816
                Vector3 DriverSeatCoordinates = NativeFunction.CallByName<Vector3>("GET_PED_BONE_COORDS", Driver, BoneIndexSpine, 0f, 0f, 0f);

                GameTimeLastTriedCarJacking = Game.GameTime;

                if (!GetCarjackingAnimations(TargetVehicle, DriverSeatCoordinates, myGun, ref dict, ref PerpAnim, ref VictimAnim))//couldnt find animations
                {

                    Game.LocalPlayer.Character.Tasks.ClearImmediately();
                    GameFiber.Sleep(200);
                    Game.LocalPlayer.Character.Tasks.EnterVehicle(TargetVehicle, SeatTryingToEnter);
                    return;
                }

                GTAPed CarjackingVictim = PoliceScanning.Civilians.FirstOrDefault(x => x.Pedestrian.Handle == Driver.Handle);
                if (CarjackingVictim != null)
                    CarjackingVictim.CanFlee = false;

                LosSantosRED.RequestAnimationDictionay(dict);
                LosSantosRED.SetPlayerToLastWeapon();

                //CameraSystem.HighLightCarjacking(TargetVehicle, true);

                float DriverHeading = Driver.Heading;
                int Scene1 = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", GameEntryPosition.X, GameEntryPosition.Y, Game.LocalPlayer.Character.Position.Z, 0.0f, 0.0f, DesiredHeading, 2);//270f //old
                NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_LOOPED", Scene1, false);
                NativeFunction.CallByName<bool>("TASK_SYNCHRONIZED_SCENE", Game.LocalPlayer.Character, Scene1, dict, PerpAnim, 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);//std_perp_ds_a
                NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_PHASE", Scene1, 0.0f);

                int Scene2 = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", DriverSeatCoordinates.X, DriverSeatCoordinates.Y, DriverSeatCoordinates.Z, 0.0f, 0.0f, DriverHeading, 2);//270f
                NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_LOOPED", Scene2, false);
                NativeFunction.CallByName<bool>("TASK_SYNCHRONIZED_SCENE", Driver, Scene2, dict, VictimAnim, 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);
                NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_PHASE", Scene2, 0.0f);

                PlayerBreakingIntoCar = true;
                bool locOpenDoor = false;
                bool Cancel = false;
                Vector3 OriginalCarPosition = TargetVehicle.Position;

                while (NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", Scene1) < 0.75f)
                {
                    float ScenePhase = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", Scene1);
                    float Scene2Phase = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", Scene2);
                    GameFiber.Yield();
                    if (ScenePhase <= 0.4f && Extensions.IsMoveControlPressed())
                    {
                        Cancel = true;
                        break;
                    }
                    if(Game.LocalPlayer.Character.IsDead)
                    {
                        Cancel = true;
                        break;
                    }

                    if (!locOpenDoor && ScenePhase > 0.05f && TargetVehicle.Doors[0].IsValid() && !TargetVehicle.Doors[0].IsFullyOpen)
                    {
                        locOpenDoor = true;
                        TargetVehicle.Doors[0].Open(false, false);
                    }
                    if (TargetVehicle.DistanceTo2D(OriginalCarPosition) >= 0.1f)
                    {
                        Cancel = true;
                        break;
                    }
                    if (Game.LocalPlayer.Character.IsConsideredArmed() && Game.IsControlPressed(2, GameControl.Attack))
                    {
                        Vector3 TargetCoordinate = Driver.GetBonePosition(PedBoneId.Head);
                        NativeFunction.CallByName<bool>("SET_PED_SHOOTS_AT_COORD", Game.LocalPlayer.Character, TargetCoordinate.X, TargetCoordinate.Y, TargetCoordinate.Z, true);
                        Police.PlayerArtificiallyShooting = true;
                        LosSantosRED.GameTimePlayerLastShot = Game.GameTime;

                        if (ScenePhase <= 0.35f)
                        {
                            Driver.WarpIntoVehicle(TargetVehicle, -1);
                            Game.LocalPlayer.Character.Tasks.Clear();
                            NativeFunction.CallByName<bool>("SET_PLAYER_FORCED_AIM", Game.LocalPlayer.Character, true);
                            break;
                        }
                    }
                    if (Game.LocalPlayer.Character.IsConsideredArmed() && Game.IsControlJustPressed(2, GameControl.Aim))
                    {
                        if (NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", Scene1) <= 0.4f)
                        {
                            Driver.WarpIntoVehicle(TargetVehicle, -1);
                            Game.LocalPlayer.Character.Tasks.Clear();
                            NativeFunction.CallByName<bool>("SET_PLAYER_FORCED_AIM", Game.LocalPlayer.Character, true);
                            break;
                        }
                    }
                    if(ScenePhase >= 0.5f)
                    {
                        //if (CameraSystem.UsingOtherCamera)
                            //CameraSystem.UnHighLightCarjacking(TargetVehicle, true);
                    }
                }

               // if (CameraSystem.UsingOtherCamera)
                   // CameraSystem.UnHighLightCarjacking(TargetVehicle, true);

                if (Game.LocalPlayer.Character.IsDead)
                {
                    PlayerBreakingIntoCar = false;
                    if (CarjackingVictim != null)
                        CarjackingVictim.CanFlee = true;
                    return;
                }
                Police.PlayerArtificiallyShooting = false;

                float FinalScenePhase = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", Scene1);
                Debugging.WriteToLog("CarJackPedWithWeapon", string.Format("Scene1 Phase: {0}", FinalScenePhase));
                if (FinalScenePhase <= 0.4f)
                {
                    if (Cancel || Driver.IsDead)
                    {
                        Driver.BlockPermanentEvents = false;
                        Driver.WarpIntoVehicle(TargetVehicle, -1);
                        Game.LocalPlayer.Character.Tasks.Clear();
                    }
                }
                else
                {
                    if (Cancel && FinalScenePhase <= 0.6f)
                    {
                        Driver.BlockPermanentEvents = false;
                        Driver.WarpIntoVehicle(TargetVehicle, -1);
                        Game.LocalPlayer.Character.Tasks.Clear();
                    }
                    else
                    {
                        Game.LocalPlayer.Character.WarpIntoVehicle(TargetVehicle, -1);
                        if (TargetVehicle.Doors[0].IsValid())
                            NativeFunction.CallByName<bool>("SET_VEHICLE_DOOR_CONTROL", TargetVehicle, 0, 4, 0f);
                    }
                }

                if (CarjackingVictim != null)
                    CarjackingVictim.CanFlee = true;

                if (Cancel)
                {
                    PlayerBreakingIntoCar = false;
                    //if (CameraSystem.UsingOtherCamera)
                    //    CameraSystem.UnHighLightCarjacking(TargetVehicle, true);
                    return;
                }


                if (TargetVehicle.Doors[0].IsValid())
                    NativeFunction.CallByName<bool>("SET_VEHICLE_DOOR_CONTROL", TargetVehicle, 0, 4, 0f);

                //if (CameraSystem.UsingOtherCamera)
                    //CameraSystem.UnHighLightCarjacking(TargetVehicle, true);

                if (Driver.IsInAnyVehicle(false))
                {
                    Debugging.WriteToLog("CarjackAnimation", "Driver In Vehicle");
                }
                else
                {
                    Debugging.WriteToLog("CarjackAnimation", "Driver Out of Vehicle");
                    if (Driver.IsAlive)
                    {
                        Driver.Tasks.ClearImmediately();
                        Driver.IsRagdoll = false;
                        Driver.BlockPermanentEvents = false;
                    }
                    if (rnd.Next(1, 11) >= 10)
                    {
                        GiveGunAndAttackPlayer(Driver);
                    }
                    else
                    {
                        Driver.Tasks.Flee(Game.LocalPlayer.Character, 100f, 30000);
                    }
                }
                GameFiber.Sleep(5000);
                PlayerBreakingIntoCar = false;
            }, "CarJackPedWithWeapon");
            Debugging.GameFibers.Add(CarJackPedWithWeapon);
        }
        catch (Exception e)
        {
            PlayerBreakingIntoCar = false;
            Debugging.WriteToLog("UnlockCarDoor", e.Message);
        }
    }
    public static void CarJackPedRegular(Vehicle TargetVehicle, Ped Driver)
    {
        GameFiber CarJackPed = GameFiber.StartNew(delegate
        {
            PlayerBreakingIntoCar = true;

            GTAPed CarjackingVictim = PoliceScanning.Civilians.FirstOrDefault(x => x.Pedestrian.Handle == Driver.Handle);
            if (CarjackingVictim != null)
                CarjackingVictim.CanFlee = false;

            GameFiber.Sleep(4000);
            if (CarjackingVictim != null)
                CarjackingVictim.CanFlee = true;

            GameFiber.Sleep(4000);
            PlayerBreakingIntoCar = false;
        }, "CarJackPed");
        Debugging.GameFibers.Add(CarJackPed);
    }
    private static bool GetCarjackingAnimations(Vehicle TargetVehicle, Vector3 DriverSeatCoordinates, GTAWeapon MyGun, ref string Dictionary, ref string PerpAnimation, ref string VictimAnimation)
    {
        if (MyGun == null || (!MyGun.IsTwoHanded && !MyGun.IsOneHanded))
            return false;

        int intVehicleClass = NativeFunction.CallByName<int>("GET_VEHICLE_CLASS", TargetVehicle);
        Vehicles.VehicleClass VehicleClass = (Vehicles.VehicleClass)intVehicleClass;
        if (VehicleClass == Vehicles.VehicleClass.Boats || VehicleClass == Vehicles.VehicleClass.Cycles || VehicleClass == Vehicles.VehicleClass.Industrial || VehicleClass == Vehicles.VehicleClass.Motorcycles || VehicleClass == Vehicles.VehicleClass.Planes || VehicleClass == Vehicles.VehicleClass.Service || VehicleClass == Vehicles.VehicleClass.Trailer || VehicleClass == Vehicles.VehicleClass.Trains)
            return false;//maybe add utility?

        if (!TargetVehicle.Doors[0].IsValid())
            return false;

        float? GroundZ = World.GetGroundZ(DriverSeatCoordinates, true, false);
        if (GroundZ == null)
            GroundZ = 0f;
        float DriverDistanceToGround = DriverSeatCoordinates.Z - (float)GroundZ;
        Debugging.WriteToLog("GetCarjackingAnimations", string.Format("VehicleClass {0},DriverSeatCoordinates: {1},GroundZ: {2}, PedHeight: {3}", VehicleClass, DriverSeatCoordinates, GroundZ, DriverDistanceToGround));
        if (VehicleClass == Vehicles.VehicleClass.Vans && DriverDistanceToGround > 1.5f)
        {
            if (MyGun.IsTwoHanded)
            {
                Dictionary = "veh@jacking@2h";
                PerpAnimation = "van_perp_ds_a";
                VictimAnimation = "van_victim_ds_a";
            }
            else if (MyGun.IsOneHanded)
            {
                Dictionary = "veh@jacking@1h";
                PerpAnimation = "van_perp_ds_a";
                VictimAnimation = "van_victim_ds_a";
            }
        }
        else if (VehicleClass == Vehicles.VehicleClass.Helicopters)
        {
            if (MyGun.IsTwoHanded)
            {
                Dictionary = "veh@jacking@2h";
                PerpAnimation = "heli_perp_ds_a";
                VictimAnimation = "heli_victim_ds_a";
            }
            else if (MyGun.IsOneHanded)
            {
                Dictionary = "veh@jacking@1h";
                PerpAnimation = "heli_perp_ds_a";
                VictimAnimation = "heli_victim_ds_a";
            }
        }
        else if (VehicleClass == Vehicles.VehicleClass.Commercial)
        {
            if (MyGun.IsTwoHanded)
            {
                Dictionary = "veh@jacking@2h";
                PerpAnimation = "truck_perp_ds_a";
                VictimAnimation = "truck_victim_ds_a";
            }
            else if (MyGun.IsOneHanded)
            {
                Dictionary = "veh@jacking@1h";
                PerpAnimation = "truck_perp_ds_a";
                VictimAnimation = "truck_victim_ds_a";
            }
        }
        else if (DriverDistanceToGround > 2f)//1.75f
        {
            if (MyGun.IsTwoHanded)
            {
                Dictionary = "veh@jacking@2h";
                PerpAnimation = "truck_perp_ds_a";
                VictimAnimation = "truck_victim_ds_a";
            }
            else if (MyGun.IsOneHanded)
            {
                Dictionary = "veh@jacking@1h";
                PerpAnimation = "truck_perp_ds_a";
                VictimAnimation = "truck_victim_ds_a";
            }
        }
        else if (DriverDistanceToGround < 0.5f)
        {
            if (MyGun.IsTwoHanded)
            {
                Dictionary = "veh@jacking@2h";
                PerpAnimation = "low_perp_ds_a";
                VictimAnimation = "low_victim_ds_a";
            }
            else if (MyGun.IsOneHanded)
            {
                Dictionary = "veh@jacking@1h";
                PerpAnimation = "low_perp_ds_a";
                VictimAnimation = "low_victim_ds_a";
            }
        }
        else
        {
            if (MyGun.IsTwoHanded)
            {
                Dictionary = "veh@jacking@2h";
                PerpAnimation = "std_perp_ds_a";
                VictimAnimation = "std_victim_ds_a";
            }
            else if (MyGun.IsOneHanded)
            {
                Dictionary = "veh@jacking@1h";
                PerpAnimation = "std_perp_ds";
                VictimAnimation = "std_victim_ds";
            }
        }
        return true;
    }
    public static void GiveGunAndAttackPlayer(Ped Attacker)
    {
        GTAWeapon GunToGive = GTAWeapons.WeaponsList.Where(x => x.Category == GTAWeapon.WeaponCategory.Pistol).PickRandom();
        Attacker.Inventory.GiveNewWeapon(GunToGive.Name, GunToGive.AmmoAmount, true);
        Attacker.Tasks.FightAgainst(Game.LocalPlayer.Character);
        Attacker.BlockPermanentEvents = true;
        Attacker.KeepTasks = true;
    }

}

