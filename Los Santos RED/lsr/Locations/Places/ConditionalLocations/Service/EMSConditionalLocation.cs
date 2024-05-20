using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EMSConditionalLocation : ConditionalLocation
{
    private Agency Agency;

    public EMSConditionalLocation(Vector3 location, float heading, float percentage) : base(location, heading, percentage)
    {
    }

    public EMSConditionalLocation()
    {

    }
    public override bool DetermineRun(bool force)
    {
        if (!Settings.SettingsManager.EMSSettings.ManageDispatching)
        {
            return false;
        }
        if (!Settings.SettingsManager.EMSSettings.AllowStationSpawning)
        {
            return false;
        }
        if (!Settings.SettingsManager.EMSSettings.AllowStationSpawningWhenPlayerWanted && Player.IsWanted)
        {
            return false;
        }
        if (Settings.SettingsManager.EMSSettings.AllowStationSpawningWhenPlayerWanted && Player.WantedLevel > Settings.SettingsManager.EMSSettings.StationSpawningWhenPlayerWantedMaxWanted)
        {
            return false;
        }
        if (!Settings.SettingsManager.EMSSettings.StationSpawningIgnoresLimits && World.Pedestrians.TotalSpawnedEMTs > Settings.SettingsManager.EMSSettings.TotalSpawnedMembersLimit)
        {
            return false;
        }
        return base.DetermineRun(force);
    }
    public override void RunSpawnTask()
    {
        try
        {
            EMTSpawnTask eMTSpawnTask = new EMTSpawnTask(Agency, SpawnLocation, DispatchableVehicle, DispatchablePerson, Settings.SettingsManager.EMSSettings.ShowSpawnedBlips, Settings, Weapons, Names, true, World, ModItems);
            eMTSpawnTask.AllowAnySpawn = true;
            eMTSpawnTask.AllowBuddySpawn = false;
            eMTSpawnTask.SpawnRequirement = TaskRequirements;
            eMTSpawnTask.ClearVehicleArea = true;
            eMTSpawnTask.PlacePedOnGround = DispatchableVehicle == null;// true;
            eMTSpawnTask.AttemptSpawn();

            eMTSpawnTask.PostRun(this, GameLocation);
            //eMTSpawnTask.CreatedPeople.ForEach(x => { World.Pedestrians.AddEntity(x); x.IsLocationSpawned = true; AddLocationRequirements(x); });
            //eMTSpawnTask.CreatedPeople.ForEach(x => World.Pedestrians.AddEntity(x));
            //eMTSpawnTask.CreatedVehicles.ForEach(x => x.AddVehicleToList(World));//why twice?
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"EMS Dispatcher Spawn Error: {ex.Message} : {ex.StackTrace}", 0);
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
            Agency = Jurisdictions.GetRandomAgency(CurrentZone?.InternalGameName, World.TotalWantedLevel, ResponseType.EMS);
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
