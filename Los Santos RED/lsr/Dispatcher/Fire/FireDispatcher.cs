using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

public class FireDispatcher
{
    private readonly IAgencies Agencies;
    private readonly IDispatchable Player;
    private readonly int LikelyHoodOfAnySpawn = 5;
    private readonly float MinimumDeleteDistance = 150f;//200f
    private readonly uint MinimumExistingTime = 20000;
    private readonly ISettingsProvideable Settings;
    private readonly IStreets Streets;
    private readonly IEntityProvideable World;
    private readonly IJurisdictions Jurisdictions;
    private readonly IZones Zones;
    private bool HasDispatchedThisTick;
    private uint GameTimeAttemptedDispatch;
    private uint GameTimeAttemptedRecall;
    private IWeapons Weapons;
    private INameProvideable Names;
    private SpawnLocation SpawnLocation;
    private Agency Agency;
    private DispatchableVehicle VehicleType;
    private DispatchablePerson PersonType;
    private IPlacesOfInterest PlacesOfInterest;
    public FireDispatcher(IEntityProvideable world, IDispatchable player, IAgencies agencies, ISettingsProvideable settings, IStreets streets, IZones zones, IJurisdictions jurisdictions, IWeapons weapons, INameProvideable names, IPlacesOfInterest placesOfInterest)
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
    private float ClosestOfficerSpawnToPlayerAllowed => Player.IsWanted ? 150f : 250f;
    private List<Firefighter> DeletableOfficers => World.Pedestrians.FirefighterList.Where(x => (x.RecentlyUpdated && x.DistanceToPlayer >= MinimumDeleteDistance && x.HasBeenSpawnedFor >= MinimumExistingTime) || x.CanRemove).ToList();
    private float DistanceToDelete => Player.IsWanted ? 600f : 1000f;
    private float DistanceToDeleteOnFoot => Player.IsWanted ? 125f : 1000f;
    private bool HasNeedToDispatch => World.Pedestrians.TotalSpawnedFirefighters == 0;
    private bool IsTimeToDispatch => Game.GameTime - GameTimeAttemptedDispatch >= 60000;
    private bool IsTimeToRecall => Game.GameTime - GameTimeAttemptedRecall >= TimeBetweenSpawn;
    private float MaxDistanceToSpawn => 900f;
    private float MinDistanceToSpawn => 350f;
    private int TimeBetweenSpawn => 60000;
    public bool Dispatch()
    {
        HasDispatchedThisTick = false;
        if (Settings.SettingsManager.FireSettings.ManageDispatching)
        {
            HandleAmbientSpawns();
            HandleStationSpawns();
        }
        return HasDispatchedThisTick;
    }

    public void LocationDispatch()
    {
        if (Settings.SettingsManager.FireSettings.ManageDispatching)
        {
            HandleStationSpawns();
        }
    }

