using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime;

public class EMSDispatcher
{
    private readonly IAgencies Agencies;
    private readonly IDispatchable Player;
    private readonly int LikelyHoodOfAnySpawn = 5;
    private readonly float MinimumDeleteDistance = 150f;//200f
    private readonly uint MinimumExistingTime = 20000;
    private readonly ISettingsProvideable Settings;
    private readonly IStreets Streets;
    private readonly IEntityProvideable World;
    private readonly IJurisdictions Jurisdictions;
    private readonly IZones Zones;
    private uint GameTimeAttemptedDispatch;
    private uint GameTimeAttemptedRecall;
    private bool HasDispatchedThisTick;
    private IWeapons Weapons;
    private INameProvideable Names;
    private SpawnLocation SpawnLocation;
    private Agency Agency;
    private DispatchableVehicle VehicleType;
    private DispatchablePerson PersonType;
    private IPlacesOfInterest PlacesOfInterest;
    private IModItems ModItems;
    private bool ShouldRunAmbientDispatch;

    public EMSDispatcher(IEntityProvideable world, IDispatchable player, IAgencies agencies, ISettingsProvideable settings, IStreets streets, IZones zones, IJurisdictions jurisdictions, IWeapons weapons, INameProvideable names, IPlacesOfInterest placesOfInterest, IModItems modItems)
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
        PlacesOfInterest = placesOfInterest;
        ModItems = modItems;
    }
    private float ClosestOfficerSpawnToPlayerAllowed => Player.Investigation.IsActive && Player.Investigation.RequiresEMS ? 100f : 250f;
    private List<EMT> DeletableOfficers => World.Pedestrians.EMTList.Where(x => (x.RecentlyUpdated && x.DistanceToPlayer >= MinimumDeleteDistance && x.HasBeenSpawnedFor >= MinimumExistingTime) || x.CanRemove).ToList();
    private float DistanceToDelete => Player.IsWanted ? 600f : 1300f;
    private float DistanceToDeleteOnFoot => Player.IsWanted ? 125f : 500f;
    private bool IsTimeToDispatch => Game.GameTime - GameTimeAttemptedDispatch >= TimeBetweenSpawn;
    private bool IsTimeToRecall => Game.GameTime - GameTimeAttemptedRecall >= 5000;
    private float MaxDistanceToSpawn => Player.Investigation.IsActive && Player.Investigation.RequiresEMS ? Settings.SettingsManager.EMSSettings.MaxDistanceToSpawn_Investigation : Settings.SettingsManager.EMSSettings.MaxDistanceToSpawn;//150f;
    private float MinDistanceToSpawn => Player.Investigation.IsActive && Player.Investigation.RequiresEMS ? Settings.SettingsManager.EMSSettings.MinDistanceToSpawn_Investigation : Settings.SettingsManager.EMSSettings.MinDistanceToSpawn;//50f;
    private float LikelyHoodOfCountySpawn => Settings.SettingsManager.EMSSettings.LikelyHoodOfCountySpawn;
    private bool HasNeedToAmbientDispatch
    {
        get
        {
            if (World.Pedestrians.TotalSpawnedEMTs > Settings.SettingsManager.EMSSettings.TotalSpawnedMembersLimit)
            {
                return false;
            }
            if (!Settings.SettingsManager.EMSSettings.AllowAmbientSpawningWhenPlayerWanted && Player.IsWanted)
            {
                return false;
            }
            if (Settings.SettingsManager.EMSSettings.AllowAmbientSpawningWhenPlayerWanted && Player.WantedLevel > Settings.SettingsManager.EMSSettings.AmbientSpawningWhenPlayerWantedMaxWanted)
            {
                return false;
            }
            if (World.Pedestrians.TotalSpawnedAmbientEMTs >= AmbientMemberLimitForZoneType)
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
            int AmbientMemberLimit = Settings.SettingsManager.EMSSettings.TotalSpawnedAmbientMembersLimit;
            if (EntryPoint.FocusZone?.Type == eLocationType.Wilderness)
            {
                AmbientMemberLimit = Settings.SettingsManager.EMSSettings.TotalSpawnedAmbientMembersLimit_Wilderness;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Rural)
            {
                AmbientMemberLimit = Settings.SettingsManager.EMSSettings.TotalSpawnedAmbientMembersLimit_Rural;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Suburb)
            {
                AmbientMemberLimit = Settings.SettingsManager.EMSSettings.TotalSpawnedAmbientMembersLimit_Suburb;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Industrial)
            {
                AmbientMemberLimit = Settings.SettingsManager.EMSSettings.TotalSpawnedAmbientMembersLimit_Industrial;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Downtown)
            {
                AmbientMemberLimit = Settings.SettingsManager.EMSSettings.TotalSpawnedAmbientMembersLimit_Downtown;
            }
            if(Player.Investigation.IsActive && Player.Investigation.RequiresEMS)
            {
                AmbientMemberLimit = Settings.SettingsManager.EMSSettings.TotalSpawnedAmbientMembersLimit_Investigation;
            }
            return AmbientMemberLimit;
        }
    }
    private int TimeBetweenSpawn// => Settings.SettingsManager.GangSettings.TimeBetweenSpawn;//15000;
    {
        get
        {
            int TotalTimeBetweenSpawns = Settings.SettingsManager.EMSSettings.TimeBetweenSpawn;
            if(Player.Investigation.IsActive && Player.Investigation.RequiresEMS)
            {
                TotalTimeBetweenSpawns = Settings.SettingsManager.EMSSettings.TimeBetweenSpawn_Investigation;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Wilderness)
            {
                TotalTimeBetweenSpawns += Settings.SettingsManager.EMSSettings.TimeBetweenSpawn_WildernessAdditional;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Rural)
            {
                TotalTimeBetweenSpawns += Settings.SettingsManager.EMSSettings.TimeBetweenSpawn_RuralAdditional;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Suburb)
            {
                TotalTimeBetweenSpawns += Settings.SettingsManager.EMSSettings.TimeBetweenSpawn_SuburbAdditional;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Industrial)
            {
                TotalTimeBetweenSpawns += Settings.SettingsManager.EMSSettings.TimeBetweenSpawn_IndustrialAdditional;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Downtown)
            {
                TotalTimeBetweenSpawns += Settings.SettingsManager.EMSSettings.TimeBetweenSpawn;
            }
            return TotalTimeBetweenSpawns;
        }
    }
    private int PercentageOfAmbientSpawn // => Settings.SettingsManager.GangSettings.TimeBetweenSpawn;//15000;
    {
        get
        {
            int ambientSpawnPercent = Settings.SettingsManager.EMSSettings.AmbientSpawnPercentage;
            if (Player.Investigation.IsActive && Player.Investigation.RequiresEMS)
            {
                ambientSpawnPercent = Settings.SettingsManager.EMSSettings.AmbientSpawnPercentage_Investigation;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Wilderness)
            {
                ambientSpawnPercent = Settings.SettingsManager.EMSSettings.AmbientSpawnPercentage_Wilderness;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Rural)
            {
                ambientSpawnPercent = Settings.SettingsManager.EMSSettings.AmbientSpawnPercentage_Rural;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Suburb)
            {
                ambientSpawnPercent = Settings.SettingsManager.EMSSettings.AmbientSpawnPercentage_Suburb;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Industrial)
            {
                ambientSpawnPercent = Settings.SettingsManager.EMSSettings.AmbientSpawnPercentage_Industrial;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Downtown)
            {
                ambientSpawnPercent = Settings.SettingsManager.EMSSettings.AmbientSpawnPercentage_Downtown;
            }
            return ambientSpawnPercent;
        }
    }
    public bool Dispatch()
    {
        HasDispatchedThisTick = false;
        if(Settings.SettingsManager.EMSSettings.ManageDispatching)
        {
            HandleAmbientSpawns();
        }   
        return HasDispatchedThisTick;
    }
    public void Dispose()
    {

    }
    public void Recall()
    {
        if (Settings.SettingsManager.EMSSettings.ManageDispatching && IsTimeToRecall)
        {
            foreach (EMT emt in DeletableOfficers)
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
        if (!IsTimeToDispatch || !HasNeedToAmbientDispatch)
        {
            return;
        }  
        bool shouldRun = RandomItems.RandomPercent(PercentageOfAmbientSpawn);
        //EntryPoint.WriteToConsole($"AMBIENT EMS SPAWN shouldRun{shouldRun}: %{PercentageOfAmbientSpawn}");
        HasDispatchedThisTick = true;



        if (ShouldRunAmbientDispatch)
        {
            //EntryPoint.WriteToConsoleTestLong($"AMBIENT EMS RunAmbientDispatch 1 TimeBetweenSpawn{TimeBetweenSpawn}");
            RunAmbientDispatch();
        }
        else
        {
            ShouldRunAmbientDispatch = RandomItems.RandomPercent(PercentageOfAmbientSpawn);
            if (ShouldRunAmbientDispatch)
            {
                //EntryPoint.WriteToConsoleTestLong($"AMBIENT EMS RunAmbientDispatch 2 TimeBetweenSpawn{TimeBetweenSpawn}");
                RunAmbientDispatch();
            }
            else
            {
                //EntryPoint.WriteToConsoleTestLong($"AMBIENT EMS Aborting Spawn for this dispatch TimeBetweenSpawn{TimeBetweenSpawn} PercentageOfAmbientSpawn{PercentageOfAmbientSpawn}");
                GameTimeAttemptedDispatch = Game.GameTime;
            }
        }






    }
    private void RunAmbientDispatch()
    {
        if (GetSpawnLocation() && GetSpawnTypes(false, null))
        {
            GameFiber.Yield();
            CallSpawnTask(false, true, false, false, TaskRequirements.None);
            GameTimeAttemptedDispatch = Game.GameTime;
        }
    }
    private bool GetSpawnLocation()
    {
        int timesTried = 0;
        bool isValidSpawn;
        SpawnLocation = new SpawnLocation();
        do
        {
            SpawnLocation.InitialPosition = GetPositionAroundPlayer();
            SpawnLocation.GetClosestStreet(false);
            isValidSpawn = IsValidSpawn(SpawnLocation);
            timesTried++;
        }
        while (!SpawnLocation.HasSpawns && !isValidSpawn && timesTried < 2);//10
        return isValidSpawn && SpawnLocation.HasSpawns;
    }
    private bool GetSpawnTypes(bool forcePed, Agency forceAgency)
    {
        Agency = null;
        VehicleType = null;
        PersonType = null;
        if (forceAgency != null)
        {
            Agency = forceAgency;
        }
        else
        {
            Agency = GetRandomAgency(SpawnLocation);
        }
        if (Agency != null)
        {
            VehicleType = Agency.GetRandomVehicle(Player.WantedLevel, false, false, false, "", Settings);
            if (forcePed)
            {
                VehicleType = null;
            }
            if (VehicleType != null)
            {
                string RequiredGroup = "";
                if (VehicleType != null)
                {
                    RequiredGroup = VehicleType.RequiredPedGroup;
                }
                PersonType = Agency.GetRandomPed(World.TotalWantedLevel, RequiredGroup);
                if (PersonType != null)
                {
                    return true;
                }
            }
            else if (forcePed)
            {
                PersonType = Agency.GetRandomPed(World.TotalWantedLevel, "");
                if (PersonType != null)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private bool CallSpawnTask(bool allowAny, bool allowBuddy, bool isLocationSpawn, bool clearArea, TaskRequirements spawnRequirement)
    {
        try
        {
            EMTSpawnTask eMTSpawnTask = new EMTSpawnTask(Agency, SpawnLocation, VehicleType, PersonType, Settings.SettingsManager.EMSSettings.ShowSpawnedBlips, Settings, Weapons, Names, true, World, ModItems);
            eMTSpawnTask.AllowAnySpawn = allowAny;
            eMTSpawnTask.AllowBuddySpawn = allowBuddy;
            eMTSpawnTask.SpawnRequirement = spawnRequirement;
            eMTSpawnTask.ClearVehicleArea = clearArea;
            eMTSpawnTask.PlacePedOnGround = VehicleType == null;
            eMTSpawnTask.AttemptSpawn();  
            eMTSpawnTask.CreatedPeople.ForEach(x => { World.Pedestrians.AddEntity(x); x.IsLocationSpawned = isLocationSpawn; });
            //eMTSpawnTask.CreatedPeople.ForEach(x => World.Pedestrians.AddEntity(x));
            eMTSpawnTask.CreatedVehicles.ForEach(x => x.AddVehicleToList(World));// World.Vehicles.AddEntity(x, ResponseType.EMS));
            return eMTSpawnTask.CreatedPeople.Any(x=>x.Pedestrian.Exists());
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"EMS Dispatcher Spawn Error: {ex.Message} : {ex.StackTrace}", 0);
            return false;
        }
    }
    private bool ShouldBeRecalled(EMT emt)
    {
        if(emt.IsInVehicle)
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
                        RemoveBlip(Passenger);
                        Passenger.Delete();
                        EntryPoint.PersistentPedsDeleted++;
                    }
                }
                if (emt.Pedestrian.Exists() && emt.Pedestrian.CurrentVehicle.Exists() && emt.Pedestrian.CurrentVehicle != null)
                {
                    Blip carBlip = emt.Pedestrian.CurrentVehicle.GetAttachedBlip();
                    if (carBlip.Exists())
                    {
                        carBlip.Delete();
                    }
                    VehicleExt vehicleExt = World.Vehicles.GetVehicleExt(emt.Pedestrian.CurrentVehicle);
                    if (vehicleExt != null)
                    {
                        vehicleExt.FullyDelete();
                    }
                    else
                    {
                        emt.Pedestrian.CurrentVehicle.Delete();
                    }
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
    private List<Agency> GetAgencies(Vector3 Position, int WantedLevel)
    {
        List<Agency> ToReturn = new List<Agency>();
        Zone CurrentZone = Zones.GetZone(Position);
        Agency ZoneAgency = Jurisdictions.GetRandomAgency(CurrentZone.InternalGameName, WantedLevel, ResponseType.EMS);
        if (ZoneAgency != null)
        {
            ToReturn.Add(ZoneAgency); //Zone Jurisdiciton Random
        }
        if (!ToReturn.Any() || RandomItems.RandomPercent(LikelyHoodOfCountySpawn))
        {
            Agency CountyAgency = Jurisdictions.GetRandomCountyAgency(CurrentZone.CountyID, WantedLevel, ResponseType.EMS);
            if (CountyAgency != null)//randomly spawn the county agency
            {
                ToReturn.Add(CountyAgency); //Zone Jurisdiciton Random
            }
        }
        if (!ToReturn.Any() || RandomItems.RandomPercent(LikelyHoodOfAnySpawn))//fall back to anybody
        {
            ToReturn.AddRange(Agencies.GetSpawnableAgencies(WantedLevel, ResponseType.EMS));
        }
        foreach (Agency ag in ToReturn)
        {
            //EntryPoint.WriteToConsole(string.Format("Debugging: Agencies At Pos: {0}", ag.Initials));
        }
        return ToReturn;
    }
    private Vector3 GetPositionAroundPlayer()
    {
        Vector3 Position;
        if (Player.IsInVehicle)
        {
            Position = Player.Character.GetOffsetPositionFront(250f);//350f
        }
        else
        {
            Position = Player.Position;
        }
        Position = Position.Around2D(MinDistanceToSpawn, MaxDistanceToSpawn);
        return Position;
    }
    private Agency GetRandomAgency(SpawnLocation spawnLocation)
    {
        Agency agency;
        List<Agency> PossibleAgencies = GetAgencies(spawnLocation.StreetPosition, Player.WantedLevel);
        agency = PossibleAgencies.PickRandom();
        if (agency == null)
        {
            agency = GetAgencies(spawnLocation.InitialPosition, Player.WantedLevel).PickRandom();
        }
        if (agency == null)
        {
            //EntryPoint.WriteToConsole("Dispatcher could not find Agency To Spawn");
        }
        return agency;
    }
    private bool IsValidSpawn(SpawnLocation spawnLocation)
    {
        if (spawnLocation.StreetPosition.DistanceTo2D(Player.Position) < ClosestOfficerSpawnToPlayerAllowed)
        {
            return false;
        }
        else if (spawnLocation.InitialPosition.DistanceTo2D(Player.Position) < ClosestOfficerSpawnToPlayerAllowed)
        {
            return false;
        }
        return true;
    }
    public void DebugSpawnEMT(string agencyID, bool onFoot, bool isEmpty, DispatchableVehicle vehicleType, DispatchablePerson personType)
    {
        VehicleType = null;
        PersonType = null;
        Agency = null;
        SpawnLocation = new SpawnLocation();
        SpawnLocation.InitialPosition = Game.LocalPlayer.Character.GetOffsetPositionFront(10f);
        SpawnLocation.StreetPosition = SpawnLocation.InitialPosition;
        SpawnLocation.Heading = Game.LocalPlayer.Character.Heading;
        if (agencyID == "")
        {
            Agency = Agencies.GetRandomAgency(ResponseType.EMS);
        }
        else
        {
            Agency = Agencies.GetAgency(agencyID);
        }
        if(Agency == null)
        {
            return;
        }
        if (!onFoot)
        {
            VehicleType = Agency.GetRandomVehicle(World.TotalWantedLevel, false, false, true, "", Settings);
        }
        if (VehicleType != null || onFoot)
        {
            string RequiredGroup = "";
            if (VehicleType != null)
            {
                RequiredGroup = VehicleType.RequiredPedGroup;
            }
            PersonType = Agency.GetRandomPed(World.TotalWantedLevel, RequiredGroup);
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
        CallSpawnTask(true, false, false, false, TaskRequirements.None);
        EntryPoint.WriteToConsole("DebugSpawnEMT");
    }

    public void OnMedicalServicesRequested()
    {
        RunAmbientDispatch();
    }
}