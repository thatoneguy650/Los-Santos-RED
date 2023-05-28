using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Linq;

public class GangSpawnTask : SpawnTask
{
    private Gang Gang;
    private IPedGroups RelationshipGroups;
    private IShopMenus ShopMenus;
    private Vehicle SpawnedVehicle;
    private ICrimes Crimes;
    private bool ForceMelee;
    private bool ForceSidearm;
    private bool ForceLongGun;
    public GangSpawnTask(Gang gang, SpawnLocation spawnLocation, DispatchableVehicle vehicleType, DispatchablePerson personType, bool addBlip, ISettingsProvideable settings, IWeapons weapons, INameProvideable names, 
        bool addOptionalPassengers, ICrimes crimes, IPedGroups pedGroups, IShopMenus shopMenus, IEntityProvideable world, IModItems modItems, bool forceMelee, bool forceSidearm, bool forceLongGun) 
        : base(spawnLocation, vehicleType, personType, addBlip, addOptionalPassengers, settings, weapons, names, world, modItems)
    {
        Gang = gang;
        RelationshipGroups = pedGroups;
        ShopMenus = shopMenus;
        Crimes = crimes;
        ForceMelee = forceMelee;
        ForceSidearm = forceSidearm;
        ForceLongGun = forceLongGun;
    }
    private bool HasGang => Gang != null;
    public bool IsHitSquad { get; set; } = false;
    public override void AttemptSpawn()
    {
        try
        {
            GameFiber.Yield();
            if (IsInvalidSpawnPosition)
            {
                //EntryPoint.WriteToConsoleTestLong($"GangSpawn: Task Invalid Spawn Position");
                return;
            }
            if (!HasGang)
            {
                //EntryPoint.WriteToConsoleTestLong($"GangSpawn: Task No GANG Supplied");
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
            EntryPoint.WriteToConsole($"GangSpawn: ERROR {ex.Message} : {ex.StackTrace}", 0);
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
            if (Gang != null)
            {
                PersonType = Gang.GetRandomPed(World.TotalWantedLevel, requiredGroup);
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
        if (HasGang && AllowBuddySpawn)
        {
            int BuddiesToSpawn = RandomItems.MyRand.Next(1, 2 + 1) - 1;
            for (int BuddyIndex = 1; BuddyIndex <= BuddiesToSpawn; BuddyIndex++)
            {
                PersonType = Gang.GetRandomPed(World.TotalWantedLevel, "");
                if (PersonType != null)
                {
                    PedExt Buddy = CreatePerson();
                    //EntryPoint.WriteToConsole($"SpawnTask: Adding Buddy To Gang Spawn", 5);
                }
            }
        }
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
            EntryPoint.WriteToConsole($"GangSpawn: ERROR DELETED VEHICLE", 0);
        }
        if (includePeople)
        {
            foreach (PedExt person in CreatedPeople)
            {
                if (person != null && person.Pedestrian.Exists())
                {
                    person.Pedestrian.Delete();
                    EntryPoint.WriteToConsole($"GangSpawn: ERROR DELETED PED", 0);
                }
            }
        }
    }
    private PedExt CreatePerson()
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
                //EntryPoint.WriteToConsoleTestLong("GangSpawn Task CREATED PED!");
                SetupPed(createdPed);
                if (!createdPed.Exists())
                {
                    return null;
                }
                PedExt Person = SetupGangPed(createdPed);
                PersonType.SetPedVariation(createdPed, Gang.PossibleHeads, true);
                GameFiber.Yield();
                CreatedPeople.Add(Person);
                return Person;
            }
            return null;
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"GangSpawn: ERROR DELETED PERSON {ex.Message} {ex.StackTrace}", 0);
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
            CreatedVehicle.IsGang = true;
            CreatedVehicle.WasModSpawned = true;
            if (Gang != null)
            {
                World.Vehicles.AddEntity(CreatedVehicle, ResponseType.None);
            }
            GameFiber.Yield();
            if (!SpawnedVehicle.Exists())
            {
                return null;
            }
            VehicleType.SetVehicleExtPermanentStats(CreatedVehicle, true);
            CreatedVehicle.AssociatedGang = Gang;
            CreatedVehicle.UpdatePlatePrefix(Gang);
            //CreatedVehicle.SimpleInventory.AddRandomItems(ModItems,8,4,true);
            CreatedVehicles.Add(CreatedVehicle);
            return CreatedVehicle;

        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"GangSpawn: ERROR DELETED VEHICLE {ex.Message} {ex.StackTrace} ATTEMPTING {VehicleType.ModelName}", 0);
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
    private PedExt SetupGangPed(Ped ped)
    {
        if(!ped.Exists())
        {
            return null;
        }
        ped.IsPersistent = true;
        EntryPoint.PersistentPedsCreated++;//TR
        RelationshipGroup rg = new RelationshipGroup(Gang.ID);
        ped.RelationshipGroup = rg;
        bool isMale = PersonType.IsMale(ped);

        GangMember GangMember;
        if (PersonType.IsAnimal)
        {
            GangMember = new GangDog(ped, Settings, Gang, true, Names.GetRandomName(isMale), Crimes, Weapons, World);
        }
        else
        {
            GangMember = new GangMember(ped, Settings, Gang, true, Names.GetRandomName(isMale), Crimes, Weapons, World);
        }
        GangMember.IsHitSquad = IsHitSquad;
        World.Pedestrians.AddEntity(GangMember);
        GangMember.SetStats(PersonType, ShopMenus, Weapons, AddBlip, ForceMelee,ForceSidearm,ForceLongGun);
        if (ped.Exists())
        {
            GangMember.SpawnPosition = ped.Position;
            GangMember.SpawnHeading = ped.Heading;
        }
        return GangMember;
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