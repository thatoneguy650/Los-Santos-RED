using LosSantosRED.lsr.Interface;
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




    protected SpawnLocation SpawnLocation;
    protected DispatchableVehicle VehicleType;
    protected DispatchablePerson PersonType;

    protected virtual float MinDistanceToSpawn => 250f;
    protected virtual float MaxDistanceToSpawn => 900f;

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
            SpawnLocation.GetClosestStreet(Player.IsWanted);
            SpawnLocation.GetClosestSidewalk();
            GameFiber.Yield();
            isValidSpawn = IsValidSpawn(SpawnLocation);
            timesTried++;
            GameFiber.Yield();
        }
        while (!SpawnLocation.HasSpawns && !isValidSpawn && timesTried < 3);//2//10
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


    protected virtual bool IsValidSpawn(SpawnLocation spawnLocation)
    {
        if (spawnLocation.StreetPosition.DistanceTo2D(Player.Position) < 250f)
        {
            return false;
        }
        else if (spawnLocation.InitialPosition.DistanceTo2D(Player.Position) < 250f)
        {
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
}