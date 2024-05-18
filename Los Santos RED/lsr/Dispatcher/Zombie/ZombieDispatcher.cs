using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

public class ZombieDispatcher
{
    private readonly IDispatchable Player;
   // private readonly int LikelyHoodOfAnySpawn = 5;
    private readonly float MinimumDeleteDistance = 150f;//200f
    private readonly uint MinimumExistingTime = 20000;
    private readonly ISettingsProvideable Settings;
    private readonly IStreets Streets;
    private readonly IEntityProvideable World;
    private readonly IJurisdictions Jurisdictions;
    private readonly IZones Zones;
    private bool HasDispatchedThisTick;
    private uint GameTimeAttemptedDispatch;
    private uint GameTimeAttemptedRecall;
    private IWeapons Weapons;
    private INameProvideable Names;
    private RelationshipGroup ZombiesRG;
    private ICrimes Crimes;
    public ZombieDispatcher(IEntityProvideable world, IDispatchable player, ISettingsProvideable settings, IStreets streets, IZones zones, IJurisdictions jurisdictions, IWeapons weapons, INameProvideable names, ICrimes crimes)
    {
        Player = player;
        World = world;
        Settings = settings;
        Streets = streets;
        Zones = zones;
        Jurisdictions = jurisdictions;
        Weapons = weapons;
        Names = names;
        Crimes = crimes;
        ZombiesRG = new RelationshipGroup("ZOMBIES");
    }
    private float ClosestZombieSpawnToPlayerAllowed => 25f;
    private List<Zombie> DeletableZombies => World.Pedestrians.ZombieList.Where(x => x.RecentlyUpdated && x.DistanceToPlayer >= MinimumDeleteDistance && x.HasBeenSpawnedFor >= MinimumExistingTime).ToList();
    private float DistanceToDelete => 175f;
    private float DistanceToDeleteOnFoot => 175f;
    private bool HasNeedToDispatch => World.Pedestrians.TotalSpawnedZombies <= 4;
    private bool IsTimeToDispatch => Game.GameTime - GameTimeAttemptedDispatch >= 5000;
    private bool IsTimeToRecall => Game.GameTime - GameTimeAttemptedRecall >= TimeBetweenSpawn;
    private float MaxDistanceToSpawn => 75f;
    private float MinDistanceToSpawn => 50f;
    private int TimeBetweenSpawn => 10000;
    public bool Dispatch()
    {
        HasDispatchedThisTick = false;
        if (IsTimeToDispatch && HasNeedToDispatch)
        {
            int timesTried = 0;
            bool isValidSpawn = false;
            SpawnLocation spawnLocation = new SpawnLocation();
            do
            {
                spawnLocation.InitialPosition = GetPositionAroundPlayer();
                spawnLocation.GetClosestStreet(false);
                isValidSpawn = IsValidSpawn(spawnLocation);
                timesTried++;
            }
            while (!spawnLocation.HasSpawns && !isValidSpawn && timesTried < 2);//10
            if (spawnLocation.HasSpawns && isValidSpawn)
            {

                Ped zombie = new Ped("u_m_y_zombie_01",spawnLocation.StreetPosition, spawnLocation.Heading);
                if (zombie.Exists())
                {
                    SetZombieStats(zombie);
                    Zombie myZombie = new Zombie(zombie, Settings, zombie.Health, true, Crimes, Weapons, "Unknown","Zombie", World);
                    World.Pedestrians.AddEntity(myZombie);
                }
                NativeFunction.Natives.SET_MODEL_AS_NO_LONGER_NEEDED(Game.GetHashKey("u_m_y_zombie_01"));
            }
            else
            {
                //EntryPoint.WriteToConsole($"DISPATCHER: Attempting to Spawn Fire Failed, Has Spawns {spawnLocation.HasSpawns} Is Valid {isValidSpawn}", 5);
            }
            GameTimeAttemptedDispatch = Game.GameTime;
        }
        return HasDispatchedThisTick;
    }
    public void Dispose()
    {

    }
    public void Recall()
    {
        if (Settings.SettingsManager.FireSettings.ManageDispatching && IsTimeToRecall)
        {
            foreach (Zombie ff in DeletableZombies)
            {
                if (ShouldBeRecalled(ff))
                {
                    Delete(ff);
                    GameFiber.Yield();
                }
            }
            GameTimeAttemptedRecall = Game.GameTime;
        }
    }
    private void SetZombieStats(Ped zombie)
    {
        if (zombie.Exists())
        {
            zombie.IsPersistent = true;
            zombie.Tasks.Clear();

            zombie.RelationshipGroup = ZombiesRG;
            PedGroup myGroup = new PedGroup(zombie.RelationshipGroup.Name, zombie.RelationshipGroup.Name, zombie.RelationshipGroup.Name, false);
            



            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(zombie, (int)eCombatAttributes.BF_AlwaysFight, true);
            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(zombie, (int)eCombatAttributes.BF_CanFightArmedPedsWhenNotArmed, true);
            NativeFunction.Natives.SET_PED_FLEE_ATTRIBUTES(zombie, 0, false);
            NativeFunction.Natives.TASK_COMBAT_HATED_TARGETS_AROUND_PED(zombie, 3000f, 0);
            zombie.KeepTasks = true;

            if (zombie.Exists())
            {
                Blip myBlip = zombie.AttachBlip();
                myBlip.Color = Color.White;
                myBlip.Scale = 0.6f;
            }

            NativeFunction.CallByName<bool>("SET_PED_IS_DRUNK", zombie, true);
            NativeFunction.CallByName<bool>("SET_PED_MOVEMENT_CLIPSET", zombie, "move_m@drunk@verydrunk", 0x3E800000);
            NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", zombie, (int)PedConfigFlags.PED_FLAG_DRUNK, true);
        }
    }
    private bool ShouldBeRecalled(Zombie ff)
    {
        if (ff.IsInVehicle)
        {
            return ff.DistanceToPlayer >= DistanceToDelete;
        }
        else
        {
            return ff.DistanceToPlayer >= DistanceToDeleteOnFoot;
        }
    }
    private void Delete(PedExt ff)
    {
        if (ff != null && ff.Pedestrian.Exists())
        {
            //EntryPoint.WriteToConsole($"Attempting to Delete {Cop.Pedestrian.Handle}");
            if (ff.Pedestrian.IsInAnyVehicle(false))
            {
                if (ff.Pedestrian.CurrentVehicle.HasPassengers)
                {
                    foreach (Ped Passenger in ff.Pedestrian.CurrentVehicle.Passengers)
                    {
                        RemoveBlip(Passenger);
                        Passenger.Delete();
                        EntryPoint.PersistentPedsDeleted++;
                    }
                }
                if (ff.Pedestrian.Exists() && ff.Pedestrian.CurrentVehicle.Exists() && ff.Pedestrian.CurrentVehicle != null)
                {
                    ff.Pedestrian.CurrentVehicle.Delete();
                    EntryPoint.PersistentVehiclesDeleted++;
                }
            }
            RemoveBlip(ff.Pedestrian);
            if (ff.Pedestrian.Exists())
            {
                //EntryPoint.WriteToConsole(string.Format("Delete Cop Handle: {0}, {1}, {2}", Cop.Pedestrian.Handle, Cop.DistanceToPlayer, Cop.AssignedAgency.Initials));
                ff.Pedestrian.Delete();
                EntryPoint.PersistentPedsDeleted++;
            }
        }
    }
    private void RemoveBlip(Ped ff)
    {
        if (!ff.Exists())
        {
            return;
        }
        Blip MyBlip = ff.GetAttachedBlip();
        if (MyBlip.Exists())
        {
            MyBlip.Delete();
        }
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
    private bool IsValidSpawn(SpawnLocation spawnLocation)
    {
        if (spawnLocation.StreetPosition.DistanceTo2D(Player.Position) < ClosestZombieSpawnToPlayerAllowed)
        {
            return false;
        }
        else if (spawnLocation.InitialPosition.DistanceTo2D(Player.Position) < ClosestZombieSpawnToPlayerAllowed)
        {
            return false;
        }
        return true;
    }
}