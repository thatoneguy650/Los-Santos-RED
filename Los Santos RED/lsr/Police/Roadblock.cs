using LosSantosRED.lsr.Interface;
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
    private Vector3 NodeOffset;
    private float NodeHeading;
    private float VehicleHeading;
    private string VehicleName;
    private Model VehicleModel;
    private string SpikeStripName = "p_ld_stinger_s";
    private Model SpikeStripModel;
    private Vector3 FrontVector;
    private List<Vehicle> CreatedVehicles = new List<Vehicle>();
    private Vehicle MainVehicle;
    private Vector3 InitialPosition;
    private List<Vector3> SpawnPoints = new List<Vector3>();
    private List<Rage.Object> CreatedProps = new List<Rage.Object>();
    private IPoliceRespondable Player;
    public Roadblock(IPoliceRespondable player, string vehicleModel, Vector3 initialPosition)
    {
        Player = player;
        VehicleName = vehicleModel;
        InitialPosition = initialPosition;
        VehicleModel = new Model(VehicleName);
        SpikeStripModel = new Model(SpikeStripName);
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
    private void DeterminVehiclePositions()
    {
        VehicleHeading = NodeHeading - 90f;
        FrontVector = new Vector3((float)Math.Cos(NodeHeading * Math.PI / 180), (float)Math.Sin(NodeHeading * Math.PI / 180), 0);
    }
    private void DeterminSpikeStripPositions()
    {
        float SpikeStripHeading = NodeHeading + 90f;
        Vector3 SideVector = new Vector3((float)Math.Cos(SpikeStripHeading * Math.PI / 180), (float)Math.Sin(SpikeStripHeading * Math.PI / 180), 0);
        float DistanceFromVehicles = 20f;
        if (Player.Position.DistanceTo2D(NodeCenter + (DistanceFromVehicles * SideVector)) <= Player.Position.DistanceTo2D(NodeCenter + (-DistanceFromVehicles * SideVector)))
        {
            NodeOffset = NodeCenter + (DistanceFromVehicles * SideVector);
        }
        else
        {
            NodeOffset = NodeCenter + (-DistanceFromVehicles * SideVector);
        }
    }
    private void FillInBlockade()
    {
        DeterminVehiclePositions();
        DeterminSpikeStripPositions();
        GameFiber.StartNew(delegate
        {
            try
            {
                if(!CreateVehicle(NodeCenter))
                {
                    return;
                }
                AddVehicles(true);
                AddVehicles(false);

                CreateSpikeStrip(NodeOffset);
                AddSpikeStrips(true);
                AddSpikeStrips(false);
                GameFiber.Sleep(500);

                foreach (Vehicle Car in CreatedVehicles)
                {
                    if (Car.Exists())
                    {
                        if (!NativeFunction.Natives.SET_VEHICLE_ON_GROUND_PROPERLY<bool>(Car, 5.0f) || !Car.IsOnAllWheels)
                        {
                            Car.Delete();
                        }
                    }
                }

                while (!Game.IsKeyDownRightNow(Keys.E))
                {
                    Game.DisplayHelp("Press E to delete roadblock");
                    //Rage.Debug.DrawArrowDebug(new Vector3(NodeCenter.X, NodeCenter.Y, NodeCenter.Z + 1f), FrontVector, Rotator.Zero, 1f, Color.Red);
                    GameFiber.Sleep(25);
                }
                RemoveItems();
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
            Vector3 SpawnPosition = NodeCenter + (FrontVector * (InFront ? 1.0f : -1.0f) * CarsAdded * Spacing);
            Created = CreateVehicle(SpawnPosition);
            if (Created)
            {
                CarsAdded++;
            }
        } while (Created && CarsAdded < 3);
    }
    private bool CreateVehicle(Vector3 SpawnPosition)
    {
        Vehicle Car = new Vehicle(VehicleName, SpawnPosition, VehicleHeading);
        GameFiber.Yield();
        if (Car.Exists() && NativeFunction.Natives.SET_VEHICLE_ON_GROUND_PROPERLY<bool>(Car, 5.0f) && Car.IsOnAllWheels)
        {
            SpawnPoints.Add(SpawnPosition);
            CreatedVehicles.Add(Car);
            if(MainVehicle == null)
            {
                MainVehicle = Car;
            }
            if (Car.HasSiren)
            {
                Car.IsSirenOn = true;
            }
            return true;
        }
        else
        {
            if (Car.Exists())
            {
                Car.Delete();
            }
            return false;
        }
    }
    private void AddSpikeStrips(bool InFront)
    {
        int StripsAdded = 1;
        float Spacing = GetPositionBetweenSpikeStrips(0.25f);
        bool Created;
        do
        {
            Vector3 SpawnPosition = NodeOffset + (FrontVector * (InFront ? 1.0f : -1.0f) * StripsAdded * Spacing);
            Created = CreateSpikeStrip(SpawnPosition);
            if (Created)
            {
                StripsAdded++;
            }
        } while (Created && StripsAdded < 5);
    }
    private bool CreateSpikeStrip(Vector3 Position)
    {
        if(NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD<bool>(Position.X, Position.Y, Position.Z, out float GroundZ, true, false))
        {
            Position = new Vector3(Position.X, Position.Y, GroundZ);
        }
        Rage.Object SpikeStrip = new Rage.Object("p_ld_stinger_s", Position, VehicleHeading);
        CreatedProps.Add(SpikeStrip);
        return SpikeStrip.Exists();
    }


}

