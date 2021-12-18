using ExtensionsMethods;
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
    private Vector3 PedPosition;
    private float NodeHeading;
    private float PedHeading;
    private Model VehicleModel;
    private DispatchablePerson Person;
    private DispatchableVehicle Vehicle;
    private string SpikeStripName = "p_ld_stinger_s";
    private Model SpikeStripModel;
    private List<Vehicle> CreatedRoadblockVehicles = new List<Vehicle>();
    private List<PedExt> CreatedRoadblockPeds = new List<PedExt>();
    private Vehicle MainVehicle;
    private Vector3 InitialPosition;
    private List<Vector3> SpawnPoints = new List<Vector3>();
    private List<Rage.Object> CreatedProps = new List<Rage.Object>();
    private IDispatchable Player;
    private IEntityProvideable World;
    private Agency Agency;
    private bool IsDisposed;
    private ISettingsProvideable Settings;
    private IWeapons Weapons;
    private INameProvideable Names;
    private List<string> ConeTypes = new List<string>() { "prop_roadcone01a", "prop_roadcone01b", "prop_roadcone01c", "prop_roadcone02a", "prop_roadcone02c", "prop_roadcone02b", "prop_mp_cone_01", "prop_mp_cone_02", "prop_mp_cone_03" };
    private float RotatedNodeHeading => NodeHeading - 90f;
    public Roadblock(IDispatchable player, IEntityProvideable world,Agency agency, DispatchableVehicle vehicle, DispatchablePerson person, Vector3 initialPosition, ISettingsProvideable settings, IWeapons weapons, INameProvideable names)
    {
        Player = player;
        World = world;
        Agency = agency;
        Vehicle = vehicle;
        Person = person;
        InitialPosition = initialPosition;
        Settings = settings;
        VehicleModel = new Model(Vehicle.ModelName);
        SpikeStripModel = new Model(SpikeStripName);
        Weapons = weapons;
        Names = names;
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

                foreach (Rage.Object obj in CreatedProps)
                {
                    NativeFunction.Natives.PLACE_OBJECT_ON_GROUND_PROPERLY(obj);
                }


                float DistanceToRoadblock = Player.Position.DistanceTo2D(CenterPosition);
                while (!IsDisposed && DistanceToRoadblock >= 125f)
                {
                    DistanceToRoadblock = Player.Position.DistanceTo2D(CenterPosition);
                    foreach (Rage.Object obj in CreatedProps)
                    {
                        NativeFunction.Natives.PLACE_OBJECT_ON_GROUND_PROPERLY(obj);
                    }
                    GameFiber.Sleep(500);
                }

                while (!IsDisposed)
                {
                    //Rage.Debug.DrawArrowDebug(new Vector3(NodeCenter.X, NodeCenter.Y, NodeCenter.Z + 2.0f), Vector3.Zero, Rotator.Zero, 1f, Color.Yellow);
                    //Rage.Debug.DrawArrowDebug(new Vector3(NodeOffset.X, NodeOffset.Y,NodeOffset.Z + 2.0f), Vector3.Zero, Rotator.Zero, 1f, Color.Red);
                    //Vector3 FrontRight = new Vector3(NodeCenter.X + VehicleModel.Dimensions.X / 2, NodeCenter.Y + VehicleModel.Dimensions.Y / 2, NodeCenter.Z+ 2.0f);
                    //Vector3 FrontLeft = new Vector3(NodeCenter.X - VehicleModel.Dimensions.X / 2, NodeCenter.Y + VehicleModel.Dimensions.Y / 2, NodeCenter.Z + 2.0f);
                    //Vector3 RearRight = new Vector3(NodeCenter.X + VehicleModel.Dimensions.X / 2, NodeCenter.Y - VehicleModel.Dimensions.Y / 2, NodeCenter.Z + 2.0f);
                    //Vector3 RearLeft = new Vector3(NodeCenter.X - VehicleModel.Dimensions.X / 2, NodeCenter.Y - VehicleModel.Dimensions.Y / 2, NodeCenter.Z + 2.0f);
                    //Rage.Debug.DrawArrowDebug(FrontRight, Vector3.Zero, Rotator.Zero, 1f, Color.White);
                    //Rage.Debug.DrawArrowDebug(FrontLeft, Vector3.Zero, Rotator.Zero, 1f, Color.Black);
                    //Rage.Debug.DrawArrowDebug(RearRight, Vector3.Zero, Rotator.Zero, 1f, Color.White);
                    //Rage.Debug.DrawArrowDebug(RearLeft, Vector3.Zero, Rotator.Zero, 1f, Color.Black);

                    if (Player.IsInVehicle && Player.CurrentVehicle != null)
                    {
                        if (Player.CurrentVehicle.Vehicle.Exists())
                        {
                            DistanceToRoadblock = Player.Position.DistanceTo2D(CenterPosition);
                            //'0 = wheel_lf / bike, plane or jet front
                            //'1 = wheel_rf
                            //'2 = wheel_lm / in 6 wheels trailer, plane or jet is first one on left
                            //'3 = wheel_rm / in 6 wheels trailer, plane or jet is first one on right
                            //'4 = wheel_lr / bike rear / in 6 wheels trailer, plane or jet is last one on left
                            //'5 = wheel_rr / in 6 wheels trailer, plane or jet is last one on right
                            if (DistanceToRoadblock <= 100)
                            {
                                List<(int, string)> WheelList = new List<(int, string)>() { (0, "wheel_lf"), (1, "wheel_rf"), (2, "wheel_lm"), (3, "wheel_rm"), (4, "wheel_lr"), (5, "wheel_rr") };
                                foreach ((int, string) wheelItem in WheelList)
                                {
                                    if (Player.CurrentVehicle.Vehicle.HasBone(wheelItem.Item2) && Player.CurrentVehicle.Vehicle.HasBone(Player.CurrentVehicle.Vehicle.GetBoneIndex(wheelItem.Item2)))
                                    {
                                        CheckTireForCollision(Player.CurrentVehicle.Vehicle.Wheels[wheelItem.Item1]);
                                    }
                                }
                            }
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
            PedPosition = LeftSide;
            PedHeading = NodeHeading +180f;
        }
        else
        {
            NodeOffset = LeftSide;
            PedPosition = RightSide;
            PedHeading = NodeHeading;
        }
    }
    private void FillInBlockade()
    {
        DeterminSpikeStripPositions();
        if (!CreateVehicle(NodeCenter, RotatedNodeHeading,true))
        {
            return;
        }
        AddVehicles(true);
        AddVehicles(false);
        CreateSpikeStrip(NodeOffset, RotatedNodeHeading);
        AddSpikeStrips(true);
        AddSpikeStrips(false);
    }
    private void RemoveItems()
    {
        foreach (Vehicle veh in CreatedRoadblockVehicles)
        {
            if (veh.Exists())
            {
                if (veh.DistanceTo2D(Player.Character) >= 250f)
                {
                    veh.Delete();
                    EntryPoint.PersistentVehiclesDeleted++;
                }
                else
                {
                    veh.IsPersistent = false;
                    EntryPoint.PersistentVehiclesNonPersistent++;
                }
            }
        }
        foreach (PedExt pedext in CreatedRoadblockPeds)
        {
            if (pedext != null && pedext.Pedestrian.Exists())
            {
                if (pedext.DistanceToPlayer >= 250f)
                {
                    pedext.Pedestrian.Delete();
                    EntryPoint.PersistentPedsDeleted++;
                }
                else
                {
                    pedext.Pedestrian.IsPersistent = false;
                    EntryPoint.PersistentPedsNonPersistent++;
                }                
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
            Created = CreateVehicle(SpawnPosition, RotatedNodeHeading, false);
            if (Created)
            {
                CarsAdded++;
            }
        } while (Created && CarsAdded < 2);//5
    }
    private bool CreateVehicle(Vector3 position, float heading, bool addPed)
    {
        Vector3 FrontRight = new Vector3(position.X + VehicleModel.Dimensions.X / 2, position.Y + VehicleModel.Dimensions.Y / 2, position.Z);
        Vector3 FrontLeft = new Vector3(position.X - VehicleModel.Dimensions.X / 2, position.Y + VehicleModel.Dimensions.Y / 2, position.Z);
        Vector3 RearRight = new Vector3(position.X + VehicleModel.Dimensions.X / 2, position.Y - VehicleModel.Dimensions.Y / 2, position.Z);
        Vector3 RearLeft = new Vector3(position.X - VehicleModel.Dimensions.X / 2, position.Y - VehicleModel.Dimensions.Y / 2, position.Z);
        float GroundZ;
        if (NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD<bool>(FrontRight.X, FrontRight.Y, FrontRight.Z, out GroundZ, true, false))
        {
            if (GroundZ == 0 || Math.Abs(GroundZ - position.Z) >= 0.5f)
            {
                return false;
            }
        }
        if (NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD<bool>(FrontLeft.X, FrontLeft.Y, FrontLeft.Z, out GroundZ, true, false))
        {
            if (GroundZ == 0 || Math.Abs(GroundZ - position.Z) >= 0.5f)
            {
                return false;
            }
        }
        if (NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD<bool>(RearRight.X, RearRight.Y, RearRight.Z, out GroundZ, true, false))
        {
            if (GroundZ == 0 || Math.Abs(GroundZ - position.Z) >= 0.5f)
            {
                return false;
            }
        }
        if (NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD<bool>(RearLeft.X, RearLeft.Y, RearLeft.Z, out GroundZ, true, false))
        {
            if (GroundZ == 0 || Math.Abs(GroundZ - position.Z) >= 0.5f)
            {
                return false;
            }
        }
        SpawnTask spawnTask = new SpawnTask(Agency, position, position, heading, Vehicle, null, false, Settings, Weapons, Names);
        spawnTask.AttemptSpawn();
        foreach(VehicleExt roadblockCar in spawnTask.CreatedVehicles)
        {
            roadblockCar.WasSpawnedEmpty = true;
        }
        if (addPed)
        {
            SpawnTask pedSpawn = new SpawnTask(Agency, PedPosition, PedPosition, PedHeading, null, Person, Settings.SettingsManager.PoliceSettings.ShowSpawnedBlips, Settings, Weapons, Names);
            pedSpawn.AttemptSpawn();
            foreach(PedExt person in pedSpawn.CreatedPeople)
            {
                World.AddEntity(person);
                CreatedRoadblockPeds.Add(person);
            }
        }
        spawnTask.CreatedPeople.ForEach(x => World.AddEntity(x));
        foreach(VehicleExt created in spawnTask.CreatedVehicles)
        {
            if(created.Vehicle.Exists())
            {
                if(!created.Vehicle.IsOnAllWheels)
                {
                    created.Vehicle.Delete();
                    EntryPoint.PersistentVehiclesDeleted++;
                }
            }
            if (created.Vehicle.Exists())
            {
                World.AddEntity(created, ResponseType.LawEnforcement);
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
                Vector3 ConePosition = NativeHelper.GetOffsetPosition(SpawnPosition, PedHeading, 5.0f);
                CreateCone(ConePosition, 0f);//heading doesnt matter on a cone (big brain time)...
                StripsAdded++;
            }
        } while (Created && StripsAdded < 5);
    }
    private bool CreateSpikeStrip(Vector3 position, float heading)
    {
        position = new Vector3(position.X, position.Y, position.Z + 1.0f);
        if (NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD<bool>(position.X, position.Y, position.Z, out float GroundZ, true, false))
        {
            position = new Vector3(position.X, position.Y, GroundZ);
        }
        Rage.Object SpikeStrip = new Rage.Object("p_ld_stinger_s", position, heading);
        NativeFunction.Natives.PLACE_OBJECT_ON_GROUND_PROPERLY(SpikeStrip);
        SpikeStrip.IsPersistent = true;
        SpikeStrip.IsGravityDisabled = false;
        SpikeStrip.IsPositionFrozen = true;
        CreatedProps.Add(SpikeStrip);
        return SpikeStrip.Exists();
    }
    private bool CreateCone(Vector3 position, float heading)
    {
        position = new Vector3(position.X, position.Y, position.Z + 1.0f);
        if (NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD<bool>(position.X, position.Y, position.Z, out float GroundZ, true, false))
        {
            position = new Vector3(position.X, position.Y, GroundZ);
        }


        Rage.Object Cone = new Rage.Object(ConeTypes.PickRandom(), position, heading);
       // NativeFunction.Natives.PLACE_OBJECT_ON_GROUND_PROPERLY(Cone);
        Cone.IsPersistent = true;
        //Cone.IsGravityDisabled = false;
        CreatedProps.Add(Cone);
        return Cone.Exists();
    }
}

