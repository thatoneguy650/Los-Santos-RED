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

public class CarJack 
{
    private int PlayerScene;
    private int VictimScene;
    private PedExt Victim;
    private Vehicle TargetVehicle;
    private Ped Driver;
    private int SeatTryingToEnter;
    private Vector3 DriverSeatCoordinates;
    private string Dictionary;
    private string PerpAnimation;
    private string VictimAnimation;
    private WeaponInformation Weapon;
    private uint GameTimeLastTriedCarJacking;
    private bool WantToCancel;
    private bool CanArmedCarJack
    {
        get
        {
            if (SeatTryingToEnter != -1)
                return false;

            if (TargetVehicle.HasBone("door_dside_f") && TargetVehicle.HasBone("door_pside_f"))
            {
                if (Game.LocalPlayer.Character.DistanceTo2D(TargetVehicle.GetBonePosition("door_dside_f")) > Game.LocalPlayer.Character.DistanceTo2D(TargetVehicle.GetBonePosition("door_pside_f")))
                {
                    return false;//Closer to passenger side, animations dont work
                }
            }
            return true;
        }
    }
    public CarJack(Vehicle VehicleToEnter, Ped DriverPed, int EntrySeat)
    {
        TargetVehicle = VehicleToEnter;
        Driver = DriverPed;
        SeatTryingToEnter = EntrySeat;
    }
    public void StartCarJack()
    {
        Victim = Mod.World.Pedestrians.Civilians.FirstOrDefault(x => x.Pedestrian.Handle == Driver.Handle);
        Weapon = Mod.DataMart.Weapons.GetCurrentWeapon(Game.LocalPlayer.Character);

        if (CanArmedCarJack && Mod.Input.IsHoldingEnter && Game.GameTime - GameTimeLastTriedCarJacking > 500 && Weapon != null && Weapon.Category != WeaponCategory.Melee)
        {
            ArmedCarJack();
        }
        else
        {
            UnarmedCarJack();
        }     
    }
    private void ArmedCarJack()
    {
        if (Victim != null)
            Victim.CanBeTasked = false;
        try
        {
            GameFiber CarJackPedWithWeapon = GameFiber.StartNew(delegate
            {
                if (!SetupCarJack())
                {
                    if (Victim != null)
                        Victim.CanBeTasked = true;
                    return;
                }
                if (!CarJackAnimation())
                {
                    if (Victim != null)
                        Victim.CanBeTasked = true;
                    return;
                }

                FinishCarJack();
                if (Victim != null)
                    Victim.CanBeTasked = true;

                //CameraManager.RestoreGameplayerCamera();

            }, "CarJackPedWithWeapon");
            Mod.Debug.GameFibers.Add(CarJackPedWithWeapon);
        }
        catch (Exception e)
        {
            Mod.Player.IsCarJackingManual = false;
            Mod.Debug.WriteToLog("UnlockCarDoor", e.Message);
        }
    }
    private bool SetupCarJack()
    {
        Mod.Player.SetPlayerToLastWeapon();
        NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", Driver, TargetVehicle, 27, -1);
        Driver.BlockPermanentEvents = true;

        Vector3 GameEntryPosition = GetEntryPosition();
        float DesiredHeading = TargetVehicle.Heading - 90f;
        int BoneIndexSpine = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Driver, 57597);//11816
        DriverSeatCoordinates = NativeFunction.CallByName<Vector3>("GET_PED_BONE_COORDS", Driver, BoneIndexSpine, 0f, 0f, 0f);

        GameTimeLastTriedCarJacking = Game.GameTime;

        if (!GetCarjackingAnimations())//couldnt find animations
        {
            Game.LocalPlayer.Character.Tasks.ClearImmediately();
            GameFiber.Sleep(200);
            Game.LocalPlayer.Character.Tasks.EnterVehicle(TargetVehicle, SeatTryingToEnter);
            return false;
        }

        AnimationDictionary AnimDictionary = new AnimationDictionary(Dictionary);
        Mod.Player.SetPlayerToLastWeapon();

        if (!Driver.IsInAnyVehicle(false))
            Driver.WarpIntoVehicle(TargetVehicle, -1);

        float DriverHeading = Driver.Heading;
        PlayerScene = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", GameEntryPosition.X, GameEntryPosition.Y, Game.LocalPlayer.Character.Position.Z, 0.0f, 0.0f, DesiredHeading, 2);//270f //old
        NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_LOOPED", PlayerScene, false);
        NativeFunction.CallByName<bool>("TASK_SYNCHRONIZED_SCENE", Game.LocalPlayer.Character, PlayerScene, Dictionary, PerpAnimation, 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);//std_perp_ds_a
        NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_PHASE", PlayerScene, 0.0f);

        VictimScene = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", DriverSeatCoordinates.X, DriverSeatCoordinates.Y, DriverSeatCoordinates.Z, 0.0f, 0.0f, DriverHeading, 2);//270f
        NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_LOOPED", VictimScene, false);
        NativeFunction.CallByName<bool>("TASK_SYNCHRONIZED_SCENE", Driver, VictimScene, Dictionary, VictimAnimation, 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);
        NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_PHASE", VictimScene, 0.0f);



