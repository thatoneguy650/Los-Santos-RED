using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

public class MerchantSpawnTask : SpawnTask
{

    protected Vehicle SpawnedVehicle;
    protected ICrimes Crimes;
    protected IShopMenus ShopMenus;
    public bool SetPersistent { get; set; } = false;
    public MerchantSpawnTask(SpawnLocation spawnLocation, DispatchableVehicle vehicleType, DispatchablePerson personType, bool addBlip, bool addOptionalPassengers, bool setPersistent, ISettingsProvideable settings,
        ICrimes crimes, IWeapons weapons, INameProvideable names, IEntityProvideable world, IModItems modItems, IShopMenus shopMenus)
        : base(spawnLocation, vehicleType, personType, addBlip, addOptionalPassengers, settings, weapons, names, world, modItems)
    {
        Crimes = crimes;
        SetPersistent = setPersistent;
        ShopMenus = shopMenus;
    }
    public override void AttemptSpawn()
    {
        try
        {
            GameFiber.Yield();
            if (IsInvalidSpawnPosition)
            {
                //EntryPoint.WriteToConsoleTestLong($"CivilianSpawn: Task Invalid Spawn Position");
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
            EntryPoint.WriteToConsole($"CivilianSpawn: ERROR {ex.Message} : {ex.StackTrace}", 0);
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
            if (!createdPed.Exists())
            {
                return null;
            }
            SetupPed(createdPed);
            if (!createdPed.Exists())
            {
                return null;
            }
            PedExt Person = SetupMerchantPed(createdPed);
            PersonType?.SetPedVariation(createdPed, null, true);
            GameFiber.Yield();
            CreatedPeople.Add(Person);
            return Person;
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"CivilianSpawn: ERROR DELETED PERSON {ex.Message} {ex.StackTrace}", 0);
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

    //private void NumberTwO()
    //{
    //    //Ped ped;
    //    string ModelName;
    //    if (VendorModels != null && VendorModels.Any())
    //    {
    //        ModelName = VendorModels.PickRandom();
    //    }
    //    else
    //    {
    //        ModelName = FallBackVendorModels.PickRandom();
    //    }
    //    NativeFunction.Natives.CLEAR_AREA(CreatePos.X, CreatePos.Y, CreatePos.Z, 2f, true, false, false, false);
    //    EntryPoint.WriteToConsole($"ATTEMPTING VENDOR AT {Name} {ModelName}");
    //    Ped ped = new Ped(ModelName, CreatePos, VendorHeading);
    //    GameFiber.Yield();
    //    if (ped.Exists())
    //    {
    //        Vendor = new Merchant(ped, Settings, "Vendor", Crimes, Weapons, World);

    //        if (!World.Pedestrians.Merchants.Any(x => x.Handle == Vendor.Handle))
    //        {
    //            World.Pedestrians.Merchants.Add(Vendor);
    //        }
    //        ped.IsPersistent = true;//THIS IS ON FOR NOW!
    //        ped.RandomizeVariation();
    //        NativeFunction.CallByName<bool>("TASK_START_SCENARIO_IN_PLACE", ped, "WORLD_HUMAN_STAND_IMPATIENT", 0, true);
    //        ped.KeepTasks = true;
    //        EntryPoint.SpawnedEntities.Add(ped);
    //        GameFiber.Yield();
    //        if (ped.Exists())
    //        {
    //            if (addMenu)
    //            {
    //                Vendor.SetupTransactionItems(Menu);
    //            }
    //            Vendor.AssociatedStore = this;
    //            Vendor.SpawnPosition = CreatePos;
    //            EntryPoint.WriteToConsole($"SPAWNED WORKED VENDOR AT {Name}");
    //        }
    //    }
    //}
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
    private PedExt SetupMerchantPed(Ped ped)
    {
        ped.IsPersistent = SetPersistent;
        EntryPoint.PersistentPedsCreated++;//TR
        bool isMale = PersonType.IsMale(ped);
        ped.RelationshipGroup = isMale ? new RelationshipGroup("CIVMALE") : new RelationshipGroup("CIVFEMALE");
        PedExt CreatedPedExt = new PedExt(ped, Settings, Crimes, Weapons, Names.GetRandomName(isMale), "", World);
        World.Pedestrians.AddEntity(CreatedPedExt);
        CreatedPedExt.SetBaseStats(PersonType, ShopMenus, Weapons, AddBlip);
        if (ped.Exists())
        {
            CreatedPedExt.SpawnPosition = ped.Position;
            CreatedPedExt.SpawnHeading = ped.Heading;
        }
        return CreatedPedExt;
    }
    protected void SetupPed(Ped ped)
    {
        PlacePed(ped);
        int DesiredHealth = RandomItems.MyRand.Next(PersonType.HealthMin, PersonType.HealthMax) + 100;
        int DesiredArmor = RandomItems.MyRand.Next(PersonType.ArmorMin, PersonType.ArmorMax);
        ped.MaxHealth = DesiredHealth;
        ped.Health = DesiredHealth;
        ped.Armor = DesiredArmor;
    }
}