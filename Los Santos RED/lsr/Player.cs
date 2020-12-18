using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
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
        private uint GameTimeLastStarsGreyedOut;
        private uint GameTimeLastStarsNotGreyedOut;
        private uint GameTimeLastDied;
        private uint GameTimeLastBusted;
        private uint GameTimePoliceNoticedVehicleChange;
        private uint GameTimeLastMoved;
        private uint PoliceLastSeenVehicleHandle;
        private Mugging Mugging = new Mugging();
        public SearchMode SearchMode { get; private set; } = new SearchMode();
        public Violations Violations { get; private set; } = new Violations(); 
        public Respawning Respawning { get; private set; } = new Respawning();
        public Surrendering Surrendering { get; private set; } = new Surrendering();
        public WeaponDropping WeaponDropping { get; private set; } = new WeaponDropping();//make private
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
        public bool IsCarJacking { get; set; }
        public bool IsLockPicking { get; set; }
        public bool IsDrunk { get; private set; }
        public bool IsChangingLicensePlates { get; set; }
        public bool IsInAutomobile { get; private set; }
        public bool IsOnMotorcycle { get; private set; }
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
        public bool IsMugging
        {
            get => Mugging.IsMugging;
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
                {
                    return true;
                }
                else
                {
                    return Game.GameTime - GameTimeLastMoved >= 1500;
                }
            }
        }
        public bool StarsRecentlyGreyedOut
        {
            get
            {
                if (GameTimeLastStarsGreyedOut == 0)
                {
                    return false;
                }
                else
                {
                    return Game.GameTime - GameTimeLastStarsGreyedOut <= 1500;
                }
            }
        }
        public bool StarsRecentlyActive
        {
            get
            {
                if (GameTimeLastStarsNotGreyedOut == 0)
                {
                    return false;
                }
                else
                {
                    return Game.GameTime - GameTimeLastStarsNotGreyedOut <= 1500;
                }
            }
        }
        public bool RecentlyDied
        {
            get
            {
                if (GameTimeLastDied == 0)
                {
                    return false;
                }
                else
                {
                    return Game.GameTime - GameTimeLastDied <= 5000;
                }
            }
        }
        public bool RecentlyBusted
        {
            get
            {
                if (GameTimeLastBusted == 0)
                {
                    return false;
                }
                else
                {
                    return Game.GameTime - GameTimeLastBusted <= 5000;
                }
            }
        }
        public bool IsBustable
        {
            get
            {
                if (Mod.Player.CurrentPoliceResponse.HasBeenWantedFor <= 3000)
                {
                    return false;
                }
                else if (Mod.Player.Surrendering.IsCommitingSuicide)
                {
                    return false;
                }
                else if (RecentlyBusted)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public bool PoliceRecentlyNoticedVehicleChange
        {
            get
            {
                if (GameTimePoliceNoticedVehicleChange == 0)
                {
                    return false;
                }
                else
                {
                    return Game.GameTime - GameTimePoliceNoticedVehicleChange <= 15000;
                }
            }
        }
        public List<VehicleExt> ReportedStolenVehicles
        {
            get { return TrackedVehicles.Where(x => x.NeedsToBeReportedStolen).ToList(); }
        }
        public Player()
        {

        }
        public void Update()
        {
            if (VanillaRespawn)
            {
                TerminateVanillaRespawn();
            }
            CacheData();
            StateTick();
            TurnOffRespawnScripts();
            TrackedVehiclesTick();
        }
        public void MuggingTick()
        {
            Mugging.Tick();
        }
        public void AddSpareLicensePlates()
        {
            SpareLicensePlates.Add(new LicensePlate(RandomItems.RandomString(8), 3, false));//random cali
        }
        public void Dispose()
        {
            ActivateVanillaRespawn();
        }
        public bool RecentlyShot(int duration)
        {
            if (GameTimeLastShot == 0)
            {
                return false;
            }
            else if (Mod.World.PedSwap.RecentlyTakenOver)
            {
                return false;
            }
            else if (Mod.Player.Respawning.RecentlyRespawned)
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
        public void FlagShooting()
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
            {
                MaxWantedLastLife = 0; //this might be a problem in here and might need to be removed
            }
        }
        private void StartManualArrest()
        {
            BeingArrested = true;
            if (!IsBusted)
            {
                BustedEvent();
            }
        }
        public void SetPlayerToLastWeapon()
        {
            if (Game.LocalPlayer.Character.Inventory.EquippedWeapon != null && LastWeaponHash != 0)
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)LastWeaponHash,true);
                Mod.Debug.WriteToLog("SetPlayerToLastWeapon", LastWeaponHash.ToString());
            }
        }
        public void DisplayPlayerNotification()
        {
            string NotifcationText = "Warrants: ~g~None~s~";
            if (Mod.Player.CurrentPoliceResponse.CurrentCrimes.CommittedAnyCrimes)
            {
                NotifcationText = "Wanted For:" + Mod.Player.CurrentPoliceResponse.CurrentCrimes.PrintCrimes();
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

            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~b~Personal Info",string.Format("~y~{0}", Mod.World.PedSwap.SuspectName), NotifcationText);
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
        public void SetMoney(int Amount)
        {
            NativeFunction.CallByName<int>("STAT_SET_INT", Natives.CashHash(Mod.DataMart.Settings.SettingsManager.General.MainCharacterToAlias), Amount, 1);
        }
        private VehicleExt UpdateCurrentVehicle()
        {
            if (!Game.LocalPlayer.Character.IsInAnyVehicle(false))
            {
                return null;
            }
            Vehicle CurrVehicle = Game.LocalPlayer.Character.CurrentVehicle;
            VehicleExt ToReturn = TrackedVehicles.Where(x => x.Vehicle.Handle == CurrVehicle.Handle).FirstOrDefault();
            if (ToReturn == null)
            {
                VehicleExt MyNewCar = new VehicleExt(CurrVehicle,Game.GameTime);
                TrackedVehicles.Add(MyNewCar);
                return MyNewCar;
            }
            ToReturn.SetAsEntered();
            return ToReturn;
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
        private void CacheData()
        {
            IsInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
            if (IsInVehicle)
            {
                if (Game.LocalPlayer.Character.IsInAirVehicle || Game.LocalPlayer.Character.IsInSeaVehicle || Game.LocalPlayer.Character.IsOnBike || Game.LocalPlayer.Character.IsInHelicopter)
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
                if(CurrentVehicle != null)
                {

                    CurrentVehicle.Update();
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

            }
            if (CurrentLocation.CharacterToLocate.Handle != Game.LocalPlayer.Character.Handle)
            {
                CurrentLocation.CharacterToLocate = Game.LocalPlayer.Character;
            }

            if (Game.LocalPlayer.Character.IsShooting)
            {
                GameTimeLastShot = Game.GameTime;
            }

            IsGettingIntoAVehicle = Game.LocalPlayer.Character.IsGettingIntoVehicle;
            IsConsideredArmed = CheckIsArmed();
            IsAimingInVehicle = IsInVehicle && Game.LocalPlayer.IsFreeAiming;
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

            if(NativeFunction.CallByName<bool>("GET_PED_CONFIG_FLAG",Game.LocalPlayer.Character,(int)PedConfigFlags.PED_FLAG_DRUNK,1) || NativeFunction.CallByName<int>("GET_TIMECYCLE_MODIFIER_INDEX") == 722)
            {
                IsDrunk = true;
            }
            else
            {
                IsDrunk = false;
            }
            NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Game.LocalPlayer.Character, (int)PedConfigFlags._PED_FLAG_DISABLE_STARTING_VEH_ENGINE, true);
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
            {
                DeathEvent();
            }


            if (Mod.World.Pedestrians.ShouldBustPlayer)
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
            Mod.World.Time.PauseTime();
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
                Vehicle EnteringVehicle = Game.LocalPlayer.Character.VehicleTryingToEnter;
                int SeatTryingToEnter = Game.LocalPlayer.Character.SeatIndexTryingToEnter;
                if (EnteringVehicle == null)
                {
                    return;
                }
                VehicleExt MyCar = Mod.World.Vehicles.GetVehicle(EnteringVehicle);
                if(MyCar != null)
                {
                    //MyCar.
                    //Maybe Call MyCar.EnterEvent(Mod.Player.Character)
                    //that will check if you are the owner and just unlock the doors
                    //alos how to check if the car is owned by ped by the game


                    //also need in tasking when you are idle set them in, use the same function iwtht he ped passed in
                    //will need to have a list of owners or occupants? 
                    //maybe check relationship group of the ped driver and if they are friendly set it unlocked

                    //add auto door locks to the cars
                        //when you start going they auto lock and peds cannot carjack you as easily
          

                    MyCar.AttemptToLock();
                    if (Mod.Input.IsHoldingEnter && EnteringVehicle.Driver == null && EnteringVehicle.LockStatus == (VehicleLockStatus)7 && !EnteringVehicle.IsEngineOn)//no driver && Unlocked
                    {
                        //Mod.Debug.WriteToLog("IsGettingIntoVehicleChanged", string.Format("1 Handle: {0} LockPick", EnteringVehicle.Handle));
                        CarLockPick MyLockPick = new CarLockPick(EnteringVehicle, SeatTryingToEnter);
                        MyLockPick.PickLock();
                    }
                    else if (Mod.Input.IsHoldingEnter && SeatTryingToEnter == -1 && EnteringVehicle.Driver != null && EnteringVehicle.Driver.IsAlive) //Driver
                    {
                        //Mod.Debug.WriteToLog("IsGettingIntoVehicleChanged", string.Format("2 Handle: {0} CarJack", EnteringVehicle.Handle));
                        CarJack MyJack = new CarJack(EnteringVehicle, EnteringVehicle.Driver, SeatTryingToEnter);
                        MyJack.StartCarJack();
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
        private void TrackedVehiclesTick()
        {
            TrackedVehicles.RemoveAll(x => !x.Vehicle.Exists());
            if (IsInVehicle)
            {
                if (CurrentVehicle == null)
                {
                    return;
                }

                if (Mod.World.Police.AnyCanSeePlayer && IsWanted && !AreStarsGreyedOut)
                {
                    if (PoliceLastSeenVehicleHandle != 0 && PoliceLastSeenVehicleHandle != CurrentVehicle.Vehicle.Handle && !CurrentVehicle.HasBeenDescribedByDispatch)
                    {
                        GameTimePoliceNoticedVehicleChange = Game.GameTime;
                        Mod.Debug.WriteToLog("PlayerState", string.Format("PoliceRecentlyNoticedVehicleChange {0}", GameTimePoliceNoticedVehicleChange));
                    }
                    PoliceLastSeenVehicleHandle = CurrentVehicle.Vehicle.Handle;
                }

                if (Mod.World.Police.AnyCanRecognizePlayer && IsWanted && !AreStarsGreyedOut)
                {
                    CurrentVehicle.UpdateDescription();
                }
            }
        }
        private bool CheckIsArmed()
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
        public void SetUnarmed()
        {
            if (!(Game.LocalPlayer.Character.Inventory.EquippedWeapon == null))
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)2725352035, true); //Unequip weapon so you don't get shot
            }
        }
    }
}