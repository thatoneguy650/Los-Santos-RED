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
    private Agency Agency;
    private int NextBeatNumber;
    private Vehicle SpawnedVehicle;
    private string UnitCode;
    private bool AddCanine;
    public LESpawnTask(Agency agency, SpawnLocation spawnLocation, DispatchableVehicle vehicleType, DispatchablePerson personType, bool addBlip, ISettingsProvideable settings, IWeapons weapons, INameProvideable names, bool addOptionalPassengers,
        IEntityProvideable world, IModItems modItems, bool addCanine) : base(spawnLocation, vehicleType, personType, addBlip, addOptionalPassengers, settings, weapons, names, world, modItems)
    {
        Agency = agency;
        AddCanine = addCanine;
    }


    private bool HasAgency => Agency != null;
    public override void AttemptSpawn()
    {
        try
        {
            GameFiber.Yield();
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
    private void AddCaninePassengers()
    {
        GameFiber.Yield();
        if (VehicleType == null || VehicleType.CaninePossibleSeats == null || Agency == null)
        {
            return;
        }
        EntryPoint.WriteToConsole($"SPAWN TASK: Add Canine Passengers {VehicleType.ModelName} START UnitCode {UnitCode}");
        foreach(int seatIndex in VehicleType.CaninePossibleSeats)
        {
            if(!LastCreatedVehicleExists || !LastCreatedVehicle.Vehicle.IsSeatFree(seatIndex))
            {
                continue;
            }
            PersonType = Agency.GetRandomPed(World.TotalWantedLevel, VehicleType.RequiredPedGroup, true);
            if(PersonType == null || !PersonType.IsAnimal)
            {
                continue;
            }
            PedExt Passenger = CreateCanine();
            if (Passenger != null && Passenger.Pedestrian.Exists() && LastCreatedVehicleExists && LastCreatedVehicle.Vehicle.IsSeatFree(seatIndex))
            {
                PutPedInVehicle(Passenger, seatIndex);
                EntryPoint.WriteToConsole($"SPAWN TASK: Add Canine {VehicleType.ModelName} ADDED ONE TO VEHICLE {seatIndex}");
            }
            else
            {
                Cleanup(false);
            }
            GameFiber.Yield();
        }
    }
    private void AddPassengers()
    {
        GameFiber.Yield();
        EntryPoint.WriteToConsole($"SPAWN TASK: Add Passengers {VehicleType.ModelName} START UnitCode {UnitCode} OccupantsToAdd {OccupantsToAdd}");
        for (int OccupantIndex = 1; OccupantIndex <= OccupantsToAdd; OccupantIndex++)
        {
            string requiredGroup = "";
            if (VehicleType != null)
            {
                requiredGroup = VehicleType.RequiredPedGroup;
            }
            if (Agency != null)
            {
                PersonType = Agency.GetRandomPed(World.TotalWantedLevel, requiredGroup, false);
            }
            if (PersonType != null && LastCreatedVehicleExists)
            {
                EntryPoint.WriteToConsole($"Adding Passenger IsAnimal: {PersonType.IsAnimal} {PersonType.ModelName} Seat {OccupantIndex - 1}");
                PedExt Passenger = CreatePerson();
                if (Passenger != null && Passenger.Pedestrian.Exists() && LastCreatedVehicleExists)
                {
                    PutPedInVehicle(Passenger, OccupantIndex - 1);
                    EntryPoint.WriteToConsole($"SPAWN TASK: Add Passengers {VehicleType.ModelName} ADDED ONE TO VEHICLE");
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
                    //EntryPoint.WriteToConsole($"SpawnTask: Adding Buddy To LE Spawn", 5);
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
                    if (AddCanine && VehicleType != null && VehicleType.CaninePossibleSeats.Any())
                    {
                        AddCaninePassengers();
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
            Vector3 CreatePos = Position;
            if(!PlacePedOnGround)
            {
                CreatePos.Z += 1.0f;
                EntryPoint.WriteToConsole("ADDED HIEGHT TO SPAWN");
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
               // GameFiber.Yield();
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
    private PedExt CreateCanine()
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
                PedExt Person = SetupAgencyAnimal(createdPed);
                PersonType.SetPedVariation(createdPed, null, true);
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
            CreatedVehicle.UpgradePerformance();
            CreatedVehicle.UpdatePlatePrefix(Agency);
            CreatedVehicle.CanRandomlyHaveIllegalItems = false;
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
            //EntryPoint.WriteToConsoleTestLong($"COP {Person.Handle} SET STAY IN VEHICLE Vehicle:{VehicleType.ModelName} seat:{seat}");
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
    private PedExt SetupAgencyPed(Ped Pedestrian)
    {
        if (!Pedestrian.Exists())
        {
            return null;
        }
        Pedestrian.IsPersistent = true;
        EntryPoint.PersistentPedsCreated++;//TR
        RelationshipGroup rg = new RelationshipGroup("COP");
        Pedestrian.RelationshipGroup = rg;
        NativeFunction.CallByName<bool>("SET_PED_AS_COP", Pedestrian, true);
        bool isMale = PersonType.IsMale(Pedestrian);
        Cop PrimaryCop = new Cop(Pedestrian, Settings, Pedestrian.Health, Agency, true, null, Weapons, Names.GetRandomName(isMale), PersonType.ModelName, World);
        World.Pedestrians.AddEntity(PrimaryCop);
        PrimaryCop.SetStats(PersonType, Weapons, AddBlip, UnitCode);//TASKING IS BROKEN FOR ALL COPS FAR FROM PLAYER AND ALL OTHER PEDS
        if (Pedestrian.Exists())
        {
            PrimaryCop.SpawnPosition = Pedestrian.Position;
            PrimaryCop.SpawnHeading = Pedestrian.Heading;
            if (SpawnWithAllWeapons || PersonType.AlwaysHasLongGun)
            {
                PrimaryCop.WeaponInventory.GiveHeavyWeapon();
            }
        }
        return PrimaryCop;
    }
    private PedExt SetupAgencyAnimal(Ped ped)
    {
        ped.IsPersistent = true;
        EntryPoint.PersistentPedsCreated++;//TR

        RelationshipGroup rg = new RelationshipGroup("COP");
        ped.RelationshipGroup = rg;
        //NativeFunction.CallByName<bool>("SET_PED_AS_COP", ped, true);
        bool isMale = true;
        CanineUnit PrimaryCop = new CanineUnit(ped, Settings, ped.Health, Agency, true, null, Weapons, Names.GetRandomDogName(isMale), PersonType.ModelName, World);
        World.Pedestrians.AddEntity(PrimaryCop);
        PrimaryCop.SetStats(PersonType, Weapons, AddBlip, UnitCode);
        //PrimaryCop.TaskRequirements = SpawnRequirement;
        if (ped.Exists())
        {
            PrimaryCop.SpawnPosition = ped.Position;
            PrimaryCop.SpawnHeading = ped.Heading;
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
        PlacePed(ped);
        int DesiredHealth = RandomItems.MyRand.Next(PersonType.HealthMin, PersonType.HealthMax) + 100;
        int DesiredArmor = RandomItems.MyRand.Next(PersonType.ArmorMin, PersonType.ArmorMax);
        ped.MaxHealth = DesiredHealth;
        ped.Health = DesiredHealth;
        ped.Armor = DesiredArmor;
    }




}