using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ArrayExtensions;
using System.Security.Cryptography.X509Certificates;

public class TaxiSpawnTask : CivilianSpawnTask
{
    private TaxiFirm TaxiFirm;
    public TaxiSpawnTask(SpawnLocation spawnLocation, DispatchableVehicle vehicleType, DispatchablePerson personType, bool addBlip, bool addOptionalPassengers, bool setPersistent, ISettingsProvideable settings, ICrimes crimes, 
        IWeapons weapons, INameProvideable names, IEntityProvideable world, IModItems modItems, IShopMenus shopMenus, TaxiFirm taxiFirm) : base(spawnLocation, vehicleType, personType, addBlip, addOptionalPassengers, setPersistent, settings, crimes, weapons, names, world, modItems, shopMenus)
    {
        TaxiFirm = taxiFirm;
    }
    protected override PedExt SetupRegularPed(Ped ped)
    {
        ped.IsPersistent = SetPersistent;
        EntryPoint.PersistentPedsCreated++;//TR
        bool isMale = PersonType.IsMale(ped);

        string groupName = "Taxi Driver";
        if(TaxiFirm != null && TaxiFirm.IsRideShare)
        {
            groupName = "Rideshare Driver";
        }

        TaxiDriver CreatedPedExt = new TaxiDriver(ped, Settings, Crimes, Weapons, Names.GetRandomName(isMale), groupName, World, true);
        CreatedPedExt.TaxiFirm = TaxiFirm;
        ped.RelationshipGroup = isMale ? new RelationshipGroup("CIVMALE") : new RelationshipGroup("CIVFEMALE");
        World.Pedestrians.AddEntity(CreatedPedExt);
        CreatedPedExt.SetBaseStats(PersonType, ShopMenus, Weapons, AddBlip);


        if(TaxiFirm != null)
        {
            TaxiFirm.SetPedStats(CreatedPedExt);
            CreatedPedExt.WeaponInventory.IssueWeapons(Weapons, RandomItems.RandomPercent(TaxiFirm.PercentageWithMelee), RandomItems.RandomPercent(TaxiFirm.PercentageWithSidearms), RandomItems.RandomPercent(TaxiFirm.PercentageWithLongGuns), PersonType);
        }


        if (ped.Exists())
        {
            CreatedPedExt.SpawnPosition = ped.Position;
            CreatedPedExt.SpawnHeading = ped.Heading;
        }
        EntryPoint.WriteToConsole("TAXI SPAWN MADE IT TO SETUP REGULAR PED");
        return CreatedPedExt;
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
            TaxiVehicleExt CreatedVehicle = new TaxiVehicleExt(SpawnedVehicle, Settings);
            CreatedVehicle.TaxiFirm = TaxiFirm;
            CreatedVehicle.Setup();
            CreatedVehicle.WasModSpawned = true;
            CreatedVehicle.AddVehicleToList(World);


            GameFiber.Yield();
            if (!SpawnedVehicle.Exists())
            {
                return null;
            }
            VehicleType.SetVehicleExtPermanentStats(CreatedVehicle, SetPersistent);
            CreatedVehicles.Add(CreatedVehicle);
            return CreatedVehicle;
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"TaxiSpawn: ERROR DELETED VEHICLE {ex.Message} {ex.StackTrace} ATTEMPTING {VehicleType.ModelName}", 0);
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
}

