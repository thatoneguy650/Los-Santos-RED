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
    private List<Vehicle> CreatedVehicles = new List<Vehicle>();
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
        Vector3 LeftSide = NativeHelper.GetOffsetPosition(NodeCenter, NodeHeading + 90f, 20f);
        Vector3 RightSide = NativeHelper.GetOffsetPosition(NodeCenter, NodeHeading - 90f, 20f);
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
        GameFiber.StartNew(delegate
        {
            try
            {
                if(!CreateVehicle(NodeCenter,RotatedNodeHeading))
                {
                    return;
                }
                AddVehicles(true);
                AddVehicles(false);

                CreateSpikeStrip(NodeOffset,RotatedNodeHeading);
                AddSpikeStrips(true);
                AddSpikeStrips(false);
                GameFiber.Sleep(2000);
                uint GameTimeStarted = Game.GameTime;
                while (Game.GameTime - GameTimeStarted <= 3000 && !IsDisposed)
                {
                    foreach (Vehicle Car in CreatedVehicles)
                    {
                        if (Car.Exists())
                        {
                            if (!Car.IsOnAllWheels)
                            {
                                Car.Delete();
                            }
                        }
                    }
                    GameFiber.Sleep(250);
                }
                //while (!Game.IsKeyDownRightNow(Keys.E))
                //{
                //    Game.DisplayHelp("Press E to delete roadblock");
                //    //Rage.Debug.DrawArrowDebug(new Vector3(NodeCenter.X, NodeCenter.Y, NodeCenter.Z + 1f), FrontVector, Rotator.Zero, 1f, Color.Red);
                //    GameFiber.Yield();
                //}
                //RemoveItems();
            }
            catch (Exception e)
            {
                RemoveItems();
                EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
            }
        }, "DebugLoop2");
    }
    private void RemoveItems()
    {
        foreach (Vehicle veh in CreatedVehicles)
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
        float Spacing = GetPositionBetweenVehicles(0.5f);
        bool Created;
        do
        {
            Vector3 SpawnPosition = NativeHelper.GetOffsetPosition(NodeCenter, NodeHeading, (InFront ? 1.0f : -1.0f) * CarsAdded * Spacing);
            Created = CreateVehicle(SpawnPosition, RotatedNodeHeading);
            if (Created)
            {
                CarsAdded++;
            }
        } while (Created && CarsAdded < 5);
    }
    private bool CreateVehicle(Vector3 position, float heading)
    {

        SpawnTask spawnTask = new SpawnTask(Agency, position, position, heading, Vehicle, null, true, Settings);
        spawnTask.AttemptSpawn();
        spawnTask.CreatedPeople.ForEach(x => World.AddEntity(x));
        spawnTask.CreatedVehicles.ForEach(x => World.AddEntity(x));
        spawnTask.CreatedVehicles.ForEach(x => x.Vehicle.IsCollisionEnabled = true);
        spawnTask.CreatedVehicles.ForEach(x => x.Vehicle.IsGravityDisabled = false);
        return spawnTask.CreatedVehicles.Any();





        //Vehicle Car = new Vehicle(VehicleName, position, heading);
        //GameFiber.Yield();
        //if (Car.Exists() && NativeFunction.Natives.SET_VEHICLE_ON_GROUND_PROPERLY<bool>(Car, 5.0f) && Car.IsOnAllWheels)
        //{
        //    Car.IsPersistent = true;


        //    Blip myBlip = Car.AttachBlip();
        //    myBlip.Color = Color.Green;
        //    myBlip.Scale = 0.6f;


        //    World.AddEntity(new VehicleExt(Car, true));
        //    SpawnPoints.Add(position);
        //    CreatedVehicles.Add(Car);
        //    if(MainVehicle == null)
        //    {
        //        MainVehicle = Car;
        //    }
        //    if (Car.HasSiren)
        //    {
        //        Car.IsSirenOn = true;
        //    }
        //    return true;
        //}
        //else
        //{
        //    if (Car.Exists())
        //    {
        //        Car.Delete();
        //    }
        //    return false;
        //}
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
        position = new Vector3(position.X, position.Y, position.Z + 3f);
        if (NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD<bool>(position.X, position.Y, position.Z, out float GroundZ, true, false))
        {
            position = new Vector3(position.X, position.Y, GroundZ);
        }
        Rage.Object SpikeStrip = new Rage.Object("p_ld_stinger_s", position, heading);
        SpikeStrip.IsPersistent = true;
        SpikeStrip.IsCollisionEnabled = true;
        SpikeStrip.IsGravityDisabled = false;
        CreatedProps.Add(SpikeStrip);
        return SpikeStrip.Exists();
    }
}

