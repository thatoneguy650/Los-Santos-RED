using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;

//lots more refactoring please
public class CarJack
{
    private Vehicle TargetVehicle;
    private PedExt Driver;
    private ICarStealable Player;
    private WeaponInformation Weapon;
    private VehicleExt VehicleExt;
    private string DriverAnimation;
    private int DriverScene;
    private string PlayerAnimation;
    private int PlayerScene;
    private Vector3 DriverSeatCoordinates;
    private uint GameTimeLastTriedCarJacking;
    private int SeatTryingToEnter;
    private string Dictionary;
    private bool WantToCancel;
    public CarJack(ICarStealable player, VehicleExt vehicle, PedExt driver, int EntrySeat, WeaponInformation weapon)
    {
        Player = player;
        VehicleExt = vehicle;
        TargetVehicle = VehicleExt.Vehicle;
        SeatTryingToEnter = EntrySeat;
        Weapon = weapon;
        Driver = driver;
    }
    public void Start()
    {
        if (CanArmedCarJack() && Game.GameTime - GameTimeLastTriedCarJacking > 500 && Weapon != null && Weapon.Category != WeaponCategory.Melee)
        {
            //EntryPoint.WriteToConsole($"CARJACK EVENT: Armed Start");
            ArmedCarJack();
        }
        else
        {
            //EntryPoint.WriteToConsole($"CARJACK EVENT: Unarmed Start");
            UnarmedCarJack();
        }
    }
    private void ArmedCarJack()
    {
        if (Driver != null)
        {
            Driver.CanBeTasked = false;
        }
        if (Driver != null && Driver.Pedestrian.Exists())
        {
            //EntryPoint.WriteToConsole($"CARJACK EVENT: Armed: Victim.CanBeTasked: {Driver.CanBeTasked}, Handle: {Driver.Pedestrian.Handle}", 3);
            try
            {
                GameFiber CarJackPedWithWeapon = GameFiber.StartNew(delegate
                {
                    try
                    {
                        GameFiber.Yield();
                        if (Driver != null && Driver.Pedestrian.Exists())
                        {
                            if (!SetupCarJack())
                            {
                                GameFiber.Yield();
                                if (Driver != null)
                                {
                                    Driver.CanBeTasked = true;
                                }
                                return;
                            }
                            if (!CarJackAnimation())
                            {
                                GameFiber.Yield();
                                if (Driver != null)
                                {
                                    Driver.CanBeTasked = true;
                                }
                                return;
                            }
                            FinishCarJack();
                            if (Driver != null)
                            {
                                Driver.CanBeTasked = true;
                            }
                            WatchDriverToReport();
                        }
                    }
                    catch (Exception ex)
                    {
                        EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                        EntryPoint.ModController.CrashUnload();
                    }
                    //CameraManager.RestoreGameplayerCamera();
                }, "CarJackPedWithWeapon");
            }
            catch (Exception e)
            {
                Player.IsCarJacking = false;
                EntryPoint.WriteToConsole("UnlockCarDoor" + e.Message + e.StackTrace, 0);
            }
        }
    }
    private bool CarJackAnimation()
    {
        Player.IsCarJacking = true;
        bool locOpenDoor = false;
        WantToCancel = false;
        Vector3 OriginalCarPosition = TargetVehicle.Position;
        //CameraManager.TransitionToAltCam(TargetVehicle, GetCameraPosition(), 1500);
        while (NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", PlayerScene) < 0.75f)
        {
            float ScenePhase = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", PlayerScene);
            GameFiber.Yield();
            if (ScenePhase <= 0.4f && Player.IsMoveControlPressed)
            {
                WantToCancel = true;
                break;
            }
            if (Game.LocalPlayer.Character.IsDead || Game.LocalPlayer.Character.IsRagdoll)
            {
                WantToCancel = true;
                break;
            }
            if (!NativeFunction.CallByName<bool>("IS_SYNCHRONIZED_SCENE_RUNNING", DriverScene))
            {
                WantToCancel = true;
                break;
            }

            if (!locOpenDoor && ScenePhase > 0.05f && TargetVehicle.Doors[0].IsValid() && !TargetVehicle.Doors[0].IsFullyOpen)
            {
                locOpenDoor = true;
                TargetVehicle.Doors[0].Open(false, false);
            }
            if (TargetVehicle.DistanceTo2D(OriginalCarPosition) >= 0.1f)
            {
                WantToCancel = true;
                break;
            }
            if (Player.IsVisiblyArmed && Game.IsControlPressed(2, GameControl.Attack))
            {
                Vector3 TargetCoordinate = Driver.Pedestrian.GetBonePosition(PedBoneId.Head);
                Player.ShootAt(TargetCoordinate);

                if (ScenePhase <= 0.35f)
                {
                    Driver.Pedestrian.WarpIntoVehicle(TargetVehicle, -1);
                    NativeFunction.Natives.CLEAR_PED_TASKS(Game.LocalPlayer.Character);
                    NativeFunction.CallByName<bool>("SET_PLAYER_FORCED_AIM", Game.LocalPlayer.Character, true);
                    break;
                }
            }
            if (Player.IsVisiblyArmed && Game.IsControlJustPressed(2, GameControl.Aim))
            {
                if (NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", PlayerScene) <= 0.4f)
                {
                    Driver.Pedestrian.WarpIntoVehicle(TargetVehicle, -1);
                    NativeFunction.Natives.CLEAR_PED_TASKS(Game.LocalPlayer.Character);
                    NativeFunction.CallByName<bool>("SET_PLAYER_FORCED_AIM", Game.LocalPlayer.Character, true);
                    break;
                }
            }
            if (ScenePhase >= 0.5f)
            {
                //CameraManager.RestoreGameplayerCamera();
            }
            if (Player.IsBusted || Player.IsDead)
            {
                WantToCancel = true;
                break;
            }
        }
        //CameraManager.RestoreGameplayerCamera();
        if (Player.IsDead)
        {
            Player.IsCarJacking = false;
            if (Driver != null)
            {
                Driver.CanBeTasked = true;
            }
            return false;
        }
        return true;
    }
    private bool FinishCarJack()
    {
        float FinalScenePhase = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", PlayerScene);
        if (FinalScenePhase <= 0.4f)
        {
            if (WantToCancel || Driver.Pedestrian.IsDead)
            {
                Driver.Pedestrian.BlockPermanentEvents = false;
                Driver.Pedestrian.WarpIntoVehicle(TargetVehicle, -1);
                NativeFunction.Natives.CLEAR_PED_TASKS(Game.LocalPlayer.Character);
            }
        }
        else
        {
            if (WantToCancel && FinalScenePhase <= 0.6f)
            {
                Driver.Pedestrian.BlockPermanentEvents = false;
                Driver.Pedestrian.WarpIntoVehicle(TargetVehicle, -1);
                NativeFunction.Natives.CLEAR_PED_TASKS(Game.LocalPlayer.Character);
            }
            else
            {
                //EntryPoint.WriteToConsole($"CARJACK EVENT: PRE WARP TargetVehicle Engine On {TargetVehicle.IsEngineOn}", 3);
                Game.LocalPlayer.Character.WarpIntoVehicle(TargetVehicle, -1);
                //EntryPoint.WriteToConsole($"CARJACK EVENT: POST WARP TargetVehicle Engine On {TargetVehicle.IsEngineOn}", 3);
                //EntryPoint.WriteToConsole($"CARJACK EVENT: POST ENGINE TOGGLE TargetVehicle Engine On {TargetVehicle.IsEngineOn}", 3);
                if (TargetVehicle.Doors[0].IsValid())
                {
                    NativeFunction.CallByName<bool>("SET_VEHICLE_DOOR_CONTROL", TargetVehicle, 0, 4, 0f);
                }
            }
        }

        if (Driver != null)
        {
            Driver.CanBeTasked = true;
        }

        if (WantToCancel)
        {
            Player.IsCarJacking = false;
            return false;
        }
        if (TargetVehicle.Doors[0].IsValid())
        {
            NativeFunction.CallByName<bool>("SET_VEHICLE_DOOR_CONTROL", TargetVehicle, 0, 4, 0f);
        }

        if (Driver.Pedestrian.IsInAnyVehicle(false))
        {

        }
        else
        {
            if (Driver.Pedestrian.IsAlive)
            {
                NativeFunction.Natives.CLEAR_PED_TASKS_IMMEDIATELY(Driver.Pedestrian);
                NativeFunction.Natives.TASK_SMART_FLEE_PED(Driver.Pedestrian, Player.Character, 500f, -1, false, false);
                Driver.Pedestrian.IsRagdoll = false;
                Driver.Pedestrian.BlockPermanentEvents = false;
            }
        }
        GameFiber.Sleep(5000);
        Player.IsCarJacking = false;
        return true;
    }
    private Vector3 GetCameraPosition()
    {
        Vector3 CameraPosition;
        float Distance = 6f;//General.MyRand.NextFloat(5f, 8f);
        float XVariance = 3f;// General.MyRand.NextFloat(0.5f, 3f);
        float YVariance = 3f;// 3f;// General.MyRand.NextFloat(0.5f, 3f);
        float ZVariance = 2f;// 1.8f;//General.MyRand.NextFloat(1.8f, 3f);
        if (TargetVehicle != null && TargetVehicle.Exists())
        {
            bool IsDriverSide = true;//for now..
            if (IsDriverSide)
            {
                Distance *= -1f;
                XVariance *= -1f;
            }
            CameraPosition = TargetVehicle.GetOffsetPositionRight(Distance);
        }
        else
        {
            CameraPosition = Game.LocalPlayer.Character.GetOffsetPositionRight(Distance);
        }
        CameraPosition += new Vector3(XVariance, YVariance, ZVariance);
        return CameraPosition;
    }
    private bool GetCarjackingAnimations()
    {
        if (Weapon == null || (!Weapon.IsTwoHanded && !Weapon.IsOneHanded))
        {
            return false;
        }

        int intVehicleClass = NativeFunction.CallByName<int>("GET_VEHICLE_CLASS", TargetVehicle);
        VehicleClass VehicleClass = (VehicleClass)intVehicleClass;
        if (VehicleClass == VehicleClass.Boat || VehicleClass == VehicleClass.Cycle || VehicleClass == VehicleClass.Industrial || VehicleClass == VehicleClass.Motorcycle || VehicleClass == VehicleClass.Plane || VehicleClass == VehicleClass.Service)
        {
            return false;//maybe add utility?
        }

        if (!TargetVehicle.Doors[0].IsValid())
        {
            return false;
        }

        float? GroundZ = Rage.World.GetGroundZ(DriverSeatCoordinates, true, false);
        if (GroundZ == null)
        {
            GroundZ = 0f;
        }
        float DriverDistanceToGround = DriverSeatCoordinates.Z - (float)GroundZ;
        if (VehicleClass == VehicleClass.Van && DriverDistanceToGround > 1.5f)
        {
            if (Weapon.IsTwoHanded)
            {
                Dictionary = "veh@jacking@2h";
                PlayerAnimation = "van_perp_ds_a";
                DriverAnimation = "van_victim_ds_a";
            }
            else if (Weapon.IsOneHanded)
            {
                Dictionary = "veh@jacking@1h";
                PlayerAnimation = "van_perp_ds_a";
                DriverAnimation = "van_victim_ds_a";
            }
        }
        else if (VehicleClass == VehicleClass.Helicopter)
        {
            if (Weapon.IsTwoHanded)
            {
                Dictionary = "veh@jacking@2h";
                PlayerAnimation = "heli_perp_ds_a";
                DriverAnimation = "heli_victim_ds_a";
            }
            else if (Weapon.IsOneHanded)
            {
                Dictionary = "veh@jacking@1h";
                PlayerAnimation = "heli_perp_ds_a";
                DriverAnimation = "heli_victim_ds_a";
            }
        }
        else if (VehicleClass == VehicleClass.Commercial)
        {
            if (Weapon.IsTwoHanded)
            {
                Dictionary = "veh@jacking@2h";
                PlayerAnimation = "truck_perp_ds_a";
                DriverAnimation = "truck_victim_ds_a";
            }
            else if (Weapon.IsOneHanded)
            {
                Dictionary = "veh@jacking@1h";
                PlayerAnimation = "truck_perp_ds_a";
                DriverAnimation = "truck_victim_ds_a";
            }
        }
        else if (DriverDistanceToGround > 2f)//1.75f
        {
            if (Weapon.IsTwoHanded)
            {
                Dictionary = "veh@jacking@2h";
                PlayerAnimation = "truck_perp_ds_a";
                DriverAnimation = "truck_victim_ds_a";
            }
            else if (Weapon.IsOneHanded)
            {
                Dictionary = "veh@jacking@1h";
                PlayerAnimation = "truck_perp_ds_a";
                DriverAnimation = "truck_victim_ds_a";
            }
        }
        else if (DriverDistanceToGround < 0.5f)
        {
            if (Weapon.IsTwoHanded)
            {
                Dictionary = "veh@jacking@2h";
                PlayerAnimation = "low_perp_ds_a";
                DriverAnimation = "low_victim_ds_a";
            }
            else if (Weapon.IsOneHanded)
            {
                Dictionary = "veh@jacking@1h";
                PlayerAnimation = "low_perp_ds_a";
                DriverAnimation = "low_victim_ds_a";
            }
        }
        else
        {
            if (Weapon.IsTwoHanded)
            {
                Dictionary = "veh@jacking@2h";
                PlayerAnimation = "std_perp_ds_a";
                DriverAnimation = "std_victim_ds_a";
            }
            else if (Weapon.IsOneHanded)
            {
                Dictionary = "veh@jacking@1h";
                PlayerAnimation = "std_perp_ds";
                DriverAnimation = "std_victim_ds";
            }
        }
        return true;
    }
    private bool SetupCarJack()
    {
        Player.WeaponEquipment.SetLastWeapon();
        NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", Driver.Pedestrian, TargetVehicle, 27, -1);
        Driver.Pedestrian.BlockPermanentEvents = true;
        Vector3 GameEntryPosition = NativeFunction.CallByHash<Vector3>(0xC0572928C0ABFDA3, TargetVehicle, 0);//GET_ENTRY_POSITION
        float DesiredHeading = TargetVehicle.Heading - 90f;
        int BoneIndexSpine = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Driver.Pedestrian, 57597);//11816
        DriverSeatCoordinates = NativeFunction.CallByName<Vector3>("GET_PED_BONE_COORDS", Driver.Pedestrian, BoneIndexSpine, 0f, 0f, 0f);
        GameTimeLastTriedCarJacking = Game.GameTime;
        if (!GetCarjackingAnimations())//couldnt find animations
        {
            NativeFunction.Natives.CLEAR_PED_TASKS_IMMEDIATELY(Game.LocalPlayer.Character);
            GameFiber.Sleep(200);
            NativeFunction.Natives.TASK_ENTER_VEHICLE(Player.Character, TargetVehicle, -1, SeatTryingToEnter, 2.0f, 1, 0);
            return false;
        }
        AnimationDictionary.RequestAnimationDictionay(Dictionary);
        Player.WeaponEquipment.SetLastWeapon();
        if (!Driver.Pedestrian.IsInAnyVehicle(false))
        {
            Driver.Pedestrian.WarpIntoVehicle(TargetVehicle, -1);
        }
        float DriverHeading = Driver.Pedestrian.Heading;
        PlayerScene = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", GameEntryPosition.X, GameEntryPosition.Y, Game.LocalPlayer.Character.Position.Z, 0.0f, 0.0f, DesiredHeading, 2);//270f //old
        NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_LOOPED", PlayerScene, false);
        NativeFunction.CallByName<bool>("TASK_SYNCHRONIZED_SCENE", Game.LocalPlayer.Character, PlayerScene, Dictionary, PlayerAnimation, 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);//std_perp_ds_a
        NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_PHASE", PlayerScene, 0.0f);
        DriverScene = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", DriverSeatCoordinates.X, DriverSeatCoordinates.Y, DriverSeatCoordinates.Z, 0.0f, 0.0f, DriverHeading, 2);//270f
        NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_LOOPED", DriverScene, false);
        NativeFunction.CallByName<bool>("TASK_SYNCHRONIZED_SCENE", Driver.Pedestrian, DriverScene, Dictionary, DriverAnimation, 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);
        NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_PHASE", DriverScene, 0.0f);
        return true;
    }
    private bool CanArmedCarJack()
    {
        if (SeatTryingToEnter != -1)
        {
            return false;
        }
        if (TargetVehicle.HasBone("door_dside_f") && TargetVehicle.HasBone("door_pside_f"))
        {
            if (Game.LocalPlayer.Character.DistanceTo2D(TargetVehicle.GetBonePosition("door_dside_f")) > Game.LocalPlayer.Character.DistanceTo2D(TargetVehicle.GetBonePosition("door_pside_f")))
            {
                return false;//Closer to passenger side, animations dont work
            }
        }
        return true;
    }
    private void UnarmedCarJack()
    {
        GameFiber CarJackPed = GameFiber.StartNew(delegate
        {
            try
            {
                GameFiber.Yield();
                if (Driver != null)
                {
                    Driver.CanBeTasked = false;
                }
                uint GameTimeStarted = Game.GameTime;
                while (!Player.Character.IsInAnyVehicle(false) && Game.GameTime - GameTimeStarted <= 5000)
                {
                    GameFiber.Yield();
                }
                if (Player.Character.IsInAnyVehicle(false))
                {

                }
                if (Driver != null)
                {
                    Driver.CanBeTasked = true;
                }
                //GameFiber.Sleep(4000);
                WatchDriverToReport();
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "CarJackPed");
    }
    private void WatchDriverToReport()
    {
        uint GameTimeStartedWatching = Game.GameTime;
        if(!Driver.WillCallPolice && !Driver.WillCallPoliceIntense && !Driver.HasCellPhone)
        {
            EntryPoint.WriteToConsole("Carjacked someone who wont call the cops or doesnt have a cellphone, ending");
            return;
        }
        bool shouldReportStolen = false;
        while (true)
        {
            if(Driver.IsDead)
            {
                EntryPoint.WriteToConsole("Carjacked driver dies DO NOT REPORT STOLEN YET");
                shouldReportStolen = false;
                break;
            }
            if(Driver.HasCalledInCrimesRecently)
            {
                EntryPoint.WriteToConsole("Carjacked has called in crimes, setting reported stolen");
                shouldReportStolen = true;
                break;
            }
            if(!Driver.Pedestrian.Exists() && !Driver.HasCalledInCrimesRecently)
            {
                EntryPoint.WriteToConsole("Carjacked driver does not exist and didnt call in any crimes");
                shouldReportStolen = false;
                break;
            }
            GameFiber.Yield();
        }
        if(shouldReportStolen && VehicleExt != null && VehicleExt.Vehicle.Exists())
        {
            VehicleExt.SetReportedStolen();
        }
    }
}