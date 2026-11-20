using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CopBrain : PedBrain
{
    private Cop Cop;
    public CopBrain(Cop cop, ISettingsProvideable settings, IEntityProvideable world, IWeapons weapons) : base(cop, settings, world, weapons)
    {
        Cop = cop;
        Settings = settings;
        World = world;
    }
    public override void Setup()
    {

    }
    public override void Dispose()
    {

    }
    public override void Update(ITargetable player, IPlacesOfInterest placesOfInterest)
    {
        Player = player;
        PlacesOfInterest = placesOfInterest;
        if (Cop.PlayerPerception.CanSeeTarget && Player.IsWanted && Player.IsInVehicle &&
            Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists() &&
            Player.CurrentVehicle.Vehicle.Driver.Exists() &&
            Player.CurrentVehicle.Vehicle.Driver.Handle == Player.Character.Handle)
        {
            Player.CurrentVehicle.OnPoliceSeenCar(true);
        }
        if (Cop.CanBeTasked)
        {
            if (Cop.HasExistedFor >= 1000 && Cop.RecentlyUpdated)
            {
                bool didOne = false;
                if (Cop.NeedsTaskAssignmentCheck && Cop.CanBeAmbientTasked)
                {
                    UpdateCurrentTask();//has yields if it does anything
                    didOne = true;
                }
                if (Cop.CurrentTask != null && Cop.CurrentTask.ShouldUpdate)//used to just be an IF
                {
                    //GameFiber.Yield();
                    if (Cop.ShouldUpdateTarget)
                    {
                        PedExt otherTarget = Cop.CurrentTask is AIApprehend activeTrafficStop && activeTrafficStop.IsTrafficStop
                            ? GetPlayerPedExt()
                            : PedToAttack();
                        Cop.CurrentTarget = otherTarget;
                        Cop.GameTimeLastUpdatedTarget = Game.GameTime;
                        Cop.UpdateTask(otherTarget);
                    }
                    else
                    {
                        Cop.UpdateTask();
                    }
                    //GameFiber.Yield();
                }
                GameFiber.Yield();
            }
        }
        else
        {
            if (Cop.CurrentTask != null)
            {
                Cop.CurrentTask = null;
            }
        }
    }
    protected override void UpdateCurrentTask()
    {
        if (Cop.CurrentTask is AIApprehend activeTrafficStop && activeTrafficStop.IsTrafficStop)
        {
            return;
        }
        if (Cop.DistanceToPlayer <= Player.ActiveDistance)// && !Cop.IsInHelicopter)//heli, dogs, boats come next?
        {

            PedExt MainTarget = PedToAttack();
            GameFiber.Yield();
            if (MainTarget != null)
            {
                SetAIApprehend(MainTarget);
            }
            else if (TryStartANPRTrafficStop())
            {
                // The registration mismatch has been detected; the traffic-stop task owns this cop.
            }
            else
            {
                if (Player.IsWanted && Cop.IsRespondingToWanted && (Cop.SawPlayerViolating || Player.PoliceResponse.WantedLevelHasBeenRadioedIn))
                {
                    if (Player.IsInSearchMode)
                    {
                        SetLocate();
                    }
                    else
                    {
                        if (Cop.DistanceToPlayer <= (Cop.IsInAirVehicle ? 400f : 200f))//150f))//200f
                        {
                            if (Player.PoliceResponse.IsDeadlyChase && !Player.IsAttemptingToSurrender)
                            {
                                SetKill();
                            }
                            else
                            {
                                SetChase();
                            }
                        }
                        else
                        {
                            SetLocate();
                        }
                    }
                }
                else if (Cop.PedAlerts.IsAlerted)// Cop.BodiesSeen.Any() || )
                {
                    SetInvestigate();
                }
                else if (Player.Investigation.IsActive && Player.Investigation.RequiresPolice && Cop.IsRespondingToInvestigation)// && Cop.IsIdleTaskable)
                {
                    SetInvestigate();
                }
                else if (World.CitizenWantedLevel > 0 && Cop.IsRespondingToCitizenWanted)
                {
                    SetInvestigate();
                }
                else if (Cop.IsMarshalTaskForceMember && !Cop.PlayerPerception.EverSeenTarget && Cop.ClosestDistanceToPlayer >= 20f && Player.CriminalHistory.HasHistory && Player.CriminalHistory.IsWithinMarshalDistance)
                {
                    SetMarshalLocate();
                }
                else if(Cop.WasModSpawned)
                {
                    SetIdle();
                }
            }
        }
        else if (Cop.WasModSpawned)
        {
            SetIdle();
        }
        Cop.GameTimeLastUpdatedTask = Game.GameTime;
    }
    private PedExt GetPlayerPedExt()
    {
        Mod.Player modPlayer = Player as Mod.Player;
        return modPlayer?.HealthState?.MyPed;
    }

    private bool TryStartANPRTrafficStop()
    {
        if (Player == null || Player.IsWanted || !Player.IsInVehicle || Player.CurrentVehicle == null ||
            !Player.CurrentVehicle.Vehicle.Exists() || !Cop.Pedestrian.Exists() || !Cop.IsDriver ||
            !Cop.IsInVehicle || !Cop.Pedestrian.CurrentVehicle.Exists())
        {
            return false;
        }

        if (Cop.DistanceToPlayer > 45f || !Cop.PlayerPerception.CanSeeTarget ||
            (Cop.CurrentTask != null && Cop.CurrentTask.Name != "Idle") ||
            Game.GameTime - Player.CurrentVehicle.GameTimeLastANPRScan < 15000)
        {
            return false;
        }

        bool isStationary = Player.CurrentVehicle.Vehicle.Speed <= 0.5f;
        bool isSuspicious = !string.IsNullOrEmpty(Player.CurrentVehicle.IsSuspicious(false)) ||
            (Player.Violations != null && Player.Violations.TrafficViolations != null &&
             Player.Violations.TrafficViolations.IsViolatingAnyTrafficLaws);

        if (!isStationary && !isSuspicious)
        {
            return false;
        }

        Player.CurrentVehicle.GameTimeLastANPRScan = Game.GameTime;

        string visiblePlate = Player.CurrentVehicle.Vehicle.LicensePlate;
        bool hasNoVisiblePlate = string.IsNullOrWhiteSpace(visiblePlate);
        bool hasNoRegistrationRecord = Player.CurrentVehicle.CarPlate == null ||
            Player.CurrentVehicle.OriginalLicensePlate == null;
        bool visiblePlateDoesNotMatchRecord = !hasNoRegistrationRecord &&
            !string.Equals(visiblePlate, Player.CurrentVehicle.CarPlate.PlateNumber, StringComparison.OrdinalIgnoreCase);
        bool registrationBelongsToAnotherVehicle = !Player.CurrentVehicle.HasOriginalPlate;
        bool registrationIsWanted = Player.CurrentVehicle.CarPlate != null && Player.CurrentVehicle.CarPlate.IsWanted;

        if (!hasNoVisiblePlate && !hasNoRegistrationRecord && !visiblePlateDoesNotMatchRecord &&
            !registrationBelongsToAnotherVehicle && !registrationIsWanted)
        {
            return false;
        }

        PedExt playerPed = GetPlayerPedExt();
        if (playerPed == null)
        {
            return false;
        }

        SetANPRTrafficStop(playerPed);
        EntryPoint.WriteToConsole(
            $"ANPR: Officer {Cop.Pedestrian.Handle} detected registration issue on vehicle {Player.CurrentVehicle.Handle}. " +
            $"NoPlate:{hasNoVisiblePlate} NoRecord:{hasNoRegistrationRecord} TextMismatch:{visiblePlateDoesNotMatchRecord} " +
            $"WrongVehicle:{registrationBelongsToAnotherVehicle} WantedPlate:{registrationIsWanted}");
        return true;
    }

    private void SetANPRTrafficStop(PedExt target)
    {
        if (Cop.CurrentTask is AIApprehend activeTrafficStop && activeTrafficStop.IsTrafficStop)
        {
            return;
        }

        Cop.CurrentTask = new AIApprehend(Cop, Player, Cop, Settings)
        {
            OtherTarget = target,
            IsTrafficStop = true,
            UseWantedLevel = false
        };
        Cop.WeaponInventory.Reset();
        GameFiber.Yield();
        Cop.CurrentTask.Start();
    }

    private PedExt PedToAttack()
    {
        if (Settings.SettingsManager.PerformanceSettings.CopGetPedToAttackDisable)
        {
            return null;
        }
        if (!World.Pedestrians.PossibleTargets.Any(x => x.IsWanted))
        {
            return null;
        }
        PedExt MainTarget = null;
        if (Cop.Pedestrian.Exists() && Cop.DistanceToPlayer <= 200f)
        {
            List<CopTarget> copsPossibleTargets = new List<CopTarget>();
            bool anyDeadlyChase = false;
            foreach (PedExt possibleTarget in World.Pedestrians.PossibleTargets.ToList())
            {
                int TotalOtherAssignedCops = World.Pedestrians.PoliceList.Count(x => x.Handle != Cop.Handle && x.CurrentTask != null && x.CurrentTask.OtherTarget != null && x.CurrentTask.OtherTarget.Handle == possibleTarget.Handle);
                //EntryPoint.WriteToConsole($"TASKER: {possibleTarget.Handle} TotalOtherAssignedCops {TotalOtherAssignedCops} WantedLevel {possibleTarget.WantedLevel}", 3);
                if (possibleTarget.Pedestrian.Exists() && possibleTarget.IsWanted && !possibleTarget.IsArrested && NativeHelper.IsNearby(Cop.CellX, Cop.CellY, possibleTarget.CellX, possibleTarget.CellY, 3))
                {
                    if (possibleTarget.IsDeadlyChase)
                    {
                        anyDeadlyChase = true;
                    }
                    copsPossibleTargets.Add(new CopTarget(possibleTarget, possibleTarget.Pedestrian.DistanceTo2D(Cop.Pedestrian)) { TotalAssignedCops = TotalOtherAssignedCops });
                }
            }
            int PlayerCops = World.Pedestrians.PoliceList.Count(x => x.Handle != Cop.Handle && x.CurrentTask != null && (x.CurrentTask.Name == "Kill" || x.CurrentTask.Name == "Chase"));


            //  if(Settings.SettingsManager.PerformanceSettings.CopGetPedToAttackYield1)
            // {
            GameFiber.Yield();
            // }

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
                    if (World.Pedestrians.ClosestCopToPlayer == null || Cop.Handle != World.Pedestrians.ClosestCopToPlayer.Handle)
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
                if (MaybeMainTarget != null && MaybeMainTarget.Target != null && (MaybeMainTarget.DistanceToTarget > Cop.DistanceToPlayer + 5f))//&& (PlayerCops < Player.WantedLevel || Player.WantedLevel >= 3))//removed for testing, want to allow cosp to engate targets close to them better
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


            if (MainTarget != null && MainTarget.IsBusted && MainTarget.Handle != Cop.CurrentTask?.OtherTarget?.Handle && World.Pedestrians.PoliceList.Any(x => x.Handle != Cop.Handle && x.CurrentTask?.OtherTarget?.Handle == MainTarget.Handle))
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
    private void SetInvestigate()
    {
        if (Cop.CurrentTask?.Name != "Investigate")
        {
            // EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Task Changed from {Cop.CurrentTask?.Name} to Investigate", 3);
            Cop.CurrentTask = new PoliceGeneralInvestigate(Cop, Cop, Player, World, null, PlacesOfInterest, Settings, Settings.SettingsManager.PoliceTaskSettings.BlockEventsDuringInvestigate, Cop, true);//Cop.CurrentTask = new Investigate(Cop, Player, Settings, World);
            Cop.WeaponInventory.Reset();
            GameFiber.Yield();//TR Added back 4
            Cop.CurrentTask.Start();
        }
    }
    protected override void SetIdle()
    {
        if (Cop.CurrentTask?.Name != "Idle")// && Cop.IsIdleTaskable)
        {
            Cop.CurrentTask = new CopGeneralIdle(Cop, Cop, Player, World, World.Vehicles.SimplePoliceVehicles, PlacesOfInterest, Settings, Settings.SettingsManager.PoliceTaskSettings.BlockEventsDuringIdle, true, Settings.SettingsManager.WorldSettings.AllowSettingSirenState, true);
            Cop.WeaponInventory.Reset();
            GameFiber.Yield();//TR Added back 4
            Cop.CurrentTask.Start();
        }
    }
    private void SetLocate()
    {
        if (Cop.CurrentTask?.Name != "Locate")
        {
            // EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Task Changed from {Cop.CurrentTask?.Name} to Locate", 3);
            //Cop.CurrentTask = new Locate(Cop, Player, Settings);

            if (Settings.SettingsManager.PoliceTaskSettings.UseLegacyLocateTasking)
            {
                Cop.CurrentTask = new Locate(Cop, Player, Settings);
            }
            else
            {
                Cop.CurrentTask = new PoliceGeneralLocate(Cop, Cop, Player, World, null, PlacesOfInterest, Settings, Settings.SettingsManager.PoliceTaskSettings.BlockEventsDuringLocate, Cop, HasSixthSense());
            }
            Cop.WeaponInventory.Reset();
            GameFiber.Yield();//TR Added back 4
            Cop.CurrentTask.Start();
        }
    }
    private void SetMarshalLocate()
    {
        if (Cop.CurrentTask?.Name != "Locate")
        {
            Cop.CurrentTask = new MarshalGeneralLocate(Cop, Cop, Player, World, null, PlacesOfInterest, Settings, Settings.SettingsManager.PoliceTaskSettings.BlockEventsDuringLocate, Cop);
            Cop.WeaponInventory.Reset();
            GameFiber.Yield();//TR Added back 4
            Cop.CurrentTask.Start();
        }
    }
    private void SetChase()
    {
        if (Cop.CurrentTask?.Name != "Chase")
        {
            //EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Task Changed from {Cop.CurrentTask?.Name} to Chase", 3);
            Cop.CurrentTask = new Chase(Cop, Player, World, Cop, Settings); //new NewChase(Cop, Player, World, Cop, Settings);
            Cop.WeaponInventory.Reset();
            GameFiber.Yield();//TR Added back 4
            Cop.CurrentTask.Start();
        }
    }
    private void SetKill()
    {
        if (Cop.CurrentTask?.Name != "Kill")
        {
            //EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Task Changed from {Cop.CurrentTask?.Name} to Kill", 3);
            Cop.CurrentTask = new Kill(Cop,Cop,World, Player, Settings);
            Cop.WeaponInventory.Reset();
            GameFiber.Yield();//TR Added back 4
            Cop.CurrentTask.Start();
        }
    }
    private void SetAIApprehend(PedExt MainTarget)
    {
        if (Cop.CurrentTask?.Name != "AIApprehend")
        {
            // EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Task Changed from {Cop.CurrentTask?.Name} to AIApprehend", 3);
            Cop.CurrentTask = new AIApprehend(Cop, Player, Cop, Settings) { OtherTarget = MainTarget };
            Cop.WeaponInventory.Reset();
            GameFiber.Yield();//TR Added back 4
            Cop.CurrentTask.Start();
        }
    }
    private bool HasSixthSense()
    {
        bool HasSixthSense = RandomItems.RandomPercent(Cop.IsInHelicopter ? Settings.SettingsManager.PoliceTaskSettings.SixthSenseHelicopterPercentage : Settings.SettingsManager.PoliceTaskSettings.SixthSensePercentage);
        if (!HasSixthSense && Cop.DistanceToPlayer <= 40f && RandomItems.RandomPercent(Settings.SettingsManager.PoliceTaskSettings.SixthSensePercentageClose))
        {
            HasSixthSense = true;
        }
        return HasSixthSense;
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