using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CopTasker
{
    private IEntityProvideable PedProvider;
    private ITargetable Player;
    private IWeapons Weapons;
    private ISettingsProvideable Settings;
    private Tasker Tasker;
    private IPlacesOfInterest PlacesOfInterest;
    private List<PedExt> PossibleTargets;
    private Cop ClosestCopToPlayer;
    private int CopsTaskedToRespond = 0;
    public CopTasker(Tasker tasker, IEntityProvideable pedProvider, ITargetable player, IWeapons weapons, ISettingsProvideable settings, IPlacesOfInterest placesOfInterest)
    {
        Tasker = tasker;
        PedProvider = pedProvider;
        Player = player;
        Weapons = weapons;
        Settings = settings;
        PlacesOfInterest = placesOfInterest;
    }
    public void Setup()
    {

    }
    public void Update()
    {
        if (Settings.SettingsManager.PoliceSettings.ManageTasking)
        {
            SetPossibleTargets();
            Tasker.ExpireSeatAssignments();
            foreach (Cop cop in PedProvider.Pedestrians.PoliceList.Where(x => x.Pedestrian.Exists() && x.HasBeenSpawnedFor >= 2000 && x.CanBeTasked))
            {
                try
                {
                    if (cop.NeedsTaskAssignmentCheck)
                    {
                        UpdateCurrentTask(cop);//has yields if it does anything
                    }
                    if (cop.CurrentTask != null && cop.CurrentTask.ShouldUpdate)
                    {
                        cop.UpdateTask(PedToAttack(cop));
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Setting Civilian Task");
                }
            }
        }
    }
    private void UpdateCurrentTask(Cop Cop)//this should be moved out?
    {
        if (Cop.DistanceToPlayer <= Player.ActiveDistance)// && !Cop.IsInHelicopter)//heli, dogs, boats come next?
        {
            PedExt MainTarget = PedToAttack(Cop);
            GameFiber.Yield();
            if (MainTarget != null)
            {
                SetAIApprehend(Cop, MainTarget);
            }
            else
            {
                if (Player.IsWanted && Cop.IsRespondingToWanted)
                {
                    if (Player.IsInSearchMode)
                    {
                        SetLocate(Cop);
                    }
                    else
                    {
                        if (Cop.DistanceToPlayer <= 150f)//200f
                        {
                            if (Player.PoliceResponse.IsDeadlyChase && !Player.IsAttemptingToSurrender)
                            {
                                SetKill(Cop);
                            }
                            else
                            {
                                SetChase(Cop);
                            }
                        }
                        else
                        {
                            SetLocate(Cop);
                        }
                    }
                }
                else if (Player.Investigation.IsActive && Player.Investigation.RequiresPolice && Cop.IsRespondingToInvestigation)// && Cop.IsIdleTaskable)
                {
                    SetInvestigate(Cop);
                }
                else
                {
                    SetIdle(Cop);
                }
            }
        }
        else
        {
            SetIdle(Cop);
        }
        Cop.GameTimeLastUpdatedTask = Game.GameTime;
    }
    private PedExt PedToAttack(Cop Cop)
    {
        if (!PossibleTargets.Any(x => x.IsWanted))
        {
            return null;
        }
        PedExt MainTarget = null;
        if (Cop.Pedestrian.Exists() && Cop.DistanceToPlayer <= 200f)
        {
            List<CopTarget> copsPossibleTargets = new List<CopTarget>();
            bool anyDeadlyChase = false;
            foreach (PedExt possibleTarget in PossibleTargets)
            {
                int TotalOtherAssignedCops = PedProvider.Pedestrians.PoliceList.Count(x => x.Handle != Cop.Handle && x.CurrentTask != null && x.CurrentTask.OtherTarget != null && x.CurrentTask.OtherTarget.Handle == possibleTarget.Handle);
                //EntryPoint.WriteToConsole($"TASKER: {possibleTarget.Handle} TotalOtherAssignedCops {TotalOtherAssignedCops} WantedLevel {possibleTarget.WantedLevel}", 3);
                if (possibleTarget.Pedestrian.Exists() && possibleTarget.IsWanted && !possibleTarget.IsArrested && NativeHelper.IsNearby(Cop.CellX, Cop.CellY, possibleTarget.CellX, possibleTarget.CellY, 3))
                {
                    if (possibleTarget.IsDeadlyChase)
                    {
                        anyDeadlyChase = true;
                    }
                    copsPossibleTargets.Add(new CopTarget(possibleTarget, possibleTarget.Pedestrian.DistanceTo2D(Cop.Pedestrian)) { TotalAssignedCops = TotalOtherAssignedCops});
                }
            }
            int PlayerCops = PedProvider.Pedestrians.PoliceList.Count(x => x.Handle != Cop.Handle && x.CurrentTask != null && (x.CurrentTask.Name == "Kill" || x.CurrentTask.Name == "Chase"));




            if (Player.IsBusted)
            {
                if (anyDeadlyChase)
                {
                    MainTarget = copsPossibleTargets
                        .OrderByDescending(x => x.Target.IsDeadlyChase)
                        .ThenBy(x => x.IsOverloaded)
                        .ThenByDescending(x => /*x.DistanceToTarget <= 20f &&*/  !x.Target.IsBusted)
                        .ThenByDescending(x => x.Target.ArrestingPedHandle == Cop.Handle)
                        .ThenBy(x => x.Target.IsBusted)
                        .ThenBy(x => x.DistanceToTarget).FirstOrDefault()?.Target;
                }
                else
                {
                    if (ClosestCopToPlayer == null || Cop.Handle != ClosestCopToPlayer.Handle)
                    {
                        MainTarget = copsPossibleTargets
                            .OrderByDescending(x => x.Target.IsDeadlyChase)
                            .ThenBy(x => x.IsOverloaded)
                            .ThenByDescending(x => /*x.DistanceToTarget <= 20f &&*/ !x.Target.IsBusted)
                            .ThenByDescending(x => x.Target.ArrestingPedHandle == Cop.Handle)
                            .ThenBy(x => x.Target.IsBusted)
                            .ThenBy(x => x.DistanceToTarget).FirstOrDefault()?.Target;
                    }
                }
            }
            else if (Player.PoliceResponse.IsDeadlyChase)
            {
                CopTarget MaybeMainTarget = copsPossibleTargets.Where(x => x.Target.IsDeadlyChase && !x.IsOverloaded && !x.Target.IsBusted)
                    //.OrderByDescending(x => x.Target.IsDeadlyChase)
                   // .OrderByDescending(x => x.IsOverloaded)
                    //.OrderByDescending(x => /*x.DistanceToTarget <= 20f &&*/  !x.Target.IsBusted)
                    .OrderByDescending(x => x.Target.ArrestingPedHandle == Cop.Handle)
                    //.ThenBy(x => x.Target.IsBusted)
                    .ThenBy(x => x.DistanceToTarget).FirstOrDefault();

                MainTarget = MaybeMainTarget?.Target;
                if (MaybeMainTarget != null && MaybeMainTarget.Target != null && (MaybeMainTarget.DistanceToTarget > Cop.DistanceToPlayer + 5f) )//&& (PlayerCops < Player.WantedLevel || Player.WantedLevel >= 3))//removed for testing, want to allow cosp to engate targets close to them better
                {
                   // EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} SET TARGET: Player is Closer Than Closest Target (Deadly)", 3);
                    //EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Player is Closer Than Closest Target (Deadly)", 3);
                    MainTarget = null;
                }
            }
            else
            {
                CopTarget MaybeMainTarget = copsPossibleTargets
                    .OrderByDescending(x => x.Target.IsDeadlyChase)
                    .ThenBy(x => x.IsOverloaded)
                    .ThenByDescending(x => /*x.DistanceToTarget <= 20f &&*/  !x.Target.IsBusted)
                    .ThenByDescending(x => x.Target.ArrestingPedHandle == Cop.Handle)
                    .ThenBy(x => x.Target.IsBusted)
                    .ThenBy(x => x.DistanceToTarget).FirstOrDefault();
                MainTarget = MaybeMainTarget?.Target;
                if (Player.IsWanted && MaybeMainTarget != null && MaybeMainTarget.Target != null && !MaybeMainTarget.Target.IsDeadlyChase && PlayerCops < Player.WantedLevel)// && MaybeMainTarget.DistanceToTarget <= Cop.DistanceToPlayer + 20f)
                {
                    //EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} SET TARGET: Player is Closer Than Closest Target (Non Deadly)", 3);
                    MainTarget = null;
                }

                if (MaybeMainTarget != null && MaybeMainTarget.Target != null && !MaybeMainTarget.Target.IsDeadlyChase && MaybeMainTarget.IsOverloaded)
                {
                    //EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} SET TARGET: Non Overloaded (Non Deadly) {MaybeMainTarget.Target.Pedestrian?.Handle} TotalAssignedCops {MaybeMainTarget.TotalAssignedCops} MaybeMainTarget.IsOverloaded {MaybeMainTarget.IsOverloaded} WantedLevel {MaybeMainTarget.Target?.WantedLevel}", 3);
                    MainTarget = null;
                }

            }


            if (MainTarget != null && MainTarget.IsBusted && MainTarget.Handle != Cop.CurrentTask?.OtherTarget?.Handle && PedProvider.Pedestrians.PoliceList.Any(x => x.Handle != Cop.Handle && x.CurrentTask?.OtherTarget?.Handle == MainTarget.Handle))
            {
                //EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} SET TARGET: Too many police already on busted person, sending away", 3);
                MainTarget = null;
            }
        }
        if (MainTarget != null && MainTarget.Pedestrian.Exists() && MainTarget.Pedestrian.Handle == Game.LocalPlayer.Character.Handle)//for ped swappiung, they get confused!
        {
           // EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} SET TARGET: MainTarget Is Player", 3);
            MainTarget = null;
        }
        return MainTarget;
    }
    private void SetPossibleTargets()
    {
        if (PedProvider.IsZombieApocalypse)
        {
            List<PedExt> TotalList = new List<PedExt>();
            TotalList.AddRange(PedProvider.Pedestrians.CivilianList);
            TotalList.AddRange(PedProvider.Pedestrians.GangMemberList);
            TotalList.AddRange(PedProvider.Pedestrians.ZombieList);
            PossibleTargets = TotalList.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsAlive && !x.IsUnconscious && (x.IsWanted || (x.IsBusted && !x.IsArrested)) && x.DistanceToPlayer <= 200f).ToList();//150f
        }
        else
        {
            List<PedExt> TotalList = new List<PedExt>();
            TotalList.AddRange(PedProvider.Pedestrians.CivilianList);
            TotalList.AddRange(PedProvider.Pedestrians.GangMemberList);
            PossibleTargets = TotalList.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsAlive && !x.IsUnconscious && (x.IsWanted || (x.IsBusted && !x.IsArrested)) && x.DistanceToPlayer <= 200f).ToList();//150f
        }
        ClosestCopToPlayer = PedProvider.Pedestrians.PoliceList.Where(x => x.Pedestrian.Exists() && !x.IsInVehicle && x.DistanceToPlayer <= 30f && x.Pedestrian.IsAlive).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
    }
    private void SetInvestigate(Cop Cop)
    {
        if (Cop.CurrentTask?.Name != "Investigate")
        {
            EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Task Changed from {Cop.CurrentTask?.Name} to Investigate", 3);
            Cop.CurrentTask = new Investigate(Cop, Player, Settings);
            Cop.WeaponInventory.Reset();
            GameFiber.Yield();//TR Added back 4
            Cop.CurrentTask.Start();
        }
    }
    private void SetIdle(Cop Cop)
    {
        if (Cop.CurrentTask?.Name != "Idle")// && Cop.IsIdleTaskable)
        {
            EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Task Changed from {Cop.CurrentTask?.Name} to Idle", 3);
            Cop.CurrentTask = new Idle(Cop, Player, PedProvider, Tasker, PlacesOfInterest, Cop);
            Cop.WeaponInventory.Reset();
            GameFiber.Yield();//TR Added back 4
            Cop.CurrentTask.Start();
        }
    }
    private void SetLocate(Cop Cop)
    {
        if (Cop.CurrentTask?.Name != "Locate")
        {
            EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Task Changed from {Cop.CurrentTask?.Name} to Locate", 3);
            Cop.CurrentTask = new Locate(Cop, Player, Settings);
            Cop.WeaponInventory.Reset();
            GameFiber.Yield();//TR Added back 4
            Cop.CurrentTask.Start();
        }
    }
    private void SetChase(Cop Cop)
    {
        if (Cop.CurrentTask?.Name != "Chase")
        {
            EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Task Changed from {Cop.CurrentTask?.Name} to Chase", 3);
            Cop.CurrentTask = new Chase(Cop, Player, PedProvider, Cop, Settings);
            Cop.WeaponInventory.Reset();
            GameFiber.Yield();//TR Added back 4
            Cop.CurrentTask.Start();
        }
    }
    private void SetKill(Cop Cop)
    {
        if (Cop.CurrentTask?.Name != "Kill")
        {
            EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Task Changed from {Cop.CurrentTask?.Name} to Kill", 3);
            Cop.CurrentTask = new Kill(Cop, Player, Settings);
            Cop.WeaponInventory.Reset();
            GameFiber.Yield();//TR Added back 4
            Cop.CurrentTask.Start();
        }
    }
    private void SetAIApprehend(Cop Cop, PedExt MainTarget)
    {
        if (Cop.CurrentTask?.Name != "AIApprehend")
        {
            EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Task Changed from {Cop.CurrentTask?.Name} to AIApprehend", 3);
            Cop.CurrentTask = new AIApprehend(Cop, Player, Cop) { OtherTarget = MainTarget };
            Cop.WeaponInventory.Reset();
            GameFiber.Yield();//TR Added back 4
            Cop.CurrentTask.Start();
        }
    }

    private class CopTarget
    {
        public CopTarget(PedExt target, float distanceToTarget)
        {
            Target = target;
            DistanceToTarget = distanceToTarget;
        }
        public PedExt Target { get; set; }
        public float DistanceToTarget { get; set; } = 999f;
        public int TotalAssignedCops { get; set; } = 0;
        public bool IsOverloaded => Target?.WantedLevel <= 2 && TotalAssignedCops > Target?.WantedLevel;
    }
}


