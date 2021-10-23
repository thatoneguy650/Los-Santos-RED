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
    public FireDispatcher(IEntityProvideable world, IDispatchable player, IAgencies agencies, ISettingsProvideable settings, IStreets streets, IZones zones, IJurisdictions jurisdictions)
    {
        Player = player;
        World = world;
        Agencies = agencies;
        Settings = settings;
        Streets = streets;
        Zones = zones;
        Jurisdictions = jurisdictions;
    }
    private float ClosestOfficerSpawnToPlayerAllowed => Player.IsWanted ? 150f : 250f;
    private List<Firefighter> DeletableOfficers => World.FirefighterList.Where(x => x.RecentlyUpdated && x.DistanceToPlayer >= MinimumDeleteDistance && x.HasBeenSpawnedFor >= MinimumExistingTime).ToList();
    private float DistanceToDelete => Player.IsWanted ? 600f : 1000f;
    private float DistanceToDeleteOnFoot => Player.IsWanted ? 125f : 1000f;
    private bool HasNeedToDispatch => World.TotalSpawnedFirefighters == 0;
    private bool IsTimeToDispatch => Game.GameTime - GameTimeAttemptedDispatch >= 60000;
    private bool IsTimeToRecall => Game.GameTime - GameTimeAttemptedRecall >= TimeBetweenSpawn;
    private float MaxDistanceToSpawn => 900f;
    private float MinDistanceToSpawn => 350f;
    private int TimeBetweenSpawn => 60000;
    public bool Dispatch()
    {
        HasDispatchedThisTick = false;
        if (Settings.SettingsManager.FireSettings.ManageDispatching && IsTimeToDispatch && HasNeedToDispatch)
        {
            EntryPoint.WriteToConsole($"DISPATCHER: Attempting Fire Spawn", 3);
            int timesTried = 0;
            bool isValidSpawn = false;
            SpawnLocation spawnLocation = new SpawnLocation();
            do
            {
                spawnLocation.InitialPosition = GetPositionAroundPlayer();
                spawnLocation.GetClosestStreet();
                isValidSpawn = IsValidSpawn(spawnLocation);
                timesTried++;
            }
            while (!spawnLocation.HasSpawns && !isValidSpawn && timesTried < 2);//10
            if (spawnLocation.HasSpawns && isValidSpawn)
            {
                Agency agency = GetRandomAgency(spawnLocation);
                DispatchableVehicle VehicleType = agency.GetRandomVehicle(Player.WantedLevel, false, false, false);
                if (VehicleType != null)
                {
                    DispatchablePerson PersonType = agency.GetRandomPed(Player.WantedLevel, VehicleType.RequiredPassengerModels);
                    if (PersonType != null)
                    {
                        try
                        {
                            SpawnTask spawnTask = new SpawnTask(agency, spawnLocation.InitialPosition, spawnLocation.StreetPosition, spawnLocation.Heading, VehicleType, PersonType, Settings.SettingsManager.PoliceSettings.ShowSpawnedBlips, Settings);
                            spawnTask.AttemptSpawn();
                            spawnTask.CreatedPeople.ForEach(x => World.AddEntity(x));
                            spawnTask.CreatedVehicles.ForEach(x => World.AddEntity(x));
                            HasDispatchedThisTick = true;
                        }
                        catch (Exception ex)
                        {
                            EntryPoint.WriteToConsole($"DISPATCHER: Spawn Fire ERROR {ex.Message} : {ex.StackTrace}", 0);
                        }
                    }
                }
            }
            else
            {
                EntryPoint.WriteToConsole($"DISPATCHER: Attempting to Spawn Fire Failed, Has Spawns {spawnLocation.HasSpawns} Is Valid {isValidSpawn}", 5);
            }
            GameTimeAttemptedDispatch = Game.GameTime;
        }
        return HasDispatchedThisTick;
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
                    }
                }
                if (ff.Pedestrian.Exists() && ff.Pedestrian.CurrentVehicle.Exists() && ff.Pedestrian.CurrentVehicle != null)
                {
                    ff.Pedestrian.CurrentVehicle.Delete();
                }
            }
            RemoveBlip(ff.Pedestrian);
            if (ff.Pedestrian.Exists())
            {
                //EntryPoint.WriteToConsole(string.Format("Delete Cop Handle: {0}, {1}, {2}", Cop.Pedestrian.Handle, Cop.DistanceToPlayer, Cop.AssignedAgency.Initials));
                ff.Pedestrian.Delete();
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