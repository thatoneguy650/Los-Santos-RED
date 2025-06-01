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

    public string OverrideDispatchableVehicleGroupID { get; set; }
    public string OverrideDispatchablePersonGroupID { get; set; }
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
        if (World.Pedestrians.TotalSpawnedCivilians >= Settings.SettingsManager.CivilianSettings.TotalSpawnedCiviliansLimit)
        {
            return false;
        }
        if (GameLocation == null)
        {
            return false;
        }
        return base.DetermineRun(force);
    }
    public override void GenerateSpawnTypes()
    {
        if (GameLocation == null)
        {
            return;
        }
        if (GameLocation.AssignedOrganization == null && string.IsNullOrEmpty(OverrideDispatchableVehicleGroupID) && string.IsNullOrEmpty(OverrideDispatchablePersonGroupID))
        {
            return;
        }
        if (IsPerson || !IsEmpty)
        {
            GetPersonType();
        }
        if (!IsPerson)
        {
            GetVehicleType();
            if (!IsEmpty && DispatchableVehicle != null)
            {
                GetPersonType();
            }
        }

    }
    private void GetPersonType()
    {
        if (GameLocation.AssignedOrganization != null)
        {
            DispatchablePerson = GameLocation.AssignedOrganization.GetRandomPed(World.TotalWantedLevel, RequiredPedGroup);
        }
        else if (!string.IsNullOrEmpty(OverrideDispatchablePersonGroupID))
        {
            DispatchablePerson = DispatchablePeople.GetPersonData(OverrideDispatchablePersonGroupID)?.RandomElementByWeight(x => x.CurrentSpawnChance(World.TotalWantedLevel));
        }
    }
    private void GetVehicleType()
    {
        if (GameLocation.AssignedOrganization != null)
        {
            DispatchableVehicle = GameLocation.AssignedOrganization.GetRandomVehicle(World.TotalWantedLevel, AllowAirVehicle, AllowBoat, true, RequiredVehicleGroup, Settings);
        }
        else if (!string.IsNullOrEmpty(OverrideDispatchableVehicleGroupID))
        {
            DispatchableVehicle = DispatchableVehicles.GetVehicleData(OverrideDispatchableVehicleGroupID)?.RandomElementByWeight(x => x.CurrentSpawnChance(World.TotalWantedLevel, Settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles));
        }
    }
}
