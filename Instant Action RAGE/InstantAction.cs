using ExtensionsMethods;
using Instant_Action_RAGE.Systems;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public static class InstantAction
{
    public static bool isDead = false;
    public static bool isBusted = false;
    private static bool BeingArrested = false;
    private static bool DiedInVehicle = false;
    private static int MaxWantedLastLife;
    private static int TimesDied;
    private static int PreviousWantedLevel;
    private static Random rnd;
    private static String LastModelHash;
    private static PedVariation myPedVariation;
    private static Vector3 PositionOfDeath;
    private static PoliceState HandsUpPreviousPoliceState;
    public static bool areHandsUp = false;
    private static bool firedWeapon = false;
    private static bool aimedAtPolice = false;
    private static PoliceState PrevPoliceState = PoliceState.Normal;
    private static bool PrevfiredWeapon = false;
    private static bool PrevPlayerHurtPolice = false;
    private static int PrevWantedLevel = 0;
    public static bool SurrenderBust = false;
    private static uint LastBust;
   // private static uint GameTimeLastReTasked = 0;
    private static int ForceSurrenderTime;
    private static Model CopModel = new Model("s_m_y_cop_01");
    private static List<EmergencyLocation> EmergencyLocations = new List<EmergencyLocation>();
    public static Ped GhostCop;

    private static uint WantedLevelStartTime;
    private static bool AnySeenThisWanted = false;
    private static int TimeAimedAtPolice = 0;

    //traffic

    private static bool CanReportLastSeen;
    private static uint GameTimeLastGreyedOut;
    private static bool PrevPlayerStarsGreyedOut;
    private static bool PlayerStarsGreyedOut;
    private static bool PrevaimedAtPolice;
    private static bool GhostCopFollow;
    private static bool PrevPlayerKilledPolice = false;
    private static int PrevCopsKilledByPlayer = 0;

    private static bool IsRunning { get; set; } = true;
    public static PoliceState CurrentPoliceState { get; set; }
    enum DRIVING_FLAGS
    {
        FLAG1_STOP_VEHS = 1,
        FLAG2_STOP_PEDS = 2,
        FLAG3_AVOID_VEHS = 4,
        FLAG5_AVOID_PEDS = 16,
        FLAG6_AVOID_OBJS = 32,
        FLAG8_STOP_LIGHTS = 128,
        FLAG11_REVERSE = 1024,
        FLAG19_SHORTEST_PATH = 262144,
        FLAG23_IGN_ROADS = 4194304,
    };
    public enum PoliceState
    {
        Normal = 0,
        UnarmedChase = 1,
        CautiousChase = 2,
        DeadlyChase = 3,
        ArrestedWait = 4,
    }
    static InstantAction()
    {
        rnd = new Random();
    }
    public static void Initialize()
    {
        CopModel.LoadAndWait();
        CopModel.LoadCollisionAndWait();

        Game.LocalPlayer.Character.CanBePulledOutOfVehicles = true;
        setupLocations();
        MainLoop();
    }

    public static void MainLoop()
    {

        GameFiber.StartNew(delegate
        {
            Settings.Initialize();
            Menus.Intitialize();
            RespawnSystem.Initialize();
            PoliceScanningSystem.Initialize();
            DispatchAudioSystem.Initialize();
            CustomOptions.Initialize();
            while (IsRunning)
            {
                StateTick();
                ControlTick();
                PoliceTick();
                DebugLoop();
                TestLoop();
                GameFiber.Yield();
            }

        });

    }
    private static void TestLoop()
    {
        //Other Fun STUFF!

        //Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE, Squad1Leader, GetLastVehicle(Squad1Leader), pos.X, pos.Y, pos.Z, 70f, FLAG3_AVOID_VEHS | FLAG5_AVOID_PEDS | FLAG6_AVOID_OBJS | FLAG19_SHORTEST_PATH, 10f);

        //NativeFunction.Natives.xB9EFD5C25018725A("WantedMusicDisabled", true);
        //NativeFunction.CallByName<bool>("SET_AUDIO_FLAG", "WantedMusicDisabled", true);

        NativeFunction.Natives.xB9EFD5C25018725A("WantedMusicDisabled", true);
        //NativeFunction.CallByName<bool>("SET_PED_MOVE_RATE_OVERRIDE", GhostCop, 0f);

        //if(Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle != null)
        //{
        //    Game.LocalPlayer.Character.CurrentVehicle.RadioStation = RadioStation.SelfRadio;
        //}


        //if (Game.LocalPlayer.Character.IsInAnyVehicle(false))
        //{
        //    if (NativeFunction.Natives.GetPlayerRadioStationIndex<int>() == 17 && Game.LocalPlayer.Character.CurrentVehicle.IsEngineOn)
        //    {
        //        // var status = CommunicationService.GetStatus();

        //        // Disable radio in car.
        //        NativeFunction.Natives.SetFrontendRadioActive(false);
        //        NativeFunction.Natives.SetVehicleRadioLoud(Game.LocalPlayer.Character.CurrentVehicle, false);
        //        NativeFunction.Natives.SetVehicleRadioEnabled(Game.LocalPlayer.Character.CurrentVehicle, false);

        //        //PushSpotifyInfo(status.Track.ArtistResource.Name, status.Track.TrackResource.Name, status.Playing);

        //        //if (DebugDraw)
        //        // DashboardScaleform.Render2D();
        //    }
        //    else
        //    {
        //        NativeFunction.CallByName<bool>("SET_VEH_RADIO_STATION", Game.LocalPlayer.Character.CurrentVehicle, "RADIO_19_USER");
        //    }

        //    // Check if vehicle radio is disabled or if it is set to be loud.
        //    if (!NativeFunction.Natives.x5F43D83FD6738741<bool>() || !NativeFunction.Natives.x032A116663A4D5AC<bool>(Game.LocalPlayer.Character.CurrentVehicle))
        //    {
        //        NativeFunction.Natives.SetFrontendRadioActive(true);
        //        NativeFunction.Natives.SetVehicleRadioLoud(Game.LocalPlayer.Character.CurrentVehicle, true);
        //        NativeFunction.Natives.SetVehicleRadioEnabled(Game.LocalPlayer.Character.CurrentVehicle, true);
        //    }
        //}
    }
    private static void DebugLoop()
    {
        if (Game.IsKeyDown(Keys.NumPad0))
        {
            Game.LocalPlayer.Character.IsInvincible = false;
        }
        if (Game.IsKeyDown(Keys.NumPad1))
        {

            Game.LocalPlayer.Character.IsInvincible = true;
            Game.LocalPlayer.Character.Health = 100;
            WriteToLog("KeyDown", "You are invicible");
        }
        if (Game.IsKeyDown(Keys.NumPad3))
        {

            CurrentPoliceState = PoliceState.Normal;
            Game.LocalPlayer.WantedLevel = 0;
            PoliceScanningSystem.UntaskAll();


            foreach (GTACop Cop in PoliceScanningSystem.K9Peds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.CopPed.IsInHelicopter))
            {
                Cop.CopPed.Delete();
            }
            foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.CopPed.IsInAnyVehicle(false) && !x.CopPed.IsInHelicopter))
            {
                Cop.CopPed.Delete();
            }

            Ped[] closestPed = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 400f, GetEntitiesFlags.ExcludePlayerPed | GetEntitiesFlags.ConsiderAnimalPeds).Where(x => x is Ped).ToArray(), (x => (Ped)x));
            foreach (Ped dog in closestPed)
            {
                dog.Delete();
            }

            Game.TimeScale = 1f;
            isBusted = false;
            BeingArrested = false;
            NativeFunction.Natives.xB4EDDC19532BFB85();

        }
        //if (Game.IsKeyDown(Keys.NumPad4))
        //{
        //    //GhostCop.Position = Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0f, -4f, 0f));
        //    //GhostCop.Heading = Game.LocalPlayer.Character.Heading;
        //    WriteToLog("KeyDown", "==========");
        //    foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => x.CopPed.Exists()))
        //    {
        //        string TaskFiberName = "";
        //        if (Cop.TaskFiber != null)
        //            TaskFiberName = Cop.TaskFiber.Name;

        //        WeaponHash Weapon = 0;
        //        if (Cop.CopPed.Inventory.EquippedWeapon != null)
        //            Weapon = Cop.CopPed.Inventory.EquippedWeapon.Hash;


        //        WriteToLog("KeyDown", "______________________");
        //        WriteToLog("KeyDown", string.Format("Handle: {0}", Cop.CopPed.Handle));
        //        WriteToLog("KeyDown", string.Format("Can See Player: {0}", Cop.canSeePlayer));
        //        WriteToLog("KeyDown", string.Format("Recently Seen Player: {0}", Cop.canSeePlayer));
        //        //WriteToLog("KeyDown", string.Format("Set Deadly: {0}", Cop.SetDeadly));
        //        //WriteToLog("KeyDown", string.Format("Set Unarmed: {0}", Cop.SetUnarmed));
        //        //WriteToLog("KeyDown", string.Format("Set Tazer: {0}", Cop.SetTazer));
        //        WriteToLog("KeyDown", string.Format("Current Weapon: {0}", Weapon));
        //        //WriteToLog("KeyDown", string.Format("TaskFiberName: {0}", TaskFiberName));
        //        //WriteToLog("KeyDown", string.Format("SimpleTaskName: {0}", Cop.SimpleTaskName));
        //        WriteToLog("KeyDown", string.Format("isTasked: {0}", Cop.isTasked));
        //        if (Cop.CopPed.IsInAnyVehicle(false))
        //            WriteToLog("KeyDown", string.Format("Vehicle: {0}", Cop.CopPed.CurrentVehicle.Model.Name));
        //        WriteToLog("KeyDown", string.Format("Range To: {0}", Cop.CopPed.RangeTo(Game.LocalPlayer.Character.Position)));
        //        //WriteToLog("KeyDown", string.Format("IsInRangeOf 25f: {0}", Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 25f)));
        //        //WriteToLog("KeyDown", string.Format("IsInRangeOf 45f: {0}", Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 45f)));
        //        //WriteToLog("KeyDown", string.Format("IsInRangeOf 75f: {0}", Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 75f)));
        //        //WriteToLog("KeyDown", string.Format("getDotVectorResult: {0}", Extensions.getDotVectorResult(Cop.CopPed, Game.LocalPlayer.Character)));
        //        WriteToLog("KeyDown", string.Format("PlayerIsInFront: {0}", Extensions.PlayerIsInFront(Cop.CopPed)));



        //    }
        //    WriteToLog("KeyDown", "==========");
        //    WriteToLog("KeyDown", string.Format("Total Cops: {0}", PoliceScanningSystem.CopPeds.Where(x => x.CopPed.Exists()).Count()));

        //}
        if (Game.IsKeyDown(Keys.NumPad5))
        {
            try
            {
                WriteToLog("KeyDown", string.Format("ModelName: {0}", Game.LocalPlayer.Character.CurrentVehicle.Model.Name));
                WriteToLog("KeyDown", string.Format("SPeed: {0}", Game.LocalPlayer.Character.CurrentVehicle.Speed));

                //MoveGhostCopToPlayer();
                //Game.LocalPlayer.Character.GiveCash(5000, "Michael");
                //MoveGhostCopToPlayer();
            }
            catch (Exception e)
            {
                WriteToLog("Car stuff", e.Message);
            }
        }
    }

    //Police
    private static void GetPoliceState()
    {
        if (CurrentPoliceState == PoliceState.ArrestedWait || CurrentPoliceState == PoliceState.DeadlyChase)
            return;

        bool AnyCanSeePlayer = PoliceScanningSystem.CopPeds.Any(x => x.canSeePlayer);

        if (Game.LocalPlayer.WantedLevel == 0)
        {
            CurrentPoliceState = PoliceState.Normal;
        }
        else if (Game.LocalPlayer.WantedLevel >= 1 && Game.LocalPlayer.WantedLevel <= 3 && AnyCanSeePlayer)
        {
            if ((!firedWeapon && !PoliceScanningSystem.PlayerHurtPolice && !aimedAtPolice) && !Game.LocalPlayer.Character.isConsideredArmed()) // Unarmed and you havent killed anyone
                CurrentPoliceState = PoliceState.UnarmedChase;
            else if ((!firedWeapon && !PoliceScanningSystem.PlayerHurtPolice && !aimedAtPolice))
                CurrentPoliceState = PoliceState.CautiousChase;
            else
                CurrentPoliceState = PoliceState.DeadlyChase;

        }
        else if (Game.LocalPlayer.WantedLevel >= 4 || PoliceScanningSystem.PlayerHurtPolice || PoliceScanningSystem.PlayerKilledPolice)
        {
            CurrentPoliceState = PoliceState.DeadlyChase;
        }

        if (Game.LocalPlayer.Character.isConsideredArmed() && Game.LocalPlayer.WantedLevel < 2 && !Game.LocalPlayer.Character.IsInAnyVehicle(false))
        {
            if (AnyCanSeePlayer)
            {
                Game.LocalPlayer.WantedLevel = 2;
                DispatchAudioSystem.ReportCarryingWeapon();
            }
        }

        if (!aimedAtPolice && Game.LocalPlayer.Character.isConsideredArmed() && Game.LocalPlayer.IsFreeAiming && AnyCanSeePlayer && PoliceScanningSystem.CopPeds.Any(x => Game.LocalPlayer.IsFreeAimingAtEntity(x.CopPed)))
        {
            if (TimeAimedAtPolice == 0)
                WriteToLog("Started Aiming at Police", "");
            TimeAimedAtPolice++;           
        }
        else
        {
            if (TimeAimedAtPolice != 0)
                WriteToLog("Stopped Aiming at Police", "");
            TimeAimedAtPolice = 0;
            
        }

        if(TimeAimedAtPolice >= 100)
        {
            WriteToLog("AimedAt Police", "");
            Game.LocalPlayer.WantedLevel = 3;
            aimedAtPolice = true;
        }

        if (!firedWeapon && Game.LocalPlayer.Character.IsShooting && (PoliceScanningSystem.CopPeds.Any(x => x.canSeePlayer || x.DistanceToPlayer <= 100f))) //if (!firedWeapon && Game.LocalPlayer.Character.IsShooting && (PoliceScanningSystem.CopPeds.Any(x => x.canSeePlayer || x.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 100f))))
        {
            firedWeapon = true;
        }

    }
    private static void PoliceTick()
    {
        PoliceScanningSystem.UpdatePolice();
        GetPoliceState();

             
        bool PlayerInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
        int RecentlySeenTime = 10000;
        if (PlayerInVehicle)
            RecentlySeenTime = 30000;

        bool AnyRecentlySeen = PoliceScanningSystem.CopPeds.Any(x => x.SeenPlayerSince(RecentlySeenTime));
        PlayerStarsGreyedOut = NativeFunction.CallByName<bool>("ARE_PLAYER_STARS_GREYED_OUT", Game.LocalPlayer);
        if (!AnySeenThisWanted && AnyRecentlySeen)
            AnySeenThisWanted = true;

        //if(Game.LocalPlayer.WantedLevel > 0)
        //{
        //    NativeFunction.CallByName<bool>("DISPLAY_RADAR", false);
        //    NativeFunction.CallByName<bool>("SET_POLICE_RADAR_BLIPS", false); // No Radar or police blips
        //}
        //else
        //{
        //    NativeFunction.CallByName<bool>("DISPLAY_RADAR", true);
        //    NativeFunction.CallByName<bool>("SET_POLICE_RADAR_BLIPS", true);
        //}


        foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => x.isInVehicle && !x.isInHelicopter))
        {
            Cop.CopPed.VisionRange = 100f;


            //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 4, true);
            //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 8, true);
           ///// NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 32, true);
            //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 512, true);
            // NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", Cop.CopPed, 8f);

            if ( CurrentPoliceState == PoliceState.DeadlyChase)
            {
                NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 262144, true);
                //CreatePassengerCop(Cop);
                NativeFunction.CallByName<bool>("SET_DRIVER_AGGRESSIVENESS", Cop.CopPed, 100f);
                //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", Cop.CopPed, 0f);
            }  
            else
            {
                NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 32, true);
                NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", Cop.CopPed, 8f);
            }
        }

        //if (PrevPoliceState != CurrentPoliceState)
        //    PoliceStateChanged();


        if (PrevPlayerKilledPolice != PoliceScanningSystem.PlayerKilledPolice)
            PlayerKilledPoliceChanged();

        if (PrevCopsKilledByPlayer != PoliceScanningSystem.CopsKilledByPlayer)
            CopsKilledChanged();

        if (PrevfiredWeapon != firedWeapon)
            FiredWeaponChanged();

        if (PrevaimedAtPolice != aimedAtPolice)
            aimedAtPoliceChanged();

        if (PrevPlayerHurtPolice != PoliceScanningSystem.PlayerHurtPolice)
            PlayerHurtPoliceChanged();

        if (PrevWantedLevel != Game.LocalPlayer.WantedLevel)
            WantedLevelChanged();

        if (PrevPlayerStarsGreyedOut != PlayerStarsGreyedOut)
            PlayerStarsGreyedOutChanged(AnyRecentlySeen);

        if (PrevPoliceState != CurrentPoliceState)
            PoliceStateChanged();

        if (CurrentPoliceState == PoliceState.Normal)
            PoliceTickNormal();
        else if (CurrentPoliceState == PoliceState.UnarmedChase)
            PoliceTickUnarmedChase();
        else if (CurrentPoliceState == PoliceState.CautiousChase)
            PoliceTickCautiousChase();
        else if (CurrentPoliceState == PoliceState.DeadlyChase)
            PoliceTickDeadlyChase();
        else if (CurrentPoliceState == PoliceState.ArrestedWait)
            PoliceTickArrestedWait();
        else
            PoliceTickNormal();

        if (Game.GameTime - WantedLevelStartTime > 180000 && AnyRecentlySeen && Game.LocalPlayer.WantedLevel > 0 && Game.LocalPlayer.WantedLevel <= 3)
        {
            Game.LocalPlayer.WantedLevel++;
            WriteToLog("WantedLevelStartTime", "Wanted Level Increased Over Time");
        }

        if (PlayerStarsGreyedOut && PoliceScanningSystem.CopPeds.All(x => !x.RecentlySeenPlayer()))
        {
            //NativeFunction.CallByName<bool>("SET_FAKE_WANTED_LEVEL", 0);
            NativeFunction.CallByName<bool>("SET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer, PoliceScanningSystem.PlacePlayerLastSeen.X, PoliceScanningSystem.PlacePlayerLastSeen.Y, PoliceScanningSystem.PlacePlayerLastSeen.Z);
            if (CanReportLastSeen && Game.GameTime - GameTimeLastGreyedOut > 4000 && AnySeenThisWanted)
            {
                WriteToLog("ReportSuspectLastSeen", "ReportSuspectLastSeen");
                DispatchAudioSystem.ReportSuspectLastSeen(rnd.NextDouble() > 0.5);
                CanReportLastSeen = false;
            }
        }

        //if (PlayerStarsGreyedOut && AnyRecentlySeen)
        //{
        //    NativeFunction.CallByName<bool>("SET_FAKE_WANTED_LEVEL", MaxWantedLastLife);
        //}

        if (AnyRecentlySeen)
        {
            NativeFunction.CallByName<bool>("SET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer, Game.LocalPlayer.Character.Position.X, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z);
            //NativeFunction.CallByName<bool>("SET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer, PoliceScanningSystem.PlacePlayerLastSeen.X, PoliceScanningSystem.PlacePlayerLastSeen.Y, PoliceScanningSystem.PlacePlayerLastSeen.Z);
        }
    }

    private static void PoliceTickNormal()
    {
        foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => x.isTasked && !x.TaskIsQueued))
        {
            Cop.TaskIsQueued = true;
            PoliceScanningSystem.CopsToTask.Add(new PoliceTask(Cop,PoliceTask.Task.Untask));
        }

    }
    private static void PoliceTickUnarmedChase()
    {
        //Cops on Foot
        foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => !x.isTasked))
        {
            if (Cop.CopPed.IsOnBike || Cop.CopPed.IsInHelicopter)
                SetUnarmed(Cop);
            else
                SetCopTazer(Cop);

            if (!isBusted && Cop.RecentlySeenPlayer() && !Cop.TaskIsQueued && PoliceScanningSystem.CopPeds.Where(x => x.isTasked || x.TaskIsQueued).Count() <= 4 && !Cop.isInVehicle && Cop.DistanceToPlayer <= 55f && (!Game.LocalPlayer.Character.IsInAnyVehicle(false) || Game.LocalPlayer.Character.CurrentVehicle.Speed <= 5f))
            {
                Cop.TaskIsQueued = true;
                PoliceScanningSystem.AddItemToQueue(new PoliceTask(Cop,PoliceTask.Task.Chase));
            }
        }

        if (SurrenderBust && !isBustTimeOut())
            SurrenderBustEvent();

        StopSearchMode();
    }
    private static void PoliceTickArrestedWait()
    {
        foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => !x.isTasked)) // Exist/Dead Check
        {
            bool InVehicle = Cop.CopPed.IsInAnyVehicle(false);
            if (InVehicle)
            {
                SetUnarmed(Cop);
            }
            else
            {
                if (!Cop.TaskIsQueued && PoliceScanningSystem.CopPeds.Where(x => x.isTasked || x.TaskIsQueued).Count() <= 3 && Cop.DistanceToPlayer <= 45f)
                {
                    Cop.TaskIsQueued = true;
                    PoliceScanningSystem.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Arrest));
                }
                else if (!Cop.TaskIsQueued && (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.NoTask || Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.Preparing) && (Cop.RecentlySeenPlayer() || Cop.DistanceToPlayer <= 65f))
                {
                    Cop.TaskIsQueued = true;
                    Cop.GameTimeLastTask = Game.GameTime;
                    PoliceScanningSystem.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.SimpleArrest));
                }
                else if (!Cop.TaskIsQueued && Game.GameTime - Cop.GameTimeLastTask > 3500 && Cop.RecentlySeenPlayer() && Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.InProgress && Cop.DistanceToPlayer > 45f)
                {
                    Cop.TaskIsQueued = true;
                    Cop.GameTimeLastTask = Game.GameTime;
                    PoliceScanningSystem.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.SimpleArrest)); //retask the arrest
                }
            }           
        }
        Game.LocalPlayer.WantedLevel = MaxWantedLastLife;

        if (PoliceScanningSystem.CopPeds.Any(x => x.DistanceToPlayer <= 4f) && Game.LocalPlayer.Character.Speed <= 4.0f && !Game.LocalPlayer.Character.IsInAnyVehicle(false) && !isBusted)
            SurrenderBust = true;

        if (SurrenderBust && !isBustTimeOut())
            SurrenderBustEvent();

        StopSearchMode();
    }
    private static void PoliceTickCautiousChase()
    {
        foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => !x.isTasked && !x.isInVehicle && !x.isInHelicopter))
        {
            SetCopDeadly(Cop);
            if (!Cop.TaskIsQueued && PoliceScanningSystem.CopPeds.Where(x => x.isTasked || x.TaskIsQueued).Count() <= 2 && Cop.DistanceToPlayer <= 45f)
            {
                Cop.TaskIsQueued = true;
                PoliceScanningSystem.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Arrest));
            }
            else if (!Cop.TaskIsQueued && Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.NoTask && (Cop.RecentlySeenPlayer() || Cop.DistanceToPlayer <= 65f))
            {
                Cop.TaskIsQueued = true;
                Cop.GameTimeLastTask = Game.GameTime;
                PoliceScanningSystem.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.SimpleArrest));
            }
            else if (!Cop.TaskIsQueued && Game.GameTime - Cop.GameTimeLastTask > 3500 && Cop.RecentlySeenPlayer() && Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.InProgress && Cop.DistanceToPlayer > 35f)
            {
                Cop.TaskIsQueued = true;
                Cop.GameTimeLastTask = Game.GameTime;
                PoliceScanningSystem.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.SimpleArrest));
            }

        }
        foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => x.isTasked && x.SimpleTaskName != ""))
        {
            if (!Cop.TaskIsQueued && Game.GameTime - Cop.GameTimeLastTask > 20000 && Cop.RecentlySeenPlayer() && Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.InProgress && Cop.DistanceToPlayer > 25f)
            {
                Cop.TaskIsQueued = true;
                Cop.GameTimeLastTask = Game.GameTime;
                WriteToLog("PoliceTickCautiousChase", "Retasked Simple Arrest");
                PoliceScanningSystem.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.SimpleArrest));
            }
            else if (!Cop.TaskIsQueued && Game.GameTime - Cop.GameTimeLastTask > 20000 && !Cop.RecentlySeenPlayer() && Cop.DistanceToPlayer > 35f)
            {
                Cop.TaskIsQueued = true;
                WriteToLog("PoliceTickDeadlyChase", "Queued An Untask");
                PoliceScanningSystem.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Untask));
            }

        }
        foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => !x.isTasked && (x.isInHelicopter || x.isOnBike)))
        {
            SetUnarmed(Cop);
        }

        if (PoliceScanningSystem.CopPeds.Any(x => x.DistanceToPlayer <= 8f) && Game.LocalPlayer.Character.Speed <= 4.0f && !Game.LocalPlayer.Character.IsInAnyVehicle(false) && !isBusted)
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
        foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => !x.isInVehicle))
        {
            SetCopDeadly(Cop);
            if (!areHandsUp && !BeingArrested && !Cop.TaskIsQueued && Cop.isTasked)
            {
                Cop.TaskIsQueued = true;
                WriteToLog("PoliceTickDeadlyChase", "Queued An Untask");
                PoliceScanningSystem.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Untask));
            }
            //if (Cop.CopPed.Tasks.CurrentTaskStatus != Rage.TaskStatus.None)
            //{
            //    Cop.CopPed.BlockPermanentEvents = false;
            //}
        }
        foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => !x.isTasked && x.isInHelicopter))
        {
            if (!areHandsUp && Game.LocalPlayer.WantedLevel >= 4)
                SetCopDeadly(Cop);
            else
                SetUnarmed(Cop);
        }

        if (SurrenderBust && !isBustTimeOut())
            SurrenderBustEvent();
    }

    private static void SurrenderBustEvent()
    {
        //return;
        //if (!NativeFunction.Natives.x36AD3E690DA5ACEB<bool>("PeyoteIn"))//_GET_SCREEN_EFFECT_IS_ACTIVE
        //    NativeFunction.Natives.x068E835A1D0DC0E3("PeyoteIn", 0, 0);//_STOP_SCREEN_EFFECT
        BeingArrested = true;
        CurrentPoliceState = PoliceState.ArrestedWait;
        NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)2725352035, true);
        areHandsUp = false;
        SurrenderBust = false;
        LastBust = Game.GameTime;
        WriteToLog("SurrenderBust", "SurrenderBust Executed");
    }
    private static bool isBustTimeOut()
    {
        if (Game.GameTime - LastBust >= 10000)
            return false;
        else
            return true;
    }
    private static void WantedLevelChanged()
    {
        if (Game.LocalPlayer.WantedLevel == 0)//Just Removed
        {
            //NativeFunction.CallByName<bool>("SET_FAKE_WANTED_LEVEL", 0);
            if(AnySeenThisWanted && MaxWantedLastLife > 0)
                DispatchAudioSystem.ReportSuspectLost();
            CurrentPoliceState = PoliceState.Normal;
            AnySeenThisWanted = false;
        }
        NativeFunction.CallByName<bool>("SET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer, Game.LocalPlayer.Character.Position.X, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z);
        WantedLevelStartTime = Game.GameTime;
        WriteToLog("ValueChecker", String.Format("WantedLevel Changed to: {0}", Game.LocalPlayer.WantedLevel));
        PrevWantedLevel = Game.LocalPlayer.WantedLevel;
    }
    private static void CopsKilledChanged()
    {
        WriteToLog("ValueChecker", String.Format("CopsKilledByPlayer Changed to: {0}", PoliceScanningSystem.CopsKilledByPlayer));
        PrevCopsKilledByPlayer = PoliceScanningSystem.CopsKilledByPlayer;
    }
    private static void PlayerHurtPoliceChanged()
    {
        WriteToLog("ValueChecker", String.Format("PlayerHurtPolice Changed to: {0}", PoliceScanningSystem.PlayerHurtPolice));
        if(PoliceScanningSystem.PlayerHurtPolice)
            DispatchAudioSystem.ReportAssualtOnOfficer();
        PrevPlayerHurtPolice = PoliceScanningSystem.PlayerHurtPolice;
    }
    private static void PlayerKilledPoliceChanged()
    {
        WriteToLog("ValueChecker", String.Format("PlayerKilledPolice Changed to: {0}", PoliceScanningSystem.PlayerKilledPolice));
        if (PoliceScanningSystem.PlayerKilledPolice)
            DispatchAudioSystem.ReportOfficerDown();
        PrevPlayerKilledPolice = PoliceScanningSystem.PlayerKilledPolice;
    }
    private static void FiredWeaponChanged()
    {
        WriteToLog("ValueChecker", String.Format("firedWeapon Changed to: {0}", firedWeapon));
        if (firedWeapon)
        {
            DispatchAudioSystem.ReportShotsFired();
        }
        PrevfiredWeapon = firedWeapon;
    }
    private static void PlayerStarsGreyedOutChanged(bool AnyRecentlySeen)
    {
        WriteToLog("ValueChecker", String.Format("PlayerStarsGreyedOut Changed to: {0}", PlayerStarsGreyedOut));
        if (PlayerStarsGreyedOut)
        {
            //DispatchAudioSystem.ReportSuspectLastSeen(true);
            CanReportLastSeen = true;
            GameTimeLastGreyedOut = Game.GameTime;
        }
        else
        {
            CanReportLastSeen = false;
        }
        //WriteToLog("ValueChecker", String.Format("GameTimeLastGreyedOut Changed to: {0}", GameTimeLastGreyedOut.ToString()));
        //WriteToLog("ValueChecker", String.Format("CanReportLastSeen Changed to: {0}", CanReportLastSeen));
        PrevPlayerStarsGreyedOut = PlayerStarsGreyedOut;
    }
    private static void aimedAtPoliceChanged()
    {
        WriteToLog("ValueChecker", String.Format("aimedAtPolice Changed to: {0}", aimedAtPolice));
        if (aimedAtPolice)
        {
            DispatchAudioSystem.ReportThreateningWithFirearm();
        }
        PrevaimedAtPolice = aimedAtPolice;
    }
    private static void PoliceStateChanged()
    {
        WriteToLog("ValueChecker", String.Format("PoliceState Changed to: {0}", CurrentPoliceState));
        WriteToLog("ValueChecker", String.Format("PreviousPoliceState Changed to: {0}", PrevPoliceState));

        if (CurrentPoliceState == PoliceState.Normal)
        {
            ResetPoliceStats();
        }

        if (CurrentPoliceState == PoliceState.ArrestedWait)
        {

        }

        if (CurrentPoliceState == PoliceState.DeadlyChase)
        {
            if(PrevPoliceState != PoliceState.ArrestedWait)
                DispatchAudioSystem.ReportLethalForceAuthorized();
        }

        PrevPoliceState = CurrentPoliceState;
    }
    internal static void KillPlayer()
    {
        Game.LocalPlayer.Character.Kill();
        //isDead = true;
    }
    private static void StopSearchMode()
    {

        if (!GhostCop.Exists())
        {
            CreateGhostCop();
        }


        if (GhostCopFollow && GhostCop != null)
        {
            GhostCop.Position = Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0f, 4f, 1f));
            GhostCop.Heading = Game.LocalPlayer.Character.Heading;
        }


        //if (Game.GameTime - GameTimeLastReTasked <= 2000)
        //    return;

        if (PoliceScanningSystem.CopPeds.Any(x => x.RecentlySeenPlayer()))//if (NativeFunction.CallByName<bool>("ARE_PLAYER_STARS_GREYED_OUT", Game.LocalPlayer) && PoliceScanningSystem.CopPeds.Any(x => x.RecentlySeenPlayer())) // Needed for the AI to keep the player in the wanted position
        {
            GhostCopFollow = true;
        }
        else
        {
            if(GhostCop != null)
                GhostCop.Position = new Vector3(0f, 0f, 0f);
            GhostCopFollow = false;
        }


        if (NativeFunction.CallByName<bool>("ARE_PLAYER_STARS_GREYED_OUT", Game.LocalPlayer) && !PoliceScanningSystem.CopPeds.Any(x => x.RecentlySeenPlayer()) && PoliceScanningSystem.CopPeds.Any(x => x.isTasked))
        {
            PoliceScanningSystem.UntaskAll();
        }
    }
    //private static void RemoveGhostCop()
    //{
    //    if (GhostCop.Exists())
    //        GhostCop.Delete();
    //}
    //private static void MoveGhostCopToPlayer()
    //{
    //    if (!GhostCop.Exists())
    //        CreateGhostCop();
    //    else
    //    {
    //        GhostCop.Position = Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0f, 4f, 1f));
    //        GhostCop.Heading = Game.LocalPlayer.Character.Heading;
    //    }

    //    WriteToLog("CreateGhostCop", "Ghost Cop Moved");
    //    GameTimeLastReTasked = Game.GameTime;
    //}
    private static void CreateGhostCop()
    {
        GhostCop = new Ped(CopModel, Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0f, 4f, 0f)), Game.LocalPlayer.Character.Heading);
        GhostCop.BlockPermanentEvents = false;
        GhostCop.IsPersistent = true;
        GhostCop.IsCollisionEnabled = false;
        GhostCop.IsVisible = false;
        Blip myBlip = GhostCop.GetAttachedBlip();
        if(myBlip != null)
            myBlip.Delete();
        GhostCop.VisionRange = 100f;
        GhostCop.HearingRange = 100f;
        GhostCop.CanRagdoll = false;
        const ulong SetPedMute = 0x7A73D05A607734C7;
        NativeFunction.CallByHash<uint>(SetPedMute, GhostCop);

    NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", GhostCop, (uint)2725352035, true); //Unequip weapon so you don't get shot
        NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", GhostCop, false);
        NativeFunction.CallByName<uint>("SET_PED_MOVE_RATE_OVERRIDE", GhostCop, 0f);
        GhostCopFollow = true;
        WriteToLog("CreateGhostCop", "Ghost Cop Created");
    }

    public static void SetUnarmed(GTACop Cop)
    {
        if (!Cop.CopPed.Exists() || (Cop.SetUnarmed && !Cop.NeedsWeaponCheck))
            return;
        Cop.CopPed.Accuracy = 10;
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
        Cop.CopPed.Accuracy = 10;
        NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Cop.CopPed, 30);
        //Cop.CopPed.BlockPermanentEvents = false;
        if (!Cop.CopPed.Inventory.Weapons.Contains(WeaponHash.Pistol))
            Cop.CopPed.Inventory.GiveNewWeapon(WeaponHash.Pistol, -1, false);
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
        Cop.CopPed.Accuracy = 10;
        NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Cop.CopPed, 30);
        //Cop.CopPed.BlockPermanentEvents = false;

        //TargetCop.Inventory.GiveNewWeapon(WeaponHash.Pistol, 100, true);


        if (!Cop.CopPed.Inventory.Weapons.Contains(WeaponHash.Pistol))
            Cop.CopPed.Inventory.GiveNewWeapon(WeaponHash.Pistol, -1, true);
        
        if((Cop.CopPed.Inventory.EquippedWeapon == null || Cop.CopPed.Inventory.EquippedWeapon.Hash == WeaponHash.StunGun) && Game.LocalPlayer.WantedLevel >= 0)
            Cop.CopPed.Inventory.GiveNewWeapon(WeaponHash.Pistol, -1, true);

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

        Cop.CopPed.Accuracy = 30;
        NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Cop.CopPed, 100);
        //Cop.CopPed.Inventory.GiveNewWeapon(WeaponHash.StunGun, 100, true);
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

    private static void StateTick()
    {

        //Dead
        if (Game.LocalPlayer.Character.IsDead && !isDead)
        {
            PositionOfDeath = Game.LocalPlayer.Character.Position;
            DiedInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
            isDead = true;
            NativeFunction.Natives.x2206BF9A37B7F724("DeathFailOut", 0, 0);//_START_SCREEN_EFFECT
            Game.LocalPlayer.Character.Kill();
            Game.LocalPlayer.Character.Health = 0;
            Game.LocalPlayer.Character.IsInvincible = true;
            Game.LocalPlayer.WantedLevel = 0;         
            Game.TimeScale = .4f;
            Menus.deathMenu.Visible = true;

            if (PrevWantedLevel > 0 || PoliceScanningSystem.CopPeds.Any(x => x.isTasked))
                DispatchAudioSystem.ReportSuspectWasted();
        }

        // Busted
        if (NativeFunction.CallByName<bool>("IS_PLAYER_BEING_ARRESTED", 0))
        {
            BeingArrested = true;
        }
        if (NativeFunction.CallByName<bool>("IS_PLAYER_BEING_ARRESTED", 1))
        {
            BeingArrested = true;
            Game.LocalPlayer.Character.Tasks.Clear();
        }

        if (BeingArrested && !isBusted)
        {
            PositionOfDeath = Game.LocalPlayer.Character.Position;
            DiedInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
            isBusted = true;
            BeingArrested = true;
            Game.LocalPlayer.Character.Tasks.Clear();
            NativeFunction.Natives.x2206BF9A37B7F724("DeathFailOut", 0, 0);//_START_SCREEN_EFFECT
            //MenuEnableDisable();
            Game.TimeScale = .4f;
            areHandsUp = false;
            Menus.bustedMenu.Visible = true;
            SetArrestedAnimation(Game.LocalPlayer.Character, false);
            DispatchAudioSystem.ReportSuspectArrested();
        }

        //NativeFunction.CallByName<uint>("DISPLAY_HUD", true);

        if (Game.LocalPlayer.WantedLevel > PreviousWantedLevel)
            PreviousWantedLevel = Game.LocalPlayer.WantedLevel;

        if (Game.LocalPlayer.WantedLevel > MaxWantedLastLife) // The max wanted level i saw in the last life, not just right before being busted
            MaxWantedLastLife = Game.LocalPlayer.WantedLevel;
        else if (Game.LocalPlayer.WantedLevel == 0 && MaxWantedLastLife > 0 && !isBusted && !isDead)
            MaxWantedLastLife = 0;

    }
    private static void ControlTick()
    {
        if (Game.IsKeyDownRightNow(Keys.E) && !Game.LocalPlayer.IsFreeAiming && (!Game.LocalPlayer.Character.IsInAnyVehicle(false) || Game.LocalPlayer.Character.CurrentVehicle.Speed < 2.5f))
        {
            
            if (!areHandsUp && !isBusted)
            {
                if (!(Game.LocalPlayer.Character.Inventory.EquippedWeapon == null))
                    NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)2725352035, true); //Unequip weapon so you don't get shot
                HandsUpPreviousPoliceState = CurrentPoliceState;
                areHandsUp = true;
                //vehicleEngineSystem.TurnOffEngine();
                RaiseHands();
                if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Speed <= 10f)
                    Game.LocalPlayer.Character.CurrentVehicle.IsDriveable = false;
            }

        }
        else
        {
            if (areHandsUp && !isBusted)
            {
                areHandsUp = false; // You put your hands down
                CurrentPoliceState = HandsUpPreviousPoliceState;
                Game.LocalPlayer.Character.Tasks.Clear();
                if (Game.LocalPlayer.Character.IsInAnyVehicle(false))
                    Game.LocalPlayer.Character.CurrentVehicle.IsDriveable = true;
            }
        }
    }
    private static void RaiseHands()
    {
        if(Game.LocalPlayer.WantedLevel > 0)
            CurrentPoliceState = PoliceState.ArrestedWait;

        bool inVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
        var sDict = (inVehicle) ? "veh@busted_std" : "ped";
        RequestAnimationDictionay(sDict);
        if (inVehicle)
        {
            NativeFunction.CallByName<bool>("ROLL_DOWN_WINDOW", Game.LocalPlayer.Character.CurrentVehicle, 0);
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, sDict, "stay_in_car_crim", 2.0f, -2.0f, -1, 50, 0, true, false, true);
            //GameFiber.Sleep(250);
            //NativeFunction.CallByName<bool>("SET_ENTITY_ANIM_CURRENT_TIME", Game.LocalPlayer.Character, sDict, "stay_in_car_crim", 0.5f);
        }
        else
        {
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, sDict, "handsup_enter", 2.0f, -2.0f, -1, 2, 0, false, false, false);
        }
        
    }

    public static void RequestAnimationDictionay(String sDict)
    {
        NativeFunction.CallByName<bool>("REQUEST_ANIM_DICT", sDict);
        while (!NativeFunction.CallByName<bool>("HAS_ANIM_DICT_LOADED", sDict))
            GameFiber.Yield();
    }
    public static void RespawnInPlace(bool AsOldCharacter)
    {
        try
        {
            isDead = false;
            isBusted = false;
            BeingArrested = false;
            Game.LocalPlayer.Character.Health = 100;
            if (DiedInVehicle)
            {
                NativeFunction.Natives.xB69317BF5E782347(Game.LocalPlayer.Character);
                //NativeFunction.CallByName<uint>("NETWORK_REQUEST_CONTROL_OF_ENTITY", Game.LocalPlayer.Character);      
                NativeFunction.Natives.xEA23C49EAA83ACFB(Game.LocalPlayer.Character.Position.X + 10f, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z, 0, false, false);
                //NativeFunction.CallByName<uint>("NETWORK_RESURRECT_LOCAL_PLAYER", Game.LocalPlayer.Character.Position.X, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z, 0, false, false);
                //NativeFunction.Natives.NetworkResurrectLocalPlayer(Game.LocalPlayer.Character.Position.X + 10F, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z, Camera.RenderingCamera.Direction, false, false);
                NativeFunction.Natives.xC0AA53F866B3134D();//_RESET_LOCALPLAYER_STATE
                if (Game.LocalPlayer.Character.LastVehicle.Exists())
                {
                    Game.LocalPlayer.Character.WarpIntoVehicle(Game.LocalPlayer.Character.LastVehicle, -1);
                }
                NativeFunction.Natives.xC0AA53F866B3134D();//_RESET_LOCALPLAYER_STATE
            }
            else
            {
                NativeFunction.Natives.xB69317BF5E782347(Game.LocalPlayer.Character);
                //NativeFunction.CallByName<uint>("NETWORK_REQUEST_CONTROL_OF_ENTITY", Game.LocalPlayer.Character);
                NativeFunction.Natives.xEA23C49EAA83ACFB(Game.LocalPlayer.Character.Position.X, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z, 0, false, false);
                //NativeFunction.CallByName<uint>("NETWORK_RESURRECT_LOCAL_PLAYER", Game.LocalPlayer.Character.Position.X, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z, 0, false, false);
                NativeFunction.Natives.xC0AA53F866B3134D();//_RESET_LOCALPLAYER_STATE
            }
            if (AsOldCharacter)
            {
                //MaxWantedLastLife = 0;
                Game.LocalPlayer.WantedLevel = MaxWantedLastLife;
                ++TimesDied;
            }
            else
            {
                Game.LocalPlayer.Character.Inventory.Weapons.Clear();
                Game.LocalPlayer.Character.Inventory.GiveNewWeapon(2725352035, 0, true);
               // Game.LocalPlayer.Character.Inventory.Weapons.Clear();
                //Game.LocalPlayer.Character.Inventory.GiveNewWeapon(WeaponDescriptor., 0, true, true);
                //if (mySettings.ReplacePlayerWithPed && mySettings.ReplacePlayerWithPedRandomMoney)
                //    Game.Player.Character.SetCash(0, mySettings.ReplacePlayerWithPedCharacter);
                PreviousWantedLevel = 0;
                Game.LocalPlayer.WantedLevel = 0;
                TimesDied = 0;
                MaxWantedLastLife = 0;
                ResetPoliceStats();
                DispatchAudioSystem.ResetReportedItems();
            }
            Game.TimeScale = 1f;
            DiedInVehicle = false;
            NativeFunction.Natives.xB4EDDC19532BFB85(); //_STOP_ALL_SCREEN_EFFECTS
            ResetPlayer(false, false);
            Game.HandleRespawn();

        }
        catch (Exception e)
        {
            Game.LogTrivial(e.Message);
            // UI.Notify(e.Message);
        }
    }
    public static Ped GetPedestrian(float Radius, bool Nearest)
    {
        Ped PedToReturn = null;   
        Ped[] closestPed = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, Radius, GetEntitiesFlags.ConsiderHumanPeds | GetEntitiesFlags.ExcludePlayerPed | GetEntitiesFlags.ConsiderAllPeds).Where(x => x is Ped).ToArray(), (x => (Ped)x));
        if (Nearest)
            PedToReturn = closestPed.Where(s => s.CanTakeoverPed()).OrderBy(s => Vector3.Distance(Game.LocalPlayer.Character.Position, s.Position)).FirstOrDefault();
        else
            PedToReturn = closestPed.Where(s => s.CanTakeoverPed()).OrderBy(s => rnd.Next()).FirstOrDefault();
        if (PedToReturn == null)
            return null;
        else if (PedToReturn.IsInAnyVehicle(false))
        {
            if (PedToReturn.CurrentVehicle.Driver.Exists())
            {
                PedToReturn.CurrentVehicle.Driver.MakePersistent();
                return PedToReturn.CurrentVehicle.Driver;
            }
            else
            {
                PedToReturn.MakePersistent();
                return PedToReturn;
            }
        }
        else
        {
            PedToReturn.MakePersistent();
            return PedToReturn;
        }
    }
    //public bool TakeoverPedCamera(Ped TargetPed)
    //{
    //    if (TargetPed == null || Vector3.Distance2D(Game.Player.Character.Position, TargetPed.Position) <= 80f)
    //        return false;

    //    Vector3 TargetPosition = TargetPed.Position;
    //    Vector3 TargetRotation = TargetPed.Rotation;

    //    Game.TimeScale = .2f;

    //    Camera Cam1 = World.CreateCamera(World.RenderingCamera.Position, World.RenderingCamera.Rotation, 90f);
    //    Cam1.AttachTo(Game.Player.Character, World.RenderingCamera.GetOffsetFromWorldCoords(new Vector3(0.0f, -2f, 0f)));
    //    Cam1.PointAt(Game.Player.Character);
    //    World.RenderingCamera = Cam1;

    //    Camera Cam2 = World.CreateCamera(Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.0f, 0.0f, 20f)), Game.Player.Character.Rotation, 90f);
    //    Cam2.PointAt(Game.Player.Character);
    //    Cam1.InterpTo(Cam2, 1000, true, true);
    //    Wait(1000);

    //    Camera Cam3 = World.CreateCamera(Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.0f, 0.0f, 200f)), Game.Player.Character.Rotation, 90f);
    //    Cam3.PointAt(Game.Player.Character);
    //    Cam2.InterpTo(Cam3, 1000, true, true);
    //    Wait(1000);

    //    Vector3 AboveTarget = TargetPosition;
    //    AboveTarget.Z = AboveTarget.Z + 200f;

    //    Camera Cam4 = World.CreateCamera(AboveTarget, TargetRotation, 90f);
    //    Cam4.PointAt(TargetPosition);
    //    Cam3.InterpTo(Cam4, 1000, true, true);
    //    Wait(1000);

    //    Vector3 CloseAboveTarget = TargetPosition;
    //    CloseAboveTarget.Z = CloseAboveTarget.Z + 20f;

    //    Camera Cam5 = World.CreateCamera(CloseAboveTarget, TargetRotation, 90f);
    //    Cam5.PointAt(TargetPosition);
    //    Cam4.InterpTo(Cam5, 1000, true, true);
    //    Wait(1500);

    //    Vector3 BehindTarget = TargetPosition;
    //    BehindTarget.Y = CloseAboveTarget.Y + -2f;

    //    Camera Cam6 = World.CreateCamera(TargetPosition, TargetRotation, 90f);
    //    Cam6.AttachTo(TargetPed, BehindTarget);
    //    Cam6.PointAt(TargetPosition);
    //    Cam5.InterpTo(Cam6, 1000, true, true);

    //    Game.TimeScale = 1f;
    //    return true;
    //}
    public static void TakeoverPed(Ped TargetPed, bool DeleteOld, bool ArrestOld)
    {
        try
        {
            if (TargetPed == null)
                return;

            if (TargetPed.Model.Hash == 225514697)
                WriteToLog("TakeoverPed", "TargetPed.Model.Hash: " + TargetPed.Model.Hash.ToString());
            else
                LastModelHash = TargetPed.Model.Name;

            bool wasInVehicle = TargetPed.IsInAnyVehicle(false);
            Vehicle carWasIn = null;
            if (wasInVehicle)
                carWasIn = TargetPed.CurrentVehicle;

            CopyPedComponentVariation(TargetPed);

            Ped CurrentPed = Game.LocalPlayer.Character;
            if (TargetPed.IsInAnyVehicle(false))
            {
                Game.LocalPlayer.Character.WarpIntoVehicle(carWasIn, -1);
                //AllyPedsToPlayer(TargetPed.CurrentVehicle.Passengers);
            }
            else
            {
                //AllyPedsToPlayer(World.GetNearbyPeds(Game.Player.Character.Position, 3f));
            }

            NativeFunction.CallByName<uint>("CHANGE_PLAYER_PED", Game.LocalPlayer, TargetPed, false, false);

            if (DeleteOld)
                CurrentPed.Delete();
            //else if (ArrestOld)
            //    SetArrestedAnimation(CurrentPed, true);
            //else
            //    AITakeoverPlayer(CurrentPed);



            NativeFunction.Natives.x2206BF9A37B7F724("MinigameTransitionOut", 5000, false);


            SetPlayerOffset();
            ChangeModel("player_zero");          
            ChangeModel(LastModelHash);


            if (!Game.LocalPlayer.Character.isMainCharacter())
                ReplacePedComponentVariation(Game.LocalPlayer.Character);

            if (wasInVehicle)
            {
                Game.LocalPlayer.Character.WarpIntoVehicle(carWasIn, -1);
                NativeFunction.CallByName<bool>("SET_VEHICLE_HAS_BEEN_OWNED_BY_PLAYER", Game.LocalPlayer.Character.CurrentVehicle, true);
            }
            else
            {
                Game.LocalPlayer.Character.IsCollisionEnabled = true;
            }

            Game.LocalPlayer.Character.SetCash(rnd.Next(500, 4000),"Michael");


            Game.LocalPlayer.Character.Inventory.Weapons.Clear();
            Game.LocalPlayer.Character.Inventory.GiveNewWeapon(2725352035, 0, true);
            TimesDied = 0;
            MaxWantedLastLife = 0;
            Game.TimeScale = 1f;
            Game.LocalPlayer.WantedLevel = 0;

            Game.HandleRespawn();
        }
        catch (Exception e3)
        {
            WriteToLog("TakeoverPed", "TakeoverPed Error; " + e3.Message);
        }
    }
    private static void CopyPedComponentVariation(Ped myPed)
    {
        try
        {
            myPedVariation = new PedVariation();
            myPedVariation.myPedComponents = new List<PedComponent>();
            myPedVariation.myPedProps = new List<PropComponent>();
            for (int ComponentNumber = 0; ComponentNumber < 12; ComponentNumber++)
            {
                myPedVariation.myPedComponents.Add(new PedComponent(ComponentNumber, NativeFunction.CallByName<int>("GET_PED_DRAWABLE_VARIATION", myPed, ComponentNumber), NativeFunction.CallByName<int>("GET_PED_TEXTURE_VARIATION", myPed, ComponentNumber), NativeFunction.CallByName<int>("GET_PED_PALETTE_VARIATION", myPed, ComponentNumber)));
            }
            for(int PropNumber = 0; PropNumber < 8;PropNumber++)
            {
                myPedVariation.myPedProps.Add(new PropComponent(PropNumber, NativeFunction.CallByName<int>("GET_PED_PROP_INDEX", myPed, PropNumber), NativeFunction.CallByName<int>("GET_PED_PROP_TEXTURE_INDEX", myPed, PropNumber)));
            }
        }
        catch (Exception e)
        {
            WriteToLog("CopyPedComponentVariation", "CopyPedComponentVariation Error; " + e.Message);
        }
    }
    private static void ReplacePedComponentVariation(Ped myPed)
    {
        try
        {
            foreach (PedComponent Component in myPedVariation.myPedComponents)
            {
                NativeFunction.CallByName<uint>("SET_PED_COMPONENT_VARIATION", myPed, Component.ComponentID, Component.DrawableID, Component.TextureID, Component.PaletteID);
            }
            foreach (PropComponent Prop in myPedVariation.myPedProps)
            {
                NativeFunction.CallByName<uint>("SET_PED_PROP_INDEX", myPed, Prop.PropID, Prop.DrawableID, Prop.TextureID,false);
            }
        }
        catch (Exception e)
        {
            WriteToLog("ReplacePedComponentVariation", "ReplacePedComponentVariation Error; " + e.Message);
        }
    }
    private static void SetPlayerOffset()
    {
        const int WORLD_OFFSET = 8;
        const int SECOND_OFFSET = 0x20;
        const int THIRD_OFFSET = 0x18;

        Memory GTA = new Memory("GTA5");
        UInt64 WorldFlirtPointer = GTA.PointerScan("48 8B 05 ? ? ? ? 45 ? ? ? ? 48 8B 48 08 48 85 C9 74 07");
        UInt64 World = GTA.ReadRelativeAddress(WorldFlirtPointer);
        UInt64 Player = GTA.Read<UInt64>(World, new int[] { WORLD_OFFSET });
        UInt64 Second = GTA.Read<UInt64>(Player + SECOND_OFFSET);
        UInt64 Third = GTA.Read<UInt64>(Second + THIRD_OFFSET);

        //if (mySettings.ReplacePlayerWithPedCharacter == "Michael")
            GTA.Write<uint>(Player + SECOND_OFFSET, 225514697, new int[] { THIRD_OFFSET });
        //else if (mySettings.ReplacePlayerWithPedCharacter == "Franklin")
        //    GTA.Write<uint>(Player + SECOND_OFFSET, 2602752943, new int[] { THIRD_OFFSET });
        //else if (mySettings.ReplacePlayerWithPedCharacter == "Trevor")
        //    GTA.Write<uint>(Player + SECOND_OFFSET, 2608926626, new int[] { THIRD_OFFSET });

    }
    private static void ChangeModel(String ModelRequested)
    {
        // Request the character model
        Model characterModel = new Model(ModelRequested);
        characterModel.LoadAndWait();
        characterModel.LoadCollisionAndWait();
        //while (!characterModel.IsCollisionLoaded)
        //{
        //    GameFiber.Yield();
        //}
        //Game.LocalPlayer.Model.ch
        Game.LocalPlayer.Model = characterModel;
        Game.LocalPlayer.Character.IsCollisionEnabled = true;
        //characterModel.Request(500);
        //// Check the model is valid
        //if (characterModel.IsInCdImage && characterModel.IsValid)
        //{
        //    // If the model isn't loaded, wait until it is   
        //    while (!characterModel.IsLoaded)
        //    {
        //        Script.Wait(100);
        //    }
        //    // Set the player's model    
        //    Function.Call(Hash.SET_PLAYER_MODEL, Game.Player, characterModel.Hash);
        //}
        //// Delete the model from memory after we've assigned it
        //characterModel.MarkAsNoLongerNeeded();
    }
    private static void SetArrestedAnimation(Ped PedToArrest, bool MarkAsNoLongerNeeded)
    {
        GameFiber.StartNew(delegate
        {
            RequestAnimationDictionay("veh@busted_std");
            RequestAnimationDictionay("busted");


            while(PedToArrest.IsRagdoll || PedToArrest.IsStunned)
                GameFiber.Yield();

            if (!PedToArrest.Exists())
                return;

            if (PedToArrest.IsInAnyVehicle(false))
            {
                Vehicle oldVehicle = PedToArrest.CurrentVehicle;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "veh@busted_std", "get_out_car_crim", 2.0f, -2.0f, 2500, 50, 0, false, false, false);
                GameFiber.Wait(2500);
                if (PedToArrest.Exists() && !oldVehicle.Exists())
                    NativeFunction.CallByName<uint>("TASK_LEAVE_VEHICLE", PedToArrest, oldVehicle, 256);
            }
            if (PedToArrest == Game.LocalPlayer.Character && !isBusted)
                return;
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "busted", "idle_2_hands_up", 2.0f, -8.0f, 5000, 2, 0, false, false, false);
            GameFiber.Wait(5000);
            if (!PedToArrest.Exists() || (PedToArrest == Game.LocalPlayer.Character && !isBusted))
                return;
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "busted", "idle_a", 8.0f, -8.0f, -1, 1, 0, false, false, false);
            PedToArrest.KeepTasks = true;
            if (MarkAsNoLongerNeeded)
                PedToArrest.IsPersistent = false;
        });

    }
    private static void UnSetArrestedAnimation(Ped PedToArrest)
    {
        GameFiber.StartNew(delegate
        {
            RequestAnimationDictionay("random@arrests");
        RequestAnimationDictionay("busted");

        if (NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_a", 1) || NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_2_hands_up", 1))
        {
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "random@arrests", "kneeling_arrest_escape", 8.0f, -8.0f, -1, 4096, 0, 0, 1, 0);
        }
        });
    }
    public static void CommitSuicide(Ped PedToSuicide)
    {
        GameFiber.StartNew(delegate
        {
            if (!PedToSuicide.IsInAnyVehicle(false))
            {
                RequestAnimationDictionay("mp_suicide");
                if (PedToSuicide.Inventory.EquippedWeapon != null && PedToSuicide.Inventory.EquippedWeapon.Hash == WeaponHash.Pistol)
                {
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToSuicide, "mp_suicide", "pistol", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                    GameFiber.Wait(750);
                }
                else
                {
                    NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", PedToSuicide, (uint)2725352035, true);
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToSuicide, "mp_suicide", "pill", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                    GameFiber.Wait(2500);
                }
            }
            PedToSuicide.Kill();
        });
    }
    public static bool RadioIn()
    {
        GTACop Cop = PoliceScanningSystem.PrimaryPursuer;
        if (Cop == null)
            return false;
        else
        {   GameFiber TaskFiber = 
            GameFiber.StartNew(delegate
            {
                RequestAnimationDictionay("random@arrests");
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Cop.CopPed, "random@arrests", "generic_radio_enter", 2.0f, -2.0f, 2500, 16, 0, false, false, false);
                
            });
            GameFiber AudioFiber =
            GameFiber.StartNew(delegate
            {
                Cop.CopPed.PlayAmbientSpeech("GENERIC_FRIGHTENED_MED");
            });

            Cop.isTasked = false; //Doing the animation clears out his other tasks?
            while (AudioFiber.IsAlive && TaskFiber.IsAlive)
                GameFiber.Yield();

            if (Cop.CopPed.Exists() && !Cop.CopPed.IsDead)
                return true; //successfully radioed it in
            else
                return false; //died beforehand
        }

    }
    public static void RespawnAtHospital()
    {
        Game.FadeScreenOut(1500);
        GameFiber.Wait(1500);
        isDead = false;
        isBusted = false;
        CurrentPoliceState = PoliceState.Normal;

        Game.LocalPlayer.Character.Inventory.Weapons.Clear();
        RespawnInPlace(true);
        Game.LocalPlayer.WantedLevel = 0;
        EmergencyLocation ClosestPolice = EmergencyLocations.Where(x => x.Type == EmergencyLocation.EmergencyLocationType.Hospital).OrderBy(s => Game.LocalPlayer.Character.Position.DistanceTo2D(s.Location)).FirstOrDefault();

        Game.LocalPlayer.Character.Position = ClosestPolice.Location;
        Game.LocalPlayer.Character.Heading = ClosestPolice.Heading;


        GameFiber.Wait(1500);
        Game.FadeScreenIn(1500);

        if (Settings.ReplacePlayerWithPedRandomMoney)
        {
            int CurrentCash = Game.LocalPlayer.Character.GetCash(Settings.ReplacePlayerWithPedCharacter);
            if (CurrentCash < 5000)
                Game.LocalPlayer.Character.SetCash(0, Settings.ReplacePlayerWithPedCharacter); // Stop it from removing and adding cash, you cant go negative.
            else
                Game.LocalPlayer.Character.GiveCash(-5000, Settings.ReplacePlayerWithPedCharacter);
        }
    }
    public static void ResistArrest()
    {
        isBusted = false;
        BeingArrested = false;
        areHandsUp = false;
        CurrentPoliceState = PoliceState.DeadlyChase;
        UnSetArrestedAnimation(Game.LocalPlayer.Character);
        NativeFunction.CallByName<uint>("RESET_PLAYER_ARREST_STATE", Game.LocalPlayer);
        ResetPlayer(false, false);
        PoliceScanningSystem.UntaskAll();
    }
    public static void Surrender()
    {
        Game.FadeScreenOut(2500);
        GameFiber.Wait(2500);
        BeingArrested = false;
        isBusted = false;
        Game.LocalPlayer.WantedLevel = 0;
        RaiseHands();
        NativeFunction.CallByName<bool>("RESET_PLAYER_ARREST_STATE", Game.LocalPlayer);
        EmergencyLocation ClosestPolice = EmergencyLocations.Where(x => x.Type == EmergencyLocation.EmergencyLocationType.Police).OrderBy(s => Game.LocalPlayer.Character.Position.DistanceTo2D(s.Location)).FirstOrDefault();
        Game.LocalPlayer.Character.Position = ClosestPolice.Location;
        Game.LocalPlayer.Character.Heading = ClosestPolice.Heading;
        Game.LocalPlayer.Character.Tasks.ClearImmediately();
        Game.LocalPlayer.Character.Inventory.Weapons.Clear();
        Game.LocalPlayer.Character.Inventory.GiveNewWeapon((WeaponHash)2725352035, -1, true);
        ResetPlayer(true, true);

        CurrentPoliceState = PoliceState.Normal;
        Game.FadeScreenIn(2500);
        Game.DisplayNotification("You are out on bail, try to stay out of trouble");
        if (Settings.ReplacePlayerWithPedRandomMoney)
            Game.LocalPlayer.Character.GiveCash(MaxWantedLastLife * -750, Settings.ReplacePlayerWithPedCharacter);

        PoliceScanningSystem.UntaskAll();
    }
    public static void BribePolice(int Amount)
    {
        if (Amount < PreviousWantedLevel * 500)
        {
            Game.DisplayNotification("Thats it? Thanks for the cash, but you're going downtown.");
            Game.LocalPlayer.Character.GiveCash(-1 * Amount, Settings.ReplacePlayerWithPedCharacter);
            return;
        }
        else
        {
            BeingArrested = false;
            isBusted = false;
            Game.DisplayNotification("Thanks for the cash, now beat it.");
            Game.LocalPlayer.Character.GiveCash(-1 * Amount, Settings.ReplacePlayerWithPedCharacter);
        }
        CurrentPoliceState = PoliceState.Normal;
        UnSetArrestedAnimation(Game.LocalPlayer.Character);
        NativeFunction.CallByName<bool>("RESET_PLAYER_ARREST_STATE", Game.LocalPlayer);
        if(Game.LocalPlayer.Character.LastVehicle.Exists())
            NativeFunction.CallByName<bool>("SET_VEHICLE_HAS_BEEN_OWNED_BY_PLAYER", Game.LocalPlayer.Character.LastVehicle, true);
        ResetPlayer(true, false);

        PoliceScanningSystem.UntaskAll();

    }
    public static void WriteToLog(String ProcedureString, String TextToLog)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ": " + ProcedureString + ": " + TextToLog + System.Environment.NewLine);
        File.AppendAllText("Plugins\\InstantAction\\" + "log.txt", sb.ToString());
        sb.Clear();
    }
    public static void ResetPlayer(bool ClearWanted, bool ResetHealth)
    {
        isDead = false;
        isBusted = false;
        BeingArrested = false;
        firedWeapon = false;
        aimedAtPolice = false;
        PoliceScanningSystem.PlayerHurtPolice = false;
        NativeFunction.CallByName<bool>("NETWORK_REQUEST_CONTROL_OF_ENTITY", Game.LocalPlayer.Character);
        NativeFunction.Natives.xC0AA53F866B3134D();
        Game.TimeScale = 1f;
        if (ClearWanted)
            Game.LocalPlayer.WantedLevel = 0;

        //ResetCamera();
        NativeFunction.Natives.xB4EDDC19532BFB85(); //_STOP_ALL_SCREEN_EFFECTS;
        if (ResetHealth)
            Game.LocalPlayer.Character.Health = 100;
    }
    private static void setupLocations()
    {

        EmergencyLocation PillBoxHillHospital = new EmergencyLocation(new Vector3(364.7124f, -583.1641f, 28.69318f), 280.637f, EmergencyLocation.EmergencyLocationType.Hospital, "Pill Box Hill Hospital");
        EmergencyLocation CentralLosStantosHospital = new EmergencyLocation(new Vector3(338.208f, -1396.154f, 32.50927f), 77.07102f, EmergencyLocation.EmergencyLocationType.Hospital, "Central Los Santos Hospital");
        EmergencyLocation SandyShoresHospital = new EmergencyLocation(new Vector3(1842.057f, 3668.679f, 33.67996f), 228.3818f, EmergencyLocation.EmergencyLocationType.Hospital, "Sandy Shores Hospital");
        EmergencyLocation PaletoBayHospital = new EmergencyLocation(new Vector3(-244.3214f, 6328.575f, 32.42618f), 219.7734f, EmergencyLocation.EmergencyLocationType.Hospital, "Paleto Bay Hospital");

        EmergencyLocations.Add(PillBoxHillHospital);
        EmergencyLocations.Add(CentralLosStantosHospital);
        EmergencyLocations.Add(SandyShoresHospital);
        EmergencyLocations.Add(PaletoBayHospital);


        EmergencyLocation DavisPolice = new EmergencyLocation(new Vector3(358.9726f, -1582.881f, 29.29195f), 323.5287f, EmergencyLocation.EmergencyLocationType.Police, "Davis Police Station");
        EmergencyLocation SandyShoresPolice = new EmergencyLocation(new Vector3(1858.19f, 3679.873f, 33.75724f), 218.3256f, EmergencyLocation.EmergencyLocationType.Police, "Sandy Shores Police Station");
        EmergencyLocation PaletoBayPolice = new EmergencyLocation(new Vector3(-437.973f, 6021.403f, 31.49011f), 316.3756f, EmergencyLocation.EmergencyLocationType.Police, "Paleto Bay Police Station");
        EmergencyLocation MissionRowPolice = new EmergencyLocation(new Vector3(440.0835f, -982.3911f, 30.68966f), 47.88088f, EmergencyLocation.EmergencyLocationType.Police, "Mission Row Police Station");
        EmergencyLocation LasMesaPolice = new EmergencyLocation(new Vector3(815.8774f, -1290.531f, 26.28391f), 74.91704f, EmergencyLocation.EmergencyLocationType.Police, "La Mesa Police Station");
        EmergencyLocation VinewoodPolice = new EmergencyLocation(new Vector3(642.1356f, -3.134667f, 82.78872f), 215.299f, EmergencyLocation.EmergencyLocationType.Police, "Vinewood Police Station");
        EmergencyLocation RockfordHillsPolice = new EmergencyLocation(new Vector3(-557.0687f, -134.7315f, 38.20231f), 214.5968f, EmergencyLocation.EmergencyLocationType.Police, "Vinewood Police Station");
        EmergencyLocation VespucciPolice = new EmergencyLocation(new Vector3(-1093.817f, -807.1993f, 19.28864f), 22.23846f, EmergencyLocation.EmergencyLocationType.Police, "Vinewood Police Station");

        EmergencyLocations.Add(DavisPolice);
        EmergencyLocations.Add(SandyShoresPolice);
        EmergencyLocations.Add(PaletoBayPolice);
        EmergencyLocations.Add(MissionRowPolice);
        EmergencyLocations.Add(LasMesaPolice);
        EmergencyLocations.Add(VinewoodPolice);
        EmergencyLocations.Add(RockfordHillsPolice);
        EmergencyLocations.Add(VespucciPolice);

    }
    public static GTAWeapon GetRandomWeapon(int RequestedLevel)
    {
        List<GTAWeapon> Weapons = new List<GTAWeapon>();
        //GTAWeapon HeavyPistol = new GTAWeapon(WeaponHash.pi, 40, "PISTOL", 1);
        //Weapons.Add(HeavyPistol);
        GTAWeapon Pistol = new GTAWeapon(WeaponHash.Pistol, 60, "PISTOL", 1);
        Weapons.Add(Pistol);
        GTAWeapon AR = new GTAWeapon(WeaponHash.CarbineRifle, 150, "AR", 3);
        Weapons.Add(AR);
        GTAWeapon MAC11 = new GTAWeapon(WeaponHash.MicroSMG, 120, "SMG", 2);
        Weapons.Add(MAC11);
        GTAWeapon Tech9 = new GTAWeapon(WeaponHash.Smg, 120, "SMG", 2);
        Weapons.Add(Tech9);
        GTAWeapon sawnoff = new GTAWeapon(WeaponHash.SawnOffShotgun, 32, "SHOTGUN", 2);
        Weapons.Add(sawnoff);
        GTAWeapon AK = new GTAWeapon(WeaponHash.AssaultRifle, 120, "AR", 3);
        Weapons.Add(AK);
        GTAWeapon M60 = new GTAWeapon(WeaponHash.MG, 200, "SMG", 4);
        Weapons.Add(M60);
        GTAWeapon SNS2 = new GTAWeapon(WeaponHash.Pistol, 6, "PISTOL", 0);
        Weapons.Add(SNS2);
        GTAWeapon switchblade = new GTAWeapon(WeaponHash.Knife, 0, "MELEE", 0);
        Weapons.Add(switchblade);

        return Weapons.OrderBy(s => rnd.Next()).Where(s => s.WeaponLevel == RequestedLevel).First();
    }
    public static void ResetPoliceStats()
    {
        PoliceScanningSystem.CopsKilledByPlayer = 0;
        PoliceScanningSystem.PlayerHurtPolice = false;
        PoliceScanningSystem.PlayerKilledPolice = false;
        aimedAtPolice = false;
        firedWeapon = false;
        DispatchAudioSystem.ResetReportedItems();
    }
    private static void CreatePassengerCop(GTACop Cop)
    {
        if (Cop.CopPed.CurrentVehicle.IsSeatFree(0))
        {
            Ped CreatedCop = new Ped("s_m_y_cop_01", Cop.CopPed.GetOffsetPosition(new Vector3(0f, -4f, 0f)), Cop.CopPed.Heading);
            CreatedCop.WarpIntoVehicle(Cop.CopPed.CurrentVehicle, 0);
        }
    }



}

