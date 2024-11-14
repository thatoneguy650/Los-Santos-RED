using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using NAudio.Wave;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
namespace LosSantosRED.lsr
{
    public class PoliceResponse
    {
        public List<CrimeEvent> CrimesObserved = new List<CrimeEvent>();
        public List<CrimeEvent> CrimesReported = new List<CrimeEvent>();
        public uint GameTimeWantedEnded;
        public List<LicensePlate> WantedPlates = new List<LicensePlate>();
        private PoliceState CurrentPoliceState;
        private uint GameTimeLastRequestedBackup;
        private uint GameTimeLastWantedEnded;
        private uint GameTimePoliceStateStart;
        private uint GameTimeWantedLevelStarted;
        private IPoliceRespondable Player;
        private PoliceState PrevPoliceState;
        private Blip LastSeenLocationBlip;
        private ISettingsProvideable Settings;
        private ITimeReportable Time;
        private IEntityProvideable World;
        private HashSet<Crime> GracePeriodCrimes = new HashSet<Crime>();
        private Vector3 CurrentWantedCenter;
        private float CurrentPlayerDistance;

        public TrainStopper TrainStopper { get; private set; }
        private enum PoliceState
        {
            Normal = 0,
            UnarmedChase = 1,
            CautiousChase = 2,
            DeadlyChase = 3,
            ArrestedWait = 4,
        }
        public string DebugText => $"Have Desc {PoliceHaveDescription} CurrentPoliceState {CurrentPoliceState} IsWeaponsFree {IsWeaponsFree}";
        public uint GameTimeWantedStarted { get; private set; }
        public uint HasBeenAtCurrentPoliceStateFor => Player.WantedLevel == 0 ? 0 : Game.GameTime - GameTimePoliceStateStart;
        public uint HasBeenAtCurrentWantedLevelFor => Player.WantedLevel == 0 ? 0 : Game.GameTime - GameTimeWantedLevelStarted;
        public uint HasBeenNotWantedFor => Player.WantedLevel != 0 || GameTimeLastWantedEnded == 0 ? 0 : Game.GameTime - GameTimeLastWantedEnded;
        public uint HasBeenWantedFor => Player.WantedLevel == 0 ? 0 : Game.GameTime - GameTimeWantedStarted;
        public bool WantedLevelHasBeenRadioedIn { get; private set; } = false;
        public bool HasObservedCrimes => CrimesObserved.Any();
        public bool IsDeadlyChase => CurrentPoliceState == PoliceState.DeadlyChase;
        public int CountCloseVehicleChasingCops { get; private set; }
        public bool IsWeaponsFree { get; set; }
        public DateTime DateTimeLastWantedEnded { get; private set; }
        public Vector3 LastWantedCenterPosition { get; set; }
        public bool LethalForceAuthorized => CrimesObserved.Any(x => x.AssociatedCrime.ResultsInLethalForce);
        public string ObservedCrimesDisplay => string.Join(",", CrimesObserved.Select(x => x.AssociatedCrime.Name));
        public int ObservedMaxWantedLevel => CrimesObserved == null ? -1 : CrimesObserved.Max(x => x.AssociatedCrime.ResultingWantedLevel);
        public Vector3 PlaceLastReportedCrime { get; private set; }
        public Interior PlaceLastReportedCrimeInterior { get; private set; }
        public Vector3 PlaceWantedStarted { get; private set; }
        public bool PlayerSeenDuringCurrentWanted { get; set; }
        public bool PlayerSeenDuringWanted { get; set; } = false;
        public bool PoliceHaveDescription { get; private set; }
        public int CurrentRespondingPoliceCount { get; private set; }





