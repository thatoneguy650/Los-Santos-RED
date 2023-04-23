using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Linq;

public class EMTSpawnTask : SpawnTask
{
    private Agency Agency;
    private Vehicle SpawnedVehicle;
    public EMTSpawnTask(Agency agency, SpawnLocation spawnLocation, DispatchableVehicle vehicleType, DispatchablePerson personType, bool addBlip, ISettingsProvideable settings, IWeapons weapons, INameProvideable names, bool addOptionalPassengers, IEntityProvideable world, IModItems modItems) 
        : base(spawnLocation, vehicleType, personType, addBlip, addOptionalPassengers, settings, weapons, names, world, modItems)
    {
        Agency = agency;
    }
    private bool HasAgency => Agency != null;
    public override void AttemptSpawn()
    {
        try
        {
            GameFiber.Yield();
            if (IsInvalidSpawnPosition)
            {
                //EntryPoint.WriteToConsoleTestLong($"EMTSpawn: Task Invalid Spawn Position");
                return;
            }
            if (!HasAgency)
            {
                //EntryPoint.WriteToConsoleTestLong($"EMTSpawn: Task No Agency Supplied");
                return;
            }
            Setup();
            if (HasVehicleToSpawn)
            {
                AttemptVehicleSpawn();
            }
            else if (HasPersonToSpawn)
            {
                AttemptPersonOnlySpawn();
            }
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"EMTSpawn: ERROR {ex.Message} : {ex.StackTrace}", 0);
            Cleanup(true);
        }
    }
    private void AddPassengers()
    {
        //EntryPoint.WriteToConsole($"SPAWN TASK: UnitCode {UnitCode} OccupantsToAdd {OccupantsToAdd}");
        for (int OccupantIndex = 1; OccupantIndex <= OccupantsToAdd; OccupantIndex++)
        {
            string requiredGroup = "";
            if (VehicleType != null)
            {
                requiredGroup = VehicleType.RequiredPedGroup;
            }
            if (Agency != null)
            {
                PersonType = Agency.GetRandomPed(World.TotalWantedLevel, requiredGroup);
            }
            if (PersonType != null && LastCreatedVehicleExists)
            {
                PedExt Passenger = CreatePerson();
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
    private void AttemptPersonOnlySpawn()
    {
        CreatePerson();
    }
    private void AttemptVehicleSpawn()
    {
        LastCreatedVehicle = CreateVehicle();
        if (LastCreatedVehicleExists)
        {
            if (HasPersonToSpawn)
            {
                PedExt Person = CreatePerson();
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
        }
    }
    private void Cleanup(bool includePeople)
    {
        if (LastCreatedVehicle != null && LastCreatedVehicle.Vehicle.Exists())
        {
            LastCreatedVehicle.Vehicle.Delete();
            EntryPoint.WriteToConsole($"EMTSpawn: ERROR DELETED VEHICLE", 0);
        }
        if (includePeople)
        {
            foreach (PedExt person in CreatedPeople)
            {
                if (person != null && person.Pedestrian.Exists())
                {
                    person.Pedestrian.Delete();
                    EntryPoint.WriteToConsole($"EMTSpawn: ERROR DELETED PED", 0);
                }
            }
        }
    }
    private PedExt CreatePerson()
    {
        try
        {
            Ped createdPed = new Ped(PersonType.ModelName, new Vector3(Position.X, Position.Y, Position.Z), SpawnLocation.Heading);
            EntryPoint.SpawnedEntities.Add(createdPed);
            GameFiber.Yield();
            if (createdPed.Exists())
            {
                SetupPed(createdPed);
                if (!createdPed.Exists())
                {
                    return null;
                }
                PedExt Person = SetupAgencyPed(createdPed);
                PersonType.SetPedVariation(createdPed, Agency.PossibleHeads, true);
                GameFiber.Yield();
                CreatedPeople.Add(Person);
                return Person;
            }
            return null;
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"EMTSpawn: ERROR DELETED PERSON {ex.Message} {ex.StackTrace}", 0);
            return null;
        }
    }
    private VehicleExt CreateVehicle()
    {
        try
        {
            if (ClearArea)
            {
                NativeFunction.Natives.CLEAR_AREA(Position.X, Position.Y, Position.Z, 3f, true, false, false, false);
            }
            SpawnedVehicle = new Vehicle(VehicleType.ModelName, Position, SpawnLocation.Heading);
            EntryPoint.SpawnedEntities.Add(SpawnedVehicle);
            GameFiber.Yield();
            if (!SpawnedVehicle.Exists())
            {
                return null;
            }
            VehicleExt CreatedVehicle = World.Vehicles.GetVehicleExt(SpawnedVehicle);
            if (CreatedVehicle == null)
            {
                CreatedVehicle = new VehicleExt(SpawnedVehicle, Settings);
                CreatedVehicle.Setup();
            }
            CreatedVehicle.WasModSpawned = true;
            CreatedVehicle.IsEMT = true;
            if (Agency != null)
            {
                World.Vehicles.AddEntity(CreatedVehicle, Agency.ResponseType);
            }
            GameFiber.Yield();
            if (!SpawnedVehicle.Exists())
            {
                return null;
            }
            VehicleType.SetVehicleExtPermanentStats(CreatedVehicle, true);
            CreatedVehicle.UpdatePlatePrefix(Agency);
            CreatedVehicle.CanRandomlyHaveIllegalItems = false;
            //CreatedVehicle.SimpleInventory.AddRandomItems(ModItems, 6,2, false);
            CreatedVehicles.Add(CreatedVehicle);
            return CreatedVehicle;
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"EMTSpawn: ERROR DELETED VEHICLE {ex.Message} {ex.StackTrace} ATTEMPTING {VehicleType.ModelName}", 0);
            if (SpawnedVehicle.Exists())
            {
                SpawnedVehicle.Delete();
            }
            GameFiber.Yield();
            return null;
        }
    }
    private void PutPedInVehicle(PedExt Person, int seat)
    {
        Person.Pedestrian.WarpIntoVehicle(LastCreatedVehicle.Vehicle, seat);
        Person.AssignedVehicle = LastCreatedVehicle;
        Person.AssignedSeat = seat;
        Person.UpdateVehicleState();
    }
    private void Setup()
    {
        if (VehicleType != null)
        {
            OccupantsToAdd = RandomItems.MyRand.Next(VehicleType.MinOccupants, VehicleType.MaxOccupants + 1) - 1;
        }
        else
        {
            OccupantsToAdd = 0;
        }
        //SetupCallSigns();
    }
    private PedExt SetupAgencyPed(Ped ped)
    {
        if (!ped.Exists())
        {
            return null;
        }
        ped.IsPersistent = true;
        EntryPoint.PersistentPedsCreated++;//TR
        ped.RelationshipGroup = new RelationshipGroup("MEDIC");
        bool isMale = PersonType.IsMale(ped);
        EMT PrimaryEmt = new EMT(ped, Settings, ped.Health, Agency, true, null, null, Names.GetRandomName(isMale), World);
        World.Pedestrians.AddEntity(PrimaryEmt);
        PrimaryEmt.SetStats(PersonType, Weapons, AddBlip);
        if (ped.Exists())
        {
            PrimaryEmt.SpawnPosition = ped.Position;
            PrimaryEmt.SpawnHeading = ped.Heading;
        }
        return PrimaryEmt;
    }
    private void SetupPed(Ped ped)
    {
        PlacePed(ped);
        int DesiredHealth = RandomItems.MyRand.Next(PersonType.HealthMin, PersonType.HealthMax) + 100;
        int DesiredArmor = RandomItems.MyRand.Next(PersonType.ArmorMin, PersonType.ArmorMax);
        ped.MaxHealth = DesiredHealth;
        ped.Health = DesiredHealth;
        ped.Armor = DesiredArmor;
    }
}