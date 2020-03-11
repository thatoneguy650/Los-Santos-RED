using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Tasking
{
    private static readonly Random rnd;
    private static List<PoliceTask> CopsToTask;
    private static uint LastBust;
    private static int ForceSurrenderTime;
    private static bool SurrenderBust;
    private static uint GameTimeLastResetWeapons;

    public static string CurrentPoliceTickRunning { get; set; }
    public static bool IsRunning { get; set; }

    static Tasking()
    {
        rnd = new Random();
    }
    public static void Initialize()
    {
        IsRunning = true;
        CopsToTask = new List<PoliceTask>();

        LastBust = 0;
        ForceSurrenderTime = 0;
        SurrenderBust = false;
        GameTimeLastResetWeapons = 0;
        CurrentPoliceTickRunning = "";
        
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    private static void DisplayQueue()
    {
        string Tasking = "";//string.Format("ToTask: {0}", CopsToTask.Count());
        foreach(PoliceTask MyTask in CopsToTask)
        {
            Tasking = MyTask.CopToAssign.Pedestrian.Handle.ToString() + ":" + MyTask.TaskToAssign.ToString();
        }
        UI.Text(Tasking, 0.8f, 0.16f, 0.35f, false, Color.White, UI.EFont.FontChaletComprimeCologne);
    }
    public static void ProcessQueue()
    {
        int _ToTask = CopsToTask.Count;

        if (_ToTask > 0)
        {
            //LocalWriteToLog("TaskQueue", string.Format("Cops To Task: {0}", _ToTask));
            PoliceTask _policeTask = CopsToTask[0];
            _policeTask.CopToAssign.isTasked = true;

            if (_policeTask.TaskToAssign == PoliceTask.Task.Untask && CopsToTask.Any(x => x.CopToAssign == _policeTask.CopToAssign && x.TaskToAssign != PoliceTask.Task.Untask && x.GameTimeAssigned >= _policeTask.GameTimeAssigned))
            {
                _policeTask.CopToAssign.TaskIsQueued = false;
                CopsToTask.RemoveAt(0);
            }
            else
            {
                if (_policeTask.TaskToAssign == PoliceTask.Task.Arrest)
                    TaskChasing(_policeTask.CopToAssign);
                else if (_policeTask.TaskToAssign == PoliceTask.Task.Chase)
                    TaskChasing(_policeTask.CopToAssign);
                else if (_policeTask.TaskToAssign == PoliceTask.Task.Untask)
                    Untask(_policeTask.CopToAssign);
                else if (_policeTask.TaskToAssign == PoliceTask.Task.SimpleArrest)
                    TaskSimpleArrest(_policeTask.CopToAssign);
                else if (_policeTask.TaskToAssign == PoliceTask.Task.SimpleChase)
                    TaskSimpleChase(_policeTask.CopToAssign);
                else if (_policeTask.TaskToAssign == PoliceTask.Task.VehicleChase)
                    TaskVehicleChase(_policeTask.CopToAssign);
                else if (_policeTask.TaskToAssign == PoliceTask.Task.SimpleInvestigate)
                    TaskSimpleInvestigate(_policeTask.CopToAssign);
                else if (_policeTask.TaskToAssign == PoliceTask.Task.GoToWantedCenter)
                    TaskGoToWantedCenter(_policeTask.CopToAssign);
                else if (_policeTask.TaskToAssign == PoliceTask.Task.RandomSpawnIdle)
                    RandomSpawnIdle(_policeTask.CopToAssign);
                else if (_policeTask.TaskToAssign == PoliceTask.Task.HeliChase)
                    TaskHeliChase(_policeTask.CopToAssign);
                else if (_policeTask.TaskToAssign == PoliceTask.Task.TaskInvestigateCrime)
                    TaskInvestigateCrime(_policeTask.CopToAssign);

                _policeTask.CopToAssign.TaskIsQueued = false;
                CopsToTask.RemoveAt(0);
                   
            }
        }
    }
    public static void AddItemToQueue(PoliceTask MyTask)
    {
        if (!CopsToTask.Any(x => x.CopToAssign == MyTask.CopToAssign && x.TaskToAssign == MyTask.TaskToAssign))
        {
            MyTask.GameTimeAssigned = Game.GameTime;
            CopsToTask.Add(MyTask);
            MyTask.CopToAssign.TaskIsQueued = true;
            Debugging.WriteToLog("InstantActionTick", string.Format("Queued: {0}, For: {1}", MyTask.TaskToAssign, MyTask.CopToAssign.Pedestrian.Handle));
        }
    }
    public static void PoliceVehicleTick()
    {
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.isInVehicle))//&& !x.isTasked))
        {
            if (!Cop.isInHelicopter)
            {
                if (Police.CurrentPoliceState == Police.PoliceState.DeadlyChase && (Police.CurrentCrimes.KillingPolice.InstancesObserved >= 1 || Police.CurrentCrimes.KillingCivilians.InstancesObserved >= 1 || LosSantosRED.PlayerWantedLevel >= 4))
                {
                    NativeFunction.CallByName<bool>("SET_DRIVER_ABILITY", Cop.Pedestrian, 100f);
                    NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.Pedestrian, 4, true);
                    NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.Pedestrian, 8, true);
                    NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.Pedestrian, 16, true);
                    NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.Pedestrian, 512, true);
                    NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.Pedestrian, 262144, true);
                    NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", Cop.Pedestrian, 8f);
                }
                else
                {
                    NativeFunction.CallByName<bool>("SET_DRIVER_ABILITY", Cop.Pedestrian, 100f);
                    NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", Cop.Pedestrian, 8f);
                    NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.Pedestrian, 32, true);
                }

                if (PlayerLocation.PlayerIsOffroad && Cop.DistanceToPlayer <= 200f)
                {
                    NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.Pedestrian, 4194304, true);
                }
                else
                {
                    NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.Pedestrian, 4194304, false);
                }
            }
            else
            {
                NativeFunction.CallByName<bool>("SET_DRIVER_ABILITY", Cop.Pedestrian, 100f);
                //if (!Cop.TaskIsQueued && Cop.CopPed.IsDriver())
                //{
                //    Cop.TaskIsQueued = true;
                //    AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.HeliChase));
                //}
                
            }
        }

        //Maybe causing phantom busts?
        //SearchModeStopping.StopSearchMode = true;
    }
    public static void PoliceStateTick()
    {
        PoliceScanning.CopPeds.RemoveAll(x => !x.Pedestrian.Exists());

        if (Police.PoliceInInvestigationMode)
            PoliceTickInvestigation();
        else if (Police.CurrentPoliceState == Police.PoliceState.Normal && LosSantosRED.PlayerWantedLevel == 0)//if (CurrentPoliceState == PoliceState.Normal)
            PoliceTickNormal();
        else if (Police.PoliceInSearchMode)
            PoliceTickSearchMode();
        else if (Police.CurrentPoliceState == Police.PoliceState.UnarmedChase)
            PoliceTickUnarmedChase();
        else if (Police.CurrentPoliceState == Police.PoliceState.CautiousChase)
            PoliceTickCautiousChase();
        else if (Police.CurrentPoliceState == Police.PoliceState.DeadlyChase)
            PoliceTickDeadlyChase();
        else if (Police.CurrentPoliceState == Police.PoliceState.ArrestedWait)
            PoliceTickArrestedWait();
        else
            CurrentPoliceTickRunning = "";

        if(LosSantosRED.PlayerIsWanted)
            PoliceVehicleTick();

        if (Police.CurrentPoliceState == Police.PoliceState.UnarmedChase || Police.CurrentPoliceState == Police.PoliceState.CautiousChase || Police.CurrentPoliceState == Police.PoliceState.ArrestedWait)
            SearchModeStopping.StopSearchMode = true;
        else
            SearchModeStopping.StopSearchMode = false;
    }
    private static void PoliceTickNormal()
    {
        CurrentPoliceTickRunning = "Normal";
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.Pedestrian.Exists()))
        {
            if (Cop.isTasked && !Cop.TaskIsQueued && Cop.TaskType != PoliceTask.Task.RandomSpawnIdle)
            {
                AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Untask));
            }
            else if (Cop.WasRandomSpawn && !Cop.isTasked && !Cop.TaskIsQueued)
            {
                AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.RandomSpawnIdle));
            }

            if (Cop.isTasked && !Cop.TaskIsQueued && Cop.TaskType == PoliceTask.Task.RandomSpawnIdle)
            {
                if(Cop.Pedestrian.IsDriver() && Cop.Pedestrian.CurrentVehicle.HasSiren && Cop.Pedestrian.CurrentVehicle.IsSirenOn)
                {
                    Cop.Pedestrian.CurrentVehicle.IsSirenOn = false;
                    Cop.Pedestrian.CurrentVehicle.IsSirenSilent = false;
                }
            }
        }
        if (Game.GameTime - Police.GameTimePoliceStateStart >= 8000 && Game.GameTime - GameTimeLastResetWeapons >= 10000)//Only reset them every 10 seconds if they need it after 8 seconds of being at normal. Incase you go from normal to deadly real fast.
        {
            foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.Pedestrian.Exists() && (x.SetDeadly || x.SetTazer || x.SetUnarmed)))
            {
                ResetCopWeapons(Cop);//just in case they get stuck
            }
            GameTimeLastResetWeapons = Game.GameTime;
        }
    }
    private static void PoliceTickInvestigation()
    {
        CurrentPoliceTickRunning = "Investigation";
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.Pedestrian.Exists()))
        {
            float DistToInvest = Cop.Pedestrian.DistanceTo2D(Police.InvestigationPosition);
            if (Cop.WasRandomSpawn && !Cop.isTasked && !Cop.TaskIsQueued && DistToInvest>= Police.InvestigationDistance)
            {
                AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.RandomSpawnIdle));
            }
            else if (Cop.isTasked && !Cop.TaskIsQueued && Cop.TaskType == PoliceTask.Task.RandomSpawnIdle && Cop.Pedestrian.IsDriver() && DistToInvest <= Police.InvestigationDistance && DistToInvest >= 35f)
            {
                AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.TaskInvestigateCrime));
            }
        }
    }
    private static void PoliceTickUnarmedChase()
    {
        CurrentPoliceTickRunning = "Unarmed Chase";
        RemoveAllIdleTasks();
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.Pedestrian.Exists() && !x.isTasked))
        {
            if (Cop.Pedestrian.IsOnBike || Cop.Pedestrian.IsInHelicopter)
                SetUnarmed(Cop);
            else
                SetCopTazer(Cop);

            if (Cop.DistanceToPlayer <= 55f)
            {
                int TotalFootChaseTasked = PoliceScanning.CopPeds.Where(x => (x.isTasked || x.TaskIsQueued) && x.TaskType == PoliceTask.Task.Chase).Count();
                int TotalVehicleChaseTasked = PoliceScanning.CopPeds.Where(x => (x.isTasked || x.TaskIsQueued) && x.TaskType == PoliceTask.Task.VehicleChase).Count();

                if (!LosSantosRED.IsBusted && Cop.RecentlySeenPlayer() && !Cop.TaskIsQueued && TotalFootChaseTasked <= 4 && !Cop.Pedestrian.IsInAnyVehicle(false) && Cop.DistanceToPlayer <= 55f && (!Game.LocalPlayer.Character.IsInAnyVehicle(false) || Game.LocalPlayer.Character.CurrentVehicle.Speed <= 5f))
                {
                    Cop.TaskIsQueued = true;
                    AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Chase));
                }
                else if (!LosSantosRED.IsBusted && Cop.RecentlySeenPlayer() && !Cop.TaskIsQueued && TotalFootChaseTasked > 0 && TotalVehicleChaseTasked <= 5 && Cop.isInVehicle && Cop.Pedestrian.CurrentVehicle.Exists() && !Cop.Pedestrian.CurrentVehicle.HasPassengers && !Cop.isInHelicopter && Cop.DistanceToPlayer <= 55f && !Game.LocalPlayer.Character.IsInAnyVehicle(false)) //&& TotalFootChaseTasked > 0
                {
                    Cop.TaskIsQueued = true;
                    AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.VehicleChase));
                }
                if ((LosSantosRED.HandsAreUp || Game.LocalPlayer.Character.IsStunned || Game.LocalPlayer.Character.IsRagdoll) && !LosSantosRED.IsBusted && Cop.DistanceToPlayer <= 4f && !Police.PlayerWasJustJacking && !Cop.isInVehicle)
                    SetSurrenderBust(true, "Unarmed Chase Tick 1");
            }
        }
        if (PoliceScanning.CopPeds.Any(x => x.DistanceToPlayer <= 4f && !x.Pedestrian.IsInAnyVehicle(false)) && (Game.LocalPlayer.Character.IsRagdoll || Game.LocalPlayer.Character.Speed <= 0.4f) && !LosSantosRED.PlayerInVehicle && !LosSantosRED.IsBusted)
        {
            ///TEMP CRAP
            foreach(GTACop MyCop in PoliceScanning.CopPeds.Where(x => x.DistanceToPlayer <= 4f && !x.isInVehicle))
            {
                Debugging.WriteToLog("", string.Format("Handle: {0}, Distance: {1}", MyCop.Pedestrian.Handle, MyCop.DistanceToPlayer));
            }
            SetSurrenderBust(true, "Unarmed Chase Tick 2");
        }

        if (SurrenderBust && !IsBustTimeOut())
            SurrenderBustEvent();

        SearchModeStopping.StopSearchMode = true;
       //StopSearchMode();
    }
    private static void PoliceTickArrestedWait()
    {
        CurrentPoliceTickRunning = "Arrested Wait";
        RemoveAllIdleTasks();
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.Pedestrian.Exists() && !x.isTasked && x.Pedestrian.Exists())) // Exist/Dead Check
        {
            bool InVehicle = Cop.Pedestrian.IsInAnyVehicle(false);
            if (InVehicle)
            {
                SetUnarmed(Cop);
            }
            else
            {
                if (!Cop.TaskIsQueued && PoliceScanning.CopPeds.Where(x => x.isTasked || x.TaskIsQueued).Count() <= 3 && Cop.DistanceToPlayer <= 45f)
                {
                    Cop.TaskIsQueued = true;
                    AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Arrest));
                }
                else if (!Cop.TaskIsQueued && (Cop.Pedestrian.Tasks.CurrentTaskStatus == Rage.TaskStatus.NoTask || Cop.Pedestrian.Tasks.CurrentTaskStatus == Rage.TaskStatus.Preparing || Cop.Pedestrian.Tasks.CurrentTaskStatus == Rage.TaskStatus.Interrupted) && (Cop.RecentlySeenPlayer() || Cop.DistanceToPlayer <= 65f))
                {
                    Cop.TaskIsQueued = true;
                    Cop.GameTimeLastTask = Game.GameTime;
                    AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.SimpleArrest));
                }
                else if (!Cop.TaskIsQueued && Game.GameTime - Cop.GameTimeLastTask > 3500 && Cop.RecentlySeenPlayer() && Cop.Pedestrian.Tasks.CurrentTaskStatus == Rage.TaskStatus.InProgress && Cop.DistanceToPlayer > 45f)
                {
                    Cop.TaskIsQueued = true;
                    Cop.GameTimeLastTask = Game.GameTime;
                    AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.SimpleArrest)); //retask the arrest
                }
                else if(!Cop.TaskIsQueued && LosSantosRED.IsDead)
                {
                    Cop.TaskIsQueued = true;
                    Cop.GameTimeLastTask = Game.GameTime;
                    AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Untask)); //retask the arrest
                }
            }
        }
        Police.SetWantedLevel(LosSantosRED.MaxWantedLastLife,"Changing it back to what it was max during your last life",true);

        if (PoliceScanning.CopPeds.Any(x => x.DistanceToPlayer <= 4f && !x.isInVehicle) && (Game.LocalPlayer.Character.IsRagdoll || Game.LocalPlayer.Character.Speed <= 1.0f) && !LosSantosRED.IsBusted)// && !InstantAction.PlayerInVehicle && !InstantAction.IsBusted)
            SetSurrenderBust(true, "Arrested Wait Tcik");

        if (SurrenderBust && !IsBustTimeOut())
            SurrenderBustEvent();

        SearchModeStopping.StopSearchMode = true;
        //StopSearchMode();
    }
    private static void PoliceTickCautiousChase()
    {
        CurrentPoliceTickRunning = "Cautious Chase";
        RemoveAllIdleTasks();
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.Pedestrian.Exists() && !x.isTasked && !x.isInVehicle && !x.isInHelicopter))
        {
            SetCopDeadly(Cop);
            if (!Cop.TaskIsQueued && PoliceScanning.CopPeds.Where(x => x.isTasked || x.TaskIsQueued).Count() <= 4 && Cop.DistanceToPlayer <= 45f)
            {
                Cop.TaskIsQueued = true;
                AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Arrest));
            }
            else if (!Cop.TaskIsQueued && Cop.Pedestrian.Tasks.CurrentTaskStatus == Rage.TaskStatus.NoTask && (Cop.RecentlySeenPlayer() || Cop.DistanceToPlayer <= 65f))
            {
                Cop.TaskIsQueued = true;
                Cop.GameTimeLastTask = Game.GameTime;
                AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.SimpleArrest));
            }
            else if (!Cop.TaskIsQueued && Game.GameTime - Cop.GameTimeLastTask > 3500 && Cop.RecentlySeenPlayer() && Cop.Pedestrian.Tasks.CurrentTaskStatus == Rage.TaskStatus.InProgress && Cop.DistanceToPlayer > 35f)
            {
                Cop.TaskIsQueued = true;
                Cop.GameTimeLastTask = Game.GameTime;
                AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.SimpleArrest));
            }

        }
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.Pedestrian.Exists() && x.isTasked && x.TaskType != PoliceTask.Task.NoTask))//foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.isTasked && x.SimpleTaskName != "")) NoTask
        {
            if (!Cop.TaskIsQueued && Game.GameTime - Cop.GameTimeLastTask > 20000 && Cop.RecentlySeenPlayer() && Cop.Pedestrian.Tasks.CurrentTaskStatus == Rage.TaskStatus.InProgress && Cop.DistanceToPlayer > 25f)
            {
                Cop.TaskIsQueued = true;
                Cop.GameTimeLastTask = Game.GameTime;
                AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.SimpleArrest));
            }
            else if (!Cop.TaskIsQueued && Game.GameTime - Cop.GameTimeLastTask > 20000 && !Cop.RecentlySeenPlayer() && Cop.DistanceToPlayer > 35f)
            {
                Cop.TaskIsQueued = true;
                AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Untask));
            }

        }
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.Pedestrian.Exists() && !x.isTasked))
        {
            if (Cop.isInVehicle)
                SetUnarmed(Cop);
            else
                SetCopTazer(Cop);
        }

        if (PoliceScanning.CopPeds.Any(x => x.DistanceToPlayer <= 8f && !x.isInVehicle) && Game.LocalPlayer.Character.Speed <= 4.0f && !Game.LocalPlayer.Character.IsInAnyVehicle(false) && !LosSantosRED.IsBusted && !Police.PlayerWasJustJacking)
            ForceSurrenderTime++;
        else
            ForceSurrenderTime = 0;

        if (ForceSurrenderTime >= 500)
            SetSurrenderBust(true, "Force Surrender Time Over 500");

        if (SurrenderBust && !IsBustTimeOut())
            SurrenderBustEvent();

        SearchModeStopping.StopSearchMode = true;
        //StopSearchMode();
    }
    private static void PoliceTickDeadlyChase()
    {
        CurrentPoliceTickRunning = "Deadly Chase";
        RemoveAllIdleTasks();
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.Pedestrian.Exists() && !x.isInVehicle))
        {
            SetCopDeadly(Cop);
            if (!LosSantosRED.HandsAreUp && !LosSantosRED.BeingArrested && !Cop.TaskIsQueued && Cop.isTasked)
            {
                Cop.TaskIsQueued = true;
                AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Untask));
            }
        }
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.Pedestrian.Exists() && !x.isTasked && x.isInHelicopter))
        {
            if (!LosSantosRED.HandsAreUp && Game.LocalPlayer.WantedLevel >= 4)
                SetCopDeadly(Cop);
            else
                SetUnarmed(Cop);
        }
        if (Settings.IssuePoliceHeavyWeapons)
        {
            foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.Pedestrian.Exists() && x.isInVehicle && x.IssuedHeavyWeapon == null))
            {
                Police.IssueCopHeavyWeapon(Cop);
                break;
            }
        }

        if (Police.CurrentCrimes.KillingPolice.InstancesObserved >= Settings.PoliceKilledSurrenderLimit && LosSantosRED.PlayerWantedLevel < 4)
        {
            Police.SetWantedLevel(4,"You killed too many cops",true);
            DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportWeaponsFree, 2));
        }

        if (SurrenderBust && !IsBustTimeOut())
            SurrenderBustEvent();
    }
    private static void PoliceTickSearchMode()
    {
        CurrentPoliceTickRunning = "Search Mode";
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.Pedestrian.Exists() && (x.DistanceToLastSeen <= 350f || x.DistanceToPlayer <= 250f)))//.Where(x => !x.isTasked))
        {
            if (Cop.isInVehicle)
            {
                SetUnarmed(Cop);
            }
            if (!Cop.isInHelicopter)
            {
                if (!Cop.AtWantedCenterDuringSearchMode && !Cop.TaskIsQueued && Cop.TaskType != PoliceTask.Task.GoToWantedCenter && Cop.DistanceToLastSeen >= 35f && Cop.Pedestrian.IsDriver())//((InVehicle && Cop.CopPed.CurrentVehicle.Driver == Cop.CopPed) || !InVehicle))
                {
                    Cop.TaskIsQueued = true;
                    AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.GoToWantedCenter));
                }
                else if (!Cop.TaskIsQueued && Cop.TaskType != PoliceTask.Task.SimpleInvestigate && Cop.DistanceToLastSeen < 35f)
                {
                    Cop.AtWantedCenterDuringSearchMode = true;
                    Cop.TaskIsQueued = true;
                    AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.SimpleInvestigate));
                }
            }
        }


    }
    private static void SurrenderBustEvent()
    {
        if (Game.LocalPlayer.WantedLevel == 0)
        {
            SetSurrenderBust(false, "Reset SurrenderBustEvent Wanted = 0");
            SurrenderBust = false;
        }
        else
        {
            LosSantosRED.BeingArrested = true;
            Police.CurrentPoliceState = Police.PoliceState.ArrestedWait;
            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)2725352035, true);
            LosSantosRED.HandsAreUp = false;
            SetSurrenderBust(false, "Reset SurrenderBustEvent");
            LastBust = Game.GameTime;
            Debugging.WriteToLog("SurrenderBust", "SurrenderBust Executed");
        }
    }

    public static void SetUnarmed(GTACop Cop)
    {
        if (!Cop.Pedestrian.Exists() || (Cop.SetUnarmed && !Cop.NeedsWeaponCheck))
            return;
        if (Settings.OverridePoliceAccuracy)
            Cop.Pedestrian.Accuracy = Settings.PoliceGeneralAccuracy;
        
        NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Cop.Pedestrian, 0);
        if (!(Cop.Pedestrian.Inventory.EquippedWeapon == null))
        {
            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Cop.Pedestrian, (uint)2725352035, true); //Unequip weapon so you don't get shot
            NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Cop.Pedestrian, false);
        }
        NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Cop.Pedestrian, 2, false);//cant do drivebys
        Cop.SetTazer = false;
        Cop.SetUnarmed = true;
        Cop.SetDeadly = false;
        Cop.GameTimeLastWeaponCheck = Game.GameTime;
    }
    private static void ResetCopWeapons(GTACop Cop)
    {
        if (!Cop.Pedestrian.Exists() || (!Cop.SetDeadly && !Cop.SetTazer && !Cop.SetUnarmed && !Cop.NeedsWeaponCheck))
            return;
        if (Settings.OverridePoliceAccuracy)
            Cop.Pedestrian.Accuracy = Settings.PoliceGeneralAccuracy;
        NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Cop.Pedestrian, 30);
        if (!Cop.Pedestrian.Inventory.Weapons.Contains(Cop.IssuedPistol.Name))
            Cop.Pedestrian.Inventory.GiveNewWeapon(Cop.IssuedPistol.Name, -1, false);
        NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Cop.Pedestrian, true);
        NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Cop.Pedestrian, 2, true);//can do drivebys
        Cop.SetTazer = false;
        Cop.SetUnarmed = false;
        Cop.SetDeadly = false;
        Cop.GameTimeLastWeaponCheck = Game.GameTime;
    }
    private static void SetCopDeadly(GTACop Cop)
    {
        if (!Cop.Pedestrian.Exists() || (Cop.SetDeadly && !Cop.NeedsWeaponCheck))
            return;
        if (Settings.OverridePoliceAccuracy)
            Cop.Pedestrian.Accuracy = Settings.PoliceGeneralAccuracy;
        NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Cop.Pedestrian, 30);
        if (!Cop.Pedestrian.Inventory.Weapons.Contains(Cop.IssuedPistol.Name))
            Cop.Pedestrian.Inventory.GiveNewWeapon(Cop.IssuedPistol.Name, -1, true);

        if ((Cop.Pedestrian.Inventory.EquippedWeapon == null || Cop.Pedestrian.Inventory.EquippedWeapon.Hash == WeaponHash.StunGun) && Game.LocalPlayer.WantedLevel >= 0)
            Cop.Pedestrian.Inventory.GiveNewWeapon(Cop.IssuedPistol.Name, -1, true);

        if (Settings.AllowPoliceWeaponVariations)
            LosSantosRED.ApplyWeaponVariation(Cop.Pedestrian, (uint)Cop.IssuedPistol.Hash, Cop.PistolVariation);
        NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Cop.Pedestrian, true);
        NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Cop.Pedestrian, 2, true);//can do drivebys
        Cop.SetTazer = false;
        Cop.SetUnarmed = false;
        Cop.SetDeadly = true;
        Cop.GameTimeLastWeaponCheck = Game.GameTime;
    }
    private static void SetCopTazer(GTACop Cop)
    {
        if (!Cop.Pedestrian.Exists() || (Cop.SetTazer && !Cop.NeedsWeaponCheck))
            return;

        if (Settings.OverridePoliceAccuracy)
            Cop.Pedestrian.Accuracy = Settings.PoliceTazerAccuracy;
        NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Cop.Pedestrian, 100);
        if (!Cop.Pedestrian.Inventory.Weapons.Contains(WeaponHash.StunGun))
        {
            Cop.Pedestrian.Inventory.GiveNewWeapon(WeaponHash.StunGun, 100, true);
        }
        else if (Cop.Pedestrian.Inventory.EquippedWeapon != WeaponHash.StunGun)
        {
            Cop.Pedestrian.Inventory.EquippedWeapon = WeaponHash.StunGun;
        }
        NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Cop.Pedestrian, false);
        NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Cop.Pedestrian, 2, false);//cant do drivebys
        Cop.SetTazer = true;
        Cop.SetUnarmed = false;
        Cop.SetDeadly = false;
        Cop.GameTimeLastWeaponCheck = Game.GameTime;
    }

    private static void SetSurrenderBust(bool ValueToSet,string DebugReason)
    {

        SurrenderBust = ValueToSet;
        Debugging.WriteToLog("SetSurrenderBust", string.Format("Reason: {0}",DebugReason));
    }
    private static bool IsBustTimeOut()
    {
        if (Police.PlayerHasBeenWantedFor <= 3000)
            return true;
        else if (Surrendering.IsCommitingSuicide)
            return true;
        else if (Game.GameTime - LastBust >= 10000)
            return false;
        else
            return true;
    }
    private static void TaskChasing(GTACop Cop)
    {
        if(!Cop.Pedestrian.Exists())
               return;
        if (Cop.Pedestrian.IsInRangeOf(Game.LocalPlayer.Character.Position, 100f) && Cop.TaskFiber != null && Cop.TaskFiber.Name == "Chase" && !Cop.RecentlySeenPlayer())
        {
            return;
        }
        if (!Cop.Pedestrian.IsInRangeOf(Game.LocalPlayer.Character.Position, 100f) && Cop.TaskFiber != null)
        {
            Cop.Pedestrian.Tasks.Clear();
            Cop.Pedestrian.BlockPermanentEvents = false;
            Cop.TaskFiber.Abort();
            Cop.TaskFiber = null;
            //LocalWriteToLog("Task Chasing", string.Format("Initial Return: {0}", Cop.CopPed.Handle));
            return;
        }
        
        Cop.TaskFiber =
        GameFiber.StartNew(delegate
        {
            if (!Cop.Pedestrian.Exists())
                return;
            //LocalWriteToLog("Task Chasing", string.Format("Started Chase: {0}", Cop.CopPed.Handle));
            uint TaskTime = 0;// = Game.GameTime;
            string LocalTaskName = "GoTo";
            double cool = rnd.NextDouble() * (1.17 - 1.075) + 1.075;//(1.175 - 1.1) + 1.1;
            float MoveRate = (float)cool;
            //Cop.SimpleTaskName = "Chase";
            Cop.TaskType = PoliceTask.Task.Chase;
            NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_USE_CLIMBOVERS", Cop.Pedestrian, true);
            NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_USE_LADDERS", Cop.Pedestrian, true);
            NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_DROP_FROM_HEIGHT", Cop.Pedestrian, true);
            Cop.Pedestrian.BlockPermanentEvents = true;

            //Main Loop
            while (Cop.Pedestrian.Exists() && !Cop.Pedestrian.IsDead)
            {
                Cop.Pedestrian.BlockPermanentEvents = true;


                if(LosSantosRED.PlayerWantedLevel >= 2)
                    NativeFunction.CallByName<uint>("SET_PED_MOVE_RATE_OVERRIDE", Cop.Pedestrian, MoveRate);

                if (TaskTime == 0 || Game.GameTime - TaskTime >= 250)//250
                {
                    ArmCopAppropriately(Cop);
                    if (Cop.DistanceToPlayer > 100f || !Cop.RecentlySeenPlayer())
                        break;

                    //if (Cop.CopPed.IsGettingIntoVehicle)
                    //{
                    //    if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Cop.CopPed.VehicleTryingToEnter.Exists() && Game.LocalPlayer.Character.CurrentVehicle.Handle == Cop.CopPed.VehicleTryingToEnter.Handle)
                    //    {
                    //        Cop.CopPed.Tasks.Clear();
                    //        NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_AIMING", Cop.CopPed, Game.LocalPlayer.Character, 2f, 20f);
                    //        Cop.CopPed.KeepTasks = true;
                    //        TaskTime = Game.GameTime;
                    //        LocalTaskName = "Arrest";
                    //        //LocalWriteToLog("TaskChasing", string.Format("Cop SubTasked with Car Arrest From Carjacking!!!!, {0}", Cop.CopPed.Handle));
                    //    }
                    //}

                    if (LosSantosRED.PlayerInVehicle && Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle != null && Game.LocalPlayer.Character.CurrentVehicle.Speed <= 2.5f)
                    {
                        if (Cop.isPursuitPrimary && Cop.DistanceToPlayer <= 25f && LocalTaskName != "CarJack")
                        {
                            Cop.Pedestrian.CanRagdoll = false;
                            //NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", Cop.CopPed, Game.LocalPlayer.Character.CurrentVehicle, -1, -1, 2f, 9);

                            NativeFunction.CallByName<bool>("TASK_OPEN_VEHICLE_DOOR", Cop.Pedestrian, Game.LocalPlayer.Character.CurrentVehicle, -1, -1, 10f);
                            Cop.Pedestrian.KeepTasks = true;
                            TaskTime = Game.GameTime;
                            LocalTaskName = "CarJack";
                            //LocalWriteToLog("TaskChasing", "Primary Cop SubTasked with CarJack 2");
                        }
                        else if (!Cop.isPursuitPrimary && Cop.DistanceToPlayer <= 25f && LocalTaskName != "Arrest")
                        {
                            NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_AIMING", Cop.Pedestrian, Game.LocalPlayer.Character, 2f, 20f);
                            Cop.Pedestrian.KeepTasks = true;
                            TaskTime = Game.GameTime;
                            LocalTaskName = "Arrest";
                            //LocalWriteToLog("TaskChasing", string.Format("Cop SubTasked with Car Arrest, {0}", Cop.CopPed.Handle));
                        }
                    }
                    else
                    {
                        if (LosSantosRED.PlayerWantedLevel <= 1)
                        {
                            if (LocalTaskName != "Approach" && Police.CurrentPoliceState == Police.PoliceState.UnarmedChase && Cop.DistanceToPlayer >= 7f)
                            {
                                NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Game.LocalPlayer.Character, -1, 4f, 0.25f, 1073741824, 1); //Original and works ok
                                TaskTime = Game.GameTime;
                                Cop.Pedestrian.KeepTasks = true;
                                LocalTaskName = "Approach";
                            }
                        }
                        else
                        {
                            if (LocalTaskName != "Arrest" && (Police.CurrentPoliceState == Police.PoliceState.ArrestedWait || (Police.CurrentPoliceState == Police.PoliceState.CautiousChase && Cop.DistanceToPlayer <= 15f)))
                            {
                                unsafe
                                {
                                    int lol = 0;
                                    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                                    NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Game.LocalPlayer.Character, -1, 20f, 500f, 1073741824, 1); //Original and works ok
                                    NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_AIMING", 0, Game.LocalPlayer.Character, 4f, 20f);
                                    NativeFunction.CallByName<bool>("TASK_AIM_GUN_AT_ENTITY", 0, Game.LocalPlayer.Character, 10000, false);
                                    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                                    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                                    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Cop.Pedestrian, lol);
                                    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                                }
                                TaskTime = Game.GameTime;
                                Cop.Pedestrian.KeepTasks = true;
                                LocalTaskName = "Arrest";
                            }
                            else if (LocalTaskName != "GotoShooting" && Police.CurrentPoliceState == Police.PoliceState.UnarmedChase && Cop.DistanceToPlayer <= 7f)
                            {
                                Cop.Pedestrian.CanRagdoll = true;
                                NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY_WHILE_AIMING_AT_ENTITY", Cop.Pedestrian, Game.LocalPlayer.Character, Game.LocalPlayer.Character, 200f, true, 4.0f, 200f, false, false, (uint)FiringPattern.DelayFireByOneSecond);
                                Cop.Pedestrian.KeepTasks = true;
                                TaskTime = Game.GameTime;
                                LocalTaskName = "GotoShooting";
                            }
                            else if (LocalTaskName != "Goto" && (Police.CurrentPoliceState == Police.PoliceState.UnarmedChase || Police.CurrentPoliceState == Police.PoliceState.CautiousChase) && Cop.DistanceToPlayer >= 15) //was 15f
                            {
                                Cop.Pedestrian.CanRagdoll = true;
                                NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", Cop.Pedestrian, Game.LocalPlayer.Character, -1, 5.0f, 500f, 1073741824, 1); //Original and works ok
                                Cop.Pedestrian.KeepTasks = true;
                                TaskTime = Game.GameTime;
                                LocalTaskName = "Goto";
                            }
                        }
                    }

                    if ((LosSantosRED.HandsAreUp || Game.LocalPlayer.Character.IsStunned || Game.LocalPlayer.Character.IsRagdoll) && !LosSantosRED.IsBusted && Cop.DistanceToPlayer <= 4f && !Police.PlayerWasJustJacking && !Cop.isInVehicle)
                        SetSurrenderBust(true, "Chase Tick1");

                    if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Speed <= 4f && !LosSantosRED.IsBusted && Cop.DistanceToPlayer <= 4f && !Police.PlayerWasJustJacking && !Cop.isInVehicle)
                        SetSurrenderBust(true, "Chase Tick2");

                    if (LosSantosRED.PlayerInVehicle && Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle != null  && (Cop.DistanceToPlayer >= 45f || Game.LocalPlayer.Character.CurrentVehicle.Speed >= 10f))
                    {
                        GameFiber.Sleep(rnd.Next(500, 2000));//GameFiber.Sleep(rnd.Next(900, 1500));//reaction time?
                        break;
                    }
                    Cop.Pedestrian.KeepTasks = true;
                    TaskTime = Game.GameTime;
                }

                GameFiber.Yield();
                if (Police.CurrentPoliceState == Police.PoliceState.Normal || Police.CurrentPoliceState == Police.PoliceState.DeadlyChase || LosSantosRED.IsDead)
                {
                    GameFiber.Sleep(rnd.Next(500, 2000));//GameFiber.Sleep(rnd.Next(900, 1500));//reaction time?
                    break;
                }
            }
            if (Cop.Pedestrian.Exists() && !Cop.Pedestrian.IsDead)
            {
                Cop.Pedestrian.BlockPermanentEvents = false;
                Cop.Pedestrian.Tasks.Clear();
                if (Cop.Pedestrian.LastVehicle.Exists() && !Cop.Pedestrian.LastVehicle.IsPoliceVehicle)
                    Cop.Pedestrian.ClearLastVehicle();
            }
           // LocalWriteToLog("Task Chasing", string.Format("Loop End: {0}", Cop.CopPed.Handle));
            Cop.TaskFiber = null;
            Cop.isTasked = false;
            Cop.TaskType = PoliceTask.Task.NoTask;
            //Cop.SimpleTaskName = "";
            if (Cop.Pedestrian.Exists() && !Cop.Pedestrian.IsDead)
                Cop.Pedestrian.CanRagdoll = true;

        }, "Chase");
        Debugging.GameFibers.Add(Cop.TaskFiber);
    }
    private static void TaskSimpleChase(GTACop Cop)
    {
        Cop.TaskType = PoliceTask.Task.SimpleChase;
        Cop.Pedestrian.BlockPermanentEvents = true;
        //Cop.SimpleTaskName = "SimpleChase";
        Cop.Pedestrian.Tasks.GoToWhileAiming(Game.LocalPlayer.Character, 10f, 40f);
        Cop.Pedestrian.KeepTasks = true;
       // LocalWriteToLog("TaskSimpleChase", "How many times i this getting called?");
    }
    private static void TaskSimpleArrest(GTACop Cop)
    {
        Cop.TaskType = PoliceTask.Task.SimpleArrest;
        Cop.Pedestrian.BlockPermanentEvents = true;
        //Cop.SimpleTaskName = "SimpleArrest";
        unsafe
        {
            int lol = 0;
            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Game.LocalPlayer.Character, -1, 20f, 500f, 1073741824, 1); //Original and works ok
            NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_AIMING", 0, Game.LocalPlayer.Character, 4f, 20f);
            NativeFunction.CallByName<bool>("TASK_AIM_GUN_AT_ENTITY", 0, Game.LocalPlayer.Character, -1, false);
            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Cop.Pedestrian, lol);
            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        }
        Cop.Pedestrian.KeepTasks = true;
       // LocalWriteToLog("TaskSimpleArrest", string.Format("Started SimpleArrest: {0}", Cop.CopPed.Handle));
    }
    private static void TaskVehicleChase(GTACop Cop)
    {
        //if (!PoliceScanning.CopPeds.Any(x => x.TaskType == PoliceTask.Task.Chase))
        //{
        //   // LocalWriteToLog("Task Vehicle Chasing", string.Format("Didn't Start Vehicle Chase: {0}", Cop.CopPed.Handle));
        //    return; //Only task this is we already have officers on foot
        //}

        Cop.TaskFiber =
        GameFiber.StartNew(delegate
        {
           // LocalWriteToLog("Task Vehicle Chasing", string.Format("Started Vehicle Chase: {0}", Cop.CopPed.Handle));
            uint TaskTime = Game.GameTime;
            if (!Cop.Pedestrian.Exists())
                return;

            //if (Cop.Pedestrian.CurrentVehicle.Exists())
            //{
            //    if (Cop.Pedestrian.CurrentVehicle.HasPassengers)
            //        return;
            //}

            Cop.Pedestrian.BlockPermanentEvents = true;
            Cop.TaskType = PoliceTask.Task.VehicleChase;
            //Cop.SimpleTaskName = "VehicleChase";

            NativeFunction.CallByName<bool>("SET_DRIVER_ABILITY", Cop.Pedestrian, 100f);
            NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Cop.Pedestrian, 3, false);
            Cop.Pedestrian.KeepTasks = true;

            while (Cop.Pedestrian.Exists() && !Cop.Pedestrian.IsDead)
            {

                if (Game.GameTime - TaskTime >= 250)
                {
                    if (!Cop.Pedestrian.IsInAnyVehicle(false))
                    {
                       // LocalWriteToLog("Task Vehicle Chase", string.Format("I got out of my car like a dummy: {0}", Cop.CopPed.Handle));
                        break;
                    }
                    if (LosSantosRED.PlayerInVehicle)
                    {
                       // LocalWriteToLog("Task Vehicle Chase", string.Format("Player got in a vehicle, letting ai takeover: {0}", Cop.CopPed.Handle));
                        break;
                    }
                    if (!Cop.RecentlySeenPlayer())
                    {
                      //  LocalWriteToLog("Task Vehicle Chase", string.Format("Lost the player, let AI takeover: {0}", Cop.CopPed.Handle));
                        break;
                    }
                    Vector3 PlayerPos = Game.LocalPlayer.Character.Position;
                    Vector3 DrivingCoords = World.GetNextPositionOnStreet(PlayerPos);
                    NativeFunction.CallByName<bool>("SET_DRIVE_TASK_DRIVING_STYLE", Cop.Pedestrian, 6);
                    NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.Pedestrian, 2, true);
                    NativeFunction.CallByName<bool>("TASK_VEHICLE_GOTO_NAVMESH", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, DrivingCoords.X, DrivingCoords.Y, DrivingCoords.Z, 25f, 110, 10f);
                    Cop.Pedestrian.KeepTasks = true;
                    TaskTime = Game.GameTime;
                }
                GameFiber.Yield();
                if (Police.CurrentPoliceState == Police.PoliceState.Normal || Police.CurrentPoliceState == Police.PoliceState.DeadlyChase || Police.CurrentPoliceState == Police.PoliceState.ArrestedWait || LosSantosRED.IsBusted || LosSantosRED.IsDead)
                {
                    GameFiber.Sleep(rnd.Next(500, 2000));//GameFiber.Sleep(rnd.Next(900, 1500));//reaction time?
                    break;
                }
            }
            if (Cop.Pedestrian.Exists() && !Cop.Pedestrian.IsDead)
            {
                NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Cop.Pedestrian, 3, true);
                Cop.Pedestrian.BlockPermanentEvents = false;
                Cop.Pedestrian.Tasks.Clear();
            }
            //LocalWriteToLog("Task Vehicle Chase", string.Format("Loop End: {0}", Cop.CopPed.Handle));
            Cop.TaskFiber = null;
            Cop.isTasked = false;
            //Cop.SimpleTaskName = "";
            Cop.TaskType = PoliceTask.Task.NoTask;
        }, "VehicleChase");
        Debugging.GameFibers.Add(Cop.TaskFiber);
    }
    private static void TaskSimpleInvestigate(GTACop Cop)
    {
        if (!Cop.Pedestrian.Exists())
            return;
        Cop.TaskType = PoliceTask.Task.SimpleInvestigate;
        Cop.Pedestrian.BlockPermanentEvents = false;
        //Cop.SimpleTaskName = "SimpleInvestigate";

        Vector3 TargetLocation = Police.PlacePlayerLastSeen.Around2D(65f);//(Police.PlayerLastSeenForwardVector * 55f).Around2D(75f);
        //Blip MyBlip = new Blip(TargetLocation, 15f)
        //{
        //    Color = Color.Purple,
        //    Alpha = 0.5f
        //};
        //Police.TempBlips.Add(MyBlip);

        if (Cop.isInVehicle && Police.PlayerLastSeenInVehicle)
        {
            Cop.Pedestrian.Tasks.CruiseWithVehicle(30f, VehicleDrivingFlags.Emergency);
            //LocalWriteToLog("TaskSimpleInvestigate", string.Format("Started SimpleInvestigate(CruiseWithVehicle): {0}", Cop.CopPed.Handle));
        }
        if (Cop.isInVehicle && !Police.PlayerLastSeenInVehicle && Police.AnyPoliceSeenPlayerThisWanted)
        {
            Vehicle CopCar = Cop.Pedestrian.CurrentVehicle;
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_VEHICLE_TEMP_ACTION", 0, CopCar, 27, 8000);     
                NativeFunction.CallByName<bool>("TASK_LEAVE_VEHICLE", 0, CopCar, 256);



                NativeFunction.CallByName<bool>("TASK_GO_STRAIGHT_TO_COORD", 0, TargetLocation.X, TargetLocation.Y, TargetLocation.Z, 500f, -1, Police.PlayerLastSeenHeading, 1f);
                //NativeFunction.CallByName<bool>("TASK_WANDER_STANDARD", 0, CopCar, 30f, 0);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Cop.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
            //LocalWriteToLog("TaskSimpleInvestigate", string.Format("Started SimpleInvestigate(ExitVehicleWander): {0}", Cop.CopPed.Handle));
        }
        else if(!Cop.isInVehicle)
        {
            //LocalWriteToLog("TaskSimpleInvestigate", string.Format("Started SimpleInvestigate(Wander): {0}", Cop.CopPed.Handle));
            Cop.Pedestrian.Tasks.GoStraightToPosition(TargetLocation, 500f, Police.PlayerLastSeenHeading, 1f, -1);
            //Cop.CopPed.Tasks.Wander();
        }
    }
    private static void TaskGoToWantedCenter(GTACop Cop)
    {
        if (!Cop.Pedestrian.Exists())
            return;
        Cop.TaskType = PoliceTask.Task.GoToWantedCenter;
        Cop.Pedestrian.BlockPermanentEvents = false;
        Vector3 WantedCenter = Police.InvestigationPosition = NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
        if (Cop.Pedestrian.IsInAnyVehicle(false))
            NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, WantedCenter.X, WantedCenter.Y, WantedCenter.Z, 35f, 4 | 16 | 32 | 262144, 15f);//NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, PositionOfInterest.X, PositionOfInterest.Y, PositionOfInterest.Z, 70f, 4 | 16 | 32 | 262144, 35f);
        else
            NativeFunction.CallByName<bool>("TASK_GO_STRAIGHT_TO_COORD", Cop.Pedestrian, WantedCenter.X, WantedCenter.Y, WantedCenter.Z, 500f, -1, 0f, 2f);

        Debugging.WriteToLog("TaskGoToWantedCenter", string.Format("Started GoToWantedCenter: {0}", Cop.Pedestrian.Handle));
    }
    private static void TaskInvestigateCrime(GTACop Cop)
    {
        if (!Cop.Pedestrian.Exists())
            return;

        if (LosSantosRED.PlayerIsWanted)
            return;

        Cop.TaskFiber =
        GameFiber.StartNew(delegate
        {
            Police.PoliceInInvestigationMode = true;
            Cop.TaskType = PoliceTask.Task.TaskInvestigateCrime;
            Cop.Pedestrian.BlockPermanentEvents = false;

            if (Police.InvestigationPosition == Vector3.Zero)
            {
                Police.InvestigationPosition = Game.LocalPlayer.Character.Position;
                if (LosSantosRED.PlayerIsWanted)
                    Police.InvestigationPosition = NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
            }

            Vector3 OriginalTaskedPosition = Police.InvestigationPosition;
            if (Cop.Pedestrian.IsInAnyVehicle(false))
                NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, Police.InvestigationPosition.X, Police.InvestigationPosition.Y, Police.InvestigationPosition.Z, 20f, 4 | 16 | 32 | 262144, 10f);//NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, PositionOfInterest.X, PositionOfInterest.Y, PositionOfInterest.Z, 70f, 4 | 16 | 32 | 262144, 35f);
            else
                NativeFunction.CallByName<bool>("TASK_GO_STRAIGHT_TO_COORD", Cop.Pedestrian, Police.InvestigationPosition.X, Police.InvestigationPosition.Y, Police.InvestigationPosition.Z, 500f, -1, 0f, 2f);


            Debugging.WriteToLog("TaskInvestigateCrime", string.Format("Started GoToWantedCenter: {0}", Cop.Pedestrian.Handle));

            uint GameTimestartedInvestigation = Game.GameTime;
            while (Cop.Pedestrian.Exists() && Cop.Pedestrian.DistanceTo2D(Police.InvestigationPosition) >= 15f && Police.PoliceInInvestigationMode && Game.GameTime - GameTimestartedInvestigation <= 180000)//less than 3 minutes
            {
                if(Police.InvestigationPosition != Vector3.Zero && Police.InvestigationPosition != OriginalTaskedPosition) //retask them if it changes
                {
                    OriginalTaskedPosition = Police.InvestigationPosition;
                    if (Cop.Pedestrian.IsInAnyVehicle(false))
                        NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, Police.InvestigationPosition.X, Police.InvestigationPosition.Y, Police.InvestigationPosition.Z, 20f, 4 | 16 | 32 | 262144, 10f);//NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, PositionOfInterest.X, PositionOfInterest.Y, PositionOfInterest.Z, 70f, 4 | 16 | 32 | 262144, 35f);
                    else
                        NativeFunction.CallByName<bool>("TASK_GO_STRAIGHT_TO_COORD", Cop.Pedestrian, Police.InvestigationPosition.X, Police.InvestigationPosition.Y, Police.InvestigationPosition.Z, 500f, -1, 0f, 2f);
                }

                if (Cop.Pedestrian.IsDriver() && Cop.Pedestrian.CurrentVehicle.HasSiren)
                {
                    if (!Cop.Pedestrian.CurrentVehicle.IsSirenOn)
                    {
                        Cop.Pedestrian.CurrentVehicle.IsSirenOn = true;
                        Cop.Pedestrian.CurrentVehicle.IsSirenSilent = false;
                    }
                }
                GameFiber.Sleep(100);
            }


            uint GameTimeStartedInvestigating = Game.GameTime;
            while (Game.GameTime - GameTimeStartedInvestigating <= 5000)
            {
                if(!Police.PoliceInInvestigationMode)
                {
                    break;
                }
                GameFiber.Sleep(100);
            }


            if (!Cop.Pedestrian.Exists())
                return;

            if (LosSantosRED.PlayerIsNotWanted)
            {
                if (Cop.Pedestrian.Exists() && Cop.Pedestrian.IsDriver() && Cop.Pedestrian.CurrentVehicle.HasSiren)
                {
                    Cop.Pedestrian.CurrentVehicle.IsSirenOn = false;
                    Cop.Pedestrian.CurrentVehicle.IsSirenSilent = false;
                }

                if (Cop.Pedestrian.DistanceTo2D(Police.InvestigationPosition) <= 15f && LosSantosRED.PlayerIsNotWanted)
                    Police.PoliceReportedAllClear();

                AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Untask));
            }

            Debugging.WriteToLog("TaskInvestigateCrime", string.Format("Finished TaskInvestigateCrime: {0}", Cop.Pedestrian.Handle));


        }, "InvestigateCrime");
        Debugging.GameFibers.Add(Cop.TaskFiber);
    }
    private static void TaskHeliChase(GTACop Cop)
    {
        if (!Cop.Pedestrian.Exists())
            return;
        Cop.TaskType = PoliceTask.Task.HeliChase;
        Cop.Pedestrian.BlockPermanentEvents = true;
        NativeFunction.CallByName<bool>("TASK_HELI_CHASE", Cop.Pedestrian, Game.LocalPlayer.Character, 25f, 25f, 40f);

        //LocalWriteToLog("TaskGoToWantedCenter", string.Format("Started HeliChase: {0}", Cop.CopPed.Handle));
    }
    public static void TaskK9(GTACop Cop)
    {
        Cop.TaskFiber =
        GameFiber.StartNew(delegate
        {
            //LocalWriteToLog("Task K9 Chasing", string.Format("Started Chase: {0}", Cop.CopPed.Handle));
            uint TaskTime = Game.GameTime;
            string LocalTaskName = "GoTo";

            Cop.Pedestrian.BlockPermanentEvents = true;
            while (Cop.Pedestrian.Exists() && !Cop.Pedestrian.IsDead && Cop.Pedestrian.IsInAnyVehicle(false) && !Cop.Pedestrian.CurrentVehicle.IsSeatFree(-1))
                GameFiber.Sleep(2000);

           // LocalWriteToLog("Task K9 Chasing", string.Format("Near Player Chase: {0}", Cop.CopPed.Handle));

            while (Cop.Pedestrian.Exists() && !Cop.Pedestrian.IsDead)
            {
                NativeFunction.CallByName<uint>("SET_PED_MOVE_RATE_OVERRIDE", Cop.Pedestrian, 1.5f);
                Cop.Pedestrian.KeepTasks = true;
                Cop.Pedestrian.BlockPermanentEvents = true;

                if (Game.GameTime - TaskTime >= 500)
                {

                    float _locrangeTo = Cop.Pedestrian.RangeTo(Game.LocalPlayer.Character.Position);
                    if (LocalTaskName != "Exit" && Cop.Pedestrian.IsInAnyVehicle(false) && Cop.Pedestrian.CurrentVehicle.Speed <= 5 && !Cop.Pedestrian.CurrentVehicle.HasDriver && _locrangeTo <= 75f)
                    {
                        NativeFunction.CallByName<bool>("TASK_LEAVE_VEHICLE", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, 16);
                        Cop.Pedestrian.FaceEntity(Game.LocalPlayer.Character);
                        //Cop.CopPed.Heading = Game.LocalPlayer.Character.Heading;
                        TaskTime = Game.GameTime;
                        LocalTaskName = "Exit";
                       // LocalWriteToLog("TaskK9Chasing", "Cop SubTasked with Exit");
                    }
                    else if (Police.CurrentPoliceState == Police.PoliceState.ArrestedWait && LocalTaskName != "Arrest")
                    {
                        NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", Cop.Pedestrian, Game.LocalPlayer.Character, -1, 5.0f, 500f, 1073741824, 1); //Original and works ok
                        TaskTime = Game.GameTime;
                        LocalTaskName = "Arrest";
                       // LocalWriteToLog("TaskK9Chasing", "Cop SubTasked with Arresting");
                    }
                    else if ((Police.CurrentPoliceState == Police.PoliceState.UnarmedChase || Police.CurrentPoliceState == Police.PoliceState.CautiousChase || Police.CurrentPoliceState == Police.PoliceState.DeadlyChase) && LocalTaskName != "GotoFighting" && _locrangeTo <= 10f) //was 10f
                    {
                        NativeFunction.CallByName<bool>("TASK_COMBAT_PED", Cop.Pedestrian, Game.LocalPlayer.Character, 0, 16);
                        Cop.Pedestrian.KeepTasks = true;
                        TaskTime = Game.GameTime;
                        LocalTaskName = "GotoFighting";
                        //GameFiber.Sleep(25000);
                       // LocalWriteToLog("TaskK9Chasing", "Cop SubTasked with Fighting");
                    }
                    else if ((Police.CurrentPoliceState == Police.PoliceState.UnarmedChase || Police.CurrentPoliceState == Police.PoliceState.CautiousChase || Police.CurrentPoliceState == Police.PoliceState.DeadlyChase) && LocalTaskName != "Goto" && _locrangeTo >= 15f) //was 15f
                    {
                        NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", Cop.Pedestrian, Game.LocalPlayer.Character, -1, 5.0f, 500f, 1073741824, 1); //Original and works ok
                        Cop.Pedestrian.KeepTasks = true;
                        TaskTime = Game.GameTime;
                        LocalTaskName = "Goto";
                       // LocalWriteToLog("TaskK9Chasing", "Cop SubTasked with GoTo");
                    }

                    if (Police.CurrentPoliceState == Police.PoliceState.Normal || Police.CurrentPoliceState == Police.PoliceState.DeadlyChase)
                    {
                        GameFiber.Sleep(rnd.Next(500, 2000));//GameFiber.Sleep(rnd.Next(900, 1500));//reaction time?
                        break;
                    }
                }
                GameFiber.Yield();
            }
            Debugging.WriteToLog("Task K9 Chasing", string.Format("Loop End: {0}", Cop.Pedestrian.Handle));
            Cop.TaskFiber = null;

            if (Cop.Pedestrian.Exists() && !Cop.Pedestrian.IsDead)
            {
                Cop.Pedestrian.IsPersistent = false;
                Cop.Pedestrian.BlockPermanentEvents = false;
                if (!Cop.Pedestrian.IsInAnyVehicle(false))
                    Cop.Pedestrian.Tasks.ReactAndFlee(Game.LocalPlayer.Character);
            }
        }, "K9");
        Debugging.GameFibers.Add(Cop.TaskFiber);
    }
    public static void RetaskAllRandomSpawns()
    {
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.WasRandomSpawn))
        {
            if (!Cop.TaskIsQueued)
            {
                Cop.TaskIsQueued = true;
                AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.RandomSpawnIdle));
            }
        }
       // LocalWriteToLog("RetaskAllRandomSpawns", "Done");
    }
    public static void RemoveAllIdleTasks()
    {
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.isTasked && (x.TaskType == PoliceTask.Task.RandomSpawnIdle || x.TaskType == PoliceTask.Task.TaskInvestigateCrime) && x.Pedestrian.DistanceTo2D(Game.LocalPlayer.Character) <= 350f))
        {
            Cop.TaskType = PoliceTask.Task.NoTask;
            Cop.isTasked = false;
            //if (!Cop.TaskIsQueued)
            //{
            //    Cop.TaskIsQueued = true;
            //    AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.RandomSpawnIdle));
            //}
        }
    }
    private static void RandomSpawnIdle(GTACop Cop)
    {
        if (Cop.Pedestrian.Exists())
        {
            if (!Cop.Pedestrian.IsInAnyVehicle(false))
            {
                Vehicle LastVehicle = Cop.Pedestrian.LastVehicle;
                if (LastVehicle.Exists() && LastVehicle.IsDriveable && Cop.WasRandomSpawnDriver)
                {
                    unsafe
                    {
                        int lol = 0;
                        NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                        NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", 0, LastVehicle, -1, -1, 2f, 9);
                        NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", 0, LastVehicle, 18f, 183);
                        NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                        NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                        NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Cop.Pedestrian, lol);
                        NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                    }
                   // LocalWriteToLog("RetaskAllRandomSpawns", "Told him to get in and drive");
                }
                else
                {
                    Cop.Pedestrian.Tasks.Wander();
                 //   LocalWriteToLog("RetaskAllRandomSpawns", "Told him to wander");
                }
            }
            else
            {
                Cop.Pedestrian.Tasks.CruiseWithVehicle(Cop.Pedestrian.CurrentVehicle, 15f, VehicleDrivingFlags.Normal);
                Cop.Pedestrian.CurrentVehicle.IsSirenOn = false;
                //NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", Cop.CopPed, Cop.CopPed.CurrentVehicle, 18f, 183);
                //Cop.CopPed.Tasks.Wander();
               // LocalWriteToLog("RetaskAllRandomSpawns", "Told him to drive");
            }
        }

    }
    public static void UntaskAll(bool OnlyTasked)
    {
        foreach (GTACop Cop in PoliceScanning.CopPeds)
        {

            if (OnlyTasked && Cop.isTasked && !Cop.TaskIsQueued)
            {
                Cop.TaskIsQueued = true;
                AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Untask));
            }
            else
            {
                Cop.TaskIsQueued = true;
                AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Untask));
            }
        }
        foreach (GTACop Cop in PoliceScanning.K9Peds)
        {
            if (Cop.isTasked && !Cop.TaskIsQueued)
            {
                Cop.TaskIsQueued = true;
                AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Untask));
            }
        }
       // LocalWriteToLog("UntaskAll", "");
    }
    public static void UntaskAllRandomSpawns(bool OnlyTasked)
    {
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.WasRandomSpawn))
        {
            if (OnlyTasked && Cop.isTasked && !Cop.TaskIsQueued)
            {
                Cop.TaskIsQueued = true;
                AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Untask));
            }
            else
            {
                Cop.TaskIsQueued = true;
                AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Untask));
            }
        }

        //LocalWriteToLog("UntaskAll Random", "");
    }
    private static void Untask(GTACop Cop)
    {
        if (Cop.Pedestrian.Exists())
        {
            if (Cop.TaskFiber != null)
            {
                Cop.TaskFiber.Abort();
                Cop.TaskFiber = null;
            }
            int seatIndex = 0;
            Vehicle CurrentVehicle = null;
            bool WasInVehicle = false;
            if (Cop.WasRandomSpawn && Cop.Pedestrian.IsInAnyVehicle(false))
            {
                WasInVehicle = true;
                CurrentVehicle = Cop.Pedestrian.CurrentVehicle;
                seatIndex = Cop.Pedestrian.SeatIndex;
            }
            Cop.Pedestrian.Tasks.Clear();

            Cop.Pedestrian.BlockPermanentEvents = false;

            if (!Cop.WasRandomSpawn)
                Cop.Pedestrian.IsPersistent = false;

            if (Cop.WasRandomSpawn && WasInVehicle && !Cop.Pedestrian.IsInAnyVehicle(false) && CurrentVehicle != null)
            {
                Cop.Pedestrian.WarpIntoVehicle(CurrentVehicle, seatIndex);

            }



            if (Cop.Pedestrian.IsDriver() && Cop.Pedestrian.CurrentVehicle.HasSiren)
            {
                Cop.Pedestrian.CurrentVehicle.IsSirenOn = false;
                Cop.Pedestrian.CurrentVehicle.IsSirenSilent = false;
            }

            //if (WasInVehicle)
            //    LocalWriteToLog("Untask", string.Format("Untasked: {0} in vehicle", Cop.CopPed.Handle));
            //else
            //    LocalWriteToLog("Untask", string.Format("Untasked: {0}", Cop.CopPed.Handle));
        }

        Cop.TaskType = PoliceTask.Task.NoTask;
        //Cop.SimpleTaskName = "";
        Cop.isTasked = false;
    }
    private static void ArmCopAppropriately(GTACop Cop)
    {
        if (Police.CurrentPoliceState == Police.PoliceState.UnarmedChase)
        {
            SetCopTazer(Cop);
        }
        else if (Police.CurrentPoliceState == Police.PoliceState.CautiousChase)
        {
            SetCopDeadly(Cop);
        }
        else if (Police.CurrentPoliceState == Police.PoliceState.ArrestedWait && Police.LastPoliceState == Police.PoliceState.UnarmedChase)
        {
            SetCopTazer(Cop);
        }
        else if (Police.CurrentPoliceState == Police.PoliceState.ArrestedWait && Police.LastPoliceState != Police.PoliceState.UnarmedChase)
        {
            SetCopDeadly(Cop);
        }
    }
}

