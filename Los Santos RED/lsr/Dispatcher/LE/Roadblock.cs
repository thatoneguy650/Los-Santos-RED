using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

public class Roadblock
{
    private Agency Agency;
    private List<string> ConeTypes = new List<string>() { "prop_roadcone01a", "prop_roadcone01b", "prop_roadcone01c", "prop_roadcone02a", "prop_roadcone02c", "prop_roadcone02b", "prop_mp_cone_01", "prop_mp_cone_02", "prop_mp_cone_03" };
    private List<Rage.Object> CreatedProps = new List<Rage.Object>();
    private List<PedExt> CreatedRoadblockPeds = new List<PedExt>();
    private List<Vehicle> CreatedRoadblockVehicles = new List<Vehicle>();
    private Vector3 InitialPosition;
    private bool IsDisposed;
    private INameProvideable Names;
    private Vector3 NodeCenter;
    private float NodeHeading;
    private Vector3 NodeOffset;
    private Vector3 BarrierOffset;
    private Vector3 ConeOffset;
    private float PedHeading;
    private Vector3 PedPosition;
    private DispatchablePerson Person;
    private IDispatchable Player;
    private ISettingsProvideable Settings;
    private List<Vector3> SpawnPoints = new List<Vector3>();
    private Model SpikeStripModel;
    private string SpikeStripName = "p_ld_stinger_s";
    private DispatchableVehicle Vehicle;
    private Model VehicleModel;
    private IWeapons Weapons;
    private IEntityProvideable World;
    private IModItems ModItems;
    private float InitialHeading;
    private bool AllowAnySpawn;
    private RoadNode RoadNode;
    private int roadblockSpeedZoneID1;
    private int roadblockSpeedZoneID2;
    private Vector3 PedSideOfRoadPosition;
    private float PedSideOfRoadHeading;

    private int VehiclesToAddFront = 0;
    private int VehiclesToAddRear = 0;
    private int BarriersToAddFront = 0;
    private int BarriersToAddRear = 0;

    private Vector3 VehicleNodeCenter;
    private bool HasSpawnedAnySpikeStrips = false;
    private bool HasSpawnedAnyBarrierProps;

    public bool EnableCarBlocks { get; set; } = true;
    public bool EnableSpikeStrips { get; set; } = true;
    public bool EnableOtherBarriers { get; set; } = true;
    private bool EnableBlockadeProps => EnableOtherBarriers || EnableSpikeStrips;
    private bool ShouldMonitorSpawnedProps => HasSpawnedAnySpikeStrips || HasSpawnedAnyBarrierProps;

