using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Linq;
using System.Security.Permissions;

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
    public bool IsBackupSquad { get; set; } = false;
    public bool IsGeneralBackup { get; set; } = false;
    public int PedSpawnLimit { get; set; } = 99;

    public override void AttemptSpawn()
    {
        try
        {
            GameFiber.Yield();

            EntryPoint.WriteToConsole($"GANG SPAWN TASK Veh:{VehicleType?.ModelName} Ped:{PersonType?.ModelName} Gang:{Gang?.ShortName}");
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

    protected override void AddPassengers()
    {
        for (int OccupantIndex = 1; OccupantIndex <= OccupantsToAdd; OccupantIndex++)
        {
            EntryPoint.WriteToConsole("ADDED PASSENGER TO GANG SPAWN");
            string requiredGroup = "";
            if (VehicleType != null)
            {
                requiredGroup = VehicleType.RequiredPedGroup;
            }
            GetNewPersonType(requiredGroup);
            if (PersonType != null)
            {
                PedExt Passenger = CreatePerson(OccupantIndex - 1);
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


    protected override PedExt CreatePerson(int seat)
    {
        try
        {
            if (string.IsNullOrEmpty(PersonType.ModelName))
            {
                return null;
            }
            Vector3 CreatePos = Position;
            if (!PlacePedOnGround || VehicleType != null)
            {
                CreatePos.Z += 1.0f;//1.0f;CreatePos.Z += 1.0f;
                //EntryPoint.WriteToConsole("ADDED HIEGHT TO SPAWN");
            }
            World.Pedestrians.CleanupAmbient();
            Ped createdPed = null;
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

            //Ped createdPed = new Ped(PersonType.ModelName, new Vector3(CreatePos.X, CreatePos.Y, CreatePos.Z), SpawnLocation.Heading);
            EntryPoint.SpawnedEntities.Add(createdPed);
            GameFiber.Yield();
            NativeFunction.Natives.SET_MODEL_AS_NO_LONGER_NEEDED(Game.GetHashKey(PersonType.ModelName));
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
            //foreach (Entity entity in Rage.World.GetEntities(Position, 3.0f, GetEntitiesFlags.ConsiderAllPeds | GetEntitiesFlags.ExcludePlayerPed).ToList())
            //{
            //    if (entity.Exists())
            //    {
            //        entity.Delete();
            //    }
            //}
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
            if (Settings.SettingsManager.WorldSettings.CheckAreaBeforeVehicleSpawn && NativeFunction.Natives.IS_POINT_OBSCURED_BY_A_MISSION_ENTITY<bool>(Position.X, Position.Y, Position.Z, 0.1f, 0.5f, 1f, 0))
            {
                return null;
            }
            SpawnedVehicle = new Vehicle(VehicleType.ModelName, Position, SpawnLocation.Heading);
            EntryPoint.SpawnedEntities.Add(SpawnedVehicle);
            GameFiber.Yield();
            NativeFunction.Natives.SET_MODEL_AS_NO_LONGER_NEEDED(Game.GetHashKey(VehicleType.ModelName));
            if (!SpawnedVehicle.Exists())
            {
                return null;
            }
            GangVehicleExt CreatedVehicle = World.Vehicles.GetGang(SpawnedVehicle);
            if (CreatedVehicle == null)
            {
                CreatedVehicle = new GangVehicleExt(SpawnedVehicle, Settings);
                CreatedVehicle.Setup();
                CreatedVehicle.AddVehicleToList(World);
            }
            CreatedVehicle.IsGang = true;
            CreatedVehicle.WasModSpawned = true;
            GameFiber.Yield();
            if (!SpawnedVehicle.Exists())
            {
                return null;
            }
            VehicleType.SetVehicleExtPermanentStats(CreatedVehicle, true);
            CreatedVehicle.AssociatedGang = Gang;
            CreatedVehicle.UpdatePlatePrefix(Gang);
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
            else
            {
                EntryPoint.ModController.AddSpawnError(new SpawnError(Game.GetHashKey(VehicleType.ModelName), Position, Game.GameTime));
            }
            GameFiber.Yield();
            return null;
        }
    }
    private void Setup()
    {
        if (VehicleType != null)
        {

            if ((IsHitSquad || IsBackupSquad || IsGeneralBackup) && RandomItems.RandomPercent(80))
            {
                OccupantsToAdd = VehicleType.MaxOccupants - 1;
                EntryPoint.WriteToConsole($"HITSQUAD BACKUP OCCUPANTS TO ADD 1 {OccupantsToAdd} PedSpawnLimit{PedSpawnLimit}");
            }
            else
            {
                OccupantsToAdd = RandomItems.MyRand.Next(VehicleType.MinOccupants, VehicleType.MaxOccupants + 1) - 1;
            }
            if(OccupantsToAdd + 1 > PedSpawnLimit)
            {
                OccupantsToAdd = PedSpawnLimit - 1;

                EntryPoint.WriteToConsole($"HITSQUAD BACKUP OCCUPANTS TO ADD 2 {OccupantsToAdd} PedSpawnLimit{PedSpawnLimit}");

            }
            if(OccupantsToAdd < 0)
            {
                OccupantsToAdd = 0;
                EntryPoint.WriteToConsole($"HITSQUAD BACKUP OCCUPANTS TO ADD 3 {OccupantsToAdd} PedSpawnLimit{PedSpawnLimit}");
            }
        }
        else
        {
            OccupantsToAdd = 0;
        }


        EntryPoint.WriteToConsole($"OCCUPANTS TO ADD 4 {OccupantsToAdd} PedSpawnLimit{PedSpawnLimit}");

    }
    private PedExt SetupGangPed(Ped ped)
    {
        if(!ped.Exists())
        {
            return null;
        }

        if (Settings.SettingsManager.GangSettings.SetPersistent)
        {
            ped.IsPersistent = true;
            EntryPoint.PersistentPedsCreated++;//TR
        }


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
        GangMember.IsBackupSquad = IsBackupSquad;
        GangMember.IsGeneralBackup = IsGeneralBackup;
        World.Pedestrians.AddEntity(GangMember);
        if(GangMember.IsHitSquad || GangMember.IsBackupSquad || GangMember.IsGeneralBackup)
        {
            ForceMelee = true;
            ForceSidearm = true;
            ForceLongGun = true;
        }
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
    protected override void GetNewPersonType(string requiredGroup)
    {
        if (Gang != null)
        {
            PersonType = Gang.GetRandomPed(World.TotalWantedLevel, requiredGroup);
        }
    }
}