using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

public class LESpawnTask : SpawnTask
{
    private bool AddBlip;
    private bool AddOptionalPassengers = false;
    private Agency Agency;
    private VehicleExt LastCreatedVehicle;
    private INameProvideable Names;
    private int NextBeatNumber;
    private int OccupantsToAdd;
    private ISettingsProvideable Settings;
    private Vehicle SpawnedVehicle;
    private string UnitCode;
    private IWeapons Weapons;
    private IEntityProvideable World;
    public LESpawnTask(Agency agency, SpawnLocation spawnLocation, DispatchableVehicle vehicleType, DispatchablePerson personType, bool addBlip, ISettingsProvideable settings, IWeapons weapons, INameProvideable names, bool addOptionalPassengers, IEntityProvideable world) : base(spawnLocation, vehicleType, personType)
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
    public bool ClearArea { get; set; } = false;
    public bool SpawnWithAllWeapons { get; set; } = false;
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
                EntryPoint.WriteToConsole($"LESpawn: Task Invalid Spawn Position");
                return;
            }
            if (!HasAgency)
            {
                EntryPoint.WriteToConsole($"LESpawn: Task No Agency Supplied");
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
            EntryPoint.WriteToConsole($"LESpawn: ERROR {ex.Message} : {ex.StackTrace}", 0);
            Cleanup(true);
        }
    }
    private void AddPassengers()
    {
        //EntryPoint.WriteToConsole($"SPAWN TASK: Add Passengers {VehicleType.ModelName} START UnitCode {UnitCode} OccupantsToAdd {OccupantsToAdd}");
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
                    //EntryPoint.WriteToConsole($"SPAWN TASK: Add Passengers {VehicleType.ModelName} ADDED ONE TO VEHICLE");
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
        if (Agency != null && AllowBuddySpawn)
        {
            int BuddiesToSpawn = RandomItems.MyRand.Next(1, 2 + 1) - 1;
            for (int BuddyIndex = 1; BuddyIndex <= BuddiesToSpawn; BuddyIndex++)
            {
                PersonType = Agency.GetRandomPed(World.TotalWantedLevel, "");
                if (PersonType != null)
                {
                    SpawnLocation.InitialPosition = Position.Around2D(1f);
                    SpawnLocation.SidewalkPosition = Vector3.Zero;
                    PedExt Buddy = CreatePerson();
                    EntryPoint.WriteToConsole($"SpawnTask: Adding Buddy To LE Spawn", 5);
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
            EntryPoint.WriteToConsole($"LESpawn: ERROR DELETED VEHICLE", 0);
        }
        if (includePeople)
        {
            foreach (PedExt person in CreatedPeople)
            {
                if (person != null && person.Pedestrian.Exists())
                {
                    person.Pedestrian.Delete();
                    EntryPoint.WriteToConsole($"LESpawn: ERROR DELETED PED", 0);
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
                createdPed = new Ped(PersonType.ModelName, new Vector3(Position.X, Position.Y, Position.Z + 1f), SpawnLocation.Heading);//createdPed = new Ped(PersonType.ModelName, new Vector3(Position.X, Position.Y, Position.Z + 1f), SpawnLocation.Heading);
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
            EntryPoint.WriteToConsole($"LESpawn: ERROR DELETED PERSON {ex.Message} {ex.StackTrace}", 0);
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
            CreatedVehicle.IsPolice = true;
            if (Agency != null)
            {
                World.Vehicles.AddEntity(CreatedVehicle, Agency.ResponseType);
            }
            CreatedVehicle.SetSpawnItems(VehicleType, Agency, null, true);
            CreatedVehicles.Add(CreatedVehicle);           
            return CreatedVehicle;
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"LESpawn: ERROR DELETED VEHICLE {ex.Message} {ex.StackTrace} ATTEMPTING {VehicleType.ModelName}", 0);
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

        if(VehicleType != null && VehicleType.ForceStayInSeats != null && VehicleType.ForceStayInSeats.Contains(seat))
        {
            Person.StayInVehicle = true;
            EntryPoint.WriteToConsole($"COP {Person.Handle} SET STAY IN VEHICLE Vehicle:{VehicleType.ModelName} seat:{seat}");
        }

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
        SetupCallSigns();
    }
    private PedExt SetupAgencyPed(Ped ped)
    {
        ped.IsPersistent = true;
        EntryPoint.PersistentPedsCreated++;//TR

        RelationshipGroup rg = new RelationshipGroup("COP");
        ped.RelationshipGroup = rg;
        NativeFunction.CallByName<bool>("SET_PED_AS_COP", ped, true);

        bool isMale;
        if (PersonType.IsFreeMode && PersonType.ModelName.ToLower() == "mp_f_freemode_01")
        {
            isMale = false;
        }
        else
        {
            isMale = ped.IsMale;
        }
        Cop PrimaryCop = new Cop(ped, Settings, ped.Health, Agency, true, null, Weapons, Names.GetRandomName(isMale), PersonType.ModelName, World);
        World.Pedestrians.AddEntity(PrimaryCop);
        PrimaryCop.SetStats(PersonType, Weapons, AddBlip, UnitCode);
        PrimaryCop.SpawnRequirement = SpawnRequirement;
        if (ped.Exists())
        {
            PrimaryCop.SpawnPosition = ped.Position;
            PrimaryCop.SpawnHeading = ped.Heading;
        }
        if (SpawnWithAllWeapons)
        {
            PrimaryCop.WeaponInventory.GiveHeavyWeapon();
        }
        return PrimaryCop;
    }
    private void SetupCallSigns()
    {
        if (PersonType != null && PersonType.UnitCode != "")
        {
            UnitCode = PersonType.UnitCode;
            NextBeatNumber = Agency.GetNextBeatNumber();
        }
        if (Agency != null && Agency.Division != -1)
        {
            if (VehicleType?.IsMotorcycle == true)
            {
                UnitCode = "Mary";
            }
            else if (VehicleType?.IsHelicopter == true)
            {
                UnitCode = "David";
            }
            else if (WillAddPassengers && OccupantsToAdd > 0 && VehicleType != null)
            {
                UnitCode = "Adam";
            }
            else if (VehicleType == null)
            {
                UnitCode = "Frank";
            }
            else
            {
                if (RandomItems.RandomPercent(80))
                {
                    UnitCode = "Lincoln";
                }
                else
                {
                    UnitCode = new List<string>() {
                        "Henry"//H: Detective ("Henry")
                        ,"Tom"//T: Traffic investigator ("Tom")
                        , "Edward"//E: Ticket-writing unit
                        ,"George"//G: Gang enforcement unit ("George")
                        , "Robert"//R: Metro Unit
                        ,"William"//W: Detective ("William")
                        ,"Victor"//V: Vice ("Victor")
                        ,"XRay"//X: Extra patrol ("X-ray")
                        ,"Nora"//N: Narcotics
                    }.PickRandom();
                }
            }
            NextBeatNumber = Agency.GetNextBeatNumber();
        }
        else
        {
            UnitCode = "";
            NextBeatNumber = 0;
        }
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