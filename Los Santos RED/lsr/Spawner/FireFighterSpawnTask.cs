using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Linq;

public class FireFighterSpawnTask : SpawnTask
{
    private Agency Agency;
    private Vehicle SpawnedVehicle;
    private IShopMenus ShopMenus;
    public FireFighterSpawnTask(Agency agency, SpawnLocation spawnLocation, DispatchableVehicle vehicleType, DispatchablePerson personType, bool addBlip, ISettingsProvideable settings, IWeapons weapons, INameProvideable names, bool addOptionalPassengers, 
        IEntityProvideable world, IModItems modItems, IShopMenus shopMenus) 
        : base(spawnLocation, vehicleType, personType, addBlip, addOptionalPassengers, settings, weapons, names, world, modItems)
    {
        Agency = agency;
        ShopMenus = shopMenus;
    }
    private bool HasAgency => Agency != null;
    public override void AttemptSpawn()
    {
        try
        {
            GameFiber.Yield();
            if (IsInvalidSpawnPosition)
            {
                //EntryPoint.WriteToConsoleTestLong($"FireFighterSpawn: Task Invalid Spawn Position");
                return;
            }
            if (!HasAgency)
            {
                //EntryPoint.WriteToConsoleTestLong($"FireFighterSpawn: Task No Agency Supplied");
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
            FireVehicleExt CreatedVehicle = World.Vehicles.GetFire(SpawnedVehicle);
            if (CreatedVehicle == null)
            {
                CreatedVehicle = new FireVehicleExt(SpawnedVehicle, Settings);
                CreatedVehicle.Setup();
                CreatedVehicle.AssociatedAgency = Agency;
                CreatedVehicle.AddVehicleToList(World);
            }
            CreatedVehicle.WasModSpawned = true;
            CreatedVehicle.IsFire = true;
            GameFiber.Yield();
            if (!SpawnedVehicle.Exists())
            {
                return null;
            }
            VehicleType.SetVehicleExtPermanentStats(CreatedVehicle, true);
            CreatedVehicle.UpdatePlatePrefix(Agency);
            CreatedVehicles.Add(CreatedVehicle);
            return CreatedVehicle;
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"FireFighterSpawn: ERROR DELETED VEHICLE {ex.Message} {ex.StackTrace} ATTEMPTING {VehicleType.ModelName}", 0);
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
            OccupantsToAdd = RandomItems.MyRand.Next(VehicleType.MinOccupants, VehicleType.MaxOccupants + 1) - 1;
        }
        else
        {
            OccupantsToAdd = 0;
        }
    }
    private PedExt SetupAgencyPed(Ped ped)
    {
        if (!ped.Exists())
        {
            return null;
        }
        ped.IsPersistent = true;
        EntryPoint.PersistentPedsCreated++;//TR
        ped.RelationshipGroup = new RelationshipGroup("FIREMAN");
        bool isMale = PersonType.IsMale(ped);
        Firefighter PrimaryFirefighter = new Firefighter(ped, Settings, ped.Health, Agency, true, null, Weapons, Names.GetRandomName(isMale), World);
        World.Pedestrians.AddEntity(PrimaryFirefighter);
        PrimaryFirefighter.SetStats(PersonType, ShopMenus, Weapons, AddBlip);
        if (ped.Exists())
        {
            PrimaryFirefighter.SpawnPosition = ped.Position;
            PrimaryFirefighter.SpawnHeading = ped.Heading;
        }
        return PrimaryFirefighter;
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