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
using static RAGENativeUI.Elements.UIMenuStatsPanel;

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
    public List<RandomHeadData> PossibleHeads { get; set; }
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
    public bool ClearVehicleArea { get; set; } = false;
    public bool PlacePedOnGround { get; set; } = false;
    public bool WillAddDriver { get; set; } = true;
    public bool AddEmptyVehicleBlip { get; set; } = false;
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
            else if ((VehicleType.IsHelicopter || VehicleType.IsPlane) && PersonType == null)
            {
                return SpawnLocation.InitialPosition;
            }
            else if (VehicleType.IsHelicopter || VehicleType.IsPlane)
            {
                return SpawnLocation.InitialPosition + new Vector3(0f, 0f, 250f);
            }
            else if (VehicleType.IsBoat && SpawnLocation.IsWater)
            {
                return SpawnLocation.WaterPosition;
            }
            else
            {
                return SpawnLocation.StreetPosition;
            }
        }
    }
    public abstract void AttemptSpawn();

    public virtual void SpawnAsPassenger(VehicleExt vehicleExt, int seatIndex)
    {

    }
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
    protected virtual void AttemptVehicleSpawn()
    {
        LastCreatedVehicle = CreateVehicle();
        if (!LastCreatedVehicleExists)
        {
            return;
        }
        if (HasPersonToSpawn)
        {
            if (WillAddDriver)
            {
                PedExt Person = CreatePerson(-1);
                if (Person != null && Person.Pedestrian.Exists() && LastCreatedVehicleExists)
                {
                    PutPedInVehicle(Person, -1);
                    if (WillAddPassengers)
                    {
                        AddPassengers();
                    }
                }
                else
                {
                    Cleanup(true);
                }
            }
            else
            {
                if (LastCreatedVehicleExists)
                {
                    if (WillAddPassengers)
                    {
                        AddPassengers();
                    }
                }
                else
                {
                    Cleanup(true);
                }
            }
        }
        else if(AddEmptyVehicleBlip)
        {
            LastCreatedVehicle.AddRegularBlip();
        }    
    }
    protected virtual void Cleanup(bool includePeople) 
    {
        if (LastCreatedVehicle != null && LastCreatedVehicle.Vehicle.Exists())
        {
            LastCreatedVehicle.Vehicle.Delete();
            EntryPoint.WriteToConsole($"Spawn Task: ERROR DELETED VEHICLE", 0);
        }
        else
        {
            EntryPoint.ModController.AddSpawnError(new SpawnError(Game.GetHashKey(VehicleType.ModelName), Position, Game.GameTime));
        }
        if (includePeople)
        {
            foreach (PedExt person in CreatedPeople)
            {
                if (person != null && person.Pedestrian.Exists())
                {
                    person.Pedestrian.Delete();
                    EntryPoint.WriteToConsole($"Spawn Task: ERROR DELETED PED", 0);
                }
            }
        }
    }
    protected virtual void AddPassengers()
    {
        for (int OccupantIndex = 1; OccupantIndex <= OccupantsToAdd; OccupantIndex++)
        {
            string requiredGroup = "";
            if (VehicleType != null)
            {
                requiredGroup = VehicleType.RequiredPedGroup;
            }
            GetNewPersonType(requiredGroup);
            if (PersonType != null)
            {
                PedExt Passenger = CreatePerson(OccupantIndex - 1);
                if (Passenger != null && Passenger.Pedestrian.Exists() && LastCreatedVehicleExists)
                {
                    PutPedInVehicle(Passenger, OccupantIndex - 1);
                }
                else
                {
                    Cleanup(false);
                }
            }
            GameFiber.Yield();
        }
    }
    protected virtual void AttemptPersonOnlySpawn()
    {
        CreatePerson(-1);
        if (!AllowBuddySpawn)
        {
            return;
        }
        int BuddiesToSpawn = RandomItems.MyRand.Next(1, 2 + 1) - 1;
        for (int BuddyIndex = 1; BuddyIndex <= BuddiesToSpawn; BuddyIndex++)
        {
            GetNewPersonType("");
            if (PersonType != null)
            {
                SpawnLocation.InitialPosition = Position.Around2D(1f);
                SpawnLocation.SidewalkPosition = Vector3.Zero;
                CreatePerson(-1);
            }
        }    
    }
    protected virtual PedExt CreatePerson(int seat)
    {
        return null;
    }
    protected virtual VehicleExt CreateVehicle()
    {
        return null;
    }
    protected virtual void PutPedInVehicle(PedExt Person, int Seat)
    {
       //Person.Pedestrian.WarpIntoVehicle(LastCreatedVehicle.Vehicle, Seat);
        Person.AssignedVehicle = LastCreatedVehicle;
        Person.AssignedSeat = Seat;
        if (VehicleType != null && VehicleType.ForceStayInSeats != null && VehicleType.ForceStayInSeats.Contains(Seat))
        {
            Person.StayInVehicle = true;
        }
        Person.UpdateVehicleState();
    }
    protected virtual void GetNewPersonType(string requiredGroup)
    {

    }
    public virtual void PostRun(ConditionalLocation conditionalLocation, GameLocation gameLocation)
    {
        CreatedPeople.ForEach(x => {
            World.Pedestrians.AddEntity(x); 
            x.IsLocationSpawned = true; 
            conditionalLocation?.AddLocationRequirements(x); 
            //gameLocation?.AddSpawnedPed(x); 
        });
        CreatedVehicles.ForEach(x => { 
            x.AddVehicleToList(World); 
            x.WasSpawnedEmpty = conditionalLocation?.IsEmptyVehicleSpawn == true;
            //gameLocation?.AddSpawnedVehicle(x); 
        });
    }
}
