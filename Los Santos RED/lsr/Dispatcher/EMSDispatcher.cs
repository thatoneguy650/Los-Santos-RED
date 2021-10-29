﻿using ExtensionsMethods;
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
    public EMSDispatcher(IEntityProvideable world, IDispatchable player, IAgencies agencies, ISettingsProvideable settings, IStreets streets, IZones zones, IJurisdictions jurisdictions, IWeapons weapons)
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
    private List<EMT> DeletableOfficers => World.EMTList.Where(x => x.RecentlyUpdated && x.DistanceToPlayer >= MinimumDeleteDistance && x.HasBeenSpawnedFor >= MinimumExistingTime).ToList();
    private float DistanceToDelete => Player.IsWanted ? 600f : 1000f;
    private float DistanceToDeleteOnFoot => Player.IsWanted ? 125f : 1000f;
    private bool HasNeedToDispatch => World.TotalSpawnedEMTs == 0;
    private bool IsTimeToDispatch => Game.GameTime - GameTimeAttemptedDispatch >= 60000;
    private bool IsTimeToRecall => Game.GameTime - GameTimeAttemptedRecall >= TimeBetweenSpawn;
    private float MaxDistanceToSpawn => 900f;
    private float MinDistanceToSpawn => 350f;
    private int TimeBetweenSpawn => 60000;
    public bool Dispatch()
    {
        HasDispatchedThisTick = false;
        if (Settings.SettingsManager.EMSSettings.ManageDispatching && IsTimeToDispatch && HasNeedToDispatch)
        {
            HasDispatchedThisTick = true;//up here for now, might be better down low
            EntryPoint.WriteToConsole($"DISPATCHER: Attempting EMS Spawn", 3);
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
                if (agency != null)
                {
                    EntryPoint.WriteToConsole($"DISPATCHER: Attempting EMS Spawn for {agency.ID}", 3);
                    DispatchableVehicle VehicleType = agency.GetRandomVehicle(Player.WantedLevel, false, false, false);
                    if (VehicleType != null)
                    {
                        EntryPoint.WriteToConsole($"DISPATCHER: Attempting EMS Spawn Vehicle {VehicleType.ModelName}", 3);
                        DispatchablePerson PersonType = agency.GetRandomPed(Player.WantedLevel, VehicleType.RequiredPassengerModels);
                        if (PersonType != null)
                        {
                            EntryPoint.WriteToConsole($"DISPATCHER: Attempting EMS Spawn Vehicle {PersonType.ModelName}", 3);
                            try
                            {
                                SpawnTask spawnTask = new SpawnTask(agency, spawnLocation.InitialPosition, spawnLocation.StreetPosition, spawnLocation.Heading, VehicleType, PersonType, Settings.SettingsManager.EMSSettings.ShowSpawnedBlips, Settings, Weapons);// Settings.SettingsManager.Police.SpawnedAmbientPoliceHaveBlip);
                                spawnTask.AttemptSpawn();
                                spawnTask.CreatedPeople.ForEach(x => World.AddEntity(x));
                                spawnTask.CreatedVehicles.ForEach(x => World.AddEntity(x, ResponseType.EMS));
                                HasDispatchedThisTick = true;
                            }
                            catch (Exception ex)
                            {
                                EntryPoint.WriteToConsole($"DISPATCHER: Spawn EMS ERROR {ex.Message} : {ex.StackTrace}", 0);
                            }
                        }
                    }
                }
            }
            else
            {
                EntryPoint.WriteToConsole($"DISPATCHER: Attempting to Spawn EMS Failed, Has Spawns {spawnLocation.HasSpawns} Is Valid {isValidSpawn}", 5);
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
        if (Settings.SettingsManager.EMSSettings.ManageDispatching && IsTimeToRecall)
        {
            foreach (EMT emt in DeletableOfficers)
            {
                if(ShouldBeRecalled(emt))
                {
                    Delete(emt);
                    GameFiber.Yield();
                }
            }
            GameTimeAttemptedRecall = Game.GameTime;
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
                    }
                }
                if (emt.Pedestrian.Exists() && emt.Pedestrian.CurrentVehicle.Exists() && emt.Pedestrian.CurrentVehicle != null)
                {
                    emt.Pedestrian.CurrentVehicle.Delete();
                }
            }
            RemoveBlip(emt.Pedestrian);
            if (emt.Pedestrian.Exists())
            {
                //EntryPoint.WriteToConsole(string.Format("Delete Cop Handle: {0}, {1}, {2}", Cop.Pedestrian.Handle, Cop.DistanceToPlayer, Cop.AssignedAgency.Initials));
                emt.Pedestrian.Delete();
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
    private Agency GetRandomAgency(Vector3 spawnLocation)
    {
        Agency agency;
        List<Agency> PossibleAgencies = GetAgencies(spawnLocation, Player.WantedLevel);
        agency = PossibleAgencies.PickRandom();
        if (agency == null)
        {
            agency = GetAgencies(spawnLocation, Player.WantedLevel).PickRandom();
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