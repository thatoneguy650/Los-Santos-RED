using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;

public class DefaultDispatcher
{
    protected readonly IAgencies Agencies;
    protected readonly IDispatchable Player;
    protected readonly ISettingsProvideable Settings;
    protected readonly IStreets Streets;
    protected readonly IEntityProvideable World;
    protected readonly IJurisdictions Jurisdictions;
    protected readonly IZones Zones;
    protected readonly IWeapons Weapons;
    protected readonly INameProvideable Names;
    protected readonly IPlacesOfInterest PlacesOfInterest;
    protected readonly ICrimes Crimes;
    protected readonly IModItems ModItems;
    protected readonly IShopMenus ShopMenus;
    protected int TimesToTryLocation = 3;



    protected SpawnLocation SpawnLocation;
    protected DispatchableVehicle VehicleType;
    protected DispatchablePerson PersonType;

    protected virtual float MinDistanceToSpawnOnDemand => 150f;
    protected virtual float MaxDistanceToSpawnOnDemand => 300f;
    protected virtual float MinDistanceToSpawn => 175f;
    protected virtual float MaxDistanceToSpawn => 650f;
    protected virtual float DistanceToDeleteInVehicle => MaxDistanceToSpawn + 150f;// 300f;
    protected virtual float DistanceToDeleteOnFoot => MaxDistanceToSpawn + 50f;// 200 + 50f grace = 250f;
    public DefaultDispatcher(IEntityProvideable world, IDispatchable player, IAgencies agencies, ISettingsProvideable settings, IStreets streets, IZones zones, IJurisdictions jurisdictions, IWeapons weapons, INameProvideable names,
        IPlacesOfInterest placesOfInterest, ICrimes crimes, IModItems modItems, IShopMenus shopMenus)
    {
        Player = player;
        World = world;
        Agencies = agencies;
        Settings = settings;
        Streets = streets;
        Zones = zones;
        Jurisdictions = jurisdictions;
        Weapons = weapons;
        Names = names;
        PlacesOfInterest = placesOfInterest;
        ShopMenus = shopMenus;
        ModItems = modItems;
        Crimes = crimes;
    }
    public virtual void Dispatch()
    {
        if(!DetermineRun())
        {
            return;
        }
        if(!GetSpawnLocation())
        {
            return;
        }
        if(!GetSpawnTypes())
        {
            return;
        }
        CallSpawnTask();
    }


    public virtual void Recall()
    {
        if (!DetermineRecall())
        {
            return;
        }
    }
    protected virtual bool DetermineRun()
    {
        return false;
    }
    protected bool GetSpawnLocation()
    {
        int timesTried = 0;
        bool isValidSpawn;
        SpawnLocation = new SpawnLocation();
        do
        {
            SpawnLocation.InitialPosition = GetSpawnPosition();
            SpawnLocation.GetClosestStreet(false);
            SpawnLocation.GetClosestSidewalk();
            GameFiber.Yield();
            isValidSpawn = IsValidSpawn(SpawnLocation);
            timesTried++;
            EntryPoint.WriteToConsole($"ATTEMPTING TAXI DISPATCH LOCATION timesTried{timesTried} TimesToTryLocation{TimesToTryLocation}");
            GameFiber.Yield();
        }
        while ((!SpawnLocation.HasSpawns || !isValidSpawn) && timesTried < TimesToTryLocation);//2//10
        return isValidSpawn && SpawnLocation.HasSpawns;
    }

    protected bool GetCloseSpawnLocation()
    {
        int timesTried = 0;
        bool isValidSpawn;
        SpawnLocation = new SpawnLocation();
        do
        {
            SpawnLocation.InitialPosition = GetCloseSpawnPosition();
            SpawnLocation.GetClosestStreet(false);
            SpawnLocation.GetClosestSidewalk();
            GameFiber.Yield();
            isValidSpawn = IsValidCloseSpawn(SpawnLocation);
            timesTried++;
            EntryPoint.WriteToConsole($"ATTEMPTING TAXI DISPATCH LOCATION CLOSE timesTried{timesTried} TimesToTryLocation{TimesToTryLocation}");
            GameFiber.Yield();
        }
        while ((!SpawnLocation.HasSpawns || !isValidSpawn) && timesTried < TimesToTryLocation);//2//10
        return isValidSpawn && SpawnLocation.HasSpawns;
    }

    protected virtual Vector3 GetSpawnPosition()
    {
        Vector3 Position;
        if (Player.IsWanted && Player.IsInVehicle)
        {
            Position = Player.Character.GetOffsetPositionFront(250f);
        }
        else
        {
            Position = Player.Position;
        }
        Position = Position.Around2D(MinDistanceToSpawn, MaxDistanceToSpawn);
        return Position;
    }
    protected virtual Vector3 GetCloseSpawnPosition()
    {
        Vector3 Position = Player.Position;
      
        Position = Position.Around2D(MinDistanceToSpawnOnDemand, MaxDistanceToSpawnOnDemand);
        return Position;
    }

