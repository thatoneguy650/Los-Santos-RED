using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class FireConditionalLocation : ConditionalLocation
{
    private Agency Agency;

    public FireConditionalLocation(Vector3 location, float heading, float percentage) : base(location, heading, percentage)
    {
    }

    public FireConditionalLocation()
    {
    }
    public override bool DetermineRun(bool force)
    {
        if (!Settings.SettingsManager.FireSettings.ManageDispatching)
        {
            return false;
        }
        if (!Settings.SettingsManager.FireSettings.AllowStationSpawning)
        {
            return false;
        }
        if (!Settings.SettingsManager.FireSettings.AllowStationSpawningWhenPlayerWanted && Player.IsWanted)
        {
            return false;
        }
        if (Settings.SettingsManager.FireSettings.AllowStationSpawningWhenPlayerWanted && Player.WantedLevel > Settings.SettingsManager.FireSettings.StationSpawningWhenPlayerWantedMaxWanted)
        {
            return false;
        }
        if (!Settings.SettingsManager.FireSettings.StationSpawningIgnoresLimits && World.Pedestrians.TotalSpawnedFirefighters > Settings.SettingsManager.FireSettings.TotalSpawnedMembersLimit)
        {
            return false;
        }
        return base.DetermineRun(force);
    }

    public override void RunSpawnTask()
    {
        try
        {
            FireFighterSpawnTask fireFighterSpawnTask = new FireFighterSpawnTask(Agency, SpawnLocation, DispatchableVehicle, DispatchablePerson, Settings.SettingsManager.FireSettings.ShowSpawnedBlips, Settings, Weapons, Names, true, World, ModItems, ShopMenus);
            fireFighterSpawnTask.AllowAnySpawn = true;
            fireFighterSpawnTask.AllowBuddySpawn = false;
            fireFighterSpawnTask.SpawnRequirement = TaskRequirements;
            fireFighterSpawnTask.ClearVehicleArea = true;
            fireFighterSpawnTask.PlacePedOnGround = DispatchableVehicle == null;// true;
            fireFighterSpawnTask.AttemptSpawn();
            fireFighterSpawnTask.PostRun(this, GameLocation);
            //fireFighterSpawnTask.CreatedPeople.ForEach(x => { World.Pedestrians.AddEntity(x); x.IsLocationSpawned = true; AddLocationRequirements(x); });
            //fireFighterSpawnTask.CreatedVehicles.ForEach(x => x.AddVehicleToList(World));//World.Vehicles.AddEntity(x, ResponseType.Fire));
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"Firefighter Dispatcher Spawn Error: {ex.Message} : {ex.StackTrace}", 0);
        }
    }
    public override void GetDispatchableGenerator()
    {
        if (Agency != null)
        {
            return;
        }
        if (!string.IsNullOrEmpty(AssociationID))
        {
            Agency = Agencies.GetAgency(AssociationID);
        }
        if (Agency == null && !string.IsNullOrEmpty(MasterAssociationID))
        {
            Agency = Agencies.GetAgency(MasterAssociationID);
        }
        if (Agency == null)
        {
            Zone CurrentZone = Zones.GetZone(Location);
            if (CurrentZone == null)
            {
                return;
            }
            Agency = Jurisdictions.GetRandomAgency(CurrentZone.InternalGameName, World.TotalWantedLevel, ResponseType.Fire);
        }
    }
    public override void GenerateSpawnTypes()
    {
        if (Agency == null)
        {
            return;
        }
        if (IsPerson || !IsEmpty)
        {
            DispatchablePerson = Agency.GetRandomPed(World.TotalWantedLevel, RequiredPedGroup);
        }
        if (!IsPerson)
        {
            DispatchableVehicle = Agency.GetRandomVehicle(World.TotalWantedLevel, AllowAirVehicle, AllowBoat, true, RequiredVehicleGroup, Settings, ForceVehicleGroup);
            if (!IsEmpty && DispatchableVehicle != null)
            {
                DispatchablePerson = Agency.GetRandomPed(World.TotalWantedLevel, DispatchableVehicle.RequiredPedGroup);
            }
        }
    }
}