        public bool HasShotAtPolice => InstancesOfCrime("KillingPolice") > 0 || InstancesOfCrime("FiringWeaponNearPolice") > 0;
        public bool HasHurtPolice => InstancesOfCrime("KillingPolice") > 0 || InstancesOfCrime("HurtingPolice") > 0;
        public int PoliceKilled => InstancesOfCrime("KillingPolice");
        public int PoliceHurt => InstancesOfCrime("HurtingPolice");
        public int CiviliansKilled => InstancesOfCrime("KillingCivilians");
        public string ReportedCrimesDisplay => string.Join(",", CrimesReported.Select(x => x.AssociatedCrime.Name));
        public float ResponseDrivingSpeed => CurrentResponse == ResponsePriority.Full ? 30f : CurrentResponse == ResponsePriority.High || CurrentResponse == ResponsePriority.Medium ? 25f : 20f;
        private ResponsePriority CurrentResponse
        {
            get
            {
                if (Player.IsNotWanted)
                {
                    if (Player.Investigation.IsActive)
                    {
                        if (CrimesReported.Any(x => x.AssociatedCrime.Priority <= Settings.SettingsManager.PoliceSettings.MediumResponseInvestigationActiveCrimePriorityRequirement))// 8))
                        {
                            return ResponsePriority.Medium;
                        }
                        else
                        {
                            return ResponsePriority.Low;
                        }
                    }
                    else
                    {
                        return ResponsePriority.None;
                    }
                }
                else
                {
                    if (Player.WantedLevel > Settings.SettingsManager.PoliceSettings.FullResponseWantedLevelRequirement)//4)
                    {
                        return ResponsePriority.Full;
                    }
                    else if (Player.WantedLevel >= Settings.SettingsManager.PoliceSettings.HighResponseWantedLevelRequirement)//  2)
                    {
                        return ResponsePriority.High;
                    }
                    else
                    {
                        return ResponsePriority.Medium;
                    }
                }
            }
        }
        public bool PlayerSeenInVehicleDuringWanted { get; set; }
        private uint CurrentWantedLevelIncreaseTime
        {
            get
            {
                if (Player.WantedLevel == 1)
                {
                    return Settings.SettingsManager.PoliceSettings.WantedLevelIncreaseTime_FromWanted1;
                }
                else if (Player.WantedLevel == 2)
                {
                    return Settings.SettingsManager.PoliceSettings.WantedLevelIncreaseTime_FromWanted2;
                }
                else if (Player.WantedLevel == 3)
                {
                    return Settings.SettingsManager.PoliceSettings.WantedLevelIncreaseTime_FromWanted3;
                }
                else if (Player.WantedLevel == 4)
                {
                    return Settings.SettingsManager.PoliceSettings.WantedLevelIncreaseTime_FromWanted4;
                }
                else if (Player.WantedLevel == 5)
                {
                    return Settings.SettingsManager.PoliceSettings.WantedLevelIncreaseTime_FromWanted5;
                }


                else if (Player.WantedLevel == 6)
                {
                    return Settings.SettingsManager.PoliceSettings.WantedLevelIncreaseTime_FromWanted6;
                }
                else if (Player.WantedLevel == 7)
                {
                    return Settings.SettingsManager.PoliceSettings.WantedLevelIncreaseTime_FromWanted7;
                }
                else if (Player.WantedLevel == 8)
                {
                    return Settings.SettingsManager.PoliceSettings.WantedLevelIncreaseTime_FromWanted8;
                }
                else if (Player.WantedLevel == 9)
                {
                    return Settings.SettingsManager.PoliceSettings.WantedLevelIncreaseTime_FromWanted9;
                }


                else
                {
                    return Settings.SettingsManager.PoliceSettings.WantedLevelIncreaseTime_FromWanted5;
                }
            }
        }

        public bool IsWithinPoliceRadius { get; private set; }
        public float CurrentPoliceRadius { get; private set; }
        public PoliceResponse(IPoliceRespondable player, ISettingsProvideable settings, ITimeReportable time, IEntityProvideable world)
        {
            Player = player;
            Settings = settings;
            Time = time;
            World = world;
            TrainStopper = new TrainStopper(Player, Settings, Time, World);
        }
        public void Update()
        {
            CurrentPoliceState = GetPoliceState();
            if (PrevPoliceState != CurrentPoliceState)
            {
                PoliceStateChanged();
            }
            AssignCops();
            if (Player.IsWanted)
            {
                UpdateWanted();
            }
            else
            {
                UpdateNotWanted();
            }
            GameFiber.Yield();//TR 05
            UpdateBlip();
            UpdateGracePeriod();
            TrainStopper.Update();
        }

        private void UpdateNotWanted()
        {
            CurrentPoliceRadius = 200f;
            IsWithinPoliceRadius = false;
        }

