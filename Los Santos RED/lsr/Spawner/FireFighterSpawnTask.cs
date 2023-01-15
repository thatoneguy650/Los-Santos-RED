using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Linq;

public class FireFighterSpawnTask : SpawnTask
{
    private bool AddBlip;
    private bool AddOptionalPassengers = false;
    private Agency Agency;
    private VehicleExt LastCreatedVehicle;
    private INameProvideable Names;
    private int OccupantsToAdd;
    private ISettingsProvideable Settings;
    private Vehicle SpawnedVehicle;
    private IWeapons Weapons;
    private IEntityProvideable World;
    public FireFighterSpawnTask(Agency agency, SpawnLocation spawnLocation, DispatchableVehicle vehicleType, DispatchablePerson personType, bool addBlip, ISettingsProvideable settings, IWeapons weapons, INameProvideable names, bool addOptionalPassengers, IEntityProvideable world) : base(spawnLocation, vehicleType, personType)
    {
        Agency = agency;
        AddBlip = addBlip;
        Settings = settings;
        Weapons = weapons;
        Names = names;
        AddOptionalPassengers = addOptionalPassengers;
        World = world;
    }
    public SpawnRequirement SpawnRequirement { get; set; }
    private bool HasAgency => Agency != null;
    private bool HasPersonToSpawn => PersonType != null;
    private bool HasVehicleToSpawn => VehicleType != null;
    private bool IsInvalidSpawnPosition => !AllowAnySpawn && Position.DistanceTo2D(Game.LocalPlayer.Character) <= 100f && Extensions.PointIsInFrontOfPed(Game.LocalPlayer.Character, Position);
    private bool LastCreatedVehicleExists => LastCreatedVehicle != null && LastCreatedVehicle.Vehicle.Exists();
    private bool WillAddPassengers => (VehicleType != null && VehicleType.MinOccupants > 1) || AddOptionalPassengers;
    public override void AttemptSpawn()
    {
        try
        {
            if (IsInvalidSpawnPosition)
            {
                EntryPoint.WriteToConsole($"FireFighterSpawn: Task Invalid Spawn Position");
                return;
            }
            if (!HasAgency)
            {
                EntryPoint.WriteToConsole($"FireFighterSpawn: Task No Agency Supplied");
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
            EntryPoint.WriteToConsole($"FireFighterSpawn: ERROR {ex.Message} : {ex.StackTrace}", 0);
            Cleanup(true);
        }
    }
    private void AddPassengers()
    {
        //EntryPoint.WriteToConsole($"SPAWN TASK: OccupantsToAdd {OccupantsToAdd}");
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
            if (PersonType != null)
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
            EntryPoint.WriteToConsole($"FireFighterSpawn: ERROR DELETED VEHICLE", 0);
        }
        if (includePeople)
        {
            foreach (PedExt person in CreatedPeople)
            {
                if (person != null && person.Pedestrian.Exists())
                {
                    person.Pedestrian.Delete();
                    EntryPoint.WriteToConsole($"FireFighterSpawn: ERROR DELETED PED", 0);
                }
            }
        }
    }
    private PedExt CreatePerson()
    {
        try
        {
            Ped createdPed;
            if (PlacePedOnGround)
            {
                createdPed = new Ped(PersonType.ModelName, new Vector3(Position.X, Position.Y, Position.Z), SpawnLocation.Heading);
            }
            else
            {
                createdPed = new Ped(PersonType.ModelName, new Vector3(Position.X, Position.Y, Position.Z + 1f), SpawnLocation.Heading);
            }
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
            EntryPoint.WriteToConsole($"FireFighterSpawn: ERROR DELETED PERSON {ex.Message} {ex.StackTrace}", 0);
            return null;
        }
    }
    private VehicleExt CreateVehicle()
    {
        try
        {
            EntryPoint.WriteToConsole($"FireFighterSpawn: Attempting to spawn {VehicleType.ModelName}", 3);
            SpawnedVehicle = new Vehicle(VehicleType.ModelName, Position, SpawnLocation.Heading);
            EntryPoint.SpawnedEntities.Add(SpawnedVehicle);
            GameFiber.Yield();
            if (SpawnedVehicle.Exists())
            {
                VehicleExt CreatedVehicle = World.Vehicles.GetVehicleExt(SpawnedVehicle);
                if (CreatedVehicle == null)
                {
                    CreatedVehicle = new VehicleExt(SpawnedVehicle, Settings);
                    CreatedVehicle.Setup();
                }
                CreatedVehicle.WasModSpawned = true;
                CreatedVehicle.IsFire = true;
                if (Agency != null)
                {
                    World.Vehicles.AddEntity(CreatedVehicle, Agency.ResponseType);
                }
                if (SpawnedVehicle.Exists())
                {
                    CreatedVehicle.WasModSpawned = true;
                    SpawnedVehicle.IsPersistent = true;
                    EntryPoint.PersistentVehiclesCreated++;

                    if (Agency != null)
                    {
                        CreatedVehicle.UpdateLivery(Agency);
                        CreatedVehicle.UpgradePerformance();
                    }
                    CreatedVehicles.Add(CreatedVehicle);
                    if (SpawnedVehicle.Exists() && VehicleType.RequiredPrimaryColorID != -1)
                    {
                        NativeFunction.Natives.SET_VEHICLE_COLOURS(SpawnedVehicle, VehicleType.RequiredPrimaryColorID, VehicleType.RequiredSecondaryColorID == -1 ? VehicleType.RequiredPrimaryColorID : VehicleType.RequiredSecondaryColorID);
                    }
                    if (VehicleType.VehicleExtras != null)
                    {
                        foreach (DispatchableVehicleExtra extra in VehicleType.VehicleExtras)
                        {
                            if (NativeFunction.Natives.DOES_EXTRA_EXIST<bool>(SpawnedVehicle, extra))
                            {
                                NativeFunction.Natives.SET_VEHICLE_EXTRA(SpawnedVehicle, extra, 0);
                            }
                        }
                    }
                    NativeFunction.Natives.SET_VEHICLE_DIRT_LEVEL(SpawnedVehicle, RandomItems.GetRandomNumberInt(0, 15));
                    EntryPoint.WriteToConsole($"FireFighterSpawn: SPAWNED {VehicleType.ModelName}", 3);
                    GameFiber.Yield();
                    return CreatedVehicle;
                }
            }
            return null;
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"FireFighterSpawn: ERROR DELETED VEHICLE {ex.Message} {ex.StackTrace} ATTEMPTING {VehicleType.ModelName}", 0);
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
    }
    private PedExt SetupAgencyPed(Ped ped)
    {
        ped.IsPersistent = true;
        EntryPoint.PersistentPedsCreated++;//TR

        RelationshipGroup rg = new RelationshipGroup("FIREMAN");
        ped.RelationshipGroup = rg;
        bool isMale;
        if (PersonType.IsFreeMode && PersonType.ModelName.ToLower() == "mp_f_freemode_01")
        {
            isMale = false;
        }
        else
        {
            isMale = ped.IsMale;
        }
        Firefighter PrimaryFirefighter = new Firefighter(ped, Settings, ped.Health, Agency, true, null, Weapons, Names.GetRandomName(isMale), World);
        World.Pedestrians.AddEntity(PrimaryFirefighter);
        if (PrimaryFirefighter != null && PersonType.OverrideVoice != null && PersonType.OverrideVoice.Any())
        {
            PrimaryFirefighter.VoiceName = PersonType.OverrideVoice.PickRandom();
        }
        if (AddBlip && ped.Exists())
        {
            Blip myBlip = ped.AttachBlip();
            NativeFunction.Natives.BEGIN_TEXT_COMMAND_SET_BLIP_NAME("STRING");
            NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(PrimaryFirefighter.GroupName);
            NativeFunction.Natives.END_TEXT_COMMAND_SET_BLIP_NAME(myBlip);
            myBlip.Color = Agency.Color;
            myBlip.Scale = 0.6f;
        }
        return PrimaryFirefighter;
    }
    private void SetupPed(Ped ped)
    {
        if (PlacePedOnGround)
        {
            float resultArg = ped.Position.Z;
            NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD(ped.Position.X, ped.Position.Y, ped.Position.Z, out resultArg, false);
            ped.Position = new Vector3(ped.Position.X, ped.Position.Y, resultArg);
        }
        int DesiredHealth = RandomItems.MyRand.Next(PersonType.HealthMin, PersonType.HealthMax) + 100;
        int DesiredArmor = RandomItems.MyRand.Next(PersonType.ArmorMin, PersonType.ArmorMax);
        ped.MaxHealth = DesiredHealth;
        ped.Health = DesiredHealth;
        ped.Armor = DesiredArmor;
    }
}