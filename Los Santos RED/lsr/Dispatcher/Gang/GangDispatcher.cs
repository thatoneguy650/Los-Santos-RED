using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using NAudio.Wave;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
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
    private bool ShouldRunAmbientDispatch;
    private IModItems ModItems;
    private bool IsPedestrianOnlySpawn = false;
    private uint TimeBetweenHitSquads;
    private uint GameTimeLastDispatchedHitSquad;
    private uint GameTimeLastAttemptedAssaultSpawn;

    public GangDispatcher(IEntityProvideable world, IDispatchable player, IGangs gangs, ISettingsProvideable settings, IStreets streets, IZones zones, IGangTerritories gangTerritories, IWeapons weapons, INameProvideable names, IPedGroups pedGroups, ICrimes crimes, IShopMenus shopMenus, IPlacesOfInterest placesOfInterest, IModItems modItems)
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
        ModItems = modItems;
        TimeBetweenHitSquads = RandomItems.GetRandomNumber(Settings.SettingsManager.GangSettings.MinTimeBetweenHitSquads, Settings.SettingsManager.GangSettings.MaxTimeBetweenHitSquads);
        GameTimeLastDispatchedHitSquad = Game.GameTime;
    }
    private float ClosestGangSpawnToPlayerAllowed => 45f;
    private List<GangMember> DeleteableGangMembers => World.Pedestrians.GangMemberList.Where(x => (x.RecentlyUpdated && x.DistanceToPlayer >= MinimumDeleteDistance && x.HasBeenSpawnedFor >= MinimumExistingTime) || x.CanRemove).ToList();
    private float DistanceToDeleteInVehicle => Settings.SettingsManager.GangSettings.MaxDistanceToSpawnInVehicle + 150f;// 500 + 150 = 650;
    private float DistanceToDeleteOnFoot => Settings.SettingsManager.GangSettings.MaxDistanceToSpawnOnFoot + 50f;// 225 + 50f grace = 275f;
    private bool IsTimeToAmbientDispatch => Game.GameTime - GameTimeAttemptedDispatch >= TimeBetweenSpawn;//15000;
    private bool IsTimeToRecall => Game.GameTime - GameTimeAttemptedRecall >= 5000;// TimeBetweenSpawn;
    private float MaxDistanceToSpawnOnFoot => Settings.SettingsManager.GangSettings.MaxDistanceToSpawnOnFoot;//150f;
    private float MinDistanceToSpawnOnFoot => Settings.SettingsManager.GangSettings.MinDistanceToSpawnOnFoot;//50f;
    private float MaxDistanceToSpawnInVehicle => Settings.SettingsManager.GangSettings.MaxDistanceToSpawnInVehicle;//150f;
    private float MinDistanceToSpawnInVehicle => Settings.SettingsManager.GangSettings.MinDistanceToSpawnInVehicle;//50f;
    private bool IsTimeToDispatchHitSquad => Game.GameTime - GameTimeLastDispatchedHitSquad >= TimeBetweenHitSquads;
    private bool HasNeedToAmbientDispatch
    {
        get
        {
            if (World.Pedestrians.TotalSpawnedGangMembers >= Settings.SettingsManager.GangSettings.TotalSpawnedMembersLimit)
            {
                return false;
            }
            //if (World.Pedestrians.TotalSpawnedGangMembers > Settings.SettingsManager.GangSettings.TotalSpawnedAmbientMembersLimit)
            //{
            //    return false;
            //}
            if(!Settings.SettingsManager.GangSettings.AllowAmbientSpawningWhenPlayerWanted && Player.IsWanted)
            {
                return false;
            }
            if(Settings.SettingsManager.GangSettings.AllowAmbientSpawningWhenPlayerWanted && Player.WantedLevel > Settings.SettingsManager.GangSettings.AmbientSpawningWhenPlayerWantedMaxWanted)
            {
                return false;
            }
            if(World.Pedestrians.TotalSpawnedAmbientGangMembers > AmbientMemberLimitForZoneType)
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
            int AmbientMemberLimit = Settings.SettingsManager.GangSettings.TotalSpawnedAmbientMembersLimit;
            if (EntryPoint.FocusZone?.Type == eLocationType.Wilderness)
            {
                AmbientMemberLimit = Settings.SettingsManager.GangSettings.TotalSpawnedAmbientMembersLimit_Wilderness;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Rural)
            {
                AmbientMemberLimit = Settings.SettingsManager.GangSettings.TotalSpawnedAmbientMembersLimit_Rural;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Suburb)
            {
                AmbientMemberLimit = Settings.SettingsManager.GangSettings.TotalSpawnedAmbientMembersLimit_Suburb;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Industrial)
            {
                AmbientMemberLimit = Settings.SettingsManager.GangSettings.TotalSpawnedAmbientMembersLimit_Industrial;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Downtown)
            {
                AmbientMemberLimit = Settings.SettingsManager.GangSettings.TotalSpawnedAmbientMembersLimit_Downtown;
            }
            return AmbientMemberLimit;
        }
    }
    private int TimeBetweenSpawn// => Settings.SettingsManager.GangSettings.TimeBetweenSpawn;//15000;
    {
        get
        {
            int TotalTimeBetweenSpawns = Settings.SettingsManager.GangSettings.TimeBetweenSpawn;
            if (EntryPoint.FocusZone?.Type == eLocationType.Wilderness)
            {
                TotalTimeBetweenSpawns += Settings.SettingsManager.GangSettings.TimeBetweenSpawn_WildernessAdditional;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Rural)
            {
                TotalTimeBetweenSpawns += Settings.SettingsManager.GangSettings.TimeBetweenSpawn_RuralAdditional;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Suburb)
            {
                TotalTimeBetweenSpawns += Settings.SettingsManager.GangSettings.TimeBetweenSpawn_SuburbAdditional;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Industrial)
            {
                TotalTimeBetweenSpawns += Settings.SettingsManager.GangSettings.TimeBetweenSpawn_IndustrialAdditional;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Downtown)
            {
                TotalTimeBetweenSpawns += Settings.SettingsManager.GangSettings.TimeBetweenSpawn;
            }
            return TotalTimeBetweenSpawns;
        }
    }
    private int PercentageOfAmbientSpawn // => Settings.SettingsManager.GangSettings.TimeBetweenSpawn;//15000;
    {
        get
        {
            int ambientSpawnPercent = Settings.SettingsManager.GangSettings.AmbientSpawnPercentage;
            if (EntryPoint.FocusZone?.Type == eLocationType.Wilderness)
            {
                ambientSpawnPercent = Settings.SettingsManager.GangSettings.AmbientSpawnPercentage_Wilderness;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Rural)
            {
                ambientSpawnPercent = Settings.SettingsManager.GangSettings.AmbientSpawnPercentage_Rural;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Suburb)
            {
                ambientSpawnPercent = Settings.SettingsManager.GangSettings.AmbientSpawnPercentage_Suburb;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Industrial)
            {
                ambientSpawnPercent = Settings.SettingsManager.GangSettings.AmbientSpawnPercentage_Industrial;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Downtown)
            {
                ambientSpawnPercent = Settings.SettingsManager.GangSettings.AmbientSpawnPercentage_Downtown;
            }
            return ambientSpawnPercent;
        }
    }
    public int LikelyHoodOfAnySpawn => Settings.SettingsManager.GangSettings.PercentSpawnOutsideTerritory;
    public int LikelyHoodOfDenSpawnWhenNear => Settings.SettingsManager.GangSettings.PercentageSpawnNearDen;
    private bool HasNeedToSpawnHeli => World.Vehicles.GangHelicoptersCount < Settings.SettingsManager.GangSettings.HeliSpawnLimit_Default;
    public bool Dispatch()
    {      
        HasDispatchedThisTick = false;
        if(!Settings.SettingsManager.GangSettings.ManageDispatching)
        {
            return false;
        }
        HandleAmbientSpawns();
        HandleHitSquadSpawns();
        HandleAssaultSpawns();
       // EntryPoint.WriteToConsole($"GANG DISPATCHER IsTimeToDispatch:{IsTimeToDispatch} GameTimeSinceDispatch:{Game.GameTime - GameTimeAttemptedDispatch} HasNeedToDispatch:{HasNeedToDispatch} TotalGangMembers:{World.Pedestrians.TotalSpawnedGangMembers} AmbientMemberLimitForZoneType:{AmbientMemberLimitForZoneType} TimeBetweenSpawn:{TimeBetweenSpawn} HasNeedToDispatchToDens:{HasNeedToDispatchToDens} PercentageOfAmbientSpawn:{PercentageOfAmbientSpawn}");
        return HasDispatchedThisTick;
    }

    private void HandleAssaultSpawns()
    {
        bool shouldAttempt = GameTimeLastAttemptedAssaultSpawn == 0 || Game.GameTime - GameTimeLastAttemptedAssaultSpawn >= 9000;
        if (!shouldAttempt)
        {
            return;
        }
        if (Player.IsDead)// || Player.IsInVehicle)
        {
            //EntryPoint.WriteToConsole("Assault Spawn failed NOT NEEDED");
            return;
        }
        GameFiber.Yield();
        //GameTimeLastAttemptedAssaultSpawn = Game.GameTime;
        GangDen closestDen = null;
        float closestDistance = 999f;
        GangDen fallbackCloseestDen = null;
        bool RecentlyAttacked = false;
        bool isHitSquad = true;
        bool IsGeneralBackup = false;
        foreach (GangDen gangDen in PlacesOfInterest.PossibleLocations.GangDens.Where(x => x.DistanceToPlayer <= 150f && x.IsEnabled && x.IsActivated && x.AssociatedGang != null))
        {
            GangReputation gr1 = Player.RelationshipManager.GangRelationships.GetReputation(gangDen.AssociatedGang);
            if (gr1 != null && gr1.RecentlyAttacked)
            {
                closestDen = gangDen;
                RecentlyAttacked = true;
                break;
            }
            if (Settings.SettingsManager.GangSettings.AllowBackupAssaultSpawns && Player.RelationshipManager.GangRelationships.CurrentGang != null && gangDen.AssociatedGang.ID == Player.RelationshipManager.GangRelationships.CurrentGang.ID)             
            {
                if(Player.Violations.WeaponViolations.ShotSomewhatRecently)
                {
                    isHitSquad = false;
                    closestDen = gangDen;
                    RecentlyAttacked = true;
                    IsGeneralBackup = true;
                    break;
                }
                else if (Player.WantedLevel >= 2 && (Player.ClosestPoliceDistanceToPlayer < 20f || Player.AnyPoliceRecentlySeenPlayer))
                {
                    isHitSquad = false;
                    closestDen = gangDen;
                    RecentlyAttacked = true;
                    IsGeneralBackup = true;
                    break;
                }
            }
            if(gangDen.DistanceToPlayer <= closestDistance)
            {
                fallbackCloseestDen = gangDen;
                closestDistance = gangDen.DistanceToPlayer;
            }
        }
        if(closestDen == null)
        {
            closestDen = fallbackCloseestDen;
        }
        if (closestDen == null || closestDen.AssociatedGang == null)
        {
            //EntryPoint.WriteToConsole("Assault Spawn failed no den or gang");
            return;
        }
       // EntryPoint.WriteToConsole($"Assault Spawn Picked {closestDen.AssociatedGang.ShortName}");
        if (closestDen.TotalAssaultSpawns >= closestDen.MaxAssaultSpawns)
        {
           // EntryPoint.WriteToConsole("Assault Spawn failed too many spawns already");
            return;
        }
        if (World.Pedestrians.GangMemberList.Count(x => x.Gang?.ID == closestDen.AssociatedGang.ID) >= closestDen.AssociatedGang.SpawnLimit)
        {
            //EntryPoint.WriteToConsole("Assault Spawn failed TOO MANY GANG MEMBERS");
            return;
        }
        //GangReputation gr = Player.RelationshipManager.GangRelationships.GetReputation(closestDen.AssociatedGang);

        bool shouldAttack = Player.Violations.WeaponViolations.ShotSomewhatRecently || RecentlyAttacked;
        if (!shouldAttack)
        {
            //EntryPoint.WriteToConsole("Assault Spawn failed havent recently attacked or shot");
            return;
        }
        if (!closestDen.AssociatedGang.CanSpawn(World.TotalWantedLevel))
        {
            //EntryPoint.WriteToConsole("Assault Spawn failed cantspawn");
            return;
        }
        GameTimeLastAttemptedAssaultSpawn = Game.GameTime;


        if (!GetAssaultSpawnTypes(closestDen.AssociatedGang))
        {
            EntryPoint.WriteToConsole("Assault Spawn failed type");
            return;
        }
        if (!GetAssaultSpawnLocation(closestDen))
        {
            EntryPoint.WriteToConsole("Assault Spawn failed location");
            return;
        }
        EntryPoint.WriteToConsole($"Assault Spawn EXECUTED TotalAssaultSpawns SO FAR:{closestDen.TotalAssaultSpawns}");
        GameFiber.Yield();
        int pedsSpawned = CallSpawnTask(true, true, true, false, TaskRequirements.None, isHitSquad, false, 99, IsGeneralBackup);
        EntryPoint.WriteToConsole($"GANG ASSAULT SPAWN PEDS SPAWNED THIS TIME {pedsSpawned}");
        if (pedsSpawned > 0)
        {
            closestDen.TotalAssaultSpawns++;
        }
    }


    private bool GetAssaultSpawnTypes(Gang gang)
    {
        Gang = null;
        VehicleType = null;
        PersonType = null;
        Gang = gang;
        if (Gang == null)
        {
            return false;
        }
        PersonType = Gang.GetRandomPed(World.TotalWantedLevel, "");
        return PersonType != null;
    }

    //private bool GetAssaultSpawnLocation(GangDen closestDen)
    //{
    //    SpawnLocation = new SpawnLocation();
    //    if (closestDen == null || PersonType == null || string.IsNullOrEmpty(PersonType.ModelName))
    //    {
    //        return false;
    //    }
    //    uint modelHash = Game.GetHashKey(PersonType.ModelName);
    //    uint GameTimeStarted = Game.GameTime;
    //    if (!NativeFunction.Natives.HAS_MODEL_LOADED<bool>(modelHash))
    //    {
    //        NativeFunction.Natives.REQUEST_MODEL(modelHash);
    //        while (!NativeFunction.Natives.HAS_MODEL_LOADED<bool>(modelHash) && Game.GameTime - GameTimeStarted <= 1000)
    //        {
    //            GameFiber.Yield();
    //        }
    //    }
    //    if (closestDen.PossiblePedSpawns.Any())
    //    {
    //        foreach (ConditionalLocation cl in closestDen.PossiblePedSpawns.Where(x=> x.Location.DistanceTo2D(Game.LocalPlayer.Character) >= 20f).OrderBy(x => Guid.NewGuid()))
    //        {
    //            if (NativeFunction.Natives.WOULD_ENTITY_BE_OCCLUDED<bool>(modelHash, cl.Location.X, cl.Location.Y, cl.Location.Z, true))
    //            {
    //                SpawnLocation.InitialPosition = cl.Location;
    //                SpawnLocation.Heading = cl.Heading;
    //                EntryPoint.WriteToConsole($"POSITION IS OCCLUDED, SPAWN THE PED {SpawnLocation.InitialPosition} {Game.LocalPlayer.Character.DistanceTo(SpawnLocation.InitialPosition)}");
    //                return true;
    //            }
    //        }
    //    }
    //    return false;
    //}


    private bool GetAssaultSpawnLocation(IAssaultSpawnable ClosestStation)
    {
        SpawnLocation = new SpawnLocation();
        if (ClosestStation == null || PersonType == null || string.IsNullOrEmpty(PersonType.ModelName))
        {
            return false;
        }
        uint modelHash = Game.GetHashKey(PersonType.ModelName);
        uint GameTimeStarted = Game.GameTime;
        if (!NativeFunction.Natives.HAS_MODEL_LOADED<bool>(modelHash))
        {
            NativeFunction.Natives.REQUEST_MODEL(modelHash);
            while (!NativeFunction.Natives.HAS_MODEL_LOADED<bool>(modelHash) && Game.GameTime - GameTimeStarted <= 1000)
            {
                GameFiber.Yield();
            }
        }

        List<SpawnPlace> PossibleSpawnPlaces = new List<SpawnPlace>();
        if (ClosestStation.AssaultSpawnLocations != null && ClosestStation.AssaultSpawnLocations.Any())
        {
            PossibleSpawnPlaces.AddRange(ClosestStation.AssaultSpawnLocations.Where(x => x.Position.DistanceTo2D(Game.LocalPlayer.Character) >= 20f).ToList());
            GameFiber.Yield();
        }
        if (!ClosestStation.RestrictAssaultSpawningUsingPedSpawns && ClosestStation.PossiblePedSpawns != null && ClosestStation.PossiblePedSpawns.Any())
        {
            foreach (ConditionalLocation cl in ClosestStation.PossiblePedSpawns.Where(x => x.Location.DistanceTo2D(Game.LocalPlayer.Character) >= 20f).ToList())
            {
                PossibleSpawnPlaces.Add(new SpawnPlace(cl.Location, cl.Heading));
            }
            GameFiber.Yield();
        }
        if (PossibleSpawnPlaces.Any())
        {
            foreach (SpawnPlace cl in PossibleSpawnPlaces.OrderBy(x => Guid.NewGuid()))
            {
                if (NativeFunction.Natives.WOULD_ENTITY_BE_OCCLUDED<bool>(modelHash, cl.Position.X, cl.Position.Y, cl.Position.Z, true))
                {
                    SpawnLocation.InitialPosition = cl.Position;
                    SpawnLocation.Heading = cl.Heading;
                    EntryPoint.WriteToConsole("POSITION IS OCCLUDED, SPAWN THE PED");
                    return true;
                }
                GameFiber.Yield();
            }
        }
        return false;
    }






    private void HandleHitSquadSpawns()
    {
        if (!Settings.SettingsManager.GangSettings.AllowHitSquads || !IsTimeToDispatchHitSquad)
        {
            return;
        }
        Gang EnemyGang;
        if (Settings.SettingsManager.GangSettings.AllowHitSquadsOnlyEnemy)
        {
            EnemyGang = Player.RelationshipManager.GangRelationships.EnemyGangs?.PickRandom();
        }
        else
        {
            EnemyGang = Player.RelationshipManager.GangRelationships.HitSquadGangs?.PickRandom();
        }
        DispatchHitSquad(EnemyGang);
        TimeBetweenHitSquads = RandomItems.GetRandomNumber(Settings.SettingsManager.GangSettings.MinTimeBetweenHitSquads, Settings.SettingsManager.GangSettings.MaxTimeBetweenHitSquads);
        GameTimeLastDispatchedHitSquad = Game.GameTime;
        HasDispatchedThisTick = true;
    }
    public void DispatchHitSquad(Gang enemyGang)
    {
        if(enemyGang == null)
        {
            EntryPoint.WriteToConsole($"DispatchHitSquad Abort, No Enemy Gang");
            return;
        }
        EntryPoint.WriteToConsole($"DispatchHitSquad Attempting to Dispatch {enemyGang.ShortName}");
        if(GetFarVehicleSpawnLocation() && GetHitSquadSpawnTypes(enemyGang,""))
        {
            EntryPoint.WriteToConsole($"DispatchHitSquad Disptaching HitSquad from {enemyGang.ShortName}");
            if(CallSpawnTask(false, true, false, false, TaskRequirements.None, true, false,99, false) > 0)
            {
                Player.OnHitSquadDispatched(enemyGang);
            }
        }
    }


    public bool DispatchGangBackup(Gang requestedGang, int membersToSpawn, string requiredVehicleModel)
    {
        if (requestedGang == null)
        {
            EntryPoint.WriteToConsole($"DispatchGangBackup Abort, No Requested Gang");
            return false;
        }
        EntryPoint.WriteToConsole($"DispatchGangBackup Attempting to Dispatch {requestedGang.ShortName} membersToSpawn{membersToSpawn}");
        GangDen closestDen = PlacesOfInterest.PossibleLocations.GangDens.Where(x => x.DistanceToPlayer <= 150f && x.IsEnabled && x.IsActivated && x.AssociatedGang != null && x.AssociatedGang.ID == requestedGang.ID).OrderBy(x=> x.DistanceToPlayer).FirstOrDefault();
        if (closestDen != null && GetAssaultSpawnTypes(closestDen.AssociatedGang) && GetAssaultSpawnLocation(closestDen))
        {
            EntryPoint.WriteToConsole("Gang Backup doing assault spawn");
            GameFiber.Yield();
            CallSpawnTask(true, true, true, false, TaskRequirements.None, false, true, 99, false);
        }
        else
        {
            if (GetCloseVehicleSpawnLocation() && GetHitSquadSpawnTypes(requestedGang, requiredVehicleModel))
            {
                EntryPoint.WriteToConsole($"DispatchGangBackup Disptaching Backup from {requestedGang.ShortName}");

                int membersSpawned = 0;
                for (int i = 0; i < 8; i++)//try 5 times to spawn the member amount, can get a LOT
                {
                    if (GetCloseVehicleSpawnLocation() && GetHitSquadSpawnTypes(requestedGang, requiredVehicleModel))
                    {
                        membersSpawned += CallSpawnTask(false, true, false, false, TaskRequirements.None, false, true, membersToSpawn - membersSpawned, false);


                        EntryPoint.WriteToConsole($"DispatchGangBackup {requestedGang.ShortName} RAN DISPATCH membersSpawned:{membersSpawned} membersToSpawn:{membersToSpawn} pedspawnlimit{membersToSpawn - membersSpawned}");


                        if (membersSpawned >= membersToSpawn)
                        {
                            break;
                        }
                    }
                }
                if (membersSpawned > 0)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void Dispose()
    {

    }
    public void Recall()
    {
        if(!IsTimeToRecall)
        {
            return;
        }
        foreach (GangMember gangMember in DeleteableGangMembers)
        {
            if (ShouldBeRecalled(gangMember))
            {
                Delete(gangMember);
                GameFiber.Yield();
            }
        }
        GameTimeAttemptedRecall = Game.GameTime;
        
    }
    private void HandleAmbientSpawns()
    {
        if(!IsTimeToAmbientDispatch || !HasNeedToAmbientDispatch)
        {
            return;
        }
        HasDispatchedThisTick = true;//up here for now, might be better down low
        if(ShouldRunAmbientDispatch)
        {
            //EntryPoint.WriteToConsole($"AMBIENT GANG RunAmbientDispatch 1 TimeBetweenSpawn{TimeBetweenSpawn}");
            RunAmbientDispatch();
        }
        else
        {
            ShouldRunAmbientDispatch = RandomItems.RandomPercent(PercentageOfAmbientSpawn);
            if(ShouldRunAmbientDispatch)
            {
                //EntryPoint.WriteToConsole($"AMBIENT GANG RunAmbientDispatch 2 TimeBetweenSpawn{TimeBetweenSpawn}");
                RunAmbientDispatch();              
            }
            else
            {
                //EntryPoint.WriteToConsole($"AMBIENT GANG Aborting Spawn for this dispatch TimeBetweenSpawn{TimeBetweenSpawn} PercentageOfAmbientSpawn{PercentageOfAmbientSpawn}");
                GameTimeAttemptedDispatch = Game.GameTime;
            }
        }  
    }
    private void RunAmbientDispatch()
    {
        //EntryPoint.WriteToConsoleTestLong($"AMBIENT GANG SPAWN RunAmbientDispatch ShouldRunAmbientDispatch{ShouldRunAmbientDispatch}: %{PercentageOfAmbientSpawn} TimeBetween:{TimeBetweenSpawn} AmbLimit:{AmbientMemberLimitForZoneType}");

        if(!GetSpawnLocation() || !GetSpawnTypes())
        {
            return;
        }
        GameTimeAttemptedDispatch = Game.GameTime;
        GameFiber.Yield();
        //EntryPoint.WriteToConsoleTestLong($"AMBIENT GANG CALLED SPAWN TASK");
        if (CallSpawnTask(false, true, false, false, TaskRequirements.None, false, false,99, false) > 0)
        {
            ShouldRunAmbientDispatch = false;
            //GameTimeAttemptedDispatch = Game.GameTime;
        }    
    }
    private bool GetSpawnLocation()
    {
        int timesTried = 0;
        bool isValidSpawn;
        GangDen = null;
        IsDenSpawn = false;
        IsPedestrianOnlySpawn = false;
        SpawnLocation = new SpawnLocation();
        do
        {
            if (RandomItems.RandomPercent(LikelyHoodOfDenSpawnWhenNear))
            {
                GangDen = PlacesOfInterest.PossibleLocations.GangDens.Where(x => x.IsNearby).PickRandom();
            }
            if(GangDen != null && GangDen.DistanceToPlayer >= 90f && GangDen.AssociatedGang != null)
            {
                Gang = GangDen.AssociatedGang;
                IsPedestrianOnlySpawn = RandomItems.RandomPercent(Gang.PedestrianSpawnPercentageAroundDen);
                IsDenSpawn = true;
                SpawnLocation.InitialPosition = GangDen.EntrancePosition.Around2D(50f);
               // EntryPoint.WriteToConsole($"Gang Dispatcher Den Spawn, Ped Only:{IsPedestrianOnlySpawn}");
            }
            else
            {
                IsPedestrianOnlySpawn = RandomItems.RandomPercent(Settings.SettingsManager.GangSettings.AmbientSpawnPedestrianAttemptPercentage);
                GangDen = null;
                SpawnLocation.InitialPosition = GetPositionAroundPlayer();
               // EntryPoint.WriteToConsole($"Gang Dispatcher Regular Spawn, Ped Only:{IsPedestrianOnlySpawn}");
            }
            SpawnLocation.GetClosestStreet(false);
            SpawnLocation.GetClosestSidewalk();
            if(IsPedestrianOnlySpawn && !IsDenSpawn && !SpawnLocation.HasSidewalk)
            {
                IsPedestrianOnlySpawn = false;
               // EntryPoint.WriteToConsole($"Gang Dispatcher attempted Ped Only spawn without sidewalks, changing to vehicle");
            }
            GameFiber.Yield();
            isValidSpawn = IsValidSpawn(SpawnLocation);
            timesTried++;
        }
        while (!SpawnLocation.HasSpawns && !isValidSpawn && timesTried < 2);//10
        return isValidSpawn && SpawnLocation.HasSpawns;
    }
    private bool GetCloseVehicleSpawnLocation()
    {
        int timesTried = 0;
        bool isValidSpawn;
        GangDen = null;
        IsDenSpawn = false;
        IsPedestrianOnlySpawn = false;
        SpawnLocation = new SpawnLocation();
        do
        {
            IsPedestrianOnlySpawn = false;
            SpawnLocation.InitialPosition = GetClosePositionAroundPlayer();
            SpawnLocation.GetClosestStreet(false);
            GameFiber.Yield();
            isValidSpawn = IsValidSpawn(SpawnLocation);
            timesTried++;
        }
        while (!SpawnLocation.HasSpawns && !isValidSpawn && timesTried < 2);//10
        return isValidSpawn && SpawnLocation.HasSpawns;
    }
    private bool GetFarVehicleSpawnLocation()
    {
        int timesTried = 0;
        bool isValidSpawn;
        GangDen = null;
        IsDenSpawn = false;
        IsPedestrianOnlySpawn = false;
        SpawnLocation = new SpawnLocation();
        do
        {
            IsPedestrianOnlySpawn = false;
            SpawnLocation.InitialPosition = GetPositionAroundPlayer();
            SpawnLocation.GetClosestStreet(false);
            GameFiber.Yield();
            isValidSpawn = IsValidSpawn(SpawnLocation);
            timesTried++;
        }
        while (!SpawnLocation.HasSpawns && !isValidSpawn && timesTried < 2);//10
        return isValidSpawn && SpawnLocation.HasSpawns;
    }
    private bool GetSpawnTypes()
    {
        Gang = null;
        VehicleType = null;
        PersonType = null;
        if (IsDenSpawn && GangDen != null)
        {
            Gang = GangDen.AssociatedGang;
        }
        else
        {
            Gang = GetRandomGang(SpawnLocation);
        }
        GameFiber.Yield();
        if (Gang == null)
        {
            return false;
        }    
        if (World.Pedestrians.GangMemberList.Count(x => x.Gang?.ID == Gang.ID) >= Gang.SpawnLimit)
        {
            return false;
        }
        if(!IsPedestrianOnlySpawn)
        {
            VehicleType = Gang.GetRandomVehicle(Player.WantedLevel, HasNeedToSpawnHeli, false, true, "", Settings);
        }
        GameFiber.Yield();
        string RequiredGroup = "";
        if (VehicleType != null)
        {
            RequiredGroup = VehicleType.RequiredPedGroup;
        }
        PersonType = Gang.GetRandomPed(Player.WantedLevel, RequiredGroup);
        GameFiber.Yield();
        if (PersonType != null)
        {
            return true;
        }         
        return false;
    }
    private bool GetHitSquadSpawnTypes(Gang gang, string requiredVehicleModel)
    {
        Gang = gang;
        VehicleType = null;
        PersonType = null;
        if (Gang == null)
        {
            return false;
        }
        if (World.Pedestrians.GangMemberList.Count(x => x.Gang?.ID == Gang.ID) >= Gang.SpawnLimit)
        {
            return false;
        }

        if(string.IsNullOrEmpty(requiredVehicleModel))
        {
            VehicleType = Gang.GetRandomVehicle(Player.WantedLevel, false, false, true, "", Settings);
        }
        else
        {
            VehicleType = gang.Vehicles.FirstOrDefault(x => x.ModelName == requiredVehicleModel);
        }
        if(VehicleType == null)
        {
            VehicleType = Gang.GetRandomVehicle(Player.WantedLevel, false, false, true, "", Settings);
        }
        GameFiber.Yield();
        string RequiredGroup = "";
        if (VehicleType != null)
        {
            RequiredGroup = VehicleType.RequiredPedGroup;
        }
        PersonType = Gang.GetRandomPed(Player.WantedLevel, RequiredGroup);
        GameFiber.Yield();
        if (PersonType != null)
        {
            return true;
        }
        return false;
    }
    private int CallSpawnTask(bool allowAny, bool allowBuddy, bool isLocationSpawn, bool clearArea, TaskRequirements spawnRequirement, bool isHitSquad, bool isBackupSquad, int pedspawnLimit, bool isGeneralBackup)
    {
        try
        {
            GameFiber.Yield();
            GangSpawnTask gangSpawnTask = new GangSpawnTask(Gang, SpawnLocation, VehicleType, PersonType, Settings.SettingsManager.GangSettings.ShowSpawnedBlip, Settings, Weapons, Names, true, Crimes, PedGroups, ShopMenus, World, ModItems, false, false, false);// Settings.SettingsManager.Police.SpawnedAmbientPoliceHaveBlip);
            gangSpawnTask.AllowAnySpawn = allowAny;
            gangSpawnTask.AllowBuddySpawn = allowBuddy;
            gangSpawnTask.SpawnRequirement = spawnRequirement;
            gangSpawnTask.ClearVehicleArea = clearArea;
            gangSpawnTask.PlacePedOnGround = VehicleType == null;
            gangSpawnTask.IsHitSquad = isHitSquad;
            gangSpawnTask.IsBackupSquad = isBackupSquad;
            gangSpawnTask.IsGeneralBackup = isGeneralBackup;
            gangSpawnTask.PedSpawnLimit = pedspawnLimit;
            gangSpawnTask.AttemptSpawn();
            foreach (PedExt created in gangSpawnTask.CreatedPeople)
            {
                World.Pedestrians.AddEntity(created);
            }
            gangSpawnTask.CreatedPeople.ForEach(x => { World.Pedestrians.AddEntity(x); x.IsLocationSpawned = isLocationSpawn; });
            gangSpawnTask.CreatedVehicles.ForEach(x => x.AddVehicleToList(World));// World.Vehicles.AddEntity(x, ResponseType.None));;
            HasDispatchedThisTick = true;
            return gangSpawnTask.CreatedPeople.Count(x => x.Pedestrian.Exists());
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"Gang Dispatcher Spawn Error: {ex.Message} : {ex.StackTrace}", 0);
            return 0;
        }
    }
    private bool ShouldBeRecalled(GangMember gangMember)
    {
        if(!gangMember.RecentlyUpdated)
        {
            return false;
        }
        else if(gangMember.IsManuallyDeleted)
        {
            return false;
        }
        else if (gangMember.IsInVehicle)
        {
            return gangMember.DistanceChecker.IsMovingAway &&  gangMember.DistanceToPlayer >= DistanceToDeleteInVehicle;
        }
        else
        {
            return gangMember.DistanceChecker.IsMovingAway && gangMember.DistanceToPlayer >= DistanceToDeleteOnFoot;
        }
    }
    private void Delete(PedExt ganegMember)
    {
        if (ganegMember != null && ganegMember.Pedestrian.Exists())
        {
            //EntryPoint.WriteToConsole($"Attempting to Delete {Cop.Pedestrian.Handle}");
            if (ganegMember.Pedestrian.IsInAnyVehicle(false))
            {
                if (ganegMember.Pedestrian.CurrentVehicle.HasPassengers)
                {
                    foreach (Ped Passenger in ganegMember.Pedestrian.CurrentVehicle.Passengers)
                    {
                        if (Passenger.Handle != Game.LocalPlayer.Character.Handle)
                        {
                            RemoveBlip(Passenger);
                            Passenger.Delete();
                            EntryPoint.PersistentPedsDeleted++;
                        }
                    }
                }
                if (ganegMember.Pedestrian.Exists() && ganegMember.Pedestrian.CurrentVehicle.Exists() && ganegMember.Pedestrian.CurrentVehicle != null)
                {
                    Blip carBlip = ganegMember.Pedestrian.CurrentVehicle.GetAttachedBlip();
                    if (carBlip.Exists())
                    {
                        carBlip.Delete();
                    }
                    VehicleExt vehicleExt = World.Vehicles.GetVehicleExt(ganegMember.Pedestrian.CurrentVehicle);
                    if (vehicleExt != null)
                    {
                        vehicleExt.FullyDelete();
                    }
                    else
                    {
                        ganegMember.Pedestrian.CurrentVehicle.Delete();
                    }
                    EntryPoint.PersistentVehiclesDeleted++;
                }
            }
            RemoveBlip(ganegMember.Pedestrian);
            if (ganegMember.Pedestrian.Exists())
            {
                //EntryPoint.WriteToConsole(string.Format("Delete Cop Handle: {0}, {1}, {2}", Cop.Pedestrian.Handle, Cop.DistanceToPlayer, Cop.AssignedAgency.Initials));
                ganegMember.Pedestrian.Delete();
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
    private Vector3 GetClosePositionAroundPlayer()
    {
        Vector3 Position;
        Position = Player.Position;
        Position = Position.Around2D(MinDistanceToSpawnOnFoot, MaxDistanceToSpawnOnFoot);
        return Position;
    }
    private Vector3 GetPositionAroundPlayer()
    {
        Vector3 Position;
        Position = Player.Position;
        Position = Position.Around2D(IsPedestrianOnlySpawn ? MinDistanceToSpawnOnFoot : MinDistanceToSpawnInVehicle, IsPedestrianOnlySpawn ? MaxDistanceToSpawnOnFoot : MaxDistanceToSpawnInVehicle);
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
    public void DebugSpawnGangMember(string gangID, bool onFoot, bool isEmpty, DispatchableVehicle vehicleType, DispatchablePerson personType)
    {
        VehicleType = null;
        PersonType = null;
        Gang = null;
        SpawnLocation = new SpawnLocation();
        SpawnLocation.InitialPosition = Game.LocalPlayer.Character.GetOffsetPositionFront(10f);
        SpawnLocation.StreetPosition = SpawnLocation.InitialPosition;
        SpawnLocation.Heading = Game.LocalPlayer.Character.Heading;
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
            VehicleType = Gang.GetRandomVehicle(Player.WantedLevel, true, true, true, "", Settings);
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

        if(isEmpty)
        {
            PersonType = null;
        }
        if (vehicleType != null)
        {
            VehicleType = vehicleType;
        }
        if (personType != null)
        {
            PersonType = personType;
        }
        CallSpawnTask(true, true, false, false, TaskRequirements.None, false, false,99, false);
    }
    public void DebugSpawnHitSquad()
    {
        GameTimeLastDispatchedHitSquad = 0;
        TimeBetweenHitSquads = 0;
    }
}