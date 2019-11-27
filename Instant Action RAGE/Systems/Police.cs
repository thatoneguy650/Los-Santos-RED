using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static class Police
{
    public static uint WantedLevelStartTime;
    public static Vector3 LastWantedCenterPosition = Vector3.Zero;

    private static int TimeAimedAtPolice = 0;

    private static bool firedWeapon = false;
    private static bool aimedAtPolice = false;
    public static bool SurrenderBust = false;
    private static uint LastBust;
    private static int ForceSurrenderTime;
    public static Model CopModel = new Model("s_m_y_cop_01");

    public static Ped GhostCop;

    private static bool CanReportLastSeen;
    private static uint GameTimeLastGreyedOut;
    private static bool GhostCopFollow;
    public static List<Blip> CreatedBlips = new List<Blip>();
    public static uint GameTimeWantedStarted;
    public static bool PlayerBreakingIntoCar = false;
    public static bool PlayerChangingPlate = false;
    public static uint GameTimeLastWantedEnded;
    public static bool PlayerArtificiallyShooting = false;
    public static Vector3 PlaceWantedStarted;
    public static Blip LastWantedCenterBlip;
    public static Blip CurrentWantedCenterBlip;

    private static bool PrevAnyPoliceRecentlySeenPlayer;
    public static bool PrevAnyCanRecognizePlayer;
    public static int PrevWantedLevel = 0;
    private static bool PrevaimedAtPolice;
    private static uint GameTimePoliceStateStart;
    public static PoliceState PrevPoliceState = PoliceState.Normal;
    public static PoliceState LastPoliceState = PoliceState.Normal;
    private static bool PrevfiredWeapon = false;
    private static bool PrevPlayerHurtPolice = false;
    private static bool PrevPlayerKilledPolice = false;
    private static int PrevCopsKilledByPlayer = 0;
    private static bool PrevPlayerStarsGreyedOut;
    private static uint GameTimeLastPoliceTick;
    private static uint GameTimeLastSetWanted;
    public static string TempCurrentPoliceTickRunning = "";
    private static int PrevCiviliansKilledByPlayer;
    public static bool PlayerIsPersonOfInterest = false;
    public static bool PrevPlayerIsJacking = false;
    public static bool PlayerIsJacking = false;
    public static uint GameTimeLastStartedJacking = 0;
    private static Random rnd;
    public static bool IsNightTime = false;

    public static bool AnyPoliceCanSeePlayer { get; set; } = false;
    public static bool AnyPoliceCanRecognizePlayer { get; set; } = false;
    public static bool AnyPoliceCanRecognizePlayerAfterWanted { get; set; } = false;
    public static bool AnyPoliceRecentlySeenPlayer { get; set; } = false;
    public static PoliceState CurrentPoliceState { get; set; }
    public static bool PlayerStarsGreyedOut { get; set; } = false;
    public static bool AnyPoliceSeenPlayerThisWanted { get; set; } = false;

    public static GTACop PrimaryPursuer { get; set; }
    public static int CopsKilledByPlayer { get; set; } = 0;
    public static int CiviliansKilledByPlayer { get; set; } = 0;
    public static bool PlayerHurtPolice { get; set; } = false;
    public static bool PlayerKilledPolice { get; set; } = false;
    public static bool PlayerKilledCivilians { get; set; } = false;
    public static Vector3 PlacePlayerLastSeen { get; set; }


    public static bool PlayerWasJustJacking
    {
        get
        {
            if (GameTimeLastStartedJacking == 0)
                return false;
            else
                return Game.GameTime - GameTimeLastStartedJacking >= 5000;
        }
    }
    public static bool RecentlySetWanted
    {
        get
        {
            if (GameTimeLastSetWanted == 0)
                return false;
            else if (Game.GameTime - GameTimeLastSetWanted <= 5000)
                return true;
            else
                return false;
        }
    }
    public static uint PlayerHasBeenNotWantedFor//seconds
    {
        get
        {
            if (Game.LocalPlayer.WantedLevel != 0)
                return 0;
            if (GameTimeLastWantedEnded == 0)
                return 0;
            else
                return (Game.GameTime - GameTimeLastWantedEnded);
        }
    }

    public static uint PlayerHasBeenWantedFor//seconds
    {
        get
        {
            if (Game.LocalPlayer.WantedLevel == 0)
                return 0;
            else
                return (Game.GameTime - WantedLevelStartTime);
        }
    }

    public static bool IsRunning { get; set; } = true;
    static Police()
    {
        rnd = new Random();
    }
    public enum PoliceState
    {
        Normal = 0,
        UnarmedChase = 1,
        CautiousChase = 2,
        DeadlyChase = 3,
        ArrestedWait = 4,
    }
    //public static void Initialize()
    //{
    //    MainLoop();
    //}
    //public static void MainLoop()
    //{
    //    var stopwatch = new Stopwatch();
    //    GameFiber.StartNew(delegate
    //    {
    //        while (IsRunning)
    //        {
    //            stopwatch.Start();
    //            PoliceTick();
    //            stopwatch.Stop();
    //            if (stopwatch.ElapsedMilliseconds >= 16)
    //                InstantAction.WriteToLog("PoliceTick", string.Format("Tick took {0} ms", stopwatch.ElapsedMilliseconds));
    //            stopwatch.Reset();
    //            GameFiber.Yield();
    //        }

    //    });
    //}
    public static void RemoveWantedBlips()
    {
        LastWantedCenterPosition = Vector3.Zero;
        if (LastWantedCenterBlip.Exists())
            LastWantedCenterBlip.Delete();
        if (CurrentWantedCenterBlip.Exists())
            CurrentWantedCenterBlip.Delete();
    }
    public static void Tick()
    {
        try
        {
            UpdatePolice();
            GetPoliceState();
            PoliceVehicleTick();
            CheckPoliceEvents();
            TrackedVehiclesTick();

            if (Game.GameTime > GameTimeLastPoliceTick + 100)
            {

                if (CurrentPoliceState == PoliceState.Normal)
                {
                    TempCurrentPoliceTickRunning = "Normal";
                    PoliceTickNormal();
                }
                else if (PlayerStarsGreyedOut && PoliceScanning.CopPeds.All(x => !x.RecentlySeenPlayer()))
                {
                    TempCurrentPoliceTickRunning = "Search Mode";
                    PoliceTickSearchMode();
                }
                else if (CurrentPoliceState == PoliceState.UnarmedChase)
                {
                    TempCurrentPoliceTickRunning = "Unarmed Chase";
                    PoliceTickUnarmedChase();
                }
                else if (CurrentPoliceState == PoliceState.CautiousChase)
                {
                    TempCurrentPoliceTickRunning = "Cautious Chase";
                    PoliceTickCautiousChase();
                }
                else if (CurrentPoliceState == PoliceState.DeadlyChase)
                {
                    TempCurrentPoliceTickRunning = "Deadly Chase";
                    PoliceTickDeadlyChase();
                }
                else if (CurrentPoliceState == PoliceState.ArrestedWait)
                {
                    TempCurrentPoliceTickRunning = "Arrested Wait";
                    PoliceTickArrestedWait();
                }
                else
                {
                    TempCurrentPoliceTickRunning = "Normal";
                    PoliceTickNormal();
                }

                GameTimeLastPoliceTick = Game.GameTime;
            }

            WantedLevelTick();
        }
        catch (Exception e)
        {
            Game.LogTrivial(e.StackTrace);
        }
    }

    //Update
    public static void UpdatePolice()
    {
        PoliceScanning.CopPeds.RemoveAll(x => !x.CopPed.Exists());
        PoliceScanning.K9Peds.RemoveAll(x => !x.CopPed.Exists() || x.CopPed.IsDead);
        foreach (GTACop Cop in PoliceScanning.CopPeds)
        {

            if (Cop.CopPed.IsDead)
            {
                if (PlayerHurtPed(Cop))
                {
                    Cop.HurtByPlayer = true;
                    PlayerHurtPolice = true;
                }
                if (PlayerKilledPed(Cop))
                {
                    CopsKilledByPlayer++;
                    PlayerKilledPolice = true;
                }
                continue;
            }
            int NewHealth = Cop.CopPed.Health;
            if (NewHealth != Cop.Health)
            {
                if (PlayerHurtPed(Cop))
                {
                    PlayerHurtPolice = true;
                    Cop.HurtByPlayer = true;
                    InstantAction.WriteToLog("UpdatePolice", String.Format("Cop {0}, Was hurt by player", Cop.CopPed.Handle));
                }
                InstantAction.WriteToLog("UpdatePolice", String.Format("Cop {0}, Health Changed from {1} to {2}", Cop.CopPed.Handle, Cop.Health, NewHealth));
                Cop.Health = NewHealth;
            }
            Cop.isInVehicle = Cop.CopPed.IsInAnyVehicle(false);
            if (Cop.isInVehicle)
            {
                Cop.isInHelicopter = Cop.CopPed.IsInHelicopter;
                if (!Cop.isInHelicopter)
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
        PoliceScanning.CopPeds.RemoveAll(x => x.CopPed.IsDead);
    }
    public static void CheckKilled()
    {
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.CopPed.Exists() && x.CopPed.IsDead))
        {
            if (PlayerKilledPed(Cop))
            {
                CopsKilledByPlayer++;
                PlayerKilledPolice = true;
            }
        }
        foreach (Ped Pedestrian in PoliceScanning.Civilians.Where(x => x.Exists() && x.IsDead))
        {
            if (PlayerKilledPed(Pedestrian))
            {
                CiviliansKilledByPlayer++;
                PlayerKilledCivilians = true;
            }
        }
        PoliceScanning.CopPeds.RemoveAll(x => !x.CopPed.Exists() || x.CopPed.IsDead);
        PoliceScanning.Civilians.RemoveAll(x => !x.Exists() || x.IsDead);
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
    public static void CheckLOS(Entity EntityToCheck)
    {
        int TotalEntityNativeLOSChecks = 0;
        bool SawPlayerThisCheck = false;
        float RangeToCheck = 55f;

        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.CopPed.IsInHelicopter))
        {
            if (SawPlayerThisCheck && TotalEntityNativeLOSChecks >= 3 && Cop.GameTimeLastLOSCheck <= 1500)//we have already done 3 checks, saw us and they were looked at last check
            {
                //InstantAction.WriteToLog("CheckLOS", "Skipped Ped checking LOS");
                break;
            }
            Cop.GameTimeLastLOSCheck = Game.GameTime;
            if (Cop.DistanceToPlayer <= RangeToCheck && Cop.CopPed.PlayerIsInFront() && !Cop.CopPed.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Cop.CopPed, EntityToCheck))//if (Cop.CopPed.PlayerIsInFront() && Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, RangeToCheck) && !Cop.CopPed.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Cop.CopPed, EntityToCheck)) //was 55f
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
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && x.CopPed.IsInHelicopter))
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

        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.CopPed.IsInHelicopter))
        {
            if (EntityToCheck.IsInFront(Cop.CopPed) && Cop.CopPed.IsInRangeOf(EntityToCheck.Position, RangeToCheck) && !Cop.CopPed.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Cop.CopPed, EntityToCheck)) //was 55f
            {
                return true;
            }
        }
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && x.CopPed.IsInHelicopter))
        {
            if (Cop.CopPed.IsInRangeOf(EntityToCheck.Position, 250f) && !Cop.CopPed.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY", Cop.CopPed, EntityToCheck, 17)) //was 55f
            {
                return true;
            }
        }
        return false;
    }
    public static void SetPrimaryPursuer()
    {
        if (PoliceScanning.CopPeds.Count == 0)
            return;
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.CopPed.IsInHelicopter))
        {
            Cop.isPursuitPrimary = false;
        }
        GTACop PursuitPrimary = PoliceScanning.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.CopPed.IsInAnyVehicle(false)).OrderBy(x => x.CopPed.Position.DistanceTo2D(Game.LocalPlayer.Character.Position)).FirstOrDefault();
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
        Pistol = GTAWeapons.WeaponsList.Where(x => x.isPoliceIssue && x.Category == GTAWeapon.WeaponCategory.Pistol).PickRandom();
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
            IssuedHeavy = GTAWeapons.WeaponsList.Where(x => x.isPoliceIssue && x.Category == GTAWeapon.WeaponCategory.AR).PickRandom();
        else if (Num == 2)
            IssuedHeavy = GTAWeapons.WeaponsList.Where(x => x.isPoliceIssue && x.Category == GTAWeapon.WeaponCategory.Shotgun).PickRandom();
        else if (Num == 3)
            IssuedHeavy = GTAWeapons.WeaponsList.Where(x => x.isPoliceIssue && x.Category == GTAWeapon.WeaponCategory.SMG).PickRandom();
        else if (Num == 4)
            IssuedHeavy = GTAWeapons.WeaponsList.Where(x => x.isPoliceIssue && x.Category == GTAWeapon.WeaponCategory.AR).PickRandom();
        else
            IssuedHeavy = GTAWeapons.WeaponsList.Where(x => x.isPoliceIssue && x.Category == GTAWeapon.WeaponCategory.AR).PickRandom();

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
        if (!PoliceScanning.CopPeds.Any(x => x.GameTimeLastSeenPlayer > 0))
            return new Vector3(0f, 0f, 0f);
        else
            return PoliceScanning.CopPeds.Where(x => x.GameTimeLastSeenPlayer > 0).OrderByDescending(x => x.GameTimeLastSeenPlayer).FirstOrDefault().PositionLastSeenPlayer;

    }


    private static void GetPoliceState()
    {
        if (CurrentPoliceState == PoliceState.ArrestedWait || CurrentPoliceState == PoliceState.DeadlyChase)
            return;

        if (InstantAction.PlayerWantedLevel == 0)
            CurrentPoliceState = PoliceState.Normal;//Default state
        else if (InstantAction.PlayerWantedLevel >= 1 && InstantAction.PlayerWantedLevel <= 3 && AnyPoliceCanSeePlayer)//AnyCanSeePlayer)
        {
            if ((!firedWeapon && !PlayerHurtPolice && !aimedAtPolice) && !InstantAction.PlayerIsConsideredArmed) // Unarmed and you havent killed anyone
                CurrentPoliceState = PoliceState.UnarmedChase;
            else if ((!firedWeapon && !PlayerHurtPolice && !aimedAtPolice))
                CurrentPoliceState = PoliceState.CautiousChase;
            else
                CurrentPoliceState = PoliceState.DeadlyChase;

        }
        else if (InstantAction.PlayerWantedLevel >= 4 || PlayerHurtPolice || PlayerKilledPolice)
            CurrentPoliceState = PoliceState.DeadlyChase;

        if (AnyPoliceCanRecognizePlayer && InstantAction.PlayerIsConsideredArmed && InstantAction.PlayerWantedLevel < 3 && !InstantAction.PlayerInVehicle)
        {
            ulong myHash = (ulong)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash;
            GTAWeapon MatchedWeapon = GTAWeapons.GetWeaponFromHash(myHash);//InstantAction.Weapons.Where(x => x.Hash == myHash).FirstOrDefault();
            bool ChangedWanted = false;
            if (MatchedWeapon != null)
            {
                if (MatchedWeapon.WeaponLevel >= 3 && InstantAction.PlayerWantedLevel < 3)
                {
                    ChangedWanted = true;
                    SetWantedLevel(3);
                }
                else if (InstantAction.PlayerWantedLevel < 2)
                {
                    ChangedWanted = true;
                    SetWantedLevel(2);
                    InstantAction.WriteToLog("GetPoliceState", "Cops Saw you with gun");
                }
            }
            else if (InstantAction.PlayerWantedLevel < 2)
            {
                ChangedWanted = true;
                SetWantedLevel(2);
                InstantAction.WriteToLog("GetPoliceState", "Cops Saw you with gun");
            }

            if (ChangedWanted)
            {
                DispatchAudio.DispatchQueueItem CarryingWeapon = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportCarryingWeapon, 3, false);
                CarryingWeapon.WeaponToReport = MatchedWeapon;
                DispatchAudio.AddDispatchToQueue(CarryingWeapon);
            }
        }

        if (!aimedAtPolice && InstantAction.PlayerIsConsideredArmed && Game.LocalPlayer.IsFreeAiming && AnyPoliceCanSeePlayer && PoliceScanning.CopPeds.Any(x => Game.LocalPlayer.IsFreeAimingAtEntity(x.CopPed)))
        {
            TimeAimedAtPolice++;
        }
        else
        {
            TimeAimedAtPolice = 0;
        }

        if (TimeAimedAtPolice >= 100 && InstantAction.PlayerWantedLevel < 3)
        {
            SetWantedLevel(3);
            aimedAtPolice = true;
        }

        if (!firedWeapon && (Game.LocalPlayer.Character.IsShooting || PlayerArtificiallyShooting) && (PoliceScanning.CopPeds.Any(x => x.canSeePlayer || (x.DistanceToPlayer <= 100f && !Game.LocalPlayer.Character.IsCurrentWeaponSilenced)))) //if (!firedWeapon && Game.LocalPlayer.Character.IsShooting && (PoliceScanning.CopPeds.Any(x => x.canSeePlayer || x.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 100f))))
        {
            SetWantedLevel(2);
            firedWeapon = true;
            InstantAction.WriteToLog("GetPoliceState", "fired gun");
        }

        if (InstantAction.PlayerWantedLevel < 2 && PlayerChangingPlate && AnyPoliceCanSeePlayer)
        {
            SetWantedLevel(2);
            DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspiciousActivity, 3, false));
            InstantAction.WriteToLog("GetPoliceState", "Changing plate");
        }

        if (InstantAction.PlayerWantedLevel < 2 && PlayerBreakingIntoCar && AnyPoliceCanSeePlayer)
        {
            SetWantedLevel(2);
            DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportGrandTheftAuto, 3, false));
            InstantAction.WriteToLog("GetPoliceState", "breaking into car");
        }

        if (InstantAction.PlayerWantedLevel > 0 && (InstantAction.PlayerWantedLevel <= 3 || CurrentPoliceState != PoliceState.DeadlyChase) && Game.LocalPlayer.Character.DistanceTo2D(new Vector3(1696f, 2566f, 0f)) <= 150f)
        {
            SetWantedLevel(2);
            CurrentPoliceState = PoliceState.DeadlyChase;
            InstantAction.WriteToLog("GetPoliceState", "WEnt to close to the prison");
        }

        //if (PlayerIsPersonOfInterest && InstantAction.PlayerWantedLevel == 0 && AnyPoliceCanRecognizePlayerAfterWanted && PlayerHasBeenNotWantedFor >= 5000 && PlayerHasBeenNotWantedFor <= 120000)
        //{
        //    Game.LocalPlayer.WantedLevel = 3;
        //    AddDispatchToQueue(new DispatchQueueItem(ReportDispatch.ReportSuspectSpotted, 3, false));
        //    InstantAction.WriteToLog("Cops Reacquired after losing them", "");
        //}
        //if (PlayerIsPersonOfInterest && InstantAction.PlayerWantedLevel == 0 && AnyPoliceCanSeePlayer && PlayerHasBeenNotWantedFor >= 5000 && PlayerHasBeenNotWantedFor <= 120000 && LastWantedCenterPosition != Vector3.Zero && Game.LocalPlayer.Character.DistanceTo2D(LastWantedCenterPosition) <= 250f)
        //{
        //    Game.LocalPlayer.WantedLevel = 3;
        //    AddDispatchToQueue(new DispatchQueueItem(ReportDispatch.ReportSuspectSpotted, 3, false));
        //    InstantAction.WriteToLog("Cops Reacquired after losing them in the same area", "");
        //}


        if (PlayerHasBeenNotWantedFor >= 120000)
        {
            PlayerIsPersonOfInterest = false;
            if (LastWantedCenterBlip.Exists())
                LastWantedCenterBlip.Delete();
        }
    }
    private static void PoliceTickNormal()
    {
        foreach (GTACop Cop in PoliceScanning.CopPeds)
        {
            if (Cop.isTasked && !Cop.TaskIsQueued && Cop.TaskType != PoliceTask.Task.RandomSpawnIdle)
            {
                Tasking.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Untask));
                InstantAction.WriteToLog("InstantActionTick", string.Format("Untask Queued {0}", Cop.CopPed.Handle));
            }
            else if (Cop.WasRandomSpawn && !Cop.isTasked && !Cop.TaskIsQueued)
            {
                Tasking.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.RandomSpawnIdle));
                InstantAction.WriteToLog("InstantActionTick", string.Format("RandomSpawnIdle Queued {0}", Cop.CopPed.Handle));
            }
        }
        if (Game.GameTime - GameTimePoliceStateStart >= 8000)
        {
            foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.SetDeadly || x.SetTazer || x.SetUnarmed))
            {
                ResetCopWeapons(Cop);//just in case they get stuck
            }
        }
    }
    private static void PoliceTickUnarmedChase()
    {
        //Cops on Foot
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => !x.isTasked))
        {
            if (Cop.CopPed.IsOnBike || Cop.CopPed.IsInHelicopter)
                SetUnarmed(Cop);
            else
                SetCopTazer(Cop);

            if (Cop.DistanceToPlayer <= 55f)
            {
                int TotalFootChaseTasked = PoliceScanning.CopPeds.Where(x => (x.isTasked || x.TaskIsQueued) && x.TaskType == PoliceTask.Task.Chase).Count();
                int TotalVehicleChaseTasked = PoliceScanning.CopPeds.Where(x => (x.isTasked || x.TaskIsQueued) && x.TaskType == PoliceTask.Task.VehicleChase).Count();

                if (!InstantAction.isBusted && Cop.RecentlySeenPlayer() && !Cop.TaskIsQueued && TotalFootChaseTasked <= 4 && !Cop.CopPed.IsInAnyVehicle(false) && Cop.DistanceToPlayer <= 55f && (!Game.LocalPlayer.Character.IsInAnyVehicle(false) || Game.LocalPlayer.Character.CurrentVehicle.Speed <= 5f))
                {
                    Cop.TaskIsQueued = true;
                    Tasking.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Chase));
                }
                else if (!InstantAction.isBusted && Cop.RecentlySeenPlayer() && !Cop.TaskIsQueued && TotalFootChaseTasked > 0 && TotalVehicleChaseTasked <= 5 && Cop.isInVehicle && !Cop.isInHelicopter && Cop.DistanceToPlayer <= 55f && !Game.LocalPlayer.Character.IsInAnyVehicle(false))
                {
                    Cop.TaskIsQueued = true;
                    Tasking.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.VehicleChase));
                }
                if ((InstantAction.areHandsUp || Game.LocalPlayer.Character.IsStunned || Game.LocalPlayer.Character.IsRagdoll) && !InstantAction.isBusted && Cop.DistanceToPlayer <= 4f && !PlayerWasJustJacking)
                    SurrenderBust = true;
            }
        }
        if (PoliceScanning.CopPeds.Any(x => x.DistanceToPlayer <= 4f) && (Game.LocalPlayer.Character.IsRagdoll || Game.LocalPlayer.Character.Speed <= 4.0f) && !InstantAction.PlayerInVehicle && !InstantAction.isBusted)
            SurrenderBust = true;

        if (SurrenderBust && !isBustTimeOut())
            SurrenderBustEvent();

        StopSearchMode();
    }
    private static void PoliceTickArrestedWait()
    {
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => !x.isTasked)) // Exist/Dead Check
        {
            bool InVehicle = Cop.CopPed.IsInAnyVehicle(false);
            if (InVehicle)
            {
                SetUnarmed(Cop);
            }
            else
            {
                if (!Cop.TaskIsQueued && PoliceScanning.CopPeds.Where(x => x.isTasked || x.TaskIsQueued).Count() <= 3 && Cop.DistanceToPlayer <= 45f)
                {
                    Cop.TaskIsQueued = true;
                    Tasking.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Arrest));
                }
                else if (!Cop.TaskIsQueued && (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.NoTask || Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.Preparing || Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.Interrupted) && (Cop.RecentlySeenPlayer() || Cop.DistanceToPlayer <= 65f))
                {
                    Cop.TaskIsQueued = true;
                    Cop.GameTimeLastTask = Game.GameTime;
                    Tasking.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.SimpleArrest));
                }
                else if (!Cop.TaskIsQueued && Game.GameTime - Cop.GameTimeLastTask > 3500 && Cop.RecentlySeenPlayer() && Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.InProgress && Cop.DistanceToPlayer > 45f)
                {
                    Cop.TaskIsQueued = true;
                    Cop.GameTimeLastTask = Game.GameTime;
                    Tasking.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.SimpleArrest)); //retask the arrest
                }
            }
        }
        SetWantedLevel(InstantAction.MaxWantedLastLife);

        if (PoliceScanning.CopPeds.Any(x => x.DistanceToPlayer <= 4f) && (Game.LocalPlayer.Character.IsRagdoll || Game.LocalPlayer.Character.Speed <= 4.0f) && !InstantAction.PlayerInVehicle && !InstantAction.isBusted)
            SurrenderBust = true;

        if (SurrenderBust && !isBustTimeOut())
            SurrenderBustEvent();

        StopSearchMode();
    }
    private static void PoliceTickCautiousChase()
    {
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => !x.isTasked && !x.isInVehicle && !x.isInHelicopter))
        {
            SetCopDeadly(Cop);
            if (!Cop.TaskIsQueued && PoliceScanning.CopPeds.Where(x => x.isTasked || x.TaskIsQueued).Count() <= 4 && Cop.DistanceToPlayer <= 45f)
            {
                Cop.TaskIsQueued = true;
                Tasking.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Arrest));
            }
            else if (!Cop.TaskIsQueued && Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.NoTask && (Cop.RecentlySeenPlayer() || Cop.DistanceToPlayer <= 65f))
            {
                Cop.TaskIsQueued = true;
                Cop.GameTimeLastTask = Game.GameTime;
                Tasking.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.SimpleArrest));
            }
            else if (!Cop.TaskIsQueued && Game.GameTime - Cop.GameTimeLastTask > 3500 && Cop.RecentlySeenPlayer() && Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.InProgress && Cop.DistanceToPlayer > 35f)
            {
                Cop.TaskIsQueued = true;
                Cop.GameTimeLastTask = Game.GameTime;
                Tasking.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.SimpleArrest));
            }

        }
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.isTasked && x.SimpleTaskName != ""))
        {
            if (!Cop.TaskIsQueued && Game.GameTime - Cop.GameTimeLastTask > 20000 && Cop.RecentlySeenPlayer() && Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.InProgress && Cop.DistanceToPlayer > 25f)
            {
                Cop.TaskIsQueued = true;
                Cop.GameTimeLastTask = Game.GameTime;
                Tasking.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.SimpleArrest));
            }
            else if (!Cop.TaskIsQueued && Game.GameTime - Cop.GameTimeLastTask > 20000 && !Cop.RecentlySeenPlayer() && Cop.DistanceToPlayer > 35f)
            {
                Cop.TaskIsQueued = true;
                Tasking.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Untask));
            }

        }
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => !x.isTasked && (x.isInHelicopter || x.isOnBike)))
        {
            SetUnarmed(Cop);
        }

        if (PoliceScanning.CopPeds.Any(x => x.DistanceToPlayer <= 8f) && Game.LocalPlayer.Character.Speed <= 4.0f && !Game.LocalPlayer.Character.IsInAnyVehicle(false) && !InstantAction.isBusted && !PlayerWasJustJacking)
            ForceSurrenderTime++;
        else
            ForceSurrenderTime = 0;

        if (ForceSurrenderTime >= 500)
            SurrenderBust = true;

        if (SurrenderBust && !isBustTimeOut())
            SurrenderBustEvent();

        StopSearchMode();
    }
    private static void PoliceTickDeadlyChase()
    {
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => !x.isInVehicle))
        {
            SetCopDeadly(Cop);
            if (!InstantAction.areHandsUp && !InstantAction.BeingArrested && !Cop.TaskIsQueued && Cop.isTasked)
            {
                Cop.TaskIsQueued = true;
                Tasking.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Untask));
            }
        }
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => !x.isTasked && x.isInHelicopter))
        {
            if (!InstantAction.areHandsUp && Game.LocalPlayer.WantedLevel >= 4)
                SetCopDeadly(Cop);
            else
                SetUnarmed(Cop);
        }
        if (Settings.IssuePoliceHeavyWeapons)
        {
            foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.isInVehicle && x.IssuedHeavyWeapon == null))
            {
                IssueCopHeavyWeapon(Cop);
                break;
            }
        }

        if (CopsKilledByPlayer >= Settings.PoliceKilledSurrenderLimit && InstantAction.PlayerWantedLevel < 4)
        {
            SetWantedLevel(4);
            DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportWeaponsFree, 2, false));
        }

        if (SurrenderBust && !isBustTimeOut())
            SurrenderBustEvent();
    }
    private static void PoliceTickSearchMode()
    {
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.DistanceToLastSeen <= 200f || x.DistanceToPlayer <= 150f))//.Where(x => !x.isTasked))
        {
            if (Cop.isInVehicle)
            {
                SetUnarmed(Cop);
            }
            if (!Cop.AtWantedCenterDuringSearchMode && !Cop.TaskIsQueued && Cop.TaskType != PoliceTask.Task.GoToWantedCenter && Cop.DistanceToLastSeen >= 35f && Cop.IsDriver)//((InVehicle && Cop.CopPed.CurrentVehicle.Driver == Cop.CopPed) || !InVehicle))
            {
                Cop.TaskIsQueued = true;
                Tasking.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.GoToWantedCenter));
            }
            else if (!Cop.TaskIsQueued && Cop.TaskType != PoliceTask.Task.SimpleInvestigate && Cop.DistanceToLastSeen < 35f)
            {
                Cop.AtWantedCenterDuringSearchMode = true;
                Cop.TaskIsQueued = true;
                Tasking.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.SimpleInvestigate));

            }
        }

        if (CanReportLastSeen && Game.GameTime - GameTimeLastGreyedOut > 10000 && AnyPoliceSeenPlayerThisWanted && PlayerHasBeenWantedFor > 45000)
        {
            DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspectLastSeen, 10, false));
            CanReportLastSeen = false;
        }
    }
    private static void PoliceVehicleTick()
    {
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.isInVehicle && !x.isInHelicopter))
        {
            if (CurrentPoliceState == PoliceState.DeadlyChase)
            {
                NativeFunction.CallByName<bool>("SET_DRIVER_ABILITY", Cop.CopPed, 100f);
                NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 4, true);
                NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 8, true);
                NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 16, true);
                NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 512, true);
                NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 262144, true);
                NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", Cop.CopPed, 8f);
            }
            else
            {
                NativeFunction.CallByName<bool>("SET_DRIVER_ABILITY", Cop.CopPed, 100f);
                NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", Cop.CopPed, 8f);
                NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 32, true);
            }


            if (InstantAction.PlayerIsOffroad && Cop.DistanceToPlayer <= 200f)
            {
                NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 4194304, true);
            }
            else
            {
                NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 4194304, false);
            }

        }
    }
    private static void CheckPoliceEvents()
    {
        if (PrevPlayerIsJacking != PlayerIsJacking)
            PlayerJackingChanged(PlayerIsJacking);



        if (PrevPlayerKilledPolice != PlayerKilledPolice)
            PlayerKilledPoliceChanged();

        if (PrevCopsKilledByPlayer != CopsKilledByPlayer)
            CopsKilledChanged();

        if (PrevCiviliansKilledByPlayer != CiviliansKilledByPlayer)
            CiviliansKilledChanged();

        if (PrevfiredWeapon != firedWeapon)
            FiredWeaponChanged();

        if (PrevaimedAtPolice != aimedAtPolice)
            aimedAtPoliceChanged();

        if (PrevPlayerHurtPolice != PlayerHurtPolice)
            PlayerHurtPoliceChanged();

        if (PrevPoliceState != CurrentPoliceState)
            PoliceStateChanged();

        if (PrevWantedLevel != Game.LocalPlayer.WantedLevel)
            WantedLevelChanged();

        if (PrevPlayerStarsGreyedOut != PlayerStarsGreyedOut)
            PlayerStarsGreyedOutChanged();

        if (PrevPoliceState != CurrentPoliceState)
            PoliceStateChanged();
    }
    private static void TrackedVehiclesTick()
    {
        InstantAction.TrackedVehicles.RemoveAll(x => !x.VehicleEnt.Exists());
        if (InstantAction.PlayerWantedLevel == 0)
        {
            foreach (GTAVehicle StolenCar in InstantAction.TrackedVehicles.Where(x => x.ShouldReportStolen))
            {
                StolenCar.QuedeReportedStolen = true;
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportStolenVehicle, 10, false, true, StolenCar));
            }
        }
        if (InstantAction.PlayerInVehicle)
        {
            Vehicle CurrVehicle = Game.LocalPlayer.Character.CurrentVehicle;
            GTAVehicle CurrTrackedVehicle = InstantAction.TrackedVehicles.Where(x => x.VehicleEnt.Handle == CurrVehicle.Handle).FirstOrDefault();
            if (AnyPoliceCanRecognizePlayer)
            {
                if (InstantAction.PlayerWantedLevel > 0 && !PlayerStarsGreyedOut)
                    UpdateVehicleDescription(CurrTrackedVehicle);

                if (InstantAction.PlayerWantedLevel < 2 && CurrTrackedVehicle.WasReportedStolen && CurrTrackedVehicle.IsStolen && CurrTrackedVehicle.MatchesOriginalDescription)
                {
                    SetWantedLevel(2);
                    DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSpottedStolenCar, 10, false, true, CurrTrackedVehicle));
                    InstantAction.WriteToLog("TrackedVehiclesTick", "First");
                }
                else if (InstantAction.PlayerWantedLevel < 2 && CurrTrackedVehicle.CarPlate.IsWanted && !CurrTrackedVehicle.IsStolen && CurrTrackedVehicle.ColorMatchesDescription)
                {
                    SetWantedLevel(2);
                    DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspiciousVehicle, 10, false, true, CurrTrackedVehicle));
                    InstantAction.WriteToLog("TrackedVehiclesTick", "Second");
                }
            }

            if (CurrTrackedVehicle.CarPlate != null && CurrTrackedVehicle.CarPlate.IsWanted && InstantAction.PlayerWantedLevel == 0 && PlayerHasBeenNotWantedFor >= 60000 && !CurrTrackedVehicle.IsStolen) //No longer care about the car
            {
                CurrTrackedVehicle.CarPlate.IsWanted = false;
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspectLost, 10, false, true));
                Menus.UpdateLists();
            }
        }
    }
    public static void UpdateVehicleDescription(GTAVehicle MyVehicle)
    {
        MyVehicle.DescriptionColor = MyVehicle.VehicleEnt.PrimaryColor;
        MyVehicle.CarPlate.IsWanted = true;
        if (MyVehicle.IsStolen && !MyVehicle.WasReportedStolen)
            MyVehicle.WasReportedStolen = true;
    }
    private static void WantedLevelTick()
    {
        if (InstantAction.PlayerWantedLevel > 0)
        {
            Vector3 CurrentWantedCenter = NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
            if (CurrentWantedCenter != Vector3.Zero)
            {
                LastWantedCenterPosition = CurrentWantedCenter;
                AddUpdateCurrentWantedBlip(CurrentWantedCenter);
            }

            if (Settings.WantedLevelIncreasesOverTime && Game.GameTime - WantedLevelStartTime > Settings.WantedLevelIncreaseTime && WantedLevelStartTime > 0 && AnyPoliceRecentlySeenPlayer && InstantAction.PlayerWantedLevel > 0 && InstantAction.PlayerWantedLevel <= Settings.WantedLevelInceaseOverTimeLimit)
            {
                Game.LocalPlayer.WantedLevel++;
                InstantAction.WriteToLog("WantedLevelStartTime", "Wanted Level Increased Over Time");
            }

            //if (Settings.SpawnNewsChopper && Game.GameTime - WantedLevelStartTime > 180000 && WantedLevelStartTime > 0 && AnyPoliceRecentlySeenPlayer && InstantAction.PlayerWantedLevel > 4 && !PoliceScanning.Reporters.Any())
            //{
            //    PoliceSpawning.SpawnNewsChopper();
            //    InstantAction.WriteToLog("WantedLevelTick", "Been at this wanted for a while, wanted news chopper spawned (if they dont already exist)");
            //}


            //if (AnyPoliceRecentlySeenPlayer && !PlayerStarsGreyedOut)
            //{
            //    PoliceScanning.PlacePlayerLastSeen = Game.LocalPlayer.Character.Position;
            //    NativeFunction.CallByName<bool>("SET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer, Game.LocalPlayer.Character.Position.X, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z);
            //}

            //if (AnyPoliceRecentlySeenPlayer && !PlayerStarsGreyedOut)
            //{
            //    PoliceScanning.PlacePlayerLastSeen = Game.LocalPlayer.Character.Position;
            //    NativeFunction.CallByName<bool>("SET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer, Game.LocalPlayer.Character.Position.X, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z);
            //}
        }
    }
    private static bool isBustTimeOut()
    {
        if (Game.GameTime - LastBust >= 10000)
            return false;
        else
            return true;
    }
    public static void SetUnarmed(GTACop Cop)
    {
        if (!Cop.CopPed.Exists() || (Cop.SetUnarmed && !Cop.NeedsWeaponCheck))
            return;
        if (Settings.OverridePoliceAccuracy)
            Cop.CopPed.Accuracy = Settings.PoliceGeneralAccuracy;
        NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Cop.CopPed, 0);
        if (!(Cop.CopPed.Inventory.EquippedWeapon == null))
        {
            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Cop.CopPed, (uint)2725352035, true); //Unequip weapon so you don't get shot
            NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Cop.CopPed, false);
        }
        Cop.SetTazer = false;
        Cop.SetUnarmed = true;
        Cop.SetDeadly = false;
        Cop.GameTimeLastWeaponCheck = Game.GameTime;
    }
    public static void ResetCopWeapons(GTACop Cop)
    {
        if (!Cop.CopPed.Exists() || (!Cop.SetDeadly && !Cop.SetTazer && !Cop.SetUnarmed && !Cop.NeedsWeaponCheck))
            return;
        if (Settings.OverridePoliceAccuracy)
            Cop.CopPed.Accuracy = Settings.PoliceGeneralAccuracy;
        NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Cop.CopPed, 30);
        if (!Cop.CopPed.Inventory.Weapons.Contains(Cop.IssuedPistol.Name))
            Cop.CopPed.Inventory.GiveNewWeapon(Cop.IssuedPistol.Name, -1, false);
        NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Cop.CopPed, true);
        Cop.SetTazer = false;
        Cop.SetUnarmed = false;
        Cop.SetDeadly = false;
        Cop.GameTimeLastWeaponCheck = Game.GameTime;
    }
    public static void SetCopDeadly(GTACop Cop)
    {
        if (!Cop.CopPed.Exists() || (Cop.SetDeadly && !Cop.NeedsWeaponCheck))
            return;
        if (Settings.OverridePoliceAccuracy)
            Cop.CopPed.Accuracy = Settings.PoliceGeneralAccuracy;
        NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Cop.CopPed, 30);
        if (!Cop.CopPed.Inventory.Weapons.Contains(Cop.IssuedPistol.Name))
            Cop.CopPed.Inventory.GiveNewWeapon(Cop.IssuedPistol.Name, -1, true);

        if ((Cop.CopPed.Inventory.EquippedWeapon == null || Cop.CopPed.Inventory.EquippedWeapon.Hash == WeaponHash.StunGun) && Game.LocalPlayer.WantedLevel >= 0)
            Cop.CopPed.Inventory.GiveNewWeapon(Cop.IssuedPistol.Name, -1, true);

        if (Settings.AllowPoliceWeaponVariations)
            InstantAction.ApplyWeaponVariation(Cop.CopPed, (uint)Cop.IssuedPistol.Hash, Cop.PistolVariation);
        NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Cop.CopPed, true);
        Cop.SetTazer = false;
        Cop.SetUnarmed = false;
        Cop.SetDeadly = true;
        Cop.GameTimeLastWeaponCheck = Game.GameTime;
    }
    public static void SetCopTazer(GTACop Cop)
    {
        if (!Cop.CopPed.Exists() || (Cop.SetTazer && !Cop.NeedsWeaponCheck))
            return;

        if (Settings.OverridePoliceAccuracy)
            Cop.CopPed.Accuracy = Settings.PoliceTazerAccuracy;
        NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Cop.CopPed, 100);
        if (!Cop.CopPed.Inventory.Weapons.Contains(WeaponHash.StunGun))
        {
            Cop.CopPed.Inventory.GiveNewWeapon(WeaponHash.StunGun, 100, true);
        }
        else if (Cop.CopPed.Inventory.EquippedWeapon != WeaponHash.StunGun)
        {
            Cop.CopPed.Inventory.EquippedWeapon = WeaponHash.StunGun;
        }
        NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Cop.CopPed, false);
        Cop.SetTazer = true;
        Cop.SetUnarmed = false;
        Cop.SetDeadly = false;
        Cop.GameTimeLastWeaponCheck = Game.GameTime;
    }
    private static void StopSearchMode()
    {
        if (InstantAction.PlayerInVehicle)
            return;
        if (!GhostCop.Exists())
        {
            CreateGhostCop();
        }
        if (GhostCopFollow && GhostCop != null)
        {
            Vector3 DesiredPosition = Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0f, 4f, 1f));
            Vector3 PlacedPosition = Vector3.Zero;
            bool FoundPlace = false;
            unsafe
            {
                FoundPlace = NativeFunction.CallByName<bool>("GET_SAFE_COORD_FOR_PED", DesiredPosition.X, DesiredPosition.Y, DesiredPosition.Z, false, &PlacedPosition, 16);
            }

            if (FoundPlace)
                GhostCop.Position = PlacedPosition;
            else
                GhostCop.Position = DesiredPosition;

            Vector3 Resultant = Vector3.Subtract(Game.LocalPlayer.Character.Position, GhostCop.Position);
            GhostCop.Heading = NativeFunction.CallByName<float>("GET_HEADING_FROM_VECTOR_2D", Resultant.X, Resultant.Y);
        }
        if (AnyPoliceRecentlySeenPlayer)// Needed for the AI to keep the player in the wanted position
        {
            GhostCopFollow = true;
        }
        else
        {
            if (GhostCop != null)
                GhostCop.Position = new Vector3(0f, 0f, 0f);
            GhostCopFollow = false;
        }
    }
    private static void CreateGhostCop()
    {
        GhostCop = new Ped(CopModel, Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0f, 4f, 0f)), Game.LocalPlayer.Character.Heading);
        GhostCop.BlockPermanentEvents = false;
        GhostCop.IsPersistent = true;
        GhostCop.IsCollisionEnabled = false;
        GhostCop.IsVisible = false;
        Blip myBlip = GhostCop.GetAttachedBlip();
        if (myBlip != null)
            myBlip.Delete();
        GhostCop.VisionRange = 100f;
        GhostCop.HearingRange = 100f;
        GhostCop.CanRagdoll = false;
        const ulong SetPedMute = 0x7A73D05A607734C7;
        NativeFunction.CallByHash<uint>(SetPedMute, GhostCop);
        NativeFunction.CallByName<bool>("STOP_PED_SPEAKING", GhostCop, true);
        NativeFunction.CallByName<uint>("SET_PED_CONFIG_FLAG", GhostCop, 69, true);
        NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", GhostCop, (uint)2725352035, true); //Unequip weapon so you don't get shot
        NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", GhostCop, false);
        NativeFunction.CallByName<uint>("SET_PED_MOVE_RATE_OVERRIDE", GhostCop, 0f);
        GhostCopFollow = true;
    }
    public static void ResetPoliceStats()
    {
        CopsKilledByPlayer = 0;
        CiviliansKilledByPlayer = 0;
        PlayerHurtPolice = false;
        PlayerKilledPolice = false;
        PlayerKilledCivilians = false;
        foreach (GTACop Cop in PoliceScanning.CopPeds)
        {
            Cop.HurtByPlayer = false;
        }
        aimedAtPolice = false;
        firedWeapon = false;
        DispatchAudio.ResetReportedItems();
    }
    public static void SetWantedLevel(int WantedLevel)
    {
        GameTimeLastSetWanted = Game.GameTime;
        if (Game.LocalPlayer.WantedLevel != WantedLevel)
        {
            Game.LocalPlayer.WantedLevel = WantedLevel;
            InstantAction.WriteToLog("SetWantedLevel", string.Format("Manually set to: {0}", WantedLevel));
        }
    }
    public static void CheckRecognition()
    {
        AnyPoliceCanSeePlayer = PoliceScanning.CopPeds.Any(x => x.canSeePlayer);
        AnyPoliceRecentlySeenPlayer = PoliceScanning.CopPeds.Any(x => x.SeenPlayerSince(Settings.PoliceRecentlySeenTime));

        uint TimeToRecongize = 2000;
        CheckNight();

        if (IsNightTime)
            TimeToRecongize = 3500;
        else if (InstantAction.PlayerInVehicle)
            TimeToRecongize = 750;
        else
            TimeToRecongize = 2000;
        AnyPoliceCanRecognizePlayer = PoliceScanning.CopPeds.Any(x => x.HasSeenPlayerFor >= TimeToRecongize || (x.canSeePlayer && x.DistanceToPlayer <= 20f) || (x.DistanceToPlayer <= 7f && x.DistanceToPlayer > 0f));

        if (InstantAction.PlayerWantedLevel == 0)
        {
            if (InstantAction.PlayerInVehicle)
                TimeToRecongize = 6000;
            else
                TimeToRecongize = 4000;

            if (IsNightTime)
                TimeToRecongize = TimeToRecongize + 6000;

            AnyPoliceCanRecognizePlayerAfterWanted = PoliceScanning.CopPeds.Any(x => x.HasSeenPlayerFor >= TimeToRecongize || (x.canSeePlayer && x.DistanceToPlayer <= 12f) || (x.DistanceToPlayer <= 7f && x.DistanceToPlayer > 0f));
        }

        if (PrevAnyCanRecognizePlayer != AnyPoliceCanRecognizePlayer)
        {
            PrevAnyCanRecognizePlayer = AnyPoliceCanRecognizePlayer;
        }

        if (!AnyPoliceSeenPlayerThisWanted)
        {
            PlacePlayerLastSeen = PlaceWantedStarted;
        }
        else if (AnyPoliceRecentlySeenPlayer && !PlayerStarsGreyedOut)
        {
            PlacePlayerLastSeen = Game.LocalPlayer.Character.Position;
        }
        NativeFunction.CallByName<bool>("SET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer, PlacePlayerLastSeen.X, PlacePlayerLastSeen.Y, PlacePlayerLastSeen.Z);
    }
    private static void CheckNight()
    {
        IsNightTime = false;
        int HourOfDay = NativeFunction.CallByName<int>("GET_CLOCK_HOURS");

        if (HourOfDay >= 20 || HourOfDay <= 5)
            IsNightTime = true;
    }
    public static void ResetPersonOfInterest()
    {
        PlayerIsPersonOfInterest = false;
        LastWantedCenterPosition = Vector3.Zero;
        if (LastWantedCenterBlip.Exists())
            LastWantedCenterBlip.Delete();
    }
    //Events
    private static void WantedLevelChanged()
    {
        if (Game.LocalPlayer.WantedLevel == 0)//Just Removed
        {
            if (AnyPoliceSeenPlayerThisWanted && PrevWantedLevel != 0)//maxwantedlastlife
            {
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspectLost, 5, false));
                AddUpdateLastWantedBlip(LastWantedCenterPosition);
            }
            CurrentPoliceState = PoliceState.Normal;
            AnyPoliceSeenPlayerThisWanted = false;
            TrafficViolations.ResetTrafficViolations();
            WantedLevelStartTime = 0;
            GameTimeWantedStarted = 0;
            DispatchAudio.ResetReportedItems();
            GameTimeLastWantedEnded = Game.GameTime;

            if (CurrentWantedCenterBlip.Exists())
                CurrentWantedCenterBlip.Delete();

            Tasking.UntaskAll(false);
            Tasking.RetaskAllRandomSpawns();
        }
        else
        {
            GameTimeLastWantedEnded = 0;
        }
        if (PrevWantedLevel == 0 && Game.LocalPlayer.WantedLevel > 0)
        {
            if (Game.LocalPlayer.WantedLevel == 1 && !RecentlySetWanted && !AnyPoliceRecentlySeenPlayer && !World.GetEntities(Game.LocalPlayer.Character.Position, 25f, GetEntitiesFlags.ConsiderHumanPeds | GetEntitiesFlags.ExcludePlayerPed).Any(x => x.IsAlive))
            {
                InstantAction.WriteToLog("ValueChecker", String.Format("FAKE WantedLevel Changed to: {0}, changing it back", Game.LocalPlayer.WantedLevel));
                SetWantedLevel(0);
                if (LastWantedCenterBlip.Exists())
                    LastWantedCenterBlip.Delete();
                return;
            }

            GameTimeWantedStarted = Game.GameTime;
            PlaceWantedStarted = Game.LocalPlayer.Character.Position;

            if (LastWantedCenterBlip.Exists())
                LastWantedCenterBlip.Delete();

            Tasking.UntaskAllRandomSpawns(false);
            PlayerIsPersonOfInterest = true;

            Game.LocalPlayer.Character.PlayAmbientSpeech("GENERIC_CURSE");
            if (InstantAction.PlayerInVehicle)
                VehicleEngine.WantedLevelTune = true;
        }
        WantedLevelStartTime = Game.GameTime;
        InstantAction.WriteToLog("ValueChecker", String.Format("WantedLevel Changed to: {0}", Game.LocalPlayer.WantedLevel));
        PrevWantedLevel = Game.LocalPlayer.WantedLevel;
    }
    private static void CopsKilledChanged()
    {
        InstantAction.WriteToLog("ValueChecker", string.Format("CopsKilledByPlayer Changed to: {0}", CopsKilledByPlayer));
        PrevCopsKilledByPlayer = CopsKilledByPlayer;
    }
    private static void CiviliansKilledChanged()
    {
        InstantAction.WriteToLog("ValueChecker", String.Format("CiviliansKilledChanged Changed to: {0}", CiviliansKilledByPlayer));
        PrevCiviliansKilledByPlayer = CiviliansKilledByPlayer;
    }
    private static void PlayerHurtPoliceChanged()
    {
        InstantAction.WriteToLog("ValueChecker", String.Format("PlayerHurtPolice Changed to: {0}", PlayerHurtPolice));
        if (PlayerHurtPolice)
        {
            //DispatchAudioSystem.ReportAssualtOnOfficer();
            DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportAssualtOnOfficer, 3, true));
        }

        PrevPlayerHurtPolice = PlayerHurtPolice;
    }
    private static void PlayerKilledPoliceChanged()
    {
        InstantAction.WriteToLog("ValueChecker", String.Format("PlayerKilledPolice Changed to: {0}", PlayerKilledPolice));
        if (PlayerKilledPolice)
        {
            DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportOfficerDown, 1, true));
        }
        PrevPlayerKilledPolice = PlayerKilledPolice;
    }
    private static void FiredWeaponChanged()
    {
        InstantAction.WriteToLog("ValueChecker", String.Format("firedWeapon Changed to: {0}", firedWeapon));
        if (firedWeapon)
        {
            //DispatchAudioSystem.ReportShotsFired();
            DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportShotsFired, 2, true));
        }
        PrevfiredWeapon = firedWeapon;
    }
    private static void SurrenderBustEvent()
    {
        //return;
        //if (!NativeFunction.Natives.x36AD3E690DA5ACEB<bool>("PeyoteIn"))//_GET_SCREEN_EFFECT_IS_ACTIVE
        //    NativeFunction.Natives.x068E835A1D0DC0E3("PeyoteIn", 0, 0);//_STOP_SCREEN_EFFECT
        InstantAction.BeingArrested = true;
        CurrentPoliceState = PoliceState.ArrestedWait;
        NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)2725352035, true);
        InstantAction.areHandsUp = false;
        SurrenderBust = false;
        LastBust = Game.GameTime;
        InstantAction.WriteToLog("SurrenderBust", "SurrenderBust Executed");
    }
    private static void aimedAtPoliceChanged()
    {
        if (aimedAtPolice)
        {
            DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportThreateningWithFirearm, 2, true));
        }
        PrevaimedAtPolice = aimedAtPolice;
    }
    private static void PlayerStarsGreyedOutChanged()
    {
        InstantAction.WriteToLog("ValueChecker", String.Format("PlayerStarsGreyedOut Changed to: {0}", PlayerStarsGreyedOut));
        if (PlayerStarsGreyedOut)
        {
            CanReportLastSeen = true;
            GameTimeLastGreyedOut = Game.GameTime;
        }
        else
        {
            foreach (GTACop Cop in PoliceScanning.CopPeds)
            {
                Cop.AtWantedCenterDuringSearchMode = false;
                if (Cop.isTasked && (Cop.TaskType == PoliceTask.Task.GoToWantedCenter || Cop.TaskType == PoliceTask.Task.SimpleInvestigate || Cop.TaskType == PoliceTask.Task.RandomSpawnIdle))
                {
                    Tasking.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Untask));
                }
            }
            //PoliceScanning.CopPeds.ForEach(x => x.AtWantedCenterDuringSearchMode = false);
            //Tasking.UntaskAll(true);
            CanReportLastSeen = false;
        }
        PrevPlayerStarsGreyedOut = PlayerStarsGreyedOut;
    }
    private static void PoliceStateChanged()
    {
        InstantAction.WriteToLog("ValueChecker", String.Format("PoliceState Changed to: {0}", CurrentPoliceState));
        InstantAction.WriteToLog("ValueChecker", String.Format("PreviousPoliceState Changed to: {0}", PrevPoliceState));
        LastPoliceState = PrevPoliceState;
        if (CurrentPoliceState == PoliceState.Normal && !InstantAction.isDead)
        {
            ResetPoliceStats();
            PoliceSpawning.DeleteNewsTeam();
            //PoliceScanning.RetaskAllRandomSpawns();
        }

        if (CurrentPoliceState == PoliceState.DeadlyChase)
        {
            if (PrevPoliceState != PoliceState.ArrestedWait)
            {
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportLethalForceAuthorized, 1, true));
            }
        }
        GameTimePoliceStateStart = Game.GameTime;
        PrevPoliceState = CurrentPoliceState;
    }
    private static void PlayerJackingChanged(bool isJacking)
    {
        PlayerIsJacking = isJacking;
        InstantAction.WriteToLog("ValueChecker", String.Format("PlayerIsJacking Changed to: {0}", PlayerIsJacking));
        if (PlayerIsJacking)
        {
            GameTimeLastStartedJacking = Game.GameTime;
        }
        PrevPlayerIsJacking = PlayerIsJacking;
    }


    private static void AddUpdateLastWantedBlip(Vector3 Position)
    {
        if (Position == Vector3.Zero)
        {
            if (LastWantedCenterBlip.Exists())
                LastWantedCenterBlip.Delete();
            return;
        }
        if (!LastWantedCenterBlip.Exists())
        {
            LastWantedCenterBlip = new Blip(LastWantedCenterPosition, 250f);
            LastWantedCenterBlip.Name = "Last Wanted Center Position";
            LastWantedCenterBlip.Color = Color.Yellow;
            LastWantedCenterBlip.Alpha = 0.5f;

            NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)LastWantedCenterBlip.Handle, true);
            CreatedBlips.Add(LastWantedCenterBlip);
        }
        if (LastWantedCenterBlip.Exists())
            LastWantedCenterBlip.Position = Position;
    }
    private static void AddUpdateCurrentWantedBlip(Vector3 Position)
    {
        if (Position == Vector3.Zero)
        {
            if (CurrentWantedCenterBlip.Exists())
                CurrentWantedCenterBlip.Delete();
            return;
        }
        if (!CurrentWantedCenterBlip.Exists())
        {
            CurrentWantedCenterBlip = new Blip(Position, 100f);
            CurrentWantedCenterBlip.Name = "Current Wanted Center Position";
            CurrentWantedCenterBlip.Color = Color.Red;
            CurrentWantedCenterBlip.Alpha = 0.5f;

            NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)CurrentWantedCenterBlip.Handle, true);
            CreatedBlips.Add(CurrentWantedCenterBlip);
        }
        if (CurrentWantedCenterBlip.Exists())
            CurrentWantedCenterBlip.Position = Position;
    }
}

