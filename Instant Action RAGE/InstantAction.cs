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
    private static PoliceState PreviousPoliceState;
    public static bool areHandsUp = false;
    private static bool firedWeapon = false;
    private static PoliceState PrevPoliceState = PoliceState.Normal;
    private static bool PrevfiredWeapon = false;
    private static bool PrevPlayerHurtPolice = false;
    private static int PrevWantedLevel = 0;
    public static bool SurrenderBust = false;
    private static uint LastBust;
    private static uint GameTimeLastReTasked;
    private static int ForceSurrenderTime;
    private static Model CopModel = new Model("s_m_y_cop_01");
    private static List<EmergencyLocation> EmergencyLocations = new List<EmergencyLocation>();
    private static Ped GhostCop;
    private static uint GameTimeInterval;

    private static bool WasInVehicle = false;
    private static bool WasInVehicle2 = false;
    private static uint LastSurrender;
    private static bool WasIsSurrenderTimeOut = false;
    private static PoliceState LastPoliceState;

    private static bool IsRunning { get; set; } = true;
    public static PoliceState CurrentPoliceState { get; set; }

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
            



            Menus.Intitialize();
            RespawnSystem.Initialize();
            PoliceScanningSystem.Initialize();

            //DispatchAudioSystem.Initialize();
            while (IsRunning)
            {
                StateTick();
                ControlTick();

                PoliceTick();

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
                if (Game.IsKeyDown(Keys.NumPad4))
                {
                    GhostCop.Position = Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0f, -4f, 0f));
                    GhostCop.Heading = Game.LocalPlayer.Character.Heading;
                    WriteToLog("KeyDown", "==========");
                    foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => x.CopPed.Exists()))
                    {
                        string TaskFiberName = "";
                        if (Cop.TaskFiber != null)
                            TaskFiberName = Cop.TaskFiber.Name;

                        WeaponHash Weapon = 0;
                        if (Cop.CopPed.Inventory.EquippedWeapon != null)
                            Weapon = Cop.CopPed.Inventory.EquippedWeapon.Hash;


                        WriteToLog("KeyDown", "______________________");
                        WriteToLog("KeyDown", string.Format("Handle: {0}", Cop.CopPed.Handle));
                        WriteToLog("KeyDown", string.Format("Can See Player: {0}", Cop.canSeePlayer));
                        WriteToLog("KeyDown", string.Format("Set Deadly: {0}", Cop.SetDeadly));
                        WriteToLog("KeyDown", string.Format("Set Unarmed: {0}", Cop.SetUnarmed));
                        WriteToLog("KeyDown", string.Format("Set Tazer: {0}", Cop.SetTazer));
                        WriteToLog("KeyDown", string.Format("Current Weapon: {0}", Weapon));
                        WriteToLog("KeyDown", string.Format("TaskFiberName: {0}", TaskFiberName));
                        WriteToLog("KeyDown", string.Format("Range To: {0}", Cop.CopPed.RangeTo(Game.LocalPlayer.Character.Position)));
                        WriteToLog("KeyDown", string.Format("IsInRangeOf 25f: {0}", Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 25f)));
                        WriteToLog("KeyDown", string.Format("IsInRangeOf 45f: {0}", Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 45f)));
                        WriteToLog("KeyDown", string.Format("IsInRangeOf 75f: {0}", Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 75f)));
                        WriteToLog("KeyDown", string.Format("getDotVectorResult: {0}", Extensions.getDotVectorResult(Cop.CopPed, Game.LocalPlayer.Character)));
                        WriteToLog("KeyDown", string.Format("PlayerIsInFront: {0}", Extensions.PlayerIsInFront(Cop.CopPed)));



                    }
                    WriteToLog("KeyDown", "==========");
                    WriteToLog("KeyDown", string.Format("Total COps: {0}", PoliceScanningSystem.CopPeds.Where(x => x.CopPed.Exists()).Count()));

                }

                //NativeFunction.Natives.xB9EFD5C25018725A("WantedMusicDisabled", true);
                //NativeFunction.CallByName<bool>("SET_AUDIO_FLAG", "WantedMusicDisabled", true);
                GameFiber.Yield();

                //NativeFunction.CallByName<bool>("SET_PED_MOVE_RATE_OVERRIDE", GhostCop, 0f);
            }

        });

    }

    //Police
    private static void GetPoliceState()
    {
        if (CurrentPoliceState == PoliceState.ArrestedWait || CurrentPoliceState == PoliceState.DeadlyChase)
            return;

        if (Game.LocalPlayer.WantedLevel == 0)
        {
            CurrentPoliceState = PoliceState.Normal;
        }
        else if (Game.LocalPlayer.WantedLevel >= 1 && Game.LocalPlayer.WantedLevel <= 3)
        {
            if ((!firedWeapon && !PoliceScanningSystem.PlayerHurtPolice) && !Game.LocalPlayer.Character.isConsideredArmed()) // Unarmed and you havent killed anyone
                CurrentPoliceState = PoliceState.UnarmedChase;
            else if ((!firedWeapon && !PoliceScanningSystem.PlayerHurtPolice))
                CurrentPoliceState = PoliceState.CautiousChase;
            else
                CurrentPoliceState = PoliceState.DeadlyChase;

        }
        else if (Game.LocalPlayer.WantedLevel >= 4 || PoliceScanningSystem.PlayerHurtPolice)
        {
            CurrentPoliceState = PoliceState.DeadlyChase;
        }

        if (Game.LocalPlayer.Character.isConsideredArmed() && Game.LocalPlayer.WantedLevel < 2 && !Game.LocalPlayer.Character.IsInAnyVehicle(false))
        {
            if (PoliceScanningSystem.CopPeds.Any(x => x.canSeePlayer))
            {
                Game.LocalPlayer.WantedLevel = 2;
                WriteToLog("PoliceTick", "Caught with gun");
            }
        }

        if (Game.LocalPlayer.Character.IsShooting && (PoliceScanningSystem.CopPeds.Any(x => x.canSeePlayer || x.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 100f))))
            firedWeapon = true;

    }
    private static void PoliceTick()
    {
        GetPoliceState();

        foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && x.CopPed.IsInAnyVehicle(false) && !x.CopPed.IsInHelicopter))
        {
            NativeFunction.CallByName<bool>("SET_DRIVER_ABILITY", Cop.CopPed, 100f);
            NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", Cop.CopPed, 8f);
            NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 32, true);
        }

        if (PrevPoliceState != CurrentPoliceState)
            PoliceStateChanged();

        if (PrevfiredWeapon != firedWeapon)
            FiredWeaponChanged();

        if (PrevPlayerHurtPolice != PoliceScanningSystem.PlayerHurtPolice)
            PlayerHurtPoliceChanged();

        if (PrevWantedLevel != Game.LocalPlayer.WantedLevel)
            WantedLevelChanged();

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

        if (CurrentPoliceState != PoliceState.Normal)
        {
            foreach (GTACop K9 in PoliceScanningSystem.K9Peds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.isTasked()))
            {
                if (K9.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 65f))
                {
                    WriteToLog("K9", "K9 Tasked?");
                    PoliceScanningSystem.TaskK9(K9);
                    break;
                }
            }
            if (CurrentPoliceState != PoliceState.UnarmedChase)
            {
                //Maybe go in the above loop? that one breaks tho
                foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.isTasked()))
                {
                    bool InVehicle = Cop.CopPed.IsInAnyVehicle(false);
                    if (Cop.WasInVehicle != InVehicle)
                    {
                        if (!InVehicle)
                        {
                            Cop.CopPed.Tasks.ClearImmediately();
                            WriteToLog("Cops", "Cop Got Out?");
                        }
                        Cop.WasInVehicle = InVehicle;
                    }
                }
            }
        }

        bool AnyRecentlySeen = PoliceScanningSystem.CopPeds.Any(x => x.SeenPlayerSince(10000));

        if (NativeFunction.CallByName<bool>("ARE_PLAYER_STARS_GREYED_OUT", Game.LocalPlayer) && PoliceScanningSystem.CopPeds.All(x => !x.RecentlySeenPlayer()))
        {
            NativeFunction.CallByName<bool>("SET_FAKE_WANTED_LEVEL", 0);
            NativeFunction.CallByName<bool>("SET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer, PoliceScanningSystem.PlacePlayerLastSeen.X, PoliceScanningSystem.PlacePlayerLastSeen.Y, PoliceScanningSystem.PlacePlayerLastSeen.Z);
        }

        if (NativeFunction.CallByName<bool>("ARE_PLAYER_STARS_GREYED_OUT", Game.LocalPlayer) && AnyRecentlySeen)
        {
            NativeFunction.CallByName<bool>("SET_FAKE_WANTED_LEVEL", MaxWantedLastLife);
        }

        if (AnyRecentlySeen)
        {
            NativeFunction.CallByName<bool>("SET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer, PoliceScanningSystem.PlacePlayerLastSeen.X, PoliceScanningSystem.PlacePlayerLastSeen.Y, PoliceScanningSystem.PlacePlayerLastSeen.Z);
        }
    }

    private static void PoliceTickNormal()
    {
        foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && (x.isTasked() || x.CopPed.Tasks.CurrentTaskStatus != Rage.TaskStatus.NoTask) && !x.TaskIsQueued))
        {
            Cop.TaskIsQueued = true;
            PoliceScanningSystem.CopsToTask.Add(new PoliceTask(Cop,PoliceTask.Task.Untask));
        }
    }
    private static void PoliceTickUnarmedChase()
    {
        //Cops on Foot
        foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.isTasked()))
        {
            SetCopTazer(Cop);
            if (!isBusted && Cop.RecentlySeenPlayer() && !Cop.TaskIsQueued && PoliceScanningSystem.CopPeds.Where(x => x.isTasked() || x.TaskIsQueued).Count() <= 4 && !Cop.CopPed.IsInAnyVehicle(false) && Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 55f))
            {
                Cop.TaskIsQueued = true;
                PoliceScanningSystem.AddItemToQueue(new PoliceTask(Cop,PoliceTask.Task.Chase));
            }
            //else if (Cop.CopPed.Tasks.CurrentTaskStatus != Rage.TaskStatus.None)
            //{
            //    Cop.CopPed.BlockPermanentEvents = false;
            //}

        }

        if (SurrenderBust && !isBustTimeOut())
            SurrenderBustEvent();

        StopSearchMode();
    }
    private static void PoliceTickArrestedWait()
    {
        foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.isTasked()))
        {
            bool InVehicle = Cop.CopPed.IsInAnyVehicle(false);
            if (InVehicle)
            {
                SetUnarmed(Cop);
            }
            else
            {
                if (PrevPoliceState != PoliceState.UnarmedChase)
                    ResetCopWeapons(Cop);
                else
                    SetCopTazer(Cop);

                if (!Cop.TaskIsQueued && PoliceScanningSystem.CopPeds.Where(x => x.isTasked() || x.TaskIsQueued).Count() <= 3 && Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 45f))//was70f
                {
                    Cop.TaskIsQueued = true;
                    PoliceScanningSystem.CopsToTask.Add(new PoliceTask(Cop, PoliceTask.Task.Arrest));
                }
                else if(!Cop.TaskIsQueued && Cop.CopPed.Tasks.CurrentTaskStatus != Rage.TaskStatus.InProgress && Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 65f))
                {
                    Cop.TaskIsQueued = true;
                    PoliceScanningSystem.CopsToTask.Add(new PoliceTask(Cop, PoliceTask.Task.SimpleArrest));
                }
            }           
        }
        Game.LocalPlayer.WantedLevel = MaxWantedLastLife;

        if (SurrenderBust && !isBustTimeOut())
            SurrenderBustEvent();

        StopSearchMode();
    }
    private static void PoliceTickCautiousChase()
    {
        foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.isTasked() && !x.CopPed.IsInAnyVehicle(false) && !x.CopPed.IsInHelicopter))
        {
            SetCopDeadly(Cop);
            if (!Cop.TaskIsQueued && PoliceScanningSystem.CopPeds.Where(x => x.isTasked() || x.TaskIsQueued).Count() <= 2 && Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 45f))
            {
                Cop.TaskIsQueued = true;
                PoliceScanningSystem.CopsToTask.Add(new PoliceTask(Cop, PoliceTask.Task.Arrest));
            }
            else if (!Cop.TaskIsQueued && Cop.RecentlySeenPlayer() && Cop.CopPed.Tasks.CurrentTaskStatus != Rage.TaskStatus.InProgress && Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 65f))
            {
                Cop.TaskIsQueued = true;
                PoliceScanningSystem.CopsToTask.Add(new PoliceTask(Cop, PoliceTask.Task.SimpleArrest));
            }

        }
        foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.isTasked() && (x.CopPed.IsInHelicopter || x.CopPed.IsOnBike)))
        {
            SetUnarmed(Cop);
        }

        if (PoliceScanningSystem.CopPeds.Any(x => x.CopPed.Exists() && !x.CopPed.IsDead && x.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 4f)) && Game.LocalPlayer.Character.Speed <= 4.0f && !Game.LocalPlayer.Character.IsInAnyVehicle(false) && !isBusted)
            ForceSurrenderTime++;
        else
            ForceSurrenderTime = 0;

        //if (ForceSurrenderTime > 0)
        //{
        //    if (!NativeFunction.Natives.x36AD3E690DA5ACEB<bool>("PeyoteIn"))//_GET_SCREEN_EFFECT_IS_ACTIVE
        //        NativeFunction.Natives.x2206BF9A37B7F724("PeyoteIn", 0, 0);//_START_SCREEN_EFFECT
        //}
        //else if (ForceSurrenderTime >= 500 || ForceSurrenderTime == 0)
        //{
        //    if (NativeFunction.Natives.x36AD3E690DA5ACEB<bool>("PeyoteIn"))//_GET_SCREEN_EFFECT_IS_ACTIVE
        //        NativeFunction.Natives.x068E835A1D0DC0E3("PeyoteIn", 0, 0);//_STOP_SCREEN_EFFECT
        //}


        if (ForceSurrenderTime >= 100)
        {
            ForceSurrenderTime = 0;
            PoliceScanningSystem.PlayerHurtPolice = true;
            //SurrenderBust = true;
        }

        if (SurrenderBust && !isBustTimeOut())
            SurrenderBustEvent();

        StopSearchMode();
    }
    private static void PoliceTickDeadlyChase()
    {
        foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.isTasked() && !x.CopPed.IsInAnyVehicle(true) && !x.CopPed.IsInHelicopter))
        {
            SetCopDeadly(Cop);
            if (!areHandsUp && !BeingArrested && !Cop.TaskIsQueued && Cop.isTasked())
            {
                Cop.TaskIsQueued = true;
                WriteToLog("PoliceTickDeadlyChase", "Queued An Untask");
                PoliceScanningSystem.CopsToTask.Add(new PoliceTask(Cop, PoliceTask.Task.Untask));
            }
            if (Cop.CopPed.Tasks.CurrentTaskStatus != Rage.TaskStatus.None)
            {
                Cop.CopPed.BlockPermanentEvents = false;
            }
        }
        foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.isTasked() && x.CopPed.IsInHelicopter))
        {
            if (!areHandsUp && Game.LocalPlayer.WantedLevel >= 4)
                SetCopDeadly(Cop);
            else
                SetUnarmed(Cop);
        }
        //foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.isTasked() && x.CopPed.IsInAnyVehicle(true)))
        //{
        //    SetUnarmed(Cop);
        //}

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
            NativeFunction.CallByName<bool>("SET_FAKE_WANTED_LEVEL", 0);
            CurrentPoliceState = PoliceState.Normal;
        }
        WriteToLog("ValueChecker", String.Format("WantedLevel Changed to: {0}", Game.LocalPlayer.WantedLevel));
        PrevWantedLevel = Game.LocalPlayer.WantedLevel;
    }
    private static void PlayerHurtPoliceChanged()
    {
        WriteToLog("ValueChecker", String.Format("PlayerHurtPolice Changed to: {0}", PoliceScanningSystem.PlayerHurtPolice));
        //ReportCrime(CrimeType.AssaultOnOfficer);

        //if (AudioController.Instance.AudioList.Count == 0)
        //{
        //    AudioController.Instance.AudioList.Add(Scanner.Resident.DISPATCH_INTRO_01.Value);
        //    AudioController.Instance.AudioList.Add(Scanner.AssistanceRequired.AssistanceRequiredRandom());
        //    AudioController.Instance.AudioList.Add(Scanner.Crimes.CRIME_ASSAULT_PEACE_OFFICER_01.Value);
        //    AudioController.Instance.AudioList.Add(Scanner.Resident.OUTRO_01.Value);
        //}
        PrevPlayerHurtPolice = PoliceScanningSystem.PlayerHurtPolice;
    }
    private static void FiredWeaponChanged()
    {
        WriteToLog("ValueChecker", String.Format("firedWeapon Changed to: {0}", firedWeapon));
        // ReportCrime(CrimeType.ShotsFired);
        //if (AudioController.Instance.AudioList.Count == 0)
        //{
        //    AudioController.Instance.AudioList.Add(Scanner.Resident.DISPATCH_INTRO_01.Value);
        //    AudioController.Instance.AudioList.Add(Scanner.AssistanceRequired.AssistanceRequiredRandom());
        //    AudioController.Instance.AudioList.Add(Scanner.Crimes.CRIME_SHOTS_FIRED_AT_AN_OFFICER_01.Value);
        //    AudioController.Instance.AudioList.Add(Scanner.Resident.OUTRO_01.Value);
        //}
        PrevfiredWeapon = firedWeapon;
    }
    private static void PoliceStateChanged()
    {
        WriteToLog("ValueChecker", String.Format("PoliceState Changed to: {0}", CurrentPoliceState));


        //foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead))
        //{
        //    if (CurrentPoliceState == PoliceState.Normal)
        //    {
        //        ResetCopWeapons(Cop.CopPed);
        //    }
        //    else if (CurrentPoliceState == PoliceState.DeadlyChase)
        //    {
        //        if (Game.LocalPlayer.WantedLevel >= 4 || !Cop.CopPed.IsInAnyVehicle(false))
        //            SetCopDeadly(Cop.CopPed);
        //        else if (Cop.CopPed.IsInAnyVehicle(false))
        //            SetUnarmed(Cop.CopPed);
        //    }
        //    else if (CurrentPoliceState == PoliceState.DeadlyChase)
        //    {
        //        SetCopDeadly(Cop.CopPed);
        //    }
        //}



        if (CurrentPoliceState == PoliceState.Normal || CurrentPoliceState == PoliceState.DeadlyChase)
        {
            
        }

        if (CurrentPoliceState == PoliceState.ArrestedWait)
        {
            //if (AudioController.Instance.AudioList.Count == 0)
            //{
            //    AudioController.Instance.AudioList.Add(Scanner.CrookArrested.CrookArrestedRandom());
            //}
        }

        if (CurrentPoliceState == PoliceState.DeadlyChase)
        {
            //if (AudioController.Instance.AudioList.Count == 0)
            //{
            //    AudioController.Instance.AudioList.Add(Scanner.Resident.DISPATCH_INTRO_01.Value);
            //    AudioController.Instance.AudioList.Add(Scanner.AssistanceRequired.AssistanceRequiredRandom());
            //    AudioController.Instance.AudioList.Add(Scanner.Crimes.CRIME_10_99_DAVID_01.Value);
            //    AudioController.Instance.AudioList.Add(Scanner.Resident.OUTRO_01.Value);
            //}
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
        if (Game.GameTime - GameTimeLastReTasked <= 5000)
            return;

        if (NativeFunction.CallByName<bool>("ARE_PLAYER_STARS_GREYED_OUT", Game.LocalPlayer) && PoliceScanningSystem.CopPeds.Any(x => x.RecentlySeenPlayer())) // Needed for the AI to keep the player in the wanted position
        {
            MoveGhostCopToPlayer();
        }
        else if (NativeFunction.CallByName<bool>("ARE_PLAYER_STARS_GREYED_OUT", Game.LocalPlayer) && !PoliceScanningSystem.CopPeds.Any(x => x.RecentlySeenPlayer()) && PoliceScanningSystem.CopPeds.Any(x => x.isTasked()))
        {
            PoliceScanningSystem.UntaskAll();
        }

        RemoveGhostCop();
    }
    private static void RemoveGhostCop()
    {
        if (GhostCop.Exists())
            GhostCop.Delete();
        //EmergencyLocation ClosestPolice = EmergencyLocations.Where(x => x.Type == EmergencyLocation.EmergencyLocationType.Police).OrderBy(s => Game.LocalPlayer.Character.Position.DistanceTo2D(s.Location)).FirstOrDefault();
        //GhostCop.IsCollisionEnabled = true;
        //if (GhostCop.Position != ClosestPolice.Location)
        //    GhostCop.Position = ClosestPolice.Location;
    }
    private static void MoveGhostCopToPlayer()
    {
        if (!GhostCop.Exists())
            CreateGhostCop();
        else
        {
            GhostCop.IsCollisionEnabled = true;
            GhostCop.Position = Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0f, -4f, 0f));
            GhostCop.Heading = Game.LocalPlayer.Character.Heading;
        }
        WriteToLog("CreateGhostCop", "Ghost Cop Moved");
        GameTimeLastReTasked = Game.GameTime;

        //GameFiber.StartNew(delegate
        //{
        //    CopModel.LoadAndWait();
        //    CopModel.LoadCollisionAndWait();
        //    Ped Ped1 = new Ped(CopModel, Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0f, -4f, 0f)), Game.LocalPlayer.Character.Heading);
        //    Ped1.BlockPermanentEvents = false;
        //    Ped1.IsCollisionEnabled = false;
        //    Ped1.IsVisible = false;
        //    SetUnarmed(Ped1);
        //    GameFiber.Sleep(50);
        //    if (Ped1.Exists())
        //        Ped1.Delete();
        //    WriteToLog("CreateGhostCop", "Ghost Cop Created");
        //    GameTimeLastReTasked = Game.GameTime;
        //});
    }
    private static void CreateGhostCop()
    {
        GhostCop = new Ped(CopModel, Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0f, -4f, 0f)), Game.LocalPlayer.Character.Heading);
        GhostCop.BlockPermanentEvents = false;
        GhostCop.IsCollisionEnabled = false;
        GhostCop.IsVisible = false;
        NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", GhostCop, (uint)2725352035, true); //Unequip weapon so you don't get shot
        NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", GhostCop, false);
        NativeFunction.CallByName<uint>("SET_PED_MOVE_RATE_OVERRIDE", GhostCop, 0f);
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
        Cop.CopPed.BlockPermanentEvents = false;
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

        Cop.CopPed.Accuracy = 100;
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
        }

        NativeFunction.CallByName<uint>("DISPLAY_HUD", true);

        if (Game.LocalPlayer.WantedLevel > PreviousWantedLevel)
            PreviousWantedLevel = Game.LocalPlayer.WantedLevel;

        if (Game.LocalPlayer.WantedLevel > MaxWantedLastLife) // The max wanted level i saw in the last life, not just right before being busted
            MaxWantedLastLife = Game.LocalPlayer.WantedLevel;
        else if (Game.LocalPlayer.WantedLevel == 0 && MaxWantedLastLife > 0 && !isBusted && !isDead)
            MaxWantedLastLife = 0;

    }
    private static void ControlTick()
    {
        if (Game.IsKeyDownRightNow(Keys.E) && (!Game.LocalPlayer.Character.IsInAnyVehicle(false) || Game.LocalPlayer.Character.CurrentVehicle.Speed < 5))
        {
            
            if (!areHandsUp && !isBusted)
            {
                if (!(Game.LocalPlayer.Character.Inventory.EquippedWeapon == null))
                    NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)2725352035, true); //Unequip weapon so you don't get shot
                PreviousPoliceState = CurrentPoliceState;
                areHandsUp = true;
                //vehicleEngineSystem.TurnOffEngine();
                RaiseHands();
                
            }

        }
        else
        {
            if (areHandsUp && !isBusted)
            {
                areHandsUp = false; // You put your hands down
                CurrentPoliceState = PreviousPoliceState;
                Game.LocalPlayer.Character.Tasks.Clear();
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

            //CurrentPoliceState = PoliceState.Normal;

            if (TargetPed.Model.Hash == 225514697)
                WriteToLog("TakeoverPed", "TargetPed.Model.Hash: " + TargetPed.Model.Hash.ToString());
            else
                LastModelHash = TargetPed.Model.Name;

            bool wasInVehicle = TargetPed.IsInAnyVehicle(false);

            CopyPedComponentVariation(TargetPed);

            Ped CurrentPed = Game.LocalPlayer.Character;
            if (TargetPed.IsInAnyVehicle(false))
            {
                Game.LocalPlayer.Character.WarpIntoVehicle(TargetPed.CurrentVehicle, -1);
                //AllyPedsToPlayer(TargetPed.CurrentVehicle.Passengers);
            }
            else
            {
                //AllyPedsToPlayer(World.GetNearbyPeds(Game.Player.Character.Position, 3f));
            }

            NativeFunction.CallByName<uint>("CHANGE_PLAYER_PED", Game.LocalPlayer, TargetPed, false, false);
            //Function.Call(Hash.CHANGE_PLAYER_PED, Game.LocalPlayer, TargetPed, false, false);

            if (DeleteOld)
                CurrentPed.Delete();
            //else if (ArrestOld)
            //    SetArrestedAnimation(CurrentPed, true);
            //else
            //    AITakeoverPlayer(CurrentPed);


            //ResetPlayer(true, true);
            //Function.Call(Hash._START_SCREEN_EFFECT, "MinigameTransitionOut", 5000, false);

            NativeFunction.Natives.x2206BF9A37B7F724("MinigameTransitionOut", 5000, false);

            //if (mySettings.ReplacePlayerWithPed)
            //{
            SetPlayerOffset();
            ChangeModel("player_zero");
            
            ChangeModel(LastModelHash);
            //

            //ChangeModel(225514697);
            //ChangeModel(LastModelHash);
            //}

            //CurrentPoliceState = PoliceState.Normal;

            if (!Game.LocalPlayer.Character.isMainCharacter())
                ReplacePedComponentVariation(Game.LocalPlayer.Character);

            if (wasInVehicle)
            {
                Game.LocalPlayer.Character.WarpIntoVehicle(Game.LocalPlayer.LastVehicle, -1);

            }
            else
            {
                Game.LocalPlayer.Character.IsCollisionEnabled = true;
            }

            //if (mySettings.ReplacePlayerWithPedRandomMoney)
            Game.LocalPlayer.Character.SetCash(rnd.Next(500, 4000),"Michael");


            Game.LocalPlayer.Character.Inventory.Weapons.Clear();
            Game.LocalPlayer.Character.Inventory.GiveNewWeapon(2725352035, 0, true);
            //GTAWeapon MyGun = GetRandomWeapon(0);
            //Game.Player.Character.Weapons.Give(MyGun.Type, MyGun.AmmoAmount, false, false);
            //Function.Call(Hash.SET_CURRENT_PED_WEAPON, Game.Player.Character, (uint)WeaponHash.Unarmed, true);
            TimesDied = 0;
            MaxWantedLastLife = 0;
            //vehicleEngineSystem.AfterPedTakeover();

           // if (!PedsTakenOver.Contains(Game.Player.Character.Handle)) { PedsTakenOver.Add(Game.Player.Character.Handle); }
           // if (!PedsTakenOver.Contains(TargetPed.Handle)) { PedsTakenOver.Add(TargetPed.Handle); }

           // UntaskAll();
        }
        catch (Exception e3)
        {
            //WriteToLog("TakeoverPed", "TakeoverPed Error; " + e3.Message);
        }
    }
    private static void CopyPedComponentVariation(Ped myPed)
    {
        try
        {
            myPedVariation = new PedVariation();
            myPedVariation.myPedComponents = new List<PedComponent>();
            for (int ComponentNumber = 0; ComponentNumber <= 12; ComponentNumber++)
            {
                myPedVariation.myPedComponents.Add(new PedComponent(ComponentNumber, NativeFunction.CallByName<int>("GET_PED_DRAWABLE_VARIATION", myPed, ComponentNumber), NativeFunction.CallByName<int>("GET_PED_TEXTURE_VARIATION", myPed, ComponentNumber), NativeFunction.CallByName<int>("GET_PED_PALETTE_VARIATION", myPed, ComponentNumber)));
            }
        }
        catch (Exception e)
        {

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
        }
        catch (Exception e)
        {

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
    //private static void SetCurrently(Ped myPed)
    //{
    //    if (!(myPed.Inventory.EquippedWeapon == null))
    //        NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", myPed, (uint)2725352035, true);
    //}

    public static void RespawnAtHospital()
    {
        isDead = false;
        isBusted = false;
        CurrentPoliceState = PoliceState.Normal;
        firedWeapon = false;
        PoliceScanningSystem.PlayerHurtPolice = false;

        Game.LocalPlayer.Character.Inventory.Weapons.Clear();
        RespawnInPlace(true);
        Game.LocalPlayer.WantedLevel = 0;
        EmergencyLocation ClosestPolice = EmergencyLocations.Where(x => x.Type == EmergencyLocation.EmergencyLocationType.Hospital).OrderBy(s => Game.LocalPlayer.Character.Position.DistanceTo2D(s.Location)).FirstOrDefault();

        Game.LocalPlayer.Character.Position = ClosestPolice.Location;
        Game.LocalPlayer.Character.Heading = ClosestPolice.Heading;

        

        // Game.Player.Character.Task.GoTo(Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 3f, 0)));
        Game.FadeScreenIn(2500);
        //if (mySettings.ReplacePlayerWithPedRandomMoney)
        //{
            int CurrentCash = Game.LocalPlayer.Character.GetCash("Michael");
            if (CurrentCash < 5000)
                Game.LocalPlayer.Character.SetCash(0, "Michael"); // Stop it from removing and adding cash, you cant go negative.
            else
                Game.LocalPlayer.Character.GiveCash(-5000, "Michael");
        //}
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
        firedWeapon = false;
        PoliceScanningSystem.PlayerHurtPolice = false;
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
        Game.FadeScreenIn(250);
        //UI.Notify("You are out on bail, try to stay out of trouble");
        //if (mySettings.ReplacePlayerWithPedRandomMoney)
        //    Game.Player.Character.GiveCash(MaxWantedLastLife * -750, mySettings.ReplacePlayerWithPedCharacter);
        //Function.Call(Hash.DISPLAY_CASH, true);

        PoliceScanningSystem.UntaskAll();
    }
    public static void BribePolice(int Amount)
    {
        if (Amount < PreviousWantedLevel * 500)
        {
            //WriteToLog("BribePolice", String.Format("Bribe Failed. required Amount: {0}, Amount Sent: {1}", Amount.ToString(), (PreviousWantedLevel * 500).ToString()));
            //UI.Notify("Thats it? Thanks for the cash, but you're going downtown.");
            //Game.Player.Character.GiveCash(-1 * Amount, mySettings.ReplacePlayerWithPedCharacter);
            //UI.Notify(String.Format("Current Cash: ${0}", Game.Player.Character.GetCash(mySettings.ReplacePlayerWithPedCharacter)));
            return;
        }
        else
        {
            BeingArrested = false;
            isBusted = false;
            //WriteToLog("BribePolice", String.Format("Bribe Worked. required Amount: {0}, Amount Sent: {1}", Amount.ToString(), (PreviousWantedLevel * 500).ToString()));
            //UI.Notify("Thanks for the cash, now beat it.");
            //Game.Player.Character.GiveCash(-1 * Amount, mySettings.ReplacePlayerWithPedCharacter);
            //UI.Notify(String.Format("Current Cash: ${0}", Game.Player.Character.GetCash(mySettings.ReplacePlayerWithPedCharacter)));
        }
        CurrentPoliceState = PoliceState.Normal;
        firedWeapon = false;
        PoliceScanningSystem.PlayerHurtPolice = false;
        UnSetArrestedAnimation(Game.LocalPlayer.Character);
        NativeFunction.CallByName<bool>("RESET_PLAYER_ARREST_STATE", Game.LocalPlayer);
        if(Game.LocalPlayer.Character.LastVehicle.Exists())
            NativeFunction.CallByName<bool>("SET_VEHICLE_HAS_BEEN_OWNED_BY_PLAYER", Game.LocalPlayer.Character.LastVehicle, true);
        ResetPlayer(true, false);

        PoliceScanningSystem.UntaskAll();
    }
    private static void WriteToLog(String ProcedureString, String TextToLog)
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
}

