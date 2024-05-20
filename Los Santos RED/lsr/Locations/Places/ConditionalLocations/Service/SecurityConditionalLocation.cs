using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SecurityConditionalLocation : ConditionalLocation
{
    private Agency Agency;

    public SecurityConditionalLocation(Vector3 location, float heading, float percentage) : base(location, heading, percentage)
    {
    }

    public SecurityConditionalLocation()
    {
    }
    public override bool DetermineRun(bool force)
    {
        if (!Settings.SettingsManager.SecuritySettings.ManageDispatching)
        {
            return false;
        }
        //if (!Settings.SettingsManager.SecuritySettings.)
        //{
        //    return false;
        //}
        //if (World.TotalWantedLevel > Settings.SettingsManager.SecuritySettings.max)
        //{
        //    return false;
        //}
        if (World.Pedestrians.TotalSpawnedSecurityGuards > Settings.SettingsManager.SecuritySettings.TotalSpawnedMembersLimit)
        {
            return false;
        }
        return base.DetermineRun(force);
    }

    public override void RunSpawnTask()
    {
        try
        {
            EntryPoint.WriteToConsole("RUN SECURITY SPAWN TASK HAS EXECUTED");
            SecurityGuardSpawnTask securitySpawnTask = new SecurityGuardSpawnTask(Agency, SpawnLocation, DispatchableVehicle, DispatchablePerson, Settings.SettingsManager.SecuritySettings.ShowSpawnedBlips, Settings, Weapons, Names, true, World, Crimes, ModItems);
            securitySpawnTask.AllowAnySpawn = true;
            securitySpawnTask.AllowBuddySpawn = false;
            securitySpawnTask.ClearVehicleArea = true;
            securitySpawnTask.SpawnRequirement = TaskRequirements;
            securitySpawnTask.PlacePedOnGround = DispatchableVehicle == null; //true;
            securitySpawnTask.AttemptSpawn();
            EntryPoint.WriteToConsole("SECUIRTY RUN SPAWN TASK PRE POST RUN");
            securitySpawnTask.PostRun(this, GameLocation);

            securitySpawnTask.CreatedPeople.ForEach(x => { EntryPoint.WriteToConsole($"I CREATED SECURITY {x.Handle}"); });

            //securitySpawnTask.CreatedPeople.ForEach(x => { World.Pedestrians.AddEntity(x); x.IsLocationSpawned = true; AddLocationRequirements(x); });
            //securitySpawnTask.CreatedVehicles.ForEach(x => x.AddVehicleToList(World));// World.Vehicles.AddEntity(x, ResponseType.Other));
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"Security Dispatcher Spawn Error: {ex.Message} : {ex.StackTrace}", 0);
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
            Agency = Jurisdictions.GetRandomAgency(CurrentZone.InternalGameName, World.TotalWantedLevel, ResponseType.Security);
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
            DispatchableVehicle = Agency.GetRandomVehicle(World.TotalWantedLevel, false, false, true, RequiredVehicleGroup, Settings, ForceVehicleGroup);
            if (!IsEmpty && DispatchableVehicle != null)
            {
                DispatchablePerson = Agency.GetRandomPed(World.TotalWantedLevel, DispatchableVehicle.RequiredPedGroup);
            }
        }
    }
}
