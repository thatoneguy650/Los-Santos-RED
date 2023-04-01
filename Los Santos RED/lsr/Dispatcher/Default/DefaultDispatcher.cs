using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime;

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





    protected SpawnLocation SpawnLocation;
    protected DispatchableVehicle VehicleType;
    protected DispatchablePerson PersonType;

    protected virtual float MinDistanceToSpawn => 250f;
    protected virtual float MaxDistanceToSpawn => 900f;

    public DefaultDispatcher(IEntityProvideable world, IDispatchable player, IAgencies agencies, ISettingsProvideable settings, IStreets streets, IZones zones, IJurisdictions jurisdictions, IWeapons weapons, INameProvideable names, IPlacesOfInterest placesOfInterest)
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


    protected bool IsValidSpawn(SpawnLocation spawnLocation)
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


    protected bool GetSpawnTypes()
    {
        return false;
    }
    protected void CallSpawnTask()
    {

    }
    protected virtual bool DetermineRecall()
    {
        return false;
    }
}