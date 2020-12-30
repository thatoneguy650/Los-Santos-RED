using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Locations;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Mod
{
    public class Player : IPlayer
    {
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
        private uint GameTimeStartedSwerving;
        private bool IsSwerveRight;
        private uint GameTimeStartedDrinking;
        private uint GameTimeStoppedDrinking;
        private bool IsDrinking;
        private float DrunkStrength;

        private uint GameTimeStartedPlaying;
        private bool isAimingInVehicle;
        private bool isCarJacking;
        private bool isGettingIntoVehicle;
        private bool isInVehicle;
        private bool LeftEngineOn;
        private Mugging Mugging;
        private List<PedExt> PlayerKilledCivilians = new List<PedExt>();
        private List<PedExt> PlayerKilledCops = new List<PedExt>();
        private ISettings Settings;
        private IStreets Streets;
        private Surrendering Surrendering;
        private bool VanillaRespawn = true;
        private Violations Violations;
        private WeaponDropping WeaponDropping;
        private IWeapons Weapons;
        private IWorld World;
        private IZones Zones;

        public uint HasBeenDrinkingFor => GameTimeStartedDrinking == 0 ? 0 : Game.GameTime - GameTimeStartedDrinking;
        public uint HasBeenNotDrinkingFor => GameTimeStoppedDrinking == 0 ? 0 : Game.GameTime - GameTimeStoppedDrinking;
        public float DrunkIntensity
        {
            get
            {
                if (HasBeenDrinkingFor >= 60000)
                {
                    return 5.0f;
                }
                else if (HasBeenDrinkingFor >= 50000)
                {
                    return 4.0f;
                }
                else if (HasBeenDrinkingFor >= 40000)
                {
                    return 3.0f;
                }
                else if (HasBeenDrinkingFor >= 30000)
                {
                    return 2.0f;
                }
                else if(HasBeenDrinkingFor >= 15000)
                {
                    return 1.0f;
                }
                else
                {
                    return 0.0f;
                }
            }
        }
        public Player(IWorld world, IStreets streets, IZones zones, ISettings settings, IWeapons weapons)
        {
            World = world;
            Streets = streets;
            Zones = zones;
            Settings = settings;
            Weapons = weapons;
            CurrentHealth = new HealthState(new PedExt(Game.LocalPlayer.Character));
            Mugging = new Mugging(this, world);
            Violations = new Violations(this, world);
            Surrendering = new Surrendering(this);
            WeaponDropping = new WeaponDropping(this, Weapons);
            Investigations = new Investigations(this, world);
            ArrestWarrant = new ArrestWarrant(this);
            CurrentLocation = new LocationData(Game.LocalPlayer.Character, Streets, Zones);
            CurrentPoliceResponse = new PoliceResponse(this, world, ArrestWarrant);
            CurrentPoliceResponse.SetWantedLevel(0, "Initial", true);
            NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Game.LocalPlayer.Character, (int)PedConfigFlags._PED_FLAG_DISABLE_STARTING_VEH_ENGINE, true);
            GameTimeStartedPlaying = Game.GameTime;
        }
        public bool AnyHumansNear
        {
            get
            {
                return World.PoliceList.Any(x => x.DistanceToPlayer <= 10f) || World.CivilianList.Any(x => x.DistanceToPlayer <= 10f);
            }
        }
        public bool AnyPoliceCanHearPlayer { get; set; }
        public bool AnyPoliceCanRecognizePlayer { get; set; }
        public bool AnyPoliceCanSeePlayer { get; set; }
        public bool AnyPoliceRecentlySeenPlayer { get; set; }
        public bool AnyPoliceSeenPlayerCurrentWanted { get; set; }
        public bool AreStarsGreyedOut { get; set; }
        public ArrestWarrant ArrestWarrant { get; private set; }
        public bool BeingArrested { get; private set; }
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
        public bool IsHoldingEnter { get; set; }
        public bool IsHotWiring { get; private set; }
        public bool IsInAirVehicle { get; private set; }
        public bool IsInAutomobile { get; private set; }
        public bool IsIncapacitated => IsStunned || IsRagdoll;
        public bool IsInSearchMode { get; set; }
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
        public bool IsMobileRadioEnabled { get; private set; }
        public bool IsMoveControlPressed { get; set; }
        public bool IsMugging => Mugging.IsMugging;
        public bool IsNotWanted => Game.LocalPlayer.WantedLevel == 0;
        public bool IsOffroad => CurrentLocation.IsOffroad;
        public bool IsOnMotorcycle { get; private set; }
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
        public int MaxWantedLastLife { get; set; }
        public int Money
        {
            get
            {
                int CurrentCash;
                unsafe
                {
                    NativeFunction.CallByName<int>("STAT_GET_INT", Natives.CashHash(Settings.SettingsManager.General.MainCharacterToAlias), &CurrentCash, -1);
                }
                return CurrentCash;
            }
        }
        public bool NearCivilianMurderVictim => PlayerKilledCivilians.Any(x => x.Pedestrian.Exists() && x.Pedestrian.DistanceTo2D(Game.LocalPlayer.Character) <= 9f);
        public bool NearCopMurderVictim => PlayerKilledCops.Any(x => x.Pedestrian.Exists() && x.Pedestrian.DistanceTo2D(Game.LocalPlayer.Character) <= 15f);
        public Vector3 PlacePoliceLastSeenPlayer { get; set; }
        public bool RecentlyBusted => GameTimeLastBusted != 0 && Game.GameTime - GameTimeLastBusted <= 5000;
        public bool RecentlyDied => GameTimeLastDied != 0 && Game.GameTime - GameTimeLastDied <= 5000;
        public bool RecentlyHurtCivilian => GameTimeLastHurtCivilian != 0 && Game.GameTime - GameTimeLastHurtCivilian <= 5000;
        public bool RecentlyHurtCop => GameTimeLastHurtCop != 0 && Game.GameTime - GameTimeLastHurtCop <= 5000;
        public bool RecentlyKilledCivilian => GameTimeLastKilledCivilian != 0 && Game.GameTime - GameTimeLastKilledCivilian <= 5000;
        public bool RecentlyKilledCop => GameTimeLastKilledCop != 0 && Game.GameTime - GameTimeLastKilledCop <= 5000;
        public bool RecentlyStartedPlaying => Game.GameTime - GameTimeStartedPlaying <= 15000;//5000;
        public List<VehicleExt> ReportedStolenVehicles => TrackedVehicles.Where(x => x.NeedsToBeReportedStolen).ToList();
        public List<LicensePlate> SpareLicensePlates { get; private set; } = new List<LicensePlate>();
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
        public void AddSpareLicensePlate()
        {
            SpareLicensePlates.Add(new LicensePlate(RandomItems.RandomString(8), 3, false));//random cali
        }
        public void ArrestWarrantUpdate()
        {
            ArrestWarrant.Update();
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
            uint PlayerCashHash = Natives.CashHash(Settings.SettingsManager.General.MainCharacterToAlias);
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
        public void GiveName(string modelBeforeSpoof, string defaultName)
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
                SuspectsName = defaultName;
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
            Game.Console.Print(string.Format("PedWoundSystem! Player Hurt {0}, IsCop: {1}", MyPed.Pedestrian.Handle, MyPed.IsCop));
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
            Game.Console.Print(string.Format("PedWoundSystem! Player Killed {0}, IsCop: {1}", MyPed.Pedestrian.Handle, MyPed.IsCop));
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
            else if (Game.GameTime - GameTimeLastShot <= duration) //15000
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Reset(bool resetWanted, bool resetTimesDied, bool clearWeapons)
        {
            IsDead = false;
            IsBusted = false;
            Game.LocalPlayer.HasControl = true;
            BeingArrested = false;
            CurrentHealth = new HealthState(new PedExt(Game.LocalPlayer.Character));
            if (resetWanted)
            {
                CurrentPoliceResponse.Reset();
                Investigations.Reset();
                ResetInjuries();
                SetSober(false);
                NativeFunction.CallByName<bool>("RESET_PLAYER_ARREST_STATE", Game.LocalPlayer);
                MaxWantedLastLife = 0;
                GameTimeStartedPlaying = Game.GameTime;
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
        }
        public void ResetInjuries()
        {
            GameTimeLastHurtCivilian = 0;
            GameTimeLastKilledCivilian = 0;
            GameTimeLastHurtCop = 0;
            GameTimeLastKilledCop = 0;
        }
        public void SetCarJacking(bool valueToSet)
        {
            isCarJacking = valueToSet;
        }
        public void SetMoney(int Amount)
        {
            NativeFunction.CallByName<int>("STAT_SET_INT", Natives.CashHash(Settings.SettingsManager.General.MainCharacterToAlias), Amount, 1);
        }
        public void SetPlayerToLastWeapon()
        {
            if (Game.LocalPlayer.Character.Inventory.EquippedWeapon != null && LastWeaponHash != 0)
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)LastWeaponHash, true);
                Game.Console.Print("SetPlayerToLastWeapon" + LastWeaponHash.ToString());
            }
        }
        public void SetShot()
        {
            GameTimeLastShot = Game.GameTime;
        }
        public void SetSmashedWindow()
        {
            GameTimeLastSmashedVehicleWindow = Game.GameTime;
        }
        public void SetUnarmed()
        {
            if (!(Game.LocalPlayer.Character.Inventory.EquippedWeapon == null))
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)2725352035, true); //Unequip weapon so you don't get shot
            }
        }
        public void StartManualArrest()
        {
            BeingArrested = true;
            if (!IsBusted)
            {
                BustedEvent();
            }
        }
        public void TrafficViolationsUpdate()
        {
            Violations.TrafficUpdate();
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
            TerminateVanillaAudio();

            UpdateData();
            UpdateState();
            TrackedVehiclesTick();
            CurrentHealth.Update(null);
            WeaponDropping.Tick();
            DrunkUpdate();
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
        private void BustedEvent()
        {
            DiedInVehicle = IsInVehicle;
            IsBusted = true;
            BeingArrested = true;
            GameTimeLastBusted = Game.GameTime;
            HandsAreUp = false;
            Surrendering.SetArrestedAnimation(Game.LocalPlayer.Character, false, WantedLevel <= 2);
            Game.LocalPlayer.HasControl = false;
        }
        private void DeathEvent()
        {
            World.PauseTime();
            DiedInVehicle = IsInVehicle;
            IsDead = true;
            GameTimeLastDied = Game.GameTime;
            Game.LocalPlayer.Character.Kill();
            Game.LocalPlayer.Character.Health = 0;
            Game.LocalPlayer.Character.IsInvincible = true;
            TransitionToSlowMo();
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
            Game.Console.Print(string.Format("IsAimingInVehicle Changed to: {0}", IsAimingInVehicle));
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
                VehicleExt MyCar = World.GetVehicle(VehicleTryingToEnter);
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
                    Game.Console.Print($"Vehicle {MyCar.Vehicle.Handle} Lock Status: {MyCar.Vehicle.LockStatus}");
                    if (IsHoldingEnter && VehicleTryingToEnter.Driver == null && VehicleTryingToEnter.LockStatus == (VehicleLockStatus)7 && !VehicleTryingToEnter.IsEngineOn)//no driver && Unlocked
                    {
                        Game.Console.Print($"Vehicle {MyCar.Vehicle.Handle} Start LockPick");
                        CarLockPick MyLockPick = new CarLockPick(World, this, VehicleTryingToEnter, SeatTryingToEnter);
                        MyLockPick.PickLock();
                    }
                    else if (IsHoldingEnter && SeatTryingToEnter == -1 && VehicleTryingToEnter.Driver != null && VehicleTryingToEnter.Driver.IsAlive) //Driver
                    {
                        Game.Console.Print($"Vehicle {MyCar.Vehicle.Handle} Start CarJack");
                        CarJack MyJack = new CarJack(World, this, VehicleTryingToEnter, VehicleTryingToEnter.Driver, SeatTryingToEnter, CurrentWeapon);
                        MyJack.StartCarJack();
                    }
                    else if (VehicleTryingToEnter.LockStatus == (VehicleLockStatus)7)
                    {
                        Game.Console.Print($"Vehicle {MyCar.Vehicle.Handle} Start Regular Smash");
                        CarBreakIn MyBreakIn = new CarBreakIn(World, this, VehicleTryingToEnter, SeatTryingToEnter);
                        MyBreakIn.BreakIn();
                    }
                }
            }
            else
            {
            }
            isGettingIntoVehicle = IsGettingIntoAVehicle;
            Game.Console.Print(string.Format("IsGettingIntoVehicleChanged to {0}", IsGettingIntoAVehicle));
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
                    CurrentVehicle.Vehicle.IsEngineOn = LeftEngineOn;
                    //Game.Console.Print(string.Format("IsInVehicle Changed: Current Engine Status: {0}", CurrentVehicle.Vehicle.IsEngineOn));
                }
                else
                {
                    // Game.Console.Print("IsInVehicle Changed: No Current Vehicle Already");
                }
            }
            Game.Console.Print(string.Format("IsInVehicle Changed: {0}", IsInVehicle));
        }
        private void TerminateVanillaAudio()
        {
            if (Settings.SettingsManager.Police.DisableAmbientScanner)
            {
                NativeFunction.Natives.xB9EFD5C25018725A("PoliceScannerDisabled", true);
            }
            if (Settings.SettingsManager.Police.WantedMusicDisable)
            {
                NativeFunction.Natives.xB9EFD5C25018725A("WantedMusicDisabled", true);
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
            bool IsGettingIntoVehicle = Game.LocalPlayer.Character.IsGettingIntoVehicle;
            bool IsInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
            if (!IsInVehicle && !IsGettingIntoVehicle)
            {
                return null;
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
                return null;
            }
            VehicleExt ToReturn = TrackedVehicles.Where(x => x.Vehicle.Handle == CurrVehicle.Handle).FirstOrDefault();
            if (ToReturn == null)
            {
                VehicleExt MyNewCar = new VehicleExt(CurrVehicle, Game.GameTime);
                TrackedVehicles.Add(MyNewCar);
                return MyNewCar;
            }
            if (IsInVehicle)
            {
                ToReturn.SetAsEntered();
            }
            ToReturn.Update();
            LeftEngineOn = ToReturn.Vehicle.IsEngineOn;
            return ToReturn;
        }
        private void UpdateData()
        {
            UpdateVehicleData();
            UpdateWeaponData();
            UpdateMiscData();
        }
        private void UpdateMiscData()
        {
            if (CurrentLocation.CharacterToLocate.Handle != Game.LocalPlayer.Character.Handle)
            {
                CurrentLocation.CharacterToLocate = Game.LocalPlayer.Character;
            }
            if (CurrentHealth.MyPed.Pedestrian.Handle != Game.LocalPlayer.Character.Handle)
            {
                CurrentHealth.MyPed = new PedExt(Game.LocalPlayer.Character);
            }
            IsStunned = Game.LocalPlayer.Character.IsStunned;
            IsRagdoll = Game.LocalPlayer.Character.IsRagdoll;


            if (NativeFunction.CallByName<bool>("GET_PED_CONFIG_FLAG", Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, 1) || NativeFunction.CallByName<int>("GET_TIMECYCLE_MODIFIER_INDEX") == 722)
            {
                IsDrunk = true;
            }
            else
            {
                IsDrunk = false;
            }
        }
        private void UpdateState()
        {
            if (Game.LocalPlayer.Character.IsDead && !IsDead)
            {
                DeathEvent();
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
        private void UpdateVehicleData()
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
                if (EntryPoint.IsMoveControlPressed || Game.LocalPlayer.Character.Speed >= 0.1f)
                {
                    GameTimeLastMoved = Game.GameTime;
                }

                if (CurrentVehicle != null && CurrentVehicle.Vehicle.IsEngineOn && CurrentVehicle.Vehicle.IsPoliceVehicle)
                {
                    if (!IsMobileRadioEnabled)
                    {
                        IsMobileRadioEnabled = true;
                        NativeFunction.CallByName<bool>("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", true);
                        Game.Console.Print("Audio! Mobile Radio Enabled");
                    }
                }
                else
                {
                    if (IsMobileRadioEnabled)
                    {
                        IsMobileRadioEnabled = false;
                        NativeFunction.CallByName<bool>("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", false);
                        Game.Console.Print("Audio! Mobile Radio Disabled");
                    }
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
            }

            if (isCarJacking)//is manually set
            {
                IsCarJacking = isCarJacking;
            }
            else
            {
                IsCarJacking = Game.LocalPlayer.Character.IsJacking;
            }
        }
        private void UpdateWeaponData()
        {
            if (Game.LocalPlayer.Character.IsShooting)
            {
                GameTimeLastShot = Game.GameTime;
            }
            IsAimingInVehicle = IsInVehicle && Game.LocalPlayer.IsFreeAiming;
            IsConsideredArmed = CheckIsArmed();
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
        }
        public void StartDrinking()
        {
            if(!IsDrinking)
            {
                IsDrinking = true;
                NativeFunction.CallByName<bool>("TASK_START_SCENARIO_IN_PLACE", Game.LocalPlayer.Character, "WORLD_HUMAN_DRINKING", 0, true);
                GameTimeStartedDrinking = Game.GameTime;
                GameTimeStoppedDrinking = 0;
                GameFiber ScenarioWatcher = GameFiber.StartNew(delegate
                {
                    while (!EntryPoint.IsMoveControlPressed)
                    {
                        GameFiber.Yield();
                    }
                    Game.LocalPlayer.Character.Tasks.Clear();
                    IsDrinking = false;
                    GameTimeStartedDrinking = 0;
                    GameTimeStoppedDrinking = Game.GameTime;
                }, "ScenarioWatcher");
            }            
        }
        private void DrunkUpdate()
        {
            if(IsDrinking && HasBeenDrinkingFor >= 10000 && DrunkStrength != DrunkIntensity)
            {
                DrunkStrength = DrunkIntensity;
                IsDrunk = true;
                GameTimeStoppedDrinking = 0;
                SetDrunk(DrunkIntensity);
            }
            else if(!IsDrinking && IsDrunk && HasBeenNotDrinkingFor >= 120000)
            {
                IsDrunk = false;
                DrunkStrength = 0f;
                GameTimeStartedDrinking = 0;
                SetSober(true);
            }
        }
        private void SetDrunk(float Strength)
        {
            string MovementClipset = "move_m@drunk@slightlydrunk";
            if (Strength >= 3)
            {
                MovementClipset = "move_m@drunk@moderatedrunk";
            }
            else if(Strength >= 5)
            {
                MovementClipset = "move_m@drunk@verydrunk";
            }
            NativeFunction.CallByName<bool>("SET_PED_IS_DRUNK", Game.LocalPlayer.Character, true);
            if (!NativeFunction.CallByName<bool>("HAS_ANIM_SET_LOADED", MovementClipset))
            {
                NativeFunction.CallByName<bool>("REQUEST_ANIM_SET", MovementClipset);
            }
            NativeFunction.CallByName<bool>("SET_PED_MOVEMENT_CLIPSET", Game.LocalPlayer.Character, MovementClipset, 0x3E800000);
            NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, true);
            NativeFunction.CallByName<int>("SET_TIMECYCLE_MODIFIER", "Drunk");
            NativeFunction.CallByName<int>("SET_TIMECYCLE_MODIFIER_STRENGTH", 1.0f);
            NativeFunction.Natives.x80C8B1846639BB19(1);
            NativeFunction.CallByName<int>("SHAKE_GAMEPLAY_CAM", "DRUNK_SHAKE", Strength);
            Game.Console.Print($"Player Made Drunk. Strength: {Strength}");
        }
        private void SetSober(bool ResetClipset)
        {
            NativeFunction.CallByName<bool>("SET_PED_IS_DRUNK", Game.LocalPlayer.Character, false);
            if(ResetClipset)
            {
                NativeFunction.CallByName<bool>("RESET_PED_MOVEMENT_CLIPSET", Game.LocalPlayer.Character);
            }
            
            NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, false);
            NativeFunction.CallByName<int>("CLEAR_TIMECYCLE_MODIFIER");
            NativeFunction.Natives.x80C8B1846639BB19(0);
            NativeFunction.CallByName<int>("STOP_GAMEPLAY_CAM_SHAKING", true);
            Game.Console.Print("Player Made Sober");
        }
    }
}