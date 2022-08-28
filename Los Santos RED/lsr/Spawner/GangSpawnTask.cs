using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Linq;

public class GangSpawnTask : SpawnTask
{
    private bool AddBlip;
    private bool AddOptionalPassengers = false;
    private Gang Gang;
    private ICrimes Crimes;
    private VehicleExt LastCreatedVehicle;
    private INameProvideable Names;
    private int OccupantsToAdd;
    private IPedGroups RelationshipGroups;
    private ISettingsProvideable Settings;
    private IShopMenus ShopMenus;
    private Vehicle SpawnedVehicle;
    private IWeapons Weapons;
    private IEntityProvideable World;
    public GangSpawnTask(Gang gang, SpawnLocation spawnLocation, DispatchableVehicle vehicleType, DispatchablePerson personType, bool addBlip, ISettingsProvideable settings, IWeapons weapons, INameProvideable names, bool addOptionalPassengers, ICrimes crimes, IPedGroups pedGroups, IShopMenus shopMenus, IEntityProvideable world) : base(spawnLocation, vehicleType, personType)
    {
        Gang = gang;
        PersonType = personType;
        VehicleType = vehicleType;
        AddBlip = addBlip;
        SpawnLocation = spawnLocation;
        Settings = settings;
        Weapons = weapons;
        Names = names;
        Crimes = crimes;
        RelationshipGroups = pedGroups;
        AddOptionalPassengers = addOptionalPassengers;
        ShopMenus = shopMenus;
        World = world;
    }
    private bool HasGang => Gang != null;
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
                EntryPoint.WriteToConsole($"GangSpawn: Task Invalid Spawn Position");
                return;
            }
            if (!HasGang)
            {
                EntryPoint.WriteToConsole($"GangSpawn: Task No Agency Supplied");
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
        EntryPoint.WriteToConsole($"SPAWN TASK: OccupantsToAdd {OccupantsToAdd}");
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
                    EntryPoint.WriteToConsole($"SpawnTask: Adding Buddy To Gang Spawn", 5);
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
                EntryPoint.WriteToConsole("GangSpawn Task CREATED PED!");
                SetupPed(createdPed);
                if (!createdPed.Exists())
                {
                    return null;
                }
                PedExt Person = SetupGangPed(createdPed);
                PersonType.SetPedVariation(createdPed, Gang.PossibleHeads);
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
            EntryPoint.WriteToConsole($"GangSpawn: Attempting to spawn {VehicleType.ModelName}", 3);
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
                if (Gang != null)
                {
                    World.Vehicles.AddEntity(CreatedVehicle, ResponseType.None);
                }
                if (SpawnedVehicle.Exists())
                {
                    CreatedVehicle.WasModSpawned = true;
                    SpawnedVehicle.IsPersistent = true;
                    CreatedVehicle.AssociatedGang = Gang;
                    EntryPoint.PersistentVehiclesCreated++;
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

                    EntryPoint.WriteToConsole($"GangSpawn: SPAWNED {VehicleType.ModelName}", 3);
                    GameFiber.Yield();
                    return CreatedVehicle;
                }
            }
            return null;
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
        ped.IsPersistent = true;
        EntryPoint.PersistentPedsCreated++;//TR
        if (AddBlip && ped.Exists())
        {
            Blip myBlip = ped.AttachBlip();
            myBlip.Color = Gang.Color;
            myBlip.Scale = 0.3f;
        }
        RelationshipGroup rg = new RelationshipGroup(Gang.ID);
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
        ShopMenu toAdd = null;
        if (RandomItems.RandomPercent(Gang.DrugDealerPercentage))
        {
            toAdd = ShopMenus.GetRandomMenu(Gang.DealerMenuGroup);
            if (toAdd == null)
            {
                toAdd = ShopMenus.GetRandomDrugDealerMenu();
            }
        }
        GangMember GangMember = new GangMember(ped, Settings, Gang, true, RandomItems.RandomPercent(Gang.FightPercentage), false, Names.GetRandomName(isMale), Crimes, Weapons) { ShopMenu = toAdd };
        World.Pedestrians.AddEntity(GangMember);
        if (GangMember.Pedestrian.Exists())
        {
            GangMember.Pedestrian.Money = 0;// GangMember.Money;
        }
        GangMember.WeaponInventory.IssueWeapons(Weapons, RandomItems.RandomPercent(Gang.PercentageWithMelee), RandomItems.RandomPercent(Gang.PercentageWithSidearms), RandomItems.RandomPercent(Gang.PercentageWithLongGuns), PersonType.EmptyHolster,PersonType.FullHolster);
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
        if (GangMember != null && PersonType.OverrideVoice != null && PersonType.OverrideVoice.Any())
        {
            GangMember.VoiceName = PersonType.OverrideVoice.PickRandom();
        }
        return GangMember;
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