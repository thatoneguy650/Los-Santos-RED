using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Mod;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

public class CivilianConditionalLocation : ConditionalLocation
{
    //protected GameLocation GameLocation;
    public CivilianConditionalLocation(Vector3 location, float heading, float percentage) : base(location, heading, percentage)
    {

    }
    public CivilianConditionalLocation()
    {

    }
    public override void RunSpawnTask()
    {
        EntryPoint.WriteToConsole("CIVILIAN CONDITIONAL SPAWN TASK RUN START");
        try
        {
            CivilianSpawnTask civilianSpawnTask = new CivilianSpawnTask(SpawnLocation, DispatchableVehicle, DispatchablePerson, false, false, true, Settings, Crimes, Weapons, Names, World, ModItems, ShopMenus);//, Names, true, Crimes, PedGroups, ShopMenus, World, ModItems, ForceMelee, ForceSidearm, ForceLongGun);// Settings.SettingsManager.Police.SpawnedAmbientPoliceHaveBlip);
            civilianSpawnTask.PossibleHeads = GameLocation.VendorPossibleHeads;
            civilianSpawnTask.AllowAnySpawn = true;
            civilianSpawnTask.AllowBuddySpawn = false;
            civilianSpawnTask.SpawnRequirement = TaskRequirements;
            civilianSpawnTask.AttemptSpawn();
            civilianSpawnTask.PostRun(this, GameLocation);
            //merchantSpawnTask.CreatedPeople.ForEach(x => { World.Pedestrians.AddEntity(x); x.IsLocationSpawned = true; AddLocationRequirements(x); });
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"Civilian Dispatcher Spawn Error: {ex.Message} : {ex.StackTrace}", 0);
        }
    }
    public override bool DetermineRun(bool force)
    {
        if (!Settings.SettingsManager.CivilianSettings.ManageDispatching)
        {
            return false;
        }
        //if (World.Pedestrians.TotalSpawnedServiceWorkers >= Settings.SettingsManager.CivilianSettings.TotalSpawnedServiceMembersLimit)
        //{
        //    return false;
        //}
        //if (GameLocation == null)
        //{
        //    return false;
        //}
        return base.DetermineRun(force);
    }
    public override void GenerateSpawnTypes()
    {
        if (GameLocation == null)
        {
            return;
        }
        if(GameLocation.AssignedOrganization == null)
        {
            return;
        }
        if (IsPerson || !IsEmpty)
        {
            DispatchablePerson = GameLocation.AssignedOrganization.GetRandomPed(World.TotalWantedLevel, RequiredPedGroup);
        }
        if (!IsPerson)
        {
            DispatchableVehicle = GameLocation.AssignedOrganization.GetRandomVehicle(World.TotalWantedLevel, AllowAirVehicle, AllowBoat, true, RequiredVehicleGroup, Settings);
            if (!IsEmpty && DispatchableVehicle != null)
            {
                DispatchablePerson = GameLocation.AssignedOrganization.GetRandomPed(World.TotalWantedLevel, DispatchableVehicle.RequiredPedGroup);
            }
        }
    }
}
