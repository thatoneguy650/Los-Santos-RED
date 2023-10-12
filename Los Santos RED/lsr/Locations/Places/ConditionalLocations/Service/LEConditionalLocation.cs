using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LEConditionalLocation : ConditionalLocation
{
    private Agency Agency;

    public LEConditionalLocation(Vector3 location, float heading, float percentage) : base(location, heading, percentage)
    {
    }

    public LEConditionalLocation()
    {
    }
    public override bool DetermineRun(bool force)
    {
        if(!Settings.SettingsManager.PoliceSpawnSettings.ManageDispatching)
        {
            return false;
        }
        if(!Settings.SettingsManager.PoliceSpawnSettings.AllowLocationSpawning)
        {
            return false;
        }
        if (World.Pedestrians.TotalSpawnedAmbientPolice > Settings.SettingsManager.PoliceSpawnSettings.PedSpawnLimit_Wanted3)
        {
            return false;
        }
        return base.DetermineRun(force);
    }
    public override void RunSpawnTask()
    {
        try
        {
            //EntryPoint.WriteToConsoleTestLong("ATTEMPT LE SPAWN");
            LESpawnTask spawnTask = new LESpawnTask(Agency, SpawnLocation, DispatchableVehicle, DispatchablePerson, Settings.SettingsManager.PoliceSpawnSettings.ShowSpawnedBlips, Settings, Weapons, Names, RandomItems.RandomPercent(Settings.SettingsManager.PoliceSpawnSettings.AddOptionalPassengerPercentage), World, ModItems, false);
            spawnTask.AllowAnySpawn = true;
            spawnTask.AllowBuddySpawn = false;
            spawnTask.ClearVehicleArea = true;
            spawnTask.SpawnRequirement = TaskRequirements;
            spawnTask.PlacePedOnGround = DispatchableVehicle == null;// true;
            spawnTask.AttemptSpawn();
            GameFiber.Yield();
            spawnTask.CreatedPeople.ForEach(x => { World.Pedestrians.AddEntity(x); x.IsLocationSpawned = true; AddLocationRequirements(x); });
            spawnTask.CreatedVehicles.ForEach(x => x.AddVehicleToList(World));//World.Vehicles.AddEntity(x, ResponseType.LawEnforcement));
            Player.OnLawEnforcementSpawn(Agency, DispatchableVehicle, DispatchablePerson);
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"LE Dispatcher Spawn Error: {ex.Message} : {ex.StackTrace}", 0);
        }
    }
    public override void GetDispatchableGenerator()
    {
        if(Agency != null)
        {
            return;
        }
        if(!string.IsNullOrEmpty(AssociationID))
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
            Agency = Jurisdictions.GetRandomAgency(CurrentZone?.InternalGameName, 0, ResponseType.LawEnforcement);
        }
        //EntryPoint.WriteToConsole($"LECONDITIONAL GetDispatchableGenerator: {Agency?.FullName}");
    }
    public override void GenerateSpawnTypes()
    {
        if(Agency == null)
        {
            return;
        }
        if(IsPerson || !IsEmpty) 
        {
            DispatchablePerson = Agency.GetRandomPed(World.TotalWantedLevel, RequiredPedGroup);
        }
        if(!IsPerson)
        {
            DispatchableVehicle = Agency.GetRandomVehicle(World.TotalWantedLevel, AllowAirVehicle, AllowBoat, true, RequiredVehicleGroup, Settings);
            if(!IsEmpty && DispatchableVehicle != null)
            {
                DispatchablePerson = Agency.GetRandomPed(World.TotalWantedLevel, DispatchableVehicle.RequiredPedGroup);
            }
        }
        //EntryPoint.WriteToConsole($"LECONDITIONAL IsPerson:{IsPerson} IsEmpty:{IsEmpty} GenerateSpawnTypes: {DispatchableVehicle?.ModelName} AllowAirVehicle:{AllowAirVehicle}");
    }
}