        return true;
    }
    private bool CarJackAnimation()
    {

        Mod.Player.IsCarJackingManual = true;
        bool locOpenDoor = false;
        WantToCancel = false;
        Vector3 OriginalCarPosition = TargetVehicle.Position;

        //CameraManager.TransitionToAltCam(TargetVehicle, GetCameraPosition(), 1500);


        while (NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", PlayerScene) < 0.75f)
        {
            float ScenePhase = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", PlayerScene);
            GameFiber.Yield();
            if (ScenePhase <= 0.4f && Mod.Input.IsMoveControlPressed)
            {
                WantToCancel = true;
                break;
            }
            if (Game.LocalPlayer.Character.IsDead)
            {
                WantToCancel = true;
                break;
            }
            if(!NativeFunction.CallByName<bool>("IS_SYNCHRONIZED_SCENE_RUNNING", VictimScene))
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
            if (Mod.Player.IsConsideredArmed && Game.IsControlPressed(2, GameControl.Attack))//Game.LocalPlayer.Character.IsConsideredArmed()
            {
                Vector3 TargetCoordinate = Driver.GetBonePosition(PedBoneId.Head);
                NativeFunction.CallByName<bool>("SET_PED_SHOOTS_AT_COORD", Game.LocalPlayer.Character, TargetCoordinate.X, TargetCoordinate.Y, TargetCoordinate.Z, true);
                Mod.Player.FlagShooting();

                if (ScenePhase <= 0.35f)
                {
                    Driver.WarpIntoVehicle(TargetVehicle, -1);
                    Game.LocalPlayer.Character.Tasks.Clear();
                    NativeFunction.CallByName<bool>("SET_PLAYER_FORCED_AIM", Game.LocalPlayer.Character, true);
                    break;
                }
            }
            if (Mod.Player.IsConsideredArmed && Game.IsControlJustPressed(2, GameControl.Aim))//Game.LocalPlayer.Character.IsConsideredArmed()
            {
                if (NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", PlayerScene) <= 0.4f)
                {
                    Driver.WarpIntoVehicle(TargetVehicle, -1);
                    Game.LocalPlayer.Character.Tasks.Clear();
                    NativeFunction.CallByName<bool>("SET_PLAYER_FORCED_AIM", Game.LocalPlayer.Character, true);
                    break;
                }
            }
            if (ScenePhase >= 0.5f)
            {
                //CameraManager.RestoreGameplayerCamera();
            }
            if(Mod.Player.IsBusted || Mod.Player.IsDead)
            {
                WantToCancel = true;
                break;
            }
        }


        //CameraManager.RestoreGameplayerCamera();

        if (Game.LocalPlayer.Character.IsDead)
        {
            Mod.Player.IsCarJackingManual = false;
            if (Victim != null)
                Victim.CanBeTasked = true;
            return false;
        }
        return true;
    }
    private bool FinishCarJack()
    {
        
        float FinalScenePhase = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", PlayerScene);
        if (FinalScenePhase <= 0.4f)
        {
            if (WantToCancel || Driver.IsDead)
            {
                Driver.BlockPermanentEvents = false;
                Driver.WarpIntoVehicle(TargetVehicle, -1);
                Game.LocalPlayer.Character.Tasks.Clear();
            }
        }
        else
        {
            if (WantToCancel && FinalScenePhase <= 0.6f)
            {
                Driver.BlockPermanentEvents = false;
                Driver.WarpIntoVehicle(TargetVehicle, -1);
                Game.LocalPlayer.Character.Tasks.Clear();
            }
            else
            {
                Game.LocalPlayer.Character.WarpIntoVehicle(TargetVehicle, -1);



                VehicleExt MyCar = Mod.World.Vehicles.GetVehicle(TargetVehicle);
                if (MyCar != null)
                {
                    MyCar.ToggleEngine(true);
                }



                if (TargetVehicle.Doors[0].IsValid())
                {
                    NativeFunction.CallByName<bool>("SET_VEHICLE_DOOR_CONTROL", TargetVehicle, 0, 4, 0f);
                }
            }
        }

        if (Victim != null)
            Victim.CanBeTasked = true;

        if (WantToCancel)
        {
            Mod.Player.IsCarJackingManual = false;
            return false;
        }


        if (TargetVehicle.Doors[0].IsValid())
            NativeFunction.CallByName<bool>("SET_VEHICLE_DOOR_CONTROL", TargetVehicle, 0, 4, 0f);


        if (Driver.IsInAnyVehicle(false))
        {
            Mod.Debug.WriteToLog("CarjackAnimation", "Driver In Vehicle");
        }
        else
        {
            Mod.Debug.WriteToLog("CarjackAnimation", "Driver Out of Vehicle");
            if (Driver.IsAlive)
            {
                Driver.Tasks.ClearImmediately();
                Driver.Tasks.Flee(Game.LocalPlayer.Character,500f,0);
                Driver.IsRagdoll = false;
                Driver.BlockPermanentEvents = false;
            }
        }
        GameFiber.Sleep(5000);
        Mod.Player.IsCarJackingManual = false;
        return true;
    }
    private void UnarmedCarJack()
    {
        GameFiber CarJackPed = GameFiber.StartNew(delegate
        {
            //Mod.Player.IsCarJacking = true;

            if (Victim != null)
                Victim.CanBeTasked = false;

            GameFiber.Sleep(4000);
            if (Victim != null)
                Victim.CanBeTasked = true;

            GameFiber.Sleep(4000);
            //Mod.Player.IsCarJacking = false;
        }, "CarJackPed");
        Mod.Debug.GameFibers.Add(CarJackPed);
    }
    private bool GetCarjackingAnimations()
    {
        if (Weapon == null || (!Weapon.IsTwoHanded && !Weapon.IsOneHanded))
            return false;

        int intVehicleClass = NativeFunction.CallByName<int>("GET_VEHICLE_CLASS", TargetVehicle);
        VehicleClass VehicleClass = (VehicleClass)intVehicleClass;
        if (VehicleClass == VehicleClass.Boat || VehicleClass == VehicleClass.Cycle || VehicleClass == VehicleClass.Industrial || VehicleClass == VehicleClass.Motorcycle || VehicleClass == VehicleClass.Plane || VehicleClass == VehicleClass.Service)
            return false;//maybe add utility?

        if (!TargetVehicle.Doors[0].IsValid())
            return false;

        float? GroundZ = Rage.World.GetGroundZ(DriverSeatCoordinates, true, false);
        if (GroundZ == null)
            GroundZ = 0f;
        float DriverDistanceToGround = DriverSeatCoordinates.Z - (float)GroundZ;
        Mod.Debug.WriteToLog("GetCarjackingAnimations", string.Format("VehicleClass {0},DriverSeatCoordinates: {1},GroundZ: {2}, PedHeight: {3}", VehicleClass, DriverSeatCoordinates, GroundZ, DriverDistanceToGround));
        if (VehicleClass == VehicleClass.Van && DriverDistanceToGround > 1.5f)
        {
            if (Weapon.IsTwoHanded)
            {
                Dictionary = "veh@jacking@2h";
                PerpAnimation = "van_perp_ds_a";
                VictimAnimation = "van_victim_ds_a";
            }
            else if (Weapon.IsOneHanded)
            {
                Dictionary = "veh@jacking@1h";
                PerpAnimation = "van_perp_ds_a";
                VictimAnimation = "van_victim_ds_a";
            }
        }
        else if (VehicleClass == VehicleClass.Helicopter)
        {
            if (Weapon.IsTwoHanded)
            {
                Dictionary = "veh@jacking@2h";
                PerpAnimation = "heli_perp_ds_a";
                VictimAnimation = "heli_victim_ds_a";
            }
            else if (Weapon.IsOneHanded)
            {
                Dictionary = "veh@jacking@1h";
                PerpAnimation = "heli_perp_ds_a";
                VictimAnimation = "heli_victim_ds_a";
            }
        }
        else if (VehicleClass == VehicleClass.Commercial)
        {
            if (Weapon.IsTwoHanded)
            {
                Dictionary = "veh@jacking@2h";
                PerpAnimation = "truck_perp_ds_a";
                VictimAnimation = "truck_victim_ds_a";
            }
            else if (Weapon.IsOneHanded)
            {
                Dictionary = "veh@jacking@1h";
                PerpAnimation = "truck_perp_ds_a";
                VictimAnimation = "truck_victim_ds_a";
            }
        }
        else if (DriverDistanceToGround > 2f)//1.75f
        {
            if (Weapon.IsTwoHanded)
            {
                Dictionary = "veh@jacking@2h";
                PerpAnimation = "truck_perp_ds_a";
                VictimAnimation = "truck_victim_ds_a";
            }
            else if (Weapon.IsOneHanded)
            {
                Dictionary = "veh@jacking@1h";
                PerpAnimation = "truck_perp_ds_a";
                VictimAnimation = "truck_victim_ds_a";
            }
        }
        else if (DriverDistanceToGround < 0.5f)
        {
            if (Weapon.IsTwoHanded)
            {
                Dictionary = "veh@jacking@2h";
                PerpAnimation = "low_perp_ds_a";
                VictimAnimation = "low_victim_ds_a";
            }
            else if (Weapon.IsOneHanded)
            {
                Dictionary = "veh@jacking@1h";
                PerpAnimation = "low_perp_ds_a";
                VictimAnimation = "low_victim_ds_a";
            }
        }
        else
        {
            if (Weapon.IsTwoHanded)
            {
                Dictionary = "veh@jacking@2h";
                PerpAnimation = "std_perp_ds_a";
                VictimAnimation = "std_victim_ds_a";
            }
            else if (Weapon.IsOneHanded)
            {
                Dictionary = "veh@jacking@1h";
                PerpAnimation = "std_perp_ds";
                VictimAnimation = "std_victim_ds";
            }
        }
        return true;
    }
    private Vector3 GetEntryPosition()
    {
        return NativeFunction.CallByHash<Vector3>(0xC0572928C0ABFDA3, TargetVehicle, 0);
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
}