    private enum LocationCreate
    {
        Middle = 0,
        Front = 1,
        Back = 2,
    }
    public Roadblock(IDispatchable player, IEntityProvideable world, Agency agency, DispatchableVehicle vehicle, DispatchablePerson person, Vector3 initialPosition, float initialHeading, ISettingsProvideable settings, 
        IWeapons weapons, INameProvideable names, bool allowAnySpawn, IModItems modItems, bool enableCarBlocks, bool enableSpikeStrips, bool enableOtherBarriers)
    {
        Player = player;
        World = world;
        Agency = agency;
        Vehicle = vehicle;
        Person = person;
        InitialPosition = initialPosition;
        InitialHeading = initialHeading;
        Settings = settings;
        VehicleModel = new Model(Vehicle.ModelName);
        SpikeStripModel = new Model(SpikeStripName);
        Weapons = weapons;
        Names = names;
        AllowAnySpawn = allowAnySpawn;
        ModItems = modItems;
        EnableCarBlocks = enableCarBlocks;
        EnableSpikeStrips = enableSpikeStrips;
        EnableOtherBarriers = enableOtherBarriers;
    }
    public Vector3 CenterPosition => NodeCenter;
    private float RotatedNodeHeading => NodeHeading - 90f;
    public void Dispose()
    {
        IsDisposed = true;
        RemoveItems();
        UnSetSpeedZone();
        UnSetRoadGenerators();
    }
    public void SpawnRoadblock()
    {
        GameFiber.Yield();
        AnimationDictionary.RequestAnimationDictionay("p_ld_stinger_s");
        if (GetPosition())
        {
            GameFiber.Yield();
            ClearArea();
            GameFiber.Yield();
            FillInBlockade();
        }
        GameFiber.Yield();
        if (ShouldMonitorSpawnedProps)
        {
            MonitorSpawnedProps();
        }
    }
    private void ClearArea()
    {
        if (Settings.SettingsManager.RoadblockSettings.RemoveGeneratedVehiclesAroundRoadblock)
        {
            NativeFunction.Natives.CLEAR_AREA(CenterPosition.X, CenterPosition.Y, CenterPosition.Z, Settings.SettingsManager.RoadblockSettings.RemoveGeneratedVehiclesAroundRoadblockDistance, true, false, false, false);
            NativeFunction.Natives.CLEAR_AREA(NodeOffset.X, NodeOffset.Y, NodeOffset.Z, Settings.SettingsManager.RoadblockSettings.RemoveGeneratedVehiclesAroundRoadblockDistance, true, false, false, false);
        }
    }
    private void MonitorSpawnedProps()
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                foreach (Rage.Object obj in CreatedProps)
                {
                    if (obj.Exists())
                    {
                        NativeFunction.Natives.PLACE_OBJECT_ON_GROUND_PROPERLY(obj);
                    }
                    GameFiber.Yield();
                }
                float DistanceToRoadblock = Player.Position.DistanceTo2D(CenterPosition);
                while (!IsDisposed && DistanceToRoadblock >= 125f)
                {
                    DistanceToRoadblock = Player.Position.DistanceTo2D(CenterPosition);
                    foreach (Rage.Object obj in CreatedProps)
                    {
                        if (obj.Exists())
                        {
                            NativeFunction.Natives.PLACE_OBJECT_ON_GROUND_PROPERLY(obj);
                        }
                        GameFiber.Yield();
                    }
                    GameFiber.Sleep(500);
                }
                if(!HasSpawnedAnySpikeStrips)
                {
                    EntryPoint.WriteToConsole("NO SPIKE STRIPS SPAWNED FOR ROADBLOCK ENDING CHECKING OF COLLISION");
                    return;
                }
                while (!IsDisposed)
                {
                    if (Player.IsInVehicle && Player.CurrentVehicle != null)
                    {
                        if (Player.CurrentVehicle.Vehicle.Exists())
                        {
                            DistanceToRoadblock = Player.Position.DistanceTo2D(CenterPosition);
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
                    GameFiber.Sleep(50);//GameFiber.Yield();
                }
            }
            catch (Exception e)
            {
                EntryPoint.WriteToConsole("Roadblock Spawn Error: " + e.Message + " : " + e.StackTrace, 0);
            }
        }, "SpikeStripTireChecker2");
    }
    private void SetSpeedZone()
    {
        roadblockSpeedZoneID1 = NativeFunction.Natives.ADD_ROAD_NODE_SPEED_ZONE<int>(CenterPosition.X, CenterPosition.Y, CenterPosition.Z, 15f, 0f, false);
        roadblockSpeedZoneID2 = NativeFunction.Natives.ADD_ROAD_NODE_SPEED_ZONE<int>(NodeOffset.X, NodeOffset.Y, NodeOffset.Z, 15f, 0f, false);
    }
    private void UnSetSpeedZone()
    {
        NativeFunction.Natives.REMOVE_ROAD_NODE_SPEED_ZONE(roadblockSpeedZoneID1);
        NativeFunction.Natives.REMOVE_ROAD_NODE_SPEED_ZONE(roadblockSpeedZoneID2);
    }
    private void SetRoadGenerators()
    {
        if (Settings.SettingsManager.RoadblockSettings.DisableVehicleGenerationAroundRoadblock)
        {
            float extendedDistance = Settings.SettingsManager.RoadblockSettings.DisableVehicleGenerationAroundRoadblockDistance;
            NativeFunction.Natives.SET_ALL_VEHICLE_GENERATORS_ACTIVE_IN_AREA(CenterPosition.X - extendedDistance, CenterPosition.Y - extendedDistance, CenterPosition.Z - extendedDistance, CenterPosition.X + extendedDistance, CenterPosition.Y + extendedDistance, CenterPosition.Z + extendedDistance, false, false);
            NativeFunction.Natives.SET_ALL_VEHICLE_GENERATORS_ACTIVE_IN_AREA(NodeOffset.X - extendedDistance, NodeOffset.Y - extendedDistance, NodeOffset.Z - extendedDistance, NodeOffset.X + extendedDistance, NodeOffset.Y + extendedDistance, NodeOffset.Z + extendedDistance, false, false);
        }
        if (Settings.SettingsManager.RoadblockSettings.RemoveGeneratedVehiclesAroundRoadblock)
        {
            float extendedDistance = Settings.SettingsManager.RoadblockSettings.RemoveGeneratedVehiclesAroundRoadblockDistance;
            NativeFunction.Natives.REMOVE_VEHICLES_FROM_GENERATORS_IN_AREA(CenterPosition.X - extendedDistance, CenterPosition.Y - extendedDistance, CenterPosition.Z - extendedDistance, CenterPosition.X + extendedDistance, CenterPosition.Y + extendedDistance, CenterPosition.Z + extendedDistance, false);
            NativeFunction.Natives.REMOVE_VEHICLES_FROM_GENERATORS_IN_AREA(NodeOffset.X - extendedDistance, NodeOffset.Y - extendedDistance, NodeOffset.Z - extendedDistance, NodeOffset.X + extendedDistance, NodeOffset.Y + extendedDistance, NodeOffset.Z + extendedDistance, false);        
        }
    }
    private void UnSetRoadGenerators()
    {
        float extendedDistance = 15f;
        if (Settings.SettingsManager.RoadblockSettings.DisableVehicleGenerationAroundRoadblock)
        {
            NativeFunction.Natives.SET_ALL_VEHICLE_GENERATORS_ACTIVE_IN_AREA(CenterPosition.X - extendedDistance, CenterPosition.Y - extendedDistance, CenterPosition.Z - extendedDistance, CenterPosition.X + extendedDistance, CenterPosition.Y + extendedDistance, CenterPosition.Z + extendedDistance, true, false);
            NativeFunction.Natives.SET_ALL_VEHICLE_GENERATORS_ACTIVE_IN_AREA(NodeOffset.X - extendedDistance, NodeOffset.Y - extendedDistance, NodeOffset.Z - extendedDistance, NodeOffset.X + extendedDistance, NodeOffset.Y + extendedDistance, NodeOffset.Z + extendedDistance, true, false);
        }
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
    private void DeterminePositions()
    {
        Vector3 LeftSide = NativeHelper.GetOffsetPosition(NodeCenter, NodeHeading + 90f, Settings.SettingsManager.RoadblockSettings.Roadblock_PedDistance);
        Vector3 RightSide = NativeHelper.GetOffsetPosition(NodeCenter, NodeHeading - 90f, Settings.SettingsManager.RoadblockSettings.Roadblock_PedDistance);
        if (Player.Position.DistanceTo2D(RightSide) <= Player.Position.DistanceTo2D(LeftSide))
        {
            NodeOffset = RightSide;
            BarrierOffset = NativeHelper.GetOffsetPosition(NodeCenter, NodeHeading - 90f, Settings.SettingsManager.RoadblockSettings.Roadblock_BarrierDistance);
            ConeOffset = NativeHelper.GetOffsetPosition(NodeCenter, NodeHeading - 90f, Settings.SettingsManager.RoadblockSettings.Roadblock_ConeDistance);
            PedPosition = LeftSide;
            PedHeading = NodeHeading + 180f;
        }
        else
        {
            NodeOffset = LeftSide;
            BarrierOffset = NativeHelper.GetOffsetPosition(NodeCenter, NodeHeading + 90f, Settings.SettingsManager.RoadblockSettings.Roadblock_BarrierDistance);
            ConeOffset = NativeHelper.GetOffsetPosition(NodeCenter, NodeHeading + 90f, Settings.SettingsManager.RoadblockSettings.Roadblock_ConeDistance);
            PedPosition = RightSide;
            PedHeading = NodeHeading;
        }
    }
    private void FillInBlockade()
    {
        RoadNode = new RoadNode(NodeCenter, NodeHeading);
        RoadNode.MajorRoadsOnly = true;
        RoadNode.GetRodeNodeProperties();
        if(RoadNode.HasRoad)
        {
            NodeCenter = RoadNode.RoadPosition;
        }
        GameFiber.Yield();
        DetermineVehiclesToAdd();
        GameFiber.Yield();
        DeterminePositions();
        GameFiber.Yield();
        if(EnableCarBlocks)//need at least one car to spawn?
        {
            if (AddVehicles(LocationCreate.Middle, 1))
            {
                //EntryPoint.WriteToConsoleTestLong($"ROADBLOCK Road Node Properties {RoadNode.Position} {RoadNode.Heading} FW: {RoadNode.ForwardLanes} BW: {RoadNode.BackwardsLanes} WIDTH: {RoadNode.Width} POS: {RoadNode.RoadPosition}");
                //EntryPoint.WriteToConsoleTestLong($"VFront: {VehiclesToAddFront} VRear: {VehiclesToAddRear} BFront: {BarriersToAddFront} BRear: {BarriersToAddRear} ");
                GameFiber.Yield();
                if (VehiclesToAddFront > 0)
                {
                    AddVehicles(LocationCreate.Front, VehiclesToAddFront);
                }
                GameFiber.Yield();
                if (VehiclesToAddRear > 0)
                {
                    AddVehicles(LocationCreate.Back, VehiclesToAddRear);
                }
                GameFiber.Yield();
            }
        }
        else
        {
            SpawnSideVehicle();
        }
        if (EnableBlockadeProps)
        {
            AddBlockadeProps(LocationCreate.Middle, 1);
            GameFiber.Yield();
            if (BarriersToAddFront > 0)
            {
                AddBlockadeProps(LocationCreate.Front, BarriersToAddFront);
            }
            GameFiber.Yield();
            if (BarriersToAddRear > 0)
            {
                AddBlockadeProps(LocationCreate.Back, BarriersToAddRear);
            }
            GameFiber.Yield();
        }
        SetSpeedZone();
        GameFiber.Yield();
        SetRoadGenerators();
        GameFiber.Yield();
    }
    private void SpawnSideVehicle()
    {
        SpawnLocation SideVehicleSpawnPosition = new SpawnLocation(VehicleNodeCenter);
        SideVehicleSpawnPosition.GetClosestStreet(false);
        SideVehicleSpawnPosition.GetClosestSideOfRoad();
        if(SideVehicleSpawnPosition.HasSideOfRoadPosition)
        {
            PedSideOfRoadPosition = SideVehicleSpawnPosition.StreetPosition;
        }
        else
        {
            PedSideOfRoadPosition = VehicleNodeCenter;
        }
        SideVehicleSpawnPosition.GetRoadBoundaryPosition();
        if (SideVehicleSpawnPosition.HasRoadBoundaryPosition)
        {
            SideVehicleSpawnPosition.StreetPosition = SideVehicleSpawnPosition.RoadBoundaryPosition;
        }
        CreateVehicle(SideVehicleSpawnPosition.FinalPosition, NodeHeading, true, true);
    }

    private void DetermineVehiclesToAdd()
    {
        VehicleNodeCenter = NodeCenter;
        if (RoadNode.HasRoad)
        {
            VehiclesToAddFront = RoadNode.TotalLanes / 2;
            VehiclesToAddRear = RoadNode.TotalLanes / 2;
            BarriersToAddFront = (RoadNode.TotalLanes / 2) + 2;
            BarriersToAddRear = (RoadNode.TotalLanes / 2) + 2;
            float Offset = (VehicleModel.Dimensions.Y / 2.0f) + 1.0f;
            if (RoadNode.TotalLanes == 6)
            {
                VehiclesToAddFront = 2;
                VehicleNodeCenter = NativeHelper.GetOffsetPosition(NodeCenter, NodeHeading, Offset);
                VehiclesToAddRear = 3;
                BarriersToAddFront = 4;
                BarriersToAddRear = 4;
            }
            else if (RoadNode.TotalLanes == 4)
            {
                VehiclesToAddFront = 1;
                VehicleNodeCenter = NativeHelper.GetOffsetPosition(NodeCenter, NodeHeading, Offset);
                VehiclesToAddRear = 2;
                BarriersToAddFront = 3;
                BarriersToAddRear = 3;
            }
            else if(RoadNode.TotalLanes == 2)
            {
                VehiclesToAddFront = 0;
                VehicleNodeCenter = NativeHelper.GetOffsetPosition(NodeCenter, NodeHeading, Offset);
                VehiclesToAddRear = 1;
                BarriersToAddFront = 2;
                BarriersToAddRear = 2;
            }
            if (VehiclesToAddFront < 0)
            {
                VehiclesToAddFront = 0;
            }
            if(VehiclesToAddRear < 0)
            {
                VehiclesToAddRear = 0;
            }
        }
        else
        {
            VehiclesToAddFront = 0;
            VehiclesToAddRear = 0;
            BarriersToAddFront = 0;
            BarriersToAddRear = 0;
        }
    }
    private bool AddVehicles(LocationCreate locationCreate, int toAdd)
    {
        int CarsAdded = 1;
        float Spacing = GetPositionBetweenVehicles(1.0f);//0.5f
        bool Created;
        do
        {
            float Offset = 0f;// (InFront ? 1.0f : -1.0f) * StripsAdded * Spacing;
            bool addPed = true;
            if (locationCreate != LocationCreate.Middle)
            {
                Offset = (locationCreate == LocationCreate.Front ? 1.0f : -1.0f) * CarsAdded * Spacing;
                addPed = false;
            }
            Vector3 SpawnPosition = NativeHelper.GetOffsetPosition(VehicleNodeCenter, NodeHeading, Offset);
            Created = CreateVehicle(SpawnPosition, RotatedNodeHeading, addPed, false);
            if (Created)
            {
                CarsAdded++;
                GameFiber.Yield();
            }
        } while (Created && CarsAdded <= toAdd);//5
        return Created;
    }
    private bool CreateVehicle(Vector3 position, float heading, bool addPed, bool useAltPedPos)
    {
        SpawnLocation pos1 = new SpawnLocation(position);
        pos1.StreetPosition = position;
        pos1.Heading = heading;
        LESpawnTask spawnTask = new LESpawnTask(Agency, pos1, Vehicle, null, false, Settings, Weapons, Names, false, World, ModItems, false);//had .Copy() on the vehicle and ped for some reason?
        spawnTask.AllowAnySpawn = AllowAnySpawn;
        spawnTask.AttemptSpawn();
        foreach (VehicleExt roadblockCar in spawnTask.CreatedVehicles)
        {
            roadblockCar.WasSpawnedEmpty = true;
            roadblockCar.IsManualCleanup = true;
        }
        if (addPed)
        {
            SpawnLocation pos2 = new SpawnLocation(PedPosition);
            pos2.StreetPosition = PedPosition;

            if(useAltPedPos)
            {
                pos2.InitialPosition = PedSideOfRoadPosition;
                pos2.StreetPosition = PedSideOfRoadPosition;
                pos2.Heading = heading;
            }

            LESpawnTask pedSpawn = new LESpawnTask(Agency, pos2, null, Person, Settings.SettingsManager.PoliceSpawnSettings.ShowSpawnedBlips, Settings, Weapons, Names, false, World, ModItems, false);
            pedSpawn.AllowAnySpawn = AllowAnySpawn;
            pedSpawn.SpawnWithAllWeapons = true;
            pedSpawn.AttemptSpawn();
            foreach (PedExt person in pedSpawn.CreatedPeople)
            {
                World.Pedestrians.AddEntity(person);
                CreatedRoadblockPeds.Add(person);
            }
            foreach (Cop cop in pedSpawn.SpawnedCops)//turned OFF for now!
            {
                cop.IsRoadblockSpawned = true;
            }
        }
        spawnTask.CreatedPeople.ForEach(x => World.Pedestrians.AddEntity(x));
        foreach (VehicleExt created in spawnTask.CreatedVehicles)
        {
            if (created.Vehicle.Exists())
            {
                if (!created.Vehicle.IsOnAllWheels)
                {
                    created.Vehicle.Delete();
                    EntryPoint.PersistentVehiclesDeleted++;
                }
            }
            if (created.Vehicle.Exists())
            {
                created.AddVehicleToList(World);
                //World.Vehicles.AddEntity(created, ResponseType.LawEnforcement);
                CreatedRoadblockVehicles.Add(created.Vehicle);
                created.Vehicle.IsCollisionEnabled = true;
                created.Vehicle.IsGravityDisabled = false;
                if (Settings.SettingsManager.WorldSettings.AllowSettingSirenState)
                {
                    created.Vehicle.IsSirenOn = true;
                    created.Vehicle.IsSirenSilent = true;
                }
            }
        }
        return spawnTask.CreatedVehicles.Any();
    }
    private void AddBlockadeProps(LocationCreate locationCreate, int toAdd)
    {
        int IncrementsAdded = 1;
        float Spacing = GetPositionBetweenSpikeStrips(0.25f);
        bool Created = false;
        do
        {
            float Offset = 0f;// (InFront ? 1.0f : -1.0f) * StripsAdded * Spacing;
            if (locationCreate != LocationCreate.Middle)
            {
                Offset = (locationCreate == LocationCreate.Front ? 1.0f : -1.0f) * IncrementsAdded * Spacing;
            }
            Vector3 SpawnPosition = NativeHelper.GetOffsetPosition(NodeOffset, NodeHeading, Offset);
            if (EnableSpikeStrips)
            {
                Created = CreateSpikeStrip(SpawnPosition, RotatedNodeHeading);
                if (Created)
                {
                    HasSpawnedAnySpikeStrips = true;
                    GameFiber.Yield();
                }
            }
            if (EnableOtherBarriers)
            {
                bool createdCone = CreateCone(NativeHelper.GetOffsetPosition(ConeOffset, NodeHeading, Offset), NodeHeading);
                bool createdBarrier = CreateBarrier(NativeHelper.GetOffsetPosition(BarrierOffset, NodeHeading, Offset), NodeHeading);
                Created = createdCone || createdBarrier;
                if (Created)
                {
                    HasSpawnedAnyBarrierProps = true;
                    GameFiber.Yield();
                }
            }
            IncrementsAdded++;
        } while (Created && IncrementsAdded <= toAdd);
    }
    private bool CreateSpikeStrip(Vector3 position, float heading)
    {    
        position = new Vector3(position.X, position.Y, position.Z + 1.0f);
        if (NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD<bool>(position.X, position.Y, position.Z, out float GroundZ, true, false))
        {
            position = new Vector3(position.X, position.Y, GroundZ);
        }
        Rage.Object SpikeStrip = new Rage.Object("p_ld_stinger_s", position, heading);
        if (SpikeStrip.Exists())
        {
            NativeFunction.Natives.PLACE_OBJECT_ON_GROUND_PROPERLY(SpikeStrip);
            SpikeStrip.IsPersistent = true;
            SpikeStrip.IsGravityDisabled = false;
            SpikeStrip.IsPositionFrozen = true;
            NativeFunction.Natives.PLAY_ENTITY_ANIM(SpikeStrip,"p_ld_stinger_s","p_stinger_s_deploy",1000,false,true,false,0.0f,0);
            CreatedProps.Add(SpikeStrip);
        }
        return SpikeStrip.Exists();
    }
    private bool CreateBarrier(Vector3 position, float heading)
    {
        position = new Vector3(position.X, position.Y, position.Z + 1.0f);
        if (NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD<bool>(position.X, position.Y, position.Z, out float GroundZ, true, false))
        {
            position = new Vector3(position.X, position.Y, GroundZ);
        }
        Rage.Object Barrier = new Rage.Object("prop_barrier_work05", position, heading);
        if (Barrier.Exists())
        {
            NativeFunction.Natives.PLACE_OBJECT_ON_GROUND_PROPERLY(Barrier);
            Barrier.IsPersistent = true;
            CreatedProps.Add(Barrier);
        }
        return Barrier.Exists();
    }
    private bool CreateCone(Vector3 position, float heading)
    {
        position = new Vector3(position.X, position.Y, position.Z + 1.0f);
        if (NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD<bool>(position.X, position.Y, position.Z, out float GroundZ, true, false))
        {
            position = new Vector3(position.X, position.Y, GroundZ);
        }
        Rage.Object Cone = new Rage.Object(ConeTypes.PickRandom(), position, heading);

        if (Cone.Exists())
        {
            NativeFunction.Natives.PLACE_OBJECT_ON_GROUND_PROPERLY(Cone);
            Cone.IsPersistent = true;
            CreatedProps.Add(Cone);
        }
        return Cone.Exists();
    }
    private bool GetPosition()
    {
        return NativeFunction.Natives.GET_CLOSEST_VEHICLE_NODE_WITH_HEADING<bool>(InitialPosition.X, InitialPosition.Y, InitialPosition.Z, out NodeCenter, out NodeHeading, 0, 3.0f, 0);
    }
    private float GetPositionBetweenSpikeStrips(float additionalSpacing)
    {
        return SpikeStripModel.Dimensions.Y + additionalSpacing;
    }
    private float GetPositionBetweenVehicles(float additionalSpacing)
    {
        return VehicleModel.Dimensions.Y + additionalSpacing;
    }
    private void RemoveItems()
    {
        foreach (Vehicle veh in CreatedRoadblockVehicles)
        {
            if (veh.Exists())
            {
                if (veh.DistanceTo2D(Player.Character) >= 150f)// 250f)
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
                if (pedext.DistanceToPlayer >= 150f)//250f)
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
}