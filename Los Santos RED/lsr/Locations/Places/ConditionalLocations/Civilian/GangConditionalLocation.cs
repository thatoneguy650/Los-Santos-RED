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
    public bool TerritorySpawnsForceMainGang { get; set; } = false;

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
            GameFiber.Yield();
            GangSpawnTask gangSpawnTask = new GangSpawnTask(Gang, SpawnLocation, DispatchableVehicle, DispatchablePerson, Settings.SettingsManager.GangSettings.ShowSpawnedBlip, Settings, Weapons, Names, true, Crimes, PedGroups, ShopMenus, World, ModItems, ForceMelee, ForceSidearm, ForceLongGun);// Settings.SettingsManager.Police.SpawnedAmbientPoliceHaveBlip);
            gangSpawnTask.AllowAnySpawn = true;
            gangSpawnTask.AllowBuddySpawn = false;
            gangSpawnTask.SpawnRequirement = TaskRequirements;
            gangSpawnTask.ClearVehicleArea = true;
            gangSpawnTask.PlacePedOnGround = DispatchableVehicle == null;// true;
            gangSpawnTask.AttemptSpawn();
            foreach (PedExt created in gangSpawnTask.CreatedPeople)
            {
                World.Pedestrians.AddEntity(created);
            }
            gangSpawnTask.PostRun(this, GameLocation);
            //gangSpawnTask.CreatedPeople.ForEach(x => { World.Pedestrians.AddEntity(x); x.IsLocationSpawned = true; AddLocationRequirements(x); });
            //gangSpawnTask.CreatedVehicles.ForEach(x => x.AddVehicleToList(World));//World.Vehicles.AddEntity(x, ResponseType.None));
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
        return base.DetermineRun(force);
    }
    public override void GetDispatchableGenerator()
    {
        //EntryPoint.WriteToConsoleTestLong($"GANG GetDispatchableGenerator: AssociationID: {AssociationID} MasterAssociationID: {MasterAssociationID}");
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
            if (CurrentZone == null)
            {
                return;
            }
            if (TerritorySpawnsForceMainGang)
            {
                Gang = GangTerritories.GetMainGang(CurrentZone.InternalGameName);// Jurisdictions.GetMainAgency(CurrentZone.InternalGameName, ResponseType.Security);
            }
            else
            {
                Gang = GangTerritories.GetRandomGang(CurrentZone.InternalGameName, World.TotalWantedLevel);
            }
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
            DispatchableVehicle = Gang.GetRandomVehicle(World.TotalWantedLevel, AllowAirVehicle, AllowBoat, true, RequiredVehicleGroup, Settings);
            if (!IsEmpty && DispatchableVehicle != null)
            {
                DispatchablePerson = Gang.GetRandomPed(World.TotalWantedLevel, DispatchableVehicle.RequiredPedGroup);
            }
        }
    }
}
