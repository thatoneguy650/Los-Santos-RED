using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using static RAGENativeUI.Elements.UIMenuStatsPanel;

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

    public List<Cop> SpawnedCops { get; set; } = new List<Cop>();
    private bool HasAgency => Agency != null;
    public bool IsMarshalMember { get; set; } = false;
    public bool IsOffDutySpawn { get; set; } = false;
    public bool CheckPosition { get; set; } = true;

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
    public override void SpawnAsPassenger(VehicleExt vehicleExt, int seatIndex)
    {
        if(vehicleExt == null || !vehicleExt.Vehicle.Exists())
        {
            return;
        }
        Setup();
        SpawnedVehicle = vehicleExt.Vehicle;
        LastCreatedVehicle = vehicleExt;
        PassengerCreate(seatIndex);
    }
    private void AddCaninePassengers()
    {
        GameFiber.Yield();
        bool created = false;
        if (VehicleType == null || VehicleType.CaninePossibleSeats == null || Agency == null)
        {
            return;
        }
        EntryPoint.WriteToConsole($"SPAWN TASK: Add Canine Passengers {VehicleType.ModelName} START UnitCode {UnitCode}");
        foreach(int seatIndex in VehicleType.CaninePossibleSeats)
        {
            EntryPoint.WriteToConsole($"LE SPAWN TASK CANINE SEAT {seatIndex}");
            if (!LastCreatedVehicleExists || !LastCreatedVehicle.Vehicle.IsSeatFree(seatIndex))
            {
                EntryPoint.WriteToConsole("LE SPAWN TASK ATTEMPT VEHICLE SPAWN CANINE5");
                continue;
            }
            PersonType = Agency.GetRandomPed(World.TotalWantedLevel, VehicleType.RequiredPedGroup, true);
            if(PersonType == null || !PersonType.IsAnimal)
            {
                EntryPoint.WriteToConsole("LE SPAWN TASK ATTEMPT VEHICLE SPAWN CANINE6");
                continue;
            }
            EntryPoint.WriteToConsole($"LE SPAWN TASK CANINE SEAT {seatIndex} CREATE START");
            PedExt Passenger = CreateCanine(seatIndex);
            if (Passenger != null && Passenger.Pedestrian.Exists() && LastCreatedVehicleExists)// && LastCreatedVehicle.Vehicle.IsSeatFree(seatIndex))
            {
                PutPedInVehicle(Passenger, seatIndex);
                EntryPoint.WriteToConsole($"SPAWN TASK: Add Canine {VehicleType.ModelName} ADDED ONE TO VEHICLE {seatIndex}");
                created = true;
            }
            else
            {
                Cleanup(false);
            }

            if(created && !RandomItems.RandomPercent(Settings.SettingsManager.PoliceSpawnSettings.AddAdditionalK9PassengerPercentage))
            {
                GameFiber.Yield();
                break;
            }

            GameFiber.Yield();
        }
    }
    protected override void AttemptVehicleSpawn()
    {
        EntryPoint.WriteToConsole("LE SPAWN TASK ATTEMPT VEHICLE SPAWN");
        LastCreatedVehicle = CreateVehicle();
        if (LastCreatedVehicleExists)
        {
            if (HasPersonToSpawn)
            {
                if (WillAddDriver)
                {
                    EntryPoint.WriteToConsole("LE SPAWN TASK ATTEMPT VEHICLE SPAWN DRIVER1");
                    PedExt Person = CreatePerson(-1);
                    if (Person != null && Person.Pedestrian.Exists() && LastCreatedVehicleExists)
                    {
                        PutPedInVehicle(Person, -1);
                        if (WillAddPassengers)
                        {
                            AddPassengers();
                        }
                        if (AddCanine && VehicleType != null && VehicleType.CaninePossibleSeats.Any())
                        {
                            EntryPoint.WriteToConsole("LE SPAWN TASK ATTEMPT VEHICLE SPAWN CANINE1");
                            AddCaninePassengers();
                        }
                    }
                }
                else
                {
                    EntryPoint.WriteToConsole("LE SPAWN TASK ATTEMPT VEHICLE SPAWN DRIVER2" );
                    if (LastCreatedVehicleExists)
                    {
                        if (WillAddPassengers)
                        {
                            AddPassengers();
                        }
                        if (AddCanine && VehicleType != null && VehicleType.CaninePossibleSeats.Any())
                        {
                            EntryPoint.WriteToConsole("LE SPAWN TASK ATTEMPT VEHICLE SPAWN CANINE2");
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
    }
    protected override void AddPassengers()
    {
        int AlreadyRanItem = -99;
        if(VehicleType != null && VehicleType.FirstPassengerIndex > 0)
        {
            AlreadyRanItem = VehicleType.FirstPassengerIndex;
            PassengerCreate(VehicleType.FirstPassengerIndex);
        }
        for (int OccupantIndex = 1; OccupantIndex <= OccupantsToAdd; OccupantIndex++)
        {
            if(OccupantIndex - 1 == AlreadyRanItem)
            {
                continue;
            }
            PassengerCreate(OccupantIndex - 1);
        }
    }
    private void PassengerCreate(int seatIndex)
    {
        string requiredGroup = "";
        if (VehicleType != null && !VehicleType.RequiredGroupIsDriverOnly)
        {
            requiredGroup = VehicleType.RequiredPedGroup;
        }
        GetNewPersonType(requiredGroup);
        if (PersonType != null)
        {
            PedExt Passenger = CreatePerson(seatIndex);
            if (Passenger != null && Passenger.Pedestrian.Exists() && LastCreatedVehicleExists)
            {
                PutPedInVehicle(Passenger, seatIndex);
            }
            else
            {
                Cleanup(false);
            }
        }
        GameFiber.Yield();
    }
    protected override PedExt CreatePerson(int seat)
    {
        try
        {
            EntryPoint.WriteToConsole("LE SPAWN TASK ATTEMPT PERSON SPAWN");
            if (string.IsNullOrEmpty(PersonType.ModelName))
            {
                return null;
            }
            Vector3 CreatePos = Position;
            if(!PlacePedOnGround || VehicleType != null)
            {
                CreatePos.Z += 1.0f;//1.0f;
                //CreatePos = CreatePos.Around2D(10f);
                //EntryPoint.WriteToConsole("ADDED HIEGHT TO SPAWN");
            }
            World.Pedestrians.CleanupAmbient();
            Ped createdPed = null;
            if (VehicleType != null && SpawnedVehicle.Exists())
            {
                uint GameTimeStarted = Game.GameTime;
                uint hashKey = Game.GetHashKey(PersonType.ModelName);
                if (!NativeFunction.Natives.HAS_MODEL_LOADED<bool>(hashKey))
                {
                    NativeFunction.Natives.REQUEST_MODEL(hashKey);
                    while (!NativeFunction.Natives.HAS_MODEL_LOADED<bool>(hashKey) && Game.GameTime - GameTimeStarted <= 1000)
                    {
                        GameFiber.Yield();
                    }
                }
                createdPed = NativeFunction.Natives.CREATE_PED_INSIDE_VEHICLE<Ped>(SpawnedVehicle, 26, hashKey, seat, true, true);
            }
            else
            {
                createdPed = new Ped(PersonType.ModelName, new Vector3(CreatePos.X, CreatePos.Y, CreatePos.Z), SpawnLocation.Heading);
            }
            EntryPoint.SpawnedEntities.Add(createdPed);
            GameFiber.Yield();
            NativeFunction.Natives.SET_MODEL_AS_NO_LONGER_NEEDED(Game.GetHashKey(PersonType.ModelName));
            if (createdPed.Exists())
            {
                SetupPed(createdPed);
                if (!createdPed.Exists())
                {
                    return null;
                }
                PedExt Person = SetupAgencyPed(createdPed);
                CreatedPeople.Add(Person);
                GameFiber.Yield();
                PersonType.SetPedVariation(createdPed, Agency.PossibleHeads, true);
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
    private PedExt CreateCanine(int seat)
    {
        try
        {
            Vector3 CreatePos = Position;
            if (!PlacePedOnGround || VehicleType != null)
            {
                CreatePos.Z += 1.0f;//1.0f;
                //CreatePos = CreatePos.Around2D(10f);
                //EntryPoint.WriteToConsole("ADDED HIEGHT TO SPAWN");
            }

            EntryPoint.WriteToConsole($"LE SPAWN TASK CreateCanine 1");

            World.Pedestrians.CleanupAmbient();
            Ped createdPed = null;// = new Ped(PersonType.ModelName, new Vector3(Position.X, Position.Y, Position.Z), SpawnLocation.Heading);
            if (VehicleType != null && SpawnedVehicle.Exists())
            {
                uint GameTimeStarted = Game.GameTime;
                if (!NativeFunction.Natives.HAS_MODEL_LOADED<bool>(Game.GetHashKey(PersonType.ModelName)))
                {
                    NativeFunction.Natives.REQUEST_MODEL(Game.GetHashKey(PersonType.ModelName));
                    while (!NativeFunction.Natives.HAS_MODEL_LOADED<bool>(Game.GetHashKey(PersonType.ModelName)) && Game.GameTime - GameTimeStarted <= 1000)
                    {
                        GameFiber.Yield();
                    }
                }
                createdPed = NativeFunction.Natives.CREATE_PED_INSIDE_VEHICLE<Ped>(SpawnedVehicle, 26, Game.GetHashKey(PersonType.ModelName), seat, true, true);
            }
            else
            {
                createdPed = new Ped(PersonType.ModelName, new Vector3(CreatePos.X, CreatePos.Y, CreatePos.Z), SpawnLocation.Heading);
            }
            EntryPoint.WriteToConsole($"LE SPAWN TASK CreateCanine 2");
            EntryPoint.SpawnedEntities.Add(createdPed);
            GameFiber.Yield();
            NativeFunction.Natives.SET_MODEL_AS_NO_LONGER_NEEDED(Game.GetHashKey(PersonType.ModelName));
            if (createdPed.Exists())
            {
                EntryPoint.WriteToConsole($"LE SPAWN TASK CreateCanine 3");
                SetupPed(createdPed);
                if (!createdPed.Exists())
                {
                    return null;
                }
                PedExt Person = SetupAgencyAnimal(createdPed);
                EntryPoint.WriteToConsole($"LE SPAWN TASK CreateCanine 4");
                PersonType.SetPedVariation(createdPed, null, true);//no head data for canines
                GameFiber.Yield();
                CreatedPeople.Add(Person);
                EntryPoint.WriteToConsole($"LE SPAWN TASK CreateCanine 5");
                return Person;
            }
            EntryPoint.WriteToConsole($"LE SPAWN TASK CreateCanine 6 FAIL");
            return null;
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"LESpawn: ERROR DELETED PERSON {ex.Message} {ex.StackTrace}", 0);
            return null;
        }
    }
    protected override VehicleExt CreateVehicle()
    {
        try
        {
            if (ClearVehicleArea)
            {
                NativeFunction.Natives.CLEAR_AREA(Position.X, Position.Y, Position.Z, 3f, true, false, false, false);
            }
            World.Vehicles.CleanupAmbient();

            if (Settings.SettingsManager.WorldSettings.CheckAreaBeforeVehicleSpawn && NativeFunction.Natives.IS_POINT_OBSCURED_BY_A_MISSION_ENTITY<bool>(Position.X, Position.Y, Position.Z, 0.1f, 0.5f, 1f, 0))//NativeFunction.Natives.IS_POSITION_OCCUPIED<bool>(Position.X, Position.Y, Position.Z, 0.1f, false, true, false, false, false, false, false))
            {
                EntryPoint.WriteToConsole("LE SPAWN TASK POSITION OCCUPIED");
                return null;
            }

            EntryPoint.WriteToConsole($"LE SPAWN TASK POSITION OF SPAWN {Position} WaterPosition:{SpawnLocation.WaterPosition} InitialPosition:{SpawnLocation.InitialPosition} ISWater:{SpawnLocation.IsWater} DebugWaterHeight{SpawnLocation.DebugWaterHeight}");
            SpawnedVehicle = new Vehicle(VehicleType.ModelName, Position, SpawnLocation.Heading);
            EntryPoint.SpawnedEntities.Add(SpawnedVehicle);
            GameFiber.Yield();
            NativeFunction.Natives.SET_MODEL_AS_NO_LONGER_NEEDED(Game.GetHashKey(VehicleType.ModelName));
            if (!SpawnedVehicle.Exists())
            {
                return null;
            }
            PoliceVehicleExt CreatedVehicle = World.Vehicles.GetPolice(SpawnedVehicle);//.GetVehicleExt(SpawnedVehicle);
            if (CreatedVehicle == null)
            {
                CreatedVehicle = new PoliceVehicleExt(SpawnedVehicle, Settings);
                CreatedVehicle.Setup();
                CreatedVehicle.AssociatedAgency = Agency;
                CreatedVehicle.AddVehicleToList(World);
                CreatedVehicle.DispatchableVehicle = VehicleType;
            }
            CreatedVehicle.IsPolice = true;
            CreatedVehicle.WasModSpawned = true;
            CreatedVehicle.IsOffDuty = IsOffDutySpawn;
            GameFiber.Yield();
            if (!SpawnedVehicle.Exists())
            {
                return null;
            }
            VehicleType.SetVehicleExtPermanentStats(CreatedVehicle, true);
            CreatedVehicle.UpgradePerformance();
            if (IsOffDutySpawn)
            {
                CreatedVehicle.UpdatePlateType(true, World.ModDataFileManager.Zones, World.ModDataFileManager.PlateTypes,true, true);
            }
            else
            {
                CreatedVehicle.UpdatePlatePrefix(Agency);
            }
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
            else
            {
                EntryPoint.ModController.AddSpawnError(new SpawnError(Game.GetHashKey(VehicleType.ModelName), Position, Game.GameTime));
            }
            GameFiber.Yield();
            return null;
        }
    }


    protected override void PutPedInVehicle(PedExt Person, int Seat)
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

        if(IsMarshalMember)
        {
            PrimaryCop.IsMarshalTaskForceMember = true;
        }
        if(IsOffDutySpawn)
        {
            PrimaryCop.IsOffDuty = true;
        }
        SpawnedCops.Add(PrimaryCop);
        World.Pedestrians.AddEntity(PrimaryCop);
        float sightDistance = Settings.SettingsManager.PoliceSettings.SightDistance;
        if(VehicleType != null && (VehicleType.IsHelicopter || VehicleType.IsPlane))
        {
            sightDistance = Settings.SettingsManager.PoliceSettings.SightDistance_Aircraft;
        }
        if(PersonType.OverrideSightDistance > 1.0f)
        {
            sightDistance = PersonType.OverrideSightDistance;
            EntryPoint.WriteToConsole($"OverrideSightDistance {sightDistance} IN THE COPS");
        }
        PrimaryCop.SetStats(PersonType, Weapons, AddBlip, UnitCode, sightDistance);//TASKING IS BROKEN FOR ALL COPS FAR FROM PLAYER AND ALL OTHER PEDS
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

        SpawnedCops.Add(PrimaryCop);
        float sightDistance = Settings.SettingsManager.PoliceSettings.SightDistance;
        if (VehicleType != null && (VehicleType.IsHelicopter || VehicleType.IsPlane))
        {
            sightDistance = Settings.SettingsManager.PoliceSettings.SightDistance_Aircraft;
        }
        World.Pedestrians.AddEntity(PrimaryCop);
        PrimaryCop.SetStats(PersonType, Weapons, AddBlip, UnitCode, sightDistance);
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
    protected override void GetNewPersonType(string requiredGroup)
    {
        if (Agency != null)
        {
            PersonType = Agency.GetRandomPed(World.TotalWantedLevel, requiredGroup);
        }
    }
}