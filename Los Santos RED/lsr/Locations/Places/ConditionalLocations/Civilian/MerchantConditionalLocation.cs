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

public class MerchantConditionalLocation : ConditionalLocation
{
    //protected GameLocation GameLocation;
    public MerchantConditionalLocation(Vector3 location, float heading, float percentage) : base(location, heading, percentage)
    {

    }
    public MerchantConditionalLocation()
    {

    }
    public override void RunSpawnTask()
    {
        try
        {
            MerchantSpawnTask merchantSpawnTask = new MerchantSpawnTask(SpawnLocation, null, DispatchablePerson, false, false, true, Settings, Crimes, Weapons, Names, World, ModItems, ShopMenus, GameLocation);//, Names, true, Crimes, PedGroups, ShopMenus, World, ModItems, ForceMelee, ForceSidearm, ForceLongGun);// Settings.SettingsManager.Police.SpawnedAmbientPoliceHaveBlip);
            merchantSpawnTask.PossibleHeads = GameLocation.VendorPossibleHeads;
            merchantSpawnTask.AllowAnySpawn = true;
            merchantSpawnTask.AllowBuddySpawn = false;
            merchantSpawnTask.SetupMenus = false;
            merchantSpawnTask.SpawnRequirement = TaskRequirements;
            //merchantSpawnTask.ClearVehicleArea = true;
            //merchantSpawnTask.PlacePedOnGround = true;
            merchantSpawnTask.AttemptSpawn();
            merchantSpawnTask.PostRun(this, GameLocation);
            //merchantSpawnTask.CreatedPeople.ForEach(x => { World.Pedestrians.AddEntity(x); x.IsLocationSpawned = true; AddLocationRequirements(x); });
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"Merchant Dispatcher Spawn Error: {ex.Message} : {ex.StackTrace}", 0);
        }
    }
    public override bool DetermineRun(bool force)
    {
        if (!Settings.SettingsManager.CivilianSettings.ManageDispatching)
        {
            return false;
        }
        if (World.Pedestrians.TotalSpawnedServiceWorkers >= Settings.SettingsManager.CivilianSettings.TotalSpawnedServiceMembersLimit)
        {
            return false;
        }
        if(GameLocation == null)
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
        if (IsPerson || !IsEmpty)
        {
            DispatchablePerson = GameLocation.VendorPersonnel.PickRandom();
        }
        if(!IsPerson)
        {
            //do a car spawn here if you wanna store car spawn stuff
        }
    }
}
