using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


public static class PoliceScanningSystem
{
    private static uint GameTimeInterval;
    private static uint LOSInterval;
    private static uint GameTimeCheckedKilled;
    private static Random rnd;
    private static List<Vehicle> ReplacedVehicles = new List<Vehicle>();   
    private static uint K9Interval;
    private static List<Entity> NewsTeam = new List<Entity>();
    private static Model K9Model = new Model("a_c_shepherd");
    private static Model LSPDMale = new Model("s_m_y_cop_01");
    private static Model LSPDFemale = new Model("s_f_y_cop_01");
    private static Model LSSDMale = new Model("s_m_y_sheriff_01");
    private static Model LSSDFemale = new Model("s_f_y_sheriff_01");
    private static Vehicle NewsChopper;
    private static uint RandomCopInterval;

    static PoliceScanningSystem()
    {
        rnd = new Random();
    }
    public static List<GTACop> CopPeds { get; private set; } = new List<GTACop>();
    public static List<GTACop> K9Peds { get; private set; } = new List<GTACop>();
    public static List<Ped> Civilians { get; private set; } = new List<Ped>();
    public static List<GTANewsReporter> Reporters { get; private set; } = new List<GTANewsReporter>();
    public static int ScanningInterval { get; private set; }
    public static float ScanningRange { get; private set; }
    public static float InnocentScanningRange { get; private set; }
    public static bool PlayerHurtPolice { get; set; } = false;
    public static bool PlayerKilledPolice { get; set; } = false;
    public static bool PlayerKilledCivilians { get; set; } = false;
    public static Vector3 PlacePlayerLastSeen { get; set; }
    public static bool IsRunning { get; set; } = true;
    public static GTACop PrimaryPursuer { get; set; }
    public static int CopsKilledByPlayer { get; set; } = 0;
    public static int CiviliansKilledByPlayer { get; set; } = 0;

    public static string AgenciesChasingPlayer
    {
        get
        {
            return string.Join(" ", CopPeds.Where(x => x.SeenPlayerSince(60000)).Select(x => x.AssignedAgency.ColorPrefix + x.AssignedAgency.Initials).Distinct().ToArray());
        }
    }

    public static void Initialize()
    {
        ScanningInterval = 5000;
        ScanningRange = 200f;
        InnocentScanningRange = 10f;
        K9Model.LoadAndWait();
        K9Model.LoadCollisionAndWait();

        LSPDMale.LoadAndWait();
        LSPDMale.LoadCollisionAndWait();
        LSPDFemale.LoadAndWait();
        LSPDFemale.LoadCollisionAndWait();
        LSSDMale.LoadAndWait();
        LSSDMale.LoadCollisionAndWait();
        LSSDFemale.LoadAndWait();
        LSSDFemale.LoadCollisionAndWait();

        MainLoop();
    }
    public static void Dispose()
    {
        IsRunning = false;
        PoliceSpawning.DeleteNewsTeam();
        foreach(GTACop Cop in CopPeds)
        {
            if(Cop.CopPed.Exists())
            {
                if (Cop.CopPed.IsInAnyVehicle(false))
                    Cop.CopPed.CurrentVehicle.Delete();
                Cop.CopPed.Delete();
            }
        }
        CopPeds.Clear();
        PoliceSpawning.RemoveAllCreatedEntities();
    }
    public static void MainLoop()
    {
        var stopwatch = new Stopwatch();
        GameFiber.StartNew(delegate
        {
            while (IsRunning)
            {
                stopwatch.Start();
                bool PlayerInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
                //Check if we have killed any cops
                int CheckKilledInterval = 200;
                if (Game.GameTime > GameTimeCheckedKilled + CheckKilledInterval) // was 2000
                {
                    CheckKilled();
                }
                if (Game.GameTime > GameTimeInterval + ScanningInterval)
                {
                    ScanForPolice();                        
                    GameTimeInterval = Game.GameTime;
                }
                int losInterval = 500;
                if (Game.GameTime > LOSInterval + losInterval) // was 2000
                {
                    CheckLOS(PlayerInVehicle, (PlayerInVehicle) ? (Entity)Game.LocalPlayer.Character.CurrentVehicle : (Entity)Game.LocalPlayer.Character);
                    SetPrimaryPursuer();
                    UpdatePlacePlayerLastSeen();
                }
                if (Settings.SpawnPoliceK9 && 1==0 && Game.GameTime > K9Interval + 5555) // was 2000
                {
                    if (Game.LocalPlayer.WantedLevel > 0 && !PlayerInVehicle && K9Peds.Count < 3)
                        PoliceSpawning.CreateK9();
                    PoliceSpawning.MoveK9s();
                    K9Interval = Game.GameTime;
                }
                if (Settings.SpawnRandomPolice && Game.GameTime > RandomCopInterval + 2000)
                {
                    if(Game.LocalPlayer.WantedLevel == 0 && CopPeds.Where(x => x.WasRandomSpawn).Count() < Settings.SpawnRandomPoliceLimit)
                        PoliceSpawning.SpawnRandomCop(true);
                    PoliceSpawning.RemoveFarAwayRandomlySpawnedCops();
                    RandomCopInterval = Game.GameTime;
                }
                stopwatch.Stop();
                if (stopwatch.ElapsedMilliseconds >= 20)
                    InstantAction.WriteToLog("PoliceScanningTick", string.Format("Tick took {0} ms", stopwatch.ElapsedMilliseconds));
                stopwatch.Reset();
                GameFiber.Yield();
            }
        });
    }