        private void UpdateWanted()
        {
            if (Player.IsBusted || Player.IsDead)
            {
                return;
            }
            CurrentWantedCenter = Player.PlacePoliceLastSeenPlayer;
            if (CurrentWantedCenter != Vector3.Zero)
            {
                LastWantedCenterPosition = CurrentWantedCenter;
            }
            if (Player.AnyPoliceCanSeePlayer)
            {
                PlayerSeenDuringCurrentWanted = true;
                PlayerSeenDuringWanted = true;
                if (Player.IsInVehicle)
                {
                    PlayerSeenInVehicleDuringWanted = true;
                }
            }
            else
            {
                if (!WantedLevelHasBeenRadioedIn && !Player.AnyPoliceSawPlayerViolating)
                {
                    ResetWanted();
                    return;
                }
            }
            if (Settings.SettingsManager.PoliceSettings.WantedLevelIncreasesOverTime && HasBeenAtCurrentWantedLevelFor > CurrentWantedLevelIncreaseTime && Player.AnyPoliceCanSeePlayer && Player.WantedLevel <= Settings.SettingsManager.PoliceSettings.MaxWantedLevel - 1)// 5)
            {
                GameTimeLastRequestedBackup = Game.GameTime;
                Player.SetWantedLevel(Player.WantedLevel + 1, "WantedLevelIncreasesOverTime", true);
                Player.OnRequestedBackUp();
            }
            if (CurrentPoliceState == PoliceState.DeadlyChase && Player.WantedLevel < Settings.SettingsManager.PoliceSettings.DeadlyChaseMinimumWantedLevel)
            {
                Player.SetWantedLevel(Settings.SettingsManager.PoliceSettings.DeadlyChaseMinimumWantedLevel, $"Deadly chase requires {Settings.SettingsManager.PoliceSettings.DeadlyChaseMinimumWantedLevel}+ wanted level", true);
            }
            PoliceKilledUpdate();
            UpdateWantedSize();
        }
        private void UpdateWantedSize()
        {      
            if(!WantedLevelHasBeenRadioedIn)
            {
                CurrentPoliceRadius = Settings.SettingsManager.PoliceSettings.MinimumPoliceResponseRadius;// 20f;
            }
            else
            {
                CurrentPoliceRadius = (Settings.SettingsManager.PoliceSettings.PoliceResponseRadiusIncrement * Player.WantedLevel); //25f + ((1 - Player.SearchModePercentage) * 175f);
            }
            if (CurrentWantedCenter == Vector3.Zero)
            {
                CurrentPlayerDistance = 999f;
            }
            else
            {
                CurrentPlayerDistance = CurrentWantedCenter.DistanceTo2D(Player.Character.Position);
            }
            IsWithinPoliceRadius = CurrentPlayerDistance <= CurrentPoliceRadius;
        }

