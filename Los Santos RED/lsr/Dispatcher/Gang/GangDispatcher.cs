using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

public class GangDispatcher
{
    private readonly IGangs Gangs;
    private readonly IDispatchable Player;

    private readonly float MinimumDeleteDistance = 150f;//200f
    private readonly uint MinimumExistingTime = 20000;
    private readonly ISettingsProvideable Settings;
    private readonly IStreets Streets;
    private readonly IEntityProvideable World;
    private readonly IZones Zones;
    private uint GameTimeAttemptedDispatch;
    private uint GameTimeAttemptedRecall;
    private bool HasDispatchedThisTick;
    private IWeapons Weapons;
    private INameProvideable Names;
    private IGangTerritories GangTerritories;
    private IPedGroups PedGroups;
    private ICrimes Crimes;
    private IShopMenus ShopMenus;
    private IPlacesOfInterest PlacesOfInterest;
    private GangDen GangDen;
    private bool IsDenSpawn;
    private SpawnLocation SpawnLocation;
    private Gang Gang;
    private DispatchableVehicle VehicleType;
    private DispatchablePerson PersonType;
    public GangDispatcher(IEntityProvideable world, IDispatchable player, IGangs gangs, ISettingsProvideable settings, IStreets streets, IZones zones, IGangTerritories gangTerritories, IWeapons weapons, INameProvideable names, IPedGroups pedGroups, ICrimes crimes, IShopMenus shopMenus, IPlacesOfInterest placesOfInterest)
    {
        Player = player;
        World = world;
        Gangs = gangs;
        Settings = settings;
        Streets = streets;
        Zones = zones;
        GangTerritories = gangTerritories;
        Weapons = weapons;
        Names = names;
        PedGroups = pedGroups;
        Crimes = crimes;
        ShopMenus = shopMenus;
        PlacesOfInterest = placesOfInterest;
    }
    private float ClosestGangSpawnToPlayerAllowed => 45f;
    private List<GangMember> DeleteableGangMembers => World.Pedestrians.GangMemberList.Where(x => (x.RecentlyUpdated && x.DistanceToPlayer >= MinimumDeleteDistance && x.HasBeenSpawnedFor >= MinimumExistingTime) || x.CanRemove).ToList();
    private float DistanceToDelete => 300f;
    private float DistanceToDeleteOnFoot => 250f;
    private bool HasNeedToDispatch => World.Pedestrians.TotalSpawnedGangMembers <= Settings.SettingsManager.GangSettings.TotalSpawnedMembersLimit && (Settings.SettingsManager.GangSettings.AllowAmbientSpawningWhenPlayerWanted || Player.IsNotWanted);// && (Settings.SettingsManager.GangSettings.AllowDenSpawningWhenPlayerWanted || Player.IsNotWanted);//not wanted is new, do i need to spawn in more peds when ur alreadywanted?


    private bool HasNeedToDispatchToDens => Settings.SettingsManager.GangSettings.AllowDenSpawning && (Settings.SettingsManager.GangSettings.AllowDenSpawningWhenPlayerWanted || Player.IsNotWanted);

    private bool IsTimeToDispatch => Game.GameTime - GameTimeAttemptedDispatch >= TimeBetweenSpawn;//15000;
    private bool IsTimeToRecall => Game.GameTime - GameTimeAttemptedRecall >= TimeBetweenSpawn;
    private float MaxDistanceToSpawn => Settings.SettingsManager.GangSettings.MaxDistanceToSpawn;//150f;
    private float MinDistanceToSpawn => Settings.SettingsManager.GangSettings.MinDistanceToSpawn;//50f;
    private int TimeBetweenSpawn => Settings.SettingsManager.GangSettings.TimeBetweenSpawn;//15000;
    public int LikelyHoodOfAnySpawn => Settings.SettingsManager.GangSettings.PercentSpawnOutsideTerritory;
    public int LikelyHoodOfDenSpawnWhenNear => Settings.SettingsManager.GangSettings.PercentageSpawnNearDen;
    public bool Dispatch()
    {
        HasDispatchedThisTick = false;
        if (Settings.SettingsManager.GangSettings.ManageDispatching )
        {
            HandleAmbientSpawns();
            HandleDenSpawns();
        }
        return HasDispatchedThisTick;
    }

    public void LocationDispatch()
    {
        if (Settings.SettingsManager.GangSettings.ManageDispatching)
        {
            HandleDenSpawns();
        }
    }

