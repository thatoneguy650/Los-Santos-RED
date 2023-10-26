using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MarshalDispatcher
{
    private IDispatchable Player;
    private LEDispatcher LEDispatcher;
    private ISettingsProvideable Settings;

    private IWeapons Weapons;
    private INameProvideable Names;
    private IPlacesOfInterest PlacesOfInterest;
    private IModItems ModItems;
    private IEntityProvideable World;
    private IAgencies Agencies;

    private uint TimeBetweenMarshals;
    private uint GameTimeLastDispatchedMarshals;
    private bool HasDispatchedThisTick;
    private SpawnLocation SpawnLocation;
    private Agency Agency;
    private DispatchableVehicle VehicleType;
    private DispatchablePerson PersonType;

    private uint GameTimeAPBStarted;
    private uint GameTimeNotWantedToAttemptDispatch;
    private bool CanDispatch = false;
    private List<PedExt> SpawnedMarshalls = new List<PedExt>();
    private bool IsTimeToDispatchMarshals => Game.GameTime - GameTimeLastDispatchedMarshals >= TimeBetweenMarshals;
    private bool HasNeedToDispatch => World.Pedestrians.TotalSpawnedAmbientPolice < Settings.SettingsManager.PoliceSpawnSettings.PedSpawnLimit_Wanted3 && World.Vehicles.SpawnedAmbientPoliceVehiclesCount < Settings.SettingsManager.PoliceSpawnSettings.VehicleSpawnLimit_Wanted3;//a few more than default so they spawnzo

    public MarshalDispatcher(IDispatchable player, LEDispatcher lEDispatcher, ISettingsProvideable settings, IEntityProvideable world, IWeapons weapons, INameProvideable names, IPlacesOfInterest placesOfInterest, IModItems modItems, IAgencies agencies)
    {
        Player = player;
        LEDispatcher = lEDispatcher;
        Settings = settings;
        Weapons = weapons;
        Names = names;
        PlacesOfInterest = placesOfInterest;
        ModItems = modItems;
        World = world;
        Agencies = agencies;
        GameTimeNotWantedToAttemptDispatch = RandomItems.GetRandomNumber(Settings.SettingsManager.PoliceSettings.MinTimeOfAPBBetweenMarshalsAPBResponse, Settings.SettingsManager.PoliceSettings.MaxTimeOfAPBBetweenMarshalsAPBResponse);
        TimeBetweenMarshals = RandomItems.GetRandomNumber(Settings.SettingsManager.PoliceSettings.MinTimeBetweenMarshalsAPBResponse, Settings.SettingsManager.PoliceSettings.MaxTimeBetweenMarshalsAPBResponse);
    }

    public bool Dispatch()
    {
        HasDispatchedThisTick = false;
        CheckPlayerStatus();
        HandleMarshalsSpawn();
        return HasDispatchedThisTick;
    }
    public void Dispose()
    {

    }
    private void CheckPlayerStatus()
    {
        SpawnedMarshalls.RemoveAll(x => !x.Pedestrian.Exists());
        bool canDispatch = false;
        if(Player.CriminalHistory.HasDeadlyHistory && !Player.IsWanted && Player.CriminalHistory.IsWithinMarshalDistance)
        {
            canDispatch = true;
        }
        if(canDispatch != CanDispatch)
        {
            if(canDispatch)
            {
                OnSpawningValid();
            }
            else
            {
                OnSpawningInvalid();
            }
            EntryPoint.WriteToConsole($"Marshal Dispatcher CanDispatch changed from {CanDispatch} to {canDispatch}");
            CanDispatch = canDispatch;
        }
    }

    private void OnSpawningInvalid()
    {

    }
    private void OnSpawningValid()
    {
        GameTimeNotWantedToAttemptDispatch = RandomItems.GetRandomNumber(Settings.SettingsManager.PoliceSettings.MinTimeOfAPBBetweenMarshalsAPBResponse, Settings.SettingsManager.PoliceSettings.MaxTimeOfAPBBetweenMarshalsAPBResponse);
        EntryPoint.WriteToConsole($"Marshal Dispatcher GameTimeNotWantedToAttemptDispatch {GameTimeNotWantedToAttemptDispatch}");
    }
    private void HandleMarshalsSpawn()
    {
        if (!Settings.SettingsManager.PoliceSettings.AllowMarshalsAPBResponse || !IsTimeToDispatchMarshals || Player.IsWanted || !Player.CriminalHistory.HasDeadlyHistory || !HasNeedToDispatch || !Player.CriminalHistory.IsWithinMarshalDistance)
        {
            return;
        }
        if(Player.PoliceResponse.HasBeenNotWantedFor < GameTimeNotWantedToAttemptDispatch)
        {
            return;
        }
        if(SpawnedMarshalls.Count > 4)
        {
            return;
        }
        EntryPoint.WriteToConsole($"Marshal Dispatcher Attempting Dispatch");
        DispatchMarshals();
        TimeBetweenMarshals = RandomItems.GetRandomNumber(Settings.SettingsManager.PoliceSettings.MinTimeBetweenMarshalsAPBResponse, Settings.SettingsManager.PoliceSettings.MaxTimeBetweenMarshalsAPBResponse);

        HasDispatchedThisTick = true;
    }
    private void DispatchMarshals()
    {
        EntryPoint.WriteToConsole($"DispatchMarshals Attempting to Dispatch");
        bool HasSpawn = GetFarVehicleSpawnLocation();
        bool HasTypes = GetMarshalTypes();
        if (!HasSpawn || !HasTypes)
        {
            EntryPoint.WriteToConsole($"DispatchMarshals FAIL HasSpawn{HasSpawn} HasTypes{HasTypes}");
            return;
        }
        EntryPoint.WriteToConsole($"DispatchMarshals Agency {Agency?.ShortName}");
        if (!CallSpawnTask(false, true, false, false, TaskRequirements.None, true))
        {
            return;
        }
        GameTimeLastDispatchedMarshals = Game.GameTime;
        Player.OnMarshalsDispatched(Agency);
    }
    private bool CallSpawnTask(bool allowAny, bool allowBuddy, bool isLocationSpawn, bool clearArea, TaskRequirements spawnRequirement, bool forcek9)
    {
        try
        {
            GameFiber.Yield();
            bool addOptionalPassengers = true;// RandomItems.RandomPercent(Settings.SettingsManager.PoliceSpawnSettings.AddOptionalPassengerPercentage);
            bool addCanine = false;// && RandomItems.RandomPercent(Settings.SettingsManager.PoliceSpawnSettings.AddK9Percentage);
            if (forcek9)
            {
                addCanine = true;
            }
            LESpawnTask spawnTask = new LESpawnTask(Agency, SpawnLocation, VehicleType, PersonType, Settings.SettingsManager.PoliceSpawnSettings.ShowSpawnedBlips, Settings, Weapons, Names, addOptionalPassengers, World, ModItems, forcek9);
            spawnTask.AllowAnySpawn = allowAny;
            spawnTask.AllowBuddySpawn = allowBuddy;
            spawnTask.ClearVehicleArea = clearArea;
            spawnTask.IsMarshalMember = true;
            spawnTask.SpawnRequirement = spawnRequirement;
            // spawnTask.PlacePedOnGround = VehicleType == null;
            spawnTask.AttemptSpawn();
            GameFiber.Yield();
            spawnTask.CreatedPeople.ForEach(x => { World.Pedestrians.AddEntity(x); x.IsLocationSpawned = isLocationSpawn;SpawnedMarshalls.Add(x); });
            spawnTask.CreatedVehicles.ForEach(x => x.AddVehicleToList(World));//World.Vehicles.AddEntity(x, ResponseType.LawEnforcement));
            HasDispatchedThisTick = true;
            Player.OnLawEnforcementSpawn(Agency, VehicleType, PersonType);
            return spawnTask.CreatedPeople.Any(x => x.Pedestrian.Exists());
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"LE Dispatcher Spawn Error: {ex.Message} : {ex.StackTrace}", 0);
            return false;
        }
    }
    private bool GetFarVehicleSpawnLocation()
    {
        int timesTried = 0;
        bool isValidSpawn;
        SpawnLocation = new SpawnLocation();
        do
        {
            SpawnLocation.InitialPosition = GetPositionAroundPlayer();
            SpawnLocation.GetClosestStreet(false);
            //SpawnLocation.GetClosestSidewalk();
            GameFiber.Yield();
            isValidSpawn = AreSpawnsValidSpawn(SpawnLocation);
            timesTried++;
        }
        while (!SpawnLocation.HasSpawns && !isValidSpawn && timesTried < 2);//10
        return isValidSpawn && SpawnLocation.HasSpawns;
    }
    private bool GetMarshalTypes()
    {
        VehicleType = null;
        PersonType = null;
        List<Agency> possibleAgencies = Agencies.GetSpawnableAgencies(3, ResponseType.LawEnforcement);
        possibleAgencies.RemoveAll(x => x.Classification != Classification.Marshal);
        Agency = possibleAgencies.PickRandom();
        if (Agency == null)
        {
            EntryPoint.WriteToConsole("DispatchMarshals NO AGENCY");
            return false;
        }
        VehicleType = Agency.GetRandomVehicle(3, false, false, true, "", Settings);
        GameFiber.Yield();
        string RequiredGroup = "";
        if (VehicleType != null)
        {
            RequiredGroup = VehicleType.RequiredPedGroup;
        }
        PersonType = Agency.GetRandomPed(3, RequiredGroup);
        GameFiber.Yield();
        if (PersonType != null)
        {  
            return true;
        }
        EntryPoint.WriteToConsole("DispatchMarshals NO PERSON");
        return false;
    }
    private Vector3 GetPositionAroundPlayer()
    {
        Vector3 Position;
        Position = Player.Position;
        Position = Position.Around2D(Settings.SettingsManager.PoliceSpawnSettings.MinDistanceToSpawn_NotWanted, Settings.SettingsManager.PoliceSpawnSettings.MaxDistanceToSpawn_NotWanted);
        return Position;
    }
    private bool AreSpawnsValidSpawn(SpawnLocation spawnLocation)
    {
        if (spawnLocation.StreetPosition.DistanceTo2D(Player.Position) < LEDispatcher.ClosestPoliceSpawnToSuspectAllowed)
        {
            return false;
        }
        else if (spawnLocation.InitialPosition.DistanceTo2D(Player.Position) < LEDispatcher.ClosestPoliceSpawnToSuspectAllowed)
        {
            return false;
        }
        return true;
    }
}

