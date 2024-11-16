using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

public class MerchantSpawnTask : SpawnTask
{

    protected Vehicle SpawnedVehicle;
    protected ICrimes Crimes;
    protected IShopMenus ShopMenus;
    protected GameLocation Store;
    public bool SetPersistent { get; set; } = false;
    public List<Merchant> SpawnedVendors { get; set; } = new List<Merchant>();
    public bool SetupMenus { get; set; } = true;

    public MerchantSpawnTask(SpawnLocation spawnLocation, DispatchableVehicle vehicleType, DispatchablePerson personType, bool addBlip, bool addOptionalPassengers, bool setPersistent, ISettingsProvideable settings,
        ICrimes crimes, IWeapons weapons, INameProvideable names, IEntityProvideable world, IModItems modItems, IShopMenus shopMenus, GameLocation store)
        : base(spawnLocation, vehicleType, personType, addBlip, addOptionalPassengers, settings, weapons, names, world, modItems)
    {
        Crimes = crimes;
        SetPersistent = setPersistent;
        ShopMenus = shopMenus;
        Store = store;
    }
    public override void AttemptSpawn()
    {
        try
        {
            GameFiber.Yield();
            if (IsInvalidSpawnPosition)
            {
                EntryPoint.WriteToConsole($"CivilianSpawn: Task Invalid Spawn Position");
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
            EntryPoint.SpawnedEntities.Add(createdPed);
            GameFiber.Yield();
            NativeFunction.Natives.SET_MODEL_AS_NO_LONGER_NEEDED(Game.GetHashKey(PersonType.ModelName));
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
            PersonType.SetPedVariation(createdPed, PossibleHeads, true);
            GameFiber.Yield();
            CreatedPeople.Add(Person);
            return Person;
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"CivilianSpawn: ERROR DELETED PERSON {ex.Message} {ex.StackTrace}", 0);
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
    private PedExt SetupMerchantPed(Ped ped)
    {
        if(!ped.Exists())
        {
            return null;
        }
        ped.IsPersistent = true;//THIS IS ON FOR NOW!
        EntryPoint.PersistentPedsCreated++;//TR
        Merchant Vendor = new Merchant(ped, Settings, "Vendor", Crimes, Weapons, World);
        if(!SetupMenus)
        {
            Vendor.SetupMenus = false;
        }
        SpawnedVendors.Add(Vendor);
        World.Pedestrians.AddEntity(Vendor);

        Vendor.AssociatedStore = Store;

        Vendor.SetStats(PersonType, ShopMenus, Weapons, AddBlip, false,false,false,Store);//TASKING IS BROKEN FOR ALL COPS FAR FROM PLAYER AND ALL OTHER PEDS
        if (ped.Exists())
        {
            Vendor.SpawnPosition = ped.Position;
            Vendor.SpawnHeading = ped.Heading;
            //if (SetupMenus)
            //{
                
            //}
            Vendor.SpawnPosition = SpawnLocation.InitialPosition;
            Vendor.WasModSpawned = true;
            Vendor.CanBeAmbientTasked = true;
            Vendor.CanBeTasked = true;
            if (SpawnWithAllWeapons || PersonType.AlwaysHasLongGun)
            {
                Vendor.WeaponInventory.GiveHeavyWeapon();
            }
        }
        GameFiber.Yield();
        if (!ped.Exists())
        {
            return null;
        }
        EntryPoint.WriteToConsole($"SPAWNED WORKED VENDOR AT {Store?.Name}");
        return Vendor;
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