    //Police General
    private static void ScanForPolice()
    {
        Ped[] Pedestrians = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 250f, GetEntitiesFlags.ConsiderHumanPeds | GetEntitiesFlags.ExcludePlayerPed).Where(x => x is Ped).ToArray(), (x => (Ped)x));
        foreach (Ped Pedestrian in Pedestrians.Where(s => s.Exists() && !s.IsDead && s.IsVisible))
        {
            if(Pedestrian.isPoliceArmy())
            {
                if (!CopPeds.Any(x => x.CopPed == Pedestrian))
                {
                    bool canSee = false;
                    if (Pedestrian.PlayerIsInFront() && Pedestrian.IsInRangeOf(Game.LocalPlayer.Character.Position, 55f) && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Pedestrian, Game.LocalPlayer.Character))
                        canSee = true;


                    GTACop myCop = new GTACop(Pedestrian, canSee, canSee ? Game.GameTime : 0, canSee ? Game.LocalPlayer.Character.Position : new Vector3(0f, 0f, 0f), Pedestrian.Health,Agencies.GetAgencyFromPed(Pedestrian));
                    Pedestrian.IsPersistent = false;
                    if (Settings.OverridePoliceAccuracy)
                        Pedestrian.Accuracy = Settings.PoliceGeneralAccuracy;
                    Pedestrian.Inventory.Weapons.Clear();
                    IssueCopPistol(myCop);
                    NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Pedestrian, 7, false);//No commandeering//https://gtaforums.com/topic/833391-researchguide-combat-behaviour-flags/
                    if (InstantAction.GhostCop != null && InstantAction.GhostCop.Handle == Pedestrian.Handle)
                        continue;

                    CopPeds.Add(myCop);

                    if (Settings.IssuePoliceHeavyWeapons && InstantAction.CurrentPoliceState == InstantAction.PoliceState.DeadlyChase)
                        IssueCopHeavyWeapon(myCop);
                }
            }
            else
            {
                //if (!Civilians.Any(x => x == Pedestrian))
                //{
                //    Civilians.Add(Pedestrian);
                //}
            }
                
        }
        CopPeds.RemoveAll(x => !x.CopPed.Exists() || x.CopPed.IsDead);
        K9Peds.RemoveAll(x => !x.CopPed.Exists() || x.CopPed.IsDead);
        Civilians.RemoveAll(x => !x.Exists() || x.IsDead);
    }
    public static void UpdatePolice()
    {
        CopPeds.RemoveAll(x => !x.CopPed.Exists());
        K9Peds.RemoveAll(x => !x.CopPed.Exists() || x.CopPed.IsDead);
        foreach (GTACop Cop in CopPeds)
        {

            if(Cop.CopPed.IsDead)
            {
                if(PlayerHurtPed(Cop))
                {
                    Cop.HurtByPlayer = true;
                    PlayerHurtPolice = true;
                }
                if(PlayerKilledPed(Cop))
                {
                    CopsKilledByPlayer++;
                    PlayerKilledPolice = true;
                }
                continue;
            }
            int NewHealth = Cop.CopPed.Health;
            if(NewHealth != Cop.Health)
            {
                if(PlayerHurtPed(Cop))
                {
                    PlayerHurtPolice = true;
                    Cop.HurtByPlayer = true;
                    InstantAction.WriteToLog("UpdatePolice", String.Format("Cop {0}, Was hurt by player", Cop.CopPed.Handle));
                }
                InstantAction.WriteToLog("UpdatePolice", String.Format("Cop {0}, Health Changed from {1} to {2}", Cop.CopPed.Handle,Cop.Health,NewHealth));
                Cop.Health = NewHealth;
            }
            Cop.isInVehicle = Cop.CopPed.IsInAnyVehicle(false);
            if (Cop.isInVehicle)
            {
                Cop.isInHelicopter = Cop.CopPed.IsInHelicopter;
                if(!Cop.isInHelicopter)
                    Cop.isOnBike = Cop.CopPed.IsOnBike;
            }
            else
            {
                Cop.isInHelicopter = false;
                Cop.isOnBike = false;
            }
                
            Cop.DistanceToPlayer = Cop.CopPed.RangeTo(Game.LocalPlayer.Character.Position);
            Cop.DistanceToLastSeen = Cop.CopPed.RangeTo(PlacePlayerLastSeen);
        }
        CopPeds.RemoveAll(x => x.CopPed.IsDead);
    }
    private static void CheckKilled()
    {
        foreach (GTACop Cop in CopPeds.Where(x => x.CopPed.Exists() && x.CopPed.IsDead))
        {
            if (PlayerKilledPed(Cop))
            {
                CopsKilledByPlayer++;
                PlayerKilledPolice = true;
            }
        }
        foreach (Ped Pedestrian in Civilians.Where(x => x.Exists() && x.IsDead))
        {
            if (PlayerKilledPed(Pedestrian))
            {
                CiviliansKilledByPlayer++;
                PlayerKilledCivilians = true;
            }
        }
        CopPeds.RemoveAll(x => !x.CopPed.Exists() || x.CopPed.IsDead);
        Civilians.RemoveAll(x => !x.Exists() || x.IsDead);
    }

    private static bool PlayerHurtPed(GTACop Cop)
    {
        if (NativeFunction.CallByName<bool>("HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY", Cop.CopPed, Game.LocalPlayer.Character, true))
        {
            InstantAction.WriteToLog("PlayerHurtPed", string.Format("Hurt: {0}", Cop.CopPed.Handle));
            return true;
                
        }
        else if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && NativeFunction.CallByName<bool>("HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY", Cop.CopPed, Game.LocalPlayer.Character.CurrentVehicle, true))
        {
            InstantAction.WriteToLog("PlayerHurtPed", string.Format("Hurt with Car: {0}", Cop.CopPed.Handle));
            return true;
        }
        return false;
    }
    private static bool PlayerKilledPed(GTACop Cop)
    {
        try
        {
            if (Cop.CopPed.IsDead)
            {
                Entity killer = NativeFunction.Natives.GetPedSourceOfDeath<Entity>(Cop.CopPed);
                if(killer.Handle == Game.LocalPlayer.Character.Handle || (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Handle == killer.Handle))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }
        catch (Exception e)
        {
            // Game.LogTrivial(e.Message);
            InstantAction.WriteToLog("PlayerKilledPed", string.Format("Cop got killed by unknow, attributing it to you must be GSW2?{0}, DId you hurt them?: {1}", Cop.CopPed.Handle, Cop.HurtByPlayer));
            if (Cop.HurtByPlayer)
                return true;
            else
                return false;
        }
    }
    private static bool PlayerKilledPed(Ped Pedestrian)
    {
        try
        {
            if (Pedestrian.IsDead)
            {
                Entity killer = NativeFunction.Natives.GetPedSourceOfDeath<Entity>(Pedestrian);
                if (killer.Handle == Game.LocalPlayer.Character.Handle || (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Handle == killer.Handle))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }
        catch (Exception e)
        {
                return false;
        }
    }
    private static void CheckLOS(bool PlayerInVehicle,Entity EntityToCheck)
    {
        int TotalEntityNativeLOSChecks = 0;
        bool SawPlayerThisCheck = false;
        float RangeToCheck = 55f;

        foreach (GTACop Cop in CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.CopPed.IsInHelicopter))
        {
            if (SawPlayerThisCheck && TotalEntityNativeLOSChecks >= 3 && Cop.GameTimeLastLOSCheck <= 1500)//we have already done 3 checks, saw us and they were looked at last check
            {
                //InstantAction.WriteToLog("CheckLOS", "Skipped Ped checking LOS");
                break;
            }
            Cop.GameTimeLastLOSCheck = Game.GameTime;
            if (Cop.CopPed.PlayerIsInFront() && Cop.DistanceToPlayer <= RangeToCheck && !Cop.CopPed.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Cop.CopPed, EntityToCheck))//if (Cop.CopPed.PlayerIsInFront() && Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, RangeToCheck) && !Cop.CopPed.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Cop.CopPed, EntityToCheck)) //was 55f
            {
                Cop.UpdateContinuouslySeen();
                Cop.canSeePlayer = true;
                Cop.GameTimeLastSeenPlayer = Game.GameTime;
                Cop.PositionLastSeenPlayer = Game.LocalPlayer.Character.Position;
                SawPlayerThisCheck = true;
                //if (PlayerInVehicle)
                //    break;
            }
            else
            {
                Cop.GameTimeContinuoslySeenPlayerSince = 0;
                Cop.canSeePlayer = false;
            }
            TotalEntityNativeLOSChecks++;
        }
        if (SawPlayerThisCheck)
            return;
        foreach (GTACop Cop in CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && x.CopPed.IsInHelicopter))
        {
            Cop.GameTimeLastLOSCheck = Game.GameTime;
            if (Cop.DistanceToPlayer <= 250f && !Cop.CopPed.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY", Cop.CopPed, EntityToCheck, 17)) //was 55f
            {
                Cop.UpdateContinuouslySeen();
                Cop.canSeePlayer = true;
                Cop.GameTimeLastSeenPlayer = Game.GameTime;
                Cop.PositionLastSeenPlayer = Game.LocalPlayer.Character.Position;
                break;//Only care if one of the people saw it as we wont be tasking them 
            }
            else
            {
                Cop.GameTimeContinuoslySeenPlayerSince = 0;
                Cop.canSeePlayer = false;
            }
        }


    }
    public static bool PoliceCanSeeEntity(Entity EntityToCheck)
    {
        if (!EntityToCheck.Exists())
            return false;

        float RangeToCheck = 55f;

        foreach (GTACop Cop in CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.CopPed.IsInHelicopter))
        {
            if (EntityToCheck.IsInFront(Cop.CopPed) && Cop.CopPed.IsInRangeOf(EntityToCheck.Position, RangeToCheck) && !Cop.CopPed.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Cop.CopPed, EntityToCheck)) //was 55f
            {
                return true;
            }
        }
        foreach (GTACop Cop in CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && x.CopPed.IsInHelicopter))
        {
            if (Cop.CopPed.IsInRangeOf(EntityToCheck.Position, 250f) && !Cop.CopPed.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY", Cop.CopPed, EntityToCheck, 17)) //was 55f
            {
                return true;
            }
        }
        return false;
    }
    private static void SetPrimaryPursuer()
    {
        if (CopPeds.Count == 0)
            return;
        foreach (GTACop Cop in CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.CopPed.IsInHelicopter))
        {
            Cop.isPursuitPrimary = false;
        }
        GTACop PursuitPrimary = CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.CopPed.IsInAnyVehicle(false)).OrderBy(x => x.CopPed.Position.DistanceTo2D(Game.LocalPlayer.Character.Position)).FirstOrDefault();
        if (PursuitPrimary == null)
        {
            PrimaryPursuer = null;
            return;
        }
        else
        {
            PrimaryPursuer = PursuitPrimary;
            PursuitPrimary.isPursuitPrimary = true;
        }
    }
    public static void IssueCopPistol(GTACop Cop)
    {
        GTAWeapon Pistol;
        Pistol = InstantAction.Weapons.Where(x => x.isPoliceIssue && x.Category == GTAWeapon.WeaponCategory.Pistol).PickRandom();
        Cop.IssuedPistol = Pistol;
        Cop.CopPed.Inventory.GiveNewWeapon(Pistol.Name, Pistol.AmmoAmount, false);
        if (Settings.AllowPoliceWeaponVariations)
        {
            WeaponVariation MyVariation = Pistol.PoliceVariations.PickRandom();
            Cop.PistolVariation = MyVariation;
            InstantAction.ApplyWeaponVariation(Cop.CopPed, (uint)Pistol.Hash, MyVariation);
        }
    }
    public static void IssueCopHeavyWeapon(GTACop Cop)
    {
        GTAWeapon IssuedHeavy;
        int Num = rnd.Next(1, 5);
        if (Num == 1)
            IssuedHeavy = InstantAction.Weapons.Where(x => x.isPoliceIssue && x.Category == GTAWeapon.WeaponCategory.AR).PickRandom();
        else if (Num == 2)
            IssuedHeavy = InstantAction.Weapons.Where(x => x.isPoliceIssue && x.Category == GTAWeapon.WeaponCategory.Shotgun).PickRandom();
        else if (Num == 3)
            IssuedHeavy = InstantAction.Weapons.Where(x => x.isPoliceIssue && x.Category == GTAWeapon.WeaponCategory.SMG).PickRandom();
        else if (Num == 4)
            IssuedHeavy = InstantAction.Weapons.Where(x => x.isPoliceIssue && x.Category == GTAWeapon.WeaponCategory.AR).PickRandom();
        else
            IssuedHeavy = InstantAction.Weapons.Where(x => x.isPoliceIssue && x.Category == GTAWeapon.WeaponCategory.AR).PickRandom();

        Cop.IssuedHeavyWeapon = IssuedHeavy;
        Cop.CopPed.Inventory.GiveNewWeapon(IssuedHeavy.Name, IssuedHeavy.AmmoAmount, true);
        if (Settings.OverridePoliceAccuracy)
            Cop.CopPed.Accuracy = Settings.PoliceHeavyAccuracy;
        if (Settings.AllowPoliceWeaponVariations)
        {
            WeaponVariation MyVariation = IssuedHeavy.PoliceVariations.PickRandom();
            Cop.HeavyVariation = MyVariation;
            InstantAction.ApplyWeaponVariation(Cop.CopPed, (uint)IssuedHeavy.Hash, MyVariation);
        }
    }
    public static Vector3 UpdatePlacePlayerLastSeen()
    {
        if (!CopPeds.Any(x => x.GameTimeLastSeenPlayer > 0))
            return new Vector3(0f, 0f, 0f);
        else
            return CopPeds.Where(x => x.GameTimeLastSeenPlayer > 0).OrderByDescending(x => x.GameTimeLastSeenPlayer).FirstOrDefault().PositionLastSeenPlayer;
            
    }


}