    protected virtual bool IsValidCloseSpawn(SpawnLocation spawnLocation)
    {
        if (spawnLocation.StreetPosition.DistanceTo2D(Player.Position) < MinDistanceToSpawnOnDemand)
        {
            EntryPoint.WriteToConsole($"NOT VALID SPAWN 1 {MinDistanceToSpawn} {spawnLocation.StreetPosition.DistanceTo2D(Player.Position)}");
            return false;
        }
        else if (spawnLocation.InitialPosition.DistanceTo2D(Player.Position) < MinDistanceToSpawnOnDemand)
        {
            EntryPoint.WriteToConsole($"NOT VALID SPAWN 2 {MinDistanceToSpawn} {spawnLocation.InitialPosition.DistanceTo2D(Player.Position)}");
            return false;
        }
        return true;
    }
    protected virtual bool IsValidSpawn(SpawnLocation spawnLocation)
    {
        if (spawnLocation.StreetPosition.DistanceTo2D(Player.Position) < MinDistanceToSpawn)
        {
            EntryPoint.WriteToConsole($"NOT VALID SPAWN 1 {MinDistanceToSpawn} {spawnLocation.StreetPosition.DistanceTo2D(Player.Position)}");
            return false;
        }
        else if (spawnLocation.InitialPosition.DistanceTo2D(Player.Position) < MinDistanceToSpawn)
        {
            EntryPoint.WriteToConsole($"NOT VALID SPAWN 2 {MinDistanceToSpawn} {spawnLocation.InitialPosition.DistanceTo2D(Player.Position)}");
            return false;
        }
        return true;
    }


    protected virtual bool GetSpawnTypes()
    {
        return false;
    }
    protected virtual bool CallSpawnTask()
    {
        return false;
    }
    protected virtual bool DetermineRecall()
    {
        return false;
    }
    protected virtual void RemoveBlip(Ped ped)
    {
        if (!ped.Exists())
        {
            return;
        }
        Blip MyBlip = ped.GetAttachedBlip();
        if (MyBlip.Exists())
        {
            MyBlip.Delete();
        }
    }
    protected virtual void Delete(PedExt pedExt)
    {
        if (pedExt != null && pedExt.Pedestrian.Exists())
        {
            //EntryPoint.WriteToConsole($"Attempting to Delete {Cop.Pedestrian.Handle}");
            if (pedExt.Pedestrian.IsInAnyVehicle(false))
            {
                if (pedExt.Pedestrian.CurrentVehicle.HasPassengers)
                {
                    foreach (Ped Passenger in pedExt.Pedestrian.CurrentVehicle.Passengers)
                    {
                        if (Passenger.Handle != Game.LocalPlayer.Character.Handle)
                        {
                            RemoveBlip(Passenger);
                            Passenger.Delete();
                            EntryPoint.PersistentPedsDeleted++;
                        }
                    }
                }
                if (pedExt.Pedestrian.Exists() && pedExt.Pedestrian.CurrentVehicle.Exists() && pedExt.Pedestrian.CurrentVehicle != null)
                {
                    Blip carBlip = pedExt.Pedestrian.CurrentVehicle.GetAttachedBlip();
                    if (carBlip.Exists())
                    {
                        carBlip.Delete();
                    }
                    VehicleExt vehicleExt = World.Vehicles.GetVehicleExt(pedExt.Pedestrian.CurrentVehicle);
                    if (vehicleExt != null)
                    {
                        vehicleExt.FullyDelete();
                    }
                    else
                    {
                        pedExt.Pedestrian.CurrentVehicle.Delete();
                    }
                    EntryPoint.PersistentVehiclesDeleted++;
                }
            }
            RemoveBlip(pedExt.Pedestrian);
            if (pedExt.Pedestrian.Exists())
            {
                //EntryPoint.WriteToConsole(string.Format("Delete Cop Handle: {0}, {1}, {2}", Cop.Pedestrian.Handle, Cop.DistanceToPlayer, Cop.AssignedAgency.Initials));
                pedExt.Pedestrian.Delete();
                EntryPoint.PersistentPedsDeleted++;
            }
        }
    }
    protected virtual bool ShouldBeRecalled(PedExt pedExt)
    {
        if (!pedExt.RecentlyUpdated)
        {
            return false;
        }
        else if (pedExt.IsManuallyDeleted)
        {
            return false;
        }
        else if (pedExt.IsInVehicle)
        {
            return pedExt.DistanceToPlayer >= DistanceToDeleteInVehicle;
        }
        else
        {
            return pedExt.DistanceToPlayer >= DistanceToDeleteOnFoot;
        }
    }
}