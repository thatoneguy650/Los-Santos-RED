using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Locations;
using LosSantosRED.lsr.Player;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Mod
{
    public class Player : IActivityPerformable, IIntoxicatable, ITargetable, IPoliceRespondable, IInputable, IPedSwappable, IMuggable, IRespawnable, IViolateable, IWeaponDroppable, IDisplayable, ICarStealable, IPlateChangeable, IActionable, IInteractionable, IInventoryable
    {
        private CriminalHistory CriminalHistory;
        private DynamicActivity DynamicActivity;
        private SearchMode SearchMode;
        private Surrendering Surrendering;
        private WeaponDropping WeaponDropping;
        private HealthState HealthState;
        private Inventory Inventory;

        private IRadioStations RadioStations;
        private ISettingsProvideable Settings;
        private ITimeControllable TimeControllable;
        private IEntityProvideable EntityProvider;
        private IWeapons Weapons;
        private IScenarios Scenarios;

        private uint GameTimeLastBusted;
        private uint GameTimeLastDied;
        private uint GameTimeLastMoved;
        private uint GameTimeLastMovedFast;
        private uint GameTimeLastShot;
        private uint GameTimeStartedHotwiring;
        private uint GameTimeStartedPlaying;
        private bool isAiming;
        private bool isAimingInVehicle;
        private bool isGettingIntoVehicle;
        private bool isHotwiring;
        private bool isInVehicle;
        private bool isCurrentVehicleEngineOn;
        private int PreviousWantedLevel;
        private uint targettingHandle;
        private bool isActive = true;
        private uint GameTimeLastUpdatedLookedAtPed;
        public Player(string modelName, bool isMale, string suspectsName, int currentMoney, IEntityProvideable provider, ITimeControllable timeControllable, IStreets streets, IZones zones, ISettingsProvideable settings, IWeapons weapons, IRadioStations radioStations, IScenarios scenarios)
        {
            ModelName = modelName;
            IsMale = isMale;
            SuspectsName = suspectsName;
            if (currentMoney != 0)
            {
                SetMoney(currentMoney);
            }
            EntityProvider = provider;
            TimeControllable = timeControllable;
            Settings = settings;
            Weapons = weapons;
            RadioStations = radioStations;
            Scenarios = scenarios;

            GameTimeStartedPlaying = Game.GameTime;

            HealthState = new HealthState(new PedExt(Game.LocalPlayer.Character));
            CurrentLocation = new LocationData(Game.LocalPlayer.Character, streets, zones);
            WeaponDropping = new WeaponDropping(this, Weapons);
            Surrendering = new Surrendering(this);
            Violations = new Violations(this, TimeControllable);
            Investigation = new Investigation(this);
            CriminalHistory = new CriminalHistory(this);
            PoliceResponse = new PoliceResponse(this);
            SearchMode = new SearchMode(this);
            Inventory = new Inventory(this);
        }
        public Investigation Investigation { get; private set; }
        public Interaction Interaction { get; private set; }
        public PoliceResponse PoliceResponse { get; private set; }
        public Violations Violations { get; private set; }
        public float ActiveDistance => Investigation.IsActive ? Investigation.Distance : 400f + (WantedLevel * 200f);
        public bool AnyHumansNear => EntityProvider.PoliceList.Any(x => x.DistanceToPlayer <= 10f) || EntityProvider.CivilianList.Any(x => x.DistanceToPlayer <= 10f);
        public bool AnyPoliceCanHearPlayer { get; set; }
        public bool AnyPoliceCanRecognizePlayer { get; set; }
        public bool AnyPoliceCanSeePlayer { get; set; }
        public bool AnyPoliceRecentlySeenPlayer { get; set; }
        public bool AnyPoliceSeenPlayerCurrentWanted { get; set; }
        public string AutoTuneStation { get; set; } = "NONE";
        public bool BeingArrested { get; private set; }
        public List<ButtonPrompt> ButtonPrompts { get; private set; } = new List<ButtonPrompt>();
        public bool CanConverse => !IsGettingIntoAVehicle && !IsBreakingIntoCar && !IsIncapacitated && !IsVisiblyArmed && IsAliveAndFree && !IsMovingDynamically;
        public bool CanConverseWithLookedAtPed => CurrentLookedAtPed != null && CurrentTargetedPed == null && CurrentLookedAtPed.CanConverse && CanConverse && (Relationship)NativeFunction.Natives.GET_RELATIONSHIP_BETWEEN_PEDS<int>(CurrentLookedAtPed.Pedestrian, Character) != Relationship.Hate;
        public bool CanDropWeapon => CanPerformActivities && WeaponDropping.CanDropWeapon;
        public bool CanHoldUpTargettedPed => CurrentTargetedPed != null && CurrentTargetedPed.CanBeMugged && !IsGettingIntoAVehicle && !IsBreakingIntoCar && !IsStunned && !IsRagdoll && IsVisiblyArmed && IsAliveAndFree && CurrentTargetedPed.DistanceToPlayer <= 7f;
        public bool CanPerformActivities => !IsMovingFast && !IsIncapacitated && !IsDead && !IsBusted && !IsInVehicle && !IsGettingIntoAVehicle && !IsAttemptingToSurrender && !IsMovingDynamically;
        public bool CanSurrender => Surrendering.CanSurrender;
        public bool CanUndie => TimesDied < Settings.SettingsManager.General.UndieLimit || Settings.SettingsManager.General.UndieLimit == 0;
        public Ped Character => Game.LocalPlayer.Character;
        public List<Crime> CivilianReportableCrimesViolating => Violations.CivilianReportableCrimesViolating;
        public LocationData CurrentLocation { get; set; }
        public PedExt CurrentLookedAtPed { get; private set; }
        public VehicleExt CurrentSeenVehicle => CurrentVehicle == null ? VehicleGettingInto : CurrentVehicle;
        public WeaponInformation CurrentSeenWeapon => !IsInVehicle ? CurrentWeapon : null;
        public string CurrentSpeedDisplay { get; private set; }
        public PedExt CurrentTargetedPed { get; private set; }
        public VehicleExt CurrentVehicle { get; private set; }
        public WeaponInformation CurrentWeapon { get; private set; }
        public WeaponCategory CurrentWeaponCategory => CurrentWeapon != null ? CurrentWeapon.Category : WeaponCategory.Unknown;
        public WeaponHash CurrentWeaponHash { get; set; }
        public string CurrentModelName { get; set; }
        public PedVariation CurrentModelVariation { get; set; }
        public bool DiedInVehicle { get; private set; }
        public bool HandsAreUp { get; set; }
        public float IntoxicatedIntensity { get; set; }
        public bool IsAiming
        {
            get => isAiming;
            private set
            {
                if (isAiming != value)
                {
                    isAiming = value;
                    IsAimingChanged();
                }
            }
        }
        public bool IsAimingInVehicle
        {
            get => isAimingInVehicle;
            private set
            {
                if (isAimingInVehicle != value)
                {
                    isAimingInVehicle = value;
                    IsAimingInVehicleChanged();
                }
            }
        }
        public bool IsAliveAndFree => !IsBusted && !IsDead;
        public bool IsAttemptingToSurrender => HandsAreUp && !PoliceResponse.IsWeaponsFree;
        public bool IsBreakingIntoCar => IsCarJacking || IsLockPicking || IsHotWiring || Game.LocalPlayer.Character.IsJacking;
        public bool IsBustable => IsAliveAndFree && PoliceResponse.HasBeenWantedFor >= 3000 && !Surrendering.IsCommitingSuicide && !RecentlyBusted && !IsInVehicle && !PoliceResponse.IsWeaponsFree && (IsIncapacitated || (!IsMoving && !IsMovingDynamically));
        public bool IsBusted { get; private set; }
        public bool IsCarJacking { get; set; }
        public bool IsChangingLicensePlates { get; set; }
        public bool IsCommitingSuicide { get; set; }
        public bool IsConversing { get; set; }
        public bool IsDead { get; private set; }
        public bool IsGettingIntoAVehicle
        {
            get => isGettingIntoVehicle;
            private set
            {
                if (isGettingIntoVehicle != value)
                {
                    isGettingIntoVehicle = value;
                    IsGettingIntoVehicleChanged();
                }
            }
        }
        public bool IsHoldingEnter { get; set; }
        public bool IsMoveControlPressed { get; set; }
        public bool IsHoldingUp { get; set; }
        public bool IsHotWiring { get; private set; }
        public bool IsInAirVehicle { get; private set; }
        public bool IsInAutomobile { get; private set; }
        public bool IsIncapacitated => IsStunned || IsRagdoll;
        public bool IsInSearchMode { get; set; }
        public bool IsInteracting => IsConversing || IsHoldingUp;
        public bool IsIntoxicated { get; set; }
        public bool IsInVehicle
        {
            get => isInVehicle;
            private set
            {
                if (isInVehicle != value)
                {
                    isInVehicle = value;
                    IsInVehicleChanged();
                }
            }
        }
        public bool IsLockPicking { get; set; }
        public bool IsMale { get; set; }
        public bool IsMobileRadioEnabled { get; private set; }
        public bool IsMoving => GameTimeLastMoved != 0 && Game.GameTime - GameTimeLastMoved <= 2000;
        public bool IsMovingDynamically { get; private set; }
        public bool IsMovingFast => GameTimeLastMovedFast != 0 && Game.GameTime - GameTimeLastMovedFast <= 2000;
        public bool IsNearScenario { get; private set; }

        public Scenario ClosestScenario { get; private set; }

        public bool IsNotWanted => Game.LocalPlayer.WantedLevel == 0;
        public bool IsOffroad => CurrentLocation.IsOffroad;
        public bool IsOnMotorcycle { get; private set; }
        public bool IsPerformingActivity { get; set; }
        public bool IsRagdoll { get; private set; }
        public bool IsSpeeding => Violations.IsSpeeding;
        public bool IsStill { get; private set; }
        public bool IsStunned { get; private set; }
        public bool IsViolatingAnyAudioBasedCivilianReportableCrime => Violations.IsViolatingAnyAudioBasedCivilianReportableCrime;
        public bool IsViolatingAnyCivilianReportableCrime => Violations.IsViolatingAnyCivilianReportableCrime;
        public bool IsViolatingAnyTrafficLaws => Violations.IsViolatingAnyTrafficLaws;
        public bool IsVisiblyArmed { get; private set; }
        public bool IsWanted => Game.LocalPlayer.WantedLevel > 0;
        public WeaponHash LastWeaponHash { get; set; }
        public int MaxWantedLastLife { get; set; }
        public string ModelName { get; set; }
        public int Money
        {
            get
            {
                int CurrentCash;
                unsafe
                {
                    NativeFunction.CallByName<int>("STAT_GET_INT", NativeHelper.CashHash(Settings.SettingsManager.General.MainCharacterToAlias), &CurrentCash, -1);
                }
                return CurrentCash;
            }
        }
        public Vector3 PlacePoliceLastSeenPlayer { get; set; }
        public bool PoliceRecentlyNoticedVehicleChange { get; set; }
        public Vector3 Position => Game.LocalPlayer.Character.Position;
        public bool RecentlyAppliedWantedStats => CriminalHistory.RecentlyAppliedWantedStats;
        public bool RecentlyBusted => GameTimeLastBusted != 0 && Game.GameTime - GameTimeLastBusted <= 5000;
        public bool RecentlyDied => GameTimeLastDied != 0 && Game.GameTime - GameTimeLastDied <= 5000;
        public bool RecentlyShot => GameTimeLastShot != 0 && RecentlyStartedPlaying && Game.GameTime - GameTimeLastShot <= 3000;
        public bool RecentlyStartedPlaying => Game.GameTime - GameTimeStartedPlaying <= 10000;//15000
        public List<VehicleExt> ReportedStolenVehicles => TrackedVehicles.Where(x => x.NeedsToBeReportedStolen).ToList();
        public List<LicensePlate> SpareLicensePlates { get; private set; } = new List<LicensePlate>();
        public bool StarsRecentlyActive => SearchMode.StarsRecentlyActive;
        public bool StarsRecentlyGreyedOut => SearchMode.StarsRecentlyGreyedOut;
        public string SuspectsName { get; private set; }
        public uint TargettingHandle
        {
            get => targettingHandle;
            private set
            {
                if (targettingHandle != value)
                {
                    targettingHandle = value;
                    TargettingHandleChanged();
                }
            }
        }
        public uint TimeInSearchMode => SearchMode.TimeInSearchMode;
        public int TimesDied { get; set; }
        public uint TimeToRecognize
        {
            get
            {
                uint Time = 2000;
                if (TimeControllable.IsNight)
                {
                    Time += 3500;
                }
                else if (IsInVehicle)
                {
                    Time += 750;
                }
                return Time;
            }
        }
        public List<VehicleExt> TrackedVehicles { get; private set; } = new List<VehicleExt>();
        public VehicleExt VehicleGettingInto { get; private set; }
        public float VehicleSpeed { get; private set; }
        public int WantedLevel => Game.LocalPlayer.WantedLevel;
        public string DebugLine1 => $"{Interaction?.DebugString}";
        public string DebugLine2 => $"WantedFor {PoliceResponse.HasBeenWantedFor} NotWantedFor {PoliceResponse.HasBeenNotWantedFor} CurrentWantedFor {PoliceResponse.HasBeenAtCurrentWantedLevelFor}";
        public string DebugLine3 => DynamicActivity?.DebugString;
        public string DebugLine4 => $"Player: {ModelName},{Game.LocalPlayer.Character.Handle} Target: {CurrentTargetedPed?.Pedestrian?.Handle} LookAt: {CurrentLookedAtPed?.Pedestrian?.Handle} NearScen {IsNearScenario}";
        public string DebugLine5 => $"Obs {PoliceResponse.ObservedCrimesDisplay}";
        public string DebugLine6 => $"Rep {PoliceResponse.ReportedCrimesDisplay}";
        public string DebugLine7 => $"Vio {Violations.LawsViolatingDisplay}";
        public string DebugLine8 => PoliceResponse.DebugText;
        public string DebugLine9 => Investigation.DebugText;
        public string DebugLine10 => $"IsMoving {IsMoving} IsMovingFast {IsMovingFast} IsMovingDynam {IsMovingDynamically}";
        public void AddCrime(Crime CrimeInstance, bool ByPolice, Vector3 Location, VehicleExt VehicleObserved, WeaponInformation WeaponObserved, bool HaveDescription)
        {

            PoliceResponse.AddCrime(CrimeInstance, ByPolice, Location, VehicleObserved, WeaponObserved, HaveDescription);
            if (!ByPolice)
            {
                Game.Console.Print($"PLAYER EVENT: INVESTIGATION START");
                Investigation.Start();
            }
        }
        public void Arrest()
        {
            BeingArrested = true;
            if (!IsBusted)
            {
                IsBustedChanged();
            }
        }
        public void DisplayPlayerNotification()
        {
            string NotifcationText = "Warrants: ~g~None~s~";
            if (PoliceResponse.HasObservedCrimes)
            {
                NotifcationText = "Wanted For:" + PoliceResponse.PrintCrimes();
            }

            if (CurrentVehicle != null && !CurrentVehicle.IsStolen)
            {
                string Make = CurrentVehicle.MakeName();
                string Model = CurrentVehicle.ModelName();
                string VehicleName = "";
                if (Make != "")
                {
                    VehicleName = Make;
                }
                if (Model != "")
                {
                    VehicleName += " " + Model;
                }

                NotifcationText += string.Format("~n~Vehicle: ~p~{0}~s~", VehicleName);
                NotifcationText += string.Format("~n~Plate: ~p~{0}~s~", CurrentVehicle.CarPlate.PlateNumber);
            }

            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~b~Personal Info", string.Format("~y~{0}", SuspectsName), NotifcationText);
        }
        public void Dispose()
        {
            Investigation.Dispose(); //remove blip
            PoliceResponse.Dispose(); //same ^
            Interaction?.Dispose();
            isActive = false;
            // NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Game.LocalPlayer.Character, (int)PedConfigFlags._PED_FLAG_DISABLE_STARTING_VEH_ENGINE, false);
        }
        public void GiveMoney(int Amount)
        {
            int CurrentCash;
            uint PlayerCashHash = NativeHelper.CashHash(Settings.SettingsManager.General.MainCharacterToAlias);
            unsafe
            {
                NativeFunction.CallByName<int>("STAT_GET_INT", PlayerCashHash, &CurrentCash, -1);
            }
            if (CurrentCash + Amount < 0)
            {
                NativeFunction.CallByName<int>("STAT_SET_INT", PlayerCashHash, 0, 1);
            }
            else
            {
                NativeFunction.CallByName<int>("STAT_SET_INT", PlayerCashHash, CurrentCash + Amount, 1);
            }
        }
        public void Reset(bool resetWanted, bool resetTimesDied, bool clearWeapons, bool clearCriminalHistory)
        {
            IsDead = false;
            IsBusted = false;
            Game.LocalPlayer.HasControl = true;
            BeingArrested = false;
            HealthState = new HealthState(new PedExt(Game.LocalPlayer.Character));
            IsPerformingActivity = false;
            if (resetWanted)
            {
                PoliceResponse.Reset();
                Investigation.Reset();
                Violations.Reset();
                //NativeFunction.CallByName<bool>("RESET_PLAYER_ARREST_STATE", Game.LocalPlayer);
                MaxWantedLastLife = 0;
                GameTimeStartedPlaying = Game.GameTime;

                // ResetModel();

                Update();
            }
            if (resetTimesDied)
            {
                TimesDied = 0;
            }
            if (clearWeapons)
            {
                Game.LocalPlayer.Character.Inventory.Weapons.Clear();
            }
            if (clearCriminalHistory)
            {
                CriminalHistory.Clear();
            }
        }
        public void SetDemographics(string modelName, bool isMale, string suspectsName, int money)
        {
            ModelName = modelName;
            SuspectsName = suspectsName;
            IsMale = IsMale;
            SetMoney(money);
        }
        public void SetMoney(int Amount)
        {
            NativeFunction.CallByName<int>("STAT_SET_INT", NativeHelper.CashHash(Settings.SettingsManager.General.MainCharacterToAlias), Amount, 1);
        }
        public void SetPlayerToLastWeapon()
        {
            if (Game.LocalPlayer.Character.Inventory.EquippedWeapon != null && LastWeaponHash != 0)
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)LastWeaponHash, true);
                //Game.Console.Print("SetPlayerToLastWeapon" + LastWeaponHash.ToString());
            }
        }
        public void SetUnarmed()
        {
            if (!(Game.LocalPlayer.Character.Inventory.EquippedWeapon == null))
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)2725352035, true); //Unequip weapon so you don't get shot
            }
        }
        public void Setup()
        {
            PoliceResponse.SetWantedLevel(0, "Initial", true);
            // NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Game.LocalPlayer.Character, (int)PedConfigFlags._PED_FLAG_DISABLE_STARTING_VEH_ENGINE, true);
            SetUnarmed();
            SpareLicensePlates.Add(new LicensePlate(RandomItems.RandomString(8), 3, false));//random cali

            CurrentModelName = Game.LocalPlayer.Character.Model.Name;
            CurrentModelVariation = NativeHelper.GetPedVariation(Game.LocalPlayer.Character);


            //this temp bullshit
            GameFiber.StartNew(delegate
            {
                while (isActive)
                {
                    if (Game.LocalPlayer.Character.IsShooting)
                    {
                        GameTimeLastShot = Game.GameTime;
                    }

                    GameFiber.Yield();
                }

            }, "IsShootingChecker");


        }
        public void ShootAt(Vector3 TargetCoordinate)
        {
            NativeFunction.CallByName<bool>("SET_PED_SHOOTS_AT_COORD", Game.LocalPlayer.Character, TargetCoordinate.X, TargetCoordinate.Y, TargetCoordinate.Z, true);
            GameTimeLastShot = Game.GameTime;
        }
        public void Update()
        {
            UpdateData();
            UpdateButtonPrompts();
        }
        //Interactions
        public void StartConversation()
        {
            if (!IsInteracting && CanConverseWithLookedAtPed)
            {
                if (Interaction != null)
                {
                    Interaction.Dispose();
                }
                IsConversing = true;
                Interaction = new Conversation(this, CurrentLookedAtPed);
                Interaction.Start();
            }
        }
        public void StartHoldUp()
        {
            if (!IsInteracting && CanHoldUpTargettedPed)
            {
                if (Interaction != null)
                {
                    Interaction.Dispose();
                }
                IsHoldingUp = true;
                Interaction = new HoldUp(this, CurrentTargetedPed);
                Interaction.Start();
            }
        }
        //Dynamic Activities
        public void ChangePlate()
        {
            if (!IsPerformingActivity && CanPerformActivities)
            {
                if (DynamicActivity != null)
                {
                    DynamicActivity.Cancel();
                }
                IsPerformingActivity = true;
                DynamicActivity = new PlateTheft(this, SpareLicensePlates[0]);
                DynamicActivity.Start();
            }
        }
        public void CommitSuicide()
        {
            if (!IsPerformingActivity && CanPerformActivities)
            {
                if (DynamicActivity != null)
                {
                    DynamicActivity.Cancel();
                }
                IsPerformingActivity = true;
                DynamicActivity = new SuicideActivity(this);
                DynamicActivity.Start();
            }
        }
        public void DrinkBeer()
        {
            if (!IsPerformingActivity && CanPerformActivities)
            {
                if (DynamicActivity != null)
                {
                    DynamicActivity.Cancel();
                }
                IsPerformingActivity = true;
                DynamicActivity = new DrinkingActivity(this);
                DynamicActivity.Start();
            }
        }
        public void RemovePlate()
        {
            if (!IsPerformingActivity && CanPerformActivities)
            {
                if (DynamicActivity != null)
                {
                    DynamicActivity.Cancel();
                }
                IsPerformingActivity = true;
                DynamicActivity = new PlateTheft(this);
                DynamicActivity.Start();
            }
        }
        public void StartScenario()
        {
            if (!IsPerformingActivity && CanPerformActivities)
            {
                if (DynamicActivity != null)
                {
                    DynamicActivity.Cancel();
                }
                IsPerformingActivity = true;
                DynamicActivity = new ScenarioActivity(this);
                DynamicActivity.Start();
            }
        }
        public void StartSmoking()
        {
            if (!IsPerformingActivity && CanPerformActivities)
            {
                if (DynamicActivity != null)
                {
                    DynamicActivity.Cancel();
                }
                IsPerformingActivity = true;
                DynamicActivity = new SmokingActivity(this, false);
                DynamicActivity.Start();
            }
            else if (IsPerformingActivity && CanPerformActivities)
            {
                DynamicActivity.Continue();
            }
        }
        public void StartSmokingPot()
        {
            if (!IsPerformingActivity && CanPerformActivities)
            {
                if (DynamicActivity != null)
                {
                    DynamicActivity.Cancel();
                }
                IsPerformingActivity = true;
                DynamicActivity = new SmokingActivity(this, true);
                DynamicActivity.Start();
            }
            else if (IsPerformingActivity && CanPerformActivities)
            {
                DynamicActivity.Continue();
            }
        }
        public void StopDynamicActivity()
        {
            if (IsPerformingActivity)
            {
                DynamicActivity?.Cancel();
            }
        }
        //Delegates
        public void ArrestWarrantUpdate() => CriminalHistory.Update();
        public void CheckInjured(PedExt MyPed) => Violations.AddInjured(MyPed);
        public void CheckMurdered(PedExt MyPed) => Violations.AddKilled(MyPed);
        public void DropWeapon() => WeaponDropping.DropWeapon();
        public void LocationUpdate() => CurrentLocation.Update();
        public void LowerHands() => Surrendering.LowerHands();
        public void RaiseHands() => Surrendering.RaiseHands();
        public void SearchModeUpdate() => SearchMode.UpdateWanted();
        public void StopVanillaSearchMode() => SearchMode.StopVanilla();
        public void StoreCriminalHistory() => CriminalHistory.StoreCriminalHistory(PoliceResponse);
        public void TrafficViolationsUpdate() => Violations.TrafficUpdate();
        public void UnSetArrestedAnimation(Ped character) => Surrendering.UnSetArrestedAnimation(character);
        public void ViolationsUpdate() => Violations.Update();
        private void IsAimingChanged()
        {
            if (IsAiming)
            {
            }
            else
            {
            }
            Game.Console.Print($"PLAYER EVENT: IsAiming Changed to: {IsAiming}");
        }
        private void IsAimingInVehicleChanged()
        {
            if (IsAimingInVehicle)
            {
                if (CurrentVehicle != null)
                {
                    CurrentVehicle.SetDriverWindow(true);
                }
            }
            else
            {
                if (CurrentVehicle != null)
                {
                    CurrentVehicle.SetDriverWindow(false);
                }
            }
            Game.Console.Print($"PLAYER EVENT: IsAimingInVehicle Changed to: {IsAimingInVehicle}");
        }
        private void IsBustedChanged()
        {
            DiedInVehicle = IsInVehicle;
            IsBusted = true;
            BeingArrested = true;
            GameTimeLastBusted = Game.GameTime;
            HandsAreUp = false;
            Surrendering.SetArrestedAnimation(Game.LocalPlayer.Character, false, WantedLevel <= 2);
            Game.LocalPlayer.HasControl = false;
            Game.Console.Print($"PLAYER EVENT: IsBusted Changed to: {IsBusted}");
        }
        private void IsDeadChanged()
        {
            TimeControllable.PauseTime();
            DiedInVehicle = IsInVehicle;
            IsDead = true;
            GameTimeLastDied = Game.GameTime;
            Game.LocalPlayer.Character.Kill();
            Game.LocalPlayer.Character.Health = 0;
            Game.LocalPlayer.Character.IsInvincible = true;
            Game.TimeScale = 0.4f;
            Game.Console.Print($"PLAYER EVENT: IsDead Changed to: {IsDead}");
        }
        private void IsGettingIntoVehicleChanged()
        {
            if (IsGettingIntoAVehicle)
            {
                Vehicle VehicleTryingToEnter = Game.LocalPlayer.Character.VehicleTryingToEnter;
                int SeatTryingToEnter = Game.LocalPlayer.Character.SeatIndexTryingToEnter;
                if (VehicleTryingToEnter == null)
                {
                    return;
                }
                VehicleExt MyCar = EntityProvider.GetVehicle(VehicleTryingToEnter);
                if (MyCar != null)
                {
                    VehicleGettingInto = MyCar;

                    //MyCar.
                    //Maybe Call MyCar.EnterEvent(Character)
                    //that will check if you are the owner and just unlock the doors
                    //alos how to check if the car is owned by ped by the game

                    //also need in tasking when you are idle set them in, use the same function iwtht he ped passed in
                    //will need to have a list of owners or occupants?
                    //maybe check relationship group of the ped driver and if they are friendly set it unlocked

                    //add auto door locks to the cars
                    //when you start going they auto lock and peds cannot carjack you as easily

                    MyCar.AttemptToLock();
                    if (IsHoldingEnter && VehicleTryingToEnter.Driver == null && VehicleTryingToEnter.LockStatus == (VehicleLockStatus)7 && !VehicleTryingToEnter.IsEngineOn)//no driver && Unlocked
                    {
                        CarLockPick MyLockPick = new CarLockPick(this, VehicleTryingToEnter, SeatTryingToEnter);
                        MyLockPick.PickLock();
                    }
                    else if (IsHoldingEnter && SeatTryingToEnter == -1 && VehicleTryingToEnter.Driver != null && VehicleTryingToEnter.Driver.IsAlive) //Driver
                    {
                        CarJack MyJack = new CarJack(this, VehicleTryingToEnter, VehicleTryingToEnter.Driver, EntityProvider.CivilianList.FirstOrDefault(x => x.Pedestrian.Handle == VehicleTryingToEnter.Driver.Handle), SeatTryingToEnter, CurrentWeapon);
                        MyJack.StartCarJack();
                    }
                    else if (VehicleTryingToEnter.LockStatus == (VehicleLockStatus)7)
                    {
                        CarBreakIn MyBreakIn = new CarBreakIn(this, VehicleTryingToEnter);
                        MyBreakIn.BreakIn();
                    }
                }
            }
            else
            {
            }
            isGettingIntoVehicle = IsGettingIntoAVehicle;
            Game.Console.Print($"PLAYER EVENT: IsGettingIntoVehicleChanged to {IsGettingIntoAVehicle}");
        }
        private void IsInVehicleChanged()
        {
            if (IsInVehicle)
            {
            }
            else
            {
                if (CurrentVehicle != null && CurrentVehicle.Vehicle.Exists())
                {
                    CurrentVehicle.Vehicle.IsEngineOn = isCurrentVehicleEngineOn;
                }
            }
            Game.Console.Print($"PLAYER EVENT: IsInVehicle to {IsInVehicle}");
        }
        private void TargettingHandleChanged()
        {
            if (TargettingHandle != 0)
            {
                CurrentTargetedPed = EntityProvider.GetCivilian(TargettingHandle);
                if (!IsInteracting && CanHoldUpTargettedPed && CurrentTargetedPed != null && CurrentTargetedPed.CanBeMugged)
                {
                    StartHoldUp();
                }
            }
            else
            {
                CurrentTargetedPed = null;
            }
            Game.Console.Print($"PLAYER EVENT: CurrentTargetedPed to {CurrentTargetedPed?.Pedestrian?.Handle}");
        }
        private void UpdateButtonPrompts()
        {
            if (!IsInteracting && CanConverseWithLookedAtPed)
            {
                if (!ButtonPrompts.Any(x => x.Identifier == $"Talk {CurrentLookedAtPed.Pedestrian.Handle}"))
                {
                    ButtonPrompts.RemoveAll(x => x.Group == "StartConversation");
                    ButtonPrompts.Add(new ButtonPrompt($"Talk to {CurrentLookedAtPed.FormattedName}", "StartConversation", $"Talk {CurrentLookedAtPed.Pedestrian.Handle}", Keys.E, 1));
                }
            }
            else
            {
                ButtonPrompts.RemoveAll(x => x.Group == "StartConversation");
            }
            if (CanPerformActivities && IsNearScenario)
            {
                if (!ButtonPrompts.Any(x => x.Identifier == $"StartScenario"))
                {
                    ButtonPrompts.RemoveAll(x => x.Group == "StartScenario");
                    ButtonPrompts.Add(new ButtonPrompt($"{ClosestScenario?.Name}", "StartScenario", $"StartScenario", Keys.P, 2));
                }
            }
            else
            {
                ButtonPrompts.RemoveAll(x => x.Group == "StartScenario");
            }



        }
        private void UpdateCurrentVehicle()
        {
            bool IsGettingIntoVehicle = Game.LocalPlayer.Character.IsGettingIntoVehicle;
            bool IsInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
            if (!IsInVehicle && !IsGettingIntoVehicle)
            {
                CurrentVehicle = null;
            }
            Vehicle CurrVehicle = null;
            if (IsGettingIntoVehicle)
            {
                CurrVehicle = Game.LocalPlayer.Character.VehicleTryingToEnter;
            }
            else
            {
                CurrVehicle = Game.LocalPlayer.Character.CurrentVehicle;
            }
            if (!CurrVehicle.Exists())
            {
                CurrentVehicle = null;
                return;
            }
            VehicleExt ToReturn = TrackedVehicles.Where(x => x.Vehicle.Handle == CurrVehicle.Handle).FirstOrDefault();
            if (ToReturn == null)
            {
                VehicleExt MyNewCar = new VehicleExt(CurrVehicle, Game.GameTime);
                TrackedVehicles.Add(MyNewCar);
                ToReturn = MyNewCar;
            }
            if (IsInVehicle)
            {
                ToReturn.SetAsEntered();
            }
            ToReturn.Update(AutoTuneStation);
            isCurrentVehicleEngineOn = ToReturn.Vehicle.IsEngineOn;
            CurrentVehicle = ToReturn;
        }
        private void UpdateData()
        {
            UpdateVehicleData();
            UpdateWeaponData();
            UpdateStateData();
        }
        private void UpdateLookedAtPed()
        {
            if (Game.GameTime - GameTimeLastUpdatedLookedAtPed >= 750)
            {


                //Works fine just going simpler
                Vector3 RayStart = Game.LocalPlayer.Character.GetBonePosition(PedBoneId.Head);
                // Vector3 RayStart = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
                Vector3 RayEnd = RayStart + NativeHelper.GetGameplayCameraDirection() * 6.0f;
                //Vector3 RayStart = Game.LocalPlayer.Character.GetBonePosition(PedBoneId.Head);
                //Vector3 RayEnd = RayStart + Game.LocalPlayer.Character.Direction * 5.0f;
                HitResult result = Rage.World.TraceCapsule(RayStart, RayEnd, 1f, TraceFlags.IntersectVehicles | TraceFlags.IntersectPedsSimpleCollision, Game.LocalPlayer.Character);//2 meter wide cylinder out 10 meters that ignores the player charater going from the head in the players direction
                                                                                                                                                                                     //  Rage.Debug.DrawArrowDebug(RayStart, Game.LocalPlayer.Character.Direction, Rotator.Zero, 1f, Color.White);
                                                                                                                                                                                     //  Rage.Debug.DrawArrowDebug(RayEnd, Game.LocalPlayer.Character.Direction, Rotator.Zero, 1f, Color.Red);
                if (result.Hit && result.HitEntity is Ped)
                {
                    // Rage.Debug.DrawArrowDebug(result.HitPosition, Game.LocalPlayer.Character.Direction, Rotator.Zero, 1f, Color.Green);
                    CurrentLookedAtPed = EntityProvider.GetCivilian(result.HitEntity.Handle);
                }
                else
                {
                    CurrentLookedAtPed = null;
                }

                //CurrentLookedAtPed = EntityProvider.CivilianList.Where(x => x.DistanceToPlayer <= 4f && !x.IsBehindPlayer).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
                GameTimeLastUpdatedLookedAtPed = Game.GameTime;
            }
        }
        private void UpdateSpeedDispay()
        {
            if (CurrentVehicle != null)//was game.localpalyer.character.isinanyvehicle(false)
            {
                CurrentSpeedDisplay = "";
                float VehicleSpeedMPH = VehicleSpeed * 2.23694f;
                if (!Game.LocalPlayer.Character.CurrentVehicle.IsEngineOn)
                {
                    CurrentSpeedDisplay = "ENGINE OFF";
                }
                else
                {
                    string ColorPrefx = "~s~";
                    if (IsSpeeding)
                    {
                        ColorPrefx = "~r~";
                    }
                    if (CurrentLocation.CurrentStreet != null)
                    {
                        CurrentSpeedDisplay = $"{ColorPrefx}{Math.Round(VehicleSpeedMPH, MidpointRounding.AwayFromZero)} ~s~MPH ({CurrentLocation.CurrentStreet.SpeedLimit})";
                    }
                }
                if (IsViolatingAnyTrafficLaws)
                {
                    CurrentSpeedDisplay += " !";
                }
                CurrentSpeedDisplay += "~n~" + CurrentVehicle.FuelTank.UIText;
            }
        }
        private void UpdateStateData()
        {

            if (Game.LocalPlayer.Character.IsDead && !IsDead)
            {
                IsDeadChanged();
            }
            if (NativeFunction.CallByName<bool>("IS_PLAYER_BEING_ARRESTED", 0))
            {
                BeingArrested = true;
            }
            if (NativeFunction.CallByName<bool>("IS_PLAYER_BEING_ARRESTED", 1))
            {
                BeingArrested = true;
                Game.LocalPlayer.Character.Tasks.Clear();
            }
            if (BeingArrested && !IsBusted)
            {
                IsBustedChanged();
            }
            if (IsAliveAndFree && !Game.LocalPlayer.Character.IsDead)
            {
                MaxWantedLastLife = WantedLevel;
            }
            if (PreviousWantedLevel != Game.LocalPlayer.WantedLevel)
            {
                WantedLevelChanged();
            }
            if (CurrentLocation.CharacterToLocate.Handle != Game.LocalPlayer.Character.Handle)
            {
                CurrentLocation.CharacterToLocate = Game.LocalPlayer.Character;
            }
            if (HealthState.MyPed.Pedestrian.Handle != Game.LocalPlayer.Character.Handle)
            {
                HealthState.MyPed = new PedExt(Game.LocalPlayer.Character);
            }
            HealthState.Update();
            IsStunned = Game.LocalPlayer.Character.IsStunned;
            IsRagdoll = Game.LocalPlayer.Character.IsRagdoll;
            IsMovingDynamically = Game.LocalPlayer.Character.IsInCover || Game.LocalPlayer.Character.IsInCombat || Game.LocalPlayer.Character.IsJumping || Game.LocalPlayer.Character.IsRunning;
            if (NativeFunction.CallByName<bool>("GET_PED_CONFIG_FLAG", Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, 1) || NativeFunction.CallByName<int>("GET_TIMECYCLE_MODIFIER_INDEX") == 722)
            {
                IsIntoxicated = true;
            }
            else
            {
                IsIntoxicated = false;
            }
            if (IsNotWanted && !IsInVehicle)//meh only on not wanted for now, well see
            {
                IsNearScenario = NativeFunction.Natives.DOES_SCENARIO_EXIST_IN_AREA<bool>(Position.X, Position.Y, Position.Z, 2f, true) && !NativeFunction.Natives.IS_SCENARIO_OCCUPIED<bool>(Position.X, Position.Y, Position.Z, 2f, true);
                ClosestScenario = new Scenario("", "Unknown");
                if (IsNearScenario)
                {
                    foreach (Scenario scenario in Scenarios.ScenarioList)
                    {
                        if (NativeFunction.Natives.DOES_SCENARIO_OF_TYPE_EXIST_IN_AREA<bool>(Position.X, Position.Y, Position.Z, scenario.InternalName, 2f, true))
                        {
                            ClosestScenario = scenario;
                            break;
                        }
                    }

                }
            }
            else
            {
                IsNearScenario = false;
            }
        }
        private void UpdateTargetedPed()
        {
            if (IsAiming)
            {
                Entity AimingAt = Game.LocalPlayer.GetFreeAimingTarget();
                if (AimingAt.Exists())
                {
                    TargettingHandle = AimingAt.Handle;
                }
                else
                {
                    TargettingHandle = 0;
                }
            }
            else
            {
                TargettingHandle = NativeHelper.GetTargettingHandle();
            }
        }
        private void UpdateVehicleData()
        {
            IsInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
            IsGettingIntoAVehicle = Game.LocalPlayer.Character.IsGettingIntoVehicle;
            if (IsInVehicle)
            {
                IsInAirVehicle = Game.LocalPlayer.Character.IsInAirVehicle;
                IsInAutomobile = !(IsInAirVehicle || Game.LocalPlayer.Character.IsInSeaVehicle || Game.LocalPlayer.Character.IsOnBike || Game.LocalPlayer.Character.IsInHelicopter);
                IsOnMotorcycle = Game.LocalPlayer.Character.IsOnBike;
                UpdateCurrentVehicle();
                IsHotWiring = CurrentVehicle != null && CurrentVehicle.Vehicle.Exists() && CurrentVehicle.Vehicle.MustBeHotwired;
                VehicleSpeed = Game.LocalPlayer.Character.CurrentVehicle.Speed;
                UpdateSpeedDispay();
                if (isHotwiring != IsHotWiring)
                {
                    if (IsHotWiring)
                    {
                        GameTimeStartedHotwiring = Game.GameTime;
                    }
                    else
                    {
                        Game.Console.Print($"PLAYER EVENT: HotWiring Took {Game.GameTime - GameTimeStartedHotwiring}");
                        GameTimeStartedHotwiring = 0;
                    }
                    Game.Console.Print($"PLAYER EVENT: IsHotWiring Changed to {IsHotWiring}");
                    isHotwiring = IsHotWiring;
                }

                if (CurrentVehicle != null && CurrentVehicle.Vehicle.IsEngineOn && CurrentVehicle.Vehicle.IsPoliceVehicle)
                {
                    if (!IsMobileRadioEnabled)
                    {
                        IsMobileRadioEnabled = true;
                        NativeFunction.CallByName<bool>("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", true);
                    }
                }
                else
                {
                    if (IsMobileRadioEnabled)
                    {
                        IsMobileRadioEnabled = false;
                        NativeFunction.CallByName<bool>("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", false);
                    }
                }
                if (VehicleSpeed >= 0.1f)
                {
                    GameTimeLastMoved = Game.GameTime;
                }
                else
                {
                    GameTimeLastMoved = 0;
                }
                if (VehicleSpeed >= 2.0f)
                {
                    GameTimeLastMovedFast = Game.GameTime;
                }
                else
                {
                    GameTimeLastMovedFast = 0;
                }
                IsStill = VehicleSpeed <= 0.1f;
            }
            else
            {
                IsOnMotorcycle = false;
                IsInAutomobile = false;
                CurrentVehicle = null;
                CurrentSpeedDisplay = "";
                float PlayerSpeed = Game.LocalPlayer.Character.Speed;
                if (PlayerSpeed >= 0.1f)
                {
                    GameTimeLastMoved = Game.GameTime;
                }
                else
                {
                    GameTimeLastMoved = 0;
                }
                if (PlayerSpeed >= 7.0f)
                {
                    GameTimeLastMovedFast = Game.GameTime;
                }
                else
                {
                    GameTimeLastMovedFast = 0;
                }
                IsStill = Game.LocalPlayer.Character.IsStill;
                NativeFunction.CallByName<bool>("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", false);
            }
            TrackedVehicles.RemoveAll(x => !x.Vehicle.Exists());
        }
        private void UpdateVisiblyArmed()
        {
            if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && !Game.LocalPlayer.IsFreeAiming)
            {
                IsVisiblyArmed = false;
            }
            else if (Game.LocalPlayer.Character.Inventory.EquippedWeapon == null)
            {
                IsVisiblyArmed = false;
            }
            else if (Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)2725352035
                || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)966099553
                || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)0x787F0BB//weapon_snowball
                || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)0x060EC506//weapon_fireextinguisher
                || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)0x34A67B97//weapon_petrolcan
                || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)0xBA536372//weapon_hazardcan
                || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)0x8BB05FD7//weapon_flashlight
                || Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash == (WeaponHash)0x23C9F95C)//weapon_ball
            {
                IsVisiblyArmed = false;
            }
            else if (!NativeFunction.Natives.IS_PLAYER_CONTROL_ON<bool>(Game.LocalPlayer))
            {
                IsVisiblyArmed = false;
            }
            else
            {
                IsVisiblyArmed = true;
            }

        }
        private void UpdateWeaponData()
        {
            if (Game.LocalPlayer.Character.IsShooting)
            {
                GameTimeLastShot = Game.GameTime;
            }
            IsAiming = Game.LocalPlayer.IsFreeAiming;
            IsAimingInVehicle = IsInVehicle && IsAiming;
            UpdateVisiblyArmed();
            WeaponDescriptor PlayerCurrentWeapon = Game.LocalPlayer.Character.Inventory.EquippedWeapon;
            CurrentWeapon = Weapons.GetCurrentWeapon(Game.LocalPlayer.Character);
            if (PlayerCurrentWeapon != null)
            {
                CurrentWeaponHash = PlayerCurrentWeapon.Hash;
            }
            else
            {
                CurrentWeaponHash = 0;
            }
            if (CurrentWeaponHash != 0 && PlayerCurrentWeapon.Hash != LastWeaponHash)
            {
                LastWeaponHash = PlayerCurrentWeapon.Hash;
            }
            WeaponDropping.Update();
            UpdateTargetedPed();
            UpdateLookedAtPed();
        }
        private void WantedLevelChanged()
        {
            if (IsNotWanted && PreviousWantedLevel != 0)//removed
            {
                PoliceResponse.OnLostWanted();
            }
            else if (IsWanted && PreviousWantedLevel == 0)//added
            {
                if (!PoliceResponse.RecentlySetWanted)//only allow my process to set the wanted level
                {
                    Game.Console.Print($"PLAYER EVENT: GAME AUTO SET WANTED TO {WantedLevel}, RESETTING");
                    Game.LocalPlayer.WantedLevel = 0;
                }
                else
                {
                    Investigation.Reset();
                    PoliceResponse.OnBecameWanted();
                }
            }
            else
            {
                PoliceResponse.OnWantedLevelIncreased();
            }
            PreviousWantedLevel = Game.LocalPlayer.WantedLevel;
        }
    }
}