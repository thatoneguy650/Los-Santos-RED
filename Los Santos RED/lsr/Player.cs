using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ExtensionsMethods;
using LosSantosRED.lsr.Locations;
using LSR.Vehicles;
using Rage;
using Rage.Native;

namespace LosSantosRED.lsr
{
    public class Player
    {
        private bool VanillaRespawn = true;
        private bool isGettingIntoVehicle;
        private bool isInVehicle;
        private bool isAimingInVehicle;
        private bool areStarsGreyedOut;
        private uint GameTimeLastShot;
        private uint GameTimeLastStartedJacking;
        private uint GameTimeLastStarsGreyedOut;
        private uint GameTimeLastStarsNotGreyedOut;
        private uint GameTimeLastDied;
        private uint GameTimeLastBusted;
        private uint GameTimePoliceNoticedVehicleChange;
        private uint GameTimeLastMoved;
        private uint PoliceLastSeenVehicleHandle;
        public Player()
        {

        }
        public VehicleEngine VehicleEngine { get; private set; } = new VehicleEngine();
        public SearchMode SearchMode { get; private set; } = new SearchMode();   
        public VehicleRadio VehicleRadio { get; private set; } = new VehicleRadio();
        public VehicleFuel VehicleFuel { get; private set; } = new VehicleFuel();
        public VehicleIndicators VehicleIndicators { get; private set; } = new VehicleIndicators();
        public Violations Violations { get; private set; } = new Violations();
        public Mugging Mugging { get; private set; } = new Mugging();
        public PedSwap PedSwap { get; private set; } = new PedSwap();
        public Respawning Respawning { get; private set; } = new Respawning();
        public Surrendering Surrendering { get; private set; } = new Surrendering();
        public WeaponDropping WeaponDropping { get; private set; } = new WeaponDropping();
        public Investigations Investigations { get; private set; } = new Investigations();
        public ArrestWarrant ArrestWarrant { get; private set; } = new ArrestWarrant();
        public LocationData CurrentLocation { get; private set; } = new LocationData(Game.LocalPlayer.Character);
        public PoliceResponse CurrentPoliceResponse { get; private set; } = new PoliceResponse();
        public List<VehicleExt> TrackedVehicles { get; private set; } = new List<VehicleExt>();
        public VehicleExt CurrentVehicle { get; private set; }
        public WeaponInformation CurrentWeapon { get; private set; }
        public WeaponCategory CurrentWeaponCategory
        {
            get
            {
                if (CurrentWeapon != null)
                    return CurrentWeapon.Category;
                return WeaponCategory.Unknown;
            }
        }
        public Vector3 CurrentPosition => Game.LocalPlayer.Character.Position;
        public List<LicensePlate> SpareLicensePlates { get; private set; } = new List<LicensePlate>();
        public WeaponHash LastWeaponHash { get; set; }
        public WeaponHash CurrentWeaponHash { get; set; }
        public int TimesDied { get; set; }
        public bool HandsAreUp { get; set; }
        public int MaxWantedLastLife { get; set; }
        public bool IsDead { get; private set; }
        public bool IsBusted { get; private set; }
        public bool BeingArrested { get; private set; }
        public bool DiedInVehicle { get; private set; }
        public bool IsConsideredArmed { get; private set; }
        public bool IsAliveAndFree
        {
            get
            {
                if(IsBusted || IsDead)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public bool IsCarJacking { get; set; }
        public bool IsLockPicking { get; set; }
        public bool IsChangingLicensePlates { get; set; }

        public bool IsInAutomobile { get; private set; }
        public bool IsOnMotorcycle { get; private set; }
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
        public bool IsAttemptingToSurrender
        {
            get
            {
                if (HandsAreUp && !Mod.Player.CurrentPoliceResponse.IsWeaponsFree)
                    return true;
                else
                    return false;
            }
        }
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
        public bool IsBreakingIntoCar
        {
            get
            {
                if (IsCarJacking || IsLockPicking)
                { 
                    return true; 
                }
                else
                {
                    return false;
                }
            }
        }
        public bool IsNotWanted => Game.LocalPlayer.WantedLevel == 0;
        public bool IsWanted => Game.LocalPlayer.WantedLevel > 0;
        public int WantedLevel => Game.LocalPlayer.WantedLevel;
        public bool IsStationary
        {
            get
            {
                if (GameTimeLastMoved == 0)
                    return true;
                return Game.GameTime - GameTimeLastMoved >= 1500;
            }
        }
        public bool StarsRecentlyGreyedOut
        {
            get
            {
                if (GameTimeLastStarsGreyedOut == 0)
                    return false;
                return Game.GameTime - GameTimeLastStarsGreyedOut <= 1500;
            }
        }
        public bool StarsRecentlyActive
        {
            get
            {
                if (GameTimeLastStarsNotGreyedOut == 0)
                    return false;
                return Game.GameTime - GameTimeLastStarsNotGreyedOut <= 1500;
            }
        }
        public bool RecentlyDied
        {
            get
            {
                if (GameTimeLastDied == 0)
                    return false;
                return Game.GameTime - GameTimeLastDied <= 5000;
            }
        }
        public bool RecentlyBusted
        {
            get
            {
                if (GameTimeLastBusted == 0)
                    return false;
                return Game.GameTime - GameTimeLastBusted <= 5000;
            }
        }
        public bool IsBustable
        {
            get
            {
                if (Mod.Player.CurrentPoliceResponse.HasBeenWantedFor <= 3000)
                    return true;
                if (Mod.Player.Surrendering.IsCommitingSuicide)
                    return true;
                if (RecentlyBusted)
                    return true;
                return true;
            }
        }
        public bool PoliceRecentlyNoticedVehicleChange
        {
            get
            {
                if (GameTimePoliceNoticedVehicleChange == 0)
                    return false;
                return Game.GameTime - GameTimePoliceNoticedVehicleChange <= 15000;
            }
        }
        public List<VehicleExt> ReportedStolenVehicles
        {
            get { return TrackedVehicles.Where(x => x.NeedsToBeReportedStolen).ToList(); }
        }

        private void TerminateVanillaRespawn()
        {
            var MyPtr = Game.GetScriptGlobalVariableAddress(4); //the script id for respawn_controller
            Marshal.WriteInt32(MyPtr, 1); //setting it to 1 turns it off somehow?
            Game.TerminateAllScriptsWithName("respawn_controller");
            VanillaRespawn = false;
        }
        private void ActivateVanillaRespawn()
        {
            var MyPtr = Game.GetScriptGlobalVariableAddress(4); //the script id for respawn_controller
            Marshal.WriteInt32(MyPtr, 0); //setting it to 0 turns it on somehow?
            Game.StartNewScript("respawn_controller");
            Game.StartNewScript("selector");
            VanillaRespawn = true;
        }
        public void Update()
        {
            if(VanillaRespawn)
            {
                TerminateVanillaRespawn();
            }
            CachePlayerData();
            StateTick();
            TurnOffRespawnScripts();
            AudioTick();
            TrackedVehiclesTick();
        }
        public void AddSpareLicensePlates()
        {
            SpareLicensePlates.Add(new LicensePlate(RandomItems.RandomString(8), 1, 1, false));
        }
        public void Dispose()
        {
            ActivateVanillaRespawn();
        }
        public bool RecentlyShot(int duration)
        {
            if (GameTimeLastShot == 0)
                return false;
            if (Mod.Player.PedSwap.RecentlyTakenOver)
                return false;
            if (Mod.Player.Respawning.RecentlyRespawned)
                return false;
            if (Game.GameTime - GameTimeLastShot <= duration) //15000
                return true;
            return false;
        }
        public void PlayerShotArtificially()
        {
            GameTimeLastShot = Game.GameTime;
        }
        public void ResetState(bool IncludeMaxWanted)
        {
            IsDead = false;
            IsBusted = false;
            Game.LocalPlayer.HasControl = true;
            BeingArrested = false;
            TimesDied = 0;
            LastWeaponHash = 0;
            if (IncludeMaxWanted)
                MaxWantedLastLife = 0; //this might be a problem in here and might need to be removed
        }
        public void StartManualArrest()
        {
            BeingArrested = true;
            if (!IsBusted)
                BustedEvent();
        }
        public void UpdateStolenStatus()
        {
            var MyVehicle = UpdateCurrentVehicle();
            if (MyVehicle == null || MyVehicle.IsStolen)
                return;

            if (Mod.Player.PedSwap.OwnedCar == null || MyVehicle.VehicleEnt.Handle != Mod.Player.PedSwap.OwnedCar.Handle)
                MyVehicle.IsStolen = true;
        }
        public void SetPlayerToLastWeapon()
        {
            if (Game.LocalPlayer.Character.Inventory.EquippedWeapon != null && LastWeaponHash != 0)
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)LastWeaponHash,
                    true);
                Mod.Debug.WriteToLog("SetPlayerToLastWeapon", LastWeaponHash.ToString());
            }
        }
        public void DisplayPlayerNotification()
        {
            var NotifcationText = "Warrants: ~g~None~s~";
            if (Mod.Player.CurrentPoliceResponse.CurrentCrimes.CommittedAnyCrimes)
                NotifcationText = "Wanted For:" + Mod.Player.CurrentPoliceResponse.CurrentCrimes.PrintCrimes();

            var MyCar = UpdateCurrentVehicle();
            if (MyCar != null && !MyCar.IsStolen)
            {
                var Make = MyCar.MakeName();
                var Model = MyCar.ModelName();
                var VehicleName = "";
                if (Make != "")
                    VehicleName = Make;
                if (Model != "")
                    VehicleName += " " + Model;

                NotifcationText += string.Format("~n~Vehicle: ~p~{0}~s~", VehicleName);
                NotifcationText += string.Format("~n~Plate: ~p~{0}~s~", MyCar.CarPlate.PlateNumber);
            }

            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~b~Personal Info",
                string.Format("~y~{0}", Mod.Player.PedSwap.SuspectName), NotifcationText);
        }
        private VehicleExt UpdateCurrentVehicle()
        {
            if (!Game.LocalPlayer.Character.IsInAnyVehicle(false)) return null;

            var CurrVehicle = Game.LocalPlayer.Character.CurrentVehicle;

            ///NativeFunction.CallByHash<bool>(0x4E20D2A627011E8E, CurrVehicle, 100f);//_SET_VEHICLE_DAMAGE_MODIFIER//doesnt do model damage just starts on fire



            var ToReturn = TrackedVehicles.Where(x => x.VehicleEnt.Handle == CurrVehicle.Handle).FirstOrDefault();
            if (ToReturn == null)
            {
                TrackCurrentVehicle();
                return TrackedVehicles.Where(x => x.VehicleEnt.Handle == CurrVehicle.Handle).FirstOrDefault();
            }

            ToReturn.SetAsEntered();
            return ToReturn;
        }
        private void CachePlayerData()
        {
            IsInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
            if (IsInVehicle)
            {
                if (Game.LocalPlayer.Character.IsInAirVehicle || Game.LocalPlayer.Character.IsInSeaVehicle ||
                    Game.LocalPlayer.Character.IsOnBike || Game.LocalPlayer.Character.IsInHelicopter)
                    IsInAutomobile = false;
                else
                    IsInAutomobile = true;

                if (Game.LocalPlayer.Character.IsOnBike)
                    IsOnMotorcycle = true;
                else
                    IsOnMotorcycle = false;

                CurrentVehicle = UpdateCurrentVehicle();


                if (Extensions.IsMoveControlPressed() || Game.LocalPlayer.Character.Speed >= 0.1f)
                    GameTimeLastMoved = Game.GameTime;
            }
            else
            {
                IsOnMotorcycle = false;
                IsInAutomobile = false;
                CurrentVehicle = null;
                if (Game.LocalPlayer.Character.Speed >= 0.2f)
                    GameTimeLastMoved = Game.GameTime;
            }
            if(CurrentLocation.CharacterToLocate.Handle != Game.LocalPlayer.Character.Handle)
            {
                CurrentLocation.CharacterToLocate = Game.LocalPlayer.Character;
            }

            if (Game.LocalPlayer.Character.IsShooting)
                GameTimeLastShot = Game.GameTime;

            IsGettingIntoAVehicle = Game.LocalPlayer.Character.IsGettingIntoVehicle;
            IsConsideredArmed = Game.LocalPlayer.Character.IsConsideredArmed();
            IsAimingInVehicle = IsInVehicle && Game.LocalPlayer.IsFreeAiming;
            var PlayerCurrentWeapon = Game.LocalPlayer.Character.Inventory.EquippedWeapon;
            CurrentWeapon = Mod.DataMart.Weapons.GetCurrentWeapon(Game.LocalPlayer.Character);

            if (PlayerCurrentWeapon != null)
                CurrentWeaponHash = PlayerCurrentWeapon.Hash;
            else
                CurrentWeaponHash = 0;

            if (CurrentWeaponHash != 0 && PlayerCurrentWeapon.Hash != LastWeaponHash)
                LastWeaponHash = PlayerCurrentWeapon.Hash;

            AreStarsGreyedOut = Mod.Player.SearchMode.IsInSearchMode;//NativeFunction.CallByName<bool>("ARE_PLAYER_STARS_GREYED_OUT", Game.LocalPlayer);
        }
        private void TurnOffRespawnScripts()
        {
            Game.DisableAutomaticRespawn = true;
            Game.FadeScreenOutOnDeath = false;
            Game.TerminateAllScriptsWithName("selector");
            NativeFunction.Natives.x1E0B4DC0D990A4E7(false);
            NativeFunction.Natives.x21FFB63D8C615361(true);
        }
        private void StateTick()
        {
            if (Game.LocalPlayer.Character.IsDead && !IsDead)
                DeathEvent();

            if (NativeFunction.CallByName<bool>("IS_PLAYER_BEING_ARRESTED", 0))
                BeingArrested = true;
            if (NativeFunction.CallByName<bool>("IS_PLAYER_BEING_ARRESTED", 1))
            {
                BeingArrested = true;
                Game.LocalPlayer.Character.Tasks.Clear();
            }

            if (BeingArrested && !IsBusted)
                BustedEvent();

            if (WantedLevel > MaxWantedLastLife) // The max wanted level i saw in the last life, not just right before being busted
                MaxWantedLastLife = WantedLevel;
        }
        private void AudioTick()
        {
            if (Mod.DataMart.Settings.MySettings.Police.DisableAmbientScanner)
                NativeFunction.Natives.xB9EFD5C25018725A("PoliceScannerDisabled", true);
            if (Mod.DataMart.Settings.MySettings.Police.WantedMusicDisable)
                NativeFunction.Natives.xB9EFD5C25018725A("WantedMusicDisabled", true);
        }
        private void TrackedVehiclesTick()
        {
            TrackedVehicles.RemoveAll(x => !x.VehicleEnt.Exists());
            if (IsInVehicle && Game.LocalPlayer.Character.IsInAnyVehicle(false)
            ) //first check is cheaper, but second is required to verify
            {
                if (CurrentVehicle == null)
                    return;

                if (Mod.World.PolicePerception.AnyCanSeePlayer && IsWanted && !AreStarsGreyedOut)
                {
                    if (PoliceLastSeenVehicleHandle != 0 &&
                        PoliceLastSeenVehicleHandle != CurrentVehicle.VehicleEnt.Handle &&
                        !CurrentVehicle.HasBeenDescribedByDispatch)
                    {
                        GameTimePoliceNoticedVehicleChange = Game.GameTime;
                        Mod.Debug.WriteToLog("PlayerState",
                            string.Format("PoliceRecentlyNoticedVehicleChange {0}", GameTimePoliceNoticedVehicleChange));
                    }

                    PoliceLastSeenVehicleHandle = CurrentVehicle.VehicleEnt.Handle;
                }

                if (Mod.World.PolicePerception.AnyCanRecognizePlayer)
                    if (WantedLevel > 0 && !AreStarsGreyedOut)
                        UpdateVehicleDescription(CurrentVehicle);
            }
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
            Mod.Player.Surrendering.SetArrestedAnimation(Game.LocalPlayer.Character, false, WantedLevel <= 2);
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
            Mod.World.Clock.PauseTime();
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
        private void IsGettingIntoVehicleChanged()
        {
            if (IsGettingIntoAVehicle)
            {
                Vehicle TargetVeh = Game.LocalPlayer.Character.VehicleTryingToEnter;
                if(TargetVeh == null)
                {
                    return;
                }
                int SeatTryingToEnter = Game.LocalPlayer.Character.SeatIndexTryingToEnter;
                if(!HasEntered(TargetVeh))//If you havent entered it
                {
                    TargetVeh.AttemptLockStatus((VehicleLockStatus)7); //Attempt to lock most car doors
                }
                if ((int)TargetVeh.LockStatus == 7) //Is Locked
                {
                    CarLockPick MyLockPick = new CarLockPick(TargetVeh, SeatTryingToEnter);
                    MyLockPick.PickLock();
                }
                else if (SeatTryingToEnter == -1 && TargetVeh.Driver != null && TargetVeh.Driver.IsAlive) //Driver
                {
                    CarJack MyJack = new CarJack(TargetVeh, TargetVeh.Driver, SeatTryingToEnter);
                    MyJack.StartCarJack();
                }
            }
            isGettingIntoVehicle = IsGettingIntoAVehicle;
        }
        private void IsInVehicleChanged()
        {
            if (IsInVehicle) UpdateStolenStatus();
            Mod.Debug.WriteToLog("ValueChecker", string.Format("IsInVehicle Changed to: {0}", IsInVehicle));
        }
        private void IsAimingInVehicleChanged()
        {
            if (IsAimingInVehicle)
                SetDriverWindow(true);
            else
                SetDriverWindow(false);
            Mod.Debug.WriteToLog("ValueChecker", string.Format("IsAimingInVehicle Changed to: {0}", IsAimingInVehicle));
        }
        private void AreStarsGreyedOutChanged()
        {
            if (AreStarsGreyedOut)
                GameTimeLastStarsGreyedOut = Game.GameTime;
            else
                GameTimeLastStarsNotGreyedOut = Game.GameTime;

            Mod.Debug.WriteToLog("ValueChecker", string.Format("AreStarsGreyedOut Changed to: {0}", AreStarsGreyedOut));
        }
        private void TrackCurrentVehicle()
        {
            var CurrVehicle = Game.LocalPlayer.Character.CurrentVehicle;
            var IsStolen = true;
            if (Mod.Player.PedSwap.OwnedCar != null && Mod.Player.PedSwap.OwnedCar.Handle == CurrVehicle.Handle)
                IsStolen = false;

            CurrVehicle.IsStolen = IsStolen;
            var AmStealingCarFromPrerson = Game.LocalPlayer.Character.IsJacking;
            Ped PreviousOwner;

            if (CurrVehicle.HasDriver && CurrVehicle.Driver.Handle != Game.LocalPlayer.Character.Handle)
                PreviousOwner = CurrVehicle.Driver;
            else
                PreviousOwner = CurrVehicle.GetPreviousPedOnSeat(-1);

            if (PreviousOwner != null && PreviousOwner.DistanceTo2D(Game.LocalPlayer.Character) <= 20f &&
                PreviousOwner.Handle != Game.LocalPlayer.Character.Handle) AmStealingCarFromPrerson = true;
            var MyPlate = new LicensePlate(CurrVehicle.LicensePlate, CurrVehicle.Handle,
                NativeFunction.CallByName<int>("GET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", CurrVehicle), false);
            var MyNewCar = new VehicleExt(CurrVehicle, Game.GameTime, AmStealingCarFromPrerson, CurrVehicle.IsAlarmSounding,IsStolen, MyPlate);


            //Maybe Add this back?
            //if (IsStolen && PreviousOwner.Exists())
            //{
            //    PedExt MyPrevOwner = Mod.PedManager.Civilians.FirstOrDefault(x => x.Pedestrian.Handle == PreviousOwner.Handle);
            //    if (MyPrevOwner != null) 
            //    { 
            //        MyPrevOwner.AddCrime(Mod.Violations.GrandTheftAuto, MyPrevOwner.Pedestrian.Position); 
            //    }
            //}

            TrackedVehicles.Add(MyNewCar);
        }
        private void SetDriverWindow(bool RollDown)
        {
            if (Game.LocalPlayer.Character.CurrentVehicle == null)
                return;

            var DriverWindowIntact =
                NativeFunction.CallByName<bool>("IS_VEHICLE_WINDOW_INTACT", Game.LocalPlayer.Character.CurrentVehicle, 0);
            var MyVehicle = CurrentVehicle;
            if (DriverWindowIntact)
            {
                if (RollDown)
                {
                    NativeFunction.CallByName<bool>("ROLL_DOWN_WINDOW", Game.LocalPlayer.Character.CurrentVehicle, 0);
                    MyVehicle.ManuallyRolledDriverWindowDown = true;
                }
                else
                {
                    MyVehicle.ManuallyRolledDriverWindowDown = false;
                    NativeFunction.CallByName<bool>("ROLL_UP_WINDOW", Game.LocalPlayer.Character.CurrentVehicle, 0);
                }
            }
            else
            {
                if (!RollDown)
                    if (MyVehicle != null && MyVehicle.ManuallyRolledDriverWindowDown)
                    {
                        MyVehicle.ManuallyRolledDriverWindowDown = false;
                        NativeFunction.CallByName<bool>("ROLL_UP_WINDOW", Game.LocalPlayer.Character.CurrentVehicle, 0);
                    }
            }
        }
        private void UpdateVehicleDescription(VehicleExt MyVehicle)
        {
            if (MyVehicle.VehicleEnt.Exists())
                MyVehicle.DescriptionColor = MyVehicle.VehicleEnt.PrimaryColor;
            if (MyVehicle.CarPlate != null)
                MyVehicle.CarPlate.IsWanted = true;
            if (MyVehicle.IsStolen && !MyVehicle.WasReportedStolen)
                MyVehicle.WasReportedStolen = true;
        }
        private bool HasEntered(Vehicle TargetVeh)
        {
            if (TrackedVehicles.Any(x => x.VehicleEnt.Handle == TargetVeh.Handle))//Entered By Player
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}