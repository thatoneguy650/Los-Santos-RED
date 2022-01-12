using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosSantosRED.lsr.Interface;

public class SpawnTask
{
    private DispatchablePerson PersonType;
    private Agency Agency;
    private bool AddBlip;
    //private Vector3 InitialPosition;
    //private Vector3 StreetPosition;
    //private float Heading;
    private VehicleExt Vehicle;
    private DispatchableVehicle VehicleType;
    private ISettingsProvideable Settings;
    private IWeapons Weapons;
    private INameProvideable Names;
    private Vehicle SpawnedVehicle;
    private bool AddOptionalPassengers = false;
    private Gang Gang;
    private IPedGroups RelationshipGroups;
    private ICrimes Crimes;
    private IShopMenus ShopMenus;

    private SpawnLocation SpawnLocation;

    public SpawnTask(Agency agency, SpawnLocation spawnLocation, DispatchableVehicle vehicleType, DispatchablePerson officerType, bool addBlip, ISettingsProvideable settings, IWeapons weapons, INameProvideable names, bool addOptionalPassengers)
    {
        Agency = agency;
        PersonType = officerType;
        VehicleType = vehicleType;
        AddBlip = addBlip;
        SpawnLocation = spawnLocation;
        //InitialPosition = initialPosition;
        //StreetPosition = streetPosition;
        //Heading = heading;
        Settings = settings;
        Weapons = weapons;
        Names = names;
        AddOptionalPassengers = addOptionalPassengers;
    }
    public SpawnTask(Gang gang, SpawnLocation spawnLocation, DispatchableVehicle vehicleType, DispatchablePerson officerType, bool addBlip, ISettingsProvideable settings, IWeapons weapons, INameProvideable names, bool addOptionalPassengers, ICrimes crimes, IPedGroups pedGroups, IShopMenus shopMenus)
    {
        Gang = gang;
        PersonType = officerType;
        VehicleType = vehicleType;
        AddBlip = addBlip;
        SpawnLocation = spawnLocation;
        //InitialPosition = initialPosition;
        //StreetPosition = streetPosition;
        //Heading = heading;
        Settings = settings;
        Weapons = weapons;
        Names = names;
        Crimes = crimes;
        RelationshipGroups = pedGroups;
        AddOptionalPassengers = addOptionalPassengers;
        ShopMenus = shopMenus;
    }
    public List<PedExt> CreatedPeople { get; private set; } = new List<PedExt>();
    public List<VehicleExt> CreatedVehicles { get; private set; } = new List<VehicleExt>();
    private Vector3 Position
    {
        get
        {
            if(VehicleType == null)
            {
                if(SpawnLocation.HasSidewalk)
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
    public void AttemptSpawn()
    {
        try
        {
            EntryPoint.WriteToConsole($"SPAWNTASK AttemptSpawn FIRST! PersonType {PersonType?.ModelName}", 3);
            if (Agency != null || Gang != null)
            {
                if (VehicleType != null)
                {
                    Vehicle = CreateVehicle();
                    if (Vehicle != null && Vehicle.Vehicle.Exists())
                    {
                        EntryPoint.WriteToConsole($"SPAWNTASK 11111111111", 3);
                        if (PersonType != null)
                        {
                            EntryPoint.WriteToConsole($"SPAWNTASK 222222 {PersonType.ModelName}", 3);
                            PedExt Person = CreatePerson();
                            if (Person != null && Person.Pedestrian.Exists() && Vehicle != null && Vehicle.Vehicle.Exists())
                            {
                                Person.Pedestrian.WarpIntoVehicle(Vehicle.Vehicle, -1);
                                Person.AssignedVehicle = Vehicle;
                                Person.AssignedSeat = -1;
                                if (VehicleType.MinOccupants > 1 || AddOptionalPassengers)
                                {
                                    int OccupantsToAdd = RandomItems.MyRand.Next(VehicleType.MinOccupants, VehicleType.MaxOccupants + 1) - 1;
                                    for (int OccupantIndex = 1; OccupantIndex <= OccupantsToAdd; OccupantIndex++)
                                    {
                                        PedExt Passenger = CreatePerson();
                                        if (Passenger != null && Passenger.Pedestrian.Exists() && Vehicle != null && Vehicle.Vehicle.Exists())
                                        {
                                            int SeatToAssign = OccupantIndex - 1;
                                            Passenger.Pedestrian.WarpIntoVehicle(Vehicle.Vehicle, SeatToAssign);
                                            Passenger.AssignedVehicle = Vehicle;
                                            Passenger.AssignedSeat = SeatToAssign;
                                        }
                                        else
                                        {
                                            EntryPoint.WriteToConsole($"SpawnTask: Adding Passenger To {VehicleType.ModelName} Failed", 5);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (Vehicle != null && Vehicle.Vehicle.Exists())
                                {
                                    Vehicle.Vehicle.Delete();
                                    EntryPoint.PersistentVehiclesDeleted++;
                                    EntryPoint.WriteToConsole("SpawnTask: Failed to complete spawn, deleting", 5);
                                }
                            }
                        }
                    }
                }
                else if (PersonType != null)
                {
                    CreatePerson();
                    if (Gang != null)
                    {
                        int BuddiesToSpawn = RandomItems.MyRand.Next(1, 2 + 1) - 1;
                        for (int BuddyIndex = 1; BuddyIndex <= BuddiesToSpawn; BuddyIndex++)
                        {
                            PedExt Buddy = CreatePerson();
                            EntryPoint.WriteToConsole($"SpawnTask: Adding Buddy To Gang Spawn", 5);
                        }
                    }
                }

            }
        }
        catch(Exception ex)
        {
            EntryPoint.WriteToConsole($"SPAWN TASK: Spawn ERROR {ex.Message} : {ex.StackTrace}", 0);
            if (Vehicle != null && Vehicle.Vehicle.Exists())
            {
                Vehicle.Vehicle.Delete();
                EntryPoint.WriteToConsole($"SPAWN TASK: Spawn ERROR DELETED VEHICLE", 0);
            }
            foreach(PedExt person in CreatedPeople)
            {
                if(person != null && person.Pedestrian.Exists())
                {
                    person.Pedestrian.Delete();
                    EntryPoint.WriteToConsole($"SPAWN TASK: Spawn ERROR DELETED PED", 0);
                }
            }
        }
    }
    private PedExt CreatePerson()
    {
        try
        {
            EntryPoint.WriteToConsole($"SPAWNTASK Attempting to spawn {PersonType.ModelName}", 3);
            Ped ped = new Ped(PersonType.ModelName, new Vector3(Position.X, Position.Y, Position.Z + 1f), SpawnLocation.Heading);
            //Model modelToCreate = new Model(Game.GetHashKey(PersonType.ModelName));
            //modelToCreate.LoadAndWait();
            //Ped ped = NativeFunction.Natives.CREATE_PED<Ped>(26, Game.GetHashKey(PersonType.ModelName), Position.X, Position.Y, Position.Z + 1f, Heading, false, false);
            EntryPoint.SpawnedEntities.Add(ped);
            GameFiber.Yield();
            if (ped.Exists())
            {
                int DesiredHealth = RandomItems.MyRand.Next(PersonType.HealthMin, PersonType.HealthMax) + 100;
                ped.MaxHealth = DesiredHealth;
                ped.Health = DesiredHealth;
                ped.Armor = RandomItems.MyRand.Next(PersonType.ArmorMin, PersonType.ArmorMax);








                EntryPoint.WriteToConsole($"SPAWN TASK: CREATED PED {ped.Handle}",2);
                ped.RandomizeVariation();
                if (VehicleType != null && VehicleType.IsMotorcycle && Agency != null)
                {
                    ped.GiveHelmet(false, HelmetTypes.PoliceMotorcycleHelmet, 4096);
                    NativeFunction.CallByName<uint>("SET_PED_COMPONENT_VARIATION", ped, 4, 0, 0, 0);
                }
                else
                {
                    NativeFunction.CallByName<uint>("SET_PED_COMPONENT_VARIATION", ped, 4, 1, 0, 0);
                }
                if (PersonType.RequiredVariation != null)
                {
                    PersonType.RequiredVariation.ApplyToPed(ped);
                }
                GameFiber.Yield();
                if(!ped.Exists())
                {
                    return null;
                }

                PedExt Person = null;
                if (Agency != null)
                {




                    ped.IsPersistent = true;
                    EntryPoint.PersistentPedsCreated++;//TR





                    if (AddBlip && ped.Exists())
                    {
                        Blip myBlip = ped.AttachBlip();
                        myBlip.Color = Agency.Color;
                        myBlip.Scale = 0.6f;
                    }
                    if (Agency.ResponseType == ResponseType.LawEnforcement)
                    {
                        NativeFunction.CallByName<bool>("SET_PED_AS_COP", ped, true);
                        Cop PrimaryCop = new Cop(ped, Settings, ped.Health, Agency, true, null, Weapons, Names.GetRandomName(ped.IsMale));
                        PrimaryCop.IssueWeapons(Weapons,(uint)WeaponHash.StunGun,true,true);
                        PrimaryCop.Accuracy = RandomItems.GetRandomNumberInt(Agency.AccuracyMin, Agency.AccuracyMax);
                        PrimaryCop.ShootRate = RandomItems.GetRandomNumberInt(Agency.ShootRateMin, Agency.ShootRateMax);
                        PrimaryCop.CombatAbility = RandomItems.GetRandomNumberInt(Agency.CombatAbilityMin, Agency.CombatAbilityMax);
                        Person = PrimaryCop;
                    }
                    else if (Agency.ResponseType == ResponseType.EMS)
                    {
                        EMT PrimaryEmt = new EMT(ped, Settings, ped.Health, Agency, true, null, Weapons, Names.GetRandomName(ped.IsMale));
                        Person = PrimaryEmt;
                    }
                    else if (Agency.ResponseType == ResponseType.Fire)
                    {
                        Firefighter PrimaryFirefighter = new Firefighter(ped, Settings, ped.Health, Agency, true, null, Weapons, Names.GetRandomName(ped.IsMale));
                        Person = PrimaryFirefighter;
                    }
                }
                else if(Gang != null)
                {
                    EntryPoint.WriteToConsole($"SPAWN TASK: CREATED GANG MEMBER {Gang.ID}", 5);
                    if (AddBlip && ped.Exists())
                    {
                        Blip myBlip = ped.AttachBlip();
                        myBlip.Color = Gang.Color;
                        myBlip.Scale = 0.6f;
                    }

                    RelationshipGroup rg = new RelationshipGroup(Gang.ID);
                    ped.RelationshipGroup = rg;

                    PedGroup myGroup = RelationshipGroups.GetPedGroup(Gang.ID);
                    if (myGroup == null)
                    {
                        myGroup = new PedGroup(Gang.ID, Gang.ID, Gang.ID, false);
                    }
                    ShopMenu toAdd = null;
                    if (RandomItems.RandomPercent(Settings.SettingsManager.CivilianSettings.GangDrugDealPercentage))
                    {
                        toAdd = ShopMenus.GetRanomdDrugMenu();//move this into the gang as well
                    }
                    GangMember GangMember = new GangMember(ped, Settings, Gang, true, RandomItems.RandomPercent(Settings.SettingsManager.CivilianSettings.GangFightPercentage), false, Names.GetRandomName(ped.IsMale), myGroup, Crimes, Weapons) { TransactionMenu = toAdd?.Items };
                    Person = GangMember;
                    WeaponInformation melee =  Weapons.GetRandomRegularWeapon(WeaponCategory.Melee);//move this into the gang soon
                    uint meleeHash = 0;
                    if (melee != null && RandomItems.RandomPercent(Gang.PercentageWithMelee))
                    {
                        meleeHash = (uint)melee.Hash;
                    }
                    GangMember.IssueWeapons(Weapons, meleeHash, RandomItems.RandomPercent(Gang.PercentageWithSidearms), RandomItems.RandomPercent(Gang.PercentageWithLongGuns));
                    GangMember.Accuracy = RandomItems.GetRandomNumberInt(Gang.AccuracyMin, Gang.AccuracyMax);
                    GangMember.ShootRate = RandomItems.GetRandomNumberInt(Gang.ShootRateMin, Gang.ShootRateMax);
                    GangMember.CombatAbility = RandomItems.GetRandomNumberInt(Gang.CombatAbilityMin, Gang.CombatAbilityMax);
                    ped.Accuracy = GangMember.Accuracy;
                    NativeFunction.Natives.SET_PED_SHOOT_RATE(ped, GangMember.ShootRate);
                    NativeFunction.Natives.SET_PED_COMBAT_ABILITY(ped, GangMember.CombatAbility);
                    //ped.BlockPermanentEvents = false;
                    //ped.KeepTasks = false;
                    //ped.Tasks.Clear();
                    //if (ped.IsInAnyVehicle(false) && ped.CurrentVehicle.Exists())
                    //{
                    //    NativeFunction.Natives.TASK_VEHICLE_DRIVE_WANDER(ped, ped.CurrentVehicle, 10f, (int)(VehicleDrivingFlags.FollowTraffic | VehicleDrivingFlags.YieldToCrossingPedestrians | VehicleDrivingFlags.RespectIntersections | (VehicleDrivingFlags)8), 10f);
                    //}
                    //else
                    //{
                    //    NativeFunction.Natives.TASK_WANDER_STANDARD(ped, 0, 0);
                    //}
                }
                CreatedPeople.Add(Person);
                return Person;
            }
            return null;
        }
        catch(Exception ex)
        {
            EntryPoint.WriteToConsole($"SPAWN TASK: Spawn ERROR DELETED PERSON {ex.Message} {ex.StackTrace}", 0);
            return null;
        }
    }
    private VehicleExt CreateVehicle()
    {
        try
        {
            EntryPoint.WriteToConsole($"SPAWNTASK Attempting to spawn {VehicleType.ModelName}", 3);
            SpawnedVehicle = new Vehicle(VehicleType.ModelName, Position, SpawnLocation.Heading);
            //Model modelToCreate = new Model(Game.GetHashKey(VehicleType.ModelName));
            //modelToCreate.LoadAndWait();
            //SpawnedVehicle = NativeFunction.Natives.CREATE_VEHICLE<Vehicle>(Game.GetHashKey(VehicleType.ModelName), Position.X, Position.Y, Position.Z, Heading, false, false);
            //PoolHandle SpawnedHandle = NativeFunction.Natives.CREATE_VEHICLE<PoolHandle>(Game.GetHashKey(VehicleType.ModelName), Position.X, Position.Y, Position.Z, Heading, false, false);
           // EntryPoint.WriteToConsole($"SPAWN TASK: CREATED VEHICLE?????? {SpawnedHandle}", 2);
            EntryPoint.SpawnedEntities.Add(SpawnedVehicle);
            GameFiber.Yield();
            if (SpawnedVehicle.Exists())
            {
                EntryPoint.WriteToConsole($"SPAWN TASK: CREATED VEHICLE {SpawnedVehicle.Handle}", 2);
                //if (!VehicleType.IsHelicopter && !VehicleType.IsBoat)
                //{
                //    NativeFunction.Natives.SET_VEHICLE_ON_GROUND_PROPERLY<bool>(SpawnedVehicle, 5.0f);
                //}
                VehicleExt CopVehicle = new VehicleExt(SpawnedVehicle, Settings);
                if (SpawnedVehicle.Exists())
                {
                    CopVehicle.WasModSpawned = true;
                    SpawnedVehicle.IsPersistent = true;
                    EntryPoint.PersistentVehiclesCreated++;

                    if (Agency != null)
                    {
                        CopVehicle.UpdateLivery(Agency);
                        CopVehicle.UpgradePerformance();
                    }
                    CreatedVehicles.Add(CopVehicle);
                    GameFiber.Yield();
                    return CopVehicle;
                }
            }
            return null;
        }
        catch(Exception ex)
        {
            EntryPoint.WriteToConsole($"SPAWN TASK: Spawn ERROR DELETED VEHICLE {ex.Message} {ex.StackTrace}", 0);
            if(SpawnedVehicle.Exists())
            {
                SpawnedVehicle.Delete();
            }
            GameFiber.Yield();
            //SpawnedVehicle = (Vehicle)Rage.World.GetClosestEntity(Position, 5f, GetEntitiesFlags.ConsiderAllVehicles);
            //if (SpawnedVehicle.Exists())
            //{
            //    EntryPoint.WriteToConsole($"SPAWN TASK: CREATED VEHICLE {SpawnedVehicle.Handle}", 2);
            //    if (!VehicleType.IsHelicopter && !VehicleType.IsBoat)
            //    {
            //        NativeFunction.Natives.SET_VEHICLE_ON_GROUND_PROPERLY<bool>(SpawnedVehicle, 5.0f);
            //    }
            //    VehicleExt CopVehicle = new VehicleExt(SpawnedVehicle, Settings);
            //    if (SpawnedVehicle.Exists())
            //    {
            //        CopVehicle.WasModSpawned = true;
            //        SpawnedVehicle.IsPersistent = true;
            //        EntryPoint.PersistentVehiclesCreated++;
            //        CopVehicle.UpdateLivery(Agency);
            //        CopVehicle.UpgradePerformance();
            //        CreatedVehicles.Add(CopVehicle);
            //        GameFiber.Yield();
            //        return CopVehicle;
            //    }
            //}
            return null;
        }
    }
}