    public void Dispose()
    {

    }
    public void Recall()
    {
        if (Settings.SettingsManager.FireSettings.ManageDispatching && IsTimeToRecall)
        {
            foreach (Firefighter ff in DeletableOfficers)
            {
                if (ShouldBeRecalled(ff))
                {
                    Delete(ff);
                    GameFiber.Yield();
                }
            }
            GameTimeAttemptedRecall = Game.GameTime;
        }
    }
    private void HandleAmbientSpawns()
    {
        if (Settings.SettingsManager.FireSettings.ManageDispatching && IsTimeToDispatch && HasNeedToDispatch)
        {
            HasDispatchedThisTick = true;//up here for now, might be better down low
            if (GetSpawnLocation() && GetSpawnTypes(false, null))
            {
                CallSpawnTask(false, true);
            }
            GameTimeAttemptedDispatch = Game.GameTime;
        }
    }
    private void HandleStationSpawns()
    {
        if (Settings.SettingsManager.FireSettings.ManageDispatching)
        {
            foreach (FireStation ps in PlacesOfInterest.PossibleLocations.FireStations.Where(x => x.IsEnabled && x.DistanceToPlayer <= 150f && x.IsNearby && !x.IsDispatchFilled))
            {
                if (ps.PossiblePedSpawns != null)
                {
                    bool spawnedsome = false;
                    foreach (ConditionalLocation cl in ps.PossiblePedSpawns)
                    {
                        if (RandomItems.RandomPercent(cl.Percentage))
                        {
                            HasDispatchedThisTick = true;
                            SpawnLocation = new SpawnLocation(cl.Location);
                            SpawnLocation.Heading = cl.Heading;
                            SpawnLocation.StreetPosition = cl.Location;
                            SpawnLocation.SidewalkPosition = cl.Location;
                            Agency toSpawn = ps.AssignedAgency;
                            if (toSpawn == null)
                            {
                                Zone CurrentZone = Zones.GetZone(cl.Location);
                                Agency ZoneAgency = Jurisdictions.GetMainAgency(CurrentZone.InternalGameName, ResponseType.LawEnforcement);
                                if (ZoneAgency != null)
                                {
                                    toSpawn = ZoneAgency;
                                }
                            }
                            if (GetSpawnTypes(true, toSpawn))
                            {
                                CallSpawnTask(true, false);
                                spawnedsome = true;
                            }
                        }
                    }
                    ps.IsDispatchFilled = true;
                }
                else
                {
                    ps.IsDispatchFilled = true;
                }
            }
            foreach (FireStation ps in PlacesOfInterest.PossibleLocations.FireStations.Where(x => x.IsEnabled && !x.IsNearby && x.IsDispatchFilled))
            {
                ps.IsDispatchFilled = false;
            }
        }
    }
    private void CallSpawnTask(bool allowAny, bool allowBuddy)
    {
        try
        {
            FireFighterSpawnTask fireFighterSpawnTask = new FireFighterSpawnTask(Agency, SpawnLocation, VehicleType, PersonType, Settings.SettingsManager.FireSettings.ShowSpawnedBlips, Settings, Weapons, Names, true, World);
            fireFighterSpawnTask.AllowAnySpawn = allowAny;
            fireFighterSpawnTask.AllowBuddySpawn = allowBuddy;
            fireFighterSpawnTask.AttemptSpawn();
            fireFighterSpawnTask.CreatedPeople.ForEach(x => World.Pedestrians.AddEntity(x));
            fireFighterSpawnTask.CreatedVehicles.ForEach(x => World.Vehicles.AddEntity(x, ResponseType.Fire));
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"EMS Dispatcher Spawn Error: {ex.Message} : {ex.StackTrace}", 0);
        }
    }

    private bool GetSpawnLocation()
    {
        int timesTried = 0;
        bool isValidSpawn;
        SpawnLocation = new SpawnLocation();
        do
        {
            SpawnLocation.InitialPosition = GetPositionAroundPlayer();
            SpawnLocation.GetClosestStreet();
            isValidSpawn = IsValidSpawn(SpawnLocation);
            timesTried++;
        }
        while (!SpawnLocation.HasSpawns && !isValidSpawn && timesTried < 2);//10
        return isValidSpawn && SpawnLocation.HasSpawns;
    }
    private bool GetSpawnTypes(bool forcePed, Agency forceAgency)
    {
        Agency = null;
        VehicleType = null;
        PersonType = null;
        if (forceAgency != null)
        {
            Agency = forceAgency;
        }
        else
        {
            Agency = GetRandomAgency(SpawnLocation);
        }
        if (Agency != null)
        {
            if (!forcePed)
            {
                VehicleType = Agency.GetRandomVehicle(Player.WantedLevel, false, false, false);
            }
            if (VehicleType != null)
            {
                string RequiredGroup = "";
                if (VehicleType != null)
                {
                    RequiredGroup = VehicleType.RequiredPedGroup;
                }
                PersonType = Agency.GetRandomPed(Player.WantedLevel, RequiredGroup);
                if (PersonType != null)
                {
                    return true;
                }
            }
            else if (forcePed)
            {
                PersonType = Agency.GetRandomPed(World.TotalWantedLevel, "");
                if (PersonType != null)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private bool ShouldBeRecalled(Firefighter ff)
    {
        if (ff.IsInVehicle)
        {
            return ff.DistanceToPlayer >= DistanceToDelete;
        }
        else
        {
            return ff.DistanceToPlayer >= DistanceToDeleteOnFoot;
        }
    }
    private void Delete(PedExt ff)
    {
        if (ff != null && ff.Pedestrian.Exists())
        {
            //EntryPoint.WriteToConsole($"Attempting to Delete {Cop.Pedestrian.Handle}");
            if (ff.Pedestrian.IsInAnyVehicle(false))
            {
                if (ff.Pedestrian.CurrentVehicle.HasPassengers)
                {
                    foreach (Ped Passenger in ff.Pedestrian.CurrentVehicle.Passengers)
                    {
                        RemoveBlip(Passenger);
                        Passenger.Delete();
                        EntryPoint.PersistentPedsDeleted++;
                    }
                }
                if (ff.Pedestrian.Exists() && ff.Pedestrian.CurrentVehicle.Exists() && ff.Pedestrian.CurrentVehicle != null)
                {
                    ff.Pedestrian.CurrentVehicle.Delete();
                    EntryPoint.PersistentVehiclesDeleted++;
                }
            }
            RemoveBlip(ff.Pedestrian);
            if (ff.Pedestrian.Exists())
            {
                //EntryPoint.WriteToConsole(string.Format("Delete Cop Handle: {0}, {1}, {2}", Cop.Pedestrian.Handle, Cop.DistanceToPlayer, Cop.AssignedAgency.Initials));
                ff.Pedestrian.Delete();
                EntryPoint.PersistentPedsDeleted++;
            }
        }
    }
    private void RemoveBlip(Ped ff)
    {
        if (!ff.Exists())
        {
            return;
        }
        Blip MyBlip = ff.GetAttachedBlip();
        if (MyBlip.Exists())
        {
            MyBlip.Delete();
        }
    }
    private List<Agency> GetAgencies(Vector3 Position, int WantedLevel)
    {
        List<Agency> ToReturn = new List<Agency>();
        Zone CurrentZone = Zones.GetZone(Position);
        Agency ZoneAgency = Jurisdictions.GetRandomAgency(CurrentZone.InternalGameName, WantedLevel, ResponseType.Fire);
        if (ZoneAgency != null)
        {
            ToReturn.Add(ZoneAgency); //Zone Jurisdiciton Random
        }
        if (!ToReturn.Any() || RandomItems.RandomPercent(LikelyHoodOfAnySpawn))//fall back to anybody
        {
            ToReturn.AddRange(Agencies.GetSpawnableAgencies(WantedLevel, ResponseType.Fire));
        }
        foreach (Agency ag in ToReturn)
        {
            //EntryPoint.WriteToConsole(string.Format("Debugging: Agencies At Pos: {0}", ag.Initials));
        }
        return ToReturn;
    }
    private Vector3 GetPositionAroundPlayer()
    {
        Vector3 Position;
        if (Player.IsInVehicle)
        {
            Position = Player.Character.GetOffsetPositionFront(250f);//350f
        }
        else
        {
            Position = Player.Position;
        }
        Position = Position.Around2D(MinDistanceToSpawn, MaxDistanceToSpawn);
        return Position;
    }
    private Agency GetRandomAgency(SpawnLocation spawnLocation)
    {
        Agency agency;
        List<Agency> PossibleAgencies = GetAgencies(spawnLocation.StreetPosition, Player.WantedLevel);
        agency = PossibleAgencies.PickRandom();
        if (agency == null)
        {
            agency = GetAgencies(spawnLocation.InitialPosition, Player.WantedLevel).PickRandom();
        }
        if (agency == null)
        {
            //EntryPoint.WriteToConsole("Dispatcher could not find Agency To Spawn");
        }
        return agency;
    }
    private bool IsValidSpawn(SpawnLocation spawnLocation)
    {
        if (spawnLocation.StreetPosition.DistanceTo2D(Player.Position) < ClosestOfficerSpawnToPlayerAllowed)
        {
            return false;
        }
        else if (spawnLocation.InitialPosition.DistanceTo2D(Player.Position) < ClosestOfficerSpawnToPlayerAllowed)
        {
            return false;
        }
        return true;
    }
}