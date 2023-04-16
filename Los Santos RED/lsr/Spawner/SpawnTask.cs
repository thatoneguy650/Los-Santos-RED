using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosSantosRED.lsr.Interface;
using System.Drawing;
using ExtensionsMethods;
using System.Windows.Media;

public abstract class SpawnTask
{
    protected bool AddBlip;
    protected bool AddOptionalPassengers = false;
    protected VehicleExt LastCreatedVehicle;
    protected INameProvideable Names;
    protected int OccupantsToAdd;
    protected ISettingsProvideable Settings;
    protected IWeapons Weapons;
    protected IEntityProvideable World;
    protected IModItems ModItems;

    protected bool IsInvalidSpawnPosition => !AllowAnySpawn && Position.DistanceTo2D(Game.LocalPlayer.Character) <= 100f && Extensions.PointIsInFrontOfPed(Game.LocalPlayer.Character, Position);
    protected bool LastCreatedVehicleExists => LastCreatedVehicle != null && LastCreatedVehicle.Vehicle.Exists();
    protected bool WillAddPassengers => (VehicleType != null && VehicleType.MinOccupants > 1) || AddOptionalPassengers;
    protected bool HasPersonToSpawn => PersonType != null;
    protected bool HasVehicleToSpawn => VehicleType != null;
    public SpawnTask(SpawnLocation spawnLocation, DispatchableVehicle vehicleType, DispatchablePerson personType, bool addBlip, bool addOptionalPassengers, ISettingsProvideable settings, IWeapons weapons, INameProvideable names, IEntityProvideable world, IModItems modItems)
    {
        SpawnLocation = spawnLocation;
        PersonType = personType;
        VehicleType = vehicleType;
        AddBlip = addBlip;
        Settings = settings;
        Weapons = weapons;
        Names = names;
        AddOptionalPassengers = addOptionalPassengers;
        World = world;
        ModItems = modItems;
    }
    public List<PedExt> CreatedPeople { get; private set; } = new List<PedExt>();
    public List<VehicleExt> CreatedVehicles { get; private set; } = new List<VehicleExt>();
    public SpawnLocation SpawnLocation { get; set; }
    public DispatchableVehicle VehicleType { get; set; }
    public DispatchablePerson PersonType { get; set; }
    public bool AllowAnySpawn { get; set; } = false;
    public bool AllowBuddySpawn { get; set; } = true;
    public bool ClearArea { get; set; } = false;
    public bool PlacePedOnGround { get; set; } = false;
    public TaskRequirements SpawnRequirement { get; set; }
    public bool SpawnWithAllWeapons { get; set; } = false;
    public Vector3 Position
    {
        get
        {
            if (VehicleType == null)
            {
                if (SpawnLocation.HasSidewalk)
                {
                    return SpawnLocation.SidewalkPosition;
                }
                return SpawnLocation.InitialPosition;
            }
            else if (VehicleType.IsHelicopter)
            {
                return SpawnLocation.InitialPosition + new Vector3(0f, 0f, 250f);
            }
            else if (VehicleType.IsBoat)
            {
                return SpawnLocation.InitialPosition;
            }
            else
            {
                return SpawnLocation.StreetPosition;
            }
        }
    }
    public abstract void AttemptSpawn();
    protected virtual void PlacePed(Ped ped)
    {
        if (PlacePedOnGround)
        {
            float resultArg = ped.Position.Z;
            if (NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD<bool>(ped.Position.X, ped.Position.Y, ped.Position.Z + 1f, out resultArg, false))
            {
                ped.Position = new Vector3(ped.Position.X, ped.Position.Y, resultArg);
            }
        }
    }
}
