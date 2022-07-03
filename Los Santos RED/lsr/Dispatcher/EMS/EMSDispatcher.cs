using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

public class EMSDispatcher
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
    private uint GameTimeAttemptedDispatch;
    private uint GameTimeAttemptedRecall;
    private bool HasDispatchedThisTick;
    private IWeapons Weapons;
    private INameProvideable Names;
    private SpawnLocation SpawnLocation;
    private Agency Agency;
    private DispatchableVehicle VehicleType;
    private DispatchablePerson PersonType;
    private IPlacesOfInterest PlacesOfInterest;

    public EMSDispatcher(IEntityProvideable world, IDispatchable player, IAgencies agencies, ISettingsProvideable settings, IStreets streets, IZones zones, IJurisdictions jurisdictions, IWeapons weapons, INameProvideable names, IPlacesOfInterest placesOfInterest)
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
    private List<EMT> DeletableOfficers => World.Pedestrians.EMTList.Where(x => (x.RecentlyUpdated && x.DistanceToPlayer >= MinimumDeleteDistance && x.HasBeenSpawnedFor >= MinimumExistingTime) || x.CanRemove).ToList();
    private float DistanceToDelete => Player.IsWanted ? 600f : 800f;
    private float DistanceToDeleteOnFoot => Player.IsWanted ? 125f : 500f;
    private bool HasNeedToDispatch => World.Pedestrians.TotalSpawnedEMTs == 0;
    private bool IsTimeToDispatch => Game.GameTime - GameTimeAttemptedDispatch >= TimeBetweenSpawn;
    private bool IsTimeToRecall => Game.GameTime - GameTimeAttemptedRecall >= TimeBetweenSpawn;
    private float MaxDistanceToSpawn => 650f;
    private float MinDistanceToSpawn => 350f;
    private int TimeBetweenSpawn => 60000;
    public bool Dispatch()
    {
        HasDispatchedThisTick = false;
        if (Settings.SettingsManager.EMSSettings.ManageDispatching && IsTimeToDispatch && HasNeedToDispatch)
        {
            HasDispatchedThisTick = true;
            if (GetSpawnLocation() && GetSpawnTypes(false,null))
            {
                CallSpawnTask(false, true);
            }
            GameTimeAttemptedDispatch = Game.GameTime;
        }
        if (Settings.SettingsManager.EMSSettings.ManageDispatching)
        {
            foreach (Hospital ps in PlacesOfInterest.PossibleLocations.Hospitals.Where(x => x.IsEnabled && x.DistanceToPlayer <= 150f && x.IsNearby && !x.IsDispatchFilled))
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
                            if (GetSpawnTypes(true, ps.AssignedAgency))
                            {
                                CallSpawnTask(true, false);
                                spawnedsome = true;
                            }
                        }
                    }
                    ps.IsDispatchFilled = true;

                    EntryPoint.WriteToConsole($"Police Station: {ps.Name} IsDispatchFilled AnySpawns: {spawnedsome}");
                }
                else
                {
                    ps.IsDispatchFilled = true;
                    EntryPoint.WriteToConsole($"Police Station: {ps.Name} IsDispatchFilled NO SPAWNS");
                }
            }
            foreach (Hospital ps in PlacesOfInterest.PossibleLocations.Hospitals.Where(x => x.IsEnabled && !x.IsNearby && x.IsDispatchFilled))
            {
                ps.IsDispatchFilled = false;
                EntryPoint.WriteToConsole($"Police Station: {ps.Name} DEACTIVATED");
            }
        }
        return HasDispatchedThisTick;
    }
    public void Dispose()
    {

    }
    public void Recall()
    {
        if (Settings.SettingsManager.EMSSettings.ManageDispatching && IsTimeToRecall)
        {
            foreach (EMT emt in DeletableOfficers)
            {
                if (ShouldBeRecalled(emt))
                {
                    Delete(emt);
                    GameFiber.Yield();
                }
            }
            GameTimeAttemptedRecall = Game.GameTime;
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
            VehicleType = Agency.GetRandomVehicle(Player.WantedLevel, false, false, false);
            if (forcePed)
            {
                VehicleType = null;
            }
            if (VehicleType != null)
            {
                string RequiredGroup = "";
                if (VehicleType != null)
                {
                    RequiredGroup = VehicleType.RequiredPedGroup;
                }
                PersonType = Agency.GetRandomPed(World.TotalWantedLevel, RequiredGroup);
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
    private void CallSpawnTask(bool allowAny, bool allowBuddy)
    {
        try
        {
            EMTSpawnTask eMTSpawnTask = new EMTSpawnTask(Agency, SpawnLocation, VehicleType, PersonType, Settings.SettingsManager.EMSSettings.ShowSpawnedBlips, Settings, Weapons, Names, true, World);
            if (allowAny)
            {
                eMTSpawnTask.AllowAnySpawn = true;
            }
            eMTSpawnTask.AttemptSpawn();
            eMTSpawnTask.CreatedPeople.ForEach(x => World.Pedestrians.AddEntity(x));
            eMTSpawnTask.CreatedVehicles.ForEach(x => World.Vehicles.AddEntity(x, ResponseType.EMS));
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"DISPATCHER: Spawn EMS ERROR {ex.Message} : {ex.StackTrace}", 0);
        }
    }
    private bool ShouldBeRecalled(EMT emt)
    {
        if(emt.IsInVehicle)
        {
            return emt.DistanceToPlayer >= DistanceToDelete;
        }
        else
        {
            return emt.DistanceToPlayer >= DistanceToDeleteOnFoot;
        }
    }
    private void Delete(PedExt emt)
    {
        if (emt != null && emt.Pedestrian.Exists())
        {
            //EntryPoint.WriteToConsole($"Attempting to Delete {Cop.Pedestrian.Handle}");
            if (emt.Pedestrian.IsInAnyVehicle(false))
            {
                if (emt.Pedestrian.CurrentVehicle.HasPassengers)
                {
                    foreach (Ped Passenger in emt.Pedestrian.CurrentVehicle.Passengers)
                    {
                        RemoveBlip(Passenger);
                        Passenger.Delete();
                        EntryPoint.PersistentPedsDeleted++;
                    }
                }
                if (emt.Pedestrian.Exists() && emt.Pedestrian.CurrentVehicle.Exists() && emt.Pedestrian.CurrentVehicle != null)
                {
                    emt.Pedestrian.CurrentVehicle.Delete();
                    EntryPoint.PersistentVehiclesDeleted++;
                }
            }
            RemoveBlip(emt.Pedestrian);
            if (emt.Pedestrian.Exists())
            {
                //EntryPoint.WriteToConsole(string.Format("Delete Cop Handle: {0}, {1}, {2}", Cop.Pedestrian.Handle, Cop.DistanceToPlayer, Cop.AssignedAgency.Initials));
                emt.Pedestrian.Delete();
                EntryPoint.PersistentPedsDeleted++;
            }
        }
    }
    private void RemoveBlip(Ped emt)
    {
        if (!emt.Exists())
        {
            return;
        }
        Blip MyBlip = emt.GetAttachedBlip();
        if (MyBlip.Exists())
        {
            MyBlip.Delete();
        }
    }
    private List<Agency> GetAgencies(Vector3 Position, int WantedLevel)
    {
        List<Agency> ToReturn = new List<Agency>();
        Zone CurrentZone = Zones.GetZone(Position);
        Agency ZoneAgency = Jurisdictions.GetRandomAgency(CurrentZone.InternalGameName, WantedLevel, ResponseType.EMS);
        if (ZoneAgency != null)
        {
            ToReturn.Add(ZoneAgency); //Zone Jurisdiciton Random
        }
        if (!ToReturn.Any() || RandomItems.RandomPercent(LikelyHoodOfAnySpawn))//fall back to anybody
        {
            ToReturn.AddRange(Agencies.GetSpawnableAgencies(WantedLevel, ResponseType.EMS));
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
    public void DebugSpawnEMT(string agencyID, bool onFoot)
    {
        SpawnLocation = new SpawnLocation();
        SpawnLocation.InitialPosition = Game.LocalPlayer.Character.GetOffsetPositionFront(10f);
        SpawnLocation.StreetPosition = SpawnLocation.InitialPosition;
        if (agencyID == "")
        {
            Agency = Agencies.GetRandomAgency(ResponseType.EMS);
        }
        else
        {
            Agency = Agencies.GetAgency(agencyID);
        }
        if(Agency == null)
        {
            return;
        }
        if (!onFoot)
        {
            VehicleType = Agency.GetRandomVehicle(World.TotalWantedLevel, false, false, true);
        }
        if (VehicleType != null || onFoot)
        {
            string RequiredGroup = "";
            if (VehicleType != null)
            {
                RequiredGroup = VehicleType.RequiredPedGroup;
            }
            PersonType = Agency.GetRandomPed(World.TotalWantedLevel, RequiredGroup);
        }
        CallSpawnTask(true, false);
    }

}