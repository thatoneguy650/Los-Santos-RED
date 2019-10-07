using ExtensionsMethods;
using Instant_Action_RAGE.Systems;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
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
    public static Vehicle OwnedCar = null;

    private static uint WantedLevelStartTime;
    private static bool AnySeenThisWanted = false;
    private static int TimeAimedAtPolice = 0;
    public static List<StolenVehicle> StolenVehicles = new List<StolenVehicle>();
    public static bool PrevPlayerIsJacking = false;


    //traffic
    private static bool ViolationDrivingAgainstTraffic = false;
    private static bool ViolationDrivingOnPavement = false;
    private static bool ViolationRanRedLight = false;
    private static bool IsRunningRedLight = false;
    private static bool PrevIsRunningRedLight = false;

    private static bool CanReportLastSeen;
    private static uint GameTimeLastGreyedOut;
    private static bool PrevPlayerStarsGreyedOut;
    private static bool PlayerStarsGreyedOut;
    private static bool PrevaimedAtPolice;
    private static bool GhostCopFollow;
    private static bool PrevPlayerKilledPolice = false;
    private static int PrevCopsKilledByPlayer = 0;
    public static bool ReportedStolenVehicle;
    private static bool JackedCurrentVehicle;
    private static bool ViolationHitPed = false;
    private static bool ViolationHitVehicle = false;
    private static uint GameTimeInterval;
    public static bool PlayerInVehicle = false;
    private static bool ViolationsSpeeding = false;
    private static int PrevTimeSincePlayerLastHitAnyVehicle;
    private static int PrevTimeSincePlayerLastHitAnyPed;

    private static bool IsRunning { get; set; } = true;
    public static PoliceState CurrentPoliceState { get; set; }
    public static bool PrevPlayerInVehicle { get; private set; }

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

        while (Game.IsLoading)
            GameFiber.Yield();
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
            PoliceSpeechSystem.Initialize();
            while (IsRunning)
            {
                StateTick();
                ControlTick();
                PoliceTick();
                DebugLoop();
                TestLoop();

                if (Game.GameTime > GameTimeInterval + 500)
                {
                    TrafficViolationsTick();
                    GameTimeInterval = Game.GameTime;
                }
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
        NativeFunction.Natives.xB9EFD5C25018725A("DISPLAY_HUD", true);
        //DISPLAY_HUD
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



        //Vehicle[] Vehicles = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 50f, GetEntitiesFlags.ConsiderAllVehicles | GetEntitiesFlags.ExcludePlayerVehicle).Where(x => x is Vehicle).ToArray(), (x => (Vehicle)x));
        //List<TrafficVehicle> TrafficVehicles = new List<TrafficVehicle>();

        //foreach (Vehicle vehicle in Vehicles)
        //{
        //    float Result = Extensions.getDotVectorResult(Game.LocalPlayer.Character.CurrentVehicle, vehicle);
        //    bool StoppedatTraffic = NativeFunction.CallByName<bool>("IS_VEHICLE_STOPPED_AT_TRAFFIC_LIGHTS", vehicle);
        //    TrafficVehicles.Add(new TrafficVehicle(vehicle, StoppedatTraffic, Result));

        //    //Extensions.getDotVectorResult(Game.LocalPlayer.Character.CurrentVehicle, vehicle);


        //    //Color VehicleColor = Color.Yellow;
        //    //if (NativeFunction.CallByName<bool>("IS_VEHICLE_STOPPED_AT_TRAFFIC_LIGHTS", vehicle))
        //    //{
        //    //    VehicleColor = Color.Red;
        //    //    if (vehicle.PlayerVehicleIsInFront())
        //    //    {
        //    //        IsRunningRedLight = true;
        //    //        VehicleColor = Color.Black;
        //    //        break;
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    IsRunningRedLight = false;
        //    //    VehicleColor = Color.Green;
        //    //}


            
        //}

        //foreach (TrafficVehicle tv in TrafficVehicles)
        //{
        //    Color VehicleColor;
            
        //    if (tv.DotProductResult > 0)
        //        VehicleColor = Color.Black;
        //    else if (tv.DotProductResult < 0)
        //        VehicleColor = Color.Blue;
        //    //else if (tv.WaitingAtRedLight)
        //    //    VehicleColor = Color.Red;
        //    else
        //        VehicleColor = Color.Yellow;
        //    Debug.DrawArrowDebug(new Vector3(tv.VehicleEntity.Position.X, tv.VehicleEntity.Position.Y, tv.VehicleEntity.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, VehicleColor);
        //}




        //if (PrevIsRunningRedLight != IsRunningRedLight)
        //{
        //    WriteToLog("IsRunningRedLight", string.Format("IsRunningRedLight: {0}", IsRunningRedLight));
        //    PrevIsRunningRedLight = IsRunningRedLight;
        //}












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
            //try
            //{
            WriteToLog("Local Stuff", string.Format("Model {0}", Game.LocalPlayer.Character.CurrentVehicle.Model.Name));
            WriteToLog("Local Stuff", string.Format("Color {0}", Game.LocalPlayer.Character.CurrentVehicle.PrimaryColor.Name));

            //Rage.Graphics Device = args.Graphics;

            //Device.DrawRectangle(new RectangleF(GetMappedPoint("Base"), GetMappedSize("Base")), Color.FromArgb(200, 40, 40, 40));

            //Device.DrawText(Name, "Arial", 17.0F, GetMappedPoint("Name"), Color.FromArgb(255, 255, 255));


            Color ClosestColor = DispatchAudioSystem.GetBaseColor(Game.LocalPlayer.Character.CurrentVehicle.PrimaryColor);

            WriteToLog("Local Stuff", string.Format("ClosestColor {0}", ClosestColor));

            var output = Regex.Replace(Game.LocalPlayer.Character.CurrentVehicle.Model.Name.ToUpper(), @"[\d-]", string.Empty);
            DispatchAudioSystem.VehicleModelNameLookup LookupModel = DispatchAudioSystem.ModelNameLookup.Where(x => x.ModelName.Contains(output)).PickRandom();
            DispatchAudioSystem.ColorLookup LookupColor = DispatchAudioSystem.ColorLookups.Where(x => x.BaseColor == ClosestColor).PickRandom();

            if (LookupModel != null)
            {
                WriteToLog("Car stuff", string.Format("Lookup Model {0}, ScannerFile {1}", LookupModel.ModelName, LookupModel.ScannerFile));
            }

            if (LookupColor != null)
            {
                WriteToLog("Car stuff", string.Format("Lookup Color {0}, ScannerFile {1}", LookupColor.BaseColor, LookupColor.ScannerFile));
            }

            float SpeedLimit = GetSpeedLimit();
                string street = GetCurrentStreet();
            float VehicleSpeedMPH = Game.LocalPlayer.Character.CurrentVehicle.Speed * 2.23694f;
            WriteToLog("Car stuff", string.Format("StreetName {0}, Speed Limit {1}, CurrentSpeed MPH {2}", street, SpeedLimit, VehicleSpeedMPH));

            //Game.HandleRespawn();


            //Vehicle[] Vehicles2 = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 25f, GetEntitiesFlags.ConsiderAllVehicles | GetEntitiesFlags.ExcludePlayerVehicle).Where(x => x is Vehicle).ToArray(), (x => (Vehicle)x));
            //List<TrafficVehicle> TrafficVehicles2 = new List<TrafficVehicle>();

            //foreach (Vehicle vehicle in Vehicles2)
            //{
            //    float Result = Extensions.getDotVectorResult(Game.LocalPlayer.Character.CurrentVehicle, vehicle);
            //    bool StoppedatTraffic = NativeFunction.CallByName<bool>("IS_VEHICLE_STOPPED_AT_TRAFFIC_LIGHTS", vehicle);
            //    TrafficVehicles2.Add(new TrafficVehicle(vehicle, StoppedatTraffic, Result));

            //    //Extensions.getDotVectorResult(Game.LocalPlayer.Character.CurrentVehicle, vehicle);


            //    //Color VehicleColor = Color.Yellow;
            //    //if (NativeFunction.CallByName<bool>("IS_VEHICLE_STOPPED_AT_TRAFFIC_LIGHTS", vehicle))
            //    //{
            //    //    VehicleColor = Color.Red;
            //    //    if (vehicle.PlayerVehicleIsInFront())
            //    //    {
            //    //        IsRunningRedLight = true;
            //    //        VehicleColor = Color.Black;
            //    //        break;
            //    //    }
            //    //}
            //    //else
            //    //{
            //    //    IsRunningRedLight = false;
            //    //    VehicleColor = Color.Green;
            //    //}


                
            //}

            //foreach (TrafficVehicle TV in TrafficVehicles2)
            //{

            //    WriteToLog("Car stuff", string.Format("Handle {0}, Stopped {1}, DotResult {2}, Distance To: {3}", TV.VehicleEntity.Handle, TV.WaitingAtRedLight, TV.DotProductResult,Game.LocalPlayer.Character.DistanceTo(TV.VehicleEntity.Position)));
            //}






            //Vehicle[] Vehicles = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 25f, GetEntitiesFlags.ConsiderAllVehicles | GetEntitiesFlags.ExcludePlayerVehicle).Where(x => x is Vehicle).ToArray(), (x => (Vehicle)x));
            //foreach (Vehicle vehicle in Vehicles)
            //{
            //    if (NativeFunction.CallByName<bool>("IS_VEHICLE_STOPPED_AT_TRAFFIC_LIGHTS", vehicle))
            //    {
            //        if (vehicle.PlayerVehicleIsBehind())
            //        {
            //            WriteToLog("Behind", string.Format("You are behind {0}, they are stopped at a traffic light, they are facing: {1}", vehicle.Handle, vehicle.Heading));
            //        }
            //    }
            //    else
            //    {
            //        WriteToLog("Behind", string.Format("Other {0}, Not Stopped, they are facing: {1}", vehicle.Handle, vehicle.Heading));
            //    }
            //}





            //WriteToLog("KeyDown", string.Format("ModelName: {0}", Game.LocalPlayer.Character.CurrentVehicle.Model.Name));
            //WriteToLog("KeyDown", string.Format("SPeed: {0}", Game.LocalPlayer.Character.CurrentVehicle.Speed));

            //RequestAnimationDictionay("veh@busted_std");
            //RequestAnimationDictionay("busted");

            //Vehicle oldVehicle = Game.LocalPlayer.Character.CurrentVehicle;
            //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "veh@busted_std", "get_out_car_crim", 2.0f, -2.0f, 2500, 50, 0, false, false, false);
            //GameFiber.Wait(2500);

            //NativeFunction.CallByName<uint>("TASK_LEAVE_VEHICLE", Game.LocalPlayer.Character, oldVehicle, 256);

            // SetArrestedAnimation(Game.LocalPlayer.Character, false);



            //MoveGhostCopToPlayer();
            //Game.LocalPlayer.Character.GiveCash(5000, "Michael");
            //MoveGhostCopToPlayer();
            //}
            //catch (Exception e)
            //{
            //    WriteToLog("Car stuff", e.Message);
            //}
        }
        if (Game.IsKeyDown(Keys.NumPad6))
        {
            try
            {
                //WriteToLog("KeyDown", string.Format("ModelName: {0}", Game.LocalPlayer.Character.CurrentVehicle.Model.Name));
                //WriteToLog("KeyDown", string.Format("SPeed: {0}", Game.LocalPlayer.Character.CurrentVehicle.Speed));
              ResetTrafficViolations();


                Game.LocalPlayer.Character.GiveCash(5000, "Michael");
                //Game.HandleRespawn();
                //   DispatchAudioSystem.ReportVehicleHitAndRun(Game.LocalPlayer.Character.CurrentVehicle);
                //UnSetArrestedAnimation(Game.LocalPlayer.Character);


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

        if (TimeAimedAtPolice >= 100 && Game.LocalPlayer.WantedLevel < 3)
        {
            WriteToLog("AimedAt Police", "");
            Game.LocalPlayer.WantedLevel = 3;
            aimedAtPolice = true;
        }

        if (!firedWeapon && Game.LocalPlayer.Character.IsShooting && (PoliceScanningSystem.CopPeds.Any(x => x.canSeePlayer || (x.DistanceToPlayer <= 100f && !Game.LocalPlayer.Character.IsCurrentWeaponSilenced)))) //if (!firedWeapon && Game.LocalPlayer.Character.IsShooting && (PoliceScanningSystem.CopPeds.Any(x => x.canSeePlayer || x.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 100f))))
        {
            Game.LocalPlayer.WantedLevel = 2;
            firedWeapon = true;
            WriteToLog("Fired weapon", "");
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
        bool AnyCanSee = PoliceScanningSystem.CopPeds.Any(x => x.canSeePlayer);
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
            //Cop.CopPed.VisionRange = 100f;

            //NativeFunction.CallByName<bool>("SET_DRIVER_ABILITY", Cop.CopPed, 100f);
            //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", Cop.CopPed, 8f);
            //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 32, true);


            //NativeFunction.CallByName<bool>("SET_DRIVER_ABILITY", Cop.CopPed, 100f);

            //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 4, true);
            //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 8, true);
            // NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 16, true);
            ////NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 32, true);
            //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 512, true);
            //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 262144, true);
            // NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", Cop.CopPed, 8f);

            if (CurrentPoliceState == PoliceState.DeadlyChase)
            {
                NativeFunction.CallByName<bool>("SET_DRIVER_ABILITY", Cop.CopPed, 100f);

                NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 4, true);
                NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 8, true);
                NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 16, true);
                //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 32, true);
                NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 512, true);
                NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 262144, true);
                NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", Cop.CopPed, 8f);



                //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 262144, true);
                ////CreatePassengerCop(Cop);
                //NativeFunction.CallByName<bool>("SET_DRIVER_AGGRESSIVENESS", Cop.CopPed, 100f);
                ////NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", Cop.CopPed, 0f);
            }
            else
            {
                NativeFunction.CallByName<bool>("SET_DRIVER_ABILITY", Cop.CopPed, 100f);
                NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", Cop.CopPed, 8f);
                NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 32, true);


                //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 32, true);
                //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", Cop.CopPed, 8f);
            }
        }

        //if (PrevPoliceState != CurrentPoliceState)
        //    PoliceStateChanged();

        //if(Game.LocalPlayer.Character.IsJacking)
        //{
        //    Vehicle TryingToEnter = Game.LocalPlayer.Character.VehicleTryingToEnter;
        //    if (TryingToEnter != null)
        //    {
        //        StolenVehicles.Add(new StolenVehicle(TryingToEnter, Game.GameTime, true));
        //        LastStolenVehicle = TryingToEnter;
        //        ReportedStolenVehicle = false;
        //    }
        //}


        //if(!ReportedStolenVehicle && ReportStolenVehicle && PlayerInStolenVehicle && Game.GameTime - GameTimeLastStolenVehicle >= 30000)
        //{
        //    LastStolenVehicle.IsStolen = true;
        //    DispatchAudioSystem.ReportStolenVehicle(LastStolenVehicle);
        //}

        StolenVehicles.RemoveAll(x => !x.VehicleEnt.Exists());
        if (Game.LocalPlayer.WantedLevel == 0)
        {
            foreach (StolenVehicle StolenCar in StolenVehicles)
            {
                if (StolenCar.ShouldReportStolen)
                {
                    WriteToLog("StolenVehicles", "ReportStolenVehicle");
                    DispatchAudioSystem.ReportStolenVehicle(StolenCar);
                }
            }
        }

        if (PlayerInVehicle && AnyCanSee)
        {
            if (StolenVehicles.Any(x => x.WasReportedStolen && x.VehicleEnt.Handle == Game.LocalPlayer.Character.CurrentVehicle.Handle) && Game.LocalPlayer.WantedLevel < 2)
            {
                Game.LocalPlayer.WantedLevel = 2;
                DispatchAudioSystem.ReportSpottedStolenCar(Game.LocalPlayer.Character.CurrentVehicle);
                WriteToLog("StolenVehicles", "Caught In Stolen Vehicle");
            }
            if (Game.LocalPlayer.WantedLevel > 0)
            {
                foreach (StolenVehicle stolenVehicle in StolenVehicles.Where(x => !x.WasReportedStolen))
                {
                    stolenVehicle.WasReportedStolen = true;
                    WriteToLog("StolenVehicles", "Cops Saw your stolen vehicle already on chase, no need to report it");
                }
            }
        }



        bool isJacking = Game.LocalPlayer.Character.IsJacking;
        if (PrevPlayerIsJacking != isJacking)
            PlayerJackingChanged(isJacking);


        if (PrevPlayerInVehicle != PlayerInVehicle)
            PlayerInVehicleChanged(PlayerInVehicle);

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

        if (PrevPoliceState != CurrentPoliceState)
            PoliceStateChanged();

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

        if (AnyRecentlySeen)
        {
            NativeFunction.CallByName<bool>("SET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer, Game.LocalPlayer.Character.Position.X, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z);
        }
    }
    private static void TrafficViolationsTick()
    {
        if (CurrentPoliceState != PoliceState.Normal)
            return;
        bool AnyCanSeePlayer = PoliceScanningSystem.CopPeds.Any(x => x.canSeePlayer && x.isInVehicle && !x.isInHelicopter);
        PlayerInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);

        if (PlayerInVehicle)
        {
            float VehicleSpeedMPH = Game.LocalPlayer.Character.CurrentVehicle.Speed * 2.23694f;
            Vehicle CurrVehicle = Game.LocalPlayer.Character.CurrentVehicle;
            if (AnyCanSeePlayer && !ViolationDrivingAgainstTraffic && Game.LocalPlayer.IsDrivingAgainstTraffic)
            {
                ViolationDrivingAgainstTraffic = true;
                Game.LocalPlayer.WantedLevel = 1;
                DispatchAudioSystem.ReportRecklessDriver(CurrVehicle);
                WriteToLog("TrafficViolationsTick", string.Format("ViolationDrivingAgainstTraffic: {0}", ViolationDrivingAgainstTraffic));
            }
            if (AnyCanSeePlayer && !ViolationDrivingOnPavement && Game.LocalPlayer.IsDrivingOnPavement)
            {
                ViolationDrivingOnPavement = true;
                Game.LocalPlayer.WantedLevel = 1;
                DispatchAudioSystem.ReportRecklessDriver(CurrVehicle);
                WriteToLog("TrafficViolationsTick", string.Format("ViolationDrivingOnPavement: {0}", ViolationDrivingOnPavement));
            }

            float SpeedLimit  = GetSpeedLimit();
            if (AnyCanSeePlayer && AnyCanSeePlayer && !ViolationsSpeeding && VehicleSpeedMPH > SpeedLimit + 20)
            {
                ViolationsSpeeding = true;
                Game.LocalPlayer.WantedLevel = 1;
                DispatchAudioSystem.ReportFelonySpeeding(CurrVehicle);
                WriteToLog("TrafficViolationsTick", string.Format("ViolationsSpeeding: {0}", ViolationsSpeeding));
            }
            int TimeSincePlayerLastHitAnyPed = Game.LocalPlayer.TimeSincePlayerLastHitAnyPed;
            if (AnyCanSeePlayer && !ViolationHitPed && TimeSincePlayerLastHitAnyPed > -1 && TimeSincePlayerLastHitAnyPed <= 1000)
            {
                ViolationHitPed = true;
                Game.LocalPlayer.WantedLevel = 2;
                DispatchAudioSystem.ReportPedHitAndRun(CurrVehicle);
                WriteToLog("TrafficViolationsTick", string.Format("ViolationHitPed: {0}", ViolationHitPed));
            }
            int TimeSincePlayerLastHitAnyVehicle = Game.LocalPlayer.TimeSincePlayerLastHitAnyVehicle;
            if (AnyCanSeePlayer && !ViolationHitVehicle && TimeSincePlayerLastHitAnyVehicle > -1 && TimeSincePlayerLastHitAnyVehicle <= 1000)
            {
                ViolationHitVehicle = true;
                Game.LocalPlayer.WantedLevel = 1;
                DispatchAudioSystem.ReportVehicleHitAndRun(CurrVehicle);
                WriteToLog("TrafficViolationsTick", string.Format("ViolationHitVehicle: {0}", ViolationHitVehicle));
            }

            //if(RunningRedLight())
            //{
            //    WriteToLog("TrafficViolationsTick", string.Format("Running Red light: {0}", ViolationHitVehicle));
            //}



        }
    }

    private static bool RunningRedLight()
    {

        Vehicle[] Vehicles = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 25f, GetEntitiesFlags.ConsiderAllVehicles | GetEntitiesFlags.ExcludePlayerVehicle).Where(x => x is Vehicle).ToArray(), (x => (Vehicle)x));
        foreach(Vehicle vehicle in Vehicles)
        {
            if(NativeFunction.CallByName<bool>("IS_VEHICLE_STOPPED_AT_TRAFFIC_LIGHTS", vehicle))
            {
                if(vehicle.PlayerVehicleIsBehind())
                {
                    return true;
                }
            }
        }
        return false;



        //this.vehs = World.GetNearbyVehicles(((Entity)Game.get_Player().get_Character()).get_Position(), 9f);
        //if (this.vehs.Length == 0)
        //    return false;
        //for (int index = 0; index < this.vehs.Length; ++index)
        //{
        //    if (this.autolista.Count == 0)
        //        this.autolista.Add(this.vehs[index]);
        //    else if (!this.autolista.Contains(this.vehs[index]))
        //        this.autolista.Add(this.vehs[index]);
        //}
        //if (this.autolista.Count == 0)
        //    return false;
        //for (int index = 0; index < this.autolista.Count; ++index)
        //{
        //    if ((double)Vector3.Distance(((Entity)this.autolista[index]).get_Position(), ((Entity)Game.get_Player().get_Character()).get_Position()) > 35.0)
        //        this.autolista.Remove(this.autolista[index]);
        //}
        //for (int index = 0; index < this.autolista.Count; ++index)
        //{
        //    if (Function.Call<bool>((Hash)2979683755510794905L, new InputArgument[1]
        //    {
        //  InputArgument.op_Implicit(this.autolista[index])
        //    }) != 0)
        //    {
        //        float num = Vector3.Angle(((Entity)this.autolista[index]).get_ForwardVector(), ((Entity)Game.get_Player().get_Character()).get_ForwardVector());
        //        if ((double)num <= 35.0 && ((double)num <= 35.0 && (double)Vector3.Distance(((Entity)this.autolista[index]).get_Position(), ((Entity)Game.get_Player().get_Character()).get_Position()) > 30.0 && (double)Vector3.Angle(Vector3.op_Subtraction(((Entity)this.autolista[index]).get_Position(), ((Entity)Game.get_Player().get_Character()).get_Position()), ((Entity)Game.get_Player().get_Character()).get_ForwardVector()) > 150.0))
        //            return true;
        //    }
        //}
        //return false;
    }
    private static void ResetTrafficViolations()
    {
        ViolationDrivingOnPavement = false;
        ViolationDrivingAgainstTraffic = false;
        ViolationHitPed = false;
        ViolationsSpeeding = false;
        ViolationHitVehicle = false;
    }
    private static void PullOver(GTACop Cop, Ped PullOverTarget)
    {

    }


    private static void PoliceTickNormal()
    {
        foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => x.isTasked && !x.TaskIsQueued))
        {
            Cop.TaskIsQueued = true;
            PoliceScanningSystem.CopsToTask.Add(new PoliceTask(Cop, PoliceTask.Task.Untask));
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
                PoliceScanningSystem.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Chase));
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
        //foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => x.isTasked)) // Exist/Dead Check
        //{
        //    if (Cop.SetTazer)
        //        SetCopTazer(Cop);
        //}
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
            if (AnySeenThisWanted && MaxWantedLastLife > 0)
                DispatchAudioSystem.ReportSuspectLost();
            CurrentPoliceState = PoliceState.Normal;
            AnySeenThisWanted = false;
            ResetTrafficViolations();

            DispatchAudioSystem.ResetReportedItems();
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
        if (PoliceScanningSystem.PlayerHurtPolice)
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
    private static void PlayerInVehicleChanged(bool playerInVehicle)
    {
        WriteToLog("ValueChecker", String.Format("playerInVehicle Changed to: {0}", playerInVehicle));
        if (playerInVehicle)
        {

            Vehicle CurrVehicle = Game.LocalPlayer.Character.CurrentVehicle;

            if (!StolenVehicles.Any(x => x.VehicleEnt.Handle == CurrVehicle.Handle))
            {
                if (OwnedCar != null && OwnedCar.Handle == CurrVehicle.Handle)
                {

                }
                else
                {
                    StolenVehicles.Add(new StolenVehicle(CurrVehicle, Game.GameTime, JackedCurrentVehicle, CurrVehicle.IsAlarmSounding,CurrVehicle.GetPreviousPedOnSeat(-1)));
                }
            }
        }
        else
        {
            JackedCurrentVehicle = false;
        }
        PlayerInVehicle = playerInVehicle;
        PrevPlayerInVehicle = playerInVehicle;
    }
    private static void PlayerJackingChanged(bool isJacking)
    {
        if (isJacking)
        {
            JackedCurrentVehicle = true;
        }
        PrevPlayerIsJacking = isJacking;
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
            if (PrevPoliceState != PoliceState.ArrestedWait)
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
        if (Game.LocalPlayer.Character.IsInAnyVehicle(false))
            return;
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
            if (GhostCop != null)
                GhostCop.Position = new Vector3(0f, 0f, 0f);
            GhostCopFollow = false;
        }


        if (NativeFunction.CallByName<bool>("ARE_PLAYER_STARS_GREYED_OUT", Game.LocalPlayer) && !PoliceScanningSystem.CopPeds.Any(x => x.RecentlySeenPlayer()) && PoliceScanningSystem.CopPeds.Any(x => x.isTasked && !x.TaskIsQueued))
        {
            PoliceScanningSystem.UntaskAll();
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

        NativeFunction.CallByName<uint>("SET_PED_CONFIG_FLAG", GhostCop, 69, true);

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

        if ((Cop.CopPed.Inventory.EquippedWeapon == null || Cop.CopPed.Inventory.EquippedWeapon.Hash == WeaponHash.StunGun) && Game.LocalPlayer.WantedLevel >= 0)
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
        if (Game.LocalPlayer.WantedLevel > 0)
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
                OwnedCar = carWasIn;
            }
            else
            {
                Game.LocalPlayer.Character.IsCollisionEnabled = true;
            }

            Game.LocalPlayer.Character.SetCash(rnd.Next(500, 4000), "Michael");


            Game.LocalPlayer.Character.Inventory.Weapons.Clear();
            Game.LocalPlayer.Character.Inventory.GiveNewWeapon(2725352035, 0, true);
            TimesDied = 0;
            MaxWantedLastLife = 0;
            Game.TimeScale = 1f;
            Game.LocalPlayer.WantedLevel = 0;
            NativeFunction.Natives.xB4EDDC19532BFB85();
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
            for (int PropNumber = 0; PropNumber < 8; PropNumber++)
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
                NativeFunction.CallByName<uint>("SET_PED_PROP_INDEX", myPed, Prop.PropID, Prop.DrawableID, Prop.TextureID, false);
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
            RequestAnimationDictionay("ped");

            if (!PedToArrest.Exists())
                return;

            while (PedToArrest.IsRagdoll || PedToArrest.IsStunned)
                GameFiber.Yield();

            if (!PedToArrest.Exists())
                return;

            if (PedToArrest.IsInAnyVehicle(false))
            {
                Vehicle oldVehicle = PedToArrest.CurrentVehicle;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "veh@busted_std", "get_out_car_crim", 2.0f, -2.0f, 2500, 50, 0, false, false, false);
                GameFiber.Sleep(2500);
                if (PedToArrest.Exists() && oldVehicle.Exists())
                    NativeFunction.CallByName<uint>("TASK_LEAVE_VEHICLE", PedToArrest, oldVehicle, 256);
            }
            if (PedToArrest == Game.LocalPlayer.Character && !isBusted)
                return;


            if (MaxWantedLastLife < 3)
            {
                NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "ped", "handsup_enter", 2.0f, -2.0f, -1, 2, 0, false, false, false);
            }
            else
            {
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "busted", "idle_2_hands_up", 2.0f, -8.0f, 5000, 2, 0, false, false, false);
                GameFiber.Sleep(5000);
                if (!PedToArrest.Exists() || (PedToArrest == Game.LocalPlayer.Character && !isBusted))
                    return;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "busted", "idle_a", 8.0f, -8.0f, -1, 1, 0, false, false, false);
            }
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
            RequestAnimationDictionay("ped");

            if (NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_a", 1) || NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_2_hands_up", 1))
            {
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "random@arrests", "kneeling_arrest_escape", 8.0f, -8.0f, -1, 4096, 0, 0, 1, 0);
            }
            else if(NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "ped", "handsup_enter", 1))
            {
                PedToArrest.Tasks.Clear();
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
        {
            GameFiber TaskFiber =
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
        if (Game.LocalPlayer.Character.LastVehicle.Exists())
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

    private static string GetCurrentStreet()
    {
        //string[] streets = new string[1];

        Vector3 PlayerPos = Game.LocalPlayer.Character.Position;

        int StreetHash = 0;
        int CrossingHash = 0;
        unsafe
        {
            NativeFunction.CallByName<uint>("GET_STREET_NAME_AT_COORD", PlayerPos.X, PlayerPos.Y, PlayerPos.Z, &StreetHash, &CrossingHash);
        }

        string StreetName = string.Empty;
        string CrossStreetName = string.Empty;
        Vector3 Position = Game.LocalPlayer.Character.Position;
        if (StreetHash != 0)
        {
            unsafe
            {
                IntPtr ptr = Rage.Native.NativeFunction.CallByName<IntPtr>("GET_STREET_NAME_FROM_HASH_KEY", StreetHash);

                StreetName = Marshal.PtrToStringAnsi(ptr);
            }
        }

        //if (CrossingHash != 0)
        //{
        //    unsafe
        //    {
        //        IntPtr ptr = Rage.Native.NativeFunction.CallByName<IntPtr>("GET_STREET_NAME_FROM_HASH_KEY", CrossingHash);

        //        CrossStreetName = Marshal.PtrToStringAnsi(ptr);
        //    }
        //}
        //WriteToLog("GetSpeedLimit", string.Format("Street Name: {0}, Cross Street Name: {`}", StreetName, CrossStreetName));
        //streets[0] = StreetName;
        //streets[1] = CrossStreetName;
        //return streets;
        return StreetName;
    }
    private static float GetSpeedLimit()
    {
        string StreetName = GetCurrentStreet();

        int SpeedLimit;

        if (StreetName == "Joshua Rd")
            SpeedLimit = 50;
        else if (StreetName == "East Joshua Road")
            SpeedLimit = 50;
        else if (StreetName == "Marina Dr")
            SpeedLimit = 35;
        else if (StreetName == "Alhambra Dr")
            SpeedLimit = 35;
        else if (StreetName == "Niland Ave")
            SpeedLimit = 35;
        else if (StreetName == "Zancudo Ave")
            SpeedLimit = 35;
        else if (StreetName == "Armadillo Ave")
            SpeedLimit = 35;
        else if (StreetName == "Algonquin Blvd")
            SpeedLimit = 35;
        else if (StreetName == "Mountain View Dr")
            SpeedLimit = 35;
        else if (StreetName == "Cholla Springs Ave")
            SpeedLimit = 35;
        else if (StreetName == "Panorama Dr")
            SpeedLimit = 40;
        else if (StreetName == "Lesbos Ln")
            SpeedLimit = 35;
        else if (StreetName == "Calafia Rd")
            SpeedLimit = 30;
        else if (StreetName == "North Calafia Way")
            SpeedLimit = 30;
        else if (StreetName == "Cassidy Trail")
            SpeedLimit = 25;
        else if (StreetName == "Seaview Rd")
            SpeedLimit = 35;
        else if (StreetName == "Grapeseed Main St")
            SpeedLimit = 35;
        else if (StreetName == "Grapeseed Ave")
            SpeedLimit = 35;
        else if (StreetName == "Joad Ln")
            SpeedLimit = 35;
        else if (StreetName == "Union Rd")
            SpeedLimit = 40;
        else if (StreetName == "O'Neil Way")
            SpeedLimit = 25;
        else if (StreetName == "Senora Fwy")
            SpeedLimit = 65;
        else if (StreetName == "Catfish View")
            SpeedLimit = 35;
        else if (StreetName == "Great Ocean Hwy")
            SpeedLimit = 60;
        else if (StreetName == "Paleto Blvd")
            SpeedLimit = 35;
        else if (StreetName == "Duluoz Ave")
            SpeedLimit = 35;
        else if (StreetName == "Procopio Dr")
            SpeedLimit = 35;
        else if (StreetName == "Cascabel Ave")
            SpeedLimit = 30;
        else if (StreetName == "Procopio Promenade")
            SpeedLimit = 25;
        else if (StreetName == "Pyrite Ave")
            SpeedLimit = 30;
        else if (StreetName == "Fort Zancudo Approach Rd")
            SpeedLimit = 25;
        else if (StreetName == "Barbareno Rd")
            SpeedLimit = 30;
        else if (StreetName == "Ineseno Road")
            SpeedLimit = 30;
        else if (StreetName == "West Eclipse Blvd")
            SpeedLimit = 35;
        else if (StreetName == "Playa Vista")
            SpeedLimit = 30;
        else if (StreetName == "Bay City Ave")
            SpeedLimit = 30;
        else if (StreetName == "Del Perro Fwy")
            SpeedLimit = 65;
        else if (StreetName == "Equality Way")
            SpeedLimit = 30;
        else if (StreetName == "Red Desert Ave")
            SpeedLimit = 30;
        else if (StreetName == "Magellan Ave")
            SpeedLimit = 25;
        else if (StreetName == "Sandcastle Way")
            SpeedLimit = 30;
        else if (StreetName == "Vespucci Blvd")
            SpeedLimit = 40;
        else if (StreetName == "Prosperity St")
            SpeedLimit = 30;
        else if (StreetName == "San Andreas Ave")
            SpeedLimit = 40;
        else if (StreetName == "North Rockford Dr")
            SpeedLimit = 35;
        else if (StreetName == "South Rockford Dr")
            SpeedLimit = 35;
        else if (StreetName == "Marathon Ave")
            SpeedLimit = 30;
        else if (StreetName == "Boulevard Del Perro")
            SpeedLimit = 35;
        else if (StreetName == "Cougar Ave")
            SpeedLimit = 30;
        else if (StreetName == "Liberty St")
            SpeedLimit = 30;
        else if (StreetName == "Bay City Incline")
            SpeedLimit = 40;
        else if (StreetName == "Conquistador St")
            SpeedLimit = 25;
        else if (StreetName == "Cortes St")
            SpeedLimit = 25;
        else if (StreetName == "Vitus St")
            SpeedLimit = 25;
        else if (StreetName == "Aguja St")
            SpeedLimit = 25;
        else if (StreetName == "Goma St")
            SpeedLimit = 25;
        else if (StreetName == "Melanoma St")
            SpeedLimit = 25;
        else if (StreetName == "Palomino Ave")
            SpeedLimit = 35;
        else if (StreetName == "Invention Ct")
            SpeedLimit = 25;
        else if (StreetName == "Imagination Ct")
            SpeedLimit = 25;
        else if (StreetName == "Rub St")
            SpeedLimit = 25;
        else if (StreetName == "Tug St")
            SpeedLimit = 25;
        else if (StreetName == "Ginger St")
            SpeedLimit = 30;
        else if (StreetName == "Lindsay Circus")
            SpeedLimit = 30;
        else if (StreetName == "Calais Ave")
            SpeedLimit = 35;
        else if (StreetName == "Adam's Apple Blvd")
            SpeedLimit = 40;
        else if (StreetName == "Alta St")
            SpeedLimit = 40;
        else if (StreetName == "Integrity Way")
            SpeedLimit = 30;
        else if (StreetName == "Swiss St")
            SpeedLimit = 30;
        else if (StreetName == "Strawberry Ave")
            SpeedLimit = 40;
        else if (StreetName == "Capital Blvd")
            SpeedLimit = 30;
        else if (StreetName == "Crusade Rd")
            SpeedLimit = 30;
        else if (StreetName == "Innocence Blvd")
            SpeedLimit = 40;
        else if (StreetName == "Davis Ave")
            SpeedLimit = 40;
        else if (StreetName == "Little Bighorn Ave")
            SpeedLimit = 35;
        else if (StreetName == "Roy Lowenstein Blvd")
            SpeedLimit = 35;
        else if (StreetName == "Jamestown St")
            SpeedLimit = 30;
        else if (StreetName == "Carson Ave")
            SpeedLimit = 35;
        else if (StreetName == "Grove St")
            SpeedLimit = 30;
        else if (StreetName == "Brouge Ave")
            SpeedLimit = 30;
        else if (StreetName == "Covenant Ave")
            SpeedLimit = 30;
        else if (StreetName == "Dutch London St")
            SpeedLimit = 40;
        else if (StreetName == "Signal St")
            SpeedLimit = 30;
        else if (StreetName == "Elysian Fields Fwy")
            SpeedLimit = 50;
        else if (StreetName == "Plaice Pl")
            SpeedLimit = 30;
        else if (StreetName == "Chum St")
            SpeedLimit = 40;
        else if (StreetName == "Chupacabra St")
            SpeedLimit = 30;
        else if (StreetName == "Miriam Turner Overpass")
            SpeedLimit = 30;
        else if (StreetName == "Autopia Pkwy")
            SpeedLimit = 35;
        else if (StreetName == "Exceptionalists Way")
            SpeedLimit = 35;
        else if (StreetName == "La Puerta Fwy")
            SpeedLimit = 60;
        else if (StreetName == "New Empire Way")
            SpeedLimit = 30;
        else if (StreetName == "Runway1")
            SpeedLimit = 90;
        else if (StreetName == "Greenwich Pkwy")
            SpeedLimit = 35;
        else if (StreetName == "Kortz Dr")
            SpeedLimit = 30;
        else if (StreetName == "Banham Canyon Dr")
            SpeedLimit = 40;
        else if (StreetName == "Buen Vino Rd")
            SpeedLimit = 40;
        else if (StreetName == "Route 68")
            SpeedLimit = 55;
        else if (StreetName == "Zancudo Grande Valley")
            SpeedLimit = 40;
        else if (StreetName == "Zancudo Barranca")
            SpeedLimit = 40;
        else if (StreetName == "Galileo Rd")
            SpeedLimit = 40;
        else if (StreetName == "Mt Vinewood Dr")
            SpeedLimit = 40;
        else if (StreetName == "Marlowe Dr")
            SpeedLimit = 40;
        else if (StreetName == "Milton Rd")
            SpeedLimit = 35;
        else if (StreetName == "Kimble Hill Dr")
            SpeedLimit = 35;
        else if (StreetName == "Normandy Dr")
            SpeedLimit = 35;
        else if (StreetName == "Hillcrest Ave")
            SpeedLimit = 35;
        else if (StreetName == "Hillcrest Ridge Access Rd")
            SpeedLimit = 35;
        else if (StreetName == "North Sheldon Ave")
            SpeedLimit = 35;
        else if (StreetName == "Lake Vinewood Dr")
            SpeedLimit = 35;
        else if (StreetName == "Lake Vinewood Est")
            SpeedLimit = 35;
        else if (StreetName == "Baytree Canyon Rd")
            SpeedLimit = 40;
        else if (StreetName == "North Conker Ave")
            SpeedLimit = 35;
        else if (StreetName == "Wild Oats Dr")
            SpeedLimit = 35;
        else if (StreetName == "Whispymound Dr")
            SpeedLimit = 35;
        else if (StreetName == "Didion Dr")
            SpeedLimit = 35;
        else if (StreetName == "Cox Way")
            SpeedLimit = 35;
        else if (StreetName == "Picture Perfect Drive")
            SpeedLimit = 35;
        else if (StreetName == "South Mo Milton Dr")
            SpeedLimit = 35;
        else if (StreetName == "Cockingend Dr")
            SpeedLimit = 35;
        else if (StreetName == "Mad Wayne Thunder Dr")
            SpeedLimit = 35;
        else if (StreetName == "Hangman Ave")
            SpeedLimit = 35;
        else if (StreetName == "Dunstable Ln")
            SpeedLimit = 35;
        else if (StreetName == "Dunstable Dr")
            SpeedLimit = 35;
        else if (StreetName == "Greenwich Way")
            SpeedLimit = 35;
        else if (StreetName == "Greenwich Pl")
            SpeedLimit = 35;
        else if (StreetName == "Hardy Way")
            SpeedLimit = 35;
        else if (StreetName == "Richman St")
            SpeedLimit = 35;
        else if (StreetName == "Ace Jones Dr")
            SpeedLimit = 35;
        else if (StreetName == "Los Santos Freeway")
            SpeedLimit = 65;
        else if (StreetName == "Senora Rd")
            SpeedLimit = 40;
        else if (StreetName == "Nowhere Rd")
            SpeedLimit = 25;
        else if (StreetName == "Smoke Tree Rd")
            SpeedLimit = 35;
        else if (StreetName == "Cholla Rd")
            SpeedLimit = 35;
        else if (StreetName == "Cat-Claw Ave")
            SpeedLimit = 35;
        else if (StreetName == "Senora Way")
            SpeedLimit = 40;
        else if (StreetName == "Palomino Fwy")
            SpeedLimit = 60;
        else if (StreetName == "Shank St")
            SpeedLimit = 25;
        else if (StreetName == "Macdonald St")
            SpeedLimit = 35;
        else if (StreetName == "Route 68 Approach")
            SpeedLimit = 55;
        else if (StreetName == "Vinewood Park Dr")
            SpeedLimit = 35;
        else if (StreetName == "Vinewood Blvd")
            SpeedLimit = 40;
        else if (StreetName == "Mirror Park Blvd")
            SpeedLimit = 35;
        else if (StreetName == "Glory Way")
            SpeedLimit = 35;
        else if (StreetName == "Bridge St")
            SpeedLimit = 35;
        else if (StreetName == "West Mirror Drive")
            SpeedLimit = 35;
        else if (StreetName == "Nikola Ave")
            SpeedLimit = 35;
        else if (StreetName == "East Mirror Dr")
            SpeedLimit = 35;
        else if (StreetName == "Nikola Pl")
            SpeedLimit = 25;
        else if (StreetName == "Mirror Pl")
            SpeedLimit = 35;
        else if (StreetName == "El Rancho Blvd")
            SpeedLimit = 40;
        else if (StreetName == "Olympic Fwy")
            SpeedLimit = 60;
        else if (StreetName == "Fudge Ln")
            SpeedLimit = 25;
        else if (StreetName == "Amarillo Vista")
            SpeedLimit = 25;
        else if (StreetName == "Labor Pl")
            SpeedLimit = 35;
        else if (StreetName == "El Burro Blvd")
            SpeedLimit = 35;
        else if (StreetName == "Sustancia Rd")
            SpeedLimit = 45;
        else if (StreetName == "South Shambles St")
            SpeedLimit = 30;
        else if (StreetName == "Hanger Way")
            SpeedLimit = 30;
        else if (StreetName == "Orchardville Ave")
            SpeedLimit = 30;
        else if (StreetName == "Popular St")
            SpeedLimit = 40;
        else if (StreetName == "Buccaneer Way")
            SpeedLimit = 45;
        else if (StreetName == "Abattoir Ave")
            SpeedLimit = 35;
        else if (StreetName == "Voodoo Place")
            SpeedLimit = 30;
        else if (StreetName == "Mutiny Rd")
            SpeedLimit = 35;
        else if (StreetName == "South Arsenal St")
            SpeedLimit = 35;
        else if (StreetName == "Forum Dr")
            SpeedLimit = 35;
        else if (StreetName == "Morningwood Blvd")
            SpeedLimit = 35;
        else if (StreetName == "Dorset Dr")
            SpeedLimit = 40;
        else if (StreetName == "Caesars Place")
            SpeedLimit = 25;
        else if (StreetName == "Spanish Ave")
            SpeedLimit = 30;
        else if (StreetName == "Portola Dr")
            SpeedLimit = 30;
        else if (StreetName == "Edwood Way")
            SpeedLimit = 25;
        else if (StreetName == "San Vitus Blvd")
            SpeedLimit = 40;
        else if (StreetName == "Eclipse Blvd")
            SpeedLimit = 35;
        else if (StreetName == "Gentry Lane")
            SpeedLimit = 30;
        else if (StreetName == "Las Lagunas Blvd")
            SpeedLimit = 40;
        else if (StreetName == "Power St")
            SpeedLimit = 40;
        else if (StreetName == "Mt Haan Rd")
            SpeedLimit = 40;
        else if (StreetName == "Elgin Ave")
            SpeedLimit = 40;
        else if (StreetName == "Hawick Ave")
            SpeedLimit = 35;
        else if (StreetName == "Meteor St")
            SpeedLimit = 30;
        else if (StreetName == "Alta Pl")
            SpeedLimit = 30;
        else if (StreetName == "Occupation Ave")
            SpeedLimit = 35;
        else if (StreetName == "Carcer Way")
            SpeedLimit = 40;
        else if (StreetName == "Eastbourne Way")
            SpeedLimit = 30;
        else if (StreetName == "Rockford Dr")
            SpeedLimit = 35;
        else if (StreetName == "Abe Milton Pkwy")
            SpeedLimit = 35;
        else if (StreetName == "Laguna Pl")
            SpeedLimit = 30;
        else if (StreetName == "Sinners Passage")
            SpeedLimit = 30;
        else if (StreetName == "Atlee St")
            SpeedLimit = 30;
        else if (StreetName == "Sinner St")
            SpeedLimit = 30;
        else if (StreetName == "Supply St")
            SpeedLimit = 30;
        else if (StreetName == "Amarillo Way")
            SpeedLimit = 35;
        else if (StreetName == "Tower Way")
            SpeedLimit = 35;
        else if (StreetName == "Decker St")
            SpeedLimit = 35;
        else if (StreetName == "Tackle St")
            SpeedLimit = 25;
        else if (StreetName == "Low Power St")
            SpeedLimit = 35;
        else if (StreetName == "Clinton Ave")
            SpeedLimit = 35;
        else if (StreetName == "Fenwell Pl")
            SpeedLimit = 35;
        else if (StreetName == "Utopia Gardens")
            SpeedLimit = 25;
        else if (StreetName == "Cavalry Blvd")
            SpeedLimit = 35;
        else if (StreetName == "South Boulevard Del Perro")
            SpeedLimit = 35;
        else if (StreetName == "Americano Way")
            SpeedLimit = 25;
        else if (StreetName == "Sam Austin Dr")
            SpeedLimit = 25;
        else if (StreetName == "East Galileo Ave")
            SpeedLimit = 35;
        else if (StreetName == "Galileo Park")
            SpeedLimit = 35;
        else if (StreetName == "West Galileo Ave")
            SpeedLimit = 35;
        else if (StreetName == "Tongva Dr")
            SpeedLimit = 40;
        else if (StreetName == "Zancudo Rd")
            SpeedLimit = 35;
        else if (StreetName == "Movie Star Way")
            SpeedLimit = 35;
        else if (StreetName == "Heritage Way")
            SpeedLimit = 35;
        else if (StreetName == "Perth St")
            SpeedLimit = 25;
        else if (StreetName == "Chianski Passage")
            SpeedLimit = 30;
        else if (StreetName == "Lolita Ave")
            SpeedLimit = 35;
        else if (StreetName == "Meringue Ln")
            SpeedLimit = 35;
        else if (StreetName == "Strangeways Dr")
            SpeedLimit = 30;
        else
            SpeedLimit = 50;


        return SpeedLimit;

    }
}