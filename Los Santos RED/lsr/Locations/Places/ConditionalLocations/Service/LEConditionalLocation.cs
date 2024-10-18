using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

public class LEConditionalLocation : ConditionalLocation
{
    private Agency Agency;
    public override int MinWantedLevelSpawn { get; set; } = 0;
    public override int MaxWantedLevelSpawn { get; set; } = 4;
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

        if(DispatchablePerson != null && World.Pedestrians.TotalSpawnedLocationPolice > Settings.SettingsManager.PoliceSpawnSettings.LocationSpawnedPedLimit)
        {
            EntryPoint.WriteToConsole("LEConditionalLocation TOO MANY LOCATION PEDS");
            return false;
        }
        if(DispatchableVehicle != null && World.Vehicles.SpawnedEmptyPoliceVehiclesCount >= Settings.SettingsManager.PoliceSpawnSettings.LocationSpawnedVehicleLimit)
        {
            EntryPoint.WriteToConsole("LEConditionalLocation TOO MANY LOCATION VEHCILES");
            return false;
        }
        return base.DetermineRun(force);
    }
    public override void RunSpawnTask()
    {
        try
        {
            EntryPoint.WriteToConsole("ATTEMPT LE SPAWN CONDITIONAL LOCATION");
            LESpawnTask spawnTask = new LESpawnTask(Agency, SpawnLocation, DispatchableVehicle, DispatchablePerson, Settings.SettingsManager.PoliceSpawnSettings.ShowSpawnedBlips, Settings, Weapons, Names, RandomItems.RandomPercent(Settings.SettingsManager.PoliceSpawnSettings.AddOptionalPassengerPercentage), World, ModItems, false);
            spawnTask.AllowAnySpawn = true;
            spawnTask.AllowBuddySpawn = false;
            spawnTask.ClearVehicleArea = true;
            spawnTask.SpawnRequirement = TaskRequirements;
            spawnTask.PlacePedOnGround = DispatchableVehicle == null;// true;
            spawnTask.AttemptSpawn();
            GameFiber.Yield();
            spawnTask.PostRun(this, GameLocation);
            //spawnTask.CreatedPeople.ForEach(x => { World.Pedestrians.AddEntity(x); x.IsLocationSpawned = true; AddLocationRequirements(x); GameLocation?.AddSpawnedPed(x); });
            //spawnTask.CreatedVehicles.ForEach(x => { x.AddVehicleToList(World); x.WasSpawnedEmpty = DispatchablePerson == null;GameLocation?.AddSpawnedVehicle(x);   } ) ;//World.Vehicles.AddEntity(x, ResponseType.LawEnforcement));
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
            DispatchableVehicle = Agency.GetRandomVehicle(World.TotalWantedLevel, AllowAirVehicle, AllowBoat, true, RequiredVehicleGroup, Settings, ForceVehicleGroup);
            if(!IsEmpty && DispatchableVehicle != null)
            {
                DispatchablePerson = Agency.GetRandomPed(World.TotalWantedLevel, DispatchableVehicle.RequiredPedGroup);
            }
        }
        EntryPoint.WriteToConsole($"LECONDITIONAL IsPerson:{IsPerson} IsEmpty:{IsEmpty} RequiredVehicleGroup:{RequiredVehicleGroup} GenerateSpawnTypes: {DispatchableVehicle?.ModelName} AllowAirVehicle:{AllowAirVehicle}");
    }
}
