using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

public class Roadblock
{
    private Vector3 NodeCenter;
    private Vector3 NodeOffset;
    private float NodeHeading;
    private Model VehicleModel;
    private DispatchableVehicle Vehicle;
    private string SpikeStripName = "p_ld_stinger_s";
    private Model SpikeStripModel;
    private List<Vehicle> CreatedRoadblockVehicles = new List<Vehicle>();
    private Vehicle MainVehicle;
    private Vector3 InitialPosition;
    private List<Vector3> SpawnPoints = new List<Vector3>();
    private List<Rage.Object> CreatedProps = new List<Rage.Object>();
    private IDispatchable Player;
    private IEntityProvideable World;
    private Agency Agency;
    private bool IsDisposed;
    private ISettingsProvideable Settings;
    private float RotatedNodeHeading => NodeHeading - 90f;
    public Roadblock(IDispatchable player, IEntityProvideable world,Agency agency, DispatchableVehicle vehicle, Vector3 initialPosition, ISettingsProvideable settings)
    {
        Player = player;
        World = world;
        Agency = agency;
        Vehicle = vehicle;
        InitialPosition = initialPosition;
        Settings = settings;
        VehicleModel = new Model(Vehicle.ModelName);
        SpikeStripModel = new Model(SpikeStripName);
    }
    public Vector3 CenterPosition => NodeCenter;
    public void Dispose()
    {
        IsDisposed = true;
        RemoveItems();
    }
    public void SpawnRoadblock()
    {
        VehicleModel.LoadAndWait();
        SpikeStripModel.LoadAndWait();

        if (GetPosition())
        {
            FillInBlockade();
        }

        CheckSpikeStrips();
    }
    private void CheckSpikeStrips()
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                while (!IsDisposed)
                {
                    if (Player.IsInVehicle && Player.CurrentVehicle != null)
                    {
                        if (Player.CurrentVehicle.Vehicle.Exists())
                        {

                            //'0 = wheel_lf / bike, plane or jet front
                            //'1 = wheel_rf
                            //'2 = wheel_lm / in 6 wheels trailer, plane or jet is first one on left
                            //'3 = wheel_rm / in 6 wheels trailer, plane or jet is first one on right
                            //'4 = wheel_lr / bike rear / in 6 wheels trailer, plane or jet is last one on left
                            //'5 = wheel_rr / in 6 wheels trailer, plane or jet is last one on right
                            List<(int, string)> WheelList = new List<(int, string)>() { (0, "wheel_lf"), (1, "wheel_rf"), (2, "wheel_lm"), (3, "wheel_rm"), (4, "wheel_lr"), (5, "wheel_rr") };
                            foreach ((int, string) wheelItem in WheelList)
                            {
                                if (Player.CurrentVehicle.Vehicle.HasBone(wheelItem.Item2) && Player.CurrentVehicle.Vehicle.HasBone(Player.CurrentVehicle.Vehicle.GetBoneIndex(wheelItem.Item2)))
                                {
                                    CheckTireForCollision(Player.CurrentVehicle.Vehicle.Wheels[wheelItem.Item1]);
                                }
                            }

                            //if (Player.CurrentVehicle.Vehicle.HasBone(Player.CurrentVehicle.Vehicle.GetBoneIndex("wheel_lf")))
                            //{
                            //    CheckTireForCollision(Player.CurrentVehicle.Vehicle.Wheels[0]);
                            //}
                            //if (Player.CurrentVehicle.Vehicle.HasBone(Player.CurrentVehicle.Vehicle.GetBoneIndex("wheel_rf")))
                            //{
                            //    CheckTireForCollision(Player.CurrentVehicle.Vehicle.Wheels[1]);
                            //}
                            //if (Player.CurrentVehicle.Vehicle.HasBone(Player.CurrentVehicle.Vehicle.GetBoneIndex("wheel_lm")))
                            //{
                            //    CheckTireForCollision(Player.CurrentVehicle.Vehicle.Wheels[2]);
                            //}
                            //if (Player.CurrentVehicle.Vehicle.HasBone(Player.CurrentVehicle.Vehicle.GetBoneIndex("wheel_rm")))
                            //{
                            //    CheckTireForCollision(Player.CurrentVehicle.Vehicle.Wheels[3]);
                            //}
                            //if (Player.CurrentVehicle.Vehicle.HasBone(Player.CurrentVehicle.Vehicle.GetBoneIndex("wheel_lr")))
                            //{
                            //    CheckTireForCollision(Player.CurrentVehicle.Vehicle.Wheels[4]);
                            //}
                            //if (Player.CurrentVehicle.Vehicle.HasBone(Player.CurrentVehicle.Vehicle.GetBoneIndex("wheel_rr")))
                            //{
                            //    CheckTireForCollision(Player.CurrentVehicle.Vehicle.Wheels[5]);
                            //}



                        }
                    }
                    GameFiber.Yield();
                }
            }
            catch (Exception e)
            {
                EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
            }
        }, "SpikeStripTireChecker2");
    }
    private void CheckTireForCollision(Rage.VehicleWheel wheel)
    {
        if (wheel.TireHealth != 0)
        {
            if (NativeFunction.Natives.DOES_OBJECT_OF_TYPE_EXIST_AT_COORDS<bool>(wheel.LastContactPoint.X, wheel.LastContactPoint.Y, wheel.LastContactPoint.Z, 1.0f, Game.GetHashKey(SpikeStripName), 0))
            {
                wheel.BurstTire();
            }
        }
    }
    private bool GetPosition()
    {
        return NativeFunction.Natives.GET_CLOSEST_VEHICLE_NODE_WITH_HEADING<bool>(InitialPosition.X, InitialPosition.Y, InitialPosition.Z, out NodeCenter, out NodeHeading, 0, 3.0f, 0);
    }
    private float GetPositionBetweenVehicles(float additionalSpacing)
    {
        return VehicleModel.Dimensions.Y + additionalSpacing;
    }
    private float GetPositionBetweenSpikeStrips(float additionalSpacing)
    {
        return SpikeStripModel.Dimensions.Y + additionalSpacing;
    }
    private void DeterminSpikeStripPositions()
    {
        Vector3 LeftSide = NativeHelper.GetOffsetPosition(NodeCenter, NodeHeading + 90f, 15f);
        Vector3 RightSide = NativeHelper.GetOffsetPosition(NodeCenter, NodeHeading - 90f, 15f);
        if (Player.Position.DistanceTo2D(RightSide) <= Player.Position.DistanceTo2D(LeftSide))
        {
            NodeOffset = RightSide;
        }
        else
        {
            NodeOffset = LeftSide;
        }
    }
    private void FillInBlockade()
    {
        DeterminSpikeStripPositions();
        if (!CreateVehicle(NodeCenter, RotatedNodeHeading))
        {
            return;
        }
        AddVehicles(true);
        AddVehicles(false);
        CreateSpikeStrip(NodeOffset, RotatedNodeHeading);
        AddSpikeStrips(true);
        AddSpikeStrips(false);



        // PLACE_OBJECT_ON_GROUND_PROPERLY


        //DeterminSpikeStripPositions();
        //GameFiber.StartNew(delegate
        //{
        //    try
        //    {
        //        if(!CreateVehicle(NodeCenter,RotatedNodeHeading))
        //        {
        //            return;
        //        }
        //        AddVehicles(true);
        //        AddVehicles(false);

        //      //  CreateSpikeStrip(NodeOffset,RotatedNodeHeading);
        //       // AddSpikeStrips(true);
        //     //   AddSpikeStrips(false);
        //        GameFiber.Sleep(2000);
        //        uint GameTimeStarted = Game.GameTime;
        //        while (Game.GameTime - GameTimeStarted <= 3000 && !IsDisposed)
        //        {
        //            foreach (Vehicle Car in CreatedVehicles)
        //            {
        //                if (Car.Exists())
        //                {
        //                    if (!Car.IsOnAllWheels)
        //                    {
        //                        Car.Delete();
        //                    }
        //                }
        //            }
        //            GameFiber.Sleep(250);
        //        }
        //        //while (!Game.IsKeyDownRightNow(Keys.E))
        //        //{
        //        //    Game.DisplayHelp("Press E to delete roadblock");
        //        //    //Rage.Debug.DrawArrowDebug(new Vector3(NodeCenter.X, NodeCenter.Y, NodeCenter.Z + 1f), FrontVector, Rotator.Zero, 1f, Color.Red);
        //        //    GameFiber.Yield();
        //        //}
        //        //RemoveItems();
        //    }
        //    catch (Exception e)
        //    {
        //        RemoveItems();
        //        EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
        //    }
        //}, "DebugLoop2");
    }
    private void RemoveItems()
    {
        foreach (Vehicle veh in CreatedRoadblockVehicles)
        {
            if (veh.Exists())
            {
                veh.Delete();
            }
        }
        foreach (Rage.Object prop in CreatedProps)
        {
            if (prop.Exists())
            {
                prop.Delete();
            }
        }
    }
    private void AddVehicles(bool InFront)
    {
        int CarsAdded = 1;
        float Spacing = GetPositionBetweenVehicles(1.0f);//0.5f
        bool Created;
        do
        {
            Vector3 SpawnPosition = NativeHelper.GetOffsetPosition(NodeCenter, NodeHeading, (InFront ? 1.0f : -1.0f) * CarsAdded * Spacing);
            Created = CreateVehicle(SpawnPosition, RotatedNodeHeading);
            if (Created)
            {
                CarsAdded++;
            }
        } while (Created && CarsAdded < 5);//5
    }
    private bool CreateVehicle(Vector3 position, float heading)
    {
        SpawnTask spawnTask = new SpawnTask(Agency, position, position, heading, Vehicle, null, true, Settings);
        spawnTask.AttemptSpawn();
        spawnTask.CreatedPeople.ForEach(x => World.AddEntity(x));
        foreach(VehicleExt created in spawnTask.CreatedVehicles)
        {
            if(created.Vehicle.Exists())
            {
                if(!created.Vehicle.IsOnAllWheels)
                {
                    created.Vehicle.Delete();
                }
            }
            if (created.Vehicle.Exists())
            {
                World.AddEntity(created);
                CreatedRoadblockVehicles.Add(created.Vehicle);
                created.Vehicle.IsCollisionEnabled = true;
                created.Vehicle.IsGravityDisabled = false;
                created.Vehicle.IsSirenOn = true;
                created.Vehicle.IsSirenSilent = true;
            }
        }
        return spawnTask.CreatedVehicles.Any();
    }
    private void AddSpikeStrips(bool InFront)
    {
        int StripsAdded = 1;
        float Spacing = GetPositionBetweenSpikeStrips(0.25f);
        bool Created;
        do
        {
            Vector3 SpawnPosition = NativeHelper.GetOffsetPosition(NodeOffset, NodeHeading, (InFront ? 1.0f : -1.0f) * StripsAdded * Spacing);
            Created = CreateSpikeStrip(SpawnPosition, RotatedNodeHeading);
            if (Created)
            {
                StripsAdded++;
            }
        } while (Created && StripsAdded < 6);
    }
    private bool CreateSpikeStrip(Vector3 position, float heading)
    {
        position = new Vector3(position.X, position.Y, position.Z + 30f);
        if (NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD<bool>(position.X, position.Y, position.Z, out float GroundZ, true, false))
        {
            EntryPoint.WriteToConsole($"DISPATCHER: SpikeStrip Position Ground Z Found", 3);
            position = new Vector3(position.X, position.Y, GroundZ);

            Rage.Object SpikeStrip = new Rage.Object("p_ld_stinger_s", position, heading);
            NativeFunction.Natives.PLACE_OBJECT_ON_GROUND_PROPERLY(SpikeStrip);
            SpikeStrip.IsPersistent = true;
            SpikeStrip.IsGravityDisabled = false;
            CreatedProps.Add(SpikeStrip);
            return SpikeStrip.Exists();
        }
        return false;
    }
}

