using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Locations;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

namespace LosSantosRED.lsr
{
    public class Player
    {
        private bool areStarsGreyedOut;
        private ArrestWarrant ArrestWarrant;
        private HealthState CurrentHealth;
        private LocationData CurrentLocation;
        private uint GameTimeLastBusted;
        private uint GameTimeLastDied;
        private uint GameTimeLastHurtCivilian;
        private uint GameTimeLastHurtCop;
        private uint GameTimeLastKilledCivilian;
        private uint GameTimeLastKilledCop;
        private uint GameTimeLastMoved;
        private uint GameTimeLastShot;
        private uint GameTimeLastSmashedVehicleWindow;
        private uint GameTimeLastStarsGreyedOut;
        private uint GameTimeLastStarsNotGreyedOut;
        private uint GameTimePoliceNoticedVehicleChange;
        private uint GameTimeStartedPlaying;
        private bool isAimingInVehicle;
        private bool isCarJacking;
        private bool isGettingIntoVehicle;
        private bool isInVehicle;
        private bool LeftEngineOn;
        private Mugging Mugging;
        private List<PedExt> PlayerKilledCivilians = new List<PedExt>();
        private List<PedExt> PlayerKilledCops = new List<PedExt>();
        private uint PoliceLastSeenVehicleHandle;
        private Respawning Respawning;
        private SearchMode SearchMode;
        private Surrendering Surrendering;
        private bool VanillaRespawn = true;
        private Violations Violations;
        private WeaponDropping WeaponDropping;
        public Player()
        {
            CurrentHealth = new HealthState(new PedExt(Game.LocalPlayer.Character));
            Mugging = new Mugging();
            SearchMode = new SearchMode(this);
            Violations = new Violations();
            Respawning = new Respawning();
            Surrendering = new Surrendering();
            WeaponDropping = new WeaponDropping();
            Investigations = new Investigations();
            ArrestWarrant = new ArrestWarrant(this);
            CurrentLocation = new LocationData(Game.LocalPlayer.Character);
            CurrentPoliceResponse = new PoliceResponse(this);
            NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Game.LocalPlayer.Character, (int)PedConfigFlags._PED_FLAG_DISABLE_STARTING_VEH_ENGINE, true);
            GameTimeStartedPlaying = Game.GameTime;
        }
        public bool AreStarsGreyedOut
        {
            get => areStarsGreyedOut;
            private set
            {
                if (areStarsGreyedOut != value)
                {
                    areStarsGreyedOut = value;
                    AreStarsGreyedOutChanged();
                }
            }
        }
        public bool BeingArrested { get; private set; }
        public Color BlipColor
        {
            get
            {
                if (SearchMode.IsInActiveMode)
                {
                    return Color.Red;
                }
                else
                {
                    return Color.Orange;
                }
            }
        }
        public float BlipSize//probably gonna remove these both and have a static size or do something else.
        {
            get
            {
                if (SearchMode.IsInActiveMode)
                {
                    return 100f;
                }
                else
                {
                    if (SearchMode.CurrentSearchTime == 0)
                    {
                        return 100f;
                    }
                    else
                    {
                        return ArrestWarrant.SearchRadius * SearchMode.TimeInSearchMode / SearchMode.CurrentSearchTime;
                    }
                }
            }
        }
        public bool CanDropWeapon => WeaponDropping.CanDropWeapon;
        public bool CanSurrender => Surrendering.CanSurrender;
        public Ped Character => Game.LocalPlayer.Character;
        public List<Crime> CivilianReportableCrimesViolating => Violations.CivilianReportableCrimesViolating;
        public Street CurrentCrossStreet => CurrentLocation.CurrentCrossStreet;
        public PoliceResponse CurrentPoliceResponse { get; private set; }
        public Vector3 CurrentPosition => Game.LocalPlayer.Character.Position;
        public VehicleExt CurrentSeenVehicle
        {
            get
            {
                if (CurrentVehicle == null)
                {
                    return VehicleGettingInto;
                }
                else
                {
                    return CurrentVehicle;
                }
            }
        }
        public WeaponInformation CurrentSeenWeapon
        {
            get
            {
                if (!IsInVehicle)
                {
                    return CurrentWeapon;
                }
                else
                {
                    return null;
                }
            }
        }
        public Street CurrentStreet => CurrentLocation.CurrentStreet;
        public VehicleExt CurrentVehicle { get; private set; }
        public WeaponInformation CurrentWeapon { get; private set; }
        public WeaponCategory CurrentWeaponCategory
        {
            get
            {
                if (CurrentWeapon != null)
                {
                    return CurrentWeapon.Category;
                }
                return WeaponCategory.Unknown;
            }
        }
        public WeaponHash CurrentWeaponHash { get; set; }
        public Zone CurrentZone
        {
            get
            {
                return CurrentLocation.CurrentZone;
            }
        }
        public bool DiedInVehicle { get; private set; }
        public bool HandsAreUp { get; set; }
        public Investigations Investigations { get; private set; }
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
        public bool IsAliveAndFree
        {
            get
            {
                if (IsBusted || IsDead)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public bool IsAttemptingToSurrender
        {
            get
            {
                if (HandsAreUp && !CurrentPoliceResponse.IsWeaponsFree)
                    return true;
                else
                    return false;
            }
        }
        public bool IsBreakingIntoCar
        {
            get
            {
                if (IsCarJacking || IsLockPicking || IsHotWiring || RecentlySmashedWindow)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool IsBustable => IsAliveAndFree && CurrentPoliceResponse.HasBeenWantedFor >= 3000 && !Surrendering.IsCommitingSuicide && !RecentlyBusted && !IsInVehicle;
        public bool IsBusted { get; private set; }
        public bool IsCarJacking { get; private set; }
        public bool IsChangingLicensePlates { get; set; }
        public bool IsCommitingSuicide => Surrendering.IsCommitingSuicide;
        public bool IsConsideredArmed { get; private set; }
        public bool IsDead { get; private set; }
        public bool IsDrunk { get; private set; }
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
        public bool IsHotWiring { get; private set; }
        public bool IsInAirVehicle { get; private set; }
        public bool IsInAutomobile { get; private set; }
        public bool IsIncapacitated => IsStunned || IsRagdoll;
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
        public bool IsMugging => Mugging.IsMugging;
        public bool IsNotWanted => Game.LocalPlayer.WantedLevel == 0;
        public bool IsOffroad => CurrentLocation.IsOffroad;
        public bool IsOnMotorcycle { get; private set; }
        public bool IsPersonOfInterest => ArrestWarrant.IsPersonOfInterest;
        public bool IsRagdoll { get; private set; }
        public bool IsSpeeding => Violations.IsSpeeding;
        public bool IsStationary => GameTimeLastMoved != 0 && Game.GameTime - GameTimeLastMoved >= 1500;
        public bool IsStunned { get; private set; }
        public bool IsViolatingAnyAudioBasedCivilianReportableCrime => Violations.IsViolatingAnyAudioBasedCivilianReportableCrime;
        public bool IsViolatingAnyCivilianReportableCrime => Violations.IsViolatingAnyCivilianReportableCrime;
        public bool IsViolatingAnyTrafficLaws => Violations.IsViolatingAnyTrafficLaws;
        public bool IsWanted => Game.LocalPlayer.WantedLevel > 0;
        public bool KilledAnyCops => PlayerKilledCops.Any();
        public WeaponHash LastWeaponHash { get; set; }
        public string LawsViolatingDisplay => Violations.LawsViolatingDisplay;
        public bool LethalForceAuthorized => ArrestWarrant.LethalForceAuthorized;
        public int MaxWantedLastLife { get; set; }
        public int MaxWantedLevel => ArrestWarrant.MaxWantedLevel;
        public int Money
        {
            get
            {
                int CurrentCash;
                unsafe
                {
                    NativeFunction.CallByName<int>("STAT_GET_INT", Natives.CashHash(Mod.DataMart.Settings.SettingsManager.General.MainCharacterToAlias), &CurrentCash, -1);
                }
                return CurrentCash;
            }
        }
        public bool NearCivilianMurderVictim => PlayerKilledCivilians.Any(x => x.Pedestrian.Exists() && x.Pedestrian.DistanceTo2D(Game.LocalPlayer.Character) <= 9f);
        public bool NearCopMurderVictim => PlayerKilledCops.Any(x => x.Pedestrian.Exists() && x.Pedestrian.DistanceTo2D(Game.LocalPlayer.Character) <= 15f);
        public bool PoliceRecentlyNoticedVehicleChange => GameTimePoliceNoticedVehicleChange != 0 && Game.GameTime - GameTimePoliceNoticedVehicleChange <= 15000;
        public bool RecentlyAppliedWantedStats => ArrestWarrant.RecentlyAppliedWantedStats;
        public bool RecentlyBribedPolice => Respawning.RecentlyBribedPolice;
        public bool RecentlyBusted => GameTimeLastBusted != 0 && Game.GameTime - GameTimeLastBusted <= 5000;
        public bool RecentlyDied => GameTimeLastDied != 0 && Game.GameTime - GameTimeLastDied <= 5000;
        public bool RecentlyHurtCivilian => GameTimeLastHurtCivilian != 0 && Game.GameTime - GameTimeLastHurtCivilian <= 5000;
        public bool RecentlyHurtCop => GameTimeLastHurtCop != 0 && Game.GameTime - GameTimeLastHurtCop <= 5000;
        public bool RecentlyKilledCivilian => GameTimeLastKilledCivilian != 0 && Game.GameTime - GameTimeLastKilledCivilian <= 5000;
        public bool RecentlyKilledCop => GameTimeLastKilledCop != 0 && Game.GameTime - GameTimeLastKilledCop <= 5000;
        public bool RecentlyRespawned => Respawning.RecentlyRespawned;
        public bool RecentlyStartedPlaying => Game.GameTime - GameTimeStartedPlaying <= 5000;
        public bool RecentlySurrenderedToPolice => Respawning.RecentlySurrenderedToPolice;
        public List<VehicleExt> ReportedStolenVehicles => TrackedVehicles.Where(x => x.NeedsToBeReportedStolen).ToList();
        public string SearchModeDebug => string.Format("IsInSearchMode {0} IsInActiveMode {1}, TimeInSearchMode {2}, TimeInActiveMode {3}", SearchMode.IsInSearchMode, SearchMode.IsInActiveMode, SearchMode.TimeInSearchMode, SearchMode.TimeInActiveMode);
        public List<LicensePlate> SpareLicensePlates { get; private set; } = new List<LicensePlate>();
        public bool StarsRecentlyActive => GameTimeLastStarsNotGreyedOut != 0 && Game.GameTime - GameTimeLastStarsNotGreyedOut <= 1500;
        public bool StarsRecentlyGreyedOut => GameTimeLastStarsGreyedOut != 0 && Game.GameTime - GameTimeLastStarsGreyedOut <= 1500;
        public string SuspectsName { get; private set; }
        public int TimesDied { get; set; }
        public List<VehicleExt> TrackedVehicles { get; private set; } = new List<VehicleExt>();
        public VehicleExt VehicleGettingInto { get; private set; }
        public int WantedLevel => Game.LocalPlayer.WantedLevel;
        private bool RecentlySmashedWindow
        {
            get
            {
                if (GameTimeLastSmashedVehicleWindow == 0)
                {
                    return false;
                }
                else
                {
                    return Game.GameTime - GameTimeLastSmashedVehicleWindow <= 5000;
                }
            }
        }
        public void AddSpareLicensePlates()
        {
            SpareLicensePlates.Add(new LicensePlate(RandomItems.RandomString(8), 3, false));//random cali
        }
        public void ArrestWarrantUpdate()
        {
            ArrestWarrant.Update();
        }
        public void BribePolice(int bribeAmount)
        {
            Respawning.BribePolice(bribeAmount);
        }
        public bool CheckIsArmed()
        {
            if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && !Game.LocalPlayer.IsFreeAiming)
            {
                return false;
            }
            else if (Game.LocalPlayer.Character.Inventory.EquippedWeapon == null)
            {
                return false;
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
                return false;
            }
            else if (!NativeFunction.CallByName<bool>("IS_PLAYER_CONTROL_ON", Game.LocalPlayer))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public void CommitSuicide()
        {
            Surrendering.CommitSuicide(Game.LocalPlayer.Character);
        }
        public void DisplayPlayerNotification()
        {
            string NotifcationText = "Warrants: ~g~None~s~";
            if (CurrentPoliceResponse.CurrentCrimes.CommittedAnyCrimes)
            {
                NotifcationText = "Wanted For:" + CurrentPoliceResponse.CurrentCrimes.PrintCrimes();
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
            ActivateVanillaRespawn();
            NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Game.LocalPlayer.Character, (int)PedConfigFlags._PED_FLAG_DISABLE_STARTING_VEH_ENGINE, false);
        }
        public void DropWeapon()
        {
            WeaponDropping.DropWeapon();
        }
        public void GiveMoney(int Amount)
        {
            int CurrentCash;
            uint PlayerCashHash = Natives.CashHash(Mod.DataMart.Settings.SettingsManager.General.MainCharacterToAlias);
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
        public void GiveName()
        {
            string ModelName = Game.LocalPlayer.Character.Model.Name.ToLower();
            if (ModelName == "player_zero")
            {
                SuspectsName = "Michael De Santa";
            }
            else if (ModelName == "player_one")
            {
                SuspectsName = "Franklin Clinton";
            }
            else if (ModelName == "player_two")
            {
                SuspectsName = "Trevor Philips";
            }
            else
            {
                SuspectsName = "John Doe";
            }
        }
        public void GiveName(string modelBeforeSpoof, bool isMale)
        {
            if (modelBeforeSpoof.ToLower() == "player_zero")
            {
                SuspectsName = "Michael De Santa";
            }
            else if (modelBeforeSpoof.ToLower() == "player_one")
            {
                SuspectsName = "Franklin Clinton";
            }
            else if (modelBeforeSpoof.ToLower() == "player_two")
            {
                SuspectsName = "Trevor Philips";
            }
            else
            {
                SuspectsName = Mod.DataMart.Names.GetRandomName(isMale);
            }
        }
        public void Injured(PedExt MyPed)
        {
            if (MyPed.IsCop)
            {
                GameTimeLastHurtCop = Game.GameTime;
            }
            else
            {
                GameTimeLastHurtCivilian = Game.GameTime;
            }
            Mod.Debug.WriteToLog("PedWoundSystem", string.Format("Player Hurt {0}, IsCop: {1}", MyPed.Pedestrian.Handle, MyPed.IsCop));
        }
        public void Killed(PedExt MyPed)
        {
            if (MyPed.IsCop)
            {
                PlayerKilledCops.Add(MyPed);
                GameTimeLastKilledCop = Game.GameTime;
                GameTimeLastHurtCop = Game.GameTime;
            }
            else
            {
                PlayerKilledCivilians.Add(MyPed);
                GameTimeLastKilledCivilian = Game.GameTime;
                GameTimeLastHurtCivilian = Game.GameTime;
            }
            Mod.Debug.WriteToLog("PedWoundSystem", string.Format("Player Killed {0}, IsCop: {1}", MyPed.Pedestrian.Handle, MyPed.IsCop));
        }
        public void LocationUpdate()
        {
            CurrentLocation.Update();
        }
        public void LowerHands()
        {
            Surrendering.LowerHands();
        }
        public void MuggingUpdate()
        {
            Mugging.Update();
        }
        public void RaiseHands()
        {
            Surrendering.RaiseHands();
        }
        public bool RecentlyShot(int duration)
        {
            if (GameTimeLastShot == 0)
            {
                return false;
            }
            else if (RecentlyStartedPlaying)
            {
                return false;
            }
            else if (Respawning.RecentlyRespawned)
            {
                return false;
            }
            else if (Game.GameTime - GameTimeLastShot <= duration) //15000
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void ResetInjuredPeds()
        {
            GameTimeLastHurtCivilian = 0;
            GameTimeLastKilledCivilian = 0;
            GameTimeLastHurtCop = 0;
            GameTimeLastKilledCop = 0;
        }
        public void ResetState(bool IncludeMaxWanted)
        {
            IsDead = false;
            IsBusted = false;
            Game.LocalPlayer.HasControl = true;
            BeingArrested = false;
            TimesDied = 0;
            LastWeaponHash = 0;
            WeaponDropping.Reset();
            if (IncludeMaxWanted)
            {
                ArrestWarrant.Reset();
                MaxWantedLastLife = 0; //this might be a problem in here and might need to be removed
            }
        }
        public void ResistArrest()
        {
            Respawning.ResistArrest();
        }
        public void RespawnAtGrave()
        {
            Respawning.RespawnAtGrave();
        }
        public void RespawnAtHospital(GameLocation currentSelectedHospitalLocation)
        {
            Respawning.RespawnAtHospital(currentSelectedHospitalLocation);
        }
        public void RespawnInPlace(bool AsOldCharacter)
        {
            Respawning.RespawnInPlace(AsOldCharacter);
        }
        public void Restart()
        {
            CurrentHealth = new HealthState(new PedExt(Game.LocalPlayer.Character));
            Mugging = new Mugging();
            SearchMode = new SearchMode(this);
            Violations = new Violations();
            Respawning = new Respawning();
            Surrendering = new Surrendering();
            WeaponDropping = new WeaponDropping();
            Investigations = new Investigations();
            ArrestWarrant = new ArrestWarrant(this);
            CurrentLocation = new LocationData(Game.LocalPlayer.Character);
            CurrentPoliceResponse = new PoliceResponse(this);
            NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Game.LocalPlayer.Character, (int)PedConfigFlags._PED_FLAG_DISABLE_STARTING_VEH_ENGINE, true);
            GameTimeStartedPlaying = Game.GameTime;
        }
        public void SearchModeUpdate()
        {
            SearchMode.UpdateWanted();
        }
        public void SetCarJacking(bool valueToSet)
        {
            isCarJacking = valueToSet;
        }
        public void SetMoney(int Amount)
        {
            NativeFunction.CallByName<int>("STAT_SET_INT", Natives.CashHash(Mod.DataMart.Settings.SettingsManager.General.MainCharacterToAlias), Amount, 1);
        }
        public void SetPlayerToLastWeapon()
        {
            if (Game.LocalPlayer.Character.Inventory.EquippedWeapon != null && LastWeaponHash != 0)
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)LastWeaponHash, true);
                Mod.Debug.WriteToLog("SetPlayerToLastWeapon", LastWeaponHash.ToString());
            }
        }
        public void SetShot()
        {
            GameTimeLastShot = Game.GameTime;
        }
        public void SetUnarmed()
        {
            if (!(Game.LocalPlayer.Character.Inventory.EquippedWeapon == null))
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)2725352035, true); //Unequip weapon so you don't get shot
            }
        }
        public void StopVanillaSearchMode()
        {
            SearchMode.StopVanilla();
        }
        public void StoreCriminalHistory(CriminalHistory currentCrimes)
        {
            ArrestWarrant.StoreCriminalHistory(currentCrimes);
        }
        public void SurrenderToPolice(GameLocation currentSelectedSurrenderLocation)
        {
            Respawning.SurrenderToPolice(currentSelectedSurrenderLocation);
        }
        public void TrafficViolationsUpdate()
        {
            Violations.TrafficUpdate();
        }
        public void UnDie()
        {
            Respawning.UnDie();
        }
        public void UnSetArrestedAnimation(Ped character)
        {
            Surrendering.UnSetArrestedAnimation(character);
        }
        public void Update()
        {
            if (VanillaRespawn)
            {
                TerminateVanillaRespawnController();
            }
            TerminateVanillaRespawnScripts();
            TerminateVanillaHealthRecharge();

            UpdateData();
            UpdateState();
            TrackedVehiclesTick();
            CurrentHealth.Update();
            WeaponDropping.Tick();
        }
        public void ViolationsUpdate()
        {
            Violations.Update();
        }
        private void ActivateVanillaRespawn()
        {
            var MyPtr = Game.GetScriptGlobalVariableAddress(4); //the script id for respawn_controller
            Marshal.WriteInt32(MyPtr, 0); //setting it to 0 turns it on somehow?
            Game.StartNewScript("respawn_controller");
            Game.StartNewScript("selector");
            VanillaRespawn = true;
        }
        private void AreStarsGreyedOutChanged()
        {
            if (AreStarsGreyedOut)
            {
                GameTimeLastStarsGreyedOut = Game.GameTime;
            }
            else
            {
                GameTimeLastStarsNotGreyedOut = Game.GameTime;
            }
            Mod.Debug.WriteToLog("ValueChecker", string.Format("AreStarsGreyedOut Changed to: {0}", AreStarsGreyedOut));
        }
        private void BustedEvent()
        {
            DiedInVehicle = IsInVehicle;
            IsBusted = true;
            BeingArrested = true;
            GameTimeLastBusted = Game.GameTime;
            // Game.LocalPlayer.Character.Tasks.Clear();
            // General.TransitionToMediumMo();//was slowmo slowmo lets try this
            HandsAreUp = false;
            Surrendering.SetArrestedAnimation(Game.LocalPlayer.Character, false, WantedLevel <= 2);
            var HandleBusted = GameFiber.StartNew(delegate
            {
                GameFiber.Wait(1000);
                Mod.Menu.ShowBustedMenu();
            }, "HandleBusted");
            Mod.Debug.GameFibers.Add(HandleBusted);
            Game.LocalPlayer.HasControl = false;
        }
        private void DeathEvent()
        {
            Mod.World.PauseTime();
            DiedInVehicle = IsInVehicle;
            IsDead = true;
            GameTimeLastDied = Game.GameTime;
            Game.LocalPlayer.Character.Kill();
            Game.LocalPlayer.Character.Health = 0;
            Game.LocalPlayer.Character.IsInvincible = true;
            TransitionToSlowMo();
            var HandleDeath = GameFiber.StartNew(delegate
            {
                GameFiber.Wait(1000);
                Mod.Menu.ShowDeathMenu();
            }, "HandleDeath");
            Mod.Debug.GameFibers.Add(HandleDeath);
        }
        private void IsAimingInVehicleChanged()
        {
            if (IsAimingInVehicle)
            {
                CurrentVehicle.SetDriverWindow(true);
            }
            else
            {
                CurrentVehicle.SetDriverWindow(false);
            }
            Mod.Debug.WriteToLog("ValueChecker", string.Format("IsAimingInVehicle Changed to: {0}", IsAimingInVehicle));
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
                VehicleExt MyCar = Mod.World.GetVehicle(VehicleTryingToEnter);
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
                    if (Mod.Input.IsHoldingEnter && VehicleTryingToEnter.Driver == null && VehicleTryingToEnter.LockStatus == (VehicleLockStatus)7 && !VehicleTryingToEnter.IsEngineOn)//no driver && Unlocked
                    {
                        //Mod.Debug.WriteToLog("IsGettingIntoVehicleChanged", string.Format("1 Handle: {0} LockPick", EnteringVehicle.Handle));
                        CarLockPick MyLockPick = new CarLockPick(VehicleTryingToEnter, SeatTryingToEnter);
                        MyLockPick.PickLock();
                    }
                    else if (Mod.Input.IsHoldingEnter && SeatTryingToEnter == -1 && VehicleTryingToEnter.Driver != null && VehicleTryingToEnter.Driver.IsAlive) //Driver
                    {
                        //Mod.Debug.WriteToLog("IsGettingIntoVehicleChanged", string.Format("2 Handle: {0} CarJack", EnteringVehicle.Handle));
                        CarJack MyJack = new CarJack(VehicleTryingToEnter, VehicleTryingToEnter.Driver, SeatTryingToEnter);
                        MyJack.StartCarJack();
                    }

                    if (VehicleTryingToEnter.LockStatus == (VehicleLockStatus)7)
                    {
                        GameTimeLastSmashedVehicleWindow = Game.GameTime;
                    }
                    //else
                    //{
                    //    //Mod.Debug.WriteToLog("IsGettingIntoVehicleChanged", string.Format("3 Handle: {0}, LockStatus: {1}, MustBeHotwired: {2}", EnteringVehicle.Handle, EnteringVehicle.LockStatus, EnteringVehicle.MustBeHotwired));
                    //}
                }
            }
            isGettingIntoVehicle = IsGettingIntoAVehicle;
            Mod.Debug.WriteToLog("IsGettingIntoVehicleChanged", string.Format(" to {0}", IsGettingIntoAVehicle));
        }
        private void IsInVehicleChanged()
        {
            if (IsInVehicle)
            {
            }
            else
            {
            }
            Mod.Debug.WriteToLog("ValueChecker", string.Format("IsInVehicle Changed: {0}", IsInVehicle));
        }
        private void StartManualArrest()
        {
            BeingArrested = true;
            if (!IsBusted)
            {
                BustedEvent();
            }
        }
        private void TerminateVanillaHealthRecharge()
        {
            NativeFunction.CallByName<bool>("SET_PLAYER_HEALTH_RECHARGE_MULTIPLIER", Game.LocalPlayer, 0f);
        }
        private void TerminateVanillaRespawnController()
        {
            var MyPtr = Game.GetScriptGlobalVariableAddress(4); //the script id for respawn_controller
            Marshal.WriteInt32(MyPtr, 1); //setting it to 1 turns it off somehow?
            Game.TerminateAllScriptsWithName("respawn_controller");
            VanillaRespawn = false;
        }
        private void TerminateVanillaRespawnScripts()
        {
            Game.DisableAutomaticRespawn = true;
            Game.FadeScreenOutOnDeath = false;
            Game.TerminateAllScriptsWithName("selector");
            NativeFunction.Natives.x1E0B4DC0D990A4E7(false);
            NativeFunction.Natives.x21FFB63D8C615361(true);
        }
        private void TrackedVehiclesTick()
        {
            TrackedVehicles.RemoveAll(x => !x.Vehicle.Exists());
            if (IsInVehicle)
            {
                if (CurrentVehicle == null)
                {
                    return;
                }

                if (Mod.World.AnyPoliceCanSeePlayer && IsWanted && !AreStarsGreyedOut)
                {
                    if (PoliceLastSeenVehicleHandle != 0 && PoliceLastSeenVehicleHandle != CurrentSeenVehicle.Vehicle.Handle && !CurrentSeenVehicle.HasBeenDescribedByDispatch)
                    {
                        GameTimePoliceNoticedVehicleChange = Game.GameTime;
                        Mod.Debug.WriteToLog("PlayerState", string.Format("PoliceRecentlyNoticedVehicleChange {0}", GameTimePoliceNoticedVehicleChange));
                    }
                    PoliceLastSeenVehicleHandle = CurrentSeenVehicle.Vehicle.Handle;
                }

                if (Mod.World.AnyPoliceCanRecognizePlayer && IsWanted && !AreStarsGreyedOut)
                {
                    CurrentVehicle.UpdateDescription();
                }
            }
        }
        private void TransitionToSlowMo()
        {
            Game.TimeScale = 0.4f;//Stuff below works, could add it back, it just doesnt really do much
                                  //GameFiber Transition = GameFiber.StartNew(delegate
                                  //{
                                  //    int WaitTime = 100;
                                  //    while (Game.TimeScale > 0.4f)
                                  //    {
                                  //        Game.TimeScale -= 0.05f;
                                  //        GameFiber.Wait(WaitTime);
                                  //        if (WaitTime <= 200)
                                  //            WaitTime += 1;
                                  //    }

            //}, "TransitionIn");
            //Debugging.GameFibers.Add(Transition);
        }
        private VehicleExt UpdateCurrentVehicle()
        {
            if (!Game.LocalPlayer.Character.IsInAnyVehicle(false) && !Game.LocalPlayer.Character.IsGettingIntoVehicle)
            {
                return null;
            }
            Vehicle CurrVehicle = null;
            if (Game.LocalPlayer.Character.IsGettingIntoVehicle)
            {
                CurrVehicle = Game.LocalPlayer.Character.VehicleTryingToEnter;
            }
            else
            {
                CurrVehicle = Game.LocalPlayer.Character.CurrentVehicle;
            }
            if (!CurrVehicle.Exists())
            {
                return null;
            }
            VehicleExt ToReturn = TrackedVehicles.Where(x => x.Vehicle.Handle == CurrVehicle.Handle).FirstOrDefault();
            if (ToReturn == null)
            {
                VehicleExt MyNewCar = new VehicleExt(CurrVehicle, Game.GameTime);
                TrackedVehicles.Add(MyNewCar);
                return MyNewCar;
            }
            ToReturn.SetAsEntered();
            ToReturn.Update();
            LeftEngineOn = ToReturn.Vehicle.IsEngineOn;
            return ToReturn;
        }
        private void UpdateData()
        {
            IsInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
            IsGettingIntoAVehicle = Game.LocalPlayer.Character.IsGettingIntoVehicle;
            if (IsInVehicle)
            {
                IsInAirVehicle = Game.LocalPlayer.Character.IsInAirVehicle;
                if (IsInAirVehicle || Game.LocalPlayer.Character.IsInSeaVehicle || Game.LocalPlayer.Character.IsOnBike || Game.LocalPlayer.Character.IsInHelicopter)
                {
                    IsInAutomobile = false;
                }
                else
                {
                    IsInAutomobile = true;
                }

                if (Game.LocalPlayer.Character.IsOnBike)
                {
                    IsOnMotorcycle = true;
                }
                else
                {
                    IsOnMotorcycle = false;
                }
                CurrentVehicle = UpdateCurrentVehicle();
                if (CurrentVehicle != null && CurrentVehicle.Vehicle.Exists() && CurrentVehicle.Vehicle.MustBeHotwired)
                {
                    IsHotWiring = true;
                }
                else
                {
                    IsHotWiring = false;
                }
                if (Mod.Input.IsMoveControlPressed || Game.LocalPlayer.Character.Speed >= 0.1f)
                {
                    GameTimeLastMoved = Game.GameTime;
                }
            }
            else
            {
                IsOnMotorcycle = false;
                IsInAutomobile = false;
                CurrentVehicle = null;
                if (Game.LocalPlayer.Character.Speed >= 0.2f)
                {
                    GameTimeLastMoved = Game.GameTime;
                }
                NativeFunction.CallByName<bool>("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", false);
                if (Game.LocalPlayer.Character.LastVehicle.Exists())
                {
                    Game.LocalPlayer.Character.LastVehicle.IsEngineOn = LeftEngineOn;
                }
            }
            if (CurrentLocation.CharacterToLocate.Handle != Game.LocalPlayer.Character.Handle)
            {
                CurrentLocation.CharacterToLocate = Game.LocalPlayer.Character;
            }

            if (Game.LocalPlayer.Character.IsShooting)
            {
                GameTimeLastShot = Game.GameTime;
            }

            IsConsideredArmed = CheckIsArmed();
            IsAimingInVehicle = IsInVehicle && Game.LocalPlayer.IsFreeAiming;
            IsStunned = Game.LocalPlayer.Character.IsStunned;
            IsRagdoll = Game.LocalPlayer.Character.IsRagdoll;

            if (isCarJacking)//is manually set
            {
                IsCarJacking = isCarJacking;
            }
            else
            {
                IsCarJacking = Game.LocalPlayer.Character.IsJacking;
            }
            WeaponDescriptor PlayerCurrentWeapon = Game.LocalPlayer.Character.Inventory.EquippedWeapon;
            CurrentWeapon = Mod.DataMart.Weapons.GetCurrentWeapon(Game.LocalPlayer.Character);

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

            if (NativeFunction.CallByName<bool>("GET_PED_CONFIG_FLAG", Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, 1) || NativeFunction.CallByName<int>("GET_TIMECYCLE_MODIFIER_INDEX") == 722)
            {
                IsDrunk = true;
            }
            else
            {
                IsDrunk = false;
            }
            AreStarsGreyedOut = SearchMode.IsInSearchMode;
        }
        private void UpdateState()
        {
            if (Game.LocalPlayer.Character.IsDead && !IsDead)
            {
                DeathEvent();
            }
            if (Mod.World.ShouldBustPlayer)
            {
                StartManualArrest();
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
                BustedEvent();
            }
            if (IsAliveAndFree && !Game.LocalPlayer.Character.IsDead)
            {
                MaxWantedLastLife = WantedLevel;
            }
        }
    }
}