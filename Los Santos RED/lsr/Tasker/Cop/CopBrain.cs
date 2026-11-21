using LosSantosRED.lsr;
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
    private VehicleExt ANPRScanVehicle;
    private uint GameTimeANPRScanStarted;
    private Vector3 ANPRScanStartPosition;
    private float ANPRScanStartSpeed;
    private string ANPRScanPlateNumber;
    private bool ANPRScanVehicleWasRecognized;
    private const uint ANPRScanDuration = 2500;
    private uint GameTimeLastBOLOCheck;

    private DateTime? LastSeenPlayerAt;
    private VehicleExt LastSeenVehicle;
    private Dictionary<VehicleExt, DateTime> LastSeenVehicles = new Dictionary<VehicleExt, DateTime>();
    private bool WasSeeingPlayer;
    private bool WasSeeingVehicle;
    private bool MemoryRecognizedPlayer;
    private bool MemoryRecognizedVehicle;
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
        UpdateRecognitionMemory();
        UpdateANPRScan();
        if (Cop.PlayerPerception.CanSeeTarget && Player != null)
        {
            Mod.Player modPlayer = Player as Mod.Player;
            if (modPlayer?.LawEnforcementIntelligence != null)
            {
                VehicleExt observedVehicle = Player.IsInVehicle ? Player.CurrentVehicle : null;
                bool hasObservedVehicle = observedVehicle != null && observedVehicle.Vehicle.Exists();
                string vehicleID = null;
                string vehicleDescription = null;
                if (hasObservedVehicle)
                {
                    string plate = observedVehicle.Vehicle.LicensePlate;
                    vehicleID = observedVehicle.CarPlate != null ? "REG:" + observedVehicle.CarPlate.RegistrationId :
                        "MODEL:" + observedVehicle.Vehicle.Model.Hash + ":PLATE:" + (plate ?? "");
                    vehicleDescription = observedVehicle.FullName(false);
                }
                modPlayer.LawEnforcementIntelligence.RecordPoliceSighting(modPlayer.CurrentDateTime,
                    Cop.Pedestrian.Handle.ToString(), hasObservedVehicle, vehicleID, vehicleDescription,
                    Player.Position.X, Player.Position.Y, Player.Position.Z);
            }
        }
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
                        PedExt otherTarget = Cop.CurrentTask is AIApprehend activeApprehend &&
                            (activeApprehend.IsTrafficStop || activeApprehend.IsANPREvasionPursuit || activeApprehend.IsWarrantApprehension)
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
        if (Cop.CurrentTask is AIApprehend activeApprehend &&
            (activeApprehend.IsTrafficStop || activeApprehend.IsANPREvasionPursuit || activeApprehend.IsWarrantApprehension))
        {
            return;
        }
        if (Cop.DistanceToPlayer <= Player.ActiveDistance)// && !Cop.IsInHelicopter)//heli, dogs, boats come next?
        {

            Mod.Player warrantPlayer = Player as Mod.Player;
            WarrantRecord activeArrestWarrant = Settings.SettingsManager.LawEnforcementIntelligenceSettings.EnableWarrants
                ? warrantPlayer?.LawEnforcementIntelligenceManager?.WarrantManager?.FindActive(
                    warrantPlayer.LawEnforcementIntelligence, Cop.AssignedAgency?.ID,
                    warrantPlayer.CurrentLocation?.CurrentZone?.InternalGameName)
                : null;
            PedExt MainTarget = PedToAttack();
            GameFiber.Yield();
            if (MainTarget != null)
            {
                if (activeArrestWarrant != null && warrantPlayer != null &&
                    MainTarget.Handle == warrantPlayer.Character.Handle)
                    warrantPlayer.BeginWarrantApprehension(activeArrestWarrant.ID);
                SetAIApprehend(MainTarget);
            }
            else if (TryStartWarrantApprehend())
            {
                // A visible subject with an active arrest-authority warrant is being apprehended.
            }
            else if (TryStartANPRTrafficStop())
            {
                // The registration mismatch has been detected; the traffic-stop task owns this cop.
            }
            else if (TryStartBOLOTrafficStop())
            {
                // An active vehicle or plate BOLO has prompted a probabilistic traffic stop.
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


    private bool TryStartBOLOTrafficStop()
    {
        if (Game.GameTime - GameTimeLastBOLOCheck < 10000)
            return false;
        if (!Settings.SettingsManager.LawEnforcementIntelligenceSettings.EnableBOLOSystem ||
            Player == null || Player.IsWanted || !Player.IsInVehicle || Player.CurrentVehicle == null ||
            !Player.CurrentVehicle.Vehicle.Exists() || !Cop.Pedestrian.Exists() || !Cop.IsDriver ||
            !Cop.IsInVehicle || !Cop.PlayerPerception.CanSeeTarget)
            return false;

        Mod.Player modPlayer = Player as Mod.Player;
        if (modPlayer?.LawEnforcementIntelligence == null || !Player.CurrentVehicle.Vehicle.Driver.Exists() ||
            Player.CurrentVehicle.Vehicle.Driver.Handle != modPlayer.Character.Handle)
            return false;

        GameTimeLastBOLOCheck = Game.GameTime;
        string zoneID = modPlayer.CurrentLocation?.CurrentZone?.InternalGameName;
        string plateNumber = Player.CurrentVehicle.Vehicle.LicensePlate;
        float stopChance = modPlayer.LawEnforcementIntelligenceManager?.BOLOManager?.GetVehicleStopChance(
            modPlayer.LawEnforcementIntelligence, modPlayer.CurrentDateTime, zoneID,
            plateNumber, Player.CurrentVehicle.FullName(false)) ?? 0f;
        if (stopChance <= 0.0f || !RandomItems.RandomPercent(stopChance * 100.0f))
            return false;

        PedExt target = GetPlayerPedExt();
        if (target == null)
            return false;

        SetBOLOTrafficStop(target);
        return true;
    }

    private void SetBOLOTrafficStop(PedExt target)
    {
        if (Cop.CurrentTask is AIApprehend activeStop && activeStop.IsTrafficStop)
            return;
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

    private bool TryStartANPRTrafficStop()
    {
        if (ANPRScanVehicle != null)
        {
            return true;
        }

        if (Player == null || Player.IsWanted || !Player.IsInVehicle || Player.CurrentVehicle == null ||
            !Player.CurrentVehicle.Vehicle.Exists() || !Cop.Pedestrian.Exists() || !Cop.IsDriver ||
            !Cop.IsInVehicle || !Cop.Pedestrian.CurrentVehicle.Exists())
        {
            return false;
        }

        VehicleExt vehicle = Player.CurrentVehicle;
        Vector3 directionToCop = (Cop.Pedestrian.Position - vehicle.Vehicle.Position).ToNormalized();
        bool copIsBehindPlayer = Vector3.Dot(directionToCop, vehicle.Vehicle.ForwardVector) < 0f;
        if (Cop.DistanceToPlayer > 45f || !Cop.PlayerPerception.CanSeeTarget || !copIsBehindPlayer ||
            (Cop.CurrentTask != null && Cop.CurrentTask.Name != "Idle") ||
            Game.GameTime - vehicle.GameTimeLastANPRScan < 15000)
        {
            return false;
        }

        bool isStationary = vehicle.Vehicle.Speed <= 0.5f;
        bool isSuspicious = !string.IsNullOrEmpty(vehicle.IsSuspicious(false)) ||
            (Player.Violations != null && Player.Violations.TrafficViolations != null &&
             Player.Violations.TrafficViolations.IsViolatingAnyTrafficLaws);
        bool recognizedFlaggedVehicle = MemoryRecognizedVehicle && vehicle.HasBeenSeenByPoliceDuringWanted;

        if (!isStationary && !isSuspicious && !recognizedFlaggedVehicle)
        {
            return false;
        }

        PedExt playerPed = GetPlayerPedExt();
        if (playerPed == null)
        {
            return false;
        }

        ANPRScanVehicle = vehicle;
        ANPRScanPlateNumber = vehicle.Vehicle.LicensePlate;
        ANPRScanStartPosition = vehicle.Vehicle.Position;
        ANPRScanStartSpeed = vehicle.Vehicle.Speed;
        ANPRScanVehicleWasRecognized = recognizedFlaggedVehicle;
        GameTimeANPRScanStarted = Game.GameTime;
        vehicle.GameTimeLastANPRScan = Game.GameTime;

        Mod.Player modPlayer = Player as Mod.Player;
        if (modPlayer != null && modPlayer.Inventory.Has(typeof(RadioItem)) &&
            Settings.SettingsManager.ScannerSettings.EnableNotifications)
        {
            new GTANotification(
                "CHAR_LS_TOURIST_BOARD",
                "CHAR_LS_TOURIST_BOARD",
                "Police Scanner",
                "~r~ANPR Alert",
                "An officer behind you is running your plate...").Display();
        }

        EntryPoint.WriteToConsole(
            $"ANPR: Officer {Cop.Pedestrian.Handle} started scanning vehicle {vehicle.Handle}, plate '{ANPRScanPlateNumber}'.");
        return true;
    }

    private void UpdateANPRScan()
    {
        if (ANPRScanVehicle == null)
        {
            return;
        }

        if (!Cop.Pedestrian.Exists() || !Cop.Pedestrian.IsAlive || !ANPRScanVehicle.Vehicle.Exists())
        {
            ClearANPRScan();
            return;
        }

        float dx = ANPRScanVehicle.Vehicle.Position.X - ANPRScanStartPosition.X;
        float dy = ANPRScanVehicle.Vehicle.Position.Y - ANPRScanStartPosition.Y;
        float dz = ANPRScanVehicle.Vehicle.Position.Z - ANPRScanStartPosition.Z;
        float distanceMoved = (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
        float currentSpeed = ANPRScanVehicle.Vehicle.Speed;

        bool changedToAnotherVehicle = Player == null ||
            (Player.IsInVehicle && Player.CurrentVehicle != ANPRScanVehicle);
        float playerDx = Player == null ? 0f : Player.Position.X - ANPRScanStartPosition.X;
        float playerDy = Player == null ? 0f : Player.Position.Y - ANPRScanStartPosition.Y;
        float playerDz = Player == null ? 0f : Player.Position.Z - ANPRScanStartPosition.Z;
        float playerDistanceFromScan = (float)Math.Sqrt(playerDx * playerDx + playerDy * playerDy + playerDz * playerDz);
        bool ranAwayOnFoot = Player != null && !Player.IsInVehicle && playerDistanceFromScan >= 15f;
        bool acceleratedAway = currentSpeed >= 10f &&
            (ANPRScanStartSpeed <= 3f || currentSpeed - ANPRScanStartSpeed >= 6f) &&
            distanceMoved >= 12f;
        bool droveAwayFromStoppedPosition = ANPRScanStartSpeed <= 3f && distanceMoved >= 25f && currentSpeed >= 5f;
        bool fledDuringScan = changedToAnotherVehicle || ranAwayOnFoot ||
            acceleratedAway || droveAwayFromStoppedPosition;

        if (fledDuringScan)
        {
            string plateToFlag = ANPRScanPlateNumber;
            LicensePlate.RegisterWantedPlateNumber(plateToFlag);
            if (ANPRScanVehicle.CarPlate != null &&
                string.Equals(ANPRScanVehicle.CarPlate.PlateNumber, plateToFlag, StringComparison.OrdinalIgnoreCase))
            {
                ANPRScanVehicle.CarPlate.IsWanted = true;
            }
            ANPRScanVehicle.HasBeenSeenByPoliceDuringWanted = true;

            EntryPoint.WriteToConsole(
                $"ANPR: Player evaded plate scan. Vehicle {ANPRScanVehicle.Handle}, displayed plate '{plateToFlag}' marked wanted and vehicle marked known to police.");

            PedExt playerPed = GetPlayerPedExt();
            ClearANPRScan();
            if (playerPed != null)
            {
                SetANPREvasionPursuit(playerPed);
            }
            return;
        }

        if (Game.GameTime - GameTimeANPRScanStarted < ANPRScanDuration)
        {
            return;
        }

        string visiblePlate = ANPRScanPlateNumber;
        bool hasNoVisiblePlate = string.IsNullOrWhiteSpace(visiblePlate);
        bool hasNoRegistrationRecord = ANPRScanVehicle.CarPlate == null ||
            ANPRScanVehicle.OriginalLicensePlate == null;
        bool visiblePlateDoesNotMatchRecord = !hasNoRegistrationRecord &&
            !string.Equals(visiblePlate, ANPRScanVehicle.CarPlate.PlateNumber, StringComparison.OrdinalIgnoreCase);
        bool registrationBelongsToAnotherVehicle = !ANPRScanVehicle.HasOriginalPlate;
        bool registrationIsWanted = LicensePlate.IsPlateNumberWanted(visiblePlate) ||
            (ANPRScanVehicle.CarPlate != null && ANPRScanVehicle.CarPlate.IsWanted);

        bool hasRegistrationIssue = hasNoVisiblePlate || hasNoRegistrationRecord ||
            visiblePlateDoesNotMatchRecord || registrationBelongsToAnotherVehicle ||
            registrationIsWanted || ANPRScanVehicleWasRecognized;

        VehicleExt scannedVehicle = ANPRScanVehicle;
        bool recognizedVehicle = ANPRScanVehicleWasRecognized;
        Mod.Player observingPlayer = Player as Mod.Player;
        if (observingPlayer?.LawEnforcementIntelligence != null)
        {
            observingPlayer.LawEnforcementIntelligence.RecordANPRRead(observingPlayer.CurrentDateTime,
                Cop.Pedestrian.Handle.ToString(), visiblePlate, scannedVehicle.CarPlate?.RegistrationId,
                scannedVehicle.FullName(false), scannedVehicle.Vehicle.Position.X,
                scannedVehicle.Vehicle.Position.Y, scannedVehicle.Vehicle.Position.Z);
        }
        ClearANPRScan();

        if (hasRegistrationIssue)
        {
            PedExt playerPed = GetPlayerPedExt();
            if (playerPed != null)
            {
                SetANPRTrafficStop(playerPed);
            }
            EntryPoint.WriteToConsole(
                $"ANPR: Officer {Cop.Pedestrian.Handle} completed scan and detected registration issue on vehicle {scannedVehicle.Handle}. " +
                $"NoPlate:{hasNoVisiblePlate} NoRecord:{hasNoRegistrationRecord} TextMismatch:{visiblePlateDoesNotMatchRecord} " +
                $"WrongVehicle:{registrationBelongsToAnotherVehicle} WantedPlate:{registrationIsWanted} RecognizedVehicle:{recognizedVehicle}");
        }
        else
        {
            EntryPoint.WriteToConsole($"ANPR: Officer {Cop.Pedestrian.Handle} completed scan; registration '{visiblePlate}' is valid.");
        }
    }

    private void ClearANPRScan()
    {
        ANPRScanVehicle = null;
        GameTimeANPRScanStarted = 0;
        ANPRScanStartPosition = Vector3.Zero;
        ANPRScanStartSpeed = 0f;
        ANPRScanPlateNumber = null;
        ANPRScanVehicleWasRecognized = false;
    }

    private void UpdateRecognitionMemory()
    {
        Mod.Player modPlayer = Player as Mod.Player;
        if (modPlayer == null || !Settings.SettingsManager.PoliceTaskSettings.EnablePoliceRecognitionMemory)
        {
            MemoryRecognizedPlayer = false;
            MemoryRecognizedVehicle = false;
            Cop.PlayerPerception.SetRecognizedFromMemory(false);
            WasSeeingPlayer = false;
            WasSeeingVehicle = false;
            return;
        }

        DateTime now = modPlayer.CurrentDateTime;
        bool canSeePlayer = Cop.PlayerPerception.CanSeeTarget;

        if (canSeePlayer)
        {
            if (!WasSeeingPlayer && LastSeenPlayerAt.HasValue)
            {
                MemoryRecognizedPlayer = RollRecognition(LastSeenPlayerAt.Value, now);
            }
            else if (!WasSeeingPlayer)
            {
                MemoryRecognizedPlayer = false;
            }

            // A person BOLO raises identification probability on a fresh visual encounter.
            // It does not itself grant arrest authority; arrest still requires an applicable warrant
            // or another existing lawful basis handled by the normal police response flow.
            if (!WasSeeingPlayer)
            {
                Mod.Player boloPlayer = Player as Mod.Player;
                float personBoloChance = Settings.SettingsManager.LawEnforcementIntelligenceSettings.EnableBOLOSystem
                    ? boloPlayer?.LawEnforcementIntelligenceManager?.BOLOManager?.GetPersonRecognitionChance(boloPlayer.LawEnforcementIntelligence, now,
                        boloPlayer.LawEnforcementIntelligence?.SubjectID,
                        boloPlayer.CurrentLocation?.CurrentZone?.InternalGameName) ?? 0f
                    : 0f;
                if (personBoloChance > 0f && RandomItems.RandomPercent(personBoloChance * 100f))
                    MemoryRecognizedPlayer = true;
            }
            LastSeenPlayerAt = now;
        }
        else
        {
            MemoryRecognizedPlayer = false;
        }
        WasSeeingPlayer = canSeePlayer;
        Cop.PlayerPerception.SetRecognizedFromMemory(MemoryRecognizedPlayer);

        VehicleExt currentVehicle = canSeePlayer && Player.IsInVehicle ? Player.CurrentVehicle : null;
        bool canSeeVehicle = currentVehicle != null && currentVehicle.Vehicle.Exists();
        if (canSeeVehicle)
        {
            bool newVehicleEncounter = !WasSeeingVehicle || LastSeenVehicle != currentVehicle;
            if (newVehicleEncounter)
            {
                DateTime previousVehicleSighting;
                MemoryRecognizedVehicle = LastSeenVehicles.TryGetValue(currentVehicle, out previousVehicleSighting) &&
                    RollRecognition(previousVehicleSighting, now);
            }

            LastSeenVehicle = currentVehicle;
            LastSeenVehicles[currentVehicle] = now;

            // Keep the per-officer vehicle memory bounded.
            if (LastSeenVehicles.Count > 64)
            {
                foreach (VehicleExt oldestVehicle in LastSeenVehicles
                    .OrderBy(x => x.Value)
                    .Take(LastSeenVehicles.Count - 64)
                    .Select(x => x.Key)
                    .ToList())
                {
                    LastSeenVehicles.Remove(oldestVehicle);
                }
            }
        }
        else
        {
            MemoryRecognizedVehicle = false;
        }
        WasSeeingVehicle = canSeeVehicle;
    }

    private bool RollRecognition(DateTime lastSeenAt, DateTime now)
    {
        TimeSpan elapsed = now >= lastSeenAt ? now - lastSeenAt : TimeSpan.Zero;
        float chance = GetRecognitionChance(elapsed.TotalHours);
        return RandomItems.RandomPercent(chance);
    }

    private float GetRecognitionChance(double elapsedHours)
    {
        PoliceTaskSettings recognitionSettings = Settings.SettingsManager.PoliceTaskSettings;
        if (elapsedHours <= 1.0)
        {
            return Lerp(recognitionSettings.PoliceRecognitionChanceInitial,
                recognitionSettings.PoliceRecognitionChanceAfter1Hour, elapsedHours);
        }
        if (elapsedHours <= 6.0)
        {
            return Lerp(recognitionSettings.PoliceRecognitionChanceAfter1Hour,
                recognitionSettings.PoliceRecognitionChanceAfter6Hours, (elapsedHours - 1.0) / 5.0);
        }
        if (elapsedHours <= 12.0)
        {
            return Lerp(recognitionSettings.PoliceRecognitionChanceAfter6Hours,
                recognitionSettings.PoliceRecognitionChanceAfter12Hours, (elapsedHours - 6.0) / 6.0);
        }
        if (elapsedHours <= 24.0)
        {
            return Lerp(recognitionSettings.PoliceRecognitionChanceAfter12Hours,
                recognitionSettings.PoliceRecognitionChanceAfter24Hours, (elapsedHours - 12.0) / 12.0);
        }
        if (elapsedHours <= 72.0)
        {
            return Lerp(recognitionSettings.PoliceRecognitionChanceAfter24Hours,
                recognitionSettings.PoliceRecognitionChanceAfter3Days, (elapsedHours - 24.0) / 48.0);
        }
        if (elapsedHours <= 168.0)
        {
            return Lerp(recognitionSettings.PoliceRecognitionChanceAfter3Days,
                recognitionSettings.PoliceRecognitionChanceAfter7Days, (elapsedHours - 72.0) / 96.0);
        }
        return recognitionSettings.PoliceRecognitionChanceAfter7Days;
    }

    private float Lerp(float start, float end, double amount)
    {
        float result = start + (end - start) * (float)Math.Max(0.0, Math.Min(1.0, amount));
        return Math.Max(0f, Math.Min(100f, result));
    }
    private void SetANPREvasionPursuit(PedExt target)
    {
        if (Cop.CurrentTask is AIApprehend activeApprehend && activeApprehend.IsANPREvasionPursuit)
        {
            return;
        }

        Cop.CurrentTask = new AIApprehend(Cop, Player, Cop, Settings)
        {
            OtherTarget = target,
            IsANPREvasionPursuit = true,
            UseWantedLevel = false
        };
        Cop.WeaponInventory.Reset();
        GameFiber.Yield();
        Cop.CurrentTask.Start();
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
    private bool TryStartWarrantApprehend()
    {
        Mod.Player modPlayer = Player as Mod.Player;
        WarrantRecord warrant = Settings.SettingsManager.LawEnforcementIntelligenceSettings.EnableWarrants
            ? modPlayer?.LawEnforcementIntelligenceManager?.WarrantManager?.FindActive(
                modPlayer.LawEnforcementIntelligence, Cop.AssignedAgency?.ID,
                modPlayer.CurrentLocation?.CurrentZone?.InternalGameName)
            : null;
        if (warrant == null || !Cop.PlayerPerception.CanSeeTarget)
            return false;
        PedExt target = GetPlayerPedExt();
        if (target == null || !target.Pedestrian.Exists())
            return false;
        modPlayer.BeginWarrantApprehension(warrant.ID);
        SetWarrantApprehend(target);
        return true;
    }

    private void SetWarrantApprehend(PedExt target)
    {
        if (Cop.CurrentTask is AIApprehend existing && existing.IsWarrantApprehension)
            return;
        AIApprehend task = new AIApprehend(Cop, Player, Cop, Settings)
        {
            OtherTarget = target,
            UseWantedLevel = false,
            IsWarrantApprehension = true
        };
        Cop.CurrentTask = task;
        Cop.WeaponInventory.Reset();
        GameFiber.Yield();
        task.Start();
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