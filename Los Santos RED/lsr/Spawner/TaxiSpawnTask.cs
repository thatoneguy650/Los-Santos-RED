using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TaxiSpawnTask : CivilianSpawnTask
{
    public TaxiSpawnTask(SpawnLocation spawnLocation, DispatchableVehicle vehicleType, DispatchablePerson personType, bool addBlip, bool addOptionalPassengers, bool setPersistent, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, INameProvideable names, IEntityProvideable world, IModItems modItems, IShopMenus shopMenus) : base(spawnLocation, vehicleType, personType, addBlip, addOptionalPassengers, setPersistent, settings, crimes, weapons, names, world, modItems, shopMenus)
    {

    }
    protected override PedExt SetupRegularPed(Ped ped)
    {
        ped.IsPersistent = SetPersistent;
        EntryPoint.PersistentPedsCreated++;//TR
        bool isMale = PersonType.IsMale(ped);

        TaxiDriver CreatedPedExt = new TaxiDriver(ped, Settings, Crimes, Weapons, Names.GetRandomName(isMale), "Taxi Driver", World, true);
        ped.RelationshipGroup = isMale ? new RelationshipGroup("CIVMALE") : new RelationshipGroup("CIVFEMALE");
        World.Pedestrians.AddEntity(CreatedPedExt);
        CreatedPedExt.SetBaseStats(PersonType, ShopMenus, Weapons, AddBlip);
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
            TaxiVehicleExt CreatedVehicle = new TaxiVehicleExt(SpawnedVehicle, Settings);
            CreatedVehicle.Setup();
            CreatedVehicle.WasModSpawned = true;
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
            EntryPoint.WriteToConsole($"CivilianSpawn: ERROR DELETED VEHICLE {ex.Message} {ex.StackTrace} ATTEMPTING {VehicleType.ModelName}", 0);
            if (SpawnedVehicle.Exists())
            {
                SpawnedVehicle.Delete();
            }
            GameFiber.Yield();
            return null;
        }
    }
}

