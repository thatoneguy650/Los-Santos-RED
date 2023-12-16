using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

public class SecurityDispatcher
{
    private readonly IAgencies Agencies;
    private readonly IDispatchable Player;
    private readonly int LikelyHoodOfAnySpawn = 5;
    private readonly float MinimumDeleteDistance = 125f;//200f
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
    private ICrimes Crimes;
    private IModItems ModItems;

    public SecurityDispatcher(IEntityProvideable world, IDispatchable player, IAgencies agencies, ISettingsProvideable settings, IStreets streets, IZones zones, IJurisdictions jurisdictions, IWeapons weapons, INameProvideable names, 
        IPlacesOfInterest placesOfInterest, ICrimes crimes, IModItems modItems)
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
        Crimes = crimes;
        ModItems = modItems;
    }
    private float ClosestOfficerSpawnToPlayerAllowed => Player.IsWanted ? 150f : 250f;
    private List<SecurityGuard> DeletableOfficers => World.Pedestrians.SecurityGuardList.Where(x => (x.RecentlyUpdated && x.DistanceToPlayer >= MinimumDeleteDistance && x.HasBeenSpawnedFor >= MinimumExistingTime) || x.CanRemove).ToList();
    private float DistanceToDelete => Player.IsWanted ? 400f : 600f;// Player.IsWanted ? 600f : 800f;
    private float DistanceToDeleteOnFoot => Player.IsWanted ? 125f : 300f;
    private bool IsTimeToRecall => Game.GameTime - GameTimeAttemptedRecall >= TimeBetweenRecall;
    private float MaxDistanceToSpawn => 650f;
    private float MinDistanceToSpawn => 350f;
    private int TimeBetweenSpawn => 60000;
    private int TimeBetweenRecall => 10000;
    public bool Dispatch()
    {
        //currently does nothing as security doesnt ambient spawn
        HasDispatchedThisTick = false;
        return HasDispatchedThisTick;
    }
    public void Dispose()
    {

    }
    public void Recall()
    {
        if (Settings.SettingsManager.SecuritySettings.ManageDispatching && IsTimeToRecall)
        {
            foreach (SecurityGuard emt in DeletableOfficers)
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
    private bool GetSpawnTypes(bool forcePed, bool forceVehicle, Agency forceAgency, string requiredGroup)
    {
        Agency = null;
        VehicleType = null;
        PersonType = null;
        Agency = forceAgency != null ? forceAgency : GetRandomAgency(SpawnLocation);
        if (Agency != null)
        {
            //EntryPoint.WriteToConsoleTestLong($"Security Dispatcher, GETTING SPAWN TYPES AGENCY NOT NULL");
            if (forcePed)
            {
                //EntryPoint.WriteToConsoleTestLong($"Security Dispatcher, FORCE PED requiredGroup:{requiredGroup}");
                PersonType = Agency.GetRandomPed(World.TotalWantedLevel, requiredGroup);
                //EntryPoint.WriteToConsoleTestLong($"Security Dispatcher, FORCE PED FOUND{PersonType != null}");
                return PersonType != null;
            }
            else if (forceVehicle)
            {
                VehicleType = Agency.GetRandomVehicle(World.TotalWantedLevel, false, false, true, requiredGroup, Settings);
                return VehicleType != null;
            }
            else
            {
                VehicleType = Agency.GetRandomVehicle(World.TotalWantedLevel, false, false, true, "", Settings);
                if (VehicleType != null)
                {
                    string RequiredGroup = "";
                    if (VehicleType != null)
                    {
                        RequiredGroup = VehicleType.RequiredPedGroup;
                    }
                    PersonType = Agency.GetRandomPed(World.TotalWantedLevel, RequiredGroup);
                    return PersonType != null;
                }
            }
        }
        return false;
    }
    private void CallSpawnTask(bool allowAny, bool allowBuddy, bool isLocationSpawn, bool clearArea, TaskRequirements spawnRequirement)
    {
        try
        {
            //EntryPoint.WriteToConsoleTestLong($"Security Dispatcher, SPAWN TASK STARTING");
            SecurityGuardSpawnTask securitySpawnTask = new SecurityGuardSpawnTask(Agency, SpawnLocation, VehicleType, PersonType, Settings.SettingsManager.SecuritySettings.ShowSpawnedBlips, Settings, Weapons, Names, true, World, Crimes, ModItems);
            securitySpawnTask.AllowAnySpawn = allowAny;
            securitySpawnTask.AllowBuddySpawn = allowBuddy;
            securitySpawnTask.ClearVehicleArea = clearArea;
            securitySpawnTask.SpawnRequirement = spawnRequirement;
            securitySpawnTask.PlacePedOnGround = VehicleType == null;
            securitySpawnTask.AttemptSpawn();
            securitySpawnTask.CreatedPeople.ForEach(x => { World.Pedestrians.AddEntity(x); x.IsLocationSpawned = isLocationSpawn; });
            securitySpawnTask.CreatedVehicles.ForEach(x => x.AddVehicleToList(World));//World.Vehicles.AddEntity(x, ResponseType.Other));
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"Security Dispatcher Spawn Error: {ex.Message} : {ex.StackTrace}", 0);
        }
    }
    private bool ShouldBeRecalled(PedExt pedExt)
    {
        if (pedExt.IsInVehicle)
        {
            return pedExt.DistanceToPlayer >= DistanceToDelete;
        }
        else
        {
            return pedExt.DistanceToPlayer >= DistanceToDeleteOnFoot;
        }
    }
    private void Delete(PedExt pedExt)
    {
        if (pedExt != null && pedExt.Pedestrian.Exists())
        {
            //EntryPoint.WriteToConsole($"Attempting to Delete {Cop.Pedestrian.Handle}");
            if (pedExt.Pedestrian.IsInAnyVehicle(false))
            {
                if (pedExt.Pedestrian.CurrentVehicle.HasPassengers)
                {
                    foreach (Ped Passenger in pedExt.Pedestrian.CurrentVehicle.Passengers)
                    {
                        RemoveBlip(Passenger);
                        Passenger.Delete();
                        EntryPoint.PersistentPedsDeleted++;
                    }
                }
                if (pedExt.Pedestrian.Exists() && pedExt.Pedestrian.CurrentVehicle.Exists() && pedExt.Pedestrian.CurrentVehicle != null)
                {
                    Blip carBlip = pedExt.Pedestrian.CurrentVehicle.GetAttachedBlip();
                    if (carBlip.Exists())
                    {
                        carBlip.Delete();
                    }
                    VehicleExt vehicleExt = World.Vehicles.GetVehicleExt(pedExt.Pedestrian.CurrentVehicle);
                    if (vehicleExt != null)
                    {
                        vehicleExt.FullyDelete();
                    }
                    else
                    {
                        pedExt.Pedestrian.CurrentVehicle.Delete();
                    }
                    EntryPoint.PersistentVehiclesDeleted++;
                }
            }
            RemoveBlip(pedExt.Pedestrian);
            if (pedExt.Pedestrian.Exists())
            {
                //EntryPoint.WriteToConsole(string.Format("Delete Cop Handle: {0}, {1}, {2}", Cop.Pedestrian.Handle, Cop.DistanceToPlayer, Cop.AssignedAgency.Initials));
                pedExt.Pedestrian.Delete();
                EntryPoint.PersistentPedsDeleted++;
            }
        }
    }
    private void RemoveBlip(Ped ped)
    {
        if (!ped.Exists())
        {
            return;
        }
        Blip MyBlip = ped.GetAttachedBlip();
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
        if (!ToReturn.Any() || RandomItems.RandomPercent(LikelyHoodOfAnySpawn))//fall back to anybody
        {
            ToReturn.AddRange(Agencies.GetSpawnableAgencies(WantedLevel, ResponseType.Other));
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
    public void DebugSpawnSecurity(string agencyID, bool onFoot, bool isEmpty, DispatchableVehicle vehicleType, DispatchablePerson personType)
    {
        VehicleType = null;
        PersonType = null;
        Agency = null;
        SpawnLocation = new SpawnLocation();
        SpawnLocation.InitialPosition = Game.LocalPlayer.Character.GetOffsetPositionFront(10f);
        if (Game.LocalPlayer.Character.DistanceTo2D(new Vector3(682.6665f, 668.7299f, 128.4526f)) <= 30f)
        {
            SpawnLocation.InitialPosition = new Vector3(682.6665f, 668.7299f, 128.4526f);
            SpawnLocation.Heading = 189.3264f;
        }
        if (Game.LocalPlayer.Character.DistanceTo2D(new Vector3(229.028f, -988.8007f, -99.52672f)) <= 30f)
        {
            SpawnLocation.InitialPosition = new Vector3(229.028f, -988.8007f, -99.52672f);
            SpawnLocation.Heading = 358.3758f;
        }
        SpawnLocation.StreetPosition = SpawnLocation.InitialPosition;
        SpawnLocation.Heading = Game.LocalPlayer.Character.Heading;
        if (agencyID == "")
        {
            Agency = Agencies.GetRandomAgency(ResponseType.Other);
        }
        else
        {
            Agency = Agencies.GetAgency(agencyID);
        }
        if (Agency == null)
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
        if (isEmpty)
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
        CallSpawnTask(true, false, false, true, TaskRequirements.None);
    }

}