        public void Dispose()
        {
            if (LastSeenLocationBlip.Exists())
            {
                LastSeenLocationBlip.Delete();
            }
            TrainStopper.Dispose();
        }
        public CrimeSceneDescription AddCrime(Crime CrimeInstance, CrimeSceneDescription crimeSceneDescription, bool isForPlayer)
        {
            //this is a fucking mess of references and isnt working properly at all
            //instances still dont work, need to rethink this entire approach, maybe store the latest info separate from the crime?
            //dont really care that it was assocaited with THIS crime, just if im going to report the crime and what the info IS
            //EntryPoint.WriteToConsole($"PLAYER EVENT: ADD CRIME: {CrimeInstance.Name} ByPolice: {crimeSceneDescription.SeenByOfficers} PlaceLastReportedCrime {PlaceLastReportedCrime} New PlaceLastReportedCrime {crimeSceneDescription.PlaceSeen} HaveDescription {crimeSceneDescription.HaveDescription}", 3);
            PlaceLastReportedCrime = crimeSceneDescription.PlaceSeen;
            PlaceLastReportedCrimeInterior = crimeSceneDescription.InteriorSeen;
            if (Player.IsAlive)//Player.IsAliveAndFree)// && !CurrentPlayer.RecentlyBribedPolice)
            {
                if (crimeSceneDescription.HaveDescription && isForPlayer)
                {
                    PoliceHaveDescription = crimeSceneDescription.HaveDescription;
                }
                //PlaceLastReportedCrime = crimeSceneDescription.PlaceSeen;
                CrimeEvent PreviousViolation;
                if (crimeSceneDescription.SeenByOfficers)
                {
                    PreviousViolation = CrimesObserved.FirstOrDefault(x => x.AssociatedCrime.ID == CrimeInstance.ID);
                }
                else
                {
                    PreviousViolation = CrimesReported.FirstOrDefault(x => x.AssociatedCrime.ID == CrimeInstance.ID);
                }
                if (PreviousViolation != null)
                {
                    PreviousViolation.AddInstance();
                    crimeSceneDescription.InstancesObserved = PreviousViolation.Instances;
                    PreviousViolation.CurrentInformation = crimeSceneDescription;
                }
                else
                {
                    //EntryPoint.WriteToConsole($"PLAYER EVENT: ADD CRIME: {CrimeInstance.Name} ByPolice: {crimeSceneDescription.SeenByOfficers} Instances {crimeSceneDescription.InstancesObserved}", 3);
                    if (crimeSceneDescription.SeenByOfficers)
                    {
                        CrimesObserved.Add(new CrimeEvent(CrimeInstance, crimeSceneDescription));
                    }
                    else
                    {
                        CrimesReported.Add(new CrimeEvent(CrimeInstance, crimeSceneDescription));
                    }
                }
                if (crimeSceneDescription.SeenByOfficers && Player.WantedLevel != CrimeInstance.ResultingWantedLevel && isForPlayer)
                {
                    Player.SetWantedLevel(CrimeInstance.ResultingWantedLevel, CrimeInstance.Name, true);
                }
                return crimeSceneDescription;
            }
            return null;
        }
        public void ApplyReportedCrimes()
        {
            if (CrimesReported.Any())
            {
                foreach (CrimeEvent MyCrimes in CrimesReported)
                {
                    CrimeEvent PreviousViolation = CrimesObserved.FirstOrDefault(x => x.AssociatedCrime == MyCrimes.AssociatedCrime);
                    if (PreviousViolation == null)
                    {
                        CrimesObserved.Add(new CrimeEvent(MyCrimes.AssociatedCrime, new CrimeSceneDescription(!Player.IsInVehicle, true, Game.LocalPlayer.Character.Position, true) { InteriorSeen = Player.CurrentLocation.CurrentInterior }));
                    }
                    else if (PreviousViolation.CanAddInstance)
                    {
                        PreviousViolation.AddInstance();
                    }
                }
                CrimeEvent WorstObserved = CrimesObserved.OrderBy(x => x.AssociatedCrime.Priority).FirstOrDefault();
                if (WorstObserved != null)
                {
                    Player.SetWantedLevel(WorstObserved.AssociatedCrime.ResultingWantedLevel, "you are a suspect!", true);
                }
            }
        }
        public void SetCloseChasingCops(int countCloseChasingCops)
        {
            CountCloseVehicleChasingCops = countCloseChasingCops;
        }
        public int InstancesOfCrime(string CrimeID)
        {
            CrimeEvent MyStuff = CrimesObserved.Where(x => x.AssociatedCrime.ID == CrimeID).FirstOrDefault();
            if (MyStuff == null)
                return 0;
            else
                return MyStuff.Instances;
        }
        public int InstancesOfCrime(Crime ToCheck)
        {
            CrimeEvent MyStuff = CrimesObserved.Where(x => x.AssociatedCrime.ID == ToCheck.ID).FirstOrDefault();
            if (MyStuff == null)
                return 0;
            else
                return MyStuff.Instances;
        }
        public bool NearLastWanted(float DistanceTo)
        {
            return LastWantedCenterPosition != Vector3.Zero && Game.LocalPlayer.Character.DistanceTo2D(LastWantedCenterPosition) <= DistanceTo;
        }
        public void OnBecameWanted()
        {
            GameTimeWantedStarted = Game.GameTime;
            GameTimeWantedLevelStarted = Game.GameTime;
            PlaceWantedStarted = Game.LocalPlayer.Character.Position;
            RelationshipGroup.Cop.SetRelationshipWith(RelationshipGroup.Player, Relationship.Hate);
            RelationshipGroup.Player.SetRelationshipWith(RelationshipGroup.Cop, Relationship.Hate);

            RelationshipGroup.SecurityGuard.SetRelationshipWith(RelationshipGroup.Player, Relationship.Hate);
            RelationshipGroup.Player.SetRelationshipWith(RelationshipGroup.SecurityGuard, Relationship.Hate);
        }
        public void OnLostWanted()
        {
            if (!Player.IsDead)
            {
                GameTimeWantedEnded = Game.GameTime;
                if (PlayerSeenDuringWanted)//otherwise leave the reported stuff?, maybe should always reset?
                {
                    Reset();
                }
            }
            GameTimeLastWantedEnded = Game.GameTime;
            DateTimeLastWantedEnded = Time.CurrentDateTime;
            RelationshipGroup.Cop.SetRelationshipWith(RelationshipGroup.Player, Relationship.Neutral);
            RelationshipGroup.Player.SetRelationshipWith(RelationshipGroup.Cop, Relationship.Neutral);

            RelationshipGroup.SecurityGuard.SetRelationshipWith(RelationshipGroup.Player, Relationship.Neutral);
            RelationshipGroup.Player.SetRelationshipWith(RelationshipGroup.SecurityGuard, Relationship.Neutral);
            //EntryPoint.WriteToConsole($"POLICE RESPONSE: Lost Wanted DateTimeLastWantedEnded {DateTimeLastWantedEnded}");
        }
        public void OnWantedLevelIncreased()
        {
            GameTimeWantedLevelStarted = Game.GameTime;
        }
        public string PrintCrimes(bool withInstances)
        {
            string CrimeString = "";

            if (withInstances)
            {
                CrimeString = string.Join(", ", CrimesObserved.Where(x => x.Instances > 0).OrderBy(x => x.AssociatedCrime.Priority).Take(3).Select(x => $"~n~{x.AssociatedCrime.Name} ({x.Instances})"));
            }
            else
            {
                CrimeString = string.Join(", ", CrimesObserved.Where(x => x.Instances > 0).OrderBy(x => x.AssociatedCrime.Priority).Take(3).Select(x => x.AssociatedCrime.Name));
            }
            return CrimeString.Trim();
        }
        public void Reset()
        {
            Player.SetWantedLevel(0, "Police Response Reset", true);
            IsWeaponsFree = false;
            PlayerSeenDuringWanted = false;
            PlayerSeenInVehicleDuringWanted = false;
            PlaceLastReportedCrime = Vector3.Zero;
            PlaceLastReportedCrimeInterior = null;
            PoliceHaveDescription = false;
            CurrentPoliceState = PoliceState.Normal;
            GameTimeWantedLevelStarted = 0;
            GameTimeWantedEnded = 0;
            WantedLevelHasBeenRadioedIn = false;
            CrimesObserved.Clear();
            CrimesReported.Clear();
            TrainStopper.Reset();
            CountCloseVehicleChasingCops = 0;
        }
        private void AssignCops()
        {
            int RespondingPolice = 0;
            if (Player.WantedLevel == 1)
            {
                if (Player.IsBusted)
                {
                    RespondingPolice = 2;
                }
                else
                {
                    RespondingPolice = 2;
                }
            }
            else if (Player.WantedLevel == 2)
            {
                if (Player.IsBusted)
                {
                    RespondingPolice = 2;
                }
                else
                {
                    RespondingPolice = 6;
                }
            }
            else if (Player.WantedLevel == 3)
            {
                if (Player.IsBusted)
                {
                    RespondingPolice = 3;
                }
                else
                {
                    RespondingPolice = 999;
                }
            }
            else if (Player.IsWanted)
            {
                if (Player.IsBusted)
                {
                    RespondingPolice = 4;
                }
                else
                {
                    RespondingPolice = 999;
                }
            }
            int tasked = 0;
            int updated = 0;
            foreach (Cop cop in World.Pedestrians.AllPoliceList.Where(x => x.Pedestrian.Exists()).OrderBy(x => x.DistanceToPlayer))
            {
                if(cop.IsRoadblockSpawned)
                {
                    cop.IsRespondingToWanted = true;
                }
                else if (!cop.IsInVehicle && cop.DistanceToPlayer >= 150f)
                {
                    cop.IsRespondingToWanted = false;
                }
                else if (!cop.IsDead && !cop.IsUnconscious && tasked < RespondingPolice)
                {
                    cop.IsRespondingToWanted = true;
                    tasked++;
                }
                else if (!cop.IsDead && !cop.IsUnconscious && Player.WantedLevel >= 2 && !Player.IsBusted && cop.DistanceToPlayer <= 75f)
                {
                    cop.IsRespondingToWanted = true;
                    tasked++;
                }
                else
                {
                    cop.IsRespondingToWanted = false;
                }
                updated++;
                if (updated > 5)
                {
                    updated = 0;
                    GameFiber.Yield();//TR 05
                }
            }
            GameFiber.Yield();//TR 05
            CurrentRespondingPoliceCount = tasked;
            //EntryPoint.WriteToConsole($"Wanted Active, RespondingPolice {RespondingPolice} Total Tasked {tasked}");  //
        }
        private PoliceState GetPoliceState()
        {
            if (Player.IsBusted)
            {
                return PoliceState.ArrestedWait;
            }
            if (Player.IsNotWanted)
            {
                return PoliceState.Normal;//Default state
            }
            else
            {
                if (Player.WantedLevel <= Settings.SettingsManager.PoliceSettings.UnarmedChaseMaxWantedLevel)// 3)
                {
                    if (LethalForceAuthorized)
                    {
                        return PoliceState.DeadlyChase;
                    }
                    else
                    {
                        return PoliceState.UnarmedChase;
                    }
                }
                else
                {
                    return PoliceState.DeadlyChase;
                }
            }
        }
        private void PoliceStateChanged()
        {
            //EntryPoint.WriteToConsole(string.Format("PoliceState Changed to: {0} Was {1}", CurrentPoliceState, PrevPoliceState));
            GameTimePoliceStateStart = Game.GameTime;
            PrevPoliceState = CurrentPoliceState;
            if (CurrentPoliceState == PoliceState.DeadlyChase)
            {
                GameFiber.Yield();//TR 05
                Player.OnLethalForceAuthorized();
            }
        }
        private void UpdateBlip()
        {
            if (Player.IsWanted)
            {
                if (!LastSeenLocationBlip.Exists() && EntryPoint.ModController.IsRunning & Settings.SettingsManager.UIGeneralSettings.CreatePoliceResponseBlip)
                {
                    LastSeenLocationBlip = new Blip(Player.PlacePoliceLastSeenPlayer, CurrentPoliceRadius)//200f)
                    {
                        Name = "Wanted Center",
                        Color = Color.Red,
                        Alpha = 0.25f
                    };
                    NativeFunction.Natives.BEGIN_TEXT_COMMAND_SET_BLIP_NAME("STRING");
                    NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME("Wanted Center");
                    NativeFunction.Natives.END_TEXT_COMMAND_SET_BLIP_NAME(LastSeenLocationBlip);
                    NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)LastSeenLocationBlip.Handle, true);
                    //NativeFunction.Natives.SET_BLIP_FLASHES((uint)LastSeenLocationBlip.Handle, true);
                    EntryPoint.WriteToConsole($"LAST SEEN LOCATION BLIP CREATED");
                }
                else if (LastSeenLocationBlip.Exists())
                {
                    LastSeenLocationBlip.Position = Player.PlacePoliceLastSeenPlayer;
                }
                if (LastSeenLocationBlip.Exists())
                {
                    if (Player.IsInSearchMode)
                    {
                        LastSeenLocationBlip.Color = Color.Orange;
                        LastSeenLocationBlip.Alpha = (Player.SearchModePercentage + 0.1f) * 0.35f;
                       // LastSeenLocationBlip.Scale = 25f + ((1 - Player.SearchModePercentage) * 175f);
                    }
                    else if (Player.CurrentLocation.IsInside && Player.AnyPoliceKnowInteriorLocation)
                    {
                        LastSeenLocationBlip.Color = Color.Black;
                        LastSeenLocationBlip.Alpha = 0.25f;
                        //LastSeenLocationBlip.Scale = 200f;
                    }
                    else
                    {
                        if (WantedLevelHasBeenRadioedIn)
                        {
                            LastSeenLocationBlip.Color = Color.Red;
                            LastSeenLocationBlip.Alpha = 0.25f;
                            //LastSeenLocationBlip.Scale = 200f;
                        }
                        else
                        {
                            LastSeenLocationBlip.Color = Color.Black;
                            LastSeenLocationBlip.Alpha = 0.25f;// 0.1f;
                            //LastSeenLocationBlip.Scale = 20f;
                        }
                        LastSeenLocationBlip.Scale = CurrentPoliceRadius;
                    }
                }
            }
            else
            {
                if (LastSeenLocationBlip.Exists())
                {
                    LastSeenLocationBlip.Delete();
                }
            }
        }
        private void PoliceKilledUpdate()
        {
            if (!Settings.SettingsManager.PoliceSettings.WantedLevelIncreasesByKillingPolice)
            {
                return;
            }
            int PoliceKilled = InstancesOfCrime("KillingPolice");
            if (PoliceKilled == 0)
            {
                return;
            }
            if (Player.WantedLevel == 9 && Settings.SettingsManager.PoliceSettings.MaxWantedLevel >= 10 && PoliceKilled >= Settings.SettingsManager.PoliceSettings.KillLimit_Wanted10)
            {
                Player.SetWantedLevel(10, "You killed too many cops 10 Stars", true);
                IsWeaponsFree = true;
                Player.OnWeaponsFree();
            }
            else if (Player.WantedLevel == 8 && Settings.SettingsManager.PoliceSettings.MaxWantedLevel >= 9 && PoliceKilled >= Settings.SettingsManager.PoliceSettings.KillLimit_Wanted9)
            {
                Player.SetWantedLevel(9, "You killed too many cops 9 Stars", true);
                IsWeaponsFree = true;
                Player.OnWeaponsFree();
            }
            else if (Player.WantedLevel == 7 && Settings.SettingsManager.PoliceSettings.MaxWantedLevel >= 8 && PoliceKilled >= Settings.SettingsManager.PoliceSettings.KillLimit_Wanted8)
            {
                Player.SetWantedLevel(8, "You killed too many cops 8 Stars", true);
                IsWeaponsFree = true;
                Player.OnWeaponsFree();
            }
            else if (Player.WantedLevel == 6 && Settings.SettingsManager.PoliceSettings.MaxWantedLevel >= 7 && PoliceKilled >= Settings.SettingsManager.PoliceSettings.KillLimit_Wanted7)
            {
                Player.SetWantedLevel(7, "You killed too many cops 7 Stars", true);
                IsWeaponsFree = true;
                Player.OnWeaponsFree();
            }
            else if (Player.WantedLevel == 5 && PoliceKilled >= Settings.SettingsManager.PoliceSettings.KillLimit_Wanted6)
            {
                Player.SetWantedLevel(6, "You killed too many cops 6 Stars", true);
                IsWeaponsFree = true;
                Player.OnWeaponsFree();
            }
            else if (Player.WantedLevel == 4 && PoliceKilled >= Settings.SettingsManager.PoliceSettings.KillLimit_Wanted5)
            {
                Player.SetWantedLevel(5, "You killed too many cops 5 Stars", true);
                IsWeaponsFree = true;
                Player.OnWeaponsFree();
            }
            else if (Player.WantedLevel < 4 && PoliceKilled >= Settings.SettingsManager.PoliceSettings.KillLimit_Wanted4)
            {
                Player.SetWantedLevel(4, "You killed too many cops 4 Stars", true);
                IsWeaponsFree = true;
                Player.OnWeaponsFree();
            }
        }
        public bool IsWithinGracePeriod(Crime crime) => GracePeriodCrimes.Any(x => x.ID == crime.ID);
        public void AddToGracePeriod()
        {
            //GracePeriodCrimes.Clear();
            foreach (Crime crime in CrimesObserved.Where(x => x.AssociatedCrime?.GracePeriod != 0).Select(x => x.AssociatedCrime))
            {
                if (!GracePeriodCrimes.Contains(crime))
                {
                    GracePeriodCrimes.Add(crime);
                }
            }
            //EntryPoint.WriteToConsoleTestLong("AddToGracePeriod");
        }
        private void UpdateGracePeriod()
        {
            if (Player.IsWanted)
            {
                GracePeriodCrimes.Clear();
                return;
            }
            GracePeriodCrimes.RemoveWhere(x => HasBeenNotWantedFor >= x.GracePeriod);
        }
        public void ResetGracePeriod()
        {
            GracePeriodCrimes.Clear();
            //EntryPoint.WriteToConsoleTestLong("ResetGracePeriod");
        }
        private void ResetWanted()
        {
            EntryPoint.WriteToConsole("Cops Did Not Radio In, Resetting Wanted");
            //List<CrimeEvent> PrevCrimesReported = CrimesReported.Copy();

            CrimeEvent worst = CrimesReported.OrderBy(x => x.AssociatedCrime.Priority).FirstOrDefault();
            Crime crime = null;
            CrimeSceneDescription csd = null;
            int instances = 0;
            //EntryPoint.WriteToConsole("Cops Did Not Radio In, Resetting Wanted 2");
            if (worst != null)
            {
                crime = worst.AssociatedCrime;
                //csd = worst.CurrentInformation.Copy();

                csd = new CrimeSceneDescription();

                csd.Speed = worst.CurrentInformation.Speed;
                csd.WeaponSeen = worst.CurrentInformation.WeaponSeen;
                csd.VehicleSeen = worst.CurrentInformation.VehicleSeen;
                csd.SeenOnFoot = worst.CurrentInformation.SeenOnFoot;
                csd.SeenByOfficers = worst.CurrentInformation.SeenByOfficers;
                csd.InstancesObserved = worst.CurrentInformation.InstancesObserved;
                csd.PlaceSeen = worst.CurrentInformation.PlaceSeen;
                csd.HaveDescription = worst.CurrentInformation.HaveDescription;
                csd.InteriorSeen = worst.CurrentInformation.InteriorSeen;
                instances = worst.Instances;
            }
            //EntryPoint.WriteToConsole("Cops Did Not Radio In, Resetting Wanted 3");
            Vector3 prevPlaceLastReportedCrime = PlaceLastReportedCrime;
            Interior prevPlaceLastReportedCrimeInterior = PlaceLastReportedCrimeInterior;
            //EntryPoint.WriteToConsole("Cops Did Not Radio In, Resetting Wanted 4");
            bool policeHaveDescription = PoliceHaveDescription;
            bool reqPolice = Player.Investigation.RequiresPolice;
            bool reqEMS = Player.Investigation.RequiresEMS;
            bool reqFire = Player.Investigation.RequiresFirefighters;
            EntryPoint.WriteToConsole($"prevPlaceLastReportedCrime {prevPlaceLastReportedCrime} policeHaveDescription{policeHaveDescription} reqPolice{reqPolice} reqEMS{reqEMS} reqFire{reqFire}");
            Reset();
            Player.Scanner.Reset();
            if (worst != null && prevPlaceLastReportedCrime != Vector3.Zero)
            {
                CrimesReported.Add(new CrimeEvent(crime, csd) { Instances = instances });
                Player.Investigation.Start(prevPlaceLastReportedCrime, policeHaveDescription, true, reqEMS, reqFire, prevPlaceLastReportedCrimeInterior);
                EntryPoint.WriteToConsole($"STARTING INVESTIGATION {crime.ID}");
            }
        }
        public void RadioInWanted()
        {
            WantedLevelHasBeenRadioedIn = true;
        }
        public void ForceSetWanted(int wantedLevel)
        {
            Player.SetWantedLevel(wantedLevel, "Debug Menu", true);
            WantedLevelHasBeenRadioedIn = true;
            Vector3 CurrentWantedCenter = Player.Position;
            if (CurrentWantedCenter != Vector3.Zero)
            {
                LastWantedCenterPosition = CurrentWantedCenter;
            }
            PlayerSeenDuringCurrentWanted = true;
            PlayerSeenDuringWanted = true;
            if (Player.IsInVehicle)
            {
                PlayerSeenInVehicleDuringWanted = true;
            }
            WantedLevelHasBeenRadioedIn = true;
        }
    }
}