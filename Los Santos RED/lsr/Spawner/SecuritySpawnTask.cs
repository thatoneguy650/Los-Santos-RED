using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Linq;

public class SecurityGuardSpawnTask : SpawnTask
{
    private Agency Agency;
    private Vehicle SpawnedVehicle;
    private ICrimes Crimes;
    public SecurityGuardSpawnTask(Agency agency, SpawnLocation spawnLocation, DispatchableVehicle vehicleType, DispatchablePerson personType, bool addBlip, ISettingsProvideable settings, IWeapons weapons, INameProvideable names, 
        bool addOptionalPassengers, IEntityProvideable world, ICrimes crimes, IModItems modItems) : base(spawnLocation, vehicleType, personType, addBlip, addOptionalPassengers, settings, weapons, names, world, modItems)
    {
        Agency = agency;
        Crimes = crimes;
}
    private bool HasAgency => Agency != null;
    public override void AttemptSpawn()
    {
        try
        {
            GameFiber.Yield();
            if (IsInvalidSpawnPosition)
            {
                //EntryPoint.WriteToConsoleTestLong($"SecurityGuardSpawnTask: Task Invalid Spawn Position");
                return;
            }
            if (!HasAgency)
            {
                //EntryPoint.WriteToConsoleTestLong($"SecurityGuardSpawnTask: Task No Agency Supplied");
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
            EntryPoint.WriteToConsole($"SecurityGuardSpawnTask: ERROR {ex.Message} : {ex.StackTrace}", 0);
            Cleanup(true);
        }
    }
    protected override PedExt CreatePerson()
    {
        try
        {
            Vector3 CreatePos = Position;
            if (!PlacePedOnGround || VehicleType != null)
            {
                CreatePos.Z += 1.0f;
                //EntryPoint.WriteToConsole("ADDED HIEGHT TO SPAWN");
            }
            Ped createdPed = new Ped(PersonType.ModelName, new Vector3(CreatePos.X, CreatePos.Y, CreatePos.Z), SpawnLocation.Heading);
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
            EntryPoint.WriteToConsole($"SecurityGuardSpawnTask: ERROR DELETED PERSON {ex.Message} {ex.StackTrace}", 0);
            return null;
        }
    }
    protected override VehicleExt CreateVehicle()
    {
        try
        {
            if (ClearArea)
            {
                NativeFunction.Natives.CLEAR_AREA(Position.X, Position.Y, Position.Z, 3f, true, false, false, false);
            }
            //EntryPoint.WriteToConsole($"SecurityGuardSpawnTask: Attempting to spawn {VehicleType.ModelName}", 3);
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
            //CreatedVehicle.SimpleInventory.AddRandomItems(ModItems,6,2,false);
            //CreatedVehicle.SetSpawnItems(VehicleType, Agency, null, true);
            CreatedVehicles.Add(CreatedVehicle);
            return CreatedVehicle;  
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"SecurityGuardSpawnTask: ERROR DELETED VEHICLE {ex.Message} {ex.StackTrace} ATTEMPTING {VehicleType.ModelName}", 0);
            if (SpawnedVehicle.Exists())
            {
                SpawnedVehicle.Delete();
            }
            GameFiber.Yield();
            return null;
        }
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
        RelationshipGroup rg = new RelationshipGroup("SECURITY_GUARD");
        ped.RelationshipGroup = rg;
        bool isMale = PersonType.IsMale(ped);
        SecurityGuard primarySecurityGuard;
          
        if(PersonType.IsAnimal)
        {
            primarySecurityGuard = new SecurityDog(ped, Settings, ped.Health, Agency, true, Crimes, Weapons, Names.GetRandomName(isMale), PersonType.ModelName, World);
        }
        else
        {
            primarySecurityGuard = new SecurityGuard(ped, Settings, ped.Health, Agency, true, Crimes, Weapons, Names.GetRandomName(isMale), PersonType.ModelName, World);
        }      
        World.Pedestrians.AddEntity(primarySecurityGuard);
        primarySecurityGuard.SetStats(PersonType, Weapons, AddBlip);
        if (ped.Exists())
        {
            primarySecurityGuard.SpawnPosition = ped.Position;
            primarySecurityGuard.SpawnHeading = ped.Heading;
        }
        return primarySecurityGuard;
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
    protected override void GetNewPersonType(string requiredGroup)
    {
        if (Agency != null)
        {
            PersonType = Agency.GetRandomPed(World.TotalWantedLevel, requiredGroup);
        }
    }
}