    public void Dispose()
    {

    }
    public void Recall()
    {
        if (IsTimeToRecall)
        {
            foreach (GangMember emt in DeleteableGangMembers)
            {
                if (ShouldBeRecalled(emt))
                {
                    Delete(emt);
                    GameFiber.Yield();
                }
            }
            GameTimeAttemptedRecall = Game.GameTime;
        }
    }
    private void HandleAmbientSpawns()
    {
        if (IsTimeToDispatch && HasNeedToDispatch)
        {
            HasDispatchedThisTick = true;//up here for now, might be better down low
            if (GetSpawnLocation() && GetSpawnTypes(false,false, null))
            {
                CallSpawnTask(false, true, false, false);
            }
            GameTimeAttemptedDispatch = Game.GameTime;
        }
    }
    private void HandleDenSpawns()
    {
        if (!HasDispatchedThisTick && HasNeedToDispatchToDens)
        {
            foreach (GangDen ps in PlacesOfInterest.PossibleLocations.GangDens.Where(x => x.IsNearby && !x.IsDispatchFilled && x.EntrancePosition.DistanceTo(Game.LocalPlayer.Character) <= 200f))
            {
                if (ps.PossiblePedSpawns != null || ps.PossibleVehicleSpawns != null)
                {
                    bool spawnedsome = false;
                    if (ps.PossiblePedSpawns != null)
                    {
                        foreach (ConditionalLocation cl in ps.PossiblePedSpawns)
                        {
                            if (RandomItems.RandomPercent(cl.Percentage) && (Settings.SettingsManager.GangSettings.DenSpawningIgnoresLimits || HasNeedToDispatch))
                            {
                                HasDispatchedThisTick = true;
                                SpawnLocation = new SpawnLocation(cl.Location);
                                SpawnLocation.Heading = cl.Heading;
                                SpawnLocation.StreetPosition = cl.Location;
                                if (GetSpawnTypes(true, false, ps.AssociatedGang))
                                {
                                    CallSpawnTask(true, false, true, false);
                                    spawnedsome = true;
                                    HasDispatchedThisTick = true;
                                }
                            }
                        }
                        
                    }
                    if (ps.PossibleVehicleSpawns != null)
                    {
                        foreach (ConditionalLocation cl in ps.PossibleVehicleSpawns)
                        {
                            if (RandomItems.RandomPercent(cl.Percentage) && (Settings.SettingsManager.GangSettings.DenSpawningIgnoresLimits || HasNeedToDispatch))
                            {
                                HasDispatchedThisTick = true;
                                SpawnLocation = new SpawnLocation(cl.Location);
                                SpawnLocation.Heading = cl.Heading;
                                SpawnLocation.StreetPosition = cl.Location;
                                SpawnLocation.SidewalkPosition = cl.Location;
                                if (GetSpawnTypes(false, true, ps.AssociatedGang))
                                {
                                    CallSpawnTask(true, false, true, true);
                                    spawnedsome = true;
                                }
                            }
                        }
                    }



                    ps.IsDispatchFilled = true;
                }
                else
                {
                    ps.IsDispatchFilled = true;
                }
            }
            foreach (GangDen ps in PlacesOfInterest.PossibleLocations.GangDens.Where(x => !x.IsNearby && x.IsDispatchFilled))
            {
                ps.IsDispatchFilled = false;
            }
        }
    }
    private bool GetSpawnLocation()
    {
        int timesTried = 0;
        bool isValidSpawn;
        GangDen = null;
        IsDenSpawn = false;
        SpawnLocation = new SpawnLocation();
        do
        {
            if (RandomItems.RandomPercent(LikelyHoodOfDenSpawnWhenNear))
            {
                GangDen = PlacesOfInterest.PossibleLocations.GangDens.Where(x => x.IsNearby).PickRandom();
            }
            if (GangDen != null)
            {
                float DistanceTo = GangDen.EntrancePosition.DistanceTo2D(Game.LocalPlayer.Character);
                if (DistanceTo >= 45f)
                {
                    IsDenSpawn = true;
                    SpawnLocation.InitialPosition = GangDen.EntrancePosition.Around2D(50f);
                }
                else
                {
                    GangDen = null;
                    SpawnLocation.InitialPosition = GetPositionAroundPlayer();
                }
            }
            else
            {
                SpawnLocation.InitialPosition = GetPositionAroundPlayer();
            }
            SpawnLocation.GetClosestStreet();
            SpawnLocation.GetClosestSidewalk();
            GameFiber.Yield();
            isValidSpawn = IsValidSpawn(SpawnLocation);
            timesTried++;
        }
        while (!SpawnLocation.HasSpawns && !isValidSpawn && timesTried < 2);//10
        return isValidSpawn && SpawnLocation.HasSpawns;
    }
    private bool GetSpawnTypes(bool forcePed, bool forceVehicle, Gang forceGang)
    {
        Gang = null;
        VehicleType = null;
        PersonType = null;

        if(forceGang != null)
        {
            Gang = forceGang;
        }
        else if (IsDenSpawn && GangDen != null)
        {
            Gang = GangDen.AssociatedGang;
        }
        else
        {
            Gang = GetRandomGang(SpawnLocation);
        }




        if (Gang != null)
        {
            if(forcePed)
            {
                PersonType = Gang.GetRandomPed(Player.WantedLevel, "");
                VehicleType = null;
                return PersonType != null;
            }
            else if (forceVehicle)
            {
                PersonType = null;
                VehicleType = Gang.GetRandomVehicle(Player.WantedLevel, false, false, true);
                return VehicleType != null;
            }
            else
            {
                if (World.Pedestrians.GangMemberList.Count(x => x.Gang?.ID == Gang.ID) >= Gang.SpawnLimit)
                {
                    return false;
                }
                else
                {
                    if (IsDenSpawn && RandomItems.RandomPercent(80))
                    {
                        VehicleType = null;
                    }
                    else if (!SpawnLocation.HasSidewalk || RandomItems.RandomPercent(Gang.VehicleSpawnPercentage))
                    {
                        VehicleType = Gang.GetRandomVehicle(Player.WantedLevel, false, false, true);
                    }

                    if (VehicleType != null || SpawnLocation.HasSidewalk || IsDenSpawn)
                    {
                        string RequiredGroup = "";
                        if (VehicleType != null)
                        {
                            RequiredGroup = VehicleType.RequiredPedGroup;
                        }
                        PersonType = Gang.GetRandomPed(Player.WantedLevel, RequiredGroup);
                        if (PersonType != null)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
    private void CallSpawnTask(bool allowAny, bool allowBuddy, bool isAmbientSpawn, bool clearArea)
    {
        try
        {
            GangSpawnTask gangSpawnTask = new GangSpawnTask(Gang, SpawnLocation, VehicleType, PersonType, Settings.SettingsManager.GangSettings.ShowSpawnedBlip, Settings, Weapons, Names, true, Crimes, PedGroups, ShopMenus, World);// Settings.SettingsManager.Police.SpawnedAmbientPoliceHaveBlip);
            gangSpawnTask.AllowAnySpawn = allowAny;
            gangSpawnTask.AllowBuddySpawn = allowBuddy;
            gangSpawnTask.AttemptSpawn();
            foreach (PedExt created in gangSpawnTask.CreatedPeople)
            {
                World.Pedestrians.AddEntity(created);
            }
            gangSpawnTask.CreatedPeople.ForEach(x => { World.Pedestrians.AddEntity(x); x.IsAmbientSpawn = isAmbientSpawn; });
            gangSpawnTask.CreatedVehicles.ForEach(x => World.Vehicles.AddEntity(x, ResponseType.None));
            HasDispatchedThisTick = true;
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"Gang Dispatcher Spawn Error: {ex.Message} : {ex.StackTrace}", 0);
        }
    }
    private bool ShouldBeRecalled(GangMember emt)
    {
        if (emt.IsInVehicle)
        {
            return emt.DistanceToPlayer >= DistanceToDelete;
        }
        else
        {
            return emt.DistanceToPlayer >= DistanceToDeleteOnFoot;
        }
    }
    private void Delete(PedExt emt)
    {
        if (emt != null && emt.Pedestrian.Exists())
        {
            //EntryPoint.WriteToConsole($"Attempting to Delete {Cop.Pedestrian.Handle}");
            if (emt.Pedestrian.IsInAnyVehicle(false))
            {
                if (emt.Pedestrian.CurrentVehicle.HasPassengers)
                {
                    foreach (Ped Passenger in emt.Pedestrian.CurrentVehicle.Passengers)
                    {
                        if (Passenger.Handle != Game.LocalPlayer.Character.Handle)
                        {
                            RemoveBlip(Passenger);
                            Passenger.Delete();
                            EntryPoint.PersistentPedsDeleted++;
                        }
                    }
                }
                if (emt.Pedestrian.Exists() && emt.Pedestrian.CurrentVehicle.Exists() && emt.Pedestrian.CurrentVehicle != null)
                {
                    emt.Pedestrian.CurrentVehicle.Delete();
                    EntryPoint.PersistentVehiclesDeleted++;
                }
            }
            RemoveBlip(emt.Pedestrian);
            if (emt.Pedestrian.Exists())
            {
                //EntryPoint.WriteToConsole(string.Format("Delete Cop Handle: {0}, {1}, {2}", Cop.Pedestrian.Handle, Cop.DistanceToPlayer, Cop.AssignedAgency.Initials));
                emt.Pedestrian.Delete();
                EntryPoint.PersistentPedsDeleted++;
            }
        }
    }
    private void RemoveBlip(Ped emt)
    {
        if (!emt.Exists())
        {
            return;
        }
        Blip MyBlip = emt.GetAttachedBlip();
        if (MyBlip.Exists())
        {
            MyBlip.Delete();
        }
    }
    private List<Gang> GetGangs(Vector3 Position, int WantedLevel)
    {
        List<Gang> ToReturn = new List<Gang>();
        Zone CurrentZone = Zones.GetZone(Position);
        Gang ZoneAgency = GangTerritories.GetRandomGang(CurrentZone.InternalGameName, WantedLevel);
        if (ZoneAgency != null)
        {
            ToReturn.Add(ZoneAgency);
        }
        if (!ToReturn.Any() || RandomItems.RandomPercent(LikelyHoodOfAnySpawn))//fall back to anybody
        {
            ToReturn.Clear();
            ToReturn.AddRange(Gangs.GetSpawnableGangs(WantedLevel));
        }
        return ToReturn;
    }
    private Vector3 GetPositionAroundPlayer()
    {
        Vector3 Position;
        Position = Player.Position;
        Position = Position.Around2D(MinDistanceToSpawn, MaxDistanceToSpawn);
        return Position;
    }
    private Gang GetRandomGang(SpawnLocation spawnLocation)
    {
        Gang Gang;
        List<Gang> PossibleGangs = GetGangs(spawnLocation.StreetPosition, Player.WantedLevel);
        Gang = PossibleGangs.PickRandom();
        if (Gang == null)
        {
            Gang = GetGangs(spawnLocation.InitialPosition, Player.WantedLevel).PickRandom();
        }
        if (Gang == null)
        {
            //EntryPoint.WriteToConsole("Dispatcher could not find Agency To Spawn");
        }
        return Gang;
    }
    private Gang GetRandomGang(Vector3 spawnLocation)
    {
        Gang agency;
        List<Gang> PossibleAgencies = GetGangs(spawnLocation, Player.WantedLevel);
        agency = PossibleAgencies.PickRandom();
        if (agency == null)
        {
            agency = GetGangs(spawnLocation, Player.WantedLevel).PickRandom();
        }
        if (agency == null)
        {
            //EntryPoint.WriteToConsole("Dispatcher could not find Agency To Spawn");
        }
        return agency;
    }
    private bool IsValidSpawn(SpawnLocation spawnLocation)
    {
        if (spawnLocation.StreetPosition.DistanceTo2D(Player.Position) < ClosestGangSpawnToPlayerAllowed)
        {
            return false;
        }
        else if (spawnLocation.InitialPosition.DistanceTo2D(Player.Position) < ClosestGangSpawnToPlayerAllowed)
        {
            return false;
        }
        return true;
    }
    public void DebugSpawnGangMember(string gangID, bool onFoot)
    {
        VehicleType = null;
        PersonType = null;
        Gang = null;
        SpawnLocation = new SpawnLocation();
        SpawnLocation.InitialPosition = Game.LocalPlayer.Character.GetOffsetPositionFront(10f);
        SpawnLocation.StreetPosition = SpawnLocation.InitialPosition;
        if (gangID == "")
        {
            Gang = GetRandomGang(SpawnLocation);
        }
        else
        {
            Gang = Gangs.GetGang(gangID);
        }
        if (Gang == null)
        {
            return;
        }
           
        if (!onFoot)
        {
            VehicleType = Gang.GetRandomVehicle(Player.WantedLevel, false, false, true);
        }
        if (VehicleType != null || onFoot)
        {
            string RequiredGroup = "";
            if (VehicleType != null)
            {
                RequiredGroup = VehicleType.RequiredPedGroup;
            }
            PersonType = Gang.GetRandomPed(Player.WantedLevel, RequiredGroup);
        }
        CallSpawnTask(true, true, false, false);
    }
    
}