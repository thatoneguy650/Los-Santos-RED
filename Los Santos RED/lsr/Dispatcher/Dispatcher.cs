using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Dispatcher
{
    private uint GameTimeStartedDispatch;
    private readonly IAgencies Agencies;
    private readonly IDispatchable Player;
    private readonly ISettingsProvideable Settings;
    private readonly IStreets Streets;
    private readonly IEntityProvideable World;
    private readonly IJurisdictions Jurisdictions;
    private readonly IZones Zones;

    private IWeapons Weapons;
    private INameProvideable Names;
    private IWeatherReportable WeatherReporter;
    private ITimeControllable Time;
    private IOrganizations Organizations;
    private ICrimes Crimes;
    private IPedGroups PedGroups;
    private IGangs Gangs;
    private IGangTerritories GangTerritories;
    private IShopMenus ShopMenus;
    private IPlacesOfInterest PlacesOfInterest;
    private IModItems ModItems;
    private IInteriors Interiors;


    public LEDispatcher LEDispatcher { get; private set; }
    public EMSDispatcher EMSDispatcher { get; private set; }
    public FireDispatcher FireDispatcher { get; private set; }
    public ZombieDispatcher ZombieDispatcher { get; private set; }
    public SecurityDispatcher SecurityDispatcher { get; private set; }
    public GangDispatcher GangDispatcher { get; private set; }
    public LocationDispatcher LocationDispatcher { get; private set; }
    public TaxiDispatcher TaxiDispatcher { get; private set; }

    public Dispatcher(IEntityProvideable world, IDispatchable player, IAgencies agencies, ISettingsProvideable settings, IStreets streets, IZones zones, IJurisdictions jurisdictions, IWeapons weapons, INameProvideable names, ICrimes crimes,
        IPedGroups pedGroups, IGangs gangs, IGangTerritories gangTerritories, IShopMenus shopMenus, IPlacesOfInterest placesOfInterest, IWeatherReportable weatherReporter, ITimeControllable time, IModItems modItems, 
        IOrganizations organizations, IInteriors interiors)
    {
        Player = player;
        World = world;
        Agencies = agencies;
        Settings = settings;
        Streets = streets;
        Zones = zones;
        Jurisdictions = jurisdictions;
        Weapons = weapons;
        Names = names;
        Crimes = crimes;
        Gangs = gangs;
        PedGroups = pedGroups;
        GangTerritories = gangTerritories;
        ShopMenus = shopMenus;
        PlacesOfInterest = placesOfInterest;
        WeatherReporter = weatherReporter;
        Time = time;
        ModItems = modItems;
        Organizations = organizations;
        Interiors = interiors;
    }
    public void Setup()
    {
        GameTimeStartedDispatch = Game.GameTime;
        LEDispatcher = new LEDispatcher(World, Player, Agencies, Settings, Streets, Zones, Jurisdictions, Weapons, Names, PlacesOfInterest, ModItems);
        EMSDispatcher = new EMSDispatcher(World, Player, Agencies, Settings, Streets, Zones, Jurisdictions, Weapons, Names, PlacesOfInterest, ModItems);
        SecurityDispatcher = new SecurityDispatcher(World, Player, Agencies, Settings, Streets, Zones, Jurisdictions, Weapons, Names, PlacesOfInterest, Crimes, ModItems);
        FireDispatcher = new FireDispatcher(World, Player, Agencies, Settings, Streets, Zones, Jurisdictions, Weapons, Names, PlacesOfInterest, ModItems, ShopMenus);
        ZombieDispatcher = new ZombieDispatcher(World, Player, Settings, Streets, Zones, Jurisdictions, Weapons, Names, Crimes);
        GangDispatcher = new GangDispatcher(World, Player, Gangs, Settings, Streets, Zones, GangTerritories, Weapons, Names, PedGroups, Crimes, ShopMenus, PlacesOfInterest, ModItems);
        LocationDispatcher = new LocationDispatcher(World, Player, Gangs, Settings, Streets, Zones, GangTerritories, Weapons, Names, PedGroups, Crimes, ShopMenus, PlacesOfInterest, Agencies, Jurisdictions, WeatherReporter, Time, ModItems, Interiors);
        TaxiDispatcher = new TaxiDispatcher(World, Player, Agencies,Settings, Streets,Zones,Jurisdictions,Weapons,Names,PlacesOfInterest,Organizations,Crimes,ModItems,ShopMenus);
    }
    public void Dispatch()
    {
        if(Player.IsCustomizingPed)
        {
            return;
        }
        if(Player.RecentlyStartedPlaying)
        {
            //EntryPoint.WriteToConsole("RECENTLY STARTED PLAYING NO DISPTACH");
            return;
        }
        if(Game.GameTime - GameTimeStartedDispatch <= 8000)
        {
            //EntryPoint.WriteToConsole("RECENTLY STARTED PLAYING222 NO DISPTACH");
            return;
        }
        GameFiber.Yield();
        if (!EntryPoint.ModController.IsRunning)
        {
            return;
        }

        //int vehicleCount = World.Vehicles.AllVehicleList.Count();
        //int pedCount = World.Pedestrians.PedExts.Count();
        //EntryPoint.WriteToConsole($"VehicleCount:{vehicleCount} PedCount:{pedCount}");
        //if (vehicleCount >= Settings.SettingsManager.WorldSettings.MaxVehiclesBeforeDispatchPause)//75)
        //{
        //    EntryPoint.WriteToConsole($"TOO MANY VEHICLES {vehicleCount} NOT DISPATCHING MAX {Settings.SettingsManager.WorldSettings.MaxVehiclesBeforeDispatchPause}");
        //    return;
        //}
        //if (pedCount >= Settings.SettingsManager.WorldSettings.MaxPedsBeforeDispatchPause)// 75)
        //{
        //    EntryPoint.WriteToConsole($"TOO MANY PEDESTRIANS {pedCount} NOT DISPATCHING MAX {Settings.SettingsManager.WorldSettings.MaxPedsBeforeDispatchPause}");
        //    return;
        //}
        if (LEDispatcher.Dispatch())
        {
            GameFiber.Yield();
        }
        if (!EntryPoint.ModController.IsRunning)
        {
            return;
        }
        if (EMSDispatcher.Dispatch())
        {
            GameFiber.Yield();
        }
        if (!EntryPoint.ModController.IsRunning)
        {
            return;
        }
        if (FireDispatcher.Dispatch())
        {
            GameFiber.Yield();
        }
        if (!EntryPoint.ModController.IsRunning)
        {
            return;
        }
        if (SecurityDispatcher.Dispatch())
        {
            GameFiber.Yield();
        }
        GameFiber.Yield();
        if (!EntryPoint.ModController.IsRunning)
        {
            return;
        }
        LocationDispatcher.Dispatch();
        GameFiber.Yield();
        if (!EntryPoint.ModController.IsRunning)
        {
            return;
        }
        GangDispatcher.Dispatch();
        GameFiber.Yield();
        if (!EntryPoint.ModController.IsRunning)
        {
            return;
        }
        if (World.IsZombieApocalypse)
        {
            GameFiber.Yield();
            if (!EntryPoint.ModController.IsRunning)
            {
                return;
            }
            ZombieDispatcher.Dispatch();
        }
        GameFiber.Yield();
        if (!EntryPoint.ModController.IsRunning)
        {
            return;
        }
        TaxiDispatcher.Dispatch();
    }
    public void Recall()
    {
        LEDispatcher.Recall();
        EMSDispatcher.Recall();
        GameFiber.Yield();
        FireDispatcher.Recall();
        SecurityDispatcher.Recall();
        if (World.IsZombieApocalypse)
        {
            GameFiber.Yield();
            ZombieDispatcher.Recall();
        }
        GameFiber.Yield();
        GangDispatcher.Recall();
        TaxiDispatcher.Recall();
        RecallCivilians();
    }
    private void RecallCivilians()
    {
        if (!Settings.SettingsManager.WorldSettings.CleanupVehicles)
        {
            return;
        }
        try
        {
            foreach (VehicleExt civilianCar in World.Vehicles.NonPoliceList.Where(x => !x.IsOwnedByPlayer && !x.IsManualCleanup && x.WasModSpawned  && x.HasExistedFor >= 12000 && x.Vehicle.Exists() && x.Vehicle.IsPersistent).ToList())//NonServiceVehicles//&& !x.WasSpawnedEmpty//15000
            {
                if (!civilianCar.Vehicle.Exists() || civilianCar.Vehicle.Occupants.Any(x => x.Exists() && x.IsAlive))
                {
                    continue;
                }
                float distanceTo = civilianCar.Vehicle.DistanceTo2D(Game.LocalPlayer.Character);

                civilianCar.DistanceChecker.UpdateMovement(distanceTo);


                if (civilianCar.DistanceChecker.IsMovingAway && (distanceTo >= 275f || (!civilianCar.WasSpawnedEmpty && distanceTo >= 150f)))//325f)//275f)//250f)
                {
                    //if (civilianCar.Vehicle.IsPersistent)
                    //{
                    //    EntryPoint.PersistentVehiclesDeleted++;
                    //}
                    //EntryPoint.WriteToConsole($"GENERAL DISPATCHER REMOVING EMPTY VEHICLE distanceTo{distanceTo} {civilianCar.Handle}");
                    //civilianCar.FullyDelete();
                    if (civilianCar.Vehicle.IsPersistent)
                    {
                        EntryPoint.PersistentVehiclesNonPersistent++;
                    }
                    EntryPoint.WriteToConsole($"GENERAL DISPATCHER NON PERSIST 1 EMPTY VEHICLE distanceTo{distanceTo} {civilianCar.Handle}");
                    civilianCar.Vehicle.IsPersistent = false;
                }
                else if (civilianCar.DistanceChecker.IsMovingAway && Settings.SettingsManager.WorldSettings.ExtendedVehicleCleanup && distanceTo >= 125f && !civilianCar.WasSpawnedEmpty)
                {
                    if (civilianCar.Vehicle.IsPersistent)
                    {
                        EntryPoint.PersistentVehiclesNonPersistent++;
                    }
                    EntryPoint.WriteToConsole($"GENERAL DISPATCHER NON PERSIST 2 EMPTY VEHICLE distanceTo{distanceTo} {civilianCar.Handle}");
                    civilianCar.Vehicle.IsPersistent = false;
                }
                GameFiber.Yield();
            }
        }
        catch (InvalidOperationException ex)
        {
            EntryPoint.WriteToConsole($"Remove Abandoned Vehicles, Collection Modified Error: {ex.Message} {ex.StackTrace}", 0);
        }
    }
    public void Dispose()
    {
        LEDispatcher.Dispose();
    }
    public void DebugSpawnRoadblock(bool enableCars, bool enableSpike, bool enableProps, float distance)
    {
        LEDispatcher.SpawnRoadblock(true, enableCars, enableSpike, enableProps, distance);
    }
    public void DebugRemoveRoadblock()
    {
        LEDispatcher.RemoveRoadblock();
        World.Vehicles.ClearSpawned(true);
        World.Pedestrians.ClearSpawned();
    }
    public void DebugSpawnCop()
    {
        LEDispatcher.DebugSpawnCop("",false, false, null, null, false);
    }
    public void DebugSpawnCop(string agencyID, bool onFoot, bool isEmpty, DispatchableVehicle dispatchableVehicle, DispatchablePerson dispatchablePerson)
    {
        LEDispatcher.DebugSpawnCop(agencyID, onFoot, isEmpty, dispatchableVehicle, dispatchablePerson, false);
    }
    public void DebugSpawnCop(string agencyID, bool onFoot, bool isEmpty)
    {
        LEDispatcher.DebugSpawnCop(agencyID,onFoot, isEmpty, null, null, false);
    }
    public void DebugSpawnCop(string agencyID, bool onFoot, bool isEmpty, DispatchableVehicle dvtoSpawn)
    {
        LEDispatcher.DebugSpawnCop(agencyID, onFoot, isEmpty, dvtoSpawn, null, false);
    }
    public void DebugSpawnK9Cop(string agencyID)
    {
        LEDispatcher.DebugSpawnCop(agencyID, false, false, null, null, true);
    }
    public void DebugSpawnGang()
    {
        GangDispatcher.DebugSpawnGangMember("",false, false, null, null);
    }
    public void DebugSpawnGang(string agencyID, bool onFoot, bool isEmpty)
    {
        GangDispatcher.DebugSpawnGangMember(agencyID, onFoot, isEmpty, null, null);
    }
    public void DebugSpawnTaxi(TaxiFirm taxifirmID, bool onFoot, bool isEmpty)
    {
        TaxiDispatcher.DebugSpawnTaxi(taxifirmID, onFoot, isEmpty);
    }
    public void ForceTaxiSpawn(TaxiFirm taxifirmID)
    {
        TaxiDispatcher.ForceTaxiSpawn(taxifirmID);
    }
    public bool DispatchGangBackup(Gang requestedGang, int membersToSpawn, string requiredVehicleModel)
    {
        return GangDispatcher.DispatchGangBackup(requestedGang, membersToSpawn, requiredVehicleModel);
    }
    public void DebugSpawnEMT(string agencyID, bool onFoot, bool isEmpty)
    {
        EMSDispatcher.DebugSpawnEMT(agencyID, onFoot, isEmpty, null, null);
    }
    public void DebugSpawnFire(string agencyID, bool onFoot, bool isEmpty)
    {
        FireDispatcher.DebugSpawnFire(agencyID, onFoot, isEmpty, null, null);
    }
    public void DebugSpawnSecurityGuard(string agencyID, bool onFoot, bool isEmpty)
    {
        SecurityDispatcher.DebugSpawnSecurity(agencyID, onFoot, isEmpty, null, null);
    }
    public void DebugResetLocations()
    {
        LocationDispatcher.Reset();
    }
    public void DebugSpawnHitSquad()
    {
        GangDispatcher.DebugSpawnHitSquad();
    }

}

