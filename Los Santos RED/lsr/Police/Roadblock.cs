using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class Roadblock
{
    private Vector3 NodeCenter;
    private float NodeHeading;
    private float VehicleHeading;
    private string VehicleModel;
    private Vector3 FrontVector;
    private List<Vehicle> CreatedVehicles = new List<Vehicle>();
    private Vehicle MainVehicle;
    private Vector3 InitialPosition;
    private List<Vector3> SpawnPoints = new List<Vector3>();
    public Roadblock(string vehicleModel, Vector3 initialPosition)
    {
        VehicleModel = vehicleModel;
        InitialPosition = initialPosition;
    }
    public void SpawnRoadblock()
    {
        if (GetPosition())
        {
            CreateCars();
        }
    }
    private bool GetPosition()
    {
        return NativeFunction.Natives.GET_CLOSEST_VEHICLE_NODE_WITH_HEADING<bool>(InitialPosition.X, InitialPosition.Y, InitialPosition.Z, out NodeCenter, out NodeHeading, 0, 3.0f, 0);
    }
    private float GetPositionBetweenVehicles(float additionalSpacing)
    {
        Model Model = new Model(VehicleModel);
        return Model.Dimensions.Length() + additionalSpacing;
    }
    private void CreateCars()
    {
        VehicleHeading = NodeHeading - 90f;
        FrontVector = new Vector3((float)Math.Sin(VehicleHeading * Math.PI / 180), (float)Math.Cos(VehicleHeading * Math.PI / 180), 0);
       // FrontVector = new Vector3((float)Math.Cos(VehicleHeading * Math.PI / 180), (float)Math.Sin(VehicleHeading * Math.PI / 180), 0);

        GameFiber.StartNew(delegate
        {
            try
            {

                MainVehicle = new Vehicle(VehicleModel, NodeCenter, VehicleHeading);
                GameFiber.Yield();
                if (MainVehicle.Exists())
                {
                    EntryPoint.WriteToConsole($"Vehicle Added Middle", 0);
                    CreatedVehicles.Add(MainVehicle);
                }
                else
                {
                    return;
                }
                AddVehicles(true);
                AddVehicles(false);

                foreach (Vehicle veh in CreatedVehicles)
                {
                    if (veh.Exists())
                    {
                        if(MainVehicle.Exists() && MainVehicle.Handle == veh.Handle)
                        {

                        }
                        else
                        {
                            veh.Delete();
                        }
                        
                    }
                }
                while (!Game.IsKeyDownRightNow(Keys.E))
                {
                    Game.DisplayHelp("Press E to delete roadblock");

                    foreach(Vector3 spawnpos in SpawnPoints)
                    {
                        Rage.Debug.DrawArrowDebug(new Vector3(spawnpos.X, spawnpos.Y, spawnpos.Z + 1f), FrontVector, Rotator.Zero, 1f, Color.White);
                    }
                    Rage.Debug.DrawArrowDebug(new Vector3(NodeCenter.X, NodeCenter.Y, NodeCenter.Z + 1f), FrontVector, Rotator.Zero, 1f, Color.Red);
                    GameFiber.Sleep(25);


                }
                foreach (Vehicle veh in CreatedVehicles)
                {
                    if (veh.Exists())
                    {
                        veh.Delete();
                    }
                }
            }
            catch (Exception e)
            {
                foreach (Vehicle veh in CreatedVehicles)
                {
                    if (veh.Exists())
                    {
                        veh.Delete();
                    }
                }
                EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
            }
        }, "DebugLoop2");
    }
    private void AddVehicles(bool InFront)
    {
        int CarsAdded = 1;
        float Spacing = GetPositionBetweenVehicles(1f);
        bool Created;
        Vehicle LastCreated = MainVehicle;
        SpawnPoints.Add(NodeCenter);
        do
        {
            Vector3 SpawnPosition;
            Vector3 SpawnPosition2;
            if (InFront)
            {
                SpawnPosition = LastCreated.GetOffsetPositionFront(Spacing);
                SpawnPosition2 = NodeCenter + (FrontVector * CarsAdded);// * Spacing);
            }
            else
            {
                SpawnPosition = LastCreated.GetOffsetPositionFront(-1.0f * Spacing);
                SpawnPosition2 = NodeCenter + (FrontVector * -CarsAdded);// * Spacing);
            }
            Vehicle Car = new Vehicle(VehicleModel, SpawnPosition, VehicleHeading);
            GameFiber.Yield();
            if (Car.Exists() && NativeFunction.Natives.SET_VEHICLE_ON_GROUND_PROPERLY<bool>(Car))
            {
                //Car.Position = new Vector3(Car.Position.X,Car.Position.Y,MainVehicle.Position.Z);
                bool OnGround = NativeFunction.Natives.SET_VEHICLE_ON_GROUND_PROPERLY<bool>(Car);
                SpawnPoints.Add(SpawnPosition2);
                LastCreated = Car;
                Car.IsCollisionEnabled = true;
                Car.IsGravityDisabled = false;
                EntryPoint.WriteToConsole($"Vehicle Added InFront: {InFront} OnGround {OnGround} Heading {VehicleHeading}", 0);
                CreatedVehicles.Add(Car);
                if (Car.HasSiren)
                {
                    Car.IsSirenOn = true;
                }
                Created = true;
                CarsAdded++;
            }
            else
            {
                if (Car.Exists())
                {
                    Car.Delete();
                }
                Created = false;
            }
        } while (Created && CarsAdded < 3);
    }
}

