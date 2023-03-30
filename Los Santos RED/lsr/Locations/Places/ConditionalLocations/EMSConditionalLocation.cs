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
        if (!force && !RandomItems.RandomPercent(Percentage))
        {
            return false;
        }
        return true;
    }


    public override void RunSpawnTask()
    {
        try
        {
            EMTSpawnTask eMTSpawnTask = new EMTSpawnTask(Agency, SpawnLocation, DispatchableVehicle, DispatchablePerson, Settings.SettingsManager.EMSSettings.ShowSpawnedBlips, Settings, Weapons, Names, true, World);
            eMTSpawnTask.AllowAnySpawn = true;
            eMTSpawnTask.AllowBuddySpawn = false;
            eMTSpawnTask.SpawnRequirement = SpawnRequirement;
            eMTSpawnTask.ClearArea = true;
            eMTSpawnTask.AttemptSpawn();
            eMTSpawnTask.CreatedPeople.ForEach(x => { World.Pedestrians.AddEntity(x); x.IsLocationSpawned = true; });
            eMTSpawnTask.CreatedPeople.ForEach(x => World.Pedestrians.AddEntity(x));
            eMTSpawnTask.CreatedVehicles.ForEach(x => World.Vehicles.AddEntity(x, ResponseType.EMS));
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
            Agency = Jurisdictions.GetRandomAgency(CurrentZone.InternalGameName, World.TotalWantedLevel, ResponseType.EMS);
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
            DispatchableVehicle = Agency.GetRandomVehicle(World.TotalWantedLevel, false, false, true, RequiredVehicleGroup, Settings);
            if (!IsEmpty && DispatchableVehicle != null)
            {
                DispatchablePerson = Agency.GetRandomPed(World.TotalWantedLevel, DispatchableVehicle.RequiredPedGroup);
            }
        }
    }
}
