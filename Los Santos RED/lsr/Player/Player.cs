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
    public class Player : IDispatchable, IActivityPerformable, IIntoxicatable, ITargetable, IPoliceRespondable, IInputable, IPedSwappable, IMuggable, IRespawnable, IViolateable, IWeaponDroppable, IDisplayable, ICarStealable, IPlateChangeable, IActionable, IInteractionable, IInventoryable, IRespawning, ISaveable
    {
        private CriminalHistory CriminalHistory;
        private DynamicActivity DynamicActivity;
        private SearchMode SearchMode;
        private SurrenderActivity Surrendering;
        private WeaponDropping WeaponDropping;
        private HealthState HealthState;
        private Inventory Inventory;
        private Respawning Respawning;
        private Scanner Scanner;

        private IRadioStations RadioStations;
        private ISettingsProvideable Settings;
        private ITimeControllable TimeControllable;
        private IEntityProvideable EntityProvider;
        private IWeapons Weapons;
        private IScenarios Scenarios;
        private ICrimes Crimes;

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
        public int UpdateState = 0;
        private uint GameTimeLastSetWanted;
        private uint GameTimeWantedLevelStarted;
        private string CurrentVehicleDebugString;

        public Player(string modelName, bool isMale, string suspectsName, int currentMoney, IEntityProvideable provider, ITimeControllable timeControllable, IStreets streets, IZones zones, ISettingsProvideable settings, IWeapons weapons, IRadioStations radioStations, IScenarios scenarios, ICrimes crimes, IAudioPlayable audio, IPlacesOfInterest placesOfInterest)
        {
            ModelName = modelName;
            IsMale = isMale;
            PlayerName = suspectsName;
            Crimes = crimes;
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
            Scanner = new Scanner(provider, this, audio, settings);
            HealthState = new HealthState(new PedExt(Game.LocalPlayer.Character));
            CurrentLocation = new LocationData(Game.LocalPlayer.Character, streets, zones);
            WeaponDropping = new WeaponDropping(this, Weapons);
            Surrendering = new SurrenderActivity(this);
            Violations = new Violations(this, TimeControllable, Crimes);
            Investigation = new Investigation(this);
            CriminalHistory = new CriminalHistory(this);
            PoliceResponse = new PoliceResponse(this);
            SearchMode = new SearchMode(this);
            Inventory = new Inventory(this);
            
            Respawning = new Respawning(TimeControllable, EntityProvider, this, Weapons, placesOfInterest, Settings);
        }
        public Investigation Investigation { get; private set; }
        public Interaction Interaction { get; private set; }
        public PoliceResponse PoliceResponse { get; private set; }
        public Violations Violations { get; private set; }
        public Scenario ClosestScenario { get; private set; }
        public float ActiveDistance => Investigation.IsActive ? Investigation.Distance : 500f + (WantedLevel * 200f);
        public bool AnyHumansNear => EntityProvider.PoliceList.Any(x => x.DistanceToPlayer <= 10f) || EntityProvider.CivilianList.Any(x => x.DistanceToPlayer <= 10f);//move or delete?
        public bool AnyPoliceCanHearPlayer { get; set; }//all this perception stuff gets moved out?
        public bool AnyPoliceCanRecognizePlayer { get; set; }
        public bool AnyPoliceCanSeePlayer { get; set; }
        public bool AnyPoliceRecentlySeenPlayer { get; set; }
        public string AutoTuneStation { get; set; } = "NONE";
        public bool BeingArrested { get; private set; }
        public List<ButtonPrompt> ButtonPrompts { get; private set; } = new List<ButtonPrompt>();
        public bool CanConverse => !IsGettingIntoAVehicle && !IsBreakingIntoCar && !IsIncapacitated && !IsVisiblyArmed && IsAliveAndFree && !IsMovingDynamically;
        public bool CanConverseWithLookedAtPed => CurrentLookedAtPed != null && CurrentTargetedPed == null && CurrentLookedAtPed.CanConverse && CanConverse;// && (Relationship)NativeFunction.Natives.GET_RELATIONSHIP_BETWEEN_PEDS<int>(CurrentLookedAtPed.Pedestrian, Character) != Relationship.Hate;//off for performance checking
        public bool CanDropWeapon => CanPerformActivities && WeaponDropping.CanDropWeapon;
        public bool CanHoldUpTargettedPed => CurrentTargetedPed != null && CurrentTargetedPed.CanBeMugged && !IsGettingIntoAVehicle && !IsBreakingIntoCar && !IsStunned && !IsRagdoll && IsVisiblyArmed && IsAliveAndFree && CurrentTargetedPed.DistanceToPlayer <= 7f;
        public bool CanPerformActivities => !IsMovingFast && !IsIncapacitated && !IsDead && !IsBusted && !IsInVehicle && !IsGettingIntoAVehicle  && !IsMovingDynamically;//&& !IsAttemptingToSurrender
        public bool CanSurrender => Surrendering.CanSurrender;
        public Ped Character => Game.LocalPlayer.Character;
        public LocationData CurrentLocation { get; set; }
        public PedExt CurrentLookedAtPed { get; private set; }
        public VehicleExt CurrentSeenVehicle => CurrentVehicle ?? VehicleGettingInto;
        public WeaponInformation CurrentSeenWeapon => !IsInVehicle ? CurrentWeapon : null;
    //    public string CurrentSpeedDisplay { get; private set; }
        public PedExt CurrentTargetedPed { get; private set; }
        public VehicleExt CurrentVehicle { get; private set; }
        public WeaponInformation CurrentWeapon { get; private set; }
        public WeaponCategory CurrentWeaponCategory => CurrentWeapon != null ? CurrentWeapon.Category : WeaponCategory.Unknown;
        public bool CurrentWeaponIsOneHanded  { get; private set; }
        public WeaponHash CurrentWeaponHash { get; set; }//move or delete?
        public string CurrentModelName { get; set; }//should be private but needed?
        public PedVariation CurrentModelVariation { get; set; }//should be private but needed?
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
                    OnAiminChanged();
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
                    OnAimingInVehicleChanged();
                }
            }
        }
        public bool IsAliveAndFree => !IsBusted && !IsDead;
        public bool IsAttemptingToSurrender => HandsAreUp && !PoliceResponse.IsWeaponsFree;
        public bool IsBreakingIntoCar => IsCarJacking || IsLockPicking || IsHotWiring || Game.LocalPlayer.Character.IsJacking;
        public bool IsBustable => IsAliveAndFree && PoliceResponse.HasBeenWantedFor >= 3000 && !Surrendering.IsCommitingSuicide && !RecentlyBusted && !RecentlyResistedArrest && !IsInVehicle && !PoliceResponse.IsWeaponsFree && (IsIncapacitated || (!IsMoving && !IsMovingDynamically));
        public bool IsBusted { get; private set; }
        public bool IsCarJacking { get; set; }
        public bool IsChangingLicensePlates { get; set; }
        public bool IsCommitingSuicide { get; set; }
        public bool IsConversing { get; set; }
        public float SearchModePercentage => SearchMode.SearchModePercentage;
        public bool IsDead { get; private set; }
        public bool IsGettingIntoAVehicle
        {
            get => isGettingIntoVehicle;
            private set
            {
                if (isGettingIntoVehicle != value)
                {
                    isGettingIntoVehicle = value;
                    OnGettingIntoAVehicleChanged();
                }
            }
        }
        public bool IsNotHoldingEnter { get; set; }
        public bool IsMoveControlPressed { get; set; }
        public bool IsHoldingUp { get; set; }
        public bool IsHotWiring { get; private set; }
        public bool IsDriver { get; private set; }
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
                    OnIsInVehicleChanged();
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
        public bool IsNotWanted => Game.LocalPlayer.WantedLevel == 0;
        public bool IsOnMotorcycle { get; private set; }
        public bool IsPerformingActivity { get; set; }
        public bool IsRagdoll { get; private set; }
        public bool IsSpeeding => Violations.IsSpeeding;
        public bool IsStill { get; private set; }
        public bool IsStunned { get; private set; }
        public bool IsViolatingAnyTrafficLaws => Violations.IsViolatingAnyTrafficLaws;
        public bool IsVisiblyArmed { get; private set; }
        public bool IsWanted => Game.LocalPlayer.WantedLevel > 0;
        public bool HasCriminalHistory => CriminalHistory.HasHistory;
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
                    NativeFunction.CallByName<int>("STAT_GET_INT", NativeHelper.CashHash(Settings.SettingsManager.GeneralSettings.MainCharacterToAlias), &CurrentCash, -1);
                }
                return CurrentCash;
            }
        }
        public Vector3 PlacePoliceLastSeenPlayer { get; set; }
        public Vector3 Position => Game.LocalPlayer.Character.Position;
        public bool RecentlyResistedArrest => Respawning.RecentlyResistedArrest;
        public bool RecentlyBusted => GameTimeLastBusted != 0 && Game.GameTime - GameTimeLastBusted <= 5000;
        public bool RecentlyShot => GameTimeLastShot != 0 && !RecentlyStartedPlaying && Game.GameTime - GameTimeLastShot <= 3000;
        public bool RecentlyStartedPlaying => GameTimeStartedPlaying != 0 && Game.GameTime - GameTimeStartedPlaying <= 3000;//10000
        public bool RecentlySetWanted => GameTimeLastSetWanted != 0 && Game.GameTime - GameTimeLastSetWanted <= 5000;
        public List<VehicleExt> ReportedStolenVehicles => TrackedVehicles.Where(x => x.NeedsToBeReportedStolen && !x.HasBeenDescribedByDispatch && !x.AddedToReportedStolenQueue).ToList();
        public List<LicensePlate> SpareLicensePlates { get; private set; } = new List<LicensePlate>();
        public string PlayerName { get; private set; }
        public uint TargettingHandle
        {
            get => targettingHandle;
            private set
            {
                if (targettingHandle != value)
                {
                    targettingHandle = value;
                    OnTargettingHandleChanged();
                }
            }
        }//move or delete?
        public uint TimeInSearchMode => SearchMode.TimeInSearchMode;
        public uint TimeToRecognize
        {
            get
            {
                uint Time = Settings.SettingsManager.PlayerSettings.Recognize_BaseTime;
                if (TimeControllable.IsNight)
                {
                    Time += Settings.SettingsManager.PlayerSettings.Recognize_NightPenalty;
                }
                else if (IsInVehicle)
                {
                    Time += Settings.SettingsManager.PlayerSettings.Recognize_VehiclePenalty;
                }
                return Time;
            }
        }//move or delete?
        public List<VehicleExt> TrackedVehicles { get; private set; } = new List<VehicleExt>();
        public VehicleExt VehicleGettingInto { get; private set; }
        public float VehicleSpeed { get; private set; }//move or delete?
        public float VehicleSpeedKMH => VehicleSpeed * 3.6f;
        public float VehicleSpeedMPH => VehicleSpeed * 2.23694f;
        public int WantedLevel => Game.LocalPlayer.WantedLevel;
        public string DebugLine1 => $"Player: {ModelName},{Game.LocalPlayer.Character.Handle} RcntStrPly: {RecentlyStartedPlaying} IsMovingDynam: {IsMovingDynamically} IsIntoxicated: {IsIntoxicated}";//$"{Interaction?.DebugString}";
        public string DebugLine2 => $"Vio: {Violations.LawsViolatingDisplay}";//$"WantedFor {PoliceResponse.HasBeenWantedFor} NotWantedFor {PoliceResponse.HasBeenNotWantedFor} CurrentWantedFor {PoliceResponse.HasBeenAtCurrentWantedLevelFor}";
        public string DebugLine3 => $"Rep: {PoliceResponse.ReportedCrimesDisplay}";//DynamicActivity?.DebugString;
        public string DebugLine4 => $"Obs: {PoliceResponse.ObservedCrimesDisplay}";
        public string DebugLine5 => CurrentVehicleDebugString;
        public string DebugLine6 => SearchMode.SearchModeDebug;//$"Rep {PoliceResponse.ReportedCrimesDisplay}";
        public string DebugLine7 => $"AnyPolice: CanSee: {AnyPoliceCanSeePlayer}, RecentlySeen: {AnyPoliceRecentlySeenPlayer}, CanHear: {AnyPoliceCanHearPlayer}, CanRecognize {AnyPoliceCanRecognizePlayer}";//PoliceResponse.DebugText;
        public string DebugLine8 => $"PlacePoliceLastSeenPlayer {PlacePoliceLastSeenPlayer}";//PoliceResponse.DebugText;
        public string DebugLine9 => CurrentVehicle != null ? $"IsEngineRunning: {CurrentVehicle.Engine.IsRunning}" : $"NO VEHICLE" + $" IsGettingIntoAVehicle: {IsGettingIntoAVehicle}, IsInVehicle: {IsInVehicle}";//$"Vio {Violations.LawsViolatingDisplay}";"";//Investigation.DebugText;
        public string DebugLine10 => $"Cop#: {EntityProvider.PoliceList.Count()} CopCar#: {EntityProvider.PoliceVehicleCount} Civ#: {EntityProvider.CivilianList.Count()} CivCar:#: {EntityProvider.CivilianVehicleCount} Tracked#: {TrackedVehicles.Count}";//$"IsMoving {IsMoving} IsMovingFast {IsMovingFast} IsMovingDynam {IsMovingDynamically} RcntStrPly {RecentlyStartedPlaying}";
        public string DebugLine11 { get; set; }
        public Scanner DebugScanner => Scanner;//temp for testing with debug
        public bool RecentlyRespawned => Respawning.RecentlyRespawned;
        public void AddCrime(Crime CrimeInstance, bool ByPolice, Vector3 Location, VehicleExt VehicleObserved, WeaponInformation WeaponObserved, bool HaveDescription, bool AnnounceCrime)
        {
            PoliceResponse.AddCrime(CrimeInstance, ByPolice, Location, VehicleObserved, WeaponObserved, HaveDescription);
            if(AnnounceCrime)
            {
                Scanner.AnnounceCrime(CrimeInstance, new CrimeSceneDescription(!IsInVehicle, ByPolice, Location, HaveDescription) { VehicleSeen = VehicleObserved, WeaponSeen = WeaponObserved, Speed = Game.LocalPlayer.Character.Speed });
            }
            if (!ByPolice && IsNotWanted)
            {
                Investigation.Start();
                
            }
        }
        public void Arrest()
        {
            BeingArrested = true;
            if (!IsBusted)
            {
                OnPlayerBusted();
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

            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~b~Personal Info", string.Format("~y~{0}", PlayerName), NotifcationText);
        }
        public void Dispose()
        {
            Investigation.Dispose(); //remove blip
            PoliceResponse.Dispose(); //same ^
            Interaction?.Dispose();
            SearchMode.Dispose();
            isActive = false;
            NativeFunction.Natives.SET_PED_CONFIG_FLAG<bool>(Game.LocalPlayer.Character, (int)PedConfigFlags._PED_FLAG_DISABLE_STARTING_VEH_ENGINE, false);
            // NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Game.LocalPlayer.Character, (int)PedConfigFlags._PED_FLAG_DISABLE_STARTING_VEH_ENGINE, false);
            MakeSober();
            Game.LocalPlayer.WantedLevel = 0;
            Game.TimeScale = 1f;
        }
        public void GiveMoney(int Amount)
        {
            int CurrentCash;
            uint PlayerCashHash = NativeHelper.CashHash(Settings.SettingsManager.GeneralSettings.MainCharacterToAlias);
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
                MaxWantedLastLife = 0;
                GameTimeStartedPlaying = Game.GameTime;
                Scanner.Reset();
                Update();
            }
            if (resetTimesDied)
            {
                Respawning.Reset();
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
        public void SetDemographics(string modelName, bool isMale, string playerName, int money)
        {
            ModelName = modelName;
            PlayerName = playerName;
            IsMale = isMale;
            SetMoney(money);
            EntryPoint.WriteToConsole($"PLAYER EVENT: SetDemographics MoneyToSet {money} Current: {Money} {NativeHelper.CashHash(Settings.SettingsManager.GeneralSettings.MainCharacterToAlias)}", 3);
        }
        public void SetMoney(int Amount)
        {
            NativeFunction.CallByName<int>("STAT_SET_INT", NativeHelper.CashHash(Settings.SettingsManager.GeneralSettings.MainCharacterToAlias), Amount, 1);
        }
        public void SetPlayerToLastWeapon()
        {
            if (Game.LocalPlayer.Character.Inventory.EquippedWeapon != null && LastWeaponHash != 0)
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)LastWeaponHash, true);
                //EntryPoint.WriteToConsole("SetPlayerToLastWeapon" + LastWeaponHash.ToString());
            }
        }
        public void SetWantedLevel(int desiredWantedLevel, string Reason, bool UpdateRecent)
        {
            if (UpdateRecent)
            {
                GameTimeLastSetWanted = Game.GameTime;
            }
            if (WantedLevel < desiredWantedLevel || (desiredWantedLevel == 0 && WantedLevel != 0))
            {
                NativeFunction.CallByName<bool>("SET_MAX_WANTED_LEVEL", desiredWantedLevel);
                Game.LocalPlayer.WantedLevel = desiredWantedLevel;
                if (desiredWantedLevel > 0)
                {
                    GameTimeWantedLevelStarted = Game.GameTime;
                }
                OnWantedLevelChanged();
                EntryPoint.WriteToConsole($"Set Wanted: From {WantedLevel} to {desiredWantedLevel} Reason: {Reason}", 3);
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
            SetWantedLevel(0, "Initial", true);
            SetUnarmed();
            SpareLicensePlates.Add(new LicensePlate(RandomItems.RandomString(8), 3, false));//random cali
            CurrentModelName = Game.LocalPlayer.Character.Model.Name;
            CurrentModelVariation = NativeHelper.GetPedVariation(Game.LocalPlayer.Character);
            if (Settings.SettingsManager.PlayerSettings.DisableAutoEngineStart)
            {
                NativeFunction.Natives.SET_PED_CONFIG_FLAG<bool>(Game.LocalPlayer.Character, (int)PedConfigFlags._PED_FLAG_DISABLE_STARTING_VEH_ENGINE, true);
            }
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
            if (Settings.SettingsManager.PlayerSettings.KeepRadioStationAutoTuned)
            {
                AutoTuneStation = Settings.SettingsManager.PlayerSettings.AutoTuneRadioStation;// "RADIO_19_USER";
            }
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
        private void UpdateData()
        {
            if (UpdateState == 0)
            {
                UpdateVehicleData();
                UpdateState++;
            }
            else if (UpdateState == 1)
            {
                UpdateWeaponData();
                UpdateState++;
            }
            else if (UpdateState == 2)
            {
                UpdateStateData();
                UpdateState = 0;
            }  
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
            if (CanPerformActivities && IsNearScenario)//currently isnearscenario is turned off
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
        private void MakeSober()
        {
            NativeFunction.Natives.SET_PED_IS_DRUNK<bool>(Game.LocalPlayer.Character, false);
            NativeFunction.Natives.RESET_PED_MOVEMENT_CLIPSET<bool>(Game.LocalPlayer.Character);
            NativeFunction.Natives.SET_PED_CONFIG_FLAG<bool>(Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, false);
            if (Settings.SettingsManager.UISettings.AllowScreenEffectReset)//this should be moved methinks
            {
                NativeFunction.Natives.CLEAR_TIMECYCLE_MODIFIER<int>();
                NativeFunction.Natives.x80C8B1846639BB19(0);
                NativeFunction.Natives.STOP_GAMEPLAY_CAM_SHAKING<int>(true);
            }
            //EntryPoint.WriteToConsole("Player Made Sober");
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
        public void ChangePlate(int Index)
        {
            if (!IsPerformingActivity && CanPerformActivities)
            {
                if (DynamicActivity != null)
                {
                    DynamicActivity.Cancel();
                }
                IsPerformingActivity = true;
                DynamicActivity = new PlateTheft(this, SpareLicensePlates[Index]);
                DynamicActivity.Start();
            }
        }
        public void ChangePlate(LicensePlate toChange)
        {
            if (!IsPerformingActivity && CanPerformActivities)
            {
                if (DynamicActivity != null)
                {
                    DynamicActivity.Cancel();
                }
                IsPerformingActivity = true;
                DynamicActivity = new PlateTheft(this, toChange);
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
                IsPerformingActivity = false;
            }
        }
        //Delegates
        public void ArrestWarrantUpdate() => CriminalHistory.Update();
        public void CheckInjured(PedExt MyPed) => Violations.AddInjured(MyPed);
        public void CheckMurdered(PedExt MyPed) => Violations.AddKilled(MyPed);
        public void DropWeapon() => WeaponDropping.DropWeapon();
        public void LocationUpdate() => CurrentLocation.Update();
        public void ScannerUpdate() => Scanner.Tick();
        public void LowerHands() => Surrendering.LowerHands();//needs to move
        public void RaiseHands() => Surrendering.RaiseHands();//needs to move
        public void SearchModeUpdate() => SearchMode.UpdateWanted();
        public void StopVanillaSearchMode() => SearchMode.StopVanilla();
        public void TrafficViolationsUpdate() => Violations.TrafficUpdate();
        public void UnSetArrestedAnimation(Ped character) => Surrendering.UnSetArrestedAnimation(character);//needs to move
        public void ViolationsUpdate() => Violations.Update();
        public void ResetScanner() => Scanner.Reset();
        public void RespawnAtGrave() => Respawning.RespawnAtGrave();
        public void RespawnAtHospital(GameLocation currentSelectedHospitalLocation) => Respawning.RespawnAtHospital(currentSelectedHospitalLocation);
        public void RespawnAtCurrentLocation(bool withInvicibility, bool resetWanted, bool clearCriminalHistory) => Respawning.RespawnAtCurrentLocation(withInvicibility, resetWanted, clearCriminalHistory);
        public void SurrenderToPolice(GameLocation currentSelectedSurrenderLocation) => Respawning.SurrenderToPolice(currentSelectedSurrenderLocation);
        public void BribePolice(int bribeAmount)
        {
            Respawning.BribePolice(bribeAmount);
            Scanner.OnBribedPolice();
        }
        public void ResistArrest() => Respawning.ResistArrest();
        public void PrintCriminalHistory() => CriminalHistory.PrintCriminalHistory();
        public void DeleteTrackedVehicles()
        {
            TrackedVehicles.Clear();
        }

        //Events
        public void OnAppliedWantedStats() => Scanner.OnAppliedWantedStats();
        public void OnInvestigationExpire()
        {
            PoliceResponse.Reset();
            Scanner.OnInvestigationExpire();
        }
        public void OnWantedSearchMode() => Scanner.OnWantedSearchMode();
        public void OnWantedActiveMode() => Scanner.OnWantedActiveMode();
        public void OnPoliceNoticeVehicleChange() => Scanner.OnPoliceNoticeVehicleChange();
        public void OnRequestedBackUp() => Scanner.OnRequestedBackUp();
        public void OnWeaponsFree() => Scanner.OnWeaponsFree();
        public void OnLethalForceAuthorized() => Scanner.OnLethalForceAuthorized();
        public void OnSuspectEluded()//runs before OnWantedLevelChanged
        {
            CriminalHistory.OnSuspectEluded(PoliceResponse.CrimesObserved.Select(x=> x.AssociatedCrime).ToList(),PlacePoliceLastSeenPlayer);
            Scanner.OnSuspectEluded();
        }
        private void OnWantedLevelChanged()//runs after OnSuspectEluded (If Applicable)
        {
            if (IsNotWanted && PreviousWantedLevel != 0)//Lost Wanted
            {
                CriminalHistory.OnLostWanted();
                PoliceResponse.OnLostWanted();
                EntityProvider.CivilianList.ForEach(x => x.CrimesWitnessed.Clear());
                EntryPoint.WriteToConsole($"PLAYER EVENT: LOST WANTED", 3);
            }
            else if (IsWanted && PreviousWantedLevel == 0)//Added Wanted Level
            {
                if (!RecentlySetWanted)//only allow my process to set the wanted level
                {
                    EntryPoint.WriteToConsole($"PLAYER EVENT: GAME AUTO SET WANTED TO {WantedLevel}, RESETTING", 3);
                    SetWantedLevel(0, "GAME AUTO SET WANTED", true);
                    //Game.LocalPlayer.WantedLevel = 0;
                }
                else
                {
                    Investigation.Reset();
                    PoliceResponse.OnBecameWanted();
                    EntryPoint.WriteToConsole($"PLAYER EVENT: BECAME WANTED", 3);
                }
            }
            else if(IsWanted && PreviousWantedLevel < WantedLevel)//Increased Wanted Level (can't decrease only remove for now.......)
            {
                PoliceResponse.OnWantedLevelIncreased();
                EntryPoint.WriteToConsole($"PLAYER EVENT: WANTED LEVEL INCREASED", 3);
            }
            else if (IsWanted && PreviousWantedLevel > WantedLevel)
            {
                //PoliceResponse.OnWantedLevelDecreased();
                EntryPoint.WriteToConsole($"PLAYER EVENT: WANTED LEVEL DECREASED", 3);
            }
            EntryPoint.WriteToConsole($"Wanted Changed: {WantedLevel} Previous: {PreviousWantedLevel}", 3);
            PreviousWantedLevel = Game.LocalPlayer.WantedLevel;
        }
        private void OnAiminChanged()
        {
            if (IsAiming)
            {
            }
            else
            {
            }
            EntryPoint.WriteToConsole($"PLAYER EVENT: IsAiming Changed to: {IsAiming}",5);
        }
        private void OnAimingInVehicleChanged()
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
            EntryPoint.WriteToConsole($"PLAYER EVENT: IsAimingInVehicle Changed to: {IsAimingInVehicle}",5);
        }
        private void OnPlayerBusted()
        {
            DiedInVehicle = IsInVehicle;
            IsBusted = true;
            BeingArrested = true;
            GameTimeLastBusted = Game.GameTime;
            HandsAreUp = false;
            Surrendering.SetArrestedAnimation(Game.LocalPlayer.Character, false, WantedLevel <= 2);//needs to move
            Game.LocalPlayer.HasControl = false;

            Scanner.OnPlayerBusted();
            EntryPoint.WriteToConsole($"PLAYER EVENT: IsBusted Changed to: {IsBusted}",3);
        }
        private void OnPlayerDied()
        {
            TimeControllable.PauseTime();
            DiedInVehicle = IsInVehicle;
            IsDead = true;
            GameTimeLastDied = Game.GameTime;
            Game.LocalPlayer.Character.Kill();
            Game.LocalPlayer.Character.Health = 0;
            Game.LocalPlayer.Character.IsInvincible = true;
            Game.TimeScale = 0.4f;
            Scanner.OnSuspectWasted();
            EntryPoint.WriteToConsole($"PLAYER EVENT: IsDead Changed to: {IsDead}",3);
        }
        private void OnGettingIntoAVehicleChanged()
        {
            if (IsGettingIntoAVehicle)
            {
                Vehicle VehicleTryingToEnter = Game.LocalPlayer.Character.VehicleTryingToEnter;
                int SeatTryingToEnter = Game.LocalPlayer.Character.SeatIndexTryingToEnter;
                if (VehicleTryingToEnter == null)
                {
                    return;
                }
                UpdateCurrentVehicle();
                if (CurrentVehicle != null)
                {
                    VehicleGettingInto = CurrentVehicle;
                    if(!CurrentVehicle.HasBeenEnteredByPlayer)
                    {
                        CurrentVehicle.AttemptToLock();
                    }
                    
                    if (IsNotHoldingEnter && VehicleTryingToEnter.Driver == null && VehicleTryingToEnter.LockStatus == (VehicleLockStatus)7 && !VehicleTryingToEnter.IsEngineOn)//no driver && Unlocked
                    {
                        EntryPoint.WriteToConsole($"PLAYER EVENT: LockPick Start", 3);
                        CarLockPick MyLockPick = new CarLockPick(this, VehicleTryingToEnter, SeatTryingToEnter);
                        MyLockPick.PickLock();
                    }
                    else if (IsNotHoldingEnter && SeatTryingToEnter == -1 && VehicleTryingToEnter.Driver != null && VehicleTryingToEnter.Driver.IsAlive) //Driver
                    {
                        EntryPoint.WriteToConsole($"PLAYER EVENT: CarJack Start", 3);
                        CarJack MyJack = new CarJack(this, CurrentVehicle, EntityProvider.CivilianList.FirstOrDefault(x => x.Pedestrian.Handle == VehicleTryingToEnter.Driver.Handle), SeatTryingToEnter, CurrentWeapon);
                        MyJack.Start();
                    }
                    else if (VehicleTryingToEnter.LockStatus == (VehicleLockStatus)7)
                    {
                        EntryPoint.WriteToConsole($"PLAYER EVENT: Car Break-In Start", 3);
                        CarBreakIn MyBreakIn = new CarBreakIn(this, VehicleTryingToEnter);
                        MyBreakIn.BreakIn();
                    }
                }
                else
                {
                    EntryPoint.WriteToConsole($"PLAYER EVENT: IsGettingIntoVehicle ERROR VEHICLE NOT FOUND (ARE YOU SCANNING ENOUGH?)", 3);
                }
            }
            else
            {
            }
            isGettingIntoVehicle = IsGettingIntoAVehicle;
            EntryPoint.WriteToConsole($"PLAYER EVENT: IsGettingIntoVehicleChanged to {IsGettingIntoAVehicle}, HoldingEnter {IsNotHoldingEnter}",3);
        }
        private void OnIsInVehicleChanged()
        {
            if (IsInVehicle)
            {
                //if (CurrentVehicle != null)
                //{
                //    EntryPoint.WriteToConsole("-------------------------------", 3);
                //    EntryPoint.WriteToConsole($" CurrentVehicle.Vehicle.Handle: {CurrentVehicle.Vehicle.Handle}", 3);
                //    EntryPoint.WriteToConsole($" CurrentVehicle.IsStolen: {CurrentVehicle.IsStolen}", 3);
                //    EntryPoint.WriteToConsole($" CurrentVehicle.WasReportedStolen: {CurrentVehicle.WasReportedStolen}", 3);
                //    EntryPoint.WriteToConsole($" CurrentVehicle.CopsRecognizeAsStolen: {CurrentVehicle.CopsRecognizeAsStolen}", 3);
                //    EntryPoint.WriteToConsole($" CurrentVehicle.NeedsToBeReportedStolen: {CurrentVehicle.NeedsToBeReportedStolen}", 3);
                //    EntryPoint.WriteToConsole($" CurrentVehicle.GameTimeToReportStolen: {CurrentVehicle.GameTimeToReportStolen}", 3);
                //    EntryPoint.WriteToConsole($" CurrentVehicle.CarPlate.IsWanted: {CurrentVehicle.CarPlate.IsWanted}", 3);
                //    EntryPoint.WriteToConsole($" CurrentVehicle.CarPlate.PlateNumber: {CurrentVehicle.CarPlate.PlateNumber}", 3);
                //    EntryPoint.WriteToConsole($" CurrentVehicle.OriginalLicensePlate.IsWanted: {CurrentVehicle.OriginalLicensePlate.IsWanted}", 3);
                //    EntryPoint.WriteToConsole($" CurrentVehicle.OriginalLicensePlate.PlateNumber: {CurrentVehicle.OriginalLicensePlate.PlateNumber}", 3);
                //    EntryPoint.WriteToConsole($" CurrentVehicle.HasOriginalPlate: {CurrentVehicle.HasOriginalPlate}", 3);
                //    EntryPoint.WriteToConsole($" CurrentVehicle.HasBeenDescribedByDispatch: {CurrentVehicle.HasBeenDescribedByDispatch}", 3);
                //    EntryPoint.WriteToConsole("-------------------------------", 3);
                //}
            }
            else
            {
                if (IsWanted && AnyPoliceCanSeePlayer)
                {
                    Scanner.OnGotOutOfVehicle();
                }
            }
            EntryPoint.WriteToConsole($"PLAYER EVENT: IsInVehicle to {IsInVehicle}",3);
        }
        private void OnTargettingHandleChanged()
        {
            if (TargettingHandle != 0)
            {
                CurrentTargetedPed = EntityProvider.GetPedExt(TargettingHandle);
                if (!IsInteracting && CanHoldUpTargettedPed && CurrentTargetedPed != null && CurrentTargetedPed.CanBeMugged)
                {
                    StartHoldUp();
                }
            }
            else
            {
                CurrentTargetedPed = null;
            }
            EntryPoint.WriteToConsole($"PLAYER EVENT: CurrentTargetedPed to {CurrentTargetedPed?.Pedestrian?.Handle}",5);
        }

        //General Updates
        private void UpdateCurrentVehicle()
        {
            bool IsGettingIntoVehicle = Game.LocalPlayer.Character.IsGettingIntoVehicle;
            bool IsInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
            if (!IsInVehicle && !IsGettingIntoVehicle)
            {
                CurrentVehicle = null;
                return;
            }
            Vehicle vehicle;
            if (IsGettingIntoVehicle)
            {
                vehicle = Game.LocalPlayer.Character.VehicleTryingToEnter;
            }
            else
            {
                vehicle = Game.LocalPlayer.Character.CurrentVehicle;
            }
            if (!vehicle.Exists())
            {
                CurrentVehicle = null;
                return;
            }
            VehicleExt existingVehicleExt = EntityProvider.GetVehicleExt(vehicle);
            if (existingVehicleExt == null)
            {
                VehicleExt createdVehicleExt = new VehicleExt(vehicle);
                TrackedVehicles.Add(createdVehicleExt);
                existingVehicleExt = createdVehicleExt;
            }
            if(!TrackedVehicles.Any(x => x.Vehicle.Handle == vehicle.Handle))
            {
                TrackedVehicles.Add(existingVehicleExt);
            }
            if (IsInVehicle && !existingVehicleExt.HasBeenEnteredByPlayer)
            {
                existingVehicleExt.SetAsEntered();
            }
            existingVehicleExt.Update(AutoTuneStation); 
            CurrentVehicle = existingVehicleExt;
        }
        private void UpdateLookedAtPed()
        {
            if (Game.GameTime - GameTimeLastUpdatedLookedAtPed >= 1000)//750
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
                    CurrentLookedAtPed = EntityProvider.GetPedExt(result.HitEntity.Handle);
                }
                else
                {
                    CurrentLookedAtPed = null;
                }

                //CurrentLookedAtPed = EntityProvider.CivilianList.Where(x => x.DistanceToPlayer <= 4f && !x.IsBehindPlayer).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
                GameTimeLastUpdatedLookedAtPed = Game.GameTime;


                GameFiber.Yield();
            }
        }
        //private void UpdateSpeedDispay()
        //{
        //    if (CurrentVehicle != null)//was game.localpalyer.character.isinanyvehicle(false)
        //    {
        //        CurrentSpeedDisplay = "";
        //        if (!CurrentVehicle.Engine.IsRunning)
        //        {
        //            CurrentSpeedDisplay = "ENGINE OFF";
        //        }
        //        else
        //        {
        //            string ColorPrefx = "~s~";
        //            if (IsSpeeding)
        //            {
        //                ColorPrefx = "~r~";
        //            }
        //            if (CurrentLocation.CurrentStreet != null)
        //            {
        //                CurrentSpeedDisplay = $"{ColorPrefx}{Math.Round(VehicleSpeedMPH, MidpointRounding.AwayFromZero)} ~s~MPH ({CurrentLocation.CurrentStreet.SpeedLimitMPH})";
        //            }
        //        }
        //        if (IsViolatingAnyTrafficLaws)
        //        {
        //            CurrentSpeedDisplay += " !";
        //        }
        //        CurrentSpeedDisplay += "~n~" + CurrentVehicle.FuelTank.UIText;
        //    }
        //}
        public void UpdateStateData()
        {
            if (Game.LocalPlayer.Character.IsDead && !IsDead)
            {
                OnPlayerDied();
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
                OnPlayerBusted();
            }
            if (IsAliveAndFree && !Game.LocalPlayer.Character.IsDead)
            {
                MaxWantedLastLife = WantedLevel;
            }
            if (PreviousWantedLevel != Game.LocalPlayer.WantedLevel)
            {
                OnWantedLevelChanged();
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
            //works fine, just turned off for now
            //if (IsNotWanted && !IsInVehicle)//meh only on not wanted for now, well see
            //{
            //    IsNearScenario = NativeFunction.Natives.DOES_SCENARIO_EXIST_IN_AREA<bool>(Position.X, Position.Y, Position.Z, 2f, true) && !NativeFunction.Natives.IS_SCENARIO_OCCUPIED<bool>(Position.X, Position.Y, Position.Z, 2f, true);
            //    ClosestScenario = new Scenario("", "Unknown");
            //    if (IsNearScenario)
            //    {
            //        foreach (Scenario scenario in Scenarios.ScenarioList)
            //        {
            //            if (NativeFunction.Natives.DOES_SCENARIO_OF_TYPE_EXIST_IN_AREA<bool>(Position.X, Position.Y, Position.Z, scenario.InternalName, 2f, true))
            //            {
            //                ClosestScenario = scenario;
            //                break;
            //            }
            //        }

            //    }
            //}
            //else
            //{
            //    IsNearScenario = false;
            //}
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
        public void UpdateVehicleData()
        {
            IsInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
            IsGettingIntoAVehicle = Game.LocalPlayer.Character.IsGettingIntoVehicle;
            if (IsInVehicle)
            {
                IsDriver = Game.LocalPlayer.Character.SeatIndex == -1;
                IsInAirVehicle = Game.LocalPlayer.Character.IsInAirVehicle;
                IsInAutomobile = !(IsInAirVehicle || Game.LocalPlayer.Character.IsInSeaVehicle || Game.LocalPlayer.Character.IsOnBike || Game.LocalPlayer.Character.IsInHelicopter);
                IsOnMotorcycle = Game.LocalPlayer.Character.IsOnBike;
                UpdateCurrentVehicle();
                GameFiber.Yield();
                IsHotWiring = CurrentVehicle != null && CurrentVehicle.Vehicle.Exists() && CurrentVehicle.Vehicle.MustBeHotwired;
                VehicleSpeed = Game.LocalPlayer.Character.CurrentVehicle.Speed;
              //  UpdateSpeedDispay();
                if (isHotwiring != IsHotWiring)
                {
                    if (IsHotWiring)
                    {
                        GameTimeStartedHotwiring = Game.GameTime;
                    }
                    else
                    {
                        EntryPoint.WriteToConsole($"PLAYER EVENT: HotWiring Took {Game.GameTime - GameTimeStartedHotwiring}",3);
                        GameTimeStartedHotwiring = 0;
                    }
                    EntryPoint.WriteToConsole($"PLAYER EVENT: IsHotWiring Changed to {IsHotWiring}",3);
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
               // CurrentVehicleDebugString = $"LSREngineOn: {CurrentVehicle.Engine.IsRunning} GTAEngineOn: {CurrentVehicle.Vehicle.IsEngineOn}";
            }
            else
            {
                IsDriver = false;
                CurrentVehicleDebugString = "";
                IsOnMotorcycle = false;
                IsInAutomobile = false;
                CurrentVehicle = null;
               // CurrentSpeedDisplay = "";
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
        public void UpdateWeaponData()
        {
            if (Game.LocalPlayer.Character.IsShooting)
            {
                GameTimeLastShot = Game.GameTime;
            }
            IsAiming = Game.LocalPlayer.IsFreeAiming;
            IsAimingInVehicle = IsInVehicle && IsAiming;
            UpdateVisiblyArmed();
            WeaponDescriptor PlayerCurrentWeapon = Game.LocalPlayer.Character.Inventory.EquippedWeapon;       
            if (PlayerCurrentWeapon != null)
            {
                
                CurrentWeaponHash = PlayerCurrentWeapon.Hash;
                CurrentWeapon = Weapons.GetCurrentWeapon(Game.LocalPlayer.Character);
                GameFiber.Yield();
            }
            else
            {
                
                CurrentWeaponHash = 0;
                CurrentWeapon = null;
            }
            if(Game.LocalPlayer.Character.Inventory.EquippedWeaponObject != null)
            {
                CurrentWeaponIsOneHanded = Game.LocalPlayer.Character.Inventory.EquippedWeaponObject.Model.Dimensions.X <= 0.4f;
            }
            else
            {
                CurrentWeaponIsOneHanded = false;
            }
            if (CurrentWeaponHash != 0 && PlayerCurrentWeapon.Hash != LastWeaponHash)
            {
                LastWeaponHash = PlayerCurrentWeapon.Hash;
            }


            



            WeaponDropping.Update();
            UpdateTargetedPed();
            GameFiber.Yield();
            UpdateLookedAtPed();
            GameFiber.Yield();
        }
    }
}