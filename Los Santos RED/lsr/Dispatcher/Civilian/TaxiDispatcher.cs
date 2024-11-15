using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DispatchScannerFiles;


public class TaxiDispatcher : DefaultDispatcher
{
    private TaxiFirm TaxiFirm;
    private IOrganizations Organizations;
    private uint GameTimeAttemptedDispatch;
    private bool ShouldRunAmbientDispatch;
    private uint GameTimeAttemptedRecall;
    private readonly float MinimumDeleteDistance = 150f;//200f
    private readonly uint MinimumExistingTime = 20000;
    protected override float MaxDistanceToSpawn => Settings.SettingsManager.TaxiSettings.MaxDistanceToSpawnInVehicle;
    protected override float MinDistanceToSpawn => Settings.SettingsManager.TaxiSettings.MinDistanceToSpawnInVehicle;
    private bool IsTimeToRecall => Game.GameTime - GameTimeAttemptedRecall >= 5000;// TimeBetweenSpawn;
    private bool IsTimeToAmbientDispatch => Game.GameTime - GameTimeAttemptedDispatch >= TimeBetweenSpawn;//15000;
    protected override float DistanceToDeleteInVehicle => Settings.SettingsManager.TaxiSettings.MaxDistanceToSpawnInVehicle + 150f;// 300f;
    protected override float DistanceToDeleteOnFoot => Settings.SettingsManager.TaxiSettings.MaxDistanceToSpawnOnFoot + 50f;// 200 + 50f grace = 250f;
    private List<TaxiDriver> DeleteableTaxiDrivers => World.Pedestrians.TaxiDriverList.Where(x => (x.TaxiRide == null || x.TaxiRide.IsActive == false) && ((x.RecentlyUpdated && x.DistanceToPlayer >= MinimumDeleteDistance && x.HasBeenSpawnedFor >= MinimumExistingTime) || x.CanRemove)).ToList();
    private bool HasNeedToAmbientDispatch
    {
        get
        {
            if(!Settings.SettingsManager.TaxiSettings.ManageDispatching)
            {
                return false;
            }
            if (World.Pedestrians.TotalSpawnedTaxiDrivers >= Settings.SettingsManager.TaxiSettings.TotalSpawnedMembersLimit)
            {
                return false;
            }
            if (World.Pedestrians.TotalSpawnedAmbientTaxiDrivers > AmbientMemberLimitForZoneType)
            {
                return false;
            }
            return true;
        }
    }
    private int AmbientMemberLimitForZoneType
    {
        get
        {
            int AmbientMemberLimit = Settings.SettingsManager.TaxiSettings.TotalSpawnedAmbientMembersLimit;
            if (EntryPoint.FocusZone?.Type == eLocationType.Wilderness)
            {
                AmbientMemberLimit = Settings.SettingsManager.TaxiSettings.TotalSpawnedAmbientMembersLimit_Wilderness;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Rural)
            {
                AmbientMemberLimit = Settings.SettingsManager.TaxiSettings.TotalSpawnedAmbientMembersLimit_Rural;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Suburb)
            {
                AmbientMemberLimit = Settings.SettingsManager.TaxiSettings.TotalSpawnedAmbientMembersLimit_Suburb;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Industrial)
            {
                AmbientMemberLimit = Settings.SettingsManager.TaxiSettings.TotalSpawnedAmbientMembersLimit_Industrial;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Downtown)
            {
                AmbientMemberLimit = Settings.SettingsManager.TaxiSettings.TotalSpawnedAmbientMembersLimit_Downtown;
            }
            return AmbientMemberLimit;
        }
    }
    private int TimeBetweenSpawn
    {
        get
        {
            int TotalTimeBetweenSpawns = Settings.SettingsManager.TaxiSettings.TimeBetweenSpawn;
            if (EntryPoint.FocusZone?.Type == eLocationType.Wilderness)
            {
                TotalTimeBetweenSpawns += Settings.SettingsManager.TaxiSettings.TimeBetweenSpawn_WildernessAdditional;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Rural)
            {
                TotalTimeBetweenSpawns += Settings.SettingsManager.TaxiSettings.TimeBetweenSpawn_RuralAdditional;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Suburb)
            {
                TotalTimeBetweenSpawns += Settings.SettingsManager.TaxiSettings.TimeBetweenSpawn_SuburbAdditional;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Industrial)
            {
                TotalTimeBetweenSpawns += Settings.SettingsManager.TaxiSettings.TimeBetweenSpawn_IndustrialAdditional;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Downtown)
            {
                TotalTimeBetweenSpawns += Settings.SettingsManager.TaxiSettings.TimeBetweenSpawn;
            }
            return TotalTimeBetweenSpawns;
        }
    }
    private int PercentageOfAmbientSpawn
    {
        get
        {
            int ambientSpawnPercent = Settings.SettingsManager.TaxiSettings.AmbientSpawnPercentage;
            if (EntryPoint.FocusZone?.Type == eLocationType.Wilderness)
            {
                ambientSpawnPercent = Settings.SettingsManager.TaxiSettings.AmbientSpawnPercentage_Wilderness;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Rural)
            {
                ambientSpawnPercent = Settings.SettingsManager.TaxiSettings.AmbientSpawnPercentage_Rural;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Suburb)
            {
                ambientSpawnPercent = Settings.SettingsManager.TaxiSettings.AmbientSpawnPercentage_Suburb;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Industrial)
            {
                ambientSpawnPercent = Settings.SettingsManager.TaxiSettings.AmbientSpawnPercentage_Industrial;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Downtown)
            {
                ambientSpawnPercent = Settings.SettingsManager.TaxiSettings.AmbientSpawnPercentage_Downtown;
            }
            return ambientSpawnPercent;
        }
    }
    public TaxiDispatcher(IEntityProvideable world, IDispatchable player, IAgencies agencies, ISettingsProvideable settings, IStreets streets, IZones zones, IJurisdictions jurisdictions,
        IWeapons weapons, INameProvideable names, IPlacesOfInterest placesOfInterest, IOrganizations organizations, ICrimes crimes, IModItems modItems, IShopMenus shopMenus) : base(world, player, agencies, settings, streets, zones, jurisdictions, weapons, names, placesOfInterest, crimes , modItems,shopMenus)
    {
        Organizations = organizations;
    }
    protected override bool DetermineRun()
    {
        bool shouldRun = false;
        if (!IsTimeToAmbientDispatch || !HasNeedToAmbientDispatch)
        {
            return false;
        }
        if (ShouldRunAmbientDispatch)
        {
            shouldRun = true;
        }
        else
        {
            ShouldRunAmbientDispatch = RandomItems.RandomPercent(PercentageOfAmbientSpawn);
            if (ShouldRunAmbientDispatch)
            {
                shouldRun = true;
            }
            else
            {
                GameTimeAttemptedDispatch = Game.GameTime;
            }
        }

        if (shouldRun)
        {
            GameTimeAttemptedDispatch = Game.GameTime;
            GameFiber.Yield();
        }

       // EntryPoint.WriteToConsole($"TAXI DISPATCHER DetermineRun shouldRun{shouldRun} TotalSpawnedTaxiDrivers{World.Pedestrians.TotalSpawnedTaxiDrivers}");
        return shouldRun;
    }
    protected override bool DetermineRecall()
    {
        if (!IsTimeToRecall)
        {
            return false;
        }
        foreach (TaxiDriver taxiDrivers in DeleteableTaxiDrivers)
        {
            if (ShouldBeRecalled(taxiDrivers))
            {
                Delete(taxiDrivers);
                GameFiber.Yield();
            }
        }
        GameTimeAttemptedRecall = Game.GameTime;
        return true;
    }
    protected override Vector3 GetSpawnPosition()
    {
        Vector3 Position;
        if (Player.IsInVehicle)
        {
            Position = Player.Character.GetOffsetPositionFront(50f);
        }
        else
        {
            Position = Player.Position;
        }
        Position = Position.Around2D(MinDistanceToSpawn, MaxDistanceToSpawn);
       // EntryPoint.WriteToConsole($"TAXI DISPATCHER GetSpawnPosition Position{Position}");
        return Position;
    }
    protected override bool GetSpawnTypes()
    {
        TaxiFirm = Organizations.GetRandomTaxiFirm(false);
        if(TaxiFirm == null)
        { 
            return false;
        }
        if(TaxiFirm.Personnel == null || TaxiFirm.Vehicles == null)
        {
            return false;
        }
        VehicleType = TaxiFirm.GetRandomVehicle(0, false, false, true, "", Settings);//.Vehicles.PickRandom();
        if (VehicleType == null)
        {
            return false;
        }
        if(string.IsNullOrEmpty(VehicleType.RequiredPedGroup))
        {
            PersonType = TaxiFirm.Personnel.PickRandom();
        }
        else
        {
            PersonType = TaxiFirm.Personnel.Where(x=> x.GroupName == VehicleType.RequiredPedGroup).PickRandom();
        }
        if(PersonType == null)
        {
            return false;
        }

       // EntryPoint.WriteToConsole($"TAXI DISPATCHER GetSpawnTypes PersonType{PersonType.ModelName} VehicleType{VehicleType.ModelName}");
        return true;
    }
    protected override bool CallSpawnTask()
    {
        TaxiSpawnTask civilianSpawnTask = new TaxiSpawnTask(SpawnLocation, VehicleType, PersonType, Settings.SettingsManager.TaxiSettings.ShowSpawnedBlip, false, true, Settings, Crimes, Weapons, Names, World, ModItems, ShopMenus, TaxiFirm);
        civilianSpawnTask.AllowAnySpawn = true;
        civilianSpawnTask.AllowBuddySpawn = false;
        civilianSpawnTask.PlacePedOnGround = false;
        civilianSpawnTask.AttemptSpawn();
        if (TaxiFirm != null)
        {
            civilianSpawnTask.PossibleHeads = TaxiFirm.PossibleHeads;
        }
        //civilianSpawnTask.CreatedPeople.ForEach(x => World.Pedestrians.AddEntity(x));
        //civilianSpawnTask.CreatedVehicles.ForEach(x => x.AddVehicleToList(World));
        PedExt spawnedDriver = civilianSpawnTask.CreatedPeople.FirstOrDefault();
        VehicleExt spawnedVehicle = civilianSpawnTask.CreatedVehicles.FirstOrDefault();
        //spawnedVehicle?.AddBlip();
        bool SpawnedItems = false;
        if (spawnedDriver != null && spawnedDriver.Pedestrian.Exists() && spawnedVehicle != null && spawnedVehicle.Vehicle.Exists())
        {
            SpawnedItems = true;
        }
        if (SpawnedItems)
        {
            ShouldRunAmbientDispatch = false;
        }
       // EntryPoint.WriteToConsole($"TAXI DISPATCHER CallSpawnTask SpawnedItems{SpawnedItems} PEDHANDLE:{spawnedDriver?.Handle} VEHHANDLE:{spawnedVehicle?.Handle}");
        return SpawnedItems;
    }
    public void ForceTaxiSpawn(TaxiFirm taxiFirm)
    {
        VehicleType = null;
        PersonType = null;
        TimesToTryLocation = 50;
        if(!GetCloseSpawnLocation())
        {
            TimesToTryLocation = 3;
            EntryPoint.WriteToConsole("ForceTaxiSpawn FAIL NO SPAWN LOCATION");
            return;
        }
        TimesToTryLocation = 3;
        TaxiFirm = taxiFirm;
        if (TaxiFirm == null)
        {
            EntryPoint.WriteToConsole("ForceTaxiSpawn FAIL NO TAXI FIRM");
            return;
        }
        VehicleType = TaxiFirm.GetRandomVehicle(Player.WantedLevel, true, true, true, "", Settings);
        if (VehicleType != null)
        {
            string RequiredGroup = "";
            if (VehicleType != null)
            {
                RequiredGroup = VehicleType.RequiredPedGroup;
            }
            PersonType = TaxiFirm.GetRandomPed(Player.WantedLevel, RequiredGroup);
        }
        if(VehicleType == null)
        {
            EntryPoint.WriteToConsole("ForceTaxiSpawn FAIL NO TAXI VEHICLE");
            return;
        }

        if (PersonType == null)
        {
            EntryPoint.WriteToConsole("ForceTaxiSpawn FAIL NO TAXI PERSON");
            return;
        }
        for (int i = 0;i<2; i++)
        {
            if (CallSpawnTask())
            {
                break;
            }
            GameFiber.Yield();
        }
        
    }
    public void DebugSpawnTaxi(TaxiFirm taxiFirm, bool onFoot, bool isEmpty)
    {
        VehicleType = null;
        PersonType = null;
        SpawnLocation = new SpawnLocation();
        SpawnLocation.InitialPosition = Game.LocalPlayer.Character.GetOffsetPositionFront(10f);
        SpawnLocation.StreetPosition = SpawnLocation.InitialPosition;
        SpawnLocation.Heading = Game.LocalPlayer.Character.Heading - 180f;
        TaxiFirm = taxiFirm;
        if(TaxiFirm == null)
        {
            return;
        }
        if (!onFoot)
        {
            VehicleType = TaxiFirm.GetRandomVehicle(Player.WantedLevel, true, true, true, "", Settings);
        }
        if (VehicleType != null || onFoot)
        {
            string RequiredGroup = "";
            if (VehicleType != null)
            {
                RequiredGroup = VehicleType.RequiredPedGroup;
            }
            PersonType = TaxiFirm.GetRandomPed(Player.WantedLevel, RequiredGroup);
        }
        if (isEmpty)
        {
            PersonType = null;
        }
        CallSpawnTask();
    }
}
