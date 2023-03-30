using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GangConditionalLocation : ConditionalLocation
{
    private Gang Gang;

    public GangConditionalLocation(Vector3 location, float heading, float percentage) : base(location, heading, percentage)
    {
    }

    public GangConditionalLocation()
    {
    }

    public override void RunSpawnTask()
    {
        try
        {
            GangSpawnTask gangSpawnTask = new GangSpawnTask(Gang, SpawnLocation, DispatchableVehicle, DispatchablePerson, Settings.SettingsManager.GangSettings.ShowSpawnedBlip, Settings, Weapons, Names, true, Crimes, PedGroups, ShopMenus, World);// Settings.SettingsManager.Police.SpawnedAmbientPoliceHaveBlip);
            gangSpawnTask.AllowAnySpawn = true;
            gangSpawnTask.AllowBuddySpawn = false;
            gangSpawnTask.SpawnRequirement = SpawnRequirement;
            gangSpawnTask.ClearArea = true;
            gangSpawnTask.AttemptSpawn();
            foreach (PedExt created in gangSpawnTask.CreatedPeople)
            {
                World.Pedestrians.AddEntity(created);
            }
            gangSpawnTask.CreatedPeople.ForEach(x => { World.Pedestrians.AddEntity(x); x.IsLocationSpawned = true; });
            gangSpawnTask.CreatedVehicles.ForEach(x => World.Vehicles.AddEntity(x, ResponseType.None));
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"Gang Dispatcher Spawn Error: {ex.Message} : {ex.StackTrace}", 0);
        }
    }
    public override bool DetermineRun(bool force)
    {
        if (!Settings.SettingsManager.GangSettings.ManageDispatching)
        {
            return false;
        }
        if (!Settings.SettingsManager.GangSettings.AllowDenSpawning)
        {
            return false;
        }
        if(!Settings.SettingsManager.GangSettings.AllowDenSpawningWhenPlayerWanted && World.TotalWantedLevel > 0)
        {
            return false;
        }
        if (World.TotalWantedLevel > Settings.SettingsManager.GangSettings.DenSpawningWhenPlayerWantedMaxWanted)
        {
            return false;
        }
        if (!Settings.SettingsManager.GangSettings.DenSpawningIgnoresLimits && World.Pedestrians.TotalSpawnedGangMembers >= Settings.SettingsManager.GangSettings.TotalSpawnedMembersLimit)
        {
            return false;
        }
        if (!force && !RandomItems.RandomPercent(Percentage))
        {
            return false;
        }
        return true;
    }
    public override void GetDispatchableGenerator()
    {
        if (Gang != null)
        {
            return;
        }
        if (!string.IsNullOrEmpty(AssociationID))
        {
            Gang = Gangs.GetGang(AssociationID);
        }
        if (Gang == null && !string.IsNullOrEmpty(MasterAssociationID))
        {
            Gang = Gangs.GetGang(MasterAssociationID);
        }
        if (Gang == null)
        {
            Zone CurrentZone = Zones.GetZone(Location);
            Gang = GangTerritories.GetRandomGang(CurrentZone.InternalGameName,World.TotalWantedLevel);// Jurisdictions.GetMainAgency(CurrentZone.InternalGameName, ResponseType.Security);
        }
    }
    public override void GenerateSpawnTypes()
    {
        if (Gang == null)
        {
            return;
        }
        if (IsPerson || !IsEmpty)
        {
            DispatchablePerson = Gang.GetRandomPed(World.TotalWantedLevel, RequiredPedGroup);
        }
        if (!IsPerson)
        {
            DispatchableVehicle = Gang.GetRandomVehicle(World.TotalWantedLevel, false, false, true, RequiredVehicleGroup, Settings);
            if (!IsEmpty && DispatchableVehicle != null)
            {
                DispatchablePerson = Gang.GetRandomPed(World.TotalWantedLevel, DispatchableVehicle.RequiredPedGroup);
            }
        }
    }
}
