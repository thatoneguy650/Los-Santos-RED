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
    private List<RandomHeadData> RandomHeadList;
    private IEntityProvideable World;
    public SpawnTask(Agency agency, SpawnLocation spawnLocation, DispatchableVehicle vehicleType, DispatchablePerson personType, bool addBlip, ISettingsProvideable settings, IWeapons weapons, INameProvideable names, bool addOptionalPassengers, List<RandomHeadData> randomHeadList, IEntityProvideable world)
    {
        Agency = agency;
        PersonType = personType;
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
        RandomHeadList = randomHeadList;
        World = world;
    }
    public SpawnTask(Gang gang, SpawnLocation spawnLocation, DispatchableVehicle vehicleType, DispatchablePerson personType, bool addBlip, ISettingsProvideable settings, IWeapons weapons, INameProvideable names, bool addOptionalPassengers, ICrimes crimes, IPedGroups pedGroups, IShopMenus shopMenus, List<RandomHeadData> randomHeadList, IEntityProvideable world)
    {
        Gang = gang;
        PersonType = personType;
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
        RandomHeadList = randomHeadList;
        World = world;
    }
    public List<PedExt> CreatedPeople { get; private set; } = new List<PedExt>();
    public List<VehicleExt> CreatedVehicles { get; private set; } = new List<VehicleExt>();
    public bool AllowAnySpawn { get; set; } = false;
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
            if(Position.DistanceTo2D(Game.LocalPlayer.Character) <= 100f && Extensions.PointIsInFrontOfPed(Game.LocalPlayer.Character, Position) && !AllowAnySpawn)
            {
                EntryPoint.WriteToConsole($"SpawnTask: Too Close and in front to spawn", 5);
                return;
            }
            
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
                                EntryPoint.WriteToConsole($"SPAWNTASK 33333 vehicle exists! {PersonType.ModelName}", 3);
                                Person.Pedestrian.WarpIntoVehicle(Vehicle.Vehicle, -1);
                                Person.AssignedVehicle = Vehicle;
                                Person.AssignedSeat = -1;

                                Person.UpdateVehicleState();

                                if (VehicleType.MinOccupants > 1 || AddOptionalPassengers)
                                {
                                    int OccupantsToAdd = RandomItems.MyRand.Next(VehicleType.MinOccupants, VehicleType.MaxOccupants + 1) - 1;
                                    for (int OccupantIndex = 1; OccupantIndex <= OccupantsToAdd; OccupantIndex++)
                                    {
                                        string requiredGroup = "";
                                        if(VehicleType != null)
                                        {
                                            requiredGroup = VehicleType.RequiredPedGroup;
                                        }
                                        if (Agency != null)
                                        {
                                            PersonType = Agency.GetRandomPed(World.TotalWantedLevel, requiredGroup);
                                        }
                                        else if (Gang != null)
                                        {
                                            PersonType = Gang.GetRandomPed(World.TotalWantedLevel, requiredGroup);
                                        }
                                        if (PersonType != null)
                                        {
                                            PedExt Passenger = CreatePerson();
                                            if (Passenger != null && Passenger.Pedestrian.Exists() && Vehicle != null && Vehicle.Vehicle.Exists())
                                            {
                                                int SeatToAssign = OccupantIndex - 1;
                                                Passenger.Pedestrian.WarpIntoVehicle(Vehicle.Vehicle, SeatToAssign);
                                                Passenger.AssignedVehicle = Vehicle;
                                                Passenger.AssignedSeat = SeatToAssign;
                                                Passenger.UpdateVehicleState();
                                            }
                                            else
                                            {
                                                EntryPoint.WriteToConsole($"SpawnTask: Adding Passenger To {VehicleType.ModelName} Failed", 5);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                EntryPoint.WriteToConsole($"SPAWNTASK 33333 no vehicle! {PersonType.ModelName}", 3);
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
                            PersonType = Gang.GetRandomPed(World.TotalWantedLevel, "");
                            if (PersonType != null)
                            {
                                PedExt Buddy = CreatePerson();
                                EntryPoint.WriteToConsole($"SpawnTask: Adding Buddy To Gang Spawn", 5);
                            }
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
            Ped ped = new Ped(PersonType.ModelName, new Vector3(Position.X, Position.Y, Position.Z + 1f), SpawnLocation.Heading);
            EntryPoint.SpawnedEntities.Add(ped);
            GameFiber.Yield();
            if (ped.Exists())
            {             
                int DesiredHealth = RandomItems.MyRand.Next(PersonType.HealthMin, PersonType.HealthMax) + 100;
                int DesiredArmor = RandomItems.MyRand.Next(PersonType.ArmorMin, PersonType.ArmorMax);
                ped.MaxHealth = DesiredHealth;
                ped.Health = DesiredHealth;
                ped.Armor = DesiredArmor;
                EntryPoint.WriteToConsole($"SPAWN TASK: CREATED PED {ped.Handle}",2);
                if(!ped.Exists())
                {
                    return null;
                }
                PedExt Person = null;
                if (Agency != null)
                {
                    Person = SetupAgencyPed(ped);
                }
                else if(Gang != null)
                {
                    Person = SetupGangMember(ped);
                }
                SetPedVariation(ped);
                GameFiber.Yield();
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
    private PedExt SetupAgencyPed(Ped ped)
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
            RelationshipGroup rg = new RelationshipGroup("COP");
            ped.RelationshipGroup = rg;
            NativeFunction.CallByName<bool>("SET_PED_AS_COP", ped, true);
            Cop PrimaryCop = new Cop(ped, Settings, ped.Health, Agency, true, null, Weapons, Names.GetRandomName(ped.IsMale), PersonType.ModelName);
            World.Pedestrians.AddEntity(PrimaryCop);
            PrimaryCop.IssueWeapons(Weapons, true, true, true);
            PrimaryCop.Accuracy = RandomItems.GetRandomNumberInt(PersonType.AccuracyMin, PersonType.AccuracyMax);
            PrimaryCop.ShootRate = RandomItems.GetRandomNumberInt(PersonType.ShootRateMin, PersonType.ShootRateMax);
            PrimaryCop.CombatAbility = RandomItems.GetRandomNumberInt(PersonType.CombatAbilityMin, PersonType.CombatAbilityMax);
            PrimaryCop.TaserAccuracy = RandomItems.GetRandomNumberInt(PersonType.TaserAccuracyMin, PersonType.TaserAccuracyMax);
            PrimaryCop.TaserShootRate = RandomItems.GetRandomNumberInt(PersonType.TaserShootRateMin, PersonType.TaserShootRateMax);
            PrimaryCop.VehicleAccuracy = RandomItems.GetRandomNumberInt(PersonType.VehicleAccuracyMin, PersonType.VehicleAccuracyMax);
            PrimaryCop.VehicleShootRate = RandomItems.GetRandomNumberInt(PersonType.VehicleShootRateMin, PersonType.VehicleShootRateMax);

            if (Settings.SettingsManager.PoliceSettings.OverrideHealth)
            {
                int health = RandomItems.GetRandomNumberInt(PersonType.HealthMin, PersonType.HealthMax) + 100;
                ped.MaxHealth = health;
                ped.Health = health;
            }
            if (Settings.SettingsManager.PoliceSettings.OverrideArmor)
            {
                int armor = RandomItems.GetRandomNumberInt(PersonType.ArmorMin, PersonType.ArmorMax);
                ped.Armor = armor;
            }

            return PrimaryCop;
        }
        else if (Agency.ResponseType == ResponseType.EMS)
        {
            RelationshipGroup rg = new RelationshipGroup("MEDIC");
            ped.RelationshipGroup = rg;
            EMT PrimaryEmt = new EMT(ped, Settings, ped.Health, Agency, true, null, Weapons, Names.GetRandomName(ped.IsMale));
            World.Pedestrians.AddEntity(PrimaryEmt);
            return PrimaryEmt;
        }
        else if (Agency.ResponseType == ResponseType.Fire)
        {
            RelationshipGroup rg = new RelationshipGroup("FIREMAN");
            ped.RelationshipGroup = rg;
            Firefighter PrimaryFirefighter = new Firefighter(ped, Settings, ped.Health, Agency, true, null, Weapons, Names.GetRandomName(ped.IsMale));
            World.Pedestrians.AddEntity(PrimaryFirefighter);
            return PrimaryFirefighter;
        }
        return null;
    }
    private PedExt SetupGangMember(Ped ped)
    {
        if (AddBlip && ped.Exists())
        {
            Blip myBlip = ped.AttachBlip();
            myBlip.Color = Color.DarkRed;
            myBlip.Scale = 0.3f;
        }
        RelationshipGroup rg = new RelationshipGroup(Gang.ID);
        ped.RelationshipGroup = rg;
        PedGroup myGroup = RelationshipGroups.GetPedGroup(Gang.ID);
        if (myGroup == null)
        {
            myGroup = new PedGroup(Gang.ID, Gang.ID, Gang.ID, false);
        }
        ShopMenu toAdd = null;
        if (RandomItems.RandomPercent(Gang.DrugDealerPercentage))
        {
            toAdd = ShopMenus.GetRandomDrugDealerMenu();//move this into the gang as well
        }
        GangMember GangMember = new GangMember(ped, Settings, Gang, true, RandomItems.RandomPercent(Gang.FightPercentage), false, Names.GetRandomName(ped.IsMale), myGroup, Crimes, Weapons) { TransactionMenu = toAdd?.Items };
        World.Pedestrians.AddEntity(GangMember);  
        GangMember.IssueWeapons(Weapons, RandomItems.RandomPercent(Gang.PercentageWithMelee), RandomItems.RandomPercent(Gang.PercentageWithSidearms), RandomItems.RandomPercent(Gang.PercentageWithLongGuns));
        GangMember.Accuracy = RandomItems.GetRandomNumberInt(PersonType.AccuracyMin, PersonType.AccuracyMax);
        GangMember.ShootRate = RandomItems.GetRandomNumberInt(PersonType.ShootRateMin, PersonType.ShootRateMax);
        GangMember.CombatAbility = RandomItems.GetRandomNumberInt(PersonType.CombatAbilityMin, PersonType.CombatAbilityMax);
        GangMember.TaserAccuracy = RandomItems.GetRandomNumberInt(PersonType.TaserAccuracyMin, PersonType.TaserAccuracyMax);
        GangMember.TaserShootRate = RandomItems.GetRandomNumberInt(PersonType.TaserShootRateMin, PersonType.TaserShootRateMax);
        GangMember.VehicleAccuracy = RandomItems.GetRandomNumberInt(PersonType.VehicleAccuracyMin, PersonType.VehicleAccuracyMax);
        GangMember.VehicleShootRate = RandomItems.GetRandomNumberInt(PersonType.VehicleShootRateMin, PersonType.VehicleShootRateMax);

        if (Settings.SettingsManager.GangSettings.OverrideHealth)
        {
            int health = RandomItems.GetRandomNumberInt(PersonType.HealthMin, PersonType.HealthMax) + 100;
            ped.MaxHealth = health;
            ped.Health = health;
        }
        if (Settings.SettingsManager.GangSettings.OverrideArmor)
        {
            int armor = RandomItems.GetRandomNumberInt(PersonType.ArmorMin, PersonType.ArmorMax);
            ped.Armor = armor;
        }
        if (Settings.SettingsManager.GangSettings.OverrideAccuracy)
        {
            ped.Accuracy = GangMember.Accuracy;
            NativeFunction.Natives.SET_PED_SHOOT_RATE(ped, GangMember.ShootRate);
            NativeFunction.Natives.SET_PED_COMBAT_ABILITY(ped, GangMember.CombatAbility);
        }
        return GangMember;
    }
    private void SetPedVariation(Ped ped)
    {
        if (PersonType.RequiredVariation == null)
        {
            ped.RandomizeVariation();
        }
        else
        {
            PersonType.RequiredVariation.ApplyToPedSlow(ped);
            if (PersonType.RandomizeHead)
            {
                bool isMale = PersonType.ModelName.ToLower() == "mp_m_freemode_01";
                RandomizeHead(ped, RandomHeadList.Where(x => x.IsMale == isMale).PickRandom());
            }
        }

        if(PersonType.RequiredHelmetType != -1)
        {
            EntryPoint.WriteToConsole($"HELMET REQUIRED: PersonType.RequiredHelmetType {PersonType.RequiredHelmetType}");
            ped.GiveHelmet(false, (HelmetTypes)PersonType.RequiredHelmetType, 4096);
        }


    }
    public void RandomizeHead(Ped ped, RandomHeadData myHead)
    {
        GameFiber.Yield();
        if (ped.Exists())
        {
            int HairColor = myHead.HairColors.PickRandom();
            int HairID = myHead.HairComponents.PickRandom();
            if (ped.Exists())
            {
                NativeFunction.Natives.SET_PED_COMPONENT_VARIATION(ped, 2, HairID, 0, 0);
                GameFiber.Yield();
            }
            if (ped.Exists())
            {
                NativeFunction.Natives.SET_PED_HEAD_BLEND_DATA(ped, myHead.HeadID, myHead.HeadID, 0, myHead.HeadID, myHead.HeadID, 0, 1.0f, 0, 0, false);
                GameFiber.Yield();
            }
            if (ped.Exists())
            {
                NativeFunction.Natives.x4CFFC65454C93A49(ped, HairColor, HairColor);
                GameFiber.Yield();
            }
            if (ped.Exists())
            {
                NativeFunction.Natives.SET_PED_HEAD_OVERLAY(ped, 2, RandomItems.GetRandomNumberInt(0, 5), 1.0f);
                GameFiber.Yield();
            }
            if (ped.Exists())
            {
                NativeFunction.Natives.x497BF74A7B9CB952(ped, 2, 1, HairColor, HairColor);//colors?
                GameFiber.Yield();
            }
            EntryPoint.WriteToConsole($"myHead {myHead.HeadID} {myHead.Name} HairID {HairID}");
        }
    }
    private VehicleExt CreateVehicle()
    {
        try
        {
            EntryPoint.WriteToConsole($"SPAWNTASK Attempting to spawn {VehicleType.ModelName}", 3);
            SpawnedVehicle = new Vehicle(VehicleType.ModelName, Position, SpawnLocation.Heading);
            EntryPoint.SpawnedEntities.Add(SpawnedVehicle);
            GameFiber.Yield();
            if (SpawnedVehicle.Exists())
            {
                VehicleExt CreatedVehicle = World.Vehicles.GetVehicleExt(SpawnedVehicle);
                if (CreatedVehicle == null)
                {
                    CreatedVehicle = new VehicleExt(SpawnedVehicle, Settings);
                }
                CreatedVehicle.WasModSpawned = true;
                if (Agency != null)
                {
                    World.Vehicles.AddEntity(CreatedVehicle, Agency.ResponseType);
                }
                else if (Gang != null)
                {
                    World.Vehicles.AddEntity(CreatedVehicle, ResponseType.None);
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
                    CreatedVehicle.AssociatedGang = Gang;
                    if (SpawnedVehicle.Exists() && VehicleType.RequiredPrimaryColorID != -1)
                    {
                        NativeFunction.Natives.SET_VEHICLE_COLOURS(SpawnedVehicle, VehicleType.RequiredPrimaryColorID, VehicleType.RequiredSecondaryColorID == -1 ? VehicleType.RequiredPrimaryColorID : VehicleType.RequiredSecondaryColorID);
                    }
                    EntryPoint.WriteToConsole($"SPAWNTASK SPAWNED {VehicleType.ModelName}", 3);
                    GameFiber.Yield();
                    return CreatedVehicle;
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
            return null;
        }
    }
}
